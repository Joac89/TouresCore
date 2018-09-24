using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Cors;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using System.Threading.Tasks;
using TouresAuthenticate.Model;
using TouresAuthenticate.Service;
using TouresCommon;

namespace TouresRestCustomer.Controllers
{
	[Produces("application/json")]
	[Route("api/v1/Token")]
	[EnableCors("*")]
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

			if (userAuth.Code == Status.Ok && userAuth.Data.Id != 0)
			{
				var jwtImpl = new JwtService();
				var jwtToken = jwtImpl.SetJWT(data.UserName, new JwtModel()
				{
					Issuer = config["token:issuer"],
					Audience = config["token:audience"],
					Expire = config["token:expire"],
					SigningKey = config["token:signingkey"]
				});
				userAuth.Data.Token = jwtToken.Token;

				result.Code = jwtToken.Status ? Status.Ok : Status.InternalError;
				result.Data = jwtToken.Status ? userAuth.Data : null;
				result.Message = userAuth.Message;

				return Ok(result);
			}
			else
			{
				result.Code = Status.Unauthorized;
				result.Message = userAuth.Message;
				result.Data = userAuth.Data;

				return StatusCode(result.Code, result);
			}
		}
	}
}