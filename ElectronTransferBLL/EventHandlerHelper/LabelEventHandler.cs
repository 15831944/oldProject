using System;
using System.Drawing;
using System.Linq;
using ElectronTransferDal.Common;
using ElectronTransferDal.Factory;
using ElectronTransferDal.HelperEventArgs;
using ElectronTransferFramework;
using ElectronTransferModel.Base;
using ElectronTransferModel.Config;
using ElectronTransferModel.V9_4;
using cadColor= Autodesk.AutoCAD.Colors.Color;

namespace ElectronTransferBll.EventHandlerHelper
{

    [ExecutionContainer(typeof(LabelExecutionFactory))]
    public class LabelEventHandler 
    {
        [Execution(40)]
        public static void Gg_pd_dlhgq_lb_sdogeom(object sender, LabelEventArgs e)
        {
            var electronSymbol = sender as ElectronSymbol;
            var dbSymbolEntry = GetDBSymbolEntry(electronSymbol.G3E_FNO);
            //var lbcn =  e.xmlDB.GetEntity<Common_n>((long) sender);
            //if (lbcn == null) return;
            //e.lbText = lbcn.SBMC;
            var str = dbSymbolEntry.Label.SingleOrDefault(o => o.CNO == electronSymbol.GetValue("CNO"));
            GetG3e_textstyle(str.Name,e);
        }

        [Execution(75)]
        public static void Gg_pd_bdcx_lb_sdogeom(object sender, LabelEventArgs e)
        {
            const string str = "变低出线标注";
            var lbcn = e.xmlDB.GetEntity<Common_n>((long)sender);
            if (lbcn == null) return;
            GetG3e_textstyle(str, e);
        }

        [Execution(77)]
        public static void Gg_pd_dlfxx_lb_sdogeom(object sender, LabelEventArgs e)
        {
            const string str = "电缆分线箱标注";
            var lbcn =  e.xmlDB.GetEntity<Common_n>((long)sender);
            if (lbcn == null) return;
            e.lbText = lbcn.SBMC;
            GetG3e_textstyle(str, e);
        }

        [Execution(79)]
        public static void Gg_pd_dymx_lb_sdogeom(object sender, LabelEventArgs e)
        {
            const string str = "低压母线标注";
            var lbcn =  e.xmlDB.GetEntity<Common_n>((long)sender);
            if (lbcn == null) return;
            e.lbText = lbcn.SBBM;
            GetG3e_textstyle(str, e);
        }
        
        [Execution(80)]
        public static void Gg_pd_bqx_lb_sdogeom(object sender, LabelEventArgs e)
        {
            const string str = "表前线标注";
            var lbcn =  e.xmlDB.GetEntity<Common_n>((long)sender);
            if (lbcn == null) return;
            e.lbText = lbcn.CD_SSXL;
            GetG3e_textstyle(str, e);
        }

        [Execution(81)]
        public static void Gg_pd_dyg_lb_sdogeom(object sender, LabelEventArgs e)
        {
            const string str = "低压柜标注";
            var lbcn =  e.xmlDB.GetEntity<Common_n>((long)sender);
            if (lbcn == null) return;
            e.lbText = lbcn.SBMC;
            GetG3e_textstyle(str, e);
        }

        //[Execution(82)]
        //public static void Gg_jx_gyb_pt_sdogeom(object sender, LabelEventArgs e)
        //{
        //    Common_n lbcn;
        //    if (e.Status)
        //    {
        //        lbcn = e.xmlDB.GetEntity<Common_n>((long)sender);
        //    }
        //    else
        //    {
        //        lbcn = DBManager.Instance.GetEntity<Common_n>((long)sender);
        //    }
        //    if (lbcn != null)
        //    {
        //    }
        //}

        [Execution(84)]
        public static void Gg_pd_cjlg_lb_sdogeom(object sender, LabelEventArgs e)
        {
            const string str = "计量柜标注";
            var lbcn =  e.xmlDB.GetEntity<Common_n>((long)sender);
            if (lbcn == null) return;
            e.lbText = lbcn.SBMC;
            GetG3e_textstyle(str, e);
        }

        [Execution(85)]
        public static void Gg_pd_ptg_lb_sdogeom(object sender, LabelEventArgs e)
        {
            const string str = "PT柜标注";
            var lbcn =  e.xmlDB.GetEntity<Common_n>((long)sender);
            if (lbcn == null) return;
            e.lbText = lbcn.SBMC;
            GetG3e_textstyle(str, e);
        }

        [Execution(86)]
        public static void Gg_jc_dcjg_pt_sdogeom(object sender, LabelEventArgs e)
        {
            const string str = "电操机构标注";
            var lbcn =  e.xmlDB.GetEntity<Common_n>((long)sender);
            if (lbcn == null) return;
            e.lbText = lbcn.SBMC;
            GetG3e_textstyle(str, e);
        }

        [Execution(90)]
        public static void Gg_jc_dtu_pt_sdogeom(object sender, LabelEventArgs e)
        {
            const string str = "DTU标注";
            var lbcn =  e.xmlDB.GetEntity<Common_n>((long)sender);
            if (lbcn == null) return;
            e.lbText = lbcn.SBMC;
            GetG3e_textstyle(str, e);
        }
        [Execution(140)]
        public static void Gg_pd_dl_lb_sdogeom(object sender, LabelEventArgs e)
        {
            const string str = "10KV电缆沿布图标注";
            var lbcn =  e.xmlDB.GetEntity<Common_n>((long)sender);
            if (lbcn == null) return;
            e.lbText = lbcn.CD_SSXL;
            GetG3e_textstyle(str, e);
        }
        [Execution(1401)]
        public static void Gg_pd_dl_lb1_sdogeom(object sender, LabelEventArgs e)
        { 
            const string str = "10KV电缆沿布图标注(设备编码)";
            var lbcn =  e.xmlDB.GetEntity<Common_n>((long)sender);
            if (lbcn == null) return;
            e.lbText = lbcn.SBBM;
            GetG3e_textstyle(str, e);
        }
        [Execution(1402)]
        public static void GG_PD_DLZX_LB(object sender, LabelEventArgs e)
        { 
            var g3e_fid = (long) sender;
            const string str = "10KV电缆杂项标注";
            var n = e.xmlDB.GetEntity<Gg_pd_dl_n>(g3e_fid);
            if (n == null) return;
            //[DLLX] -[JMJ] / [CD]m 
            if (!string.IsNullOrEmpty(n.DLLX) && !string.IsNullOrEmpty(n.JMJ.ToString()) && !string.IsNullOrEmpty(n.CD.ToString()))
            {
                e.lbText = string.Format("{0}-{1}/{2}m", n.DLLX, n.JMJ, n.CD);
            }
            GetG3e_textstyle(str, e);
        }
        [Execution(141)]
        public static void Gg_pd_dx_lb_sdogeom(object sender, LabelEventArgs e)
        { 
            var g3e_fid = (long)sender;
            const string str = "10KV导线标注";
            var lbcn = e.xmlDB.GetEntity<Common_n>(g3e_fid);
            if (lbcn == null) return;
            e.lbText = lbcn.CD_SSXL;
            GetG3e_textstyle(str, e);
        }
        [Execution(1411)]
        public static void Gg_pd_dx_lb1_sdogeom(object sender, LabelEventArgs e)
        { 
            var g3e_fid = (long)sender;
            const string str = "10KV导线沿布图标注(设备编码)";
            var lbcn = e.xmlDB.GetEntity<Common_n>(g3e_fid);
            if (lbcn == null) return;
            e.lbText = lbcn.SBBM;
            GetG3e_textstyle(str, e);
        }
        [Execution(1412)]
        public static void Gg_pd_dx_lb2_sdogeom(object sender, LabelEventArgs e)
        { 
            var g3e_fid = (long)sender;
            const string str = "10KV导线杂项标注";
            var n = e.xmlDB.GetEntity<Gg_pd_dx_n>(g3e_fid);
            if (n == null) return;
            //[DXGG] -[JMJ] /[CD]m 
            if(!string.IsNullOrEmpty(n.DXGG) && !string.IsNullOrEmpty(n.JMJ.ToString()) && !string.IsNullOrEmpty(n.CD.ToString()))
            {
                e.lbText = string.Format("{0}-{1}/{2}m", n.DXGG, n.JMJ, n.CD);
            }
            GetG3e_textstyle(str, e);
        }

        [Execution(142)]
        public static void Gg_pd_gydf_lb_sdogeom(object sender, LabelEventArgs e)
        { 
            var g3e_fid = (long)sender;
            const string str = "公用电房标注";
            var lbcn = e.xmlDB.GetEntity<Common_n>(g3e_fid);
            if (lbcn == null) return;
            e.lbText = lbcn.SBMC;
            GetG3e_textstyle(str, e);
        }
        [Execution(1421)]
        public static void Gg_pd_gydf_lb1_sdogeom(object sender, LabelEventArgs e)
        {
            var g3e_fid = (long)sender;
            const string str = "公用电房沿布图标注(设备编码)";
            var lbcn = e.xmlDB.GetEntity<Common_n>(g3e_fid);
            if (lbcn == null) return;
            e.lbText = lbcn.SBBM;
            GetG3e_textstyle(str, e);
        }

        [Execution(143)]
        public static void Gg_pd_zfmx_lb_sdogeom(object sender, LabelEventArgs e)
        {
            var g3e_fid = (long)sender;
            const string str = "站房母线标注";
            var lbcn = e.xmlDB.GetEntity<Common_n>(g3e_fid);
            if (lbcn == null) return;
            e.lbText = lbcn.CD_SSXL;
            GetG3e_textstyle(str, e);
        }
        [Execution(1431)]
        public static void Gg_pd_zfmx_lb1_sdogeom(object sender, LabelEventArgs e)
        {
            var g3e_fid = (long)sender;
            const string str = "站房母线沿布图标注(设备编码)";
            var lbcn = e.xmlDB.GetEntity<Common_n>(g3e_fid);
            if (lbcn == null) return;
            e.lbText = lbcn.SBBM;
            GetG3e_textstyle(str, e);
        }

        [Execution(144)]
        public static void Gg_pd_zfyx_lb_sdogeom(object sender, LabelEventArgs e)
        {
            const string str = "站房引线标注";
            var lbcn =  e.xmlDB.GetEntity<Common_n>((long)sender);
            if (lbcn == null) return;
            e.lbText = lbcn.SBMC;
            GetG3e_textstyle(str, e);
        }

        [Execution(145)]
        public static void Gg_pd_zfllkg_lb_sdogeom(object sender, LabelEventArgs e)
        {
            const string str = "站房联络开关标注";
            var lbcn =  e.xmlDB.GetEntity<Common_n>((long)sender);
            if (lbcn == null) return;
            e.lbText = lbcn.SBMC;
            GetG3e_textstyle(str, e);
        }

        [Execution(146)]
        public static void Gg_pd_zfhwg_lb_sdogeom(object sender, LabelEventArgs e)
        {
            var g3e_fid = (long)sender;
            const string str = "10KV开关标注";
            var lbcn = e.xmlDB.GetEntity<Common_n>(g3e_fid);

            if (lbcn == null) return;
            e.lbText = lbcn.SBMC;
            GetG3e_textstyle(str, e);
        }
        [Execution(1461)]
        public static void Gg_pd_zfhwg_lb1_sdogeom(object sender, LabelEventArgs e)
        {
            var g3e_fid = (long)sender;
            const string str = "10kV开关标注(设备编码)";
            var lbcn = e.xmlDB.GetEntity<Common_n>(g3e_fid);

            if (lbcn == null) return;
            e.lbText = lbcn.YXBH;
            GetG3e_textstyle(str, e);
        }

        [Execution(147)]
        public static void Gg_pd_zfdlt_lb_sdogeom(object sender, LabelEventArgs e)
        {
            const string str = "站房电缆头标注";
            var lbcn =  e.xmlDB.GetEntity<Common_n>((long)sender);
            if (lbcn == null) return;
            e.lbText = lbcn.SBMC;
            GetG3e_textstyle(str, e);
        }

        [Execution(148)]
        public static void Gg_pd_zfbyq_pt_sdogeom(object sender, LabelEventArgs e)
        {
            ////var g3e_fid = (long)sender;
            ////const string str = "站房变压器标注";
            ////var lbcn = e.xmlDB.GetEntity<Common_n>(g3e_fid);
            ////if (lbcn == null) return;
            ////e.lbText = lbcn.CD_XHGE;
            ////GetG3e_textstyle(str, e);
            //var electronSymbol = sender as ElectronSymbol;
            //var dbSymbolEntry = GetDBSymbolEntry(electronSymbol.G3E_FNO);
            ////var lbcn =  e.xmlDB.GetEntity<Common_n>((long) sender);
            ////if (lbcn == null) return;
            ////e.lbText = lbcn.SBMC;
            //var str = dbSymbolEntry.Label.SingleOrDefault(o => o.CNO == electronSymbol.GetValue("CNO"));
            //GetG3e_textstyle(str.Name, e);
            GetLabelTextStyle(sender, e);
        }


        [Execution(1481)]
        public static void Gg_pd_zfbyq_pt_sdogeom1(object sender, LabelEventArgs e)
        {
            var g3e_fid = (long)sender;
            const string str = "变压器标注(名称)";
            var lbcn = e.xmlDB.GetEntity<Common_n>(g3e_fid);
            if (lbcn == null) return;
            e.lbText = lbcn.SBMC;
            GetG3e_textstyle(str, e);
        }
        [Execution(1482)]
        public static void Gg_pd_zfbyq_pt_sdogeom2(object sender, LabelEventArgs e)
        {
            var g3e_fid = (long)sender;
            const string str = "变压器沿布图标注(用户用电号)";
            var lbcn = e.xmlDB.GetEntity<Gg_pd_zfbyq_n>(g3e_fid);
            if (lbcn == null) return;
            e.lbText = lbcn.BZ;
            GetG3e_textstyle(str, e);
        }

        [Execution(149)]
        public static void Gg_pd_xssb_lb_sdogeom(object sender, LabelEventArgs e)
        {
            const string str = "箱式设备标注";
            var lbcn =  e.xmlDB.GetEntity<Common_n>((long)sender);
            if (lbcn == null) return;
            e.lbText = lbcn.SBMC;
            GetG3e_textstyle(str, e);
        }

        [Execution(150)]
        public static void Gg_pd_dlzjjt_lb_sdogeom(object sender, LabelEventArgs e)
        {
            const string str = "10KV电缆中间接头标注";
            var lbcn =  e.xmlDB.GetEntity<Common_n>((long)sender);
            if (lbcn == null) return;
            e.lbText = lbcn.SBMC;
            GetG3e_textstyle(str, e);
        }

        [Execution(151)]
        public static void Gg_pd_dlzdt_lb_sdogeom(object sender, LabelEventArgs e)
        {
            const string str = "10KV电缆终端头标注";
            var lbcn =  e.xmlDB.GetEntity<Common_n>((long)sender);
            if (lbcn == null) return;
            e.lbText = lbcn.SBMC;
            GetG3e_textstyle(str, e);
        }

        [Execution(156)]
        public static void Gg_pd_dydx_lb_sdogeom(object sender, LabelEventArgs e)
        {
            const string str = "低压导线标注";
          
                var lbn = e.xmlDB.GetEntity<Gg_pd_dydx_n>((long)sender);
                var lbcn = e.xmlDB.GetEntity<Common_n>((long)sender);
          
            if (lbn != null && lbcn != null)
            {
                e.lbText = string.Format("{0}-{1}/{2}", lbcn.CD_XHGE, lbn.DXJMJ, lbn.DXCD);
                GetG3e_textstyle(str, e);
            }
        }

        [Execution(157)]
        public static void Gg_pd_dydl_lb_sdogeom(object sender, LabelEventArgs e)
        {
            Common_n lbcn;
            Gg_pd_dydl_n lbn;
            const string str = "低压电缆标注";
          
                lbn = e.xmlDB.GetEntity<Gg_pd_dydl_n>((long)sender);
                lbcn = e.xmlDB.GetEntity<Common_n>((long)sender);
            
            if (lbn != null && lbcn != null)
            {
                e.lbText = string.Format("{0}-{1}/{2}", lbcn.CD_XHGE, lbn.DLJM, lbn.CD);
                GetG3e_textstyle(str, e);
            }
        }

        [Execution(163)]
        public static void Gg_pd_zydf_lb_sdogeom(object sender, LabelEventArgs e)
        {
            const string str = "专用电房标注";
            var lbcn =  e.xmlDB.GetEntity<Common_n>((long)sender);
            if (lbcn == null) return;
            e.lbText = lbcn.SBMC;
            GetG3e_textstyle(str,e);
        }

        
        [Execution(171)]
        public static void Gg_pd_zdfdj_lb_sdogeom(object sender, LabelEventArgs e)
        {
            const string str = "用户自带发电机标注";
            var lbcn =  e.xmlDB.GetEntity<Common_n>((long)sender);
            if (lbcn == null) return;
            e.lbText = lbcn.SBBM;
            GetG3e_textstyle(str, e);
        }

        [Execution(172)]
        public static void Gg_pd_ddj_lb_sdogeom(object sender, LabelEventArgs e)
        {
            const string str = "电动机标注";
            var lbcn =  e.xmlDB.GetEntity<Common_n>((long)sender);
            if (lbcn == null) return;
            e.lbText = lbcn.SBBM;
            GetG3e_textstyle(str, e);
        }

        [Execution(173)]
        public static void Gg_pd_zfjddz_lb_sdogeom(object sender, LabelEventArgs e)
        {
            const string str = "站房接地刀闸标注";
            var lbcn =  e.xmlDB.GetEntity<Common_n>((long)sender);
            if (lbcn == null) return;
            e.lbText = lbcn.YXBH;
            GetG3e_textstyle(str, e);
        }

        [Execution(174)]
        public static void Gg_pd_wgbc_lb_sdogeom(object sender, LabelEventArgs e)
        {
            const string str = "无功补偿器标注";
            var lbcn =  e.xmlDB.GetEntity<Common_n>((long)sender);
            if (lbcn == null) return;
            e.lbText = lbcn.SBBM;
            GetG3e_textstyle(str, e);
        }

        [Execution(176)]
        public static void Gg_pd_jdk_lb_sdogeom(object sender, LabelEventArgs e)
        {
            const string str = "解断口标注";
            var lbcn =  e.xmlDB.GetEntity<Common_n>((long)sender);
            if (lbcn == null) return;
            e.lbText = lbcn.SBMC;
            GetG3e_textstyle(str, e);
        }

        [Execution(177)]
        public static void Gg_pd_blq_lb_sdogeom(object sender, LabelEventArgs e)
        {
            const string str = "避雷器标注";
            var lbcn =  e.xmlDB.GetEntity<Common_n>((long)sender);
            if (lbcn == null) return;
            e.lbText = lbcn.SBBM;
            GetG3e_textstyle(str, e);
        }

        [Execution(180)]
        public static void Gg_jc_ftu_pt_sdogeom(object sender, LabelEventArgs e)
        {
            const string str = "FTU标注";
            var lbcn =  e.xmlDB.GetEntity<Common_n>((long)sender);
            if (lbcn == null) return;
            e.lbText = lbcn.SBMC;
            GetG3e_textstyle(str, e);
        }

        [Execution(181)]
        public static void Gg_jc_pwy_lb_sdogeom(object sender, LabelEventArgs e)
        {
            const string str = "配网仪标注";
            var lbn = e.xmlDB.GetEntity<Gg_jc_pwy_n>((long)sender);
            if (lbn == null) return;
            e.lbText = string.Format("{0}最小、最大、平均电流{1}{2}{3}最小、最大、平均电压：{4}{5}{6}最小、最大、平均有功:{7}{8}{9}最小、最大、平均无功：{10}{11}{12}", lbn.PBMC, lbn.ZXDL, lbn.ZDDL, lbn.PJDL, lbn.ZXDY, lbn.ZDDY, lbn.PJDY, lbn.ZXYGDL, lbn.ZDYGDL, lbn.PJYGDL, lbn.ZXWGDL, lbn.ZDWGDL, lbn.PJWGDL);
            GetG3e_textstyle(str, e);
        }

        [Execution(182)]
        public static void Gg_jc_gzzsq_pt_sdogeom(object sender, LabelEventArgs e)
        {
            const string str = "故障指示器标注";
            var lbcn =  e.xmlDB.GetEntity<Common_n>((long)sender);
            if (lbcn == null) return;
            e.lbText = lbcn.SBMC;
            GetG3e_textstyle(str, e);
        }

        [Execution(201)]
        public static void Gg_gz_dg_lb_sdogeom(object sender, LabelEventArgs e)
        {
            var g3e_fid = (long)sender;
            const string str = "电杆标注";
            var lbcn = e.xmlDB.GetEntity<Gg_gz_dg_n>(g3e_fid);
            if (lbcn == null) return;
            e.lbText = lbcn.GH;
            GetG3e_textstyle(str, e);
        }

        [Execution(302)]
        public static void Gg_pd_dypdf_lb_sdogeom(object sender, LabelEventArgs e)
        {
            const string str = "低压配电房标注";
            var lbcn =  e.xmlDB.GetEntity<Common_n>((long)sender);
            if (lbcn == null) return;
            e.lbText = lbcn.SBMC;
            GetG3e_textstyle(str, e);
        }

        [Execution(307)]
        public static void Gg_pd_dyhgq_lb_sdogeom(object sender, LabelEventArgs e)
        {
            const string str = "电压互感器标注";
            var lbcn =  e.xmlDB.GetEntity<Common_n>((long)sender);
            if (lbcn == null) return;
            e.lbText = lbcn.SBMC;
            GetG3e_textstyle(str, e);
        }

        [Execution(309)]
        public static void Gg_pd_gydj_lb_sdogeom(object sender, LabelEventArgs e)
        {
            const string str = "高压电机标注";
            var lbcn =  e.xmlDB.GetEntity<Common_n>((long)sender);
            if (lbcn == null) return;
            e.lbText = lbcn.SBMC;
            GetG3e_textstyle(str, e);
        }

        /******************************************************/

        [Execution(73)]
        public static void Gg_pd_dldy_lb_sdogeom(object sender, LabelEventArgs e)
        {
            const string str = "低压断连标注";
            var lbcn =e.xmlDB.GetEntity<Gg_pd_dldy_n>((long)sender);

            if (lbcn == null) return;
            e.lbText = lbcn.JDBH;
            GetG3e_textstyle(str, e);
        }

        [Execution(74)]
        public static void Gg_pd_dyzd_lb_sdogeom(object sender, LabelEventArgs e)
        {
            const string str = "低压终端标注";
            var lbn = e.xmlDB.GetEntity<Gg_pd_dyzd_n>((long)sender);
            if (lbn == null) return;
            e.lbText = lbn.JDBH;
            GetG3e_textstyle(str, e);
        }

        [Execution(155)]
        public static void Gg_pd_dykg_lb_sdogeom(object sender, LabelEventArgs e)
        {
            var g3e_fid = (long)sender;
            const string str = "低压开关标注";
            var lbcn = e.xmlDB.GetEntity<Common_n>(g3e_fid);
            if (lbcn == null) return;
            e.lbText = lbcn.SBMC;
            GetG3e_textstyle(str, e);
        }
        [Execution(1551)]
        public static void Gg_pd_dykg_lb1_sdogeom(object sender, LabelEventArgs e)
        {
            var g3e_fid = (long)sender;
            const string str = "低压开关沿布图标注(设备编码)";
            var lbcn = e.xmlDB.GetEntity<Common_n>(g3e_fid);
            if (lbcn == null) return;
            e.lbText = lbcn.SBBM;
            GetG3e_textstyle(str, e);
        }

        [Execution(158)]
        public static void Gg_pd_dyzjjt_lb_sdogeom(object sender, LabelEventArgs e)
        {
            const string str = "低压电缆中间接头标注";
            var lbcn =  e.xmlDB.GetEntity<Common_n>((long)sender);
            if (lbcn == null) return;
            e.lbText = lbcn.SBMC;
            GetG3e_textstyle(str, e);
        }

        [Execution(159)]
        public static void Gg_pd_cbx_pt_sdogeom(object sender, LabelEventArgs e)
        {
            const string str = "低压集中抄表箱标注";
            var lbn =e.xmlDB.GetEntity<Gg_pd_cbx_n>((long)sender) ;
            if (lbn == null) return;
            e.lbText = lbn.HH;
            GetG3e_textstyle(str, e);
        }

        [Execution(169)]
        public static void Gg_pd_dydlt_lb_sdogeom(object sender, LabelEventArgs e)
        {
            const string str = "低压电缆头标注";
            var lbcn =  e.xmlDB.GetEntity<Common_n>((long)sender);
            if (lbcn == null) return;
            e.lbText = lbcn.SBMC;
            GetG3e_textstyle(str, e);
        }

        [Execution(175)]
        public static void Gg_pd_dll_lb_sdogeom(object sender, LabelEventArgs e)
        {
            const string str = "10KV断连标注";
            var lbcn =  e.xmlDB.GetEntity<Common_n>((long)sender);
            if (lbcn == null) return;
            e.lbText = lbcn.SBBM;
            GetG3e_textstyle(str, e);
        }

        [Execution(205)]
        public static void Gg_gz_rj_pt_sdogeom(object sender, LabelEventArgs e)
        {
            const string str = "人井标注";
            var lbcn =  e.xmlDB.GetEntity<Common_n>((long)sender);
            if (lbcn == null) return;
            e.lbText = lbcn.SBMC;
            GetG3e_textstyle(str, e);
        }

        [Execution(215)]
        public static void Gg_gz_dydg_lb_sdogeom(object sender, LabelEventArgs e)
        {
            try
            {
                const string str = "低压电杆标注";
                var lbcn =  e.xmlDB.GetEntity<Common_n>((long)sender);
                if (lbcn == null)
                {
                }
                else
                {
                    e.lbText = lbcn.SBBM;
                    GetG3e_textstyle(str, e);
                }
            }
            catch (Exception ex)
            {
                LogManager.Instance.Error(ex);
            }
        }

        [Execution(218)]
        public static void Gg_gz_dyzc_lb_sdogeom(object sender, LabelEventArgs e)
        {
            const string str = "低压支撑标注";
            var lbn = e.Status ? e.xmlDB.GetEntity<Gg_gz_dyzc_n>((long)sender) : DBManager.Instance.GetEntity<Gg_gz_dyzc_n>((long)sender);
            if (lbn == null) return;
            e.lbText = string.Format("{0}{1}", lbn.CD_JCLX, lbn.JCXH);
            GetG3e_textstyle(str, e);
        }

        [Execution(320)]
        public static void Gg_pd_jszz_lb_sdogeom(object sender, LabelEventArgs e)
        {
            const string str = "监视装置标注";
            var lbcn =  e.xmlDB.GetEntity<Common_n>((long)sender);
            if (lbcn == null) return;
            e.lbText = lbcn.SBMC;
            GetG3e_textstyle(str, e);
        }

        /**************************新增*****************************/
        [Execution(71)]
        public static void Gg_pd_cqtg_lb_sdogeom(object sender, LabelEventArgs e)
        {
            const string str = "穿墙套管标注";
            var lbcn =  e.xmlDB.GetEntity<Common_n>((long)sender);
            if (lbcn == null) return;
            e.lbText = lbcn.SBMC;
            GetG3e_textstyle(str, e);
        }

        [Execution(72)]
        public static void Gg_pd_jdgh_lb_sdogeom(object sender, LabelEventArgs e)
        {
            const string str = "接地挂环标注";
            var lbcn =  e.xmlDB.GetEntity<Common_n>((long)sender);
            if (lbcn == null) return;
            e.lbText = lbcn.SBMC;
            GetG3e_textstyle(str, e);
        }

        [Execution(76)]
        public static void Gg_pd_gkjlg_lb_sdogeom(object sender, LabelEventArgs e)
        {
            const string str = "关口计量柜标注";
            var lbcn =  e.xmlDB.GetEntity<Common_n>((long)sender);
            if (lbcn == null) return;
            e.lbText = lbcn.SBMC;
            GetG3e_textstyle(str, e);
        }

        [Execution(183)]
        public static void Gg_jc_qyjcy_lb_sdogeom(object sender, LabelEventArgs e)
        {
            const string str = "气压监测仪标注";
            var lbcn =  e.xmlDB.GetEntity<Common_n>((long)sender);
            if (lbcn == null) return;
            e.lbText = lbcn.SBMC;
            GetG3e_textstyle(str, e);
        }

        [Execution(184)]
        public static void Gg_jc_yyjcy_lb_sdogeom(object sender, LabelEventArgs e)
        {
            const string str = "油压监测仪标注";
            var lbcn =  e.xmlDB.GetEntity<Common_n>((long)sender);
            if (lbcn == null) return;
            e.lbText = lbcn.SBMC;
            GetG3e_textstyle(str, e);
        }

        [Execution(185)]
        public static void Gg_jc_dyjcy_lb_sdogeom(object sender, LabelEventArgs e)
        {
            const string str = "电压监测仪标注";
            var lbcn =  e.xmlDB.GetEntity<Common_n>((long)sender);
            if (lbcn == null) return;
            e.lbText = lbcn.SBMC;
            GetG3e_textstyle(str, e);
        }

        [Execution(186)]
        public static void Gg_jc_ldkgx_lb_sdogeom(object sender, LabelEventArgs e)
        {
            const string str = "路灯开关箱标注";
            var lbcn =  e.xmlDB.GetEntity<Common_n>((long)sender);
            if (lbcn == null) return;
            e.lbText = lbcn.SBMC;
            GetG3e_textstyle(str, e);
        }

        [Execution(188)]
        public static void Gg_jc_fk_lb_sdogeom(object sender, LabelEventArgs e)
        {
            const string str = "负控标注";
            var lbn = e.Status ? e.xmlDB.GetEntity<Gg_jc_fk_n>((long) sender) : DBManager.Instance.GetEntity<Gg_jc_fk_n>((long) sender);
            if (lbn == null) return;
            e.lbText = string.Format("{0}最大电流：{1}最小电流{2}平均电流：{3}最大电压：{4}最小电压：{5}平均电压：{6}", lbn.YHH, lbn.ZDDL, lbn.ZXDL, lbn.PJDL, lbn.ZDDL, lbn.ZXDL, lbn.PJDY);
            GetG3e_textstyle(str, e);
        }

        private static void GetG3e_textstyle(string g3e_UserName, LabelEventArgs e)
        {
            const double increment = 0.05;
            try
            {
                var textstyle = CDDBManager.Instance.GetEntity<G3e_textstyle>(o => o.G3E_USERNAME.ToUpper() == g3e_UserName);
                if (textstyle != null)
                {
                    if (textstyle.G3E_COLOR != null)
                        e.color = cadColor.FromColor(Color.FromArgb((int)textstyle.G3E_COLOR));
                    if (textstyle.G3E_SIZE != null)
                        e.lbHeight = (double)textstyle.G3E_SIZE + increment;
                }else
                {
                    e.lbHeight = 0.2 + increment;
                    e.color = cadColor.FromRgb(0, 0, 0);
                }
            }
            catch
            {
                e.lbHeight = 0.2 + increment;
                e.color = cadColor.FromRgb(0, 0, 0);
            }
        }
        private static DBSymbolEntry GetDBSymbolEntry(long g3e_fno)
        {
            return SimpleDBSymbolsConfig.Instance.DBSymbol.SingleOrDefault(o => o.G3E_FNO == g3e_fno);
        }

        private static LabelEventArgs GetLabelTextStyle(object sender, LabelEventArgs e)
        {
            var electronSymbol = sender as ElectronSymbol;
            var dbSymbolEntry = GetDBSymbolEntry(electronSymbol.G3E_FNO);
            //var lbcn =  e.xmlDB.GetEntity<Common_n>((long) sender);
            //if (lbcn == null) return;
            //e.lbText = lbcn.SBMC;
            var str = dbSymbolEntry.Label.SingleOrDefault(o => o.CNO == electronSymbol.GetValue("CNO"));
            GetG3e_textstyle(str.Name, e);
            return e;
        }
    }
}
