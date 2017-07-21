using Kukil.PickingNumberService.MessageContracts;
using MassTransit;
using MassTransit.Logging;
using MassTransit.Util;
using System;
using System.Threading.Tasks;
using Topshelf.Logging;

namespace Kukil.PickingNumberService.WindowsService
{
	internal class LoadOrderPickingNumberService
	{
		private readonly LogWriter _log = HostLogger.Get<LoadOrderPickingNumberService>();
		private readonly IBusControl _busControl;

		public LoadOrderPickingNumberService(IBusControl bus)
		{
			_busControl = bus;
		}

		public bool Start()
		{
			_log.Info("Starting bus...");

			TaskUtil.Await(() => _busControl.StartAsync());

			return true;
		}

		public bool Stop()
		{
			_log.Info("Stopping bus...");

			_busControl?.Stop();

			return true;
		}
	}

	internal class LoadOrderPickingNumberServiceConsumer : IConsumer<ILoadOrderRequest>
	{
		private readonly ILog _log = Logger.Get<LoadOrderPickingNumberServiceConsumer>();

		public async Task Consume(ConsumeContext<ILoadOrderRequest> context)
		{
			_log.InfoFormat("Returning document key for {0} at ({1})", context.Message.DocumentKey, context.Message.Timestamp);

			//Thread.Sleep(3 * 1000);

			context.Respond(new LoadOrderResponse
			{
				DocumentKey = context.Message.DocumentKey,
				Timestamp = DateTime.UtcNow,
				GeneratedLoadOrderNumber = "4348384939"
			});
		}

		private class LoadOrderResponse : ILoadOrderResponse
		{
			public int DocumentKey { get; set; }
			public string GeneratedLoadOrderNumber { get; set; }
			public DateTime Timestamp { get; set; }
		}
	}
}