using System;
using ElectronTransferModel.Base;
using ElectronTransferModel.Geo;

namespace ElectronTransferModel.V9_4
{
    [Serializable]
    public class Gg_jx_dyx_n : ElectronBase
    {
        [Precision(10)]
        public System.Double? DXCD { get; set; }
        public System.String DXCZ { get; set; }
        public System.String DXDDL { get; set; }
        public System.String DXGS { get; set; }
        [Precision(10)]
        public System.Double? DXJMJ { get; set; }
        [Precision(10)]
        public System.Double? DXZJ { get; set; }
        [Precision(10)]
        public System.Int64? G3E_CID { get; set; }
        [Precision(5)]
        public System.Int32? G3E_CNO { get; set; }
        public System.String JKGD { get; set; }
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
        public System.String YXQK { get; set; }
    }
}
