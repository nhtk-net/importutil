namespace ImportUtil {
    partial class frmImport {
        /// <summary>
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing) {
            if (disposing && (components != null)) {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows Form Designer generated code

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent() {
            this.components = new System.ComponentModel.Container();
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(frmImport));
            this.grSrc = new System.Windows.Forms.GroupBox();
            this.btnGetTableName = new System.Windows.Forms.Button();
            this.btnTestCntSrc = new System.Windows.Forms.Button();
            this.lblTblSrc = new System.Windows.Forms.Label();
            this.txtTblSrc = new System.Windows.Forms.TextBox();
            this.lblCntSrc = new System.Windows.Forms.Label();
            this.txtCntSrc = new System.Windows.Forms.TextBox();
            this.cboProviderSrc = new System.Windows.Forms.ComboBox();
            this.lblProviderSrc = new System.Windows.Forms.Label();
            this.grDest = new System.Windows.Forms.GroupBox();
            this.btnCutDMY = new System.Windows.Forms.Button();
            this.txtSchemaDest = new System.Windows.Forms.TextBox();
            this.lblSchemaDest = new System.Windows.Forms.Label();
            this.btnTestCntDest = new System.Windows.Forms.Button();
            this.txtTblDest = new System.Windows.Forms.TextBox();
            this.lblTblDest = new System.Windows.Forms.Label();
            this.lblCntDest = new System.Windows.Forms.Label();
            this.txtCntDest = new System.Windows.Forms.TextBox();
            this.cboProviderDest = new System.Windows.Forms.ComboBox();
            this.lblCntStringDest = new System.Windows.Forms.Label();
            this.btnImport = new System.Windows.Forms.Button();
            this.btnCancel = new System.Windows.Forms.Button();
            this.txtMsg = new System.Windows.Forms.TextBox();
            this.toolTip = new System.Windows.Forms.ToolTip(this.components);
            this.prbProcess = new System.Windows.Forms.ProgressBar();
            this.lblTuSo = new System.Windows.Forms.Label();
            this.txtLimit = new System.Windows.Forms.TextBox();
            this.label1 = new System.Windows.Forms.Label();
            this.grSrc.SuspendLayout();
            this.grDest.SuspendLayout();
            this.SuspendLayout();
            // 
            // grSrc
            // 
            this.grSrc.Controls.Add(this.txtLimit);
            this.grSrc.Controls.Add(this.label1);
            this.grSrc.Controls.Add(this.btnGetTableName);
            this.grSrc.Controls.Add(this.btnTestCntSrc);
            this.grSrc.Controls.Add(this.lblTblSrc);
            this.grSrc.Controls.Add(this.txtTblSrc);
            this.grSrc.Controls.Add(this.lblCntSrc);
            this.grSrc.Controls.Add(this.txtCntSrc);
            this.grSrc.Controls.Add(this.cboProviderSrc);
            this.grSrc.Controls.Add(this.lblProviderSrc);
            this.grSrc.ForeColor = System.Drawing.SystemColors.ControlText;
            this.grSrc.Location = new System.Drawing.Point(11, 8);
            this.grSrc.Name = "grSrc";
            this.grSrc.Size = new System.Drawing.Size(774, 179);
            this.grSrc.TabIndex = 0;
            this.grSrc.TabStop = false;
            this.grSrc.Text = "Source";
            // 
            // btnGetTableName
            // 
            this.btnGetTableName.Location = new System.Drawing.Point(535, 23);
            this.btnGetTableName.Name = "btnGetTableName";
            this.btnGetTableName.Size = new System.Drawing.Size(177, 24);
            this.btnGetTableName.TabIndex = 12;
            this.btnGetTableName.Text = "List Table Foxpro, Excel";
            this.btnGetTableName.UseVisualStyleBackColor = true;
            this.btnGetTableName.Click += new System.EventHandler(this.btnGetTableName_Click);
            // 
            // btnTestCntSrc
            // 
            this.btnTestCntSrc.Location = new System.Drawing.Point(410, 23);
            this.btnTestCntSrc.Name = "btnTestCntSrc";
            this.btnTestCntSrc.Size = new System.Drawing.Size(119, 24);
            this.btnTestCntSrc.TabIndex = 3;
            this.btnTestCntSrc.Text = "Test connection";
            this.btnTestCntSrc.UseVisualStyleBackColor = true;
            this.btnTestCntSrc.Click += new System.EventHandler(this.btnTestCntSrc_Click);
            // 
            // lblTblSrc
            // 
            this.lblTblSrc.AutoSize = true;
            this.lblTblSrc.Location = new System.Drawing.Point(12, 122);
            this.lblTblSrc.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.lblTblSrc.Name = "lblTblSrc";
            this.lblTblSrc.Size = new System.Drawing.Size(77, 14);
            this.lblTblSrc.TabIndex = 11;
            this.lblTblSrc.Text = "Table source";
            // 
            // txtTblSrc
            // 
            this.txtTblSrc.Location = new System.Drawing.Point(161, 122);
            this.txtTblSrc.Name = "txtTblSrc";
            this.txtTblSrc.Size = new System.Drawing.Size(604, 22);
            this.txtTblSrc.TabIndex = 2;
            this.txtTblSrc.Enter += new System.EventHandler(this.txtTblSrc_Enter);
            this.txtTblSrc.Leave += new System.EventHandler(this.txtTblSrc_Leave);
            // 
            // lblCntSrc
            // 
            this.lblCntSrc.AutoSize = true;
            this.lblCntSrc.Location = new System.Drawing.Point(11, 56);
            this.lblCntSrc.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.lblCntSrc.Name = "lblCntSrc";
            this.lblCntSrc.Size = new System.Drawing.Size(127, 14);
            this.lblCntSrc.TabIndex = 9;
            this.lblCntSrc.Text = "Connect string source";
            // 
            // txtCntSrc
            // 
            this.txtCntSrc.Location = new System.Drawing.Point(161, 53);
            this.txtCntSrc.Multiline = true;
            this.txtCntSrc.Name = "txtCntSrc";
            this.txtCntSrc.Size = new System.Drawing.Size(604, 65);
            this.txtCntSrc.TabIndex = 1;
            this.txtCntSrc.Enter += new System.EventHandler(this.txtCntSrc_Enter);
            this.txtCntSrc.Leave += new System.EventHandler(this.txtCntSrc_Leave);
            // 
            // cboProviderSrc
            // 
            this.cboProviderSrc.FormattingEnabled = true;
            this.cboProviderSrc.Items.AddRange(new object[] {
            "Oracle",
            "Ms SQL",
            "My SQL",
            "Fox",
            "Excel",
            "Excel 2007",
            "Access"});
            this.cboProviderSrc.Location = new System.Drawing.Point(161, 23);
            this.cboProviderSrc.Name = "cboProviderSrc";
            this.cboProviderSrc.Size = new System.Drawing.Size(242, 22);
            this.cboProviderSrc.TabIndex = 0;
            this.cboProviderSrc.SelectedIndexChanged += new System.EventHandler(this.cboProviderSrc_SelectedIndexChanged);
            // 
            // lblProviderSrc
            // 
            this.lblProviderSrc.AutoSize = true;
            this.lblProviderSrc.Location = new System.Drawing.Point(11, 30);
            this.lblProviderSrc.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.lblProviderSrc.Name = "lblProviderSrc";
            this.lblProviderSrc.Size = new System.Drawing.Size(91, 14);
            this.lblProviderSrc.TabIndex = 6;
            this.lblProviderSrc.Text = "Provider source";
            // 
            // grDest
            // 
            this.grDest.Controls.Add(this.btnCutDMY);
            this.grDest.Controls.Add(this.txtSchemaDest);
            this.grDest.Controls.Add(this.lblSchemaDest);
            this.grDest.Controls.Add(this.btnTestCntDest);
            this.grDest.Controls.Add(this.txtTblDest);
            this.grDest.Controls.Add(this.lblTblDest);
            this.grDest.Controls.Add(this.lblCntDest);
            this.grDest.Controls.Add(this.txtCntDest);
            this.grDest.Controls.Add(this.cboProviderDest);
            this.grDest.Controls.Add(this.lblCntStringDest);
            this.grDest.Location = new System.Drawing.Point(12, 195);
            this.grDest.Name = "grDest";
            this.grDest.Size = new System.Drawing.Size(774, 179);
            this.grDest.TabIndex = 1;
            this.grDest.TabStop = false;
            this.grDest.Text = "Dest";
            // 
            // btnCutDMY
            // 
            this.btnCutDMY.Location = new System.Drawing.Point(302, 150);
            this.btnCutDMY.Name = "btnCutDMY";
            this.btnCutDMY.Size = new System.Drawing.Size(179, 24);
            this.btnCutDMY.TabIndex = 16;
            this.btnCutDMY.Text = "Cut DMY in Table Name";
            this.toolTip.SetToolTip(this.btnCutDMY, "Xoá chuỗi ngày tháng năm trong tên table sau dấu _");
            this.btnCutDMY.UseVisualStyleBackColor = true;
            this.btnCutDMY.Click += new System.EventHandler(this.btnCutDMY_Click);
            // 
            // txtSchemaDest
            // 
            this.txtSchemaDest.Location = new System.Drawing.Point(161, 150);
            this.txtSchemaDest.Name = "txtSchemaDest";
            this.txtSchemaDest.Size = new System.Drawing.Size(136, 22);
            this.txtSchemaDest.TabIndex = 3;
            this.toolTip.SetToolTip(this.txtSchemaDest, "Tên schema đích dùng cho Oracle");
            // 
            // lblSchemaDest
            // 
            this.lblSchemaDest.AutoSize = true;
            this.lblSchemaDest.Location = new System.Drawing.Point(11, 150);
            this.lblSchemaDest.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.lblSchemaDest.Name = "lblSchemaDest";
            this.lblSchemaDest.Size = new System.Drawing.Size(78, 14);
            this.lblSchemaDest.TabIndex = 15;
            this.lblSchemaDest.Text = "Schema dest";
            // 
            // btnTestCntDest
            // 
            this.btnTestCntDest.Location = new System.Drawing.Point(409, 21);
            this.btnTestCntDest.Name = "btnTestCntDest";
            this.btnTestCntDest.Size = new System.Drawing.Size(119, 24);
            this.btnTestCntDest.TabIndex = 4;
            this.btnTestCntDest.Text = "Test connection";
            this.btnTestCntDest.UseVisualStyleBackColor = true;
            this.btnTestCntDest.Click += new System.EventHandler(this.btnTestCntDest_Click);
            // 
            // txtTblDest
            // 
            this.txtTblDest.Location = new System.Drawing.Point(161, 121);
            this.txtTblDest.Name = "txtTblDest";
            this.txtTblDest.Size = new System.Drawing.Size(604, 22);
            this.txtTblDest.TabIndex = 2;
            this.txtTblDest.Enter += new System.EventHandler(this.txtTblDest_Enter);
            this.txtTblDest.Leave += new System.EventHandler(this.txtTblDest_Leave);
            // 
            // lblTblDest
            // 
            this.lblTblDest.AutoSize = true;
            this.lblTblDest.Location = new System.Drawing.Point(11, 121);
            this.lblTblDest.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.lblTblDest.Name = "lblTblDest";
            this.lblTblDest.Size = new System.Drawing.Size(65, 14);
            this.lblTblDest.TabIndex = 12;
            this.lblTblDest.Text = "Table dest";
            // 
            // lblCntDest
            // 
            this.lblCntDest.AutoSize = true;
            this.lblCntDest.Location = new System.Drawing.Point(11, 55);
            this.lblCntDest.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.lblCntDest.Name = "lblCntDest";
            this.lblCntDest.Size = new System.Drawing.Size(115, 14);
            this.lblCntDest.TabIndex = 11;
            this.lblCntDest.Text = "Connect string dest";
            // 
            // txtCntDest
            // 
            this.txtCntDest.Location = new System.Drawing.Point(161, 52);
            this.txtCntDest.Multiline = true;
            this.txtCntDest.Name = "txtCntDest";
            this.txtCntDest.Size = new System.Drawing.Size(604, 65);
            this.txtCntDest.TabIndex = 1;
            // 
            // cboProviderDest
            // 
            this.cboProviderDest.FormattingEnabled = true;
            this.cboProviderDest.Items.AddRange(new object[] {
            "Oracle",
            "Ms SQL",
            "My SQL",
            "Fox",
            "Excel",
            "Excel 2007",
            "Access"});
            this.cboProviderDest.Location = new System.Drawing.Point(161, 22);
            this.cboProviderDest.Name = "cboProviderDest";
            this.cboProviderDest.Size = new System.Drawing.Size(242, 22);
            this.cboProviderDest.TabIndex = 0;
            this.cboProviderDest.SelectedIndexChanged += new System.EventHandler(this.cboProviderDest_SelectedIndexChanged);
            // 
            // lblCntStringDest
            // 
            this.lblCntStringDest.AutoSize = true;
            this.lblCntStringDest.Location = new System.Drawing.Point(11, 29);
            this.lblCntStringDest.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.lblCntStringDest.Name = "lblCntStringDest";
            this.lblCntStringDest.Size = new System.Drawing.Size(79, 14);
            this.lblCntStringDest.TabIndex = 8;
            this.lblCntStringDest.Text = "Provider dest";
            // 
            // btnImport
            // 
            this.btnImport.Location = new System.Drawing.Point(287, 547);
            this.btnImport.Name = "btnImport";
            this.btnImport.Size = new System.Drawing.Size(78, 32);
            this.btnImport.TabIndex = 3;
            this.btnImport.Text = "Import";
            this.btnImport.UseVisualStyleBackColor = true;
            this.btnImport.Click += new System.EventHandler(this.btnImport_Click);
            // 
            // btnCancel
            // 
            this.btnCancel.Location = new System.Drawing.Point(371, 547);
            this.btnCancel.Name = "btnCancel";
            this.btnCancel.Size = new System.Drawing.Size(78, 32);
            this.btnCancel.TabIndex = 4;
            this.btnCancel.Text = "Cancel";
            this.btnCancel.UseVisualStyleBackColor = true;
            this.btnCancel.Click += new System.EventHandler(this.btnCancel_Click);
            // 
            // txtMsg
            // 
            this.txtMsg.Location = new System.Drawing.Point(12, 400);
            this.txtMsg.Multiline = true;
            this.txtMsg.Name = "txtMsg";
            this.txtMsg.ScrollBars = System.Windows.Forms.ScrollBars.Both;
            this.txtMsg.Size = new System.Drawing.Size(773, 141);
            this.txtMsg.TabIndex = 2;
            // 
            // prbProcess
            // 
            this.prbProcess.Location = new System.Drawing.Point(173, 384);
            this.prbProcess.Name = "prbProcess";
            this.prbProcess.Size = new System.Drawing.Size(612, 8);
            this.prbProcess.TabIndex = 5;
            this.prbProcess.Visible = false;
            // 
            // lblTuSo
            // 
            this.lblTuSo.AutoSize = true;
            this.lblTuSo.ForeColor = System.Drawing.SystemColors.HotTrack;
            this.lblTuSo.Location = new System.Drawing.Point(22, 379);
            this.lblTuSo.Name = "lblTuSo";
            this.lblTuSo.Size = new System.Drawing.Size(27, 14);
            this.lblTuSo.TabIndex = 6;
            this.lblTuSo.Text = ".....";
            // 
            // txtLimit
            // 
            this.txtLimit.Location = new System.Drawing.Point(161, 148);
            this.txtLimit.MaxLength = 10;
            this.txtLimit.Name = "txtLimit";
            this.txtLimit.Size = new System.Drawing.Size(242, 22);
            this.txtLimit.TabIndex = 16;
            this.txtLimit.Text = "100";
            this.toolTip.SetToolTip(this.txtLimit, "Giới hạn số lượng record import cho mỗi table, xóa trống để bỏ giới hạn");
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(12, 148);
            this.label1.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(76, 14);
            this.label1.TabIndex = 17;
            this.label1.Text = "Limit records";
            // 
            // frmImport
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(7F, 14F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(792, 582);
            this.Controls.Add(this.lblTuSo);
            this.Controls.Add(this.prbProcess);
            this.Controls.Add(this.txtMsg);
            this.Controls.Add(this.btnCancel);
            this.Controls.Add(this.btnImport);
            this.Controls.Add(this.grDest);
            this.Controls.Add(this.grSrc);
            this.Font = new System.Drawing.Font("Tahoma", 8.830189F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.Margin = new System.Windows.Forms.Padding(4);
            this.MaximizeBox = false;
            this.Name = "frmImport";
            this.Text = "Import Data";
            this.toolTip.SetToolTip(this, "List table name on free table foxpro on directory and sheets name of Excel file.");
            this.Load += new System.EventHandler(this.frmImport_Load);
            this.grSrc.ResumeLayout(false);
            this.grSrc.PerformLayout();
            this.grDest.ResumeLayout(false);
            this.grDest.PerformLayout();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.GroupBox grSrc;
        private System.Windows.Forms.Label lblCntSrc;
        private System.Windows.Forms.TextBox txtCntSrc;
        private System.Windows.Forms.ComboBox cboProviderSrc;
        private System.Windows.Forms.Label lblProviderSrc;
        private System.Windows.Forms.GroupBox grDest;
        private System.Windows.Forms.Label lblCntDest;
        private System.Windows.Forms.TextBox txtCntDest;
        private System.Windows.Forms.ComboBox cboProviderDest;
        private System.Windows.Forms.Label lblCntStringDest;
        private System.Windows.Forms.Button btnImport;
        private System.Windows.Forms.Button btnCancel;
        private System.Windows.Forms.TextBox txtMsg;
        private System.Windows.Forms.Label lblTblSrc;
        private System.Windows.Forms.TextBox txtTblSrc;
        private System.Windows.Forms.TextBox txtTblDest;
        private System.Windows.Forms.Label lblTblDest;
        private System.Windows.Forms.Button btnTestCntSrc;
        private System.Windows.Forms.Button btnTestCntDest;
        private System.Windows.Forms.TextBox txtSchemaDest;
        private System.Windows.Forms.Label lblSchemaDest;
        private System.Windows.Forms.ToolTip toolTip;
        private System.Windows.Forms.Button btnGetTableName;
        private System.Windows.Forms.ProgressBar prbProcess;
        private System.Windows.Forms.Label lblTuSo;
        private System.Windows.Forms.Button btnCutDMY;
        private System.Windows.Forms.TextBox txtLimit;
        private System.Windows.Forms.Label label1;
    }
}

