using System;
using ElectronTransferModel.Base;
using ElectronTransferModel.Geo;

namespace ElectronTransferModel.V9_4
{
    [Serializable]
    public class Gg_pd_dyshb_n : ElectronBase
    {
        public System.String AZDZ { get; set; }
        public System.String BH { get; set; }
        public System.String BXH { get; set; }
        public System.String BZ { get; set; }
        public System.String BZ1 { get; set; }
        public System.String CD_SSBDZ { get; set; }
        public System.String CD_SSDW { get; set; }
        public System.String CD_SSXL { get; set; }
        [Precision(1)]
        public System.Int16? DWFX_SFSDY { get; set; }
        public System.String DZJL { get; set; }
        [Precision(10)]
        public System.Int64? G3E_CID { get; set; }
        [Precision(5)]
        public System.Int32? G3E_CNO { get; set; }
        public System.String GDH { get; set; }
        public System.String GZZT { get; set; }
        public System.String HBLX { get; set; }
        public System.String HH { get; set; }
        public System.String KHXYH { get; set; }
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
        public System.String MBH { get; set; }
        public System.String QT { get; set; }
        public System.String SFYHYB { get; set; }
        public System.String SMZQ { get; set; }
        public System.String XS { get; set; }
        public System.String YDH { get; set; }
        public System.String YDLB { get; set; }
        public System.String YHLX { get; set; }
        public System.String YHXM { get; set; }
    }
}
