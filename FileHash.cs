using System;
using System.Collections.Generic;
using System.IO;
using System.Security.Cryptography;
using System.Text;
using System.Threading;
using System.Windows.Forms;

namespace Beinet.cn.Tools
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
            btnSelectFile.Enabled = false;
            btnStop.Enabled = true;
            _stop = false;
            txtRet.Text = string.Empty;
            ThreadPool.UnsafeQueueUserWorkItem(CountMd5, ofd.FileNames);
        }

        private void btnClear_Click(object sender, EventArgs e)
        {
            txtRet.Text = string.Empty;
        }

        private void btnCopy_Click(object sender, EventArgs e)
        {
            Clipboard.SetText(txtRet.Text);
            MessageBox.Show("已复制到剪贴板");
        }

        private void btnSave_Click(object sender, EventArgs e)
        {
            var ofd = new SaveFileDialog();
            if (ofd.ShowDialog(this) != DialogResult.OK)
                return;
            using(var sw = new StreamWriter(ofd.FileName, false, Encoding.UTF8))
            {
                sw.Write(txtRet.Text);
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

                CountDirMd5(files);
            }
            catch(Exception exp)
            {
                MessageBox.Show("出错了：\r\n" + exp.Message);
            }
            finally
            {
                btnSelectFile.Enabled = true;
                btnStop.Enabled = false;
            }
        }


        int CountDirMd5(IEnumerable<string> dirsOrFiles)
        {
            int cnt = 0;
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
                    md5 = txtRet.Text + "\r\n\r\n文件名:" + file + "\r\nmd5:" + md5;
                    Utility.SetValue(txtRet, "Text", md5);
                    cnt++;
                    Utility.SetValue(labRet, "Text", "处理了" + cnt.ToString() + "个文件");
                }
                else if (Directory.Exists(file))
                {
                    cnt += CountDirMd5(Directory.GetFiles(file));
                    cnt += CountDirMd5(Directory.GetDirectories(file));
                }
            }
            return cnt;
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
            ThreadPool.UnsafeQueueUserWorkItem(CountMd5, files);
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

    }
}
