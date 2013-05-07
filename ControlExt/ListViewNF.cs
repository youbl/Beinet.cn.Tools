using System;
using System.Windows.Forms;

namespace Beinet.cn.Tools.ControlExt
{
    class ListViewNF : System.Windows.Forms.ListView
    {
        #region 动态添加数据，不闪烁的代码
        public ListViewNF()
        {            // 开启双缓冲          
            this.SetStyle(ControlStyles.OptimizedDoubleBuffer | ControlStyles.AllPaintingInWmPaint, true);
            // Enable the OnNotifyMessage event so we get a chance to filter out       
            // Windows messages before they get to the form's WndProc       
            this.SetStyle(ControlStyles.EnableNotifyMessage, true);
        }
        protected override void OnNotifyMessage(Message m)
        {
            //Filter out the WM_ERASEBKGND message        
            if (m.Msg != 0x14)
            {
                base.OnNotifyMessage(m);
            }
        }
        #endregion


        #region 支持Scroll事件的代码

        public event EventHandler Scroll;
        public event EventHandler HScroll;
        public event EventHandler VScroll;

        const int WM_HSCROLL = 0x0114;
        const int WM_VSCROLL = 0x0115;
        protected override void WndProc(ref Message m)
        {
            switch (m.Msg)
            {
                case WM_HSCROLL:
                    var HArg = new EventArgs();
                    OnScroll(this, HArg);
                    OnHScroll(this, HArg);
                    break;
                case WM_VSCROLL:
                    var VArg = new EventArgs();
                    OnScroll(this, VArg);
                    OnVScroll(this, VArg);
                    break;
            }
            base.WndProc(ref m);
        }

        virtual protected void OnHScroll(object sender, EventArgs e)
        {
            if (HScroll != null)
                HScroll(this, e);
        }
        virtual protected void OnVScroll(object sender, EventArgs e)
        {
            if (VScroll != null)
                VScroll(this, e);
        }
        virtual protected void OnScroll(object sender, EventArgs e)
        {
            if (Scroll != null)
                Scroll(this, e);
        }
        #endregion

    }
}
