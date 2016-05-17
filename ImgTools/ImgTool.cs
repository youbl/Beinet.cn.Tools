using System;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.Text.RegularExpressions;
using System.Threading;
using System.Windows.Forms;

namespace Beinet.cn.Tools.ImgTools
{
    public partial class ImgTool : Form
    {
        public ImgTool()
        {
            InitializeComponent();

            InitCols();
        }



        private void btnSelectFile_Click(object sender, EventArgs e)
        {
            FolderBrowserDialog fbd = new FolderBrowserDialog();
            if (fbd.ShowDialog(this) != DialogResult.OK)
                return;

            OperationFiles(Directory.GetFiles(fbd.SelectedPath));
        }

        private void btnClear_Click(object sender, EventArgs e)
        {
            //dataGridView1.Columns.Clear();
            dataGridView1.Rows.Clear();
        }


        private void btnPreview_Click(object sender, EventArgs e)
        {
            var dgv = dataGridView1;
            if (dgv.Rows.Count <= 0)
            {
                MessageBox.Show("列表还没文件，搞清楚啊");
                return;
            }
            string rule = textBox1.Text.Trim();
            if (rule.Length == 0)
            {
                MessageBox.Show("规则还没写呢，搞清楚啊");
                return;
            }

            int idx = 1;
            Regex reg = new Regex(@"\[o(\d+)-(\d+)\]");
            foreach (DataGridViewRow row in dgv.Rows)
            {
                //重命名说明:[o]为原名，[o2-5]为原名第2~5个字符，[d]为1开始的序号，[p]为拍照日期
                string newname = rule;
                string oldname = Convert.ToString(row.Cells[1].Value);
                string phototime = Regex.Replace(Convert.ToString(row.Cells[6].Value), @"[_\-\\\/\s:]", "");

                newname = newname.Replace("[o]", oldname);
                newname = newname.Replace("[d]", idx.ToString());
                newname = newname.Replace("[p]", phototime);
                newname = newname.Replace("[e]", Path.GetExtension(oldname));

                // 取原名某几个字符的代码
                Match match = reg.Match(newname);
                while (match.Success)
                {
                    int start = int.Parse(match.Groups[1].Value);
                    if (start <= 0)
                    {
                        start = 1;
                    }
                    int end = int.Parse(match.Groups[2].Value);
                    if (end <= start)
                    {
                        MessageBox.Show("结束值要大于开始值啊：" + match.Value);
                        return;
                    }
                    string cutstr;
                    if (start > oldname.Length)
                    {
                        cutstr = "";
                    }
                    else
                    {
                        if (end >= oldname.Length)
                        {
                            end = oldname.Length;
                        }
                        cutstr = oldname.Substring(start - 1, end - start + 1);
                    }
                    newname = reg.Replace(newname, cutstr);
                    match = match.NextMatch();
                }

                row.Cells[9].Value = newname;
                idx++;
            }
            btnSave.Enabled = true;
        }

        private void btnSave_Click(object sender, EventArgs e)
        {
            var dgv = dataGridView1;
            if (dgv.Rows.Count <= 0)
            {
                MessageBox.Show("列表还没文件，搞清楚啊");
                return;
            }

            var result = MessageBox.Show("此操作不可逆，你确认要重命名？", "警告", MessageBoxButtons.OKCancel, MessageBoxIcon.Warning,
                MessageBoxDefaultButton.Button2);
            if (result != DialogResult.OK)
            {
                return;
            }

            int num = 0, fail = 0;
            foreach (DataGridViewRow row in dgv.Rows)
            {
                string newname = Convert.ToString(row.Cells[9].Value).Trim();
                if (string.IsNullOrEmpty(newname))
                {
                    row.DefaultCellStyle.BackColor = Color.Coral;
                    row.Cells[10].Value = "新文件名不能为空";
                    fail++;
                    continue;
                }
                string dir = Convert.ToString(row.Cells[2].Value).Trim();
                newname = Path.Combine(dir, newname);
                if (File.Exists(newname))
                {
                    row.DefaultCellStyle.BackColor = Color.Coral;
                    row.Cells[10].Value = "新文件名已经存在";
                    fail++;
                    continue;
                }
                string oldname = Convert.ToString(row.Cells[1].Value).Trim();
                oldname = Path.Combine(dir, oldname);
                if (!File.Exists(oldname))
                {
                    row.DefaultCellStyle.BackColor = Color.Coral;
                    row.Cells[10].Value = "旧文件名不存在，可能已删除？";
                    fail++;
                    continue;
                }
                File.Move(oldname, newname);
                num++;
            }
            string msg = "重命名成功文件个数：" + num.ToString();
            if (fail > 0)
            {
                msg += "，失败个数：" + fail.ToString();
            }
            MessageBox.Show(msg);
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
            OperationFiles(files);
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


        private void OperationFiles(string[] files)
        {
            btnSave.Enabled = false;

            if (files.Length == 1 && Directory.Exists(files[0]))
            {
                files = Directory.GetFiles(files[0]);
            }
            ThreadPool.UnsafeQueueUserWorkItem(state =>
            {
                var dgv = dataGridView1;
                int oknum = 0;

                int batchNum = 10;
                object[][] arrobj = new object[batchNum][];
                int objidx = 0;
                foreach (string file in files)
                {
                    if (!File.Exists(file))
                    {
                        continue;
                    }
                    Metadata exif = ImgHelper.GetExif(file);
                    if (!exif.IsImg)
                    {
                        continue;
                    }
                    //string exif = Utility.XmlSerialize(m);

                    string dir = Path.GetDirectoryName(file);
                    string name = Path.GetFileName(file);
                    string modiTime = File.GetLastWriteTime(file).ToString("yyyy-MM-dd HH:mm:ss");

                    object[] row = new object[9];
                    row[0] = oknum + 1;
                    row[1] = name;
                    row[2] = dir;
                    row[3] = modiTime;
                    row[4] = exif.EquipmentMake.DisplayValue;
                    row[5] = exif.CameraModel.DisplayValue;
                    row[6] = exif.DatePictureTaken.DisplayValue;
                    row[7] = exif.Fstop.DisplayValue;
                    row[8] = exif.ISOSpeed.DisplayValue;
                    arrobj[objidx] = row;
                    objidx++;
                    if (objidx >= arrobj.Length)
                    {
                        object[][] arroldobj = arrobj;
                        arrobj = new object[batchNum][];
                        objidx = 0;
                        Utility.InvokeControl(dgv, () =>
                        {
                            foreach (object[] objrow in arroldobj)
                            {
                                dgv.Rows.Add(objrow);
                            }
                        });
                    }
                    oknum++;
                }
                if (oknum == 0)
                {
                    MessageBox.Show("没有找到图片文件，你搞错目录了吧！");
                }
                Utility.InvokeControl(dgv, () =>
                {
                    foreach (object[] objrow in arrobj)
                    {
                        if (objrow != null && objrow[0] != null)
                        {
                            dgv.Rows.Add(objrow);
                        }
                    }
                });
            }, null);
        }

        private void InitCols()
        {
            var dgv = dataGridView1;
            //dgv.AutoSize = true;
            DataGridViewColumn column = new DataGridViewTextBoxColumn();
            column.HeaderText = "序号";
            column.AutoSizeMode = DataGridViewAutoSizeColumnMode.DisplayedCells;
            dgv.Columns.Add(column);

            column = new DataGridViewTextBoxColumn();
            column.HeaderText = "目录";
            column.AutoSizeMode = DataGridViewAutoSizeColumnMode.DisplayedCells;
            dgv.Columns.Add(column);

            column = new DataGridViewTextBoxColumn();
            column.HeaderText = "文件名";
            column.AutoSizeMode = DataGridViewAutoSizeColumnMode.DisplayedCells;
            dgv.Columns.Add(column);

            column = new DataGridViewTextBoxColumn();
            column.HeaderText = "最后修改时间";
            column.AutoSizeMode = DataGridViewAutoSizeColumnMode.DisplayedCells;
            dgv.Columns.Add(column);

            column = new DataGridViewTextBoxColumn();
            column.HeaderText = "厂商";
            column.AutoSizeMode = DataGridViewAutoSizeColumnMode.AllCells;
            dgv.Columns.Add(column);

            column = new DataGridViewTextBoxColumn();
            column.HeaderText = "相机型号";
            column.AutoSizeMode = DataGridViewAutoSizeColumnMode.AllCells;
            dgv.Columns.Add(column);

            column = new DataGridViewTextBoxColumn();
            column.HeaderText = "拍照时间";
            column.AutoSizeMode = DataGridViewAutoSizeColumnMode.DisplayedCells;
            dgv.Columns.Add(column);

            column = new DataGridViewTextBoxColumn();
            column.HeaderText = "光圈";
            column.AutoSizeMode = DataGridViewAutoSizeColumnMode.AllCells;
            dgv.Columns.Add(column);

            column = new DataGridViewTextBoxColumn();
            column.HeaderText = "ISO感光度";
            column.AutoSizeMode = DataGridViewAutoSizeColumnMode.AllCells;
            dgv.Columns.Add(column);

            column = new DataGridViewTextBoxColumn();
            column.HeaderText = "重命名预览";
            column.AutoSizeMode = DataGridViewAutoSizeColumnMode.AllCells;
            dgv.Columns.Add(column);

            column = new DataGridViewTextBoxColumn();
            column.HeaderText = "";
            column.AutoSizeMode = DataGridViewAutoSizeColumnMode.AllCells;
            dgv.Columns.Add(column);
        }

    }
}
