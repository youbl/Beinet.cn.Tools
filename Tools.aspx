<%@ Page Language="C#" EnableViewState="false" %>
<%@ Import Namespace="System.Data" %>
<%@ Import Namespace="System.Data.SqlClient" %>
<%@ Import Namespace="System.IO" %>
<%@ Import Namespace="System.Net" %>
<%@ Import Namespace="System.Net.Sockets" %>
<%@ Import Namespace="System.Reflection" %>
<%@ Import Namespace="System.Runtime.Serialization.Json" %>
<%@ Import Namespace="System.Security.Cryptography" %>
<%@ Import Namespace="System.Threading" %>
<%@ Import Namespace="System.Xml" %>
<%@ Import Namespace="System.Collections.Generic" %>
<%@ Import Namespace="System.Web.Configuration" %>
<%@ Import Namespace="ICSharpCode.SharpZipLib.Zip" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">

<html xmlns="http://www.w3.org/1999/xhtml">
<head>
<title>CustomConfig页面</title>

<!-- 要自定义的变量列表 -->    
<script language="c#" runat="server">
    // **********************＝＝＝＝＝重要＝＝＝＝＝***************************
    // 因为需要压缩，bin目录下必须有ICSharpCode.SharpZipLib.Zip.dll

    //下面替换为你需要用的md5值，比如e10adc3949ba59abbe56e057f20f883e是123456
    private const string _pwd = "e10adc3949ba59abbe56e057f20f883e"; // ip在白名单里时，进入此页面的密码md5值
    protected string _xxx = "aaa42296669b958c3cee6c0475c8093e";     // ip不在白名单时，进入此页面的密码md5值
    //要显示在 CustomConfig配置文本框里的ip列表
    private string m_ipLst = "127.0.0.1;";

    private const string m_tmpdir = @"e:\upload";// 可以写入的目录，用于创建zip文件等临时操作

</script>
<script language="C#" runat="server">
    private string m_currentUrl;
    private string m_localIp, m_remoteIp, m_remoteIpLst;
    protected override void OnInit(EventArgs e)
    {
        try 
        { 
            m_localIp = GetServerIpList();
            m_remoteIp = GetRemoteIp();
            m_remoteIpLst = GetRemoteIpLst();
            m_currentUrl = GetUrl(false);
            
            Log("客户端ip：" + m_remoteIpLst +
                "\r\n服务器ip：" + m_localIp +
                "\r\nUrl：" + Request.Url.ToString() +
                "\r\nPost：" + Request.Form.ToString(), 
                "", null);
            
            if (string.IsNullOrEmpty(_pwd))
            {
                Response.Write("未设置密码，请修改页面源码以设置密码\r\n" +
                               m_remoteIp + ";" + m_localIp);
                Response.End();
                return;
            }

            // 检查md5不需要密码flg == "clientdirmd5"
            if ((Request.Form["f"] ?? "") != "clientdirmd5" && !IsLogined(m_remoteIp))
            {
                Response.Write(Request.QueryString + "\r\n<hr />\r\n" + Request.Form.ToString() + "\r\n<hr />\r\n" +
                               m_remoteIp + ";" + m_localIp);
                Response.End();
                return;
            }

            // 文件上传，特殊处理
            string flg = Request.QueryString["flg"];
            if(!string.IsNullOrEmpty(flg) && flg == "fileupload")
            {
                UploadFile(m_remoteIpLst, m_localIp);
                return;
            }
            
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
                para = System.Text.RegularExpressions.Regex.Replace(para, @"(?:^|&)ip=[^&]+", "");
                para += "&p=" + GetSession("p") + "&cl=" + m_remoteIp;
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

            flg = Request.Form["flg"];
            if(!string.IsNullOrEmpty(flg))
            {
                flg = flg.Trim().ToLower();
                switch (flg)
                {
                    case "telnet":
                        Telnet();
                        break;
                    case "sql":
                        SqlRun();
                        break;
                    case "redis":
                        Redis();
                        break;
                    case "filemanager":
                        DoFileManager();
                        break;
                }
                Response.End();
            }
        }
        catch (ThreadAbortException) { }
        catch (Exception exp)
        {
            Response.Write("客户ip：" + m_remoteIpLst + "；服务器：" + m_localIp + "\r\n" + exp);
            Response.End();
        }
    }

    // 判断是否登录
    protected bool IsLogined(string ip)
    {
        //if (ip.StartsWith("127.") || ip == "::1")
        //    return true;
        // 是否内网ip
        bool isInner = (
            ip.StartsWith("192.168.") || ip.StartsWith("172.16.") || ip.StartsWith("10.") ||
            ip.StartsWith("127.") || ip == "::1");
        
        bool redirect = false;
        string str = Request.QueryString["p"];
        if (!string.IsNullOrEmpty(str))
            redirect = true;

        if (!string.IsNullOrEmpty(str))
        {
            str = FormsAuthentication.HashPasswordForStoringInConfigFile(str, "MD5");
            SetSession("p", str);
        }
        else
            str = GetSession("p");

        // ip proxy通过Form提交加密好的密码,proxy只允许是内网ip
        if (string.IsNullOrEmpty(str) && isInner)
            str = Request.Form["p"];

        if (string.IsNullOrEmpty(str))
            return false;
        if (str.Equals(_pwd, StringComparison.OrdinalIgnoreCase))
        {
            if (isInner)
            {
                if (redirect)
                {
                    Response.Redirect(m_currentUrl);
                }
                return true;
            }
        }
        else if (str.Equals(_xxx, StringComparison.OrdinalIgnoreCase))
        {
            if (redirect)
            {
                Response.Redirect(m_currentUrl);
            }
            return true;
        }
        return false;
    }
    
    static object lockobj = new object();
    // 每个操作都要求写入日志
    static void Log(string msg, string prefix, string filename)
    {
        DateTime now = DateTime.Now;
        if (string.IsNullOrEmpty(filename))
        {
            filename = @"e:\weblogs\zzCustomConfigLog\" + prefix + "\\" + now.ToString("yyyyMMddHH") + ".txt";
        }
        string dir = Path.GetDirectoryName(filename);
        if (dir != null && !Directory.Exists(dir))
            Directory.CreateDirectory(dir);
        lock (lockobj)
        {
            using (StreamWriter sw = new StreamWriter(filename, true, Encoding.UTF8))
            {
                sw.WriteLine(now.ToString("yyyy-MM-dd HH:mm:ss_fff"));
                sw.WriteLine(msg);
                sw.WriteLine();
            }
        }
    }

    </script>    

<!-- Telnet配置相关的方法集 -->    
<script language="c#" runat="server">
    void Telnet()
    {
        StringBuilder sb = new StringBuilder();
        string ips = Request.Form["tip"] ?? "";
        IPAddress ip;
        int port;
        string[] tmp;
        foreach (string item in ips.Split(new char[] { '\r', '\n' }, StringSplitOptions.RemoveEmptyEntries))
        {
            tmp = item.Trim().Split(new char[] { ':', ' ' }, StringSplitOptions.RemoveEmptyEntries);
            if (tmp.Length != 2)
            {
                sb.AppendFormat("{0} 不是ip:端口\r\n", item);
                continue;
            }
            if(!IPAddress.TryParse(tmp[0], out ip) || !int.TryParse(tmp[1], out port))
            {
                sb.AppendFormat("{0} ip格式错误或端口不是数字\r\n", item);
                continue;
            }
            DateTime start = DateTime.Now;
            try
            {
                IPEndPoint serverInfo = new IPEndPoint(ip, port);
                using (Socket socket = new Socket(serverInfo.AddressFamily, SocketType.Stream, ProtocolType.Tcp))
                {
                    //socket.BeginConnect(serverInfo, CallBackMethod, socket);
                    socket.Connect(serverInfo);
                    if (socket.Connected)
                    {
                        sb.AppendFormat("{0} 连接正常({1:N0}ms)\r\n", item, (DateTime.Now - start).TotalMilliseconds);
                    }
                    else
                    {
                        sb.AppendFormat("{0} 连接失败({1:N0}ms)\r\n", item, (DateTime.Now - start).TotalMilliseconds);
                    }
                    socket.Close();
                }
            }
            catch (Exception exp)
            {
                sb.AppendFormat("{0} 连接出错({2}ms) {1:N0}\r\n", item, exp.Message, (DateTime.Now - start).TotalMilliseconds);
            }
        }
        sb.AppendFormat("\r\n（客户ip：{0}；服务器：{1}）", m_remoteIpLst, m_localIp);
        Response.Write(sb.ToString());
    }
</script>

<!-- Sql测试方法集 -->    
<script language="C#" runat="server">
    void SqlRun()
    {
        string prefix = "（客户ip：" + m_remoteIpLst + "；服务器：" + m_localIp + "）<br />\r\n";
        string sql = Request.Form["sql"];
        string constr = Request.Form["constr"];
        if (!string.IsNullOrEmpty(sql) && !string.IsNullOrEmpty(constr))
        {
            //if (!sql.StartsWith("select ", StringComparison.OrdinalIgnoreCase))
            if (!Regex.IsMatch(sql, @"^(?i)select\s+top(?:\s|\()"))
            {
                Response.Write(prefix + "只允许Select语句，且必须包含Top子句，以免返回记录数太多");
                return;
            }
            DataTable dt = new DataTable();
            using (SqlConnection con = new SqlConnection(constr))
            using (SqlCommand command = con.CreateCommand())
            using (SqlDataAdapter dataAdapter = new SqlDataAdapter(command))
            {
                con.Open();
                command.CommandText = sql;
                dataAdapter.Fill(dt);
                con.Close();
            }
            GridView gv1 = new GridView();
            gv1.DataSource = dt;
            gv1.DataBind();
            Response.Write(prefix + GetHtml(gv1));
        }
        else
        {
            Response.Write(prefix + "没有输入Sql或连接串");
        }
    }
    /// <summary>
    /// 返回 Web服务器控件的HTML 输出
    /// </summary>
    /// <param name="ctl"></param>
    /// <returns></returns>
    public static string GetHtml(Control ctl)
    {
        if (ctl == null)
            return string.Empty;

        using (StringWriter sw = new StringWriter())
        using (HtmlTextWriter htw = new HtmlTextWriter(sw))
        {
            ctl.RenderControl(htw);
            return sw.ToString();
        }
    }
</script>

<!-- Redis管理方法集 -->    
<script language="c#" runat="server">
    void Redis()
    {
        string tip = Request.Form["tip"] ?? "";
        int pwdSplit = tip.LastIndexOf('@');
        string pwd = null;
        if (pwdSplit >= 0)
        {
            pwd = tip.Substring(0, pwdSplit);
            tip = tip.Substring(pwdSplit + 1);
        }
        string[] tmparr = tip.Split(':');
        string ipStr, portStr;
        if (tmparr.Length == 2)
        {
            ipStr = tmparr[0];
            portStr = tmparr[1];
        }
        else
        {
            ipStr = tmparr[0];
            portStr = "6379";
        }
        IPAddress ip;
        int port;
        if (!IPAddress.TryParse(ipStr, out ip) || !int.TryParse(portStr, out port))
        {
            Response.Write(tip + " ip格式错误或端口不是数字");
            return;
        }

        List<byte> arrAll = new List<byte>(1024);
        IPEndPoint serverInfo = new IPEndPoint(ip, port);
        using (Socket socket = new Socket(serverInfo.AddressFamily, SocketType.Stream, ProtocolType.Tcp))
        {
            //socket.BeginConnect(serverInfo, CallBackMethod, socket);
            socket.Connect(serverInfo);
            if (socket.Connected)
            {
                byte[] bytesReceived = new byte[1024];
                byte[] command;
                if(!string.IsNullOrEmpty(pwd))
                {
                    command = Encoding.UTF8.GetBytes("auth " + pwd + "\r\n");
                    socket.Send(command);
                    socket.Receive(bytesReceived, bytesReceived.Length, 0);
                }
                command = Encoding.UTF8.GetBytes((Request.Form["cm"] ?? "info") + "\r\n");
                socket.Send(command);

                int zeroCnt = 0;
                while (true)
                {
                    int tmp = socket.Receive(bytesReceived, bytesReceived.Length, 0);
                    if (tmp <= 0)
                    {
                        zeroCnt++;// 总共5次0字节时，退出
                        if (zeroCnt > 5)
                        {
                            break;
                        }
                    }
                    else
                    {
                        if (tmp == bytesReceived.Length)
                        {
                            arrAll.AddRange(bytesReceived);
                        }
                        else
                        {
                            byte[] tarr = new byte[tmp];
                            Array.Copy(bytesReceived, tarr, tmp);
                            arrAll.AddRange(tarr);
                            break;
                        }
                    }
                }
            }
            socket.Close();
        }
        Response.Write(Encoding.UTF8.GetString(arrAll.ToArray()));

        //sb.AppendFormat("\r\n（客户ip：{0}；服务器：{1}）", m_remoteIpLst, m_localIp);
    }
</script>

<!-- 文件管理相关方法 -->
<script language="C#" runat="server">
    void DoFileManager()
    {
        string dir = Request.Form["d"];
        if (dir.Length == 2)
            dir += "\\";// 处理e:这样的格式,因为e:是当前站点根目录，不是磁盘根目录
        
        string flg = Request.Form["f"];
        if (string.IsNullOrEmpty(flg))
            flg = "dir";
        if (flg == "clientdirmd5")
        {
            //获取指定目录下所有文件的md5值，用于接口调用
            Dictionary<string, string> md5s = new Dictionary<string, string>();
            string err = GetFileMd5(dir, md5s, dir);
            //string retJson = @"{""err"":""" + err + @""",""ip"":""" + m_localIp + @""",""data"":" + ToJson(md5s) + "};";
            //string callback = Request.QueryString["callback"];
            //if(!string.IsNullOrEmpty(callback))
            //{
            //    retJson = callback + "(" + retJson + ")";
            //}
            Response.ContentType = "text/plain";
            if (!string.IsNullOrEmpty(err))
            {
                Response.Write("出错了," + err + "\r\n");
            }
            foreach (KeyValuePair<string, string> pair in md5s)
            {
                Response.Write(pair.Key + "," + pair.Value + "\r\n");
            }
            return;
        }
        
        string filePrefix = "（客户ip：" + m_remoteIpLst + "；服务器：<span style='font-size:20px;color:green;'>" + m_localIp + "</span>）";
        string str = "<span style='font-weight:bold;color:blue;'>";
        int sort;
        int.TryParse(Request.Form["s"], out sort);
        bool showMd5 = (Request.Form["md5"] ?? "0") == "1";
        try
        {
            //else
            //    flg = flg.ToLower();
            switch (flg)
            {
                //case "dir":// 列目录
                //    break;
                case "dirReName":
                    str += BeinetReName(dir, false);
                    break;
                case "fileReName":
                    str += BeinetReName(dir, true);
                    break;
                case "dirDel":
                    str += BeinetDel(dir, false);
                    break;
                case "fileDel":
                    str += BeinetDel(dir, true);
                    break;
                case "dirSize":
                    BeinetCntSize(dir);
                    return;
                case "fileDown":
                    BeinetFileOpen(dir, true);
                    return;
                case "fileOpen":
                    BeinetFileOpen(dir, false);
                    return;
                case "fileZipDown":
                    ZipAndDown(dir);
                    return;
                case "fileMove":
                    str += FileMove(dir);
                    break;
                case "fileDelSelect":
                    str += FileDelSelect(dir);
                    break;
                case "fileDirClear":
                    str += FileDirClear(dir);
                    break;
                case "fileCreateDir":
                    str += FileCreateDir();
                    break;
                case "fileUnZip":
                    str += FileUnZip(dir);
                    break;
            }
        }
        catch (Exception exp)
        {
            str += exp.ToString();
        }
        str += "</span>\r\n" + filePrefix + Beinetget(dir, sort, showMd5);
        Response.Write(str);
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

    /// <summary>
    /// 获取目录和文件进行绑定
    /// </summary>
    /// <param name="dirPath"></param>
    /// <param name="sort">排序 10,11文件名；20,21扩展名；30,31文件大小；40,41修改时间</param>
    /// <param name="showMd5">是否显示文件MD5值</param>
    protected string Beinetget(string dirPath, int sort, bool showMd5)
    {
        if (string.IsNullOrEmpty(dirPath) || !Directory.Exists(dirPath))
        {
            return "<span style='font-weight:bold;color:red;'>" + dirPath + " 目录不存在</span>";
        }

        DirectoryInfo dirShow = new DirectoryInfo(dirPath);

        DirectoryInfo[] arrDir;
        FileInfo[] arrFile;
        try
        {
            arrDir = dirShow.GetDirectories("*");
            arrFile = dirShow.GetFiles("*.*");
        }
        catch (Exception exp)
        {
            return "<span style='font-weight:bold;color:red;'>" +dirPath + " 子目录或文件列表获取失败</span><br />\r\n" + exp;
        }
        // 排序
        Array.Sort(arrDir, delegate(DirectoryInfo a, DirectoryInfo b)
                               {
                                   switch (sort)
                                   {
                                       default:
                                           return String.Compare(a.Name, b.Name, StringComparison.OrdinalIgnoreCase);
                                       case 11:
                                           return -String.Compare(a.Name, b.Name, StringComparison.OrdinalIgnoreCase);
                                       case 40:
                                           return a.LastWriteTime.CompareTo(b.LastWriteTime);
                                       case 41:
                                           return -a.LastWriteTime.CompareTo(b.LastWriteTime);
                                   }
                               });
        Array.Sort(arrFile, delegate(FileInfo a, FileInfo b)
                                {
                                    switch (sort)
                                    {
                                        default: // 文件名正序
                                            return String.Compare(a.Name, b.Name, StringComparison.OrdinalIgnoreCase);
                                        case 11: // 文件名倒序
                                            return -String.Compare(a.Name, b.Name, StringComparison.OrdinalIgnoreCase);
                                        case 20: // 扩展名正序
                                            return String.Compare(Path.GetExtension(a.Name), Path.GetExtension(b.Name), StringComparison.OrdinalIgnoreCase);
                                        case 21: // 扩展名倒序
                                            return -String.Compare(Path.GetExtension(a.Name), Path.GetExtension(b.Name), StringComparison.OrdinalIgnoreCase);
                                        case 30: // 文件大小正序
                                            return a.Length.CompareTo(b.Length);
                                        case 31: // 文件大小倒序
                                            return -a.Length.CompareTo(b.Length);
                                        case 40: // 修改时间正序
                                            return a.LastWriteTime.CompareTo(b.LastWriteTime);
                                        case 41: // 修改时间倒序
                                            return -a.LastWriteTime.CompareTo(b.LastWriteTime);
                                    }
                                });
        StringBuilder sbRet = new StringBuilder("<span style='font-weight:bold;color:red;'>" + arrDir.Length + " dirs, " + arrFile.Length + " files. server time:" + 
            DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss_fff"));

        sbRet.Append(@"</span>
<table border='0' cellpadding='0' cellspacing='0' style='table-layout:fixed;' class='filetb' id='tbFileManager'>
<tr style='background-color:#96d9f9'>
<th style='width:130px; text-align:left;'>
    <label><input type='checkbox' onclick=""chgChkColor($('#divret input[type=checkbox]'), this.checked);"" />全选</label>&nbsp;
    <a href='#0' onclick='OpenCheckOption();'>条件</a>
    <br />
    <label><input type='checkbox' onclick='CheckAllDir(this);' />目录</label>
    <label><input type='checkbox' onclick='CheckAllFile(this);' />文件</label>
</th>
<th style='width:370px; text-align:left;'>
    [<a href='#0' onclick='fileOpenDir(0);'>上级目录</a>]
    [<a href='#0' onclick='fileOpenDir(1);'>根目录</a>]
</th>
<th style='width:50px;'>扩展名</th>
<th style='width:70px;'>大小(byte)</th>
<th style='width:150px;'>修改日期</th>" + (showMd5 ? "<th>MD5</th>" : "") + @"<th>序<br/>号</th>
</tr>
");
        int idx = 1;
        string[] colors = new string[] { "#ffffff", "#dadada" };//f0f0f0
        // 绑定目录列表
        foreach (DirectoryInfo dir in arrDir)
        {
            sbRet.AppendFormat(@"
<tr style='height:20px;background-color:{4}' onmouseover='onRowOver(this);' onmouseout='onRowOut(this);' onclick='onRowClick(this);'>
<td style='text-align:left;'>
    <label><input type='checkbox' value='{0}' name='chkDirListBeinet' /></label>
    <a href='#0' onclick=""fileReName('{0}', 'dirReName');"" tabindex='-1'>改</a>|<a href='#0' onclick=""fileDel('{0}', 'dirDel');"" tabindex='-1'>删</a>
</td>
<td style='text-align:left; font-weight:bold;'><a href='#0' onclick=""fileOpenDir('{0}');"" tabindex='-1'>{0}</a></td>
<td style='text-align:center;'>目录</td>
<td style='text-align:center;'><a href='#0' onclick=""countDirSize('{1}');"" tabindex='-1'>计算大小</a></td>
<td style='text-align:center;'>{2}</td>" + (showMd5 ? "<td></td>" : "") + @"<th>{3}</th>
</tr>
",
            dir.Name, dir.FullName.Replace(@"\", @"\\"), dir.LastWriteTime.ToString("yyyy-MM-dd HH:mm:ss.fff"), idx, colors[idx % 2]);
            idx++;
        }
        // 绑定文件列表
        foreach (FileInfo file in arrFile)
        {
            sbRet.AppendFormat(@"
<tr style='height:22px;background-color:{7}' onmouseover='onRowOver(this);' onmouseout='onRowOut(this);' onclick='onRowClick(this);'>
<td style='text-align:left;'>
    <label><input type='checkbox' value='{0}' name='chkFileListBeinet' /></label>
    <a href='#0' onclick=""fileReName('{0}', 'fileReName');"" tabindex='-1'>改</a>|<a href='#0' 
onclick=""fileDel('{0}', 'fileDel');"" tabindex='-1'>删</a>|<a href='#0' 
onclick='fileDownOpen(""{4}"", true);' tabindex='-1'>下</a>|<a href='#0' 
onclick='fileDownOpen(""{4}"", false, 1);' tabindex='-1'>GB</a>|<a href='#0' 
onclick='fileDownOpen(""{4}"", false, 2);' tabindex='-1'>UTF8</a>
</td>
<td style='text-align:left;'>{0}</td>
<td style='text-align:center;'>{1}</td>
<td style='text-align:right;'>{2}</td>
<td style='text-align:center;'>{3}</td>{5}
<th>{6}</th>
</tr>
",
            file.Name, 
            Path.GetExtension(file.Name), 
            file.Length.ToString("N0"), 
            file.LastWriteTime.ToString("yyyy-MM-dd HH:mm:ss.fff"),
            file.FullName.Replace(@"\", @"\\"),
            (showMd5 ? "<td style='text-align:right;'>" + GetMD5(file.FullName) + "</th>" : ""),
            idx, colors[idx % 2]);
            idx++;
        }
        return (sbRet.ToString());
    }

    string BeinetReName(string dirPath, bool isFile)
    {
        string nameOld = Request.Form["old"];
        string nameNew = Request.Form["new"];
        if (string.IsNullOrEmpty(nameNew) || string.IsNullOrEmpty(nameOld))
            return "参数错误";

        nameOld = Path.Combine(dirPath, nameOld);
        nameNew = Path.Combine(dirPath, nameNew);
        if (File.Exists(nameNew) || Directory.Exists(nameNew))
        {
            return nameNew + " 同名文件或目录已经存在";
        }
        try
        {
            if (isFile)
                File.Move(nameOld, nameNew);
            else
                Directory.Move(nameOld, nameNew);
            return "重命名成功";
        }
        catch (Exception exp)
        {
            return (nameOld + " 移动失败\r\n<br />\r\n" + exp);
        }
    }
    
    string BeinetDel(string dirPath, bool isFile)
    {
        string nameOld = Request.Form["old"];
        if (string.IsNullOrEmpty(nameOld))
            return "参数错误";

        nameOld = Path.Combine(dirPath, nameOld);
        if ((isFile && !File.Exists(nameOld)) || (!isFile && !Directory.Exists(nameOld)))
        {
            return nameOld + " 文件或目录不存在";
        }
        try
        {
            if (isFile)
                File.Delete(nameOld);
            else
                Directory.Delete(nameOld, true);
            return "删除成功";
        }
        catch (Exception exp)
        {
            return (nameOld + " 删除失败\r\n<br />\r\n" + exp);
        }
    }
    void BeinetCntSize(string dir)
    {
        if(!string.IsNullOrEmpty(dir) && Directory.Exists(dir))
        {
            int cntFile = 0, cntDir = 0;
            long size = GetDirSize(dir, ref cntFile, ref cntDir);
            StringBuilder sb = new StringBuilder(200);
            sb.AppendFormat(@"<script type='text/javascript'>
alert('（客户ip：{0}；服务器：{1}）\r\n{6}\r\n\r\n大小：{5}({2:N0}字节)\r\n包含：{3:N0}个文件，{4:N0}个目录');</",
                            m_remoteIpLst, m_localIp, size, cntFile, cntDir, TransSize(size), dir.Replace(@"\", @"\\"));
            sb.Append("script>");

            Response.Write(sb.ToString());
        }
        else
        {
            if (dir == null) dir = string.Empty;
            Response.Write("<script type='text/javascript'>alert('" + dir.Replace(@"\", @"\\") + "目录不存在');</" + "script>");
        }
    }
    void BeinetFileOpen(string file, bool isDown)
    {
        if (!string.IsNullOrEmpty(file) && File.Exists(file))
        {
            string en = Request.Form["en"];
            if (string.IsNullOrEmpty(en))
                en = "GB2312";
            Encoding encode = Encoding.GetEncoding(en);
            Response.Buffer = true;
            Response.Clear();
            Response.Charset = encode.BodyName;
            Response.ContentEncoding = encode;//System.Text.Encoding.UTF8;

            if (isDown)
            {
                Response.AppendHeader("Content-Disposition",
                                      "attachment;filename=" + HttpUtility.UrlEncode(Path.GetFileName(file)));
                Response.ContentType = "application/unknown";
            }
            else
            {
                Response.ContentType = "text/plain"; //"text/html";
            }
            Response.Flush();
            if (Response.IsClientConnected)
                Response.WriteFile(file);
        }
        else
        {
            if (file == null) file = string.Empty;
            Response.Write("<script type='text/javascript'>alert('" + file.Replace(@"\", @"\\") + "不存在');</" + "script>");
        }
    }

    protected void ZipAndDown(string dirPath)
    {
        string zipfilepath = Path.Combine(m_tmpdir, @"ziptmp.zip");
        if (File.Exists(zipfilepath))
            File.Delete(zipfilepath);

        string[] files = (Request.Form["file"] ?? string.Empty).Split(new char[] { ',' }, StringSplitOptions.RemoveEmptyEntries);
        string[] dirs = (Request.Form["dir"] ?? string.Empty).Split(new char[] { ',' }, StringSplitOptions.RemoveEmptyEntries);
        if (files.Length <= 0 && dirs.Length <= 0)
        {
            Response.Write("<script type='text/javascript'>alert('未提交参数');</" + "script>");
            return;
        }

        Response.AppendHeader("Content-Disposition", "attachment;filename=ziptmp.zip");
        Response.ContentType = "application/unknown";
        if (files.Length > 0)
        {
            string[] pathfiles = new string[files.Length];
            int idx = 0;
            foreach (string file in files)
            {
                pathfiles[idx] = Path.Combine(dirPath, file);
                idx++;
            }
            ZipFiles(zipfilepath, pathfiles);
            if (Response.IsClientConnected)
                Response.WriteFile(zipfilepath);
            return;
        }

        string[] pathdirs = new string[dirs.Length];
        int idx2 = 0;
        foreach (string file in dirs)
        {
            pathdirs[idx2] = Path.Combine(dirPath, file);
            idx2++;
        }
        ZipDirs(zipfilepath, pathdirs);
        if (Response.IsClientConnected)
            Response.WriteFile(zipfilepath);
    }

    string FileMove(string dirPath)
    {
        string[] files = (Request.Form["file"] ?? string.Empty).Split(new char[] { ',' }, StringSplitOptions.RemoveEmptyEntries);
        string[] dirs = (Request.Form["dir"] ?? string.Empty).Split(new char[] { ',' }, StringSplitOptions.RemoveEmptyEntries);
        string dirTo = Request.Form["to"];
        if ((files.Length <= 0 && dirs.Length <= 0) || string.IsNullOrEmpty(dirTo))
        {
            return "未提交参数";
        }

        if (!Directory.Exists(dirTo))
            Directory.CreateDirectory(dirTo);
        foreach (string file in files)
        {
            string mf = Path.Combine(dirPath, file);
            string to = Path.Combine(dirTo, file);
            if (File.Exists(to))
                File.Delete(to);
            File.Move(mf, to);
        }
        int dirFiles = 0;
        foreach (string dir in dirs)
        {
            string mf = Path.Combine(dirPath, dir);
            string to = Path.Combine(dirTo, dir);
            string msg = string.Empty;
            dirFiles = DirMove(mf, to, ref msg);
            if (dirFiles < 0)
                return msg;
        }
        return "移动了" + files.Length + "个文件，" + dirs.Length + "个目录下的" + dirFiles + "个文件";
    }

    string FileDelSelect(string dirPath)
    {
        string[] files = (Request.Form["file"] ?? string.Empty).Split(new char[] { ',' }, StringSplitOptions.RemoveEmptyEntries);
        string[] dirs = (Request.Form["dir"] ?? string.Empty).Split(new char[] { ',' }, StringSplitOptions.RemoveEmptyEntries);
        if (files.Length <= 0 && dirs.Length <= 0)
        {
            return "未提交参数";
        }

        foreach (string file in files)
        {
            string mf = Path.Combine(dirPath, file);
            if (File.Exists(mf))
                File.Delete(mf);
        }
        foreach (string dir in dirs)
        {
            string mf = Path.Combine(dirPath, dir);
            if (Directory.Exists(mf))
                Directory.Delete(mf, true);
        }
        return "删除了" + files.Length + "个文件和" + dirs.Length + "个目录";
    }
    string FileDirClear(string dirPath)
    {
        string clearTime = Request.Form["tm"];
        DateTime dtClear;
        if (!string.IsNullOrEmpty(clearTime) && DateTime.TryParse(clearTime, out dtClear))
        {
            bool includeSub = !string.IsNullOrEmpty(Request.Form["subdir"]);
            bool delEmptySubdir = !string.IsNullOrEmpty(Request.Form["delem"]);
            string msg = null;
            int cntFile = DirClear(dirPath, dtClear, includeSub, delEmptySubdir, ref msg);
            if (cntFile >= 0)
                return("成功删除" + cntFile + "个文件" + clearTime);
            return msg;
        }
        else
            return("时间错误" + clearTime);
    }
    string FileCreateDir()
    {
        string newdir = Request.Form["dir"];
        if (string.IsNullOrEmpty(newdir))
            return "没有参数";
        if (Directory.Exists(newdir))
            return newdir + "目录已存在";
        Directory.CreateDirectory(newdir);
        return "创建成功";
    }

    string FileUnZip(string dir)
    {
        string zipfile = Request.Form["zipfile"];
        if (string.IsNullOrEmpty(zipfile))
            return "没有参数";
        zipfile = Path.Combine(dir, zipfile);
        if (!File.Exists(zipfile))
            return zipfile + "文件不存在";
        int cnt = UnZipFile(zipfile, null);
        return "解压成功" + cnt + "个文件";
    }
    /// <summary>
    /// 获取指定目录大小
    /// </summary>
    /// <param name="dir"></param>
    /// <param name="cntFile">文件个数</param>
    /// <param name="cntDir">目录个数</param>
    protected long GetDirSize(string dir, ref int cntFile, ref int cntDir)
    {
        long ret = 0;
        // 递归访问全部子目录
        foreach (string subdir in Directory.GetDirectories(dir))
        {
            ret += GetDirSize(subdir, ref cntFile, ref cntDir);
            cntDir++;
        }
        // 访问全部文件
        foreach (string subfile in Directory.GetFiles(dir))
        {
            ret += new FileInfo(subfile).Length;
            cntFile++;
        }
        return ret;
    }
    /// <summary>
    /// 把字节大小转换成最合适的显示大小
    /// </summary>
    /// <param name="size"></param>
    /// <returns></returns>
    protected string TransSize(long size)
    {
        int len = size.ToString().Length;
        switch (len)
        {
            default:
                return string.Empty;
            case 4:
            case 5:
            case 6:
                return (size / 1024m).ToString("N") + "KB";
            case 7:
            case 8:
            case 9:
                return (size / 1024m / 1024m).ToString("N") + "MB";
            case 10:
            case 11:
            case 12:
                return (size / 1024m / 1024m / 1024m).ToString("N") + "GB";
            case 13:
            case 14:
            case 15:
                return (size / 1024m / 1024m / 1024m / 1024m).ToString("N") + "TB";
        }
    }

    // 删除目录下全部文件，includeSub时子目录下文件也全部删除，delEmptySubdir时，删除空子目录
    protected int DirClear(string dir, DateTime dt, bool includeSub, bool delEmptySubdir, ref string msg)
    {
        int cntFile = 0;
        // 删除全部文件
        foreach (string subfile in Directory.GetFiles(dir))
        {
            try
            {
                if (File.GetLastWriteTime(subfile) < dt)
                {
                    File.Delete(subfile);
                    cntFile++;
                }
            }
            catch (Exception exp)
            {
                msg =(subfile + " 文件删除失败\r\n<br />\r\n" + exp);
                return -1;
            }
        }
        // 递归删除全部子目录
        if (includeSub)
        {
            foreach (string subdir in Directory.GetDirectories(dir))
            {
                int cntTmp = DirClear(subdir, dt, true, delEmptySubdir, ref msg);
                if (cntTmp < 0)
                    return -1;
                cntFile += cntTmp;
                // 清除空目录
                if (delEmptySubdir && Directory.GetFiles(subdir).Length == 0 && Directory.GetDirectories(subdir).Length == 0)
                    Directory.Delete(subdir);
            }
        }

        return cntFile;
    }
    
    /// <summary>
    /// 移动单个目录
    /// </summary>
    /// <param name="dirFrom">要移动的目录</param>
    /// <param name="dirTo">移动到的父目录</param>
    /// <returns></returns>
    protected int DirMove(string dirFrom, string dirTo, ref string msg)
    {
        int cntFile = 0;
        if (!Directory.Exists(dirFrom))
        {
            msg = dirFrom + "目录不存在";
            return -1;
        }
        try
        {
            // 判断目标目录是否存在，不存在则创建
            if (!Directory.Exists(dirTo))
                Directory.CreateDirectory(dirTo);

            DirectoryInfo objDir = new DirectoryInfo(dirFrom);
            FileSystemInfo[] sfiles = objDir.GetFileSystemInfos();
            if (sfiles.Length > 0)
            {
                foreach (FileSystemInfo t1 in sfiles)
                {
                    string movName = Path.GetFileName(t1.FullName);
                    if (t1.Attributes == FileAttributes.Directory)
                    {
                        // 递归移动子目录
                        int tmp = DirMove(t1.FullName, Path.Combine(dirTo, movName), ref msg);
                        if (tmp == -1)
                            return -1;
                    }
                    else
                    {
                        string to = Path.Combine(dirTo, movName);
                        if (File.Exists(to))
                            File.Delete(to);
                        File.Move(t1.FullName, to);
                        cntFile++;
                    }
                }
            }
            // 删除当前目录
            Directory.Delete(dirFrom);
        }
        catch (Exception exp)
        {
            msg = dirFrom + " 目录移动失败\r\n<br />\r\n" + exp;
            return -1;
        }
        return cntFile;
    }
</script>

<!-- 压缩解压方法集 -->    
<script language="C#" runat="server">
/// <summary>
/// 压缩指定的文件列表
/// </summary>
/// <param name="zipFilePath">要压缩到的文件路径名</param>
/// <param name="files">要被压缩的文件列表</param>
public static void ZipFiles(string zipFilePath, params string[] files)
{
    if (files == null || files.Length == 0)
        throw new ArgumentException("文件列表不能为空", "files");
    if (string.IsNullOrEmpty(zipFilePath))
        zipFilePath = Path.GetFileName(files[0]) + ".zip";

    using (ZipOutputStream zos = new ZipOutputStream(File.Create(zipFilePath)))
    {
        foreach (string file in files)
        {
            // GetDirectoryName，是用于把文件名中不需要压缩的路径替换掉，避免压缩包里出现C:这样的目录结构
            string filename = Path.GetFullPath(file);   // 避免出现c:\\//a.txt这样的多斜杠的路径，造成压缩路径显示错误
            AddFileEntry(zos, filename, Path.GetDirectoryName(filename));
        }
        zos.Close();
    }
}    
/// <summary>
/// 压缩指定的目录列表
/// </summary>
/// <param name="zipFilePath">要压缩到的文件路径名</param>
/// <param name="dirs">要被压缩的目录列表</param>
public static void ZipDirs(string zipFilePath, params string[] dirs)
{
    if (dirs == null || dirs.Length == 0)
        throw new ArgumentException("目录列表不能为空", "dirs");
    if (string.IsNullOrEmpty(zipFilePath))
        zipFilePath = Path.GetFileName(dirs[0]) + ".zip";

    using (ZipOutputStream zos = new ZipOutputStream(File.Create(zipFilePath)))
    {
        foreach (string dir in dirs)
        {
            string dirname = Path.GetFullPath(dir);   // 避免出现c:\\//abc这样的多斜杠的路径，造成压缩路径显示错误
            AddDirEntry(zos, dirname, Path.GetDirectoryName(dirname));
        }
    }
}
/// <summary>
/// 把目录加入压缩包,返回压缩的目录和文件数
/// </summary>
/// <param name="zos"></param>
/// <param name="dir"></param>
/// <param name="rootDir">用于把文件名中不需要压缩的路径替换掉，避免压缩包里出现C:这样的目录结构</param>
private static int AddDirEntry(ZipOutputStream zos, string dir, string rootDir)
{
    string[] dirs = Directory.GetDirectories(dir);
    string[] files = Directory.GetFiles(dir);
    int ret = files.Length + dirs.Length;
    foreach (string subdir in dirs)
    {
        int tmp = AddDirEntry(zos, subdir, rootDir);
        if (tmp == 0)
        {
            string strEntryName = subdir.Replace(rootDir, "");
            ZipEntry entry = new ZipEntry(strEntryName + "\\_");
            zos.PutNextEntry(entry);
        }
        ret += tmp;
    }
    foreach (string file in files)
    {
        AddFileEntry(zos, file, rootDir);
    }
    return ret;
}

/// <summary>
/// 把文件加入压缩包
/// </summary>
/// <param name="zos"></param>
/// <param name="file"></param>
/// <param name="rootDir">用于把文件名中不需要压缩的路径替换掉，避免压缩包里出现C:这样的目录结构</param>
private static void AddFileEntry(ZipOutputStream zos, string file, string rootDir)
{
    //rootDir = Regex.Replace(rootDir, @"[/\\]+", @"\");// 把多个斜杠替换为一个
    //file = Regex.Replace(file, @"[/\\]+", @"\");// 把多个斜杠替换为一个
    if (!rootDir.EndsWith(@"\"))
        rootDir += @"\";
    using (FileStream fs = File.OpenRead(file))
    {
        string strEntryName = file.Replace(rootDir, "");
        ZipEntry entry = new ZipEntry(strEntryName);
        zos.PutNextEntry(entry);
        int size = 1024;
        byte[] array = new byte[size];
        while (fs.Position < fs.Length)
        {
            int length = fs.Read(array, 0, size);
            zos.Write(array, 0, length);
        }
        fs.Close();
    }
}
/// <summary>
/// 把指定压缩包解压到指定文件夹，并返回解压文件数，文件夹为空时，解压到压缩包所在目录
/// </summary>
/// <param name="zipfilename"></param>
/// <param name="unzipDir"></param>
public static int UnZipFile(string zipfilename, string unzipDir)
{
    int filecount = 0;
    if (string.IsNullOrEmpty(unzipDir))
    {
        unzipDir = Path.GetDirectoryName(zipfilename);
        if (unzipDir == null)
            throw new ArgumentException("目录信息不存在", "zipfilename");
    }
    else if (!Directory.Exists(unzipDir))
    {
        //生成解压目录
        Directory.CreateDirectory(unzipDir);
    }
    using (ZipInputStream s = new ZipInputStream(File.OpenRead(zipfilename)))
    {
        ZipEntry theEntry;
        while ((theEntry = s.GetNextEntry()) != null)
        {
            string path = Path.Combine(unzipDir, theEntry.Name);
            if (theEntry.IsDirectory)
            {
                if (!Directory.Exists(path))
                    Directory.CreateDirectory(path);
            }
            else if (theEntry.IsFile)
            {
                filecount++;
                string dir = Path.GetDirectoryName(path);
                if(string.IsNullOrEmpty(dir))
                    throw new Exception("压缩文件有问题，有个文件没有目录" + path);
                if (!Directory.Exists(dir))
                    Directory.CreateDirectory(dir);
                //解压文件到指定的目录)
                using (FileStream fs = File.Create(path))
                {
                    byte[] data = new byte[2048];
                    while (true)
                    {
                        int size = s.Read(data, 0, data.Length);
                        if (size <= 0)
                            break;
                        fs.Write(data, 0, size);
                    }
                    fs.Close();
                }
            }
        }
        s.Close();
    }
    return filecount;
}
</script>

<!-- 上传下载方法集 -->    
<script language="C#" runat="server">
    public static void UploadFile(string remoteIpLst, string localIp)
    {
        var Request = HttpContext.Current.Request;
        var Response = HttpContext.Current.Response;
        Response.Clear();
        try
        {
            if (Request.Files.Count == 0)
            {
                Response.Write("未上传文件");
                Response.End();
                return;
            }
            string dir = Request.Form["fileUploadDir"];
            if(string.IsNullOrEmpty(dir))
            {
                Response.Write("未指定上传目录");
                Response.End();
                return;
            }
            string serverFileName = Request.Files[0].FileName;
            if (string.IsNullOrEmpty(serverFileName))
                serverFileName = "tmp.tmp";
            else
                serverFileName = Path.GetFileName(serverFileName) ?? "tmp.tmp";
            serverFileName = Path.Combine(dir, serverFileName);

            if (File.Exists(serverFileName))
            {
                File.Delete(serverFileName);
            }
            Request.Files[0].SaveAs(serverFileName);
            //byte[] data = Request.BinaryRead(Request.ContentLength);

            //using (FileStream stream = GetWriteStream(serverFileName))
            //{
            //    stream.Write(data, 0, data.Length);
            //    stream.Flush();
            //    stream.Close();
            //}
            string filePrefix = "（客户ip：" + remoteIpLst + "；服务器：" + localIp + "）";
            Response.Write("<script type='text/javascript'>top.document.getElementById('fileBtnGetDir').click();alert('上传成功\\r\\n" + 
                filePrefix + "');</" + "script>");
        }
        catch (ThreadAbortException)
        {
        }
        catch (Exception exp)
        {
            Response.Write(exp.ToString());
        }
        Response.End();
    }
    static FileStream GetWriteStream(string savePath)
    {
        FileStream stream;
        if (File.Exists(savePath))
        {
            stream = File.OpenWrite(savePath);
            stream.Seek(stream.Length, SeekOrigin.Current); //移动文件流中的当前指针 
        }
        else
        {
            string dir = Path.GetDirectoryName(savePath) ?? string.Empty;
            if (!Directory.Exists(dir))
                Directory.CreateDirectory(dir);
            stream = new FileStream(savePath, FileMode.Create);
        }
        return stream;
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

<!-- 通用方法集 -->    
<script language="C#" runat="server">
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
    
    static string GetRemoteIpLst()
    {
        if (HttpContext.Current == null)
            return string.Empty;
        var request = HttpContext.Current.Request;
        string ip1 = request.UserHostAddress;
        string ip2 = request.ServerVariables["REMOTE_ADDR"];
        string realip = request.ServerVariables["HTTP_X_REAL_IP"];
        string isvia = request.ServerVariables["HTTP_VIA"];
        string forwardip = request.ServerVariables["HTTP_X_FORWARDED_FOR"];
        string proxy = request.Headers.Get("HTTP_NDUSER_FORWARDED_FOR_HAPROXY");
        return ip1 + ";" + ip2 + ";" + realip + ";" + isvia + ":" + forwardip + ";" + proxy;
    }
    // 获取本机IP列表
    static string GetServerIpList()
    {
        try
        {
            StringBuilder ips = new StringBuilder();
            IPHostEntry IpEntry = Dns.GetHostEntry(Dns.GetHostName());
            foreach (IPAddress ipa in IpEntry.AddressList)
            {
                if (ipa.AddressFamily == AddressFamily.InterNetwork)
                    ips.AppendFormat("{0};", ipa.ToString());
            }
            return ips.ToString();
        }
        catch (Exception)
        {
            //LogHelper.WriteCustom("获取本地ip错误" + ex, @"zIP\", false);
            return string.Empty;
        }
    }
            
    // 获取Session，如果禁用Session时，获取Cookie
    static string GetSession(string key)
    {
        SessionStateSection sessionStateSection = (SessionStateSection)ConfigurationManager.GetSection("system.web/sessionState");
        if (sessionStateSection.Mode == SessionStateMode.Off)
        {
            HttpCookie cook = HttpContext.Current.Request.Cookies[key];
            if (cook == null) return string.Empty;
            return cook.Value;
        }
        else
            return Convert.ToString(HttpContext.Current.Session[key]);
    }
    // 设置Session，如果禁用Session时，设置Cookie
    static void SetSession(string key, string value)
    {
        SessionStateSection sessionStateSection = (SessionStateSection)ConfigurationManager.GetSection("system.web/sessionState");
        if (sessionStateSection.Mode == SessionStateMode.Off)
            HttpContext.Current.Response.Cookies.Add(new HttpCookie(key, value));
        else
            HttpContext.Current.Session[key] = value;
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

    // 返回按key进行排序的SortedList
    static SortedList ConvertDict2(IDictionary dict)
    {
        SortedList ret = new SortedList();
        foreach (DictionaryEntry pair in dict)
        {
            ret.Add(pair.Key, pair.Value);
        }
        return ret;
    }
    
    public static string ToJson<T>(T source)
    {
        DataContractJsonSerializer jsonSerializer = new DataContractJsonSerializer(typeof(T));
        using (MemoryStream ms = new MemoryStream())
        {
            jsonSerializer.WriteObject(ms, source);
            StringBuilder sb = new StringBuilder();
            sb.Append(Encoding.UTF8.GetString(ms.ToArray()));

            return sb.ToString();
        }
    }

    // 返回按key进行排序的SortedList
    static SortedList<T1, T2> ConvertDict<T1, T2>(IDictionary<T1, T2> dict)
    {
        SortedList<T1, T2> ret = new SortedList<T1, T2>();
        foreach (KeyValuePair<T1, T2> pair in dict)
        {
            ret.Add(pair.Key, pair.Value);
        }
        return ret;
    }

    static object GetValue(object obj, string propertyName)
    {
        return GetValue(obj, propertyName, true);
    }

    static object GetValue(object obj, string propertyName, bool IsPublic)
    {
        object l_ret;

        BindingFlags flags =
            BindingFlags.GetProperty |
            BindingFlags.Instance;
        if (IsPublic)
            flags |= BindingFlags.Public;
        else
            flags |= BindingFlags.NonPublic;

        PropertyInfo info =
            obj.GetType().GetProperty(propertyName, flags);
        if (info != null)
        {
            l_ret = info.GetValue(obj, null);
        }
        else
        {
            l_ret = null;
        }
        return l_ret;
    }
    /// <summary>
    /// 执行对象的公共或非公共方法
    /// </summary>
    /// <param name="type"></param>
    /// <param name="methodName"></param>
    /// <param name="IsPublic">是否公共方法</param>
    /// <param name="args">用ref，因为对象的方法可能也加了ref或out</param>
    /// <returns></returns>
    public static object ExecuteStaticMethod(Type type, string methodName, bool IsPublic, ref object[] args)
    {
        object l_ret;

        BindingFlags flags =
            BindingFlags.InvokeMethod |
            BindingFlags.Static;

        if (IsPublic)
            flags |= BindingFlags.Public;
        else
            flags |= BindingFlags.NonPublic;

        Type[] types;
        if (args == null)
            types = new Type[0];
        else
        {
            // 定义参数类型，避免重载方法调用导致：发现不明确的匹配
            types = new Type[args.Length];
            int i = 0;
            foreach (object arg in args)
            {
                types[i] = arg.GetType();
                i++;
            }
        }
        MethodInfo method = type.GetMethod(methodName, flags, null, types, null);
        if (method != null)
        {
            // 没有传递参数时,根据方法的参数个数，传递指定个null参数过去
            if (args == null)
            {
                ParameterInfo[] arrPara = method.GetParameters();
                if (arrPara.Length > 0)
                {
                    args = new object[arrPara.Length];
                }
            }
            l_ret = method.Invoke(null, args);
            //l_ret = obj.GetType().InvokeMember(methodName, flags, null, obj, args);
        }
        else
        {
            l_ret = "指定的方法不存在：" + methodName;
        }
        return l_ret;
    }




</script>

    <script type="text/javascript" src="http://image.sjpic.91rb.com/client91_cache/jquery/jquery-1.7.2.min.js"></script>
    <script type="text/javascript" src="http://image.sjpic.91rb.com/client91_cache/ui.tabs/ui.tabs.js"></script>
    <link rel="stylesheet" href="http://image.sjpic.91rb.com/client91_cache/ui.tabs/ui.tabs.css" type="text/css" media="print, projection, screen" />

    <script type="text/javascript" src="http://image.sjpic.91rb.com/client91_cache/jquery/jqModal.js"></script>
	<link rel="stylesheet" href="http://image.sjpic.91rb.com/client91_cache/jquery/jqModal.css" />

    <script type="text/javascript" src="http://image.sjpic.91rb.com/client91_cache/other/rowColor.js"></script>
    <style type="text/css">
        .filetb { border-collapse: collapse;}
        .filetb td,th{ border: black 1px solid;padding: 2px 2px 2px 2px}
    </style>
    <script type="text/javascript">

        $(document).ready(function () {
            // 初始化标签
            var s = new UI_TAB();
            s.init("container-1");

            // 初始化弹出层
            $('#dialog').jqm({ modal: true });

            // 文件管理器按回车时，获取文件
            $("#fileDir").keyup(function (e) {
                if (e.which == 13) {
                    fileManager();
                }
            });
        });

        function doSubmit(callback) {
            $("#divret").html("");
            var ips = getIps();
            for (var i = 0; i < ips.length; i++) {
                var ip = ips[i];
                $("#divret").append("<div id='div" + i + "' style='border:solid 1px blue;'>" + ip + "处理中……<br /></div>");
                callback(ip, i);
            }
        }

        function getIps() {
            var ips = $("#txtIp").val();
            var ret = [];
            if (ips.length > 0) {
                var iparr = ips.split(';');
                for (var i = 0, j = iparr.length; i < j; i++) {
                    var ip = $.trim(iparr[i]);
                    if (ip.length > 0)
                        ret.push(ip);
                }
            }
            if (ret.length <= 0) {
                $("#txtIp").val("127.0.0.1");
                ret.push("127.0.0.1");
            }
            return ret;
        }

        function ajaxSend(para, callback) {
            var url = '<%=m_currentUrl %>' + "?" + new Date();
            $.ajax({
                url: url,
                //dataType: "json",
                type: "POST",
                data: para,
                success: callback,
                error: ajaxError
            });
        }
        // ajax失败时的回调函数
        function ajaxError(httpRequest, textStatus, errorThrown) {
            // 通常 textStatus 和 errorThrown 之中, 只有一个会包含信息
            //this; // 调用本次AJAX请求时传递的options参数
            alert(textStatus + errorThrown);

            // 防止重复请求文件管理
            fileGeting = false;
        }
    </script>
</head>
<body style="font-size:12px;">
<div style="white-space: nowrap">
    服务器IP列表:<input type="text" id="txtIp" style="width:800px;" value="<%=m_ipLst %>" /><br/>
    <span style="color:Blue; font-weight: bold">多个ip以半角分号分隔，ip与端口以冒号分隔，如：10.1.2.3:88;10.1.2.4;</span>
    (remote ip:<span style="color:blue;"><%=m_remoteIpLst %></span>　local ip:<span style="color:blue;"><%=m_localIp %></span>)
    <hr />
</div>
<div id="container-1">
    <ul class="ui-tabs-nav">
        <li class="ui-tabs-selected"><a href="#fragment2"><span>Telnet测试</span></a></li>
        <li class=""><a href="#fragment3"><span>Sql查询</span></a></li>
        <li class=""><a href="#fragment5"><span>Redis管理</span></a></li>
        <li class=""><a href="#fragment4" onclick="if($('#tbFileManager').length<=0){fileManager();}"><span>文件管理器</span></a></li>
    </ul>

    <!-- Telnet测试 -->
    <div class="ui-tabs-panel ui-tabs-hide" id="fragment2">
        <hr style="height: 5px; background-color: green" />
        目标服务器IP和端口列表（ip:端口 换行 ip:端口）：<br />
        <textarea id="txtTelnetIp" rows="5" cols="40">192.168.1.1:3389
192.168.1.2:1433</textarea><br/>
        <input type="button" value="测试" onclick="telnetTest(this);" style="width:200px;"/>
        <script type="text/javascript">
            function telnetTest(btn) {
                var ipTo = $.trim($("#txtTelnetIp").val());
                if (ipTo.length <= 0) {
                    alert("请输入测试IP和端口列表");
                    return;
                }
                $("#divret").html("");

                $(btn).attr("disabled", "disabled");
                doSubmit(function (ip, idx) {
                    var para = "flg=telnet&ip=" + ip + "&tip=" + ipTo;
                    ajaxSend(para, function (msg) {
                        $("#div" + idx).html("<pre>" + ip + "返回如下：(处理时间：" +
                            (new Date()).toString() + ")\r\n" + msg.replace(/</g, "&lt;") + "</pre>");
                    });
                });
                $(btn).removeAttr("disabled");
            }
        </script>
    </div>

    <!-- Sql查询 -->
    <div class="ui-tabs-panel ui-tabs-hide" id="fragment3">
        <hr style="height: 5px; background-color: green" />
        <div>说明：Sql查询只能测试单机，不能多服务器查询，多服务器请用Telnet测试</div>
        <div style="color: red;">注意：只允许使用select查询语句，不允许update等修改性语句，且select语句不是很耗性能的语句</div>
        <br />
        数据库连接串：<input type="text" id="txtSqlCon" value="server=192.168.1.1;database=db;uid=xx;pwd=xx" style="width:900px"/><br/>
        SQL：<textarea id="txtSql" rows="2" cols="20" style="height:200px;width:1000px;">select top 2 * 
  from softs with(nolock)
 where 1=1</textarea><br/>
        <input type="button" value="测试" onclick="sqlTest(this);" style="width:200px;"/>
        <script type="text/javascript">
            function sqlTest(btn) {
                var constr = $.trim($("#txtSqlCon").val());
                if (constr.length <= 0) {
                    alert("请输入数据库连接串");
                    return;
                }
                var sql = $.trim($("#txtSql").val());
                if (sql.length <= 0) {
                    alert("请输入SQL");
                    return;
                }
                $("#divret").html("查询中，请稍候……");

                $(btn).attr("disabled", "disabled");
                var para = "flg=sql&sql=" + sql + "&constr=" + constr;
                ajaxSend(para, function (msg) {
                    $("#divret").html(msg);
                });
                $(btn).removeAttr("disabled");
            }
        </script>
    </div>

    <!-- Redis管理 -->
    <div class="ui-tabs-panel ui-tabs-hide" id="fragment5">
        <hr style="height: 5px; background-color: green" />
        <table>
            <tr>
                <td>Redis IP和端口（格式：密码@ip:端口）：</td>
                <td>
                    <select onchange="$('#txtRedisServer').val($(this).val());">
                        <optgroup label="主Redis">
                            <option>password@10.1.2.3:6379</option>
                        </optgroup>
                    </select>
                    <input type="text" id="txtRedisServer" style="width: 200px" value="password@10.1.2.3:6379"/>
                </td>
            </tr>
            <tr>
                <td>命令：</td><td><input type="text" id="txtRedisCommand" style="width: 500px" value="info"/></td>
            </tr>
            <tr>
                <td><input type="button" value="提交" onclick="sendRedis(this);"/></td>
            </tr>
        </table>
        <script type="text/javascript">
            function sendRedis(btn) {
                var ipTo = $.trim($("#txtRedisServer").val());
                if (ipTo.length <= 0) {
                    alert("请输入RedisIP和端口");
                    return;
                }
                var sql = $.trim($("#txtRedisCommand").val());
                if (sql.length <= 0) {
                    alert("请输入命令");
                    return;
                }
                $("#divret").html("查询中，请稍候……");

                $(btn).attr("disabled", "disabled");
                var para = "flg=redis&cm=" + sql + "&tip=" + ipTo;
                ajaxSend(para, function (msg) {
                    $("#divret").html("<pre>" + msg + "</pre>");
                });
                $(btn).removeAttr("disabled");
            }
        </script>
    </div>

    <!-- 文件管理器 -->
    <div class="ui-tabs-panel ui-tabs-hide" id="fragment4">
        <hr style="height: 5px; background-color: green" />
        <table border="0" cellpadding="0" cellspacing="0" style="table-layout:fixed;">
            <tr>
                <td colspan="2" style="text-align:center; font-weight:bold; color:Green;">
                    <a href="#0" onclick="fileUpload();">上传..</a>
                    ｜
                    <a href="#0" onclick="fileZipDown();" title="打包下面选中的下载目录或文件">打包下载</a>
                    ｜
                    <a href="#0" onclick="fileMove();" title="把选中的目录和文件移动到指定的目录">移动选中..</a>
                    ｜
                    <a href="#0" onclick="fileDelSelect();" title="删除下面选中的目录和文件">删除选中</a>
                    ｜
                    <a href="#0" onclick="fileDirClear();">按时间删除..</a>
                    ｜
                    <a href="#0" onclick="fileCreateDir();">创建目录..</a>
                    ｜
                    <a href="#0" onclick="fileUnZip();">解压ZIP</a>
                    <hr />
                </td>
            </tr>
            <tr>
                <td>服务器</td>
                <td>
                    <select onchange="$('#txtFileServer').val($(this).val());">
                        <option>127.0.0.1</option>
                    </select>
                    <input type="text" id="txtFileServer" value="127.0.0.1" style="width:290px;"/>　
                    
                </td>
            </tr>
            <tr>
                <td>网站根目录</td>
                <td>
                    <input type="text" style="width:600px" disabled="disabled" value="<%=Server.MapPath("") %>"/>
                </td>
            </tr>
            <tr>
                <td>显示的目录</td>
                <td>
                    <input type="text" id="fileDir" style="width:600px" value="<%=Server.MapPath("") %>"/>
                </td>
            </tr>
            <tr>
                <td colspan="2">
                    <span style="color:royalblue;font-weight: bold;">目录或文件排序:</span>
                    <select id="lstFileSort">
	                    <option value="10">文件名</option>
	                    <option value="11">文件名倒序</option>
	                    <option value="20">扩展名</option>
	                    <option value="21">扩展名倒序</option>
	                    <option value="30">文件大小</option>
	                    <option value="31">文件大小倒序</option>
	                    <option value="40">修改日期</option>
	                    <option value="41">修改日期倒序</option>
                    </select>
                    　<label style="color: royalblue;font-weight: bold"><input type="checkbox" id="chkShowFileMd5" />显示MD5</label>
                    <input type="button" id="fileBtnGetDir" onclick="fileManager();" value="获取" style="width:100px; font-weight: bold"/>
                </td>
            </tr>
        </table>
        
        <script type="text/javascript">
            var fileGeting = false;
            
            function testJson() {
                fileManager("clientdirmd5");
            }
            
            function fileManager(flg, p) {
                if (fileGeting) {
                    alert("加载中，请稍候……");
                    return;
                }
                
                //$("#divret").html("服务端处理中，请稍候……");
                $("#fileBtnGetDir").attr("disabled", "disabled").val("处理中……");
                var txt = $("#fileDir").val();
                if(txt.length == 0) {
                    alert("目录不能为空");
                    return;
                }
                if (!flg) flg = "dir";
                var para = "flg=filemanager&f=" + flg + "&d=" + encodeURIComponent(txt) +
                    "&s=" + $("#lstFileSort").val();
                if ($("#chkShowFileMd5").is(":checked"))
                    para += "&md5=1";// 显示md5
                if (p)
                    para += "&" + p;
                var ip = $("#txtFileServer").val();
                if(ip.length > 0 && ip != "127.0.0.1")
                    para += "&ip=" + ip;
                fileGeting = true;
                try {
                    ajaxSend(para, function (msg) {
                        fileGeting = false;
                        $("#fileBtnGetDir").removeAttr("disabled").val("获取");
                        $("#divret").html(msg);
                    });
                }
                catch(ee1) {
                    fileGeting = false;
                    $("#fileBtnGetDir").removeAttr("disabled").val("获取");
                }
            }
        
            // 打开目录，dir为0表示打开上级目录，dir为1表示打开根目录，dir为字符串表示打开子目录
            function fileOpenDir(dir) {
                var txt = $("#fileDir");
                var strdir = txt.val().replace("/", "\\");
                txt.val(strdir);
                if (dir.constructor == Number){ //typeof(dir) == "number" 也可以
                    if (strdir.indexOf("\\") < 0) {
                        alert("已是根目录");
                        return;
                    }
                    switch(dir){
                    case 0:     // 返回上级目录
                        txt.val(strdir.substring(0, strdir.lastIndexOf("\\")));
                        break;
                    case 1:     // 返回根目录
                        txt.val(strdir.substring(0, strdir.indexOf("\\")));
                        break;
                    }
                } else 
                    txt.val(strdir + "\\" + dir);
                fileManager();
            }
            
            // 目录改名，文件改名
            function fileReName(str, flg) {
                var html = ("<p>&nbsp;原　名：" + str + "<br />&nbsp;更改为：<input type='text' id='txtNewName' value='" + 
                    str + "' style='width:400px;'/></p>");
                showDialog(html, function () {
                    var newName = $("#txtNewName").val();
                    if (newName.length <= 0 || str == newName) {
                        alert("请输入新的名字");
                        return;
                    }
                    fileManager(flg, "old=" + encodeURIComponent(str) + "&new=" + encodeURIComponent(newName));
                });
            }

            // 目录或文件删除
            function fileDel(str, flg) {
                var msg = "你确认要删除 " + str;
                if (flg == "dirDel")
                    msg += "目录及下面的全部子目录和子文件吗";
                if (!confirm(msg + "？"))
                    return;
                fileManager(flg, "old=" + encodeURIComponent(str));
            }

            // 获取目录大小
            function countDirSize(dir) {
                var iptxt = $("#txtFileServer").val();
                if (iptxt.length > 0 && iptxt != "127.0.0.1")
                    iptxt += "<input type='hidden' name='ip' value='" + iptxt + "'/>";
                else
                    iptxt = "";

                var txt = dir; // $("#fileDir").val() + "\\" + dir;
                var htm = "<form id='formSize' action='?xx' target='hiddenIFrame' method='post'>" +
                    "<input type='hidden' name='flg' value='filemanager' />" +
                    "<input type='hidden' name='f' value='dirSize' />" +
                    "<input type='hidden' name='d' value='" + txt + "' />" +
                    iptxt +
                    "</form>";
                showDialog(htm, null);
                $("#formSize").submit();
                hideDialog();
            }
            // 下载或打开文件
            function fileDownOpen(dir, isdown, code) {
                var iptxt = $("#txtFileServer").val();
                if (iptxt.length > 0 && iptxt != "127.0.0.1")
                    iptxt += "<input type='hidden' name='ip' value='" + iptxt + "'/>";
                else
                    iptxt = "";

                var flg = isdown ? "fileDown" : "fileOpen";
                var txt = dir; // $("#fileDir").val() + "\\" + dir;
                var en;
                if (code == 1)
                    en = "GB2312";
                else
                    en = "UTF-8";
                var htm = "<form id='formSize' action='?contentType=" + (isdown ? "down" : "text") + 
                    "' target='" + (isdown ? "hiddenIFrame" : "_blank") + "' method='post'>" +
                    "<input type='hidden' name='flg' value='filemanager' />" +
                    "<input type='hidden' name='f' value='" + flg + "' />" +
                    "<input type='hidden' name='d' value='" + txt + "' />" +
                    "<input type='hidden' name='en' value='" + en + "' />" +
                    iptxt +
                    "</form>";
                showDialog(htm, null);
                $("#formSize").submit();
                hideDialog();
            }

            // 上传文件
            function fileUpload() {
                var ip = $("#txtFileServer").val();
                if (ip.length > 0 && ip != "127.0.0.1") {
                    alert("文件只能上传到127.0.0.1");
                    return;
                }
                var txt = $("#fileDir").val();
                var htm = "<form id='formUpload' action='?flg=fileupload' enctype='multipart/form-data' target='hiddenIFrame' method='post'><p>" +
                    "&nbsp;文件选择：<input type='file' name='file1' id='file1' style='width:400px;' /><br />" +
                    "&nbsp;上传目录：<input type='text' name='fileUploadDir' value='" + txt +
                    "' readonly='readonly' style='width:500px;background-color:gray'/><br />" +
                    "<span style='color:red;'>注意：文件上传仅支持上传到127.0.0.1本机</span></p></form>";

                showDialog(htm, function () {
                    var fname = $("#file1").val();
                    if (fname.length == 0) {
                        alert("请选择要上传的文件");
                        return;
                    }
                    $("#divret").html("上传中，请稍候……");
                    $("#formUpload").submit();
                });
            }
            // 打包下载
            function fileZipDown() {
                var file = $("input:checkbox[name='chkFileListBeinet']:checked"); 
                var dir = $("input:checkbox[name='chkDirListBeinet']:checked");

                if (file.length == 0 && dir.length == 0) {
                    alert("没有选中任何文件或目录");
                    return;
                }
                if (file.length > 0 && dir.length > 0) {
                    alert("不能同时选中文件或目录");
                    return;
                }
                var msg = "你确认要打包并下载选中的";
                if(file.length > 0)
                    msg += file.length + "个文件吗？";
                else
                    msg += dir.length + "个目录吗？";
                if (!confirm(msg))
                    return;
                var filestr = "";
                var dirstr = "";
                file.each(function () {
                    filestr += $(this).val() + ",";
                });
                dir.each(function () {
                    dirstr += $(this).val() + ",";
                });
                var txt = $("#fileDir").val();

                var iptxt = $("#txtFileServer").val();
                if (iptxt.length > 0 && iptxt != "127.0.0.1")
                    iptxt += "<input type='hidden' name='ip' value='" + iptxt + "'/>";
                else
                    iptxt = "";

                var htm = "<form id='formSize' action='?contentType=down' target='hiddenIFrame' method='post'>" +
                    "<input type='hidden' name='flg' value='filemanager' />" +
                    "<input type='hidden' name='f' value='fileZipDown' />" +
                    "<input type='hidden' name='d' value='" + txt + "' />" +
                    "<input type='hidden' name='file' value='" + filestr + "' />" +
                    "<input type='hidden' name='dir' value='" + dirstr + "' />" +
                    iptxt + "</form>";
                showDialog(htm, null);
                $("#formSize").submit();
                hideDialog();
            }
            // 移动选中
            function fileMove() {
                var file = $("input:checkbox[name='chkFileListBeinet']:checked"); 
                var dir = $("input:checkbox[name='chkDirListBeinet']:checked");

                if (file.length == 0 && dir.length == 0) {
                    alert("没有选中任何文件或目录");
                    return;
                }
                var txt = $("#fileDir").val();
                var htm = "移动到的目录：<input type='text' id='fileToDir' style='width:400px;' value='" + txt + "'/>";

                showDialog(htm, function () {
                    var todir = $("#fileToDir").val();
                    if (todir.length == 0) {
                        alert("请输入要上传到的目录");
                        return;
                    }
                    var msg = "你确定要移动选中的";
                    if (file.length > 0)
                        msg += file.length + "个文件 ";
                    if (dir.length > 0)
                        msg += dir.length + "个目录 ";
                    if(!confirm(msg + "吗?"))
                        return;
                    
                    $("#divret").html("处理中，请稍候……");
                    var filestr = "";
                    var dirstr = "";
                    file.each(function () {
                        filestr += $(this).val() + ",";
                    });
                    dir.each(function () {
                        dirstr += $(this).val() + ",";
                    });
                    fileManager("fileMove", "file=" + filestr +
                        "&dir=" + encodeURIComponent(dirstr) + "&to=" + encodeURIComponent(todir));
                });
            }
            // 删除选中
            function fileDelSelect() {
                var file = $("input:checkbox[name='chkFileListBeinet']:checked"); 
                var dir = $("input:checkbox[name='chkDirListBeinet']:checked");

                if (file.length == 0 && dir.length == 0) {
                    alert("没有选中任何文件或目录");
                    return;
                }
                var msg = "你确定要删除选中的";
                if (file.length > 0)
                    msg += file.length + "个文件 ";
                if (dir.length > 0)
                    msg += dir.length + "个目录 ";
                if(!confirm(msg + "吗?"))
                    return;
                
                var filestr = "";
                var dirstr = "";
                file.each(function () {
                    filestr += $(this).val() + ",";
                });
                dir.each(function () {
                    dirstr += $(this).val() + ",";
                });
                fileManager("fileDelSelect", "file=" + encodeURIComponent(filestr) + "&dir=" + encodeURIComponent(dirstr));
            }
            // 按时间删除
            function fileDirClear() {
                var tm = '<%=DateTime.Now.AddMonths(-3).ToString("yyyy-MM-dd HH:mm:ss")%>';
                var htm = "删除所有文件修改时间小于：<input type='text' id='txtDirClearTime' style='width:150px;' " +
                    "value='" + tm + "'/><br />" +
                    "<label><input type='checkbox' id='chkDirClear' checked />包含子目录</label>" +
                    "<label><input type='checkbox' id='chkdelEmpty' checked />删除空子目录</label>";

                showDialog(htm, function () {
                    tm = $("#txtDirClearTime").val();
                    var msg = "你确定要删除当前目录时间小于" + tm + "的文件吗？";
                    if (!confirm(msg))
                        return;

                    $("#divret").html("处理中，请稍候……");
                    var para = "tm=" + tm;
                    if ($("#chkDirClear").is(":checked"))
                        para += "&subdir=1";
                    if ($("#chkdelEmpty").is(":checked"))
                        para += "&delem=1";
                    fileManager("fileDirClear", para);
                });
            }
            // 创建目录
            function fileCreateDir() {
                var txt = $("#fileDir").val();
                var htm = "要创建的目录：<input type='text' id='fileNewDir' style='width:400px;' value='" + txt + "'/>";

                showDialog(htm, function () {
                    txt = $("#fileNewDir").val();

                    $("#divret").html("处理中，请稍候……");
                    var para = "dir=" + encodeURIComponent(txt);
                    fileManager("fileCreateDir", para);
                });
            }
            // 解压ZIP
            function fileUnZip() {
                var obj = $("input:checkbox[name='chkFileListBeinet']:checked");
                if (obj.length == 0) {
                    alert("没有选中任何文件");
                    return;
                }
                if (obj.length != 1) {
                    alert("一次只能解压一个文件");
                    return;
                }
                if (!confirm("你确认要解压选中的文件吗？\r\n注1：文件必须是ZIP格式\r\n注2：解压会直接覆盖当前目录下文件，请确认清楚"))
                    return;
                fileManager("fileUnZip", "zipfile=" + encodeURIComponent(obj.val()));
            }
        </script>
    <script type="text/javascript">
        function showDialog(html, confirmClk) {
            $("#dialogContent").html(html);
            $("#dialogConfirm").unbind("click").click(function () {
                if (confirmClk != null)
                    confirmClk();
                hideDialog();
            });
            $("#dialog").jqmShow();
        }
        function hideDialog() {
            $("#dialog").jqmHide();
        }

        // 选中或取消全部文件
        function CheckAllFile(obj) {
            chgChkColor($("input[name='chkFileListBeinet']"), obj.checked);
        }
        // 选中或取消全部目录
        function CheckAllDir(obj) {
            chgChkColor($("input[name='chkDirListBeinet']"), obj.checked);
        }
        function chgChkColor(objchk, check) {
            if (check) {
                $(objchk).attr("checked", true);
                //$(objchk).css("background-color", "green");
            } else {
                $(objchk).attr("checked", false);
                //$(objchk).css("background-color", "white");
            }
        }

        // 弹出条件选择框
        function OpenCheckOption() {
            var htm = ("<p>\
<span style='font-size:15px;color:blue;'>根据下列条件选中指定的目录或文件</span><br />\
扩展名或数字输入框：<input type='text' id='txtCheckExt' /><br />\
&nbsp;<label><input type='radio' name='radCheckOption' value='0' checked='checked' />按文件扩展名</label><br />\
&nbsp;<label><input type='radio' name='radCheckOption' value='1' />搜索文件名或目录名</label><br />\
&nbsp;<label><input type='radio' name='radCheckOption' value='2' />选择前若干个目录</label><br />\
&nbsp;<label><input type='radio' name='radCheckOption' value='3' />选择前若干个文件</label><br />\
</p>");

            showDialog(htm, function () {
                // 取消全部选定
                chgChkColor($('#divret input[type=checkbox]'), false);

                var txt = $("#txtCheckExt").val().toLowerCase();
                var type = $("input:radio[name='radCheckOption']:checked").val();
                switch (type) {
                case "0": //按文件扩展名
                    if (txt.length <= 0) {
                        alert("请输入扩展名");
                        return;
                    }
                    $("input:checkbox[name='chkFileListBeinet']").each(function () {
                        var filename = $(this).parents("td").next().text().toLowerCase();
                        var extstart = filename.lastIndexOf('.');
                        if (extstart >= 0 && filename.substring(extstart + 1) == txt)
                            chgChkColor(this, true);
                    });
                    break;
                case "1": //搜索文件名或目录名
                    if (txt.length <= 0) {
                        alert("请输入要搜索的字符串");
                        return;
                    }
                    $("input:checkbox[name='chkFileListBeinet']").each(function () {
                        var filename = $(this).parents("td").next().text().toLowerCase();
                        if (filename.indexOf(txt) >= 0)
                            chgChkColor(this, true);
                    });
                    $("input:checkbox[name='chkDirListBeinet']").each(function () {
                        var filename = $(this).parents("td").next().text().toLowerCase();
                        if (filename.indexOf(txt) >= 0)
                            chgChkColor(this, true);
                    });
                    break;
                case "2": //选择前若干个目录
                    if (!(/^\d+$/).test(txt)) {
                        alert("请输入数字");
                        return;
                    }
                    $("input:checkbox:lt(" + txt + ")[name='chkDirListBeinet']").each(function () {
                        chgChkColor(this, true);
                    });
                    break;
                case "3": //选择前若干个文件
                    if (!(/^\d+$/).test(txt)) {
                        alert("请输入数字");
                        return;
                    }
                    $("input:checkbox:lt(" + txt + ")[name='chkFileListBeinet']").each(function () {
                        chgChkColor(this, true);
                    });
                    break;
                }
            });
        }
    </script>
    </div>

    <hr style="height: 5px; background-color: green" />
    <div id="divret"></div>

<div class="jqmWindow" id="dialog">
    <a href="#" class="jqmClose">取消</a>｜<a href="#" id="dialogConfirm">确定</a>
    <hr />
    <div id="dialogContent"></div>
</div>

<iframe name="hiddenIFrame" width="100%" height="30" frameborder="0" scrolling="no"></iframe>

</div>    
</body>
</html>
