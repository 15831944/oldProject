using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using ElectronTransferModel.Config;

namespace ElectronTransferDal.Common
{
    public abstract class QueryBuilder:IQueryBuilder
    {
        private const string SEPERATOR = ":";
        protected readonly Type Type;
        protected readonly Expression Expr;
        protected readonly GeometryQuery GeometryQuery;
        protected readonly SimpleMapping Mapping;
        protected readonly IEnumerable<string> AvoidFields;
        protected readonly bool SelectSingle;
        private string _text;
        public QueryBuilder(Type type, IEnumerable<string> avoidFields, SimpleMapping mapping, GeometryQuery geometryQuery, Expression expr, bool selectSingle)
        {
            Type = type;
            AvoidFields = avoidFields;
            Mapping = mapping;
            GeometryQuery = geometryQuery;
            Expr = expr;
            SelectSingle = selectSingle;
        }

        public abstract string BuildQueryString();

        public override string ToString()
        {
            if (string.IsNullOrEmpty(_text))
                _text = BuildQueryString();
            return _text;
        }

        protected virtual string Seperator
        {
            get { return SEPERATOR; }
        }
    }
}
