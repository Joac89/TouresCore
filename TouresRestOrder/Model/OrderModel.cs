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
        /// <summary>
        /// Pendiente = 1,Aprobado = 2,Rechazado = 3,Cancelado = 4
        /// </summary>
        public int IdEstado { get; set; }
        public string Status { get; set; }
        public string Comments { get; set; }
        public List<ItemModel> LItems { get; set; }
    }
}
