namespace Beinet.cn.Tools.LvsManager
{
    partial class LVSControl
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(LVSControl));
            System.Windows.Forms.ListViewItem listViewItem1 = new System.Windows.Forms.ListViewItem(new string[] {
            "12323",
            "aa",
            "bb",
            "cc"}, "online");
            this.panel1 = new System.Windows.Forms.Panel();
            this.webBrowser1 = new System.Windows.Forms.WebBrowser();
            this.imageList1 = new System.Windows.Forms.ImageList(this.components);
            this.statusStrip1 = new System.Windows.Forms.StatusStrip();
            this.toolStripProgressBar1 = new System.Windows.Forms.ToolStripProgressBar();
            this.labStatus2 = new System.Windows.Forms.ToolStripStatusLabel();
            this.lvServers = new System.Windows.Forms.ListView();
            this.columnHeader1 = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.colName = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.LVSState = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.LVSRealState = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.LastRequestTime = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.currentTime = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.colPv = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.cmsServer = new System.Windows.Forms.ContextMenuStrip(this.components);
            this.上架服务器ToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.下架服务器ToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.toolStripSeparator1 = new System.Windows.Forms.ToolStripSeparator();
            this.查看详细状态ToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.查看客户端请求的IPToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.btnLookState = new System.Windows.Forms.ToolStripButton();
            this.btnSetOnline = new System.Windows.Forms.ToolStripButton();
            this.btnSetOffline = new System.Windows.Forms.ToolStripButton();
            this.toolStripSeparator2 = new System.Windows.Forms.ToolStripSeparator();
            this.btnSettings = new System.Windows.Forms.ToolStripButton();
            this.toolStrip1 = new System.Windows.Forms.ToolStrip();
            this.splitContainer1 = new System.Windows.Forms.SplitContainer();
            this.flowLayoutPanel1 = new System.Windows.Forms.FlowLayoutPanel();
            this.panel2 = new System.Windows.Forms.Panel();
            this.linkLabel1 = new System.Windows.Forms.LinkLabel();
            this.labStatus1 = new System.Windows.Forms.Label();
            this.panel1.SuspendLayout();
            this.statusStrip1.SuspendLayout();
            this.cmsServer.SuspendLayout();
            this.toolStrip1.SuspendLayout();
            this.splitContainer1.Panel1.SuspendLayout();
            this.splitContainer1.Panel2.SuspendLayout();
            this.splitContainer1.SuspendLayout();
            this.panel2.SuspendLayout();
            this.SuspendLayout();
            // 
            // panel1
            // 
            this.panel1.Controls.Add(this.webBrowser1);
            this.panel1.Dock = System.Windows.Forms.DockStyle.Bottom;
            this.panel1.Location = new System.Drawing.Point(0, 377);
            this.panel1.Name = "panel1";
            this.panel1.Size = new System.Drawing.Size(769, 109);
            this.panel1.TabIndex = 0;
            // 
            // webBrowser1
            // 
            this.webBrowser1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.webBrowser1.Location = new System.Drawing.Point(0, 0);
            this.webBrowser1.MinimumSize = new System.Drawing.Size(20, 20);
            this.webBrowser1.Name = "webBrowser1";
            this.webBrowser1.Size = new System.Drawing.Size(769, 109);
            this.webBrowser1.TabIndex = 3;
            // 
            // imageList1
            // 
            this.imageList1.ImageStream = ((System.Windows.Forms.ImageListStreamer)(resources.GetObject("imageList1.ImageStream")));
            this.imageList1.TransparentColor = System.Drawing.Color.Transparent;
            this.imageList1.Images.SetKeyName(0, "online");
            this.imageList1.Images.SetKeyName(1, "offline");
            // 
            // statusStrip1
            // 
            this.statusStrip1.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.toolStripProgressBar1,
            this.labStatus2});
            this.statusStrip1.Location = new System.Drawing.Point(0, 486);
            this.statusStrip1.Name = "statusStrip1";
            this.statusStrip1.Size = new System.Drawing.Size(769, 22);
            this.statusStrip1.TabIndex = 2;
            this.statusStrip1.Text = "statusStrip1";
            // 
            // toolStripProgressBar1
            // 
            this.toolStripProgressBar1.Name = "toolStripProgressBar1";
            this.toolStripProgressBar1.Size = new System.Drawing.Size(100, 16);
            this.toolStripProgressBar1.Style = System.Windows.Forms.ProgressBarStyle.Marquee;
            // 
            // labStatus2
            // 
            this.labStatus2.Name = "labStatus2";
            this.labStatus2.Size = new System.Drawing.Size(20, 17);
            this.labStatus2.Text = "cc";
            // 
            // lvServers
            // 
            this.lvServers.Columns.AddRange(new System.Windows.Forms.ColumnHeader[] {
            this.columnHeader1,
            this.colName,
            this.LVSState,
            this.LVSRealState,
            this.LastRequestTime,
            this.currentTime,
            this.colPv});
            this.lvServers.ContextMenuStrip = this.cmsServer;
            this.lvServers.Dock = System.Windows.Forms.DockStyle.Fill;
            this.lvServers.FullRowSelect = true;
            this.lvServers.Items.AddRange(new System.Windows.Forms.ListViewItem[] {
            listViewItem1});
            this.lvServers.LargeImageList = this.imageList1;
            this.lvServers.Location = new System.Drawing.Point(0, 0);
            this.lvServers.MultiSelect = false;
            this.lvServers.Name = "lvServers";
            this.lvServers.Size = new System.Drawing.Size(769, 261);
            this.lvServers.SmallImageList = this.imageList1;
            this.lvServers.TabIndex = 4;
            this.lvServers.UseCompatibleStateImageBehavior = false;
            this.lvServers.View = System.Windows.Forms.View.Details;
            this.lvServers.SelectedIndexChanged += new System.EventHandler(this.lvServers_SelectedIndexChanged);
            // 
            // columnHeader1
            // 
            this.columnHeader1.Text = "服务器IP";
            this.columnHeader1.Width = 115;
            // 
            // colName
            // 
            this.colName.Text = "域名";
            this.colName.Width = 170;
            // 
            // LVSState
            // 
            this.LVSState.Text = "LVS设置";
            this.LVSState.Width = 90;
            // 
            // LVSRealState
            // 
            this.LVSRealState.Text = "当前状态";
            this.LVSRealState.Width = 70;
            // 
            // LastRequestTime
            // 
            this.LastRequestTime.Text = "上次请求时间";
            this.LastRequestTime.Width = 130;
            // 
            // currentTime
            // 
            this.currentTime.Text = "服务器当前时间";
            this.currentTime.Width = 130;
            // 
            // colPv
            // 
            this.colPv.Text = "PV";
            // 
            // cmsServer
            // 
            this.cmsServer.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.上架服务器ToolStripMenuItem,
            this.下架服务器ToolStripMenuItem,
            this.toolStripSeparator1,
            this.查看详细状态ToolStripMenuItem,
            this.查看客户端请求的IPToolStripMenuItem});
            this.cmsServer.Name = "cmsServer";
            this.cmsServer.Size = new System.Drawing.Size(184, 98);
            this.cmsServer.Opening += new System.ComponentModel.CancelEventHandler(this.cmsServer_Opening);
            // 
            // 上架服务器ToolStripMenuItem
            // 
            this.上架服务器ToolStripMenuItem.Name = "上架服务器ToolStripMenuItem";
            this.上架服务器ToolStripMenuItem.Size = new System.Drawing.Size(183, 22);
            this.上架服务器ToolStripMenuItem.Text = "上架服务器";
            this.上架服务器ToolStripMenuItem.Click += new System.EventHandler(this.上架服务器ToolStripMenuItem_Click);
            // 
            // 下架服务器ToolStripMenuItem
            // 
            this.下架服务器ToolStripMenuItem.Name = "下架服务器ToolStripMenuItem";
            this.下架服务器ToolStripMenuItem.Size = new System.Drawing.Size(183, 22);
            this.下架服务器ToolStripMenuItem.Text = "下架服务器";
            this.下架服务器ToolStripMenuItem.Click += new System.EventHandler(this.下架服务器ToolStripMenuItem_Click);
            // 
            // toolStripSeparator1
            // 
            this.toolStripSeparator1.Name = "toolStripSeparator1";
            this.toolStripSeparator1.Size = new System.Drawing.Size(180, 6);
            // 
            // 查看详细状态ToolStripMenuItem
            // 
            this.查看详细状态ToolStripMenuItem.Name = "查看详细状态ToolStripMenuItem";
            this.查看详细状态ToolStripMenuItem.Size = new System.Drawing.Size(183, 22);
            this.查看详细状态ToolStripMenuItem.Text = "查看真实状态";
            this.查看详细状态ToolStripMenuItem.Click += new System.EventHandler(this.查看详细状态ToolStripMenuItem_Click);
            // 
            // 查看客户端请求的IPToolStripMenuItem
            // 
            this.查看客户端请求的IPToolStripMenuItem.Name = "查看客户端请求的IPToolStripMenuItem";
            this.查看客户端请求的IPToolStripMenuItem.Size = new System.Drawing.Size(183, 22);
            this.查看客户端请求的IPToolStripMenuItem.Text = "查看客户端请求的IP";
            this.查看客户端请求的IPToolStripMenuItem.Click += new System.EventHandler(this.查看客户端请求的IPToolStripMenuItem_Click);
            // 
            // btnLookState
            // 
            this.btnLookState.Enabled = false;
            this.btnLookState.Image = global::Beinet.cn.Tools.Properties.Resources.c0;
            this.btnLookState.ImageScaling = System.Windows.Forms.ToolStripItemImageScaling.None;
            this.btnLookState.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.btnLookState.Name = "btnLookState";
            this.btnLookState.Size = new System.Drawing.Size(66, 36);
            this.btnLookState.Text = "状态";
            this.btnLookState.Click += new System.EventHandler(this.查看详细状态ToolStripMenuItem_Click);
            // 
            // btnSetOnline
            // 
            this.btnSetOnline.Enabled = false;
            this.btnSetOnline.Image = global::Beinet.cn.Tools.Properties.Resources.c1;
            this.btnSetOnline.ImageScaling = System.Windows.Forms.ToolStripItemImageScaling.None;
            this.btnSetOnline.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.btnSetOnline.Name = "btnSetOnline";
            this.btnSetOnline.Size = new System.Drawing.Size(68, 36);
            this.btnSetOnline.Text = "上架";
            this.btnSetOnline.Click += new System.EventHandler(this.上架服务器ToolStripMenuItem_Click);
            // 
            // btnSetOffline
            // 
            this.btnSetOffline.Enabled = false;
            this.btnSetOffline.Image = global::Beinet.cn.Tools.Properties.Resources.c3;
            this.btnSetOffline.ImageScaling = System.Windows.Forms.ToolStripItemImageScaling.None;
            this.btnSetOffline.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.btnSetOffline.Name = "btnSetOffline";
            this.btnSetOffline.Size = new System.Drawing.Size(68, 36);
            this.btnSetOffline.Text = "下架";
            this.btnSetOffline.Click += new System.EventHandler(this.下架服务器ToolStripMenuItem_Click);
            // 
            // toolStripSeparator2
            // 
            this.toolStripSeparator2.Name = "toolStripSeparator2";
            this.toolStripSeparator2.Size = new System.Drawing.Size(6, 39);
            // 
            // btnSettings
            // 
            this.btnSettings.Image = global::Beinet.cn.Tools.Properties.Resources.c4;
            this.btnSettings.ImageScaling = System.Windows.Forms.ToolStripItemImageScaling.None;
            this.btnSettings.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.btnSettings.Name = "btnSettings";
            this.btnSettings.Size = new System.Drawing.Size(90, 36);
            this.btnSettings.Text = "参数设置";
            this.btnSettings.Click += new System.EventHandler(this.btnSettings_Click);
            // 
            // toolStrip1
            // 
            this.toolStrip1.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.btnLookState,
            this.btnSetOnline,
            this.btnSetOffline,
            this.toolStripSeparator2,
            this.btnSettings});
            this.toolStrip1.Location = new System.Drawing.Point(0, 0);
            this.toolStrip1.Name = "toolStrip1";
            this.toolStrip1.Size = new System.Drawing.Size(769, 39);
            this.toolStrip1.TabIndex = 3;
            this.toolStrip1.Text = "toolStrip1";
            // 
            // splitContainer1
            // 
            this.splitContainer1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.splitContainer1.FixedPanel = System.Windows.Forms.FixedPanel.Panel2;
            this.splitContainer1.Location = new System.Drawing.Point(0, 39);
            this.splitContainer1.Name = "splitContainer1";
            this.splitContainer1.Orientation = System.Windows.Forms.Orientation.Horizontal;
            // 
            // splitContainer1.Panel1
            // 
            this.splitContainer1.Panel1.Controls.Add(this.lvServers);
            this.splitContainer1.Panel1MinSize = 1;
            // 
            // splitContainer1.Panel2
            // 
            this.splitContainer1.Panel2.Controls.Add(this.flowLayoutPanel1);
            this.splitContainer1.Panel2MinSize = 1;
            this.splitContainer1.Size = new System.Drawing.Size(769, 338);
            this.splitContainer1.SplitterDistance = 261;
            this.splitContainer1.SplitterWidth = 1;
            this.splitContainer1.TabIndex = 0;
            this.splitContainer1.TabStop = false;
            // 
            // flowLayoutPanel1
            // 
            this.flowLayoutPanel1.AutoScroll = true;
            this.flowLayoutPanel1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.flowLayoutPanel1.Location = new System.Drawing.Point(0, 0);
            this.flowLayoutPanel1.Margin = new System.Windows.Forms.Padding(1);
            this.flowLayoutPanel1.Name = "flowLayoutPanel1";
            this.flowLayoutPanel1.Size = new System.Drawing.Size(769, 76);
            this.flowLayoutPanel1.TabIndex = 0;
            // 
            // panel2
            // 
            this.panel2.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.panel2.Controls.Add(this.linkLabel1);
            this.panel2.Location = new System.Drawing.Point(612, 7);
            this.panel2.Name = "panel2";
            this.panel2.Size = new System.Drawing.Size(157, 26);
            this.panel2.TabIndex = 5;
            // 
            // linkLabel1
            // 
            this.linkLabel1.AutoSize = true;
            this.linkLabel1.Location = new System.Drawing.Point(95, 6);
            this.linkLabel1.Name = "linkLabel1";
            this.linkLabel1.Size = new System.Drawing.Size(53, 12);
            this.linkLabel1.TabIndex = 1;
            this.linkLabel1.TabStop = true;
            this.linkLabel1.Text = "工具原理";
            this.linkLabel1.LinkClicked += new System.Windows.Forms.LinkLabelLinkClickedEventHandler(this.linkLabel1_LinkClicked);
            // 
            // labStatus1
            // 
            this.labStatus1.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.labStatus1.AutoSize = true;
            this.labStatus1.Location = new System.Drawing.Point(646, 495);
            this.labStatus1.Name = "labStatus1";
            this.labStatus1.Size = new System.Drawing.Size(0, 12);
            this.labStatus1.TabIndex = 6;
            // 
            // LVSControl
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(769, 508);
            this.Controls.Add(this.labStatus1);
            this.Controls.Add(this.splitContainer1);
            this.Controls.Add(this.panel2);
            this.Controls.Add(this.panel1);
            this.Controls.Add(this.toolStrip1);
            this.Controls.Add(this.statusStrip1);
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.Name = "LVSControl";
            this.Text = "LVS状态控制器";
            this.panel1.ResumeLayout(false);
            this.statusStrip1.ResumeLayout(false);
            this.statusStrip1.PerformLayout();
            this.cmsServer.ResumeLayout(false);
            this.toolStrip1.ResumeLayout(false);
            this.toolStrip1.PerformLayout();
            this.splitContainer1.Panel1.ResumeLayout(false);
            this.splitContainer1.Panel2.ResumeLayout(false);
            this.splitContainer1.ResumeLayout(false);
            this.panel2.ResumeLayout(false);
            this.panel2.PerformLayout();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Panel panel1;
        private System.Windows.Forms.ImageList imageList1;
        private System.Windows.Forms.StatusStrip statusStrip1;
        private System.Windows.Forms.WebBrowser webBrowser1;
        private System.Windows.Forms.ListView lvServers;
        private System.Windows.Forms.ColumnHeader columnHeader1;
        private System.Windows.Forms.ColumnHeader LVSState;
        private System.Windows.Forms.ColumnHeader LVSRealState;
        private System.Windows.Forms.ColumnHeader LastRequestTime;
        private System.Windows.Forms.ContextMenuStrip cmsServer;
        private System.Windows.Forms.ToolStripMenuItem 上架服务器ToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem 下架服务器ToolStripMenuItem;
        private System.Windows.Forms.ToolStripSeparator toolStripSeparator1;
        private System.Windows.Forms.ToolStripMenuItem 查看详细状态ToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem 查看客户端请求的IPToolStripMenuItem;
        private System.Windows.Forms.ToolStripProgressBar toolStripProgressBar1;
        private System.Windows.Forms.ToolStripButton btnLookState;
        private System.Windows.Forms.ToolStripButton btnSetOnline;
        private System.Windows.Forms.ToolStripButton btnSetOffline;
        private System.Windows.Forms.ToolStripSeparator toolStripSeparator2;
        private System.Windows.Forms.ToolStripButton btnSettings;
        private System.Windows.Forms.ToolStrip toolStrip1;
        private System.Windows.Forms.ColumnHeader currentTime;
        private System.Windows.Forms.ColumnHeader colPv;
        private System.Windows.Forms.SplitContainer splitContainer1;
        private System.Windows.Forms.Panel panel2;
        private System.Windows.Forms.LinkLabel linkLabel1;
        private System.Windows.Forms.FlowLayoutPanel flowLayoutPanel1;
        private System.Windows.Forms.ColumnHeader colName;
        private System.Windows.Forms.ToolStripStatusLabel labStatus2;
        private System.Windows.Forms.Label labStatus1;
    }
}