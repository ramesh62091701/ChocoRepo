namespace Aspx_To_React
{
    partial class AspxToFigmaMapping
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
            cmbFigma1 = new ComboBox();
            cmbAspx1 = new ComboBox();
            label1 = new Label();
            label2 = new Label();
            btnAdd = new Button();
            btnSumit = new Button();
            label3 = new Label();
            SuspendLayout();
            // 
            // cmbFigma1
            // 
            cmbFigma1.FormattingEnabled = true;
            cmbFigma1.Location = new Point(27, 175);
            cmbFigma1.Name = "cmbFigma1";
            cmbFigma1.Size = new Size(287, 33);
            cmbFigma1.TabIndex = 0;
            // 
            // cmbAspx1
            // 
            cmbAspx1.DropDownWidth = 244;
            cmbAspx1.FormattingEnabled = true;
            cmbAspx1.Location = new Point(344, 175);
            cmbAspx1.Name = "cmbAspx1";
            cmbAspx1.Size = new Size(311, 33);
            cmbAspx1.TabIndex = 1;
            // 
            // label1
            // 
            label1.AutoSize = true;
            label1.Location = new Point(131, 141);
            label1.Name = "label1";
            label1.Size = new Size(61, 25);
            label1.TabIndex = 2;
            label1.Text = "Figma";
            // 
            // label2
            // 
            label2.AutoSize = true;
            label2.Location = new Point(458, 141);
            label2.Name = "label2";
            label2.Size = new Size(51, 25);
            label2.TabIndex = 3;
            label2.Text = "Aspx";
            // 
            // btnAdd
            // 
            btnAdd.Location = new Point(110, 66);
            btnAdd.Name = "btnAdd";
            btnAdd.Size = new Size(200, 54);
            btnAdd.TabIndex = 4;
            btnAdd.Text = "Add";
            btnAdd.UseVisualStyleBackColor = true;
            btnAdd.Click += btnAdd_Click;
            // 
            // btnSumit
            // 
            btnSumit.BackColor = Color.FromArgb(255, 128, 0);
            btnSumit.Location = new Point(364, 64);
            btnSumit.Name = "btnSumit";
            btnSumit.Size = new Size(200, 54);
            btnSumit.TabIndex = 5;
            btnSumit.Text = "Close";
            btnSumit.UseVisualStyleBackColor = false;
            btnSumit.Click += btnSumit_Click;
            // 
            // label3
            // 
            label3.AutoSize = true;
            label3.Font = new Font("Segoe UI", 14F, FontStyle.Bold, GraphicsUnit.Point);
            label3.Location = new Point(139, 9);
            label3.Name = "label3";
            label3.Size = new Size(374, 38);
            label3.TabIndex = 6;
            label3.Text = "Map Figma to Aspx control";
            // 
            // AspxToFigmaMapping
            // 
            AutoScaleDimensions = new SizeF(10F, 25F);
            AutoScaleMode = AutoScaleMode.Font;
            ClientSize = new Size(741, 655);
            Controls.Add(label3);
            Controls.Add(btnSumit);
            Controls.Add(btnAdd);
            Controls.Add(label2);
            Controls.Add(label1);
            Controls.Add(cmbAspx1);
            Controls.Add(cmbFigma1);
            Name = "AspxToFigmaMapping";
            Text = "FigmaToAspxMapping";
            ResumeLayout(false);
            PerformLayout();
        }

        #endregion

        private ComboBox cmbFigma1;
        private ComboBox cmbAspx1;
        private Label label1;
        private Label label2;
        private Button btnAdd;
        private Button btnSumit;
        private Label label3;
    }
}