using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Common;
using System.Data.SqlClient;
using System.Text;
using System.Threading;
using System.Windows.Forms;

// ReSharper disable once CheckNamespace
namespace Beinet.cn.Tools
{
    public static class SqlHelper
    {
        private static SqlConnection CreateConn(string connstr)
        {
            // 在连接串里设置Connection Timeout=5不生效，仅调试状态有效，很奇怪
            //if (connstr.IndexOf("Connection Timeout", StringComparison.OrdinalIgnoreCase) < 0)
            //{
            //    connstr += ";Connection Timeout=5";// + timeout.ToString();
            //}
            //MessageBox.Show(connstr);

            // 以线程阻塞方式打开连接，并等待5秒，避免无效IP导致要等待30秒
            Exception exp = null;
            var ret = new SqlConnection(connstr);
            var th = new Thread(() =>
            {
                try
                {
                    ret.Open();
                }
                catch (Exception ex)
                {
                    exp = ex;
                    ret = null;
                }
            });
            th.Start();
            th.Join(5000);
            if (th.IsAlive)
            {
                ThreadPool.UnsafeQueueUserWorkItem(state =>
                {
                    try
                    {
                        th.Abort();
                    }
                    catch
                    {
                        // ignored
                    }
                }, th);
                throw exp ?? new Exception("连接超过5秒未成功"); 
            }
            return ret;
        }
        public static DataSet ExecuteDataSet(string connstr, string sql, int timeout = 10, params SqlParameter[] parameters)
        {
            using (var conn = CreateConn(connstr))
            using (var command = conn.CreateCommand())
            using (var da = new SqlDataAdapter(command))
            {
                command.CommandText = sql;
                command.CommandTimeout = timeout;
                if (parameters != null)
                    command.Parameters.AddRange(parameters);
                DataSet ds = new DataSet();
                da.Fill(ds);
                return ds;
            }
        }

        public static SqlDataReader ExecuteReader(string connstr, string sql, int timeout = 10, params SqlParameter[] parameters)
        {
            return ExecuteReader(connstr, sql, out _, timeout, parameters);
        }

        // 增加这个方法的用处：是为了便于提早关闭DataReader，参考SqlDataReader.Close方法的说明：
        /*
         * Close 方法填写输出参数的值、返回值和 RecordsAffected，从而增加了关闭用于处理大型或复杂查询的 SqlDataReader 所用的时间。 
         * 如果返回值和查询影响的记录的数量不重要，则可以在调用 Close 方法前调用关联的 SqlCommand 对象的 Cancel 方法，
         * 从而减少关闭 SqlDataReader 所需的时间。 
         */
        public static SqlDataReader ExecuteReader(string connstr, string sql, out SqlCommand command, int timeout = 10, params SqlParameter[] parameters)
        {
            var conn = CreateConn(connstr);
            command = conn.CreateCommand();
            command.CommandText = sql;
            command.CommandTimeout = timeout;
            if (parameters != null)
                command.Parameters.AddRange(parameters);
            if (conn.State != ConnectionState.Open)
                conn.Open();
            return command.ExecuteReader(CommandBehavior.CloseConnection);
        }

        public static object ExecuteScalar(string connstr, string sql, params SqlParameter[] parameters)
        {
            using (var conn = CreateConn(connstr))
            using (var command = conn.CreateCommand())
            {
                command.CommandText = sql;
                if (parameters != null)
                    command.Parameters.AddRange(parameters);
                if (conn.State != ConnectionState.Open)
                    conn.Open();
                return command.ExecuteScalar();
            }
        }

        public static int ExecuteNonQuery(string connstr, string sql, params SqlParameter[] parameters)
        {
            using (var conn = CreateConn(connstr))
            using (var command = conn.CreateCommand())
            {
                command.CommandText = sql;
                if (parameters != null)
                    command.Parameters.AddRange(parameters);
                if (conn.State != ConnectionState.Open)
                    conn.Open();
                return command.ExecuteNonQuery();
            }
        }

        public static List<string> GetColNames(DbDataReader reader)
        {
            List<string> ret = new List<string>();
            if (reader == null || !reader.HasRows)
                return ret;
            for (int i = 0; i < reader.FieldCount; i++)
            {
                ret.Add(reader.GetName(i));
            }
            return ret;
        }

        public static string GetCreateSql(SqlDataReader reader, string tbName)
        {
            if (reader == null)
                return null;
            StringBuilder sb = new StringBuilder("Create table [" + tbName + "](");
            DataTable dt = reader.GetSchemaTable();
            if (dt == null || dt.Rows.Count <= 0)
                return null;
            foreach (DataRow row in dt.Rows)
            {
                string type = row["DataTypeName"].ToString().ToLower();
                sb.Append("[" + row["ColumnName"] + "] " + type);
                switch (type)
                {
                    case "bigint":
                    case "bit":
                    case "datetime":
                    case "float":
                    case "image":
                    case "int":
                    case "money":
                    case "ntext":
                    case "real":
                    case "smalldatetime":
                    case "smallint":
                    case "smallmoney":
                    case "sql_variant":
                    case "text":
                    case "timestamp":
                    case "tinyint":
                    case "uniqueidentifier":
                    case "xml":
                        break;
                    case "decimal":
                    case "numeric":
                        sb.Append("(" + row["NumericPrecision"] + ", " + row["NumericScale"] + ")");
                        break;
                    case "binary":
                    case "char":
                    case "nchar":
                    case "nvarchar":
                    case "varbinary":
                    case "varchar":
                        if ((int)row["ColumnSize"] == int.MaxValue)
                            sb.Append("(max)");
                        else
                            sb.Append("(" + row["ColumnSize"] + ")");
                        break;
                }
                sb.Append(",");
            }
            sb.Remove(sb.Length - 1, 1);// 移除最后一个逗号
            sb.Append(")");
            return sb.ToString();
        }

        public static bool ExistsTable(string connstr, string tableName)
        {
            string sql = "select count(1) from sys.objects where type='U' and name = @tbName";
            SqlParameter para = new SqlParameter("@tbName", SqlDbType.VarChar, 100) { Value = tableName };
            return (int)ExecuteScalar(connstr, sql, para) > 0;
        }

        public static void ClearTable(string connstr, string tableName, bool useTruncate = true)
        {
            string sql = (useTruncate ? "Truncate Table " : "Delete From ") + tableName;
            ExecuteNonQuery(connstr, sql);
        }

        public static long GetTableRows(string connstr, string tableName)
        {
            string sql =
                "SELECT a.rowcnt FROM sysindexes a, sys.tables b WHERE a.id = b.[object_id] AND a.indid <=1 AND b.[name] = @tbName";
            SqlParameter para = new SqlParameter("@tbName", SqlDbType.VarChar, 100) { Value = tableName };
            object obj = ExecuteScalar(connstr, sql, para);
            if (obj == null || obj == DBNull.Value)
                return -1;
            return Convert.ToInt64(obj);
        }


        public static void BulkCopy(DataTable data,
            string targetConnectionString, string targetTableName,
            int timeOut = 30, bool keepIdentity = true, int batchSize = 2000,
            SqlRowsCopiedEventHandler copiedEventHandler = null)
        {
            if (data == null || data.Rows.Count <= 0)
            {
                return;
            }
            DataTableReader sourceData = data.CreateDataReader();
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
    }
}
