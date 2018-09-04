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

            repository.Parameters.Add("P_CustId", OracleDbType.Int64).Value = data.CustId;
            repository.Parameters.Add("P_OrdenDate", OracleDbType.Date, 200).Value = DateTime.Now;          
            repository.Parameters.Add("P_Price", OracleDbType.Long, 200).Value = data.Price;
            repository.Parameters.Add("P_Status", OracleDbType.Varchar2).Value = data.Status;
            repository.Parameters.Add("P_Comments", OracleDbType.Varchar2).Value = data.Comments;
            repository.Parameters.Add("P_ORDID", OracleDbType.Int64).Direction = ParameterDirection.Output;

            repository.SaveChanges("PKG_B2C_ORDERS.B2C_ORDERS_INSERT");
            if (repository.Status.Code == Status.Ok)
            {
                response.Data = true;
                response.Message = repository.Status.Message;
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
