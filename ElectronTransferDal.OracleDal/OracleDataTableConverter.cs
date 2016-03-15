
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using ElectronTransferDal.Common;
using Oracle.DataAccess.Client;
using Oracle.DataAccess.Types;
using System.Reflection;
using System.Data.Common;
namespace ElectronTransferDal.OracleDal
{
    class OracleDataTableConverter : DataTableConverter
    {
        protected override object GetValue(System.Data.Common.DbDataReader reader, PropertyInfo property)
        {

            var oracleReader = (reader as OracleDataReader);
            int ord = reader.GetOrdinal(property.Name);
            if (reader.IsDBNull(ord))
            {
                return null;
            }
         
            if (oracleReader.GetFieldType(ord) == typeof(decimal))
            {
                return GetDecimal(oracleReader, ord, property);
            }
            else
            {
                return oracleReader[property.Name];
            }
        }
        protected override object GetValue(DbDataReader reader, string field)
        {
            const int PRICISION_LIMIT = 19;
            var oracleReader = (reader as OracleDataReader);
            int ord = reader.GetOrdinal(field);
            if (reader.IsDBNull(ord))
            {
                return null;
            }
            if (oracleReader.GetFieldType(ord) == typeof(decimal))
            {
                return (decimal)(OracleDecimal.SetPrecision(oracleReader.GetOracleDecimal(ord), PRICISION_LIMIT));
            }
            else
            {
                return base.GetValue(reader, field);
            }
        }
        private decimal GetDecimal(OracleDataReader reader, int index,PropertyInfo property) 
        {
            return (decimal)(OracleDecimal.SetPrecision(reader.GetOracleDecimal(index), PrecisionCache.Instance[property]));
        }
    }
}
