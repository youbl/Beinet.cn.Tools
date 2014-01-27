using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Diagnostics;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading;
using System.Windows.Forms;

namespace Beinet.cn.Tools.WebContentCompare
{
    public partial class Compare : Form
    {
        /// <summary>
        /// 验证ip的正则
        /// </summary>
        static Regex _regIp = new Regex(@"^\d{1,3}\.\d{1,3}\.\d{1,3}\.\d{1,3}$", RegexOptions.Compiled);
        /// <summary>
        /// 验证url的正则
        /// </summary>
        static Regex _regUrl = new Regex(@"^(?i:https?)://[^\r\n]+$", RegexOptions.Compiled);

        private const int COL_URL = 0;
        private const int COL_POST = 1;
        private const int COL_RETURN = 2;
        private const int COL_DEL = 3;
        private const int COL_OPEN = 4;

        private ThreadScheduler _scheduler = new ThreadScheduler();

        public Compare()
        {
            InitializeComponent();

            // 设置protected属性，此属性指示此控件是否应使用辅助缓冲区重绘其图面，
            // 以 减少或避免闪烁（CellMouseEnter里修改背景色会导致闪烁）
            typeof(DataGridView).GetProperty("DoubleBuffered",
                BindingFlags.Instance | BindingFlags.NonPublic).SetValue(lvUrls, true, null);
        }


        private void btnLoad_Click(object sender, EventArgs e)
        {
            OpenFileDialog ofd = new OpenFileDialog();
            ofd.Multiselect = false;
            var result = ofd.ShowDialog(this);
            if (result != DialogResult.OK)
            {
                return;
            }
            LoadInfoFromFile(ofd.FileName);
        }

        /// <summary>
        /// 保存到配置文件
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnSave_Click(object sender, EventArgs e)
        {
            if (!CheckForm())
                return;

            var sfd = new SaveFileDialog();
            var result = sfd.ShowDialog(this);
            if (result != DialogResult.OK)
            {
                return;
            }
            string filename = sfd.FileName;

            var sb = new StringBuilder(800);
            sb.AppendFormat(@"[beforePublishServerIp]
{0}

[afterPublishServerIp]
{1}

", txtCompareIp.Text, txtPublishServer.Text);

            int rowCnt = lvUrls.RowCount - 1; // 不处理未提交的行(即空白行)
            foreach (DataGridViewRow row in lvUrls.Rows)
            {
                if (row.Index >= rowCnt)
                    break;

                string url = Convert.ToString(row.Cells[COL_URL].Value);
                string post = Convert.ToString(row.Cells[COL_POST].Value);
                sb.AppendFormat(@"[url]
{0}
{1}

", url, post);
            }
            using (var sw = new StreamWriter(filename, false, Encoding.UTF8))
            {
                sw.Write(sb.ToString());
            }
        }

        private void btnCompare_Click(object sender, EventArgs e)
        {
            if (!CheckForm())
                return;

            SetReadOnly();
            ThreadPool.UnsafeQueueUserWorkItem(DoCompare, null);
        }


        #region 文件拖拽进来的处理,要先设置Form.AllowDrop为true

        private void Compare_DragDrop(object sender, DragEventArgs e)
        {
            var obj = e.Data.GetData(DataFormats.FileDrop);
            if (obj == null)
            {
                return;
            }
            //MessageBox.Show(obj.ToString());
            string[] files = obj as string[];
            if (files == null || files.Length < 1)
            {
                return;
            }

            LoadInfoFromFile(files[0]);
        }

        /// <summary>
        /// 拖拽文件进来时，才允许
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void Compare_DragEnter(object sender, DragEventArgs e)
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
        #endregion


        /// <summary>
        /// 通过输入文字添加新行时也会触发
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void lvUrls_CurrentCellDirtyStateChanged(object sender, EventArgs e)
        {
            var dgv = lvUrls;
            if (dgv.CurrentRow == null)
                return;

            // 如果是新行，给最后一格添加删除字样
            if (dgv.CurrentRow.Cells[COL_DEL].Value == null)
            {
                dgv.CurrentRow.Cells[COL_DEL].Value = "删除";
            }

            dgv.CommitEdit(DataGridViewDataErrorContexts.Commit);
        }

        private void lvUrls_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            if (e.RowIndex < 0 || e.ColumnIndex < 0)
                return;
            var dgv = lvUrls;

            DataGridViewRow dgvRow = dgv.Rows[e.RowIndex];
            #region 点击删除按钮
            if (!dgv.ReadOnly && e.RowIndex < dgv.RowCount - 1 && e.ColumnIndex == COL_DEL)//Rows.Count - 1是不让点击未提交的新行
            {
                dgv.Rows.RemoveAt(e.RowIndex);
                dgv.ClearSelection();
            }
            #endregion

            #region 点击目录按钮
            if (e.RowIndex < dgv.RowCount - 1 && e.ColumnIndex == COL_OPEN)//Rows.Count - 1是不让点击未提交的新行
            {
                string dir = Convert.ToString(dgvRow.Cells[COL_OPEN].Tag);
                if (!string.IsNullOrEmpty(dir) && Directory.Exists(dir))
                    Process.Start(dir);
            }
            #endregion
        }

        private void dataGridView1_RowsAdded(object sender, DataGridViewRowsAddedEventArgs e)
        {
            var dgv = (DataGridView)sender;
            //for (int i = 0; i < e.RowCount; i++)
            //{
            //    dgv.Rows[e.RowIndex + i].HeaderCell.Style.Alignment = DataGridViewContentAlignment.MiddleRight;
            //    dgv.Rows[e.RowIndex + i].HeaderCell.Value = (e.RowIndex + i + 1).ToString();
            //}
            //for (int i = e.RowIndex + e.RowCount; i < dgv.Rows.Count; i++)
            for (int i = 0; i < dgv.Rows.Count; i++)
            {
                //dgv.Rows[i].HeaderCell.Style.Alignment = DataGridViewContentAlignment.MiddleRight;
                dgv.Rows[i].HeaderCell.Value = (i + 1).ToString();
            }
        }

        private void dataGridView1_RowsRemoved(object sender, DataGridViewRowsRemovedEventArgs e)
        {
            var dgv = (DataGridView)sender;
            //for (int i = 0; i < e.RowCount && i < dgv.Rows.Count; i++)
            //{
            //    dgv.Rows[e.RowIndex + i].HeaderCell.Style.Alignment = DataGridViewContentAlignment.MiddleRight;
            //    dgv.Rows[e.RowIndex + i].HeaderCell.Value = (e.RowIndex + i + 1).ToString();
            //}
            //for (int i = e.RowIndex + e.RowCount; i < dgv.Rows.Count; i++)
            for (int i = 0; i < dgv.Rows.Count; i++)
            {
                //dgv.Rows[i].HeaderCell.Style.Alignment = DataGridViewContentAlignment.MiddleRight;
                dgv.Rows[i].HeaderCell.Value = (i + 1).ToString();
            }
        }

        private void Compare_KeyDown(object sender, KeyEventArgs e)
        {
            //如果是菜单，请改用 menuSave.ShortcutKeys更好
            if (e.Control)
            {
                // Ctrl+S保存
                if (e.KeyCode == Keys.S)
                {
                    // 如果焦点在TextBox上时，可能把S输入，所以这里设置为true，以取消 KeyPress 事件
                    e.Handled = true;
                    btnSave.PerformClick();
                    //btnSave_Click(null, null);
                }
            }
        }


        void SetReadOnly(bool readOnly = true)
        {
            Utility.InvokeControl(lvUrls, () =>
            {
                txtCompareIp.Enabled = !readOnly;
                txtPublishServer.Enabled = !readOnly;
                btnLoad.Enabled = !readOnly;
                btnCompare.Enabled = !readOnly;

                var dgv = lvUrls;
                dgv.ReadOnly = readOnly;
                if (readOnly)
                {
                    dgv.DefaultCellStyle.BackColor = Color.Gainsboro;
                }
                else
                {
                    dgv.DefaultCellStyle.BackColor = Color.White;
                }
            });
        }

        void LoadInfoFromFile(string file)
        {
            if (!File.Exists(file))
                return;

            lvUrls.Rows.Clear();

            using (var sr = new StreamReader(file, Encoding.UTF8))
            {
                while (!sr.EndOfStream)
                {
                    // 读取标记
                    string line = (sr.ReadLine() ?? "").Trim();
                    if (sr.EndOfStream)
                        break;

                    if (line == string.Empty)
                        continue;

                    // 读取标记值
                    string value = (sr.ReadLine() ?? "").Trim();
                    if (value == string.Empty)
                    {
                        continue;
                    }

                    switch (line)
                    {
                        case "[beforePublishServerIp]":
                            txtCompareIp.Text = value;
                            break;
                        case "[afterPublishServerIp]":
                            txtPublishServer.Text = value;
                            break;
                        case "[url]":
                            string url = value;
                            if (!_regUrl.IsMatch(url))
                                continue;
                            string post = (sr.ReadLine() ?? "").Trim();
                            var row = new object[COL_OPEN + 1];
                            row[COL_URL] = url;
                            row[COL_POST] = post;
                            row[COL_DEL] = "删除";
                            lvUrls.Rows.Add(row);
                            break;
                    }
                }
            }
        }

        bool CheckForm()
        {
            if (lvUrls.RowCount <= 1)// 会存在一行空白行
            {
                MessageBox.Show("请添加至少一条URL");
                return false;
            }

            int rowCnt = lvUrls.RowCount - 1; // 不处理未提交的行(即空白行)
            int idx = 0;
            foreach (DataGridViewRow row in lvUrls.Rows)
            {
                if (row.Index >= rowCnt)
                    break;

                idx++;
                string url = Convert.ToString(row.Cells[COL_URL].Value);
                //string post = Convert.ToString(row.Cells[COL_POST].Value);
                string useUrl = url.Trim();

                // 删除url里的锚点
                int anchorIdx = useUrl.IndexOf('#');
                if (anchorIdx > 0)
                    useUrl = useUrl.Substring(0, anchorIdx);

                if (!_regUrl.IsMatch(useUrl))
                {
                    string msg = string.Format("第{0}行的url格式不正确\r\n{1}", idx.ToString(), useUrl);
                    MessageBox.Show(msg);
                    return false;
                }

                if (useUrl.Length != url.Length)
                {
                    row.Cells[COL_URL].Value = url;
                }
            }

            txtCompareIp.Text = txtCompareIp.Text.Trim();
            if (!_regIp.IsMatch(txtCompareIp.Text))
            {
                MessageBox.Show("请正确输入发布前IP");
                return false;
            }

            if (txtPublishServer.Text == string.Empty)
            {
                MessageBox.Show("请输入发布后IP");
                return false;
            }
            var servers = new StringBuilder();
            foreach (string item in txtPublishServer.Text.Split(';'))
            {
                string ip = item.Trim();
                if (ip == string.Empty)
                    continue;
                if (!_regIp.IsMatch(ip))
                {
                    MessageBox.Show("请正确输入发布后IP:" + ip);
                    return false;
                }
                servers.AppendFormat("{0};", ip);
            }
            if (servers.Length == 0)
            {
                MessageBox.Show("请正确输入发布后IP");
                return false;
            }
            txtPublishServer.Text = servers.ToString();

            return true;
        }

        // 主调程序
        void DoCompare(object state)
        {
            int[] taskCnt = { 0 };
            try
            {
                _scheduler.StartWork(5);

                string compareIp = txtCompareIp.Text;

                string fileDir = Path.Combine(Utility.StartPath, "WebCompare\\" + DateTime.Now.ToString("yyyyMMddHHmmssfff"));

                int idx = 0;
                int rowCnt = lvUrls.RowCount - 1; // 不处理未提交的行(即空白行)
                foreach (DataGridViewRow row in lvUrls.Rows)
                {
                    if (row.Index >= rowCnt)
                        break;

                    idx++;
                    string url = Convert.ToString(row.Cells[COL_URL].Value);
                    string post = Convert.ToString(row.Cells[COL_POST].Value);
                    row.Cells[COL_RETURN].Value = string.Empty;
                    bool isPost = !string.IsNullOrEmpty(post);

                    //GetPage方法已经加了随机数
                    //url = AddRndCh(url);

                    int rowIdx = idx;

                    // 任务加1
                    Interlocked.Increment(ref taskCnt[0]);

                    _scheduler.QueueUserWorkItem(obj =>
                    {
                        try
                        {
                            string rowDir = Path.Combine(fileDir, rowIdx.ToString());
                            string compareHtml = Utility.GetPage(url, post, compareIp, isPost) ?? string.Empty;
                            if (compareHtml.Trim() == string.Empty)
                            {
                                // 任务减1
                                Interlocked.Decrement(ref taskCnt[0]);
                                string errMsg = string.Format("第{0}行：对比源服务器返回空内容，无法继续对比", rowIdx.ToString());
                                ShowCompareErr(errMsg);
                                return;
                            }

                            WriteFile(rowDir, "compare" + compareIp + ".txt", url, post, compareHtml);
                            // 重新读取文件，因为换行不同
                            compareHtml = ReadFile(rowDir, "compare" + compareIp + ".txt");

                            string ret = string.Empty;
                            string[] serverIps = txtPublishServer.Text.Split(new char[] {';'},
                                StringSplitOptions.RemoveEmptyEntries);
                            foreach (string ip in serverIps)
                            {
                                string html;
                                try
                                {
                                    html = Utility.GetPage(url, post, ip, isPost);
                                }
                                catch (Exception exp)
                                {
                                    string errMsg = string.Format("第{0}行：{2}页面出错：{1}", 
                                        rowIdx.ToString(), exp.Message, ip);
                                    ShowCompareErr(errMsg);
                                    ret = string.Format("{0}{1}(出错);", ret, ip);
                                    continue;
                                }
                                WriteFile(rowDir, ip + ".txt", url, post, html);
                                // 重新读取文件，因为换行不同
                                html = ReadFile(rowDir, ip + ".txt");
                                if (html != compareHtml)
                                {
                                    ret = string.Format("{0}{1}({2});", ret, ip, GetFirstDiff(html, compareHtml));
                                }
                            }
                            if (ret == string.Empty)
                                ret = "OK";
                            else
                                ret = "不一致:" + ret;
                            //ret += ". 对比结果参考目录" + rowDir;
                            Utility.InvokeControl(lvUrls, () =>
                            {
                                // 任务减1
                                Interlocked.Decrement(ref taskCnt[0]);
                                var resultrow = lvUrls.Rows[rowIdx - 1];
                                resultrow.Cells[COL_RETURN].Value = ret;

                                var resultcell = resultrow.Cells[COL_OPEN];
                                resultcell.Value = "结果目录";
                                resultcell.Tag = rowDir;
                                //if (ret != "OK")
                                //{
                                //    cell.Style.BackColor 
                                //}
                            });
                        }
                        catch (Exception exp)
                        {
                            // 任务减1
                            Interlocked.Decrement(ref taskCnt[0]);
                            string errMsg = string.Format("第{0}行：对比出错：\r\n{1}", rowIdx.ToString(), exp);
                            ShowCompareErr(errMsg);
                        }

                    }, null);
                }

            }
            catch (Exception exp)
            {
                string errMsg = string.Format("对比时出错，循环中止：\r\n{0}", exp);
                ShowCompareErr(errMsg);
            }
            finally
            {
                // 等待所有线程完成
                //while (_scheduler.QueueLength > 0)
                while (taskCnt[0] > 0)
                    Thread.Sleep(100);
                _scheduler.StopWork();
                SetReadOnly(false);
            }
        }

        void ShowCompareErr(string msg)
        {
            Utility.InvokeControl(txtErr, () =>
            {
                txtErr.Text = string.Format("{0}\r\n\r\n{1}", msg, txtErr.Text);
            });
        }

        void WriteFile(string fileDir, string fileName,
            string url, string post, string content)
        {
            if (!Directory.Exists(fileDir))
                Directory.CreateDirectory(fileDir);
            using (var sw = new StreamWriter(Path.Combine(fileDir, fileName), false, Encoding.UTF8))
            {
                sw.WriteLine(url);
                sw.WriteLine(post);
                sw.WriteLine();
                sw.Write(content);
            }
        }

        string ReadFile(string fileDir, string fileName)
        {
            using (var stre = new StreamReader(Path.Combine(fileDir, fileName), Encoding.UTF8))
            {
                return stre.ReadToEnd();
            }
        }


        /// <summary>
        /// 获取2个字符串的第1个不同的字符在第几行第几位 row,column
        /// </summary>
        /// <param name="str1"></param>
        /// <param name="str2"></param>
        /// <returns></returns>
        string GetFirstDiff(string str1, string str2)
        {
            int len1 = str1.Length;
            int len2 = str2.Length;
            int row = 1;
            int col = 0;
            int i;
            for (i = 0; i < len1 && i < len2; i++)
            {
                char ch1 = str1[i];
                char ch2 = str2[i];
                if (ch1 != ch2)
                {
                    break;
                }
                if (ch1 == '\r' || ch1 == '\n')
                {
                    int next = i + 1;
                    if (next >= len1 || next >= len2)
                        break;

                    ch1 = str1[next];
                    ch2 = str2[next];
                    if ((ch1 == '\r' || ch1 == '\n') && (ch2 == '\r' || ch2 == '\n'))
                        i++;

                    row++;
                    col = 0;
                }
                else
                {
                    col++;
                }
            }
            if (i < len1 || i < len2)
                return string.Format("{0},{1}", row.ToString(), col.ToString());

            return "0,0";
        }
    }
}
