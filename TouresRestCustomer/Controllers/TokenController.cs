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
	[Route("api/v1/Token")]
	public class TokenController : Controller
	{
		private IConfiguration config;

		public TokenController(IConfiguration configuration)
		{
			config = configuration;
		}

		[AllowAnonymous]
		[HttpPost]
		public async Task<IActionResult> GetToken([FromBody] AuthenticateModel data)
		{
			var result = new ResponseBase<AuthenticateResponse>();
			var userAuth = new ResponseBase<AuthenticateResponse>();

			userAuth = await new AuthenticateService(config["oracleConnection"]).Authenticate(data);

			if (userAuth.Code == Status.Ok && userAuth.Data.CUSTID != 0)
			{
				var claims = new[]
				{
					new Claim(JwtRegisteredClaimNames.Sub, data.UserName),
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
                userAuth.Data.token = new JwtSecurityTokenHandler().WriteToken(token);


                result.Code = Status.Ok;
                result.Data = userAuth.Data;
                result.Message = userAuth.Message;

				return Ok(result);
			}
			else
			{
				result.Code = TouresCommon.Status.Unauthorized;
				result.Message = userAuth.Message;
				result.Data = userAuth.Data;

				return StatusCode(result.Code, result);
			}
		}
	}
}