using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using ElectronTransferDal;
using ElectronTransferDal.Cad;
using ElectronTransferDal.Common;
using ElectronTransferFramework;
using ElectronTransferModel;
using ElectronTransferModel.Base;
using ElectronTransferModel.V9_4;

namespace ElectronTransferBLL
{
    [ExecutionContainer(typeof(MoveSymbolExecutionFactory))]
    public class MoveSymbolEventHandler
    {
        #region 线
        [Execution(140)]
        public static void Gg_pd_dl_ln_sdogeom(object sender, MoveSymbolEventArgs e)
        {
            var pt = DBManager.Instance.GetEntity<Gg_pd_dl_ln_sdogeom>(o => o.G3E_FID == (long)sender);
            pt.EntityState = pt.EntityState==EntityState.Insert?EntityState.InsertUpdate:EntityState.Update;
            pt.G3E_GEOMETRY = e.lineStringValue;
            DBManager.Instance.Update<Gg_pd_dl_ln_sdogeom>(pt);
        }
        [Execution(141)]
        public static void Gg_pd_dx_ln_sdogeom(object sender, MoveSymbolEventArgs e)
        {
            var pt = DBManager.Instance.GetEntity<Gg_pd_dx_ln_sdogeom>(o => o.G3E_FID == (long)sender);
            pt.EntityState = pt.EntityState==EntityState.Insert?EntityState.InsertUpdate:EntityState.Update;
            pt.G3E_GEOMETRY = e.lineStringValue;
            DBManager.Instance.Update<Gg_pd_dx_ln_sdogeom>(pt);
        }
        [Execution(143)]
        public static void Gg_pd_zfmx_ln_sdogeom(object sender, MoveSymbolEventArgs e)
        {
            var pt = DBManager.Instance.GetEntity<Gg_pd_zfmx_ln_sdogeom>(o => o.G3E_FID == (long)sender);
            pt.EntityState = pt.EntityState==EntityState.Insert?EntityState.InsertUpdate:EntityState.Update;
            pt.G3E_GEOMETRY = e.lineStringValue;
            DBManager.Instance.Update<Gg_pd_zfmx_ln_sdogeom>(pt);
        }
        [Execution(144)]
        public static void Gg_pd_zfyx_ln_sdogeom(object sender, MoveSymbolEventArgs e)
        {
            var pt = DBManager.Instance.GetEntity<Gg_pd_zfyx_ln_sdogeom>(o => o.G3E_FID == (long)sender);
            pt.EntityState = pt.EntityState==EntityState.Insert?EntityState.InsertUpdate:EntityState.Update;
            pt.G3E_GEOMETRY = e.lineStringValue;
            DBManager.Instance.Update<Gg_pd_zfyx_ln_sdogeom>(pt);
        }
        [Execution(79)]
        public static void Gg_pd_dymx_ln_sdogeom(object sender, MoveSymbolEventArgs e)
        {
            var pt = DBManager.Instance.GetEntity<Gg_pd_dymx_ln_sdogeom>(o => o.G3E_FID == (long)sender);
            pt.EntityState = pt.EntityState==EntityState.Insert?EntityState.InsertUpdate:EntityState.Update;
            pt.G3E_GEOMETRY = e.lineStringValue;
            DBManager.Instance.Update<Gg_pd_dymx_ln_sdogeom>(pt);
        }
        [Execution(156)]
        public static void Gg_pd_dydx_ln_sdogeom(object sender, MoveSymbolEventArgs e)
        {
            var pt = DBManager.Instance.GetEntity<Gg_pd_dydx_ln_sdogeom>(o => o.G3E_FID == (long)sender);
            pt.EntityState = pt.EntityState==EntityState.Insert?EntityState.InsertUpdate:EntityState.Update;
            pt.G3E_GEOMETRY = e.lineStringValue;
            DBManager.Instance.Update<Gg_pd_dydx_ln_sdogeom>(pt);
        }
        [Execution(157)]
        public static void Gg_pd_dydl_ln_sdogeom(object sender, MoveSymbolEventArgs e)
        {
            var pt = DBManager.Instance.GetEntity<Gg_pd_dydl_ln_sdogeom>(o => o.G3E_FID == (long)sender);
            pt.EntityState = pt.EntityState==EntityState.Insert?EntityState.InsertUpdate:EntityState.Update;
            pt.G3E_GEOMETRY = e.lineStringValue;
            DBManager.Instance.Update<Gg_pd_dydl_ln_sdogeom>(pt);
        }
        [Execution(80)]
        public static void Gg_pd_bqx_ln_sdogeom(object sender, MoveSymbolEventArgs e)
        {
            var pt = DBManager.Instance.GetEntity<Gg_pd_bqx_ln_sdogeom>(o => o.G3E_FID == (long)sender);
            pt.EntityState = pt.EntityState==EntityState.Insert?EntityState.InsertUpdate:EntityState.Update;
            pt.G3E_GEOMETRY = e.lineStringValue;
            DBManager.Instance.Update<Gg_pd_bqx_ln_sdogeom>(pt);
        }
        [Execution(75)]
        public static void Gg_pd_bdcx_pt_sdogeom(object sender, MoveSymbolEventArgs e)
        {
            var pt = DBManager.Instance.GetEntity<Gg_pd_bdcx_pt_sdogeom>(o => o.G3E_FID == (long)sender);
            pt.EntityState = pt.EntityState==EntityState.Insert?EntityState.InsertUpdate:EntityState.Update;
            pt.G3E_GEOMETRY = e.lineStringValue;
            DBManager.Instance.Update<Gg_pd_bdcx_pt_sdogeom>(pt);
        }
        [Execution(165)]
        public static void Gg_jx_yx_ln_sdogeom(object sender, MoveSymbolEventArgs e)
        {
            var pt = DBManager.Instance.GetEntity<Gg_jx_yx_ln_sdogeom>(o => o.G3E_FID == (long)sender);
            pt.EntityState = pt.EntityState==EntityState.Insert?EntityState.InsertUpdate:EntityState.Update;
            pt.G3E_GEOMETRY = e.lineStringValue;
            DBManager.Instance.Update<Gg_jx_yx_ln_sdogeom>(pt);
        }
        #endregion

        #region 面
        [Execution(77)]
        public static void Gg_pd_dlfxx_ar_sdogeom(object sender, MoveSymbolEventArgs e)
        {
            var pt = DBManager.Instance.GetEntity<Gg_pd_dlfxx_ar_sdogeom>(o => o.G3E_FID == (long)sender);
            pt.EntityState = pt.EntityState==EntityState.Insert?EntityState.InsertUpdate:EntityState.Update;
            pt.G3E_GEOMETRY = e.lineStringValue;
            DBManager.Instance.Update<Gg_pd_dlfxx_ar_sdogeom>(pt);
        }

        [Execution(81)]
        public static void Gg_pd_dyg_ar_sdogeom(object sender, MoveSymbolEventArgs e)
        {
            var pt = DBManager.Instance.GetEntity<Gg_pd_dyg_ar_sdogeom>(o => o.G3E_FID == (long)sender);
            pt.EntityState = pt.EntityState==EntityState.Insert?EntityState.InsertUpdate:EntityState.Update;
            pt.G3E_GEOMETRY = e.lineStringValue;
            DBManager.Instance.Update<Gg_pd_dyg_ar_sdogeom>(pt);
        }

        [Execution(142)]
        public static void Gg_pd_gydf_ar_sdogeom(object sender, MoveSymbolEventArgs e)
        {
            var pt = DBManager.Instance.GetEntity<Gg_pd_gydf_ar_sdogeom>(o => o.G3E_FID == (long)sender);
            pt.EntityState = pt.EntityState==EntityState.Insert?EntityState.InsertUpdate:EntityState.Update;
            pt.G3E_GEOMETRY = e.lineStringValue;
            DBManager.Instance.Update<Gg_pd_gydf_ar_sdogeom>(pt);
        }

        [Execution(149)]
        public static void Gg_pd_xssb_ar_sdogeom(object sender, MoveSymbolEventArgs e)
        {
            var pt = DBManager.Instance.GetEntity<Gg_pd_xssb_ar_sdogeom>(o => o.G3E_FID == (long)sender);
            pt.EntityState = pt.EntityState==EntityState.Insert?EntityState.InsertUpdate:EntityState.Update;
            pt.G3E_GEOMETRY = e.lineStringValue;
            DBManager.Instance.Update<Gg_pd_xssb_ar_sdogeom>(pt);
        }

        [Execution(163)]
        public static void Gg_pd_zydf_ar_sdogeom(object sender, MoveSymbolEventArgs e)
        {
            var pt = DBManager.Instance.GetEntity<Gg_pd_zydf_ar_sdogeom>(o => o.G3E_FID == (long)sender);
            pt.EntityState = pt.EntityState==EntityState.Insert?EntityState.InsertUpdate:EntityState.Update;
            pt.G3E_GEOMETRY = e.lineStringValue;
            DBManager.Instance.Update<Gg_pd_zydf_ar_sdogeom>(pt);
        }

        [Execution(302)]
        public static void Gg_pd_dypdf_ar_sdogeom(object sender, MoveSymbolEventArgs e)
        {
            var pt = DBManager.Instance.GetEntity<Gg_pd_dypdf_ar_sdogeom>(o => o.G3E_FID == (long)sender);
            pt.EntityState = pt.EntityState==EntityState.Insert?EntityState.InsertUpdate:EntityState.Update;
            pt.G3E_GEOMETRY = e.lineStringValue;
            DBManager.Instance.Update<Gg_pd_dypdf_ar_sdogeom>(pt);
        }

        [Execution(198)]
        public static void Gg_pd_kgg_ar_sdogeom(object sender, MoveSymbolEventArgs e)
        {
            var pt = DBManager.Instance.GetEntity<Gg_pd_kgg_ar_sdogeom>(o => o.G3E_FID == (long)sender);
            pt.EntityState = pt.EntityState==EntityState.Insert?EntityState.InsertUpdate:EntityState.Update;
            pt.G3E_GEOMETRY = e.lineStringValue;
            DBManager.Instance.Update<Gg_pd_kgg_ar_sdogeom>(pt);
        }

        [Execution(199)]
        public static void Gg_gz_tj_ar_sdogeom(object sender, MoveSymbolEventArgs e)
        {
            var pt = DBManager.Instance.GetEntity<Gg_gz_tj_ar_sdogeom>(o => o.G3E_FID == (long)sender);
            pt.EntityState = pt.EntityState==EntityState.Insert?EntityState.InsertUpdate:EntityState.Update;
            pt.G3E_GEOMETRY = e.lineStringValue;
            DBManager.Instance.Update<Gg_gz_tj_ar_sdogeom>(pt);
        }
        #endregion

        #region 点设备
        [Execution(40)]
        public static void Gg_pd_dlhgq_pt_sdogeom(object sender, MoveSymbolEventArgs e)
        {
            var pt = DBManager.Instance.GetEntity<Gg_pd_dlhgq_pt_sdogeom>(o => o.G3E_FID == (long)sender);
            pt.EntityState = pt.EntityState==EntityState.Insert?EntityState.InsertUpdate:EntityState.Update;
            pt.G3E_GEOMETRY = e.multipointValue;
            DBManager.Instance.Update<Gg_pd_dlhgq_pt_sdogeom>(pt);
        }

        [Execution(84)]
        public static void Gg_pd_cjlg_pt_sdogeom(object sender, MoveSymbolEventArgs e)
        {
            var pt = DBManager.Instance.GetEntity<Gg_pd_cjlg_pt_sdogeom>(o => o.G3E_FID == (long)sender);
            pt.EntityState = pt.EntityState==EntityState.Insert?EntityState.InsertUpdate:EntityState.Update;
            pt.G3E_GEOMETRY = e.multipointValue;
            DBManager.Instance.Update<Gg_pd_cjlg_pt_sdogeom>(pt);
        }

        [Execution(85)]
        public static void Gg_pd_ptg_pt_sdogeom(object sender, MoveSymbolEventArgs e)
        {
            var pt = DBManager.Instance.GetEntity<Gg_pd_ptg_pt_sdogeom>(o => o.G3E_FID == (long)sender);
            pt.EntityState = pt.EntityState==EntityState.Insert?EntityState.InsertUpdate:EntityState.Update;
            pt.G3E_GEOMETRY = e.multipointValue;
            DBManager.Instance.Update<Gg_pd_ptg_pt_sdogeom>(pt);
        }

        [Execution(90)]
        public static void Gg_jc_dtu_pt_sdogeom(object sender, MoveSymbolEventArgs e)
        {
            var pt = DBManager.Instance.GetEntity<Gg_jc_dtu_pt_sdogeom>(o => o.G3E_FID == (long)sender);
            pt.EntityState = pt.EntityState==EntityState.Insert?EntityState.InsertUpdate:EntityState.Update;
            pt.G3E_GEOMETRY = e.multipointValue;
            DBManager.Instance.Update<Gg_jc_dtu_pt_sdogeom>(pt);
        }

        [Execution(145)]
        public static void Gg_pd_zfllkg_pt_sdogeom(object sender, MoveSymbolEventArgs e)
        {
            var pt = DBManager.Instance.GetEntity<Gg_pd_zfllkg_pt_sdogeom>(o => o.G3E_FID == (long)sender);
            pt.EntityState = pt.EntityState==EntityState.Insert?EntityState.InsertUpdate:EntityState.Update;
            pt.G3E_GEOMETRY = e.multipointValue;
            DBManager.Instance.Update<Gg_pd_zfllkg_pt_sdogeom>(pt);
        }

        [Execution(146)]
        public static void Gg_pd_zfhwg_pt_sdogeom(object sender, MoveSymbolEventArgs e)
        {
            var pt = DBManager.Instance.GetEntity<Gg_pd_zfhwg_pt_sdogeom>(o => o.G3E_FID == (long)sender);
            pt.EntityState = pt.EntityState==EntityState.Insert?EntityState.InsertUpdate:EntityState.Update;
            pt.G3E_GEOMETRY = e.multipointValue;
            DBManager.Instance.Update<Gg_pd_zfhwg_pt_sdogeom>(pt);
        }

        [Execution(147)]
        public static void Gg_pd_zfdlt_pt_sdogeom(object sender, MoveSymbolEventArgs e)
        {
            var pt = DBManager.Instance.GetEntity<Gg_pd_zfdlt_pt_sdogeom>(o => o.G3E_FID == (long)sender);
            pt.EntityState = pt.EntityState==EntityState.Insert?EntityState.InsertUpdate:EntityState.Update;
            pt.G3E_GEOMETRY = e.multipointValue;
            DBManager.Instance.Update<Gg_pd_zfdlt_pt_sdogeom>(pt);
        }

        [Execution(148)]
        public static void Gg_pd_zfbyq_pt_sdogeom(object sender, MoveSymbolEventArgs e)
        {
            //var found = DBManager.Instance.GetEntities(TypeCache.Instance.GetTypeFromCache(typeof(DBEntity), "Common_n")).FirstOrDefault(o => o.GetValue<long>("G3E_FID") == (long)sender);
            //Assert.NotNull(found);

            var pt = DBManager.Instance.GetEntity<Gg_pd_zfbyq_pt_sdogeom>(o => o.G3E_FID == (long)sender);
            pt.EntityState = pt.EntityState==EntityState.Insert?EntityState.InsertUpdate:EntityState.Update;
            pt.G3E_GEOMETRY = e.multipointValue;
            DBManager.Instance.Update<Gg_pd_zfbyq_pt_sdogeom>(pt);
        }

        [Execution(150)]
        public static void Gg_pd_dlzjjt_pt_sdogeom(object sender, MoveSymbolEventArgs e)
        {
            var pt = DBManager.Instance.GetEntity<Gg_pd_dlzjjt_pt_sdogeom>(o => o.G3E_FID == (long)sender);
            pt.EntityState = pt.EntityState==EntityState.Insert?EntityState.InsertUpdate:EntityState.Update;
            pt.G3E_GEOMETRY = e.multipointValue;
            DBManager.Instance.Update<Gg_pd_dlzjjt_pt_sdogeom>(pt);
        }

        [Execution(151)]
        public static void Gg_pd_dlzdt_pt_sdogeom(object sender, MoveSymbolEventArgs e)
        {
            var pt = DBManager.Instance.GetEntity<Gg_pd_dlzdt_pt_sdogeom>(o => o.G3E_FID == (long)sender);
            pt.EntityState = pt.EntityState==EntityState.Insert?EntityState.InsertUpdate:EntityState.Update;
            pt.G3E_GEOMETRY = e.multipointValue;
            DBManager.Instance.Update<Gg_pd_dlzdt_pt_sdogeom>(pt);
        }

        [Execution(152)]
        public static void Gg_pd_dlq_pt_sdogeom(object sender, MoveSymbolEventArgs e)
        {
            var pt = DBManager.Instance.GetEntity<Gg_pd_dlq_pt_sdogeom>(o => o.G3E_FID == (long)sender);
            pt.EntityState = pt.EntityState==EntityState.Insert?EntityState.InsertUpdate:EntityState.Update;
            pt.G3E_GEOMETRY = e.multipointValue;
            DBManager.Instance.Update<Gg_pd_dlq_pt_sdogeom>(pt);
        }

        [Execution(154)]
        public static void Gg_pd_zsbyq_pt_sdogeom(object sender, MoveSymbolEventArgs e)
        {
            var pt = DBManager.Instance.GetEntity<Gg_pd_zsbyq_pt_sdogeom>(o => o.G3E_FID == (long)sender);
            pt.EntityState = pt.EntityState==EntityState.Insert?EntityState.InsertUpdate:EntityState.Update;
            pt.G3E_GEOMETRY = e.multipointValue;
            DBManager.Instance.Update<Gg_pd_zsbyq_pt_sdogeom>(pt);
        }

        [Execution(171)]
        public static void Gg_pd_zdfdj_pt_sdogeom(object sender, MoveSymbolEventArgs e)
        {
            var pt = DBManager.Instance.GetEntity<Gg_pd_zdfdj_pt_sdogeom>(o => o.G3E_FID == (long)sender);
            pt.EntityState = pt.EntityState==EntityState.Insert?EntityState.InsertUpdate:EntityState.Update;
            pt.G3E_GEOMETRY = e.multipointValue;
            DBManager.Instance.Update<Gg_pd_zdfdj_pt_sdogeom>(pt);
        }

        [Execution(172)]
        public static void Gg_pd_ddj_pt_sdogeom(object sender, MoveSymbolEventArgs e)
        {
            var pt = DBManager.Instance.GetEntity<Gg_pd_ddj_pt_sdogeom>(o => o.G3E_FID == (long)sender);
            pt.EntityState = pt.EntityState==EntityState.Insert?EntityState.InsertUpdate:EntityState.Update;
            pt.G3E_GEOMETRY = e.multipointValue;
            DBManager.Instance.Update<Gg_pd_ddj_pt_sdogeom>(pt);
        }

        [Execution(173)]
        public static void Gg_pd_zfjddz_pt_sdogeom(object sender, MoveSymbolEventArgs e)
        {
            var pt = DBManager.Instance.GetEntity<Gg_pd_zfjddz_pt_sdogeom>(o => o.G3E_FID == (long)sender);
            pt.EntityState = pt.EntityState==EntityState.Insert?EntityState.InsertUpdate:EntityState.Update;
            pt.G3E_GEOMETRY = e.multipointValue;
            DBManager.Instance.Update<Gg_pd_zfjddz_pt_sdogeom>(pt);
        }

        [Execution(174)]
        public static void Gg_pd_wgbc_pt_sdogeom(object sender, MoveSymbolEventArgs e)
        {
            var pt = DBManager.Instance.GetEntity<Gg_pd_wgbc_pt_sdogeom>(o => o.G3E_FID == (long)sender);
            pt.EntityState = pt.EntityState==EntityState.Insert?EntityState.InsertUpdate:EntityState.Update;
            pt.G3E_GEOMETRY = e.multipointValue;
            DBManager.Instance.Update<Gg_pd_wgbc_pt_sdogeom>(pt);
        }

        [Execution(175)]
        public static void Gg_pd_dll_pt_sdogeom(object sender, MoveSymbolEventArgs e)
        {
            var pt = DBManager.Instance.GetEntity<Gg_pd_dll_pt_sdogeom>(o => o.G3E_FID == (long)sender);
            pt.EntityState = pt.EntityState==EntityState.Insert?EntityState.InsertUpdate:EntityState.Update;
            pt.G3E_GEOMETRY = e.multipointValue;
            DBManager.Instance.Update<Gg_pd_dll_pt_sdogeom>(pt);
        }

        [Execution(176)]
        public static void Gg_pd_jdk_pt_sdogeom(object sender, MoveSymbolEventArgs e)
        {
            var pt = DBManager.Instance.GetEntity<Gg_pd_jdk_pt_sdogeom>(o => o.G3E_FID == (long)sender);
            pt.EntityState = pt.EntityState==EntityState.Insert?EntityState.InsertUpdate:EntityState.Update;
            pt.G3E_GEOMETRY = e.multipointValue;
            DBManager.Instance.Update<Gg_pd_jdk_pt_sdogeom>(pt);
        }

        [Execution(177)]
        public static void Gg_pd_blq_pt_sdogeom(object sender, MoveSymbolEventArgs e)
        {
            var pt = DBManager.Instance.GetEntity<Gg_pd_blq_pt_sdogeom>(o => o.G3E_FID == (long)sender);
            pt.EntityState = pt.EntityState==EntityState.Insert?EntityState.InsertUpdate:EntityState.Update;
            pt.G3E_GEOMETRY = e.multipointValue;
            DBManager.Instance.Update<Gg_pd_blq_pt_sdogeom>(pt);
        }


        [Execution(181)]
        public static void Gg_jc_pwy_pt_sdogeom(object sender, MoveSymbolEventArgs e)
        {
            var pt = DBManager.Instance.GetEntity<Gg_jc_pwy_pt_sdogeom>(o => o.G3E_FID == (long)sender);
            pt.EntityState = pt.EntityState==EntityState.Insert?EntityState.InsertUpdate:EntityState.Update;
            pt.G3E_GEOMETRY = e.multipointValue;
            DBManager.Instance.Update<Gg_jc_pwy_pt_sdogeom>(pt);
        }

        [Execution(182)]
        public static void Gg_jc_gzzsq_pt_sdogeom(object sender, MoveSymbolEventArgs e)
        {
            var pt = DBManager.Instance.GetEntity<Gg_jc_gzzsq_pt_sdogeom>(o => o.G3E_FID == (long)sender);
            pt.EntityState = pt.EntityState==EntityState.Insert?EntityState.InsertUpdate:EntityState.Update;
            pt.G3E_GEOMETRY = e.multipointValue;
            DBManager.Instance.Update<Gg_jc_gzzsq_pt_sdogeom>(pt);
        }

        [Execution(201)]
        public static void Gg_gz_dg_pt_sdogeom(object sender, MoveSymbolEventArgs e)
        {
            var pt = DBManager.Instance.GetEntity<Gg_gz_dg_pt_sdogeom>(o => o.G3E_FID == (long)sender);
            pt.EntityState = pt.EntityState==EntityState.Insert?EntityState.InsertUpdate:EntityState.Update;
            pt.G3E_GEOMETRY = e.multipointValue;
            DBManager.Instance.Update<Gg_gz_dg_pt_sdogeom>(pt);
        }

        [Execution(307)]
        public static void Gg_pd_dyhgq_pt_sdogeom(object sender, MoveSymbolEventArgs e)
        {
            var pt = DBManager.Instance.GetEntity<Gg_pd_dyhgq_pt_sdogeom>(o => o.G3E_FID == (long)sender);
            pt.EntityState = pt.EntityState==EntityState.Insert?EntityState.InsertUpdate:EntityState.Update;
            pt.G3E_GEOMETRY = e.multipointValue;
            DBManager.Instance.Update<Gg_pd_dyhgq_pt_sdogeom>(pt);
        }

        [Execution(309)]
        public static void Gg_pd_gydj_pt_sdogeom(object sender, MoveSymbolEventArgs e)
        {
            var pt = DBManager.Instance.GetEntity<Gg_pd_gydj_pt_sdogeom>(o => o.G3E_FID == (long)sender);
            pt.EntityState = pt.EntityState==EntityState.Insert?EntityState.InsertUpdate:EntityState.Update;
            pt.G3E_GEOMETRY = e.multipointValue;
            DBManager.Instance.Update<Gg_pd_gydj_pt_sdogeom>(pt);
        }

        /******************************************************/

        [Execution(73)]
        public static void Gg_pd_dldy_pt_sdogeom(object sender, MoveSymbolEventArgs e)
        {
            var pt = DBManager.Instance.GetEntity<Gg_pd_dldy_pt_sdogeom>(o => o.G3E_FID == (long)sender);
            pt.EntityState = pt.EntityState==EntityState.Insert?EntityState.InsertUpdate:EntityState.Update;
            pt.G3E_GEOMETRY = e.multipointValue;
            DBManager.Instance.Update<Gg_pd_dldy_pt_sdogeom>(pt);
        }

        [Execution(74)]
        public static void Gg_pd_dyzd_pt_sdogeom(object sender, MoveSymbolEventArgs e)
        {
            var pt = DBManager.Instance.GetEntity<Gg_pd_dyzd_pt_sdogeom>(o => o.G3E_FID == (long)sender);
            pt.EntityState = pt.EntityState==EntityState.Insert?EntityState.InsertUpdate:EntityState.Update;
            pt.G3E_GEOMETRY = e.multipointValue;
            DBManager.Instance.Update<Gg_pd_dyzd_pt_sdogeom>(pt);
        }

        [Execution(155)]
        public static void Gg_pd_dykg_pt_sdogeom(object sender, MoveSymbolEventArgs e)
        {
            var pt = DBManager.Instance.GetEntity<Gg_pd_dykg_pt_sdogeom>(o => o.G3E_FID == (long)sender);
            pt.EntityState = pt.EntityState==EntityState.Insert?EntityState.InsertUpdate:EntityState.Update;
            pt.G3E_GEOMETRY = e.multipointValue;
            DBManager.Instance.Update<Gg_pd_dykg_pt_sdogeom>(pt);
        }

        [Execution(158)]
        public static void Gg_pd_dyzjjt_pt_sdogeom(object sender, MoveSymbolEventArgs e)
        {
            var pt = DBManager.Instance.GetEntity<Gg_pd_dyzjjt_pt_sdogeom>(o => o.G3E_FID == (long)sender);
            pt.EntityState = pt.EntityState==EntityState.Insert?EntityState.InsertUpdate:EntityState.Update;
            pt.G3E_GEOMETRY = e.multipointValue;
            DBManager.Instance.Update<Gg_pd_dyzjjt_pt_sdogeom>(pt);
        }

        [Execution(159)]
        public static void Gg_pd_cbx_pt_sdogeom(object sender, MoveSymbolEventArgs e)
        {
            var pt = DBManager.Instance.GetEntity<Gg_pd_cbx_pt_sdogeom>(o => o.G3E_FID == (long)sender);
            pt.EntityState = pt.EntityState==EntityState.Insert?EntityState.InsertUpdate:EntityState.Update;
            pt.G3E_GEOMETRY = e.multipointValue;
            DBManager.Instance.Update<Gg_pd_cbx_pt_sdogeom>(pt);
        }

        [Execution(169)]
        public static void Gg_pd_dydlt_pt_sdogeom(object sender, MoveSymbolEventArgs e)
        {
            var pt = DBManager.Instance.GetEntity<Gg_pd_dydlt_pt_sdogeom>(o => o.G3E_FID == (long)sender);
            pt.EntityState = pt.EntityState==EntityState.Insert?EntityState.InsertUpdate:EntityState.Update;
            pt.G3E_GEOMETRY = e.multipointValue;
            DBManager.Instance.Update<Gg_pd_dydlt_pt_sdogeom>(pt);
        }

        [Execution(215)]
        public static void Gg_gz_dydg_pt_sdogeom(object sender, MoveSymbolEventArgs e)
        {
            var pt = DBManager.Instance.GetEntity<Gg_gz_dydg_pt_sdogeom>(o => o.G3E_FID == (long)sender);
            pt.EntityState = pt.EntityState==EntityState.Insert?EntityState.InsertUpdate:EntityState.Update;
            pt.G3E_GEOMETRY = e.multipointValue;
            DBManager.Instance.Update<Gg_gz_dydg_pt_sdogeom>(pt);
        }

        [Execution(218)]
        public static void Gg_gz_dyzc_pt_sdogeom(object sender, MoveSymbolEventArgs e)
        {
            var pt = DBManager.Instance.GetEntity<Gg_gz_dyzc_pt_sdogeom>(o => o.G3E_FID == (long)sender);
            pt.EntityState = pt.EntityState==EntityState.Insert?EntityState.InsertUpdate:EntityState.Update;
            pt.G3E_GEOMETRY = e.multipointValue;
            DBManager.Instance.Update<Gg_gz_dyzc_pt_sdogeom>(pt);
        }
        #endregion
    }
}
