using System;
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
                dataGridView1.DataSource = ds.Tables[0];
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
            if (server == rootNode.Text)
            {
                return true;
            }

            rootNode.Text = "未连接";
            rootNode.Nodes.Clear();

            var sql = "SELECT type,name FROM sys.objects WHERE type in ('u','v','p','tr','fn') ORDER BY type,name";
            var begintime = DateTime.Now;
            try
            {
                using (var reader = SqlHelper.ExecuteReader(constr, sql))
                {
                    var usetime = (DateTime.Now - begintime).TotalMilliseconds;
                    labStatus.Text = "连接成功,耗时:" + usetime.ToString("N0") + "毫秒";
                    rootNode.Text = server;
                    while (reader.Read())
                    {
                        var name = reader["name"] + "-" + reader["type"];
                        rootNode.Nodes.Add(new TreeNode(name));
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
            var nodetxt = tvDB.SelectedNode.Text;
            var idx = nodetxt.LastIndexOf("-", StringComparison.Ordinal);
            if (idx > 0)
            {
                nodetxt = nodetxt.Substring(0, idx);
            }
            txtSql.Text += "\r\nSELECT TOP 1000 * \r\n FROM [" + nodetxt + "]";
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
