using System;
using System.Data;
using System.Data.OleDb;
using System.Collections.Generic;

namespace ImportUtil.DataAccess
{
	public class DataOleDB : BaseDataObjectOleDB
	{
	#region Constructors
		public DataOleDB() : base() {}
		/// <summary>
		/// Creates a new data object and specifies a connection/ transaction with
		/// which to operate
		/// </summary>
		public DataOleDB(IDbConnection cxn) : base(cxn) {}
		public DataOleDB(IDbTransaction txn) : base(txn) {}
		public DataOleDB(IDbConnection cxn, IDbTransaction txn) : base(cxn, txn) {}
	#endregion //Constructors

	#region Get Data
		/// <summary>
		/// Lấy dữ liệu
		/// </summary>
		public DataTable GetData(string psSql){
			return ExecuteDataTable(psSql);
		}
	#endregion //Get Data

    #region Add Data
        /// <summary>
		/// Tạo cấu trúc table file foxpro, access
		/// </summary>
		public string MakeTable(string psProvider, string psTable, DataTable pdtDataSrc){
            int i, lCount, lLen;
            string lsSQL = "", lsTypeSrc, lsFieldName;
            try{
                lCount = pdtDataSrc.Columns.Count;
                for(i = 0; i < lCount; i++){
                    lsTypeSrc = pdtDataSrc.Columns[i].DataType.Name;
                    lsFieldName = pdtDataSrc.Columns[i].ColumnName;
                    if (psProvider == "Fox"){//chiều dài field <= 10 ký tự
                        lLen = lsFieldName.Length;
                        if (lLen > 10)
                            lsFieldName = lsFieldName.Substring(0, 10);
                    }
                    switch(lsTypeSrc){
                        case "String":
                            lsSQL += (lsSQL.Length > 0 ? "," : "") + lsFieldName + " C(254)";
                            break;
                        case "Int16": case "UInt16":
                            lsSQL += (lsSQL.Length > 0 ? "," : "") + lsFieldName + " N(5)";
                            break;
                        case "Int32": case "UInt32":
                            lsSQL += (lsSQL.Length > 0 ? "," : "") + lsFieldName + " N(15)";
                            break;
                        case "Int64"://long
                            lsSQL += (lsSQL.Length > 0 ? "," : "") + lsFieldName + " N(20)";
                            break;
                        case "Byte":
                            lsSQL += (lsSQL.Length > 0 ? "," : "") + lsFieldName + " N(2)";
                            break;
                        case "Boolean":
                            lsSQL += (lsSQL.Length > 0 ? "," : "") + lsFieldName + " L";
                            break;
                        case "DateTime":
                            lsSQL += (lsSQL.Length > 0 ? "," : "") + lsFieldName + " D";
                            break;
                        case "Decimal":
                            lsSQL += (lsSQL.Length > 0 ? "," : "") + lsFieldName + " N(15,2)";
                            break;
                        case "Single":
                            lsSQL += (lsSQL.Length > 0 ? "," : "") + lsFieldName + " N(10,2)";
                            break;
                        case "Double":
                            lsSQL += (lsSQL.Length > 0 ? "," : "") + lsFieldName + " N(20,2)";
                            break;
                        default:
                            lsSQL += "," + lsTypeSrc;
                            break;
                    }
                }
                lsSQL = "Create Table " + psTable + " (" + lsSQL + ")";
                ExecuteNonQuery(lsSQL);
            }catch(Exception ex){
                throw new Exception("[MakeTable] " + ex.Message);
            }
            return lsSQL;
        }
        /// <summary>
		/// Tạo command add dữ liệu foxpro, access
		/// </summary>
        public OleDbCommand MakeCommandAdd(string psProvider, string psTable, DataTable pdtDataSrc, string psListFieldDest)
        {
			string lsSQL, lsFieldName, lsFieldNew, lsTypeSrc;
            string lsFields = "";
			OleDbCommand lcmdData = null;
			List<OleDbParameter> lstParams = new List<OleDbParameter>();
			int i, lCountField, lLen;
            try{
                lCountField = pdtDataSrc.Columns.Count;
                for(i = 0; i < lCountField; i++){
                    lsFieldName = pdtDataSrc.Columns[i].ColumnName;
                    lsFieldNew = lsFieldName;
                    if(psProvider == "Fox"){
                        lLen = lsFieldName.Length;
                        if(lLen > 10) //Foxpro chiều dài field không được quá 10 char
                            lsFieldNew = lsFieldName.Substring(0,10);
                    }
                    if (!ClsUtil.IsExistField(lsFieldNew, psListFieldDest))//nếu field không thuộc table dest: bỏ qua
                        continue;
                    lsTypeSrc = pdtDataSrc.Columns[i].DataType.Name;
                    lsFields += (lsFields.Length == 0 ? "" : ",") + lsFieldNew;
                    switch(lsTypeSrc){
                        case "String":
                            lstParams.Add(CreateParameter(lsFieldName, OleDbType.VarChar, DBNull.Value));
                            break;
                        case "Int16": case "UInt16":
                            lstParams.Add(CreateParameter(lsFieldName, OleDbType.SmallInt, DBNull.Value));
                            break;
                        case "Int32": case "UInt32":
                            lstParams.Add(CreateParameter(lsFieldName, OleDbType.Integer, DBNull.Value));
                            break;
                        case "Int64"://long
                            lstParams.Add(CreateParameter(lsFieldName, OleDbType.BigInt, DBNull.Value));
                            break;
                        case "Byte":
                            lstParams.Add(CreateParameter(lsFieldName, OleDbType.TinyInt, DBNull.Value));
                            break;
                        case "Boolean":
                            lstParams.Add(CreateParameter(lsFieldName, OleDbType.Boolean, DBNull.Value));
                            break;
                        case "DateTime":
                            lstParams.Add(CreateParameter(lsFieldName, OleDbType.Date, DBNull.Value));
                            break;
                        case "Decimal":
                            lstParams.Add(CreateParameter(lsFieldName, OleDbType.Decimal, DBNull.Value));
                            break;
                        case "Single":
                            lstParams.Add(CreateParameter(lsFieldName, OleDbType.Single, DBNull.Value));
                            break;
                        case "Double":
                            lstParams.Add(CreateParameter(lsFieldName, OleDbType.Double, DBNull.Value));
                            break;
                    }
                }
			    lsSQL = CreateStringSQL(psTable, lsFields);
			    lcmdData = CreateCommand(lsSQL, lstParams.ToArray());
			    lstParams.Clear(); lstParams = null;
            }catch(Exception ex){
                throw new Exception("[MakeCommandAdd] " + ex.Message);
            }
            return lcmdData;
        }
        /// <summary>
		/// Tạo dữ liệu 1 record foxpro, access
		/// </summary>
        public int AddNewRow(DataRow pdrData, OleDbCommand pcmdData, string psListFieldDest)
        {
			string lsFieldName, lsTypeSrc, lsListData = "", lsTableName;
			int i, lCountField, lRes = 0, lCountFieldFound = 0;
            try{
                lCountField = pdrData.Table.Columns.Count;
			    for(i = 0; i < lCountField; i++){
				    lsTypeSrc = pdrData.Table.Columns[i].DataType.Name;
                    lsFieldName = pdrData.Table.Columns[i].ColumnName;
                    if (!ClsUtil.IsExistField(lsFieldName, psListFieldDest))//nếu field không thuộc table dest: bỏ qua
                        continue;
                    lCountFieldFound++;
                    switch (lsTypeSrc){
                        case "String":
                            pcmdData.Parameters[lsFieldName].Value = ClsUtil.parseString(pdrData, lsFieldName);
                            break;
                        case "Int16": case "UInt16":
                        case "Int32": case "UInt32":
                        case "Int64"://long
                            pcmdData.Parameters[lsFieldName].Value = ClsUtil.parseInt(pdrData, lsFieldName);
                            break;
                        case "Byte":
                            pcmdData.Parameters[lsFieldName].Value = ClsUtil.parseByte(pdrData, lsFieldName);
                            break;
                        case "Boolean":
                            pcmdData.Parameters[lsFieldName].Value = ClsUtil.parseBool(pdrData, lsFieldName);
                            break;
                        case "DateTime":
                            pcmdData.Parameters[lsFieldName].Value = ClsUtil.parseDateTime(pdrData, lsFieldName);
                            break;
                        case "Decimal":
                            pcmdData.Parameters[lsFieldName].Value = ClsUtil.parseDecimal(pdrData, lsFieldName);
                            break;
                        case "Single":
                            pcmdData.Parameters[lsFieldName].Value = ClsUtil.parseInt(pdrData, lsFieldName);
                            break;
                        case "Double":
                            pcmdData.Parameters[lsFieldName].Value = ClsUtil.parseDouble(pdrData, lsFieldName);
                            break;
                    }
                    lsListData += (lsListData.Length > 0 ? "," : "") + lsFieldName + " = " + pcmdData.Parameters[lsFieldName].Value;
			    }//of for i
			    lRes = ExecuteNonQuery(pcmdData);
            }catch(Exception ex){
                lsTableName = pdrData.Table.TableName;
                throw new Exception(Environment.NewLine + "[AddNewRow] " + lsTableName + ": " + ex.Message + ". Data: " + lsListData + ", Found " + lCountFieldFound + " fields in table dest " + lsTableName);
            }
			return lRes;
		}
        /// <summary>
		/// Tạo dữ liệu foxpro, access
		/// </summary>
		public int AddNew(string psTable, DataTable pdtDataSrc){
			string lsSQL, lsFieldName, lsTypeSrc;
            string lsFields = "";
			OleDbCommand lcmdData;
			List<OleDbParameter> lstParams = new List<OleDbParameter>();
			int i, j, lCount, lCountField, lRes = 0;

			lCount = pdtDataSrc.Rows.Count;
            lCountField = pdtDataSrc.Columns.Count;
            for(i = 0; i < lCount; i++){
                lsTypeSrc = pdtDataSrc.Columns[i].DataType.Name;
                lsFieldName = pdtDataSrc.Columns[i].ColumnName;
                lsFields += (lsFields.Length == 0 ? "" : ",") + lsFieldName;
                switch(lsTypeSrc){
                    case "String":
                        lstParams.Add(CreateParameter(lsFieldName, OleDbType.VarChar, DBNull.Value));
                        break;
                    case "Int16": case "UInt16":
                        lstParams.Add(CreateParameter(lsFieldName, OleDbType.SmallInt, DBNull.Value));
                        break;
                    case "Int32": case "UInt32":
                        lstParams.Add(CreateParameter(lsFieldName, OleDbType.Integer, DBNull.Value));
                        break;
                    case "Int64"://long
                        lstParams.Add(CreateParameter(lsFieldName, OleDbType.BigInt, DBNull.Value));
                        break;
                    case "Byte":
                        lstParams.Add(CreateParameter(lsFieldName, OleDbType.TinyInt, DBNull.Value));
                        break;
                    case "Boolean":
                        lstParams.Add(CreateParameter(lsFieldName, OleDbType.Boolean, DBNull.Value));
                        break;
                    case "DateTime":
                        lstParams.Add(CreateParameter(lsFieldName, OleDbType.Date, DBNull.Value));
                        break;
                    case "Decimal":
                        lstParams.Add(CreateParameter(lsFieldName, OleDbType.Decimal, DBNull.Value));
                        break;
                    case "Single":
                        lstParams.Add(CreateParameter(lsFieldName, OleDbType.Single, DBNull.Value));
                        break;
                    case "Double":
                        lstParams.Add(CreateParameter(lsFieldName, OleDbType.Double, DBNull.Value));
                        break;
                }
            }
			lsSQL = CreateStringSQL(psTable, lsFields);
			lcmdData = CreateCommand(lsSQL, lstParams.ToArray());
			lstParams.Clear(); lstParams = null;
			for(i = 0; i < lCount; i++){
				for(j = 0; j < lCountField; j++){
					lsTypeSrc = pdtDataSrc.Columns[j].DataType.Name;
                    lsFieldName = pdtDataSrc.Columns[j].ColumnName;
                    switch(lsTypeSrc){
                        case "String":
                            lcmdData.Parameters[lsFieldName].Value = ClsUtil.parseString(pdtDataSrc.Rows[i], lsFieldName);
                            break;
                        case "Int16": case "UInt16":
                        case "Int32": case "UInt32":
                        case "Int64"://long
                            lcmdData.Parameters[lsFieldName].Value = ClsUtil.parseInt(pdtDataSrc.Rows[i], lsFieldName);
                            break;
                        case "Byte":
                            lcmdData.Parameters[lsFieldName].Value = ClsUtil.parseByte(pdtDataSrc.Rows[i], lsFieldName);
                            break;
                        case "Boolean":
                            lcmdData.Parameters[lsFieldName].Value = ClsUtil.parseBool(pdtDataSrc.Rows[i], lsFieldName);
                            break;
                        case "DateTime":
                            lcmdData.Parameters[lsFieldName].Value = ClsUtil.parseDateTime(pdtDataSrc.Rows[i], lsFieldName);
                            break;
                        case "Decimal":
                            lcmdData.Parameters[lsFieldName].Value = ClsUtil.parseDecimal(pdtDataSrc.Rows[i], lsFieldName);
                            break;
                        case "Single":
                            lcmdData.Parameters[lsFieldName].Value = ClsUtil.parseInt(pdtDataSrc.Rows[i], lsFieldName);
                            break;
                        case "Double":
                            lcmdData.Parameters[lsFieldName].Value = ClsUtil.parseDouble(pdtDataSrc.Rows[i], lsFieldName);
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
		public string CreateStringSQL(string psTable, string psFields){
			string[] larrField;
			string lsRes;
			int i, lCount;
			larrField = psFields.Split(',');
			lsRes = "";
			lCount = larrField.Length;
			for (i = 0 ; i < lCount; i++){
				lsRes += (lsRes.Equals("") ? "" : ",") + "?";
			}
			lsRes = "INSERT INTO " + psTable + "(" + psFields + ") VALUES (" + lsRes + ")";
			return lsRes;
		}
    #endregion //Add Data

    
	}
}
