using Microsoft.AspNetCore.Mvc;

namespace TouresCommon
{
	class ResponseController: ControllerBase
    {
		public ObjectResult HttpStatus<T>(int code, T result)
		{
			return code == Status.Ok ? base.Ok(result) : base.StatusCode(code, result);
		}		
	}

	public static class ExtensionController
	{
		public static ObjectResult Result<T>(this ControllerBase controller, int code, T result)
		{
			var ctrl = new ResponseController();

			return ctrl.HttpStatus(code, result);
		}
	}
}
