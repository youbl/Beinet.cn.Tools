using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Drawing;
using System.IO;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading;
using System.Windows.Forms;

namespace Beinet.cn.Tools.SqlInjectForm
{
    public partial class SqlInject : Form
    {
        private HashSet<string> _ignoreSqls;
        private string _ignoreFile = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "ignoreSql");

        public SqlInject()
        {
            InitializeComponent();
            
            if(File.Exists(_ignoreFile))
            {
                _ignoreSqls = Utility.XmlDeserialize<HashSet<string>>(_ignoreFile);
            }
            else
            {
                _ignoreSqls = new HashSet<string>();
            }
        }

        private void button1_Click(object sender, EventArgs e)
        {
            FolderBrowserDialog fbd = new FolderBrowserDialog();
            if (Directory.Exists(textBox1.Text))
                fbd.SelectedPath = textBox1.Text;
            else
            {
                fbd.SelectedPath = "E:\\";
            }
            if (fbd.ShowDialog(this) != DialogResult.OK)
                return;
            textBox1.Text = fbd.SelectedPath;
        }

        private void button2_Click(object sender, EventArgs e)
        {
            string dir = textBox1.Text;
            if(!Directory.Exists(dir))
            {
                ShowMsg("目录不存在");
                return;
            }
            string exts = txtExts.Text;
            lvNoErr.Items.Clear();
            lvErr.Items.Clear();
            idxNo = 0;
            idxErr = 0;
            labRet.Text = string.Empty;
            ThreadPool.UnsafeQueueUserWorkItem(ScanDir, new string[] { dir, exts });
        }

        private int idxNo = 0, idxErr = 0;
        void ScanDir(object state)
        {
            try
            {
                string[] arr = state as string[];
                if (arr == null)
                    return;

                string dir = Convert.ToString(arr[0]);
                if (string.IsNullOrEmpty(dir) || !Directory.Exists(dir))
                {
                    ShowMsg("目录不存在");
                    return;
                }
                string exts = Convert.ToString(arr[1]);
                string[] extsArr = exts.Split(new char[] {','}, StringSplitOptions.RemoveEmptyEntries);
                HashSet<string> exts2 = new HashSet<string>();
                foreach (string s in extsArr)
                {
                    string tmp = s.Trim().ToLower();
                    if (tmp != string.Empty)
                    {
                        if (tmp[0] != '.')
                            tmp = "." + tmp;
                        exts2.Add(tmp);
                    }
                }
                int files = ScanDirReal(dir, exts2);
                string msg = "扫描完成,共扫描文件数:" + files.ToString();
                ShowMsg(msg);
                Utility.SetValue(labRet, "Text", msg);
            }
            catch(Exception exp)
            {
                ShowMsg("扫描出错:\r\n" + exp.ToString());
            }
        }

        // 匹配sql直接拼接的
        static Regex _regAdd = new Regex(@"""\s*(?:select|delete|update|insert)\s+[^""]+""\s*\+[^;]+;",
            RegexOptions.IgnoreCase | RegexOptions.Compiled);
        // 匹配sql通过string.Format拼接的
        static Regex _regFormat = new Regex(@"\(\s*""\s*(?:select|delete|update|insert)\s+[^""]+""[^;]+;",
            RegexOptions.IgnoreCase | RegexOptions.Compiled);
        static Regex _regRowNum = new Regex(@"[\r\n]{1,2}", RegexOptions.Compiled);
        int ScanDirReal(string dir, HashSet<string> exts)
        {
            int ret = 0;
            foreach (string file in Directory.GetFiles(dir))
            {
                if (exts.Count > 0)
                {
                    string ext = Path.GetExtension(file);
                    if (string.IsNullOrEmpty(ext) || !exts.Contains(ext.ToLower()))
                        continue;
                }
                ret++;
                string txt;
                using(var sr = new StreamReader(file, Encoding.UTF8))
                {
                    txt = sr.ReadToEnd();
                }
                // 匹配测试2种情况
                bool success = false;
                Match m = _regAdd.Match(txt);
                while(m.Success)
                {
                    // 判断是否注释
                    string tmp = txt.Substring(0, m.Index);
                    // 获取sql所在行的开头
                    int idxRowStart = tmp.LastIndexOf('\n');
                    if (idxRowStart < 0)
                        idxRowStart = tmp.LastIndexOf('\r');
                    if (idxRowStart >= 0 && 
                        (tmp.Substring(idxRowStart).TrimStart().StartsWith("//") || tmp.Substring(idxRowStart).TrimStart().StartsWith("/*")))
                    {
                        m = m.NextMatch();// 是注释，跳过
                        continue;
                    }

                    // 是否忽略项
                    if (_ignoreSqls.Contains(m.Value))
                    {
                        m = m.NextMatch();
                        continue;
                    }

                    success = true;
                    idxErr++;

                    // 计算行号
                    int rownum = 1;
                    Match mRow = _regRowNum.Match(tmp);
                    while (mRow.Success)
                    {
                        rownum++;
                        mRow = mRow.NextMatch();
                    }

                    AddErrRow(file, m, rownum);

                    //string olderr = GetValue(txtErr, "Text").ToString();
                    //if (olderr != string.Empty)
                    //    olderr += "\r\n";
                    //SetValue(txtErr, "Text", olderr + "===发现SQL拼接 " + file + "\r\n" + m.Value);

                    m = m.NextMatch();
                }
                m = _regFormat.Match(txt);
                while (m.Success)
                {
                    // 判断是否注释
                    string tmp = txt.Substring(0, m.Index);
                    // 获取sql所在行的开头
                    int idxRowStart = tmp.LastIndexOf('\n');
                    if (idxRowStart < 0)
                        idxRowStart = tmp.LastIndexOf('\r');
                    if (idxRowStart >= 0 &&
                        (tmp.Substring(idxRowStart).TrimStart().StartsWith("//") || tmp.Substring(idxRowStart).TrimStart().StartsWith("/*")))
                    {
                        m = m.NextMatch();// 是注释，跳过
                        continue;
                    }
                    
                    // 是否忽略项
                    if (_ignoreSqls.Contains(m.Value))
                    {
                        m = m.NextMatch();
                        continue;
                    }

                    success = true;
                    idxErr++;

                    // 计算行号
                    //string tmp = txt.Substring(0, m.Index);
                    int rownum = 1;
                    Match mRow = _regRowNum.Match(tmp);
                    while(mRow.Success)
                    {
                        rownum++;
                        mRow = mRow.NextMatch();
                    }

                    AddErrRow(file, m, rownum);

                    //string olderr = GetValue(txtErr, "Text").ToString();
                    //if (olderr != string.Empty)
                    //    olderr += "\r\n";
                    //SetValue(txtErr, "Text", olderr + "===发现SQL拼接 " + file + "\r\n" + m.Value);

                    m = m.NextMatch();
                }
                if (!success)
                {
                    idxNo++;
                    string[] vals = new string[]
                                        {
                                            idxNo.ToString(),
                                            file,
                                        };
                    ListViewItem item = new ListViewItem(vals);
                    AddRow(lvNoErr, item);
                    //string oldscan = GetValue(txtScaned, "Text").ToString();
                    //if (oldscan != string.Empty)
                    //    oldscan += "\r\n";
                    //SetValue(txtScaned, "Text", oldscan + "未发现 " + file);
                }
            }

            foreach (string directory in Directory.GetDirectories(dir))
            {
                ret += ScanDirReal(directory, exts);
            }
            return ret;
        }

        void AddErrRow(string file, Match m, int rownum)
        {
            string[] vals = new string[]
                                        {
                                            idxErr.ToString(),
                                            file,
                                            m.Value,
                                            rownum.ToString(),
                                            "忽略这个SQL",
                                        };
            ListViewItem item = new ListViewItem(vals);
            item.Tag = m;
            AddRow(lvErr, item);
        }

        void AddRow(ListView lv, ListViewItem item)
        {
            Utility.InvokeControl(lvNoErr, () => lv.Items.Add(item));
        }

        private void lvNoErr_MouseDoubleClick(object sender, MouseEventArgs e)
        {
            if (lvNoErr.SelectedItems.Count <= 0)
                return;
            Process.Start("notepad.exe", lvNoErr.SelectedItems[0].SubItems[1].Text);
        }

        private void lvErr_MouseDoubleClick(object sender, MouseEventArgs e)
        {
            if (lvErr.SelectedItems.Count <= 0)
                return;
            new ErrDetail(lvErr.SelectedItems[0], this).ShowDialog(this);
        }


        /// <summary>
        /// 通过委托方法设置或隐藏Label
        /// </summary>
        /// <param name="lab"></param>
        /// <param name="txt"></param>
        void OperationLabelMethod(Label lab, string txt)
        {
            Utility.InvokeControl(lab, () =>
            {
                if (string.IsNullOrEmpty(txt))
                {
                    lab.Text = string.Empty;
                    lab.Visible = false;
                }
                else
                {
                    lab.Text = txt;
                    lab.Visible = true;
                }
            });
        }
        /// <summary>
        /// 显示信息,几秒后关闭
        /// </summary>
        /// <param name="msg"></param>
        void ShowMsg(string msg)
        {
            ThreadPool.UnsafeQueueUserWorkItem(state =>
            {
                TimeSpan ts = new TimeSpan(0, 0, 0, 1);
                for (int i = 3; i > 0; i--)
                {
                    OperationLabelMethod(labStatus, msg + "\r\n" + i + "秒后关闭");
                    Thread.Sleep(ts);
                }
                OperationLabelMethod(labStatus, null);
            }, null);
            //MessageBox.Show(msg);
        }

        private void lvErr_MouseClick(object sender, MouseEventArgs e)
        {
            if(e.Button == MouseButtons.Left && lvErr.SelectedItems.Count > 0)
            {
                var item = lvErr.SelectedItems[0];
                var idx = item.Index;
                Rectangle rectIgnore = lvErr.GetItemRect(idx);
                // 计算前3列的宽度，区域的Left加前3列宽度，就是第4列的Left
                int left2 = 0;
                for (int i = 0; i < 4; i++)
                {
                    left2 += lvErr.Columns[i].Width;
                }
                int left = rectIgnore.Left + left2;
                int top = rectIgnore.Top;
                int right = left + lvErr.Columns[4].Width;
                int bottom = top + rectIgnore.Height;
                if (left <= e.X && e.X <= right && top <= e.Y && e.Y <= bottom)
                {
                    //var diaRet = MessageBox.Show("以后扫描不再提示这条Sql", "确实要忽略吗？", MessageBoxButtons.YesNo, MessageBoxIcon.Warning,
                    //                             MessageBoxDefaultButton.Button2);
                    //if(diaRet == DialogResult.Yes)
                    {
                        IgnoreSql(item);
                        ShowMsg("已忽略，下次不会再提示这条sql，");
                    }
                }
                //rectIgnore.Width
            }
        }


        public void IgnoreSql(ListViewItem item)
        {
            string sql = ((Match)item.Tag).Value;
            _ignoreSqls.Add(sql);
            Utility.XmlSerialize(_ignoreSqls, _ignoreFile);
            lvErr.Items.RemoveAt(item.Index);
        }


    }
}
