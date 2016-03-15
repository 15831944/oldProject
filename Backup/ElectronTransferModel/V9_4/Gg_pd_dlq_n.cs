using System;
using ElectronTransferModel.Base;
using ElectronTransferModel.Geo;

namespace ElectronTransferModel.V9_4
{
    [Serializable]
    public class Gg_pd_dlq_n : ElectronBase
    {
        public System.String CXSJ { get; set; }
        public System.String DLQBM { get; set; }
        public System.String FZSJ { get; set; }
        [Precision(10)]
        public System.Int64? G3E_CID { get; set; }
        [Precision(5)]
        public System.Int32? G3E_CNO { get; set; }
        public System.String GHDL { get; set; }
        [Precision(10)]
        public System.Double? GZDY { get; set; }
        public System.String HZSJ { get; set; }
        public System.String JGXH { get; set; }
        public System.String JXDL { get; set; }
        public System.String KDDL { get; set; }
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
        public System.String MHXS { get; set; }
        public System.String RWDDL { get; set; }
        public System.String SSDG { get; set; }
        public System.String YXQK { get; set; }
    }
}
