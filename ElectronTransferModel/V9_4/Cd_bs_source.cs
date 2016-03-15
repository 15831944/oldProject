using System;
using ElectronTransferModel.Base;
using ElectronTransferModel.Geo;

namespace ElectronTransferModel.V9_4
{
    [Serializable]
    public class Cd_bs_source : DBEntity
    {
        public System.String BM { get; set; }
        [KeyField]
        public System.String NAME { get; set; }
        public System.String SSDW { get; set; }
    }
}
