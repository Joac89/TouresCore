using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace TouresRestOrder.Model
{
    public class ReportOrdenModel
    {
        public long ordid { get; set; }
        public string fname { get; set; }
        public DateTime ordendate { get; set; }
        public double price { get; set; }
        public string comments { get; set; }
        public string nombre_estado { get; set; }
    }

    public class ReportClienteModel
    {
        public string fname { get; set; }
        public double Total { get; set; }
        public int custid { get; set; }
    }

    public class ReportProductModel
    {
        public string productname { get; set; }
        public int Cantidad { get; set; }
    }
}
