using System;
using System.Data;
using System.Data.SqlClient;
using System.Collections.Generic;

namespace ImportUtil.DataAccess
{
	public class DataSql : BaseDataObjectSql
	{
	#region Constructors
		public DataSql() : base() {}
		/// <summary>
		/// Creates a new data object and specifies a connection/ transaction with
		/// which to operate
		/// </summary>
		public DataSql(IDbConnection cxn) : base(cxn) {}
		public DataSql(IDbTransaction txn) : base(txn) {}
		public DataSql(IDbConnection cxn, IDbTransaction txn) : base(cxn, txn) {}
	#endregion //Constructors

	#region Get Data
		/// <summary>
		/// Lấy dữ liệu
		/// </summary>
		public DataTable GetData(string psSql){
			return ExecuteDataTable(psSql);
		}
	#endregion //Get Data

	#region Update Data
		
	#endregion //Update Data

    #region Add Data
        /// <summary>
		/// Tạo cấu trúc table MsSql
		/// </summary>
		public string MakeTable(string psSchema, string psTable, DataTable pdtDataSrc){
            int i, lCount;
            string lsSQL = "", lsTypeSrc, lsFieldName;
            try{
                lCount = pdtDataSrc.Columns.Count;
                for(i = 0; i < lCount; i++){
                    lsTypeSrc = pdtDataSrc.Columns[i].DataType.Name;
                    lsFieldName = pdtDataSrc.Columns[i].ColumnName;
                    switch(lsTypeSrc){
                        case "String":
                            lsSQL += (lsSQL.Length > 0 ? "," : "") + lsFieldName + " NVarChar(254)";
                            break;
                        case "Int16": case "UInt16":
                            lsSQL += (lsSQL.Length > 0 ? "," : "") + lsFieldName + " SmallInt";
                            break;
                        case "Int32": case "UInt32":
                            lsSQL += (lsSQL.Length > 0 ? "," : "") + lsFieldName + " Int";
                            break;
                        case "Int64"://long
                            lsSQL += (lsSQL.Length > 0 ? "," : "") + lsFieldName + " BigInt";
                            break;
                        case "Byte":
                            lsSQL += (lsSQL.Length > 0 ? "," : "") + lsFieldName + " TinyInt";
                            break;
                        case "Boolean":
                            lsSQL += (lsSQL.Length > 0 ? "," : "") + lsFieldName + " Bit";
                            break;
                        case "DateTime":
                            lsSQL += (lsSQL.Length > 0 ? "," : "") + lsFieldName + " DateTime";
                            break;
                        case "Decimal":
                            lsSQL += (lsSQL.Length > 0 ? "," : "") + lsFieldName + " Decimal(15,2)";
                            break;
                        case "Single":
                            lsSQL += (lsSQL.Length > 0 ? "," : "") + lsFieldName + " Float";
                            break;
                        case "Double":
                            lsSQL += (lsSQL.Length > 0 ? "," : "") + lsFieldName + " Decimal(20,2)";
                            break;
                        default:
                            lsSQL += "," + lsTypeSrc;
                            break;
                    }
                }
                lsSQL = "Create Table " + (psSchema.Length > 0 ? psSchema + "." : "") + psTable + " (" + lsSQL + ")";
                ExecuteNonQuery(lsSQL);
            }catch(Exception ex){
                throw new Exception("[MakeTable] " + ex.Message);
            }
            return lsSQL;
        }
        /// <summary>
		/// Tạo dữ liệu MsSql
		/// </summary>
        public SqlCommand MakeCommandAdd(string psSchema, string psTable, DataTable pdtDataSrc, string psListFieldDest)
        {
			string lsSQL, lsFieldName, lsTypeSrc;
            string lsFields = "", lsParaName;
			SqlCommand lcmdData = null;
			List<SqlParameter> lstParams = new List<SqlParameter>();
			int i, lCountField;
            try{
                lCountField = pdtDataSrc.Columns.Count;
                for(i = 0; i < lCountField; i++){
                    lsFieldName = pdtDataSrc.Columns[i].ColumnName;
                    if (!ClsUtil.IsExistField(lsFieldName, psListFieldDest))//nếu field không thuộc table dest: bỏ qua
                        continue;
                    lsTypeSrc = pdtDataSrc.Columns[i].DataType.Name;
                    lsParaName = "@" + lsFieldName;
                    lsFields += (lsFields.Length == 0 ? "" : ",") + lsFieldName;
                    switch(lsTypeSrc){
                        case "String":
                            lstParams.Add(CreateParameter(lsParaName, SqlDbType.NVarChar, DBNull.Value));
                            break;
                        case "Int16": case "UInt16":
                            lstParams.Add(CreateParameter(lsParaName, SqlDbType.SmallInt, DBNull.Value));
                            break;
                        case "Int32": case "UInt32":
                            lstParams.Add(CreateParameter(lsParaName, SqlDbType.Int, DBNull.Value));
                            break;
                        case "Int64"://long
                            lstParams.Add(CreateParameter(lsParaName, SqlDbType.BigInt, DBNull.Value));
                            break;
                        case "Byte":
                            lstParams.Add(CreateParameter(lsParaName, SqlDbType.TinyInt, DBNull.Value));
                            break;
                        case "Boolean":
                            lstParams.Add(CreateParameter(lsParaName, SqlDbType.Bit, DBNull.Value));
                            break;
                        case "DateTime":
                            lstParams.Add(CreateParameter(lsParaName, SqlDbType.Date, DBNull.Value));
                            break;
                        case "Decimal": case "Double":
                            lstParams.Add(CreateParameter(lsParaName, SqlDbType.Decimal, DBNull.Value));
                            break;
                        case "Single":
                            lstParams.Add(CreateParameter(lsParaName, SqlDbType.Float, DBNull.Value));
                            break;
                    }
                }
			    lsSQL = CreateStringSQL(psSchema, psTable, lsFields);
			    lcmdData = CreateCommand(lsSQL, lstParams.ToArray());
			    lstParams.Clear(); lstParams = null;
            }catch(Exception ex){
                throw new Exception("[MakeCommandAdd] " + ex.Message);
            }
            return lcmdData;
        }
        /// <summary>
		/// Tạo dữ liệu MsSql
		/// </summary>
        public int AddNewRow(DataRow pdrData, SqlCommand pcmdData, string psListFieldDest)
        {
			string lsFieldName, lsTypeSrc, lsTableName;
            string lsParaName, lsListData = "";
			int i, lCountField, lRes = 0, lCountFieldFound = 0;
            try{
                lCountField = pdrData.Table.Columns.Count;
			    for(i = 0; i < lCountField; i++){
				    lsTypeSrc = pdrData.Table.Columns[i].DataType.Name;
                    lsFieldName = pdrData.Table.Columns[i].ColumnName;
                    if (!ClsUtil.IsExistField(lsFieldName, psListFieldDest))//nếu field không thuộc table dest: bỏ qua
                        continue;
                    lCountFieldFound++;
                    lsParaName = "@" + lsFieldName;
                    switch(lsTypeSrc){
                        case "String":
                            pcmdData.Parameters[lsParaName].Value = ClsUtil.parseString(pdrData, lsFieldName);
                            break;
                        case "Int16": case "UInt16":
                        case "Int32": case "UInt32":
                        case "Int64"://long
                            pcmdData.Parameters[lsParaName].Value = ClsUtil.parseInt(pdrData, lsFieldName);
                            break;
                        case "Byte":
                            pcmdData.Parameters[lsParaName].Value = ClsUtil.parseByte(pdrData, lsFieldName);
                            break;
                        case "Boolean":
                            pcmdData.Parameters[lsParaName].Value = ClsUtil.parseBool(pdrData, lsFieldName);
                            break;
                        case "DateTime":
                            pcmdData.Parameters[lsParaName].Value = ClsUtil.parseDateTime(pdrData, lsFieldName);
                            break;
                        case "Decimal":
                            pcmdData.Parameters[lsParaName].Value = ClsUtil.parseDecimal(pdrData, lsFieldName);
                            break;
                        case "Single":
                            pcmdData.Parameters[lsParaName].Value = ClsUtil.parseInt(pdrData, lsFieldName);
                            break;
                        case "Double":
                            pcmdData.Parameters[lsParaName].Value = ClsUtil.parseDouble(pdrData, lsFieldName);
                            break;
                    }
                    lsListData += (lsListData.Length > 0 ? "," : "") + lsFieldName + " = " + pcmdData.Parameters[lsParaName].Value;
			    }//of for i
			    lRes = ExecuteNonQuery(pcmdData);
            }catch(Exception ex){
                lsTableName = pdrData.Table.TableName;
                throw new Exception(Environment.NewLine + "[AddNewRow] " + lsTableName + ": " + ex.Message + ". Data: " + lsListData + ", Found " + lCountFieldFound + " fields in table dest " + lsTableName);
            }
			return lRes;
		}
        /// <summary>
		/// Tạo dữ liệu MsSql
		/// </summary>
		public int AddNew(string psSchema, string psTable, DataTable pdtDataSrc){
			string lsSQL, lsFieldName, lsTypeSrc;
            string lsFields = "", lsParaName;
			SqlCommand lcmdData;
			List<SqlParameter> lstParams = new List<SqlParameter>();
			int i, j, lCount, lCountField, lRes = 0;

			lCount = pdtDataSrc.Rows.Count;
            lCountField = pdtDataSrc.Columns.Count;
            for(i = 0; i < lCountField; i++){
                lsTypeSrc = pdtDataSrc.Columns[i].DataType.Name;
                lsFieldName = pdtDataSrc.Columns[i].ColumnName;
                lsParaName = "@" + lsFieldName;
                lsFields += (lsFields.Length == 0 ? "" : ",") + lsFieldName;
                switch(lsTypeSrc){
                    case "String":
                        lstParams.Add(CreateParameter(lsParaName, SqlDbType.NVarChar, DBNull.Value));
                        break;
                    case "Int16": case "UInt16":
                        lstParams.Add(CreateParameter(lsParaName, SqlDbType.SmallInt, DBNull.Value));
                        break;
                    case "Int32": case "UInt32":
                        lstParams.Add(CreateParameter(lsParaName, SqlDbType.Int, DBNull.Value));
                        break;
                    case "Int64"://long
                        lstParams.Add(CreateParameter(lsParaName, SqlDbType.BigInt, DBNull.Value));
                        break;
                    case "Byte":
                        lstParams.Add(CreateParameter(lsParaName, SqlDbType.TinyInt, DBNull.Value));
                        break;
                    case "Boolean":
                        lstParams.Add(CreateParameter(lsParaName, SqlDbType.Bit, DBNull.Value));
                        break;
                    case "DateTime":
                        lstParams.Add(CreateParameter(lsParaName, SqlDbType.Date, DBNull.Value));
                        break;
                    case "Decimal": case "Double":
                        lstParams.Add(CreateParameter(lsParaName, SqlDbType.Decimal, DBNull.Value));
                        break;
                    case "Single":
                        lstParams.Add(CreateParameter(lsParaName, SqlDbType.Float, DBNull.Value));
                        break;
                }
            }
			lsSQL = CreateStringSQL(psSchema, psTable, lsFields);
			lcmdData = CreateCommand(lsSQL, lstParams.ToArray());
			lstParams.Clear(); lstParams = null;
			for(i = 0; i < lCount; i++){
				for(j = 0; j < lCountField; j++){
					lsTypeSrc = pdtDataSrc.Columns[j].DataType.Name;
                    lsFieldName = pdtDataSrc.Columns[j].ColumnName;
                    lsParaName = "@" + lsFieldName;
                    switch(lsTypeSrc){
                        case "String":
                            lcmdData.Parameters[lsParaName].Value = ClsUtil.parseString(pdtDataSrc.Rows[i], lsFieldName);
                            break;
                        case "Int16": case "UInt16":
                        case "Int32": case "UInt32":
                        case "Int64"://long
                            lcmdData.Parameters[lsParaName].Value = ClsUtil.parseInt(pdtDataSrc.Rows[i], lsFieldName);
                            break;
                        case "Byte":
                            lcmdData.Parameters[lsParaName].Value = ClsUtil.parseByte(pdtDataSrc.Rows[i], lsFieldName);
                            break;
                        case "Boolean":
                            lcmdData.Parameters[lsParaName].Value = ClsUtil.parseBool(pdtDataSrc.Rows[i], lsFieldName);
                            break;
                        case "DateTime":
                            lcmdData.Parameters[lsParaName].Value = ClsUtil.parseDateTime(pdtDataSrc.Rows[i], lsFieldName);
                            break;
                        case "Decimal":
                            lcmdData.Parameters[lsParaName].Value = ClsUtil.parseDecimal(pdtDataSrc.Rows[i], lsFieldName);
                            break;
                        case "Single":
                            lcmdData.Parameters[lsParaName].Value = ClsUtil.parseInt(pdtDataSrc.Rows[i], lsFieldName);
                            break;
                        case "Double":
                            lcmdData.Parameters[lsParaName].Value = ClsUtil.parseDouble(pdtDataSrc.Rows[i], lsFieldName);
                            break;
                    }
				}//of for j
				lRes += ExecuteNonQuery(lcmdData);
			}
			return lRes;
		}
        /// <summary>
		/// Tạo câu query Insert
		/// </summary>
		public string CreateStringSQL(string psSchema, string psTable, string psFields){
			string[] larrField;
			string lsRes;
			int i, lCount;
			larrField = psFields.Split(',');
			lsRes = "";
			lCount = larrField.Length;
			for (i = 0 ; i < lCount; i++){
				lsRes += (lsRes.Equals("") ? "" : ",") + "@" + larrField[i];
			}
			lsRes = "INSERT INTO " + (psSchema.Length > 0 ? psSchema + "." : "") + psTable + "(" + psFields + ") VALUES (" + lsRes + ")";
			return lsRes;
		}
    #endregion //Add Data
	}
}
