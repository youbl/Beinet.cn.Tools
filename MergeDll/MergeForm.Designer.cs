namespace Beinet.cn.Tools.MergeDll
{
    partial class MergeForm
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
            this.listBox1 = new System.Windows.Forms.ListBox();
            this.groupBox1 = new System.Windows.Forms.GroupBox();
            this.radExe = new System.Windows.Forms.RadioButton();
            this.radWin = new System.Windows.Forms.RadioButton();
            this.radDll = new System.Windows.Forms.RadioButton();
            this.radSame = new System.Windows.Forms.RadioButton();
            this.btnAddDlls = new System.Windows.Forms.Button();
            this.btnDel = new System.Windows.Forms.Button();
            this.btnDelAll = new System.Windows.Forms.Button();
            this.btnDo = new System.Windows.Forms.Button();
            this.txtOutput = new System.Windows.Forms.TextBox();
            this.btnOutput = new System.Windows.Forms.Button();
            this.btnLogFile = new System.Windows.Forms.Button();
            this.txtLogFile = new System.Windows.Forms.TextBox();
            this.label1 = new System.Windows.Forms.Label();
            this.chkXml = new System.Windows.Forms.CheckBox();
            this.chkDebug = new System.Windows.Forms.CheckBox();
            this.label2 = new System.Windows.Forms.Label();
            this.txtVersion = new System.Windows.Forms.TextBox();
            this.label3 = new System.Windows.Forms.Label();
            this.groupBox1.SuspendLayout();
            this.SuspendLayout();
            // 
            // listBox1
            // 
            this.listBox1.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.listBox1.FormattingEnabled = true;
            this.listBox1.ItemHeight = 12;
            this.listBox1.Location = new System.Drawing.Point(3, 125);
            this.listBox1.Name = "listBox1";
            this.listBox1.Size = new System.Drawing.Size(735, 376);
            this.listBox1.TabIndex = 2;
            // 
            // groupBox1
            // 
            this.groupBox1.Controls.Add(this.radExe);
            this.groupBox1.Controls.Add(this.radWin);
            this.groupBox1.Controls.Add(this.radDll);
            this.groupBox1.Controls.Add(this.radSame);
            this.groupBox1.Location = new System.Drawing.Point(12, 3);
            this.groupBox1.Name = "groupBox1";
            this.groupBox1.Size = new System.Drawing.Size(154, 67);
            this.groupBox1.TabIndex = 3;
            this.groupBox1.TabStop = false;
            this.groupBox1.Text = "输出类型";
            // 
            // radExe
            // 
            this.radExe.AutoSize = true;
            this.radExe.Location = new System.Drawing.Point(108, 21);
            this.radExe.Name = "radExe";
            this.radExe.Size = new System.Drawing.Size(41, 16);
            this.radExe.TabIndex = 0;
            this.radExe.Text = "EXE";
            this.radExe.UseVisualStyleBackColor = true;
            // 
            // radWin
            // 
            this.radWin.AutoSize = true;
            this.radWin.Location = new System.Drawing.Point(7, 45);
            this.radWin.Name = "radWin";
            this.radWin.Size = new System.Drawing.Size(65, 16);
            this.radWin.TabIndex = 0;
            this.radWin.Text = "WINFORM";
            this.radWin.UseVisualStyleBackColor = true;
            // 
            // radDll
            // 
            this.radDll.AutoSize = true;
            this.radDll.Location = new System.Drawing.Point(108, 45);
            this.radDll.Name = "radDll";
            this.radDll.Size = new System.Drawing.Size(41, 16);
            this.radDll.TabIndex = 0;
            this.radDll.Text = "DLL";
            this.radDll.UseVisualStyleBackColor = true;
            // 
            // radSame
            // 
            this.radSame.AutoSize = true;
            this.radSame.Checked = true;
            this.radSame.Location = new System.Drawing.Point(7, 21);
            this.radSame.Name = "radSame";
            this.radSame.Size = new System.Drawing.Size(95, 16);
            this.radSame.TabIndex = 0;
            this.radSame.TabStop = true;
            this.radSame.Text = "跟主文件一致";
            this.radSame.UseVisualStyleBackColor = true;
            // 
            // btnAddDlls
            // 
            this.btnAddDlls.Location = new System.Drawing.Point(12, 99);
            this.btnAddDlls.Name = "btnAddDlls";
            this.btnAddDlls.Size = new System.Drawing.Size(86, 23);
            this.btnAddDlls.TabIndex = 4;
            this.btnAddDlls.Text = "添加文件..";
            this.btnAddDlls.UseVisualStyleBackColor = true;
            this.btnAddDlls.Click += new System.EventHandler(this.btnAddDlls_Click);
            // 
            // btnDel
            // 
            this.btnDel.Location = new System.Drawing.Point(239, 99);
            this.btnDel.Name = "btnDel";
            this.btnDel.Size = new System.Drawing.Size(85, 23);
            this.btnDel.TabIndex = 5;
            this.btnDel.Text = "移除选定文件";
            this.btnDel.UseVisualStyleBackColor = true;
            this.btnDel.Click += new System.EventHandler(this.btnDel_Click);
            // 
            // btnDelAll
            // 
            this.btnDelAll.Location = new System.Drawing.Point(330, 99);
            this.btnDelAll.Name = "btnDelAll";
            this.btnDelAll.Size = new System.Drawing.Size(85, 23);
            this.btnDelAll.TabIndex = 5;
            this.btnDelAll.Text = "移除全部文件";
            this.btnDelAll.UseVisualStyleBackColor = true;
            this.btnDelAll.Click += new System.EventHandler(this.btnDelAll_Click);
            // 
            // btnDo
            // 
            this.btnDo.Location = new System.Drawing.Point(636, 86);
            this.btnDo.Name = "btnDo";
            this.btnDo.Size = new System.Drawing.Size(102, 36);
            this.btnDo.TabIndex = 5;
            this.btnDo.Text = "开始合并";
            this.btnDo.UseVisualStyleBackColor = true;
            this.btnDo.Click += new System.EventHandler(this.btnDo_Click);
            // 
            // txtOutput
            // 
            this.txtOutput.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.txtOutput.Location = new System.Drawing.Point(282, 28);
            this.txtOutput.Name = "txtOutput";
            this.txtOutput.Size = new System.Drawing.Size(456, 21);
            this.txtOutput.TabIndex = 7;
            // 
            // btnOutput
            // 
            this.btnOutput.Location = new System.Drawing.Point(174, 26);
            this.btnOutput.Name = "btnOutput";
            this.btnOutput.Size = new System.Drawing.Size(102, 23);
            this.btnOutput.TabIndex = 8;
            this.btnOutput.Text = "选择输出文件..";
            this.btnOutput.UseVisualStyleBackColor = true;
            this.btnOutput.Click += new System.EventHandler(this.btnOutput_Click);
            // 
            // btnLogFile
            // 
            this.btnLogFile.Location = new System.Drawing.Point(174, 57);
            this.btnLogFile.Name = "btnLogFile";
            this.btnLogFile.Size = new System.Drawing.Size(102, 23);
            this.btnLogFile.TabIndex = 8;
            this.btnLogFile.Text = "选择日志文件..";
            this.btnLogFile.UseVisualStyleBackColor = true;
            this.btnLogFile.Click += new System.EventHandler(this.btnLogFile_Click);
            // 
            // txtLogFile
            // 
            this.txtLogFile.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.txtLogFile.Location = new System.Drawing.Point(282, 59);
            this.txtLogFile.Name = "txtLogFile";
            this.txtLogFile.Size = new System.Drawing.Size(456, 21);
            this.txtLogFile.TabIndex = 7;
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Font = new System.Drawing.Font("SimSun", 9F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.label1.ForeColor = System.Drawing.Color.DarkRed;
            this.label1.Location = new System.Drawing.Point(104, 104);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(122, 12);
            this.label1.TabIndex = 9;
            this.label1.Text = "第一个文件是主文件";
            // 
            // chkXml
            // 
            this.chkXml.AutoSize = true;
            this.chkXml.Checked = true;
            this.chkXml.CheckState = System.Windows.Forms.CheckState.Checked;
            this.chkXml.Location = new System.Drawing.Point(421, 103);
            this.chkXml.Name = "chkXml";
            this.chkXml.Size = new System.Drawing.Size(66, 16);
            this.chkXml.TabIndex = 10;
            this.chkXml.Text = "合并XML";
            this.chkXml.UseVisualStyleBackColor = true;
            // 
            // chkDebug
            // 
            this.chkDebug.AutoSize = true;
            this.chkDebug.Checked = true;
            this.chkDebug.CheckState = System.Windows.Forms.CheckState.Checked;
            this.chkDebug.Location = new System.Drawing.Point(493, 103);
            this.chkDebug.Name = "chkDebug";
            this.chkDebug.Size = new System.Drawing.Size(114, 16);
            this.chkDebug.TabIndex = 10;
            this.chkDebug.Text = "生成PDB调试文件";
            this.chkDebug.UseVisualStyleBackColor = true;
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(10, 77);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(71, 12);
            this.label2.TabIndex = 11;
            this.label2.Text = "设置版本号:";
            // 
            // txtVersion
            // 
            this.txtVersion.Location = new System.Drawing.Point(81, 74);
            this.txtVersion.Name = "txtVersion";
            this.txtVersion.Size = new System.Drawing.Size(85, 21);
            this.txtVersion.TabIndex = 7;
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Font = new System.Drawing.Font("SimSun", 9F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.label3.ForeColor = System.Drawing.Color.DarkRed;
            this.label3.Location = new System.Drawing.Point(172, 9);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(469, 12);
            this.label3.TabIndex = 9;
            this.label3.Text = "只能合并Net的DLL，如果DLL引用了C++组件，那么这个NET的DLL也可能无法合并";
            // 
            // MergeForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(743, 508);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.chkDebug);
            this.Controls.Add(this.chkXml);
            this.Controls.Add(this.label3);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.btnLogFile);
            this.Controls.Add(this.btnOutput);
            this.Controls.Add(this.txtVersion);
            this.Controls.Add(this.txtLogFile);
            this.Controls.Add(this.txtOutput);
            this.Controls.Add(this.btnDo);
            this.Controls.Add(this.btnDelAll);
            this.Controls.Add(this.btnDel);
            this.Controls.Add(this.btnAddDlls);
            this.Controls.Add(this.groupBox1);
            this.Controls.Add(this.listBox1);
            this.Name = "MergeForm";
            this.Text = "MergeForm";
            this.groupBox1.ResumeLayout(false);
            this.groupBox1.PerformLayout();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.ListBox listBox1;
        private System.Windows.Forms.GroupBox groupBox1;
        private System.Windows.Forms.RadioButton radExe;
        private System.Windows.Forms.RadioButton radWin;
        private System.Windows.Forms.RadioButton radDll;
        private System.Windows.Forms.RadioButton radSame;
        private System.Windows.Forms.Button btnAddDlls;
        private System.Windows.Forms.Button btnDel;
        private System.Windows.Forms.Button btnDelAll;
        private System.Windows.Forms.Button btnDo;
        private System.Windows.Forms.TextBox txtOutput;
        private System.Windows.Forms.Button btnOutput;
        private System.Windows.Forms.Button btnLogFile;
        private System.Windows.Forms.TextBox txtLogFile;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.CheckBox chkXml;
        private System.Windows.Forms.CheckBox chkDebug;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.TextBox txtVersion;
        private System.Windows.Forms.Label label3;
    }
}