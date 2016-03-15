using System;
using ElectronTransferModel.Base;
using ElectronTransferModel.Geo;

namespace ElectronTransferModel.V9_4
{
    [Serializable]
    public class Gg_pd_zydltfg_ac : ElectronBase
    {
        public System.String CD_DFFL { get; set; }
        public System.String DFMC { get; set; }
        [Precision(10)]
        public System.Int64? G3E_CID { get; set; }
        [Precision(5)]
        public System.Int32? G3E_CNO { get; set; }
        public System.String GYCS { get; set; }
        public System.String HWGBM { get; set; }
        public System.String HWGMC { get; set; }
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
        public System.String MHXS { get; set; }
        [Precision(10)]
        public System.Int64? SCM_DID { get; set; }
        public System.String YXQK { get; set; }
    }
}
