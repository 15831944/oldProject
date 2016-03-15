
using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Linq;
using System.Text;
using System.ComponentModel;
using ElectronTransferDal.Common;
using ElectronTransferModel.Base;
using ElectronTransferModel.V9_4;

namespace ElectronTransferDal.AutoGeneration
{
    class DropDownList
    {
    }
    #region

    ///// <summary>
    ///// 检测点类别
    ///// </summary>
    //public class JcdlbList : StringConverter
    //{
    //    public override bool GetStandardValuesSupported(ITypeDescriptorContext context)
    //    {
    //        return true;
    //    }
    //    public override bool GetStandardValuesExclusive(ITypeDescriptorContext context)
    //    {
    //        return true;
    //    }
    //    public override TypeConverter.StandardValuesCollection GetStandardValues(ITypeDescriptorContext context)
    //    {
    //        IEnumerable<Cd_lb> jddz = CDDBManager.Instance.GetEntities<Cd_lb>().OrderBy(o => o.NAME);
    //        var str = jddz.Select(o => o.NAME).ToList();
    //        str.Add("");
    //        return new StandardValuesCollection(str);
    //    }
    //}
    ///// <summary>
    ///// PT柜分类
    ///// </summary>
    //public class PtList : StringConverter
    //{
    //    public override bool GetStandardValuesSupported(ITypeDescriptorContext context)
    //    {
    //        return true;
    //    }
    //    public override bool GetStandardValuesExclusive(ITypeDescriptorContext context)
    //    {
    //        return true;
    //    }
    //    public override TypeConverter.StandardValuesCollection GetStandardValues(ITypeDescriptorContext context)
    //    {
    //        IEnumerable<Cd_ptgfl> jddz = CDDBManager.Instance.GetEntities<Cd_ptgfl>().OrderBy(o => o.NAME);
    //        var str = jddz.Select(o => o.NAME).ToList();
    //        str.Add("");
    //        return new StandardValuesCollection(str);
    //    }
    //}
    ///// <summary>
    ///// 计量表类别
    ///// </summary>
    //public class JlbList : StringConverter
    //{
    //    public override bool GetStandardValuesSupported(ITypeDescriptorContext context)
    //    {
    //        return true;
    //    }
    //    public override bool GetStandardValuesExclusive(ITypeDescriptorContext context)
    //    {
    //        return true;
    //    }
    //    public override TypeConverter.StandardValuesCollection GetStandardValues(ITypeDescriptorContext context)
    //    {
    //        IEnumerable<Cd_jlblx> jddz = CDDBManager.Instance.GetEntities<Cd_jlblx>().OrderBy(o => o.NAME);
    //        var str = jddz.Select(o => o.NAME).ToList();
    //        str.Add("");
    //        return new StandardValuesCollection(str);
    //    }
    //}
    ///// <summary>
    ///// 低压支撑类型
    ///// </summary>
    //public class ZclxList : StringConverter
    //{
    //    public override bool GetStandardValuesSupported(ITypeDescriptorContext context)
    //    {
    //        return true;
    //    }
    //    public override bool GetStandardValuesExclusive(ITypeDescriptorContext context)
    //    {
    //        return true;
    //    }
    //    public override TypeConverter.StandardValuesCollection GetStandardValues(ITypeDescriptorContext context)
    //    {
    //        IEnumerable<Cd_zclx> jddz = CDDBManager.Instance.GetEntities<Cd_zclx>().OrderBy(o => o.NAME);
    //        var str = jddz.Select(o => o.NAME).ToList();
    //        str.Add("");
    //        return new StandardValuesCollection(str);
    //    }
    //}
    ///// <summary>
    ///// 接地刀闸样式
    ///// </summary>
    //public class JddzysList : StringConverter
    //{
    //    public override bool GetStandardValuesSupported(ITypeDescriptorContext context)
    //    {
    //        return true;
    //    }
    //    public override bool GetStandardValuesExclusive(ITypeDescriptorContext context)
    //    {
    //        return true;
    //    }
    //    public override TypeConverter.StandardValuesCollection GetStandardValues(ITypeDescriptorContext context)
    //    {
    //        IEnumerable<Cd_jddzys> jddz = CDDBManager.Instance.GetEntities<Cd_jddzys>().OrderBy(o => o.NAME);
    //        var str = jddz.Select(o => o.NAME).ToList();
    //        str.Add("");
    //        return new StandardValuesCollection(str);
    //    }
    //}
    ///// <summary>
    ///// 是否多源供电
    ///// </summary>
    //public class SfdygdList : StringConverter
    //{
    //    public override bool GetStandardValuesSupported(ITypeDescriptorContext context)
    //    {
    //        return true;
    //    }
    //    public override bool GetStandardValuesExclusive(ITypeDescriptorContext context)
    //    {
    //        return true;
    //    }
    //    public override TypeConverter.StandardValuesCollection GetStandardValues(ITypeDescriptorContext context)
    //    {
    //        IEnumerable<Cd_sfdqkx> jddz = CDDBManager.Instance.GetEntities<Cd_sfdqkx>().OrderBy(o => o.NAME);
    //        var str = jddz.Select(o => o.NAME).ToList();
    //        str.Add("");
    //        return new StandardValuesCollection(str);
    //    }
    //}
   

    ///// <summary>
    ///// 配变类别
    ///// </summary>
    //public class PblbList : StringConverter
    //{
    //    public override bool GetStandardValuesSupported(ITypeDescriptorContext context)
    //    {
    //        return true;
    //    }
    //    public override bool GetStandardValuesExclusive(ITypeDescriptorContext context)
    //    {
    //        return true;
    //    }
    //    public override TypeConverter.StandardValuesCollection GetStandardValues(ITypeDescriptorContext context)
    //    {
    //        IEnumerable<Cd_pblb> pblb = CDDBManager.Instance.GetEntities<Cd_pblb>().OrderBy(o => o.NAME);
    //        var str = pblb.Select(o => o.NAME).ToList();
    //        str.Add("");
    //        return new StandardValuesCollection(str);
    //    }
    //}


    ///// <summary>
    ///// 计量柜分类
    ///// </summary>
    //public class JlgList : StringConverter
    //{
    //    public override bool GetStandardValuesSupported(ITypeDescriptorContext context)
    //    {
    //        return true;
    //    }
    //    public override bool GetStandardValuesExclusive(ITypeDescriptorContext context)
    //    {
    //        return true;
    //    }
    //    public override TypeConverter.StandardValuesCollection GetStandardValues(ITypeDescriptorContext context)
    //    {
    //        IEnumerable<Cd_sblx_jlg> byq = CDDBManager.Instance.GetEntities<Cd_sblx_jlg>().OrderBy(o => o.NAME);
    //        var str = byq.Select(o => o.NAME).ToList();
    //        str.Add("");
    //        return new StandardValuesCollection(str);
    //    }
    //}
    ///// <summary>
    ///// 柱上变压器分类
    ///// </summary>
    //public class ZsbyqList : StringConverter
    //{
    //    public override bool GetStandardValuesSupported(ITypeDescriptorContext context)
    //    {
    //        return true;
    //    }
    //    public override bool GetStandardValuesExclusive(ITypeDescriptorContext context)
    //    {
    //        return true;
    //    }
    //    public override TypeConverter.StandardValuesCollection GetStandardValues(ITypeDescriptorContext context)
    //    {
    //        IEnumerable<Cd_zsbyqfl> byq = CDDBManager.Instance.GetEntities<Cd_zsbyqfl>().OrderBy(o => o.NAME);
    //        var str = byq.Select(o => o.NAME).ToList();
    //        str.Add("");
    //        return new StandardValuesCollection(str);
    //    }
    //}
    ///// <summary>
    ///// 代表区域
    ///// </summary>
    //public class DbqyList : StringConverter
    //{
    //    public override bool GetStandardValuesSupported(ITypeDescriptorContext context)
    //    {
    //        return true;
    //    }
    //    public override bool GetStandardValuesExclusive(ITypeDescriptorContext context)
    //    {
    //        return true;
    //    }
    //    public override TypeConverter.StandardValuesCollection GetStandardValues(ITypeDescriptorContext context)
    //    {
    //        IEnumerable<Cd_dbqy> dbqy = CDDBManager.Instance.GetEntities<Cd_dbqy>().OrderBy(o => o.NAME);
    //        var str = dbqy.Select(o => o.NAME).ToList();
    //        str.Add("");
    //        return new StandardValuesCollection(str);
    //    }
    //}
    ///// <summary>
    ///// 是否为级联的从终端
    ///// </summary>
    //public class SfczdList : StringConverter
    //{
    //    public override bool GetStandardValuesSupported(ITypeDescriptorContext context)
    //    {
    //        return true;
    //    }
    //    public override bool GetStandardValuesExclusive(ITypeDescriptorContext context)
    //    {
    //        return true;
    //    }
    //    public override TypeConverter.StandardValuesCollection GetStandardValues(ITypeDescriptorContext context)
    //    {
    //        IEnumerable<Cd_sfczd> byq = CDDBManager.Instance.GetEntities<Cd_sfczd>().OrderBy(o => o.NAME);
    //        var str = byq.Select(o => o.NAME).ToList();
    //        str.Add("");
    //        return new StandardValuesCollection(str);
    //    }
    //}
    ///// <summary>
    ///// 灭弧方式
    ///// </summary>
    //public class MhfsList : StringConverter
    //{
    //    public override bool GetStandardValuesSupported(ITypeDescriptorContext context)
    //    {
    //        return true;
    //    }
    //    public override bool GetStandardValuesExclusive(ITypeDescriptorContext context)
    //    {
    //        return true;
    //    }
    //    public override TypeConverter.StandardValuesCollection GetStandardValues(ITypeDescriptorContext context)
    //    {
    //        IEnumerable<Cd_mhxx> mhxx = CDDBManager.Instance.GetEntities<Cd_mhxx>().OrderBy(o => o.NAME);
    //        var str = mhxx.Select(o => o.NAME).ToList();
    //        str.Add("");
    //        return new StandardValuesCollection(str);
    //    }
    //}
    ///// <summary>
    ///// 工作状态
    ///// </summary>
    //public class GzztList : StringConverter
    //{
    //    public override bool GetStandardValuesSupported(ITypeDescriptorContext context)
    //    {
    //        return true;
    //    }
    //    public override bool GetStandardValuesExclusive(ITypeDescriptorContext context)
    //    {
    //        return true;
    //    }
    //    public override TypeConverter.StandardValuesCollection GetStandardValues(ITypeDescriptorContext context)
    //    {
    //        IEnumerable<Cd_gzzt> gzzt = CDDBManager.Instance.GetEntities<Cd_gzzt>().OrderBy(o => o.NAME);
    //        var str = gzzt.Select(o => o.NAME).ToList();
    //        str.Add("");
    //        return new StandardValuesCollection(str);
    //    }
    //}
    ///// <summary>
    ///// 相数
    ///// </summary>
    //public class XsList : StringConverter
    //{
    //    public override bool GetStandardValuesSupported(ITypeDescriptorContext context)
    //    {
    //        return true;
    //    }
    //    public override bool GetStandardValuesExclusive(ITypeDescriptorContext context)
    //    {
    //        return true;
    //    }
    //    public override TypeConverter.StandardValuesCollection GetStandardValues(ITypeDescriptorContext context)
    //    {
    //        IEnumerable<Cd_xs> xs = CDDBManager.Instance.GetEntities<Cd_xs>().OrderBy(o => o.NAME);
    //        var str = xs.Select(o => o.NAME).ToList();
    //        str.Add("");
    //        return new StandardValuesCollection(str);
    //    }
    //}
    ///// <summary>
    ///// 抄表箱类型
    ///// </summary>
    //public class CbxList : StringConverter
    //{
    //    public override bool GetStandardValuesSupported(ITypeDescriptorContext context)
    //    {
    //        return true;
    //    }
    //    public override bool GetStandardValuesExclusive(ITypeDescriptorContext context)
    //    {
    //        return true;
    //    }
    //    public override TypeConverter.StandardValuesCollection GetStandardValues(ITypeDescriptorContext context)
    //    {
    //        IEnumerable<Cd_cbx_lx> xs = CDDBManager.Instance.GetEntities<Cd_cbx_lx>().OrderBy(o => o.NAME);
    //        var str = xs.Select(o => o.NAME).ToList();
    //        str.Add("");
    //        return new StandardValuesCollection(str);
    //    }
    //}
    ///// <summary>
    ///// 柱上短路器类型
    ///// </summary>
    //public class ZsdlqList : StringConverter
    //{
    //    public override bool GetStandardValuesSupported(ITypeDescriptorContext context)
    //    {
    //        return true;
    //    }
    //    public override bool GetStandardValuesExclusive(ITypeDescriptorContext context)
    //    {
    //        return true;
    //    }
    //    public override TypeConverter.StandardValuesCollection GetStandardValues(ITypeDescriptorContext context)
    //    {
    //        IEnumerable<Cd_zsdlqlx> xs = CDDBManager.Instance.GetEntities<Cd_zsdlqlx>().OrderBy(o => o.NAME);
    //        var str = xs.Select(o => o.NAME).ToList();
    //        str.Add("");
    //        return new StandardValuesCollection(str);
    //    }
    //}
    ///// <summary>
    ///// 电缆沿布
    ///// </summary>
    //public class DlybList : StringConverter
    //{
    //    public override bool GetStandardValuesSupported(ITypeDescriptorContext context)
    //    {
    //        return true;
    //    }
    //    public override bool GetStandardValuesExclusive(ITypeDescriptorContext context)
    //    {
    //        return true;
    //    }
    //    public override TypeConverter.StandardValuesCollection GetStandardValues(ITypeDescriptorContext context)
    //    {
    //        IEnumerable<Cd_dldlyb> dlyb = CDDBManager.Instance.GetEntities<Cd_dldlyb>().OrderBy(o => o.NAME);
    //        var str = dlyb.Select(o => o.NAME).ToList();
    //        str.Add("");
    //        return new StandardValuesCollection(str);
    //    }
    //}
    ///// <summary>
    ///// 设备评级
    ///// </summary>
    //public class SbpjList : StringConverter
    //{
    //    public override bool GetStandardValuesSupported(ITypeDescriptorContext context)
    //    {
    //        return true;
    //    }
    //    public override bool GetStandardValuesExclusive(ITypeDescriptorContext context)
    //    {
    //        return true;
    //    }
    //    public override TypeConverter.StandardValuesCollection GetStandardValues(ITypeDescriptorContext context)
    //    {
    //        IEnumerable<Cd_sbpj> sbpj = CDDBManager.Instance.GetEntities<Cd_sbpj>().OrderBy(o => o.NAME);
    //        var str = sbpj.Select(o => o.NAME).ToList();
    //        str.Add("");
    //        return new StandardValuesCollection(str);
    //    }
    //}
    ///// <summary>
    ///// 用电类别
    ///// </summary>
    //public class YdlbList : StringConverter
    //{
    //    public override bool GetStandardValuesSupported(ITypeDescriptorContext context)
    //    {
    //        return true;
    //    }
    //    public override bool GetStandardValuesExclusive(ITypeDescriptorContext context)
    //    {
    //        return true;
    //    }
    //    public override TypeConverter.StandardValuesCollection GetStandardValues(ITypeDescriptorContext context)
    //    {
    //        IEnumerable<Cd_ydlb> ydlb = CDDBManager.Instance.GetEntities<Cd_ydlb>().OrderBy(o => o.NAME);
    //        var str = ydlb.Select(o => o.NAME).ToList();
    //        str.Add("");
    //        return new StandardValuesCollection(str);
    //    }
    //}

  
    ///// <summary>
    ///// 生命周期
    ///// </summary>
    //public class SmzqList : StringConverter
    //{
    //    public override bool GetStandardValuesSupported(ITypeDescriptorContext context)
    //    {
    //        return true;
    //    }
    //    public override bool GetStandardValuesExclusive(ITypeDescriptorContext context)
    //    {
    //        return true;
    //    }
    //    public override TypeConverter.StandardValuesCollection GetStandardValues(ITypeDescriptorContext context)
    //    {
    //        IEnumerable<Cd_smzq> smzq = CDDBManager.Instance.GetEntities<Cd_smzq>().OrderBy(o => o.NAME);
    //        List<string> str = smzq.Select(o => o.NAME).ToList();
    //        str.Add("");
    //        return new StandardValuesCollection(str);
    //    }
    //}
    ///// <summary>
    ///// 电压等级
    ///// </summary>
    //public class DydjList : StringConverter
    //{
    //    public override bool GetStandardValuesSupported(ITypeDescriptorContext context)
    //    {
    //        return true;
    //    }
    //    public override bool GetStandardValuesExclusive(ITypeDescriptorContext context)
    //    {
    //        return true;
    //    }
    //    public override TypeConverter.StandardValuesCollection GetStandardValues(ITypeDescriptorContext context)
    //    {
    //        IEnumerable<Cd_dydj> dydj = CDDBManager.Instance.GetEntities<Cd_dydj>().OrderBy(o => o.NAME);
    //        var str = dydj.Select(o => o.NAME).ToList();
    //        str.Add("");
    //        return new StandardValuesCollection(str);
    //    }
    //}
    ///// <summary>
    ///// 地区特征
    ///// </summary>
    //public class DqtzList : StringConverter
    //{
    //    public override bool GetStandardValuesSupported(ITypeDescriptorContext context)
    //    {
    //        return true;
    //    }
    //    public override bool GetStandardValuesExclusive(ITypeDescriptorContext context)
    //    {
    //        return true;
    //    }
    //    public override TypeConverter.StandardValuesCollection GetStandardValues(ITypeDescriptorContext context)
    //    {
    //        IEnumerable<Cd_dqtz> dqtz = CDDBManager.Instance.GetEntities<Cd_dqtz>().OrderBy(o => o.NAME);
    //        var str = dqtz.Select(o => o.NAME).ToList();
    //        str.Add("");
    //        return new StandardValuesCollection(str);
    //    }
    //}

    ///// <summary>
    ///// 电杆分类
    ///// </summary>
    //public class CllbList : StringConverter
    //{
    //    public override bool GetStandardValuesSupported(ITypeDescriptorContext context)
    //    {
    //        return true;
    //    }
    //    public override bool GetStandardValuesExclusive(ITypeDescriptorContext context)
    //    {
    //        return true;
    //    }
    //    public override TypeConverter.StandardValuesCollection GetStandardValues(ITypeDescriptorContext context)
    //    {
    //        IEnumerable<Cd_cllb> cllb = CDDBManager.Instance.GetEntities<Cd_cllb>().OrderBy(o => o.NAME);
    //        var str = cllb.Select(o => o.NAME).ToList();
    //        str.Add("");
    //        return new StandardValuesCollection(str);
    //    }
    //}
    ///// <summary>
    ///// 通讯方式
    ///// </summary>
    //public class TxfsList : StringConverter
    //{
    //    public override bool GetStandardValuesSupported(ITypeDescriptorContext context)
    //    {
    //        return true;
    //    }
    //    public override bool GetStandardValuesExclusive(ITypeDescriptorContext context)
    //    {
    //        return true;
    //    }
    //    public override TypeConverter.StandardValuesCollection GetStandardValues(ITypeDescriptorContext context)
    //    {
    //        IEnumerable<Cd_txfs> txfs = CDDBManager.Instance.GetEntities<Cd_txfs>().OrderBy(o => o.NAME);
    //        var str = txfs.Select(o => o.NAME).ToList();
    //        str.Add("");
    //        return new StandardValuesCollection(str);
    //    }
    //}
    ///// <summary>
    ///// 配网仪分类
    ///// </summary>
    //public class PwyList : StringConverter
    //{
    //    public override bool GetStandardValuesSupported(ITypeDescriptorContext context)
    //    {
    //        return true;
    //    }
    //    public override bool GetStandardValuesExclusive(ITypeDescriptorContext context)
    //    {
    //        return true;
    //    }
    //    public override TypeConverter.StandardValuesCollection GetStandardValues(ITypeDescriptorContext context)
    //    {
    //        IEnumerable<Cd_sblx_ttu> pwy = CDDBManager.Instance.GetEntities<Cd_sblx_ttu>().OrderBy(o => o.NAME);
    //        var str = pwy.Select(o => o.NAME).ToList();
    //        str.Add("");
    //        return new StandardValuesCollection(str);
    //    }
    //}
    ///// <summary>
    ///// 电房分类
    ///// </summary>
    //public class DfList : StringConverter
    //{
    //    public override bool GetStandardValuesSupported(ITypeDescriptorContext context)
    //    {
    //        return true;
    //    }
    //    public override bool GetStandardValuesExclusive(ITypeDescriptorContext context)
    //    {
    //        return true;
    //    }
    //    public override TypeConverter.StandardValuesCollection GetStandardValues(ITypeDescriptorContext context)
    //    {
    //        IEnumerable<Cd_dflx> df = CDDBManager.Instance.GetEntities<Cd_dflx>().OrderBy(o => o.NAME);
    //        var str = df.Select(o => o.NAME).ToList();
    //        str.Add("");
    //        return new StandardValuesCollection(str);
    //    }
    //}
    ///// <summary>
    ///// 中压截面积
    ///// </summary>
    //public class ZyJmjList : DoubleConverter
    //{
    //    public override bool GetStandardValuesSupported(ITypeDescriptorContext context)
    //    {
    //        return true;
    //    }
    //    public override bool GetStandardValuesExclusive(ITypeDescriptorContext context)
    //    {
    //        return true;
    //    }
    //    public override TypeConverter.StandardValuesCollection GetStandardValues(ITypeDescriptorContext context)
    //    {
    //        IEnumerable<Cd_jmj> jmj = CDDBManager.Instance.GetEntities<Cd_jmj>().OrderBy(o => o.NAME);
    //        var str = jmj.Select(o => o.NAME).ToList();
    //        str.Add("");
    //        return new StandardValuesCollection(str);
    //    }
    //}
    ///// <summary>
    ///// 变低出线
    ///// </summary>
    //public class BdcxflList : StringConverter
    //{
    //    public override bool GetStandardValuesSupported(ITypeDescriptorContext context)
    //    {
    //        return true;
    //    }
    //    public override bool GetStandardValuesExclusive(ITypeDescriptorContext context)
    //    {
    //        return true;
    //    }
    //    public override TypeConverter.StandardValuesCollection GetStandardValues(ITypeDescriptorContext context)
    //    {
    //        IEnumerable<Cd_bdcxfl> bdcx = CDDBManager.Instance.GetEntities<Cd_bdcxfl>().OrderBy(o => o.NAME);
    //        var str = bdcx.Select(o => o.NAME).ToList();
    //        str.Add("");
    //        return new StandardValuesCollection(str);
    //    }
    //}
    ///// <summary>
    ///// 电缆分线箱
    ///// </summary>
    //public class DlfxxflList : StringConverter
    //{
    //    public override bool GetStandardValuesSupported(ITypeDescriptorContext context)
    //    {
    //        return true;
    //    }
    //    public override bool GetStandardValuesExclusive(ITypeDescriptorContext context)
    //    {
    //        return true;
    //    }
    //    public override TypeConverter.StandardValuesCollection GetStandardValues(ITypeDescriptorContext context)
    //    {
    //        IEnumerable<Cd_dlfxxfl> dlfxx = CDDBManager.Instance.GetEntities<Cd_dlfxxfl>().OrderBy(o => o.NAME);
    //        var str = dlfxx.Select(o => o.NAME).ToList();
    //        str.Add("");
    //        return new StandardValuesCollection(str);
    //    }
    //}
    ///// <summary>
    ///// 粗细度
    ///// </summary>
    //public class CxdList : StringConverter
    //{
    //    public override bool GetStandardValuesSupported(ITypeDescriptorContext context)
    //    {
    //        return true;
    //    }
    //    public override bool GetStandardValuesExclusive(ITypeDescriptorContext context)
    //    {
    //        return true;
    //    }
    //    public override TypeConverter.StandardValuesCollection GetStandardValues(ITypeDescriptorContext context)
    //    {
    //        IEnumerable<Cd_cxd> cxd = CDDBManager.Instance.GetEntities<Cd_cxd>().OrderBy(o => o.NAME);
    //        var str = cxd.Select(o => o.NAME).ToList();
    //        str.Add("");
    //        return new StandardValuesCollection(str);
    //    }
    //}
    ///// <summary>
    ///// 电缆头分类
    ///// </summary>
    //public class DltflList : StringConverter
    //{
    //    public override bool GetStandardValuesSupported(ITypeDescriptorContext context)
    //    {
    //        return true;
    //    }
    //    public override bool GetStandardValuesExclusive(ITypeDescriptorContext context)
    //    {
    //        return true;
    //    }
    //    public override TypeConverter.StandardValuesCollection GetStandardValues(ITypeDescriptorContext context)
    //    {
    //        IEnumerable<Cd_sfsdlt> dltfl = CDDBManager.Instance.GetEntities<Cd_sfsdlt>().OrderBy(o => o.NAME);
    //        var str = dltfl.Select(o => o.NAME).ToList();
    //        str.Add("");
    //        return new StandardValuesCollection(str);
    //    }
    //}

    ///// <summary>
    ///// 低压开关分类
    ///// </summary>
    //public class DykgflList : StringConverter
    //{
    //    public override bool GetStandardValuesSupported(ITypeDescriptorContext context)
    //    {
    //        return true;
    //    }
    //    public override bool GetStandardValuesExclusive(ITypeDescriptorContext context)
    //    {
    //        return true;
    //    }
    //    public override TypeConverter.StandardValuesCollection GetStandardValues(ITypeDescriptorContext context)
    //    {
    //        IEnumerable<Cd_dykgfl> dykg = CDDBManager.Instance.GetEntities<Cd_dykgfl>().OrderBy(o => o.NAME);
    //        var str = dykg.Select(o => o.NAME).ToList();
    //        str.Add("");
    //        return new StandardValuesCollection(str);
    //    }
    //}
    ///// <summary>
    ///// 节点类型
    ///// </summary>
    //public class JdlxList : StringConverter
    //{
    //    public override bool GetStandardValuesSupported(ITypeDescriptorContext context)
    //    {
    //        return true;
    //    }
    //    public override bool GetStandardValuesExclusive(ITypeDescriptorContext context)
    //    {
    //        return true;
    //    }
    //    public override TypeConverter.StandardValuesCollection GetStandardValues(ITypeDescriptorContext context)
    //    {
    //        IEnumerable<Cd_jdlx> jdlx = CDDBManager.Instance.GetEntities<Cd_jdlx>().OrderBy(o => o.NAME);
    //        var str = jdlx.Select(o => o.NAME).ToList();
    //        str.Add("");
    //        return new StandardValuesCollection(str);
    //    }
    //}
    ///// <summary>
    ///// 是否智能门锁
    ///// </summary>
    //public class SfznmsList : StringConverter
    //{
    //    public override bool GetStandardValuesSupported(ITypeDescriptorContext context)
    //    {
    //        return true;
    //    }
    //    public override bool GetStandardValuesExclusive(ITypeDescriptorContext context)
    //    {
    //        return true;
    //    }
    //    public override TypeConverter.StandardValuesCollection GetStandardValues(ITypeDescriptorContext context)
    //    {
    //        IEnumerable<Cd_yfznms> znms = CDDBManager.Instance.GetEntities<Cd_yfznms>().OrderBy(o => o.NAME);
    //        var str = znms.Select(o => o.NAME).ToList();
    //        str.Add("");
    //        return new StandardValuesCollection(str);
    //    }
    //}
    ///// <summary>
    ///// 线路分类（干线/支线）
    ///// </summary>
    //public class XlflList : StringConverter
    //{
    //    public override bool GetStandardValuesSupported(ITypeDescriptorContext context)
    //    {
    //        return true;
    //    }
    //    public override bool GetStandardValuesExclusive(ITypeDescriptorContext context)
    //    {
    //        return true;
    //    }
    //    public override TypeConverter.StandardValuesCollection GetStandardValues(ITypeDescriptorContext context)
    //    {

    //        IEnumerable<Cd_xlfl> xlfl = CDDBManager.Instance.GetEntities<Cd_xlfl>().OrderBy(o => o.NAME);
    //        var str = xlfl.Select(o => o.NAME).ToList();
    //        str.Add("");
    //        return new StandardValuesCollection(str);
    //    }
    //}
    ///// <summary>
    ///// 产权属性
    ///// </summary>
    //public class CqsxList : StringConverter
    //{
    //    public override bool GetStandardValuesSupported(ITypeDescriptorContext context)
    //    {
    //        return true;
    //    }
    //    public override bool GetStandardValuesExclusive(ITypeDescriptorContext context)
    //    {
    //        return true;
    //    }
    //    public override TypeConverter.StandardValuesCollection GetStandardValues(ITypeDescriptorContext context)
    //    {
    //        IEnumerable<Cd_cqsx> cqsx = CDDBManager.Instance.GetEntities<Cd_cqsx>().OrderBy(o => o.NAME);
    //        var str = cqsx.Select(o => o.NAME).ToList();
    //        str.Add("");
    //        return new StandardValuesCollection(str);
    //    }
    //}
    ///// <summary>
    ///// 是否可带负荷操作
    ///// </summary>
    //public class SfdfhczList : StringConverter
    //{
    //    public override bool GetStandardValuesSupported(ITypeDescriptorContext context)
    //    {
    //        return true;
    //    }
    //    public override bool GetStandardValuesExclusive(ITypeDescriptorContext context)
    //    {
    //        return true;
    //    }
    //    public override TypeConverter.StandardValuesCollection GetStandardValues(ITypeDescriptorContext context)
    //    {
    //        IEnumerable<Cd_sfsdl> sfsdl = CDDBManager.Instance.GetEntities<Cd_sfsdl>().OrderBy(o => o.NAME);
    //        var str = sfsdl.Select(o => o.NAME).ToList();
    //        str.Add("");
    //        return new StandardValuesCollection(str);
    //    }
    //}
    ///// <summary>
    ///// 是否带电
    ///// </summary>
    //public class SfddList : StringConverter
    //{
    //    public override bool GetStandardValuesSupported(ITypeDescriptorContext context)
    //    {
    //        return true;
    //    }
    //    public override bool GetStandardValuesExclusive(ITypeDescriptorContext context)
    //    {
    //        return true;
    //    }
    //    public override TypeConverter.StandardValuesCollection GetStandardValues(ITypeDescriptorContext context)
    //    {
    //        IEnumerable<Cd_sfdd> sfdd = CDDBManager.Instance.GetEntities<Cd_sfdd>().OrderBy(o => o.NAME);
    //        var str = sfdd.Select(o => o.NAME).ToList();
    //        str.Add("");
    //        return new StandardValuesCollection(str);
    //    }
    //}
    ///// <summary>
    ///// 当前状态
    ///// </summary>
    //public class DqztList : StringConverter
    //{
    //    public override bool GetStandardValuesSupported(ITypeDescriptorContext context)
    //    {
    //        return true;
    //    }
    //    public override bool GetStandardValuesExclusive(ITypeDescriptorContext context)
    //    {
    //        return true;
    //    }
    //    public override TypeConverter.StandardValuesCollection GetStandardValues(ITypeDescriptorContext context)
    //    {
    //        IEnumerable<Cd_dqzt> dqzt = CDDBManager.Instance.GetEntities<Cd_dqzt>().OrderBy(o => o.NAME);
    //        var str = dqzt.Select(o => o.NAME).ToList();
    //        str.Add("");
    //        return new StandardValuesCollection(str);
    //    }
    //}
    ///// <summary>
    ///// 功能位置设施分类
    ///// </summary>
    //public class GnssflList : StringConverter
    //{
    //    public override bool GetStandardValuesSupported(ITypeDescriptorContext context)
    //    {
    //        return true;
    //    }
    //    public override bool GetStandardValuesExclusive(ITypeDescriptorContext context)
    //    {
    //        return true;
    //    }
    //    public override TypeConverter.StandardValuesCollection GetStandardValues(ITypeDescriptorContext context)
    //    {
    //        IEnumerable<Cd_ssfl> ssfl = CDDBManager.Instance.GetEntities<Cd_ssfl>().OrderBy(o => o.NAME);
    //        var str = ssfl.Select(o => o.NAME).ToList();
    //        str.Add("");
    //        return new StandardValuesCollection(str);
    //    }
    //}
    ///// <summary>
    ///// 是否带电操机构
    ///// </summary>
    //public class SfdcjgList : StringConverter
    //{
    //    public override bool GetStandardValuesSupported(ITypeDescriptorContext context)
    //    {
    //        return true;
    //    }
    //    public override bool GetStandardValuesExclusive(ITypeDescriptorContext context)
    //    {
    //        return true;
    //    }
    //    public override TypeConverter.StandardValuesCollection GetStandardValues(ITypeDescriptorContext context)
    //    {
    //        IEnumerable<Cd_sfdcjg> dcjg = CDDBManager.Instance.GetEntities<Cd_sfdcjg>().OrderBy(o => o.NAME);
    //        var str = dcjg.Select(o => o.NAME).ToList();
    //        str.Add("");
    //        return new StandardValuesCollection(str);
    //    }
    //}
    ///// <summary>
    ///// 是否带熔断器
    ///// </summary>
    //public class SfdrdqList : StringConverter
    //{
    //    public override bool GetStandardValuesSupported(ITypeDescriptorContext context)
    //    {
    //        return true;
    //    }
    //    public override bool GetStandardValuesExclusive(ITypeDescriptorContext context)
    //    {
    //        return true;
    //    }
    //    public override TypeConverter.StandardValuesCollection GetStandardValues(ITypeDescriptorContext context)
    //    {
    //        IEnumerable<Cd_sfdyrdq> rdq = CDDBManager.Instance.GetEntities<Cd_sfdyrdq>().OrderBy(o => o.NAME);
    //        var str = rdq.Select(o => o.NAME).ToList();
    //        str.Add("");
    //        return new StandardValuesCollection(str);
    //    }
    //}
    ///// <summary>
    ///// 是否站出线
    ///// </summary>
    //public class SfzcxList : StringConverter
    //{
    //    public override bool GetStandardValuesSupported(ITypeDescriptorContext context)
    //    {
    //        return true;
    //    }
    //    public override bool GetStandardValuesExclusive(ITypeDescriptorContext context)
    //    {
    //        return true;
    //    }
    //    public override TypeConverter.StandardValuesCollection GetStandardValues(ITypeDescriptorContext context)
    //    {
    //        IEnumerable<Cd_sfzcx> zcx = CDDBManager.Instance.GetEntities<Cd_sfzcx>().OrderBy(o => o.NAME);
    //        var str = zcx.Select(o => o.NAME).ToList();
    //        str.Add("");
    //        return new StandardValuesCollection(str);
    //    }
    //}
    ///// <summary>
    ///// 同杆架设情况
    ///// </summary>
    //public class TgjsqkList : StringConverter
    //{
    //    public override bool GetStandardValuesSupported(ITypeDescriptorContext context)
    //    {
    //        return true;
    //    }
    //    public override bool GetStandardValuesExclusive(ITypeDescriptorContext context)
    //    {
    //        return true;
    //    }
    //    public override TypeConverter.StandardValuesCollection GetStandardValues(ITypeDescriptorContext context)
    //    {
    //        IEnumerable<Cd_ggjsqk> tgjs = CDDBManager.Instance.GetEntities<Cd_ggjsqk>().OrderBy(o => o.NAME);
    //        var str = tgjs.Select(o => o.NAME).ToList();
    //        str.Add("");
    //        return new StandardValuesCollection(str);
    //    }
    //}
    ///// <summary>
    ///// 维护归属
    ///// </summary>
    //public class WhgsList : StringConverter
    //{
    //    public override bool GetStandardValuesSupported(ITypeDescriptorContext context)
    //    {
    //        return true;
    //    }
    //    public override bool GetStandardValuesExclusive(ITypeDescriptorContext context)
    //    {
    //        return true;
    //    }
    //    public override TypeConverter.StandardValuesCollection GetStandardValues(ITypeDescriptorContext context)
    //    {
    //        IEnumerable<Cd_whgs> whgs = CDDBManager.Instance.GetEntities<Cd_whgs>().OrderBy(o => o.NAME);
    //        var str = whgs.Select(o => o.NAME).ToList();
    //        str.Add("");
    //        return new StandardValuesCollection(str);
    //    }
    //}
    #endregion
    /// <summary>
    /// 所属供电所,维护班所
    /// </summary>
    public class SsgdsList : StringConverter
    {
        public override bool GetStandardValuesSupported(ITypeDescriptorContext context)
        {
            return true;
        }
        public override bool GetStandardValuesExclusive(ITypeDescriptorContext context)
        {
            return true;
        }
        public override TypeConverter.StandardValuesCollection GetStandardValues(ITypeDescriptorContext context)
        {
            var pd = context.Instance;
            if (pd!=null)
            {
                XProps xprops = (XProps) pd;
                var result = GenerateHelper.GetFirstXPropOfHasValue(xprops, "CD_SSDW");
                if (result!=null)
                {
                    var gdj = result.Value;
                    if (gdj != null && !string.IsNullOrEmpty(gdj.ToString()))
                    {
                        IEnumerable<Cd_bs> query =
                            CDDBManager.Instance.GetEntities<Cd_bs>(
                                o => !string.IsNullOrEmpty(o.SSDW) && o.SSDW.Equals(gdj.ToString()));
                        IEnumerable<Cd_bs> gdjBs =
                            CDDBManager.Instance.GetEntities<Cd_bs>(
                                o => string.IsNullOrEmpty(o.SSDW) && !o.NAME.Contains("供电局"));
                        IEnumerable<Cd_bs> selfBs =
                            CDDBManager.Instance.GetEntities<Cd_bs>(
                                o =>
                                    string.IsNullOrEmpty(o.SSDW) && o.NAME.Contains(gdj.ToString()) ||
                                    o.NAME.Equals("江门江门供电局"));

                        query = query.Union(gdjBs);
                        query = query.Union(selfBs).OrderBy(o => o.NAME);
                        var res = query.Select(o => o.NAME).Distinct().ToList();
                        res.Add("");
                        return new StandardValuesCollection(res);
                    }
                }
            }
            return new StandardValuesCollection(null);
        }
    }
    /// <summary>
    /// 所属单位
    /// </summary>
    public class SsgdjList : StringConverter
    {
        public override bool GetStandardValuesSupported(ITypeDescriptorContext context)
        {
            return true;
        }
        public override bool GetStandardValuesExclusive(ITypeDescriptorContext context)
        {
            return true;
        }
        public override TypeConverter.StandardValuesCollection GetStandardValues(ITypeDescriptorContext context)
        {
            IEnumerable<Cd_ssxl> query = CDDBManager.Instance.GetEntities<Cd_ssxl>().OrderBy(o => o.CD_SSDW);
            var ssdw = query.Select(o => o.CD_SSDW).Distinct().ToList();
            ssdw.Add("");
            return new StandardValuesCollection(ssdw);

        }
    }
    /// <summary>
    /// 所属变电站
    /// </summary>
    public class SsbdzList : StringConverter
    {
        public override bool GetStandardValuesSupported(ITypeDescriptorContext context)
        {
            return true;
        }
        public override bool GetStandardValuesExclusive(ITypeDescriptorContext context)
        {
            return true;
        }
        public override TypeConverter.StandardValuesCollection GetStandardValues(ITypeDescriptorContext context)
        {
            var pd = context.Instance;
            if (pd != null)
            {
                XProps xprops = (XProps) pd;
                var result = GenerateHelper.GetFirstXPropOfHasValue(xprops, "CD_SSDW");
                if (result!=null)
                {
                    var ssgdj = result.Value;
                    if (ssgdj!=null&&!string.IsNullOrEmpty(ssgdj.ToString()))
                    {
                        var bdz = CDDBManager.Instance.GetEntities<Cd_ssxl>().OrderBy(o => o.SSBDZ);
                        var bdzlist = bdz.Where(o => o.CD_SSDW == ssgdj.ToString()).Select(o => o.SSBDZ).Distinct().ToList();
                        bdzlist.Add("");
                        return new StandardValuesCollection(bdzlist);
                    }
                }
               
            }
            return new StandardValuesCollection(null);
        }
    }
    /// <summary>
    /// 受电馈线
    /// </summary>
    public class SsxlList : StringConverter
    {
        public String InPut { get; set; }
        public override bool GetStandardValuesSupported(ITypeDescriptorContext context)
        {
            return true;
        }
        public override bool GetStandardValuesExclusive(ITypeDescriptorContext context)
        {
            //GetKxmcs(context);
            return true;
        }
        public override TypeConverter.StandardValuesCollection GetStandardValues(ITypeDescriptorContext context)
        {
            var pd = context.Instance;
            if (pd != null)
            {
                XProps xprops = (XProps) pd;
                var result = GenerateHelper.GetFirstXPropOfHasValue(xprops, "CD_SSBDZ");
                if (result!=null)
                {
                    var bdz = result.Value;
                    if (bdz!=null&&!string.IsNullOrEmpty(bdz.ToString()))
                    {
                       var kx = CDDBManager.Instance.GetEntities<Cd_ssxl>().OrderBy(o => o.NAME);
                        var query = kx.Where(o => o.SSBDZ == bdz.ToString()).Select(o => o.NAME).Distinct().ToList();
                        query.Add("");
                        return new StandardValuesCollection(query);
                    }
                }
                var gdj = GenerateHelper.GetFirstXPropOfHasValue(xprops, "CD_SSDW");
                if (gdj!=null)
                {
                    var ssgdj = gdj.Value;
                    if (ssgdj != null && !string.IsNullOrEmpty(ssgdj.ToString()))
                    {
                        var kx = CDDBManager.Instance.GetEntities<Cd_ssxl>().OrderBy(o => o.NAME);
                        var query = kx.Where(o => o.CD_SSDW == ssgdj.ToString()).Select(o => o.NAME).Distinct().ToList();
                        List<string> kxmc = new List<string>();
                        kxmc.AddRange(query);
                        kxmc.Add("");
                        return new StandardValuesCollection(kxmc);
                    }
                }
            }
            return new StandardValuesCollection(null);
        }
        #region
        //public List<string> GetKxmcs(ITypeDescriptorContext context)
        //{
        //    List<string> kxmc = new List<string>();
        //    if (context!=null&&SsgdjList.str != null)
        //    {
        //        IEnumerable<Cd_ssxl> kx = CDDBManager.Instance.GetEntities<Cd_ssxl>().OrderBy(o => o.NAME);
        //        IEnumerable<Cd_ssxl> query = kx.Where(o => o.CD_SSDW == SsgdjList.str);
        //        var kxs = query.Select(o => o.NAME).Distinct().ToList();
        //        var ssxl = GenerateHelper.GetPropertyValue((XProps)context.Instance, "CD_SSXL");
        //        if (context!=null&&ssxl!=null&&!string.IsNullOrEmpty(ssxl.ToString()))
        //        {
        //            bool flag = false;
        //            foreach (var item in kxs)
        //            {
        //                if (item.Equals(ssxl.ToString()))
        //                {
        //                    flag = true;
        //                    break;
        //                }
        //            }
        //            if (!flag)
        //            {
        //                InPut = ssxl.ToString();
        //                kxmc = kxs.Where(o => o.StartsWith(InPut)).ToList();
        //                if (!kxmc.Any())
        //                    kxmc.AddRange(kxs);
        //                else
        //                    GenerateHelper.SetPropertyValue((XProps) context.Instance, "CD_SSXL", kxmc.First());
        //            }
        //            else
        //            {
        //                //二次进来会匹配到
        //                if (string.IsNullOrEmpty(InPut))
        //                    kxmc = kxs;
        //                else
        //                    kxmc = kxs.Where(o => o.StartsWith(InPut)).ToList();
        //            }
        //        }
        //        else
        //        {
        //            kxmc.AddRange(kxs);
        //        }
        //        kxmc.Add("");
        //    }
        //    return kxmc;
        //}
        #endregion

    }
    #region  通过FNO过滤出下拉列表
  ///// <summary>
  //  /// 设备分类
  //  /// </summary>
  //  public class FlList : StringConverter
  //  {
  //      public static int fno { get; set; }
  //      public override bool GetStandardValuesSupported(ITypeDescriptorContext context)
  //      {
  //          return true;
  //      }
  //      public override bool GetStandardValuesExclusive(ITypeDescriptorContext context)
  //      {
  //          return true;
  //      }
  //      public override TypeConverter.StandardValuesCollection GetStandardValues(ITypeDescriptorContext context)
  //      {
  //          var fl = CDDBManager.Instance.GetEntities<gnwz_fl>().OrderBy(o => o.NAME);
  //          var str = fl.Where(o => o.G3E_FNO == fno).Select(o => o.NAME).Distinct().ToList();
  //          str.Add("");
  //          return new StandardValuesCollection(str);
  //      }
  //  }
  //  /// <summary>
  //  /// 安装位置
  //  /// </summary>
  //  public class AzwzList : StringConverter
  //  {
  //      public static int fno { get; set; }
  //      public override bool GetStandardValuesSupported(ITypeDescriptorContext context)
  //      {
  //          return true;
  //      }
  //      public override bool GetStandardValuesExclusive(ITypeDescriptorContext context)
  //      {
  //          return true;
  //      }
  //      public override TypeConverter.StandardValuesCollection GetStandardValues(ITypeDescriptorContext context)
  //      {
  //          IEnumerable<gnwz_fl2> azwz = CDDBManager.Instance.GetEntities<gnwz_fl2>().OrderBy(o => o.NAME);
  //          var azwzList = azwz.Where(o => o.G3E_FNO == fno);
  //          var str = azwzList.Select(o => o.NAME).Distinct().ToList();
  //          str.Add("");
  //          return new StandardValuesCollection(str);
  //      }
  //  }
  //  /// <summary>
  //  /// 型号规格
  //  /// </summary>
  //  public class XhggList : StringConverter
  //  {
  //      public static int fno { get; set; }
  //      public override bool GetStandardValuesSupported(ITypeDescriptorContext context)
  //      {
  //          return true;
  //      }
  //      public override bool GetStandardValuesExclusive(ITypeDescriptorContext context)
  //      {
  //          return true;
  //      }
  //      public override TypeConverter.StandardValuesCollection GetStandardValues(ITypeDescriptorContext context)
  //      {
  //          IEnumerable<Cd_xhge> xhgellst = CDDBManager.Instance.GetEntities<Cd_xhge>().OrderBy(o => o.NAME);
  //          var str = xhgellst.Where(o => o.G3E_FNO == fno).Select(o => o.NAME).Distinct().ToList();
  //          str.Add("");
  //          return new StandardValuesCollection(str);

  //      }
  //  }
    #endregion

    /// <summary>
    /// 额定电流(特殊情况，它的下拉列表可以自己填写)
    /// </summary>
    public class EddlList : StringConverter
    {
        public override bool GetStandardValuesSupported(ITypeDescriptorContext context)
        {
            return true;
        }
        public override bool GetStandardValuesExclusive(ITypeDescriptorContext context)
        {
            return false;
        }
        public override TypeConverter.StandardValuesCollection GetStandardValues(ITypeDescriptorContext context)
        {
            IEnumerable<Cd_eddl> jddz = CDDBManager.Instance.GetEntities<Cd_eddl>().OrderBy(o => o.NAME);
            var str = jddz.Select(o => o.NAME).ToList();
            str.Add("");
            return new StandardValuesCollection(str);
        }
    }
    /// <summary>
    /// 备注字段
    /// </summary>
    public class BZList : StringConverter
    {
        
        public override bool GetStandardValuesSupported(ITypeDescriptorContext context)
        {
            return true;
        }
        public override bool GetStandardValuesExclusive(ITypeDescriptorContext context)
        {
            return false;
        }
        public override TypeConverter.StandardValuesCollection GetStandardValues(ITypeDescriptorContext context)
        {
            return new StandardValuesCollection(new string[]{"无开关柜","无台架","无杆塔","无变压器","无电房","无支线","无台区或台架",""});
        }
    }
    /// <summary>
    /// 通用方法
    /// </summary>
    public class DropDownItem : StringConverter
    {
        private string tableName = string.Empty;
        private int G3e_Fno;
        public DropDownItem(string drowDownListTableName,int fno)
        {
            tableName = drowDownListTableName;
            G3e_Fno = fno;
        }
        public override bool GetStandardValuesSupported(ITypeDescriptorContext context)
        {
            return true;
        }
        public override bool GetStandardValuesExclusive(ITypeDescriptorContext context)
        {
            return true;
        }
        public override TypeConverter.StandardValuesCollection GetStandardValues(ITypeDescriptorContext context)
        {
            List<string> dropDownListItem = new List<string> {};
            //先根据字符串获取表实体
            var dropDownEntity = TypeCache.Instance.GetTypeFromCache(typeof(DBEntity), tableName.Trim());
            if (dropDownEntity!=null)
            {
                var entities = CDDBManager.Instance.GetEntities(dropDownEntity);
                List<DBEntity> entitiesAfterFilter = new List<DBEntity>();
                if (entities!=null&&entities.Any())
                {
                    foreach (var item in entities)
                    {
                        if (item.HasAttribute("G3E_FNO"))
                        {
                            var fno = SurfaceInteractive.GetAttribute(item, "G3E_FNO");
                            if (!string.IsNullOrEmpty(fno) && fno.Equals(G3e_Fno.ToString()))
                            {
                                entitiesAfterFilter.Add(item);
                            }
                        }
                        else
                        {
                            entitiesAfterFilter.Add(item);
                        }
                    }
                    if (entitiesAfterFilter.Any())
                    {
                        foreach (var item in entitiesAfterFilter)
                        {
                            if (item.HasAttribute("NAME"))
                            {
                                var str = SurfaceInteractive.GetAttribute(item, "NAME");
                                if (!string.IsNullOrEmpty(str))
                                    dropDownListItem.Add(str);
                            }
                        }
                    }
                   
                }
            }
            dropDownListItem.Sort();
            dropDownListItem.Add("");
            return new StandardValuesCollection(dropDownListItem);
        }
    }
}
