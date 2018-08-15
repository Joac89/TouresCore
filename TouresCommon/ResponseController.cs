using Microsoft.AspNetCore.Mvc;

namespace TouresCommon
{
	public class ResponseController: ControllerBase
    {
		public ObjectResult Message<T>(int code, T result)
		{
			return code == TouresCommon.Status.Ok ? base.Ok(result) : base.StatusCode(code, result);
		}
	}
}
