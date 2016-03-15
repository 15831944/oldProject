using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Services;
using System.IO;
using System.Text;
using ElectronTransferDal.Common;
using ElectronTransferDal.OracleDal;
using ElectronTransferDal.XmlDal;
using ElectronTransferModel.V9_4;
using Oracle.DataAccess.Client;
using ElectronTransferFramework;
using System.Reflection;
using ElectronTransferModel.Config;
using ElectronTransferFramework.Serialize;
using Ionic.Zip;
using ElectronTransferDal;
using ElectronTransferModel.Base;
using ElectronTransferModel;
using ElectronTransferDal.Common.Exceptions;
using System.Diagnostics;
using ElectronTransferServiceDll;
using System.Data;
using System.Threading;
using CYZFramework.Log;
using CYZFramework.DB;
using ElectronTransferService.dll;
//using System.Data.OracleClient;


namespace ElectronTransferService
{
    /// <summary>
    /// ElectronTransferService 的摘要说明
    /// </summary>
    [WebService(Namespace = "http://tempuri.org/")]
    [WebServiceBinding(ConformsTo = WsiProfiles.BasicProfile1_1)]
    [System.ComponentModel.ToolboxItem(false)]
    // 若要允许使用 ASP.NET AJAX 从脚本中调用此 Web 服务，请取消对下行的注释。
    // [System.Web.Script.Services.ScriptService]
    public class ElectronTransferService : System.Web.Services.WebService
    {
        /// <summary>
        /// 按工程单号撤销工单数据
        /// </summary>
        /// <param name="GC_ID">关联工程工单的id</param>
        /// <returns></returns>
        [WebMethod]
        public string DisableTicket1(string GC_ID)
        {
            string ss = "";
            try
            {
                CYZLog.writeLog("DisableTicket1(" + GC_ID + ")");

                if (TickHelper.qxtick1(GC_ID))
                {
                    ss = "TRUE";
                    CYZLog.writeLog("撤销成功： " + DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss"));
                }
            }
            catch (Exception ex)
            {
                CYZLog.writeLog(ex.ToString());
                ss = "ERROR:" + ex.ToString();
            }

            CYZLog.writeLog("End DisableTicket; return " + ss);
            return ss;
        }
        /// <summary>
        /// 按fno，fid撤销工单数据
        /// </summary>
        /// <param name="FNOFIDS">需要检查的设备fno,fid组。如:{"148,123456","140,654321","156,789546"}</param>
        /// <returns></returns>
        [WebMethod]
        public string DisableTicket2(List<string> FNOFIDS)
        {
            string ss = "";
            try
            {
                foreach (string s in FNOFIDS)
                {
                    ss += s + ",";
                }
                if (ss != "")
                {
                    ss = ss.Substring(0, ss.Length - 1);
                }
                CYZLog.writeLog("DisableTicket2(" + ss + ")");

                if (TickHelper.qxtick2(FNOFIDS))
                    ss = "TRUE";
            }
            catch (Exception ex)
            {
                CYZLog.writeLog(ex.ToString());
                ss = "ERROR:" + ex.ToString();
            }

            CYZLog.writeLog("End DisableTicket2 (" + ss + ")");
            return ss;
        }

        /// <summary>
        /// 按fno，fid撤销工单数据,不是本工单的设备不得撤销
        /// </summary>
        /// <param name="FNOFIDS">需要解锁的设备fno,fid组。如:{"148,123456","140,654321","156,789546"}</param>
        /// <param name="gc_id"> </param>
        /// <returns></returns>
        [WebMethod]
        public List<string> DisableTicket3(List<string> FNOFIDS, string gc_id)
        {  //List<string> FNOFIDS
            //List<string> FNOFIDS = new List<string>();

            List<string> FildFNOFIDS = new List<string>();
            string ss = "";
            try
            {
                foreach (string s in FNOFIDS)
                {
                    ss += s + ",";
                }
                if (ss != "")
                {
                    ss = ss.Substring(0, ss.Length - 1);
                }
                CYZLog.writeLog("DisableTicket3(" + ss + ")");
                LockKgg(ref FNOFIDS);
                FildFNOFIDS = TickHelper.qxtick3(FNOFIDS, gc_id);
            }
            catch (Exception ex)
            {
                CYZLog.writeLog(ex.ToString());
                ss = "ERROR:" + ex.ToString();
            }

            CYZLog.writeLog("End DisableTicket3()");
            return FildFNOFIDS;
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="GC_FNOFIDS">需要检查的设备fno,fid组。如:{"148,123456","140,654321","156,789546"}</param>
        /// <returns></returns>
        [WebMethod]
        public string CheckTickes(List<string> GC_FNOFIDS)
        {
            string ss = "";
            try
            {
                foreach (string s in GC_FNOFIDS)
                {
                    ss += s + ",";
                }
                if (ss != "")
                {
                    ss = ss.Substring(0, ss.Length - 1);
                }
                CYZLog.writeLog("CheckTickes(" + ss + ")");

                //TickHelper.checkSpatial(GC_FNOFIDS);
                ss = "TRUE";
            }
            catch (Exception ex)
            {
                CYZLog.writeLog(ex.ToString());
                ss = "ERROR:" + ex.ToString();
            }

            CYZLog.writeLog("End CheckTickes");
            return ss;
        }
        /// <summary>
        /// 开启按fid开工单锁定设备的任务。需要传入一组设备的fid、工程工单的责任人和关联工程工单的id
        /// </summary>
        /// <param name="GC_FNOFIDS">需要开工单锁定的设备fno,fid组。如:{"148,123456","140,654321","156,789546"}</param>
        /// <param name="GC_UserName">关联工程工单的责任人名称</param>
        /// <param name="GC_ID">关联工程工单的id</param>
        /// <returns>
        /// 开工单成功，则返回gis工单id。
        /// 失败，则返回具体的错误信息
        /// </returns>
        [WebMethod]
        public string OpenTicket(List<string> GC_FNOFIDS, string GC_UserName, string GC_ID)
        {
            string ss = "";
            try
            {
                foreach (string s in GC_FNOFIDS)
                {
                    ss += s + ",";
                }
                if (ss != "")
                {
                    ss = ss.Substring(0, ss.Length - 1);
                }
                CYZLog.writeLog("OpenTicket(" + ss + "," + GC_UserName + "," + GC_ID + ")");

                ss = TickHelper.openticks(GC_FNOFIDS, GC_UserName, GC_ID, tickerrh * 100000 + (tickerre++));

            }
            catch (Exception ex)
            {
                CYZLog.writeLog(ex.ToString());
                ss = "ERROR:" + ex.ToString();
            }

            CYZLog.writeLog("End OpenTicket");
            return ss;
        }
        static int tickerrh = int.Parse(DateTime.Today.ToString("MMdd"));
        static int tickerre = 1;


        /// <summary>
        /// 开启按fid开工单锁定设备的任务。需要传入一组设备的fid、工程工单的责任人和关联工程工单的id
        /// </summary>
        /// <param name="GC_FNOFIDS">需要开工单锁定的设备fno,fid组。如:{"148,123456","140,654321","156,789546"}</param>
        /// <param name="GC_ID">关联工程工单的id</param>
        /// <param name="ltt_name"> </param>
        /// <returns>
        /// 开工单成功，则返回gis工单id。
        /// 失败，则返回具体的错误信息
        /// </returns>
        [WebMethod]
        public string LockGisSb(List<string> GC_FNOFIDS, string GC_ID, string ltt_name)
        {
            string ss = "";
            try
            {
                foreach (string s in GC_FNOFIDS)
                {
                    ss += s + ",";
                }
                if (ss != "")
                {
                    ss = ss.Substring(0, ss.Length - 1);
                }
                CYZLog.writeLog("LockGisSb(" + ss + "," + GC_ID + ")");
                LockKgg(ref GC_FNOFIDS);//锁定依附与房之类的开关柜
                LockHb(ref GC_FNOFIDS);//锁定依附与集抄得户表
                ss = TickHelper.LockSb(GC_FNOFIDS, ltt_name, GC_ID, tickerrh * 100000 + (tickerre++));
               // SxSj(ltt_name);


            }
            catch (Exception ex)
            {
                CYZLog.writeLog(ex.ToString());
                ss = "ERROR:" + ex.ToString();
            }

            CYZLog.writeLog("End LockGisSb");
            return ss;
        }

        /// <summary>
        /// 主要锁定依附与房的开关柜
        /// </summary>
        /// <param name="GC_FNOFIDS">需要开工单锁定的设备fno,fid组。如:{"148,123456","140,654321","156,789546"}</param>
        public void LockKgg(ref List<string> GC_FNOFIDS)
        {
            string ss = "";
            CDBManger dbm = null;
            StringBuilder sb = new StringBuilder("");
            OracleDataReader Oredder = null;
            try
            {
                dbm = new CDBManger();
                foreach (string s in GC_FNOFIDS)
                {
                    ss += s.Split(',')[1] + ",";
                }
                if (ss != "")
                {
                    ss = ss.Substring(0, ss.Length - 1);
                    sb.Append("select g3e_fno,g3e_fid from b$Gg_pd_kgg_n where g3e_fid in( ");
                    sb.Append(" select g3e_fid from b$common_n  where g3e_fno=198 and owner1_id in( ");
                    sb.Append(" select  g3e_id from b$common_n where g3e_fid in (" + ss + ")))");
                    Oredder = dbm.ExecuteReader(sb.ToString());
                    while (Oredder.Read())
                    {
                        string Fnofid = Oredder["g3e_fno"].ToString();
                        Fnofid += "," + Oredder["g3e_fid"];
                        GC_FNOFIDS.Add(Fnofid);
                        Fnofid = "";

                    }
                }

            }
            catch (Exception ex)
            {
                CYZLog.writeLog(ex.ToString());
            }
            finally
            {
                CloseOracleDataReader(Oredder, dbm);
            }
        }


        /// <summary>
        /// 主要锁定依附与集抄的户表
        /// </summary>
        public void LockHb(ref List<string> GC_FNOFIDS)
        {
            string ss = "";
            CDBManger dbm = null;
            StringBuilder sb = new StringBuilder("");
            OracleDataReader Oredder = null;
            try
            {
                dbm = new CDBManger();
                foreach (string s in GC_FNOFIDS)
                {
                    ss += s.Split(',')[1] + ",";
                }
                if (ss != "")
                {
                    ss = ss.Substring(0, ss.Length - 1);
                    sb.Append("select * from b$gg_jx_shbd_pt t where t.g3e_detailid in(  ");
                    sb.Append(" select a.g3e_detailid from b$detailreference_n a where a.g3e_fid in (" + ss + "))");
                    Oredder = dbm.ExecuteReader(sb.ToString());
                    while (Oredder.Read())
                    {
                        string Fnofid = Oredder["g3e_fno"].ToString();
                        Fnofid += "," + Oredder["g3e_fid"];
                        GC_FNOFIDS.Add(Fnofid);
                        Fnofid = "";

                    }
                }

            }
            catch (Exception ex)
            {
                CYZLog.writeLog(ex.ToString());
            }
            finally
            {
                CloseOracleDataReader(Oredder, dbm);
            }
        }
 [WebMethod]
 public string PublishAppend(string GC_ID)
 {
     string ss = "TRUE";
     try
     {
         CYZLog.writeLog("PublishAppend(" + GC_ID + ")");
         var userName = string.Empty;
         var oldVersion = string.Empty;
         string gistick = string.Empty;
         string smzq = Smzq(GC_ID, ref gistick);
         if (smzq == "")
         {
             return "工单不存在。";
         }
         ss = TickHelper.posttick(GC_ID);
     }
     catch (Exception ex)
     {
         CYZLog.writeLog(ex.ToString());
         ss = "ERROR:" + ex.ToString();
     }

     CYZLog.writeLog("End PublishAppend");
     return ss;
 }
        /// <summary>
        /// 提交增量数据。需要传入工程工单id和增量数据包的ftp路径。
        /// </summary>
        /// <param name="AppendFile">增量数据包的ftp文件路径</param>
        /// <param name="GC_ID">工程工单的id</param>
        /// <returns>
        /// 返回“TRUE”表示提交成功。
        /// 否则就是具体的错误信息。 
        /// </returns>
        [WebMethod]
        public string UpLoadAppend(string AppendFile, string GC_ID)
        {
            string ss = "TRUE";
            int SjBVerion = 0;
            string smzq = "";
            bool bbreval = false;
            string UserName = "";
            string PassWord = "";
            string QjPz = "";
            CDBManger cdm = null;
            try
            {
                DateTime appenshiptime = DateTime.Now;

                CYZLog.writeLog("UpLoadAppend(" + AppendFile + "," + GC_ID + ")");



                string gistick = "";
                smzq = Smzq(GC_ID, ref gistick);

                if (smzq == "")
                {
                    return "工单不存在。";
                }
                bbreval = false;

                var xmldb = new XmlDBManager();
                xmldb.FileName = Application["dataappendpath"] + GC_ID + "\\" + AppendFile.Substring(AppendFile.LastIndexOf("/") + 1, AppendFile.Length - AppendFile.LastIndexOf("/") - 1);
                xmldb.Initialize();

                if (xmldb.Count() <= 0) {
                    
                    CYZLog.writeLog("数据格式不正确");
                    return "数据格式不正确请重新导出";
                }
               
                int ZlVerion = -1;
                var CadV = xmldb.GetEntity<CadVersion>(o => o.G3E_FID == 1);  //这个版本号是从增量文件得到的
           
                ZlVerion = CadV.Version;

                //对比tickship里面的版本号
                cdm = new CDBManger();
                GetUserNamePwd(GC_ID, ref UserName, ref PassWord, ref QjPz);
                if (string.IsNullOrEmpty(UserName) || string.IsNullOrEmpty(PassWord) || string.IsNullOrEmpty(QjPz))
                {
                    CYZLog.writeLog("账号或密码不存在");
                    return "账号或密码不存在";
                }
          
                var oracledb = new OracleDBManager { UserName = UserName, Password = PassWord };
                //var oracledb = new OracleDBManager();

                string str = "select VERSION from cadgis.appendship t  where t.gcticket_id='" + GC_ID + "'";
                DataTable tab = cdm.ExecuteTable(str);
                if (tab != null && tab.Rows.Count > 0)
                {
                    SjBVerion = int.Parse(tab.Rows[0][0].ToString());
                    if (ZlVerion==1||SjBVerion == 1)//数据库的版本等于1是说明这个数据从来没有到过直接按原来的导入即使是导过一次版本都等于1也是要撤销重新导入的
                    {
                        if (!TickHelper.qxtick1(GC_ID, UserName, PassWord,QjPz)) { return "FALSE"; }
                        OnceImport(oracledb, xmldb, GC_ID, smzq, appenshiptime,QjPz);
                      
                    }
                    else
                    {
                        if (ZlVerion <= SjBVerion)
                        {

                            if (!TickHelper.qxtick1(GC_ID, UserName, PassWord, QjPz)) { return "FALSE"; }//发现增量文件版本低与tickship里面的版本号,就撤销工单.
                            OnceImport(oracledb, xmldb, GC_ID, smzq, appenshiptime,QjPz);
                            
                        }
                        else//如果要导入的版本高于数据库的版本执行下面的导入
                        {
                            TwoImport(oracledb, xmldb, GC_ID, smzq, appenshiptime,QjPz);
                           
                        }
                    }

                }
                else
                {

                    if (!TickHelper.qxtick1(GC_ID, UserName, PassWord, QjPz)) { return "FALSE"; }
                    OnceImport(oracledb, xmldb, GC_ID, smzq, appenshiptime,QjPz);

                }
                if (cdm != null) { cdm.close(); }
                //增量的增删改       

                //更新公共属性表里面的从属关系
                UploadEventHandler.COMMON_N(oracledb, xmldb, GC_ID, smzq, appenshiptime,QjPz);
                CYZLog.writeLog("End 更新公共属性表里面的从属关系");
                UploadEventHandler.GG_PD_GNWZMC_N(oracledb, xmldb, GC_ID, smzq, appenshiptime, QjPz);
                CYZLog.writeLog("End 更新功能位置表");
                //更新计量表坐标的
                UploadEventHandler.UploadGg_jx_jlb_pt_sdogeom(oracledb, xmldb, GC_ID, smzq, appenshiptime, QjPz);
                CYZLog.writeLog("End 更新计量表坐标的");
                //更新户表坐标的
                UploadEventHandler.UploadGg_jx_shbd_pt_sdogeom(oracledb, xmldb, GC_ID, smzq, appenshiptime, QjPz);
                CYZLog.writeLog("End 更新户表坐标的");
               // UploadEventHandler.UploadGg_gl_zxbz_n(oracledb, xmldb, GC_ID, smzq, appenshiptime, QjPz);
                CYZLog.writeLog("End 更新杂项标注");
                UploadEventHandler.UpLoadGg_gz_tj_n(oracledb, xmldb, GC_ID, smzq, appenshiptime, QjPz);
                CYZLog.writeLog("End 更新杂项标注");
                UploadEventHandler.UpLoadGg_pd_cbx_n(oracledb, xmldb, GC_ID, smzq, appenshiptime, QjPz);
                CYZLog.writeLog("End 更新集抄所属变压器");
                UploadEventHandler.UpGg_pd_dykg_n(oracledb, xmldb, GC_ID, smzq, appenshiptime, QjPz);
                CYZLog.writeLog("End 更新低压开关所属变压器");
                UploadEventHandler.UpGg_jc_pwy_n(oracledb, xmldb, GC_ID, smzq, appenshiptime, QjPz);
                CYZLog.writeLog("End 更新配网仪所属变压器");
                //修改所有新增修改删除数据的版本
                cdm = new CDBManger();
                string strSql = "update cadgis.ticketship set version=" + ZlVerion + " where gcticket_id='" + GC_ID + "'";
                string strSql2 = "update cadgis.appendship  set version=" + ZlVerion + " where gcticket_id='" + GC_ID + "'";
                string StrSql3 = "update cadgis.appendfidship  set version=" + ZlVerion + " where gcticket_id='" + GC_ID + "'";
              
                cdm.ExecuteNonQuery(strSql);
                cdm.ExecuteNonQuery(strSql2);
                cdm.ExecuteNonQuery(StrSql3);
               
                TickHelper.SxSj(smzq, QjPz,UserName,PassWord);
                string tzdz = Application["dataappendpath"] + GC_ID;
                string filename=null;
                 GetAllDbFiles(tzdz,ref filename);
                //上传生产的增量
                TZInterface.UpLoadTZData(gistick.ToString(),
                    System.Web.HttpUtility.UrlEncode(Application["dataftppath"].ToString().Replace("export", "import") + GC_ID + "/" + filename, Encoding.UTF8).Replace("%", "<M>")
                    );
            }
            catch (Exception ex)
            {
                bbreval = false;
                ss = ex.ToString();
                CYZLog.writeLog(ex.ToString());
            }
            finally
            {
                if (cdm != null)
                    cdm.close();
                
            }

            CYZLog.writeLog("End UpLoadAppend");
            return ss;
        }



        [WebMethod]
        public string UpLoadTz( string GC_ID)
        {
            string ss = "";
            string smzq="";
            string gistick = "";

            try
            {
             
                smzq = Smzq(GC_ID, ref gistick);
                string tzdz = Application["dataappendpath"] + GC_ID;
                string filename = null;
                GetAllDbFiles(tzdz, ref filename);

                //上传生产的增量
                TZInterface.UpLoadTZData(gistick.ToString(),
                    System.Web.HttpUtility.UrlEncode(Application["dataftppath"].ToString().Replace("export", "import") + GC_ID + "/" + filename, Encoding.UTF8).Replace("%", "<M>")
                    );
            }
            catch (Exception ex)
            {
                CYZLog.writeLog(ex.ToString());
                throw ex;
            }
            
            return ss;

      }
        /// <summary>
        /// 按馈线下载电力数据
        /// </summary>
        /// <param name="SSDW_ID">用户所属区局的编码</param>
        /// <param name="KX_ID">馈线id</param>
        /// <param name="ZYDY">中低压类型。0:中压，1：低压，2：全部</param>
        /// <param name="GC_ID">工程工单的id</param>
        /// <param name="SESSION_ID">导出任务的会话id。8位随机数（如：12345678）</param>
        /// <returns>
        /// 返回导出任务的结果。是一个数据包的ftp路径。（ftp://192.168.1.132/cyzftp/Export/aaa.cadp）
        /// 返回空值表示导出失败。
        /// </returns>
        [WebMethod(EnableSession = true)]
        public string DownByKX(string SSDW_ID, string KX_ID, string ZYDY, string GC_ID, string SESSION_ID)
        {
            string sj = DateTime.Now.ToString("MMddHHmm");
            string reval = "";
            try
            {
                string kxmc = getKXNameByKxfid(KX_ID);

                CYZLog.writeLog("DownByKX(" + SSDW_ID + "," + KX_ID + "," + ZYDY + "," + GC_ID + "," + SESSION_ID + ");");
                List<string> args = new List<string>();
                args.Add("DownByKX");
                args.Add(SSDW_ID.ToString());
                args.Add(KX_ID.ToString());
                args.Add(ZYDY.ToString());
                args.Add(GC_ID.ToString());
                args.Add(SESSION_ID.ToString());
                args.Add(kxmc);

                System.Threading.Thread tt = new System.Threading.Thread(new System.Threading.ParameterizedThreadStart(ElectronTransferServicePro.Pro));
                tt.Start(args);


                reval = kxmc + "_" + GC_ID + ".zip<R>" + Application["dataftppath"].ToString() +
                    System.Web.HttpUtility.UrlEncode(kxmc + "_" + SESSION_ID + ".zip", Encoding.GetEncoding("GB2312"));

            }
            catch (Exception ex)
            {
                CYZLog.writeLog(ex.ToString(), "");
                reval = ex.ToString();
            }
            finally
            {
                CYZLog.writeLog("DownByKX return:" + reval, "");
            }

            CYZLog.writeLog("End DownByKX");
            return reval;

        }
        /// <summary>
        /// 查询导出任务的进度。需要传入导出任务的会话id
        /// </summary>
        /// <param name="SESSION_ID">导出任务的会话id。8位随机数（如：12345678）</param>
        /// <returns>返回导出任务的进度。0.43＝43％</returns>
        [WebMethod(EnableSession = true)]
        public string CheckDownState(string SESSION_ID)
        {
            try
            {
                CYZLog.writeLog("CheckDownState(" + SESSION_ID + ")", "");
                return PublicMethod.read_state(Application["datapackagepath"].ToString(), SESSION_ID);
            }
            catch (Exception ex)
            {
                CYZLog.writeLog(ex.ToString(), "");
                return "-100";
            }
        }

        /// <summary>
        /// 开启按范围导出数据任务
        /// </summary>
        /// <param name="Xmin">左下角x坐标</param>
        /// <param name="Xmax">右上角x坐标</param>
        /// <param name="Ymin">左下角y坐标</param>
        /// <param name="Ymax">右上角Y坐标</param>
        /// <param name="ZYDY">中低压类型。0:中压，1：低压，2：全部</param>
        /// <param name="GC_ID">工程工单的id</param>
        /// <param name="SESSION_ID">导出任务的会话id。8位随机数（如：12345678）</param>
        /// <returns>
        /// 返回导出任务的结果。是一个数据包的ftp路径。（ftp://192.168.1.132/cyzftp/Export/aaa.cadp）
        /// 返回空值表示导出失败。
        /// </returns>
        [WebMethod(EnableSession = true)]
        public string DownByFW(double Xmin, double Xmax, double Ymin, double Ymax, string ZYDY, string GC_ID, string SESSION_ID, List<string> FidFnoList)
        {
            //string str = "144,115404825,146,115404715";
            string reval = "";
            try
            {
                string str = "";

                string sj = DateTime.Now.ToString("MMddHHmm");
                if (FidFnoList.Count > 0)
                {
                    foreach (var v in FidFnoList)
                    {
                        str += v + ",";
                    }

                    str = str.Substring(0, str.Length - 1);
                }

                CYZLog.writeLog("DownByFW(" + Xmin + "," + Xmax + "," + Ymin + "," + Ymax + "," + ZYDY + "," + GC_ID + "," + SESSION_ID + "," + str + ");");

                string kxmc = "";

                List<string> args = new List<string>();
                args.Add("DownByFW");
                args.Add(Xmin.ToString());
                args.Add(Xmax.ToString());
                args.Add(Ymin.ToString());
                args.Add(Ymax.ToString());
                args.Add(ZYDY.ToString());
                args.Add(GC_ID.ToString());
                args.Add(SESSION_ID.ToString());
                args.Add(kxmc);
                args.Add(sj);
                args.Add(str);


                System.Threading.Thread tt = new System.Threading.Thread(new System.Threading.ParameterizedThreadStart(ElectronTransferServicePro.Pro));
                tt.Start(args);

                reval = kxmc + "_" + SESSION_ID + ".zip<R>" + Application["dataftppath"].ToString() +
                     System.Web.HttpUtility.UrlEncode(kxmc + "_" + SESSION_ID + ".zip", Encoding.GetEncoding("GB2312"));
            }
            catch (Exception ex)
            {
                CYZLog.writeLog(ex.ToString(), "");
                reval = ex.ToString();
            }
            finally
            {
                CYZLog.writeLog("DownByFW return:" + reval, "");
            }

            CYZLog.writeLog("End DownByFW");
            return reval;
        }
        /// <summary>
        /// 按馈线下载和 馈线范围内的辅助设备
        /// </summary>
        /// <param name="SSDW_ID">用户所属区局的编码</param>
        /// <param name="KX_ID">馈线id</param>
        /// <param name="ZYDY">中低压类型。0:中压，1：低压，2：全部</param>
        /// <param name="GC_ID">工程工单的id</param>
        /// <param name="SESSION_ID">导出任务的会话id。8位随机数（如：12345678）</param>
        /// <returns>
        /// 返回导出任务的结果。是一个数据包的ftp路径。（ftp://192.168.1.132/cyzftp/Export/aaa.cadp）
        /// 返回空值表示导出失败。
        /// </returns>
        [WebMethod(EnableSession = true)]
        public string DownByKXandFW(string SSDW_ID, string KX_ID, string ZYDY, string GC_ID, string SESSION_ID, List<string> FidFnoList)//这面要新增一个新的参数 List<string> FidFnoList
        {
            string reval = "";
            try
            {
                string str = "";
                if (FidFnoList.Count > 0)
                {
                    foreach (var v in FidFnoList)
                    {
                        str += v + ",";
                    }

                    str = str.Substring(0, str.Length - 1);
                }


                string kxmc = getKXNameByKxfid(KX_ID);

                CYZLog.writeLog("DownByKXandFW(" + SSDW_ID + "," + KX_ID + "," + ZYDY + "," + GC_ID + "," + SESSION_ID + "," + str + ")", "");
                List<string> args = new List<string>();
                args.Add("DownByKXandFW");
                args.Add(SSDW_ID.ToString());
                args.Add(KX_ID.ToString());
                args.Add(ZYDY.ToString());
                args.Add(GC_ID.ToString());
                args.Add(SESSION_ID.ToString());
                args.Add(kxmc);
                args.Add(str);

                System.Threading.Thread tt = new System.Threading.Thread(new System.Threading.ParameterizedThreadStart(ElectronTransferServicePro.Pro));
                tt.Start(args);


                reval = kxmc + "_" + SESSION_ID + ".zip<R>" + Application["dataftppath"].ToString() +
                    System.Web.HttpUtility.UrlEncode(kxmc + "_" + SESSION_ID + ".zip", Encoding.GetEncoding("UTF-8"));

            }
            catch (Exception ex)
            {
                CYZLog.writeLog(ex.ToString(), "");
                reval = ex.ToString();
            }
            finally
            {
                CYZLog.writeLog("DownByKXandFW return:" + reval, "");
            }

            CYZLog.writeLog("End DownByKXandFW");
            return reval;

        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="ZYDY"></param>
        /// <param name="GC_ID"></param>
        /// <param name="SESSION_ID"></param>
        /// <param name="FidFnoList">什么用处</param>
        /// <param name="DbxList">多边形坐标</param>
        /// <returns></returns>
        [WebMethod(EnableSession = true)]
        public string DownByDBXFW(string ZYDY, string GC_ID, string SESSION_ID, List<string> FidFnoList, List<string> DbxList)
        {
            //,List<string> FidFnoList,List<string> DbxList参数

            //List<string> FidFnoList;
            //List<string> DbxList;
            //DbxList=new List<string>();
            //DbxList.Add("113.095716003866,22.611090842355022");
            //DbxList.Add("113.0956668871458,22.611009188586628");
            //DbxList.Add("113.09578163882078,22.611042576253833");

            string DbxStr = "";
            if (DbxList.Count > 0)
            {
                foreach (var v in DbxList)
                {
                    DbxStr += v + ",";
                }
            }

            DbxStr = DbxStr.Substring(0, DbxStr.Length - 1);
            int ii = 0;

            string reval = "";

            try
            {

                string str = "";
                string sj = DateTime.Now.ToString("MMddHHmm");
                if (FidFnoList.Count > 0)
                {
                    foreach (var v in FidFnoList)
                    {
                        str += v + ",";
                    }

                    str = str.Substring(0, str.Length - 1);
                }


                string kxmc = "";

                CYZLog.writeLog("DownByDBXFW(" + ZYDY + "," + GC_ID + "," + SESSION_ID + "," + str + "," + DbxStr + ")", "");
                List<string> args = new List<string>();
                args.Add("DownByDBXFW");
                args.Add(ZYDY.ToString());
                args.Add(GC_ID.ToString());
                args.Add(SESSION_ID.ToString());
                args.Add(kxmc);
                args.Add(str);
                args.Add(DbxStr);
                args.Add(sj);


                // args.Add(ii.ToString());

                ElectronTransferServicePro.dataappendpath = Application["dataappendpath"].ToString();
                ElectronTransferServicePro.datapackagepath = Application["datapackagepath"].ToString();
                ElectronTransferServicePro.dataftppath = Application["dataftppath"].ToString();
                ElectronTransferServicePro.QJConfig = Application["QJConfig"].ToString();


                System.Threading.Thread tt = new System.Threading.Thread(new System.Threading.ParameterizedThreadStart(ElectronTransferServicePro.Pro));
                tt.Start(args);


                reval = kxmc + "_" + SESSION_ID + ".zip<R>" + Application["dataftppath"].ToString() +
                    System.Web.HttpUtility.UrlEncode(kxmc + "_" + SESSION_ID + ".zip", Encoding.GetEncoding("GB2312"));

            }
            catch (Exception ex)
            {
                CYZLog.writeLog(ex.ToString(), "");
                reval = ex.ToString();
            }
            finally
            {
                CYZLog.writeLog("DownByDBXFW return:" + reval, "");
            }

            CYZLog.writeLog("End DownByDBXFW");
            return reval;

        }
        /// <summary>
        /// 查询导出任务的进度。需要传入导出任务的会话id
        /// </summary>
        /// <param name="SESSION_ID">导出任务的会话id。8位随机数（如：12345678）</param>
        /// <returns>返回导出任务的进度。0.43＝43％</returns>
        //[WebMethod(EnableSession = true)]
        //public string CheckDownState(string SESSION_ID)
        //{
        //    try
        //    {
        //        CYZLog.writeLog("CheckDownState(" + SESSION_ID + ")", "");
        //        //return PublicMethod.read_state(Application["datapackagepath"].ToString() + SESSION_ID + "\\");
        //        return PublicMethod.read_state(Application["datapackagepath"].ToString(), SESSION_ID);
        //    }
        //    catch (Exception ex)
        //    {
        //        CYZLog.writeLog(ex.ToString(), "");
        //        return "0.0";
        //    }
        //}

        [WebMethod]
        public void fireEvent(string eventname)
        {
            TickHelper.fireEventShip(eventname);
        }
        [WebMethod]
        public void endFireEvent(string eventname)
        {
            CYZFramework.Log.CYZLog.writeLog(eventname);

            //TickHelper.endFireEventShip(eventname);
            //Thread.Sleep(60 * 1000);
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="srcPath"></param>
        /// <param name="aimPath"></param>
        public void CopyDir(string srcPath, string aimPath)
        {

            try
            {

                if (aimPath[aimPath.Length - 1] != System.IO.Path.DirectorySeparatorChar)
                {
                    aimPath += System.IO.Path.DirectorySeparatorChar;
                }

                if (!System.IO.Directory.Exists(aimPath))
                {
                    System.IO.Directory.CreateDirectory(aimPath);
                }

                string[] fileList = System.IO.Directory.GetFileSystemEntries(srcPath);

                foreach (string file in fileList)
                {
                    if (System.IO.Directory.Exists(file))
                    {
                        CopyDir(file, aimPath + System.IO.Path.GetFileName(file));
                    }
                    else
                    {
                        System.IO.File.Copy(file, aimPath + System.IO.Path.GetFileName(file), true);
                    }


                }
            }
            catch (System.Exception ex)
            {

                throw;
            }
            finally
            {
            }
        }
        /// <summary>
        /// 得到生命周期
        /// </summary>
        /// <param name="GcId"></param>
        /// <param name="GisTick"></param>
        /// <returns></returns>
        public string  Smzq(string GcId, ref string GisTick)
        {
            string StrSmzq = "";
            CDBManger cdb = new CDBManger();
            OracleDataReader OdReader = null;
            try
            {
                string sql = "select LTT_NAME,gisticket_id from cadgis.ticketship where GCTICKET_ID='" + GcId + "'";
                OdReader = cdb.ExecuteReader(sql);
                if (OdReader.Read())
                {
                    StrSmzq = OdReader["LTT_NAME"].ToString();
                    GisTick = OdReader["gisticket_id"].ToString();
                }
            }
            finally
            {
                CloseOracleDataReader(OdReader, cdb);
            }
            return StrSmzq;
        }


        /// <summary>
        /// 获取用户名和密码和区局配置
        /// </summary>
        /// <param name="GcId"></param>
        /// <param name="username"></param>
        /// <param name="password"></param>
        /// <param name="qjpz"></param>
        public static void GetUserNamePwd(string GcId, ref string username, ref string password, ref string qjpz)
        {
            CDBManger dbm = null;
            try
            {
                dbm = new CDBManger();
                string sql = "select GCUSERNAME from cadgis.ticketship where GCTICKET_ID='" + GcId + "'";
                DataTable dt_ticketship = dbm.ExecuteTable(sql);



                if (dt_ticketship != null && dt_ticketship.Rows.Count > 0)
                {
                    username = dt_ticketship.Rows[0]["GCUSERNAME"].ToString();

                }
                else 
                {
                    username = null;
                    password = null;
                    qjpz = null;
                }
                if (!string.IsNullOrEmpty(username))
                {
                    
                    sql = "SELECT password_out(QX_YH_MM) FROM GG_XT_QX_YH where QX_YH_MC='"+username+"'";
                    DataTable dt_ticketship2 = dbm.ExecuteTable(sql);
                    if (dt_ticketship2 != null && dt_ticketship2.Rows.Count > 0)
                    {
                        password = dt_ticketship2.Rows[0][0].ToString();
                    }
                  
                }
                if (!string.IsNullOrEmpty(username))
                {
                    StringBuilder sb = new StringBuilder("");
                    sb.Append("select m.XT_BM_CONFIGURAN from gg_xt_bm_dzhyj  m");
                    sb.Append(" where  m.XT_BM_MC in (");
                    sb.Append(" SELECT  b.qx_zzjg_mc ");
                    sb.Append(" FROM GG_XT_QX_YH　a,GG_XT_QX_ZZJG b  ");
                    sb.Append(" where a.qx_yh_ejdw=b.qx_zzjg_ejdw ");
                    sb.Append(" and  upper(a.qx_yh_mc)='" + username + "' and b.qx_zzjg_mc  in (select x.xt_bm_mc from gg_xt_bm_dzhyj x ))");
                    DataTable dt_ticketship3 = dbm.ExecuteTable(sb.ToString());
                    if (dt_ticketship3 != null && dt_ticketship3.Rows.Count > 0)
                    {
                        qjpz = dt_ticketship3.Rows[0]["XT_BM_CONFIGURAN"].ToString();
                    }

                }
            }
            catch (Exception ex)
            {
                throw;
            }
            finally
            {
                if (dbm != null)
                {
                    dbm.close();
                }
            }
         
        }
        
        /// <summary>
        ///  第一次导数据的方法
        /// </summary>
        /// <param name="oracledb"></param>
        /// <param name="xmldb"></param>
        /// <param name="GC_ID"></param>
        /// <param name="smzq"></param>
        /// <param name="appenshiptime"></param>
        /// <param name="qj"></param>
        public  void OnceImport(OracleDBManager oracledb, XmlDBManager xmldb, string GC_ID, string smzq, DateTime appenshiptime,string qj) 
        {
            try
            {
                ElectronTransferServicePro.uploadPro2(oracledb, xmldb, smzq, GC_ID, appenshiptime, qj);
                CYZLog.writeLog("End 增量的增删改");
           
                UploadEventHandler.CONTAIN_N2(oracledb, xmldb, GC_ID, smzq, appenshiptime, qj);
                CYZLog.writeLog("End 更新包含关系表");
                UploadEventHandler.CONNECTIVITY_N2(oracledb, xmldb, GC_ID, smzq, appenshiptime, qj);
                CYZLog.writeLog("End 更新连接关系表");
                UploadEventHandler.UploadDetailreference_n2(oracledb, xmldb, GC_ID, smzq, appenshiptime, qj);
                CYZLog.writeLog("End 更新祥图关系表");
                UploadEventHandler.UploadGg_jx_jlb_pt2(oracledb, xmldb, GC_ID, smzq, appenshiptime, qj);
                CYZLog.writeLog("End 更新计量表与变压器和计量柜对照表");
                UploadEventHandler.UploadGg_jx_shbd_pt2(oracledb, xmldb, GC_ID, smzq, appenshiptime, qj);
                CYZLog.writeLog("End 更新户表与集抄对照表");
            }
            catch(Exception ex)
            {
                throw;
            }

        }


       
        /// <summary>
        /// 多次导入导数据的方法
        /// </summary>
        /// <param name="oracledb"></param>
        /// <param name="xmldb"></param>
        /// <param name="GC_ID"></param>
        /// <param name="smzq"></param>
        /// <param name="appenshiptime"></param>
        /// <param name="qj"></param>
        public void TwoImport(OracleDBManager oracledb, XmlDBManager xmldb, string GC_ID, string smzq, DateTime appenshiptime,string qj)
        {
            try
            {
                ElectronTransferServicePro.uploadPro(oracledb, xmldb, smzq, GC_ID, appenshiptime,qj);
                CYZLog.writeLog("End 增量的增删改");
                UploadEventHandler.CONTAIN_N(oracledb, xmldb, GC_ID, smzq, appenshiptime, qj);
                CYZLog.writeLog("End 更新包含关系表");
                UploadEventHandler.CONNECTIVITY_N(oracledb, xmldb, GC_ID, smzq, appenshiptime, qj);
                CYZLog.writeLog("End 更新连接关系表");
                UploadEventHandler.UploadDetailreference_n(oracledb, xmldb, GC_ID, smzq, appenshiptime, qj);
                CYZLog.writeLog("End 更新祥图关系表");
                UploadEventHandler.UploadGg_jx_jlb_pt(oracledb, xmldb, GC_ID, smzq, appenshiptime, qj);
                CYZLog.writeLog("End 更新计量表与变压器和计量柜对照表");
                UploadEventHandler.UploadGg_jx_shbd_pt(oracledb, xmldb, GC_ID, smzq, appenshiptime, qj);
                CYZLog.writeLog("End 更新户表与集抄对照表");
            }
            catch (Exception ex)
            {
                CYZLog.writeLog(ex.ToString(), "");
            }

        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="GcId"></param>
        /// <returns></returns>
        public string GisId(string GcId)
        {
            string gisid = "";
            OracleDataReader odr = null;
            string sql = "select GCTICKET_ID from cadgis.ticketship t where t.gcticket_id='" + GcId + "'";
            CDBManger cdb = new CDBManger();
            try
            {
                odr = cdb.ExecuteReader(sql);
                if (odr.Read())
                {
                    gisid = odr["GCTICKET_ID"].ToString();
                }
            }
            catch (Exception ex)
            {
                CYZLog.writeLog(ex.ToString(), "");
            }
            finally
            {
                CloseOracleDataReader(odr, cdb);
            }
            return gisid;
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="kxfid"></param>
        /// <returns></returns>
        public string getKXNameByKxfid(string kxfid)
        {
            CDBManger cdb = new CDBManger();
            OracleDataReader odr = null;
            string sql = "select kxmc  from gg_kxinfo t1, gg_kxmanage t2 where t1.kx_id=t2.kx_id and t1.yx_fid=" + kxfid;
            string gisid = "";
            try
            {
                odr = cdb.ExecuteReader(sql);
                if (odr.Read())
                {
                    gisid = odr["kxmc"].ToString();
                }
            }
            catch (Exception ex)
            {
                CYZLog.writeLog(ex.ToString(), "");
            }
            finally
            {
                CloseOracleDataReader(odr, cdb);
            } return gisid;
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="Xmin"></param>
        /// <param name="Xmax"></param>
        /// <param name="Ymin"></param>
        /// <param name="Ymax"></param>
        /// <returns></returns>
        public string getKXNameByFw(double Xmin, double Xmax, double Ymin, double Ymax)
        {
            CDBManger cdb = new CDBManger();
            OracleDataReader odr = null;
            string sql = "select kxmc  from gg_kxinfo t1, gg_kxmanage t2 where t1.kx_id=t2.kx_id and t1.yx_fid=";
            string gisid = "";
            try
            {
                 odr = cdb.ExecuteReader(sql);
                if (odr.Read())
                {
                    gisid = odr["kxmc"].ToString();
                }
            }
            catch (Exception ex)
            {
                CYZLog.writeLog(ex.ToString(), "");
            }
            finally
            {
                CloseOracleDataReader(odr, cdb);
            }
            return gisid;
        }

        /// <summary>
        /// 提取台账文件名
        /// </summary>
        /// <returns></returns>
        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        public void GetAllDbFiles(string path, ref string fileName)
        {
            try
            {

                if (Directory.Exists(path))
                {
                    var dbDirInfo = new DirectoryInfo(path);
                    var dbFileInfo = dbDirInfo.GetFileSystemInfos("*.zip");
                    foreach (var file in dbFileInfo)
                    {
                        fileName = file.Name;
                    }
                }
            }
            catch (Exception ex)
            {
                CYZLog.writeLog(ex.ToString(), "");
            }
        }
        [WebMethod]
        public string DisableAllTicket()
        {
            string ss = "";
            CDBManger dbm = null;
            try
            {
                dbm = new CDBManger();
                DataTable dt = dbm.ExecuteTable("select distinct t.gcticket_id from cadgis.ticketship t");

                if (dt != null)
                {
                    for (int i = 0; i < dt.Rows.Count; i++)
                    {

                        string GC_ID = dt.Rows[i][0].ToString();
                        CYZLog.writeLog("DisableTicket1(" + GC_ID + ")");
                        if (TickHelper.qxtick1(GC_ID))
                            ss = "TRUE";

                        Thread.Sleep(1000);
                    }
                }


            }
            catch (Exception ex)
            {
                CYZLog.writeLog(ex.ToString(), "");
                ss = "ERROR:" + ex;
            }
            finally 
            {
                if (dbm != null) { dbm.close(); }
            }

            CYZLog.writeLog("End DisableTicket; return " + ss);
            return ss;
        }
        /// <summary>
        /// 关闭OracleDataReader
        /// </summary>
        /// <param name="odr"></param>
        /// <param name="dbManager"> </param>
        private void CloseOracleDataReader(OracleDataReader odr, CDBManger dbManager)
        {
            if (odr != null)
                odr.Close();
            if (dbManager != null)
                dbManager.close();
        }

        /// <summary>
        /// 关闭OracleDataReader
        /// </summary>
        /// <param name="odr"></param>
        private void CloseOracleDataReader(OracleDataReader odr)
        {
            if (odr != null)
                odr.Close();
        }
        /// <summary>
        /// 关闭连接
        /// </summary>
        /// <param name="dbManager"></param>
        private void CloseOracleConnection(CDBManger dbManager)
        {
            if (dbManager != null)
                dbManager.close();
        }
    }
}
