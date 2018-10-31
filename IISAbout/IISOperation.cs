using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.Web.Administration;
using Microsoft.Win32;

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
        /// 获取IIS版本
        /// </summary>
        /// <returns></returns>
        public Version GetIisVersion()
        {
            using (var componentsKey = Registry.LocalMachine.OpenSubKey(@"Software\Microsoft\InetStp", false))
            {
                if (componentsKey != null)
                {
                    int majorVersion = (int)componentsKey.GetValue("MajorVersion", -1);
                    int minorVersion = (int)componentsKey.GetValue("MinorVersion", -1);

                    if (majorVersion != -1 && minorVersion != -1)
                    {
                        return new Version(majorVersion, minorVersion);
                    }
                }

                return new Version(0, 0);
            }
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
                foreach (Microsoft.Web.Administration.Site iissite in sm.Sites.OrderBy(item => item.Name))
                {
                    var site = BindSite(iissite, sm);
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
                var site = BindSite(iissite, sm);
                return site;
            }
        }

        Site BindSite(Microsoft.Web.Administration.Site iissite, ServerManager sm)
        {
            if (iissite == null)
            {
                return null;
            }
            var defaultApp = iissite.Applications[SITE_KEY];
            var defaultVirDir = defaultApp.VirtualDirectories[SITE_KEY];
            int state;
            try
            {
                state = (int) iissite.State;
            }
            catch
            {
                // 注意：ftp站点会抛异常，ftp状态要用 iissite.GetChildElement("ftpServer")["state"]  
                state = -1;
            }
            var site = new Site()
            {
                Id = iissite.Id,
                Name = iissite.Name,
                State = state,
                Dir = defaultVirDir.PhysicalPath,
                Bindings = GetBinding(iissite.Bindings, out var isHttp),
                PoolName = defaultApp.ApplicationPoolName,
                ConnectionTimeOut = (int)iissite.Limits.ConnectionTimeout.TotalSeconds,
            };
            site.LogDir = iissite.LogFile.Enabled ? iissite.LogFile.Directory + "\\W3SVC" + site.Id.ToString() : "";
            site.IsHttp = isHttp;
            try
            {
                // 读取是否启用预加载
                site.Preload = (bool) defaultApp.GetAttributeValue("preloadEnabled");
            }
            catch
            {
                site.Preload = false;
            }
            var pool = sm.ApplicationPools[site.PoolName];
            site.GcTimeStr = GetSchedule(pool);
            
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
                Utility.Log("stop site:" + name);
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
                Utility.Log("start site:" + name);
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
            Utility.Log("restart site:" + name);
            return 0;
        }

        public string CreateSite(string name, string sitePath, string binding, 
            string poolname, bool preloadEnabled, int timeoutSeconds, string recyleTimes)
        {
            using (var sm = ServerManager.OpenRemote(ServerIp))
            {
                var site = sm.Sites.FirstOrDefault(x => x.Name == name);
                if (site != null)
                {
                    return name + "站点已存在";
                }
                ApplicationPool pool;
                try
                {
                    pool = CreatePool(poolname ?? name, sm);
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
                site.Limits.ConnectionTimeout = TimeSpan.FromSeconds(timeoutSeconds); // 连接超时
                for (var i = 1; i < arrBind.Count; i++)
                {
                    site.Bindings.Add(arrBind[i][1], arrBind[i][0]);
                }
                //site.Bindings[0].EndPoint.Port = port;
                Application root = site.Applications[SITE_KEY];
                root.ApplicationPoolName = name;
                root.VirtualDirectories[SITE_KEY].PhysicalPath = sitePath;
                root.SetAttributeValue("preloadEnabled", preloadEnabled); // 预加载,true会执行Application_Start方法。

                SetPoolRecyleTime(pool, recyleTimes);
                sm.CommitChanges();
            }
            Utility.Log($"create site:{name}，{sitePath}，{binding}，{poolname}，{preloadEnabled}，" +
                        $"{timeoutSeconds}，{recyleTimes}");
            return "OK";
        }

        public string UpdateSite(string name, string sitePath, string binding, 
            string poolname, bool preloadEnabled, int timeoutSeconds, string recyleTimes)
        {
            using (var sm = ServerManager.OpenRemote(ServerIp))
            {
                var site = sm.Sites.FirstOrDefault(x => x.Name == name);
                if (site == null)
                {
                    return name + "站点未找到，无法更新";
                }
                var pool = sm.ApplicationPools.FirstOrDefault(x => x.Name == poolname);
                if (pool == null)
                {
                    return poolname + "程序池未找到，无法设置";
                }
                // 清空并重新绑定
                var arrBind = SplitBind(binding);
                if (arrBind.Count <= 0)
                {
                    return "无效的绑定：" + binding;
                }
                site.Bindings.Clear();
                for (var i = 0; i < arrBind.Count; i++)
                {
                    var bindinfo = arrBind[i][1];
                    var protocal = arrBind[i][0];
                    site.Bindings.Add(bindinfo, protocal);
                }

                site.Limits.ConnectionTimeout = TimeSpan.FromSeconds(timeoutSeconds); // 连接超时
                Application root = site.Applications[SITE_KEY];
                root.ApplicationPoolName = poolname;
                root.VirtualDirectories[SITE_KEY].PhysicalPath = sitePath;
                root.SetAttributeValue("preloadEnabled", preloadEnabled); // 预加载,true会执行Application_Start方法。

                SetPoolRecyleTime(pool, recyleTimes);
                sm.CommitChanges();
            }
            Utility.Log($"update site:{name}，{sitePath}，{binding}，{poolname}，{preloadEnabled}，" +
                        $"{timeoutSeconds}，{recyleTimes}");
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

        /// <summary>
        /// 停止全部站点
        /// </summary>
        /// <param name="startLater">停止后是否要启动</param>
        /// <param name="siteNames"></param>
        /// <returns></returns>
        public string StopSite(bool startLater, params string[] siteNames)
        {
            int ok = 0, fail = 0, noop = 0;
            var ret = new StringBuilder();
            var waitSecond = 30;
            using (var sm = ServerManager.OpenRemote(ServerIp))
            {
                var options = new ParallelOptions();
                options.MaxDegreeOfParallelism = 100;
                var pools = new HashSet<string>();
                // 循环停止所有站点
                Parallel.ForEach(sm.Sites, options, state =>
                {
                    var site = state;
                    if (siteNames == null || siteNames.Length == 0 || siteNames.Contains(site.Name))
                    {
                        var poolName = site.Applications[SITE_KEY].ApplicationPoolName;
                        lock (pools)
                        {
                            pools.Add(poolName);
                        }
                        try
                        {
                            if (site.State != ObjectState.Stopped)
                            {
                                site.Stop();
                                if (!CheckState(site, ObjectState.Stopped, waitSecond))
                                {
                                    ret.AppendFormat("site:{0},", site.Name);
                                    fail++;
                                }
                                else
                                {
                                    ok++;
                                }
                            }
                            else
                            {
                                noop++;
                            }
                        }
                        catch (Exception)
                        {
                            ret.AppendFormat("site:{0},", site.Name);
                        }
                        if (startLater && site.State == ObjectState.Stopped)
                        {
                            site.Start();
                        }
                    }
                });
                Parallel.ForEach(pools, options, poolname =>
                {
                    // ReSharper disable once AccessToDisposedClosure
                    var pool = sm.ApplicationPools[poolname];
                    try
                    {
                        if (pool.State != ObjectState.Stopped)
                        {
                            pool.Stop();
                            if (!CheckState(pool, ObjectState.Stopped, waitSecond))
                            {
                                ret.AppendFormat("pool:{0},", pool.Name);
                                fail++;
                            }
                            else
                            {
                                ok++;
                            }
                        }
                        else
                        {
                            noop++;
                        }
                    }
                    catch (Exception)
                    {
                        ret.AppendFormat("pool:{0},", pool.Name);
                    }
                    if (startLater && pool.State == ObjectState.Stopped)
                    {
                        pool.Start();
                    }
                });
            }
            if (ret.Length > 0)
            {
                ret.Insert(0, "操作失败列表：\r\n");
            }
            var strCnt = $"成功/失败/无操作:{ok.ToString()}/{fail.ToString()}/{noop.ToString()}\r\n";
            ret.Insert(0, strCnt);
            Utility.Log("StopSite:" + startLater + "\r\n" + ret);
            return ret.ToString();
        }

        /// <summary>
        /// 启动指定站点的预加载
        /// </summary>
        /// <param name="sites"></param>
        /// <returns></returns>
        public string StartSitePreload(params string[] sites)
        {
            int ok = 0, fail = 0, noop = 0;
            var ret = new StringBuilder();
            using (var sm = ServerManager.OpenRemote(ServerIp))
            {
                foreach (var site in sm.Sites)
                {
                    if (sites == null || sites.Length == 0 || sites.Contains(site.Name))
                    {
                        try
                        {
                            var defaultApp = site.Applications[SITE_KEY];
                            if ((bool) defaultApp.GetAttributeValue("preloadEnabled"))
                            {
                                noop++;
                            }
                            else
                            {
                                defaultApp.SetAttributeValue("preloadEnabled", true);
                                ok++;
                            }
                        }
                        catch
                        {
                            fail++;
                            ret.AppendFormat("{0},", site.Name);
                        }
                    }
                }
                sm.CommitChanges();
            }
            if (ret.Length > 0)
            {
                ret.Insert(0, "操作失败列表：\r\n");
            }
            var strCnt = $"成功/失败/无操作:{ok.ToString()}/{fail.ToString()}/{noop.ToString()}\r\n";
            ret.Insert(0, strCnt);
            Utility.Log("批量启动站点预加载:\r\n" + ret);
            return ret.ToString();
        }

        /// <summary>
        /// 根据站点名创建程序池，并修改站点的程序池
        /// </summary>
        /// <param name="sites"></param>
        /// <returns></returns>
        public string ModiSitesPool(params string[] sites)
        {
            int ok = 0, fail = 0, noop = 0;
            var ret = new StringBuilder();
            using (var sm = ServerManager.OpenRemote(ServerIp))
            {
                foreach (var site in sm.Sites)
                {
                    if (sites == null || sites.Length == 0 || sites.Contains(site.Name))
                    {
                        try
                        {
                            var defaultApp = site.Applications[SITE_KEY];
                            if (defaultApp.ApplicationPoolName == site.Name)
                            {
                                noop++;
                            }
                            else
                            {
                                CreatePool(site.Name, sm);
                                defaultApp.ApplicationPoolName = site.Name;
                                ok++;
                            }
                        }
                        catch(Exception)
                        {
                            fail++;
                            ret.AppendFormat("{0},", site.Name);
                        }
                    }
                }
                sm.CommitChanges();
            }
            if (ret.Length > 0)
            {
                ret.Insert(0, "操作失败列表：\r\n");
            }
            var strCnt = $"成功/失败/无操作:{ok.ToString()}/{fail.ToString()}/{noop.ToString()}\r\n";
            ret.Insert(0, strCnt);
            Utility.Log("修改站点为同名程序池:\r\n" + ret);
            return ret.ToString();
        }
        #endregion


        #region 程序池操作

        /// <summary>
        /// 批量设置站点对应的程序池回收时间
        /// </summary>
        /// <param name="hour"></param>
        /// <param name="minute"></param>
        /// <param name="diffMinute"></param>
        /// <param name="sites"></param>
        /// <returns></returns>
        public string SetPoolsRecyleTime(int hour, int minute, int diffMinute, params string[] sites)
        {
            int ok = 0, fail = 0;
            var ret = new StringBuilder();
            var logMsg = new StringBuilder("批量设置程序池回收时间：\r\n");
            var ts = new TimeSpan(hour, minute, 0);
            var addTs = TimeSpan.FromMinutes(diffMinute);
            using (var sm = ServerManager.OpenRemote(ServerIp))
            {
                foreach (var site in sm.Sites.OrderBy(item => item.Name))
                {
                    if (sites == null || sites.Length == 0 || sites.Contains(site.Name))
                    {
                        try
                        {
                            var poolName = site.Applications[SITE_KEY].ApplicationPoolName;
                            var pool = sm.ApplicationPools[poolName];
                            logMsg.AppendFormat("{0}-Pool:{1} :{2}\r\n", site.Name, pool.Name, ts.ToString());
                            pool.Recycling.PeriodicRestart.Schedule.Clear();
                            pool.Recycling.PeriodicRestart.Schedule.Add(ts);
                            ok++;
                        }
                        catch
                        {
                            fail++;
                            ret.AppendFormat("{0},", site.Name);
                        }
                    }
                    ts = ts.Add(addTs);
                }
                sm.CommitChanges();
            }

            if (ret.Length > 0)
            {
                ret.Insert(0, "操作失败列表：\r\n");
            }
            var strCnt = $"成功/失败:{ok.ToString()}/{fail.ToString()}\r\n";
            ret.Insert(0, strCnt);
            Utility.Log(logMsg + "\r\n" + ret);
            return ret.ToString();
        }


        private void SetPoolRecyleTime(ApplicationPool pool, string times)
        {
            pool.Recycling.PeriodicRestart.Schedule.Clear();
            foreach (var item in times.Split(';'))
            {
                if (TimeSpan.TryParse(item, out var ts))
                {
                    pool.Recycling.PeriodicRestart.Schedule.Add(ts);
                }
            }
        }

        private ApplicationPool CreatePool(string name, ServerManager sm)
        {
            //创建应用程序池
            ApplicationPool appPool = sm.ApplicationPools.FirstOrDefault(x => x.Name == name);
            if (appPool != null)
            {
                return appPool;
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
            return appPool;
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
                Utility.Log("stop pool:" + name);
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
                Utility.Log("start pool:" + name);
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
            Utility.Log("restart pool:" + name);
            return 0;
        }


        /// <summary>
        /// 关闭指定程序池的特定时间回收配置
        /// </summary>
        /// <param name="poolNames"></param>
        /// <returns></returns>
        public string RemovePoolGc(params string[] poolNames)
        {
            int ok = 0, fail = 0, noop = 0;
            var ret = new StringBuilder();
            using (var sm = ServerManager.OpenRemote(ServerIp))
            {
                foreach (ApplicationPool pool in sm.ApplicationPools)
                {
                    if (poolNames == null || poolNames.Length == 0 || poolNames.Contains(pool.Name))
                    {
                        try
                        {
                            if (pool.Recycling.PeriodicRestart.Schedule.Count > 0)
                            {
                                noop++;
                            }
                            else
                            {
                                ok++;
                                pool.Recycling.PeriodicRestart.Schedule.Clear();
                            }
                        }
                        catch (Exception)
                        {
                            fail++;
                            ret.AppendFormat("{0},", pool.Name);
                        }
                    }
                }
                sm.CommitChanges();
            }
            if (ret.Length > 0)
            {
                ret.Insert(0, "下列程序池操作失败：\r\n");
            }
            var strCnt = $"成功/失败/无操作:{ok.ToString()}/{fail.ToString()}/{noop.ToString()}\r\n";
            ret.Insert(0, strCnt);
            Utility.Log("批量删除程序池回收时间设置:\r\n" + ret);
            return ret.ToString();
        }
        #endregion




        string GetSchedule(ApplicationPool pool)
        {
            var ret = new StringBuilder();
            foreach (Schedule item in pool.Recycling.PeriodicRestart.Schedule)
            {
                ret.AppendFormat("{0};", item.Time.ToString("c"));
            }
            return ret.ToString();
        }

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


        string GetBinding(BindingCollection bindings, out bool haveHttp)
        {
            haveHttp = false;
            if (bindings == null)
            {
                return "";
            }
            var sb = new StringBuilder();
            foreach (var binding in bindings)
            {
                if (binding.Protocol.StartsWith("http", StringComparison.OrdinalIgnoreCase))
                    haveHttp = true;
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

        /// <summary>
        /// 应用程序池里的定期回收时间配置，分号分隔多项
        /// </summary>
        public string GcTimeStr { get; set; }
        /// <summary>
        /// 站点是否开启了预加载
        /// </summary>
        public bool Preload { get; set; }

        /// <summary>
        /// 当前站点是否允许http协议（比如ftp站点就不允许）
        /// </summary>
        public bool IsHttp { get; set; }

        public override string ToString()
        {
            var sb = new StringBuilder();
            sb.AppendFormat(@"ID与名称：{0} {1}
物理目录：{2}
绑定域名：{3}
程序池名：{4}
站点状态：{5}
日志目录：{6}
连接超时：{7}秒
定时回收：{8}
预加载：{9}
", Id, Name, Dir, Bindings, PoolName, StateText, LogDir,
                ConnectionTimeOut.ToString(), GcTimeStr, Preload ? "启用" : "禁用");
            return sb.ToString();
        }
    }
}


/**
 * https://www.cnblogs.com/jjg0519/p/7277971.html
Microsoft.Web.Administration.ServerManager sm = new Microsoft.Web.Administration.ServerManager();

System.Console.WriteLine("应用程序池默认设置：");
System.Console.WriteLine("\t常规：");
System.Console.WriteLine("\t\t.NET Framework 版本：{0}", sm.ApplicationPoolDefaults.ManagedRuntimeVersion);
System.Console.WriteLine("\t\t队列长度：{0}", sm.ApplicationPoolDefaults.QueueLength);
System.Console.WriteLine("\t\t托管管道模式：{0}", sm.ApplicationPoolDefaults.ManagedPipelineMode.ToString());
System.Console.WriteLine("\t\t自动启动：{0}", sm.ApplicationPoolDefaults.AutoStart);

System.Console.WriteLine("\tCPU：");
System.Console.WriteLine("\t\t处理器关联掩码：{0}", sm.ApplicationPoolDefaults.Cpu.SmpProcessorAffinityMask);
System.Console.WriteLine("\t\t限制：{0}", sm.ApplicationPoolDefaults.Cpu.Limit);
System.Console.WriteLine("\t\t限制操作：{0}", sm.ApplicationPoolDefaults.Cpu.Action.ToString());
System.Console.WriteLine("\t\t限制间隔（分钟）：{0}", sm.ApplicationPoolDefaults.Cpu.ResetInterval.TotalMinutes);
System.Console.WriteLine("\t\t已启用处理器关联：{0}", sm.ApplicationPoolDefaults.Cpu.SmpAffinitized);

System.Console.WriteLine("\t回收：");
System.Console.WriteLine("\t\t发生配置更改时禁止回收：{0}", sm.ApplicationPoolDefaults.Recycling.DisallowRotationOnConfigChange);
System.Console.WriteLine("\t\t固定时间间隔（分钟）：{0}", sm.ApplicationPoolDefaults.Recycling.PeriodicRestart.Time.TotalMinutes);
System.Console.WriteLine("\t\t禁用重叠回收：{0}", sm.ApplicationPoolDefaults.Recycling.DisallowOverlappingRotation);
System.Console.WriteLine("\t\t请求限制：{0}", sm.ApplicationPoolDefaults.Recycling.PeriodicRestart.Requests);
System.Console.WriteLine("\t\t虚拟内存限制（KB）：{0}", sm.ApplicationPoolDefaults.Recycling.PeriodicRestart.Memory);
System.Console.WriteLine("\t\t专用内存限制（KB）：{0}", sm.ApplicationPoolDefaults.Recycling.PeriodicRestart.PrivateMemory);
System.Console.WriteLine("\t\t特定时间：{0}", sm.ApplicationPoolDefaults.Recycling.PeriodicRestart.Schedule.ToString());
System.Console.WriteLine("\t\t生成回收事件日志条目：{0}", sm.ApplicationPoolDefaults.Recycling.LogEventOnRecycle.ToString());

System.Console.WriteLine("\t进程孤立：");
System.Console.WriteLine("\t\t可执行文件：{0}", sm.ApplicationPoolDefaults.Failure.OrphanActionExe);
System.Console.WriteLine("\t\t可执行文件参数：{0}", sm.ApplicationPoolDefaults.Failure.OrphanActionParams);
System.Console.WriteLine("\t\t已启用：{0}", sm.ApplicationPoolDefaults.Failure.OrphanWorkerProcess);

System.Console.WriteLine("\t进程模型：");
System.Console.WriteLine("\t\tPing 间隔（秒）：{0}", sm.ApplicationPoolDefaults.ProcessModel.PingInterval.TotalSeconds);
System.Console.WriteLine("\t\tPing 最大响应时间（秒）：{0}", sm.ApplicationPoolDefaults.ProcessModel.PingResponseTime.TotalSeconds);
System.Console.WriteLine("\t\t标识：{0}", sm.ApplicationPoolDefaults.ProcessModel.IdentityType);
System.Console.WriteLine("\t\t用户名：{0}", sm.ApplicationPoolDefaults.ProcessModel.UserName);
System.Console.WriteLine("\t\t密码：{0}", sm.ApplicationPoolDefaults.ProcessModel.Password);
System.Console.WriteLine("\t\t关闭时间限制（秒）：{0}", sm.ApplicationPoolDefaults.ProcessModel.ShutdownTimeLimit.TotalSeconds);
System.Console.WriteLine("\t\t加载用户配置文件：{0}", sm.ApplicationPoolDefaults.ProcessModel.LoadUserProfile);
System.Console.WriteLine("\t\t启动时间限制（秒）：{0}", sm.ApplicationPoolDefaults.ProcessModel.StartupTimeLimit.TotalSeconds);
System.Console.WriteLine("\t\t允许 Ping：{0}", sm.ApplicationPoolDefaults.ProcessModel.PingingEnabled);
System.Console.WriteLine("\t\t闲置超时（分钟）：{0}", sm.ApplicationPoolDefaults.ProcessModel.IdleTimeout.TotalMinutes);
System.Console.WriteLine("\t\t最大工作进程数：{0}", sm.ApplicationPoolDefaults.ProcessModel.MaxProcesses);

System.Console.WriteLine("\t快速故障防护：");
System.Console.WriteLine("\t\t“服务不可用”响应类型：{0}", sm.ApplicationPoolDefaults.Failure.LoadBalancerCapabilities.ToString());
System.Console.WriteLine("\t\t故障间隔（分钟）：{0}", sm.ApplicationPoolDefaults.Failure.RapidFailProtectionInterval.TotalMinutes);
System.Console.WriteLine("\t\t关闭可执行文件：{0}", sm.ApplicationPoolDefaults.Failure.AutoShutdownExe);
System.Console.WriteLine("\t\t关闭可执行文件参数：{0}", sm.ApplicationPoolDefaults.Failure.AutoShutdownParams);
System.Console.WriteLine("\t\t已启用：{0}", sm.ApplicationPoolDefaults.Failure.RapidFailProtection);
System.Console.WriteLine("\t\t最大故障数：{0}", sm.ApplicationPoolDefaults.Failure.RapidFailProtectionMaxCrashes);
System.Console.WriteLine("\t\t允许32位应用程序运行在64位 Windows 上：{0}", sm.ApplicationPoolDefaults.Enable32BitAppOnWin64);

System.Console.WriteLine();
System.Console.WriteLine("网站默认设置：");
System.Console.WriteLine("\t常规：");
System.Console.WriteLine("\t\t物理路径凭据：UserName={0}, Password={1}", sm.VirtualDirectoryDefaults.UserName, sm.VirtualDirectoryDefaults.Password);
System.Console.WriteLine("\t\t物理路径凭据登录类型：{0}", sm.VirtualDirectoryDefaults.LogonMethod.ToString());
System.Console.WriteLine("\t\t应用程序池：{0}", sm.ApplicationDefaults.ApplicationPoolName);
System.Console.WriteLine("\t\t自动启动：{0}", sm.SiteDefaults.ServerAutoStart);
System.Console.WriteLine("\t行为：");
System.Console.WriteLine("\t\t连接限制：");
System.Console.WriteLine("\t\t\t连接超时（秒）：{0}", sm.SiteDefaults.Limits.ConnectionTimeout.TotalSeconds);
System.Console.WriteLine("\t\t\t最大并发连接数：{0}", sm.SiteDefaults.Limits.MaxConnections);
System.Console.WriteLine("\t\t\t最大带宽（字节/秒）：{0}", sm.SiteDefaults.Limits.MaxBandwidth);
System.Console.WriteLine("\t\t失败请求跟踪：");
System.Console.WriteLine("\t\t\t跟踪文件的最大数量：{0}", sm.SiteDefaults.TraceFailedRequestsLogging.MaxLogFiles);
System.Console.WriteLine("\t\t\t目录：{0}", sm.SiteDefaults.TraceFailedRequestsLogging.Directory);
System.Console.WriteLine("\t\t\t已启用：{0}", sm.SiteDefaults.TraceFailedRequestsLogging.Enabled);
System.Console.WriteLine("\t\t已启用的协议：{0}", sm.ApplicationDefaults.EnabledProtocols);

foreach (var s in sm.Sites)//遍历网站
{
System.Console.WriteLine();
System.Console.WriteLine("模式名：{0}", s.Schema.Name);
System.Console.WriteLine("编号：{0}", s.Id);
System.Console.WriteLine("网站名称：{0}", s.Name);
System.Console.WriteLine("物理路径：{0}", s.Applications[SITE_KEY].VirtualDirectories[SITE_KEY].PhysicalPath);
System.Console.WriteLine("物理路径凭据：{0}", s.Methods.ToString());
System.Console.WriteLine("应用程序池：{0}", s.Applications[SITE_KEY].ApplicationPoolName);
System.Console.WriteLine("已启用的协议：{0}", s.Applications[SITE_KEY].EnabledProtocols);
System.Console.WriteLine("自动启动：{0}", s.ServerAutoStart);
System.Console.WriteLine("运行状态：{0}", s.State.ToString());

System.Console.WriteLine("网站绑定：");
foreach (var tmp in s.Bindings)
{
System.Console.WriteLine("\t类型：{0}", tmp.Protocol);
System.Console.WriteLine("\tIP 地址：{0}", tmp.EndPoint.Address.ToString());
System.Console.WriteLine("\t端口：{0}", tmp.EndPoint.Port.ToString());
System.Console.WriteLine("\t主机名：{0}", tmp.Host);
//System.Console.WriteLine(tmp.BindingInformation);
//System.Console.WriteLine(tmp.CertificateStoreName);
//System.Console.WriteLine(tmp.IsIPPortHostBinding);
//System.Console.WriteLine(tmp.IsLocallyStored);
//System.Console.WriteLine(tmp.UseDsMapper);
}

System.Console.WriteLine("连接限制：");
System.Console.WriteLine("\t连接超时（秒）：{0}", s.Limits.ConnectionTimeout.TotalSeconds);
System.Console.WriteLine("\t最大并发连接数：{0}", s.Limits.MaxConnections);
System.Console.WriteLine("\t最大带宽（字节/秒）：{0}", s.Limits.MaxBandwidth);

System.Console.WriteLine("失败请求跟踪：");
System.Console.WriteLine("\t跟踪文件的最大数量：{0}", s.TraceFailedRequestsLogging.MaxLogFiles);
System.Console.WriteLine("\t目录：{0}", s.TraceFailedRequestsLogging.Directory);
System.Console.WriteLine("\t已启用：{0}", s.TraceFailedRequestsLogging.Enabled);

System.Console.WriteLine("日志：");
//System.Console.WriteLine("\t启用日志服务：{0}", s.LogFile.Enabled);
System.Console.WriteLine("\t格式：{0}", s.LogFile.LogFormat.ToString());
System.Console.WriteLine("\t目录：{0}", s.LogFile.Directory);
System.Console.WriteLine("\t文件包含字段：{0}", s.LogFile.LogExtFileFlags.ToString());
System.Console.WriteLine("\t计划：{0}", s.LogFile.Period.ToString());
System.Console.WriteLine("\t最大文件大小（字节）：{0}", s.LogFile.TruncateSize);
System.Console.WriteLine("\t使用本地时间进行文件命名和滚动更新：{0}", s.LogFile.LocalTimeRollover);

System.Console.WriteLine("----应用程序的默认应用程序池：{0}", s.ApplicationDefaults.ApplicationPoolName);
System.Console.WriteLine("----应用程序的默认已启用的协议：{0}", s.ApplicationDefaults.EnabledProtocols);
//System.Console.WriteLine("----应用程序的默认物理路径凭据：{0}", s.ApplicationDefaults.Methods.ToString());
//System.Console.WriteLine("----虚拟目录的默认物理路径凭据：{0}", s.VirtualDirectoryDefaults.Methods.ToString());
System.Console.WriteLine("----虚拟目录的默认物理路径凭据登录类型：{0}", s.VirtualDirectoryDefaults.LogonMethod.ToString());
System.Console.WriteLine("----虚拟目录的默认用户名：{0}", s.VirtualDirectoryDefaults.UserName);
System.Console.WriteLine("----虚拟目录的默认用户密码：{0}", s.VirtualDirectoryDefaults.Password);
System.Console.WriteLine("应用程序 列表：");
foreach (var tmp in s.Applications)
{
if (tmp.Path != SITE_KEY)
{
System.Console.WriteLine("\t模式名：{0}", tmp.Schema.Name);
System.Console.WriteLine("\t虚拟路径：{0}", tmp.Path);
System.Console.WriteLine("\t物理路径：{0}", tmp.VirtualDirectories[SITE_KEY].PhysicalPath);
//System.Console.WriteLine("\t物理路径凭据：{0}", tmp.Methods.ToString());
System.Console.WriteLine("\t应用程序池：{0}", tmp.ApplicationPoolName);
System.Console.WriteLine("\t已启用的协议：{0}", tmp.EnabledProtocols);
}
System.Console.WriteLine("\t虚拟目录 列表：");
foreach (var tmp2 in tmp.VirtualDirectories)
{
if (tmp2.Path != SITE_KEY)
{
    System.Console.WriteLine("\t\t模式名：{0}", tmp2.Schema.Name);
    System.Console.WriteLine("\t\t虚拟路径：{0}", tmp2.Path);
    System.Console.WriteLine("\t\t物理路径：{0}", tmp2.PhysicalPath);
    //System.Console.WriteLine("\t\t物理路径凭据：{0}", tmp2.Methods.ToString());
    System.Console.WriteLine("\t\t物理路径凭据登录类型：{0}", tmp2.LogonMethod.ToString());
}
}
}
}
 */
