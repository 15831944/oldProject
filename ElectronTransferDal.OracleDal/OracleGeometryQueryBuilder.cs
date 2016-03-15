using System;
using System.Collections.Generic;
using System.Linq;
using ElectronTransferDal.Common;
using ElectronTransferModel.Base;
using ElectronTransferModel.Config;

namespace ElectronTransferDal.OracleDal
{
    class OracleGeometryQueryBuilder //: IQueryBuilder
    {
        Type _type;
        SimpleMapping _mapping;
        DBEntity _entity;
        string _geometryField;
        public OracleGeometryQueryBuilder(Type type, SimpleMapping mapping, DBEntity entity, string geometryField)
        {
            _type = type;
            _mapping = mapping;
            _entity = entity;
            _geometryField = geometryField;
        }
        public override string ToString()
        {
            var tableName = _mapping.GetTableName(_type.Name);
            IList<string> expressions = new List<string>();
            
            foreach (var keyField in KeyFieldCache.Instance[_type]) 
            {
                expressions.Add(string.Format("{0} = {1}", keyField.Name, keyField.GetValue(_entity, null)));
            }

            return string.Format("SELECT {0} FROM {1} WHERE {2}",_geometryField,tableName,string.Join(" AND ",expressions.ToArray()));
        }

    }
}
