using Oracle.ManagedDataAccess.Client;
using System.Collections.Generic;
using System.Threading.Tasks;
using TouresCommon;
using TouresCommon.Middleware;
using TouresRepository;
using TouresRestOrder.Model;

namespace TouresRestOrder.Service
{
    public class OrderMock
    {
        public OrderMock() { }

        public async Task<ResponseBase<bool>> InsertOrder(OrderModel data)
        {
            var response = new ResponseBase<bool>();
            var validate = ValidateMiddle.Result(data);

            if (validate.Status)
            {
                IRepository<OracleParameterCollection> repository = new OracleRepository();
                repository.Status.Code = Status.Ok;

                if (repository.Status.Code == Status.Ok)
                {
                    var OrdId = 1;
                    foreach (var item in data.LItems)
                    {
                        item.OrdId = OrdId;
                        item.IdEstado = 1;
                        InsertItem(item);
                    };

                    response.Data = true;
                    response.Message = "Orden insertada correctamente";
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

        public async Task<ResponseBase<List<OrderModel>>> GetOrders(long custId)
        {
            var response = new ResponseBase<List<OrderModel>>();

            if (custId > 0)
            {
                IRepository<OracleParameterCollection> repository = new OracleRepository();
                var order = new OrderModel();
                var lOrder = new List<OrderModel>();

                repository.Status.Code = Status.Ok;
                if (repository.Status.Code == Status.Ok)
                {
                    for (var item = 0; item < 100; ++item)
                    {
                        order = new OrderModel();
                        order.CustId = custId;
                        order.OrdId = item;
                        order.Status = "1";
                        order.LItems = GetItemFromOrder(order.OrdId).Result.Data;

                        lOrder.Add(order);
                    }

                    response.Data = lOrder;
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
                response.Message = "The field CustId is zero(0)";
            }

            return await Task.Run(() => response);
        }

        public async Task<ResponseBase<List<ItemModel>>> GetItemFromOrder(long IdOrder)
        {
            var response = new ResponseBase<List<ItemModel>>();

            if (IdOrder > 0)
            {
                IRepository<OracleParameterCollection> repository = new OracleRepository();
                var ObjItem = new ItemModel();
                var listItem = new List<ItemModel>();

                repository.Status.Code = Status.Ok;
                if (repository.Status.Code == Status.Ok)
                {
                    for (var item = 0; item < 50; ++item)
                    {
                        ObjItem = new ItemModel();
                        ObjItem.ItemId = item;
                        ObjItem.OrdId = IdOrder;

                        listItem.Add(ObjItem);
                    };
                    response.Data = listItem;
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
                response.Message = "The field IdOrder is zero(0)";
            }

            return await Task.Run(() => response);
        }

        public async Task<ResponseBase<bool>> ActualizaEstadoOrder(int IdOrden, int IdEstado)
        {
            var response = new ResponseBase<bool>();

            if (IdOrden > 0 && IdEstado > 0)
            {
                IRepository<OracleParameterCollection> repository = new OracleRepository();
                repository.Status.Code = Status.Ok;
              
                if (repository.Status.Code == Status.Ok)
                {
                    response.Data = true;
                    response.Message = "Orden actualizada correctamente";
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
                response.Message = "The field IdOrder or IdEstado is zero(0)";
            }

            return await Task.Run(() => response);
        }

        private ResponseBase<bool> InsertItem(ItemModel ObjItem)
        {
            var response = new ResponseBase<bool>();
            var validate = ValidateMiddle.Result(ObjItem);

            if (validate.Status)
            {
                IRepository<OracleParameterCollection> repository = new OracleRepository();
                repository.Status.Code = Status.Ok;

                if (repository.Status.Code == Status.Ok)
                {
                    response.Code = Status.Ok;
                    response.Data = true;
                }
                else
                {
                    response.Code = repository.Status.Code;
                    response.Message = repository.Status.Message;
                    response.Data = false;
                }
            }
            else
            {
                response.Code = Status.InvalidData;
                response.Message = validate.Message;
            }

            return response;
        }
    }
}
