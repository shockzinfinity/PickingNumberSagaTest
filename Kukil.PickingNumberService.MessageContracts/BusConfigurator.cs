using MassTransit;
using MassTransit.RabbitMqTransport;
using System;

namespace Kukil.PickingNumberService.MessageContracts
{
	public class BusConfigurator
	{
		public static IBusControl ConfigureBus(string busUri, string username, string password, Action<IRabbitMqBusFactoryConfigurator, IRabbitMqHost> registerationAction = null)
		{
			return Bus.Factory.CreateUsingRabbitMq(config =>
			{
				var host = config.Host(new Uri(busUri), hst =>
				{
					hst.Username(username);
					hst.Password(password);
				});

				registerationAction?.Invoke(config, host);
			});
		}
	}
}