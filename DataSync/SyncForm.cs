using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Diagnostics;
using System.Drawing;
using System.Runtime.Serialization;
using System.Text;
using System.Threading;
using System.Windows.Forms;

namespace Beinet.cn.Tools.DataSync
{
    public partial class SyncForm : Form
    {
        public SyncForm(SyncTask task)
        {
            InitializeComponent();
            ShowInTaskbar = false;// 不能放到OnLoad里，会导致窗体消失

            if (task.TimeOut <= 0)
                task.TimeOut = 30;

            m_task = task;
        }


        private bool m_canceled = false;
        private bool m_finished = false;
        private readonly SyncTask m_task;

        protected override void OnLoad(EventArgs e)
        {
            base.OnLoad(e);

            if (Owner != null)
            {
                Left = Owner.Left + 20;
                Top = Owner.Top + Owner.Height - Height - 20;
                if (Top < 0)
                    Top = 0;
            }

            ThreadPool.UnsafeQueueUserWorkItem(DoSync, null);

            //new Thread(DoSync).Start();
        }

        void DoSync(object state)
        {
            foreach (var item in m_task.Items)
            {
                if (m_canceled)
                    break;

                var row = new ListViewItem(new string[] { item.Source, item.Target, "处理中……" });
                _viewItem = row;
                ListViewAddRow(lvTables, row);

                int errSetp = 0;
                try
                {
                    string sql = item.Source;
                    if (!item.IsSqlSource)
                    {
                        sql = "SELECT * FROM [" + sql + "]";
                        if (m_task.AddNoLock)
                            sql += " WITH(NOLOCK)";
                    }
                    errSetp = 10;
                    SqlCommand command;
                    using (SqlDataReader reader = SqlHelper.ExecuteReader(m_task.SourceConstr, sql, m_task.TimeOut, out command))
                    {
                        errSetp = 20;
                        if (!reader.HasRows)
                        {
                            SetListViewText(_viewItem, 2, "源表无数据，未同步", Color.Red);
                            continue;
                        }
                        errSetp = 30;
                        List<string> arrColNames = SqlHelper.GetColNames(reader);
                        if(arrColNames.Count <= 0)
                        {
                            SetListViewText(_viewItem, 2, "源表未选择字段，未同步", Color.Red);
                            if (!m_task.ErrContinue)
                                break;
                            continue;
                        }

                        if (SqlHelper.ExistsTable(m_task.TargetConstr, item.Target))
                        {
                            errSetp = 40;
                            //检查目标表结构和源表结构是否一致
                            StringBuilder checkSchmaSql = new StringBuilder("select ");
                            foreach (string colName in arrColNames)
                            {
                                checkSchmaSql.Append(colName + ",");
                            }
                            checkSchmaSql.Remove(checkSchmaSql.Length - 1, 1);// 删除最后一个逗号
                            checkSchmaSql.Append(" from " + item.Target + " where 1=2");
                            try
                            {
                                errSetp = 43;
                                SqlHelper.ExecuteNonQuery(m_task.TargetConstr, checkSchmaSql.ToString());
                            }
                            catch(Exception exp)
                            {
                                SetListViewText(_viewItem, 2, "目标表缺少源表字段:" + exp.Message, Color.Red);
                                command.Cancel();// 如果DataReader数据量非常大,Close会超时，所以这里必须调用command.Cancel方法来减少超时
                                //reader.Close();

                                if (!m_task.ErrContinue)
                                    break;
                                continue;
                            }
                            errSetp = 45;
                            if (item.TruncateOld)
                            {
                                try
                                {
                                    SqlHelper.ClearTable(m_task.TargetConstr, item.Target, m_task.UseTruncate);
                                }
                                catch (Exception exp)
                                {
                                    SetListViewText(_viewItem, 2, "清空数据出错,请改用TRUNCATE:" + exp.Message, Color.Red);
                                    command.Cancel();// 如果DataReader数据量非常大,Close会超时，所以这里必须调用command.Cancel方法来减少超时
                                    //reader.Close();

                                    if (!m_task.ErrContinue)
                                        break;
                                    continue;
                                }
                            }
                            errSetp = 50;
                        }
                        else
                        {
                            errSetp = 60;
                            // 目标不存在，创建它
                            string createSql = SqlHelper.GetCreateSql(reader, item.Target);
                            SqlHelper.ExecuteNonQuery(m_task.TargetConstr, createSql);
                            errSetp = 70;
                        }
                        var opn = item.UseIdentifier ? SqlBulkCopyOptions.KeepIdentity : SqlBulkCopyOptions.Default;
                        Stopwatch sw = new Stopwatch();
                        sw.Start();
                        errSetp = 80;
                        using (SqlBulkCopy bcp = new SqlBulkCopy(m_task.TargetConstr, opn))
                        {
                            errSetp = 90;
                            bcp.BulkCopyTimeout = m_task.TimeOut;
                            bcp.SqlRowsCopied += bcp_SqlRowsCopied; // 用于进度显示
                            int batchSize = 2000;
                            bcp.BatchSize = batchSize;
                            bcp.NotifyAfter = batchSize; // 设置为1，状态栏提示比较准确，但是速度很慢

                            bcp.DestinationTableName = item.Target;

                            // 设置同名列的映射,避免建表语句列顺序不一致导致无法同步的bug
                            errSetp = 100;
                            foreach (string colName in arrColNames)
                            {
                                bcp.ColumnMappings.Add(colName, colName);
                            }
                            errSetp = 110;
                            bcp.WriteToServer(reader);
                            errSetp = 120;
                        }
                        //long totalrow = Common.GetTableRows(targetCon, item.Target);
                        sw.Stop();
                        errSetp = 130;
                        string oldTxt = _viewItem.SubItems[2].Text;
                        SetListViewText(_viewItem, 2,
                            oldTxt + " 同步完成,耗时:" + sw.ElapsedMilliseconds.ToString("N0") + "毫秒", Color.Green);//，记录数:" + totalrow.ToString("N0")
                        errSetp = 140;
                    }
                }
                catch (OperationAbortedException)
                {
                    //bcp_SqlRowsCopied里调用Abort
                }
                catch (Exception exp)
                {
                    string err = exp.ToString();
                    var sqlExp = exp as SqlException;
                    if (sqlExp != null && !string.IsNullOrEmpty(sqlExp.Server))
                        err = sqlExp.Server + ":" + err;
                    //if (isBulkErr)
                    err = "Step " + errSetp.ToString() + "错误: " + err;
                    SetListViewText(_viewItem, 2, err, Color.Red);
                    if (!m_task.ErrContinue)
                        break;
                }
            }
            m_finished = true;
            btnCancel.Enabled = false;
            //MessageBox.Show("完成");
        }

        private ListViewItem _viewItem;
        void bcp_SqlRowsCopied(object sender, SqlRowsCopiedEventArgs args)
        {
            if (m_canceled)
            {
                args.Abort = true;
                //var bcp = sender as SqlBulkCopy;
                //if (bcp != null)
                //    bcp.Close();// 不能在这里调用Close事件，会异常
            }
            if (_viewItem != null)
            {
                SetListViewText(_viewItem, 2,
                    "已同步:" + args.RowsCopied.ToString("N0") + "条" + (m_canceled ? " 已中止" : ""),
                    Color.Black);
            }
        }

        private delegate void SetListViewTextDele(ListViewItem item, int col, string text, Color color);
        void SetListViewText(ListViewItem item, int col, string text, Color color)
        {
            if (item == null || item.ListView == null)
                return;
            if (item.ListView.InvokeRequired)
            {
                SetListViewTextDele de = SetListViewText;
                Invoke(de, item, col, text, color);
            }
            else
            {
                item.SubItems[col].Text = text;
                item.SubItems[col].ForeColor = color;
            }
        }

        private delegate void ListViewAddRowDele(ListView lv, ListViewItem item);
        void ListViewAddRow(ListView lv, ListViewItem item)
        {
            if (lv == null || item == null)
                return;
            if (lv.InvokeRequired)
            {
                ListViewAddRowDele de = ListViewAddRow;
                Invoke(de, lv, item);
            }
            else
            {
                lv.Items.Add(item);
            }
        }


        private void btnClose_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        protected override void OnClosing(System.ComponentModel.CancelEventArgs e)
        {
            //base.OnClosing(e);
            if (!m_canceled && !m_finished &&
                MessageBox.Show("尚未同步完成，是否确认中止退出？", "未同步完成", MessageBoxButtons.YesNo) != DialogResult.Yes)
            {
                e.Cancel = true;
            }
            else
            {
                m_canceled = true;
            }
        }

        private void btnCancel_Click(object sender, EventArgs e)
        {
            if (!m_canceled && !m_finished &&
                MessageBox.Show("尚未同步完成，是否确认中止退出？", "未同步完成", MessageBoxButtons.YesNo) == DialogResult.Yes)
            {
                m_canceled = true;
                btnCancel.Enabled = false;
            }
        }

        private void SyncForm_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (e.KeyChar == 27)// 27表示ESC
            {
                if (!m_canceled && !m_finished)
                    btnCancel_Click(sender, e);
                else
                    btnClose_Click(sender, e);
            }
        }

        private void lvTables_MouseDoubleClick(object sender, MouseEventArgs e)
        {
            if (lvTables.SelectedItems.Count <= 0)
                return;
            var sb = new StringBuilder();
            foreach (ListViewItem item in lvTables.SelectedItems)
            {
                sb.AppendLine(item.SubItems[0].Text + "," + item.SubItems[1].Text + "," + item.SubItems[2].Text);
            }
            Clipboard.SetText(sb.ToString());
            MessageBox.Show("已经复制到粘贴板");
        }
    }

    [DataContract]
    public class SyncItem
    {
        /// <summary>
        /// 源表或源SQL
        /// </summary>
        [DataMember(EmitDefaultValue = false, IsRequired = false, Order = 0)]
        public string Source { get; set; }

        /// <summary>
        /// 目标表名
        /// </summary>
        [DataMember(EmitDefaultValue = false, IsRequired = false, Order = 1)]
        public string Target { get; set; }

        /// <summary>
        /// 指示Source是表名还是SQL
        /// </summary>
        [DataMember(EmitDefaultValue = false, IsRequired = false, Order = 2, Name = "IsSql")]
        public bool IsSqlSource { get; set; }

        /// <summary>
        /// 同步前是否要清空目标表
        /// </summary>
        [DataMember(EmitDefaultValue = false, IsRequired = false, Order = 3, Name = "trun")]
        public bool TruncateOld { get; set; }

        /// <summary>
        /// 同步数据时是否使用标识插入，这样可以同步那些Identify字段
        /// </summary>
        [DataMember(EmitDefaultValue = false, IsRequired = false, Order = 4, Name = "iden")]
        public bool UseIdentifier { get; set; }

    }

    [DataContract]
    public class SyncTask
    {
        /// <summary>
        /// 源数据库连接字符串
        /// </summary>
        [DataMember(EmitDefaultValue = false, IsRequired = false, Order = 0)]
        public string SourceConstr { get; set; }

        /// <summary>
        /// 目标数据库连接字符串
        /// </summary>
        [DataMember(EmitDefaultValue = false, IsRequired = false, Order = 1)]
        public string TargetConstr { get; set; }

        /// <summary>
        /// 出错时是否继续同步下面的表
        /// </summary>
        [DataMember(EmitDefaultValue = false, IsRequired = false, Order = 2)]
        public bool ErrContinue { get; set; }

        /// <summary>
        /// 要同步的所有源表
        /// </summary>
        [DataMember(EmitDefaultValue = false, IsRequired = false, Order = 3)]
        public IEnumerable<SyncItem> Items { get; set; }

        /// <summary>
        /// 同步表时，是否要增加NoLock约束
        /// </summary>
        [DataMember(EmitDefaultValue = false, IsRequired = false, Order = 4)]
        public bool AddNoLock { get; set; }

        /// <summary>
        /// 超时时间设置
        /// </summary>
        [DataMember(EmitDefaultValue = false, IsRequired = false, Order = 5)]
        public int TimeOut { get; set; }

        /// <summary>
        /// 是否加密，用于兼容旧配置文件
        /// </summary>
        [DataMember(EmitDefaultValue = false, IsRequired = false, Order = 6, Name = "en")]
        public bool Encrypted { get; set; }

        /// <summary>
        /// 清空目标表里是否使用Truncate语句（因为Truncate要求权限比较高，有时可能没有这个权限）
        /// </summary>
        [DataMember(EmitDefaultValue = false, IsRequired = false, Order = 7, Name = "del")]
        public bool UseTruncate { get; set; }
    }
}
