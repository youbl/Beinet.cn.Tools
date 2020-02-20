namespace Beinet.cn.Tools.RegexTool
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
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle1 = new System.Windows.Forms.DataGridViewCellStyle();
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(MainForm));
            this.splitMain = new System.Windows.Forms.SplitContainer();
            this.split1 = new System.Windows.Forms.SplitContainer();
            this.lnkMatchFile = new System.Windows.Forms.LinkLabel();
            this.label1 = new System.Windows.Forms.Label();
            this.btnGroupBy0 = new System.Windows.Forms.Button();
            this.btnGroupBy3 = new System.Windows.Forms.Button();
            this.btnGroupBy2 = new System.Windows.Forms.Button();
            this.btnGroupBy1 = new System.Windows.Forms.Button();
            this.btnGroupBy = new System.Windows.Forms.Button();
            this.btnExeOne = new System.Windows.Forms.Button();
            this.btnExecute = new System.Windows.Forms.Button();
            this.chkSplit = new System.Windows.Forms.CheckBox();
            this.chkReplace = new System.Windows.Forms.CheckBox();
            this.chkComment = new System.Windows.Forms.CheckBox();
            this.chkCompiled = new System.Windows.Forms.CheckBox();
            this.chkMultiLine = new System.Windows.Forms.CheckBox();
            this.chkSingle = new System.Windows.Forms.CheckBox();
            this.chkIgnoreCase = new System.Windows.Forms.CheckBox();
            this.txtReg = new System.Windows.Forms.RichTextBox();
            this.menuReg = new System.Windows.Forms.ContextMenuStrip(this.components);
            this.menuRegCut = new System.Windows.Forms.ToolStripMenuItem();
            this.menuRegCopy = new System.Windows.Forms.ToolStripMenuItem();
            this.menuRegParse = new System.Windows.Forms.ToolStripMenuItem();
            this.menuRegDel = new System.Windows.Forms.ToolStripMenuItem();
            this.toolStripSeparator1 = new System.Windows.Forms.ToolStripSeparator();
            this.mnuSaveReg = new System.Windows.Forms.ToolStripMenuItem();
            this.toolStripSeparator2 = new System.Windows.Forms.ToolStripSeparator();
            this.menuRegCommon = new System.Windows.Forms.ToolStripMenuItem();
            this.menuRegSave = new System.Windows.Forms.ToolStripMenuItem();
            this.txtOld = new System.Windows.Forms.RichTextBox();
            this.split2 = new System.Windows.Forms.SplitContainer();
            this.txtReplace = new System.Windows.Forms.RichTextBox();
            this.splitResult = new System.Windows.Forms.SplitContainer();
            this.dgvResult = new System.Windows.Forms.DataGridView();
            this.group0 = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.txtResult = new System.Windows.Forms.RichTextBox();
            this.txtStatus = new System.Windows.Forms.TextBox();
            this.toolTip1 = new System.Windows.Forms.ToolTip(this.components);
            ((System.ComponentModel.ISupportInitialize)(this.splitMain)).BeginInit();
            this.splitMain.Panel1.SuspendLayout();
            this.splitMain.Panel2.SuspendLayout();
            this.splitMain.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.split1)).BeginInit();
            this.split1.Panel1.SuspendLayout();
            this.split1.Panel2.SuspendLayout();
            this.split1.SuspendLayout();
            this.menuReg.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.split2)).BeginInit();
            this.split2.Panel1.SuspendLayout();
            this.split2.Panel2.SuspendLayout();
            this.split2.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.splitResult)).BeginInit();
            this.splitResult.Panel1.SuspendLayout();
            this.splitResult.Panel2.SuspendLayout();
            this.splitResult.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.dgvResult)).BeginInit();
            this.SuspendLayout();
            // 
            // splitMain
            // 
            this.splitMain.Dock = System.Windows.Forms.DockStyle.Fill;
            this.splitMain.Location = new System.Drawing.Point(0, 0);
            this.splitMain.Name = "splitMain";
            this.splitMain.Orientation = System.Windows.Forms.Orientation.Horizontal;
            // 
            // splitMain.Panel1
            // 
            this.splitMain.Panel1.Controls.Add(this.split1);
            // 
            // splitMain.Panel2
            // 
            this.splitMain.Panel2.Controls.Add(this.split2);
            this.splitMain.Size = new System.Drawing.Size(926, 702);
            this.splitMain.SplitterDistance = 333;
            this.splitMain.SplitterWidth = 1;
            this.splitMain.TabIndex = 0;
            this.splitMain.TabStop = false;
            // 
            // split1
            // 
            this.split1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.split1.Location = new System.Drawing.Point(0, 0);
            this.split1.Name = "split1";
            this.split1.Orientation = System.Windows.Forms.Orientation.Horizontal;
            // 
            // split1.Panel1
            // 
            this.split1.Panel1.Controls.Add(this.lnkMatchFile);
            this.split1.Panel1.Controls.Add(this.label1);
            this.split1.Panel1.Controls.Add(this.btnGroupBy0);
            this.split1.Panel1.Controls.Add(this.btnGroupBy3);
            this.split1.Panel1.Controls.Add(this.btnGroupBy2);
            this.split1.Panel1.Controls.Add(this.btnGroupBy1);
            this.split1.Panel1.Controls.Add(this.btnGroupBy);
            this.split1.Panel1.Controls.Add(this.btnExeOne);
            this.split1.Panel1.Controls.Add(this.btnExecute);
            this.split1.Panel1.Controls.Add(this.chkSplit);
            this.split1.Panel1.Controls.Add(this.chkReplace);
            this.split1.Panel1.Controls.Add(this.chkComment);
            this.split1.Panel1.Controls.Add(this.chkCompiled);
            this.split1.Panel1.Controls.Add(this.chkMultiLine);
            this.split1.Panel1.Controls.Add(this.chkSingle);
            this.split1.Panel1.Controls.Add(this.chkIgnoreCase);
            this.split1.Panel1.Controls.Add(this.txtReg);
            // 
            // split1.Panel2
            // 
            this.split1.Panel2.Controls.Add(this.txtOld);
            this.split1.Size = new System.Drawing.Size(926, 333);
            this.split1.SplitterDistance = 135;
            this.split1.SplitterWidth = 1;
            this.split1.TabIndex = 0;
            this.split1.TabStop = false;
            // 
            // lnkMatchFile
            // 
            this.lnkMatchFile.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.lnkMatchFile.AutoSize = true;
            this.lnkMatchFile.Location = new System.Drawing.Point(181, 117);
            this.lnkMatchFile.Name = "lnkMatchFile";
            this.lnkMatchFile.Size = new System.Drawing.Size(95, 12);
            this.lnkMatchFile.TabIndex = 16;
            this.lnkMatchFile.TabStop = true;
            this.lnkMatchFile.Text = "匹配指定文件...";
            this.lnkMatchFile.LinkClicked += new System.Windows.Forms.LinkLabelLinkClickedEventHandler(this.LnkMatchFile_LinkClicked);
            // 
            // label1
            // 
            this.label1.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.label1.AutoSize = true;
            this.label1.Font = new System.Drawing.Font("宋体", 9F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.label1.ForeColor = System.Drawing.Color.Red;
            this.label1.Location = new System.Drawing.Point(416, 90);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(161, 12);
            this.label1.TabIndex = 0;
            this.label1.Text = "正则框内按右键可保存正则";
            // 
            // btnGroupBy0
            // 
            this.btnGroupBy0.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.btnGroupBy0.Location = new System.Drawing.Point(451, 111);
            this.btnGroupBy0.Name = "btnGroupBy0";
            this.btnGroupBy0.Size = new System.Drawing.Size(91, 23);
            this.btnGroupBy0.TabIndex = 11;
            this.btnGroupBy0.Text = "统计整个匹配";
            this.btnGroupBy0.UseVisualStyleBackColor = true;
            this.btnGroupBy0.Click += new System.EventHandler(this.btnGroupBy_Click);
            // 
            // btnGroupBy3
            // 
            this.btnGroupBy3.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.btnGroupBy3.Location = new System.Drawing.Point(718, 111);
            this.btnGroupBy3.Name = "btnGroupBy3";
            this.btnGroupBy3.Size = new System.Drawing.Size(67, 23);
            this.btnGroupBy3.TabIndex = 14;
            this.btnGroupBy3.Text = "统计分组3";
            this.btnGroupBy3.UseVisualStyleBackColor = true;
            this.btnGroupBy3.Click += new System.EventHandler(this.btnGroupBy_Click);
            // 
            // btnGroupBy2
            // 
            this.btnGroupBy2.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.btnGroupBy2.Location = new System.Drawing.Point(637, 111);
            this.btnGroupBy2.Name = "btnGroupBy2";
            this.btnGroupBy2.Size = new System.Drawing.Size(67, 23);
            this.btnGroupBy2.TabIndex = 13;
            this.btnGroupBy2.Text = "统计分组2";
            this.btnGroupBy2.UseVisualStyleBackColor = true;
            this.btnGroupBy2.Click += new System.EventHandler(this.btnGroupBy_Click);
            // 
            // btnGroupBy1
            // 
            this.btnGroupBy1.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.btnGroupBy1.Location = new System.Drawing.Point(556, 111);
            this.btnGroupBy1.Name = "btnGroupBy1";
            this.btnGroupBy1.Size = new System.Drawing.Size(67, 23);
            this.btnGroupBy1.TabIndex = 12;
            this.btnGroupBy1.Text = "统计分组1";
            this.btnGroupBy1.UseVisualStyleBackColor = true;
            this.btnGroupBy1.Click += new System.EventHandler(this.btnGroupBy_Click);
            // 
            // btnGroupBy
            // 
            this.btnGroupBy.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.btnGroupBy.Location = new System.Drawing.Point(799, 112);
            this.btnGroupBy.Name = "btnGroupBy";
            this.btnGroupBy.Size = new System.Drawing.Size(106, 23);
            this.btnGroupBy.TabIndex = 15;
            this.btnGroupBy.Text = "选择分组并统计";
            this.btnGroupBy.UseVisualStyleBackColor = true;
            this.btnGroupBy.Click += new System.EventHandler(this.btnGroupBy_Click);
            // 
            // btnExeOne
            // 
            this.btnExeOne.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.btnExeOne.Location = new System.Drawing.Point(7, 111);
            this.btnExeOne.Name = "btnExeOne";
            this.btnExeOne.Size = new System.Drawing.Size(80, 23);
            this.btnExeOne.TabIndex = 9;
            this.btnExeOne.Text = "逐一匹配";
            this.btnExeOne.UseVisualStyleBackColor = true;
            this.btnExeOne.Click += new System.EventHandler(this.btnExeOne_Click);
            // 
            // btnExecute
            // 
            this.btnExecute.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.btnExecute.Location = new System.Drawing.Point(93, 111);
            this.btnExecute.Name = "btnExecute";
            this.btnExecute.Size = new System.Drawing.Size(80, 23);
            this.btnExecute.TabIndex = 10;
            this.btnExecute.Text = "全部匹配";
            this.btnExecute.UseVisualStyleBackColor = true;
            this.btnExecute.Click += new System.EventHandler(this.btnExecute_Click);
            // 
            // chkSplit
            // 
            this.chkSplit.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.chkSplit.AutoSize = true;
            this.chkSplit.Location = new System.Drawing.Point(832, 89);
            this.chkSplit.Name = "chkSplit";
            this.chkSplit.Size = new System.Drawing.Size(72, 16);
            this.chkSplit.TabIndex = 8;
            this.chkSplit.Text = "分割模式";
            this.chkSplit.UseVisualStyleBackColor = true;
            this.chkSplit.CheckedChanged += new System.EventHandler(this.chkReplace_CheckedChanged);
            // 
            // chkReplace
            // 
            this.chkReplace.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.chkReplace.AutoSize = true;
            this.chkReplace.Location = new System.Drawing.Point(744, 89);
            this.chkReplace.Name = "chkReplace";
            this.chkReplace.Size = new System.Drawing.Size(72, 16);
            this.chkReplace.TabIndex = 7;
            this.chkReplace.Text = "替换模式";
            this.chkReplace.UseVisualStyleBackColor = true;
            this.chkReplace.CheckedChanged += new System.EventHandler(this.chkReplace_CheckedChanged);
            // 
            // chkComment
            // 
            this.chkComment.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.chkComment.AutoSize = true;
            this.chkComment.Location = new System.Drawing.Point(336, 89);
            this.chkComment.Name = "chkComment";
            this.chkComment.Size = new System.Drawing.Size(72, 16);
            this.chkComment.TabIndex = 6;
            this.chkComment.Text = "注释模式";
            this.chkComment.UseVisualStyleBackColor = true;
            this.chkComment.CheckedChanged += new System.EventHandler(this.EnvironmentChanged);
            // 
            // chkCompiled
            // 
            this.chkCompiled.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.chkCompiled.AutoSize = true;
            this.chkCompiled.Location = new System.Drawing.Point(258, 89);
            this.chkCompiled.Name = "chkCompiled";
            this.chkCompiled.Size = new System.Drawing.Size(72, 16);
            this.chkCompiled.TabIndex = 5;
            this.chkCompiled.Text = "编译模式";
            this.chkCompiled.UseVisualStyleBackColor = true;
            this.chkCompiled.CheckedChanged += new System.EventHandler(this.EnvironmentChanged);
            // 
            // chkMultiLine
            // 
            this.chkMultiLine.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.chkMultiLine.AutoSize = true;
            this.chkMultiLine.Location = new System.Drawing.Point(180, 89);
            this.chkMultiLine.Name = "chkMultiLine";
            this.chkMultiLine.Size = new System.Drawing.Size(72, 16);
            this.chkMultiLine.TabIndex = 4;
            this.chkMultiLine.Text = "多行模式";
            this.chkMultiLine.UseVisualStyleBackColor = true;
            this.chkMultiLine.CheckedChanged += new System.EventHandler(this.EnvironmentChanged);
            // 
            // chkSingle
            // 
            this.chkSingle.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.chkSingle.AutoSize = true;
            this.chkSingle.Location = new System.Drawing.Point(102, 89);
            this.chkSingle.Name = "chkSingle";
            this.chkSingle.Size = new System.Drawing.Size(72, 16);
            this.chkSingle.TabIndex = 3;
            this.chkSingle.Text = "单行模式";
            this.chkSingle.UseVisualStyleBackColor = true;
            this.chkSingle.CheckedChanged += new System.EventHandler(this.EnvironmentChanged);
            // 
            // chkIgnoreCase
            // 
            this.chkIgnoreCase.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.chkIgnoreCase.AutoSize = true;
            this.chkIgnoreCase.Location = new System.Drawing.Point(12, 89);
            this.chkIgnoreCase.Name = "chkIgnoreCase";
            this.chkIgnoreCase.Size = new System.Drawing.Size(84, 16);
            this.chkIgnoreCase.TabIndex = 2;
            this.chkIgnoreCase.Text = "忽略大小写";
            this.chkIgnoreCase.UseVisualStyleBackColor = true;
            this.chkIgnoreCase.CheckedChanged += new System.EventHandler(this.EnvironmentChanged);
            // 
            // txtReg
            // 
            this.txtReg.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.txtReg.CausesValidation = false;
            this.txtReg.ContextMenuStrip = this.menuReg;
            this.txtReg.DetectUrls = false;
            this.txtReg.HideSelection = false;
            this.txtReg.Location = new System.Drawing.Point(3, 3);
            this.txtReg.Name = "txtReg";
            this.txtReg.ScrollBars = System.Windows.Forms.RichTextBoxScrollBars.ForcedBoth;
            this.txtReg.ShowSelectionMargin = true;
            this.txtReg.Size = new System.Drawing.Size(920, 80);
            this.txtReg.TabIndex = 1;
            this.txtReg.Text = "";
            this.txtReg.WordWrap = false;
            this.txtReg.TextChanged += new System.EventHandler(this.TextBoxChanged);
            this.txtReg.Enter += new System.EventHandler(this.txt_Enter);
            this.txtReg.Leave += new System.EventHandler(this.txt_Leave);
            // 
            // menuReg
            // 
            this.menuReg.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.menuRegCut,
            this.menuRegCopy,
            this.menuRegParse,
            this.menuRegDel,
            this.toolStripSeparator1,
            this.mnuSaveReg,
            this.toolStripSeparator2,
            this.menuRegCommon,
            this.menuRegSave});
            this.menuReg.Name = "menuReg";
            this.menuReg.Size = new System.Drawing.Size(161, 170);
            // 
            // menuRegCut
            // 
            this.menuRegCut.Name = "menuRegCut";
            this.menuRegCut.Size = new System.Drawing.Size(160, 22);
            this.menuRegCut.Text = "剪切";
            this.menuRegCut.Click += new System.EventHandler(this.menuReg_Click);
            // 
            // menuRegCopy
            // 
            this.menuRegCopy.Name = "menuRegCopy";
            this.menuRegCopy.Size = new System.Drawing.Size(160, 22);
            this.menuRegCopy.Text = "复制";
            this.menuRegCopy.Click += new System.EventHandler(this.menuReg_Click);
            // 
            // menuRegParse
            // 
            this.menuRegParse.Name = "menuRegParse";
            this.menuRegParse.Size = new System.Drawing.Size(160, 22);
            this.menuRegParse.Text = "粘贴";
            this.menuRegParse.Click += new System.EventHandler(this.menuReg_Click);
            // 
            // menuRegDel
            // 
            this.menuRegDel.Name = "menuRegDel";
            this.menuRegDel.Size = new System.Drawing.Size(160, 22);
            this.menuRegDel.Text = "删除";
            this.menuRegDel.Click += new System.EventHandler(this.menuReg_Click);
            // 
            // toolStripSeparator1
            // 
            this.toolStripSeparator1.Name = "toolStripSeparator1";
            this.toolStripSeparator1.Size = new System.Drawing.Size(157, 6);
            // 
            // mnuSaveReg
            // 
            this.mnuSaveReg.Name = "mnuSaveReg";
            this.mnuSaveReg.Size = new System.Drawing.Size(160, 22);
            this.mnuSaveReg.Text = "保存正则...";
            this.mnuSaveReg.Click += new System.EventHandler(this.mnuSaveReg_Click);
            // 
            // toolStripSeparator2
            // 
            this.toolStripSeparator2.Name = "toolStripSeparator2";
            this.toolStripSeparator2.Size = new System.Drawing.Size(157, 6);
            // 
            // menuRegCommon
            // 
            this.menuRegCommon.Name = "menuRegCommon";
            this.menuRegCommon.Size = new System.Drawing.Size(160, 22);
            this.menuRegCommon.Text = "插入常用正则";
            // 
            // menuRegSave
            // 
            this.menuRegSave.Name = "menuRegSave";
            this.menuRegSave.Size = new System.Drawing.Size(160, 22);
            this.menuRegSave.Text = "插入自定义正则";
            // 
            // txtOld
            // 
            this.txtOld.ContextMenuStrip = this.menuReg;
            this.txtOld.DetectUrls = false;
            this.txtOld.Dock = System.Windows.Forms.DockStyle.Fill;
            this.txtOld.EnableAutoDragDrop = true;
            this.txtOld.HideSelection = false;
            this.txtOld.Location = new System.Drawing.Point(0, 0);
            this.txtOld.Name = "txtOld";
            this.txtOld.ScrollBars = System.Windows.Forms.RichTextBoxScrollBars.ForcedBoth;
            this.txtOld.ShowSelectionMargin = true;
            this.txtOld.Size = new System.Drawing.Size(926, 197);
            this.txtOld.TabIndex = 16;
            this.txtOld.Text = "";
            this.txtOld.WordWrap = false;
            this.txtOld.TextChanged += new System.EventHandler(this.TextBoxChanged);
            this.txtOld.Enter += new System.EventHandler(this.txt_Enter);
            this.txtOld.Leave += new System.EventHandler(this.txt_Leave);
            // 
            // split2
            // 
            this.split2.Dock = System.Windows.Forms.DockStyle.Fill;
            this.split2.IsSplitterFixed = true;
            this.split2.Location = new System.Drawing.Point(0, 0);
            this.split2.Name = "split2";
            this.split2.Orientation = System.Windows.Forms.Orientation.Horizontal;
            // 
            // split2.Panel1
            // 
            this.split2.Panel1.Controls.Add(this.txtReplace);
            this.split2.Panel1MinSize = 0;
            // 
            // split2.Panel2
            // 
            this.split2.Panel2.Controls.Add(this.splitResult);
            this.split2.Size = new System.Drawing.Size(926, 368);
            this.split2.SplitterDistance = 25;
            this.split2.SplitterWidth = 1;
            this.split2.TabIndex = 0;
            this.split2.TabStop = false;
            // 
            // txtReplace
            // 
            this.txtReplace.ContextMenuStrip = this.menuReg;
            this.txtReplace.DetectUrls = false;
            this.txtReplace.Dock = System.Windows.Forms.DockStyle.Fill;
            this.txtReplace.EnableAutoDragDrop = true;
            this.txtReplace.HideSelection = false;
            this.txtReplace.Location = new System.Drawing.Point(0, 0);
            this.txtReplace.Name = "txtReplace";
            this.txtReplace.ScrollBars = System.Windows.Forms.RichTextBoxScrollBars.ForcedBoth;
            this.txtReplace.ShowSelectionMargin = true;
            this.txtReplace.Size = new System.Drawing.Size(926, 25);
            this.txtReplace.TabIndex = 17;
            this.txtReplace.Text = "";
            this.txtReplace.WordWrap = false;
            this.txtReplace.TextChanged += new System.EventHandler(this.TextBoxChanged);
            this.txtReplace.Enter += new System.EventHandler(this.txt_Enter);
            this.txtReplace.Leave += new System.EventHandler(this.txt_Leave);
            // 
            // splitResult
            // 
            this.splitResult.Dock = System.Windows.Forms.DockStyle.Fill;
            this.splitResult.Location = new System.Drawing.Point(0, 0);
            this.splitResult.Name = "splitResult";
            this.splitResult.Orientation = System.Windows.Forms.Orientation.Horizontal;
            // 
            // splitResult.Panel1
            // 
            this.splitResult.Panel1.Controls.Add(this.dgvResult);
            this.splitResult.Panel1.Controls.Add(this.txtResult);
            // 
            // splitResult.Panel2
            // 
            this.splitResult.Panel2.Controls.Add(this.txtStatus);
            this.splitResult.Panel2MinSize = 15;
            this.splitResult.Size = new System.Drawing.Size(926, 342);
            this.splitResult.SplitterDistance = 313;
            this.splitResult.SplitterWidth = 1;
            this.splitResult.TabIndex = 0;
            this.splitResult.TabStop = false;
            // 
            // dgvResult
            // 
            dataGridViewCellStyle1.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleLeft;
            dataGridViewCellStyle1.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(233)))), ((int)(((byte)(245)))), ((int)(((byte)(178)))));
            this.dgvResult.AlternatingRowsDefaultCellStyle = dataGridViewCellStyle1;
            this.dgvResult.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.dgvResult.Columns.AddRange(new System.Windows.Forms.DataGridViewColumn[] {
            this.group0});
            this.dgvResult.Dock = System.Windows.Forms.DockStyle.Fill;
            this.dgvResult.Location = new System.Drawing.Point(0, 0);
            this.dgvResult.Name = "dgvResult";
            this.dgvResult.RowHeadersWidth = 60;
            this.dgvResult.RowTemplate.Height = 23;
            this.dgvResult.Size = new System.Drawing.Size(926, 313);
            this.dgvResult.TabIndex = 0;
            this.dgvResult.TabStop = false;
            this.dgvResult.CellClick += new System.Windows.Forms.DataGridViewCellEventHandler(this.dgvResult_CellClick);
            this.dgvResult.ColumnHeaderMouseClick += new System.Windows.Forms.DataGridViewCellMouseEventHandler(this.dgvResult_ColumnHeaderMouseClick);
            this.dgvResult.RowsAdded += new System.Windows.Forms.DataGridViewRowsAddedEventHandler(this.dgvResult_RowsAdded);
            this.dgvResult.RowsRemoved += new System.Windows.Forms.DataGridViewRowsRemovedEventHandler(this.dataGridView1_RowsRemoved);
            // 
            // group0
            // 
            this.group0.HeaderText = "匹配值";
            this.group0.Name = "group0";
            this.group0.Width = 800;
            // 
            // txtResult
            // 
            this.txtResult.ContextMenuStrip = this.menuReg;
            this.txtResult.DetectUrls = false;
            this.txtResult.Dock = System.Windows.Forms.DockStyle.Fill;
            this.txtResult.EnableAutoDragDrop = true;
            this.txtResult.HideSelection = false;
            this.txtResult.Location = new System.Drawing.Point(0, 0);
            this.txtResult.Name = "txtResult";
            this.txtResult.ScrollBars = System.Windows.Forms.RichTextBoxScrollBars.ForcedBoth;
            this.txtResult.ShowSelectionMargin = true;
            this.txtResult.Size = new System.Drawing.Size(926, 313);
            this.txtResult.TabIndex = 18;
            this.txtResult.Text = "";
            this.txtResult.WordWrap = false;
            this.txtResult.Enter += new System.EventHandler(this.txt_Enter);
            this.txtResult.Leave += new System.EventHandler(this.txt_Leave);
            // 
            // txtStatus
            // 
            this.txtStatus.BackColor = System.Drawing.SystemColors.InactiveBorder;
            this.txtStatus.BorderStyle = System.Windows.Forms.BorderStyle.None;
            this.txtStatus.Dock = System.Windows.Forms.DockStyle.Fill;
            this.txtStatus.Location = new System.Drawing.Point(0, 0);
            this.txtStatus.Name = "txtStatus";
            this.txtStatus.Size = new System.Drawing.Size(926, 14);
            this.txtStatus.TabIndex = 0;
            this.txtStatus.TabStop = false;
            // 
            // toolTip1
            // 
            this.toolTip1.AutomaticDelay = 100;
            this.toolTip1.AutoPopDelay = 10000;
            this.toolTip1.InitialDelay = 100;
            this.toolTip1.ReshowDelay = 20;
            this.toolTip1.ShowAlways = true;
            // 
            // MainForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(926, 702);
            this.Controls.Add(this.splitMain);
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.KeyPreview = true;
            this.Name = "MainForm";
            this.Text = "正则工具";
            this.KeyDown += new System.Windows.Forms.KeyEventHandler(this.MainForm_KeyDown);
            this.splitMain.Panel1.ResumeLayout(false);
            this.splitMain.Panel2.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.splitMain)).EndInit();
            this.splitMain.ResumeLayout(false);
            this.split1.Panel1.ResumeLayout(false);
            this.split1.Panel1.PerformLayout();
            this.split1.Panel2.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.split1)).EndInit();
            this.split1.ResumeLayout(false);
            this.menuReg.ResumeLayout(false);
            this.split2.Panel1.ResumeLayout(false);
            this.split2.Panel2.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.split2)).EndInit();
            this.split2.ResumeLayout(false);
            this.splitResult.Panel1.ResumeLayout(false);
            this.splitResult.Panel2.ResumeLayout(false);
            this.splitResult.Panel2.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.splitResult)).EndInit();
            this.splitResult.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.dgvResult)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.SplitContainer splitMain;
        private System.Windows.Forms.SplitContainer split2;
        private System.Windows.Forms.RichTextBox txtReplace;
        private System.Windows.Forms.RichTextBox txtResult;
        private System.Windows.Forms.SplitContainer split1;
        private System.Windows.Forms.Button btnExecute;
        private System.Windows.Forms.CheckBox chkReplace;
        private System.Windows.Forms.CheckBox chkCompiled;
        private System.Windows.Forms.CheckBox chkMultiLine;
        private System.Windows.Forms.CheckBox chkSingle;
        private System.Windows.Forms.CheckBox chkIgnoreCase;
        private System.Windows.Forms.RichTextBox txtReg;
        private System.Windows.Forms.Button btnGroupBy;
        private System.Windows.Forms.DataGridView dgvResult;
        private System.Windows.Forms.DataGridViewTextBoxColumn group0;
        private System.Windows.Forms.RichTextBox txtOld;
        private System.Windows.Forms.SplitContainer splitResult;
        private System.Windows.Forms.TextBox txtStatus;
        private System.Windows.Forms.ContextMenuStrip menuReg;
        private System.Windows.Forms.ToolStripMenuItem menuRegCut;
        private System.Windows.Forms.ToolStripMenuItem menuRegCopy;
        private System.Windows.Forms.ToolStripMenuItem menuRegParse;
        private System.Windows.Forms.ToolStripMenuItem menuRegDel;
        private System.Windows.Forms.ToolStripSeparator toolStripSeparator1;
        private System.Windows.Forms.ToolStripMenuItem menuRegCommon;
        private System.Windows.Forms.Button btnGroupBy0;
        private System.Windows.Forms.Button btnGroupBy1;
        private System.Windows.Forms.Button btnExeOne;
        private System.Windows.Forms.CheckBox chkSplit;
        private System.Windows.Forms.ToolTip toolTip1;
        private System.Windows.Forms.CheckBox chkComment;
        private System.Windows.Forms.Button btnGroupBy3;
        private System.Windows.Forms.Button btnGroupBy2;
        private System.Windows.Forms.ToolStripMenuItem mnuSaveReg;
        private System.Windows.Forms.ToolStripSeparator toolStripSeparator2;
        private System.Windows.Forms.ToolStripMenuItem menuRegSave;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.LinkLabel lnkMatchFile;
    }
}

