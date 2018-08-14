using System.Collections.Generic;
using System.Data;
using TouresApiProduct.Model;
using TouresCommon;
using TouresDataAccess;

namespace TouresApiProduct.Dao
{
	/// <summary>
	/// CRUD para Productos
	/// </summary>
	public class ProductDao
	{
		public ResponseBase<List<OutputProduct>> Consult(InputProduct data)
		{
			var dao = new SqlAccess(Properties.Resources.SqlConnection);
			var lst = new List<OutputProduct>();
			var result = new ResponseBase<List<OutputProduct>>();

			dao.Parameter("id", (int)SqlDbType.Int, data.Id);

			var response = dao.Read("usp_product_consult");
			if (dao.State.Code == ResponseCode.Ok)
			{
				while (response.Read())
				{
					var item = new OutputProduct()
					{
						Id = int.Parse(response["id"].ToString()),
						Name = response["Name"].ToString()
					};
					lst.Add(item);
				}
				result.Data = lst;
			}
			result.Code = dao.State.Code;
			result.Message = result.Message;

			return result;
		}

		public ResponseBase<bool> Register(InputProduct data)
		{
			var dao = new SqlAccess(Properties.Resources.SqlConnection);
			var result = new ResponseBase<bool>();

			dao.Parameter("id", (int)SqlDbType.Int, data.Id);
			dao.Parameter("name", (int)SqlDbType.NVarChar, data.Name, 50);
					
			result.Data = dao.Save("usp_product_create");
			result.Code = dao.State.Code;
			result.Message = dao.State.Message;

			return result;
		}

		public ResponseBase<int> Update(InputProduct data)
		{
			var dao = new SqlAccess(Properties.Resources.SqlConnection);
			var result = new ResponseBase<int>();

			dao.Parameter("id", (int)SqlDbType.Int, data.Id);
			dao.Parameter("name", (int)SqlDbType.NVarChar, data.Name, 50);
			dao.Parameter("codeupdate", (int)SqlDbType.Int, ParameterDirection.Output);

			var response = dao.SaveOutput("usp_product_update");
			var codeupdate = (int)response["codeupdate"].Value;

			result.Data = codeupdate;
			result.Code = dao.State.Code;			
			result.Message = dao.State.Message;
			
			return result;
		}

		public ResponseBase<bool> Delete(int idProduct)
		{
			var dao = new SqlAccess(Properties.Resources.SqlConnection);
			var result = new ResponseBase<bool>();

			dao.Parameter("id", (int)SqlDbType.Int, idProduct);

			result.Data = dao.Save("usp_product_delete");
			result.Code = dao.State.Code;
			result.Message = dao.State.Message;

			return result;
		}
	}
}
