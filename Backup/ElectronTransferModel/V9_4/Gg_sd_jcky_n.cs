using System;
using ElectronTransferModel.Base;
using ElectronTransferModel.Geo;

namespace ElectronTransferModel.V9_4
{
    [Serializable]
    public class Gg_sd_jcky_n : ElectronBase
    {
        public System.String CD_BKWMC { get; set; }
        public System.String CD_BKWXZ { get; set; }
        public System.String CD_SSBM { get; set; }
        public System.String CD_SSDW { get; set; }
        public System.String CD_XLMC { get; set; }
        public System.String CZT { get; set; }
        public System.String DHT { get; set; }
        [Precision(10)]
        public System.Int64? FID_BKYSB { get; set; }
        [Precision(10)]
        public System.Int64? G3E_CID { get; set; }
        [Precision(5)]
        public System.Int32? G3E_CNO { get; set; }
        public System.String JCJD { get; set; }
        public System.String JL { get; set; }
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
        public System.String XHT { get; set; }
        public System.String XYCZJL { get; set; }
        public System.String XYQK { get; set; }
        public System.String YQCZJL { get; set; }
    }
}
