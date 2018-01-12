namespace Beinet.cn.Tools.Others
{
    partial class QrCodeTool
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
            this.label7 = new System.Windows.Forms.Label();
            this.button1 = new System.Windows.Forms.Button();
            this.label6 = new System.Windows.Forms.Label();
            this.button3 = new System.Windows.Forms.Button();
            this.label5 = new System.Windows.Forms.Label();
            this.button4 = new System.Windows.Forms.Button();
            this.txtOtherQrSize = new System.Windows.Forms.TextBox();
            this.txtOtherQr = new System.Windows.Forms.TextBox();
            this.button2 = new System.Windows.Forms.Button();
            this.label4 = new System.Windows.Forms.Label();
            this.lstOtherError = new System.Windows.Forms.ComboBox();
            this.pictureBox1 = new System.Windows.Forms.PictureBox();
            this.pictureBox2 = new System.Windows.Forms.PictureBox();
            this.btnRead = new System.Windows.Forms.Button();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox1)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox2)).BeginInit();
            this.SuspendLayout();
            // 
            // label7
            // 
            this.label7.AutoSize = true;
            this.label7.Location = new System.Drawing.Point(239, 36);
            this.label7.Name = "label7";
            this.label7.Size = new System.Drawing.Size(191, 48);
            this.label7.TabIndex = 1;
            this.label7.Text = "内容请勿超过152个英文或76个汉字\r\n\r\n插入图片会自动缩放\r\n为二维码的1/5";
            // 
            // button1
            // 
            this.button1.Location = new System.Drawing.Point(12, 73);
            this.button1.Name = "button1";
            this.button1.Size = new System.Drawing.Size(52, 41);
            this.button1.TabIndex = 4;
            this.button1.Text = "预览\r\n二维码";
            this.button1.UseVisualStyleBackColor = true;
            this.button1.Click += new System.EventHandler(this.button1_Click);
            // 
            // label6
            // 
            this.label6.AutoSize = true;
            this.label6.Location = new System.Drawing.Point(85, 40);
            this.label6.Name = "label6";
            this.label6.Size = new System.Drawing.Size(59, 12);
            this.label6.TabIndex = 1;
            this.label6.Text = "纠错级别:";
            // 
            // button3
            // 
            this.button3.Location = new System.Drawing.Point(246, 93);
            this.button3.Name = "button3";
            this.button3.Size = new System.Drawing.Size(105, 22);
            this.button3.TabIndex = 7;
            this.button3.Text = "插入图片...";
            this.button3.UseVisualStyleBackColor = true;
            this.button3.Click += new System.EventHandler(this.button3_Click);
            // 
            // label5
            // 
            this.label5.AutoSize = true;
            this.label5.Location = new System.Drawing.Point(8, 40);
            this.label5.Name = "label5";
            this.label5.Size = new System.Drawing.Size(35, 12);
            this.label5.TabIndex = 0;
            this.label5.Text = "大小:";
            // 
            // button4
            // 
            this.button4.Location = new System.Drawing.Point(357, 93);
            this.button4.Name = "button4";
            this.button4.Size = new System.Drawing.Size(105, 22);
            this.button4.TabIndex = 8;
            this.button4.Text = "清除插入的图片";
            this.button4.UseVisualStyleBackColor = true;
            this.button4.Click += new System.EventHandler(this.button4_Click);
            // 
            // txtOtherQrSize
            // 
            this.txtOtherQrSize.Location = new System.Drawing.Point(43, 37);
            this.txtOtherQrSize.Name = "txtOtherQrSize";
            this.txtOtherQrSize.Size = new System.Drawing.Size(37, 21);
            this.txtOtherQrSize.TabIndex = 2;
            this.txtOtherQrSize.Text = "4";
            // 
            // txtOtherQr
            // 
            this.txtOtherQr.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.txtOtherQr.Location = new System.Drawing.Point(45, 10);
            this.txtOtherQr.Name = "txtOtherQr";
            this.txtOtherQr.Size = new System.Drawing.Size(427, 21);
            this.txtOtherQr.TabIndex = 1;
            this.txtOtherQr.Text = "http://www.baidu.com/s?wd=91";
            // 
            // button2
            // 
            this.button2.Location = new System.Drawing.Point(70, 74);
            this.button2.Name = "button2";
            this.button2.Size = new System.Drawing.Size(52, 41);
            this.button2.TabIndex = 5;
            this.button2.Text = "保存\r\n二维码";
            this.button2.UseVisualStyleBackColor = true;
            this.button2.Click += new System.EventHandler(this.button2_Click);
            // 
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.Location = new System.Drawing.Point(10, 13);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(35, 12);
            this.label4.TabIndex = 0;
            this.label4.Text = "内容:";
            // 
            // lstOtherError
            // 
            this.lstOtherError.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.lstOtherError.FormattingEnabled = true;
            this.lstOtherError.Items.AddRange(new object[] {
            "最高",
            "较高",
            "较低",
            "最低"});
            this.lstOtherError.Location = new System.Drawing.Point(140, 36);
            this.lstOtherError.Name = "lstOtherError";
            this.lstOtherError.Size = new System.Drawing.Size(52, 20);
            this.lstOtherError.TabIndex = 3;
            // 
            // pictureBox1
            // 
            this.pictureBox1.Location = new System.Drawing.Point(10, 121);
            this.pictureBox1.Name = "pictureBox1";
            this.pictureBox1.Size = new System.Drawing.Size(537, 317);
            this.pictureBox1.SizeMode = System.Windows.Forms.PictureBoxSizeMode.Zoom;
            this.pictureBox1.TabIndex = 0;
            this.pictureBox1.TabStop = false;
            // 
            // pictureBox2
            // 
            this.pictureBox2.Location = new System.Drawing.Point(474, 50);
            this.pictureBox2.Name = "pictureBox2";
            this.pictureBox2.Size = new System.Drawing.Size(65, 65);
            this.pictureBox2.SizeMode = System.Windows.Forms.PictureBoxSizeMode.StretchImage;
            this.pictureBox2.TabIndex = 5;
            this.pictureBox2.TabStop = false;
            // 
            // btnRead
            // 
            this.btnRead.Location = new System.Drawing.Point(129, 62);
            this.btnRead.Name = "btnRead";
            this.btnRead.Size = new System.Drawing.Size(81, 53);
            this.btnRead.TabIndex = 6;
            this.btnRead.Text = "解析二维码\r\n图片...";
            this.btnRead.UseVisualStyleBackColor = true;
            this.btnRead.Click += new System.EventHandler(this.btnRead_Click);
            // 
            // QrCodeTool
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(609, 468);
            this.Controls.Add(this.btnRead);
            this.Controls.Add(this.pictureBox2);
            this.Controls.Add(this.lstOtherError);
            this.Controls.Add(this.pictureBox1);
            this.Controls.Add(this.label7);
            this.Controls.Add(this.button1);
            this.Controls.Add(this.label4);
            this.Controls.Add(this.label6);
            this.Controls.Add(this.button2);
            this.Controls.Add(this.button3);
            this.Controls.Add(this.txtOtherQr);
            this.Controls.Add(this.label5);
            this.Controls.Add(this.txtOtherQrSize);
            this.Controls.Add(this.button4);
            this.Name = "QrCodeTool";
            this.Text = "二维码工具";
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox1)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox2)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Label label7;
        private System.Windows.Forms.Button button1;
        private System.Windows.Forms.Label label6;
        private System.Windows.Forms.Button button3;
        private System.Windows.Forms.Label label5;
        private System.Windows.Forms.Button button4;
        private System.Windows.Forms.TextBox txtOtherQrSize;
        private System.Windows.Forms.TextBox txtOtherQr;
        private System.Windows.Forms.Button button2;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.ComboBox lstOtherError;
        private System.Windows.Forms.PictureBox pictureBox1;
        private System.Windows.Forms.PictureBox pictureBox2;
        private System.Windows.Forms.Button btnRead;
    }
}