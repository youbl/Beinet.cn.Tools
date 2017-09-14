using System;
using System.Data;
using System.IO;
using System.Text;
using System.Threading;
using System.Windows.Forms;

namespace Beinet.cn.Tools.DataSync
{
    public partial class IISLogForm : Form
    {
        public IISLogForm()
        {
            InitializeComponent();
            txtTbName.Text = "iislog" + DateTime.Now.ToString("yyyyMM");
        }

        private void button1_Click(object sender, EventArgs e)
        {
            var ofd = new OpenFileDialog();
            ofd.Multiselect = true;
            ofd.InitialDirectory = AppDomain.CurrentDomain.BaseDirectory;
            ofd.Filter = "Log files (*.log)|*.log|All files (*.*)|*.*";

            var dialogRet = ofd.ShowDialog(this);
            if (dialogRet != DialogResult.OK)
                return;
            var logpath = ofd.FileNames;
            if (logpath.Length > 0)
            {
                foreach (string file in logpath)
                {
                    labFiles.Text += Path.GetFileName(file) + ";";
                }
                ImportIISLogs(logpath);
            }
        }

        private void ImportIISLogs(string[] files)
        {
            string constr = txtConstr.Text;
            //ImportOneIISLog(@"D:\work-mike\log\ttt.log", constr);
            //return;
            ThreadPool.UnsafeQueueUserWorkItem(state =>
            {
                foreach (var file in files)
                {
                    try
                    {
                        ImportOneIISLog(file, constr);
                    }
                    catch (Exception exp)
                    {
                        MessageBox.Show(Path.GetFileName(file) + Environment.NewLine + exp.Message);
                    }
                }
            }, null);
        }

        private DataTable CreateIisTable(string fields, string constr)
        {
            var iisTableName = txtTbName.Text;
            var dt = new DataTable(iisTableName);

            var arr = fields.Trim().Split(' ');
            var sql = "create table " + iisTableName + "([id] [int] IDENTITY(1,1) primary key NOT NULL";
            foreach (var row in arr)
            {
                var field = row.Trim();
                dt.Columns.Add(field, typeof(string));

                var len = 20;
                if (field == "cs-uri-stem" || field == "cs-uri-query" || field == "cs(Referer)" || field == "cs(User-Agent)")
                {
                    len = 5000;
                }
                sql += ",[" + field + "]" + " varchar(" + len + ")";
            }
            sql += ")";

            var sqlexists = "select 1 from sys.objects WHERE type='u' AND name='" + iisTableName + "'";
            if (SqlHelper.ExecuteScalar(constr, sqlexists) != null)
            {
                return dt;
            }

            SqlHelper.ExecuteNonQuery(constr, sql);
            // 建索引
            sql = "CREATE INDEX [idxtime] ON " + iisTableName + "([date],[time])";
            SqlHelper.ExecuteNonQuery(constr, sql);
            return dt;
        }

        private void ImportOneIISLog(string file, string constr)
        {
            var begintime = DateTime.Now;
            var sum = 0;
            DataTable iisTable = null;//CreateIisTable();
            using (var sr = new StreamReader(file, Encoding.GetEncoding("GB2312")))
            {
                while (!sr.EndOfStream)
                {
                    var line = sr.ReadLine() ?? "";
                    if (line.StartsWith("#"))
                    {
                        string fieldstart = "#Fields: ";
                        if (line.StartsWith(fieldstart))
                        {
                            try
                            {
                                // 根据iis里的日志建表
                                iisTable = CreateIisTable(line.Substring(fieldstart.Length), constr);
                            }
                            catch (Exception exp)
                            {
                                MessageBox.Show("建表失败" + Environment.NewLine + exp.Message);
                                return;
                            }
                        }
                        continue;
                    }
                    // 默认iis日志要先有field行，然后创建表，没有field行，将导入失败
                    if (iisTable == null)
                    {
                        continue;
                    }
                    var arrdata = line.Split(' ');
                    if (arrdata.Length != iisTable.Columns.Count)
                    {
                        continue;
                    }
                    var row = iisTable.NewRow();
                    for (int i = 0, j = arrdata.Length; i < j; i++)
                    {
                        row[i] = arrdata[i];
                    }
                    iisTable.Rows.Add(row);
                    Interlocked.Increment(ref sum);
                    if (sum % 3000 == 0)
                    {
                        var outsum = sum;
                        try
                        {
                            SqlHelper.BulkCopy(iisTable, constr, iisTable.TableName);
                        }
                        catch (Exception exp)
                        {
                            MessageBox.Show("本次插入失败:" + Environment.NewLine + exp.Message);
                        }
                        iisTable.Rows.Clear();
                        var tm = (DateTime.Now - begintime).TotalMilliseconds.ToString("N0");
                        Utility.InvokeControl(txtRet, () =>
                        {
                            txtRet.Text = Path.GetFileName(file) + " 导入: " + outsum.ToString("N0")
                                          + "条, 耗时:" + tm + "毫秒" + Environment.NewLine + txtRet.Text;
                        });
                    }
                }
            }
            if (iisTable == null)
            {
                var tm3 = (DateTime.Now - begintime).TotalMilliseconds.ToString("N0");
                Utility.InvokeControl(txtRet, () =>
                {
                    txtRet.Text = Path.GetFileName(file) + " 未找到Field标题行，导入失败, 耗时:"
                                  + tm3 + "毫秒" + Environment.NewLine + txtRet.Text;
                });
                return;
            }
            SqlHelper.BulkCopy(iisTable, constr, iisTable.TableName);
            iisTable.Rows.Clear();
            var end2 = DateTime.Now;
            var tm2 = (end2 - begintime).TotalMilliseconds.ToString("N0");
            Utility.InvokeControl(txtRet, () =>
            {
                txtRet.Text = end2.ToString("HH:mm:ss") + Path.GetFileName(file) + " 导入完成: " + sum.ToString("N0")
                                   + "条, 耗时:" + tm2 + "毫秒" + Environment.NewLine + txtRet.Text;
            });
        }

    }
}
