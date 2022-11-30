using System;
using System.Data;
using System.Collections.Generic;
using DAL = ImportUtil.DataAccess;
using Oracle.DataAccess.Client;

namespace ImportUtil
{
	public class DataOra : BaseBusinessObject
	{
	#region Get Data
		public static DataTable GetData(string psSql, IDbConnection Cxn){
			return new DAL.DataOra(Cxn).GetData(psSql);
		}
	#endregion //Get Data

	#region Update Data
		/// <summary>
		/// Cập nhật tên font Unicode
		/// </summary>
		public static void UpdateTen(string psAbb, string psAbb_ND, string psDai, string psTen)
		{
			new DAL.DataOra().UpdateTen(psAbb, psAbb_ND, psDai, psTen);
		}
	#endregion //Update Data

    #region Add Data
        /// <summary>
		/// Tạo cấu trúc table Oracle
		/// </summary>
		public static void MakeTable(string psSchema, string psTable, DataTable pdtDataSrc, IDbConnection Cxn){
            new DAL.DataOra(Cxn).MakeTable(psSchema, psTable, pdtDataSrc);
        }
        /// <summary>
		/// Tạo dữ liệu Oracle
		/// </summary>
		public static int AddNew(string psSchema, string psTable, DataTable pdtDataSrc, IDbConnection Cxn){
            return (new DAL.DataOra(Cxn).AddNew(psSchema, psTable, pdtDataSrc));
        }
        /// <summary>
		/// Tạo command add dữ liệu Oracle
		/// </summary>
        public static OracleCommand MakeCommandAdd(string psSchema, string psTable, DataTable pdtDataSrc, string psListFieldDest, IDbConnection Cxn)
        {
            return (new DAL.DataOra(Cxn).MakeCommandAdd(psSchema, psTable, pdtDataSrc, psListFieldDest));
        }
        /// <summary>
		/// Tạo dữ liệu 1 record Oracle
		/// </summary>
		public static int AddNewRow(DataRow pdrData, OracleCommand pcmdData, string psListFieldDest){
            return (new DAL.DataOra().AddNewRow(pdrData, pcmdData, psListFieldDest));
        }
    #endregion //Add Data
	}
}
