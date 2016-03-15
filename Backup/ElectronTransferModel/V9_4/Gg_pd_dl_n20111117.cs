using System;
using ElectronTransferModel.Base;
using ElectronTransferModel.Geo;

namespace ElectronTransferModel.V9_4
{
    [Serializable]
    public class Gg_pd_dl_n20111117 : ElectronBase
    {
        [SelectOnly]
        public System.String BZ { get; set; }
        [SelectOnly]
        [Precision(10)]
        public System.Double? CD { get; set; }
        [SelectOnly]
        public System.String CD_ZCHF { get; set; }
        [SelectOnly]
        public System.String CXD { get; set; }
        [SelectOnly]
        public System.String DLBM { get; set; }
        [SelectOnly]
        public System.String DLLX { get; set; }
        [SelectOnly]
        public System.String DLLX1 { get; set; }
        [SelectOnly]
        public System.String DLLX2 { get; set; }
        [SelectOnly]
        public System.String DLMC { get; set; }
        [SelectOnly]
        public System.String DLWZ1 { get; set; }
        [SelectOnly]
        public System.String DLWZ2 { get; set; }
        [SelectOnly]
        public System.String DLYB { get; set; }
        [SelectOnly]
        public System.DateTime? FSRQ { get; set; }
        [SelectOnly]
        public System.String FSXS { get; set; }
        [SelectOnly]
        [Precision(10)]
        public System.Int64 G3E_CID { get; set; }
        [SelectOnly]
        [Precision(5)]
        public System.Int32 G3E_CNO { get; set; }
        [SelectOnly]
        public System.String GXDW { get; set; }
        [SelectOnly]
        public System.DateTime? JGRQ { get; set; }
        [SelectOnly]
        [Precision(10)]
        public System.Double? JMJ { get; set; }
        [SelectOnly]
        public System.String JYLX { get; set; }
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
        [Precision(10)]
        public System.Int64? SCM_DID { get; set; }
        [SelectOnly]
        public System.String SFZCX { get; set; }
        [SelectOnly]
        public System.String SFZGXL { get; set; }
        [SelectOnly]
        public System.String SGDW { get; set; }
        [SelectOnly]
        public System.String SSKX { get; set; }
        [SelectOnly]
        public System.String WZZB { get; set; }
        [SelectOnly]
        public System.String YXQK { get; set; }
        [SelectOnly]
        public System.String ZCGS { get; set; }
        [SelectOnly]
        public System.String ZJJTSL { get; set; }
    }
}
