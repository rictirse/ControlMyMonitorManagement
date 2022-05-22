using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CMM.Library.Method
{
    public static class AssemblyData
    {
        /// <summary>
        /// 當下Assembly名稱
        /// </summary>
        public static string AssemblyName => System.Reflection.Assembly.GetExecutingAssembly().GetName().Name;

        public static string AppName => AppDomain.CurrentDomain.FriendlyName;

        /// <summary>
        /// 程式根目錄，無視工作目錄
        /// </summary>
        public static string Path => AppDomain.CurrentDomain.BaseDirectory;
        /// <summary>
        /// 版本
        /// </summary>
        public static string AssemblyVersion => GetAssemblyVersion();
        public static string AppVersion => GetFileVersion(Process.GetCurrentProcess().MainModule.FileName);
        /// <summary>
        /// CCM 輸出
        /// </summary>
        public static string smonitors => System.IO.Path.Combine(Path, "smonitors.tmp");

        static string GetAssemblyVersion()
        {
            var fi = System.Reflection.Assembly.GetExecutingAssembly().GetName().CodeBase.Replace(@"file:///", "");
            return GetFileVersion(fi);
        }

        public static string GetFileVersion(string filePath)
        {
            var fvi = FileVersionInfo.GetVersionInfo(filePath);
            return $"{fvi.FileMajorPart}." +
                   $"{fvi.FileMinorPart}." +
                   $"{fvi.FileBuildPart}." +
                   $"{fvi.FilePrivatePart}";
        }
    }
}
