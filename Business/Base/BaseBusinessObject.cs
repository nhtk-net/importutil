using System;
using System.Data;

namespace ImportUtil
{
	public class BaseBusinessObject : BaseGets
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
			DataAccess.BaseDataObject.OpenData(ref cxn);
		}
		public static void CloseConnect(ref IDbConnection cxn){
			DataAccess.BaseDataObject.CloseConnect(ref cxn);
        }
		public static void BeginTransaction(ref IDbConnection cxn, ref IDbTransaction txn)
		{
			txn = DataAccess.BaseDataObject.BeginTransaction(ref cxn);
		}
		public static void Commit(ref IDbTransaction txn){
			try{
				DataAccess.BaseDataObject.Commit(ref txn);
			}catch(Exception ex){
				throw new Exception("[Commit] " + ex.Message);
			}
        }
		public static void Rollback(ref IDbTransaction txn){
			try{
				DataAccess.BaseDataObject.Rollback(ref txn);
			}catch(Exception ex){
				throw new Exception("[Rollback] " + ex.Message);
			}
        }
		public void OpenData(){
			DataAccess.BaseDataObject.OpenData(ref _cxn);
		}
		
		public void CloseConnect(){
			DataAccess.BaseDataObject.CloseConnect(ref _cxn, ref _txn);
        }
		/// <summary>
		/// Begin a business transaction from an opened connection
		/// </summary>
		public void BeginTransaction()
		{
			_txn = DataAccess.BaseDataObject.BeginTransaction(ref _cxn);
		}
		public void Commit(){
			try{
				DataAccess.BaseDataObject.Commit(ref _txn);
			}catch(Exception ex){
				throw new Exception("[Commit] " + ex.Message);
			}
        }
		public void Rollback(){
			try{
				DataAccess.BaseDataObject.Rollback(ref _txn);
			}catch(Exception ex){
				throw new Exception("[Rollback] " + ex.Message);
			}
        }
	#endregion //Connection and Transaction New Methods

	}
}
