using System;
using ElectronTransferModel.Base;
using ElectronTransferModel.Geo;

namespace ElectronTransferModel.V9_4
{
    [Serializable]
    public class Common_n : ElectronBase
    {
        public System.String BZ { get; set; }
        public System.String BZ1 { get; set; }
        public System.String BZ2 { get; set; }
        public System.String BZ3 { get; set; }
        public System.DateTime? CCRQ { get; set; }
        public System.String CD_CQSX { get; set; }
        public System.String CD_DYDJ { get; set; }
        public System.String CD_GSDY { get; set; }
        public System.String CD_SBPJ { get; set; }
        public System.String CD_SMZQ { get; set; }
        public System.String CD_SSBDZ { get; set; }
        public System.String CD_SSDW { get; set; }
        public System.String CD_SSXL { get; set; }
        public System.String CD_XHGE { get; set; }
        public System.String CD_ZZCJ { get; set; }
        public System.String DQTZ { get; set; }
        [Precision(10)]
        public System.Double? DWFX_DQFH { get; set; }
        [Precision(10)]
        public System.Double? DWFX_SYPJFH { get; set; }
        [Precision(10)]
        public System.Double? DWFX_ZRL { get; set; }
        public System.String EDDL { get; set; }
        public System.String EDDY { get; set; }
        [Precision(10)]
        public System.Int64 G3E_CID { get; set; }
        [Precision(5)]
        public System.Int32 G3E_CNO { get; set; }
        public System.String GDZCKH { get; set; }
        public System.String HH { get; set; }
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
        [Precision(10)]
        public System.Int64? OWNER1_ID { get; set; }
        [Precision(10)]
        public System.Int64? OWNER2_ID { get; set; }
        public System.String QT { get; set; }
        public System.String SBBM { get; set; }
        public System.String SBMC { get; set; }
        public System.String SBPJSM { get; set; }
        [Precision(10)]
        public System.Int64? SCM_DID { get; set; }
        public System.String SGDW { get; set; }
        public System.DateTime? TYRQ { get; set; }
        public System.String WHBS { get; set; }
        public System.String YXBH { get; set; }
        public System.String YXBH1 { get; set; }
    }
}
