using System;
using ElectronTransferModel.Base;
using ElectronTransferModel.Geo;

namespace ElectronTransferModel.V9_4
{
    [Serializable]
    public class Cd_xlfl : DBEntity
    {
        public System.String FL { get; set; }
        [KeyField]
        public System.String NAME { get; set; }
    }
}
