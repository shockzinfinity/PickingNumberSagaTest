using Kukil.PickingNumberService.MessageContracts;
using MassTransit;
using System;
using System.Threading.Tasks;

namespace Kukil.Service2
{
	public class ProcessLoadOrderConsumer : IConsumer<INeedToProcessLoadOrder>
	{
		private readonly ILoadOrderRepository _repository;

		public ProcessLoadOrderConsumer(ILoadOrderRepository repository)
		{
			if (repository == null)
				throw new ArgumentNullException(nameof(repository));

			_repository = repository;
		}

		public Task Consume(ConsumeContext<INeedToProcessLoadOrder> context)
		{
			INeedToProcessLoadOrder toProcess = context.Message;

			_repository.GenerateLoadOrderNumber(toProcess.DocumentKey);

			// 다시 consumer 측에서 RegisteredLoadOrder 이벤트 publish
			context.Publish<IRegisteredLoadOrder>(new
			{
				CorrelationId = toProcess.CorrelationId,
				DocumentKey = toProcess.DocumentKey,
				Timestamp = DateTime.UtcNow,
				GeneratedLoadOrderNumber = ""
			});

			return Task.FromResult(context.Message);
		}
	}
}