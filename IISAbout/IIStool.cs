using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Diagnostics;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading;
using System.Windows.Forms;

namespace Beinet.cn.Tools.IISAbout
{
    public partial class IIStool : Form
    {
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

            // 初始化IIS树的右键菜单
            InitContextMenuStripIIS();
        }

        #region IIS操作相关方法
        // 停止或重启站点的等待时间
        private const int _stopWaitSecond = 30;
        

        private IISOperation _operation { get; set; }

        // 收集所有站点列表
        private Dictionary<string, Site> _arrSites;
        // 收集所有右键菜单操作方法
        Dictionary<string, Action<object>> _arrMenuActions;


        #region 事件方法集

        private void btnListSite_Click(object sender, EventArgs e)
        {
            var root = treeIISSite.TopNode;
            root.Nodes.Clear();

            _operation = new IISOperation(txtIISIP.Text);
            var sites = _operation.ListSite();
            _arrSites = sites.ToDictionary(item => item.Name);
            foreach (var site in sites)
            {
                var node = new TreeNode(site.Name);
                node.ImageIndex = site.State == 1 ? 0 : 1;
                node.SelectedImageIndex = node.ImageIndex;
                root.Nodes.Add(node);
            }
            root.ExpandAll();
        }

        private void treeIISSite_AfterSelect(object sender, TreeViewEventArgs e)
        {
            if (sender != treeIISSite || treeIISSite.SelectedNode == null)
            {
                return;
            }
            ClearSiteTxt();
            var node = treeIISSite.SelectedNode;
            switch (node.Level)
            {
                case 1:
                    ShowSite(node.Text);
                    break;
            }
        }


        private void treeIISSite_MouseDown(object sender, MouseEventArgs e)
        {
            if (e.Button == MouseButtons.Right)
            {
                var clickPoint = new Point(e.X, e.Y);
                var node = treeIISSite.GetNodeAt(clickPoint);
                if (node != null && node.Level == 1)
                {
                    node.ContextMenuStrip = contextMenuStripIIS;
                    treeIISSite.SelectedNode = node;
                }
            }
        }


        // 右键菜单点击事件
        private void contextMenuStripIIS_ItemClicked(object sender, ToolStripItemClickedEventArgs e)
        {
            if (_arrMenuActions.TryGetValue(e.ClickedItem.Text, out var action))
            {
                action(treeIISSite.SelectedNode.Text);
            }
        }

        private void btnRefreshIIS_Click(object sender, EventArgs e)
        {
            OperationSite(-1);
        }

        private void btnRestartIIS_Click(object sender, EventArgs e)
        {
            OperationSite(1);
        }

        private void btnStopIIS_Click(object sender, EventArgs e)
        {
            OperationSite(2);
        }
 
        private void btnCopyIIS_Click(object sender, EventArgs e)
        {
            OperationSite(0);
        }


        private void labLogDir_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        {
            // %SystemDrive%环境变量转换
            var dir = Environment.ExpandEnvironmentVariables(labLogDir.Text);
            if (!Directory.Exists(dir))
            {
                Alert("目录不存在：" + dir);
                return;
            }
            Process.Start("explorer", dir);
        }
       
        private void btnNewSite_Click(object sender, EventArgs e)
        {
            if (_operation == null)
            {
                Alert("请先连接服务器");
                return;
            }
            var name = txtSiteName.Text.Trim();
            if (name.Length == 0)
            {
                Alert("请输入网站名");
                return;
            }
            var poolName = txtSitePoolName.Text.Trim();
            var dir = txtSiteDir.Text.Trim();
            dir = Environment.ExpandEnvironmentVariables(dir);
            if (!Directory.Exists(dir))
            {
                if (!Confirm(dir + "目录不存在，是否创建?"))
                {
                    return;
                }
                try
                {
                    Directory.CreateDirectory(dir);
                }
                catch (Exception exp)
                {
                    Alert(dir + "目录创建失败：" + exp.Message);
                    return;
                }
            }
            var binding = txtSiteBind.Text.Trim();
            var preload = lstPreView.Text == "启用";
            if (!int.TryParse(txtTimeout.Text, out var timeout))
            {
                timeout = 10;
            }
            var ret = _operation.CreateSite(name, dir, binding, poolName, preload, timeout, txtGcTime.Text);
            Alert(ret);
        }

        private void btnUpdate_Click(object sender, EventArgs e)
        {
            if (_operation == null)
            {
                Alert("请先连接服务器");
                return;
            }
            var name = txtSiteName.Text.Trim();
            if (name.Length == 0)
            {
                Alert("请输入网站名");
                return;
            }
            var poolName = txtSitePoolName.Text.Trim();
            var dir = txtSiteDir.Text.Trim();
            dir = Environment.ExpandEnvironmentVariables(dir);
            if (!Directory.Exists(dir))
            {
                if (!Confirm(dir + "目录不存在，是否创建?"))
                {
                    return;
                }
                try
                {
                    Directory.CreateDirectory(dir);
                }
                catch (Exception exp)
                {
                    Alert(dir + "目录创建失败：" + exp.Message);
                    return;
                }
            }
            var binding = txtSiteBind.Text.Trim();
            var preload = lstPreView.Text == "启用";
            if (!int.TryParse(txtTimeout.Text, out var timeout))
            {
                timeout = 10;
            }
            var ret = _operation.UpdateSite(name, dir, binding, poolName, preload, timeout, txtGcTime.Text);
            Alert(ret);
        }

        private void btnReset_Click(object sender, EventArgs e)
        {
            if (MessageBox.Show("确认要重启本机的IIS？", "危险操作",
                    MessageBoxButtons.OKCancel, MessageBoxIcon.Error, MessageBoxDefaultButton.Button2)
                == DialogResult.OK)
            {
                Process.Start("IISReset");
            }
        }
        private void txtSiteName_KeyUp(object sender, KeyEventArgs e)
        {
            var name = txtSiteName.Text.Trim();
            if (name.Length <= 0)
                return;
            txtSitePoolName.Text = name;
            txtSiteDir.Text = "d:\\wwwroot\\" + name;
            txtSiteBind.Text = "http:*:80:" + name;
        }


        private void btnCopyAll_Click(object sender, EventArgs e)
        {
            if (_arrSites == null)
            {
                Alert("你还 没连接服务器呢");
                return;
            }
            var sb = new StringBuilder();
            foreach (var site in _arrSites)
            {
                sb.AppendFormat("{0}\r\n", site.Value);
            }
            Clipboard.SetText(sb.ToString());
            Alert("已复制到剪贴板");
        }

        private void btnDelAllGc_Click(object sender, EventArgs e)
        {
            if (_operation == null)
            {
                Alert("请先连接服务器");
                return;
            }
            if (!Confirm("将删除所有程序池的特定时间回收配置，此操作无法恢复，确认吗？？？"))
            {
                return;
            }
            var ret = _operation.RemovePoolGc();
            Alert("下列程序池已取消特定时间回收配置：\n" + ret);
        }
        
        private void btnStopAll_Click(object sender, EventArgs e)
        {
            if (_operation == null)
            {
                Alert("请先连接服务器");
                return;
            }

            if (!Confirm("确认要停止所有站点和程序池吗？？？"))
            {
                return;
            }
            var begin = DateTime.Now;
            var ret = _operation.StopSite(false);
            if (ret.Length == 0)
            {
                Alert("全部停止完成", begin);
            }
            else
            {
                Alert("下列站点或程序池停止失败：\n" + ret, begin);
            }
        }


        private void btnRestartAll_Click(object sender, EventArgs e)
        {
            if (_operation == null)
            {
                Alert("请先连接服务器");
                return;
            }

            if (!Confirm("确认要重启所有站点和程序池吗？？？\n注：已停止站点也会启动"))
            {
                return;
            }
            var begin = DateTime.Now;
            var ret = _operation.StopSite(true);
            if (ret.Length == 0)
            {
                Alert("全部重启完成", begin);
            }
            else
            {
                Alert("下列站点或程序池重启失败：\n" + ret, begin);
            }
        }



        private void btnEnableAllPreload_Click(object sender, EventArgs e)
        {
            if (_operation == null)
            {
                Alert("请先连接服务器");
                return;
            }

            if (!Confirm("确认要开启所有站点的预加载吗？？？\n注：站点启动时会自动执行Application_Start"))
            {
                return;
            }
            var begin = DateTime.Now;
            var ret = _operation.StartSitePreload();
            if (ret.Length == 0)
            {
                Alert("全部开启完成", begin);
            }
            else
            {
                Alert("下列站点开启失败：\n" + ret, begin);
            }
        }
        #endregion



        #region 非事件方法集

        Site FindSite(string siteName)
        {
            if (_arrSites == null || !_arrSites.TryGetValue(siteName, out var site))
                return null;
            return site;
        }

        void ShowSite(string siteName)
        {
            var site = FindSite(siteName);
            SetSiteTxt(site);
        }

        void ClearSiteTxt()
        {
            txtSiteBind.Text = "";
            txtSiteDir.Text = "";
            labSiteId.Text = "";
            txtSiteName.Text = "";
            txtSitePoolName.Text = "";
            labSiteStatus.Text = "";
            labLogDir.Text = "";
            txtTimeout.Text = "";
            txtGcTime.Text = "";
            lstPreView.Text = "禁用";
        }

        void SetSiteTxt(Site site)
        {
            if (site == null)
                return;
            txtSiteBind.Text = site.Bindings;
            txtSiteDir.Text = site.Dir;
            labSiteId.Text = site.Id.ToString();
            txtSiteName.Text = site.Name;
            txtSitePoolName.Text = site.PoolName;
            labSiteStatus.Text = site.StateText;
            labLogDir.Text = site.LogDir;
            txtTimeout.Text = site.ConnectionTimeOut.ToString();
            txtGcTime.Text = site.GcTimeStr;
            lstPreView.Text = site.Preload ? "启用" : "禁用";
        }
        void InitContextMenuStripIIS()
        {
            _arrMenuActions = new Dictionary<string, Action<object>>
            {
                {"复制配置到剪切板", CopySite},
                {"停止站点和应用程序池", StopSite},
                {"重启站点和应用程序池", RestartSite}
            };

            foreach (var item in _arrMenuActions)
            {
                contextMenuStripIIS.Items.Add(item.Key);
            }
        }

        // 复制站点配置
        void CopySite(object data)
        {
            var siteName = data.ToString();
            var site = FindSite(siteName);
            if (site == null)
            {
                Alert("未找到站点:" + siteName);
                return;
            }
            Clipboard.SetText(site.ToString());
        }
        // 重启站点和应用程序池
        void RestartSite(object data)
        {
            var begin = DateTime.Now;
            var siteName = data.ToString();
            var site = FindSite(siteName);
            if (site == null)
            {
                Alert("未找到站点:" + siteName);
                return;
            }
            var restartSecond = _stopWaitSecond;
            var ret = _operation.RestartSite(site.Name, restartSecond);
            if (ret != 0)
            {
                if (ret == 1)
                {
                    Alert("站点停止失败:" + siteName, begin);
                }
                else if (ret == 2)
                {
                    Alert("站点启动失败:" + siteName, begin);
                }
                else
                {
                    Alert("站点其它失败:" + siteName, begin);
                }
                return;
            }
            ret = _operation.RestartPool(site.PoolName, restartSecond);
            if (ret != 0)
            {
                if (ret == 1)
                {
                    Alert("程序池停止失败:" + siteName, begin);
                }
                else if (ret == 2)
                {
                    Alert("程序池启动失败:" + siteName, begin);
                }
                else
                {
                    Alert("程序池其它失败:" + siteName, begin);
                }
                return;
            }
            Alert("站点和程序池重启成功:" + siteName, begin);
        }
        // 重启站点和应用程序池
        void StopSite(object data)
        {
            var begin = DateTime.Now;
            var siteName = data.ToString();
            var site = FindSite(siteName);
            if (site == null)
            {
                Alert("未找到站点:" + siteName);
                return;
            }
            var restartSecond = _stopWaitSecond;
            var ret = _operation.StopSite(site.Name, restartSecond);
            if (!ret)
            {
                Alert("站点停止失败:" + siteName, begin);
                return;
            }

            ret = _operation.StopPool(site.PoolName, restartSecond);
            if (!ret)
            {
                Alert("程序池停止失败:" + siteName, begin);
                return;
            }

            Alert("站点和程序池停止成功: " + siteName, begin);
        }

        void OperationSite(int flg)
        {
            var siteName = txtSiteName.Text.Trim();
            if (siteName.Length <= 0)
            {
                Alert("要先选择站点");
                return;
            }
            var site = FindSite(siteName);
            if (site == null)
            {
                Alert("站点未找到：" + siteName);
                return;
            }
            switch (flg)
            {
                case 0:
                    CopySite(siteName);
                    return;
                case 1:
                    RestartSite(siteName);
                    break;
                case 2:
                    StopSite(siteName);
                    break;
            }
            // 刷新
            RefreshSiteFromServer(siteName);
        }

        void RefreshSiteFromServer(string siteName)
        {
            var site = _operation.FindSite(siteName);
            _arrSites[siteName] = site;
            SetSiteTxt(site);
        }



        #endregion



        #endregion


        #region IIS日志导入相关属性方法

        private const string IDLE_TXT = "启动导入";
        private const string WORK_TXT = "导入中...";


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
                Alert("导入中，请稍候");
                return;
            }
            string logfile = txtLogFile.Text.Trim();
            if (logfile == string.Empty) // || !File.Exists(logfile))
            {
                Alert("请选择正确的IIS日志文件");
                return;
            }
            //if (!IsTextFile(logfile))
            //{
            //    Alert("请选择文本格式的文件");
            //    return;
            //}

            string constr = txtDbTarget.Text.Trim();
            if (constr == string.Empty)
            {
                Alert("请输入数据库连接字符串");
                return;
            }
            string tbname = txtTbName.Text.Trim();
            if (tbname == string.Empty)
            {
                Alert("请输入新表表名");
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
                    Alert("导入错误：" + exp.Message);
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
                insertNum += ImportPerLog(file, constr, tbname, add8, txt, dt, ref isTbAdded);
            }

            var totalEnd = DateTime.Now;
            var totalTime = (totalEnd - totalBegin).TotalSeconds;
            var msg2 = totalEnd.ToString("HH:mm:ss.fff") + " " + insertNum.ToString() +
                       "行完成，总过程耗时:" + totalTime.ToString("N2") + "\r\n" + txt.Text;
            Utility.InvokeControl(txt, () => txt.Text = msg2);
        }

        static int ImportPerLog(string logfile, string constr, string tbname, bool add8, TextBox txt, DataTable dt,
            ref bool isTbAdded)
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
                            Alert("字段个数少于2个：" + line);
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
                    if (add8 && __dateColIdx >= 0 && __timeColIdx >= 0)
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
            var sql =
                @"select top 100 count(1) [次数],avg([time-taken]) [平均毫秒],sum([time-taken]) [累计毫秒],[cs-method],[cs-uri-stem],[cs-uri-query]
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

        #endregion


        static void Alert(string msg, DateTime? beginTime = null)
        {
            if (beginTime != null)
            {
                var time = (DateTime.Now - beginTime.Value).TotalSeconds.ToString("N2");
                msg += " 耗时:" + time + "秒";
            }
            MessageBox.Show(msg);
        }

        static bool Confirm(string msg)
        {
            var ret = MessageBox.Show(msg, "", MessageBoxButtons.YesNo, MessageBoxIcon.Warning,
                MessageBoxDefaultButton.Button2);
            return ret == DialogResult.Yes;
        }
    }
}
