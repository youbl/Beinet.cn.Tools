using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows.Forms;

namespace Beinet.cn.Tools.LvsManager
{
    public partial class ConfigSet : Form
    {
        private List<Site> arrServer { get; set; }
        public ConfigSet()
        {
            InitializeComponent();
            ShowInTaskbar = false;// 不能放到OnLoad里，会导致窗体消失

            arrServer = Common.Sites;
            foreach (Site server in arrServer)
            {
                lstServers.Items.Add(server.name);
            }
            if (lstServers.Items.Count > 0)
                lstServers.SelectedIndex = 0;
        }


        private void ConfigSet_Load(object sender, EventArgs e)
        {
            if(Owner != null)
            {
                Left = Owner.Left + 20;
                Top = Owner.Top + 20;
                if (Top < 0)
                    Top = 0;
            }
        }


        private void btnSave_Click(object sender, EventArgs e)
        {
            int second;
            if (!int.TryParse(txtRefreshSecond.Text, out second))
            {
                MessageBox.Show("输入的刷新时间不正确");
                return;
            }
            int offlinesecond;
            if (!int.TryParse(txtOfflineSecond.Text, out offlinesecond))
            {
                MessageBox.Show("输入的离线时间不正确");
                return;
            }
            string url = txtUrl.Text;
            List<string> servers = new List<string>();
            foreach (string ip in txtIP.Text.Split(new char[] { ' ', ',', ';' }, StringSplitOptions.RemoveEmptyEntries))
            {
                long longip = Utility.ConvertIp(ip);
                if (longip == -1)
                {
                    MessageBox.Show("输入的ip不正确" + ip);
                    return;
                }
                servers.Add(Utility.ConvertIp(longip));
            }
            if (!servers.Any())
            {
                MessageBox.Show("没有输入ip");
                return;
            }

            string domain = lstServers.Text;
            string host = Common.GetHost(url);
            if (!host.Equals(domain, StringComparison.OrdinalIgnoreCase))
            {
                if (MessageBox.Show("url里的域名与您输入的域名不一致，确认要保存吗？", "",
                    MessageBoxButtons.YesNo, MessageBoxIcon.Warning, MessageBoxDefaultButton.Button2) != DialogResult.Yes)
                    return;
            }

            Site server = FindItem(domain);
            bool isadd = false;
            if (server == null)
            {
                string name = domain;
                server = new Site { name = name };
                arrServer.Add(server);
                isadd = true;
            }
            server.url = url;
            server.resreshSecond = second;
            server.offlineSecond = offlinesecond;
            server.Servers = servers;
            Common.SaveServers(arrServer);// 保存到文件

            if (isadd)
                lstServers.Items.Add(server.name);

            MessageBox.Show("保存成功");
        }

        private void btnDel_Click(object sender, EventArgs e)
        {
            string domain = lstServers.Text;
            Site server = FindItem(domain);
            if (server != null)
            {
                arrServer.Remove(server);
                Common.SaveServers(arrServer);// 保存到文件

                lstServers.Items.Remove(domain);
                if (lstServers.Items.Count > 0)
                {
                    lstServers.SelectedIndex = 0;
                    MessageBox.Show(domain + "删除成功");
                }
                else
                {
                    lstServers.Text = string.Empty;
                    MessageBox.Show(domain + "删除成功，当前没有任何站点");
                }
            }
            else
            {
                MessageBox.Show("指定的记录不存在");
            }
        }

        private void lstServers_SelectedIndexChanged(object sender, EventArgs e)
        {
            Site server = FindItem(lstServers.Text);
            if (server != null)
            {
                txtUrl.Text = server.url;
                txtRefreshSecond.Text = server.resreshSecond.ToString();
                txtOfflineSecond.Text = server.offlineSecond.ToString();
                txtIP.Text = string.Empty;
                foreach (string ip in server.Servers)
                {
                    txtIP.Text += ip + ";";
                }
            }
        }

        Site FindItem(string domain)
        {
            Site server = arrServer.Find(item => item.name.Equals(domain, StringComparison.OrdinalIgnoreCase));
            return server;
        }

        private void ConfigSet_KeyDown(object sender, KeyEventArgs e)
        {
            if(e.KeyCode == Keys.Escape)
                Close();
        }
    }
}