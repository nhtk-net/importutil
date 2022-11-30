using System;
using System.Collections.Generic;
using System.Data;

namespace ImportUtil {
	public abstract class BaseObjectCollection<T> : List<T> where T : BaseBusinessObject, new(){
	#region Map Data
		public bool MapObjects(DataSet pdsData)
        {
            if (pdsData != null && pdsData.Tables.Count > 0)
            {
                return MapObjects(pdsData.Tables[0]);
            }
            else
            {
                return false;
            }
        }
		/////////////////////////////////////////////////////////////////////////
        public bool MapObjects(DataTable pdtData)
        {
            Clear();
            for (int i = 0; i<pdtData.Rows.Count; i++)
            {
                T loData = new T();
                loData.MapData(pdtData.Rows[i]);
                this.Add(loData);
            }
            return true;
        }
	#endregion //Map Data
	}
}
