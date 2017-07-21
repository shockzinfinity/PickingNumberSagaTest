namespace Kukil.PickingNumberService.MessageContracts
{
	public interface ILoadOrderRepository
	{
		string GenerateLoadOrderNumber(string documentKey);
	}
}