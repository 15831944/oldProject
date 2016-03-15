using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using ElectronTransferModel.Base;
using ElectronTransferModel.Geo;

namespace ElectronTransferModel.V9_4
{
    [Serializable]
    public class Gg_kxmanage : DBEntity
    {
        [Precision(29)]
        public System.Decimal? ID { get; set; }
        [KeyField]
        [Precision(29)]
        public System.Decimal? KX_ID { get; set; }
        [KeyField]
        public System.String SSDW { get; set; }
    }
}
