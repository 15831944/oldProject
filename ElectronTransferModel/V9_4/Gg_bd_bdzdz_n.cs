using System;
using ElectronTransferModel.Base;
using ElectronTransferModel.Geo;

namespace ElectronTransferModel.V9_4
{
    [Serializable]
    public class Gg_bd_bdzdz_n : ElectronBase
    {
        public System.String DKJ1 { get; set; }
        public System.String DKJ2 { get; set; }
        public System.String DLCX { get; set; }
        public System.String EDDZ { get; set; }
        public System.String EDFZ { get; set; }
        public System.String EDPL { get; set; }
        [Precision(10)]
        public System.Int64? G3E_CID { get; set; }
        [Precision(5)]
        public System.Int32? G3E_CNO { get; set; }
        public System.String GPNY { get; set; }
        public System.String HLSP { get; set; }
        public System.String JYDJ { get; set; }
        public System.String LDDY { get; set; }
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
        public System.String PJBJ { get; set; }
        public System.String WDDL { get; set; }
        public System.String WJPJ { get; set; }
        public System.String XDD1 { get; set; }
        public System.String XDD2 { get; set; }
        public System.String XS { get; set; }
        public System.String YRDL { get; set; }
        public System.String YRGF { get; set; }
        public System.String ZGDY { get; set; }
    }
}
