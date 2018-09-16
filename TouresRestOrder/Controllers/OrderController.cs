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
using System.Collections.Generic;

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

        #region Publicas

        [AllowAnonymous]
        [HttpPost]
        public async Task<IActionResult> InsertOrder([FromBody] Order data)
        {
            try
            {
                var result = new ResponseBase<Boolean>();

                //data.LItems = new System.Collections.Generic.List<Item>();
                //Item objItem = new Item();
                //objItem.PartNum = "2" ;
                //objItem.Price = 50000;
                //objItem.ProdId = 1;
                //objItem.ProductName = "MUNDIAL";
                //objItem.Quantity = 8;
                //data.LItems.Add(objItem);
                string mensaje = string.Empty;
                if (ValidaOrden(data, ref mensaje))
                {
                    result = await new OrderService(config["oracleConnection"]).InsertOrder(data);
                    return Ok(result);
                }
                else
                {
                    return Ok(new ResponseBase<Boolean>()
                    {
                        Code = 500,
                        Data = false,
                        Message = mensaje
                    });
                }
            }
            catch (Exception ex)
            {

                throw ex;
            }
            
        }

        [AllowAnonymous]
        [HttpGet]
        public async Task<IActionResult>  GetOrder()
        {
            try
            {
                var result = new ResponseBase<List<Order>>();


                result = await new OrderService(config["oracleConnection"]).GetOrder();

                if (result.Code == Status.Ok)
                {
                    return Ok(result);
                }
                else
                {
                    result.Code = TouresCommon.Status.Unauthorized;
                    return StatusCode(result.Code, result);
                }
            }
            catch (Exception ex)
            {

                throw ex;
            }
           
        }

        [AllowAnonymous]
        [HttpGet]
        public async Task<IActionResult> GetItem([FromBody] int IdOrder)
        {
            try
            {
                var result = new ResponseBase<List<Item>>();


                result = await new OrderService(config["oracleConnection"]).GetItem(IdOrder);

                if (result.Code == Status.Ok)
                {
                    return Ok(result);
                }
                else
                {
                    result.Code = TouresCommon.Status.Unauthorized;
                    return StatusCode(result.Code, result);
                }
            }
            catch (Exception ex)
            {

                throw ex;
            }

        }
        #endregion


        private Boolean ValidaOrden(Order ObjOrden, ref string Error)
        {
            Boolean retorno = true;
            if (ObjOrden.CustId == 0)
            {
                Error = "El cliente ingresado no es valido.";
                retorno = false;
            }
            if (ObjOrden.Price == 0)
            {
                Error += " El precio ingresado no es valido.";
                retorno = false;
            }
            if (ObjOrden.LItems == null || ObjOrden.LItems.Count == 0)
            {
                Error += " La orden debe tener al menos un producto asociado.";
                retorno = false;
            }
            foreach (var item in ObjOrden.LItems)
            {
                if (item.ProductName == string.Empty)
                {
                    Error += " El producto no es valido.";
                    retorno = false;
                }
                if (item.ProdId == 0)
                {
                    Error += " El codigo del producto " + item.ProductName + " no es valido.";
                    retorno = false;
                }
                if (item.Price == 0)
                {
                    Error += " El precio del producto " + item.ProductName + " no es valido.";
                    retorno = false;
                }
                if (item.Quantity == 0)
                {
                    Error += " La cantidad del producto " + item.ProductName + " no es valida.";
                    retorno = false;
                }

            }
            return retorno;
        }
    }
}