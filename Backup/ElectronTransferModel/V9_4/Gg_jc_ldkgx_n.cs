using System;
using ElectronTransferModel.Base;
using ElectronTransferModel.Geo;

namespace ElectronTransferModel.V9_4
{
    [Serializable]
    public class Gg_jc_ldkgx_n : ElectronBase
    {
        public System.String BBH { get; set; }
        public System.String DDBBH { get; set; }
        public System.String DDBJBH { get; set; }
        public System.String DDBRL { get; set; }
        [Precision(10)]
        public System.Int64? G3E_CID { get; set; }
        [Precision(5)]
        public System.Int32? G3E_CNO { get; set; }
        public System.String KD { get; set; }
        public System.String KGBH { get; set; }
        public System.String KGDD { get; set; }
        public System.String KXMC { get; set; }
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
        public System.String SSGBF { get; set; }
        public System.String XH { get; set; }
        public System.String XLMC { get; set; }
        public System.String XLZXA { get; set; }
        public System.String XLZXB { get; set; }
        public System.String XLZXC { get; set; }
        public System.String XX { get; set; }
        public System.String ZBMC { get; set; }
    }
}
