using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using ElectronTransferDal.Common;
using ElectronTransferModel.Geo;
using System.Diagnostics;

namespace ElectronTransferDal
{
    public class MySqlDBManager:RDBManagerBase
    {
        protected override string GetDBName()
        {
            return "mysql";
        }

        protected override GeometryQuery GeometryQuery
        {
            get { return new MySqlGeometryQuery(); }
        }

        protected override object GetValue(System.Data.Common.DbDataReader reader, string fieldName)
        {
            if (reader[fieldName] is DBNull) return null;
            return reader[fieldName];
        }

        protected override Geometry GetGeometry(string valueString)
        {
            Geometry geometry = null;
            try
            {
                switch (Geometry.GetGeometryType(valueString))
                {

                    case egtype.point:
                        geometry = Point.Parse(valueString);
                        break;
                    case egtype.multipoint:
                        geometry = Multipoint.Parse(valueString, ',', ' ');
                        break;
                    case egtype.linestring:
                        geometry = LineString.Parse(valueString, ',', ' ');
                        break;
                    case egtype.polygon:
                        geometry = Polygon.Parse(valueString, ',', ' ');
                        break;
                    case egtype.none:
                    default:
                        break;
                }
            }
            catch (GeometryException ex)
            {
                Debug.Write(ex);
                //log it later
            }
            return geometry;
        }

        protected override void MakeGeometry(ElectronTransferModel.Base.DBEntity entity)
        {
            var type = entity.GetType();
            var keys=KeyFieldCache.Instance.FindKeyFields(type);

            var geometryProperty = type.GetProperty( GeometryQuery.GeometryField );
        }

        protected override string FetchGeometryText(string valueString)
        {
            return string.Concat(Regex.Matches(valueString, @"([\d\s,]+)").Cast<Match>().Select(o => o.Value).ToArray());
        }
    }
}
