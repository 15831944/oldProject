using System;
using ElectronTransferModel.Base;
using ElectronTransferModel.Geo;

namespace ElectronTransferModel.V9_4
{
    [Serializable]
    public class Gg_jx_kx_zyx_n : ElectronBase
    {
        [Precision(10)]
        public System.Double? CD { get; set; }
        public System.String CD_SFBLX { get; set; }
        public System.String DXDY { get; set; }
        public System.String DXFH { get; set; }
        public System.String DXGG { get; set; }
        public System.String DXLX { get; set; }
        [Precision(10)]
        public System.Int64? G3E_CID { get; set; }
        [Precision(5)]
        public System.Int32? G3E_CNO { get; set; }
        public System.String GGJSQK { get; set; }
        public System.String GXDW { get; set; }
        public System.String JDCTQK { get; set; }
        public System.DateTime? JGRQ { get; set; }
        [Precision(10)]
        public System.Double? JMJ { get; set; }
        public System.String KXWZ { get; set; }
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
        public System.String QZGH { get; set; }
        public System.String SGDW { get; set; }
        public System.String SM { get; set; }
        public System.String XLMC { get; set; }
        public System.String YXQK { get; set; }
    }
}
