using System;

namespace Kukil.PickingNumberService.MessageContracts
{
	public interface IRequestLoadOrder
	{
		string DocumentKey { get; }
		DateTime Timestamp { get; }
	}

	public interface IRegisteredLoadOrder
	{
		Guid CorrelationId { get; }
		string DocumentKey { get; }
		DateTime Timestamp { get; }
		string GeneratedLoadOrderNumber { get; }
	}

	public interface INeedToProcessLoadOrder
	{
		Guid CorrelationId { get; }
		string DocumentKey { get; }
	}
}