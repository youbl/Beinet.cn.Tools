using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Data.SqlClient;
using System.IO;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Text.RegularExpressions;

namespace Beinet.cn.Tools.QQWry
{
    /// <summary>
    /// 纯真IP数据库操作类,首次使用，请先调用静态方法Initialize(dat文件路径)进行初始化，
    /// 再通过IPLocator.Instance.Query查询
    /// </summary>
    public class IPLocator
    {
        private byte[] data;
        private static IPLocator _instance;
        // 保存初始化使用的文件，避免反复初始化
        private static string _instanceFilePath;
        private long _firstStartIpOffset, _lastStartIpOffset;
        // 构造函数私有化，不允许外部实例化
        private IPLocator()
        {
        }

        /// <summary>
        /// 返回总ip纪录数
        /// </summary>
        public long Count { get; private set; }

        // 返回唯一实例
        private static IPLocator Instance
        {
            get
            {
                if (_instance == null)
                {
                    throw new Exception("请先调用Initialize方法进行初始化");
                }
                return _instance;
            }
        }


        #region 静态方法
        /// <summary>
        /// 初始化方法，必须先调用这个方法进行初始化
        /// </summary>
        /// <param name="dataPath">纯真IP数据库文件地址</param>
        public static void Initialize(string dataPath)
        {
            if (dataPath == _instanceFilePath)
                return;

                //throw new Exception("请不要反复初始化同一文件路径");
            _instanceFilePath = dataPath;

            var loc = new IPLocator();
            using (FileStream fs = File.OpenRead(dataPath))
            {
                loc.data = new byte[fs.Length];
                fs.Read(loc.data, 0, loc.data.Length);
            }
            byte[] buffer = new byte[8];
            Array.Copy(loc.data, 0, buffer, 0, 8);
            loc._firstStartIpOffset = ((buffer[0] + (buffer[1] * 0x100)) + ((buffer[2] * 0x100) * 0x100)) + (((buffer[3] * 0x100) * 0x100) * 0x100);
            loc._lastStartIpOffset = ((buffer[4] + (buffer[5] * 0x100)) + ((buffer[6] * 0x100) * 0x100)) + (((buffer[7] * 0x100) * 0x100) * 0x100);
            loc.Count = Convert.ToInt64((((loc._lastStartIpOffset - loc._firstStartIpOffset)) / 7d));

            if (loc.Count <= 1L)
            {
                throw new ArgumentException("IP File DataError.");
            }

            _instance = loc;
        }


        /// <summary>
        /// 查询并返回指定IPv4的实体，null表示不是正确IP，或属于环回地址，或属于局域网地址
        /// </summary>
        /// <param name="ip"></param>
        /// <param name="dataPath"></param>
        /// <returns></returns>
        public static IPLocation Query(string ip, string dataPath)
        {
            if (dataPath != _instanceFilePath)
                Initialize(dataPath);
            return Query(ip);
        }


        /// <summary>
        /// 查询并返回指定IPv4的实体，null表示不是正确IP，或属于环回地址，或属于局域网地址
        /// </summary>
        /// <param name="ip"></param>
        /// <returns></returns>
        public static IPLocation Query(string ip)
        {
            IPLocator objLocator = Instance;

            IPLocation ipLocation = new IPLocation() { IP = ip };

            IPAddress ipa;
            if (!IPAddress.TryParse(ip, out ipa))
            {
                ipLocation.Country = "非法IP地址";
                return ipLocation;
            }

            if (IPAddress.IsLoopback(ipa))
            {
                ipLocation.Country = "本地环回地址";
                return ipLocation;
            }
            if (IsLocalNetwork(ipa))
            {
                ipLocation.Country = "局域网地址";
                return ipLocation;
            }

            long intIP = ConvertIpToLong(ip);

            long right = objLocator.Count;
            long left = 0L;
            long middle;
            long startIp;
            long endIpOff;
            long endIp;
            int countryFlag;
            while (left < (right - 1L))
            {
                middle = (right + left) / 2L;
                startIp = objLocator.GetStartIp(middle, out endIpOff);
                if (intIP == startIp)
                {
                    left = middle;
                    break;
                }
                if (intIP > startIp)
                {
                    left = middle;
                }
                else
                {
                    right = middle;
                }
            }
            startIp = objLocator.GetStartIp(left, out endIpOff);
            endIp = objLocator.GetEndIp(endIpOff, out countryFlag);
            if ((startIp <= intIP) && (endIp >= intIP))
            {
                string local;
                ipLocation.Country = objLocator.GetCountry(endIpOff, countryFlag, out local);
                ipLocation.Area = local;
            }
            else
            {
                ipLocation.Country = "未知国家";
                ipLocation.Area = "未知地区";
            }
            return ipLocation;
        }

        /// <summary>
        /// 把ipv4格式转换为长整型
        /// </summary>
        /// <param name="ip"></param>
        /// <returns></returns>
        public static long ConvertIpToLong(string ip)
        {
            string[] arr = ip.Split('.');
            if (arr.Length != 4)
                return 0L;
            try
            {
                return (long.Parse(arr[0]) << 24) | (long.Parse(arr[1]) << 16) | (long.Parse(arr[2]) << 8) | long.Parse(arr[3]);
            }
            catch
            {
                return 0L;
            }
        }

        /// <summary>
        /// 把长整型转换为ipv4格式
        /// </summary>
        /// <param name="ip"></param>
        /// <returns></returns>
        public static string ConvertLongToIP(long ip)
        {
            return string.Format("{0}.{1}.{2}.{3}",
                                    (ip & 0xff000000L) >> 24,
                                    (ip & 0xff0000L) >> 16,
                                    (ip & 0xff00L) >> 8,
                                    ip & 0xffL
                                    );
        }

        /// <summary>
        /// 判断是否局域网地址
        /// </summary>
        /// <param name="ip"></param>
        /// <returns></returns>
        public static bool IsLocalNetwork(IPAddress ip)
        {
            if (ip.AddressFamily == AddressFamily.InterNetwork)
            {
                //byte[] b = ip.GetAddressBytes();

                return IPAddress.IsLoopback(ip) ||
                    IsSubnet(ip, IPAddress.Parse("10.0.0.0"), IPAddress.Parse("255.0.0.0")) ||
                    IsSubnet(ip, IPAddress.Parse("172.16.0.0"), IPAddress.Parse("255.240.0.0")) ||
                    IsSubnet(ip, IPAddress.Parse("192.168.0.0"), IPAddress.Parse("255.255.0.0"));

            }
            else
            {
                return false;
            }
        }

        /// <summary>
        /// 判断ip是否属于subnet网段
        /// </summary>
        /// <param name="ip">IP</param>
        /// <param name="subnet">网段</param>
        /// <param name="mask">子网掩码</param>
        /// <returns></returns>
        public static bool IsSubnet(IPAddress ip, IPAddress subnet, IPAddress mask)
        {
            byte[] a = ip.GetAddressBytes();
            byte[] s = subnet.GetAddressBytes();
            byte[] m = mask.GetAddressBytes();

            bool r = true;
            for (int i = 0; i < a.Length; i++)
            {
                if ((a[i] & m[i]) != s[i])
                {
                    r = false;
                }
            }
            return r;
        }

        /// <summary>
        /// 判断输入的字符串，是否有效的IPv4地址
        /// </summary>
        /// <param name="strIPAddress"></param>
        /// <returns></returns>
        public static bool IsValidIPAddress(string strIPAddress)
        {
            return Regex.IsMatch(strIPAddress,
                @"^(\d{1,2}|1\d\d|2[0-4]\d|25[0-5])\.(\d{1,2}|1\d\d|2[0-4]\d|25[0-5])\.(\d{1,2}|1\d\d|2[0-4]\d|25[0-5])\.(\d{1,2}|1\d\d|2[0-4]\d|25[0-5])$");
        }
        #endregion


        #region 导出到数据库相关方法

        /// <summary>
        /// 把全部ip数据导入SqlServer数据库(注意：会清空目标表）
        /// </summary>
        /// <param name="connectionString"></param>
        /// <param name="tbName">表如果存在，会清空；如果不存在，会自动创建</param>
        public static void ExportToSqlServer(string connectionString, string tbName)
        {
            ExistsAndCreateTable(connectionString, tbName);

            long right = Instance.Count;
            long left = 0L;
            long startIp;
            long endIpOff;
            long endIp;
            int countryFlag;
            long startLast = 0, endLast = 0;
            var dt = new DataTable();
            dt.Columns.Add("startip", typeof(long));
            dt.Columns.Add("endip", typeof(long));
            dt.Columns.Add("country", typeof(string));
            dt.Columns.Add("area", typeof(string));
            dt.Columns.Add("startip1", typeof(string));
            dt.Columns.Add("endip1", typeof(string));
            DataRow row;

            while (left < (right - 1L))
            {
                startIp = Instance.GetStartIp(left, out endIpOff);
                endIp = Instance.GetEndIp(endIpOff, out countryFlag);
                if (startIp == startLast && endIp == endLast)
                    continue;
                string local;
                string Country = Instance.GetCountry(endIpOff, countryFlag, out local);
                row = dt.NewRow();
                row[0] = startIp;
                row[1] = endIp;
                row[2] = Country;
                row[3] = local;
                row[4] = ConvertLongToIP(startIp);
                row[5] = ConvertLongToIP(endIp);
                dt.Rows.Add(row);

                startLast = startIp;
                endLast = endIp;
                left++;
            }
            Console.WriteLine(DateTime.Now + " 开始BulkCopy");
            BulkCopy(dt, connectionString, tbName);
        }


        // 判断表是否存在，不存在时创建它
        static void ExistsAndCreateTable(string connstr, string tableName)
        {
            using (var conn = new SqlConnection(connstr))
            using (var command = conn.CreateCommand())
            {
                string sql = "select count(1) from sys.objects where type='U' and name = @tbName";
                command.CommandText = sql;
                SqlParameter para = new SqlParameter("@tbName", SqlDbType.VarChar, 100) { Value = tableName };
                command.Parameters.Add(para);
                conn.Open();
                if (Convert.ToInt32(command.ExecuteScalar()) <= 0)
                    sql = @"CREATE TABLE [" + tableName + @"](
	[startip] [bigint],
	[endip] [bigint],
	[country] [varchar](100),
	[area] [varchar](300),
	[startip1] [varchar](20),
	[endip1] [varchar](20)
)  ";
                else
                    sql = "truncate table " + tableName;
                command.CommandText = sql;
                command.Parameters.Clear();
                command.ExecuteNonQuery();
            }
        }
        #endregion


        #region 私有方法
        private long GetStartIp(long left, out long endIpOff)
        {
            long leftOffset = _firstStartIpOffset + (left * 7L);
            byte[] buffer = new byte[7];
            Array.Copy(data, leftOffset, buffer, 0, 7);
            endIpOff = (Convert.ToInt64(buffer[4].ToString()) + (Convert.ToInt64(buffer[5].ToString()) * 0x100L)) + ((Convert.ToInt64(buffer[6].ToString()) * 0x100L) * 0x100L);
            return ((Convert.ToInt64(buffer[0].ToString()) + (Convert.ToInt64(buffer[1].ToString()) * 0x100L)) + ((Convert.ToInt64(buffer[2].ToString()) * 0x100L) * 0x100L)) + (((Convert.ToInt64(buffer[3].ToString()) * 0x100L) * 0x100L) * 0x100L);
        }

        private long GetEndIp(long endIpOff, out int countryFlag)
        {
            byte[] buffer = new byte[5];
            Array.Copy(data, endIpOff, buffer, 0, 5);
            countryFlag = buffer[4];
            return ((Convert.ToInt64(buffer[0].ToString()) + (Convert.ToInt64(buffer[1].ToString()) * 0x100L)) + ((Convert.ToInt64(buffer[2].ToString()) * 0x100L) * 0x100L)) + (((Convert.ToInt64(buffer[3].ToString()) * 0x100L) * 0x100L) * 0x100L);
        }

        private string GetCountry(long endIpOff, int countryFlag, out string local)
        {
            string country;
            long offset = endIpOff + 4L;
            switch (countryFlag)
            {
                case 1:
                case 2:
                    country = GetFlagStr(ref offset, ref countryFlag, ref endIpOff);
                    offset = endIpOff + 8L;
                    local = (1 == countryFlag) ? "" : GetFlagStr(ref offset, ref countryFlag, ref endIpOff);
                    break;
                default:
                    country = GetFlagStr(ref offset, ref countryFlag, ref endIpOff);
                    local = GetFlagStr(ref offset, ref countryFlag, ref endIpOff);
                    break;
            }
            return country;
        }

        private string GetFlagStr(ref long offset, ref int countryFlag, ref long endIpOff)
        {
            int flag;
            byte[] buffer = new byte[3];

            while (true)
            {
                //用于向前累加偏移量
                long forwardOffset = offset;
                flag = data[forwardOffset++];
                //没有重定向
                if (flag != 1 && flag != 2)
                {
                    break;
                }
                Array.Copy(data, forwardOffset, buffer, 0, 3);
                //forwardOffset += 3;
                if (flag == 2)
                {
                    countryFlag = 2;
                    endIpOff = offset - 4L;
                }
                offset = (Convert.ToInt64(buffer[0].ToString()) + (Convert.ToInt64(buffer[1].ToString()) * 0x100L)) + ((Convert.ToInt64(buffer[2].ToString()) * 0x100L) * 0x100L);
            }
            if (offset < 12L)
            {
                return "";
            }
            return GetStr(ref offset);
        }

        private string GetStr(ref long offset)
        {
            byte lowByte;
            byte highByte;
            StringBuilder stringBuilder = new StringBuilder();
            byte[] bytes = new byte[2];
            Encoding encoding = Encoding.GetEncoding("GB2312");
            while (true)
            {
                lowByte = data[offset++];
                if (lowByte == 0)
                {
                    return stringBuilder.ToString();
                }
                if (lowByte > 0x7f)
                {
                    highByte = data[offset++];
                    bytes[0] = lowByte;
                    bytes[1] = highByte;
                    if (highByte == 0)
                    {
                        return stringBuilder.ToString();
                    }
                    stringBuilder.Append(encoding.GetString(bytes));
                }
                else
                {
                    stringBuilder.Append((char)lowByte);
                }
            }
        }
        #endregion



        /// <summary>
        /// 把DataReader里的数据通过SqlBulkCopy复制到目标数据库的指定表中
        /// </summary>
        /// <param name="data">数据源</param>
        /// <param name="targetConnectionString">目标数据库连接字符串</param>
        /// <param name="targetTableName">目标表名</param>
        /// <param name="timeOut">超时之前操作完成所需的秒数</param>
        /// <param name="keepIdentity">是否保留源标识值，如果为false，则由目标分配标识值</param>
        /// <param name="batchSize">每一批次中的行数。在每一批次结束时，将该批次中的行发送到服务器</param>
        /// <param name="copiedEventHandler">在每次处理完 batchSize条记录时触发此事件</param>
        static void BulkCopy(DataTable data,
            string targetConnectionString, string targetTableName,
            int timeOut = 30, bool keepIdentity = true, int batchSize = 2000,
            SqlRowsCopiedEventHandler copiedEventHandler = null)
        {
            IDataReader sourceData = data.CreateDataReader();
            var opn = keepIdentity ? SqlBulkCopyOptions.KeepIdentity : SqlBulkCopyOptions.Default;
            using (SqlBulkCopy bcp = new SqlBulkCopy(targetConnectionString, opn))
            {
                bcp.BulkCopyTimeout = timeOut;
                if (copiedEventHandler != null)
                    bcp.SqlRowsCopied += copiedEventHandler; // 用于进度显示

                bcp.BatchSize = batchSize;
                bcp.NotifyAfter = batchSize;// 设置为1，状态栏提示比较准确，但是速度很慢

                bcp.DestinationTableName = targetTableName;

                // 设置同名列的映射,避免建表语句列顺序不一致导致无法同步的bug
                List<string> arrColNames = GetColNames(sourceData);
                foreach (string colName in arrColNames)
                {
                    bcp.ColumnMappings.Add(colName, colName);
                }
                bcp.WriteToServer(sourceData);
            }
        }

        /// <summary>
        /// 获取DataReader的每列列名
        /// </summary>
        /// <param name="reader"></param>
        /// <returns></returns>
        static List<string> GetColNames(IDataReader reader)
        {
            List<string> ret = new List<string>();
            if (reader == null)// || !reader.HasRows)
                return ret;
            for (int i = 0; i < reader.FieldCount; i++)
            {
                ret.Add(reader.GetName(i));
            }
            return ret;
        }

    }

    /// <summary>
    /// 返回纯真IP实体
    /// </summary>
    public class IPLocation
    {
        /// <summary>
        /// 字符串形式的IP
        /// </summary>
        public string IP { get; set; }
        /// <summary>
        /// 对应国家或地区,如：安徽省安庆市枞阳县；白俄罗斯
        /// </summary>
        public string Country { get; set; }
        /// <summary>
        /// 对应位置，如：电信、联通 或 柞村镇昌盛网吧
        /// </summary>
        public string Area { get; set; }
        /// <summary>
        /// 根据Area，判断所属的运营商
        /// </summary>
        public NetOperator Operator
        {
            get
            {
                string area = Area;
                if (!string.IsNullOrEmpty(area))
                {
                    if (area.Contains("电信"))
                        return NetOperator.Telecom;
                    if (area.Contains("联通"))
                        return NetOperator.Unicom;
                    if (area.Contains("移动"))
                        return NetOperator.CMCC;
                    if (area.Contains("铁通"))
                        return NetOperator.CRC;
                }
                return NetOperator.Unknown;
            }
        }

        /// <summary>
        /// 重载ToString
        /// </summary>
        /// <returns></returns>
        public override string ToString()
        {
            return string.Format("IP:{0}\r\nCountry:{1}\r\nLocal:{2}", IP, Country, Area);
        }
    }

    /// <summary>
    /// 网络运营商
    /// </summary>
    public enum NetOperator
    {
        /// <summary>
        /// 未知
        /// </summary>
        [Description("未知")]
        Unknown = 0,
        /// <summary>
        /// 电信
        /// </summary>
        [Description("中国电信")]
        Telecom = 1,
        /// <summary>
        /// 联通
        /// </summary>
        [Description("中国联通")]
        Unicom = 2,
        /// <summary>
        /// 移动
        /// </summary>
        [Description("中国移动")]
        CMCC = 3,
        /// <summary>
        /// 铁通
        /// </summary>
        [Description("中国铁通")]
        CRC = 4
    }
}
