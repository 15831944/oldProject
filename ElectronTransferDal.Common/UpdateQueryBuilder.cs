using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using ElectronTransferModel.Config;

namespace ElectronTransferDal.Common
{
    class UpdateQueryBuilder:QueryBuilder
    {
        private object _entity;
        private bool _byView;
        public  IList<string> Fields { get; private set; }
        //public IList<string> Fields {
        //    get { return _fields; }
        //    private set { _fields = value; }
        //}
        public UpdateQueryBuilder(object entity,Type type, IEnumerable<string> avoidFields, SimpleMapping mapping, GeometryQuery geometryQuery,bool byView) : base(type, avoidFields, mapping, geometryQuery, null, true)
        {
            _entity = entity;
            _byView = byView;
        }

        public override string BuildQueryString()
        {
            var properties = _entity.GetType().IsAnonymous() ? _entity.GetType().GetProperties() : this.Type.GetProperties().Where(o => !AvoidFields.Contains(o.Name));
            var keyFields = KeyFieldCache.Instance.FindKeyFields(this.Type);
            this.Fields = properties.Where(o => !keyFields.Contains( o)).Concat(keyFields).Select(o=>o.Name).ToList();

            //to do
            var hasGeometry = properties.Any(o => o.Name == this.GeometryQuery.GeometryField);
            if (hasGeometry)
            {
                //    for (int i = 0; i < parameterStrings.Length; i++)
                //    {
                //        if (parameterStrings[i] == "?G3E_GEOMETRY")
                //        {
                //            parameterStrings[i] = "GeomFromText(?G3E_GEOMETRY)";
                //        }
                //    }
            }
            //var conditionVisitor = new ConditionVisitor(this.Expr);
            //key field must be not update field
            
            //foreach (var propertyInfo in keyFields)
            //{
            //    this.Fields.Add(propertyInfo.Name);
            //}
            var condition = string.Join(" AND ", keyFields.Select(o => string.Format("{0}={1}{0}", o.Name, Seperator)).ToArray());
            return string.Format("UPDATE {0} SET {1} WHERE {2} ", _byView? Mapping.GetUpdateView(Type.Name):Mapping.GetTableName(this.Type.Name),
                string.Join(",", properties.Where(o=>!keyFields.Contains(o)).Select(o => string.Format( "{0}={1}{0}",o.Name,Seperator)).ToArray()),
                condition);


        }
    }
}
