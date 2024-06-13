namespace Aspx_To_React
{
    partial class AspxConverter
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
            folderBrowserDialog1 = new FolderBrowserDialog();
            txtFigmaPath = new TextBox();
            btnConvert = new Button();
            txtAspxPath = new TextBox();
            lblFigma = new Label();
            lblAspx = new Label();
            txtLogs = new TextBox();
            txtOutput = new TextBox();
            lblOutput = new Label();
            btnImage = new Button();
            button2 = new Button();
            button3 = new Button();
            radioButton1 = new RadioButton();
            radioButton2 = new RadioButton();
            label1 = new Label();
            pbLogoSonata = new PictureBox();
            pictureBox2 = new PictureBox();
            openFileDialog1 = new OpenFileDialog();
            label2 = new Label();
            lblFigmaUrl = new Label();
            txtFigmaUrl = new TextBox();
            rdbImage = new RadioButton();
            rdbFileUrl = new RadioButton();
            panel1 = new Panel();
            rdbUseBoth = new RadioButton();
            label3 = new Label();
            btnConvertToReact = new Button();
            ((System.ComponentModel.ISupportInitialize)pbLogoSonata).BeginInit();
            ((System.ComponentModel.ISupportInitialize)pictureBox2).BeginInit();
            panel1.SuspendLayout();
            SuspendLayout();
            // 
            // txtFigmaPath
            // 
            txtFigmaPath.Location = new Point(108, 127);
            txtFigmaPath.Name = "txtFigmaPath";
            txtFigmaPath.Size = new Size(612, 31);
            txtFigmaPath.TabIndex = 0;
            txtFigmaPath.Text = "D:\\Demo\\CSOD\\image.png";
            // 
            // btnConvert
            // 
            btnConvert.BackColor = Color.FromArgb(255, 128, 0);
            btnConvert.Font = new Font("Segoe UI", 9F, FontStyle.Bold, GraphicsUnit.Point);
            btnConvert.Location = new Point(798, 239);
            btnConvert.Name = "btnConvert";
            btnConvert.Size = new Size(209, 111);
            btnConvert.TabIndex = 1;
            btnConvert.Text = "Convert to HTML";
            btnConvert.UseVisualStyleBackColor = false;
            btnConvert.Click += btnConvert_Click;
            // 
            // txtAspxPath
            // 
            txtAspxPath.Location = new Point(111, 234);
            txtAspxPath.Name = "txtAspxPath";
            txtAspxPath.Size = new Size(612, 31);
            txtAspxPath.TabIndex = 2;
            txtAspxPath.Text = "D:\\Demo\\CSOD\\Assign.aspx";
            // 
            // lblFigma
            // 
            lblFigma.AutoSize = true;
            lblFigma.Location = new Point(30, 131);
            lblFigma.Name = "lblFigma";
            lblFigma.Size = new Size(66, 25);
            lblFigma.TabIndex = 3;
            lblFigma.Text = "Image:";
            // 
            // lblAspx
            // 
            lblAspx.AutoSize = true;
            lblAspx.Location = new Point(28, 239);
            lblAspx.Name = "lblAspx";
            lblAspx.Size = new Size(55, 25);
            lblAspx.TabIndex = 4;
            lblAspx.Text = "Aspx:";
            // 
            // txtLogs
            // 
            txtLogs.Location = new Point(28, 403);
            txtLogs.Margin = new Padding(0);
            txtLogs.Multiline = true;
            txtLogs.Name = "txtLogs";
            txtLogs.ScrollBars = ScrollBars.Vertical;
            txtLogs.Size = new Size(1336, 414);
            txtLogs.TabIndex = 5;
            // 
            // txtOutput
            // 
            txtOutput.Location = new Point(114, 329);
            txtOutput.Name = "txtOutput";
            txtOutput.Size = new Size(612, 31);
            txtOutput.TabIndex = 6;
            txtOutput.Text = "D:\\Demo\\CSOD\\output";
            // 
            // lblOutput
            // 
            lblOutput.AutoSize = true;
            lblOutput.Location = new Point(28, 335);
            lblOutput.Name = "lblOutput";
            lblOutput.Size = new Size(73, 25);
            lblOutput.TabIndex = 7;
            lblOutput.Text = "Output:";
            // 
            // btnImage
            // 
            btnImage.Location = new Point(722, 127);
            btnImage.Name = "btnImage";
            btnImage.Size = new Size(42, 34);
            btnImage.TabIndex = 8;
            btnImage.Text = "...";
            btnImage.UseVisualStyleBackColor = true;
            btnImage.Click += button1_Click;
            // 
            // button2
            // 
            button2.Location = new Point(725, 231);
            button2.Name = "button2";
            button2.Size = new Size(42, 34);
            button2.TabIndex = 9;
            button2.Text = "...";
            button2.UseVisualStyleBackColor = true;
            button2.Click += button2_Click;
            // 
            // button3
            // 
            button3.Location = new Point(729, 327);
            button3.Name = "button3";
            button3.Size = new Size(42, 34);
            button3.TabIndex = 10;
            button3.Text = "...";
            button3.UseVisualStyleBackColor = true;
            button3.Click += button3_Click;
            // 
            // radioButton1
            // 
            radioButton1.AutoSize = true;
            radioButton1.Checked = true;
            radioButton1.Location = new Point(32, 275);
            radioButton1.Name = "radioButton1";
            radioButton1.Size = new Size(151, 29);
            radioButton1.TabIndex = 11;
            radioButton1.TabStop = true;
            radioButton1.Text = "Use React MUI";
            radioButton1.UseVisualStyleBackColor = true;
            // 
            // radioButton2
            // 
            radioButton2.AutoSize = true;
            radioButton2.Location = new Point(186, 275);
            radioButton2.Name = "radioButton2";
            radioButton2.Size = new Size(205, 29);
            radioButton2.TabIndex = 12;
            radioButton2.TabStop = true;
            radioButton2.Text = "Use Custom Controls";
            radioButton2.UseVisualStyleBackColor = true;
            // 
            // label1
            // 
            label1.AutoSize = true;
            label1.Font = new Font("Segoe UI", 18F, FontStyle.Bold, GraphicsUnit.Point);
            label1.Location = new Point(576, 9);
            label1.Name = "label1";
            label1.Size = new Size(229, 48);
            label1.TabIndex = 13;
            label1.Text = "Figma2Code";
            // 
            // pbLogoSonata
            // 
            pbLogoSonata.ImageLocation = "images/sonata.png";
            pbLogoSonata.Location = new Point(1151, 7);
            pbLogoSonata.Margin = new Padding(0);
            pbLogoSonata.Name = "pbLogoSonata";
            pbLogoSonata.Size = new Size(213, 149);
            pbLogoSonata.SizeMode = PictureBoxSizeMode.Zoom;
            pbLogoSonata.TabIndex = 14;
            pbLogoSonata.TabStop = false;
            // 
            // pictureBox2
            // 
            pictureBox2.ImageLocation = "images/lightning.png";
            pictureBox2.Location = new Point(12, 7);
            pictureBox2.Name = "pictureBox2";
            pictureBox2.Size = new Size(300, 50);
            pictureBox2.TabIndex = 15;
            pictureBox2.TabStop = false;
            // 
            // openFileDialog1
            // 
            openFileDialog1.FileName = "openFileDialog1";
            // 
            // label2
            // 
            label2.AutoSize = true;
            label2.Location = new Point(28, 370);
            label2.Name = "label2";
            label2.Size = new Size(54, 25);
            label2.TabIndex = 16;
            label2.Text = "Logs:";
            // 
            // lblFigmaUrl
            // 
            lblFigmaUrl.AutoSize = true;
            lblFigmaUrl.Location = new Point(2, 106);
            lblFigmaUrl.Name = "lblFigmaUrl";
            lblFigmaUrl.Size = new Size(38, 25);
            lblFigmaUrl.TabIndex = 18;
            lblFigmaUrl.Text = "Url:";
            // 
            // txtFigmaUrl
            // 
            txtFigmaUrl.Location = new Point(79, 105);
            txtFigmaUrl.Name = "txtFigmaUrl";
            txtFigmaUrl.Size = new Size(612, 31);
            txtFigmaUrl.TabIndex = 18;
            txtFigmaUrl.Text = "https://api.figma.com/v1/files/QFYvJ8SwzxdpTgdcpsSEkc/nodes?ids=1275-25182";
            // 
            // rdbImage
            // 
            rdbImage.AutoSize = true;
            rdbImage.Checked = true;
            rdbImage.Location = new Point(6, 34);
            rdbImage.Name = "rdbImage";
            rdbImage.Size = new Size(121, 29);
            rdbImage.TabIndex = 19;
            rdbImage.TabStop = true;
            rdbImage.Text = "Use Image";
            rdbImage.UseVisualStyleBackColor = true;
            rdbImage.CheckedChanged += rdbImage_CheckedChanged;
            // 
            // rdbFileUrl
            // 
            rdbFileUrl.AutoSize = true;
            rdbFileUrl.Location = new Point(148, 34);
            rdbFileUrl.Name = "rdbFileUrl";
            rdbFileUrl.Size = new Size(93, 29);
            rdbFileUrl.TabIndex = 20;
            rdbFileUrl.TabStop = true;
            rdbFileUrl.Text = "Use Url";
            rdbFileUrl.UseVisualStyleBackColor = true;
            rdbFileUrl.CheckedChanged += rdbFileUrl_CheckedChanged;
            // 
            // panel1
            // 
            panel1.BorderStyle = BorderStyle.FixedSingle;
            panel1.Controls.Add(rdbUseBoth);
            panel1.Controls.Add(label3);
            panel1.Controls.Add(rdbFileUrl);
            panel1.Controls.Add(lblFigmaUrl);
            panel1.Controls.Add(txtFigmaUrl);
            panel1.Controls.Add(rdbImage);
            panel1.Location = new Point(28, 59);
            panel1.Name = "panel1";
            panel1.Size = new Size(871, 150);
            panel1.TabIndex = 21;
            // 
            // rdbUseBoth
            // 
            rdbUseBoth.AutoSize = true;
            rdbUseBoth.Location = new Point(264, 34);
            rdbUseBoth.Name = "rdbUseBoth";
            rdbUseBoth.Size = new Size(109, 29);
            rdbUseBoth.TabIndex = 22;
            rdbUseBoth.TabStop = true;
            rdbUseBoth.Text = "Use both";
            rdbUseBoth.UseVisualStyleBackColor = true;
            rdbUseBoth.CheckedChanged += rdbUseBoth_CheckedChanged;
            // 
            // label3
            // 
            label3.AutoSize = true;
            label3.Font = new Font("Segoe UI", 9F, FontStyle.Bold, GraphicsUnit.Point);
            label3.Location = new Point(7, 0);
            label3.Name = "label3";
            label3.Size = new Size(63, 25);
            label3.TabIndex = 21;
            label3.Text = "Figma";
            // 
            // btnConvertToReact
            // 
            btnConvertToReact.BackColor = Color.FromArgb(255, 128, 0);
            btnConvertToReact.Font = new Font("Segoe UI", 9F, FontStyle.Bold, GraphicsUnit.Point);
            btnConvertToReact.Location = new Point(1013, 239);
            btnConvertToReact.Name = "btnConvertToReact";
            btnConvertToReact.Size = new Size(209, 111);
            btnConvertToReact.TabIndex = 22;
            btnConvertToReact.Text = "Convert to React";
            btnConvertToReact.UseVisualStyleBackColor = false;
            btnConvertToReact.Click += btnConvertToReact_Click;
            // 
            // AspxConverter
            // 
            AutoScaleDimensions = new SizeF(10F, 25F);
            AutoScaleMode = AutoScaleMode.Font;
            ClientSize = new Size(1370, 839);
            Controls.Add(btnConvertToReact);
            Controls.Add(label2);
            Controls.Add(pictureBox2);
            Controls.Add(pbLogoSonata);
            Controls.Add(label1);
            Controls.Add(radioButton2);
            Controls.Add(radioButton1);
            Controls.Add(button3);
            Controls.Add(button2);
            Controls.Add(btnImage);
            Controls.Add(lblOutput);
            Controls.Add(txtOutput);
            Controls.Add(txtLogs);
            Controls.Add(lblAspx);
            Controls.Add(lblFigma);
            Controls.Add(txtAspxPath);
            Controls.Add(btnConvert);
            Controls.Add(txtFigmaPath);
            Controls.Add(panel1);
            Name = "AspxConverter";
            Text = "Figma2Code Assistant";
            ((System.ComponentModel.ISupportInitialize)pbLogoSonata).EndInit();
            ((System.ComponentModel.ISupportInitialize)pictureBox2).EndInit();
            panel1.ResumeLayout(false);
            panel1.PerformLayout();
            ResumeLayout(false);
            PerformLayout();
        }

        #endregion

        private FolderBrowserDialog folderBrowserDialog1;
        private TextBox txtFigmaPath;
        private Button btnConvert;
        private TextBox txtAspxPath;
        private Label lblFigma;
        private Label lblAspx;
        private TextBox txtLogs;
        private TextBox txtOutput;
        private Label lblOutput;
        private Button btnImage;
        private Button button2;
        private Button button3;
        private RadioButton radioButton1;
        private RadioButton radioButton2;
        private Label label1;
        private PictureBox pbLogoSonata;
        private PictureBox pictureBox2;
        private OpenFileDialog openFileDialog1;
        private Label label2;
        private Label lblFigmaUrl;
        private TextBox txtFigmaUrl;
        private RadioButton rdbImage;
        private RadioButton rdbFileUrl;
        private Panel panel1;
        private Label label3;
        private Button btnConvertToReact;
        private RadioButton rdbUseBoth;
    }
}
