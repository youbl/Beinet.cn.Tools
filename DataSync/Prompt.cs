using System;
using System.Windows.Forms;

namespace Beinet.cn.Tools.DataSync
{
    public partial class Prompt : Form
    {
        public Prompt()
        {
            InitializeComponent();
            ShowInTaskbar = false;// 不能放到OnLoad里，会导致窗体消失
            textBox1.Text = Sql;
        }
        protected override void OnLoad(EventArgs e)
        {
            base.OnLoad(e);

            if (Owner != null)
            {
                Left = Owner.Left + 20;
                Top = Owner.Top + Owner.Height - Height - 20;
                if (Top < 0)
                    Top = 0;
            }
        }

        public string Sql { get; set; }
        public bool IsOk { get; set; }

        private void btnOK_Click(object sender, EventArgs e)
        {
            string sql = textBox1.Text.Trim();
            if(sql == string.Empty)
            {
                MessageBox.Show("请输入SQL");
                return;
            }
            Sql = sql;
            IsOk = true;
            this.Close();//Hide
        }

        private void btnCancel_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        // 用于配合Click事件，实现3次点击，全选文本框，在400毫秒内点3次，认为是3连击
        DateTime doubleClickTimer; 
        private void textBox1_DoubleClick(object sender, EventArgs e)
        {
            doubleClickTimer = DateTime.Now;
        }
        private void textBox1_Click(object sender, EventArgs e)
        {
            if((DateTime.Now - doubleClickTimer).TotalMilliseconds <= 400)
            {
                ((TextBox)sender).SelectAll();
            }
        }

        private void Prompt_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Escape)
                btnCancel_Click(sender, e);
        }
    }
}
