using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Drawing;
using System.IO;
using System.Text;
using System.Text.RegularExpressions;
using System.Windows.Forms;

namespace Beinet.cn.Tools.RegexTool
{
    public partial class MainForm : Form
    {
        private static string _version = "1.0 by youbl";
        private static readonly string _regFile = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "reg.txt");
        private const char SAVEREGSPLIT = 'ǒ';
        /// <summary>
        /// 构造函数
        /// </summary>
        public MainForm()
        {
            InitializeComponent();

            SetTextboxSelectAll(Controls);

            // 设置默认显示文字
            txt_Leave(txtReg, null);
            txt_Leave(txtOld, null);
            txt_Leave(txtResult, null);
            txt_Leave(txtReplace, null);

            // 不允许用户添加新行
            dgvResult.AllowUserToAddRows = false;
            //dgvResult.RowHeadersWidth = 400;
            //dgvResult.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.ColumnHeader;
            // 显示行号列自适应宽度
            dgvResult.RowHeadersWidthSizeMode = DataGridViewRowHeadersWidthSizeMode.AutoSizeToFirstHeader;
            //dgvResult.rowhea
            //txtOld.SelectionColor = Color.Red;

            // 不是焦点时，也允许突出显示选中项
            txtOld.HideSelection = false;
            txtOld.DetectUrls = false;
        }

        /// <summary>
        /// RichTextBox的AutoWordSelection设置，必须在OnLoad里设置才生效
        /// </summary>
        /// <param name="e"></param>
        protected override void OnLoad(EventArgs e)
        {
            base.OnLoad(e);
            Text += _version;
            
            txtReg.AutoWordSelection = false;
            txtReplace.AutoWordSelection = false;
            txtResult.AutoWordSelection = false;
            txtOld.AutoWordSelection = false;

            toolTip1.SetToolTip(chkIgnoreCase, "启用RegexOptions.IgnoreCase");
            toolTip1.SetToolTip(chkSingle, "启用RegexOptions.Singleline");
            toolTip1.SetToolTip(chkMultiLine, "启用RegexOptions.Multiline");
            toolTip1.SetToolTip(chkCompiled, "启用RegexOptions.Compiled"); 
            toolTip1.SetToolTip(chkComment, "启用RegexOptions.IgnorePatternWhitespace");

            toolTip1.SetToolTip(chkReplace, "用正则进行替换");
            toolTip1.SetToolTip(chkSplit, "用正则对要匹配的文本进行Split分割");

            // 添加常用正则
            AddCommonReg();

            // 添加常用正则
            AddSaveReg();
        }

        // 添加常用正则
        void AddCommonReg()
        {
            ToolStripMenuItem menu;
            //menu.Size = new System.Drawing.Size(152, 22);
            menu = new ToolStripMenuItem("替换为科学计数法");
            menuRegCommon.DropDownItems.Add(menu);
            menu.ToolTipText = ",";        // 用于标识替换文本
            menu.Tag = @"(?<=\d)(?=(?:\d{3})+(?!\d))";
            menu.Click += menuRegCommon_Click;

            menu = new ToolStripMenuItem("保留3位小数");
            menuRegCommon.DropDownItems.Add(menu);
            menu.ToolTipText = "$1";       // 用于标识替换文本
            menu.Tag = @"(\.\d\d(?>[1-9]?))\d+";
            menu.Click += menuRegCommon_Click;

            menu = new ToolStripMenuItem("双引号内容");
            menuRegCommon.DropDownItems.Add(menu);
            menu.Tag = @"(?>([^""]|\\.)*)";
            menu.Click += menuRegCommon_Click;

            menu = new ToolStripMenuItem("HTML标签");
            menuRegCommon.DropDownItems.Add(menu);
            menu.Tag = @"<(""[^""]*""|'[^']*'|[^'"">])*>";
            menu.Click += menuRegCommon_Click;

            menu = new ToolStripMenuItem("最内层DIV标签");
            menuRegCommon.DropDownItems.Add(menu);
            menu.Tag = @"<div[^>]*>(?:(?!<div).)*?</div>";
            menu.Click += menuRegCommon_Click;

            menu = new ToolStripMenuItem("最外层DIV标签");
            menuRegCommon.DropDownItems.Add(menu);
            menu.Tag = @"<div>((?<o><div>)|(?<-o></div>)|(?:(?!</?div)[\s\S]))*(?(o)(?!))</div>";
            menu.Click += menuRegCommon_Click;

            menu = new ToolStripMenuItem("A标签的href");
            menuRegCommon.DropDownItems.Add(menu);
            menu.Tag = @"<a\s[^>]*href=(['""])?(?(1)((?:(?!\1).)*)\1|([^\s>]*))";
            menu.Click += menuRegCommon_Click;

            menu = new ToolStripMenuItem("只能且必须字母数字组合6~20位");
            menuRegCommon.DropDownItems.Add(menu);
            menu.Tag = @"^(?=.*[A-Z])(?=.*[a-z])(?=.*[0-9])[a-zA-Z0-9]{6,20}$";
            menu.Click += menuRegCommon_Click;

            menu = new ToolStripMenuItem("只能且必须字母数字组合6~20位");
            menuRegCommon.DropDownItems.Add(menu);
            menu.Tag = @"^(?:([0-9])|([a-z])|([A-Z])){6,20}(?(1)|(?!))(?(2)|(?!))(?(3)|(?!))$";
            menu.Click += menuRegCommon_Click;

            menu = new ToolStripMenuItem("文件名1");
            menuRegCommon.DropDownItems.Add(menu);
            menu.Tag = @"^[a-zA-Z]:[\\/]+(?:[^\<\>\/\\\|\:""\*\?\r\n]+[\\/]+)*[^\<\>\/\\\|\:""\*\?\r\n]*$";
            menu.Click += menuRegCommon_Click;

            menu = new ToolStripMenuItem("文件名2");
            menuRegCommon.DropDownItems.Add(menu);
            menu.Tag = @"([A-Za-z]:)\\([^/:*<>""|\\]+\\)*[^/:*<>""|\\]+";
            menu.Click += menuRegCommon_Click;

            menu = new ToolStripMenuItem("Internet网址");
            menuRegCommon.DropDownItems.Add(menu);
            menu.Tag = @"(i:http|https)://([\w-]+\.)+[\w-]+(/[\w- ./%&=]*)";
            menu.Click += menuRegCommon_Click;

            menu = new ToolStripMenuItem("Email");
            menuRegCommon.DropDownItems.Add(menu);
            menu.Tag = @"\w+([-+.]\w+)*@\w+([-.]\w+)*\.\w+([-.]\w+)*";
            menu.Click += menuRegCommon_Click;

            menu = new ToolStripMenuItem("IP地址");
            menuRegCommon.DropDownItems.Add(menu);
            menu.Tag = @"((25[0-5]|2[0-4]\d|1\d\d|[1-9]\d|\d)\.){3}(25[0-5]|2[0-4]\d|1\d\d|[1-9]\d|\d)";
            menu.Click += menuRegCommon_Click;
        }

        // 添加保存的正则
        void AddSaveReg()
        {
            menuRegSave.DropDownItems.Clear();

            ToolStripMenuItem menu;
            if (File.Exists(_regFile))
            {
                using (var sr = new StreamReader(_regFile, Encoding.UTF8))
                {
                    while (!sr.EndOfStream)
                    {
                        string line = sr.ReadLine();
                        if (string.IsNullOrEmpty(line))
                            continue;
                        int idx = line.IndexOf(SAVEREGSPLIT);
                        if (idx < 1 || idx == line.Length - 1)
                            continue;
                        string name = line.Substring(0, idx);
                        string reg = line.Substring(idx + 1);
                        menu = new ToolStripMenuItem(name);
                        menuRegSave.DropDownItems.Add(menu);
                        menu.Tag = reg;
                        menu.Click += menuRegCommon_Click;
                    }
                }
            }
        }

        #region 字段与属性
        /// <summary>
        /// 记录上一次的正则，如果相同则不重新new Regex
        /// </summary>
        private string _oldreg;
        /// <summary>
        /// 保存生成的正则
        /// </summary>
        Regex _reg;
        /// <summary>
        /// 要使用的正则对象
        /// </summary>
        Regex RegObj
        {
            get
            {
                if (string.IsNullOrEmpty(txtReg.Text) || txtReg.Text == GetTxtToolTip(txtReg))
                {
                    MessageBox.Show("请输入正则");
                    return null;
                }
                //if (_reg == null || _oldreg == null || txtReg.Text != _oldreg)
                //{
                    _oldreg = txtReg.Text;
                    try
                    {
                        _reg = new Regex(_oldreg, GetRegOption);
                    }
                    catch (Exception exp)
                    {
                        MessageBox.Show("输入的正则有误，请确认，错误信息：" + exp);
                        return null;
                    }
                //}
                return _reg;
            }
        }
        /// <summary>
        /// 获取最近一次的匹配
        /// </summary>
        private Match Mat { get; set; }
        /// <summary>
        /// 根据用户选择，返回相应的正则选项
        /// </summary>
        RegexOptions GetRegOption
        {
            get
            {
                RegexOptions ret = RegexOptions.None;
                if (chkCompiled.Checked)
                    ret |= RegexOptions.Compiled;
                if (chkIgnoreCase.Checked)
                    ret |= RegexOptions.IgnoreCase;
                if (chkMultiLine.Checked)
                    ret |= RegexOptions.Multiline;
                if (chkSingle.Checked)
                    ret |= RegexOptions.Singleline;
                if (chkComment.Checked)
                    ret |= RegexOptions.IgnorePatternWhitespace;
                return ret;
            }
        }
        /// <summary>
        /// 当前模式是普通运行还是分组统计,分组时，DataGridView不执行CellClick事件
        /// </summary>
        private bool isGroupBy;
        /// <summary>
        /// 点击DataGridView时，是否可以选中源串中匹配的字符串
        /// false情况：未进行匹配时，原始文本修改了时，DataGridView不可见时
        /// </summary>
        private bool isMatchSelect;
        /// <summary>
        /// 环境改变时要重置这个值
        /// </summary>
        private int cntMatch = 0;
        #endregion

        
        #region 统一的文本框事件
        /// <summary>
        /// 失去焦点且文本框为空时，显示提示信息
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void txt_Leave(object sender, EventArgs e)
        {
            var txt = (TextBoxBase)sender;
            var tip = GetTxtToolTip(txt);

            if (string.IsNullOrEmpty(tip))
            {
                return;
            }
            if (!string.IsNullOrEmpty(txt.Text) && txt.Text != tip)
            {
                txt.ForeColor = Color.FromArgb(0);
                return;
            }
            txt.Text = tip;
            txt.ForeColor = Color.FromArgb(0xaabbbb);//0x808080
        }

        /// <summary>
        /// 获得焦点且文本框为提示信息时，清空文本框
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void txt_Enter(object sender, EventArgs e)
        {
            var txt = sender as TextBoxBase;
            if (txt == null)
                return;
            var tip = GetTxtToolTip(txt);

            if (string.IsNullOrEmpty(tip) || (!string.IsNullOrEmpty(txt.Text) && txt.Text != tip))
                return;
            txt.Text = string.Empty;
            txt.ForeColor = Color.FromArgb(0);
        }

        /// <summary>
        /// 实现文本框的全选
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void txt_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.Control && e.KeyCode == Keys.A && !e.Alt && !e.Shift)
            {
                ((TextBoxBase) sender).SelectAll();
                e.Handled = true;
                //e.SuppressKeyPress = true;
            }

            if (e.Control && e.KeyCode == Keys.V)
            {
                RichTextBox txt = sender as RichTextBox;
                if (txt != null)
                {
                    e.Handled = true;
                    //e.SuppressKeyPress = true;
                    txt.Paste(DataFormats.GetFormat(DataFormats.Text));
                }
            }

        }

        // 设置所有的TextBox的双击全选，和Ctrl+A全选
        void SetTextboxSelectAll(Control.ControlCollection controls)
        {
            if (controls == null || controls.Count == 0)
                return;
            foreach (Control control in controls)
            {
                TextBoxBase tb = control as TextBoxBase;
                if (tb != null)
                {
                    tb.DoubleClick += txt_DoubleClick;
                    tb.KeyDown += txt_KeyDown;
                }
                SetTextboxSelectAll(control.Controls);
            }
        }

        private void txt_DoubleClick(object sender, EventArgs e)
        {
            ((TextBoxBase)sender).SelectAll();
        }

        #endregion


        private void TextBoxChanged(object sender, EventArgs e)
        {
            isMatchSelect = false;
            EnvironmentChanged(sender, e);
            //txt_Leave(sender, null);
        }

        string GetTxtToolTip(object txt)
        {
            string tip;
            if (txt == txtReg)
                tip = @"（在这里输入正则表达式，如：\d+(\.\d+)? ）";
            else if (txt == txtOld)
                tip = @"（在这里输入用于匹配的原始文本 ）";
            else if (txt == txtResult)
                tip = @"（这里用于显示匹配结果 ）";
            else if (txt == txtReplace)
                tip = @"（在这里输入要替换成的字符串，如:$1 ）";
            else
                tip = string.Empty;
            return tip;
        }

        /// <summary>
        /// 进入与取消替换模式
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void chkReplace_CheckedChanged(object sender, EventArgs e)
        {
            EnvironmentChanged(sender, e);

            var chk = sender as CheckBox;
            if (chk == null)
                return;
            if (sender == chkReplace)
            {
                if (chk.Checked)
                {
                    chkSplit.Checked = false;// 替换与分割模式不能一起用

                    split2.Panel1.Visible = true;
                    split2.SplitterDistance = 50;
                    txtResult.Visible = true;
                    dgvResult.Visible = false;
                }
                else
                {
                    split2.Panel1.Visible = false;
                    split2.SplitterDistance = 0;
                    txtResult.Visible = false;
                    dgvResult.Visible = true;
                }
            }
            else //if (sender == chkSplit)
            {
                if (chk.Checked)
                {
                    chkReplace.Checked = false;// 替换与分割模式不能一起用

                    split2.Panel1.Visible = false;
                    split2.SplitterDistance = 0;
                    txtResult.Visible = false;
                    dgvResult.Visible = true;
                }
            }
        }

        /// <summary>
        /// 按F5执行
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void MainForm_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.F5)
            {
                btnExecute_Click(null, null);
            }
        }

        
        #region 主调方法，匹配和分组统计
        /// <summary>
        /// 逐一匹配
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnExeOne_Click(object sender, EventArgs e)
        {
            if (chkReplace.Checked)
            {
                btnExecute_Click(sender, e);
                return;
            }

            Stopwatch watch = new Stopwatch();
            watch.Start();

            var reg = RegObj;
            if (reg == null)
                return;

            if (cntMatch == 0)
            {
                Mat = reg.Match(txtOld.Text);
                if (!Mat.Success)
                {
                    MessageBox.Show("匹配失败，没有匹配结果");
                    return;
                }
                txtResult.Text = txtOld.Text;
            }
            else
            {
                Mat = Mat.NextMatch();
                if (!Mat.Success)
                {
                    cntMatch = 0;
                    MessageBox.Show("全部匹配完成");
                    return;
                }
            }
            cntMatch++;

            isGroupBy = false;
            isMatchSelect = true;

            #region 显示到DataGridView中
            var groups = reg.GetGroupNumbers();

            if (cntMatch <= 1)
            {
                dgvResult.Visible = true;
                dgvResult.Rows.Clear();
                dgvResult.Columns.Clear();

                #region 添加全部分组为列

                int i = 0;
                foreach (var gid in groups)
                {
                    string gname;
                    if (gid == 0)
                        gname = "整个匹配";
                    else
                    {
                        int tmp;
                        gname = reg.GroupNameFromNumber(gid);
                        if (!string.IsNullOrEmpty(gname) && !int.TryParse(gname, out tmp))
                            gname = "分组" + gid + "(" + gname + ")";
                        else
                            gname = "分组" + gid;
                    }
                    dgvResult.Columns.Add("group" + gid, gname);
                    if (gid == 0)
                        dgvResult.Columns[i++].Width = 300;
                    else
                        dgvResult.Columns[i++].Width = 100;
                }

                #endregion
            }

            //while (Mat.Success)
            {
                int colidx = 0;
                object[] rowVals = new object[groups.Length];
                foreach (var gid in groups)
                {
                    rowVals[colidx++] = Mat.Groups[gid].Value;
                }
                var newrow = dgvResult.Rows.Add(rowVals);
                // 保存当前匹配，用于获得位置信息
                dgvResult.Rows[newrow].Tag = Mat;
                //Mat = Mat.NextMatch();
            }
            #endregion

            watch.Stop();
            txtStatus.Text = dgvResult.Rows.Count + "个匹配, 耗时：" + watch.ElapsedMilliseconds + "毫秒";
        }
        /// <summary>
        /// 全部匹配
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnExecute_Click(object sender, EventArgs e)
        {
            Stopwatch watch = new Stopwatch();
            watch.Start();

            var reg = RegObj;
            if (reg == null)
                return;

            Mat = reg.Match(txtOld.Text);
            if (!Mat.Success)
            {
                MessageBox.Show("匹配失败，没有匹配结果");
                return;
            }
            if(chkReplace.Checked)
            {
                var replace = txtReplace.Text;
                if (txtReplace.Text == GetTxtToolTip(txtReplace))
                {
                    replace = string.Empty;
                }

                SetResult(reg.Replace(txtOld.Text, replace));
                watch.Stop();
                txtStatus.Text = "替换成功，耗时：" + watch.ElapsedMilliseconds + "毫秒";
                return;
            }

            // 初始化DataGridView
            dgvResult.Visible = true;
            dgvResult.Rows.Clear();
            dgvResult.Columns.Clear();
            if (chkSplit.Checked)
            {
                isGroupBy = false;
                isMatchSelect = false;
                var arr = reg.Split(txtOld.Text);

                #region 显示到DataGridView中
                dgvResult.Columns.Add("group0", "分割结果");
                dgvResult.Columns[0].AutoSizeMode = DataGridViewAutoSizeColumnMode.DisplayedCells;
                foreach (string s in arr)
                {
                    dgvResult.Rows.Add(s);
                }
                #endregion
            }
            else
            {
                isGroupBy = false;
                isMatchSelect = true;

                #region 显示到DataGridView中
                var groups = reg.GetGroupNumbers();


                #region 添加全部分组为列
                int i = 0;
                foreach (var gid in groups)
                {
                    string gname;
                    if (gid == 0)
                        gname = "整个匹配";
                    else
                    {
                        int tmp;
                        gname = reg.GroupNameFromNumber(gid);
                        if (!string.IsNullOrEmpty(gname) && !int.TryParse(gname, out tmp))
                            gname = "分组" + gid + "(" + gname + ")";
                        else
                            gname = "分组" + gid;
                    }
                    dgvResult.Columns.Add("group" + gid, gname);
                    if (gid == 0)
                        dgvResult.Columns[i++].Width = 300;
                    else
                        dgvResult.Columns[i++].Width = 100;
                }

                #endregion

                while (Mat.Success)
                {
                    i = 0;
                    object[] rowVals = new object[groups.Length];
                    foreach (var gid in groups)
                    {
                        rowVals[i++] = Mat.Groups[gid].Value;
                    }
                    var newrow = dgvResult.Rows.Add(rowVals);
                    // 保存当前匹配，用于获得位置信息
                    dgvResult.Rows[newrow].Tag = Mat;
                    Mat = Mat.NextMatch();
                }

                #endregion
            }
            watch.Stop();
            txtStatus.Text = dgvResult.Rows.Count + "个匹配, 耗时：" + watch.ElapsedMilliseconds + "毫秒";
        }

        /// <summary>
        /// 按正则里的分组进行统计
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnGroupBy_Click(object sender, EventArgs e)
        {
            Stopwatch watch = new Stopwatch();
            watch.Start();
            
            isGroupBy = true;
            isMatchSelect = false;

            var reg = RegObj;
            if (reg == null)
                return;

            List<int> groups = new List<int>();
            string spliter = string.Empty;
            string groupOption;
            var groupLen = reg.GetGroupNumbers().Length;
            // 没有分组时，按整个匹配分组
            if (sender == btnGroupBy1)
            {
                if (groupLen <= 1)
                {
                    MessageBox.Show("不存在分组1");
                    return;
                }
                groups.Add(1);
                groupOption = "分组1";
            }
            else if (sender == btnGroupBy2)
            {
                if (groupLen <= 2)
                {
                    MessageBox.Show("不存在分组2");
                    return;
                }
                groups.Add(2);
                groupOption = "分组2";
            }
            else if (sender == btnGroupBy3)
            {
                if (groupLen <= 3)
                {
                    MessageBox.Show("不存在分组3");
                    return;
                }
                groups.Add(3);
                groupOption = "分组3";
            }
            else if (groupLen == 1 || sender == btnGroupBy0)
            {
                groups.Add(0);
                groupOption = "整个匹配";
            }
            else
            {
                #region 弹出窗口选择分组项
                var opn = new OptionSelect(reg);
                if (opn.IsDisposed)
                    return;

                opn.ShowDialog(this);
                groupOption = opn.ret;
                if (string.IsNullOrEmpty(groupOption))
                    return;

                #endregion

                var mg = Regex.Match(groupOption, @"\d+");
                while(mg.Success)
                {
                    groups.Add(int.Parse(mg.Value));
                    mg = mg.NextMatch();
                }
                spliter = opn.spliter;
                groupOption = "分组" + groupOption + "(分隔符" + spliter + ")";
            }
            Mat = reg.Match(txtOld.Text);
            if(!Mat.Success)
            {
                MessageBox.Show("匹配失败，没有匹配结果");
                return;
            }
            chkReplace.Checked = false;
            var ret = new SortedList<string, int>();
            while (Mat.Success)
            {
                string val = string.Empty;
                foreach (int gp in groups)
                {
                    val += spliter + Mat.Groups[gp].Value;
                }
                if (val != string.Empty && !string.IsNullOrEmpty(spliter))
                    val = val.Substring(spliter.Length);// 移除最前的分隔符
                if (ret.ContainsKey(val))
                    ret[val]++;
                else
                    ret.Add(val, 1);
                Mat = Mat.NextMatch();
            }
            // SortedList不方便根据Value排序，改成KeyValuePair数组
            var ret2 = new KeyValuePair<string, int>[ret.Count];
            var i = 0;
            foreach (var pair in ret)
            {
                ret2[i++] = pair;
            }
            // 按统计倒序排列
            Array.Sort(ret2, (a, b) => -a.Value.CompareTo(b.Value));
            
            #region 显示到DataGridView中
            dgvResult.Visible = true;
            dgvResult.Rows.Clear();
            dgvResult.Columns.Clear();
            dgvResult.Columns.Add("group0", groupOption);
            dgvResult.Columns[0].AutoSizeMode = DataGridViewAutoSizeColumnMode.DisplayedCells;
            //dgvResult.Columns[0].Width = 750;
            dgvResult.Columns.Add("matchTime", "匹配次数");
            dgvResult.Columns[1].AutoSizeMode = DataGridViewAutoSizeColumnMode.DisplayedCells;
            //dgvResult.Columns[1].Width = 80;
            foreach (var pair in ret2)
            {
                dgvResult.Rows.Add(pair.Key, pair.Value);
            }
            #endregion

            watch.Stop();
            txtStatus.Text = dgvResult.Rows.Count + "个匹配, 耗时：" + watch.ElapsedMilliseconds + "毫秒";
        }
        #endregion

        void SetResult(string txt, Match mt = null, int length = 0)
        {
            txtResult.Visible = true;
            dgvResult.Visible = false;
            
            txt_Enter(txtResult, null);
            txtResult.Text = txt;
            if(mt!=null)
            {
                //mt.Value.Length;
                txtResult.Select(mt.Index, length);
            }
            txtResult.Focus();
        }

        /// <summary>
        /// 点击表格时，高亮相应的结果文本
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void dgvResult_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            if (isGroupBy || !isMatchSelect) return;

            var col = e.ColumnIndex;
            var row = e.RowIndex;
            if (row < 0) return;    // 点击标题了
            var dgvRow = dgvResult.Rows[row];
            var mat = dgvRow.Tag as Match;
            if (mat == null)
                return;
            int start, len;
            if(col > 0)
            {
               start = mat.Groups[col].Index;
               len = mat.Groups[col].Length;
            }
            else
            {
                start = mat.Index;
                len = mat.Length;
            }
            
            //txtOld.Focus();
            //txtOld.ForeColor = Color.Black;
            txtOld.Select(start, len);
            txtOld.ScrollToCaret();
        }

        #region 用于显示DataGridView的行号
        private void dgvResult_RowsAdded(object sender, DataGridViewRowsAddedEventArgs e)
        {
            var dgv = sender as DataGridView;
            if (dgv == null)
                return;
            for (int i = 0; i < e.RowCount; i++)
            {
                dgv.Rows[e.RowIndex + i].HeaderCell.Style.Alignment = DataGridViewContentAlignment.MiddleCenter;
                dgv.Rows[e.RowIndex + i].HeaderCell.Value = (e.RowIndex + i + 1).ToString();
            }
            for (int i = e.RowIndex + e.RowCount; i < dgv.Rows.Count; i++)
            {
                dgv.Rows[i].HeaderCell.Style.Alignment = DataGridViewContentAlignment.MiddleCenter;
                dgv.Rows[i].HeaderCell.Value = (i + 1).ToString();
            }
        }

        private void dataGridView1_RowsRemoved(object sender, DataGridViewRowsRemovedEventArgs e)
        {
            var dgv = sender as DataGridView;
            if (dgv == null)
                return;
            for (int i = 0; i < e.RowCount && i < dgv.Rows.Count; i++)
            {
                dgv.Rows[e.RowIndex + i].HeaderCell.Style.Alignment = DataGridViewContentAlignment.MiddleCenter;
                dgv.Rows[e.RowIndex + i].HeaderCell.Value = (e.RowIndex + i + 1).ToString();
            }
            for (int i = e.RowIndex + e.RowCount; i < dgv.Rows.Count; i++)
            {
                dgv.Rows[i].HeaderCell.Style.Alignment = DataGridViewContentAlignment.MiddleCenter;
                dgv.Rows[i].HeaderCell.Value = (i + 1).ToString();
            }
        }
        #endregion

        #region 菜单事件
        /// <summary>
        /// 复制粘贴相关菜单
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void menuReg_Click(object sender, EventArgs e)
        {
            // SHIFT用+，CTRL用^，ALT用%
            if (sender == menuRegCopy)
                SendKeys.Send("^C");
            else if (sender == menuRegCut)
                SendKeys.Send("^X");
            else if (sender == menuRegDel)
                SendKeys.Send("{DEL}");
            else if (sender == menuRegParse)
                SendKeys.Send("^V");
        }

        private void menuRegCommon_Click(object sender, EventArgs e)
        {
            var menu = sender as ToolStripMenuItem;
            if (menu == null || menu.Tag == null)
                return;
            txtReg.Text = menu.Tag.ToString();
            if(!string.IsNullOrEmpty(menu.ToolTipText))
            {
                chkReplace.Checked = true;
                txtReplace.Text = menu.ToolTipText;
            }
        }

        private void mnuSaveReg_Click(object sender, EventArgs e)
        {
            if (RegObj == null)
            {
                return;
            }

            //string name = Microsoft.VisualBasic.Interaction.InputBox(
            //    "请输入快捷方式名", "请输入快捷方式名", "", 100, 100);
            string name;
            if (!PromptWin.GetPrompt(out name, "请输入快捷方式名") || 
                name == null || (name = name.Trim()) == string.Empty)
            {
                return;
            }

            string save = txtReg.Text;
            using (var sw = new StreamWriter(_regFile, true, Encoding.UTF8))
            {
                sw.WriteLine(name + SAVEREGSPLIT + save);
            }
            // 重新绑定自定义正则
            AddSaveReg();
            MessageBox.Show("保存成功");
        }
        #endregion

        /// <summary>
        /// 环境变化，导致匹配要重新开始
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void EnvironmentChanged(object sender, EventArgs e)
        {
            cntMatch = 0;
        }

        private void dgvResult_ColumnHeaderMouseClick(object sender, DataGridViewCellMouseEventArgs e)
        {
            if (e.Button == MouseButtons.Right && e.ColumnIndex>=0)
            {
                //dgvResult.Columns[e.ColumnIndex].Selected = true;
                dgvResult.ClearSelection();
                foreach (DataGridViewRow row in dgvResult.Rows)
                {
                    row.Cells[e.ColumnIndex].Selected = true;
                }
            }
        }



    }
}
