using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using TouresRestOrder.Model;
using TouresCommon;
using TouresDataAccess;
using Oracle.ManagedDataAccess.Client;
using System.Data;

namespace TouresRestOrder.Service
{
    public class OrderService
    {
        private string connString = "";
        public OrderService(string ConnectionString)
        {
            connString = ConnectionString;
        }

        public async Task<ResponseBase<Boolean>> InsertOrder(Order data)
        {
            IRepository<OracleParameterCollection> repository = new OracleRepository(connString, "P_ORDID");
            var response = new ResponseBase<Boolean>();

            repository.Parameters.Add("P_Comments", OracleDbType.Int64).Value = data.Comments;//data.UserName;
            repository.Parameters.Add("P_OrdenDate", OracleDbType.Varchar2, 200).Value = data.OrdenDate;
            repository.Parameters.Add("P_CustId", OracleDbType.Int64).Value = data.CustId;//data.UserName;
            repository.Parameters.Add("P_Price", OracleDbType.Varchar2, 200).Value = data.Price;
            repository.Parameters.Add("P_Status", OracleDbType.Int64).Value = data.Status;//data.UserName;
            repository.Parameters.Add("P_ORDID", OracleDbType.Long).Direction = ParameterDirection.Output;

            var result = repository.Get("PKG_B2C_ORDERS.B2C_ORDERS_INSERT");
            if (repository.Status.Code == Status.Ok)
            {
                foreach (var item in result)
                {
                  
                }
                response.Data = true;
            }
            else
            {
                response.Message = repository.Status.Message;
            }
            response.Code = repository.Status.Code;
            response.Data = false;
            return await Task.Run(() => response);
        }
    }
}
