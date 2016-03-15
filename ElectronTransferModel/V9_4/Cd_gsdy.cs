using System;
using ElectronTransferModel.Base;
using ElectronTransferModel.Geo;

namespace ElectronTransferModel.V9_4
{
    [Serializable]
    public class Cd_gsdy : DBEntity
    {
        [KeyField]
        public System.String BDZ_NAME { get; set; }
        [KeyField]
        public System.String NAME { get; set; }
    }
}
