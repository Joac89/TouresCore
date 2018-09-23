using System;
using System.Collections.Generic;
using System.Text;

namespace TouresAuthenticate.Model
{
    public class JwtModel
    {
		public string Issuer { get; set; }
		public string Audience { get; set; }
		public string SigningKey { get; set; }
		public string Expire { get; set; }
	}
}
