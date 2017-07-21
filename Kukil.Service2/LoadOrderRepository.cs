using Kukil.PickingNumberService.MessageContracts;
using System;
using System.Threading;

namespace Kukil.Service2
{
	public class LoadOrderRepository : ILoadOrderRepository
	{
		private readonly PickingNumberDbContext _context;

		public LoadOrderRepository(PickingNumberDbContext context)
		{
			if (context == null)
				throw new ArgumentNullException(nameof(context));

			_context = context;
		}

		public string GenerateLoadOrderNumber(string documentKey)
		{
			// consumer 측에서 이 repository 메서드를 호출

			Console.Out.WriteLineAsync($"Document Key: {documentKey} is generating...");

			// TODO: DB에서 문서타입에 맞는 키값 조회 -> 증가 -> DB Save -> publish with generated number
			Thread.Sleep(3 * 1000);

			Console.Out.WriteLineAsync($"Document Key: {documentKey} is generated...");

			return string.Empty;
		}
	}
}