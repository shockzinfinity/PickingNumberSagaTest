using Autofac;
using Kukil.PickingNumberService.MessageContracts;
using MassTransit;
using MassTransit.EntityFrameworkIntegration;
using MassTransit.EntityFrameworkIntegration.Saga;
using MassTransit.NLogIntegration.Logging;
using MassTransit.Saga;
using Topshelf;
using Topshelf.Autofac;

namespace Kukil.Service2
{
	internal class Program
	{
		private static void Main(string[] args)
		{
			NLogLogger.Use();

			var builder = new ContainerBuilder();

			builder.Register(c => new PickingNumberDbContext(SagaDbContextFactoryProvider.ConnectionString)).As<PickingNumberDbContext>().SingleInstance();
			builder.RegisterType<LoadOrderRepository>().As<ILoadOrderRepository>().SingleInstance();
			builder.RegisterType<ProcessLoadOrderConsumer>();
			builder.RegisterType<LoadOrderService>();
			builder.Register(c => new EntityFrameworkSagaRepository<LoadOrder>(() => new SagaDbContext<LoadOrder, LoadOrderMap>(SagaDbContextFactoryProvider.ConnectionString), optimistic: true))
				.As<ISagaRepository<LoadOrder>>()
				.SingleInstance();
			builder.RegisterStateMachineSagas(typeof(LoadOrderSaga).Assembly);
			builder.Register(context =>
			{
				IBusControl busControl = BusConfigurator.ConfigureBus(Constants.RabbitMqUri, Constants.UserName, Constants.Password, (config, host) =>
				{
					config.ReceiveEndpoint(host, Constants.LoadOrderSagaServiceQueue, e =>
					{
						e.PurgeOnStartup = true;
						e.UseInMemoryOutbox();
						e.LoadStateMachineSagas(context);
					});

					config.ReceiveEndpoint(host, Constants.LoadOrderRegisteredQueue, e =>
					{
						e.PurgeOnStartup = true;
						e.LoadFrom(context);
					});
				});

				return busControl;
			})
			.SingleInstance()
			.As<IBusControl>()
			.As<IBus>();

			var container = builder.Build();

			HostFactory.Run(h =>
			{
				h.UseAutofacContainer(container);
				h.Service<LoadOrderService>(service =>
				{
					service.ConstructUsingAutofacContainer();
					service.WhenStarted(l => l.Start());
					service.WhenStopped(l => l.Stop());
				});

				h.UseNLog();
				h.SetDescription("출고지시, 상차지시, 상차, 출고확정을 위한 채번 서비스");
				h.SetDisplayName("LoadOrder Picking number service for kukil");
				h.SetServiceName("LoadOrderService");
				//h.RunAsLocalSystem(); // for production
			});
		}
	}
}