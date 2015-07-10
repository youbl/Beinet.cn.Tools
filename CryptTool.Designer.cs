namespace Beinet.cn.Tools
{
    partial class CryptTool
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
            this.label2 = new System.Windows.Forms.Label();
            this.txtPwd1 = new System.Windows.Forms.TextBox();
            this.txtStr1 = new System.Windows.Forms.TextBox();
            this.btnDes1 = new System.Windows.Forms.Button();
            this.btnUnDes1 = new System.Windows.Forms.Button();
            this.btn3des1 = new System.Windows.Forms.Button();
            this.btnUn3des1 = new System.Windows.Forms.Button();
            this.btnMD5_1 = new System.Windows.Forms.Button();
            this.btnMD5_2 = new System.Windows.Forms.Button();
            this.btnUn3des2 = new System.Windows.Forms.Button();
            this.btn3des2 = new System.Windows.Forms.Button();
            this.btnUnDes2 = new System.Windows.Forms.Button();
            this.btnDes2 = new System.Windows.Forms.Button();
            this.txtStr2 = new System.Windows.Forms.TextBox();
            this.txtPwd2 = new System.Windows.Forms.TextBox();
            this.label3 = new System.Windows.Forms.Label();
            this.label4 = new System.Windows.Forms.Label();
            this.button6 = new System.Windows.Forms.Button();
            this.txtRet = new System.Windows.Forms.TextBox();
            this.panel1 = new System.Windows.Forms.Panel();
            this.radEncoding2_2 = new System.Windows.Forms.RadioButton();
            this.radEncoding2_1 = new System.Windows.Forms.RadioButton();
            this.panel2 = new System.Windows.Forms.Panel();
            this.radEncoding1_2 = new System.Windows.Forms.RadioButton();
            this.radEncoding1_1 = new System.Windows.Forms.RadioButton();
            this.btnBase64_1 = new System.Windows.Forms.Button();
            this.btnUnBase64_1 = new System.Windows.Forms.Button();
            this.btnBase64_2 = new System.Windows.Forms.Button();
            this.btnUnBase64_2 = new System.Windows.Forms.Button();
            this.btnHash1 = new System.Windows.Forms.Button();
            this.btnHash32_1 = new System.Windows.Forms.Button();
            this.btnHash2 = new System.Windows.Forms.Button();
            this.btnHash32_2 = new System.Windows.Forms.Button();
            this.labScanStatus = new System.Windows.Forms.Label();
            this.panel1.SuspendLayout();
            this.panel2.SuspendLayout();
            this.SuspendLayout();
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(3, 13);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(35, 12);
            this.label1.TabIndex = 0;
            this.label1.Text = "密码:";
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(145, 13);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(83, 12);
            this.label2.TabIndex = 0;
            this.label2.Text = "明文或加密串:";
            // 
            // txtPwd1
            // 
            this.txtPwd1.Location = new System.Drawing.Point(39, 10);
            this.txtPwd1.Name = "txtPwd1";
            this.txtPwd1.Size = new System.Drawing.Size(100, 21);
            this.txtPwd1.TabIndex = 1;
            // 
            // txtStr1
            // 
            this.txtStr1.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.txtStr1.Location = new System.Drawing.Point(235, 9);
            this.txtStr1.Name = "txtStr1";
            this.txtStr1.Size = new System.Drawing.Size(513, 21);
            this.txtStr1.TabIndex = 2;
            // 
            // btnDes1
            // 
            this.btnDes1.Location = new System.Drawing.Point(132, 38);
            this.btnDes1.Name = "btnDes1";
            this.btnDes1.Size = new System.Drawing.Size(61, 23);
            this.btnDes1.TabIndex = 3;
            this.btnDes1.Text = "DES加密";
            this.btnDes1.UseVisualStyleBackColor = true;
            this.btnDes1.Click += new System.EventHandler(this.btnDes1_Click);
            // 
            // btnUnDes1
            // 
            this.btnUnDes1.Location = new System.Drawing.Point(199, 38);
            this.btnUnDes1.Name = "btnUnDes1";
            this.btnUnDes1.Size = new System.Drawing.Size(65, 23);
            this.btnUnDes1.TabIndex = 4;
            this.btnUnDes1.Text = "DES解密";
            this.btnUnDes1.UseVisualStyleBackColor = true;
            this.btnUnDes1.Click += new System.EventHandler(this.btnUnDes1_Click);
            // 
            // btn3des1
            // 
            this.btn3des1.Location = new System.Drawing.Point(266, 38);
            this.btn3des1.Name = "btn3des1";
            this.btn3des1.Size = new System.Drawing.Size(65, 23);
            this.btn3des1.TabIndex = 5;
            this.btn3des1.Text = "3DES加密";
            this.btn3des1.UseVisualStyleBackColor = true;
            this.btn3des1.Click += new System.EventHandler(this.btn3des1_Click);
            // 
            // btnUn3des1
            // 
            this.btnUn3des1.Location = new System.Drawing.Point(333, 38);
            this.btnUn3des1.Name = "btnUn3des1";
            this.btnUn3des1.Size = new System.Drawing.Size(65, 23);
            this.btnUn3des1.TabIndex = 6;
            this.btnUn3des1.Text = "3DES解密";
            this.btnUn3des1.UseVisualStyleBackColor = true;
            this.btnUn3des1.Click += new System.EventHandler(this.btnUn3des_Click);
            // 
            // btnMD5_1
            // 
            this.btnMD5_1.Location = new System.Drawing.Point(400, 38);
            this.btnMD5_1.Name = "btnMD5_1";
            this.btnMD5_1.Size = new System.Drawing.Size(65, 23);
            this.btnMD5_1.TabIndex = 7;
            this.btnMD5_1.Text = "MD5加密";
            this.btnMD5_1.UseVisualStyleBackColor = true;
            this.btnMD5_1.Click += new System.EventHandler(this.btnMD5_Click);
            // 
            // btnMD5_2
            // 
            this.btnMD5_2.Location = new System.Drawing.Point(400, 101);
            this.btnMD5_2.Name = "btnMD5_2";
            this.btnMD5_2.Size = new System.Drawing.Size(65, 23);
            this.btnMD5_2.TabIndex = 16;
            this.btnMD5_2.Text = "MD5加密";
            this.btnMD5_2.UseVisualStyleBackColor = true;
            this.btnMD5_2.Click += new System.EventHandler(this.btnMD5_Click);
            // 
            // btnUn3des2
            // 
            this.btnUn3des2.Location = new System.Drawing.Point(333, 101);
            this.btnUn3des2.Name = "btnUn3des2";
            this.btnUn3des2.Size = new System.Drawing.Size(65, 23);
            this.btnUn3des2.TabIndex = 15;
            this.btnUn3des2.Text = "3DES解密";
            this.btnUn3des2.UseVisualStyleBackColor = true;
            this.btnUn3des2.Click += new System.EventHandler(this.btnUn3des_Click);
            // 
            // btn3des2
            // 
            this.btn3des2.Location = new System.Drawing.Point(266, 101);
            this.btn3des2.Name = "btn3des2";
            this.btn3des2.Size = new System.Drawing.Size(65, 23);
            this.btn3des2.TabIndex = 14;
            this.btn3des2.Text = "3DES加密";
            this.btn3des2.UseVisualStyleBackColor = true;
            this.btn3des2.Click += new System.EventHandler(this.btn3des1_Click);
            // 
            // btnUnDes2
            // 
            this.btnUnDes2.Location = new System.Drawing.Point(199, 101);
            this.btnUnDes2.Name = "btnUnDes2";
            this.btnUnDes2.Size = new System.Drawing.Size(65, 23);
            this.btnUnDes2.TabIndex = 13;
            this.btnUnDes2.Text = "DES解密";
            this.btnUnDes2.UseVisualStyleBackColor = true;
            this.btnUnDes2.Click += new System.EventHandler(this.btnUnDes1_Click);
            // 
            // btnDes2
            // 
            this.btnDes2.Location = new System.Drawing.Point(132, 101);
            this.btnDes2.Name = "btnDes2";
            this.btnDes2.Size = new System.Drawing.Size(65, 23);
            this.btnDes2.TabIndex = 12;
            this.btnDes2.Text = "DES加密";
            this.btnDes2.UseVisualStyleBackColor = true;
            this.btnDes2.Click += new System.EventHandler(this.btnDes1_Click);
            // 
            // txtStr2
            // 
            this.txtStr2.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.txtStr2.Location = new System.Drawing.Point(235, 74);
            this.txtStr2.Name = "txtStr2";
            this.txtStr2.Size = new System.Drawing.Size(513, 21);
            this.txtStr2.TabIndex = 11;
            // 
            // txtPwd2
            // 
            this.txtPwd2.Location = new System.Drawing.Point(39, 75);
            this.txtPwd2.Name = "txtPwd2";
            this.txtPwd2.Size = new System.Drawing.Size(100, 21);
            this.txtPwd2.TabIndex = 10;
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Location = new System.Drawing.Point(145, 79);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(83, 12);
            this.label3.TabIndex = 8;
            this.label3.Text = "明文或加密串:";
            // 
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.Location = new System.Drawing.Point(3, 79);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(35, 12);
            this.label4.TabIndex = 9;
            this.label4.Text = "密码:";
            // 
            // button6
            // 
            this.button6.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.button6.BackColor = System.Drawing.Color.Red;
            this.button6.Location = new System.Drawing.Point(5, 64);
            this.button6.Name = "button6";
            this.button6.Size = new System.Drawing.Size(752, 5);
            this.button6.TabIndex = 17;
            this.button6.TabStop = false;
            this.button6.UseVisualStyleBackColor = false;
            // 
            // txtRet
            // 
            this.txtRet.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.txtRet.Location = new System.Drawing.Point(5, 132);
            this.txtRet.Multiline = true;
            this.txtRet.Name = "txtRet";
            this.txtRet.ScrollBars = System.Windows.Forms.ScrollBars.Both;
            this.txtRet.Size = new System.Drawing.Size(752, 360);
            this.txtRet.TabIndex = 18;
            this.txtRet.WordWrap = false;
            // 
            // panel1
            // 
            this.panel1.Controls.Add(this.radEncoding2_2);
            this.panel1.Controls.Add(this.radEncoding2_1);
            this.panel1.Location = new System.Drawing.Point(12, 100);
            this.panel1.Name = "panel1";
            this.panel1.Size = new System.Drawing.Size(115, 25);
            this.panel1.TabIndex = 19;
            // 
            // radEncoding2_2
            // 
            this.radEncoding2_2.AutoSize = true;
            this.radEncoding2_2.Checked = true;
            this.radEncoding2_2.Location = new System.Drawing.Point(56, 4);
            this.radEncoding2_2.Name = "radEncoding2_2";
            this.radEncoding2_2.Size = new System.Drawing.Size(59, 16);
            this.radEncoding2_2.TabIndex = 0;
            this.radEncoding2_2.TabStop = true;
            this.radEncoding2_2.Text = "GB2312";
            this.radEncoding2_2.UseVisualStyleBackColor = true;
            // 
            // radEncoding2_1
            // 
            this.radEncoding2_1.AutoSize = true;
            this.radEncoding2_1.Location = new System.Drawing.Point(3, 4);
            this.radEncoding2_1.Name = "radEncoding2_1";
            this.radEncoding2_1.Size = new System.Drawing.Size(47, 16);
            this.radEncoding2_1.TabIndex = 0;
            this.radEncoding2_1.Text = "UTF8";
            this.radEncoding2_1.UseVisualStyleBackColor = true;
            // 
            // panel2
            // 
            this.panel2.Controls.Add(this.radEncoding1_2);
            this.panel2.Controls.Add(this.radEncoding1_1);
            this.panel2.Location = new System.Drawing.Point(12, 36);
            this.panel2.Name = "panel2";
            this.panel2.Size = new System.Drawing.Size(115, 25);
            this.panel2.TabIndex = 19;
            // 
            // radEncoding1_2
            // 
            this.radEncoding1_2.AutoSize = true;
            this.radEncoding1_2.Checked = true;
            this.radEncoding1_2.Location = new System.Drawing.Point(56, 4);
            this.radEncoding1_2.Name = "radEncoding1_2";
            this.radEncoding1_2.Size = new System.Drawing.Size(59, 16);
            this.radEncoding1_2.TabIndex = 0;
            this.radEncoding1_2.TabStop = true;
            this.radEncoding1_2.Text = "GB2312";
            this.radEncoding1_2.UseVisualStyleBackColor = true;
            // 
            // radEncoding1_1
            // 
            this.radEncoding1_1.AutoSize = true;
            this.radEncoding1_1.Location = new System.Drawing.Point(3, 4);
            this.radEncoding1_1.Name = "radEncoding1_1";
            this.radEncoding1_1.Size = new System.Drawing.Size(47, 16);
            this.radEncoding1_1.TabIndex = 0;
            this.radEncoding1_1.Text = "UTF8";
            this.radEncoding1_1.UseVisualStyleBackColor = true;
            // 
            // btnBase64_1
            // 
            this.btnBase64_1.Location = new System.Drawing.Point(467, 38);
            this.btnBase64_1.Name = "btnBase64_1";
            this.btnBase64_1.Size = new System.Drawing.Size(75, 23);
            this.btnBase64_1.TabIndex = 7;
            this.btnBase64_1.Text = "Base64加密";
            this.btnBase64_1.UseVisualStyleBackColor = true;
            this.btnBase64_1.Click += new System.EventHandler(this.btnBase64_1_Click);
            // 
            // btnUnBase64_1
            // 
            this.btnUnBase64_1.Location = new System.Drawing.Point(544, 38);
            this.btnUnBase64_1.Name = "btnUnBase64_1";
            this.btnUnBase64_1.Size = new System.Drawing.Size(75, 23);
            this.btnUnBase64_1.TabIndex = 7;
            this.btnUnBase64_1.Text = "Base64解密";
            this.btnUnBase64_1.UseVisualStyleBackColor = true;
            this.btnUnBase64_1.Click += new System.EventHandler(this.btnUnBase64_1_Click);
            // 
            // btnBase64_2
            // 
            this.btnBase64_2.Location = new System.Drawing.Point(467, 101);
            this.btnBase64_2.Name = "btnBase64_2";
            this.btnBase64_2.Size = new System.Drawing.Size(75, 23);
            this.btnBase64_2.TabIndex = 7;
            this.btnBase64_2.Text = "Base64加密";
            this.btnBase64_2.UseVisualStyleBackColor = true;
            this.btnBase64_2.Click += new System.EventHandler(this.btnBase64_1_Click);
            // 
            // btnUnBase64_2
            // 
            this.btnUnBase64_2.Location = new System.Drawing.Point(544, 101);
            this.btnUnBase64_2.Name = "btnUnBase64_2";
            this.btnUnBase64_2.Size = new System.Drawing.Size(75, 23);
            this.btnUnBase64_2.TabIndex = 7;
            this.btnUnBase64_2.Text = "Base64解密";
            this.btnUnBase64_2.UseVisualStyleBackColor = true;
            this.btnUnBase64_2.Click += new System.EventHandler(this.btnUnBase64_1_Click);
            // 
            // btnHash1
            // 
            this.btnHash1.Location = new System.Drawing.Point(621, 38);
            this.btnHash1.Name = "btnHash1";
            this.btnHash1.Size = new System.Drawing.Size(43, 23);
            this.btnHash1.TabIndex = 20;
            this.btnHash1.Text = "Hash";
            this.btnHash1.UseVisualStyleBackColor = true;
            this.btnHash1.Click += new System.EventHandler(this.btnHash1_Click);
            // 
            // btnHash32_1
            // 
            this.btnHash32_1.Location = new System.Drawing.Point(669, 38);
            this.btnHash32_1.Name = "btnHash32_1";
            this.btnHash32_1.Size = new System.Drawing.Size(52, 23);
            this.btnHash32_1.TabIndex = 20;
            this.btnHash32_1.Text = "Hash32";
            this.btnHash32_1.UseVisualStyleBackColor = true;
            this.btnHash32_1.Click += new System.EventHandler(this.btnHash32_1_Click);
            // 
            // btnHash2
            // 
            this.btnHash2.Location = new System.Drawing.Point(621, 101);
            this.btnHash2.Name = "btnHash2";
            this.btnHash2.Size = new System.Drawing.Size(43, 23);
            this.btnHash2.TabIndex = 20;
            this.btnHash2.Text = "Hash";
            this.btnHash2.UseVisualStyleBackColor = true;
            this.btnHash2.Click += new System.EventHandler(this.btnHash1_Click);
            // 
            // btnHash32_2
            // 
            this.btnHash32_2.Location = new System.Drawing.Point(669, 101);
            this.btnHash32_2.Name = "btnHash32_2";
            this.btnHash32_2.Size = new System.Drawing.Size(52, 23);
            this.btnHash32_2.TabIndex = 20;
            this.btnHash32_2.Text = "Hash32";
            this.btnHash32_2.UseVisualStyleBackColor = true;
            this.btnHash32_2.Click += new System.EventHandler(this.btnHash32_1_Click);
            // 
            // labScanStatus
            // 
            this.labScanStatus.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.labScanStatus.AutoSize = true;
            this.labScanStatus.Location = new System.Drawing.Point(5, 496);
            this.labScanStatus.Name = "labScanStatus";
            this.labScanStatus.Size = new System.Drawing.Size(77, 12);
            this.labScanStatus.TabIndex = 0;
            this.labScanStatus.Text = "没有文件拖入";
            // 
            // CryptTool
            // 
            this.AllowDrop = true;
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(760, 512);
            this.Controls.Add(this.labScanStatus);
            this.Controls.Add(this.btnHash32_2);
            this.Controls.Add(this.btnHash2);
            this.Controls.Add(this.btnHash32_1);
            this.Controls.Add(this.btnHash1);
            this.Controls.Add(this.panel2);
            this.Controls.Add(this.panel1);
            this.Controls.Add(this.txtRet);
            this.Controls.Add(this.button6);
            this.Controls.Add(this.btnMD5_2);
            this.Controls.Add(this.btnUn3des2);
            this.Controls.Add(this.btn3des2);
            this.Controls.Add(this.btnUnDes2);
            this.Controls.Add(this.btnDes2);
            this.Controls.Add(this.txtStr2);
            this.Controls.Add(this.txtPwd2);
            this.Controls.Add(this.label3);
            this.Controls.Add(this.label4);
            this.Controls.Add(this.btnUnBase64_2);
            this.Controls.Add(this.btnUnBase64_1);
            this.Controls.Add(this.btnBase64_2);
            this.Controls.Add(this.btnBase64_1);
            this.Controls.Add(this.btnMD5_1);
            this.Controls.Add(this.btnUn3des1);
            this.Controls.Add(this.btn3des1);
            this.Controls.Add(this.btnUnDes1);
            this.Controls.Add(this.btnDes1);
            this.Controls.Add(this.txtStr1);
            this.Controls.Add(this.txtPwd1);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.label1);
            this.Name = "CryptTool";
            this.Text = "CryptTool";
            this.DragDrop += new System.Windows.Forms.DragEventHandler(this.CryptTool_DragDrop);
            this.DragEnter += new System.Windows.Forms.DragEventHandler(this.CryptTool_DragEnter);
            this.panel1.ResumeLayout(false);
            this.panel1.PerformLayout();
            this.panel2.ResumeLayout(false);
            this.panel2.PerformLayout();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.TextBox txtPwd1;
        private System.Windows.Forms.TextBox txtStr1;
        private System.Windows.Forms.Button btnDes1;
        private System.Windows.Forms.Button btnUnDes1;
        private System.Windows.Forms.Button btn3des1;
        private System.Windows.Forms.Button btnUn3des1;
        private System.Windows.Forms.Button btnMD5_1;
        private System.Windows.Forms.Button btnMD5_2;
        private System.Windows.Forms.Button btnUn3des2;
        private System.Windows.Forms.Button btn3des2;
        private System.Windows.Forms.Button btnUnDes2;
        private System.Windows.Forms.Button btnDes2;
        private System.Windows.Forms.TextBox txtStr2;
        private System.Windows.Forms.TextBox txtPwd2;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.Button button6;
        private System.Windows.Forms.TextBox txtRet;
        private System.Windows.Forms.Panel panel1;
        private System.Windows.Forms.RadioButton radEncoding2_2;
        private System.Windows.Forms.RadioButton radEncoding2_1;
        private System.Windows.Forms.Panel panel2;
        private System.Windows.Forms.RadioButton radEncoding1_2;
        private System.Windows.Forms.RadioButton radEncoding1_1;
        private System.Windows.Forms.Button btnBase64_1;
        private System.Windows.Forms.Button btnUnBase64_1;
        private System.Windows.Forms.Button btnBase64_2;
        private System.Windows.Forms.Button btnUnBase64_2;
        private System.Windows.Forms.Button btnHash1;
        private System.Windows.Forms.Button btnHash32_1;
        private System.Windows.Forms.Button btnHash2;
        private System.Windows.Forms.Button btnHash32_2;
        private System.Windows.Forms.Label labScanStatus;

    }
}