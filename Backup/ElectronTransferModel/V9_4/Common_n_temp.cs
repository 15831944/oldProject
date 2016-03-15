using System;
using ElectronTransferModel.Base;
using ElectronTransferModel.Geo;

namespace ElectronTransferModel.V9_4
{
    [Serializable]
    public class Common_n_temp : ElectronBase
    {
        [SelectOnly]
        public System.String BZ { get; set; }
        [SelectOnly]
        public System.String BZ1 { get; set; }
        [SelectOnly]
        public System.String BZ2 { get; set; }
        [SelectOnly]
        public System.String BZ3 { get; set; }
        [SelectOnly]
        public System.DateTime? CCRQ { get; set; }
        [SelectOnly]
        public System.String CD_CQSX { get; set; }
        [SelectOnly]
        public System.String CD_DYDJ { get; set; }
        [SelectOnly]
        public System.String CD_GSDY { get; set; }
        [SelectOnly]
        public System.String CD_SBPJ { get; set; }
        [SelectOnly]
        public System.String CD_SMZQ { get; set; }
        [SelectOnly]
        public System.String CD_SSBDZ { get; set; }
        [SelectOnly]
        public System.String CD_SSDW { get; set; }
        [SelectOnly]
        public System.String CD_SSXL { get; set; }
        [SelectOnly]
        public System.String CD_XHGE { get; set; }
        [SelectOnly]
        public System.String CD_ZZCJ { get; set; }
        [SelectOnly]
        public System.String DQTZ { get; set; }
        [SelectOnly]
        [Precision(10)]
        public System.Double? DWFX_DQFH { get; set; }
        [SelectOnly]
        [Precision(10)]
        public System.Double? DWFX_SYPJFH { get; set; }
        [SelectOnly]
        [Precision(10)]
        public System.Double? DWFX_ZRL { get; set; }
        [SelectOnly]
        public System.String EDDL { get; set; }
        [SelectOnly]
        public System.String EDDY { get; set; }
        [SelectOnly]
        [Precision(10)]
        public System.Int64 G3E_CID { get; set; }
        [SelectOnly]
        [Precision(5)]
        public System.Int32 G3E_CNO { get; set; }
        [SelectOnly]
        public System.String GDZCKH { get; set; }
        [SelectOnly]
        public System.String HH { get; set; }
        [SelectOnly]
        public System.String JGDBH { get; set; }
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
        public System.Int64? OWNER1_ID { get; set; }
        [SelectOnly]
        [Precision(10)]
        public System.Int64? OWNER2_ID { get; set; }
        [SelectOnly]
        public System.String QT { get; set; }
        [SelectOnly]
        public System.String SBBM { get; set; }
        [SelectOnly]
        public System.String SBMC { get; set; }
        [SelectOnly]
        public System.String SBPJSM { get; set; }
        [SelectOnly]
        [Precision(10)]
        public System.Int64? SCM_DID { get; set; }
        [SelectOnly]
        public System.String SGDW { get; set; }
        [SelectOnly]
        public System.DateTime? TYRQ { get; set; }
        [SelectOnly]
        public System.String WHBS { get; set; }
        [SelectOnly]
        public System.String YXBH { get; set; }
    }
}
