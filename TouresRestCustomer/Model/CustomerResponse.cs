using System;

namespace TouresRestCustomer.Model
{
	public class CustomerResponse
    {        
        public long CUSTID { get; set; }
        public string FNAME { get; set; }
        public string LNAME { get; set; }
        public string PHONENUMBER { get; set; }
        public string EMAIL { get; set; }
        public string PASSWORD { get; set; }
        public string CREDITCARDTYPE { get; set; }
        public string CREDITCARDNUMBER { get; set; }
        public string STATUS { get; set; }
        public string DOCNUMBER { get; set; }
        public string USERNAME { get; set; }
    }
}
