using System;
using ElectronTransferModel.Base;
using ElectronTransferModel.Geo;

namespace ElectronTransferModel.V9_4
{
    [Serializable]
    public class Gg_gh_ljx_n : ElectronBase
    {
        public System.String F_BDZHID { get; set; }
        public System.String F_BDZHMCH { get; set; }
        public System.String F_BEGID { get; set; }
        public System.String F_BEGMCH { get; set; }
        [Precision(7)]
        public System.Single? F_DYDJ { get; set; }
        public System.String F_ENDID { get; set; }
        public System.String F_ENDMCH { get; set; }
        public System.String F_EZBCH { get; set; }
        public System.String F_GZBCH { get; set; }
        public System.String F_ID { get; set; }
        public System.String F_MCH { get; set; }
        [Precision(2)]
        public System.Int16? F_SHFYX { get; set; }
        public System.String F_SSGHBH { get; set; }
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
