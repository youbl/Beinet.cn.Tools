using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading;
using System.Windows.Forms;

namespace Beinet.cn.Tools.QQWry
{
    public partial class IP_QQWry : Form
    {
        public IP_QQWry()
        {
            InitializeComponent();
            txtIPFile.Text = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "qqwry.dat");
        }

        private void btnSelect_Click(object sender, EventArgs e)
        {
            var ofd = new OpenFileDialog();
            ofd.Multiselect = false;
            ofd.FileName = txtIPFile.Text;
            ofd.Filter = "dat纯真库文件|*.dat|所有文件|*.*";
            if (ofd.ShowDialog(this) != DialogResult.OK)
            {
                return;
            }
            txtIPFile.Text = ofd.FileName;
        }

        private void btnSearch_Click(object sender, EventArgs e)
        {
            string ipfile = txtIPFile.Text.Trim();
            if (!File.Exists(ipfile))
            {
                MessageBox.Show("未指定IP纯真库文件，或指定的文件不存在");
                return;
            }
            string ip = txtIP.Text.Trim(new char[] { ';', ',', ':', '.' });
            ip = ip.Trim();
            if (string.IsNullOrEmpty(ip))
            {
                MessageBox.Show("请输入IP");
                return;
            }
            long lIP;
            if (long.TryParse(ip, out lIP))
            {
                ip = IPLocator.ConvertLongToIP(lIP);
            }
            if (!Regex.IsMatch(ip, @"^\d{1,3}\.\d{1,3}\.\d{1,3}\.\d{1,3}$"))
            {
                MessageBox.Show("请输入正确格式的IP");
                return;
            }
            try
            {
                IPLocation location = IPLocator.Query(ip, ipfile);
                txtRet.Text = "　　IP: " + location.IP + "\r\n" +
                              "整型IP: " + IPLocator.ConvertIpToLong(location.IP) + "\r\n" +
                              "  地址: " + location.Country + "\r\n" +
                              "  其它: " + location.Area + "\r\n\r\n" + txtRet.Text;
            }
            catch (Exception exp)
            {
                txtRet.Text = "出错了:\r\n" + exp + "\r\n\r\n" + txtRet.Text;
            }
        }

        private void btnImport_Click(object sender, EventArgs e)
        {
            string ipfile = txtIPFile.Text.Trim();
            if (!File.Exists(ipfile))
            {
                MessageBox.Show("未指定IP纯真库文件，或指定的文件不存在");
                return;
            }
            try
            {
                IPLocator.Query("127.0.0.1", ipfile);
            }
            catch (Exception)
            {
                MessageBox.Show("指定的IP纯真库文件格式有误");
                return;
            }
            string constr = txtConstr.Text.Trim();
            if (string.IsNullOrEmpty(constr))
            {
                MessageBox.Show("请输入数据库连接串");
                return;
            }
            btnImport.Enabled = false;
            ThreadPool.UnsafeQueueUserWorkItem(state =>
            {
                try
                {
                    DateTime now = DateTime.Now;
                    string tbname = "ip_" + now.ToString("yyyyMMddHHmmss");
                    IPLocator.ExportToSqlServer(constr, tbname);
                    Utility.InvokeControl(btnImport, () =>
                    {
                        btnImport.Enabled = true;
                        string msg = "导入成功，新建的表名:" + tbname + "耗时(ms):" +
                                    (DateTime.Now - now).TotalMilliseconds.ToString("N0");
                        MessageBox.Show(msg);
                        txtRet.Text = msg + "\r\n\r\n" + txtRet.Text;
                    });
                }
                catch (Exception exp)
                {
                    MessageBox.Show("导入出错:" + exp);
                }
            }, null);
        }

        private void btnLocal_Click(object sender, EventArgs e)
        {
            txtRet.Text = Utility.GetServerIpList() + "\r\n\r\n" + txtRet.Text;
        }
    }
}
