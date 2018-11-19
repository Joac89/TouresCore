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
        [HttpGet("{tipo}/{fecha1}/{fecha2}")]
        public async Task<IActionResult> GetOrders(int tipo, string fecha1, string fecha2)
        {
            
            switch (tipo)
            {
                case 1:
                case 2:
                    var result = new ResponseBase<List<ReportOrdenModel>>();
                    result = await new ReportService(oracleConn).GetReportOrders(tipo, fecha1, fecha2);
                    return this.Result(result.Code, result);
                case 3:
                    var resultCli = new ResponseBase<List<ReportClienteModel>>();
                    resultCli = await new ReportService(oracleConn).GetReportClientes(tipo, fecha1, fecha2);
                    return this.Result(resultCli.Code, resultCli);
                default:
                    var resultdef = new ResponseBase<List<ReportOrdenModel>>();
                    resultdef = await new ReportService(oracleConn).GetReportOrders(tipo, fecha1, fecha2);
                    return this.Result(resultdef.Code, resultdef);
            }
            

            
        }


        /// <summary>
        /// Obtiene las ordenes de un usuario por id
        /// </summary>
        /// <param name="customer">Id de usuario</param>
        /// <returns>Devuelve un objeto con la información de las ordenes consultadas</returns>
        /// <response code="422">Invalid Data</response>
        [ProducesResponseType(200)]
        [ProducesResponseType(422)]
        [HttpGet("cliente/{cusid}/{fecha1}/{fecha2}")]
        public async Task<IActionResult> GetRankingClientes(int cusid, string fecha1, string fecha2)
        {
                    var result = new ResponseBase<List<ReportOrdenModel>>();
                    result = await new ReportService(oracleConn).GetReportRankingClientes(cusid, fecha1, fecha2);
                    return this.Result(result.Code, result);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="customer">Id de usuario</param>
        /// <returns>Devuelve un objeto con la información de las ordenes consultadas</returns>
        /// <response code="422">Invalid Data</response>
        [ProducesResponseType(200)]
        [ProducesResponseType(422)]
        [HttpGet("product/{tipo}/{fecha1}/{fecha2}")]
        public async Task<IActionResult> GetRankingProduct(int tipo, string fecha1, string fecha2)
        {
            var result = new ResponseBase<List<ReportProductModel>>();
            result = await new ReportService(oracleConn).GetReportProducto(tipo, fecha1, fecha2);
            return this.Result(result.Code, result);
        }
    }
}