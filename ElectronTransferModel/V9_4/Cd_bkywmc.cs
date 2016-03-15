using System;
using ElectronTransferModel.Base;
using ElectronTransferModel.Geo;

namespace ElectronTransferModel.V9_4
{
    [Serializable]
    public class Cd_bkywmc : DBEntity
    {
        [KeyField]
        public System.String NAME { get; set; }
        public System.String SDO_TABLE { get; set; }
    }
}
