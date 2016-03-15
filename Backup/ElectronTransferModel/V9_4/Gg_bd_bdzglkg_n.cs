using System;
using ElectronTransferModel.Base;
using ElectronTransferModel.Geo;

namespace ElectronTransferModel.V9_4
{
    [Serializable]
    public class Gg_bd_bdzglkg_n : ElectronBase
    {
        public System.String CD_BDZGLKGFZHLJYSP { get; set; }
        public System.String CD_PDBJ { get; set; }
        public System.String CD_XS { get; set; }
        [Precision(10)]
        public System.Double? DKJ1 { get; set; }
        public System.String DKJ2 { get; set; }
        public System.String DLCXSJ { get; set; }
        [Precision(10)]
        public System.Double? DQSM { get; set; }
        [Precision(10)]
        public System.Double? EDPL { get; set; }
        public System.String FZBTQX { get; set; }
        [Precision(10)]
        public System.Double? FZNSDL { get; set; }
        [Precision(10)]
        public System.Int64? G3E_CID { get; set; }
        [Precision(5)]
        public System.Int32? G3E_CNO { get; set; }
        public System.String GPNSDY { get; set; }
        [Precision(10)]
        public System.Double? HLDZ { get; set; }
        public System.String HZBTQX { get; set; }
        public System.String JSDYBH { get; set; }
        public System.String JXSM { get; set; }
        public System.String KGFZSJ { get; set; }
        public System.String KGHLDZ { get; set; }
        public System.String KGHUDZ { get; set; }
        public System.String KGHZSJ { get; set; }
        public System.String LJNSDY { get; set; }
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
        public System.String RWDDL { get; set; }
        public System.String SZGSBH { get; set; }
        public System.String SZGSMC { get; set; }
        [Precision(10)]
        public System.Double? XDD1 { get; set; }
        [Precision(10)]
        public System.Double? XDD2 { get; set; }
        [Precision(10)]
        public System.Double? ZKDY { get; set; }
    }
}
