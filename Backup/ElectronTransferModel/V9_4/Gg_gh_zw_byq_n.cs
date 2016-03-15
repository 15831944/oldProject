using System;
using ElectronTransferModel.Base;
using ElectronTransferModel.Geo;

namespace ElectronTransferModel.V9_4
{
    [Serializable]
    public class Gg_gh_zw_byq_n : ElectronBase
    {
        [Precision(7)]
        public System.Int32? CD_DYDJ { get; set; }
        public System.String CD_SMZQ { get; set; }
        public System.String CD_SSBDZ { get; set; }
        public System.String CD_SSDW { get; set; }
        public System.String CD_XHGE { get; set; }
        public System.String F_BDZHID { get; set; }
        public System.String F_BDZHMCH { get; set; }
        [Precision(10)]
        public System.Int64? F_DKQ { get; set; }
        [Precision(10)]
        public System.Int64? F_EX { get; set; }
        [Precision(10)]
        public System.Int64? F_EY { get; set; }
        public System.String F_EZBCH { get; set; }
        [Precision(10)]
        public System.Int64? F_GX { get; set; }
        [Precision(10)]
        public System.Int64? F_GY { get; set; }
        public System.String F_GZBCH { get; set; }
        [Precision(7)]
        public System.Int32? F_HEDDY { get; set; }
        [Precision(5)]
        public System.Int32? F_HFJT { get; set; }
        public System.String F_HMXID { get; set; }
        public System.String F_HMXMCH { get; set; }
        [Precision(2)]
        public System.Int16? F_HZHT { get; set; }
        public System.String F_ID { get; set; }
        [Precision(10)]
        public System.Int64? F_KGZDRL { get; set; }
        [Precision(7)]
        public System.Int32? F_LDYDJ { get; set; }
        [Precision(7)]
        public System.Int32? F_LEDDY { get; set; }
        [Precision(10)]
        public System.Int64? F_LKGZDRL { get; set; }
        public System.String F_LMXID { get; set; }
        public System.String F_LMXMCH { get; set; }
        [Precision(10)]
        public System.Int64? F_LRL { get; set; }
        [Precision(2)]
        public System.Int16? F_LZHT { get; set; }
        public System.String F_MCH { get; set; }
        [Precision(7)]
        public System.Int32? F_MDYDJ { get; set; }
        [Precision(7)]
        public System.Int32? F_MEDDY { get; set; }
        [Precision(5)]
        public System.Int32? F_MFJT { get; set; }
        [Precision(10)]
        public System.Int64? F_MKGZDRL { get; set; }
        public System.String F_MMXID { get; set; }
        public System.String F_MMXMCH { get; set; }
        [Precision(10)]
        public System.Int64? F_MRL { get; set; }
        [Precision(2)]
        public System.Int16? F_MZHT { get; set; }
        [Precision(10)]
        public System.Int64? F_RL { get; set; }
        [Precision(2)]
        public System.Int16? F_SHFYZTY { get; set; }
        public System.String F_TYPE { get; set; }
        public System.String F_WGPZH { get; set; }
        [Precision(10)]
        public System.Int64? F_WGRL { get; set; }
        public System.String F_YHID { get; set; }
        [Precision(10)]
        public System.Int64 G3E_CID { get; set; }
        [Precision(5)]
        public System.Int32 G3E_CNO { get; set; }
        public System.String GHBH { get; set; }
        public System.String GHXZ { get; set; }
        [SelectOnly]
        public System.DateTime? LTT_DATE { get; set; }
        [SelectOnly]
        [Precision(29)]
        public System.Decimal LTT_ID { get; set; }
        [SelectOnly]
        public System.String LTT_STATUS { get; set; }
        [SelectOnly]
        [Precision(29)]
        public System.Decimal? LTT_TID { get; set; }
        public System.String SBLX { get; set; }
        public System.DateTime? TYRQ { get; set; }
    }
}
