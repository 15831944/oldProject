using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using ElectronTransferFramework;

namespace ElectronTransferDal.Cad
{
    public class DeleteSymbolEventArgs:ValueEventArgs
    {
        public long g3e_fid { set; get; }
    }
}
