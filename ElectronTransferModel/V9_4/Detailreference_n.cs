using System;
using ElectronTransferModel.Base;
using ElectronTransferModel.Geo;

namespace ElectronTransferModel.V9_4
{
    [Serializable]
    public class Detailreference_n : ElectronBase
    {
        [Precision(10)]
        public System.Int64? DETAIL_LEGENDNUMBER { get; set; }
        [Precision(29)]
        public System.Decimal? DETAIL_MBRXHIGH { get; set; }
        [Precision(29)]
        public System.Decimal? DETAIL_MBRXLO { get; set; }
        [Precision(29)]
        public System.Decimal? DETAIL_MBRXOFFSET { get; set; }
        [Precision(29)]
        public System.Decimal? DETAIL_MBRYHIGH { get; set; }
        [Precision(29)]
        public System.Decimal? DETAIL_MBRYLO { get; set; }
        [Precision(29)]
        public System.Decimal? DETAIL_MBRYOFFSET { get; set; }
        public System.String DETAIL_USERNAME { get; set; }
        [Precision(5)]
        public System.Int32 G3E_CID { get; set; }
        [Precision(5)]
        public System.Int32 G3E_CNO { get; set; }
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
        [Precision(10)]
        public System.Int64? SCM_DID { get; set; }
    }
}
