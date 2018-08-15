using System.Collections.Generic;
using System.Data.Common;
using TouresCommon;

namespace TouresDataAccess
{
	public interface IRepository<T>
    {
		T Parameters { get; set; }
		ResponseStatus Status { get; set; }
		void SaveChanges(string storeProcedure);
		List<IDictionary<string, object>> Get(string storeProcedure);
	}
}
