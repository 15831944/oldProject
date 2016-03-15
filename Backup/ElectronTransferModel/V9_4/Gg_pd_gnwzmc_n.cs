using System;
using ElectronTransferModel.Base;
using ElectronTransferModel.Geo;

namespace ElectronTransferModel.V9_4
{
    [Serializable]
    public class Gg_pd_gnwzmc_n : ElectronBase
    {
        public System.String BM { get; set; }
        public System.String BZ { get; set; }
        public System.String DXLB { get; set; }
        [Precision(10)]
        public System.Int64 G3E_CID { get; set; }
        [Precision(5)]
        public System.Int32 G3E_CNO { get; set; }
        public System.String GNWZ_BYQRL { get; set; }
        public System.String GNWZ_CXD { get; set; }
        public System.String GNWZ_DLYB { get; set; }
        public System.String GNWZ_DYDXTH { get; set; }
        public System.String GNWZ_DYGFL { get; set; }
        public System.String GNWZ_FL { get; set; }
        public System.String GNWZ_FL2 { get; set; }
        public System.String GNWZ_FLZGDYDJ { get; set; }
        public System.String GNWZ_KGGLX { get; set; }
        public System.String GNWZ_QY { get; set; }
        public System.String GNWZ_SFDYRDQ { get; set; }
        public System.String GNWZ_SFKQXL { get; set; }
        public System.String GNWZ_SSDF { get; set; }
        public System.String GNWZ_SSFL { get; set; }
        public System.String GNWZ_SSGT { get; set; }
        public System.String GNWZ_SSKGG { get; set; }
        public System.String GNWZ_SSTJ { get; set; }
        public System.String GNWZ_XLFL { get; set; }
        public System.String GNWZ_YGGL { get; set; }
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
        public System.String LX { get; set; }
        public System.String MC { get; set; }
    }
}
