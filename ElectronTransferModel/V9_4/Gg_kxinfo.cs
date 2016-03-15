using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using ElectronTransferModel.Base;
using ElectronTransferModel.Geo;

namespace ElectronTransferModel.V9_4
{
    [Serializable]
    public class Gg_kxinfo : DBEntity
    {
        [KeyField]
        [Precision(29)]
        public System.Decimal? KX_ID { get; set; }

        public System.String BDZMC { get; set; }

        [Precision(29)]
        public System.Decimal BDZ_FID { get; set; }

        public System.String CXBH { get; set; }

        [Precision(29)]
        public System.Decimal YX_FID { get; set; }

        public System.String XZFL { get; set; }

        public System.String YFLL { get; set; }

        public System.DateTime TCRQ { get; set; }

        public System.String KXMC { get; set; }

        [Precision(29)]
        public System.Decimal SFCX { get; set; }

        //public System.String GDS1 { get; set; }
        //[Precision(29)]
        //public System.Decimal GDS1_ID { get; set; }

        //public System.String GDS2 { get; set; }
        //[Precision(29)]
        //public System.Decimal GDS2_ID { get; set; }




    }
}
