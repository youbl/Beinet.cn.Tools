using System;
using System.Net;
using System.Runtime.InteropServices;
using System.Threading;
using System.Windows.Forms;

namespace Beinet.cn.Tools.Others
{
    public partial class OtherTools : Form
    {
        public OtherTools()
        {
            InitializeComponent();
        }

        private void btnSearch_Click(object sender, EventArgs e)
        {
            IPAddress beginIp, endIp;

            string strBeginIp = txtIp1.Text.Trim();
            if (!IPAddress.TryParse(strBeginIp, out beginIp))
            {
                txtResult.Text = "起始IP格式不正确:" + strBeginIp;
                return;
            }
            
            string strEndIp = txtIp2.Text.Trim();
            if (strEndIp == string.Empty)
                endIp = beginIp;
            else if (!IPAddress.TryParse(strEndIp, out endIp))
            {
                txtResult.Text = "结束IP格式不正确:" + strEndIp;
                return;
            }
            txtResult.Text = "";
            bool showErr = chkShowErrIp.Checked;
            ThreadPool.UnsafeQueueUserWorkItem(state =>
            {
                try
                {
                    do
                    {
                        IPAddress ip = beginIp;
                        string mac = GetMac(ip);
                        if (showErr || !mac.StartsWith("err"))
                            Utility.InvokeControl(txtResult, () =>
                                txtResult.Text += ip + " : " + mac + "\r\n");
                        beginIp = GetNextIp(beginIp);

                    } while (IsBigger(endIp, beginIp));
                }
                catch (Exception exp)
                {
                    Utility.InvokeControl(txtResult, () => 
                        txtResult.Text = exp + "\r\n\r\n" + txtResult.Text);
                }

            }, null);
        }

        // GetAddressBytes得到的结果，如：{192, 168, 254, 58}
        static bool IsBigger(IPAddress ip1, IPAddress ip2)
        {
            //uint intIp1 = BitConverter.ToUInt32(ip1.GetAddressBytes(), 0);
            //uint intIp2 = BitConverter.ToUInt32(ip2.GetAddressBytes(), 0);
            //return intIp1 >= intIp2;
            byte[] arrIp1 = ip1.GetAddressBytes();
            byte[] arrIp2 = ip2.GetAddressBytes();
            if (arrIp1[0] != arrIp2[0])
                return arrIp1[0] > arrIp2[0];
            if (arrIp1[1] != arrIp2[1])
                return arrIp1[1] > arrIp2[1];
            if (arrIp1[2] != arrIp2[2])
                return arrIp1[2] > arrIp2[2];
            return arrIp1[3] >= arrIp2[3];
        }

        static IPAddress GetNextIp(IPAddress ip)
        {
            byte[] arrIp = ip.GetAddressBytes();
            arrIp[3]++;
            if (arrIp[3] == 255 || arrIp[3] == 0)
            {
                arrIp[3] = 1;
                arrIp[2]++;
                if (arrIp[2] == 0)
                {
                    arrIp[2] = 1;
                    arrIp[1]++;
                    if (arrIp[1] == 0)
                    {
                        arrIp[1] = 1;
                        arrIp[0]++;
                    }
                }
            }

            return new IPAddress(arrIp);
        }

        static string GetMac(IPAddress ip)
        {
            uint intIp = BitConverter.ToUInt32(ip.GetAddressBytes(), 0);

            ulong pMacAddr = 0;
            uint addrLen = 6;
            uint error_code = SendARP(intIp, 0, ref pMacAddr, ref addrLen);
            if (error_code != 0)
                return "err code:" + error_code.ToString();
            byte[] arr = BitConverter.GetBytes(pMacAddr);
            return BitConverter.ToString(arr, 0, 6);
        }

        /// <summary>
        /// 使用ARP获取MAC地址
        /// </summary>
        /// <param name="DestIP">目标IP</param>
        /// <param name="SrcIP">0</param>
        /// <param name="pMacAddr">两个uint 都是255</param>
        /// <param name="PhyAddrLen">长度6</param>
        /// <returns>返回错误信息</returns>
        [DllImport("Iphlpapi.dll")]
        static extern uint SendARP(uint DestIP, uint SrcIP, ref ulong pMacAddr, ref uint PhyAddrLen);
    }
}
