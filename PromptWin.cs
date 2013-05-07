using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace Beinet.cn.Tools
{
    public partial class PromptWin : Form
    {
        public string PromptText { get; set; }

        //public PromptWin()
        //    : this(null)
        //{
        //}

        public PromptWin(string title)
        {
            InitializeComponent();
            if (!string.IsNullOrEmpty(title))
            {
                Text = title;
            }
            ShowInTaskbar = false;// 不能放到OnLoad里，会导致窗体消失
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
            this.DialogResult = DialogResult.OK;
            PromptText = textBox1.Text;
            Close();
        }

        private void button2_Click(object sender, EventArgs e)
        {
            this.DialogResult = DialogResult.Cancel;
            PromptText = null;
            Close();
        }

        /// <summary>
        /// 弹出模态窗口，获取返回值
        /// </summary>
        /// <param name="msg"></param>
        /// <param name="title"></param>
        /// <param name="window"></param>
        /// <returns></returns>
        public static bool GetPrompt(out string msg, string title = null, IWin32Window window = null)
        {
            var win = new PromptWin(title);
            DialogResult ret = win.ShowDialog(window);
            if(ret == DialogResult.OK)
            {
                msg = win.PromptText;
                return true;
            }
            msg = null;
            return false;
        }
    }
}
