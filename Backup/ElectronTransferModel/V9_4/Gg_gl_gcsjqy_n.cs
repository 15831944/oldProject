using System;
using ElectronTransferModel.Base;
using ElectronTransferModel.Geo;

namespace ElectronTransferModel.V9_4
{
    [Serializable]
    public class Gg_gl_gcsjqy_n : ElectronBase
    {
        [Precision(10)]
        public System.Int64? G3E_CID { get; set; }
        [Precision(5)]
        public System.Int32? G3E_CNO { get; set; }
        public System.String GCBH { get; set; }
        public System.String GCMC { get; set; }
        public System.String GCSPH { get; set; }
        [Precision(29)]
        public System.Decimal? GDO_GID { get; set; }
        public System.String HTCBS { get; set; }
        public System.String HTH { get; set; }
        public System.String KX { get; set; }
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
        public System.String SJDW { get; set; }
        public System.String SJR { get; set; }
        public System.DateTime? SPRQ { get; set; }
        public System.DateTime? YSRQ { get; set; }
    }
}
