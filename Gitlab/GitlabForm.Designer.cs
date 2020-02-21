namespace Beinet.cn.Tools.Gitlab
{
    partial class GitlabForm
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
            this.splitContainer1 = new System.Windows.Forms.SplitContainer();
            this.txtPrivateToken = new System.Windows.Forms.TextBox();
            this.label2 = new System.Windows.Forms.Label();
            this.btnShowGitlab = new System.Windows.Forms.Button();
            this.txtGitlabUrl = new System.Windows.Forms.TextBox();
            this.label1 = new System.Windows.Forms.Label();
            this.splitContainer2 = new System.Windows.Forms.SplitContainer();
            this.btnSelectGitDir = new System.Windows.Forms.Button();
            this.txtGitDir = new System.Windows.Forms.TextBox();
            this.labProjectNum = new System.Windows.Forms.Label();
            this.label4 = new System.Windows.Forms.Label();
            this.label3 = new System.Windows.Forms.Label();
            this.btnGitClone = new System.Windows.Forms.Button();
            this.lvProjects = new System.Windows.Forms.ListView();
            this.columnHeader1 = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.columnHeader2 = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.columnHeader3 = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.columnHeader4 = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.chkIgnoreExistsProj = new System.Windows.Forms.CheckBox();
            ((System.ComponentModel.ISupportInitialize)(this.splitContainer1)).BeginInit();
            this.splitContainer1.Panel1.SuspendLayout();
            this.splitContainer1.Panel2.SuspendLayout();
            this.splitContainer1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.splitContainer2)).BeginInit();
            this.splitContainer2.Panel1.SuspendLayout();
            this.splitContainer2.Panel2.SuspendLayout();
            this.splitContainer2.SuspendLayout();
            this.SuspendLayout();
            // 
            // splitContainer1
            // 
            this.splitContainer1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.splitContainer1.FixedPanel = System.Windows.Forms.FixedPanel.Panel1;
            this.splitContainer1.Location = new System.Drawing.Point(0, 0);
            this.splitContainer1.Margin = new System.Windows.Forms.Padding(1);
            this.splitContainer1.Name = "splitContainer1";
            this.splitContainer1.Orientation = System.Windows.Forms.Orientation.Horizontal;
            // 
            // splitContainer1.Panel1
            // 
            this.splitContainer1.Panel1.Controls.Add(this.chkIgnoreExistsProj);
            this.splitContainer1.Panel1.Controls.Add(this.btnSelectGitDir);
            this.splitContainer1.Panel1.Controls.Add(this.txtPrivateToken);
            this.splitContainer1.Panel1.Controls.Add(this.btnGitClone);
            this.splitContainer1.Panel1.Controls.Add(this.txtGitDir);
            this.splitContainer1.Panel1.Controls.Add(this.label2);
            this.splitContainer1.Panel1.Controls.Add(this.btnShowGitlab);
            this.splitContainer1.Panel1.Controls.Add(this.label4);
            this.splitContainer1.Panel1.Controls.Add(this.txtGitlabUrl);
            this.splitContainer1.Panel1.Controls.Add(this.label1);
            this.splitContainer1.Panel1MinSize = 1;
            // 
            // splitContainer1.Panel2
            // 
            this.splitContainer1.Panel2.Controls.Add(this.splitContainer2);
            this.splitContainer1.Panel2MinSize = 1;
            this.splitContainer1.Size = new System.Drawing.Size(946, 520);
            this.splitContainer1.SplitterDistance = 62;
            this.splitContainer1.SplitterWidth = 1;
            this.splitContainer1.TabIndex = 0;
            // 
            // txtPrivateToken
            // 
            this.txtPrivateToken.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.txtPrivateToken.Location = new System.Drawing.Point(691, 8);
            this.txtPrivateToken.Name = "txtPrivateToken";
            this.txtPrivateToken.Size = new System.Drawing.Size(141, 21);
            this.txtPrivateToken.TabIndex = 4;
            this.txtPrivateToken.Text = "1234567890";
            // 
            // label2
            // 
            this.label2.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(599, 11);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(95, 12);
            this.label2.TabIndex = 3;
            this.label2.Text = "Private Token：";
            // 
            // btnShowGitlab
            // 
            this.btnShowGitlab.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.btnShowGitlab.Location = new System.Drawing.Point(841, 6);
            this.btnShowGitlab.Name = "btnShowGitlab";
            this.btnShowGitlab.Size = new System.Drawing.Size(93, 23);
            this.btnShowGitlab.TabIndex = 2;
            this.btnShowGitlab.Text = "显示所有项目";
            this.btnShowGitlab.UseVisualStyleBackColor = true;
            this.btnShowGitlab.Click += new System.EventHandler(this.btnShowGitlab_Click);
            // 
            // txtGitlabUrl
            // 
            this.txtGitlabUrl.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.txtGitlabUrl.Location = new System.Drawing.Point(85, 8);
            this.txtGitlabUrl.Name = "txtGitlabUrl";
            this.txtGitlabUrl.Size = new System.Drawing.Size(508, 21);
            this.txtGitlabUrl.TabIndex = 1;
            this.txtGitlabUrl.Text = "https://172.18.0.78";
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(12, 11);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(77, 12);
            this.label1.TabIndex = 0;
            this.label1.Text = "gitlab地址：";
            // 
            // splitContainer2
            // 
            this.splitContainer2.Dock = System.Windows.Forms.DockStyle.Fill;
            this.splitContainer2.FixedPanel = System.Windows.Forms.FixedPanel.Panel1;
            this.splitContainer2.Location = new System.Drawing.Point(0, 0);
            this.splitContainer2.Margin = new System.Windows.Forms.Padding(1);
            this.splitContainer2.Name = "splitContainer2";
            this.splitContainer2.Orientation = System.Windows.Forms.Orientation.Horizontal;
            // 
            // splitContainer2.Panel1
            // 
            this.splitContainer2.Panel1.Controls.Add(this.labProjectNum);
            this.splitContainer2.Panel1.Controls.Add(this.label3);
            // 
            // splitContainer2.Panel2
            // 
            this.splitContainer2.Panel2.Controls.Add(this.lvProjects);
            this.splitContainer2.Size = new System.Drawing.Size(946, 457);
            this.splitContainer2.SplitterDistance = 29;
            this.splitContainer2.SplitterWidth = 1;
            this.splitContainer2.TabIndex = 1;
            // 
            // btnSelectGitDir
            // 
            this.btnSelectGitDir.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.btnSelectGitDir.Location = new System.Drawing.Point(594, 33);
            this.btnSelectGitDir.Name = "btnSelectGitDir";
            this.btnSelectGitDir.Size = new System.Drawing.Size(75, 23);
            this.btnSelectGitDir.TabIndex = 5;
            this.btnSelectGitDir.Text = "选择目录..";
            this.btnSelectGitDir.UseVisualStyleBackColor = true;
            this.btnSelectGitDir.Click += new System.EventHandler(this.btnSelectGitDir_Click);
            // 
            // txtGitDir
            // 
            this.txtGitDir.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.txtGitDir.Location = new System.Drawing.Point(85, 34);
            this.txtGitDir.Name = "txtGitDir";
            this.txtGitDir.Size = new System.Drawing.Size(508, 21);
            this.txtGitDir.TabIndex = 4;
            this.txtGitDir.Text = "D:\\";
            // 
            // labProjectNum
            // 
            this.labProjectNum.AutoSize = true;
            this.labProjectNum.Font = new System.Drawing.Font("宋体", 9F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.labProjectNum.ForeColor = System.Drawing.Color.Red;
            this.labProjectNum.Location = new System.Drawing.Point(68, 10);
            this.labProjectNum.Name = "labProjectNum";
            this.labProjectNum.Size = new System.Drawing.Size(12, 12);
            this.labProjectNum.TabIndex = 1;
            this.labProjectNum.Text = "0";
            // 
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.Location = new System.Drawing.Point(24, 37);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(65, 12);
            this.label4.TabIndex = 3;
            this.label4.Text = "本地目录：";
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Location = new System.Drawing.Point(12, 10);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(65, 12);
            this.label3.TabIndex = 0;
            this.label3.Text = "项目个数：";
            // 
            // btnGitClone
            // 
            this.btnGitClone.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.btnGitClone.Enabled = false;
            this.btnGitClone.Location = new System.Drawing.Point(841, 33);
            this.btnGitClone.Name = "btnGitClone";
            this.btnGitClone.Size = new System.Drawing.Size(93, 23);
            this.btnGitClone.TabIndex = 2;
            this.btnGitClone.Text = "克隆全部项目";
            this.btnGitClone.UseVisualStyleBackColor = true;
            this.btnGitClone.Click += new System.EventHandler(this.btnGitClone_Click);
            // 
            // lvProjects
            // 
            this.lvProjects.Columns.AddRange(new System.Windows.Forms.ColumnHeader[] {
            this.columnHeader1,
            this.columnHeader2,
            this.columnHeader3,
            this.columnHeader4});
            this.lvProjects.Dock = System.Windows.Forms.DockStyle.Fill;
            this.lvProjects.HideSelection = false;
            this.lvProjects.Location = new System.Drawing.Point(0, 0);
            this.lvProjects.Name = "lvProjects";
            this.lvProjects.Size = new System.Drawing.Size(946, 427);
            this.lvProjects.TabIndex = 0;
            this.lvProjects.UseCompatibleStateImageBehavior = false;
            this.lvProjects.View = System.Windows.Forms.View.Details;
            this.lvProjects.ColumnClick += new System.Windows.Forms.ColumnClickEventHandler(this.lvProjects_ColumnClick);
            this.lvProjects.MouseDoubleClick += new System.Windows.Forms.MouseEventHandler(this.lvProjects_MouseDoubleClick);
            // 
            // columnHeader1
            // 
            this.columnHeader1.Text = "Id";
            this.columnHeader1.Width = 65;
            // 
            // columnHeader2
            // 
            this.columnHeader2.Text = "Name";
            this.columnHeader2.Width = 160;
            // 
            // columnHeader3
            // 
            this.columnHeader3.Text = "Git Url";
            this.columnHeader3.Width = 350;
            // 
            // columnHeader4
            // 
            this.columnHeader4.Text = "描述";
            this.columnHeader4.Width = 600;
            // 
            // chkIgnoreExistsProj
            // 
            this.chkIgnoreExistsProj.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.chkIgnoreExistsProj.AutoSize = true;
            this.chkIgnoreExistsProj.Location = new System.Drawing.Point(724, 37);
            this.chkIgnoreExistsProj.Name = "chkIgnoreExistsProj";
            this.chkIgnoreExistsProj.Size = new System.Drawing.Size(108, 16);
            this.chkIgnoreExistsProj.TabIndex = 6;
            this.chkIgnoreExistsProj.Text = "忽略已存在项目";
            this.chkIgnoreExistsProj.UseVisualStyleBackColor = true;
            // 
            // GitlabForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(946, 520);
            this.Controls.Add(this.splitContainer1);
            this.Name = "GitlabForm";
            this.Text = "GitlabForm";
            this.splitContainer1.Panel1.ResumeLayout(false);
            this.splitContainer1.Panel1.PerformLayout();
            this.splitContainer1.Panel2.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.splitContainer1)).EndInit();
            this.splitContainer1.ResumeLayout(false);
            this.splitContainer2.Panel1.ResumeLayout(false);
            this.splitContainer2.Panel1.PerformLayout();
            this.splitContainer2.Panel2.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.splitContainer2)).EndInit();
            this.splitContainer2.ResumeLayout(false);
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.SplitContainer splitContainer1;
        private System.Windows.Forms.Button btnShowGitlab;
        private System.Windows.Forms.TextBox txtGitlabUrl;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.TextBox txtPrivateToken;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.ListView lvProjects;
        private System.Windows.Forms.ColumnHeader columnHeader1;
        private System.Windows.Forms.ColumnHeader columnHeader2;
        private System.Windows.Forms.ColumnHeader columnHeader3;
        private System.Windows.Forms.ColumnHeader columnHeader4;
        private System.Windows.Forms.SplitContainer splitContainer2;
        private System.Windows.Forms.Label labProjectNum;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.Button btnSelectGitDir;
        private System.Windows.Forms.TextBox txtGitDir;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.Button btnGitClone;
        private System.Windows.Forms.CheckBox chkIgnoreExistsProj;
    }
}