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
            this.label1 = new System.Windows.Forms.Label();
            this.label2 = new System.Windows.Forms.Label();
            this.txtIp1 = new System.Windows.Forms.TextBox();
            this.txtIp2 = new System.Windows.Forms.TextBox();
            this.btnSearch = new System.Windows.Forms.Button();
            this.txtResult = new System.Windows.Forms.TextBox();
            this.chkShowErrIp = new System.Windows.Forms.CheckBox();
            this.SuspendLayout();
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(14, 15);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(35, 12);
            this.label1.TabIndex = 0;
            this.label1.Text = "起始:";
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(180, 15);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(35, 12);
            this.label2.TabIndex = 0;
            this.label2.Text = "结束:";
            // 
            // txtIp1
            // 
            this.txtIp1.Location = new System.Drawing.Point(55, 12);
            this.txtIp1.Name = "txtIp1";
            this.txtIp1.Size = new System.Drawing.Size(113, 21);
            this.txtIp1.TabIndex = 1;
            this.txtIp1.Text = "192.168.254.50";
            // 
            // txtIp2
            // 
            this.txtIp2.Location = new System.Drawing.Point(221, 12);
            this.txtIp2.Name = "txtIp2";
            this.txtIp2.Size = new System.Drawing.Size(118, 21);
            this.txtIp2.TabIndex = 2;
            this.txtIp2.Text = "192.168.254.58";
            // 
            // btnSearch
            // 
            this.btnSearch.Location = new System.Drawing.Point(469, 10);
            this.btnSearch.Name = "btnSearch";
            this.btnSearch.Size = new System.Drawing.Size(86, 23);
            this.btnSearch.TabIndex = 3;
            this.btnSearch.Text = "扫描MAC地址";
            this.btnSearch.UseVisualStyleBackColor = true;
            this.btnSearch.Click += new System.EventHandler(this.btnSearch_Click);
            // 
            // txtResult
            // 
            this.txtResult.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.txtResult.Location = new System.Drawing.Point(16, 48);
            this.txtResult.Multiline = true;
            this.txtResult.Name = "txtResult";
            this.txtResult.ScrollBars = System.Windows.Forms.ScrollBars.Both;
            this.txtResult.Size = new System.Drawing.Size(529, 213);
            this.txtResult.TabIndex = 4;
            this.txtResult.WordWrap = false;
            // 
            // chkShowErrIp
            // 
            this.chkShowErrIp.AutoSize = true;
            this.chkShowErrIp.Location = new System.Drawing.Point(345, 14);
            this.chkShowErrIp.Name = "chkShowErrIp";
            this.chkShowErrIp.Size = new System.Drawing.Size(120, 16);
            this.chkShowErrIp.TabIndex = 5;
            this.chkShowErrIp.Text = "显示获取出错的IP";
            this.chkShowErrIp.UseVisualStyleBackColor = true;
            // 
            // OtherTools
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(557, 273);
            this.Controls.Add(this.chkShowErrIp);
            this.Controls.Add(this.txtResult);
            this.Controls.Add(this.btnSearch);
            this.Controls.Add(this.txtIp2);
            this.Controls.Add(this.txtIp1);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.label1);
            this.Name = "OtherTools";
            this.Text = "OtherTools";
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.TextBox txtIp1;
        private System.Windows.Forms.TextBox txtIp2;
        private System.Windows.Forms.Button btnSearch;
        private System.Windows.Forms.TextBox txtResult;
        private System.Windows.Forms.CheckBox chkShowErrIp;
    }
}