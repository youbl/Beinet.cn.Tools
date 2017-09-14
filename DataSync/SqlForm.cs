using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
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
            if (e.KeyCode == Keys.Escape)
            {
                Hide();
                return;
            }
            if (e.KeyCode == Keys.F5 || e.KeyCode == Keys.F9)
            {
                RunSql();
            }
        }

        private void RunSql()
        {
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
                dataGridView1.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.AllCellsExceptHeader;
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

        protected override void OnFormClosing(FormClosingEventArgs e)
        {
            // 不关闭，只隐藏
            e.Cancel = true;
            this.Hide();
            // base.OnFormClosing(e);
        }
    }
}
