using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using ElectronTransferModel.Base;
using ElectronTransferModel.Geo;

namespace ElectronTransferModel.V9_4
{
    [Serializable]
    public class G3e_linestyle : DBEntity
    {
        public int G3E_SNO { get; set; }
        public string G3E_USERNAME { get; set; }
        [Precision(9)]
        public Decimal? G3E_COLOR { get; set; }
        [Precision(29)]
        public Decimal? G3E_WIDTH { get; set; }
    }
}