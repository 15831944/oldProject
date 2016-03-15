using System;
using ElectronTransferModel.Base;
using ElectronTransferModel.Geo;

namespace ElectronTransferModel.V9_4
{
    [Serializable]
    public class gnwz_fl2:DBEntity
    {
        [KeyField]
        public System.String NAME { get; set; }
        [KeyField]
        [Precision(29)]
        public System.Decimal G3E_FNO { get; set; }
        [KeyField]
        public System.String FL { get; set; }
    }
}
