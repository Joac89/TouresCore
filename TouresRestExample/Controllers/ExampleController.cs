using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Cors;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using System.Threading.Tasks;
using TouresCommon;
using TouresRestExample.Common;
using TouresRestExample.Dao;
using TouresRestExample.Model;

namespace TouresRestExample.Controllers
{
	[Route("api/v1/[controller]")]
	[Produces("application/json")]
	[EnableCors("*")]
	public class ExampleController : Controller
	{
		private IConfiguration config;
		private ResponseController httpResult;
		private string connString = "";

		public ExampleController(IConfiguration configuration)
		{
			config = configuration;
			httpResult = new ResponseController();
			connString = config["sqlConnection"];
		}
				
		[HttpPost]
		[Authorize]
		[ValidateModel]
		public async Task<IActionResult> Create([FromBody] ExampleModel data)
		{
			var service = new ExampleService(connString);
			var result = await service.Create(data);

			return httpResult.Message(result.Code, result);
		}

		[HttpGet]
		[Authorize]
		[ValidateModel]
		public async Task<IActionResult> All()
		{
			var service = new ExampleService(connString);
			var result = await service.All();

			return httpResult.Message(result.Code, result);
		}

		[HttpGet("{id}")]
		[Authorize]
		[ValidateModel]
		public async Task<IActionResult> Read(long id)
		{
			var service = new ExampleService(connString);
			var result = await service.Read(id);

			return httpResult.Message(result.Code, result);
		}

		[HttpPut("{id}")]
		[Authorize]
		[ValidateModel]
		public async Task<IActionResult> Update(long id, [FromBody] ExampleModel data)
		{
			data.Id = id;

			var service = new ExampleService(connString);
			var result = await service.Update(data);

			return httpResult.Message(result.Code, result);
		}

		[HttpDelete("{id}")]
		[Authorize]
		[ValidateModel]
		public async Task<IActionResult> Delete(long id)
		{
			var service = new ExampleService(connString);
			var result = await service.Delete(id);

			return httpResult.Message(result.Code, result);
		}
	}
}
