using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ElectronTransferModel.Geo
{
    [Serializable]
    public abstract class Geometry
    {
        public static egtype GetGeometryType(string text) 
        {
            string thisText = text.ToUpper();
            if (IsLineString(thisText))
                return egtype.linestring;
            else if (IsMultiPoint(thisText))
                return egtype.multipoint;
            else if (IsPoint(thisText))
                return egtype.point;
            else if (IsPolygon(thisText))
                return egtype.polygon;
            else return egtype.none;
        }
        public static bool IsLineString(string text) 
        {
            return text.Contains("LINESTRING");
        }

        public static bool IsPoint(string text) 
        {
            return text.Contains("POINT") && !IsMultiPoint(text);
        }

        public static bool IsMultiPoint(string text) 
        {
            return text.Contains("MULTIPOINT");
        }

        public static bool IsPolygon(string text) 
        {
            return text.Contains("POLYGON");
        }

        public abstract egtype GeometryType { get; }
    }
}
