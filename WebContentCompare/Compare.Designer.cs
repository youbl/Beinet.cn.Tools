namespace Beinet.cn.Tools.WebContentCompare
{
    partial class Compare
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
            this.txtCompareIp = new System.Windows.Forms.TextBox();
            this.label2 = new System.Windows.Forms.Label();
            this.txtPublishServer = new System.Windows.Forms.TextBox();
            this.btnLoad = new System.Windows.Forms.Button();
            this.btnSave = new System.Windows.Forms.Button();
            this.btnCompare = new System.Windows.Forms.Button();
            this.lvUrls = new System.Windows.Forms.DataGridView();
            this.splitContainer1 = new System.Windows.Forms.SplitContainer();
            this.colUrl = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.colPost = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.colResult = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.colDel = new System.Windows.Forms.DataGridViewLinkColumn();
            this.colOpenDir = new System.Windows.Forms.DataGridViewLinkColumn();
            this.linkLabel1 = new System.Windows.Forms.LinkLabel();
            this.label5 = new System.Windows.Forms.Label();
            this.lstRet = new System.Windows.Forms.ListView();
            this.colRetSn = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.colRetContent = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.label3 = new System.Windows.Forms.Label();
            this.txtThreadNum = new System.Windows.Forms.TextBox();
            ((System.ComponentModel.ISupportInitialize)(this.lvUrls)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.splitContainer1)).BeginInit();
            this.splitContainer1.Panel1.SuspendLayout();
            this.splitContainer1.Panel2.SuspendLayout();
            this.splitContainer1.SuspendLayout();
            this.SuspendLayout();
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(24, 13);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(95, 12);
            this.label1.TabIndex = 0;
            this.label1.Text = "对比源服务器IP:";
            // 
            // txtCompareIp
            // 
            this.txtCompareIp.Location = new System.Drawing.Point(126, 10);
            this.txtCompareIp.Name = "txtCompareIp";
            this.txtCompareIp.Size = new System.Drawing.Size(95, 21);
            this.txtCompareIp.TabIndex = 1;
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(12, 44);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(107, 12);
            this.label2.TabIndex = 0;
            this.label2.Text = "目标服务器IP列表:";
            // 
            // txtPublishServer
            // 
            this.txtPublishServer.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.txtPublishServer.Location = new System.Drawing.Point(125, 41);
            this.txtPublishServer.Name = "txtPublishServer";
            this.txtPublishServer.Size = new System.Drawing.Size(391, 21);
            this.txtPublishServer.TabIndex = 1;
            // 
            // btnLoad
            // 
            this.btnLoad.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.btnLoad.Location = new System.Drawing.Point(244, 8);
            this.btnLoad.Name = "btnLoad";
            this.btnLoad.Size = new System.Drawing.Size(134, 23);
            this.btnLoad.TabIndex = 3;
            this.btnLoad.Text = "从配置文件加载..";
            this.btnLoad.UseVisualStyleBackColor = true;
            this.btnLoad.Click += new System.EventHandler(this.btnLoad_Click);
            // 
            // btnSave
            // 
            this.btnSave.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.btnSave.Location = new System.Drawing.Point(384, 8);
            this.btnSave.Name = "btnSave";
            this.btnSave.Size = new System.Drawing.Size(134, 23);
            this.btnSave.TabIndex = 3;
            this.btnSave.Text = "保存全部到配置文件..";
            this.btnSave.UseVisualStyleBackColor = true;
            this.btnSave.Click += new System.EventHandler(this.btnSave_Click);
            // 
            // btnCompare
            // 
            this.btnCompare.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.btnCompare.Location = new System.Drawing.Point(677, 9);
            this.btnCompare.Name = "btnCompare";
            this.btnCompare.Size = new System.Drawing.Size(91, 53);
            this.btnCompare.TabIndex = 4;
            this.btnCompare.Text = "开始检查";
            this.btnCompare.UseVisualStyleBackColor = true;
            this.btnCompare.Click += new System.EventHandler(this.btnCompare_Click);
            // 
            // lvUrls
            // 
            this.lvUrls.AutoSizeColumnsMode = System.Windows.Forms.DataGridViewAutoSizeColumnsMode.Fill;
            this.lvUrls.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.lvUrls.Columns.AddRange(new System.Windows.Forms.DataGridViewColumn[] {
            this.colUrl,
            this.colPost,
            this.colResult,
            this.colDel,
            this.colOpenDir});
            this.lvUrls.Dock = System.Windows.Forms.DockStyle.Fill;
            this.lvUrls.EditMode = System.Windows.Forms.DataGridViewEditMode.EditOnEnter;
            this.lvUrls.Location = new System.Drawing.Point(0, 0);
            this.lvUrls.MultiSelect = false;
            this.lvUrls.Name = "lvUrls";
            this.lvUrls.RowHeadersWidth = 60;
            this.lvUrls.RowTemplate.Height = 23;
            this.lvUrls.Size = new System.Drawing.Size(767, 326);
            this.lvUrls.TabIndex = 5;
            this.lvUrls.CellClick += new System.Windows.Forms.DataGridViewCellEventHandler(this.lvUrls_CellClick);
            this.lvUrls.CurrentCellDirtyStateChanged += new System.EventHandler(this.lvUrls_CurrentCellDirtyStateChanged);
            this.lvUrls.RowsAdded += new System.Windows.Forms.DataGridViewRowsAddedEventHandler(this.dataGridView1_RowsAdded);
            this.lvUrls.RowsRemoved += new System.Windows.Forms.DataGridViewRowsRemovedEventHandler(this.dataGridView1_RowsRemoved);
            // 
            // splitContainer1
            // 
            this.splitContainer1.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.splitContainer1.Location = new System.Drawing.Point(1, 99);
            this.splitContainer1.Name = "splitContainer1";
            this.splitContainer1.Orientation = System.Windows.Forms.Orientation.Horizontal;
            // 
            // splitContainer1.Panel1
            // 
            this.splitContainer1.Panel1.Controls.Add(this.lvUrls);
            this.splitContainer1.Panel1MinSize = 1;
            // 
            // splitContainer1.Panel2
            // 
            this.splitContainer1.Panel2.Controls.Add(this.lstRet);
            this.splitContainer1.Panel2MinSize = 1;
            this.splitContainer1.Size = new System.Drawing.Size(767, 449);
            this.splitContainer1.SplitterDistance = 326;
            this.splitContainer1.SplitterWidth = 1;
            this.splitContainer1.TabIndex = 1;
            this.splitContainer1.TabStop = false;
            // 
            // colUrl
            // 
            this.colUrl.FillWeight = 50F;
            this.colUrl.HeaderText = "Url";
            this.colUrl.Name = "colUrl";
            // 
            // colPost
            // 
            this.colPost.FillWeight = 20F;
            this.colPost.HeaderText = "Post数据";
            this.colPost.Name = "colPost";
            // 
            // colResult
            // 
            this.colResult.FillWeight = 30F;
            this.colResult.HeaderText = "对比结果";
            this.colResult.Name = "colResult";
            // 
            // colDel
            // 
            this.colDel.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.None;
            this.colDel.FillWeight = 1F;
            this.colDel.HeaderText = "";
            this.colDel.Name = "colDel";
            this.colDel.ReadOnly = true;
            this.colDel.Resizable = System.Windows.Forms.DataGridViewTriState.False;
            this.colDel.Width = 35;
            // 
            // colOpenDir
            // 
            this.colOpenDir.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.None;
            this.colOpenDir.FillWeight = 1F;
            this.colOpenDir.HeaderText = "";
            this.colOpenDir.Name = "colOpenDir";
            this.colOpenDir.ReadOnly = true;
            this.colOpenDir.Resizable = System.Windows.Forms.DataGridViewTriState.False;
            this.colOpenDir.Width = 60;
            // 
            // linkLabel1
            // 
            this.linkLabel1.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.linkLabel1.AutoSize = true;
            this.linkLabel1.Location = new System.Drawing.Point(606, 8);
            this.linkLabel1.Name = "linkLabel1";
            this.linkLabel1.Size = new System.Drawing.Size(65, 12);
            this.linkLabel1.TabIndex = 5;
            this.linkLabel1.TabStop = true;
            this.linkLabel1.Text = "快捷键说明";
            this.linkLabel1.LinkClicked += new System.Windows.Forms.LinkLabelLinkClickedEventHandler(this.linkLabel1_LinkClicked);
            // 
            // label5
            // 
            this.label5.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.label5.AutoSize = true;
            this.label5.Font = new System.Drawing.Font("宋体", 9F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.label5.ForeColor = System.Drawing.SystemColors.ActiveCaption;
            this.label5.Location = new System.Drawing.Point(6, 67);
            this.label5.Name = "label5";
            this.label5.Size = new System.Drawing.Size(747, 24);
            this.label5.TabIndex = 6;
            this.label5.Text = "注1：对比源的IP只能填写1个；目标服务器IP可以多个，分号分隔\r\n注2：结果OK表示对比完全一致，“不一致ip：121.207.240.199(4,4140)”" +
    "里的4,4140表示第1个不同点在第4行,第4140个字符";
            // 
            // lstRet
            // 
            this.lstRet.Columns.AddRange(new System.Windows.Forms.ColumnHeader[] {
            this.colRetSn,
            this.colRetContent});
            this.lstRet.Dock = System.Windows.Forms.DockStyle.Fill;
            this.lstRet.FullRowSelect = true;
            this.lstRet.GridLines = true;
            this.lstRet.HideSelection = false;
            this.lstRet.LabelEdit = true;
            this.lstRet.Location = new System.Drawing.Point(0, 0);
            this.lstRet.MultiSelect = false;
            this.lstRet.Name = "lstRet";
            this.lstRet.Size = new System.Drawing.Size(767, 122);
            this.lstRet.TabIndex = 0;
            this.lstRet.UseCompatibleStateImageBehavior = false;
            this.lstRet.View = System.Windows.Forms.View.Details;
            this.lstRet.SelectedIndexChanged += new System.EventHandler(this.lstRet_SelectedIndexChanged);
            // 
            // colRetSn
            // 
            this.colRetSn.Text = "出错行号";
            // 
            // colRetContent
            // 
            this.colRetContent.Text = "内容描述";
            this.colRetContent.Width = 1000;
            // 
            // label3
            // 
            this.label3.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.label3.AutoSize = true;
            this.label3.Location = new System.Drawing.Point(522, 44);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(95, 12);
            this.label3.TabIndex = 0;
            this.label3.Text = "启动检查线程数:";
            // 
            // txtThreadNum
            // 
            this.txtThreadNum.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.txtThreadNum.Location = new System.Drawing.Point(617, 41);
            this.txtThreadNum.Name = "txtThreadNum";
            this.txtThreadNum.Size = new System.Drawing.Size(48, 21);
            this.txtThreadNum.TabIndex = 7;
            this.txtThreadNum.Text = "5";
            this.txtThreadNum.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
            // 
            // Compare
            // 
            this.AllowDrop = true;
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(770, 550);
            this.Controls.Add(this.txtThreadNum);
            this.Controls.Add(this.label5);
            this.Controls.Add(this.linkLabel1);
            this.Controls.Add(this.splitContainer1);
            this.Controls.Add(this.btnCompare);
            this.Controls.Add(this.btnSave);
            this.Controls.Add(this.btnLoad);
            this.Controls.Add(this.txtPublishServer);
            this.Controls.Add(this.txtCompareIp);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.label3);
            this.Controls.Add(this.label1);
            this.KeyPreview = true;
            this.Name = "Compare";
            this.Text = "Compare";
            this.DragDrop += new System.Windows.Forms.DragEventHandler(this.Compare_DragDrop);
            this.DragEnter += new System.Windows.Forms.DragEventHandler(this.Compare_DragEnter);
            this.KeyDown += new System.Windows.Forms.KeyEventHandler(this.Compare_KeyDown);
            ((System.ComponentModel.ISupportInitialize)(this.lvUrls)).EndInit();
            this.splitContainer1.Panel1.ResumeLayout(false);
            this.splitContainer1.Panel2.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.splitContainer1)).EndInit();
            this.splitContainer1.ResumeLayout(false);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.TextBox txtCompareIp;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.TextBox txtPublishServer;
        private System.Windows.Forms.Button btnLoad;
        private System.Windows.Forms.Button btnSave;
        private System.Windows.Forms.Button btnCompare;
        private System.Windows.Forms.DataGridView lvUrls;
        private System.Windows.Forms.SplitContainer splitContainer1;
        private System.Windows.Forms.DataGridViewTextBoxColumn colUrl;
        private System.Windows.Forms.DataGridViewTextBoxColumn colPost;
        private System.Windows.Forms.DataGridViewTextBoxColumn colResult;
        private System.Windows.Forms.DataGridViewLinkColumn colDel;
        private System.Windows.Forms.DataGridViewLinkColumn colOpenDir;
        private System.Windows.Forms.LinkLabel linkLabel1;
        private System.Windows.Forms.Label label5;
        private System.Windows.Forms.ListView lstRet;
        private System.Windows.Forms.ColumnHeader colRetSn;
        private System.Windows.Forms.ColumnHeader colRetContent;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.TextBox txtThreadNum;
    }
}