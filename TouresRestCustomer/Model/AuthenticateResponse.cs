using System;

namespace TouresRestCustomer.Model
{
	public class AuthenticateResponse
    {
        //public long Id { get; set; }
        //public string Names { get; set; }
        //public string Surnames { get; set; }
        //public DateTime BirthDate { get; set; }
        //public int Age { get; set; }

        public long CUSTID { get; set; }
        public string FNAME { get; set; }
        public string LNAME { get; set; }
        public string PHONENUMBER { get; set; }
        public string EMAIL { get; set; }
        public string PASSWORD { get; set; }
        public string CREDITCARDTYPE { get; set; }
        public string CREDITCARDNUMBER { get; set; }
        public string STATUS { get; set; }
        public string token { get; set; }
    }
}
