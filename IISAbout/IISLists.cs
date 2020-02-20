using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace Beinet.cn.Tools.IISAbout
{
    public partial class IISLists : Form
    {
        private IISOperation _operation { get; set; }

        public IISLists()
        {
            InitializeComponent();
        }

        private void BtnGetIIS_Click(object sender, EventArgs e)
        {
            dgvSites.DataSource = null;
            _operation = new IISOperation(txtIP.Text);
            var sites = _operation.ListSite();
            dgvSites.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.AllCellsExceptHeader;
            dgvSites.DataSource = sites;
        }

        private void DgvSites_CellFormatting(object sender, DataGridViewCellFormattingEventArgs e)
        {

        }
    }
}
