using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

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

            txtGitDir.Text = dialog.SelectedPath;
        }


    }
}
