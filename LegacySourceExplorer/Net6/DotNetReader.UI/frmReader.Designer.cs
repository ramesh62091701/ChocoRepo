using System.Drawing;
using System.Windows.Forms;

namespace DotNetAppReader
{
    partial class frmReader
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
            btnSelectSolutionFile = new Button();
            openFileDialog1 = new OpenFileDialog();
            txtFileName = new TextBox();
            lblFileName = new Label();
            btnRead = new Button();
            treeView1 = new TreeView();
            SuspendLayout();
            // 
            // btnSelectSolutionFile
            // 
            btnSelectSolutionFile.Location = new Point(464, 40);
            btnSelectSolutionFile.Name = "btnSelectSolutionFile";
            btnSelectSolutionFile.Size = new Size(75, 23);
            btnSelectSolutionFile.TabIndex = 0;
            btnSelectSolutionFile.Text = "Browse";
            btnSelectSolutionFile.UseVisualStyleBackColor = true;
            btnSelectSolutionFile.Click += btnSelectSolutionFile_Click;
            // 
            // txtFileName
            // 
            txtFileName.Location = new Point(12, 40);
            txtFileName.Name = "txtFileName";
            txtFileName.Size = new Size(446, 23);
            txtFileName.TabIndex = 1;
            txtFileName.Text = "D:\\Code\\CleanArchitecture.WebApi\\CleanArchitecture.WebApi.sln";
            // 
            // lblFileName
            // 
            lblFileName.AutoSize = true;
            lblFileName.Location = new Point(13, 23);
            lblFileName.Name = "lblFileName";
            lblFileName.Size = new Size(143, 15);
            lblFileName.TabIndex = 2;
            lblFileName.Text = "Visual Studio Solution File";
            // 
            // btnRead
            // 
            btnRead.Location = new Point(13, 69);
            btnRead.Name = "btnRead";
            btnRead.Size = new Size(75, 23);
            btnRead.TabIndex = 3;
            btnRead.Text = "Read";
            btnRead.UseVisualStyleBackColor = true;
            btnRead.Click += btnRead_Click;
            // 
            // treeView1
            // 
            treeView1.Location = new Point(13, 110);
            treeView1.Name = "treeView1";
            treeView1.Size = new Size(301, 299);
            treeView1.TabIndex = 4;
            // 
            // frmDotNetAppReader
            // 
            AutoScaleDimensions = new SizeF(7F, 15F);
            AutoScaleMode = AutoScaleMode.Font;
            ClientSize = new Size(800, 450);
            Controls.Add(treeView1);
            Controls.Add(btnRead);
            Controls.Add(lblFileName);
            Controls.Add(txtFileName);
            Controls.Add(btnSelectSolutionFile);
            Name = "frmDotNetAppReader";
            Text = ".Net App Reader";
            ResumeLayout(false);
            PerformLayout();
        }

        #endregion

        private Button btnSelectSolutionFile;
        private OpenFileDialog openFileDialog1;
        private TextBox txtFileName;
        private Label lblFileName;
        private Button btnRead;
        private TreeView treeView1;
    }
}