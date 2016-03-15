using System;
using ElectronTransferModel.Base;
using ElectronTransferModel.Geo;

namespace ElectronTransferModel.V9_4
{
    [Serializable]
    public class Cd_txfsdtu : DBEntity
    {
        [KeyField]
        [Precision(8)]
        public System.Int32 G3E_FNO { get; set; }
        [KeyField]
        public System.String NAME { get; set; }
    }
}
