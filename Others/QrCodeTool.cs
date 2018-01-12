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
    public partial class QrCodeTool : Form
    {
        public QrCodeTool()
        {
            InitializeComponent();
            lstOtherError.SelectedIndex = 1;
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

        /// <summary>
        /// 保存二维码
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
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

        /// <summary>
        /// 清除插入图片
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void button4_Click(object sender, EventArgs e)
        {
            if (pictureBox2.Image != null)
                pictureBox2.Image.Dispose();
            pictureBox2.Image = null;
        }
        /// <summary>
        /// 插入图片
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
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


        /// <summary>
        /// 解析二维码图片
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
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
