using System;
using ElectronTransferModel.Base;
using ElectronTransferModel.Geo;

namespace ElectronTransferModel.V9_4
{
    [Serializable]
    public class Gg_gh_znml_n : ElectronBase
    {
        public System.String F_BDZHID { get; set; }
        public System.String F_BDZHMCH { get; set; }
        public System.String F_BEGID { get; set; }
        [Precision(7)]
        public System.Int32? F_DYDJ { get; set; }
        public System.String F_ENDID { get; set; }
        public System.String F_ID { get; set; }
        [Precision(1)]
        public System.Int16? F_JZLX { get; set; }
        public System.String F_MCH { get; set; }
        public System.String F_SSGHBH { get; set; }
        public System.DateTime? F_TYRQ { get; set; }
        public System.String F_YHID { get; set; }
        [Precision(1)]
        public System.Int16? F_ZHT { get; set; }
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
