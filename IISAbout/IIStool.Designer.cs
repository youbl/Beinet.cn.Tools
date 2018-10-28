namespace Beinet.cn.Tools.IISAbout
{
    partial class IIStool
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
            this.components = new System.ComponentModel.Container();
            System.Windows.Forms.TreeNode treeNode1 = new System.Windows.Forms.TreeNode("网站列表");
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(IIStool));
            this.label1 = new System.Windows.Forms.Label();
            this.txtLogFile = new System.Windows.Forms.TextBox();
            this.btnLogFile = new System.Windows.Forms.Button();
            this.txtRet = new System.Windows.Forms.TextBox();
            this.label2 = new System.Windows.Forms.Label();
            this.txtDbTarget = new System.Windows.Forms.TextBox();
            this.label3 = new System.Windows.Forms.Label();
            this.txtTbName = new System.Windows.Forms.TextBox();
            this.btnImport = new System.Windows.Forms.Button();
            this.button1 = new System.Windows.Forms.Button();
            this.chkClear = new System.Windows.Forms.CheckBox();
            this.chkTimeAdd8 = new System.Windows.Forms.CheckBox();
            this.tabControl1 = new System.Windows.Forms.TabControl();
            this.tabPage2 = new System.Windows.Forms.TabPage();
            this.labLogDir = new System.Windows.Forms.LinkLabel();
            this.txtTimeout = new System.Windows.Forms.TextBox();
            this.txtSitePoolName = new System.Windows.Forms.TextBox();
            this.txtSiteBind = new System.Windows.Forms.TextBox();
            this.txtSiteDir = new System.Windows.Forms.TextBox();
            this.labSiteId = new System.Windows.Forms.Label();
            this.labSiteStatus = new System.Windows.Forms.Label();
            this.label10 = new System.Windows.Forms.Label();
            this.label11 = new System.Windows.Forms.Label();
            this.label13 = new System.Windows.Forms.Label();
            this.txtSiteName = new System.Windows.Forms.TextBox();
            this.label9 = new System.Windows.Forms.Label();
            this.label8 = new System.Windows.Forms.Label();
            this.treeIISSite = new System.Windows.Forms.TreeView();
            this.label7 = new System.Windows.Forms.Label();
            this.btnRestartIIS = new System.Windows.Forms.Button();
            this.btnStopIIS = new System.Windows.Forms.Button();
            this.btnCopyIIS = new System.Windows.Forms.Button();
            this.btnRefreshIIS = new System.Windows.Forms.Button();
            this.btnReset = new System.Windows.Forms.Button();
            this.btnNewSite = new System.Windows.Forms.Button();
            this.btnListSite = new System.Windows.Forms.Button();
            this.label6 = new System.Windows.Forms.Label();
            this.txtRestartSecond = new System.Windows.Forms.TextBox();
            this.txtIISIP = new System.Windows.Forms.TextBox();
            this.label12 = new System.Windows.Forms.Label();
            this.label5 = new System.Windows.Forms.Label();
            this.label4 = new System.Windows.Forms.Label();
            this.tabPage1 = new System.Windows.Forms.TabPage();
            this.contextMenuStripIIS = new System.Windows.Forms.ContextMenuStrip(this.components);
            this.imageList1 = new System.Windows.Forms.ImageList(this.components);
            this.tabControl1.SuspendLayout();
            this.tabPage2.SuspendLayout();
            this.tabPage1.SuspendLayout();
            this.SuspendLayout();
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(2, 7);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(77, 12);
            this.label1.TabIndex = 0;
            this.label1.Text = "IIS日志文件:";
            // 
            // txtLogFile
            // 
            this.txtLogFile.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.txtLogFile.Location = new System.Drawing.Point(81, 4);
            this.txtLogFile.Name = "txtLogFile";
            this.txtLogFile.Size = new System.Drawing.Size(570, 21);
            this.txtLogFile.TabIndex = 1;
            // 
            // btnLogFile
            // 
            this.btnLogFile.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.btnLogFile.Location = new System.Drawing.Point(657, 3);
            this.btnLogFile.Name = "btnLogFile";
            this.btnLogFile.Size = new System.Drawing.Size(75, 23);
            this.btnLogFile.TabIndex = 2;
            this.btnLogFile.Text = "选择..";
            this.btnLogFile.UseVisualStyleBackColor = true;
            this.btnLogFile.Click += new System.EventHandler(this.btnLogFile_Click);
            // 
            // txtRet
            // 
            this.txtRet.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.txtRet.Location = new System.Drawing.Point(6, 82);
            this.txtRet.Multiline = true;
            this.txtRet.Name = "txtRet";
            this.txtRet.ScrollBars = System.Windows.Forms.ScrollBars.Both;
            this.txtRet.Size = new System.Drawing.Size(726, 433);
            this.txtRet.TabIndex = 7;
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(2, 31);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(71, 12);
            this.label2.TabIndex = 0;
            this.label2.Text = "目标数据库:";
            // 
            // txtDbTarget
            // 
            this.txtDbTarget.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.txtDbTarget.Location = new System.Drawing.Point(81, 28);
            this.txtDbTarget.Name = "txtDbTarget";
            this.txtDbTarget.Size = new System.Drawing.Size(570, 21);
            this.txtDbTarget.TabIndex = 3;
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Location = new System.Drawing.Point(2, 58);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(59, 12);
            this.label3.TabIndex = 0;
            this.label3.Text = "导入表名:";
            // 
            // txtTbName
            // 
            this.txtTbName.Location = new System.Drawing.Point(81, 55);
            this.txtTbName.Name = "txtTbName";
            this.txtTbName.Size = new System.Drawing.Size(241, 21);
            this.txtTbName.TabIndex = 4;
            this.txtTbName.Text = "iislog2017";
            // 
            // btnImport
            // 
            this.btnImport.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.btnImport.Location = new System.Drawing.Point(657, 32);
            this.btnImport.Name = "btnImport";
            this.btnImport.Size = new System.Drawing.Size(75, 45);
            this.btnImport.TabIndex = 5;
            this.btnImport.Text = "启动导入";
            this.btnImport.UseVisualStyleBackColor = true;
            this.btnImport.Click += new System.EventHandler(this.btnImport_Click);
            // 
            // button1
            // 
            this.button1.Location = new System.Drawing.Point(559, 53);
            this.button1.Name = "button1";
            this.button1.Size = new System.Drawing.Size(75, 23);
            this.button1.TabIndex = 6;
            this.button1.Text = "常用SQL";
            this.button1.UseVisualStyleBackColor = true;
            this.button1.Click += new System.EventHandler(this.button1_Click);
            // 
            // chkClear
            // 
            this.chkClear.AutoSize = true;
            this.chkClear.Location = new System.Drawing.Point(328, 57);
            this.chkClear.Name = "chkClear";
            this.chkClear.Size = new System.Drawing.Size(96, 16);
            this.chkClear.TabIndex = 8;
            this.chkClear.Text = "导入前清空表";
            this.chkClear.UseVisualStyleBackColor = true;
            // 
            // chkTimeAdd8
            // 
            this.chkTimeAdd8.AutoSize = true;
            this.chkTimeAdd8.Checked = true;
            this.chkTimeAdd8.CheckState = System.Windows.Forms.CheckState.Checked;
            this.chkTimeAdd8.Location = new System.Drawing.Point(425, 57);
            this.chkTimeAdd8.Name = "chkTimeAdd8";
            this.chkTimeAdd8.Size = new System.Drawing.Size(114, 16);
            this.chkTimeAdd8.TabIndex = 9;
            this.chkTimeAdd8.Text = "访问时间加8小时";
            this.chkTimeAdd8.UseVisualStyleBackColor = true;
            // 
            // tabControl1
            // 
            this.tabControl1.Controls.Add(this.tabPage2);
            this.tabControl1.Controls.Add(this.tabPage1);
            this.tabControl1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tabControl1.Location = new System.Drawing.Point(0, 0);
            this.tabControl1.Name = "tabControl1";
            this.tabControl1.SelectedIndex = 0;
            this.tabControl1.Size = new System.Drawing.Size(748, 549);
            this.tabControl1.TabIndex = 10;
            // 
            // tabPage2
            // 
            this.tabPage2.Controls.Add(this.labLogDir);
            this.tabPage2.Controls.Add(this.txtTimeout);
            this.tabPage2.Controls.Add(this.txtSitePoolName);
            this.tabPage2.Controls.Add(this.txtSiteBind);
            this.tabPage2.Controls.Add(this.txtSiteDir);
            this.tabPage2.Controls.Add(this.labSiteId);
            this.tabPage2.Controls.Add(this.labSiteStatus);
            this.tabPage2.Controls.Add(this.label10);
            this.tabPage2.Controls.Add(this.label11);
            this.tabPage2.Controls.Add(this.label13);
            this.tabPage2.Controls.Add(this.txtSiteName);
            this.tabPage2.Controls.Add(this.label9);
            this.tabPage2.Controls.Add(this.label8);
            this.tabPage2.Controls.Add(this.treeIISSite);
            this.tabPage2.Controls.Add(this.label7);
            this.tabPage2.Controls.Add(this.btnRestartIIS);
            this.tabPage2.Controls.Add(this.btnStopIIS);
            this.tabPage2.Controls.Add(this.btnCopyIIS);
            this.tabPage2.Controls.Add(this.btnRefreshIIS);
            this.tabPage2.Controls.Add(this.btnReset);
            this.tabPage2.Controls.Add(this.btnNewSite);
            this.tabPage2.Controls.Add(this.btnListSite);
            this.tabPage2.Controls.Add(this.label6);
            this.tabPage2.Controls.Add(this.txtRestartSecond);
            this.tabPage2.Controls.Add(this.txtIISIP);
            this.tabPage2.Controls.Add(this.label12);
            this.tabPage2.Controls.Add(this.label5);
            this.tabPage2.Controls.Add(this.label4);
            this.tabPage2.Location = new System.Drawing.Point(4, 22);
            this.tabPage2.Name = "tabPage2";
            this.tabPage2.Padding = new System.Windows.Forms.Padding(3);
            this.tabPage2.Size = new System.Drawing.Size(740, 523);
            this.tabPage2.TabIndex = 1;
            this.tabPage2.Text = "IIS操作";
            this.tabPage2.UseVisualStyleBackColor = true;
            // 
            // labLogDir
            // 
            this.labLogDir.AutoSize = true;
            this.labLogDir.Location = new System.Drawing.Point(299, 209);
            this.labLogDir.Name = "labLogDir";
            this.labLogDir.Size = new System.Drawing.Size(0, 12);
            this.labLogDir.TabIndex = 9;
            this.labLogDir.LinkClicked += new System.Windows.Forms.LinkLabelLinkClickedEventHandler(this.labLogDir_LinkClicked);
            // 
            // txtTimeout
            // 
            this.txtTimeout.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.txtTimeout.Location = new System.Drawing.Point(301, 177);
            this.txtTimeout.Name = "txtTimeout";
            this.txtTimeout.Size = new System.Drawing.Size(62, 21);
            this.txtTimeout.TabIndex = 8;
            // 
            // txtSitePoolName
            // 
            this.txtSitePoolName.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.txtSitePoolName.Location = new System.Drawing.Point(301, 150);
            this.txtSitePoolName.Name = "txtSitePoolName";
            this.txtSitePoolName.Size = new System.Drawing.Size(436, 21);
            this.txtSitePoolName.TabIndex = 8;
            // 
            // txtSiteBind
            // 
            this.txtSiteBind.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.txtSiteBind.Location = new System.Drawing.Point(301, 123);
            this.txtSiteBind.Name = "txtSiteBind";
            this.txtSiteBind.Size = new System.Drawing.Size(436, 21);
            this.txtSiteBind.TabIndex = 7;
            // 
            // txtSiteDir
            // 
            this.txtSiteDir.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.txtSiteDir.Location = new System.Drawing.Point(301, 96);
            this.txtSiteDir.Name = "txtSiteDir";
            this.txtSiteDir.Size = new System.Drawing.Size(436, 21);
            this.txtSiteDir.TabIndex = 6;
            // 
            // labSiteId
            // 
            this.labSiteId.AutoSize = true;
            this.labSiteId.Font = new System.Drawing.Font("宋体", 9F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.labSiteId.ForeColor = System.Drawing.Color.Red;
            this.labSiteId.Location = new System.Drawing.Point(299, 45);
            this.labSiteId.Name = "labSiteId";
            this.labSiteId.Size = new System.Drawing.Size(0, 12);
            this.labSiteId.TabIndex = 0;
            // 
            // labSiteStatus
            // 
            this.labSiteStatus.AutoSize = true;
            this.labSiteStatus.ForeColor = System.Drawing.Color.Red;
            this.labSiteStatus.Location = new System.Drawing.Point(301, 239);
            this.labSiteStatus.Name = "labSiteStatus";
            this.labSiteStatus.Size = new System.Drawing.Size(0, 12);
            this.labSiteStatus.TabIndex = 0;
            // 
            // label10
            // 
            this.label10.AutoSize = true;
            this.label10.Location = new System.Drawing.Point(231, 239);
            this.label10.Name = "label10";
            this.label10.Size = new System.Drawing.Size(65, 12);
            this.label10.TabIndex = 0;
            this.label10.Text = "站点状态：";
            // 
            // label11
            // 
            this.label11.AutoSize = true;
            this.label11.Location = new System.Drawing.Point(231, 209);
            this.label11.Name = "label11";
            this.label11.Size = new System.Drawing.Size(65, 12);
            this.label11.TabIndex = 0;
            this.label11.Text = "日志目录：";
            // 
            // label13
            // 
            this.label13.AutoSize = true;
            this.label13.Location = new System.Drawing.Point(219, 180);
            this.label13.Name = "label13";
            this.label13.Size = new System.Drawing.Size(83, 12);
            this.label13.TabIndex = 0;
            this.label13.Text = "连接超时-秒：";
            // 
            // txtSiteName
            // 
            this.txtSiteName.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.txtSiteName.Location = new System.Drawing.Point(301, 69);
            this.txtSiteName.Name = "txtSiteName";
            this.txtSiteName.Size = new System.Drawing.Size(436, 21);
            this.txtSiteName.TabIndex = 5;
            this.txtSiteName.KeyUp += new System.Windows.Forms.KeyEventHandler(this.txtSiteName_KeyUp);
            // 
            // label9
            // 
            this.label9.AutoSize = true;
            this.label9.Location = new System.Drawing.Point(219, 153);
            this.label9.Name = "label9";
            this.label9.Size = new System.Drawing.Size(77, 12);
            this.label9.TabIndex = 0;
            this.label9.Text = "应用程序池：";
            // 
            // label8
            // 
            this.label8.AutoSize = true;
            this.label8.Location = new System.Drawing.Point(255, 126);
            this.label8.Name = "label8";
            this.label8.Size = new System.Drawing.Size(41, 12);
            this.label8.TabIndex = 0;
            this.label8.Text = "绑定：";
            // 
            // treeIISSite
            // 
            this.treeIISSite.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left)));
            this.treeIISSite.ImageIndex = 2;
            this.treeIISSite.ImageList = this.imageList1;
            this.treeIISSite.Location = new System.Drawing.Point(9, 42);
            this.treeIISSite.Name = "treeIISSite";
            treeNode1.Name = "iisRootNode";
            treeNode1.Text = "网站列表";
            this.treeIISSite.Nodes.AddRange(new System.Windows.Forms.TreeNode[] {
            treeNode1});
            this.treeIISSite.SelectedImageIndex = 0;
            this.treeIISSite.Size = new System.Drawing.Size(210, 473);
            this.treeIISSite.TabIndex = 3;
            this.treeIISSite.TabStop = false;
            this.treeIISSite.AfterSelect += new System.Windows.Forms.TreeViewEventHandler(this.treeIISSite_AfterSelect);
            this.treeIISSite.MouseDown += new System.Windows.Forms.MouseEventHandler(this.treeIISSite_MouseDown);
            // 
            // label7
            // 
            this.label7.AutoSize = true;
            this.label7.Location = new System.Drawing.Point(255, 99);
            this.label7.Name = "label7";
            this.label7.Size = new System.Drawing.Size(41, 12);
            this.label7.TabIndex = 0;
            this.label7.Text = "目录：";
            // 
            // btnRestartIIS
            // 
            this.btnRestartIIS.Location = new System.Drawing.Point(627, 40);
            this.btnRestartIIS.Name = "btnRestartIIS";
            this.btnRestartIIS.Size = new System.Drawing.Size(110, 23);
            this.btnRestartIIS.TabIndex = 2;
            this.btnRestartIIS.TabStop = false;
            this.btnRestartIIS.Text = "重启站点和程序池";
            this.btnRestartIIS.UseVisualStyleBackColor = true;
            this.btnRestartIIS.Click += new System.EventHandler(this.btnRestartIIS_Click);
            // 
            // btnStopIIS
            // 
            this.btnStopIIS.Location = new System.Drawing.Point(503, 40);
            this.btnStopIIS.Name = "btnStopIIS";
            this.btnStopIIS.Size = new System.Drawing.Size(118, 23);
            this.btnStopIIS.TabIndex = 2;
            this.btnStopIIS.TabStop = false;
            this.btnStopIIS.Text = "停止站点和程序池";
            this.btnStopIIS.UseVisualStyleBackColor = true;
            this.btnStopIIS.Click += new System.EventHandler(this.btnStopIIS_Click);
            // 
            // btnCopyIIS
            // 
            this.btnCopyIIS.Location = new System.Drawing.Point(405, 40);
            this.btnCopyIIS.Name = "btnCopyIIS";
            this.btnCopyIIS.Size = new System.Drawing.Size(64, 23);
            this.btnCopyIIS.TabIndex = 2;
            this.btnCopyIIS.TabStop = false;
            this.btnCopyIIS.Text = "复制配置";
            this.btnCopyIIS.UseVisualStyleBackColor = true;
            this.btnCopyIIS.Click += new System.EventHandler(this.btnCopyIIS_Click);
            // 
            // btnRefreshIIS
            // 
            this.btnRefreshIIS.Location = new System.Drawing.Point(335, 40);
            this.btnRefreshIIS.Name = "btnRefreshIIS";
            this.btnRefreshIIS.Size = new System.Drawing.Size(64, 23);
            this.btnRefreshIIS.TabIndex = 2;
            this.btnRefreshIIS.TabStop = false;
            this.btnRefreshIIS.Text = "刷新状态";
            this.btnRefreshIIS.UseVisualStyleBackColor = true;
            this.btnRefreshIIS.Click += new System.EventHandler(this.btnRefreshIIS_Click);
            // 
            // btnReset
            // 
            this.btnReset.Location = new System.Drawing.Point(382, 7);
            this.btnReset.Name = "btnReset";
            this.btnReset.Size = new System.Drawing.Size(87, 23);
            this.btnReset.TabIndex = 2;
            this.btnReset.Text = "IISReset";
            this.btnReset.UseVisualStyleBackColor = true;
            this.btnReset.Click += new System.EventHandler(this.btnReset_Click);
            // 
            // btnNewSite
            // 
            this.btnNewSite.Location = new System.Drawing.Point(276, 9);
            this.btnNewSite.Name = "btnNewSite";
            this.btnNewSite.Size = new System.Drawing.Size(87, 23);
            this.btnNewSite.TabIndex = 2;
            this.btnNewSite.Text = "创建站点";
            this.btnNewSite.UseVisualStyleBackColor = true;
            this.btnNewSite.Click += new System.EventHandler(this.btnNewSite_Click);
            // 
            // btnListSite
            // 
            this.btnListSite.Location = new System.Drawing.Point(173, 9);
            this.btnListSite.Name = "btnListSite";
            this.btnListSite.Size = new System.Drawing.Size(87, 23);
            this.btnListSite.TabIndex = 2;
            this.btnListSite.Text = "获取站点列表";
            this.btnListSite.UseVisualStyleBackColor = true;
            this.btnListSite.Click += new System.EventHandler(this.btnListSite_Click);
            // 
            // label6
            // 
            this.label6.AutoSize = true;
            this.label6.Location = new System.Drawing.Point(243, 72);
            this.label6.Name = "label6";
            this.label6.Size = new System.Drawing.Size(53, 12);
            this.label6.TabIndex = 0;
            this.label6.Text = "网站名：";
            // 
            // txtRestartSecond
            // 
            this.txtRestartSecond.Location = new System.Drawing.Point(674, 9);
            this.txtRestartSecond.Name = "txtRestartSecond";
            this.txtRestartSecond.Size = new System.Drawing.Size(49, 21);
            this.txtRestartSecond.TabIndex = 1;
            this.txtRestartSecond.TabStop = false;
            this.txtRestartSecond.Text = "10";
            // 
            // txtIISIP
            // 
            this.txtIISIP.Location = new System.Drawing.Point(67, 9);
            this.txtIISIP.Name = "txtIISIP";
            this.txtIISIP.Size = new System.Drawing.Size(100, 21);
            this.txtIISIP.TabIndex = 1;
            this.txtIISIP.Text = "127.0.0.1";
            this.txtIISIP.Visible = false;
            // 
            // label12
            // 
            this.label12.AutoSize = true;
            this.label12.Location = new System.Drawing.Point(571, 12);
            this.label12.Name = "label12";
            this.label12.Size = new System.Drawing.Size(107, 12);
            this.label12.TabIndex = 0;
            this.label12.Text = "重启等待时间-秒：";
            // 
            // label5
            // 
            this.label5.AutoSize = true;
            this.label5.Location = new System.Drawing.Point(243, 45);
            this.label5.Name = "label5";
            this.label5.Size = new System.Drawing.Size(53, 12);
            this.label5.TabIndex = 0;
            this.label5.Text = "网站ID：";
            // 
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.Location = new System.Drawing.Point(6, 12);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(65, 12);
            this.label4.TabIndex = 0;
            this.label4.Text = "服务器IP：";
            this.label4.Visible = false;
            // 
            // tabPage1
            // 
            this.tabPage1.Controls.Add(this.txtRet);
            this.tabPage1.Controls.Add(this.chkTimeAdd8);
            this.tabPage1.Controls.Add(this.label1);
            this.tabPage1.Controls.Add(this.chkClear);
            this.tabPage1.Controls.Add(this.txtLogFile);
            this.tabPage1.Controls.Add(this.button1);
            this.tabPage1.Controls.Add(this.btnLogFile);
            this.tabPage1.Controls.Add(this.label3);
            this.tabPage1.Controls.Add(this.btnImport);
            this.tabPage1.Controls.Add(this.txtTbName);
            this.tabPage1.Controls.Add(this.txtDbTarget);
            this.tabPage1.Controls.Add(this.label2);
            this.tabPage1.Location = new System.Drawing.Point(4, 22);
            this.tabPage1.Name = "tabPage1";
            this.tabPage1.Padding = new System.Windows.Forms.Padding(3);
            this.tabPage1.Size = new System.Drawing.Size(740, 523);
            this.tabPage1.TabIndex = 0;
            this.tabPage1.Text = "IIS日志导入SqlServer";
            this.tabPage1.UseVisualStyleBackColor = true;
            // 
            // contextMenuStripIIS
            // 
            this.contextMenuStripIIS.Name = "contextMenuStripIIS";
            this.contextMenuStripIIS.Size = new System.Drawing.Size(61, 4);
            this.contextMenuStripIIS.ItemClicked += new System.Windows.Forms.ToolStripItemClickedEventHandler(this.contextMenuStripIIS_ItemClicked);
            // 
            // imageList1
            // 
            this.imageList1.ImageStream = ((System.Windows.Forms.ImageListStreamer)(resources.GetObject("imageList1.ImageStream")));
            this.imageList1.TransparentColor = System.Drawing.Color.Transparent;
            this.imageList1.Images.SetKeyName(0, "start.jpg");
            this.imageList1.Images.SetKeyName(1, "stop.jpg");
            this.imageList1.Images.SetKeyName(2, "root.jpg");
            // 
            // IIStool
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(748, 549);
            this.Controls.Add(this.tabControl1);
            this.Name = "IIStool";
            this.Text = "IIS日志工具";
            this.Load += new System.EventHandler(this.IIStool_Load);
            this.tabControl1.ResumeLayout(false);
            this.tabPage2.ResumeLayout(false);
            this.tabPage2.PerformLayout();
            this.tabPage1.ResumeLayout(false);
            this.tabPage1.PerformLayout();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.TextBox txtLogFile;
        private System.Windows.Forms.Button btnLogFile;
        private System.Windows.Forms.TextBox txtRet;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.TextBox txtDbTarget;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.TextBox txtTbName;
        private System.Windows.Forms.Button btnImport;
        private System.Windows.Forms.Button button1;
        private System.Windows.Forms.CheckBox chkClear;
        private System.Windows.Forms.CheckBox chkTimeAdd8;
        private System.Windows.Forms.TabControl tabControl1;
        private System.Windows.Forms.TabPage tabPage1;
        private System.Windows.Forms.TabPage tabPage2;
        private System.Windows.Forms.Button btnListSite;
        private System.Windows.Forms.TextBox txtIISIP;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.TreeView treeIISSite;
        private System.Windows.Forms.ContextMenuStrip contextMenuStripIIS;
        private System.Windows.Forms.TextBox txtSitePoolName;
        private System.Windows.Forms.TextBox txtSiteBind;
        private System.Windows.Forms.TextBox txtSiteDir;
        private System.Windows.Forms.Label labSiteStatus;
        private System.Windows.Forms.Label label10;
        private System.Windows.Forms.TextBox txtSiteName;
        private System.Windows.Forms.Label label9;
        private System.Windows.Forms.Label label8;
        private System.Windows.Forms.Label label7;
        private System.Windows.Forms.Label label6;
        private System.Windows.Forms.Label label5;
        private System.Windows.Forms.Label label11;
        private System.Windows.Forms.TextBox txtRestartSecond;
        private System.Windows.Forms.Label label12;
        private System.Windows.Forms.Button btnRefreshIIS;
        private System.Windows.Forms.Button btnRestartIIS;
        private System.Windows.Forms.Button btnStopIIS;
        private System.Windows.Forms.Label labSiteId;
        private System.Windows.Forms.Button btnNewSite;
        private System.Windows.Forms.LinkLabel labLogDir;
        private System.Windows.Forms.Button btnCopyIIS;
        private System.Windows.Forms.Button btnReset;
        private System.Windows.Forms.TextBox txtTimeout;
        private System.Windows.Forms.Label label13;
        private System.Windows.Forms.ImageList imageList1;
    }
}