namespace Beinet.cn.Tools.WebContentCompare
{
    partial class CompareRegEdit
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
            this.splitContainer1 = new System.Windows.Forms.SplitContainer();
            this.btnOk = new System.Windows.Forms.Button();
            this.txtPost = new System.Windows.Forms.TextBox();
            this.txtUrl = new System.Windows.Forms.TextBox();
            this.label2 = new System.Windows.Forms.Label();
            this.label1 = new System.Windows.Forms.Label();
            this.lvRegs = new System.Windows.Forms.DataGridView();
            this.colReg = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.colReplace = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.colDel = new System.Windows.Forms.DataGridViewLinkColumn();
            ((System.ComponentModel.ISupportInitialize)(this.splitContainer1)).BeginInit();
            this.splitContainer1.Panel1.SuspendLayout();
            this.splitContainer1.Panel2.SuspendLayout();
            this.splitContainer1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.lvRegs)).BeginInit();
            this.SuspendLayout();
            // 
            // splitContainer1
            // 
            this.splitContainer1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.splitContainer1.Location = new System.Drawing.Point(0, 0);
            this.splitContainer1.Margin = new System.Windows.Forms.Padding(1);
            this.splitContainer1.Name = "splitContainer1";
            this.splitContainer1.Orientation = System.Windows.Forms.Orientation.Horizontal;
            // 
            // splitContainer1.Panel1
            // 
            this.splitContainer1.Panel1.Controls.Add(this.btnOk);
            this.splitContainer1.Panel1.Controls.Add(this.txtPost);
            this.splitContainer1.Panel1.Controls.Add(this.txtUrl);
            this.splitContainer1.Panel1.Controls.Add(this.label2);
            this.splitContainer1.Panel1.Controls.Add(this.label1);
            // 
            // splitContainer1.Panel2
            // 
            this.splitContainer1.Panel2.Controls.Add(this.lvRegs);
            this.splitContainer1.Size = new System.Drawing.Size(540, 273);
            this.splitContainer1.SplitterDistance = 60;
            this.splitContainer1.SplitterWidth = 1;
            this.splitContainer1.TabIndex = 7;
            this.splitContainer1.TabStop = false;
            // 
            // btnOk
            // 
            this.btnOk.Location = new System.Drawing.Point(490, 2);
            this.btnOk.Name = "btnOk";
            this.btnOk.Size = new System.Drawing.Size(47, 54);
            this.btnOk.TabIndex = 3;
            this.btnOk.Text = "确认";
            this.btnOk.UseVisualStyleBackColor = true;
            this.btnOk.Click += new System.EventHandler(this.btnOk_Click);
            // 
            // txtPost
            // 
            this.txtPost.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.txtPost.Location = new System.Drawing.Point(49, 33);
            this.txtPost.Name = "txtPost";
            this.txtPost.Size = new System.Drawing.Size(434, 21);
            this.txtPost.TabIndex = 2;
            // 
            // txtUrl
            // 
            this.txtUrl.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.txtUrl.Location = new System.Drawing.Point(49, 3);
            this.txtUrl.Name = "txtUrl";
            this.txtUrl.Size = new System.Drawing.Size(434, 21);
            this.txtUrl.TabIndex = 1;
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(12, 36);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(35, 12);
            this.label2.TabIndex = 0;
            this.label2.Text = "Post:";
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(13, 13);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(29, 12);
            this.label1.TabIndex = 0;
            this.label1.Text = "Url:";
            // 
            // lvRegs
            // 
            this.lvRegs.AutoSizeColumnsMode = System.Windows.Forms.DataGridViewAutoSizeColumnsMode.Fill;
            this.lvRegs.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.lvRegs.Columns.AddRange(new System.Windows.Forms.DataGridViewColumn[] {
            this.colReg,
            this.colReplace,
            this.colDel});
            this.lvRegs.Dock = System.Windows.Forms.DockStyle.Fill;
            this.lvRegs.EditMode = System.Windows.Forms.DataGridViewEditMode.EditOnEnter;
            this.lvRegs.Location = new System.Drawing.Point(0, 0);
            this.lvRegs.MultiSelect = false;
            this.lvRegs.Name = "lvRegs";
            this.lvRegs.RowHeadersWidth = 60;
            this.lvRegs.RowTemplate.Height = 23;
            this.lvRegs.Size = new System.Drawing.Size(540, 212);
            this.lvRegs.TabIndex = 7;
            this.lvRegs.CellClick += new System.Windows.Forms.DataGridViewCellEventHandler(this.lvRegs_CellClick);
            this.lvRegs.CurrentCellDirtyStateChanged += new System.EventHandler(this.lvRegs_CurrentCellDirtyStateChanged);
            // 
            // colReg
            // 
            this.colReg.FillWeight = 230.3587F;
            this.colReg.HeaderText = "正则";
            this.colReg.Name = "colReg";
            // 
            // colReplace
            // 
            this.colReplace.FillWeight = 84.78477F;
            this.colReplace.HeaderText = "替换文本";
            this.colReplace.Name = "colReplace";
            // 
            // colDel
            // 
            this.colDel.FillWeight = 54.39976F;
            this.colDel.HeaderText = "";
            this.colDel.Name = "colDel";
            // 
            // CompareRegEdit
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(540, 273);
            this.Controls.Add(this.splitContainer1);
            this.Name = "CompareRegEdit";
            this.Text = "CompareRegEdit";
            this.splitContainer1.Panel1.ResumeLayout(false);
            this.splitContainer1.Panel1.PerformLayout();
            this.splitContainer1.Panel2.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.splitContainer1)).EndInit();
            this.splitContainer1.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.lvRegs)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.SplitContainer splitContainer1;
        private System.Windows.Forms.DataGridView lvRegs;
        private System.Windows.Forms.Button btnOk;
        private System.Windows.Forms.TextBox txtPost;
        private System.Windows.Forms.TextBox txtUrl;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.DataGridViewTextBoxColumn colReg;
        private System.Windows.Forms.DataGridViewTextBoxColumn colReplace;
        private System.Windows.Forms.DataGridViewLinkColumn colDel;

    }
}