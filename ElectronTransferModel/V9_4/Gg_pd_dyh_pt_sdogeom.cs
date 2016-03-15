using System;
using ElectronTransferModel.Base;
using ElectronTransferModel.Geo;

namespace ElectronTransferModel.V9_4
{
    [Serializable]
    public class Gg_pd_dyh_pt_sdogeom : ElectronSymbol
    {
        public System.Int32? G3E_CNO { get; set; }
        public System.Int64? G3E_CID { get; set; }
        public System.Decimal? SDO_GID { get; set; }
        public System.Decimal? LTT_ID { get; set; }
        public System.String LTT_STATUS { get; set; }
        public System.DateTime? LTT_DATE { get; set; }
        public System.Decimal? LTT_TID { get; set; }
    }
}
