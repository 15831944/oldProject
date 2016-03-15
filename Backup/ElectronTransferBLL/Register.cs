using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Win32;

namespace ElectronTransferBll
{
    public class Register
    {
        /// <summary>
        /// 获取AutoCAD的注册表键值
        /// </summary>
        /// <returns>返回AutoCAD的注册表键值</returns>
        public static string GetAutoCADKey()
        {
            // 获取当前用户的注册表键值
            RegistryKey hive = Registry.CurrentUser;
            //打开AutoCAD的注册表键值
            RegistryKey ack = hive.OpenSubKey("Software\\Autodesk\\AutoCAD");
            using (ack)
            {
                // 获取AutoCAD主版本的注册表键值
                string ver = ack.GetValue("CurVer") as string;
                if (ver == null)
                {
                    return "";
                }
                RegistryKey verk = ack.OpenSubKey(ver);
                using (verk)
                {
                    // 获取对应语言版本的注册表键值
                    string lng = verk.GetValue("CurVer") as string;
                    if (lng == null)
                    {
                        return "";
                    }
                    RegistryKey lngk = verk.OpenSubKey(lng);
                    using (lngk)
                    {
                        // 返回无前缀的注册表键值
                        return lngk.Name.Substring(hive.Name.Length + 1);
                    }
                }
            }
        }
        private static RegistryKey GetKey(string path)
        {
            return Registry.LocalMachine.OpenSubKey(path, true);
        }
        /// <summary>
        /// 获取键值
        /// </summary>
        /// <param name="path"></param>
        /// <param name="keyName"></param>
        /// <returns></returns>
        public static string GetKeyValue(string path,string keyName)
        {
            var key = GetKey(path);
            return key.GetValue(keyName).ToString();
        }

        /// <summary>
        /// 获取AutoCAD的版本号
        /// </summary>
        /// <returns></returns>
        public static string GetAutoCADVersion()
        {
            return GetAutoCADKey().Split('\\')[3];
        }

        /// <summary>
        /// 获取AutoCAD所属的注册表键名
        /// </summary>
        /// <returns>返回AutoCAD所属的注册表键名</returns>
        public static string GetAutoCADKeyName()
        {
            // 获取HKEY_CURRENT_USER键
            RegistryKey keyCurrentUser = Registry.CurrentUser;
            // 打开AutoCAD所属的注册表键:HKEY_CURRENT_USER\Software\Autodesk\AutoCAD
            RegistryKey keyAutoCAD = keyCurrentUser.OpenSubKey("Software\\Autodesk\\AutoCAD");
            //获得表示当前的AutoCAD版本的注册表键值:R18.2
            string valueCurAutoCAD = keyAutoCAD.GetValue("CurVer").ToString();
            if (valueCurAutoCAD == null) return "";//如果未安装AutoCAD，则返回
            //获取当前的AutoCAD版本的注册表键:HKEY_LOCAL_MACHINE\Software\Autodesk\AutoCAD\R18.2
            RegistryKey keyCurAutoCAD = keyAutoCAD.OpenSubKey(valueCurAutoCAD);
            //获取表示AutoCAD当前语言的注册表键值:ACAD-a001:804
            string language = keyCurAutoCAD.GetValue("CurVer").ToString();
            //获取AutoCAD当前语言的注册表键:HKEY_LOCAL_MACHINE\Software\Autodesk\AutoCAD\R18.2\ACAD-a001:804
            RegistryKey keyLanguage = keyCurAutoCAD.OpenSubKey(language);
            //返回去除HKEY_LOCAL_MACHINE前缀的当前AutoCAD注册表项的键名:Software\Autodesk\AutoCAD\R18.2\ACAD-a001:804
            return keyLanguage.Name.Substring(keyCurrentUser.Name.Length + 1);
        }

        /// <summary>
        /// 创建自动加载.NET程序所需要的注册表项
        /// </summary>
        /// <param name="appName">.NET程序名</param>
        /// <param name="appPath">.NET程序的路径</param>
        /// <param name="currentUser">注册表项是创建在HKEY_CURRENT_USER键下还是HKEY_LOCAL_MACHINE健下</param>
        /// <param name="overwrite">是否覆盖同名的程序</param>
        /// <returns>如果创建注册表项成功则返回true,否则返回false</returns>
        public static bool CreateDemandLoadingEntries(string appName, string appPath, bool currentUser, bool overwrite)
        {
            //获取AutoCAD所属的注册表键名
            var autoCADKeyName = GetAutoCADKeyName();
            //确定是HKEY_CURRENT_USER还是HKEY_LOCAL_MACHINE
            RegistryKey keyRoot = currentUser ? Registry.CurrentUser : Registry.LocalMachine;
            // 由于某些AutoCAD版本的HKEY_CURRENT_USER可能不包括Applications键值，因此要创建该键值
            // 如果已经存在该鍵，无须担心可能的覆盖操作问题，因为CreateSubKey函数会以写的方式打开它而不会执行覆盖操作
            RegistryKey keyApp = keyRoot.CreateSubKey(autoCADKeyName + "\\" + "Applications");
            //若存在同名的程序且选择不覆盖则返回
            if (!overwrite && keyApp.GetSubKeyNames().Contains(appName))
                return false;
            //创建相应的键并设置自动加载应用程序的选项
            RegistryKey keyUserApp = keyApp.CreateSubKey(appName);
            keyUserApp.SetValue(appName,appPath, RegistryValueKind.String);
            return true;//创建键成功则返回
        }

        /// <summary>
        /// 获取注册表项
        /// </summary>
        /// <param name="itemName"></param>
        /// <param name="keyName"> </param>
        /// <param name="currentUser">注册表项是创建在HKEY_CURRENT_USER键下还是HKEY_LOCAL_MACHINE健下</param>
        /// <returns>如果获取注册表项成功则返回Value，否则返回空</returns>
        public static string GetItemValue(string itemName,string keyName, bool currentUser)
        {
            var value = string.Empty;
            try
            {
                // 获取AutoCAD所属的注册表键名
                string cadName = GetAutoCADKeyName();
                // 确定是HKEY_CURRENT_USER还是HKEY_LOCAL_MACHINE
                RegistryKey keyRoot = currentUser ? Registry.CurrentUser : Registry.LocalMachine;
                // 以写的方式打开Applications注册表键
                RegistryKey keyApp = keyRoot.OpenSubKey(cadName + "\\" + "Applications" + "\\" + itemName, true);
                value=keyApp.GetValue(keyName).ToString();
            }
            catch
            {
                return value;
            }
            return value;
        }

        /// <summary>
        /// 修改注册表项
        /// </summary>
        /// <param name="itemName"></param>
        /// <param name="keyName"> </param>
        /// <param name="obj"> </param>
        /// <param name="currentUser">注册表项是创建在HKEY_CURRENT_USER键下还是HKEY_LOCAL_MACHINE健下</param>
        /// <returns>如果修改注册表项成功则返回true，否则返回false</returns>
        public static bool SetItemValue(string itemName, string keyName,object obj, bool currentUser)
        {
            try
            {
                // 获取AutoCAD所属的注册表键名
                string cadName = GetAutoCADKeyName();
                // 确定是HKEY_CURRENT_USER还是HKEY_LOCAL_MACHINE
                RegistryKey keyRoot = currentUser ? Registry.CurrentUser : Registry.LocalMachine;
                // 以写的方式打开Applications注册表键
                RegistryKey keyApp = keyRoot.OpenSubKey(cadName + "\\" + "Applications" + "\\" + itemName, true);
                keyApp.SetValue(keyName, obj);
                return true;
            }
            catch
            {
                return false;
            }
        }
        /// <summary>
        /// 删除自动加载.NET程序所需要的注册表项
        /// </summary>
        /// <param name="appName">.NET程序名</param>
        /// <param name="currentUser">注册表项是创建在HKEY_CURRENT_USER键下还是HKEY_LOCAL_MACHINE健下</param>
        /// <returns>如果删除注册表项成功则返回true，否则返回false</returns>
        public static bool RemoveDemandLoadingEntries(string appName, bool currentUser)
        {
            try
            {
                // 获取AutoCAD所属的注册表键名
                string cadName = GetAutoCADKeyName();
                // 确定是HKEY_CURRENT_USER还是HKEY_LOCAL_MACHINE
                RegistryKey keyRoot = currentUser ? Registry.CurrentUser : Registry.LocalMachine;
                // 以写的方式打开Applications注册表键
                RegistryKey keyApp = keyRoot.OpenSubKey(cadName + "\\" + "Applications", true);
                //删除指定名称的注册表键
                keyApp.DeleteSubKeyTree(appName);
            }
            catch
            {
                return false;
            }
            return true;
        }
    }
}
