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
    [Route("api/v1/Report")]
    public class ReportController : Controller
    {
        private IConfiguration config;
        private string oracleConn;

        public ReportController(IConfiguration configuration)
        {
            config = configuration;
            oracleConn = config["oracleConnection"];
        }

        /// <summary>
        /// Obtiene las ordenes de un usuario por id
        /// </summary>
        /// <param name="customer">Id de usuario</param>
        /// <returns>Devuelve un objeto con la información de las ordenes consultadas</returns>
        /// <response code="422">Invalid Data</response>
        [ProducesResponseType(200)]
        [ProducesResponseType(422)]
        [HttpGet("{tipo}")]
        public async Task<IActionResult> GetOrders(int tipo)
        {
            var result = new ResponseBase<List<ReportOrdenModel>>();
            switch (tipo)
            {
                case 1:
                case 2:
                    result = await new ReportService(oracleConn).GetReportOrders(tipo);
                    break;
                default:
                    Console.WriteLine("Default case");
                    break;
            }
            

            return this.Result(result.Code, result);
        }

    }
}