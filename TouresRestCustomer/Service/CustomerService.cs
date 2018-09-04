using Oracle.ManagedDataAccess.Client;
using System;
using System.Data;
using System.Data.SqlClient;
using System.Threading.Tasks;
using TouresCommon;
using TouresDataAccess;
using TouresRestCustomer.Model;

namespace TouresRestCustomer.Service
{
	public class CustomerService
	{
		private string connString = "";
		public CustomerService(string ConnectionString)
		{
			connString = ConnectionString;
		}

		public async Task<ResponseBase<CustomerResponse>> GetCustomer(CustomerModel data)
		{
            IRepository<OracleParameterCollection> repository = new OracleRepository(connString, "C_DATASET");
            var response = new ResponseBase<CustomerResponse>();
            var user = new CustomerResponse();

            repository.Parameters.Add("P_DOCNUMBER", OracleDbType.Varchar2, 200).Value = data.DocNumber;
            repository.Parameters.Add("C_DATASET", OracleDbType.RefCursor).Direction = ParameterDirection.Output;

            var result = repository.Get("PKG_B2C_CUSTOMER.B2C_CUSTOMER_SELECT");
            if (repository.Status.Code == Status.Ok)
            {
                
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
                    user.DOCNUMBER = item["DOCNUMBER"].ToString();
                    user.USERNAME = item["USERNAME"].ToString();
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
