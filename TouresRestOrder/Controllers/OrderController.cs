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
using TouresRestOrder.Model;
using TouresRestOrder.Service;

namespace TouresRestOrder.Controllers
{
    [Produces("application/json")]
    [Route("api/v1/Order")]
    public class OrderController : Controller
    {
        private IConfiguration config;

        public OrderController(IConfiguration configuration)
        {
            config = configuration;
        }


        [AllowAnonymous]
        [HttpPost]
        public async Task<IActionResult> InsertOrder([FromBody] Order data)
        {
            var result = new ResponseBase<Boolean>();
            result = await new OrderService(config["oracleConnection"]).InsertOrder(data);
            return Ok(result);
           
        }
    }
}