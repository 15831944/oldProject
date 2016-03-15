using System;
using ElectronTransferModel.Base;
using ElectronTransferModel.Geo;

namespace ElectronTransferModel.V9_4
{
    [Serializable]
    public class Gg_bd_bdzdrq_n : ElectronBase
    {
        public System.String CD_BDZDRQJXFS { get; set; }
        public System.String CD_JYYPH { get; set; }
        public System.String CD_SBLX { get; set; }
        public System.String CD_XS { get; set; }
        public System.String CD_ZHFS { get; set; }
        public System.String DRQXH { get; set; }
        [Precision(10)]
        public System.Double? DTDRQ { get; set; }
        [Precision(10)]
        public System.Double? EDPL { get; set; }
        [Precision(10)]
        public System.Double? EDRL { get; set; }
        [Precision(10)]
        public System.Int64? G3E_CID { get; set; }
        [Precision(5)]
        public System.Int32? G3E_CNO { get; set; }
        public System.String JXDLGF { get; set; }
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
        public System.String LXGF { get; set; }
        [Precision(10)]
        public System.Double? RDQEDDL { get; set; }
        [Precision(10)]
        public System.Double? RDQKDDL { get; set; }
        public System.String RDQXH { get; set; }
        public System.String RDQZZC { get; set; }
        [Precision(10)]
        public System.Double? WPDBJ { get; set; }
    }
}
