namespace Beinet.cn.Tools.Others
{
    partial class OtherTools
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
            this.tabPage2 = new System.Windows.Forms.TabPage();
            this.pictureBox2 = new System.Windows.Forms.PictureBox();
            this.pictureBox1 = new System.Windows.Forms.PictureBox();
            this.lstOtherError = new System.Windows.Forms.ComboBox();
            this.label4 = new System.Windows.Forms.Label();
            this.button2 = new System.Windows.Forms.Button();
            this.txtOtherQr = new System.Windows.Forms.TextBox();
            this.txtOtherQrSize = new System.Windows.Forms.TextBox();
            this.button4 = new System.Windows.Forms.Button();
            this.label5 = new System.Windows.Forms.Label();
            this.button3 = new System.Windows.Forms.Button();
            this.label6 = new System.Windows.Forms.Label();
            this.button1 = new System.Windows.Forms.Button();
            this.label7 = new System.Windows.Forms.Label();
            this.tabPage1 = new System.Windows.Forms.TabPage();
            this.labTh = new System.Windows.Forms.Label();
            this.label8 = new System.Windows.Forms.Label();
            this.tabControl2 = new System.Windows.Forms.TabControl();
            this.tabPage3 = new System.Windows.Forms.TabPage();
            this.txtResult = new System.Windows.Forms.TextBox();
            this.tabPage4 = new System.Windows.Forms.TabPage();
            this.txtErr = new System.Windows.Forms.TextBox();
            this.chkMac = new System.Windows.Forms.CheckBox();
            this.chkPortAll = new System.Windows.Forms.CheckBox();
            this.chkNormalPort = new System.Windows.Forms.CheckBox();
            this.label1 = new System.Windows.Forms.Label();
            this.txtIp1 = new System.Windows.Forms.TextBox();
            this.txtIp2 = new System.Windows.Forms.TextBox();
            this.label2 = new System.Windows.Forms.Label();
            this.labStatus = new System.Windows.Forms.Label();
            this.btnPort = new System.Windows.Forms.Button();
            this.btnSearch = new System.Windows.Forms.Button();
            this.tabControl1 = new System.Windows.Forms.TabControl();
            this.btnRead = new System.Windows.Forms.Button();
            this.tabPage2.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox2)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox1)).BeginInit();
            this.tabPage1.SuspendLayout();
            this.tabControl2.SuspendLayout();
            this.tabPage3.SuspendLayout();
            this.tabPage4.SuspendLayout();
            this.tabControl1.SuspendLayout();
            this.SuspendLayout();
            // 
            // tabPage2
            // 
            this.tabPage2.Controls.Add(this.btnRead);
            this.tabPage2.Controls.Add(this.pictureBox2);
            this.tabPage2.Controls.Add(this.pictureBox1);
            this.tabPage2.Controls.Add(this.lstOtherError);
            this.tabPage2.Controls.Add(this.label4);
            this.tabPage2.Controls.Add(this.button2);
            this.tabPage2.Controls.Add(this.txtOtherQr);
            this.tabPage2.Controls.Add(this.txtOtherQrSize);
            this.tabPage2.Controls.Add(this.button4);
            this.tabPage2.Controls.Add(this.label5);
            this.tabPage2.Controls.Add(this.button3);
            this.tabPage2.Controls.Add(this.label6);
            this.tabPage2.Controls.Add(this.button1);
            this.tabPage2.Controls.Add(this.label7);
            this.tabPage2.Location = new System.Drawing.Point(4, 22);
            this.tabPage2.Name = "tabPage2";
            this.tabPage2.Padding = new System.Windows.Forms.Padding(3);
            this.tabPage2.Size = new System.Drawing.Size(601, 442);
            this.tabPage2.TabIndex = 1;
            this.tabPage2.Text = "二维码生成工具";
            this.tabPage2.UseVisualStyleBackColor = true;
            // 
            // pictureBox2
            // 
            this.pictureBox2.Location = new System.Drawing.Point(470, 47);
            this.pictureBox2.Name = "pictureBox2";
            this.pictureBox2.Size = new System.Drawing.Size(65, 65);
            this.pictureBox2.SizeMode = System.Windows.Forms.PictureBoxSizeMode.StretchImage;
            this.pictureBox2.TabIndex = 5;
            this.pictureBox2.TabStop = false;
            // 
            // pictureBox1
            // 
            this.pictureBox1.Location = new System.Drawing.Point(6, 118);
            this.pictureBox1.Name = "pictureBox1";
            this.pictureBox1.Size = new System.Drawing.Size(537, 317);
            this.pictureBox1.SizeMode = System.Windows.Forms.PictureBoxSizeMode.Zoom;
            this.pictureBox1.TabIndex = 0;
            this.pictureBox1.TabStop = false;
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
            this.lstOtherError.Location = new System.Drawing.Point(136, 33);
            this.lstOtherError.Name = "lstOtherError";
            this.lstOtherError.Size = new System.Drawing.Size(52, 20);
            this.lstOtherError.TabIndex = 3;
            // 
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.Location = new System.Drawing.Point(6, 10);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(35, 12);
            this.label4.TabIndex = 1;
            this.label4.Text = "内容:";
            // 
            // button2
            // 
            this.button2.Location = new System.Drawing.Point(66, 71);
            this.button2.Name = "button2";
            this.button2.Size = new System.Drawing.Size(52, 41);
            this.button2.TabIndex = 5;
            this.button2.Text = "保存\r\n二维码";
            this.button2.UseVisualStyleBackColor = true;
            this.button2.Click += new System.EventHandler(this.button2_Click);
            // 
            // txtOtherQr
            // 
            this.txtOtherQr.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.txtOtherQr.Location = new System.Drawing.Point(40, 7);
            this.txtOtherQr.Name = "txtOtherQr";
            this.txtOtherQr.Size = new System.Drawing.Size(388, 21);
            this.txtOtherQr.TabIndex = 2;
            this.txtOtherQr.Text = "http://www.baidu.com/s?wd=91";
            // 
            // txtOtherQrSize
            // 
            this.txtOtherQrSize.Location = new System.Drawing.Point(38, 34);
            this.txtOtherQrSize.Name = "txtOtherQrSize";
            this.txtOtherQrSize.Size = new System.Drawing.Size(37, 21);
            this.txtOtherQrSize.TabIndex = 2;
            this.txtOtherQrSize.Text = "4";
            // 
            // button4
            // 
            this.button4.Location = new System.Drawing.Point(353, 90);
            this.button4.Name = "button4";
            this.button4.Size = new System.Drawing.Size(105, 22);
            this.button4.TabIndex = 7;
            this.button4.Text = "清除插入的图片";
            this.button4.UseVisualStyleBackColor = true;
            this.button4.Click += new System.EventHandler(this.button4_Click);
            // 
            // label5
            // 
            this.label5.AutoSize = true;
            this.label5.Location = new System.Drawing.Point(4, 37);
            this.label5.Name = "label5";
            this.label5.Size = new System.Drawing.Size(35, 12);
            this.label5.TabIndex = 1;
            this.label5.Text = "大小:";
            // 
            // button3
            // 
            this.button3.Location = new System.Drawing.Point(242, 90);
            this.button3.Name = "button3";
            this.button3.Size = new System.Drawing.Size(105, 22);
            this.button3.TabIndex = 6;
            this.button3.Text = "插入图片...";
            this.button3.UseVisualStyleBackColor = true;
            this.button3.Click += new System.EventHandler(this.button3_Click);
            // 
            // label6
            // 
            this.label6.AutoSize = true;
            this.label6.Location = new System.Drawing.Point(81, 37);
            this.label6.Name = "label6";
            this.label6.Size = new System.Drawing.Size(59, 12);
            this.label6.TabIndex = 1;
            this.label6.Text = "纠错级别:";
            // 
            // button1
            // 
            this.button1.Location = new System.Drawing.Point(8, 70);
            this.button1.Name = "button1";
            this.button1.Size = new System.Drawing.Size(52, 41);
            this.button1.TabIndex = 4;
            this.button1.Text = "预览\r\n二维码";
            this.button1.UseVisualStyleBackColor = true;
            this.button1.Click += new System.EventHandler(this.button1_Click);
            // 
            // label7
            // 
            this.label7.AutoSize = true;
            this.label7.Location = new System.Drawing.Point(235, 33);
            this.label7.Name = "label7";
            this.label7.Size = new System.Drawing.Size(191, 48);
            this.label7.TabIndex = 1;
            this.label7.Text = "内容请勿超过152个英文或76个汉字\r\n\r\n插入图片会自动缩放\r\n为二维码的1/5";
            // 
            // tabPage1
            // 
            this.tabPage1.Controls.Add(this.labTh);
            this.tabPage1.Controls.Add(this.label8);
            this.tabPage1.Controls.Add(this.tabControl2);
            this.tabPage1.Controls.Add(this.chkMac);
            this.tabPage1.Controls.Add(this.chkPortAll);
            this.tabPage1.Controls.Add(this.chkNormalPort);
            this.tabPage1.Controls.Add(this.label1);
            this.tabPage1.Controls.Add(this.txtIp1);
            this.tabPage1.Controls.Add(this.txtIp2);
            this.tabPage1.Controls.Add(this.label2);
            this.tabPage1.Controls.Add(this.labStatus);
            this.tabPage1.Controls.Add(this.btnPort);
            this.tabPage1.Controls.Add(this.btnSearch);
            this.tabPage1.Location = new System.Drawing.Point(4, 22);
            this.tabPage1.Name = "tabPage1";
            this.tabPage1.Padding = new System.Windows.Forms.Padding(3);
            this.tabPage1.Size = new System.Drawing.Size(601, 442);
            this.tabPage1.TabIndex = 0;
            this.tabPage1.Text = "IP段mac扫描工具";
            this.tabPage1.UseVisualStyleBackColor = true;
            // 
            // labTh
            // 
            this.labTh.AutoSize = true;
            this.labTh.Location = new System.Drawing.Point(521, 42);
            this.labTh.Name = "labTh";
            this.labTh.Size = new System.Drawing.Size(11, 12);
            this.labTh.TabIndex = 12;
            this.labTh.Text = "0";
            // 
            // label8
            // 
            this.label8.AutoSize = true;
            this.label8.Location = new System.Drawing.Point(450, 42);
            this.label8.Name = "label8";
            this.label8.Size = new System.Drawing.Size(71, 12);
            this.label8.TabIndex = 0;
            this.label8.Text = "当前线程数:";
            // 
            // tabControl2
            // 
            this.tabControl2.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.tabControl2.Controls.Add(this.tabPage3);
            this.tabControl2.Controls.Add(this.tabPage4);
            this.tabControl2.Location = new System.Drawing.Point(3, 81);
            this.tabControl2.Name = "tabControl2";
            this.tabControl2.SelectedIndex = 0;
            this.tabControl2.Size = new System.Drawing.Size(590, 358);
            this.tabControl2.TabIndex = 10;
            // 
            // tabPage3
            // 
            this.tabPage3.Controls.Add(this.txtResult);
            this.tabPage3.Location = new System.Drawing.Point(4, 22);
            this.tabPage3.Name = "tabPage3";
            this.tabPage3.Padding = new System.Windows.Forms.Padding(3);
            this.tabPage3.Size = new System.Drawing.Size(582, 332);
            this.tabPage3.TabIndex = 0;
            this.tabPage3.Text = "成功记录";
            this.tabPage3.UseVisualStyleBackColor = true;
            // 
            // txtResult
            // 
            this.txtResult.Dock = System.Windows.Forms.DockStyle.Fill;
            this.txtResult.Location = new System.Drawing.Point(3, 3);
            this.txtResult.Multiline = true;
            this.txtResult.Name = "txtResult";
            this.txtResult.ScrollBars = System.Windows.Forms.ScrollBars.Both;
            this.txtResult.Size = new System.Drawing.Size(576, 326);
            this.txtResult.TabIndex = 8;
            this.txtResult.WordWrap = false;
            // 
            // tabPage4
            // 
            this.tabPage4.Controls.Add(this.txtErr);
            this.tabPage4.Location = new System.Drawing.Point(4, 22);
            this.tabPage4.Name = "tabPage4";
            this.tabPage4.Padding = new System.Windows.Forms.Padding(3);
            this.tabPage4.Size = new System.Drawing.Size(582, 332);
            this.tabPage4.TabIndex = 1;
            this.tabPage4.Text = "失败记录";
            this.tabPage4.UseVisualStyleBackColor = true;
            // 
            // txtErr
            // 
            this.txtErr.Dock = System.Windows.Forms.DockStyle.Fill;
            this.txtErr.Location = new System.Drawing.Point(3, 3);
            this.txtErr.Multiline = true;
            this.txtErr.Name = "txtErr";
            this.txtErr.ScrollBars = System.Windows.Forms.ScrollBars.Both;
            this.txtErr.Size = new System.Drawing.Size(576, 326);
            this.txtErr.TabIndex = 0;
            this.txtErr.TabStop = false;
            // 
            // chkMac
            // 
            this.chkMac.AutoSize = true;
            this.chkMac.Location = new System.Drawing.Point(452, 12);
            this.chkMac.Name = "chkMac";
            this.chkMac.Size = new System.Drawing.Size(108, 16);
            this.chkMac.TabIndex = 7;
            this.chkMac.Text = "扫描MAC-仅内网";
            this.chkMac.UseVisualStyleBackColor = true;
            // 
            // chkPortAll
            // 
            this.chkPortAll.AutoSize = true;
            this.chkPortAll.Location = new System.Drawing.Point(303, 42);
            this.chkPortAll.Name = "chkPortAll";
            this.chkPortAll.Size = new System.Drawing.Size(144, 16);
            this.chkPortAll.TabIndex = 6;
            this.chkPortAll.Text = "扫描全部端口:1~65535";
            this.chkPortAll.UseVisualStyleBackColor = true;
            this.chkPortAll.CheckedChanged += new System.EventHandler(this.chkPortAll_CheckedChanged);
            // 
            // chkNormalPort
            // 
            this.chkNormalPort.AutoSize = true;
            this.chkNormalPort.Checked = true;
            this.chkNormalPort.CheckState = System.Windows.Forms.CheckState.Checked;
            this.chkNormalPort.Location = new System.Drawing.Point(303, 12);
            this.chkNormalPort.Name = "chkNormalPort";
            this.chkNormalPort.Size = new System.Drawing.Size(96, 16);
            this.chkNormalPort.TabIndex = 5;
            this.chkNormalPort.Text = "扫描常用端口";
            this.chkNormalPort.UseVisualStyleBackColor = true;
            this.chkNormalPort.CheckedChanged += new System.EventHandler(this.chkPortAll_CheckedChanged);
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(4, 13);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(35, 12);
            this.label1.TabIndex = 0;
            this.label1.Text = "起始:";
            // 
            // txtIp1
            // 
            this.txtIp1.Location = new System.Drawing.Point(41, 10);
            this.txtIp1.Name = "txtIp1";
            this.txtIp1.Size = new System.Drawing.Size(113, 21);
            this.txtIp1.TabIndex = 1;
            this.txtIp1.Text = "10.2.0.1";
            // 
            // txtIp2
            // 
            this.txtIp2.Location = new System.Drawing.Point(41, 37);
            this.txtIp2.Name = "txtIp2";
            this.txtIp2.Size = new System.Drawing.Size(113, 21);
            this.txtIp2.TabIndex = 2;
            this.txtIp2.Text = "10.2.0.20";
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(5, 39);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(35, 12);
            this.label2.TabIndex = 0;
            this.label2.Text = "结束:";
            // 
            // labStatus
            // 
            this.labStatus.AutoSize = true;
            this.labStatus.ForeColor = System.Drawing.Color.Red;
            this.labStatus.Location = new System.Drawing.Point(5, 66);
            this.labStatus.Name = "labStatus";
            this.labStatus.Size = new System.Drawing.Size(65, 12);
            this.labStatus.TabIndex = 0;
            this.labStatus.Text = "未开始扫描";
            // 
            // btnPort
            // 
            this.btnPort.Location = new System.Drawing.Point(161, 37);
            this.btnPort.Name = "btnPort";
            this.btnPort.Size = new System.Drawing.Size(137, 23);
            this.btnPort.TabIndex = 4;
            this.btnPort.Text = "单IP多线程扫描端口";
            this.btnPort.UseVisualStyleBackColor = true;
            this.btnPort.Click += new System.EventHandler(this.btnPort_Click);
            // 
            // btnSearch
            // 
            this.btnSearch.Location = new System.Drawing.Point(161, 8);
            this.btnSearch.Name = "btnSearch";
            this.btnSearch.Size = new System.Drawing.Size(137, 23);
            this.btnSearch.TabIndex = 3;
            this.btnSearch.Text = "扫描";
            this.btnSearch.UseVisualStyleBackColor = true;
            this.btnSearch.Click += new System.EventHandler(this.btnSearch_Click);
            // 
            // tabControl1
            // 
            this.tabControl1.Controls.Add(this.tabPage1);
            this.tabControl1.Controls.Add(this.tabPage2);
            this.tabControl1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tabControl1.Location = new System.Drawing.Point(0, 0);
            this.tabControl1.Name = "tabControl1";
            this.tabControl1.SelectedIndex = 0;
            this.tabControl1.Size = new System.Drawing.Size(609, 468);
            this.tabControl1.TabIndex = 1;
            // 
            // btnRead
            // 
            this.btnRead.Location = new System.Drawing.Point(124, 59);
            this.btnRead.Name = "btnRead";
            this.btnRead.Size = new System.Drawing.Size(81, 53);
            this.btnRead.TabIndex = 8;
            this.btnRead.Text = "解析\r\n二维码...";
            this.btnRead.UseVisualStyleBackColor = true;
            this.btnRead.Click += new System.EventHandler(this.btnRead_Click);
            // 
            // OtherTools
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(609, 468);
            this.Controls.Add(this.tabControl1);
            this.Name = "OtherTools";
            this.Text = "OtherTools";
            this.tabPage2.ResumeLayout(false);
            this.tabPage2.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox2)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox1)).EndInit();
            this.tabPage1.ResumeLayout(false);
            this.tabPage1.PerformLayout();
            this.tabControl2.ResumeLayout(false);
            this.tabPage3.ResumeLayout(false);
            this.tabPage3.PerformLayout();
            this.tabPage4.ResumeLayout(false);
            this.tabPage4.PerformLayout();
            this.tabControl1.ResumeLayout(false);
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.TabPage tabPage2;
        private System.Windows.Forms.PictureBox pictureBox2;
        private System.Windows.Forms.PictureBox pictureBox1;
        private System.Windows.Forms.ComboBox lstOtherError;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.Button button2;
        private System.Windows.Forms.TextBox txtOtherQr;
        private System.Windows.Forms.TextBox txtOtherQrSize;
        private System.Windows.Forms.Button button4;
        private System.Windows.Forms.Label label5;
        private System.Windows.Forms.Button button3;
        private System.Windows.Forms.Label label6;
        private System.Windows.Forms.Button button1;
        private System.Windows.Forms.Label label7;
        private System.Windows.Forms.TabPage tabPage1;
        private System.Windows.Forms.CheckBox chkMac;
        private System.Windows.Forms.CheckBox chkPortAll;
        private System.Windows.Forms.CheckBox chkNormalPort;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.TextBox txtResult;
        private System.Windows.Forms.TextBox txtIp1;
        private System.Windows.Forms.TextBox txtIp2;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.Label labStatus;
        private System.Windows.Forms.Button btnSearch;
        private System.Windows.Forms.TabControl tabControl1;
        private System.Windows.Forms.TabControl tabControl2;
        private System.Windows.Forms.TabPage tabPage3;
        private System.Windows.Forms.TabPage tabPage4;
        private System.Windows.Forms.TextBox txtErr;
        private System.Windows.Forms.Button btnPort;
        private System.Windows.Forms.Label labTh;
        private System.Windows.Forms.Label label8;
        private System.Windows.Forms.Button btnRead;
    }
}