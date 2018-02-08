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
            this.btnBuild = new System.Windows.Forms.Button();
            this.label6 = new System.Windows.Forms.Label();
            this.button3 = new System.Windows.Forms.Button();
            this.label5 = new System.Windows.Forms.Label();
            this.button4 = new System.Windows.Forms.Button();
            this.txtOtherQrSize = new System.Windows.Forms.TextBox();
            this.txtOtherQr = new System.Windows.Forms.TextBox();
            this.btnSaveToFile = new System.Windows.Forms.Button();
            this.label4 = new System.Windows.Forms.Label();
            this.lstOtherError = new System.Windows.Forms.ComboBox();
            this.pictureBox1 = new System.Windows.Forms.PictureBox();
            this.pictureBox2 = new System.Windows.Forms.PictureBox();
            this.btnRead = new System.Windows.Forms.Button();
            this.txtRet = new System.Windows.Forms.TextBox();
            this.label1 = new System.Windows.Forms.Label();
            this.btnParseUrl = new System.Windows.Forms.Button();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox1)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox2)).BeginInit();
            this.SuspendLayout();
            // 
            // label7
            // 
            this.label7.AutoSize = true;
            this.label7.ForeColor = System.Drawing.Color.Red;
            this.label7.Location = new System.Drawing.Point(198, 37);
            this.label7.Name = "label7";
            this.label7.Size = new System.Drawing.Size(191, 36);
            this.label7.TabIndex = 0;
            this.label7.Text = "内容请勿超过152个英文或76个汉字\r\n\r\n插入图片会自动缩放为二维码的1/5";
            // 
            // btnBuild
            // 
            this.btnBuild.Location = new System.Drawing.Point(12, 79);
            this.btnBuild.Name = "btnBuild";
            this.btnBuild.Size = new System.Drawing.Size(52, 41);
            this.btnBuild.TabIndex = 4;
            this.btnBuild.Text = "预览\r\n二维码";
            this.btnBuild.UseVisualStyleBackColor = true;
            this.btnBuild.Click += new System.EventHandler(this.btnBuild_Click);
            // 
            // label6
            // 
            this.label6.AutoSize = true;
            this.label6.Location = new System.Drawing.Point(85, 40);
            this.label6.Name = "label6";
            this.label6.Size = new System.Drawing.Size(59, 12);
            this.label6.TabIndex = 0;
            this.label6.Text = "纠错级别:";
            // 
            // button3
            // 
            this.button3.Location = new System.Drawing.Point(420, 93);
            this.button3.Name = "button3";
            this.button3.Size = new System.Drawing.Size(105, 22);
            this.button3.TabIndex = 9;
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
            this.button4.Location = new System.Drawing.Point(420, 51);
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
            this.txtOtherQr.Location = new System.Drawing.Point(79, 10);
            this.txtOtherQr.Name = "txtOtherQr";
            this.txtOtherQr.Size = new System.Drawing.Size(517, 21);
            this.txtOtherQr.TabIndex = 1;
            this.txtOtherQr.Text = "http://www.baidu.com/s?wd=91";
            // 
            // btnSaveToFile
            // 
            this.btnSaveToFile.Location = new System.Drawing.Point(70, 80);
            this.btnSaveToFile.Name = "btnSaveToFile";
            this.btnSaveToFile.Size = new System.Drawing.Size(52, 41);
            this.btnSaveToFile.TabIndex = 5;
            this.btnSaveToFile.Text = "保存\r\n二维码";
            this.btnSaveToFile.UseVisualStyleBackColor = true;
            this.btnSaveToFile.Click += new System.EventHandler(this.btnSaveToFile_Click);
            // 
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.Location = new System.Drawing.Point(10, 13);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(71, 12);
            this.label4.TabIndex = 0;
            this.label4.Text = "二维码内容:";
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
            this.pictureBox1.Location = new System.Drawing.Point(12, 149);
            this.pictureBox1.Name = "pictureBox1";
            this.pictureBox1.Size = new System.Drawing.Size(346, 317);
            this.pictureBox1.SizeMode = System.Windows.Forms.PictureBoxSizeMode.Zoom;
            this.pictureBox1.TabIndex = 0;
            this.pictureBox1.TabStop = false;
            // 
            // pictureBox2
            // 
            this.pictureBox2.Location = new System.Drawing.Point(531, 50);
            this.pictureBox2.Name = "pictureBox2";
            this.pictureBox2.Size = new System.Drawing.Size(65, 65);
            this.pictureBox2.SizeMode = System.Windows.Forms.PictureBoxSizeMode.StretchImage;
            this.pictureBox2.TabIndex = 5;
            this.pictureBox2.TabStop = false;
            // 
            // btnRead
            // 
            this.btnRead.Location = new System.Drawing.Point(267, 79);
            this.btnRead.Name = "btnRead";
            this.btnRead.Size = new System.Drawing.Size(81, 42);
            this.btnRead.TabIndex = 7;
            this.btnRead.Text = "解析二维码\r\n图片文件..";
            this.btnRead.UseVisualStyleBackColor = true;
            this.btnRead.Click += new System.EventHandler(this.btnRead_Click);
            // 
            // txtRet
            // 
            this.txtRet.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.txtRet.Location = new System.Drawing.Point(79, 124);
            this.txtRet.Name = "txtRet";
            this.txtRet.Size = new System.Drawing.Size(517, 21);
            this.txtRet.TabIndex = 10;
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(16, 127);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(59, 12);
            this.label1.TabIndex = 0;
            this.label1.Text = "解析结果:";
            // 
            // btnParseUrl
            // 
            this.btnParseUrl.Location = new System.Drawing.Point(169, 80);
            this.btnParseUrl.Name = "btnParseUrl";
            this.btnParseUrl.Size = new System.Drawing.Size(92, 42);
            this.btnParseUrl.TabIndex = 6;
            this.btnParseUrl.Text = "解析二维码URL";
            this.btnParseUrl.UseVisualStyleBackColor = true;
            this.btnParseUrl.Click += new System.EventHandler(this.btnParseUrl_Click);
            // 
            // QrCodeTool
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(609, 468);
            this.Controls.Add(this.btnParseUrl);
            this.Controls.Add(this.label7);
            this.Controls.Add(this.txtOtherQr);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.txtRet);
            this.Controls.Add(this.btnRead);
            this.Controls.Add(this.pictureBox2);
            this.Controls.Add(this.lstOtherError);
            this.Controls.Add(this.pictureBox1);
            this.Controls.Add(this.btnBuild);
            this.Controls.Add(this.label4);
            this.Controls.Add(this.label6);
            this.Controls.Add(this.btnSaveToFile);
            this.Controls.Add(this.button3);
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
        private System.Windows.Forms.Button btnBuild;
        private System.Windows.Forms.Label label6;
        private System.Windows.Forms.Button button3;
        private System.Windows.Forms.Label label5;
        private System.Windows.Forms.Button button4;
        private System.Windows.Forms.TextBox txtOtherQrSize;
        private System.Windows.Forms.TextBox txtOtherQr;
        private System.Windows.Forms.Button btnSaveToFile;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.ComboBox lstOtherError;
        private System.Windows.Forms.PictureBox pictureBox1;
        private System.Windows.Forms.PictureBox pictureBox2;
        private System.Windows.Forms.Button btnRead;
        private System.Windows.Forms.TextBox txtRet;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Button btnParseUrl;
    }
}