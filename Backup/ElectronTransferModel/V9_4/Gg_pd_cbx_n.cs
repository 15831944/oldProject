using System;
using ElectronTransferModel.Base;
using ElectronTransferModel.Geo;

namespace ElectronTransferModel.V9_4
{
    [Serializable]
    public class Gg_pd_cbx_n : ElectronBase
    {
        public System.String AZDZ { get; set; }
        public System.String BH { get; set; }
        public System.String BZ { get; set; }
        public System.String BZ1 { get; set; }
        public System.String CD_CQSX { get; set; }
        public System.String CD_DYDJ { get; set; }
        public System.String CD_SMZQ { get; set; }
        public System.String CD_SSDW { get; set; }
        public System.String CD_SSXL { get; set; }
        public System.String CD_XHGE { get; set; }
        public System.String CD_ZZCJ { get; set; }
        [Precision(1)]
        public System.Int16? DWFX_SFSDY { get; set; }
        public System.String DZJL { get; set; }
        [Precision(10)]
        public System.Int64? G3E_CID { get; set; }
        [Precision(5)]
        public System.Int32? G3E_CNO { get; set; }
        public System.String GDH { get; set; }
        public System.String HH { get; set; }
        [Precision(5)]
        public System.Int32? HS { get; set; }
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
        public System.String LX { get; set; }
        public System.String QT { get; set; }
        public System.String SBBM { get; set; }
        public System.String SSBYQ { get; set; }
        public System.String YDLB { get; set; }
        public System.String YXBH { get; set; }
        [Precision(29)]
        public System.Decimal? ZBZRL { get; set; }
        [Precision(29)]
        public System.Decimal? ZDL { get; set; }
    }
}
