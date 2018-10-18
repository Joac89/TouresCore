using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace TouresRestOrder.Model
{
	public class OrderModel
	{
		public long OrdId { get; set; }

		[Required(AllowEmptyStrings = false)]
		[Range(0, long.MaxValue)]
		public long CustId { get; set; }

		[Required(AllowEmptyStrings = false)]
		public DateTime OrdenDate { get; set; }

		[Required(AllowEmptyStrings = false)]
		[Range(1.0, (double)decimal.MaxValue)]
		public decimal Price { get; set; }

		[Required(AllowEmptyStrings = false)]
		[StringLength(10)]
		public string Status { get; set; }

		[Required(AllowEmptyStrings = false)]
		[StringLength(4000)]
		public string Comments { get; set; }

		[Required(AllowEmptyStrings = false)]
		public List<ItemModel> LItems { get; set; }
	}
}
