using System;
using System.Linq;
using ElectronTransferDal.Common;
using ElectronTransferDal.Factory;
using ElectronTransferDal.HelperEventArgs;
using ElectronTransferDal.Query;
using ElectronTransferFramework;
using ElectronTransferModel;
using ElectronTransferModel.Base;
using ElectronTransferModel.Config;
using ElectronTransferModel.V9_4;
using CADColor = Autodesk.AutoCAD.Colors.Color;
using ElectronTransferDal.AutoGeneration;


namespace ElectronTransferBll.EventHandlerHelper
{

    [ExecutionContainer(typeof (SymbolExecutionFactory))]
    public class SymbolEventHandler
    {
        private const string Zero = "0",
                             One = "1",
                             Two = "2",
                             Three = "3",
                             Four = "4",
                             Five = "5",
                             Six = "6",
                             Seven = "7",
                             Eight = "8",
                             Nine = "9",
                             Ten = "10";


        #region 线符号

        [Execution(140)]
        public static void Gg_pd_dl_ln_sdogeom(object sender, SymbolEventArgs e)
        {
            e.lineWidth = CABLEManager._lineWidth002;
            var g3e_fid = (sender as ElectronSymbol).G3E_FID;
             if (e.electronSymbol.KxType) return;
            var com = DBEntityFinder.Instance.GetCommon_n(e.DBManager,g3e_fid);
            var con = DBEntityFinder.Instance.GetConnectivity_n(e.DBManager,g3e_fid);
            var n = e.DBManager.GetEntity<Gg_pd_dl_n>(g3e_fid);
            if (com == null || n == null)
                return;

            e.SBMC = com.SBMC;
            var str = "10kV电缆符号";
            if (!string.IsNullOrEmpty(com.CD_SMZQ))
            {
                //是否带电
                var sfdd = con == null ? "" : con.CD_SFDD;
                str = DBEntityFinder.Instance.GetUserNameBySMZQorSFDD(str, com.CD_SMZQ, sfdd);

                if (n.CXD == "粗")
                {
                    str += "-" + n.CXD;
                }
            }
            DBEntityFinder.Instance.GetLineStyle(e, str);
        }

        //0803下午
        [Execution(141)]
        public static void Gg_pd_dx_ln_sdogeom(object sender, SymbolEventArgs e)
        {
            e.lineWidth = CABLEManager._lineWidth002;
            var g3e_fid = (sender as ElectronSymbol).G3E_FID;
             if (e.electronSymbol.KxType) return;
            var com = DBEntityFinder.Instance.GetCommon_n(e.DBManager,g3e_fid);
            var con = DBEntityFinder.Instance.GetConnectivity_n(e.DBManager,g3e_fid);
            var cx = e.DBManager.GetEntity<Gg_pd_dx_n>(g3e_fid);
            if (com == null) return;

            e.SBMC = com.SBMC;
            var str = "10kV导线符号";

            if (!string.IsNullOrEmpty(com.CD_SMZQ))
            {
                //是否带电
                var sfdd = con == null ? "" : con.CD_SFDD;
                str = DBEntityFinder.Instance.GetUserNameBySMZQorSFDD(str, com.CD_SMZQ, sfdd);

                if (cx != null && cx.CXD == "粗")
                {
                    str += "-" + cx.CXD;
                }
            }
            DBEntityFinder.Instance.GetLineStyle(e, str);
        }

        [Execution(143)]
        public static void Gg_pd_zfmx_ln_sdogeom(object sender, SymbolEventArgs e)
        {
            e.lineWidth = CABLEManager._lineWidth004;
            var g3e_fid = (sender as ElectronSymbol).G3E_FID;
             if (e.electronSymbol.KxType) return;
            var com = DBEntityFinder.Instance.GetCommon_n(e.DBManager,g3e_fid);
            var con = DBEntityFinder.Instance.GetConnectivity_n(e.DBManager,g3e_fid);
            if (com == null || con == null) return;

            e.SBMC = com.SBMC;
            string str = "站房母线符号";

            str = DBEntityFinder.Instance.GetUserNameBySMZQorSFDD(str, com.CD_SMZQ, con.CD_SFDD);
            if (str.IndexOf("-投运") > 0)
            {
                if (!string.IsNullOrEmpty(com.CD_DYDJ))
                {
                    if (com.CD_DYDJ == "0.4")
                    {
                        str = "站房母线符号-400V投运";
                    }
                    else if (com.CD_DYDJ == "10")
                    {
                        str = "站房母线符号-10kV投运";
                    }
                }
            }
            DBEntityFinder.Instance.GetLineStyle(e, str);
        }

        //0804

        [Execution(144)]
        public static void Gg_pd_zfyx_ln_sdogeom(object sender, SymbolEventArgs e)
        {
            e.lineWidth = CABLEManager._lineWidth001;

            var g3e_fid = (sender as ElectronSymbol).G3E_FID;
             if (e.electronSymbol.KxType) return;
            var com = DBEntityFinder.Instance.GetCommon_n(e.DBManager,g3e_fid);
            var con = DBEntityFinder.Instance.GetConnectivity_n(e.DBManager,g3e_fid);
            if (com == null || con == null) return;

            //
            e.SBMC = com.SBMC;
            string str = "站房引线符号";

            str = DBEntityFinder.Instance.GetUserNameBySMZQorSFDD(str, com.CD_SMZQ, con.CD_SFDD);
            if (str.IndexOf("-投运") > 0)
            {
                if (!string.IsNullOrEmpty(com.CD_DYDJ))
                {
                    if (com.CD_DYDJ == "0.4")
                    {
                        str = "站房引线符号-400V投运";
                    }
                    else if (com.CD_DYDJ == "10")
                    {
                        str = "站房引线符号-10kV投运";
                    }
                }
            }
            DBEntityFinder.Instance.GetLineStyle(e, str);
        }

        [Execution(79)]
        public static void Gg_pd_dymx_ln_sdogeom(object sender, SymbolEventArgs e)
        {
            e.lineWidth = CABLEManager._lineWidth004;

            var g3e_fid = (sender as ElectronSymbol).G3E_FID;
             if (e.electronSymbol.KxType) return;
            var com = DBEntityFinder.Instance.GetCommon_n(e.DBManager,g3e_fid);
            if (com != null)
                e.SBMC = com.SBMC;
        }

        [Execution(156)]
        public static void Gg_pd_dydx_ln_sdogeom(object sender, SymbolEventArgs e)
        {
            e.lineWidth = CABLEManager._lineWidth002;
            var g3e_fid = (sender as ElectronSymbol).G3E_FID;
             if (e.electronSymbol.KxType) return;
            var com = DBEntityFinder.Instance.GetCommon_n(e.DBManager,g3e_fid);
            if (com == null) return;

            e.SBMC = com.SBMC;
            const string str = "低压导线符号";
            DBEntityFinder.Instance.GetLineStyle(str, com, e);
        }

        [Execution(157)]
        public static void Gg_pd_dydl_ln_sdogeom(object sender, SymbolEventArgs e)
        {
            e.lineWidth = CABLEManager._lineWidth002;

            var g3e_fid = (sender as ElectronSymbol).G3E_FID;
             if (e.electronSymbol.KxType) return;
            var com = DBEntityFinder.Instance.GetCommon_n(e.DBManager,g3e_fid);
            if (com == null) return;

            e.SBMC = com.SBMC;
            const string str = "低压电缆符号";
            DBEntityFinder.Instance.GetLineStyle(str, com, e);
        }

        [Execution(80)]
        public static void Gg_pd_bqx_ln_sdogeom(object sender, SymbolEventArgs e)
        {
            e.lineWidth = CABLEManager._lineWidth002;

            var g3e_fid = (sender as ElectronSymbol).G3E_FID;
             if (e.electronSymbol.KxType) return;
            var com = DBEntityFinder.Instance.GetCommon_n(e.DBManager,g3e_fid);
            if (com == null) return;

            e.SBMC = com.SBMC;
            const string str = "表前线符号";
            DBEntityFinder.Instance.GetLineStyle(str, com, e);
        }

        [Execution(75)]
        public static void Gg_pd_bdcx_pt_sdogeom(object sender, SymbolEventArgs e)
        {
            e.lineWidth = CABLEManager._lineWidth002;

            var g3e_fid = (sender as ElectronSymbol).G3E_FID;
             if (e.electronSymbol.KxType) return;
            var com = DBEntityFinder.Instance.GetCommon_n(e.DBManager,g3e_fid);
            if (com != null)
                e.SBMC = com.SBMC;
        }

        #endregion


        #region 点符号

        [Execution(40)]
        public static void Gg_pd_dlhgq_pt_sdogeom(object sender, SymbolEventArgs e)
        {
            var g3e_fid = (sender as ElectronSymbol).G3E_FID;
             if (e.electronSymbol.KxType) return;
            var com = DBEntityFinder.Instance.GetCommon_n(e.DBManager,g3e_fid);
            if (com != null)
            {
                e.SBMC = com.SBMC;
                const string str = "电流互感器符号";
                e.color = DBEntityFinder.Instance.GetPointStyle(str, com, e.color);
            }
        }

        [Execution(82)]
        public static void Gg_jx_gyb_pt_sdogeom(object sender, SymbolEventArgs e)
        {
            var g3e_fid = (sender as ElectronSymbol).G3E_FID;
             if (e.electronSymbol.KxType) return;
            var cn = e.DBManager.GetEntity<Gg_pd_gyb_n>(g3e_fid);
            var com = DBEntityFinder.Instance.GetCommon_n(e.DBManager,g3e_fid);
            var con = DBEntityFinder.Instance.GetConnectivity_n(e.DBManager,g3e_fid);
            if (cn != null && com != null && con!=null)
            {
                e.SBMC = com.SBMC;

                string str = "高压表符号";
                if (com.CD_SMZQ.Equals("停运"))
                {
                    str += "-停运";
                }
                else if (com.CD_SMZQ.Equals("待投运"))
                {
                    str += "-待投运";
                }
                else
                {
                    var sfdd = con.CD_SFDD;
                    str = string.Format("{0}-{1}-{2}", str, com.CD_SMZQ, sfdd);
                }
                e.color = DBEntityFinder.Instance.GetPointStyle(e.color, str);
            }
        }

        [Execution(84)]
        public static void Gg_pd_cjlg_pt_sdogeom(object sender, SymbolEventArgs e)
        {
            var g3e_fid = (sender as ElectronSymbol).G3E_FID;
            if (!e.electronSymbol.KxType)
            {
                var com = DBEntityFinder.Instance.GetCommon_n(e.DBManager,g3e_fid);
                var n = e.DBManager.GetEntity<Gg_pd_cjlg_n>(g3e_fid);
                var con = e.DBManager.GetEntity<Connectivity_n>(g3e_fid);
                if (com != null && n != null && con!=null)
                {
                    switch (n.SBLX)
                    {
                        case "抽屉式计量柜":
                            e.SymbolType = One;
                            break;
                        case "固定式计量柜":
                            e.SymbolType = Zero;
                            break;
                        case "计量箱":
                            e.SymbolType = One;
                            break;
                        default :
                            e.SymbolType = Zero;
                            break;
                    }

                    var str = string.Format("{0}符号", n.SBLX);
                    if (!string.IsNullOrEmpty(com.CD_SMZQ))
                    {
                        if (str.Contains("停运"))
                        {
                            str += "-停运";
                        }
                        else
                            str += "-" + com.CD_SMZQ;
                        if (str.IndexOf("-投运") > 0)
                        {
                            var sfdd = con.CD_SFDD;
                            str = string.Format("{0}-{1}", str, sfdd);
                        }
                    }
                    e.color = DBEntityFinder.Instance.GetPointStyle(e.color, str);
                }
            }else
            {
                e.SymbolType = Zero;
            }
        }


        [Execution(175)]
        public static void Gg_pd_dll_pt_sdogeom(object sender, SymbolEventArgs e)
        {
            var g3e_fid = (sender as ElectronSymbol).G3E_FID;
             if (e.electronSymbol.KxType) return;
            var com = DBEntityFinder.Instance.GetCommon_n(e.DBManager,g3e_fid);
            var con = DBEntityFinder.Instance.GetConnectivity_n(e.DBManager,g3e_fid);
            if (com != null && con != null)
            {
                e.SBMC = com.SBMC;
                var str = "10kV断连符号";

                str = DBEntityFinder.Instance.GetUserNameBySMZQorSFDD(str, com.CD_SMZQ, con.CD_SFDD);
                e.color = DBEntityFinder.Instance.GetPointStyle(e.color, str);
            }
        }

        [Execution(85)]
        public static void Gg_pd_ptg_pt_sdogeom(object sender, SymbolEventArgs e)
        {
            var g3e_fid = (sender as ElectronSymbol).G3E_FID;
             if (e.electronSymbol.KxType) return;
            var com = DBEntityFinder.Instance.GetCommon_n(e.DBManager,g3e_fid);
            var con = DBEntityFinder.Instance.GetConnectivity_n(e.DBManager,g3e_fid);
            if (com != null && con!=null)
            {
                e.SBMC = com.SBMC;

                string str = "PT柜符号";
                if (!string.IsNullOrEmpty(com.CD_SMZQ))
                {
                    str += "-" + com.CD_SMZQ;
                }
                if (str.IndexOf("-投运") > 0)
                {
                    if (!string.IsNullOrEmpty(con.CD_SFDD))
                    {
                        str += "-" + con.CD_SFDD;
                    }
                }
                e.color = DBEntityFinder.Instance.GetPointStyle(e.color, str);
            }
        }

        [Execution(86)]
        public static void Gg_jc_dcjg_pt_sdogeom(object sender, SymbolEventArgs e)
        {
            var g3e_fid = (sender as ElectronSymbol).G3E_FID;
             if (e.electronSymbol.KxType) return;
            var com = DBEntityFinder.Instance.GetCommon_n(e.DBManager,g3e_fid);
            if (com != null)
            {
                e.SBMC = com.SBMC;

                const string str = "电操机构符号";
                e.color = DBEntityFinder.Instance.GetPointStyle(str, com, e.color);
            }
        }

        [Execution(90)]
        public static void Gg_jc_dtu_pt_sdogeom(object sender, SymbolEventArgs e)
        {
            var g3e_fid = (sender as ElectronSymbol).G3E_FID;
             if (e.electronSymbol.KxType) return;
            var com = DBEntityFinder.Instance.GetCommon_n(e.DBManager,g3e_fid);
            if (com == null) return;
            e.SBMC = com.SBMC;
            var str = "DTU符号";
            if (!string.IsNullOrEmpty(com.CD_SMZQ))
            {
                str += "-" + com.CD_SMZQ;
            }
            e.color = DBEntityFinder.Instance.GetPointStyle(e.color, str);
        }

        [Execution(145)]
        public static void Gg_pd_zfllkg_pt_sdogeom(object sender, SymbolEventArgs e)
        {
            var g3e_fid = (sender as ElectronSymbol).G3E_FID;
             if (e.electronSymbol.KxType) return;
            var com = DBEntityFinder.Instance.GetCommon_n(e.DBManager,g3e_fid);
            if (com == null) return;
            e.SBMC = com.SBMC;
            string str = "站房联络开关符号";

            if (!string.IsNullOrEmpty(com.CD_SMZQ))
            {
                str += "-" + com.CD_SMZQ;
            }
            var zfllkg = DBEntityFinder.Instance.GetConnectivity_n(e.DBManager,g3e_fid);
            if (zfllkg != null && !string.IsNullOrEmpty(zfllkg.CD_DQZT))
            {
                if (str.IndexOf("-投运") > 0)
                {
                    if (!string.IsNullOrEmpty(com.CD_DYDJ))
                    {
                        if (com.CD_DYDJ == "0.4")
                        {
                            str += "-400V" + zfllkg.CD_DQZT;
                        }
                        else if (com.CD_DYDJ == "10")
                        {
                            str += "-10kV" + zfllkg.CD_DQZT;
                        }
                    }
                }
                else if (str.IndexOf("-停运") > 0)
                {
                    str += "-" + zfllkg.CD_DQZT;
                }
            }
            e.color = DBEntityFinder.Instance.GetPointStyle(e.color, str);
        }

        [Execution(146)]
        public static void Gg_pd_zfhwg_pt_sdogeom(object sender, SymbolEventArgs e)
        {
            var g3e_fid = (sender as ElectronSymbol).G3E_FID;
            if (!e.electronSymbol.KxType)
            {
                var cn = e.DBManager.GetEntity<Gg_pd_zfhwg_n>(g3e_fid);
                var gnwz = e.DBManager.GetEntity<Gg_pd_gnwzmc_n>(g3e_fid);
                var com = DBEntityFinder.Instance.GetCommon_n(e.DBManager,g3e_fid);
                var con = DBEntityFinder.Instance.GetConnectivity_n(e.DBManager,g3e_fid);
                if (com != null && cn != null && con != null)
                {
                    e.SBMC = com.SBMC;
                    switch (gnwz.GNWZ_FL)
                    {
                        case "断路器":
                        case "固定式断路器":
                            e.SymbolType = Zero;
                            break;
                        case "刀闸":
                            e.SymbolType = One;
                            break;
                        case "跌落式熔断器":
                            e.SymbolType = Two;
                            break;
                        case "断路器小车式":
                            e.SymbolType = Three;
                            break;
                        case "负荷开关":
                            e.SymbolType = Four;
                            break;
                        case "带熔丝负荷开关":
                        case "不带开关的手车柜":
                            e.SymbolType = Five;
                            break;
                        case "副柜":
                            e.SymbolType = Seven;
                            break;
                        case "简易开关柜":
                            e.SymbolType = Six;
                            break;
                        case "隔离开关":
                            e.SymbolType = Eight;
                            break;
                        case "自动化负荷开关":
                            e.SymbolType = Nine;
                            break;
                        case "自动化断路器":
                            e.SymbolType = Ten;
                            break;
                        default:
                            e.SymbolType = Zero;
                            break;
                    }
                    //副柜、简易开关柜例外
                    if (e.SymbolType == Seven || e.SymbolType == Six)
                    {
                    }
                    else
                    {
                        e.BreakerStatus = con.CD_DQZT == "闭合" ? "1" : "0";
                    }

                    var str = "10kV开关符号";
                    if (!string.IsNullOrEmpty(com.CD_SMZQ))
                    {
                        str += "-" + com.CD_SMZQ;
                    }
                    //只要是遇到停运或者不带电都是默认一种颜色
                    if (str.Contains("-停运") || con.CD_SFDD.Equals("不带电"))
                    {
                        str = string.Format("10kV开关符号-停运-{0}", con.CD_DQZT);
                    }
                    else if (str.Contains("投运"))
                    {
                        str = string.Format("{0}-{1}", str, con.CD_DQZT);
                    }
                    e.color = DBEntityFinder.Instance.GetPointStyle(e.color, str);
                }
            }
            else
            {
                e.SymbolType = Zero;
                e.BreakerStatus = "0";
            }
        }

        [Execution(147)]
        public static void Gg_pd_zfdlt_pt_sdogeom(object sender, SymbolEventArgs e)
        {
            var g3e_fid = (sender as ElectronSymbol).G3E_FID;
            if (!e.electronSymbol.KxType)
            {
                var cn = e.DBManager.GetEntity<Gg_pd_zfdlt_n>(g3e_fid);
                var com = DBEntityFinder.Instance.GetCommon_n(e.DBManager,g3e_fid);
                var con = DBEntityFinder.Instance.GetConnectivity_n(e.DBManager,g3e_fid);
                if (com != null && cn != null && con != null)
                {
                    e.SBMC = com.SBMC;
                    switch (cn.CD_SFSDL)
                    {
                        case "双电缆头":
                            e.SymbolType = One;
                            break;
                        default:
                            e.SymbolType = Zero;
                            break;
                    }
                    string str = "站房电缆头符号";
                    if (!string.IsNullOrEmpty(com.CD_SMZQ))
                    {
                        str = string.Format("{0}-{1}", str, com.CD_SMZQ);
                    }
                    var dlt = cn.CD_SFSDL == "双电缆头" ? "双头" : "单头";
                    //停运和不带电情况
                    if (str.Contains("停运") || con.CD_SFDD.Equals("不带电"))
                    {
                        str = string.Format("站房电缆头符号-停运-{0}", dlt);
                    }
                    else if (str.IndexOf("-投运") > 0)
                    {
                        if (!string.IsNullOrEmpty(com.CD_DYDJ) && !string.IsNullOrEmpty(cn.CD_SFSDL))
                        {
                            var dj = com.CD_DYDJ == "0.4" ? "400V" : (com.CD_DYDJ == "10" ? "10kV" : "");
                            str = string.Format("{0}-{1}{2}", str, dj, dlt);
                        }
                    }
                    else
                    {
                        if (!string.IsNullOrEmpty(com.CD_DYDJ))
                        {
                            str = string.Format("{0}-{1}", str, dlt);
                        }
                    }
                    e.color = DBEntityFinder.Instance.GetPointStyle(e.color, str);
                }
            }
            else
            {
                e.SymbolType = Zero;
            }
        }

        [Execution(148)]
        public static void Gg_pd_zfbyq_pt_sdogeom(object sender, SymbolEventArgs e)
        {
            var g3e_fid = (sender as ElectronSymbol).G3E_FID;
            if (!e.electronSymbol.KxType)
            {
                var tjn = e.DBManager.GetEntity<Gg_pd_zfbyq_n>(g3e_fid);
                var com = DBEntityFinder.Instance.GetCommon_n(e.DBManager,g3e_fid);
                var con = DBEntityFinder.Instance.GetConnectivity_n(e.DBManager,g3e_fid);
                if (tjn != null && com != null && con != null)
                {
                    e.SBMC = com.SBMC;
                    switch (tjn.CD_PBLB)
                    {
                        case "公变":
                            e.SymbolType = Zero;
                            break;
                        case "专变":
                            e.SymbolType = One;
                            break;
                        case "农变":
                            e.SymbolType = Two;
                            break;
                        case "专改公":
                            e.SymbolType = Three;
                            break;
                        case "其它":
                            e.SymbolType = Four;
                            break;
                        case "其他":
                            e.SymbolType = Four;
                            break;
                        default:
                            e.SymbolType = One;
                            break;
                    }
                    string str = "站房变压器符号";

                    str = DBEntityFinder.Instance.GetUserNameBySMZQorSFDD(str, com.CD_SMZQ, con.CD_SFDD);
                    var pblb = tjn.CD_PBLB.Replace("他", "它");
                    str +="-" + pblb;
                    e.color = DBEntityFinder.Instance.GetPointStyle(e.color, str);
                }
            }
            else
            {
                e.SymbolType = Zero;
            }
        }

        [Execution(150)]
        public static void Gg_pd_dlzjjt_pt_sdogeom(object sender, SymbolEventArgs e)
        {
            var g3e_fid = (sender as ElectronSymbol).G3E_FID;
             if (e.electronSymbol.KxType) return;
            var com = DBEntityFinder.Instance.GetCommon_n(e.DBManager,g3e_fid);
            var con = DBEntityFinder.Instance.GetConnectivity_n(e.DBManager,g3e_fid);
            if (com != null && con != null)
            {
                e.SBMC = com.SBMC;
                string str = "10kV电缆中间接头符号";

                str = DBEntityFinder.Instance.GetUserNameBySMZQorSFDD(str, com.CD_SMZQ, con.CD_SFDD);
                e.color = DBEntityFinder.Instance.GetPointStyle(e.color, str);
            }
        }

        [Execution(151)]
        public static void Gg_pd_dlzdt_pt_sdogeom(object sender, SymbolEventArgs e)
        {
            var g3e_fid = (sender as ElectronSymbol).G3E_FID;
            if (!e.electronSymbol.KxType)
            {
                var cn = e.DBManager.GetEntity<Gg_pd_dlzdt_n>(g3e_fid);
                var com = DBEntityFinder.Instance.GetCommon_n(e.DBManager,g3e_fid);
                var con = DBEntityFinder.Instance.GetConnectivity_n(e.DBManager,g3e_fid);
                if (com != null && cn != null && con != null)
                {
                    e.SBMC = com.SBMC;
                    switch (cn.ZDTXS)
                    {
                        case "双电缆头":
                            e.SymbolType = One;
                            break;
                        default:
                            e.SymbolType = Zero;
                            break;
                    }
                    string str = "10kV电缆终端头符号";
                    str = DBEntityFinder.Instance.GetUserNameBySMZQorSFDD(str, com.CD_SMZQ, con.CD_SFDD);
                    e.color = DBEntityFinder.Instance.GetPointStyle(e.color, str);
                }
            }
            else
            {
                e.SymbolType = Zero;
            }
        }

        [Execution(154)]
        public static void Gg_pd_zsbyq_pt_sdogeom(object sender, SymbolEventArgs e)
        {
            var g3e_fid = (sender as ElectronSymbol).G3E_FID;
            if (!e.electronSymbol.KxType)
            {
                var cn = e.DBManager.GetEntity<Gg_pd_zsbyq_n>(g3e_fid);
                var com = DBEntityFinder.Instance.GetCommon_n(e.DBManager,g3e_fid);
                if (cn != null && com != null)
                {
                    e.SBMC = com.SBMC;
                    switch (cn.CD_PBLB)
                    {
                        case "公变":
                            e.SymbolType = Zero;
                            break;
                        case "专变":
                            e.SymbolType = One;
                            break;
                        case "农变":
                            e.SymbolType = Two;
                            break;
                        case "专改公":
                            e.SymbolType = Three;
                            break;
                        default:
                            e.SymbolType = Zero;
                            break;
                    }
                    string str = "柱上变压器符号";
                    str += string.IsNullOrEmpty(com.CD_SMZQ) ? "" : "-" + com.CD_SMZQ;
                    str += string.IsNullOrEmpty(cn.CD_PBLB) ? "" : "-" + cn.CD_PBLB;
                    e.color = DBEntityFinder.Instance.GetPointStyle(e.color, str);
                }
            }
            else
            {
                e.SymbolType = Zero;
            }
        }

        [Execution(171)]
        public static void Gg_pd_zdfdj_pt_sdogeom(object sender, SymbolEventArgs e)
        {
            var g3e_fid = (sender as ElectronSymbol).G3E_FID;
             if (e.electronSymbol.KxType) return;
            var com = DBEntityFinder.Instance.GetCommon_n(e.DBManager,g3e_fid);
            if (com != null)
            {
                e.SBMC = com.SBMC;
                string str = "用户自带发电机符号";
                str += string.Format("-{0}", com.CD_SMZQ);
                e.color = DBEntityFinder.Instance.GetPointStyle(e.color, str);
            }
        }

        [Execution(172)]
        public static void Gg_pd_ddj_pt_sdogeom(object sender, SymbolEventArgs e)
        {
            var g3e_fid = (sender as ElectronSymbol).G3E_FID;
             if (e.electronSymbol.KxType) return;
            var com = DBEntityFinder.Instance.GetCommon_n(e.DBManager,g3e_fid);
            if (com != null)
            {
                e.SBMC = com.SBMC;

                string str = "电动机符号";
                str += string.Format("-{0}", com.CD_SMZQ);
                e.color = DBEntityFinder.Instance.GetPointStyle(e.color, str);
            }
        }

        [Execution(173)]
        public static void Gg_pd_zfjddz_pt_sdogeom(object sender, SymbolEventArgs e)
        {
            var g3e_fid = (sender as ElectronSymbol).G3E_FID;
            if (!e.electronSymbol.KxType)
            {
                var com = DBEntityFinder.Instance.GetCommon_n(e.DBManager,g3e_fid);
                var jddz = e.DBManager.GetEntity<Gg_pd_zfjddz_n>(g3e_fid);
                var con = e.DBManager.GetEntity<Connectivity_n>(g3e_fid);
                if (com != null && con != null && jddz!=null)
                {
                    e.SBMC = com.SBMC;

                    switch (jddz.SGDW)
                    {
                        case "新":
                            e.SymbolType = Zero;
                            break;
                        case "旧":
                            e.SymbolType = One;
                            break;
                        default:
                            e.SymbolType = Zero;
                            break;
                    }
                    e.BreakerStatus = con.CD_DQZT == "闭合" ? "1" : "0";

                    string str = "站房接地刀闸符号";

                    str = DBEntityFinder.Instance.GetUserNameBySMZQorSFDD(str, com.CD_SMZQ, con.CD_SFDD);
                    if (!string.IsNullOrEmpty(con.CD_DQZT))
                    {
                        str += "-" + con.CD_DQZT;
                    }
                    e.color = DBEntityFinder.Instance.GetPointStyle(e.color, str);
                }
            }
            else
                e.SymbolType = Zero;
        }

        [Execution(174)]
        public static void Gg_pd_wgbc_pt_sdogeom(object sender, SymbolEventArgs e)
        {

            var g3e_fid = (sender as ElectronSymbol).G3E_FID;
             if (e.electronSymbol.KxType) return;
            var com = DBEntityFinder.Instance.GetCommon_n(e.DBManager,g3e_fid);
            var con = DBEntityFinder.Instance.GetConnectivity_n(e.DBManager,g3e_fid);
            if (com != null && con!=null)
            {
                e.SBMC = com.SBMC;
                string str = "无功补偿器符号";
                str = DBEntityFinder.Instance.GetUserNameBySMZQorSFDD(str, com.CD_SMZQ, con.CD_SFDD);
                e.color = DBEntityFinder.Instance.GetPointStyle(e.color, str);
            }
        }

        [Execution(176)]
        public static void Gg_pd_jdk_pt_sdogeom(object sender, SymbolEventArgs e)
        {
            var g3e_fid = (sender as ElectronSymbol).G3E_FID;
             if (e.electronSymbol.KxType) return;
            var com = DBEntityFinder.Instance.GetCommon_n(e.DBManager,g3e_fid);
            var con = DBEntityFinder.Instance.GetConnectivity_n(e.DBManager,g3e_fid);
            if (com != null && con != null)
            {
                e.SBMC = com.SBMC;
                string str = "解断口符号";
                str = DBEntityFinder.Instance.GetUserNameBySMZQorSFDD(str, com.CD_SMZQ, con.CD_SFDD);
                e.color = DBEntityFinder.Instance.GetPointStyle(e.color, str);
            }
        }

        [Execution(177)]
        public static void Gg_pd_blq_pt_sdogeom(object sender, SymbolEventArgs e)
        {
            var g3e_fid = (sender as ElectronSymbol).G3E_FID;
            if (!e.electronSymbol.KxType)
            {
                var com = DBEntityFinder.Instance.GetCommon_n(e.DBManager, g3e_fid);
                var con = DBEntityFinder.Instance.GetConnectivity_n(e.DBManager, g3e_fid);
                if (com != null && con != null)
                {
                    e.SBMC = com.SBMC;
                    var azwz = GetAzwz(177, g3e_fid);
                    if (string.IsNullOrEmpty(azwz))
                        return;
                    switch (azwz)
                    {
                        case "房内":
                        case "户内":
                        case "箱式":
                            e.SymbolType = Zero;
                            break;
                        default:
                            e.SymbolType = One;
                            break;
                    }
                    string str = "避雷器符号";
                    if (!string.IsNullOrEmpty(com.CD_SMZQ))
                    {
                        if (azwz.Equals("柱上"))
                        {
                            str = string.Format("{0}{1}", azwz, str);
                        }
                        else if (azwz.Equals("户内") || azwz.Equals("房内"))
                        {
                            str = string.Format("房内{0}", str);
                        }
                        else
                        {
                            str = string.Format("{0}{1}", azwz, str);
                        }

                        //停运和不带电情况
                        if ((com.CD_SMZQ.Equals("停运") || con.CD_SFDD.Equals("不带电")))
                        {
                            str = string.Format("{0}-停运", str);
                        }
                        else if ((com.CD_SMZQ.Equals("停运") || con.CD_SFDD.Equals("不带电")))
                        {
                            str = string.Format("{0}-停运", str);
                        }
                        else
                            str += "-" + com.CD_SMZQ;
                    }
                    e.color = DBEntityFinder.Instance.GetPointStyle(e.color, str);
                }
            }
            else
            {
                e.SymbolType = Zero;
            }
        }

        [Execution(180)]
        public static void Gg_jc_ftu_pt_sdogeom(object sender, SymbolEventArgs e)
        {
            var g3e_fid = (sender as ElectronSymbol).G3E_FID;
             if (e.electronSymbol.KxType) return;
            var com = DBEntityFinder.Instance.GetCommon_n(e.DBManager,g3e_fid);
            if (com != null)
            {
                e.SBMC = com.SBMC;
                const string str = "FTU符号";
                e.color = DBEntityFinder.Instance.GetPointStyle(str, com, e.color);
            }
        }

        [Execution(181)]
        public static void Gg_jc_pwy_pt_sdogeom(object sender, SymbolEventArgs e)
        {
            var g3e_fid = (sender as ElectronSymbol).G3E_FID;
             if (e.electronSymbol.KxType) return;
            var com = DBEntityFinder.Instance.GetCommon_n(e.DBManager,g3e_fid);
            if (com != null)
            {
                e.SBMC = com.SBMC;
                const string str = "配网仪符号";
                e.color = DBEntityFinder.Instance.GetPointStyle(str, com, e.color);
            }
        }

        [Execution(182)]
        public static void Gg_jc_gzzsq_pt_sdogeom(object sender, SymbolEventArgs e)
        {
            var g3e_fid = (sender as ElectronSymbol).G3E_FID;
             if (e.electronSymbol.KxType) return;
            var com = DBEntityFinder.Instance.GetCommon_n(e.DBManager,g3e_fid);
            if (com != null)
            {
                e.SBMC = com.SBMC;
                const string str = "故障指示器符号";
                e.color = DBEntityFinder.Instance.GetPointStyle(str, com, e.color);
            }
        }

        [Execution(201)]
        public static void Gg_gz_dg_pt_sdogeom(object sender, SymbolEventArgs e)
        {
            var g3e_fid = (sender as ElectronSymbol).G3E_FID;
            if (!e.electronSymbol.KxType)
            {
                var com = DBEntityFinder.Instance.GetCommon_n(e.DBManager,g3e_fid);
                var cn = e.DBManager.GetEntity<Gg_gz_dg_n>(g3e_fid);
                if (com != null && cn != null)
                {
                    e.SBMC = com.SBMC;
                    switch (cn.CD_CLLB)
                    {
                        case "铁塔":
                        case "铁杆":
                            e.SymbolType = Two;
                            break;
                        //case "钢杆":
                        //    e.SymbolType = Zero;
                        //    break;
                        case "水泥杆":
                            e.SymbolType = One;
                            break;
                        default:
                            e.SymbolType = Zero;
                            break;
                    }
                    var str = "电杆符号";
                    str += string.IsNullOrEmpty(cn.CD_CLLB) ? "" : "-" + cn.CD_CLLB;
                    str += string.IsNullOrEmpty(com.CD_SMZQ) ? "" : "-" + com.CD_SMZQ;

                    e.color = DBEntityFinder.Instance.GetPointStyle(e.color, str);
                }
            }
            else
            {
                e.SymbolType = Zero;
            }
        }

        [Execution(205)]
        public static void Gg_gz_rj_pt_sdogeom(object sender, SymbolEventArgs e)
        {
            var g3e_fid = (sender as ElectronSymbol).G3E_FID;
             if (e.electronSymbol.KxType) return;
            var com = DBEntityFinder.Instance.GetCommon_n(e.DBManager,g3e_fid);
            if (com != null)
            {
                e.SBMC = com.SBMC;
            }
        }

        [Execution(307)]
        public static void Gg_pd_dyhgq_pt_sdogeom(object sender, SymbolEventArgs e)
        {
            var g3e_fid = (sender as ElectronSymbol).G3E_FID;
             if (e.electronSymbol.KxType) return;
            var com = DBEntityFinder.Instance.GetCommon_n(e.DBManager,g3e_fid);
            if (com != null)
            {
                e.SBMC = com.SBMC;
                const string str = "电压互感器符号";
                e.color = DBEntityFinder.Instance.GetPointStyle(str, com, e.color);
            }
        }

        [Execution(309)]
        public static void Gg_pd_gydj_pt_sdogeom(object sender, SymbolEventArgs e)
        {
            var g3e_fid = (sender as ElectronSymbol).G3E_FID;
             if (e.electronSymbol.KxType) return;
            var com = DBEntityFinder.Instance.GetCommon_n(e.DBManager,g3e_fid);
            if (com != null)
            {
                e.SBMC = com.SBMC;
                var str = "高压电机符号";
                if (!string.IsNullOrEmpty(com.CD_SMZQ))
                {
                    str = string.Format("{0}-{1}", str, com.CD_SMZQ);
                }
                e.color = DBEntityFinder.Instance.GetPointStyle(e.color, str);
            }
        }

        /****************************低压设备**************************/

        [Execution(73)]
        public static void Gg_pd_dldy_pt_sdogeom(object sender, SymbolEventArgs e)
        {
            var g3e_fid = (sender as ElectronSymbol).G3E_FID;
             if (e.electronSymbol.KxType) return;
            var com = DBEntityFinder.Instance.GetCommon_n(e.DBManager,g3e_fid);
            if (com != null)
            {
                e.SBMC = com.SBMC;
            }
        }

        [Execution(74)]
        public static void Gg_pd_dyzd_pt_sdogeom(object sender, SymbolEventArgs e)
        {
            var g3e_fid = (sender as ElectronSymbol).G3E_FID;
            if (!e.electronSymbol.KxType)
            {
                var com = DBEntityFinder.Instance.GetCommon_n(e.DBManager,g3e_fid);
                var cn = e.DBManager.GetEntity<Gg_pd_dyzd_n>(g3e_fid);
                if (com != null && cn != null)
                {
                    e.SBMC = com.SBMC;
                    switch (cn.JDLX)
                    {
                        case "终端":
                            e.SymbolType = Zero;
                            break;
                        case "联络终端":
                            e.SymbolType = One;
                            break;
                        default:
                            e.SymbolType = Zero;
                            break;
                    }
                }
            }
            else
                e.SymbolType = Zero;
        }

        [Execution(155)]
        public static void Gg_pd_dykg_pt_sdogeom(object sender, SymbolEventArgs e)
        {
            var g3e_fid = (sender as ElectronSymbol).G3E_FID;
            if (!e.electronSymbol.KxType)
            {
                var cn = e.DBManager.GetEntity<Gg_pd_dykg_n>(g3e_fid);
                var com = DBEntityFinder.Instance.GetCommon_n(e.DBManager,g3e_fid);
                var con = DBEntityFinder.Instance.GetConnectivity_n(e.DBManager,g3e_fid);
                if (con != null && com != null && cn!=null)
                {
                    e.SBMC = com.SBMC;
                    switch (cn.CD_DYKGFL)
                    {
                        case "断路器":
                            e.SymbolType = Zero;
                            break;
                        case "低压柜刀闸":
                            e.SymbolType = Zero;
                            break;
                        case "塑壳开关":
                            e.SymbolType = Zero;
                            break;
                        case "空气开关":
                            e.SymbolType = Zero;
                            break;
                        case "条形开关":
                            e.SymbolType = Zero;
                            break;
                        case "刀闸":
                            e.SymbolType = One;
                            break;
                        case "隔离器":
                            e.SymbolType = Zero;
                            break;
                        case "智能控制器":
                            e.SymbolType = Zero;
                            break;
                        default:
                            e.SymbolType = Zero;
                            break;
                    }
                    e.BreakerStatus = con.CD_DQZT == "闭合" ? "1" : "0";

                    string str = "低压开关符号";
                    str += string.IsNullOrEmpty(com.CD_SMZQ) ? "" : "-" + com.CD_SMZQ;
                    str += string.IsNullOrEmpty(cn.CD_DYKGFL) ? "" : "-" + cn.CD_DYKGFL;
                    if (str.IndexOf("-停运") > 0)
                    {
                        str += string.IsNullOrEmpty(con.CD_DQZT) ? "" : "-" + con.CD_DQZT;
                    }

                    if (str.IndexOf("-投运") > 0)
                    {
                        str += string.IsNullOrEmpty(con.CD_DQZT) ? "" : "-" + con.CD_DQZT;
                        if (!string.IsNullOrEmpty(con.CD_DQZT))
                            str += string.IsNullOrEmpty(con.CD_SFDD) ? "" : "-" + con.CD_SFDD;
                    }

                    e.color = DBEntityFinder.Instance.GetPointStyle(e.color, str);
                }
            }
            else
            {
                e.SymbolType = Zero;
                e.BreakerStatus = "0";
            }
        }

        [Execution(158)]
        public static void Gg_pd_dyzjjt_pt_sdogeom(object sender, SymbolEventArgs e)
        {
            var g3e_fid = (sender as ElectronSymbol).G3E_FID;
             if (e.electronSymbol.KxType) return;
            var com = DBEntityFinder.Instance.GetCommon_n(e.DBManager,g3e_fid);
            var con = DBEntityFinder.Instance.GetConnectivity_n(e.DBManager,g3e_fid);
            if (com != null && con != null)
            {
                e.SBMC = com.SBMC;
                var str = "低压电缆中间接头符号";

                str = DBEntityFinder.Instance.GetUserNameBySMZQorSFDD(str, com.CD_SMZQ, con.CD_SFDD);
                e.color = DBEntityFinder.Instance.GetPointStyle(str, com, e.color);
            }
        }

        [Execution(159)]
        public static void Gg_pd_cbx_pt_sdogeom(object sender, SymbolEventArgs e)
        {
            var g3e_fid = (sender as ElectronSymbol).G3E_FID;
            if (!e.electronSymbol.KxType)
            {
                var cn = e.DBManager.GetEntity<Gg_pd_cbx_n>(g3e_fid);
                if (cn != null)
                {
                    e.SBMC = cn.HH;

                    switch (cn.LX)
                    {
                        case "集抄箱":
                            e.SymbolType = Zero;
                            break;
                        case "门牌地址":
                            e.SymbolType = One;
                            break;
                        case "复录仪":
                            e.SymbolType = Two;
                            break;
                        default:
                            e.SymbolType = Zero;
                            break;
                    }
                    string str = "低压集中抄表箱符号";

                    str += string.IsNullOrEmpty(cn.LX) ? "" : "-" + cn.LX;

                    var con = e.DBManager.GetEntity<Connectivity_n>(g3e_fid);
                    if (con != null)
                    {
                        //不带电情况
                        if (cn.LX.Equals("集抄箱") && con.CD_SFDD.Equals("不带电"))
                        {
                            str = "低压集中抄表箱符号-停电";
                        }
                        else if (cn.LX.Equals("复录仪") && con.CD_SFDD.Equals("不带电"))
                            str = string.Format("{0}-停电", str);
                    }
                    else
                    {
                        LogManager.Instance.Error("集抄缺失连接关系表！FID：" + g3e_fid);
                    }
                    e.color = DBEntityFinder.Instance.GetPointStyle(e.color, str);
                }
            }
            else
            {
                e.SymbolType = Zero;
            }
        }

        [Execution(169)]
        public static void Gg_pd_dydlt_pt_sdogeom(object sender, SymbolEventArgs e)
        {
            var g3e_fid = (sender as ElectronSymbol).G3E_FID;
            if (!e.electronSymbol.KxType)
            {
                var cn = e.DBManager.GetEntity<Gg_pd_dydlt_n>(g3e_fid);
                var com = DBEntityFinder.Instance.GetCommon_n(e.DBManager,g3e_fid);
                if (com != null && cn != null)
                {
                    e.SBMC = com.SBMC;
                    switch (cn.CD_SFSDL)
                    {
                        case "双电缆头":
                            e.SymbolType = One;
                            break;
                        default:
                            e.SymbolType = Zero;
                            break;
                    }
                    string str = "低压电缆头符号";
                    if (!string.IsNullOrEmpty(com.CD_SMZQ))
                    {
                        str = string.Format("{0}-{1}", str, com.CD_SMZQ);
                    }
                    if (!string.IsNullOrEmpty(cn.CD_SFSDL))
                    {
                        str = string.Format("{0}-{1}", str, cn.CD_SFSDL == "双电缆头" ? "双头" : "单头");
                    }
                    e.color = DBEntityFinder.Instance.GetPointStyle(e.color, str);
                }
            }
            else
            {
                e.SymbolType = Zero;
            }
        }

        [Execution(215)]
        public static void Gg_gz_dydg_pt_sdogeom(object sender, SymbolEventArgs e)
        {
            var g3e_fid = (sender as ElectronSymbol).G3E_FID;
             if (e.electronSymbol.KxType) return;
            var com = DBEntityFinder.Instance.GetCommon_n(e.DBManager,g3e_fid);
            if (com != null)
            {
                e.SBMC = com.SBMC;
            }
        }

        [Execution(218)]
        public static void Gg_gz_dyzc_pt_sdogeom(object sender, SymbolEventArgs e)
        {
            var g3e_fid = (sender as ElectronSymbol).G3E_FID;
             if (e.electronSymbol.KxType) return;
            var com = DBEntityFinder.Instance.GetCommon_n(e.DBManager,g3e_fid);
            if (com != null)
            {
                e.SBMC = com.SBMC;
                const string str = "低压支撑符号";
                e.color = DBEntityFinder.Instance.GetPointStyle(str, com, e.color);
            }
        }

        [Execution(320)]
        public static void Gg_pd_jszz_pt_sdogeom(object sender, SymbolEventArgs e)
        {
            var g3e_fid = (sender as ElectronSymbol).G3E_FID;
             if (e.electronSymbol.KxType) return;
            var com = DBEntityFinder.Instance.GetCommon_n(e.DBManager,g3e_fid);
            if (com != null)
            {
                e.SBMC = com.SBMC;
                const string str = "监视装置符号";
                e.color = DBEntityFinder.Instance.GetPointStyle(e.color, str);
            }
        }

        [Execution(385)]
        public static void Gg_pd_fkjc_pt_sdogeom(object sender, SymbolEventArgs e)
        {
            var g3e_fid = (sender as ElectronSymbol).G3E_FID;
             if (e.electronSymbol.KxType) return;
            var com = DBEntityFinder.Instance.GetCommon_n(e.DBManager,g3e_fid);
            if (com != null)
            {

                e.SBMC = com.SBMC;
            }
        }

        #endregion

        #region 面符号

        [Execution(77)]
        public static void Gg_pd_dlfxx_ar_sdogeom(object sender, SymbolEventArgs e)
        {
            var g3e_fid = (sender as ElectronSymbol).G3E_FID;
             if (e.electronSymbol.KxType) return;
            var com = DBEntityFinder.Instance.GetCommon_n(e.DBManager,g3e_fid);
            if (com != null)
                e.SBMC = com.SBMC;
        }

        [Execution(81)]
        public static void Gg_pd_dyg_ar_sdogeom(object sender, SymbolEventArgs e)
        {
            var g3e_fid = (sender as ElectronSymbol).G3E_FID;
             if (e.electronSymbol.KxType) return;
            var com = DBEntityFinder.Instance.GetCommon_n(e.DBManager,g3e_fid);
            if (com != null)
            {
                e.SBMC = com.SBMC;
                if (com.EntityState == EntityState.Delete) return;

                const string str = "低压柜符号";
                e.color = DBEntityFinder.Instance.GetAreaStyle(str, com, e.color);
            }
        }

        [Execution(142)]
        public static void Gg_pd_gydf_ar_sdogeom(object sender, SymbolEventArgs e)
        {
            var g3e_fid = (sender as ElectronSymbol).G3E_FID;
             if (e.electronSymbol.KxType) return;
            var com = DBEntityFinder.Instance.GetCommon_n(e.DBManager,g3e_fid);
            if (com != null)
            {
                e.SBMC = com.SBMC;

                string str = "公用电房符号";

                var n = e.DBManager.GetEntity<Gg_pd_gydf_n>(g3e_fid);
                if (n != null &&
                    ((n.CD_ZCGS == "是") || (n.CD_ZCGS == "有"))
                    )
                {
                    str += "-带锁";
                }
                if (!string.IsNullOrEmpty(com.CD_SMZQ))
                {
                    str += "-" + com.CD_SMZQ;
                }
                e.color = DBEntityFinder.Instance.GetAreaStyle(e.color, str);
            }
        }

        [Execution(149)]
        public static void Gg_pd_xssb_ar_sdogeom(object sender, SymbolEventArgs e)
        {
            var g3e_fid = (sender as ElectronSymbol).G3E_FID;
             if (e.electronSymbol.KxType) return;
            var com = DBEntityFinder.Instance.GetCommon_n(e.DBManager,g3e_fid);
            if (com != null)
            {
                e.SBMC = com.SBMC;
                const string str = "箱式设备符号";
                e.color = DBEntityFinder.Instance.GetAreaStyle(str, com, e.color);
            }
        }

        [Execution(163)]
        public static void Gg_pd_zydf_ar_sdogeom(object sender, SymbolEventArgs e)
        {
            var g3e_fid = (sender as ElectronSymbol).G3E_FID;
             if (e.electronSymbol.KxType) return;
            var com = DBEntityFinder.Instance.GetCommon_n(e.DBManager,g3e_fid);
            if (com != null)
            {
                e.SBMC = com.SBMC;

                string str = "专用电房符号";

                //var gn = DBEntityFinder.Instance.GetGnwzmc_n(e.DBManager,g3e_fid);
                //if (gn != null &&
                //    ((gn.GNWZ_YFZNMS == "是") || (gn.GNWZ_YFZNMS == "有"))
                //    )
                //{
                //    str = string.Format("{0}-带锁", str);
                //}
                if (!string.IsNullOrEmpty(com.CD_SMZQ))
                {
                    str = string.Format("{0}-{1}", str, com.CD_SMZQ);
                }
                e.color = DBEntityFinder.Instance.GetAreaStyle(e.color, str);
            }
        }

        [Execution(302)]
        public static void Gg_pd_dypdf_ar_sdogeom(object sender, SymbolEventArgs e)
        {
            var g3e_fid = (sender as ElectronSymbol).G3E_FID;
             if (e.electronSymbol.KxType) return;
            var com = DBEntityFinder.Instance.GetCommon_n(e.DBManager,g3e_fid);
            if (com != null)
                e.SBMC = com.SBMC;
        }

        #endregion


        [Execution(71)]
        public static void Gg_pd_cqtg_pt_sdogeom(object sender, SymbolEventArgs e)
        {
            var g3e_fid = (sender as ElectronSymbol).G3E_FID;
             if (e.electronSymbol.KxType) return;
            var com = DBEntityFinder.Instance.GetCommon_n(e.DBManager,g3e_fid);
            if (com != null)
            {
                e.SBMC = com.SBMC;

                string str = "穿墙套管符号";
                str = string.Format("{0}-{1}", str, com.CD_SMZQ);
                e.color = DBEntityFinder.Instance.GetPointStyle(str, com, e.color);
            }
        }

        [Execution(72)]
        public static void Gg_pd_jdgh_pt_sdogeom(object sender, SymbolEventArgs e)
        {
            var g3e_fid = (sender as ElectronSymbol).G3E_FID;
             if (e.electronSymbol.KxType) return;
            var com = DBEntityFinder.Instance.GetCommon_n(e.DBManager,g3e_fid);
            if (com != null)
            {
                e.SBMC = com.SBMC;
                string str = "接地挂环符号";
                str = string.Format("{0}-{1}", str, com.CD_SMZQ);
                e.color = DBEntityFinder.Instance.GetPointStyle(str, com, e.color);
            }
        }

        [Execution(76)]
        public static void Gg_pd_gkjlg_pt_sdogeom(object sender, SymbolEventArgs e)
        {
            var g3e_fid = (sender as ElectronSymbol).G3E_FID;
             if (e.electronSymbol.KxType) return;
            var com = DBEntityFinder.Instance.GetCommon_n(e.DBManager,g3e_fid);
            if (com != null)
            {
                e.SBMC = com.SBMC;
                string str = "关口计量柜符号";
                str = string.Format("{0}-{1}", str, com.CD_SMZQ);
                e.color = DBEntityFinder.Instance.GetPointStyle(str, com, e.color);
            }
        }

        [Execution(183)]
        public static void Gg_jc_qyjcy_pt_sdogeom(object sender, SymbolEventArgs e)
        {
            var g3e_fid = (sender as ElectronSymbol).G3E_FID;
             if (e.electronSymbol.KxType) return;
            var com = DBEntityFinder.Instance.GetCommon_n(e.DBManager,g3e_fid);
            if (com != null)
            {
                e.SBMC = com.SBMC;

                string str = "气压监测仪符号";
                str = string.Format("{0}-{1}", str, com.CD_SMZQ);
                e.color = DBEntityFinder.Instance.GetPointStyle(str, com, e.color);
            }
        }

        [Execution(184)]
        public static void Gg_jc_yyjcy_pt_sdogeom(object sender, SymbolEventArgs e)
        {
            var g3e_fid = (sender as ElectronSymbol).G3E_FID;
             if (e.electronSymbol.KxType) return;
            var com = DBEntityFinder.Instance.GetCommon_n(e.DBManager,g3e_fid);
            if (com != null)
            {
                e.SBMC = com.SBMC;
                string str = "油压监测仪符号";
                str = string.Format("{0}-{1}", str, com.CD_SMZQ);
                e.color = DBEntityFinder.Instance.GetPointStyle(str, com, e.color);
            }
        }

        [Execution(185)]
        public static void Gg_jc_dyjcy_pt_sdogeom(object sender, SymbolEventArgs e)
        {
            var g3e_fid = (sender as ElectronSymbol).G3E_FID;
             if (e.electronSymbol.KxType) return;
            var com = DBEntityFinder.Instance.GetCommon_n(e.DBManager,g3e_fid);
            if (com != null)
            {
                e.SBMC = com.SBMC;
                string str = "电压监测仪符号";
                str = string.Format("{0}-{1}", str, com.CD_SMZQ);
                e.color = DBEntityFinder.Instance.GetPointStyle(str, com, e.color);
            }
        }


        [Execution(186)]
        public static void Gg_jc_ldkgx_pt_sdogeom(object sender, SymbolEventArgs e)
        {
            var g3e_fid = (sender as ElectronSymbol).G3E_FID;
             if (e.electronSymbol.KxType) return;
            var com = DBEntityFinder.Instance.GetCommon_n(e.DBManager,g3e_fid);
            if (com != null)
            {
                e.SBMC = com.SBMC;
                string str = "路灯开关箱符号";
                str = string.Format("{0}-{1}", str, com.CD_SMZQ);
                e.color = DBEntityFinder.Instance.GetPointStyle(str, com, e.color);
            }
        }

        [Execution(188)]
        public static void Gg_jc_fk_pt_sdogeom(object sender, SymbolEventArgs e)
        {
            var g3e_fid = (sender as ElectronSymbol).G3E_FID;
             if (e.electronSymbol.KxType) return;
            var com = DBEntityFinder.Instance.GetCommon_n(e.DBManager,g3e_fid);
            if (com != null)
            {
                e.SBMC = com.SBMC;
                string str = "负控符号";
                str = string.Format("{0}-{1}", str, com.CD_SMZQ);
                e.color = DBEntityFinder.Instance.GetPointStyle(str, com, e.color);
            }
        }
        /// <summary>
        /// 获取安装位置
        /// </summary>
        /// <param name="g3e_fno"></param>
        /// <param name="g3e_fid"></param>
        /// <returns></returns>
        private static string GetAzwz(long g3e_fno,long g3e_fid)
        {
            var azwz = string.Empty;
            try
            {
                var simpleEntry = DeviceAttributeConfig.Instance.Attributes.SingleOrDefault(o => o.Fno == g3e_fno);
                if (simpleEntry != null)
                {
                    if (simpleEntry.InstallLocationOption != null)
                    {
                        var tableName = simpleEntry.InstallLocationOption.TableName;
                        var fieldName = simpleEntry.InstallLocationOption.FieldName;
                        var type = TypeCache.Instance.GetTypeFromCache(typeof (DBEntity), tableName.Trim());
                        var gnwz = DBManager.Instance.GetEntity(type, g3e_fid);
                        if (gnwz != null)
                        {
                            azwz = gnwz.GetValue<string>(fieldName);
                        }
                    }
                }
            }catch(Exception exception)
            {
                LogManager.Instance.Error(exception);
            }
            return azwz;
        }
    }
}
