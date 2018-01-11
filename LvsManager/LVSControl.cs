using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Windows.Forms;
using System.Threading;

namespace Beinet.cn.Tools.LvsManager
{
    public partial class LVSControl : Form
    {
        private const int COLS = 7;

        //private const int COLIP = 0;
        private const int COLNAME = 1;
        private const int COLLVS = 2;
        private const int COLSTATUS = 3;
        private const int COLLASTTIME = 4;
        private const int COLNOW = 5;
        private const int COLPV = 6;

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
                for(int i=_threads.Count-1;i>=0;i--)
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
                        lock (_threads)
                        {
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
                    string html = Utility.GetPage(site.url + "?getrealstate=yes&format=robot", null, serverIP); 
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
                        str = Utility.GetPage(url, null, serverIP); 
                    }
                    catch (Exception ex)
                    {
                        str = ex.ToString();
                    }
                    webBrowser1.DocumentText = str;
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
                string serverIP = lvServers.SelectedItems[0].Text;
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
                    if (curentServerTime == null)
                    {
                        item.SubItems[COLSTATUS].Text = "获取中..";
                        return;
                    }
                    if (lvsState.Equals("online", StringComparison.CurrentCultureIgnoreCase))
                    {
                        if ((curentServerTime.Value - lastRequestTime).TotalSeconds < site.offlineSecond)
                        {
                            item.SubItems[COLSTATUS].Text = "运行中";
                            item.ForeColor = Color.Green;
                        }
                        else
                        {
                            item.SubItems[COLSTATUS].Text = "无流量";
                            item.ForeColor = Color.Red;
                        }
                    }
                    else
                    {
                        if ((curentServerTime.Value - lastRequestTime).TotalSeconds < site.offlineSecond)
                        {
                            item.SubItems[COLSTATUS].Text = "正在下架中...";
                            item.ForeColor = Color.Blue;
                        }
                        else
                        {
                            item.SubItems[COLSTATUS].Text = "已下架";
                            item.ForeColor = Color.Red;
                        }
                    }
                    //ms framework的bug,必须重新设置一下Text才能触发subitem的界面更新。该bug和机器有关
                    //http://social.msdn.microsoft.com/Forums/en-US/winforms/thread/3e312c15-a470-4d65-98b6-c2e658b35454
                    item.Text = serverIP;
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
    }
}
