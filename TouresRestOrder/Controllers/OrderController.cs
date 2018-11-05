using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Cors;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using TouresCommon;
using TouresRestOrder.Model;
using TouresRestOrder.Service;

namespace TouresRestOrder.Controllers
{
	[Produces("application/json")]
	[Route("api/v1/Order")]
	[EnableCors("*")]
	public class OrderController : Controller
	{
		private IConfiguration config;
		private string oracleConn;

		public OrderController(IConfiguration configuration)
		{
			config = configuration;
			oracleConn = config["oracleConnection"];
		}

		#region Publicas

		[Authorize]
		[HttpPost("insert")]
		public async Task<IActionResult> InsertOrder([FromBody] OrderModel data)
		{
			var result = new ResponseBase<bool>();

			result = await new OrderService(oracleConn).InsertOrder(data);
			return this.Result(result.Code, result);
		}

		[Authorize]
		[HttpGet("{customer}")]
		public async Task<IActionResult> GetOrders(long customer)
		{
			var result = new ResponseBase<List<OrderModel>>();

			result = await new OrderService(oracleConn).GetOrders(customer);
            result.Data = (from item in result.Data orderby item.IdEstado descending select item).ToList();

			return this.Result(result.Code, result);
		}

		[Authorize]
		[HttpGet]
		public async Task<IActionResult> GetItemFromOrder([FromBody] int IdOrder)
		{
			var result = new ResponseBase<List<ItemModel>>();

			result = await new OrderService(oracleConn).GetItemFromOrder(IdOrder);
			return this.Result(result.Code, result);
		}
		#endregion

		[Authorize]
		[HttpDelete("{id}")]
		public async Task<IActionResult> DeleteOrder(int id, int estado)
		{
			var result = new ResponseBase<bool>();
			var mensaje = string.Empty;

			result = await new OrderService(oracleConn).ActualizaEstadoOrder(id, estado);
			return this.Result(result.Code, result);
		}

		[Authorize]
		[HttpGet("{idOrder}")]
		public async Task<IActionResult> GetItem(int idOrder)
		{
			var result = new ResponseBase<List<ItemModel>>();

			result = await new OrderService(oracleConn).GetItemFromOrder(idOrder);
			return this.Result(result.Code, result);
		}

		[Authorize]
		[HttpPost("update/{idOrder}/{idEstado}")]
		public async Task<IActionResult> UpdateEstadoOrder(int idOrder, int idEstado)
		{
			var result = new ResponseBase<bool>();
			var mensaje = string.Empty;
			result = await new OrderService(oracleConn).ActualizaEstadoOrder(idOrder, idEstado);
			return this.Result(result.Code, result);
		}

		[Authorize]
		[HttpPost("cancel/{idOrder}")]
		public async Task<IActionResult> CancelOrder(int idOrder)
		{
			var result = new ResponseBase<bool>();
			var mensaje = string.Empty;
			result = await new OrderService(oracleConn).ActualizaEstadoOrder(idOrder, 4);
			return this.Result(result.Code, result);
		}
	}
}