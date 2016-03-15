using System;
using ElectronTransferModel.Base;
using ElectronTransferModel.Geo;

namespace ElectronTransferModel.V9_4
{
    [Serializable]
    public class Gg_pd_zsbyq_n : ElectronBase
    {
        public System.String CCBH { get; set; }
        public System.String CD_PBLB { get; set; }
        public System.String CD_ZSBYQFL { get; set; }
        [Precision(10)]
        public System.Double? CFRL { get; set; }
        public System.String DLDY { get; set; }
        public System.String DLSH { get; set; }
        [Precision(10)]
        public System.Double? EDRL { get; set; }
        [Precision(10)]
        public System.Int64? G3E_CID { get; set; }
        [Precision(5)]
        public System.Int32? G3E_CNO { get; set; }
        public System.String KZDL { get; set; }
        public System.String KZSH { get; set; }
        public System.String LJZ { get; set; }
        public System.String LQFS { get; set; }
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
        public System.String PL { get; set; }
        [Precision(10)]
        public System.Double? QSZ { get; set; }
        public System.String RLDW { get; set; }
        public System.String SBBH { get; set; }
        [Precision(10)]
        public System.Int64? SCM_DID { get; set; }
        public System.String XH { get; set; }
        public System.String XS { get; set; }
        public System.String YXQK { get; set; }
        [Precision(10)]
        public System.Double? YZ { get; set; }
        [Precision(10)]
        public System.Double? ZZ { get; set; }
    }
}
