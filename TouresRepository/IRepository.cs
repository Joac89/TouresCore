using System.Collections.Generic;
using System.Data.Common;
using TouresCommon;

namespace TouresRepository
{
	public interface IRepository<T>
    {
		T Parameters { get; set; }
		StatusResponse Status { get; set; }
		void SaveChanges(string storeProcedure);
		List<IDictionary<string, object>> Get(string storeProcedure);
	}
}
