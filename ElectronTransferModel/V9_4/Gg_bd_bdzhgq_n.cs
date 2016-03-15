using System;
using ElectronTransferModel.Base;
using ElectronTransferModel.Geo;

namespace ElectronTransferModel.V9_4
{
    [Serializable]
    public class Gg_bd_bdzhgq_n : ElectronBase
    {
        public System.String BAXS { get; set; }
        public System.String BBFW { get; set; }
        [Precision(10)]
        public System.Double? BJYL { get; set; }
        public System.String CD_DLHGQLX { get; set; }
        public System.String CD_JYYPH { get; set; }
        public System.String CD_XS { get; set; }
        public System.String CJNY { get; set; }
        public System.String DLSJ { get; set; }
        public System.String DWDL { get; set; }
        [Precision(10)]
        public System.Double? EDPL { get; set; }
        public System.String EDRL { get; set; }
        [Precision(10)]
        public System.Double? EDYL { get; set; }
        public System.String FZNL { get; set; }
        [Precision(10)]
        public System.Int64? G3E_CID { get; set; }
        [Precision(5)]
        public System.Int32? G3E_CNO { get; set; }
        public System.String GSBH { get; set; }
        public System.String GSMC { get; set; }
        public System.String GZDY { get; set; }
        public System.String GZXH { get; set; }
        public System.String HLNY { get; set; }
        public System.String JSDYBH { get; set; }
        [Precision(10)]
        public System.Double? JYYZ { get; set; }
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
        public System.String NSDY { get; set; }
        public System.String PDBJ { get; set; }
        public System.String WDDL { get; set; }
        public System.String XYBB { get; set; }
        [Precision(10)]
        public System.Double? ZL { get; set; }
        public System.String ZQJ { get; set; }
    }
}
