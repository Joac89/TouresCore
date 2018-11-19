using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Cors;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using System.Collections.Generic;
using System.Threading.Tasks;
using TouresCommon;
using TouresRestCustomer.Model;
using TouresRestCustomer.Service;

namespace TouresRestCustomer.Controllers
{
	[Produces("application/json")]
	[Route("api/v1/Customer")]
	[EnableCors("*")]
	public class CustomerController : Controller
	{
		private IConfiguration config;
		private string oracleConn;

		public CustomerController(IConfiguration configuration)
		{
			config = configuration;
			oracleConn = config["oracleConnection"];
		}

        /// <summary>
        /// Busca un cliente por documento de identidad
        /// </summary>
        /// <param name="document">Tipo de documento del cliente a buscar</param>
        /// <returns>Devuelve un objeto con la información del cliente consultado</returns>
        /// <response code="422">Invalid Data</response>
        [ProducesResponseType(200)]
        [ProducesResponseType(422)]
        [Authorize]
		[HttpGet("{document}")]
        public async Task<IActionResult> GetCustomer(string document)
		{
			var result = new ResponseBase<CustomerModel>();
			result = await new CustomerService(oracleConn).GetCustomer(document);

			if (result.Data.CustId == 0) result.Code = Status.NotFound;

			return this.Result(result.Code, result);
		}
        /// <summary>
        /// Busca un cliente por documento de identidad
        /// </summary>
        /// <param name="product">Producto del cliente a buscar</param>
        /// <returns>Devuelve un objeto con la información del cliente consultado</returns>
        /// <response code="422">Invalid Data</response>
        [ProducesResponseType(200)]
        [ProducesResponseType(422)]       
        [HttpGet("Product/{product}")]
        public async Task<IActionResult> GetCustomerbyProduct(string product)
        {
            var result = new ResponseBase<List<CustomerModel>>();
            result = await new CustomerService(oracleConn).GetCustomerbyProduct(product);

            if (result.Data.Count == 0) result.Code = Status.NotFound;

            return this.Result(result.Code, result);
        }
        
        /// <summary>
        /// Crea un cliente
        /// </summary>
        /// <param name="data">Datos del cliente a crear</param>
        /// <returns>Devuelve un objeto que indica si el cliente fue creado</returns>
        /// <response code="422">Invalid Data</response>
        [ProducesResponseType(200)]
        [ProducesResponseType(422)]
        [Authorize]
		[HttpPost]
		public async Task<IActionResult> InsertCustomer([FromBody] CustomerModel data)
		{
			var result = new ResponseBase<bool>();
			result = await new CustomerService(oracleConn).InsertCustomer(data);

			return this.Result(result.Code, result);
		}

        /// <summary>
        /// Autentica un cliente
        /// </summary>
        /// <param name="data">Datos del cliente que se necesitan para autenticar</param>
        /// <returns>Devuelve un objeto que indica si el cliente tiene o no acceso</returns>
        /// <response code="422">Invalid Data</response>
        [ProducesResponseType(200)]
        [ProducesResponseType(422)]
        [Authorize]
		[HttpPost("login")]
		public async Task<IActionResult> LoginCustomer([FromBody] CustomerAuthModel data)
		{
			var result = new ResponseBase<CustomerModel>();
			result = await new CustomerService(oracleConn).LoginCustomer(data);

			if (result.Data != null && result.Data.CustId == -1) result.Code = Status.NotFound;

			return this.Result(result.Code, result);
		}

        /// <summary>
        /// Actualiza un cliente
        /// </summary>
        /// <param name="data">Datos del cliente que se va a actualizar</param>
        /// <returns>Devuelve un objeto con la información del cliente actualizado</returns>
        /// <response code="422">Invalid Data</response>
        [ProducesResponseType(200)]
        [ProducesResponseType(422)]
        [Authorize]
		[HttpPut]
		public async Task<IActionResult> UpdateCustomer([FromBody] CustomerModel data)
		{
			var result = new ResponseBase<bool>();
			result = await new CustomerService(oracleConn).UpdateCustomer(data);

			return this.Result(result.Code, result);
		}

        /// <summary>
        /// Elimina un cliente por Id
        /// </summary>
        /// <param name="id">Id del cliente a eliminar</param>
        /// <returns>Devuelve el resultado de la eliminación del cliente</returns>
        /// <response code="422">Invalid Data</response>
        [ProducesResponseType(200)]
        [ProducesResponseType(422)]
        [Authorize]
		[HttpDelete]
		public async Task<IActionResult> DeleteCustomer(long id)
		{
			var result = new ResponseBase<bool>();
			result = await new CustomerService(oracleConn).DeleteCustomer(id);

			return this.Result(result.Code, result);
		}
	}
}