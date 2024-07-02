namespace Main_App
{
    partial class Main
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
            lblTitle = new Label();
            btnUiMigration = new Button();
            btnBackendMigration = new Button();
            SuspendLayout();
            // 
            // lblTitle
            // 
            lblTitle.AutoSize = true;
            lblTitle.Font = new Font("Segoe UI", 18F, FontStyle.Bold, GraphicsUnit.Point, 0);
            lblTitle.Location = new Point(261, 27);
            lblTitle.Name = "lblTitle";
            lblTitle.Size = new Size(269, 48);
            lblTitle.TabIndex = 0;
            lblTitle.Text = "Migration Tool";
            // 
            // btnUiMigration
            // 
            btnUiMigration.Location = new Point(172, 185);
            btnUiMigration.Name = "btnUiMigration";
            btnUiMigration.Size = new Size(169, 34);
            btnUiMigration.TabIndex = 1;
            btnUiMigration.Text = "UI Migration";
            btnUiMigration.UseVisualStyleBackColor = true;
            btnUiMigration.Click += btnUiMigration_Click;
            // 
            // btnBackendMigration
            // 
            btnBackendMigration.Location = new Point(390, 185);
            btnBackendMigration.Name = "btnBackendMigration";
            btnBackendMigration.Size = new Size(178, 34);
            btnBackendMigration.TabIndex = 2;
            btnBackendMigration.Text = "Backend Migration";
            btnBackendMigration.UseVisualStyleBackColor = true;
            btnBackendMigration.Click += btnBackendMigration_Click;
            // 
            // Main
            // 
            AutoScaleDimensions = new SizeF(10F, 25F);
            AutoScaleMode = AutoScaleMode.Font;
            ClientSize = new Size(800, 450);
            Controls.Add(btnBackendMigration);
            Controls.Add(btnUiMigration);
            Controls.Add(lblTitle);
            Name = "Main";
            Text = "Migration Tool";
            ResumeLayout(false);
            PerformLayout();
        }

        #endregion

        private Label lblTitle;
        private Button btnUiMigration;
        private Button btnBackendMigration;
    }
}
