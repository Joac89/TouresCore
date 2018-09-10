using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using System;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;
using TouresCommon;
using TouresRestCustomer.Model;
using TouresRestCustomer.Service;

namespace TouresApiExample.Controllers
{
	[Produces("application/json")]
	[Route("api/v1/Customer")]
	public class CustomerController : Controller
	{
		private IConfiguration config;

		public CustomerController(IConfiguration configuration)
		{
			config = configuration;
		}

		[AllowAnonymous]
		[HttpPost]
		public async Task<IActionResult> GetCustomer([FromBody] CustomerModel data)
		{
			var result = new ResponseBase<Customer>();
			var customer = new ResponseBase<Customer>();

            customer = await new CustomerService(config["oracleConnection"]).GetCustomer(data);

			if (customer.Code == Status.Ok && customer.Data.CUSTID != 0)
			{
				var claims = new[]
				{
					new Claim(JwtRegisteredClaimNames.Sub, data.DocNumber),
					new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString())
				};

				var token = new JwtSecurityToken
				(
				  issuer: config["token:issuer"],
					audience: config["token:audience"],
					claims: claims,
					expires: DateTime.UtcNow.AddHours(double.Parse(config["token:expire"])),
					notBefore: DateTime.UtcNow,
					signingCredentials: new SigningCredentials(new SymmetricSecurityKey(Encoding.UTF8.GetBytes(config["token:signingkey"])), SecurityAlgorithms.HmacSha256)
				);
                
                result.Code = Status.Ok;
                result.Data = customer.Data;
                result.Message = customer.Message;

				return Ok(result);
			}
			else
			{
				result.Code = TouresCommon.Status.Unauthorized;
				result.Message = customer.Message;
				result.Data = customer.Data;

				return StatusCode(result.Code, result);
			}
		}

        [AllowAnonymous]
        [HttpPost]
        public async Task<IActionResult> InsertCustomer([FromBody] Customer data)
        {
            var result = new ResponseBase<Boolean>();
            result = await new CustomerService(config["oracleConnection"]).InsertCustomer(data);
            return Ok(result);

        }

        [AllowAnonymous]
        [HttpPost]
        public async Task<IActionResult> UpdateCustomer([FromBody] Customer data)
        {
            var result = new ResponseBase<Boolean>();
            result = await new CustomerService(config["oracleConnection"]).UpdateCustomer(data);
            return Ok(result);

        }
    }
}