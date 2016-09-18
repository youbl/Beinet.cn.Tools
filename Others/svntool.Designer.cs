namespace Beinet.cn.Tools.Others
{
    partial class svntool
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
            this.txtSvnDir = new System.Windows.Forms.TextBox();
            this.btnSvnDir = new System.Windows.Forms.Button();
            this.btnSvnup = new System.Windows.Forms.Button();
            this.txtSvnver = new System.Windows.Forms.TextBox();
            this.txtRet = new System.Windows.Forms.TextBox();
            this.SuspendLayout();
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(12, 9);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(83, 12);
            this.label1.TabIndex = 0;
            this.label1.Text = "svn目录或url:";
            // 
            // txtSvnDir
            // 
            this.txtSvnDir.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.txtSvnDir.Location = new System.Drawing.Point(97, 6);
            this.txtSvnDir.Name = "txtSvnDir";
            this.txtSvnDir.Size = new System.Drawing.Size(558, 21);
            this.txtSvnDir.TabIndex = 1;
            // 
            // btnSvnDir
            // 
            this.btnSvnDir.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.btnSvnDir.Location = new System.Drawing.Point(661, 4);
            this.btnSvnDir.Name = "btnSvnDir";
            this.btnSvnDir.Size = new System.Drawing.Size(75, 23);
            this.btnSvnDir.TabIndex = 2;
            this.btnSvnDir.Text = "选择..";
            this.btnSvnDir.UseVisualStyleBackColor = true;
            this.btnSvnDir.Click += new System.EventHandler(this.btnSvnDir_Click);
            // 
            // btnSvnup
            // 
            this.btnSvnup.Location = new System.Drawing.Point(12, 33);
            this.btnSvnup.Name = "btnSvnup";
            this.btnSvnup.Size = new System.Drawing.Size(75, 23);
            this.btnSvnup.TabIndex = 3;
            this.btnSvnup.Text = "更新到:";
            this.btnSvnup.UseVisualStyleBackColor = true;
            this.btnSvnup.Click += new System.EventHandler(this.btnSvnup_Click);
            // 
            // txtSvnver
            // 
            this.txtSvnver.Location = new System.Drawing.Point(97, 35);
            this.txtSvnver.Name = "txtSvnver";
            this.txtSvnver.Size = new System.Drawing.Size(58, 21);
            this.txtSvnver.TabIndex = 4;
            this.txtSvnver.Text = "0";
            // 
            // txtRet
            // 
            this.txtRet.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.txtRet.Location = new System.Drawing.Point(12, 77);
            this.txtRet.Multiline = true;
            this.txtRet.Name = "txtRet";
            this.txtRet.Size = new System.Drawing.Size(724, 460);
            this.txtRet.TabIndex = 5;
            // 
            // svntool
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(748, 549);
            this.Controls.Add(this.txtRet);
            this.Controls.Add(this.txtSvnver);
            this.Controls.Add(this.btnSvnup);
            this.Controls.Add(this.btnSvnDir);
            this.Controls.Add(this.txtSvnDir);
            this.Controls.Add(this.label1);
            this.Name = "svntool";
            this.Text = "svntool";
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.TextBox txtSvnDir;
        private System.Windows.Forms.Button btnSvnDir;
        private System.Windows.Forms.Button btnSvnup;
        private System.Windows.Forms.TextBox txtSvnver;
        private System.Windows.Forms.TextBox txtRet;
    }
}