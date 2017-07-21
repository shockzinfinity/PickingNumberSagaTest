using System;

namespace Kukil.PickingNumberService.MessageContracts
{
	public interface ILoadOrderRequest
	{
		int DocumentKey { get; }
		DateTime Timestamp { get; }
	}
}