using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data;

namespace ImportUtil
{
	public class BaseBO : BaseGets
	{
		#region Map Data
		public virtual bool MapData(DataSet pdsData)
		{
			if (pdsData != null && pdsData.Tables.Count > 0 && pdsData.Tables[0].Rows.Count > 0)
			{
				return MapData(pdsData.Tables[0].Rows[0]);
			}
			else
			{
				return false;
			}
		}

		//////////////////////////////////////////////////////////////////////////////
		public virtual bool MapData(DataTable pdtData)
		{
			if (pdtData != null && pdtData.Rows.Count > 0)
			{
				return MapData(pdtData.Rows[0]);
			}
			else
			{
				return false;
			}
		}

		//////////////////////////////////////////////////////////////////////////////
		public virtual bool MapData(DataRow pdrData)
		{
			//You can put common data mapping items here (e.g. create date, modified date, etc)
			return true;
		}
	#endregion //MapData
	}
}
