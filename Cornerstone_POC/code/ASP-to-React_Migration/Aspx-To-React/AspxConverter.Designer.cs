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
            button1 = new Button();
            button2 = new Button();
            button3 = new Button();
            radioButton1 = new RadioButton();
            radioButton2 = new RadioButton();
            label1 = new Label();
            pbLogoSonata = new PictureBox();
            pictureBox2 = new PictureBox();
            openFileDialog1 = new OpenFileDialog();
            ((System.ComponentModel.ISupportInitialize)pbLogoSonata).BeginInit();
            ((System.ComponentModel.ISupportInitialize)pictureBox2).BeginInit();
            SuspendLayout();
            // 
            // txtFigmaPath
            // 
            txtFigmaPath.Location = new Point(123, 88);
            txtFigmaPath.Name = "txtFigmaPath";
            txtFigmaPath.Size = new Size(612, 31);
            txtFigmaPath.TabIndex = 0;
            // 
            // btnConvert
            // 
            btnConvert.Location = new Point(123, 256);
            btnConvert.Name = "btnConvert";
            btnConvert.Size = new Size(370, 34);
            btnConvert.TabIndex = 1;
            btnConvert.Text = "Convert";
            btnConvert.UseVisualStyleBackColor = true;
            btnConvert.Click += btnConvert_Click;
            // 
            // txtAspxPath
            // 
            txtAspxPath.Location = new Point(123, 124);
            txtAspxPath.Name = "txtAspxPath";
            txtAspxPath.Size = new Size(612, 31);
            txtAspxPath.TabIndex = 2;
            // 
            // lblFigma
            // 
            lblFigma.AutoSize = true;
            lblFigma.Location = new Point(28, 94);
            lblFigma.Name = "lblFigma";
            lblFigma.Size = new Size(65, 25);
            lblFigma.TabIndex = 3;
            lblFigma.Text = "Figma:";
            // 
            // lblAspx
            // 
            lblAspx.AutoSize = true;
            lblAspx.Location = new Point(28, 131);
            lblAspx.Name = "lblAspx";
            lblAspx.Size = new Size(55, 25);
            lblAspx.TabIndex = 4;
            lblAspx.Text = "Aspx:";
            // 
            // txtLogs
            // 
            txtLogs.Location = new Point(123, 319);
            txtLogs.Margin = new Padding(0);
            txtLogs.Multiline = true;
            txtLogs.Name = "txtLogs";
            txtLogs.ScrollBars = ScrollBars.Vertical;
            txtLogs.Size = new Size(1241, 404);
            txtLogs.TabIndex = 5;
            // 
            // txtOutput
            // 
            txtOutput.Location = new Point(123, 161);
            txtOutput.Name = "txtOutput";
            txtOutput.Size = new Size(612, 31);
            txtOutput.TabIndex = 6;
            // 
            // lblOutput
            // 
            lblOutput.AutoSize = true;
            lblOutput.Location = new Point(28, 167);
            lblOutput.Name = "lblOutput";
            lblOutput.Size = new Size(73, 25);
            lblOutput.TabIndex = 7;
            lblOutput.Text = "Output:";
            // 
            // button1
            // 
            button1.Location = new Point(741, 86);
            button1.Name = "button1";
            button1.Size = new Size(42, 34);
            button1.TabIndex = 8;
            button1.Text = "...";
            button1.UseVisualStyleBackColor = true;
            button1.Click += button1_Click;
            // 
            // button2
            // 
            button2.Location = new Point(741, 122);
            button2.Name = "button2";
            button2.Size = new Size(42, 34);
            button2.TabIndex = 9;
            button2.Text = "...";
            button2.UseVisualStyleBackColor = true;
            button2.Click += button2_Click;
            // 
            // button3
            // 
            button3.Location = new Point(741, 159);
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
            radioButton1.Location = new Point(123, 206);
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
            radioButton2.Location = new Point(305, 208);
            radioButton2.Name = "radioButton2";
            radioButton2.Size = new Size(188, 29);
            radioButton2.TabIndex = 12;
            radioButton2.TabStop = true;
            radioButton2.Text = "Use CSOD controls";
            radioButton2.UseVisualStyleBackColor = true;
            // 
            // label1
            // 
            label1.AutoSize = true;
            label1.Font = new Font("Segoe UI", 18F, FontStyle.Bold, GraphicsUnit.Point);
            label1.Location = new Point(576, 9);
            label1.Name = "label1";
            label1.Size = new Size(349, 48);
            label1.TabIndex = 13;
            label1.Text = "Migration Assistant";
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
            // AspxConverter
            // 
            AutoScaleDimensions = new SizeF(10F, 25F);
            AutoScaleMode = AutoScaleMode.Font;
            ClientSize = new Size(1370, 839);
            Controls.Add(pictureBox2);
            Controls.Add(pbLogoSonata);
            Controls.Add(label1);
            Controls.Add(radioButton2);
            Controls.Add(radioButton1);
            Controls.Add(button3);
            Controls.Add(button2);
            Controls.Add(button1);
            Controls.Add(lblOutput);
            Controls.Add(txtOutput);
            Controls.Add(txtLogs);
            Controls.Add(lblAspx);
            Controls.Add(lblFigma);
            Controls.Add(txtAspxPath);
            Controls.Add(btnConvert);
            Controls.Add(txtFigmaPath);
            Name = "AspxConverter";
            Text = "React migration tool";
            ((System.ComponentModel.ISupportInitialize)pbLogoSonata).EndInit();
            ((System.ComponentModel.ISupportInitialize)pictureBox2).EndInit();
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
        private Button button1;
        private Button button2;
        private Button button3;
        private RadioButton radioButton1;
        private RadioButton radioButton2;
        private Label label1;
        private PictureBox pbLogoSonata;
        private PictureBox pictureBox2;
        private OpenFileDialog openFileDialog1;
    }
}
