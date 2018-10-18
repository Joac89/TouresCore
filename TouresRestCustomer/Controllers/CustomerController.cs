using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Cors;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
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

		[Authorize]
		[HttpGet("{document}")]
		public async Task<IActionResult> GetCustomer(string document)
		{
			var result = new ResponseBase<CustomerModel>();
			result = await new CustomerService(oracleConn).GetCustomer(document);

			if (result.Data.CustId == 0) result.Code = Status.NotFound;

			return this.Result(result.Code, result);
		}

		[Authorize]
		[HttpPost]
		public async Task<IActionResult> InsertCustomer([FromBody] CustomerModel data)
		{
			var result = new ResponseBase<bool>();
			result = await new CustomerService(oracleConn).InsertCustomer(data);

			return this.Result(result.Code, result);
		}

		[Authorize]
		[HttpPost("login")]
		public async Task<IActionResult> LoginCustomer([FromBody] CustomerAuthModel data)
		{
			var result = new ResponseBase<CustomerModel>();
			result = await new CustomerService(oracleConn).LoginCustomer(data);

			if (result.Data != null && result.Data.CustId == -1) result.Code = Status.NotFound;

			return this.Result(result.Code, result);
		}

		[Authorize]
		[HttpPut]
		public async Task<IActionResult> UpdateCustomer([FromBody] CustomerModel data)
		{
			var result = new ResponseBase<bool>();
			result = await new CustomerService(oracleConn).UpdateCustomer(data);

			return this.Result(result.Code, result);
		}

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