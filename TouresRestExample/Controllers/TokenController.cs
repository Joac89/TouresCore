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
using TouresRestExample.Model;
using TouresRestExample.Service;

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
			var result = new ResponseBase<string>();
			var userAuth = new ResponseBase<AuthenticateResponse>();

			userAuth = await new AuthenticateService(config["sqlConnection"]).Authenticate(data);

			if (userAuth.Code == Status.Ok && userAuth.Data.Id != 0)
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

				result.Code = Status.Ok;
				result.Data = new JwtSecurityTokenHandler().WriteToken(token);
				result.Message = userAuth.Message;

				return Ok(result);
			}
			else
			{
				result.Code = TouresCommon.Status.Unauthorized;
				result.Message = userAuth.Message;
				result.Data = "";

				return StatusCode(result.Code, result);
			}
		}
	}
}