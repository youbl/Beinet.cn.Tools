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
        private const int COL_RETURN = 3;
        private const int COL_REG = 2;
        private const int COL_DEL = 4;
        private const int COL_RECOMPARE = 5;
        private const int COL_OPEN = 6;
        private const string COLRECOMPARE_TXT = "执行";
        private const string COLDEL_TXT = "删除";
        public const string COLREG_TXT = "正则编辑";

        private ThreadScheduler _scheduler = new ThreadScheduler();

        public Compare()
        {
            InitializeComponent();

            // 设置protected属性，此属性指示此控件是否应使用辅助缓冲区重绘其图面，
            // 以 减少或避免闪烁（CellMouseEnter里修改背景色会导致闪烁）
            typeof(DataGridView).GetProperty("DoubleBuffered",
                BindingFlags.Instance | BindingFlags.NonPublic).SetValue(lvUrls, true, null);
            //lstRet.AutoResizeColumns(ColumnHeaderAutoResizeStyle.);
        }

        /// <summary>
        /// 窗体的点击事件
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void Compare_KeyDown(object sender, KeyEventArgs e)
        {
            //如果是菜单，请改用 menuSave.ShortcutKeys更好
            if (e.Control)
            {
                switch (e.KeyCode)
                {
                    // Ctrl+S保存
                    case Keys.S:
                        // 如果焦点在TextBox上时，可能把S输入，所以这里设置为true，以取消 KeyPress 事件
                        e.Handled = true;
                        btnSave.PerformClick();
                        //btnSave_Click(null, null);
                        break;
                    // Ctrl+R运行
                    case Keys.R:
                        e.Handled = true;
                        btnCompare.PerformClick();
                        break;
                    case Keys.L:
                        e.Handled = true;
                        btnLoad.PerformClick();
                        break;
                }
            }
        }


        #region 按钮事件
        /// <summary>
        /// 从配置文件加载
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
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

        private void btnLoad_StopClick(object sender, EventArgs e)
        {
            _stopLoad = true;
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

[globalReplace]
{2}

", txtCompareIp.Text, txtPublishServer.Text, GetRegexStr(lnkReg.Tag));

            int rowCnt = lvUrls.RowCount - 1; // 不处理未提交的行(即空白行)
            foreach (DataGridViewRow row in lvUrls.Rows)
            {
                if (row.Index >= rowCnt)
                    break;

                string url = Convert.ToString(row.Cells[COL_URL].Value);
                string post = Convert.ToString(row.Cells[COL_POST].Value);
                object objRegs = row.Cells[COL_REG].Tag;
                sb.AppendFormat(@"[url]
{0}
{1}
{2}

", url, post, GetRegexStr(objRegs));
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
            lstRet.Items.Clear(); // 清空结果集

            ThreadPool.UnsafeQueueUserWorkItem(DoCompare, null);
        }

        private void linkLabel1_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        {
            MessageBox.Show(@"Ctrl+L: 从配置文件加载
Ctrl+S: 保存全部到配置文件
Ctrl+R: 开始检查");
        }


        private void btnRemoveOK_Click(object sender, EventArgs e)
        {
            btnRemoveOK.Enabled = false;
            btnCompare.Enabled = false;
            ThreadPool.UnsafeQueueUserWorkItem(state =>
            {
                try
                {
                    int rowCnt = lvUrls.RowCount - 1; // 不处理未提交的行(即空白行)
                    //foreach (DataGridViewRow row in lvUrls.Rows)
                    for (var i = rowCnt - 1; i >= 0; i--)
                    {
                        int i1 = i;
                        if (Convert.ToString(lvUrls.Rows[i].Cells[COL_RETURN].Value) == "OK")
                        {
                            Utility.InvokeControl(lvUrls, () => lvUrls.Rows.RemoveAt(i1));
                        }
                    }
                }
                catch (Exception exp)
                {
                    MessageBox.Show("移除行时出错：\r\n" + exp);
                }
                finally
                {
                    btnRemoveOK.Enabled = true;
                    btnCompare.Enabled = true;
                }
            }, null);
        }
        #endregion


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


        #region DataGridView事件
        
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
                dgv.CurrentRow.Cells[COL_RECOMPARE].Value = COLRECOMPARE_TXT;
                dgv.CurrentRow.Cells[COL_DEL].Value = COLDEL_TXT;
                dgv.CurrentRow.Cells[COL_REG].Value = COLREG_TXT;
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
                return;
            }
            #endregion

            #region 点击编辑正则按钮
            if (!dgv.ReadOnly && e.RowIndex < dgv.RowCount - 1 && e.ColumnIndex == COL_REG)//Rows.Count - 1是不让点击未提交的新行
            {
                DataGridViewCell cellUrl = dgvRow.Cells[COL_URL];
                DataGridViewCell cellPost = dgvRow.Cells[COL_POST];
                DataGridViewCell cellReg = dgvRow.Cells[COL_REG];
                new CompareRegEdit(cellUrl, cellPost, cellReg).ShowDialog(this);
            }
            #endregion

            #region 点击重比按钮
            if (e.RowIndex < dgv.RowCount - 1 && e.ColumnIndex == COL_RECOMPARE)//Rows.Count - 1是不让点击未提交的新行
            {
                if (!CheckForm())
                    return;
                SetReadOnly();
                lstRet.Items.Clear(); // 清空结果集

                ThreadPool.UnsafeQueueUserWorkItem(DoCompare, e.RowIndex);
                return;
            }
            #endregion

            #region 点击结果按钮
            if (e.RowIndex < dgv.RowCount - 1 && e.ColumnIndex == COL_OPEN)//Rows.Count - 1是不让点击未提交的新行
            {
                string dir = Convert.ToString(dgvRow.Cells[COL_OPEN].Tag);
                if (!string.IsNullOrEmpty(dir))
                {
                    if (Directory.Exists(dir))
                        Process.Start(dir);
                    else
                        MessageBox.Show("目录不存在:" + dir);
                }
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
        #endregion


        /// <summary>
        /// 点击结果行时，同时选中DataGridView的相应行
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void lstRet_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (lstRet.SelectedItems.Count <= 0)
                return;
            ListViewItem item = lstRet.SelectedItems[0];
            string strIdx = item.SubItems[0].Text ?? "";
            if (strIdx == string.Empty)
                return;
            int idx;
            if (!int.TryParse(strIdx, out idx))
                return;
            if (idx >= lvUrls.RowCount)
                return;

            // 设置该行显示在第一行，并设置选中（注意Grid可能允许多选）
            lvUrls.FirstDisplayedScrollingRowIndex = idx - 1;
            lvUrls.Rows[idx - 1].Selected = true;
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

        private bool _stopLoad;
        void LoadInfoFromFile(string file)
        {
            if (!File.Exists(file))
                return;

            int speed;
            if (!int.TryParse(txtLoadSpeed.Text, out speed))
            {
                MessageBox.Show("文件加载速度不是数值");
                return;
            }
            if (speed <= 0)
                speed = 30;
            lvUrls.Rows.Clear();
            lstRet.Items.Clear(); // 清空结果集
            SetBtnEnable(false);
            ThreadPool.UnsafeQueueUserWorkItem(state =>
            {
                int addRowNum = 0;
                DateTime beginTime = DateTime.Now;
                try
                {
                    var arr = new Dictionary<object[], object>();
                    using (var sr = new StreamReader(file, Encoding.UTF8))
                    {
                        bool needReadTagUrl = true;
                        while (!_stopLoad && !sr.EndOfStream)
                        {
                            // 读取标记
                            string line;
                            if (needReadTagUrl)
                            {
                                line = (sr.ReadLine() ?? "").Trim();
                                if (sr.EndOfStream)
                                    break;
                                if (line == string.Empty)
                                    continue;
                            }
                            else
                            {
                                line = "[url]";
                            }

                            // 读取标记值
                            string value = (sr.ReadLine() ?? "").Trim();
                            if (value == string.Empty)
                            {
                                continue;
                            }

                            switch (line)
                            {
                                case "[beforePublishServerIp]":
                                    Utility.InvokeControl(txtCompareIp, () => { txtCompareIp.Text = value; });
                                    break;
                                case "[afterPublishServerIp]":
                                    Utility.InvokeControl(txtPublishServer, () => { txtPublishServer.Text = value; });
                                    break;
                                case "[globalReplace]":
                                    Utility.InvokeControl(lnkReg, () => { lnkReg.Tag = GetRegexArr(value); });
                                    break;
                                case "[url]":
                                    string url = value;
                                    if (!_regUrl.IsMatch(url))
                                        continue;
                                    string post = sr.EndOfStream ? string.Empty : (sr.ReadLine() ?? "").Trim();
                                    List<string[]> strRegs = null;
                                    if (post != "[url]")
                                    {
                                        string tmpReg = sr.EndOfStream ? string.Empty : sr.ReadLine() ?? "";

                                        needReadTagUrl = tmpReg != "[url]";
                                        if (!needReadTagUrl)
                                            tmpReg = string.Empty;

                                        strRegs = GetRegexArr(tmpReg);
                                    }
                                    else
                                    {
                                        needReadTagUrl = false;
                                        post = string.Empty;
                                    }
                                    
                                    var row = new object[COL_OPEN + 1];
                                    row[COL_URL] = url;
                                    row[COL_POST] = post;
                                    row[COL_REG] = COLREG_TXT;
                                    row[COL_DEL] = COLDEL_TXT;
                                    row[COL_RECOMPARE] = COLRECOMPARE_TXT;
                                    arr.Add(row, strRegs);
                                    if (arr.Count >= 10)
                                    {
                                        Utility.InvokeControl(lvUrls, () =>
                                        {
                                            foreach (var pair in arr)
                                            {
                                                int idx = lvUrls.Rows.Add(pair.Key);
                                                lvUrls.Rows[idx].Cells[COL_REG].Tag = pair.Value;// 正则替换的清单
                                            }
                                            addRowNum += arr.Count;
                                            lvUrls.FirstDisplayedCell = lvUrls.Rows[lvUrls.Rows.Count - 1].Cells[0];
                                        });
                                        arr.Clear();
                                        Thread.Sleep(speed);
                                    }
                                    break;
                            }
                        }
                    }// end using
                    if (arr.Count > 0)
                    {
                        Utility.InvokeControl(lvUrls, () =>
                        {
                            foreach (var pair in arr)
                            {
                                int idx = lvUrls.Rows.Add(pair.Key);
                                lvUrls.Rows[idx].Cells[COL_REG].Tag = pair.Value;// 正则替换的清单
                            }
                            addRowNum += arr.Count;
                            lvUrls.FirstDisplayedCell = lvUrls.Rows[lvUrls.Rows.Count - 1].Cells[0];
                        });
                    }
                }
                catch (Exception exp)
                {
                    MessageBox.Show("出错了:\r\n" + exp);
                }
                finally
                {
                    DateTime endTime = DateTime.Now;
                    string result = string.Format("耗时:{0}ms, 加载行数{1}", 
                        (endTime - beginTime).TotalMilliseconds.ToString("N0"), addRowNum.ToString("N0"));
                    ListViewItem item = new ListViewItem(new[] { "", result });
                    Utility.InvokeControl(lstRet, () => lstRet.Items.Add(item));
                    SetBtnEnable(true);
                }
            }, null);
        }

        void SetBtnEnable(bool enable)
        {
            _stopLoad = false;
            Utility.InvokeControl(btnCompare, () =>
            {
                if (enable)
                {
                    btnLoad.Text = "从配置文件加载..";
                    btnLoad.Click -= btnLoad_StopClick;
                    btnLoad.Click += btnLoad_Click;
                }
                else
                {
                    btnLoad.Text = "加载中，点击取消";
                    btnLoad.Click -= btnLoad_Click;
                    btnLoad.Click += btnLoad_StopClick;
                }
                btnCompare.Enabled = enable;
                btnSave.Enabled = enable;
                txtLoadSpeed.Enabled = enable;
                txtCompareIp.Enabled = enable;
                txtPublishServer.Enabled = enable;
            });
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

            string compareIp = txtCompareIp.Text.Trim();
            if (!_regIp.IsMatch(compareIp))
            {
                MessageBox.Show("请正确输入发布前IP");
                return false;
            }
            txtCompareIp.Text = compareIp;

            if (txtPublishServer.Text == string.Empty)
            {
                MessageBox.Show("请输入发布后IP");
                return false;
            }
            var servers = new StringBuilder();
            foreach (string item in txtPublishServer.Text.Split(new char[] { ';', ',', '|', ' ' }))
            {
                string ip = item.Trim();
                if (ip == string.Empty)
                    continue;
                if (!_regIp.IsMatch(ip))
                {
                    MessageBox.Show("请正确输入发布后IP:" + ip);
                    return false;
                }
                if (ip == compareIp)
                {
                    var confirm = MessageBox.Show("目标服务器IP中包含源服务器ip,确认要继续吗？", ip, MessageBoxButtons.YesNo,
                        MessageBoxIcon.Warning, MessageBoxDefaultButton.Button2);
                    if(confirm != DialogResult.Yes)
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
            // 统计总对比耗时用
            var begin = DateTime.Now;
            // 数组第0个元素用于统计任务个数，在后面进行阻塞
            // 数组第1个元素用于统计出错的行数
            int[] taskCnt = { 0, 0 };
            try
            {
                // 启动工作线程，去处理所有url
                int threadNum;
                if (!int.TryParse(txtThreadNum.Text, out threadNum))
                {
                    MessageBox.Show("请正确输入检查线程数");
                    return;
                }
                _scheduler.StartWork(threadNum);

                string compareIp = txtCompareIp.Text;

                string fileDir = Path.Combine(Utility.StartPath, "WebCompare\\" + DateTime.Now.ToString("yyyyMMddHHmmssfff"));

                object globalRegs = lnkReg.Tag;

                int idx = 0;
                int rowCnt = lvUrls.RowCount - 1; // 不处理未提交的行(即空白行)
                if (state != null)
                {
                    int rowIdx = (int) state;
                    if (rowIdx < 0 || rowIdx >= rowCnt)
                        return;
                    DataGridViewRow row = lvUrls.Rows[(int) state];
                    CompareRow(row, rowIdx + 1, compareIp, fileDir, globalRegs, taskCnt);
                }
                else
                {
                    foreach (DataGridViewRow row in lvUrls.Rows)
                    {
                        if (row.Index >= rowCnt)
                            break;

                        idx++;
                        CompareRow(row, idx, compareIp, fileDir, globalRegs, taskCnt);
                    }
                }
            }
            catch (Exception exp)
            {
                string errMsg = string.Format("对比时出错，循环中止：\r\n{0}", exp);
                ShowCompareErr("", errMsg);
            }
            finally
            {
                // 等待所有线程完成
                //while (_scheduler.QueueLength > 0)
                while (taskCnt[0] > 0)
                    Thread.Sleep(100);
                _scheduler.StopWork();
                SetReadOnly(false);

                string timeMsg = string.Format("全部对比完成，耗时:{0}秒，累计 {1} 个Url对比存在差异",
                    (DateTime.Now - begin).TotalSeconds.ToString("N2"),
                    taskCnt[1].ToString());
                ShowCompareErr("", timeMsg);
            }
        }

        void CompareRow(DataGridViewRow row, int rowIdx, string compareIp, string fileDir, object globalRegs,
             int[] taskCnt)
        {
            string url = Convert.ToString(row.Cells[COL_URL].Value);
            string post = Convert.ToString(row.Cells[COL_POST].Value);
            row.Cells[COL_RETURN].Value = string.Empty;
            bool isPost = !string.IsNullOrEmpty(post);
            object regs = row.Cells[COL_REG].Tag;

            //GetPage方法已经加了随机数
            //url = AddRndCh(url);

            // 任务加1
            Interlocked.Increment(ref taskCnt[0]);

            // 任务加入队列去执行
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

                        // 错误数加1
                        Interlocked.Increment(ref taskCnt[1]);
                        string errMsg = "对比源服务器返回空内容，无法继续对比";
                        ShowCompareErr(rowIdx.ToString(), errMsg);
                        return;
                    }

                    string compareFile = "compare" + compareIp + ".txt";
                    WriteFile(rowDir, compareFile, url, post, compareHtml);
                    // 重新读取文件，因为写入文件前换行符有所不同
                    compareHtml = ReadFile(rowDir, compareFile);

                    // 替换后的内容也写入文件
                    compareHtml = ReplaceWithRegex(ReplaceWithRegex(compareHtml, regs), globalRegs);
                    string replaceFile = "compare" + compareIp + "replace.txt";
                    WriteFile(rowDir, replaceFile, "", "", compareHtml);
                    compareHtml = ReadFile(rowDir, replaceFile);

                    if (compareHtml.Trim() == string.Empty)
                    {
                        // 任务减1
                        Interlocked.Decrement(ref taskCnt[0]);

                        // 错误数加1
                        Interlocked.Increment(ref taskCnt[1]);
                        string errMsg = "对比源服务器返回内容替换后为空，无法继续对比";
                        ShowCompareErr(rowIdx.ToString(), errMsg);
                        return;
                    }

                    string[] serverIps = txtPublishServer.Text.Split(new char[] { ';', ',', '|' },
                        StringSplitOptions.RemoveEmptyEntries);

                    string[] arrRet = new string[serverIps.Length];
                    int idxIp = 0;
                    bool hasDiff = false;
                    foreach (string ipItem in serverIps)
                    {
                        int idxUse = idxIp;
                        idxIp++;

                        ThreadPool.UnsafeQueueUserWorkItem(thArg =>
                        {
                            string ip = Convert.ToString(thArg);
                            string html;
                            try
                            {
                                html = Utility.GetPage(url, post, ip, isPost);

                                WriteFile(rowDir, ip + ".txt", url, post, html);
                                // 重新读取文件，因为换行不同
                                html = ReadFile(rowDir, ip + ".txt");


                                // 替换后的内容也写入文件
                                html = ReplaceWithRegex(ReplaceWithRegex(html, regs), globalRegs);
                                WriteFile(rowDir, ip + "replace.txt", "", "", html);
                                html = ReadFile(rowDir, ip + "replace.txt");
                            }
                            catch (Exception exp)
                            {
                                arrRet[idxUse] = string.Format("{0}(出错);", ip);
                                hasDiff = true;

                                string errMsg = string.Format("{0} 获取页面出错：{1}",
                                     ip, exp.Message);
                                ShowCompareErr(rowIdx.ToString(), errMsg);
                                return;
                            }
                            if (html != compareHtml)
                            {
                                hasDiff = true;
                                arrRet[idxUse] = string.Format("{0}({1});", ip, GetFirstDiff(html, compareHtml));
                            }
                            else
                            {
                                arrRet[idxUse] = string.Empty;
                            }
                        }, ipItem);
                    }

                    // 等待线程全部完成
                    while (arrRet.Contains(null))
                        Thread.Sleep(10);

                    string ret;
                    if (!hasDiff)
                        ret = "OK";
                    else
                    {
                        ret = arrRet.Aggregate("不一致:", (current, strRet) => current + strRet);
                        ShowCompareErr(rowIdx.ToString(), ret);
                        // 错误数加1
                        Interlocked.Increment(ref taskCnt[1]);
                    }
                    //ret += ". 对比结果参考目录" + rowDir;
                    Utility.InvokeControl(lvUrls, () =>
                    {
                        // 任务减1
                        Interlocked.Decrement(ref taskCnt[0]);
                        var resultrow = lvUrls.Rows[rowIdx - 1];
                        resultrow.Cells[COL_RETURN].Value = ret;

                        var resultcell = resultrow.Cells[COL_OPEN];
                        resultcell.Value = "结果";
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
                    ShowCompareErr(rowIdx.ToString(), exp.ToString());
                }

            }, null);
        }

        void ShowCompareErr(string rowIdx, string msg)
        {
            var lv = lstRet;
            Utility.InvokeControl(lv, () =>
            {
                string[] obj = new string[] { rowIdx, msg };
                ListViewItem item = new ListViewItem(obj);
                lv.Items.Insert(0, item);
            });
        }

        void WriteFile(string fileDir, string fileName,
            string url, string post, string content)
        {
            if (!Directory.Exists(fileDir))
                Directory.CreateDirectory(fileDir);
            using (var sw = new StreamWriter(Path.Combine(fileDir, fileName), false, Encoding.UTF8))
            {
                if (!string.IsNullOrEmpty(url))
                {
                    sw.WriteLine(url);
                    sw.WriteLine(post);
                    sw.WriteLine();
                }
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

        /// <summary>
        /// 用配置的正则进行替换
        /// </summary>
        /// <param name="html"></param>
        /// <param name="objRegs"></param>
        /// <returns></returns>
        static string ReplaceWithRegex(string html, object objRegs)
        {
            if (objRegs == null)
                return html;
            var arrReg = objRegs as List<string[]>;
            if (arrReg == null)
                return html;
            foreach (string[] item in arrReg)
            {
                if (item[0] == string.Empty)
                    continue;
                try
                {
                    string str = item[1].Replace(@"\r\n", "\r\n");
                    html = Regex.Replace(html, item[0], str);
                }
                // ReSharper disable once EmptyGeneralCatchClause
                catch{}
            }
            return html;
        }



        private const string SPLITLINE = "|||||";
        private const string SPLITITEM = "-----";
        /// <summary>
        /// 把配置的正则替换清单转换成字符串用于保存
        /// </summary>
        /// <param name="objRegs"></param>
        /// <returns></returns>
        string GetRegexStr(object objRegs)
        {
            if (objRegs == null)
                return "";
            var arrReg = objRegs as List<string[]>;
            if (arrReg == null)
                return "";
            StringBuilder sb = new StringBuilder();
            foreach (string[] item in arrReg)
            {
                if (item[0] == string.Empty)
                    continue;
                if (sb.Length > 0)
                    sb.Append(SPLITLINE);
                sb.AppendFormat("{0}{1}{2}", item[0], SPLITITEM, item[1]);
            }
            return sb.ToString();
        }
        /// <summary>
        /// 把字符串转换回正则替换清单
        /// </summary>
        /// <param name="strRegs"></param>
        /// <returns></returns>
        List<string[]> GetRegexArr(string strRegs)
        {
            List<string[]> ret = null;
            int idx;
            while ((idx = strRegs.IndexOf(SPLITLINE, StringComparison.Ordinal)) > 0)
            {
                string line = strRegs.Substring(0, idx);
                strRegs = strRegs.Substring(idx + SPLITLINE.Length);

                string[] item = SplitItem(line);
                if (item != null)
                {
                    if (ret == null)
                        ret = new List<string[]>();
                    ret.Add(item);
                }
            }
            if (strRegs != string.Empty)
            {
                string[] item = SplitItem(strRegs);
                if (item != null)
                {
                    if (ret == null)
                        ret = new List<string[]>();
                    ret.Add(item);
                }
            }
            return ret;
        }

        string[] SplitItem(string line)
        {
            int itemIdx = line.IndexOf(SPLITITEM, StringComparison.Ordinal);
            if (itemIdx > 0)
            {
                string reg = line.Substring(0, itemIdx);
                string replace = line.Substring(itemIdx + SPLITITEM.Length);
                return new string[] { reg, replace };
            }
            return null;
        }

        private void lnkReg_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        {
            new CompareRegEdit(lnkReg).ShowDialog(this);
        }

        /// <summary>
        /// 移除重复的行
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnRemoveLike_Click(object sender, EventArgs e)
        {
            bool compareUrlAndData = sender == btnRemoveAllLike;
            var allUrl = new HashSet<string>();
            var allData = new HashSet<string>();

            int rowCnt = lvUrls.RowCount - 1; // 不处理未提交的行(即空白行)
            int removeNum = 0;
            for (int i = 0; i < rowCnt; i++)
            {
                DataGridViewRow row = lvUrls.Rows[i];
                string url = Convert.ToString(row.Cells[COL_URL].Value);
                string post = Convert.ToString(row.Cells[COL_POST].Value);
                bool isNewUrl = allUrl.Add(url);
                bool isNewData = allData.Add(post);

                if (isNewUrl)
                {
                    continue;
                }
                if (compareUrlAndData && isNewData)
                {
                    continue;
                }
                lvUrls.Rows.RemoveAt(i);
                rowCnt--;
                i--;
                removeNum++;
            }
            lstRet.Items.Clear(); // 清空结果集
            ShowCompareErr("", "移除行数：" + removeNum.ToString());
        }
    }
}
