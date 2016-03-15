using System.Data;
using System.Data.Common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Reflection;

namespace ElectronTransferDal.Common
{
    public class DataTableConverter
    {
        private DataColumn[] GetColumns(DbDataReader reader) 
        {
            var schema = reader.GetSchemaTable();
            return schema.Rows.Cast<DataRow>().Select(o => new DataColumn { ColumnName = o["ColumnName"].ToString(), DataType = o["DataType"] as Type }).ToArray();
        }
        public DataTable Convert(DbDataReader reader,PropertyInfo[] properties)
        {
            DataTable table = new DataTable();
            var columns = GetColumns(reader);
            table.Columns.AddRange(columns);
            while(reader.Read())
            {
                var row = table.NewRow();
                foreach (var column in columns) 
                {
                    var property = properties.FirstOrDefault(o=>o.Name == column.ColumnName);
                    var value = (property != null)?GetValue(reader, property):GetValue(reader, column.ColumnName);
                    row[column.ColumnName] = (value != null) ? value: DBNull.Value;   
                }
                table.Rows.Add(row);

            }
            return table;
        }

        protected virtual object GetValue(DbDataReader reader, PropertyInfo property) 
        {
            return reader[property.Name];
        }

        protected virtual object GetValue(DbDataReader reader, string field) 
        {
            return reader[field];
        }


    }
}
