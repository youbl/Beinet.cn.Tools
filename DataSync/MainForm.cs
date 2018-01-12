using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Data.OleDb;
using System.Data.SqlClient;
using System.Diagnostics;
using System.Drawing;
using System.IO;
using System.Runtime.Serialization;
using System.Text;
using System.Threading;
using System.Windows.Forms;
using System.Xml;

namespace Beinet.cn.Tools.DataSync
{
    public partial class MainForm : Form
    {
        // 总列数
        private const int COL_COUNT = 6;
        // 源表列号
        private const int COL_SOURCE = 0;
        // 目标表列号
        private const int COL_TARGET = 1;
        // 清空目标表列号，用于指定导入前是否要清空目标表数据
        private const int COL_TRUNCATE = 2;
        // 标识插入列号，用于指定导入是否要导入源表的标识字段
        private const int COL_IDENTIFIER = 3;

        // 源表备份列号，用于标识这行是否自定义查询
        private const int COL_SOURCEBACK = 4;
        // 目标表备份列号，用于输入目标表，又取消勾选，再勾选时的恢复
        private const int COL_TARGETBACK = 5;

        private string _defaultPath = null;//Path.Combine(System.AppDomain.CurrentDomain.BaseDirectory, "sync.xml");

        public MainForm()
        {
            InitializeComponent();

            // 用于后面可以给标题栏加复选框
            //lvTables.OwnerDraw = true;
        }


        private void Form_Load(object sender, EventArgs e)
        {
            txtDbTarget.Text = ConfigurationManager.AppSettings["DefalutConn"];
            txtDbSource.Text = txtDbTarget.Text;
        }

        private void lvTables_DrawColumnHeader(object sender, DrawListViewColumnHeaderEventArgs e)
        {
            //if (e.ColumnIndex == 0)
            //{
            //    TextFormatFlags flags = TextFormatFlags.LeftAndRightPadding;
            //    e.DrawBackground();

            //    //CheckBoxRenderer.DrawCheckBox(e.Graphics, ClientRectangle.Location, System.Windows.Forms.VisualStyles.CheckBoxState.UncheckedNormal); 
            //    CheckBoxRenderer.DrawCheckBox(e.Graphics, ClientRectangle.Location, Rectangle.Empty, "", this.Font,
            //                                  TextFormatFlags.HorizontalCenter, false, CheckBoxState.CheckedHot);

            //    e.DrawText(flags);
            //}
        }


        // 备份主调方法

        private void btnSyncBegin_Click(object sender, EventArgs e)
        {
            SyncTask task = GetTask();
            if (task == null)
                return;
            try
            {
                using (var sync = new SyncForm(task))
                {
                    sync.ShowDialog(this);
                }
            }
            catch(Exception exp)
            {
                MessageBox.Show(exp.ToString());
            }
        }

        // 保存为文件
        private void btnSaveConfig_Click(object sender, EventArgs e)
        {
            SyncTask task = GetTask();
            if (task == null)
                return;

            SaveFileDialog sfd = new SaveFileDialog();
            if (!string.IsNullOrEmpty(_defaultPath))
                sfd.InitialDirectory = _defaultPath;
            else
                sfd.InitialDirectory = System.AppDomain.CurrentDomain.BaseDirectory;
            sfd.FileName = "sync.xml";
            sfd.Filter = "Xml files (*.xml)|*.xml|All files (*.*)|*.*";
            var dialogRet = sfd.ShowDialog(this);
            if (dialogRet != DialogResult.OK)
                return;
            string configPath = sfd.FileName;
            try
            {
                // 加密连接字符串
                if (!string.IsNullOrEmpty(task.SourceConstr))
                    task.SourceConstr = Utility.TripleDES_Encrypt(task.SourceConstr, Encoding.UTF8);
                if (!string.IsNullOrEmpty(task.TargetConstr))
                    task.TargetConstr = Utility.TripleDES_Encrypt(task.TargetConstr, Encoding.UTF8);
                task.Encrypted = true;

                Utility.XmlSerialize(task, configPath);
                var diag = MessageBox.Show("成功保存到配置文件\r\n是否打开文件所在目录？", "打开目录", MessageBoxButtons.YesNo);
                if(diag == DialogResult.Yes)
                {
                    Process.Start("explorer.exe", @" /select," + configPath);
                }
            }
            catch(Exception exp)
            {
                MessageBox.Show("保存到文件" + configPath + "失败:\r\n" + exp);
            }
        }

        // 从文件加载
        private void btnLoad_Click(object sender, EventArgs e)
        {
            OpenFileDialog ofd = new OpenFileDialog();
            if (!string.IsNullOrEmpty(_defaultPath))
                ofd.InitialDirectory = _defaultPath;
            else
                ofd.InitialDirectory = System.AppDomain.CurrentDomain.BaseDirectory;
            ofd.Filter = "Xml files (*.xml)|*.xml|All files (*.*)|*.*";

            var dialogRet = ofd.ShowDialog(this);
            if (dialogRet != DialogResult.OK)
                return;

            string configPath = ofd.FileName;//Path.Combine(System.AppDomain.CurrentDomain.BaseDirectory, "sync.xml");
            _defaultPath = Path.GetDirectoryName(configPath);
            if (File.Exists(configPath))
            {
                #region 配置文件存在时，从配置文件加载数据
                lvTables.Items.Clear();

                SyncTask task;
                try
                {
                    string xmlStr;
                    // 兼容旧版本，需要替换旧版本的命名空间后再反序列化
                    using (var memoryStream = new StreamReader(configPath, Encoding.UTF8))
                    {
                        xmlStr = memoryStream.ReadToEnd();
                    }
                    xmlStr = xmlStr.Replace("/Check64dll.", "/Beinet.cn.Tools.");
                    task = Utility.XmlDeserializeFromStr<SyncTask>(xmlStr);
                    if (task != null && task.Encrypted)
                    {
                        if (!string.IsNullOrEmpty(task.SourceConstr))
                            task.SourceConstr = Utility.TripleDES_Decrypt(task.SourceConstr, Encoding.UTF8);
                        if (!string.IsNullOrEmpty(task.TargetConstr))
                            task.TargetConstr = Utility.TripleDES_Decrypt(task.TargetConstr, Encoding.UTF8);
                    }
                }
                catch (Exception exp)
                {
                    task = null;
                    MessageBox.Show("打开文件出错：" + exp.Message);
                }
                if (task != null)
                {
                    txtDbSource.Text = task.SourceConstr;
                    txtDbTarget.Text = task.TargetConstr;
                    chkErrContinue.Checked = task.ErrContinue;
                    chkWithNolock.Checked = task.AddNoLock;
                    chkUseTruncate.Checked = task.UseTruncate;
                    if (task.Items != null)
                    {
                        bool haverow = false;
                        foreach (SyncItem item in task.Items)
                        {
                            string[] values = new string[COL_COUNT];
                            values[COL_SOURCE] = item.Source;
                            values[COL_TARGET] = item.Target;
                            values[COL_TRUNCATE] = item.TruncateOld ? "true" : "false";
                            values[COL_IDENTIFIER] = item.UseIdentifier ? "true" : "false";
                            if (item.IsSqlSource)
                                values[COL_SOURCEBACK] = item.Target;

                            lvTables.Items.Add(new ListViewItem(values) { Checked = true });
                            haverow = true;
                        }
                        if (haverow)
                        {
                            btnSyncBegin.Enabled = true;
                            btnAddNewSql.Enabled = true;
                            btnDelRow.Enabled = true;
                            btnSaveConfig.Enabled = true;
                            chkAll.Checked = true;
                        }
                    }
                }
                lstTarget.Items.Add("请点击按钮：获取表结构");

                #endregion
            }
        }

        SyncTask GetTask()
        {
            string strSourceConn, strTargetConn;
            if (!CheckConn(out strSourceConn, out strTargetConn))
            {
                return null;
            }
            if (lvTables.CheckedItems.Count <= 0)
            {
                MessageBox.Show("请选择要同步的源表");
                return null;
            }
            var arr = new Dictionary<string, SyncItem>();
            foreach (ListViewItem item in lvTables.CheckedItems)
            {
                string target = item.SubItems[COL_TARGET].Text.Trim();
                string key = target.ToLower();
                if (arr.ContainsKey(key))
                {
                    MessageBox.Show(target + " 目标表选择重复，请重新指定");
                    return null;
                }
                string source = item.SubItems[COL_SOURCE].Text.Trim();

                arr.Add(key, new SyncItem
                {
                    Source = source,
                    Target = target,
                    IsSqlSource = !string.IsNullOrEmpty(item.SubItems[COL_SOURCEBACK].Text),
                    TruncateOld = item.SubItems[COL_TRUNCATE].Text == "true",
                    UseIdentifier = item.SubItems[COL_IDENTIFIER].Text == "true",
                });
            }
            var task = new SyncTask
            {
                Items = arr.Values,
                ErrContinue = chkErrContinue.Checked,
                AddNoLock = chkWithNolock.Checked,
                UseTruncate = chkUseTruncate.Checked,
                SourceConstr = strSourceConn,
                TargetConstr = strTargetConn,
            };
            return task;
        }


        private void chkAll_CheckedChanged(object sender, EventArgs e)
        {
            var check = (((CheckBox)sender).Checked);
            foreach (ListViewItem item in lvTables.Items)
            {
                // 会触发lvTables_ItemChecked事件
                item.Checked = check;
            }
        }




        private void btnGetSchma_Click(object sender, EventArgs e)
        {
            lstBoolean.Visible = false;
            lstTarget.Visible = false;

            string strSourceConn, strTargetConn;
            if (!CheckConn(out strSourceConn, out strTargetConn))
            {
                return;
            }
            try
            {
                // 绑定源数据库的所有表和视图到ListView
                BindTables(strSourceConn, lvTables, true);
                // 绑定目标数据库的所有表到ComboBox
                BindTables(strTargetConn, lstTarget, false);

                btnSyncBegin.Enabled = true;
                btnAddNewSql.Enabled = true;
                btnDelRow.Enabled = true;
                btnSaveConfig.Enabled = true;
            }
            catch(Exception exp)
            {
                MessageBox.Show("绑定出错:" + exp.Message);
            }
        }

        private void btnAddNewSql_Click(object sender, EventArgs e)
        {
            //MessageBox.Show("请输入自定义SQL", "", MessageBoxButtons.OKCancel, )
            string sql = string.Empty;
            using (Prompt prompt = new Prompt())
            {
                prompt.ShowDialog(this);
                if (prompt.IsOk && !prompt.IsDisposed)
                    sql = prompt.Sql;
            }
            if (string.IsNullOrEmpty(sql))
                return;

            // 源数据库增加自定义查询行
            string[] values = new string[COL_COUNT];
            values[COL_SOURCE] = sql;
            string tbName = "Query_" + Guid.NewGuid().GetHashCode().ToString();
            values[COL_SOURCEBACK] = tbName;
            values[COL_TARGET] = tbName;
            values[COL_TRUNCATE] = "false";
            values[COL_IDENTIFIER] = "false";

            var item = new ListViewItem(values) { Checked = true };
            lvTables.Items.Insert(0, item);
        }

        private void btnDelRow_Click(object sender, EventArgs e)
        {
            bool removed = false;
            foreach (ListViewItem item in lvTables.CheckedItems)
            {
                removed = true;
                lvTables.Items.Remove(item);
            }
            if (!removed)
                MessageBox.Show("请先选择行");
        }


        private void txtDbSource_TextChanged(object sender, EventArgs e)
        {
            //btnSyncBegin.Enabled = false;
            //btnAddNewSql.Enabled = false;
            //btnDelRow.Enabled = false;
        }

        private bool CheckConn(out string sourceConn, out string targetConn)
        {
            sourceConn = txtDbSource.Text.Trim();
            targetConn = txtDbTarget.Text.Trim();
            if (sourceConn == string.Empty || targetConn == string.Empty)
            {
                MessageBox.Show("请输入数据源或目标连接字符串");
                return false;
            }
            if (sourceConn.Equals(targetConn, StringComparison.OrdinalIgnoreCase))
            {
                if (DialogResult.Yes != MessageBox.Show("数据源与目标连接串相同，要继续吗？", "确认", MessageBoxButtons.YesNo))
                {
                    return false;
                }
            }
            return true;
        }

        private void lvTables_MouseUp(object sender, MouseEventArgs e)
        {
            ListView lv = lvTables;
            ComboBox lst;
            int x = e.X, y = e.Y;

            var item = lv.GetItemAt(x, y);
            if (item == null || !item.Checked)
                return;
            comboboxItem = item;

            int lWidth, rWidth;
            int col = GetMouseCol(lv, x, y, out lWidth, out rWidth);
            switch (col)
            {
                case COL_TARGET:
                    lst = lstTarget;
                    break;
                case COL_TRUNCATE:
                case COL_IDENTIFIER:
                    lst = lstBoolean;
                    break;
                default:
                    return;
            }

            //获取所在位置的行的Bounds            
            Rectangle rect = item.Bounds;
            //修改Rect的范围使其与第二列的单元格的大小相同，为了好看 ，左边缩进了2个单位                       
            rect.X += lv.Left + lWidth + 2;
            rect.Y += lv.Top + 2;
            rect.Width = rWidth - lWidth;
            lst.Bounds = rect;
            string val = item.SubItems[col].Text;
            // 目标表要设置默认值
            if (string.IsNullOrEmpty(val) && col == COL_TARGET)
                val = item.SubItems[COL_SOURCE].Text;
            lst.Text = val;
            lst.Visible = true;
            lst.BringToFront();
            lst.Focus();
            lst.Name = col.ToString();
        }

        private void lvTables_ItemChecked(object sender, ItemCheckedEventArgs e)
        {
            string targetTbName = e.Item.SubItems[COL_TARGET].Text;
            if (e.Item.Checked)
            {
                if (string.IsNullOrEmpty(targetTbName))
                    targetTbName = e.Item.SubItems[COL_TARGETBACK].Text;
                if (string.IsNullOrEmpty(targetTbName))
                    targetTbName = e.Item.SubItems[COL_SOURCE].Text;
                e.Item.SubItems[COL_TARGET].Text = targetTbName;
                if (string.IsNullOrEmpty(e.Item.SubItems[COL_IDENTIFIER].Text))
                    e.Item.SubItems[COL_IDENTIFIER].Text = "false";
                if (string.IsNullOrEmpty(e.Item.SubItems[COL_TRUNCATE].Text))
                    e.Item.SubItems[COL_TRUNCATE].Text = "false";
            }
            else
            {
                e.Item.SubItems[COL_TARGET].Text = string.Empty;
                e.Item.SubItems[COL_TARGETBACK].Text = targetTbName;
            }
        }

        void lstChange(object sender, EventArgs e)
        {
            ComboBox lst = (ComboBox)sender;

            int col;
            if (!lst.Visible || !int.TryParse(lst.Name, out col))
                return;
            if (comboboxItem != null)
            {
                string target = lst.Text.Trim();
                if (target != string.Empty)
                    comboboxItem.SubItems[col].Text = target;
            }
        }
        void lstLeave(object sender, EventArgs e)
        {
            ComboBox lst = sender as ComboBox;
            if (lst == null)
            {
                // 不是ComboBox时，表示是ListView的Scroll事件
                lstBoolean.Visible = false;
                lstTarget.Visible = false;
                return;
            }
            lstChange(sender, e);
            lst.Visible = false;
        }

        #region 静态方法集
        // 获取鼠标所在的列号
        private static int GetMouseCol(ListView lv, int x, int y, out int lWidth, out int rWidth)
        {
            lWidth = rWidth = 0;
            var item = lv.GetItemAt(x, y);
            if (item == null || !item.Checked)
                return -1;
            comboboxItem = item;

            for (int i = 0; i < item.SubItems.Count; i++)
            {
                int tmp = lv.Columns[i].Width;
                rWidth += tmp;
                if (x <= rWidth && x >= lWidth)
                    return i;

                lWidth = rWidth;
            }
            return -1;
        }

        private static void BindTables(string connstr, Control ctl, bool getViews)
        {
            ComboBox lst = ctl as ComboBox;
            if (lst != null)
                lst.Items.Clear();
            ListView lv = ctl as ListView;
            if (lv != null)
                lv.Items.Clear();

            string sql = "select name from sys.objects where type='U'";
            if (getViews)
                sql += " or type='V'";
            sql += " order by name";

            using (SqlDataReader reader = SqlHelper.ExecuteReader(connstr, sql, 30))
            {
                while (reader.Read())
                {
                    string name = Convert.ToString(reader[0]);
                    if (lst != null)
                        lst.Items.Add(name);
                    else if (lv != null)
                    {
                        string[] values = new string[COL_COUNT];
                        values[COL_SOURCE] = name;
                        //values[COL_TRUNCATE] = "false";
                        //values[COL_IDENTIFIER] = "false";
                        lv.Items.Add(new ListViewItem(values));
                    }
                }
            }
        }

        private static ListViewItem comboboxItem = null;
        /// <summary>
        /// 
        /// </summary>
        /// <param name="lv"></param>
        /// <param name="lst"></param>
        /// <param name="x">鼠标的x坐标</param>
        /// <param name="y">鼠标的y坐标</param>
        /// <param name="col">ComboBox显示在哪个列上</param>
        /// <param name="defaultValueCol">ComboBox的默认值列</param>
        private static void ShowCombobox(ListView lv, ComboBox lst, int x, int y, int col, int defaultValueCol)
        {
            var item = lv.GetItemAt(x, y);
            if (item == null || !item.Checked)
                return;
            comboboxItem = item;

            int lWidth = 0, rWidth = 0;
            for (int i = 0; i <= col; i++)
            {
                int tmp = lv.Columns[i].Width;
                if (i < col)
                    lWidth += tmp;
                rWidth += tmp;
            }

            if (x > rWidth || x < lWidth)
            {
                lst.Visible = false;
                return;
            }

            //获取所在位置的行的Bounds            
            Rectangle rect = item.Bounds;
            //修改Rect的范围使其与第二列的单元格的大小相同，为了好看 ，左边缩进了2个单位                       
            rect.X += lv.Left + lWidth + 2;
            rect.Y += lv.Top + 2;
            rect.Width = rWidth - lWidth;
            lst.Bounds = rect;
            string val = item.SubItems[col].Text;
            if (string.IsNullOrEmpty(val) && defaultValueCol >= 0 && item.SubItems.Count > defaultValueCol)
                val = item.SubItems[defaultValueCol].Text;
            lst.Text = val;
            lst.Visible = true;
            lst.BringToFront();
            lst.Focus();
            lst.Name = col.ToString();

            //lst.SelectedIndexChanged -= lstSelectedIndexChanged;// (obj, args) => { MessageBox.Show("1"); };
        }

        #endregion

        private void chkWithNolock_CheckedChanged(object sender, EventArgs e)
        {
            if (chkWithNolock.Checked)
            {
                MessageBox.Show("您选择了给表增加With(NoLock)选项，可能导致脏数据产生");
            }
        }

        private void chkAllClear_CheckedChanged(object sender, EventArgs e)
        {
            CheckAllCol(sender);
        }

        private void chkAllIdentify_CheckedChanged(object sender, EventArgs e)
        {
            CheckAllCol(sender);
        }

        private void CheckAllCol(object sender)
        {
            int colNum;
            if (sender == chkAllClear)
            {
                colNum = COL_TRUNCATE;
            }
            else if (sender == chkAllIdentify)
            {
                colNum = COL_IDENTIFIER;
            }
            else
            {
                return;
            }
            string val = ((CheckBox) sender).Checked ? "true" : "false";
            foreach (ListViewItem item in lvTables.CheckedItems)
            {
                item.SubItems[colNum].Text = val;
            }
        }

    }
}