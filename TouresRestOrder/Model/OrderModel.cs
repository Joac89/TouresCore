using System;
using System.Collections.Generic;

namespace TouresRestOrder.Model
{
    public class OrderModel
    {
        public long OrdId { get; set; }
        public long CustId { get; set; }
        public DateTime OrdenDate { get; set; }
        public decimal Price { get; set; }
        public string Status { get; set; }
        public string Comments { get; set; }
        public List<ItemModel> LItems { get; set; }
    }
}
