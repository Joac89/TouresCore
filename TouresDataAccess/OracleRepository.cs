using Oracle.ManagedDataAccess.Client;
using Oracle.ManagedDataAccess.Types;
using System;
using System.Collections.Generic;
using TouresCommon;

namespace TouresDataAccess
{
	public class OracleRepository: IRepository<OracleParameterCollection>
    {
		private OracleCommand command;
		private OracleConnection connection;
		private OracleDataReader reader;
		private string connectionString = "";
        private string outputParam = "";

		public OracleParameterCollection Parameters { get; set; }
		public StatusResponse Status { get; set; } = new StatusResponse();

		public OracleRepository(string ConnectionString, string OutputParameter)
		{
            outputParam = OutputParameter;
			connectionString = ConnectionString;
			command = new OracleCommand()
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
					command.ExecuteNonQuery();

                    reader = ((OracleRefCursor)command.Parameters[outputParam].Value).GetDataReader();

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
				connection = new OracleConnection(connectionString); 
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
