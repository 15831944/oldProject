using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Text;
using System.Web.UI.MobileControls;
using Oracle.DataAccess.Client;
using ElectronTransferModel.Config;
using ElectronTransferDal.OracleDal;
using System.Reflection;
using System.IO;
using ElectronTransferDal.Common;
using ElectronTransferModel.Base;
using ElectronTransferFramework;
using System.Data;
using System.Threading;
using CYZFramework.Log;
using System.Security.AccessControl;
using CYZFramework.DB;
using System.Net;


//using System.Data.OracleClient;

namespace ElectronTransferServiceDll
{
    public class TickHelper
    {
        /// <summary>
        /// 
        /// </summary>
        /// <param name="session_id"></param>
        /// <param name="SSDW_ID"></param>
        /// <param name="KX_ID"></param>
        /// <param name="ZYDY"></param>
        /// <returns></returns>
        public static bool Trans(string session_id, string SSDW_ID, string KX_ID, string ZYDY, string ftplj)
        {
            bool reval = false;
            CDBManger dbm = new CDBManger();
            try
            {
                StringBuilder sqlstr = new StringBuilder("");

                sqlstr.Append(" BEGIN ");
                //sqlstr.Append(" GT_GETFEATURETOCAD_PKG.GETFEATUREBYFEEDER('GDDL-WJ', 2, 11310701, T_LIST); ");
                sqlstr.Append(" GT_GETFEATURETOCAD_PKG.GETFEATUREBYFEEDER('" + SSDW_ID + "', " + ZYDY + ", " + KX_ID + ", '" + session_id + "'); commit;");
                sqlstr.Append(" END; ");

                dbm.ExecuteNonQuery(sqlstr.ToString());


                PublicMethod.write_state(ftplj, 0.05);

                reval = true;
            }
            catch (Exception ex)
            {
                CYZLog.writeLog(ex.ToString());
                reval = false;
            }
            finally
            {
                if (dbm != null)
                {
                    dbm.close();
                }
            }
            return reval;
        }


       
        /// <summary>
        /// 下载馈线和编辑范围里面的设备
        /// </summary>
        /// <param name="session_id"></param>
        /// <param name="Xmin"></param>
        /// <param name="Xmax"></param>
        /// <param name="Ymin"></param>
        /// <param name="Ymax"></param>
        /// <param name="ZYDY"></param>
        /// <returns></returns>
        public static bool Trans3(string session_id, double Xmin, double Xmax, double Ymin, double Ymax, string ZYDY, string ftplj)
        {
            bool reval = false;
            CDBManger dbm = new CDBManger();
            try
            {
                StringBuilder sqlstr = new StringBuilder("create table " + session_id + " as select g3e_fno,g3e_fid FROM");
                sqlstr.Append(" (select * FROM cadgis.b$gg_pd_dx_ln_sdogeom ");
                sqlstr.Append(" union all select * from cadgis.b$gg_pd_dl_ln_sdogeom ");
                sqlstr.Append(" union all select * from cadgis.b$gg_pd_gydf_ar_sdogeom ");
                sqlstr.Append(" union all select * from cadgis.b$gg_pd_zydf_ar_sdogeom ");
                sqlstr.Append(" union all select * from cadgis.b$gg_pd_xssb_ar_sdogeom) ");
                sqlstr.Append(" WHERE MDSYS.SDO_RELATE( ");
                sqlstr.Append(" G3e_Geometry, ");
                sqlstr.Append(" MDSYS.SDO_GEOMETRY  ");
                sqlstr.Append(" ( ");
                sqlstr.Append(" 3003, ");
                sqlstr.Append(" NULL, ");
                sqlstr.Append(" NULL, ");
                sqlstr.Append(" MDSYS.sdo_elem_info_array (1,1003,1), ");
                sqlstr.Append(" MDSYS.sdo_ordinate_array ( ");
                sqlstr.Append("  " + Xmin + "," + Ymin + ",0, ");
                sqlstr.Append("  " + Xmax + "," + Ymin + ",0, ");
                sqlstr.Append("  " + Xmax + "," + Ymax + ",0,  ");
                sqlstr.Append("  " + Xmin + "," + Ymax + ",0, ");
                sqlstr.Append("  " + Xmin + "," + Ymin + ",0) ");
                sqlstr.Append(" ), ");
                sqlstr.Append("  'MASK= ANYINTERACT' ");
                sqlstr.Append(" ) ='TRUE' ");


                dbm.ExecuteNonQuery(sqlstr.ToString());

                string sql = "delete " + session_id + " where  g3e_fid in (select g3e_fid from " + session_id.Substring(0, session_id.Length - 2) + ")";

                dbm.ExecuteNonQuery(sql.ToString());


                PublicMethod.write_state(ftplj, 0.08);
                reval = true;
            }
            catch (Exception ex)
            {
                CYZLog.writeLog(ex.ToString(), "");
                reval = false;
            }
            finally
            {
                if (dbm != null)
                {
                    dbm.close();
                }
            }
            return reval;
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="session_id"></param>
        /// <param name="Xmin"></param>
        /// <param name="Xmax"></param>
        /// <param name="Ymin"></param>
        /// <param name="Ymax"></param>
        /// <param name="ZYDY"></param>
        /// <returns></returns>
        public static bool Trans2(string session_id, double Xmin, double Xmax, double Ymin, double Ymax, string ZYDY)
        {
            bool reval = false;
            CDBManger dbm = new CDBManger();
            try
            {
                StringBuilder sqlstr = new StringBuilder("");
                sqlstr.Append(" declare ");
                sqlstr.Append(" area_sdo_list GT_GETFEATURETOCAD_PKG.sdo_plist; ");
                sqlstr.Append(" begin ");
                sqlstr.Append(" AREA_SDO_LIST := GT_GETFEATURETOCAD_PKG.SDO_PLIST(); ");
                sqlstr.Append(" AREA_SDO_LIST.EXTEND; ");
                sqlstr.Append(" AREA_SDO_LIST(1).SDO_X := " + Xmin + "; ");//0
                sqlstr.Append(" AREA_SDO_LIST(1).SDO_Y := " + Ymin + "; ");//2
                sqlstr.Append(" AREA_SDO_LIST.EXTEND; ");
                sqlstr.Append(" AREA_SDO_LIST(2).SDO_X := " + Xmax + "; ");//1
                sqlstr.Append(" AREA_SDO_LIST(2).SDO_Y := " + Ymin + "; ");//2
                sqlstr.Append(" AREA_SDO_LIST.EXTEND; ");
                sqlstr.Append(" AREA_SDO_LIST(3).SDO_X := " + Xmax + "; ");//1
                sqlstr.Append(" AREA_SDO_LIST(3).SDO_Y := " + Ymax + "; ");//3
                sqlstr.Append(" AREA_SDO_LIST.EXTEND; ");
                sqlstr.Append(" AREA_SDO_LIST(4).SDO_X := " + Xmin + "; ");//0
                sqlstr.Append(" AREA_SDO_LIST(4).SDO_Y := " + Ymax + "; ");//3
                sqlstr.Append(" GT_GETFEATURETOCAD_PKG.getfeaturebyarea(AREA_SDO_LIST, ");
                sqlstr.Append(" " + ZYDY + ", ");
                sqlstr.Append(" '" + session_id + "'); ");
                sqlstr.Append(" commit; ");
                sqlstr.Append(" end; ");

                dbm.ExecuteNonQuery(sqlstr.ToString());

                reval = true;
            }
            catch (Exception ex)
            {
                CYZLog.writeLog(ex.ToString(), "");
                reval = false;
            }
            finally
            {
                if (dbm != null) 
                {
                    dbm.close();
                }
            }
            return reval;
        }
        /// <summary>
        /// 多边形的方法
        /// </summary>
        /// <param name="session_id"></param>
        /// <param name="Xmin"></param>
        /// <param name="Xmax"></param>
        /// <param name="Ymin"></param>
        /// <param name="Ymax"></param>
        /// <param name="ZYDY"></param>
        /// <returns></returns>
        public static bool TransDbx(string session_id, string ZYDY, List<string> DbxList)
        {
            Console.WriteLine("dddddddddd" + session_id);
            bool reval = false;
            CDBManger dbm = new CDBManger();
            try
            {
                int i = 1;
                StringBuilder sqlstr = new StringBuilder("");
                sqlstr.Append(" declare ");
                sqlstr.Append(" area_sdo_list GT_GETFEATURETOCAD_PKG.sdo_plist; ");
                sqlstr.Append(" begin ");
                sqlstr.Append(" AREA_SDO_LIST := GT_GETFEATURETOCAD_PKG.SDO_PLIST(); ");

                foreach (var v in DbxList)
                {
                    sqlstr.Append(" AREA_SDO_LIST.EXTEND; ");
                    sqlstr.Append(" AREA_SDO_LIST(" + i + ").SDO_X := " + double.Parse(v.Split(',')[0]) + "; ");//0
                    sqlstr.Append(" AREA_SDO_LIST(" + i + ").SDO_Y := " + double.Parse(v.Split(',')[1]) + "; ");//2

                    i++;
                }

                sqlstr.Append(" GT_GETFEATURETOCAD_PKG.GETFEATUREBYAREADBX(AREA_SDO_LIST, ");
                sqlstr.Append(" " + ZYDY + ", ");
                sqlstr.Append(" '" + session_id + "'); ");
                sqlstr.Append(" commit; ");
                sqlstr.Append(" end; ");

                dbm.ExecuteNonQuery(sqlstr.ToString());

                reval = true;
            }
            catch (Exception ex)
            {
                CYZLog.writeLog(ex.ToString(), "");
                reval = false;
            }
            finally
            {
                if (dbm != null) 
                {
                    dbm.close();
                }
            }
            return reval;
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="session_id"></param>
        /// <returns></returns>
        public static List<string> getFnos(string session_id)
        {
            List<string> fnos = new List<string>();
            CDBManger dbm = new CDBManger();
            OracleDataReader dr = null;
            try
            {
                StringBuilder sqlstr = new StringBuilder(" select distinct G3E_FNO from " + session_id);
                dr = dbm.ExecuteReader(sqlstr.ToString());
                while (dr.Read())
                {
                    fnos.Add(dr[0].ToString());
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
            return fnos;
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="fid"></param>
        /// <returns></returns>
        public static string getFno(string fid)
        {
            string fno = "";
            CDBManger dbm = new CDBManger(); OracleDataReader dr = null;
            try
            {
                StringBuilder sqlstr = new StringBuilder(" select G3E_FNO from b$common where g3e_fid=" + fid);
                dr = dbm.ExecuteReader(sqlstr.ToString());
                if (dr.Read())
                {
                    fno = dr[0].ToString();
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
            return fno;
        }
        /// <summary>
        /// 开工单锁设备
        /// </summary>
        /// <param name="GC_FNOFIDS"></param>
        /// <param name="GC_UserName"></param>
        /// <param name="GC_ID"></param>
        /// <param name="tickerr"></param>
        /// <returns></returns>
        public static string openticks(List<string> GC_FNOFIDS, string GC_UserName, string GC_ID, int tickerr)
        {
            string ss = "";
            CDBManger dbm = new CDBManger();

            try
            {
                //if (!qxtick1(GC_ID)) { ; }

                //1.创建临时表"tickerr" + GC_ID
                //GC_ID = GC_ID.Replace("-", "_");

                if (!dbm.ExecuteNonQuery("create table tickerr" + tickerr + "(g3e_fno number(5),g3e_fid number(10))"))
                {
                    ss = "ERROR:创建临时表错误,请联系服务接口创始人。";
                }
                else
                {
                    StringBuilder sb = new StringBuilder();
                    sb.Append(" declare ");
                    sb.Append(" c_data      gt_getfeaturetocad_pkg.T_FDATA; ");
                    sb.Append(" c_data_ERR  gt_getfeaturetocad_pkg.T_FDATA; ");
                    sb.Append(" LTTID       NUMBER; ");
                    sb.Append(" begin ");
                    sb.Append(" c_data     := gt_getfeaturetocad_pkg.T_FDATA(); ");
                    sb.Append(" c_data_ERR := gt_getfeaturetocad_pkg.T_FDATA(); ");
                    foreach (string fnofid in GC_FNOFIDS)
                    {
                        if (fnofid.Split(',').Length == 2)
                        {
                            sb.Append(" c_data.extend; ");
                            sb.Append(" c_data(c_data.count).g3e_fno := " + fnofid.Split(',')[0] + "; ");
                            sb.Append(" c_data(c_data.count).g3e_fid := " + fnofid.Split(',')[1] + "; ");
                        }
                    }
                    sb.Append(" LTTID := gt_getfeaturetocad_pkg.GOPENTICKET('" + ElectronTransferServicePro.QJConfig + "' , '" + GC_ID + "' , c_data , c_data_ERR); ");
                    sb.Append(" FOR I IN 1 .. c_data_ERR.COUNT LOOP ");
                    sb.Append(" insert into tickerr" + tickerr + " values(c_data_ERR(I).g3e_fno,c_data_ERR(I).g3e_fid); ");
                    sb.Append(" END LOOP; ");

                    sb.Append(" insert into cadgis.ticketship(gisticket_id,gcticket_id,gcusername,ltt_name,status,ticket_time,version) values(LTTID,'" + GC_ID + "','" + GC_UserName + "','','',to_date('" + DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss") + "','yyyy-MM-dd HH24:mi:ss'),"+1+" ); ");

                    sb.Append(" update  cadgis.ticketship t  ");
                    sb.Append(" set t.ltt_name= ");
                    sb.Append(" (select ltt_name from ltt_identifiers where ltt_id=LTTID) ");
                    sb.Append(" where t.gisticket_id=LTTID; ");
                    sb.Append(" commit;");

                    sb.Append(" END; ");

                    dbm = new CDBManger();
                    if (dbm.ExecuteNonQuery(sb.ToString()))
                    {

                        dbm = new CDBManger();
                        OracleDataReader dr = dbm.ExecuteReader("select * from cadgis.ticketship where gcticket_id='" + GC_ID + "'");
                        if (dr != null && dr.Read())
                        {
                            ss = "GISTICKETID:" + dr["GISTICKET_ID"];
                            DateTime dtimenow = DateTime.Now;
                            foreach (string fnofid in GC_FNOFIDS)
                            {
                                if (fnofid.Split(',').Length == 2)
                                {
                                    ElectronTransferService.UploadEventHandler.InsertTempTable(long.Parse(fnofid.Split(',')[1]), int.Parse(fnofid.Split(',')[0]), "Update",
                                         GC_ID, int.Parse(dr["gisticket_id"].ToString()), dtimenow,1);
                                }
                            }
                        }
                        CloseOracleDataReader(dr);

                        bool issuccess = true;
                        OracleDataReader errdr = dbm.ExecuteReader("select * from tickerr" + tickerr);
                        if (errdr != null)
                        {
                            string strerrfids = "";
                            int iiii = 0;
                            while (errdr.Read())
                            {
                                if (iiii == 0) { ss += ";ERRLIST:"; }
                                ss += errdr[0] + "," + errdr[1] + "-";
                                strerrfids += errdr[1] + ",";
                                iiii++;
                                issuccess = false;

                            }
                            if (iiii > 0)
                            {
                                strerrfids = strerrfids.Substring(0, strerrfids.Length - 1);
                                string ErrFidSql = "delete from  cadgis.appendship where g3e_fid in("+strerrfids+")";
                                dbm.ExecuteNonQuery(ErrFidSql);

                                ss = ss.Substring(0, ss.Length - 1);
                                CYZLog.writeLog(ss, "");
                            }
                        }
                        CloseOracleDataReader(errdr);
                    }
                    else
                    {
                        ss = "ERRLIST:调用开工单包失败。";
                    }
                }
            }
            catch (Exception ex)
            {
                if (dbm != null)
                {
                    dbm.close();
                }
                ss = "ERRLIST:" + ex;
                CYZLog.writeLog(ex.ToString(), "");

                throw ex;
            }
            finally
            {
                dbm = new CDBManger();
                dbm.ExecuteNonQuery("drop table tickerr" + tickerr);
                Thread.Sleep(500);
                foreach (string fnofid in GC_FNOFIDS)
                {
                    if (fnofid.Split(',').Length == 2)
                    {
                        fireEventShip(fnofid.Split(',')[1]);
                    }
                }
                Thread.Sleep(500);
                fireEvent();
                
            }

            return ss;
        }

        /// <summary>
        /// 只开工单
        /// </summary>
        /// <param name="GC_ID"></param>
        /// <returns></returns>
        public static string openticks1(string qj_config, string GC_ID)
        {
            string ss = "";
            CDBManger dbm = null;

            try
            {   //if (!qxtick1(GC_ID)) { ; }

                StringBuilder sb = new StringBuilder();
                sb.Append(" declare ");
                sb.Append(" LTTID       NUMBER; ");
                sb.Append(" begin ");
                sb.Append(" LTTID := GT_GETFEATURETOCAD_PKG.GOPENTICKET('" + qj_config + "' , '" + GC_ID + "' );");
                sb.Append(" insert into cadgis.ticketship(gisticket_id,gcticket_id,gcusername,ltt_name,status,ticket_time) values(LTTID,'" + GC_ID + "','" + "" + "','','',to_date('" + DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss") + "','yyyy-MM-dd HH24:mi:ss') ); ");

                sb.Append(" update  cadgis.ticketship t  ");
                sb.Append(" set t.ltt_name= ");
                sb.Append(" (select ltt_name from ltt_identifiers where ltt_id=LTTID) ");
                sb.Append(" where t.gisticket_id=LTTID; ");

                sb.Append(" commit;");

                sb.Append(" END; ");


                dbm = new CDBManger();
                if (dbm.ExecuteNonQuery(sb.ToString()))
                {
                    ss = "开工单成功";
                }

            }
            catch (Exception ex)
            {

                ss = "ERRLIST:" + ex.ToString();
                CYZLog.writeLog(ex.ToString(), "");
            }
            finally 
            {
                if (dbm != null) 
                {
                    dbm.close();
                }
            }



            return ss;
        }


        /// <summary>
        /// 只锁定设备
        /// </summary>
        /// <param name="GC_FNOFIDS"></param>
        /// <param name="ltt_name"></param>
        /// <param name="GC_ID"></param>
        /// <param name="tickerr"></param>
        /// <returns></returns>
        public static string LockSb(List<string> GC_FNOFIDS, string ltt_name, string GC_ID, int tickerr)
        {
            string ss = "FALSE";
            CDBManger dbm = new CDBManger();

            try
            {
                int? ltt_id = default(int);
                OracleDataReader dr = dbm.ExecuteReader("select gisticket_id from cadgis.ticketship where gcticket_id='" + GC_ID + "'");
                bool s = dr.Read();
                if (dr == null || !s)
                {
                    ss = "ERROR:没有此工单。";
                    return ss;
                }
                else
                {
                    ltt_id = Convert.ToInt32(dr["gisticket_id"]);
                }
                CloseOracleDataReader(dr);

                //1.创建临时表"tickerr" + GC_ID
                //GC_ID = GC_ID.Replace("-", "_");

               // if (!qxtick1(GC_ID)) { ; }

                //1.创建临时表"tickerr" + GC_ID
                //GC_ID = GC_ID.Replace("-", "_");

                if (!dbm.ExecuteNonQuery("create table  tickerr" + tickerr + "   (g3e_fno number(5),g3e_fid number(10))"))
                {
                    ss = "ERROR:创建临时表错误,请联系服务接口创始人。";
                }
                else
                {
                    StringBuilder sb = new StringBuilder();
                    sb.Append(" declare ");
                    sb.Append(" c_data      GT_GETFEATURETOCAD_PKG.T_FDATA; ");
                    sb.Append(" c_data_ERR  GT_GETFEATURETOCAD_PKG.T_FDATA; ");
                    sb.Append(" begin ");
                    sb.Append(" c_data     := GT_GETFEATURETOCAD_PKG.T_FDATA(); ");
                    sb.Append(" c_data_ERR := GT_GETFEATURETOCAD_PKG.T_FDATA(); ");
                    foreach (string fnofid in GC_FNOFIDS)
                    {
                        if (fnofid.Split(',').Length == 2)
                        {
                            sb.Append(" c_data.extend; ");
                            sb.Append(" c_data(c_data.count).g3e_fno := " + fnofid.Split(',')[0] + "; ");
                            sb.Append(" c_data(c_data.count).g3e_fid := " + fnofid.Split(',')[1] + "; ");
                        }
                    }
                    sb.Append(" GT_GETFEATURETOCAD_PKG.LOCKGISTICKET('" + ElectronTransferServicePro.QJConfig + "', '" + ltt_name + "'," + ltt_id + "," + "c_data,c_data_ERR); ");
                    sb.Append(" FOR I IN 1 .. c_data_ERR.COUNT LOOP ");
                    sb.Append(" insert into tickerr" + tickerr + " values(c_data_ERR(I).g3e_fno,c_data_ERR(I).g3e_fid); ");
                    sb.Append(" END LOOP; ");
                    sb.Append(" commit;");
                    sb.Append(" END; ");

                    dbm = new CDBManger();
                    if (dbm.ExecuteNonQuery(sb.ToString()))
                    {
                        dbm = new CDBManger();
                        // ss = "GISTICKETID:" + dr["GISTICKET_ID"].ToString();
                        DateTime dtimenow = DateTime.Now;
                        foreach (string fnofid in GC_FNOFIDS)
                        {
                            if (fnofid.Split(',').Length == 2)
                            {
                                ElectronTransferService.UploadEventHandler.InsertTempTable(long.Parse(fnofid.Split(',')[1]), int.Parse(fnofid.Split(',')[0]), "Update",
                                     GC_ID, ltt_id.Value, dtimenow, 1);
                            }
                        }

                        bool issuccess = true;
                        OracleDataReader errdr = dbm.ExecuteReader("select * from tickerr" + tickerr);
                        if (errdr != null)
                        {
                            string strerrfids = "";
                            int iiii = 0;
                            while (errdr.Read())
                            {
                                if (iiii == 0) { ss += ";ERRLIST:"; }
                                ss += errdr[0] + "," + errdr[1] + "-";
                                strerrfids += errdr[1] + ",";
                                iiii++;
                                issuccess = false;
                            }
                            CloseOracleDataReader(errdr);
                            if (iiii > 0)
                            {
                                strerrfids = strerrfids.Substring(0, strerrfids.Length - 1);
                                string ErrFidSql = "delete from  cadgis.appendship where g3e_fid in(" + strerrfids + ")";
                                dbm.ExecuteNonQuery(ErrFidSql);

                                ss = ss.Substring(0, ss.Length - 1);
                                CYZLog.writeLog(ss, "");
                            }
                        }
                        CloseOracleDataReader(errdr, dbm);
                        ss = "TRUE";
                    }
                    else
                    {
                        ss = "ERRLIST:调用开工单包失败。";
                    }
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
            finally
            {
                dbm.ExecuteNonQuery("drop table tickerr" + tickerr);
                Thread.Sleep(500);
                foreach (string fnofid in GC_FNOFIDS)
                {
                    if (fnofid.Split(',').Length == 2)
                    {
                        fireEventShip(fnofid.Split(',')[1]);
                    }
                }
                Thread.Sleep(500);
                fireEvent();
                if (dbm != null) { dbm.close(); }
            }
            CYZLog.writeLog(ss, "");
            return ss;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="GC_ID"></param>
        /// <returns></returns>
        public static string posttick(string GC_ID)
        {
            CDBManger dbm = null;
            string ss = "TRUE";
            string ltt_id = "";
            string ltt_name = "";
            string UserName = "";
            string PassWord = "";
            string QjPz = "";
            try
            {
                ElectronTransferService.ElectronTransferService.GetUserNamePwd(GC_ID, ref UserName, ref PassWord, ref QjPz);
                if (string.IsNullOrEmpty(UserName) || string.IsNullOrEmpty(PassWord) || string.IsNullOrEmpty(QjPz))
                {
                    CYZLog.writeLog("账号或密码不存在");
                    return "账号或密码不存在";
                }

                CYZLog.writeLog("posttick", GC_ID);
                //TickHelper.endFireEvent();
                Dictionary<string, string> DicFidFno = new Dictionary<string, string>();

                //1.创建临时表"tickerr" + GC_ID
                dbm = new CDBManger();
                DataTable dt_ticketship = dbm.ExecuteTable(" select gisticket_id,ltt_name from cadgis.ticketship t where t.gcticket_id='" + GC_ID + "'");
                if (dt_ticketship != null && dt_ticketship.Rows.Count > 0)
                {
                    ltt_id = dt_ticketship.Rows[0]["gisticket_id"].ToString();
                    ltt_name = dt_ticketship.Rows[0]["ltt_name"].ToString();
                }
                else
                {
                    ss = "FALSE";
                    return ss;
                }
                if (dbm != null) { dbm.close(); }


                StringBuilder sb = new StringBuilder();
                sb.Append(" BEGIN ");
                sb.Append(" LTT_USER.SETCONFIGURATION('" + QjPz + "'); ");
                sb.Append(" LTT_USER.EDITJOB('" + ltt_name + "'); ");
                sb.Append(" LTT_POST.POST; ");
                sb.Append(" LTT_USER.DONE; ");
                sb.Append(" END; ");

                dbm = new CDBManger() { UserName = UserName, Password = PassWord };
                if (dbm.ExecuteNonQuerySxsj(sb.ToString()))
                {
                    ss = "TRUE";
                    string sql = "insert into cadgis.appendshipbak select * from cadgis.appendship where gcticket_id='"+GC_ID+"'";
                    dbm.ExecuteNonQuery(sql);
                    string sql2 = "delete cadgis.appendship where gcticket_id='"+GC_ID+"'";
                    dbm.ExecuteNonQuery(sql2);
                    string sql3 = "insert into cadgis.appendfidshipbak select * from cadgis.appendfidship where gcticket_id='" + GC_ID + "'";
                    dbm.ExecuteNonQuery(sql3);
                    string sql4 = "delete cadgis.appendfidship where gcticket_id='" + GC_ID + "'";
                    dbm.ExecuteNonQuery(sql4);

                }
                else 
                {
                    ss = "FALSE";
                }


            }
            catch (Exception ex)
            {
                CYZLog.writeLog(ex.ToString(), "");
                ss = "ERROR:" + ex;
            }
            finally
            {
                //dbm = new CDBManger();
                //dbm.ExecuteNonQuery("drop table tickerr" + tickerr);
                Thread.Sleep(500);
                DataTable dt_appendship = dbm.ExecuteTable(" select g3e_fid from cadgis.appendship t where t.gcticket_id='" + GC_ID + "'");
                if (dt_appendship != null && dt_appendship.Rows.Count > 0)
                {
                    for (int i = 0; i < dt_appendship.Rows.Count; i++)
                        fireEventShip(dt_appendship.Rows[i]["g3e_fid"].ToString());
                }
                Thread.Sleep(500);
                TickHelper.fireEvent();
                if (dbm != null) { dbm.close(); }
            }
            CYZLog.writeLog("posttickend", GC_ID);
            return ss;
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="GC_ID"></param>
        /// <returns></returns>
        public static bool qxtick1(string GC_ID,string username,string password,string qjpz)
        {
            bool ss = false;
            DataTable dt_appendship = null;
            CDBManger dbm = null;
            CDBManger dbmzh = null;
            try
            {
                dbm = new CDBManger();

                DataTable dt_ticketship = dbm.ExecuteTable(" select ltt_name,gisticket_id from cadgis.ticketship t where t.gcticket_id='" + GC_ID + "'");
                if (dt_ticketship != null && dt_ticketship.Rows.Count > 0)
                {
                    string ltt_name = dt_ticketship.Rows[0]["ltt_name"].ToString();
                    string ltt_id = dt_ticketship.Rows[0]["gisticket_id"].ToString();

                    dt_appendship = dbm.ExecuteTable(" select g3e_fid from cadgis.appendship t where t.gcticket_id='" + GC_ID + "'");
                    if (dt_appendship != null && dt_appendship.Rows.Count > 0)
                    {
                        for (int i = 0; i < dt_appendship.Rows.Count; i++)
                            endFireEventShip(dt_appendship.Rows[i]["g3e_fid"].ToString());
                    }

                    if (ltt_id != "" || ltt_name != "")
                    {
                        string SqlDel = "DELETE FROM  LTT_SESSIONS  WHERE LTT_ID=" + ltt_id;
                        string SqlDelPend = "DELETE FROM  pendingedits  WHERE LTT_ID=" + ltt_id;
                        dbm.ExecuteNonQuery(SqlDel);
                        dbm.ExecuteNonQuery(SqlDelPend);
                        //撤销工单
                        dbmzh = new CDBManger() { UserName = username, Password = password };
                        StringBuilder sb = new StringBuilder("");
                        sb.Append(" BEGIN ");
                        sb.Append(" LTT_USER.SETCONFIGURATION('" +qjpz+ "'); ");
                        sb.Append(" LTT_USER.EDITJOB('" + ltt_name + "'); ");
                        sb.Append("  LTT_USER.DISCARDJOB; ");
                        sb.Append("  LTT_USER.DONE; ");
                        sb.Append(" END; ");
                        dbmzh.ExecuteNonQuerySxsj(sb.ToString());
                        dbm = new CDBManger();
                        dbm.ExecuteNonQuery("delete cadgis.appendfidship  t where t.gcticket_id='" + GC_ID + "'");
                       // dbm = new CDBManger();
                        dbm.ExecuteNonQuery("delete cadgis.appendship  t where t.gcticket_id='" + GC_ID + "'");
                       // dbm = new CDBManger();
                        dbm.ExecuteNonQuery("delete cadgis.AppendFidShipLbDe  t where t.gcticket_id='" + GC_ID + "'");

                    }
                    ss = true;
                }
                else
                {
                    CYZLog.writeLog("没有此工单");
                    ss = true;
                }

            }
            catch (Exception ex)
            {
                CYZLog.writeLog(ex.ToString(), "撤销工单出错");
                ss = false;
                throw ex;
            }
            finally
            {
                Thread.Sleep(500);
                if (dt_appendship != null && dt_appendship.Rows.Count > 0)
                {
                    for (int i = 0; i < dt_appendship.Rows.Count; i++)
                        fireEventShip(dt_appendship.Rows[i]["g3e_fid"].ToString());
                }
                Thread.Sleep(500);
                fireEvent();
                if (dbm != null) { dbm.close(); }
                if (dbmzh != null) { dbmzh.close(); }
            }
            return ss;
        }



        /// <summary>
        /// 以前的撤销工单
        /// </summary>
        /// <param name="GC_ID"></param>
        /// <returns></returns>
        public static bool qxtick1(string GC_ID)
        {
            CDBManger dbm = null;
            string ltt_name = "";
            string ltt_id = "";
            bool ss = false;
            DataTable dt_appendship = null;
            try
            {
                dbm = new CDBManger();

                DataTable dt_ticketship = dbm.ExecuteTable(" select * from cadgis.ticketship t where t.gcticket_id='" + GC_ID + "'");
                if (dt_ticketship != null && dt_ticketship.Rows.Count > 0)
                {
                    ltt_name = dt_ticketship.Rows[0]["ltt_name"].ToString();
                    ltt_id = dt_ticketship.Rows[0]["gisticket_id"].ToString();

                    dt_appendship = dbm.ExecuteTable(" select g3e_fid from cadgis.appendship t where t.gcticket_id='" + GC_ID + "'");
                    if (dt_appendship != null && dt_appendship.Rows.Count > 0)
                    {
                        for (int i = 0; i < dt_appendship.Rows.Count; i++)
                            endFireEventShip(dt_appendship.Rows[i]["g3e_fid"].ToString());
                    }

                    if (ltt_id != "" || ltt_name != "")
                    {
                        //撤销工单
                        StringBuilder sb = new StringBuilder("");
                        sb.Append(" BEGIN ");
                        sb.Append(" LTT_USER.SETCONFIGURATION('" + ElectronTransferServicePro.QJConfig + "'); ");
                        sb.Append(" LTT_USER.EDITJOB('" + ltt_name + "'); ");
                        sb.Append("  LTT_USER.DISCARDJOB; ");
                        sb.Append("  LTT_USER.DONE; ");
                        sb.Append(" END; ");
                        dbm.ExecuteNonQuery(sb.ToString());

                        dbm = new CDBManger();
                        dbm.ExecuteNonQuery("delete cadgis.appendfidship  t where t.gcticket_id='" + GC_ID + "'");
                        dbm = new CDBManger();
                        dbm.ExecuteNonQuery("delete cadgis.appendship  t where t.gcticket_id='" + GC_ID + "'");

                    }
                    ss = true;
                }
                else
                {
                    CYZLog.writeLog("没有此工单");
                    ss = true;
                }

            }
            catch (Exception ex)
            {
                CYZLog.writeLog(ex.ToString(), "");
                ss = false;
                throw ex;
            }
            finally
            {


                Thread.Sleep(500);
                if (dt_appendship != null && dt_appendship.Rows.Count > 0)
                {
                    for (int i = 0; i < dt_appendship.Rows.Count; i++)
                        fireEventShip(dt_appendship.Rows[i]["g3e_fid"].ToString());
                }
                Thread.Sleep(500);
                fireEvent();
                if (dbm != null) { dbm.close(); }
            }
            return ss;
        }



        /// <summary>
        /// 
        /// </summary>
        /// <param name="FNOFIDS"></param>
        /// <returns></returns>
        public static bool qxtick2(List<string> FNOFIDS)
        {
            CDBManger dbm = null;
            bool ss = false;
            try
            {
                //endFireEvent();
                foreach (string fnofid in FNOFIDS)
                {
                    string ltt_id = "";
                    string ltt_name = "";

                    getLttnameFromFid(fnofid.Split(',')[0], fnofid.Split(',')[1], ref ltt_id, ref ltt_name);
                    if (!string.IsNullOrEmpty(ltt_id) && !string.IsNullOrEmpty(ltt_name))
                    {
                        dbm = new CDBManger();
                        //撤销工单
                        StringBuilder sb = new StringBuilder("");
                        sb.Append(" BEGIN ");
                        sb.Append(" LTT_USER.SETCONFIGURATION('" + ElectronTransferServicePro.QJConfig + "'); ");
                        sb.Append(" LTT_USER.EDITJOB('" + ltt_name + "'); ");
                        sb.Append(" LTT_USER.DISCARDFID(" + fnofid.Split(',')[1] + "); ");
                        sb.Append(" LTT_USER.DONE; ");
                        sb.Append(" END; ");
                        dbm.ExecuteNonQuery(sb.ToString());

                        dbm = new CDBManger();
                        dbm.ExecuteNonQuery("delete cadgis.appendfidship  t where t.g3e_fid='" + fnofid.Split(',')[1] + "'");
                        //dbm = new CDBManger();
                        dbm.ExecuteNonQuery("delete cadgis.appendship  t where t.g3e_fid='" + fnofid.Split(',')[1] + "'");
                    }
                }
                ss = true;
            }
            catch (Exception ex)
            {
                CYZLog.writeLog(ex.ToString(), "");
                ss = false;
            }
            finally
            {
                Thread.Sleep(500);
                foreach (string fnofid in FNOFIDS)
                {
                    if (fnofid.Split(',').Length == 2)
                    {
                        fireEventShip(fnofid.Split(',')[1]);
                    }
                }
                Thread.Sleep(500);
                fireEvent();
                if (dbm != null) { dbm.close(); }
            }
            return ss;
        }



        /// <summary>
        /// 查找被其他工单锁定的
        /// </summary>
        /// <param name="ltt_id"></param>
        /// <param name="ltt_name"></param>
        /// <param name="gc_id"></param>
        /// <returns></returns>
        public static string OtherLock(decimal? ltt_id,string ltt_name,string gc_id)
        {
            CDBManger dbm = null;
            string ss=null ;
            string bz = null;
            try
            {
                //endFireEvent();
               
                    
                    if (ltt_id!=0&&ltt_id!=null)
                    {
                        dbm = new CDBManger();
                        //刷新工单下设备
                        StringBuilder sb = new StringBuilder("");
                        sb.Append(" BEGIN ");
                        sb.Append(" LTT_USER.SETCONFIGURATION('" + ElectronTransferServicePro.QJConfig + "'); ");
                        sb.Append(" LTT_USER.EDITJOB('" + ltt_name + "'); ");
                        sb.Append(" ltt_user.findpendingedits;end; ");
                        sb.Append(" g3e_updatedeltas.UpdateDeltasPrivate; ");
                        dbm.ExecuteNonQuery(sb.ToString());

                        string sql = "select g3e_fid from cadgis.appendship where gisticket_id=" + ltt_id + " and g3e_fid not in(select g3e_fid from pendingedits where ltt_id="+ltt_id+")";
                        OracleDataReader odr = dbm.ExecuteReader(sql);

                        while (odr.Read())
                        {
                            ss += odr["g3e_fid"] + ",";
                        }
                        CloseOracleDataReader(odr);  
                      
                        if (ss != null)
                        {
                            
                            ss = ss.Substring(0, ss.Length - 1);
                            dbm.ExecuteNonQuery("delete cadgis.appendship  t where t.g3e_fid in(" + ss + ")");
                        }
                        bz = "true";
                        
                      
                    }
            
            }
            catch (Exception ex)
            {
                CYZLog.writeLog(ex.ToString(), "");
                bz = "false";
            }
            finally
            {
               
                CloseOracleConnection(dbm);
            }
            return ss;
        }
        /// <summary>
        /// 按fid和工单号撤销设备工单
        /// </summary>
        /// <param name="FNOFIDS"></param>
        /// <param name="GC_ID"></param>
        /// <returns>返回不成功撤销的数据</returns>
        public static List<string> qxtick3(List<string> FNOFIDS, string GC_ID)
        {
            string sss = null;
            List<string> FildFnoFid = new List<string>();
            List<string> RemoveFildFnoFid = new List<string>();
            CDBManger dbm = null;

            try
            {
                dbm = new CDBManger();
                foreach (string fnofid in FNOFIDS)
                {
                    string ltt_id = "";
                    string ltt_name = "";

                    if (!getLttnameFromFid3(fnofid.Split(',')[0], fnofid.Split(',')[1], ref ltt_id, ref ltt_name, GC_ID))
                    {
                        if (ltt_name == null)
                        {
                            ltt_name = "12345678";
                        }
                        else
                        {
                            sss = fnofid.Split(',')[1] + "," + ltt_name;
                        }

                        ltt_name = null;
                        FildFnoFid.Add(sss);
                        RemoveFildFnoFid.Add(fnofid);
                    }

                    if (!string.IsNullOrEmpty(ltt_id) && !string.IsNullOrEmpty(ltt_name))
                    {
                        //按fid撤销工单
                        StringBuilder sb = new StringBuilder("");
                        sb.Append(" BEGIN ");
                        sb.Append(" LTT_USER.SETCONFIGURATION('" + ElectronTransferServicePro.QJConfig + "'); ");
                        sb.Append(" LTT_USER.EDITJOB('" + ltt_name + "'); ");
                        sb.Append(" LTT_USER.DISCARDFID(" + fnofid.Split(',')[1] + "); ");
                        sb.Append(" LTT_USER.DONE; ");
                        sb.Append(" END; ");
                        dbm.ExecuteNonQuery(sb.ToString());

                        dbm.ExecuteNonQuery("delete cadgis.appendfidship  t where t.g3e_fid='" + fnofid.Split(',')[1] + "'");
                        dbm.ExecuteNonQuery("delete cadgis.appendship  t where t.g3e_fid='" + fnofid.Split(',')[1] + "'");
                    }
                }

                foreach (string fnofid in RemoveFildFnoFid)
                {
                    FNOFIDS.Remove(fnofid);
                }
            }
            catch (Exception ex)
            {
                CYZLog.writeLog(ex.ToString(), "");
            }
            finally
            {
                if (FNOFIDS.Count() != 0)
                {
                    foreach (string fnofid in FNOFIDS)
                    {
                        if (fnofid.Split(',').Length == 2)
                        {
                            fireEventShip(fnofid.Split(',')[1]);
                        }
                    }
                    Thread.Sleep(500);
                    fireEvent();
                }
                if (dbm != null) { dbm.close(); }
            }
            return FildFnoFid;
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="GC_ID"></param>
        /// <returns></returns>
        public static Decimal SpatialLtt_id(string GC_ID)
        {
            CDBManger cdb = null;
            Decimal ltt_id = -1;
            try
            {
                if (string.IsNullOrEmpty(GC_ID))
                {
                    ltt_id = -1;
                    return ltt_id;
                }

                cdb = new CDBManger();
                string sql = "select GISTICKET_ID from cadgis.ticketship t where t.gcticket_id='" + GC_ID + "'";
                OracleDataReader odr = cdb.ExecuteReader(sql);
                if (odr.Read())
                {
                    ltt_id = Decimal.Parse(odr["GISTICKET_ID"].ToString());
                }
                CloseOracleDataReader(odr);
            }
            catch (Exception ex)
            {
                CYZLog.writeLog(ex.ToString(), "");
                ltt_id = -1;
            }
            finally
            {
                CloseOracleConnection(cdb);
            }


            return ltt_id;
        }

        /// <summary>
        /// 触发oracle外部程序的事件 终止状态
        /// </summary>
        public static void fireEvent()
        {
            try
            {
                Thread.Sleep(500);
                using (EventWaitHandle mevent = createEvent(ElectronTransferServicePro.cadEventName))
                {
                    if (mevent != null)
                    {
                        mevent.Set();
                        CYZLog.writeLog("fireEvent(" + ElectronTransferServicePro.cadEventName + ")");
                    }
                    else
                    {
                        CYZLog.writeLog("EventWaitHandle=null");
                    }
                }
            }
            catch (Exception ex)
            {
                CYZLog.writeLog("ex=" + ex);
            }
            finally
            {
                waitEvent();
            }
        }

        /// <summary>
        /// 触发oracle外部程序的事件 终止状态
        /// </summary>
        /// <param name="eventName"></param>
        public static void fireEventShip(string eventName)
        {
            try
            {
                CDBManger dbm = new CDBManger();
                string sqlstr = "update cadgis.eventship set eventstatus='" + eventName + "' where g3e_fid=" + eventName;
                dbm.ExecuteNonQuery(sqlstr);
            }
            catch (Exception ex)
            {
                CYZLog.writeLog("ex=" + ex.ToString());
            }
            finally
            {
            }
        }
        /// <summary>
        ///  触发oracle外部程序的事件 非终止状态
        /// </summary>
        /// <param name="eventName"></param>
        public static void endFireEventShip(string eventName)
        {
            try
            {
                CDBManger dbm = new CDBManger();
                string sqlstr = "update cadgis.eventship set eventstatus='' where g3e_fid=" + eventName;
                dbm.ExecuteNonQuery(sqlstr);
            }
            catch (Exception ex)
            {
                CYZLog.writeLog("ex=" + ex.ToString());
            }
            finally
            {
            }
        }
        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        public static bool waitEvent()
        {
            bool reval = false;
            try
            {
                bool bbEvent1 = false;
                using (EventWaitHandle mmEvent = new EventWaitHandle(false, EventResetMode.AutoReset, ElectronTransferServicePro.spatialEventName, out bbEvent1))
                {
                    if (mmEvent != null)
                    {
                        if (mmEvent.WaitOne(10 * 1000))
                        {
                            CYZLog.writeLog("receive event " + ElectronTransferServicePro.spatialEventName);

                            reval = true;
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                CYZLog.writeLog("ex=" + ex.ToString());
            }
            return reval;
        }
        /// <summary>
        /// 创建一个跨进程事件
        /// </summary>
        /// <param name="ewhName"></param>
        /// <returns></returns>
        static EventWaitHandle createEvent(string ewhName)
        {
            EventWaitHandle ewh = null;
            bool doesNotExist = false;
            bool unauthorized = false;
            bool wasCreated;

            try
            {
                ewh = EventWaitHandle.OpenExisting(ewhName);
            }
            catch (WaitHandleCannotBeOpenedException)
            {
                Console.WriteLine("Named event does not exist.");
                doesNotExist = true;
            }
            catch (UnauthorizedAccessException ex)
            {
                Console.WriteLine("Unauthorized access: {0}", ex.Message);
                unauthorized = true;
            }
            if (doesNotExist)
            {
                string user = Environment.UserDomainName + "\\" + Environment.UserName;

                //CYZFramework.Log.CYZLog.writeLog("user:" + user);

                EventWaitHandleSecurity ewhSec =
                    new EventWaitHandleSecurity();

                EventWaitHandleAccessRule rule =
                        new EventWaitHandleAccessRule(user,
                            EventWaitHandleRights.FullControl,
                            AccessControlType.Allow);
                ewhSec.AddAccessRule(rule);

                ewh = new EventWaitHandle(false, EventResetMode.AutoReset, ewhName, out wasCreated, ewhSec);

                if (wasCreated)
                {
                    CYZFramework.Log.CYZLog.writeLog("Created new event(" + ewhName + ")");
                }
                else
                {
                    CYZFramework.Log.CYZLog.writeLog("Unable to create the event.");
                    return null;
                }
            }
            else if (unauthorized)
            {
                try
                {
                    ewh = EventWaitHandle.OpenExisting(ewhName,
                        EventWaitHandleRights.ReadPermissions |
                        EventWaitHandleRights.ChangePermissions);

                    EventWaitHandleSecurity ewhSec = ewh.GetAccessControl();

                    string user = Environment.UserDomainName + "\\"
                        + Environment.UserName;

                    //CYZFramework.Log.CYZLog.writeLog("user:" + user);

                    EventWaitHandleAccessRule rule =
                        new EventWaitHandleAccessRule(user,
                            EventWaitHandleRights.FullControl,
                            AccessControlType.Allow);
                    ewhSec.AddAccessRule(rule);

                    ewh.SetAccessControl(ewhSec);

                    ewh = EventWaitHandle.OpenExisting(ewhName);

                    CYZFramework.Log.CYZLog.writeLog("open event(" + ewhName + ")");
                }
                catch (UnauthorizedAccessException ex)
                {
                    CYZFramework.Log.CYZLog.writeLog("Unable to change permissions: " + ex.Message);
                    return null;
                }

            }

            return ewh;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="fid"></param>
        /// <param name="ltt_id"></param>
        /// <param name="ltt_name"></param>
        /// <returns></returns>
        public static bool getLttnameFromFid3(string fno, string fid, ref string ltt_id, ref string ltt_name, string gc_id)
        {
            bool reval = false;
            CDBManger dbmgc = null;
            string Oldltt_id = null;
            CDBManger dbm = null;
            CDBManger dbm3 = null;

            try
            {
                dbmgc = new CDBManger();
                string GisidSql = "select distinct gisticket_id from   cadgis.ticketship where gcticket_id='" + gc_id + "'";
                DataTable GisIddt = dbmgc.ExecuteTable(GisidSql);
                if (GisIddt != null && GisIddt.Rows.Count > 0)
                {
                    Oldltt_id = GisIddt.Rows[0][0].ToString();
                }
                string sqlstr = " select t1.g3e_name from g3e_component t1,g3e_feature t2 ,g3e_featurecomponent t3 where t1.g3e_cno=t3.g3e_cno and t2.g3e_fno=t3.g3e_fno and t2.g3e_fno=" + fno +
                    " and ( NLS_UPPER(t1.g3e_username)=NLS_UPPER(t2.g3e_username)||'符号' or " +
                    " NLS_UPPER(t1.g3e_username)=NLS_UPPER(t2.g3e_username)||'标注' or " +
                    " NLS_UPPER(t1.g3e_username)=NLS_UPPER(t2.g3e_username)||'标注(名称)' or " +
                    " NLS_UPPER(t1.g3e_username)=NLS_UPPER(t2.g3e_username)||'标注(型号)' or " +
                    " NLS_UPPER(t1.g3e_username)=NLS_UPPER(t2.g3e_username)||'标注(用户用电号)' or " +
                    " NLS_UPPER(t1.g3e_username)=NLS_UPPER(t2.g3e_username) or " +
                    " NLS_UPPER(t1.g3e_username)='公共属性' or " +
                    " NLS_UPPER(t1.g3e_username)='连接属性' or " +
                    " NLS_UPPER(t1.g3e_username)='连接属性' " +
                    " ) ";
                 dbm = new CDBManger();
                DataTable dt = dbm.ExecuteTable(sqlstr);
                string[] sends = new[] { "_PT", "_LN", "_AR", "_LB", "_LB1", "_LB2" };
                if (dt != null && dt.Rows.Count > 0)
                {
                    for (int i = 0; i < dt.Rows.Count; i++)
                    {
                        string tabname = dt.Rows[i][0].ToString().ToUpper();
                        foreach (string send in sends)
                        {
                            if (tabname.IndexOf(send) > 0)
                            {
                                tabname = tabname + "_SDOGEOM";
                                break;
                            }
                        }
                        tabname = "B$" + tabname;

                        string sqlstr2 = "select distinct ltt_id from " + tabname + " where g3e_fid=" + fid + " and ltt_id<>0 and ltt_id is not null ";

                        DataTable dt2 = dbm.ExecuteTable(sqlstr2);
                        if (dt2 != null && dt2.Rows.Count > 0)
                        {
                            ltt_id = dt2.Rows[0][0].ToString();

                            string sqlstr3 = "select ltt_name from ltt_identifiers t where t.ltt_id=" + ltt_id;
                            dbm3 = new CDBManger();
                            DataTable dt3 = dbm.ExecuteTable(sqlstr3);
                            if (dt3 != null && dt3.Rows.Count > 0)
                            {
                                ltt_name = dt3.Rows[0][0].ToString();
                                break;
                            }
                        }
                    }
                }
                if (Oldltt_id == ltt_id)
                {
                    reval = true;
                }
                else
                {
                    reval = false;
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine("ex=" + ex);
            }
            finally
            {
                if (dbmgc != null) { dbmgc.close(); }
                if (dbm != null) { dbm.close(); }
                if (dbm3 != null) { dbm3.close(); }

            }
            return reval;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="fid"></param>
        /// <param name="ltt_id"></param>
        /// <param name="ltt_name"></param>
        /// <returns></returns>
        public static bool getLttnameFromFid(string fno, string fid, ref string ltt_id, ref string ltt_name)
        {
            bool reval = false;
            CDBManger dbm = null;
            CDBManger dbm2 = null;
            try
            {
                string sqlstr = " select t1.g3e_name from g3e_component t1,g3e_feature t2 ,g3e_featurecomponent t3 where t1.g3e_cno=t3.g3e_cno and t2.g3e_fno=t3.g3e_fno and t2.g3e_fno=" + fno +
                    " and ( NLS_UPPER(t1.g3e_username)=NLS_UPPER(t2.g3e_username)||'符号' or " +
                    " NLS_UPPER(t1.g3e_username)=NLS_UPPER(t2.g3e_username)||'标注' or " +
                    " NLS_UPPER(t1.g3e_username)=NLS_UPPER(t2.g3e_username)||'标注(名称)' or " +
                    " NLS_UPPER(t1.g3e_username)=NLS_UPPER(t2.g3e_username)||'标注(型号)' or " +
                    " NLS_UPPER(t1.g3e_username)=NLS_UPPER(t2.g3e_username)||'标注(用户用电号)' or " +
                    " NLS_UPPER(t1.g3e_username)=NLS_UPPER(t2.g3e_username) or " +
                    " NLS_UPPER(t1.g3e_username)='公共属性' or " +
                    " NLS_UPPER(t1.g3e_username)='连接属性' or " +
                    " NLS_UPPER(t1.g3e_username)='连接属性' " +
                    " ) ";
                dbm = new CDBManger();
                DataTable dt = dbm.ExecuteTable(sqlstr);
                string[] sends = new string[] { "_PT", "LN", "_AR", "_LB", "_LB1", "_LB2" };
                if (dt != null && dt.Rows.Count > 0)
                {
                    for (int i = 0; i < dt.Rows.Count; i++)
                    {
                        string tabname = dt.Rows[i][0].ToString().ToUpper();
                        foreach (string send in sends)
                        {
                            if (tabname.IndexOf(send) > 0)
                            {
                                tabname = tabname + "_SDOGEOM";
                                break;
                            }
                        }
                        tabname = "B$" + tabname;

                        string sqlstr2 = "select distinct ltt_id from " + tabname + " where g3e_fid=" + fid + " and ltt_id<>0 and ltt_id is not null ";

                         dbm2 = new CDBManger();
                        DataTable dt2 = dbm.ExecuteTable(sqlstr2);
                        if (dt2 != null && dt2.Rows.Count > 0)
                        {
                            ltt_id = dt2.Rows[0][0].ToString();

                            string sqlstr3 = "select ltt_name from ltt_identifiers t where t.ltt_id=" + ltt_id;
                           
                            DataTable dt3 = dbm.ExecuteTable(sqlstr3);
                            if (dt3 != null && dt3.Rows.Count > 0)
                            {
                                ltt_name = dt3.Rows[0][0].ToString();
                                break;
                            }
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine("ex=" + ex);
            }
            finally
            {
                if (dbm != null) { dbm.close(); }
                if (dbm2 != null) { dbm2.close(); }
            }
            return reval;
        }

        /// <summary>
        /// 如果是范围导出的话要导出锁定低压户表和集抄相对应的变压器
        /// </summary>
        /// <param name="TableName"></param>
        /// <param name="FnoFidList"></param>
        public static void DcByq(string TableName,List<string> FnoFidList)
        {
            CDBManger dbm = null;
            CDBManger dbmByq = null;
            string FidList = null;
            string SsxlList = null;
            List<string> FnoFidByq = new List<string>();
            StringBuilder Cd_ssxlList = new StringBuilder();
            try
            {
                dbm = new CDBManger();
                dbmByq = new CDBManger();
                foreach (var FnoFid in FnoFidList)
                {
                    FidList += FnoFid.Split(',')[1] + ",";
                }
                if (FidList != null)
                {
                    FidList=FidList.Substring(0, FidList.Length - 1);
                    //Dms.Append(" BEGIN ");
                    //Dms.Append(" LTT_USER.SETCONFIGURATION('" + ElectronTransferServicePro.QJConfig + "'); ");
                    //Dms.Append("LTT_ADMIN.v_LTTMode:='SHORT'; ");
                    //Dms.Append(" END; ");
                    //dbm.ExecuteNonQuery(Dms.ToString());
                    Cd_ssxlList.Append(" select distinct cd_ssxl from (select cd_ssxl,g3e_fid from b$common_n ");
                    Cd_ssxlList.Append(" union all select cd_ssxl,g3e_fid from b$gg_pd_cbx_n ");
                    Cd_ssxlList.Append(" union all select cd_ssxl,g3e_fid  from b$gg_pd_dyshb_n) a");
                    Cd_ssxlList.Append(" where a.g3e_fid in(" + FidList + ")");
                    OracleDataReader dr = dbm.ExecuteReader(Cd_ssxlList.ToString());
                    while (dr != null && dr.Read())
                    {
                        SsxlList +="'"+ dr["cd_ssxl"] + "',";
                    }
                    CloseOracleDataReader(dr);
                    if(SsxlList!=null)
                    {
                        SsxlList=SsxlList.Substring(0, SsxlList.Length - 1);
                        string Byqsql = "select distinct g3e_fno,g3e_fid from b$common_n t where t.cd_ssxl in(" + SsxlList + ") and t.g3e_fno=148 and t.g3e_fid not in( select g3e_fid from " + TableName + ")";
                        OracleDataReader Byqdr = dbmByq.ExecuteReader(Byqsql);
                        while (Byqdr != null && Byqdr.Read())
                        {
                            FnoFidByq.Add(Byqdr["g3e_fno"] + "," + Byqdr["g3e_fid"]); 
                            
                        }
                        CloseOracleDataReader(Byqdr);
                    }
                    if(FnoFidByq.Any())
                    {
                        foreach (var v in FnoFidByq)
                        {
                            string InsSql = "insert into "+TableName+" (g3e_fno,g3e_fid) values ("+v.Split(',')[0]+","+v.Split(',')[1]+")";
                            dbm.ExecuteNonQuery(InsSql);
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                CYZLog.writeLog("DcByq=" + ex);
            }
            finally
            {
                CloseOracleConnection(dbmByq);
                CloseOracleConnection(dbm);
            }
        }

        
        /// <summary>
        /// 刷新工单数据
        /// </summary>
        /// <param name="ltt_name"></param>
        /// <param name="qjpz"></param>
        /// <param name="Username"></param>
        /// <param name="Password"></param>
        public static void SxSj(string ltt_name,string qjpz,string Username,string Password)
        {
            CDBManger cdb = null;
            try
            {
                cdb = new CDBManger { UserName=Username,Password=Password};
                StringBuilder sb = new StringBuilder();
                sb.Append(" begin  LTT_USER.SETCONFIGURATION('" + qjpz + "'); ");
                sb.Append(" LTT_USER.EDITJOB('" + ltt_name + "'); ");
                sb.Append(" G3E_UpdateDeltas.UpdateDeltasPrivate; end;");
                cdb.ExecuteNonQuerySxsj(sb.ToString());
                CYZLog.writeLog(sb.ToString());
            }
            catch (Exception ex)
            {
                CYZLog.writeLog(ex.ToString());
            }
            finally
            {
                CloseOracleConnection(cdb);
            }
        }
        /// <summary>
        /// 关闭OracleDataReader
        /// </summary>
        /// <param name="odr"></param>
        /// <param name="dbManager"> </param>
        private static void CloseOracleDataReader(OracleDataReader odr, CDBManger dbManager)
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
        private static void CloseOracleDataReader(OracleDataReader odr)
        {
            if (odr != null)
                odr.Close();
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
    }
}
