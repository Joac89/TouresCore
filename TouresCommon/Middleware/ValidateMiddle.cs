using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace TouresCommon.Middleware
{
	public static class ValidateMiddle
	{		
		public static ValidateResponse Result<T>(T data)
		{
			var context = new ValidationContext((object)data, null, null);
			var results = new List<ValidationResult>();
			var valid = Validator.TryValidateObject((object)data, context, results, true);
			var errors = "";

			foreach (var item in results) errors += $"{item.ErrorMessage},";
			errors = errors != "" ? errors.Substring(0, errors.Length - 1) : "";

			return new ValidateResponse()
			{
				Status = valid,
				Message = errors
			};
		}
	}
}
