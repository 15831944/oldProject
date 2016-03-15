using System.Collections.Generic;
using System.Linq;
using ElectronTransferFramework;
using ElectronTransferModel.Base;
using ElectronTransferModel.Config;

namespace ElectronTransferDal.OracleDal
{
    internal class InsertSdoGeometryQueryBuilder : GeometryQueryBuilder
    {
        public InsertSdoGeometryQueryBuilder(ElectronSymbol symbol, IEnumerable<string> avoidFields,
                                             SimpleMapping mapping)
            : base(symbol, avoidFields, mapping)
        {
        }

        public override string BuildQueryString()
        {
            GeometryFields = new string[] { "G3E_GEOMETRY" };
            ObjectFields = Symbol.GetPropertyNames().Where(o => !AvoidFields.Contains(o) && !GeometryFields.Contains(o)).ToList();
            
            var tableName = Mapping.GetTableName(this.Type.Name);

            //Fields.AddRange(new string[] { "SDO_X1", "SDO_Y1", "SDO_X2", "SDO_Y2", "SDO_X3", "SDO_Y3", "SDO_X4", "SDO_Y4", "SDO_ORIENTATION" });
            string sql = string.Format("INSERT INTO {0} ({1}) VALUES ({2})", tableName,
                                       string.Join(",", AllFields.ToArray()),
                                       string.Join(",", AllFields.Select(o => ":" + o).ToArray())
                );
            return sql;
        }
    }
}
