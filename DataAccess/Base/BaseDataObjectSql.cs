using System;
using System.Data;
using System.Configuration;
using System.Data.SqlClient;

namespace ImportUtil.DataAccess
{
	public class BaseDataObjectSql
	{//should check share connection and transaction, not true
	#region Fields
        private bool _isShareTxn = false;   //True if service has transaction open outside
		private bool _isShareCxn = false;	//True if service has the connection open outside
        private IDbConnection _cxn;			//Reference to the current transaction
		private IDbTransaction _txn;		//Reference to the current connection
    #endregion // Fields

    #region Properties
        ////////////////////////////////////////////////////////////////////////
        public IDbTransaction Txn
        {
            get { return _txn; }
            //set { _txn = value; }
        }

		public IDbConnection Cxn
        {
            get { return _cxn; }
            //set { _cxn = value; } avoid refuse connection in use
        }
	#endregion //Properties

    #region Constructors
		public BaseDataObjectSql() {
			_isShareTxn = false;
			_isShareCxn = false;
		}
		public BaseDataObjectSql (IDbConnection pCxn)
		{
		    if (pCxn == null)
		        _isShareCxn = false;
		    else{
		        _cxn = pCxn;
		        _isShareCxn = true;
		    }
			_isShareTxn = false;
		}
		public BaseDataObjectSql(IDbTransaction pTxn)
        {
            if (pTxn == null){
                _isShareTxn = false;
				_isShareCxn = false;
            }else{
                _txn = pTxn;
				_cxn = pTxn.Connection;
                _isShareTxn = true;
				_isShareCxn = true;
            }
        }
		public BaseDataObjectSql(IDbConnection pCxn, IDbTransaction pTxn)
		{
		    if (pCxn == null){
		        _isShareCxn = false;
				_isShareTxn = false;
		    }else{
				_isShareCxn = true;
				if (pTxn == null){
					_cxn = pCxn;
					_isShareTxn = false;
				}else{
					_txn = pTxn;
					_cxn = pTxn.Connection;
					_isShareTxn = true;
				}
		    }
		}
	#endregion Constructors

	#region Connection and Transaction Methods
		#region Methods Connect, Close For Outside
		public static IDbTransaction BeginTransaction(ref IDbConnection cxn){
			IDbTransaction txn = null;
            try{
				txn = cxn.BeginTransaction();
			}catch (Exception ex){
                throw new Exception("Không mở được Database.\n\r" + ex.Message);
            }return txn;
        }
		public static void OpenData(ref IDbConnection cxn){
            try{
				if (cxn != null){
					if(cxn.State != ConnectionState.Broken
						&& cxn.State != ConnectionState.Connecting
						&& cxn.State != ConnectionState.Executing
						&& cxn.State != ConnectionState.Fetching
						&& cxn.State != ConnectionState.Open)
							cxn.Open();
				}else {
					cxn = new SqlConnection(WebConfig.ConnectionStringSQL);
					cxn.Open();
				}
			}catch (Exception ex){
                throw new Exception("Không mở được Database.\n\r" + ex.Message);
            }
        }
		public static void CloseConnect(ref IDbConnection cxn){
            try{
                if (cxn != null){
                    cxn.Close();
					cxn.Dispose(); cxn = null;
                }
            }catch{
                //throw ex;
            }
        }
		public static void CloseConnect(ref IDbConnection cxn, ref IDbTransaction txn){
            try{
                if (cxn != null){
                    cxn.Close();
					cxn.Dispose(); cxn = null;
                }if(txn !=null){
					txn.Dispose(); txn = null;
				}
            }catch{
                //throw ex;
            }
        }
		#endregion //Methods Connect, Close For Outside
		
		protected void OpenConnection() {
			try{
				if (_isShareCxn){//check exist connection is open
                    if (_cxn != null && 
                        _cxn.State != ConnectionState.Broken
                        && _cxn.State != ConnectionState.Connecting
                        && _cxn.State != ConnectionState.Executing
                        && _cxn.State != ConnectionState.Fetching
                        && _cxn.State != ConnectionState.Open)
                        _cxn.Open();
				}else{
					_cxn = new SqlConnection(WebConfig.ConnectionStringSQL);
					_cxn.Open();
				}
			}catch (Exception ex){
                throw new Exception("Không mở được Database. " + ex.Message);
            }
        }
		private void CloseConnect(){
            try{
				_isShareCxn = false;
				_isShareTxn = false;
                if (_cxn != null){
                    _cxn.Close();
                    _cxn.Dispose(); _cxn = null;
                }
            }catch{
                //throw ex;
            }
        }
	#endregion //Connection and Transaction Methods

    #region Commit & Rollback Transaction Methods
        ////////////////////////////////////////////////////////////////////////
		#region Methods For Outside
		public static void Commit(ref IDbTransaction txn){
            txn.Commit();
			txn.Dispose(); txn = null;
        }
		public static void Rollback(ref IDbTransaction txn){
            try{
                txn.Rollback();
				txn.Dispose(); txn = null;
            }catch{ }
        }
		#endregion //Methods For Outside
		////////////////////////////////////////////////////////////////////////
		#region Methods For Inside
		//chưa sử dụng
		private void Commit(){
            _txn.Commit();
        }
        private void Commit(bool closeConnection){
            _txn.Commit();
			if(closeConnection){
				_isShareCxn = false;
				_isShareTxn = false;
				_txn.Connection.Close();
			}
        }
		private void Rollback(){
            try{
                _txn.Rollback();
            }catch{ }
        }
        private void Rollback(bool closeConnection){
            try{
                _txn.Rollback();
				if(closeConnection){
					_isShareCxn = false;
					_isShareTxn = false;
					_txn.Connection.Close();
				}
            }catch{ }
		}
		#endregion //Methods For Inside
	#endregion //Commit & Rollback Transaction Methods

	#region CreateParameter Methods
        protected SqlParameter CreateParameter(string paramName,
            SqlDbType paramType, object paramValue)
        {
            SqlParameter param = new SqlParameter(paramName, paramType);
            if (paramValue != DBNull.Value)
            {
                switch (paramType)
                {
                    case SqlDbType.VarChar:
                    case SqlDbType.NVarChar:
                    case SqlDbType.Char:
                    case SqlDbType.NChar:
						if(paramValue == null)
							paramValue = String.Empty;
                        paramValue = CheckParamValue(paramValue.ToString());
                        break;
                    case SqlDbType.Date:
                        paramValue = CheckParamValue((DateTime?)paramValue);
                        break;
                    case SqlDbType.SmallInt:
						paramValue = CheckParamValue(Convert.ToInt16(paramValue));
                        break;
					case SqlDbType.Int:
					case SqlDbType.BigInt:
                        paramValue = CheckParamValueInt(paramValue);
                        break;
                    case SqlDbType.Real:
                    case SqlDbType.Float:
                        paramValue = CheckParamValue(Convert.ToSingle(paramValue));
                        break;
                    case SqlDbType.Decimal:
                        paramValue = CheckParamValue(Convert.ToDecimal(paramValue));
                        break;
                }
            }
            param.Value = paramValue;
            return param;
        }

        protected SqlParameter CreateParameter(string paramName, SqlDbType paramType, ParameterDirection direction)
        {
            SqlParameter returnVal = CreateParameter(paramName, paramType, DBNull.Value);
            returnVal.Direction = direction;
            return returnVal;
        }

        protected SqlParameter CreateParameter(string paramName, SqlDbType paramType, object paramValue, ParameterDirection direction)
        {
            SqlParameter returnVal = CreateParameter(paramName, paramType, paramValue);
            returnVal.Direction = direction;
            return returnVal;
        }

        protected SqlParameter CreateParameter(string paramName, SqlDbType paramType, object paramValue, int size)
        {
            SqlParameter returnVal = CreateParameter(paramName, paramType, paramValue);
            returnVal.Size = size;
            return returnVal;
        }

        protected SqlParameter CreateParameter(string paramName, SqlDbType paramType, object paramValue, int size, ParameterDirection direction)
        {
            SqlParameter returnVal = CreateParameter(paramName, paramType, paramValue);
            returnVal.Direction = direction;
            returnVal.Size = size;
            return returnVal;
        }

        protected SqlParameter CreateParameter(string paramName, SqlDbType paramType, object paramValue, int size, byte precision)
        {
            SqlParameter returnVal = CreateParameter(paramName, paramType, paramValue);
            returnVal.Size = size;
            ((SqlParameter)returnVal).Precision = precision;
            return returnVal;
        }

        protected SqlParameter CreateParameter(string paramName, SqlDbType paramType, object paramValue, int size, byte precision, ParameterDirection direction)
        {
            SqlParameter returnVal = CreateParameter(paramName, paramType, paramValue);
            returnVal.Direction = direction;
            returnVal.Size = size;
            returnVal.Precision = precision;
            return returnVal;
        }
	#endregion //CreateParameter Methods
    
    #region CheckParamValue Methods
        protected object CheckParamValue(string paramValue)
        {
            if (paramValue == null || paramValue.Equals(Constants.NullString))
				return DBNull.Value;
            else
				return paramValue;
        }

		protected object CheckParamValue(DateTime? paramValue)
        {
            if (paramValue == Constants.NullDateTime)
                return DBNull.Value;
            else
                return paramValue;
        }

        protected object CheckParamValue(double? paramValue)
        {
            if (paramValue == Constants.NullDouble)
                return DBNull.Value;
            else
                return paramValue;
        }

        protected object CheckParamValue(float? paramValue)
        {
            if (paramValue == Constants.NullFloat)
                return DBNull.Value;
            else
                return paramValue;
        }

        protected object CheckParamValue(Decimal? paramValue)
        {
            if (paramValue == Constants.NullDecimal)
                return DBNull.Value;
            else
                return paramValue;
        }
        protected object CheckParamValueInt(object paramValue)
        {
            if (paramValue == null)
                return DBNull.Value;                
            else
                return paramValue;
        }
        protected object CheckParamValue(int? paramValue)
        {
            if (paramValue == Constants.NullInt)
                return DBNull.Value;                
            else
                return paramValue;
        }
		protected object CheckParamValue(Int16? paramValue)
        {
            if (paramValue == Constants.NullInt16)
                return DBNull.Value;                
            else
                return paramValue;
        }
	#endregion //CheckParamValue Methods

	#region ExecuteDataSet Methods
		public DataSet ExecuteDataSet(string procName, params IDataParameter[] procParams)
        {
            SqlCommand lcmdData;
            return ExecuteDataSet(out lcmdData, procName, procParams);
        }

		private DataSet ExecuteDataSet(out SqlCommand pcmdData, string procName, params IDataParameter[] procParams)
        {
            DataSet ldsData = null;
            SqlDataAdapter ldaData = null;
			int liParaCount;
            pcmdData = null;
            try{
                //Setup command object
                pcmdData = new SqlCommand(procName);
                pcmdData.CommandType = (procName.Trim().IndexOf(" ") > 0 ? CommandType.Text : CommandType.StoredProcedure);
                if (procParams != null){
					liParaCount = procParams.Length;
                    for (int index = 0; index < liParaCount; index++)
                    {
                        pcmdData.Parameters.Add(procParams[index]);
                    }
                }
                //Determine the transaction owner and process accordingly
				if (_isShareTxn){
					pcmdData.Connection = (SqlConnection)_txn.Connection;
                    pcmdData.Transaction = (SqlTransaction)_txn;
				}else{
					OpenConnection();
					pcmdData.Connection = (SqlConnection)_cxn;
                }
                //Fill the dataset
				ldsData = new DataSet();
				ldaData = new SqlDataAdapter(pcmdData);
                ldaData.Fill(ldsData);
            }catch{
                throw;
            }finally{
                if(ldaData != null) ldaData.Dispose();
                if(pcmdData != null) pcmdData.Dispose();
				if (!_isShareCxn && _cxn != null)
                    _cxn.Dispose(); //Implicitly calls cnx.Close()
            }
            return ldsData;
        }
	#endregion //ExecuteDataSet Methods
	
    #region ExecuteDataTable Methods
		public DataTable ExecuteDataTable(string procName, params IDataParameter[] procParams)
        {
            SqlCommand lcmdData;
            return ExecuteDataTable(out lcmdData, procName, procParams);
        }
		
		private DataTable ExecuteDataTable(out SqlCommand pcmdData, string procName, params IDataParameter[] procParams)
        {
            DataTable ldtData = null;
            SqlDataAdapter ldaData = null;
			int liParaCount;
            pcmdData = null;
            try{
                //Setup command object
                pcmdData = new SqlCommand(procName);
                pcmdData.CommandType = (procName.Trim().IndexOf(" ") > 0 ? CommandType.Text : CommandType.StoredProcedure);
                if (procParams != null){
					liParaCount = procParams.Length;
                    for (int index = 0; index < liParaCount; index++)
                    {
                        pcmdData.Parameters.Add(procParams[index]);
                    }
                }
                //Determine the transaction owner and process accordingly
				if (_isShareTxn){
					pcmdData.Connection = (SqlConnection)_txn.Connection;
                    pcmdData.Transaction = (SqlTransaction)_txn;
				}else{
					OpenConnection();
					pcmdData.Connection = (SqlConnection)_cxn;
                }
				ldtData = new DataTable();
				ldaData = new SqlDataAdapter(pcmdData);
                //Fill the datatable
                ldaData.Fill(ldtData);
            }catch{
                throw;
            }finally{
                if(ldaData != null) ldaData.Dispose();
                if(pcmdData != null) pcmdData.Dispose();
				if (!_isShareCxn && _cxn != null)
                    _cxn.Dispose(); //Implicitly calls cnx.Close()
            }
            return ldtData;
        }
	#endregion //ExecuteDataTable Methods

	#region ExecuteNonQuery Methods
        /// <summary>
		/// Thực thi command, không giải phóng biến pcmdData
		/// </summary>
		public int ExecuteNonQuery(SqlCommand pcmdData)
        {
			int liRes = 0;
            try{
                //Execute the command
                liRes = pcmdData.ExecuteNonQuery();
            }catch{
                throw;
			}
			return liRes;
        }
        public int ExecuteNonQuery(string procName, params IDataParameter[] procParams)
        {
            SqlCommand cmd;
            return ExecuteNonQuery(out cmd, procName, procParams);
        }
        
		private int ExecuteNonQuery(out SqlCommand pcmdData, string procName, params IDataParameter[] procParams)
        {
            //Method variables
            pcmdData = null;  //Avoids "Use of unassigned variable" compiler error
			int liParaCount, liRes = 0;
            try{
                //Setup command object
                pcmdData = new SqlCommand(procName);
                pcmdData.CommandType = (procName.Trim().IndexOf(" ") > 0 ? CommandType.Text : CommandType.StoredProcedure);
				liParaCount = procParams.Length;
                for (int index = 0; index < liParaCount; index++)
                {
                    pcmdData.Parameters.Add(procParams[index]);
                }
				//Determine the transaction owner and process accordingly
				if (_isShareTxn){
					pcmdData.Connection = (SqlConnection)_txn.Connection;
                    pcmdData.Transaction = (SqlTransaction)_txn;
				}else{
					OpenConnection();
					pcmdData.Connection = (SqlConnection)_cxn;
                }
                //Execute the command
                liRes = pcmdData.ExecuteNonQuery();
            }catch{
                throw;
			}finally{
                if(pcmdData != null) pcmdData.Dispose();
                if (!_isShareCxn && _cxn != null)
                    _cxn.Dispose(); //Implicitly calls cnx.Close()
            }
			return liRes;
        }
	#endregion //ExecuteNonQuery Methods

    #region CreateCommand Methods
		/// <summary>
		/// Tạo command
		/// </summary>
		public SqlCommand CreateCommand(string procName, params IDataParameter[] procParams)
        {
            SqlCommand lcmdData = null;  //Avoids "Use of unassigned variable" compiler error
			int liParaCount;
            try{
                //Setup command object
                lcmdData = new SqlCommand(procName);
                lcmdData.CommandType = (procName.Trim().IndexOf(" ") > 0 ? CommandType.Text : CommandType.StoredProcedure);
				liParaCount = procParams.Length;
                for (int index = 0; index < liParaCount; index++)
                {
                    lcmdData.Parameters.Add(procParams[index]);
                }
                //Determine the transaction owner and process accordingly
				if (_isShareTxn){
					lcmdData.Connection = (SqlConnection)_txn.Connection;
                    lcmdData.Transaction = (SqlTransaction)_txn;
				}else{
					OpenConnection();
					lcmdData.Connection = (SqlConnection)_cxn;
                }
            }catch{
                throw;
			}
			return lcmdData;
        }
	#endregion //CreateCommand Methods
	}
}
