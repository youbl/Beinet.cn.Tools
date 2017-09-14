using System;
using System.Collections.Generic;
using System.Configuration;
using System.Diagnostics;
using System.IO;
using System.Reflection;
using System.Text;
using System.Windows.Forms;
using Microsoft.Win32;

namespace Beinet.cn.Tools
{
    public partial class MainForm : Form
    {
        /// <summary>
        /// 指定哪个TabPage要显示什么工具窗体.
        /// 1、在主界面添加一个tab，并修改name，假设是tabNewTool；
        /// 2、在MainForm的构造函数里，添加一个对应关系，完成
        /// </summary>
        Dictionary<TabPage, Type> _tabForms; 

        public MainForm()
            : this(-1)
        {
        }

        public MainForm(int tabIndex)
        {
            InitializeComponent();

            // 暂时不处理合并dll功能，有bug

            #region 添加TabPage跟窗体类型的对应关系
            _tabForms = new Dictionary<TabPage, Type>();
            _tabForms.Add(tabGZip, typeof(GzipTest));
            _tabForms.Add(tabMd5File, typeof(FileHash.FileHash));
            _tabForms.Add(tabSqlInject, typeof(SqlInjectForm.SqlInject));
            _tabForms.Add(tabLvs, typeof(LvsManager.LVSControl));
            _tabForms.Add(tabEncrypt, typeof(CryptTool));
            _tabForms.Add(tabRegex, typeof(RegexTool.MainForm));
            _tabForms.Add(tabDataSync, typeof(DataSync.SqlServerTabForm));
            _tabForms.Add(tabQQWry, typeof(QQWry.IP_QQWry));
            _tabForms.Add(tabWebCompare, typeof(WebContentCompare.Compare));
            _tabForms.Add(tabOtherTools, typeof(Others.OtherTools));
            _tabForms.Add(tabImgTool, typeof(ImgTools.ImgTool));
            _tabForms.Add(tabIIS, typeof(Others.IIStool));
            #endregion



            string netVer = GetNetVersion();
            if(!string.IsNullOrEmpty(netVer))
            {
                textBox1.Text = "本机安装的.Net FrameWork版本清单：\r\n" + netVer;
            }

            // 隐藏不显示的窗体
            string strHide = ConfigurationManager.AppSettings["HideIndex"];
            if (!string.IsNullOrEmpty(strHide))
            {
                foreach (string item in strHide.Split(',', ';', ' '))
                {
                    string strIdx = item.Trim();
                    if (strIdx.Length <= 0)
                        continue;
                    int hideIdx;
                    if (int.TryParse(item.Trim(), out hideIdx))
                    {
                        tabControl1.TabPages[hideIdx].Visible = false;
                    }
                }
            }


            // 设置默认显示标签页
            int idx = tabIndex;
            if (tabIndex <= -1)
            {
                string tmp = ConfigurationManager.AppSettings["StartIndex"] ?? "";
                if(!int.TryParse(tmp, out idx))
                {
                    idx = -1;
                }
            }
            if (idx < tabControl1.TabPages.Count && idx >= 0)
            {

                tabControl1.SelectedIndex = idx;
                tabControl1_SelectedIndexChanged(tabControl1, null);
            }
        }

        private void button1_Click(object sender, EventArgs e)
        {
            OpenFileDialog ofd = new OpenFileDialog();
            ofd.Multiselect = true;
            var result = ofd.ShowDialog(this);
//            MessageBox.Show(result.ToString());
            if (result != DialogResult.OK)
            {
                return;
            }
            ShowFilesInfo(ofd.FileNames);
        }



        void ShowFilesInfo(IEnumerable<string> files)
        {
            bool isBit = rad1Bit.Checked;
            StringBuilder sb32 = new StringBuilder();
            StringBuilder sb64 = new StringBuilder();
            StringBuilder sb0 = new StringBuilder();

            ShowDirInfo(files, isBit, sb32, sb64, sb0);

            textBox1.Text = sb32.ToString();
            textBox2.Text = sb64.ToString();
            textBox3.Text = sb0.ToString();
        }

        void ShowDirInfo(IEnumerable<string> files, bool isBit, StringBuilder sb32, StringBuilder sb64, StringBuilder sb0)
        {
            foreach (string fileName in files)
            {
                if (File.Exists(fileName))
                {

                    string ver = GetVerInfo(fileName);
                    ushort t2 = GetPEArchitecture(fileName);
                    string bit;
                    if (t2 == 0x10b)
                    {
                        bit = "32位";
                        if (isBit)
                        {
                            sb32.AppendFormat("32位\t{0}\r\n\t{1}\r\n\r\n", fileName, ver);
                        }
                    }
                    else if (t2 == 0x20b)
                    {
                        bit = "64位";
                        if (isBit)
                            sb64.AppendFormat("64位\t{0}\r\n\t{1}\r\n\r\n", fileName, ver);
                    }
                    else
                    {
                        bit = "未知";
                        if (isBit)
                            sb0.AppendFormat("未知\t{0}\r\n\t{1}\r\n\r\n", fileName, ver);
                    }

                    if (!isBit)
                    {
                        if (ver.Contains("Debug版本"))
                        {
                            sb32.AppendFormat("{2}\t{0}\r\n\t{1}\r\n\r\n", fileName, ver, bit);
                        }
                        else if (ver.Contains("Release版本"))
                        {
                            sb64.AppendFormat("{2}\t{0}\r\n\t{1}\r\n\r\n", fileName, ver, bit);
                        }
                        else
                        {
                            sb0.AppendFormat("{2}\t{0}\r\n\t{1}\r\n\r\n", fileName, ver, bit);
                        }
                    }
                }
                else if (Directory.Exists(fileName))
                {
                    string fname = Path.GetFileName(fileName.TrimEnd('\\')).ToLower();
                    if (fname != string.Empty || !Utility.DirNoProcess.Contains(fname))
                    {
                        ShowDirInfo(Directory.GetFiles(fileName), isBit, sb32, sb64, sb0);
                        ShowDirInfo(Directory.GetDirectories(fileName), isBit, sb32, sb64, sb0);
                    }
                }
            }
        }

        static string GetVerInfo(string filename)
        {
            try
            {
                using (FileStream stream = new FileStream(filename, FileMode.Open))
                using (MemoryStream memStream = new MemoryStream())
                {
                    byte[] b = new byte[4096];
                    while (stream.Read(b, 0, b.Length) > 0)
                    {
                        memStream.Write(b, 0, b.Length);
                    }
                    Assembly asms = Assembly.Load(memStream.ToArray());//ReflectionOnlyLoad

                    object[] debugAtt = asms.GetCustomAttributes(typeof(DebuggableAttribute), false);
                    string ret = string.Empty;
                    if (debugAtt.Length >= 1)
                    {
                        if (((DebuggableAttribute)debugAtt[0]).IsJITTrackingEnabled)
                            ret += "Debug版本，基于.Net " + asms.ImageRuntimeVersion;
                        else
                            ret += "Release版本，基于.Net " + asms.ImageRuntimeVersion;
                    }
                    else
                    {
                        ret += "未知版本，基于.Net " + asms.ImageRuntimeVersion;
                    }
                    ret = asms.FullName + "\r\n\t" + ret;
                    return ret;
                }
            }
            catch (Exception exp)
            {
                return exp.Message;//.ToString();
            }
        }

        public static ushort GetPEArchitecture(string pFilePath)
        {
            ushort architecture = 0;
            try
            {
                using (System.IO.FileStream fStream = new System.IO.FileStream(pFilePath, System.IO.FileMode.Open, System.IO.FileAccess.Read))
                {
                    using (System.IO.BinaryReader bReader = new System.IO.BinaryReader(fStream))
                    {
                        if (bReader.ReadUInt16() == 23117) //check the MZ signature
                        {
                            fStream.Seek(0x3A, System.IO.SeekOrigin.Current); //seek to e_lfanew.
                            fStream.Seek(bReader.ReadUInt32(), System.IO.SeekOrigin.Begin); //seek to the start of the NT header.
                            if (bReader.ReadUInt32() == 17744) //check the PE\0\0 signature.
                            {
                                fStream.Seek(20, System.IO.SeekOrigin.Current); //seek past the file header,
                                architecture = bReader.ReadUInt16(); //read the magic number of the optional header.
                            }
                        }
                    }
                }
            }
            catch (Exception)
            {
                return 0;
            }
            //if architecture returns 0, there has been an error.
            return architecture;
        }


        #region 文件拖拽进来的处理,要先设置Form.AllowDrop为true
        
        private void Form1_DragDrop(object sender, DragEventArgs e)
        {
            var obj = e.Data.GetData(DataFormats.FileDrop);
            if (obj == null)
            {
                return;
            }
            //MessageBox.Show(obj.ToString());
            string[] files = obj as string[];
            if (files == null)
            {
                return;
            }

            ShowFilesInfo(files);
        }

        /// <summary>
        /// 拖拽文件进来时，才允许
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void Form1_DragEnter(object sender, DragEventArgs e)
        {
            if (tabDllAnlyse.TabIndex == tabControl1.SelectedIndex && e.Data.GetDataPresent(DataFormats.FileDrop))
            {
                e.Effect = DragDropEffects.Link;
            }
            else
            {
                e.Effect = DragDropEffects.None;
            }
        }
        #endregion


        private void tabControl1_SelectedIndexChanged(object sender, EventArgs e)
        {
            TabPage page = tabControl1.SelectedTab;
            Type formType;
            if (_tabForms.TryGetValue(page, out formType))
            {
                if (page.Controls.Count <= 0)
                    AddFormControl(page, formType);
            }
            //if (tabControl1.SelectedTab == )
            //{
            //    AddFormControl<>(page);
            //}
            //else if (tabControl1.SelectedTab == )
            //{
            //    AddFormControl<>(page);
            //}
        }

        //static void AddFormControl<T>(Control parent) where T : Form, new()
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




        // 获取本机安装的net版本清单
        public static string GetNetVersion()
        {
            StringBuilder sb = new StringBuilder();
            using (RegistryKey registryKey = Registry.LocalMachine.OpenSubKey("SOFTWARE\\Microsoft\\NET Framework Setup\\NDP\\"))
            {
                if (registryKey == null)
                {
                    return null;
                }

                string[] subKeyNames = registryKey.GetSubKeyNames();
                foreach (string verKey in subKeyNames)
                {
                    if (verKey[0] != 'v')
                    {
                        continue;
                    }
                    RegistryKey registryKey2 = registryKey.OpenSubKey(verKey);
                    if (registryKey2 == null)
                    {
                        continue;
                    }

                    string ver = Convert.ToString(registryKey2.GetValue("Version", ""));
                    string sp = Convert.ToString(registryKey2.GetValue("SP", ""));
                    string install = Convert.ToString(registryKey2.GetValue("Install", ""));
                    if (string.IsNullOrEmpty(install))
                    {
                        sb.AppendFormat("{0}{1}\r\n", verKey.PadRight(12), ver);
                    }
                    else if (!string.IsNullOrEmpty(sp) && install == "1")
                    {
                        sb.AppendFormat("{0}{1}  SP{2}\r\n", verKey.PadRight(12), ver, sp);
                    }

                    if (!string.IsNullOrEmpty(ver))
                    {
                        continue;
                    }

                    string[] subKeyNames2 = registryKey2.GetSubKeyNames();
                    foreach (string text4 in subKeyNames2)
                    {
                        RegistryKey registryKey3 = registryKey2.OpenSubKey(text4);
                        if (registryKey3 == null)
                        {
                            continue;
                        }
                        ver = Convert.ToString(registryKey3.GetValue("Version", ""));
                        if (ver != "")
                        {
                            sp = Convert.ToString(registryKey3.GetValue("SP", ""));
                        }
                        install = Convert.ToString(registryKey3.GetValue("Install", ""));
                        if (install == "")
                        {
                            sb.AppendFormat("{0}{1}\r\n", verKey.PadRight(12), ver);
                        }
                        else if (sp != "" && install == "1")
                        {
                            sb.AppendFormat("            {0}  {1}  SP{2}\r\n", text4, ver, sp);
                        }
                        else if (install == "1")
                        {
                            sb.AppendFormat("            {0}  {1}\r\n", text4, ver);
                        }
                    }
                }
            }
            return sb.ToString();
        }

    }
}
