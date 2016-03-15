using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Autodesk.AutoCAD.DatabaseServices;
using ElectronTransferFramework;
using ElectronTransferModel.V9_4;
using ElectronTransferModel.Base;

namespace ElectronTransferDal.Cad
{
    public class ObjectHistoryManager : Singleton<ObjectHistoryImplment>
    {
    }

    public class ObjectHistoryImplment : DictionaryWithEvent<ObjectId, List<DBEntity>>
    {
        public IEnumerable<List<DBEntity>> DBSymbols
        {
            get { return Values; }
        }
    }
}
