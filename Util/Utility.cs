using System;
using System.Collections.Generic;
using System.Configuration;
using System.Diagnostics;
using System.IO;
using System.IO.Compression;
using System.Net;
using System.Net.Sockets;
using System.Reflection;
using System.Runtime.Serialization;
using System.Security.Cryptography;
using System.Text;
using System.Threading;
using System.Windows.Forms;
using System.Xml;
using System.Drawing;
using System.Net.Security;
using System.Security.Cryptography.X509Certificates;
using System.Text.RegularExpressions;
using Newtonsoft.Json;
using Formatting = System.Xml.Formatting;

namespace Beinet.cn.Tools
{
    public static class Utility
    {
        public static string Dir { get; } = AppDomain.CurrentDomain.BaseDirectory;

        static Utility()
        {
            //调大默认连接池
            ServicePointManager.DefaultConnectionLimit = 1024;
            //连接池中的TCP连接不使用Nagle算法
            ServicePointManager.UseNagleAlgorithm = false;

            // 如果证书有问题，导致异常： 远程主机强迫关闭了一个现有的连接，要加这一句，或调整一下别的安全协议
            ServicePointManager.SecurityProtocol = System.Net.SecurityProtocolType.Tls12;

            //https证书验证回掉
            ServicePointManager.ServerCertificateValidationCallback = CheckValidationResult;
        }
        static bool CheckValidationResult(object sender, X509Certificate certificate, X509Chain chain, SslPolicyErrors errors)
        {
            // Always accept
            //Console.WriteLine("accept" + certificate.GetName());
            return true; //总是接受
        }

        private static readonly string _path = AppDomain.CurrentDomain.BaseDirectory;
        /// <summary>
        /// exe的启动目录
        /// </summary>
        public static string StartPath { get { return _path; } }

        /// <summary>
        /// 是否ip的正则
        /// </summary>
        static Regex regIp = new Regex(@"^\d{1,3}\.\d{1,3}\.\d{1,3}\.\d{1,3}$", RegexOptions.Compiled);

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
            if (frm == null || frm.IsDisposed)
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
                catch (Exception exp)
                {
                    MessageBox.Show(frm, exp.ToString());
                }
            }
        }


        public static void BindToDataGrid(DataGridView dgv, List<string[]> arrData, List<int> rowColor = null)
        {
            if (arrData == null || arrData.Count <= 0)
            {
                return;
            }
            // 用于隔行变色
            Color[] rowBack = { Color.AliceBlue, Color.AntiqueWhite };
            int idx = 0;
            Color color;

            // 为了后面Clone作准备，懒得研究和改DataTable了
            if (dgv.Rows.Count <= 0)
            {
                color = rowColor != null ? rowBack[rowColor[idx]] : rowBack[idx];
                var color1 = color;
                string[] tmpData = arrData[0];
                InvokeControl(dgv, () =>
                {
                    // ReSharper disable once CoVariantArrayConversion
                    dgv.Rows.Add(tmpData);
                    dgv.Rows[0].DefaultCellStyle.BackColor = color1;
                });
                arrData.RemoveAt(0);
                if (arrData.Count <= 0)
                {
                    return;
                }
                idx++;
            }

            var arrRow = new List<DataGridViewRow>();
            foreach (var item in arrData)
            {
                DataGridViewRow row = (DataGridViewRow)dgv.Rows[0].Clone();
                for (var i = 0; i < item.Length; i++)
                {
                    row.Cells[i].Value = item[i];
                }
                color = rowColor != null ? rowBack[rowColor[idx]] : rowBack[idx % 2];
                idx++;
                row.DefaultCellStyle.BackColor = color;
                arrRow.Add(row);
            }
            var tmpArr = arrRow.ToArray();
            InvokeControl(dgv, () => {
                dgv.Rows.AddRange(tmpArr);
            });
        }

        //private delegate void SetValueDele(Control ctl, string propName, object val);
        public static void SetValue(Control ctl, string propName, object val)
        {
            InvokeControl(ctl, () => SetValue((object)ctl, propName, val));// 转换后调用下面的SetValue重载

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
        /// 把对象序列化为Xml字符串
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="obj"></param>
        /// <returns></returns>
        public static string XmlSerialize<T>(T obj)
        {
            DataContractSerializer formatter = new DataContractSerializer(typeof(T));
            using (MemoryStream memory = new MemoryStream())
            {
                formatter.WriteObject(memory, obj);
                memory.Seek(0, SeekOrigin.Begin);
                using (StreamReader sr = new StreamReader(memory, Encoding.UTF8))
                {
                    return sr.ReadToEnd();
                }
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
                return (T)xs.ReadObject(xmlreader);
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

            //if (!regIp.IsMatch(strIp))
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
        /*
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
        */
        #endregion


        /// <summary>
        /// 
        /// </summary>
        /// <param name="url"></param>
        /// <param name="param"></param>
        /// <param name="isPost"></param>
        /// <param name="isJson"></param>
        /// <param name="encoding"></param>
        /// <param name="showHeader"></param>
        /// <param name="proxy"></param>
        /// <param name="allowRedirect"></param>
        /// <param name="headers">要添加的header</param>
        /// <returns></returns>
        public static string GetPage(string url, string param = null, string proxy = null, bool isPost = false,
            bool isJson = false, Encoding encoding = null, bool showHeader = false, bool allowRedirect = false,
            Dictionary<string, string> headers = null)
        {
            if (encoding == null)
                encoding = Encoding.UTF8;

            // 增加随机数，避免缓存
            var rnd = Guid.NewGuid().GetHashCode().ToString();
            if (url.IndexOf('?') > 0)
                url += "&" + rnd;
            else
                url += "?" + rnd;

            if (!isPost && !string.IsNullOrEmpty(param))
            {
                url = url + "&" + param;
            }

            var needSetHost = !string.IsNullOrEmpty(proxy);
            if (needSetHost)
            {
                // 不再使用proxy方案，改用替换url里的host为ip，并设置header里的host实现
                SwitchHost(ref url, ref proxy);
            }

            string contentType = null;

            HttpWebRequest request = (HttpWebRequest) WebRequest.Create(url);
            request.Referer = url;
            request.AllowAutoRedirect = allowRedirect;
            request.UserAgent = "Mozilla/4.0 (compatible; MSIE 6.0; Windows NT 5.2; SV1;)";
            request.Headers.Add("Accept-Encoding", "gzip, deflate");
            if (headers != null)
            {
                foreach (var pair in headers)
                {
                    if (pair.Key == null || pair.Value == null)
                        continue;
                    if (pair.Key.Equals("ContentType", StringComparison.OrdinalIgnoreCase) ||
                        pair.Key.Equals("Content-Type", StringComparison.OrdinalIgnoreCase))
                        contentType = pair.Value;
                    else
                        request.Headers[pair.Key] = pair.Value; // 不用add，避免跟前面的key重复
                }
            }
            request.Timeout = 30000;
            if (isJson)
            {
                request.Accept = "application/json";
            }
            else
            {
                request.Accept = "*/*";
            }

            // 必须在写入Post Stream之前设置Proxy
            if (needSetHost)
            {
                request.Host = proxy.Split(':')[0]; // 避免替换出来的域名带了端口
                // request.Proxy = null;// 直接连接，避免fiddler干扰, 在Web.Config里配置

                //#region 设置代理
                //string[] tmp = proxy.Split(new[] { ':' }, StringSplitOptions.RemoveEmptyEntries);
                //int port = 80;
                //if (tmp.Length >= 2)
                //{
                //    if (!int.TryParse(tmp[1], out port))
                //    {
                //        port = 80;
                //    }
                //}
                //request.Proxy = new WebProxy(tmp[0], port);
                //#endregion
            }

            if (!isPost)
            {
                request.Method = "GET";
                request.ContentType = contentType ?? "text/html";
            }
            else
            {
                request.Method = "POST";
                request.ContentType = contentType ?? "application/x-www-form-urlencoded";
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
                    request.ContentLength = 0; // POST时，必须设置ContentLength属性
            }

            HttpWebResponse response = (HttpWebResponse) request.GetResponse();
            if (response.StatusCode == HttpStatusCode.OK)
            {
                Stream stream2;
                using (stream2 = response.GetResponseStream())
                {
                    if (stream2 == null)
                        return "GetResponseStream is null";
                    string contentEncoding = response.ContentEncoding.ToLower();
                    if (contentEncoding.Contains("gzip"))
                    {
                        stream2 = new GZipStream(stream2, CompressionMode.Decompress);
                    }
                    else if (contentEncoding.Contains("deflate"))
                    {
                        stream2 = new DeflateStream(stream2, CompressionMode.Decompress);
                    }

                    using (StreamReader reader = new StreamReader(stream2, encoding))
                    {
                        string str = reader.ReadToEnd();
                        if (showHeader)
                        {
                            str = string.Concat(new object[]
                                {"请求头信息：\r\n", request.Headers, "\r\n\r\n响应头信息：\r\n", response.Headers, "\r\n", str});
                        }

                        return str;
                    }
                }
            }

            string errMsg = string.Format("Response.StatusCode:{0}, {1}", response.StatusCode,
                response.StatusDescription);
            throw new Exception(errMsg);
        }

        /// <summary>
        /// 替换url里的主机和ip
        /// </summary>
        /// <param name="url"></param>
        /// <param name="hostip"></param>
        public static void SwitchHost(ref string url, ref string hostip)
        {
            var urlHost = GetHostFromUrl(url);
            if (urlHost == String.Empty)
            {
                return;
            }
            if (string.IsNullOrEmpty(hostip) || !regIp.IsMatch(hostip.Split(':')[0]))
            {
                return;
            }
            url = url.Replace(urlHost, hostip);
            hostip = urlHost.Split(':')[0];
        }

        /// <summary>
        /// 从url中截取出域名和端口
        /// </summary>
        /// <param name="url"></param>
        /// <returns></returns>
        public static string GetHostFromUrl(string url)
        {
            if (string.IsNullOrWhiteSpace(url))
            {
                return string.Empty;
            }
            if (url.StartsWith("http://", StringComparison.OrdinalIgnoreCase))
            {
                url = url.Substring("http://".Length);
            }
            else if (url.StartsWith("https://", StringComparison.OrdinalIgnoreCase))
            {
                url = url.Substring("https://".Length);
            }

            var idx = url.IndexOf('/');
            if (idx == 0)
            {
                return string.Empty;
            }
            if (idx < 0)
            {
                return url;
            }
            return url.Substring(0, idx);
        }

        /// <summary>
        /// 获取本机所有IPV4地址列表
        /// </summary>
        /// <returns>本机所有IPV4地址列表，以分号分隔</returns>
        public static string GetServerIpList()
        {
            try
            {
                StringBuilder ips = new StringBuilder();
                IPHostEntry IpEntry = Dns.GetHostEntry(Dns.GetHostName());
                foreach (IPAddress ipa in IpEntry.AddressList)
                {
                    if (ipa.AddressFamily == AddressFamily.InterNetwork)
                        ips.AppendFormat("{0};", ipa);
                }
                return ips.ToString();
            }
            catch (Exception)
            {
                //LogHelper.WriteCustom("获取本地ip错误" + ex, @"zIP\", false);
                return string.Empty;
            }
        }


        private static HashSet<string> _dirNoProcess;
        /// <summary>
        /// Dll分析 或 文件MD5 处理时要跳过的目录列表，以分号分隔
        /// </summary>
        public static HashSet<string> DirNoProcess
        {
            get
            {
                if (_dirNoProcess == null)
                {
                    var ret = new HashSet<string>();
                    string tmp = ConfigurationManager.AppSettings["DirNoProcess"] ?? ".svn;.git";
                    foreach (string str in tmp.Split(new[] { ';', '|' }, StringSplitOptions.RemoveEmptyEntries))
                    {
                        string item = str.Trim();
                        if (item != string.Empty)
                            ret.Add(item.ToLower());
                    }

                    _dirNoProcess = ret;
                }
                return _dirNoProcess;
            }
        }


        /// <summary>
        /// 通过多线程同步执行多个方法
        /// </summary>
        /// <param name="methods"></param>
        public static void ThreadRun(params ThreadStart[] methods)
        {
            List<Thread> arrTh = new List<Thread>();
            foreach (ThreadStart method in methods)
            {
                Thread th = new Thread(method);
                th.IsBackground = true;
                arrTh.Add(th);
                th.Start();
            }

            // 阻塞所有线程，等待结束
            foreach (Thread thread in arrTh)
            {
                thread.Join();
            }
        }



        public static string ExecCmd(string cmd, string args)
        {
            try
            {
                using (var p = new Process())
                {
                    p.StartInfo.FileName = cmd;//"cmd.exe";
                    p.StartInfo.Arguments = args;
                    p.StartInfo.UseShellExecute = false; // 在当前进程中启动，不使用系统外壳程序启动
                    p.StartInfo.RedirectStandardInput = false; //设置为true，后面可以通过StandardInput输入dos命令
                    p.StartInfo.RedirectStandardOutput = true;
                    p.StartInfo.RedirectStandardError = true;
                    p.StartInfo.CreateNoWindow = true; //不创建窗口
                    p.Start();
                    // string _dosPing = "ping -n {0} -l 512 ";// 修改这个，必须同时修改上面的正则
                    // string _dosExit = "exit";// 修改这个，必须同时修改上面的正则
                    // p.StandardInput.WriteLine(dosping + ipOrNs);
                    // p.StandardInput.WriteLine(_dosExit);
                    string strRst = p.StandardOutput.ReadToEnd();
                    return strRst;

                    //p.StartInfo.FileName = "cmd.exe";
                    //p.StartInfo.UseShellExecute = true; // 在当前进程中启动，不使用系统外壳程序启动
                    ////p.StartInfo.WindowStyle = ProcessWindowStyle.Maximized;// 让dos窗体最大化
                    //p.StartInfo.Arguments = "/C ping " + ipOrNs; //设定参数，其中的“/C”表示执行完命令后马上退出
                    //p.StartInfo.RedirectStandardInput = false; //设置为true，后面可以通过StandardInput输入dos命令
                    //p.StartInfo.RedirectStandardOutput = false;
                    ////p.StartInfo.CreateNoWindow = true;     //不创建窗口
                    //p.Start();
                    ////SetWindowPos(p.Handle, 3, Left, Top, Width, Height, 8);
                    ////p.StandardInput.WriteLine("ping " + url);
                    //p.WaitForExit(1000);
                    ////MessageBox.Show(p.StandardOutput.ReadToEnd());
                    //p.Close();
                }
            }
            catch (Exception exp)
            {
                return exp.Message;
            }
        }


        public static bool SetConfigValue(string AppKey, string AppValue)
        {
            string configFile = System.Windows.Forms.Application.ExecutablePath + ".config";
            if (!File.Exists(configFile))
            {
                using (StreamWriter sw = new StreamWriter(configFile, false, Encoding.UTF8))
                {
                    sw.Write(@"<?xml version=""1.0""?><configuration><appSettings></appSettings></configuration>");
                }
            }
            System.Xml.XmlDocument xDoc = new System.Xml.XmlDocument();
            xDoc.Load(configFile);

            System.Xml.XmlNode xNode;
            System.Xml.XmlElement xElem1;
            System.Xml.XmlElement xElem2;
            xNode = xDoc.SelectSingleNode("//appSettings");
            if (xNode == null)
                return false;

            xElem1 = (System.Xml.XmlElement)xNode.SelectSingleNode("//add[@key='" + AppKey + "']");
            if (xElem1 != null) xElem1.SetAttribute("value", AppValue);
            else
            {
                xElem2 = xDoc.CreateElement("add");
                xElem2.SetAttribute("key", AppKey);
                xElem2.SetAttribute("value", AppValue);
                xNode.AppendChild(xElem2);
            }
            xDoc.Save(configFile);
            return true;
        }

        public static void txt_KeyDownUp(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.A && e.Control && !e.Alt && !e.Shift)
            {
                var txtBox = sender as TextBox;
                if (txtBox != null)
                {
                    txtBox.SelectAll();
                }
                else
                {
                    var richBox = sender as RichTextBox;
                    richBox?.SelectAll();
                }
                e.Handled = true;
                //e.SuppressKeyPress = true;
            }
        }



        /// <summary>
        /// 对象生成Json串
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="source"></param>
        /// <returns></returns>
        public static string ToJson<T>(T source)
        {
            return JsonConvert.SerializeObject(source);
            /* // 微软原生，效率低
            System.Runtime.Serialization.Json.DataContractJsonSerializer jsonSerializer = new DataContractJsonSerializer(typeof(T));
            using (MemoryStream ms = new MemoryStream())
            {
                jsonSerializer.WriteObject(ms, source);
                StringBuilder sb = new StringBuilder();
                sb.Append(Encoding.UTF8.GetString(ms.ToArray()));

                return sb.ToString();
            }
            */
        }

        /// <summary>
        /// 字符串转对象
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="source"></param>
        /// <returns></returns>
        public static T FromJson<T>(string source)
        {
            return JsonConvert.DeserializeObject<T>(source);
            /*// 微软原生，效率低
            System.Runtime.Serialization.Json.DataContractJsonSerializer jsonSerializer = new DataContractJsonSerializer(typeof(T));
            using (MemoryStream ms = new MemoryStream(Encoding.UTF8.GetBytes(source)))
            {
                T obj = (T)jsonSerializer.ReadObject(ms);
                return obj;
            }
            */
        }


        public static void OpenDir(string dir)
        {
            if (string.IsNullOrEmpty(dir))
            {
                MessageBox.Show("目录不能为空");
                return;
            }
            // %SystemDrive%环境变量转换
            dir = Environment.ExpandEnvironmentVariables(dir);
            if (!Directory.Exists(dir))
            {
                MessageBox.Show("目录不存在：" + dir);
                return;
            }
            Process.Start("explorer", dir);
        }
    }
}
