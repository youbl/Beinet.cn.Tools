using Beinet.cn.Tools.ControlExt;

namespace Beinet.cn.Tools.SqlInjectForm
{
    partial class SqlInject
    {
        /// <summary>
        /// 必需的设计器变量。
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// 清理所有正在使用的资源。
        /// </summary>
        /// <param name="disposing">如果应释放托管资源，为 true；否则为 false。</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows 窗体设计器生成的代码

        /// <summary>
        /// 设计器支持所需的方法 - 不要
        /// 使用代码编辑器修改此方法的内容。
        /// </summary>
        private void InitializeComponent()
        {
            this.textBox1 = new System.Windows.Forms.TextBox();
            this.button1 = new System.Windows.Forms.Button();
            this.button2 = new System.Windows.Forms.Button();
            this.txtExts = new System.Windows.Forms.TextBox();
            this.label1 = new System.Windows.Forms.Label();
            this.splitContainer1 = new System.Windows.Forms.SplitContainer();
            this.labStatus = new System.Windows.Forms.Label();
            this.lvNoErr = new ListViewNF();
            this.colSn = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.colFile = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.lvErr = new ListViewNF();
            this.colSnErr = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.colFileErr = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.colErrDetail = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.colErrRow = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.colErrOp = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.label2 = new System.Windows.Forms.Label();
            this.labRet = new System.Windows.Forms.Label();
            ((System.ComponentModel.ISupportInitialize)(this.splitContainer1)).BeginInit();
            this.splitContainer1.Panel1.SuspendLayout();
            this.splitContainer1.Panel2.SuspendLayout();
            this.splitContainer1.SuspendLayout();
            this.SuspendLayout();
            // 
            // textBox1
            // 
            this.textBox1.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.textBox1.Location = new System.Drawing.Point(2, 3);
            this.textBox1.Name = "textBox1";
            this.textBox1.Size = new System.Drawing.Size(549, 21);
            this.textBox1.TabIndex = 0;
            this.textBox1.Text = "e:\\";
            // 
            // button1
            // 
            this.button1.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.button1.Location = new System.Drawing.Point(557, 3);
            this.button1.Name = "button1";
            this.button1.Size = new System.Drawing.Size(59, 23);
            this.button1.TabIndex = 1;
            this.button1.Text = "浏览..";
            this.button1.UseVisualStyleBackColor = true;
            this.button1.Click += new System.EventHandler(this.button1_Click);
            // 
            // button2
            // 
            this.button2.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.button2.Location = new System.Drawing.Point(622, 3);
            this.button2.Name = "button2";
            this.button2.Size = new System.Drawing.Size(75, 23);
            this.button2.TabIndex = 3;
            this.button2.Text = "开始扫描";
            this.button2.UseVisualStyleBackColor = true;
            this.button2.Click += new System.EventHandler(this.button2_Click);
            // 
            // txtExts
            // 
            this.txtExts.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.txtExts.Location = new System.Drawing.Point(45, 27);
            this.txtExts.Name = "txtExts";
            this.txtExts.Size = new System.Drawing.Size(125, 21);
            this.txtExts.TabIndex = 2;
            this.txtExts.Text = "aspx,cs,";
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(2, 31);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(47, 12);
            this.label1.TabIndex = 5;
            this.label1.Text = "扩展名:";
            // 
            // splitContainer1
            // 
            this.splitContainer1.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.splitContainer1.Location = new System.Drawing.Point(2, 54);
            this.splitContainer1.Margin = new System.Windows.Forms.Padding(1);
            this.splitContainer1.Name = "splitContainer1";
            this.splitContainer1.Orientation = System.Windows.Forms.Orientation.Horizontal;
            // 
            // splitContainer1.Panel1
            // 
            this.splitContainer1.Panel1.Controls.Add(this.labStatus);
            this.splitContainer1.Panel1.Controls.Add(this.lvNoErr);
            // 
            // splitContainer1.Panel2
            // 
            this.splitContainer1.Panel2.Controls.Add(this.lvErr);
            this.splitContainer1.Size = new System.Drawing.Size(695, 442);
            this.splitContainer1.SplitterDistance = 221;
            this.splitContainer1.SplitterWidth = 1;
            this.splitContainer1.TabIndex = 1;
            this.splitContainer1.TabStop = false;
            // 
            // labStatus
            // 
            this.labStatus.AutoSize = true;
            this.labStatus.Font = new System.Drawing.Font("宋体", 24F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.labStatus.ForeColor = System.Drawing.Color.Red;
            this.labStatus.Location = new System.Drawing.Point(96, 109);
            this.labStatus.Name = "labStatus";
            this.labStatus.Size = new System.Drawing.Size(114, 33);
            this.labStatus.TabIndex = 6;
            this.labStatus.Text = "状态栏";
            this.labStatus.Visible = false;
            // 
            // lvNoErr
            // 
            this.lvNoErr.Columns.AddRange(new System.Windows.Forms.ColumnHeader[] {
            this.colSn,
            this.colFile});
            this.lvNoErr.Dock = System.Windows.Forms.DockStyle.Fill;
            this.lvNoErr.FullRowSelect = true;
            this.lvNoErr.GridLines = true;
            this.lvNoErr.HeaderStyle = System.Windows.Forms.ColumnHeaderStyle.Nonclickable;
            this.lvNoErr.HideSelection = false;
            this.lvNoErr.Location = new System.Drawing.Point(0, 0);
            this.lvNoErr.MultiSelect = false;
            this.lvNoErr.Name = "lvNoErr";
            this.lvNoErr.Size = new System.Drawing.Size(695, 221);
            this.lvNoErr.TabIndex = 0;
            this.lvNoErr.TabStop = false;
            this.lvNoErr.UseCompatibleStateImageBehavior = false;
            this.lvNoErr.View = System.Windows.Forms.View.Details;
            this.lvNoErr.MouseDoubleClick += new System.Windows.Forms.MouseEventHandler(this.lvNoErr_MouseDoubleClick);
            // 
            // colSn
            // 
            this.colSn.Text = "sn";
            this.colSn.Width = 50;
            // 
            // colFile
            // 
            this.colFile.Text = "文件名";
            this.colFile.Width = 620;
            // 
            // lvErr
            // 
            this.lvErr.Columns.AddRange(new System.Windows.Forms.ColumnHeader[] {
            this.colSnErr,
            this.colFileErr,
            this.colErrDetail,
            this.colErrRow,
            this.colErrOp});
            this.lvErr.Dock = System.Windows.Forms.DockStyle.Fill;
            this.lvErr.FullRowSelect = true;
            this.lvErr.GridLines = true;
            this.lvErr.HeaderStyle = System.Windows.Forms.ColumnHeaderStyle.Nonclickable;
            this.lvErr.HideSelection = false;
            this.lvErr.Location = new System.Drawing.Point(0, 0);
            this.lvErr.MultiSelect = false;
            this.lvErr.Name = "lvErr";
            this.lvErr.Size = new System.Drawing.Size(695, 220);
            this.lvErr.TabIndex = 0;
            this.lvErr.TabStop = false;
            this.lvErr.UseCompatibleStateImageBehavior = false;
            this.lvErr.View = System.Windows.Forms.View.Details;
            this.lvErr.MouseClick += new System.Windows.Forms.MouseEventHandler(this.lvErr_MouseClick);
            this.lvErr.MouseDoubleClick += new System.Windows.Forms.MouseEventHandler(this.lvErr_MouseDoubleClick);
            // 
            // colSnErr
            // 
            this.colSnErr.Text = "sn";
            this.colSnErr.Width = 50;
            // 
            // colFileErr
            // 
            this.colFileErr.Text = "文件名";
            this.colFileErr.Width = 300;
            // 
            // colErrDetail
            // 
            this.colErrDetail.Text = "拼接详情";
            this.colErrDetail.Width = 200;
            // 
            // colErrRow
            // 
            this.colErrRow.Text = "行号";
            this.colErrRow.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
            this.colErrRow.Width = 50;
            // 
            // colErrOp
            // 
            this.colErrOp.Text = "操作";
            this.colErrOp.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
            this.colErrOp.Width = 90;
            // 
            // label2
            // 
            this.label2.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.label2.AutoSize = true;
            this.label2.Font = new System.Drawing.Font("宋体", 9F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.label2.ForeColor = System.Drawing.Color.Red;
            this.label2.Location = new System.Drawing.Point(176, 31);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(174, 12);
            this.label2.TabIndex = 6;
            this.label2.Text = "双击可以查看详情或打开文件";
            // 
            // labRet
            // 
            this.labRet.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.labRet.AutoSize = true;
            this.labRet.Font = new System.Drawing.Font("宋体", 9F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.labRet.ForeColor = System.Drawing.Color.Blue;
            this.labRet.Location = new System.Drawing.Point(513, 31);
            this.labRet.Name = "labRet";
            this.labRet.Size = new System.Drawing.Size(0, 12);
            this.labRet.TabIndex = 6;
            // 
            // SqlInject
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(699, 497);
            this.Controls.Add(this.labRet);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.splitContainer1);
            this.Controls.Add(this.txtExts);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.button2);
            this.Controls.Add(this.button1);
            this.Controls.Add(this.textBox1);
            this.Name = "SqlInject";
            this.Text = "Sql拼接检查";
            this.splitContainer1.Panel1.ResumeLayout(false);
            this.splitContainer1.Panel1.PerformLayout();
            this.splitContainer1.Panel2.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.splitContainer1)).EndInit();
            this.splitContainer1.ResumeLayout(false);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.TextBox textBox1;
        private System.Windows.Forms.Button button1;
        private System.Windows.Forms.Button button2;
        private System.Windows.Forms.TextBox txtExts;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.SplitContainer splitContainer1;
        private ListViewNF lvNoErr;
        private ListViewNF lvErr;
        private System.Windows.Forms.ColumnHeader colSn;
        private System.Windows.Forms.ColumnHeader colFile;
        private System.Windows.Forms.ColumnHeader colSnErr;
        private System.Windows.Forms.ColumnHeader colFileErr;
        private System.Windows.Forms.ColumnHeader colErrDetail;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.ColumnHeader colErrRow;
        private System.Windows.Forms.Label labStatus;
        private System.Windows.Forms.ColumnHeader colErrOp;
        private System.Windows.Forms.Label labRet;
    }
}

