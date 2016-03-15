using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using ElectronTransferModel.Config;

namespace ElectronTransferDal.Common
{
    class SelectQueryBuilder : SelectQueryBuilderBase
    {

        public SelectQueryBuilder(Type type, IEnumerable<string> avoidFields, SimpleMapping mapping, GeometryQuery geometryQuery, Expression expr, bool selectSingle)
            : base( type,  avoidFields,  mapping,  geometryQuery,  expr,  selectSingle)
        {
        }
        protected override string GetTableName(Type type)
        {
            return Mapping.GetTableName(type.Name);
        }
        protected override string ExtendColumns
        {
            get
            {
                return base.ExtendColumns;
            }
        }

        protected override string RowExenstion
        {
            get {  return (SelectSingle ? " AND ROWNUM =1 " : string.Empty); }
        }
        
    }
}
