using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using ElectronTransferModel.Base;
using ElectronTransferModel.Geo;

namespace ElectronTransferModel.V9_4
{
    [Serializable]
    public class G3e_nodeedgeconnectivity : DBEntity
    {
        [KeyField]
        public long G3E_NECNO { get; set; }

        public int G3E_SOURCEFNO { get; set; }

        public int G3E_CONNECTINGFNO { get; set; }
    }
}
