namespace Beinet.cn.Tools
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(MainForm));
            this.tabControl1 = new System.Windows.Forms.TabControl();
            this.tabDllAnlyse = new System.Windows.Forms.TabPage();
            this.splitContainer1 = new System.Windows.Forms.SplitContainer();
            this.splitContainer2 = new System.Windows.Forms.SplitContainer();
            this.textBox1 = new System.Windows.Forms.TextBox();
            this.splitContainer3 = new System.Windows.Forms.SplitContainer();
            this.textBox2 = new System.Windows.Forms.TextBox();
            this.textBox3 = new System.Windows.Forms.TextBox();
            this.panel1 = new System.Windows.Forms.Panel();
            this.rad1Debug = new System.Windows.Forms.RadioButton();
            this.rad1Bit = new System.Windows.Forms.RadioButton();
            this.label2 = new System.Windows.Forms.Label();
            this.label1 = new System.Windows.Forms.Label();
            this.button1 = new System.Windows.Forms.Button();
            this.tabGZip = new System.Windows.Forms.TabPage();
            this.tabMd5File = new System.Windows.Forms.TabPage();
            this.tabSqlInject = new System.Windows.Forms.TabPage();
            this.tabLvs = new System.Windows.Forms.TabPage();
            this.tabEncrypt = new System.Windows.Forms.TabPage();
            this.tabRegex = new System.Windows.Forms.TabPage();
            this.tabDataSync = new System.Windows.Forms.TabPage();
            this.tabDllMerge = new System.Windows.Forms.TabPage();
            this.tabQQWry = new System.Windows.Forms.TabPage();
            this.tabWebCompare = new System.Windows.Forms.TabPage();
            this.tabControl1.SuspendLayout();
            this.tabDllAnlyse.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.splitContainer1)).BeginInit();
            this.splitContainer1.Panel1.SuspendLayout();
            this.splitContainer1.Panel2.SuspendLayout();
            this.splitContainer1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.splitContainer2)).BeginInit();
            this.splitContainer2.Panel1.SuspendLayout();
            this.splitContainer2.Panel2.SuspendLayout();
            this.splitContainer2.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.splitContainer3)).BeginInit();
            this.splitContainer3.Panel1.SuspendLayout();
            this.splitContainer3.Panel2.SuspendLayout();
            this.splitContainer3.SuspendLayout();
            this.panel1.SuspendLayout();
            this.SuspendLayout();
            // 
            // tabControl1
            // 
            this.tabControl1.Controls.Add(this.tabDllAnlyse);
            this.tabControl1.Controls.Add(this.tabLvs);
            this.tabControl1.Controls.Add(this.tabEncrypt);
            this.tabControl1.Controls.Add(this.tabDllMerge);
            this.tabControl1.Controls.Add(this.tabMd5File);
            this.tabControl1.Controls.Add(this.tabWebCompare);
            this.tabControl1.Controls.Add(this.tabSqlInject);
            this.tabControl1.Controls.Add(this.tabRegex);
            this.tabControl1.Controls.Add(this.tabDataSync);
            this.tabControl1.Controls.Add(this.tabQQWry);
            this.tabControl1.Controls.Add(this.tabGZip);
            this.tabControl1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tabControl1.Location = new System.Drawing.Point(0, 0);
            this.tabControl1.Name = "tabControl1";
            this.tabControl1.SelectedIndex = 0;
            this.tabControl1.Size = new System.Drawing.Size(784, 562);
            this.tabControl1.TabIndex = 0;
            this.tabControl1.SelectedIndexChanged += new System.EventHandler(this.tabControl1_SelectedIndexChanged);
            // 
            // tabDllAnlyse
            // 
            this.tabDllAnlyse.Controls.Add(this.splitContainer1);
            this.tabDllAnlyse.Location = new System.Drawing.Point(4, 22);
            this.tabDllAnlyse.Name = "tabDllAnlyse";
            this.tabDllAnlyse.Padding = new System.Windows.Forms.Padding(3);
            this.tabDllAnlyse.Size = new System.Drawing.Size(776, 536);
            this.tabDllAnlyse.TabIndex = 0;
            this.tabDllAnlyse.Text = "Dll分析";
            this.tabDllAnlyse.UseVisualStyleBackColor = true;
            // 
            // splitContainer1
            // 
            this.splitContainer1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.splitContainer1.FixedPanel = System.Windows.Forms.FixedPanel.Panel2;
            this.splitContainer1.Location = new System.Drawing.Point(3, 3);
            this.splitContainer1.Margin = new System.Windows.Forms.Padding(1);
            this.splitContainer1.Name = "splitContainer1";
            this.splitContainer1.Orientation = System.Windows.Forms.Orientation.Horizontal;
            // 
            // splitContainer1.Panel1
            // 
            this.splitContainer1.Panel1.Controls.Add(this.splitContainer2);
            // 
            // splitContainer1.Panel2
            // 
            this.splitContainer1.Panel2.Controls.Add(this.panel1);
            this.splitContainer1.Panel2.Controls.Add(this.label2);
            this.splitContainer1.Panel2.Controls.Add(this.label1);
            this.splitContainer1.Panel2.Controls.Add(this.button1);
            this.splitContainer1.Size = new System.Drawing.Size(770, 530);
            this.splitContainer1.SplitterDistance = 504;
            this.splitContainer1.SplitterWidth = 1;
            this.splitContainer1.TabIndex = 2;
            this.splitContainer1.TabStop = false;
            // 
            // splitContainer2
            // 
            this.splitContainer2.Dock = System.Windows.Forms.DockStyle.Fill;
            this.splitContainer2.Location = new System.Drawing.Point(0, 0);
            this.splitContainer2.Margin = new System.Windows.Forms.Padding(1);
            this.splitContainer2.Name = "splitContainer2";
            this.splitContainer2.Orientation = System.Windows.Forms.Orientation.Horizontal;
            // 
            // splitContainer2.Panel1
            // 
            this.splitContainer2.Panel1.Controls.Add(this.textBox1);
            // 
            // splitContainer2.Panel2
            // 
            this.splitContainer2.Panel2.Controls.Add(this.splitContainer3);
            this.splitContainer2.Size = new System.Drawing.Size(770, 504);
            this.splitContainer2.SplitterDistance = 165;
            this.splitContainer2.SplitterWidth = 1;
            this.splitContainer2.TabIndex = 0;
            this.splitContainer2.TabStop = false;
            // 
            // textBox1
            // 
            this.textBox1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.textBox1.Location = new System.Drawing.Point(0, 0);
            this.textBox1.Multiline = true;
            this.textBox1.Name = "textBox1";
            this.textBox1.ScrollBars = System.Windows.Forms.ScrollBars.Vertical;
            this.textBox1.Size = new System.Drawing.Size(770, 165);
            this.textBox1.TabIndex = 0;
            this.textBox1.WordWrap = false;
            // 
            // splitContainer3
            // 
            this.splitContainer3.Dock = System.Windows.Forms.DockStyle.Fill;
            this.splitContainer3.Location = new System.Drawing.Point(0, 0);
            this.splitContainer3.Margin = new System.Windows.Forms.Padding(1);
            this.splitContainer3.Name = "splitContainer3";
            this.splitContainer3.Orientation = System.Windows.Forms.Orientation.Horizontal;
            // 
            // splitContainer3.Panel1
            // 
            this.splitContainer3.Panel1.Controls.Add(this.textBox2);
            // 
            // splitContainer3.Panel2
            // 
            this.splitContainer3.Panel2.Controls.Add(this.textBox3);
            this.splitContainer3.Size = new System.Drawing.Size(770, 338);
            this.splitContainer3.SplitterDistance = 179;
            this.splitContainer3.SplitterWidth = 1;
            this.splitContainer3.TabIndex = 0;
            // 
            // textBox2
            // 
            this.textBox2.Dock = System.Windows.Forms.DockStyle.Fill;
            this.textBox2.Location = new System.Drawing.Point(0, 0);
            this.textBox2.Multiline = true;
            this.textBox2.Name = "textBox2";
            this.textBox2.ScrollBars = System.Windows.Forms.ScrollBars.Vertical;
            this.textBox2.Size = new System.Drawing.Size(770, 179);
            this.textBox2.TabIndex = 1;
            this.textBox2.Text = "把文件或目录拖到窗口里";
            this.textBox2.WordWrap = false;
            // 
            // textBox3
            // 
            this.textBox3.Dock = System.Windows.Forms.DockStyle.Fill;
            this.textBox3.Location = new System.Drawing.Point(0, 0);
            this.textBox3.Multiline = true;
            this.textBox3.Name = "textBox3";
            this.textBox3.ScrollBars = System.Windows.Forms.ScrollBars.Vertical;
            this.textBox3.Size = new System.Drawing.Size(770, 158);
            this.textBox3.TabIndex = 1;
            this.textBox3.WordWrap = false;
            // 
            // panel1
            // 
            this.panel1.Controls.Add(this.rad1Debug);
            this.panel1.Controls.Add(this.rad1Bit);
            this.panel1.Location = new System.Drawing.Point(111, 3);
            this.panel1.Name = "panel1";
            this.panel1.Size = new System.Drawing.Size(200, 26);
            this.panel1.TabIndex = 4;
            // 
            // rad1Debug
            // 
            this.rad1Debug.AutoSize = true;
            this.rad1Debug.Location = new System.Drawing.Point(87, 7);
            this.rad1Debug.Name = "rad1Debug";
            this.rad1Debug.Size = new System.Drawing.Size(107, 16);
            this.rad1Debug.TabIndex = 0;
            this.rad1Debug.Text = "按是否调试版本";
            this.rad1Debug.UseVisualStyleBackColor = true;
            // 
            // rad1Bit
            // 
            this.rad1Bit.AutoSize = true;
            this.rad1Bit.Checked = true;
            this.rad1Bit.Location = new System.Drawing.Point(4, 7);
            this.rad1Bit.Name = "rad1Bit";
            this.rad1Bit.Size = new System.Drawing.Size(77, 16);
            this.rad1Bit.TabIndex = 0;
            this.rad1Bit.TabStop = true;
            this.rad1Bit.Text = "按32/64位";
            this.rad1Bit.UseVisualStyleBackColor = true;
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.ForeColor = System.Drawing.Color.Red;
            this.label2.Location = new System.Drawing.Point(317, 8);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(65, 12);
            this.label2.TabIndex = 3;
            this.label2.Text = "支持拖文件";
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.ForeColor = System.Drawing.Color.Red;
            this.label1.Location = new System.Drawing.Point(562, 8);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(203, 12);
            this.label1.TabIndex = 2;
            this.label1.Text = "编译为AnyCpu的dll，会被认为是32位";
            // 
            // button1
            // 
            this.button1.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.button1.Location = new System.Drawing.Point(5, 5);
            this.button1.Name = "button1";
            this.button1.Size = new System.Drawing.Size(99, 23);
            this.button1.TabIndex = 1;
            this.button1.Text = "选择文件...";
            this.button1.UseVisualStyleBackColor = true;
            this.button1.Click += new System.EventHandler(this.button1_Click);
            // 
            // tabGZip
            // 
            this.tabGZip.Location = new System.Drawing.Point(4, 22);
            this.tabGZip.Name = "tabGZip";
            this.tabGZip.Padding = new System.Windows.Forms.Padding(3);
            this.tabGZip.Size = new System.Drawing.Size(776, 536);
            this.tabGZip.TabIndex = 1;
            this.tabGZip.Text = "GZip测试";
            this.tabGZip.UseVisualStyleBackColor = true;
            // 
            // tabMd5File
            // 
            this.tabMd5File.Location = new System.Drawing.Point(4, 22);
            this.tabMd5File.Name = "tabMd5File";
            this.tabMd5File.Padding = new System.Windows.Forms.Padding(3);
            this.tabMd5File.Size = new System.Drawing.Size(776, 536);
            this.tabMd5File.TabIndex = 2;
            this.tabMd5File.Text = "文件MD5";
            this.tabMd5File.UseVisualStyleBackColor = true;
            // 
            // tabSqlInject
            // 
            this.tabSqlInject.Location = new System.Drawing.Point(4, 22);
            this.tabSqlInject.Name = "tabSqlInject";
            this.tabSqlInject.Padding = new System.Windows.Forms.Padding(3);
            this.tabSqlInject.Size = new System.Drawing.Size(776, 536);
            this.tabSqlInject.TabIndex = 3;
            this.tabSqlInject.Text = "SQL拼接检查";
            this.tabSqlInject.UseVisualStyleBackColor = true;
            // 
            // tabLvs
            // 
            this.tabLvs.Location = new System.Drawing.Point(4, 22);
            this.tabLvs.Name = "tabLvs";
            this.tabLvs.Padding = new System.Windows.Forms.Padding(3);
            this.tabLvs.Size = new System.Drawing.Size(776, 536);
            this.tabLvs.TabIndex = 4;
            this.tabLvs.Text = "Lvs控制";
            this.tabLvs.UseVisualStyleBackColor = true;
            // 
            // tabEncrypt
            // 
            this.tabEncrypt.Location = new System.Drawing.Point(4, 22);
            this.tabEncrypt.Name = "tabEncrypt";
            this.tabEncrypt.Padding = new System.Windows.Forms.Padding(3);
            this.tabEncrypt.Size = new System.Drawing.Size(776, 536);
            this.tabEncrypt.TabIndex = 5;
            this.tabEncrypt.Text = "加解密";
            this.tabEncrypt.UseVisualStyleBackColor = true;
            // 
            // tabRegex
            // 
            this.tabRegex.Location = new System.Drawing.Point(4, 22);
            this.tabRegex.Name = "tabRegex";
            this.tabRegex.Padding = new System.Windows.Forms.Padding(3);
            this.tabRegex.Size = new System.Drawing.Size(776, 536);
            this.tabRegex.TabIndex = 6;
            this.tabRegex.Text = "正则工具";
            this.tabRegex.UseVisualStyleBackColor = true;
            // 
            // tabDataSync
            // 
            this.tabDataSync.Location = new System.Drawing.Point(4, 22);
            this.tabDataSync.Name = "tabDataSync";
            this.tabDataSync.Padding = new System.Windows.Forms.Padding(3);
            this.tabDataSync.Size = new System.Drawing.Size(776, 536);
            this.tabDataSync.TabIndex = 7;
            this.tabDataSync.Text = "数据库同步";
            this.tabDataSync.UseVisualStyleBackColor = true;
            // 
            // tabDllMerge
            // 
            this.tabDllMerge.Location = new System.Drawing.Point(4, 22);
            this.tabDllMerge.Name = "tabDllMerge";
            this.tabDllMerge.Padding = new System.Windows.Forms.Padding(3);
            this.tabDllMerge.Size = new System.Drawing.Size(776, 536);
            this.tabDllMerge.TabIndex = 8;
            this.tabDllMerge.Text = "DLL合并";
            this.tabDllMerge.UseVisualStyleBackColor = true;
            // 
            // tabQQWry
            // 
            this.tabQQWry.Location = new System.Drawing.Point(4, 22);
            this.tabQQWry.Name = "tabQQWry";
            this.tabQQWry.Padding = new System.Windows.Forms.Padding(3);
            this.tabQQWry.Size = new System.Drawing.Size(776, 536);
            this.tabQQWry.TabIndex = 9;
            this.tabQQWry.Text = "IP纯真库";
            this.tabQQWry.UseVisualStyleBackColor = true;
            // 
            // tabWebCompare
            // 
            this.tabWebCompare.Location = new System.Drawing.Point(4, 22);
            this.tabWebCompare.Name = "tabWebCompare";
            this.tabWebCompare.Padding = new System.Windows.Forms.Padding(3);
            this.tabWebCompare.Size = new System.Drawing.Size(776, 536);
            this.tabWebCompare.TabIndex = 10;
            this.tabWebCompare.Text = "Web内容对比";
            this.tabWebCompare.UseVisualStyleBackColor = true;
            // 
            // MainForm
            // 
            this.AllowDrop = true;
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(784, 562);
            this.Controls.Add(this.tabControl1);
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.Name = "MainForm";
            this.Text = "工具合集_by ybl";
            this.DragDrop += new System.Windows.Forms.DragEventHandler(this.Form1_DragDrop);
            this.DragEnter += new System.Windows.Forms.DragEventHandler(this.Form1_DragEnter);
            this.tabControl1.ResumeLayout(false);
            this.tabDllAnlyse.ResumeLayout(false);
            this.splitContainer1.Panel1.ResumeLayout(false);
            this.splitContainer1.Panel2.ResumeLayout(false);
            this.splitContainer1.Panel2.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.splitContainer1)).EndInit();
            this.splitContainer1.ResumeLayout(false);
            this.splitContainer2.Panel1.ResumeLayout(false);
            this.splitContainer2.Panel1.PerformLayout();
            this.splitContainer2.Panel2.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.splitContainer2)).EndInit();
            this.splitContainer2.ResumeLayout(false);
            this.splitContainer3.Panel1.ResumeLayout(false);
            this.splitContainer3.Panel1.PerformLayout();
            this.splitContainer3.Panel2.ResumeLayout(false);
            this.splitContainer3.Panel2.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.splitContainer3)).EndInit();
            this.splitContainer3.ResumeLayout(false);
            this.panel1.ResumeLayout(false);
            this.panel1.PerformLayout();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.TabControl tabControl1;
        private System.Windows.Forms.TabPage tabDllAnlyse;
        private System.Windows.Forms.SplitContainer splitContainer1;
        private System.Windows.Forms.SplitContainer splitContainer2;
        private System.Windows.Forms.TextBox textBox1;
        private System.Windows.Forms.SplitContainer splitContainer3;
        private System.Windows.Forms.TextBox textBox2;
        private System.Windows.Forms.TextBox textBox3;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Button button1;
        private System.Windows.Forms.TabPage tabGZip;
        private System.Windows.Forms.Panel panel1;
        private System.Windows.Forms.RadioButton rad1Debug;
        private System.Windows.Forms.RadioButton rad1Bit;
        private System.Windows.Forms.TabPage tabMd5File;
        private System.Windows.Forms.TabPage tabSqlInject;
        private System.Windows.Forms.TabPage tabLvs;
        private System.Windows.Forms.TabPage tabEncrypt;
        private System.Windows.Forms.TabPage tabRegex;
        private System.Windows.Forms.TabPage tabDataSync;
        private System.Windows.Forms.TabPage tabDllMerge;
        private System.Windows.Forms.TabPage tabQQWry;
        private System.Windows.Forms.TabPage tabWebCompare;


    }
}

