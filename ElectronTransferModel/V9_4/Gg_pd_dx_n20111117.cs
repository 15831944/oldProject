using System;
using ElectronTransferModel.Base;
using ElectronTransferModel.Geo;

namespace ElectronTransferModel.V9_4
{
    [Serializable]
    public class Gg_pd_dx_n20111117 : ElectronBase
    {
        [SelectOnly]
        [Precision(10)]
        public System.Double? CD { get; set; }
        [SelectOnly]
        public System.String CD_SFBLX { get; set; }
        [SelectOnly]
        public System.String CXD { get; set; }
        [SelectOnly]
        public System.String DXDY { get; set; }
        [SelectOnly]
        public System.String DXFH { get; set; }
        [SelectOnly]
        public System.String DXGG { get; set; }
        [SelectOnly]
        public System.String DXLX { get; set; }
        [SelectOnly]
        [Precision(10)]
        public System.Int64 G3E_CID { get; set; }
        [SelectOnly]
        [Precision(5)]
        public System.Int32 G3E_CNO { get; set; }
        [SelectOnly]
        public System.String GGJSQK { get; set; }
        [SelectOnly]
        [Precision(29)]
        public System.Decimal? GS { get; set; }
        [SelectOnly]
        public System.String GXDW { get; set; }
        [SelectOnly]
        public System.String JDCTQK { get; set; }
        [SelectOnly]
        public System.DateTime? JGRQ { get; set; }
        [SelectOnly]
        [Precision(10)]
        public System.Double? JMJ { get; set; }
        [SelectOnly]
        public System.String KXWZ { get; set; }
        [SelectOnly]
        public System.String LJDL { get; set; }
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
        [SelectOnly]
        public System.String QZGH { get; set; }
        [SelectOnly]
        [Precision(10)]
        public System.Int64? SCM_DID { get; set; }
        [SelectOnly]
        public System.String SFZCX { get; set; }
        [SelectOnly]
        public System.String SFZGXL { get; set; }
        [SelectOnly]
        public System.String SGDW { get; set; }
        [SelectOnly]
        public System.String SM { get; set; }
        [SelectOnly]
        public System.String WZZB { get; set; }
        [SelectOnly]
        public System.String XLMC { get; set; }
        [SelectOnly]
        public System.String YXQK { get; set; }
    }
}
