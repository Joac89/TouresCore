using Oracle.ManagedDataAccess.Client;
using System.Data;
using System.Threading.Tasks;
using TouresCommon;
using TouresCommon.Model;

namespace TouresDataAccess.Service
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
			IRepository<OracleParameterCollection> repository = new OracleRepository(connString, "C_DATASET");
			var response = new ResponseBase<AuthenticateResponse>();
			var user = new AuthenticateResponse();

			repository.Parameters.Add("P_USERNAME", OracleDbType.Int64).Value = data.UserName;
			repository.Parameters.Add("P_CONTRASENA", OracleDbType.Varchar2, 200).Value = data.Password;
			repository.Parameters.Add("C_DATASET", OracleDbType.RefCursor).Direction = ParameterDirection.Output;

			var result = repository.Get("PKG_B2C_CUSTOMER.B2C_CUSTOMER_SELECT_AUTENTICAR");
			if (repository.Status.Code == Status.Ok)
			{
				var r = result[0]["CUSTID"];

				foreach (var item in result)
				{
					user.CUSTID = long.Parse(item["CUSTID"].ToString());
					user.FNAME = item["FNAME"].ToString();
					user.LNAME = item["LNAME"].ToString();
					user.PHONENUMBER = item["PHONENUMBER"].ToString();
					user.EMAIL = item["EMAIL"].ToString();
					user.PASSWORD = item["PASSWORD"].ToString();
					user.CREDITCARDTYPE = item["CREDITCARDTYPE"].ToString();
					user.CREDITCARDNUMBER = item["CREDITCARDNUMBER"].ToString();
					user.STATUS = item["STATUS"].ToString();
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
