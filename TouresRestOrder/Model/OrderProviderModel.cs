using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace TouresRestOrder.Model
{
	public class OrderProviderModel
	{
		public long OrdId { get; set; }
        public long ItemId { get; set; }
        public string Status { get; set; }
        public string Provider { get; set; }
        public long ConfirmationId { get; set; }
        
	}
}
