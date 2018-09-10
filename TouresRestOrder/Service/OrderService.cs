using Oracle.ManagedDataAccess.Client;
using System;
using System.Data;
using System.Threading.Tasks;
using TouresCommon;
using TouresDataAccess;
using TouresRestOrder.Model;

namespace TouresRestOrder.Service
{
    public class OrderService
    {
        private string connString = "";
        public OrderService(string ConnectionString)
        {
            connString = ConnectionString;
        }

        #region Publicas
        public async Task<ResponseBase<Boolean>> InsertOrder(Order data)
        {
            try
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
                    var OrdId = Int64.Parse(repository.Parameters["P_ORDID"].Value.ToString());
                    foreach (var item in data.LItems)
                    {
                        item.OrdId = OrdId;
                        if (!InsertItem(item).Data)
                        {

                        }
                    }
                    response.Data = true;
                    response.Message = "Orden insertada correctamente";
                }
                else
                {
                    response.Data = false;
                    response.Message = repository.Status.Message;
                }
                response.Code = repository.Status.Code;

                return await Task.Run(() => response);
            }
            catch (Exception)
            {

                throw;
            }

        }
        #endregion

        #region Privadas
        private ResponseBase<Boolean> InsertItem(Item ObjItem)
        {
            IRepository<OracleParameterCollection> repository = new OracleRepository(connString, "P_ITEMID");
            repository.Parameters.Add("P_ORDID", OracleDbType.Int64).Value = ObjItem.OrdId;
            repository.Parameters.Add("P_PRODID", OracleDbType.Int64).Value = ObjItem.ProdId;
            repository.Parameters.Add("P_PRODUCTNAME", OracleDbType.Varchar2).Value = ObjItem.ProductName;
            repository.Parameters.Add("P_PARTNUM", OracleDbType.Varchar2).Value = ObjItem.PartNum;
            repository.Parameters.Add("P_PRICE", OracleDbType.Long).Value = ObjItem.Price;
            repository.Parameters.Add("P_QUANTITY", OracleDbType.Int16).Value = ObjItem.Quantity;
            repository.Parameters.Add("P_ITEMID", OracleDbType.Int64).Direction = ParameterDirection.Output;
            repository.SaveChanges("PKG_B2C_ORDERS.B2C_ITEMS_INSERT");
            if (repository.Status.Code == Status.Ok)
            {
                return new ResponseBase<bool>
                {
                    Code = repository.Status.Code,
                    Data = true,
                    Message = "Inserto correcto"
                };
            }
            else
            {
                return new ResponseBase<bool>
                {
                    Code = repository.Status.Code,
                    Data = false,
                    Message = repository.Status.Message
                };
            }
        }
        #endregion

    }
}
