using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.Configuration;
using System.Data;
using System.Diagnostics;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading;
using System.Windows.Forms;

namespace Beinet.cn.Tools.Gitlab
{
    public partial class GitlabForm : Form
    {
        private List<GitlabHelper.GitProject> allProjects = new List<GitlabHelper.GitProject>();

        public GitlabForm()
        {
            InitializeComponent();

            lvProjects.FullRowSelect = true;
            txtGitlabUrl.Text = ConfigurationManager.AppSettings["GitlabUrl"] ?? "";
            txtPrivateToken.Text = ConfigurationManager.AppSettings["GitlabPrivateToken"] ?? "";
        }

        private void btnShowGitlab_Click(object sender, EventArgs e)
        {
            var url = txtGitlabUrl.Text.Trim();
            if (url.Length == 0)
            {
                MessageBox.Show("请输入Gitlab地址");
                return;
            }

            if (url.IndexOf("http://", StringComparison.OrdinalIgnoreCase) != 0 &&
                url.IndexOf("https://", StringComparison.OrdinalIgnoreCase) != 0)
            {
                MessageBox.Show("Gitlab地址必须以http开头");
                return;
            }

            if (url[url.Length - 1] != '/')
            {
                url = url + '/';
                txtGitlabUrl.Text = url;
            }

            var token = txtPrivateToken.Text.Trim();
            if (token.Length == 0)
            {
                MessageBox.Show("请输入Gitlab的访问令牌");
                return;
            }

            btnShowGitlab.Enabled = false;
            btnShowGitlab.Text = "加载中..";
            allProjects.Clear();

            lvProjects.Items.Clear();
            ThreadPool.UnsafeQueueUserWorkItem(state =>
            {
                var begin = DateTime.Now;
                var isErr = false;
                try
                {
                    var helper = new GitlabHelper(url, token);
                    BindListView(lvProjects, helper);
                }
                catch (Exception exp)
                {
                    MessageBox.Show(exp.Message);
                    isErr = true;
                }

                Utility.InvokeControl(btnShowGitlab, () =>
                {
                    btnShowGitlab.Enabled = true;
                    btnShowGitlab.Text = "显示所有项目";
                    if (!isErr)
                    {
                        btnListSearch.Enabled = true;
                        btnGitClone.Enabled = true;
                    }

                    labProjectNum.Text = labProjectNum.Text + " 耗时:" +
                                         (DateTime.Now - begin).TotalSeconds.ToString("N2") + "秒";
                });
            }, null);
        }

        void BindListView(ListView lv, GitlabHelper helper)
        {
            var page = 1;
            var pageSize = 100;
            List<GitlabHelper.GitProject> result;
            do
            {
                result = helper.GetProjects(page, pageSize);
                page++;

                if (result != null && result.Count > 0)
                {
                    // 添加到统一的列表，便于检索
                    allProjects.AddRange(result);

                    AppendProjects(result, lv);
                }
            } while (result != null && result.Count >= pageSize);
        }

        private void lvProjects_MouseDoubleClick(object sender, MouseEventArgs e)
        {
            // 双击复制
            var lv = (ListView) sender;
            var row = lv.GetItemAt(e.X, e.Y);
            var cell = row.GetSubItemAt(e.X, e.Y);
            if (cell == null || string.IsNullOrEmpty(cell.Text))
                return;

            string strText = cell.Text;
            try
            {
                Clipboard.SetDataObject(strText);
                string info = $"已复制: {strText}";
                MessageBox.Show(info);
            }
            catch (System.Exception ex)
            {
                MessageBox.Show(ex.Message);
            }

        }

        private void lvProjects_ColumnClick(object sender, ColumnClickEventArgs e)
        {
            // 点标题栏排序
            var up = "🡹";
            var down = "🡻"; // 🡹🡻🡱🡳🠉🠟🠇🠋🠝🡅🡇

            var lv = lvProjects; //(ListView)sender;
            var colIdx = e?.Column ?? 1;

            var sorter = lv.ListViewItemSorter as ListViewItemComparer;
            if (sorter == null)
            {
                sorter = new ListViewItemComparer(colIdx);
                lv.ListViewItemSorter = new ListViewItemComparer(colIdx);
            }
            else if (e != null)
            {
                lv.Columns[sorter.Column].Text = lv.Columns[sorter.Column].Text.Replace(up, "").Replace(down, "");

                if (sorter.Column == colIdx)
                {
                    sorter.IsAsc = !sorter.IsAsc;
                }
                else
                {
                    sorter.Column = colIdx;
                    sorter.IsAsc = true;
                }
            }

            lv.Sort();
            if (e != null)
                lv.Columns[colIdx].Text = lv.Columns[colIdx].Text + (sorter.IsAsc ? up : down);
        }

        class ListViewItemComparer : IComparer
        {
            /// <summary>
            /// 排序列
            /// </summary>
            public int Column { get; set; }

            /// <summary>
            /// 是否升序
            /// </summary>
            public bool IsAsc { get; set; } = true;

            public ListViewItemComparer(int column = 0)
            {
                Column = column;
            }

            public int Compare(object x, object y)
            {
                if (x == null || y == null)
                    return 0;
                var xTxt = ((ListViewItem) x).SubItems[Column].Text;
                var yTxt = ((ListViewItem) y).SubItems[Column].Text;

                int ret;
                if (Column == 0)
                {
                    ret = (int.Parse(xTxt) - int.Parse(yTxt));
                }
                else
                {
                    ret = String.Compare(xTxt, yTxt, StringComparison.OrdinalIgnoreCase);
                }

                return IsAsc ? ret : -ret;
            }
        }

        private void btnSelectGitDir_Click(object sender, EventArgs e)
        {
            var dialog = new FolderBrowserDialog();
            dialog.RootFolder =
                Environment.SpecialFolder
                    .MyComputer; // (string.IsNullOrEmpty(txtGitDir.Text) ? "C:\\" : txtGitDir.Text);
            if (dialog.ShowDialog() != DialogResult.OK)
                return;

            txtGitDir.Text = dialog.SelectedPath;
        }


        private void btnGitClone_Click(object sender, EventArgs e)
        {
            if (lvProjects.Items.Count <= 0)
            {
                MessageBox.Show("请先点击：显示所有项目");
                return;
            }

//            // 调试语句
//            while (lvProjects.Items.Count > 15)
//                lvProjects.Items.RemoveAt(1);

            var gitDir = txtGitDir.Text.Trim();
            if (gitDir.Length == 0)
            {
                MessageBox.Show("请选择目录。");
                return;
            }

            // 创建Git根目录
            if (!Directory.Exists(gitDir))
            {
                var dialogResult = MessageBox.Show("目录不存在，是否要创建？", "新建目录", MessageBoxButtons.OKCancel);
                if (dialogResult != DialogResult.OK)
                    return;

                if (!CreateDir(gitDir))
                    return;
            }

            var ignoreExistsProj = chkIgnoreExistsProj.Checked;

            var newProjNum = 0;
            var batCloneFile = Path.Combine(Utility.Dir, "gitClone.bat");
            var batUpdateFile = Path.Combine(gitDir, "gitUpdate.bat");
            using (var swClone = new StreamWriter(batCloneFile, false, Encoding.Default))
            using (var swUpdate = new StreamWriter(batUpdateFile, true, Encoding.Default))
            {
                foreach (ListViewItem listViewItem in lvProjects.Items)
                {
                    var itemUrl = listViewItem.SubItems[2].Text;

                    var itemDir = itemUrl.Replace(txtGitlabUrl.Text, "").Replace(".git", "");
                    itemDir = itemDir.Replace("/", "@");
                    itemDir = Path.Combine(gitDir,
                        itemDir); // listViewItem.SubItems[1].Text + "_" + listViewItem.SubItems[0].Text);

                    var isProjExists = Directory.Exists(itemDir);
                    if (isProjExists)
                    {
                        if (!ignoreExistsProj)
                        {
                            MessageBox.Show("子目录已存在，请重新指定: " + itemDir);
                            return;
                        }
                    }
                    else if (!CreateDir(itemDir))
                    {
                        return;
                    }

                    if (!isProjExists)
                    {
                        newProjNum++;
                        swClone.WriteLine("git.exe clone \"{0}\" \"{1}\"", itemUrl, itemDir);
                        swUpdate.WriteLine("cd \"{0}\" && git.exe pull", itemDir);
                    }
                }

                if (newProjNum > 0)
                {
                    swClone.WriteLine("@echo. ");
                    swClone.WriteLine("@echo -----------------------------------------------");
                    swClone.WriteLine("@echo 克隆完成，请定期执行更新脚本：" + batUpdateFile);
                    swClone.WriteLine("@echo -----------------------------------------------");
                    swClone.WriteLine("pause");
                    // swUpdate.WriteLine("pause");
                }
            }

            //            var cmd = File.ReadAllLines(batFile);
            if (newProjNum > 0)
                TestCmd(batCloneFile);
            else
                MessageBox.Show("未找到新项目，克隆操作未执行");
        }

        void TestCmd(string batFile)
        {
            var p = new Process();
            {
                p.StartInfo.FileName = @"cmd.exe";
                p.StartInfo.Arguments = "/C \"" + batFile + "\"";
                p.StartInfo.UseShellExecute = true;
                p.StartInfo.CreateNoWindow = false; //创建窗口

                p.Start();
            }
        }

        static bool CreateDir(string dir)
        {
            try
            {
                Directory.CreateDirectory(dir);
                return true;
            }
            catch (Exception exp)
            {
                MessageBox.Show("创建目录失败: " + dir + "：" + exp.Message);
                return false;
            }
        }

        private void btnListSearch_Click(object sender, EventArgs e)
        {
            if (!btnListSearch.Enabled || lvProjects.Items.Count <= 0)
            {
                return;
            }

            var keyword = txtKeyword.Text.Trim();
            List<GitlabHelper.GitProject> showProjs;
            if (keyword.Length <= 0)
                showProjs = allProjects;
            else
            {
                showProjs = new List<GitlabHelper.GitProject>();
                foreach (var item in allProjects)
                {
                    if (keyword == item.Id.ToString() ||
                        item.Name.IndexOf(keyword, StringComparison.OrdinalIgnoreCase) >= 0 ||
                        item.Desc.IndexOf(keyword, StringComparison.OrdinalIgnoreCase) >= 0 ||
                        item.Url.IndexOf(keyword, StringComparison.OrdinalIgnoreCase) >= 0)
                        showProjs.Add(item);
                }
            }

            lvProjects.Items.Clear();
            AppendProjects(showProjs, lvProjects);
            lvProjects_ColumnClick(null, null);
        }

        private void AppendProjects(List<GitlabHelper.GitProject> showProjs, ListView lv)
        {
            var rows = new ListViewItem[showProjs.Count];
            var rowIdx = 0;
            foreach (var gitProject in showProjs)
            {
                var lvItem = new ListViewItem(new string[]
                {
                    gitProject.Id.ToString(),
                    gitProject.Name,
                    gitProject.Url,
                    gitProject.Desc
                });
                rows[rowIdx] = (lvItem);
                rowIdx++;
            }

            Utility.InvokeControl(lv, () =>
            {
                lv.Items.AddRange(rows.ToArray());
                labProjectNum.Text = lv.Items.Count.ToString();
            });
        }

        private void txtKeyword_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Enter)
                btnListSearch_Click(null, null);
        }
    }
}