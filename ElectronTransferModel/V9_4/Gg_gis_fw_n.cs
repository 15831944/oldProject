using System;
using ElectronTransferModel.Base;
using ElectronTransferModel.Geo;

namespace ElectronTransferModel.V9_4
{
    [Serializable]
    public class Gg_gis_fw_n : ElectronBase
    {
        [Precision(10)]
        public System.Int64? FW_AREA { get; set; }
        public System.String FW_AZWZ { get; set; }
        public System.String FW_BDZMC { get; set; }
        public System.String FW_CANAD { get; set; }
        public System.String FW_DYDJ { get; set; }
        public System.String FW_DZ { get; set; }
        public System.DateTime? FW_FINDATE { get; set; }
        public System.String FW_FLOORS { get; set; }
        public System.String FW_ID { get; set; }
        public System.String FW_SSQY { get; set; }
        public System.String FW_TYPE { get; set; }
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
    }
}
