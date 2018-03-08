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
            this.colUrl = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.colPost = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.colReg = new System.Windows.Forms.DataGridViewLinkColumn();
            this.colResult = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.colDel = new System.Windows.Forms.DataGridViewLinkColumn();
            this.colOpenDir = new System.Windows.Forms.DataGridViewLinkColumn();
            this.colReCompare = new System.Windows.Forms.DataGridViewLinkColumn();
            this.splitContainer1 = new System.Windows.Forms.SplitContainer();
            this.lstRet = new System.Windows.Forms.ListView();
            this.colRetSn = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.colRetContent = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.linkLabel1 = new System.Windows.Forms.LinkLabel();
            this.label5 = new System.Windows.Forms.Label();
            this.label3 = new System.Windows.Forms.Label();
            this.txtThreadNum = new System.Windows.Forms.TextBox();
            this.txtLoadSpeed = new System.Windows.Forms.TextBox();
            this.label4 = new System.Windows.Forms.Label();
            this.label6 = new System.Windows.Forms.Label();
            this.btnRemoveOK = new System.Windows.Forms.Button();
            this.lnkReg = new System.Windows.Forms.LinkLabel();
            this.label7 = new System.Windows.Forms.Label();
            this.btnRemoveUrlLike = new System.Windows.Forms.Button();
            this.btnRemoveAllLike = new System.Windows.Forms.Button();
            this.label8 = new System.Windows.Forms.Label();
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
            this.label1.Location = new System.Drawing.Point(14, 9);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(71, 12);
            this.label1.TabIndex = 0;
            this.label1.Text = "源服务器IP:";
            // 
            // txtCompareIp
            // 
            this.txtCompareIp.Location = new System.Drawing.Point(85, 6);
            this.txtCompareIp.Name = "txtCompareIp";
            this.txtCompareIp.Size = new System.Drawing.Size(95, 21);
            this.txtCompareIp.TabIndex = 1;
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(186, 9);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(107, 12);
            this.label2.TabIndex = 0;
            this.label2.Text = "目标服务器IP列表:";
            // 
            // txtPublishServer
            // 
            this.txtPublishServer.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.txtPublishServer.Location = new System.Drawing.Point(294, 6);
            this.txtPublishServer.Name = "txtPublishServer";
            this.txtPublishServer.Size = new System.Drawing.Size(391, 21);
            this.txtPublishServer.TabIndex = 2;
            // 
            // btnLoad
            // 
            this.btnLoad.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.btnLoad.Font = new System.Drawing.Font("宋体", 9F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.btnLoad.Location = new System.Drawing.Point(646, 105);
            this.btnLoad.Name = "btnLoad";
            this.btnLoad.Size = new System.Drawing.Size(118, 23);
            this.btnLoad.TabIndex = 7;
            this.btnLoad.Text = "从配置文件加载..";
            this.btnLoad.UseVisualStyleBackColor = true;
            this.btnLoad.Click += new System.EventHandler(this.btnLoad_Click);
            // 
            // btnSave
            // 
            this.btnSave.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.btnSave.Location = new System.Drawing.Point(646, 79);
            this.btnSave.Name = "btnSave";
            this.btnSave.Size = new System.Drawing.Size(118, 23);
            this.btnSave.TabIndex = 6;
            this.btnSave.Text = "保存配置到文件..";
            this.btnSave.UseVisualStyleBackColor = true;
            this.btnSave.Click += new System.EventHandler(this.btnSave_Click);
            // 
            // btnCompare
            // 
            this.btnCompare.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.btnCompare.Location = new System.Drawing.Point(699, 9);
            this.btnCompare.Name = "btnCompare";
            this.btnCompare.Size = new System.Drawing.Size(69, 53);
            this.btnCompare.TabIndex = 5;
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
            this.colReg,
            this.colResult,
            this.colDel,
            this.colOpenDir,
            this.colReCompare});
            this.lvUrls.Dock = System.Windows.Forms.DockStyle.Fill;
            this.lvUrls.EditMode = System.Windows.Forms.DataGridViewEditMode.EditOnEnter;
            this.lvUrls.Location = new System.Drawing.Point(0, 0);
            this.lvUrls.MultiSelect = false;
            this.lvUrls.Name = "lvUrls";
            this.lvUrls.RowHeadersWidth = 60;
            this.lvUrls.RowTemplate.Height = 23;
            this.lvUrls.Size = new System.Drawing.Size(767, 300);
            this.lvUrls.TabIndex = 12;
            this.lvUrls.CellClick += new System.Windows.Forms.DataGridViewCellEventHandler(this.lvUrls_CellClick);
            this.lvUrls.CurrentCellDirtyStateChanged += new System.EventHandler(this.lvUrls_CurrentCellDirtyStateChanged);
            this.lvUrls.RowsAdded += new System.Windows.Forms.DataGridViewRowsAddedEventHandler(this.dataGridView1_RowsAdded);
            this.lvUrls.RowsRemoved += new System.Windows.Forms.DataGridViewRowsRemovedEventHandler(this.dataGridView1_RowsRemoved);
            // 
            // colUrl
            // 
            this.colUrl.FillWeight = 88.23532F;
            this.colUrl.HeaderText = "Url";
            this.colUrl.Name = "colUrl";
            // 
            // colPost
            // 
            this.colPost.FillWeight = 64.3811F;
            this.colPost.HeaderText = "Post数据";
            this.colPost.Name = "colPost";
            // 
            // colReg
            // 
            this.colReg.FillWeight = 22F;
            this.colReg.HeaderText = "结果替换";
            this.colReg.Name = "colReg";
            // 
            // colResult
            // 
            this.colResult.FillWeight = 35.53344F;
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
            this.colOpenDir.Width = 35;
            // 
            // colReCompare
            // 
            this.colReCompare.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.None;
            this.colReCompare.FillWeight = 1F;
            this.colReCompare.HeaderText = "";
            this.colReCompare.Name = "colReCompare";
            this.colReCompare.ReadOnly = true;
            this.colReCompare.Resizable = System.Windows.Forms.DataGridViewTriState.False;
            this.colReCompare.Width = 35;
            // 
            // splitContainer1
            // 
            this.splitContainer1.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.splitContainer1.Location = new System.Drawing.Point(1, 133);
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
            this.splitContainer1.Size = new System.Drawing.Size(767, 415);
            this.splitContainer1.SplitterDistance = 300;
            this.splitContainer1.SplitterWidth = 1;
            this.splitContainer1.TabIndex = 0;
            this.splitContainer1.TabStop = false;
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
            this.lstRet.Size = new System.Drawing.Size(767, 114);
            this.lstRet.TabIndex = 0;
            this.lstRet.TabStop = false;
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
            // linkLabel1
            // 
            this.linkLabel1.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.linkLabel1.AutoSize = true;
            this.linkLabel1.Location = new System.Drawing.Point(628, 36);
            this.linkLabel1.Name = "linkLabel1";
            this.linkLabel1.Size = new System.Drawing.Size(65, 12);
            this.linkLabel1.TabIndex = 0;
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
            this.label5.Location = new System.Drawing.Point(6, 60);
            this.label5.Name = "label5";
            this.label5.Size = new System.Drawing.Size(398, 12);
            this.label5.TabIndex = 0;
            this.label5.Text = "注1：源服务器只允许1个IP；目标服务器允许多个IP，半角逗号分隔";
            // 
            // label3
            // 
            this.label3.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.label3.AutoSize = true;
            this.label3.Location = new System.Drawing.Point(152, 36);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(95, 12);
            this.label3.TabIndex = 0;
            this.label3.Text = "启动检查线程数:";
            // 
            // txtThreadNum
            // 
            this.txtThreadNum.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.txtThreadNum.Location = new System.Drawing.Point(247, 33);
            this.txtThreadNum.Name = "txtThreadNum";
            this.txtThreadNum.Size = new System.Drawing.Size(48, 21);
            this.txtThreadNum.TabIndex = 6;
            this.txtThreadNum.Text = "4";
            this.txtThreadNum.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
            // 
            // txtLoadSpeed
            // 
            this.txtLoadSpeed.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.txtLoadSpeed.Location = new System.Drawing.Point(73, 33);
            this.txtLoadSpeed.Name = "txtLoadSpeed";
            this.txtLoadSpeed.Size = new System.Drawing.Size(32, 21);
            this.txtLoadSpeed.TabIndex = 3;
            this.txtLoadSpeed.Text = "30";
            this.txtLoadSpeed.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
            // 
            // label4
            // 
            this.label4.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.label4.AutoSize = true;
            this.label4.Location = new System.Drawing.Point(14, 36);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(59, 12);
            this.label4.TabIndex = 0;
            this.label4.Text = "加载速度:";
            // 
            // label6
            // 
            this.label6.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.label6.AutoSize = true;
            this.label6.Location = new System.Drawing.Point(105, 36);
            this.label6.Name = "label6";
            this.label6.Size = new System.Drawing.Size(29, 12);
            this.label6.TabIndex = 0;
            this.label6.Text = "毫秒";
            // 
            // btnRemoveOK
            // 
            this.btnRemoveOK.Location = new System.Drawing.Point(305, 107);
            this.btnRemoveOK.Name = "btnRemoveOK";
            this.btnRemoveOK.Size = new System.Drawing.Size(153, 23);
            this.btnRemoveOK.TabIndex = 10;
            this.btnRemoveOK.Text = "移除对比结果OK的所有行";
            this.btnRemoveOK.UseVisualStyleBackColor = true;
            this.btnRemoveOK.Click += new System.EventHandler(this.btnRemoveOK_Click);
            // 
            // lnkReg
            // 
            this.lnkReg.AutoSize = true;
            this.lnkReg.Location = new System.Drawing.Point(464, 112);
            this.lnkReg.Name = "lnkReg";
            this.lnkReg.Size = new System.Drawing.Size(77, 12);
            this.lnkReg.TabIndex = 11;
            this.lnkReg.TabStop = true;
            this.lnkReg.Text = "全局正则替换";
            this.lnkReg.LinkClicked += new System.Windows.Forms.LinkLabelLinkClickedEventHandler(this.lnkReg_LinkClicked);
            // 
            // label7
            // 
            this.label7.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.label7.AutoSize = true;
            this.label7.Font = new System.Drawing.Font("宋体", 9F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.label7.ForeColor = System.Drawing.SystemColors.ActiveCaption;
            this.label7.Location = new System.Drawing.Point(6, 90);
            this.label7.Name = "label7";
            this.label7.Size = new System.Drawing.Size(626, 12);
            this.label7.TabIndex = 0;
            this.label7.Text = "注3：\"全局正则替换\":把所有返回结果进行替换，比如时间会影响对比；\"结果替换\":只替换当前行返回结果";
            // 
            // btnRemoveUrlLike
            // 
            this.btnRemoveUrlLike.Location = new System.Drawing.Point(4, 107);
            this.btnRemoveUrlLike.Name = "btnRemoveUrlLike";
            this.btnRemoveUrlLike.Size = new System.Drawing.Size(115, 23);
            this.btnRemoveUrlLike.TabIndex = 8;
            this.btnRemoveUrlLike.Text = "移除Url重复的行";
            this.btnRemoveUrlLike.UseVisualStyleBackColor = true;
            this.btnRemoveUrlLike.Click += new System.EventHandler(this.btnRemoveLike_Click);
            // 
            // btnRemoveAllLike
            // 
            this.btnRemoveAllLike.Location = new System.Drawing.Point(125, 107);
            this.btnRemoveAllLike.Name = "btnRemoveAllLike";
            this.btnRemoveAllLike.Size = new System.Drawing.Size(170, 23);
            this.btnRemoveAllLike.TabIndex = 9;
            this.btnRemoveAllLike.Text = "移除Url和数据同时重复的行";
            this.btnRemoveAllLike.UseVisualStyleBackColor = true;
            this.btnRemoveAllLike.Click += new System.EventHandler(this.btnRemoveLike_Click);
            // 
            // label8
            // 
            this.label8.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.label8.AutoSize = true;
            this.label8.Font = new System.Drawing.Font("宋体", 9F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.label8.ForeColor = System.Drawing.SystemColors.ActiveCaption;
            this.label8.Location = new System.Drawing.Point(6, 75);
            this.label8.Name = "label8";
            this.label8.Size = new System.Drawing.Size(625, 12);
            this.label8.TabIndex = 0;
            this.label8.Text = "注2：OK表示对比一致，“不一致ip：10.2.0.9(4,312)”里的4,312表示第1个不同点在第4行,第312个字符";
            // 
            // Compare
            // 
            this.AllowDrop = true;
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(770, 550);
            this.Controls.Add(this.label8);
            this.Controls.Add(this.label4);
            this.Controls.Add(this.lnkReg);
            this.Controls.Add(this.label6);
            this.Controls.Add(this.txtLoadSpeed);
            this.Controls.Add(this.btnCompare);
            this.Controls.Add(this.txtPublishServer);
            this.Controls.Add(this.txtCompareIp);
            this.Controls.Add(this.txtThreadNum);
            this.Controls.Add(this.label7);
            this.Controls.Add(this.label5);
            this.Controls.Add(this.linkLabel1);
            this.Controls.Add(this.splitContainer1);
            this.Controls.Add(this.btnRemoveAllLike);
            this.Controls.Add(this.btnRemoveUrlLike);
            this.Controls.Add(this.btnRemoveOK);
            this.Controls.Add(this.btnSave);
            this.Controls.Add(this.btnLoad);
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
        private System.Windows.Forms.LinkLabel linkLabel1;
        private System.Windows.Forms.Label label5;
        private System.Windows.Forms.ListView lstRet;
        private System.Windows.Forms.ColumnHeader colRetSn;
        private System.Windows.Forms.ColumnHeader colRetContent;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.TextBox txtThreadNum;
        private System.Windows.Forms.TextBox txtLoadSpeed;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.Label label6;
        private System.Windows.Forms.Button btnRemoveOK;
        private System.Windows.Forms.LinkLabel lnkReg;
        private System.Windows.Forms.Label label7;
        private System.Windows.Forms.Button btnRemoveUrlLike;
        private System.Windows.Forms.Button btnRemoveAllLike;
        private System.Windows.Forms.DataGridViewTextBoxColumn colUrl;
        private System.Windows.Forms.DataGridViewTextBoxColumn colPost;
        private System.Windows.Forms.DataGridViewLinkColumn colReg;
        private System.Windows.Forms.DataGridViewTextBoxColumn colResult;
        private System.Windows.Forms.DataGridViewLinkColumn colDel;
        private System.Windows.Forms.DataGridViewLinkColumn colOpenDir;
        private System.Windows.Forms.DataGridViewLinkColumn colReCompare;
        private System.Windows.Forms.Label label8;
    }
}