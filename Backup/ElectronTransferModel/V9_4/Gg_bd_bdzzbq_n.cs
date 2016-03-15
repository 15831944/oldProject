using System;
using ElectronTransferModel.Base;
using ElectronTransferModel.Geo;

namespace ElectronTransferModel.V9_4
{
    [Serializable]
    public class Gg_bd_bdzzbq_n : ElectronBase
    {
        [Precision(10)]
        public System.Double? BLQDY { get; set; }
        [Precision(10)]
        public System.Double? BLQZL { get; set; }
        [Precision(10)]
        public System.Double? CXDL { get; set; }
        [Precision(10)]
        public System.Double? DRQDR { get; set; }
        [Precision(10)]
        public System.Double? DRQDTZL { get; set; }
        [Precision(10)]
        public System.Double? EDDG { get; set; }
        [Precision(10)]
        public System.Double? EDDLFZ { get; set; }
        [Precision(10)]
        public System.Double? EDDLSJ { get; set; }
        [Precision(10)]
        public System.Double? EDPL { get; set; }
        [Precision(10)]
        public System.Double? EDRL { get; set; }
        [Precision(10)]
        public System.Double? EDRWDDL { get; set; }
        [Precision(10)]
        public System.Int64? G3E_CID { get; set; }
        [Precision(5)]
        public System.Int32? G3E_CNO { get; set; }
        public System.String JYDJ { get; set; }
        public System.String JYSP { get; set; }
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
        public System.Double? XGSLL { get; set; }
        [Precision(10)]
        public System.Double? ZL { get; set; }
        public System.String ZXPLFW { get; set; }
        public System.String ZXZSDZ { get; set; }
        public System.String ZXZSZK { get; set; }
    }
}
