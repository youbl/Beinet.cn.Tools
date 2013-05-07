using System;
using System.Text.RegularExpressions;
using System.Windows.Forms;

namespace Beinet.cn.Tools.RegexTool
{
    public partial class OptionSelect : Form
    {
        public object para { get; set; }
        public string ret { get; set; }
        public string spliter { get; set; }
        public OptionSelect(object arg)
        {
            ret = null;
            para = arg;
            if (para == null)
            {
                Close();
                //MessageBox.Show("aaa");
                return;
            }
            InitializeComponent();
            ShowInTaskbar = false;// 不能放到OnLoad里，会导致窗体消失

            if(para is Regex)
            {
                var panel = flowLayoutPanel1;
                panel.Controls.Clear();

                var reg = para as Regex;
                int tmp;
                foreach (var gid in reg.GetGroupNumbers())
                {
                    string gname;
                    if (gid == 0)
                        continue;

                    gname = reg.GroupNameFromNumber(gid);
                    if (!string.IsNullOrEmpty(gname) && !int.TryParse(gname, out tmp))
                        gname = "分组" + gid + "(" + gname + ")";
                    else
                        gname = "分组" + gid;

                    CheckBox chk = new CheckBox();
                    panel.Controls.Add(chk);
                    chk.Text = gname;
                    //chk.BackColor = Color.Red;
                    chk.Width = 190;
                    chk.Padding = new Padding(5, 0, 0, 0);
                    chk.Margin = new Padding(2, 0, 0, 0);
                }
            }
        }

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
        }

        private void button1_Click(object sender, EventArgs e)
        {
            ret = string.Empty;
            var panel = flowLayoutPanel1;
            foreach (Control ctl in panel.Controls)
            {
                CheckBox chk = ctl as CheckBox;
                if (chk == null || !chk.Checked)
                    continue;

                //Text格式："分组" + gid + "(" + gname + ")";
                string txt = chk.Text;
                int idx = txt.IndexOf('(');
                if (idx < 0)
                    txt = txt.Substring(2);
                else
                    txt = txt.Substring(2, idx - 2);
                ret += txt + ",";
            }
            if (string.IsNullOrEmpty(ret))
            {
                MessageBox.Show("请选择分组");
                return;
            }
            ret = ret.Substring(0, ret.Length - 1);// 移除最后一个逗号
            //MessageBox.Show(ret);
            spliter = textBox1.Text;
            Close();
        }

        private void OptionSelect_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Escape)
                Close();
        }
    }
}
