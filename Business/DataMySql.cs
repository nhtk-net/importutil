using System;
using System.Data;
using System.Collections.Generic;
using DAL = ImportUtil.DataAccess;
using MySql.Data.MySqlClient;

namespace ImportUtil
{
	public class DataMySql : BaseBusinessObjectMySql
	{
	#region Get Data
		public static DataTable GetData(string psSql, IDbConnection Cxn){
			return new DAL.DataMySql(Cxn).GetData(psSql);
		}
	#endregion //Get Data

	#region Update Data
		/// <summary>
		/// Cập nhật tên font Unicode
		/// </summary>
		public static void UpdateTen(string psAbb, string psAbb_ND, string psDai, string psTen)
		{
			//new DAL.DataMySql().UpdateTen(psAbb, psAbb_ND, psDai, psTen);
		}
	#endregion //Update Data

    #region Add Data
        /// <summary>
		/// Tạo cấu trúc table MsSql
		/// </summary>
        public static void MakeTable(string psSchema, string psTable, DataTable pdtDataSrc, IDbConnection Cxn){
            new DAL.DataMySql(Cxn).MakeTable(psSchema, psTable, pdtDataSrc);
        }
        /// <summary>
		/// Tạo dữ liệu MsSql
		/// </summary>
		public static int AddNew(string psSchema, string psTable, DataTable pdtDataSrc, IDbConnection Cxn){
            return (new DAL.DataMySql(Cxn).AddNew(psSchema, psTable, pdtDataSrc));
        }
        /// <summary>
		/// Tạo command add dữ liệu MsSql
		/// </summary>
        public static MySqlCommand MakeCommandAdd(string psSchema, string psTable, DataTable pdtDataSrc, string psListFieldDest, IDbConnection Cxn)
        {
            return (new DAL.DataMySql(Cxn).MakeCommandAdd(psSchema, psTable, pdtDataSrc, psListFieldDest));
        }
        /// <summary>
		/// Tạo dữ liệu 1 record MsSql
		/// </summary>
        public static int AddNewRow(DataRow pdrData, MySqlCommand pcmdData, string psListFieldDest)
        {
            return (new DAL.DataMySql().AddNewRow(pdrData, pcmdData, psListFieldDest));
        }
    #endregion //Add Data
	}
}
