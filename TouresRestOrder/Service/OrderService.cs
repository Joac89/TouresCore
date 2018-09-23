using Oracle.ManagedDataAccess.Client;
using System;
using System.Collections.Generic;
using System.Data;
using System.Threading.Tasks;
using TouresCommon;
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
		public async Task<ResponseBase<bool>> InsertOrder(OrderModel data)
		{
			IRepository<OracleParameterCollection> repository = new OracleRepository(connString, "P_ORDID");
			var response = new ResponseBase<bool>();

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

		public async Task<ResponseBase<List<OrderModel>>> GetOrder()
		{
			IRepository<OracleParameterCollection> repository = new OracleRepository(connString, "C_DATASET");
			var response = new ResponseBase<List<OrderModel>>();
			var order = new OrderModel();
			var lOrder = new List<OrderModel>();

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
					order.Status = item["STATUS"].ToString();
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

		public async Task<ResponseBase<List<ItemModel>>> GetItem(int IdOrder)
		{
			IRepository<OracleParameterCollection> repository = new OracleRepository(connString, "C_DATASET");
			var response = new ResponseBase<List<ItemModel>>();
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
					ObjItem.ItemId = long.Parse(item["COMMENTS"].ToString());
					ObjItem.OrdId = long.Parse(item["OrdId"].ToString());
					ObjItem.PartNum = item["PartNum"].ToString();
					ObjItem.Price = decimal.Parse(item["Price"].ToString());
					ObjItem.ProdId = long.Parse(item["ProdId"].ToString());
					ObjItem.ProductName = item["ProductName"].ToString();
					ObjItem.Quantity = int.Parse(item["ProductName"].ToString());
					listItem.Add(ObjItem);
				}
				response.Data = listItem;
			}
			else
			{
				response.Message = repository.Status.Message;
			}
			response.Code = repository.Status.Code;

			return await Task.Run(() => response);
		}
		#endregion

		#region Privadas
		private ResponseBase<Boolean> InsertItem(ItemModel ObjItem)
		{
			var response = new ResponseBase<bool>();

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
				response.Code = Status.Ok;
				response.Data = true;
			}
			else
			{
				response.Code = repository.Status.Code;
				response.Message = repository.Status.Message;
				response.Data = false;
			}

			return response;
		}
		#endregion
	}
}
