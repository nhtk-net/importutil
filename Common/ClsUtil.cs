using System;
using System.Data;
using System.IO;
using System.Web;
using System.Configuration;
using System.Net.Mail;
using System.Security.Cryptography;
using System.Text;
using System.Linq;
using System.Collections;

namespace ImportUtil{
public class ClsUtil
{
	/// <summary>
	/// Tạo Stt có chiều dài >= 5
	/// </summary>
	public static void FillStt(ref string psStt){
		int lLen;
		lLen = psStt.Length;
		if(lLen < 5)
			psStt = psStt.PadLeft(5, '0');
	}
	public static string FillString(string psData, int pLen){
			string lsRes = "";
			int lLenData;
			lsRes = psData;
			lLenData = psData.Length;
			if(pLen != lLenData){
				lsRes = lsRes.PadLeft(2, '0');
				lLenData = lsRes.Length;
				lsRes = lsRes.Substring(lLenData - 2, pLen);
			}
			return lsRes;
		}
	public static string HashMD5(string data)
	{
		//mã hóa pwd
		//string sExample = System.Web.Security.FormsAuthentication.HashPasswordForStoringInConfigFile("greg", "MD5");
		MD5 md5 = new MD5CryptoServiceProvider();
		byte[] rawBytes = ASCIIEncoding.ASCII.GetBytes(data);
		byte[] hashData = md5.ComputeHash(rawBytes);
		string str = BitConverter.ToString(hashData);
		return str.Replace("-", "");
	}
    
	public static string Generate_Pwd()
	{
		string lsChainPwd = "";
        int lLen_Char;
		int liCount, i = 0, li_Char_Num_Idx;
		//lLen_Char : chiều dài pwd
		// List of characters composing the random chain
		//string[] lsChars = { "a", "b", "c", "d", "e", "f", "g", "h", "i", "j", "k", "l", "m", "n", "o", "p", "q", "r", "s", "t", "u", "v", "w", "x", "y", "z", "0", "1", "2", "3", "4", "5", "6", "7", "8", "9" };
        //không sử dụng số 1 và chữ l
        string[] lsChars = { "a", "b", "c", "d", "e", "f", "g", "h", "i", "j", "k", "m", "n", "o", "p", "q", "r", "s", "t", "u", "v", "w", "x", "y", "z", "0", "2", "3", "4", "5", "6", "7", "8", "9" };
		Random lrdRandom = new Random(DateTime.Now.Millisecond);
		liCount = lsChars.Length;
        //The letter’s size is random
		lLen_Char = lrdRandom.Next(8, 11); //generate 8 -> 10
		for (i = 0; i < lLen_Char; i++)
		{
			//The letter is random too.
			li_Char_Num_Idx = lrdRandom.Next(0, liCount); //generate from 0 to Len - 1, liCount: length of lsChars possibility 0 -> Len
			lsChainPwd += lsChars[li_Char_Num_Idx];
		}
		return lsChainPwd;
	}//end function
	
	public static string FormatInput(string strInputEntry) { 
		strInputEntry = strInputEntry.Replace ("&", "&amp;"); 
		strInputEntry = strInputEntry.Replace ("<", "&lt;"); 
		strInputEntry = strInputEntry.Replace (">", "&gt;"); 
		strInputEntry = strInputEntry.Replace ("'", "&#146;"); 
		strInputEntry = strInputEntry.Replace ("[", "&#091;"); 
		strInputEntry = strInputEntry.Replace ("]", "&#093;"); 
		strInputEntry = strInputEntry.Replace ("=", "&#061;"); 
		strInputEntry = strInputEntry.Replace ("select", "sel&#101;ct"); 
		strInputEntry = strInputEntry.Replace ("join", "jo&#105;n"); 
		strInputEntry = strInputEntry.Replace ("union", "un&#105;on"); 
		strInputEntry = strInputEntry.Replace ("where", "wh&#101;re"); 
		strInputEntry = strInputEntry.Replace ("insert", "ins&#101;rt"); 
		strInputEntry = strInputEntry.Replace ("delete", "del&#101;te"); 
		strInputEntry = strInputEntry.Replace ("update", "up&#100;ate"); 
		strInputEntry = strInputEntry.Replace ("like", "lik&#101;"); 
		strInputEntry = strInputEntry.Replace ("drop", "dro&#112;"); 
		strInputEntry = strInputEntry.Replace ("create", "cr&#101;ate"); 
		strInputEntry = strInputEntry.Replace ("modify", "mod&#105;fy"); 
		strInputEntry = strInputEntry.Replace ("rename", "ren&#097;me"); 
		strInputEntry = strInputEntry.Replace ("alter", "alt&#101;r"); 
		strInputEntry = strInputEntry.Replace ("cast", "ca&#115;t"); 
		strInputEntry = strInputEntry.Replace ("user", "us&#101;r"); 
		strInputEntry = strInputEntry.Replace (" and ", " &#097;nd "); 
		strInputEntry = strInputEntry.Replace (" or ", " &#111;r "); 
		strInputEntry = strInputEntry.Replace ("from", "fr&#111;m"); 
		return strInputEntry; 
	} 
	
	public static bool IsNumeric(string psVal) {
		//kiểm tra ? kiểu số, chi dung cho so nguyen
		return IsNumeric(psVal,true,true);
	}// end function
	public static bool IsNumeric(string psVal,bool pbNegative,bool pbDecimal)
	{	//kiểm tra ? kiểu số, chi dung cho so nguyen
		bool lbRes = true;
		string lstrChar;
		int lintLen, i;
		Double ldblVal;
		try{
			psVal = psVal.Trim();
			lintLen = psVal.Length;
			for (i = 0; lbRes && (i < lintLen); i++){
				lstrChar = psVal.Substring(i, 1);
				switch (lstrChar){
				case "0":
				case "1":
				case "2":
				case "3":
				case "4":
				case "5":
				case "6":
				case "7":
				case "8":
				case "9":
				case ",":
					break;
				case "-":
					if(!pbNegative)
						lbRes = false;
					break;
				case ".":
					if (!pbDecimal)
						lbRes = false;
					break;
				default:
					lbRes = false;
					break;
				}//end switch
				if (!lbRes)
					break;
			}//end for
			if (lbRes)//kiểm tra lại
				ldblVal = Double.Parse(psVal);
		}catch{
			lbRes = false;
		}
		return lbRes;
	}// end function
	public static int parseInt(string psVal){
		try{
			if(IsNumeric(psVal)){
				psVal = psVal.Replace(",", "");
				psVal = psVal.Replace(".", "");
				return int.Parse(psVal);
			}else
				return 0;
		}catch{return 0;}
	}
	public static int parseInt(object pVal){
		try{
			return parseInt(pVal.ToString());
		} catch { return 0; }
	}
	public static double GetDouble(string psVal){
		try{
			if(IsNumeric(psVal)){
				psVal = psVal.Replace(",", "");
				return double.Parse(psVal);
			}else
				return 0;
		}catch{return 0;}
	}
	public static string KeyConfig(string pstrKey) {
		string lstrValue = "";
		try {
			lstrValue = ConfigurationManager.AppSettings[pstrKey];
		}
		catch{}
		return lstrValue;
	}//end function
	
	public static bool SendMail(string pstrFrom,string pstrTo,string pstrServer,string pstrSubject,string pstrBody){
		bool lbRes = false;
		/*
		mail.From        = MailAddress pstrFrom;
		mail.To          = pstrTo;
		mail.BodyEncoding= System.Text.Encoding.UTF8;
		mail.Subject     = pstrSubject;
		mail.Body        = pstrBody;
		mail.BodyFormat  = MailFormat.Html;
		mail.Priority	 = MailPriority.Low;
		SmtpMail.SmtpServer = pstrServer;
		 */
		MailMessage mail = null;
		try {
			mail = new MailMessage();
			//email from : không được quá ngắn
			MailAddress from = new MailAddress(pstrFrom, pstrFrom);
			MailAddress to = new MailAddress(pstrTo, pstrTo);
			MailMessage message = new MailMessage(from, to);
			message.Subject = pstrSubject;
			message.Body = pstrBody;
			message.IsBodyHtml = true;
			SmtpClient client = new SmtpClient(pstrServer);
			// Include credentials if the server requires them.
			//client.Credentials = CredentialCache.DefaultNetworkCredentials;
			client.Send(message);
			lbRes = true;
		}catch(Exception ex) {
			throw new Exception("Không thể gởi được email. " + ex.Message);
		}finally {
			mail = null;
		}return lbRes;
	}
	private static string Right(string psVal, int pintLen)
	{
		string lstrRes;
		int lintLenStr;
		lstrRes = psVal.Trim();
		lintLenStr = lstrRes.Length;
		if (lintLenStr > pintLen)
			lstrRes = lstrRes.Substring(lintLenStr - pintLen, pintLen);
		return lstrRes;
	}
	public static string FillFullDate(string psVal)
	{
		string lstrRes;
		lstrRes = psVal.Trim();
		try
		{
			string lstrCurYear;
			string[] larrData;
			int lintLenFull, lintLenYear;
			if (psVal.Length != 10)
			{
				larrData = psVal.Split('/');
				psVal = Right("0" + larrData[0], 2) + "/" + Right("0" + larrData[1], 2);
				lstrCurYear = DateTime.Today.ToString("yyyy");
				lintLenYear = larrData[2].Length;
				lintLenFull = lstrCurYear.Length;
				if (lintLenYear < lintLenFull)
					lstrRes = psVal + "/" + lstrCurYear.Substring(0, lintLenFull - lintLenYear) + larrData[2];
				else
					lstrRes = psVal + "/" + larrData[2];
			}
		}
		catch { }
		return lstrRes;
	}
	public static DateTime? GetDate(string psDate){
		DateTime? ldatRes;
		GetDate(psDate, out ldatRes);
		return ldatRes;
	}
	public static bool GetDate(string psDate, out DateTime? pdatRes){
		bool lbRes = false;
		pdatRes = null;
		try{
			if (psDate != ""){
				psDate = psDate.Replace('.', '/');
				psDate = psDate.Replace('-', '/');
				if (psDate.Length != 10)
					psDate = FillFullDate(psDate);
				try
				{
					pdatRes = DateTime.ParseExact(psDate, "dd/MM/yyyy", null);
					lbRes = true;
				} catch { 
					pdatRes = null; 
				}
			}
		}
		catch { }
		return lbRes;
	}
    public static DateTime? GetDateTime(string psDate){
        int lLen;
		DateTime? pdatRes = null;
		try{
			if (psDate != ""){
				psDate = psDate.Replace('.', '/');
				psDate = psDate.Replace('-', '/');
				lLen = psDate.Length;
				if (lLen < 10)
					psDate = FillFullDate(psDate);
                lLen = psDate.Length;
				try
				{
					if(lLen == 10)
					    pdatRes = DateTime.ParseExact(psDate, "dd/MM/yyyy", null);
                    else if(lLen > 10)
					    pdatRes = DateTime.ParseExact(psDate, "dd/MM/yyyy HH:mm", null);
				}
				catch { }
			}
		}
		catch { }
		return pdatRes;
	}
    public static bool GetDate(string psFrom, string psTo, out DateTime? pdatFrom, out DateTime? pdatTo) {
        pdatFrom = null;
        pdatTo = null;
        bool lbRes = false;
        try {
            bool lbFrom = false, lbTo = false;
            DateTime? ldatDate;
            lbFrom = GetDate(psFrom, out pdatFrom);
            lbTo = GetDate(psTo, out pdatTo);
            if (lbFrom || lbTo) {
                if (!lbFrom)
                    pdatFrom = pdatTo;
                if (!lbTo)
                    pdatTo = pdatFrom;
                if (pdatFrom > pdatTo) {
                    ldatDate = pdatFrom;
                    pdatFrom = pdatTo;
                    pdatTo = ldatDate;
                }
                lbRes = true;
            }
        } catch { }
        return lbRes;
    }
    /// <summary>
    /// Lấy kiểu ngày DB Oracle
    /// </summary>
    public static string ParseStringDateDbOra(DateTime pdatRes){
        string lsRes = "";
        int lMonth;
        string[] laMonth = { "JAN", "FEB", "MAR", "APR", "MAY", "JUN", "JUL", "AUG", "SEP", "OCT", "NOV", "DEC" };
		try{
            lsRes = pdatRes.ToString("dd-");
            lMonth = pdatRes.Month - 1;
            lsRes += laMonth[lMonth] + pdatRes.ToString("-yy");
		}catch { }
		return lsRes;
	}
	public static bool IsDate(string psDate)
	{
		bool lbRes = false;
		DateTime pdatTest;
		pdatTest = DateTime.Today;
		try{
			if (psDate != ""){
				psDate = psDate.Replace('.', '/');
				psDate = psDate.Replace('-', '/');
				if (psDate.Length != 10)
					psDate = FillFullDate(psDate);
				try{
					pdatTest = DateTime.ParseExact(psDate, "dd/MM/yyyy", null);
					lbRes = true;
				}catch { }
			}
		}catch { }
		return lbRes;
	}
	private static string FillFullMonth(string psVal)
	{
		string lstrRes;
		lstrRes = psVal.Trim();
		try
		{
			string lstrCurYear;
			string[] larrData;
			int lintLenFull, lintLenYear;
			if (psVal.Length != 7)
			{
				larrData = psVal.Split('/');
				psVal = Right("0" + larrData[0], 2);
				lstrCurYear = DateTime.Today.ToString("yyyy");
				lintLenYear = larrData[1].Length;
				lintLenFull = lstrCurYear.Length;
				if (lintLenYear < lintLenFull)
					lstrRes = psVal + "/" + lstrCurYear.Substring(0, lintLenFull - lintLenYear) + larrData[1];
				else
					lstrRes = psVal + "/" + larrData[1];
			}
		}
		catch { }
		return lstrRes;
	}
	public static bool GetMonth(string psDate, out DateTime pdatRes)
	{
		bool lbRes = false;
		pdatRes = DateTime.Today;
		try
		{
			if (psDate != "")
			{
				psDate = psDate.Replace('.', '/');
				psDate = psDate.Replace('-', '/');
				if (psDate.Length != 7)
					psDate = FillFullMonth(psDate);
				try
				{
					pdatRes = DateTime.ParseExact(psDate, "MM/yyyy", null);
					lbRes = true;
				}
				catch { }
			}
		}
		catch { }
		return lbRes;
	}
	/// <summary>
	/// Kiểm tra thời gian quá 1 ngày ?
	/// </summary>
	public static bool IsExpire(DateTime? pNgay){
		bool lbExpire = false;
		DateTime lNgayTrunc;
		try{
			if(pNgay != Constants.NullDateTime){
				lNgayTrunc = DateTime.ParseExact(((DateTime) pNgay).ToString("dd/MM/yyyy"), "dd/MM/yyyy", null);
				if (DateTime.Today > lNgayTrunc)
					lbExpire = true;
			}
		}catch{}
		return lbExpire;
	}
    /// <summary>
	/// Kiểm tra thời gian quá 30 ngày ?
	/// </summary>
	public static bool IsPastDate(DateTime? pDate){
		bool lbPass = false;
		DateTime lDateF, lDateCheck;
		try{
			if(pDate != Constants.NullDateTime){
                lDateF = (DateTime)pDate;
                lDateCheck = DateTime.Today.AddDays(-30);
				if (lDateF < lDateCheck)
					lbPass = true;
			}
		}catch{}
		return lbPass;
	}
	
    private static string Space(int pNum){
        string kq = "";
        int i;
        for (i = 0; i < pNum; i++){
            kq = " " + kq;
        }
        return kq;
    }
    /// <summary>
    /// Đọc từ số sang chữ.
    /// </summary>
    public static string ReadNumber(int pSoInput){
        string KetQua = "", SoTien = "", Nhom = "", Dich = "";
        string Chu = "", S1 = "", S2 = "", S3 = "";
        int i, j, S, lLen, lTienAbs;
        string KHONG_DONG, SO_LON, TRU, TRAM, MUOI, GIDO, NGANTY, TY, TRIEU, NGAN, DONG, XU;
        string MOT, HAI, BA, BON, NAM, SAU, BAY, TAM, CHIN, LE, MUOII;
        #region Init
        KHONG_DONG = "Không đồng";
        SO_LON = "Số quá lớn";
        TRU = "trừ";
        TRAM = "trăm";
        MUOI = "mươi";
        GIDO = "gì đó";
        NGANTY = "ngàn tỷ";
        TY = "tỷ";
        TRIEU = "triệu";
        NGAN = "ngàn";
        DONG = "đồng";
        XU = "xu";
        MOT = "một";
        HAI = "hai";
        BA = "ba";
        BON = "bốn";
        NAM = "năm";
        SAU = "sáu";
        BAY = "bảy";
        TAM = "tám";
        CHIN = "chín";
        LE = "lẽ";
        MUOII = "mười";
        #endregion //Init
        string[] Hang = new string []{TRAM, MUOI, GIDO};
        string[] Doc = new string []{NGANTY, TY, TRIEU, NGAN, DONG, XU};
        string[] Dem = new string []{MOT, HAI, BA, BON, NAM, SAU, BAY, TAM, CHIN};
        lTienAbs = pSoInput;
        lLen = pSoInput.ToString().Length;
        if (pSoInput == 0)
            return KHONG_DONG;
        else if (lLen >= 16)
            return SO_LON;
        else{
            lTienAbs = Math.Abs(lTienAbs);
            SoTien = (Space(15) + lTienAbs.ToString());
            SoTien = SoTien.Substring(SoTien.Length - 15);

            for (i = 0; i < 5; i++){
                Nhom = SoTien.Substring(i * 3, 3);
                if (Nhom != Space(3)){
                    switch (Nhom){
                        case "000":
                            if (i == 4)
                                Chu = DONG + Space(1);
                            else
                                Chu = Space(0);
                            break;
                        default:
                            S1 = Nhom.Substring(0, 1).Replace(" ", "0");
                            S2 = Nhom.Substring(1, 1).Replace(" ", "0");
                            S3 = Nhom.Substring(2, 1).Replace(" ", "0");
                            Chu = Space(0);
                            Hang[2] = Doc[i];
                            for (j = 0; j < 3; j++)
                            {
                                Dich = Space(0);
                                if (Nhom.Substring(j, 1) == " " )
                                    S = 0;
                                else
                                    S = parseInt(Nhom.Substring(j, 1));
                                if (S > 0)
                                    Dich = Dem[S - 1] + Space(1) + Hang[j] + Space(1);
                                switch (j){
                                    case 1:
                                        if (S == 1)
                                            Dich = MUOII + Space(1);
                                        if (S == 0 && S3 != "0")
                                            if (((parseInt(S1) >= 1) && (parseInt(S1) <= 9)) || ((S1 == "0") && (i == 4)))
                                                Dich = LE + Space(1);
                                        break;
                                    case 2:
                                        if (S == 0 && Nhom != (Space(2) + "0"))
                                            Dich = Hang[j] + Space(1);
                                        if ((S == 5 && S2 != Space(1) && S2 != "0"))
                                            Dich = "l" + Dich.Substring(1);
                                        break;
                                }
                                Chu = Chu + Dich;
                            }
                            break;
                    }
                    KetQua = KetQua + Chu;
                }
            }
        }
        KetQua = KetQua.Substring(0, 1).ToUpper() + KetQua.Substring(1);
        if (lTienAbs <= 0)
              KetQua = TRU + Space(1) + KetQua;
        return KetQua;
    }
    /// <summary>
    /// Get list column name of table
    /// </summary>
    public static string ListColName(DataTable pdtData)
    {
        string lsRes = "";
        try
        {
            if (pdtData != null)
            {
                lsRes = (from dc in pdtData.Columns.Cast<DataColumn>()
                         select dc.ColumnName).Aggregate((current, next) => current.ToLower() + "," + next.ToLower());
                //lsRes = pdtData.Columns.Cast<DataColumn>().Select(column => column.ColumnName).Aggregate((current, next) => current + "," + next);
            }
        }
        catch { }
        return lsRes;
    }
    /// <summary>
    /// Kiểm tra id trong chuỗi
    /// </summary>
    public static bool IsInString(string psData, string psListData)
    {
        bool lbRes = false;
        if (psData != null && psData.Length > 0)
        {
            psData = "," + psData + ",";
            psListData = "," + psListData + ",";
            if (psListData.IndexOf(psData) >= 0)
                lbRes = true;
        }
        return lbRes;
    }
    /// <summary>
    /// Kiểm tra tên field có tồn tại trong danh sách
    /// </summary>
    public static bool IsExistField(string psField, string psListFields)
    {
        bool lbRes = true;
        if (psListFields != null && psListFields.Length > 0)
        {
            lbRes = IsInString(psField.ToLower(), psListFields);
        }
        return lbRes;
    }
    public static void GetChangeLog(ref string psLogChange, string psName, int pValOld, int pValNew){
		if (pValOld != pValNew)
			psLogChange += psName + " : " + pValOld + "->" + pValNew + ". ";
	}
    public static void GetChangeLog(ref string psLogChange, string psName, float? pValOld, float? pValNew){
		if (pValOld != pValNew)
			psLogChange += psName + " : " + pValOld + "->" + pValNew + ". ";
	}
	public static void GetChangeLog(ref string psLogChange, string psName, string psValOld, string psValNew){
		if (!psValOld.ToLower().Equals(psValNew.ToLower()))
			psLogChange += psName + " : " + psValOld + "->" + psValNew + ". ";
	}
#region Methods Parse
	/// <summary>
	/// return 0 if Field is null
	/// </summary>
	public static int parseInt(DataRow pdrRow, string psColName){
		//try{
			return (pdrRow[psColName] != DBNull.Value) ? Convert.ToInt32(pdrRow[psColName]) : 0;
		//}catch{return 0;}
	}
	public static int parseInt(DataRowView pdrRow, string psColName){
		//try{
			return (pdrRow[psColName] != DBNull.Value) ? Convert.ToInt32(pdrRow[psColName]) : 0;
		//}catch{return 0;}
	}
	/// <summary>
	/// return null if Field is null
	/// </summary>
	public static int? parseIntExt(DataRow pdrRow, string psColName){
		if(pdrRow[psColName] != DBNull.Value)
			return Convert.ToInt32(pdrRow[psColName]);
		else
			return null;
	}
	/// <summary>
	/// return Constants.NullInt if Field is null
	/// </summary>
	public static int parseInteger(DataRow pdrRow, string psColName){
		//try{
			return (pdrRow[psColName] != DBNull.Value) ? Convert.ToInt32(pdrRow[psColName]) : 0;
		//}catch{return 0;}
	}
    public static byte parseByte(DataRow pdrRow, string psColName){
		//try{
			return (pdrRow[psColName] != DBNull.Value) ? Convert.ToByte(pdrRow[psColName]) : (byte)0;
		//}catch{return 0;}
	}
	public static DateTime? parseDateTime(DataRow pdrRow, string psColName)
	{
		DateTime? lDate = null;
		//try{
			if (pdrRow[psColName] != DBNull.Value) lDate = Convert.ToDateTime(pdrRow[psColName]);
		//}catch{}
		return lDate;
	}
	public static bool parseBool(DataRow pdrRow, string psColName)
	{
		//try{
			return (pdrRow[psColName] != DBNull.Value) ? Convert.ToBoolean(pdrRow[psColName]) : false;
		//}catch{return false;}
	}
	public static string parseString(DataRow pdrRow, string psColName)
	{
		//try{
			return (pdrRow[psColName] != DBNull.Value) ? Convert.ToString(pdrRow[psColName]).Trim() : Constants.NullString;
		//}catch{return Constants.NullString;}
	}
	public static string parseString(DataRowView pdrRow, string psColName)
	{
		//try{
			return (pdrRow[psColName] != DBNull.Value) ? Convert.ToString(pdrRow[psColName]).Trim() : Constants.NullString;
		//}catch{return Constants.NullString;}
	}
	public static float parseSingle(DataRow pdrRow, string psColName)
	{
		//try{
			return (pdrRow[psColName] != DBNull.Value) ? Convert.ToSingle(pdrRow[psColName]) : 0;
		//}catch{return 0;}
	}
    public static double parseDouble(DataRow pdrRow, string psColName)
	{
		//try{
			return (pdrRow[psColName] != DBNull.Value) ? Convert.ToDouble(pdrRow[psColName]) : 0;
		//}catch{return 0;}
	}
    public static decimal parseDecimal(DataRow pdrRow, string psColName)
	{
		//try{
			return (pdrRow[psColName] != DBNull.Value) ? Convert.ToDecimal(pdrRow[psColName]) : 0;
		//}catch{return 0;}
	}
#endregion //Methods Parse
}
}