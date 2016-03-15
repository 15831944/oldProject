using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using ElectronTransferDal.Cad;
using ElectronTransferFramework;

namespace ElectronTransferView.ViewManager
{
    public class ViewManagerHelper
    {
        /// <summary>
        /// 获取设备组
        /// </summary>
        /// <param name="dirPath">符号库路径</param>
        /// <returns></returns>
        public List<string> GetGroupListBySymbolPath(string dirPath)
        {
            var listGroup = new List<string>();
            try
            {
                if (!string.IsNullOrEmpty(dirPath))
                {
                    var directory = new DirectoryInfo(dirPath);
                    var fileSystemInfos = directory.GetFileSystemInfos();
                    foreach (var fileSystemInfo in fileSystemInfos.OfType<DirectoryInfo>())
                    {
                        listGroup.Add(fileSystemInfo.Name);
                    }
                }
                else
                    PublicMethod.Instance.ShowMessage("符号库路径不存在！");
            }
            catch (Exception ex)
            {
                LogManager.Instance.Error(ex);
            }
            return listGroup;
        }
    }
}
