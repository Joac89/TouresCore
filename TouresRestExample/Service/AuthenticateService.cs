using System;
using System.Data;
using System.Data.SqlClient;
using System.Threading.Tasks;
using TouresAuthenticate.Model;
using TouresCommon;
using TouresCommon.Model;
using TouresDataAccess;

namespace TouresRestExample.Service
{
	public class AuthenticateService
	{
		private string connString = "";
		public AuthenticateService(string ConnectionString)
		{
			connString = ConnectionString;
		}

		public async Task<ResponseBase<AuthenticateResponse>> Authenticate(AuthenticateModel data)
		{
			IRepository<SqlParameterCollection> repository = new SqlServerRepository(connString);
			var response = new ResponseBase<AuthenticateResponse>();
			var user = new AuthenticateResponse();

			repository.Parameters.Add("@username", SqlDbType.NVarChar, 50).Value = data.UserName;
			repository.Parameters.Add("@password", SqlDbType.NVarChar, 200).Value = data.Password;

			var result = repository.Get("usp_toures_authenticate");
			if (repository.Status.Code == Status.Ok)
			{
				foreach (var item in result)
				{
					//user.Id = (long)result[0]["IdUser"];
					//user.Names = (string)result[0]["Names"];
					//user.Surnames = (string)result[0]["Surnames"];
					//user.BirthDate = (DateTime)result[0]["BirthDate"];
					//user.Age = (int)result[0]["Age"];					
				}
				response.Data = user;
			}
			else
			{
				response.Message = repository.Status.Message;
			}
			response.Code = repository.Status.Code;

			return await Task.Run(() => response);
		}
	}
}
