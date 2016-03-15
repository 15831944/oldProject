using System;
using ElectronTransferModel.Base;
using ElectronTransferModel.Geo;

namespace ElectronTransferModel.V9_4
{
    [Serializable]
    public class Cd_ydlb : DBEntity
    {
        [KeyField]
        public System.String NAME { get; set; }
        [Precision(29)]
        public System.Decimal? VALUE { get; set; }
    }
}
