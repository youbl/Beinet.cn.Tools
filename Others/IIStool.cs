using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.IO;
using System.Text;
using System.Threading;
using System.Windows.Forms;

namespace Beinet.cn.Tools.Others
{
    public partial class IIStool : Form
    {
        private const string IDLE_TXT = "启动导入";
        private const string WORK_TXT = "导入中...";
        public IIStool()
        {
            InitializeComponent();
        }

        protected override void OnLoad(EventArgs e)
        {
            base.OnLoad(e);

            btnImport.Text = IDLE_TXT;
            txtRet.KeyUp += Utility.txt_KeyDownUp;
            txtDbTarget.KeyUp += Utility.txt_KeyDownUp;
            txtLogFile.KeyUp += Utility.txt_KeyDownUp;
            txtTbName.KeyUp += Utility.txt_KeyDownUp;
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
            fbd.Multiselect = true;
            if (fbd.ShowDialog(this) != DialogResult.OK)
                return;

            txtLogFile.Text = string.Join(",", fbd.FileNames);
        }

        private void btnImport_Click(object sender, EventArgs e)
        {
            if (btnImport.Text.StartsWith(WORK_TXT, StringComparison.Ordinal))
            {
                MessageBox.Show("导入中，请稍候");
                return;
            }
            string logfile = txtLogFile.Text.Trim();
            if (logfile == string.Empty)// || !File.Exists(logfile))
            {
                MessageBox.Show("请选择正确的IIS日志文件");
                return;
            }
            //if (!IsTextFile(logfile))
            //{
            //    MessageBox.Show("请选择文本格式的文件");
            //    return;
            //}

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

            btnImport.Text = WORK_TXT;
            bool add8 = chkTimeAdd8.Checked;

            ThreadPool.UnsafeQueueUserWorkItem(state =>
            {
                if (chkClear.Checked)
                {
                    if (SqlHelper.ExistsTable(constr, tbname))
                    {
                        SqlHelper.ExecuteNonQuery(constr, "truncate table [" + tbname + "]");
                    }
                }

                try
                {
                    ImportLog(logfile, constr, tbname, add8, txtRet);
                }
                catch (Exception exp)
                {
                    MessageBox.Show("导入错误：" + exp.Message);
                }
                Utility.InvokeControl(btnImport, () => btnImport.Text = IDLE_TXT);
            }, null);
        }

        /// <summary>
        /// 用于给小时加8用
        /// </summary>
        static int __dateColIdx = -1;
        /// <summary>
        /// 用于给小时加8用
        /// </summary>
        static int __timeColIdx = -1;
        static void ImportLog(string logfile, string constr, string tbname, bool add8, TextBox txt)
        {
            var totalBegin = DateTime.Now;
            string msg1 = totalBegin.ToString("HH:mm:ss.fff") + "启动\r\n";
            Utility.InvokeControl(txt, () => txt.Text = msg1);

            var insertNum = 0;
            var dt = new DataTable();
            var files = logfile.Split(',');
            bool isTbAdded = false;
            foreach (var file in files)
            {
                if (!File.Exists(file))
                {
                    continue;
                }
                insertNum +=  ImportPerLog(file, constr, tbname, add8, txt, dt, ref isTbAdded);
            }
            
            var totalEnd = DateTime.Now;
            var totalTime = (totalEnd - totalBegin).TotalSeconds;
            var msg2 = totalEnd.ToString("HH:mm:ss.fff") +" " + insertNum.ToString() + 
                "行完成，总过程耗时:" + totalTime.ToString("N2") + "\r\n" + txt.Text;
            Utility.InvokeControl(txt, () => txt.Text = msg2);
        }

        static int ImportPerLog(string logfile, string constr, string tbname, bool add8, TextBox txt, DataTable dt, ref bool isTbAdded)
        {
            var totalBegin = DateTime.Now;
            double totalInsert = 0;
            string msg1 = logfile + ":" + totalBegin.ToString("HH:mm:ss.fff") + "启动\r\n" + txt.Text;
            Utility.InvokeControl(txt, () => txt.Text = msg1);

            var insertNum = 0;
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
                            return insertNum;
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
                    if (add8 && __dateColIdx >=0 && __timeColIdx >= 0)
                    {
                        Add8Hour(linedata, __dateColIdx, __timeColIdx);
                    }
                    insertNum++;
                    if (isTbAdded)
                    {
                        AddToDT(linedata, dt);
                        if (insertNum % 100000 == 0)
                        {
                            var insertBegin = DateTime.Now;
                            SqlHelper.BulkCopy(dt, constr, tbname);
                            var insertEnd = DateTime.Now;
                            dt.Rows.Clear();
                            var ts = (insertEnd - insertBegin).TotalSeconds;
                            totalInsert += ts;
                            var msgtmp = "  " + insertEnd.ToString("HH:mm:ss.fff") + " " + insertNum
                                         + "行完成，本次插入耗时:" + ts.ToString("N2") + "秒，"
                                         + "总插入耗时:" + totalInsert.ToString("N2") + "秒\r\n" + txt.Text;
                            Utility.InvokeControl(txt, () => txt.Text = msgtmp);
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
            var msg2 = logfile + "\r\n  " + totalEnd.ToString("HH:mm:ss.fff") + " " + insertNum
                         + "行完成，总过程耗时:" + totalTime.ToString("N2") + "秒，"
                         + "本次插入耗时:" + ts2.ToString("N2") + "秒，"
                         + "总插入耗时:" + totalInsert.ToString("N2") + "秒\r\n" + txt.Text;
            Utility.InvokeControl(txt, () => txt.Text = msg2);
            return insertNum;
        }

        static void Add8Hour(string[] rowdata, int dateCol, int timeCol)
        {
            var cnt = rowdata.Length;
            if (cnt <= dateCol || cnt <= timeCol)
            {
                return;
            }
            var strDt = rowdata[dateCol] + " " + rowdata[timeCol];
            DateTime dt;
            if (!DateTime.TryParse(strDt, out dt))
            {
                return;
            }
            dt = dt.AddHours(8);
            rowdata[dateCol] = dt.ToString("yyyy-MM-dd");
            rowdata[timeCol] = dt.ToString("HH:mm:ss");
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
            // [id] [bigint] IDENTITY(1,1) NOT NULL PRIMARY KEY
            var sql = new StringBuilder(@"CREATE TABLE [" + tbname
                + @"](");
            var idx = 0;
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
                    case "date":
                        __dateColIdx = idx;
                        type = "[varchar](10)";
                        break;
                    case "time":
                        __timeColIdx = idx;
                        type = "[varchar](8)";
                        break;
                    default:
                        type = "[varchar](20)";
                        break;
                }
                sql.AppendFormat("[{0}] {1}, ", field, type);
                idx++;
            }
            sql.Remove(sql.Length - 2, 2);
            sql.Append(")");


            if (SqlHelper.ExistsTable(constr, tbname))
            {
                return true;
            }

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
        // ReSharper disable once UnusedMember.Local
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

        private void button1_Click(object sender, EventArgs e)
        {
            var sql = @"select top 100 count(1) [次数],avg([time-taken]) [平均毫秒],sum([time-taken]) [累计毫秒],[cs-method],[cs-uri-stem],[cs-uri-query]
 from iislog2017
where [time] > '10:30:30'
 group by  [cs-method],[cs-uri-stem],[cs-uri-query] having(count(1)>300)
 order by [平均毫秒] desc;

select top 111 left([time],4) minu, count(1) [访问次数] from iislog2017
group by left([time],4)
order by minu;

-- 按小时进行访问量汇总
select top 1111 left(convert(varchar,dateadd(hour,8,cast([date]+' '+[time] as datetime)),120),13) logtime, count(1) [访问次数]
from iislog2017
group by left(convert(varchar,dateadd(hour,8,cast([date]+' '+[time] as datetime)),120),13)
order by logtime
";
            txtRet.Text = sql + "\r\n\r\n" + txtRet.Text;
        }

        private void IIStool_Load(object sender, EventArgs e)
        {
            txtDbTarget.Text = ConfigurationManager.AppSettings["DefalutConn"];
        }
    }
}
