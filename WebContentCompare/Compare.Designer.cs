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
            this.label3 = new System.Windows.Forms.Label();
            this.btnLoad = new System.Windows.Forms.Button();
            this.btnSave = new System.Windows.Forms.Button();
            this.btnCompare = new System.Windows.Forms.Button();
            this.lvUrls = new System.Windows.Forms.DataGridView();
            this.txtErr = new System.Windows.Forms.TextBox();
            this.colUrl = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.colPost = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.colResult = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.colDel = new System.Windows.Forms.DataGridViewLinkColumn();
            this.colOpenDir = new System.Windows.Forms.DataGridViewLinkColumn();
            ((System.ComponentModel.ISupportInitialize)(this.lvUrls)).BeginInit();
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
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Font = new System.Drawing.Font("宋体", 9F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.label3.ForeColor = System.Drawing.Color.DarkBlue;
            this.label3.Location = new System.Drawing.Point(227, 9);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(201, 24);
            this.label3.TabIndex = 0;
            this.label3.Text = "对比源的IP只能填写1个\r\n目标服务器IP可以多个，分号分隔";
            // 
            // btnLoad
            // 
            this.btnLoad.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.btnLoad.Location = new System.Drawing.Point(522, 8);
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
            this.btnSave.Location = new System.Drawing.Point(522, 41);
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
            this.lvUrls.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.lvUrls.AutoSizeColumnsMode = System.Windows.Forms.DataGridViewAutoSizeColumnsMode.Fill;
            this.lvUrls.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.lvUrls.Columns.AddRange(new System.Windows.Forms.DataGridViewColumn[] {
            this.colUrl,
            this.colPost,
            this.colResult,
            this.colDel,
            this.colOpenDir});
            this.lvUrls.EditMode = System.Windows.Forms.DataGridViewEditMode.EditOnEnter;
            this.lvUrls.Location = new System.Drawing.Point(4, 68);
            this.lvUrls.Name = "lvUrls";
            this.lvUrls.RowTemplate.Height = 23;
            this.lvUrls.Size = new System.Drawing.Size(764, 419);
            this.lvUrls.TabIndex = 5;
            this.lvUrls.CellClick += new System.Windows.Forms.DataGridViewCellEventHandler(this.lvUrls_CellClick);
            this.lvUrls.CurrentCellDirtyStateChanged += new System.EventHandler(this.lvUrls_CurrentCellDirtyStateChanged);
            this.lvUrls.RowsAdded += new System.Windows.Forms.DataGridViewRowsAddedEventHandler(this.dataGridView1_RowsAdded);
            this.lvUrls.RowsRemoved += new System.Windows.Forms.DataGridViewRowsRemovedEventHandler(this.dataGridView1_RowsRemoved);
            // 
            // txtErr
            // 
            this.txtErr.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.txtErr.ForeColor = System.Drawing.Color.Red;
            this.txtErr.Location = new System.Drawing.Point(4, 493);
            this.txtErr.Multiline = true;
            this.txtErr.Name = "txtErr";
            this.txtErr.ScrollBars = System.Windows.Forms.ScrollBars.Vertical;
            this.txtErr.Size = new System.Drawing.Size(764, 54);
            this.txtErr.TabIndex = 6;
            this.txtErr.Text = "这里显示工作过程中产生的错误信息。";
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
            this.colDel.Width = 40;
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
            // Compare
            // 
            this.AllowDrop = true;
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(770, 550);
            this.Controls.Add(this.txtErr);
            this.Controls.Add(this.lvUrls);
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
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.TextBox txtCompareIp;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.TextBox txtPublishServer;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.Button btnLoad;
        private System.Windows.Forms.Button btnSave;
        private System.Windows.Forms.Button btnCompare;
        private System.Windows.Forms.DataGridView lvUrls;
        private System.Windows.Forms.TextBox txtErr;
        private System.Windows.Forms.DataGridViewTextBoxColumn colUrl;
        private System.Windows.Forms.DataGridViewTextBoxColumn colPost;
        private System.Windows.Forms.DataGridViewTextBoxColumn colResult;
        private System.Windows.Forms.DataGridViewLinkColumn colDel;
        private System.Windows.Forms.DataGridViewLinkColumn colOpenDir;
    }
}