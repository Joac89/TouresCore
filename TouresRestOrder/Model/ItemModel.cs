﻿using System;

namespace TouresRestOrder.Model
{
    public class ItemModel
    {
        public long ItemId {get;set;}
        public long OrdId { get; set; }
        public long ProdId { get; set; }
        public string ProductName { get; set; }
        public string PartNum { get; set; }
        public decimal Price { get; set; }
        public int Quantity { get; set; }
        public Estado IdEstado { get; set; }
        public string Status { get; set; }
    }
}
