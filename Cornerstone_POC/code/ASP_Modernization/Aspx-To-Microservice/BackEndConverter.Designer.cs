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
            label3 = new Label();
            rbNet6 = new RadioButton();
            rbNet8 = new RadioButton();
            groupBox1 = new GroupBox();
            button3 = new Button();
            txtOutputPath = new TextBox();
            label5 = new Label();
            folderBrowserDialog1 = new FolderBrowserDialog();
            cbMultiProject = new CheckBox();
            cbAddComments = new CheckBox();
            cbAddUnitTest = new CheckBox();
            cbAddSwagger = new CheckBox();
            cbAddAuthentication = new CheckBox();
            cbEntityFramework = new CheckBox();
            ((System.ComponentModel.ISupportInitialize)pictureBox2).BeginInit();
            ((System.ComponentModel.ISupportInitialize)pbLogoSonata).BeginInit();
            groupBox1.SuspendLayout();
            SuspendLayout();
            // 
            // lblSolutionPath
            // 
            lblSolutionPath.AutoSize = true;
            lblSolutionPath.Location = new Point(67, 135);
            lblSolutionPath.Name = "lblSolutionPath";
            lblSolutionPath.Size = new Size(123, 25);
            lblSolutionPath.TabIndex = 0;
            lblSolutionPath.Text = "Solution path:";
            // 
            // txtSolutionPath
            // 
            txtSolutionPath.Location = new Point(207, 133);
            txtSolutionPath.Name = "txtSolutionPath";
            txtSolutionPath.Size = new Size(621, 31);
            txtSolutionPath.TabIndex = 1;
            txtSolutionPath.Text = "C:\\Users\\m.abhishek.SONATA\\source\\repos\\Aspx_Demo\\Aspx_Demo.sln";
            // 
            // txtClassName
            // 
            txtClassName.Location = new Point(207, 237);
            txtClassName.Name = "txtClassName";
            txtClassName.Size = new Size(621, 31);
            txtClassName.TabIndex = 3;
            txtClassName.Text = "Assign";
            // 
            // label1
            // 
            label1.AutoSize = true;
            label1.Location = new Point(67, 239);
            label1.Name = "label1";
            label1.Size = new Size(108, 25);
            label1.TabIndex = 2;
            label1.Text = "Class Name:";
            // 
            // button1
            // 
            button1.Location = new Point(834, 130);
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
            button2.BackColor = Color.FromArgb(52, 152, 219);
            button2.Cursor = Cursors.Hand;
            button2.FlatAppearance.BorderSize = 0;
            button2.FlatStyle = FlatStyle.Flat;
            button2.Font = new Font("Arial", 9F, FontStyle.Bold);
            button2.ForeColor = Color.White;
            button2.Location = new Point(915, 345);
            button2.Name = "button2";
            button2.Size = new Size(200, 80);
            button2.TabIndex = 17;
            button2.Text = "Generate";
            button2.UseVisualStyleBackColor = false;
            button2.Click += button2_Click;
            // 
            // txtBELogs
            // 
            txtBELogs.Location = new Point(58, 464);
            txtBELogs.Margin = new Padding(0);
            txtBELogs.Multiline = true;
            txtBELogs.Name = "txtBELogs";
            txtBELogs.ScrollBars = ScrollBars.Vertical;
            txtBELogs.Size = new Size(1248, 284);
            txtBELogs.TabIndex = 6;
            // 
            // lblLogs
            // 
            lblLogs.AutoSize = true;
            lblLogs.Location = new Point(62, 436);
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
            label2.Location = new Point(530, 43);
            label2.Name = "label2";
            label2.Size = new Size(252, 48);
            label2.TabIndex = 18;
            label2.Text = "Legacy2Micro";
            // 
            // label3
            // 
            label3.AutoSize = true;
            label3.Location = new Point(67, 294);
            label3.Name = "label3";
            label3.Size = new Size(104, 25);
            label3.TabIndex = 19;
            label3.Text = "Framework:";
            // 
            // rbNet6
            // 
            rbNet6.AutoSize = true;
            rbNet6.Location = new Point(27, 14);
            rbNet6.Name = "rbNet6";
            rbNet6.Size = new Size(82, 29);
            rbNet6.TabIndex = 20;
            rbNet6.Text = ".NET6";
            rbNet6.UseVisualStyleBackColor = true;
            // 
            // rbNet8
            // 
            rbNet8.AutoSize = true;
            rbNet8.Checked = true;
            rbNet8.Location = new Point(188, 13);
            rbNet8.Name = "rbNet8";
            rbNet8.Size = new Size(82, 29);
            rbNet8.TabIndex = 21;
            rbNet8.TabStop = true;
            rbNet8.Text = ".NET8";
            rbNet8.UseVisualStyleBackColor = true;
            // 
            // groupBox1
            // 
            groupBox1.Controls.Add(rbNet8);
            groupBox1.Controls.Add(rbNet6);
            groupBox1.Location = new Point(207, 277);
            groupBox1.Name = "groupBox1";
            groupBox1.Size = new Size(621, 47);
            groupBox1.TabIndex = 25;
            groupBox1.TabStop = false;
            // 
            // button3
            // 
            button3.Location = new Point(834, 180);
            button3.Name = "button3";
            button3.Size = new Size(37, 34);
            button3.TabIndex = 29;
            button3.Text = "...";
            button3.UseVisualStyleBackColor = true;
            button3.Click += button3_Click;
            // 
            // txtOutputPath
            // 
            txtOutputPath.Location = new Point(207, 183);
            txtOutputPath.Name = "txtOutputPath";
            txtOutputPath.Size = new Size(621, 31);
            txtOutputPath.TabIndex = 28;
            txtOutputPath.Text = "D:\\Demo\\CSOD\\Projects";
            // 
            // label5
            // 
            label5.AutoSize = true;
            label5.Location = new Point(67, 185);
            label5.Name = "label5";
            label5.Size = new Size(114, 25);
            label5.TabIndex = 27;
            label5.Text = "Output path:";
            // 
            // cbMultiProject
            // 
            cbMultiProject.AutoSize = true;
            cbMultiProject.Location = new Point(67, 345);
            cbMultiProject.Name = "cbMultiProject";
            cbMultiProject.Size = new Size(161, 29);
            cbMultiProject.TabIndex = 30;
            cbMultiProject.Text = "Multiple Project";
            cbMultiProject.UseVisualStyleBackColor = true;
            // 
            // cbAddComments
            // 
            cbAddComments.AutoSize = true;
            cbAddComments.Location = new Point(254, 345);
            cbAddComments.Name = "cbAddComments";
            cbAddComments.Size = new Size(125, 29);
            cbAddComments.TabIndex = 31;
            cbAddComments.Text = "Comments";
            cbAddComments.UseVisualStyleBackColor = true;
            // 
            // cbAddUnitTest
            // 
            cbAddUnitTest.AutoSize = true;
            cbAddUnitTest.Location = new Point(759, 345);
            cbAddUnitTest.Name = "cbAddUnitTest";
            cbAddUnitTest.Size = new Size(105, 29);
            cbAddUnitTest.TabIndex = 32;
            cbAddUnitTest.Text = "Unit Test";
            cbAddUnitTest.UseVisualStyleBackColor = true;
            // 
            // cbAddSwagger
            // 
            cbAddSwagger.AutoSize = true;
            cbAddSwagger.Location = new Point(409, 345);
            cbAddSwagger.Name = "cbAddSwagger";
            cbAddSwagger.Size = new Size(107, 29);
            cbAddSwagger.TabIndex = 34;
            cbAddSwagger.Text = "Swagger";
            cbAddSwagger.UseVisualStyleBackColor = true;
            // 
            // cbAddAuthentication
            // 
            cbAddAuthentication.AutoSize = true;
            cbAddAuthentication.Location = new Point(67, 385);
            cbAddAuthentication.Name = "cbAddAuthentication";
            cbAddAuthentication.Size = new Size(153, 29);
            cbAddAuthentication.TabIndex = 35;
            cbAddAuthentication.Text = "Authentication";
            cbAddAuthentication.UseVisualStyleBackColor = true;
            // 
            // cbEntityFramework
            // 
            cbEntityFramework.AutoSize = true;
            cbEntityFramework.Location = new Point(544, 345);
            cbEntityFramework.Name = "cbEntityFramework";
            cbEntityFramework.Size = new Size(175, 29);
            cbEntityFramework.TabIndex = 36;
            cbEntityFramework.Text = "Entity Framework";
            cbEntityFramework.UseVisualStyleBackColor = true;
            // 
            // BackEndConverter
            // 
            AutoScaleDimensions = new SizeF(10F, 25F);
            AutoScaleMode = AutoScaleMode.Font;
            ClientSize = new Size(1370, 839);
            Controls.Add(cbEntityFramework);
            Controls.Add(cbAddAuthentication);
            Controls.Add(cbAddSwagger);
            Controls.Add(cbAddUnitTest);
            Controls.Add(cbAddComments);
            Controls.Add(cbMultiProject);
            Controls.Add(button3);
            Controls.Add(txtOutputPath);
            Controls.Add(label5);
            Controls.Add(groupBox1);
            Controls.Add(label3);
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
            Text = "Legacy2Micro Assistant";
            ((System.ComponentModel.ISupportInitialize)pictureBox2).EndInit();
            ((System.ComponentModel.ISupportInitialize)pbLogoSonata).EndInit();
            groupBox1.ResumeLayout(false);
            groupBox1.PerformLayout();
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
        private Label label3;
        private RadioButton rbNet6;
        private RadioButton rbNet8;
        private GroupBox groupBox1;
        private Button button3;
        private TextBox txtOutputPath;
        private Label label5;
        private FolderBrowserDialog folderBrowserDialog1;
        private CheckBox cbMultiProject;
        private CheckBox cbAddComments;
        private CheckBox cbAddUnitTest;
        private CheckBox cbAddSwagger;
        private CheckBox cbAddAuthentication;
        private CheckBox cbEntityFramework;
    }
}
