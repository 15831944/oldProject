using System;
using ElectronTransferModel.Base;
using ElectronTransferModel.Geo;

namespace ElectronTransferModel.V9_4
{
    [Serializable]
    public class Gg_pd_zfmx_lb_sdogeom1 : ElectronBase
    {
        [SelectOnly]
        [Precision(10)]
        public System.Int64? G3E_CID { get; set; }
        [SelectOnly]
        [Precision(5)]
        public System.Int32? G3E_CNO { get; set; }
        [SelectOnly]
        [Precision(29)]
        public System.Decimal? GDO_ATTRIBUTES { get; set; }
        [SelectOnly]
        [Precision(29)]
        public System.Decimal? GDO_NORMAL1 { get; set; }
        [SelectOnly]
        [Precision(29)]
        public System.Decimal? GDO_NORMAL2 { get; set; }
        [SelectOnly]
        [Precision(29)]
        public System.Decimal? GDO_NORMAL3 { get; set; }
        [SelectOnly]
        [Precision(29)]
        public System.Decimal? GDO_RADIUS { get; set; }
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
        [Precision(29)]
        public System.Decimal? SDO_ESEQ { get; set; }
        [SelectOnly]
        [Precision(29)]
        public System.Decimal? SDO_ETYPE { get; set; }
        [SelectOnly]
        [Precision(29)]
        public System.Decimal? SDO_GID { get; set; }
        [SelectOnly]
        [Precision(29)]
        public System.Decimal? SDO_ORIENTATION { get; set; }
        [SelectOnly]
        [Precision(29)]
        public System.Decimal? SDO_SEQ { get; set; }
        [SelectOnly]
        [Precision(29)]
        public System.Decimal? SDO_X1 { get; set; }
        [SelectOnly]
        [Precision(29)]
        public System.Decimal? SDO_X2 { get; set; }
        [SelectOnly]
        [Precision(29)]
        public System.Decimal? SDO_X3 { get; set; }
        [SelectOnly]
        [Precision(29)]
        public System.Decimal? SDO_X4 { get; set; }
        [SelectOnly]
        [Precision(29)]
        public System.Decimal? SDO_Y1 { get; set; }
        [SelectOnly]
        [Precision(29)]
        public System.Decimal? SDO_Y2 { get; set; }
        [SelectOnly]
        [Precision(29)]
        public System.Decimal? SDO_Y3 { get; set; }
        [SelectOnly]
        [Precision(29)]
        public System.Decimal? SDO_Y4 { get; set; }
    }
}
