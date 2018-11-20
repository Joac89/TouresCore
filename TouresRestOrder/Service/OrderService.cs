using Oracle.ManagedDataAccess.Client;
using System;
using System.Collections.Generic;
using System.Data;
using System.Threading.Tasks;
using TouresCommon;
using TouresCommon.Middleware;
using TouresRepository;
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
		public async Task<ResponseBase<long>> InsertOrder(OrderModel data)
		{
			var response = new ResponseBase<long>();
			var validate = ValidateMiddle.Result(data);

			if (validate.Status)
			{
				IRepository<OracleParameterCollection> repository = new OracleRepository(connString, "P_ORDID");

				repository.Parameters.Add("P_CUSTID", OracleDbType.Int64).Value = data.CustId;
				repository.Parameters.Add("P_ORDENDATE", OracleDbType.Date, 200).Value = DateTime.Now;
				repository.Parameters.Add("P_PRICE", OracleDbType.Long, 200).Value = data.Price;
				repository.Parameters.Add("P_IDESTADO", OracleDbType.Int16).Value = 1;
				//repository.Parameters.Add("P_STATUS", OracleDbType.Varchar2).Value = data.Status;
				repository.Parameters.Add("P_COMMENTS", OracleDbType.Varchar2).Value = data.Comments;
				repository.Parameters.Add("P_ORDID", OracleDbType.Int64).Direction = ParameterDirection.Output;

				repository.SaveChanges("PKG_B2C_ORDERS.B2C_ORDERS_INSERT");
				if (repository.Status.Code == Status.Ok)
				{
					var OrdId = Int64.Parse(repository.Parameters["P_ORDID"].Value.ToString());
					foreach (var item in data.LItems)
					{
						item.OrdId = OrdId;
						item.IdEstado = 1;
						InsertItem(item);
					};

					response.Data = OrdId;
					response.Message = "Orden insertada correctamente";
				}
				else
				{
					response.Data = -1;
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
				IRepository<OracleParameterCollection> repository = new OracleRepository(connString, "C_DATASET");
				var order = new OrderModel();
				var lOrder = new List<OrderModel>();

				repository.Parameters.Add("P_CUSTID", OracleDbType.Int64).Value = custId;
				repository.Parameters.Add("C_DATASET", OracleDbType.RefCursor).Direction = ParameterDirection.Output;

				var result = repository.Get("PKG_B2C_ORDERS.B2C_ORDERS_SELECT_CLIENTE");
				if (repository.Status.Code == Status.Ok)
				{
					foreach (var item in result)
					{
						order = new OrderModel();
						order.Comments = item["COMMENTS"].ToString();
						order.CustId = long.Parse(item["CUSTID"].ToString());
						order.OrdenDate = DateTime.Parse(item["ORDENDATE"].ToString());
						order.OrdId = long.Parse(item["ORDID"].ToString());
						order.Price = decimal.Parse(item["PRICE"].ToString());
						order.Status = item["IDESTADO"].ToString();
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
        public async Task<ResponseBase<List<OrderModel>>> GetOrdersById(long OrderId)
        {
            var response = new ResponseBase<List<OrderModel>>();

            if (OrderId > 0)
            {
                IRepository<OracleParameterCollection> repository = new OracleRepository(connString, "C_DATASET");
                var order = new OrderModel();
                var lOrder = new List<OrderModel>();

                repository.Parameters.Add("P_ORDID", OracleDbType.Int64).Value = OrderId;
                repository.Parameters.Add("C_DATASET", OracleDbType.RefCursor).Direction = ParameterDirection.Output;

                var result = repository.Get("PKG_B2C_ORDERS.B2C_ORDERS_SELECT_ID");
                if (repository.Status.Code == Status.Ok)
                {
                    foreach (var item in result)
                    {
                        order = new OrderModel();
                        order.Comments = item["COMMENTS"].ToString();
                        order.CustId = long.Parse(item["CUSTID"].ToString());
                        order.OrdenDate = DateTime.Parse(item["ORDENDATE"].ToString());
                        order.OrdId = long.Parse(item["ORDID"].ToString());
                        order.Price = decimal.Parse(item["PRICE"].ToString());
                        order.Status = item["IDESTADO"].ToString();
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
        public async Task<ResponseBase<List<OrderModel>>> GetOrdersByProduct(long ProductId)
        {
            var response = new ResponseBase<List<OrderModel>>();

            if (ProductId > 0)
            {
                IRepository<OracleParameterCollection> repository = new OracleRepository(connString, "C_DATASET");
                var order = new OrderModel();
                var lOrder = new List<OrderModel>();

                repository.Parameters.Add("P_PRODID", OracleDbType.Int64).Value = ProductId;
                repository.Parameters.Add("C_DATASET", OracleDbType.RefCursor).Direction = ParameterDirection.Output;

                var result = repository.Get("PKG_B2C_ORDERS.B2C_ORDERS_SELECT_PRODUCT");
                if (repository.Status.Code == Status.Ok)
                {
                    foreach (var item in result)
                    {
                        order = new OrderModel();
                        order.Comments = item["COMMENTS"].ToString();
                        order.CustId = long.Parse(item["CUSTID"].ToString());
                        order.OrdenDate = DateTime.Parse(item["ORDENDATE"].ToString());
                        order.OrdId = long.Parse(item["ORDID"].ToString());
                        order.Price = decimal.Parse(item["PRICE"].ToString());
                        order.Status = item["IDESTADO"].ToString();
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
        //B2C_ORDERS_SELECT_PRODUCT
        public async Task<ResponseBase<List<OrderModel>>> GetAllOrders()
        {
            var response = new ResponseBase<List<OrderModel>>();

           
                IRepository<OracleParameterCollection> repository = new OracleRepository(connString, "C_DATASET");
                var order = new OrderModel();
                var lOrder = new List<OrderModel>();

                //repository.Parameters.Add("P_CUSTID", OracleDbType.Int64).Value = custId;
                repository.Parameters.Add("C_DATASET", OracleDbType.RefCursor).Direction = ParameterDirection.Output;

                var result = repository.Get("PKG_B2C_ORDERS.B2C_ORDERS_SELECT");
                if (repository.Status.Code == Status.Ok)
                {
                    foreach (var item in result)
                    {
                        order = new OrderModel();
                        order.Comments = item["COMMENTS"].ToString();
                        order.CustId = long.Parse(item["CUSTID"].ToString());
                        order.OrdenDate = DateTime.Parse(item["ORDENDATE"].ToString());
                        order.OrdId = long.Parse(item["ORDID"].ToString());
                        order.Price = decimal.Parse(item["PRICE"].ToString());
                        order.Status = item["IDESTADO"].ToString();
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
            

            return await Task.Run(() => response);
        }

        public async Task<ResponseBase<List<ItemModel>>> GetItemFromOrder(long IdOrder)
		{
			var response = new ResponseBase<List<ItemModel>>();

			if (IdOrder > 0)
			{
				IRepository<OracleParameterCollection> repository = new OracleRepository(connString, "C_DATASET");
				var ObjItem = new ItemModel();
				var listItem = new List<ItemModel>();

				repository.Parameters.Add("P_ORDID", OracleDbType.Int64).Value = IdOrder;
				repository.Parameters.Add("C_DATASET", OracleDbType.RefCursor).Direction = ParameterDirection.Output;

				var result = repository.Get("PKG_B2C_ORDERS.B2C_ITEMS_SELECT_ORDEN");
				if (repository.Status.Code == Status.Ok)
				{
					foreach (var item in result)
					{
						ObjItem = new ItemModel();
						ObjItem.ItemId = long.Parse(item["ITEMID"].ToString());
						ObjItem.OrdId = long.Parse(item["ORDID"].ToString());
						ObjItem.PartNum = item["PARTNUM"].ToString();
						ObjItem.Price = decimal.Parse(item["PRICE"].ToString());
						ObjItem.ProdId = long.Parse(item["PRODID"].ToString());
						ObjItem.ProductName = item["PRODUCTNAME"].ToString();
						ObjItem.Quantity = int.Parse(item["QUANTITY"].ToString());

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

			if (IdOrden > 0)
			{
				IRepository<OracleParameterCollection> repository = new OracleRepository(connString, "P_ROWCOUNT");

				repository.Parameters.Add("P_ORDID", OracleDbType.Int64).Value = IdOrden;
				repository.Parameters.Add("P_IDESTADO", OracleDbType.Int16).Value = IdEstado;
				repository.Parameters.Add("P_ROWCOUNT", OracleDbType.Int64).Direction = ParameterDirection.Output;

				repository.SaveChanges("PKG_B2C_ORDERS.B2C_ORDERS_ACTUALIZA");
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

        public async Task<ResponseBase<OrderProviderModel>> CancelOrderProvider(int IdOrden, int ItemId, string Provider)
        {
            var response = new ResponseBase<OrderProviderModel>();

            if (IdOrden > 0)
            {
                var orderProvider = new OrderProviderModel();

                orderProvider.OrdId = IdOrden;
                orderProvider.ItemId = ItemId;
                orderProvider.Provider = Provider;
                orderProvider.Status = "Cancelado";
                orderProvider.ConfirmationId = GetRandomNumber(1000000, 2000000);

                    response.Data = orderProvider;
                    response.Message = "Existoso";
                    response.Code = 200;
            }
            else
            {
                response.Code = Status.InvalidData;
                response.Message = "The field IdOrder or IdEstado is zero(0)";
            }

            return await Task.Run(() => response);
        }

        //Function to get random number
        private static readonly Random getrandom = new Random();

        public static int GetRandomNumber(int min, int max)
        {
            lock (getrandom) // synchronize
            {
                return getrandom.Next(min, max);
            }
        }

        #endregion

        #region Privadas
        private ResponseBase<Boolean> InsertItem(ItemModel ObjItem)
		{
			var response = new ResponseBase<bool>();
			var validate = ValidateMiddle.Result(ObjItem);

			if (validate.Status)
			{
				IRepository<OracleParameterCollection> repository = new OracleRepository(connString, "P_ITEMID");

				repository.Parameters.Add("P_ORDID", OracleDbType.Int64).Value = ObjItem.OrdId;
				repository.Parameters.Add("P_PRODID", OracleDbType.Int64).Value = ObjItem.ProdId;
				repository.Parameters.Add("P_PRODUCTNAME", OracleDbType.Varchar2).Value = ObjItem.ProductName;
				repository.Parameters.Add("P_PARTNUM", OracleDbType.Varchar2).Value = ObjItem.PartNum;
				repository.Parameters.Add("P_PRICE", OracleDbType.Long).Value = ObjItem.Price;
				repository.Parameters.Add("P_QUANTITY", OracleDbType.Int16).Value = ObjItem.Quantity;
				repository.Parameters.Add("P_IDESTADO", OracleDbType.Int16).Value = ObjItem.IdEstado;
				repository.Parameters.Add("P_ITEMID", OracleDbType.Int64).Direction = ParameterDirection.Output;

				repository.SaveChanges("PKG_B2C_ORDERS.B2C_ITEMS_INSERT");
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
		#endregion
	}
}
