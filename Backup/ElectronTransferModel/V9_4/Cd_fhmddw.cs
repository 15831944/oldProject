using System;
using ElectronTransferModel.Base;
using ElectronTransferModel.Geo;

namespace ElectronTransferModel.V9_4
{
    [Serializable]
    public class Cd_fhmddw : DBEntity
    {
        public System.String CD_SSDW { get; set; }
        [KeyField]
        public System.String NAME { get; set; }
    }
}
