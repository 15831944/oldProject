using System;
using ElectronTransferModel.Base;
using ElectronTransferModel.Geo;

namespace ElectronTransferModel.V9_4
{
    [Serializable]
    public class Gg_sd_sdxlnzd_ac : ElectronBase
    {
        public System.String CD_DXNZXJGE { get; set; }
        public System.String CD_DXNZXJGG { get; set; }
        public System.String CD_DXXCXJGE { get; set; }
        public System.String CD_LDXGE { get; set; }
        public System.String CD_RDXGE { get; set; }
        public System.String DBDJ { get; set; }
        public System.String DXAQXS { get; set; }
        public System.String DXDXXS { get; set; }
        public System.DateTime? DXGGRI { get; set; }
        public System.String DXJGBGE { get; set; }
        public System.String DXSCCJ { get; set; }
        public System.String DXXCXJGE { get; set; }
        [Precision(10)]
        public System.Int64? G3E_CID { get; set; }
        [Precision(5)]
        public System.Int32? G3E_CNO { get; set; }
        public System.String GLGE { get; set; }
        public System.String GLJTWZ { get; set; }
        public System.String LDXSCCJ { get; set; }
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
        public System.Double? NZDCD { get; set; }
        public System.String OHDXGE { get; set; }
        public System.String QSTH { get; set; }
        public System.String RDXSCCJ { get; set; }
        public System.String ZZTH { get; set; }
    }
}
