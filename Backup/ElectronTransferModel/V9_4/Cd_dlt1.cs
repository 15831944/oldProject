using System;
using ElectronTransferModel.Base;
using ElectronTransferModel.Geo;

namespace ElectronTransferModel.V9_4
{
    [Serializable]
    public class Cd_dlt1 : DBEntity
    {
        [Precision(10)]
        public System.Int64? G3E_FID { get; set; }
    }
}
