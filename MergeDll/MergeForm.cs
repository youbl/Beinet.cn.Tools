using System;
using System.Text.RegularExpressions;
using System.Threading;
using System.Windows.Forms;
using ILMerging;

namespace Beinet.cn.Tools.MergeDll
{
    public partial class MergeForm : Form
    {
        //private string _path = AppDomain.CurrentDomain.BaseDirectory;
        static bool _mergeRunning = false;
        static Regex _verReg = new Regex(@"^\d+(?:\.\d+){0,3}$", RegexOptions.Compiled);

        public MergeForm()
        {
            InitializeComponent();
        }


        static void DoMerge(string outputFile, string[] dlls,
            ILMerge.Kind targetType = ILMerge.Kind.SameAsPrimaryAssembly,
            string logFile = null, bool getXml = false, bool getPDB = false,
            string version = null, string[] searchDirs = null)
        {
            if (string.IsNullOrEmpty(outputFile))
            {
                throw new ArgumentException("未提供输出文件名outputFile");
            }

            if (dlls == null || dlls.Length == 0)
            {
                throw new ArgumentException("未提供输入文件dlls");
            }

            ILMerge merge = new ILMerge();

            // 是否允许同名的public class，如果允许，把这些类型添加进去，比如需要合并有混淆的dll时（那些类被变成a、b、c之类了）
            // 如果AllowDuplicateType的参数设置为null，表示允许所有的同名类（用重命名的方法）
            // 默认不允许，如果有同名的public class会抛出异常
            //merge.AllowDuplicateType("");
            // 注：AllowDuplicateType不允许跟UnionMerge属性同时使用

            // 默认值false
            merge.AllowWildCards = false;// 是否允许文件名里包含通配符，注：.. 这样的2个点 设置为true也不允许

            //merge.Closed = true;
            //merge.AllowZeroPeKind = true;


            // 默认值true
            merge.DebugInfo = getPDB;    // 是否要创建一个pdb调试文件

            if (!string.IsNullOrEmpty(logFile))
            {
                // 默认值false
                merge.Log = true; // 是否输出日志
                merge.LogFile = logFile; // 如果为null，日志输出到控制台，否则输出到这个文件里
            }

            // 必须设置
            merge.OutputFile = outputFile; // 输出的文件路径+文件名

            // 设置要合并的dll列表，第一个文件是主文件
            //string[] dlls = new string[] { };
            merge.SetInputAssemblies(dlls);

            // 如果dll列表没有设置全路径，那么会在dllDirs这些目录下查找
            if (searchDirs != null && searchDirs.Length > 0)
            {
                string[] dllDirs = searchDirs;
                merge.SetSearchDirectories(dllDirs);
            }

            // 设置.net framework的版本，第一个参数只能是"v1", "v1.1", "v2", "v4"
            // 第二个参数是mscorlib.dll所在目录
            //merge.SetTargetPlatform("", "");

            // 默认跟主文件一致
            merge.TargetKind = targetType;    // 输出类型:Dll, Exe, WinExe

            // 如果设置为true，则所有同名的类型下的所有成员，会合并到一个类型下
            // 默认值false
            // merge.UnionMerge = true;
            // 注：AllowDuplicateType不允许跟UnionMerge属性同时使用

            // 默认值null 设置目标文件的版本号
            if (!string.IsNullOrEmpty(version))
            {
                merge.Version = new Version(version);
            }
            // 默认值false 是否合并出一个xml文档
            merge.XmlDocumentation = getXml;

            merge.Merge();
        }


        private void btnOutput_Click(object sender, EventArgs e)
        {
            SaveFileDialog ofd = new SaveFileDialog();
            //ofd.FileName = Path.Combine(_path, "a.dll");
            ofd.Filter = "EXE文件|*.exe|DLL文件|*.dll|所有文件|*.*";
            if (ofd.ShowDialog(this) != DialogResult.OK)
            {
                return;
            }
            txtOutput.Text = ofd.FileName;
        }

        private void btnLogFile_Click(object sender, EventArgs e)
        {
            SaveFileDialog ofd = new SaveFileDialog();
            //ofd.FileName = Path.Combine(_path, "a.dll");
            ofd.Filter = "日志文件(*.log;*.txt)|*.log;*.txt|所有文件|*.*";
            if (ofd.ShowDialog(this) != DialogResult.OK)
            {
                return;
            }
            txtLogFile.Text = ofd.FileName;
        }

        private void btnAddDlls_Click(object sender, EventArgs e)
        {
            OpenFileDialog ofd = new OpenFileDialog();
            //ofd.FileName = Path.Combine(_path, "a.dll");
            ofd.Multiselect = true;
            ofd.Filter = "DLL或EXE文件|*.dll;*.exe|所有文件|*.*";
            if (ofd.ShowDialog(this) != DialogResult.OK)
            {
                return;
            }
            foreach (string fileName in ofd.FileNames)
            {
                listBox1.Items.Add(fileName);
            }
        }

        private void btnDel_Click(object sender, EventArgs e)
        {
            for (int idx = listBox1.SelectedItems.Count - 1; idx >= 0; idx--)
            {
                listBox1.Items.Remove(listBox1.SelectedItems[idx]);
            }
        }

        private void btnDelAll_Click(object sender, EventArgs e)
        {
            listBox1.Items.Clear();
        }

        private void btnDo_Click(object sender, EventArgs e)
        {
            if (_mergeRunning)
            {
                MessageBox.Show("上一次合并工作还在进行中，请稍候");
                return;
            }

            if (string.IsNullOrEmpty(txtOutput.Text))
            {
                MessageBox.Show("请选择要保存到的文件名");
                return;
            }
            if (listBox1.Items.Count <= 1)
            {
                MessageBox.Show("请选择要合并的文件，至少2个");
                return;
            }
            string version = txtVersion.Text;
            if (!string.IsNullOrEmpty(version) && !_verReg.IsMatch(version))
            {
                MessageBox.Show("请正确输入版本号，格式xx.xx.xx.xx，最少一组数字，最多4组");
                return;
            }

            string[] files = new string[listBox1.Items.Count];
            int idx = 0;
            foreach (var item in listBox1.Items)
            {
                files[idx] = item.ToString();
                idx++;
            }

            ILMerge.Kind type;
            if (radDll.Checked)
            {
                type = ILMerge.Kind.Dll;
            }
            else if (radExe.Checked)
            {
                type = ILMerge.Kind.Exe;
            }
            else if (radWin.Checked)
            {
                type = ILMerge.Kind.WinExe;
            }
            else
            {
                type = ILMerge.Kind.SameAsPrimaryAssembly;
            }
            string logFile = string.IsNullOrEmpty(txtLogFile.Text) ? null : txtLogFile.Text;
            bool getXml = chkXml.Checked;
            bool debug = chkDebug.Checked;

            _mergeRunning = true;
            ThreadPool.UnsafeQueueUserWorkItem(state =>
            {
                try
                {
                    DoMerge(txtOutput.Text, files, type, logFile, getXml, debug, version);
                    MessageBox.Show("合并成功");
                }
                catch (Exception exp)
                {
                    MessageBox.Show("出错:" + exp);
                }
                finally
                {
                    _mergeRunning = false;
                }
            }, null);
        }
    }
}
