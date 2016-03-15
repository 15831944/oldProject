using System;
using ElectronTransferModel.Base;
using ElectronTransferModel.Geo;

namespace ElectronTransferModel.V9_4
{
    [Serializable]
    public class Gg_jx_gyb_pt_sdogeom : ElectronSymbol
    {
        [Precision(10)]
        public System.Int64? G3E_CID { get; set; }
        [Precision(10)]
        public System.Int32? G3E_CNO { get; set; }
        [SelectOnly]
        public System.DateTime? LTT_DATE { get; set; }
        [SelectOnly]
        [Precision(38)]
        public System.Decimal? LTT_ID { get; set; }
        [SelectOnly]
        public System.String LTT_STATUS { get; set; }
        [SelectOnly]
        [Precision(38)]
        public System.Decimal? LTT_TID { get; set; }
        [Precision(38)]
        public System.Decimal? SDO_GID { get; set; }
    }
}
