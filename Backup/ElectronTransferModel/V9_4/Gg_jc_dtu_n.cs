using System;
using ElectronTransferModel.Base;
using ElectronTransferModel.Geo;

namespace ElectronTransferModel.V9_4
{
    [Serializable]
    public class Gg_jc_dtu_n : ElectronBase
    {
        public System.String ACXCTBB { get; set; }
        public System.String CCBH { get; set; }
        public System.String DFLX { get; set; }
        [Precision(10)]
        public System.Int64? DYDF { get; set; }
        [Precision(10)]
        public System.Int64 G3E_CID { get; set; }
        [Precision(5)]
        public System.Int32 G3E_CNO { get; set; }
        [SelectOnly]
        public System.DateTime? LTT_DATE { get; set; }
        [SelectOnly]
        [Precision(29)]
        public System.Decimal LTT_ID { get; set; }
        [SelectOnly]
        public System.String LTT_STATUS { get; set; }
        [SelectOnly]
        [Precision(29)]
        public System.Decimal? LTT_TID { get; set; }
        public System.String LXCTBB { get; set; }
        public System.String SIMKH { get; set; }
        public System.String TXFS { get; set; }
        public System.String ZDIPDZ { get; set; }
        public System.String ZDKZPZ { get; set; }
        public System.String ZDLFS { get; set; }
        public System.String ZDZKPZ { get; set; }
    }
}
