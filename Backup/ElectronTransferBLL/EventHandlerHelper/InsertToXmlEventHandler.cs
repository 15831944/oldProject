using System;
using System.Linq;
using ElectronTransferBll.DBEntityHelper;
using ElectronTransferDal.AutoGeneration;
using ElectronTransferDal.Common;
using ElectronTransferDal.Factory;
using ElectronTransferDal.HelperEventArgs;
using ElectronTransferFramework;
using ElectronTransferModel;
using ElectronTransferModel.Config;
using ElectronTransferModel.V9_4;

namespace ElectronTransferBll.EventHandlerHelper
{
    [ExecutionContainer(typeof(InsertToXmlExecutionFactory))]
    public class InsertToXmlEventHandler
    {
        #region 线

        [Execution(140)]
        public static void Gg_pd_dl_ln_sdogeom(object sender, InsertSymbolEventArgs e)
        {
            var n = new Gg_pd_dl_n
            {
                G3E_CID = 1,
                G3E_FNO = e.g3e_fno,
                G3E_CNO = 14001,
                G3E_FID = e.g3e_fid,
                G3E_ID = e.g3e_id,
                LTT_ID = MapConfig.Instance.LTTID,
                EntityState = EntityState.Insert
            };
            var pt = new Gg_pd_dl_ln_sdogeom
            {
                G3E_CID = 1,
                G3E_FNO = e.g3e_fno,
                G3E_CNO = 14002,
                G3E_FID = e.g3e_fid,
                G3E_ID = e.g3e_id,
                LTT_ID = MapConfig.Instance.LTTID,
                EntityState = EntityState.Insert,
                OriginalG3e_Fid = e.g3e_fid,
                G3E_GEOMETRY = e.lineStringValue
            };

            AddCommon(e);
            AddConnectivity(e);
            AddGnwz(e);

            GenerateHelper.PartialCopyFromCAD(e.symbolObj, n, e.g3e_fno);
            DBManager.Instance.Insert(n);
            DBManager.Instance.Insert(pt);

            e.insertobj = pt;
        }

        [Execution(141)]
        public static void Gg_pd_dx_ln_sdogeom(object sender, InsertSymbolEventArgs e)
        {
            var n = new Gg_pd_dx_n
            {
                G3E_CID = 1,
                G3E_FNO = e.g3e_fno,
                G3E_CNO = 14101,
                G3E_FID = e.g3e_fid,
                G3E_ID = e.g3e_id,
                LTT_ID = MapConfig.Instance.LTTID,
                EntityState = EntityState.Insert
            };
            var pt = new Gg_pd_dx_ln_sdogeom
            {
                G3E_CID = 1,
                G3E_FNO = e.g3e_fno,
                G3E_CNO = 14102,
                G3E_FID = e.g3e_fid,
                G3E_ID = e.g3e_id,
                LTT_ID = MapConfig.Instance.LTTID,
                EntityState = EntityState.Insert,
                OriginalG3e_Fid = e.g3e_fid,
                G3E_GEOMETRY = e.lineStringValue
            };


            AddCommon(e);
            AddConnectivity(e);
            AddGnwz(e);

            //GenerateHelper.PartialCopyFromCAD(e.symbolObj, n, e.g3e_fno);
            GenerateHelper.PartialCopyFromCAD(e.symbolObj, n, e.g3e_fno);
            DBManager.Instance.Insert(n);
            DBManager.Instance.Insert(pt);

            e.insertobj = pt;
        }

        [Execution(143)]
        public static void Gg_pd_zfmx_ln_sdogeom(object sender, InsertSymbolEventArgs e)
        {
            var n = new Gg_pd_zfmx_n
            {
                G3E_CID = 1,
                G3E_FNO = e.g3e_fno,
                G3E_CNO = 14301,
                G3E_FID = e.g3e_fid,
                G3E_ID = e.g3e_id,
                LTT_ID = MapConfig.Instance.LTTID,
                EntityState = EntityState.Insert
            };
            var pt = new Gg_pd_zfmx_ln_sdogeom
            {
                G3E_CID = 1,
                G3E_FNO = e.g3e_fno,
                G3E_CNO = 14302,
                G3E_FID = e.g3e_fid,
                G3E_ID = e.g3e_id,
                LTT_ID = MapConfig.Instance.LTTID,
                EntityState = EntityState.Insert,
                OriginalG3e_Fid = e.g3e_fid,
                G3E_GEOMETRY = e.lineStringValue
            };


            AddCommon(e);
            AddGnwz(e);
            AddConnectivity(e);

            GenerateHelper.PartialCopyFromCAD(e.symbolObj, n, e.g3e_fno);

            DBManager.Instance.Insert(n);
            DBManager.Instance.Insert(pt);

            e.insertobj = pt;
        }

        [Execution(144)]
        public static void Gg_pd_zfyx_ln_sdogeom(object sender, InsertSymbolEventArgs e)
        {
            var n = new Gg_pd_zfyx_n
            {
                G3E_CID = 1,
                G3E_FNO = e.g3e_fno,
                G3E_CNO = 14401,
                G3E_FID = e.g3e_fid,
                G3E_ID = e.g3e_id,
                LTT_ID = MapConfig.Instance.LTTID,
                EntityState = EntityState.Insert
            };
            var pt = new Gg_pd_zfyx_ln_sdogeom
            {
                G3E_CID = 1,
                G3E_FNO = e.g3e_fno,
                G3E_CNO = 14402,
                G3E_FID = e.g3e_fid,
                G3E_ID = e.g3e_id,
                LTT_ID = MapConfig.Instance.LTTID,
                EntityState = EntityState.Insert,
                OriginalG3e_Fid = e.g3e_fid,
                G3E_GEOMETRY = e.lineStringValue
            };


            AddCommon(e);
            AddGnwz(e);
            AddConnectivity(e);

            GenerateHelper.PartialCopyFromCAD(e.symbolObj, n, e.g3e_fno);

            DBManager.Instance.Insert(n);
            DBManager.Instance.Insert(pt);

            e.insertobj = pt;
        }

        [Execution(79)]
        public static void Gg_pd_dymx_ln_sdogeom(object sender, InsertSymbolEventArgs e)
        {
            var n = new Gg_pd_dymx_n
            {
                G3E_CID = 1,
                G3E_FNO = e.g3e_fno,
                G3E_CNO = 7901,
                G3E_FID = e.g3e_fid,
                G3E_ID = e.g3e_id,
                LTT_ID = MapConfig.Instance.LTTID,
                EntityState = EntityState.Insert
            };
            var pt = new Gg_pd_dymx_ln_sdogeom
            {
                G3E_CID = 1,
                G3E_FNO = e.g3e_fno,
                G3E_CNO = 7902,
                G3E_FID = e.g3e_fid,
                G3E_ID = e.g3e_id,
                LTT_ID = MapConfig.Instance.LTTID,
                EntityState = EntityState.Insert,
                OriginalG3e_Fid = e.g3e_fid,
                G3E_GEOMETRY = e.lineStringValue
            };


            AddCommon(e);
            AddConnectivity(e);

            GenerateHelper.PartialCopyFromCAD(e.symbolObj, n, e.g3e_fno);

            DBManager.Instance.Insert(n);
            DBManager.Instance.Insert(pt);

            e.insertobj = pt;
        }

        [Execution(156)]
        public static void Gg_pd_dydx_ln_sdogeom(object sender, InsertSymbolEventArgs e)
        {
            var n = new Gg_pd_dydx_n();
            n.G3E_CID = 1;
            n.G3E_FNO = e.g3e_fno;
            n.G3E_CNO = 15601;
            n.G3E_FID = e.g3e_fid;
            n.G3E_ID = e.g3e_id;
            n.LTT_ID = MapConfig.Instance.LTTID;
            n.EntityState = EntityState.Insert;
            var pt = new Gg_pd_dydx_ln_sdogeom();
            pt.G3E_CID = 1;
            pt.G3E_FNO = e.g3e_fno;
            pt.G3E_CNO = 15602;
            pt.G3E_FID = e.g3e_fid;
            pt.G3E_ID = e.g3e_id;
            pt.LTT_ID = MapConfig.Instance.LTTID;
            pt.EntityState = EntityState.Insert;
            pt.OriginalG3e_Fid = e.g3e_fid; pt.G3E_GEOMETRY = e.lineStringValue;

            AddCommon(e);
            AddConnectivity(e);
            AddGnwz(e);
            GenerateHelper.PartialCopyFromCAD(e.symbolObj, n, e.g3e_fno);

            DBManager.Instance.Insert(n);
            DBManager.Instance.Insert(pt);

            e.insertobj = pt;
        }

        [Execution(157)]
        public static void Gg_pd_dydl_ln_sdogeom(object sender, InsertSymbolEventArgs e)
        {
            var n = new Gg_pd_dydl_n();
            n.G3E_CID = 1;
            n.G3E_FNO = e.g3e_fno;
            n.G3E_CNO = 15701;
            n.G3E_FID = e.g3e_fid;
            n.G3E_ID = e.g3e_id;
            n.LTT_ID = MapConfig.Instance.LTTID;
            n.EntityState = EntityState.Insert;
            var pt = new Gg_pd_dydl_ln_sdogeom();
            pt.G3E_CID = 1;
            pt.G3E_FNO = e.g3e_fno;
            pt.G3E_CNO = 15702;
            pt.G3E_FID = e.g3e_fid;
            pt.G3E_ID = e.g3e_id;
            pt.LTT_ID = MapConfig.Instance.LTTID;
            pt.EntityState = EntityState.Insert;
            pt.OriginalG3e_Fid = e.g3e_fid; pt.G3E_GEOMETRY = e.lineStringValue;

            AddCommon(e);
            AddConnectivity(e);
            AddGnwz(e);

            GenerateHelper.PartialCopyFromCAD(e.symbolObj, n, e.g3e_fno);
            DBManager.Instance.Insert(n);
            DBManager.Instance.Insert(pt);

            e.insertobj = pt;
        }

        [Execution(80)]
        public static void Gg_pd_bqx_ln_sdogeom(object sender, InsertSymbolEventArgs e)
        {
            var n = new Gg_pd_bqx_n();
            n.G3E_CID = 1;
            n.G3E_FNO = e.g3e_fno;
            n.G3E_CNO = 8001;
            n.G3E_FID = e.g3e_fid;
            n.G3E_ID = e.g3e_id;
            n.LTT_ID = MapConfig.Instance.LTTID;
            n.EntityState = EntityState.Insert;
            var pt = new Gg_pd_bqx_ln_sdogeom();
            pt.G3E_CID = 1;
            pt.G3E_FNO = e.g3e_fno;
            pt.G3E_CNO = 8002;
            pt.G3E_FID = e.g3e_fid;
            pt.G3E_ID = e.g3e_id;
            pt.LTT_ID = MapConfig.Instance.LTTID;
            pt.EntityState = EntityState.Insert;
            pt.OriginalG3e_Fid = e.g3e_fid; pt.G3E_GEOMETRY = e.lineStringValue;


            AddCommon(e);
            AddConnectivity(e);


            //ReflectionUtils.PartialCopy(e.symbolObj, n);
            GenerateHelper.PartialCopyFromCAD(e.symbolObj, n, e.g3e_fno);

            DBManager.Instance.Insert(n);
            DBManager.Instance.Insert(pt);

            e.insertobj = pt;
        }

        [Execution(75)]
        public static void Gg_pd_bdcx_pt_sdogeom(object sender, InsertSymbolEventArgs e)
        {
            var n = new Gg_pd_bdcx_n();
            n.G3E_CID = 1;
            n.G3E_FNO = e.g3e_fno;
            n.G3E_CNO = 7501;
            n.G3E_FID = e.g3e_fid;
            n.G3E_ID = e.g3e_id;
            n.LTT_ID = MapConfig.Instance.LTTID;
            n.EntityState = EntityState.Insert;
            var pt = new Gg_pd_bdcx_pt_sdogeom();
            pt.G3E_CID = 1;
            pt.G3E_FNO = e.g3e_fno;
            pt.G3E_CNO = 7502;
            pt.G3E_FID = e.g3e_fid;
            pt.G3E_ID = e.g3e_id;
            pt.LTT_ID = MapConfig.Instance.LTTID;
            pt.EntityState = EntityState.Insert;
            pt.OriginalG3e_Fid = e.g3e_fid; pt.G3E_GEOMETRY = e.lineStringValue;


            AddGnwz(e);
            AddCommon(e);
            AddConnectivity(e);

            GenerateHelper.PartialCopyFromCAD(e.symbolObj, n, e.g3e_fno);

            DBManager.Instance.Insert(n);
            DBManager.Instance.Insert(pt);

            e.insertobj = pt;
        }

        #endregion

        #region 面

        [Execution(77)]
        public static void Gg_pd_dlfxx_ar_sdogeom(object sender, InsertSymbolEventArgs e)
        {
            Gg_pd_dlfxx_n n = new Gg_pd_dlfxx_n();
            n.G3E_CID = 1;
            n.G3E_FNO = e.g3e_fno;
            n.G3E_CNO = 7701;
            n.G3E_FID = e.g3e_fid;
            n.G3E_ID = e.g3e_id;
            n.LTT_ID = MapConfig.Instance.LTTID;
            n.EntityState = EntityState.Insert;
            Gg_pd_dlfxx_ar_sdogeom pt = new Gg_pd_dlfxx_ar_sdogeom();
            pt.G3E_CID = 1;
            pt.G3E_FNO = e.g3e_fno;
            pt.G3E_CNO = 7702;
            pt.G3E_FID = e.g3e_fid;
            pt.G3E_ID = e.g3e_id;
            pt.LTT_ID = MapConfig.Instance.LTTID;
            pt.EntityState = EntityState.Insert;
            pt.OriginalG3e_Fid = e.g3e_fid; pt.G3E_GEOMETRY = e.polygonPointValue;


            AddCommon(e);
            //AddConnectivity(e,entry);
            AddGnwz(e);

            //ReflectionUtils.PartialCopy(e.symbolObj, n);
            GenerateHelper.PartialCopyFromCAD(e.symbolObj, n, e.g3e_fno);

            DBManager.Instance.Insert(n);
            DBManager.Instance.Insert(pt);


            e.insertobj = pt;
        }

        [Execution(81)]
        public static void Gg_pd_dyg_ar_sdogeom(object sender, InsertSymbolEventArgs e)
        {
            var n = new Gg_pd_dyg_n
            {
                G3E_CID = 1,
                G3E_FNO = e.g3e_fno,
                G3E_CNO = 8101,
                G3E_FID = e.g3e_fid,
                G3E_ID = e.g3e_id,
                LTT_ID = MapConfig.Instance.LTTID,
                EntityState = EntityState.Insert
            };
            var pt = new Gg_pd_dyg_ar_sdogeom
            {
                G3E_CID = 1,
                G3E_FNO = e.g3e_fno,
                G3E_CNO = 8102,
                G3E_FID = e.g3e_fid,
                G3E_ID = e.g3e_id,
                LTT_ID = MapConfig.Instance.LTTID,
                EntityState = EntityState.Insert,
                OriginalG3e_Fid = e.g3e_fid,
                G3E_GEOMETRY = e.polygonPointValue
            };


            AddCommon(e);
            //AddConnectivity(e,entry);
            AddGnwz(e);
            //ReflectionUtils.PartialCopy(e.symbolObj, n);
            GenerateHelper.PartialCopyFromCAD(e.symbolObj, n, e.g3e_fno);
            DBManager.Instance.Insert(n);
            DBManager.Instance.Insert(pt);

            e.insertobj = pt;
        }

        [Execution(142)]
        public static void Gg_pd_gydf_ar_sdogeom(object sender, InsertSymbolEventArgs e)
        {
            Gg_pd_gydf_n n = new Gg_pd_gydf_n();
            n.G3E_CID = 1;
            n.G3E_FNO = e.g3e_fno;
            n.G3E_CNO = 14201;
            n.G3E_FID = e.g3e_fid;
            n.G3E_ID = e.g3e_id;
            n.LTT_ID = MapConfig.Instance.LTTID;
            n.EntityState = EntityState.Insert;
            Gg_pd_gydf_ar_sdogeom pt = new Gg_pd_gydf_ar_sdogeom();
            pt.G3E_CID = 1;
            pt.G3E_FNO = e.g3e_fno;
            pt.G3E_CNO = 14202;
            pt.G3E_FID = e.g3e_fid;
            pt.G3E_ID = e.g3e_id;
            pt.LTT_ID = MapConfig.Instance.LTTID;
            pt.EntityState = EntityState.Insert;
            pt.OriginalG3e_Fid = e.g3e_fid; pt.G3E_GEOMETRY = e.polygonPointValue;


            AddCommon(e);
            //AddConnectivity(e,entry);
            AddGnwz(e);

            //ReflectionUtils.PartialCopy(e.symbolObj, n);
            GenerateHelper.PartialCopyFromCAD(e.symbolObj, n, e.g3e_fno);
            DBManager.Instance.Insert(n);
            DBManager.Instance.Insert(pt);

            e.insertobj = pt;
        }

        [Execution(149)]
        public static void Gg_pd_xssb_ar_sdogeom(object sender, InsertSymbolEventArgs e)
        {
            Gg_pd_xssb_n n = new Gg_pd_xssb_n();
            n.G3E_CID = 1;
            n.G3E_FNO = e.g3e_fno;
            n.G3E_CNO = 14901;
            n.G3E_FID = e.g3e_fid;
            n.G3E_ID = e.g3e_id;
            n.LTT_ID = MapConfig.Instance.LTTID;
            n.EntityState = EntityState.Insert;
            Gg_pd_xssb_ar_sdogeom pt = new Gg_pd_xssb_ar_sdogeom();
            pt.G3E_CID = 1;
            pt.G3E_FNO = e.g3e_fno;
            pt.G3E_CNO = 14902;
            pt.G3E_FID = e.g3e_fid;
            pt.G3E_ID = e.g3e_id;
            pt.LTT_ID = MapConfig.Instance.LTTID;
            pt.EntityState = EntityState.Insert;
            pt.OriginalG3e_Fid = e.g3e_fid; pt.G3E_GEOMETRY = e.polygonPointValue;


            AddCommon(e);
            //AddConnectivity(e,entry);
            AddGnwz(e);

            //ReflectionUtils.PartialCopy(e.symbolObj, n);
            GenerateHelper.PartialCopyFromCAD(e.symbolObj, n, e.g3e_fno);
            DBManager.Instance.Insert(n);
            DBManager.Instance.Insert(pt);

            e.insertobj = pt;
        }

        [Execution(163)]
        public static void Gg_pd_zydf_ar_sdogeom(object sender, InsertSymbolEventArgs e)
        {
            Gg_pd_zydf_n n = new Gg_pd_zydf_n();
            n.G3E_CID = 1;
            n.G3E_FNO = e.g3e_fno;
            n.G3E_CNO = 16301;
            n.G3E_FID = e.g3e_fid;
            n.G3E_ID = e.g3e_id;
            n.LTT_ID = MapConfig.Instance.LTTID;
            n.EntityState = EntityState.Insert;
            Gg_pd_zydf_ar_sdogeom pt = new Gg_pd_zydf_ar_sdogeom();
            pt.G3E_CID = 1;
            pt.G3E_FNO = e.g3e_fno;
            pt.G3E_CNO = 16302;
            pt.G3E_FID = e.g3e_fid;
            pt.G3E_ID = e.g3e_id;
            pt.LTT_ID = MapConfig.Instance.LTTID;
            pt.EntityState = EntityState.Insert;
            pt.OriginalG3e_Fid = e.g3e_fid; pt.G3E_GEOMETRY = e.polygonPointValue;


            AddCommon(e);
            //AddConnectivity(e,entry);
            AddGnwz(e);

            //ReflectionUtils.PartialCopy(e.symbolObj, n);
            GenerateHelper.PartialCopyFromCAD(e.symbolObj, n, e.g3e_fno);
            DBManager.Instance.Insert(n);
            DBManager.Instance.Insert(pt);

            e.insertobj = pt;
        }

        [Execution(302)]
        public static void Gg_pd_dypdf_ar_sdogeom(object sender, InsertSymbolEventArgs e)
        {
            Gg_pd_dypdf_n n = new Gg_pd_dypdf_n();
            n.G3E_CID = 1;
            n.G3E_FNO = e.g3e_fno;
            n.G3E_CNO = 30201;
            n.G3E_FID = e.g3e_fid;
            n.G3E_ID = e.g3e_id;
            n.LTT_ID = MapConfig.Instance.LTTID;
            n.EntityState = EntityState.Insert;
            Gg_pd_dypdf_ar_sdogeom pt = new Gg_pd_dypdf_ar_sdogeom();
            pt.G3E_CID = 1;
            pt.G3E_FNO = e.g3e_fno;
            pt.G3E_CNO = 30202;
            pt.G3E_FID = e.g3e_fid;
            pt.G3E_ID = e.g3e_id;
            pt.LTT_ID = MapConfig.Instance.LTTID;
            pt.EntityState = EntityState.Insert;
            pt.OriginalG3e_Fid = e.g3e_fid; pt.G3E_GEOMETRY = e.polygonPointValue;


            AddCommon(e);
            //AddConnectivity(e,entry);
            AddGnwz(e);

            //ReflectionUtils.PartialCopy(e.symbolObj, n);
            GenerateHelper.PartialCopyFromCAD(e.symbolObj, n, e.g3e_fno);
            DBManager.Instance.Insert(n);
            DBManager.Instance.Insert(pt);

            e.insertobj = pt;
        }

        [Execution(198)]
        public static void Gg_pd_kgg_ar_sdogeom(object sender, InsertSymbolEventArgs e)
        {
            Gg_pd_kgg_n n = new Gg_pd_kgg_n();
            n.G3E_CID = 1;
            n.G3E_FNO = e.g3e_fno;
            n.G3E_CNO = 19801;
            n.G3E_FID = e.g3e_fid;
            n.G3E_ID = e.g3e_id;
            n.LTT_ID = MapConfig.Instance.LTTID;
            n.EntityState = EntityState.Insert;

            Common_n cn = new Common_n();
            cn.G3E_CID = 1;
            cn.G3E_FNO = e.g3e_fno;
            cn.G3E_ID = e.g3e_id;
            cn.G3E_CNO = 30;
            cn.G3E_FID = e.g3e_fid;
            cn.LTT_ID = MapConfig.Instance.LTTID;
            cn.OWNER1_ID = GenerateHelper.Unrf.kggG3EID;
            cn.EntityState = EntityState.Insert;
            GenerateHelper.PartialCopyFromCAD(e.symbolObj, cn, e.g3e_fno);
            DBManager.Instance.Insert(cn);

            GenerateHelper.PartialCopyFromCAD(e.symbolObj, n, e.g3e_fno);
            e.SBMC = cn.SBMC;//这句不能注释


            DBManager.Instance.Insert(n);
            //AddGnwz(e, entry);
            #region
            var gn = new Gg_pd_gnwzmc_n();
            gn.G3E_CID = 1;
            gn.G3E_FNO = e.g3e_fno;
            gn.G3E_CNO = 50;
            gn.G3E_FID = e.g3e_fid;
            gn.G3E_ID = e.g3e_id;
            gn.MC = cn.SBMC;
            gn.LTT_ID = MapConfig.Instance.LTTID;
            gn.EntityState = EntityState.Insert;
            GenerateHelper.PartialCopyFromCAD(e.symbolObj, gn, e.g3e_fno);
            DBManager.Instance.Insert(gn);

            #endregion
        }

        [Execution(199)]
        public static void Gg_gz_tj_ar_sdogeom(object sender, InsertSymbolEventArgs e)
        {
            Gg_gz_tj_n n = new Gg_gz_tj_n();
            n.G3E_CID = 1;
            n.G3E_FNO = e.g3e_fno;
            n.G3E_CNO = 19901;
            n.G3E_FID = e.g3e_fid;
            n.G3E_ID = e.g3e_id;
            n.GNWZ_FID = e.parentFid.ToString();
            n.LTT_ID = MapConfig.Instance.LTTID;
            n.EntityState = EntityState.Insert;

            //var com= AddCommon(e,entry);

            #region

            Common_n cn = new Common_n();
            cn.G3E_CID = 1;
            cn.G3E_FNO = e.g3e_fno;
            cn.G3E_ID = e.g3e_id;
            cn.G3E_CNO = 30;
            cn.G3E_FID = e.g3e_fid;
            cn.LTT_ID = MapConfig.Instance.LTTID;
            cn.EntityState = EntityState.Insert;
            GenerateHelper.PartialCopyFromCAD(e.symbolObj, cn, e.g3e_fno);
            DBManager.Instance.Insert(cn);

            #endregion

            //AddConnectivity(e,entry);
            //AddGnwz(e,entry);

            #region

            Gg_pd_gnwzmc_n gn = new Gg_pd_gnwzmc_n();
            gn.G3E_CID = 1;
            gn.G3E_FNO = e.g3e_fno;
            gn.G3E_CNO = 50;
            gn.G3E_FID = e.g3e_fid;
            gn.G3E_ID = e.g3e_id;
            gn.LTT_ID = MapConfig.Instance.LTTID;
            gn.EntityState = EntityState.Insert;
            GenerateHelper.PartialCopyFromCAD(e.symbolObj, gn, e.g3e_fno);
            gn.GNWZ_SSTJ = cn.SBMC;
            DBManager.Instance.Insert(gn);

            #endregion

            //ReflectionUtils.PartialCopy(e.symbolObj, n);
            GenerateHelper.PartialCopyFromCAD(e.symbolObj, n, e.g3e_fno);
            DBManager.Instance.Insert(n);
            e.SBMC = cn.SBMC;
        }

        #endregion

        #region 点

        [Execution(40)]
        public static void Gg_pd_dlhgq_pt_sdogeom(object sender, InsertSymbolEventArgs e)
        {
            var n = new Gg_pd_dlhgq_n();
            n.G3E_CID = 1;
            n.G3E_FNO = e.g3e_fno;
            n.G3E_CNO = 4001;
            n.G3E_FID = e.g3e_fid;
            n.G3E_ID = e.g3e_id;
            n.LTT_ID = MapConfig.Instance.LTTID;
            n.EntityState = EntityState.Insert;
            var pt = new Gg_pd_dlhgq_pt_sdogeom();
            pt.G3E_CID = 1;
            pt.G3E_FNO = e.g3e_fno;
            pt.G3E_CNO = 4002;
            pt.G3E_FID = e.g3e_fid;
            pt.G3E_ID = e.g3e_id;
            pt.LTT_ID = MapConfig.Instance.LTTID;
            pt.EntityState = EntityState.Insert;
            pt.OriginalG3e_Fid = e.g3e_fid; pt.G3E_GEOMETRY = e.multipointValue;
            pt.BlockName = e.blockName;

            AddCommon(e);
            AddConnectivity(e);
            AddGnwz(e);
            GenerateHelper.PartialCopyFromCAD(e.symbolObj, n, e.g3e_fno);

            DBManager.Instance.Insert(n);
            DBManager.Instance.Insert(pt);
            e.insertobj = pt;
        }

        [Execution(82)]
        public static void Gg_jx_gyb_pt_sdogeom(object sender, InsertSymbolEventArgs e)
        {
            var n = new Gg_pd_gyb_n
            {
                G3E_CID = 1,
                G3E_FNO = e.g3e_fno,
                G3E_CNO = 8201,
                G3E_FID = e.g3e_fid,
                G3E_ID = e.g3e_id,
                LTT_ID = MapConfig.Instance.LTTID,
                EntityState = EntityState.Insert
            };
            DBManager.Instance.Insert(n);

            var pt = new Gg_pd_gyb_pt_sdogeom
            {
                G3E_CID = 1,
                G3E_FNO = e.g3e_fno,
                G3E_CNO = 8206,
                G3E_FID = e.g3e_fid,
                G3E_ID = e.g3e_id,
                LTT_ID = MapConfig.Instance.LTTID,
                EntityState = EntityState.Insert,
                BlockName = e.blockName,
                OriginalG3e_Fid = e.g3e_fid,
                G3E_GEOMETRY = e.multipointValue
            };
            DBManager.Instance.Insert(pt);


            AddCommon(e);
            AddGnwz(e);
            AddConnectivity(e);
            e.insertobj = pt;
        }

        [Execution(84)]
        public static void Gg_pd_cjlg_pt_sdogeom(object sender, InsertSymbolEventArgs e)
        {
            var n = new Gg_pd_cjlg_n
            {
                G3E_CID = 1,
                G3E_FNO = e.g3e_fno,
                G3E_CNO = 8401,
                G3E_FID = e.g3e_fid,
                G3E_ID = e.g3e_id,
                LTT_ID = MapConfig.Instance.LTTID,
                EntityState = EntityState.Insert
            };
            var pt = new Gg_pd_cjlg_pt_sdogeom
            {
                G3E_CID = 1,
                G3E_FNO = e.g3e_fno,
                G3E_CNO = 8402,
                G3E_FID = e.g3e_fid,
                G3E_ID = e.g3e_id,
                LTT_ID = MapConfig.Instance.LTTID,
                EntityState = EntityState.Insert,
                OriginalG3e_Fid = e.g3e_fid,
                G3E_GEOMETRY = e.multipointValue,
                BlockName = e.blockName
            };


            AddCommon(e);
            AddGnwz(e);
            AddConnectivity(e);

            GenerateHelper.PartialCopyFromCAD(e.symbolObj, n, e.g3e_fno);

            DBManager.Instance.Insert(n);
            DBManager.Instance.Insert(pt);

            var df = new Detailreference_n
            {
                G3E_CID = 1,
                G3E_ID = e.g3e_id,
                G3E_CNO = 35,
                G3E_FID = e.g3e_fid,
                G3E_FNO = e.g3e_fno,
                LTT_ID = MapConfig.Instance.LTTID,
                EntityState = EntityState.Insert,
                G3E_DETAILID = CYZCommonFunc.getid(),
                DETAIL_USERNAME = e.g3e_fid.ToString(),
                DETAIL_LEGENDNUMBER = 35,
                DETAIL_MBRXLO = Convert.ToDecimal(112.356172100576),
                DETAIL_MBRYLO = Convert.ToDecimal(21.9600071382762),
                DETAIL_MBRXHIGH = Convert.ToDecimal(112.356316442584),
                DETAIL_MBRYHIGH = Convert.ToDecimal(21.9601062562456),
                DETAIL_MBRXOFFSET = 50000,
                DETAIL_MBRYOFFSET = 50000
            };

            DBManager.Instance.Insert(df);

            e.insertobj = pt;
        }

        [Execution(85)]
        public static void Gg_pd_ptg_pt_sdogeom(object sender, InsertSymbolEventArgs e)
        {
            var n = new Gg_pd_ptg_n();
            n.G3E_CID = 1;
            n.G3E_FNO = e.g3e_fno;
            n.G3E_CNO = 8501;
            n.G3E_FID = e.g3e_fid;
            n.G3E_ID = e.g3e_id;
            n.LTT_ID = MapConfig.Instance.LTTID;
            n.EntityState = EntityState.Insert;
            var pt = new Gg_pd_ptg_pt_sdogeom
            {
                G3E_CID = 1,
                G3E_FNO = e.g3e_fno,
                G3E_CNO = 8502,
                G3E_FID = e.g3e_fid,
                G3E_ID = e.g3e_id,
                LTT_ID = MapConfig.Instance.LTTID,
                EntityState = EntityState.Insert,
                OriginalG3e_Fid = e.g3e_fid,
                G3E_GEOMETRY = e.multipointValue,
                BlockName = e.blockName
            };


            AddCommon(e);
            AddGnwz(e);
            AddConnectivity(e);

            GenerateHelper.PartialCopyFromCAD(e.symbolObj, n, e.g3e_fno);

            DBManager.Instance.Insert(n);
            DBManager.Instance.Insert(pt);

            e.insertobj = pt;
        }


        [Execution(90)]
        public static void Gg_jc_dtu_pt_sdogeom(object sender, InsertSymbolEventArgs e)
        {
            var n = new Gg_jc_dtu_n();
            n.G3E_CID = 1;
            n.G3E_FNO = e.g3e_fno;
            n.G3E_CNO = 9001;
            n.G3E_FID = e.g3e_fid;
            n.G3E_ID = e.g3e_id;
            n.LTT_ID = MapConfig.Instance.LTTID;
            n.EntityState = EntityState.Insert;
            var pt = new Gg_jc_dtu_pt_sdogeom
            {
                G3E_CID = 1,
                G3E_FNO = e.g3e_fno,
                G3E_CNO = 9002,
                G3E_FID = e.g3e_fid,
                G3E_ID = e.g3e_id,
                LTT_ID = MapConfig.Instance.LTTID,
                EntityState = EntityState.Insert,
                OriginalG3e_Fid = e.g3e_fid,
                G3E_GEOMETRY = e.multipointValue,
                BlockName = e.blockName
            };


            AddCommon(e);
            AddGnwz(e);

            GenerateHelper.PartialCopyFromCAD(e.symbolObj, n, e.g3e_fno);

            DBManager.Instance.Insert(n);
            DBManager.Instance.Insert(pt);

            e.insertobj = pt;
        }

        [Execution(146)]
        public static void Gg_pd_zfhwg_pt_sdogeom(object sender, InsertSymbolEventArgs e)
        {
            var n = new Gg_pd_zfhwg_n
            {
                G3E_CID = 1,
                G3E_FNO = e.g3e_fno,
                G3E_CNO = 14601,
                G3E_FID = e.g3e_fid,
                G3E_ID = e.g3e_id,
                LTT_ID = MapConfig.Instance.LTTID,
                EntityState = EntityState.Insert
            };
            var pt = new Gg_pd_zfhwg_pt_sdogeom();
            pt.G3E_CID = 1;
            pt.G3E_FNO = e.g3e_fno;
            pt.G3E_CNO = 14602;
            pt.G3E_FID = e.g3e_fid;
            pt.G3E_ID = e.g3e_id;
            pt.LTT_ID = MapConfig.Instance.LTTID;
            pt.EntityState = EntityState.Insert;
            pt.OriginalG3e_Fid = e.g3e_fid; pt.G3E_GEOMETRY = e.multipointValue;

            pt.BlockName = e.blockName;

            AddCommon(e);

            AddGnwz(e);
            AddConnectivity(e);

            GenerateHelper.PartialCopyFromCAD(e.symbolObj, n, e.g3e_fno);

            DBManager.Instance.Insert(n);
            DBManager.Instance.Insert(pt);

            e.insertobj = pt;
        }

        [Execution(147)]
        public static void Gg_pd_zfdlt_pt_sdogeom(object sender, InsertSymbolEventArgs e)
        {
            var n = new Gg_pd_zfdlt_n
            {
                G3E_CID = 1,
                G3E_FNO = e.g3e_fno,
                G3E_CNO = 14701,
                G3E_FID = e.g3e_fid,
                G3E_ID = e.g3e_id,
                LTT_ID = MapConfig.Instance.LTTID,
                EntityState = EntityState.Insert
            };

            var pt = new Gg_pd_zfdlt_pt_sdogeom
            {
                G3E_CID = 1,
                G3E_FNO = e.g3e_fno,
                G3E_CNO = 14702,
                G3E_FID = e.g3e_fid,
                G3E_ID = e.g3e_id,
                LTT_ID = MapConfig.Instance.LTTID,
                EntityState = EntityState.Insert,
                OriginalG3e_Fid = e.g3e_fid
            };
            pt.OriginalG3e_Fid = e.g3e_fid; pt.G3E_GEOMETRY = e.multipointValue;

            pt.BlockName = e.blockName;

            AddCommon(e);
            AddGnwz(e);

            AddConnectivity(e);

            GenerateHelper.PartialCopyFromCAD(e.symbolObj, n, e.g3e_fno);
            DBManager.Instance.Insert(n);
            DBManager.Instance.Insert(pt);

            e.insertobj = pt;
        }

        [Execution(148)]
        public static void Gg_pd_zfbyq_pt_sdogeom(object sender, InsertSymbolEventArgs e)
        {

            AddCommon(e);
            AddGnwz(e);

            AddConnectivity(e);

            var n = new Gg_pd_zfbyq_n
            {
                G3E_CID = 1,
                G3E_FNO = e.g3e_fno,
                G3E_CNO = 14801,
                G3E_FID = e.g3e_fid,
                G3E_ID = e.g3e_id,
                LTT_ID = MapConfig.Instance.LTTID,
                EntityState = EntityState.Insert
            };

            var pt = new Gg_pd_zfbyq_pt_sdogeom
            {
                G3E_CID = 1,
                G3E_FNO = e.g3e_fno,
                G3E_CNO = 14802,
                G3E_FID = e.g3e_fid,
                G3E_ID = e.g3e_id,
                LTT_ID = MapConfig.Instance.LTTID,
                EntityState = EntityState.Insert,
                OriginalG3e_Fid = e.g3e_fid,
                G3E_GEOMETRY = e.multipointValue,
                BlockName = e.blockName
            };

            GenerateHelper.PartialCopyFromCAD(e.symbolObj, n, e.g3e_fno);
            DBManager.Instance.Insert(n);
            DBManager.Instance.Insert(pt);

            var df = new Detailreference_n
            {
                G3E_CID = 1,
                G3E_ID = e.g3e_id,
                G3E_CNO = 35,
                G3E_FID = e.g3e_fid,
                G3E_FNO = e.g3e_fno,
                LTT_ID = MapConfig.Instance.LTTID,
                EntityState = EntityState.Insert,
                G3E_DETAILID = CYZCommonFunc.getid(),
                DETAIL_USERNAME = e.g3e_fid.ToString(),
                DETAIL_LEGENDNUMBER = 34,
                DETAIL_MBRXLO = Convert.ToDecimal(112.983447102654),
                DETAIL_MBRYLO = Convert.ToDecimal(22.498385930756),
                DETAIL_MBRXHIGH = Convert.ToDecimal(112.983583232819),
                DETAIL_MBRYHIGH = Convert.ToDecimal(22.4985193187308),
                DETAIL_MBRXOFFSET = 50000,
                DETAIL_MBRYOFFSET = 50000
            };
            DBManager.Instance.Insert(df);
            e.insertobj = pt;
        }



        [Execution(150)]
        public static void Gg_pd_dlzjjt_pt_sdogeom(object sender, InsertSymbolEventArgs e)
        {

            AddCommon(e);
            AddGnwz(e);
            AddConnectivity(e);
            var n = new Gg_pd_dlzjjt_n
            {
                G3E_CID = 1,
                G3E_FNO = e.g3e_fno,
                G3E_CNO = 15001,
                G3E_FID = e.g3e_fid,
                G3E_ID = e.g3e_id,
                LTT_ID = MapConfig.Instance.LTTID,
                EntityState = EntityState.Insert
            };
            var pt = new Gg_pd_dlzjjt_pt_sdogeom
            {
                G3E_CID = 1,
                G3E_FNO = e.g3e_fno,
                G3E_CNO = 15002,
                G3E_FID = e.g3e_fid,
                G3E_ID = e.g3e_id,
                LTT_ID = MapConfig.Instance.LTTID,
                EntityState = EntityState.Insert,
                OriginalG3e_Fid = e.g3e_fid,
                G3E_GEOMETRY = e.multipointValue,
                BlockName = e.blockName
            };


            GenerateHelper.PartialCopyFromCAD(e.symbolObj, n, e.g3e_fno);

            DBManager.Instance.Insert(n);
            DBManager.Instance.Insert(pt);
            e.insertobj = pt;
        }

        [Execution(151)]
        public static void Gg_pd_dlzdt_pt_sdogeom(object sender, InsertSymbolEventArgs e)
        {
            var n = new Gg_pd_dlzdt_n
            {
                G3E_CID = 1,
                G3E_FNO = e.g3e_fno,
                G3E_CNO = 15101,
                G3E_FID = e.g3e_fid,
                G3E_ID = e.g3e_id,
                LTT_ID = MapConfig.Instance.LTTID,
                EntityState = EntityState.Insert
            };
            var pt = new Gg_pd_dlzdt_pt_sdogeom
            {
                G3E_CID = 1,
                G3E_FNO = e.g3e_fno,
                G3E_CNO = 15102,
                G3E_FID = e.g3e_fid,
                G3E_ID = e.g3e_id,
                LTT_ID = MapConfig.Instance.LTTID,
                EntityState = EntityState.Insert,
                OriginalG3e_Fid = e.g3e_fid,
                G3E_GEOMETRY = e.multipointValue,
                BlockName = e.blockName
            };


            AddCommon(e);
            AddGnwz(e);
            AddConnectivity(e);

            GenerateHelper.PartialCopyFromCAD(e.symbolObj, n, e.g3e_fno);

            DBManager.Instance.Insert(n);
            DBManager.Instance.Insert(pt);

            e.insertobj = pt;
        }

        [Execution(171)]
        public static void Gg_pd_zdfdj_pt_sdogeom(object sender, InsertSymbolEventArgs e)
        {

            AddCommon(e);
            AddConnectivity1(e);
            var n = new Gg_pd_zdfdj_n
            {
                G3E_CID = 1,
                G3E_FNO = e.g3e_fno,
                G3E_CNO = 17101,
                G3E_FID = e.g3e_fid,
                G3E_ID = e.g3e_id,
                LTT_ID = MapConfig.Instance.LTTID,
                EntityState = EntityState.Insert
            };
            var pt = new Gg_pd_zdfdj_pt_sdogeom
            {
                G3E_CID = 1,
                G3E_FNO = e.g3e_fno,
                G3E_CNO = 17102,
                G3E_FID = e.g3e_fid,
                G3E_ID = e.g3e_id,
                LTT_ID = MapConfig.Instance.LTTID,
                EntityState = EntityState.Insert,
                OriginalG3e_Fid = e.g3e_fid,
                G3E_GEOMETRY = e.multipointValue,
                BlockName = e.blockName
            };


            GenerateHelper.PartialCopyFromCAD(e.symbolObj, n, e.g3e_fno);

            DBManager.Instance.Insert(n);
            DBManager.Instance.Insert(pt);

            e.insertobj = pt;
        }

        [Execution(172)]
        public static void Gg_pd_ddj_pt_sdogeom(object sender, InsertSymbolEventArgs e)
        {

            AddCommon(e);
            AddConnectivity(e);
            var n = new Gg_pd_ddj_n
            {
                G3E_CID = 1,
                G3E_FNO = e.g3e_fno,
                G3E_CNO = 30304,
                G3E_FID = e.g3e_fid,
                G3E_ID = e.g3e_id,
                LTT_ID = MapConfig.Instance.LTTID,
                EntityState = EntityState.Insert
            };
            var pt = new Gg_pd_ddj_pt_sdogeom
            {
                G3E_CID = 1,
                G3E_FNO = e.g3e_fno,
                G3E_CNO = 17201,
                G3E_FID = e.g3e_fid,
                G3E_ID = e.g3e_id,
                LTT_ID = MapConfig.Instance.LTTID,
                EntityState = EntityState.Insert,
                OriginalG3e_Fid = e.g3e_fid,
                G3E_GEOMETRY = e.multipointValue,
                BlockName = e.blockName
            };

            GenerateHelper.PartialCopyFromCAD(e.symbolObj, n, e.g3e_fno);

            DBManager.Instance.Insert(n);
            DBManager.Instance.Insert(pt);

            e.insertobj = pt;
        }

        [Execution(173)]
        public static void Gg_pd_zfjddz_pt_sdogeom(object sender, InsertSymbolEventArgs e)
        {
            var n = new Gg_pd_zfjddz_n();
            n.G3E_CID = 1;
            n.G3E_FNO = e.g3e_fno;
            n.G3E_CNO = 17301;
            n.G3E_FID = e.g3e_fid;
            n.G3E_ID = e.g3e_id;
            n.LTT_ID = MapConfig.Instance.LTTID;
            n.EntityState = EntityState.Insert;
            var pt = new Gg_pd_zfjddz_pt_sdogeom
            {
                G3E_CID = 1,
                G3E_FNO = e.g3e_fno,
                G3E_CNO = 17302,
                G3E_FID = e.g3e_fid,
                G3E_ID = e.g3e_id,
                LTT_ID = MapConfig.Instance.LTTID,
                EntityState = EntityState.Insert,
                OriginalG3e_Fid = e.g3e_fid,
                G3E_GEOMETRY = e.multipointValue,
                BlockName = e.blockName
            };

            AddCommon(e);
            AddGnwz(e);


            AddConnectivity(e);
            GenerateHelper.PartialCopyFromCAD(e.symbolObj, n, e.g3e_fno);

            DBManager.Instance.Insert(n);
            DBManager.Instance.Insert(pt);
            e.insertobj = pt;
        }

        [Execution(174)]
        public static void Gg_pd_wgbc_pt_sdogeom(object sender, InsertSymbolEventArgs e)
        {

            AddCommon(e);
            AddConnectivity(e);
            AddGnwz(e);

            var pt = new Gg_pd_wgbc_pt_sdogeom
            {
                G3E_CID = 1,
                G3E_FNO = e.g3e_fno,
                G3E_CNO = 17401,
                G3E_FID = e.g3e_fid,
                G3E_ID = e.g3e_id,
                LTT_ID = MapConfig.Instance.LTTID,
                EntityState = EntityState.Insert,
                OriginalG3e_Fid = e.g3e_fid,
                G3E_GEOMETRY = e.multipointValue,
                BlockName = e.blockName
            };

            DBManager.Instance.Insert(pt);

            e.insertobj = pt;
        }

        [Execution(176)]
        public static void Gg_pd_jdk_pt_sdogeom(object sender, InsertSymbolEventArgs e)
        {

            AddCommon(e);
            AddConnectivity(e);
            var n = new Gg_pd_jdk_n();
            n.G3E_CID = 1;
            n.G3E_FNO = e.g3e_fno;
            n.G3E_CNO = 17607;
            n.G3E_FID = e.g3e_fid;
            n.G3E_ID = e.g3e_id;
            n.LTT_ID = MapConfig.Instance.LTTID;
            n.EntityState = EntityState.Insert;
            var pt = new Gg_pd_jdk_pt_sdogeom();
            pt.G3E_CID = 1;
            pt.G3E_FNO = e.g3e_fno;
            pt.G3E_CNO = 17601;
            pt.G3E_FID = e.g3e_fid;
            pt.G3E_ID = e.g3e_id;
            pt.LTT_ID = MapConfig.Instance.LTTID;
            pt.EntityState = EntityState.Insert;
            pt.OriginalG3e_Fid = e.g3e_fid; pt.G3E_GEOMETRY = e.multipointValue;
            pt.BlockName = e.blockName;

            GenerateHelper.PartialCopyFromCAD(e.symbolObj, n, e.g3e_fno);

            DBManager.Instance.Insert(n);
            DBManager.Instance.Insert(pt);

            e.insertobj = pt;
        }

        [Execution(177)]
        public static void Gg_pd_blq_pt_sdogeom(object sender, InsertSymbolEventArgs e)
        {

            AddCommon(e);
            AddConnectivity(e);
            var n = new Gg_pd_blq_n
            {
                G3E_CID = 1,
                G3E_FNO = e.g3e_fno,
                G3E_CNO = 17701,
                G3E_FID = e.g3e_fid,
                G3E_ID = e.g3e_id,
                LTT_ID = MapConfig.Instance.LTTID,
                EntityState = EntityState.Insert
            };
            var pt = new Gg_pd_blq_pt_sdogeom();
            pt.G3E_CID = 1;
            pt.G3E_FNO = e.g3e_fno;
            pt.G3E_CNO = 17702;
            pt.G3E_FID = e.g3e_fid;
            pt.G3E_ID = e.g3e_id;
            pt.LTT_ID = MapConfig.Instance.LTTID;
            pt.EntityState = EntityState.Insert;
            pt.OriginalG3e_Fid = e.g3e_fid; pt.G3E_GEOMETRY = e.multipointValue;
            pt.BlockName = e.blockName;


            AddGnwz(e);
            GenerateHelper.PartialCopyFromCAD(e.symbolObj, n, e.g3e_fno);

            DBManager.Instance.Insert(n);
            DBManager.Instance.Insert(pt);

            e.insertobj = pt;
        }

        [Execution(180)]
        public static void Gg_jc_ftu_pt_sdogeom(object sender, InsertSymbolEventArgs e)
        {

            AddCommon(e);
            AddGnwz(e);
            AddConnectivity1(e);
            var n = new Gg_jc_ftu_n
            {
                G3E_CID = 1,
                G3E_FNO = e.g3e_fno,
                G3E_CNO = 18001,
                G3E_FID = e.g3e_fid,
                G3E_ID = e.g3e_id,
                LTT_ID = MapConfig.Instance.LTTID,
                EntityState = EntityState.Insert
            };
            var pt = new Gg_jc_ftu_pt_sdogeom
            {
                G3E_CID = 1,
                G3E_FNO = e.g3e_fno,
                G3E_CNO = 18002,
                G3E_FID = e.g3e_fid,
                G3E_ID = e.g3e_id,
                LTT_ID = MapConfig.Instance.LTTID,
                EntityState = EntityState.Insert,
                OriginalG3e_Fid = e.g3e_fid,
                G3E_GEOMETRY = e.multipointValue,
                BlockName = e.blockName
            };

            DBManager.Instance.Insert(n);
            DBManager.Instance.Insert(pt);

            e.insertobj = pt;
        }

        [Execution(181)]
        public static void Gg_jc_pwy_pt_sdogeom(object sender, InsertSymbolEventArgs e)
        {

            AddCommon(e);
            AddGnwz(e);
            AddConnectivity1(e);
            var n = new Gg_jc_pwy_n();
            n.G3E_CID = 1;
            n.G3E_FNO = e.g3e_fno;
            n.G3E_CNO = 18101;
            n.G3E_FID = e.g3e_fid;
            n.G3E_ID = e.g3e_id;
            n.LTT_ID = MapConfig.Instance.LTTID;
            n.EntityState = EntityState.Insert;
            var pt = new Gg_jc_pwy_pt_sdogeom();
            pt.G3E_CID = 1;
            pt.G3E_FNO = e.g3e_fno;
            pt.G3E_CNO = 18102;
            pt.G3E_FID = e.g3e_fid;
            pt.G3E_ID = e.g3e_id;
            pt.LTT_ID = MapConfig.Instance.LTTID;
            pt.EntityState = EntityState.Insert;
            pt.OriginalG3e_Fid = e.g3e_fid; pt.G3E_GEOMETRY = e.multipointValue;
            pt.BlockName = e.blockName;


            GenerateHelper.PartialCopyFromCAD(e.symbolObj, n, e.g3e_fno);
            long fid = 0;
            if (long.TryParse(e.spf.Ssbyq, out fid))
                n.PBMC = fid;

            DBManager.Instance.Insert(n);
            DBManager.Instance.Insert(pt);

            e.insertobj = pt;
        }

        [Execution(182)]
        public static void Gg_jc_gzzsq_pt_sdogeom(object sender, InsertSymbolEventArgs e)
        {

            AddCommon(e);
            AddConnectivity1(e);
            var n = new Gg_jc_gzzsq_n();
            n.G3E_CID = 1;
            n.G3E_FNO = e.g3e_fno;
            n.G3E_CNO = 18201;
            n.G3E_FID = e.g3e_fid;
            n.G3E_ID = e.g3e_id;
            n.LTT_ID = MapConfig.Instance.LTTID;
            n.EntityState = EntityState.Insert;
            var pt = new Gg_jc_gzzsq_pt_sdogeom();
            pt.G3E_CID = 1;
            pt.G3E_FNO = e.g3e_fno;
            pt.G3E_CNO = 18202;
            pt.G3E_FID = e.g3e_fid;
            pt.G3E_ID = e.g3e_id;
            pt.LTT_ID = MapConfig.Instance.LTTID;
            pt.EntityState = EntityState.Insert;
            pt.OriginalG3e_Fid = e.g3e_fid; pt.G3E_GEOMETRY = e.multipointValue;
            pt.BlockName = e.blockName;

            GenerateHelper.PartialCopyFromCAD(e.symbolObj, n, e.g3e_fno);

            DBManager.Instance.Insert(n);
            DBManager.Instance.Insert(pt);

            e.insertobj = pt;
        }

        [Execution(201)]
        public static void Gg_gz_dg_pt_sdogeom(object sender, InsertSymbolEventArgs e)
        {
            var n = new Gg_gz_dg_n();
            n.G3E_CID = 1;
            n.G3E_FNO = e.g3e_fno;
            n.G3E_CNO = 20101;
            n.G3E_FID = e.g3e_fid;
            n.G3E_ID = e.g3e_id;
            n.LTT_ID = MapConfig.Instance.LTTID;
            n.EntityState = EntityState.Insert;
            var pt = new Gg_gz_dg_pt_sdogeom();
            pt.G3E_CID = 1;
            pt.G3E_FNO = e.g3e_fno;
            pt.G3E_CNO = 20102;
            pt.G3E_FID = e.g3e_fid;
            pt.G3E_ID = e.g3e_id;
            pt.LTT_ID = MapConfig.Instance.LTTID;
            pt.EntityState = EntityState.Insert;
            pt.OriginalG3e_Fid = e.g3e_fid; pt.G3E_GEOMETRY = e.multipointValue;
            pt.BlockName = e.blockName;

            var contain = new Contain_n
            {
                G3E_CID = 1,
                G3E_FNO = e.g3e_fno,
                G3E_FID = e.g3e_fid,
                G3E_CNO = 38,
                G3E_ID = e.g3e_id,
                G3E_OWNERFID = 0,
                G3E_OWNERFNO = 0,
                EntityState = EntityState.Insert,
                LTT_ID = MapConfig.Instance.LTTID
            };



            AddCommon(e);
            AddGnwz(e);

            GenerateHelper.PartialCopyFromCAD(e.symbolObj, n, e.g3e_fno);

            DBManager.Instance.Insert(n);
            DBManager.Instance.Insert(pt);
            DBManager.Instance.Insert(contain);

            e.insertobj = pt;
        }

        [Execution(307)]
        public static void Gg_pd_dyhgq_pt_sdogeom(object sender, InsertSymbolEventArgs e)
        {

            AddCommon(e);
            AddGnwz(e);
            AddConnectivity(e);
            var n = new Gg_pd_dyhgq_n();
            n.G3E_CID = 1;
            n.G3E_FNO = e.g3e_fno;
            n.G3E_CNO = 30701;
            n.G3E_FID = e.g3e_fid;
            n.G3E_ID = e.g3e_id;
            n.LTT_ID = MapConfig.Instance.LTTID;
            n.EntityState = EntityState.Insert;
            var pt = new Gg_pd_dyhgq_pt_sdogeom();
            pt.G3E_CID = 1;
            pt.G3E_FNO = e.g3e_fno;
            pt.G3E_CNO = 30702;
            pt.G3E_FID = e.g3e_fid;
            pt.G3E_ID = e.g3e_id;
            pt.LTT_ID = MapConfig.Instance.LTTID;
            pt.EntityState = EntityState.Insert;
            pt.OriginalG3e_Fid = e.g3e_fid; pt.G3E_GEOMETRY = e.multipointValue;
            pt.BlockName = e.blockName;

            GenerateHelper.PartialCopyFromCAD(e.symbolObj, n, e.g3e_fno);

            DBManager.Instance.Insert(n);
            DBManager.Instance.Insert(pt);

            e.insertobj = pt;
        }

        [Execution(309)]
        public static void Gg_pd_gydj_pt_sdogeom(object sender, InsertSymbolEventArgs e)
        {

            AddCommon(e);
            AddConnectivity(e);
            AddGnwz(e);
            var n = new Gg_pd_gydj_n();
            n.G3E_CID = 1;
            n.G3E_FNO = e.g3e_fno;
            n.G3E_CNO = 30901;
            n.G3E_FID = e.g3e_fid;
            n.G3E_ID = e.g3e_id;
            n.LTT_ID = MapConfig.Instance.LTTID;
            n.EntityState = EntityState.Insert;
            var pt = new Gg_pd_gydj_pt_sdogeom();
            pt.G3E_CID = 1;
            pt.G3E_FNO = e.g3e_fno;
            pt.G3E_CNO = 30902;
            pt.G3E_FID = e.g3e_fid;
            pt.G3E_ID = e.g3e_id;
            pt.LTT_ID = MapConfig.Instance.LTTID;
            pt.EntityState = EntityState.Insert;
            pt.OriginalG3e_Fid = e.g3e_fid; pt.G3E_GEOMETRY = e.multipointValue;
            pt.BlockName = e.blockName;

            GenerateHelper.PartialCopyFromCAD(e.symbolObj, n, e.g3e_fno);

            DBManager.Instance.Insert(n);
            DBManager.Instance.Insert(pt);

            e.insertobj = pt;
        }

        /******************************************************/

        [Execution(73)]
        public static void Gg_pd_dldy_pt_sdogeom(object sender, InsertSymbolEventArgs e)
        {

            AddCommon(e);
            var n = new Gg_pd_dldy_n();
            n.G3E_CID = 1;
            n.G3E_FNO = e.g3e_fno;
            n.G3E_CNO = 7301;
            n.G3E_FID = e.g3e_fid;
            n.G3E_ID = e.g3e_id;
            n.LTT_ID = MapConfig.Instance.LTTID;
            n.EntityState = EntityState.Insert;
            var pt = new Gg_pd_dldy_pt_sdogeom();
            pt.G3E_CID = 1;
            pt.G3E_FNO = e.g3e_fno;
            pt.G3E_CNO = 7302;
            pt.G3E_FID = e.g3e_fid;
            pt.G3E_ID = e.g3e_id;
            pt.LTT_ID = MapConfig.Instance.LTTID;
            pt.EntityState = EntityState.Insert;
            pt.OriginalG3e_Fid = e.g3e_fid; pt.G3E_GEOMETRY = e.multipointValue;
            pt.BlockName = e.blockName;

            AddConnectivity(e);

            GenerateHelper.PartialCopyFromCAD(e.symbolObj, n, e.g3e_fno);

            DBManager.Instance.Insert(n);
            DBManager.Instance.Insert(pt);

            e.insertobj = pt;
        }

        [Execution(74)]
        public static void Gg_pd_dyzd_pt_sdogeom(object sender, InsertSymbolEventArgs e)
        {

            AddCommon(e);
            var n = new Gg_pd_dyzd_n();
            n.G3E_CID = 1;
            n.G3E_FNO = e.g3e_fno;
            n.G3E_CNO = 7401;
            n.G3E_FID = e.g3e_fid;
            n.G3E_ID = e.g3e_id;
            n.LTT_ID = MapConfig.Instance.LTTID;
            n.EntityState = EntityState.Insert;
            var pt = new Gg_pd_dyzd_pt_sdogeom();
            pt.G3E_CID = 1;
            pt.G3E_FNO = e.g3e_fno;
            pt.G3E_CNO = 7402;
            pt.G3E_FID = e.g3e_fid;
            pt.G3E_ID = e.g3e_id;
            pt.LTT_ID = MapConfig.Instance.LTTID;
            pt.EntityState = EntityState.Insert;
            pt.OriginalG3e_Fid = e.g3e_fid; pt.G3E_GEOMETRY = e.multipointValue;
            pt.BlockName = e.blockName;

            AddConnectivity(e);

            GenerateHelper.PartialCopyFromCAD(e.symbolObj, n, e.g3e_fno);

            DBManager.Instance.Insert(n);
            DBManager.Instance.Insert(pt);

            e.insertobj = pt;
        }

        [Execution(155)]
        public static void Gg_pd_dykg_pt_sdogeom(object sender, InsertSymbolEventArgs e)
        {

            AddCommon(e);
            var n = new Gg_pd_dykg_n();
            n.G3E_CID = 1;
            n.G3E_FNO = e.g3e_fno;
            n.G3E_CNO = 15501;
            n.G3E_FID = e.g3e_fid;
            n.G3E_ID = e.g3e_id;
            n.LTT_ID = MapConfig.Instance.LTTID;

            n.EntityState = EntityState.Insert;
            var pt = new Gg_pd_dykg_pt_sdogeom();
            pt.G3E_CID = 1;
            pt.G3E_FNO = e.g3e_fno;
            pt.G3E_CNO = 15502;
            pt.G3E_FID = e.g3e_fid;
            pt.G3E_ID = e.g3e_id;
            pt.LTT_ID = MapConfig.Instance.LTTID;
            pt.EntityState = EntityState.Insert;
            pt.OriginalG3e_Fid = e.g3e_fid; pt.G3E_GEOMETRY = e.multipointValue;
            pt.BlockName = e.blockName;

            AddGnwz(e);
            AddConnectivity(e);

            GenerateHelper.PartialCopyFromCAD(e.symbolObj, n, e.g3e_fno);
            n.SSBYQ = e.spf.Ssbyq;


            DBManager.Instance.Insert(n);
            DBManager.Instance.Insert(pt);

            e.insertobj = pt;
        }

        [Execution(158)]
        public static void Gg_pd_dyzjjt_pt_sdogeom(object sender, InsertSymbolEventArgs e)
        {
            AddCommon(e);
            AddGnwz(e);
            AddConnectivity(e);
            var n = new Gg_pd_dyzjjt_n();
            n.G3E_CID = 1;
            n.G3E_FNO = e.g3e_fno;
            n.G3E_CNO = 15801;
            n.G3E_FID = e.g3e_fid;
            n.G3E_ID = e.g3e_id;
            n.LTT_ID = MapConfig.Instance.LTTID;
            n.EntityState = EntityState.Insert;
            var pt = new Gg_pd_dyzjjt_pt_sdogeom();
            pt.G3E_CID = 1;
            pt.G3E_FNO = e.g3e_fno;
            pt.G3E_CNO = 15802;
            pt.G3E_FID = e.g3e_fid;
            pt.G3E_ID = e.g3e_id;
            pt.LTT_ID = MapConfig.Instance.LTTID;
            pt.EntityState = EntityState.Insert;
            pt.OriginalG3e_Fid = e.g3e_fid; pt.G3E_GEOMETRY = e.multipointValue;
            pt.BlockName = e.blockName;

            GenerateHelper.PartialCopyFromCAD(e.symbolObj, n, e.g3e_fno);

            DBManager.Instance.Insert(n);
            DBManager.Instance.Insert(pt);

            e.insertobj = pt;
        }

        [Execution(159)]
        public static void Gg_pd_cbx_pt_sdogeom(object sender, InsertSymbolEventArgs e)
        {

            AddCommon(e);

            var n = new Gg_pd_cbx_n();
            n.G3E_CID = 1;
            n.G3E_FNO = e.g3e_fno;
            n.G3E_CNO = 15901;
            n.G3E_FID = e.g3e_fid;
            n.G3E_ID = e.g3e_id;
            n.LTT_ID = MapConfig.Instance.LTTID;
            n.EntityState = EntityState.Insert;
            var pt = new Gg_pd_cbx_pt_sdogeom();
            pt.G3E_CID = 1;
            pt.G3E_FNO = e.g3e_fno;
            pt.G3E_CNO = 15902;
            pt.G3E_FID = e.g3e_fid;
            pt.G3E_ID = e.g3e_id;
            pt.LTT_ID = MapConfig.Instance.LTTID;
            pt.EntityState = EntityState.Insert;
            pt.OriginalG3e_Fid = e.g3e_fid; pt.G3E_GEOMETRY = e.multipointValue;
            pt.BlockName = e.blockName;

            GenerateHelper.PartialCopyFromCAD(e.symbolObj, n, e.g3e_fno);
            n.SSBYQ = e.spf.Ssbyq;
            AddConnectivity(e);

            DBManager.Instance.Insert(n);
            DBManager.Instance.Insert(pt);

            var df = new Detailreference_n
            {
                G3E_CID = 1,
                G3E_ID = e.g3e_id,
                G3E_CNO = 35,
                G3E_FID = e.g3e_fid,
                G3E_FNO = e.g3e_fno,
                LTT_ID = MapConfig.Instance.LTTID,
                EntityState = EntityState.Insert,
                G3E_DETAILID = CYZCommonFunc.getid(),
                DETAIL_USERNAME = e.g3e_fid.ToString(),
                DETAIL_LEGENDNUMBER = 6,
                DETAIL_MBRXLO = Convert.ToDecimal(113.054316785185),
                DETAIL_MBRYLO = Convert.ToDecimal(22.5901489851852),
                DETAIL_MBRXHIGH = Convert.ToDecimal(113.058946414815),
                DETAIL_MBRYHIGH = Convert.ToDecimal(22.5947786148148),
                DETAIL_MBRXOFFSET = 50000,
                DETAIL_MBRYOFFSET = 50000
            };
            DBManager.Instance.Insert(df);
            e.insertobj = pt;
        }


        [Execution(169)]
        public static void Gg_pd_dydlt_pt_sdogeom(object sender, InsertSymbolEventArgs e)
        {

            AddCommon(e);
            AddGnwz(e);
            AddConnectivity(e);
            var n = new Gg_pd_dydlt_n();
            n.G3E_CID = 1;
            n.G3E_FNO = e.g3e_fno;
            n.G3E_CNO = 16901;
            n.G3E_FID = e.g3e_fid;
            n.G3E_ID = e.g3e_id;
            n.LTT_ID = MapConfig.Instance.LTTID;
            n.EntityState = EntityState.Insert;
            var pt = new Gg_pd_dydlt_pt_sdogeom();
            pt.G3E_CID = 1;
            pt.G3E_FNO = e.g3e_fno;
            pt.G3E_CNO = 16902;
            pt.G3E_FID = e.g3e_fid;
            pt.G3E_ID = e.g3e_id;
            pt.LTT_ID = MapConfig.Instance.LTTID;
            pt.EntityState = EntityState.Insert;
            pt.OriginalG3e_Fid = e.g3e_fid; pt.G3E_GEOMETRY = e.multipointValue;
            pt.BlockName = e.blockName;

            GenerateHelper.PartialCopyFromCAD(e.symbolObj, n, e.g3e_fno);

            DBManager.Instance.Insert(n);
            DBManager.Instance.Insert(pt);

            e.insertobj = pt;
        }

        [Execution(175)]
        public static void Gg_pd_dll_pt_sdogeom(object sender, InsertSymbolEventArgs e)
        {

            AddCommon(e);
            AddConnectivity(e);
            var n = new Gg_pd_dll_n();
            n.G3E_CID = 1;
            n.G3E_FNO = e.g3e_fno;
            n.G3E_CNO = 17501;
            n.G3E_FID = e.g3e_fid;
            n.G3E_ID = e.g3e_id;
            n.LTT_ID = MapConfig.Instance.LTTID;
            n.EntityState = EntityState.Insert;
            var pt = new Gg_pd_dll_pt_sdogeom();
            pt.G3E_CID = 1;
            pt.G3E_FNO = e.g3e_fno;
            pt.G3E_CNO = 17502;
            pt.G3E_FID = e.g3e_fid;
            pt.G3E_ID = e.g3e_id;
            pt.LTT_ID = MapConfig.Instance.LTTID;
            pt.EntityState = EntityState.Insert;
            pt.OriginalG3e_Fid = e.g3e_fid; pt.G3E_GEOMETRY = e.multipointValue;

            pt.BlockName = e.blockName;

            GenerateHelper.PartialCopyFromCAD(e.symbolObj, n, e.g3e_fno);

            DBManager.Instance.Insert(n);
            DBManager.Instance.Insert(pt);

            e.insertobj = pt;
        }

        [Execution(215)]
        public static void Gg_gz_dydg_pt_sdogeom(object sender, InsertSymbolEventArgs e)
        {
            var n = new Gg_gz_dydg_n();
            n.G3E_CID = 1;
            n.G3E_FNO = e.g3e_fno;
            n.G3E_CNO = 21501;
            n.G3E_FID = e.g3e_fid;
            n.G3E_ID = e.g3e_id;
            n.LTT_ID = MapConfig.Instance.LTTID;
            n.EntityState = EntityState.Insert;
            var pt = new Gg_gz_dydg_pt_sdogeom();
            pt.G3E_CID = 1;
            pt.G3E_FNO = e.g3e_fno;
            pt.G3E_CNO = 21502;
            pt.G3E_FID = e.g3e_fid;
            pt.G3E_ID = e.g3e_id;
            pt.LTT_ID = MapConfig.Instance.LTTID;
            pt.EntityState = EntityState.Insert;
            pt.OriginalG3e_Fid = e.g3e_fid; pt.G3E_GEOMETRY = e.multipointValue;
            pt.BlockName = e.blockName;


            AddCommon(e);
            AddGnwz(e);

            GenerateHelper.PartialCopyFromCAD(e.symbolObj, n, e.g3e_fno);
            DBManager.Instance.Insert(n);
            DBManager.Instance.Insert(pt);

            e.insertobj = pt;
        }

        [Execution(218)]
        public static void Gg_gz_dyzc_pt_sdogeom(object sender, InsertSymbolEventArgs e)
        {

            AddCommon(e);
            var n = new Gg_gz_dyzc_n();
            n.G3E_CID = 1;
            n.G3E_FNO = e.g3e_fno;
            n.G3E_CNO = 20201;
            n.G3E_FID = e.g3e_fid;
            n.G3E_ID = e.g3e_id;
            n.LTT_ID = MapConfig.Instance.LTTID;
            n.EntityState = EntityState.Insert;
            var pt = new Gg_gz_dyzc_pt_sdogeom();
            pt.G3E_CID = 1;
            pt.G3E_FNO = e.g3e_fno;
            pt.G3E_CNO = 20202;
            pt.G3E_FID = e.g3e_fid;
            pt.G3E_ID = e.g3e_id;
            pt.LTT_ID = MapConfig.Instance.LTTID;
            pt.EntityState = EntityState.Insert;
            pt.OriginalG3e_Fid = e.g3e_fid; pt.G3E_GEOMETRY = e.multipointValue;
            pt.BlockName = e.blockName;

            GenerateHelper.PartialCopyFromCAD(e.symbolObj, n, e.g3e_fno);

            DBManager.Instance.Insert(n);
            DBManager.Instance.Insert(pt);

            e.insertobj = pt;
        }

        [Execution(320)]
        public static void Gg_pd_jszz_pt_sdogeom(object sender, InsertSymbolEventArgs e)
        {

            AddCommon(e);
            AddConnectivity1(e);
            var n = new Gg_pd_jszz_n();
            n.G3E_CID = 1;
            n.G3E_FNO = e.g3e_fno;
            n.G3E_CNO = 32001;
            n.G3E_FID = e.g3e_fid;
            n.G3E_ID = e.g3e_id;
            n.LTT_ID = MapConfig.Instance.LTTID;
            n.EntityState = EntityState.Insert;
            var pt = new Gg_pd_jszz_pt_sdogeom();
            pt.G3E_CID = 1;
            pt.G3E_FNO = e.g3e_fno;
            pt.G3E_CNO = 32002;
            pt.G3E_FID = e.g3e_fid;
            pt.G3E_ID = e.g3e_id;
            pt.LTT_ID = MapConfig.Instance.LTTID;
            pt.EntityState = EntityState.Insert;
            pt.OriginalG3e_Fid = e.g3e_fid; pt.G3E_GEOMETRY = e.multipointValue;
            pt.BlockName = e.blockName;

            GenerateHelper.PartialCopyFromCAD(e.symbolObj, n, e.g3e_fno);

            DBManager.Instance.Insert(n);
            DBManager.Instance.Insert(pt);

            e.insertobj = pt;
        }


        [Execution(71)]
        public static void Gg_pd_cqtg_pt_sdogeom(object sender, InsertSymbolEventArgs e)
        {

            AddCommon(e);
            AddConnectivity1(e);
            var pt = new Gg_pd_cqtg_pt_sdogeom();
            pt.G3E_CID = 1;
            pt.G3E_FNO = e.g3e_fno;
            pt.G3E_CNO = 7102;
            pt.G3E_FID = e.g3e_fid;
            pt.G3E_ID = e.g3e_id;
            pt.LTT_ID = MapConfig.Instance.LTTID;
            pt.EntityState = EntityState.Insert;
            pt.OriginalG3e_Fid = e.g3e_fid; pt.G3E_GEOMETRY = e.multipointValue;
            pt.BlockName = e.blockName;

            DBManager.Instance.Insert(pt);

            e.insertobj = pt;
        }

        [Execution(72)]
        public static void Gg_pd_jdgh_pt_sdogeom(object sender, InsertSymbolEventArgs e)
        {

            AddCommon(e);
            AddConnectivity(e);
            var pt = new Gg_pd_jdgh_pt_sdogeom();
            pt.G3E_CID = 1;
            pt.G3E_FNO = e.g3e_fno;
            pt.G3E_CNO = 7202;
            pt.G3E_FID = e.g3e_fid;
            pt.G3E_ID = e.g3e_id;
            pt.LTT_ID = MapConfig.Instance.LTTID;
            pt.EntityState = EntityState.Insert;
            pt.OriginalG3e_Fid = e.g3e_fid; pt.G3E_GEOMETRY = e.multipointValue;
            pt.BlockName = e.blockName;

            DBManager.Instance.Insert(pt);

            e.insertobj = pt;
        }

        [Execution(76)]
        public static void Gg_pd_gkjlg_pt_sdogeom(object sender, InsertSymbolEventArgs e)
        {

            AddCommon(e);
            AddConnectivity(e);
            AddGnwz(e);
            var pt = new Gg_pd_gkjlg_pt_sdogeom();
            pt.G3E_CID = 1;
            pt.G3E_FNO = e.g3e_fno;
            pt.G3E_CNO = 7602;
            pt.G3E_FID = e.g3e_fid;
            pt.G3E_ID = e.g3e_id;
            pt.LTT_ID = MapConfig.Instance.LTTID;
            pt.EntityState = EntityState.Insert;
            pt.OriginalG3e_Fid = e.g3e_fid; pt.G3E_GEOMETRY = e.multipointValue;
            pt.BlockName = e.blockName;

            var n = new Gg_pd_gkjlg_n
            {
                G3E_CID = 1,
                G3E_FNO = e.g3e_fno,
                G3E_CNO = 7601,
                G3E_FID = e.g3e_fid,
                G3E_ID = e.g3e_id,
                LTT_ID = MapConfig.Instance.LTTID,
                EntityState = EntityState.Insert
            };
            GenerateHelper.PartialCopyFromCAD(e.symbolObj, n, e.g3e_fno);
            DBManager.Instance.Insert(n);
            DBManager.Instance.Insert(pt);

            e.insertobj = pt;
        }

        [Execution(205)]
        public static void Gg_gz_rj_pt_sdogeom(object sender, InsertSymbolEventArgs e)
        {

            AddCommon(e);
            AddGnwz(e);
            var pt = new Gg_gz_rj_pt_sdogeom();
            pt.G3E_CID = 1;
            pt.G3E_FNO = e.g3e_fno;
            pt.G3E_CNO = 20502;
            pt.G3E_FID = e.g3e_fid;
            pt.G3E_ID = e.g3e_id;
            pt.LTT_ID = MapConfig.Instance.LTTID;
            pt.EntityState = EntityState.Insert;
            pt.OriginalG3e_Fid = e.g3e_fid; pt.G3E_GEOMETRY = e.multipointValue;
            pt.BlockName = e.blockName;

            var n = new Gg_gz_rj_n
            {
                G3E_CID = 1,
                G3E_FNO = e.g3e_fno,
                G3E_CNO = 20501,
                G3E_FID = e.g3e_fid,
                G3E_ID = e.g3e_id,
                LTT_ID = MapConfig.Instance.LTTID,
                EntityState = EntityState.Insert
            };
            GenerateHelper.PartialCopyFromCAD(e.symbolObj, n, e.g3e_fno);
            DBManager.Instance.Insert(n);
            DBManager.Instance.Insert(pt);

            e.insertobj = pt;
        }

        [Execution(183)]
        public static void Gg_jc_qyjcy_pt_sdogeom(object sender, InsertSymbolEventArgs e)
        {

            AddCommon(e);
            var pt = new Gg_jc_qyjcy_pt_sdogeom();
            pt.G3E_CID = 1;
            pt.G3E_FNO = e.g3e_fno;
            pt.G3E_CNO = 18302;
            pt.G3E_FID = e.g3e_fid;
            pt.G3E_ID = e.g3e_id;
            pt.LTT_ID = MapConfig.Instance.LTTID;
            pt.EntityState = EntityState.Insert;
            pt.OriginalG3e_Fid = e.g3e_fid; pt.G3E_GEOMETRY = e.multipointValue;
            pt.BlockName = e.blockName;

            var n = new Gg_jc_qyjcy_n
            {
                G3E_CID = 1,
                G3E_FNO = e.g3e_fno,
                G3E_CNO = 18301,
                G3E_FID = e.g3e_fid,
                G3E_ID = e.g3e_id,
                LTT_ID = MapConfig.Instance.LTTID,
                EntityState = EntityState.Insert
            };

            DBManager.Instance.Insert(n);
            DBManager.Instance.Insert(pt);

            e.insertobj = pt;
        }

        [Execution(184)]
        public static void Gg_jc_yyjcy_pt_sdogeom(object sender, InsertSymbolEventArgs e)
        {

            AddCommon(e);
            var pt = new Gg_jc_yyjcy_pt_sdogeom();
            pt.G3E_CID = 1;
            pt.G3E_FNO = e.g3e_fno;
            pt.G3E_CNO = 18402;
            pt.G3E_FID = e.g3e_fid;
            pt.G3E_ID = e.g3e_id;
            pt.LTT_ID = MapConfig.Instance.LTTID;
            pt.EntityState = EntityState.Insert;
            pt.OriginalG3e_Fid = e.g3e_fid; pt.G3E_GEOMETRY = e.multipointValue;
            pt.BlockName = e.blockName;

            var n = new Gg_jc_yyjcy_n
            {
                G3E_CID = 1,
                G3E_FNO = e.g3e_fno,
                G3E_CNO = 18401,
                G3E_FID = e.g3e_fid,
                G3E_ID = e.g3e_id,
                LTT_ID = MapConfig.Instance.LTTID,
                EntityState = EntityState.Insert
            };

            DBManager.Instance.Insert(n);
            DBManager.Instance.Insert(pt);

            e.insertobj = pt;
        }

        [Execution(185)]
        public static void Gg_jc_dyjcy_pt_sdogeom(object sender, InsertSymbolEventArgs e)
        {

            AddCommon(e);
            var pt = new Gg_jc_dyjcy_pt_sdogeom();
            pt.G3E_CID = 1;
            pt.G3E_FNO = e.g3e_fno;
            pt.G3E_CNO = 18502;
            pt.G3E_FID = e.g3e_fid;
            pt.G3E_ID = e.g3e_id;
            pt.LTT_ID = MapConfig.Instance.LTTID;
            pt.EntityState = EntityState.Insert;
            pt.OriginalG3e_Fid = e.g3e_fid; pt.G3E_GEOMETRY = e.multipointValue;
            pt.BlockName = e.blockName;

            var n = new Gg_jc_dyjcy_n
            {
                G3E_CID = 1,
                G3E_FNO = e.g3e_fno,
                G3E_CNO = 18501,
                G3E_FID = e.g3e_fid,
                G3E_ID = e.g3e_id,
                LTT_ID = MapConfig.Instance.LTTID,
                EntityState = EntityState.Insert
            };
            GenerateHelper.PartialCopyFromCAD(e.symbolObj, n, e.g3e_fno);

            DBManager.Instance.Insert(n);
            DBManager.Instance.Insert(pt);

            e.insertobj = pt;
        }

        [Execution(186)]
        public static void Gg_jc_ldkgx_pt_sdogeom(object sender, InsertSymbolEventArgs e)
        {

            AddCommon(e);
            var pt = new Gg_jc_ldkgx_pt_sdogeom();
            pt.G3E_CID = 1;
            pt.G3E_FNO = e.g3e_fno;
            pt.G3E_CNO = 18602;
            pt.G3E_FID = e.g3e_fid;
            pt.G3E_ID = e.g3e_id;
            pt.LTT_ID = MapConfig.Instance.LTTID;
            pt.EntityState = EntityState.Insert;
            pt.OriginalG3e_Fid = e.g3e_fid; pt.G3E_GEOMETRY = e.multipointValue;
            pt.BlockName = e.blockName;

            var n = new Gg_jc_ldkgx_n
            {
                G3E_CID = 1,
                G3E_FNO = e.g3e_fno,
                G3E_CNO = 18601,
                G3E_FID = e.g3e_fid,
                G3E_ID = e.g3e_id,
                LTT_ID = MapConfig.Instance.LTTID,
                EntityState = EntityState.Insert
            };
            GenerateHelper.PartialCopyFromCAD(e.symbolObj, n, e.g3e_fno);
            DBManager.Instance.Insert(n);
            DBManager.Instance.Insert(pt);

            e.insertobj = pt;
        }

        [Execution(188)]
        public static void Gg_jc_fk_pt_sdogeom(object sender, InsertSymbolEventArgs e)
        {
            AddCommon(e);
            AddConnectivity(e);
            var pt = new Gg_jc_fk_pt_sdogeom
            {
                G3E_CID = 1,
                G3E_FNO = e.g3e_fno,
                G3E_CNO = 18802,
                G3E_FID = e.g3e_fid,
                G3E_ID = e.g3e_id,
                LTT_ID = MapConfig.Instance.LTTID,
                EntityState = EntityState.Insert,
                OriginalG3e_Fid = e.g3e_fid,
                G3E_GEOMETRY = e.multipointValue,
                BlockName = e.blockName
            };

            var n = new Gg_jc_fk_n
            {
                G3E_CID = 1,
                G3E_FNO = e.g3e_fno,
                G3E_CNO = 18801,
                G3E_FID = e.g3e_fid,
                G3E_ID = e.g3e_id,
                LTT_ID = MapConfig.Instance.LTTID,
                EntityState = EntityState.Insert
            };
            GenerateHelper.PartialCopyFromCAD(e.symbolObj, n, e.g3e_fno);
            DBManager.Instance.Insert(n);
            DBManager.Instance.Insert(pt);
            e.insertobj = pt;
        }

        [Execution(86)]
        public static void Gg_jc_dcjg_pt_sdogeom(object sender, InsertSymbolEventArgs e)
        {
            AddCommon(e);
            var pt = new Gg_jc_dcjg_pt_sdogeom
            {
                G3E_CID = 1,
                G3E_FNO = e.g3e_fno,
                G3E_CNO = 8602,
                G3E_FID = e.g3e_fid,
                G3E_ID = e.g3e_id,
                LTT_ID = MapConfig.Instance.LTTID,
                EntityState = EntityState.Insert,
                OriginalG3e_Fid = e.g3e_fid,
                G3E_GEOMETRY = e.multipointValue,
                BlockName = e.blockName
            };

            var n = new Gg_jc_dcjg_n
            {
                G3E_CID = 1,
                G3E_FNO = e.g3e_fno,
                G3E_CNO = 8601,
                G3E_FID = e.g3e_fid,
                G3E_ID = e.g3e_id,
                LTT_ID = MapConfig.Instance.LTTID,
                EntityState = EntityState.Insert
            };
            GenerateHelper.PartialCopyFromCAD(e.symbolObj, n, e.g3e_fno);
            long dffid = 0, kggFid = 0;
            if (long.TryParse(e.spf.Ssdf, out dffid))
                n.SSDF = dffid;
            if (long.TryParse(e.spf.Sskgg, out kggFid))
                n.DYKGG = kggFid;

            DBManager.Instance.Insert(n);
            DBManager.Instance.Insert(pt);

            e.insertobj = pt;
        }

        /// <summary>
        /// 公共表属性类赋值
        /// </summary>
        public static void AddCommon(InsertSymbolEventArgs e)
        {
            var entry = SimpleDBSymbolsConfig.Instance.DBSymbol.SingleOrDefault(o => o.G3E_FNO == e.g3e_fno);
            if (string.IsNullOrEmpty(entry.ComponentTable.Common)) return;
            var com = new Common_n
            {
                G3E_CID = 1,
                G3E_FNO = e.g3e_fno,
                G3E_ID = e.g3e_id,
                G3E_CNO = 30,
                G3E_FID = e.g3e_fid,
                LTT_ID = MapConfig.Instance.LTTID,
                EntityState = EntityState.Insert
            };
            GenerateHelper.PartialCopyFromCAD(e.symbolObj, com, e.g3e_fno);
            var bz1 = e.symbolObj.SingleOrDefault(o => o.SaveValueByFid && o.Name.Equals("BZ1"));
            if (bz1 != null)
            {
                com.BZ1 = e.spf.SsZx;
            }
            else
            {
                var bz2 = e.symbolObj.SingleOrDefault(o => o.SaveValueByFid && o.Name.Equals("BZ2"));
                if (bz2 != null)
                    com.BZ2 = e.spf.SsZx;
            }
            DBManager.Instance.Insert(com);

            if (string.IsNullOrEmpty(entry.ComponentTable.Gg_Pd_Sdkx_Ac)) return;
            if (!string.IsNullOrEmpty(com.CD_SSBDZ) && !string.IsNullOrEmpty(com.CD_SSXL))
                InsertDBEntity.InsertSDKX(com);
        }

        /// <summary>
        /// 连接表属性类赋值
        /// </summary>
        /// <returns></returns>
        public static void AddConnectivity(InsertSymbolEventArgs e)
        {
            var entry = SimpleDBSymbolsConfig.Instance.DBSymbol.SingleOrDefault(o => o.G3E_FNO == e.g3e_fno);
            if (string.IsNullOrEmpty(entry.ComponentTable.Connectivity)) return;
            var con = new Connectivity_n
            {
                G3E_CID = 1,
                G3E_FNO = e.g3e_fno,
                G3E_ID = e.g3e_id,
                G3E_CNO = 31,
                G3E_FID = e.g3e_fid,
                NODE1_ID = 0,
                NODE2_ID = 0,
                LTT_ID = MapConfig.Instance.LTTID,
                EntityState = EntityState.Add_Nal_Nal
            };
            GenerateHelper.PartialCopyFromCAD(e.symbolObj, con, e.g3e_fno);
            DBManager.Instance.Insert(con);
        }

        /// <summary>
        /// 连接表属性类赋值
        /// </summary>
        /// <returns></returns>
        public static void AddConnectivity1(InsertSymbolEventArgs e)
        {
            var entry = SimpleDBSymbolsConfig.Instance.DBSymbol.SingleOrDefault(o => o.G3E_FNO == e.g3e_fno);
            if (string.IsNullOrEmpty(entry.ComponentTable.Connectivity)) return;
            var con = new Connectivity_n
            {
                G3E_CID = 1,
                G3E_FNO = e.g3e_fno,
                G3E_ID = e.g3e_id,
                G3E_CNO = 31,
                G3E_FID = e.g3e_fid,
                NODE1_ID = 0,
                NODE2_ID = 0,
                LTT_ID = MapConfig.Instance.LTTID,
                EntityState = EntityState.Add_Nal_Nal
            };

            DBManager.Instance.Insert(con);
        }

        /// <summary>
        /// 功能位置
        /// </summary>
        public static void AddGnwz(InsertSymbolEventArgs e)
        {
            var entry = SimpleDBSymbolsConfig.Instance.DBSymbol.SingleOrDefault(o => o.G3E_FNO == e.g3e_fno);
            if (string.IsNullOrEmpty(entry.ComponentTable.Gnwz)) return;
            var gn = new Gg_pd_gnwzmc_n
            {
                G3E_CID = 1,
                G3E_FNO = e.g3e_fno,
                G3E_CNO = 50,
                G3E_FID = e.g3e_fid,
                G3E_ID = e.g3e_id,
                LTT_ID = MapConfig.Instance.LTTID,
                EntityState = EntityState.Insert
            };
            GenerateHelper.PartialCopyFromCAD(e.symbolObj, gn, e.g3e_fno);
            gn.GNWZ_SSDF = e.spf.Ssdf;
            gn.GNWZ_SSGT = e.spf.Ssgt;
            gn.GNWZ_SSKGG = e.spf.Sskgg;
            gn.GNWZ_SSTJ = e.spf.Sstj;
            //gn.GNWZ_SSTQHTJ = e.spf.Sstqhtj;
            DBManager.Instance.Insert(gn);
        }

        #endregion

        #region

        //private static void AddSelfAttribute(InsertSymbolEventArgs e, DBEntity dest)
        //{
        //    var sourcePropertyNames = GenerateHelper.GetPropertyNames(e.symbolObj);
        //    var destPropertyNames = dest.GetPropertyNames();
        //    var interset = sourcePropertyNames.Intersect(destPropertyNames);
        //    if (dest is Gg_jc_pwy_n)
        //    {
        //        foreach (var item in interset)
        //        {
        //            try
        //            {
        //                if (item.Equals("PBMC"))
        //                {
        //                    long fid = 0;
        //                    if (long.TryParse(e.spf.Ssbyq, out fid))
        //                        dest.SetValue(item, fid);
        //                }
        //                else
        //                {
        //                    var value = GenerateHelper.GetPropertyValue(e.symbolObj, item);
        //                    dest.SetValue(item, value);
        //                }
        //            }
        //            catch (Exception)
        //            {
        //                dest.SetValue(item, null);
        //            }

        //        }
        //    }
        //    else if (dest is Gg_jc_dcjg_n)
        //    {
        //        foreach (var item in interset)
        //        {
        //            try
        //            {
        //                if (item.Equals("SSDF"))
        //                {
        //                    long fid = 0;
        //                    if (long.TryParse(e.spf.Ssdf, out fid))
        //                        dest.SetValue(item, fid);
        //                }
        //                else if (item.Equals("DYKGG"))
        //                {
        //                    long fid = 0;
        //                    if (long.TryParse(e.spf.Sskgg, out fid))
        //                        dest.SetValue(item, fid);
        //                }
        //                else
        //                {
        //                    var value = GenerateHelper.GetPropertyValue(e.symbolObj, item);
        //                    dest.SetValue(item, value);
        //                }
        //            }
        //            catch (Exception)
        //            {
        //                dest.SetValue(item, null);
        //            }

        //        }
        //    }

        //}

        #endregion
    }
}
