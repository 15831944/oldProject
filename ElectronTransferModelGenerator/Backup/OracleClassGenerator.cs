using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Oracle.DataAccess.Client;
using System.Data;
using System.Configuration;

namespace ElectronTransferModelGenerator
{
    class OracleClassGenerator:ClassGenerator
    {
        Dictionary<string, string> _typeMap = new Dictionary<string, string>();
        public OracleClassGenerator(string dir, string nameOfNameSpace)
            : base(dir, nameOfNameSpace)
        {

        }
        protected override void Initialize()
        {
            _typeMap.Add("VARCHAR2", "string");
            _typeMap.Add("VARCHAR", "string");
            _typeMap.Add("NVARCHAR2", "string");
            _typeMap.Add("NVARCHAR", "string");
            _typeMap.Add("CHAR", "string");
            _typeMap.Add("DATE", "DateTime");
            _typeMap.Add("SDO_GEOMETRY", "Geometry");
            _typeMap.Add("NUMBER", "long");
        }
        string GetTypeName(string typeInDb) 
        {
            try
            {
                return _typeMap[typeInDb];
            }
            catch(KeyNotFoundException ex )
            {
                return "object";
            }
        }
        protected override IEnumerable<MyColumn> GetPrimayColumns(MyTable table)
        {
            using (OracleConnection conn = new OracleConnection(GetConnectString()))
            {
                var cmd = conn.CreateCommand();
                cmd.CommandText = string.Format("SELECT CONSTRAINT_NAME FROM ALL_CONSTRAINTS WHERE TABLE_NAME = '{0}' AND CONSTRAINT_TYPE ='P' AND OWNER = '{1}'", table.Name.Split('.').Last(), table.Name.Split('.').First());
                conn.Open();
                var reader=cmd.ExecuteReader();
                if (reader.Read())
                {
                    var constraintName = reader[0].ToString();
                    var secondCmd = conn.CreateCommand();
                    secondCmd.CommandText =
                        string.Format("SELECT COLUMN_NAME FROM ALL_CONS_COLUMNS WHERE CONSTRAINT_NAME = '{0}'",
                                      constraintName);
                    var secondReader = secondCmd.ExecuteReader();
                    while (secondReader.Read())
                    {
                        yield return table.Columns.SingleOrDefault(o => o.Name == secondReader[0].ToString());
                    }

                }
                
            }
        }

        private IList<MyColumn> GetSelectOnlyColumns(DataTable tableSchema, DataTable viewSchema)
        {
            List<MyColumn> columns=new List<MyColumn>();
            var tableColumns = tableSchema.Rows.Cast<DataRow>().Select(o => o["ColumnName"].ToString());
            var viewColumns = viewSchema.Rows.Cast<DataRow>().Select(o => o["ColumnName"].ToString());
            return tableColumns.Where(o => !viewColumns.Contains(o)).Select(o => new MyColumn { Name = o }).ToList();
            
        }

        private DataTable GetUpdateViewColumns(OracleDataReader reader)
        {
            DataTable dataTable = new DataTable();
            dataTable.Columns.Add("ColumnName");
            while (reader.Read())
            {
                var row=dataTable.NewRow();
                row["ColumnName"] = reader["COLUMN_NAME"];
                dataTable.Rows.Add(row);
                //dataTable.Rows.Add(new DataRow() {new object[] {reader["COLUMN_NAME"]}});
            }
            return dataTable;
        }

        protected override void FillTableDescription(MyTable table)
        {
            using (OracleConnection conn = new OracleConnection(GetConnectString()))
            {
                var cmd = conn.CreateCommand();
                var isGeometry = table.Name.EndsWith("_SDOGEOM");
                var user = table.User;//isGeometry ? "CADGIS" : "GZDLGIS";
                cmd.CommandText = string.Format("SELECT COLUMN_NAME,DATA_TYPE,OWNER FROM ALL_TAB_COLUMNS WHERE TABLE_NAME='{0}' AND OWNER ='{1}'", table.Name, user);
                table.Name = string.Format("{0}.{1}", user, table.Name);
                conn.Open();
                var reader = cmd.ExecuteReader();
                var columns = new List<string>();
                while (reader.Read())
                {
                    var columnName = reader["COLUMN_NAME"].ToString();
                    if (reader["DATA_TYPE"].ToString() == "SDO_GEOMETRY") 
                    {
                        table.Columns.Add(new MyColumn { Name = columnName, TypeName = GetTypeName("SDO_GEOMETRY"),Table = table});
                    }
                    else 
                        columns.Add(reader["COLUMN_NAME"].ToString());
                }
                //"_SDOGEOM"
                string sql = string.Format("SELECT {0} FROM {1}", string.Join(",", columns.ToArray()), table.Name);
                string updateViewSql = string.Format("select COLUMN_NAME from all_tab_columns where table_name ='{0}'", table.UpdateView);
                //conn.Open();
                var testCmd = conn.CreateCommand();
                testCmd.CommandText = sql;
                var testReader = testCmd.ExecuteReader();

                var testUpdateViewCmd = conn.CreateCommand();
                testUpdateViewCmd.CommandText = updateViewSql;
                var testUpdateViewReader = testUpdateViewCmd.ExecuteReader();
                
                var schemaTable = testReader.GetSchemaTable();

                DataTable updateViewSchemaTable=null;
                if (testUpdateViewReader!=null)
                    updateViewSchemaTable = GetUpdateViewColumns(testUpdateViewReader);
                IList<MyColumn> selectOnlyColumns = updateViewSchemaTable != null
                                                        ? GetSelectOnlyColumns(schemaTable
                                                                               , updateViewSchemaTable)
                                                        : new List<MyColumn>();

                if (schemaTable != null)
                    foreach (DataRow row in schemaTable.Rows) 
                    {
                        var dataType = row["DataType"] as Type;
                        var isNull = ((bool)row["AllowDBNull"]) && dataType != typeof(string);
                        var column = new MyColumn
                            {
                                Name = row["ColumnName"].ToString(),
                                TypeName = dataType.ToString(),
                                IsNullable = isNull,
                                Table = table,
                                IsKey = !row.IsNull("IsKey")&&(bool)row["IsKey"]        
                                
                            };
                        
                        if (row.IsNull("NumericPrecision"))
                            column.NumericPrecision = null;
                        else column.NumericPrecision = Math.Min((short)row["NumericPrecision"],(short)29);
                        column.IsSelectOnly = IsSelectOnly(column, selectOnlyColumns);
                        table.Columns.Add(column);
                    }
                /*
                 * COLUMN_NAME
                 * DATA_TYPE
                 * DATA_LENGTH
                 * NULLABLE
                 */
                //while (reader.Read()) 
                //{
                //    var dataType = reader["DATA_TYPE"].ToString();
                //    decimal? scale = reader["DATA_SCALE"] as decimal?;
                //    string typename = (dataType == "NUMBER" && scale.HasValue && (int)(decimal)reader["DATA_SCALE"] > 0) ? "double":GetTypeName(dataType);
                    
                //    MyColumn column = new MyColumn 
                //    {
                //        Name = reader["COLUMN_NAME"].ToString(),
                //        TypeName = typename,
                //        IsNullable = reader["NULLABLE"].ToString() == "Y" && typename != "Geometry"
                        
                //    };
                    
                //    table.Columns.Add(column);
                //}
            }
            table.PrimaryKey.AddRange(GetPrimayColumns(table));
            table.Columns.Sort((o1, o2) => o1.Name.CompareTo(o2.Name));
        }
        protected override string GetConnectString()
        {
            return ConfigurationManager.ConnectionStrings["db"].ConnectionString;
            //return "User Id=GZDLGIS;Password=bcc;Data Source=192.168.1.101/DGGIS15";
            //return string.Format("User Id={0};Password={1};Data Source={2}/{3}", User, Password,Host, Db);
        }

        private bool IsValid(string tableName)
        {
            if (tableName.EndsWith("_SDOGEOM"))
            {
                using (OracleConnection conn = new OracleConnection(GetConnectString()))
                {
                    var cmd = conn.CreateCommand();
                    cmd.CommandText =
                        string.Format(
                            "SELECT count(*) FROM ALL_TABLES WHERE ( OWNER = 'CADGIS' OR OWNER = 'GZDLGIS' ) AND TABLE_NAME = '{0}'",
                            tableName);
                    conn.Open();

                    return 2 == (decimal) cmd.ExecuteScalar();
                }
            }
            return true;
        }

        protected override IEnumerable<MyTable> GetTables()
        {
            using (OracleConnection conn = new OracleConnection(GetConnectString()))
            {
                var cmd = conn.CreateCommand();
                cmd.CommandText = "select table_name ,OWNER from all_tables where ( table_name like 'B$%' OR table_name like 'CD_%' ) and not table_name like '%_TMP%' and not table_name like '%_BAK%' and ( OWNER ='CADGIS' OR OWNER ='GZDLGIS' )";
                try
                {
                    conn.Open();
                }
                catch (Exception ex) 
                {
                }
                List<MyTable>tables=new List<MyTable>();
                var reader = cmd.ExecuteReader();
                while (reader.Read())
                {
                    var tablename = reader["table_name"].ToString();
                    //string prefix = tablename.EndsWith("_SDOGEOM") ? "cadgis." : string.Empty;
                    var available = IsValid(tablename);
                    var className = tablename.ToLower();
                    className = className.Replace("b$",string.Empty);
                    className = (className[0] + string.Empty).ToUpper() + className.Substring(1);
                    if (available)
                        tables.Add(new MyTable { Name= tablename , ClassName = className,User = reader["OWNER"].ToString(),UpdateView = tablename.Replace("B$",string.Empty)});
                    foreach (var table in tables.Where(o=>o.User=="CADGIS").ToArray())
                    {
                        var found = tables.SingleOrDefault(o => o.Name == table.Name && o.User == "GZDLGIS");
                        if(found!=null)
                            tables.Remove(found);
                    }
                    
                }
                return tables;
            }
            
        }

        protected override bool IsSelectOnly(MyColumn column,IList<MyColumn>columns )
        {
            return columns.Contains(column);
        }
    }
}
