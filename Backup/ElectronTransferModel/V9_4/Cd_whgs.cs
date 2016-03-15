using System;
using ElectronTransferModel.Base;
using ElectronTransferModel.Geo;

namespace ElectronTransferModel.V9_4
{
    [Serializable]
    public class Cd_whgs : DBEntity
    {
        public System.String BM { get; set; }
        [KeyField]
        public System.String NAME { get; set; }
    }
}
