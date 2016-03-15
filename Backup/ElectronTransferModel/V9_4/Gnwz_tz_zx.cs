using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using ElectronTransferModel.Base;
using ElectronTransferModel.Geo;

namespace ElectronTransferModel.V9_4
{
    [Serializable]
    public class Gnwz_tz_zx : DBEntity
    {
        [KeyField]
        public  long ID { get; set; }

        public  long G3E_FID { get; set; }

        public DateTime IN_TIME { get; set; }
    }
}
