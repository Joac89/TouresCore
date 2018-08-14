using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using TouresApiProduct.Dao;
using TouresApiProduct.Model;
using TouresCommon;

namespace TouresApiProduct.Controllers
{
	[Route("api/[controller]")]
	public class ProductController : Controller
	{
		[HttpGet]
		[Route("/api/consult")]
		public ResponseBase<List<OutputProduct>> Consult(InputProduct data)
		{
			var model = new ProductDao();
			var result = model.Consult(data);

			return result;
		}

		[HttpPost]
		public ResponseBase<bool> Register([FromBody] InputProduct data)
		{
			var model = new ProductDao();
			var result = model.Register(data);

			return result;
		}

		[HttpPut]
		public ResponseBase<int> Update([FromBody] InputProduct data)
		{
			var model = new ProductDao();
			var result = model.Update(data);

			return result;
		}

		[HttpDelete()]
		public ResponseBase<bool> Delete(int id)
		{
			var model = new ProductDao();
			var result = model.Delete(id);

			return result;
		}
	}
}
