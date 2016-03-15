using System;
using ElectronTransferModel.Base;
using ElectronTransferModel.Geo;

namespace ElectronTransferModel.V9_4
{
    [Serializable]
    public class Deficiency_n : ElectronBase
    {
        [Precision(10)]
        public System.Int64? G3E_CID { get; set; }
        [Precision(5)]
        public System.Int32? G3E_CNO { get; set; }
        public System.String G3E_DEFICIENCYCATEGORY { get; set; }
        [Precision(10)]
        public System.Int64? G3E_DEFICIENCYCID { get; set; }
        [Precision(5)]
        public System.Int32? G3E_DEFICIENCYCNO { get; set; }
        [Precision(10)]
        public System.Int64? G3E_DEFICIENCYFID { get; set; }
        [Precision(5)]
        public System.Int32? G3E_DEFICIENCYFNO { get; set; }
        public System.String G3E_DEFICIENCYTYPE { get; set; }
        public System.String G3E_DEFICIENCYVALUE { get; set; }
        public System.DateTime? G3E_REPAIRED_DATE { get; set; }
        public System.String G3E_REPAIRED_JOB { get; set; }
        public System.String G3E_STATUS { get; set; }
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
