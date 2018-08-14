using System.Data;
using System.Data.Common;
using TouresCommon;

namespace TouresDataAccess
{
	public interface IDataAccess
	{
		ResponseState State { get; set; }
		bool Save(string storeProcedure);
		DbParameterCollection SaveOutput(string storeProcedure);
		DbDataReader Read(string StoreProcedure);
		bool Parameter<T>(string name, int type, T value);
		bool Parameter<T>(string name, int type, T value, int size);
		bool Parameter<T>(string name, int type, ParameterDirection direction);
		bool Open();
		void Close();
	}
}
