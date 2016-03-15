using System;
using System.Text;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using ElectronTransferDal.Common;
using ElectronTransferDal.OracleDal;
using ElectronTransferDal.XmlDal;
using ElectronTransferModel.V9_4;
using Oracle.DataAccess.Client;
using ElectronTransferFramework;
using ElectronTransferModel.Base;
using System.IO;
using ElectronTransferModel.Config;
using System.Reflection;
using CYZFramework.Log;
//using System.Data.OracleClient;

namespace ElectronTransferServiceDll
{
    public class ExportSymbolEventHandler
    {
        public static void exportHandle(XmlDBManager xmldb, OracleDBManagerV94 oracledb, string session_id, string ftplj, decimal? ltt_id)
        {
            try
            {
                

                try
                {
                    OracleSequenceValueGenerator.Instance.DbManager = oracledb;

                    string symboldir = Path.Combine(Assembly.GetExecutingAssembly().GetPhysicalDirectory(), "SymbolConfig.xml");
                    SimpleSymbolConfig.Instance = ElectronTransferFramework.Serialize.XmlSerializeUtils.Load<SimpleSymbolConfig>(symboldir, new Type[] { });

                }
                catch (Exception ex)
                {
                    CYZLog.writeLog(ex.Message, "");
                }
                int i = 0;
                string[] allowStatus = new string[] { "EDIT", "ADD", "DELETE" };
                int fnocount = SimpleSymbolConfig.Instance.Symbols.Count;
                foreach (var symbol in SimpleSymbolConfig.Instance.Symbols)
                {
                    try
                    {
                        //需要导出详图的设备
                        if (symbol.Fno == 159 || symbol.Fno == 84 || symbol.Fno == 148)
                        {
                            #region 
                            var DetailSet = oracledb.GetEntities(typeof(Detailreference_n), " where g3e_fid in (select g3e_fid from " + session_id + " where g3e_fno=" + symbol.Fno + ") ").Cast<ElectronBase>().ToList();
                            var ASet_Detail = from item in DetailSet.Where(o => o.GetValue<decimal?>("LTT_ID") == ltt_id
                                && allowStatus.Contains(o.GetValue<string>("LTT_STATUS")))
                                              group item by item.G3E_FID into g
                                              select g.OrderBy(o => o.G3E_ID).Last();

                            var BSet_Detail = from item in DetailSet.Where(o => o.GetValue<decimal?>("LTT_ID") == 0
                                && ASet_Detail.Count(a => a.G3E_FID == o.G3E_FID) == 0)
                                              select item;
                            var UnionSet_Detail = ASet_Detail.Union(BSet_Detail);
                            xmldb.InsertBulk(UnionSet_Detail.Cast<DBEntity>());
                            #endregion

                        }
                        //根据比哪一期导出台架
                       if (symbol.Fno == 148)
                        {
                            #region
                            StringBuilder sb = new StringBuilder(" where g3e_fid in (   select gnwz_sstj from b$gg_pd_gnwzmc_n  where g3e_fno=148 and g3e_fid in ");
                            sb.Append("(  select g3e_fid from " + session_id + " where g3e_fno=148)) ");



                            var shbdSet = oracledb.GetEntities(typeof(Gg_gz_tj_n), sb.ToString()).Cast<ElectronBase>().ToList();
                            var ASet_shbd = from item in shbdSet.Where(o => o.GetValue<decimal?>("LTT_ID") == ltt_id
                                && allowStatus.Contains(o.GetValue<string>("LTT_STATUS")))
                                            group item by item.G3E_FID into g
                                            select g.OrderBy(o => o.G3E_ID).Last();

                            var BSet_shbd = from item in shbdSet.Where(o => o.GetValue<decimal?>("LTT_ID") == 0
                                && ASet_shbd.Count(a => a.G3E_FID == o.G3E_FID) == 0)
                                            select item;
                            var UnionSet_shbd = ASet_shbd.Union(BSet_shbd);
                            xmldb.InsertBulk(UnionSet_shbd.Cast<DBEntity>());



                            var shbdSet1 = oracledb.GetEntities(typeof(Common_n), sb.ToString()).Cast<ElectronBase>().ToList();
                            var ASet_shbd1 = from item in shbdSet1.Where(o => o.GetValue<decimal?>("LTT_ID") == ltt_id
                                && allowStatus.Contains(o.GetValue<string>("LTT_STATUS")))
                                             select item;
                            var BSet_shbd1 = from item in shbdSet1.Where(o => o.GetValue<decimal?>("LTT_ID") == 0
                                && ASet_shbd1.Count(a => a.G3E_FID == o.G3E_FID) == 0)
                                             select item;
                            var UnionSet_shbd1 = ASet_shbd1.Union(BSet_shbd1);
                            xmldb.InsertBulk(UnionSet_shbd1.Cast<DBEntity>());


                            var shbdSet2 = oracledb.GetEntities(typeof(Gg_pd_gnwzmc_n), sb.ToString()).Cast<ElectronBase>().ToList();
                            var ASet_shbd2 = from item in shbdSet2.Where(o => o.GetValue<decimal?>("LTT_ID") == ltt_id
                                && allowStatus.Contains(o.GetValue<string>("LTT_STATUS")))
                                             select item;
                            var BSet_shbd2 = from item in shbdSet2.Where(o => o.GetValue<decimal?>("LTT_ID") == 0
                                && ASet_shbd2.Count(a => a.G3E_FID == o.G3E_FID) == 0)
                                             select item;
                            var UnionSet_shbd2 = ASet_shbd2.Union(BSet_shbd2);
                            xmldb.InsertBulk(UnionSet_shbd2.Cast<DBEntity>());
                            #endregion
                        }

                        //需要导出导出开关柜的设备
                         if (symbol.Fno == 142 || symbol.Fno == 149 || symbol.Fno == 163)
                        {
                            #region 
                            StringBuilder sb = new StringBuilder(" where g3e_fid in (   select g3e_fid from b$common_n  where g3e_fno=198 and owner1_id in ");
                            sb.Append("(  select  g3e_id from b$common_n where g3e_fid in ( select g3e_fid from " + session_id + " where g3e_fno= " + symbol.Fno + ")) ) ");

                            var shbdSet = oracledb.GetEntities(typeof(Gg_pd_kgg_n), sb.ToString()).Cast<ElectronBase>().ToList();
                            var ASet_shbd = from item in shbdSet.Where(o => o.GetValue<decimal?>("LTT_ID") == ltt_id
                                && allowStatus.Contains(o.GetValue<string>("LTT_STATUS")))
                                            group item by item.G3E_FID into g
                                            select g.OrderBy(o => o.G3E_ID).Last();

                            var BSet_shbd = from item in shbdSet.Where(o => o.GetValue<decimal?>("LTT_ID") == 0
                                && ASet_shbd.Count(a => a.G3E_FID == o.G3E_FID) == 0)
                                            select item;
                            var UnionSet_shbd = ASet_shbd.Union(BSet_shbd);
                            xmldb.InsertBulk(UnionSet_shbd.Cast<DBEntity>());


                            StringBuilder sb1 = new StringBuilder(" where g3e_fid in (   select g3e_fid from b$common_n  where g3e_fno=198 and owner1_id in ");
                            sb1.Append("(  select  g3e_id from b$common_n where g3e_fid in ( select g3e_fid from " + session_id + " where g3e_fno=" + symbol.Fno + ")) )");

                            var shbdSet1 = oracledb.GetEntities(typeof(Gg_pd_kgg_ar_sdogeom), sb1.ToString()).Cast<ElectronBase>().ToList();
                            var ASet_shbd1 = from item in shbdSet1.Where(o => o.GetValue<decimal?>("LTT_ID") == ltt_id
                                && allowStatus.Contains(o.GetValue<string>("LTT_STATUS")))
                                             select item;
                            var BSet_shbd1 = from item in shbdSet1.Where(o => o.GetValue<decimal?>("LTT_ID") == 0
                                && ASet_shbd1.Count(a => a.G3E_FID == o.G3E_FID) == 0)
                                             select item;
                            var UnionSet_shbd1 = ASet_shbd1.Union(BSet_shbd1);
                            xmldb.InsertBulk(UnionSet_shbd1.Cast<DBEntity>());


                            StringBuilder sb2 = new StringBuilder(" where g3e_fid in (   select g3e_fid from b$common_n  where g3e_fno=198 and owner1_id in ");
                            sb2.Append("(  select  g3e_id from b$common_n where g3e_fid in ( select g3e_fid from " + session_id + " where g3e_fno=" + symbol.Fno + ")) )");

                            var shbdSet2 = oracledb.GetEntities(typeof(Gg_pd_kgg_lb_sdogeom), sb2.ToString()).Cast<ElectronBase>().ToList();
                            var ASet_shbd2 = from item in shbdSet2.Where(o => o.GetValue<decimal?>("LTT_ID") == ltt_id
                                && allowStatus.Contains(o.GetValue<string>("LTT_STATUS")))
                                             select item;
                            var BSet_shbd2 = from item in shbdSet2.Where(o => o.GetValue<decimal?>("LTT_ID") == 0
                                && ASet_shbd2.Count(a => a.G3E_FID == o.G3E_FID) == 0)
                                             select item;
                            var UnionSet_shbd2 = ASet_shbd2.Union(BSet_shbd2);
                            xmldb.InsertBulk(UnionSet_shbd2.Cast<DBEntity>());

                            StringBuilder sb3 = new StringBuilder(" where g3e_fid in (   select g3e_fid from b$common_n  where g3e_fno=198 and owner1_id in ");
                            sb3.Append("(  select  g3e_id from b$common_n where g3e_fid in ( select g3e_fid from " + session_id + " where g3e_fno=" + symbol.Fno + ")) )");

                            var shbdSet3 = oracledb.GetEntities(typeof(Common_n), sb3.ToString()).Cast<ElectronBase>().ToList();
                            var ASet_shbd3 = from item in shbdSet3.Where(o => o.GetValue<decimal?>("LTT_ID") == ltt_id
                                && allowStatus.Contains(o.GetValue<string>("LTT_STATUS")))
                                             select item;
                            var BSet_shbd3 = from item in shbdSet3.Where(o => o.GetValue<decimal?>("LTT_ID") == 0
                                && ASet_shbd3.Count(a => a.G3E_FID == o.G3E_FID) == 0)
                                             select item;
                            var UnionSet_shbd3 = ASet_shbd3.Union(BSet_shbd3);
                            xmldb.InsertBulk(UnionSet_shbd3.Cast<DBEntity>());

                            StringBuilder sb4 = new StringBuilder(" where g3e_fid in (   select g3e_fid from b$common_n  where g3e_fno=198 and owner1_id in ");
                            sb4.Append("(  select  g3e_id from b$common_n where g3e_fid in ( select g3e_fid from " + session_id + " where g3e_fno=" + symbol.Fno + ")) )");

                            var shbdSet4 = oracledb.GetEntities(typeof(Gg_pd_gnwzmc_n), sb4.ToString()).Cast<ElectronBase>().ToList();
                            var ASet_shbd4 = from item in shbdSet4.Where(o => o.GetValue<decimal?>("LTT_ID") == ltt_id
                                && allowStatus.Contains(o.GetValue<string>("LTT_STATUS")))
                                             select item;
                            var BSet_shbd4 = from item in shbdSet4.Where(o => o.GetValue<decimal?>("LTT_ID") == 0
                                && ASet_shbd4.Count(a => a.G3E_FID == o.G3E_FID) == 0)
                                             select item;
                            var UnionSet_shbd4 = ASet_shbd4.Union(BSet_shbd4);
                            xmldb.InsertBulk(UnionSet_shbd4.Cast<DBEntity>());
                            #endregion

                        }
                        //159 下面的 低压散户表的详图数据
                         if (symbol.Fno == 160)
                        {
                            #region
                            StringBuilder sb = new StringBuilder(" where g3e_fid in (  select g3e_fid from b$gg_jx_shbd_pt  t where t.g3e_detailid in ( ");
                            sb.Append(" select t.g3e_detailid from b$detailreference_n t where t.g3e_fid in( ");
                            sb.Append(" select g3e_fid from " + session_id + " t where t.g3e_fno=159) )  )");

                            var shbdSet = oracledb.GetEntities(typeof(Gg_jx_shbd_pt), sb.ToString()).Cast<ElectronBase>().ToList();
                            var ASet_shbd = from item in shbdSet.Where(o => o.GetValue<decimal?>("LTT_ID") == ltt_id
                                && allowStatus.Contains(o.GetValue<string>("LTT_STATUS")))
                                            group item by item.G3E_FID into g
                                            select g.OrderBy(o => o.G3E_ID).Last();

                            var BSet_shbd = from item in shbdSet.Where(o => o.GetValue<decimal?>("LTT_ID") == 0
                                && ASet_shbd.Count(a => a.G3E_FID == o.G3E_FID) == 0)
                                            select item;
                            var UnionSet_shbd = ASet_shbd.Union(BSet_shbd);
                            xmldb.InsertBulk(UnionSet_shbd.Cast<DBEntity>());


                            var dyshbSet = oracledb.GetEntities(typeof(Gg_pd_dyshb_n), sb.ToString()).Cast<ElectronBase>().ToList();
                            var ASet_dyshb = from item in dyshbSet.Where(o => o.GetValue<decimal?>("LTT_ID") == ltt_id
                                && allowStatus.Contains(o.GetValue<string>("LTT_STATUS")))
                                             group item by item.G3E_FID into g
                                             select g.OrderBy(o => o.G3E_ID).Last();

                            var BSet_dyshb = from item in dyshbSet.Where(o => o.GetValue<decimal?>("LTT_ID") == 0
                                && ASet_dyshb.Count(a => a.G3E_FID == o.G3E_FID) == 0)
                                             select item;
                            var UnionSet_dyshb = ASet_dyshb.Union(BSet_dyshb);
                            xmldb.InsertBulk(UnionSet_dyshb.Cast<DBEntity>());


                            var dyshbSet1 = oracledb.GetEntities(typeof(Gg_jx_shbd_pt_sdogeom), sb.ToString()).Cast<ElectronBase>().ToList();
                            var ASet_dyshb1 = from item in dyshbSet1.Where(o => o.GetValue<decimal?>("LTT_ID") == ltt_id
                                && allowStatus.Contains(o.GetValue<string>("LTT_STATUS")))
                                              group item by item.G3E_FID into g
                                              select g.OrderBy(o => o.G3E_ID).Last();

                            var BSet_dyshb1 = from item in dyshbSet1.Where(o => o.GetValue<decimal?>("LTT_ID") == 0
                                && ASet_dyshb1.Count(a => a.G3E_FID == o.G3E_FID) == 0)
                                              select item;
                            var UnionSet_dyshb1 = ASet_dyshb1.Union(BSet_dyshb1);
                            xmldb.InsertBulk(UnionSet_dyshb1.Cast<DBEntity>());
                            #endregion
                          
                        }
                        //84和148 下面的 计量表的详图数据
                      else  if (symbol.Fno == 41)
                        {
                            #region
                            StringBuilder sb = new StringBuilder(" where g3e_fid in (  select g3e_fid from b$GG_JX_JLB_PT  t where t.g3e_detailid in ( ");
                            sb.Append(" select t.g3e_detailid from b$detailreference_n t where t.g3e_fid in( ");
                            sb.Append(" select g3e_fid from " + session_id + " t where t.g3e_fno in(148,84) ) ) )");

                            var shbdSet = oracledb.GetEntities(typeof(Gg_jx_jlb_pt), sb.ToString()).Cast<ElectronBase>().ToList();
                            var ASet_shbd = from item in shbdSet.Where(o => o.GetValue<decimal?>("LTT_ID") == ltt_id
                                && allowStatus.Contains(o.GetValue<string>("LTT_STATUS")))
                                            group item by item.G3E_FID into g
                                            select g.OrderBy(o => o.G3E_ID).Last();

                            var BSet_shbd = from item in shbdSet.Where(o => o.GetValue<decimal?>("LTT_ID") == 0
                                && ASet_shbd.Count(a => a.G3E_FID == o.G3E_FID) == 0)
                                            select item;
                            var UnionSet_shbd = ASet_shbd.Union(BSet_shbd);
                            xmldb.InsertBulk(UnionSet_shbd.Cast<DBEntity>());


                            var dyshbSet = oracledb.GetEntities(typeof(Gg_pd_jlb_n), sb.ToString()).Cast<ElectronBase>().ToList();
                            var ASet_dyshb = from item in dyshbSet.Where(o => o.GetValue<decimal?>("LTT_ID") == ltt_id
                                && allowStatus.Contains(o.GetValue<string>("LTT_STATUS")))
                                             group item by item.G3E_FID into g
                                             select g.OrderBy(o => o.G3E_ID).Last();

                            var BSet_dyshb = from item in dyshbSet.Where(o => o.GetValue<decimal?>("LTT_ID") == 0
                                && ASet_dyshb.Count(a => a.G3E_FID == o.G3E_FID) == 0)
                                             select item;
                            var UnionSet_dyshb = ASet_dyshb.Union(BSet_dyshb);
                            xmldb.InsertBulk(UnionSet_dyshb.Cast<DBEntity>());



                            var dyshbSet1 = oracledb.GetEntities(typeof(Common_n), sb.ToString()).Cast<ElectronBase>().ToList();
                            var ASet_dyshb1 = from item in dyshbSet1.Where(o => o.GetValue<decimal?>("LTT_ID") == ltt_id
                                && allowStatus.Contains(o.GetValue<string>("LTT_STATUS")))
                                              group item by item.G3E_FID into g
                                              select g.OrderBy(o => o.G3E_ID).Last();

                            var BSet_dyshb1 = from item in dyshbSet1.Where(o => o.GetValue<decimal?>("LTT_ID") == 0
                                && ASet_dyshb1.Count(a => a.G3E_FID == o.G3E_FID) == 0)
                                              select item;
                            var UnionSet_dyshb1 = ASet_dyshb1.Union(BSet_dyshb1);
                            xmldb.InsertBulk(UnionSet_dyshb1.Cast<DBEntity>());

                            var dyshbSet2 = oracledb.GetEntities(typeof(Gg_jx_jlb_pt_sdogeom), sb.ToString()).Cast<ElectronBase>().ToList();
                            var ASet_dyshb2 = from item in dyshbSet2.Where(o => o.GetValue<decimal?>("LTT_ID") == ltt_id
                                && allowStatus.Contains(o.GetValue<string>("LTT_STATUS")))
                                              group item by item.G3E_FID into g
                                              select g.OrderBy(o => o.G3E_ID).Last();

                            var BSet_dyshb2 = from item in dyshbSet2.Where(o => o.GetValue<decimal?>("LTT_ID") == 0
                                && ASet_dyshb2.Count(a => a.G3E_FID == o.G3E_FID) == 0)
                                              select item;
                            var UnionSet_dyshb2 = ASet_dyshb2.Union(BSet_dyshb2);
                            xmldb.InsertBulk(UnionSet_dyshb2.Cast<DBEntity>());
                            #endregion
                         
                        }
                        else
                        {
                            #region
                            if (symbol.Fno == 198 || symbol.Fno == 41 || symbol.Fno == 199)
                            {
                                continue;
                            }
                            Type type;
                            //加入自身属性 
                            if (!string.IsNullOrEmpty(symbol.SelfAttribute))
                            {
                                type = TypeCache.Instance.GetTypeFromCache(typeof(DBEntity), symbol.SelfAttribute);
                                var selfSet = oracledb.GetEntities(type, " where g3e_fid in (select g3e_fid from " + session_id + " where g3e_fno=" + symbol.Fno + ") ").Cast<ElectronBase>().ToList();
                                var ASet_self = from item in selfSet.Where(o => o.GetValue<decimal?>("LTT_ID") == ltt_id
                                    && allowStatus.Contains(o.GetValue<string>("LTT_STATUS")))
                                                group item by item.G3E_FID into g
                                                select g.OrderBy(o => o.G3E_ID).Last();

                                var BSet_self = from item in selfSet.Where(o => o.GetValue<decimal?>("LTT_ID") == 0
                                    && ASet_self.Count(a => a.G3E_FID == o.G3E_FID) == 0)
                                                select item;
                                var UnionSet_self = ASet_self.Union(BSet_self);
                                xmldb.InsertBulk(UnionSet_self.Cast<DBEntity>());
                            }
                            //加入符号

                            if (!string.IsNullOrEmpty(symbol.PtClassName))
                            {
                                type = TypeCache.Instance.GetTypeFromCache(typeof(DBEntity), symbol.PtClassName);

                                var symbolSet = oracledb.GetEntities(type, " where g3e_fid in (select g3e_fid from " + session_id + " where g3e_fno=" + symbol.Fno + ") ").Cast<ElectronBase>().ToList();

                                var ASet_symbol = from item in symbolSet.Where(o => o.GetValue<decimal?>("LTT_ID") == ltt_id
                                  && allowStatus.Contains(o.GetValue<string>("LTT_STATUS")))
                                                  group item by item.G3E_FID into g
                                                  select g.OrderBy(o => o.G3E_ID).Last();

                                var BSet_symbol = from item in symbolSet.Where(o => o.GetValue<decimal?>("LTT_ID") == 0
                                    && ASet_symbol.Count(a => a.G3E_FID == o.G3E_FID) == 0)
                                                  select item;
                                var UnionSet_symbol = ASet_symbol.Union(BSet_symbol);
                                xmldb.InsertBulk(UnionSet_symbol.Cast<DBEntity>());
                            }



                            //加入标注
                            if (!string.IsNullOrEmpty(symbol.LableClassName))
                            {
                                string[] slable = symbol.LableClassName.Split(',');

                                for (int ii = 0; ii < slable.Length; ii++)
                                {
                                    if (!string.IsNullOrEmpty(slable[ii]))
                                    {
                                        type = TypeCache.Instance.GetTypeFromCache(typeof(DBEntity), slable[ii]);
                                        var lbSet0 = oracledb.GetEntities(type, " where g3e_fid in (select g3e_fid from " + session_id + " where g3e_fno=" + symbol.Fno + ") ").Cast<ElectronBase>().ToList();
                                        var ASet_lb0 = from item in lbSet0.Where(o => o.GetValue<decimal?>("LTT_ID") == ltt_id
                                             && allowStatus.Contains(o.GetValue<string>("LTT_STATUS")))
                                                       group item by item.G3E_FID into g
                                                       select g.OrderBy(o => o.G3E_ID).Last();

                                        var BSet_lb0 = from item in lbSet0.Where(o => o.GetValue<decimal?>("LTT_ID") == 0
                                            && ASet_lb0.Count(a => a.G3E_FID == o.G3E_FID) == 0)
                                                       select item;
                                        var UnionSet_lb0 = ASet_lb0.Union(BSet_lb0);
                                        xmldb.InsertBulk(UnionSet_lb0.Cast<DBEntity>());
                                    }
                                }
                            }

                            //加入符号lb

                            if (!string.IsNullOrEmpty(symbol.PtClassNameLb))
                            {
                                type = TypeCache.Instance.GetTypeFromCache(typeof(DBEntity), symbol.PtClassNameLb);
                                var symbolSet = oracledb.GetEntities(type, " where g3e_fid in (select g3e_fid from " + session_id + " where g3e_fno=" + symbol.Fno + ") ").Cast<ElectronBase>().ToList();

                                var ASet_symbol = from item in symbolSet.Where(o => o.GetValue<decimal?>("LTT_ID") == ltt_id
                                  && allowStatus.Contains(o.GetValue<string>("LTT_STATUS")))
                                                  group item by item.G3E_FID into g
                                                  select g.OrderBy(o => o.G3E_ID).Last();

                                var BSet_symbol = from item in symbolSet.Where(o => o.GetValue<decimal?>("LTT_ID") == 0
                                    && ASet_symbol.Count(a => a.G3E_FID == o.G3E_FID) == 0)
                                                  select item;
                                var UnionSet_symbol = ASet_symbol.Union(BSet_symbol);
                                xmldb.InsertBulk(UnionSet_symbol.Cast<DBEntity>());
                            }

                            //加入标注LB
                            if (!string.IsNullOrEmpty(symbol.LableClassNameLb))
                            {
                                string[] slable = symbol.LableClassNameLb.Split(',');

                                for (int ii = 0; ii < slable.Length; ii++)
                                {
                                    if (!string.IsNullOrEmpty(slable[ii]))
                                    {
                                        type = TypeCache.Instance.GetTypeFromCache(typeof(DBEntity), slable[ii]);
                                        var lbSet0 = oracledb.GetEntities(type, " where g3e_fid in (select g3e_fid from " + session_id + " where g3e_fno=" + symbol.Fno + ") ").Cast<ElectronBase>().ToList();
                                        var ASet_lb0 = from item in lbSet0.Where(o => o.GetValue<decimal?>("LTT_ID") == ltt_id
                                             && allowStatus.Contains(o.GetValue<string>("LTT_STATUS")))
                                                       group item by item.G3E_FID into g
                                                       select g.OrderBy(o => o.G3E_ID).Last();

                                        var BSet_lb0 = from item in lbSet0.Where(o => o.GetValue<decimal?>("LTT_ID") == 0
                                            && ASet_lb0.Count(a => a.G3E_FID == o.G3E_FID) == 0)
                                                       select item;
                                        var UnionSet_lb0 = ASet_lb0.Union(BSet_lb0);
                                        xmldb.InsertBulk(UnionSet_lb0.Cast<DBEntity>());
                                    }
                                }
                            }
                            #endregion
                        }
                    }
                    catch (Exception ex)
                    {
                        CYZLog.writeLog(ex.ToString(), "");
                    }
                    finally
                    {
                        PublicMethod.write_state(ftplj, 0.1 + i * 0.6 / fnocount);
                        i++;
                    }
                }
            }
            catch (Exception ex)
            {
                CYZLog.writeLog(ex.ToString(), "");

            }
            finally
            {
                CYZLog.writeLog(SimpleSymbolConfig.Instance.Symbols.Count.ToString());
            }
        }


        public static void exportHandle2(XmlDBManager xmldb, OracleDBManagerV94 oracledb, string session_id, string ftplj, decimal? ltt_id)
        {
            try
            {
                if (oracledb == null) { return; }
                if (xmldb == null) { return; }

                OracleSequenceValueGenerator.Instance.DbManager = oracledb;

                string symboldir = Path.Combine(Assembly.GetExecutingAssembly().GetPhysicalDirectory(), "SymbolConfig.xml");
                SimpleSymbolConfig.Instance = ElectronTransferFramework.Serialize.XmlSerializeUtils.Load<SimpleSymbolConfig>(symboldir, new Type[] { });

                int i = 0;
                string[] allowStatus = new string[] { "EDIT", "ADD", "DELETE" };
                int fnocount = SimpleSymbolConfig.Instance.Symbols.Count;
                foreach (var symbol in SimpleSymbolConfig.Instance.Symbols)
                {
                    try
                    {
                        if (symbol.Fno == 140 || symbol.Fno == 141 || symbol.Fno == 142 || symbol.Fno == 163)
                        {
                            Type type;
                            if (!string.IsNullOrEmpty(symbol.PtClassName))
                            {
                                type = TypeCache.Instance.GetTypeFromCache(typeof(DBEntity), symbol.PtClassName);
                                var symbolSet = oracledb.GetEntities(type, " where g3e_fid in (select g3e_fid from " + session_id + " where g3e_fno=" + symbol.Fno + ") ").Cast<ElectronBase>().ToList();

                                var ASet_symbol = from item in symbolSet.Where(o => o.GetValue<decimal?>("LTT_ID") == ltt_id
                                  && allowStatus.Contains(o.GetValue<string>("LTT_STATUS")))
                                                  group item by item.G3E_FID into g
                                                  select g.OrderBy(o => o.G3E_ID).Last();

                                var BSet_symbol = from item in symbolSet.Where(o => o.GetValue<decimal?>("LTT_ID") == 0
                                    && ASet_symbol.Count(a => a.G3E_FID == o.G3E_FID) == 0)
                                                  select item;
                                var UnionSet_symbol = ASet_symbol.Union(BSet_symbol);

                                xmldb.InsertBulk_SetKxType(UnionSet_symbol.Cast<DBEntity>());
                            }
                            i++;
                            PublicMethod.write_state(ftplj, 0.81 + i * 0.15 / 4);
                        }
                    }
                    //}
                    catch (Exception ex)
                    {
                        CYZLog.writeLog(ex.ToString(), "");
                    }
                    finally
                    {
                    }

                }
            }
            catch (Exception ex)
            {
                CYZLog.writeLog(ex.ToString(), "");
            }

        }



        public static string dataappendpath = "";
        public static string datapackagepath = "";
        public static string dataftppath = "";
        public static string dataBaseDir = "";
    }
}
