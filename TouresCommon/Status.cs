using System;
using System.Collections.Generic;
using System.Text;

namespace TouresCommon
{
    public static class Status
    {
		public const int Ok = 200;
		public const int InternalError = 500;
		public const int ConnectionDbError = 503;
		public const int NotFound = 404;
		public const int Forbiden = 403;
		public const int Unauthorized = 407;
    }
}
