namespace Syriatel.NPM.Desktop
{
    partial class Form1
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
            this.label1 = new System.Windows.Forms.Label();
            this.fileName_box = new System.Windows.Forms.TextBox();
            this.importFile_btn = new System.Windows.Forms.Button();
            this.loadingImage = new System.Windows.Forms.PictureBox();
            this.pictureBox1 = new System.Windows.Forms.PictureBox();
            ((System.ComponentModel.ISupportInitialize)(this.loadingImage)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox1)).BeginInit();
            this.SuspendLayout();
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Font = new System.Drawing.Font("Microsoft Sans Serif", 10.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label1.Location = new System.Drawing.Point(12, 89);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(244, 17);
            this.label1.TabIndex = 1;
            this.label1.Text = "Enter process code in the box below :";
            // 
            // fileName_box
            // 
            this.fileName_box.Location = new System.Drawing.Point(15, 126);
            this.fileName_box.Name = "fileName_box";
            this.fileName_box.Size = new System.Drawing.Size(215, 20);
            this.fileName_box.TabIndex = 2;
            // 
            // importFile_btn
            // 
            this.importFile_btn.Location = new System.Drawing.Point(156, 167);
            this.importFile_btn.Name = "importFile_btn";
            this.importFile_btn.Size = new System.Drawing.Size(148, 25);
            this.importFile_btn.TabIndex = 3;
            this.importFile_btn.Text = "Import to Business Objects";
            this.importFile_btn.UseVisualStyleBackColor = true;
            this.importFile_btn.Click += new System.EventHandler(this.importFile_btn_Click);
            // 
            // loadingImage
            // 
            this.loadingImage.Image = global::Syriatel.NPM.Desktop.Properties.Resources.loading;
            this.loadingImage.Location = new System.Drawing.Point(254, 82);
            this.loadingImage.Name = "loadingImage";
            this.loadingImage.Size = new System.Drawing.Size(41, 40);
            this.loadingImage.SizeMode = System.Windows.Forms.PictureBoxSizeMode.StretchImage;
            this.loadingImage.TabIndex = 4;
            this.loadingImage.TabStop = false;
            // 
            // pictureBox1
            // 
            this.pictureBox1.Image = global::Syriatel.NPM.Desktop.Properties.Resources._2;
            this.pictureBox1.Location = new System.Drawing.Point(0, 0);
            this.pictureBox1.Name = "pictureBox1";
            this.pictureBox1.Size = new System.Drawing.Size(315, 68);
            this.pictureBox1.SizeMode = System.Windows.Forms.PictureBoxSizeMode.StretchImage;
            this.pictureBox1.TabIndex = 0;
            this.pictureBox1.TabStop = false;
            // 
            // Form1
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(316, 214);
            this.Controls.Add(this.loadingImage);
            this.Controls.Add(this.importFile_btn);
            this.Controls.Add(this.fileName_box);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.pictureBox1);
            this.Name = "Form1";
            this.Text = "NPM desktop";
            ((System.ComponentModel.ISupportInitialize)(this.loadingImage)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox1)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.PictureBox pictureBox1;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.TextBox fileName_box;
        private System.Windows.Forms.Button importFile_btn;
        private System.Windows.Forms.PictureBox loadingImage;
    }
}

