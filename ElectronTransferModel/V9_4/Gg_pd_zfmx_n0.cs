using System;
using ElectronTransferModel.Base;
using ElectronTransferModel.Geo;

namespace ElectronTransferModel.V9_4
{
    [Serializable]
    public class Gg_pd_zfmx_n0 : ElectronBase
    {
        [SelectOnly]
        public System.String FZDL { get; set; }
        [SelectOnly]
        [Precision(10)]
        public System.Int64? G3E_CID { get; set; }
        [SelectOnly]
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
        [SelectOnly]
        [Precision(10)]
        public System.Double? MXCD { get; set; }
        [SelectOnly]
        public System.String MXCL { get; set; }
        [SelectOnly]
        [Precision(10)]
        public System.Double? MXJMJ { get; set; }
        [SelectOnly]
        public System.String WDDL { get; set; }
        [SelectOnly]
        public System.String YXQK { get; set; }
    }
}
