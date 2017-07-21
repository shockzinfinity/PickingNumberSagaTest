using MassTransit.EntityFrameworkIntegration;

namespace Kukil.Service2
{
	public class LoadOrderMap : SagaClassMapping<LoadOrder>
	{
		public LoadOrderMap()
		{
			Property(l => l.DocumentKey);
			Property(l => l.ReceivedAt).IsOptional();
			Property(l => l.RegisteredAt).IsOptional();
			Property(l => l.RowVersion).IsRowVersion();
		}
	}
}