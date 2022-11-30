using System;
using System.Data;
using Oracle.DataAccess.Client;
using System.Configuration;

namespace ImportUtil.DataAccess
{
	public class BaseDataObject
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
		public BaseDataObject() {
			_isShareTxn = false;
			_isShareCxn = false;
		}
		public BaseDataObject (IDbConnection pCxn)
		{
		    if (pCxn == null)
		        _isShareCxn = false;
		    else{
		        _cxn = pCxn;
		        _isShareCxn = true;
		    }
			_isShareTxn = false;
		}
		public BaseDataObject(IDbTransaction pTxn)
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
		public BaseDataObject(IDbConnection pCxn, IDbTransaction pTxn)
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
					cxn = new OracleConnection(WebConfig.ConnectionString);
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
					_cxn = new OracleConnection(WebConfig.ConnectionString);
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
        protected OracleParameter CreateParameter(string paramName,
            OracleDbType paramType, object paramValue)
        {
            OracleParameter param = new OracleParameter(paramName, paramType);
            if (paramValue != DBNull.Value)
            {
                switch (paramType)
                {
                    case OracleDbType.Varchar2:
                    case OracleDbType.NVarchar2:
                    case OracleDbType.Char:
                    case OracleDbType.NChar:
						if(paramValue == null)
							paramValue = String.Empty;
                        paramValue = CheckParamValue(paramValue.ToString());
                        break;
                    case OracleDbType.Date:
                        paramValue = CheckParamValue((DateTime?)paramValue);
                        break;
                    case OracleDbType.Int16:
						paramValue = CheckParamValue(Convert.ToInt16(paramValue));
                        break;
					case OracleDbType.Int32:
					case OracleDbType.Int64:
					case OracleDbType.Long:
                        //paramValue = CheckParamValue((int?)paramValue);
                        paramValue = CheckParamValueInt(paramValue);
                        break;
                    case OracleDbType.Single:
                        paramValue = CheckParamValue(Convert.ToSingle(paramValue));
                        break;
                    case OracleDbType.Decimal:
                        paramValue = CheckParamValue(Convert.ToDecimal(paramValue));
                        break;
                }
            }
            param.Value = paramValue;
            return param;
        }

        protected OracleParameter CreateParameter(string paramName, OracleDbType paramType, ParameterDirection direction)
        {
            OracleParameter returnVal = CreateParameter(paramName, paramType, DBNull.Value);
            returnVal.Direction = direction;
            return returnVal;
        }

        protected OracleParameter CreateParameter(string paramName, OracleDbType paramType, object paramValue, ParameterDirection direction)
        {
            OracleParameter returnVal = CreateParameter(paramName, paramType, paramValue);
            returnVal.Direction = direction;
            return returnVal;
        }

        protected OracleParameter CreateParameter(string paramName, OracleDbType paramType, object paramValue, int size)
        {
            OracleParameter returnVal = CreateParameter(paramName, paramType, paramValue);
            returnVal.Size = size;
            return returnVal;
        }

        protected OracleParameter CreateParameter(string paramName, OracleDbType paramType, object paramValue, int size, ParameterDirection direction)
        {
            OracleParameter returnVal = CreateParameter(paramName, paramType, paramValue);
            returnVal.Direction = direction;
            returnVal.Size = size;
            return returnVal;
        }

        protected OracleParameter CreateParameter(string paramName, OracleDbType paramType, object paramValue, int size, byte precision)
        {
            OracleParameter returnVal = CreateParameter(paramName, paramType, paramValue);
            returnVal.Size = size;
            ((OracleParameter)returnVal).Precision = precision;
            return returnVal;
        }

        protected OracleParameter CreateParameter(string paramName, OracleDbType paramType, object paramValue, int size, byte precision, ParameterDirection direction)
        {
            OracleParameter returnVal = CreateParameter(paramName, paramType, paramValue);
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
            OracleCommand lcmdData;
            return ExecuteDataSet(out lcmdData, procName, procParams);
        }

		private DataSet ExecuteDataSet(out OracleCommand pcmdData, string procName, params IDataParameter[] procParams)
        {
            DataSet ldsData = null;
            OracleDataAdapter ldaData = null;
			int liParaCount;
            pcmdData = null;
            try{
                //Setup command object
                pcmdData = new OracleCommand(procName);
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
					pcmdData.Connection = (OracleConnection)_txn.Connection;
                    pcmdData.Transaction = (OracleTransaction)_txn;
				}else{
					OpenConnection();
					pcmdData.Connection = (OracleConnection)_cxn;
                }
                //Fill the dataset
				ldsData = new DataSet();
				ldaData = new OracleDataAdapter(pcmdData);
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
            OracleCommand lcmdData;
            return ExecuteDataTable(out lcmdData, procName, procParams);
        }
		
		private DataTable ExecuteDataTable(out OracleCommand pcmdData, string procName, params IDataParameter[] procParams)
        {
            DataTable ldtData = null;
            OracleDataAdapter ldaData = null;
			int liParaCount;
            pcmdData = null;
            try{
                //Setup command object
                pcmdData = new OracleCommand(procName);
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
					pcmdData.Connection = (OracleConnection)_txn.Connection;
                    pcmdData.Transaction = (OracleTransaction)_txn;
				}else{
					OpenConnection();
					pcmdData.Connection = (OracleConnection)_cxn;
                }
				ldtData = new DataTable();
				ldaData = new OracleDataAdapter(pcmdData);
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
		public int ExecuteNonQuery(OracleCommand pcmdData)
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
            OracleCommand cmd;
            return ExecuteNonQuery(out cmd, procName, procParams);
        }
        
		private int ExecuteNonQuery(out OracleCommand pcmdData, string procName, params IDataParameter[] procParams)
        {
            //Method variables
            pcmdData = null;  //Avoids "Use of unassigned variable" compiler error
			int liParaCount, liRes = 0;
            try{
                //Setup command object
                pcmdData = new OracleCommand(procName);
                pcmdData.CommandType = (procName.Trim().IndexOf(" ") > 0 ? CommandType.Text : CommandType.StoredProcedure);
				liParaCount = procParams.Length;
                for (int index = 0; index < liParaCount; index++)
                {
                    pcmdData.Parameters.Add(procParams[index]);
                }
				//Determine the transaction owner and process accordingly
				if (_isShareTxn){
					pcmdData.Connection = (OracleConnection)_txn.Connection;
                    pcmdData.Transaction = (OracleTransaction)_txn;
				}else{
					OpenConnection();
					pcmdData.Connection = (OracleConnection)_cxn;
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
		public OracleCommand CreateCommand(string procName, params IDataParameter[] procParams)
        {
            OracleCommand lcmdData = null;  //Avoids "Use of unassigned variable" compiler error
			int liParaCount;
            try{
                //Setup command object
                lcmdData = new OracleCommand(procName);
                lcmdData.CommandType = (procName.Trim().IndexOf(" ") > 0 ? CommandType.Text : CommandType.StoredProcedure);
				liParaCount = procParams.Length;
                for (int index = 0; index < liParaCount; index++)
                {
                    lcmdData.Parameters.Add(procParams[index]);
                }
                //Determine the transaction owner and process accordingly
				if (_isShareTxn){
					lcmdData.Connection = (OracleConnection)_txn.Connection;
                    lcmdData.Transaction = (OracleTransaction)_txn;
				}else{
					OpenConnection();
					lcmdData.Connection = (OracleConnection)_cxn;
                }
            }catch{
                throw;
			}
			return lcmdData;
        }
	#endregion //CreateCommand Methods
	}
}
