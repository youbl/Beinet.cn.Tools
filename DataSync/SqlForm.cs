using System;
using System.Configuration;
using System.Data;
using System.Text.RegularExpressions;
using System.Windows.Forms;

namespace Beinet.cn.Tools.DataSync
{
    public partial class SqlForm : Form
    {
        public SqlForm()
        {
            InitializeComponent();
        }

        protected override void OnLoad(EventArgs e)
        {
            base.OnLoad(e);

            txtSql.KeyUp += Utility.txt_KeyDownUp;
            txtConstr.KeyUp += Utility.txt_KeyDownUp;
        }

        private void SqlForm_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.F5 || e.KeyCode == Keys.F9)
            {
                RunSql();
            }
        }

        private void RunSql()
        {
            if (!TestConnection())
            {
                return;
            }
            // 只执行选中的sql
            var sql = txtSql.SelectedText;
            if (string.IsNullOrEmpty(sql))
            {
                sql = txtSql.Text;
            }
            // dataGridView1.AutoGenerateColumns = false;
            dataGridView1.DataSource = null;
            labStatus.Text = "";
            var begintime = DateTime.Now;
            try
            {
                var ds = SqlHelper.ExecuteDataSet(txtConstr.Text, sql);
                var usetime = (DateTime.Now - begintime).TotalMilliseconds;
                labStatus.Text = "耗时:" + usetime.ToString("N0") + "毫秒";
                if (ds.Tables.Count <= 0)
                {
                    MessageBox.Show("未找到数据");
                    return;
                }
                labStatus.Text += "; " + ds.Tables[0].Rows.Count.ToString("N0") + "行";

                dataGridView1.DataSource = ConvertDt(ds.Tables[0]);
                dataGridView1.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.AllCells;
                /*using (var reader = SqlHelper.ExecuteReader(constr, sql))
                {
                    reader
                }*/
            }
            catch (Exception exp)
            {
                var usetime = (DateTime.Now - begintime).TotalMilliseconds;
                labStatus.Text = "耗时:" + usetime.ToString("N0") + "毫秒";
                MessageBox.Show(exp.Message);
            }
        }

        private DataTable ConvertDt(DataTable dt)
        {
            for (var i = dt.Columns.Count - 1; i >= 0; i--)
            {
                var column = dt.Columns[i];
                if (column.DataType == typeof(byte[]))
                {
                    dt.Columns.Remove(column);
                }
            }
            return dt;
            //var arrConvert = new List<DataColumn>();
            //foreach (DataColumn column in dt.Columns)
            //{
            //    // byte会被识别为图片并展示
            //    if (column.DataType == typeof(byte[]))
            //    {
            //        arrConvert.Add(column);
            //    }
            //}
            //foreach (var column in arrConvert)
            //{
            //    var newColName = column.ColumnName + "-2";
            //    dt.Columns.Add(newColName, typeof(string));
            //    foreach (DataRow row in dt.Rows)
            //    {
            //        var obj = row[column.ColumnName];
            //        if (obj == null || obj == DBNull.Value)
            //        {
            //            continue;
            //        }
            //        row[newColName] = Encoding.UTF8.GetString((byte[])obj);
            //    }
            //}
            //foreach (var column in arrConvert)
            //{
            //    dt.Columns.Remove(column);
            //}
            //return dt;
        }

        private void btnTest_Click(object sender, EventArgs e)
        {
            TestConnection();
        }

        private bool TestConnection()
        {
            if (tvDB.Nodes.Count <= 0)
            {
                tvDB.Nodes.Add(new TreeNode("未连接"));
            }
            var rootNode = tvDB.Nodes[0];

            var constr = txtConstr.Text.Trim();
            if (constr == string.Empty)
            {
                MessageBox.Show("请输入数据库连接字符串");
                return false;
            }
            var server = GetServer(constr);
            if (constr == Convert.ToString(rootNode.Tag))
            {
                return true;
            }
            
            rootNode.Text = "未连接";
            rootNode.Nodes.Clear();
            rootNode.Nodes.Add(new TreeNode("用户表"));
            rootNode.Nodes.Add(new TreeNode("视图"));
            rootNode.Nodes.Add(new TreeNode("存储过程"));
            rootNode.Nodes.Add(new TreeNode("触发器"));
            rootNode.Nodes.Add(new TreeNode("函数"));

            var sql = "SELECT type,name FROM sys.objects WHERE type in ('u','v','p','tr','fn') ORDER BY type,name";
            var begintime = DateTime.Now;
            try
            {
                using (var reader = SqlHelper.ExecuteReader(constr, sql))
                {
                    var usetime = (DateTime.Now - begintime).TotalMilliseconds;
                    labStatus.Text = "连接成功,耗时:" + usetime.ToString("N0") + "毫秒";
                    rootNode.Text = server;
                    rootNode.Tag = constr;
                    while (reader.Read())
                    {
                        var name = Convert.ToString(reader["name"]);
                        var nodeIdx = 0;
                        switch (Convert.ToString(reader["type"]).ToLower().Trim())
                        {
                            case "u":
                                nodeIdx = 0;
                                break;
                            case "v":
                                nodeIdx = 1;
                                break;
                            case "p":
                                nodeIdx = 2;
                                break;
                            case "tr":
                                nodeIdx = 3;
                                break;
                            case "fn":
                                nodeIdx = 4;
                                break;
                        }
                        rootNode.Nodes[nodeIdx].Nodes.Add(new TreeNode(name));
                    }
                }
                rootNode.ExpandAll();
                return true;
            }
            catch (Exception exp)
            {
                var usetime = (DateTime.Now - begintime).TotalMilliseconds;
                labStatus.Text = "连接失败，耗时:" + usetime.ToString("N0") + "毫秒";
                MessageBox.Show(exp.Message);
                return false;
            }
        }

        /// <summary>
        /// 从连接字符串里提取服务器名或ip
        /// </summary>
        /// <param name="connectionstring"></param>
        /// <returns></returns>
        private string GetServer(string connectionstring)
        {
            var match = Regex.Match(connectionstring, "server=([^;]+)");
            if (match.Success)
            {
                return match.Result("$1");
            }
            return connectionstring.GetHashCode().ToString();
        }

        private void tvDB_DoubleClick(object sender, EventArgs e)
        {
            var node = tvDB.SelectedNode;
            
            var nodetxt = node.Text;
            if (txtSql.Text.Length > 0)
            {
                txtSql.Text += "\r\n\r\n";
            }
            string sql;
            if (node.Level == 2 && node.Parent.Text == "用户表")
            {
                sql = "SELECT TOP 1000 * \r\n FROM [" + nodetxt + "]";
            }
            else
            {
                sql = nodetxt;
            }
            txtSql.Text += sql;
            var len = sql.Length;
            var alllen = txtSql.Text.Length;
            txtSql.Focus();
            txtSql.Select(alllen - len, len);

            // txtSql.SelectedText = sql;
        }

        private void dataGridView1_DataError(object sender, DataGridViewDataErrorEventArgs e)
        {
            e.ThrowException = false;
        }
        
        private void dataGridView1_ColumnAdded(object sender, DataGridViewColumnEventArgs e)
        {
            if (e.Column == null)
            {
                return;
            }
            var type = e.Column.ValueType;
            if (type == null)
            {
                e.Column.ValueType = typeof(string);
                return;
            }
            if(type == typeof(byte[]))
            {
                e.Column.ValueType = typeof(string);
                // return;
            }
        }

        private void SqlForm_Load(object sender, EventArgs e)
        {
            txtConstr.Text = ConfigurationManager.AppSettings["DefalutConn"];
        }

        /*
        protected override void OnFormClosing(FormClosingEventArgs e)
        {
            // 不关闭，只隐藏
            e.Cancel = true;
            Hide();
            // base.OnFormClosing(e);
        }*/
    }
}
