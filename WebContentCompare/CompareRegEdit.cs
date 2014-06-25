using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Windows.Forms;

namespace Beinet.cn.Tools.WebContentCompare
{
    public partial class CompareRegEdit : Form
    {
        //public const string SPLIT_LINE = @"\|\|\|\|\|";
        //public const string SPLIT_ITEM = @"\.\.\.\.\.";
        //static readonly string _splitLine = SPLIT_LINE.Replace("\\", "");
        //static readonly string _splitItem = SPLIT_ITEM.Replace("\\", "");

        private const int COL_REG = 0;
        private const int COL_REPLACE = 1;
        private const int COL_DEL = 2;
        private const string COLDEL_TXT = "删除";

        private DataGridViewCell _cellUrl;
        private DataGridViewCell _cellPost;
        private DataGridViewCell _cellReg;

        private LinkLabel _lnkReg;

        #region 构造函数
        public CompareRegEdit(LinkLabel lnkReg)
        {
            _lnkReg = lnkReg;

            InitializeComponent();
            ShowInTaskbar = false;// 不能放到OnLoad里，会导致窗体消失
        }

        public CompareRegEdit(DataGridViewCell cellUrl, 
            DataGridViewCell cellPost, 
            DataGridViewCell cellReg)
        {
            _cellUrl = cellUrl;
            _cellPost = cellPost;
            _cellReg = cellReg;

            InitializeComponent();
            ShowInTaskbar = false;// 不能放到OnLoad里，会导致窗体消失
        }


        #endregion

        protected override void OnLoad(EventArgs e)
        {
            base.OnLoad(e);
            if (Owner != null)
            {
                Left = Owner.Left + 20;
                Top = Owner.Top + 20;
                if (Top < 0)
                    Top = 0;
            }

            object objRegs;
            if (_lnkReg == null)
            {
                txtUrl.Text = Convert.ToString(_cellUrl.Value);
                txtPost.Text = Convert.ToString(_cellPost.Value);
                objRegs = _cellReg.Tag;
            }
            else
            {
                objRegs = _lnkReg.Tag;
            }

            if (objRegs != null)
            {
                List<string[]> arrReg = objRegs as List<string[]>;
                if (arrReg != null)
                {
                    foreach (string[] item in arrReg)
                    {
                        if (item.Length >= 2)
                        {
                            var row = new object[COL_DEL + 1];
                            row[COL_REG] = item[0];
                            row[COL_REPLACE] = item[1];
                            row[COL_DEL] = COLDEL_TXT;
                            lvRegs.Rows.Add(row);
                        }
                    }
                }
            }
        }



        private void btnOk_Click(object sender, EventArgs e)
        {
            List<string[]> arrReg = new List<string[]>();
            foreach (DataGridViewRow row in lvRegs.Rows)
            {
                var reg = Convert.ToString(row.Cells[COL_REG].Value); // 正则可能需要空格，所以不能trim
                if (reg == string.Empty)
                    continue;
                try
                {
                    // ReSharper disable once ObjectCreationAsStatement
                    new Regex(reg);
                }
                catch
                {
                    continue;
                }
                var replace = Convert.ToString(row.Cells[COL_REPLACE].Value);
                arrReg.Add(new[] { reg, replace });
            }

            if (_lnkReg == null)
            {
                if (arrReg.Count > 0)
                {
                    _cellReg.Tag = arrReg;
                    _cellReg.Value = arrReg.Count.ToString() + "个正则";
                }
                else
                {
                    _cellReg.Tag = null;
                    _cellReg.Value = Compare.COLREG_TXT;
                }

                _cellUrl.Value = txtUrl.Text;
                _cellPost.Value = txtPost.Text;
            }
            else
            {
                if (arrReg.Count > 0)
                {
                    _lnkReg.Tag = arrReg;
                    _lnkReg.Text = arrReg.Count.ToString() + "个正则";
                }
                else
                {
                    _lnkReg.Tag = null;
                    _lnkReg.Text = "全局正则替换";
                }
            }
            Close();
        }

        private void lvRegs_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            if (e.RowIndex < 0 || e.ColumnIndex < 0)
                return;
            var dgv = lvRegs;

            //DataGridViewRow dgvRow = dgv.Rows[e.RowIndex];

            #region 点击删除按钮
            if (!dgv.ReadOnly && e.RowIndex < dgv.RowCount - 1 && e.ColumnIndex == COL_DEL)//Rows.Count - 1是不让点击未提交的新行
            {
                dgv.Rows.RemoveAt(e.RowIndex);
                dgv.ClearSelection();
            }
            #endregion

        }

        private void lvRegs_CurrentCellDirtyStateChanged(object sender, EventArgs e)
        {
            var dgv = lvRegs;
            if (dgv.CurrentRow == null)
                return;

            // 如果是新行，给最后一格添加删除字样
            if (dgv.CurrentRow.Cells[COL_DEL].Value == null)
            {
                dgv.CurrentRow.Cells[COL_DEL].Value = COLDEL_TXT;
            }

            dgv.CommitEdit(DataGridViewDataErrorContexts.Commit);
        }
    }
}
