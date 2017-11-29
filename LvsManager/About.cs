using System;
using System.Windows.Forms;

namespace Beinet.cn.Tools.LvsManager
{
    public partial class About : Form
    {
        public About()
        {
            InitializeComponent();
            ShowInTaskbar = false;// 不能放到OnLoad里，会导致窗体消失
        }

        protected override void OnLoad(EventArgs e)
        {
            base.OnLoad(e);
            if (Owner != null)
            {
                Left = Owner.Left + 20;
                Top = Owner.Top + 20;
                if (Top < 0)
                    Top = 0;
            }

            textBox1.Text = @"原理说明：
1、本工具本身并不能控制服务器是否下线，而是由服务器站点下的checkipinfo.aspx页面代码控制

2、在这个checkipinfo.aspx页面有下面这样一段代码：
    if (!string.IsNullOrEmpty(Request.QueryString[""SetState""]))
    {
        string tmp = Request.QueryString[""SetState""];
        bool setState = string.IsNullOrEmpty(tmp) || tmp == ""1"" ||
                        tmp.Equals(""true"", StringComparison.OrdinalIgnoreCase) ||
                        tmp.Equals(""online"", StringComparison.OrdinalIgnoreCase);
        SetCurrentStatus(setState);
        currentState = setState ? ONLINE : OFFLINE;
    }

   通过这段代码控制输出给 前端负载均衡的HTTP状态码和响应字符串，以便 前端负载均衡控制 它的上线或下线

3、某些前端负载均衡控制 对服务器下线是有一定时间间隔的，通常需要20秒至60秒的反应时间，
   如果工具一直显示下线中，始终无法下线，那么你要确认是否有用户设置了host指向这台服务器，从而导致无法下线

4、工具对服务器下线与否的依据，是在你的Global.asax.cs里有个静态变量：Global.LAST_ACCESS_TIME_KEY，你需要设置如下代码：
    // 最近一次用户访问的时间    
    public static DateTime LAST_ACCESS_TIME_KEY = DateTime.Now;
    // 站点启动以来，正常用户访问次数
    public static int AccessCount = 0;
    void Application_EndRequest(object sender, EventArgs e)
    {
        string url = Request.Url.ToString().ToLower();
        // 记录活动时间，用于判断站点是否被用户使用中（这些判断代码注意要屏蔽测试页面）
        if (url.IndexOf(""iswebmon="", StringComparison.Ordinal) < 0 &&           // 站点监控程序访问，不作为用户
            url.IndexOf(""/checkipinfo.aspx"", StringComparison.Ordinal) < 0 &&   // 前端轮询时，不作为用户
            url.IndexOf(""/z.aspx"", StringComparison.Ordinal) < 0)       
        {
            LAST_ACCESS_TIME_KEY = DateTime.Now;
            Interlocked.Increment(ref AccessCount);
        }
   如果这个Global.LAST_ACCESS_TIME_KEY时间跟服务器的DateTime.Now相差超过10秒，那么工具认为下线成功";
            textBox1.Select(0, 0);
        }

        private void About_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Escape)
                Close();
        }
    }
}
