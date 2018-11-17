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
        /// <summary>
        /// Inserta una orden
        /// </summary>
        /// <param name="data">Tipo de documento del cliente a buscar</param>
        /// <returns>Devuelve un objeto con la información de la orden insertada</returns>
        /// <response code="422">Invalid Data</response>
        [ProducesResponseType(200)]
        [ProducesResponseType(422)]
        [Authorize]
        [HttpPost("insert")]
        public async Task<IActionResult> InsertOrder([FromBody] OrderModel data)
        {
            var result = new ResponseBase<bool>();

            result = await new OrderService(oracleConn).InsertOrder(data);
            return this.Result(result.Code, result);
        }

        /// <summary>
        /// Obtiene las ordenes de un usuario por id
        /// </summary>
        /// <param name="customer">Id de usuario</param>
        /// <returns>Devuelve un objeto con la información de las ordenes consultadas</returns>
        /// <response code="422">Invalid Data</response>
        [ProducesResponseType(200)]
        [ProducesResponseType(422)]
        [Authorize]
        [HttpGet("{customer}")]
        public async Task<IActionResult> GetOrders(long customer)
        {
            var result = new ResponseBase<List<OrderModel>>();

            result = await new OrderService(oracleConn).GetOrders(customer);
            result.Data = (from item in result.Data orderby item.IdEstado descending select item).ToList();

            return this.Result(result.Code, result);
        }
        /// <summary>
        /// Obtiene las ordenes
        /// </summary>
        /// <returns>Devuelve un objeto con la información de las ordenes consultadas</returns>
        /// <response code="422">Invalid Data</response>
        [ProducesResponseType(200)]
        [ProducesResponseType(422)]
        [Authorize]
        //[HttpGet("{customer}")]
        public async Task<IActionResult> GetAllOrders()
        {
            var result = new ResponseBase<List<OrderModel>>();

            result = await new OrderService(oracleConn).GetAllOrders();
            result.Data = (from item in result.Data orderby item.IdEstado descending select item).ToList();

            return this.Result(result.Code, result);
        }

        /// <summary>
        /// Obtiene los items de una orden
        /// </summary>
        /// <param name="IdOrder">Id de la orden</param>
        /// <returns>Devuelve una lista con los items de la orden</returns>
        /// <response code="422">Invalid Data</response>
        [ProducesResponseType(200)]
        [ProducesResponseType(422)]
        [Authorize]
        [HttpGet("{IdOrder}")]
        public async Task<IActionResult> GetItemFromOrder(int IdOrder)
        {
            var result = new ResponseBase<List<ItemModel>>();

            result = await new OrderService(oracleConn).GetItemFromOrder(IdOrder);
            return this.Result(result.Code, result);
        }
        #endregion

        /// <summary>
        /// Borra una orden (deprecado. use CancelOrder)
        /// </summary>
        /// <param name="id">Id de la orden</param>
        /// <returns>Devuelve un objeto con el resultado de la orden eliminada</returns>
        /// <response code="422">Invalid Data</response>
        [ProducesResponseType(200)]
        [ProducesResponseType(422)]
        [Authorize]
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteOrder(int id, int estado)
        {
            var result = new ResponseBase<bool>();
            var mensaje = string.Empty;

            result = await new OrderService(oracleConn).ActualizaEstadoOrder(id, estado);
            return this.Result(result.Code, result);
        }

        /// <summary>
        /// Obtiene los items de una orden (deprecado, use GetItemFromOrder)
        /// </summary>
        /// <param name="IdOrder">Id de la orden</param>
        /// <returns>Devuelve una lista con los items de la orden</returns>
        /// <response code="422">Invalid Data</response>
        [ProducesResponseType(200)]
        [ProducesResponseType(422)]
        [Authorize]
        [HttpGet("{idOrder}")]
        public async Task<IActionResult> GetItem(int idOrder)
        {
            var result = new ResponseBase<List<ItemModel>>();

            result = await new OrderService(oracleConn).GetItemFromOrder(idOrder);
            return this.Result(result.Code, result);
        }

        /// <summary>
        /// Actualiza una orden
        /// </summary>
        /// <param name="id">Id de la orden</param>
        /// <param name="idEstado">Nuevo estado de la orden</param>
        /// <returns>Devuelve un objeto con el resultado de la orden actualizada</returns>
        /// <response code="422">Invalid Data</response>
        [ProducesResponseType(200)]
        [ProducesResponseType(422)]
        [Authorize]
        [HttpPost("update/{idOrder}/{idEstado}")]
        public async Task<IActionResult> UpdateEstadoOrder(int idOrder, int idEstado)
        {
            var result = new ResponseBase<bool>();
            var mensaje = string.Empty;
            result = await new OrderService(oracleConn).ActualizaEstadoOrder(idOrder, idEstado);
            return this.Result(result.Code, result);
        }

        /// <summary>
        /// Borra una orden
        /// </summary>
        /// <param name="id">Id de la orden</param>
        /// <returns>Devuelve un objeto con el resultado de la orden eliminada</returns>
        /// <response code="422">Invalid Data</response>
        [ProducesResponseType(200)]
        [ProducesResponseType(422)]
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