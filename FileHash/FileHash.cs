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

        public FileHash()
        {
            InitializeComponent();
        }

        private void btnSelectFile_Click(object sender, EventArgs e)
        {
            OpenFileDialog ofd = new OpenFileDialog();
            ofd.Multiselect = true;
            if (ofd.ShowDialog(this) != DialogResult.OK)
                return;

            DoCountMd5(ofd.FileNames);
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
                MessageBox.Show("处理的文件和目录个数：" + cnt.ToString());
            }
            catch(Exception exp)
            {
                MessageBox.Show("出错了：\r\n" + exp.Message);
            }
            finally
            {
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
            var dgv = dataGridView1;
            foreach (string file in dirsOrFiles)
            {
                if (_stop)
                    return cnt;

                if (File.Exists(file))
                {
                    string md5;
                    using (MD5CryptoServiceProvider get_md5 = new MD5CryptoServiceProvider())
                    using (FileStream get_file = new FileStream(file, FileMode.Open, FileAccess.Read, FileShare.Read))
                    {
                        md5 = BitConverter.ToString(get_md5.ComputeHash(get_file)).Replace("-", "");
                    }
                    string file1 = file.Replace(root, "");
                    Utility.InvokeControl(dgv, () => dgv.Rows.Add(file1, md5));
                    cnt++;
                }
                else if (Directory.Exists(file))
                {
                    cnt += CountDirMd5(Directory.GetFiles(file), root);
                    cnt += CountDirMd5(Directory.GetDirectories(file), root);
                }
            }
            return cnt;
        }

        void AddCol(string dir)
        {
            var dgv = dataGridView1;
            DataGridViewColumn column = new DataGridViewTextBoxColumn();
            column.HeaderText = "文件名(目录:" + dir + ")";
            column.AutoSizeMode = DataGridViewAutoSizeColumnMode.DisplayedCells;
            dgv.Columns.Add(column); 
            column = new DataGridViewTextBoxColumn();
            column.HeaderText = "本地MD5";
            column.AutoSizeMode = DataGridViewAutoSizeColumnMode.DisplayedCells;
            dgv.Columns.Add(column);
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

        void DoCountMd5(string[] dirsOrFiles)
        {
            btnSelectFile.Enabled = false;
            btnStop.Enabled = true;
            _stop = false;

            btnClear_Click(null, null);

            string path = Path.GetDirectoryName(dirsOrFiles.First());
            if (dirsOrFiles.Length == 1 && Directory.Exists(dirsOrFiles[0]))
            {
                path = dirsOrFiles[0];
            }
            else
            {
                path = Path.GetDirectoryName(dirsOrFiles[0]);
            }
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
                var hostNodes = node.SelectNodes("Host");
                if (hostNodes == null || hostNodes.Count == 0)
                {
                    MessageBox.Show("Service结点没有Host子结点");
                    return;
                }
                var postNode = node.SelectSingleNode("PostData");
                string url = urlAtt.InnerText;
                string localPath = localAtt.InnerText;
                string post = postNode == null ? null : postNode.InnerText;
                string[] hosts = new string[hostNodes.Count];
                for (int i = 0; i < hostNodes.Count; i++)
                {
                    hosts[i] = hostNodes[i].InnerText;
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
                    column.HeaderText = host;
                    column.AutoSizeMode = mode;
                    dgv.Columns.Add(column);
                }

                #endregion

                btnLoadConfig.Enabled = false;
                ThreadPool.UnsafeQueueUserWorkItem(state =>
                {
                    try { BeginCompare(url, post, localPath, hosts); }
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

        void BeginCompare(string url, string postData, string localPath, string[] hosts)
        {
            Dictionary<string, string[]> result = new Dictionary<string, string[]>();
            // 计算本地文件MD5
            CountDirMd5ForCompare(new string[] { localPath }, localPath, result, hosts.Length + 2);

            int idx = 2;
            // 获取网络文件MD5
            foreach (string host in hosts)
            {
                string ret = Utility.GetPage(url, postData, host);
                if(string.IsNullOrEmpty(ret))
                {
                    MessageBox.Show("获取页面数据为空:" + host);
                    return;
                }
                string[] fileArr = ret.Split(new char[] { '\r', '\n' }, StringSplitOptions.RemoveEmptyEntries);
                foreach (string fileItem in fileArr)
                {
                    string[] arrItem = fileItem.Split(',');
                    if(arrItem.Length != 2)
                    {
                        MessageBox.Show("返回数据格式有误，可能aspx不支持MD5对比功能:" + url + " " + host);
                        return;
                    }
                    string key = arrItem[0].ToLower();
                    string[] val;
                    if(!result.TryGetValue(key, out val))
                    {
                        val = new string[hosts.Length + 2];
                        result.Add(key, val);
                    }
                    val[idx] = arrItem[1];
                }
                idx++;
            }

            var dgv = dataGridView1;
            foreach (KeyValuePair<string, string[]> pair in result)
            {
                object[] val = pair.Value.Select(item => (object)item).ToArray();
                val[0] = pair.Key;
                Utility.InvokeControl(dgv, () =>
                                               {
                                                   int rowIdx = dgv.Rows.Add(val);
                                                   for (int i = 2; i < val.Length; i++)
                                                   {
                                                       if (Convert.ToString(val[i]) != Convert.ToString(val[1]))
                                                       {
                                                           dgv.Rows[rowIdx].Cells[i].Style.BackColor = Color.Salmon;
                                                       }
                                                   }
                                               });
            }
        }

        void CountDirMd5ForCompare(IEnumerable<string> dirsOrFiles, string root, Dictionary<string, string[]> result, int valLen)
        {
            foreach (string file in dirsOrFiles)
            {
                if (file.IndexOf(".svn", StringComparison.OrdinalIgnoreCase) >= 0)
                    continue;
                if (_stop)
                    return;

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
                    CountDirMd5ForCompare(Directory.GetFiles(file), root, result, valLen);
                    CountDirMd5ForCompare(Directory.GetDirectories(file), root, result, valLen);
                }
            }
        }

        #endregion
    }
}
