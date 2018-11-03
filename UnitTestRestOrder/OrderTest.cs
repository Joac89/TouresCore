using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using TouresCommon;
using TouresRestOrder.Model;
using TouresRestOrder.Service;

namespace UnitTestRestOrder
{
    [TestClass]
    public class OrderTest
    {
        [TestMethod]
        public void InsertOrder()
        {
            var result = new ResponseBase<bool>();
            var data = new OrderModel();
            try
            {
                data = new OrderModel()
                {
                    OrdenDate = Convert.ToDateTime("1999-01-01"),
                    Status = "1",
                    Comments = "prueba",
                    CustId = 1,
                    IdEstado = 1,
                    LItems = new System.Collections.Generic.List<ItemModel>(),
                    Price = 1000
                };
                for (var x = 0; x < 10; ++x)
                {
                    data.LItems.Add(new ItemModel()
                    {
                        ItemId = x
                    });
                }
            }
            catch (Exception ex)
            {
                result.Message = ex.Message;
            }

            result = new OrderMock().InsertOrder(data).Result;
            Assert.IsTrue(result.Code == Status.Ok, result.Message);
        }

        [TestMethod]
        public void GetOrders()
        {
            var custId = 1L;

            var result = new OrderMock().GetOrders(custId).Result;
            Assert.IsTrue(result.Code == Status.Ok || result.Code == Status.NotFound, result.Message);
        }

        [TestMethod]
        public void GetItemFromOrder()
        {
            var idOrden = 1L;

            var result = new OrderMock().GetItemFromOrder(idOrden).Result;
            Assert.IsTrue(result.Code == Status.Ok || result.Code == Status.NotFound, result.Message);
        }

        [TestMethod]
        public void ActualizaEstadoOrder()
        {
            var idOrden = 1;
            var idEstado = 1;

            var result = new OrderMock().ActualizaEstadoOrder(idOrden, idEstado).Result;
            Assert.IsTrue(result.Code == Status.Ok || result.Code == Status.NotFound, result.Message);
        }
    }
}
