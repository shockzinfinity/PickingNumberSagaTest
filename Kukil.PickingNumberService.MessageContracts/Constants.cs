namespace Kukil.PickingNumberService.MessageContracts
{
	public class Constants
	{
		public const string RabbitMqUri = "rabbitmq://sapdev1.semubot.com/kukil/";
		public const string UserName = "kukil";
		public const string Password = "kukil";
		public const string LoadOrderRequestServiceQueue = "loadOrder.request.service";
		public const string LoadOrderSagaServiceQueue = "loadOrder.saga.service";
		public const string LoadOrderRegisteredQueue = "loadOrder.registered.service";
	}
}