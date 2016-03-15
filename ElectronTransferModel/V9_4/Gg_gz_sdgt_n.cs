using System;
using ElectronTransferModel.Base;
using ElectronTransferModel.Geo;

namespace ElectronTransferModel.V9_4
{
    [Serializable]
    public class Gg_gz_sdgt_n : ElectronBase
    {
        public System.String CD_CLLB { get; set; }
        public System.String CD_DM { get; set; }
        public System.String CD_HHWZ { get; set; }
        public System.String CD_HXWZ { get; set; }
        public System.String CD_LXWZ { get; set; }
        public System.String CD_SSWQ { get; set; }
        public System.String CD_TX { get; set; }
        public System.String CD_XCBZ { get; set; }
        public System.String CD_ZLLX { get; set; }
        public System.String CD_ZWNZ { get; set; }
        public System.String CLR { get; set; }
        public System.DateTime? CLRQ { get; set; }
        public System.String CMGK { get; set; }
        public System.String DDGG { get; set; }
        [Precision(10)]
        public System.Int64? G3E_CID { get; set; }
        [Precision(5)]
        public System.Int32? G3E_CNO { get; set; }
        public System.String GTH { get; set; }
        [Precision(10)]
        public System.Double? HDJ { get; set; }
        [Precision(10)]
        public System.Double? JCCD { get; set; }
        public System.DateTime? JYZRQ { get; set; }
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
        public System.Double? MS { get; set; }
        public System.String SGDW { get; set; }
        public System.String SJDW { get; set; }
        [Precision(10)]
        public System.Double? TG { get; set; }
        public System.DateTime? TTRQ { get; set; }
        public System.DateTime? WZRQ { get; set; }
        public System.DateTime? XJRQ { get; set; }
        public System.String XLZJ { get; set; }
        [Precision(10)]
        public System.Double? ZL { get; set; }
        public System.String ZMGK { get; set; }
    }
}
