using System;
using System.Data;
using System.Configuration;
using System.Linq;
using System.Web;
using System.Web.Security;
using System.Web.UI;
using System.Web.UI.HtmlControls;
using System.Web.UI.WebControls;
using System.Web.UI.WebControls.WebParts;
using System.Xml.Linq;
using ElectronTransferDal.Common;
using ElectronTransferDal.OracleDal;
using ElectronTransferDal.XmlDal;
using ElectronTransferModel.V9_4;
using Oracle.DataAccess.Client;
using ElectronTransferFramework;
using ElectronTransferService;
using ElectronTransferModel;
using ElectronTransferModel.Base;
using System.Web.UI.MobileControls;
using System.Collections;
using System.Collections.Generic;
using ElectronTransferServiceDll;
using ElectronTransferModel.Config;
using System.IO;
using System.Reflection;
using CYZFramework.Log;
using CYZFramework.DB;
using ElectronTransferService.dll;
using ElectronTransferServiceDll;
using ElectronTransferDal.Common.Exceptions;
using System.Text.RegularExpressions;
//using System.Data.OracleClient;

namespace ElectronTransferService
{
    public class UploadEventHandler
    {
        /// <summary>
        /// 公共属性
        /// </summary>
        /// <param name="oracledb"></param>
        /// <param name="xmldb"></param>
        /// <param name="gc_id"></param>
        /// <param name="ltt_name"></param>
        /// <param name="appenshiptime"></param>
        /// <param name="qj"> </param>
        public static void COMMON_N(OracleDBManager oracledb, XmlDBManager xmldb, string gc_id, string ltt_name, DateTime appenshiptime, string qj)
        {
            try
            {
                List<ElectronTransferServicePro.InsertTempTable> InsertTempTables = new List<ElectronTransferServicePro.InsertTempTable>();
                List<Common_n> ccs = new List<Common_n>();
                List<ElectronBase> DeleteEnt = new List<ElectronBase>();

                OracleSequenceValueGenerator.Instance.DbManager = oracledb;

                var v1 = xmldb.GetEntities<Common_n>(); // .Cast<ElectronBase>();
                if (v1.Count()>0)
                {
                    foreach (var v2 in v1)
                    {
                        if (v2.EntityState == EntityState.Delete)
                        {
                            DeleteEnt.Add(v2);
                            continue;
                        }
                        long bz2 = 0;
                        long BzFid=0;
                        long? OldOwner1 = v2.OWNER1_ID;
                        long? OldOwner2 = v2.OWNER2_ID;

                        long newOwner1 = QueryOwnerIdTemp(OldOwner1, gc_id);
                        long newOwner2 = QueryOwnerIdTemp(OldOwner2, gc_id);

                        long G3E_ID = v2.G3E_ID;
                        long G3E_FID = v2.G3E_FID;

                        //QueryCommFid(v2.G3E_FID, e.Value4.ToString(), ref G3E_FID, ref G3E_ID, cdm_QueryCommFid);
                        QueryCommFid(v2.G3E_FID, gc_id, ref G3E_FID, ref G3E_ID);

                        v2.G3E_ID = G3E_ID;
                        v2.G3E_FID = G3E_FID;
                        if (!string.IsNullOrEmpty(v2.BZ2))
                        {
                            if (long.TryParse(v2.BZ2, out bz2))
                            {  
                                if(bz2!=0)
                                {
                                    QueryCommFid(bz2, gc_id, ref BzFid);
                                    v2.BZ2=BzFid.ToString();
                                }
                                
                            }

                        }

                        if (newOwner1 != 0 && newOwner2 == 0)
                        {
                            v2.OWNER1_ID = newOwner1;

                            ccs.Add(v2);
                            var vv = new ElectronTransferServicePro.InsertTempTable();
                            vv.g3e_fid = v2.G3E_FID;
                            vv.g3e_fno = v2.G3E_FNO;
                            vv.v2 = v2;
                            vv.ltt_name = ltt_name;
                            vv.GcTicket_ID = gc_id;
                            vv.Sjversion = int.Parse(v2.GetValue("Version").ToString());
                            InsertTempTables.Add(vv);
                        }

                        if (newOwner2 != 0 && newOwner1 == 0)
                        {
                            ccs.Add(v2);
                            var vv = new ElectronTransferServicePro.InsertTempTable();
                            vv.g3e_fid = v2.G3E_FID;
                            vv.g3e_fno = v2.G3E_FNO;
                            vv.v2 = v2;
                            vv.ltt_name = ltt_name;
                            vv.GcTicket_ID = gc_id;
                            vv.Sjversion = int.Parse(v2.GetValue("Version").ToString());
                            InsertTempTables.Add(vv);
                        }


                        if (newOwner2 != 0 && newOwner1 != 0)
                        {
                            v2.OWNER1_ID = newOwner1;
                            v2.OWNER2_ID = newOwner2;
                            ccs.Add(v2);
                            var vv = new ElectronTransferServicePro.InsertTempTable();
                            vv.g3e_fid = v2.G3E_FID;
                            vv.g3e_fno = v2.G3E_FNO;
                            vv.v2 = v2;
                            vv.ltt_name = ltt_name;
                            vv.GcTicket_ID = gc_id;
                            vv.Sjversion = int.Parse(v2.GetValue("Version").ToString());
                            InsertTempTables.Add(vv);
                        }

                        if (newOwner2 == 0 && newOwner1 == 0)
                        {
                            v2.OWNER1_ID = newOwner1;
                            v2.OWNER2_ID = newOwner2;
                            ccs.Add(v2);
                            var vv = new ElectronTransferServicePro.InsertTempTable();
                            vv.g3e_fid = v2.G3E_FID;
                            vv.g3e_fno = v2.G3E_FNO;
                            vv.v2 = v2;
                            vv.ltt_name = ltt_name;
                            vv.GcTicket_ID = gc_id;
                            vv.Sjversion = int.Parse(v2.GetValue("Version").ToString());
                            InsertTempTables.Add(vv);
                        }
                    }

                    oracledb.UpdateBulk(ccs.Cast<DBEntity>(), true, new ActiveJob(qj, ltt_name));
                    oracledb.DeleteBulk(DeleteEnt.Cast<DBEntity>(), true, new ActiveJob(qj, ltt_name));

                    int ltt_idd = 0;
                    string sqlltt = "select LTT_ID from ltt_identifiers  where ltt_name='" + ltt_name + "'";
                    CDBManger cdb_lttid = new CDBManger();
                    DataTable dtLttid = cdb_lttid.ExecuteTable(sqlltt);
                    if (dtLttid != null && dtLttid.Rows.Count > 0)
                    {
                        ltt_idd = int.Parse(dtLttid.Rows[0]["LTT_ID"].ToString());
                    }
                    if (cdb_lttid != null) { cdb_lttid.close(); }
                    foreach (var AppendShip in InsertTempTables)
                    {
                        InsertTempTable(
                        AppendShip.g3e_fid, AppendShip.g3e_fno, AppendShip.v2.GetValue("EntityState").ToString(),
                        AppendShip.GcTicket_ID, ltt_idd, appenshiptime, AppendShip.Sjversion);
                    }

                }
            }
            catch (NotExistException ex)
            {

            }
            catch (Exception ex)
            {
                CYZLog.writeLog(ex.ToString(), "");
                throw ex;
            }
        }
        /// <summary>
        /// 户表和集抄之间的关系表2次微调使用
        /// </summary>
        /// <param name="oracledb"></param>
        /// <param name="xmldb"></param>
        /// <param name="gc_id"></param>
        /// <param name="ltt_name"></param>
        /// <param name="appenshiptime"></param>
        public static void UploadDetailreference_n(OracleDBManager oracledb, XmlDBManager xmldb, string gc_id, string ltt_name, DateTime appenshiptime, string qj)
        {
            try
            {
                decimal? ltt_id = 0;
                List<ElectronTransferServicePro.InsertTempTable> InsertTempTables = new List<ElectronTransferServicePro.InsertTempTable>();
                OracleSequenceValueGenerator.Instance.DbManager = oracledb;
                int SjversionGlob = 0;
                var v1 = xmldb.GetEntities<Detailreference_n>();
                List<ElectronBase> insertccs = new List<ElectronBase>();
                List<ElectronBase> updateccs = new List<ElectronBase>();
                List<ElectronBase> delteccs = new List<ElectronBase>();
                List<ElectronBase> InsertEntUp = new List<ElectronBase>();//修改增量

                if (v1.Count() > 0)
                {
                    foreach (var v2 in v1)
                    {
                        long old_g3e_fid = v2.G3E_FID;
                        long old_g3e_id = v2.G3E_ID;
                        long? old_g3e_detatild = v2.G3E_DETAILID;
                        long? old_g3e_detatild2 = 0;
                        long new_g3e_fid = 0;
                        ltt_id = v2.LTT_ID;

                        var _entityStatus = v2.GetValue("EntityState");
                        if ((int)_entityStatus == (int)EntityState.Insert)
                        {
                            var vv = new ElectronTransferServicePro.InsertTempTable();
                            vv.g3e_fid = v2.G3E_FID;
                            vv.g3e_fno = v2.G3E_FNO;
                            vv.v2 = v2;
                            vv.ltt_name = ltt_name;
                            vv.GcTicket_ID = gc_id;
                            vv.Sjversion = int.Parse(v2.GetValue("Version").ToString());
                            new_g3e_fid = selectG3e_detatlid(v2.G3E_FID, v2.G3E_FNO, v2, ltt_name, gc_id, v2.LTT_ID, appenshiptime);

                            if (new_g3e_fid != 0)
                            {
                                v2.G3E_FID = new_g3e_fid;
                                v2.G3E_ID = OracleSequenceValueGenerator.Instance.GenerateTableId(v2.GetType());
                                v2.G3E_DETAILID = OracleSequenceValueGenerator.Instance.GenerateDetailId();

                                insertccs.Add(v2);
                                UpdateG3e_detatlid(v2.G3E_DETAILID, old_g3e_fid, old_g3e_id, old_g3e_detatild, v2.G3E_FID);
                                vv.g3e_fid = v2.G3E_FID;
                                vv.v2 = v2;
                                vv.Sjversion = int.Parse(v2.GetValue("Version").ToString());
                                InsertTempTables.Add(vv);

                                // InsertG3e_detatlid(v2, old_g3e_fid, old_g3e_id, old_g3e_detatild);
                                //还要新增一个序列 v2.G3E_DETAILID
                            }
                            else if (new_g3e_fid == 0)
                            {
                                old_g3e_detatild2 = InsertG3e_detatlid2(v2.G3E_FID, v2.G3E_FNO, v2, ltt_name, gc_id, v2.LTT_ID, appenshiptime, old_g3e_id, old_g3e_detatild);

                                if (old_g3e_detatild2 != 0)
                                {
                                    v2.G3E_ID = OracleSequenceValueGenerator.Instance.GenerateTableId(v2.GetType());
                                    v2.G3E_DETAILID = old_g3e_detatild2;
                                }
                                else
                                {
                                    v2.G3E_ID = OracleSequenceValueGenerator.Instance.GenerateTableId(v2.GetType());
                                    v2.G3E_DETAILID = OracleSequenceValueGenerator.Instance.GenerateDetailId();
                                }

                                insertccs.Add(v2);


                            }
                        }
                        else if ((int)_entityStatus == (int)EntityState.Update)
                        {
                            updateccs.Add(v2);
                            var vv = new ElectronTransferServicePro.InsertTempTable();
                            vv.g3e_fid = v2.G3E_FID;
                            vv.g3e_fno = v2.G3E_FNO;
                            vv.v2 = v2;
                            vv.ltt_name = ltt_name;
                            vv.GcTicket_ID = gc_id;
                            vv.Sjversion = int.Parse(v2.GetValue("Version").ToString());
                            InsertTempTables.Add(vv);
                        }
                        else if ((int)_entityStatus == (int)EntityState.Delete)
                        {
                            delteccs.Add(v2);
                            var vv = new ElectronTransferServicePro.InsertTempTable();
                            vv.g3e_fid = v2.G3E_FID;
                            vv.g3e_fno = v2.G3E_FNO;
                            vv.v2 = v2;
                            vv.ltt_name = ltt_name;
                            vv.GcTicket_ID = gc_id;
                            vv.Sjversion = int.Parse(v2.GetValue("Version").ToString());
                            InsertTempTables.Add(vv);
                        }
                    }
                    QuVersion(gc_id, ref SjversionGlob);
                    Updalist3(ref insertccs, ref updateccs, gc_id, ref InsertEntUp, SjversionGlob);
                    oracledb.InsertBulk(insertccs.Cast<DBEntity>(), true, new ActiveJob(qj, ltt_name));
                    oracledb.UpdateBulk(updateccs.Cast<DBEntity>(), true, new ActiveJob(qj, ltt_name));
                    oracledb.DeleteBulk(delteccs.Cast<DBEntity>(), true, new ActiveJob(qj, ltt_name));

                    foreach (var AppendShip in InsertTempTables)
                    {
                        InsertTempTable(
                            AppendShip.g3e_fid, AppendShip.g3e_fno, AppendShip.v2.GetValue("EntityState").ToString(),
                            AppendShip.GcTicket_ID, (int)ltt_id, appenshiptime, AppendShip.Sjversion);
                    }

                }


            }
            catch (NotExistException ex)
            {

            }
            catch (Exception ex)
            {
                CYZLog.writeLog(ex.ToString(), "");
            }
        }


        /// <summary>
        /// 户表和集抄之间的关系表
        /// </summary>
        /// <param name="oracledb"></param>
        /// <param name="xmldb"></param>
        /// <param name="gc_id"></param>
        /// <param name="ltt_name"></param>
        /// <param name="appenshiptime"></param>
        public static void UploadDetailreference_n2(OracleDBManager oracledb, XmlDBManager xmldb, string gc_id, string ltt_name, DateTime appenshiptime, string qj)
        {
            try
            {
                decimal? ltt_id = 0;
                List<ElectronTransferServicePro.InsertTempTable> InsertTempTables = new List<ElectronTransferServicePro.InsertTempTable>();
                OracleSequenceValueGenerator.Instance.DbManager = oracledb;
                var v1 = xmldb.GetEntities<Detailreference_n>();
                List<Detailreference_n> insertccs = new List<Detailreference_n>();
                List<Detailreference_n> updateccs = new List<Detailreference_n>();
                List<Detailreference_n> delteccs = new List<Detailreference_n>();
                if (v1.Count() > 0)
                {
                    foreach (var v2 in v1)
                    {
                        long old_g3e_fid = v2.G3E_FID;
                        long old_g3e_id = v2.G3E_ID;
                        long? old_g3e_detatild = v2.G3E_DETAILID;
                        long new_g3e_fid = 0;
                        long? old_g3e_detatild2 = 0;
                        ltt_id = v2.LTT_ID;


                        var _entityStatus = v2.GetValue("EntityState");
                        if ((int)_entityStatus == (int)EntityState.Insert)
                        {
                            var vv = new ElectronTransferServicePro.InsertTempTable();
                            vv.g3e_fid = v2.G3E_FID;
                            vv.g3e_fno = v2.G3E_FNO;
                            vv.v2 = v2;
                            vv.ltt_name = ltt_name;
                            vv.GcTicket_ID = gc_id;
                            vv.Sjversion = int.Parse(v2.GetValue("Version").ToString());
                            new_g3e_fid = selectG3e_detatlid(v2.G3E_FID, v2.G3E_FNO, v2, ltt_name, gc_id, v2.LTT_ID, appenshiptime);

                            if (new_g3e_fid != 0)
                            {
                                v2.G3E_FID = new_g3e_fid;
                                v2.G3E_ID = OracleSequenceValueGenerator.Instance.GenerateTableId(v2.GetType());
                                v2.G3E_DETAILID = OracleSequenceValueGenerator.Instance.GenerateDetailId();

                                insertccs.Add(v2);
                                UpdateG3e_detatlid(v2.G3E_DETAILID, old_g3e_fid, old_g3e_id, old_g3e_detatild, v2.G3E_FID);
                                vv.g3e_fid = v2.G3E_FID;
                                vv.v2 = v2;
                                vv.Sjversion = int.Parse(v2.GetValue("Version").ToString());
                                InsertTempTables.Add(vv);

                                // InsertG3e_detatlid(v2, old_g3e_fid, old_g3e_id, old_g3e_detatild);
                                //还要新增一个序列 v2.G3E_DETAILID
                            }
                            else if (new_g3e_fid == 0)
                            {


                                v2.G3E_ID = OracleSequenceValueGenerator.Instance.GenerateTableId(v2.GetType());
                                v2.G3E_DETAILID = OracleSequenceValueGenerator.Instance.GenerateDetailId();
                                insertccs.Add(v2);
                                old_g3e_detatild2 = InsertG3e_detatlid2(v2.G3E_FID, v2.G3E_FNO, v2, ltt_name, gc_id, v2.LTT_ID, appenshiptime, old_g3e_id, old_g3e_detatild);

                            }
                        }
                        else if ((int)_entityStatus == (int)EntityState.Update)
                        {
                            updateccs.Add(v2);
                            var vv = new ElectronTransferServicePro.InsertTempTable();
                            vv.g3e_fid = v2.G3E_FID;
                            vv.g3e_fno = v2.G3E_FNO;
                            vv.v2 = v2;
                            vv.ltt_name = ltt_name;
                            vv.GcTicket_ID = gc_id;
                            vv.Sjversion = int.Parse(v2.GetValue("Version").ToString());
                            InsertTempTables.Add(vv);
                        }
                        else if ((int)_entityStatus == (int)EntityState.Delete)
                        {
                            delteccs.Add(v2);
                            var vv = new ElectronTransferServicePro.InsertTempTable();
                            vv.g3e_fid = v2.G3E_FID;
                            vv.g3e_fno = v2.G3E_FNO;
                            vv.v2 = v2;
                            vv.ltt_name = ltt_name;
                            vv.GcTicket_ID = gc_id;
                            vv.Sjversion = int.Parse(v2.GetValue("Version").ToString());
                            InsertTempTables.Add(vv);
                        }
                    }
                    oracledb.InsertBulk(insertccs.Cast<DBEntity>(), true, new ActiveJob(qj, ltt_name));
                    oracledb.UpdateBulk(updateccs.Cast<DBEntity>(), true, new ActiveJob(qj, ltt_name));
                    oracledb.DeleteBulk(delteccs.Cast<DBEntity>(), true, new ActiveJob(qj, ltt_name));

                    foreach (var AppendShip in InsertTempTables)
                    {
                        InsertTempTable(
                            AppendShip.g3e_fid, AppendShip.g3e_fno, AppendShip.v2.GetValue("EntityState").ToString(),
                            AppendShip.GcTicket_ID, (int)ltt_id, appenshiptime, AppendShip.Sjversion);
                    }
                }
                
            }
            catch (NotExistException ex)
            {

            }
            catch (Exception ex)
            {
                CYZLog.writeLog(ex.ToString(), "");
            }
        }
        /// <summary>
        /// 集抄与户表的关系2次微调使用
        /// </summary>
        /// <param name="oracledb"></param>
        /// <param name="xmldb"></param>
        /// <param name="gc_id"></param>
        /// <param name="ltt_name"></param>
        /// <param name="appenshiptime"></param>
        public static void UploadGg_jx_shbd_pt(OracleDBManager oracledb, XmlDBManager xmldb, string gc_id, string ltt_name, DateTime appenshiptime, string qj)
        {
            try
            {
                decimal? ltt_id = 0;
                int SjversionGlob = 0;
                List<ElectronTransferServicePro.InsertTempTable> InsertTempTables = new List<ElectronTransferServicePro.InsertTempTable>();
                OracleSequenceValueGenerator.Instance.DbManager = oracledb;
                var v1 = xmldb.GetEntities<Gg_jx_shbd_pt>();
                // List<Gg_jx_shbd_pt> insertccs = new List<Gg_jx_shbd_pt>();
                List<ElectronBase> insertccs_s = new List<ElectronBase>();
                List<ElectronBase> updateccs = new List<ElectronBase>();
                List<ElectronBase> delteccs = new List<ElectronBase>();
                List<ElectronBase> InsertEntUp = new List<ElectronBase>();//修改增量
                if (v1.Count() > 0) 
                {
                    foreach (var v2 in v1)
                    {
                        long old_g3e_fid = v2.G3E_FID;
                        long old_g3e_id = v2.G3E_ID;
                        long? old_g3e_detatild = v2.G3E_DETAILID;
                        long new_g3e_fid = 0;
                        long new_Detailreference = 0;
                        ltt_id = v2.LTT_ID;

                        var _entityStatus = v2.GetValue("EntityState");
                        if ((int)_entityStatus == (int)EntityState.Insert)
                        {
                            var vv = new ElectronTransferServicePro.InsertTempTable();
                            vv.g3e_fid = v2.G3E_FID;
                            vv.g3e_fno = v2.G3E_FNO;
                            vv.v2 = v2;
                            vv.ltt_name = ltt_name;
                            vv.GcTicket_ID = gc_id;
                            vv.Sjversion = int.Parse(v2.GetValue("Version").ToString());
                            new_g3e_fid = selectG3e_Gg_jx_shbd_ptfid(old_g3e_fid, v2.G3E_FNO, v2, ltt_name, gc_id, v2.LTT_ID, appenshiptime);//查找户表是否是新的
                            new_Detailreference = selectG3e_Gg_jx_shbd_ptG3e_detatlid(old_g3e_detatild, v2.G3E_FNO, v2, ltt_name, gc_id, v2.LTT_ID, appenshiptime); //查找集抄是否是新的


                    
                            if (new_g3e_fid != 0 && new_Detailreference == 0)
                            {
                                v2.G3E_FID = new_g3e_fid;
                                v2.G3E_ID = OracleSequenceValueGenerator.Instance.GenerateTableId(v2.GetType());
                                insertccs_s.Add(v2);

                                Type type = TypeCache.Instance.GetTypeFromCache(typeof(DBEntity), "Gg_jx_shbd_pt_sdogeom");
                                var symb = xmldb.GetEntity(type, old_g3e_fid) as ElectronBase;




                                symb.G3E_ID = OracleSequenceValueGenerator.Instance.GenerateTableId(type);
                                symb.G3E_FID = v2.G3E_FID;
                                symb.SetValue("SDO_GID", (Decimal?)v2.G3E_ID);
                                insertccs_s.Add(symb);

                                vv.g3e_fid = new_g3e_fid;
                                vv.v2 = v2;
                                vv.Sjversion = int.Parse(v2.GetValue("Version").ToString());
                                InsertTempTables.Add(vv);

                            }
                            else if (new_g3e_fid != 0 && new_Detailreference != 0)
                            {
                                v2.G3E_DETAILID = new_Detailreference;
                                v2.G3E_FID = new_g3e_fid;
                                v2.G3E_ID = OracleSequenceValueGenerator.Instance.GenerateTableId(v2.GetType());
                                insertccs_s.Add(v2);

                                Type type = TypeCache.Instance.GetTypeFromCache(typeof(DBEntity), "Gg_jx_shbd_pt_sdogeom");
                                var symb = xmldb.GetEntity(type, old_g3e_fid) as ElectronBase;




                                symb.G3E_ID = OracleSequenceValueGenerator.Instance.GenerateTableId(type);
                                symb.G3E_FID = v2.G3E_FID;
                                symb.SetValue("SDO_GID", (Decimal?)v2.G3E_ID);
                                insertccs_s.Add(symb);
                                vv.g3e_fid = new_g3e_fid;
                                vv.v2 = v2;
                                vv.Sjversion = int.Parse(v2.GetValue("Version").ToString());
                                InsertTempTables.Add(vv);
                            }
                        }

                        if ((int)_entityStatus == (int)EntityState.Update)
                        {
                            new_Detailreference = selectG3e_Gg_jx_shbd_ptG3e_detatlid(old_g3e_detatild, v2.G3E_FNO, v2, ltt_name, gc_id, v2.LTT_ID, appenshiptime);
                            if (new_Detailreference != 0)
                            {
                                v2.G3E_DETAILID = new_Detailreference;
                                updateccs.Add(v2);
                            }
                            else
                            {
                                updateccs.Add(v2);
                            }
                            var vv = new ElectronTransferServicePro.InsertTempTable();
                            vv.g3e_fid = v2.G3E_FID;
                            vv.g3e_fno = v2.G3E_FNO;
                            vv.v2 = v2;
                            vv.ltt_name = ltt_name;
                            vv.GcTicket_ID = gc_id;
                            vv.Sjversion = int.Parse(v2.GetValue("Version").ToString());
                            InsertTempTables.Add(vv);

                        }
                        if ((int)_entityStatus == (int)EntityState.Delete)
                        {
                            delteccs.Add(v2);
                            var vv = new ElectronTransferServicePro.InsertTempTable();
                            vv.g3e_fid = v2.G3E_FID;
                            vv.g3e_fno = v2.G3E_FNO;
                            vv.v2 = v2;
                            vv.ltt_name = ltt_name;
                            vv.GcTicket_ID = gc_id;
                            vv.Sjversion = int.Parse(v2.GetValue("Version").ToString());
                            InsertTempTables.Add(vv);
                        }
                    }
                    QuVersion(gc_id, ref SjversionGlob);
                    CYZLog.writeLog("版本：", SjversionGlob.ToString());
                    Updalist2(ref insertccs_s, ref updateccs, gc_id, ref InsertEntUp, SjversionGlob);
                    oracledb.InsertBulk(insertccs_s.Cast<DBEntity>(), true, new ActiveJob(qj, ltt_name));
                    oracledb.UpdateBulk(updateccs.Cast<DBEntity>(), true, new ActiveJob(qj, ltt_name));
                    oracledb.DeleteBulk(delteccs.Cast<DBEntity>(), true, new ActiveJob(qj, ltt_name));


                    foreach (var AppendShip in InsertTempTables)
                    {
                        InsertTempTable(
                            AppendShip.g3e_fid, AppendShip.g3e_fno, AppendShip.v2.GetValue("EntityState").ToString(),
                            AppendShip.GcTicket_ID, (int)ltt_id, appenshiptime, AppendShip.Sjversion);
                    }
                }
              
            }
            catch (NotExistException ex)
            {

            }
            catch (Exception ex)
            {
                CYZLog.writeLog(ex.ToString(), "");
            }
        }



        /// <summary>
        /// 集抄与户表的关系
        /// </summary>
        /// <param name="oracledb"></param>
        /// <param name="xmldb"></param>
        /// <param name="gc_id"></param>
        /// <param name="ltt_name"></param>
        /// <param name="appenshiptime"></param>
        public static void UploadGg_jx_shbd_pt2(OracleDBManager oracledb, XmlDBManager xmldb, string gc_id, string ltt_name, DateTime appenshiptime, string qj)
        {
            try
            {
                decimal? ltt_id = 0;
                List<ElectronTransferServicePro.InsertTempTable> InsertTempTables = new List<ElectronTransferServicePro.InsertTempTable>();
                OracleSequenceValueGenerator.Instance.DbManager = oracledb;
                var v1 = xmldb.GetEntities<Gg_jx_shbd_pt>();
                // List<Gg_jx_shbd_pt> insertccs = new List<Gg_jx_shbd_pt>();
                List<ElectronBase> insertccs_s = new List<ElectronBase>();
                List<ElectronBase> updateccs = new List<ElectronBase>();
                List<ElectronBase> delteccs = new List<ElectronBase>();

                if (v1.Count() > 0) 
                {
                    foreach (var v2 in v1)
                    {
                        long old_g3e_fid = v2.G3E_FID;
                        long old_g3e_id = v2.G3E_ID;
                        long? old_g3e_detatild = v2.G3E_DETAILID;
                        long new_g3e_fid = 0;
                        long new_Detailreference = 0;
                        ltt_id = v2.LTT_ID;

                        var _entityStatus = v2.GetValue("EntityState");
                        if ((int)_entityStatus == (int)EntityState.Insert)
                        {
                            var vv = new ElectronTransferServicePro.InsertTempTable();
                            vv.g3e_fid = v2.G3E_FID;
                            vv.g3e_fno = v2.G3E_FNO;
                            vv.v2 = v2;
                            vv.ltt_name = ltt_name;
                            vv.GcTicket_ID = gc_id;
                            vv.Sjversion = int.Parse(v2.GetValue("Version").ToString());
                            new_g3e_fid = selectG3e_Gg_jx_shbd_ptfid(old_g3e_fid, v2.G3E_FNO, v2, ltt_name, gc_id, v2.LTT_ID, appenshiptime);//查找户表是否是新的
                            new_Detailreference = selectG3e_Gg_jx_shbd_ptG3e_detatlid(old_g3e_detatild, v2.G3E_FNO, v2, ltt_name, gc_id, v2.LTT_ID, appenshiptime); //查找集抄是否是新的



                            if (new_g3e_fid != 0 && new_Detailreference == 0)
                            {
                                v2.G3E_FID = new_g3e_fid;
                                v2.G3E_ID = OracleSequenceValueGenerator.Instance.GenerateTableId(v2.GetType());
                                insertccs_s.Add(v2);

                                Type type = TypeCache.Instance.GetTypeFromCache(typeof(DBEntity), "Gg_jx_shbd_pt_sdogeom");
                                var symb = xmldb.GetEntity(type, old_g3e_fid) as ElectronBase;




                                symb.G3E_ID = OracleSequenceValueGenerator.Instance.GenerateTableId(type);
                                symb.G3E_FID = v2.G3E_FID;
                                symb.SetValue("SDO_GID", (Decimal?)v2.G3E_ID);
                                insertccs_s.Add(symb);

                                vv.g3e_fid = new_g3e_fid;
                                vv.v2 = v2;
                                vv.Sjversion = int.Parse(v2.GetValue("Version").ToString());
                                InsertTempTables.Add(vv);

                            }
                            else if (new_g3e_fid != 0 && new_Detailreference != 0)
                            {
                                v2.G3E_DETAILID = new_Detailreference;
                                v2.G3E_FID = new_g3e_fid;
                                v2.G3E_ID = OracleSequenceValueGenerator.Instance.GenerateTableId(v2.GetType());
                                insertccs_s.Add(v2);

                                Type type = TypeCache.Instance.GetTypeFromCache(typeof(DBEntity), "Gg_jx_shbd_pt_sdogeom");
                                var symb = xmldb.GetEntity(type, old_g3e_fid) as ElectronBase;




                                symb.G3E_ID = OracleSequenceValueGenerator.Instance.GenerateTableId(type);
                                symb.G3E_FID = v2.G3E_FID;
                                symb.SetValue("SDO_GID", (Decimal?)v2.G3E_ID);
                                insertccs_s.Add(symb);
                                vv.g3e_fid = new_g3e_fid;
                                vv.v2 = v2;
                                vv.Sjversion = int.Parse(v2.GetValue("Version").ToString());
                                InsertTempTables.Add(vv);
                            }
                        }

                        if ((int)_entityStatus == (int)EntityState.Update)
                        {

                            new_Detailreference = selectG3e_Gg_jx_shbd_ptG3e_detatlid(old_g3e_detatild, v2.G3E_FNO, v2, ltt_name, gc_id, v2.LTT_ID, appenshiptime);
                            if (new_Detailreference != 0)
                            {
                                v2.G3E_DETAILID = new_Detailreference;
                                updateccs.Add(v2);
                            }
                            else
                            {
                                updateccs.Add(v2);
                            }
                            var vv = new ElectronTransferServicePro.InsertTempTable();
                            vv.g3e_fid = v2.G3E_FID;
                            vv.g3e_fno = v2.G3E_FNO;
                            vv.v2 = v2;
                            vv.ltt_name = ltt_name;
                            vv.GcTicket_ID = gc_id;
                            vv.Sjversion = int.Parse(v2.GetValue("Version").ToString());
                            InsertTempTables.Add(vv);

                        }
                        if ((int)_entityStatus == (int)EntityState.Delete)
                        {
                            delteccs.Add(v2);
                            var vv = new ElectronTransferServicePro.InsertTempTable();
                            vv.g3e_fid = v2.G3E_FID;
                            vv.g3e_fno = v2.G3E_FNO;
                            vv.v2 = v2;
                            vv.ltt_name = ltt_name;
                            vv.GcTicket_ID = gc_id;
                            vv.Sjversion = int.Parse(v2.GetValue("Version").ToString());
                            InsertTempTables.Add(vv);
                        }
                    }

                    oracledb.InsertBulk(insertccs_s.Cast<DBEntity>(), true, new ActiveJob(qj, ltt_name));
                    oracledb.UpdateBulk(updateccs.Cast<DBEntity>(), true, new ActiveJob(qj, ltt_name));
                    oracledb.DeleteBulk(delteccs.Cast<DBEntity>(), true, new ActiveJob(qj, ltt_name));


                    foreach (var AppendShip in InsertTempTables)
                    {
                        InsertTempTable(
                            AppendShip.g3e_fid, AppendShip.g3e_fno, AppendShip.v2.GetValue("EntityState").ToString(),
                            AppendShip.GcTicket_ID, (int)ltt_id, appenshiptime, AppendShip.Sjversion);
                    }
                }
               
            }
            catch (NotExistException ex)
            {

            }
            catch (Exception ex)
            {
                CYZLog.writeLog(ex.ToString(), "");
            }
        }


        /// <summary>
        /// 散户表坐标的添加
        /// </summary>
        /// <param name="oracledb"></param>
        /// <param name="xmldb"></param>
        /// <param name="gc_id"></param>
        /// <param name="ltt_name"></param>
        /// <param name="appenshiptime"></param>
        /// <param name="qj"></param>
        public static void UploadGg_jx_shbd_pt_sdogeom(OracleDBManager oracledb, XmlDBManager xmldb, string gc_id, string ltt_name, DateTime appenshiptime, string qj)
        {

            try
            {
                decimal? ltt_id = 0;
                List<ElectronTransferServicePro.InsertTempTable> InsertTempTables = new List<ElectronTransferServicePro.InsertTempTable>();
                OracleSequenceValueGenerator.Instance.DbManager = oracledb;
                var v1 = xmldb.GetEntities<Gg_jx_shbd_pt_sdogeom>();
                List<Gg_jx_shbd_pt_sdogeom> updateccs = new List<Gg_jx_shbd_pt_sdogeom>();
                List<Gg_jx_shbd_pt_sdogeom> delteccs = new List<Gg_jx_shbd_pt_sdogeom>();

                if (v1.Count() > 0)
                {
                    foreach (var v2 in v1)
                    {

                        ltt_id = v2.LTT_ID;

                        var _entityStatus = v2.GetValue("EntityState");


                        if ((int)_entityStatus == (int)EntityState.Update)
                        {
                            updateccs.Add(v2);
                            var vv = new ElectronTransferServicePro.InsertTempTable();
                            vv.g3e_fid = v2.G3E_FID;
                            vv.g3e_fno = v2.G3E_FNO;
                            vv.v2 = v2;
                            vv.ltt_name = ltt_name;
                            vv.GcTicket_ID = gc_id;
                            vv.Sjversion = int.Parse(v2.GetValue("Version").ToString());
                            InsertTempTables.Add(vv);

                        }
                        if ((int)_entityStatus == (int)EntityState.Delete)
                        {
                            delteccs.Add(v2);
                            var vv = new ElectronTransferServicePro.InsertTempTable();
                            vv.g3e_fid = v2.G3E_FID;
                            vv.g3e_fno = v2.G3E_FNO;
                            vv.v2 = v2;
                            vv.ltt_name = ltt_name;
                            vv.GcTicket_ID = gc_id;
                            vv.Sjversion = int.Parse(v2.GetValue("Version").ToString());
                            InsertTempTables.Add(vv);
                        }
                    }

                    oracledb.UpdateBulk(updateccs.Cast<DBEntity>(), true, new ActiveJob(qj, ltt_name));
                    oracledb.DeleteBulk(delteccs.Cast<DBEntity>(), true, new ActiveJob(qj, ltt_name));


                    foreach (var AppendShip in InsertTempTables)
                    {
                        InsertTempTable(
                            AppendShip.g3e_fid, AppendShip.g3e_fno, AppendShip.v2.GetValue("EntityState").ToString(),
                            AppendShip.GcTicket_ID, (int)ltt_id, appenshiptime, AppendShip.Sjversion);
                    }
                }

            }
            catch (NotExistException ex)
            {

            }
            catch (Exception ex)
            {
                CYZLog.writeLog(ex.ToString(), "");
            }
        }


        ///// <summary>
        /////    杂项标注修改对应的设备
        ///// </summary>
        ///// <param name="oracledb"></param>
        ///// <param name="xmldb"></param>
        ///// <param name="gc_id"></param>
        ///// <param name="ltt_name"></param>
        ///// <param name="appenshiptime"></param>
        ///// <param name="qj"> </param>
        //public static void UploadGg_gl_zxbz_n(OracleDBManager oracledb, XmlDBManager xmldb, string gc_id, string ltt_name, DateTime appenshiptime, string qj)
        //{

        //    try
        //    {
        //        decimal? ltt_id = 0;
        //        long ZxG3e_fid = 0;
        //        string DySbG3e_fid = null;
        //        List<ElectronTransferServicePro.InsertTempTable> InsertTempTables = new List<ElectronTransferServicePro.InsertTempTable>();
        //        OracleSequenceValueGenerator.Instance.DbManager = oracledb;
        //        var v1 = xmldb.GetEntities<Gg_gl_zxbz_n>();
        //        // List<Gg_jx_jlb_pt> insertccs = new List<Gg_jx_jlb_pt>();
        //        //  List<ElectronBase> insertccs_s = new List<ElectronBase>();
        //        List<ElectronBase> updateccs = new List<ElectronBase>();
        //        Type type = TypeCache.Instance.GetTypeFromCache(typeof(DBEntity), "Gg_gl_zxbz_n");
        //        if (v1.Count() > 0)
        //        {
        //            foreach (var v2 in v1)
        //            {
        //                var _entityStatus = v2.GetValue("EntityState");

        //                if ((int)_entityStatus == (int)EntityState.Insert)
        //                {
        //                    v2.G3E_ID = OracleSequenceValueGenerator.Instance.GenerateTableId(type);
        //                    UpdateGg_gl_zxbz_n_G3e_fid(v2.G3E_FID, v2.BZ_DYSB, ref ZxG3e_fid, ref DySbG3e_fid);

        //                    if (ZxG3e_fid != 0)
        //                    {
        //                        v2.G3E_FID = ZxG3e_fid;

        //                    }
        //                    if (DySbG3e_fid != null)
        //                    {
        //                        v2.BZ_DYSB = DySbG3e_fid;
        //                    }
        //                    updateccs.Add(v2);


        //                }

        //            }

        //            oracledb.UpdateBulk(updateccs.Cast<DBEntity>(), true, new ActiveJob(qj, ltt_name));



        //            foreach (var AppendShip in InsertTempTables)
        //            {
        //                InsertTempTable(
        //                    AppendShip.g3e_fid, AppendShip.g3e_fno, AppendShip.v2.GetValue("EntityState").ToString(),
        //                    AppendShip.GcTicket_ID, (int)ltt_id, appenshiptime, AppendShip.Sjversion);
        //            }
        //        }

        //    }
        //    catch (NotExistException ex)
        //    {

        //    }
        //    catch (Exception ex)
        //    {
        //        CYZLog.writeLog(ex.ToString(), "");
        //    }
        //}



        /// <summary>
        ///台架所属变压器
        /// </summary>
        /// <param name="oracledb"></param>
        /// <param name="xmldb"></param>
        /// <param name="gc_id"></param>
        /// <param name="ltt_name"></param>
        /// <param name="appenshiptime"></param>
        /// <param name="qj"> </param>
        public static void UpLoadGg_gz_tj_n(OracleDBManager oracledb, XmlDBManager xmldb, string gc_id, string ltt_name, DateTime appenshiptime, string qj)
        {

            try
            {
                decimal? ltt_id = 0;
                List<ElectronTransferServicePro.InsertTempTable> InsertTempTables = new List<ElectronTransferServicePro.InsertTempTable>();
                OracleSequenceValueGenerator.Instance.DbManager = oracledb;
                var v1 = xmldb.GetEntities<Gg_gz_tj_n>();
                // List<Gg_jx_jlb_pt> insertccs = new List<Gg_jx_jlb_pt>();
                //  List<ElectronBase> insertccs_s = new List<ElectronBase>();
                List<ElectronBase> updateccs = new List<ElectronBase>();
                Type type = TypeCache.Instance.GetTypeFromCache(typeof(DBEntity), "Gg_gz_tj_n");
                if (v1.Count() > 0) 
                {
                    foreach (var v2 in v1)
                    {
                        var _entityStatus = v2.GetValue("EntityState");

                        if ((int)_entityStatus == (int)EntityState.Insert)
                        {
                            long newFid = 0;
                            if (v2.GNWZ_FID != null)
                            {
                                newFid = QueryContainFIdTemp(long.Parse(v2.GNWZ_FID), gc_id);
                            }

                            long NewTj = QueryTemp(v2.G3E_FID, gc_id);

                            v2.G3E_ID = OracleSequenceValueGenerator.Instance.GenerateTableId(type);
                            v2.G3E_FID = NewTj;
                            v2.GNWZ_FID = newFid.ToString();
                            updateccs.Add(v2);
                        }



                    }

                    oracledb.UpdateBulk(updateccs.Cast<DBEntity>(), true, new ActiveJob(qj, ltt_name));


                }
              

            }
            catch (NotExistException ex)
            {

            }
            catch (Exception ex)
            {
                CYZLog.writeLog(ex.ToString(), "");
                throw ex;
            }
        }

        /// <summary>
        ///集抄所属变压器
        /// </summary>
        /// <param name="oracledb"></param>
        /// <param name="xmldb"></param>
        /// <param name="gc_id"></param>
        /// <param name="ltt_name"></param>
        /// <param name="appenshiptime"></param>
        /// <param name="qj"> </param>
        public static void UpLoadGg_pd_cbx_n(OracleDBManager oracledb, XmlDBManager xmldb, string gc_id, string ltt_name, DateTime appenshiptime, string qj)
        {

            try
            {
                decimal? ltt_id = 0;
                List<ElectronTransferServicePro.InsertTempTable> InsertTempTables = new List<ElectronTransferServicePro.InsertTempTable>();
                OracleSequenceValueGenerator.Instance.DbManager = oracledb;
                var v1 = xmldb.GetEntities<Gg_pd_cbx_n>();
                List<ElectronBase> updateccs = new List<ElectronBase>();
                Type type = TypeCache.Instance.GetTypeFromCache(typeof(DBEntity), "Gg_pd_cbx_n");
                if (v1.Count() > 0)
                {
                    foreach (var v2 in v1)
                    {
                        var _entityStatus = v2.GetValue("EntityState");
                        long newByqFid = 0;
                        if ((int)_entityStatus == (int)EntityState.Insert)
                        {

                            if (!string.IsNullOrEmpty(v2.SSBYQ))
                            {    
                                bool isNum2=RegexValidate("^[0-9]*$",v2.SSBYQ);
                                if (isNum2)
                                {
                                    newByqFid = QueryContainFIdTemp(long.Parse(v2.SSBYQ), gc_id);
                                }
                             
                               
                            }

                            long Newcj = QueryTemp(v2.G3E_FID, gc_id);
                            if (Newcj != 0)
                            {


                                v2.G3E_ID = OracleSequenceValueGenerator.Instance.GenerateTableId(type);
                                v2.G3E_FID = Newcj;
                                v2.SSBYQ = newByqFid.ToString();
                            }

                            updateccs.Add(v2);
                        }
                        else if ((int)_entityStatus == (int)EntityState.Update)
                        {

                            long Newbyq = QueryContainFIdTemp(long.Parse(v2.SSBYQ), gc_id);
                            if (Newbyq != 0)
                            {
                                v2.SSBYQ = Newbyq.ToString();
                                updateccs.Add(v2);
                            }
                            else
                            {
                                updateccs.Add(v2);
                            }

                        }

                    }

                    oracledb.UpdateBulk(updateccs.Cast<DBEntity>(), true, new ActiveJob(qj, ltt_name));

                }

            }
            catch (NotExistException ex)
            {

            }
            catch (Exception ex)
            {
                CYZLog.writeLog(ex.ToString(), "");
                throw ex;
            }
        }


        /// <summary>
        ///低压开关变压器
        /// </summary>
        /// <param name="oracledb"></param>
        /// <param name="xmldb"></param>
        /// <param name="gc_id"></param>
        /// <param name="ltt_name"></param>
        /// <param name="appenshiptime"></param>
        /// <param name="qj"> </param>
        public static void UpGg_pd_dykg_n(OracleDBManager oracledb, XmlDBManager xmldb, string gc_id, string ltt_name, DateTime appenshiptime, string qj)
        {

            try
            {
                decimal? ltt_id = 0;
                List<ElectronTransferServicePro.InsertTempTable> InsertTempTables = new List<ElectronTransferServicePro.InsertTempTable>();
                OracleSequenceValueGenerator.Instance.DbManager = oracledb;
                var v1 = xmldb.GetEntities<Gg_pd_dykg_n>();
                List<ElectronBase> updateccs = new List<ElectronBase>();
                Type type = TypeCache.Instance.GetTypeFromCache(typeof(DBEntity), "Gg_pd_cbx_n");
                if (v1.Count() > 0)
                {
                    foreach (var v2 in v1)
                    {
                        var _entityStatus = v2.GetValue("EntityState");
                        long newByqFid = 0;
                        if ((int)_entityStatus == (int)EntityState.Insert)
                        {

                            if (!string.IsNullOrEmpty(v2.SSBYQ))
                            {
                                bool isNum2 = RegexValidate("^[0-9]*$", v2.SSBYQ);
                                if (isNum2)
                                {
                                    newByqFid = QueryContainFIdTemp(long.Parse(v2.SSBYQ), gc_id);
                                }


                            }

                            long Newcj = QueryTemp(v2.G3E_FID, gc_id);
                            if (Newcj != 0)
                            {


                                v2.G3E_ID = OracleSequenceValueGenerator.Instance.GenerateTableId(type);
                                v2.G3E_FID = Newcj;
                                v2.SSBYQ = newByqFid.ToString();
                            }

                            updateccs.Add(v2);
                        }
                        else if ((int)_entityStatus == (int)EntityState.Update)
                        {

                            long Newbyq = QueryContainFIdTemp(long.Parse(v2.SSBYQ), gc_id);
                            if (Newbyq != 0)
                            {
                                v2.SSBYQ = Newbyq.ToString();
                                updateccs.Add(v2);
                            }
                            else
                            {
                                updateccs.Add(v2);
                            }

                        }

                    }

                    oracledb.UpdateBulk(updateccs.Cast<DBEntity>(), true, new ActiveJob(qj, ltt_name));

                }

            }
            catch (NotExistException ex)
            {

            }
            catch (Exception ex)
            {
                CYZLog.writeLog(ex.ToString(), "");
                throw ex;
            }
        }


        /// <summary>
        ///配网仪
        /// </summary>
        /// <param name="oracledb"></param>
        /// <param name="xmldb"></param>
        /// <param name="gc_id"></param>
        /// <param name="ltt_name"></param>
        /// <param name="appenshiptime"></param>
        /// <param name="qj"> </param>
        public static void UpGg_jc_pwy_n(OracleDBManager oracledb, XmlDBManager xmldb, string gc_id, string ltt_name, DateTime appenshiptime, string qj)
        {

            try
            {
                decimal? ltt_id = 0;
                List<ElectronTransferServicePro.InsertTempTable> InsertTempTables = new List<ElectronTransferServicePro.InsertTempTable>();
                OracleSequenceValueGenerator.Instance.DbManager = oracledb;
                var v1 = xmldb.GetEntities<Gg_jc_pwy_n>();
                List<ElectronBase> updateccs = new List<ElectronBase>();
                Type type = TypeCache.Instance.GetTypeFromCache(typeof(DBEntity), "Gg_pd_cbx_n");
                if (v1.Count() > 0)
                {
                    foreach (var v2 in v1)
                    {
                        var _entityStatus = v2.GetValue("EntityState");
                        long newByqFid = 0;
                        if ((int)_entityStatus == (int)EntityState.Insert)
                        {

                            if (v2.PBMC!=0)
                            {
                                bool isNum2 = RegexValidate("^[0-9]*$", v2.PBMC.ToString());
                                if (isNum2)
                                {
                                    newByqFid = QueryContainFIdTemp(v2.PBMC, gc_id);
                                }


                            }

                            long Newcj = QueryTemp(v2.G3E_FID, gc_id);
                            if (Newcj != 0)
                            {


                                v2.G3E_ID = OracleSequenceValueGenerator.Instance.GenerateTableId(type);
                                v2.G3E_FID = Newcj;
                                v2.PBMC = newByqFid;
                            }

                            updateccs.Add(v2);
                        }
                        else if ((int)_entityStatus == (int)EntityState.Update)
                        {

                            long Newbyq = QueryContainFIdTemp(v2.PBMC, gc_id);
                            if (Newbyq != 0)
                            {
                                v2.PBMC = Newbyq;
                                updateccs.Add(v2);
                            }
                            else
                            {
                                updateccs.Add(v2);
                            }

                        }

                    }

                    oracledb.UpdateBulk(updateccs.Cast<DBEntity>(), true, new ActiveJob(qj, ltt_name));

                }

            }
            catch (NotExistException ex)
            {

            }
            catch (Exception ex)
            {
                CYZLog.writeLog(ex.ToString(), "");
                throw ex;
            }
        }
        

        /// <summary>
        ///  判断是否是数字
        /// </summary>
        /// <param name="oracledb"></param>
        /// <param name="xmldb"></param>
        /// <param name="gc_id"></param>
        /// <param name="ltt_name"></param>
        /// <param name="appenshiptime"></param>  

        public static bool RegexValidate(string regexString, string validateString)
        {
            Regex regex = new Regex(regexString); return regex.IsMatch(validateString.Trim());
        }
        /// <summary>
        ///  计量表祥图关系id和变压器的关系2次微调使用
        /// </summary>
        /// <param name="oracledb"></param>
        /// <param name="xmldb"></param>
        /// <param name="gc_id"></param>
        /// <param name="ltt_name"></param>
        /// <param name="appenshiptime"></param>    

        public static void UploadGg_jx_jlb_pt(OracleDBManager oracledb, XmlDBManager xmldb, string gc_id, string ltt_name, DateTime appenshiptime, string qj)
        {
            try
            {
                decimal? ltt_id = 0;
                List<ElectronTransferServicePro.InsertTempTable> InsertTempTables = new List<ElectronTransferServicePro.InsertTempTable>();
                OracleSequenceValueGenerator.Instance.DbManager = oracledb;
                var v1 = xmldb.GetEntities<Gg_jx_jlb_pt>();
                List<ElectronBase> insertccs_s = new List<ElectronBase>();
                List<ElectronBase> updateccs = new List<ElectronBase>();
                List<ElectronBase> delteccs = new List<ElectronBase>();
                List<ElectronBase> InsertEntUp = new List<ElectronBase>();//修改增量
                int SjversionGlob = 0;
                if (v1.Count() > 0)
                {
                    foreach (var v2 in v1)
                    {
                        long old_g3e_fid = v2.G3E_FID;
                        long old_g3e_id = v2.G3E_ID;
                        long? old_g3e_detatild = v2.G3E_DETAILID;
                        long new_g3e_fid = 0;
                        long new_Detailreference = 0;
                        ltt_id = v2.LTT_ID;

                        var _entityStatus = v2.GetValue("EntityState");
                        if ((int)_entityStatus == (int)EntityState.Insert)
                        {
                            var vv = new ElectronTransferServicePro.InsertTempTable();
                            vv.g3e_fid = v2.G3E_FID;
                            vv.g3e_fno = v2.G3E_FNO;
                            vv.v2 = v2;
                            vv.ltt_name = ltt_name;
                            vv.GcTicket_ID = gc_id;
                            vv.Sjversion = int.Parse(v2.GetValue("Version").ToString());

                            new_g3e_fid = selectG3e_Gg_jx_shbd_ptfid(old_g3e_fid, v2.G3E_FNO, v2, ltt_name, gc_id, v2.LTT_ID, appenshiptime);//查找计量表是否是新的
                            new_Detailreference = selectG3e_Gg_jx_shbd_ptG3e_detatlid(old_g3e_detatild, v2.G3E_FNO, v2, ltt_name, gc_id, v2.LTT_ID, appenshiptime); //查找变压器是否是新的


                            //if (new_g3e_fid == 0 && new_Detailreference!=0)
                            //{
                            //    v2.G3E_ID = OracleSequenceValueGenerator.Instance.GenerateTableId(v2.GetType());
                            //    v2.G3E_DETAILID = new_Detailreference;
                            //    insertccs.Add(v2);
                            //    vv.v2 = v2;
                            //    InsertTempTables.Add(vv);

                            //}
                            if (new_g3e_fid != 0 && new_Detailreference == 0)
                            {
                                v2.G3E_FID = new_g3e_fid;
                                v2.G3E_ID = OracleSequenceValueGenerator.Instance.GenerateTableId(v2.GetType());
                                insertccs_s.Add(v2);
                                Type type = TypeCache.Instance.GetTypeFromCache(typeof(DBEntity), "Gg_jx_jlb_pt_sdogeom");
                                var symb = xmldb.GetEntity(type, old_g3e_fid) as ElectronBase;
                                symb.G3E_ID = OracleSequenceValueGenerator.Instance.GenerateTableId(type);
                                symb.G3E_FID = v2.G3E_FID;
                                symb.SetValue("SDO_GID", (Decimal?)v2.G3E_ID);
                                insertccs_s.Add(symb);

                                vv.g3e_fid = new_g3e_fid;
                                vv.v2 = v2;
                                InsertTempTables.Add(vv);

                            }
                            else if (new_g3e_fid != 0 && new_Detailreference != 0)
                            {
                                v2.G3E_DETAILID = new_Detailreference;
                                v2.G3E_FID = new_g3e_fid;
                                v2.G3E_ID = OracleSequenceValueGenerator.Instance.GenerateTableId(v2.GetType());
                                insertccs_s.Add(v2);
                                Type type = TypeCache.Instance.GetTypeFromCache(typeof(DBEntity), "Gg_jx_jlb_pt_sdogeom");
                                var symb = xmldb.GetEntity(type, old_g3e_fid) as ElectronBase;

                                //坐标的插入
                                symb.G3E_ID = OracleSequenceValueGenerator.Instance.GenerateTableId(type);
                                symb.G3E_FID = v2.G3E_FID;
                                symb.SetValue("SDO_GID", (Decimal?)v2.G3E_ID);
                                insertccs_s.Add(symb);

                                vv.g3e_fid = new_g3e_fid;
                                vv.v2 = v2;
                                InsertTempTables.Add(vv);
                            }
                        }

                        if ((int)_entityStatus == (int)EntityState.Update)
                        {
                            new_Detailreference = selectG3e_Gg_jx_shbd_ptG3e_detatlid(old_g3e_detatild, v2.G3E_FNO, v2, ltt_name, gc_id, v2.LTT_ID, appenshiptime);
                            if (new_Detailreference != 0)
                            {
                                v2.G3E_DETAILID = new_Detailreference;
                                updateccs.Add(v2);
                            }
                            else
                            {
                                updateccs.Add(v2);
                            }
                            var vv = new ElectronTransferServicePro.InsertTempTable();
                            vv.g3e_fid = v2.G3E_FID;
                            vv.g3e_fno = v2.G3E_FNO;
                            vv.v2 = v2;
                            vv.ltt_name = ltt_name;
                            vv.GcTicket_ID = gc_id;
                            vv.Sjversion = int.Parse(v2.GetValue("Version").ToString());
                            InsertTempTables.Add(vv);

                        }
                        if ((int)_entityStatus == (int)EntityState.Delete)
                        {
                            delteccs.Add(v2);
                            var vv = new ElectronTransferServicePro.InsertTempTable();
                            vv.g3e_fid = v2.G3E_FID;
                            vv.g3e_fno = v2.G3E_FNO;
                            vv.v2 = v2;
                            vv.ltt_name = ltt_name;
                            vv.GcTicket_ID = gc_id;
                            vv.Sjversion = int.Parse(v2.GetValue("Version").ToString());
                            InsertTempTables.Add(vv);
                        }
                    }
                    QuVersion(gc_id, ref SjversionGlob);
                    Updalist2(ref insertccs_s, ref updateccs, gc_id, ref InsertEntUp, SjversionGlob);
                    oracledb.InsertBulk(insertccs_s.Cast<DBEntity>(), true, new ActiveJob(qj, ltt_name));
                    oracledb.UpdateBulk(updateccs.Cast<DBEntity>(), true, new ActiveJob(qj, ltt_name));
                    oracledb.DeleteBulk(delteccs.Cast<DBEntity>(), true, new ActiveJob(qj, ltt_name));


                    foreach (var AppendShip in InsertTempTables)
                    {
                        InsertTempTable(
                            AppendShip.g3e_fid, AppendShip.g3e_fno, AppendShip.v2.GetValue("EntityState").ToString(),
                            AppendShip.GcTicket_ID, (int)ltt_id, appenshiptime, AppendShip.Sjversion);
                    }
                }
                
            }
            catch (NotExistException ex)
            {

            }
            catch (Exception ex)
            {
                CYZLog.writeLog(ex.ToString(), "");
            }
        }


        /// <summary>
        ///  计量表祥图关系id和变压器的关系
        /// </summary>
        /// <param name="oracledb"></param>
        /// <param name="xmldb"></param>
        /// <param name="gc_id"></param>
        /// <param name="ltt_name"></param>
        /// <param name="appenshiptime"></param>
        /// <param name="qj"> </param>    
        public static void UploadGg_jx_jlb_pt2(OracleDBManager oracledb, XmlDBManager xmldb, string gc_id, string ltt_name, DateTime appenshiptime, string qj)
        {
            try
            {
                decimal? ltt_id = 0;
                List<ElectronTransferServicePro.InsertTempTable> InsertTempTables = new List<ElectronTransferServicePro.InsertTempTable>();
                OracleSequenceValueGenerator.Instance.DbManager = oracledb;
                var v1 = xmldb.GetEntities<Gg_jx_jlb_pt>();
                List<ElectronBase> insertccs_s = new List<ElectronBase>();
                List<ElectronBase> updateccs = new List<ElectronBase>();
                List<ElectronBase> delteccs = new List<ElectronBase>();
                if(v1.Count()>0)
                {

                    foreach (var v2 in v1)
                    {
                        long old_g3e_fid = v2.G3E_FID;
                        long old_g3e_id = v2.G3E_ID;
                        long? old_g3e_detatild = v2.G3E_DETAILID;
                        long new_g3e_fid = 0;
                        long new_Detailreference = 0;
                        ltt_id = v2.LTT_ID;

                        var _entityStatus = v2.GetValue("EntityState");
                        if ((int)_entityStatus == (int)EntityState.Insert)
                        {
                            var vv = new ElectronTransferServicePro.InsertTempTable();
                            vv.g3e_fid = v2.G3E_FID;
                            vv.g3e_fno = v2.G3E_FNO;
                            vv.v2 = v2;
                            vv.ltt_name = ltt_name;
                            vv.GcTicket_ID = gc_id;
                            vv.Sjversion = int.Parse(v2.GetValue("Version").ToString());

                            new_g3e_fid = selectG3e_Gg_jx_shbd_ptfid(old_g3e_fid, v2.G3E_FNO, v2, ltt_name, gc_id, v2.LTT_ID, appenshiptime);//查找计量表是否是新的
                            new_Detailreference = selectG3e_Gg_jx_shbd_ptG3e_detatlid(old_g3e_detatild, v2.G3E_FNO, v2, ltt_name, gc_id, v2.LTT_ID, appenshiptime); //查找变压器是否是新的


                            //if (new_g3e_fid == 0 && new_Detailreference!=0)
                            //{
                            //    v2.G3E_ID = OracleSequenceValueGenerator.Instance.GenerateTableId(v2.GetType());
                            //    v2.G3E_DETAILID = new_Detailreference;
                            //    insertccs.Add(v2);
                            //    vv.v2 = v2;
                            //    InsertTempTables.Add(vv);

                            //}
                            if (new_g3e_fid != 0 && new_Detailreference == 0)
                            {
                                v2.G3E_FID = new_g3e_fid;
                                v2.G3E_ID = OracleSequenceValueGenerator.Instance.GenerateTableId(v2.GetType());
                                insertccs_s.Add(v2);

                                Type type = TypeCache.Instance.GetTypeFromCache(typeof(DBEntity), "Gg_jx_jlb_pt_sdogeom");
                                var symb = xmldb.GetEntity(type, old_g3e_fid) as ElectronBase;




                                symb.G3E_ID = OracleSequenceValueGenerator.Instance.GenerateTableId(type);
                                symb.G3E_FID = v2.G3E_FID;
                                symb.SetValue("SDO_GID", (Decimal?)v2.G3E_ID);
                                insertccs_s.Add(symb);

                                vv.g3e_fid = new_g3e_fid;
                                vv.v2 = v2;
                                InsertTempTables.Add(vv);

                            }
                            else if (new_g3e_fid != 0 && new_Detailreference != 0)
                            {
                                v2.G3E_DETAILID = new_Detailreference;
                                v2.G3E_FID = new_g3e_fid;
                                v2.G3E_ID = OracleSequenceValueGenerator.Instance.GenerateTableId(v2.GetType());
                                insertccs_s.Add(v2);
                                Type type = TypeCache.Instance.GetTypeFromCache(typeof(DBEntity), "Gg_jx_jlb_pt_sdogeom");
                                var symb = xmldb.GetEntity(type, old_g3e_fid) as ElectronBase;



                                //坐标的插入
                                symb.G3E_ID = OracleSequenceValueGenerator.Instance.GenerateTableId(type);
                                symb.G3E_FID = v2.G3E_FID;
                                symb.SetValue("SDO_GID", (Decimal?)v2.G3E_ID);
                                insertccs_s.Add(symb);

                                vv.g3e_fid = new_g3e_fid;
                                vv.v2 = v2;
                                InsertTempTables.Add(vv);
                            }

                        }

                        if ((int)_entityStatus == (int)EntityState.Update)
                        {
                            new_Detailreference = selectG3e_Gg_jx_shbd_ptG3e_detatlid(old_g3e_detatild, v2.G3E_FNO, v2, ltt_name, gc_id, v2.LTT_ID, appenshiptime);
                            if (new_Detailreference != 0)
                            {
                                v2.G3E_DETAILID = new_Detailreference;
                                updateccs.Add(v2);
                            }
                            else
                            {
                                updateccs.Add(v2);
                            }

                            var vv = new ElectronTransferServicePro.InsertTempTable();
                            vv.g3e_fid = v2.G3E_FID;
                            vv.g3e_fno = v2.G3E_FNO;
                            vv.v2 = v2;
                            vv.ltt_name = ltt_name;
                            vv.GcTicket_ID = gc_id;
                            vv.Sjversion = int.Parse(v2.GetValue("Version").ToString());
                            InsertTempTables.Add(vv);

                        }
                        if ((int)_entityStatus == (int)EntityState.Delete)
                        {
                            delteccs.Add(v2);
                            var vv = new ElectronTransferServicePro.InsertTempTable();
                            vv.g3e_fid = v2.G3E_FID;
                            vv.g3e_fno = v2.G3E_FNO;
                            vv.v2 = v2;
                            vv.ltt_name = ltt_name;
                            vv.GcTicket_ID = gc_id;
                            vv.Sjversion = int.Parse(v2.GetValue("Version").ToString());
                            InsertTempTables.Add(vv);
                        }
                    }


                    oracledb.InsertBulk(insertccs_s.Cast<DBEntity>(), true, new ActiveJob(qj, ltt_name));
                    oracledb.UpdateBulk(updateccs.Cast<DBEntity>(), true, new ActiveJob(qj, ltt_name));
                    oracledb.DeleteBulk(delteccs.Cast<DBEntity>(), true, new ActiveJob(qj, ltt_name));


                    foreach (var AppendShip in InsertTempTables)
                    {
                        InsertTempTable(
                            AppendShip.g3e_fid, AppendShip.g3e_fno, AppendShip.v2.GetValue("EntityState").ToString(),
                            AppendShip.GcTicket_ID, (int)ltt_id, appenshiptime, AppendShip.Sjversion);
                    }
                }
                
            }
            catch (NotExistException ex)
            {

            }
            catch (Exception ex)
            {
                CYZLog.writeLog(ex.ToString(), "");
            }
        }

        /// <summary>
        /// 计量表坐标
        /// </summary>
        /// <param name="oracledb"></param>
        /// <param name="xmldb"></param>
        /// <param name="gc_id"></param>
        /// <param name="ltt_name"></param>
        /// <param name="appenshiptime"></param>
        /// <param name="qj"> </param>
        public static void UploadGg_jx_jlb_pt_sdogeom(OracleDBManager oracledb, XmlDBManager xmldb, string gc_id, string ltt_name, DateTime appenshiptime, string qj)
        {
            try
            {
                decimal? ltt_id = 0;
                List<ElectronTransferServicePro.InsertTempTable> InsertTempTables = new List<ElectronTransferServicePro.InsertTempTable>();
                OracleSequenceValueGenerator.Instance.DbManager = oracledb;
                var v1 = xmldb.GetEntities<Gg_jx_jlb_pt_sdogeom>();
                // List<Gg_jx_jlb_pt> insertccs = new List<Gg_jx_jlb_pt>();
                //  List<ElectronBase> insertccs_s = new List<ElectronBase>();
                List<Gg_jx_jlb_pt_sdogeom> updateccs = new List<Gg_jx_jlb_pt_sdogeom>();
                List<Gg_jx_jlb_pt_sdogeom> delteccs = new List<Gg_jx_jlb_pt_sdogeom>();
                if(v1.Count()>0)
                {

                    foreach (var v2 in v1)
                    {

                        ltt_id = v2.LTT_ID;

                        var _entityStatus = v2.GetValue("EntityState");


                        if ((int)_entityStatus == (int)EntityState.Update)
                        {
                            updateccs.Add(v2);
                            var vv = new ElectronTransferServicePro.InsertTempTable();
                            vv.g3e_fid = v2.G3E_FID;
                            vv.g3e_fno = v2.G3E_FNO;
                            vv.v2 = v2;
                            vv.ltt_name = ltt_name;
                            vv.GcTicket_ID = gc_id;
                            vv.Sjversion = int.Parse(v2.GetValue("Version").ToString());
                            InsertTempTables.Add(vv);

                        }
                        if ((int)_entityStatus == (int)EntityState.Delete)
                        {
                            delteccs.Add(v2);
                            var vv = new ElectronTransferServicePro.InsertTempTable();
                            vv.g3e_fid = v2.G3E_FID;
                            vv.g3e_fno = v2.G3E_FNO;
                            vv.v2 = v2;
                            vv.ltt_name = ltt_name;
                            vv.GcTicket_ID = gc_id;
                            vv.Sjversion = int.Parse(v2.GetValue("Version").ToString());
                            InsertTempTables.Add(vv);
                        }
                    }

                    oracledb.UpdateBulk(updateccs.Cast<DBEntity>(), true, new ActiveJob(qj, ltt_name));
                    oracledb.DeleteBulk(delteccs.Cast<DBEntity>(), true, new ActiveJob(qj, ltt_name));


                    foreach (var AppendShip in InsertTempTables)
                    {
                        InsertTempTable(
                            AppendShip.g3e_fid, AppendShip.g3e_fno, AppendShip.v2.GetValue("EntityState").ToString(),
                            AppendShip.GcTicket_ID, (int)ltt_id, appenshiptime, AppendShip.Sjversion);
                    }
                }
                
            }
            catch (NotExistException ex)
            {

            }
            catch (Exception ex)
            {
                CYZLog.writeLog(ex.ToString(), "");
            }
        }

        /// <summary>
        /// 功能位置
        /// </summary>
        /// <param name="oracledb"></param>
        /// <param name="xmldb"></param>
        /// <param name="gc_id"></param>
        /// <param name="ltt_name"></param>
        /// <param name="appenshiptime"></param>
        /// <param name="qj"> </param>
        public static void GG_PD_GNWZMC_N(OracleDBManager oracledb, XmlDBManager xmldb, string gc_id, string ltt_name, DateTime appenshiptime, string qj)
        {
            try
            {
                List<ElectronTransferServicePro.InsertTempTable> InsertTempTables = new List<ElectronTransferServicePro.InsertTempTable>();
                List<Gg_pd_gnwzmc_n> gnwzsUpdate = new List<Gg_pd_gnwzmc_n>();
                List<Gg_pd_gnwzmc_n> gnwzsDelete = new List<Gg_pd_gnwzmc_n>();
                Type type2 = TypeCache.Instance.GetTypeFromCache(typeof(DBEntity), "Gg_pd_gnwzmc_n");
                OracleSequenceValueGenerator.Instance.DbManager = oracledb;

                var v1 = xmldb.GetEntities<Gg_pd_gnwzmc_n>();// .Cast<ElectronBase>();
                if (v1.Count()>0)
                {
                    IList<int> fnoList = new List<int> { 40, 146, 173, 177, 86, 174 };

                    IList<int> Sstj = new List<int> {148,84,146,177,81,199,180,90,82};
                    foreach (var v2 in v1)
                    {
                        var _entityStatus = v2.GetValue("EntityState");

                        if ((int)_entityStatus == (int)EntityState.Insert)
                        {
                            if (fnoList.Contains(v2.G3E_FNO))
                            {

                                long NewSskggFid = 0;
                                long NewSskggFid2 = 0;
                             ;

                                if (!string.IsNullOrEmpty(v2.GNWZ_SSKGG))
                                {
                                    if (long.TryParse(v2.GNWZ_SSKGG, out NewSskggFid2))
                                    {
                                        NewSskggFid = QueryTemp(NewSskggFid2, gc_id);
                                    }
                                }

                                if (NewSskggFid != 0)
                                {
                                    v2.GNWZ_SSKGG = NewSskggFid.ToString();
                                }

                                long NewFid = QueryTemp(v2.G3E_FID, gc_id);

                                if (NewFid != 0)
                                {
                                    v2.G3E_FID = NewFid;
                                    v2.G3E_ID = OracleSequenceValueGenerator.Instance.GenerateTableId(type2);
                                }

                                gnwzsUpdate.Add(v2);
                            }
                            if (Sstj.Contains(v2.G3E_FNO))
                            {
                                long NewTj = 0;
                                long ByqFid = 0;
                                long byqFid2 = 0;
                                if (long.TryParse(v2.GNWZ_SSTJ, out byqFid2))
                                {

                                    NewTj = QueryTemp(byqFid2, gc_id);
                                    ByqFid = QueryTemp(v2.G3E_FID, gc_id);

                                    if (NewTj == 0 && ByqFid != 0)
                                    {
                                        v2.G3E_FID = ByqFid;
                                        v2.G3E_ID = OracleSequenceValueGenerator.Instance.GenerateTableId(type2);
                                        gnwzsUpdate.Add(v2);

                                    }

                                    else if (NewTj != 0 && ByqFid == 0)
                                    {

                                        //v2.G3E_FID = ByqFid;
                                        v2.GNWZ_SSTJ = NewTj.ToString();
                                        v2.G3E_ID = OracleSequenceValueGenerator.Instance.GenerateTableId(type2);
                                        gnwzsUpdate.Add(v2);

                                    }
                                    else if (NewTj != 0 && ByqFid != 0)
                                    {


                                        v2.G3E_FID = ByqFid;
                                        v2.GNWZ_SSTJ = NewTj.ToString();
                                        v2.G3E_ID = OracleSequenceValueGenerator.Instance.GenerateTableId(type2);
                                        gnwzsUpdate.Add(v2);

                                    }
                                    else 
                                    {
                                        gnwzsUpdate.Add(v2);
                                    }
                                }
                               
                               

                            }

                          

                        }

                        else if ((EntityState) _entityStatus ==EntityState.Update)
                        {
                            long NewSskggFid2 = 0;
                            long NewSskggFid = 0;
                          
                            if (!string.IsNullOrEmpty(v2.GNWZ_SSKGG))
                            {
                              if(long.TryParse(v2.GNWZ_SSKGG,out NewSskggFid2))
                              {
                                  NewSskggFid = QueryTemp(NewSskggFid2, gc_id);
                              }  
                            }

                            if (NewSskggFid != 0)
                            {
                                v2.GNWZ_SSKGG = NewSskggFid.ToString();
                            }
                            gnwzsUpdate.Add(v2);
                            var vv = new ElectronTransferServicePro.InsertTempTable();
                            vv.g3e_fid = v2.G3E_FID;
                            vv.g3e_fno = v2.G3E_FNO;
                            vv.v2 = v2;
                            vv.ltt_name = ltt_name;
                            vv.GcTicket_ID = gc_id;
                            vv.Sjversion = int.Parse(v2.GetValue("Version").ToString());

                            InsertTempTables.Add(vv);

                        }
                        else if ((int)_entityStatus == (int)EntityState.Delete)
                        {
                            //oracledb.Delete<Gg_pd_gnwzmc_n>(v2, true, new ActiveJob("GZDL-XX", e.Value2.ToString()));
                            //InsertTempTable(v2.G3E_FID, v2.G3E_FNO, v2, e.Value2, e.Value4);
                            gnwzsDelete.Add(v2);
                            var vv = new ElectronTransferServicePro.InsertTempTable();
                            vv.g3e_fid = v2.G3E_FID;
                            vv.g3e_fno = v2.G3E_FNO;
                            vv.v2 = v2;
                            vv.ltt_name = ltt_name;
                            vv.GcTicket_ID = gc_id;
                            vv.Sjversion = int.Parse(v2.GetValue("Version").ToString());
                            InsertTempTables.Add(vv);
                        }
                    }

                    oracledb.UpdateBulk(gnwzsUpdate.Cast<DBEntity>(), true, new ActiveJob(qj, ltt_name));
                    oracledb.DeleteBulk(gnwzsDelete.Cast<DBEntity>(), true, new ActiveJob(qj, ltt_name));

                    int ltt_idd = 0;
                    string sqlltt = "select LTT_ID from ltt_identifiers  where ltt_name='" + ltt_name + "'";
                    CDBManger cdb_lttid = new CDBManger();
                    DataTable dtLttid = cdb_lttid.ExecuteTable(sqlltt);
                    if (dtLttid != null && dtLttid.Rows.Count > 0)
                    {
                        ltt_idd = int.Parse(dtLttid.Rows[0]["LTT_ID"].ToString());
                    }
                    if (cdb_lttid != null) { cdb_lttid.close(); }
                    foreach (var AppendShip in InsertTempTables)
                    {
                        InsertTempTable(
                            AppendShip.g3e_fid, AppendShip.g3e_fno, AppendShip.v2.GetValue("EntityState").ToString(),
                            AppendShip.GcTicket_ID, ltt_idd, appenshiptime, AppendShip.Sjversion);
                    }
                }
            }
            catch (NotExistException ex)
            {

            }
            catch (Exception ex)
            {
                CYZLog.writeLog(ex.ToString(), "");
                throw ex;
                return;
            }
        }



        /// <summary>
        /// 连接关系
        /// </summary>
        /// <param name="oracledb"></param>
        /// <param name="xmldb"></param>
        /// <param name="gc_id"></param>
        /// <param name="ltt_name"></param>
        /// <param name="appenshiptime"></param>
        /// <param name="qj"> </param>
        public static void CONNECTIVITY_N(OracleDBManager oracledb, XmlDBManager xmldb, string gc_id, string ltt_name, DateTime appenshiptime, string qj)
        {
            Dictionary<long?, long?> DicLong = new Dictionary<long?, long?>();
            int SjversionGlob = 0;
            try
            {
                List<ElectronTransferServicePro.InsertTempTable> InsertTempTables = new List<ElectronTransferServicePro.InsertTempTable>();
                List<Connectivity_n> conntUpdate = new List<Connectivity_n>();
                List<Connectivity_n> conntInsert = new List<Connectivity_n>();
                List<Connectivity_n> conntDelete = new List<Connectivity_n>();
                List<Connectivity_n> InsertEntUp = new List<Connectivity_n>();//修改增量

                OracleSequenceValueGenerator.Instance.DbManager = oracledb;
                var v1 = xmldb.GetEntities<Connectivity_n>();// .Cast<ElectronBase>();
                if (v1.Count()>0)
                {
                    foreach (var v2 in v1)
                    {
                        SjversionGlob = int.Parse(v2.GetValue("Version").ToString());

                        var _entityStatus = (EntityState)v2.GetValue("EntityState");
                        if ((int)_entityStatus <= 3)
                        {
                            OldDateConnUpDel(v2, gc_id, ltt_name, appenshiptime, qj, _entityStatus, ref DicLong, ref   conntUpdate, ref  conntInsert, ref   conntDelete, ref  InsertTempTables);
                        }
                        else if ((int)_entityStatus <= 9 && (int)_entityStatus > 5)
                        {
                            OldDateConnOldOld(v2, gc_id, ltt_name, appenshiptime, qj, _entityStatus, ref DicLong, ref   conntUpdate, ref  conntInsert, ref   conntDelete, ref  InsertTempTables);
                        }
                        else if ((int)_entityStatus <= 13 && (int)_entityStatus > 9)
                        {
                            OldDateConnOld_Del(v2, gc_id, ltt_name, appenshiptime, qj, _entityStatus, ref DicLong, ref   conntUpdate, ref  conntInsert, ref   conntDelete, ref  InsertTempTables);

                        }
                        else if ((int)_entityStatus <= 17 && (int)_entityStatus > 13)
                        {
                            OldDateConnOld_Add(v2, gc_id, ltt_name, appenshiptime, qj, _entityStatus, ref DicLong, ref   conntUpdate, ref  conntInsert, ref   conntDelete, ref  InsertTempTables);
                        }
                        else if ((int)_entityStatus <= 21 && (int)_entityStatus > 17)
                        {
                            OldDateConnOld_Nal(v2, gc_id, ltt_name, appenshiptime, qj, _entityStatus, ref DicLong, ref   conntUpdate, ref  conntInsert, ref   conntDelete, ref  InsertTempTables);
                        }
                        else if ((int)_entityStatus <= 25 && (int)_entityStatus > 21)
                        {
                            NewDateConnAddOld(v2, gc_id, ltt_name, appenshiptime, qj, _entityStatus, ref DicLong, ref   conntUpdate, ref  conntInsert, ref   conntDelete, ref  InsertTempTables);
                        }
                        else if ((int)_entityStatus <= 29 && (int)_entityStatus > 24)
                        {
                            NewDateConnAddDel(v2, gc_id, ltt_name, appenshiptime, qj, _entityStatus, ref DicLong, ref   conntUpdate, ref  conntInsert, ref   conntDelete, ref  InsertTempTables);
                        }
                        else if ((int)_entityStatus <= 34 && (int)_entityStatus > 29)
                        {
                            NewDateConnAddADD(v2, gc_id, ltt_name, appenshiptime, qj, _entityStatus, ref DicLong, ref   conntUpdate, ref  conntInsert, ref   conntDelete, ref  InsertTempTables);
                        }
                        else if ((int)_entityStatus <= 38 && (int)_entityStatus > 34)
                        {
                            NewDateConnAddNal(v2, gc_id, ltt_name, appenshiptime, qj, _entityStatus, ref DicLong, ref   conntUpdate, ref  conntInsert, ref   conntDelete, ref  InsertTempTables);
                        }   
                    }

                    QuVersion(gc_id, ref SjversionGlob);
                    UpdalistConn(ref conntInsert, ref conntUpdate, gc_id, ref InsertEntUp, SjversionGlob);
                    oracledb.InsertBulk(conntInsert.Cast<DBEntity>(), true, new ActiveJob(qj, ltt_name));
                    oracledb.UpdateBulk(conntUpdate.Cast<DBEntity>(), true, new ActiveJob(qj, ltt_name));
                    oracledb.DeleteBulk(conntDelete.Cast<DBEntity>(), true, new ActiveJob(qj, ltt_name));

                    int ltt_idd = 0;
                    string sqlltt = "select LTT_ID from ltt_identifiers  where ltt_name='" + ltt_name + "'";
                    CDBManger cdb_lttid = new CDBManger();
                    DataTable dtLtt = cdb_lttid.ExecuteTable(sqlltt);
                    if (dtLtt != null && dtLtt.Rows.Count > 0)
                    {
                        ltt_idd = int.Parse(dtLtt.Rows[0]["LTT_ID"].ToString());
                    }
                    if (cdb_lttid != null) { cdb_lttid.close(); }
                    foreach (var AppendShip in InsertTempTables)
                    {
                        InsertTempTable(
                            AppendShip.g3e_fid, AppendShip.g3e_fno, AppendShip.v2.GetValue("EntityState").ToString(),
                            AppendShip.GcTicket_ID, ltt_idd, appenshiptime, AppendShip.Sjversion);
                    }
                }
            }
            catch (NotExistException ex)
            {

            }
            catch (Exception ex)
            {
                CDBManger ddd = new CDBManger();
                OracleDataReader odr = ddd.ExecuteReader("select gisticket_id from  cadgis.ticketship");
                if (odr != null && odr.Read())
                {
                    CYZLog.writeLog("tttt:" + odr["gisticket_id"], "");
                }
                CloseOracleDataReader(odr, ddd);
                CYZLog.writeLog(ex.ToString(), "");
                throw ex;
                return;
            }

        }



        /// <summary>
        /// 已经存在的数据的连接关系
        /// </summary>
        /// <param name="oracledb"></param>
        /// <param name="xmldb"></param>
        /// <param name="gc_id"></param>
        /// <param name="ltt_name"></param>
        /// <param name="appenshiptime"></param>
        public static void OldDateCONNECTIVITY_N(Connectivity_n v2, string gc_id, string ltt_name, DateTime appenshiptime, string qj, EntityState _entityStatus, ref Dictionary<long?, long?> DicLong, ref  List<ElectronBase> conntUpdate, ref  List<ElectronBase> conntInsert, ref  List<ElectronBase> conntDelete, ref List<ElectronTransferServicePro.InsertTempTable> InsertTempTables)
        {

            try 
            {
               if ((int)_entityStatus == (int)EntityState.Old_Add_Add)
            {
                Int64? OldNODE1_ID = v2.NODE1_ID;
                Int64? OldNODE2_ID = v2.NODE2_ID;


                if (DicLong.ContainsKey(OldNODE1_ID))
                {

                    v2.NODE1_ID = DicLong[OldNODE1_ID];
                }
                else
                {
                    v2.NODE1_ID = OracleSequenceValueGenerator.Instance.GenerateNodeId();
                    if (!DicLong.ContainsKey(OldNODE1_ID))
                    {
                        DicLong.Add(OldNODE1_ID, v2.NODE1_ID);
                    }
                }

                if (DicLong.ContainsKey(OldNODE2_ID))
                {

                    v2.NODE2_ID = DicLong[OldNODE2_ID];
                }
                else
                {
                    v2.NODE2_ID = OracleSequenceValueGenerator.Instance.GenerateNodeId();
                    if (!DicLong.ContainsKey(OldNODE2_ID))
                    {
                        DicLong.Add(OldNODE2_ID, v2.NODE2_ID);
                    }
                }
                CYZLog.writeLog("conntInsert start：", "Old_Add_Add");
                conntUpdate.Add(v2);
                v2.SetValue("EntityState", EntityState.Update);
                var vv = new ElectronTransferServicePro.InsertTempTable();
                vv.g3e_fid = v2.G3E_FID;
                vv.g3e_fno = v2.G3E_FNO;
                vv.v2 = v2;
                vv.ltt_name = ltt_name;
                vv.GcTicket_ID = gc_id;
                vv.Sjversion = int.Parse(v2.GetValue("Version").ToString());
                InsertTempTables.Add(vv);

            }
            else if ((int)_entityStatus == (int)EntityState.Old_Add_Del)
            {

                Int64? OldNODE1_ID = v2.NODE1_ID;


                if (DicLong.ContainsKey(OldNODE1_ID))
                {
                    // DicLong.TryGetValue(OldNODE1_ID, out OldNODE1_ID);
                    v2.NODE1_ID = DicLong[OldNODE1_ID];
                }
                else
                {
                    v2.NODE1_ID = OracleSequenceValueGenerator.Instance.GenerateNodeId();
                    if (!DicLong.ContainsKey(OldNODE1_ID))
                    {
                        DicLong.Add(OldNODE1_ID, v2.NODE1_ID);
                    }
                }
                v2.NODE2_ID = 0;
                CYZLog.writeLog("conntInsert start：", "Old_Add_Del");
                conntUpdate.Add(v2);
                v2.SetValue("EntityState", EntityState.Update);
                var vv = new ElectronTransferServicePro.InsertTempTable();
                vv.g3e_fid = v2.G3E_FID;
                vv.g3e_fno = v2.G3E_FNO;
                vv.v2 = v2;
                vv.ltt_name = ltt_name;
                vv.GcTicket_ID = gc_id;
                vv.Sjversion = int.Parse(v2.GetValue("Version").ToString());
                InsertTempTables.Add(vv);
            }
            else if ((int)_entityStatus == (int)EntityState.Old_Add_Old)
            {
                Int64? OldNODE1_ID = v2.NODE1_ID;


                if (DicLong.ContainsKey(OldNODE1_ID))
                {

                    v2.NODE1_ID = DicLong[OldNODE1_ID];

                }
                else
                {
                    v2.NODE1_ID = OracleSequenceValueGenerator.Instance.GenerateNodeId();
                    if (!DicLong.ContainsKey(OldNODE1_ID))
                    {
                        DicLong.Add(OldNODE1_ID, v2.NODE1_ID);
                    }
                }
                CYZLog.writeLog("conntInsert start：", "Old_Add_Old");
                conntUpdate.Add(v2);
                v2.SetValue("EntityState", EntityState.Update);
                var vv = new ElectronTransferServicePro.InsertTempTable();
                vv.g3e_fid = v2.G3E_FID;
                vv.g3e_fno = v2.G3E_FNO;
                vv.v2 = v2;
                vv.ltt_name = ltt_name;
                vv.GcTicket_ID = gc_id;
                vv.Sjversion = int.Parse(v2.GetValue("Version").ToString());
                InsertTempTables.Add(vv);

            }
            else if ((int)_entityStatus == (int)EntityState.Old_Add_Nal)
            {

                Int64? OldNODE1_ID = v2.NODE1_ID;


                if (DicLong.ContainsKey(OldNODE1_ID))
                {
                    // DicLong.TryGetValue(OldNODE1_ID, out outOldNODE1_ID);
                    v2.NODE1_ID = DicLong[OldNODE1_ID];
                }
                else
                {
                    v2.NODE1_ID = OracleSequenceValueGenerator.Instance.GenerateNodeId();
                    if (!DicLong.ContainsKey(OldNODE1_ID))
                    {
                        DicLong.Add(OldNODE1_ID, v2.NODE1_ID);
                    }
                }
                CYZLog.writeLog("conntInsert start：", "Old_Add_Nal");
                conntUpdate.Add(v2);
                v2.SetValue("EntityState", EntityState.Update);
                var vv = new ElectronTransferServicePro.InsertTempTable();
                vv.g3e_fid = v2.G3E_FID;
                vv.g3e_fno = v2.G3E_FNO;
                vv.v2 = v2;
                vv.ltt_name = ltt_name;
                vv.GcTicket_ID = gc_id;
                vv.Sjversion = int.Parse(v2.GetValue("Version").ToString());
                InsertTempTables.Add(vv);
            }
            else if ((int)_entityStatus == (int)EntityState.Old_Del_Add)
            {
                Int64? OldNODE2_ID = v2.NODE2_ID;


                if (DicLong.ContainsKey(OldNODE2_ID))
                {

                    v2.NODE2_ID = DicLong[OldNODE2_ID];
                }
                else
                {
                    v2.NODE2_ID = OracleSequenceValueGenerator.Instance.GenerateNodeId();
                    if (!DicLong.ContainsKey(OldNODE2_ID))
                    {
                        DicLong.Add(OldNODE2_ID, v2.NODE2_ID);
                    }
                }
                v2.NODE1_ID = 0;
                CYZLog.writeLog("conntInsert start：", "Old_Del_Add");
                conntUpdate.Add(v2);
                v2.SetValue("EntityState", EntityState.Update);
                var vv = new ElectronTransferServicePro.InsertTempTable();
                vv.g3e_fid = v2.G3E_FID;
                vv.g3e_fno = v2.G3E_FNO;
                vv.v2 = v2;
                vv.ltt_name = ltt_name;
                vv.GcTicket_ID = gc_id;
                vv.Sjversion = int.Parse(v2.GetValue("Version").ToString());
                InsertTempTables.Add(vv);
            }


            else if ((int)_entityStatus == (int)EntityState.Old_Del_Del)
            {

                v2.NODE1_ID = 0;
                v2.NODE2_ID = 0;
                CYZLog.writeLog("conntInsert start：", "Old_Del_Del");
                conntUpdate.Add(v2);
                v2.SetValue("EntityState", EntityState.Update);
                var vv = new ElectronTransferServicePro.InsertTempTable();
                vv.g3e_fid = v2.G3E_FID;
                vv.g3e_fno = v2.G3E_FNO;
                vv.v2 = v2;
                vv.ltt_name = ltt_name;
                vv.GcTicket_ID = gc_id;
                vv.Sjversion = int.Parse(v2.GetValue("Version").ToString());
                InsertTempTables.Add(vv);
            }

            else if ((int)_entityStatus == (int)EntityState.Old_Del_Nal)
            {
                v2.NODE1_ID = 0;
                CYZLog.writeLog("conntInsert start：", "Old_Del_Nal");
                conntUpdate.Add(v2);
                v2.SetValue("EntityState", EntityState.Update);
                var vv = new ElectronTransferServicePro.InsertTempTable();
                vv.g3e_fid = v2.G3E_FID;
                vv.g3e_fno = v2.G3E_FNO;
                vv.v2 = v2;
                vv.ltt_name = ltt_name;
                vv.GcTicket_ID = gc_id;
                vv.Sjversion = int.Parse(v2.GetValue("Version").ToString());
                InsertTempTables.Add(vv);
            }

            else if ((int)_entityStatus == (int)EntityState.Old_Del_Old)
            {
                v2.NODE1_ID = 0;
                CYZLog.writeLog("conntInsert start：", "Old_Del_Old");
                conntUpdate.Add(v2);
                v2.SetValue("EntityState", EntityState.Update);
                var vv = new ElectronTransferServicePro.InsertTempTable();
                vv.g3e_fid = v2.G3E_FID;
                vv.g3e_fno = v2.G3E_FNO;
                vv.v2 = v2;
                vv.ltt_name = ltt_name;
                vv.GcTicket_ID = gc_id;
                vv.Sjversion = int.Parse(v2.GetValue("Version").ToString());
                InsertTempTables.Add(vv);
            }
            else if ((int)_entityStatus == (int)EntityState.Old_Nal_Add)
            {
                Int64? OldNODE2_ID = v2.NODE2_ID;


                if (DicLong.ContainsKey(OldNODE2_ID))
                {

                    v2.NODE2_ID = DicLong[OldNODE2_ID];
                }
                else
                {
                    v2.NODE2_ID = OracleSequenceValueGenerator.Instance.GenerateNodeId();
                    if (!DicLong.ContainsKey(OldNODE2_ID))
                    {
                        DicLong.Add(OldNODE2_ID, v2.NODE2_ID);
                    }
                }
                CYZLog.writeLog("conntInsert start：", "Old_Nal_Add");
                conntUpdate.Add(v2);
                v2.SetValue("EntityState", EntityState.Update);
                var vv = new ElectronTransferServicePro.InsertTempTable();
                vv.g3e_fid = v2.G3E_FID;
                vv.g3e_fno = v2.G3E_FNO;
                vv.v2 = v2;
                vv.ltt_name = ltt_name;
                vv.GcTicket_ID = gc_id;
                vv.Sjversion = int.Parse(v2.GetValue("Version").ToString());
                InsertTempTables.Add(vv);
            }
            else if ((int)_entityStatus == (int)EntityState.Old_Nal_Del)
            {
                v2.NODE2_ID = 0;
                CYZLog.writeLog("conntInsert start：", "Old_Nal_Del");
                conntUpdate.Add(v2);
                v2.SetValue("EntityState", EntityState.Update);
                var vv = new ElectronTransferServicePro.InsertTempTable();
                vv.g3e_fid = v2.G3E_FID;
                vv.g3e_fno = v2.G3E_FNO;
                vv.v2 = v2;
                vv.ltt_name = ltt_name;
                vv.GcTicket_ID = gc_id;
                vv.Sjversion = int.Parse(v2.GetValue("Version").ToString());
                InsertTempTables.Add(vv);
            }
            else if ((int)_entityStatus == (int)EntityState.Old_Nal_Nal)
            {
                CYZLog.writeLog("conntInsert start：", "Old_Nal_Nal");
                conntUpdate.Add(v2);
                v2.SetValue("EntityState", EntityState.Update);
                var vv = new ElectronTransferServicePro.InsertTempTable();
                vv.g3e_fid = v2.G3E_FID;
                vv.g3e_fno = v2.G3E_FNO;
                vv.v2 = v2;
                vv.ltt_name = ltt_name;
                vv.GcTicket_ID = gc_id;
                vv.Sjversion = int.Parse(v2.GetValue("Version").ToString());
                InsertTempTables.Add(vv);
            }
            else if ((int)_entityStatus == (int)EntityState.Old_Nal_Old)
            {
                CYZLog.writeLog("conntInsert start：", "Old_Nal_Old");
                conntUpdate.Add(v2);
                v2.SetValue("EntityState", EntityState.Update);
                var vv = new ElectronTransferServicePro.InsertTempTable();
                vv.g3e_fid = v2.G3E_FID;
                vv.g3e_fno = v2.G3E_FNO;
                vv.v2 = v2;
                vv.ltt_name = ltt_name;
                vv.GcTicket_ID = gc_id;
                vv.Sjversion = int.Parse(v2.GetValue("Version").ToString());
                InsertTempTables.Add(vv);
            }
            else if ((int)_entityStatus == (int)EntityState.Old_Old_Add)
            {

                Int64? OldNODE2_ID = v2.NODE2_ID;


                if (DicLong.ContainsKey(OldNODE2_ID))
                {

                    v2.NODE2_ID = DicLong[OldNODE2_ID];
                }
                else
                {
                    v2.NODE2_ID = OracleSequenceValueGenerator.Instance.GenerateNodeId();
                    if (!DicLong.ContainsKey(OldNODE2_ID))
                    {
                        DicLong.Add(OldNODE2_ID, v2.NODE2_ID);
                    }
                }
                CYZLog.writeLog("conntInsert start：", "Old_Old_Add");
                conntUpdate.Add(v2);
                v2.SetValue("EntityState", EntityState.Update);
                var vv = new ElectronTransferServicePro.InsertTempTable();
                vv.g3e_fid = v2.G3E_FID;
                vv.g3e_fno = v2.G3E_FNO;
                vv.v2 = v2;
                vv.ltt_name = ltt_name;
                vv.GcTicket_ID = gc_id;
                vv.Sjversion = int.Parse(v2.GetValue("Version").ToString());
                InsertTempTables.Add(vv);
            }
            else if ((int)_entityStatus == (int)EntityState.Old_Old_Del)
            {
                v2.NODE2_ID = 0;
                CYZLog.writeLog("conntInsert start：", "Old_Old_Del");
                conntUpdate.Add(v2);
                v2.SetValue("EntityState", EntityState.Update);
                var vv = new ElectronTransferServicePro.InsertTempTable();
                vv.g3e_fid = v2.G3E_FID;
                vv.g3e_fno = v2.G3E_FNO;
                vv.v2 = v2;
                vv.ltt_name = ltt_name;
                vv.GcTicket_ID = gc_id;
                vv.Sjversion = int.Parse(v2.GetValue("Version").ToString());
                InsertTempTables.Add(vv);
            }

            else if ((int)_entityStatus == (int)EntityState.Old_Old_Nal)
            {

                conntUpdate.Add(v2);
                CYZLog.writeLog("conntInsert start：", "Old_Old_Nal");
                v2.SetValue("EntityState", EntityState.Update);
                var vv = new ElectronTransferServicePro.InsertTempTable();
                vv.g3e_fid = v2.G3E_FID;
                vv.g3e_fno = v2.G3E_FNO;
                vv.v2 = v2;
                vv.ltt_name = ltt_name;
                vv.GcTicket_ID = gc_id;
                vv.Sjversion = int.Parse(v2.GetValue("Version").ToString());
                InsertTempTables.Add(vv);
            }

            else if ((int)_entityStatus == (int)EntityState.Old_Old_Old)
            {
                conntUpdate.Add(v2);
                CYZLog.writeLog("conntInsert start：", "Old_Old_Old");
                v2.SetValue("EntityState", EntityState.Update);
                var vv = new ElectronTransferServicePro.InsertTempTable();
                vv.g3e_fid = v2.G3E_FID;
                vv.g3e_fno = v2.G3E_FNO;
                vv.v2 = v2;
                vv.ltt_name = ltt_name;
                vv.GcTicket_ID = gc_id;
                vv.Sjversion = int.Parse(v2.GetValue("Version").ToString());
                InsertTempTables.Add(vv);
            }
            else if (_entityStatus == EntityState.Update)
               {
                   CYZLog.writeLog("conntInsert start：", "Update");
                   conntUpdate.Add(v2);
                   v2.SetValue("EntityState", EntityState.Update);
                   var vv = new ElectronTransferServicePro.InsertTempTable();
                   vv.g3e_fid = v2.G3E_FID;
                   vv.g3e_fno = v2.G3E_FNO;
                   vv.v2 = v2;
                   vv.ltt_name = ltt_name;
                   vv.GcTicket_ID = gc_id;
                   vv.Sjversion = int.Parse(v2.GetValue("Version").ToString());
                   InsertTempTables.Add(vv);
               }
               else if (_entityStatus == EntityState.Delete)
               {
                   CYZLog.writeLog("conntInsert start：", "Delete");
                   conntDelete.Add(v2);
                   v2.SetValue("EntityState", EntityState.Delete);
                   var vv = new ElectronTransferServicePro.InsertTempTable();
                   vv.g3e_fid = v2.G3E_FID;
                   vv.g3e_fno = v2.G3E_FNO;
                   vv.v2 = v2;
                   vv.ltt_name = ltt_name;
                   vv.GcTicket_ID = gc_id;
                   vv.Sjversion = int.Parse(v2.GetValue("Version").ToString());
                   InsertTempTables.Add(vv);
               }
              
           
            }
            catch (NotExistException ex)
            {

            }
            catch(Exception ex)
            {
                CYZLog.writeLog("connn旧的连接关系",ex.ToString());
                throw;
            }
           
        }

        
        /// <summary>
        /// update和delete的所有选项
        /// </summary>
        /// <param name="oracledb"></param>
        /// <param name="xmldb"></param>
        /// <param name="gc_id"></param>
        /// <param name="ltt_name"></param>
        public static void OldDateConnUpDel(Connectivity_n v2, string gc_id, string ltt_name, DateTime appenshiptime, string qj, EntityState _entityStatus, ref Dictionary<long?, long?> DicLong, ref  List<Connectivity_n> conntUpdate, ref  List<Connectivity_n> conntInsert, ref  List<Connectivity_n> conntDelete, ref List<ElectronTransferServicePro.InsertTempTable> InsertTempTables)
        {
            try
            {

                if (_entityStatus == EntityState.Update)
                {
                    CYZLog.writeLog("conntInsert start：", "Update");
                    conntUpdate.Add(v2);
                    v2.SetValue("EntityState", EntityState.Update);
                    var vv = new ElectronTransferServicePro.InsertTempTable();
                    vv.g3e_fid = v2.G3E_FID;
                    vv.g3e_fno = v2.G3E_FNO;
                    vv.v2 = v2;
                    vv.ltt_name = ltt_name;
                    vv.GcTicket_ID = gc_id;
                    vv.Sjversion = int.Parse(v2.GetValue("Version").ToString());
                    InsertTempTables.Add(vv);
                }
                else if (_entityStatus == EntityState.Delete)
                {
                    CYZLog.writeLog("conntInsert start：", "Delete");
                    conntDelete.Add(v2);
                    v2.SetValue("EntityState", EntityState.Delete);
                    var vv = new ElectronTransferServicePro.InsertTempTable();
                    vv.g3e_fid = v2.G3E_FID;
                    vv.g3e_fno = v2.G3E_FNO;
                    vv.v2 = v2;
                    vv.ltt_name = ltt_name;
                    vv.GcTicket_ID = gc_id;
                    vv.Sjversion = int.Parse(v2.GetValue("Version").ToString());
                    InsertTempTables.Add(vv);
                }
            }
            catch (Exception ex)
            {
                CYZLog.writeLog("connn旧的连接关系", ex.ToString());
                throw ex;
            }
        }

         /// <summary>
        /// Old_Old的所有选项
        /// </summary>
        /// <param name="oracledb"></param>
        /// <param name="xmldb"></param>
        /// <param name="gc_id"></param>
        /// <param name="ltt_name"></param>
        public static void OldDateConnOldOld(Connectivity_n v2, string gc_id, string ltt_name, DateTime appenshiptime, string qj, EntityState _entityStatus, ref Dictionary<long?, long?> DicLong, ref  List<Connectivity_n> conntUpdate, ref  List<Connectivity_n> conntInsert, ref  List<Connectivity_n> conntDelete, ref List<ElectronTransferServicePro.InsertTempTable> InsertTempTables)
        {
            try 
            {
                if ((int)_entityStatus == (int)EntityState.Old_Old_Old)
                {
                    conntUpdate.Add(v2);
                    CYZLog.writeLog("conntInsert start：", "Old_Old_Old");
                    v2.SetValue("EntityState", EntityState.Update);
                    var vv = new ElectronTransferServicePro.InsertTempTable();
                    vv.g3e_fid = v2.G3E_FID;
                    vv.g3e_fno = v2.G3E_FNO;
                    vv.v2 = v2;
                    vv.ltt_name = ltt_name;
                    vv.GcTicket_ID = gc_id;
                    vv.Sjversion = int.Parse(v2.GetValue("Version").ToString());
                    InsertTempTables.Add(vv);
                }
                else if ((int)_entityStatus == (int)EntityState.Old_Old_Del)
                {
                    v2.NODE2_ID = 0;
                    CYZLog.writeLog("conntInsert start：", "Old_Old_Del");
                    conntUpdate.Add(v2);
                    v2.SetValue("EntityState", EntityState.Update);
                    var vv = new ElectronTransferServicePro.InsertTempTable();
                    vv.g3e_fid = v2.G3E_FID;
                    vv.g3e_fno = v2.G3E_FNO;
                    vv.v2 = v2;
                    vv.ltt_name = ltt_name;
                    vv.GcTicket_ID = gc_id;
                    vv.Sjversion = int.Parse(v2.GetValue("Version").ToString());
                    InsertTempTables.Add(vv);
                }
                else if ((int)_entityStatus == (int)EntityState.Old_Old_Add)
                {

                    Int64? OldNODE2_ID = v2.NODE2_ID;

                    if (DicLong.ContainsKey(OldNODE2_ID))
                    {

                        v2.NODE2_ID = DicLong[OldNODE2_ID];
                    }
                    else
                    {
                        v2.NODE2_ID = OracleSequenceValueGenerator.Instance.GenerateNodeId();
                        if (!DicLong.ContainsKey(OldNODE2_ID))
                        {
                            DicLong.Add(OldNODE2_ID, v2.NODE2_ID);
                        }
                    }
                    CYZLog.writeLog("conntInsert start：", "Old_Old_Add");
                    conntUpdate.Add(v2);
                    v2.SetValue("EntityState", EntityState.Update);
                    var vv = new ElectronTransferServicePro.InsertTempTable();
                    vv.g3e_fid = v2.G3E_FID;
                    vv.g3e_fno = v2.G3E_FNO;
                    vv.v2 = v2;
                    vv.ltt_name = ltt_name;
                    vv.GcTicket_ID = gc_id;
                    vv.Sjversion = int.Parse(v2.GetValue("Version").ToString());
                    InsertTempTables.Add(vv);
                }
                else if ((int)_entityStatus == (int)EntityState.Old_Old_Nal)
                {

                    conntUpdate.Add(v2);
                    CYZLog.writeLog("conntInsert start：", "Old_Old_Nal");
                    v2.SetValue("EntityState", EntityState.Update);
                    var vv = new ElectronTransferServicePro.InsertTempTable();
                    vv.g3e_fid = v2.G3E_FID;
                    vv.g3e_fno = v2.G3E_FNO;
                    vv.v2 = v2;
                    vv.ltt_name = ltt_name;
                    vv.GcTicket_ID = gc_id;
                    vv.Sjversion = int.Parse(v2.GetValue("Version").ToString());
                    InsertTempTables.Add(vv);
                } 

            }catch(Exception ex)
            {

                CYZLog.writeLog("connn旧的连接关系", ex.ToString());
                throw ex;
            }
        }

        
         /// <summary>
        /// Old_Del的所有选项
        /// </summary>
        /// <param name="oracledb"></param>
        /// <param name="xmldb"></param>
        /// <param name="gc_id"></param>
        /// <param name="ltt_name"></param>
        public static void OldDateConnOld_Del(Connectivity_n v2, string gc_id, string ltt_name, DateTime appenshiptime, string qj, EntityState _entityStatus, ref Dictionary<long?, long?> DicLong, ref  List<Connectivity_n> conntUpdate, ref  List<Connectivity_n> conntInsert, ref  List<Connectivity_n> conntDelete, ref List<ElectronTransferServicePro.InsertTempTable> InsertTempTables)
        {
            try {
                if ((int)_entityStatus == (int)EntityState.Old_Del_Old)
                {
                    v2.NODE1_ID = 0;
                    CYZLog.writeLog("conntInsert start：", "Old_Del_Old");
                    conntUpdate.Add(v2);
                    v2.SetValue("EntityState", EntityState.Update);
                    var vv = new ElectronTransferServicePro.InsertTempTable();
                    vv.g3e_fid = v2.G3E_FID;
                    vv.g3e_fno = v2.G3E_FNO;
                    vv.v2 = v2;
                    vv.ltt_name = ltt_name;
                    vv.GcTicket_ID = gc_id;
                    vv.Sjversion = int.Parse(v2.GetValue("Version").ToString());
                    InsertTempTables.Add(vv);
                }

                else if ((int)_entityStatus == (int)EntityState.Old_Del_Del)
                {

                    v2.NODE1_ID = 0;
                    v2.NODE2_ID = 0;
                    CYZLog.writeLog("conntInsert start：", "Old_Del_Del");
                    conntUpdate.Add(v2);
                    v2.SetValue("EntityState", EntityState.Update);
                    var vv = new ElectronTransferServicePro.InsertTempTable();
                    vv.g3e_fid = v2.G3E_FID;
                    vv.g3e_fno = v2.G3E_FNO;
                    vv.v2 = v2;
                    vv.ltt_name = ltt_name;
                    vv.GcTicket_ID = gc_id;
                    vv.Sjversion = int.Parse(v2.GetValue("Version").ToString());
                    InsertTempTables.Add(vv);
                }
                else if ((int)_entityStatus == (int)EntityState.Old_Del_Add)
                {
                    Int64? OldNODE2_ID = v2.NODE2_ID;


                    if (DicLong.ContainsKey(OldNODE2_ID))
                    {

                        v2.NODE2_ID = DicLong[OldNODE2_ID];
                    }
                    else
                    {
                        v2.NODE2_ID = OracleSequenceValueGenerator.Instance.GenerateNodeId();
                        if (!DicLong.ContainsKey(OldNODE2_ID))
                        {
                            DicLong.Add(OldNODE2_ID, v2.NODE2_ID);
                        }
                    }
                    v2.NODE1_ID = 0;
                    CYZLog.writeLog("conntInsert start：", "Old_Del_Add");
                    conntUpdate.Add(v2);
                    v2.SetValue("EntityState", EntityState.Update);
                    var vv = new ElectronTransferServicePro.InsertTempTable();
                    vv.g3e_fid = v2.G3E_FID;
                    vv.g3e_fno = v2.G3E_FNO;
                    vv.v2 = v2;
                    vv.ltt_name = ltt_name;
                    vv.GcTicket_ID = gc_id;
                    vv.Sjversion = int.Parse(v2.GetValue("Version").ToString());
                    InsertTempTables.Add(vv);
                }
                else if ((int)_entityStatus == (int)EntityState.Old_Del_Nal)
                {
                    v2.NODE1_ID = 0;
                    CYZLog.writeLog("conntInsert start：", "Old_Del_Nal");
                    conntUpdate.Add(v2);
                    v2.SetValue("EntityState", EntityState.Update);
                    var vv = new ElectronTransferServicePro.InsertTempTable();
                    vv.g3e_fid = v2.G3E_FID;
                    vv.g3e_fno = v2.G3E_FNO;
                    vv.v2 = v2;
                    vv.ltt_name = ltt_name;
                    vv.GcTicket_ID = gc_id;
                    vv.Sjversion = int.Parse(v2.GetValue("Version").ToString());
                    InsertTempTables.Add(vv);
                }
            }
            catch (Exception ex)
            {

                CYZLog.writeLog("connn旧的连接关系", ex.ToString());
                throw ex;
            }
        }

           /// <summary>
        ///  Old_Add的所有选项
        /// </summary>
        /// <param name="oracledb"></param>
        /// <param name="xmldb"></param>
        /// <param name="gc_id"></param>
        /// <param name="ltt_name"></param>
        public static void OldDateConnOld_Add(Connectivity_n v2, string gc_id, string ltt_name, DateTime appenshiptime, string qj, EntityState _entityStatus, ref Dictionary<long?, long?> DicLong, ref  List<Connectivity_n> conntUpdate, ref  List<Connectivity_n> conntInsert, ref  List<Connectivity_n> conntDelete, ref List<ElectronTransferServicePro.InsertTempTable> InsertTempTables)
        {
            try
            {
                if ((int)_entityStatus == (int)EntityState.Old_Add_Old)
                {
                    Int64? OldNODE1_ID = v2.NODE1_ID;


                    if (DicLong.ContainsKey(OldNODE1_ID))
                    {

                        v2.NODE1_ID = DicLong[OldNODE1_ID];

                    }
                    else
                    {
                        v2.NODE1_ID = OracleSequenceValueGenerator.Instance.GenerateNodeId();
                        if (!DicLong.ContainsKey(OldNODE1_ID))
                        {
                            DicLong.Add(OldNODE1_ID, v2.NODE1_ID);
                        }
                    }
                    CYZLog.writeLog("conntInsert start：", "Old_Add_Old");
                    conntUpdate.Add(v2);
                    v2.SetValue("EntityState", EntityState.Update);
                    var vv = new ElectronTransferServicePro.InsertTempTable();
                    vv.g3e_fid = v2.G3E_FID;
                    vv.g3e_fno = v2.G3E_FNO;
                    vv.v2 = v2;
                    vv.ltt_name = ltt_name;
                    vv.GcTicket_ID = gc_id;
                    vv.Sjversion = int.Parse(v2.GetValue("Version").ToString());
                    InsertTempTables.Add(vv);

                }
                else if ((int)_entityStatus == (int)EntityState.Old_Add_Del)
                {

                    Int64? OldNODE1_ID = v2.NODE1_ID;


                    if (DicLong.ContainsKey(OldNODE1_ID))
                    {
                        // DicLong.TryGetValue(OldNODE1_ID, out OldNODE1_ID);
                        v2.NODE1_ID = DicLong[OldNODE1_ID];
                    }
                    else
                    {
                        v2.NODE1_ID = OracleSequenceValueGenerator.Instance.GenerateNodeId();
                        if (!DicLong.ContainsKey(OldNODE1_ID))
                        {
                            DicLong.Add(OldNODE1_ID, v2.NODE1_ID);
                        }
                    }
                    v2.NODE2_ID = 0;
                    CYZLog.writeLog("conntInsert start：", "Old_Add_Del");
                    conntUpdate.Add(v2);
                    v2.SetValue("EntityState", EntityState.Update);
                    var vv = new ElectronTransferServicePro.InsertTempTable();
                    vv.g3e_fid = v2.G3E_FID;
                    vv.g3e_fno = v2.G3E_FNO;
                    vv.v2 = v2;
                    vv.ltt_name = ltt_name;
                    vv.GcTicket_ID = gc_id;
                    vv.Sjversion = int.Parse(v2.GetValue("Version").ToString());
                    InsertTempTables.Add(vv);
                }
                else if ((int)_entityStatus == (int)EntityState.Old_Add_Add)
                {
                    Int64? OldNODE1_ID = v2.NODE1_ID;
                    Int64? OldNODE2_ID = v2.NODE2_ID;


                    if (DicLong.ContainsKey(OldNODE1_ID))
                    {

                        v2.NODE1_ID = DicLong[OldNODE1_ID];
                    }
                    else
                    {
                        v2.NODE1_ID = OracleSequenceValueGenerator.Instance.GenerateNodeId();
                        if (!DicLong.ContainsKey(OldNODE1_ID))
                        {
                            DicLong.Add(OldNODE1_ID, v2.NODE1_ID);
                        }
                    }

                    if (DicLong.ContainsKey(OldNODE2_ID))
                    {

                        v2.NODE2_ID = DicLong[OldNODE2_ID];
                    }
                    else
                    {
                        v2.NODE2_ID = OracleSequenceValueGenerator.Instance.GenerateNodeId();
                        if (!DicLong.ContainsKey(OldNODE2_ID))
                        {
                            DicLong.Add(OldNODE2_ID, v2.NODE2_ID);
                        }
                    }
                    CYZLog.writeLog("conntInsert start：", "Old_Add_Add");
                    conntUpdate.Add(v2);
                    v2.SetValue("EntityState", EntityState.Update);
                    var vv = new ElectronTransferServicePro.InsertTempTable();
                    vv.g3e_fid = v2.G3E_FID;
                    vv.g3e_fno = v2.G3E_FNO;
                    vv.v2 = v2;
                    vv.ltt_name = ltt_name;
                    vv.GcTicket_ID = gc_id;
                    vv.Sjversion = int.Parse(v2.GetValue("Version").ToString());
                    InsertTempTables.Add(vv);

                }
                else if ((int)_entityStatus == (int)EntityState.Old_Add_Nal)
                {

                    Int64? OldNODE1_ID = v2.NODE1_ID;


                    if (DicLong.ContainsKey(OldNODE1_ID))
                    {
                        // DicLong.TryGetValue(OldNODE1_ID, out outOldNODE1_ID);
                        v2.NODE1_ID = DicLong[OldNODE1_ID];
                    }
                    else
                    {
                        v2.NODE1_ID = OracleSequenceValueGenerator.Instance.GenerateNodeId();
                        if (!DicLong.ContainsKey(OldNODE1_ID))
                        {
                            DicLong.Add(OldNODE1_ID, v2.NODE1_ID);
                        }
                    }
                    CYZLog.writeLog("conntInsert start：", "Old_Add_Nal");
                    conntUpdate.Add(v2);
                    v2.SetValue("EntityState", EntityState.Update);
                    var vv = new ElectronTransferServicePro.InsertTempTable();
                    vv.g3e_fid = v2.G3E_FID;
                    vv.g3e_fno = v2.G3E_FNO;
                    vv.v2 = v2;
                    vv.ltt_name = ltt_name;
                    vv.GcTicket_ID = gc_id;
                    vv.Sjversion = int.Parse(v2.GetValue("Version").ToString());
                    InsertTempTables.Add(vv);
                }
            }
            catch (Exception ex)
            {
                CYZLog.writeLog("connn旧的连接关系", ex.ToString());
                throw ex;
            }
        }

            /// <summary>
        ///  Old_Nal的所有选项
        /// </summary>
        /// <param name="oracledb"></param>
        /// <param name="xmldb"></param>
        /// <param name="gc_id"></param>
        /// <param name="ltt_name"></param>
        public static void OldDateConnOld_Nal(Connectivity_n v2, string gc_id, string ltt_name, DateTime appenshiptime, string qj, EntityState _entityStatus, ref Dictionary<long?, long?> DicLong, ref  List<Connectivity_n> conntUpdate, ref  List<Connectivity_n> conntInsert, ref  List<Connectivity_n> conntDelete, ref List<ElectronTransferServicePro.InsertTempTable> InsertTempTables)
        {
            try
            {
                if ((int)_entityStatus == (int)EntityState.Old_Nal_Old)
                {
                    CYZLog.writeLog("conntInsert start：", "Old_Nal_Old");
                    conntUpdate.Add(v2);
                    v2.SetValue("EntityState", EntityState.Update);
                    var vv = new ElectronTransferServicePro.InsertTempTable();
                    vv.g3e_fid = v2.G3E_FID;
                    vv.g3e_fno = v2.G3E_FNO;
                    vv.v2 = v2;
                    vv.ltt_name = ltt_name;
                    vv.GcTicket_ID = gc_id;
                    vv.Sjversion = int.Parse(v2.GetValue("Version").ToString());
                    InsertTempTables.Add(vv);
                }
                else if ((int)_entityStatus == (int)EntityState.Old_Nal_Del)
                {
                    v2.NODE2_ID = 0;
                    CYZLog.writeLog("conntInsert start：", "Old_Nal_Del");
                    conntUpdate.Add(v2);
                    v2.SetValue("EntityState", EntityState.Update);
                    var vv = new ElectronTransferServicePro.InsertTempTable();
                    vv.g3e_fid = v2.G3E_FID;
                    vv.g3e_fno = v2.G3E_FNO;
                    vv.v2 = v2;
                    vv.ltt_name = ltt_name;
                    vv.GcTicket_ID = gc_id;
                    vv.Sjversion = int.Parse(v2.GetValue("Version").ToString());
                    InsertTempTables.Add(vv);
                }
                else if ((int)_entityStatus == (int)EntityState.Old_Nal_Add)
                {
                    Int64? OldNODE2_ID = v2.NODE2_ID;


                    if (DicLong.ContainsKey(OldNODE2_ID))
                    {

                        v2.NODE2_ID = DicLong[OldNODE2_ID];
                    }
                    else
                    {
                        v2.NODE2_ID = OracleSequenceValueGenerator.Instance.GenerateNodeId();
                        if (!DicLong.ContainsKey(OldNODE2_ID))
                        {
                            DicLong.Add(OldNODE2_ID, v2.NODE2_ID);
                        }
                    }
                    CYZLog.writeLog("conntInsert start：", "Old_Nal_Add");
                    conntUpdate.Add(v2);
                    v2.SetValue("EntityState", EntityState.Update);
                    var vv = new ElectronTransferServicePro.InsertTempTable();
                    vv.g3e_fid = v2.G3E_FID;
                    vv.g3e_fno = v2.G3E_FNO;
                    vv.v2 = v2;
                    vv.ltt_name = ltt_name;
                    vv.GcTicket_ID = gc_id;
                    vv.Sjversion = int.Parse(v2.GetValue("Version").ToString());
                    InsertTempTables.Add(vv);
                }


                else if ((int)_entityStatus == (int)EntityState.Old_Nal_Nal)
                {
                    CYZLog.writeLog("conntInsert start：", "Old_Nal_Nal");
                    conntUpdate.Add(v2);
                    v2.SetValue("EntityState", EntityState.Update);
                    var vv = new ElectronTransferServicePro.InsertTempTable();
                    vv.g3e_fid = v2.G3E_FID;
                    vv.g3e_fno = v2.G3E_FNO;
                    vv.v2 = v2;
                    vv.ltt_name = ltt_name;
                    vv.GcTicket_ID = gc_id;
                    vv.Sjversion = int.Parse(v2.GetValue("Version").ToString());
                    InsertTempTables.Add(vv);
                }
            }
            catch (Exception ex)
            {
                CYZLog.writeLog("connn旧的连接关系", ex.ToString());
                throw ex;
            }
        }

        /// <summary>
        /// 新增的数据的连接关系
        /// </summary>
        /// <param name="oracledb"></param>
        /// <param name="xmldb"></param>
        /// <param name="gc_id"></param>
        /// <param name="ltt_name"></param>
        /// <param name="appenshiptime"></param>
        public static void NewDateCONNECTIVITY_N(Connectivity_n v2, string gc_id, string ltt_name, DateTime appenshiptime, string qj, EntityState _entityStatus, ref Dictionary<long?, long?> DicLong, ref  List<ElectronBase> conntUpdate, ref  List<ElectronBase> conntInsert, ref  List<ElectronBase> conntDelete, ref List<ElectronTransferServicePro.InsertTempTable> InsertTempTables)
        {

            try
            {

                if ((int)_entityStatus == (int)EntityState.Add_Add_Add)
                {
                    long NewFid = QueryTemp(v2.G3E_FID, gc_id);
                    long OldFid = v2.G3E_FID;
                    CYZLog.writeLog("conntInsert start：", "Add_Add_Add2");
                    if (NewFid != 0)
                    {
                        v2.G3E_ID = OracleSequenceValueGenerator.Instance.GenerateTableId(v2.GetType());
                        v2.G3E_FID = NewFid;
                        CYZLog.writeLog("conntInsert start：", "Add_Add_Add3");
                        Int64? OldNODE1_ID = v2.NODE1_ID;
                        Int64? OldNODE2_ID = v2.NODE2_ID;
                        long? outOldNODE1_ID = 0;

                        if (DicLong.ContainsKey(OldNODE1_ID))
                        {

                            v2.NODE1_ID = DicLong[OldNODE1_ID];
                        }
                        else
                        {
                            v2.NODE1_ID = OracleSequenceValueGenerator.Instance.GenerateNodeId();
                            if (!DicLong.ContainsKey(OldNODE1_ID))
                            {
                                DicLong.Add(OldNODE1_ID, v2.NODE1_ID);
                            }
                        }

                        if (DicLong.ContainsKey(OldNODE2_ID))
                        {

                            v2.NODE2_ID = DicLong[OldNODE2_ID];
                        }
                        else
                        {
                            v2.NODE2_ID = OracleSequenceValueGenerator.Instance.GenerateNodeId();
                            if (!DicLong.ContainsKey(OldNODE2_ID))
                            {
                                DicLong.Add(OldNODE2_ID, v2.NODE2_ID);
                            }
                        }

                        conntInsert.Add(v2);

                        CYZLog.writeLog("conntInsert start：", "Add_Add_Add");
                        v2.SetValue("EntityState", EntityState.Insert);
                        var vv = new ElectronTransferServicePro.InsertTempTable();
                        vv.g3e_fid = v2.G3E_FID;
                        vv.g3e_fno = v2.G3E_FNO;
                        vv.v2 = v2;
                        vv.ltt_name = ltt_name;
                        vv.GcTicket_ID = gc_id;
                        vv.Sjversion = int.Parse(v2.GetValue("Version").ToString());
                        InsertTempTables.Add(vv);
                    }

                }
                else if ((int)_entityStatus == (int)EntityState.Add_Add_Del)
                {
                    long NewFid = QueryTemp(v2.G3E_FID, gc_id);
                    long OldFid = v2.G3E_FID;
                    CYZLog.writeLog("conntInsert start：", "Add_Add_Del2");
                    if (NewFid != 0)
                    {
                        v2.G3E_ID = OracleSequenceValueGenerator.Instance.GenerateTableId(v2.GetType());
                        v2.G3E_FID = NewFid;
                        CYZLog.writeLog("conntInsert start：", "Add_Add_Del3");

                        Int64? OldNODE1_ID = v2.NODE1_ID;


                        if (DicLong.ContainsKey(OldNODE1_ID))
                        {

                            v2.NODE1_ID = DicLong[OldNODE1_ID];

                        }
                        else
                        {
                            v2.NODE1_ID = OracleSequenceValueGenerator.Instance.GenerateNodeId();
                            if (!DicLong.ContainsKey(OldNODE1_ID))
                            {
                                DicLong.Add(OldNODE1_ID, v2.NODE1_ID);
                            }
                        }

                        v2.NODE2_ID = 0;

                        conntInsert.Add(v2);
                        CYZLog.writeLog("conntInsert start：", "Add_Add_Del");
                        v2.SetValue("EntityState", EntityState.Insert);
                        var vv = new ElectronTransferServicePro.InsertTempTable();
                        vv.g3e_fid = v2.G3E_FID;
                        vv.g3e_fno = v2.G3E_FNO;
                        vv.v2 = v2;
                        vv.ltt_name = ltt_name;
                        vv.GcTicket_ID = gc_id;
                        vv.Sjversion = int.Parse(v2.GetValue("Version").ToString());
                        InsertTempTables.Add(vv);
                    }
                }
                else if ((int)_entityStatus == (int)EntityState.Add_Add_Nal)
                {
                    long NewFid = QueryTemp(v2.G3E_FID, gc_id);
                    long OldFid = v2.G3E_FID;
                    long? outOldNODE1_ID = 0;
                    CYZLog.writeLog("conntInsert start：", "Add_Add_Nal2");
                    if (NewFid != 0)
                    {
                        v2.G3E_ID = OracleSequenceValueGenerator.Instance.GenerateTableId(v2.GetType());
                        v2.G3E_FID = NewFid;
                        CYZLog.writeLog("conntInsert start：", "Add_Add_Nal3");
                        Int64? OldNODE1_ID = v2.NODE1_ID;

                        if (DicLong.ContainsKey(OldNODE1_ID))
                        {
                            // DicLong.TryGetValue(OldNODE1_ID, out outOldNODE1_ID);
                            v2.NODE1_ID = DicLong[OldNODE1_ID];
                        }
                        else
                        {
                            v2.NODE1_ID = OracleSequenceValueGenerator.Instance.GenerateNodeId();
                            if (!DicLong.ContainsKey(OldNODE1_ID))
                            {
                                DicLong.Add(OldNODE1_ID, v2.NODE1_ID);
                            }
                        }
                      
                        conntInsert.Add(v2);
                        CYZLog.writeLog("conntInsert start：", "Add_Add_Nal");
                        v2.SetValue("EntityState", EntityState.Insert);
                        var vv = new ElectronTransferServicePro.InsertTempTable();
                        vv.g3e_fid = v2.G3E_FID;
                        vv.g3e_fno = v2.G3E_FNO;
                        vv.v2 = v2;
                        vv.ltt_name = ltt_name;
                        vv.GcTicket_ID = gc_id;
                        vv.Sjversion = int.Parse(v2.GetValue("Version").ToString());
                        InsertTempTables.Add(vv);
                    }
                }
                else if ((int)_entityStatus == (int)EntityState.Add_Add_Old)
                {
                    long NewFid = QueryTemp(v2.G3E_FID, gc_id);
                    long OldFid = v2.G3E_FID;
                    long? outOldNODE1_ID = 0;
                    CYZLog.writeLog("conntInsert start：", "Add_Add_Old2");
                    if (NewFid != 0)
                    {
                        v2.G3E_ID = OracleSequenceValueGenerator.Instance.GenerateTableId(v2.GetType());
                        v2.G3E_FID = NewFid;
                        CYZLog.writeLog("conntInsert start：", "Add_Add_Old3");


                        Int64? OldNODE1_ID = v2.NODE1_ID;


                        if (DicLong.ContainsKey(OldNODE1_ID))
                        {

                            v2.NODE1_ID = DicLong[OldNODE1_ID];
                        }
                        else
                        {
                            v2.NODE1_ID = OracleSequenceValueGenerator.Instance.GenerateNodeId();
                            if (!DicLong.ContainsKey(OldNODE1_ID))
                            {
                                DicLong.Add(OldNODE1_ID, v2.NODE1_ID);
                            }
                        }
                        
                        conntInsert.Add(v2);
                        CYZLog.writeLog("conntInsert start：", "Add_Add_Old");
                        v2.SetValue("EntityState", EntityState.Insert);
                        var vv = new ElectronTransferServicePro.InsertTempTable();
                        vv.g3e_fid = v2.G3E_FID;
                        vv.g3e_fno = v2.G3E_FNO;
                        vv.v2 = v2;
                        vv.ltt_name = ltt_name;
                        vv.GcTicket_ID = gc_id;
                        vv.Sjversion = int.Parse(v2.GetValue("Version").ToString());
                        InsertTempTables.Add(vv);
                    }
                }

                else if ((int)_entityStatus == (int)EntityState.Add_Del_Add)
                {
                    long NewFid = QueryTemp(v2.G3E_FID, gc_id);
                    long OldFid = v2.G3E_FID;
                    CYZLog.writeLog("conntInsert start：", "Add_Del_Del2");
                    if (NewFid != 0)
                    {
                        v2.G3E_ID = OracleSequenceValueGenerator.Instance.GenerateTableId(v2.GetType());
                        v2.G3E_FID = NewFid;

                        CYZLog.writeLog("conntInsert start：", "Add_Del_Del3");
                        Int64? OldNODE2_ID = v2.NODE2_ID;
                        long? outOldNODE1_ID = 0;


                        if (DicLong.ContainsKey(OldNODE2_ID))
                        {
                            //DicLong.TryGetValue(OldNODE2_ID, out outOldNODE1_ID);
                            v2.NODE2_ID = DicLong[OldNODE2_ID];
                        }
                        else
                        {
                            v2.NODE2_ID = OracleSequenceValueGenerator.Instance.GenerateNodeId();
                            if (!DicLong.ContainsKey(OldNODE2_ID))
                            {
                                DicLong.Add(OldNODE2_ID, v2.NODE2_ID);
                            }
                        }
                        v2.NODE1_ID = 0;
                        
                        conntInsert.Add(v2);
                        CYZLog.writeLog("conntInsert start：", "Add_Del_Add");
                        v2.SetValue("EntityState", EntityState.Insert);
                        var vv = new ElectronTransferServicePro.InsertTempTable();
                        vv.g3e_fid = v2.G3E_FID;
                        vv.g3e_fno = v2.G3E_FNO;
                        vv.v2 = v2;
                        vv.ltt_name = ltt_name;
                        vv.GcTicket_ID = gc_id;
                        vv.Sjversion = int.Parse(v2.GetValue("Version").ToString());
                        InsertTempTables.Add(vv);

                    }
                }
                else if ((int)_entityStatus == (int)EntityState.Add_Del_Del)
                {
                    long NewFid = QueryTemp(v2.G3E_FID, gc_id);
                    long OldFid = v2.G3E_FID;
                    CYZLog.writeLog("conntInsert start：", "Add_Del_Del2");
                    if (NewFid != 0)
                    {
                        v2.G3E_ID = OracleSequenceValueGenerator.Instance.GenerateTableId(v2.GetType());
                        v2.G3E_FID = NewFid;
                        CYZLog.writeLog("conntInsert start：", "Add_Del_Del3");
                    }

                    v2.NODE1_ID = 0;
                    v2.NODE2_ID = 0;
                
                    conntInsert.Add(v2);
                    CYZLog.writeLog("conntInsert start：", "Add_Del_Del");
                    v2.SetValue("EntityState", EntityState.Insert);
                    var vv = new ElectronTransferServicePro.InsertTempTable();
                    vv.g3e_fid = v2.G3E_FID;
                    vv.g3e_fno = v2.G3E_FNO;
                    vv.v2 = v2;
                    vv.ltt_name = ltt_name;
                    vv.GcTicket_ID = gc_id;
                    vv.Sjversion = int.Parse(v2.GetValue("Version").ToString());
                    InsertTempTables.Add(vv);
                }
                else if ((int)_entityStatus == (int)EntityState.Add_Del_Nal)
                {
                    long NewFid = QueryTemp(v2.G3E_FID, gc_id);
                    long OldFid = v2.G3E_FID;
                    CYZLog.writeLog("conntInsert start：", "Add_Del_Nal2");
                    if (NewFid != 0)
                    {
                        v2.G3E_ID = OracleSequenceValueGenerator.Instance.GenerateTableId(v2.GetType());
                        v2.G3E_FID = NewFid;
                    }

                    v2.NODE1_ID = 0;
           
                    conntInsert.Add(v2);
                    CYZLog.writeLog("conntInsert start：", "Add_Del_Nal");
                    v2.SetValue("EntityState", EntityState.Insert);
                    var vv = new ElectronTransferServicePro.InsertTempTable();
                    vv.g3e_fid = v2.G3E_FID;
                    vv.g3e_fno = v2.G3E_FNO;
                    vv.v2 = v2;
                    vv.ltt_name = ltt_name;
                    vv.GcTicket_ID = gc_id;
                    vv.Sjversion = int.Parse(v2.GetValue("Version").ToString());
                    InsertTempTables.Add(vv);
                }

                else if ((int)_entityStatus == (int)EntityState.Add_Del_Old)
                {
                    long NewFid = QueryTemp(v2.G3E_FID, gc_id);
                    long OldFid = v2.G3E_FID;
                    CYZLog.writeLog("conntInsert start：", "Add_Del_Old2");
                    if (NewFid != 0)
                    {
                        v2.G3E_ID = OracleSequenceValueGenerator.Instance.GenerateTableId(v2.GetType());
                        v2.G3E_FID = NewFid;
                    }
                    v2.NODE1_ID = 0;
                  
                    conntInsert.Add(v2);
                    CYZLog.writeLog("conntInsert start：", "Add_Del_Old");
                    v2.SetValue("EntityState", EntityState.Insert);
                    var vv = new ElectronTransferServicePro.InsertTempTable();
                    vv.g3e_fid = v2.G3E_FID;
                    vv.g3e_fno = v2.G3E_FNO;
                    vv.v2 = v2;
                    vv.ltt_name = ltt_name;
                    vv.GcTicket_ID = gc_id;
                    vv.Sjversion = int.Parse(v2.GetValue("Version").ToString());
                    InsertTempTables.Add(vv);

                }
                else if ((int)_entityStatus == (int)EntityState.Add_Nal_Add)
                {
                    long NewFid = QueryTemp(v2.G3E_FID, gc_id);
                    long OldFid = v2.G3E_FID;
                    CYZLog.writeLog("conntInsert start：", "Add_Nal_Add2");
                    if (NewFid != 0)
                    {
                        v2.G3E_ID = OracleSequenceValueGenerator.Instance.GenerateTableId(v2.GetType());
                        v2.G3E_FID = NewFid;


                        Int64? OldNODE2_ID = v2.NODE2_ID;
                        long? outOldNODE1_ID = 0;

                        if (DicLong.ContainsKey(OldNODE2_ID))
                        {
                            //  DicLong.TryGetValue(OldNODE2_ID, out outOldNODE1_ID);
                            v2.NODE2_ID = DicLong[OldNODE2_ID];
                        }
                        else
                        {
                            v2.NODE2_ID = OracleSequenceValueGenerator.Instance.GenerateNodeId();
                            if (!DicLong.ContainsKey(OldNODE2_ID))
                            {
                                DicLong.Add(OldNODE2_ID, v2.NODE2_ID);
                            }
                        }
                       
                        conntInsert.Add(v2);
                        CYZLog.writeLog("conntInsert start：", "Add_Nal_Add");
                        v2.SetValue("EntityState", EntityState.Insert);
                        var vv = new ElectronTransferServicePro.InsertTempTable();
                        vv.g3e_fid = v2.G3E_FID;
                        vv.g3e_fno = v2.G3E_FNO;
                        vv.v2 = v2;
                        vv.ltt_name = ltt_name;
                        vv.GcTicket_ID = gc_id;
                        vv.Sjversion = int.Parse(v2.GetValue("Version").ToString());
                        InsertTempTables.Add(vv);

                    }
                }
                else if ((int)_entityStatus == (int)EntityState.Add_Nal_Del)
                {
                    long NewFid = QueryTemp(v2.G3E_FID, gc_id);
                    long OldFid = v2.G3E_FID;
                    CYZLog.writeLog("conntInsert start：", "Add_Nal_Del2");
                    if (NewFid != 0)
                    {
                        v2.G3E_ID = OracleSequenceValueGenerator.Instance.GenerateTableId(v2.GetType());
                        v2.G3E_FID = NewFid;
                    }
                    v2.NODE2_ID = 0;
                    CYZLog.writeLog("conntInsert start：", "Add_Nal_Del");
                    conntInsert.Add(v2);
                    v2.SetValue("EntityState", EntityState.Insert);
                    var vv = new ElectronTransferServicePro.InsertTempTable();
                    vv.g3e_fid = v2.G3E_FID;
                    vv.g3e_fno = v2.G3E_FNO;
                    vv.v2 = v2;
                    vv.ltt_name = ltt_name;
                    vv.GcTicket_ID = gc_id;
                    vv.Sjversion = int.Parse(v2.GetValue("Version").ToString());
                    InsertTempTables.Add(vv);
                }
                else if ((int)_entityStatus == (int)EntityState.Add_Nal_Nal)
                {
                    long NewFid = QueryTemp(v2.G3E_FID, gc_id);
                    long OldFid = v2.G3E_FID;
                    if (NewFid != 0)
                    {
                        v2.G3E_ID = OracleSequenceValueGenerator.Instance.GenerateTableId(v2.GetType());
                        v2.G3E_FID = NewFid;
                    }
                   
                    conntInsert.Add(v2);
                    CYZLog.writeLog("conntInsert start：", "Add_Nal_Nal");
                    v2.SetValue("EntityState", EntityState.Insert);
                    var vv = new ElectronTransferServicePro.InsertTempTable();
                    vv.g3e_fid = v2.G3E_FID;
                    vv.g3e_fno = v2.G3E_FNO;
                    vv.v2 = v2;
                    vv.ltt_name = ltt_name;
                    vv.GcTicket_ID = gc_id;
                    vv.Sjversion = int.Parse(v2.GetValue("Version").ToString());
                    InsertTempTables.Add(vv);
                }
                else if ((int)_entityStatus == (int)EntityState.Add_Nal_Old)
                {
                    long NewFid = QueryTemp(v2.G3E_FID, gc_id);
                    long OldFid = v2.G3E_FID;
                    CYZLog.writeLog("conntInsert start：", "Add_Nal_Old2");
                    if (NewFid != 0)
                    {
                        v2.G3E_ID = OracleSequenceValueGenerator.Instance.GenerateTableId(v2.GetType());
                        v2.G3E_FID = NewFid;
                    }
             
                    conntInsert.Add(v2);
                    CYZLog.writeLog("conntInsert start：", "Add_Nal_Old");
                    v2.SetValue("EntityState", EntityState.Insert);
                    var vv = new ElectronTransferServicePro.InsertTempTable();
                    vv.g3e_fid = v2.G3E_FID;
                    vv.g3e_fno = v2.G3E_FNO;
                    vv.v2 = v2;
                    vv.ltt_name = ltt_name;
                    vv.GcTicket_ID = gc_id;
                    vv.Sjversion = int.Parse(v2.GetValue("Version").ToString());
                    InsertTempTables.Add(vv);
                }
                else if ((int)_entityStatus == (int)EntityState.Add_Old_Add)
                {
                    long NewFid = QueryTemp(v2.G3E_FID, gc_id);
                    long OldFid = v2.G3E_FID;
                    CYZLog.writeLog("conntInsert start：", "Add_Old_Add2");
                    if (NewFid != 0)
                    {
                        v2.G3E_ID = OracleSequenceValueGenerator.Instance.GenerateTableId(v2.GetType());
                        v2.G3E_FID = NewFid;


                        Int64? OldNODE2_ID = v2.NODE2_ID;
                        long? outOldNODE1_ID = 0;

                        if (DicLong.ContainsKey(OldNODE2_ID))
                        {
                            //DicLong.TryGetValue(OldNODE2_ID, out outOldNODE1_ID);
                            v2.NODE2_ID = DicLong[OldNODE2_ID];
                        }
                        else
                        {
                            v2.NODE2_ID = OracleSequenceValueGenerator.Instance.GenerateNodeId();
                            if (!DicLong.ContainsKey(OldNODE2_ID))
                            {
                                DicLong.Add(OldNODE2_ID, v2.NODE2_ID);
                            }
                        }
                     
                        conntInsert.Add(v2);
                        CYZLog.writeLog("conntInsert start：", "Add_Old_Add");
                        v2.SetValue("EntityState", EntityState.Insert);
                        var vv = new ElectronTransferServicePro.InsertTempTable();
                        vv.g3e_fid = v2.G3E_FID;
                        vv.g3e_fno = v2.G3E_FNO;
                        vv.v2 = v2;
                        vv.ltt_name = ltt_name;
                        vv.GcTicket_ID = gc_id;
                        vv.Sjversion = int.Parse(v2.GetValue("Version").ToString());
                        InsertTempTables.Add(vv);
                    }
                }
                else if ((int)_entityStatus == (int)EntityState.Add_Old_Del)
                {
                    long NewFid = QueryTemp(v2.G3E_FID, gc_id);
                    long OldFid = v2.G3E_FID;
                    CYZLog.writeLog("conntInsert start：", "Add_Old_Del2");
                    if (NewFid != 0)
                    {
                        v2.G3E_ID = OracleSequenceValueGenerator.Instance.GenerateTableId(v2.GetType());
                        v2.G3E_FID = NewFid;
                    }
                    v2.NODE2_ID = 0;
                  
                    conntInsert.Add(v2);
                    CYZLog.writeLog("conntInsert start：", "Add_Old_Del");
                    v2.SetValue("EntityState", EntityState.Insert);
                    var vv = new ElectronTransferServicePro.InsertTempTable();
                    vv.g3e_fid = v2.G3E_FID;
                    vv.g3e_fno = v2.G3E_FNO;
                    vv.v2 = v2;
                    vv.ltt_name = ltt_name;
                    vv.GcTicket_ID = gc_id;
                    vv.Sjversion = int.Parse(v2.GetValue("Version").ToString());
                    InsertTempTables.Add(vv);
                }
                else if ((int)_entityStatus == (int)EntityState.Add_Old_Nal)
                {
                    long NewFid = QueryTemp(v2.G3E_FID, gc_id);
                    long OldFid = v2.G3E_FID;
                    CYZLog.writeLog("conntInsert start：", "Add_Old_Nal2");
                    if (NewFid != 0)
                    {
                        v2.G3E_ID = OracleSequenceValueGenerator.Instance.GenerateTableId(v2.GetType());
                        v2.G3E_FID = NewFid;
                    }

                    conntInsert.Add(v2);
                    CYZLog.writeLog("conntInsert start：", "Add_Old_Nal");
                    v2.SetValue("EntityState", EntityState.Insert);
                    var vv = new ElectronTransferServicePro.InsertTempTable();
                    vv.g3e_fid = v2.G3E_FID;
                    vv.g3e_fno = v2.G3E_FNO;
                    vv.v2 = v2;
                    vv.ltt_name = ltt_name;
                    vv.GcTicket_ID = gc_id;
                    vv.Sjversion = int.Parse(v2.GetValue("Version").ToString());
                    InsertTempTables.Add(vv);
                }

                else if ((int)_entityStatus == (int)EntityState.Add_Old_Old)
                {
                    long NewFid = QueryTemp(v2.G3E_FID, gc_id);
                    long OldFid = v2.G3E_FID;
                    CYZLog.writeLog("conntInsert start：", "Add_Old_Old2");
                    if (NewFid != 0)
                    {
                        v2.G3E_ID = OracleSequenceValueGenerator.Instance.GenerateTableId(v2.GetType());
                        v2.G3E_FID = NewFid;
                    }
                    conntInsert.Add(v2);
                    CYZLog.writeLog("conntInsert start：", "Add_Old_Old");
                    v2.SetValue("EntityState", EntityState.Insert);
                    var vv = new ElectronTransferServicePro.InsertTempTable();
                    vv.g3e_fid = v2.G3E_FID;
                    vv.g3e_fno = v2.G3E_FNO;
                    vv.v2 = v2;
                    vv.ltt_name = ltt_name;
                    vv.GcTicket_ID = gc_id;
                    vv.Sjversion = int.Parse(v2.GetValue("Version").ToString());
                    InsertTempTables.Add(vv);
                }
            }
            catch (NotExistException ex)
            {

            }
            catch (Exception ex)
            {
                CYZLog.writeLog("connn新的连接关系",ex.ToString());
                throw;
            }
        }


        /// <summary>
        /// Add_Old的所有选项
        /// </summary>
        /// <param name="oracledb"></param>
        /// <param name="xmldb"></param>
        /// <param name="gc_id"></param>
        /// <param name="ltt_name"></param>
        public static void NewDateConnAddOld(Connectivity_n v2, string gc_id, string ltt_name, DateTime appenshiptime, string qj, EntityState _entityStatus, ref Dictionary<long?, long?> DicLong, ref  List<Connectivity_n> conntUpdate, ref  List<Connectivity_n> conntInsert, ref  List<Connectivity_n> conntDelete, ref List<ElectronTransferServicePro.InsertTempTable> InsertTempTables)
        {
            try
            {
                if ((int)_entityStatus == (int)EntityState.Add_Old_Old)
                {   
                    long NewFid = QueryTemp(v2.G3E_FID, gc_id);
                    CYZLog.writeLog("conntInsert start：", "Add_Old_Old2");
                    long OldFid = v2.G3E_FID;
                    if (NewFid != 0)
                    {
                        CYZLog.writeLog("conntInsert start：", "Add_Old_Old3");
                        v2.G3E_ID = OracleSequenceValueGenerator.Instance.GenerateTableId(v2.GetType());
                        v2.G3E_FID = NewFid;
                    }
                  
                    conntInsert.Add(v2);
                    CYZLog.writeLog("conntInsert start：", "Add_Old_Old");
                    v2.SetValue("EntityState", EntityState.Insert);
                    var vv = new ElectronTransferServicePro.InsertTempTable();
                    vv.g3e_fid = v2.G3E_FID;
                    vv.g3e_fno = v2.G3E_FNO;
                    vv.v2 = v2;
                    vv.ltt_name = ltt_name;
                    vv.GcTicket_ID = gc_id;
                    vv.Sjversion = int.Parse(v2.GetValue("Version").ToString());
                    InsertTempTables.Add(vv);
                }
                else if ((int)_entityStatus == (int)EntityState.Add_Old_Del)
                {
                    long NewFid = QueryTemp(v2.G3E_FID, gc_id);
                    long OldFid = v2.G3E_FID;
                    CYZLog.writeLog("conntInsert start：", "Add_Old_Del2");
                    if (NewFid != 0)
                    {
                        CYZLog.writeLog("conntInsert start：", "Add_Old_Del3");
                        v2.G3E_ID = OracleSequenceValueGenerator.Instance.GenerateTableId(v2.GetType());
                        v2.G3E_FID = NewFid;
                    }
                    v2.NODE2_ID = 0;
            
                    conntInsert.Add(v2);
                    CYZLog.writeLog("conntInsert start：", "Add_Old_Del");
                    v2.SetValue("EntityState", EntityState.Insert);
                    var vv = new ElectronTransferServicePro.InsertTempTable();
                    vv.g3e_fid = v2.G3E_FID;
                    vv.g3e_fno = v2.G3E_FNO;
                    vv.v2 = v2;
                    vv.ltt_name = ltt_name;
                    vv.GcTicket_ID = gc_id;
                    vv.Sjversion = int.Parse(v2.GetValue("Version").ToString());
                    InsertTempTables.Add(vv);
                }
                else if ((int)_entityStatus == (int)EntityState.Add_Old_Add)
                {
                    long NewFid = QueryTemp(v2.G3E_FID, gc_id);
                    long OldFid = v2.G3E_FID;
                    CYZLog.writeLog("conntInsert start：", "Add_Old_Add3");
                    if (NewFid != 0)
                    {
                        CYZLog.writeLog("conntInsert start：", "Add_Old_Add4");
                        v2.G3E_ID = OracleSequenceValueGenerator.Instance.GenerateTableId(v2.GetType());
                        v2.G3E_FID = NewFid;
                        CYZLog.writeLog("conntInsert start：", "Add_Old_Add2");

                        Int64? OldNODE2_ID = v2.NODE2_ID;
                        long? outOldNODE1_ID = 0;

                        if (DicLong.ContainsKey(OldNODE2_ID))
                        {
                            //DicLong.TryGetValue(OldNODE2_ID, out outOldNODE1_ID);
                            v2.NODE2_ID = DicLong[OldNODE2_ID];
                        }
                        else
                        {
                            v2.NODE2_ID = OracleSequenceValueGenerator.Instance.GenerateNodeId();
                            if (!DicLong.ContainsKey(OldNODE2_ID))
                            {
                                DicLong.Add(OldNODE2_ID, v2.NODE2_ID);
                            }
                        }
                       
                        conntInsert.Add(v2);
                        CYZLog.writeLog("conntInsert start：", "Add_Old_Add");
                        v2.SetValue("EntityState", EntityState.Insert);
                        var vv = new ElectronTransferServicePro.InsertTempTable();
                        vv.g3e_fid = v2.G3E_FID;
                        vv.g3e_fno = v2.G3E_FNO;
                        vv.v2 = v2;
                        vv.ltt_name = ltt_name;
                        vv.GcTicket_ID = gc_id;
                        vv.Sjversion = int.Parse(v2.GetValue("Version").ToString());
                        InsertTempTables.Add(vv);
                    }
                }
                else if ((int)_entityStatus == (int)EntityState.Add_Old_Nal)
                {
                    long NewFid = QueryTemp(v2.G3E_FID, gc_id);
                    long OldFid = v2.G3E_FID;
                    CYZLog.writeLog("conntInsert start：", "Add_Old_Nal2");
                    if (NewFid != 0)
                    {
                        CYZLog.writeLog("conntInsert start：", "Add_Old_Nal3");
                        v2.G3E_ID = OracleSequenceValueGenerator.Instance.GenerateTableId(v2.GetType());
                        v2.G3E_FID = NewFid;
                    }
                   
                    conntInsert.Add(v2);
                    CYZLog.writeLog("conntInsert start：", "Add_Old_Nal");
                    v2.SetValue("EntityState", EntityState.Insert);
                    var vv = new ElectronTransferServicePro.InsertTempTable();
                    vv.g3e_fid = v2.G3E_FID;
                    vv.g3e_fno = v2.G3E_FNO;
                    vv.v2 = v2;
                    vv.ltt_name = ltt_name;
                    vv.GcTicket_ID = gc_id;
                    vv.Sjversion = int.Parse(v2.GetValue("Version").ToString());
                    InsertTempTables.Add(vv);

                }
            }
            catch (Exception ex)
            {
                CYZLog.writeLog("connn新的连接关系", ex.ToString());
                throw ex;
            }
        }

        /// <summary>
        ///  Add_Del的所有选项
        /// </summary>
        /// <param name="oracledb"></param>
        /// <param name="xmldb"></param>
        /// <param name="gc_id"></param>
        /// <param name="ltt_name"></param>

        public static void NewDateConnAddDel(Connectivity_n v2, string gc_id, string ltt_name, DateTime appenshiptime, string qj, EntityState _entityStatus, ref Dictionary<long?, long?> DicLong, ref  List<Connectivity_n> conntUpdate, ref  List<Connectivity_n> conntInsert, ref  List<Connectivity_n> conntDelete, ref List<ElectronTransferServicePro.InsertTempTable> InsertTempTables)
        {
            try
            {
                if ((int)_entityStatus == (int)EntityState.Add_Del_Old)
                {
                  
                    long NewFid = QueryTemp(v2.G3E_FID, gc_id);
                    long OldFid = v2.G3E_FID;
                    CYZLog.writeLog("conntInsert start：", "Add_Del_Old2");
                    if (NewFid != 0)
                    {
                        CYZLog.writeLog("conntInsert start：", "Add_Del_Old3");
                        v2.G3E_ID = OracleSequenceValueGenerator.Instance.GenerateTableId(v2.GetType());
                        v2.G3E_FID = NewFid;
                    }
                    v2.NODE1_ID = 0;
              
                    conntInsert.Add(v2);
                    CYZLog.writeLog("conntInsert start：", "Add_Del_Old");
                    v2.SetValue("EntityState", EntityState.Insert);
                    var vv = new ElectronTransferServicePro.InsertTempTable();
                    vv.g3e_fid = v2.G3E_FID;
                    vv.g3e_fno = v2.G3E_FNO;
                    vv.v2 = v2;
                    vv.ltt_name = ltt_name;
                    vv.GcTicket_ID = gc_id;
                    vv.Sjversion = int.Parse(v2.GetValue("Version").ToString());
                    InsertTempTables.Add(vv);

                }
                else if ((int)_entityStatus == (int)EntityState.Add_Del_Del)
                {
                    long NewFid = QueryTemp(v2.G3E_FID, gc_id);
                    long OldFid = v2.G3E_FID;
                    CYZLog.writeLog("conntInsert start：", "Add_Del_Del4");
                    if (NewFid != 0)
                    {
                        CYZLog.writeLog("conntInsert start：", "Add_Del_Del3");
                        v2.G3E_ID = OracleSequenceValueGenerator.Instance.GenerateTableId(v2.GetType());
                        v2.G3E_FID = NewFid;
                    }

                    v2.NODE1_ID = 0;
                    v2.NODE2_ID = 0;
                   
                    conntInsert.Add(v2);
                    CYZLog.writeLog("conntInsert start：", "Add_Del_Del");
                    v2.SetValue("EntityState", EntityState.Insert);
                    var vv = new ElectronTransferServicePro.InsertTempTable();
                    vv.g3e_fid = v2.G3E_FID;
                    vv.g3e_fno = v2.G3E_FNO;
                    vv.v2 = v2;
                    vv.ltt_name = ltt_name;
                    vv.GcTicket_ID = gc_id;
                    vv.Sjversion = int.Parse(v2.GetValue("Version").ToString());
                    InsertTempTables.Add(vv);
                }
                else if ((int)_entityStatus == (int)EntityState.Add_Del_Add)
                {
                    long NewFid = QueryTemp(v2.G3E_FID, gc_id);
                    long OldFid = v2.G3E_FID;
                    CYZLog.writeLog("conntInsert start：", "Add_Del_Add2");
                    if (NewFid != 0)
                    {
                        CYZLog.writeLog("conntInsert start：", "Add_Del_Old");
                        v2.G3E_ID = OracleSequenceValueGenerator.Instance.GenerateTableId(v2.GetType());
                        v2.G3E_FID = NewFid;

                        CYZLog.writeLog("conntInsert start：", "Add_Del_Add3");
                        Int64? OldNODE2_ID = v2.NODE2_ID;
                        long? outOldNODE1_ID = 0;


                        if (DicLong.ContainsKey(OldNODE2_ID))
                        {
                            //DicLong.TryGetValue(OldNODE2_ID, out outOldNODE1_ID);
                            v2.NODE2_ID = DicLong[OldNODE2_ID];
                        }
                        else
                        {
                            v2.NODE2_ID = OracleSequenceValueGenerator.Instance.GenerateNodeId();
                            if (!DicLong.ContainsKey(OldNODE2_ID))
                            {
                                DicLong.Add(OldNODE2_ID, v2.NODE2_ID);
                            }
                        }
                        v2.NODE1_ID = 0;
                
                        conntInsert.Add(v2);
                        CYZLog.writeLog("conntInsert start：", "Add_Del_Add");
                        v2.SetValue("EntityState", EntityState.Insert);
                        var vv = new ElectronTransferServicePro.InsertTempTable();
                        vv.g3e_fid = v2.G3E_FID;
                        vv.g3e_fno = v2.G3E_FNO;
                        vv.v2 = v2;
                        vv.ltt_name = ltt_name;
                        vv.GcTicket_ID = gc_id;
                        vv.Sjversion = int.Parse(v2.GetValue("Version").ToString());
                        InsertTempTables.Add(vv);

                    }
                }

                else if ((int)_entityStatus == (int)EntityState.Add_Del_Nal)
                {
                    long NewFid = QueryTemp(v2.G3E_FID, gc_id);
                    long OldFid = v2.G3E_FID;
                    CYZLog.writeLog("conntInsert start：", "Add_Del_Nal2");
                    if (NewFid != 0)
                    {
                        CYZLog.writeLog("conntInsert start：", "Add_Del_Nal3");
                        v2.G3E_ID = OracleSequenceValueGenerator.Instance.GenerateTableId(v2.GetType());
                        v2.G3E_FID = NewFid;
                    }

                    v2.NODE1_ID = 0;
                   
                    conntInsert.Add(v2);
                    CYZLog.writeLog("conntInsert start：", "Add_Del_Nal");
                    v2.SetValue("EntityState", EntityState.Insert);
                    var vv = new ElectronTransferServicePro.InsertTempTable();
                    vv.g3e_fid = v2.G3E_FID;
                    vv.g3e_fno = v2.G3E_FNO;
                    vv.v2 = v2;
                    vv.ltt_name = ltt_name;
                    vv.GcTicket_ID = gc_id;
                    vv.Sjversion = int.Parse(v2.GetValue("Version").ToString());
                    InsertTempTables.Add(vv);
                }

            }catch(Exception ex)
            {
                CYZLog.writeLog("connn新的连接关系", ex.ToString());
                throw ex;
            }
        }

        /// <summary>
        ///  Add_Add的所有选项
        /// </summary>
        /// <param name="oracledb"></param>
        /// <param name="xmldb"></param>
        /// <param name="gc_id"></param>
        /// <param name="ltt_name"></param>

        public static void NewDateConnAddADD(Connectivity_n v2, string gc_id, string ltt_name, DateTime appenshiptime, string qj, EntityState _entityStatus, ref Dictionary<long?, long?> DicLong, ref  List<Connectivity_n> conntUpdate, ref  List<Connectivity_n> conntInsert, ref  List<Connectivity_n> conntDelete, ref List<ElectronTransferServicePro.InsertTempTable> InsertTempTables)
        {
            try
            {
                if ((int)_entityStatus == (int)EntityState.Add_Add_Old)
                {
                    long NewFid = QueryTemp(v2.G3E_FID, gc_id);
                    long OldFid = v2.G3E_FID;
                    long? outOldNODE1_ID = 0;
                    CYZLog.writeLog("conntInsert start：", "Add_Add_Old2");
                    if (NewFid != 0)
                    {
                        CYZLog.writeLog("conntInsert start：", "Add_Add_Old3");
                        v2.G3E_ID = OracleSequenceValueGenerator.Instance.GenerateTableId(v2.GetType());
                        v2.G3E_FID = NewFid;


                        Int64? OldNODE1_ID = v2.NODE1_ID;


                        if (DicLong.ContainsKey(OldNODE1_ID))
                        {

                            v2.NODE1_ID = DicLong[OldNODE1_ID];
                        }
                        else
                        {
                            v2.NODE1_ID = OracleSequenceValueGenerator.Instance.GenerateNodeId();
                            if (!DicLong.ContainsKey(OldNODE1_ID))
                            {
                                DicLong.Add(OldNODE1_ID, v2.NODE1_ID);
                            }
                        }

                        conntInsert.Add(v2);
                        CYZLog.writeLog("conntInsert start：", "Add_Add_Old");
                        v2.SetValue("EntityState", EntityState.Insert);
                        var vv = new ElectronTransferServicePro.InsertTempTable();
                        vv.g3e_fid = v2.G3E_FID;
                        vv.g3e_fno = v2.G3E_FNO;
                        vv.v2 = v2;
                        vv.ltt_name = ltt_name;
                        vv.GcTicket_ID = gc_id;
                        vv.Sjversion = int.Parse(v2.GetValue("Version").ToString());
                        InsertTempTables.Add(vv);
                    }
                }
                else if ((int)_entityStatus == (int)EntityState.Add_Add_Del)
                {
                    long NewFid = QueryTemp(v2.G3E_FID, gc_id);
                    long OldFid = v2.G3E_FID;
                    CYZLog.writeLog("conntInsert start：", "Add_Add_Del2");
                    if (NewFid != 0)
                    {
                        CYZLog.writeLog("conntInsert start：", "Add_Add_Del3");
                        v2.G3E_ID = OracleSequenceValueGenerator.Instance.GenerateTableId(v2.GetType());
                        v2.G3E_FID = NewFid;


                        Int64? OldNODE1_ID = v2.NODE1_ID;


                        if (DicLong.ContainsKey(OldNODE1_ID))
                        {

                            v2.NODE1_ID = DicLong[OldNODE1_ID];

                        }
                        else
                        {
                            v2.NODE1_ID = OracleSequenceValueGenerator.Instance.GenerateNodeId();
                            if (!DicLong.ContainsKey(OldNODE1_ID))
                            {
                                DicLong.Add(OldNODE1_ID, v2.NODE1_ID);
                            }
                        }

                        v2.NODE2_ID = 0;

                        conntInsert.Add(v2);
                        CYZLog.writeLog("conntInsert start：", "Add_Add_Del");
                        v2.SetValue("EntityState", EntityState.Insert);
                        var vv = new ElectronTransferServicePro.InsertTempTable();
                        vv.g3e_fid = v2.G3E_FID;
                        vv.g3e_fno = v2.G3E_FNO;
                        vv.v2 = v2;
                        vv.ltt_name = ltt_name;
                        vv.GcTicket_ID = gc_id;
                        vv.Sjversion = int.Parse(v2.GetValue("Version").ToString());
                        InsertTempTables.Add(vv);
                    }
                }
                else if ((int)_entityStatus == (int)EntityState.Add_Add_Add)
                {
                    long NewFid = QueryTemp(v2.G3E_FID, gc_id);
                    long OldFid = v2.G3E_FID;
                    if (NewFid != 0)
                    {
                        CYZLog.writeLog("conntInsert start：", "Add_Add_Add3");
                        v2.G3E_ID = OracleSequenceValueGenerator.Instance.GenerateTableId(v2.GetType());
                        v2.G3E_FID = NewFid;
                        CYZLog.writeLog("conntInsert start：", "Add_Add_Add2");
                        Int64? OldNODE1_ID = v2.NODE1_ID;
                        Int64? OldNODE2_ID = v2.NODE2_ID;
                        long? outOldNODE1_ID = 0;

                        if (DicLong.ContainsKey(OldNODE1_ID))
                        {

                            v2.NODE1_ID = DicLong[OldNODE1_ID];
                        }
                        else
                        {
                            v2.NODE1_ID = OracleSequenceValueGenerator.Instance.GenerateNodeId();
                            if (!DicLong.ContainsKey(OldNODE1_ID))
                            {
                                DicLong.Add(OldNODE1_ID, v2.NODE1_ID);
                            }
                        }

                        if (DicLong.ContainsKey(OldNODE2_ID))
                        {

                            v2.NODE2_ID = DicLong[OldNODE2_ID];
                        }
                        else
                        {
                            v2.NODE2_ID = OracleSequenceValueGenerator.Instance.GenerateNodeId();
                            if (!DicLong.ContainsKey(OldNODE2_ID))
                            {
                                DicLong.Add(OldNODE2_ID, v2.NODE2_ID);
                            }
                        }

                        conntInsert.Add(v2);

                        CYZLog.writeLog("conntInsert start：", "Add_Add_Add");
                        v2.SetValue("EntityState", EntityState.Insert);
                        var vv = new ElectronTransferServicePro.InsertTempTable();
                        vv.g3e_fid = v2.G3E_FID;
                        vv.g3e_fno = v2.G3E_FNO;
                        vv.v2 = v2;
                        vv.ltt_name = ltt_name;
                        vv.GcTicket_ID = gc_id;
                        vv.Sjversion = int.Parse(v2.GetValue("Version").ToString());
                        InsertTempTables.Add(vv);
                    }

                }
                else if ((int)_entityStatus == (int)EntityState.Add_Add_Nal)
                {
                    long NewFid = QueryTemp(v2.G3E_FID, gc_id);
                    long OldFid = v2.G3E_FID;
                    long? outOldNODE1_ID = 0;

                    if (NewFid != 0)
                    {
                        CYZLog.writeLog("conntInsert start：", "Add_Add_Nal3");
                        v2.G3E_ID = OracleSequenceValueGenerator.Instance.GenerateTableId(v2.GetType());
                        v2.G3E_FID = NewFid;
                        CYZLog.writeLog("conntInsert start：", "Add_Add_Nal2");

                        Int64? OldNODE1_ID = v2.NODE1_ID;

                        if (DicLong.ContainsKey(OldNODE1_ID))
                        {
                            // DicLong.TryGetValue(OldNODE1_ID, out outOldNODE1_ID);
                            v2.NODE1_ID = DicLong[OldNODE1_ID];
                        }
                        else
                        {
                            v2.NODE1_ID = OracleSequenceValueGenerator.Instance.GenerateNodeId();
                            if (!DicLong.ContainsKey(OldNODE1_ID))
                            {
                                DicLong.Add(OldNODE1_ID, v2.NODE1_ID);
                            }
                        }
                  
                        conntInsert.Add(v2);
                        CYZLog.writeLog("conntInsert start：", "Add_Add_Nal");
                        v2.SetValue("EntityState", EntityState.Insert);
                        var vv = new ElectronTransferServicePro.InsertTempTable();
                        vv.g3e_fid = v2.G3E_FID;
                        vv.g3e_fno = v2.G3E_FNO;
                        vv.v2 = v2;
                        vv.ltt_name = ltt_name;
                        vv.GcTicket_ID = gc_id;
                        vv.Sjversion = int.Parse(v2.GetValue("Version").ToString());
                        InsertTempTables.Add(vv);
                    }
                }
            }
            catch (Exception ex)
            {
                CYZLog.writeLog("connn新的连接关系", ex.ToString());
                throw ex;
            }
        }

        /// <summary>
        ///  Add_Nal的所有选项
        /// </summary>
        /// <param name="oracledb"></param>
        /// <param name="xmldb"></param>
        /// <param name="gc_id"></param>
        /// <param name="ltt_name"></param>

        public static void NewDateConnAddNal(Connectivity_n v2, string gc_id, string ltt_name, DateTime appenshiptime, string qj, EntityState _entityStatus, ref Dictionary<long?, long?> DicLong, ref  List<Connectivity_n> conntUpdate, ref  List<Connectivity_n> conntInsert, ref  List<Connectivity_n> conntDelete, ref List<ElectronTransferServicePro.InsertTempTable> InsertTempTables)
        {
            try
            {
                if ((int)_entityStatus == (int)EntityState.Add_Nal_Old)
                {
                    long NewFid = QueryTemp(v2.G3E_FID, gc_id);
                    long OldFid = v2.G3E_FID;
                    if (NewFid != 0)
                    {
                        CYZLog.writeLog("conntInsert start：", "Add_Nal_Old3");
                        v2.G3E_ID = OracleSequenceValueGenerator.Instance.GenerateTableId(v2.GetType());
                        v2.G3E_FID = NewFid;
                    }
                   
                    conntInsert.Add(v2);
                    CYZLog.writeLog("conntInsert start：", "Add_Nal_Old2");
                    v2.SetValue("EntityState", EntityState.Insert);
                    var vv = new ElectronTransferServicePro.InsertTempTable();
                    vv.g3e_fid = v2.G3E_FID;
                    vv.g3e_fno = v2.G3E_FNO;
                    vv.v2 = v2;
                    vv.ltt_name = ltt_name;
                    vv.GcTicket_ID = gc_id;
                    vv.Sjversion = int.Parse(v2.GetValue("Version").ToString());
                    InsertTempTables.Add(vv);
                }
                else if ((int)_entityStatus == (int)EntityState.Add_Nal_Del)
                {
                    long NewFid = QueryTemp(v2.G3E_FID, gc_id);
                    long OldFid = v2.G3E_FID;
                    if (NewFid != 0)
                    {
                        CYZLog.writeLog("conntInsert start：", "Add_Nal_Del2");
                        v2.G3E_ID = OracleSequenceValueGenerator.Instance.GenerateTableId(v2.GetType());
                        v2.G3E_FID = NewFid;
                    }
                    v2.NODE2_ID = 0;
            
                    conntInsert.Add(v2);
                    CYZLog.writeLog("conntInsert start：", "Add_Nal_Del");
                    v2.SetValue("EntityState", EntityState.Insert);
                    var vv = new ElectronTransferServicePro.InsertTempTable();
                    vv.g3e_fid = v2.G3E_FID;
                    vv.g3e_fno = v2.G3E_FNO;
                    vv.v2 = v2;
                    vv.ltt_name = ltt_name;
                    vv.GcTicket_ID = gc_id;
                    vv.Sjversion = int.Parse(v2.GetValue("Version").ToString());
                    InsertTempTables.Add(vv);
                }
                else if ((int)_entityStatus == (int)EntityState.Add_Nal_Add)
                {
                    long NewFid = QueryTemp(v2.G3E_FID, gc_id);
                    long OldFid = v2.G3E_FID;
                    CYZLog.writeLog("conntInsert start：", "Add_Nal_Add");
                    if (NewFid != 0)
                    {
                        CYZLog.writeLog("conntInsert start：", "Add_Nal_Add3");
                        v2.G3E_ID = OracleSequenceValueGenerator.Instance.GenerateTableId(v2.GetType());
                        v2.G3E_FID = NewFid;
                        CYZLog.writeLog("conntInsert start：", "Add_Nal_Add2");


                        Int64? OldNODE2_ID = v2.NODE2_ID;
                        long? outOldNODE1_ID = 0;

                        if (DicLong.ContainsKey(OldNODE2_ID))
                        {
                            //  DicLong.TryGetValue(OldNODE2_ID, out outOldNODE1_ID);
                            v2.NODE2_ID = DicLong[OldNODE2_ID];
                        }
                        else
                        {
                            v2.NODE2_ID = OracleSequenceValueGenerator.Instance.GenerateNodeId();
                            if (!DicLong.ContainsKey(OldNODE2_ID))
                            {
                                DicLong.Add(OldNODE2_ID, v2.NODE2_ID);
                            }
                        }
                      
                        conntInsert.Add(v2);
                        CYZLog.writeLog("conntInsert start：", "Add_Nal_Add");
                        v2.SetValue("EntityState", EntityState.Insert);
                        var vv = new ElectronTransferServicePro.InsertTempTable();
                        vv.g3e_fid = v2.G3E_FID;
                        vv.g3e_fno = v2.G3E_FNO;
                        vv.v2 = v2;
                        vv.ltt_name = ltt_name;
                        vv.GcTicket_ID = gc_id;
                        vv.Sjversion = int.Parse(v2.GetValue("Version").ToString());
                        InsertTempTables.Add(vv);

                    }
                }
                else if ((int)_entityStatus == (int)EntityState.Add_Nal_Nal)
                {
                    long NewFid = QueryTemp(v2.G3E_FID, gc_id);
                    long OldFid = v2.G3E_FID;
                    if (NewFid != 0)
                    {
                        CYZLog.writeLog("conntInsert start：", "Add_Nal_Nal3");
                        v2.G3E_ID = OracleSequenceValueGenerator.Instance.GenerateTableId(v2.GetType());
                        v2.G3E_FID = NewFid;
                        CYZLog.writeLog("conntInsert start：", "Add_Nal_Nal2");
                    }
         
                    conntInsert.Add(v2);
                    CYZLog.writeLog("conntInsert start：", "Add_Nal_Nal");
                    v2.SetValue("EntityState", EntityState.Insert);
                    var vv = new ElectronTransferServicePro.InsertTempTable();
                    vv.g3e_fid = v2.G3E_FID;
                    vv.g3e_fno = v2.G3E_FNO;
                    vv.v2 = v2;
                    vv.ltt_name = ltt_name;
                    vv.GcTicket_ID = gc_id;
                    vv.Sjversion = int.Parse(v2.GetValue("Version").ToString());
                    InsertTempTables.Add(vv);
                }

            }
            catch (Exception ex)
            {
                CYZLog.writeLog("connn新的连接关系", ex.ToString());
                throw ex;
            }
        }

        /// <summary>
        /// 连接关系2这是第一次导入的时候使用
        /// </summary>
        /// <param name="oracledb"></param>
        /// <param name="xmldb"></param>
        /// <param name="gc_id"></param>
        /// <param name="ltt_name"></param>
        /// <param name="appenshiptime"></param>
        public static void CONNECTIVITY_N2(OracleDBManager oracledb, XmlDBManager xmldb, string gc_id, string ltt_name, DateTime appenshiptime, string qj)
        {
            Dictionary<long?, long?> DicLong = new Dictionary<long?, long?>();
            try
            {
                OracleSequenceValueGenerator.Instance.DbManager = oracledb;
                List<ElectronTransferServicePro.InsertTempTable> InsertTempTables = new List<ElectronTransferServicePro.InsertTempTable>();
                List<Connectivity_n> conntUpdate = new List<Connectivity_n>();
                List<Connectivity_n> conntInsert = new List<Connectivity_n>();
                List<Connectivity_n> conntDelete = new List<Connectivity_n>();
                List<Connectivity_n> InsertEntUp = new List<Connectivity_n>();//修改增量

                CYZLog.writeLog("conntInsert start：", conntInsert.Count.ToString());

                var v1 = xmldb.GetEntities<Connectivity_n>();
                if (v1.Count()>0)
                {
                    foreach (var v2 in v1)
                    {
                        var _entityStatus = (EntityState)v2.GetValue("EntityState");

                        if ((int)_entityStatus <=3)
                        {
                            OldDateConnUpDel(v2, gc_id, ltt_name, appenshiptime, qj, _entityStatus, ref DicLong, ref   conntUpdate, ref  conntInsert, ref   conntDelete, ref  InsertTempTables);
                        }
                        else if ((int)_entityStatus <= 9 && (int)_entityStatus>5)
                        {
                            OldDateConnOldOld(v2, gc_id, ltt_name, appenshiptime, qj, _entityStatus, ref DicLong, ref   conntUpdate, ref  conntInsert, ref   conntDelete, ref  InsertTempTables);
                        }
                        else if ((int)_entityStatus <= 13 && (int)_entityStatus >9)
                        {
                            OldDateConnOld_Del(v2, gc_id, ltt_name, appenshiptime, qj, _entityStatus, ref DicLong, ref   conntUpdate, ref  conntInsert, ref   conntDelete, ref  InsertTempTables);

                        }else if((int)_entityStatus <=17 && (int)_entityStatus >13)
                        {
                            OldDateConnOld_Add(v2, gc_id, ltt_name, appenshiptime, qj, _entityStatus, ref DicLong, ref   conntUpdate, ref  conntInsert, ref   conntDelete, ref  InsertTempTables);
                        }
                        else if ((int)_entityStatus <= 21 && (int)_entityStatus >17)
                        {
                            OldDateConnOld_Nal(v2, gc_id, ltt_name, appenshiptime, qj, _entityStatus, ref DicLong, ref   conntUpdate, ref  conntInsert, ref   conntDelete, ref  InsertTempTables);
                        }
                        else if ((int)_entityStatus <= 25 && (int)_entityStatus > 21)
                        {
                            NewDateConnAddOld(v2, gc_id, ltt_name, appenshiptime, qj, _entityStatus, ref DicLong, ref   conntUpdate, ref  conntInsert, ref   conntDelete, ref  InsertTempTables);
                        }
                        else if ((int)_entityStatus <= 29 && (int)_entityStatus > 24)
                        {
                            NewDateConnAddDel(v2, gc_id, ltt_name, appenshiptime, qj, _entityStatus, ref DicLong, ref   conntUpdate, ref  conntInsert, ref   conntDelete, ref  InsertTempTables);
                        }
                        else if ((int)_entityStatus <= 34 && (int)_entityStatus > 29)
                        {
                            NewDateConnAddADD(v2, gc_id, ltt_name, appenshiptime, qj, _entityStatus, ref DicLong, ref   conntUpdate, ref  conntInsert, ref   conntDelete, ref  InsertTempTables);
                        }
                        else if ((int)_entityStatus <= 38 && (int)_entityStatus > 34)
                        {
                            NewDateConnAddNal(v2, gc_id, ltt_name, appenshiptime, qj, _entityStatus, ref DicLong, ref   conntUpdate, ref  conntInsert, ref   conntDelete, ref  InsertTempTables);
                        }
         
                    }


                    CYZLog.writeLog("conntInsertby：", conntInsert.Count.ToString());
                    CYZLog.writeLog("conntUpdateby：", conntUpdate.Count.ToString());
                    CYZLog.writeLog("conntDeleteby：", conntDelete.Count.ToString());
                    oracledb.InsertBulk(conntInsert.Cast<DBEntity>(), true, new ActiveJob(qj, ltt_name));
                    CYZLog.writeLog("conntInsert：", conntInsert.Count.ToString());
                    oracledb.UpdateBulk(conntUpdate.Cast<DBEntity>(), true, new ActiveJob(qj, ltt_name));
                    CYZLog.writeLog("conntUpdate：", conntUpdate.Count.ToString());
                    oracledb.DeleteBulk(conntDelete.Cast<DBEntity>(), true, new ActiveJob(qj, ltt_name));      

                    int ltt_idd = 0;
                    string sqlltt = "select LTT_ID from ltt_identifiers  where ltt_name='" + ltt_name + "'";
                    CDBManger cdb_lttid = new CDBManger();
                    DataTable dtLtt = cdb_lttid.ExecuteTable(sqlltt);
                    if (dtLtt != null && dtLtt.Rows.Count > 0)
                    {
                        ltt_idd = int.Parse(dtLtt.Rows[0]["LTT_ID"].ToString());
                    }
                    if (cdb_lttid != null) { cdb_lttid.close();}
                    foreach (var AppendShip in InsertTempTables)
                    {
                        InsertTempTable(
                            AppendShip.g3e_fid, AppendShip.g3e_fno, AppendShip.v2.GetValue("EntityState").ToString(),
                            AppendShip.GcTicket_ID, ltt_idd, appenshiptime, AppendShip.Sjversion);
                    }
                }
            }
            catch (NotExistException ex)
            {

            }
            catch (Exception ex)
            {
                CDBManger ddd = new CDBManger();
                OracleDataReader odr = ddd.ExecuteReader("select gisticket_id from  cadgis.ticketship");
                if (odr != null && odr.Read())
                {
                    CYZLog.writeLog("tttt:" + odr["gisticket_id"], "");
                }
                CloseOracleDataReader(odr, ddd);

                if (ex is NotExistException == false)
                    CYZLog.writeLog(ex.ToString(), "");
                throw ex;
              
            }

        }


        /// <summary>
        /// 包含关系
        /// </summary>
        /// <param name="oracledb"></param>
        /// <param name="xmldb"></param>
        /// <param name="gc_id"></param>
        /// <param name="ltt_name"></param>
        /// <param name="appenshiptime"></param>
        /// <param name="qj"> </param>
        public static void CONTAIN_N(OracleDBManager oracledb, XmlDBManager xmldb, string gc_id, string ltt_name, DateTime appenshiptime, string qj)
        {
            CDBManger cdb_lttid = null;
            try
            {
                OracleSequenceValueGenerator.Instance.DbManager = oracledb;

                List<ElectronTransferServicePro.InsertTempTable> InsertTempTables = new List<ElectronTransferServicePro.InsertTempTable>();
                List<ElectronBase> contInsert = new List<ElectronBase>();
                List<ElectronBase> contUpdate = new List<ElectronBase>();
                List<ElectronBase> contDelete = new List<ElectronBase>();

                var v1 = xmldb.GetEntities<Contain_n>();// .Cast<ElectronBase>();
                if (v1.Count()>0)
                {
                    int ltt_idd = 0;
                    string sqlltt = "select LTT_ID from ltt_identifiers  where ltt_name='" + ltt_name + "'";
                    cdb_lttid = new CDBManger();
                    OracleDataReader ltt_Reader = cdb_lttid.ExecuteReader(sqlltt);
                    if (ltt_Reader.Read())
                    {
                        ltt_idd = int.Parse(ltt_Reader["LTT_ID"].ToString());
                    }
                    CloseOracleDataReader(ltt_Reader, cdb_lttid);

                    foreach (var v2 in v1)
                    {
                        var _entityStatus = v2.GetValue("EntityState");
                        if ((int)_entityStatus == (int)EntityState.Insert)
                        {
                            long lll = OracleSequenceValueGenerator.Instance.GenerateTableId(v2.GetType());
                            v2.G3E_ID = lll;
                            v2.G3E_ID = lll;
                            long? OldOwnerFID = v2.G3E_OWNERFID;
                            long? Oldfid = v2.G3E_FID;

                            long NewOwnerFID = QueryContainFIdTemp(OldOwnerFID, gc_id);
                            long newFid = QueryContainFIdTemp(Oldfid, gc_id);

                            if (NewOwnerFID != 0 && newFid != 0)
                            {
                                v2.G3E_FID = newFid;
                                v2.G3E_OWNERFID = NewOwnerFID;
                                v2.G3E_OWNERFNO = 201;

                                contInsert.Add(v2);

                            }
                            else if (NewOwnerFID != 0 && newFid == 0)
                            {
                                v2.G3E_OWNERFID = NewOwnerFID;
                                v2.G3E_OWNERFNO = 201;

                                contInsert.Add(v2); ;
                            }
                            else if (NewOwnerFID == 0 && newFid != 0)
                            {
                                v2.G3E_FID = newFid;
                                contInsert.Add(v2);
                            }
                        }
                        if ((int)_entityStatus == (int)EntityState.Update)
                        {

                            long? OldOwnerFID = v2.G3E_OWNERFID;
                            long? Oldfid = v2.G3E_FID;
                            long NewOwnerFID = QueryContainFIdTemp(OldOwnerFID, gc_id);
                            long newFid = QueryContainFIdTemp(Oldfid, gc_id);

                            if (NewOwnerFID != 0 && newFid != 0)
                            {
                                v2.G3E_FID = newFid;
                                v2.G3E_OWNERFID = NewOwnerFID;
                                v2.G3E_OWNERFNO = 201;

                                contUpdate.Add(v2);

                                var vv = new ElectronTransferServicePro.InsertTempTable();
                                vv.g3e_fid = v2.G3E_FID;
                                vv.g3e_fno = v2.G3E_FNO;
                                vv.v2 = v2;
                                vv.ltt_name = ltt_name;
                                vv.GcTicket_ID = gc_id;
                                vv.Sjversion = int.Parse(v2.GetValue("Version").ToString());

                                InsertTempTables.Add(vv);
                            }
                            else if (NewOwnerFID != 0 && newFid == 0)
                            {
                                v2.G3E_OWNERFID = NewOwnerFID;
                                v2.G3E_OWNERFNO = 201;

                                contUpdate.Add(v2);
                                var vv = new ElectronTransferServicePro.InsertTempTable();
                                vv.g3e_fid = v2.G3E_FID;
                                vv.g3e_fno = v2.G3E_FNO;
                                vv.v2 = v2;
                                vv.ltt_name = ltt_name;
                                vv.GcTicket_ID = gc_id;
                                vv.Sjversion = int.Parse(v2.GetValue("Version").ToString());
                                InsertTempTables.Add(vv);
                            }
                            else if (NewOwnerFID == 0 && newFid != 0)
                            {
                                v2.G3E_FID = newFid;
                                contUpdate.Add(v2);
                                var vv = new ElectronTransferServicePro.InsertTempTable();
                                vv.g3e_fid = v2.G3E_FID;
                                vv.g3e_fno = v2.G3E_FNO;
                                vv.v2 = v2;
                                vv.ltt_name = ltt_name;
                                vv.GcTicket_ID = gc_id;
                                vv.Sjversion = int.Parse(v2.GetValue("Version").ToString());
                                InsertTempTables.Add(vv);
                            }
                        }
                        if ((int)_entityStatus == (int)EntityState.Delete)
                        {
                            contDelete.Add(v2);
                            var vv = new ElectronTransferServicePro.InsertTempTable();
                            vv.g3e_fid = v2.G3E_FID;
                            vv.g3e_fno = v2.G3E_FNO;
                            vv.v2 = v2;
                            vv.ltt_name = ltt_name;
                            vv.GcTicket_ID = gc_id;
                            vv.Sjversion = int.Parse(v2.GetValue("Version").ToString());
                            InsertTempTables.Add(vv);
                        }
                    }
                    QueryCont(ref contInsert, ref contUpdate);
                    oracledb.InsertBulk(contInsert.Cast<DBEntity>(), true, new ActiveJob(qj, ltt_name));
                    oracledb.UpdateBulk(contUpdate.Cast<DBEntity>(), true, new ActiveJob(qj, ltt_name));
                    oracledb.DeleteBulk(contDelete.Cast<DBEntity>(), true, new ActiveJob(qj, ltt_name));

                    foreach (var AppendShip in InsertTempTables)
                    {
                        InsertTempTable(
                            AppendShip.g3e_fid, AppendShip.g3e_fno, AppendShip.v2.GetValue("EntityState").ToString(),
                            AppendShip.GcTicket_ID, ltt_idd, appenshiptime, AppendShip.Sjversion);
                    }
                }
            }
            catch (NotExistException ex)
            {

            }
            catch (Exception ex)
            {
                CYZLog.writeLog(ex.ToString(), "");
            }
            finally
            {
                if (cdb_lttid != null)
                {
                    cdb_lttid.close();
                }

            }
        }


        /// <summary>
        /// 包含关系
        /// </summary>
        /// <param name="oracledb"></param>
        /// <param name="xmldb"></param>
        /// <param name="gc_id"></param>
        /// <param name="ltt_name"></param>
        /// <param name="appenshiptime"></param>
        public static void CONTAIN_N2(OracleDBManager oracledb, XmlDBManager xmldb, string gc_id, string ltt_name, DateTime appenshiptime, string qj)
        {
            CDBManger cdb_lttid = null;
            try
            {
                OracleSequenceValueGenerator.Instance.DbManager = oracledb;

                List<ElectronTransferServicePro.InsertTempTable> InsertTempTables = new List<ElectronTransferServicePro.InsertTempTable>();
                List<ElectronBase> contInsert = new List<ElectronBase>();
                List<ElectronBase> contUpdate = new List<ElectronBase>();
                List<ElectronBase> contDelete = new List<ElectronBase>();

                var v1 = xmldb.GetEntities<Contain_n>();// .Cast<ElectronBase>();
                if (v1.Count()>0)
                {
                    int ltt_idd = 0;
                    string sqlltt = "select LTT_ID from ltt_identifiers  where ltt_name='" + ltt_name + "'";
                    cdb_lttid = new CDBManger();
                    OracleDataReader ltt_Reader = cdb_lttid.ExecuteReader(sqlltt);
                    if (ltt_Reader.Read())
                    {
                        ltt_idd = int.Parse(ltt_Reader["LTT_ID"].ToString());
                    }
                    CloseOracleDataReader(ltt_Reader, cdb_lttid);

                    foreach (var v2 in v1)
                    {
                        var _entityStatus = v2.GetValue("EntityState");
                        if ((int)_entityStatus == (int)EntityState.Insert)
                        {
                            long lll = OracleSequenceValueGenerator.Instance.GenerateTableId(v2.GetType());
                            v2.G3E_ID = lll;
                            ((Contain_n)v2).G3E_ID = lll;
                            long? OldOwnerFID = v2.G3E_OWNERFID;
                            long? Oldfid = v2.G3E_FID;

                            long NewOwnerFID = QueryContainFIdTemp(OldOwnerFID, gc_id);
                            long newFid = QueryContainFIdTemp(Oldfid, gc_id);

                            if (NewOwnerFID != 0 && newFid != 0)
                            {
                                v2.G3E_FID = newFid;
                                v2.G3E_OWNERFID = NewOwnerFID;
                                v2.G3E_OWNERFNO = 201;

                                contInsert.Add(v2);

                            }
                            else if (NewOwnerFID != 0 && newFid == 0)
                            {
                                v2.G3E_OWNERFID = NewOwnerFID;
                                v2.G3E_OWNERFNO = 201;

                                contInsert.Add(v2); ;
                            }
                            else if (NewOwnerFID == 0 && newFid != 0)
                            {
                                v2.G3E_FID = newFid;
                                contInsert.Add(v2);
                            }
                        }
                        if ((int)_entityStatus == (int)EntityState.Update)
                        {

                            long? OldOwnerFID = v2.G3E_OWNERFID;
                            long? Oldfid = v2.G3E_FID;
                            long NewOwnerFID = QueryContainFIdTemp(OldOwnerFID, gc_id);
                            long newFid = QueryContainFIdTemp(Oldfid, gc_id);

                            if (NewOwnerFID != 0 && newFid != 0)
                            {
                                v2.G3E_FID = newFid;
                                v2.G3E_OWNERFID = NewOwnerFID;
                                v2.G3E_OWNERFNO = 201;

                                contUpdate.Add(v2);

                                var vv = new ElectronTransferServicePro.InsertTempTable();
                                vv.g3e_fid = v2.G3E_FID;
                                vv.g3e_fno = v2.G3E_FNO;
                                vv.v2 = v2;
                                vv.ltt_name = ltt_name;
                                vv.GcTicket_ID = gc_id;
                                vv.Sjversion = int.Parse(v2.GetValue("Version").ToString());
                                InsertTempTables.Add(vv);
                            }
                            else if (NewOwnerFID != 0 && newFid == 0)
                            {
                                v2.G3E_OWNERFID = NewOwnerFID;
                                v2.G3E_OWNERFNO = 201;

                                contUpdate.Add(v2);
                                var vv = new ElectronTransferServicePro.InsertTempTable();
                                vv.g3e_fid = v2.G3E_FID;
                                vv.g3e_fno = v2.G3E_FNO;
                                vv.v2 = v2;
                                vv.ltt_name = ltt_name;
                                vv.GcTicket_ID = gc_id;
                                vv.Sjversion = int.Parse(v2.GetValue("Version").ToString());
                                InsertTempTables.Add(vv);
                            }
                            else if (NewOwnerFID == 0 && newFid != 0)
                            {
                                v2.G3E_FID = newFid;
                                contUpdate.Add(v2);
                                var vv = new ElectronTransferServicePro.InsertTempTable();
                                vv.g3e_fid = v2.G3E_FID;
                                vv.g3e_fno = v2.G3E_FNO;
                                vv.v2 = v2;
                                vv.ltt_name = ltt_name;
                                vv.GcTicket_ID = gc_id;
                                vv.Sjversion = int.Parse(v2.GetValue("Version").ToString());
                                InsertTempTables.Add(vv);
                            }
                        }
                        if ((int)_entityStatus == (int)EntityState.Delete)
                        {
                            contDelete.Add(v2);
                            var vv = new ElectronTransferServicePro.InsertTempTable();
                            vv.g3e_fid = v2.G3E_FID;
                            vv.g3e_fno = v2.G3E_FNO;
                            vv.v2 = v2;
                            vv.ltt_name = ltt_name;
                            vv.GcTicket_ID = gc_id;
                            vv.Sjversion = int.Parse(v2.GetValue("Version").ToString());
                            InsertTempTables.Add(vv);
                        }
                    }

                    oracledb.InsertBulk(contInsert.Cast<DBEntity>(), true, new ActiveJob(qj, ltt_name));
                    oracledb.UpdateBulk(contUpdate.Cast<DBEntity>(), true, new ActiveJob(qj, ltt_name));
                    oracledb.DeleteBulk(contDelete.Cast<DBEntity>(), true, new ActiveJob(qj, ltt_name));

                    foreach (var AppendShip in InsertTempTables)
                    {
                        InsertTempTable(
                            AppendShip.g3e_fid, AppendShip.g3e_fno, AppendShip.v2.GetValue("EntityState").ToString(),
                            AppendShip.GcTicket_ID, ltt_idd, appenshiptime, AppendShip.Sjversion);
                    }
                }
            }
            catch (NotExistException ex)
            {

            }
            catch (Exception ex)
            {
                CYZLog.writeLog(ex.ToString(), "");
            }
            finally
            {
                if (cdb_lttid != null)
                {
                    cdb_lttid.close();
                }
            }

        }


        /// <summary>
        /// 查询包含关系表2次导入的时候是否已经导入过
        /// </summary>
        /// <param name="contInsert"></param>
        /// <param name="contUpdate"></param>
        public static void QueryCont(ref List<ElectronBase> contInsert, ref List<ElectronBase> contUpdate)
        {
            CDBManger dbm = null;
            OracleDataReader dr = null;
            List<ElectronBase> InsDateEntCopy = new List<ElectronBase>();//修改增量
            //List<ElectronBase> InsDateEntCopy = new List<ElectronBase>();

            try
            {
                Dictionary<long, List<ElectronBase>> upTemp = new Dictionary<long, List<ElectronBase>>();
                dbm = new CDBManger();

                foreach (var VarFid in contInsert)
                {
                    string AllFidsSql = "select * from gzdlgis.b$contain_n where g3e_fid ='" + VarFid.G3E_FID + "' and ltt_status='ADD' and g3e_ownerfid=" + ((Contain_n)VarFid).G3E_OWNERFID;
                    dr = dbm.ExecuteReader(AllFidsSql);
                    if (dr.Read() && dr != null)
                    {
                        contUpdate.Add(VarFid);
                        InsDateEntCopy.Add(VarFid);
                    }
                }

                foreach (var VARIABLE in InsDateEntCopy)
                {
                    contInsert.Remove(VARIABLE);
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
        /// //查找连接关系的fid
        /// </summary>
        /// <param name="CadFid"></param>
        /// <param name="GC_ID"></param>
        /// <returns></returns>
        public static long QueryTemp(long CadFid, string GC_ID)
        {
            CDBManger cdb_QueryTemp = null;
            long NewFid = 0;

            try
            {
                cdb_QueryTemp = new CDBManger();
                string sql = "select G3E_FID from cadgis.AppendFidShip  where  G3E_fid2=" + CadFid + " and gcticket_id='" + GC_ID + "'";
                DataTable dtFID = cdb_QueryTemp.ExecuteTable(sql);
                if (dtFID != null && dtFID.Rows.Count > 0)
                {
                    NewFid = int.Parse(dtFID.Rows[0]["G3E_FID"].ToString());
                }
            }
            catch (Exception ex)
            {
                CYZLog.writeLog(ex.ToString(), "");
            }
            finally
            {
                if (cdb_QueryTemp != null)
                {
                    cdb_QueryTemp.close();
                }
            }
            return NewFid;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="g3e_fid"></param>
        /// <param name="g3e_fno"></param>
        /// <param name="EntityState"> </param>
        /// <param name="GC_ID"></param>
        /// <param name="ltt_idd"></param>
        /// <param name="appenshiptime"></param>
        /// <param name="verion"> </param>
        public static void InsertTempTable(long g3e_fid, int g3e_fno, string EntityState, string GC_ID, int ltt_idd, DateTime appenshiptime, int verion)
        {
            CDBManger cdb_AppendShip = null;
            CDBManger cdb_yxfid = null;
            CDBManger cdb_insert = null;


            int kxid = 0;
            string ssdw = "";
            string sql1 = "";
            try
            {
                if (g3e_fno == 250 || g3e_fno == 160)
                {
                    return;
                }
                if (ltt_idd == 0)
                {
                    return;
                }

                cdb_AppendShip = new CDBManger();
                cdb_yxfid = new CDBManger();
                cdb_insert = new CDBManger();

                string sql = "select count(1) from cadgis.AppendShip  where  g3e_fid=" + g3e_fid + " and gcticket_id='" + GC_ID + "'";
                DataTable dtAppendShip = cdb_AppendShip.ExecuteTable(sql);
                if (dtAppendShip != null && dtAppendShip.Rows.Count > 0)
                {
                    if (dtAppendShip.Rows[0][0].ToString() != "0")
                    {
                        sql = "delete cadgis.AppendShip  where  g3e_fid=" + g3e_fid + " and gcticket_id='" + GC_ID + "'";
                        cdb_AppendShip.ExecuteNonQuery(sql);
                    }
                }

                if (g3e_fno == 160)
                {
                    sql1 = "select cd_ssxl from b$gg_pd_dyshb_n t where  t.g3e_fid=" + g3e_fid + " order by ltt_tid desc ";
                }
                else if (g3e_fno == 159)
                {
                    sql1 = "select cd_ssxl from b$gg_pd_cbx_n t where t.g3e_fid=" + g3e_fid + " order by ltt_tid desc ";
                }
                else
                {
                    sql1 = "select t.cd_ssxl from b$common_n t where t.g3e_fid=" + g3e_fid + " order by ltt_tid desc";
                }

                //sql1 = "select yx_fid,gds1 from Gg_Kxinfo where kxmc in ("+sql1+")";
                sql1 = "select distinct a.yx_fid,b.cd_ssdw from Gg_Kxinfo a,b$common_n  b where a.yx_fid=b.g3e_fid  and a.kxmc in ( select  * from ( " + sql1 + " )  where rownum<2 )";
                DataTable dtYXFID = cdb_yxfid.ExecuteTable(sql1);
                if (dtYXFID != null && dtYXFID.Rows.Count > 0)
                {
                    kxid = int.Parse(dtYXFID.Rows[0]["yx_fid"].ToString());
                    ssdw = dtYXFID.Rows[0]["cd_ssdw"].ToString();
                }


                string sql2 = "insert into cadgis.AppendShip(G3E_FID,G3E_FNO,KX_FID,GisTicket_ID,GcTicket_ID,SSDW,AppType,AppendTime,VERSION) values("
                                    + g3e_fid + "," + g3e_fno + "," + kxid + "," + ltt_idd + ",'" + GC_ID + "'," + "'" + ssdw + "'" + ",'" + EntityState
                                    + "', to_date('" + appenshiptime.ToString("yyyy-MM-dd HH:mm:ss") + "','yyyy-MM-dd HH24:mi:ss') " + "," + verion + ")";

                cdb_insert.ExecuteNonQuery(sql2);



                CYZLog.writeLog("insert into cadgis.AppendShip");
            }
            catch (Exception ex)
            {
                CYZLog.writeLog(ex.ToString(), "");
            }
            finally
            {

                if (cdb_AppendShip != null) { cdb_AppendShip.close(); }
                if (cdb_yxfid != null) { cdb_AppendShip.close(); }
                if (cdb_insert != null) { cdb_insert.close(); }

            }
        }


        /// <summary>
        /// 查询祥图
        /// </summary>
        /// <param name="oldg3e_fid"> </param>
        /// <param name="g3e_fno"></param>
        /// <param name="v2"></param>
        /// <param name="ltt_name"></param>
        /// <param name="GC_ID"></param>
        /// <param name="ltt_idd"></param>
        /// <param name="appenshiptime"></param>
        /// <returns></returns>
        public static long selectG3e_detatlid(long oldg3e_fid, int g3e_fno, DBEntity v2, string ltt_name, string GC_ID, decimal? ltt_idd, DateTime appenshiptime)
        {
            CDBManger cdb_newfid = null;
            long new_g3e_fid = 0;
            int num = 0;
            try
            {
                cdb_newfid = new CDBManger();


                string sql = " select g3e_fid from cadgis.appendfidship where g3e_fid2 ='" + oldg3e_fid + "' and gcticket_id=" + GC_ID;
                DataTable dtNewfid = cdb_newfid.ExecuteTable(sql);
                if (dtNewfid != null && dtNewfid.Rows.Count > 0)
                {
                    new_g3e_fid = long.Parse(dtNewfid.Rows[0]["g3e_fid"].ToString());
                }
                else
                {
                    new_g3e_fid = 0;
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
        /// 插入临时表
        /// </summary>
        /// <param name="oldg3e_fid"></param>
        /// <param name="g3e_fno"></param>
        /// <param name="v2"></param>
        /// <param name="ltt_name"></param>
        /// <param name="GC_ID"></param>
        /// <param name="ltt_idd"></param>
        /// <param name="appenshiptime"></param>
        /// <param name="OldG3e_id"> </param>
        /// <param name="OldG3e_detailid"> </param>
        /// <returns></returns>
        public static long InsertG3e_detatlid2(long oldg3e_fid, int g3e_fno, DBEntity v2, string ltt_name, string GC_ID, decimal? ltt_idd, DateTime appenshiptime, long? OldG3e_id, long? OldG3e_detailid)
        {
            CDBManger cdb_newfid = null;
            long new_g3e_fid = 0;
            int num = 0;
            DataTable dtNewfid = null;
            try
            {
                cdb_newfid = new CDBManger();


                string sql2 = "select G3E_DETAILID from cadgis.appendfidship where g3e_fid2=" + oldg3e_fid;
                dtNewfid = cdb_newfid.ExecuteTable(sql2);
                if (dtNewfid != null && dtNewfid.Rows.Count > 0)
                {
                    new_g3e_fid = long.Parse(dtNewfid.Rows[0]["G3E_DETAILID"].ToString());
                }
                else
                {
                    new_g3e_fid = 0;
                    string sql = "insert into cadgis.AppendFidShip(G3E_FID2,GCTICKET_ID,G3E_ID,G3E_ID2,VERSION,G3E_FNO,G3E_DETAILID,G3E_DETAILID2) values(" + oldg3e_fid + ",'"
                               + GC_ID + "', " + v2.GetValue("G3E_ID") + "," + OldG3e_id + "," + v2.GetValue("Version") + "," + g3e_fno + "," + v2.GetValue("G3E_DETAILID") + "," + OldG3e_detailid + ")";

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
        /// 查询集抄和祥图对应关系
        /// </summary>
        /// <param name="oldg3e_fid"></param>
        /// <param name="g3e_fno"></param>
        /// <param name="v2"></param>
        /// <param name="ltt_name"></param>
        /// <param name="GC_ID"></param>
        /// <param name="ltt_idd"></param>
        /// <param name="appenshiptime"></param>
        /// <returns></returns>
        /// 
        public static long selectG3e_Gg_jx_shbd_ptfid(long oldg3e_fid, int g3e_fno, DBEntity v2, string ltt_name, string GC_ID, decimal? ltt_idd, DateTime appenshiptime)
        {

            CDBManger cdb_newfid = null;
            long new_g3e_fid = 0;
            int num = 0;
            try
            {
                cdb_newfid = new CDBManger();

                string sql = " select g3e_fid from cadgis.appendfidship where g3e_fid2 ='" + oldg3e_fid + "'";
                DataTable dtNewfid = cdb_newfid.ExecuteTable(sql);
                if (dtNewfid != null && dtNewfid.Rows.Count > 0)
                {
                    new_g3e_fid = long.Parse(dtNewfid.Rows[0]["g3e_fid"].ToString());
                }
                else
                {
                    new_g3e_fid = 0;
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
        /// 查询集抄和祥图对应关系
        /// </summary>
        /// <param name="oldG3e_detatlid"></param>
        /// <param name="g3e_fno"></param>
        /// <param name="v2"></param>
        /// <param name="ltt_name"></param>
        /// <param name="GC_ID"></param>
        /// <param name="ltt_idd"></param>
        /// <param name="appenshiptime"></param>
        /// <returns></returns>
        /// 
        public static long selectG3e_Gg_jx_shbd_ptG3e_detatlid(long? oldG3e_detatlid, int g3e_fno, DBEntity v2, string ltt_name, string GC_ID, decimal? ltt_idd, DateTime appenshiptime)
        {

            CDBManger cdb_newfid = null;
            long new_etatlid = 0;
            int num = 0;
            try
            {
                cdb_newfid = new CDBManger();

                string sql = " select G3e_detailid from cadgis.appendfidship where G3e_detailid2 ='" + oldG3e_detatlid + "'";
                DataTable dtNewfid = cdb_newfid.ExecuteTable(sql);
                if (dtNewfid != null && dtNewfid.Rows.Count > 0)
                {
                    new_etatlid = long.Parse(dtNewfid.Rows[0]["G3e_detailid"].ToString());
                }
                else
                {
                    new_etatlid = 0;
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
            return new_etatlid;

        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="newg3e_detailid"></param>
        /// <param name="oldfid"></param>
        /// <param name="oldid"></param>
        /// <param name="oldg3e_detailid"></param>
        /// <param name="newfid"> </param>
        /// <returns></returns>
        public static long UpdateG3e_detatlid(long? newg3e_detailid, long oldfid, long oldid, long? oldg3e_detailid, long newfid)
        {
            CDBManger insert_newfid = null;
            long new_g3e_fid = 0;
            bool InsSign = false;
            try
            {
                insert_newfid = new CDBManger();

                string sql = "update  cadgis.appendfidship  set g3e_detailid=" + newg3e_detailid + "," + " g3e_detailid2=" + oldg3e_detailid + " where g3e_fid=" + newfid;
                insert_newfid.ExecuteNonQuery(sql);
            }
            catch (System.Exception ex)
            {
                CYZLog.writeLog(ex.ToString(), "");
            }
            finally
            {
                if (insert_newfid != null)
                {
                    insert_newfid.close();
                }
            }
            return new_g3e_fid;
        }



        /// <summary>
        /// 修改杂项标注的对应设备fid
        /// </summary>
        /// <param name="Cadzxg3e_fid"></param>
        /// <param name="CadDySbG3e_fid"></param>
        /// <param name="zxg3e_fid"></param>
        /// <param name="DySbG3e_fid"></param>
        public static void UpdateGg_gl_zxbz_n_G3e_fid(long Cadzxg3e_fid, string CadDySbG3e_fid, ref long zxg3e_fid, ref  string DySbG3e_fid)
        {
            CDBManger insert_newfid = null;
            DataTable dtQueryZxbz = null;

            try
            {
                insert_newfid = new CDBManger();

                string sql = "select g3e_fid from  cadgis.appendfidship  where  g3e_fid2=" + Cadzxg3e_fid;
                dtQueryZxbz = insert_newfid.ExecuteTable(sql);
                if (dtQueryZxbz != null && dtQueryZxbz.Rows.Count > 0)
                {
                    zxg3e_fid = long.Parse(dtQueryZxbz.Rows[0]["g3e_fid"].ToString());
                }
                else
                {
                    zxg3e_fid = 0;
                }

                string sql2 = "select g3e_fid from  cadgis.appendfidship  where  g3e_fid2=" + CadDySbG3e_fid;

                DataTable dtQueryZxbz2 = insert_newfid.ExecuteTable(sql2);
                if (dtQueryZxbz2 != null && dtQueryZxbz2.Rows.Count > 0)
                {
                    DySbG3e_fid = dtQueryZxbz2.Rows[0]["g3e_fid"].ToString();
                }
                else
                {
                    DySbG3e_fid = null;
                }


            }
            catch (System.Exception ex)
            {
                CYZLog.writeLog(ex.ToString(), "");
            }
            finally
            {

                if (insert_newfid != null)
                {
                    insert_newfid.close();
                }
            }

        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="v2"></param>
        /// <param name="oldfid"></param>
        /// <param name="oldid"></param>
        /// <param name="oldg3e_detailid"></param>
        /// <returns></returns>
        public static long InsertG3e_detatlid(DBEntity v2, long oldfid, long oldid, long oldg3e_detailid)
        {
            CDBManger insert_newfid = null;

            long new_g3e_fid = 0;
            bool InsSign = false;

            try
            {
                insert_newfid = new CDBManger();
                string sql2 = "insert into cadgis.appendfidship (G3E_FID,g3e_fid2,gcticket_id,g3e_id,g3e_id2,g3e_detailid,g3e_detailid2) values("
                    + v2.GetValue("g3e_fid") + "," + oldfid + ",'" + v2.GetValue("gcticket_id") + "'," + v2.GetValue("g3e_id") + "," + oldid + "," + v2.GetValue("G3E_DETAILID") + "," + oldg3e_detailid + ")";
                InsSign = insert_newfid.ExecuteNonQuery(sql2);
            }
            catch (System.Exception ex)
            {
                CYZLog.writeLog(ex.ToString(), "");
            }
            finally
            {
                if (insert_newfid != null)
                {
                    insert_newfid.close();
                }
            }

            return new_g3e_fid;
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="Owner_id"></param>
        /// <param name="GC_ID"></param>
        /// <returns></returns>
        public static long QueryOwnerIdTemp(long? Owner_id, string GC_ID)
        {
            CDBManger cdm_QueryOwnerIdTemp = null;
            long NewOwner_id = 0;
            try
            {
                cdm_QueryOwnerIdTemp = new CDBManger();
                if (Owner_id != null && Owner_id.ToString() != "")
                {
                    string sql = "select g3e_id from cadgis.appendfidship where g3e_id2=" + Owner_id + "  and gcticket_id='" + GC_ID + "'";

                    DataTable dtQueryOwnerIdTemp = cdm_QueryOwnerIdTemp.ExecuteTable(sql);
                    if (dtQueryOwnerIdTemp != null && dtQueryOwnerIdTemp.Rows.Count > 0)
                    {
                        NewOwner_id = long.Parse(dtQueryOwnerIdTemp.Rows[0]["g3e_id"].ToString());
                    }
                    else
                    {
                        sql = "select g3e_id from b$common_n where g3e_id= " + Owner_id;
                        DataTable dtQueryOwnerIdTemp2 = cdm_QueryOwnerIdTemp.ExecuteTable(sql);
                        if (dtQueryOwnerIdTemp2 != null && dtQueryOwnerIdTemp2.Rows.Count > 0)
                        {
                            NewOwner_id = (long)Owner_id;
                        }
                    }
                }
                else
                {
                    NewOwner_id = 0;
                }
            }
            catch (Exception ex)
            {
                CYZLog.writeLog(ex.ToString(), "");
            }
            finally
            {
                if (cdm_QueryOwnerIdTemp != null)
                {
                    cdm_QueryOwnerIdTemp.close();
                }
            }
            return NewOwner_id;
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="CadFid"></param>
        /// <param name="GC_ID"></param>
        /// <param name="g3e_fid"></param>
        /// <param name="g3e_id"></param>
        /// <returns></returns>
        public static bool QueryCommFid(long? CadFid, string GC_ID, ref long g3e_fid, ref long g3e_id)
        {
            CDBManger cdm_QueryCommFid = null;
            bool reval = false;
            try
            {
                if (CadFid == null || CadFid == 0)
                {
                    return true;
                }
                string sql = "select g3e_id,g3e_fid from cadgis.appendfidship where g3e_fid2=" + CadFid + " and gcticket_id='" + GC_ID + "'";
                cdm_QueryCommFid = new CDBManger();
                DataTable dtQueryCommFid = cdm_QueryCommFid.ExecuteTable(sql);
                if (dtQueryCommFid != null && dtQueryCommFid.Rows.Count > 0)
                {
                   
                        g3e_fid = int.Parse(dtQueryCommFid.Rows[0]["g3e_fid"].ToString());
                    
                    
                    g3e_id = int.Parse(dtQueryCommFid.Rows[0]["g3e_id"].ToString());
                }

                reval = true;
            }
            catch (Exception ex)
            {
                CYZLog.writeLog(ex.ToString(), "");
            }
            finally
            {
                if (cdm_QueryCommFid != null)
                {
                    cdm_QueryCommFid.close();
                }
            }
            return reval;
        }



        /// <summary>
        /// 得到分线支路的fid
        /// </summary>
        /// <param name="CadFid"></param>
        /// <param name="GC_ID"></param>
        /// <param name="g3e_fid"></param>
        /// <param name="g3e_id"></param>
        /// <returns></returns>
        public static bool QueryCommFid(long? CadFid, string GC_ID, ref long g3e_fid)
        {
            CDBManger cdm_QueryCommFid = null;
            bool reval = false;
            try
            {
                if (CadFid == null || CadFid == 0)
                {
                    return true;
                }
                string sql = "select g3e_id,g3e_fid from cadgis.appendfidship where g3e_fid2=" + CadFid + " and gcticket_id='" + GC_ID + "'";
                cdm_QueryCommFid = new CDBManger();
                DataTable dtQueryCommFid = cdm_QueryCommFid.ExecuteTable(sql);
                if (dtQueryCommFid != null && dtQueryCommFid.Rows.Count > 0)
                {

                    g3e_fid = int.Parse(dtQueryCommFid.Rows[0]["g3e_fid"].ToString());


                //    g3e_id = int.Parse(dtQueryCommFid.Rows[0]["g3e_id"].ToString());
                }

                reval = true;
            }
            catch (Exception ex)
            {
                CYZLog.writeLog(ex.ToString(), "");
            }
            finally
            {
                if (cdm_QueryCommFid != null)
                {
                    cdm_QueryCommFid.close();
                }
            }
            return reval;
        }


       
        /// <summary>
        /// 
        /// </summary>
        /// <param name="CadFid"></param>
        /// <param name="GC_ID"></param>
        /// <returns></returns>
        public static long QueryContainFIdTemp(long? CadFid, string GC_ID)
        {
            long gisfid = 0;
            CDBManger cdm_QueryContainFIdTemp = null;
            try
            {
                if (CadFid == null || CadFid == 0)
                {
                    return 0;
                }
                cdm_QueryContainFIdTemp = new CDBManger();
                string sql = "select g3e_fid from cadgis.appendfidship where g3e_fid2=" + CadFid + " and gcticket_id='" + GC_ID + "'";
                DataTable dtQueryContainFId = cdm_QueryContainFIdTemp.ExecuteTable(sql);
                if (dtQueryContainFId != null && dtQueryContainFId.Rows.Count > 0)
                {
                    gisfid = long.Parse(dtQueryContainFId.Rows[0]["g3e_fid"].ToString());
                }
                else
                {
                    sql = "select * from b$common_n where g3e_fid= " + CadFid;
                    DataTable dtQueryContainFId2 = cdm_QueryContainFIdTemp.ExecuteTable(sql);
                    if (dtQueryContainFId2 != null && dtQueryContainFId2.Rows.Count > 0)
                    {
                        gisfid = (long)CadFid;
                    }
                }
            }
            catch (Exception ex)
            {
                CYZLog.writeLog(ex.ToString(), "");
            }
            finally
            {
                if (cdm_QueryContainFIdTemp != null)
                {
                    cdm_QueryContainFIdTemp.close();
                }
            }
            return gisfid;
        }


        /// <summary>
        /// contain_n如果是新增的数据但是fid是旧的这个要插入新增表中
        /// </summary>
        /// <param name="CadFid"></param>
        /// <param name="GC_ID"></param>
        /// <returns></returns>
        public static long QueryContainAppFid(long? CadFid, string GC_ID, ref int Bj)
        {
            long gisfid = 0;
            CDBManger cdm_QueryContainFIdTemp = null;
            try
            {
                if (CadFid == null || CadFid == 0)
                {
                    return 0;
                }
                cdm_QueryContainFIdTemp = new CDBManger();
                string sql = "select g3e_fid from cadgis.appendfidship where g3e_fid2=" + CadFid + " and gcticket_id='" + GC_ID + "'";
                DataTable dtQueryContainFId = cdm_QueryContainFIdTemp.ExecuteTable(sql);
                if (dtQueryContainFId != null && dtQueryContainFId.Rows.Count > 0)
                {
                    gisfid = long.Parse(dtQueryContainFId.Rows[0]["g3e_fid"].ToString());
                    Bj = 0;
                }
                else
                {
                    sql = "select * from b$common_n where g3e_fid= " + CadFid;
                    DataTable dtQueryContainFId2 = cdm_QueryContainFIdTemp.ExecuteTable(sql);
                    if (dtQueryContainFId2 != null && dtQueryContainFId2.Rows.Count > 0)
                    {
                        gisfid = (long)CadFid;
                        Bj = 1;
                    }
                }
            }
            catch (Exception ex)
            {
                CYZLog.writeLog(ex.ToString(), "");
            }
            finally
            {
                if (cdm_QueryContainFIdTemp != null)
                {
                    cdm_QueryContainFIdTemp.close();
                }
            }
            return gisfid;
        }


        /// <summary>
        /// 如果是已经存在的新增数据那么就要把连接关系已经存在的新增数据放到updatelist集合
        /// </summary>
        /// <param name="InsDateEnt"></param>
        /// <param name="UpDateEnt"></param>
        /// <param name="gc_id"></param>
        /// <param name="InsDateEntCopy"></param>
        /// <param name="version"></param>
        public static void UpdalistConn(ref  List<Connectivity_n> InsDateEnt, ref  List<Connectivity_n> UpDateEnt, string gc_id, ref List<Connectivity_n> InsDateEntCopy, int version)
        {
            CDBManger dbm = null;
            OracleDataReader dr = null;

            try
            {
                Dictionary<long, List<Connectivity_n>> upTemp = new Dictionary<long, List<Connectivity_n>>();

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
                        List<Connectivity_n> ttlist = new List<Connectivity_n>();
                        ttlist.Add(VarFid);
                        upTemp.Add(VarFid.G3E_FID, ttlist);
                    }
                }
                if (fids == "")
                    return;

                fids = fids.Substring(0, fids.Length - 1);
                string AllFidsSql = "select g3e_fid from cadgis.appendfidship where gcticket_id='" + gc_id + "' and version=" + version + " and g3e_fid  in(" + fids + ")";
                dbm = new CDBManger();
                dr = dbm.ExecuteReader(AllFidsSql);
                while (dr.Read())
                {
                    if (upTemp.Keys.Contains(long.Parse(dr["G3E_FID"].ToString())))
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
        /// 如果是已经存在的新增数据那么就要把连接关系已经存在的新增数据放到updatelist集合
        /// </summary>
        /// <param name="InsDateEnt"></param>
        /// <param name="UpDateEnt"></param>
        /// <param name="gc_id"></param>
        /// <param name="InsDateEntCopy"></param>
        /// <param name="version"></param>
        public static void Updalist2(ref  List<ElectronBase> InsDateEnt, ref  List<ElectronBase> UpDateEnt, string gc_id, ref List<ElectronBase> InsDateEntCopy, int version)
        {
            CDBManger dbm = null;
            OracleDataReader dr = null;

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
                string AllFidsSql = "select g3e_fid from cadgis.appendfidship where gcticket_id='" + gc_id + "' and version=" + version + " and g3e_fid  in(" + fids + ")";
                dbm = new CDBManger();
                dr = dbm.ExecuteReader(AllFidsSql);
                while (dr.Read())
                {
                    if (upTemp.Keys.Contains(long.Parse(dr["G3E_FID"].ToString())))
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
        /// 如果是光详图已经存在的新增数据那么就要把详图表的
        /// </summary>
        /// <param name="InsDateEnt"></param>
        /// <param name="UpDateEnt"></param>
        /// <param name="gc_id"></param>
        /// <param name="InsDateEntCopy"></param>
        /// <param name="version"></param>
        public static void Updalist3(ref  List<ElectronBase> InsDateEnt, ref  List<ElectronBase> UpDateEnt, string gc_id, ref List<ElectronBase> InsDateEntCopy, int version)
        {
            CDBManger dbm = null;
            OracleDataReader dr = null;

            CDBManger dbm2 = null;
            OracleDataReader dr2 = null;
            List<ElectronBase> insDetailreference = new List<ElectronBase>();
            // List<ElectronBase> UpDateDel = new List<ElectronBase>();

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
                string AllFidsSql = "select g3e_fid from cadgis.appendfidship where gcticket_id='" + gc_id + "' and version=" + version + " and g3e_fid  in(" + fids + ")";
                dbm = new CDBManger();
                dr = dbm.ExecuteReader(AllFidsSql);
                while (dr.Read())
                {
                    if (upTemp.Keys.Contains(long.Parse(dr["G3E_FID"].ToString())))
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
                string AllFidsSql2 = "select g3e_fid2 from cadgis.appendfidship where gcticket_id='" + gc_id + "' and version=" + version + " and g3e_fid2  in(" + fids + ")";
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
                CloseOracleDataReader(dr, dbm);
                CloseOracleDataReader(dr2, dbm2);
            }
        }
        /// <summary>
        /// 查询已经存在的版本号
        /// </summary>
        /// <param name="gc_id"></param>
        /// <param name="verson"></param>
        public static void QuVersion(string gc_id, ref int verson)
        {
            CDBManger AppCadFid = null;
            OracleDataReader AppCaddr = null;
            try
            {
                //1.如果在appendfidship找不到og3e_fid,那么就可以在gis里面新增fid
                AppCadFid = new CDBManger();
                string AppCadSql = "select version from cadgis.appendfidship t  where t.gcticket_id='" + gc_id + "'" + " order by t.version";
                AppCaddr = AppCadFid.ExecuteReader(AppCadSql);
                if (AppCaddr.Read() && AppCaddr != null)
                {
                    verson = int.Parse(AppCaddr["version"].ToString());
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



        public class ActiveJob : ISurround
        {
            public string Configuration { get; private set; }
            public string JobName { get; private set; }

            public ActiveJob(string configuration, string jobName)
            {
                Configuration = configuration;
                JobName = jobName;
             //   CYZLog.writeLog(configuration.ToString());
            }
            public string Begin()
            {
                return string.Format("BEGIN\nLTT_USER.SETCONFIGURATION('{0}');\nLTT_USER.EDITJOB('{1}');\nEND;", Configuration, JobName);
            }

            public string End()
            {
                return "BEGIN\nLtt_user.Done;\nEND;";
            }
        }
    }


}