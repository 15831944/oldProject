using System;
using ElectronTransferModel.Base;
using ElectronTransferModel.Geo;

namespace ElectronTransferModel.V9_4
{
    [Serializable]
    public class Gg_jc_fk_n : ElectronBase
    {
        public System.String BZ { get; set; }
        public System.String CD_SSDW { get; set; }
        [Precision(29)]
        public System.Decimal? DLJJZ { get; set; }
        public System.String DYDJ { get; set; }
        [Precision(29)]
        public System.Decimal? DYJJZ { get; set; }
        [Precision(10)]
        public System.Int64? G3E_CID { get; set; }
        [Precision(5)]
        public System.Int32? G3E_CNO { get; set; }
        public System.String GDDYLS { get; set; }
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
        [Precision(29)]
        public System.Decimal? PJDL { get; set; }
        [Precision(29)]
        public System.Decimal? PJDY { get; set; }
        public System.String YDXZ { get; set; }
        public System.String YHDJ { get; set; }
        public System.String YHDWDZ { get; set; }
        public System.String YHH { get; set; }
        [Precision(29)]
        public System.Decimal? ZDDL { get; set; }
        [Precision(29)]
        public System.Decimal? ZDDY { get; set; }
        [Precision(29)]
        public System.Decimal? ZXDL { get; set; }
        [Precision(29)]
        public System.Decimal? ZXDY { get; set; }
    }
}
