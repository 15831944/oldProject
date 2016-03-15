using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ElectronTransferDal.QueryVerifyHelper
{
    public class QueryRes
    {
        public int SerialNum { get; set; }

        public long G3E_FID { get; set; }

        public string FeatureType { get; set; }

        public string SDKX { get; set; }

        public string State { get; set; }

        public string Sbmc { get; set; }
    }
}
