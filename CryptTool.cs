using System;
using System.Configuration;
using System.Text;
using System.Windows.Forms;

namespace Beinet.cn.Tools
{
    public partial class CryptTool : Form
    {
        public CryptTool()
        {
            InitializeComponent();

            string pwd = ConfigurationManager.AppSettings["cryptPwd"] ?? "";
            txtPwd1.Text = pwd;
            txtPwd2.Text = pwd;
        }

        private string _pwd;
        private string _str;
        private Encoding _encoding;
        private StringBuilder _sb = new StringBuilder();

        private void btnDes1_Click(object sender, EventArgs e)
        {
            try
            {
                Init(sender);
                _sb.Length = 0;
                _sb.AppendFormat("密码:{0} 明文:{1} 编码:{2}\r\n{4}:{3}\r\n\r\n",
                                 _pwd, _str, _encoding.Equals(Encoding.UTF8) ? "UTF8" : "GB2312",
                    Utility.DES_Encrypt(_str, _pwd, _encoding), ((Button)sender).Text);
                txtRet.Text += _sb.ToString();
            }
            catch (Exception exp)
            {
                MessageBox.Show(exp.ToString());
            }
        }

        private void btnUnDes1_Click(object sender, EventArgs e)
        {
            try
            {
                Init(sender);
                _sb.Length = 0;
                _sb.AppendFormat("密码:{0} 密文:{1} 编码:{2}\r\n{4}:{3}\r\n\r\n",
                                 _pwd, _str, _encoding.Equals(Encoding.UTF8) ? "UTF8" : "GB2312",
                    Utility.DES_Decrypt(_str, _pwd, _encoding), ((Button)sender).Text);
                txtRet.Text += _sb.ToString();
            }
            catch (Exception exp)
            {
                MessageBox.Show(exp.ToString());
            }
        }

        private void btn3des1_Click(object sender, EventArgs e)
        {
            try
            {
                Init(sender);
                _sb.Length = 0;
                _sb.AppendFormat("密码:{0} 明文:{1} 编码:{2}\r\n{4}:{3}\r\n\r\n",
                                 _pwd, _str, _encoding.Equals(Encoding.UTF8) ? "UTF8" : "GB2312",
                    Utility.TripleDES_Encrypt(_str, _encoding, _pwd), ((Button)sender).Text);
                txtRet.Text += _sb.ToString();
            }
            catch (Exception exp)
            {
                MessageBox.Show(exp.ToString());
            }
        }

        private void btnUn3des_Click(object sender, EventArgs e)
        {
            try
            {
                Init(sender);
                _sb.Length = 0;
                _sb.AppendFormat("密码:{0} 密文:{1} 编码:{2}\r\n{4}:{3}\r\n\r\n",
                                 _pwd, _str, _encoding.Equals(Encoding.UTF8) ? "UTF8" : "GB2312",
                    Utility.TripleDES_Decrypt(_str, _encoding, _pwd), ((Button)sender).Text);
                txtRet.Text += _sb.ToString();
            }
            catch (Exception exp)
            {
                MessageBox.Show(exp.ToString());
            }
        }

        private void btnMD5_Click(object sender, EventArgs e)
        {
            try
            {
                Init(sender);
                _sb.Length = 0;
                _sb.AppendFormat("明文:{0} 编码:{1}\r\n{3}:{2}\r\n\r\n",
                                 _str, _encoding.Equals(Encoding.UTF8) ? "UTF8" : "GB2312",
                    Utility.MD5_Encrypt(_str, null, _encoding), ((Button)sender).Text);
                txtRet.Text += _sb.ToString();
            }
            catch (Exception exp)
            {
                MessageBox.Show(exp.ToString());
            }
        }


        private void btnBase64_1_Click(object sender, EventArgs e)
        {
            try
            {
                Init(sender);
                _sb.Length = 0;
                _sb.AppendFormat("明文:{0} 编码:{1}\r\n{3}:{2}\r\n\r\n",
                                 _str, _encoding.Equals(Encoding.UTF8) ? "UTF8" : "GB2312",
                                 Utility.HttpBase64Encode(_str, _encoding), ((Button)sender).Text);
                txtRet.Text += _sb.ToString();
            }
            catch (Exception exp)
            {
                MessageBox.Show(exp.ToString());
            }
        }

        private void btnUnBase64_1_Click(object sender, EventArgs e)
        {
            try
            {
                Init(sender);
                _sb.Length = 0;
                _sb.AppendFormat("密文:{0} 编码:{1}\r\n{3}:{2}\r\n\r\n",
                    _str, _encoding.Equals(Encoding.UTF8) ? "UTF8" : "GB2312",
                    Utility.HttpBase64Decode(_str, _encoding), ((Button)sender).Text);
                txtRet.Text += _sb.ToString();
            }
            catch (Exception exp)
            {
                MessageBox.Show(exp.ToString());
            }
        }

        private void btnHash1_Click(object sender, EventArgs e)
        {
            try
            {
                Init(sender);
                _sb.Length = 0;
                _sb.AppendFormat("文本:{0}\r\n{2}:{1}\r\n\r\n",
                    _str, _str.GetHashCode().ToString(), ((Button)sender).Text);
                txtRet.Text += _sb.ToString();
            }
            catch (Exception exp)
            {
                MessageBox.Show(exp.ToString());
            }
        }

        private void btnHash32_1_Click(object sender, EventArgs e)
        {
            try
            {
                Init(sender);
                _sb.Length = 0;
                _sb.AppendFormat("文本:{0}\r\n{2}:{1}\r\n\r\n",
                    _str, Utility.GetHashCode32(_str, false).ToString(), ((Button)sender).Text);
                txtRet.Text += _sb.ToString();
            }
            catch (Exception exp)
            {
                MessageBox.Show(exp.ToString());
            }
        }


        private void Init(object sender)
        {
            if (sender == btnDes1 || sender == btnUnDes1 ||
                sender == btnUn3des1 || sender == btn3des1 ||
                sender == btnMD5_1 ||
                sender == btnBase64_1 || sender == btnUnBase64_1 ||
                sender == btnHash1 || sender == btnHash32_1)
            {
                _pwd = txtPwd1.Text;
                _str = txtStr1.Text;
                if (radEncoding1_1.Checked)
                {
                    _encoding = Encoding.UTF8;
                }
                else
                {
                    _encoding = Encoding.GetEncoding("GB2312");
                }
            }
            else
            {
                _pwd = txtPwd2.Text;
                _str = txtStr2.Text;
                if (radEncoding2_1.Checked)
                {
                    _encoding = Encoding.UTF8;
                }
                else
                {
                    _encoding = Encoding.GetEncoding("GB2312");
                }
            }
        }


    }
}
