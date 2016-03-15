using System;
using ElectronTransferModel.Base;
using ElectronTransferModel.Geo;

namespace ElectronTransferModel.V9_4
{
    [Serializable]
    public class Detailreference_n_t112 : ElectronBase
    {
        [SelectOnly]
        [Precision(10)]
        public System.Int64? DETAIL_LEGENDNUMBER { get; set; }
        [SelectOnly]
        [Precision(29)]
        public System.Decimal? DETAIL_MBRXHIGH { get; set; }
        [SelectOnly]
        [Precision(29)]
        public System.Decimal? DETAIL_MBRXLO { get; set; }
        [SelectOnly]
        [Precision(29)]
        public System.Decimal? DETAIL_MBRXOFFSET { get; set; }
        [SelectOnly]
        [Precision(29)]
        public System.Decimal? DETAIL_MBRYHIGH { get; set; }
        [SelectOnly]
        [Precision(29)]
        public System.Decimal? DETAIL_MBRYLO { get; set; }
        [SelectOnly]
        [Precision(29)]
        public System.Decimal? DETAIL_MBRYOFFSET { get; set; }
        [SelectOnly]
        public System.String DETAIL_USERNAME { get; set; }
        [SelectOnly]
        [Precision(5)]
        public System.Int32 G3E_CID { get; set; }
        [SelectOnly]
        [Precision(5)]
        public System.Int32 G3E_CNO { get; set; }
        [SelectOnly]
        [Precision(10)]
        public System.Int64? G3E_DETAILID { get; set; }
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
        [SelectOnly]
        [Precision(10)]
        public System.Int64? SCM_DID { get; set; }
    }
}
