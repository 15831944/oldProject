using System;
using System.Collections.Generic;
using System.Data.Common;
using System.Linq;
using System.Reflection;
using System.Xml;
using ElectronTransferDal.Common;
using ElectronTransferFramework;
using ElectronTransferModel.Base;
using ElectronTransferModel.Geo;
using Oracle.DataAccess.Client;
using Oracle.DataAccess.Types;

namespace ElectronTransferDal.OracleDal
{
    public class OracleDBManager : RDBManagerBase
    {
        public string UserName { get; set; }
        public string Password { get; set; }
        protected override GeometryQuery GeometryQuery
        {
            get { return new OracleGeometryQuery(); }
        }

        protected override void OnInitialize()
        {
            SequenceValueGenerator = OracleSequenceValueGenerator.Instance;
            QueryVersion = QueryVersion.V10;
            
        }
        protected override string[] GetNameAndPwd()
        {
            string[] str = new string[2];
            str[0] = UserName;
            str[1] = Password;
            return str;
        }
        protected override DataTableConverter GetConverter()
        {
            return new OracleDataTableConverter();
        }
        protected override object GetValue(DbDataReader reader, PropertyInfo property)
        {
            var oracleReader = (reader as OracleDataReader);
            int ord = reader.GetOrdinal(property.Name);
            if (reader.IsDBNull(ord))
            {
                return null;
            }
            if (oracleReader.GetFieldType(ord) == typeof (decimal) )
            {
                return (decimal) (OracleDecimal.SetPrecision(oracleReader.GetOracleDecimal(ord), PrecisionCache.Instance[property]));
            }
            else
            {
                return oracleReader[property.Name];
            }
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
                        geometry = Multipoint.Parse(valueString, ' ', ',');
                        break;
                    case egtype.linestring:
                        geometry = LineString.Parse(valueString, ' ', ',');
                        break;
                    case egtype.polygon:
                        geometry = Polygon.Parse(valueString, ' ', ',');
                        break;
                    case egtype.none:
                    default:
                        break;
                }
            }
            catch (GeometryException ex)
            {
                LogManager.Instance.Error(ex);
            }
            return geometry;
        }

        protected override void UpdateItem(Type type, object entity, bool byView, DbConnection conn)
        {
            if (entity is ElectronSymbol)
            {
                GeometryCommandFactory.Instance.GetUpdateCommand(entity as ElectronSymbol, Mapping, this, byView,conn).Execute();
            }
            else

                base.UpdateItem(type, entity, byView, conn);
        }
        protected override void InsertItem(Type type, object entity, bool byView, DbConnection conn)
        {
            if (entity is ElectronSymbol)
            {
                GeometryCommandFactory.Instance.GetInsertCommand(entity as ElectronSymbol, Mapping, this, byView, conn).Execute();
            }
            else
                base.InsertItem(type, entity, byView, conn);
        }
        internal DbConnection RevealConnection()
        {
            return GetConnection();
        }

        internal void RevealedRunsurrounds(ISurround[] surrounds, DbConnection conn) 
        {
            Runsurrounds(surrounds, conn);
        }
        internal void RevealedRunEnds(ISurround[] surrounds, DbConnection conn)
        {
            RunEnds(surrounds, conn);
        }
        internal void RevealedDeleteItem(DBEntity entity, bool byView, DbConnection conn) 
        {
            DeleteItem(entity, byView, conn);
        }
        
        protected override void MakeGeometry(DBEntity entity)
        {
            Type type = entity.GetType();
            using (var conn = (OracleConnection) GetConnection())
            {
                string geometryField = GeometryQuery.GeometryField;
                string sql = new OracleGeometryQueryBuilder(type, Mapping, entity, geometryField).ToString();
                OracleCommand cmd = conn.CreateCommand();
                cmd.CommandText = sql;
                cmd.XmlCommandType = OracleXmlCommandType.Query;
                conn.Open();
                XmlReader reader = cmd.ExecuteXmlReader();

                var document = new XmlDocument();
                document.Load(reader);

                XmlNodeList geometryElement = document.GetElementsByTagName(geometryField);
                XmlNode node = geometryElement.Item(0);
                if (node == null) return;
                int gtype;
                if (!int.TryParse(node["SDO_GTYPE"].InnerText, out gtype)) 
                {
                    throw new Exception("Wrong SDO_GTYPE" + node["SDO_GTYPE"].InnerText);
                }
                SdoGeoType geoType = (SdoGeoType)gtype;
                XmlElement ordinates = node["SDO_ORDINATES"];
                Geometry geometry = null;
                List<Point> points = null;
                switch (geoType)
                {
                    case SdoGeoType.Multipoint:

                        var multiPoint = new Multipoint();
                        points = multiPoint.Points;
                        geometry = multiPoint;
                        break;
                    case SdoGeoType.LineString:
                        var lineString = new LineString();
                        points = lineString.Points;
                        geometry = lineString;
                        break;
                    case SdoGeoType.Polygon:
                        var polygon = new Polygon();
                        points = polygon.Lines.First().Points;
                        geometry = polygon;
                        break;
                    default:
                        break;
                }
                for (int index = 0; index < ordinates.ChildNodes.Count; index += 3)
                {
                    double x = double.Parse(ordinates.ChildNodes[index].InnerText);
                    double y = double.Parse(ordinates.ChildNodes[index + 1].InnerText);
                    double z = double.Parse(ordinates.ChildNodes[index + 2].InnerText);
                    var point = new Point(x, y, z);
                    points.Add(point);
                }
                entity.SetValue(geometryField, geometry);
                //type.GetProperty(geometryField).SetValue(entity, geometry, null);
            }
        }

        protected override string FetchGeometryText(string valueString)
        {
            var doc = new XmlDocument();
            doc.LoadXml(valueString);
            return doc.GetElementsByTagName("gml:coordinates")[0].InnerText;
        }


    }
}