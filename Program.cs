using System;
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
            Application.SetUnhandledExceptionMode(UnhandledExceptionMode.CatchException);
            // 对非UI线程异常进行处理，但是无法不退出程序
            AppDomain.CurrentDomain.UnhandledException += CurrentDomain_UnhandledException;


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

        /// <summary>
        /// 处理应用程序域内的未处理异常（非UI线程异常）
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        static void CurrentDomain_UnhandledException(object sender, UnhandledExceptionEventArgs e)
        {
            try
            {
                Exception ex = e.ExceptionObject as Exception;
                var msg = ex?.Message ?? Convert.ToString(e.ExceptionObject);
                MessageBox.Show(msg);
            }
            catch { }
        }
        private static void Application_ThreadException(object sender, ThreadExceptionEventArgs e)
        {
            string message = "出错了：" + e.Exception;
            MessageBox.Show(message);
            Utility.Log(message, "global");
        }
    }
}
