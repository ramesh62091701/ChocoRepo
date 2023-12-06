namespace BackendServiceGenerator
{
    partial class BackendServiceGenerator
    {
        /// <summary>
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Clean up any resources being used.
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
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            lblChoose = new Label();
            txtFilePath = new TextBox();
            btnBrowse = new Button();
            treeView1 = new TreeView();
            btnShowFiles = new Button();
            SuspendLayout();
            // 
            // lblChoose
            // 
            lblChoose.AutoSize = true;
            lblChoose.Location = new Point(12, 42);
            lblChoose.Name = "lblChoose";
            lblChoose.Size = new Size(115, 15);
            lblChoose.TabIndex = 0;
            lblChoose.Text = "Choose Solution File";
            // 
            // txtFilePath
            // 
            txtFilePath.Location = new Point(133, 39);
            txtFilePath.Name = "txtFilePath";
            txtFilePath.Size = new Size(375, 23);
            txtFilePath.TabIndex = 1;
            // 
            // btnBrowse
            // 
            btnBrowse.Location = new Point(514, 39);
            btnBrowse.Name = "btnBrowse";
            btnBrowse.Size = new Size(75, 23);
            btnBrowse.TabIndex = 2;
            btnBrowse.Text = "Browse";
            btnBrowse.UseVisualStyleBackColor = true;
            btnBrowse.Click += btnBrowse_Click;
            // 
            // treeView1
            // 
            treeView1.Location = new Point(12, 135);
            treeView1.Name = "treeView1";
            treeView1.Size = new Size(496, 286);
            treeView1.TabIndex = 3;
            // 
            // btnShowFiles
            // 
            btnShowFiles.Location = new Point(269, 82);
            btnShowFiles.Name = "btnShowFiles";
            btnShowFiles.Size = new Size(75, 23);
            btnShowFiles.TabIndex = 4;
            btnShowFiles.Text = "Show Files";
            btnShowFiles.UseVisualStyleBackColor = true;
            btnShowFiles.Click += btnShowFiles_Click;
            // 
            // BackendServiceGenerator
            // 
            AutoScaleDimensions = new SizeF(7F, 15F);
            AutoScaleMode = AutoScaleMode.Font;
            ClientSize = new Size(720, 450);
            Controls.Add(btnShowFiles);
            Controls.Add(treeView1);
            Controls.Add(btnBrowse);
            Controls.Add(txtFilePath);
            Controls.Add(lblChoose);
            Name = "BackendServiceGenerator";
            Text = "BackendServiceGenerator";
            ResumeLayout(false);
            PerformLayout();
        }

        #endregion

        private Label lblChoose;
        private TextBox txtFilePath;
        private Button btnBrowse;
        private TreeView treeView1;
        private Button btnShowFiles;
    }
}