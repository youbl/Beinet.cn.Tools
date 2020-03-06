using System;
using System.Collections.Generic;
using System.IO;
using System.Security.Cryptography;

namespace Beinet.cn.Tools.FileHash
{
    public static class HashUtil
    {
        /// <summary>
        /// 计算指定文件的MD5和SHA1值
        /// </summary>
        /// <param name="file">文件路径</param>
        /// <param name="countSha1">是否计算SHA1</param>
        /// <returns></returns>
        public static string[] CountMD5(string file, bool countSha1)
        {
            var ret = new string[2];

            SHA1CryptoServiceProvider getSha1 = null;
            if(countSha1)
                getSha1 = new SHA1CryptoServiceProvider();

            using (getSha1)
            using (MD5CryptoServiceProvider getMd5 = new MD5CryptoServiceProvider())
            using (FileStream getFile = new FileStream(file, FileMode.Open, FileAccess.Read, FileShare.Read))
            {
                getFile.Seek(0, SeekOrigin.Begin);
                ret[0] = BitConverter.ToString(getMd5.ComputeHash(getFile)).Replace("-", "");
                if (getSha1 != null)
                {
                    getFile.Seek(0, SeekOrigin.Begin);
                    ret[1] = BitConverter.ToString(getSha1.ComputeHash(getFile)).Replace("-", "");
                }
            }

            return ret;
        }

        /// <summary>
        /// 顺序计算指定文件夹下文件的MD5和SHA1值
        /// </summary>
        /// <param name="dir">目录</param>
        /// <param name="countSha1">是否计算SHA1</param>
        /// <param name="searchOption">计算顶层目录还是全部子目录</param>
        /// <returns></returns>
        public static Dictionary<string, string[]> CountDirMD5(string dir, bool countSha1, SearchOption searchOption = SearchOption.TopDirectoryOnly)
        {
            var files = Directory.GetFiles(dir, "*", searchOption);

            var ret = new Dictionary<string, string[]>();
            foreach (var file in files)
            {
                ret.Add(file, CountMD5(file, countSha1));
            }

            return ret;
        }

        /// <summary>
        /// 多线程计算指定文件夹下文件的MD5和SHA1值
        /// </summary>
        /// <param name="dir">目录</param>
        /// <param name="countSha1">是否计算SHA1</param>
        /// <param name="searchOption">计算顶层目录还是全部子目录</param>
        /// <returns></returns>
        public static Dictionary<string, string[]> CountDirMD5Sync(string dir, bool countSha1, SearchOption searchOption = SearchOption.TopDirectoryOnly)
        {
            var files = Directory.GetFiles(dir, "*", searchOption);

            var ret = new Dictionary<string, string[]>();
            foreach (var file in files)
            {
                ret.Add(file, CountMD5(file, countSha1));
            }

            return ret;
        }
    }
}
