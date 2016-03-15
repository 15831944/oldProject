using System;
using ElectronTransferModel.Base;
using ElectronTransferModel.Geo;

namespace ElectronTransferModel.V9_4
{
    [Serializable]
    public class Gg_jx_dx_n : ElectronBase
    {
        [Precision(10)]
        public System.Double? CD { get; set; }
        public System.String DLLX { get; set; }
        public System.String DLLX1 { get; set; }
        public System.String DLLX2 { get; set; }
        public System.String DLWZ1 { get; set; }
        public System.String DLWZ2 { get; set; }
        public System.String DXGG { get; set; }
        public System.String FSXS { get; set; }
        [Precision(10)]
        public System.Int64? G3E_CID { get; set; }
        [Precision(5)]
        public System.Int32? G3E_CNO { get; set; }
        public System.String GXDW { get; set; }
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
        public System.String XLMC { get; set; }
        public System.String YXQK { get; set; }
        public System.String ZCGS { get; set; }
        public System.String ZJJTSL { get; set; }
    }
}
