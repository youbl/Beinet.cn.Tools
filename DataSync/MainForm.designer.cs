using Beinet.cn.Tools.ControlExt;

namespace Beinet.cn.Tools.DataSync
{
    partial class MainForm
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
            this.components = new System.ComponentModel.Container();
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(MainForm));
            this.label1 = new System.Windows.Forms.Label();
            this.txtDbSource = new System.Windows.Forms.TextBox();
            this.splitContainer1 = new System.Windows.Forms.SplitContainer();
            this.chkUseTruncate = new System.Windows.Forms.CheckBox();
            this.chkErrContinue = new System.Windows.Forms.CheckBox();
            this.chkAll = new System.Windows.Forms.CheckBox();
            this.btnDelRow = new System.Windows.Forms.Button();
            this.btnAddNewSql = new System.Windows.Forms.Button();
            this.btnGetSchma = new System.Windows.Forms.Button();
            this.btnLoad = new System.Windows.Forms.Button();
            this.btnSaveConfig = new System.Windows.Forms.Button();
            this.btnSyncBegin = new System.Windows.Forms.Button();
            this.label2 = new System.Windows.Forms.Label();
            this.label4 = new System.Windows.Forms.Label();
            this.label3 = new System.Windows.Forms.Label();
            this.txtDbTimeout = new System.Windows.Forms.TextBox();
            this.txtDbTarget = new System.Windows.Forms.TextBox();
            this.lstBoolean = new System.Windows.Forms.ComboBox();
            this.lstTarget = new System.Windows.Forms.ComboBox();
            this.lvTables = new ListViewNF();
            this.colSource = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.colTarget = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.colTruncate = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.colIdentifier = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.imglstForLvTables = new System.Windows.Forms.ImageList(this.components);
            this.chkWithNolock = new System.Windows.Forms.CheckBox();
            this.splitContainer1.Panel1.SuspendLayout();
            this.splitContainer1.Panel2.SuspendLayout();
            this.splitContainer1.SuspendLayout();
            this.SuspendLayout();
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(24, 12);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(47, 12);
            this.label1.TabIndex = 0;
            this.label1.Text = "数据源:";
            // 
            // txtDbSource
            // 
            this.txtDbSource.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.txtDbSource.Location = new System.Drawing.Point(72, 9);
            this.txtDbSource.Name = "txtDbSource";
            this.txtDbSource.Size = new System.Drawing.Size(448, 21);
            this.txtDbSource.TabIndex = 1;
            this.txtDbSource.Text = "server=127.0.0.1;database=mySourceDb;uid=sa;pwd=xxx";
            this.txtDbSource.TextChanged += new System.EventHandler(this.txtDbSource_TextChanged);
            // 
            // splitContainer1
            // 
            this.splitContainer1.CausesValidation = false;
            this.splitContainer1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.splitContainer1.FixedPanel = System.Windows.Forms.FixedPanel.Panel1;
            this.splitContainer1.Location = new System.Drawing.Point(0, 0);
            this.splitContainer1.Margin = new System.Windows.Forms.Padding(0);
            this.splitContainer1.Name = "splitContainer1";
            this.splitContainer1.Orientation = System.Windows.Forms.Orientation.Horizontal;
            // 
            // splitContainer1.Panel1
            // 
            this.splitContainer1.Panel1.Controls.Add(this.chkWithNolock);
            this.splitContainer1.Panel1.Controls.Add(this.chkUseTruncate);
            this.splitContainer1.Panel1.Controls.Add(this.chkErrContinue);
            this.splitContainer1.Panel1.Controls.Add(this.chkAll);
            this.splitContainer1.Panel1.Controls.Add(this.btnDelRow);
            this.splitContainer1.Panel1.Controls.Add(this.btnAddNewSql);
            this.splitContainer1.Panel1.Controls.Add(this.btnGetSchma);
            this.splitContainer1.Panel1.Controls.Add(this.btnLoad);
            this.splitContainer1.Panel1.Controls.Add(this.btnSaveConfig);
            this.splitContainer1.Panel1.Controls.Add(this.btnSyncBegin);
            this.splitContainer1.Panel1.Controls.Add(this.label2);
            this.splitContainer1.Panel1.Controls.Add(this.label4);
            this.splitContainer1.Panel1.Controls.Add(this.label3);
            this.splitContainer1.Panel1.Controls.Add(this.label1);
            this.splitContainer1.Panel1.Controls.Add(this.txtDbTimeout);
            this.splitContainer1.Panel1.Controls.Add(this.txtDbTarget);
            this.splitContainer1.Panel1.Controls.Add(this.txtDbSource);
            this.splitContainer1.Panel1MinSize = 1;
            // 
            // splitContainer1.Panel2
            // 
            this.splitContainer1.Panel2.Controls.Add(this.lstBoolean);
            this.splitContainer1.Panel2.Controls.Add(this.lstTarget);
            this.splitContainer1.Panel2.Controls.Add(this.lvTables);
            this.splitContainer1.Panel2MinSize = 1;
            this.splitContainer1.Size = new System.Drawing.Size(659, 452);
            this.splitContainer1.SplitterDistance = 109;
            this.splitContainer1.SplitterWidth = 1;
            this.splitContainer1.TabIndex = 0;
            this.splitContainer1.TabStop = false;
            // 
            // chkUseTruncate
            // 
            this.chkUseTruncate.AutoSize = true;
            this.chkUseTruncate.Checked = true;
            this.chkUseTruncate.CheckState = System.Windows.Forms.CheckState.Checked;
            this.chkUseTruncate.Location = new System.Drawing.Point(136, 60);
            this.chkUseTruncate.Name = "chkUseTruncate";
            this.chkUseTruncate.Size = new System.Drawing.Size(156, 16);
            this.chkUseTruncate.TabIndex = 4;
            this.chkUseTruncate.Text = "使用Truncate清空目标表";
            this.chkUseTruncate.UseVisualStyleBackColor = true;
            // 
            // chkErrContinue
            // 
            this.chkErrContinue.AutoSize = true;
            this.chkErrContinue.Checked = true;
            this.chkErrContinue.CheckState = System.Windows.Forms.CheckState.Checked;
            this.chkErrContinue.Location = new System.Drawing.Point(412, 60);
            this.chkErrContinue.Name = "chkErrContinue";
            this.chkErrContinue.Size = new System.Drawing.Size(144, 16);
            this.chkErrContinue.TabIndex = 6;
            this.chkErrContinue.Text = "错误时继续同步其它表";
            this.chkErrContinue.UseVisualStyleBackColor = true;
            // 
            // chkAll
            // 
            this.chkAll.AutoSize = true;
            this.chkAll.Location = new System.Drawing.Point(14, 88);
            this.chkAll.Name = "chkAll";
            this.chkAll.Size = new System.Drawing.Size(48, 16);
            this.chkAll.TabIndex = 11;
            this.chkAll.Text = "全选";
            this.chkAll.UseVisualStyleBackColor = true;
            this.chkAll.CheckedChanged += new System.EventHandler(this.chkAll_CheckedChanged);
            // 
            // btnDelRow
            // 
            this.btnDelRow.Enabled = false;
            this.btnDelRow.Location = new System.Drawing.Point(63, 81);
            this.btnDelRow.Name = "btnDelRow";
            this.btnDelRow.Size = new System.Drawing.Size(72, 23);
            this.btnDelRow.TabIndex = 12;
            this.btnDelRow.Text = "删除选定";
            this.btnDelRow.UseVisualStyleBackColor = true;
            this.btnDelRow.Click += new System.EventHandler(this.btnDelRow_Click);
            // 
            // btnAddNewSql
            // 
            this.btnAddNewSql.Enabled = false;
            this.btnAddNewSql.Location = new System.Drawing.Point(141, 81);
            this.btnAddNewSql.Name = "btnAddNewSql";
            this.btnAddNewSql.Size = new System.Drawing.Size(87, 23);
            this.btnAddNewSql.TabIndex = 13;
            this.btnAddNewSql.Text = "新增查询同步";
            this.btnAddNewSql.UseVisualStyleBackColor = true;
            this.btnAddNewSql.Click += new System.EventHandler(this.btnAddNewSql_Click);
            // 
            // btnGetSchma
            // 
            this.btnGetSchma.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.btnGetSchma.Location = new System.Drawing.Point(526, 9);
            this.btnGetSchma.Name = "btnGetSchma";
            this.btnGetSchma.Size = new System.Drawing.Size(57, 39);
            this.btnGetSchma.TabIndex = 7;
            this.btnGetSchma.Text = " 获取\r\n表结构";
            this.btnGetSchma.UseVisualStyleBackColor = true;
            this.btnGetSchma.Click += new System.EventHandler(this.btnGetSchma_Click);
            // 
            // btnLoad
            // 
            this.btnLoad.Location = new System.Drawing.Point(572, 56);
            this.btnLoad.Name = "btnLoad";
            this.btnLoad.Size = new System.Drawing.Size(84, 22);
            this.btnLoad.TabIndex = 9;
            this.btnLoad.Text = "从文件加载";
            this.btnLoad.UseVisualStyleBackColor = true;
            this.btnLoad.Click += new System.EventHandler(this.btnLoad_Click);
            // 
            // btnSaveConfig
            // 
            this.btnSaveConfig.Enabled = false;
            this.btnSaveConfig.Location = new System.Drawing.Point(572, 81);
            this.btnSaveConfig.Name = "btnSaveConfig";
            this.btnSaveConfig.Size = new System.Drawing.Size(84, 22);
            this.btnSaveConfig.TabIndex = 10;
            this.btnSaveConfig.Text = "保存为文件";
            this.btnSaveConfig.UseVisualStyleBackColor = true;
            this.btnSaveConfig.Click += new System.EventHandler(this.btnSaveConfig_Click);
            // 
            // btnSyncBegin
            // 
            this.btnSyncBegin.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.btnSyncBegin.BackColor = System.Drawing.SystemColors.Control;
            this.btnSyncBegin.Enabled = false;
            this.btnSyncBegin.Font = new System.Drawing.Font("宋体", 10.5F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.btnSyncBegin.Location = new System.Drawing.Point(589, 9);
            this.btnSyncBegin.Name = "btnSyncBegin";
            this.btnSyncBegin.Size = new System.Drawing.Size(67, 39);
            this.btnSyncBegin.TabIndex = 8;
            this.btnSyncBegin.Text = "开始\r\n同步";
            this.btnSyncBegin.UseVisualStyleBackColor = false;
            this.btnSyncBegin.Click += new System.EventHandler(this.btnSyncBegin_Click);
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(12, 35);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(59, 12);
            this.label2.TabIndex = 0;
            this.label2.Text = "数据目标:";
            // 
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.Location = new System.Drawing.Point(113, 58);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(17, 12);
            this.label4.TabIndex = 0;
            this.label4.Text = "秒";
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Location = new System.Drawing.Point(12, 58);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(59, 12);
            this.label3.TabIndex = 0;
            this.label3.Text = "超时设置:";
            // 
            // txtDbTimeout
            // 
            this.txtDbTimeout.Location = new System.Drawing.Point(72, 55);
            this.txtDbTimeout.Name = "txtDbTimeout";
            this.txtDbTimeout.Size = new System.Drawing.Size(35, 21);
            this.txtDbTimeout.TabIndex = 3;
            this.txtDbTimeout.Text = "30";
            this.txtDbTimeout.TextAlign = System.Windows.Forms.HorizontalAlignment.Right;
            this.txtDbTimeout.TextChanged += new System.EventHandler(this.txtDbSource_TextChanged);
            // 
            // txtDbTarget
            // 
            this.txtDbTarget.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.txtDbTarget.Location = new System.Drawing.Point(72, 32);
            this.txtDbTarget.Name = "txtDbTarget";
            this.txtDbTarget.Size = new System.Drawing.Size(448, 21);
            this.txtDbTarget.TabIndex = 2;
            this.txtDbTarget.Text = "server=127.0.0.1;database=myTargetDb;uid=sa;pwd=xxx";
            this.txtDbTarget.TextChanged += new System.EventHandler(this.txtDbSource_TextChanged);
            // 
            // lstBoolean
            // 
            this.lstBoolean.DropDownHeight = 1060;
            this.lstBoolean.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.lstBoolean.FormattingEnabled = true;
            this.lstBoolean.IntegralHeight = false;
            this.lstBoolean.Items.AddRange(new object[] {
            "true",
            "false"});
            this.lstBoolean.Location = new System.Drawing.Point(435, 185);
            this.lstBoolean.Name = "lstBoolean";
            this.lstBoolean.Size = new System.Drawing.Size(121, 20);
            this.lstBoolean.TabIndex = 0;
            this.lstBoolean.TabStop = false;
            this.lstBoolean.Visible = false;
            this.lstBoolean.SelectedIndexChanged += new System.EventHandler(this.lstChange);
            this.lstBoolean.Leave += new System.EventHandler(this.lstLeave);
            // 
            // lstTarget
            // 
            this.lstTarget.DropDownHeight = 1060;
            this.lstTarget.FormattingEnabled = true;
            this.lstTarget.IntegralHeight = false;
            this.lstTarget.Location = new System.Drawing.Point(250, 185);
            this.lstTarget.Name = "lstTarget";
            this.lstTarget.Size = new System.Drawing.Size(121, 20);
            this.lstTarget.TabIndex = 0;
            this.lstTarget.TabStop = false;
            this.lstTarget.Visible = false;
            this.lstTarget.SelectedIndexChanged += new System.EventHandler(this.lstChange);
            this.lstTarget.Leave += new System.EventHandler(this.lstLeave);
            // 
            // lvTables
            // 
            this.lvTables.CheckBoxes = true;
            this.lvTables.Columns.AddRange(new System.Windows.Forms.ColumnHeader[] {
            this.colSource,
            this.colTarget,
            this.colTruncate,
            this.colIdentifier});
            this.lvTables.Dock = System.Windows.Forms.DockStyle.Fill;
            this.lvTables.FullRowSelect = true;
            this.lvTables.GridLines = true;
            this.lvTables.HeaderStyle = System.Windows.Forms.ColumnHeaderStyle.Nonclickable;
            this.lvTables.HideSelection = false;
            this.lvTables.Location = new System.Drawing.Point(0, 0);
            this.lvTables.Margin = new System.Windows.Forms.Padding(0);
            this.lvTables.MultiSelect = false;
            this.lvTables.Name = "lvTables";
            this.lvTables.Size = new System.Drawing.Size(659, 342);
            this.lvTables.SmallImageList = this.imglstForLvTables;
            this.lvTables.TabIndex = 0;
            this.lvTables.TabStop = false;
            this.lvTables.UseCompatibleStateImageBehavior = false;
            this.lvTables.View = System.Windows.Forms.View.Details;
            this.lvTables.Scroll += new System.EventHandler(this.lstLeave);
            this.lvTables.DrawColumnHeader += new System.Windows.Forms.DrawListViewColumnHeaderEventHandler(this.lvTables_DrawColumnHeader);
            this.lvTables.ItemChecked += new System.Windows.Forms.ItemCheckedEventHandler(this.lvTables_ItemChecked);
            this.lvTables.MouseUp += new System.Windows.Forms.MouseEventHandler(this.lvTables_MouseUp);
            // 
            // colSource
            // 
            this.colSource.Text = "源表";
            this.colSource.Width = 250;
            // 
            // colTarget
            // 
            this.colTarget.Text = "目标表";
            this.colTarget.Width = 250;
            // 
            // colTruncate
            // 
            this.colTruncate.Text = "清空目标表";
            this.colTruncate.Width = 75;
            // 
            // colIdentifier
            // 
            this.colIdentifier.Text = "标识插入";
            // 
            // imglstForLvTables
            // 
            this.imglstForLvTables.ColorDepth = System.Windows.Forms.ColorDepth.Depth8Bit;
            this.imglstForLvTables.ImageSize = new System.Drawing.Size(1, 20);
            this.imglstForLvTables.TransparentColor = System.Drawing.Color.Transparent;
            // 
            // chkWithNolock
            // 
            this.chkWithNolock.AutoSize = true;
            this.chkWithNolock.Location = new System.Drawing.Point(298, 60);
            this.chkWithNolock.Name = "chkWithNolock";
            this.chkWithNolock.Size = new System.Drawing.Size(108, 16);
            this.chkWithNolock.TabIndex = 5;
            this.chkWithNolock.Text = "增加NoLock选项";
            this.chkWithNolock.UseVisualStyleBackColor = true;
            this.chkWithNolock.CheckedChanged += new System.EventHandler(this.chkWithNolock_CheckedChanged);
            // 
            // MainForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(659, 452);
            this.Controls.Add(this.splitContainer1);
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.Name = "MainForm";
            this.Text = "Sql Server数据导入导出";
            this.splitContainer1.Panel1.ResumeLayout(false);
            this.splitContainer1.Panel1.PerformLayout();
            this.splitContainer1.Panel2.ResumeLayout(false);
            this.splitContainer1.ResumeLayout(false);
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.TextBox txtDbSource;
        private System.Windows.Forms.SplitContainer splitContainer1;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.TextBox txtDbTarget;
        private System.Windows.Forms.Button btnGetSchma;
        private System.Windows.Forms.Button btnSyncBegin;
        private ListViewNF lvTables;
        private System.Windows.Forms.ColumnHeader colSource;
        private System.Windows.Forms.ColumnHeader colTarget;
        private System.Windows.Forms.ComboBox lstTarget;
        private System.Windows.Forms.ImageList imglstForLvTables;
        private System.Windows.Forms.Button btnAddNewSql;
        private System.Windows.Forms.Button btnDelRow;
        private System.Windows.Forms.CheckBox chkErrContinue;
        private System.Windows.Forms.CheckBox chkAll;
        private System.Windows.Forms.Button btnSaveConfig;
        private System.Windows.Forms.ColumnHeader colTruncate;
        private System.Windows.Forms.ColumnHeader colIdentifier;
        private System.Windows.Forms.ComboBox lstBoolean;
        private System.Windows.Forms.Button btnLoad;
        private System.Windows.Forms.CheckBox chkUseTruncate;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.TextBox txtDbTimeout;
        private System.Windows.Forms.CheckBox chkWithNolock;
    }
}

