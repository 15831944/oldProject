using System;
using ElectronTransferModel.Base;
using ElectronTransferModel.Geo;

namespace ElectronTransferModel.V9_4
{
    [Serializable]
    public class Inspect_n : ElectronBase
    {
        [Precision(10)]
        public System.Int64? G3E_CID { get; set; }
        public System.String G3E_CIRCUITNUMBER { get; set; }
        [Precision(5)]
        public System.Int32? G3E_CNO { get; set; }
        public System.DateTime? G3E_DATEPLACED { get; set; }
        [Precision(5)]
        public System.Int32? G3E_DEFICIENCYCOUNT { get; set; }
        public System.String G3E_JOBPLACED { get; set; }
        public System.String G3E_NOTES { get; set; }
        [Precision(10)]
        public System.Int64? G3E_OWNER_ID { get; set; }
        [Precision(10)]
        public System.Int64? G3E_OWNERCNO { get; set; }
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
    }
}
