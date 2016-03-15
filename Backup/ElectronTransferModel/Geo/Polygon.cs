using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;

namespace ElectronTransferModel.Geo
{
    [Serializable]
    public class Polygon : Geometry
    {
        private List<LineString> _lines = new List<LineString>();
        public Polygon()
        {
            _lines.Add(new LineString());
        }
        public LineString UniqueLineString
        {
            get
            {
                return _lines.FirstOrDefault(o => o.Points.Count != 0);
            }
        }
        public List<LineString> Lines { get { return _lines; } }
        public static Polygon Parse(string text,char pointSeparator,char componentSeparator)
        {
            try
            {
                Polygon polygon = new Polygon();
                //var match = Regex.Match(text, @"\(\((.+)\)\)");
                var body = text;
                //var body = match.Groups[1].Value;
                var lineString = new LineString();
                foreach (var pointBody in body.Split(pointSeparator))
                {
                    var components = pointBody.Split(componentSeparator).Select(o => double.Parse(o));
                    lineString.Points.Add(new Point (components.ToArray()));
                }
                polygon.Lines.Add(lineString);
                return polygon;
            }
            catch (Exception ex)
            {
                throw new GeometryException(text, ex);
            }
        }
       

        public override string ToString()
        {
            return string.Format("POLYGON({0})",
                string.Join(",",_lines.Select(line => string.Format("({0})", string.Join(",", line.Points.Select(o => string.Format("{0} {1}", o.X, o.Y)).ToArray()))).
                ToArray()));
        }

        public override egtype GeometryType
        {
            get { return egtype.polygon; }
        }

    
    }
}
