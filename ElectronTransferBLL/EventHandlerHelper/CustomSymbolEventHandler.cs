using System;
using ElectronTransferDal.Cad;
using ElectronTransferDal.Common;
using ElectronTransferDal.Factory;
using ElectronTransferDal.HelperEventArgs;
using ElectronTransferDal.Query;
using ElectronTransferFramework;
using ElectronTransferModel;
using ElectronTransferModel.Base;
using ElectronTransferModel.V9_4;
using CADColor = Autodesk.AutoCAD.Colors.Color;
using Color = System.Drawing.Color;

namespace ElectronTransferBll.EventHandlerHelper
{
    [ExecutionContainer(typeof(CustomSymbolExecutionFactory))]
    public class CustomSymbolEventHandler
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
            e.color = CADColor.FromColor(Color.FromArgb(CABLEManager._color3));

            var com = e.DBManager.GetEntity<Common_n>((sender as ElectronSymbol).G3E_FID);
            if (com == null)
                return;
            
            
            e.SBMC = com.SBMC;
            string str = "10kV电缆符号";
            if (!string.IsNullOrEmpty(com.CD_SMZQ))
            {
                str += "-" + com.CD_SMZQ;
                var cx = e.DBManager.GetEntity<Gg_pd_dl_n>((sender as ElectronSymbol).G3E_FID);
                if (cx != null && cx.CXD == "粗")
                {
                    str += "-" + cx.CXD;
                }
            }
            e=DBEntityFinder.Instance.GetLineStyle(e,str);
        }

        //0803下午
        [Execution(141)]
        public static void Gg_pd_dx_ln_sdogeom(object sender, SymbolEventArgs e)
        {
            e.lineWidth = CABLEManager._lineWidth002;
            e.color = CADColor.FromColor(Color.FromArgb(CABLEManager._color3));

            var com = e.DBManager.GetEntity<Common_n>((sender as ElectronSymbol).G3E_FID);
            if (com == null) return;

            
            e.SBMC = com.SBMC;
            string str = "10kV导线符号";

            if (!string.IsNullOrEmpty(com.CD_SMZQ))
            {
                str += "-" + com.CD_SMZQ;
                var cx = e.DBManager.GetEntity<Gg_pd_dx_n>((sender as ElectronSymbol).G3E_FID);
                if (cx != null && cx.CXD == "粗")
                {
                    str += "-" + cx.CXD;
                }
            }
            else
            {
                var cx = e.DBManager.GetEntity<Gg_pd_dx_n>((sender as ElectronSymbol).G3E_FID);
                if (cx != null && cx.CXD == "粗")
                {
                    str += "-" + cx.CXD;
                }
            }
            e = DBEntityFinder.Instance.GetLineStyle(e, str);
        }

        [Execution(143)]
        public static void Gg_pd_zfmx_ln_sdogeom(object sender, SymbolEventArgs e)
        {
            e.lineWidth = CABLEManager._lineWidth004;
            e.color = CADColor.FromColor(Color.FromArgb(CABLEManager._color3));


            var com = e.DBManager.GetEntity<Common_n>((sender as ElectronSymbol).G3E_FID);
            if (com == null) return;
            
            e.SBMC = com.SBMC;
            string str = "站房母线符号";

            if (!string.IsNullOrEmpty(com.CD_SMZQ))
            {
                str += "-" + com.CD_SMZQ;
            }
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
            e = DBEntityFinder.Instance.GetLineStyle(e, str);
        }

        //0804

        [Execution(144)]
        public static void Gg_pd_zfyx_ln_sdogeom(object sender, SymbolEventArgs e)
        {
            e.lineWidth = CABLEManager._lineWidth001;
            e.color = CADColor.FromColor(Color.FromArgb(CABLEManager._color3));

            var com = e.DBManager.GetEntity<Common_n>((sender as ElectronSymbol).G3E_FID);
            if (com == null) return;
            
            e.SBMC = com.SBMC;
            string str = "站房引线符号";
            if (!string.IsNullOrEmpty(com.CD_SMZQ))
            {
                str += "-" + com.CD_SMZQ;
            }
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
            e = DBEntityFinder.Instance.GetLineStyle(e, str);
        }

        [Execution(79)]
        public static void Gg_pd_dymx_ln_sdogeom(object sender, SymbolEventArgs e)
        {

            e.lineWidth = CABLEManager._lineWidth004;
            e.color = CADColor.FromColor(Color.FromArgb(CABLEManager._color2));
            var com = e.DBManager.GetEntity<Common_n>((sender as ElectronSymbol).G3E_FID);
            if (com != null)
                e.SBMC = com.SBMC;
        }

        [Execution(156)]
        public static void Gg_pd_dydx_ln_sdogeom(object sender, SymbolEventArgs e)
        {
            e.lineWidth = CABLEManager._lineWidth002;
            e.color = CADColor.FromColor(Color.FromArgb(CABLEManager._color2));

            var com = e.DBManager.GetEntity<Common_n>((sender as ElectronSymbol).G3E_FID);
            if (com == null) return;

            e.SBMC = com.SBMC;
            string str = "低压导线符号";
            e = DBEntityFinder.Instance.GetLineStyle(str, com, e);
        }

        [Execution(157)]
        public static void Gg_pd_dydl_ln_sdogeom(object sender, SymbolEventArgs e)
        {
            e.lineWidth = CABLEManager._lineWidth002;
            e.color = CADColor.FromColor(Color.FromArgb(CABLEManager._color2));

            var com = e.DBManager.GetEntity<Common_n>((sender as ElectronSymbol).G3E_FID);
            if (com == null) return;

            
            e.SBMC = com.SBMC;
            string str = "低压电缆符号";
            e = DBEntityFinder.Instance.GetLineStyle(str, com, e);
        }

        [Execution(80)]
        public static void Gg_pd_bqx_ln_sdogeom(object sender, SymbolEventArgs e)
        {
            e.lineWidth = CABLEManager._lineWidth002;
            e.color = CADColor.FromColor(Color.FromArgb(CABLEManager._color2));

            var com = e.DBManager.GetEntity<Common_n>((sender as ElectronSymbol).G3E_FID);
            if (com == null) return;

            e.SBMC = com.SBMC;
            string str = "表前线符号";

            e = DBEntityFinder.Instance.GetLineStyle(str, com, e);
        }

        [Execution(75)]
        public static void Gg_pd_bdcx_pt_sdogeom(object sender, SymbolEventArgs e)
        {
            e.lineWidth = CABLEManager._lineWidth002;
            e.color = CADColor.FromColor(Color.FromArgb(CABLEManager._color2));
            var com = e.DBManager.GetEntity<Common_n>((sender as ElectronSymbol).G3E_FID);
            if (com != null)
                e.SBMC = com.SBMC;
        }
        #endregion


        #region 点符号

        [Execution(40)]
        public static void Gg_pd_dlhgq_pt_sdogeom(object sender, SymbolEventArgs e)
        {
            e.color = CADColor.FromColor(Color.FromArgb(CABLEManager._color3));

            var com = e.DBManager.GetEntity<Common_n>((sender as ElectronSymbol).G3E_FID);
            if (com != null)
            {
                
                e.SBMC = com.SBMC;
                
                string str = "电流互感器符号";
                if (!string.IsNullOrEmpty(com.CD_SMZQ))
                {
                    str += "-" + com.CD_SMZQ;
                }
                e.color =DBEntityFinder.Instance.GetPointStyle(e.color, str);
            }
            else
            {
                e.color = CADColor.FromColor(Color.FromArgb(CABLEManager._color0));
            }
        }

        [Execution(82)]
        public static void Gg_jx_gyb_pt_sdogeom(object sender, SymbolEventArgs e)
        {
            var g3e_fid = (sender as ElectronSymbol).G3E_FID;
            e.color = CADColor.FromColor(Color.FromArgb(CABLEManager._color3));
            var cn = e.DBManager.GetEntity<Gg_pd_gyb_n>(g3e_fid);
            var com = e.DBManager.GetEntity<Common_n>(g3e_fid);
            var con = e.DBManager.GetEntity<Connectivity_n>(g3e_fid);
            if (cn != null&&com!=null&&con!=null)
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
                    str = string.Format("{0}-{1}-{2}", str, com.CD_SMZQ, con.CD_SFDD);
                }
                e.color =DBEntityFinder.Instance.GetPointStyle(e.color, str);
            }
        }

        [Execution(84)]
        public static void Gg_pd_cjlg_pt_sdogeom(object sender, SymbolEventArgs e)
        {
            var g3e_fid = (sender as ElectronSymbol).G3E_FID;
            
            e.color = CADColor.FromColor(Color.FromArgb(CABLEManager._color3));
            var com = e.DBManager.GetEntity<Common_n>((sender as ElectronSymbol).G3E_FID);
            var cn = e.DBManager.GetEntity<Gg_pd_cjlg_n>(g3e_fid);
            var con = e.DBManager.GetEntity<Connectivity_n>(g3e_fid);
            if (com != null && cn != null && con!=null)
            {
                e.SBMC = com.SBMC;
                switch (cn.SBLX)
                {
                    case "抽屉式计量柜":
                        e.SymbolType = One;
                        break;
                    case "固定式计量柜":
                        e.SymbolType = Zero;
                        break;
                }
                var str = string.Format("{0}符号", cn.SBLX);
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
                        str = string.Format("{0}-{1}", str, con.CD_SFDD);
                    }
                }
                e.color = DBEntityFinder.Instance.GetPointStyle(e.color, str);
            }
        }

        [Execution(85)]
        public static void Gg_pd_ptg_pt_sdogeom(object sender, SymbolEventArgs e)
        {
            e.color = CADColor.FromColor(Color.FromArgb(CABLEManager._color3));
            var com = e.DBManager.GetEntity<Common_n>((sender as ElectronSymbol).G3E_FID);
            var con = e.DBManager.GetEntity<Connectivity_n>((sender as ElectronSymbol).G3E_FID);
            if (com != null && con!=null)
            {
                e.SBMC = com.SBMC;
                if (com.EntityState == EntityState.Delete)
                {
                    e.color = CADColor.FromColor(Color.FromArgb(CABLEManager._color0));
                    return;
                }

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
            else
            {
                e.color = CADColor.FromColor(Color.FromArgb(CABLEManager._color0));
            }
        }

        [Execution(86)]
        public static void Gg_jc_dcjg_pt_sdogeom(object sender, SymbolEventArgs e)
        {
            e.color = CADColor.FromColor(Color.FromArgb(CABLEManager._color3));
            var com = e.DBManager.GetEntity<Common_n>((sender as ElectronSymbol).G3E_FID);
            if (com != null)
            {
                e.SBMC = com.SBMC;

                string str = "电操机构符号" + (string.IsNullOrEmpty(com.CD_SMZQ) ? "" : "-" + com.CD_SMZQ);

                e.color = DBEntityFinder.Instance.GetPointStyle(e.color, str);
            }
            else
            {
                e.color = CADColor.FromColor(Color.FromArgb(CABLEManager._color0));
            }
        }

        [Execution(90)]
        public static void Gg_jc_dtu_pt_sdogeom(object sender, SymbolEventArgs e)
        {
            e.color = CADColor.FromColor(Color.FromArgb(CABLEManager._color3));
            var com = e.DBManager.GetEntity<Common_n>((sender as ElectronSymbol).G3E_FID);
            if (com != null)
            {
                e.SBMC = com.SBMC;
                string str = "DTU符号";
                if (!string.IsNullOrEmpty(com.CD_SMZQ))
                {
                    str += "-" + com.CD_SMZQ;
                }
                e.color = DBEntityFinder.Instance.GetPointStyle(e.color, str);
            }
            else
            {
                e.color = CADColor.FromColor(Color.FromArgb(CABLEManager._color0));
            }
        }

        [Execution(145)]
        public static void Gg_pd_zfllkg_pt_sdogeom(object sender, SymbolEventArgs e)
        {
            e.color = CADColor.FromColor(Color.FromArgb(CABLEManager._color3));
            var com = e.DBManager.GetEntity<Common_n>((sender as ElectronSymbol).G3E_FID);
            if (com != null)
            {
                e.SBMC = com.SBMC;
                string str = "站房联络开关符号";

                if (!string.IsNullOrEmpty(com.CD_SMZQ))
                {
                    str += "-" + com.CD_SMZQ;
                }
                var zfllkg =
                    e.DBManager.GetEntity<Connectivity_n>((sender as ElectronSymbol).G3E_FID);
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
                    if (str.IndexOf("-停运") > 0)
                    {
                        str += "-" + zfllkg.CD_DQZT;
                    }
                }
                e.color = DBEntityFinder.Instance.GetPointStyle(e.color, str);
            }
            else
            {
                e.color = CADColor.FromColor(Color.FromArgb(CABLEManager._color0));
            }
        }

        [Execution(146)]
        public static void Gg_pd_zfhwg_pt_sdogeom(object sender, SymbolEventArgs e)
        {
            e.color = CADColor.FromColor(Color.FromArgb(CABLEManager._color3));
            var cn = e.DBManager.GetEntity<Gg_pd_zfhwg_n>((sender as ElectronSymbol).G3E_FID);
            var com = e.DBManager.GetEntity<Common_n>((sender as ElectronSymbol).G3E_FID);
            var con = e.DBManager.GetEntity<Connectivity_n>((sender as ElectronSymbol).G3E_FID);
            if (com != null && cn != null && con != null)
            {
                e.SBMC = com.SBMC;
                switch (cn.CD_ZFHWGFL)
                {
                    case "断路器":
                        e.SymbolType = Zero;
                        break;
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
                        e.SymbolType = Five;
                        break;
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

                string str = "10kV开关符号";
                if (!string.IsNullOrEmpty(com.CD_SMZQ))
                {
                    str += "-" + com.CD_SMZQ;
                }
                if (con != null)
                {
                    if (str.IndexOf("-投运") > 0 || str.IndexOf("-停运") > 0)
                    {
                        if (!string.IsNullOrEmpty(con.CD_DQZT))
                        {
                            str += "-" + con.CD_DQZT;
                        }
                    }
                }
                e.color = DBEntityFinder.Instance.GetPointStyle(e.color, str);
            }
        }

        [Execution(147)]
        public static void Gg_pd_zfdlt_pt_sdogeom(object sender, SymbolEventArgs e)
        {
            e.color = CADColor.FromColor(Color.FromArgb(CABLEManager._color3));
            var cn = e.DBManager.GetEntity<Gg_pd_zfdlt_n>((sender as ElectronSymbol).G3E_FID);
            var com = e.DBManager.GetEntity<Common_n>((sender as ElectronSymbol).G3E_FID);
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
                string str = "站房电缆头符号";
                if (!string.IsNullOrEmpty(com.CD_SMZQ))
                {
                    str =string.Format("{0}-{1}",str,com.CD_SMZQ);
                }
                if (str.IndexOf("-投运") > 0)
                {
                    if (!string.IsNullOrEmpty(com.CD_DYDJ) && !string.IsNullOrEmpty(cn.CD_SFSDL))
                    {
                        var dj = com.CD_DYDJ == "0.4" ? "400V" : (com.CD_DYDJ == "10" ? "10kV" : "");
                        var dlt= cn.CD_SFSDL == "双电缆头" ? "双头" : "单头";
                        string.Format("{0}-{1}{2}",str,dj,dlt);
                    }
                }
                else
                {
                    if (!string.IsNullOrEmpty(com.CD_DYDJ))
                    {
                        str =string.Format("{0}-{1}",str, cn.CD_SFSDL == "双电缆头" ? "双头" : "单头");
                    }
                }

                e.color = DBEntityFinder.Instance.GetPointStyle(e.color, str);
            }
        }

        [Execution(148)]
        public static void Gg_pd_zfbyq_pt_sdogeom(object sender, SymbolEventArgs e)
        {
            e.color = CADColor.FromColor(Color.FromArgb(CABLEManager._color3));
            var tjn = e.DBManager.GetEntity<Gg_pd_zfbyq_n>((sender as ElectronSymbol).G3E_FID);
            var com = e.DBManager.GetEntity<Common_n>((sender as ElectronSymbol).G3E_FID);
            if (tjn != null && com != null)
            {
                e.SBMC = com.SBMC;
                var pblb = tjn.CD_PBLB.Replace("他", "它");
                switch (pblb)
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
                    default:
                        e.SymbolType = Four;
                        break;
                }

                string str = "站房变压器符号";
                str += string.IsNullOrEmpty(com.CD_SMZQ) ? "" : "-" + com.CD_SMZQ;
                str += string.IsNullOrEmpty(pblb) ? "" : "-" + pblb;

                e.color = DBEntityFinder.Instance.GetPointStyle(e.color, str);
            }
        }

        [Execution(150)]
        public static void Gg_pd_dlzjjt_pt_sdogeom(object sender, SymbolEventArgs e)
        {
            e.color = CADColor.FromColor(Color.FromArgb(CABLEManager._color3));
            var com = e.DBManager.GetEntity<Common_n>((sender as ElectronSymbol).G3E_FID);
            if (com != null)
            {
                e.SBMC = com.SBMC;

                string str = "10kV电缆中间接头符号";
                str += string.IsNullOrEmpty(com.CD_SMZQ) ? "" : "-" + com.CD_SMZQ;
                e.color = DBEntityFinder.Instance.GetPointStyle(e.color, str);
            }
            else
            {
                e.color = CADColor.FromColor(Color.FromArgb(CABLEManager._color0));
            }
        }

        [Execution(151)]
        public static void Gg_pd_dlzdt_pt_sdogeom(object sender, SymbolEventArgs e)
        {
            e.color = CADColor.FromColor(Color.FromArgb(CABLEManager._color3));
            var cn = e.DBManager.GetEntity<Gg_pd_dlzdt_n>((sender as ElectronSymbol).G3E_FID);
            var com = e.DBManager.GetEntity<Common_n>((sender as ElectronSymbol).G3E_FID);
            if (com != null && cn != null)
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
                str += string.IsNullOrEmpty(com.CD_SMZQ) ? "" : "-" + com.CD_SMZQ;

                e.color = DBEntityFinder.Instance.GetPointStyle(e.color, str);
            }
            else
            {
                e.SymbolType = Zero;
                e.color = CADColor.FromColor(Color.FromArgb(CABLEManager._color0));
            }
        }

        [Execution(152)]
        public static void Gg_pd_dlq_pt_sdogeom(object sender, SymbolEventArgs e)
        {
            e.color = CADColor.FromColor(Color.FromArgb(CABLEManager._color3));
            var com = e.DBManager.GetEntity<Common_n>((sender as ElectronSymbol).G3E_FID);
            var con = e.DBManager.GetEntity<Connectivity_n>((sender as ElectronSymbol).G3E_FID);
            if (con != null && com != null)
            {
                e.SBMC = com.SBMC;
                e.BreakerStatus = con.CD_DQZT == "闭合" ? "1" : "0";

                string str = "断路器符号";
                str += string.IsNullOrEmpty(com.CD_SMZQ) ? "" : "-" + com.CD_SMZQ;

                e.color = DBEntityFinder.Instance.GetPointStyle(e.color, str);
            }
        }

        [Execution(154)]
        public static void Gg_pd_zsbyq_pt_sdogeom(object sender, SymbolEventArgs e)
        {
            e.color = CADColor.FromColor(Color.FromArgb(CABLEManager._color3));
            var cn = e.DBManager.GetEntity<Gg_pd_zsbyq_n>((sender as ElectronSymbol).G3E_FID);
            var com = e.DBManager.GetEntity<Common_n>((sender as ElectronSymbol).G3E_FID);
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

        [Execution(171)]
        public static void Gg_pd_zdfdj_pt_sdogeom(object sender, SymbolEventArgs e)
        {
            e.color = CADColor.FromColor(Color.FromArgb(CABLEManager._color3));
            var com = e.DBManager.GetEntity<Common_n>((sender as ElectronSymbol).G3E_FID);
            if (com != null)
            {
                e.SBMC = com.SBMC;

                string str = "用户自带发电机符号";
                str += string.IsNullOrEmpty(com.CD_SMZQ) ? "" : "-" + com.CD_SMZQ;

                e.color = DBEntityFinder.Instance.GetPointStyle(e.color, str);
            }
            else
            {
                e.color = CADColor.FromColor(Color.FromArgb(CABLEManager._color0));
            }
        }

        [Execution(172)]
        public static void Gg_pd_ddj_pt_sdogeom(object sender, SymbolEventArgs e)
        {
            e.color = CADColor.FromColor(Color.FromArgb(CABLEManager._color3));
            var com = e.DBManager.GetEntity<Common_n>((sender as ElectronSymbol).G3E_FID);
            if (com != null)
            {
                e.SBMC = com.SBMC;

                string str = "电动机符号";
                str += string.IsNullOrEmpty(com.CD_SMZQ) ? "" : "-" + com.CD_SMZQ;

                e.color = DBEntityFinder.Instance.GetPointStyle(e.color, str);
            }
            else
            {
                e.color = CADColor.FromColor(Color.FromArgb(CABLEManager._color0));
            }
        }

        [Execution(173)]
        public static void Gg_pd_zfjddz_pt_sdogeom(object sender, SymbolEventArgs e)
        {
            e.color = CADColor.FromColor(Color.FromArgb(CABLEManager._color1));
            var com = e.DBManager.GetEntity<Common_n>((sender as ElectronSymbol).G3E_FID);
            var jddz = e.DBManager.GetEntity<Gg_pd_zfjddz_n>((sender as ElectronSymbol).G3E_FID);
            var conn = e.DBManager.GetEntity<Connectivity_n>((sender as ElectronSymbol).G3E_FID);
            if (com != null && conn != null)
            {
                e.SBMC = com.SBMC;
                e.BreakerStatus = conn.CD_DQZT == "闭合" ? "1" : "0";

                string str = "站房接地刀闸符号";
                if(!string.IsNullOrEmpty(com.CD_SMZQ))
                {
                    str +=  "-" + com.CD_SMZQ;
                }
                if(!string.IsNullOrEmpty(conn.CD_DQZT))
                {
                    str += "-" + conn.CD_DQZT;
                }
                switch (jddz.SGDW)
                {
                    case "新":
                        e.SymbolType = Zero;
                        break;
                    case "旧":
                        e.SymbolType = One;
                        break;
                }

                e.color = DBEntityFinder.Instance.GetPointStyle(e.color, str);
            }
            else
            {
                e.color = CADColor.FromColor(Color.FromArgb(CABLEManager._color0));
            }
        }

        [Execution(174)]
        public static void Gg_pd_wgbc_pt_sdogeom(object sender, SymbolEventArgs e)
        {
            e.color = CADColor.FromColor(Color.FromArgb(CABLEManager._color3));
            var com = e.DBManager.GetEntity<Common_n>((sender as ElectronSymbol).G3E_FID);
            if (com != null)
            {
                e.SBMC = com.SBMC;
                string str = "无功补偿器符号";
                str += string.IsNullOrEmpty(com.CD_SMZQ) ? "" : "-" + com.CD_SMZQ;

                e.color = DBEntityFinder.Instance.GetPointStyle(e.color, str);
            }
            else
            {
                e.color = CADColor.FromColor(Color.FromArgb(CABLEManager._color0));
            }
        }

        [Execution(176)]
        public static void Gg_pd_jdk_pt_sdogeom(object sender, SymbolEventArgs e)
        {
            e.color = CADColor.FromColor(Color.FromArgb(CABLEManager._color3));
            var com = e.DBManager.GetEntity<Common_n>((sender as ElectronSymbol).G3E_FID);
            if (com != null)
            {
                e.SBMC = com.SBMC;
                string str = "解断口符号";
                str += string.IsNullOrEmpty(com.CD_SMZQ) ? "" : "-" + com.CD_SMZQ;

                e.color = DBEntityFinder.Instance.GetPointStyle(e.color, str);
            }
            else
            {
                e.color = CADColor.FromColor(Color.FromArgb(CABLEManager._color0));
            }
        }

        [Execution(177)]
        public static void Gg_pd_blq_pt_sdogeom(object sender, SymbolEventArgs e)
        {
            e.color = CADColor.FromColor(Color.FromArgb(CABLEManager._color3));
            var com = e.DBManager.GetEntity<Common_n>((sender as ElectronSymbol).G3E_FID);
            var blq = e.DBManager.GetEntity<Gg_pd_blq_n>((sender as ElectronSymbol).G3E_FID);
            if (com != null)
            {
                e.SBMC = com.SBMC;
                switch (blq.AZWZ)
                {
                    case "房内":
                    case "户内":
                    case "箱式":
                        e.SymbolType = Zero;
                        break;
                    case "户外":
                    case "柱上":
                        e.SymbolType = One;
                        break;
                    default: 
                        e.SymbolType = One;
                        break;
                }
            }
            else
            {
                e.color = CADColor.FromColor(Color.FromArgb(CABLEManager._color0));
            }
        }

        [Execution(180)]
        public static void Gg_jc_ftu_pt_sdogeom(object sender, SymbolEventArgs e)
        {
            e.color = CADColor.FromColor(Color.FromArgb(CABLEManager._color3));
            var com = e.DBManager.GetEntity<Common_n>((sender as ElectronSymbol).G3E_FID);
            if (com != null)
            {
                e.SBMC = com.SBMC;
                string str = "FTU符号";
                str += string.IsNullOrEmpty(com.CD_SMZQ) ? "" : "-" + com.CD_SMZQ;

                e.color = DBEntityFinder.Instance.GetPointStyle(e.color, str);
            }
            else
            {
                e.color = CADColor.FromColor(Color.FromArgb(CABLEManager._color0));
            }
        }

        [Execution(181)]
        public static void Gg_jc_pwy_pt_sdogeom(object sender, SymbolEventArgs e)
        {
            e.color = CADColor.FromColor(Color.FromArgb(CABLEManager._color3));
            var com = e.DBManager.GetEntity<Common_n>((sender as ElectronSymbol).G3E_FID);
            if (com != null)
            {
                e.SBMC = com.SBMC;
                string str = "配网仪符号";
                str += string.IsNullOrEmpty(com.CD_SMZQ) ? "" : "-" + com.CD_SMZQ;

                e.color = DBEntityFinder.Instance.GetPointStyle(e.color, str);
            }
            else
            {
                e.color = CADColor.FromColor(Color.FromArgb(CABLEManager._color0));
            }
        }

        [Execution(182)]
        public static void Gg_jc_gzzsq_pt_sdogeom(object sender, SymbolEventArgs e)
        {
            e.color = CADColor.FromColor(Color.FromArgb(CABLEManager._color3));
            var com = e.DBManager.GetEntity<Common_n>((sender as ElectronSymbol).G3E_FID);
            if (com != null)
            {
                e.SBMC = com.SBMC;
                string str = "故障指示器符号";
                str += string.IsNullOrEmpty(com.CD_SMZQ) ? "" : "-" + com.CD_SMZQ;

                e.color = DBEntityFinder.Instance.GetPointStyle(e.color, str);
            }
            else
            {
                e.color = CADColor.FromColor(Color.FromArgb(CABLEManager._color0));
            }
        }

        [Execution(201)]
        public static void Gg_gz_dg_pt_sdogeom(object sender, SymbolEventArgs e)
        {
            e.color = CADColor.FromColor(Color.FromArgb(CABLEManager._color3));
            var com = e.DBManager.GetEntity<Common_n>((sender as ElectronSymbol).G3E_FID);
            var cn = e.DBManager.GetEntity<Gg_gz_dg_n>((sender as ElectronSymbol).G3E_FID);
            if (com != null && cn != null)
            {
                e.SBMC = com.SBMC;
                switch (cn.CD_CLLB)
                {
                    case "铁塔":
                        e.SymbolType = Two;
                        break;
                    case "铁杆":
                        e.SymbolType = Two;
                        break;
                    case "钢杆":
                        e.SymbolType = Zero;
                        break;
                    case "水泥杆":
                        e.SymbolType = One;
                        break;
                    default:
                        e.SymbolType = Zero;
                        break;
                }

                string str = "电杆符号";
                str += string.IsNullOrEmpty(cn.CD_CLLB) ? "" : "-" + cn.CD_CLLB;
                str += string.IsNullOrEmpty(com.CD_SMZQ) ? "" : "-" + com.CD_SMZQ;

                e.color = DBEntityFinder.Instance.GetPointStyle(e.color, str);
            }
            else
            {
                e.SymbolType = Zero;
                e.color = CADColor.FromColor(Color.FromArgb(CABLEManager._color0));
            }
        }

        [Execution(307)]
        public static void Gg_pd_dyhgq_pt_sdogeom(object sender, SymbolEventArgs e)
        {
            e.color = CADColor.FromColor(Color.FromArgb(CABLEManager._color3));
            var com = e.DBManager.GetEntity<Common_n>((sender as ElectronSymbol).G3E_FID);
            if (com != null)
            {
                e.SBMC = com.SBMC;
                string str = "电压互感器符号";
                str += string.IsNullOrEmpty(com.CD_SMZQ) ? "" : "-" + com.CD_SMZQ;

                e.color = DBEntityFinder.Instance.GetPointStyle(e.color, str);
            }
            else
            {
                e.color = CADColor.FromColor(Color.FromArgb(CABLEManager._color0));
            }
        }

        [Execution(309)]
        public static void Gg_pd_gydj_pt_sdogeom(object sender, SymbolEventArgs e)
        {
            e.color = CADColor.FromColor(Color.FromArgb(CABLEManager._color3));
            var com = e.DBManager.GetEntity<Common_n>((sender as ElectronSymbol).G3E_FID);
            if (com != null)
            {
                e.SBMC = com.SBMC;
                string str = "高压电机符号";
                str += string.IsNullOrEmpty(com.CD_SMZQ) ? "" : "-" + com.CD_SMZQ;

                e.color = DBEntityFinder.Instance.GetPointStyle(e.color, str);
            }
            else
            {
                e.color = CADColor.FromColor(Color.FromArgb(CABLEManager._color0));
            }
        }

        /******************************************************/

        [Execution(73)]
        public static void Gg_pd_dldy_pt_sdogeom(object sender, SymbolEventArgs e)
        {
            e.color = CADColor.FromColor(Color.FromArgb(CABLEManager._color2));
            var com = e.DBManager.GetEntity<Common_n>((sender as ElectronSymbol).G3E_FID);
            if (com != null)
            {
                e.SBMC = com.SBMC;

                //string str = string.Format("低压断连符号");
                //var style = DBManager.Instance.GetEntity<G3e_pointstyle>(o => o.G3E_USERNAME == str);
                //if (style == null) return;
                //if (style.G3E_COLOR != null) e.color = CADColor.FromColor(Color.FromArgb((int)style.G3E_COLOR));
            }
            else
            {
                e.color = CADColor.FromColor(Color.FromArgb(CABLEManager._color0));
            }
        }

        [Execution(74)]
        public static void Gg_pd_dyzd_pt_sdogeom(object sender, SymbolEventArgs e)
        {
            e.color = CADColor.FromColor(Color.FromArgb(CABLEManager._color2));
            var com = e.DBManager.GetEntity<Common_n>((sender as ElectronSymbol).G3E_FID);
            var cn = e.DBManager.GetEntity<Gg_pd_dyzd_n>((sender as ElectronSymbol).G3E_FID);
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
            else
            {
                e.color = CADColor.FromColor(Color.FromArgb(CABLEManager._color0));
            }
        }

        [Execution(155)]
        public static void Gg_pd_dykg_pt_sdogeom(object sender, SymbolEventArgs e)
        {
            e.color = CADColor.FromColor(Color.FromArgb(CABLEManager._color2));
            var cn = e.DBManager.GetEntity<Gg_pd_dykg_n>((sender as ElectronSymbol).G3E_FID);
            var com = e.DBManager.GetEntity<Common_n>((sender as ElectronSymbol).G3E_FID);
            var con = e.DBManager.GetEntity<Connectivity_n>((sender as ElectronSymbol).G3E_FID);
            if (com != null && com != null && con != null)
            {
                e.SBMC = com.SBMC;
                switch (cn.CD_DYKGFL)
                {
                    case "断路器":
                        e.SymbolType = Zero;
                        break;
                    case "刀闸":
                        e.SymbolType = Two;
                        break;
                    case "低压柜刀闸":
                        e.SymbolType = One;
                        break;
                    case "塑壳开关":
                        e.SymbolType = Zero;
                        break;
                    case "隔离器":
                        e.SymbolType = Zero;
                        break;
                    case "空气开关":
                        e.SymbolType = Three;
                        break;
                    case "条形开关":
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

        [Execution(158)]
        public static void Gg_pd_dyzjjt_pt_sdogeom(object sender, SymbolEventArgs e)
        {
            e.color = CADColor.FromColor(Color.FromArgb(CABLEManager._color2));
            var com = e.DBManager.GetEntity<Common_n>((sender as ElectronSymbol).G3E_FID);
            if (com != null)
            {
                e.SBMC = com.SBMC;
                string str = "低压电缆中间接头符号";
                str += string.IsNullOrEmpty(com.CD_SMZQ) ? "" : "-" + com.CD_SMZQ;


                e.color = DBEntityFinder.Instance.GetPointStyle(e.color, str);
            }
            else
            {
                e.color = CADColor.FromColor(Color.FromArgb(CABLEManager._color0));
            }
        }

        [Execution(159)]
        public static void Gg_pd_cbx_pt_sdogeom(object sender, SymbolEventArgs e)
        {
            var g3e_fid = (sender as ElectronSymbol).G3E_FID;
            e.color = CADColor.FromColor(Color.FromArgb(CABLEManager._color2));
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
                var con = DBEntityFinder.Instance.GetConnectivity_n(g3e_fid);
                if(con!=null)
                {
                    if (cn.LX.Equals("集抄箱") &&con.CD_SFDD. Equals("不带电"))
                    {
                        str = "低压集中抄表箱符号-停电";
                    }
                else
                    if (cn.LX.Equals("复录仪") && con.CD_SFDD.Equals("不带电"))
                        str = string.Format("{0}-停电", str);
                }
                e.color = DBEntityFinder.Instance.GetPointStyle(e.color, str);
            }
        }


        [Execution(169)]
        public static void Gg_pd_dydlt_pt_sdogeom(object sender, SymbolEventArgs e)
        {
            e.color = CADColor.FromColor(Color.FromArgb(CABLEManager._color2));
            var cn = e.DBManager.GetEntity<Gg_pd_dydlt_n>((sender as ElectronSymbol).G3E_FID);
            var com = e.DBManager.GetEntity<Common_n>((sender as ElectronSymbol).G3E_FID);
            if (com != null && com != null)
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
                if(!string.IsNullOrEmpty(com.CD_SMZQ))
                {
                    str =string.Format("{0}-{1}",str,com.CD_SMZQ);
                }
                if(!string.IsNullOrEmpty(cn.CD_SFSDL))
                {
                    str =string.Format("{0}-{1}",str, cn.CD_SFSDL == "双电缆头" ? "双头" : "单头");
                }

                DBEntityFinder.Instance.GetPointStyle(e.color, str);
            }
        }

        [Execution(175)]
        public static void Gg_pd_dll_pt_sdogeom(object sender, SymbolEventArgs e)
        {
            e.color = CADColor.FromColor(Color.FromArgb(CABLEManager._color3));
            var com = e.DBManager.GetEntity<Common_n>((sender as ElectronSymbol).G3E_FID);
            if (com != null)
            {
                e.SBMC = com.SBMC;
            }
            else
            {
                e.color = CADColor.FromColor(Color.FromArgb(CABLEManager._color0));
            }
        }

        [Execution(205)]
        public static void Gg_gz_rj_pt_sdogeom(object sender, SymbolEventArgs e)
        {
            e.color = CADColor.FromColor(Color.FromArgb(CABLEManager._color3));
            var com = e.DBManager.GetEntity<Common_n>((sender as ElectronSymbol).G3E_FID);
            if (com != null)
            {
                e.SBMC = com.SBMC;
            }
            else
            {
                e.color = CADColor.FromColor(Color.FromArgb(CABLEManager._color0));
            }
        }

        [Execution(215)]
        public static void Gg_gz_dydg_pt_sdogeom(object sender, SymbolEventArgs e)
        {
            try
            {
                e.color = CADColor.FromColor(Color.FromArgb(CABLEManager._color2));
                var com = e.DBManager.GetEntity<Common_n>((sender as ElectronSymbol).G3E_FID);
                if (com != null)
                {
                    e.SBMC = com.SBMC;
                }
                else
                {
                    e.color = CADColor.FromColor(Color.FromArgb(CABLEManager._color0));
                }
            }
            catch (Exception ex)
            {
                LogManager.Instance.Error(ex);
            }
        }

        [Execution(218)]
        public static void Gg_gz_dyzc_pt_sdogeom(object sender, SymbolEventArgs e)
        {
            e.color = CADColor.FromColor(Color.FromArgb(CABLEManager._color2));
            var com = e.DBManager.GetEntity<Common_n>((sender as ElectronSymbol).G3E_FID);
            if (com != null)
            {
                e.SBMC = com.SBMC;
                string str = "低压支撑符号";
                if(!string.IsNullOrEmpty(com.CD_SMZQ))
                {
                    str = string.Format("{0}-{1}", str, com.CD_SMZQ);
                }
                DBEntityFinder.Instance.GetPointStyle(e.color, str);
            }
        }

        [Execution(320)]
        public static void Gg_pd_jszz_pt_sdogeom(object sender, SymbolEventArgs e)
        {
            e.color = CADColor.FromColor(Color.FromArgb(CABLEManager._color3));
            var com = e.DBManager.GetEntity<Common_n>((sender as ElectronSymbol).G3E_FID);
            if (com != null)
            {
                e.SBMC = com.SBMC;
            }
            e.color = CADColor.FromColor(Color.FromArgb(CABLEManager._color0));
        }

        [Execution(385)]
        public static void Gg_pd_fkjc_pt_sdogeom(object sender, SymbolEventArgs e)
        {
            e.color = CADColor.FromColor(Color.FromArgb(CABLEManager._color3));
            var com = e.DBManager.GetEntity<Common_n>((sender as ElectronSymbol).G3E_FID);
            if (com != null)
            {
                e.SBMC = com.SBMC;
            }
            else
            {
                e.color = CADColor.FromColor(Color.FromArgb(CABLEManager._color0));
            }
        }

        #endregion

        #region 面符号

        [Execution(77)]
        public static void Gg_pd_dlfxx_ar_sdogeom(object sender, SymbolEventArgs e)
        {
            e.color = CADColor.FromRgb(0, 0, 255);
            var com = e.DBManager.GetEntity<Common_n>((sender as ElectronSymbol).G3E_FID);
            if (com != null)
                e.SBMC = com.SBMC;
        }

        [Execution(81)]
        public static void Gg_pd_dyg_ar_sdogeom(object sender, SymbolEventArgs e)
        {
            e.color = CADColor.FromColor(Color.FromArgb(CABLEManager._color2));

            var com = e.DBManager.GetEntity<Common_n>((sender as ElectronSymbol).G3E_FID);
            if (com != null)
            {
                e.SBMC = com.SBMC;

                string str = "低压柜符号";
                if (!string.IsNullOrEmpty(com.CD_SMZQ))
                {
                    str += "-" + com.CD_SMZQ;
                }
                DBEntityFinder.Instance.GetAreaStyle(e.color, str);
            }
        }

        [Execution(142)]
        public static void Gg_pd_gydf_ar_sdogeom(object sender, SymbolEventArgs e)
        {
            e.color = CADColor.FromColor(Color.FromArgb(CABLEManager._color3));

            var com = e.DBManager.GetEntity<Common_n>((sender as ElectronSymbol).G3E_FID);
            if (com != null)
            {
                e.SBMC = com.SBMC;

                string str = "公用电房符号";

                var gn =e.DBManager.GetEntity<Gg_pd_gnwzmc_n>((sender as ElectronSymbol).G3E_FID);
                if (gn != null &&
                    ((gn.GNWZ_YFZNMS == "是") || (gn.GNWZ_YFZNMS == "有"))
                    )
                {
                    str += "-带锁";
                }
                if (!string.IsNullOrEmpty(com.CD_SMZQ))
                {
                    str += "-" + com.CD_SMZQ;
                }
                DBEntityFinder.Instance.GetAreaStyle(e.color, str);
            }
        }

        [Execution(149)]
        public static void Gg_pd_xssb_ar_sdogeom(object sender, SymbolEventArgs e)
        {
            e.color = CADColor.FromColor(Color.FromArgb(CABLEManager._color3));

            var com = e.DBManager.GetEntity<Common_n>((sender as ElectronSymbol).G3E_FID);
            if (com != null)
            {
                e.SBMC = com.SBMC;

                e.SBMC = com.SBMC;
                string str = "箱式设备符号";
                if (!string.IsNullOrEmpty(com.CD_SMZQ))
                {
                    str += "-" + com.CD_SMZQ;
                }
                DBEntityFinder.Instance.GetAreaStyle(e.color, str);
            }
        }

        [Execution(163)]
        public static void Gg_pd_zydf_ar_sdogeom(object sender, SymbolEventArgs e)
        {
            e.color = CADColor.FromColor(Color.FromArgb(CABLEManager._color3));
            e.lineWidth = 0;
            var com = e.DBManager.GetEntity<Common_n>((sender as ElectronSymbol).G3E_FID);
            if (com != null)
            {
                e.SBMC = com.SBMC;

                string str = "专用电房符号";

                var gn =e.DBManager.GetEntity<Gg_pd_gnwzmc_n>((sender as ElectronSymbol).G3E_FID);
                if (gn != null &&
                    ((gn.GNWZ_YFZNMS == "是") || (gn.GNWZ_YFZNMS == "有"))
                    )
                {
                    str =string.Format("{0}-带锁",str);
                }
                if (!string.IsNullOrEmpty(com.CD_SMZQ))
                {
                    str = string.Format("{0}-{1}", str,com.CD_SMZQ);
                }
                DBEntityFinder.Instance.GetAreaStyle(e.color, str);
            }
        }


        [Execution(302)]
        public static void Gg_pd_dypdf_ar_sdogeom(object sender, SymbolEventArgs e)
        {
            e.color = CADColor.FromColor(Color.FromArgb(CABLEManager._color2));
            var com = e.DBManager.GetEntity<Common_n>((sender as ElectronSymbol).G3E_FID);
            if (com != null)
                e.SBMC = com.SBMC;
        }


        [Execution(71)]
        public static void Gg_pd_cqtg_pt_sdogeom(object sender, SymbolEventArgs e)
        {
            e.color = Autodesk.AutoCAD.Colors.Color.FromColor(Color.FromArgb(CABLEManager._color3));
            var com = DBManager.Instance.GetEntity<Common_n>((sender as ElectronSymbol).G3E_FID);
            if (com != null)
            {
                e.SBMC = com.SBMC;
                string str = "穿墙套管符号";
                str = string.Format("{0}-{1}", str, com.CD_SMZQ);
                e.color = DBEntityFinder.Instance.GetPointStyle(e.color, str);
            }
        }
        [Execution(72)]
        public static void Gg_pd_jdgh_pt_sdogeom(object sender, SymbolEventArgs e)
        {
            e.color = Autodesk.AutoCAD.Colors.Color.FromColor(Color.FromArgb(CABLEManager._color3));
            var com = e.DBManager.GetEntity<Common_n>((sender as ElectronSymbol).G3E_FID);
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
            e.color = Autodesk.AutoCAD.Colors.Color.FromColor(Color.FromArgb(CABLEManager._color3));
            var com = e.DBManager.GetEntity<Common_n>((sender as ElectronSymbol).G3E_FID);
            if (com != null)
            {
                e.SBMC = com.SBMC;
                string str = "关口计量柜符号";
                str = string.Format("{0}-{1}", str, com.CD_SMZQ);
                e.color = DBEntityFinder.Instance.GetPointStyle(e.color, str);
            }
        }

        [Execution(183)]
        public static void Gg_jc_qyjcy_pt_sdogeom(object sender, SymbolEventArgs e)
        {
            e.color = Autodesk.AutoCAD.Colors.Color.FromColor(Color.FromArgb(CABLEManager._color3));
            var com = e.DBManager.GetEntity<Common_n>((sender as ElectronSymbol).G3E_FID);
            if (com != null)
            {
                e.SBMC = com.SBMC;
                string str = "气压监测仪符号";
                str = string.Format("{0}-{1}", str, com.CD_SMZQ);
                e.color = DBEntityFinder.Instance.GetPointStyle(e.color,str);
            }
        }

        [Execution(184)]
        public static void Gg_jc_yyjcy_pt_sdogeom(object sender, SymbolEventArgs e)
        {
            e.color = Autodesk.AutoCAD.Colors.Color.FromColor(Color.FromArgb(CABLEManager._color3));
            var com = e.DBManager.GetEntity<Common_n>((sender as ElectronSymbol).G3E_FID);
            if (com != null)
            {
                e.SBMC = com.SBMC;
                string str = "油压监测仪符号";
                str = string.Format("{0}-{1}", str, com.CD_SMZQ);
                e.color = DBEntityFinder.Instance.GetPointStyle(e.color, str);
            }
        }

        [Execution(185)]
        public static void Gg_jc_dyjcy_pt_sdogeom(object sender, SymbolEventArgs e)
        {
            e.color = Autodesk.AutoCAD.Colors.Color.FromColor(Color.FromArgb(CABLEManager._color3));
            var com = e.DBManager.GetEntity<Common_n>((sender as ElectronSymbol).G3E_FID);
            if (com != null)
            {
                e.SBMC = com.SBMC;
                string str = "电压监测仪符号";
                str = string.Format("{0}-{1}", str, com.CD_SMZQ);
                e.color = DBEntityFinder.Instance.GetPointStyle(e.color, str);
            }
        }


        [Execution(186)]
        public static void Gg_jc_ldkgx_pt_sdogeom(object sender, SymbolEventArgs e)
        {
            e.color = Autodesk.AutoCAD.Colors.Color.FromColor(Color.FromArgb(CABLEManager._color3));
            var com = e.DBManager.GetEntity<Common_n>((sender as ElectronSymbol).G3E_FID);
            if (com != null)
            {
                e.SBMC = com.SBMC;
                string str = "路灯开关箱符号";
                str = string.Format("{0}-{1}", str, com.CD_SMZQ);
                e.color = DBEntityFinder.Instance.GetPointStyle(e.color, str);
            }
        }

        [Execution(188)]
        public static void Gg_jc_fk_pt_sdogeom(object sender, SymbolEventArgs e)
        {
            e.color = Autodesk.AutoCAD.Colors.Color.FromColor(Color.FromArgb(CABLEManager._color3));
            var com = e.DBManager.GetEntity<Common_n>((sender as ElectronSymbol).G3E_FID);
            if (com != null)
            {
                e.SBMC = com.SBMC;
                string str = "负控符号";
                str = string.Format("{0}-{1}", str, com.CD_SMZQ);
                e.color = DBEntityFinder.Instance.GetPointStyle(e.color, str);
            }
        }
        #endregion
    }
}
