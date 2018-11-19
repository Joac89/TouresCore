using System;
using System.ComponentModel.DataAnnotations;

namespace TouresRestCustomer.Model
{
	public class CustomerModel
	{
		[Required(AllowEmptyStrings = false)]
		[StringLength(40, MinimumLength = 5)]
		public string FName { get; set; }

		[Required(AllowEmptyStrings = false)]
		[StringLength(40, MinimumLength = 5)]
		public string LName { get; set; }

		[StringLength(40, MinimumLength = 10)]
		[Required(AllowEmptyStrings = false)]
		public string PhoneNumber { get; set; }

		[Required(AllowEmptyStrings = false)]
		[StringLength(40, MinimumLength = 6)]
		[RegularExpression("^[a-zA-Z0-9.!#$%&'*+/=?^_`{|}~-]+@[a-zA-Z0-9](?:[a-zA-Z0-9-]{0,61}[a-zA-Z0-9])?(?:.[a-zA-Z0-9](?:[a-zA-Z0-9-]{0,61}[a-zA-Z0-9])?)*$")]
		public string Email { get; set; }

		[Required(AllowEmptyStrings = false)]
		[StringLength(40, MinimumLength = 5)]
		public string Password { get; set; }

		[Required(AllowEmptyStrings = false)]
		[StringLength(40, MinimumLength = 1)]
		public string CreditCardType { get; set; }

		[Required(AllowEmptyStrings = false)]
		[StringLength(40, MinimumLength = 14)]
		public string CreditCardNumber { get; set; }
		
		[Required(AllowEmptyStrings = false)]
		[StringLength(20, MinimumLength = 5)]
		[RegularExpression("^(0|[1-9][0-9]*)$")]
		public string DocNumber { get; set; }

        public string ClientType { get; set; }

		public long CustId { get; set; }
		public string UserName { get; set; }
		public string Address { get; set; }
		public string Status { get; set; }
        public string TipoCliente { get; set; }
        public string ordid { get; set; }
        public string itemid { get; set; }
        
    }
}
