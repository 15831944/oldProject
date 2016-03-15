using System;
using ElectronTransferModel.Base;
using ElectronTransferModel.Geo;

namespace ElectronTransferModel.V9_4
{
    [Serializable]
    public class Gg_sd_jchlx_ac : ElectronBase
    {
        public System.DateTime? CCRQ { get; set; }
        public System.String CD_JCZZCJ { get; set; }
        public System.String CD_SSXL { get; set; }
        public System.String CD_XHGE { get; set; }
        [Precision(10)]
        public System.Int64? G3E_CID { get; set; }
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
        public System.String SBMC { get; set; }
        public System.DateTime? TYRQ { get; set; }
    }
}
