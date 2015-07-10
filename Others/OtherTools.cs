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

namespace Beinet.cn.Tools.Others
{
    public partial class OtherTools : Form
    {
        /// <summary>
        /// 实时记录工作中扫描线程数
        /// </summary>
        private static int _threads;
        /// <summary>
        /// 扫描的最大线程数量
        /// </summary>
        private int _maxThread = 0;
        /// <summary>
        /// 扫描过程中，是否中断进程
        /// </summary>
        private bool _stop = false;

        private const string RUNTXT = "扫描中……";

        public OtherTools()
        {
            InitializeComponent();

            txtResult.Text = GetLocalInfo();

            lstOtherError.SelectedIndex = 1;
        }

        private void btnSearch_Click(object sender, EventArgs e)
        {
            if (labStatus.Text == RUNTXT)
            {
                if (MessageBox.Show("扫描进行中，你要停止扫描吗？", "确认中止？", 
                    MessageBoxButtons.OKCancel, MessageBoxIcon.Warning,
                    MessageBoxDefaultButton.Button2) == DialogResult.OK)
                {
                    _stop = true;
                }
                return;
            }
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

            _threads = 0;
            if (!int.TryParse(txtThreadNum.Text, out _maxThread))
            {
                _maxThread = 5;
                txtThreadNum.Text = _maxThread.ToString();
            }
            //bool showErr = chkShowErrIp.Checked;
            txtResult.Text = GetLocalInfo() + "\r\n扫描结果：\r\n";
            _stop = false;
            labStatus.Text = RUNTXT;
            ThreadPool.UnsafeQueueUserWorkItem(state =>
            {
                do
                {
                    if (_stop)
                        break;
                    Interlocked.Increment(ref _threads);
                    while (_threads > _maxThread)// 最多几个线程
                    {
                        Thread.Sleep(1000);
                    }

                    IPAddress ip = beginIp;
                    ThreadPool.UnsafeQueueUserWorkItem(GetAndShow, ip);
                    beginIp = GetNextIp(beginIp);
                } while (IsBigger(endIp, beginIp));

                // 等待全部线程完成
                while (_threads > 0)
                {
                    Thread.Sleep(1000);
                }
                Utility.InvokeControl(labStatus, () => labStatus.Text = "扫描完成");
            }, null);
        }

        void GetAndShow(object state)
        {
            if (!_stop)
            {
                try
                {
                    IPAddress ip = (IPAddress) state;
                    string mac = GetMac(ip);
                    if (chkShowErrIp.Checked || !mac.StartsWith("err"))
                        Utility.InvokeControl(txtResult, () =>
                            txtResult.Text += ip + " : " + mac + "\r\n");
                }
                catch (Exception exp)
                {
                    Utility.InvokeControl(txtResult, () =>
                        txtResult.Text = exp + "\r\n\r\n" + txtResult.Text);
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
    }
}
