using System;
using ElectronTransferModel.Base;
using ElectronTransferModel.Geo;

namespace ElectronTransferModel.V9_4
{
    [Serializable]
    public class Gg_gz_dlg_n : ElectronBase
    {
        public System.String DLGGG { get; set; }
        public System.String DLGLB { get; set; }
        public System.String DLSL { get; set; }
        public System.String DSZJ { get; set; }
        public System.String FJLX { get; set; }
        public System.String FSXS { get; set; }
        [Precision(10)]
        public System.Int64? G3E_CID { get; set; }
        [Precision(5)]
        public System.Int32? G3E_CNO { get; set; }
        public System.String GBGG { get; set; }
        public System.String GCJWZ { get; set; }
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
        public System.String QMLR1 { get; set; }
        public System.String QMLR2 { get; set; }
        public System.String SCLD { get; set; }
        public System.String SJDL { get; set; }
    }
}
