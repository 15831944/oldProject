using System;
using ElectronTransferModel.Base;
using ElectronTransferModel.Geo;

namespace ElectronTransferModel.V9_4
{
    [Serializable]
    public class Gg_sd_sdxl_n : ElectronBase
    {
        public System.String BZ { get; set; }
        public System.String CD_DXGG { get; set; }
        public System.String CD_SZ { get; set; }
        public System.String CD_WZ { get; set; }
        public System.String CD_XCBZ { get; set; }
        public System.String CD_XLDJ { get; set; }
        public System.String CD_XLMC { get; set; }
        public System.String CPCSZQ { get; set; }
        public System.DateTime? DXGGRQ { get; set; }
        [Precision(10)]
        public System.Int64? G3E_CID { get; set; }
        [Precision(5)]
        public System.Int32? G3E_CNO { get; set; }
        public System.String HLQK { get; set; }
        public System.String JDDZCSZQ { get; set; }
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
        public System.String QDWZ { get; set; }
        public System.String XLBH { get; set; }
        [Precision(10)]
        public System.Double? XLCD { get; set; }
        public System.String ZDWZ { get; set; }
    }
}
