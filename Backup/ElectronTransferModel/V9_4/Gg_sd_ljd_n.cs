using System;
using ElectronTransferModel.Base;
using ElectronTransferModel.Geo;

namespace ElectronTransferModel.V9_4
{
    [Serializable]
    public class Gg_sd_ljd_n : ElectronBase
    {
        public System.String CD_SSDW { get; set; }
        [Precision(10)]
        public System.Double? DL { get; set; }
        public System.String DWZ { get; set; }
        [Precision(10)]
        public System.Int64? G3E_CID { get; set; }
        [Precision(5)]
        public System.Int32? G3E_CNO { get; set; }
        public System.String GH { get; set; }
        public System.String HJ { get; set; }
        [Precision(10)]
        public System.Double? JD { get; set; }
        [Precision(10)]
        public System.Double? JL { get; set; }
        public System.DateTime? LJSJ { get; set; }
        [SelectOnly]
        public System.DateTime? LTT_DATE { get; set; }
        [SelectOnly]
        [Precision(29)]
        public System.Decimal? LTT_ID { get; set; }
        [SelectOnly]
        public System.String LTT_STATUS { get; set; }
        [SelectOnly]
        [Precision(29)]
        public System.Decimal? LTT_TID { get; set; }
        [Precision(10)]
        public System.Double? WD { get; set; }
    }
}
