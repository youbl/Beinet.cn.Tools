using System;
using System.Collections.Generic;
using System.Configuration;
using System.IO;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading;
using System.Windows.Forms;

namespace Beinet.cn.Tools
{
    public partial class CryptTool : Form
    {
        /// <summary>
        /// 允许拖入的文件扩展名
        /// </summary>
        private HashSet<string> _enableExt =
            new HashSet<string>(new String[] { "", ".aspx", ".asax", ".ashx", ".asp", ".config", ".txt", ".xml", ".ini", ".html", ".htm" });
        public CryptTool()
        {
            InitializeComponent();

            string pwd = ConfigurationManager.AppSettings["cryptPwd"] ?? "";
            string pwd2 = ConfigurationManager.AppSettings["cryptPwd2"] ?? pwd;
            txtPwd1.Text = pwd;
            txtPwd2.Text = pwd2;

            labScanStatus.Text = _enableExt.Aggregate("允许拖拽进入窗体的扩展名列表：", (s, s1) => s + " " + s1);
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

        private void CryptTool_DragDrop(object sender, DragEventArgs e)
        {
            var obj = e.Data.GetData(DataFormats.FileDrop);
            if (obj == null)
            {
                return;
            }
            // 使用第一个密码
            Init(btnDes1);

            ThreadPool.UnsafeQueueUserWorkItem(DoUnCrypt, obj);
        }

        private void CryptTool_DragEnter(object sender, DragEventArgs e)
        {
            if (e.Data.GetDataPresent(DataFormats.FileDrop))
            {
                e.Effect = DragDropEffects.Link;
            }
            else
            {
                e.Effect = DragDropEffects.None;
            }
        }


        void DoUnCrypt(object state)
        {
            // 处理files文件列表（可能是目录和文件混合）
            string[] files = state as string[];
            if (files == null || files.Length < 1)
            {
                return;
            }

            TextBox txt = txtRet;
            Utility.InvokeControl(txt, () => txt.Text = "");
            var ret = LoopDir(files, txt);
            Utility.InvokeControl(labScanStatus, () =>
            {
                labScanStatus.Text = "找到" + ret[0].ToString() +
                           "个文件中的" + ret[1].ToString() +
                           "个加密串  注：使用第一个密码和编码进行解密哦";
            });
        }

        int[] LoopDir(string[] arr, TextBox txt)
        {
            int[] ret = {0, 0};
            foreach (string item in arr)
            {
                string item1 = item;
                Utility.InvokeControl(labScanStatus, () => labScanStatus.Text = item1 + "扫描中...");
                if (Directory.Exists(item))
                {
                    if (item.EndsWith(".svn") || item.EndsWith(".git"))
                        continue;
                    
                    var nums = LoopDir(Directory.GetFiles(item), txt);
                    ret[0] += nums[0];
                    ret[1] += nums[1];
                    nums = LoopDir(Directory.GetDirectories(item), txt);
                    ret[0] += nums[0];
                    ret[1] += nums[1];
                }
                else if (File.Exists(item))
                {
                    string ext = Path.GetExtension(item) ?? "";
                    if (!_enableExt.Contains(ext.ToLower()))
                        continue;
                    
                    int undesNum = UnCryptFile(item, txt);
                    if (undesNum > 0)
                    {
                        ret[0]++;
                        ret[1] += undesNum;
                    }
                }
            }
            return ret;
        }


        int UnCryptFile(string file, TextBox txt)
        {
            bool fileShowed = false;
            int desNum = 0;
            // 只处理小于2m的文件
            if (File.Exists(file) && new FileInfo(file).Length <= 2000000)
            {
                try
                {
                    string content;
                    using (var sr = new StreamReader(file, _encoding))
                    {
                        content = sr.ReadToEnd();
                    }
                    HashSet<string> desStrs = new HashSet<string>();
                    // DES加密的结果都是ToString("X2")，因此匹配0-9A-Fa-f
                    Match match = Regex.Match(content, "([^\\s]+)=\"([0-9A-Fa-f]+)\"");
                    while (match.Success)
                    {
                        string attName = match.Groups[1].Value;
                        string attValue = match.Groups[2].Value;
                        if (!desStrs.Contains(attValue))
                        {
                            try
                            {
                                string value = Utility.DES_Decrypt(attValue, _pwd, _encoding);
                                desNum++;
                                desStrs.Add(attValue);
                                Utility.InvokeControl(txt,
                                    () =>
                                    {
                                        if (!fileShowed)
                                        {
                                            fileShowed = true;
                                            txt.Text += "文件: " + file + "\r\n";
                                        }
                                        txt.Text += string.Format("    {0}={1}\r\n        {2}\r\n", attName, attValue, value);
                                    });
                            }
                            // ReSharper disable once EmptyGeneralCatchClause
                            catch (Exception)
                            {
                            }
                        }
                        else
                        {
                            desNum++;
                        }
                        match = match.NextMatch();
                    }
                    if (fileShowed)
                    {
                        Utility.InvokeControl(txt, () => txt.Text += "\r\n");
                    }
                }
                catch (Exception exp)
                {
                    Utility.InvokeControl(txt, () => txt.Text += exp + "\r\n");
                }
            }
            return desNum;
        }
    }
}
