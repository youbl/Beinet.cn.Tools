using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace Beinet.cn.Tools.Gitlab
{
    public partial class GitlabForm : Form
    {
        public GitlabForm()
        {
            InitializeComponent();

            lvProjects.FullRowSelect = true;
        }

        private void btnShowGitlab_Click(object sender, EventArgs e)
        {
            var helper = new GitlabHelper(txtGitlabUrl.Text.Trim(), txtPrivateToken.Text.Trim());
            var result = helper.GetAllProjects();
            lvProjects.Items.Clear();

            if (result != null)
            {
                foreach (var gitProject in result.OrderBy(item => item.Name))
                {
                    var lvItem = new string[]
                    {
                        gitProject.Id.ToString(),
                        gitProject.Name,
                        gitProject.Url,
                        gitProject.Desc
                    };
                    lvProjects.Items.Add(new ListViewItem(lvItem));
                }
            }
        }

        private void lvProjects_MouseDoubleClick(object sender, MouseEventArgs e)
        {
            // 双击复制
            ListView listview = (ListView)sender;
            ListViewItem lstrow = listview.GetItemAt(e.X, e.Y);
            System.Windows.Forms.ListViewItem.ListViewSubItem lstcol = lstrow.GetSubItemAt(e.X, e.Y);
            if (lstcol == null || string.IsNullOrEmpty(lstcol.Text))
                return;

            string strText = lstcol.Text;
            try
            {
                Clipboard.SetDataObject(strText);
                string info = string.Format("内容【{0}】已复制", strText);
                MessageBox.Show(info);
            }
            catch (System.Exception ex)
            {
                MessageBox.Show(ex.Message, "提示", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }

        }
    }
}
