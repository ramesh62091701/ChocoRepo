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
            pictureBox1 = new PictureBox();
            pictureBox2 = new PictureBox();
            label1 = new Label();
            label2 = new Label();
            ((System.ComponentModel.ISupportInitialize)pictureBox1).BeginInit();
            ((System.ComponentModel.ISupportInitialize)pictureBox2).BeginInit();
            SuspendLayout();
            // 
            // lblTitle
            // 
            lblTitle.AutoSize = true;
            lblTitle.Font = new Font("Segoe UI", 18F, FontStyle.Bold, GraphicsUnit.Point, 0);
            lblTitle.Location = new Point(201, 40);
            lblTitle.Name = "lblTitle";
            lblTitle.Size = new Size(349, 48);
            lblTitle.TabIndex = 0;
            lblTitle.Text = "Modernization Tool";
            // 
            // pictureBox1
            // 
            pictureBox1.Cursor = Cursors.Hand;
            pictureBox1.ImageLocation = "images/ux.png";
            pictureBox1.Location = new Point(98, 135);
            pictureBox1.Name = "pictureBox1";
            pictureBox1.Size = new Size(184, 152);
            pictureBox1.SizeMode = PictureBoxSizeMode.StretchImage;
            pictureBox1.TabIndex = 3;
            pictureBox1.TabStop = false;
            pictureBox1.Click += pictureBox1_Click;
            // 
            // pictureBox2
            // 
            pictureBox2.Cursor = Cursors.Hand;
            pictureBox2.ImageLocation = "images/backend.png";
            pictureBox2.Location = new Point(472, 135);
            pictureBox2.Name = "pictureBox2";
            pictureBox2.Size = new Size(178, 152);
            pictureBox2.SizeMode = PictureBoxSizeMode.StretchImage;
            pictureBox2.TabIndex = 4;
            pictureBox2.TabStop = false;
            pictureBox2.Click += pictureBox2_Click;
            // 
            // label1
            // 
            label1.AutoSize = true;
            label1.Location = new Point(132, 300);
            label1.Name = "label1";
            label1.Size = new Size(111, 25);
            label1.TabIndex = 5;
            label1.Text = "UI Migration";
            // 
            // label2
            // 
            label2.AutoSize = true;
            label2.Location = new Point(484, 300);
            label2.Name = "label2";
            label2.Size = new Size(160, 25);
            label2.TabIndex = 6;
            label2.Text = "Backend Migration";
            // 
            // Main
            // 
            AutoScaleDimensions = new SizeF(10F, 25F);
            AutoScaleMode = AutoScaleMode.Font;
            ClientSize = new Size(800, 450);
            Controls.Add(label2);
            Controls.Add(label1);
            Controls.Add(pictureBox2);
            Controls.Add(pictureBox1);
            Controls.Add(lblTitle);
            Name = "Main";
            Text = "Migration Tool";
            ((System.ComponentModel.ISupportInitialize)pictureBox1).EndInit();
            ((System.ComponentModel.ISupportInitialize)pictureBox2).EndInit();
            ResumeLayout(false);
            PerformLayout();
        }

        #endregion

        private Label lblTitle;
        private PictureBox pictureBox1;
        private PictureBox pictureBox2;
        private Label label1;
        private Label label2;
    }
}
