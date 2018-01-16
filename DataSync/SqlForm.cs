using System;
using System.Collections.Generic;
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
            var tbNode = new TreeNode("用户表");
            rootNode.Nodes.Add(tbNode);
            var viewNode = new TreeNode("视图");
            rootNode.Nodes.Add(viewNode);
            var uspNode = new TreeNode("存储过程");
            rootNode.Nodes.Add(uspNode);
            var trigNode = new TreeNode("触发器");
            rootNode.Nodes.Add(trigNode);
            var funcNode = new TreeNode("函数");
            rootNode.Nodes.Add(funcNode);

            // 查询所有表、视图、存储过程、触发器、函数的sql
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
                        TreeNode nodeRoot = tbNode;
                        switch (Convert.ToString(reader["type"]).ToLower().Trim())
                        {
                            case "u":
                                nodeRoot = tbNode;
                                break;
                            case "v":
                                nodeRoot = viewNode;
                                break;
                            case "p":
                                nodeRoot = uspNode;
                                break;
                            case "tr":
                                nodeRoot = trigNode;
                                break;
                            case "fn":
                                nodeRoot = funcNode;
                                break;
                        }
                        var eachTbNode = new TreeNode(name);
                        nodeRoot.Nodes.Add(eachTbNode);
                    }
                }
                rootNode.ExpandAll();

                // 绑定字段和索引
                var cols = GetAllCols(constr);
                var indexes = GetAllIndex(constr);
                foreach (TreeNode subTbNode in tbNode.Nodes)
                {
                    BindColAndIndex(cols, indexes, subTbNode);
                }

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

        static void BindColAndIndex(Dictionary<string, Dictionary<string, string>> cols,
            Dictionary<string, Dictionary<string, string>> indexes, TreeNode tbNode)
        {
            string name = tbNode.Text;
            // 绑定字段
            Dictionary<string, string> tbCols;
            if (!cols.TryGetValue(name, out tbCols))
            {
                return;
            }
            foreach (var pair in tbCols)
            {
                var txt = pair.Key + " " + pair.Value;
                tbNode.Nodes.Add(new TreeNode(txt));
            }

            // 绑定索引
            Dictionary<string, string> tbIdx;
            if (!indexes.TryGetValue(name, out tbIdx))
            {
                return;
            }
            //var nodeIdx = new TreeNode("索引");
            //tbNode.Nodes.Add(nodeIdx);
            foreach (var pair in tbIdx)
            {
                var txt = "索引:" + pair.Value + " (" + pair.Key + ")";
                tbNode.Nodes.Add(new TreeNode(txt));
            }
        }

        static Dictionary<string, Dictionary<string, string>> GetAllCols(string constr)
        {
            var ret = new Dictionary<string, Dictionary<string, string>>();
            // 获取表结构和行号
            var sqlIdx = @"SELECT --b.colorder,
       a.name tablename,
       b.name column_name,
       d.value comments,            -- 字段注释
       e.name data_type,                -- 字段类型
       b.length char_length,            -- 指示nvarchar的长度(不是表定义里的长度)
       b.scale data_scale,                -- 指示number小数的长度
       b.isnullable,                    -- 字段是否允许为空值,1允许,0不允许
       f.text column_default             -- 字段默认值
FROM   sys.sysobjects a
INNER JOIN sys.syscolumns b ON  a.id = b.id
 LEFT JOIN sys.extended_properties d on d.major_id = b.id and d.minor_id = b.colid
INNER JOIN systypes e ON  e.xusertype = b.xusertype
 LEFT JOIN syscomments f ON  b.cdefault = f.id
ORDER BY a.name, b.colorder";
            try
            {
                using (var reader = SqlHelper.ExecuteReader(constr, sqlIdx))
                {
                    while (reader.Read())
                    {
                        var name = Convert.ToString(reader["tablename"]);
                        Dictionary<string, string> tbCols;
                        if (!ret.TryGetValue(name, out tbCols))
                        {
                            tbCols = new Dictionary<string, string>();
                            ret[name] = tbCols;
                        }

                        var colName = Convert.ToString(reader["column_name"]);
                        var collen = Convert.ToString(reader["char_length"]);
                        var colScale = Convert.ToString(reader["data_scale"]).Trim();
                        if (colScale.Length > 0)
                        {
                            collen += "," + colScale;
                        }
                        else if(collen == "-1")
                        {
                            collen = "max";
                        }

                        var colNull = Convert.ToString(reader["isnullable"]).Trim() == "0" ? " NOT NULL" : "";
                        var colDefault = Convert.ToString(reader["column_default"]).Trim();
                        if (colDefault.Length > 0)
                        {
                            colDefault = " DEFAULT " + colDefault;
                        }
                        var colcomments = Convert.ToString(reader["comments"]).Trim();
                        if (colcomments.Length > 0)
                        {
                            colcomments = " comment " + colDefault;
                        }
                        var colAtt = Convert.ToString(reader["data_type"]) + "(" + collen + ")" + colNull +
                                     colDefault + colcomments;
                        tbCols[colName] = colAtt;
                    }
                }
                return ret;
            }
            catch (Exception)
            {
                return ret;
            }
        }

        static Dictionary<string, Dictionary<string, string>> GetAllIndex(string constr)
        {
            var ret = new Dictionary<string, Dictionary<string, string>>();
            // 查询所有索引的sql
            var sqlIdx = @"select tb.name tablename, idx.name idxname, tp.name coltype,
       idxcol.index_column_id colIdx, col.name colname, idx.type_desc
 from sys.columns col, sys.index_columns idxcol,sys.indexes idx,sys.tables tb,sys.types tp
where col.column_id=idxcol.column_id and idxcol.index_id=idx.index_id 
  and idx.object_id=tb.object_id and idxcol.object_id=idx.object_id
  and col.object_id=idx.object_id and tp.user_type_id=col.user_type_id
order by tb.name, idx.name, idxcol.index_column_id";
            try
            {
                using (var reader = SqlHelper.ExecuteReader(constr, sqlIdx))
                {
                    while (reader.Read())
                    {
                        var name = Convert.ToString(reader["tablename"]);
                        Dictionary<string, string> tbIdx;
                        if (!ret.TryGetValue(name, out tbIdx))
                        {
                            tbIdx = new Dictionary<string, string>();
                            ret[name] = tbIdx;
                        }

                        var idxName = Convert.ToString(reader["idxname"]);
                        string idxStr;
                        if (tbIdx.TryGetValue(idxName, out idxStr))
                        {
                            idxStr += ", ";
                        }
                        else
                        {
                            idxStr = Convert.ToString(reader["type_desc"]) == "CLUSTERED" ? "PK:" : "";
                        }
                        idxStr += Convert.ToString(reader["colname"]);
                        tbIdx[idxName] = idxStr;
                    }
                }
                return ret;
            }
            catch (Exception)
            {
                return ret;
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
