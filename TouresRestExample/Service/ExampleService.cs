using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Threading.Tasks;
using TouresCommon;
using TouresDataAccess;
using TouresRestExample.Model;

namespace TouresRestExample.Dao
{
	public class ExampleService
	{
		private string connString = "";
		public ExampleService(string ConnectionString)
		{
			connString = ConnectionString;
		}

		public async Task<ResponseBase<bool>> Create(ExampleModel data)
		{
			IRepository<SqlParameterCollection> repository = new SqlServerRepository(connString);
			var response = new ResponseBase<bool>() { Data = true, Message = "Datos registrados" };

			repository.Parameters.Add("@name", SqlDbType.NVarChar, 100).Value = data.Name;
			repository.Parameters.Add("@date", SqlDbType.DateTime).Value = data.Date;

			repository.SaveChanges("usp_toures_create");

			if (repository.Status.Code != Status.Ok)
			{
				response.Data = false;
				response.Message = repository.Status.Message;
			}
			response.Code = repository.Status.Code;

			return await Task.Run(() => response);
		}

		public async Task<ResponseBase<List<ExampleResponse>>> Read(long id)
		{
			IRepository<SqlParameterCollection> repository = new SqlServerRepository(connString);
			var lst = new List<ExampleResponse>();
			var response = new ResponseBase<List<ExampleResponse>>() { Message = "Datos consultados" };

			repository.Parameters.Add("@id", SqlDbType.BigInt).Value = id;

			var result = repository.Get("usp_toures_read");
			if (repository.Status.Code == Status.Ok)
			{
				var x = 0;
				foreach (var item in result)
				{
					var example = new ExampleResponse()
					{
						Id = (long)result[x]["Id"],
						Name = (string)result[x]["Name"],
						Date = (DateTime)result[x]["Date"]
					};
					lst.Add(example);

					x += 1;
				}
				response.Data = lst;
			}
			else
			{
				response.Message = repository.Status.Message;
			}
			response.Code = repository.Status.Code;

			return await Task.Run(() => response);
		}

		public async Task<ResponseBase<List<ExampleResponse>>> All()
		{
			IRepository<SqlParameterCollection> repository = new SqlServerRepository(connString);
			var lst = new List<ExampleResponse>();
			var response = new ResponseBase<List<ExampleResponse>>() { Message = "Datos consultados" };

			var result = repository.Get("usp_toures_all");
			if (repository.Status.Code == Status.Ok)
			{
				var x = 0;
				foreach (var item in result)
				{
					var example = new ExampleResponse()
					{
						Id = (long)result[x]["Id"],
						Name = (string)result[x]["Name"],
						Date = (DateTime)result[x]["Date"]
					};
					lst.Add(example);

					x += 1;
				}
				response.Data = lst;
			}
			else
			{
				response.Message = repository.Status.Message;
			}
			response.Code = repository.Status.Code;

			return await Task.Run(() => response);
		}


		public async Task<ResponseBase<long>> Update(ExampleModel data)
		{
			IRepository<SqlParameterCollection> repository = new SqlServerRepository(connString);
			var response = new ResponseBase<long>() { Message = "Datos actualizados" };

			repository.Parameters.Add("@id", SqlDbType.BigInt).Value = data.Id;
			repository.Parameters.Add("@name", SqlDbType.NVarChar, 100).Value = data.Name;
			repository.Parameters.Add("@date", SqlDbType.DateTime).Value = data.Date;

			repository.SaveChanges("usp_toures_update");

			if (repository.Status.Code != Status.Ok)
			{
				response.Data = 0;
				response.Message = repository.Status.Message;
			}
			else
			{
				response.Data = data.Id;
			}
			response.Code = repository.Status.Code;

			return await Task.Run(() => response);
		}

		public async Task<ResponseBase<bool>> Delete(long id)
		{
			IRepository<SqlParameterCollection> repository = new SqlServerRepository(connString);
			var response = new ResponseBase<bool>() { Data = true, Message = "Datos eliminados" };

			repository.Parameters.Add("@id", SqlDbType.BigInt).Value = id;

			repository.SaveChanges("usp_toures_delete");

			if (repository.Status.Code != Status.Ok)
			{
				response.Data = false;
				response.Message = repository.Status.Message;
			}
			response.Code = repository.Status.Code;

			return await Task.Run(() => response);
		}
	}
}
