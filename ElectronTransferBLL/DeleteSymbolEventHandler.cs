using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using ArxMap;
using ElectronTransferBll;
using ElectronTransferDal.Cad;
using ElectronTransferDal;
using ElectronTransferDal.Common;
using ElectronTransferFramework;
using ElectronTransferModel;
using ElectronTransferModel.V9_4;

namespace ElectronTransferBLL
{
    [ExecutionContainer(typeof (DeleteSymbolExecutionFactory))]
    public class DeleteSymbolEventHandler
    {
        #region 线

        [Execution(140)]
        public static void Gg_pd_dl_ln_sdogeom(object sender, DeleteSymbolEventArgs e)
        {
            ErasedEntityArgs eea = (ErasedEntityArgs) sender;
            UpdateCommon(eea);
            UpdateGnwz(eea);
            UpdateConnection(eea);
            var n = DBManager.Instance.GetEntity<Gg_pd_dl_n>(o => o.G3E_FID == eea.G3e_fid);
            n.EntityState = eea.Erased
                                ? n.EntityState == EntityState.Insert ? EntityState.InsertDelete : EntityState.Delete
                                : n.EntityState == EntityState.Insert ? EntityState.InsertUpdate : EntityState.Update;
            var pt = DBManager.Instance.GetEntity<Gg_pd_dl_ln_sdogeom>(o => o.G3E_FID == eea.G3e_fid);
            pt.EntityState = n.EntityState;
            DBManager.Instance.Update<Gg_pd_dl_n>(n);
            DBManager.Instance.Update<Gg_pd_dl_ln_sdogeom>(pt);
        }

        [Execution(141)]
        public static void Gg_pd_dx_ln_sdogeom(object sender, DeleteSymbolEventArgs e)
        {
            ErasedEntityArgs eea = (ErasedEntityArgs) sender;
            UpdateCommon(eea);
            UpdateGnwz(eea);
            UpdateConnection(eea);
            var n = DBManager.Instance.GetEntity<Gg_pd_dx_n>(o => o.G3E_FID == eea.G3e_fid);
            n.EntityState = eea.Erased
                                ? n.EntityState == EntityState.Insert ? EntityState.InsertDelete : EntityState.Delete
                                : n.EntityState == EntityState.Insert ? EntityState.InsertUpdate : EntityState.Update;
            var pt = DBManager.Instance.GetEntity<Gg_pd_dx_ln_sdogeom>(o => o.G3E_FID == eea.G3e_fid);
            pt.EntityState = n.EntityState;
            DBManager.Instance.Update<Gg_pd_dx_n>(n);
            DBManager.Instance.Update<Gg_pd_dx_ln_sdogeom>(pt);
        }

        [Execution(143)]
        public static void Gg_pd_zfmx_ln_sdogeom(object sender, DeleteSymbolEventArgs e)
        {
            ErasedEntityArgs eea = (ErasedEntityArgs) sender;
            UpdateCommon(eea);
            var n = DBManager.Instance.GetEntity<Gg_pd_zfmx_n>(o => o.G3E_FID == eea.G3e_fid);
            n.EntityState = eea.Erased
                                ? n.EntityState == EntityState.Insert ? EntityState.InsertDelete : EntityState.Delete
                                : n.EntityState == EntityState.Insert ? EntityState.InsertUpdate : EntityState.Update;
            var pt = DBManager.Instance.GetEntity<Gg_pd_zfmx_ln_sdogeom>(o => o.G3E_FID == eea.G3e_fid);
            pt.EntityState = n.EntityState;
            DBManager.Instance.Update<Gg_pd_zfmx_n>(n);
            DBManager.Instance.Update<Gg_pd_zfmx_ln_sdogeom>(pt);
        }

        [Execution(144)]
        public static void Gg_pd_zfyx_ln_sdogeom(object sender, DeleteSymbolEventArgs e)
        {
            ErasedEntityArgs eea = (ErasedEntityArgs) sender;
            UpdateCommon(eea);
            var n = DBManager.Instance.GetEntity<Gg_pd_zfyx_n>(o => o.G3E_FID == eea.G3e_fid);
            n.EntityState = eea.Erased
                                ? n.EntityState == EntityState.Insert ? EntityState.InsertDelete : EntityState.Delete
                                : n.EntityState == EntityState.Insert ? EntityState.InsertUpdate : EntityState.Update;
            var pt = DBManager.Instance.GetEntity<Gg_pd_zfyx_ln_sdogeom>(o => o.G3E_FID == eea.G3e_fid);
            pt.EntityState = n.EntityState;
            DBManager.Instance.Update<Gg_pd_zfyx_n>(n);
            DBManager.Instance.Update<Gg_pd_zfyx_ln_sdogeom>(pt);
        }

        [Execution(79)]
        public static void Gg_pd_dymx_ln_sdogeom(object sender, DeleteSymbolEventArgs e)
        {
            ErasedEntityArgs eea = (ErasedEntityArgs) sender;
            UpdateCommon(eea);
            var n = DBManager.Instance.GetEntity<Gg_pd_dymx_n>(o => o.G3E_FID == eea.G3e_fid);
            n.EntityState = eea.Erased
                                ? n.EntityState == EntityState.Insert ? EntityState.InsertDelete : EntityState.Delete
                                : n.EntityState == EntityState.Insert ? EntityState.InsertUpdate : EntityState.Update;
            var pt = DBManager.Instance.GetEntity<Gg_pd_dymx_ln_sdogeom>(o => o.G3E_FID == eea.G3e_fid);
            pt.EntityState = n.EntityState;
            DBManager.Instance.Update<Gg_pd_dymx_n>(n);
            DBManager.Instance.Update<Gg_pd_dymx_ln_sdogeom>(pt);
        }

        [Execution(156)]
        public static void Gg_pd_dydx_ln_sdogeom(object sender, DeleteSymbolEventArgs e)
        {
            ErasedEntityArgs eea = (ErasedEntityArgs) sender;
            UpdateCommon(eea);
            UpdateGnwz(eea);
            UpdateConnection(eea);
            var n = DBManager.Instance.GetEntity<Gg_pd_dydx_n>(o => o.G3E_FID == eea.G3e_fid);
            n.EntityState = eea.Erased
                                ? n.EntityState == EntityState.Insert ? EntityState.InsertDelete : EntityState.Delete
                                : n.EntityState == EntityState.Insert ? EntityState.InsertUpdate : EntityState.Update;
            var pt = DBManager.Instance.GetEntity<Gg_pd_dydx_ln_sdogeom>(o => o.G3E_FID == eea.G3e_fid);
            pt.EntityState = n.EntityState;
            DBManager.Instance.Update<Gg_pd_dydx_n>(n);
            DBManager.Instance.Update<Gg_pd_dydx_ln_sdogeom>(pt);
        }

        [Execution(157)]
        public static void Gg_pd_dydl_ln_sdogeom(object sender, DeleteSymbolEventArgs e)
        {
            ErasedEntityArgs eea = (ErasedEntityArgs) sender;
            UpdateCommon(eea);
            UpdateGnwz(eea);
            UpdateConnection(eea);
            var n = DBManager.Instance.GetEntity<Gg_pd_dydl_n>(o => o.G3E_FID == eea.G3e_fid);
            n.EntityState = eea.Erased
                                ? n.EntityState == EntityState.Insert ? EntityState.InsertDelete : EntityState.Delete
                                : n.EntityState == EntityState.Insert ? EntityState.InsertUpdate : EntityState.Update;
            var pt = DBManager.Instance.GetEntity<Gg_pd_dydl_ln_sdogeom>(o => o.G3E_FID == eea.G3e_fid);
            pt.EntityState = n.EntityState;
            DBManager.Instance.Update<Gg_pd_dydl_n>(n);
            DBManager.Instance.Update<Gg_pd_dydl_ln_sdogeom>(pt);
        }

        [Execution(80)]
        public static void Gg_pd_bqx_ln_sdogeom(object sender, DeleteSymbolEventArgs e)
        {
            ErasedEntityArgs eea = (ErasedEntityArgs) sender;
            UpdateCommon(eea);
            var n = DBManager.Instance.GetEntity<Gg_pd_bqx_n>(o => o.G3E_FID == eea.G3e_fid);
            n.EntityState = eea.Erased
                                ? n.EntityState == EntityState.Insert ? EntityState.InsertDelete : EntityState.Delete
                                : n.EntityState == EntityState.Insert ? EntityState.InsertUpdate : EntityState.Update;
            var pt = DBManager.Instance.GetEntity<Gg_pd_bqx_ln_sdogeom>(o => o.G3E_FID == eea.G3e_fid);
            pt.EntityState = n.EntityState;
            DBManager.Instance.Update<Gg_pd_bqx_n>(n);
            DBManager.Instance.Update<Gg_pd_bqx_ln_sdogeom>(pt);
        }

        [Execution(75)]
        public static void Gg_pd_bdcx_pt_sdogeom(object sender, DeleteSymbolEventArgs e)
        {
            ErasedEntityArgs eea = (ErasedEntityArgs) sender;
            UpdateCommon(eea);
            var n = DBManager.Instance.GetEntity<Gg_pd_bdcx_n>(o => o.G3E_FID == eea.G3e_fid);
            n.EntityState = eea.Erased
                                ? n.EntityState == EntityState.Insert ? EntityState.InsertDelete : EntityState.Delete
                                : n.EntityState == EntityState.Insert ? EntityState.InsertUpdate : EntityState.Update;
            var pt = DBManager.Instance.GetEntity<Gg_pd_bdcx_pt_sdogeom>(o => o.G3E_FID == eea.G3e_fid);
            pt.EntityState = n.EntityState;
            DBManager.Instance.Update<Gg_pd_bdcx_n>(n);
            DBManager.Instance.Update<Gg_pd_bdcx_pt_sdogeom>(pt);
        }

        [Execution(165)]
        public static void Gg_jx_yx_ln_sdogeom(object sender, DeleteSymbolEventArgs e)
        {
            ErasedEntityArgs eea = (ErasedEntityArgs) sender;
            UpdateCommon(eea);
            var n = DBManager.Instance.GetEntity<Gg_jx_yx_n>(o => o.G3E_FID == eea.G3e_fid);
            n.EntityState = eea.Erased
                                ? n.EntityState == EntityState.Insert ? EntityState.InsertDelete : EntityState.Delete
                                : n.EntityState == EntityState.Insert ? EntityState.InsertUpdate : EntityState.Update;
            var pt = DBManager.Instance.GetEntity<Gg_jx_yx_ln_sdogeom>(o => o.G3E_FID == eea.G3e_fid);
            pt.EntityState = n.EntityState;
            DBManager.Instance.Update<Gg_jx_yx_n>(n);
            DBManager.Instance.Update<Gg_jx_yx_ln_sdogeom>(pt);
        }

        #endregion

        [Execution(40)]
        public static void Gg_pd_dlhgq_pt_sdogeom(object sender, DeleteSymbolEventArgs e)
        {
            ErasedEntityArgs eea = (ErasedEntityArgs) sender;
            UpdateCommon(eea);
            var n = DBManager.Instance.GetEntity<Gg_pd_dlhgq_n>(o => o.G3E_FID == eea.G3e_fid);
            n.EntityState = eea.Erased
                                ? n.EntityState == EntityState.Insert ? EntityState.InsertDelete : EntityState.Delete
                                : n.EntityState == EntityState.Insert ? EntityState.InsertUpdate : EntityState.Update;
            var pt = DBManager.Instance.GetEntity<Gg_pd_dlhgq_pt_sdogeom>(o => o.G3E_FID == eea.G3e_fid);
            pt.EntityState = n.EntityState;
            DBManager.Instance.Update<Gg_pd_dlhgq_n>(n);
            DBManager.Instance.Update<Gg_pd_dlhgq_pt_sdogeom>(pt);
        }


        [Execution(84)]
        public static void Gg_pd_cjlg_pt_sdogeom(object sender, DeleteSymbolEventArgs e)
        {
            ErasedEntityArgs eea = (ErasedEntityArgs) sender;
            UpdateCommon(eea);
            var n = DBManager.Instance.GetEntity<Gg_pd_cjlg_n>(o => o.G3E_FID == eea.G3e_fid);
            n.EntityState = eea.Erased
                                ? n.EntityState == EntityState.Insert ? EntityState.InsertDelete : EntityState.Delete
                                : n.EntityState == EntityState.Insert ? EntityState.InsertUpdate : EntityState.Update;
            var pt = DBManager.Instance.GetEntity<Gg_pd_cjlg_pt_sdogeom>(o => o.G3E_FID == eea.G3e_fid);
            pt.EntityState = n.EntityState;
            DBManager.Instance.Update<Gg_pd_cjlg_n>(n);
            DBManager.Instance.Update<Gg_pd_cjlg_pt_sdogeom>(pt);
        }

        [Execution(85)]
        public static void Gg_pd_ptg_pt_sdogeom(object sender, DeleteSymbolEventArgs e)
        {
            ErasedEntityArgs eea = (ErasedEntityArgs) sender;
            UpdateCommon(eea);
            var n = DBManager.Instance.GetEntity<Gg_pd_ptg_n>(o => o.G3E_FID == eea.G3e_fid);
            n.EntityState = eea.Erased
                                ? n.EntityState == EntityState.Insert ? EntityState.InsertDelete : EntityState.Delete
                                : n.EntityState == EntityState.Insert ? EntityState.InsertUpdate : EntityState.Update;
            var pt = DBManager.Instance.GetEntity<Gg_pd_ptg_pt_sdogeom>(o => o.G3E_FID == eea.G3e_fid);
            pt.EntityState = n.EntityState;
            DBManager.Instance.Update<Gg_pd_ptg_n>(n);
            DBManager.Instance.Update<Gg_pd_ptg_pt_sdogeom>(pt);
        }

        [Execution(145)]
        public static void Gg_pd_zfllkg_pt_sdogeom(object sender, DeleteSymbolEventArgs e)
        {
            ErasedEntityArgs eea = (ErasedEntityArgs)sender;
            UpdateCommon(eea);
            var n = DBManager.Instance.GetEntity<Gg_pd_zfllkg_n>(o => o.G3E_FID == eea.G3e_fid);
            n.EntityState = eea.Erased ? n.EntityState == EntityState.Insert ? EntityState.InsertDelete : EntityState.Delete : n.EntityState == EntityState.Insert ? EntityState.InsertUpdate : EntityState.Update;
            var pt = DBManager.Instance.GetEntity<Gg_pd_zfllkg_pt_sdogeom>(o => o.G3E_FID == eea.G3e_fid);
            pt.EntityState = n.EntityState;
            DBManager.Instance.Update<Gg_pd_zfllkg_n>(n);
            DBManager.Instance.Update<Gg_pd_zfllkg_pt_sdogeom>(pt);
        }

        [Execution(146)]
        public static void Gg_pd_zfhwg_pt_sdogeom(object sender, DeleteSymbolEventArgs e)
        {
            ErasedEntityArgs eea = (ErasedEntityArgs) sender;
            UpdateCommon(eea);
            UpdateConnection(eea);
            var n = DBManager.Instance.GetEntity<Gg_pd_zfhwg_n>(o => o.G3E_FID == eea.G3e_fid);
            n.EntityState = eea.Erased
                                ? n.EntityState == EntityState.Insert ? EntityState.InsertDelete : EntityState.Delete
                                : n.EntityState == EntityState.Insert ? EntityState.InsertUpdate : EntityState.Update;
            var pt = DBManager.Instance.GetEntity<Gg_pd_zfhwg_pt_sdogeom>(o => o.G3E_FID == eea.G3e_fid);
            pt.EntityState = n.EntityState;
            DBManager.Instance.Update<Gg_pd_zfhwg_n>(n);
            DBManager.Instance.Update<Gg_pd_zfhwg_pt_sdogeom>(pt);
        }

        [Execution(147)]
        public static void Gg_pd_zfdlt_pt_sdogeom(object sender, DeleteSymbolEventArgs e)
        {
            ErasedEntityArgs eea = (ErasedEntityArgs) sender;
            UpdateCommon(eea);
            var n = DBManager.Instance.GetEntity<Gg_pd_zfdlt_n>(o => o.G3E_FID == eea.G3e_fid);
            n.EntityState = eea.Erased
                                ? n.EntityState == EntityState.Insert ? EntityState.InsertDelete : EntityState.Delete
                                : n.EntityState == EntityState.Insert ? EntityState.InsertUpdate : EntityState.Update;
            var pt = DBManager.Instance.GetEntity<Gg_pd_zfdlt_pt_sdogeom>(o => o.G3E_FID == eea.G3e_fid);
            pt.EntityState = n.EntityState;
            DBManager.Instance.Update<Gg_pd_zfdlt_n>(n);
            DBManager.Instance.Update<Gg_pd_zfdlt_pt_sdogeom>(pt);
        }

        [Execution(148)]
        public static void Gg_pd_zfbyq_pt_sdogeom(object sender, DeleteSymbolEventArgs e)
        {
            ErasedEntityArgs eea = (ErasedEntityArgs) sender;
            UpdateCommon(eea);
            var n = DBManager.Instance.GetEntity<Gg_pd_zfbyq_n>(o => o.G3E_FID == eea.G3e_fid);
            n.EntityState = eea.Erased
                                ? n.EntityState == EntityState.Insert ? EntityState.InsertDelete : EntityState.Delete
                                : n.EntityState == EntityState.Insert ? EntityState.InsertUpdate : EntityState.Update;
            var pt = DBManager.Instance.GetEntity<Gg_pd_zfbyq_pt_sdogeom>(o => o.G3E_FID == eea.G3e_fid);
            pt.EntityState = n.EntityState;
            DBManager.Instance.Update<Gg_pd_zfbyq_n>(n);
            DBManager.Instance.Update<Gg_pd_zfbyq_pt_sdogeom>(pt);
        }

        [Execution(150)]
        public static void Gg_pd_dlzjjt_pt_sdogeom(object sender, DeleteSymbolEventArgs e)
        {
            ErasedEntityArgs eea = (ErasedEntityArgs) sender;
            UpdateCommon(eea);
            var n = DBManager.Instance.GetEntity<Gg_pd_dlzjjt_n>(o => o.G3E_FID == eea.G3e_fid);
            n.EntityState = eea.Erased
                                ? n.EntityState == EntityState.Insert ? EntityState.InsertDelete : EntityState.Delete
                                : n.EntityState == EntityState.Insert ? EntityState.InsertUpdate : EntityState.Update;
            var pt = DBManager.Instance.GetEntity<Gg_pd_dlzjjt_pt_sdogeom>(o => o.G3E_FID == eea.G3e_fid);
            pt.EntityState = n.EntityState;
            DBManager.Instance.Update<Gg_pd_dlzjjt_n>(n);
            DBManager.Instance.Update<Gg_pd_dlzjjt_pt_sdogeom>(pt);
        }

        [Execution(151)]
        public static void Gg_pd_dlzdt_pt_sdogeom(object sender, DeleteSymbolEventArgs e)
        {
            ErasedEntityArgs eea = (ErasedEntityArgs) sender;
            UpdateCommon(eea);
            var n = DBManager.Instance.GetEntity<Gg_pd_dlzdt_n>(o => o.G3E_FID == eea.G3e_fid);
            n.EntityState = eea.Erased
                                ? n.EntityState == EntityState.Insert ? EntityState.InsertDelete : EntityState.Delete
                                : n.EntityState == EntityState.Insert ? EntityState.InsertUpdate : EntityState.Update;
            var pt = DBManager.Instance.GetEntity<Gg_pd_dlzdt_pt_sdogeom>(o => o.G3E_FID == eea.G3e_fid);
            pt.EntityState = n.EntityState;
            DBManager.Instance.Update<Gg_pd_dlzdt_n>(n);
            DBManager.Instance.Update<Gg_pd_dlzdt_pt_sdogeom>(pt);
        }

        [Execution(152)]
        public static void Gg_pd_dlq_pt_sdogeom(object sender, DeleteSymbolEventArgs e)
        {
            ErasedEntityArgs eea = (ErasedEntityArgs) sender;
            UpdateCommon(eea);
            var n = DBManager.Instance.GetEntity<Gg_pd_dlq_n>(o => o.G3E_FID == eea.G3e_fid);
            n.EntityState = eea.Erased
                                ? n.EntityState == EntityState.Insert ? EntityState.InsertDelete : EntityState.Delete
                                : n.EntityState == EntityState.Insert ? EntityState.InsertUpdate : EntityState.Update;
            var pt = DBManager.Instance.GetEntity<Gg_pd_dlq_pt_sdogeom>(o => o.G3E_FID == eea.G3e_fid);
            pt.EntityState = n.EntityState;
            DBManager.Instance.Update<Gg_pd_dlq_n>(n);
            DBManager.Instance.Update<Gg_pd_dlq_pt_sdogeom>(pt);
        }

        [Execution(154)]
        public static void Gg_pd_zsbyq_pt_sdogeom(object sender, DeleteSymbolEventArgs e)
        {
            //ErasedEntityArgs eea = (ErasedEntityArgs)sender;
            //UpdateCommon(eea);
            //var n = DBManager.Instance.GetEntity<Gg_pd_zfdlt_n>(o => o.G3E_FID == eea.G3e_fid);
            //n.EntityState = eea.Erased ? n.EntityState == EntityState.Insert ? EntityState.InsertDelete : EntityState.Delete : n.EntityState == EntityState.Insert ? EntityState.InsertUpdate : EntityState.Update;
            //var pt = DBManager.Instance.GetEntity<Gg_pd_zfdlt_pt_sdogeom>(o => o.G3E_FID == eea.G3e_fid);
            //pt.EntityState = n.EntityState;
            //DBManager.Instance.Update<Gg_pd_zfdlt_n>(n);
            //DBManager.Instance.Update<Gg_pd_zfdlt_pt_sdogeom>(pt);
        }

        [Execution(171)]
        public static void Gg_pd_zdfdj_pt_sdogeom(object sender, DeleteSymbolEventArgs e)
        {
            ErasedEntityArgs eea = (ErasedEntityArgs) sender;
            UpdateCommon(eea);
            var n = DBManager.Instance.GetEntity<Gg_pd_zdfdj_n>(o => o.G3E_FID == eea.G3e_fid);
            n.EntityState = eea.Erased
                                ? n.EntityState == EntityState.Insert ? EntityState.InsertDelete : EntityState.Delete
                                : n.EntityState == EntityState.Insert ? EntityState.InsertUpdate : EntityState.Update;
            var pt = DBManager.Instance.GetEntity<Gg_pd_zdfdj_pt_sdogeom>(o => o.G3E_FID == eea.G3e_fid);
            pt.EntityState = n.EntityState;
            DBManager.Instance.Update<Gg_pd_zdfdj_n>(n);
            DBManager.Instance.Update<Gg_pd_zdfdj_pt_sdogeom>(pt);
        }

        [Execution(172)]
        public static void Gg_pd_ddj_pt_sdogeom(object sender, DeleteSymbolEventArgs e)
        {
            ErasedEntityArgs eea = (ErasedEntityArgs) sender;
            UpdateCommon(eea);
            var n = DBManager.Instance.GetEntity<Gg_pd_dlzdt_n>(o => o.G3E_FID == eea.G3e_fid);
            n.EntityState = eea.Erased
                                ? n.EntityState == EntityState.Insert ? EntityState.InsertDelete : EntityState.Delete
                                : n.EntityState == EntityState.Insert ? EntityState.InsertUpdate : EntityState.Update;
            var pt = DBManager.Instance.GetEntity<Gg_pd_ddj_pt_sdogeom>(o => o.G3E_FID == eea.G3e_fid);
            pt.EntityState = n.EntityState;
            DBManager.Instance.Update<Gg_pd_dlzdt_n>(n);
            DBManager.Instance.Update<Gg_pd_ddj_pt_sdogeom>(pt);
        }

        [Execution(173)]
        public static void Gg_pd_zfjddz_pt_sdogeom(object sender, DeleteSymbolEventArgs e)
        {
            ErasedEntityArgs eea = (ErasedEntityArgs) sender;
            UpdateCommon(eea);
            UpdateConnection(eea);
            var n = DBManager.Instance.GetEntity<Gg_pd_zfjddz_n>(o => o.G3E_FID == eea.G3e_fid);
            n.EntityState = eea.Erased
                                ? n.EntityState == EntityState.Insert ? EntityState.InsertDelete : EntityState.Delete
                                : n.EntityState == EntityState.Insert ? EntityState.InsertUpdate : EntityState.Update;
            var pt = DBManager.Instance.GetEntity<Gg_pd_zfjddz_pt_sdogeom>(o => o.G3E_FID == eea.G3e_fid);
            pt.EntityState = n.EntityState;
            DBManager.Instance.Update<Gg_pd_zfjddz_n>(n);
            DBManager.Instance.Update<Gg_pd_zfjddz_pt_sdogeom>(pt);
        }

        [Execution(174)]
        public static void Gg_pd_wgbc_pt_sdogeom(object sender, DeleteSymbolEventArgs e)
        {
            //ErasedEntityArgs eea = (ErasedEntityArgs)sender;
            //UpdateCommon(eea);
            //var n = DBManager.Instance.GetEntity<Gg_pd_zdfdj_n>(o => o.G3E_FID == eea.G3e_fid);
            //n.EntityState = eea.Erased ? n.EntityState == EntityState.Insert ? EntityState.InsertDelete : EntityState.Delete : n.EntityState == EntityState.Insert ? EntityState.InsertUpdate : EntityState.Update;
            //var pt = DBManager.Instance.GetEntity<Gg_pd_zdfdj_pt_sdogeom>(o => o.G3E_FID == eea.G3e_fid);
            //pt.EntityState = n.EntityState;
            //DBManager.Instance.Update<Gg_pd_zdfdj_n>(n);
            //DBManager.Instance.Update<Gg_pd_zdfdj_pt_sdogeom>(pt);
        }

        [Execution(175)]
        public static void Gg_pd_dll_pt_sdogeom(object sender, DeleteSymbolEventArgs e)
        {
            ErasedEntityArgs eea = (ErasedEntityArgs) sender;
            UpdateCommon(eea);
            var n = DBManager.Instance.GetEntity<Gg_pd_dll_n>(o => o.G3E_FID == eea.G3e_fid);
            n.EntityState = eea.Erased
                                ? n.EntityState == EntityState.Insert ? EntityState.InsertDelete : EntityState.Delete
                                : n.EntityState == EntityState.Insert ? EntityState.InsertUpdate : EntityState.Update;
            var pt = DBManager.Instance.GetEntity<Gg_pd_dll_pt_sdogeom>(o => o.G3E_FID == eea.G3e_fid);
            pt.EntityState = n.EntityState;
            DBManager.Instance.Update<Gg_pd_dll_n>(n);
            DBManager.Instance.Update<Gg_pd_dll_pt_sdogeom>(pt);
        }

        [Execution(176)]
        public static void Gg_pd_jdk_pt_sdogeom(object sender, DeleteSymbolEventArgs e)
        {
            ErasedEntityArgs eea = (ErasedEntityArgs) sender;
            UpdateCommon(eea);
            var n = DBManager.Instance.GetEntity<Gg_pd_jdk_n>(o => o.G3E_FID == eea.G3e_fid);
            n.EntityState = eea.Erased
                                ? n.EntityState == EntityState.Insert ? EntityState.InsertDelete : EntityState.Delete
                                : n.EntityState == EntityState.Insert ? EntityState.InsertUpdate : EntityState.Update;
            var pt = DBManager.Instance.GetEntity<Gg_pd_jdk_pt_sdogeom>(o => o.G3E_FID == eea.G3e_fid);
            pt.EntityState = n.EntityState;
            DBManager.Instance.Update<Gg_pd_jdk_n>(n);
            DBManager.Instance.Update<Gg_pd_jdk_pt_sdogeom>(pt);
        }

        [Execution(177)]
        public static void Gg_pd_blq_pt_sdogeom(object sender, DeleteSymbolEventArgs e)
        {
            ErasedEntityArgs eea = (ErasedEntityArgs) sender;
            UpdateCommon(eea);
            var n = DBManager.Instance.GetEntity<Gg_pd_zdfdj_n>(o => o.G3E_FID == eea.G3e_fid);
            n.EntityState = eea.Erased
                                ? n.EntityState == EntityState.Insert ? EntityState.InsertDelete : EntityState.Delete
                                : n.EntityState == EntityState.Insert ? EntityState.InsertUpdate : EntityState.Update;
            var pt = DBManager.Instance.GetEntity<Gg_pd_zdfdj_pt_sdogeom>(o => o.G3E_FID == eea.G3e_fid);
            pt.EntityState = n.EntityState;
            DBManager.Instance.Update<Gg_pd_zdfdj_n>(n);
            DBManager.Instance.Update<Gg_pd_zdfdj_pt_sdogeom>(pt);
        }


        [Execution(181)]
        public static void Gg_jc_pwy_pt_sdogeom(object sender, DeleteSymbolEventArgs e)
        {

        }

        [Execution(182)]
        public static void Gg_jc_gzzsq_pt_sdogeom(object sender, DeleteSymbolEventArgs e)
        {

        }

        [Execution(201)]
        public static void Gg_gz_dg_pt_sdogeom(object sender, DeleteSymbolEventArgs e)
        {
            ErasedEntityArgs eea = (ErasedEntityArgs) sender;
            UpdateCommon(eea);
            var n = DBManager.Instance.GetEntity<Gg_gz_dg_n>(o => o.G3E_FID == eea.G3e_fid);
            n.EntityState = eea.Erased
                                ? n.EntityState == EntityState.Insert ? EntityState.InsertDelete : EntityState.Delete
                                : n.EntityState == EntityState.Insert ? EntityState.InsertUpdate : EntityState.Update;
            var pt = DBManager.Instance.GetEntity<Gg_gz_dg_pt_sdogeom>(o => o.G3E_FID == eea.G3e_fid);
            pt.EntityState = n.EntityState;
            DBManager.Instance.Update<Gg_gz_dg_n>(n);
            DBManager.Instance.Update<Gg_gz_dg_pt_sdogeom>(pt);
        }

        [Execution(307)]
        public static void Gg_pd_dyhgq_pt_sdogeom(object sender, DeleteSymbolEventArgs e)
        {
            ErasedEntityArgs eea = (ErasedEntityArgs) sender;
            UpdateCommon(eea);
            var n = DBManager.Instance.GetEntity<Gg_gz_dg_n>(o => o.G3E_FID == eea.G3e_fid);
            n.EntityState = eea.Erased
                                ? n.EntityState == EntityState.Insert ? EntityState.InsertDelete : EntityState.Delete
                                : n.EntityState == EntityState.Insert ? EntityState.InsertUpdate : EntityState.Update;
            var pt = DBManager.Instance.GetEntity<Gg_gz_dg_pt_sdogeom>(o => o.G3E_FID == eea.G3e_fid);
            pt.EntityState = n.EntityState;
            DBManager.Instance.Update<Gg_gz_dg_n>(n);
            DBManager.Instance.Update<Gg_gz_dg_pt_sdogeom>(pt);
        }

        [Execution(309)]
        public static void Gg_pd_gydj_pt_sdogeom(object sender, DeleteSymbolEventArgs e)
        {
            ErasedEntityArgs eea = (ErasedEntityArgs)sender;
            UpdateCommon(eea);
            var n = DBManager.Instance.GetEntity<Gg_pd_gydj_n>(o => o.G3E_FID == eea.G3e_fid);
            n.EntityState = eea.Erased ? n.EntityState == EntityState.Insert ? EntityState.InsertDelete : EntityState.Delete : n.EntityState == EntityState.Insert ? EntityState.InsertUpdate : EntityState.Update;
            var pt = DBManager.Instance.GetEntity<Gg_pd_gydj_pt_sdogeom>(o => o.G3E_FID == eea.G3e_fid);
            pt.EntityState = n.EntityState;
            DBManager.Instance.Update<Gg_pd_gydj_n>(n);
            DBManager.Instance.Update<Gg_pd_gydj_pt_sdogeom>(pt);
        }

        /******************************************************/

        [Execution(73)]
        public static void Gg_pd_dldy_pt_sdogeom(object sender, DeleteSymbolEventArgs e)
        {
            ErasedEntityArgs eea = (ErasedEntityArgs) sender;
            UpdateCommon(eea);
            var n = DBManager.Instance.GetEntity<Gg_pd_dldy_n>(o => o.G3E_FID == eea.G3e_fid);
            n.EntityState = eea.Erased
                                ? n.EntityState == EntityState.Insert ? EntityState.InsertDelete : EntityState.Delete
                                : n.EntityState == EntityState.Insert ? EntityState.InsertUpdate : EntityState.Update;
            var pt = DBManager.Instance.GetEntity<Gg_pd_dldy_pt_sdogeom>(o => o.G3E_FID == eea.G3e_fid);
            pt.EntityState = n.EntityState;
            DBManager.Instance.Update<Gg_pd_dldy_n>(n);
            DBManager.Instance.Update<Gg_pd_dldy_pt_sdogeom>(pt);
        }

        [Execution(74)]
        public static void Gg_pd_dyzd_pt_sdogeom(object sender, DeleteSymbolEventArgs e)
        {
            ErasedEntityArgs eea = (ErasedEntityArgs) sender;
            UpdateCommon(eea);
            var n = DBManager.Instance.GetEntity<Gg_pd_dyzd_n>(o => o.G3E_FID == eea.G3e_fid);
            n.EntityState = eea.Erased
                                ? n.EntityState == EntityState.Insert ? EntityState.InsertDelete : EntityState.Delete
                                : n.EntityState == EntityState.Insert ? EntityState.InsertUpdate : EntityState.Update;
            var pt = DBManager.Instance.GetEntity<Gg_pd_dyzd_pt_sdogeom>(o => o.G3E_FID == eea.G3e_fid);
            pt.EntityState = n.EntityState;
            DBManager.Instance.Update<Gg_pd_dyzd_n>(n);
            DBManager.Instance.Update<Gg_pd_dyzd_pt_sdogeom>(pt);
        }

        [Execution(155)]
        public static void Gg_pd_dykg_pt_sdogeom(object sender, DeleteSymbolEventArgs e)
        {
            ErasedEntityArgs eea = (ErasedEntityArgs) sender;
            UpdateCommon(eea);
            UpdateConnection(eea);
            var n = DBManager.Instance.GetEntity<Gg_pd_dykg_n>(o => o.G3E_FID == eea.G3e_fid);
            n.EntityState = eea.Erased
                                ? n.EntityState == EntityState.Insert ? EntityState.InsertDelete : EntityState.Delete
                                : n.EntityState == EntityState.Insert ? EntityState.InsertUpdate : EntityState.Update;
            var pt = DBManager.Instance.GetEntity<Gg_pd_dykg_pt_sdogeom>(o => o.G3E_FID == eea.G3e_fid);
            pt.EntityState = n.EntityState;
            DBManager.Instance.Update<Gg_pd_dykg_n>(n);
            DBManager.Instance.Update<Gg_pd_dykg_pt_sdogeom>(pt);
        }

        [Execution(158)]
        public static void Gg_pd_dyzjjt_pt_sdogeom(object sender, DeleteSymbolEventArgs e)
        {
            ErasedEntityArgs eea = (ErasedEntityArgs) sender;
            UpdateCommon(eea);
            var n = DBManager.Instance.GetEntity<Gg_pd_dyzjjt_n>(o => o.G3E_FID == eea.G3e_fid);
            n.EntityState = eea.Erased
                                ? n.EntityState == EntityState.Insert ? EntityState.InsertDelete : EntityState.Delete
                                : n.EntityState == EntityState.Insert ? EntityState.InsertUpdate : EntityState.Update;
            var pt = DBManager.Instance.GetEntity<Gg_pd_dyzjjt_pt_sdogeom>(o => o.G3E_FID == eea.G3e_fid);
            pt.EntityState = n.EntityState;
            DBManager.Instance.Update<Gg_pd_dyzjjt_n>(n);
            DBManager.Instance.Update<Gg_pd_dyzjjt_pt_sdogeom>(pt);
        }

        [Execution(159)]
        public static void Gg_pd_cbx_pt_sdogeom(object sender, DeleteSymbolEventArgs e)
        {
            ErasedEntityArgs eea = (ErasedEntityArgs) sender;
            UpdateCommon(eea);
            var n = DBManager.Instance.GetEntity<Gg_pd_cbx_n>(o => o.G3E_FID == eea.G3e_fid);
            n.EntityState = eea.Erased
                                ? n.EntityState == EntityState.Insert ? EntityState.InsertDelete : EntityState.Delete
                                : n.EntityState == EntityState.Insert ? EntityState.InsertUpdate : EntityState.Update;
            var pt = DBManager.Instance.GetEntity<Gg_pd_cbx_pt_sdogeom>(o => o.G3E_FID == eea.G3e_fid);
            pt.EntityState = n.EntityState;
            DBManager.Instance.Update<Gg_pd_cbx_n>(n);
            DBManager.Instance.Update<Gg_pd_cbx_pt_sdogeom>(pt);
        }

        [Execution(169)]
        public static void Gg_pd_dydlt_pt_sdogeom(object sender, DeleteSymbolEventArgs e)
        {
            ErasedEntityArgs eea = (ErasedEntityArgs) sender;
            UpdateCommon(eea);
            var n = DBManager.Instance.GetEntity<Gg_pd_dydlt_n>(o => o.G3E_FID == eea.G3e_fid);
            n.EntityState = eea.Erased
                                ? n.EntityState == EntityState.Insert ? EntityState.InsertDelete : EntityState.Delete
                                : n.EntityState == EntityState.Insert ? EntityState.InsertUpdate : EntityState.Update;
            var pt = DBManager.Instance.GetEntity<Gg_pd_dydlt_pt_sdogeom>(o => o.G3E_FID == eea.G3e_fid);
            pt.EntityState = n.EntityState;
            DBManager.Instance.Update<Gg_pd_dydlt_n>(n);
            DBManager.Instance.Update<Gg_pd_dydlt_pt_sdogeom>(pt);
        }

        [Execution(215)]
        public static void Gg_gz_dydg_pt_sdogeom(object sender, DeleteSymbolEventArgs e)
        {
            ErasedEntityArgs eea = (ErasedEntityArgs) sender;
            UpdateCommon(eea);
            UpdateGnwz(eea);
            var n = DBManager.Instance.GetEntity<Gg_gz_dydg_n>(o => o.G3E_FID == eea.G3e_fid);
            n.EntityState = eea.Erased
                                ? n.EntityState == EntityState.Insert ? EntityState.InsertDelete : EntityState.Delete
                                : n.EntityState == EntityState.Insert ? EntityState.InsertUpdate : EntityState.Update;
            var pt = DBManager.Instance.GetEntity<Gg_gz_dydg_pt_sdogeom>(o => o.G3E_FID == eea.G3e_fid);
            pt.EntityState = n.EntityState;
            DBManager.Instance.Update<Gg_gz_dydg_n>(n);
            DBManager.Instance.Update<Gg_gz_dydg_pt_sdogeom>(pt);
        }

        [Execution(218)]
        public static void Gg_gz_dyzc_pt_sdogeom(object sender, DeleteSymbolEventArgs e)
        {
            ErasedEntityArgs eea = (ErasedEntityArgs) sender;
            UpdateCommon(eea);
            var n = DBManager.Instance.GetEntity<Gg_gz_dyzc_n>(o => o.G3E_FID == eea.G3e_fid);
            n.EntityState = eea.Erased
                                ? n.EntityState == EntityState.Insert ? EntityState.InsertDelete : EntityState.Delete
                                : n.EntityState == EntityState.Insert ? EntityState.InsertUpdate : EntityState.Update;
            var pt = DBManager.Instance.GetEntity<Gg_gz_dyzc_pt_sdogeom>(o => o.G3E_FID == eea.G3e_fid);
            pt.EntityState = n.EntityState;
            DBManager.Instance.Update<Gg_gz_dyzc_n>(n);
            DBManager.Instance.Update<Gg_gz_dyzc_pt_sdogeom>(pt);
        }


        /// <summary>
        /// 更新公共属性
        /// </summary>
        /// <param name="eea"></param>
        public static void UpdateCommon(ErasedEntityArgs eea)
        {
            if (eea.G3e_fno == 159 || eea.G3e_fno == 160 || eea.G3e_fno == 152
                || eea.G3e_fno == 153 || eea.G3e_fno == 154) return;
            var cn = DBManager.Instance.GetEntity<Common_n>(o => o.G3E_FID == eea.G3e_fid);
            cn.EntityState = eea.Erased ? EntityState.Delete : EntityState.Update;
            DBManager.Instance.Update<Common_n>(cn);
        }

        /// <summary>
        /// 更新连接属性
        /// </summary>
        /// <param name="eea"></param>
        public static void UpdateConnection(ErasedEntityArgs eea)
        {
            var cn = DBManager.Instance.GetEntity<Connectivity_n>(o => o.G3E_FID == eea.G3e_fid);
            cn.EntityState = eea.Erased ? EntityState.Delete : EntityState.Update;
            DBManager.Instance.Update<Connectivity_n>(cn);
        }

        /// <summary>
        /// 更新功能位置
        /// </summary>
        /// <param name="eea"></param>
        public static void UpdateGnwz(ErasedEntityArgs eea)
        {
            var cn = DBManager.Instance.GetEntity<Gg_pd_gnwzmc_n>(o => o.G3E_FID == eea.G3e_fid);
            cn.EntityState = eea.Erased ? EntityState.Delete : EntityState.Update;
            DBManager.Instance.Update<Gg_pd_gnwzmc_n>(cn);
        }
    }
}
