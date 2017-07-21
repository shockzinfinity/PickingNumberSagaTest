using System.Data.Entity;

namespace Kukil.Service2
{
	public partial class PickingNumberDbContext : DbContext
	{
		public PickingNumberDbContext(string connectionString) : base(connectionString)
		{
		}

		public virtual DbSet<PickingNumber> PickingNumber { get; set; }

		protected override void OnModelCreating(DbModelBuilder modelBuilder)
		{
			modelBuilder.Entity<PickingNumber>()
				.Property(e => e.RowVersion)
				.IsFixedLength();
		}
	}
}