using System.Collections.Generic;
using System.Data.Common;
using ElectronTransferDal.Common;

namespace ElectronTransferDal.OracleDal
{
    class OracleParametersBuilder : ParametersBuilder
    {
        public OracleParametersBuilder(object entity, DbCommand command, IList<string> fields) : base(entity, command, fields)
        {
        }
    }

}
