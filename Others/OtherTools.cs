using System;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Drawing.Imaging;
using System.Net;
using System.Net.Sockets;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading;
using System.Windows.Forms;
using ThoughtWorks.QRCode.Codec;
using ThoughtWorks.QRCode.Codec.Data;

namespace Beinet.cn.Tools.Others
{
    public partial class OtherTools : Form
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

        IPAddress beginIp, endIp;

        private const string RUNTXT = "扫描中……";

        public OtherTools()
        {
            InitializeComponent();

            txtResult.Text = GetLocalInfo();

            lstOtherError.SelectedIndex = 1;

            //int minworkthreads;
            //int miniocpthreads;
            //ThreadPool.GetMinThreads(out minworkthreads, out miniocpthreads);
            ThreadPool.SetMinThreads(50, 50);
        }


        private bool Init(Button btn)
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

            string strBeginIp = txtIp1.Text.Trim();
            if (!IPAddress.TryParse(strBeginIp, out beginIp))
            {
                txtResult.Text = "起始IP格式不正确:" + strBeginIp;
                return false;
            }

            string strEndIp = txtIp2.Text.Trim();
            if (strEndIp == string.Empty)
                endIp = beginIp;
            else if (btn == btnSearch && !IPAddress.TryParse(strEndIp, out endIp))
            {
                txtResult.Text = "结束IP格式不正确:" + strEndIp;
                return false;
            }

            _threads = 0;

            txtResult.Text = GetLocalInfo() + "\r\n扫描结果：\r\n";
            txtErr.Text = "";
            _stop = false;
            labStatus.Text = RUNTXT + DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss.fff");
            _sb.Clear();
            return true;
        }


        private void btnSearch_Click(object sender, EventArgs e)
        {
            if (!Init(sender as Button))
            {
                return;
            }

            ThreadPool.UnsafeQueueUserWorkItem(state =>
            {
                do
                {
                    if (_stop)
                        break;
                    Interlocked.Increment(ref _threads);
                    IPAddress ip = beginIp;
                    ThreadPool.UnsafeQueueUserWorkItem(GetAndShow, ip);
                    beginIp = GetNextIp(beginIp);
                } while (IsBigger(endIp, beginIp));

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
                IPAddress ip = (IPAddress)state;
                // 获取mac地址
                if (chkMac.Checked)
                {
                    try
                    {
                        string mac = GetMac(ip);
                        if (!mac.StartsWith("err"))
                        {
                            var msg1 = txtResult.Text + ip + " : " + mac + "\r\n";
                            Utility.InvokeControl(txtResult, () => txtResult.Text = msg1);
                        }
                        else
                        {
                            AddErrMsg(ip + " : " + mac + "\r\n");
                        }
                    }
                    catch (Exception exp)
                    {
                        var msg1 = exp + "\r\n\r\n";
                        AddErrMsg(msg1);
                    }
                }
                if (chkNormalPort.Checked)
                {
                    DoPortScan(ip, true);
                }
                else if (chkPortAll.Checked)
                {
                    DoPortScan(ip, false);
                }
            }
            Interlocked.Decrement(ref _threads);
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

        static string GetLocalInfo()
        {
            StringBuilder ips = new StringBuilder("本机信息\r\n");
            ips.AppendFormat("已开机: {0}\r\n", GetTime(Environment.TickCount));
            ips.AppendFormat("系  统: {0}{1}\r\n", Environment.OSVersion, Environment.Is64BitOperatingSystem ? " 64位系统" : "");
            ips.AppendFormat("机器名: {0}\r\n", Environment.MachineName);
            ips.AppendFormat("登录人: {0}\r\n", Environment.UserName);
            ips.AppendFormat("所属域: {0}\r\n", Environment.UserDomainName);
            ips.AppendFormat("CPU数 : {0}\r\n", Environment.ProcessorCount.ToString());
            try
            {
                IPHostEntry IpEntry = Dns.GetHostEntry(Dns.GetHostName());
                foreach (IPAddress ipa in IpEntry.AddressList)
                {
                    if (ipa.AddressFamily == AddressFamily.InterNetwork)
                    {
                        string mac = GetMac(ipa);
                        ips.AppendFormat("MAC   : {1}  对应IP: {0}\r\n", ipa, mac);
                    }
                }
                return ips.ToString();
            }
            catch (Exception exp)
            {
                //LogHelper.WriteCustom("获取本地ip错误" + ex, @"zIP\", false);
                return ips + "\r\n" + exp + "\r\n";
            }
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

        static string GetTime(int totalMs)
        {
            //int ms = totalMs % 1000;
            int totalSecond = totalMs / 1000;
            int second = totalSecond % 60;
            string ret = second.ToString() + "秒";
            int totalMin = totalSecond / 60;
            if (totalMin <= 0)
                return ret;

            int min = totalMin % 60;
            ret = min.ToString() + "分" + ret;
            int totalHour = totalMin / 60;
            if (totalHour <= 0)
                return ret;

            int hour = totalHour % 24;
            ret = hour.ToString() + "小时" + ret;
            int totalDay = totalHour / 24;
            if (totalDay <= 0)
                return ret;

            return totalDay.ToString() + "天" + ret;
        }



        /// <summary>
        /// 生成二维码
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void button1_Click(object sender, EventArgs e)
        {
            Image img = BuildQrCode();
            if (img == null)
                return;
            if (pictureBox1.Image != null)
                pictureBox1.Image.Dispose();
            pictureBox1.Image = img;
        }

        private void button2_Click(object sender, EventArgs e)
        {
            using (Image img = BuildQrCode())
            {
                if (img == null)
                    return;
                var dailog = new SaveFileDialog();
                DialogResult result = dailog.ShowDialog(this);
                if (result == DialogResult.OK)
                {
                    img.Save(dailog.FileName, ImageFormat.Jpeg);
                }
            }
        }


        private void button4_Click(object sender, EventArgs e)
        {
            if (pictureBox2.Image != null)
                pictureBox2.Image.Dispose();
            pictureBox2.Image = null;
        }

        private void button3_Click(object sender, EventArgs e)
        {
            var dailog = new OpenFileDialog();
            dailog.Filter = "Image Files(*.BMP;*.JPG;*.GIF;*.PNG)|*.BMP;*.JPG;*.GIF;*.PNG|All files (*.*)|*.*";
            if (dailog.ShowDialog() != DialogResult.OK)
                return;
            try
            {
                pictureBox2.Image = Image.FromFile(dailog.FileName);
            }
            catch(Exception exp)
            {
                MessageBox.Show("加载文件出错,可能你选的不是图片:" + exp.Message);
            }
        }



        #region 非事件的方法集
        /// <summary>
        /// 根据配置，生成二维码返回
        /// </summary>
        /// <returns></returns>
        Image BuildQrCode()
        {
            string str = txtOtherQr.Text.Trim();
            if (str == string.Empty)
            {
                MessageBox.Show("请输入文字");
                return null;
            }
            int size;
            if (!int.TryParse(txtOtherQrSize.Text.Trim(), out size) || size <= 0)
            {
                size = 4;
            }

            QRCodeEncoder qrCodeEncoder = new QRCodeEncoder();
            // byte支持中文，另外2种枚举支持的是数字
            qrCodeEncoder.QRCodeEncodeMode = QRCodeEncoder.ENCODE_MODE.BYTE;
            // 大小(值越大生成的二维码图片像素越高)
            qrCodeEncoder.QRCodeScale = size;
            //版本(注意：设置为0主要是防止编码的字符串太长时发生错误);
            qrCodeEncoder.QRCodeVersion = 8;
            // 错误检验和更正级别
            qrCodeEncoder.QRCodeErrorCorrect = (QRCodeEncoder.ERROR_CORRECTION)lstOtherError.SelectedIndex;
            Image img;
            try
            {
                img = qrCodeEncoder.Encode(str);
            }
            catch (Exception exp)
            {
                MessageBox.Show("生成出错，请确认字数是否超限:\r\n" + exp.Message);
                return null;
            }
            if (pictureBox2.Image != null)
            {
                img = CombinImage(img, pictureBox2.Image);
            }
            return img;
        }


        /// <summary>  
        /// 调用此函数后使此两种图片合并，类似相册，有个  
        /// 背景图，中间贴自己的目标图片  
        /// </summary>  
        /// <param name="imgBack">粘贴的源图片</param>  
        /// <param name="destImg">粘贴的目标图片</param>  
        static Image CombinImage(Image imgBack, Image destImg)
        {
            int width = (int)Math.Floor(imgBack.Width / 5d);
            int height = (int)Math.Floor(imgBack.Height / 5d);
            //Image destImg = Image.FromFile(destImg);        //照片图片    
            if (destImg.Height != width || destImg.Width != height)
            {
                destImg = KiResizeImage(destImg, width, height);
            }
            using (Graphics g = Graphics.FromImage(imgBack))
            {
                g.DrawImage(imgBack, 0, 0, imgBack.Width, imgBack.Height); //g.DrawImage(imgBack, 0, 0, 相框宽, 相框高); 
                //g.FillRectangle(System.Drawing.Brushes.White, imgBack.Width / 2 - img.Width / 2 - 1, imgBack.Width / 2 - img.Width / 2 - 1,1,1);//相片四周刷一层黑色边框  
                //g.DrawImage(img, 照片与相框的左边距, 照片与相框的上边距, 照片宽, 照片高);  
                g.DrawImage(destImg, imgBack.Width/2 - destImg.Width/2, imgBack.Width/2 - destImg.Width/2,
                    destImg.Width, destImg.Height);
            }
            //GC.Collect();
            return imgBack;
        }


        /// <summary>  
        /// Resize图片  
        /// </summary>  
        /// <param name="bmp">原始Bitmap</param>  
        /// <param name="newW">新的宽度</param>  
        /// <param name="newH">新的高度</param>
        /// <returns>处理以后的图片</returns>  
        static Image KiResizeImage(Image bmp, int newW, int newH)
        {
            try
            {
                Image b = new Bitmap(newW, newH);
                using (Graphics g = Graphics.FromImage(b))
                {
                    // 插值算法的质量  
                    g.InterpolationMode = InterpolationMode.HighQualityBicubic;
                    g.DrawImage(bmp, new Rectangle(0, 0, newW, newH), 
                        new Rectangle(0, 0, bmp.Width, bmp.Height),
                        GraphicsUnit.Pixel);
                }
                return b;
            }
            catch
            {
                return null;
            }
        }
        #endregion

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

        private void DoPortScan(IPAddress ip, bool normal)
        {
            if (_stop) return;
            if (normal)
            {
                int[] arrPort = new[] { 80, 8080, 3128, 8081, 9080, 1080, 21, 23, 443, 69, 22, 25, 110, 7001, 9090, 3389, 1521, 1158, 2100, 1433, 3306 };
                foreach (var port in arrPort)
                {
                    PerPortCheck(ip, port);
                }
            }
            else
            {
                for (var port = 1; port < 65535; port++)
                {
                    PerPortCheck(ip, port);
                }
            }
        }

        private void PerPortCheck(IPAddress ip, int port)
        {
            if (_stop)
                return;
            try
            {
                IPEndPoint serverInfo = new IPEndPoint(ip, port);
                using (Socket socket = new Socket(ip.AddressFamily, SocketType.Stream, ProtocolType.Tcp))
                {
                    //socket.BeginConnect(serverInfo, CallBackMethod, socket);
                    socket.Connect(serverInfo);
                    if (socket.Connected)
                    {
                        // ok
                        var txt = txtResult.Text + ip + " : " + port + " 开放 \r\n";
                        Utility.InvokeControl(txtResult, () => txtResult.Text = txt);
                    }
                    else
                    {
                        // fail
                        var txt = ip + " : " + port + " 未开放 \r\n";
                        AddErrMsg(txt);
                    }
                    socket.Close();
                }
            }
            catch (Exception exp)
            {
                // err
                var txt = ip + " : " + port + " 未开放:" + exp.Message + " \r\n";
                AddErrMsg(txt);
            }
        }

        private void btnPort_Click(object sender, EventArgs e)
        {
            if (!Init(sender as Button))
            {
                return;
            }

            ThreadPool.UnsafeQueueUserWorkItem(state =>
            {
                if (chkPortAll.Checked)
                {
                    for (var port = 1; port < 65535; port++)
                    {
                        if (_stop)
                            break;
                        var prot1 = port;
                        ThreadPool.UnsafeQueueUserWorkItem(aaa =>
                        {
                            PerPortCheck(beginIp, prot1);
                            Interlocked.Decrement(ref _threads);
                        }, null);
                        Interlocked.Increment(ref _threads);
                    }
                }
                else
                {
                    int[] arrPort = new[]
                    {
                        80, 8080, 3128, 8081, 9080, 1080, 21, 23, 443, 69, 22, 25, 110, 7001, 9090, 3389, 1521, 1158,
                        2100, 1433, 3306
                    };
                    foreach (var port in arrPort)
                    {
                        if (_stop)
                            break;
                        var prot1 = port;
                        ThreadPool.UnsafeQueueUserWorkItem(aaa =>
                        {
                            PerPortCheck(beginIp, prot1);
                            Interlocked.Decrement(ref _threads);
                        }, null);
                        Interlocked.Increment(ref _threads);
                    }
                }
                EndCheck();
            }, null);
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

        private void btnRead_Click(object sender, EventArgs e)
        {
            OpenFileDialog ofd = new OpenFileDialog();
            ofd.InitialDirectory = "D:\\";
            ofd.Filter = "Img files (*.jpg;*.png;*.gif;*.bmp)|*.jpg;*.png;*.gif;*.bmp|All files (*.*)|*.*";

            var dialogRet = ofd.ShowDialog(this);
            if (dialogRet != DialogResult.OK)
                return;
            var file = ofd.FileName;
            try
            {
                string ret;
                using (Bitmap bmp = new Bitmap(file))
                {
                    QRCodeDecoder decoder = new QRCodeDecoder();
                    ret = decoder.decode(new QRCodeBitmapImage(bmp));
                }
                txtOtherQr.Text = ret;

                if (pictureBox1.Image != null)
                    pictureBox1.Image.Dispose();
                pictureBox1.Image = Image.FromFile(file);
            }
            catch (Exception exp)
            {
                MessageBox.Show(file + "解析出错：" + exp.Message);
            }
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
