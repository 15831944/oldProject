using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Configuration;
using System.Text;
using CYZFramework.DB;
using CYZFramework.Log;
using Ionic.Zip;
using ElectronTransferServiceDll;
using System.IO;
using System.Net;

namespace ElectronTransferService.dll
{
    public class TZInterface
    {
        /// <summary>
        /// 下载台账数据
        /// </summary>
        /// <param name="GIS_ID">GIS在线工单ID</param>
        /// <param name="tempTableName">中间表名</param>
        /// <param name="downDataFilePath">存放*db文件路径</param>
        /// <returns></returns>
        public static string DownTZData(string GIS_ID, string tempTableName, string downDataFilePath)
        {
            string ss = "";
            try
            {   
                CYZLog.writeLog("start to 下载台账数据 DownTZData('" + GIS_ID + "','" + tempTableName + "','" + downDataFilePath + "')");
                string TZUrl = ConfigurationManager.AppSettings["DownTZUrl"];
                string TZServerPath = ConfigurationManager.AppSettings["TZServerPath"];
                string DownStr = string.Format(TZUrl, TZServerPath, GIS_ID, tempTableName, downDataFilePath);

                CYZLog.writeLog("DownStr='" + DownStr + "'");

                WebClient wc = new WebClient();
                
                wc.DownloadDataCompleted+=wc_DownloadDataCompleted;
                wc.BaseAddress = DownStr;
                wc.DownloadDataAsync(new Uri(DownStr));

            }
            catch (Exception ex)
            {
                ss = ex.Message;
                CYZLog.writeLog(ss);
            }
            return ss;
        }
        /// <summary>
        /// 台帐下载
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private static void wc_DownloadDataCompleted(object sender, DownloadDataCompletedEventArgs e) 
        {
            try
            {
                //台帐返回信息
                var infoString = Encoding.UTF8.GetString(e.Result);
                CYZLog.writeLog(infoString);
                //获取台帐包路径
                var tzPacketPath = GetTzPacketPath(infoString);

                string tableName = "";
                string downDataFilePath = "";
                WebClient wc = (WebClient)sender;
                if (wc != null)
                {
                    CYZLog.writeLog(wc.BaseAddress);
                    string downurl = wc.BaseAddress;
                    downurl = downurl.Replace("<M>", "%");
                    downurl = HttpUtility.UrlDecode(downurl, Encoding.UTF8);

                    CYZLog.writeLog(downurl);
                    string paramsStr = downurl.Split('?')[1];
                    foreach (string str in paramsStr.Split('&'))
                    {
                        if (str.IndexOf("tempTableName") >= 0)
                        {
                            tableName = str.Split('=')[1];
                        }
                        if (str.IndexOf("downDataFilePath") >= 0)
                        {
                            downDataFilePath = str.Split('=')[1];
                        }
                    }
                }
                if (!string.IsNullOrEmpty(tableName))
                {
                    CYZLog.writeLog("删除台账临时表:" + tableName);
                   // CDBManger dbm = new CDBManger();
                    //StringBuilder sqlstr = new StringBuilder("drop table " + tableName);
                    //删除表
                   // dbm.ExecuteNonQuery(sqlstr.ToString());
                    //if (dbm != null) { dbm.close(); }
                }
                if (!string.IsNullOrEmpty(downDataFilePath))
                {
                    //downDataFilePath="ftp://administrator:abc_123@192.168.1.101/cadftp/Export/10kV北氨线511_0389495A/";                    
                    downDataFilePath = downDataFilePath.Split('/')[downDataFilePath.Split('/').Length - 2];
                    string FtpLj = ConfigurationManager.AppSettings["datapackagepath"] + downDataFilePath ;
                    string session_id = downDataFilePath.Split('_')[1];
                    string dstate = PublicMethod.read_state(ConfigurationManager.AppSettings["datapackagepath"], session_id);

                    if (!(infoString.IndexOf("'status':true") >= 0))
                    {
                        using (FileStream fs = new FileStream(FtpLj + "\\no_emmis.db", FileMode.OpenOrCreate, FileAccess.ReadWrite))
                        {
                            fs.Close();
                        }

                        PublicMethod.write_state(FtpLj + "\\", -1.01);
                    }
                    else
                    {
                       // bool bz = false;
                        TZDown(FtpLj + "\\emmis.zip", tzPacketPath, FtpLj);
                        //PublicMethod.write_state(FtpLj + "\\", 1.0);
                    }
                    if (dstate.IndexOf("正在等待台账...") >= 0)
                    {
                        using (ZipFile zf = new ZipFile(System.Text.Encoding.Default))
                        {
                            zf.AddDirectory(FtpLj);
                            //压缩之后保存路径及压缩文件名
                            zf.Save(FtpLj + ".zip");
                        }

                        PublicMethod.write_state(FtpLj + "\\", 1.0);
                    }
                }

            }
            catch (Exception ex)
            {
                CYZLog.writeLog(ex.ToString());
               // PublicMethod.write_state(FtpLj + "\\", -1.01);
            }
            finally
            {
               
            }

        }
        /// <summary>
        /// 上传台账数据
        /// </summary>
        /// <param name="GIS_ID">GIS在线工单ID</param>
        /// <param name="UpLoadDataFilePath">*db文件上传路径</param>
        /// <returns></returns>
        //[WebMethod(Description = "上传台账设备数据")]
        public static string UpLoadTZData(string GIS_ID, string UpLoadDataFilePath)
        {
            string ss = "";
            try
            {
                CYZFramework.Log.CYZLog.writeLog("start to 上传台账数据 UpLoadTZData('" + GIS_ID + "','" + UpLoadDataFilePath + "')");
                string TZUrl = ConfigurationManager.AppSettings["UpTZUrl"].ToString();
                string TZServerPath = ConfigurationManager.AppSettings["TZServerPath"].ToString();
                string UpStr = string.Format(TZUrl, TZServerPath, GIS_ID, UpLoadDataFilePath);

                CYZFramework.Log.CYZLog.writeLog("UpStr='" + UpStr + "'");

                System.Net.WebClient wc = new System.Net.WebClient();
                //byte[] redata = wc.DownloadData(UpStr);
                //ss = Encoding.UTF8.GetString(redata);
                wc.DownloadDataCompleted += new System.Net.DownloadDataCompletedEventHandler(wc_DownloadDataCompleted2);
                wc.BaseAddress = UpStr;
                wc.DownloadDataAsync(new Uri(UpStr));
            }
            catch (System.Exception ex)
            {
                ss = ex.Message;
                CYZFramework.Log.CYZLog.writeLog(ss);
            }
            return ss;

        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private static void wc_DownloadDataCompleted2(object sender, System.Net.DownloadDataCompletedEventArgs e)
        {
        }

        /// <summary>
        /// 从台账服务器拿db文件http的方式
        /// </summary>
        /// <param name="infoString"></param>
        /// <returns></returns>

        public static bool TZDown(string strFileName, string TZdz, string ftplj)
        {
            //将文件名改一下
            strFileName = strFileName + ".wait";

            bool flag = false;
            //打开上次下载的文件
            long SPosition = 0;
            //实例化流对象
            FileStream FStream;
            //判断要下载的文件夹是否存在
            if (File.Exists(strFileName))
            {
                //打开要下载的文件
                FStream = File.OpenWrite(strFileName);
                //获取已经下载的长度
                SPosition = FStream.Length;
                FStream.Seek(SPosition, SeekOrigin.Current);
            }
            else
            {
                //文件不保存创建一个文件
                FStream = new FileStream(strFileName, FileMode.Create);
                SPosition = 0;
            }
            try
            {
                //打开网络连接
                HttpWebRequest myRequest = (HttpWebRequest)HttpWebRequest.Create(TZdz);
                if (SPosition > 0)
                    myRequest.AddRange((int)SPosition);             //设置Range值
                //向服务器请求,获得服务器的回应数据流
                Stream myStream = myRequest.GetResponse().GetResponseStream();
                //定义一个字节数据
                byte[] btContent = new byte[1024];
                int intSize = 0;
                intSize = myStream.Read(btContent, 0, 1024);
                while (intSize > 0)
                {
                    FStream.Write(btContent, 0, intSize);
                    intSize = myStream.Read(btContent, 0, 1024);
                }
                FStream.Flush();
                //关闭流
                FStream.Close();
                myStream.Close();

                //将文件名改回来
                string tempName = strFileName;
                strFileName = strFileName.Substring(0, strFileName.Length - 5);
                File.Move(tempName, strFileName);
                flag = true;        //返回true下载成功
            }
            catch (Exception ex)
            {
                PublicMethod.write_state(ftplj + "\\", -1.01);
                CYZFramework.Log.CYZLog.writeLog(ex.ToString());

                FStream.Close();
                flag = false;       //返回false下载失败
            }
            return flag;
        }

        /// <summary>
        /// 截取台帐包下载路径
        /// </summary>
        /// <param name="infoString"></param>
        /// <returns></returns>
        private static string GetTzPacketPath(string infoString)
        {
            var tzPacketPath = string.Empty;
            try
            {
                if (!string.IsNullOrEmpty(infoString))
                {
                    if (infoString.Contains("true"))
                    {
                        //{'status':true,'errorInfo':'Http://10.128.6.117:9230/emmis/downloadDBFile.gis?projectName=白沙10kV沙岭线F723_wzj888'}
                        var index = infoString.IndexOf("errorInfo':'");
                        var pwscemmisPath = infoString.Substring(index, infoString.Length - index - 2);
                        index = pwscemmisPath.IndexOf(':');
                        pwscemmisPath = pwscemmisPath.Substring(index + 2, pwscemmisPath.Length - (index + 2));
                        tzPacketPath = pwscemmisPath.Replace("\\", @"\");
                    }
                }
            }
            catch (Exception)
            {
                CYZLog.writeLog("获取台帐包路径失败！");
            }
            return tzPacketPath;
        }
    }
}
