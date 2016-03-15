using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using ElectronTransferDal.Common;

namespace ElectronTransferDal
{
    class MySqlGeometryQuery:GeometryQuery
    {
        public override string GeometryQueryText
        {
            get { return string.Format(",ASTEXT({0}) AS {1}", GeometryField, GeometryFieldAlias); }
        }
    }
}
