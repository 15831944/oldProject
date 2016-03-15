using System;
using ElectronTransferModel.Base;
using ElectronTransferModel.Geo;

namespace ElectronTransferModel.V9_4
{
    [Serializable]
    public class Gg_gl_fasjqy_n : ElectronBase
    {
        public System.String BZ { get; set; }
        public System.String FAH { get; set; }
        public System.String FAMC { get; set; }
        public System.String FANR { get; set; }
        [Precision(10)]
        public System.Int64? G3E_CID { get; set; }
        [Precision(5)]
        public System.Int32? G3E_CNO { get; set; }
        [Precision(29)]
        public System.Decimal? GDO_GID { get; set; }
        public System.String JSRY { get; set; }
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
        public System.DateTime? SJRQ { get; set; }
        public System.String SJZG { get; set; }
        public System.String ZT { get; set; }
    }
}
