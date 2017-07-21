using Autofac;
using Kukil.PickingNumberService.MessageContracts;
using MassTransit;
using MassTransit.NLogIntegration.Logging;
using Topshelf;
using Topshelf.Autofac;

namespace Kukil.PickingNumberService.WindowsService
{
	internal class Program
	{
		private static void Main(string[] args)
		{
			NLogLogger.Use();

			var builder = new ContainerBuilder();
			builder.RegisterType<LoadOrderPickingNumberService>();
			builder.RegisterType<LoadOrderPickingNumberServiceConsumer>();
			builder.Register(context =>
			{
				IBusControl control = BusConfigurator.ConfigureBus(Constants.RabbitMqUri, Constants.UserName, Constants.Password, (cfg, host) =>
				{
					cfg.ReceiveEndpoint(host, Constants.LoadOrderRequestServiceQueue, e =>
					{
						e.PurgeOnStartup = true;
						e.UseInMemoryOutbox();
						e.LoadFrom(context);
						//e.Consumer<LoadOrderPickingNumberServiceConsumer>(_consumer);
					});
				});

				return control;
			})
			.SingleInstance()
			.As<IBusControl>()
			.As<IBus>();

			var container = builder.Build();

			HostFactory.Run(x =>
			{
				x.UseAutofacContainer(container);
				x.Service<LoadOrderPickingNumberService>(s =>
				{
					s.ConstructUsingAutofacContainer();
					s.WhenStarted(service => { service.Start(); });
					s.WhenStopped(service => { service.Stop(); });
				});

				x.UseNLog();
				x.SetDescription("출고지시, 상차지시, 상차, 출고확정을 위한 채번 서비스");
				x.SetDisplayName("LoadOrder Picking number service for kukil");
				x.SetServiceName("LoadOrderPickingNumberService");

				//x.RunAsLocalSystem(); // for production
			});
		}
	}
}