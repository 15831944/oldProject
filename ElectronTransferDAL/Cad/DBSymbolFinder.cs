using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Autodesk.AutoCAD.DatabaseServices;
using ElectronTransferModel.Base;
using ElectronTransferFramework;
using ElectronTransferModel;
using System.Collections;

namespace ElectronTransferDal.Cad
{
    public class DBSymbolFinder : Singleton<DBSymbolFinderImplment> 
    {
    }
    public class DBSymbolFinderImplment : DictionaryWithEvent<ObjectId, ElectronSymbol>
    {
        public IEnumerable<ElectronSymbol> DBSymbols { get { return this.Values; } }
    }
}
