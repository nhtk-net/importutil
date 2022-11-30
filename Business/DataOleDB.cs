using System;
using System.Data;
using System.Collections.Generic;
using DAL = ImportUtil.DataAccess;
using System.Data.OleDb;

namespace ImportUtil
{
	public class DataOleDB : BaseBusinessObjectOleDB
	{
	#region Get Data
		public static DataTable GetData(string psSql, IDbConnection Cxn){
			return new DAL.DataOleDB(Cxn).GetData(psSql);
		}
	#endregion //Get Data

	#region Update Data
		/// <summary>
		/// Cập nhật tên font Unicode
		/// </summary>
		public static void UpdateTen(string psAbb, string psAbb_ND, string psDai, string psTen)
		{
			//new DAL.DataOleDB().UpdateTen(psAbb, psAbb_ND, psDai, psTen);
		}
	#endregion //Update Data

    #region Add Data
        /// <summary>
		/// Tạo cấu trúc table foxpro, access
		/// </summary>
		public static void MakeTable(string psProvider, string psTable, DataTable pdtDataSrc, IDbConnection Cxn){
            new DAL.DataOleDB(Cxn).MakeTable(psProvider, psTable, pdtDataSrc);
        }
        /// <summary>
		/// Tạo dữ liệu foxpro, access
		/// </summary>
		public static int AddNew(string psTable, DataTable pdtDataSrc, IDbConnection Cxn){
            return (new DAL.DataOleDB(Cxn).AddNew(psTable, pdtDataSrc));
        }
        /// <summary>
		/// Tạo command add dữ liệu foxpro, access
		/// </summary>
        public static OleDbCommand MakeCommandAdd(string psProvider, string psTable, DataTable pdtDataSrc, string psListFieldDest, IDbConnection Cxn)
        {
            return (new DAL.DataOleDB(Cxn).MakeCommandAdd(psProvider, psTable, pdtDataSrc, psListFieldDest));
        }
        /// <summary>
		/// Tạo dữ liệu 1 record foxpro, access
		/// </summary>
        public static int AddNewRow(DataRow pdrData, OleDbCommand pcmdData, string psListFieldDest)
        {
            return (new DAL.DataOleDB().AddNewRow(pdrData, pcmdData, psListFieldDest));
        }
    #endregion //Add Data
	}
}
