namespace Aspx_To_Microservice
{
    partial class BackEndConverter
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
            lblSolutionPath = new Label();
            txtSolutionPath = new TextBox();
            txtClassName = new TextBox();
            label1 = new Label();
            button1 = new Button();
            openFileDialog1 = new OpenFileDialog();
            button2 = new Button();
            txtBELogs = new TextBox();
            lblLogs = new Label();
            pictureBox2 = new PictureBox();
            pbLogoSonata = new PictureBox();
            label2 = new Label();
            ((System.ComponentModel.ISupportInitialize)pictureBox2).BeginInit();
            ((System.ComponentModel.ISupportInitialize)pbLogoSonata).BeginInit();
            SuspendLayout();
            // 
            // lblSolutionPath
            // 
            lblSolutionPath.AutoSize = true;
            lblSolutionPath.Location = new Point(67, 150);
            lblSolutionPath.Name = "lblSolutionPath";
            lblSolutionPath.Size = new Size(123, 25);
            lblSolutionPath.TabIndex = 0;
            lblSolutionPath.Text = "Solution path:";
            // 
            // txtSolutionPath
            // 
            txtSolutionPath.Location = new Point(207, 144);
            txtSolutionPath.Name = "txtSolutionPath";
            txtSolutionPath.Size = new Size(621, 31);
            txtSolutionPath.TabIndex = 1;
            txtSolutionPath.Text = "C:\\Users\\m.abhishek.SONATA\\source\\repos\\Aspx_Demo\\Aspx_Demo.sln";
            // 
            // txtClassName
            // 
            txtClassName.Location = new Point(207, 193);
            txtClassName.Name = "txtClassName";
            txtClassName.Size = new Size(621, 31);
            txtClassName.TabIndex = 3;
            txtClassName.Text = "Assign";
            // 
            // label1
            // 
            label1.AutoSize = true;
            label1.Location = new Point(67, 199);
            label1.Name = "label1";
            label1.Size = new Size(108, 25);
            label1.TabIndex = 2;
            label1.Text = "Class Name:";
            // 
            // button1
            // 
            button1.Location = new Point(834, 141);
            button1.Name = "button1";
            button1.Size = new Size(37, 34);
            button1.TabIndex = 4;
            button1.Text = "...";
            button1.UseVisualStyleBackColor = true;
            button1.Click += button1_Click;
            // 
            // openFileDialog1
            // 
            openFileDialog1.FileName = "openFileDialog1";
            // 
            // button2
            // 
            button2.Location = new Point(63, 247);
            button2.Name = "button2";
            button2.Size = new Size(141, 34);
            button2.TabIndex = 5;
            button2.Text = "Get Methods";
            button2.UseVisualStyleBackColor = true;
            button2.Click += button2_Click;
            // 
            // txtBELogs
            // 
            txtBELogs.Location = new Point(58, 334);
            txtBELogs.Margin = new Padding(0);
            txtBELogs.Multiline = true;
            txtBELogs.Name = "txtBELogs";
            txtBELogs.ScrollBars = ScrollBars.Vertical;
            txtBELogs.Size = new Size(1248, 453);
            txtBELogs.TabIndex = 6;
            // 
            // lblLogs
            // 
            lblLogs.AutoSize = true;
            lblLogs.Location = new Point(58, 306);
            lblLogs.Name = "lblLogs";
            lblLogs.Size = new Size(59, 25);
            lblLogs.TabIndex = 7;
            lblLogs.Text = "Logs: ";
            // 
            // pictureBox2
            // 
            pictureBox2.ImageLocation = "images/lightning.png";
            pictureBox2.Location = new Point(12, 12);
            pictureBox2.Name = "pictureBox2";
            pictureBox2.Size = new Size(300, 50);
            pictureBox2.TabIndex = 16;
            pictureBox2.TabStop = false;
            // 
            // pbLogoSonata
            // 
            pbLogoSonata.ImageLocation = "images/sonata.png";
            pbLogoSonata.Location = new Point(1148, 9);
            pbLogoSonata.Margin = new Padding(0);
            pbLogoSonata.Name = "pbLogoSonata";
            pbLogoSonata.Size = new Size(213, 149);
            pbLogoSonata.SizeMode = PictureBoxSizeMode.Zoom;
            pbLogoSonata.TabIndex = 17;
            pbLogoSonata.TabStop = false;
            // 
            // label2
            // 
            label2.AutoSize = true;
            label2.Font = new Font("Segoe UI", 18F, FontStyle.Bold, GraphicsUnit.Point, 0);
            label2.Location = new Point(465, 43);
            label2.Name = "label2";
            label2.Size = new Size(379, 48);
            label2.TabIndex = 18;
            label2.Text = "ASPX to Microservice";
            // 
            // BackEndConverter
            // 
            AutoScaleDimensions = new SizeF(10F, 25F);
            AutoScaleMode = AutoScaleMode.Font;
            ClientSize = new Size(1370, 839);
            Controls.Add(label2);
            Controls.Add(pbLogoSonata);
            Controls.Add(pictureBox2);
            Controls.Add(lblLogs);
            Controls.Add(txtBELogs);
            Controls.Add(button2);
            Controls.Add(button1);
            Controls.Add(txtClassName);
            Controls.Add(label1);
            Controls.Add(txtSolutionPath);
            Controls.Add(lblSolutionPath);
            Name = "BackEndConverter";
            Text = "Aspx to Microservice";
            ((System.ComponentModel.ISupportInitialize)pictureBox2).EndInit();
            ((System.ComponentModel.ISupportInitialize)pbLogoSonata).EndInit();
            ResumeLayout(false);
            PerformLayout();
        }

        #endregion

        private Label lblSolutionPath;
        private TextBox txtSolutionPath;
        private TextBox txtClassName;
        private Label label1;
        private Button button1;
        private OpenFileDialog openFileDialog1;
        private Button button2;
        private TextBox txtBELogs;
        private Label lblLogs;
        private PictureBox pictureBox2;
        private PictureBox pbLogoSonata;
        private Label label2;
    }
}
