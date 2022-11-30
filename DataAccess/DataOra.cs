using System;
using System.Data;
using Oracle.DataAccess.Client;
using System.Collections.Generic;

namespace ImportUtil.DataAccess
{
	public class DataOra : BaseDataObject
	{
		#region Constructors
		public DataOra() : base() {}
		/// <summary>
		/// Creates a new data object and specifies a connection/ transaction with
		/// which to operate
		/// </summary>
		public DataOra(IDbConnection cxn) : base(cxn) {}
		public DataOra(IDbTransaction txn) : base(txn) {}
		public DataOra(IDbConnection cxn, IDbTransaction txn) : base(cxn, txn) {}
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
		/// <summary>
		/// Cập nhật tên font Unicode
		/// </summary>
		public void UpdateTen(string psAbb, string psAbb_ND, string psDai, string psTen)
		{
			ExecuteNonQuery("Update DM_Abb_Dai_Tram Set Ten = :p_Ten Where Abb = :p_Abb And Abb_ND = :p_Abb_ND And Dai_1 = :p_Dai_1",
				CreateParameter("p_Ten", OracleDbType.Varchar2, psTen),
				CreateParameter("p_Abb", OracleDbType.Varchar2, psAbb),
				CreateParameter("p_Abb_ND", OracleDbType.Varchar2, psAbb_ND),
				CreateParameter("p_Dai_1", OracleDbType.Varchar2, psDai));
		}
	#endregion //Update Data

    #region Add Data
        /// <summary>
		/// Tạo cấu trúc table Oracle
		/// </summary>
		public string MakeTable(string psSchema, string psTable, DataTable pdtDataSrc){
            int i, lCount;
            string lsSQL = "", lsTypeSrc, lsFieldName;
            try{
                lCount = pdtDataSrc.Columns.Count;
                for(i = 0; i < lCount; i++){
                    lsTypeSrc = pdtDataSrc.Columns[i].DataType.Name;
                    lsFieldName = pdtDataSrc.Columns[i].ColumnName;
                    if (lsFieldName.IndexOf(" ") > 0)
                        lsFieldName = "\"" + lsFieldName + "\"";
                    switch(lsTypeSrc){
                        case "String":
                            lsSQL += (lsSQL.Length > 0 ? "," : "") + lsFieldName + " VarChar2(254)";
                            break;
                        case "Int16": case "UInt16":
                            lsSQL += (lsSQL.Length > 0 ? "," : "") + lsFieldName + " Number(5,0)";
                            break;
                        case "Int32": case "UInt32":
                            lsSQL += (lsSQL.Length > 0 ? "," : "") + lsFieldName + " Number(15,0)";
                            break;
                        case "Int64"://long
                            lsSQL += (lsSQL.Length > 0 ? "," : "") + lsFieldName + " Number(20,0)";
                            break;
                        case "Byte":
                            lsSQL += (lsSQL.Length > 0 ? "," : "") + lsFieldName + " Number(2,0)";
                            break;
                        case "Boolean":
                            lsSQL += (lsSQL.Length > 0 ? "," : "") + lsFieldName + " Number(1,0)";
                            break;
                        case "DateTime":
                            lsSQL += (lsSQL.Length > 0 ? "," : "") + lsFieldName + " Date";
                            break;
                        case "Decimal":
                            lsSQL += (lsSQL.Length > 0 ? "," : "") + lsFieldName + " Number(15,2)";
                            break;
                        case "Single":
                            lsSQL += (lsSQL.Length > 0 ? "," : "") + lsFieldName + " Number(10,2)";
                            break;
                        case "Double":
                            lsSQL += (lsSQL.Length > 0 ? "," : "") + lsFieldName + " Number(20,2)";
                            break;
                        default:
                            lsSQL += "," + lsTypeSrc;
                            break;
                    }
                }
                lsSQL = "Create Table " + (psSchema.Length > 0 ? psSchema + "." : "") + psTable + " (" + lsSQL + ")";
                ExecuteNonQuery(lsSQL);
            }catch(Exception ex){
                throw new Exception("[MakeTable] " + ex.Message + Environment.NewLine + "SQL: " + lsSQL);
            }
            return lsSQL;
        }
        /// <summary>
        /// Tạo command add dữ liệu Oracle, pdtStructDest: cấu trúc của table dest
		/// </summary>
        public OracleCommand MakeCommandAdd(string psSchema, string psTable, DataTable pdtDataSrc, string psListFieldDest)
        {
			string lsSQL, lsFieldName, lsTypeSrc;
            string lsFields = "";
			OracleCommand lcmdData = null;
			List<OracleParameter> lstParams = new List<OracleParameter>();
			int i, lCountField;
            try{
                lCountField = pdtDataSrc.Columns.Count;
                for(i = 0; i < lCountField; i++){
                    lsFieldName = pdtDataSrc.Columns[i].ColumnName;
                    if (!ClsUtil.IsExistField(lsFieldName, psListFieldDest))//nếu field không thuộc table dest: bỏ qua
                        continue;
                    lsTypeSrc = pdtDataSrc.Columns[i].DataType.Name;
                    lsFields += (lsFields.Length == 0 ? "" : ",") + lsFieldName;
                    switch(lsTypeSrc){
                        case "String":
                            lstParams.Add(CreateParameter(lsFieldName, OracleDbType.Varchar2, DBNull.Value));
                            break;
                        case "Int16": case "UInt16":
                            lstParams.Add(CreateParameter(lsFieldName, OracleDbType.Int16, DBNull.Value));
                            break;
                        case "Int32": case "UInt32":
                            lstParams.Add(CreateParameter(lsFieldName, OracleDbType.Int32, DBNull.Value));
                            break;
                        case "Int64"://long
                            lstParams.Add(CreateParameter(lsFieldName, OracleDbType.Int64, DBNull.Value));
                            break;
                        case "Byte":
                            lstParams.Add(CreateParameter(lsFieldName, OracleDbType.Byte, DBNull.Value));
                            break;
                        case "Boolean":
                            lstParams.Add(CreateParameter(lsFieldName, OracleDbType.Byte, DBNull.Value));
                            break;
                        case "DateTime":
                            lstParams.Add(CreateParameter(lsFieldName, OracleDbType.Date, DBNull.Value));
                            break;
                        case "Decimal":
                            lstParams.Add(CreateParameter(lsFieldName, OracleDbType.Decimal, DBNull.Value));
                            break;
                        case "Single":
                            lstParams.Add(CreateParameter(lsFieldName, OracleDbType.Single, DBNull.Value));
                            break;
                        case "Double":
                            lstParams.Add(CreateParameter(lsFieldName, OracleDbType.Double, DBNull.Value));
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
		/// Tạo dữ liệu 1 record Oracle
		/// </summary>
        public int AddNewRow(DataRow pdrData, OracleCommand pcmdData, string psListFieldDest)
        {
			string lsFieldName, lsField, lsTypeSrc, lsListData = "", lsTableName;
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
                            pcmdData.Parameters[lsFieldName].Value = (ClsUtil.parseBool(pdrData, lsFieldName) ? 1 : 0);
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
                    if (lsFieldName.IndexOf(" ") > 0)
                        lsField = "\""+ lsFieldName + "\"";
                    else
                        lsField = lsFieldName;
                    lsListData += (lsListData.Length > 0 ? "," : "") + lsField + " = " + pcmdData.Parameters[lsFieldName].Value;
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
            string lsFields = "";
			OracleCommand lcmdData;
			List<OracleParameter> lstParams = new List<OracleParameter>();
			int i, j, lCount, lCountField, lRes = 0;

			lCount = pdtDataSrc.Rows.Count;
            lCountField = pdtDataSrc.Columns.Count;
            for(i = 0; i < lCountField; i++){
                lsTypeSrc = pdtDataSrc.Columns[i].DataType.Name;
                lsFieldName = pdtDataSrc.Columns[i].ColumnName;
                lsFields += (lsFields.Length == 0 ? "" : ",") + lsFieldName;
                switch(lsTypeSrc){
                    case "String":
                        lstParams.Add(CreateParameter(lsFieldName, OracleDbType.Varchar2, DBNull.Value));
                        break;
                    case "Int16": case "UInt16":
                        lstParams.Add(CreateParameter(lsFieldName, OracleDbType.Int16, DBNull.Value));
                        break;
                    case "Int32": case "UInt32":
                        lstParams.Add(CreateParameter(lsFieldName, OracleDbType.Int32, DBNull.Value));
                        break;
                    case "Int64"://long
                        lstParams.Add(CreateParameter(lsFieldName, OracleDbType.Int64, DBNull.Value));
                        break;
                    case "Byte":
                        lstParams.Add(CreateParameter(lsFieldName, OracleDbType.Byte, DBNull.Value));
                        break;
                    case "Boolean":
                        lstParams.Add(CreateParameter(lsFieldName, OracleDbType.Byte, DBNull.Value));
                        break;
                    case "DateTime":
                        lstParams.Add(CreateParameter(lsFieldName, OracleDbType.Date, DBNull.Value));
                        break;
                    case "Decimal":
                        lstParams.Add(CreateParameter(lsFieldName, OracleDbType.Decimal, DBNull.Value));
                        break;
                    case "Single":
                        lstParams.Add(CreateParameter(lsFieldName, OracleDbType.Single, DBNull.Value));
                        break;
                    case "Double":
                        lstParams.Add(CreateParameter(lsFieldName, OracleDbType.Double, DBNull.Value));
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
                            lcmdData.Parameters[lsFieldName].Value = (ClsUtil.parseBool(pdtDataSrc.Rows[i], lsFieldName) ? 1 : 0);
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
			}//of for i
			return lRes;
		}
        /// <summary>
		/// Tạo câu query Insert
		/// </summary>
		public string CreateStringSQL(string psSchema, string psTable, string psFields){
			string[] larrField;
			string lsRes, lsFieldName, lsFieldInsert = "";
			int i, lCount;
			larrField = psFields.Split(',');
			lsRes = "";
			lCount = larrField.Length;
			for (i = 0 ; i < lCount; i++){
                lsFieldName = larrField[i];
                if (lsFieldName.IndexOf(" ") > 0)
                    lsFieldName = "\"" + lsFieldName + "\"";
				lsRes += (lsRes.Equals("") ? "" : ",") + ":" + lsFieldName;
                lsFieldInsert += (lsFieldInsert.Equals("") ? "" : ",") + lsFieldName;
			}
			lsRes = "INSERT INTO " + (psSchema.Length > 0 ? psSchema + "." : "") + psTable + "(" + lsFieldInsert + ") VALUES (" + lsRes + ")";
			return lsRes;
		}
    #endregion //Add Data
	}
}
