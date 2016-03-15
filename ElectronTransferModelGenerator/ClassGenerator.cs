using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;
//using MySql.Data.MySqlClient;
using ElectronTransferModel.Config;


namespace ElectronTransferModelGenerator
{
    public abstract class ClassGenerator
    {
        private string _dir;
        protected string Host;
        protected string User;
        protected string Password;
        protected string Db;
        protected string ConnectString;
        private string _nameOfNameSpace;
        public ClassGenerator(string dir, string nameOfNameSpace)
        {
            _dir = dir;
            _nameOfNameSpace = nameOfNameSpace;
            ConnectString = GetConnectString();
            Initialize();
        }
        protected abstract void Initialize();
        protected abstract IEnumerable<MyColumn> GetPrimayColumns(MyTable table);
        protected abstract bool IsSelectOnly(MyColumn column,IList<MyColumn>columns );

        void WriteImport(StreamWriter writer)
        {
            writer.WriteLine("using System;");
            writer.WriteLine("using ElectronTransferModel.Base;");
            writer.WriteLine("using ElectronTransferModel.Geo;");
            writer.WriteLine();
        }
        void StartNameSpace(StreamWriter writer,string nameOfNameSpace) 
        {
            writer.WriteLine( string.Format("namespace {0}",nameOfNameSpace ));
            writer.WriteLine("{");
        }
        void EndNameSpace(StreamWriter writer)
        {
            
            writer.WriteLine("}");
        }
        void StartClass(StreamWriter writer,MyTable table) 
        {
            var hasGeometry = table.HasGeometry;
            var hasG3EId = table.Columns.Exists(o => o.Name == "G3E_ID");
            writer.WriteLine("    [Serializable]");
            writer.WriteLine(string.Format("    public class {0} : {1}", table.ClassName, hasGeometry ? "ElectronSymbol" : ( hasG3EId? "ElectronBase":"DBEntity")));
            writer.WriteLine("    {");
        }
        void EndClass(StreamWriter writer)
        {
            writer.WriteLine("    }");
        }
        void WriteProperty(StreamWriter writer,MyColumn column)
        {
            List<string> avoid=new List<string>(new string[] {  "G3E_FID", "G3E_ID","G3E_FNO" });
            if (column.Table.HasGeometry)
            {
                avoid.Add("G3E_GEOMETRY");
            }
            if (!column.Table.ClassName.ToUpper().StartsWith("CD_") && avoid.Contains(column.Name))
                return;
            if (column.Table.PrimaryKey.Contains(column))
            {
                writer.WriteLine("        [KeyField]");
            }
            if (column.IsSelectOnly)
            {
                writer.WriteLine("        [SelectOnly]");
            }
            if (column.NumericPrecision.HasValue)
            {
                writer.WriteLine(string.Format("        [Precision({0})]", column.NumericPrecision));
            }

            writer.WriteLine(string.Format("        public {0}{1} {2} {{ get; set; }}", column.TypeName,column.IsNullable&&column.TypeName!="string"  ?"?":string.Empty, column.Name));
            
        }
        void EndProperty() 
        { }
        void Generate(MyTable table)
        {
            string path = Path.Combine(_dir, table.ClassName+".cs");
            using (StreamWriter writer = new StreamWriter(path)) 
            {
                WriteImport( writer );
                StartNameSpace(writer,_nameOfNameSpace);
                StartClass(writer, table);
                foreach (var column in table.Columns) 
                {
                    WriteProperty(writer, column);
                }
                EndClass(writer);
                EndNameSpace(writer);
            }
        }
        protected abstract string GetConnectString();
        protected abstract IEnumerable<MyTable> GetTables();
        protected abstract void FillTableDescription(MyTable table);
        //protected virtual IEnumerable<MyTable> GetTables() 
        //{
        //    using (MySqlConnection conn = new MySqlConnection(_connectString))
        //    {
                
        //        MySqlCommand cmd = new MySqlCommand("show tables", conn);
        //        conn.Open();
        //        var reader = cmd.ExecuteReader();
        //        while (reader.Read()) 
        //        {
        //            var tablename = reader[0].ToString();
        //            if ( !tablename.EndsWith("tb") ) continue;
        //            tablename = tablename.Replace("_tb",string.Empty);
        //            var className = tablename.ToLower();
        //            className = className.Replace("b_",string.Empty);
        //            className = (className[0] + string.Empty).ToUpper() + className.Substring(1);
        //            yield return new MyTable { ClassName = className , Name = tablename};
        //        }
        //    }
        //}
        //protected virtual void FillTableDescription(MyTable table) 
        //{
        //    using (MySqlConnection conn = new MySqlConnection(_connectString))
        //    {
        //        MySqlCommand cmd = new MySqlCommand("select colname,coltype,isnull from "+table.Name+"_tb", conn);
        //        try
        //        {
        //            conn.Open();
        //            var reader = cmd.ExecuteReader();
        //            while (reader.Read())
        //            {
        //                var typename = reader[1].ToString();
        //                if (typename == "GEOMETRY") typename = "Geometry";
        //                table.Columns.Add(new MyColumn { Name = reader[0].ToString(), TypeName = typename, IsNullable = reader[2].ToString() == "Y" && typename != "Geometry" });
        //            }
        //        }
        //        catch (MySqlException e) 
        //        {
        //            Console.WriteLine(table.Name);
        //        }
        //    }
        //}
        
        public void GenerateAll() 
        {
            SimpleMapping mapping = new SimpleMapping();
            foreach (var table in GetTables()) 
            {
                FillTableDescription(table);
                Generate(table);
                mapping.Add(new SimpleMappingPair { TableName = table.Name, ClassName = table.ClassName, UpdateView = table.UpdateView});
            }
            mapping.Save(SimpleMapping.MAP_FILE);

        }

        
        
        
    }

}
