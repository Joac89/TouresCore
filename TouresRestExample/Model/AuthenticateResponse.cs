﻿using System;

namespace TouresRestExample.Model
{
	public class AuthenticateResponse
    {
		public long Id { get; set; }
		public string Names { get; set; }
		public string Surnames { get; set; }
		public DateTime BirthDate { get; set; }
		public int Age { get; set; }
    }
}
