using System;
using System.Collections.Generic;
using System.IO;
using System.Security.Cryptography;
using System.Text;

namespace Beinet.cn.Tools.FileHash
{
    public static class HashUtil
    {
        /// <summary>
        /// 计算指定文件的MD5和SHA1值，以及文件大小、文件编码返回
        /// </summary>
        /// <param name="file">文件路径</param>
        /// <param name="countSha1">是否计算SHA1</param>
        /// <returns></returns>
        public static string[] CountMD5(string file, bool countSha1)
        {
            var ret = new string[4];

            SHA1CryptoServiceProvider getSha1 = null;
            if (countSha1)
                getSha1 = new SHA1CryptoServiceProvider();

            using (getSha1)
            using (MD5CryptoServiceProvider getMd5 = new MD5CryptoServiceProvider())
            using (FileStream getFile = new FileStream(file, FileMode.Open, FileAccess.Read, FileShare.Read))
            {
                ret[2] = getFile.Length.ToString();

                getFile.Seek(0, SeekOrigin.Begin);
                ret[0] = BitConverter.ToString(getMd5.ComputeHash(getFile)).Replace("-", "");

                if (getSha1 != null)
                {
                    getFile.Seek(0, SeekOrigin.Begin);
                    ret[1] = BitConverter.ToString(getSha1.ComputeHash(getFile)).Replace("-", "");
                }

                getFile.Seek(0, SeekOrigin.Begin);
                ret[3] = TxtFileEncoder.GetType(getFile).EncodingName;
            }

            return ret;
        }

        /// <summary>
        /// 顺序计算指定文件夹下文件的MD5和SHA1值
        /// </summary>
        /// <param name="dir">目录</param>
        /// <param name="countSha1">是否计算SHA1</param>
        /// <param name="containSubDir">计算是否包含子目录</param>
        /// <returns></returns>
        public static Dictionary<string, string[]> CountDirMD5(string dir, bool countSha1, bool containSubDir)
        {
            var ret = new Dictionary<string, string[]>();

            var files = Directory.GetFiles(dir, "*", SearchOption.TopDirectoryOnly);
            foreach (var file in files)
            {
                ret.Add(file, CountMD5(file, countSha1));
            }

            var subdirs = Directory.GetDirectories(dir, "*", SearchOption.TopDirectoryOnly);
            foreach (var subdir in subdirs)
            {
                var result = CountDirMD5(subdir, countSha1, containSubDir);
                foreach (var item in result)
                {
                    ret.Add(item.Key, item.Value);
                }
            }

            return ret;
        }


        private static int _fileCnt = 0;
        public static void HashAndSaveToFile(string dir, bool countSha1, bool containSubDir, string saveFile)//, int threadNum
        {
            // var arrHandler = new HashFileClass[threadNum];
            // for (var i = 0; i < threadNum; i++)
            // {
            //     arrHandler[i] = new HashFileClass(i, countSha1);
            // }

            _fileCnt = 0;
            using (var sw = new StreamWriter(saveFile, true, Encoding.UTF8))
            {
                string errFile = null;
                try
                {
                    var files = Directory.GetFiles(dir, "*", SearchOption.TopDirectoryOnly);
                    foreach (var file in files)
                    {
                        errFile = file;
                        var result = CountMD5(file, countSha1);
                        sw.Write("{0},{1},{2},{3}\r\n", file, result[0], result[1], result[2]);
                        _fileCnt++;
                        if(_fileCnt % 1000 == 0)
                            sw.Flush();
                    }
                }
                catch(Exception exp)
                {
                    Error(dir + ":" + errFile + ":" + exp);
                }
            }

            try
            {
                var subdirs = Directory.GetDirectories(dir, "*", SearchOption.TopDirectoryOnly);
                foreach (var subdir in subdirs)
                {
                    HashAndSaveToFile(subdir, countSha1, containSubDir, saveFile);
                }
            }
            catch (Exception exp)
            {
                Error(dir + ":::" + exp);
            }
        }

        static void Error(string exp)
        {
            var errFile = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "err.txt");
            using (var sw = new StreamWriter(errFile, true, Encoding.UTF8))
            {
                sw.WriteLine("{0} {1}\r\n", DateTime.Now, exp);
            }
        }

        // class HashFileClass
        // {
        //     SemaphoreSlim _semaphore = new SemaphoreSlim(1, 1);
        //     private bool _countSha1;
        //     private StringBuilder _sb = new StringBuilder();
        //
        //
        //     private string _file;
        //
        //     private int _count;
        //     public int Count { get => _count; }
        //
        //     public HashFileClass(int idx, bool countSha1)
        //     {
        //         _countSha1 = countSha1;
        //
        //         _file = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, $"{idx}.csv");
        //     }
        //     public void Process(string file)
        //     {
        //         _semaphore.Wait();
        //         try
        //         {
        //             var cnt = Interlocked.Increment(ref _count);
        //             var result = CountMD5(file, _countSha1);
        //             _sb.AppendFormat("{0},{1},{2},{3}\r\n", file, result[0], result[1], result[2]);
        //
        //             if(cnt % 100 == 0)
        //                 Save();
        //         }
        //         finally
        //         {
        //             _semaphore.Release();
        //         }
        //     }
        //
        //     void Save()
        //     {
        //         string tmp = _sb.ToString();
        //         _sb.Clear();
        //
        //         using (var sw = new StreamWriter(_file, true, Encoding.UTF8))
        //         {
        //             sw.WriteLine(tmp);
        //         }
        //     }
        // }
    }
}
