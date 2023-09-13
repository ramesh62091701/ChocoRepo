namespace SapBOMigration
{
    partial class Form1
    {
        /// <summary>
        ///  Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        ///  Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows Form Designer generated code

        /// <summary>
        ///  Required method for Designer support - do not modify
        ///  the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            button1 = new Button();
            chkUniverses = new CheckBox();
            chkDocuments = new CheckBox();
            btnAnalyze = new Button();
            textBox1 = new TextBox();
            lbl = new Label();
            label1 = new Label();
            label2 = new Label();
            panel1 = new Panel();
            lnkPrevReport = new LinkLabel();
            chkConnection = new CheckBox();
            checkBox1 = new CheckBox();
            chkDocumentVariables = new CheckBox();
            chkReportElements = new CheckBox();
            chkObjects = new CheckBox();
            chkClasses = new CheckBox();
            chkDataProviders = new CheckBox();
            chkQueries = new CheckBox();
            chkReports = new CheckBox();
            tbAnalysisLog = new TextBox();
            panel2 = new Panel();
            lblMode = new Label();
            cbMode = new ComboBox();
            rbDocument = new RadioButton();
            rbUniverse = new RadioButton();
            cbUniverseList = new ComboBox();
            label5 = new Label();
            lblConnDetails = new Label();
            cbConnList = new ComboBox();
            btnCsv = new Button();
            label3 = new Label();
            pbLogoLightning = new PictureBox();
            pbLogoSonata = new PictureBox();
            lblHeaderText = new Label();
            label4 = new Label();
            panel1.SuspendLayout();
            panel2.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)pbLogoLightning).BeginInit();
            ((System.ComponentModel.ISupportInitialize)pbLogoSonata).BeginInit();
            SuspendLayout();
            // 
            // button1
            // 
            button1.Location = new Point(32, 304);
            button1.Name = "button1";
            button1.Size = new Size(267, 33);
            button1.TabIndex = 0;
            button1.Text = "Create Dataset";
            button1.UseVisualStyleBackColor = true;
            button1.Click += button1_Click;
            // 
            // chkUniverses
            // 
            chkUniverses.AutoSize = true;
            chkUniverses.Checked = true;
            chkUniverses.CheckState = CheckState.Checked;
            chkUniverses.Location = new Point(23, 72);
            chkUniverses.Name = "chkUniverses";
            chkUniverses.Size = new Size(113, 29);
            chkUniverses.TabIndex = 1;
            chkUniverses.Text = "Universes";
            chkUniverses.UseVisualStyleBackColor = true;
            // 
            // chkDocuments
            // 
            chkDocuments.AutoSize = true;
            chkDocuments.Checked = true;
            chkDocuments.CheckState = CheckState.Checked;
            chkDocuments.Location = new Point(23, 103);
            chkDocuments.Name = "chkDocuments";
            chkDocuments.Size = new Size(129, 29);
            chkDocuments.TabIndex = 2;
            chkDocuments.Text = "Documents";
            chkDocuments.UseVisualStyleBackColor = true;
            // 
            // btnAnalyze
            // 
            btnAnalyze.Location = new Point(94, 252);
            btnAnalyze.Name = "btnAnalyze";
            btnAnalyze.Size = new Size(339, 33);
            btnAnalyze.TabIndex = 3;
            btnAnalyze.Text = "Discover";
            btnAnalyze.UseVisualStyleBackColor = true;
            btnAnalyze.Click += btnAnalyze_Click;
            // 
            // textBox1
            // 
            textBox1.Location = new Point(157, 208);
            textBox1.Name = "textBox1";
            textBox1.Size = new Size(150, 31);
            textBox1.TabIndex = 5;
            textBox1.Text = "6699";
            // 
            // lbl
            // 
            lbl.AutoSize = true;
            lbl.Location = new Point(31, 208);
            lbl.Name = "lbl";
            lbl.Size = new Size(120, 25);
            lbl.TabIndex = 6;
            lbl.Text = "Document Id:";
            // 
            // label1
            // 
            label1.AutoSize = true;
            label1.Location = new Point(32, 76);
            label1.Name = "label1";
            label1.Size = new Size(105, 25);
            label1.TabIndex = 7;
            label1.Text = "Datasource:";
            // 
            // label2
            // 
            label2.AutoSize = true;
            label2.Font = new Font("Segoe UI", 16F, FontStyle.Regular, GraphicsUnit.Point);
            label2.Location = new Point(19, 12);
            label2.Name = "label2";
            label2.Size = new Size(156, 45);
            label2.TabIndex = 8;
            label2.Text = "Discovery";
            // 
            // panel1
            // 
            panel1.AutoSizeMode = AutoSizeMode.GrowAndShrink;
            panel1.BorderStyle = BorderStyle.FixedSingle;
            panel1.Controls.Add(lnkPrevReport);
            panel1.Controls.Add(chkConnection);
            panel1.Controls.Add(checkBox1);
            panel1.Controls.Add(chkDocumentVariables);
            panel1.Controls.Add(chkReportElements);
            panel1.Controls.Add(chkObjects);
            panel1.Controls.Add(chkClasses);
            panel1.Controls.Add(chkDataProviders);
            panel1.Controls.Add(chkQueries);
            panel1.Controls.Add(chkReports);
            panel1.Controls.Add(chkDocuments);
            panel1.Controls.Add(label2);
            panel1.Controls.Add(chkUniverses);
            panel1.Controls.Add(btnAnalyze);
            panel1.Location = new Point(20, 123);
            panel1.Name = "panel1";
            panel1.Size = new Size(621, 414);
            panel1.TabIndex = 9;
            // 
            // lnkPrevReport
            // 
            lnkPrevReport.AutoSize = true;
            lnkPrevReport.Location = new Point(467, 370);
            lnkPrevReport.Margin = new Padding(4, 0, 4, 0);
            lnkPrevReport.Name = "lnkPrevReport";
            lnkPrevReport.Size = new Size(137, 25);
            lnkPrevReport.TabIndex = 15;
            lnkPrevReport.TabStop = true;
            lnkPrevReport.Text = "Previous Report";
            lnkPrevReport.LinkClicked += lnkPrevReport_LinkClicked;
            // 
            // chkConnection
            // 
            chkConnection.AutoSize = true;
            chkConnection.Checked = true;
            chkConnection.CheckState = CheckState.Checked;
            chkConnection.Location = new Point(374, 175);
            chkConnection.Name = "chkConnection";
            chkConnection.Size = new Size(136, 29);
            chkConnection.TabIndex = 14;
            chkConnection.Text = "Connections";
            chkConnection.UseVisualStyleBackColor = true;
            // 
            // checkBox1
            // 
            checkBox1.AutoSize = true;
            checkBox1.Checked = true;
            checkBox1.CheckState = CheckState.Checked;
            checkBox1.Location = new Point(183, 175);
            checkBox1.Name = "checkBox1";
            checkBox1.Size = new Size(130, 29);
            checkBox1.TabIndex = 13;
            checkBox1.Text = "Expressions";
            checkBox1.UseVisualStyleBackColor = true;
            // 
            // chkDocumentVariables
            // 
            chkDocumentVariables.AutoSize = true;
            chkDocumentVariables.Checked = true;
            chkDocumentVariables.CheckState = CheckState.Checked;
            chkDocumentVariables.Location = new Point(183, 103);
            chkDocumentVariables.Name = "chkDocumentVariables";
            chkDocumentVariables.Size = new Size(196, 29);
            chkDocumentVariables.TabIndex = 12;
            chkDocumentVariables.Text = "Document Variables";
            chkDocumentVariables.UseVisualStyleBackColor = true;
            // 
            // chkReportElements
            // 
            chkReportElements.AutoSize = true;
            chkReportElements.Checked = true;
            chkReportElements.CheckState = CheckState.Checked;
            chkReportElements.Location = new Point(183, 138);
            chkReportElements.Name = "chkReportElements";
            chkReportElements.Size = new Size(167, 29);
            chkReportElements.TabIndex = 11;
            chkReportElements.Text = "Report elements";
            chkReportElements.UseVisualStyleBackColor = true;
            // 
            // chkObjects
            // 
            chkObjects.AutoSize = true;
            chkObjects.Checked = true;
            chkObjects.CheckState = CheckState.Checked;
            chkObjects.Location = new Point(374, 72);
            chkObjects.Name = "chkObjects";
            chkObjects.Size = new Size(98, 29);
            chkObjects.TabIndex = 10;
            chkObjects.Text = "Objects";
            chkObjects.UseVisualStyleBackColor = true;
            // 
            // chkClasses
            // 
            chkClasses.AutoSize = true;
            chkClasses.Checked = true;
            chkClasses.CheckState = CheckState.Checked;
            chkClasses.Location = new Point(183, 68);
            chkClasses.Name = "chkClasses";
            chkClasses.Size = new Size(95, 29);
            chkClasses.TabIndex = 9;
            chkClasses.Text = "Classes";
            chkClasses.UseVisualStyleBackColor = true;
            // 
            // chkDataProviders
            // 
            chkDataProviders.AutoSize = true;
            chkDataProviders.Checked = true;
            chkDataProviders.CheckState = CheckState.Checked;
            chkDataProviders.Location = new Point(23, 175);
            chkDataProviders.Name = "chkDataProviders";
            chkDataProviders.Size = new Size(154, 29);
            chkDataProviders.TabIndex = 6;
            chkDataProviders.Text = "Data Providers";
            chkDataProviders.UseVisualStyleBackColor = true;
            // 
            // chkQueries
            // 
            chkQueries.AutoSize = true;
            chkQueries.Checked = true;
            chkQueries.CheckState = CheckState.Checked;
            chkQueries.Location = new Point(374, 138);
            chkQueries.Name = "chkQueries";
            chkQueries.Size = new Size(133, 29);
            chkQueries.TabIndex = 5;
            chkQueries.Text = "Query plans";
            chkQueries.UseVisualStyleBackColor = true;
            // 
            // chkReports
            // 
            chkReports.AutoSize = true;
            chkReports.Checked = true;
            chkReports.CheckState = CheckState.Checked;
            chkReports.Location = new Point(23, 137);
            chkReports.Name = "chkReports";
            chkReports.Size = new Size(99, 29);
            chkReports.TabIndex = 4;
            chkReports.Text = "Reports";
            chkReports.UseVisualStyleBackColor = true;
            // 
            // tbAnalysisLog
            // 
            tbAnalysisLog.BackColor = SystemColors.ControlLightLight;
            tbAnalysisLog.CausesValidation = false;
            tbAnalysisLog.Location = new Point(20, 581);
            tbAnalysisLog.Multiline = true;
            tbAnalysisLog.Name = "tbAnalysisLog";
            tbAnalysisLog.ReadOnly = true;
            tbAnalysisLog.ScrollBars = ScrollBars.Both;
            tbAnalysisLog.Size = new Size(1295, 286);
            tbAnalysisLog.TabIndex = 9;
            tbAnalysisLog.WordWrap = false;
            // 
            // panel2
            // 
            panel2.BorderStyle = BorderStyle.FixedSingle;
            panel2.Controls.Add(lblMode);
            panel2.Controls.Add(cbMode);
            panel2.Controls.Add(rbDocument);
            panel2.Controls.Add(rbUniverse);
            panel2.Controls.Add(cbUniverseList);
            panel2.Controls.Add(label5);
            panel2.Controls.Add(lblConnDetails);
            panel2.Controls.Add(cbConnList);
            panel2.Controls.Add(btnCsv);
            panel2.Controls.Add(label3);
            panel2.Controls.Add(button1);
            panel2.Controls.Add(label1);
            panel2.Controls.Add(textBox1);
            panel2.Controls.Add(lbl);
            panel2.Location = new Point(673, 123);
            panel2.Name = "panel2";
            panel2.Size = new Size(629, 414);
            panel2.TabIndex = 10;
            // 
            // lblMode
            // 
            lblMode.AutoSize = true;
            lblMode.Location = new Point(37, 256);
            lblMode.Name = "lblMode";
            lblMode.Size = new Size(63, 25);
            lblMode.TabIndex = 18;
            lblMode.Text = "Mode:";
            // 
            // cbMode
            // 
            cbMode.FormattingEnabled = true;
            cbMode.Location = new Point(157, 248);
            cbMode.Name = "cbMode";
            cbMode.Size = new Size(182, 33);
            cbMode.TabIndex = 17;
            // 
            // rbDocument
            // 
            rbDocument.AutoSize = true;
            rbDocument.Checked = true;
            rbDocument.Location = new Point(157, 123);
            rbDocument.Name = "rbDocument";
            rbDocument.Size = new Size(120, 29);
            rbDocument.TabIndex = 15;
            rbDocument.TabStop = true;
            rbDocument.Text = "Document";
            rbDocument.UseVisualStyleBackColor = true;
            rbDocument.CheckedChanged += rbDocument_CheckedChanged;
            // 
            // rbUniverse
            // 
            rbUniverse.AutoSize = true;
            rbUniverse.Location = new Point(37, 123);
            rbUniverse.Name = "rbUniverse";
            rbUniverse.Size = new Size(104, 29);
            rbUniverse.TabIndex = 14;
            rbUniverse.Text = "Universe";
            rbUniverse.UseVisualStyleBackColor = true;
            rbUniverse.CheckedChanged += rbUniverse_CheckedChanged;
            // 
            // cbUniverseList
            // 
            cbUniverseList.Enabled = false;
            cbUniverseList.FormattingEnabled = true;
            cbUniverseList.Location = new Point(157, 167);
            cbUniverseList.Name = "cbUniverseList";
            cbUniverseList.Size = new Size(385, 33);
            cbUniverseList.TabIndex = 13;
            cbUniverseList.Text = "Select universe";
            // 
            // label5
            // 
            label5.AutoSize = true;
            label5.Location = new Point(37, 167);
            label5.Name = "label5";
            label5.Size = new Size(91, 25);
            label5.TabIndex = 12;
            label5.Text = "Universes:";
            // 
            // lblConnDetails
            // 
            lblConnDetails.AutoSize = true;
            lblConnDetails.Location = new Point(166, 175);
            lblConnDetails.Name = "lblConnDetails";
            lblConnDetails.Size = new Size(0, 25);
            lblConnDetails.TabIndex = 11;
            lblConnDetails.Click += label4_Click;
            // 
            // cbConnList
            // 
            cbConnList.AllowDrop = true;
            cbConnList.FormattingEnabled = true;
            cbConnList.Location = new Point(157, 75);
            cbConnList.Name = "cbConnList";
            cbConnList.Size = new Size(385, 33);
            cbConnList.TabIndex = 10;
            cbConnList.Text = "Select Database";
            cbConnList.SelectedIndexChanged += cbConnList_SelectedIndexChanged;
            // 
            // btnCsv
            // 
            btnCsv.Location = new Point(327, 304);
            btnCsv.Name = "btnCsv";
            btnCsv.Size = new Size(239, 33);
            btnCsv.TabIndex = 9;
            btnCsv.Text = "Create Report Data";
            btnCsv.UseVisualStyleBackColor = true;
            btnCsv.Click += btnCsv_Click;
            // 
            // label3
            // 
            label3.AutoSize = true;
            label3.Font = new Font("Segoe UI", 16F, FontStyle.Regular, GraphicsUnit.Point);
            label3.Location = new Point(19, 12);
            label3.Name = "label3";
            label3.Size = new Size(159, 45);
            label3.TabIndex = 8;
            label3.Text = "Migration";
            // 
            // pbLogoLightning
            // 
            pbLogoLightning.ImageLocation = "images/lightning.png";
            pbLogoLightning.Location = new Point(20, 18);
            pbLogoLightning.Margin = new Padding(3, 30, 3, 3);
            pbLogoLightning.Name = "pbLogoLightning";
            pbLogoLightning.Size = new Size(16, 16);
            pbLogoLightning.SizeMode = PictureBoxSizeMode.AutoSize;
            pbLogoLightning.TabIndex = 11;
            pbLogoLightning.TabStop = false;
            pbLogoLightning.WaitOnLoad = true;
            // 
            // pbLogoSonata
            // 
            pbLogoSonata.Anchor = AnchorStyles.Top | AnchorStyles.Right;
            pbLogoSonata.BackgroundImageLayout = ImageLayout.Center;
            pbLogoSonata.ImageLocation = "images/sonata.png";
            pbLogoSonata.Location = new Point(1249, 3);
            pbLogoSonata.MaximumSize = new Size(86, 100);
            pbLogoSonata.Name = "pbLogoSonata";
            pbLogoSonata.Size = new Size(86, 100);
            pbLogoSonata.SizeMode = PictureBoxSizeMode.Zoom;
            pbLogoSonata.TabIndex = 12;
            pbLogoSonata.TabStop = false;
            // 
            // lblHeaderText
            // 
            lblHeaderText.Anchor = AnchorStyles.Top | AnchorStyles.Right;
            lblHeaderText.AutoSize = true;
            lblHeaderText.Font = new Font("Segoe UI", 18F, FontStyle.Bold, GraphicsUnit.Point);
            lblHeaderText.ForeColor = SystemColors.ActiveCaptionText;
            lblHeaderText.ImageAlign = ContentAlignment.TopCenter;
            lblHeaderText.Location = new Point(324, 18);
            lblHeaderText.Name = "lblHeaderText";
            lblHeaderText.Size = new Size(627, 48);
            lblHeaderText.TabIndex = 0;
            lblHeaderText.Text = "SAP BO Reports Migration Assistant";
            lblHeaderText.TextAlign = ContentAlignment.TopCenter;
            // 
            // label4
            // 
            label4.AutoSize = true;
            label4.Location = new Point(23, 553);
            label4.Name = "label4";
            label4.Size = new Size(54, 25);
            label4.TabIndex = 13;
            label4.Text = "Logs:";
            // 
            // Form1
            // 
            AutoScaleDimensions = new SizeF(10F, 25F);
            AutoScaleMode = AutoScaleMode.Font;
            AutoSizeMode = AutoSizeMode.GrowAndShrink;
            ClientSize = new Size(1347, 908);
            Controls.Add(label4);
            Controls.Add(lblHeaderText);
            Controls.Add(tbAnalysisLog);
            Controls.Add(pbLogoSonata);
            Controls.Add(pbLogoLightning);
            Controls.Add(panel2);
            Controls.Add(panel1);
            MinimumSize = new Size(1361, 936);
            Name = "Form1";
            Text = "SAP BO Migration Assistant";
            Load += Form1_Load;
            panel1.ResumeLayout(false);
            panel1.PerformLayout();
            panel2.ResumeLayout(false);
            panel2.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)pbLogoLightning).EndInit();
            ((System.ComponentModel.ISupportInitialize)pbLogoSonata).EndInit();
            ResumeLayout(false);
            PerformLayout();
        }

        #endregion

        private Button button1;
        private CheckBox chkUniverses;
        private CheckBox chkDocuments;
        private Button btnAnalyze;
        private TextBox textBox1;
        private Label lbl;
        private Label label1;
        private Label label2;
        private Panel panel1;
        private Panel panel2;
        private CheckBox chkQueries;
        private CheckBox chkReports;
        private CheckBox chkDataProviders;
        private Label label3;
        private PictureBox pbLogoLightning;
        private PictureBox pbLogoSonata;
        private Label lblHeaderText;
        private TextBox tbAnalysisLog;
        private CheckBox chkDocumentVariables;
        private CheckBox chkReportElements;
        private CheckBox chkObjects;
        private CheckBox chkClasses;
        private CheckBox checkBox1;
        private Button btnCsv;
        private CheckBox chkConnection;
        private ComboBox cbConnList;
        private Label lblConnDetails;
        private Label label4;
        private LinkLabel lnkPrevReport;
        private ComboBox cbUniverseList;
        private Label label5;
        private RadioButton rbDocument;
        private RadioButton rbUniverse;
        private Label lblMode;
        private ComboBox cbMode;
    }
}