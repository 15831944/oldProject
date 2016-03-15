using System;
using ElectronTransferModel.Base;
using ElectronTransferModel.Geo;

namespace ElectronTransferModel.V9_4
{
    [Serializable]
    public class Gg_jc_ftu_n : ElectronBase
    {
        public System.String BH { get; set; }
        public System.String CD_FHKG { get; set; }
        public System.String CD_GZXH { get; set; }
        public System.String CD_JDDZ { get; set; }
        public System.String CWBJ { get; set; }
        [Precision(10)]
        public System.Int64? G3E_CID { get; set; }
        [Precision(5)]
        public System.Int32? G3E_CNO { get; set; }
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
        public System.String MC { get; set; }
        public System.String RSRD { get; set; }
        [Precision(10)]
        public System.Int64? SCM_DID { get; set; }
        public System.String SXDL { get; set; }
        public System.String SXDY { get; set; }
        public System.String TXFS { get; set; }
        public System.String TXSBCJ { get; set; }
        public System.String TXSBLX { get; set; }
        public System.String TXSBXH { get; set; }
        public System.String XH { get; set; }
    }
}
