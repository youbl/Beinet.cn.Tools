using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace Beinet.cn.Tools.Others
{
    public partial class svntool : Form
    {
        public svntool()
        {
            InitializeComponent();
        }

        private void btnSvnDir_Click(object sender, EventArgs e)
        {
            FolderBrowserDialog fbd = new FolderBrowserDialog();
            string dir = txtSvnDir.Text.Trim();
            if (dir != string.Empty)
            {
                fbd.SelectedPath = dir;
            }
            if (fbd.ShowDialog(this) != DialogResult.OK)
                return;

            txtSvnDir.Text = fbd.SelectedPath;
        }

        private void btnSvnup_Click(object sender, EventArgs e)
        {
            string dir = txtSvnDir.Text.Trim();
            if (!Directory.Exists(dir))
            {
                MessageBox.Show("指定目录不存在");
                return;
            }

            string cmd = "up ";
            string ver = txtSvnver.Text.Trim();
            int tmp;
            if (ver != string.Empty && int.TryParse(ver, out tmp) && tmp > 0)
            {
                cmd += " -r " + ver + " ";
            }
            cmd += dir;

            txtRet.Text = Utility.ExecCmd("svn", cmd);
        }
    }
}
