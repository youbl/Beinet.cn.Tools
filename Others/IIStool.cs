using System;
using System.Collections.Generic;
using System.Data;
using System.IO;
using System.Text;
using System.Threading;
using System.Windows.Forms;

namespace Beinet.cn.Tools.Others
{
    public partial class IIStool : Form
    {
        public IIStool()
        {
            InitializeComponent();
        }

        private void btnLogFile_Click(object sender, EventArgs e)
        {
            var fbd = new OpenFileDialog();
            fbd.Filter = "日志文件(*.log;*.txt)|*.log;*.txt|所有文件|*.*";
            string dir = txtLogFile.Text.Trim();
            if (dir != string.Empty)
            {
                fbd.FileName = dir;
            }
            if (fbd.ShowDialog(this) != DialogResult.OK)
                return;

            txtLogFile.Text = fbd.FileName;
        }

        private void btnImport_Click(object sender, EventArgs e)
        {
            string logfile = txtLogFile.Text.Trim();
            if (logfile == string.Empty || !File.Exists(logfile))
            {
                MessageBox.Show("请选择正确的IIS日志文件");
                return;
            }
            if (!IsTextFile(logfile))
            {
                MessageBox.Show("请选择文本格式的文件");
                return;
            }

            string constr = txtDbTarget.Text.Trim();
            if (constr == string.Empty)
            {
                MessageBox.Show("请输入数据库连接字符串");
                return;
            }
            string tbname = txtTbName.Text.Trim();
            if (tbname == string.Empty)
            {
                MessageBox.Show("请输入新表表名");
                return;
            }
            ThreadPool.UnsafeQueueUserWorkItem(state =>
            {
                try
                {
                    ImportLog(logfile, constr, tbname, txtRet);
                }
                catch (Exception exp)
                {
                    MessageBox.Show("导入错误：" + exp.Message);
                }
            }, null);
        }


        static void ImportLog(string logfile, string constr, string tbname, TextBox txt)
        {
            var totalBegin = DateTime.Now;
            double totalInsert = 0;
            string msg1 = totalBegin.ToString("HH:mm:ss.fff") + "启动";
            Utility.InvokeControl(txt, () => txt.Text = msg1 + txt.Text);

            var insertNum = 0;
            bool isTbAdded = false;
            var dt = new DataTable();
            var arrData = new List<string[]>();
            using (var sr = new StreamReader(logfile, Encoding.UTF8))
            {
                while (!sr.EndOfStream)
                {
                    var line = sr.ReadLine();
                    if (string.IsNullOrEmpty(line))
                    {
                        continue;
                    }
                    string fieldPrefix = "#Fields: ";
                    if (!isTbAdded && line.StartsWith(fieldPrefix, StringComparison.Ordinal))
                    {
                        // string[] arrFields = line.Substring(fieldPrefix.Length)
                        //    .Split(new char[] {' '}, StringSplitOptions.RemoveEmptyEntries);
                        string[] arrFields = line.Substring(fieldPrefix.Length).Split(' ');
                        if (arrFields.Length <= 1)
                        {
                            // 字段长度小于2个，忽略不导入
                            MessageBox.Show("字段个数少于2个：" + line);
                            return;
                        }
                        isTbAdded = CreateTable(arrFields, constr, tbname);
                        if (isTbAdded)
                        {
                            // 初始化dt，用于后续bulkcopy
                            foreach (string field in arrFields)
                            {
                                dt.Columns.Add(field, typeof(string));
                            }
                            if (arrData.Count > 0)
                            {
                                foreach (string[] rowdata in arrData)
                                {
                                    AddToDT(rowdata, dt);
                                }
                            }
                        }
                    }

                    if (line[0] == '#')
                    {
                        continue;
                    }
                    string[] linedata = line.Split(' ');
                    insertNum++;
                    if (isTbAdded)
                    {
                        AddToDT(linedata, dt);
                        if (insertNum % 10000 == 0)
                        {
                            var insertBegin = DateTime.Now;
                            SqlHelper.BulkCopy(dt, constr, tbname);
                            var insertEnd = DateTime.Now;
                            dt.Rows.Clear();
                            var ts = (insertEnd - insertBegin).TotalSeconds;
                            totalInsert += ts;
                            var msgtmp = insertEnd.ToString("HH:mm:ss.fff") + " " + insertNum
                                         + "行完成，本次插入耗时:" + ts.ToString("N2") + "秒，"
                                         + "总插入耗时:" + totalInsert.ToString("N2") + "秒\r\n";
                            Utility.InvokeControl(txt, () => txt.Text = msgtmp + txt.Text);
                        }
                    }
                    else
                    {
                        arrData.Add(linedata);
                    }
                }
            }
            var insertBegin2 = DateTime.Now;
            SqlHelper.BulkCopy(dt, constr, tbname);
            var totalEnd = DateTime.Now;
            var ts2 = (totalEnd - insertBegin2).TotalSeconds;
            totalInsert += ts2;
            var totalTime = (totalEnd - totalBegin).TotalSeconds;
            var msg2 = totalEnd.ToString("HH:mm:ss.fff") + " " + insertNum
                         + "行完成，总过程耗时:" + totalTime.ToString("N2") + "秒，"
                         + "本次插入耗时:" + ts2.ToString("N2") + "秒，"
                         + "总插入耗时:" + totalInsert.ToString("N2") + "秒\r\n";
            Utility.InvokeControl(txt, () => txt.Text = msg2 + txt.Text);
        }

        static void AddToDT(string[] rowdata, DataTable dt)
        {
            var row = dt.NewRow();
            for (int i = 0; i < rowdata.Length; i++)
            {
                row[i] = rowdata[i];
            }
            dt.Rows.Add(row);
        }

        /// <summary>
        /// 创建日志用的表
        /// </summary>
        /// <param name="arrFields"></param>
        /// <param name="constr"></param>
        /// <param name="tbname"></param>
        /// <returns></returns>
        static bool CreateTable(string[] arrFields, string constr, string tbname)
        {
            if (SqlHelper.ExistsTable(constr, tbname))
            {
                return true;
            }

            // [id] [bigint] IDENTITY(1,1) NOT NULL PRIMARY KEY
            var sql = new StringBuilder(@"CREATE TABLE [" + tbname
                + @"](");
            foreach (string field in arrFields)
            {
                string type;
                switch (field)
                {
                    case "cs-uri-stem":
                    case "cs(User-Agent)":
                    case "cs-uri-query":
                    case "cs(Referer)":
                        type = "[varchar](max)";
                        break;
                    case "time-taken":
                    case "s-port":
                    case "sc-status":
                    case "sc-substatus":
                    case "sc-win32-status":
                        type = "[int]";
                        break;
                    default:
                        type = "[varchar](20)";
                        break;
                }
                sql.AppendFormat("[{0}] {1}, ", field, type);
            }
            sql.Remove(sql.Length - 2, 2);
            sql.Append(")");
            SqlHelper.ExecuteNonQuery(constr, sql.ToString());
            return true;
        }


        /// <summary>
        /// Checks the file is textfile or not.
        /// 把给定的那个文件看作是无类型的二进制文件，然后顺序地读出这个文件的每一个字节，如果文件里有一个字节的值等于0，
        /// 那么这个文件就不是文本文件；反之，如果这个文件中没有一个字节的值是0的话，就可以判定这个文件是文本文件了
        /// http://www.cnblogs.com/criedshy/archive/2010/05/24/1742918.html
        /// </summary>
        /// <param name="fileName">Name of the file.</param>
        /// <returns></returns>
        static bool IsTextFile(string fileName)
        {
            using (FileStream fs = new FileStream(fileName, FileMode.Open, FileAccess.Read))
            {
                bool isTextFile = true;
                int i = 0;
                int length = (int) fs.Length;

                // 最多检查10M，避免文件太大导致卡死
                int maxlength = 10240 * 1024;
                length = length > maxlength ? maxlength : length;
                byte data;
                while (i < length && isTextFile)
                {
                    data = (byte) fs.ReadByte();
                    isTextFile = (data != 0);
                    i++;
                }
                return isTextFile;
            }
        }
    }
}
