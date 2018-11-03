using Microsoft.VisualStudio.TestTools.UnitTesting;
using TouresCommon;
using TouresRestCustomer.Model;
using TouresRestCustomer.Service;

namespace UnitTestRestCustomer
{
    [TestClass]
    public class CustomerTest
    {
        [TestMethod]
        public void LoginCustomer()
        {
            var data = new CustomerAuthModel()
            {
                Username = "peperez",
                Password = "123456"
            };
            
            var result = new CustomerMock().LoginCustomer(data).Result;
            if (result.Data != null && result.Data.CustId == -1) result.Code = Status.NotFound;
            Assert.IsTrue(result.Code == Status.Ok, result.Message);

        }

        [TestMethod]
        public void GetCustomer()
        {
            var document = "123456";
            
            var result = new CustomerMock().GetCustomer(document).Result;
            if (result.Data != null && result.Data.CustId == -1) result.Code = Status.NotFound;
            Assert.IsTrue(result.Code == Status.Ok || result.Code == Status.NotFound, result.Message);
        }

        [TestMethod]
        public void InsertCustomer()
        {
            var data = new CustomerModel()
            {
                Address = "addres prueba",
                CreditCardNumber = "0000000000000000",
                CreditCardType = "MASTERCARD",
                DocNumber = "987654",
                Status = "1",
                Email = "prueba@gmail.com",
                FName = "prueba",
                LName = "prueba",
                PhoneNumber = "3110000000",
                UserName = "prueba",
                Password = "123456"
            };
            
            var result = new CustomerMock().InsertCustomer(data).Result;
            Assert.IsTrue(result.Code == Status.Ok, result.Message);
        }

        [TestMethod]
        public void UpdateCustomer()
        {
            var data = new CustomerModel()
            {
                CustId = 1,
                Address = "addres prueba",
                CreditCardNumber = "0000000000000000",
                CreditCardType = "MASTERCARD",
                DocNumber = "987654",
                Status = "1",
                Email = "prueba@gmail.com",
                FName = "prueba",
                LName = "prueba",
                PhoneNumber = "3110000000",
                UserName = "prueba",
                Password = "123456"
            };
            
            var result = new CustomerMock().UpdateCustomer(data).Result;
            Assert.IsTrue(result.Code == Status.Ok, result.Message);
        }

        [TestMethod]
        public void DeleteCustomer()
        {
            var id = 1L;           
            
            var result = new CustomerMock().DeleteCustomer(id).Result;
            Assert.IsTrue(result.Code == Status.Ok, result.Message);
        }
    }
}
