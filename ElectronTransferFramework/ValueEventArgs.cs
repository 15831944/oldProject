using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ElectronTransferFramework
{
    public class ValueEventArgs:EventArgs
    {
        public object Value { get; set; }
    }
}
