using System;
using ElectronTransferModel.Base;
using ElectronTransferModel.Geo;

namespace ElectronTransferModel.V9_4
{
    [Serializable]
    public class Gg_pd_zfjddz_n : ElectronBase
    {
        public System.String AZDD { get; set; }
        public System.String CD_GXDW { get; set; }
        public System.String CD_ZCGS { get; set; }
        [Precision(10)]
        public System.Int64? G3E_CID { get; set; }
        [Precision(5)]
        public System.Int32? G3E_CNO { get; set; }
        public System.String GYCS { get; set; }
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
        public System.DateTime? SCRQ { get; set; }
        public System.String SGDW { get; set; }
        public System.String YXQK { get; set; }
    }
}
