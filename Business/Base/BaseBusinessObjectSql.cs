using System;
using System.Data;

namespace ImportUtil
{
	public class BaseBusinessObjectSql : BaseGets
	{
	#region Attributes
		private IDbConnection _cxn;			//Reference to the current transaction
		private IDbTransaction _txn;		//Reference to the current connection
		////////////////////////////////////////////////////////////////////////
		public IDbConnection Cxn
        {
            get { return _cxn; }
			set { _cxn = value; }
            //not use set : avoid refuse connection in use
        }
		public IDbTransaction Txn
        {
            get { return _txn; }
        }
	#endregion //Attributes

	#region Map Data
		public virtual bool MapData(DataSet pdsData)
		{
			if (pdsData != null && pdsData.Tables.Count > 0 && pdsData.Tables[0].Rows.Count > 0)
			{
				return MapData(pdsData.Tables[0].Rows[0]);
			}
			else
			{
				return false;
			}
		}

		//////////////////////////////////////////////////////////////////////////////
		public virtual bool MapData(DataTable pdtData)
		{
			if (pdtData != null && pdtData.Rows.Count > 0)
			{
				return MapData(pdtData.Rows[0]);
			}
			else
			{
				return false;
			}
		}

		//////////////////////////////////////////////////////////////////////////////
		public virtual bool MapData(DataRow pdrData)
		{
			//You can put common data mapping items here (e.g. create date, modified date, etc)
			return true;
		}
	#endregion //MapData

	#region Connection and Transaction New Methods
		public static void OpenData(ref IDbConnection cxn){
			DataAccess.BaseDataObjectSql.OpenData(ref cxn);
		}
		public static void CloseConnect(ref IDbConnection cxn){
			DataAccess.BaseDataObjectSql.CloseConnect(ref cxn);
        }
		public static void BeginTransaction(ref IDbConnection cxn, ref IDbTransaction txn)
		{
			txn = DataAccess.BaseDataObjectSql.BeginTransaction(ref cxn);
		}
		public static void Commit(ref IDbTransaction txn){
			try{
				DataAccess.BaseDataObjectSql.Commit(ref txn);
			}catch(Exception ex){
				throw new Exception("[Commit] " + ex.Message);
			}
        }
		public static void Rollback(ref IDbTransaction txn){
			try{
				DataAccess.BaseDataObjectSql.Rollback(ref txn);
			}catch(Exception ex){
				throw new Exception("[Rollback] " + ex.Message);
			}
        }
		public void OpenData(){
			DataAccess.BaseDataObjectSql.OpenData(ref _cxn);
		}
		
		public void CloseConnect(){
			DataAccess.BaseDataObjectSql.CloseConnect(ref _cxn, ref _txn);
        }
		/// <summary>
		/// Begin a business transaction from an opened connection
		/// </summary>
		public void BeginTransaction()
		{
			_txn = DataAccess.BaseDataObjectSql.BeginTransaction(ref _cxn);
		}
		public void Commit(){
			try{
				DataAccess.BaseDataObjectSql.Commit(ref _txn);
			}catch(Exception ex){
				throw new Exception("[Commit] " + ex.Message);
			}
        }
		public void Rollback(){
			try{
				DataAccess.BaseDataObjectSql.Rollback(ref _txn);
			}catch(Exception ex){
				throw new Exception("[Rollback] " + ex.Message);
			}
        }
	#endregion //Connection and Transaction New Methods

	}
}
