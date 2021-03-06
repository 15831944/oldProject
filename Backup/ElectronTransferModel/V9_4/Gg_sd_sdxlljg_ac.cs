using System;
using ElectronTransferModel.Base;
using ElectronTransferModel.Geo;

namespace ElectronTransferModel.V9_4
{
    [Serializable]
    public class Gg_sd_sdxlljg_ac : ElectronBase
    {
        public System.String CD_CZT { get; set; }
        public System.String CD_LJGXB { get; set; }
        public System.String CD_LJGXS { get; set; }
        public System.String CD_SZX { get; set; }
        public System.String DHT { get; set; }
        [Precision(10)]
        public System.Int64? G3E_CID { get; set; }
        [Precision(5)]
        public System.Int32? G3E_CNO { get; set; }
        [Precision(10)]
        public System.Double? JL { get; set; }
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
        public System.Int64? SL { get; set; }
        public System.String XHT { get; set; }
    }
}
