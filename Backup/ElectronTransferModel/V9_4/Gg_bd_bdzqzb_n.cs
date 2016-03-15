using System;
using ElectronTransferModel.Base;
using ElectronTransferModel.Geo;

namespace ElectronTransferModel.V9_4
{
    [Serializable]
    public class Gg_bd_bdzqzb_n : ElectronBase
    {
        public System.String DLZK { get; set; }
        public System.String EDRL { get; set; }
        public System.String FZSH { get; set; }
        [Precision(10)]
        public System.Int64? G3E_CID { get; set; }
        [Precision(5)]
        public System.Int32? G3E_CNO { get; set; }
        public System.String JXFS { get; set; }
        public System.String KZDL { get; set; }
        public System.String KZSH { get; set; }
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
        public System.String LXDK { get; set; }
        public System.String LXZK { get; set; }
        public System.String PDBJ { get; set; }
    }
}
