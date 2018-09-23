using System;

namespace TouresCommon
{
    public class ResponseBase<T>
    {
		public int Code { get; set; } = 0;
		public string Message { get; set; } = "";
		public T Data { get; set; }
    }
}
