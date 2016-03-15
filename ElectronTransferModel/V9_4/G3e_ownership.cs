using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using ElectronTransferModel.Base;
using ElectronTransferModel.Geo;

namespace ElectronTransferModel.V9_4
{
    [Serializable]
    public class G3e_ownership : DBEntity
    {
        [KeyField]
        public int G3E_ONO { get; set; }

        public int G3E_SOURCEFNO { get; set; }

        public int? G3E_OWNERFNO { get; set; }
    }
}
