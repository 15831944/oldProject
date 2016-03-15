using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Windows.Forms;
using Microsoft.Win32;

namespace MyAutoCAD
{
    static class Program
    {
        /// <summary>
        /// 应用程序的主入口点。
        /// </summary>
        [STAThread]
        static void Main()
        {
            try
            {
                CheckVersion();
            }catch(Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }
        private const string v2010Path = @"SOFTWARE\Autodesk\AutoCAD\R18.0\ACAD-8001:804";
        private const string v2012Path = @"SOFTWARE\Autodesk\AutoCAD\R18.2\ACAD-A001:804";
        private const string v2010AppPath = @"SOFTWARE\Autodesk\AutoCAD\R18.0\ACAD-8001:804\Applications\CustomAutoCAD";
        private const string v2012AppPath = @"SOFTWARE\Autodesk\AutoCAD\R18.2\ACAD-A001:804\Applications\CustomAutoCAD";

        [Flags]
        enum Version { None = 0, v2010 = 1, v2012 = 4 };
        private static Version GetVersion()
        {
            Version version = Version.None;
            var key2010 = GetKey(v2010Path);
            if (key2010 != null)
            {
                version = Version.v2010;
                key2010.Close();
            }
            var key2012 = GetKey(v2012Path);
            if (key2012 != null)
            {
                version |= Version.v2012;
                key2012.Close();
            }
            return version;
        }
        private static void Install(string regPath, string assemblyPath, string description)
        {
            var key = GetKey(regPath);
            key.SetValue("DESCRIPTION", description, RegistryValueKind.String);
            key.SetValue("LOADER", assemblyPath, RegistryValueKind.String);
            key.SetValue("LOADCTRLS", 0x1c102, RegistryValueKind.DWord);
            key.SetValue("MANAGED", 0x1c101, RegistryValueKind.DWord);
        }
        private static void InstallAll(string assemblyPath, string description)
        {
            var applications = new List<string>();
            var acadVer = Register.GetAutoCADVersion();

            if (acadVer.Contains("R18.0"))
            {
                applications.Add(v2010AppPath);
                CreateKey(v2010AppPath);
            }
            applications.ForEach(o => Install(o, assemblyPath, description));
        }
        private static RegistryKey GetKey(string path)
        {
            return Registry.LocalMachine.OpenSubKey(path, true);
        }

        private static void CheckVersion()
        {
            var version = GetVersion();
            if (version == Version.None)
            {
                MessageBox.Show("本机没有合适的AutoCad");
            }
            var assemblyPath = Directory.GetCurrentDirectory();
            //写注册表
            InstallAll(Path.Combine(assemblyPath, "ElectronTransferView.dll"), "北京中合实创电力科技有限公司开发");
            var cadLocationPath = GetAutoCADLocationPath();
            //启动AutoCAD
            StartAutoCAD(cadLocationPath);
        }
        /// <summary>
        /// 获取AutoCAD可执行路径
        /// </summary>
        /// <returns></returns>
        private static string GetAutoCADLocationPath()
        {
            //获取CAD2010的可执行路径
            var acadLocation = GetKeyValue(v2010Path, "AcadLocation");
            //cad路径
            return Path.Combine(acadLocation, "acad.exe");
        }
        /// <summary>
        /// 启动AutoCAD
        /// </summary>
        /// <param name="cadLocationPath"></param>
        private static void StartAutoCAD(string cadLocationPath)
        {
            System.Diagnostics.Process.Start(cadLocationPath);
        }

        private static RegistryKey CreateKey(string path)
        {
            return Registry.LocalMachine.CreateSubKey(path);
        }
        /// <summary>
        /// 获取键值
        /// </summary>
        /// <param name="path"></param>
        /// <param name="keyName"></param>
        /// <returns></returns>
        public static string GetKeyValue(string path, string keyName)
        {
            var key = GetKey(path);
            return key.GetValue(keyName).ToString();
        }
    }
}
