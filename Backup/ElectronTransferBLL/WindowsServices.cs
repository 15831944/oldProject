using System;
using System.IO;
using System.ServiceProcess;
using System.Text;
using System.Threading;
using ElectronTransferFramework;
using ElectronTransferModel.Config;
using System.Windows.Forms;

namespace ElectronTransferBll
{
    public class WindowsServices
    {
        /// <summary>
        /// 台帐服务地址
        /// </summary>
        private static string TzServicePath { set; get; }
        /// <summary>
        /// 拷贝台帐数据包
        /// </summary>
        /// <param name="sourcePath">源路径</param>
        /// <returns>返回台帐增量包路径</returns>
        public static void CopyTZPacket(object sourcePath)
        {
            try
            {
                sourcePath = GetDirectoryName(sourcePath.ToString());
                if (!Directory.Exists(sourcePath.ToString())) return;
                //源路径
                var tzPacketSourcePath = Path.Combine(sourcePath.ToString(), "emmis.zip"); 
                if (!File.Exists(tzPacketSourcePath))
                {
                    MessageBox.Show("台帐数据包不存在！\n如不需要编辑原始台帐，则不会影响新增台帐，反之请联系运维人员！", "温馨提示", MessageBoxButtons.OK, MessageBoxIcon.Question);
                    return;
                }
                //从注册表中读取台帐服务地址
                TzServicePath = MapConfig.Instance.TZPacketPath; //GetTzServicePath();
                if (!Directory.Exists(TzServicePath))
                {
                    MessageBox.Show("台帐服务地址不存在，请联系运维人员！", "温馨提示", MessageBoxButtons.OK, MessageBoxIcon.Question);
                    return;
                }
                //目标路径
                var tzPackettargetPath = Path.Combine(TzServicePath, "emmis.zip");

                //再拷贝台帐包
                File.Copy(tzPacketSourcePath, tzPackettargetPath, true);
                //tzTargetPath = tzPackettargetPath;
                //合并台帐
                MergerTZDB();
            }
            catch (Exception ex)
            {
                LogManager.Instance.Error(ex);
            }
        }
        /// <summary>
        /// 截取文件夹路径
        /// </summary>
        /// <param name="fileName"></param>
        /// <returns></returns>
        private static string GetDirectoryName(string fileName)
        {
            var index=fileName.LastIndexOf('\\');
            return fileName.Substring(0, index);
        }

        /// <summary>
        /// 合并台帐DB文件
        /// </summary>
        private static void MergerTZDB()
        {
            try
            {
                const string url = "http://localhost:9090/emmis/equipGisMappingTemp/cadRestful/mergerDB.gis";
                var webClient = new System.Net.WebClient();
                var data = webClient.DownloadData(url);
                var value=Encoding.UTF8.GetString(data);
                if (value.Contains("true"))
                {
                    LogManager.Instance.Info("台帐DB合并成功！");
                }
                else
                {
                    MessageBox.Show("台帐DB合并失败！请联系运维人员！\n" + value, "温馨提示", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }catch(Exception ex)
            {
                MessageBox.Show("台帐DB合并失败！请联运维人员！\n" + ex.Message, "温馨提示", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }
        /// <summary>
        /// 获取台帐服务地址
        /// </summary>
        /// <returns></returns>
        private static string GetTzServicePath()
        {
            const string keyName = "EMMIS_PATH";
            const string emmisPath = @"SYSTEM\ControlSet001\Control\Session Manager\Environment";
            var value = Register.GetKeyValue(emmisPath, keyName);
            var index = value.LastIndexOf(':');
            index--;
            return value.Substring(index, value.Length - index);
        }
        #region 启动Tomcat
        /// <summary>
        /// 启动Tomcat服务
        /// </summary>
        public static void TomcatServiceStart()
        {
            var th = new Thread(ApacheTomcatStart);
            th.Start();
        }
        /// <summary>
        /// 启动ApacheTomcat
        /// </summary>
        private static void ApacheTomcatStart()
        {
            try
            {
                var service = new ServiceController();
                //启动Tomcat服务
                service.ServiceName = "ApacheTomcat";
                if (service.Status == ServiceControllerStatus.Stopped)
                {
                    service.Start();
                    service.WaitForStatus(ServiceControllerStatus.Running);
                }
            }
            catch (Exception ex)
            {
                LogManager.Instance.Error("ApacheTomcat服务启动失败!\n" + ex);        
            }
        }

        #endregion


        /// <summary>
        /// 导出增量包
        /// </summary>
        /// <param name="filePath"></param>
        /// <param name="fileName">名称</param>
        public static void ExportIncrementPacket(string filePath,string fileName)
        {
            try
            {
                //保存的根目录
                var path = Path.GetDirectoryName(filePath);
                var value = GetExportZip(fileName);
                if (!string.IsNullOrEmpty(value))
                {
                    if (value.Contains("true"))
                    {
                        //{'status':true,'errorInfo':'D:\pwsc_emmis\export\485390_485390_emmis_23111434.zip'}
                        var index = value.IndexOf("errorInfo':'");
                        var pwscemmisPath = value.Substring(index, value.Length - index - 2);
                        index = pwscemmisPath.IndexOf(':');
                        pwscemmisPath = pwscemmisPath.Substring(index + 2, pwscemmisPath.Length - (index + 2));
                        pwscemmisPath = pwscemmisPath.Replace("\\", @"\");
                        if (File.Exists(pwscemmisPath))
                        {
                            var tzFileName = Path.GetFileName(pwscemmisPath);
                            var tzTargetPath = Path.Combine(path, tzFileName);
                            File.Copy(pwscemmisPath, tzTargetPath, true);
                        }
                    }
                }


                //var files = new List<string>();
                ////添加gis增量包
                //files.Add(Path.GetFileName(filePath));
                ////添加台帐增量包
                //if (!string.IsNullOrEmpty(tzTargetPath))
                //    files.Add(Path.GetFileName(tzTargetPath));
                //var xmlSoucreFile = MapConfig.Instance.ClientXmlPath;
                //if (!string.IsNullOrEmpty(xmlSoucreFile))
                //    files.Add(Path.GetFileName(xmlSoucreFile));

                //var zipPath = Path.ChangeExtension(filePath, "zip");
                ////创建压缩包
                //ZipHelper.CreateZip(files, zipPath);
            }
            catch (Exception ex)
            {
                MessageBox.Show("导出台帐增量包失败！请联系运维人员！\n" + ex.Message, "温馨提示", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }
        private static string GetExportZip(string fileName)
        {
            try
            {
                var lttid = MapConfig.Instance.LTTID;
                var bit = Encoding.UTF8.GetBytes(fileName);
                var projectName = BitConverter.ToString(bit).Replace("-", "<M>");
                
                var url =string.Format("http://localhost:9090/emmis/equipGisMappingTemp/cadRestful/exportZip.gis?gisJobId={0}&projectName={1}",
                        lttid, projectName);
                var webClient = new System.Net.WebClient();
                var data = webClient.DownloadData(url);
                return Encoding.UTF8.GetString(data);
            }
            catch (Exception ex)
            {
                MessageBox.Show("调用台帐接口失败！请联系运维人员！\n" + ex.Message, "温馨提示", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
            return null;
        }
    }
}
