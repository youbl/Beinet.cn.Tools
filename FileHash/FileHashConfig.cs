using System;
using System.Windows.Forms;

namespace Beinet.cn.Tools.FileHash
{
    public partial class FileHashConfig : Form
    {
        public FileHashConfig()
        {
            InitializeComponent();
            ShowInTaskbar = false;// 不能放到OnLoad里，会导致窗体消失
        }
        protected override void OnLoad(EventArgs e)
        {
            base.OnLoad(e);

            if (Owner != null)
            {
                Left = Owner.Left + 20;
                Top = Owner.Top + Owner.Height - Height - 20;
                if (Top < 0)
                    Top = 0;
            }

            textBox1.Text = @"<?xml version=""1.0"" encoding=""utf-8"" ?>
<HashSetting>
  <Service>
    <HashUrl><![CDATA[http://a.com/CheckIpInfo.aspx?md5=1]]></HashUrl>
    <PostData><![CDATA[d=E:\WwwRoot\a.com\WebRoot\bin]]></PostData>
    <LocalPath><![CDATA[e:\发布\a.com\bin]]></LocalPath>
    <Host>10.1.240.188</Host>
    <Host>10.1.240.199</Host>
  </Service>
</HashSetting>";
        }
    }
}
