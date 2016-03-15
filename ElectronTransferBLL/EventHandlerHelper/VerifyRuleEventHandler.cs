using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Runtime.Serialization;
using System.Text;
using ElectronTransferDal;
using ElectronTransferDal.Common;
using ElectronTransferDal.HelperEventArgs;
using ElectronTransferFramework;
using ElectronTransferDal.Factory;
using ElectronTransferDal.Cad;
using ElectronTransferModel.Base;
using ElectronTransferModel.Config;
using ElectronTransferModel.V9_4;
using ElectronTransferDal.AutoGeneration;

namespace ElectronTransferBll.EventHandlerHelper
{
    [ExecutionContainer(typeof (VerifyRuleExecutionFactory))]
    public class VerifyRuleEventHandler
    {
        #region 线

        [Execution(140)]
        public static void Gg_pd_dl_ln_sdogeom(object sender, VerifyRuleEventArgs e)
        {
            try
            {
                CheckTableIsNull(e);
                var chooseAttr = GenerateHelper.FilterField(e.DevTables.DevObj, e.DevTables.Fno);
                StringBuilder sb = new StringBuilder();
                if (!IsFilterSszx(ref chooseAttr, e.DevTables.Fno))
                {
                    var xlfl = GetXlfl(e.DevTables);
                    if (string.IsNullOrEmpty(xlfl.BzDeclare))
                    {
                        sb.AppendLine(" 线路分类不能为空 ");
                    }
                    else if (xlfl.BzDeclare.Equals("连接导线"))
                    {
                        sb.AppendLine(" 电缆的线路分类不能为连接导线 ");
                    }
                    else if (!xlfl.BzDeclare.Equals("支线"))
                    {
                        if (e.DevTables.ComObj != null)
                        {
                            var sszx = GetBZValue(e.DevTables);
                            if (!string.IsNullOrEmpty(sszx.BzDeclare))
                            {
                                sb.AppendLine(" 不为支线时，所属支线必须为空 ");
                            }
                            AddChooseAttr(ref chooseAttr, sszx.Field);
                        }

                    }
                    else if (xlfl.BzDeclare.Equals("支线"))
                    {
                        if (e.DevTables.ComObj != null)
                        {
                            var sszx = GetBZValue(e.DevTables);
                            if (string.IsNullOrEmpty(sszx.BzDeclare))
                            {
                                sb.AppendLine(" 为支线时，所属支线必须不能为空 ");
                            }
                            AddChooseAttr(ref chooseAttr, sszx.Field);

                        }

                    }
                    AddChooseAttr(ref chooseAttr, xlfl.Field);
                    var otherAttr = GenerateHelper.FilterVerify(e.DevTables, chooseAttr, e.DevTables.Fno);
                    if (otherAttr != null && otherAttr.ToString().Trim().Length > 0)
                        sb.AppendLine(otherAttr.ToString());
                    e.ErrorMsg = sb;
                    e.IsUseFactoryVerify = true;
                }
            }
            catch (Exception ex)
            {
                LogManager.Instance.Error(ex.Message);
            }

        }

        [Execution(141)]
        public static void Gg_pd_dx_ln_sdogeom(object sender, VerifyRuleEventArgs e)
        {
            CheckTableIsNull(e);
            var chooseAttr = GenerateHelper.FilterField(e.DevTables.DevObj, e.DevTables.Fno);
            StringBuilder sb = new StringBuilder();
            if (!IsFilterSszx(ref chooseAttr, e.DevTables.Fno))
            {
                var xlfl = GetXlfl(e.DevTables);
                if (string.IsNullOrEmpty(xlfl.BzDeclare))
                {
                    sb.AppendLine(" 线路分类不能为空 ");
                }
                else if (!xlfl.BzDeclare.Equals("支线"))
                {
                    if (e.DevTables.ComObj != null)
                    {
                        var sszx = GetBZValue(e.DevTables);
                        if (!string.IsNullOrEmpty(sszx.BzDeclare))
                        {
                            sb.AppendLine(" 不为支线时，所属支线必须为空 ");
                        }
                        AddChooseAttr(ref chooseAttr, sszx.Field);
                    }

                }
                else if (xlfl.BzDeclare.Equals("支线"))
                {
                    if (e.DevTables.ComObj != null)
                    {
                        var sszx = GetBZValue(e.DevTables);
                        if (string.IsNullOrEmpty(sszx.BzDeclare))
                        {
                            sb.AppendLine(" 为支线时，所属支线必须不能为空 ");
                        }
                        AddChooseAttr(ref chooseAttr, sszx.Field);
                    }

                }
                AddChooseAttr(ref chooseAttr, xlfl.Field);
                var otherAttr = GenerateHelper.FilterVerify(e.DevTables, chooseAttr, e.DevTables.Fno);
                if (otherAttr != null && otherAttr.ToString().Trim().Length > 0)
                    sb.AppendLine(otherAttr.ToString());
                e.ErrorMsg = sb;
                e.IsUseFactoryVerify = true;
            }
           
        }

        [Execution(143)]
        public static void Gg_pd_zfmx_ln_sdogeom(object sender, VerifyRuleEventArgs e)
        {

        }

        [Execution(144)]
        public static void Gg_pd_zfyx_ln_sdogeom(object sender, VerifyRuleEventArgs e)
        {
        }

        [Execution(79)]
        public static void Gg_pd_dymx_ln_sdogeom(object sender, VerifyRuleEventArgs e)
        {

        }

        [Execution(156)]
        public static void Gg_pd_dydx_ln_sdogeom(object sender, VerifyRuleEventArgs e)
        {

        }

        [Execution(157)]
        public static void Gg_pd_dydl_ln_sdogeom(object sender, VerifyRuleEventArgs e)
        {


        }

        [Execution(80)]
        public static void Gg_pd_bqx_ln_sdogeom(object sender, VerifyRuleEventArgs e)
        {

        }

        [Execution(75)]
        public static void Gg_pd_bdcx_pt_sdogeom(object sender, VerifyRuleEventArgs e)
        {
            //对截面积进行校验
            //CheckTableIsNull(e);
            //StringBuilder sb = new StringBuilder();
            //if (e.DevTables.SelfObj != null)
            //{
            //    var jmj = e.DevTables.SelfObj.GetAttribute("JMJ");
            //    if (string.IsNullOrEmpty(jmj))
            //    {
            //        sb.AppendLine(" 截面积不能为空 ");
            //    }
            //    else
            //    {

            //    }
            //}

            //var chooseAttr = GenerateHelper.FilterField(e.DevTables.DevObj, 140);
            //chooseAttr.Add("JMJ");
            //var otherAttr = GenerateHelper.FilterVerify(e.DevTables, chooseAttr, 140);
            //if (otherAttr != null && otherAttr.ToString().Trim().Length > 0)
            //    sb.AppendLine(otherAttr.ToString());
            //e.ErrorMsg = sb;
            //e.IsUseFactoryVerify = true;
        }

        //[Execution(165)]
        //public static void Gg_jx_yx_ln_sdogeom(object sender, VerifyRuleEventArgs e)
        //{

        //}

        #endregion

        #region 面

        [Execution(77)]
        public static void Gg_pd_dlfxx_ar_sdogeom(object sender, VerifyRuleEventArgs e)
        {

        }

        [Execution(81)]
        public static void Gg_pd_dyg_ar_sdogeom(object sender, VerifyRuleEventArgs e)
        {

        }

        [Execution(142)]
        public static void Gg_pd_gydf_ar_sdogeom(object sender, VerifyRuleEventArgs e)
        {

        }

        [Execution(149)]
        public static void Gg_pd_xssb_ar_sdogeom(object sender, VerifyRuleEventArgs e)
        {

        }

        [Execution(163)]
        public static void Gg_pd_zydf_ar_sdogeom(object sender, VerifyRuleEventArgs e)
        {

        }

        [Execution(302)]
        public static void Gg_pd_dypdf_ar_sdogeom(object sender, VerifyRuleEventArgs e)
        {

        }

        [Execution(198)]
        public static void Gg_pd_kgg_ar_sdogeom(object sender, VerifyRuleEventArgs e)
        {
            CheckTableIsNull(e);
            StringBuilder sb = new StringBuilder();

            #region 开关柜特殊校验部分

            if (e.DevTables.ComObj != null)
            {
                if (e.DevTables.ComObj.OWNER1_ID == null)
                    sb.AppendLine(" 该开关柜未和电房建立关联关系 ");
                else
                {
                    var df = DBManager.Instance.GetEntity<Common_n>(o => o.G3E_ID == e.DevTables.ComObj.OWNER1_ID);
                    if (df == null)
                    {
                        sb.AppendLine(" 开关柜所从属的电房不存在 ");
                    }
                    //else
                    //{
                    //if (!df.CD_SSXL.Equals(e.DevTables.ComObj.CD_SSXL))
                    //    sb.AppendLine(" 开关柜和它所从属的电房不在统一馈线下 ");
                    //}
                }
            }
            else
                sb.AppendLine(" 开关柜公共属性表创建失败 ");

            #endregion


            var otherAttr = GenerateHelper.VerifySingleDeviceAttribute(e.DevTables, e.DevTables.Fno);
            if (otherAttr != null && otherAttr.ToString().Trim().Length > 0)
                sb.AppendLine(otherAttr.ToString());
            e.ErrorMsg = sb;
            e.IsUseFactoryVerify = true;

        }

        [Execution(199)]
        public static void Gg_gz_tj_ar_sdogeom(object sender, VerifyRuleEventArgs e)
        {
            CheckTableIsNull(e);
            StringBuilder sb = new StringBuilder();

            #region 台架特殊处理部分

            var gnwz =
                DBManager.Instance.GetEntity<Gg_pd_gnwzmc_n>(
                    o =>
                        o.G3E_FNO == 148 && !string.IsNullOrEmpty(o.GNWZ_SSTJ) &&
                        o.GNWZ_SSTJ.Equals(e.DevTables.Fid.ToString()));
            if (gnwz == null)
                sb.AppendLine(" 该台架附属的变压器已经不存在,可能是删除数据错误 ");

            #endregion

            var otherAttr = GenerateHelper.VerifySingleDeviceAttribute(e.DevTables, e.DevTables.Fno);
            if (otherAttr != null && otherAttr.ToString().Trim().Length > 0)
                sb.AppendLine(otherAttr.ToString());
            e.ErrorMsg = sb;
            e.IsUseFactoryVerify = true;

        }

        #endregion



        [Execution(40)]
        public static void Gg_pd_dlhgq_pt_sdogeom(object sender, VerifyRuleEventArgs e)
        {

        }

        [Execution(82)]
        public static void Gg_jx_gyb_pt_sdogeom(object sender, VerifyRuleEventArgs e)
        {

        }

        [Execution(84)]
        public static void Gg_pd_cjlg_pt_sdogeom(object sender, VerifyRuleEventArgs e)
        {

        }

        [Execution(85)]
        public static void Gg_pd_ptg_pt_sdogeom(object sender, VerifyRuleEventArgs e)
        {

        }


        [Execution(90)]
        public static void Gg_jc_dtu_pt_sdogeom(object sender, VerifyRuleEventArgs e)
        {

        }

        [Execution(145)]
        public static void Gg_pd_zfllkg_pt_sdogeom(object sender, VerifyRuleEventArgs e)
        {
        }

        [Execution(146)]
        public static void Gg_pd_zfhwg_pt_sdogeom(object sender, VerifyRuleEventArgs e)
        {
            CheckTableIsNull(e);
            //当时美式箱变的时候不需要所属开关柜
            //开关柜的ownerid==电房的g3eid
            //取出开关的从属电房
            StringBuilder sb = new StringBuilder();
            var chooseAttr = GenerateHelper.FilterField(e.DevTables.DevObj, e.DevTables.Fno);

            var azwz = DeviceAttributeConfig.Instance.Attributes.SingleOrDefault(o => o.Fno == e.DevTables.Fno);
            if (azwz != null)
            {
                if (azwz.InstallLocationOption != null)
                {
                    var azwzStr = GetFieldValue(azwz.InstallLocationOption.TableName,
                        azwz.InstallLocationOption.FieldName,
                        e.DevTables.Fid);

                    #region

                    if (string.IsNullOrEmpty(azwzStr))
                    {
                        sb.AppendLine(" 安装位置不能为空 ");
                    }
                    else if (azwzStr.Equals("户内"))
                    {
                        var azwzOption =
                            azwz.InstallLocationOption.AzwzCollection.SingleOrDefault(o => o.AzwzState.Equals("户内"));
                        if (azwzOption != null)
                        {
                            var showFields = GenerateHelper.GetFieldsFromAzwzCollection(azwzOption.ShowFields);
                            if (showFields.Any())
                            {
                                var fields =
                                    showFields.SingleOrDefault(o => o.FromOtherFidOfFiledName.Equals("GNWZ_SSKGG"));
                                if (fields != null && !chooseAttr.Contains(fields.FromOtherFidOfFiledName))
                                {
                                    #region  下面都是校验开关柜

                                    var kgOwnerShip = DBManager.Instance.GetEntity<Common_n>(e.DevTables.Fid);
                                    if (kgOwnerShip.OWNER1_ID != null && kgOwnerShip.OWNER1_ID != 0)
                                    {
                                        var dfcommon =
                                            DBManager.Instance.GetEntity<Common_n>(
                                                o => o.G3E_ID == kgOwnerShip.OWNER1_ID);
                                        if (dfcommon == null)
                                        {
                                            sb.AppendLine(" 开关从属建立失败 ");
                                        }
                                        else
                                        {
                                            #region 有从属了，再根据从属判断

                                            if (dfcommon.G3E_FNO == 149) //表示是箱式设备
                                            {
                                                //无需判断是否需要开关柜
                                            }
                                            else
                                            {
                                                #region 到此表示开关柜不能为空

                                                var sskgg = e.DevTables.GnwzObj.GNWZ_SSKGG;

                                                //再次查看当前开关柜是否属于开关从属的电房
                                                long fid = 0;
                                                if (long.TryParse(sskgg, out fid))
                                                {
                                                    #region 查看是否是随便填写的开关柜，原则是当前开关所从属的电房应该和开关柜从属的电房是一致的

                                                    var kggOwnerShip = DBManager.Instance.GetEntity<Common_n>(fid);
                                                    if (kggOwnerShip != null && kggOwnerShip.OWNER1_ID != null &&
                                                        kggOwnerShip.OWNER1_ID != 0)
                                                    {
                                                        var dfToKgg =
                                                            DBManager.Instance.GetEntity<Common_n>(
                                                                o => o.G3E_ID == kggOwnerShip.OWNER1_ID);
                                                        if ((dfToKgg == null) || (dfcommon.G3E_FID != dfToKgg.G3E_FID))
                                                        {
                                                            sb.AppendLine(" 当前开关所属开关柜从属的电房与当前开关从属的电房不是同一个电房 ");

                                                        }
                                                    }
                                                    else
                                                    {
                                                        sb.AppendLine(" 当前开关所属开关柜未能从属于开关从属的电房 ");
                                                    }

                                                    #endregion
                                                }
                                                else
                                                {
                                                    #region 原因没有填写

                                                    if (string.IsNullOrEmpty(sskgg)) //当该字段为空时看看有没有开关柜从属与和开关同样的电房
                                                    {
                                                        #region 存在房内设备不需要填写开关柜字段值，查看电房内是否有开关柜，有开关柜则必须填写

                                                        var kggItems =
                                                            DBManager.Instance.GetEntities<Common_n>(
                                                                o => o.G3E_FNO == 198);
                                                        if (kggItems != null && kggItems.Any())
                                                        {
                                                            foreach (var item in kggItems)
                                                            {
                                                                var df =
                                                                    DBManager.Instance.GetEntity<Common_n>(
                                                                        o => o.G3E_ID == item.OWNER1_ID);
                                                                if (df != null)
                                                                {
                                                                    if (df.G3E_FID == dfcommon.G3E_FID) //存在开关柜
                                                                    {
                                                                        sb.AppendLine(" 所属开关柜字段不能为空 ");
                                                                        break;
                                                                    }
                                                                    else
                                                                    {
                                                                        //不存在开关柜那么表示该房内开关无需填写开关柜
                                                                    }
                                                                }
                                                            }
                                                        }

                                                        #endregion
                                                    }

                                                    #endregion
                                                }

                                                #endregion

                                                //到此表示开关的所属开关柜校验完整
                                                AddChooseAttr(ref chooseAttr, fields.FromOtherFidOfFiledName);


                                            }

                                            #endregion
                                        }

                                    }
                                    else
                                    {
                                        //暂时没建立从属关系
                                        sb.AppendLine(" 缺失从属关系无法判断是否要填写开关柜 ");
                                    }

                                    #endregion
                                }
                            }



                        }
                    }
                    else if (azwzStr.Equals("柱上"))
                    {
                        #region  校验所属支线

                        if (!IsFilterSszx(ref chooseAttr, e.DevTables.Fno))
                        {
                            if (e.DevTables.GnwzObj != null)
                            {
                                var xlfl = GetXlfl(e.DevTables);
                                if (string.IsNullOrEmpty(xlfl.BzDeclare))
                                {
                                    sb.AppendLine(" 线路分类不能为空 ");
                                }
                                else if (!xlfl.BzDeclare.Equals("支线"))
                                {
                                    if (e.DevTables.ComObj != null)
                                    {
                                        var sszx = GetBZValue(e.DevTables);
                                        if (!string.IsNullOrEmpty(sszx.BzDeclare))
                                        {
                                            sb.AppendLine(" 不为支线时，所属支线必须为空 ");
                                        }
                                        AddChooseAttr(ref chooseAttr, sszx.Field);
                                    }
                                }
                                else if (xlfl.BzDeclare.Equals("支线"))
                                {
                                    if (e.DevTables.ComObj != null)
                                    {
                                        var sszx = GetBZValue(e.DevTables);
                                        if (string.IsNullOrEmpty(sszx.BzDeclare))
                                        {
                                            sb.AppendLine(" 为支线时，所属支线不能为空 ");
                                        }
                                        AddChooseAttr(ref chooseAttr, sszx.Field);
                                    }
                                }
                                AddChooseAttr(ref chooseAttr, xlfl.Field);
                            }

                        }
                      
                        #endregion
                    }
                    AddChooseAttr(ref chooseAttr, azwz.InstallLocationOption.FieldName);

                    var otherAttr = GenerateHelper.FilterVerify(e.DevTables, chooseAttr, e.DevTables.Fno);
                    if (otherAttr != null && otherAttr.ToString().Trim().Length > 0)
                        sb.AppendLine(otherAttr.ToString());
                    e.ErrorMsg = sb;
                    e.IsUseFactoryVerify = true;

                    #endregion
                }
            }
        }

        [Execution(147)]
        public static void Gg_pd_zfdlt_pt_sdogeom(object sender, VerifyRuleEventArgs e)
        {



        }

        [Execution(148)]
        public static void Gg_pd_zfbyq_pt_sdogeom(object sender, VerifyRuleEventArgs e)
        {
        }



        [Execution(150)]
        public static void Gg_pd_dlzjjt_pt_sdogeom(object sender, VerifyRuleEventArgs e)
        {

        }

        [Execution(151)]
        public static void Gg_pd_dlzdt_pt_sdogeom(object sender, VerifyRuleEventArgs e)
        {

        }

        //[Execution(152)]
        //public static void Gg_pd_dlq_pt_sdogeom(object sender, VerifyRuleEventArgs e)
        //{

        //}

        [Execution(154)]
        public static void Gg_pd_zsbyq_pt_sdogeom(object sender, VerifyRuleEventArgs e)
        {
        }

        [Execution(171)]
        public static void Gg_pd_zdfdj_pt_sdogeom(object sender, VerifyRuleEventArgs e)
        {

        }

        [Execution(172)]
        public static void Gg_pd_ddj_pt_sdogeom(object sender, VerifyRuleEventArgs e)
        {

        }

        [Execution(173)]
        public static void Gg_pd_zfjddz_pt_sdogeom(object sender, VerifyRuleEventArgs e)
        {

        }

        [Execution(174)]
        public static void Gg_pd_wgbc_pt_sdogeom(object sender, VerifyRuleEventArgs e)
        {

        }

        [Execution(176)]
        public static void Gg_pd_jdk_pt_sdogeom(object sender, VerifyRuleEventArgs e)
        {

        }

        [Execution(177)]
        public static void Gg_pd_blq_pt_sdogeom(object sender, VerifyRuleEventArgs e)
        {
            CheckTableIsNull(e);
            StringBuilder sb = new StringBuilder();
            var chooseAttr = GenerateHelper.FilterField(e.DevTables.DevObj, e.DevTables.Fno);
            var blqazwz = DeviceAttributeConfig.Instance.Attributes.SingleOrDefault(o => o.Fno == e.DevTables.Fno);
            if (blqazwz != null)
            {
                if (blqazwz.InstallLocationOption != null)
                {
                    var azwz = GetFieldValue(blqazwz.InstallLocationOption.TableName,
                        blqazwz.InstallLocationOption.FieldName,
                        e.DevTables.Fid);
                    if (string.IsNullOrEmpty(azwz))
                    {
                        sb.AppendLine(" 安装位置不能为空 ");
                    }
                    else if (azwz.Equals("柱上"))
                    {
                        if (!IsFilterSszx(ref chooseAttr, e.DevTables.Fno))
                        {
                            if (e.DevTables.GnwzObj != null)
                            {
                                var xlfl = GetXlfl(e.DevTables);
                                if (string.IsNullOrEmpty(xlfl.BzDeclare))
                                {
                                    sb.AppendLine(" 线路分类不能为空 ");
                                }
                                else if (!xlfl.BzDeclare.Equals("支线"))
                                {
                                    if (e.DevTables.ComObj != null)
                                    {
                                        var sszx = GetBZValue(e.DevTables);
                                        if (!string.IsNullOrEmpty(sszx.BzDeclare))
                                        {
                                            sb.AppendLine(" 不为支线时，所属支线必须为空 ");
                                        }
                                        AddChooseAttr(ref chooseAttr, sszx.Field);
                                    }
                                }
                                else if (xlfl.BzDeclare.Equals("支线"))
                                {
                                    if (e.DevTables.ComObj != null)
                                    {
                                        var sszx = GetBZValue(e.DevTables);
                                        if (string.IsNullOrEmpty(sszx.BzDeclare))
                                        {
                                            sb.AppendLine(" 为支线时，所属支线不能为空 ");
                                        }
                                        AddChooseAttr(ref chooseAttr, sszx.Field);
                                    }
                                }
                                AddChooseAttr(ref chooseAttr, xlfl.Field);
                            }
                        }
                       
                    }
                    AddChooseAttr(ref chooseAttr, blqazwz.InstallLocationOption.FieldName);
                    var otherAttr = GenerateHelper.FilterVerify(e.DevTables, chooseAttr, e.DevTables.Fno);
                    if (otherAttr != null && otherAttr.ToString().Trim().Length > 0)
                        sb.AppendLine(otherAttr.ToString());
                    e.ErrorMsg = sb;
                    e.IsUseFactoryVerify = true;
                }
            }
        }

        [Execution(180)]
        public static void Gg_jc_ftu_pt_sdogeom(object sender, VerifyRuleEventArgs e)
        {

        }

        [Execution(181)]
        public static void Gg_jc_pwy_pt_sdogeom(object sender, VerifyRuleEventArgs e)
        {

        }

        [Execution(182)]
        public static void Gg_jc_gzzsq_pt_sdogeom(object sender, VerifyRuleEventArgs e)
        {

        }

        [Execution(201)]
        public static void Gg_gz_dg_pt_sdogeom(object sender, VerifyRuleEventArgs e)
        {
            CheckTableIsNull(e);
            StringBuilder sb = new StringBuilder();
            var chooseAttr = GenerateHelper.FilterField(e.DevTables.DevObj, e.DevTables.Fno);

            if (!IsFilterSszx(ref chooseAttr, e.DevTables.Fno))
            {
                if (e.DevTables.GnwzObj != null)
                {
                    var xlfl = GetXlfl(e.DevTables);
                    if (string.IsNullOrEmpty(xlfl.BzDeclare))
                    {
                        sb.AppendLine(" 线路分类不能为空 ");
                    }
                    else if (!xlfl.BzDeclare.Equals("支线"))
                    {
                        if (e.DevTables.ComObj != null)
                        {
                            var sszx = GetBZValue(e.DevTables);
                            if (!string.IsNullOrEmpty(sszx.BzDeclare))
                            {
                                sb.AppendLine(" 不为支线时，所属支线必须为空 ");
                            }
                            AddChooseAttr(ref chooseAttr, sszx.Field);
                        }
                    }
                    else if (xlfl.BzDeclare.Equals("支线"))
                    {
                        if (e.DevTables.ComObj != null)
                        {
                            var sszx = GetBZValue(e.DevTables);
                            if (string.IsNullOrEmpty(sszx.BzDeclare))
                            {
                                sb.AppendLine(" 为支线时，所属支线不能为空 ");
                            }
                            AddChooseAttr(ref chooseAttr, sszx.Field);
                        }
                    }
                    AddChooseAttr(ref chooseAttr, xlfl.Field);
                }
                else
                {
                    sb.AppendLine(" 缺失功能位置表，无法判断相关属性 ");
                }
            }
            var otherAttr = GenerateHelper.FilterVerify(e.DevTables, chooseAttr, e.DevTables.Fno);
            if (otherAttr != null && otherAttr.ToString().Trim().Length > 0)
                sb.AppendLine(otherAttr.ToString());
            e.ErrorMsg = sb;
            e.IsUseFactoryVerify = true;
        }

        [Execution(307)]
        public static void Gg_pd_dyhgq_pt_sdogeom(object sender, VerifyRuleEventArgs e)
        {



        }

        [Execution(309)]
        public static void Gg_pd_gydj_pt_sdogeom(object sender, VerifyRuleEventArgs e)
        {

        }

        /******************************************************/

        [Execution(73)]
        public static void Gg_pd_dldy_pt_sdogeom(object sender, VerifyRuleEventArgs e)
        {

        }

        [Execution(74)]
        public static void Gg_pd_dyzd_pt_sdogeom(object sender, VerifyRuleEventArgs e)
        {

        }

        [Execution(155)]
        public static void Gg_pd_dykg_pt_sdogeom(object sender, VerifyRuleEventArgs e)
        {



        }

        [Execution(158)]
        public static void Gg_pd_dyzjjt_pt_sdogeom(object sender, VerifyRuleEventArgs e)
        {
        }

        [Execution(159)]
        public static void Gg_pd_cbx_pt_sdogeom(object sender, VerifyRuleEventArgs e)
        {

        }


        [Execution(169)]
        public static void Gg_pd_dydlt_pt_sdogeom(object sender, VerifyRuleEventArgs e)
        {

        }

        [Execution(175)]
        public static void Gg_pd_dll_pt_sdogeom(object sender, VerifyRuleEventArgs e)
        {

        }

        [Execution(215)]
        public static void Gg_gz_dydg_pt_sdogeom(object sender, VerifyRuleEventArgs e)
        {

        }

        [Execution(218)]
        public static void Gg_gz_dyzc_pt_sdogeom(object sender, VerifyRuleEventArgs e)
        {

        }

        [Execution(320)]
        public static void Gg_pd_jszz_pt_sdogeom(object sender, VerifyRuleEventArgs e)
        {
        }

        [Execution(71)]
        public static void Gg_pd_cqtg_pt_sdogeom(object sender, VerifyRuleEventArgs e)
        {

        }

        [Execution(72)]
        public static void Gg_pd_jdgh_pt_sdogeom(object sender, VerifyRuleEventArgs e)
        {

        }

        [Execution(76)]
        public static void Gg_pd_gkjlg_pt_sdogeom(object sender, VerifyRuleEventArgs e)
        {

        }

        [Execution(205)]
        public static void Gg_gz_rj_pt_sdogeom(object sender, VerifyRuleEventArgs e)
        {

        }

        [Execution(183)]
        public static void Gg_jc_qyjcy_pt_sdogeom(object sender, VerifyRuleEventArgs e)
        {

        }

        [Execution(184)]
        public static void Gg_jc_yyjcy_pt_sdogeom(object sender, VerifyRuleEventArgs e)
        {


        }

        [Execution(185)]
        public static void Gg_jc_dyjcy_pt_sdogeom(object sender, VerifyRuleEventArgs e)
        {

        }

        [Execution(186)]
        public static void Gg_jc_ldkgx_pt_sdogeom(object sender, VerifyRuleEventArgs e)
        {

        }

        [Execution(188)]
        public static void Gg_jc_fk_pt_sdogeom(object sender, VerifyRuleEventArgs e)
        {

        }

        [Execution(86)]
        public static void Gg_jc_dcjg_pt_sdogeom(object sender, VerifyRuleEventArgs e)
        {

        }

        public static void CheckTableIsNull(VerifyRuleEventArgs e)
        {
            if (e.DevTables == null)
            {
                e.ErrorMsg.AppendLine(" 表缺失 ");
                e.IsUseFactoryVerify = true;
                return;
            }
        }

        public static void AddChooseAttr(ref List<string> attrList, string attr)
        {
            if (!attrList.Contains(attr))
                attrList.Add(attr);
        }

        public static FieldTableNameAndBZMerger GetXlfl(RequiredDevTables rdt)
        {
            FieldTableNameAndBZMerger xlfl = new FieldTableNameAndBZMerger();
            try
            {
                var simpleEntry = DeviceAttributeConfig.Instance.Attributes.SingleOrDefault(o => o.Fno == rdt.Fno);
                if (simpleEntry != null)
                {
                    if (simpleEntry.GnwzXlfl != null)
                    {
                        try
                        {
                            var type = TypeCache.Instance.GetTypeFromCache(typeof (DBEntity),
                                simpleEntry.GnwzXlfl.TableName);
                            if (type != null)
                            {
                                var ent = DBManager.Instance.GetEntity(type, rdt.Fid);
                                xlfl.BzDeclare = ent.GetAttribute(simpleEntry.GnwzXlfl.FieldName);
                                xlfl.Field = simpleEntry.GnwzXlfl.FieldName;
                            }
                        }
                        catch (Exception ex)
                        {
                            LogManager.Instance.Error(ex.Message);
                        }

                    }
                }
            }
            catch (Exception ex)
            {
                LogManager.Instance.Error(ex.Message);
            }
            return xlfl;
        }

        public static FieldTableNameAndBZMerger GetBZValue(RequiredDevTables rdt)
        {
            FieldTableNameAndBZMerger bz = new FieldTableNameAndBZMerger();
            try
            {
                if (rdt != null)
                {
                    if (rdt.ComObj != null)
                    {
                        var bz1 = rdt.DevObj.SingleOrDefault(o => o.Name.Equals("BZ1") && o.SaveValueByFid);
                            //查看下到底所属支线是那个字段
                        if (bz1 != null)
                        {
                            bz.Field = bz1.Name;
                            bz.BzDeclare = rdt.ComObj.BZ1;
                        }
                        else
                        {
                            var bz2 = rdt.DevObj.SingleOrDefault(o => o.Name.Equals("BZ2") && o.SaveValueByFid);
                            if (bz2 != null)
                            {
                                bz.Field = bz2.Name;
                                bz.BzDeclare = rdt.ComObj.BZ2;
                            }
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                LogManager.Instance.Error(ex.Message);
            }
            return bz;
        }

        public static string GetFieldValue(string tableName, string fieldName, long fid)
        {
            string obj = string.Empty;
            try
            {
                var type = TypeCache.Instance.GetTypeFromCache(typeof (DBEntity),
                    tableName);
                if (type != null)
                {
                    var ent = DBManager.Instance.GetEntity(type, fid);
                    obj = ent.GetAttribute(fieldName);
                }
            }
            catch (Exception ex)
            {
                LogManager.Instance.Error(ex.Message);
            }
            return obj;
        }

        public static bool IsFilterSszx(ref List<string> chooseAttr,int fno)
        {
            bool flag = false;
            try
            {
                var simpleEntry = DeviceAttributeConfig.Instance.Attributes.SingleOrDefault(o => o.Fno == fno);
                if (simpleEntry != null)
                {
                    if (simpleEntry.SSzxOption != null)
                    {
                        if (chooseAttr.Contains(simpleEntry.SSzxOption.FieldName))
                        {
                            flag = true;
                        }
                    }
                    else
                    {
                        flag = true;
                    }
                }
                else
                {
                    flag = true;
                }
            }
            catch (Exception ex)
            {
                LogManager.Instance.Error(ex.Message);
            }
            return flag;
        }
    }
}
