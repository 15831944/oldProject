using System;
using ElectronTransferModel.Base;
using ElectronTransferModel.Geo;

namespace ElectronTransferModel.V9_4
{
    [Serializable]
    public class Gg_pd_dykg_n : ElectronBase
    {
        public System.DateTime? AZSJ { get; set; }
        public System.String BXSDQZ { get; set; }
        public System.String CD_DYKGFL { get; set; }
        [Precision(10)]
        public System.Int64? G3E_CID { get; set; }
        [Precision(5)]
        public System.Int32? G3E_CNO { get; set; }
        public System.String GDFW { get; set; }
        public System.String KGGXH { get; set; }
        public System.String KGXH { get; set; }
        public System.String KGXU { get; set; }
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
        public System.String SSBYQ { get; set; }
        public System.String YXQK { get; set; }
    }
}
