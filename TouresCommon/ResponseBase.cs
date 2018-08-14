using System;

namespace TouresCommon
{
    public class ResponseBase<T>
    {
		public int Code { get; set; }
		public string Message { get; set; }
		public T Data { get; set; }
    }
}
