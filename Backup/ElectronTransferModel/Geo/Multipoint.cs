using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;

namespace ElectronTransferModel.Geo
{
    [Serializable]
    public class Multipoint : Geometry
    {
        private List<Point> _points = new List<Point>();

        public List<Point> Points { get { return _points; } }
        public static Multipoint Parse(string text,char pointSeparator,char componentSeparator)
        {
            try
            {
                Multipoint multipoint = new Multipoint();
                //var match = Regex.Match(text, @"\((.+)\)");
                //var body = match.Groups[1].Value;
                var body = text;
                foreach (var pointBody in body.Split(pointSeparator/*','*/))
                {
                    var components = pointBody.Split(componentSeparator/*' '*/).Select(o => double.Parse(o));
                    multipoint.Points.Add(new Point (components.ToArray()));
                }
                return multipoint;
            }
            catch (Exception ex)
            {
                throw new GeometryException(text, ex);
            }
        }
        

        public override string ToString()
        {
            return string.Format("MULTIPOINT({0})", string.Join(",", _points.Select(o => string.Format("{0} {1}", o.X, o.Y)).ToArray()));
        }

        public override egtype GeometryType
        {
            get { return egtype.multipoint;
            }
        }
       
    }
}
