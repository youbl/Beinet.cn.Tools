using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.IO;
using System.Net;
using System.Text;
using System.Text.RegularExpressions;
using System.Windows.Forms;
using System.Threading;

namespace Beinet.cn.Tools.LvsManager
{
    public partial class LVSControl : Form
    {
        private const int COLS = 8;

        //private const int COLIP = 0;
        /// <summary>
        /// 服务器域名列
        /// </summary>
        private const int COLNAME = 1;
        /// <summary>
        /// Web服务器状态列
        /// </summary>
        private const int COLLVS = 2;
        /// <summary>
        /// Web服务器是否运行中状态
        /// </summary>
        private const int COLSTATUS = 3;
        /// <summary>
        /// Nginx对这台机器的状态指示
        /// </summary>
        private const int COLNGINX = 4;
        /// <summary>
        /// Web服务器上次接收到的用户请求时间
        /// </summary>
        private const int COLLASTTIME = 5;
        /// <summary>
        /// Web服务器当前时间
        /// </summary>
        private const int COLNOW = 6;
        /// <summary>
        /// Web服务器返回的访问量
        /// </summary>
        private const int COLPV = 7;

        private List<Site> _sites;
        private List<Thread> _threads = new List<Thread>();

        private Color[] _colors = new Color[] { Color.FromArgb(0xE8EDCC), Color.FromArgb(0xE1CBF0) };

        public LVSControl()
        {
            InitializeComponent();
        }

        protected override void OnLoad(EventArgs e)
        {
            base.OnLoad(e);

            LoadList();
            // 如果有nginx的接口，可以开启这个方法获取
            LoopAndSetNginxState();
        }

        // 加载服务器列表
        private void LoadList()
        {
            lock (_threads)
            {
                // 终止并清空全部线程
                foreach (Thread thread in _threads)
                {
                    if (thread.IsAlive)
                    {
                        //thread.Abort();// 不直接Abort，避免卡死界面
                        Thread thread1 = thread;
                        ThreadPool.UnsafeQueueUserWorkItem(state => thread1.Abort(), null);
                    }
                }
                _threads.Clear();
            }

            var panel = flowLayoutPanel1;//splitContainer1.Panel2
            panel.Controls.Clear();
            lvServers.Items.Clear();

            _sites = Common.Sites;
            if (_sites.Count > 0)
            {
                int idx = 0;
                foreach (Site server in _sites)
                {
                    if (!string.IsNullOrEmpty(server.name))
                    {
                        CheckBox chk = new CheckBox();
                        panel.Controls.Add(chk);
                        chk.CheckedChanged += sites_CheckChanged;
                        chk.Text = server.name;
                        //chk.BackColor = Color.Red;
                        chk.Width = 180;
                        chk.Padding = new Padding(5, 0, 0, 0);
                        chk.Margin = new Padding(2, 0, 0, 0);
                        if (idx == 0)
                            chk.Checked = true;//触发sites_CheckChanged事件 
                        idx++;
                    }
                }
            }
         //   panel.AutoScroll = true;
        }

        // 勾选站点，开始对这个站点扫描
        private void sites_CheckChanged(object sender, EventArgs e)
        {
            var chk = sender as CheckBox;
            if (chk == null)
                return;
            if (chk.Checked)
            {
                var site = _sites.Find(item => item.name == chk.Text);
                if (site == null)
                    return;

                Color color = _colors[0];
                var cnt = lvServers.Items.Count;
                if (cnt > 0 && lvServers.Items[cnt - 1].BackColor == color)
                    color = _colors[1];

                #region 添加行，并启动相应线程
                foreach (string server in site.Servers)
                {
                    ListViewItem listItem = new ListViewItem();
                    listItem.ImageKey = "online";
                    for (int i = 0; i < COLS - 1; i++)
                        listItem.SubItems.Add(new ListViewItem.ListViewSubItem());

                    listItem.Text = server;
                    //listItem.SubItems[COLIP].Text = server;
                    listItem.SubItems[COLNAME].Text = site.name;
                    listItem.SubItems[COLLVS].Text = "获取中...";
                    listItem.SubItems[COLSTATUS].Text = "获取中...";
                    listItem.SubItems[COLNGINX].Text = "获取中...";
                    listItem.SubItems[COLLASTTIME].Text = "获取中...";
                    listItem.SubItems[COLNOW].Text = "获取中...";
                    listItem.SubItems[COLPV].Text = "0";
                    lvServers.Items.Add(listItem);
                    listItem.BackColor = color;

                    string serverTmp = server;
                    var listItemTmp = listItem;
                    ThreadPool.UnsafeQueueUserWorkItem(state =>
                    {
                        // 启动当前服务器扫描线程
                        Thread tdScan = new Thread(Scan) { IsBackground = true, Name = site.name };
                        lock (_threads)
                        {
                            _threads.Add(tdScan);
                        }
                        Thread.Sleep(50);
                        tdScan.Start(new object[] { site, serverTmp, listItemTmp });

                    }, null);
                }
                #endregion
            }
            else
            {
                #region 删除行并中止相应线程
                for (int i = lvServers.Items.Count - 1; i >= 0; i--)
                {
                    ListViewItem item = lvServers.Items[i];
                    // 比较域名
                    if (item.SubItems[COLNAME].Text == chk.Text)
                    {
                        lvServers.Items.RemoveAt(i);
                    }
                }
                //中止这些轮询的线程
                lock (_threads)
                {
                    for (int i = _threads.Count - 1; i >= 0; i--)
                    {
                        Thread thread = _threads[i];
                        if (thread.Name == chk.Text)
                        {
                            if (thread.IsAlive)
                            {
                                //thread.Abort();// 不直接Abort，避免卡死界面
                                Thread thread1 = thread;
                                ThreadPool.UnsafeQueueUserWorkItem(state => thread1.Abort(), null);
                            }
                            _threads.RemoveAt(i);
                        }
                    }
                }
                #endregion
            }

            ThreadPool.UnsafeQueueUserWorkItem(state =>
            {
                while (true)
                {
                    // ReSharper disable once InconsistentlySynchronizedField
                    Utility.SetValue(labStatus1, "Text", "扫描线程数量【" + _threads.Count.ToString() + "】个");
                    Thread.Sleep(1000);
                }
// ReSharper disable FunctionNeverReturns
            }, null);
// ReSharper restore FunctionNeverReturns
        }

        private void Scan(Object lvsInterfaceObj)
        {
            object[] arrPara = lvsInterfaceObj as object[];
            if (arrPara == null || arrPara.Length != 3)
                return;
            Site site = arrPara[0] as Site;
            if (site == null)
                return;
            ListViewItem listItem = arrPara[2] as ListViewItem;
            if (listItem == null)
                return;
            string serverIP = arrPara[1].ToString();

            CheckBox chk = null;
            foreach (var control in flowLayoutPanel1.Controls)
            {
                CheckBox chkTmp = control as CheckBox;
                if (chkTmp != null && chkTmp.Text == site.name)
                {
                    chk = chkTmp;
                    break;
                }
            }
            while (chk != null && chk.Checked)
            {
                try
                {
                    string html = DoGetPage(site.url + "?getrealstate=yes&format=robot", null, serverIP); 
                    string[] arr = html.Split('|');
                    if (arr.Length > 2)
                    {
                        DateTime lastRequestTime = Convert.ToDateTime(arr[0]);
                        DateTime curentServerTime = Convert.ToDateTime(arr[1]);
                        string lvsState = arr[2];
                        int pv;
                        if (arr.Length <= 3 || !int.TryParse(arr[3], out pv))
                            pv = 0;
                        SetListItemState(listItem, serverIP, lastRequestTime, curentServerTime, lvsState, pv.ToString());
                    }
                    else
                    {
                        SetListItemState(listItem, serverIP, DateTime.MinValue, null, "返回值无效", "");
                    }
                }
                catch (ThreadAbortException)
                {
                    return;
                }
                catch (Exception ex)
                {
                    SetListItemState(listItem, serverIP, DateTime.MinValue, null, ex.Message, "");
                    //if (!ex.Message.Contains("操作超时"))
                    //{
                    //    Utility.Log("Scan出错：" + site.name + " " + serverIP + Environment.NewLine + ex, "lvs");
                    //}
                }

                Thread.Sleep(TimeSpan.FromSeconds(site.resreshSecond));
            }
        }

        private void cmsServer_Opening(object sender, CancelEventArgs e)
        {
            if (lvServers.SelectedItems.Count == 0)
            {
                e.Cancel = true;
            }
        }

        private void PostCommandByMenu(string para)
        {
            if (lvServers.SelectedItems.Count > 0)
            {
                var selectedItem = lvServers.SelectedItems[0];
                Site site = FindSite(selectedItem);
                if (site == null)
                    return;
                string serverIP = selectedItem.Text;
                string url = site.url;
                if (url.IndexOf('?') > 0)
                    url += "&" + para;
                else
                    url += "?" + para;
                ThreadPool.UnsafeQueueUserWorkItem(state =>
                {
                    string str;
                    try
                    {
                        //var selectedItem = GetValue(lvServers, "SelectedItems") as
                        //    ListView.SelectedListViewItemCollection;
                        str = DoGetPage(url, null, serverIP);
                    }
                    catch (WebException webExp)
                    {
                        str = webExp.ToString();
                    }
                    catch (Exception ex)
                    {
                        str = ex.ToString();
                    }
                    webBrowser1.DocumentText = url + "<br/>\r\n" + str;
                }, null);
            }
        }

        private void 上架服务器ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            PostCommandByMenu("SetState=ONLINE&do=1");
        }

        private void 下架服务器ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (lvServers.SelectedItems.Count > 0)
            {
                var downitem = lvServers.SelectedItems[0];
                string serverIP = downitem.Text;
                string serverDomain = downitem.SubItems[COLNAME].Text;
                int upCnt = 0;
                foreach (ListViewItem item in lvServers.Items)
                {
                    if (item.SubItems[COLNAME].Text != serverDomain)
                    {
                        continue;
                    }
                    var ngStatus = item.SubItems[COLNGINX].Text;
                    // ng状态不等于down就ok，避免未配置的情况
                    if (item.SubItems[COLLVS].Text.Equals("ONLINE", StringComparison.OrdinalIgnoreCase) 
                        && !ngStatus.Equals("down", StringComparison.OrdinalIgnoreCase))
                    {
                        upCnt++;
                    }
                }
                if (upCnt <= 1)
                {
                    MessageBox.Show("【" + serverDomain + "】只有一台服务器在线，不允许下线");
                    return;
                }
                if (MessageBox.Show(this, "是否确认将服务器【" + serverIP + "】下架", "确认下架", 
                    MessageBoxButtons.OKCancel) == System.Windows.Forms.DialogResult.OK)
                {
                    PostCommandByMenu("SetState=OFFLINE&do=1");
                }
            }
        }

        private void 查看详细状态ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            PostCommandByMenu("getrealstate=yes");
        }

        private void 查看客户端请求的IPToolStripMenuItem_Click(object sender, EventArgs e)
        {
            PostCommandByMenu("getuserip=yes");
        }

        private void lvServers_SelectedIndexChanged(object sender, EventArgs e)
        {
            btnLookState.Enabled = lvServers.SelectedItems.Count > 0;
            btnSetOnline.Enabled = lvServers.SelectedItems.Count > 0;
            btnSetOffline.Enabled = lvServers.SelectedItems.Count > 0;
        }

        private void btnSettings_Click(object sender, EventArgs e)
        {
            DialogResult ret;
            using (var configset = new ConfigSet())
            {
                ret = configset.ShowDialog(this);
            }
            if(ret == DialogResult.OK)
                // 有修改才重新加载
                LoadList();
        }



        private void SetListItemState(ListViewItem listItem, string serverIP,
            DateTime lastRequestTime, DateTime? curentServerTime, string lvsState, string pv)
        {
            Utility.InvokeControl(lvServers, () =>
            {
                labStatus2.Text = "#[" + DateTime.Now + "]:扫描[" + serverIP + "]完成";
                ListViewItem item = listItem;
                Site site = FindSite(listItem);
                if (site == null)
                    return;
                if (item.Text.Equals(serverIP) && item.SubItems[COLNAME].Text == site.name)
                {
                    item.ImageKey = lvsState.Equals("online", StringComparison.CurrentCultureIgnoreCase) ? "online" : "offline";
                    item.SubItems[COLLVS].Text = lvsState;
                    item.SubItems[COLLASTTIME].Text = curentServerTime == null ?
                        "获取中.." : lastRequestTime.ToString("yyyy-MM-dd HH:mm:ss");
                    item.SubItems[COLNOW].Text = curentServerTime == null ?
                        "获取中.." : curentServerTime.Value.ToString("yyyy-MM-dd HH:mm:ss");
                    item.SubItems[COLPV].Text = pv;
                    var realStatusItem = item.SubItems[COLSTATUS];
                    if (curentServerTime == null)
                    {
                        realStatusItem.Text = "获取中..";
                        return;
                    }
                    if (lvsState.Equals("online", StringComparison.CurrentCultureIgnoreCase))
                    {
                        if ((curentServerTime.Value - lastRequestTime).TotalSeconds < site.offlineSecond)
                        {
                            realStatusItem.Text = "运行中";
                            item.ForeColor = Color.Green;
                        }
                        else
                        {
                            realStatusItem.Text = "无流量";
                            item.ForeColor = Color.Red;
                        }
                    }
                    else
                    {
                        if ((curentServerTime.Value - lastRequestTime).TotalSeconds < site.offlineSecond)
                        {
                            realStatusItem.Text = "正在下架中...";
                            item.ForeColor = Color.Blue;
                        }
                        else
                        {
                            realStatusItem.Text = "已下架";
                            item.ForeColor = Color.Red;
                        }
                    }
                    //ms framework的bug,必须重新设置一下Text才能触发subitem的界面更新。该bug和机器有关
                    //http://social.msdn.microsoft.com/Forums/en-US/winforms/thread/3e312c15-a470-4d65-98b6-c2e658b35454
                    item.Text = serverIP;
                }
            });
        }


        /// <summary>
        /// 全局轮询Nginx状态
        /// </summary>
        private void LoopAndSetNginxState()
        {
            ThreadPool.UnsafeQueueUserWorkItem(state =>
            {
                var ngurl = "http://www.chidaoni.com/check_status?format=json";
                var ipmap = InOutIpMap;
                while (true)
                {
                    Thread.Sleep(3000);
                    string html = null;
                    try
                    {
                        html = DoGetPage(ngurl);
                        //var obj = Utility.FromJson<object>(html);
                    }
                    catch (Exception exp)
                    {
                        webBrowser1.DocumentText = ngurl + " 网络出错：\r\n<br/>" + exp;
                    }
                    try
                    {
                        if (!string.IsNullOrEmpty(html))
                        {
                            var statuses = MatchNginxRows(html);
                            SetGridStatus(lvServers, statuses, ipmap);
                        }
                    }
                    catch (Exception exp)
                    {
                        webBrowser1.DocumentText = "解析出错：\r\n<br/>" + exp + "\r\n<br/>" + html;
                    }
                }
                // ReSharper disable once FunctionNeverReturns
            }, null);
        }

        private static Dictionary<string, string> InOutIpMap = LoadIpMap();
        /// <summary>
        /// 加载内外网ip映射关系
        /// </summary>
        /// <returns></returns>
        static Dictionary<string, string> LoadIpMap()
        {
            var ret = new Dictionary<string, string>();
            try
            {
                string mapFile = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "ipmap.txt");
                if (!File.Exists(mapFile))
                {
                    return ret;
                }
                using (var sr = new StreamReader(mapFile, Encoding.UTF8))
                {
                    while (!sr.EndOfStream)
                    {
                        var arrline = (sr.ReadLine() ?? "").Trim().Split(',', ';');
                        if (arrline.Length > 2)
                        {
                            var ip1 = arrline[1].Trim();
                            var ip2 = arrline[2].Trim();
                            ret[ip1] = ip2;
                            ret[ip2] = ip1;
                        }
                    }
                }
            }
            // ReSharper disable once EmptyGeneralCatchClause
            catch (Exception)
            { }
            return ret;
        }

        // 匹配整行和域名
        static Regex regRow = new Regex(@"\{[^\{\}]+""upstream""\s*:\s*""([^""]+)""[^\{\}]+\}", RegexOptions.Compiled);
        // 匹配单行里的ip字段
        static Regex regName = new Regex(@"""name""\s*:\s*""([^""]+)""", RegexOptions.Compiled);
        // 匹配单行里的上线状态字段
        static Regex regStatus = new Regex(@"""status""\s*:\s*""([^""]+)""", RegexOptions.Compiled);

        /// <summary>
        /// key为upstream域名，value为多行name即IP列表
        /// </summary>
        /// <param name="html"></param>
        /// <returns></returns>
        private static Dictionary<string, List<Tuple<string, string>>> MatchNginxRows(string html)
        {
            var ret = new Dictionary<string, List<Tuple<string, string>>>();
            var match = regRow.Match(html);
            while (match.Success)
            {
                var row = match.Value;
                var upstream = match.Result("$1").Trim().Replace("_conf", "");
                List<Tuple<string, string>> domainIpLst;
                if (!ret.TryGetValue(upstream, out domainIpLst))
                {
                    domainIpLst = new List<Tuple<string, string>>();
                    ret[upstream] = domainIpLst;
                }

                var matchName = regName.Match(row);
                var name = string.Empty;
                if (matchName.Success)
                {
                    name = matchName.Result("$1").Trim();
                    if (name.EndsWith(":80"))
                    {
                        name = name.Substring(0, name.Length - 3);
                    }
                }

                var matchStatus = regStatus.Match(row);
                var status = string.Empty;
                if (matchStatus.Success)
                {
                    status = matchStatus.Result("$1").Trim();
                }
                domainIpLst.Add(new Tuple<string, string>(name, status));
                match = match.NextMatch();
            }
            return ret;
        }

        private static void SetGridStatus(ListView lv, Dictionary<string, List<Tuple<string, string>>> statuses,
            Dictionary<string, string> ipmap)
        {
            Utility.InvokeControl(lv, () =>
            {
                foreach (ListViewItem item in lv.Items)
                {
                    List<Tuple<string, string>> ngStatus;
                    var domain = item.SubItems[COLNAME].Text;
                    if (!statuses.TryGetValue(domain, out ngStatus))
                    {
                        // 该域名在ng列表不存在
                        item.SubItems[COLNGINX].Text = "未配置";
                        continue;
                    }

                    var ipall = item.SubItems[0].Text;
                    var arr = ipall.Split(':');
                    string ip2;
                    if (!ipmap.TryGetValue(arr[0], out ip2))
                    {
                        ip2 = string.Empty;
                    }
                    if (arr.Length > 1)
                    {
                        ip2 = ip2 + ":" + arr[1];
                    }
                    bool statusSetted = false;
                    foreach (Tuple<string, string> tuple in ngStatus)
                    {
                        if (tuple.Item1 == ipall || tuple.Item1 == ip2)
                        {
                            item.SubItems[COLNGINX].Text = tuple.Item2;
                            statusSetted = true;
                            break;
                        }
                    }
                    if (!statusSetted)
                    {
                        item.SubItems[COLNGINX].Text = "未配置";
                    }
                }
            });
        }


        private void linkLabel1_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        {
            new About().ShowDialog(this);
        }

        Site FindSite(ListViewItem item)
        {
            string name = item.SubItems[COLNAME].Text;
            return _sites.Find(site => site.name == name);
        }


        static string DoGetPage(string url, string para = null, string proxy = null, bool showHeader = false)
        {
            // 10网段的ip代理，要切换为外网ip
            if (proxy != null && proxy.StartsWith("10.", StringComparison.Ordinal))
            {
                var ipmap = InOutIpMap;
                var arr = proxy.Split(':');
                string ip2;
                if (ipmap.TryGetValue(arr[0], out ip2))
                {
                    if (arr.Length > 1)
                    {
                        ip2 = ip2 + ":" + arr[1];
                    }
                    proxy = ip2;
                }
            }

            return Utility.GetPage(url, para, proxy, showHeader: showHeader, allowRedirect: true);
        }
    }
}
