using System;
using ElectronTransferModel.Base;
using ElectronTransferModel.Geo;

namespace ElectronTransferModel.V9_4
{
    [Serializable]
    public class Gg_pd_sdkx_ac_0322 : ElectronBase
    {
        [SelectOnly]
        [Precision(10)]
        public System.Int64 G3E_CID { get; set; }
        [SelectOnly]
        [Precision(5)]
        public System.Int32 G3E_CNO { get; set; }
        [SelectOnly]
        public System.String GDBDZ { get; set; }
        [SelectOnly]
        public System.String KXH { get; set; }
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
        [SelectOnly]
        [Precision(10)]
        public System.Int64? SCM_DID { get; set; }
    }
}
