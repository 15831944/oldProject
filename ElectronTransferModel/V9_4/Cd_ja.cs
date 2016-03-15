using System;
using ElectronTransferModel.Base;
using ElectronTransferModel.Geo;

namespace ElectronTransferModel.V9_4
{
    [Serializable]
    public class Cd_ja : DBEntity
    {
        [Precision(29)]
        public System.Decimal? NAME { get; set; }
    }
}
