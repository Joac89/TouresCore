using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace TouresRestCustomer.Model
{
	public class CustomerAuthModel
	{
		[Required(AllowEmptyStrings = false)]
		[StringLength(20, MinimumLength = 5)]
		public string Username { get; set; }

		[Required(AllowEmptyStrings = false)]
		[StringLength(40, MinimumLength = 6)]
		public string Password { get; set; }
	}
}
