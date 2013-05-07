using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Windows.Forms;

namespace Beinet.cn.Tools
{
    static class Program
    {
        /// <summary>
        /// 应用程序的主入口点。
        /// </summary>
        [STAThread]
        static void Main(string[] args)
        {
            Application.ThreadException += Application_ThreadException;
            
            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);
            MainForm frm;
            int tabIndex;
            if (args == null || args.Length <= 0 || !int.TryParse(args[0], out tabIndex))
            {
                frm = new MainForm();
            }
            else
            {
                frm = new MainForm(tabIndex);
            }
            Application.Run(frm);
        }

        private static void Application_ThreadException(object sender, ThreadExceptionEventArgs e)
        {
            string message = "出错了：" + e.Exception;
            MessageBox.Show(message);
            Utility.Log(message, "global");
        }
    }
}
