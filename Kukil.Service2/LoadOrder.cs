using Automatonymous;
using System;

namespace Kukil.Service2
{
	public class LoadOrder : SagaStateMachineInstance
	{
		public LoadOrder(Guid correlationId)
		{
			CorrelationId = correlationId;
		}

		protected LoadOrder() { }

		public Guid CorrelationId { get; set; }
		public string CurrentStatus { get; set; }
		public string DocumentKey { get; set; }
		public DateTime? ReceivedAt { get; set; }
		public DateTime? RegisteredAt { get; set; }
		public byte[] RowVersion { get; set; }
	}
}