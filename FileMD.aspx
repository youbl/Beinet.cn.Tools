<%@ Page Language="C#" EnableViewState="false" %>
<%@ Import Namespace="System.Collections.Generic" %>
<%@ Import Namespace="System.IO" %>
<%@ Import Namespace="System.Net" %>
<%@ Import Namespace="System.Security.Cryptography" %>

<script language="C#" runat="server">
    protected void Page_Load(object sender, EventArgs e)
    {
        string m_remoteIp = GetRemoteIp();
        string m_currentUrl = GetUrl(false);

        // 如果提交了ip参数，表示是请求Proxy
        string ip = Request.Form["ip"];
        if(!string.IsNullOrEmpty(ip))
        {
            string[] tmp = ip.Split('_');
            ip = tmp[0];
            string proxyurl;
            if (tmp.Length >= 2)
            {
                proxyurl = "http://" + tmp[1] + "/";
                if (tmp.Length >= 3)
                    proxyurl += tmp[2];
                else
                    proxyurl += m_currentUrl.Substring(m_currentUrl.LastIndexOf('/') + 1);
            }
            else
                proxyurl = m_currentUrl;
            string para = HttpUtility.UrlDecode(Request.Form.ToString());
            para = Regex.Replace(para, @"(?:^|&)ip=[^&]+", "");
            para += "&cl=" + m_remoteIp;
            byte[] arrBin = GetPage(proxyurl, para, ip);
            string contentType = Request.QueryString["contentType"];
            if (contentType != null)
            {
                if(contentType == "down")
                {
                    Response.AppendHeader("Content-Disposition", "attachment;filename=tmp");
                    Response.ContentType = "application/unknown";
                }
                else if (contentType == "text")
                {
                    Response.ContentType = "text/plain"; //"text/html";
                }
            }
            Response.BinaryWrite(arrBin);
            Response.End();
            return;
        }
        DoGetMd5();
    }

    private void DoGetMd5()
    {
        string dir = Request.Form["d"];
        if (dir == null || (dir = dir.Trim()) == string.Empty)
            return;
        if (dir.Length == 2)
            dir += "\\"; // 处理e:这样的格式,因为e:是当前站点根目录，不是磁盘根目录
        //获取指定目录下所有文件的md5值，用于接口调用
        Dictionary<string, string> md5s = new Dictionary<string, string>();
        string err = GetFileMd5(dir, md5s, dir);
        Response.ContentType = "text/plain";
        if (!string.IsNullOrEmpty(err))
        {
            Response.Write("出错了," + err + "\r\n");
        }
        foreach (KeyValuePair<string, string> pair in md5s)
        {
            Response.Write(pair.Key + "," + pair.Value + "\r\n");
        }

    }

    protected string GetFileMd5(string dirPath, Dictionary<string, string> md5s, string root)
    {
        if (string.IsNullOrEmpty(dirPath) || !Directory.Exists(dirPath))
        {
            return dirPath + " 目录不存在";
        }
        try
        {
            foreach (string file in Directory.GetFiles(dirPath))
            {
                md5s.Add(file.Replace(root, ""), GetMD5(file));
            }
            foreach (string dir in Directory.GetDirectories(dirPath))
            {
                string ret = GetFileMd5(dir, md5s, root);
                if (ret != string.Empty)
                {
                    return ret;
                }
            }
            return string.Empty;
        }
        catch (Exception exp)
        {
            return dirPath + " 子目录或文件列表获取失败:" + exp;
        }
    }
    private string GetMD5(string path)
    {
        try
        {
            using (MD5CryptoServiceProvider get_md5 = new MD5CryptoServiceProvider())
            using (FileStream get_file = new FileStream(path, FileMode.Open, FileAccess.Read, FileShare.Read))
            {
                return BitConverter.ToString(get_md5.ComputeHash(get_file)).Replace("-", "");
            }
        }
        catch (Exception exp)
        {
            return exp.Message;
        }
    }
    /// <summary>
    /// 获取当前访问的页面的完整URL，如http://sj.com/dir/a.aspx
    /// </summary>
    /// <param name="getQueryString"></param>
    /// <returns></returns>
    static string GetUrl(bool getQueryString)
    {
        string url = HttpContext.Current.Request.ServerVariables["SERVER_NAME"];

        if (HttpContext.Current.Request.ServerVariables["SERVER_PORT"] != "80")
            url += ":" + HttpContext.Current.Request.ServerVariables["SERVER_PORT"];

        url += HttpContext.Current.Request.ServerVariables["SCRIPT_NAME"];

        if (getQueryString)
        {
            if (HttpContext.Current.Request.QueryString.ToString() != "")
            {
                url += "?" + HttpContext.Current.Request.QueryString;
            }
        }

        string https = HttpContext.Current.Request.ServerVariables["HTTPS"];
        if (string.IsNullOrEmpty(https) || https == "off")
        {
            url = "http://" + url;
        }
        else
        {
            url = "https://" + url;
        }
        return url;
    }
    // 获取远程IP列表
    static string GetRemoteIp()
    {
        string ip = HttpContext.Current.Request.ServerVariables["REMOTE_ADDR"];
        if (ip != null && ip.StartsWith("10."))
        {
            string realIp = HttpContext.Current.Request.ServerVariables["HTTP_X_REAL_IP"];
            if (realIp != null && (realIp = realIp.Trim()) != string.Empty)
                ip = realIp;
        }
        return ip;
    }

    // POST获取网页内容
    static byte[] GetPage(string url, string param, string proxy)
    {
        HttpWebRequest request = (HttpWebRequest)WebRequest.Create(url);
        request.Headers.Add(HttpRequestHeader.CacheControl, "no-cache");
        request.Headers.Add("Accept-Charset", "utf-8");
        request.UserAgent = "Mozilla/4.0 (compatible; MSIE 8.0; Windows NT 6.1; Trident/4.0;)";
        request.AllowAutoRedirect = true; //出现301或302之类的转向时，是否要转向
        if (!string.IsNullOrEmpty(proxy))
        {
            string[] tmp = proxy.Split(new[] { ':' }, StringSplitOptions.RemoveEmptyEntries);
            int port = 80;
            if (tmp.Length >= 2)
                if (!int.TryParse(tmp[1], out port))
                    port = 80;
            request.Proxy = new WebProxy(tmp[0], port);
        }
        request.Method = "POST";
        request.ContentType = "application/x-www-form-urlencoded";
        // 设置提交的数据
        if (!string.IsNullOrEmpty(param))
        {
            // 把数据转换为字节数组
            byte[] l_data = Encoding.UTF8.GetBytes(param);
            request.ContentLength = l_data.Length;
            // 必须先设置ContentLength，才能打开GetRequestStream
            // ContentLength设置后，reqStream.Close前必须写入相同字节的数据，否则Request会被取消
            using (Stream newStream = request.GetRequestStream())
            {
                newStream.Write(l_data, 0, l_data.Length);
                newStream.Close();
            }
        }
        using (HttpWebResponse response = (HttpWebResponse)request.GetResponse())
        using (Stream stream = response.GetResponseStream())
        {
            if (stream == null)
                return new byte[0];
            //using (var sr = new StreamReader(stream, Encoding.UTF8))
            //{
            //    return sr.ReadToEnd();
            //}
            List<byte> ret = new List<byte>(10000);
            byte[] arr = new byte[10000];
            int readcnt;
            while ((readcnt = stream.Read(arr, 0, arr.Length)) > 0)
            {
                for (int i = 0; i < readcnt; i++)
                    ret.Add(arr[i]);
                //ret.AddRange(arr.Take(readcnt));
            }
            return ret.ToArray();
        }
    }
</script>