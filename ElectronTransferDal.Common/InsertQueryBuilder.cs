using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using ElectronTransferModel.Config;

namespace ElectronTransferDal.Common
{
    class InsertQueryBuilder:QueryBuilder
    {
        private object _entity;
        private bool _byView;
        public IList<string> Fields { get;private set; }
        public InsertQueryBuilder(object entity,Type type, IEnumerable<string> avoidFields, SimpleMapping mapping, GeometryQuery geometryQuery,bool byView)
            : base(type, avoidFields, mapping, geometryQuery, null, false)
        {
            _entity = entity;
            _byView = byView;
        }

        public override string BuildQueryString()
        {
            var properties = _entity.GetType().IsAnonymous() ? _entity.GetType().GetProperties() : this.Type.GetProperties().Where(o=> !AvoidFields.Contains(o.Name));
            Fields = properties.Select(o => o.Name).ToList();


            var hasGeometry = properties.Any(o => o.Name == this.GeometryQuery.GeometryField);
            var parameterStrings = Fields.Select(o => Seperator + o).ToArray();
            return string.Format("INSERT INTO {0} ({1}) VALUES ({2})", _byView? Mapping.GetUpdateView(Type.Name):Mapping.GetTableName(Type.Name),
                string.Join(",", properties.Select(o => o.Name).ToArray()),
                string.Join(",", parameterStrings));


            
        }
        
    }
}
