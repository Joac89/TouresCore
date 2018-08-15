﻿using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using System.Net;
using TouresCommon;

namespace TouresRestExample.Common
{
	public class ValidateModelAttribute : ActionFilterAttribute
	{
		public override void OnActionExecuted(ActionExecutedContext context)
		{
			//throw new NotImplementedException();
		}

		public override void OnActionExecuting(ActionExecutingContext actionContext)
		{
			if (!actionContext.ModelState.IsValid)
			{
				var result = new ResponseBase<BadRequestObjectResult>()
				{
					Code = (int)HttpStatusCode.BadRequest,
					Data = new BadRequestObjectResult(actionContext.ModelState),
					Message = "Invalid Model"
				};
				actionContext.Result = new BadRequestObjectResult(result);
			}
		}
	}
}
