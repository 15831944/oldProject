using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ElectronTransferView.SyncTZDataToGIS
{
    public class TzTableValue
    {
        public string TableName { set; get; }
        public List<KeyValue> Values { set; get; }
    }
    public class KeyValue
    {
        public string cloumName { set; get; }
        public string cloumValue { set; get; }
    }
}
