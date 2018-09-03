using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace TouresRestOrder.Model
{
    public class Order
    {
        public long OrdId { get; set; }
        public long CustId { get; set; }
        public DateTime OrdenDate { get; set; }
        public decimal Price { get; set; }
        public string Status { get; set; } 
        public string Comments { get; set; }
    }
}
