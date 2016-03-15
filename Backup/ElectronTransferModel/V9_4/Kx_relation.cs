using System;
using ElectronTransferModel.Base;
using ElectronTransferModel.Geo;

namespace ElectronTransferModel.V9_4
{
    [Serializable]
    public class Kx_relation : DBEntity
    {
        [SelectOnly]
        public System.String A { get; set; }
        [SelectOnly]
        public System.String B { get; set; }
        [SelectOnly]
        public System.String C { get; set; }
    }
}
