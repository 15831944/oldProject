using System.Collections.Generic;
using System.Linq;
using ElectronTransferModel.Base;
using ElectronTransferModel.Config;
using ElectronTransferFramework;

namespace ElectronTransferDal.OracleDal
{
    internal class InsertPlainGeometryQueryBuilder : GeometryQueryBuilder
    {

        public InsertPlainGeometryQueryBuilder(ElectronSymbol symbol, IEnumerable<string> avoidFields,
                                               SimpleMapping mapping)
            : base(symbol, avoidFields, mapping)
        {
        }

        public override string BuildQueryString()
        {
            ObjectFields = Symbol.GetType().GetProperties().Where(o=>!AvoidFields.Contains(o.Name)&& !o.PropertyType.IsSubclassOf(typeof(ElectronTransferModel.Geo.Geometry)) ).Select(o=>o.Name).ToList();
            GeometryFields = new string[] { "SDO_X1", "SDO_Y1", "SDO_X2", "SDO_Y2", "SDO_X3", "SDO_Y3", "SDO_X4", "SDO_Y4", "SDO_ORIENTATION", "SDO_SEQ", "SDO_ESEQ", "SDO_ETYPE", "GDO_ATTRIBUTES" };
            var tableName = Mapping.GetUpdateView(this.Type.Name);
            
            //Fields.AddRange(new string[] { "SDO_X1", "SDO_Y1", "SDO_X2", "SDO_Y2", "SDO_X3", "SDO_Y3", "SDO_X4", "SDO_Y4", "SDO_ORIENTATION" });
            string sql = string.Format("INSERT INTO {0} ({1}) VALUES ({2})",tableName,
                string.Join(",", AllFields.ToArray()),
                string.Join(",", AllFields.Select(o => ":" + o).ToArray())
                );
            return sql;
            /*
            List<Point> points = new List<Point>();
            if (Symbol.G3E_GEOMETRY is Point)
            {
                Point point = Symbol.G3E_GEOMETRY as Point;
                points.Add(point);
            }
            else if (Symbol.G3E_GEOMETRY is Multipoint)
            {
                Multipoint multipoint = Symbol.G3E_GEOMETRY as Multipoint;
                //points.Add(multipoint.Points);
            }
            else if (Symbol.G3E_GEOMETRY is LineString)
            {
                LineString lineString = Symbol.G3E_GEOMETRY as LineString;
                points.AddRange(lineString.Points);
            }
            else if (Symbol.G3E_GEOMETRY is Polygon)
            {
                Polygon polygon = Symbol.G3E_GEOMETRY as Polygon;
                points.AddRange(polygon.Lines.First().Points);
            }

            int step = 4;

            for (int page = 0; page < (points.Count + step/2)/4; page++)
            {
                var items = points.Skip(page*step).Take(step);
                for (int i = 0; i < items.Count(); i++)
                {
                    string viewName = Mapping.GetUpdateView(Symbol.GetType().Name);

                    //string sql = string.Format("INSERT INTO {0} ({1}) VALUES {2}",viewName);
                }
            }
            */
            //string sql;
        }
    }

}
