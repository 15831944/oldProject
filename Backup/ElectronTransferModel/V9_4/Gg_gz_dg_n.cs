using System;
using ElectronTransferModel.Base;
using ElectronTransferModel.Geo;

namespace ElectronTransferModel.V9_4
{
    [Serializable]
    public class Gg_gz_dg_n : ElectronBase
    {
        public System.String AZTJ { get; set; }
        public System.String BX { get; set; }
        public System.String CD_CLLB { get; set; }
        public System.String CD_SFZJ { get; set; }
        [Precision(10)]
        public System.Int64? G3E_CID { get; set; }
        [Precision(5)]
        public System.Int32? G3E_CNO { get; set; }
        [Precision(10)]
        public System.Double? GG { get; set; }
        public System.String GH { get; set; }
        [Precision(10)]
        public System.Double? GJ { get; set; }
        public System.String GX { get; set; }
        public System.String LPGG { get; set; }
        public System.String LPSM { get; set; }
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
        public System.String MS { get; set; }
        [Precision(10)]
        public System.Int64? SCM_DID { get; set; }
        public System.String ZWJD { get; set; }
    }
}
