using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using ElectronTransferDal.Common;
using ElectronTransferFramework;
using ElectronTransferModel.Base;
using ElectronTransferModel.Config;

namespace ElectronTransferDal.OracleDal
{
    class UpdateSdoGeometryQueryBuilder:GeometryQueryBuilder
    {
        public UpdateSdoGeometryQueryBuilder(ElectronSymbol symbol, IEnumerable<string> avoidFields, SimpleMapping mapping) : base(symbol, avoidFields, mapping)
        {
        }

        public override string BuildQueryString()
        {
            GeometryFields = new string[] { "G3E_GEOMETRY" };
            ObjectFields = Symbol.GetPropertyNames().Where(o=>!AvoidFields.Contains(o) && o!="G3E_FID" && !GeometryFields.Contains(o)).ToList();
            
            var tableName = Mapping.GetTableName(this.Type.Name);
            Expression<Func<ElectronSymbol, bool>> conditionExpression = o => o.G3E_FID == Symbol.G3E_FID;
            var conditionVisitor = new ConditionVisitor(conditionExpression);
            //Fields.AddRange(new string[] { "SDO_X1", "SDO_Y1", "SDO_X2", "SDO_Y2", "SDO_X3", "SDO_Y3", "SDO_X4", "SDO_Y4", "SDO_ORIENTATION" });
            string sql = string.Format("UPDATE {0} SET {1} WHERE {2}", tableName,
                                       string.Join(",", AllFields.Select(o => string.Format("{0}=:{0}", o)).ToArray()),
                                       conditionVisitor
                                       );
            return sql;
        }
    }
}
