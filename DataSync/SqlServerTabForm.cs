using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace Beinet.cn.Tools.DataSync
{
    public partial class SqlServerTabForm : Form
    {
        Dictionary<TabPage, Type> _tabForms;

        public SqlServerTabForm()
        {
            InitializeComponent();
            _tabForms = new Dictionary<TabPage, Type>();
            _tabForms.Add(tabPage1, typeof(SqlForm));
            _tabForms.Add(tabPage2, typeof(MainForm));
            _tabForms.Add(tabPage3, typeof(IISLogForm));
        }

        protected override void OnLoad(EventArgs e)
        {
            base.OnLoad(e);
            tabControl1_SelectedIndexChanged(tabControl1, null);
        }

        private void tabControl1_SelectedIndexChanged(object sender, EventArgs e)
        {
            TabPage page = tabControl1.SelectedTab;
            Type formType;
            if (_tabForms.TryGetValue(page, out formType))
            {
                if (page.Controls.Count <= 0)
                    AddFormControl(page, formType);
            }
        }
        private static void AddFormControl(Control parent, Type formType)
        {
            //formType.BaseType
            Form frm = Activator.CreateInstance(formType) as Form;
            if (frm != null)
            {
                frm.FormBorderStyle = FormBorderStyle.None;
                frm.Dock = DockStyle.Fill;
                frm.TopLevel = false;
                parent.Controls.Clear();
                parent.Controls.Add(frm);
                frm.Show();
            }
        }
    }
}
