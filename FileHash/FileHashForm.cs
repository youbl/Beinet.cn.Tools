using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Diagnostics;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;
using Beinet.cn.Tools.Util;

namespace Beinet.cn.Tools.FileHash
{
    public partial class FileHashForm : Form
    {
        public FileHashForm()
        {
            InitializeComponent();
        }

        private void BtnSelectFiles_Click(object sender, EventArgs e)
        {
            var dialog = new FolderBrowserDialog();
            dialog.RootFolder = Environment.SpecialFolder.MyComputer;
            if (dialog.ShowDialog() != DialogResult.OK)
                return;

            var countDir = dialog.SelectedPath;
            labStatus.Text = countDir;

            if (chkToFile.Checked)
            {
                var sfd = new SaveFileDialog();
                var result = sfd.ShowDialog(this);
                if (result != DialogResult.OK)
                {
                    return;
                }

                HashUtil.HashAndSaveToFile(countDir, chkSha1.Checked, chkAllSubDir.Checked, sfd.FileName);
                Process.Start("explorer.exe", "/select," + sfd.FileName);
                return;
            }

            var option = chkAllSubDir.Checked ? SearchOption.AllDirectories : SearchOption.TopDirectoryOnly;
            var files = Directory.GetFiles(countDir, "*", option);
            DoCount(files);
        }


        private void FileHashForm_DragEnter(object sender, DragEventArgs e)
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

        private void FileHashForm_DragDrop(object sender, DragEventArgs e)
        {
            var obj = e.Data.GetData(DataFormats.FileDrop);
            if (obj == null)
            {
                return;
            }

            string[] fileOrDirs = obj as string[];
            if (fileOrDirs == null || fileOrDirs.Length < 1)
            {
                return;
            }

            var files = GetFiles(fileOrDirs);
            labStatus.Text = files[0] + "...";
            DoCount(files);
        }



        void DoCount(IEnumerable<string> files)
        {
            var countSha1 = chkSha1.Checked;
            if (!int.TryParse(txtThreadNum.Text.Trim(), out var threadNum) || threadNum <= 0)
            {
                MessageBox.Show("请正确输入线程数");
                return;
            }

            AddCol();

            ThreadPool.UnsafeQueueUserWorkItem(tmp =>
            {
                var md5Datas = new List<string[]>();
                var thread = new MultiThread(state =>
                {
                    var file = state.ToString();
                    var result = HashUtil.CountMD5(file, countSha1);

                    var row = new string[] {file, result[0], result[1], result[2]};
                    List<string[]> showDatas = null;
                    lock (md5Datas)
                    {
                        md5Datas.Add(row);
                        // 每50个刷新一次列表
                        if (md5Datas.Count > 20)
                        {
                            showDatas = md5Datas;
                            md5Datas = new List<string[]>();
                        }
                    }

                    if (showDatas != null)
                    {
                        Utility.BindToDataGrid(dgvFiles, showDatas);
                    }
                }, threadNum);

                thread.Start();

                foreach (var file in files)
                {
                    thread.Add(file);
                }

                while (!thread.Completed)
                {
                    Thread.Sleep(1000);
                }

                var now = DateTime.Now.ToString("HH:mm:ss");
                var costTime = (thread.EndTime - thread.StartTime).TotalSeconds.ToString("N0");
                var msg = $"-{now} 完成耗时:{costTime}秒，共{thread.ProcessCount.ToString("N0")}个文件";
                Utility.InvokeControl(labStatus, () => labStatus.Text += msg);

                Utility.BindToDataGrid(dgvFiles, md5Datas);

            }, null);
        }

        List<string> GetFiles(string[] files)
        {
            var option = chkAllSubDir.Checked ? SearchOption.AllDirectories : SearchOption.TopDirectoryOnly;

            List<string> ret = new List<string>();
            foreach (var item in files)
            {
                if (File.Exists(item))
                    ret.Add(item);
                else if (Directory.Exists(item))
                    ret.AddRange(Directory.GetFiles(item, "*", option));
            }

            return ret;
        }


        void AddCol()
        {
            var dgv = dgvFiles;
            if (dgv.Columns.Count > 0)
                return;

            DataGridViewColumn column;
            column = new DataGridViewTextBoxColumn();
            column.HeaderText = "文件名";
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

            column = new DataGridViewTextBoxColumn();
            column.HeaderText = "文件大小";
            column.AutoSizeMode = DataGridViewAutoSizeColumnMode.DisplayedCells;
            dgv.Columns.Add(column);

            //dgv.AutoSize = true;
        }

        private void btnClear_Click(object sender, EventArgs e)
        {
            dgvFiles.Rows.Clear();
        }

        // 导出列表
        private void button1_Click(object sender, EventArgs e)
        {
            if (dgvFiles.Rows.Count <= 0)
            {
                MessageBox.Show("列表为空");
                return;
            }

            var ofd = new SaveFileDialog();
            var result = ofd.ShowDialog(this);
            if (result != DialogResult.OK)
            {
                return;
            }

            using (var sw = new StreamWriter(ofd.FileName, false, Encoding.UTF8))
            {
                foreach (DataGridViewRow row in dgvFiles.Rows)
                {
                    foreach (DataGridViewCell cell in row.Cells)
                    {
                        sw.Write(cell.Value + ",");
                    }

                    sw.WriteLine();
                }
            }

            Process.Start("explorer.exe", "/select," + ofd.FileName);
        }

        private void chkShowSame_CheckedChanged(object sender, EventArgs e)
        {
            if (!chkShowSame.Checked || dgvFiles.Rows.Count <= 0)
            {
                return;
            }

            var collect = new Dictionary<string, SameFileHash>();
            var idx = 0;
            foreach (DataGridViewRow row in dgvFiles.Rows)
            {
                // MD5+大小，作为唯一值判断
                var key = row.Cells[1].Value + "|" + row.Cells[3].Value;
                if (!collect.TryGetValue(key, out var num))
                {
                    num = new SameFileHash();
                    collect.Add(key, num);
                }

                num.num++;
                num.Rownum.Add(idx);
                idx++;
            }

            // 不重复的行号倒序排列，然后移除
            var removeIdxs = collect.Where(item => item.Value.num == 1).Select(item => item.Value.Rownum[0])
                .OrderByDescending(item => item);
            foreach (var rowIdx in removeIdxs)
            {
                dgvFiles.Rows.RemoveAt(rowIdx);
            }

            // 按MD5进行排序
            dgvFiles.Sort(dgvFiles.Columns[1], ListSortDirection.Ascending);
        }

        class SameFileHash
        {
            /// <summary>
            /// key相同数
            /// </summary>
            public int num { get; set; }

            /// <summary>
            /// 行号
            /// </summary>
            public List<int> Rownum { get; set; } = new List<int>();
        }

        private void BtnShowResult_Click(object sender, EventArgs e)
        {
            var ofd = new OpenFileDialog();
            ofd.Multiselect = true;
            var result = ofd.ShowDialog(this);
            if (result != DialogResult.OK)
            {
                return;
            }
            AddCol();

            ThreadPool.UnsafeQueueUserWorkItem(state =>
            {
                try
                {
                    // 相同的md5及行号列表
                    var allMd5s = new List<string[]>();
                    var collect = new Dictionary<string, SameFileHash>();
                    string[] files = (string[]) state;
                    var idx = 0;
                    foreach (var file in files)
                    {
                        using (var sr = new StreamReader(file, Encoding.UTF8))
                        {
                            while (!sr.EndOfStream)
                            {
                                var line = sr.ReadLine();
                                var arrLine = SplitLine(line);
                                if (arrLine == null)
                                    continue;

                                allMd5s.Add(arrLine);

                                var key = arrLine[1] + "|" + arrLine[3];
                                if (!collect.TryGetValue(key, out var num))
                                {
                                    num = new SameFileHash();
                                    collect.Add(key, num);
                                }

                                num.num++;
                                num.Rownum.Add(idx);
                                idx++;
                            }
                        }
                    }

                    // 不重复的行号倒序排列，然后移除
                    var removeIdxs = collect.Where(item => item.Value.num == 1).Select(item => item.Value.Rownum[0])
                        .OrderByDescending(item => item);
                    foreach (var rowIdx in removeIdxs)
                    {
                        allMd5s.RemoveAt(rowIdx);
                    }

                    if (allMd5s.Count <= 0)
                    {
                        MessageBox.Show("未找到重复项");
                        return;
                    }

                    // 按MD5进行排序
                    var sortArr = allMd5s.OrderBy(item => item[1]).ThenBy(item => item[0]);
                    Utility.BindToDataGrid(dgvFiles, sortArr);

                    // MessageBox.Show("任务完成");
                    labStatus.Text = $"找到{allMd5s.Count}条重复记录";
                }
                catch (Exception exp)
                {
                    MessageBox.Show(exp.ToString());
                }

            }, ofd.FileNames);
        }

        string[] SplitLine(string line)
        {
            if (line == null || (line = line.Trim()).Length <= 0)
                return null;
            var arrIdx = new List<int>();
            var idx = 0;
            do
            {
                idx = line.IndexOf(',', idx + 1);
                if (idx <= 0)
                    break;
                arrIdx.Add(idx);
            } while (true);

            var len = arrIdx.Count;
            if (len < 3)
                return null;

            var ret = new string[4];
            ret[3] = line.Substring(arrIdx[len - 1] + 1);

            ret[2] = line.Substring(arrIdx[len - 2] + 1, arrIdx[len - 1] - arrIdx[len - 2] - 1);
            ret[1] = line.Substring(arrIdx[len - 3] + 1, arrIdx[len - 2] - arrIdx[len - 3] - 1);
            ret[0] = line.Substring(0, arrIdx[len - 3]);
            return ret;
        }
    }
}
