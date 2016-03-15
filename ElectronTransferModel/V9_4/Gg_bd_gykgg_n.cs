using System;
using ElectronTransferModel.Base;
using ElectronTransferModel.Geo;

namespace ElectronTransferModel.V9_4
{
    [Serializable]
    public class Gg_bd_gykgg_n : ElectronBase
    {
        public System.String CD_XS { get; set; }
        public System.String CSSJ { get; set; }
        [Precision(10)]
        public System.Double? DKJ1 { get; set; }
        [Precision(10)]
        public System.Double? DKJ2 { get; set; }
        [Precision(10)]
        public System.Double? EDDZ { get; set; }
        [Precision(10)]
        public System.Double? EDPL { get; set; }
        public System.String FZHLJYSP { get; set; }
        [Precision(10)]
        public System.Int64? G3E_CID { get; set; }
        [Precision(5)]
        public System.Int32? G3E_CNO { get; set; }
        public System.String GPNY { get; set; }
        [Precision(10)]
        public System.Double? GZDY { get; set; }
        public System.String JYBJ { get; set; }
        public System.String JYDJ { get; set; }
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
        public System.Int64? NSDL { get; set; }
        public System.String NSDYFZ { get; set; }
        public System.String PDBJ { get; set; }
        [Precision(10)]
        public System.Double? RWDDL { get; set; }
        [Precision(10)]
        public System.Double? XDD1 { get; set; }
        [Precision(10)]
        public System.Int64? XDD2 { get; set; }
        public System.String YRDL { get; set; }
        public System.String YRGF { get; set; }
    }
}
