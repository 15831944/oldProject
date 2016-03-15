using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data.Common;
using ElectronTransferModel.Base;
using ElectronTransferFramework;
using ElectronTransferModel.Geo;
using System.Diagnostics;
using System.Data;
using ElectronTransferModel;

namespace ElectronTransferDal.OracleDal
{
    public class OracleDBManagerV94 : OracleDBManager
    {
        protected override void OnInitialize()
        {
            base.OnInitialize();
            QueryVersion = QueryVersion.V94;
        }
        protected override IEnumerable<ElectronTransferModel.Base.DBEntity> ConstructEntities(System.Data.Common.DbDataReader reader, Type type)
        {
            if (!type.CheckPropertyAvailable("G3E_GEOMETRY", typeof(Geometry)))
                return base.ConstructEntities(reader, type);
            else
                return ConstructEntitiesByRows(reader, type);
        }

        protected override IEnumerable<DBEntity> ConstructEntities(System.Data.DataTable table, Type type)
        {
            if (!type.CheckPropertyAvailable("G3E_GEOMETRY", typeof(Geometry)))
                return base.ConstructEntities(table, type);
            else
                return ConstructEntitiesByRows(table, type);
        }

        protected override DBEntity ConstructEntity(System.Data.DataTable table, Type type)
        {

            if (!type.CheckPropertyAvailable("G3E_GEOMETRY", typeof(Geometry)))
                return base.ConstructEntity(table, type);
            else
                return ConstructEntitiesByRows(table, type).First();
        }

        protected override DBEntity ConstructEntity(DbDataReader reader, Type type)
        {
            if (!type.CheckPropertyAvailable("G3E_GEOMETRY", typeof(Geometry)))
                return base.ConstructEntity(reader, type);
            else return ConstructEntitiesByRows(reader, type).First();
        }

        private Dictionary<string, object> FakeDynamicObject(DbDataReader reader, Type type)
        {
            Dictionary<string, object> obj = new Dictionary<string, object>();
            var xNames = new string[] { "SDO_X1", "SDO_X2", "SDO_X3", "SDO_X4" };
            var yNames = new string[] { "SDO_Y1", "SDO_Y2", "SDO_Y3", "SDO_Y4" };
            foreach (var property in type.GetProperties())
            {
                try
                {
                    if (property.GetCustomAttributes(typeof(NonColumnAttribute), true).Any())
                        continue;
                    obj.Add(property.Name, GetValue(reader, property));
                }
                catch (IndexOutOfRangeException)
                {

                }
            }

            foreach (var coordName in xNames.Concat(yNames))
            {
                obj.Add(coordName, reader[coordName]);
            }
            obj.Add("SDO_ETYPE", reader["SDO_ETYPE"]);
            return obj;
        }

        private Dictionary<string, object> FakeDynamicObject(DataRow row, Type type)
        {
            Dictionary<string, object> obj = new Dictionary<string, object>();
            var xNames = new string[] { "SDO_X1", "SDO_X2", "SDO_X3", "SDO_X4" };
            var yNames = new string[] { "SDO_Y1", "SDO_Y2", "SDO_Y3", "SDO_Y4" };
            foreach (var property in type.GetProperties())
            {
                try
                {
                    if (property.GetCustomAttributes(typeof(NonColumnAttribute), true).Any())
                        continue;
                    obj.Add(property.Name, row[property.Name]);
                }
                catch (Exception ex)
                {

                }
            }

            foreach (var coordName in xNames.Concat(yNames))
            {
                obj.Add(coordName, row[coordName]);
            }
            obj.Add("SDO_ETYPE", row["SDO_ETYPE"]);
            obj.Add("SDO_ORIENTATION", row["SDO_ORIENTATION"]);
            return obj;
        }


        private IEnumerable<Dictionary<string, object>> GetFakeDynamicObjects(DataTable table, Type type)
        {
            foreach (DataRow row in table.Rows)
            {
                yield return FakeDynamicObject(row, type);

            }
        }


        private IEnumerable<Dictionary<string, object>> GetFakeDynamicObjects(DbDataReader reader, Type type)
        {
            while (reader.Read())
            {
                yield return FakeDynamicObject(reader, type);
            }
        }




        private IEnumerable<DBEntity> ConstructEntitiesByRows(DataTable table, Type type)
        {
            var entities = GetFakeDynamicObjects(table, type).ToList();
            var xNames = new string[] { "SDO_X1", "SDO_X2", "SDO_X3", "SDO_X4" };
            var yNames = new string[] { "SDO_Y1", "SDO_Y2", "SDO_Y3", "SDO_Y4" };
            List<DBEntity> ret = new List<DBEntity>();

            foreach (var entityObjects in entities.Where(o => ((Decimal?)o["SDO_ETYPE"]) >= 1 && ((Decimal?)o["SDO_ETYPE"]) <= 4).OrderBy(o => o["G3E_ID"]).GroupBy(o => o["G3E_FID"]))
            {
                foreach (var entityInLtt in entityObjects.GroupBy(o => o["LTT_STATUS"]))
                {
                    var entity = ReflectionUtils.CreateObject(new { }, type) as ElectronSymbol;
                    List<Point> points = new List<Point>();
                    bool hasOrientation = false;
                    foreach (var entityObject in entityInLtt)
                    {

                        for (int i = 0; i < 4; i++)
                        {
                            string xName = xNames[i];
                            string yName = yNames[i];
                            if ((entityObject[xName] is DBNull))
                                break;
                            decimal x = (decimal)entityObject[xName];
                            decimal y = (decimal)entityObject[yName];
                            points.Add(new Point { X = (double)x, Y = (double)y });

                        }
                        hasOrientation = !(entityObject["SDO_ORIENTATION"] is DBNull);
                        if (!(entityObject["SDO_ORIENTATION"] is DBNull))
                        {
                            var orientation = (double)((entityObject["SDO_ORIENTATION"]) as decimal?).Value;
                            points.Add(new Point { X = Math.Cos(orientation), Y = Math.Sin(orientation) });
                        }



                    }
                    for (int currentIndex = points.Count - 2; currentIndex >= 0; currentIndex--)
                    {
                        var lastIndex = currentIndex + 1;
                        if (points[lastIndex].Equals(points[currentIndex]))
                        {
                            points.RemoveAt(lastIndex);
                        }
                    }
                    var etype = (egtype)Enum.ToObject(typeof(egtype), (int)(System.Decimal?)entityObjects.First()["SDO_ETYPE"]);
                    //List<Point> pointsReferenced =null;
                    if (etype == egtype.point)
                    {
                        if (hasOrientation)
                        {
                            etype = egtype.multipoint;
                        }
                    }
                    switch (etype)
                    {
                        case egtype.none:
                            break;
                        case egtype.point:

                            break;
                        case egtype.multipoint:
                            Multipoint multipoint = new Multipoint();
                            multipoint.Points.AddRange(points);
                            entity.G3E_GEOMETRY = multipoint;
                            break;
                        case egtype.linestring:
                            LineString lineString = new LineString();
                            lineString.Points.AddRange(points);
                            entity.G3E_GEOMETRY = lineString;
                            break;
                        case egtype.polygon:
                            Polygon polygon = new Polygon();
                            polygon.Lines.First().Points.AddRange(points);
                            entity.G3E_GEOMETRY = polygon;
                            break;
                        default:
                            break;
                    }

                    var aEntityObject = entityInLtt.First();
                    foreach (var property in type.GetProperties())
                    {
                        if (aEntityObject.ContainsKey(property.Name))
                        {
                            try
                            {
                                entity.SetValue(property.Name, aEntityObject[property.Name]);
                            }
                            catch
                            {
                            }
                        }
                    }
                    ret.Add(entity);

                }
                //yield return entity;
            }
            return ret;

        }


        private IEnumerable<DBEntity> ConstructEntitiesByRows(DbDataReader reader, Type type)
        {
            var entities = GetFakeDynamicObjects(reader, type).ToList();
            var xNames = new string[] { "SDO_X1", "SDO_X2", "SDO_X3", "SDO_X4" };
            var yNames = new string[] { "SDO_Y1", "SDO_Y2", "SDO_Y3", "SDO_Y4" };
            List<DBEntity> ret = new List<DBEntity>();
            foreach (var entityObjects in entities.Where(o => ((Decimal?)o["SDO_ETYPE"]) >= 1 && ((Decimal?)o["SDO_ETYPE"]) <= 4).OrderBy(o => o["G3E_ID"]).GroupBy(o => o["G3E_FID"]))
            {
                var entity = ReflectionUtils.CreateObject(new { }, type) as ElectronSymbol;
                List<Point> points = new List<Point>();
                foreach (var entityObject in entityObjects)
                {

                    for (int i = 0; i < 4; i++)
                    {
                        string xName = xNames[i];
                        string yName = yNames[i];
                        if (!(entityObject[xName] is DBNull))
                        {
                            decimal x = (decimal)entityObject[xName];
                            decimal y = (decimal)entityObject[yName];
                            points.Add(new Point { X = (double)x, Y = (double)y });
                        }

                    }


                }
                for (int currentIndex = points.Count - 2; currentIndex >= 0; currentIndex--)
                {
                    var lastIndex = currentIndex + 1;
                    if (points[lastIndex].Equals(points[currentIndex]))
                    {
                        points.RemoveAt(lastIndex);
                    }
                }
                var etype = (egtype)Enum.ToObject(typeof(egtype), (int)(System.Decimal?)entityObjects.First()["SDO_ETYPE"]);
                //List<Point> pointsReferenced =null;
                switch (etype)
                {
                    case egtype.none:
                        break;
                    case egtype.point:

                        break;
                    case egtype.multipoint:
                        Multipoint multipoint = new Multipoint();
                        multipoint.Points.AddRange(points);
                        entity.G3E_GEOMETRY = multipoint;
                        break;
                    case egtype.linestring:
                        LineString lineString = new LineString();
                        lineString.Points.AddRange(points);
                        entity.G3E_GEOMETRY = lineString;
                        break;
                    case egtype.polygon:
                        Polygon polygon = new Polygon();
                        polygon.Lines.First().Points.AddRange(points);
                        entity.G3E_GEOMETRY = polygon;
                        break;
                    default:
                        break;
                }
                ret.Add(entity);


                //yield return entity;
            }
            return ret;

        }


    }
}
