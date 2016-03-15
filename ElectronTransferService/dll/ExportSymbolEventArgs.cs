using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using ElectronTransferDal.Common;
using ElectronTransferDal.OracleDal;
using ElectronTransferDal.XmlDal;
using ElectronTransferModel.V9_4;
using Oracle.DataAccess.Client;
using ElectronTransferFramework;

namespace ElectronTransferServiceDll
{
    public class ExportSymbolEventArgs:ValueEventArgs
    {
        public object Value { get; set; }

        public object Value2 { get; set; }

        public object Value3 { get; set; }
    }
}
