using System;
using System.Data.SqlClient;

namespace Kukil.Service2
{
	public class SagaDbContextFactoryProvider
	{
		private static readonly string[] _possibleLocalDbConnectionStrings = new[]
		{
			 @"Data Source=(LocalDb)\MSSQLLocalDB;User Id=sa;Password=sap2017&&;Initial Catalog=kukil_picking_number_test;",
			 //@"Data Source=(LocalDb)\ProjectsV12;Integrated Security=True;Initial Catalog=kukil_picking_number_test;",
			 //@"Data Source=(LocalDb)\v11.0;Integrated Security=True;Initial Catalog=kukil_picking_number_test;",
		};

		private static readonly Lazy<string> _connectionString = new Lazy<string>(GetLocalDbConnectionString);
		public static string ConnectionString => _connectionString.Value;

		public static string GetLocalDbConnectionString()
		{
			foreach (var connectionString in _possibleLocalDbConnectionStrings)
			{
				try
				{
					using (var connection = new SqlConnection(connectionString))
					{
						return connectionString;
					}
				}
				catch (Exception)
				{
				}
			}

			throw new InvalidOperationException("Could not connect to any of the LocalDB databases");
		}
	}
}