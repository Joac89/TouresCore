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
			var result = new ResponseBase<CustomerModel>();
			result = new CustomerMock().LoginCustomer(data).Result;

			if (result.Data != null && result.Data.CustId == -1) result.Code = Status.NotFound;

			Assert.IsTrue(result.Code == Status.Ok, result.Message);

		}
	}
}
