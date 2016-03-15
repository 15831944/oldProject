using System;
using ElectronTransferModel.Base;
using ElectronTransferModel.Geo;

namespace ElectronTransferModel.V9_4
{
    [Serializable]
    public class Gg_gh_bdz_n : ElectronBase
    {
        [Precision(2)]
        public System.Int16? F_BDZHLX { get; set; }
        [Precision(7)]
        public System.Int32? F_DYCJG { get; set; }
        [Precision(7)]
        public System.Int32? F_DYCYYJG { get; set; }
        [Precision(7)]
        public System.Single? F_DYDJ { get; set; }
        [Precision(15)]
        public System.Double? F_EX { get; set; }
        [Precision(15)]
        public System.Double? F_EY { get; set; }
        public System.String F_EZBCH { get; set; }
        public System.String F_FQ { get; set; }
        public System.String F_FQID { get; set; }
        [Precision(2)]
        public System.Int16? F_GHXZH { get; set; }
        [Precision(15)]
        public System.Double? F_GX { get; set; }
        [Precision(15)]
        public System.Double? F_GY { get; set; }
        [Precision(7)]
        public System.Int32? F_GYCJG { get; set; }
        [Precision(7)]
        public System.Int32? F_GYCYYJG { get; set; }
        public System.String F_GZBCH { get; set; }
        public System.String F_ID { get; set; }
        [Precision(1)]
        public System.Int16? F_JGXS { get; set; }
        [Precision(10)]
        public System.Double? F_JJRL { get; set; }
        [Precision(1)]
        public System.Int16? F_JSHXS { get; set; }
        [Precision(10)]
        public System.Double? F_MAXFH { get; set; }
        public System.String F_MCH { get; set; }
        [Precision(10)]
        public System.Double? F_RL { get; set; }
        [Precision(2)]
        public System.Int16? F_SHFCJYH { get; set; }
        [Precision(2)]
        public System.Int16? F_SHFGY { get; set; }
        public System.String F_SSGHBH { get; set; }
        public System.DateTime? F_TYRQ { get; set; }
        [Precision(15)]
        public System.Double? F_TZFY { get; set; }
        [Precision(10)]
        public System.Int64? F_WGBCHRL { get; set; }
        [Precision(1)]
        public System.Int16? F_WRZHBSHL { get; set; }
        public System.String F_YHID { get; set; }
        public System.String F_ZHBRL { get; set; }
        [Precision(10)]
        public System.Double? F_ZHDMJ { get; set; }
        [Precision(7)]
        public System.Int32? F_ZHYCJG { get; set; }
        [Precision(7)]
        public System.Int32? F_ZHYCYYJG { get; set; }
        [Precision(1)]
        public System.Int16? F_ZHZDHSHL { get; set; }
        [Precision(10)]
        public System.Int64 G3E_CID { get; set; }
        [Precision(5)]
        public System.Int32 G3E_CNO { get; set; }
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
    }
}
