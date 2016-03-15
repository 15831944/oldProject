using System;
using ElectronTransferModel.Base;
using ElectronTransferModel.Geo;

namespace ElectronTransferModel.V9_4
{
    [Serializable]
    public class Gg_pd_dydl_n : ElectronBase
    {
        [Precision(10)]
        public System.Double? CD { get; set; }
        public System.String DLJM { get; set; }
        public System.String DLMC { get; set; }
        public System.String DTXJ { get; set; }
        public System.String FSFS { get; set; }
        public System.DateTime? FSRQ { get; set; }
        [Precision(10)]
        public System.Int64 G3E_CID { get; set; }
        [Precision(5)]
        public System.Int32 G3E_CNO { get; set; }
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
        public System.String QSDZ { get; set; }
        public System.String SFZMD { get; set; }
        public System.String SSBYQ { get; set; }
        public System.String YXQK { get; set; }
        public System.String ZZDZ { get; set; }
    }
}
