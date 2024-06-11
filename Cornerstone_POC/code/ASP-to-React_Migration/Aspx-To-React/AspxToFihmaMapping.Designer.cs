namespace Aspx_To_React
{
    partial class AspxToFihmaMapping
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
            comboBox1 = new ComboBox();
            comboBox2 = new ComboBox();
            label1 = new Label();
            label2 = new Label();
            btnAdd = new Button();
            btnSumit = new Button();
            SuspendLayout();
            // 
            // comboBox1
            // 
            comboBox1.FormattingEnabled = true;
            comboBox1.Location = new Point(71, 83);
            comboBox1.Name = "comboBox1";
            comboBox1.Size = new Size(182, 33);
            comboBox1.TabIndex = 0;
            // 
            // comboBox2
            // 
            comboBox2.FormattingEnabled = true;
            comboBox2.Location = new Point(292, 83);
            comboBox2.Name = "comboBox2";
            comboBox2.Size = new Size(182, 33);
            comboBox2.TabIndex = 1;
            // 
            // label1
            // 
            label1.AutoSize = true;
            label1.Location = new Point(152, 43);
            label1.Name = "label1";
            label1.Size = new Size(61, 25);
            label1.TabIndex = 2;
            label1.Text = "Figma";
            // 
            // label2
            // 
            label2.AutoSize = true;
            label2.Location = new Point(359, 43);
            label2.Name = "label2";
            label2.Size = new Size(51, 25);
            label2.TabIndex = 3;
            label2.Text = "Aspx";
            // 
            // btnAdd
            // 
            btnAdd.Location = new Point(524, 83);
            btnAdd.Name = "btnAdd";
            btnAdd.Size = new Size(112, 34);
            btnAdd.TabIndex = 4;
            btnAdd.Text = "Add";
            btnAdd.UseVisualStyleBackColor = true;
            // 
            // btnSumit
            // 
            btnSumit.Location = new Point(71, 219);
            btnSumit.Name = "btnSumit";
            btnSumit.Size = new Size(280, 34);
            btnSumit.TabIndex = 5;
            btnSumit.Text = "Submit";
            btnSumit.UseVisualStyleBackColor = true;
            // 
            // AspxToFihmaMapping
            // 
            AutoScaleDimensions = new SizeF(10F, 25F);
            AutoScaleMode = AutoScaleMode.Font;
            ClientSize = new Size(800, 450);
            Controls.Add(btnSumit);
            Controls.Add(btnAdd);
            Controls.Add(label2);
            Controls.Add(label1);
            Controls.Add(comboBox2);
            Controls.Add(comboBox1);
            Name = "AspxToFihmaMapping";
            Text = "AspxToFihmaMapping";
            ResumeLayout(false);
            PerformLayout();
        }

        #endregion

        private ComboBox comboBox1;
        private ComboBox comboBox2;
        private Label label1;
        private Label label2;
        private Button btnAdd;
        private Button btnSumit;
    }
}