using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Net;
using System.Text;
using System.Text.RegularExpressions;
using System.Windows.Forms;

namespace Beinet.cn.Tools.GetIpFromBaidu
{
    public partial class GetIp : Form
    {
        public GetIp()
        {
            InitializeComponent();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            string ip = textBox1.Text;
            string html = GetNet(ip);
            textBox2.Text = html;
            //textBox3.Text = string.Empty;

            //Match match = Regex.Match(html, @"(?i)(?<=来[^<>]+自[^<>]+<strong>)[^<]*");
            //if(match.Success)
            //{
            //    textBox3.Text = match.Value;
            //}
        }

        static Regex _reg = new Regex(@"(?i)(?<=来[^<>]+自[^<>]+<strong>)[^<]*", RegexOptions.Compiled);
        private string GetNet(string ip)
        {
            string url =
                @"http://www.baidu.com/s?wd=" + ip + 
                "&rsv_bp=0&ch=&tn=baidu&bar=&rsv_spt=3&ie=utf-8&rsv_sug3=14&rsv_sug=0&rsv_sug1=10&rsv_sug4=1602&inputT=10463&" + 
                Guid.NewGuid().GetHashCode();

            HttpWebRequest request = (HttpWebRequest)WebRequest.Create(url);
            request.Headers.Add(HttpRequestHeader.CacheControl, "no-cache");
            request.UserAgent = "Mozilla/5.0 (Windows NT 6.1; WOW64; rv:20.0) Gecko/20100101 Firefox/20.0";
            request.Referer = "http://www.baidu.com/";
            request.AllowAutoRedirect = true; //出现301或302之类的转向时，是否要转向
            
            request.Method = "GET";
            request.ContentType = "application/x-www-form-urlencoded";
            string html;
            using (HttpWebResponse response = (HttpWebResponse)request.GetResponse())
            using (Stream stream = response.GetResponseStream())
            {
                if (stream == null)
                    return null;
                using (var sr = new StreamReader(stream, Encoding.UTF8))
                {
                    html = sr.ReadToEnd();
                }
            }

            Match match = _reg.Match(html);
            if (match.Success)
            {
                string full = match.Value;

                //textBox3.Text = full;

                if(full.IndexOf("电信", StringComparison.Ordinal) >= 0)
                {
                    return "电信";
                }
                if (full.IndexOf("铁通", StringComparison.Ordinal) >= 0)
                {
                    return "铁通";
                }
                if (full.IndexOf("联通", StringComparison.Ordinal) >= 0)
                {
                    return "联通";
                }

                return full;
            }
            return null;
        }
    }
}
