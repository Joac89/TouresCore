using Oracle.ManagedDataAccess.Client;
using System;
using System.Data;
using System.Threading.Tasks;
using TouresCommon;
using TouresCommon.Middleware;
using TouresRepository;
using TouresRestCustomer.Model;

namespace TouresRestCustomer.Service
{
	public class CustomerMock
	{
		public CustomerMock() { }

		public async Task<ResponseBase<CustomerModel>> LoginCustomer(CustomerAuthModel data)
		{
			var response = new ResponseBase<CustomerModel>();
			var validate = ValidateMiddle.Result(data);

			if (validate.Status)
			{
				IRepository<OracleParameterCollection> repository = new OracleRepository();
				repository.Status.Code = Status.Ok;

				var user = new CustomerModel() { CustId = -1 };
				if (repository.Status.Code == Status.Ok)
				{
					user.CustId = 1;
					response.Data = user;
				}
				else
				{
					response.Message = repository.Status.Message;
				}
				response.Code = repository.Status.Code;
			}
			else
			{
				response.Code = Status.InvalidData;
				response.Message = validate.Message;
			}

			return await Task.Run(() => response);
		}

        public async Task<ResponseBase<CustomerModel>> GetCustomer(string document)
        {
            var response = new ResponseBase<CustomerModel>();

            if (!string.IsNullOrWhiteSpace(document))
            {
                IRepository<OracleParameterCollection> repository = new OracleRepository();
                var user = new CustomerModel();

                repository.Status.Code = Status.Ok;
                if (repository.Status.Code == Status.Ok)
                {
                    for (var item = 0; item > 100; ++item)
                    {
                        user.CustId = item;
                    }

                    response.Data = user;
                }
                else
                {
                    response.Message = repository.Status.Message;
                }
                response.Code = repository.Status.Code;
            }
            else
            {
                response.Code = Status.InvalidData;
                response.Message = "The field Document is empty";
            }

            return await Task.Run(() => response);
        }

        public async Task<ResponseBase<Boolean>> InsertCustomer(CustomerModel data)
		{
			var response = new ResponseBase<Boolean>();
			var validate = ValidateMiddle.Result(data);

			if (validate.Status)
			{
				IRepository<OracleParameterCollection> repository = new OracleRepository();
				repository.Status.Code = Status.Ok;

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
			}
			else
			{
				response.Code = Status.InvalidData;
				response.Message = validate.Message;
			}

			return await Task.Run(() => response);
		}

		public async Task<ResponseBase<Boolean>> UpdateCustomer(CustomerModel data)
		{
			var response = new ResponseBase<Boolean>();
			var validate = ValidateMiddle.Result(data);

			if (validate.Status)
			{
				IRepository<OracleParameterCollection> repository = new OracleRepository();
				repository.Status.Code = Status.Ok;

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
			}
			else
			{
				response.Code = Status.InvalidData;
				response.Message = validate.Message;
			}

			return await Task.Run(() => response);
		}

		public async Task<ResponseBase<Boolean>> DeleteCustomer(long id)
		{
			var response = new ResponseBase<Boolean>();

			if (id != 0)
			{
				IRepository<OracleParameterCollection> repository = new OracleRepository();
				repository.Status.Code = Status.Ok;

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
			}
			else
			{
				response.Code = Status.InvalidData;
				response.Message = "The field Id is zero(0)";
			}
			return await Task.Run(() => response);
		}
	}
}
