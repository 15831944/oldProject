using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ElectronTransferDal.Common
{
    internal abstract class JobSurround:ISurround
    {
        public abstract string Begin();
        public abstract string End();


    }
}
