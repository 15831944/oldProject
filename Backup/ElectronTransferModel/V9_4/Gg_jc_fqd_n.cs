using System;
using ElectronTransferModel.Base;
using ElectronTransferModel.Geo;

namespace ElectronTransferModel.V9_4
{
    [Serializable]
    public class Gg_jc_fqd_n : ElectronBase
    {
        public System.String AZDD { get; set; }
        public System.String BZ { get; set; }
        public System.String CD_SSDW { get; set; }
        [Precision(29)]
        public System.Decimal? DLJJZ { get; set; }
        [Precision(29)]
        public System.Decimal? DYJJZ { get; set; }
        [Precision(10)]
        public System.Int64? G3E_CID { get; set; }
        [Precision(5)]
        public System.Int32? G3E_CNO { get; set; }
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
        public System.String LX { get; set; }
        [Precision(29)]
        public System.Decimal? PJDL { get; set; }
        [Precision(29)]
        public System.Decimal? PJDY { get; set; }
        [Precision(29)]
        public System.Decimal? PJWGDL { get; set; }
        [Precision(29)]
        public System.Decimal? PJYGDL { get; set; }
        public System.String SBBH { get; set; }
        [Precision(29)]
        public System.Decimal? WGDLJJZ { get; set; }
        [Precision(29)]
        public System.Decimal? YGDLJJZ { get; set; }
        public System.String YHH { get; set; }
        public System.String YHM { get; set; }
        [Precision(29)]
        public System.Decimal? ZDDL { get; set; }
        [Precision(29)]
        public System.Decimal? ZDDY { get; set; }
        [Precision(29)]
        public System.Decimal? ZDWGDL { get; set; }
        [Precision(29)]
        public System.Decimal? ZDYGDL { get; set; }
        [Precision(29)]
        public System.Decimal? ZXDL { get; set; }
        [Precision(29)]
        public System.Decimal? ZXDY { get; set; }
        [Precision(29)]
        public System.Decimal? ZXWGDL { get; set; }
        [Precision(29)]
        public System.Decimal? ZXYGDL { get; set; }
    }
}
