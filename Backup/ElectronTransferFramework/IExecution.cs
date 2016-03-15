using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ElectronTransferFramework
{
    public interface IExecution
    {
        void Execute(object sender, ValueEventArgs args);
    }
}
