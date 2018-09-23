using Oracle.ManagedDataAccess.Client;
using System;
using System.Data;
using System.Threading.Tasks;
using TouresCommon;
using TouresRepository;
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

		public async Task<ResponseBase<CustomerModel>> GetCustomer(string document)
		{
            IRepository<OracleParameterCollection> repository = new OracleRepository(connString, "C_DATASET");
            var response = new ResponseBase<CustomerModel>();
            var user = new CustomerModel();

            repository.Parameters.Add("P_DOCNUMBER", OracleDbType.Varchar2, 200).Value = document;
            repository.Parameters.Add("C_DATASET", OracleDbType.RefCursor).Direction = ParameterDirection.Output;

            var result = repository.Get("PKG_B2C_CUSTOMER.B2C_CUSTOMER_SELECT");
            if (repository.Status.Code == Status.Ok)
            {
                
                foreach (var item in result)
                {
                    user.CustId = long.Parse(item["CUSTID"].ToString());
                    user.FName = item["FNAME"].ToString();
                    user.LName = item["LNAME"].ToString();
                    user.PhoneNumber = item["PHONENUMBER"].ToString();
                    user.Email = item["EMAIL"].ToString();
                    user.Password = item["PASSWORD"].ToString();
                    user.CreditCardType = item["CREDITCARDTYPE"].ToString();
                    user.CreditCardNumber = item["CREDITCARDNUMBER"].ToString();
                    user.Status = item["STATUS"].ToString();
                    user.DocNumber = item["DOCNUMBER"].ToString();
                    user.UserName = item["USERNAME"].ToString();
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

        public async Task<ResponseBase<Boolean>> InsertCustomer(CustomerModel data)
        {
            IRepository<OracleParameterCollection> repository = new OracleRepository(connString, "P_CUSTID");
            var response = new ResponseBase<Boolean>();

            repository.Parameters.Add("P_FNAME", OracleDbType.Varchar2, 200).Value = data.FName;
            repository.Parameters.Add("P_LNAME", OracleDbType.Varchar2, 200).Value = data.LName;
            repository.Parameters.Add("P_PHONENUMBER", OracleDbType.Varchar2, 200).Value = data.PhoneNumber;
            repository.Parameters.Add("P_EMAIL", OracleDbType.Varchar2, 200).Value = data.Email;
            repository.Parameters.Add("P_PASSWORD", OracleDbType.Varchar2, 200).Value = data.Password;
            repository.Parameters.Add("P_CREDITCARDTYPE", OracleDbType.Varchar2, 200).Value = data.CreditCardType;
            repository.Parameters.Add("P_CREDITCARDNUMBER", OracleDbType.Varchar2, 200).Value = data.CreditCardNumber;
            repository.Parameters.Add("P_STATUS", OracleDbType.Varchar2, 200).Value = data.Status;            
            repository.Parameters.Add("P_DOCNUMBER", OracleDbType.Int64).Value = data.DocNumber;
            repository.Parameters.Add("P_USERNAME", OracleDbType.Varchar2, 200).Value = data.UserName;            
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

        public async Task<ResponseBase<Boolean>> UpdateCustomer(CustomerModel data)
        {
            IRepository<OracleParameterCollection> repository = new OracleRepository(connString, "P_CUSTID");
            var response = new ResponseBase<Boolean>();

            repository.Parameters.Add("P_CUSTID", OracleDbType.Int64).Value = data.CustId;
            repository.Parameters.Add("P_FNAME", OracleDbType.Varchar2, 200).Value = data.FName;
            repository.Parameters.Add("P_LNAME", OracleDbType.Varchar2, 200).Value = data.LName;
            repository.Parameters.Add("P_PHONENUMBER", OracleDbType.Varchar2, 200).Value = data.PhoneNumber;
            repository.Parameters.Add("P_EMAIL", OracleDbType.Varchar2, 200).Value = data.Email;
            repository.Parameters.Add("P_PASSWORD", OracleDbType.Varchar2, 200).Value = data.Password;
            repository.Parameters.Add("P_CREDITCARDTYPE", OracleDbType.Varchar2, 200).Value = data.CreditCardType;
            repository.Parameters.Add("P_CREDITCARDNUMBER", OracleDbType.Varchar2, 200).Value = data.CreditCardNumber;
            repository.Parameters.Add("P_STATUS", OracleDbType.Varchar2, 200).Value = data.Status;
            repository.Parameters.Add("P_DOCNUMBER", OracleDbType.Int64).Value = data.DocNumber;
            repository.Parameters.Add("P_USERNAME", OracleDbType.Varchar2, 200).Value = data.UserName;
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

        public async Task<ResponseBase<Boolean>> DeleteCustomer(long id)
        {
            IRepository<OracleParameterCollection> repository = new OracleRepository(connString, "P_CUSTID");
            var response = new ResponseBase<Boolean>();

            repository.Parameters.Add("P_CUSTID", OracleDbType.Varchar2, 200).Value = id;
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
