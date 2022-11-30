using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using Oracle.DataAccess.Client;
using System.Data.SqlClient;
using System.Data.OleDb;
using System.IO;
using ExcelLibrary.SpreadSheet;
using MySql.Data.MySqlClient;

namespace ImportUtil {
    public partial class frmImport : Form {
        #region Vars
        bool mbCancel = false;
        ImpSetting misSetting;
        #endregion //Vars

        #region Event Control
        public frmImport() {
            InitializeComponent();
        }
        private void frmImport_Load(object sender, EventArgs e) {
            InitControls();
        }
        private void btnImport_Click(object sender, EventArgs e) {
            ImportData();
        }
        private void btnCancel_Click(object sender, EventArgs e) {
            if(MessageBox.Show("Bạn muốn ngừng thực thi !", "Xác nhận", MessageBoxButtons.YesNo) == DialogResult.Yes)
				mbCancel = true;
        }
        private void btnTestCntSrc_Click(object sender, EventArgs e) {
            TestCnt(true);
        }
        private void btnGetTableName_Click(object sender, EventArgs e) {
            GetTablesName();
        }
        private void btnTestCntDest_Click(object sender, EventArgs e) {
            TestCnt(false);
        }

        private void cboProviderSrc_SelectedIndexChanged(object sender, EventArgs e) {
            ProviderChange(true, cboProviderSrc.Text.Trim());
        }

        private void cboProviderDest_SelectedIndexChanged(object sender, EventArgs e) {
            ProviderChange(false, cboProviderDest.Text.Trim());
        }
        private void txtCntSrc_Enter(object sender, EventArgs e) {
            if(cboProviderSrc.Text.Trim() == "Fox")
                toolTip.Show(@"Đăng ký file dll vfpoledb.dll để ASP.NET connect file db foxpro. regsvr32 F:\Soft\VS.Net\vfpoledb.dll", txtCntSrc);
        }

        private void txtCntSrc_Leave(object sender, EventArgs e) {
            if(cboProviderSrc.Text.Trim() == "Fox")
                toolTip.Hide(txtCntSrc);
        }
        private void txtTblSrc_Enter(object sender, EventArgs e) {
            toolTip.Show("Danh sách table name phân cách nhau bằng dấu ,", txtTblSrc);
        }

        private void txtTblSrc_Leave(object sender, EventArgs e) {
            toolTip.Hide(txtTblSrc);
        }
        private void txtTblDest_Enter(object sender, EventArgs e)
        {
            toolTip.Show("Danh sách table name phân cách nhau bằng dấu ,", txtTblDest);
        }

        private void txtTblDest_Leave(object sender, EventArgs e)
        {
            toolTip.Hide(txtTblDest);
        }
        private void btnCutDMY_Click(object sender, EventArgs e) {
            CutDMY();
        }
        #endregion //Event Control

        #region Methods Util
        private void ProviderChange(bool pbSrc, string psProvider) {
            string lsConn = "";

            switch(psProvider){
                case "Oracle":
                    if (pbSrc)
                        lsConn = misSetting.Source.Oracle;
                    else
                        lsConn = misSetting.Dest.Oracle;
                    break;
                case "Ms SQL":
                    if (pbSrc)
                        lsConn = misSetting.Source.MsSQL;
                    else
                        lsConn = misSetting.Dest.MsSQL;
                    break;
                case "My SQL":
                    if (pbSrc)
                        lsConn = misSetting.Source.MySQL;
                    else
                        lsConn = misSetting.Dest.MySQL;
                    if (lsConn == null || lsConn == "")
                        //lsConn = "Server=[myServerAddress];Port=3306;Database=[myDataBase];Uid=[myUsername];Pwd=[myPassword];";
                        //lsConnSsh = "Server=[myServerAddress];Port=3306;Database=[myDataBase];Uid=[myUsername];Pwd=[myPassword];sshHostName=[SshHostName];sshUserName=[SshUserName];sshPassword=[SshPassword];sshPort=[SshPort];";
                        //lsConn = "Server=localhost;Port=3306;Database=mydb;Uid=admin;Pwd=mypwd;sshHostName=127.0.0.1;sshUserName=myuser;sshPassword=pwdweb;sshPort=22;charset=utf8;";
                        lsConn = "server=localhost;database=mydb;uid=root;pwd=;charset=utf8;";
                    //lsConn = "Server=localhost;Port=3306;Database=mydb;Uid=myuser;Pwd=mypwd;sshHostName=127.0.0.1;sshUserName=webuser;sshPassword=pwdweb;sshPort=22;charset=utf8;";
                    break;
                case "Fox":
                    if (pbSrc)
                        lsConn = misSetting.Source.Fox;
                    else
                        lsConn = misSetting.Dest.Fox;
                    break;
                case "Excel":
                    if (pbSrc)
                        lsConn = misSetting.Source.Excel;
                    else
                        lsConn = misSetting.Dest.Excel;
                    break;
                case "Excel 2007":
                    if (pbSrc)
                        lsConn = misSetting.Source.Excel2007;
                    else
                        lsConn = misSetting.Dest.Excel2007;
                    break;
                case "Access":
                    if (pbSrc)
                        lsConn = misSetting.Source.Access;
                    else
                        lsConn = misSetting.Dest.Access;
                    break;
            }
            if(pbSrc)
                txtCntSrc.Text = lsConn;
            else
                txtCntDest.Text = lsConn;
        }
        /// <summary>
        /// Kiểm tra kết nối
        /// </summary>
        private void TestCnt(bool pbSrc) {
            IDbConnection lCxn = null;
            try{
                txtMsg.Text = "";
                lCxn = OpenCnt(pbSrc);
                BaseBusinessObject.CloseConnect(ref lCxn);
                SetMsg((pbSrc ? "Source: " : "Dest: ") + " test connection succeeded.");
                //ghi lại config
                WriteConfig();
            }catch(Exception ex){
                SetMsg("[TestCnt] " + (pbSrc ? "Source: " : "Dest: ") + ex.Message);
            }
        }
        /// <summary>
        /// Mở kết nối
        /// </summary>
        private IDbConnection OpenCnt(bool pbSrc) {
            string lsProvider, lsConn = "";
            IDbConnection lCxn = null;
            try{
                if(pbSrc){
                    lsConn = txtCntSrc.Text.Trim();
                    lsProvider = cboProviderSrc.Text.Trim();
                }else{
                    lsProvider = cboProviderDest.Text.Trim();
                    lsConn = txtCntDest.Text.Trim();
                }
                if (lsConn.Length == 0)
                    throw new Exception("Vui lòng nhập thông tin kết nối!");
                switch(lsProvider){
                    case "Oracle":
                        WebConfig.ConnectionString = lsConn;
                        BaseBusinessObject.OpenData(ref lCxn);
                        break;
                    case "Ms SQL":
                        WebConfig.ConnectionStringSQL = lsConn;
                        BaseBusinessObjectSql.OpenData(ref lCxn);
                        break;
                    case "My SQL":
                        WebConfig.ConnectionStringMySQL = lsConn;
                        BaseBusinessObjectMySql.OpenData(ref lCxn);
                        break;
                    case "Fox": case "Excel": case "Excel 2007": case "Access":
                        WebConfig.ConnectionStringOleDB = lsConn;
                        BaseBusinessObjectOleDB.OpenData(ref lCxn);
                        break;
                }
            }catch(Exception ex){
                throw new Exception("[OpenCnt] " + (pbSrc ? "Source " : "Dest ") + ex.Message);
            }
            return lCxn;
        }
        private void SetMsg(string psMsg){
            //string lsMsg;
			//lsMsg = txtMsg.Text;
            //txtMsg.SelectionStart = lsMsg.Length;
			txtMsg.AppendText(psMsg + "\r\n");
        }
        private void ChangeProgress(int pValue){
            Application.DoEvents();
			lblTuSo.Text = pValue.ToString();
            try{
                prbProcess.Value = pValue;
            }catch{}
			prbProcess.Refresh();
        }
        /// <summary>
        /// Xoá schema trước tên table
        /// </summary>
        private string RemoveDotInTableName(string psListTable) {
            int i, lCount;
            string[] larrTbl;
            string lsRes = "", lsTable;
            try
            {
                larrTbl = psListTable.Split(',');
                lCount = larrTbl.Length;
                for (i = 0; i < lCount; i++) {
                    lsTable = larrTbl[i].Trim();
                    if (lsTable.IndexOf(".") >= 0)
                        lsRes += (lsRes.Length > 0 ? "," : "") + lsTable.Split('.')[1];
                    else
                        lsRes += (lsRes.Length > 0 ? "," : "") + lsTable;
                }
            }
            catch (Exception ex) {
                throw new Exception("[RemoveDotInTableName] Lỗi: " + ex.Message);
            }
            return lsRes;
        }
        /// <summary>
        /// Kiểm tra dữ liệu Import.
        /// </summary>
        private bool IsValidData() {
            string lsProviderS, lsProviderD;
            string lsTableS, lsTableD;
            int lCountS, lCountD;
            bool lbRes = false;
            try{
                txtMsg.Text = "";
                lsProviderS = cboProviderSrc.Text.Trim();
                lsProviderD = cboProviderDest.Text.Trim();
                lsTableS = txtTblSrc.Text.Trim();
                lsTableD = txtTblDest.Text.Trim();
                if (lsTableD.Length == 0){
                    lsTableD = lsTableS;
                    if (lsProviderS == "Oracle" || lsProviderS == "Ms SQL") {
                        if (lsTableD.IndexOf(".") > 0)
                            lsTableD = RemoveDotInTableName(lsTableD);
                    }
                    txtTblDest.Text = lsTableD;
                }
                lCountS = lsTableS.Split(',').Length;
                lCountD = lsTableD.Split(',').Length;
                if (lsProviderS.Length == 0){
                    cboProviderSrc.Focus();
                    throw new Exception("Vui lòng nhập thông tin kết nối Source!");
                }else if (lsProviderD.Length == 0){
                    cboProviderDest.Focus();
                    throw new Exception("Vui lòng nhập thông tin kết nối Dest!");
                }else if (lsTableS.Length == 0){
                    txtTblSrc.Focus();
                    throw new Exception("Vui lòng nhập tên Table Source!");
                }else if(lsTableD.IndexOf("_") == 0){
                    txtTblDest.Focus();
                    throw new Exception("Vui lòng kiểm tra tên Table Dest, không được có ký tự đặc biệt!");
                }else if(lCountS != lCountD){
                    txtTblDest.Focus();
                    throw new Exception("Số lượng table Source " + lCountS + " <> table Dest " + lCountD);
                }else
                    lbRes = true;
            }catch(Exception ex){
                SetMsg(ex.Message);
            }
            return lbRes;
        }
        /// <summary>
        /// Lấy tên sheet của Excel và tên table của Foxpro.
        /// </summary>
        private void GetTablesName() {
            string lsConnect;
            string lsProvider;
            try{
                txtMsg.Text = "";
                lsProvider = cboProviderSrc.Text.Trim();
                if(lsProvider != "Fox" && lsProvider != "Excel" && lsProvider != "Excel 2007"){
                    SetMsg("Thao tác này chỉ dành cho Dữ liệu Foxpro, Excel!");
                    return;
                }
                lsConnect = txtCntSrc.Text.Trim();
                if (lsConnect.Length == 0){
                    SetMsg("Vui lòng nhập kết nối Source!");
                    return;
                }
                if (lsProvider == "Fox")
                    ListAllFile();
                else //excel
                    ListAllSheetName();
            }catch(Exception ex){
                SetMsg(ex.Message);
            }
        }
        /// <summary>
        /// Lấy danh sách table name foxpro.
        /// </summary>
        private void ListAllFile(){
            string[] filePaths;
            string lsPath, lsConnect, lsRes = "", lsFileName;
            int lPosS, i, lCount;
            try{
                lsConnect = txtCntSrc.Text.Trim();
                lsPath = GetPathFile(lsConnect);
                filePaths = Directory.GetFiles(lsPath, "*.dbf");
                lCount = filePaths.Length;
                for(i = 0; i < lCount; i++){
                    lsFileName = filePaths[i];
                    lsFileName = lsFileName.Replace(lsPath, "");
                    lsFileName = lsFileName.Replace(@"\", "");
                    lsFileName = lsFileName.Replace(".dbf", "");
                    lPosS = lsFileName.IndexOf(".");
                    if(lPosS > 0)
                        lsFileName = lsFileName.Substring(0,lPosS);
                    lsRes += (lsRes.Length > 0 ? "," : "") + lsFileName;
                    
                }
                txtTblSrc.Text = lsRes;
                if (lsRes.Length == 0)
                    MessageBox.Show("Không tìm thấy file foxpro dbf trong thư mục " + lsPath + ".");
                else if (txtTblDest.Text.Trim().Length == 0)
                    txtTblDest.Text = lsRes;
                
            }catch(Exception ex){
                SetMsg("[ListAllFile] " + ex.Message);
            }
        }
        /// <summary>
        /// Lấy danh sách tên sheet của file Excel.
        /// </summary>
        private void ListAllSheetName(){
            string lsPath, lsConnect, lsRes = "", lsFileName;
            int i, lCount;
            DataTable ldtSheets;
            IDbConnection lCxn = null;
            try{
                lsConnect = txtCntSrc.Text.Trim();
                lsPath = GetPathFile(lsConnect);
                
                lCxn = OpenCnt(true);
                ldtSheets = ((OleDbConnection)lCxn).GetOleDbSchemaTable(OleDbSchemaGuid.Tables, null);
                BaseBusinessObject.CloseConnect(ref lCxn);
                lCount = ldtSheets.Rows.Count;
                for(i = 0; i < lCount; i++){
                    lsFileName = ClsUtil.parseString(ldtSheets.Rows[i],"table_name").Replace("'","").Replace("$","");
                    lsRes += (lsRes.Length > 0 ? "," : "") + lsFileName;
                }
                txtTblSrc.Text = lsRes;
                if (lsRes.Length == 0)
                    MessageBox.Show("Không tìm thấy file Excel trong thư mục " + lsPath + ".");
                else if (txtTblDest.Text.Trim().Length == 0)
                    txtTblDest.Text = lsRes;
                if (ldtSheets != null){
                    ldtSheets.Dispose();
                    ldtSheets = null;
                }
            }catch(Exception ex){
                SetMsg("[ListAllSheetName] " + ex.Message);
            }
        }
        private string GetPathFile(string psConnect){
            string lsFind, lsPath = "";
            int lPosS, lLen, lPosE;
            try{
                lsFind = "Data Source=";
                lLen = lsFind.Length;
                lPosS = psConnect.IndexOf(lsFind);
                lPosE = psConnect.IndexOf(";", lPosS);
                if(lPosE > 0)
                    lsPath = psConnect.Substring(lPosS + lLen,lPosE - lPosS - lLen);
                else
                    lsPath = psConnect.Substring(lPosS + lLen);
            }catch(Exception ex){
                SetMsg("[GetPathFile] " + ex.Message);
            }
            return lsPath;
        }
        /// <summary>
        /// Lấy path của file Excel.
        /// </summary>
        private string GetFileExcelName(out string psFileName){
            string lsConnect, lsPath = "";
            int lPos, lLen;
            psFileName = "";
            try{
                lsConnect = txtCntDest.Text.Trim();
                lsPath = GetPathFile(lsConnect);
                if (lsPath.Length == 0)
                    SetMsg("Không tìm thấy thông tin đường dẫn file Excel trong chuỗi kết nối.");
                else if (lsPath.ToLower().EndsWith(".xls") || lsPath.ToLower().EndsWith(".xlsx")){
                    psFileName = lsPath;
                    //Xoá tên file trong Path:
                    lLen = lsPath.Length;
                    lPos = lsPath.LastIndexOf(@"\");
                    if(lPos > 0)
                        lsPath = lsPath.Substring(0, lPos);
                }
            }catch(Exception ex){
                SetMsg("[GetFileExcelName] " + ex.Message);
            }
            return lsPath;
        }
        /// <summary>
        /// Xoá chuỗi ngày tháng năm trong tên table
        /// </summary>
        private void CutDMY() {
            int i, lCount, lPos, j, lLen;
            string lsFileName, lsRes = "";
            string[] lsChars = {"0", "1", "2", "3", "4", "5", "6", "7", "8", "9" };
            string[] lListFile;
            try{
                lLen = lsChars.Length;
                lListFile = txtTblDest.Text.Trim().Split(',');
                lCount = lListFile.Length;
                for(i = 0; i < lCount; i++){
                    lPos = -1;
                    lsFileName = lListFile[i];
                    lPos = lsFileName.LastIndexOf("_");
                    if(lPos > 0)
                        lsFileName = lsFileName.Substring(0,lPos);
                    else{
                        for(j = 0; j < lLen; j++){
                            if (lsFileName.IndexOf(lsChars[j]) > 0)
                                lsFileName = lsFileName.Replace(lsChars[j], "");
                        }
                    }
                    lsRes += (lsRes.Length > 0 ? "," : "") + lsFileName;
                    
                }
                txtTblDest.Text = lsRes;
            }catch(Exception ex){
                SetMsg("[CutDMY] Lỗi: " + ex.Message);
            }
        }
        /// <summary>
        /// Kiểm tra kết nối
        /// </summary>
        private void InitControls() {
            string lsPath, lsFile;
            string lsCN;
            bool lbLocal = false, lbExist;
            try{
                txtMsg.Text = "";
                lsCN = System.Environment.MachineName;
                lbLocal = (lsCN == "KV2-PM3-1071");
                misSetting = new ImpSetting();
                lsPath = Application.StartupPath;
                lsPath = lsPath.Replace(@"bin\Debug", "");
                if(!lsPath.EndsWith(@"\"))
                    lsPath += @"\";
                lsFile = lsPath + "Config_Imp.xml";
                lbExist = ClsFile.IsExistsFile(lsFile);
                if(lbExist)
                    ClsFile.ReadXML(ref misSetting, lsFile);
                else if(lbLocal){
                    //for 2007 Excel file xlsx : Microsoft.ACE.OLEDB.12.0;Excel 12.0";
                    misSetting.Source.Oracle = "User ID=qlkh;Data Source=SAN1;Password=QL@adm7235";
                    //lsConn = "User ID=qlkh;Data Source=Qlkh120;Password=qlkh";
                    misSetting.Source.MsSQL = @"server=10.70.39.163\sqlexpress;uid=sa;pwd=sa2012;database=xdsl";
                    misSetting.Source.MySQL = "Server=192.168.4.200;Port=3306;Database=mydb;Uid=eocuser;Pwd=Eoc#bPc.VN271;sshHostName=113.161.183.244;sshUserName=webuser;sshPassword=9Z!fcHHyf$;sshPort=22;charset=utf8;Convert Zero Datetime=True;";
                    misSetting.Source.Fox = @"Data Source=F:\Temp\Foxpro\;Provider=VFPOLEDB.1;Mode=Share Deny None;Password=False";
                    misSetting.Source.Excel = @"Provider=Microsoft.Jet.OLEDB.4.0;Data Source=F:\Temp\Excel\Test.xls;Extended Properties=Excel 8.0";
                    misSetting.Source.Excel2007 = @"Provider=Microsoft.ACE.OLEDB.12.0;Data Source=F:\Temp\Excel\Test.xlsx;Extended Properties=Excel 12.0";
                    misSetting.Source.Access = @"Provider=Microsoft.Jet.OLEDB.4.0;Data Source=F:\Temp\Access\Test.mdb;Persist Security Info=False";

                    misSetting.Dest.Oracle = "User ID=qlkh;Data Source=SAN1;Password=QL@adm7235";
                    misSetting.Dest.MsSQL = @"server=10.70.39.163\sqlexpress;uid=sa;pwd=sa2012;database=xdsl";
                    misSetting.Dest.MySQL = "server=localhost;database=dbnhtk;uid=root;pwd=;charset=utf8;Convert Zero Datetime=True;";
                    misSetting.Dest.Fox = @"Data Source=F:\Temp\Foxpro\;Provider=VFPOLEDB.1;Mode=Share Deny None;Password=False";
                    misSetting.Dest.Excel = @"Provider=Microsoft.Jet.OLEDB.4.0;Data Source=F:\Temp\Excel\Test.xls;Extended Properties=Excel 8.0";
                    misSetting.Dest.Excel2007 = @"Provider=Microsoft.ACE.OLEDB.12.0;Data Source=F:\Temp\Excel\Test.xlsx;Extended Properties=Excel 12.0";
                    misSetting.Dest.Access = @"Provider=Microsoft.Jet.OLEDB.4.0;Data Source=F:\Temp\Access\Test.mdb;Persist Security Info=False";
                }else{
                    //misSetting.Source.Oracle = "User ID=[Username];Data Source=[TnsName];Password=[Password];Pooling=false;";
                    misSetting.Source.Oracle = "Data Source=(DESCRIPTION=(ADDRESS_LIST=(ADDRESS=(PROTOCOL=TCP)(HOST=[IP_Server])(PORT=1521)))(CONNECT_DATA=(SERVICE_NAME=[ServiceName];)));User ID=[Username];Password=[Password];";
                    misSetting.Source.MsSQL = "server=[IP_Server];uid=[Username];pwd=[Password];database=[DBName]";
                    misSetting.Source.MySQL = "Server=[IP_DBServer];Port=3306;Database=[DBName];Uid=[UserDb];Pwd=[PasswordDb];sshHostName=[IP_WebServer];sshUserName=[UserWeb];sshPassword=[PasswordWeb];sshPort=22;charset=utf8;Convert Zero Datetime=True;";
                    misSetting.Source.Fox = "Data Source=[Path_Dir];Provider=VFPOLEDB.1;Mode=Share Deny None;Password=False";
                    misSetting.Source.Excel = "Provider=Microsoft.Jet.OLEDB.4.0;Data Source=[Path_File];Extended Properties=Excel 8.0";
                    misSetting.Source.Excel2007 = "Provider=Microsoft.ACE.OLEDB.12.0;Data Source=[Path_File];Extended Properties=Excel 12.0";
                    misSetting.Source.Access = "Provider=Microsoft.Jet.OLEDB.4.0;Data Source=[Path_File];Persist Security Info=False";

                    //misSetting.Dest.Oracle = "User ID=[Username];Data Source=[TnsName];Password=[Password];Pooling=false;";
                    misSetting.Source.Oracle = "Data Source=(DESCRIPTION=(ADDRESS_LIST=(ADDRESS=(PROTOCOL=TCP)(HOST=[IP_Server])(PORT=1521)))(CONNECT_DATA=(SERVICE_NAME=[ServiceName];)));User ID=[Username];Password=[Password];";
                    misSetting.Dest.MsSQL = "server=[IP_Server];uid=[Username];pwd=[Password];database=[DBName]";
                    misSetting.Dest.MySQL = "server=[IP_Server];database=[DBName];uid=[Username];pwd=[Password];charset=utf8;Convert Zero Datetime=True;";
                    misSetting.Dest.Fox = "Data Source=[Path_Dir];Provider=VFPOLEDB.1;Mode=Share Deny None;Password=False";
                    misSetting.Dest.Excel = "Provider=Microsoft.Jet.OLEDB.4.0;Data Source=[Path_File];Extended Properties=Excel 8.0";
                    misSetting.Dest.Excel2007 = "Provider=Microsoft.ACE.OLEDB.12.0;Data Source=[Path_File];Extended Properties=Excel 12.0";
                    misSetting.Dest.Access = "Provider=Microsoft.Jet.OLEDB.4.0;Data Source=[Path_File];Persist Security Info=False";
                }
  
            }catch(Exception ex){
                SetMsg("[InitControls] " + ex.Message);
            }
        }
        /// <summary>
        /// Kiểm tra kết nối
        /// </summary>
        private void WriteConfig() {
            string lsPath, lsFile, lsProviderS, lsProviderD, lsConS, lsConD;
            try{
                lsProviderS = cboProviderSrc.Text.Trim();
                lsProviderD = cboProviderDest.Text.Trim();
                lsConS = txtCntSrc.Text.Trim();
                lsConD = txtCntDest.Text.Trim();
                if(lsProviderS.Length > 0 && lsConS.Length > 0){
                    switch(lsProviderS){
                        case "Oracle":
                            misSetting.Source.Oracle = lsConS;
                            break;
                        case "Ms SQL":
                            misSetting.Source.MsSQL = lsConS;
                            break;
                        case "My SQL":
                            misSetting.Source.MySQL = lsConS;
                            break;
                        case "Fox":
                            misSetting.Source.Fox = lsConS;
                            break;
                        case "Excel":
                            misSetting.Source.Excel = lsConS;
                            break;
                        case "Excel 2007":
                            misSetting.Source.Excel2007 = lsConS;
                            break;
                        case "Access":
                            misSetting.Source.Access = lsConS;
                            break;
                    }//of switch
                }
                if(lsProviderD.Length > 0 && lsConS.Length > 0){
                    switch(lsProviderD){
                        case "Oracle":
                            misSetting.Dest.Oracle = lsConD;
                            break;
                        case "Ms SQL":
                            misSetting.Dest.MsSQL = lsConD;
                            break;
                        case "My SQL":
                            misSetting.Dest.MySQL = lsConD;
                            break;
                        case "Fox":
                            misSetting.Dest.Fox = lsConD;
                            break;
                        case "Excel":
                            misSetting.Dest.Excel = lsConD;
                            break;
                        case "Excel 2007":
                            misSetting.Dest.Excel2007 = lsConD;
                            break;
                        case "Access":
                            misSetting.Dest.Access = lsConD;
                            break;
                    }//of switch
                }
                lsPath = Application.StartupPath;
                lsPath = lsPath.Replace(@"bin\Debug","");
                if(!lsPath.EndsWith(@"\"))
                    lsPath += @"\";
                lsFile = lsPath + "Config_Imp.xml";
                ClsFile.WriteXML(misSetting, lsFile);
            }catch(Exception ex){
                SetMsg("[WriteConfig] " + ex.Message);
            }
        }
        #endregion //Methods Util

        #region Methods Import
        /// <summary>
        /// Import dữ liệu
        /// </summary>
        private void ImportData() {
            string lsProviderS = "", lsProviderD = "";
            string lsListTableS = "", lsListTableD = "";
            string[] larrTableS, larrTableD;
            string lsTableS = "", lsTableD = "";
            int i, lCountS, lSuccess, lRows;
            IDbConnection lCxnS = null, lCxnD = null;
            DataTable ldtSource = null, ldtStructDest = null;
            try{
                txtMsg.Text = "";
                if (!IsValidData())
                    return;
                mbCancel = false;
				btnImport.Enabled = false;
				prbProcess.Visible = true;
				ChangeProgress(0);
                lsProviderS = cboProviderSrc.Text.Trim();
                lsProviderD = cboProviderDest.Text.Trim();
                lsListTableS = txtTblSrc.Text.Trim();
                lsListTableD = txtTblDest.Text.Trim();
                SetMsg("*** Begin Open Database.");
                lCxnS = OpenCnt(true);
                ChangeProgress(100);
				ChangeProgress(0);
                if(lsProviderD.IndexOf("Excel") < 0)//không được mở connection Dest nếu là File excel => lỗi khi tạo file excel
                    lCxnD = OpenCnt(false);
                SetMsg("Finish Open Database.");
                ChangeProgress(100);
                //ghi lại config
                WriteConfig();
				ChangeProgress(0);
                larrTableS = lsListTableS.Split(',');
                larrTableD = lsListTableD.Split(',');
                lCountS = larrTableS.Length;
                for(i = 0; i < lCountS; i++){
                    lSuccess = 0; lRows = 0;
                    ChangeProgress(0);
                    prbProcess.Maximum = 100;
                    lsTableS = larrTableS[i].Trim();
                    lsTableD = larrTableD[i].Trim();
                    ldtSource = LoadDataImport(true, lsProviderS, lsTableS, lCxnS);
                    ldtSource.TableName = lsTableS;
                    ChangeProgress(100);
                    lRows = ldtSource.Rows.Count;
                    if(lRows == 0){
                        SetMsg("Không có dữ liệu " + lsTableS + " để Import. Kiểm tra tạo table nếu chưa có.");
                        //continue;
                    }else
                        SetMsg("*** Begin import to " + lsTableD + ": " + lRows + " records.");
                    ChangeProgress(0);
                    if(lRows > 0)
                        prbProcess.Maximum = lRows;
                    //kiểm tra table dest đã tồn tại, trả về cấu trúc nếu có, tạo table nếu chưa có
                    ldtStructDest = CheckTableDest(ldtSource, lsProviderD, lsTableD, lCxnD);
                    lSuccess = ImportToDest(ldtSource, lsProviderD, lsTableD, ldtStructDest, lCxnD, i + 1);
                    SetMsg("Import " + lsTableS + " -> " + lsTableD + " success " + lSuccess + " records.");
					if(mbCancel)
						break;
                }//of for
                SetMsg("*** Finish Import.");
                MessageBox.Show("Import succeeded.");
            }catch(Exception ex){
                SetMsg("Lỗi [ImportData] " + lsTableS + ": " + ex.Message);
            }finally{
                BaseBusinessObject.CloseConnect(ref lCxnS);
                BaseBusinessObject.CloseConnect(ref lCxnD);
                btnImport.Enabled = true;
            }
        }
        /// <summary>
        /// Lấy dữ liệu nguồn và kiểm tra table đích tạo table nếu chưa có.
        /// </summary>
        private DataTable LoadDataImport(bool pbSrc, string psProvider, string psTable, IDbConnection Cxn) {
            string lsSql = "", vsValue;
            int vLimit = -1;
            DataTable ldtData = null;
            try{
                if (psTable.Length == 0)
                    throw new Exception("Table " + (pbSrc ? "Source" : "Dest") + " Empty!");
                if(psProvider.IndexOf("Excel") >= 0)
                    lsSql = "SELECT * FROM [" + psTable + "$]";
                else
                    lsSql = "SELECT * FROM " + psTable;
                if (pbSrc)
                {
                    vsValue = txtLimit.Text.Trim();
                    if(ClsUtil.IsNumeric(vsValue))
                        vLimit = Convert.ToInt32(vsValue);
                }
                else
                    lsSql += " WHERE 0 = 1";
                switch(psProvider){
                    case "Oracle":
                        if (pbSrc && vLimit >= 0)
                            lsSql += " WHERE rownum <= " + vLimit;
                        ldtData = DataOra.GetData(lsSql, Cxn);
                        break;
                    case "Ms SQL":
                        if (pbSrc && vLimit > 0)
                            lsSql = lsSql.Replace("SELECT", "SELECT TOP " + vLimit);
                        ldtData = DataSql.GetData(lsSql, Cxn);
                        break;
                    case "My SQL":
                        if(pbSrc && vLimit >= 0)
                            lsSql += " LIMIT 0, " + vLimit;
                        ldtData = DataMySql.GetData(lsSql, Cxn);
                        break;
                    case "Fox": case "Excel": case "Excel 2007": case "Access":
                        if (pbSrc && vLimit >= 0 && psProvider == "Access")
                            lsSql = lsSql.Replace("SELECT", "SELECT TOP " + vLimit);
                        ldtData = DataOleDB.GetData(lsSql, Cxn);
                        break;
                }
            }catch(Exception ex){
                throw new Exception ("[LoadDataImport] " + ex.Message);
            }
            return ldtData;
        }
        /// <summary>
        /// Kiểm tra table đích, tạo table nếu chưa có, trả về cấu trúc table đích. Excel không cần kiểm tra.
        /// </summary>
        private DataTable CheckTableDest(DataTable pdtSource, string psProvider, string psTable, IDbConnection Cxn)
        {
            string lsSql = "";
            string lsSchema = "";
            DataTable ldtDest = null;
            try{
                lsSchema = txtSchemaDest.Text.Trim();
                if (psTable.Length == 0)
                    psTable = txtTblSrc.Text.Trim();
                if (psTable.Length == 0)
                    throw new Exception("Table Dest Empty!");
                lsSql = "SELECT * FROM " + (lsSchema.Length > 0 ? lsSchema + "." : "") + psTable + " WHERE 0 = 1";
                switch(psProvider){//Excel không cần kiểm tra
                    case "Oracle":
                        try{
                            ldtDest = DataOra.GetData(lsSql, Cxn);
                        }catch{
                            DataOra.MakeTable(lsSchema, psTable, pdtSource, Cxn);
                        }
                        break;
                    case "Ms SQL":
                        try{
                            ldtDest = DataSql.GetData(lsSql, Cxn);
                        }catch{
                            DataSql.MakeTable(lsSchema, psTable, pdtSource, Cxn);
                        }
                        break;
                    case "My SQL":
                        try
                        {
                            ldtDest = DataMySql.GetData(lsSql, Cxn);
                        }
                        catch
                        {
                            DataMySql.MakeTable(lsSchema, psTable, pdtSource, Cxn);
                        }
                        break;
                    case "Fox": case "Access":
                        try{
                            ldtDest = DataOleDB.GetData(lsSql, Cxn);
                        }catch{
                            DataOleDB.MakeTable(psProvider, psTable, pdtSource, Cxn);
                        }
                        break;
                }
            }catch(Exception ex){
                throw new Exception ("[CheckTableDest] " + ex.Message);
            }
            return ldtDest;
        }
        /// <summary>
        /// Import dữ liệu từ source -> dest. pCountTbl: số thứ tự table được import, bắt đầu từ 1
        /// </summary>
        private int ImportToDest(DataTable pdtSource, string psProvider, string psTableD, DataTable pdtStructDest, IDbConnection Cxn, int pCountTbl) {
            int lSuccess = 0;
            try{
               switch(psProvider){
                    case "Oracle":
                       lSuccess = ImportToOracle(pdtSource, psTableD, pdtStructDest, Cxn);
                        break;
                    case "Ms SQL":
                        lSuccess = ImportToMsSql(pdtSource, psTableD, pdtStructDest, Cxn);
                        break;
                    case "My SQL":
                        lSuccess = ImportToMySql(pdtSource, psTableD, pdtStructDest, Cxn);
                        break;
                    case "Fox": case "Access":
                        lSuccess = ImportToOleDb(psProvider, pdtSource, psTableD, pdtStructDest, Cxn);
                        break;
                   case "Excel":
                        lSuccess = ImportToExcelBySheet(pdtSource, psTableD, pCountTbl);
                        break;
                   case "Excel 2007":
                       throw new Exception ("Chương trình hiện chưa hỗ trợ xuất ra dữ liệu " + psProvider + ".");
                   default:
                       throw new Exception ("Chương trình hiện chưa hỗ trợ xuất ra dữ liệu " + psProvider + ".");
                }
            }catch(Exception ex){
                throw new Exception ("[ImportToDest] " + ex.Message);
            }
            return lSuccess;
        }
        /// <summary>
        /// Import dữ liệu từ source -> Oracle.
        /// </summary>
        private int ImportToOracle(DataTable pdtSource, string psTableD, DataTable pdtStructDest, IDbConnection Cxn) {
            string lsSchema = "", lsListFieldDest;
            int i = 0, lResCount = 0, lRows, lSuccess = 0;
            OracleCommand lcmdData = null;
            try{
                lsSchema = txtSchemaDest.Text.Trim();
                lRows = pdtSource.Rows.Count;
                lsListFieldDest = ClsUtil.ListColName(pdtStructDest);
                lcmdData = DataOra.MakeCommandAdd(lsSchema, psTableD, pdtSource, lsListFieldDest, Cxn);
                for (i = 0; i < lRows; i++){
                    lResCount = DataOra.AddNewRow(pdtSource.Rows[i], lcmdData, lsListFieldDest);
                    lSuccess += lResCount;
                    ChangeProgress(i + 1);
					if(mbCancel)
						break;
                }
            }catch(Exception ex){
                throw new Exception ("[ImportToOracle] Row Progress: " + (i + 1) + ", " + ex.Message);
            }finally{
                if(lcmdData != null){
                    lcmdData.Dispose();
                    lcmdData = null;
                }
            }
            return lSuccess;
        }
        /// <summary>
        /// Import dữ liệu từ source -> MsSql.
        /// </summary>
        private int ImportToMsSql(DataTable pdtSource, string psTableD, DataTable pdtStructDest, IDbConnection Cxn) {
            string lsSchema = "", lsListFieldDest;
            int i = 0, lResCount = 0, lRows, lSuccess = 0;
            SqlCommand lcmdData = null;
            try{
                lsSchema = txtSchemaDest.Text.Trim();
                lRows = pdtSource.Rows.Count;
                lsListFieldDest = ClsUtil.ListColName(pdtStructDest);
                lcmdData = DataSql.MakeCommandAdd(lsSchema, psTableD, pdtSource, lsListFieldDest, Cxn);
                for (i = 0; i < lRows; i++){
                    lResCount = DataSql.AddNewRow(pdtSource.Rows[i], lcmdData, lsListFieldDest);
                    lSuccess += lResCount;
                    ChangeProgress(i + 1);
					if(mbCancel)
						break;
                }
            }catch(Exception ex){
                throw new Exception ("[ImportToMsSql] Row Progress: " + (i + 1) + ", " + ex.Message);
            }finally{
                if(lcmdData != null){
                    lcmdData.Dispose();
                    lcmdData = null;
                }
            }
            return lSuccess;
        }
        /// <summary>
        /// Import dữ liệu từ source -> MsSql.
        /// </summary>
        private int ImportToMySql(DataTable pdtSource, string psTableD, DataTable pdtStructDest, IDbConnection Cxn)
        {
            string lsSchema = "", lsListFieldDest;
            int i = 0, lResCount = 0, lRows, lSuccess = 0;
            MySqlCommand lcmdData = null;
            try
            {
                lsSchema = txtSchemaDest.Text.Trim();
                lRows = pdtSource.Rows.Count;
                lsListFieldDest = ClsUtil.ListColName(pdtStructDest);
                lcmdData = DataMySql.MakeCommandAdd(lsSchema, psTableD, pdtSource, lsListFieldDest, Cxn);
                for (i = 0; i < lRows; i++)
                {
                    lResCount = DataMySql.AddNewRow(pdtSource.Rows[i], lcmdData, lsListFieldDest);
                    lSuccess += lResCount;
                    ChangeProgress(i + 1);
                    if (mbCancel)
                        break;
                }
            }
            catch (Exception ex)
            {
                throw new Exception("[ImportToMySql] Row Progress: " + (i + 1) + ", " + ex.Message);
            }
            finally
            {
                if (lcmdData != null)
                {
                    lcmdData.Dispose();
                    lcmdData = null;
                }
            }
            return lSuccess;
        }
        /// <summary>
        /// Import dữ liệu từ source -> Fox, Access.
        /// </summary>
        private int ImportToOleDb(string psProvider, DataTable pdtSource, string psTableD, DataTable pdtStructDest, IDbConnection Cxn)
        {
            string lsListFieldDest;
            int i = 0, lResCount = 0, lRows, lSuccess = 0;
            OleDbCommand lcmdData = null;
            try{
                lRows = pdtSource.Rows.Count;
                lsListFieldDest = ClsUtil.ListColName(pdtStructDest);
                lcmdData = DataOleDB.MakeCommandAdd(psProvider, psTableD, pdtSource, lsListFieldDest, Cxn);
                for (i = 0; i < lRows; i++){
                    lResCount = DataOleDB.AddNewRow(pdtSource.Rows[i], lcmdData, lsListFieldDest);
                    lSuccess += lResCount;
                    ChangeProgress(i + 1);
					if(mbCancel)
						break;
                }
            }catch(Exception ex){
                throw new Exception ("[ImportToOleDb] Row Progress: " + (i + 1) + ", " + ex.Message);
            }finally{
                if(lcmdData != null){
                    lcmdData.Dispose();
                    lcmdData = null;
                }
            }
            return lSuccess;
        }
        /// <summary>
        /// Import dữ liệu từ source -> Excel. Lỗi Invalid Cell value, không thể dùng
        /// </summary>
        public int ImportToExcel(DataTable pdtSource, string psTableD, int pCountTbl)
        {
            int lSuccess = 0;
            string lsFileName, lsPath;
            DataSet ldsData;
            try
            {
                lsPath = GetFileExcelName(out lsFileName);
                if (lsPath == "")
                    return lSuccess;
                else if (lsFileName == "" || pCountTbl > 1)
                    lsFileName = lsPath + @"\" + psTableD + ".xls";
                //Here's the easy part. Create the Excel worksheet from the data set
                ldsData = new DataSet();
                pdtSource.TableName = psTableD;
                //Set the locale for each
                ldsData.Locale = System.Threading.Thread.CurrentThread.CurrentCulture;
                pdtSource.Locale = System.Threading.Thread.CurrentThread.CurrentCulture;
                ldsData.Tables.Add(pdtSource);
                ExcelLibrary.DataSetHelper.CreateWorkbook(lsFileName, ldsData);
                lSuccess = pdtSource.Rows.Count;
                ldsData.Dispose();
                ldsData = null;
            }catch (Exception ex) {
                throw new Exception ("[ImportToExcel] " + psTableD + ", " + ex.Message);
            }
            return lSuccess;
        }
        /// <summary>
        /// Import dữ liệu từ source -> Excel. Tạo chi tiết sheet dữ liệu.
        /// </summary>
        public int ImportToExcelBySheet(DataTable pdtSource, string psTableD, int pCountTbl)
        {
            int lSuccess = 0, i, j, lCols, lRows;
            string lsPath, lsFileName, lsTypeSrc, lsFieldName;
            //Create a workbook instance
            Workbook workbook;
            Worksheet worksheet;
            int iSheetCount = 0;
            Double  dTemp = 0;
            DateTime? dtTemp;
            bool lbOver = false;
            try
            {
                lsPath = GetFileExcelName(out lsFileName);
                if (lsPath == "")
                    return lSuccess;
                else if (lsFileName == "" || pCountTbl > 1)
                    lsFileName = lsPath + @"\" + psTableD + ".xls";
                lCols = pdtSource.Columns.Count;
                lRows = pdtSource.Rows.Count;
                //Here//s the easy part. Create the Excel worksheet from the data set
                pdtSource.TableName = txtTblDest.Text.Trim();
                workbook = new Workbook();
                //Create a worksheet instance
                iSheetCount = iSheetCount + 1;
                worksheet = new Worksheet(psTableD);
 
                //Write Table Header
                for(i = 0; i < lCols; i++){
                    worksheet.Cells[0, i] = new Cell(pdtSource.Columns[i].ColumnName);
                }
 
            //Write Table Body
                if (lRows > 65000)
                {
                    lRows = 65000;
                    lbOver = true;
                }
                for(i = 0; i < lRows; i++){
                    for(j = 0; j < lCols; j++){
                        lsTypeSrc = pdtSource.Columns[j].DataType.Name;
                        lsFieldName = pdtSource.Columns[j].ColumnName;
                        switch(lsTypeSrc){
                            case "DateTime":
                                dtTemp = ClsUtil.parseDateTime(pdtSource.Rows[i], lsFieldName);
                                worksheet.Cells[i + 1, j] = new Cell(dtTemp, "dd/MM/yyyy");
                                break;
                            case "Double":
                                dTemp = ClsUtil.parseDouble(pdtSource.Rows[i], lsFieldName);
                                worksheet.Cells[i + 1, j] = new Cell(dTemp, "#,##0.00");
                                break;
                            default:
                                worksheet.Cells[i + 1, j] = new Cell(ClsUtil.parseString(pdtSource.Rows[i], lsFieldName));
                                break;
                        }
                    }
                    lSuccess++;
                    ChangeProgress(i + 1);
				    if(mbCancel)
					    break;
                }
                //Attach worksheet to workbook
                workbook.Worksheets.Add(worksheet);
                //Bug on Excel Library, min file size must be 7 Kb
                //thus we need to add empty row for safety
                /*if(lRows < 100){
                    worksheet = new Worksheet("Sheet 2");
                    i = 1;
                    while (i < 100){
                        worksheet.Cells[i, 0] = new Cell(" ");
                        i++;
                    }
                    workbook.Worksheets.Add(worksheet);
                }*/
                workbook.Save(lsFileName);
                if (lbOver)
                    SetMsg(psTableD + ": chỉ có thể tạo file Excel 65000 rows, vui lòng kiểm tra lại.");
            }catch (Exception ex) {
                throw new Exception ("[ImportToExcelBySheet] " + ex.Message);
            }
            return lSuccess;
        }
        #endregion //Methods Import
    }
}
