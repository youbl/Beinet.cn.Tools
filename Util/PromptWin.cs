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
        private string _title { get; set; }

        //public PromptWin()
        //    : this(null)
        //{
        //}

        public PromptWin(string title)
        {
            InitializeComponent();
            _title = title;
            ShowInTaskbar = false;// 不能放到OnLoad里，会导致窗体消失
        }

        protected override void OnLoad(EventArgs e)
        {
            base.OnLoad(e);
            if (!string.IsNullOrEmpty(_title))
            {
                Text = _title;
            }

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
            Ok();
        }

        private void button2_Click(object sender, EventArgs e)
        {
            Cancel();
        }

        void Ok()
        {
            this.DialogResult = DialogResult.OK;
            PromptText = textBox1.Text;
            Close();
        }
        void Cancel()
        {
            this.DialogResult = DialogResult.Cancel;
            PromptText = null;
            Close();
        }

        private void PromptWin_KeyDown(object sender, KeyEventArgs e)
        {
            switch (e.KeyCode)
            {
                case Keys.Escape:
                    Cancel();
                    break;
                case Keys.Enter:
                    Ok();
                    break;
            }
        }



        /// <summary>
        /// 弹出模态窗口，获取返回值
        /// </summary>
        /// <returns></returns>
        public static bool GetPrompt(out string msg, string title = null, 
            string defaultVal = null, IWin32Window window = null)
        {
            try
            {
                using (var win = new PromptWin(title))
                {
                    if (!string.IsNullOrEmpty(defaultVal))
                    {
                        win.textBox1.Text = defaultVal;
                    }
                    DialogResult ret = win.ShowDialog(window);
                    if (ret == DialogResult.OK)
                    {
                        msg = win.PromptText;
                        return true;
                    }
                }
                msg = null;
            }
            catch
            {
                msg = null;
            }
            return false;
        }
    }
}
