using System;
using ElectronTransferModel.Base;
using ElectronTransferModel.Geo;

namespace ElectronTransferModel.V9_4
{
    [Serializable]
    public class Gg_jc_pwy_n : ElectronBase
    {
        public System.String ADRESS { get; set; }
        public System.String BH { get; set; }
        public System.String BZ { get; set; }
        public System.String CCBH { get; set; }
        [Precision(29)]
        public System.Decimal? CIRCUITNUMBER { get; set; }
        public System.String CPSM { get; set; }
        public System.String CTRATIO { get; set; }
        [Precision(29)]
        public System.Decimal? DLJJZ { get; set; }
        public System.String DRCS { get; set; }
        public System.String DYHGL { get; set; }
        [Precision(29)]
        public System.Decimal? DYJJZ { get; set; }
        public System.DateTime? FHCXSJ { get; set; }
        [Precision(10)]
        public System.Int64 G3E_CID { get; set; }
        [Precision(5)]
        public System.Int32 G3E_CNO { get; set; }
        public System.String ISCASCADEDTERMINAL { get; set; }
        public System.String KGMC { get; set; }
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
        [Precision(10)]
        public System.Int64? PBMC { get; set; }
        [Precision(29)]
        public System.Decimal? PJDL { get; set; }
        [Precision(29)]
        public System.Decimal? PJDY { get; set; }
        [Precision(29)]
        public System.Decimal? PJWGDL { get; set; }
        [Precision(29)]
        public System.Decimal? PJYGDL { get; set; }
        public System.String PTRATIO { get; set; }
        public System.String SBLX { get; set; }
        public System.String SBPJ { get; set; }
        public System.String SCCJ { get; set; }
        [Precision(10)]
        public System.Int64? SCM_DID { get; set; }
        public System.String SERIALNUMBER { get; set; }
        public System.String SIM { get; set; }
        public System.String SXPHL { get; set; }
        public System.String SYQK { get; set; }
        public System.String TXFS { get; set; }
        [Precision(29)]
        public System.Decimal? WGDLJJZ { get; set; }
        public System.String XH { get; set; }
        public System.String XTIP { get; set; }
        public System.String YDFP_IP { get; set; }
        [Precision(29)]
        public System.Decimal? YGDLJJZ { get; set; }
        [Precision(29)]
        public System.Decimal? ZDDL { get; set; }
        [Precision(29)]
        public System.Decimal? ZDDY { get; set; }
        [Precision(29)]
        public System.Decimal? ZDWGDL { get; set; }
        [Precision(29)]
        public System.Decimal? ZDYGDL { get; set; }
        public System.String ZGFH { get; set; }
        [Precision(29)]
        public System.Decimal? ZXDL { get; set; }
        [Precision(29)]
        public System.Decimal? ZXDY { get; set; }
        [Precision(29)]
        public System.Decimal? ZXWGDL { get; set; }
        [Precision(29)]
        public System.Decimal? ZXYGDL { get; set; }
    }
}
