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

		public async Task<ResponseBase<Customer>> GetCustomer(CustomerModel data)
		{
            IRepository<OracleParameterCollection> repository = new OracleRepository(connString, "C_DATASET");
            var response = new ResponseBase<Customer>();
            var user = new Customer();

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

        public async Task<ResponseBase<Boolean>> InsertCustomer(Customer data)
        {
            IRepository<OracleParameterCollection> repository = new OracleRepository(connString, "P_CUSTID");
            var response = new ResponseBase<Boolean>();

           //repository.Parameters.Add("P_CUSTID", OracleDbType.Int64).Value = data.CUSTID;
            repository.Parameters.Add("P_FNAME", OracleDbType.Varchar2, 200).Value = data.FNAME;
            repository.Parameters.Add("P_LNAME", OracleDbType.Varchar2, 200).Value = data.LNAME;
            repository.Parameters.Add("P_PHONENUMBER", OracleDbType.Varchar2, 200).Value = data.PHONENUMBER;
            repository.Parameters.Add("P_EMAIL", OracleDbType.Varchar2, 200).Value = data.EMAIL;
            repository.Parameters.Add("P_PASSWORD", OracleDbType.Varchar2, 200).Value = data.PASSWORD;
            repository.Parameters.Add("P_CREDITCARDTYPE", OracleDbType.Varchar2, 200).Value = data.CREDITCARDTYPE;
            repository.Parameters.Add("P_CREDITCARDNUMBER", OracleDbType.Varchar2, 200).Value = data.CREDITCARDNUMBER;
            repository.Parameters.Add("P_STATUS", OracleDbType.Varchar2, 200).Value = data.STATUS;            
            repository.Parameters.Add("P_DOCNUMBER", OracleDbType.Int64).Value = data.DOCNUMBER;
            repository.Parameters.Add("P_USERNAME", OracleDbType.Varchar2, 200).Value = data.USERNAME;            
            repository.Parameters.Add("P_CUSTID", OracleDbType.Int64).Direction = ParameterDirection.Output;

            repository.SaveChanges("PKG_B2C_ORDERS.B2C_CUSTOMER_INSERT");
            if (repository.Status.Code == Status.Ok)
            {
                response.Data = true;
                response.Message = "Customer creado correctamente";
            }
            else
            {
                response.Data = false;
                response.Message = repository.Status.Message;
            }
            response.Code = repository.Status.Code;

            return await Task.Run(() => response);
        }

        public async Task<ResponseBase<Boolean>> UpdateCustomer(Customer data)
        {
            IRepository<OracleParameterCollection> repository = new OracleRepository(connString, "P_CUSTID");
            var response = new ResponseBase<Boolean>();

            repository.Parameters.Add("P_CUSTID", OracleDbType.Int64).Value = data.CUSTID;
            repository.Parameters.Add("P_FNAME", OracleDbType.Varchar2, 200).Value = data.FNAME;
            repository.Parameters.Add("P_LNAME", OracleDbType.Varchar2, 200).Value = data.LNAME;
            repository.Parameters.Add("P_PHONENUMBER", OracleDbType.Varchar2, 200).Value = data.PHONENUMBER;
            repository.Parameters.Add("P_EMAIL", OracleDbType.Varchar2, 200).Value = data.EMAIL;
            repository.Parameters.Add("P_PASSWORD", OracleDbType.Varchar2, 200).Value = data.PASSWORD;
            repository.Parameters.Add("P_CREDITCARDTYPE", OracleDbType.Varchar2, 200).Value = data.CREDITCARDTYPE;
            repository.Parameters.Add("P_CREDITCARDNUMBER", OracleDbType.Varchar2, 200).Value = data.CREDITCARDNUMBER;
            repository.Parameters.Add("P_STATUS", OracleDbType.Varchar2, 200).Value = data.STATUS;
            repository.Parameters.Add("P_DOCNUMBER", OracleDbType.Int64).Value = data.DOCNUMBER;
            repository.Parameters.Add("P_USERNAME", OracleDbType.Varchar2, 200).Value = data.USERNAME;
            repository.Parameters.Add("P_ROWCOUNT", OracleDbType.Int64).Direction = ParameterDirection.Output;

            repository.SaveChanges("PKG_B2C_ORDERS.B2C_CUSTOMER_ACTUALIZAR");
            if (repository.Status.Code == Status.Ok)
            {
                response.Data = true;
                response.Message = "Customer Actualizado correctamente";
            }
            else
            {
                response.Data = false;
                response.Message = repository.Status.Message;
            }
            response.Code = repository.Status.Code;

            return await Task.Run(() => response);
        }

        public async Task<ResponseBase<Boolean>> DeleteCustomer(Customer data)
        {
            IRepository<OracleParameterCollection> repository = new OracleRepository(connString, "P_CUSTID");
            var response = new ResponseBase<Boolean>();

            repository.Parameters.Add("P_CUSTID", OracleDbType.Varchar2, 200).Value = data.CUSTID;
            repository.Parameters.Add("P_ROWCOUNT", OracleDbType.Int64).Direction = ParameterDirection.Output;

            repository.SaveChanges("PKG_B2C_ORDERS.B2C_CUSTOMER_ELIMINAR");

            if (repository.Status.Code == Status.Ok)
            {
                response.Data = true;
                response.Message = "Customer Eliminado correctamente";
            }
            else
            {
                response.Data = false;
                response.Message = repository.Status.Message;
            }
            response.Code = repository.Status.Code;

            return await Task.Run(() => response);
        }
    }
}
