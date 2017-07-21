using System;

namespace Kukil.PickingNumberService.MessageContracts
{
	public interface ILoadOrderResponse
	{
		int DocumentKey { get; }
		string GeneratedLoadOrderNumber { get; }
		DateTime Timestamp { get; }
	}
}