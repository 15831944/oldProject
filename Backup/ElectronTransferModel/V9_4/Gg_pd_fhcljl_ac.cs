using System;
using ElectronTransferModel.Base;
using ElectronTransferModel.Geo;

namespace ElectronTransferModel.V9_4
{
    [Serializable]
    public class Gg_pd_fhcljl_ac : ElectronBase
    {
        public System.String AXDL { get; set; }
        public System.String AXDY { get; set; }
        public System.String BPHL { get; set; }
        public System.String BXDL { get; set; }
        public System.String BXDY { get; set; }
        public System.String CLR { get; set; }
        public System.DateTime? CLSJ { get; set; }
        public System.String CXDL { get; set; }
        public System.String CXDY { get; set; }
        public System.String CXMC { get; set; }
        public System.String FZL { get; set; }
        [Precision(10)]
        public System.Int64? G3E_CID { get; set; }
        [Precision(5)]
        public System.Int32? G3E_CNO { get; set; }
        public System.String KGXH { get; set; }
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
        public System.String PBMC { get; set; }
        public System.String QW { get; set; }
        [Precision(10)]
        public System.Int64? SCM_DID { get; set; }
        public System.String YXQK { get; set; }
    }
}
