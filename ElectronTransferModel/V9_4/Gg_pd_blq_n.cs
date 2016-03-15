using System;
using ElectronTransferModel.Base;
using ElectronTransferModel.Geo;

namespace ElectronTransferModel.V9_4
{
    [Serializable]
    public class Gg_pd_blq_n : ElectronBase
    {
        public System.String AZWZ { get; set; }
        public System.String BCDL { get; set; }
        public System.String BKDL { get; set; }
        public System.String CJDY { get; set; }
        public System.String CXDY { get; set; }
        public System.String CY { get; set; }
        public System.String FDDY { get; set; }
        [Precision(10)]
        public System.Int64 G3E_CID { get; set; }
        [Precision(5)]
        public System.Int32 G3E_CNO { get; set; }
        public System.String GPDY { get; set; }
        public System.String GPNY { get; set; }
        public System.String JDJM { get; set; }
        public System.String LDDY { get; set; }
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
        public System.String SZWZ { get; set; }
        public System.String XLDL { get; set; }
        public System.String XLPJ { get; set; }
        public System.String XTDY { get; set; }
        public System.String ZDCY { get; set; }
        public System.String ZLDY { get; set; }
    }
}
