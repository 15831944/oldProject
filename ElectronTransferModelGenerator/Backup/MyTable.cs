using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ElectronTransferModelGenerator
{
    public class MyTable
    {
        public MyTable()
        {
            Columns = new List<MyColumn>();
            PrimaryKey = new List<MyColumn>();
        }

        private bool? _hasGeometry;
        public string Name { get; set; }
        public string ClassName { get; set; }
        public string User { get; set; }
        public string UpdateView { get; set; }
        public List<MyColumn> Columns { get; private set; }

        public List<MyColumn> PrimaryKey { get; set; }

        public bool HasGeometry {
            get
            {
                if (!_hasGeometry.HasValue)
                    _hasGeometry = Columns.Exists(o => o.TypeName == "Geometry");
                return _hasGeometry.Value;
            }
        }
    }
}
