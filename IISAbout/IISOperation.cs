using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using Microsoft.Web.Administration;

namespace Beinet.cn.Tools.IISAbout
{
    /// <summary>
    /// IIS操作辅助类.
    /// 需要添加引用：C:\Windows\assembly\GAC_MSIL\Microsoft.Web.Administration\7.0.0.0__31bf3856ad364e35\Microsoft.Web.Administration.dll
    /// MSDN: https://msdn.microsoft.com/en-us/library/microsoft.web.administration
    /// </summary>
    public class IISOperation
    {
        const string SITE_KEY = "/";
        /// <summary>
        /// 要操作的IIS服务器IP,默认为本机
        /// </summary>
        public string ServerIp { get; private set; }
        public IISOperation(string serverIp = "127.0.0.1")
        {
            if (string.IsNullOrEmpty(serverIp))
            {
                serverIp = "127.0.0.1";
            }
            ServerIp = serverIp;
        }

        /// <summary>
        /// 获取所有站点列表返回
        /// </summary>
        /// <returns></returns>
        public List<Site> ListSite()
        {
            var ret = new List<Site>();
            using (var sm = ServerManager.OpenRemote(ServerIp))
            {
                foreach (Microsoft.Web.Administration.Site iissite in sm.Sites)
                {
                    var site = BindSite(iissite);
                    if (site != null)
                    {
                        ret.Add(site);
                    }
                }
            }
            return ret;
        }

        public Site FindSite(string name)
        {
            using (var sm = ServerManager.OpenRemote(ServerIp))
            {
                var iissite = sm.Sites[name];
                var site = BindSite(iissite);
                return site;
            }
        }

        Site BindSite(Microsoft.Web.Administration.Site iissite)
        {
            if (iissite == null)
            {
                return null;
            }
            var defaultApp = iissite.Applications[SITE_KEY];
            var defaultVirDir = defaultApp.VirtualDirectories[SITE_KEY];
            var site = new Site()
            {
                Id = iissite.Id,
                Name = iissite.Name,
                State = (int)iissite.State,
                Dir = defaultVirDir.PhysicalPath,
                Bindings = GetBinding(iissite.Bindings),
                PoolName = defaultApp.ApplicationPoolName,
                ConnectionTimeOut = (int)iissite.Limits.ConnectionTimeout.TotalSeconds,
            };
            site.LogDir = iissite.LogFile.Enabled ? iissite.LogFile.Directory + "\\W3SVC" + site.Id.ToString() : "";
            return site;
        }

        #region 站点操作

        /// <summary>
        /// 停止站点，返回成功失败
        /// </summary>
        /// <param name="name"></param>
        /// <param name="waitSecond">等待间隔</param>
        /// <returns></returns>
        public bool StopSite(string name, int waitSecond = 10)
        {
            using (var sm = ServerManager.OpenRemote(ServerIp))
            {
                var site = sm.Sites[name];
                if (site.State == ObjectState.Stopped)
                    return true;
                site.Stop();
                return (CheckState(site, ObjectState.Stopped, waitSecond));
            }
        }

        /// <summary>
        /// 启动站点，返回成功失败
        /// </summary>
        /// <param name="name"></param>
        /// <param name="waitSecond">等待间隔</param>
        /// <returns></returns>
        public bool StartSite(string name, int waitSecond = 10)
        {
            using (var sm = ServerManager.OpenRemote(ServerIp))
            {
                var site = sm.Sites[name];
                if (site.State == ObjectState.Started)
                    return true;
                site.Start();
                return (CheckState(site, ObjectState.Started, waitSecond));
            }
        }

        /// <summary>
        /// 0重启成功；1停止失败；2启动失败
        /// </summary>
        /// <param name="name"></param>
        /// <param name="waitSecond">等待间隔</param>
        /// <returns></returns>
        public int RestartSite(string name, int waitSecond = 10)
        {
            using (var sm = ServerManager.OpenRemote(ServerIp))
            {
                var site = sm.Sites[name];
                if (site.State != ObjectState.Stopped)
                {
                    site.Stop();
                    if (!CheckState(site, ObjectState.Stopped, waitSecond))
                        return 1;
                }
                if (site.State != ObjectState.Started)
                {
                    site.Start();
                    if (!CheckState(site, ObjectState.Started, waitSecond))
                        return 2;
                }
            }
            return 0;
        }

        public string CreateSite(string name, string sitePath, string binding, string poolname)
        {
            using (var sm = ServerManager.OpenRemote(ServerIp))
            {
                var site = sm.Sites.FirstOrDefault(x => x.Name == name);
                if (site != null)
                {
                    return name + "站点已存在";
                }
                try
                {
                    CreatePool(poolname ?? name, sm);
                }
                catch (Exception exp)
                {
                    return "建应用程序池出错:" + exp.Message;
                }
                //创建Site
                var arrBind = SplitBind(binding);
                if (arrBind.Count <= 0)
                {
                    return "无效的绑定：" + binding;
                }
                var defaultBind = arrBind[0];
                site = sm.Sites.Add(name, defaultBind[0], defaultBind[1], sitePath);
                site.ServerAutoStart = true;                // 是否随着IIS一起启动
                site.Limits.ConnectionTimeout = new TimeSpan(0, 0, 10); // 连接超时为10秒
                for (var i = 1; i < arrBind.Count; i++)
                {
                    site.Bindings.Add(arrBind[i][1], arrBind[i][0]);
                }
                //site.Bindings[0].EndPoint.Port = port;
                Application root = site.Applications["/"];
                root.ApplicationPoolName = name;
                root.VirtualDirectories["/"].PhysicalPath = sitePath;
                root.SetAttributeValue("preloadEnabled", true); // 预加载

                sm.CommitChanges();
            }
            return "OK";
        }

        List<string[]> SplitBind(string binding)
        {
            var ret = new List<string[]>();
            var arrBind = binding.Split(';');
            foreach (var item in arrBind)
            {
                string protocal;
                string bind;
                if (item.StartsWith("http:", StringComparison.OrdinalIgnoreCase))
                {
                    protocal = "http";
                    bind = item.Substring("http:".Length).Trim();
                }
                else if (item.StartsWith("https:", StringComparison.OrdinalIgnoreCase))
                {
                    protocal = "https";
                    bind = item.Substring("https:".Length).Trim();
                }
                else
                {
                    protocal = "http";
                    bind = item.Trim();
                }
                if (bind.Length == 0)
                {
                    continue;
                }
                ret.Add(new string[] { protocal, bind });
            }
            return ret;
        }
        #endregion

        #region 程序池操作

        void CreatePool(string name, ServerManager sm)
        {
            //创建应用程序池
            ApplicationPool appPool = sm.ApplicationPools.FirstOrDefault(x => x.Name == name);
            if (appPool != null)
            {
                return;
            }
            appPool = sm.ApplicationPools.Add(name);
            appPool.AutoStart = true; // 是否随着IIS一起启动
            appPool.ManagedRuntimeVersion = "v4.0"; // .net版本
            appPool.Enable32BitAppOnWin64 = false; // 不要支持32位站点
            appPool.ManagedPipelineMode = ManagedPipelineMode.Integrated; // 集成模式
            appPool.QueueLength = 65527;
            appPool.StartMode = StartMode.AlwaysRunning; // 启动模式，一直运行
            //appPool.Cpu.Limit = 3;  // CPU限制
            appPool.Recycling.PeriodicRestart.Time = new TimeSpan(0, 0, 0); //回收时间间隔, 必须是分钟级
            appPool.Recycling.PeriodicRestart.Requests = 0; // 请求限制：回收前可以处理的请求数
            //appPool.Recycling.PeriodicRestart.Schedule.Add(new TimeSpan(4, 32, 0)); // 特定时间回收, 必须是分钟级
            appPool.ProcessModel.IdleTimeout = new TimeSpan(0, 0, 0); //闲置超时
            appPool.ProcessModel.MaxProcesses = 1; //最大工作进程数
            sm.CommitChanges(); // 这里可以不提交，跟站点一起提交
        }

        /// <summary>
        /// 停止程序池，返回成功失败
        /// </summary>
        /// <param name="name"></param>
        /// <param name="waitSecond">等待间隔</param>
        /// <returns></returns>
        public bool StopPool(string name, int waitSecond = 10)
        {
            using (var sm = ServerManager.OpenRemote(ServerIp))
            {
                var pool = sm.ApplicationPools[name];
                if (pool.State == ObjectState.Stopped)
                    return true;
                pool.Stop();
                return (CheckState(pool, ObjectState.Stopped, waitSecond));
            }
        }

        /// <summary>
        /// 启动程序池，返回成功失败
        /// </summary>
        /// <param name="name"></param>
        /// <param name="waitSecond">等待间隔</param>
        /// <returns></returns>
        public bool StartPool(string name, int waitSecond = 10)
        {
            using (var sm = ServerManager.OpenRemote(ServerIp))
            {
                var pool = sm.ApplicationPools[name];
                if (pool.State == ObjectState.Started)
                    return true;
                pool.Start();
                return (CheckState(pool, ObjectState.Started, waitSecond));
            }
        }

        /// <summary>
        /// 0重启程序池成功；1停止失败；2启动失败
        /// </summary>
        /// <param name="name"></param>
        /// <param name="waitSecond">等待间隔</param>
        /// <returns></returns>
        public int RestartPool(string name, int waitSecond = 10)
        {
            using (var sm = ServerManager.OpenRemote(ServerIp))
            {
                var pool = sm.ApplicationPools[name];

                if (pool.State != ObjectState.Stopped)
                {
                    pool.Stop();
                    if (!CheckState(pool, ObjectState.Stopped, waitSecond))
                        return 1;
                }
                if (pool.State != ObjectState.Started)
                {
                    pool.Start();
                    if (!CheckState(pool, ObjectState.Started, waitSecond))
                        return 2;
                }
            }
            return 0;
        }

        #endregion

        bool CheckState(Microsoft.Web.Administration.Site site, ObjectState state, int waitSecond)
        {
            int waitNum = waitSecond * 10;
            while (site.State != state && waitNum > 0)
            {
                waitNum--;
                Thread.Sleep(100);
            }
            return site.State == state;
        }

        bool CheckState(Microsoft.Web.Administration.ApplicationPool pool, ObjectState state, int waitSecond)
        {
            int waitNum = waitSecond * 10;
            while (pool.State != state && waitNum > 0)
            {
                waitNum--;
                Thread.Sleep(100);
            }
            return pool.State == state;
        }


        string GetBinding(BindingCollection bindings)
        {
            if (bindings == null)
            {
                return "";
            }
            var sb = new StringBuilder();
            foreach (var binding in bindings)
            {
                sb.AppendFormat("{0}:{1};", binding.Protocol, binding.BindingInformation);
            }
            return sb.ToString();
        }


    }

    /// <summary>
    /// IIS站点对象
    /// </summary>
    public class Site
    {
        /// <summary>
        /// 站点Id
        /// </summary>
        public long Id { get; set; }

        /// <summary>
        /// 站点名
        /// </summary>
        public string Name { get; set; }

        /// <summary>
        /// 运行状态,Starting-0,Started-1,Stopping-2,Stopped-3,Unknown-4
        /// </summary>
        public int State { get; set; }

        public string StateText
        {
            get
            {
                switch (State)
                {
                    case 0:
                        return "Starting";
                    case 1:
                        return "Started";
                    case 2:
                        return "Stopping";
                    case 3:
                        return "Stopped";
                    case 4:
                        return "Unknown";
                    default:
                        return "Unknown-" + State.ToString();
                }
            }
        }

        /// <summary>
        /// 绑定的物理目录
        /// </summary>
        public string Dir { get; set; }

        /// <summary>
        /// 站点日志所在目录，为空表示未启用日志
        /// </summary>
        public string LogDir { get; set; }


        /// <summary>
        /// 绑定的主机头和IP等
        /// </summary>
        public string Bindings { get; set; }

        /// <summary>
        /// 使用的应用程序池名
        /// </summary>
        public string PoolName { get; set; }
        /// <summary>
        /// 站点限制里的连接超时，单位秒
        /// </summary>
        public int ConnectionTimeOut { get; set; }

        public override string ToString()
        {
            var sb = new StringBuilder();
            sb.AppendFormat(@"站点ID：{0}
站点名称：{1}
物理目录：{2}
绑定域名：{3}
程序池名：{4}
站点状态：{5}
日志目录：{6}
连接超时：{7}秒
", Id, Name, Dir, Bindings, PoolName, StateText, LogDir, ConnectionTimeOut.ToString());
            return sb.ToString();
        }
    }
}
