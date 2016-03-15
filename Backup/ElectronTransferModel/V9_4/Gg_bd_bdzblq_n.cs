using System;
using ElectronTransferModel.Base;
using ElectronTransferModel.Geo;

namespace ElectronTransferModel.V9_4
{
    [Serializable]
    public class Gg_bd_bdzblq_n : ElectronBase
    {
        [Precision(10)]
        public System.Int64? BEDDY { get; set; }
        public System.String CD_BLQLX { get; set; }
        [Precision(10)]
        public System.Double? CJDL { get; set; }
        [Precision(10)]
        public System.Double? CKDY { get; set; }
        [Precision(10)]
        public System.Double? CZCY { get; set; }
        [Precision(10)]
        public System.Double? DBCY { get; set; }
        [Precision(10)]
        public System.Int64? G3E_CID { get; set; }
        [Precision(5)]
        public System.Int32? G3E_CNO { get; set; }
        [Precision(10)]
        public System.Int64? GZDY { get; set; }
        [Precision(10)]
        public System.Double? LDCY { get; set; }
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
        public System.Double? PDJL { get; set; }
        [Precision(10)]
        public System.Double? TLRL { get; set; }
        [Precision(10)]
        public System.Int64? XTDY { get; set; }
    }
}
