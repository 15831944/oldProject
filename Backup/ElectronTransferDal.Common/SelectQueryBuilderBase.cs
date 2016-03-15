using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using ElectronTransferModel.Config;
using System.Linq.Expressions;

namespace ElectronTransferDal.Common
{
    abstract class SelectQueryBuilderBase: QueryBuilder
    {

        public SelectQueryBuilderBase(Type type, IEnumerable<string> avoidFields, SimpleMapping mapping, GeometryQuery geometryQuery, Expression expr, bool selectSingle)
            :base(type,avoidFields,mapping,geometryQuery, expr, selectSingle)
        {
        }

        protected virtual string ExtendColumns 
        {
            get
            {
                return string.Empty;
            }
        }

        public override string BuildQueryString()
        {
            const string format = "SELECT {0}{3} FROM {1} {2}";
            var fields = string.Join(",", this.Type.GetProperties().Where(o => !this.AvoidFields.Contains(o.Name) && o.Name != this.GeometryQuery.GeometryField).Select(o => o.Name).ToArray());
            bool hasGeometry = this.Type.GetProperties().Any(o => o.Name == this.GeometryQuery.GeometryField);
            if (hasGeometry && GeometryQuery != null) fields += (GeometryQuery.GeometryQueryText);
            var tablename = GetTableName(this.Type);
            return string.Format(format, fields, tablename, Expr == null ? string.Empty : " WHERE " + new ConditionVisitor(Expr) +RowExenstion,ExtendColumns);
        }

        protected abstract string RowExenstion { get; }
        protected abstract string GetTableName(Type type);
        
    }
}
