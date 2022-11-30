using System;
using System.Collections.Generic;
using System.Text;
using System.IO;
using System.Linq;
using System.Xml.Linq;
namespace ImportUtil {
	public class ClsFile {
	#region Folder
		public static void CreateFolder(string psPath)
		{			
			try{
				if (!Directory.Exists(psPath))
					Directory.CreateDirectory(psPath);
			}catch {}
		}
	#endregion //Folder

	#region File
		public static bool IsExistsFile(string psFileName)
		{
			return (File.Exists(psFileName));
		}
		public static bool WriteToFile(string pstrPath, string psData) {
			// Writes file to folder server
			bool lbRes = false;
			try {//Encoding.Default
				using (StreamWriter lswWrite = new StreamWriter(pstrPath, false, Encoding.UTF8)) {
					// Add some text to the file.
					lswWrite.Write(psData);
					lswWrite.Close();
				}
				lbRes = true;
			}catch{}
			return lbRes;
		}
		public static string ReadFile(string psFile) {
			string lsContent = "";
			if (File.Exists(psFile)) {
				try {
					using (StreamReader lsrRead = new StreamReader(psFile, Encoding.UTF8)){
						while (lsrRead.Peek() >= 0){
							lsContent = lsrRead.ReadToEnd();
						}
						lsrRead.Close();
					}
				} catch{}
			}
			return lsContent;
		}
	#endregion //File

	#region File XML
		public static void WriteXML(ImpSetting pisSetting, string psFile){
			// Create the new XML document.
			XDocument lNewDoc;
			try{
				lNewDoc = new XDocument(
						new XDeclaration("1.0", "utf-8", "yes"),
						new XElement("Provider", "Info Provider"));
				if(pisSetting != null){
					lNewDoc.Element("Provider").Add(
						new XElement("Source", 
							new XElement("Oracle", pisSetting.Source.Oracle),
							new XElement("MsSQL", pisSetting.Source.MsSQL),
							new XElement("MySQL", pisSetting.Source.MySQL),
							new XElement("Fox", pisSetting.Source.Fox),
							new XElement("Excel", pisSetting.Source.Excel),
                            new XElement("Excel2007", pisSetting.Source.Excel2007),
							new XElement("Access", pisSetting.Source.Access)));
                    lNewDoc.Element("Provider").Add(
						new XElement("Dest", 
							new XElement("Oracle", pisSetting.Dest.Oracle),
							new XElement("MsSQL", pisSetting.Dest.MsSQL),
							new XElement("MySQL", pisSetting.Dest.MySQL),
							new XElement("Fox", pisSetting.Dest.Fox),
							new XElement("Excel", pisSetting.Dest.Excel),
                            new XElement("Excel2007", pisSetting.Dest.Excel2007),
							new XElement("Access", pisSetting.Dest.Access)));
				}
				lNewDoc.Save(psFile);
				lNewDoc = null;
			} catch(Exception ex) { 
				throw new Exception("[WriteXML] " + ex.Message); 
			}
		}
		public static void ReadXML(ref ImpSetting pisSetting, string psFile){
			// Load the document.
			XDocument lNewDoc;
			XElement leSetting;
			try{
				if (File.Exists(psFile)) {
					lNewDoc = XDocument.Load(psFile);
					if(pisSetting == null)
						pisSetting = new ImpSetting();
					leSetting = lNewDoc.Element("Provider").Element("Source");
					if(leSetting != null){
						pisSetting.Source.Oracle = leSetting.Element("Oracle").Value;
						pisSetting.Source.MsSQL = leSetting.Element("MsSQL").Value;
						pisSetting.Source.MySQL = (leSetting.Element("MySQL") != null ? leSetting.Element("MySQL").Value : "");
						pisSetting.Source.Fox = leSetting.Element("Fox").Value.ToString();
						pisSetting.Source.Excel = leSetting.Element("Excel").Value;
                        pisSetting.Source.Excel2007 = leSetting.Element("Excel2007").Value;
						pisSetting.Source.Access = leSetting.Element("Access").Value;;
					}
                    leSetting = lNewDoc.Element("Provider").Element("Dest");
					if(leSetting != null){
						pisSetting.Dest.Oracle = leSetting.Element("Oracle").Value;
						pisSetting.Dest.MsSQL = leSetting.Element("MsSQL").Value;
						pisSetting.Dest.MySQL = (leSetting.Element("MySQL") != null ? leSetting.Element("MySQL").Value : "");
						pisSetting.Dest.Fox = leSetting.Element("Fox").Value.ToString();
						pisSetting.Dest.Excel = leSetting.Element("Excel").Value;
                        pisSetting.Dest.Excel2007 = leSetting.Element("Excel2007").Value;
						pisSetting.Dest.Access = leSetting.Element("Access").Value;;
					}
				}
			} catch(Exception ex) { 
				throw new Exception("[ReadXML] " + ex.Message); 
			}
		}
	#endregion //File XML
	}
    public class Provider{
		/// <summary>
		/// Connect string to Oracle.
		/// </summary>
		public string Oracle = string.Empty;
		/// <summary>
		/// Connect string to MsSQL.
		/// </summary>
		public string MsSQL = string.Empty;
        /// <summary>
		/// Connect string to MySQL.
		/// </summary>
		public string MySQL = string.Empty;
        /// <summary>
        /// Connect string to Fox.
        /// </summary>
		public string Fox = string.Empty;
		/// <summary>
		/// Connect string to Excel.
		/// </summary>
		public string Excel = string.Empty;
        /// <summary>
		/// Connect string to Excel 2007 xlsx.
		/// </summary>
		public string Excel2007 = string.Empty;
		/// <summary>
		/// Connect string to Access
		/// </summary>
		public string Access = string.Empty;
    }
	public class ImpSetting{
		/// <summary>
		/// Connect string to source.
		/// </summary>
		public Provider Source;
		/// <summary>
		/// Connect string to Dest.
		/// </summary>
		public Provider Dest;

        public ImpSetting(){
            Source = new Provider();
            Dest = new Provider();
        }
		public ImpSetting(string psOracle, string psMsSQL, string psFox, string psExcel, string psExcel2007, string psAccess) {
            Source = new Provider();
            Dest = new Provider();
			Init(psOracle, psMsSQL, psFox, psExcel, psExcel2007, psAccess);
		}
		public void Init(string psOracle, string psMsSQL, string psFox, string psExcel, string psExcel2007, string psAccess) {
			Source.Oracle = psOracle;
			Source.MsSQL = psMsSQL;
			Source.Fox = psFox;
			Source.Excel = psExcel;
            Source.Excel2007 = psExcel2007;
			Source.Access = psAccess;

            Dest.Oracle = psOracle;
			Dest.MsSQL = psMsSQL;
			Dest.Fox = psFox;
			Dest.Excel = psExcel;
            Dest.Excel2007 = psExcel2007;
			Dest.Access = psAccess;
		}
	}
}
