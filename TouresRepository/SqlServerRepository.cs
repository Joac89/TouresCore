using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using TouresCommon;

namespace TouresRepository
{
	public class SqlServerRepository : IRepository<SqlParameterCollection>
	{
		private SqlCommand command;
		private SqlConnection connection;
		private SqlDataReader reader;
		private string connectionString = "";

		public SqlParameterCollection Parameters { get; set; }
		public StatusResponse Status { get; set; } = new StatusResponse();

		public SqlServerRepository(string ConnectionString)
		{
			connectionString = ConnectionString;
			command = new SqlCommand()
			{
				CommandType = System.Data.CommandType.StoredProcedure
			};
			Parameters = command.Parameters;
		}

		public void SaveChanges(string storeProcedure)
		{
			var open = OpenConnection();

			if (open)
			{
				command.CommandText = storeProcedure;			
				try
				{
					command.ExecuteNonQuery();
					CloseConnection();

					Status.Code = TouresCommon.Status.Ok;
				}
				catch (Exception ex)
				{
					Status.Code = TouresCommon.Status.InternalError;
					Status.Message = $"Error al transportar datos a la base de datos: {ex.Message}";
				}
			}
		}
		public List<IDictionary<string, object>> Get(string storeProcedure)
		{
			var list = new List<IDictionary<string, object>>();
			var open = OpenConnection();

			if (open)
			{
				command.CommandText = storeProcedure;
				try
				{
					reader = command.ExecuteReader();
					while (reader.Read())
					{
						var cols = reader.FieldCount;
						var dic = new Dictionary<string, object>();

						for (var i = 0; i < cols; ++i)
						{
							dic.Add(reader.GetName(i), reader[i]);
						}

						list.Add(dic);
					}

					CloseConnection();
					Status.Code = TouresCommon.Status.Ok;
				}
				catch (Exception ex)
				{
					Status.Code = TouresCommon.Status.InternalError;
					Status.Message = $"Error al consultar datos a la base de datos: {ex.Message}";
				}
			}

			return list;
		}

		private bool OpenConnection()
		{
			try
			{
				connection = new SqlConnection(connectionString);
				connection.Open();
				command.Connection = connection;

				return true;
			}
			catch (Exception ex)
			{
				Status.Code = TouresCommon.Status.ConnectionDbError;
				Status.Message = $"Error al conectarse con base de datos: {ex.Message}";

				return false;
			}
		}
		private void CloseConnection()
		{
			if (connection.State == System.Data.ConnectionState.Open)
			{
				if (reader != null && !reader.IsClosed) reader.Close();
				connection.Close();
				connection.Dispose();
			}
		}
	}
}
