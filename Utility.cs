using System;
using System.IO;
using System.IO.Compression;
using System.Net;
using System.Reflection;
using System.Runtime.Serialization;
using System.Security.Cryptography;
using System.Text;
using System.Threading;
using System.Windows.Forms;
using System.Xml;

namespace Beinet.cn.Tools
{
    public static class Utility
    {
        private static readonly string _path = AppDomain.CurrentDomain.BaseDirectory;
        
        private static readonly object lockobj = new object();
        public static void Log(string msg, string suffix = null)
        {
            string logfile = Path.Combine(_path, "zlog\\" + DateTime.Now.ToString("yyyyMMdd") + suffix + ".txt");
            string logpath = Path.GetDirectoryName(logfile);
            if (logpath != null && !Directory.Exists(logpath))
            {
                Directory.CreateDirectory(logpath);
            }
            lock (lockobj)
            {
                // 之所以允许重试3次，因为可能同时运行多个程序，这多个程序同时访问txt，造成冲突
                int retry = 3;
                while (retry > 0)
                {
                    retry--;
                    try
                    {
                        using (StreamWriter sw = new StreamWriter(logfile, true, Encoding.UTF8))
                        {
                            sw.WriteLine(DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss") + ":" + msg + Environment.NewLine);
                        }
                        break;
                    }
                    catch (Exception)
                    {
                        // 写入失败时，重试3次
                        Thread.Sleep(TimeSpan.FromMilliseconds(10));
                    }
                }
            }
        }
        //static bool IsFileLocked(IOException exception)
        //{
        //    int errorCode = Marshal.GetHRForException(exception) & ((1 << 16) - 1);
        //    return errorCode == 32 || errorCode == 33;
        //}


        public static void InvokeControl(Control ctl, Action method)
        {
            Form frm = ctl.FindForm();
            if(frm == null || frm.IsDisposed)
            {
                return;
            }
            if (ctl.InvokeRequired)
            {
                frm.Invoke(method);
            }
            else
            {
                try
                {
                    method();
                }
                catch(Exception exp)
                {
                    MessageBox.Show(frm, exp.ToString());
                }
            }
        }

        //private delegate void SetValueDele(Control ctl, string propName, object val);
        public static void SetValue(Control ctl, string propName, object val)
        {
            InvokeControl(ctl, () => Utility.SetValue((object)ctl, propName, val));// 转换后调用下面的SetValue重载

            //if (ctl.InvokeRequired)
            //    Invoke(new SetValueDele(SetValue), ctl, propName, val);
            //else
            //    Utility.SetValue(ctl, propName, val);
        }

        #region 序列化和反序列化
        /// <summary>
        /// 把Xml字符串反序列化成对象
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="xml"></param>
        /// <returns></returns>
        public static T XmlDeserializeFromStr<T>(string xml)
        {
            var xs = new DataContractSerializer(typeof(T));
            using (var memoryStream = new MemoryStream(Encoding.UTF8.GetBytes(xml)))
            using (var xmlreader = new XmlTextReader(memoryStream))
            {
                // [\x0-\x8\x11\x12\x14-\x32]
                // 默认为true，如果序列化的对象含有比如0x1e之类的非打印字符，反序列化就会出错，因此设置为false http://msdn.microsoft.com/en-us/library/aa302290.aspx
                xmlreader.Normalization = false;
                xmlreader.WhitespaceHandling = WhitespaceHandling.Significant;
                xmlreader.XmlResolver = null;
                return (T)xs.ReadObject(xmlreader);
            }
        }

        /// <summary>
        /// 把对象序列化为Xml文件
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="obj"></param>
        /// <param name="configPath"></param>
        /// <returns></returns>
        public static void XmlSerialize<T>(T obj, string configPath)
        {
            var formatter = new DataContractSerializer(typeof(T));
            using (XmlTextWriter fs = new XmlTextWriter(configPath, Encoding.UTF8))//FileStream(configPath, FileMode.Create))
            {
                fs.Formatting = Formatting.Indented;
                formatter.WriteObject(fs, obj);
            }

            //// 让输出的xml可读性好
            //XmlWriterSettings settings = new XmlWriterSettings
            //{
            //    Indent = true,
            //    Encoding = Encoding.UTF8,
            //    IndentChars = "    "
            //};
            //using (FileStream fs = new FileStream(configPath, FileMode.Create))
            //using (XmlWriter writer = XmlWriter.Create(fs, settings))
            //{
            //    formatter.WriteObject(writer, obj);
            //}
        }

        /// <summary>
        /// 把Xml文件反序列化成对象
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="xmlPath"></param>
        /// <returns></returns>
        public static T XmlDeserialize<T>(string xmlPath)
        {
            var xs = new DataContractSerializer(typeof(T));
            using (var memoryStream = new StreamReader(xmlPath, Encoding.UTF8))
            using (var xmlreader = new XmlTextReader(memoryStream))
            {
                // [\x0-\x8\x11\x12\x14-\x32]
                // 默认为true，如果序列化的对象含有比如0x1e之类的非打印字符，反序列化就会出错，因此设置为false http://msdn.microsoft.com/en-us/library/aa302290.aspx
                xmlreader.Normalization = false;
                xmlreader.WhitespaceHandling = WhitespaceHandling.Significant;
                xmlreader.XmlResolver = null;
                return (T) xs.ReadObject(xmlreader);
            }
        }
        #endregion


        /// <summary>
        /// 通过反射，设置属性的值
        /// </summary>
        /// <param name="obj"></param>
        /// <param name="propName"></param>
        /// <param name="val"></param>
        public static void SetValue(object obj, string propName, object val)
        {
            Type type = obj.GetType();
            BindingFlags flags = BindingFlags.GetProperty | BindingFlags.Public | BindingFlags.Instance;
            PropertyInfo info = type.GetProperty(propName, flags);
            if (info == null)
                return;
            info.SetValue(obj, val, null);
        }
        /// <summary>
        /// 通过反射，获取属性的值
        /// </summary>
        /// <param name="obj"></param>
        /// <param name="propName"></param>
        /// <returns></returns>
        public static object GetValue(object obj, string propName)
        {
            Type type = obj.GetType();
            BindingFlags flags = BindingFlags.GetProperty | BindingFlags.Public | BindingFlags.Instance;
            PropertyInfo info = type.GetProperty(propName, flags);
            if (info == null)
                return null;
            return info.GetValue(obj, null);
        }


        /// <summary>
        /// 把类似1.2.3.4这样的ip转换成长整形返回
        /// </summary>
        /// <param name="strIp"></param>
        /// <returns></returns>
        public static long ConvertIp(string strIp)
        {
            System.Net.IPAddress ip;
            if (!System.Net.IPAddress.TryParse(strIp, out ip))
                return -1;
            return BitConverter.ToInt32(ip.GetAddressBytes(), 0);

            //if (!Regex.IsMatch(strIp, @"\d{1,3}\.\d{1,3}\.\d{1,3}\.\d{1,3}"))
            //    return -1;
            //string[] subIP = strIp.Split('.');
            //long ip = 16777216 * Convert.ToInt64(subIP[0]) +
            //    65536 * Convert.ToInt64(subIP[1]) +
            //    256 * Convert.ToInt64(subIP[2]) +
            //    Convert.ToInt64(subIP[3]);

            // 下面是对应System.Net.IPAddress.Parse("1.2.3.4")的方案,正好与上面相反
            //long ip2 = 16777216 * Convert.ToInt64(subIP[3]) +
            //   65536 * Convert.ToInt64(subIP[2]) +
            //   256 * Convert.ToInt64(subIP[1]) +
            //   Convert.ToInt64(subIP[0]);

            //return ip;
        }


        /// <summary>
        /// 把长整形转换成类似1.2.3.4这样的ip返回
        /// </summary>
        /// <param name="longIp"></param>
        /// <returns></returns>
        public static string ConvertIp(long longIp)
        {
            byte[] arr = BitConverter.GetBytes(longIp);
            if (arr.Length > 4)
                arr = new byte[] { arr[0], arr[1], arr[2], arr[3] };
            return new System.Net.IPAddress(arr).ToString();
        }



        #region 加解密方法

        #region  MD5加密

        /// <summary>
        /// 标准MD5加密
        /// </summary>
        /// <param name="source">待加密字符串</param>
        /// <param name="addKey">附加字符串</param>
        /// <param name="encoding">编码方式</param>
        /// <returns></returns>
        public static string MD5_Encrypt(string source, string addKey, Encoding encoding)
        {
            if (!string.IsNullOrEmpty(addKey))
            {
                source = source + addKey;
            }

            MD5 MD5 = new MD5CryptoServiceProvider();
            byte[] datSource = encoding.GetBytes(source);
            byte[] newSource = MD5.ComputeHash(datSource);
            string byte2String = null;
            for (int i = 0; i < newSource.Length; i++)
            {
                string thisByte = newSource[i].ToString("x");
                if (thisByte.Length == 1) thisByte = "0" + thisByte;
                byte2String += thisByte;
            }
            return byte2String;
        }

        #endregion

        #region  DES 加解密

        public static string DES_Encrypt(string source, string key, Encoding encoding)
        {
            if (string.IsNullOrEmpty(source))
                return null;
            DESCryptoServiceProvider des = new DESCryptoServiceProvider();

            //把字符串放到byte数组中  
            byte[] inputByteArray = encoding.GetBytes(source);

            // 密钥必须是8位，否则会报错 System.ArgumentException: 指定键的大小对于此算法无效。
            if (key == null)
                key = "!@#ASD12";
            else
            {
                int len = key.Length;
                if (len > 8)
                    key = key.Substring(0, 8);
                else if (len < 8)
                    key = key.PadRight(8, '.'); // 不足8位，用小数点补足
            }

            //建立加密对象的密钥和偏移量  
            //原文使用ASCIIEncoding.ASCII方法的GetBytes方法  
            //使得输入密码必须输入英文文本  
            //            des.Key = UTF8Encoding.UTF8.GetBytes(key);
            //            des.IV  = UTF8Encoding.UTF8.GetBytes(key);
            des.Key = encoding.GetBytes(key);
            des.IV = encoding.GetBytes(key);

            MemoryStream ms = new MemoryStream();
            CryptoStream cs = new CryptoStream(ms, des.CreateEncryptor(), CryptoStreamMode.Write);

            cs.Write(inputByteArray, 0, inputByteArray.Length);
            cs.FlushFinalBlock();

            StringBuilder ret = new StringBuilder();
            foreach (byte b in ms.ToArray())
            {
                ret.AppendFormat("{0:X2}", b);
            }

            return ret.ToString();
        }


        public static string DES_Decrypt(string source, string key, Encoding encoding)
        {
            if (string.IsNullOrEmpty(source))
                return null;

            DESCryptoServiceProvider des = new DESCryptoServiceProvider();

            //将字符串转为字节数组  
            byte[] inputByteArray = new byte[source.Length / 2];
            for (int x = 0; x < source.Length / 2; x++)
            {
                int i = (Convert.ToInt32(source.Substring(x * 2, 2), 16));
                inputByteArray[x] = (byte)i;
            }

            // 密钥必须是8位，否则会报错 System.ArgumentException: 指定键的大小对于此算法无效。
            if (key == null)
                key = "!@#ASD12";
            else
            {
                int len = key.Length;
                if (len > 8)
                    key = key.Substring(0, 8);
                else if (len < 8)
                    key = key.PadRight(8, '.'); // 不足8位，用小数点补足
            }

            //建立加密对象的密钥和偏移量，此值重要，不能修改  
            des.Key = encoding.GetBytes(key);
            des.IV = encoding.GetBytes(key);

            MemoryStream ms = new MemoryStream();
            CryptoStream cs = new CryptoStream(ms, des.CreateDecryptor(), CryptoStreamMode.Write);
            cs.Write(inputByteArray, 0, inputByteArray.Length);
            cs.FlushFinalBlock();

            //建立StringBuild对象，CreateDecrypt使用的是流对象，必须把解密后的文本变成流对象  
            //StringBuilder ret = new StringBuilder();

            return encoding.GetString(ms.ToArray());
        }

        #endregion

        #region 3DES加解密

        private const string _default3desKey = "beinet.cn%G1&73#;0(=+)`!";//"abcpoiUUgG5%&73#;0(=+)`!";

        public static string TripleDES_Encrypt(string input, Encoding encoding, string key = null, string iv = null)
        {
            key = key ?? _default3desKey;
            int lenKey = 24 - key.Length;
            if (lenKey < 0)
                key = key.Substring(0, 24); // 太长时，只取前24位
            else if (lenKey > 0)
                key += _default3desKey.Substring(0, lenKey); // 太短时，补足24位

            iv = iv ?? key.Substring(0, 8);
            int lenIV = 8 - iv.Length;
            if (lenIV < 0)
                iv = iv.Substring(0, 24); // 太长时，只取前8位
            else if (lenIV > 0)
                iv += iv.Substring(0, lenIV); // 太短时，补足8位

            byte[] arrKey = encoding.GetBytes(key);
            byte[] arrIV = encoding.GetBytes(iv);

            // 获取加密后的字节数据
            byte[] arrData = encoding.GetBytes(input);
            byte[] result = TripleDesEncrypt(arrKey, arrIV, arrData);

            // 转换为16进制字符串
            StringBuilder ret = new StringBuilder();
            foreach (byte b in result)
            {
                ret.AppendFormat("{0:X2}", b);
            }
            return ret.ToString();
        }

        public static string TripleDES_Decrypt(string input, Encoding encoding, string key = null, string iv = null)
        {
            key = key ?? _default3desKey;
            int lenKey = 24 - key.Length;
            if (lenKey < 0)
                key = key.Substring(0, 24); // 太长时，只取前24位
            else if (lenKey > 0)
                key += _default3desKey.Substring(0, lenKey); // 太短时，补足24位

            iv = iv ?? key.Substring(0, 8);
            int lenIV = 8 - iv.Length;
            if (lenIV < 0)
                iv = iv.Substring(0, 24); // 太长时，只取前8位
            else if (lenIV > 0)
                iv += iv.Substring(0, lenIV); // 太短时，补足8位

            byte[] arrKey = encoding.GetBytes(key);
            byte[] arrIV = encoding.GetBytes(iv);

            // 获取加密后的字节数据
            int len = input.Length / 2;
            byte[] arrData = new byte[len];
            for (int x = 0; x < len; x++)
            {
                int i = (Convert.ToInt32(input.Substring(x * 2, 2), 16));
                arrData[x] = (byte)i;
            }

            byte[] result = TripleDesDecrypt(arrKey, arrIV, arrData);
            return encoding.GetString(result);
        }


        #region TripleDesEncrypt加密(3DES加密)

        /// <summary>
        /// 3Des加密，密钥长度必需是24字节
        /// </summary>
        /// <param name="key">密钥字节数组</param>
        /// <param name="iv">向量字节数组</param>
        /// <param name="source">源字节数组</param>
        /// <returns>加密后的字节数组</returns>
        private static byte[] TripleDesEncrypt(byte[] key, byte[] iv, byte[] source)
        {
            TripleDESCryptoServiceProvider dsp = new TripleDESCryptoServiceProvider();
            dsp.Mode = CipherMode.CBC; // 默认值
            dsp.Padding = PaddingMode.PKCS7; // 默认值
            using (MemoryStream mStream = new MemoryStream())
            using (
                CryptoStream cStream = new CryptoStream(mStream, dsp.CreateEncryptor(key, iv), CryptoStreamMode.Write))
            {
                cStream.Write(source, 0, source.Length);
                cStream.FlushFinalBlock();
                byte[] result = mStream.ToArray();
                cStream.Close();
                mStream.Close();
                return result;
            }
        }

        #endregion

        #region TripleDesDecrypt解密(3DES解密)

        /// <summary>
        /// 3Des解密，密钥长度必需是24字节
        /// </summary>
        /// <param name="key">密钥字节数组</param>
        /// <param name="iv">向量字节数组</param>
        /// <param name="source">加密后的字节数组</param>
        /// <param name="dataLen">解密后的数据长度</param>
        /// <returns>解密后的字节数组</returns>
        private static byte[] TripleDesDecrypt(byte[] key, byte[] iv, byte[] source, out int dataLen)
        {
            TripleDESCryptoServiceProvider dsp = new TripleDESCryptoServiceProvider();
            dsp.Mode = CipherMode.CBC; // 默认值
            dsp.Padding = PaddingMode.PKCS7; // 默认值
            using (MemoryStream mStream = new MemoryStream(source))
            using (CryptoStream cStream = new CryptoStream(mStream, dsp.CreateDecryptor(key, iv), CryptoStreamMode.Read)
                )
            {
                byte[] result = new byte[source.Length];
                dataLen = cStream.Read(result, 0, result.Length);
                cStream.Close();
                mStream.Close();
                return result;
            }
        }

        /// <summary>
        /// 3Des解密，密钥长度必需是24字节
        /// </summary>
        /// <param name="key">密钥字节数组</param>
        /// <param name="iv">向量字节数组</param>
        /// <param name="source">加密后的字节数组</param>
        /// <returns>解密后的字节数组</returns>
        private static byte[] TripleDesDecrypt(byte[] key, byte[] iv, byte[] source)
        {
            int dataLen;
            byte[] result = TripleDesDecrypt(key, iv, source, out dataLen);

            if (result.Length != dataLen)
            {
                // 如果数组长度不是解密后的实际长度，需要截断多余的数据，用来解决Gzip的"Magic byte doesn't match"的问题
                byte[] resultToReturn = new byte[dataLen];
                Array.Copy(result, resultToReturn, dataLen);
                return resultToReturn;
            }
            else
                return result;
        }

        #endregion



        #endregion

        #region Base64
        public static string HttpBase64Encode(string source, Encoding encoding)
        {
            //空串处理
            if (string.IsNullOrEmpty(source))
            {
                return "";
            }

            //编码
            string encodeString = Convert.ToBase64String(encoding.GetBytes(source));

            ////过滤
            //encodeString = encodeString.Replace("+", "~");
            //encodeString = encodeString.Replace("/", "@");
            //encodeString = encodeString.Replace("=", "$");

            //返回
            return encodeString;
        }


        public static string HttpBase64Decode(string source, Encoding encoding)
        {
            //空串处理
            if (string.IsNullOrEmpty(source))
            {
                return "";
            }

            ////还原
            string deocdeString = source;
            //deocdeString = deocdeString.Replace("~", "+");
            //deocdeString = deocdeString.Replace("@", "/");
            //deocdeString = deocdeString.Replace("$", "=");

            //Base64解码
            deocdeString = encoding.GetString(Convert.FromBase64String(deocdeString));

            //返回
            return deocdeString;
        }
        #endregion

        /// <summary>
        /// 在32位系统和64位系统上生成的HashCode会不一致，请统一使用此方法获取哈希值
        /// </summary>
        /// <param name="s"></param>
        /// <param name="retAbs">是否只返回绝对值</param>
        /// <returns></returns>
        public static unsafe int GetHashCode32(string s, bool retAbs)
        {
            fixed (char* chRef = s.ToCharArray())
            {
                char* chPtr = chRef;
                int num = 0x15051505;
                int num2 = num;
                int* numPtr = (int*)chPtr;
                for (int i = s.Length; i > 0; i -= 4)
                {
                    num = (((num << 5) + num) + (num >> 0x1b)) ^ numPtr[0];
                    if (i <= 2)
                    {
                        break;
                    }
                    num2 = (((num2 << 5) + num2) + (num2 >> 0x1b)) ^ numPtr[1];
                    numPtr += 2;
                }
                int ret = num + (num2 * 0x5d588b65);
                if (retAbs)
                {
                    // 如果ret是int的最小值，取绝对值时，会报错：对 2 的补数的最小值求反的操作无效
                    if (ret == int.MinValue)
                        ret = int.MaxValue;
                    else
                        ret = Math.Abs(ret);
                }
                return ret;
            }
        }
    
        #endregion


        public static string GetPage(string url, string postData, string proxy)
        {
            string result = null;
            return GetPage(url, postData, "POST", Encoding.UTF8, false, proxy, ref result);
        }

        public static string GetPage(string url, string param, string HttpMethod,
            Encoding encoding, bool showHeader, string proxy, ref string result)
        {
            // 增加随机数，避免缓存
            var rnd = Guid.NewGuid().GetHashCode().ToString();
            if (url.IndexOf('?') > 0)
                url += "&" + rnd;
            else
                url += "?" + rnd;
            
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
                result = "未启用压缩 =======";
                Stream stream2;
                using (stream2 = response.GetResponseStream())
                {
                    if (response.ContentEncoding.ToLower().Contains("gzip"))
                    {
                        result = "已启用压缩 gzip   ";
                        stream2 = new GZipStream(stream2, CompressionMode.Decompress);
                    }
                    else if (response.ContentEncoding.ToLower().Contains("deflate"))
                    {
                        result = "已启用压缩 deflate";
                        stream2 = new DeflateStream(stream2, CompressionMode.Decompress);
                    }
                    if (stream2 == null)
                        return "null stream";
                    using (StreamReader reader = new StreamReader(stream2, encoding))
                    {
                        result = result + DateTime.Now.ToString(" yyyy-MM-dd HH:mm:ss_fff");
                        string str = reader.ReadToEnd();
                        if (showHeader)
                        {
                            str = string.Concat(new object[] { "请求头信息：\r\n", request.Headers, "\r\n\r\n响应头信息：\r\n", response.Headers, "\r\n", str });
                        }
                        return str;
                    }
                }
            }
            result = "其它结果";
            return string.Concat(new object[] { "远程服务器返回代码不为200,", response.StatusCode, ",", response.StatusDescription });
        }

    }
}
