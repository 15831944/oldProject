using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using MySql.Data.MySqlClient;

namespace ElectronTransferModelGenerator
{
    class MySqlClassGenerator:ClassGenerator
    {
        public MySqlClassGenerator(string dir, string nameOfNameSpace, string host, string user, string password, string db)
            : base(dir, nameOfNameSpace, host, user, password, db)
        {
            
        }
        protected override void Initialize()
        {
            throw new NotImplementedException();
        }
        protected sealed override IEnumerable<MyTable> GetTables()
        {
            using (MySqlConnection conn = new MySqlConnection(GetConnectString()))
            {

                MySqlCommand cmd = new MySqlCommand("show tables", conn);
                conn.Open();
                var reader = cmd.ExecuteReader();
                while (reader.Read())
                {
                    var tablename = reader[0].ToString();
                    if (!tablename.EndsWith("tb")) continue;
                    tablename = tablename.Replace("_tb", string.Empty);
                    var className = tablename.ToLower();
                    className = className.Replace("b_", string.Empty);
                    className = (className[0] + string.Empty).ToUpper() + className.Substring(1);
                    yield return new MyTable { ClassName = className, Name = tablename };
                }
            }
            return null;
        }
        protected sealed override string GetConnectString()
        {
            return String.Format("server={0};user id={1}; password={2}; database={3}; pooling=false", Host, User, Password, Db);
        }
        protected sealed override void FillTableDescription(MyTable table)
        {
            using (MySqlConnection conn = new MySqlConnection(GetConnectString()))
            {
                MySqlCommand cmd = new MySqlCommand("select colname,coltype,isnull from " + table.Name + "_tb", conn);
                try
                {
                    conn.Open();
                    var reader = cmd.ExecuteReader();
                    while (reader.Read())
                    {
                        var typename = reader[1].ToString();
                        if (typename == "GEOMETRY") typename = "Geometry";
                        table.Columns.Add(new MyColumn { Name = reader[0].ToString(), TypeName = typename, IsNullable = reader[2].ToString() == "Y" && typename != "Geometry" });
                    }
                }
                catch (MySqlException e)
                {
                    Console.WriteLine(table.Name);
                }
            }
        }
    }
}
