using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Threading;
using System.Windows.Forms;

namespace Beinet.cn.Tools.Others
{
    public partial class ScanPort : Form
    {
        /// <summary>
        /// 实时记录工作中扫描线程数
        /// </summary>
        private static int _threads;
        /// <summary>
        /// 扫描过程中，是否中断进程
        /// </summary>
        private bool _stop = false;
        private StringBuilder _sb = new StringBuilder();

        private const string RUNTXT = "扫描中……";

        private IPAddress[] _ips;

        public ScanPort()
        {
            InitializeComponent();

            //int minworkthreads;
            //int miniocpthreads;
            //ThreadPool.GetMinThreads(out minworkthreads, out miniocpthreads);
            ThreadPool.SetMinThreads(50, 50);
        }


        private bool Init()
        {
            if (labStatus.Text.StartsWith(RUNTXT, StringComparison.Ordinal))
            {
                if (MessageBox.Show("扫描进行中，你要停止扫描吗？", "确认中止？\r\n已启动的部分线程需要一点时间结束",
                        MessageBoxButtons.OKCancel, MessageBoxIcon.Warning,
                        MessageBoxDefaultButton.Button2) == DialogResult.OK)
                {
                    _stop = true;
                }
                return false;
            }

            string[] strBeginIp = txtIp1.Text.Split(new char[]{',',' ',';'}, StringSplitOptions.RemoveEmptyEntries);
            var ips = new List<IPAddress>();
            IPAddress tmpIp;
            foreach (var item in strBeginIp)
            {
                var idx = item.IndexOf('-');
                if (idx > 0 && idx < item.Length)
                {
                    IPAddress sIp, eIp;
                    var startIp = item.Substring(0, idx);
                    if (!IPAddress.TryParse(startIp, out sIp))
                    {
                        txtResult.Text = "起始IP格式不正确:" + item;
                        return false;
                    }
                    var endIp = item.Substring(idx + 1);
                    if (!IPAddress.TryParse(endIp, out eIp))
                    {
                        txtResult.Text = "结束IP格式不正确:" + item;
                        return false;
                    }

                    AddIpRange(sIp, eIp, ips);
                    continue;
                }
                if (!IPAddress.TryParse(item, out tmpIp))
                {
                    txtResult.Text = "存在IP格式不正确:" + item;
                    return false;
                }
                ips.Add(tmpIp);
            }

            _ips = ips.ToArray();
            _threads = 0;

            txtResult.Text = "扫描结果：\r\n";
            txtErr.Text = "";
            _stop = false;
            labStatus.Text = RUNTXT + DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss.fff");
            _sb.Clear();
            return true;
        }


        private void btnSearch_Click(object sender, EventArgs e)
        {
            if (!Init())
            {
                return;
            }

            ThreadPool.UnsafeQueueUserWorkItem(state =>
            {
                foreach (var ip in _ips)
                {
                    if (_stop)
                        break;
                    GetAndShow(ip);
                }

                EndCheck();
            }, null);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="state"></param>
        void GetAndShow(object state)
        {
            if (!_stop)
            {
                int seconds;
                if (!int.TryParse(txtTimeout.Text, out seconds))
                {
                    seconds = 4;
                }

                IPAddress ip = (IPAddress)state;
                if (chkNormalPort.Checked)
                {
                    var arrPort = new HashSet<int>();
                    foreach (var item in txtCustomPort.Text.Split(',', ' '))
                    {
                        if(item.Length > 0 && int.TryParse(item, out var port))
                            arrPort.Add(port);
                    }
                    if (arrPort.Count <= 0)
                    {
                        MessageBox.Show("请正确输入要扫描的端口");
                        return;
                    }
                    DoPortScan(ip, arrPort.ToArray(), seconds);
                }
                else if (chkPortAll.Checked)
                {
                    DoPortScan(ip, null, seconds);
                }
            }
        }

        static void AddIpRange(IPAddress beginIp, IPAddress endIp, List<IPAddress> ips)
        {
            do
            {
                IPAddress ip = beginIp;
                ips.Add(ip);
                beginIp = GetNextIp(beginIp);
            } while (IsBigger(endIp, beginIp));
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
        
       

       private void chkPortAll_CheckedChanged(object sender, EventArgs e)
        {
            var chkbox = (CheckBox) sender;
            if (chkbox.Checked)
            {
                if (chkbox == chkNormalPort)
                    chkPortAll.Checked = false;
                else if (chkbox == chkPortAll)
                    chkNormalPort.Checked = false;
            }
        }

        private void DoPortScan(IPAddress ip, IEnumerable<int> arrPort, int seconds)
        {
            void StartOnePort(int port)
            {
                Interlocked.Increment(ref _threads);
                ThreadPool.UnsafeQueueUserWorkItem(state =>
                {
                    PerPortCheck(ip, port, seconds);
                }, null);
                while (_threads > 300)
                {
                    Thread.Sleep(5000);
                }
            }
            if (_stop) return;
            if (arrPort != null)
            {
                //int[] arrPort = new[] { 80, 8080, 3128, 8081, 9080, 1080, 21, 23, 443, 69, 22, 25, 110, 7001, 9090, 3389, 1521, 1158, 2100, 1433, 3306 };
                foreach (var port in arrPort)
                {
                    StartOnePort(port);
                }
            }
            else
            {
                for (var port = 1; port < 65535; port++)
                {
                    StartOnePort(port);
                }
            }
        }

        private void PerPortCheck(IPAddress ip, int port, int seconds)
        {
            if (_stop)
            {
                Interlocked.Decrement(ref _threads);
                return;
            }
            var begin = DateTime.Now;
            string txt = "";
            bool isok = false;
            try
            {
                IPEndPoint serverInfo = new IPEndPoint(ip, port);
                using (Socket socket = new Socket(ip.AddressFamily, SocketType.Stream, ProtocolType.Tcp))
                {

                    // 单位毫秒
                    // socket.SendTimeout = 2000;
                    // socket.ReceiveTimeout = 2000;
                    //socket.Connect(serverInfo);

                    IAsyncResult connResult = socket.BeginConnect(serverInfo, null, null);
                    connResult.AsyncWaitHandle.WaitOne(1000 * seconds, true); //等待4秒

                    if (connResult.IsCompleted && socket.Connected)
                    {
                        // ok
                        isok = true;
                        var tmptxt = txtResult.Text +
                                     "(" + (DateTime.Now - begin).TotalSeconds.ToString("N2") + "秒)" + ip + " : " + port + " 开放 \r\n";
                        Utility.InvokeControl(txtResult, () => txtResult.Text = tmptxt);
                    }
                    else
                    {
                        // fail
                        txt = ip + " : " + port + " 未开放 \r\n";
                    }
                    socket.Close();
                }
            }
            catch (Exception exp)
            {
                // err
                txt = ip + " : " + port + " 未开放:" + exp.Message + " \r\n";
            }
            finally
            {
                Interlocked.Decrement(ref _threads);
            }
            if (!isok)
            {
                txt = "(" + (DateTime.Now - begin).TotalSeconds.ToString("N2") + "秒)" + txt;
                AddErrMsg(txt);
            }
        }

        private void EndCheck()
        {
            // 等待全部线程完成
            while (_threads > 0)
            {
                Thread.Sleep(2000);
                var maxlen = txtErr.MaxLength;
                var start = maxlen / 2;
                string errmsg;
                lock (_sb)
                {
                    var len = _sb.Length;
                    if (len > maxlen)
                    {
                        _sb.Remove(start, len - start);
                    }
                    errmsg = _sb.ToString();
                }


                ThreadPool.GetAvailableThreads(out var minworkthreads, out _);
                var msg = $"{_threads.ToString()}/{minworkthreads.ToString()},";
                ThreadPool.GetMaxThreads(out minworkthreads, out _);
                msg += $"{minworkthreads.ToString()}";
                Utility.InvokeControl(labTh, () =>
                {
                    labTh.Text = msg;
                    txtErr.Text = errmsg;
                });
            }
            Utility.InvokeControl(labStatus, () =>
            {
                labStatus.Text = "扫描完成" + DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss.fff") + ", 启动于:" + labStatus.Text.Replace(RUNTXT, "");
                lock (_sb)
                {
                    txtErr.Text = _sb.ToString();
                }
            });
        }
        
        private void AddErrMsg(string msg)
        {
            // StringBuilder线程不安全，简单处理吧
            lock (_sb)
            {
                _sb.Insert(0, msg);
            }
        }
    }
}
