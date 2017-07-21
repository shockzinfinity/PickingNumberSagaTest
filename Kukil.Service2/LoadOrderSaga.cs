using Automatonymous;
using Kukil.PickingNumberService.MessageContracts;
using System;

namespace Kukil.Service2
{
	public class LoadOrderSaga : MassTransitStateMachine<LoadOrder>
	{
		public State Processing { get; private set; }

		public Event<IRequestLoadOrder> RequestLoadOrder { get; private set; } // 채번 요청 (진입포인트)
		public Event<IRegisteredLoadOrder> RegisteredLoadOrder { get; private set; } // 채번 프로세싱 종료 이후에 발생하도록...
		public Event<INeedToProcessLoadOrder> ToProcessLoadOrder { get; private set; }

		public LoadOrderSaga()
		{
			InstanceState(s => s.CurrentStatus);

			Event(() => RequestLoadOrder, x => x.CorrelateBy(request => request.DocumentKey, context => context.Message.DocumentKey).SelectId(context => Guid.NewGuid()));
			Event(() => RegisteredLoadOrder, x => x.CorrelateById(context => context.Message.CorrelationId));
			Event(() => ToProcessLoadOrder, x => x.CorrelateById(context => context.Message.CorrelationId));

			Initially(
				When(RequestLoadOrder)
					.Then(HandleRequestLoadOrder)
					.ThenAsync(context => Console.Out.WriteLineAsync($"Request LoadOrder number for DocumentKey({context.Data.DocumentKey}. // request id is {context.Instance.CorrelationId})"))
					.TransitionTo(Processing)
					.Publish(context => new NeedToProcessLoadOrder(context.Instance))
				);

			During(Processing,
				When(RegisteredLoadOrder)
					.Then(context =>
					{
						// 저장 된 이후에 발생하는 이벤트의 결과
					})
					.ThenAsync(context => Console.Out.WriteLineAsync($"Document Key: '{context.Data.DocumentKey}' is generated to '{context.Data.GeneratedLoadOrderNumber}'"))
					// publish to client with generated number
					.Finalize(),
				When(RequestLoadOrder)
					.Then(HandleRequestLoadOrder)
					.ThenAsync(context => Console.Out.WriteLineAsync($"Request LoadOrder number for DocumentKey({context.Data.DocumentKey}. // request id is {context.Instance.CorrelationId})"))
				);

			SetCompletedWhenFinalized();
		}

		private static void HandleRequestLoadOrder(BehaviorContext<LoadOrder, IRequestLoadOrder> context)
		{
			context.Instance.ReceivedAt = DateTime.UtcNow;
			context.Instance.DocumentKey = context.Data.DocumentKey;
		}

		private class NeedToProcessLoadOrder : INeedToProcessLoadOrder
		{
			private readonly LoadOrder _instance;

			public NeedToProcessLoadOrder(LoadOrder instance)
			{
				_instance = instance;
			}

			public Guid CorrelationId => _instance.CorrelationId;
			public string DocumentKey => _instance.DocumentKey;
		}
	}
}