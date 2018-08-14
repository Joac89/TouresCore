using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Common;
using System.Data.SqlClient;
using TouresCommon;

namespace TouresDataAccess
{
	public class SqlAccess : IDataAccess
	{
		private SqlCommand command;
		private SqlConnection connection;
		private SqlDataReader reader;
		private List<SqlParameter> parameters;
		private string connString = "";

		public ResponseState State { get; set; }
		public SqlAccess(string ConnectionString)
		{
			connString = ConnectionString;
		}

		public bool Open()
		{
			try
			{
				connection = new SqlConnection(connString);
				connection.Open();
				command = new SqlCommand()
				{
					Connection = connection
				};

				return true;
			}
			catch (Exception ex)
			{
				State.Code = ResponseCode.ConnectionDbError;
				State.Message = $"Error al conectar con  base de datos: {ex.Message}";

				return false;
			}
		}
		public void Close()
		{
			if (connection.State == ConnectionState.Open)
			{
				if (!reader.IsClosed) reader.Close();
				command.Dispose();
				connection.Close();
				connection.Dispose();
			}
		}

		public DbParameterCollection SaveOutput(string storeProcedure)
		{
			SqlParameterCollection parameters = null;
			var success = Open();

			if (success)
			{
				command.CommandType = CommandType.StoredProcedure;
				command.CommandText = storeProcedure;
				command.ExecuteNonQuery();

				State.Code = ResponseCode.Ok;
				parameters = command.Parameters;
			}

			return parameters;
		}
		public bool Save(string storeProcedure)
		{
			var result = false;
			var success = Open();

			if (success)
			{
				command.CommandType = CommandType.StoredProcedure;
				command.CommandText = storeProcedure;
				command.ExecuteNonQuery();

				State.Code = ResponseCode.Ok;
				result = true;
			}

			return result;
		}
		public DbDataReader Read(string storeProcedure)
		{
			DbDataReader resultReader = null;
			var success = Open();

			if (success)
			{
				command.CommandType = CommandType.StoredProcedure;
				command.CommandText = storeProcedure;
				reader = command.ExecuteReader();

				State.Code = ResponseCode.Ok;
				resultReader = reader;
			}

			return resultReader;
		}

		public bool Parameter<T>(string name, int type, T value)
		{
			var result = false;
			try
			{
				command.Parameters.Add($"@{name}", (SqlDbType)type).Value = value;
				result = true;
			}
			catch (Exception ex)
			{
				State.Code = ResponseCode.InternalError;
				State.Message = $"Error al agregar parámetro: {ex.Message}";
			}
			return result;
		}
		public bool Parameter<T>(string name, int type, T value, int size)
		{
			var result = false;
			try
			{
				command.Parameters.Add($"@{name}", (SqlDbType)type, size).Value = value;
				result = true;
			}
			catch (Exception ex)
			{
				State.Code = ResponseCode.InternalError;
				State.Message = $"Error al agregar parámetro: {ex.Message}";
			}
			return result;
		}
		public bool Parameter<T>(string name, int type, ParameterDirection direction)
		{
			var result = false;
			try
			{
				command.Parameters.Add($"@{name}", (SqlDbType)type).Direction = direction;
				result = true;
			}
			catch (Exception ex)
			{
				State.Code = ResponseCode.InternalError;
				State.Message = $"Error al agregar parámetro: {ex.Message}";
			}
			return result;
		}
	}
}
