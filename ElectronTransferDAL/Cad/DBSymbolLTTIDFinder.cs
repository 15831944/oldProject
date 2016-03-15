using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using ElectronTransferFramework;

namespace ElectronTransferDal.Cad
{
    public class DBSymbolLTTIDFinder:Singleton<DBSymbolLTTIDFinderImplment>
    {
    }
    public class DBSymbolLTTIDFinderImplment : DictionaryWithEvent<long, EntityObj>
    {
        public IEnumerable<EntityObj> DBSymbols { get { return this.Values; } }
    }
}
