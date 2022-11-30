using System;
using System.Data;
using System.Configuration;
/// <summary>
/// Configuration of Website
/// </summary>
namespace ImportUtil{
	public class WebConfig
	{
	#region Attributes
		private static string _ConnectionString = Constants.NullString;
        private static string _ConnectionStringSQL = Constants.NullString;
        private static string _ConnectionStringMySQL = Constants.NullString;
        private static string _ConnectionStringOleDB = Constants.NullString;
		private static string _DataDir = Constants.NullString;
		private static int _SmallImageWidth = 110;
		private static string _URL = Constants.NullString;
		private static string _SmtpServer = "221.132.39.167";
		private static string _Email = Constants.NullString;
		private static bool _DebugMode = true;
		private static int _PageSize = 30;
	#endregion //Attributes

	#region Properties
		public static string ConnectionString{
			get{return _ConnectionString;}
			set{_ConnectionString = value;}
		}
        public static string ConnectionStringSQL{
			get{return _ConnectionStringSQL;}
			set{_ConnectionStringSQL = value;}
		}
        public static string ConnectionStringMySQL
        {
            get { return _ConnectionStringMySQL; }
            set { _ConnectionStringMySQL = value; }
        }
        public static string ConnectionStringOleDB{
			get{return _ConnectionStringOleDB;}
			set{_ConnectionStringOleDB = value;}
		}
		public static string DataDir{
			get{return _DataDir;}
		}
		public static int SmallImageWidth{
			get{return _SmallImageWidth;}
		}
		public static string URL{
			get{return _URL;}
		}
		public static string SmtpServer{
			get{return _SmtpServer;}
		}
		public static string Email{
			get{return _Email;}
		}
		public static bool DebugMode{
			get{return _DebugMode;}
		}
		public static int PageSize{
			get{return _PageSize;}
		}
	#endregion //Properties

	#region Contructors
		static WebConfig(){
			try{
				_ConnectionString = ConfigurationManager.AppSettings["connectionString"];
				if (_ConnectionString == null)
				{
					if(ConfigurationManager.ConnectionStrings["connectionString"] != null)
						_ConnectionString = ConfigurationManager.ConnectionStrings["connectionString"].ConnectionString;
				}
                _ConnectionStringSQL = ConfigurationManager.AppSettings["connectionStringSql"];
                _ConnectionStringMySQL = ConfigurationManager.AppSettings["connectionStringMySql"];
                _ConnectionStringOleDB = ConfigurationManager.AppSettings["connectionStringOleDB"];
				_DataDir = parseString(ConfigurationManager.AppSettings["DATA_DIR"], _DataDir);
				_SmallImageWidth = parseInt("SmallImageWidth", _SmallImageWidth);
				_URL = parseString(ConfigurationManager.AppSettings["URL"], _URL);
				_SmtpServer = parseString("SmtpServer", _SmtpServer);
				_DebugMode = parseBool("DebugMode", _DebugMode);
				_PageSize = parseInt("PAGESIZE", _PageSize);
			}catch{}
		}
	#endregion //Contructors

	#region Methods
		public static int parseInt(string psKey, int pDefault){
			return (ConfigurationManager.AppSettings[psKey] != null ? ClsUtil.parseInt(ConfigurationManager.AppSettings[psKey]) : pDefault);
		}
		public static string parseString(string psKey, string psDefault){
			return (ConfigurationManager.AppSettings[psKey] != null ? ConfigurationManager.AppSettings[psKey] : psDefault);
		}
		public static bool parseBool(string psKey, bool pbDefault){
			return (ConfigurationManager.AppSettings[psKey] != null ? (ConfigurationManager.AppSettings[psKey] == "true" ? true : false) : pbDefault);
		}
	#endregion //Methods
	}
}