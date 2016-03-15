using System;
using ElectronTransferModel.Base;
using ElectronTransferModel.Geo;

namespace ElectronTransferModel.V9_4
{
    [Serializable]
    public class Gg_gz_dgblq_ac : ElectronBase
    {
        public System.String BKDL { get; set; }
        [Precision(10)]
        public System.Double? CJDY { get; set; }
        public System.String CY { get; set; }
        [Precision(10)]
        public System.Double? FDDL { get; set; }
        [Precision(10)]
        public System.Int64? G3E_CID { get; set; }
        [Precision(5)]
        public System.Int32? G3E_CNO { get; set; }
        [Precision(10)]
        public System.Double? GD { get; set; }
        public System.String GGXH { get; set; }
        [Precision(10)]
        public System.Double? GPCKDY { get; set; }
        [Precision(10)]
        public System.Double? GPDY { get; set; }
        [Precision(10)]
        public System.Double? GPNY { get; set; }
        public System.String JDJM { get; set; }
        public System.String LDCY { get; set; }
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
        [Precision(10)]
        public System.Double? XLDL { get; set; }
        public System.String XLPJ { get; set; }
        public System.String XTDY { get; set; }
        [Precision(10)]
        public System.Double? YXDY { get; set; }
        [Precision(10)]
        public System.Double? ZDCY { get; set; }
        [Precision(10)]
        public System.Double? ZJ { get; set; }
        [Precision(10)]
        public System.Double? ZL { get; set; }
        [Precision(10)]
        public System.Double? ZLDY { get; set; }
    }
}
