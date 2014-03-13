using System;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.IO.Compression;
using System.Linq;
using System.Net;
using System.Text;
using System.Text.RegularExpressions;
using System.Windows.Forms;

namespace Beinet.cn.Tools
{
    public partial class GzipTest : Form
    {
        // Methods
        public GzipTest()
        {
            this.InitializeComponent();
            this.lstMethod.SelectedIndex = 0;

            string url = System.Configuration.ConfigurationManager.AppSettings["gzipurl"];
            if (string.IsNullOrEmpty(url))
            {
                if (txtUrl.Items.Count > 0)
                {
                    this.txtUrl.SelectedIndex = 0;
                }
            }
            else
            {
                this.txtUrl.Text = url;
            }

            string ips = System.Configuration.ConfigurationManager.AppSettings["gzipips"];
            if (!string.IsNullOrEmpty(ips))
                this.txtIps.Text = ips;
        }

        private void button1_Click(object sender, EventArgs e)
        {
            // 清除旧的结果
            for(int i=tabControl1.TabPages.Count-1;i>=1;i--)
                tabControl1.TabPages.RemoveAt(i);

            Encoding encoding;
            if (!this.txtUrl.Text.StartsWith("http://", StringComparison.OrdinalIgnoreCase))
            {
                this.txtUrl.Text = "http://" + this.txtUrl.Text;
            }
            string url = this.txtUrl.Text;
            string result = string.Empty;
            if (this.radioButton2.Checked)
            {
                encoding = Encoding.GetEncoding("GB2312");
            }
            else
            {
                encoding = Encoding.UTF8;
            }
            string[] ips = Regex.Split(txtIps.Text, @"[,;\s|]");

            // 增加随机参数，避免请求到缓存
            var rndCode = "isgziptest=" + Guid.NewGuid().ToString("N");
            if (url.IndexOf('#') > 0)
            {
                url = url.Substring(0, url.IndexOf('#'));
            }
            if (url.IndexOf('?') < 0)
                url += "?" + rndCode;
            else
                url += "&" + rndCode;

            StringBuilder sbRet = new StringBuilder("请求地址：" + url + "\r\n\r\n");
            foreach (string ip in ips)
            {
                if (ips.Length > 1 && ip == string.Empty)
                    continue;

                string ret = GetPage(url, this.txtPara.Text, this.lstMethod.Text, encoding, this.chkHeader.Checked, ip, ref result);

                string title = ip == string.Empty ? "无代理" : ip;
                sbRet.AppendFormat("{0}\t的测试结果：{1}\r\n\r\n", title, result);

                #region 把结果放入新的TabPage
                TabPage page = new TabPage(title);
                tabControl1.TabPages.Add(page);
                page.ForeColor = Color.Red;

                TextBox tb = new TextBox();
                page.Controls.Add(tb);
                tb.Dock = System.Windows.Forms.DockStyle.Fill;
                tb.Location = new System.Drawing.Point(3, 3);
                tb.Multiline = true;
                tb.ScrollBars = System.Windows.Forms.ScrollBars.Both;
                tb.Size = new System.Drawing.Size(660, 419);
                tb.TabIndex = 0;
                tb.WordWrap = false;

                tb.Text = ret;
                #endregion

            }
            this.txtResult.Text = sbRet.ToString();
        }

        private void button2_Click(object sender, EventArgs e)
        {
            this.txtResult.Text = "IIS 7.5中配置HTTP压缩的步骤如下： \r\n1、打开Internet信息服务(IIS)管理器，点击站点，在右侧找到“压缩”，双击进行勾选；\r\n2、打开文件打开C:\\Windows\\System32\\inetsrv\\config\\applicationhost.config,修改配置：\r\n<httpCompression directory=\"%SystemDrive%/inetpub/temp/IIS Temporary Compressed Files\">\r\n    <scheme name=\"gzip\" dll=\"%Windir%/system32/inetsrv/gzip.dll\" />\r\n    <dynamicTypes>\r\n        <add mimeType=\"*/*\" enabled=\"true\" />\r\n    </dynamicTypes>\r\n    <staticTypes>\r\n        <add mimeType=\"*/*\" enabled=\"true\" />\r\n    </staticTypes>\r\n</httpCompression>\r\n\r\n\r\nIIS 6.0中配置HTTP压缩的步骤如下： \r\n1、打开Internet信息服务(IIS)管理器，右击 网站->属性，选择 服务。\r\n   在 HTTP压缩 框中选中 压缩应用程序文件 和 压缩静态文件 ，\r\n   按需要设置 临时目录 和 临时目录的最大限制； \r\n2、在Internet信息服务(IIS)管理器，右击 Web服务扩展 -> 增加一个新的Web服务扩展...，\r\n   在 新建Web服务扩展 框中输入扩展名 HTTP Compression ，\r\n   添加 要求的文件 为C:\\WINDOWS\\system32\\inetsrv\\gzip.dll，\r\n   并选中 设置扩展状态为允许\r\n3、使用文本编辑器打开C:\\Windows\\System32\\inetsrv\\MetaBase.xml(建议先备份),\r\n   找到Location =\"/LM/W3SVC/Filters/Compression/gzip\"，\r\n   3.1、如果需要压缩动态文件，则将HcDoDynamicCompression设置为 TRUE，\r\n        并在HcScriptFileExtensions中增加您要压缩的动态文件后缀名，如aspx；\r\n   3.2、如果需要压缩静态文件，则将HcDoStaticCompression和HcDoOnDemandCompression设置为 TRUE，\r\n        并在HcFileExtensions中增加您需要压缩的静态文件后缀名，如xml、css等；\r\n   HcDynamicCompressionLevel和HcOnDemandCompLevel为0~9之间的数字，\r\n   表示需要的压缩率，数字越小压缩率越低，建议设置为9\r\n4、编辑完毕后保存MetaBase.xml文件；如果文件无法保存，则可能IIS正在使用该文件。\r\n   打开 开始->管理工具->服务，停止 IIS Admin Service 后，即可保存； \r\n5、重新启动IIS。\r\n\r\n注：上面Windows系统目录根据您的安装可能有所不同。\r\n     gif、jpg图片，已经是压缩的，所以没必要配置压缩\r\n\r\n配置示例：\r\n<IIsCompressionScheme\tLocation =\"/LM/W3SVC/Filters/Compression/gzip\"\r\n\t\tHcCompressionDll=\"%windir%\\system32\\inetsrv\\gzip.dll\"\r\n\t\tHcCreateFlags=\"1\"\r\n\t\tHcDoDynamicCompression=\"TRUE\"\r\n\t\tHcDoOnDemandCompression=\"TRUE\"\r\n\t\tHcDoStaticCompression=\"TRUE\"\r\n\t\tHcDynamicCompressionLevel=\"9\"\r\n\t\tHcFileExtensions=\"htm\r\n\t\t\thtml\r\n\t\t\txml\r\n\t\t\tcss\r\n\t\t\tjs\r\n\t\t\tbmp\r\n\t\t\ttxt\"\r\n\t\tHcOnDemandCompLevel=\"9\"\r\n\t\tHcPriority=\"1\"\r\n\t\tHcScriptFileExtensions=\"asp\r\n\t\t\tdll\r\n\t\t\taspx\r\n\t\t\texe\r\n\t\t\tasmx\r\n\t\t\tashx\"\r\n\t>\r\n</IIsCompressionScheme>\r\n\r\n附：\r\n1. HTTP压缩概述\r\nHTTP压缩是在Web服务器和浏览器间传输压缩文本内容的方法。HTTP压缩采用通用的压缩算法如gzip等压缩HTML、JavaScript或CSS文件。\r\n\r\n2. HTTP压缩工作原理\r\nWeb服务器处理HTTP压缩的工作原理如下： \r\nWeb服务器接收到浏览器的HTTP请求后，检查浏览器是否支持HTTP压缩； \r\n如果浏览器支持HTTP压缩，Web服务器检查请求文件的后缀名； \r\n如果请求文件是HTML、CSS等静态文件，Web服务器到压缩缓冲目录中检查是否已经存在请求文件的最新压缩文件； \r\n如果请求文件的压缩文件不存在，Web服务器向浏览器返回未压缩的请求文件，并在压缩缓冲目录中存放请求文件的压缩文件； \r\n如果请求文件的最新压缩文件已经存在，则直接返回请求文件的压缩文件； \r\n如果请求文件是ASPX等动态文件，Web服务器动态压缩内容并返回浏览器，压缩内容不存放到压缩缓存目录中。 \r\n\r\n也就是说，对iis启用压缩，并不会影响那么不支持gzip的浏览器访问\r\n";
            tabControl1.SelectedIndex = 0;
        }



        private static string GetPage(string url, string param, string HttpMethod, Encoding encoding, bool showHeader, string proxy, ref string result)
        {
            HttpWebResponse response;
            bool flag = string.IsNullOrEmpty(HttpMethod) || HttpMethod.Equals("GET", StringComparison.OrdinalIgnoreCase);
            if (flag && !string.IsNullOrEmpty(param))
            {
                url = url + "&" + param;
            }

            HttpWebRequest request = (HttpWebRequest)WebRequest.Create(url);
            request.Referer = url;
            request.AllowAutoRedirect = false;
            request.UserAgent = "Mozilla/4.0 (compatible; MSIE 6.0; Windows NT 5.2; SV1;)";
            request.Headers.Add("Accept-Encoding", "gzip, deflate");

            // 必须在写入Post Stream之前设置Proxy
            if (!string.IsNullOrEmpty(proxy))
            {
                #region 设置代理
                string[] tmp = proxy.Split(new[] { ':' }, StringSplitOptions.RemoveEmptyEntries);
                int port = 80;
                if (tmp.Length >= 2)
                {
                    if (!int.TryParse(tmp[1], out port))
                    {
                        port = 80;
                    }
                }
                request.Proxy = new WebProxy(tmp[0], port);
                #endregion
            }

            if (flag)
            {
                request.Method = "GET";
                request.ContentType = "text/html";
            }
            else
            {
                request.Method = "POST";
                request.ContentType = "application/x-www-form-urlencoded";
                if (!string.IsNullOrEmpty(param))
                {
                    byte[] bytes = encoding.GetBytes(param);
                    request.ContentLength = bytes.Length;
                    // 必须先设置ContentLength，才能打开GetRequestStream
                    using (Stream requestStream = request.GetRequestStream())
                    {
                        requestStream.Write(bytes, 0, bytes.Length);
                        requestStream.Close();
                    }
                }
                else
                    request.ContentLength = 0;// POST时，必须设置ContentLength属性
            }

            try
            {
                response = (HttpWebResponse)request.GetResponse();
            }
            catch (Exception exception)
            {
                result = "出错了";
                return ("返回错误：" + exception);
            }
            if (response.StatusCode == HttpStatusCode.OK)
            {
                result = "未启用压缩 ======= 长度:";
                Stream stream2;
                using (stream2 = response.GetResponseStream())
                {
                    if (stream2 == null)
                    {
                        return result + "GetResponseStream为null";
                    }
                    //using (BinaryReader s = new BinaryReader(stream2))
                    //{
                    //    byte[] buf = s.ReadBytes(1024 * 1024);
                    //}
                    List<byte> ret = new List<byte>(10000);
                    byte[] arr = new byte[10000];
                    int readcnt;
                    while ((readcnt = stream2.Read(arr, 0, arr.Length)) > 0)
                    {
                        ret.AddRange(arr.Take(readcnt));
                    }
                    arr = ret.ToArray();
                    using (var mem = new MemoryStream(arr))
                    {
                        bool isZip = false;
                        var contentLen = arr.Length; //response.ContentLength; gzip时ContentLength可能为-1
                        if (response.ContentEncoding.ToLower().Contains("gzip"))
                        {
                            isZip = true;
                            result = "已启用压缩 gzip 长度:" + contentLen.ToString("N0") + " ";
                            stream2 = new GZipStream(mem, CompressionMode.Decompress);
                        }
                        else if (response.ContentEncoding.ToLower().Contains("deflate"))
                        {
                            isZip = true;
                            result = "已启用压缩 deflate 长度:" + contentLen.ToString("N0") + " ";
                            stream2 = new DeflateStream(mem, CompressionMode.Decompress);
                        }
                        else
                        {
                            result += contentLen.ToString("N0") + " ";
                            stream2 = mem;
                        }
                        using (StreamReader reader = new StreamReader(stream2, encoding))
                        {
                            string str = reader.ReadToEnd();
                            int realLen = str.Length;
                            result = string.Format("{0} 实际长度:{1} 压缩率:{2} {3}",
                                result,
                                realLen.ToString("N0"),
                                ((realLen - contentLen) * 100.0 / realLen).ToString("N2"),
                                DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss_fff"));

                            if (showHeader)
                            {
                                str =
                                    string.Concat(new object[]
                                    {"请求头信息：\r\n", request.Headers, "\r\n\r\n响应头信息：\r\n", response.Headers, "\r\n", str});
                            }
                            return str;
                        }
                    }
                }
            }
            result = "其它结果";
            return string.Concat(new object[] { "远程服务器返回代码不为200,", response.StatusCode, ",", response.StatusDescription });
        }

        private void txtUrl_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Return)
            {
                this.button1_Click(null, null);
            }
        }
    }
}
