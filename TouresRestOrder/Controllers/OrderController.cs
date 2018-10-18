﻿using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Cors;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
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
		[HttpPost]
		public async Task<IActionResult> InsertOrder([FromBody] OrderModel data)
		{
			var result = new ResponseBase<bool>();
			var mensaje = string.Empty;

			if (ValidaOrden(data, ref mensaje))
			{
				result = await new OrderService(oracleConn).InsertOrder(data);
			}
			else
			{
				result.Code = Status.UnprocessableEntity;
				result.Message = mensaje;
			}

			//if (ValidaOrden(data, ref mensaje))
			//{
			//	result = await new OrderService(oracleConn).InsertOrder(data);
			//}
			//else
			//{
			//	result.Code = Status.UnprocessableEntity;
			//	result.Message = mensaje;
			//}

			return this.Result(result.Code, result);
		}

		[Authorize]
		[HttpGet("{customer}")]
		public async Task<IActionResult> GetOrders(long customer)
		{
			var result = new ResponseBase<List<OrderModel>>();

			result = await new OrderService(oracleConn).GetOrders(customer);
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
		public async Task<IActionResult> DeleteOrder(long id)
		{
			var result = new ResponseBase<bool>();
			var mensaje = string.Empty;

			result = await new OrderService(oracleConn).DeleteOrder(id);
			return this.Result(result.Code, result);
		}


		private Boolean ValidaOrden(OrderModel ObjOrden, ref string Error)
		{
			Boolean retorno = true;
			if (ObjOrden.CustId == 0)
			{
				Error = "El cliente ingresado no es valido.";
				retorno = false;
			}
			if (ObjOrden.Price == 0)
			{
				Error += " El precio ingresado no es valido.";
				retorno = false;
			}
			if (ObjOrden.LItems == null || ObjOrden.LItems.Count == 0)
			{
				Error += " La orden debe tener al menos un producto asociado.";
				retorno = false;
			}
			foreach (var item in ObjOrden.LItems)
			{
				if (item.ProductName == string.Empty)
				{
					Error += " El producto no es valido.";
					retorno = false;
				}
				if (item.ProdId == 0)
				{
					Error += " El codigo del producto " + item.ProductName + " no es valido.";
					retorno = false;
				}
				if (item.Price == 0)
				{
					Error += " El precio del producto " + item.ProductName + " no es valido.";
					retorno = false;
				}
				if (item.Quantity == 0)
				{
					Error += " La cantidad del producto " + item.ProductName + " no es valida.";
					retorno = false;
				}
			}
			return retorno;
		}
	}
}