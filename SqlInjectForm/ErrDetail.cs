using System;
using System.Diagnostics;
using System.Text.RegularExpressions;
using System.Windows.Forms;

namespace Beinet.cn.Tools.SqlInjectForm
{
    public partial class ErrDetail : Form
    {
        private ListViewItem _item;
        private Match _match;
        private SqlInject _frm;
        public ErrDetail(ListViewItem item, SqlInject frm)
        {
            InitializeComponent();
            ShowInTaskbar = false; // 不能放到OnLoad里，会导致窗体消失

            _frm = frm;// item.ListView.FindForm() as SqlInject;
            
            InitText(item);
        }

        protected override void OnLoad(EventArgs e)
        {
            if (_match == null)
            {
                Close();
                return;
            }

            base.OnLoad(e);
            if (Owner != null)
            {
                Left = Owner.Left + 20;
                Top = Owner.Top + 20;
                if (Top < 0)
                    Top = 0;
            }

            if (_frm == null)
            {
                Close();
            }
        }


        private void ErrDetail_KeyDown(object sender, KeyEventArgs e)
        {
            if(e.KeyCode == Keys.Escape)
                Close();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            Process.Start("notepad.exe", _item.SubItems[1].Text);
        }

        private void btnIgnore_Click(object sender, EventArgs e)
        {
            //var diaRet = MessageBox.Show("以后扫描不再提示这条Sql", "确实要忽略吗？", MessageBoxButtons.YesNo, MessageBoxIcon.Warning,
            //                 MessageBoxDefaultButton.Button2);
            //if (diaRet == DialogResult.Yes)
            {
                int idx = _item.Index;
                var lv = _item.ListView;
                _frm.IgnoreSql(_item);
                if (idx < lv.Items.Count && idx >= 0)
                    InitText(lv.Items[idx]);
                else if (lv.Items.Count > 0)
                    InitText(lv.Items[0]);
                else
                    Close();
            }
        }

        private void button2_Click(object sender, EventArgs e)
        {
            Process.Start(_item.SubItems[1].Text);
        }

        private void btnBefore_Click(object sender, EventArgs e)
        {
            if (_item.Index <= 0)
            {
                MessageBox.Show("已经是第一条");
                return;
            }
            InitText(_item.ListView.Items[_item.Index - 1]);
        }

        private void btnNext_Click(object sender, EventArgs e)
        {
            if (_item.Index >= _item.ListView.Items.Count - 1)
            {
                MessageBox.Show("已经是最后一条");
                return;
            }
            InitText(_item.ListView.Items[_item.Index + 1]);
        }

        void InitText(ListViewItem item)
        {
            _item = item;
            if (item != null && item.Tag != null)
                _match = item.Tag as Match;

            if(_match == null)
            {
                return;
            }
            txtFileName.Text = _item.SubItems[1].Text;
            txtErr.Text = "第" + _item.SubItems[3].Text + "行\r\n" + _match.Value;
        }
    }
}
