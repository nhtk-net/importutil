using System;
using System.Collections.Generic;
using System.Data;

namespace ImportUtil
{
	public class BaseGets
	{
		#region Get Functions
		//////////////////////////////////////////////////////////////////////////////
		protected int GetInt(DataRow pdrRow, string psColName)
		{
			try{
				return (pdrRow[psColName] != DBNull.Value) ? Convert.ToInt32(pdrRow[psColName]) : 0;
			}catch{return 0;}
		}

		//////////////////////////////////////////////////////////////////////////////
		/*protected DateTime GetDateTime(DataRow pdrRow, string psColName)
		{
			try{
				return (pdrRow[psColName] != DBNull.Value) ? Convert.ToDateTime(pdrRow[psColName]) : Constants.NullDateTime;
			}catch{return Constants.NullDateTime;}
		}*/
		protected DateTime? GetDateTime(DataRow pdrRow, string psColName)
		{
			DateTime? lDate = null;
			try{
				if (pdrRow[psColName] != DBNull.Value) 
					lDate = Convert.ToDateTime(pdrRow[psColName]);
			}catch{}
			return lDate;
		}
		//////////////////////////////////////////////////////////////////////////////
		protected Decimal GetDecimal(DataRow pdrRow, string psColName)
		{
			try{
				return (pdrRow[psColName] != DBNull.Value) ? Convert.ToDecimal(pdrRow[psColName]) : 0;
			}catch{return 0;}
		}

		//////////////////////////////////////////////////////////////////////////////
		protected bool GetBool(DataRow pdrRow, string psColName)
		{
			try{
				return (pdrRow[psColName] != DBNull.Value) ? Convert.ToBoolean(pdrRow[psColName]) : false;
			}catch{return false;}
		}

		//////////////////////////////////////////////////////////////////////////////
		protected string GetString(DataRow pdrRow, string psColName)
		{
			try{
				return (pdrRow[psColName] != DBNull.Value) ? Convert.ToString(pdrRow[psColName]).Trim() : Constants.NullString;
			}catch{return Constants.NullString;}
		}

		//////////////////////////////////////////////////////////////////////////////
		protected double GetDouble(DataRow pdrRow, string psColName)
		{
			try{
				return (pdrRow[psColName] != DBNull.Value) ? Convert.ToDouble(pdrRow[psColName]) : 0;
			}catch{return 0;}
		}

		//////////////////////////////////////////////////////////////////////////////
		protected float GetFloat(DataRow pdrRow, string psColName)
		{
			try{
				return (pdrRow[psColName] != DBNull.Value) ? Convert.ToSingle(pdrRow[psColName]) : 0;
			}catch{return 0;}
		}

		//////////////////////////////////////////////////////////////////////////////
		protected long GetLong(DataRow pdrRow, string psColName)
		{
			try{
				return (pdrRow[psColName] != DBNull.Value) ? (long)(pdrRow[psColName]) : 0;
			}catch{return 0;}
		}
		
		//////////////////////////////////////////////////////////////////////////////
        protected byte GetByte(DataRow pdrRow, string psColName)
        {
			try{
				return (pdrRow[psColName] != DBNull.Value) ? Convert.ToByte(pdrRow[psColName]) : (byte)0;
			}catch{return 0;}
        }
	#endregion
	}
}
