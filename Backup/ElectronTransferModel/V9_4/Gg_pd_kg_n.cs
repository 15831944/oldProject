using System;
using ElectronTransferModel.Base;
using ElectronTransferModel.Geo;

namespace ElectronTransferModel.V9_4
{
    [Serializable]
    public class Gg_pd_kg_n : ElectronBase
    {
        [Precision(10)]
        public System.Int64? CFRL { get; set; }
        public System.String CJDY { get; set; }
        [Precision(10)]
        public System.Double? DKDL { get; set; }
        [Precision(10)]
        public System.Double? DLRL { get; set; }
        public System.String FZSJ { get; set; }
        [Precision(10)]
        public System.Int64? G3E_CID { get; set; }
        [Precision(5)]
        public System.Int32? G3E_CNO { get; set; }
        public System.String GPNY { get; set; }
        [Precision(10)]
        public System.Double? GZDY { get; set; }
        public System.String HLDZ { get; set; }
        public System.String HZSJ { get; set; }
        public System.String KGBH { get; set; }
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
        public System.String RLDW { get; set; }
        public System.String SBBH { get; set; }
        public System.String TDLB { get; set; }
        [Precision(10)]
        public System.Double? WDDL { get; set; }
        public System.String XS { get; set; }
        public System.String YXQK { get; set; }
        [Precision(10)]
        public System.Double? ZXDL { get; set; }
    }
}
