using System;
using ElectronTransferModel.Base;
using ElectronTransferModel.Geo;

namespace ElectronTransferModel.V9_4
{
    [Serializable]
    public class Gg_gh_xlxd_n : ElectronBase
    {
        [Precision(6)]
        public System.Single? F_ALLLEN { get; set; }
        [Precision(10)]
        public System.Double? F_B { get; set; }
        public System.String F_BEGID { get; set; }
        [Precision(10)]
        public System.Double? F_BEGKGZDRL { get; set; }
        public System.String F_BEGMCH { get; set; }
        [Precision(7)]
        public System.Single? F_DYDJ { get; set; }
        public System.String F_ENDID { get; set; }
        [Precision(10)]
        public System.Double? F_ENDKGZDRL { get; set; }
        public System.String F_ENDMCH { get; set; }
        public System.String F_EZBCH { get; set; }
        public System.String F_FQ { get; set; }
        public System.String F_FQID { get; set; }
        [Precision(10)]
        public System.Double? F_G { get; set; }
        [Precision(2)]
        public System.Int16? F_GHXZH { get; set; }
        public System.String F_GZBCH { get; set; }
        public System.String F_ID { get; set; }
        [Precision(6)]
        public System.Single? F_LEN { get; set; }
        [Precision(10)]
        public System.Double? F_LEN2 { get; set; }
        [Precision(10)]
        public System.Double? F_LEN3 { get; set; }
        public System.String F_LTYPE { get; set; }
        public System.String F_LTYPE2 { get; set; }
        public System.String F_LTYPE3 { get; set; }
        public System.String F_MCH { get; set; }
        [Precision(10)]
        public System.Double? F_R { get; set; }
        [Precision(2)]
        public System.Int16? F_SHFDL { get; set; }
        [Precision(2)]
        public System.Int16? F_SHFDX { get; set; }
        [Precision(2)]
        public System.Int16? F_SHFYX { get; set; }
        public System.String F_SSGHBH { get; set; }
        public System.DateTime? F_TYRQ { get; set; }
        [Precision(10)]
        public System.Double? F_X { get; set; }
        public System.String F_XLLX { get; set; }
        public System.String F_YHID { get; set; }
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
