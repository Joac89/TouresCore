using System;
using System.ComponentModel.DataAnnotations;

namespace TouresRestOrder.Model
{
	public class ItemModel
	{
		public long ItemId { get; set; }

		[Required(AllowEmptyStrings = false)]
		[Range(0, long.MaxValue)]
		public long OrdId { get; set; }

		[Required(AllowEmptyStrings = false)]
		[Range(0, long.MaxValue)]
		public long ProdId { get; set; }

		[Required(AllowEmptyStrings = false)]
		[StringLength(50, MinimumLength = 5)]
		public string ProductName { get; set; }

		[Required(AllowEmptyStrings = false)]
		[StringLength(20, MinimumLength = 5)]
		public string PartNum { get; set; }

		[Required(AllowEmptyStrings = false)]
		[Range(1.0, (double)decimal.MaxValue)]
		public decimal Price { get; set; }

		[Required(AllowEmptyStrings = false)]
		[Range(1, 999999999)]
		public int Quantity { get; set; }
	}
}
