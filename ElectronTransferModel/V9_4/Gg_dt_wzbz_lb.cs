using System;
using ElectronTransferModel.Base;
using ElectronTransferModel.Geo;

namespace ElectronTransferModel.V9_4
{
    [Serializable]
    public class Gg_dt_wzbz_lb_sdogeom_cad : ElectronSymbol
    {
        //[Precision(10)]
        //public System.Int64? G3E_CID { get; set; }
        //[Precision(5)]
        //public System.Int32? G3E_CNO { get; set; }
        //[Precision(29)]
        //public System.Decimal? GDO_GID { get; set; }
        //[Precision(5)]
        //public System.Int32? G3E_ALIGNMENT { get; set; }
        //[SelectOnly]
        //public System.String CD_GXDW { get; set; }
        [SelectOnly]
        public System.String CD_YSLX { get; set; }
        //[SelectOnly]
        //public System.String DTDM { get; set; }
        [SelectOnly]
        public System.String MIF_TEXT { get; set; }
        [SelectOnly]
        public System.String LTT_STATUS { get; set; }


       
    }
}
