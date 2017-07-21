using MassTransit;
using MassTransit.Util;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Topshelf.Logging;

namespace Kukil.Service2
{
	internal class LoadOrderService
	{
		private readonly LogWriter _log = HostLogger.Get<LoadOrderService>();
		private readonly IBusControl _busControl;

		public LoadOrderService(IBusControl busControl)
		{
			if (busControl == null)
				throw new ArgumentNullException(nameof(busControl));

			this._busControl = busControl;
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
}
