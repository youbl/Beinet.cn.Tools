using System;
using System.Collections.Generic;
using System.IO;
using System.Text.RegularExpressions;
using System.Threading;
using System.Runtime.Serialization;

namespace Beinet.cn.Tools.LvsManager
{
    public static class Common
    {
        private static readonly string _configfile = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "LvsServer.xml");
        /// <summary>
        /// 读取或保存服务器配置
        /// </summary>
        public static List<Site> Sites
        {
            get
            {
                if (!File.Exists(_configfile))
                    return new List<Site>();

                var ret = Utility.XmlDeserialize<List<Site>>(_configfile);
                ret.Sort((x, y) => String.Compare(x.name, y.name, StringComparison.Ordinal));
                return ret;
            }
            private set
            {
                if (value != null)
                {
                    Utility.XmlSerialize(value, _configfile);
                }
            }
        }

        //保存服务器清单
        public static void SaveServers(List<Site> sites)
        {
            Sites = sites;
        }


        static readonly Regex regUrl = new Regex(@"^https?://([^/]+)", RegexOptions.IgnoreCase);
        public static string GetHost(string url)
        {
            if (url == null || (url = url.Trim()) == string.Empty)
            {
                return null;
            }
            Match match = regUrl.Match(url);
            if (match.Success)
                return match.Groups[1].Value;
            return null;
        }
    }

    [DataContract]
    public class Site
    {
        /// <summary>
        /// 域名
        /// </summary>
        [DataMember(EmitDefaultValue = false, IsRequired = false, Order = 0)]
        public string name { get; set; }
        /// <summary>
        /// 下架时间间隔定义
        /// </summary>
        [DataMember(EmitDefaultValue = false, IsRequired = false, Order = 1)]
        public int offlineSecond { get; set; }
        /// <summary>
        /// 刷新间隔，秒
        /// </summary>
        [DataMember(EmitDefaultValue = false, IsRequired = false, Order = 2)]
        public int resreshSecond { get; set; }
        /// <summary>
        /// 刷新用的url
        /// </summary>
        [DataMember(EmitDefaultValue = false, IsRequired = false, Order = 3)]
        public string url { get; set; }
        /// <summary>
        /// ip清单
        /// </summary>
        [DataMember(EmitDefaultValue = false, IsRequired = false, Order = 4)]
        public List<string> Servers { get; set; }

        private int _currentIndex = -1;
        /// <summary>
        /// 按顺序获取其中一个ip地址，线程安全
        /// </summary>
        /// <returns></returns>
        public string PopServer()
        {
            int currentIndex = Interlocked.Increment(ref _currentIndex);
            if (currentIndex < Servers.Count)
            {
                return Servers[currentIndex];
            }
            else
            {
                Interlocked.Exchange(ref _currentIndex, -1);
            }
            return string.Empty;
        }
    }
}
