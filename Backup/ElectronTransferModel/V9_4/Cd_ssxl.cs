using System;
using ElectronTransferModel.Base;
using ElectronTransferModel.Geo;

namespace ElectronTransferModel.V9_4
{
    [Serializable]
    public class Cd_ssxl : DBEntity
    {
        [KeyField]
        public System.String CD_SSDW { get; set; }
        [KeyField]
        public System.String NAME { get; set; }
        [KeyField]
        public System.String SSBDZ { get; set; }
    }
}
