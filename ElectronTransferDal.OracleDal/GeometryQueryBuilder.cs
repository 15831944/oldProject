using System.Collections.Generic;
using System.Linq;
using ElectronTransferDal.Common;
using ElectronTransferModel.Base;
using ElectronTransferModel.Config;

namespace ElectronTransferDal.OracleDal
{
    abstract class GeometryQueryBuilder:QueryBuilder
    {
        protected ElectronSymbol Symbol;

        public IEnumerable<string> ObjectFields { get; protected set; }
        public IEnumerable<string> GeometryFields { get; protected set; }
        public IEnumerable<string> AllFields { get { return ObjectFields.Concat(GeometryFields); } }

        protected GeometryQueryBuilder(ElectronSymbol symbol, IEnumerable<string> avoidFields, SimpleMapping mapping)
            : base(symbol.GetType(), avoidFields, mapping, null, null, true)
        {
            Symbol = symbol;
        }
        
        
    }
}
