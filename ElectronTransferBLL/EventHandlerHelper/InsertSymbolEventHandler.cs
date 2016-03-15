using ElectronTransferDal.Factory;
using ElectronTransferDal.HelperEventArgs;
using ElectronTransferDal.Query;
using ElectronTransferFramework;
using ElectronTransferModel.Geo;
using CADColor = Autodesk.AutoCAD.Colors.Color;
using Color = System.Drawing.Color;
using ElectronTransferDal.AutoGeneration;

namespace ElectronTransferBll.EventHandlerHelper
{
    [ExecutionContainer(typeof(InsertSymbolExecutionFactory))]
    public class InsertSymbolEventHandler
    {
        #region 线
        //0804
        [Execution(140)]
        public static void Gg_pd_dl_ln_sdogeom(object sender, InsertSymbolEventArgs e)
        {
            e = GetLineDefaultStyle(e, false, CABLEManager._lineWidth002, CABLEManager._color3);

            string str = "10kV电缆符号";
            str=GetUserNameBySMZQorSFDD(e.symbolObj,str);
            var cxd = GetCXD(e.symbolObj,140);
            if (!string.IsNullOrEmpty(cxd))
            {
                if (cxd == "粗")
                    str += "-" + cxd;
            }
            DBEntityFinder.Instance.GetLineStyle(e, str);
        }

        [Execution(141)]
        public static void Gg_pd_dx_ln_sdogeom(object sender, InsertSymbolEventArgs e)
        {
            e = GetLineDefaultStyle(e, true, CABLEManager._lineWidth002, CABLEManager._color3);

            string str = "10kV导线符号";
            str = GetUserNameBySMZQorSFDD(e.symbolObj, str);
            var cxd = GetCXD(e.symbolObj,141);
            if (!string.IsNullOrEmpty(cxd))
            {
                if (cxd == "粗")
                    str += "-" + cxd;
            }
            DBEntityFinder.Instance.GetLineStyle(e, str);
        }

        [Execution(143)]
        public static void Gg_pd_zfmx_ln_sdogeom(object sender, InsertSymbolEventArgs e)
        {
            string str = "站房母线符号";
            e = GetLineDefaultStyle(e, true, CABLEManager._lineWidth004, CABLEManager._color3);


            str = GetUserNameBySMZQorSFDD(e.symbolObj, str);
            if (str.IndexOf("-投运") > 0)
            {
                var cddydj = GetDYDJ(e.symbolObj);
                if (cddydj != null)
                {
                    if (cddydj == "0.4")
                    {
                        str = "站房母线符号-400V投运";
                    }
                    else if (cddydj== "10")
                    {
                        str = "站房母线符号-10kV投运";
                    }
                }
            }
            DBEntityFinder.Instance.GetLineStyle(e, str);
        }
        [Execution(144)]
        public static void Gg_pd_zfyx_ln_sdogeom(object sender, InsertSymbolEventArgs e)
        {
            e = GetLineDefaultStyle(e, true, CABLEManager._lineWidth001, CABLEManager._color3);
            
            string str = "站房引线符号";

            str = GetUserNameBySMZQorSFDD(e.symbolObj, str);
            if (str.IndexOf("-投运") > 0)
            {
                var cddydj = GetDYDJ(e.symbolObj);
                if (cddydj != null)
                {
                    if (cddydj == "0.4")
                    {
                        str = "站房引线符号-400V投运";
                    }
                    else if (cddydj == "10")
                    {
                        str = "站房引线符号-10kV投运";
                    }
                }
            }
            DBEntityFinder.Instance.GetLineStyle(e, str);
        }
        [Execution(79)]
        public static void Gg_pd_dymx_ln_sdogeom(object sender, InsertSymbolEventArgs e)
        {
            GetLineDefaultStyle(e, true, CABLEManager._lineWidth004, CABLEManager._color2);
 
            //数据库中只有一种默认样式
            //string str = "低压母线符号";
            //if (!string.IsNullOrEmpty((string)e.symbolObj.GetValue("CD_SMZQ")))
            //{
            //    str += "-" + e.symbolObj.GetValue("CD_SMZQ");
            //}
            //var style = DBManager.Instance.GetEntity<G3e_linestyle>(o => o.G3E_USERNAME == str);
            //if (style == null) return;
            //if (style.G3E_WIDTH != null) e.lineWidth = (double)style.G3E_WIDTH;
            //if (style.G3E_COLOR != null) e.symbolColor = CADColor.FromColor(Color.FromArgb((int)style.G3E_COLOR));
        }
        [Execution(156)]
        public static void Gg_pd_dydx_ln_sdogeom(object sender, InsertSymbolEventArgs e)
        {
            e = GetLineDefaultStyle(e, true, CABLEManager._lineWidth002, CABLEManager._color2);
            
            string str = "低压导线符号";

            str = GetUserNameBySMZQorSFDD(e.symbolObj, str);
            DBEntityFinder.Instance.GetLineStyle(e, str);
        }
        [Execution(157)]
        public static void Gg_pd_dydl_ln_sdogeom(object sender, InsertSymbolEventArgs e)
        {
            e = GetLineDefaultStyle(e, false, CABLEManager._lineWidth002, CABLEManager._color2);
            
            string str = "低压电缆符号";
            str = GetUserNameBySMZQorSFDD(e.symbolObj, str);
            DBEntityFinder.Instance.GetLineStyle(e, str);
        }
        [Execution(80)]
        public static void Gg_pd_bqx_ln_sdogeom(object sender, InsertSymbolEventArgs e)
        {
            e = GetLineDefaultStyle(e, true, CABLEManager._lineWidth002, CABLEManager._color2);
            
            string str = "表前线符号";
            str = GetUserNameBySMZQorSFDD(e.symbolObj, str);
            DBEntityFinder.Instance.GetLineStyle(e, str);
        }
        [Execution(75)]
        public static void Gg_pd_bdcx_pt_sdogeom(object sender, InsertSymbolEventArgs e)
        {
            GetLineDefaultStyle(e, true, CABLEManager._lineWidth002, CABLEManager._color2);
            
        }
        #endregion

        #region 点
        [Execution(40)]
        public static void Gg_pd_dlhgq_pt_sdogeom(object sender, InsertSymbolEventArgs e)
        {
            e = GetPointStyle(e, CABLEManager._color3);
           
            string str = "电流互感器符号";
            var smzq = GetCD_SMZQ(e.symbolObj);
            if (smzq != null)
            {
                str += smzq;
            }
            e.symbolColor = DBEntityFinder.Instance.GetPointStyle(e.symbolColor, str);
         }

        [Execution(82)]
        public static void Gg_jx_gyb_pt_sdogeom(object sender, InsertSymbolEventArgs e)
        {
            e = GetPointStyle(e, CABLEManager._color3);
            string str = "高压表符号";
            var smzq = GetCD_SMZQ(e.symbolObj);
            var sfdd = GetCD_SFDD(e.symbolObj);
            if (smzq.Equals("停运"))
            {
                str += "-停运";
            }
            else if (smzq.Equals("待投运"))
            {
                str += "-待投运";
            }
            else
            {
                str =string.Format("{0}-{1}-{2}",str,smzq , sfdd) ;
            }
            e.symbolColor = DBEntityFinder.Instance.GetPointStyle(e.symbolColor, str);
        }

        [Execution(84)]
        public static void Gg_pd_cjlg_pt_sdogeom(object sender, InsertSymbolEventArgs e)
        {
            e = GetPointStyle(e, CABLEManager._color3);
            var sblx = GenerateHelper.GetPropertyValue(e.symbolObj, GenerateHelper.GetSelfTableNameByFno(e.g3e_fno), "SBLX");
            if (sblx != null)
            {
                switch (sblx.ToString())
                {
                    case "抽屉式计量柜":
                        e.bTypeStr = string.Format("{0}", 1);
                        break;
                    case "固定式计量柜":
                        e.bTypeStr = string.Format("{0}", 0);
                        break;
                    case "计量箱":
                        e.bTypeStr = string.Format("{0}", 0);
                        break;
                }
                var str = string.Format("{0}符号", sblx);

                var smzq = GetCD_SMZQ(e.symbolObj);
                var sfdd = GetCD_SFDD(e.symbolObj);
                if (!string.IsNullOrEmpty(smzq))
                {
                    if (smzq.Equals("停运"))
                    {
                        str += "-停运";
                    }
                    else
                        str = string.Format("{0}-{1}", str, smzq);

                    if (smzq.Equals("投运"))
                    {
                        str = string.Format("{0}-{1}", str, sfdd);
                    }
                }
                e.symbolColor = DBEntityFinder.Instance.GetPointStyle(e.symbolColor, str);
            }
        }


        [Execution(85)]
        public static void Gg_pd_ptg_pt_sdogeom(object sender, InsertSymbolEventArgs e)
        {
            e = GetPointStyle(e, CABLEManager._color3);

            string str="PT柜符号";
            str += "-" + GetCD_SMZQ(e.symbolObj);
            if (str.IndexOf("-投运") > 0)
            {
                str += "-" + GetCD_SFDD(e.symbolObj);
            }
            e.symbolColor = DBEntityFinder.Instance.GetPointStyle(e.symbolColor, str);
        }

        [Execution(90)]
        public static void Gg_jc_dtu_pt_sdogeom(object sender, InsertSymbolEventArgs e)
        {
            e = GetPointStyle(e, CABLEManager._color3);
            string str = "DTU符号";
            var smzq = GetCD_SMZQ(e.symbolObj);
            if (!string.IsNullOrEmpty(smzq))
                str = string.Format("{0}-{1}", str, smzq);
            e.symbolColor = DBEntityFinder.Instance.GetPointStyle(e.symbolColor,str);
        }

        [Execution(145)]
        public static void Gg_pd_zfllkg_pt_sdogeom(object sender, InsertSymbolEventArgs e)
        {
            e = GetPointStyle(e, CABLEManager._color3);
          
            string str = "站房联络开关符号";
            str += "-" + GetCD_SMZQ(e.symbolObj);
            var dqzt = GetDQZT(e.symbolObj);
            if (!string.IsNullOrEmpty(dqzt))
            {
                if (str.IndexOf("-投运") > 0)
                {
                    var dydj = GetDYDJ(e.symbolObj);
                    if (dydj == "0.4")
                        str += "-400" + GetDQZT(e.symbolObj);
                    else if (dydj == "10")
                        str += "-10kV" + GetDQZT(e.symbolObj);
                }
                if (str.IndexOf("-停运") > 0)
                {
                    str += "-" + GetDQZT(e.symbolObj);
                }
            }
            e.symbolColor = DBEntityFinder.Instance.GetPointStyle(e.symbolColor, str);
        }

        [Execution(146)]
        public static void Gg_pd_zfhwg_pt_sdogeom(object sender, InsertSymbolEventArgs e)
        {
            e = GetPointStyle(e, CABLEManager._color3);
            var zfhwgfl = GenerateHelper.GetPropertyValue(e.symbolObj, GenerateHelper.GetGnwzTableNameByFno(e.g3e_fno), "GNWZ_FL");
            if (zfhwgfl != null)
            {
                switch (zfhwgfl.ToString())
                {
                    case "断路器":
                    case "固定式断路器":
                        e.bTypeStr = string.Format("{0}", 0);
                        break;
                    case "刀闸":
                        e.bTypeStr = string.Format("{0}", 1);
                        break;
                    case "跌落式熔断器":
                        e.bTypeStr = string.Format("{0}", 2);
                        break;
                    case "断路器小车式":
                        e.bTypeStr = string.Format("{0}", 3);
                        break;
                    case "负荷开关":
                        e.bTypeStr = string.Format("{0}", 4);
                        break;
                    case "带熔丝负荷开关":
                    case "不带开关的手车柜":
                        e.bTypeStr = string.Format("{0}", 5);
                        break;
                    case "副柜":
                        e.bTypeStr = string.Format("{0}", 7);
                        break;
                    case "简易开关柜":
                        e.bTypeStr = string.Format("{0}", 6);
                        break;
                    case "隔离开关":
                        e.bTypeStr = string.Format("{0}", 8);
                        break;
                    case "自动化负荷开关":
                        e.bTypeStr = string.Format("{0}", 9);
                        break;
                    case "自动化断路器":
                        e.bTypeStr = string.Format("{0}", 10);
                        break;
                    default:
                        e.bTypeStr = string.Format("{0}", 0);
                        break;
                }
                //副柜和简易开关柜没有闭合状态
                if (!e.bTypeStr.Equals("6") && !e.bTypeStr.Equals("7"))
                {
                    e.bTypeStr = string.Format("{0}_{1}", e.bTypeStr, GetDQZT(e.symbolObj) != "闭合" ? "0" : "1");
                }
            }
            var str = "10kV开关符号";
            str = string.Format("{0}-{1}", str, GetCD_SMZQ(e.symbolObj));
            var dqzt = GetDQZT(e.symbolObj);
            var sfdd = GetCD_SFDD(e.symbolObj);
            if (dqzt != null)
            {
                if (str.Contains("停运") || sfdd.Equals("不带电"))
                {
                    str = string.Format("10kV开关符号-停运-{0}", dqzt);
                }
                else if (str.Contains("投运"))
                {
                    str = string.Format("{0}-{1}", str, dqzt);
                }
            }
            e.symbolColor = DBEntityFinder.Instance.GetPointStyle(e.symbolColor, str);
        }

        [Execution(147)]
        public static void Gg_pd_zfdlt_pt_sdogeom(object sender, InsertSymbolEventArgs e)
        {
            e = GetPointStyle(e, CABLEManager._color3);
            var dltlx = GenerateHelper.GetPropertyValue(e.symbolObj, GenerateHelper.GetSelfTableNameByFno(e.g3e_fno), "CD_SFSDL");
            if (dltlx != null)
            {
                switch (dltlx.ToString())
                {
                    case "双电缆头":
                        e.bTypeStr = string.Format("{0}", 1);
                        break;
                    case "单电缆头":
                        e.bTypeStr = string.Format("{0}", 0);
                        break;
                    default:
                        e.bTypeStr = string.Format("{0}", 0);
                        break;
                }
                string str = "站房电缆头符号";
                str = string.Format("{0}-{1}", str, GetCD_SMZQ(e.symbolObj));
                var dlt = (string) dltlx == "双电缆头" ? "双头" : "单头";
                //停运和不带电情况
                if (str.Contains("停运"))
                {
                    str = string.Format("站房电缆头符号-停运-{0}", dlt);
                }
                else
                    if (str.IndexOf("-投运") > 0)
                    {
                        var dj = GetDYDJ(e.symbolObj);
                        str = string.Format("{0}-{1}{2}", str, dj=="0.4"?"400V":(dj=="10"?"10kV":""), dlt);
                    }
                    else
                    {
                        if(!string.IsNullOrEmpty(GetDYDJ(e.symbolObj)))
                            str = string.Format("{0}-{1}", str, dlt);
                    }
                e.symbolColor = DBEntityFinder.Instance.GetPointStyle(e.symbolColor, str);
            }
        }

        [Execution(148)]
        public static void Gg_pd_zfbyq_pt_sdogeom(object sender, InsertSymbolEventArgs e)
        {
            e = GetPointStyle(e, CABLEManager._color3);
            var pblb = GenerateHelper.GetPropertyValue(e.symbolObj, GenerateHelper.GetSelfTableNameByFno(e.g3e_fno), "CD_PBLB");
            if (pblb != null && !string.IsNullOrEmpty(pblb.ToString()))
            {
                switch (pblb.ToString())
                {
                    case "公变":
                        e.bTypeStr = string.Format("{0}", 0);
                        break;
                    case "专变":
                        e.bTypeStr = string.Format("{0}", 1);
                        break;
                    case "农变":
                        e.bTypeStr = string.Format("{0}", 2);
                        break;
                    case "专改公":
                        e.bTypeStr = string.Format("{0}", 3);
                        break;
                    case "其他":
                        e.bTypeStr = string.Format("{0}", 4);
                        break;
                    default:
                        e.bTypeStr = string.Format("{0}",1);
                        break;
                }                
            }
            var str = "站房变压器符号";

            str = GetUserNameBySMZQorSFDD(e.symbolObj, str);
            var pblb1 = GenerateHelper.GetPropertyValue(e.symbolObj, GenerateHelper.GetSelfTableNameByFno(e.g3e_fno), "CD_PBLB");
            if(pblb1!=null)
                pblb = pblb1.ToString().Replace("他", "它");
            str += "-" + pblb;
            e.symbolColor = DBEntityFinder.Instance.GetPointStyle(e.symbolColor, str);
        }

        [Execution(150)]
        public static void Gg_pd_dlzjjt_pt_sdogeom(object sender, InsertSymbolEventArgs e)
        {
            e = GetPointStyle(e, CABLEManager._color3);

            string str = "10kV电缆中间接头符号";
            var smzq = GetCD_SMZQ(e.symbolObj);
            if (!string.IsNullOrEmpty(smzq))
            {
                str = GetUserNameBySMZQorSFDD(e.symbolObj, str);
            }
            e.symbolColor = DBEntityFinder.Instance.GetPointStyle(e.symbolColor, str);
        }

        [Execution(151)]
        public static void Gg_pd_dlzdt_pt_sdogeom(object sender, InsertSymbolEventArgs e)
        {
            e = GetPointStyle(e, CABLEManager._color3);
            //var zdtxs = GenerateHelper.GetPropertyValue(e.symbolObj, GenerateHelper.GetSelfTableNameByFno(e.g3e_fno), "ZDTXS");
            //if (zdtxs != null)
            //{
            //    switch (zdtxs.ToString())
            //    {
            //        case "双电缆头":
            //            e.bTypeStr = string.Format("{0}", 1);
            //            break;
            //        case "单电缆头":
            //            e.bTypeStr = string.Format("{0}", 0);
            //            break;
            //        default:
            //            e.bTypeStr = string.Format("{0}", 0);
            //            break;
            //    }
            e.bTypeStr = string.Format("{0}", 0);
            var str = "10kV电缆终端头符号";
            if (!string.IsNullOrEmpty(GetCD_SMZQ(e.symbolObj)))
            {
                str = GetUserNameBySMZQorSFDD(e.symbolObj, str);
            }
            e.symbolColor = DBEntityFinder.Instance.GetPointStyle(e.symbolColor, str);
        }

        [Execution(154)]
        public static void Gg_pd_zsbyq_pt_sdogeom(object sender, InsertSymbolEventArgs e)
        {
            e = GetPointStyle(e, CABLEManager._color3);
          
            string str = "柱上变压器符号";
            str += "-" + GetCD_SMZQ(e.symbolObj);
            var pblb = GenerateHelper.GetPropertyValue(e.symbolObj, GenerateHelper.GetSelfTableNameByFno(e.g3e_fno), "CD_PBLB");
            if (pblb != null)
            {
                str += "-" + pblb;
                switch (pblb.ToString())
                {
                    case "公变":
                        e.bTypeStr = string.Format("{0}", 0);
                        break;
                    case "专变":
                        e.bTypeStr = string.Format("{0}", 1);
                        break;
                    case "农变":
                        e.bTypeStr = string.Format("{0}", 2);
                        break;
                    case "专改公":
                        e.bTypeStr = string.Format("{0}", 3);
                        break;
                    default:
                        e.bTypeStr = string.Format("{0}", 0);
                        break;
                }
            }
            e.symbolColor = DBEntityFinder.Instance.GetPointStyle(e.symbolColor, str);
        }

        [Execution(171)]
        public static void Gg_pd_zdfdj_pt_sdogeom(object sender, InsertSymbolEventArgs e)
        {
            e = GetPointStyle(e, CABLEManager._color3);

            string str = "用户自带发电机符号";
            str += string.Format("-{0}", GetCD_SMZQ(e.symbolObj));
           
            e.symbolColor = DBEntityFinder.Instance.GetPointStyle(e.symbolColor, str);
        }

        [Execution(172)]
        public static void Gg_pd_ddj_pt_sdogeom(object sender, InsertSymbolEventArgs e)
        {
            e = GetPointStyle(e, CABLEManager._color3);

            string str = "电动机符号";
            str += string.Format("-{0}", GetCD_SMZQ(e.symbolObj));
          
            e.symbolColor = DBEntityFinder.Instance.GetPointStyle(e.symbolColor, str);
        }

        [Execution(173)]
        public static void Gg_pd_zfjddz_pt_sdogeom(object sender, InsertSymbolEventArgs e)
        {
            e = GetPointStyle(e, CABLEManager._color3);
           
            var str = "站房接地刀闸符号";
            var smzq = GetCD_SMZQ(e.symbolObj);

            if (!string.IsNullOrEmpty(smzq))
            {
                str = GetUserNameBySMZQorSFDD(e.symbolObj, str);
            }
            var sgdw = GenerateHelper.GetPropertyValue(e.symbolObj, GenerateHelper.GetSelfTableNameByFno(e.g3e_fno), "SGDW");
            if (sgdw != null)
            {
                //接地刀闸的自身属性表的样式字段和公共属性表的施工单位字段一样，批量修改公共属性的施工单位时，导致接地刀闸样式改变
                if (sgdw.Equals("新") || sgdw.Equals("旧"))
                {
                    e.bTypeStr = string.Format("{0}", sgdw.ToString() != "新" ? "1" : "0");
                }
            }
            var dqzt = GetDQZT(e.symbolObj);
            if (dqzt != null)
            {
                if (e.bTypeStr != null)
                {
                    e.bTypeStr += string.Format("{0}", dqzt != "闭合" ? "_0" : "_1");
                }
                str += "-" + dqzt;
            }
            //缺少公共区域字段
            e.symbolColor = DBEntityFinder.Instance.GetPointStyle(e.symbolColor, str);
        }

        [Execution(174)]
        public static void Gg_pd_wgbc_pt_sdogeom(object sender, InsertSymbolEventArgs e)
        {
            e = GetPointStyle(e, CABLEManager._color3);

            string str = "无功补偿器符号";
            var smzq = GetCD_SMZQ(e.symbolObj);
            if (!string.IsNullOrEmpty(smzq))
            {
                str = GetUserNameBySMZQorSFDD(e.symbolObj, str);
            }
            e.symbolColor = DBEntityFinder.Instance.GetPointStyle(e.symbolColor, str);
        }

        [Execution(175)]
        public static void Gg_pd_dll_pt_sdogeom(object sender, InsertSymbolEventArgs e)
        {
            e = GetPointStyle(e, CABLEManager._color3);
            var str = "10kV断连符号";
            var smzq = GetCD_SMZQ(e.symbolObj);
            if (!string.IsNullOrEmpty(smzq))
            {
                str = GetUserNameBySMZQorSFDD(e.symbolObj, str);
            }
            e.symbolColor = DBEntityFinder.Instance.GetPointStyle(e.symbolColor, str);
        }

        [Execution(176)]
        public static void Gg_pd_jdk_pt_sdogeom(object sender, InsertSymbolEventArgs e)
        {
            e = GetPointStyle(e, CABLEManager._color3);

            string str = "关口计量柜符号";
            str += string.Format("-{0}", GetCD_SMZQ(e.symbolObj));


            e.symbolColor = DBEntityFinder.Instance.GetPointStyle(e.symbolColor, str);
        }

        [Execution(177)]
        public static void Gg_pd_blq_pt_sdogeom(object sender, InsertSymbolEventArgs e)
        {
            e = GetPointStyle(e, CABLEManager._color3);
            var azwz = GenerateHelper.GetPropertyValue(e.symbolObj, GenerateHelper.GetGnwzTableNameByFno(e.g3e_fno), "GNWZ_FL2");
            if (azwz != null)
            {
                switch (azwz.ToString())
                {
                    case "房内":
                    case "户内":
                    case "箱式":
                        e.bTypeStr = string.Format("{0}", 0);
                        break;
                    //case "户外":
                    //case "柱上":
                    //    e.bTypeStr = string.Format("{0}", 1);
                    //    break;
                    default:
                        e.bTypeStr = string.Format("{0}", 1);
                        break;
                }
                string str = "避雷器符号";
                var smzq = GetCD_SMZQ(e.symbolObj);
                if (!string.IsNullOrEmpty(smzq))
                {
                    if (azwz.ToString().Equals("柱上"))
                    {
                        str = string.Format("{0}{1}", azwz, str);
                    }
                    else if (azwz.ToString().Equals("户内") || azwz.ToString().Equals("房内"))
                    {
                        str = string.Format("房内{0}", str);
                    }
                    else
                    {
                        str = string.Format("{0}{1}", azwz, str);
                    }

                    str = GetUserNameBySMZQorSFDD(e.symbolObj, str);
                }
                e.symbolColor = DBEntityFinder.Instance.GetPointStyle(e.symbolColor, str);

            }
        }


        [Execution(181)]
        public static void Gg_jc_pwy_pt_sdogeom(object sender, InsertSymbolEventArgs e)
        {
            e = GetPointStyle(e, CABLEManager._color3);

            string str = "配网仪符号";
            var smzq = GetCD_SMZQ(e.symbolObj);
            if (smzq != null)
            {
                str += "-" + smzq;
            }
            e.symbolColor = DBEntityFinder.Instance.GetPointStyle(e.symbolColor, str);
        }

        [Execution(182)]
        public static void Gg_jc_gzzsq_pt_sdogeom(object sender, InsertSymbolEventArgs e)
        {
            e = GetPointStyle(e, CABLEManager._color3);

            string str = "故障指示器符号";
            var smzq = GetCD_SMZQ(e.symbolObj);
            if (smzq != null)
            {
                str += "-" + smzq;
            }
            e.symbolColor = DBEntityFinder.Instance.GetPointStyle(e.symbolColor, str);
        }

        [Execution(201)]
        public static void Gg_gz_dg_pt_sdogeom(object sender, InsertSymbolEventArgs e)
        {
            e = GetPointStyle(e, CABLEManager._color3);
            var cllb = GenerateHelper.GetPropertyValue(e.symbolObj, GenerateHelper.GetSelfTableNameByFno(e.g3e_fno), "CD_CLLB");
            if (cllb != null)
            {
                switch (cllb.ToString())
                {
                    case "铁塔":
                    case "铁杆":
                        e.bTypeStr = string.Format("{0}", 2);
                        break;
                    //case "钢杆":
                    //    e.bTypeStr = string.Format("{0}", 0);
                    //    break;
                    case "水泥杆":
                        e.bTypeStr = string.Format("{0}", 1);
                        break;
                    default:
                        e.bTypeStr = string.Format("{0}", 0);
                        break;
                }
            }
            string str = "电杆符号";
            if (cllb!=null)
            {
                str += "-" + cllb;
            }
            var smzq = GetCD_SMZQ(e.symbolObj);
            if (!string.IsNullOrEmpty(smzq))
            {
                str += "-" + smzq;
            }
            e.symbolColor = DBEntityFinder.Instance.GetPointStyle(e.symbolColor, str);
        }

        [Execution(307)]
        public static void Gg_pd_dyhgq_pt_sdogeom(object sender, InsertSymbolEventArgs e)
        {
            e = GetPointStyle(e, CABLEManager._color3);
            
            string str = "电压互感器符号";
            var smzq = GetCD_SMZQ(e.symbolObj);
            if (!string.IsNullOrEmpty(smzq))
            {
                str += "-" + smzq;
            }
            e.symbolColor = DBEntityFinder.Instance.GetPointStyle(e.symbolColor, str);
        }

        [Execution(309)]
        public static void Gg_pd_gydj_pt_sdogeom(object sender, InsertSymbolEventArgs e)
        {
            e = GetPointStyle(e, CABLEManager._color3);
            string str = "高压电机符号";
            var smzq = GetCD_SMZQ(e.symbolObj);
            if (!string.IsNullOrEmpty(smzq))
            {
                str += "-" + smzq;
            }
            e.symbolColor = DBEntityFinder.Instance.GetPointStyle(e.symbolColor, str);
        }

        /******************************************************/

        [Execution(73)]
        public static void Gg_pd_dldy_pt_sdogeom(object sender, InsertSymbolEventArgs e)
        {
            GetPointStyle(e, CABLEManager._color2);
        }

        [Execution(74)]
        public static void Gg_pd_dyzd_pt_sdogeom(object sender, InsertSymbolEventArgs e)
        {
            e = GetPointStyle(e, CABLEManager._color2);
            var jdlx = GenerateHelper.GetPropertyValue(e.symbolObj, GenerateHelper.GetSelfTableNameByFno(e.g3e_fno), "JDLX");
            if (jdlx != null)
            {
                switch (jdlx.ToString())
                {
                    case "终端":
                        e.bTypeStr = string.Format("{0}", 0);
                        break;
                    case "联络终端":
                        e.bTypeStr = string.Format("{0}", 1);
                        break;
                    default:
                        e.bTypeStr = string.Format("{0}", 0);
                        break;
                }
            }
        }

        [Execution(155)]
        public static void Gg_pd_dykg_pt_sdogeom(object sender, InsertSymbolEventArgs e)
        {
            e = GetPointStyle(e, CABLEManager._color2);
            var dykgfl = GenerateHelper.GetPropertyValue(e.symbolObj, GenerateHelper.GetSelfTableNameByFno(e.g3e_fno), "CD_DYKGFL");
            if (dykgfl != null)
            {
                switch (dykgfl.ToString())
                {
                    case "断路器":
                        e.bTypeStr = string.Format("{0}", 0);
                        break;
                    case "低压柜刀闸":
                        e.bTypeStr = string.Format("{0}", 0);
                        break;
                    case "塑壳开关":
                        e.bTypeStr = string.Format("{0}", 0);
                        break;
                    case "空气开关":
                        e.bTypeStr = string.Format("{0}", 0);
                        break;
                    case "条形开关":
                        e.bTypeStr = string.Format("{0}", 0);
                        break;
                    case "隔离器":
                        e.bTypeStr = string.Format("{0}", 0);
                        break;
                    case "刀闸":
                        e.bTypeStr = string.Format("{0}", 1);
                        break;
                    case "智能控制器":
                        e.bTypeStr = string.Format("{0}", 0);
                        break;
                    default:
                        e.bTypeStr = string.Format("{0}", 0);
                        break;
                }
                var dqzt = GetDQZT(e.symbolObj);
                if(e.bTypeStr!=null)
                      e.bTypeStr += string.Format("_{0}", dqzt != "闭合" ? "0" : "1");
               
                string str = "低压开关符号";
                str += "-" + GetCD_SMZQ(e.symbolObj);
                str += "-" + dykgfl;
                if (str.IndexOf("-停运") > 0)
                {
                    str += "-" + GetDQZT(e.symbolObj);
                }
                if (str.IndexOf("-投运") > 0)
                {
                    str += "-" + GetDQZT(e.symbolObj);
                    str += "-" + GetCD_SFDD(e.symbolObj);
                }
                e.symbolColor = DBEntityFinder.Instance.GetPointStyle(e.symbolColor, str);
            }
        }

        [Execution(158)]
        public static void Gg_pd_dyzjjt_pt_sdogeom(object sender, InsertSymbolEventArgs e)
        {
            e = GetPointStyle(e, CABLEManager._color2);
            var str = "低压电缆中间接头符号-";
            var smzq = GetCD_SMZQ(e.symbolObj);
            if (!string.IsNullOrEmpty(smzq))
            {
                str = GetUserNameBySMZQorSFDD(e.symbolObj, str);
            }
            e.symbolColor = DBEntityFinder.Instance.GetPointStyle(e.symbolColor, str);
        }

        [Execution(159)]
        public static void Gg_pd_cbx_pt_sdogeom(object sender, InsertSymbolEventArgs e)
        {
            e = GetPointStyle(e, CABLEManager._color2);
          
            e.bTypeStr = string.Format("{0}", 0);
                
            var str = "低压集中抄表箱符号";
            var lx = GenerateHelper.GetPropertyValue(e.symbolObj, GenerateHelper.GetSelfTableNameByFno(e.g3e_fno), "LX");
            if (lx!=null)
            {
                switch (lx.ToString())
                {
                    case "集抄箱":
                        e.bTypeStr = string.Format("{0}", 0);
                        break;
                    case "门牌地址":
                        e.bTypeStr = string.Format("{0}", 1);
                        break;
                    case "复录仪":
                        e.bTypeStr = string.Format("{0}", 2);
                        break;
                    default:
                        e.bTypeStr = string.Format("{0}", 0);
                        break;
                }
                var sfdd = GetCD_SFDD(e.symbolObj);

                str = string.Format("{0}-{1}", str, lx);
                if (lx.ToString().Equals("集抄箱") && sfdd.Equals("不带电"))
                {
                    str = "低压集中抄表箱符号-停电";
                }
                else if (lx.ToString().Equals("复录仪") && sfdd.Equals("不带电"))
                    str = string.Format("{0}-停电", str);
            }
            e.symbolColor = DBEntityFinder.Instance.GetPointStyle(e.symbolColor, str);
        }

        [Execution(169)]
        public static void Gg_pd_dydlt_pt_sdogeom(object sender, InsertSymbolEventArgs e)
        {
            e = GetPointStyle(e, CABLEManager._color2);
            var sfsdl = GenerateHelper.GetPropertyValue(e.symbolObj, GenerateHelper.GetSelfTableNameByFno(e.g3e_fno), "CD_SFSDL");
            if (sfsdl != null)
            {
                switch (sfsdl.ToString())
                {
                    case "双电缆头":
                        e.bTypeStr = string.Format("{0}", 1);
                        break;
                    case "单电缆头":
                        e.bTypeStr = string.Format("{0}", 0);
                        break;
                    default:
                        e.bTypeStr = string.Format("{0}", 0);
                        break;
                }
                var str = "低压电缆头符号";
                var smzq = GetCD_SMZQ(e.symbolObj);
                var sfdd = GetCD_SFDD(e.symbolObj);
                if (!string.IsNullOrEmpty(smzq))
                {
                    str = GetUserNameBySMZQorSFDD(e.symbolObj, str);
                }
                if (!string.IsNullOrEmpty(sfdd))
                {
                    str = string.Format("{0}-{1}", str, sfdd == "双电缆头" ? "双头" : "单头");
                }
                e.symbolColor = DBEntityFinder.Instance.GetPointStyle(e.symbolColor, str);
            }
        }

        [Execution(205)]
        public static void Gg_gz_rj_pt_sdogeom(object sender, InsertSymbolEventArgs e)
        {
            e = GetPointStyle(e, CABLEManager._color3);
            string str = "人井符号";
            str += "-" + GetCD_SMZQ(e.symbolObj);
            e.symbolColor = DBEntityFinder.Instance.GetPointStyle(e.symbolColor, str);
        }

        [Execution(215)]
        public static void Gg_gz_dydg_pt_sdogeom(object sender, InsertSymbolEventArgs e)
        {
            GetPointStyle(e, CABLEManager._color2);
        }

        [Execution(218)]
        public static void Gg_gz_dyzc_pt_sdogeom(object sender, InsertSymbolEventArgs e)
        {
            e = GetPointStyle(e, CABLEManager._color2);

            string str = "低压支撑符号";

            //缺少类型 ：终端点、联络点、断连
            str += "-" + GetCD_SMZQ(e.symbolObj);
            e.symbolColor = DBEntityFinder.Instance.GetPointStyle(e.symbolColor, str);
        }

        [Execution(320)]
        public static void Gg_pd_jszz_pt_sdogeom(object sender, InsertSymbolEventArgs e)
        {
            e = GetPointStyle(e, CABLEManager._color3);
            string str = "监视装置符号";
            str += "-" + GetCD_SMZQ(e.symbolObj);
            e.symbolColor = DBEntityFinder.Instance.GetPointStyle(e.symbolColor, str);
        }

        [Execution(385)]
        public static void Gg_pd_fkjc_pt_sdogeom(object sender, InsertSymbolEventArgs e)
        {
            GetPointStyle(e, CABLEManager._color3);
        }

        #endregion

        #region 面
        [Execution(77)]
        public static void Gg_pd_dlfxx_ar_sdogeom(object sender, InsertSymbolEventArgs e)
        {
            GetLineDefaultStyle(e, CABLEManager._color2);
        }

        [Execution(81)]
        public static void Gg_pd_dyg_ar_sdogeom(object sender, InsertSymbolEventArgs e)
        {
            e = GetLineDefaultStyle(e, CABLEManager._color2);

            string str = "低压柜符号";
            str += "-" + GetCD_SMZQ(e.symbolObj);
            e.symbolColor = DBEntityFinder.Instance.GetAreaStyle(e.symbolColor, str);
        }

        [Execution(142)]
        public static void Gg_pd_gydf_ar_sdogeom(object sender, InsertSymbolEventArgs e)
        {
            e = GetLineDefaultStyle(e, CABLEManager._color3);

            string str = "公用电房符号";
            var zcgs = GenerateHelper.GetPropertyValue(e.symbolObj, GenerateHelper.GetSelfTableNameByFno(e.g3e_fno), "CD_ZCGS");
            if (zcgs != null)
            {
                if(zcgs.ToString()=="是"||zcgs.ToString()=="有")
                    str += "-带锁";
            }
            str +="-" + GetCD_SMZQ(e.symbolObj);
            e.symbolColor = DBEntityFinder.Instance.GetAreaStyle(e.symbolColor, str);
        }
    
        [Execution(149)]
        public static void Gg_pd_xssb_ar_sdogeom(object sender, InsertSymbolEventArgs e)
        {
            e = GetLineDefaultStyle(e, CABLEManager._color3);

            string str = "箱式设备符号";
            str += "-" + GetCD_SMZQ(e.symbolObj);
            e.symbolColor = DBEntityFinder.Instance.GetAreaStyle(e.symbolColor, str);
        }
 
        [Execution(163)]
        public static void Gg_pd_zydf_ar_sdogeom(object sender, InsertSymbolEventArgs e)
        {
            e = GetLineDefaultStyle(e, CABLEManager._color3);

            string str = "专用电房符号";
            //var yfznms = GenerateHelper.GetPropertyValue(e.symbolObj, "Gg_pd_gnwzmc_n", "GNWZ_YFZNMS");
            //if (yfznms != null)
            //{
            //    if(yfznms.ToString()=="是"||yfznms.ToString()=="有")
            //        str += "-带锁";
            //}
            str += "-" + GetCD_SMZQ(e.symbolObj);
            e.symbolColor = DBEntityFinder.Instance.GetAreaStyle(e.symbolColor, str);
        }
        [Execution(198)]
        public static void Gg_pd_kgg_ar_sdogeom(object sender, InsertSymbolEventArgs e)
        {
            GetLineDefaultStyle(e, CABLEManager._color3);
        }

        [Execution(199)]
        public static void Gg_gz_tj_ar_sdogeom(object sender, InsertSymbolEventArgs e)
        {
            GetLineDefaultStyle(e, CABLEManager._color3);
        }
        [Execution(302)]
        public static void Gg_pd_dypdf_ar_sdogeom(object sender, InsertSymbolEventArgs e)
        {
            GetLineDefaultStyle(e, CABLEManager._color2);
        }
        #endregion



        [Execution(71)]
        public static void Gg_pd_cqtg_pt_sdogeom(object sender, InsertSymbolEventArgs e)
        {
            e = GetPointStyle(e, CABLEManager._color3);

            var str = "穿墙套管符号";
            str += string.Format("-{0}", GetCD_SMZQ(e.symbolObj));
            e.symbolColor = DBEntityFinder.Instance.GetPointStyle(e.symbolColor, str);
        }

        [Execution(72)]
        public static void Gg_pd_jdgh_pt_sdogeom(object sender, InsertSymbolEventArgs e)
        {
            e = GetPointStyle(e, CABLEManager._color3);

            var str = "接地挂环符号";
            str += string.Format("-{0}", GetCD_SMZQ(e.symbolObj));
            e.symbolColor = DBEntityFinder.Instance.GetPointStyle(e.symbolColor, str);
        }

        [Execution(76)]
        public static void Gg_pd_gkjlg_pt_sdogeom(object sender, InsertSymbolEventArgs e)
        {
            e = GetPointStyle(e, CABLEManager._color3);

            var str = "关口计量柜符号";

            str += string.Format("-{0}", GetCD_SMZQ(e.symbolObj));

            e.symbolColor = DBEntityFinder.Instance.GetPointStyle(e.symbolColor, str);
        }

        [Execution(180)]
        public static void Gg_jc_ftu_pt_sdogeom(object sender, InsertSymbolEventArgs e)
        {
            e = GetPointStyle(e, CABLEManager._color3);
            var str = "FTU符号";
            str += string.Format("-{0}", GetCD_SMZQ(e.symbolObj));
            

            e.symbolColor = DBEntityFinder.Instance.GetPointStyle(e.symbolColor, str);
        }
        

        [Execution(183)]
        public static void Gg_jc_qyjcy_pt_sdogeom(object sender, InsertSymbolEventArgs e)
        {
           e = GetPointStyle(e, CABLEManager._color3);
            var str = "气压监测仪符号";
            var smzq = GetCD_SMZQ(e.symbolObj);
            if (!string.IsNullOrEmpty(smzq))
            {
                str += string.Format("-{0}", smzq);
            }
            e.symbolColor = DBEntityFinder.Instance.GetPointStyle(e.symbolColor, str);

        }

        [Execution(184)]
        public static void Gg_jc_yyjcy_pt_sdogeom(object sender, InsertSymbolEventArgs e)
        {
            e = GetPointStyle(e, CABLEManager._color3);
            var str = "油压监测仪符号";
            var smzq = GetCD_SMZQ(e.symbolObj);
            if (!string.IsNullOrEmpty(smzq))
            {
                str += string.Format("-{0}", smzq);
            }
            e.symbolColor = DBEntityFinder.Instance.GetPointStyle(e.symbolColor, str);
        }

        [Execution(185)]
        public static void Gg_jc_dyjcy_pt_sdogeom(object sender, InsertSymbolEventArgs e)
        {
            e = GetPointStyle(e, CABLEManager._color3);

            var str = "电压监测仪符号";
            var smzq = GetCD_SMZQ(e.symbolObj);
            if (!string.IsNullOrEmpty(smzq))
            {
                str += string.Format("-{0}", smzq);
            }
            e.symbolColor = DBEntityFinder.Instance.GetPointStyle(e.symbolColor, str);
        }


        [Execution(186)]
        public static void Gg_jc_ldkgx_pt_sdogeom(object sender, InsertSymbolEventArgs e)
        {
            e = GetPointStyle(e, CABLEManager._color3);
            var str = "路灯开关箱符号";
            var smzq = GetCD_SMZQ(e.symbolObj);
            if (!string.IsNullOrEmpty(smzq))
            {
                str += string.Format("-{0}", smzq);
            }
            e.symbolColor = DBEntityFinder.Instance.GetPointStyle(e.symbolColor, str);
        }


        [Execution(188)]
        public static void Gg_jc_fk_pt_sdogeom(object sender, InsertSymbolEventArgs e)
        {
            e = GetPointStyle(e, CABLEManager._color3);

            const string str = "负控符号";
            e.symbolColor = DBEntityFinder.Instance.GetPointStyle(e.symbolColor, str);
        }

        [Execution(86)]
        public static void Gg_jc_dcjg_pt_sdogeom(object sender, InsertSymbolEventArgs e)
        {
            e = GetPointStyle(e, CABLEManager._color3);

            var str = "电操机构符号";
            var smzq = GetCD_SMZQ(e.symbolObj);
            if (!string.IsNullOrEmpty(smzq))
            {
                str += string.Format("-{0}", smzq);
            }
            e.symbolColor = DBEntityFinder.Instance.GetPointStyle(e.symbolColor, str);

        }

        /// <summary>
        /// 线的默认样式
        /// </summary>
        /// <param name="e"></param>
        /// <param name="type">true导线、false电缆</param>
        /// <param name="width"> </param>
        /// <param name="colorIndex"> </param>
        /// <returns></returns>
        private static InsertSymbolEventArgs GetLineDefaultStyle(InsertSymbolEventArgs e, bool type, double width, int colorIndex)
        {
            e.lineWidth = width;
            e.symbolColor = Autodesk.AutoCAD.Colors.Color.FromColor(Color.FromArgb(colorIndex));
            e.lineTypeStr = type ? "Continuous" : "DashLines";
            e.isSymbolType = egtype.linestring;
            return e;
        }
        /// <summary>
        /// 面的默认样式
        /// </summary>
        /// <param name="e"></param>
        /// <param name="colorIndex"></param>
        /// <returns></returns>
        private static InsertSymbolEventArgs GetLineDefaultStyle(InsertSymbolEventArgs e, int colorIndex)
        {
            e.lineWidth = 0.02;
            e.lineTypeStr = "Continuous";
            e.isSymbolType = egtype.polygon;
            e.symbolColor = Autodesk.AutoCAD.Colors.Color.FromColor(Color.FromArgb(colorIndex));
            return e;
        }

        private static InsertSymbolEventArgs GetPointStyle(InsertSymbolEventArgs e,int colorIndex)
        {
            e.symbolColor = Autodesk.AutoCAD.Colors.Color.FromColor(Color.FromArgb(colorIndex));
            e.isSymbolType = egtype.multipoint;
            return e;
        }

        //获取是否带电
        private static string GetCD_SFDD(XProps obj)
        {
            var sfdd = string.Empty;
            var cdsfdd = GenerateHelper.GetPropertyValue(obj,"Connectivity_n", "CD_SFDD");
            if (cdsfdd != null)
                sfdd = cdsfdd.ToString();
            return sfdd;
        }
        /// <summary>
        /// 获取生命周期
        /// </summary>
        /// <param name="obj"></param>
        /// <returns></returns>
        private static string GetCD_SMZQ(XProps obj)
        {
            var smzq = string.Empty;
            var cdsmzq = GenerateHelper.GetPropertyValue(obj,"Common_n", "CD_SMZQ");
            if (cdsmzq != null)
                smzq = cdsmzq.ToString();
            return smzq;
        }

        private static string GetCXD(XProps obj,int fno)
        {
            var cxd = string.Empty;
            var cdcxd = GenerateHelper.GetPropertyValue(obj, GenerateHelper.GetSelfTableNameByFno(fno), "CXD");
            if (cdcxd != null)
                cxd = cdcxd.ToString();
            return cxd;
        }

        private static string GetDQZT(XProps obj)
        {
            var dqzt = string.Empty;
            var cddqzt = GenerateHelper.GetPropertyValue(obj, "Connectivity_n", "CD_DQZT");
            if (cddqzt != null)
                dqzt = cddqzt.ToString();
            return dqzt;
        }

        private static string GetDYDJ(XProps obj)
        {
            var dydj = string.Empty;
            var cddydj = GenerateHelper.GetPropertyValue(obj, "Common_n", "CD_DYDJ");
            if (cddydj != null)
                dydj = cddydj.ToString();
            return dydj;
        }
        /// <summary>
        /// 根据生命周期和是否带电获取样式
        /// </summary>
        /// <param name="obj"> </param>
        /// <param name="userName"> </param>
        /// <returns></returns>
        private static string GetUserNameBySMZQorSFDD(XProps obj, string userName)
        {
            var smzq = GetCD_SMZQ(obj);
            var sfdd = GetCD_SFDD(obj);
            if (!string.IsNullOrEmpty(smzq))
            {
                //停运和不带电情况
                if (smzq.Equals("停运") || sfdd.Equals("不带电"))
                {
                    userName += "-停运";
                }else if(smzq.Equals("投运") && sfdd.Equals("带电"))
                {
                    userName += "-投运";
                }
                else
                    userName += "-" + smzq;
            }
            return userName;
        }
    }
}
