using System;
using ElectronTransferModel.Base;
using ElectronTransferModel.Geo;

namespace ElectronTransferModel.V9_4
{
    [Serializable]
    public class Gg_yxt_rs : ElectronBase
    {
        [Precision(10)]
        public System.Int64 G3E_CID { get; set; }
        [Precision(5)]
        public System.Int32 G3E_CNO { get; set; }
        public System.String G3E_GEOMETRY { get; set; }
        [Precision(29)]
        public System.Decimal? GDO_GID { get; set; }
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
        [Precision(29)]
        public System.Decimal? RASTERMATRIX1 { get; set; }
        [Precision(29)]
        public System.Decimal? RASTERMATRIX10 { get; set; }
        [Precision(29)]
        public System.Decimal? RASTERMATRIX11 { get; set; }
        [Precision(29)]
        public System.Decimal? RASTERMATRIX12 { get; set; }
        [Precision(29)]
        public System.Decimal? RASTERMATRIX13 { get; set; }
        [Precision(29)]
        public System.Decimal? RASTERMATRIX14 { get; set; }
        [Precision(29)]
        public System.Decimal? RASTERMATRIX15 { get; set; }
        [Precision(29)]
        public System.Decimal? RASTERMATRIX16 { get; set; }
        [Precision(29)]
        public System.Decimal? RASTERMATRIX2 { get; set; }
        [Precision(29)]
        public System.Decimal? RASTERMATRIX3 { get; set; }
        [Precision(29)]
        public System.Decimal? RASTERMATRIX4 { get; set; }
        [Precision(29)]
        public System.Decimal? RASTERMATRIX5 { get; set; }
        [Precision(29)]
        public System.Decimal? RASTERMATRIX6 { get; set; }
        [Precision(29)]
        public System.Decimal? RASTERMATRIX7 { get; set; }
        [Precision(29)]
        public System.Decimal? RASTERMATRIX8 { get; set; }
        [Precision(29)]
        public System.Decimal? RASTERMATRIX9 { get; set; }
        public System.String RASTERNAME { get; set; }
    }
}
