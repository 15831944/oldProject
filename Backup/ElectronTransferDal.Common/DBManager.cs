using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ElectronTransferDal.Common
{
    public static class DBManager
    {
        private static IDBManager _manager = null;

        public static void Set(IDBManager manager)
        {
            _manager = manager;
        }

        public static IDBManager Instance {
            get { return _manager; }
        }

    }
}   
