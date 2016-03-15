using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Autodesk.AutoCAD.Runtime;
using ElectronTransferDal.Cad;
using ElectronTransferDal.Common;
using ElectronTransferDal.XmlDal;
using ElectronTransferFramework;
using ElectronTransferModel.Config;
using System.IO;
using Exception = System.Exception;
using Timer = System.Windows.Forms.Timer;

namespace ElectronTransferView
{
    public class CADTimer
    {
        /// <summary>
        /// 定时器
        /// </summary>
        private Timer timer = new Timer();
        /// <summary>
        /// 保存备份文件路径
        /// </summary>
        private List<string> backPaths= new List<string>();
        /// <summary>
        /// 自动保存
        /// </summary>
        [CommandMethod("autosave")]
        public void StartTimer()
        {
            timer.Interval = GetInterval();
            timer.Enabled = true;
            timer.Start();
            timer.Tick += timer_Tick;
        }

        void timer_Tick(object sender, EventArgs e)
        {
            try
            {
                if (!string.IsNullOrEmpty(MapConfig.Instance.ClientXmlPath))
                {
                    const string folder = "XmlBackups";
                    var clientXmlPath = Path.GetDirectoryName(MapConfig.Instance.ClientXmlPath);
                    var path = Path.Combine(clientXmlPath, folder);
                    //判断是否存在
                    if (!Directory.Exists(path))
                        Directory.CreateDirectory(path);

                    var fileName = GetFileName();
                    var filePath = Path.Combine(path, fileName);

                    SaveXml(filePath);
                }
            }catch(Exception ex)
            {
                LogManager.Instance.Error(ex);
            }
        }
        private int GetInterval()
        {
            //获取CAD自动
            var saveTime = PublicMethod.Instance.GetSystemCMD("SAVETIME");
            return int.Parse(saveTime) * 60000;
        }
        private string GetFileName()
        {
            return string.Format("{0}.xml",DateTime.Now.ToString("yyyyMMddHHmmss"));
        }
        private void SaveXml(string filePath)
        {
            PublicMethod.Instance.ShowMessage("正在备份，请稍候……");
            var result = (DBManager.Instance as XmlDBManager).Save(filePath);
            var mess = result ? "自动备份成功！" : "自动备份失败！";
            PublicMethod.Instance.ShowMessage(string.Format("{0}\n{1}",mess,filePath));
            backPaths.Add(filePath);
            //备份成功才删掉上一版本
            DeleteFile(result,filePath);
            //设置保存间隔时间
            timer.Interval = GetInterval();
        }

        /// <summary>
        /// 删除文件
        /// </summary>
        /// <param name="result"></param>
        /// <param name="filePath"> </param>
        private void DeleteFile(bool result,string filePath)
        {
            if (result)
            {
                if (backPaths.Count > 1)
                {
                    var fileInfo = new FileInfo(filePath);
                    if (fileInfo.Length > 0)
                    {
                        var path = backPaths[0];
                        File.Delete(path);
                        backPaths.RemoveAt(0);
                    }
                }
            }
        }
        /// <summary>
        /// 停止自动保存
        /// </summary>
        [CommandMethod("stopsave")]
        public void StopTimer()
        {
            timer.Stop();
        }
    }
}
