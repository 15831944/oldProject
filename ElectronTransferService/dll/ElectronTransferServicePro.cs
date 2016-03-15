using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;
using System.Data;
using System.Reflection;
using ElectronTransferDal.Common;
using ElectronTransferDal.OracleDal;
using ElectronTransferDal.XmlDal;
using ElectronTransferModel.V9_4;
using Oracle.DataAccess.Client;
using ElectronTransferFramework;
using ElectronTransferModel.Config;
using ElectronTransferFramework.Serialize;
using Ionic.Zip;
using ElectronTransferDal;
using ElectronTransferModel.Base;
using ElectronTransferModel;
using CYZFramework.Log;
using CYZFramework.DB;
using ElectronTransferService.dll;
using CYZFramework;
using System.Configuration;
using ElectronTransferService;
using ElectronTransferDal.Common.Exceptions;


//using System.Data.OracleClient;

namespace ElectronTransferServiceDll
{
    public class ElectronTransferServicePro
    {

        public static string dataappendpath = "";
        public static string datapackagepath = "";
        public static string dataftppath = "";
        public static string QJConfig = "";
        public static string cadEventName = "";
        public static string spatialEventName = "";
        public static string fdoconnectstring = ConfigurationManager.ConnectionStrings[ConfigurationManager.AppSettings["dbInstance"]].ConnectionString.Replace("User Id", "Username").Replace("Password", "Password").Replace("Data Source", "Service");


        /// <summary>
        /// 
        /// </summary>
        /// <param name="oo"></param>
        public static void Pro(object oo)
        {
            try
            {
                List<string> args = (List<string>)oo;

                foreach (string ss in args)
                {
                    Console.WriteLine(ss);
                }

                if (args[0] != null && args.Count > 0)
                {
                    switch (args[0])
                    {
                        case "DownByFW":
                            DownByFW(double.Parse(args[1]), double.Parse(args[2]), double.Parse(args[3]), double.Parse(args[4]), args[5], args[6], args[7], args[8], args[9], args[10]);
                            break;
                        case "DownByKX":
                            //DownByKX(args[1], args[2], args[3], args[4], args[5], args[6]);
                            break;
                        case "DownByKXandFW":
                            DownByKXandFW(args[1], args[2], args[3], args[4], args[5], args[6], args[7]);
                            CYZLog.writeLog("baiyaodfdfdfd2");
                            break;
                        case "DownByDBXFW":
                            DownByDBXFW(args[1], args[2], args[3], args[4], args[5], args[6], args[7]);
                            break;
                        default:
                            break;
                    }
                }
                CYZLog.writeLog(args[0] + " over", "");
            }
            catch (Exception ex)
            {

                CYZLog.writeLog(ex.ToString(), "");
            }
        }
        /// <summary>
        /// 得到传过来的fno
        /// </summary>
        /// <param name="SSDW_ID"></param>
        /// <param name="KX_ID"></param>
        /// <param name="ZYDY"></param>
        /// <param name="GC_ID"></param>
        /// <param name="SESSION_ID"></param>
        /// <param name="kxmc"></param>
        /// <returns></returns>
        public static List<string> Zlsj(string str)
        {
            List<string> FnofidTemp = new List<string>();
            string strtemp = null;

            string[] FnofidTempXh = str.Split(',');
            int itemp = 0;
            foreach (var s in FnofidTempXh)
            {
                if ((itemp % 2) == 0)
                {
                    strtemp = s;
                }
                if ((itemp % 2) == 1)
                {
                    strtemp += "," + s;
                    FnofidTemp.Add(strtemp);
                    strtemp = null;
                }
                itemp++;
            }

            return FnofidTemp;

        }
        #region 暂时不要
        /*
        /// <summary>
        /// 馈线下载主程序
        /// </summary>
        /// <param name="SSDW_ID"></param>
        /// <param name="KX_ID"></param>
        /// <param name="ZYDY"></param>
        /// <param name="GC_ID"></param>
        /// <param name="SESSION_ID"></param>
        /// <param name="kxmc"></param>
        /// <returns></returns>
        public static string DownByKX(string SSDW_ID, string KX_ID, string ZYDY, string GC_ID, string SESSION_ID, string kxmc)//,string strFnoFid
        {
            string sdirx = Path.Combine(Assembly.GetExecutingAssembly().GetPhysicalDirectory(), "MapConfig.xml");
            MapConfig.Instance = ElectronTransferFramework.Serialize.XmlSerializeUtils.Load<MapConfig>(sdirx, new Type[] { });
            string reval = "";
            string tableName = "dbcad_" + SESSION_ID;
            string FtpLj = datapackagepath + kxmc + "_" + SESSION_ID + "\\";
            string FtpUrlPath = dataftppath + kxmc + "_" + SESSION_ID + "/";
            FtpUrlPath = System.Web.HttpUtility.UrlEncode(FtpUrlPath, Encoding.UTF8);
            FtpUrlPath = FtpUrlPath.Replace("%", "<M>");

            try
            {
                //0.新建一个文件夹  SESSION_ID
                Console.WriteLine(FtpLj);
                if (!Directory.Exists(FtpLj))
                {
                    Directory.CreateDirectory(FtpLj);
                }
                else
                {
                    Directory.Delete(FtpLj);
                }

                PublicMethod.write_state(FtpLj, 0.0);

                double xxxxmin = 0;
                double xxxxmax = 0;
                double yyyymin = 0;
                double yyyymax = 0;
                decimal? ltt_id = 123456789;
                string ltt_name = "";
                CDBManger dbm = new CDBManger();
                OracleDataReader dr = dbm.ExecuteReader("select * from cadgis.kxarea t where t.kx_fid=" + KX_ID);
                if (dr != null && dr.Read())
                {
                    xxxxmax = double.Parse(dr["kx_xmax"].ToString());
                    yyyymax = double.Parse(dr["kx_ymax"].ToString());
                    xxxxmin = double.Parse(dr["kx_xmin"].ToString());
                    yyyymin = double.Parse(dr["kx_ymin"].ToString());
                }
                dbm.close();

                dbm = new CDBManger();
                dr = dbm.ExecuteReader("select * from cadgis.ticketship t where t.gcticket_id='" + GC_ID + "'");
                MapConfig.Instance.GCID = GC_ID;
                if (dr != null && dr.Read())
                {
                    if (dr["gisticket_id"] != null)
                        ltt_id = decimal.Parse(dr["gisticket_id"].ToString());
                    else
                        ltt_id = 123456789;
                    ltt_name = dr["ltt_name"].ToString();
                }
                dbm.close();

                if (TickHelper.Trans(tableName, SSDW_ID, KX_ID, ZYDY))
                {
                    //离线台账数据
                    dbm = new CDBManger();
                    string tz_tableName = tableName.Replace("dbcad", "dbtz");
                    dbm.ExecuteNonQuery("create table " + tz_tableName + " as select * from " + tableName);
                    TZInterface.DownTZData(ltt_id.ToString(), tz_tableName, FtpUrlPath);

                    //1.导出的电力设备文件 
                    if (downPro(SESSION_ID, GC_ID, kxmc, ltt_id))
                    {
                        //3.配置文件  MapConfig.xml
                        string sdir = Path.Combine(Assembly.GetExecutingAssembly().GetPhysicalDirectory(), "MapConfig.xml");
                        MapConfig.Instance = ElectronTransferFramework.Serialize.XmlSerializeUtils.Load<MapConfig>(sdir, new Type[] { });

                        MapConfig.Instance.ProjectionMaxX = xxxxmax; //double.Parse(dr["kx_xmax"].ToString());
                        MapConfig.Instance.ProjectionMaxY = yyyymax;// double.Parse(dr["kx_ymax"].ToString());
                        MapConfig.Instance.ProjectionMinX = xxxxmin;// double.Parse(dr["kx_xmin"].ToString());
                        MapConfig.Instance.ProjectionMinY = yyyymin;// double.Parse(dr["kx_ymin"].ToString());
                        MapConfig.Instance.LTTID = (long)ltt_id;
                        MapConfig.Instance.LTTNAME = ltt_name;
                        MapConfig.Instance.KXMC = kxmc;
                        MapConfig.Instance.GCID = GC_ID;

                        MapConfig.Instance.SaveAs(FtpLj + "MapConfig.xml", new Type[] { });


                        PublicMethod.write_state(FtpLj, 0.995);

                        //4.压缩成  SESSION_ID.zip
                        reval = datapackagepath + kxmc + "_" + SESSION_ID;
                        if (File.Exists(reval + @"\emmis.db") || File.Exists(reval + @"\no_emmis.db"))
                        {
                            using (ZipFile zf = new ZipFile(System.Text.Encoding.Default))
                            {
                                zf.AddDirectory(reval);
                                //压缩之后保存路径及压缩文件名
                                zf.Save(reval + ".zip");
                            }

                            PublicMethod.write_state(FtpLj, 1.0);
                        }
                        else
                        {
                            PublicMethod.write_state(FtpLj, "正在下载台账和功能位置，将要完成....");
                        }
                        reval = dataftppath + kxmc + "_" + SESSION_ID + ".zip";
                    }
                    else
                    {
                        PublicMethod.write_state(FtpLj, -1.0);
                        reval = "";
                    }
                }
                else
                {
                    PublicMethod.write_state(FtpLj, -1.0);
                    reval = "";
                }
            }
            catch (Exception ex)
            {
                PublicMethod.write_state(FtpLj, -1.0);
                reval = "";
                CYZLog.writeLog(ex.ToString(), "");
            }
            finally
            {
                try
                {
                    CYZLog.writeLog("删除gis临时表:" + tableName);
                    CDBManger dbm = new CDBManger();
                    StringBuilder sqlstr = new StringBuilder("drop table " + tableName);
                    dbm.ExecuteNonQuery(sqlstr.ToString());
                }
                catch (Exception ex)
                {
                    CYZLog.writeLog(ex.ToString()); 
                }

            }
            return reval;
        }
         */
        #endregion
        ///// <summary>
        /////传给生产的锁定fid和ltt_ld
        ///// </summary>
        ///// <param name="SSDW_ID"></param>
        ///// <param name="KX_ID"></param>
        ///// <param name="ZYDY"></param>
        ///// <param name="GC_ID"></param>
        ///// <param name="SESSION_ID"></param>
        ///// <param name="kxmc"></param>
        ///// <returns></returns>
        //public static void ScTableltt_id(List<string> ltt_idList, decimal? ltt_id, string tablename)
        //{
        //    CDBManger dbm = null;
        //    CDBManger insdbm = null;
        //    CDBManger Cxdbm = null;
        //    try
        //    {
        //        insdbm = new CDBManger();

        //        dbm = new CDBManger();
        //        Cxdbm = new CDBManger();

        //        foreach (var list in ltt_idList)
        //        {
        //            string sqlCX = "select * from  " + tablename + "  where  g3e_fno=" + decimal.Parse(list.Split(',')[0]) + " and g3e_fid=" + decimal.Parse(list.Split(',')[1]);
        //            OracleDataReader dr = dbm.ExecuteReader(sqlCX);
        //            if (dr != null && dr.Read())
        //            {

        //            }
        //            else
        //            {
        //                string inssql = "insert into " + tablename + " (g3e_fno,g3e_fid) values(" + decimal.Parse(list.Split(',')[0]) + "," + decimal.Parse(list.Split(',')[1]) + ")";
        //                Cxdbm.ExecuteNonQuery(inssql);
        //            }
        //        }
        //        foreach (var list in ltt_idList)
        //        {
        //            string upsql = "update " + tablename + " set ltt_id=" + ltt_id + " where  g3e_fno=" + decimal.Parse(list.Split(',')[0]) + " and g3e_fid=" + decimal.Parse(list.Split(',')[1]);
        //            dbm.ExecuteNonQuery(upsql);
        //        }

        //    }
        //    catch (Exception ex)
        //    {
        //        CYZLog.writeLog(ex.ToString());
        //    }

        //}
        /// <summary>
        /// 更新临时表里面的ltt_id
        /// </summary>
        /// <param name="ltt_idList"></param>
        /// <param name="ltt_id"></param>
        /// <param name="tablename"></param>
        public static void ScTableltt_idFW(List<string> ltt_idList, decimal? ltt_id, string tablename)
        {
            CDBManger dbm = null;
            CDBManger Cxdbm = null;
            try
            {
                dbm = new CDBManger();

                string sql1 = "update " + tablename + " set ltt_id=null";
                dbm.ExecuteNonQuery(sql1);

                string fids = "";
                foreach (var list in ltt_idList)
                {
                    fids += list.Split(',')[1] + ",";
                }
                if (fids.Length > 0)
                {
                    fids = fids.Substring(0, fids.Length - 1);
                    sql1 = "update " + tablename + " set ltt_id=" + ltt_id.ToString();
                    sql1 += " where g3e_fid in (" + fids + ") ";
                    dbm.ExecuteNonQuery(sql1);
                }
            }
            catch (Exception ex)
            {

                CYZLog.writeLog(ex.ToString(), "");
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
        /// 馈线和编辑范围
        /// </summary>
        /// <param name="SSDW_ID"></param>
        /// <param name="KX_ID"></param>
        /// <param name="ZYDY"></param>
        /// <param name="GC_ID"></param>
        /// <param name="SESSION_ID"></param>
        /// <param name="kxmc"></param>
        /// <param name="strFnoFid"> </param>
        /// <returns></returns>
        public static string DownByKXandFW(string SSDW_ID, string KX_ID, string ZYDY, string GC_ID, string SESSION_ID, string kxmc, string strFnoFid)////, string strFnoFid
        {
            CYZLog.writeLog("baiyaodfdfdfd");
            List<string> FnofidTemp1 = new List<string>();
            if (strFnoFid != null)
            {
                FnofidTemp1 = Zlsj(strFnoFid);
            }

            string sdirx = Path.Combine(Assembly.GetExecutingAssembly().GetPhysicalDirectory(), "MapConfig.xml");
            MapConfig.Instance = XmlSerializeUtils.Load<MapConfig>(sdirx, new Type[] { });

            string reval = "";
            string tableName = "dbcad_" + SESSION_ID;
            string FtpLj = datapackagepath + kxmc + "_" + SESSION_ID + "\\";
            string FtpUrlPath = dataftppath + kxmc + "_" + SESSION_ID + "/";
            FtpUrlPath = System.Web.HttpUtility.UrlEncode(FtpUrlPath, Encoding.UTF8);
            FtpUrlPath = FtpUrlPath.Replace("%", "<M>");

            try
            {
                //0.新建一个文件夹  SESSION_ID
                Console.WriteLine(FtpLj);
                if (!Directory.Exists(FtpLj))
                {
                    Directory.CreateDirectory(FtpLj);
                }
                else
                {
                    Directory.Delete(FtpLj);
                }

                PublicMethod.write_state(FtpLj, 0.0);

                double xxxxmin = 0;
                double xxxxmax = 0;
                double yyyymin = 0;
                double yyyymax = 0;
                decimal? ltt_id = 123456789;
                string gc_code = "";
                string ltt_name = "";
                CDBManger dbm = new CDBManger();
                OracleDataReader dr = dbm.ExecuteReader("select kx_xmax,kx_ymax,kx_xmin,kx_ymin from cadgis.kxarea t where t.kx_fid=" + KX_ID);
                if (dr != null && dr.Read())
                {
                    xxxxmax = double.Parse(dr["kx_xmax"].ToString());
                    yyyymax = double.Parse(dr["kx_ymax"].ToString());
                    xxxxmin = double.Parse(dr["kx_xmin"].ToString());
                    yyyymin = double.Parse(dr["kx_ymin"].ToString());
                }
                CloseOracleDataReader(dr,dbm);

                dbm = new CDBManger();
                string sql = "select gisticket_id,ltt_name,GCCODE from cadgis.ticketship t where t.gcticket_id='" + GC_ID + "'";
                dr = dbm.ExecuteReader(sql);
                MapConfig.Instance.GCID = GC_ID;
                if (dr != null && dr.Read())
                {
                    if (dr["gisticket_id"] != null)
                    {
                        ltt_id = decimal.Parse(dr["gisticket_id"].ToString());
                        CYZLog.writeLog("工单存在：", ltt_id.ToString());
                    }
                    else
                    {
                        ltt_id = 123456789;
                    }
                    ltt_name = dr["ltt_name"].ToString();
                    gc_code = dr["GCCODE"].ToString();

                }
                CloseOracleDataReader(dr, dbm);

                var xmldb = new XmlDBManager();
                xmldb.FileName = FtpLj + kxmc + "_" + SESSION_ID + ".xml";
                xmldb.Initialize();
                if (TickHelper.Trans(tableName, SSDW_ID, KX_ID, ZYDY, FtpLj))
                {
                    if (TickHelper.Trans3(tableName + "_2", xxxxmin, xxxxmax, yyyymin, yyyymax, ZYDY, FtpLj))
                    {
                        if (FnofidTemp1.Any())
                        {
                            ScTableltt_idFW(FnofidTemp1, ltt_id, tableName);
                        }
                        //离线台账数据
                        dbm = new CDBManger();
                        string tz_tableName = tableName.Replace("dbcad", "dbtz");
                        dbm.ExecuteNonQuery("create table " + tz_tableName + " as select * from " + tableName);
                        TZInterface.DownTZData(ltt_id.ToString(), tz_tableName, FtpUrlPath);
                        string kxSql = TransKxFidSql(KX_ID);

                        if (downPro4(SESSION_ID, GC_ID, kxmc, ltt_id, xmldb, kxSql) && downPro2(SESSION_ID + "_2", GC_ID, kxmc, ltt_id, xmldb))
                        {//1.导出的电力设备文件                                 //导出的编辑范围的数据
                            ////3.配置文件  MapConfig.xml
                            //string sdir = Path.Combine(Assembly.GetExecutingAssembly().GetPhysicalDirectory(), "MapConfig.xml");
                            //MapConfig.Instance = XmlSerializeUtils.Load<MapConfig>(sdir, new Type[] { });

                            MapConfig.Instance.ProjectionMaxX = xxxxmax; //double.Parse(dr["kx_xmax"].ToString());
                            MapConfig.Instance.ProjectionMaxY = yyyymax;// double.Parse(dr["kx_ymax"].ToString());
                            MapConfig.Instance.ProjectionMinX = xxxxmin;// double.Parse(dr["kx_xmin"].ToString());
                            MapConfig.Instance.ProjectionMinY = yyyymin;// double.Parse(dr["kx_ymin"].ToString());
                            MapConfig.Instance.LTTID = (long)ltt_id;
                            MapConfig.Instance.LTTNAME = ltt_name;
                            MapConfig.Instance.KXMC = kxmc;
                            MapConfig.Instance.GCID = gc_code;

                            MapConfig.Instance.SaveAs(FtpLj + "MapConfig.xml", new Type[] { });


                            PublicMethod.write_state(FtpLj, 0.995);

                            //4.压缩成  SESSION_ID.zip
                            reval = datapackagepath + kxmc + "_" + SESSION_ID;
                            if (File.Exists(reval + @"\emmis.zip") || File.Exists(reval + @"\no_emmis.db"))
                            {
                                using (ZipFile zf = new ZipFile(Encoding.Default))
                                {
                                    zf.AddDirectory(reval);
                                    //压缩之后保存路径及压缩文件名
                                    zf.Save(reval + ".zip");
                                }

                                PublicMethod.write_state(FtpLj, 1.0);
                            }
                            else
                            {
                                PublicMethod.write_state(FtpLj, "gis功能位置已下载完成。正在等待台账...");
                            }
                            reval = dataftppath + kxmc + "_" + SESSION_ID + ".zip";
                        }
                        else
                        {
                           PublicMethod.write_state(FtpLj, -1.0);
                            reval = "";
                        }
                    }
                    else
                    {
                        PublicMethod.write_state(FtpLj, -1.0);
                        reval = "";
                    }
                }
                else
                {
                    PublicMethod.write_state(FtpLj, -1.0);
                    reval = "";
                }
            }
            catch (Exception ex)
            {
                PublicMethod.write_state(FtpLj, -1.0);
                reval = "";
                CYZLog.writeLog(ex.ToString(), "");
            }
            finally
            {
                try
                {
                    CDBManger dbm = new CDBManger();
                    StringBuilder sqlstr = new StringBuilder("drop table " + tableName);
                    dbm.ExecuteNonQuery(sqlstr.ToString());
                    sqlstr = new StringBuilder("drop table " + tableName + "_2");
                    dbm.ExecuteNonQuery(sqlstr.ToString());
                    dbm.close();
                }
                catch (Exception ex) { CYZLog.writeLog(ex.ToString(), ""); }

            }
            return reval;
        }

        /// <summary>
        /// 关闭OracleDataReader
        /// </summary>
        /// <param name="odr"></param>
        /// <param name="dbManager"> </param>
        private static void CloseOracleDataReader(OracleDataReader odr,CDBManger dbManager)
        {
            if (odr != null)
                odr.Close();
            if (dbManager != null)
                dbManager.close();
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="Xmin"></param>
        /// <param name="Xmax"></param>
        /// <param name="Ymin"></param>
        /// <param name="Ymax"></param>
        /// <param name="ZYDY"></param>
        /// <param name="GC_ID"></param>
        /// <param name="SESSION_ID"></param>
        /// <param name="kxmc"></param>
        /// <param name="sj"> </param>
        /// <param name="strFnoFid"> </param>
        /// <returns></returns>
        public static string DownByFW(double Xmin, double Xmax, double Ymin, double Ymax, string ZYDY, string GC_ID, string SESSION_ID, string kxmc, string sj, string strFnoFid)//, string strFnoFid
        {
            //SESSION_ID = GC_ID;
            Console.WriteLine("start DownByFW");
            string reval = "";
            List<string> FnofidTemp1 = new List<string>();
            if (strFnoFid != null)
            {
                FnofidTemp1 = Zlsj(strFnoFid);
            }

            string tableName = "dbcad_" + SESSION_ID;
            string FtpLj = datapackagepath + kxmc + "_" + SESSION_ID + "\\";
            string FtpUrlPath = dataftppath + kxmc + "_" + SESSION_ID + "/";
            FtpUrlPath = System.Web.HttpUtility.UrlEncode(FtpUrlPath, Encoding.UTF8);
            FtpUrlPath = FtpUrlPath.Replace("%", "<M>");
            try
            {
                //0.新建一个文件夹  SESSION_ID

                Console.WriteLine(FtpLj);
                if (!Directory.Exists(FtpLj))
                {
                    Directory.CreateDirectory(FtpLj);
                }
                else
                {
                    Directory.Delete(FtpLj);
                }

                PublicMethod.write_state(FtpLj, 0.0);

                decimal? ltt_id = 123456789;
                string ltt_name = "";
                string gc_code = "";
                string KxMcList = "";
                CDBManger dbm = new CDBManger();
                OracleDataReader dr = null;
                try
                {
                    dr = dbm.ExecuteReader("select * from cadgis.ticketship t where t.gcticket_id='" + GC_ID + "'");
                }
                catch (Exception ex)
                {
                    CYZLog.writeLog(ex.Message);
                }
               
                MapConfig.Instance.GCID = GC_ID;
                if (dr != null && dr.Read())
                {
                    if (dr["gisticket_id"] != null)
                    {
                        ltt_id = decimal.Parse(dr["gisticket_id"].ToString());
                        CYZLog.writeLog("工单存在：", ltt_id.ToString());
                    }
                    else
                    {
                        ltt_id = 123456789;
                    }
                    ltt_name = dr["ltt_name"].ToString();
                    gc_code = dr["GCCODE"].ToString();

                }
                CloseOracleDataReader(dr, dbm);

                if (TickHelper.Trans2(tableName, Xmin, Xmax, Ymin, Ymax, ZYDY))
                {
                    if (ZYDY == "1" || ZYDY == "2")
                    {
                        TickHelper.DcByq(tableName, FnofidTemp1);
                    }


                    if (FnofidTemp1.Any())
                    {
                        ScTableltt_idFW(FnofidTemp1, ltt_id, tableName);
                    }

                    //离线台账数据
                    dbm = new CDBManger();
                    string tz_tableName = tableName.Replace("dbcad", "dbtz");
                    dbm.ExecuteNonQuery("create table " + tz_tableName + " as select * from " + tableName);
                    
                    TZInterface.DownTZData(ltt_id.ToString(), tz_tableName, FtpUrlPath);

                    string fwSql = TransfwFidSql(tableName, ref KxMcList);

                    if (downProsj(SESSION_ID, GC_ID, kxmc, ltt_id, sj, fwSql,gc_code))
                    {//1.导出的电力设备文件 


                        //3.配置文件  MapConfig.xml
                        string sdir = Path.Combine(Assembly.GetExecutingAssembly().GetPhysicalDirectory(), "MapConfig.xml");
                        MapConfig.Instance = ElectronTransferFramework.Serialize.XmlSerializeUtils.Load<MapConfig>(sdir, new Type[] { });
                        MapConfig.Instance.ProjectionMaxX = Xmax;
                        MapConfig.Instance.ProjectionMaxY = Ymax;
                        MapConfig.Instance.ProjectionMinX = Xmin;
                        MapConfig.Instance.ProjectionMinY = Ymin;
                        MapConfig.Instance.LTTID = (long)ltt_id;
                        MapConfig.Instance.LTTNAME = ltt_name;
                        MapConfig.Instance.KXMC = KxMcList;
                        MapConfig.Instance.GCID = gc_code;

                        MapConfig.Instance.SaveAs(FtpLj + "MapConfig.xml", new Type[] { });

                        //4.压缩成  SESSION_ID.zip
                        reval = datapackagepath + kxmc + "_" + SESSION_ID;
                        // string path1 = @"E:\work\CAD\svn100\文档\地图\DGDT";
                        //System.Text.Encoding.Default解决中文文件夹名称乱码

                        if (File.Exists(reval + @"\emmis.zip") || File.Exists(reval + @"\no_emmis.db"))
                        {
                            using (ZipFile zf = new ZipFile(System.Text.Encoding.Default))
                            {
                                zf.AddDirectory(reval);
                                //压缩之后保存路径及压缩文件名
                                zf.Save(reval + ".zip");
                            }
                            PublicMethod.write_state(FtpLj, 1.0);
                        }
                        else
                        {
                            PublicMethod.write_state(FtpLj, "gis功能位置已下载完成。正在等待台账...");
                        }
                        reval = dataftppath + kxmc + "_" + SESSION_ID + ".zip";

                    }
                    else
                    {
                        PublicMethod.write_state(FtpLj, -1.0);
                        reval = "";
                    }
                }
                else
                {
                    PublicMethod.write_state(FtpLj, -1.0);
                    reval = "";
                }
            }
            catch (Exception ex)
            {
                PublicMethod.write_state(FtpLj, -1.0);
                reval = "";
               // CYZLog.writeLog(ex.ToString(), "");
            }
            finally
            {
                try
                {
                    CDBManger dbm = new CDBManger();
                    StringBuilder sqlstr = new StringBuilder("drop table " + tableName);
                    dbm.ExecuteNonQuery(sqlstr.ToString());
                    dbm.close();
                    //sqlstr = new StringBuilder("drop table " + tableName + "_2");
                    //dbm.ExecuteNonQuery(sqlstr.ToString());
                }
                catch (Exception ex)
                {
                    CYZLog.writeLog(ex.ToString(), "");
                }

            }
            return reval;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="ZYDY"></param>
        /// <param name="GC_ID"></param>
        /// <param name="SESSION_ID"></param>
        /// <param name="kxmc"></param>
        /// <param name="strFnoFid"></param>
        /// <param name="DbxList"></param>
        /// <param name="sj"> </param>
        /// <returns></returns>
        public static string DownByDBXFW(string ZYDY, string GC_ID, string SESSION_ID, string kxmc, string strFnoFid, string DbxList, string sj)//, string strFnoFid
        {
            //SESSION_ID = GC_ID;
            Console.WriteLine("start DownByDBXFW");
            string reval = "";
            List<string> FnofidTemp1 = new List<string>();
            List<string> DbxlistZb = new List<string>();
            if (strFnoFid != null)
            {
                FnofidTemp1 = Zlsj(strFnoFid);
            }

            if (DbxList != null)
            {
                DbxlistZb = Zlsj(DbxList);
            }


            string tableName = "dbcad_" + SESSION_ID;

            //string FtpLj = datapackagepath + SESSION_ID + "\\";
            string FtpLj = datapackagepath + kxmc + "_" + SESSION_ID + "\\";
            string FtpUrlPath = dataftppath + kxmc + "_" + SESSION_ID + "/";
            FtpUrlPath = System.Web.HttpUtility.UrlEncode(FtpUrlPath, Encoding.UTF8);
            FtpUrlPath = FtpUrlPath.Replace("%", "<M>");
            try
            {
                //0.新建一个文件夹  SESSION_ID

                Console.WriteLine(FtpLj);
                if (!Directory.Exists(FtpLj))
                {
                    Directory.CreateDirectory(FtpLj);
                }
                else
                {
                    Directory.Delete(FtpLj);
                }

                PublicMethod.write_state(FtpLj, 0.0);
                double Xmin = 0;
                double Xmax = 0;
                double Ymin = 0;
                double Ymax = 0;
                double[] pXmax = new double[DbxlistZb.Count()];
                double[] pYmax = new double[DbxlistZb.Count()];
                int Pxi = 0;
                foreach (var v in DbxlistZb)
                {
                    string[] px = v.Split(',');
                    pXmax[Pxi] = double.Parse(px[0]);
                    pYmax[Pxi] = double.Parse(px[1]);
                    Pxi++;
                }


                decimal? ltt_id = 123456789;
                string ltt_name = "";
                string gc_code = "";
                string KxMcList = "";
                CDBManger dbm = new CDBManger();
                OracleDataReader dr = dbm.ExecuteReader("select * from cadgis.ticketship t where t.gcticket_id='" + GC_ID + "'");
                MapConfig.Instance.GCID = GC_ID;
                if (dr != null && dr.Read())
                {
                    if (dr["gisticket_id"] != null)
                    {
                        ltt_id = decimal.Parse(dr["gisticket_id"].ToString());
                        CYZLog.writeLog("工单存在：", ltt_id.ToString());
                    }
                    else
                    {
                        ltt_id = 123456789;
                    }
                    ltt_name = dr["ltt_name"].ToString();
                    gc_code = dr["GCCODE"].ToString();

                }
                CloseOracleDataReader(dr, dbm);

                if (TickHelper.TransDbx(tableName, ZYDY, DbxlistZb))
                {
                    if (ZYDY == "1" || ZYDY == "2")
                    {
                        TickHelper.DcByq(tableName, FnofidTemp1);
                    }

                    if (FnofidTemp1.Any())
                    {
                        ScTableltt_idFW(FnofidTemp1, ltt_id, tableName);
                    }

                    //离线台账数据
                    dbm = new CDBManger();
                    string tz_tableName = tableName.Replace("dbcad", "dbtz");
                    dbm.ExecuteNonQuery("create table " + tz_tableName + " as select * from " + tableName);
                    TZInterface.DownTZData(ltt_id.ToString(), tz_tableName, FtpUrlPath);
                    string fwSql = TransfwFidSql(tableName, ref KxMcList);
                    //1.导出的电力设备文件 
                    if (downPro(SESSION_ID, GC_ID, kxmc, ltt_id, fwSql, sj,gc_code))
                    {
                        //3.配置文件  MapConfig.xml
                        string sdir = Path.Combine(Assembly.GetExecutingAssembly().GetPhysicalDirectory(), "MapConfig.xml");
                        MapConfig.Instance = ElectronTransferFramework.Serialize.XmlSerializeUtils.Load<MapConfig>(sdir, new Type[] { });
                        MapConfig.Instance.ProjectionMaxX = pXmax.Max(); 
                        MapConfig.Instance.ProjectionMaxY = pYmax.Max();
                        MapConfig.Instance.ProjectionMinX = pXmax.Min();
                        MapConfig.Instance.ProjectionMinY = pYmax.Min();
                        MapConfig.Instance.LTTID = (long)ltt_id;
                        MapConfig.Instance.LTTNAME = ltt_name;
                        MapConfig.Instance.KXMC = KxMcList;
                        MapConfig.Instance.GCID = gc_code;

                        MapConfig.Instance.SaveAs(FtpLj + "MapConfig.xml", new Type[] { });

                        //4.压缩成  SESSION_ID.zip
                        reval = datapackagepath + kxmc + "_" + SESSION_ID;
                        // string path1 = @"E:\work\CAD\svn100\文档\地图\DGDT";
                        //System.Text.Encoding.Default解决中文文件夹名称乱码

                        if (File.Exists(reval + @"\emmis.zip") || File.Exists(reval + @"\no_emmis.db"))
                        {
                            using (ZipFile zf = new ZipFile(System.Text.Encoding.Default))
                            {
                                zf.AddDirectory(reval);
                                //压缩之后保存路径及压缩文件名
                                zf.Save(reval + ".zip");
                            }
                            PublicMethod.write_state(FtpLj, 1.0);
                        }
                        else
                        {
                            PublicMethod.write_state(FtpLj, "gis功能位置已下载完成。正在等待台账...");
                        }
                        reval = dataftppath + kxmc + "_" + SESSION_ID + ".zip";

                    }
                    else
                    {
                        PublicMethod.write_state(FtpLj, -1.0);
                        reval = "";
                    }
                }
                else
                {
                    PublicMethod.write_state(FtpLj, -1.0);
                    reval = "";
                }
            }
            catch (Exception ex)
            {
                PublicMethod.write_state(FtpLj, -1.0);
                reval = "";
                CYZLog.writeLog(ex.ToString(), "");
            }
            finally
            {
                try
                {
                    CDBManger dbm = new CDBManger();
                    StringBuilder sqlstr = new StringBuilder("drop table " + tableName);
                    dbm.ExecuteNonQuery(sqlstr.ToString());
                }
                catch (Exception ex)
                {
                    CYZLog.writeLog(ex.ToString(), "");
                }

            }
            return reval;
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="session_id"></param>
        /// <param name="gc_id"></param>
        /// <param name="kxmc"></param>
        /// <param name="ltt_id"></param>
        /// <returns></returns>
        private static bool downPro(string session_id, string gc_id, string kxmc, decimal? ltt_id, string FwSql, string sj, string gc_code)
        {
            string FtpLj = datapackagepath + kxmc + "_" + session_id + "\\";
            string tableName = "dbcad_" +session_id;
            string[] allowStatus = new string[] { "EDIT", "ADD", "DELETE" };
            bool bbreval = false;
            CDBManger dbm = new CDBManger();

            try
            {
                var xmldb = new XmlDBManager();
                xmldb.FileName = FtpLj + kxmc+gc_code + "_" + session_id + ".xml";
                xmldb.Initialize();
                //var oracledb = new OracleDBManager();
                var oracledb = new OracleDBManagerV94();

                ExportSymbolEventHandler.exportHandle(xmldb, oracledb, tableName, FtpLj, ltt_id);

                IEnumerable<Common_n> CommonSet = oracledb.GetEntities<Common_n>(" where g3e_fid in (select g3e_fid from " + tableName + ") ").ToList();
                var ASet_Common = from item in CommonSet.Where(o => o.LTT_ID == ltt_id
                                && allowStatus.Contains(o.GetValue<string>("LTT_STATUS")))
                                  group item by item.G3E_FID into g
                                  select g.OrderBy(o => o.G3E_ID).Last();
                var BSet_Common = from item in CommonSet.Where(o => o.GetValue<decimal?>("LTT_ID") == 0
                    && ASet_Common.Count(a => a.G3E_FID == o.G3E_FID) == 0)
                                  select item;
                var UnionSet_Common = ASet_Common.Union(BSet_Common);
                xmldb.InsertBulk(UnionSet_Common.Cast<DBEntity>());
                // var BSet_Common = from item in CommonSet.Where(o => o.GetValue<decimal?>("LTT_ID") == 0
                PublicMethod.write_state(FtpLj, 0.8);

                //连接关系  b$connectivity_n
                //IEnumerable<Connectivity_n> ConnectivitySet = oracledb.GetEntities<Connectivity_n>(FwSql);
                //var ASet_Connectivity = from item in ConnectivitySet.Where(o => o.GetValue<decimal?>("LTT_ID") == ltt_id
                //               && allowStatus.Contains(o.GetValue<string>("LTT_STATUS")))
                //                        group item by item.G3E_FID into g
                //                        select g.OrderBy(o => o.G3E_ID).Last();
                //var BSet_Connectivity = from item in ConnectivitySet.Where(o => o.GetValue<decimal?>("LTT_ID") == 0
                //    && ASet_Connectivity.Count(a => a.G3E_FID == o.G3E_FID) == 0)
                //                        select item;
                //var UnionSet_Connectivity = ASet_Connectivity.Union(BSet_Connectivity);
                //xmldb.InsertBulk(UnionSet_Connectivity.Cast<DBEntity>());

                string initFileName = Path.Combine(Assembly.GetExecutingAssembly().GetPhysicalDirectory(), "connectivityparams.txt");
                if (File.Exists(initFileName))
                {
                    WApi.exportconnectivity(FwSql, session_id, ltt_id.ToString(), initFileName, FtpLj, fdoconnectstring);
                    XmlDBManager xmldb_connectivity = null;
                    xmldb_connectivity = new XmlDBManager();
                    xmldb_connectivity.FileName = FtpLj + "connectivity_n_" + session_id + ".xml";
                    xmldb_connectivity.Initialize();
                    xmldb.Merge(xmldb_connectivity);

                }
                if (File.Exists(FtpLj + "connectivity_n_" + session_id + ".xml"))
                {
                    File.Delete(FtpLj + "connectivity_n_" + session_id + ".xml");
                }

                PublicMethod.write_state(FtpLj, 0.82);

                //包含关系  b$contain_n
                IEnumerable<Contain_n> ContainSet = oracledb.GetEntities<Contain_n>(" where g3e_fid in (select g3e_fid from " + tableName + ") ").ToList();
                var ASet_Contain = from item in ContainSet.Where(o => o.GetValue<decimal?>("LTT_ID") == ltt_id
                            && allowStatus.Contains(o.GetValue<string>("LTT_STATUS")))
                                   group item by item.G3E_FID into g
                                   select g.OrderBy(o => o.G3E_ID).Last();
                var BSet_Contain = from item in ContainSet.Where(o => o.GetValue<decimal?>("LTT_ID") == 0
                    && ASet_Contain.Count(a => a.G3E_FID == o.G3E_FID) == 0)
                                   select item;
                var UnionSet_Contain = ASet_Contain.Union(BSet_Contain);
                xmldb.InsertBulk(UnionSet_Contain.Cast<DBEntity>());

                PublicMethod.write_state(FtpLj, 0.84);
                //受电馈线
                IEnumerable<Gg_pd_sdkx_ac> SdkxSet = oracledb.GetEntities<Gg_pd_sdkx_ac>(" where g3e_fid in (select g3e_fid from " + tableName + ") ").ToList();
                var ASet_sdkx = from item in SdkxSet.Where(o => o.GetValue<decimal?>("LTT_ID") == ltt_id
                            && allowStatus.Contains(o.GetValue<string>("LTT_STATUS")))
                                group item by item.G3E_FID into g
                                select g.OrderBy(o => o.G3E_ID).Last();
                var BSet_sdkx = from item in SdkxSet.Where(o => o.GetValue<decimal?>("LTT_ID") == 0
                    && ASet_sdkx.Count(a => a.G3E_FID == o.G3E_FID) == 0)
                                select item;
                var UnionSet_sdkx = ASet_sdkx.Union(BSet_sdkx);
                xmldb.InsertBulk(UnionSet_sdkx.Cast<DBEntity>());
                //o =>object.Equals( o.GetValue("LTT_ID") , ltt_id )
                //o => object.Equals(o.GetValue("LTT_ID"), 0)
                //功能位置表
                IEnumerable<Gg_pd_gnwzmc_n> gnwzmcSet = oracledb.GetEntities<Gg_pd_gnwzmc_n>(" where g3e_fid in (select g3e_fid from " + tableName + ") ").ToList();
                var ASet_gnwzmc = from item in gnwzmcSet.Where(o => o.GetValue<decimal?>("LTT_ID") == ltt_id
                            && allowStatus.Contains(o.GetValue<string>("LTT_STATUS")))
                                  group item by item.G3E_FID into g
                                  select g.OrderBy(o => o.G3E_ID).Last();
                var BSet_gnwzmc = from item in gnwzmcSet.Where(o => o.GetValue<decimal?>("LTT_ID") == 0
                    && ASet_gnwzmc.Count(a => a.G3E_FID == o.G3E_FID) == 0)
                                  select item;
                var UnionSet_gnwzmc = ASet_gnwzmc.Union(BSet_gnwzmc);
                xmldb.InsertBulk(UnionSet_gnwzmc.Cast<DBEntity>());

                PublicMethod.write_state(FtpLj, 0.89);
                
                //地图
                GetDTWZBZ(FtpLj);
                //CD表
                GetCDConfig(FtpLj);
                
               
                PublicMethod.write_state(FtpLj, 0.99);


                xmldb.Submit();

                bbreval = true;

            }
            catch (Exception ex)
            {
                CYZLog.writeLog(ex.ToString(), "");
                bbreval = false;
            }
            finally
            {
                dbm.close();
            }
            return bbreval;
        }
        /// <summary>
        /// 多加一个时间参数
        /// </summary>
        /// <param name="session_id"></param>
        /// <param name="gc_id"></param>
        /// <param name="kxmc"></param>
        /// <param name="ltt_id"></param>
        /// <param name="sj"></param>
        /// <returns></returns>
        private static bool downProsj(string session_id, string gc_id, string kxmc, decimal? ltt_id, string sj, string fwSql, string gc_code)
        {
            string FtpLj = datapackagepath + kxmc + "_" + session_id + "\\";
            string tableName = "dbcad_" + session_id;
            string[] allowStatus = new string[] { "EDIT", "ADD", "DELETE" };
            bool bbreval = false;
            CDBManger dbm = new CDBManger();

            try
            {
                var xmldb = new XmlDBManager();
                xmldb.FileName = FtpLj + kxmc+gc_code + "_" + session_id + ".xml";
                xmldb.Initialize();
                //var oracledb = new OracleDBManager();
                var oracledb = new OracleDBManagerV94();
              
                
               ExportSymbolEventHandler.exportHandle(xmldb, oracledb, tableName, FtpLj, ltt_id);
               //IList<Common_n> CommonSet1 = oracledb.GetEntities<Common_n>(" where g3e_fid in (select g3e_fid from " + tableName + ") ");
               IEnumerable<Common_n> CommonSet = oracledb.GetEntities<Common_n>(" where g3e_fid in (select g3e_fid from " + tableName + ") ").ToList();
               var ASet_Common = from item in CommonSet.Where(o => o.LTT_ID == ltt_id
                               && allowStatus.Contains(o.GetValue<string>("LTT_STATUS")))
                                 group item by item.G3E_FID into g
                                 select g.OrderBy(o => o.G3E_ID).Last();
               var BSet_Common = from item in CommonSet.Where(o => o.GetValue<decimal?>("LTT_ID") == 0
                   && ASet_Common.Count(a => a.G3E_FID == o.G3E_FID) == 0)
                                 select item;
               var UnionSet_Common = ASet_Common.Union(BSet_Common);
               xmldb.InsertBulk(UnionSet_Common.Cast<DBEntity>());

                PublicMethod.write_state(FtpLj, 0.8);

                //连接关系  b$connectivity_n
                //IEnumerable<Connectivity_n> ConnectivitySet = oracledb.GetEntities<Connectivity_n>(" where g3e_fid in (select g3e_fid from " + tableName + ") ");
                //var ASet_Connectivity = from item in ConnectivitySet.Where(o => o.GetValue<decimal?>("LTT_ID") == ltt_id
                //               && allowStatus.Contains(o.GetValue<string>("LTT_STATUS")))
                //                        group item by item.G3E_FID into g
                //                        select g.OrderBy(o => o.G3E_ID).Last();
                //var BSet_Connectivity = from item in ConnectivitySet.Where(o => o.GetValue<decimal?>("LTT_ID") == 0
                //    && ASet_Connectivity.Count(a => a.G3E_FID == o.G3E_FID) == 0)
                //                        select item;
                //var UnionSet_Connectivity = ASet_Connectivity.Union(BSet_Connectivity);
                //xmldb.InsertBulk(UnionSet_Connectivity.Cast<DBEntity>());
                string initFileName = Path.Combine(Assembly.GetExecutingAssembly().GetPhysicalDirectory(), "connectivityparams.txt");
                if (File.Exists(initFileName))
                {
                    WApi.exportconnectivity(fwSql, session_id,ltt_id.ToString(), initFileName, FtpLj,fdoconnectstring);
                    XmlDBManager xmldb_connectivity = null;
                    xmldb_connectivity = new XmlDBManager();
                    xmldb_connectivity.FileName = FtpLj + "connectivity_n_" + session_id + ".xml";
                    xmldb_connectivity.Initialize();
                    xmldb.Merge(xmldb_connectivity);
                }


                if (File.Exists(FtpLj + "connectivity_n_" + session_id + ".xml"))
                {
                    File.Delete(FtpLj + "connectivity_n_" + session_id + ".xml");
                }


                PublicMethod.write_state(FtpLj, 0.82);

                //包含关系  b$contain_n
                IEnumerable<Contain_n> ContainSet = oracledb.GetEntities<Contain_n>(" where g3e_fid in (select g3e_fid from " + tableName + ") ").ToList();
                var ASet_Contain = from item in ContainSet.Where(o => o.GetValue<decimal?>("LTT_ID") == ltt_id
                            && allowStatus.Contains(o.GetValue<string>("LTT_STATUS")))
                                   group item by item.G3E_FID into g
                                   select g.OrderBy(o => o.G3E_ID).Last();
                var BSet_Contain = from item in ContainSet.Where(o => o.GetValue<decimal?>("LTT_ID") == 0
                    && ASet_Contain.Count(a => a.G3E_FID == o.G3E_FID) == 0)
                                   select item;
                var UnionSet_Contain = ASet_Contain.Union(BSet_Contain);
                xmldb.InsertBulk(UnionSet_Contain.Cast<DBEntity>());

                PublicMethod.write_state(FtpLj, 0.84);
                //受电馈线
                //IEnumerable<Gg_pd_sdkx_ac> SdkxSet = oracledb.GetEntities<Gg_pd_sdkx_ac>(" where g3e_fid in (select g3e_fid from " + tableName + ") ").ToList();
                //var ASet_sdkx = from item in SdkxSet.Where(o => o.GetValue<decimal?>("LTT_ID") == ltt_id
                //            && allowStatus.Contains(o.GetValue<string>("LTT_STATUS")))
                //                group item by item.G3E_FID into g
                //                select g.OrderBy(o => o.G3E_ID).Last(); 
                //var BSet_sdkx = from item in SdkxSet.Where(o => o.GetValue<decimal?>("LTT_ID") == 0
                //    && ASet_sdkx.Count(a => a.G3E_FID == o.G3E_FID) == 0)
                //                select item;
                //var UnionSet_sdkx = ASet_sdkx.Union(BSet_sdkx);
                //xmldb.InsertBulk(UnionSet_sdkx.Cast<DBEntity>());

                IEnumerable<Gg_pd_sdkx_ac> SdkxSet = oracledb.GetEntities<Gg_pd_sdkx_ac>(" where g3e_fid in (select g3e_fid from " + tableName + ") ").ToList();
                var ASet_sdkx = from item in SdkxSet.Where(o => o.GetValue<decimal?>("LTT_ID") == ltt_id
                            && allowStatus.Contains(o.GetValue<string>("LTT_STATUS")))
                                group item by item.G3E_FID into g
                                select g.OrderBy(o => o.G3E_ID);

                var BSet_sdkx = from item in SdkxSet.Where(o => o.GetValue<decimal?>("LTT_ID") == 0
                    && ASet_sdkx.Count(a => a.Last().G3E_FID == o.G3E_FID) == 0)
                                select item;
                // IEnumerable<Gg_pd_sdkx_ac> UnionSet_sdkx=null;
                foreach (var sdkx in ASet_sdkx)
                {
                    xmldb.InsertBulk(sdkx.Cast<DBEntity>());
                }

                 
                xmldb.InsertBulk(BSet_sdkx.Cast<DBEntity>());

                //功能位置表
                IEnumerable<Gg_pd_gnwzmc_n> gnwzmcSet = oracledb.GetEntities<Gg_pd_gnwzmc_n>(" where g3e_fid in (select g3e_fid from " + tableName + ") ").ToList();
                var ASet_gnwzmc = from item in gnwzmcSet.Where(o => o.GetValue<decimal?>("LTT_ID") == ltt_id
                            && allowStatus.Contains(o.GetValue<string>("LTT_STATUS")))
                                  group item by item.G3E_FID into g
                                  select g.OrderBy(o => o.G3E_ID).Last();
                var BSet_gnwzmc = from item in gnwzmcSet.Where(o => o.GetValue<decimal?>("LTT_ID") == 0
                    && ASet_gnwzmc.Count(a => a.G3E_FID == o.G3E_FID) == 0)
                                  select item;
                var UnionSet_gnwzmc = ASet_gnwzmc.Union(BSet_gnwzmc);
                xmldb.InsertBulk(UnionSet_gnwzmc.Cast<DBEntity>());

                PublicMethod.write_state(FtpLj, 0.89);
                //地图
                GetDTWZBZ(FtpLj);
                //CD表
                //XmlDBManager xmldb_cdconfig = null;
                //if (GetCDConfig(ref xmldb_cdconfig))
                //{
                //    xmldb.Merge(xmldb_cdconfig);
                //}
                GetCDConfig(FtpLj);

                PublicMethod.write_state(FtpLj, 0.99);


                xmldb.Submit();

                bbreval = true;

            }
            catch (Exception ex)
            {
                CYZLog.writeLog(ex.ToString());
                bbreval = false;
            }
            finally
            {
                dbm.close();
            }
            return bbreval;
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="session_id"></param>
        /// <param name="gc_id"></param>
        /// <param name="kxmc"></param>
        /// <param name="ltt_id"></param>
        /// <param name="xmldb"></param>
        /// <param name="ii"></param>
        /// <returns></returns>
        private static bool downPro2(string session_id, string gc_id, string kxmc, decimal? ltt_id, XmlDBManager xmldb)
        {
            string FtpLj = datapackagepath + kxmc + "_" + session_id.Substring(0, session_id.Length - 2) + "\\";

            string[] allowStatus = new string[] { "EDIT", "ADD", "DELETE" };
            bool bbreval = false;
            // CDBManger dbm = new CDBManger();
            var oracledb = new OracleDBManagerV94();
            //var oracledb = new OracleDBManager();

            double bfb1 = (20 / (double)1) * 0.01;
            double bfb2 = 0.8;
            try
            {
                string tableName = "dbcad_" + session_id;

                ExportSymbolEventHandler.exportHandle2(xmldb, oracledb, tableName, FtpLj, ltt_id);

                PublicMethod.write_state(FtpLj, 0.99);

                xmldb.Submit();

                bbreval = true;
            }
            catch (Exception ex)
            {
                CYZLog.writeLog(ex.ToString(), "");
                bbreval = false;
            }
            finally
            {
                //dbm.close();
            }
            return bbreval;
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="session_id"></param>
        /// <param name="gc_id"></param>
        /// <param name="kxmc"></param>
        /// <param name="ltt_id"></param>
        /// <param name="xmldb"></param>
        /// <returns></returns>
        private static bool downPro4(string session_id, string gc_id, string kxmc, decimal? ltt_id, XmlDBManager xmldb, string kxsql)
        {
            string FtpLj = datapackagepath + kxmc + "_" + session_id + "\\";
            string tableName = "dbcad_" + session_id;
            string[] allowStatus = new string[] { "EDIT", "ADD", "DELETE" };
            bool bbreval = false;
            CDBManger dbm = new CDBManger();

            try
            {
                var oracledb = new OracleDBManagerV94();
                //var oracledb = new OracleDBManager();

               ExportSymbolEventHandler.exportHandle(xmldb, oracledb, tableName, FtpLj, ltt_id);

               IEnumerable<Common_n> CommonSet = oracledb.GetEntities<Common_n>(" where g3e_fid in (select g3e_fid from " + tableName + ") and g3e_fno not in(198,199)").ToList();
               var ASet_Common = from item in CommonSet.Where(o => o.LTT_ID == ltt_id
                               && allowStatus.Contains(o.GetValue<string>("LTT_STATUS")))
                                 group item by item.G3E_FID into g
                                 select g.OrderBy(o => o.G3E_ID).Last();
               var BSet_Common = from item in CommonSet.Where(o => o.GetValue<decimal?>("LTT_ID") == 0
                   && ASet_Common.Count(a => a.G3E_FID == o.G3E_FID) == 0)
                                 select item;
               var UnionSet_Common = ASet_Common.Union(BSet_Common);
               xmldb.InsertBulk(UnionSet_Common.Cast<DBEntity>());

               PublicMethod.write_state(FtpLj, 0.8);

               //连接关系  b$connectivity_n
               //IEnumerable<Connectivity_n> ConnectivitySet = oracledb.GetEntities<Connectivity_n>(kxsql);
               //var ASet_Connectivity = from item in ConnectivitySet.Where(o => o.GetValue<decimal?>("LTT_ID") == ltt_id
               //               && allowStatus.Contains(o.GetValue<string>("LTT_STATUS")))
               //                        group item by item.G3E_FID into g
               //                        select g.OrderBy(o => o.G3E_ID).Last();
               //var BSet_Connectivity = from item in ConnectivitySet.Where(o => o.GetValue<decimal?>("LTT_ID") == 0
               //    && ASet_Connectivity.Count(a => a.G3E_FID == o.G3E_FID) == 0)
               //                        select item;
               //var UnionSet_Connectivity = ASet_Connectivity.Union(BSet_Connectivity);
               //xmldb.InsertBulk(UnionSet_Connectivity.Cast<DBEntity>());

               string initFileName = Path.Combine(Assembly.GetExecutingAssembly().GetPhysicalDirectory(), "connectivityparams.txt");
               if (File.Exists(initFileName))
               {
                   WApi.exportconnectivity(kxsql, session_id, ltt_id.ToString(), initFileName, FtpLj, fdoconnectstring);
                   XmlDBManager xmldb_connectivity = null;
                   xmldb_connectivity = new XmlDBManager();
                   xmldb_connectivity.FileName = FtpLj + "connectivity_n_" + session_id + ".xml";
                   xmldb_connectivity.Initialize();
                   xmldb.Merge(xmldb_connectivity);

               }
               if (File.Exists(FtpLj + "connectivity_n_" + session_id + ".xml"))
               {
                   File.Delete(FtpLj + "connectivity_n_" + session_id + ".xml");
               }

               PublicMethod.write_state(FtpLj, 0.7);

               //包含关系  b$contain_n
               IEnumerable<Contain_n> ContainSet = oracledb.GetEntities<Contain_n>(" where g3e_fid in (select g3e_fid from " + tableName + ") ").ToList();
               var ASet_Contain = from item in ContainSet.Where(o => o.GetValue<decimal?>("LTT_ID") == ltt_id
                           && allowStatus.Contains(o.GetValue<string>("LTT_STATUS")))
                                  group item by item.G3E_FID into g
                                  select g.OrderBy(o => o.G3E_ID).Last();
               var BSet_Contain = from item in ContainSet.Where(o => o.GetValue<decimal?>("LTT_ID") == 0
                   && ASet_Contain.Count(a => a.G3E_FID == o.G3E_FID) == 0)
                                  select item;
               var UnionSet_Contain = ASet_Contain.Union(BSet_Contain);
               xmldb.InsertBulk(UnionSet_Contain.Cast<DBEntity>());

               PublicMethod.write_state(FtpLj, 0.73);
               //受电馈线
               IEnumerable<Gg_pd_sdkx_ac> SdkxSet = oracledb.GetEntities<Gg_pd_sdkx_ac>(" where g3e_fid in (select g3e_fid from " + tableName + ") ").ToList();
               var ASet_sdkx = from item in SdkxSet.Where(o => o.GetValue<decimal?>("LTT_ID") == ltt_id
                           && allowStatus.Contains(o.GetValue<string>("LTT_STATUS")))
                               group item by item.G3E_FID into g
                               select g.OrderBy(o => o.G3E_ID);

               var BSet_sdkx = from item in SdkxSet.Where(o => o.GetValue<decimal?>("LTT_ID") == 0
                   && ASet_sdkx.Count(a => a.Last().G3E_FID == o.G3E_FID) == 0)
                               select item;
               // IEnumerable<Gg_pd_sdkx_ac> UnionSet_sdkx=null;
               foreach (var sdkx in ASet_sdkx)
               {
                   xmldb.InsertBulk(sdkx.Cast<DBEntity>());
               }


               xmldb.InsertBulk(BSet_sdkx.Cast<DBEntity>());
               //xmldb.InsertBulk(ASet_sdkx.Cast<DBEntity>());


               //IEnumerable<Gg_pd_sdkx_ac> SdkxSet = oracledb.GetEntities<Gg_pd_sdkx_ac>(" where g3e_fid in (select g3e_fid from " + tableName + ") ").ToList();
               //var ASet_sdkx = from item in SdkxSet.Where(o => o.GetValue<decimal?>("LTT_ID") == ltt_id
               //            && allowStatus.Contains(o.GetValue<string>("LTT_STATUS")))
               //                group item by item.G3E_FID into g
               //                select g.OrderBy(o => o.G3E_ID).Last();
               //var BSet_sdkx = from item in SdkxSet.Where(o => o.GetValue<decimal?>("LTT_ID") == 0
               //    && ASet_sdkx.Count(a => a.G3E_FID == o.G3E_FID) == 0)
               //                select item;
               //var UnionSet_sdkx = ASet_sdkx.Union(BSet_sdkx);
               //xmldb.InsertBulk(UnionSet_sdkx.Cast<DBEntity>());


                //功能位置表
                IEnumerable<Gg_pd_gnwzmc_n> gnwzmcSet = oracledb.GetEntities<Gg_pd_gnwzmc_n>(" where g3e_fid in (select g3e_fid from " + tableName + ") and g3e_fno not in(198,199)").ToList();
                var ASet_gnwzmc = from item in gnwzmcSet.Where(o => o.GetValue<decimal?>("LTT_ID") == ltt_id
                            && allowStatus.Contains(o.GetValue<string>("LTT_STATUS")))
                                  group item by item.G3E_FID into g
                                  select g.OrderBy(o => o.G3E_ID).Last();
                var BSet_gnwzmc = from item in gnwzmcSet.Where(o => o.GetValue<decimal?>("LTT_ID") == 0
                    && ASet_gnwzmc.Count(a => a.G3E_FID == o.G3E_FID) == 0)
                                  select item;
                var UnionSet_gnwzmc = ASet_gnwzmc.Union(BSet_gnwzmc);
                xmldb.InsertBulk(UnionSet_gnwzmc.Cast<DBEntity>());

               
             

                PublicMethod.write_state(FtpLj, 0.75);
                //地图
              GetDTWZBZ(FtpLj);
                //CD表
                //XmlDBManager xmldb_cdconfig = null;
                //if (GetCDConfig(ref xmldb_cdconfig))
                //{
                //    xmldb.Merge(xmldb_cdconfig);
                //}
                GetCDConfig(FtpLj);

                PublicMethod.write_state(FtpLj, 0.8);

                bbreval = true;

            }
            catch (Exception ex)
            {
                CYZLog.writeLog(ex.ToString(), "");
                bbreval = false;
            }
            finally
            {
                dbm.close();
            }
            return bbreval;
        }


        /// <summary>
        /// cd表的生成
        /// </summary>
        /// <param name="oracledb"></param>
        /// <param name="xmldb"></param>
        /// <param name="smzq"></param>
        /// <param name="GC_ID"></param>
        /// <param name="appenshiptime"></param>
        //public static bool GetCDConfig(ref XmlDBManager xmldb_cdconfig)
        public static bool GetCDConfig(string ftplj)
        {
            bool bbreval = false;
            try
            {
                string sdirx = datapackagepath + "CdConfig.xml";
                if (File.Exists(sdirx))
                {
                    //xmldb_cdconfig = new XmlDBManager();
                    //xmldb_cdconfig.FileName = sdirx;
                    //xmldb_cdconfig.Initialize();
                    File.Copy(sdirx, ftplj + "CdConfig.xml");
                    return true;
                }

                XmlDBManager xmldb_cdconfig = new XmlDBManager();
                xmldb_cdconfig.FileName = sdirx;
                xmldb_cdconfig.Initialize();

                var oracledb2 = new OracleDBManager();

                IEnumerable<Cd_dlxhgg> set68 = oracledb2.GetEntities<Cd_dlxhgg>();
                xmldb_cdconfig.InsertBulk(set68.Cast<DBEntity>());
                //
                IEnumerable<Cd_ctbb> set69 = oracledb2.GetEntities<Cd_ctbb>();
                xmldb_cdconfig.InsertBulk(set69.Cast<DBEntity>());
                //制造厂家
                IEnumerable<Cd_zzcj> set70 = oracledb2.GetEntities<Cd_zzcj>();
                xmldb_cdconfig.InsertBulk(set70.Cast<DBEntity>());
                //编辑类型
                IEnumerable<Cd_bjlx> set71 = oracledb2.GetEntities<Cd_bjlx>();
                xmldb_cdconfig.InsertBulk(set71.Cast<DBEntity>());
                //
                IEnumerable<Cd_circuitnumber> set72 = oracledb2.GetEntities<Cd_circuitnumber>();
                xmldb_cdconfig.InsertBulk(set72.Cast<DBEntity>());
                //
                IEnumerable<Cd_hlmc> set73 = oracledb2.GetEntities<Cd_hlmc>();
                xmldb_cdconfig.InsertBulk(set73.Cast<DBEntity>());
                //线路分类2
                try
                {
                    IEnumerable<Cd_xlfl2> set74 = oracledb2.GetEntities<Cd_xlfl2>();
                    xmldb_cdconfig.InsertBulk(set74.Cast<DBEntity>());
                }
                catch (Exception ex)
                {
                }
            
                //
                IEnumerable<Cd_xw> set75 = oracledb2.GetEntities<Cd_xw>();
                xmldb_cdconfig.InsertBulk(set75.Cast<DBEntity>());
                //用户类型
                IEnumerable<Cd_yhlx> set76 = oracledb2.GetEntities<Cd_yhlx>();
                xmldb_cdconfig.InsertBulk(set76.Cast<DBEntity>());

                IEnumerable<Cd_sfsdlt> set77 = oracledb2.GetEntities<Cd_sfsdlt>();
                xmldb_cdconfig.InsertBulk(set77.Cast<DBEntity>());

                IEnumerable<Cd_bzlx> set78 = oracledb2.GetEntities<Cd_bzlx>();
                xmldb_cdconfig.InsertBulk(set78.Cast<DBEntity>());

                //IEnumerable<Cd_ptgfl> set79 = oracledb2.GetEntities<Cd_ptgfl>();
                //xmldb_cdconfig.InsertBulk(set79.Cast<DBEntity>());

                IEnumerable<Cd_zclx> set80 = oracledb2.GetEntities<Cd_zclx>();
                xmldb_cdconfig.InsertBulk(set80.Cast<DBEntity>());

                IEnumerable<Cd_sfczd> set81 = oracledb2.GetEntities<Cd_sfczd>();
                xmldb_cdconfig.InsertBulk(set81.Cast<DBEntity>());

                IEnumerable<Cd_zsdlqlx> set82 = oracledb2.GetEntities<Cd_zsdlqlx>();
                xmldb_cdconfig.InsertBulk(set82.Cast<DBEntity>());

                IEnumerable<Cd_jlblx> set83 = oracledb2.GetEntities<Cd_jlblx>();
                xmldb_cdconfig.InsertBulk(set83.Cast<DBEntity>());


                IEnumerable<Cd_azwz> set5 = oracledb2.GetEntities<Cd_azwz>();
                xmldb_cdconfig.InsertBulk(set5.Cast<DBEntity>());

                //变电出线分类
                IEnumerable<Cd_bdcxfl> set6 = oracledb2.GetEntities<Cd_bdcxfl>();
                xmldb_cdconfig.InsertBulk(set6.Cast<DBEntity>());


                try
                {
                    //避雷器分类
                    IEnumerable<Cd_blqfl> set7 = oracledb2.GetEntities<Cd_blqfl>();
                    xmldb_cdconfig.InsertBulk(set7.Cast<DBEntity>());
                }
                catch (Exception ex)
                {
                }
            
                //PublicMethod.write_state(FtpLj, 0.88);

                //电房类型
                IEnumerable<Cd_dflx> set9 = oracledb2.GetEntities<Cd_dflx>();
                xmldb_cdconfig.InsertBulk(set9.Cast<DBEntity>());

                //电缆分支箱分类
                IEnumerable<Cd_dlfxxfl> set10 = oracledb2.GetEntities<Cd_dlfxxfl>();
                xmldb_cdconfig.InsertBulk(set10.Cast<DBEntity>());
                // PublicMethod.write_state(FtpLj, 0.75);

                //电缆头分类
                IEnumerable<Cd_dltfl> set11 = oracledb2.GetEntities<Cd_dltfl>();
                xmldb_cdconfig.InsertBulk(set11.Cast<DBEntity>());

                //地区特征
                IEnumerable<Cd_dqtz> set12 = oracledb2.GetEntities<Cd_dqtz>();
                xmldb_cdconfig.InsertBulk(set12.Cast<DBEntity>());

                //地区状态
                IEnumerable<Cd_dqzt> set13 = oracledb2.GetEntities<Cd_dqzt>();
                xmldb_cdconfig.InsertBulk(set13.Cast<DBEntity>());

                //电压等级
                IEnumerable<Cd_dydj> set14 = oracledb2.GetEntities<Cd_dydj>();
                xmldb_cdconfig.InsertBulk(set14.Cast<DBEntity>());

                //电压柜分类
                IEnumerable<Cd_dygfl> set15 = oracledb2.GetEntities<Cd_dygfl>();
                xmldb_cdconfig.InsertBulk(set15.Cast<DBEntity>());

                //分类
                IEnumerable<Cd_dykgfl> set16 = oracledb2.GetEntities<Cd_dykgfl>();
                xmldb_cdconfig.InsertBulk(set16.Cast<DBEntity>());
                //PublicMethod.write_state(FtpLj, 0.91);

                //同杆架设情况
                IEnumerable<Cd_ggjsqk> set17 = oracledb2.GetEntities<Cd_ggjsqk>();
                xmldb_cdconfig.InsertBulk(set17.Cast<DBEntity>());

                //截面积
                IEnumerable<Cd_jmj> set18 = oracledb2.GetEntities<Cd_jmj>();
                xmldb_cdconfig.InsertBulk(set18.Cast<DBEntity>());

                //开关柜分类
                IEnumerable<Cd_kggfl> set19 = oracledb2.GetEntities<Cd_kggfl>();
                xmldb_cdconfig.InsertBulk(set19.Cast<DBEntity>());
                //  PublicMethod.write_state(FtpLj, 0.92);

                //配变类别
                IEnumerable<Cd_pblb> set20 = oracledb2.GetEntities<Cd_pblb>();
                xmldb_cdconfig.InsertBulk(set20.Cast<DBEntity>());

                //CD_PTFL
                IEnumerable<Cd_ptfl> set21 = oracledb2.GetEntities<Cd_ptfl>();
                xmldb_cdconfig.InsertBulk(set21.Cast<DBEntity>());

                //CD_SBLX_JLG 设备类型
                IEnumerable<Cd_sblx_jlg> set22 = oracledb2.GetEntities<Cd_sblx_jlg>();
                xmldb_cdconfig.InsertBulk(set22.Cast<DBEntity>());

                //CD_SBLX_TTU 设备类型
                IEnumerable<Cd_sblx_ttu> set23 = oracledb2.GetEntities<Cd_sblx_ttu>();
                xmldb_cdconfig.InsertBulk(set23.Cast<DBEntity>());

                //是否导出结果
                IEnumerable<Cd_sfdcjg> set24 = oracledb2.GetEntities<Cd_sfdcjg>();
                xmldb_cdconfig.InsertBulk(set24.Cast<DBEntity>());

                //是否带电
                IEnumerable<Cd_sfdd> set25 = oracledb2.GetEntities<Cd_sfdd>();
                xmldb_cdconfig.InsertBulk(set25.Cast<DBEntity>());

                //是否带熔断器 
                IEnumerable<Cd_sfdyrdq> set26 = oracledb2.GetEntities<Cd_sfdyrdq>();
                xmldb_cdconfig.InsertBulk(set26.Cast<DBEntity>());

                //是否可带负荷操作
                IEnumerable<Cd_sfsdl> set27 = oracledb2.GetEntities<Cd_sfsdl>();
                xmldb_cdconfig.InsertBulk(set27.Cast<DBEntity>());

                //是否站出线
                IEnumerable<Cd_sfzcx> set28 = oracledb2.GetEntities<Cd_sfzcx>();
                xmldb_cdconfig.InsertBulk(set28.Cast<DBEntity>());

                //生命周期
                IEnumerable<Cd_smzq> set29 = oracledb2.GetEntities<Cd_smzq>();
                xmldb_cdconfig.InsertBulk(set29.Cast<DBEntity>());

                //所属单位
                //IEnumerable<Cd_ssdw> set30 = oracledb.GetEntities<Cd_ssdw>();
                //xmldb_cdconfig.InsertBulk(set30.Cast<DBEntity>());

                //功能位置设施分类
                IEnumerable<Cd_ssfl> set31 = oracledb2.GetEntities<Cd_ssfl>();
                xmldb_cdconfig.InsertBulk(set31.Cast<DBEntity>());

                //通信分类
                IEnumerable<Cd_txfs> set32 = oracledb2.GetEntities<Cd_txfs>();
                xmldb_cdconfig.InsertBulk(set32.Cast<DBEntity>());

                //线路分类
                IEnumerable<Cd_xlfl> set33 = oracledb2.GetEntities<Cd_xlfl>();
                xmldb_cdconfig.InsertBulk(set33.Cast<DBEntity>());

                //箱式设备类型
                IEnumerable<Cd_xssblx> set34 = oracledb2.GetEntities<Cd_xssblx>();
                xmldb_cdconfig.InsertBulk(set34.Cast<DBEntity>());

                //CD_YFZNMS
                IEnumerable<Cd_yfznms> set35 = oracledb2.GetEntities<Cd_yfznms>();
                xmldb_cdconfig.InsertBulk(set35.Cast<DBEntity>());

                //开关分类
                IEnumerable<Cd_zfhwgfl> set36 = oracledb2.GetEntities<Cd_zfhwgfl>();
                xmldb_cdconfig.InsertBulk(set36.Cast<DBEntity>());

                //所属线路
                IEnumerable<Cd_ssxl> set37 = oracledb2.GetEntities<Cd_ssxl>();
                xmldb_cdconfig.InsertBulk(set37.Cast<DBEntity>());

                //电杆类别
                IEnumerable<Cd_cllb> set38 = oracledb2.GetEntities<Cd_cllb>();
                xmldb_cdconfig.InsertBulk(set38.Cast<DBEntity>());

                //型号规格
                IEnumerable<Cd_xhge> set44 = oracledb2.GetEntities<Cd_xhge>().Where(o => o.G3E_FNO != 0);
                xmldb_cdconfig.InsertBulk(set44.Cast<DBEntity>());

                //柱上变压器分类
                IEnumerable<Cd_zsbyqfl> set45 = oracledb2.GetEntities<Cd_zsbyqfl>();
                xmldb_cdconfig.InsertBulk(set45.Cast<DBEntity>());

                //代表区域
                IEnumerable<Cd_dbqy> set46 = oracledb2.GetEntities<Cd_dbqy>();
                xmldb_cdconfig.InsertBulk(set46.Cast<DBEntity>());

                //灭弧方式
                IEnumerable<Cd_mhxx> set47 = oracledb2.GetEntities<Cd_mhxx>();
                xmldb_cdconfig.InsertBulk(set47.Cast<DBEntity>());

                //工作状态
                IEnumerable<Cd_gzzt> set48 = oracledb2.GetEntities<Cd_gzzt>();
                xmldb_cdconfig.InsertBulk(set48.Cast<DBEntity>());

                //相数
                IEnumerable<Cd_xs> set49 = oracledb2.GetEntities<Cd_xs>();
                xmldb_cdconfig.InsertBulk(set49.Cast<DBEntity>());

                //抄表箱类型
                IEnumerable<Cd_cbx_lx> set50 = oracledb2.GetEntities<Cd_cbx_lx>();
                xmldb_cdconfig.InsertBulk(set50.Cast<DBEntity>());

                //电缆沿布
                IEnumerable<Cd_dldlyb> set51 = oracledb2.GetEntities<Cd_dldlyb>();
                xmldb_cdconfig.InsertBulk(set51.Cast<DBEntity>());

                ////设备评级
                IEnumerable<Cd_sbpj> set52 = oracledb2.GetEntities<Cd_sbpj>();
                xmldb_cdconfig.InsertBulk(set52.Cast<DBEntity>());

                //用电类别
                IEnumerable<Cd_ydlb> set53 = oracledb2.GetEntities<Cd_ydlb>();
                xmldb_cdconfig.InsertBulk(set53.Cast<DBEntity>());

                //粗细度
                IEnumerable<Cd_cxd> set54 = oracledb2.GetEntities<Cd_cxd>();
                xmldb_cdconfig.InsertBulk(set54.Cast<DBEntity>());

                //节点类型
                IEnumerable<Cd_jdlx> set55 = oracledb2.GetEntities<Cd_jdlx>();
                xmldb_cdconfig.InsertBulk(set55.Cast<DBEntity>());

                //产权属性
                IEnumerable<Cd_cqsx> set56 = oracledb2.GetEntities<Cd_cqsx>();
                xmldb_cdconfig.InsertBulk(set56.Cast<DBEntity>());

                // PublicMethod.write_state(FtpLj, 0.983333);
                //维护归属
                IEnumerable<Cd_whgs> set57 = oracledb2.GetEntities<Cd_whgs>();
                xmldb_cdconfig.InsertBulk(set57.Cast<DBEntity>());

                //维护班所
                IEnumerable<Cd_bs> set63 = oracledb2.GetEntities<Cd_bs>();
                xmldb_cdconfig.InsertBulk(set63.Cast<DBEntity>());

                //连接关系规范
                IEnumerable<G3e_nodeedgeconnectivity> set39 = oracledb2.GetEntities<G3e_nodeedgeconnectivity>();
                xmldb_cdconfig.InsertBulk(set39.Cast<DBEntity>());

                //从属关系规范
                IEnumerable<G3e_ownership> set40 = oracledb2.GetEntities<G3e_ownership>();
                xmldb_cdconfig.InsertBulk(set40.Cast<DBEntity>());

                //点符号样式规范
                IEnumerable<G3e_pointstyle> set41 = oracledb2.GetEntities<G3e_pointstyle>();
                xmldb_cdconfig.InsertBulk(set41.Cast<DBEntity>());

                IEnumerable<G3e_textstyle> set84 = oracledb2.GetEntities<G3e_textstyle>();
                xmldb_cdconfig.InsertBulk(set84.Cast<DBEntity>());


                //线符号样式规范
                IEnumerable<G3e_linestyle> set42 = oracledb2.GetEntities<G3e_linestyle>();
                xmldb_cdconfig.InsertBulk(set42.Cast<DBEntity>());

                //面符号样式规范
                IEnumerable<G3e_areastyle> set43 = oracledb2.GetEntities<G3e_areastyle>();
                xmldb_cdconfig.InsertBulk(set43.Cast<DBEntity>());


                try
                {
                    //支线表
                    IEnumerable<Gnwz_tz_zx> set58 = oracledb2.GetEntities<Gnwz_tz_zx>();
                    xmldb_cdconfig.InsertBulk(set58.Cast<DBEntity>());
                }
                catch (Exception ex)
                {
                }



                try
                {
                    //kxinfo
                    IEnumerable<Gg_kxinfo> set59 = oracledb2.GetEntities<Gg_kxinfo>();
                    xmldb_cdconfig.InsertBulk(set59.Cast<DBEntity>());
                }
                catch (Exception ex)
                {
                }
            

                //Gg_kxmanage
                IEnumerable<Gg_kxmanage> set60 = oracledb2.GetEntities<Gg_kxmanage>();
                xmldb_cdconfig.InsertBulk(set60.Cast<DBEntity>());


                try 
                {
                    //gnwz_fl
                    IEnumerable<gnwz_fl> set61 = oracledb2.GetEntities<gnwz_fl>();
                    xmldb_cdconfig.InsertBulk(set61.Cast<DBEntity>());
                }catch(Exception ex)
                {
                }
       

                //gnwz_fl2
                IEnumerable<gnwz_fl2> set62 = oracledb2.GetEntities<gnwz_fl2>();
                xmldb_cdconfig.InsertBulk(set62.Cast<DBEntity>());

                //额定电流
                IEnumerable<Cd_eddl> set64 = oracledb2.GetEntities<Cd_eddl>();
                xmldb_cdconfig.InsertBulk(set64.Cast<DBEntity>());

                IEnumerable<Cd_jddzys> set65 = oracledb2.GetEntities<Cd_jddzys>();
                xmldb_cdconfig.InsertBulk(set65.Cast<DBEntity>());

                IEnumerable<Cd_sfdqkx> set66 = oracledb2.GetEntities<Cd_sfdqkx>();
                xmldb_cdconfig.InsertBulk(set66.Cast<DBEntity>());

                //检测点类别
                IEnumerable<Cd_lb> set67 = oracledb2.GetEntities<Cd_lb>();
                xmldb_cdconfig.InsertBulk(set67.Cast<DBEntity>());

                xmldb_cdconfig.Submit();
                bbreval = true;

                File.Copy(sdirx, ftplj + "CdConfig.xml");
            }
            catch (Exception ex)
            {
                CYZLog.writeLog(ex.ToString(), "");
                bbreval = false;
            }
            finally
            {
                //dbm.close();
            }
            return bbreval;
        }

         /// <summary>
        /// 地图表的生成
        /// </summary>
        /// <param name="oracledb"></param>
        /// <param name="xmldb"></param>
        /// <param name="smzq"></param>
        /// <param name="GC_ID"></param>
        public static void GetDTWZBZ(string FtpLj)
        {
            try
            {

                string sdirx = datapackagepath + "DT_WZBZ.xml";
                if (!File.Exists(sdirx))
                {

                    XmlDBManager xmldb_dtconfig = new XmlDBManager();
                    xmldb_dtconfig.FileName = sdirx;
                    xmldb_dtconfig.Initialize();
                    var oracledbDT = new OracleDBManagerV94();
                    IEnumerable<Gg_dt_wzbz_lb_sdogeom_cad> dtset = oracledbDT.GetEntities<Gg_dt_wzbz_lb_sdogeom_cad>();
                    xmldb_dtconfig.InsertBulk(dtset.Cast<DBEntity>());
                    xmldb_dtconfig.Submit();
                    File.Copy(sdirx, FtpLj + "DT_WZBZ.xml");
                }
                else {

                    File.Copy(sdirx, FtpLj + "DT_WZBZ.xml");
                }
            }
            catch (Exception ex)
            {
                CYZLog.writeLog(ex.ToString(), "");
            }
        }


        

        /// <summary>
        /// 
        /// </summary>
        /// <param name="oracledb"></param>
        /// <param name="xmldb"></param>
        /// <param name="smzq"></param>
        /// <param name="GC_ID"></param>
        /// <param name="appenshiptime"></param>
        public static void uploadPro(OracleDBManager oracledb, XmlDBManager xmldb, string smzq, string GC_ID, DateTime appenshiptime, string qj)
        {
            try
            {
                List<string> AppendFidShipS = new List<string>();
                List<InsertTempTable> InsertTempTableS = new List<InsertTempTable>();
                List<ElectronBase> InsertEntSpatial = new List<ElectronBase>();
                List<ElectronBase> InsertEnt = new List<ElectronBase>();
                List<ElectronBase> UpdateEnt = new List<ElectronBase>();
                List<ElectronBase> DeleteEnt = new List<ElectronBase>();
                List<ElectronBase> InsLb = new List<ElectronBase>();
                List<ElectronBase> InsertEntUp = new List<ElectronBase>();//修改增量
                if (oracledb == null) { return; }
                if (xmldb == null) { return; }
                if (string.IsNullOrEmpty(smzq)) { return; }
                if (string.IsNullOrEmpty(GC_ID)) { return; }

                OracleSequenceValueGenerator.Instance.DbManager = oracledb;

                string symboldir = Path.Combine(Assembly.GetExecutingAssembly().GetPhysicalDirectory(), "SymbolConfig.xml");
                SimpleSymbolConfig.Instance = ElectronTransferFramework.Serialize.XmlSerializeUtils.Load<SimpleSymbolConfig>(symboldir, new Type[] { });

                //List<string> clearSpatialStr = new List<string>();
                CDBManger cdm = new CDBManger();
                string lttid = "";
                string str = "select gisticket_id from cadgis.ticketship t  where t.gcticket_id='" + GC_ID + "'";
                OracleDataReader dr = cdm.ExecuteReader(str);
                if (dr != null && dr.Read() && dr["gisticket_id"] != null)
                {
                    lttid = dr["gisticket_id"].ToString();
                }
                CloseOracleDataReader(dr, cdm);

                foreach (var symbol in SimpleSymbolConfig.Instance.Symbols)
                {
                    //
                    //自身属性表
                    //
                    try
                    {
                        ////if (symbol.Fno ==82)
                        ////{
                        ////    CYZLog.writeLog("148", "");
                        ////}

                        //撤销spatial的工单数据
                        //clearSpatial(symbol.PtClassName, GC_ID);
                        //if (lttid != "")
                        //{
                        //    clearSpatialStr.Add("delete cadgis.b$" + symbol.PtClassName + " where ltt_id=" + lttid);
                        //}

                        //72接地挂环    没有自身属性表
                        //174无功补偿器 没有自身属性表
                        if (!string.IsNullOrEmpty(symbol.SelfAttribute))
                        {
                            Type type = TypeCache.Instance.GetTypeFromCache(typeof(DBEntity), symbol.SelfAttribute);
                            var es = xmldb.GetEntities(type).Cast<ElectronBase>();


                            foreach (var e in es)
                            {

                                if (e == null) { continue; }
                                //148有多个标注， lb 14803 ， lb1 14815 ，lb2 14804
                                //172  17201符号，17202标注
                                //176  17601符号，17602标注
                                if (e.EntityState == EntityState.Insert)
                                {
                                    long og3e_fid = e.G3E_FID;
                                    long Old_G3e_id = e.G3E_ID;
                                    //
                                    //开始插入自身属性
                                    //
                                    //1.如果在appendfidship找不到og3e_fid,那么就可以在gis里面新增fid
                                    long OldGisFid = 0;
                                    long OldGisid = 0;
                                    if (ReFid(og3e_fid, GC_ID, ref OldGisFid, ref OldGisid))
                                    {
                                        e.G3E_ID = OracleSequenceValueGenerator.Instance.GenerateTableId(type);
                                        e.G3E_FID = OldGisFid;

                                    }
                                    else
                                    {
                                        e.G3E_ID = OracleSequenceValueGenerator.Instance.GenerateTableId(type);
                                        e.G3E_FID = OracleSequenceValueGenerator.Instance.GenerateGlobalId();
                                    }


                                    //2.否则,需要在appendfidship里面找出对于的fid

                                    //3.需要吧这个版本的所有新增的og3e_fid记录起来



                                    InsertEnt.Add(e);
                                    //
                                    //开始插入公共属性
                                    //                                
                                    if (e.G3E_FNO == 159 || e.G3E_FNO == 160 || e.G3E_FNO == 250)
                                    {
                                        //if (QueryAppendfidship(e.G3E_FID, e.G3E_ID))
                                        if (e.G3E_FID != OldGisFid)
                                        {
                                            AppendFidShipS.Add("insert into cadgis.AppendFidShip(G3E_FID,G3E_FID2,GCTICKET_ID,G3E_ID,G3E_ID2,VERSION,G3E_FNO) values(" + e.G3E_FID + "," + og3e_fid + ",'"
                                         + GC_ID + "', " + e.G3E_ID + "," + Old_G3e_id + "," + e.GetValue("Version") + "," + e.G3E_FNO + ")");
                                        }
                                    }
                                    else
                                    {
                                        Type type1 = TypeCache.Instance.GetTypeFromCache(typeof(DBEntity), symbol.Common);
                                        var com = xmldb.GetEntities(type1).Cast<ElectronBase>().Where(o => o.G3E_FID == og3e_fid).FirstOrDefault();
                                        if (com != null)
                                        {
                                            if (OldGisid == 0)
                                            {
                                                com.G3E_ID = OracleSequenceValueGenerator.Instance.GenerateTableId(type1);
                                            }
                                            else
                                            {
                                                com.G3E_ID = OldGisid;
                                            }

                                            com.G3E_FID = e.G3E_FID;
                                            InsertEnt.Add(com);
                                            //if (QueryAppendfidship(e.G3E_FID, e.G3E_ID))
                                            if (e.G3E_FID != OldGisFid)
                                            {
                                                AppendFidShipS.Add("insert into cadgis.AppendFidShip(G3E_FID,G3E_FID2,GCTICKET_ID,G3E_ID,G3E_ID2,VERSION,G3E_FNO) values(" + e.G3E_FID + "," + og3e_fid + ",'"
                                               + GC_ID + "', " + com.G3E_ID + "," + Old_G3e_id + "," + e.GetValue("Version") + "," + e.G3E_FNO + ")");
                                            }
                                        }
                                        else
                                        {
                                            continue;
                                        }
                                    }
                                    if (symbol.Fno != 160)
                                    {
                                        //插入符号坐标表
                                        insertSymbol(symbol.PtClassName, og3e_fid, e, ref oracledb, ref xmldb, ref InsertEnt);
                                        //插入标注坐标表
                                        var lables = symbol.LableClassName.Split(',');
                                        foreach (string lbstr in lables)
                                        {
                                            insertSymbolLb(lbstr, og3e_fid, e, ref oracledb, ref xmldb, ref InsertEnt);
                                        }
                                    }

                                    //插入功能位置表
                                    try
                                    {
                                        if (symbol.Fno != 160 && symbol.Fno != 159)
                                        {
                                            if (!string.IsNullOrEmpty(symbol.Gnwz))
                                            {
                                                Type type2 = TypeCache.Instance.GetTypeFromCache(typeof(DBEntity), symbol.Gnwz);
                                                var gnwz = xmldb.GetEntities(type2).Cast<ElectronBase>().FirstOrDefault(o => o.G3E_FID == og3e_fid);
                                                if (gnwz != null)
                                                {
                                                    gnwz.G3E_ID = OracleSequenceValueGenerator.Instance.GenerateTableId(type2);
                                                    gnwz.G3E_FID = e.G3E_FID;
                                                    InsertEnt.Add(gnwz);
                                                }
                                            }
                                        }
                                    }
                                    catch (Exception ex)
                                    {
                                        if (!(ex.ToString().IndexOf("不存在") >= 0))
                                        {
                                            CYZLog.writeLog(ex.ToString(), "");
                                        }
                                        LogManager.Instance.Error(ex);
                                    }

                                    //插入手电馈线
                                    try
                                    {
                                        if (!string.IsNullOrEmpty(symbol.Gg_Pd_Sdkx_Ac))
                                        {
                                            Type type2 = TypeCache.Instance.GetTypeFromCache(typeof(DBEntity), symbol.Gg_Pd_Sdkx_Ac);
                                            var SdkxList = xmldb.GetEntities(type2).Cast<ElectronBase>().Where(o => o.G3E_FID == og3e_fid);
                                            if (SdkxList != null)
                                            {
                                                int i = 1;
                                                foreach (var v in SdkxList)
                                                {
                                                    long lll = OracleSequenceValueGenerator.Instance.GenerateTableId(type2);
                                                    v.G3E_ID = lll;
                                                    v.G3E_FID = e.G3E_FID;
                                                    ((Gg_pd_sdkx_ac)v).G3E_ID = lll;
                                                    ((Gg_pd_sdkx_ac)v).G3E_CID = i;
                                                    InsertEnt.Add(v);
                                                    i++;
                                                }

                                            }
                                        }
                                    }
                                    catch (Exception ex)
                                    {
                                        if (!(ex.ToString().IndexOf("不存在") >= 0))
                                        {
                                            CYZLog.writeLog(ex.ToString(), "");
                                        }
                                        LogManager.Instance.Error(ex);
                                    }


                                }
                                else if (e.EntityState == EntityState.Update)
                                {
                                    UpdateEnt.Add(e);
                                }
                                else if (e.EntityState == EntityState.Delete)
                                {
                                    DeleteEnt.Add(e);
                                }
                                var inTt = new InsertTempTable();
                                inTt.g3e_fid = e.G3E_FID;
                                inTt.g3e_fno = e.G3E_FNO;
                                inTt.v2 = e;
                                inTt.ltt_name = smzq;
                                inTt.GcTicket_ID = GC_ID;
                                inTt.Sjversion = int.Parse(e.GetValue("Version").ToString());
                                InsertTempTableS.Add(inTt);
                            }
                        }
                    }
                    catch (Exception ex)
                    {
                        if (!(ex.ToString().IndexOf("不存在") >= 0))
                        {
                            CYZLog.writeLog(ex.ToString(), "");
                        }
                        LogManager.Instance.Error(ex);
                    }
                    //
                    //符号坐标表
                    //
                    try
                    {
                        if (!string.IsNullOrEmpty(symbol.PtClassName))
                        {
                            Type type = TypeCache.Instance.GetTypeFromCache(typeof(DBEntity), symbol.PtClassName.Trim());
                            var es = xmldb.GetEntities(type).Cast<ElectronSymbol>();
                            foreach (var e in es)
                            {
                                if (e != null)
                                {
                                    if (e.EntityState == EntityState.Insert)
                                    {
                                    }
                                    else if (e.EntityState == EntityState.Update)
                                    {
                                        //更新符号坐标表  
                                        UpdateEnt.Add(e);

                                        var inTt = new InsertTempTable();
                                        inTt.g3e_fid = e.G3E_FID;
                                        inTt.g3e_fno = e.G3E_FNO;
                                        inTt.v2 = e;
                                        inTt.ltt_name = smzq;
                                        inTt.GcTicket_ID = GC_ID;
                                        inTt.Sjversion = int.Parse(e.GetValue("Version").ToString());
                                        InsertTempTableS.Add(inTt);
                                    }
                                    else if (e.EntityState == EntityState.Delete)
                                    {
                                        //更新符号坐标表  
                                        DeleteEnt.Add(e);

                                        type = TypeCache.Instance.GetTypeFromCache(typeof(DBEntity), symbol.PtClassName.Replace("_sdogeom", ""));
                                        var symb_f = ReflectionUtils.CreateObject(
                                            new
                                            {
                                                G3E_FID = e.G3E_FID
                                            }, type) as ElectronBase;
                                        if (symb_f != null)
                                        {
                                            DeleteEnt.Add(symb_f);
                                        }

                                        var inTt = new InsertTempTable();
                                        inTt.g3e_fid = e.G3E_FID;
                                        inTt.g3e_fno = e.G3E_FNO;
                                        inTt.v2 = e;
                                        inTt.ltt_name = smzq;
                                        inTt.GcTicket_ID = GC_ID;
                                        inTt.Sjversion = int.Parse(e.GetValue("Version").ToString());
                                        InsertTempTableS.Add(inTt);
                                    }
                                }
                            }
                        }
                    }
                    catch (Exception ex)
                    {
                        if (!(ex.ToString().IndexOf("不存在") >= 0))
                        {
                            CYZLog.writeLog(ex.ToString(), "");
                        }
                        LogManager.Instance.Error(ex);
                    }

                    ////
                    ////受电馈线
                    ////
                    try
                    {
                        if (!string.IsNullOrEmpty(symbol.Gg_Pd_Sdkx_Ac))
                        {
                            Type type = TypeCache.Instance.GetTypeFromCache(typeof(DBEntity), symbol.Gg_Pd_Sdkx_Ac.Trim());
                            var es = xmldb.GetEntities(type).Cast<ElectronBase>();
                            foreach (var e in es)
                            {
                                if (e != null)
                                {
                                    if (e.EntityState == EntityState.Insert)
                                    {
                                    }
                                    else if (e.EntityState == EntityState.Update)
                                    {
                                        //更新受电馈线表  
                                        UpdateEnt.Add(e);

                                        var inTt = new InsertTempTable();
                                        inTt.g3e_fid = e.G3E_FID;
                                        inTt.g3e_fno = e.G3E_FNO;
                                        inTt.v2 = e;
                                        inTt.ltt_name = smzq;
                                        inTt.GcTicket_ID = GC_ID;
                                        inTt.Sjversion = int.Parse(e.GetValue("Version").ToString());
                                        InsertTempTableS.Add(inTt);
                                    }
                                    else if (e.EntityState == EntityState.Delete)
                                    {
                                        //更新受电馈线表  
                                        DeleteEnt.Add(e);

                                        type = TypeCache.Instance.GetTypeFromCache(typeof(DBEntity), symbol.Gg_Pd_Sdkx_Ac.Trim());
                                        var symb_f = ReflectionUtils.CreateObject(
                                            new
                                            {
                                                G3E_FID = e.G3E_FID
                                            }, type) as ElectronBase;
                                        if (symb_f != null)
                                        {
                                            DeleteEnt.Add(symb_f);
                                        }

                                        var inTt = new InsertTempTable();
                                        inTt.g3e_fid = e.G3E_FID;
                                        inTt.g3e_fno = e.G3E_FNO;
                                        inTt.v2 = e;
                                        inTt.ltt_name = smzq;
                                        inTt.GcTicket_ID = GC_ID;
                                        inTt.Sjversion = int.Parse(e.GetValue("Version").ToString());
                                        InsertTempTableS.Add(inTt);
                                    }
                                }
                            }
                        }
                    }
                    catch (Exception ex)
                    {
                        if (!(ex.ToString().IndexOf("不存在") >= 0))
                        {
                            CYZLog.writeLog(ex.ToString(), "");
                        }
                        LogManager.Instance.Error(ex);
                    }
                    //标注坐标表
                    //
                    try
                    {
                        if (!string.IsNullOrEmpty(symbol.LableClassName))
                        {
                            var lables = symbol.LableClassName.Split(',');
                            foreach (string lbstr in lables)
                            {    
                                if (!string.IsNullOrEmpty(lbstr))
                                {
                                    Type type = TypeCache.Instance.GetTypeFromCache(typeof(DBEntity), lbstr);
                                    IEnumerable<ElectronSymbol> es = null;
                                    //Type type = TypeCache.Instance.GetTypeFromCache(typeof(DBEntity), lbstr);
                                    try
                                    {

                                        es = xmldb.GetEntities(type).Cast<ElectronSymbol>();
                                    }
                                    catch (Exception ex)
                                    {
                                        continue;
                                    }
                                    foreach (var e in es)
                                    {
                                        if (e != null)
                                        {
                                            
                                            if (e.EntityState == EntityState.Insert)
                                            {
                                                long GisFid = 0;
                                                long GisFid2 = 0;
                                                Cxbz(e.G3E_FID, ref GisFid);
                                                if (GisFid != 0)
                                                {
                                                    e.G3E_ID = OracleSequenceValueGenerator.Instance.GenerateTableId(type);
                                                   
                                                    insertSymbolLb(lbstr, GisFid, e, ref oracledb, ref xmldb, ref InsLb);

                                                }
                                            }
                                            else if (e.EntityState == EntityState.Update)
                                            {
                                                //更新标注坐标表  
                                                UpdateSymbolLb(lbstr, e.G3E_FID, e, ref oracledb, ref xmldb, ref UpdateEnt);

                                                var inTt = new InsertTempTable();
                                                inTt.g3e_fid = e.G3E_FID;
                                                inTt.g3e_fno = e.G3E_FNO;
                                                inTt.v2 = e;
                                                inTt.ltt_name = smzq;
                                                inTt.GcTicket_ID = GC_ID;
                                                inTt.Sjversion = int.Parse(e.GetValue("Version").ToString());
                                                InsertTempTableS.Add(inTt);
                                            }
                                            else if (e.EntityState == EntityState.Delete)
                                            {
                                                //删除标注坐标表  
                                                DeleteEnt.Add(e);
                                                //更新spatial表的数据和工单状态
                                                //StateAreNOEnt.Add(e);
                                                //删除坐标符号表
                                                type = TypeCache.Instance.GetTypeFromCache(typeof(DBEntity), lables[0].Replace("_sdogeom", ""));
                                                var symb_f = ReflectionUtils.CreateObject(
                                                    new
                                                    {
                                                        G3E_FID = e.G3E_FID
                                                    }, type) as ElectronBase;
                                                if (symb_f != null)
                                                {
                                                    DeleteEnt.Add(symb_f);
                                                }

                                                var inTt = new InsertTempTable();
                                                inTt.g3e_fid = e.G3E_FID;
                                                inTt.g3e_fno = e.G3E_FNO;
                                                inTt.v2 = e;
                                                inTt.ltt_name = smzq;
                                                inTt.GcTicket_ID = GC_ID;
                                                inTt.Sjversion = int.Parse(e.GetValue("Version").ToString());
                                                InsertTempTableS.Add(inTt);
                                            }
                                        }
                                    }
                                }
                            }
                        }
                    }
                    catch (Exception ex)
                    {
                        if (!(ex.ToString().IndexOf("不存在") >= 0))
                        {
                            CYZLog.writeLog(ex.ToString(), "");
                        }
                        LogManager.Instance.Error(ex);
                    }
                }

                //根据记录的所有og3e_fid与appendfidship对比那些新增的fid已经被消除了

                string delFids = "";
                Dellist(InsertEnt, GC_ID, ref  DeleteEnt, ref delFids);
                Updalist(ref InsertEnt, ref UpdateEnt, GC_ID, ref InsertEntUp);
                UpdateLblist(InsLb, UpdateEnt,GC_ID);
                var fireFids = InsertEnt.Concat(UpdateEnt).Concat(DeleteEnt).Select(o => o.G3E_FID).Distinct();
                try
                {
                    oracledb.InsertBulk(InsertEnt.Cast<DBEntity>(), true, new ElectronTransferService.UploadEventHandler.ActiveJob(qj, smzq));
                    oracledb.UpdateBulk(UpdateEnt.Cast<DBEntity>(), true, new ElectronTransferService.UploadEventHandler.ActiveJob(qj, smzq));
                    oracledb.DeleteBulk(DeleteEnt.Cast<DBEntity>(), true, new ElectronTransferService.UploadEventHandler.ActiveJob(qj, smzq));
                    oracledb.InsertBulk(InsLb.Cast<DBEntity>(), true, new ElectronTransferService.UploadEventHandler.ActiveJob(qj, smzq));
                }
                catch (Exception ex)
                {
                    CYZLog.writeLog(ex.ToString(), "");
                    throw;
                }

                System.Threading.Thread.Sleep(500);
                foreach (var vfid in fireFids)
                {
                    TickHelper.fireEventShip(vfid.ToString());
                }
                System.Threading.Thread.Sleep(500);
                TickHelper.fireEvent();


                CDBManger dbm = new CDBManger();
                if (delFids != "")
                {
                    dbm.ExecuteNonQuery("delete from cadgis.appendship where g3e_fid in (" + delFids + ") and gcticket_id='" + GC_ID + "' ");
                    dbm.ExecuteNonQuery("delete from cadgis.appendfidship where g3e_fid in (" + delFids + ") and gcticket_id='" + GC_ID + "' ");
                }

                foreach (var appstr in AppendFidShipS)
                {
                    dbm.ExecuteNonQuery(appstr);
                }
                // UpApp(InsertEntUp,GC_ID);
                // UpApp2(UpdateEnt, GC_ID);

                int ltt_idd = 0;
                try
                {

                    ltt_idd = int.Parse(lttid);
                }
                catch { }
                finally
                {
                    if (dbm != null)
                    {
                        dbm.close();
                    }
                }

                foreach (var AppendShip in InsertTempTableS)
                {
                    ElectronTransferService.UploadEventHandler.InsertTempTable(
                        AppendShip.g3e_fid, AppendShip.g3e_fno, AppendShip.v2.GetValue("EntityState").ToString(),
                        AppendShip.GcTicket_ID, ltt_idd, appenshiptime, AppendShip.Sjversion);
                }
            }
           // catch (KeyNotFoundException ex) { }
            catch (Exception ex)
            {
                if (!(ex.ToString().IndexOf("uploadPro 跳出异常") >= 0))
                {
                    CYZLog.writeLog(ex.ToString(), "uploadPro 跳出异常");
                }
                LogManager.Instance.Error(ex);
            }
        }


        /// <summary>
        /// 插入符号表
        /// </summary>
        /// <param name="className"></param>
        /// <param name="og3e_fid"></param>
        /// <param name="e"></param>
        /// <param name="oracledb"></param>
        /// <param name="xmldb"></param>
        /// <param name="InsertEnt"></param>
        public static void insertSymbol(string className, long og3e_fid, ElectronBase e, ref OracleDBManager oracledb, ref XmlDBManager xmldb, ref List<ElectronBase> InsertEnt)
        {
            try
            {
                //插入符号坐标表
                Type type = TypeCache.Instance.GetTypeFromCache(typeof(DBEntity), className);
                var symb = xmldb.GetEntity(type, og3e_fid) as ElectronBase;
                if (symb != null)
                {
                    //插入符号表
                    Type type2 = TypeCache.Instance.GetTypeFromCache(typeof(DBEntity), className.Replace("_sdogeom", ""));
                    var fh_id = OracleSequenceValueGenerator.Instance.GenerateTableId(type2);
                    var symb_f = ReflectionUtils.CreateObject(
                        new
                        {
                            G3E_ID = fh_id,
                            G3E_FID = e.G3E_FID,
                            G3E_CNO =symb.GetValue("G3E_CNO"),
                            G3E_CID = 1L,
                            G3E_FNO = e.G3E_FNO
                        }, type2) as ElectronBase;
                    InsertEnt.Add(symb_f);


                    symb.G3E_ID = OracleSequenceValueGenerator.Instance.GenerateTableId(type);
                    symb.G3E_FID = e.G3E_FID;
                    symb.SetValue("SDO_GID", (Decimal?)fh_id);
                    InsertEnt.Add(symb);
                }
            }
            catch (KeyNotFoundException ex) { }
            catch (Exception ex)
            {
                if (!(ex.ToString().IndexOf("不存在") >= 0))
                {
                    CYZLog.writeLog(ex.ToString(), "");
                }
                LogManager.Instance.Error(ex);
            }
        }

        /// <summary>
        /// 标注的坐标
        /// </summary>
        /// <param name="className"></param>
        /// <param name="og3e_fid"></param>
        /// <param name="e"></param>
        /// <param name="oracledb"></param>
        /// <param name="xmldb"></param>
        /// <param name="InsertEnt"></param>
        public static void insertSymbolLb(string className, long og3e_fid, ElectronBase e, ref OracleDBManager oracledb, ref XmlDBManager xmldb, ref List<ElectronBase> InsertEnt)
        {
            try
            {
                //插入符号坐标表
                Type type = TypeCache.Instance.GetTypeFromCache(typeof(DBEntity), className);
                var symb = xmldb.GetEntity(type, og3e_fid) as ElectronBase;
                if (symb != null)
                {
                    //插入符号表
                    Type type2 = TypeCache.Instance.GetTypeFromCache(typeof(DBEntity), className.Replace("_sdogeom", ""));
                    var fh_id = OracleSequenceValueGenerator.Instance.GenerateTableId(type2);
                    var symb_f = xmldb.GetEntity(type2, og3e_fid) as ElectronBase;
                    if (symb_f != null)
                    {
                        symb_f.G3E_ID = fh_id;
                        symb_f.G3E_FID = e.G3E_FID;
                        InsertEnt.Add(symb_f);
                    }
                   
                    symb.G3E_ID = OracleSequenceValueGenerator.Instance.GenerateTableId(type);
                    symb.G3E_FID = e.G3E_FID;
                    symb.SetValue("SDO_GID", (Decimal?)fh_id);
                    InsertEnt.Add(symb);
                }
            }
            catch (KeyNotFoundException ex) { }
            catch (Exception ex)
            {
                if (!(ex.ToString().IndexOf("不存在") >= 0))
                {
                    CYZLog.writeLog(ex.ToString(), "");
                }
                LogManager.Instance.Error(ex);
            }
        }




        /// <summary>
        /// 标注的lb的修改
        /// </summary>
        /// <param name="className"></param>
        /// <param name="og3e_fid"></param>
        /// <param name="e"></param>
        /// <param name="oracledb"></param>
        /// <param name="xmldb"></param>
        /// <param name="InsertEnt"></param>
        public static void UpdateSymbolLb(string className, long og3e_fid, ElectronBase e, ref OracleDBManager oracledb, ref XmlDBManager xmldb, ref List<ElectronBase> UpdateEnt)
        {
            try
            {
                //插入符号坐标表
                Type type = TypeCache.Instance.GetTypeFromCache(typeof(DBEntity), className);
                var symb = xmldb.GetEntity(type, og3e_fid) as ElectronBase;
                if (symb != null)
                {
                    //插入符号表
                    Type type2 = TypeCache.Instance.GetTypeFromCache(typeof(DBEntity), className.Replace("_sdogeom", ""));
                    var fh_id = OracleSequenceValueGenerator.Instance.GenerateTableId(type2);
                    try
                    {
                        var symb_f = xmldb.GetEntity(type2, og3e_fid) as ElectronBase;
                        if (symb_f != null)
                        {

                            UpdateEnt.Add(symb_f);
                        }

                    }catch(Exception ex){}
                   
                 
                    UpdateEnt.Add(symb);
                }
            }
            catch (KeyNotFoundException ex) { }
            catch (Exception ex)
            {
                if (!(ex.ToString().IndexOf("不存在") >= 0))
                {
                    CYZLog.writeLog(ex.ToString(), "");
                }
                LogManager.Instance.Error(ex);
            }
        }
        /// <summary>
        /// 得到kxarea表的坐标范围
        /// </summary>
        /// <param name="Kxid"></param>
        /// <returns></returns>
        public static string kxZB(string Kxid)
        {
            CDBManger cdb = null;
            string XYzuobiao = "";
            OracleDataReader OdReader = null;
            try
            {
                cdb = new CDBManger();
                string sql = "select KX_XMIN,KX_XMAX,KX_YMIN,KX_YMAX from cadgis.kxarea  where kx_fid=" + long.Parse(Kxid);

                 OdReader = cdb.ExecuteReader(sql);

                if (OdReader != null && OdReader.Read())
                {
                    XYzuobiao = OdReader["KX_XMIN"] + ",";
                    XYzuobiao += OdReader["KX_XMAX"] + ",";
                    XYzuobiao += OdReader["KX_YMIN"] + ",";
                    XYzuobiao += OdReader["KX_YMAX"].ToString();
                }
                else
                {
                    CYZLog.writeLog(Kxid + ":", "在kxarea没有范围");
                }
            }
            catch (Exception ex)
            {
                CYZLog.writeLog(ex.ToString(), "");
            }
            finally
            {
                if (OdReader != null)
                { OdReader.Close(); } 

                if (cdb != null)
                {
                    cdb.close();
                }
            }
            return XYzuobiao;
        }

        /// <summary>
        ///  返回馈线的所属线路的sql语句
        /// </summary>
        /// <param name="KX_ID"></param>
        /// <returns></returns>
        public static string TransKxFidSql(string KX_ID)
        {
            CDBManger dbm = null;
            OracleDataReader dr = null;
            string kxSql = "null";
            string cxbh = "null";
            try
            {
                dbm = new CDBManger();
                string sql = "select cxbh from gg_kxinfo where yx_fid=" + KX_ID;
                dr = dbm.ExecuteReader(sql);
                if (dr != null && dr.Read())
                {
                    cxbh = dr["cxbh"].ToString();

                }
                //kxSql = "where g3e_fid in	(select g3e_fid from b$common_n  t where t.cd_ssxl like '%" + cxbh + "')";
                kxSql = "%" + cxbh;
            }
            catch (Exception ex)
            {
                CYZLog.writeLog(ex.ToString());
            }
            finally
            {
                CloseOracleDataReader(dr, dbm);
            }
            return kxSql;
        }

        /// <summary>
        /// 返回范围的所属线路的sql语句
        /// </summary>
        /// <param name="session_id"></param>
        /// <param name="kxlist"></param>
        /// <returns></returns>
        public static string TransfwFidSql(string session_id,ref string kxlist)
        {
            CDBManger dbm = null;
            string kxSql = "";
            string cxbh = "";
            string KxMclist = null;
            List<string> fwSsxl = new List<string>();
            try
            {
                dbm = new CDBManger();
                string sql = "select distinct cd_ssxl from b$common_n where g3e_fid in (select g3e_fid from " + session_id + ") "
                    + " UNION ALL  select distinct cd_ssxl from b$gg_pd_cbx_n  where g3e_fid in(select g3e_fid from "+session_id+")";
                OracleDataReader dr = dbm.ExecuteReader(sql);
                while (dr != null && dr.Read())
                {
                    cxbh = dr["cd_ssxl"].ToString();
                    fwSsxl.Add(cxbh);

                }
                CloseOracleDataReader(dr, dbm);

                foreach (var VARIABLE in fwSsxl)
                {
                    KxMclist += "'" + VARIABLE + "'" + ",";
                    kxlist+=VARIABLE+",";
                }
                KxMclist = KxMclist.Substring(0, KxMclist.Length - 1);
                kxlist = kxlist.Substring(0, kxlist.Length - 1);
                string sql2 = "select cxbh from gg_kxinfo where  kxmc in(" + KxMclist + ")";
                OracleDataReader dr2 = dbm.ExecuteReader(sql2);

                while (dr2 != null && dr2.Read())
                {
                    cxbh = dr2["cxbh"].ToString();
                    //fwCon.Add(cxbh);
                    kxSql += "%" + cxbh + "<C>";
                }
                CloseOracleDataReader(dr2, dbm);

                if (kxSql != "") {
                    kxSql = kxSql.Substring(0, kxSql.Length - 3);
                }

            }
            catch (Exception ex)
            {
                CYZLog.writeLog(ex.ToString(), "");
            }
            finally
            {
                if (dbm != null) 
                {
                    dbm.close();
                }
            }
            return kxSql;
        }

        /// <summary>
        /// 查询appendfidship表如果有的就不执行插入语句 List<ElectronBase> UpdateEnt 
        /// </summary>
        /// <param name="gisfid"></param>
        /// <param name="gisid"></param>
        /// <returns></returns>
        public static bool QueryAppendfidship(long gisfid, long gisid)
        {
            bool reval = true;
            CDBManger dbm = null;
            OracleDataReader dr = null;
            try
            {
                dbm = new CDBManger();
                string sql = "select * from cadgis.appendfidship where  g3e_fid=" + gisfid;
                dr = dbm.ExecuteReader(sql);
                if (dr.Read() && dr != null)
                {
                    reval = false;
                }

            }
            catch (Exception ex)
            {
                CYZLog.writeLog(ex.ToString(), "");
            }
            finally
            {
                CloseOracleDataReader(dr, dbm);
            }
            return reval;
        }


        /// <summary>
        /// 查询appendfidship表如果有的就不执行插入语句 List<ElectronBase> UpdateEnt 
        /// </summary>
        /// <param name="UpdateEnt"> </param>
        /// <returns></returns>
        public static List<ElectronBase> Uplist(List<ElectronBase> UpdateEnt)
        {
            bool reval = true;
            CDBManger dbm = null;
            OracleDataReader dr = null;
            List<ElectronBase> UpdateFid = new List<ElectronBase>();
            try
            {
                dbm = new CDBManger();

                foreach (var electronBase in UpdateEnt)
                {
                    string UpdateSql = "select g3e_id,g3e_fid from cadgis.appendfidship where g3e_fid2=" + electronBase.G3E_FID + " and g3e_id2=" + electronBase.G3E_ID;
                    dr = dbm.ExecuteReader(UpdateSql);
                    if (dr.Read() && dr != null)
                    {
                        electronBase.G3E_FID = long.Parse(dr["g3e_fid"].ToString());
                        electronBase.G3E_ID = long.Parse(dr["g3e_id"].ToString());
                        UpdateFid.Add(electronBase);
                    }
                    else
                    {
                        UpdateFid.Add(electronBase);
                    }

                }
            }
            catch (Exception ex)
            {
                CYZLog.writeLog(ex.ToString(), "");
            }
            finally
            {
                CloseOracleDataReader(dr, dbm);
            }
            return UpdateFid;
        }

        /// <summary>
        /// 查询appendfidship表新增删除的数据
        /// </summary>
        /// <param name="InsDateEnt"> </param>
        /// <param name="gc_id"> </param>
        /// <param name="delDateEnt"> </param>
        /// <param name="delFids"> </param>
        /// <returns></returns>
        public static void Dellist(List<ElectronBase> InsDateEnt, string gc_id, ref  List<ElectronBase> delDateEnt, ref string delFids)
        {
            bool reval = true;
            CDBManger dbm = null;
            string str = null;
            Dictionary<string, List<string>> Fno_Fids = new Dictionary<string, List<string>>();

            delFids = "";
            
            string symboldir = Path.Combine(Assembly.GetExecutingAssembly().GetPhysicalDirectory(), "SymbolConfig.xml");
            SimpleSymbolConfig.Instance = XmlSerializeUtils.Load<SimpleSymbolConfig>(symboldir, new Type[] { });

            try
            {
                var AllFids = InsDateEnt.Select(o => o.G3E_FID).Distinct();

                if (AllFids.Any())
                {
                    foreach (var v in AllFids)
                    {
                        str += v + ",";
                    }
                    str = str.Substring(0, str.Length - 1);
                }
                string AllFidsSql = "select g3e_fid,g3e_fno from cadgis.appendfidship where gcticket_id='" + gc_id + "' and g3e_fid not in(" + str + ")";

                dbm = new CDBManger();
                OracleDataReader dr = dbm.ExecuteReader(AllFidsSql);
                while (dr.Read() && dr != null)
                {
                    if (Fno_Fids.Keys.Contains(dr["g3e_fno"].ToString()))
                    {
                        Fno_Fids[dr["g3e_fno"].ToString()].Add(dr["g3e_fid"].ToString());
                    }
                    else
                    {
                        var fidsvar = new List<string>();
                        fidsvar.Add(dr["g3e_fid"].ToString());
                        Fno_Fids.Add(dr["g3e_fno"].ToString(), fidsvar);
                    }
                    delFids += dr["g3e_fid"] + ",";
                }
                CloseOracleDataReader(dr, dbm);

                if (delFids == "")
                    return;

                delFids = delFids.Substring(0, delFids.Length - 1);

                foreach (var sy in SimpleSymbolConfig.Instance.Symbols)
                {
                    if (Fno_Fids.Keys.Contains(sy.Fno.ToString()))
                    {
                        foreach (string fidTemp in Fno_Fids[sy.Fno.ToString()])
                        {
                            if (sy.Fno == 141)
                            {
                                if (sy.Contain != null)
                                {
                                    Type typeContain = TypeCache.Instance.GetTypeFromCache(typeof(DBEntity), sy.Contain);
                                    DelCreDxFid(ref delDateEnt, long.Parse(fidTemp), typeContain);
                                }

                            }
                            if (sy.Fno == 201)
                            {
                                long g3e_id = 0;
                                long g3e_fid = 0;
                                if (sy.Contain != null)
                                {
                                    Type typeContain = TypeCache.Instance.GetTypeFromCache(typeof(DBEntity), sy.Contain);
                                    QueryContainOwner2(long.Parse(fidTemp), ref g3e_id);
                                    DelCrecon(ref delDateEnt, long.Parse(fidTemp), typeContain, g3e_id);
                                    g3e_id = 0;
                                    QueryContainOwner(long.Parse(fidTemp), ref g3e_id, ref g3e_fid);
                                    DelCrecon2(ref delDateEnt, g3e_fid, typeContain, g3e_id);
                                }
                            }

                            if (sy.PtClassName != null)
                            {
                                Type typePtClassName = TypeCache.Instance.GetTypeFromCache(typeof(DBEntity), sy.PtClassName);
                                DelCreFid(ref delDateEnt, long.Parse(fidTemp), typePtClassName);
                            }

                            if (sy.Common != null)
                            {
                                Type typeCom = TypeCache.Instance.GetTypeFromCache(typeof(DBEntity), sy.Common);
                                DelCreFid(ref delDateEnt, long.Parse(fidTemp), typeCom);
                            }

                            if (sy.SelfAttribute != null)
                            {
                                Type typeSelf = TypeCache.Instance.GetTypeFromCache(typeof(DBEntity), sy.SelfAttribute);
                                DelCreFid(ref delDateEnt, long.Parse(fidTemp), typeSelf);
                            }
                            try
                            {
                                if (sy.Connectivity != null)
                                {
                                    Type typecon = TypeCache.Instance.GetTypeFromCache(typeof(DBEntity), sy.Connectivity);
                                    DelCreFid(ref delDateEnt, long.Parse(fidTemp), typecon);
                                }
                            }
                            catch (Exception ex)
                            {
                                CYZLog.writeLog(ex.ToString(), "");
                            }

                            try
                            {
                                if (sy.Gnwz != null)
                                {
                                    Type typegnwz = TypeCache.Instance.GetTypeFromCache(typeof(DBEntity), sy.Gnwz);
                                    DelCreFid(ref delDateEnt, long.Parse(fidTemp), typegnwz);
                                }
                            }
                            catch (Exception ex)
                            {
                                CYZLog.writeLog(ex.ToString(), "");
                            }


                            foreach (var VARIABLE in sy.LableClassName.Split(','))
                            {

                                if (VARIABLE != null)
                                {
                                    Type typelab = TypeCache.Instance.GetTypeFromCache(typeof(DBEntity), VARIABLE);
                                    DelCreFid(ref delDateEnt, long.Parse(fidTemp), typelab);
                                }

                            }

                            if (sy.Gg_Pd_Sdkx_Ac != null)
                            {
                                Type typekxh = TypeCache.Instance.GetTypeFromCache(typeof(DBEntity), sy.Gg_Pd_Sdkx_Ac);
                                DelCreFid(ref delDateEnt, long.Parse(fidTemp), typekxh);
                            }
                            if (sy.Fno == 148 || sy.Fno == 159 || sy.Fno == 84)
                            {
                                if (sy.Detailreference != null)
                                {
                                    Type typeDeta = TypeCache.Instance.GetTypeFromCache(typeof(DBEntity), sy.Detailreference);
                                    DelCreFid(ref delDateEnt, long.Parse(fidTemp), typeDeta);
                                }
                            }


                            foreach (var VARIABLE in sy.LableClassNameLb.Split(','))
                            {

                                if (VARIABLE != null)
                                {
                                    Type typeLb = TypeCache.Instance.GetTypeFromCache(typeof(DBEntity), VARIABLE);
                                    DelCreFid(ref delDateEnt, long.Parse(fidTemp), typeLb);
                                }

                            }

                        }
                    }
                }
            }
            catch (Exception ex)
            {
                CYZLog.writeLog(ex.ToString(), "");
            }
            finally
            {
                dbm.close();
            }

        }


        /// <summary>
        /// contain_n电杆如果是新增的数据数据库已经存在
        /// </summary>
        /// <param name="ownerfid"> </param>
        /// <param name="gisid"> </param>
        /// <param name="gisfid"> </param>
        /// <returns></returns>
        public static void QueryContainOwner(long? ownerfid, ref long gisid,ref long gisfid)
        {

            CDBManger cdm_QueryContainFIdTemp = null;
            try
            {
                if (ownerfid == null || ownerfid == 0)
                {
                    return;
                }
                cdm_QueryContainFIdTemp = new CDBManger();
                string sql = "select g3e_id,g3e_fid from gzdlgis.b$contain_n  where g3e_ownerfid=" + ownerfid + " and ltt_status='ADD'";
                DataTable dtQueryContainFId = cdm_QueryContainFIdTemp.ExecuteTable(sql);
                if (dtQueryContainFId != null && dtQueryContainFId.Rows.Count > 0)
                {
                    gisid = long.Parse(dtQueryContainFId.Rows[0]["g3e_id"].ToString());
                    gisfid = long.Parse(dtQueryContainFId.Rows[0]["g3e_fid"].ToString());


                }

            }
            catch (Exception ex)
            {
                CYZLog.writeLog(ex.ToString(), "");
            }
            finally
            {
                CloseOracleConnection(cdm_QueryContainFIdTemp);
            }
        }

        /// <summary>
        /// contain_n电杆如果是新增的数据数据库已经存在
        /// </summary>
        /// <param name="g3e_fid"> </param>
        /// <param name="gisid"> </param>
        /// <returns></returns>
        public static void QueryContainOwner2(long? g3e_fid, ref long gisid)
        {

            CDBManger cdm_QueryContainFIdTemp = null;
            try
            {
                if (g3e_fid == null || g3e_fid == 0)
                {
                    return;
                }
                cdm_QueryContainFIdTemp = new CDBManger();
                string sql = "select g3e_id from gzdlgis.b$contain_n  where g3e_fid=" + g3e_fid + " and ltt_status='ADD'";
                DataTable dtQueryContainFId = cdm_QueryContainFIdTemp.ExecuteTable(sql);
                if (dtQueryContainFId != null && dtQueryContainFId.Rows.Count > 0)
                {
                    gisid = long.Parse(dtQueryContainFId.Rows[0]["g3e_id"].ToString());

                }

            }
            catch (Exception ex)
            {
                CYZLog.writeLog(ex.ToString(), "");
            }
            finally
            {
                CloseOracleConnection(cdm_QueryContainFIdTemp);
            }

        }


        /// <summary>
        /// 如果是已经存在的新增数据那么就要把已经存在的新增数据放到list集合
        /// </summary>
        /// <param name="InsDateEnt"> </param>
        /// <param name="UpDateEnt"> </param>
        /// <param name="gc_id"> </param>
        /// <param name="InsDateEntCopy"> </param>
        /// <returns></returns>
        public static void Updalist(ref  List<ElectronBase> InsDateEnt, ref  List<ElectronBase> UpDateEnt, string gc_id, ref List<ElectronBase> InsDateEntCopy)
        {
            CDBManger dbm = null;
            OracleDataReader dr = null;
            //List<ElectronBase> InsDateEntCopy = new List<ElectronBase>();

            try
            {
                Dictionary<long,List<ElectronBase>> upTemp=new Dictionary<long,List<ElectronBase>>();              

                string fids = "";
                foreach (var VarFid in InsDateEnt)
                {
                    fids += VarFid.G3E_FID + ",";
                    if(upTemp.Keys.Contains(VarFid.G3E_FID))
                    {
                        upTemp[VarFid.G3E_FID].Add(VarFid);
                    }
                    else
                    {
                        List<ElectronBase> ttlist=new List<ElectronBase>();
                        ttlist.Add(VarFid);
                        upTemp.Add(VarFid.G3E_FID,ttlist);
                    }
                }
                if (fids == "")
                    return;

                fids = fids.Substring(0, fids.Length - 1);
                string AllFidsSql = "select g3e_fid from cadgis.appendfidship where gcticket_id='" + gc_id + "' and g3e_fid  in(" + fids + ")";
                dbm = new CDBManger();
                dr = dbm.ExecuteReader(AllFidsSql);
                while (dr.Read())
                {
                    if(upTemp.Keys.Contains(long.Parse(dr["G3E_FID"].ToString())))
                    {
                        foreach (var vv in upTemp[long.Parse(dr["G3E_FID"].ToString())])
                        {
                            InsDateEntCopy.Add(vv);
                            UpDateEnt.Add(vv);
                        }
                    }
                }

                foreach (var VARIABLE in InsDateEntCopy)
                {
                    InsDateEnt.Remove(VARIABLE);
                }
              
            }
            catch (Exception ex)
            {

                CYZLog.writeLog(ex.ToString(), "");
            }
            finally
            {
                CloseOracleDataReader(dr, dbm);
            }
        }

        /// <summary>
        /// 这个方法待用
        /// </summary>
        /// <param name="InsDateEntCopy"> </param>
        /// <param name="gc_id"> </param>
        /// <returns></returns>
        public static void UpApp(List<ElectronBase> InsDateEntCopy, string gc_id)
        {
            CDBManger cdb = null;
            int kxid = 0;
            string ssdw = "";
            string sql = null;
            try
            {

                cdb = new CDBManger();
                foreach (var g3efid in InsDateEntCopy)
                {
                    if (g3efid.G3E_FNO == 160)
                    {
                        sql = "select cd_ssxl from b$gg_pd_dyshb_n t where  t.g3e_fid=" + g3efid.G3E_FID + " and ltt_status='ADD'";
                    }
                    else if (g3efid.G3E_FNO == 159)
                    {
                        sql = "select cd_ssxl from b$gg_pd_cbx_n t where t.g3e_fid=" + g3efid.G3E_FID + " and ltt_status='ADD'";
                    }
                    else
                    {
                        sql = "select t.cd_ssxl from b$common_n t where t.g3e_fid=" + g3efid.G3E_FID + " and ltt_status='ADD'";
                    }

                    string sql2 = "select distinct a.yx_fid,b.cd_ssdw from Gg_Kxinfo a,b$common_n  b where a.yx_fid=b.g3e_fid  and a.kxmc in ( " + sql + ")";
                    DataTable dtYXFID = cdb.ExecuteTable(sql2);
                    if (dtYXFID != null && dtYXFID.Rows.Count > 0)
                    {
                        kxid = int.Parse(dtYXFID.Rows[0]["yx_fid"].ToString());
                        ssdw = dtYXFID.Rows[0]["cd_ssdw"].ToString();

                        string strSql3 = "update cadgis.appendship set kx_fid=" + kxid + " where gcticket_id='" + gc_id + "' and g3e_fid=" + g3efid.G3E_FID;
                        string strSql4 = "update cadgis.appendship set ssdw='" + ssdw + "'" + " where gcticket_id='" + gc_id + "' and g3e_fid=" + g3efid.G3E_FID;
                        cdb.ExecuteNonQuery(strSql3);
                        cdb.ExecuteNonQuery(strSql4);
                    }
                }

            }
            catch (Exception ex)
            {
                CYZLog.writeLog(ex.ToString(), "");
            }
            finally 
            {
                if (cdb != null) 
                {
                    cdb.close();
                }
            }


        }

        /// <summary>
        /// 如果是新增的变成修改的中间的所属线路可能改变所以要重新修改下增量的所属线路这个是update的状态
        /// </summary>
        /// <param name="updateEntCopy"> </param>
        /// <param name="gc_id"> </param>
        /// <returns></returns>
        public static void UpApp2(List<ElectronBase> updateEntCopy, string gc_id)
        {
            CDBManger cdb = null;
            int kxid = 0;
            string ssdw = "";
            string sql = null;
            try
            {

                cdb = new CDBManger();
                foreach (var g3efid in updateEntCopy)
                {
                    if (g3efid.G3E_FNO == 160)
                    {
                        sql = "select cd_ssxl from b$gg_pd_dyshb_n t where  t.g3e_fid=" + g3efid.G3E_FID + " and ltt_status='EDIT'";
                    }
                    else if (g3efid.G3E_FNO == 159)
                    {
                        sql = "select cd_ssxl from b$gg_pd_cbx_n t where t.g3e_fid=" + g3efid.G3E_FID + " and ltt_status='EDIT'";
                    }
                    else
                    {
                        sql = "select t.cd_ssxl from b$common_n t where t.g3e_fid=" + g3efid.G3E_FID + " and ltt_status='EDIT'";
                    }

                    string sql2 = "select distinct a.yx_fid,b.cd_ssdw from Gg_Kxinfo a,b$common_n  b where a.yx_fid=b.g3e_fid  and a.kxmc in ( " + sql + ")";
                    DataTable dtYXFID = cdb.ExecuteTable(sql2);
                    if (dtYXFID != null && dtYXFID.Rows.Count > 0)
                    {
                        kxid = int.Parse(dtYXFID.Rows[0]["yx_fid"].ToString());
                        ssdw = dtYXFID.Rows[0]["cd_ssdw"].ToString();

                        string strSql3 = "update cadgis.appendship set kx_fid=" + kxid + " where gcticket_id='" + gc_id + "' and g3e_fid=" + g3efid.G3E_FID;
                        string strSql4 = "update cadgis.appendship set set ssdw='" + ssdw + "'" + " where gcticket_id='" + gc_id + "' and g3e_fid=" + g3efid.G3E_FID;
                        cdb.ExecuteNonQuery(strSql3);
                        cdb.ExecuteNonQuery(strSql4);
                    }
                }

            }
            catch (Exception ex)
            {
                CYZLog.writeLog(ex.ToString(), "");
            }
            finally 
            {
                if (cdb != null) 
                {
                    cdb.close();
                }
            }


        }

        /// <summary>
        /// 查找已经在xml文件里面消失的增量创建并且删除
        /// </summary>
        /// <param name="delDateEnt"></param>
        /// <param name="g3e_fid"></param>
        /// <param name="ClassName"></param>
        public static void DelCreFid(ref  List<ElectronBase> delDateEnt, long g3e_fid, Type ClassName)
        {
            var symb_f = ReflectionUtils.CreateObject(
                                   new
                                   {
                                       G3E_FID = g3e_fid
                                   }, ClassName) as ElectronBase;
            if (symb_f != null)
            {
                delDateEnt.Add(symb_f);
            }
        }



        /// <summary>
        /// 导线需要特殊处理
        /// </summary>
        /// <param name="delDateEnt"></param>
        /// <param name="g3e_fid"></param>
        /// <param name="ClassName"></param>
        public static void DelCreDxFid(ref  List<ElectronBase> delDateEnt, long g3e_fid, Type ClassName)
        {
            CDBManger AppCadFid = null;
            OracleDataReader AppCaddr = null;
            long OldGisFid = 0;
            long OldGisid = 0;

            try
            {
                AppCadFid = new CDBManger();
                string AppCadSql = "select g3e_id,g3e_fid from gzdlgis.b$contain_n t  where t.g3e_fid=" + g3e_fid;
                AppCaddr = AppCadFid.ExecuteReader(AppCadSql);
                while (AppCaddr.Read())
                {
                    OldGisFid = long.Parse(AppCaddr["g3e_fid"].ToString());
                    OldGisid = long.Parse(AppCaddr["g3e_id"].ToString());
                    var symb_f = ReflectionUtils.CreateObject(
                                   new
                                   {
                                       g3e_fid = OldGisFid,
                                       G3E_ID = OldGisid
                                   }, ClassName) as ElectronBase;
                    if (symb_f != null)
                    {
                        symb_f.G3E_ID = OldGisid;
                        symb_f.G3E_FID = OldGisFid;
                        delDateEnt.Add(symb_f);
                    }


                }


            }
            catch (Exception ex)
            {
                CYZLog.writeLog(ex.ToString(), "");
            }
            finally
            {
                CloseOracleDataReader(AppCaddr, AppCadFid);
            }
          
        }

        
        /// <summary>
        /// 删除包含关系
        /// </summary>
        /// <param name="delDateEnt"></param>
        /// <param name="g3e_fid"></param>
        /// <param name="ClassName"></param>
        /// <param name="g3e_id"></param>
        public static void DelCrecon(ref  List<ElectronBase> delDateEnt, long g3e_fid, Type ClassName,long g3e_id)
        {
            var symb_f = ReflectionUtils.CreateObject(
                                   new
                                   {   g3e_fid=g3e_fid,
                                       G3E_ID =g3e_id
                                   }, ClassName) as ElectronBase;
            if (symb_f != null)
            {
                symb_f.G3E_ID = g3e_id;
                symb_f.G3E_FID =g3e_fid;
                delDateEnt.Add(symb_f);
            }
        }


        public static void DelCrecon2(ref  List<ElectronBase> delDateEnt, long g3e_fid, Type ClassName, long g3e_id)
        {
            var symb_f = ReflectionUtils.CreateObject(
                                   new
                                   {
                                       g3e_fid = g3e_fid,
                                       G3E_ID = g3e_id
                                   }, ClassName) as ElectronBase;
            if (symb_f != null)
            {
                symb_f.G3E_ID = g3e_id;
                symb_f.G3E_FID = g3e_fid;
                delDateEnt.Add(symb_f);
            }
        }

        /// <summary>
        /// 查询appendfidship表新增删除的数据
        /// </summary>
        /// <param name="CadFid"></param>
        /// <param name="gc_id"></param>
        /// <param name="OldGisFid"></param>
        /// <param name="OldGisid"></param>
        /// <returns></returns>
        public static bool ReFid(long CadFid, string gc_id, ref long OldGisFid, ref long OldGisid)
        {
            bool rel = false;
            CDBManger AppCadFid = null;
            OracleDataReader AppCaddr = null;
            try
            {
                //1.如果在appendfidship找不到og3e_fid,那么就可以在gis里面新增fid
                AppCadFid = new CDBManger();
                string AppCadSql = "select g3e_id,g3e_fid from cadgis.appendfidship t  where t.gcticket_id='" + gc_id + "' and  g3e_fid2=" + CadFid;
                AppCaddr = AppCadFid.ExecuteReader(AppCadSql);
                if (AppCaddr.Read() && AppCaddr != null)
                {
                    OldGisFid = long.Parse(AppCaddr["g3e_fid"].ToString());
                    OldGisid = long.Parse(AppCaddr["g3e_id"].ToString());
                    rel = true;
                }
                else
                {
                    OldGisFid = 0;
                    OldGisid = 0;
                    rel = false;
                }

            }
            catch (Exception ex)
            {
                CYZLog.writeLog(ex.ToString(), "");
            }
            finally
            {
                CloseOracleDataReader(AppCaddr, AppCadFid);
            }
            return rel;
        }


        /// <summary>
        /// 如果是第一次导入新增量就用是原来的方法
        /// </summary>
        /// <param name="oracledb"></param>
        /// <param name="xmldb"></param>
        /// <param name="smzq"></param>
        /// <param name="GC_ID"></param>
        /// <param name="appenshiptime"></param>
        /// <summary/>
        public static void uploadPro2(OracleDBManager oracledb, XmlDBManager xmldb, string smzq, string GC_ID, DateTime appenshiptime,string qj)
        {
            try
            {
                List<string> AppendFidShipS = new List<string>();
                //List<ElectronBase> StateAreNOEnt = new List<ElectronBase>();
                List<InsertTempTable> InsertTempTableS = new List<InsertTempTable>();
                List<ElectronBase> InsertEntSpatial = new List<ElectronBase>();
                List<ElectronBase> InsertEnt = new List<ElectronBase>();
                List<ElectronBase> UpdateEnt = new List<ElectronBase>();
                List<ElectronBase> DeleteEnt = new List<ElectronBase>();
                List<ElectronBase> InsertLB = new List<ElectronBase>();
                //CDBManger cdb = null;
                if (oracledb == null) { return; }
                if (xmldb == null) { return; }
                if (string.IsNullOrEmpty(smzq)) { return; }
                if (string.IsNullOrEmpty(GC_ID)) { return; }

                OracleSequenceValueGenerator.Instance.DbManager = oracledb;

                string symboldir = Path.Combine(Assembly.GetExecutingAssembly().GetPhysicalDirectory(), "SymbolConfig.xml");
                SimpleSymbolConfig.Instance = XmlSerializeUtils.Load<SimpleSymbolConfig>(symboldir, new Type[] { });

                //List<string> clearSpatialStr = new List<string>();
                CDBManger cdm = new CDBManger();
                string lttid = "";
                string str = "select gisticket_id from cadgis.ticketship t  where t.gcticket_id='" + GC_ID + "'";
                OracleDataReader dr = cdm.ExecuteReader(str);
                if (dr != null && dr.Read() && dr["gisticket_id"] != null)
                {
                    lttid = dr["gisticket_id"].ToString();
                }
                CloseOracleDataReader(dr, cdm);


                foreach (var symbol in SimpleSymbolConfig.Instance.Symbols)
                {
                    if (symbol.Fno == 169)
                    {
                        CYZLog.writeLog("", "");
                    }

                    //
                    //自身属性表
                    //
                    try
                    {

                        //72接地挂环    没有自身属性表
                        //174无功补偿器 没有自身属性表
                        if (!string.IsNullOrEmpty(symbol.SelfAttribute))
                        {
                            Type type = TypeCache.Instance.GetTypeFromCache(typeof(DBEntity), symbol.SelfAttribute);
                            var es = xmldb.GetEntities(type).Cast<ElectronBase>();


                            foreach (var e in es)
                            {
                               
                                if (e == null) { continue; }
                                //148有多个标注， lb 14803 ， lb1 14815 ，lb2 14804
                                //172  17201符号，17202标注
                                //176  17601符号，17602标注
                                if (e.EntityState == EntityState.Insert)
                                {
                                    long og3e_fid = e.G3E_FID;
                                    long Old_G3e_id = e.G3E_ID;
                                    //
                                    //开始插入自身属性
                                    //
                                    e.G3E_ID = OracleSequenceValueGenerator.Instance.GenerateTableId(type);
                                    e.G3E_FID = OracleSequenceValueGenerator.Instance.GenerateGlobalId();

                                    InsertEnt.Add(e);
                                    //
                                    //开始插入公共属性
                                    //                                
                                    if (e.G3E_FNO == 159 || e.G3E_FNO == 160 || e.G3E_FNO == 250)
                                    {
                                        AppendFidShipS.Add("insert into cadgis.AppendFidShip(G3E_FID,G3E_FID2,GCTICKET_ID,G3E_ID,G3E_ID2,VERSION,G3E_FNO) values(" + e.G3E_FID + "," + og3e_fid + ",'"
                                            + GC_ID + "', " + e.G3E_ID + "," + Old_G3e_id + "," + e.GetValue("Version") + "," + e.G3E_FNO + ")");
                                    }

                                    else
                                    {
                                        Type type1 = TypeCache.Instance.GetTypeFromCache(typeof(DBEntity), symbol.Common);
                                        var com = xmldb.GetEntities(type1).Cast<ElectronBase>().FirstOrDefault(o => o.G3E_FID == og3e_fid);
                                        if (com != null)
                                        {
                                            com.G3E_ID = OracleSequenceValueGenerator.Instance.GenerateTableId(type1);
                                            com.G3E_FID = e.G3E_FID;
                                            InsertEnt.Add(com);
                                            AppendFidShipS.Add("insert into cadgis.AppendFidShip(G3E_FID,G3E_FID2,GCTICKET_ID,G3E_ID,G3E_ID2,VERSION,G3E_FNO) values(" + e.G3E_FID + "," + og3e_fid + ",'"
                                            + GC_ID + "', " + com.G3E_ID + "," + Old_G3e_id + "," + e.GetValue("Version") + "," + e.G3E_FNO + ")");
                                        }
                                        else
                                        {
                                            continue;
                                        }
                                    }
                                    if (symbol.Fno != 160)
                                    {
                                        //插入符号坐标表
                                        insertSymbol(symbol.PtClassName, og3e_fid, e, ref oracledb, ref xmldb, ref InsertEnt);
                                        //插入标注坐标表
                                        var lables = symbol.LableClassName.Split(',');
                                        foreach (string lbstr in lables)
                                        {
                                            insertSymbolLb(lbstr, og3e_fid, e, ref oracledb, ref xmldb, ref InsertEnt);
                                        }
                                    }

                                    //插入功能位置表
                                    try
                                    {
                                        if (symbol.Fno != 160 && symbol.Fno != 159)
                                        {
                                            if (!string.IsNullOrEmpty(symbol.Gnwz))
                                            {
                                                Type type2 = TypeCache.Instance.GetTypeFromCache(typeof(DBEntity), symbol.Gnwz);
                                                var gnwz = xmldb.GetEntities(type2).Cast<ElectronBase>().FirstOrDefault(o => o.G3E_FID == og3e_fid);
                                                if (gnwz != null)
                                                {

                                                    gnwz.G3E_ID = OracleSequenceValueGenerator.Instance.GenerateTableId(type2);
                                                    gnwz.G3E_FID = e.G3E_FID;
                                                    InsertEnt.Add(gnwz);


                                                }
                                            }
                                        }
                                    }
                                    catch (Exception ex)
                                    {
                                        if (!(ex.ToString().IndexOf("不存在") >= 0))
                                        {
                                            CYZLog.writeLog(ex.ToString(), "");
                                        }
                                        LogManager.Instance.Error(ex);
                                    }
                                    //插入手电馈线
                                    try
                                    {
                                        if (!string.IsNullOrEmpty(symbol.Gg_Pd_Sdkx_Ac))
                                        {
                                            Type type2 = TypeCache.Instance.GetTypeFromCache(typeof(DBEntity), symbol.Gg_Pd_Sdkx_Ac);
                                            var SdkxList = xmldb.GetEntities(type2).Cast<ElectronBase>().Where(o => o.G3E_FID == og3e_fid);
                                            if (SdkxList != null)
                                            {
                                                long i = 1;
                                                foreach (var v in SdkxList)
                                                {
                                                    long lll = OracleSequenceValueGenerator.Instance.GenerateTableId(type2);
                                                    v.G3E_ID = lll;
                                                    v.G3E_FID = e.G3E_FID;
                                                    ((Gg_pd_sdkx_ac)v).G3E_ID = lll;
                                                    ((Gg_pd_sdkx_ac)v).G3E_CID = i;
                                                    InsertEnt.Add(v);
                                                    i++;
                                                }

                                            }
                                        }

                                    }
                                    catch (Exception ex)
                                    {
                                        if (!(ex.ToString().IndexOf("不存在") >= 0))
                                        {
                                            CYZLog.writeLog(ex.ToString(), "");
                                        }
                                        LogManager.Instance.Error(ex);
                                    }
                                }
                                else if (e.EntityState == EntityState.Update)
                                {
                                    UpdateEnt.Add(e);
                                }
                                else if (e.EntityState == EntityState.Delete)
                                {
                                    DeleteEnt.Add(e);
                                }
                                var inTt = new InsertTempTable();
                                inTt.g3e_fid = e.G3E_FID;
                                inTt.g3e_fno = e.G3E_FNO;
                                inTt.v2 = e;
                                inTt.ltt_name = smzq;
                                inTt.GcTicket_ID = GC_ID;
                                inTt.Sjversion = int.Parse(e.GetValue("Version").ToString());
                                InsertTempTableS.Add(inTt);
                            }
                        }
                    }
                    catch (Exception ex)
                    {
                        if (!(ex.ToString().IndexOf("不存在") >= 0))
                        {
                            CYZLog.writeLog(ex.ToString(), "");
                        }
                        LogManager.Instance.Error(ex);
                    }
                    //
                    //符号坐标表
                    //
                    try
                    {
                        if (!string.IsNullOrEmpty(symbol.PtClassName))
                        {
                            Type type = TypeCache.Instance.GetTypeFromCache(typeof(DBEntity), symbol.PtClassName.Trim());
                            var es = xmldb.GetEntities(type).Cast<ElectronSymbol>();
                            foreach (var e in es)
                            {
                                if (e != null)
                                {
                                    if (e.EntityState == EntityState.Insert)
                                    {
                                    }
                                    else if (e.EntityState == EntityState.Update)
                                    {
                                        //更新符号坐标表  
                                        UpdateEnt.Add(e);

                                        var inTt = new InsertTempTable();
                                        inTt.g3e_fid = e.G3E_FID;
                                        inTt.g3e_fno = e.G3E_FNO;
                                        inTt.v2 = e;
                                        inTt.ltt_name = smzq;
                                        inTt.GcTicket_ID = GC_ID;
                                        inTt.Sjversion = int.Parse(e.GetValue("Version").ToString());
                                        InsertTempTableS.Add(inTt);
                                    }
                                    else if (e.EntityState == EntityState.Delete)
                                    {
                                        //更新符号坐标表  
                                        DeleteEnt.Add(e);

                                        type = TypeCache.Instance.GetTypeFromCache(typeof(DBEntity), symbol.PtClassName.Replace("_sdogeom", ""));
                                        var symb_f = ReflectionUtils.CreateObject(
                                            new
                                            {
                                                G3E_FID = e.G3E_FID
                                            }, type) as ElectronBase;
                                        if (symb_f != null)
                                        {
                                            DeleteEnt.Add(symb_f);
                                        }

                                        var inTt = new InsertTempTable();
                                        inTt.g3e_fid = e.G3E_FID;
                                        inTt.g3e_fno = e.G3E_FNO;
                                        inTt.v2 = e;
                                        inTt.ltt_name = smzq;
                                        inTt.GcTicket_ID = GC_ID;
                                        inTt.Sjversion = int.Parse(e.GetValue("Version").ToString());
                                        InsertTempTableS.Add(inTt);
                                    }
                                }
                            }
                        }
                    }
                    catch (Exception ex)
                    {
                        if (!(ex.ToString().IndexOf("不存在") >= 0))
                        {
                            CYZLog.writeLog(ex.ToString(), "");
                        }
                        LogManager.Instance.Error(ex);
                    }
                    ////
                    ////受电馈线
                    ////
                    try
                    {
                        if (!string.IsNullOrEmpty(symbol.Gg_Pd_Sdkx_Ac))
                        {
                            Type type = TypeCache.Instance.GetTypeFromCache(typeof(DBEntity), symbol.Gg_Pd_Sdkx_Ac.Trim());
                            var es = xmldb.GetEntities(type).Cast<ElectronBase>();
                            foreach (var e in es)
                            {
                                if (e != null)
                                {
                                    if (e.EntityState == EntityState.Insert)
                                    {
                                    }
                                    else if (e.EntityState == EntityState.Update)
                                    {
                                        //更新受电馈线表  
                                        UpdateEnt.Add(e);

                                        var inTt = new InsertTempTable();
                                        inTt.g3e_fid = e.G3E_FID;
                                        inTt.g3e_fno = e.G3E_FNO;
                                        inTt.v2 = e;
                                        inTt.ltt_name = smzq;
                                        inTt.GcTicket_ID = GC_ID;
                                        inTt.Sjversion = int.Parse(e.GetValue("Version").ToString());
                                        InsertTempTableS.Add(inTt);
                                    }
                                    else if (e.EntityState == EntityState.Delete)
                                    {
                                        //更新受电馈线表  
                                        DeleteEnt.Add(e);

                                        type = TypeCache.Instance.GetTypeFromCache(typeof(DBEntity), symbol.Gg_Pd_Sdkx_Ac.Trim());
                                        var symb_f = ReflectionUtils.CreateObject(
                                            new
                                            {
                                                G3E_FID = e.G3E_FID
                                            }, type) as ElectronBase;
                                        if (symb_f != null)
                                        {
                                            DeleteEnt.Add(symb_f);
                                        }

                                        var inTt = new InsertTempTable();
                                        inTt.g3e_fid = e.G3E_FID;
                                        inTt.g3e_fno = e.G3E_FNO;
                                        inTt.v2 = e;
                                        inTt.ltt_name = smzq;
                                        inTt.GcTicket_ID = GC_ID;
                                        inTt.Sjversion = int.Parse(e.GetValue("Version").ToString());
                                        InsertTempTableS.Add(inTt);
                                    }
                                }
                            }
                        }
                    }
                    catch (Exception ex)
                    {
                        if (!(ex.ToString().IndexOf("不存在") >= 0))
                        {
                            CYZLog.writeLog(ex.ToString(), "");
                        }
                        LogManager.Instance.Error(ex);
                    }
                    //
                    //标注坐标表
                    //
                    try
                    {
                        if (!string.IsNullOrEmpty(symbol.LableClassName))
                        {
                            var lables = symbol.LableClassName.Split(',');
                            foreach (string lbstr in lables)
                            {
                                
                                if (!string.IsNullOrEmpty(lbstr))
                                {
                                    IEnumerable<ElectronSymbol> es=null;
                                    Type type = TypeCache.Instance.GetTypeFromCache(typeof(DBEntity), lbstr);
                                         try {

                                             es = xmldb.GetEntities(type).Cast<ElectronSymbol>();
                                             }catch(Exception ex)
                                         {
                                             continue;
                                         }
                                      
 
                                    if(es.Count()>0)
                                    {
                                        foreach (var e in es)
                                        {
                                            if (e != null)
                                            {
                                                if (e.EntityState == EntityState.Insert)
                                                {
                                                    long OldG3e_fid =e.G3E_ID;
                                                    long GisFid = 0;
                                                    long GisFid2 = 0;
                                                    Cxbz(e.G3E_FID, ref GisFid);
                                                    if (GisFid != 0)
                                                    {
                                                        e.G3E_ID = OracleSequenceValueGenerator.Instance.GenerateTableId(type);
                                                        GisFid2 = InsLbFid(e.G3E_FID, e.G3E_FNO, e, smzq, GC_ID, (decimal?)e.GetValue("LTT_ID"), OldG3e_fid);
                                                        insertSymbolLb(lbstr, GisFid, e, ref oracledb, ref xmldb, ref InsertLB);
                                                    }

                                                }
                                                else if (e.EntityState == EntityState.Update)
                                                {
                                                    //更新标注坐标表  
                                                    UpdateSymbolLb(lbstr, e.G3E_FID, e, ref oracledb, ref xmldb, ref UpdateEnt);

                                                    var inTt = new InsertTempTable();
                                                    inTt.g3e_fid = e.G3E_FID;
                                                    inTt.g3e_fno = e.G3E_FNO;
                                                    inTt.v2 = e;
                                                    inTt.ltt_name = smzq;
                                                    inTt.GcTicket_ID = GC_ID;
                                                    inTt.Sjversion = int.Parse(e.GetValue("Version").ToString());
                                                    InsertTempTableS.Add(inTt);
                                                }
                                                else if (e.EntityState == EntityState.Delete)
                                                {
                                                    //删除标注坐标表  
                                                    DeleteEnt.Add(e);
                                                    //更新spatial表的数据和工单状态
                                                    //StateAreNOEnt.Add(e);
                                                    //删除坐标符号表
                                                    type = TypeCache.Instance.GetTypeFromCache(typeof(DBEntity), lables[0].Replace("_sdogeom", ""));
                                                    var symb_f = ReflectionUtils.CreateObject(
                                                        new
                                                        {
                                                            G3E_FID = e.G3E_FID
                                                        }, type) as ElectronBase;
                                                    if (symb_f != null)
                                                    {
                                                        DeleteEnt.Add(symb_f);
                                                    }

                                                    var inTt = new InsertTempTable();
                                                    inTt.g3e_fid = e.G3E_FID;
                                                    inTt.g3e_fno = e.G3E_FNO;
                                                    inTt.v2 = e;
                                                    inTt.ltt_name = smzq;
                                                    inTt.GcTicket_ID = GC_ID;
                                                    inTt.Sjversion = e.GetValue<int>("Version");
                                                    InsertTempTableS.Add(inTt);
                                                }
                                            }
                                        }
                                    }
                                 
                                }
                            }
                        }
                    }
                    catch (Exception ex)
                    {

                        if (!(ex.ToString().IndexOf("不存在") >= 0))
                        {
                            CYZLog.writeLog(ex.ToString(), "");
                        }
                        LogManager.Instance.Error(ex);
                    }
                    finally
                    {
                        if (cdm != null) { cdm.close(); }
                    }
                }

                var fireFids = InsertEnt.Concat(UpdateEnt).Concat(DeleteEnt).Select(o => o.G3E_FID).Distinct();
                try
                {
                    oracledb.InsertBulk(InsertEnt.Cast<DBEntity>(), true, new ElectronTransferService.UploadEventHandler.ActiveJob(qj, smzq));
                    oracledb.UpdateBulk(UpdateEnt.Cast<DBEntity>(), true, new ElectronTransferService.UploadEventHandler.ActiveJob(qj, smzq));
                    oracledb.DeleteBulk(DeleteEnt.Cast<DBEntity>(), true, new ElectronTransferService.UploadEventHandler.ActiveJob(qj, smzq));
                    oracledb.InsertBulk(InsertLB.Cast<DBEntity>(), true, new ElectronTransferService.UploadEventHandler.ActiveJob(qj, smzq));

                }
                catch (NotExistException ex)
                {

                }
                catch (Exception ex)
                {

                    CYZLog.writeLog(ex.ToString(), "");
                    throw ex;
                }

                // System.Threading.Thread.Sleep(500);
                foreach (var vfid in fireFids)
                {
                    TickHelper.fireEventShip(vfid.ToString());
                }
                System.Threading.Thread.Sleep(500);
                TickHelper.fireEvent();


                CDBManger dbm = new CDBManger();
                foreach (var appstr in AppendFidShipS)
                {
                    dbm.ExecuteNonQuery(appstr);
                }

                int ltt_idd = 0;
                try
                {
                    ltt_idd = int.Parse(lttid);
                }
                catch
                {
                }
                finally
                {
                    if (dbm != null)
                    {
                        dbm.close();
                    }
                }

                foreach (var AppendShip in InsertTempTableS)
                {
                    ElectronTransferService.UploadEventHandler.InsertTempTable(
                        AppendShip.g3e_fid, AppendShip.g3e_fno, AppendShip.v2.GetValue("EntityState").ToString(),
                        AppendShip.GcTicket_ID, ltt_idd, appenshiptime, AppendShip.Sjversion);
                }
            }
           // catch (KeyNotFoundException ex) {}
            catch (Exception ex)
            {
                if (!(ex.ToString().IndexOf("uploadPro 跳出异常") >= 0))
                {
                    CYZLog.writeLog(ex.ToString(), "uploadPro 跳出异常");
                }
                LogManager.Instance.Error(ex);
            }
        }


        /// <summary>
        /// 关闭连接
        /// </summary>
        /// <param name="dbManager"></param>
        private static void CloseOracleConnection(CDBManger dbManager)
        {
            if (dbManager != null)
                dbManager.close();
        }

        public class InsertTempTable
        {
            public long g3e_fid;
            public int g3e_fno;
            public DBEntity v2;
            public string ltt_name;
            public string GcTicket_ID;
            public int Sjversion;
        }

        /// <summary>
        /// 如果是原有设备，只新增标注
        /// List<ElectronBase> InsertLB
        /// </summary>
        /// <param name="dbManager"></param>
        public static void Cxbz(long GisFid,ref long GisNewFid)
        {
            CDBManger cdb = null;
            try
            {
                cdb = new CDBManger();
                string sql = "select g3e_fid from b$common_n where g3e_fid=" + GisFid;
                 DataTable tab = cdb.ExecuteTable(sql);
                 if (tab != null && tab.Rows.Count > 0)
                 {
                     GisNewFid = (long)tab.Rows[0][0];//long.Parse(tab.Rows[0][0].ToString());
                 }
            }
            catch (Exception ex)
            {
            }
            finally
            {
                if (cdb != null) { cdb.close(); }
            }
        }


        public static long InsLbFid(long oldg3e_fid, int g3e_fno, DBEntity v2, string ltt_name, string GC_ID, decimal? ltt_idd,long OldG3e_id)
        {
            CDBManger cdb_newfid = null;
            long new_g3e_fid = 0;
            int num = 0;
            DataTable dtNewfid = null;
            try
            {
                cdb_newfid = new CDBManger();


                string sql2 = "select g3e_fid2 from cadgis.AppendFidShipLbDe where g3e_fid2=" + oldg3e_fid + " and gcticket_id=" + GC_ID + " and  bs=" + 0;

                  
                dtNewfid = cdb_newfid.ExecuteTable(sql2);
                if (dtNewfid != null && dtNewfid.Rows.Count > 0)
                {
                    new_g3e_fid = long.Parse(dtNewfid.Rows[0]["g3e_fid2"].ToString());
                }
                else
                {
                    new_g3e_fid = 0;
                    string sql = "insert into cadgis.AppendFidShipLbDe(G3E_FID2,GCTICKET_ID,G3E_ID,G3E_ID2,VERSION,G3E_FNO,bs) values(" + oldg3e_fid + ",'"
                               + GC_ID + "', " + v2.GetValue("G3E_ID") + "," + OldG3e_id + "," + v2.GetValue("Version") + "," + g3e_fno +","+0+ ")";

                    cdb_newfid.ExecuteNonQuery(sql);
                }





            }
            catch (System.Exception ex)
            {
                CYZLog.writeLog(ex.ToString(), "");
            }
            finally
            {
                if (cdb_newfid != null)
                {
                    cdb_newfid.close();
                }
            }
            return new_g3e_fid;
        }

        /// <summary>
        /// 专门处理原有数据但是新增标注的数据
        /// List<ElectronBase> InsertLB
        /// </summary>
        /// <param name="dbManager"></param>
        public static void UpdateLblist(List<ElectronBase> InsDateEnt,List<ElectronBase> UpDateEnt, string gc_id)
        {
            //CDBManger dbm = null;
            //OracleDataReader dr = null;

            CDBManger dbm2 = null;
            OracleDataReader dr2 = null;
            List<ElectronBase> insDetailreference = new List<ElectronBase>();
            try
            {
                Dictionary<long, List<ElectronBase>> upTemp = new Dictionary<long, List<ElectronBase>>();

                string fids = "";
                foreach (var VarFid in InsDateEnt)
                {
                    fids += VarFid.G3E_FID + ",";
                    if (upTemp.Keys.Contains(VarFid.G3E_FID))
                    {
                        upTemp[VarFid.G3E_FID].Add(VarFid);
                    }
                    else
                    {
                        List<ElectronBase> ttlist = new List<ElectronBase>();
                        ttlist.Add(VarFid);
                        upTemp.Add(VarFid.G3E_FID, ttlist);
                    }
                }
                if (fids == "")
                    return;

                fids = fids.Substring(0, fids.Length - 1);

                string AllFidsSql2 = "select g3e_fid2 from cadgis.AppendFidShipLbDe where gcticket_id='" + gc_id + "'  and bs=0 and g3e_fid2  in(" + fids + ")";
                dbm2 = new CDBManger();
                dr2 = dbm2.ExecuteReader(AllFidsSql2);


                while (dr2.Read())
                {
                    if (upTemp.Keys.Contains(long.Parse(dr2["G3E_FID2"].ToString())))
                    {
                        foreach (var vv in upTemp[long.Parse(dr2["G3E_FID2"].ToString())])
                        {
                            insDetailreference.Add(vv);
                            UpDateEnt.Add(vv);
                        }
                    }
                }

                foreach (var VARIABLE in insDetailreference)
                {
                    InsDateEnt.Remove(VARIABLE);
                }

            }
            catch (Exception ex)
            {

                CYZLog.writeLog(ex.ToString(), "");
            }
            finally
            {
                //CloseOracleDataReader(dr, dbm);
                CloseOracleDataReader(dr2, dbm2);
            }
        }
      
    }
}
