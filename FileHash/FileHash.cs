using System;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Threading;
using System.Windows.Forms;
using System.Xml;

namespace Beinet.cn.Tools.FileHash
{
    public partial class FileHash : Form
    {
        private bool _stop = false;
        private DateTime _start;

        /// <summary>
        /// 用于查找相同文件的变量
        /// </summary>
        private Dictionary<string, string> _md5list;
        /// <summary>
        /// 收集查找到的相同文件的变量
        /// </summary>
        private Dictionary<string, HashSet<string>> _md5samelist; 

        public FileHash()
        {
            InitializeComponent();
        }

        private void btnSelectFile_Click(object sender, EventArgs e)
        {            
            var ofd = new FolderBrowserDialog();
            if(string.IsNullOrEmpty(ofd.SelectedPath))
            {
                ofd.SelectedPath = AppDomain.CurrentDomain.BaseDirectory;
            }
            if (ofd.ShowDialog(this) != DialogResult.OK)
                return;

            DoCountMd5(ofd.SelectedPath);
        }

        private void btnClear_Click(object sender, EventArgs e)
        {
            dataGridView1.Columns.Clear();
            //dataGridView1.Rows.Clear();
        }

        private void btnCopy_Click(object sender, EventArgs e)
        {
            Clipboard.SetText(GetContents());
            MessageBox.Show("已复制到剪贴板");
        }

        private void btnSave_Click(object sender, EventArgs e)
        {
            var ofd = new SaveFileDialog();
            if (ofd.ShowDialog(this) != DialogResult.OK)
                return;
            using(var sw = new StreamWriter(ofd.FileName, false, Encoding.UTF8))
            {
                sw.Write(GetContents());
            }
            MessageBox.Show("保存成功");
        }

        private void btnStop_Click(object sender, EventArgs e)
        {
            _stop = true;
        }

        void CountMd5(object state)
        {
            try
            {
                string[] files = state as string[];
                if (files == null || files.Length == 0)
                    return;

                string root;
                if (files.Length == 1 && Directory.Exists(files[0]))
                {
                    root = files[0];
                }
                else
                {
                    root = Path.GetDirectoryName(files[0]);
                }

                int cnt = CountDirMd5(files, root);
                if (chkSameFile.Checked)
                {
                    // 用于不同Md5变色
                    var arrColorIdx = new List<int>();
                    var arrRow = new List<string[]>();
                    int idx = 0;

                    foreach (KeyValuePair<string, HashSet<string>> pair in _md5samelist)
                    {
                        string md5 = pair.Key;
                        int colorIdx = idx % 2;
                        idx++;
                        foreach (string file in pair.Value)
                        {
                            string[] tmp = (md5 + "|" + file).Split('|');
                            arrRow.Add(tmp);
                            arrColorIdx.Add(colorIdx);
                        }
                    }
                    // 绑定到DataGridView
                    Utility.BindToDataGrid(dataGridView1, arrRow, arrColorIdx);
                }
                _stop = true;
                MessageBox.Show("处理的文件和目录个数：" + cnt.ToString());
            }
            catch(Exception exp)
            {
                _stop = true;
                MessageBox.Show("出错了：\r\n" + exp.Message);
            }
            finally
            {
                _stop = true;
                Utility.InvokeControl(btnSelectFile,()=>
                                                        {
                                                            btnSelectFile.Enabled = true;
                                                            btnStop.Enabled = false;
                                                        });
            }
        }


        int CountDirMd5(IEnumerable<string> dirsOrFiles, string root)
        {
            int cnt = 0;
            if (_stop)
            {
                return cnt;
            }
            var arrRow = new List<string[]>();
            foreach (string file in dirsOrFiles)
            {
                if (_stop)
                {
                    // 绑定到DataGridView
                    Utility.BindToDataGrid(dataGridView1, arrRow);
                    return cnt;
                }
                if (File.Exists(file))
                {
                    string md5, sha1;
                    using (MD5CryptoServiceProvider get_md5 = new MD5CryptoServiceProvider())
                    using (SHA1CryptoServiceProvider get_sha1 = new SHA1CryptoServiceProvider())
                    using (FileStream get_file = new FileStream(file, FileMode.Open, FileAccess.Read, FileShare.Read))
                    {
                        get_file.Seek(0, SeekOrigin.Begin);
                        md5 = BitConverter.ToString(get_md5.ComputeHash(get_file)).Replace("-", "");
                        get_file.Seek(0, SeekOrigin.Begin);
                        sha1 = BitConverter.ToString(get_sha1.ComputeHash(get_file)).Replace("-", "");
                    }
                    string file1 = file.Replace(root, "");
                    if (chkSameFile.Checked)
                    {
                        // 查找相同文件时的收集工作
                        string tmp;
                        if (_md5list.TryGetValue(md5, out tmp))
                        {
                            HashSet<string> tmpSameFile;
                            if (!_md5samelist.TryGetValue(md5, out tmpSameFile))
                            {
                                tmpSameFile = new HashSet<string>();
                                tmpSameFile.Add(tmp);
                                _md5samelist[md5] = tmpSameFile;
                            }
                            tmpSameFile.Add(file + "|" + sha1);
                        }
                        else
                        {
                            _md5list.Add(md5, file + "|" + sha1);
                        }
                    }
                    else
                    {
                        arrRow.Add(new string[]{ file1, md5, sha1 });
                    }
                    
                    cnt++;
                }
                else if (Directory.Exists(file))
                {
                    string fname = Path.GetFileName(file.TrimEnd('\\')).ToLower();
                    if (fname != string.Empty || !Utility.DirNoProcess.Contains(fname))
                    {
                        cnt += CountDirMd5(Directory.GetFiles(file), root);
                        cnt += CountDirMd5(Directory.GetDirectories(file), root);
                    }
                }
            }
            // 绑定到DataGridView
            Utility.BindToDataGrid(dataGridView1, arrRow);
            return cnt;
        }

        void AddCol(string dir)
        {
            var dgv = dataGridView1;
            DataGridViewColumn column;
            if (chkSameFile.Checked)
            {
                column = new DataGridViewTextBoxColumn();
                column.HeaderText = "MD5";
                column.AutoSizeMode = DataGridViewAutoSizeColumnMode.DisplayedCells;
                dgv.Columns.Add(column);
                column = new DataGridViewTextBoxColumn();
                column.HeaderText = "文件";
                column.AutoSizeMode = DataGridViewAutoSizeColumnMode.DisplayedCells;
                dgv.Columns.Add(column);
                column = new DataGridViewTextBoxColumn();
                column.HeaderText = "SHA1";
                column.AutoSizeMode = DataGridViewAutoSizeColumnMode.DisplayedCells;
                dgv.Columns.Add(column);
            }
            else
            {
                column = new DataGridViewTextBoxColumn();
                column.HeaderText = "文件名(目录:" + dir + ")";
                column.AutoSizeMode = DataGridViewAutoSizeColumnMode.DisplayedCells;
                dgv.Columns.Add(column);
                column = new DataGridViewTextBoxColumn();
                column.HeaderText = "本地MD5";
                column.AutoSizeMode = DataGridViewAutoSizeColumnMode.DisplayedCells;
                dgv.Columns.Add(column);
                column = new DataGridViewTextBoxColumn();
                column.HeaderText = "本地SHA1";
                column.AutoSizeMode = DataGridViewAutoSizeColumnMode.DisplayedCells;
                dgv.Columns.Add(column);
            }
            //dgv.AutoSize = true;
        }

        string GetContents()
        {
            var dgv = dataGridView1;
            StringBuilder sb = new StringBuilder(1000);
            foreach (DataGridViewColumn column in dgv.Columns)
            {
                sb.AppendFormat("{0}\t", column.HeaderText);
            }
            sb.Append("\r\n");
            foreach (DataGridViewRow row in dgv.Rows)
            {
                foreach (DataGridViewCell cell in row.Cells)
                {
                    sb.AppendFormat("{0}\t", cell.Value);
                }
                sb.Append("\r\n");
            }
            return sb.ToString();
        }

        void DoCountMd5(params string[] dirsOrFiles)
        {
            if (chkSameFile.Checked)
            {
                _md5list = new Dictionary<string, string>();
                _md5samelist = new Dictionary<string, HashSet<string>>();
            }
            btnSelectFile.Enabled = false;
            btnStop.Enabled = true;
            _stop = false;
            _start = DateTime.Now;

            ThreadPool.UnsafeQueueUserWorkItem(state => {
                // 计时程序
                var ts = TimeSpan.FromSeconds(1);
                while (!_stop)
                {
                    Thread.Sleep(ts);
                    var timeDiff = (DateTime.Now - _start).ToString(@"d\.hh\:mm\:ss");
                    Utility.InvokeControl(btnStop, () => btnStop.Text = "停止计算\r\n" + timeDiff);
                }
            }, null);

            btnClear_Click(null, null);

            string path;
            if (dirsOrFiles.Length == 1 && Directory.Exists(dirsOrFiles[0]))
            {
                path = dirsOrFiles[0];
            }
            else
            {
                path = Path.GetDirectoryName(dirsOrFiles[0]);
            }
            // 初始化Grid
            AddCol(path);
            ThreadPool.UnsafeQueueUserWorkItem(CountMd5, dirsOrFiles);
        }

        #region 文件拖拽进来的处理,要先设置Form.AllowDrop为true

        private void Form1_DragDrop(object sender, DragEventArgs e)
        {
            var obj = e.Data.GetData(DataFormats.FileDrop);
            if (obj == null)
            {
                return;
            }
            //MessageBox.Show(obj.ToString());
            string[] files = obj as string[];
            if (files == null)
            {
                return;
            }
            DoCountMd5(files);
        }

        /// <summary>
        /// 拖拽文件进来时，才允许
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void Form1_DragEnter(object sender, DragEventArgs e)
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


        #region 加载配置文件对比网站md5的方法集
        
        private void btnLoadConfig_Click(object sender, EventArgs e)
        {
            var ofd = new OpenFileDialog();
            ofd.FileName = AppDomain.CurrentDomain.BaseDirectory.TrimEnd('\\');// 不去掉斜框，win2003会出错
            ofd.Filter = "Xml files (*.xml)|*.xml|All files (*.*)|*.*";

            DialogResult result = ofd.ShowDialog();
            if (result != DialogResult.OK)
            {
                return;
            }
            try
            {
                XmlDocument doc = new XmlDocument();
                doc.Load(ofd.FileName);
                var root = doc.DocumentElement;
                if(root == null)
                {
                    MessageBox.Show("配置文件不存在根结点");
                    return;
                }
                var node = root.SelectSingleNode("Service");
                if (node == null)
                {
                    MessageBox.Show("配置文件不存在Service结点");
                    return;
                }
                var urlAtt = node.SelectSingleNode("HashUrl");
                if (urlAtt == null)
                {
                    MessageBox.Show("Service结点没有HashUrl子结点");
                    return;
                }
                var localAtt = node.SelectSingleNode("LocalPath");
                if (localAtt == null)
                {
                    MessageBox.Show("Service结点没有LocalPath子结点");
                    return;
                }

                // 服务器列表
                string[] hosts;
                var hostNodes = node.SelectNodes("Host");
                if (hostNodes == null || hostNodes.Count == 0)
                {
                    hosts = new string[1];
                    hosts[0] = "";
                }
                else
                {
                    hosts = new string[hostNodes.Count];
                    for (int i = 0; i < hostNodes.Count; i++)
                    {
                        hosts[i] = hostNodes[i].InnerText.Trim();
                    }
                }
                string url = urlAtt.InnerText.Trim();
                if (url == string.Empty)
                {
                    MessageBox.Show("没有设置HashUrl的值");
                    return;
                }
                string localPath = localAtt.InnerText.Trim().TrimEnd('\\', '/');
                if (localPath == string.Empty)
                {
                    MessageBox.Show("没有设置LocalPath的值");
                    return;
                }
                if (!Directory.Exists(localPath))
                {
                    MessageBox.Show("指定的目录不存在：" + localPath);
                    return;
                } 
                var postNode = node.SelectSingleNode("PostData");
                string post = postNode == null ? null : postNode.InnerText;

                string[] ignoreDirs;
                var ignoreNodes = node.SelectNodes("IgnorePath");
                if (ignoreNodes == null || ignoreNodes.Count == 0)
                {
                    ignoreDirs = new string[1];
                    ignoreDirs[0] = ".svn";
                }
                else
                {
                    ignoreDirs = new string[ignoreNodes.Count];
                    for (int i = 0; i < ignoreNodes.Count; i++)
                    {
                        ignoreDirs[i] = ignoreNodes[i].InnerText.Trim();
                    }
                }

                #region 重建结果列
                const DataGridViewAutoSizeColumnMode mode = DataGridViewAutoSizeColumnMode.AllCells;
                var dgv = dataGridView1;
                dgv.Columns.Clear();

                DataGridViewColumn column = new DataGridViewTextBoxColumn();
                column.HeaderText = "文件名(目录:" + localPath + ")";
                column.AutoSizeMode = mode;
                dgv.Columns.Add(column);
                column = new DataGridViewTextBoxColumn();
                column.HeaderText = "本地MD5";
                column.AutoSizeMode = mode;
                dgv.Columns.Add(column);

                foreach (string host in hosts)
                {
                    column = new DataGridViewTextBoxColumn();
                    column.HeaderText = string.IsNullOrEmpty(host) ? "Web站点" : host;
                    column.AutoSizeMode = mode;
                    dgv.Columns.Add(column);
                }

                #endregion

                btnLoadConfig.Enabled = false;
                ThreadPool.UnsafeQueueUserWorkItem(state =>
                {
                    try { BeginCompare(url, post, localPath, hosts, ignoreDirs); }
                    catch (Exception ee)
                    {
                        MessageBox.Show(ee.ToString());
                    }
                    finally
                    {
                        Utility.InvokeControl(btnLoadConfig, () => btnLoadConfig.Enabled = true);
                    }
                }, null);
            }
            catch(Exception exp)
            {
                MessageBox.Show("加载配置文件出错:" + exp.Message);
            }
        }

        void BeginCompare(string url, string postData, string localPath, string[] hosts, string[] ignoreDirs)
        {
            Dictionary<string, string[]> result = new Dictionary<string, string[]>();
            // 计算本地文件MD5
            CountDirMd5ForCompare(new[] { localPath }, localPath, result, hosts.Length + 2, ignoreDirs);

            postData = (postData ?? "") + "&ignoreDir=";
            foreach (string ignoreDir in ignoreDirs)
            {
                postData += ignoreDir + "|";
            }

            int idx = 2;
            // 获取网络文件MD5
            foreach (string host in hosts)
            {
                string ret;
                try
                {
                    ret = Utility.GetPage(url, postData, host, true);
                }
                catch (Exception exp)
                {
                    MessageBox.Show(host + "出现异常:" + exp);
                    return;
                }
                if(string.IsNullOrEmpty(ret))
                {
                    MessageBox.Show("获取页面数据为空:" + host);
                    return;
                }
                string[] fileArr = ret.Split(new char[] { '\r', '\n' }, StringSplitOptions.RemoveEmptyEntries);
                int getFiles = 0;
                foreach (string fileItem in fileArr)
                {
                    string[] arrItem = fileItem.Split(',');
                    if(arrItem.Length != 2)
                    {
                        continue;
                        //MessageBox.Show("返回数据格式有误，可能aspx不支持MD5对比功能:" + url + " " + host);
                        //return;
                    }
                    getFiles++;
                    string key = arrItem[0].ToLower();
                    if (key[0] != '\\' && key[0] != '/')
                        key = '\\' + key;

                    // 检查是否需要忽略的目录
                    bool isbreak = false;
                    foreach (string ignoreDir in ignoreDirs)
                    {
                        if (key.IndexOf("\\" + ignoreDir, StringComparison.OrdinalIgnoreCase) >= 0)
                        {
                            isbreak = true;
                            break;
                        }
                    }
                    if (isbreak)
                    {
                        continue;
                    }

                    string[] val;
                    if(!result.TryGetValue(key, out val))
                    {
                        val = new string[hosts.Length + 2];
                        result.Add(key, val);
                    }
                    val[idx] = arrItem[1];
                }
                if (getFiles == 0)
                {
                    MessageBox.Show("返回文件个数为0，可能aspx不支持MD5对比功能:" + url + " " + host + "\r\n" + ret);
                    return;
                }
                idx++;
            }

            var dgv = dataGridView1;
            bool ishide = chkViewDiff.Checked;
            foreach (KeyValuePair<string, string[]> pair in result)
            {
                object[] val = pair.Value.Select(item => (object)item).ToArray();
                val[0] = pair.Key;
                Utility.InvokeControl(dgv, () =>
                {
                    bool hasDiff = false;
                    int rowIdx = dgv.Rows.Add(val);
                    for (int i = 2; i < val.Length; i++)
                    {
                        if (!Convert.ToString(val[i]).Equals(Convert.ToString(val[1]), StringComparison.OrdinalIgnoreCase))
                        {
                            dgv.Rows[rowIdx].Cells[i].Style.BackColor = Color.Salmon;
                            hasDiff = true;
                        }
                    }
                    if (!hasDiff && ishide)
                    {
                        dgv.Rows[rowIdx].Visible = false;
                    }
                });
            }
        }

        void CountDirMd5ForCompare(IEnumerable<string> dirsOrFiles, string root, Dictionary<string, string[]> result, int valLen, string[] ignoreDirs)
        {
            foreach (string file in dirsOrFiles)
            {
                if (_stop)
                {
                    return;
                }
                // 检查是否需要忽略的目录
                bool isbreak = false;
                foreach (string ignoreDir in ignoreDirs)
                {
                    if (file.IndexOf("\\" + ignoreDir, StringComparison.OrdinalIgnoreCase) >= 0)
                    {
                        isbreak = true;
                        break;
                    }
                }
                if (isbreak)
                {
                    continue;
                }
                
                if (File.Exists(file))
                {
                    string md5;
                    using (MD5CryptoServiceProvider get_md5 = new MD5CryptoServiceProvider())
                    using (FileStream get_file = new FileStream(file, FileMode.Open, FileAccess.Read, FileShare.Read))
                    {
                        md5 = BitConverter.ToString(get_md5.ComputeHash(get_file)).Replace("-", "");
                    }
                    string file1 = file.Replace(root, "");
                    string[] val = new string[valLen];
                    val[1] = md5;
                    result.Add(file1.ToLower(), val);
                }
                else if (Directory.Exists(file))
                {
                    CountDirMd5ForCompare(Directory.GetFiles(file), root, result, valLen, ignoreDirs);
                    CountDirMd5ForCompare(Directory.GetDirectories(file), root, result, valLen, ignoreDirs);
                }
            }
        }

        #endregion

        /// <summary>
        /// 是否只显示差异项
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void chkViewDiff_CheckedChanged(object sender, EventArgs e)
        {
            var dgv = dataGridView1;
            var colCnt = dgv.Columns.Count;
            if (colCnt >= 3)
            {
                ThreadPool.UnsafeQueueUserWorkItem(DoHide, dgv);
            }
        }

        private void DoHide(object obj)
        {
            DataGridView dgv = (DataGridView) obj;

            bool ishide = chkViewDiff.Checked;
            List<int> arrShow = new List<int>();
            foreach (DataGridViewRow eachRow in dgv.Rows)
            {
                DataGridViewRow row = eachRow;
                if (!ishide)
                {
                    arrShow.Add(row.Index);
                    continue;
                }

                bool hasDiff = false;
                for (int i = 2, colCnt = dgv.Columns.Count; i < colCnt; i++)
                {
                    string localMd5 = Convert.ToString(row.Cells[1].Value);
                    string remoteMd5 = Convert.ToString(row.Cells[i].Value);
                    if (!localMd5.Equals(remoteMd5, StringComparison.OrdinalIgnoreCase))
                    {
                        hasDiff = true;
                        break;
                    }
                }
                if (hasDiff)
                {
                    arrShow.Add(row.Index);
                }
            }
            Utility.InvokeControl(dgv, () =>
            {
                foreach (DataGridViewRow eachRow in dgv.Rows)
                {
                    bool isShow;
                    if (arrShow.Count > 0 && eachRow.Index == arrShow[0])
                    {
                        isShow = true;
                        arrShow.RemoveAt(0);
                    }
                    else
                    {
                        isShow = false;
                    }
                    if (eachRow != null && eachRow.Visible != isShow)
                    {
                        eachRow.Visible = isShow;
                    }
                }
            });
        }

    }
}
