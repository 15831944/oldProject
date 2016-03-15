using System;
using System.Collections.Generic;
using System.Data.Common;
using System.Linq;
using ElectronTransferDal.Common;
using ElectronTransferModel.Base;
using ElectronTransferModel.Config;
using ElectronTransferModel.Geo;
using Oracle.DataAccess.Client;
using SpatialAnalysis;
using SAGeometry = SpatialAnalysis.Geometry;

//using ElectronTransferModel.Geo;

namespace ElectronTransferDal.OracleDal
{
    class GeometryInsertTableCommand:GeometryCommand
    {
        public GeometryInsertTableCommand(ElectronSymbol entity, SimpleMapping mapping, RDBManagerBase dbManager, DbConnection conn)
            : base(entity,mapping, dbManager, conn)
        {
        }

        public override bool Execute()
        {
            var dbManager = (DbManager as OracleDBManager);
            var conn = Connection;
            //using (var conn = dbManager.RevealConnection())
            //{
            //    conn.Open();
            //    dbManager.RevealedRunsurrounds(Surrounds, conn);
                var geometryCmd = conn.CreateCommand();
                var geometryQueryBuilder = new InsertSdoGeometryQueryBuilder(Entity, AvoidFieldsCache.Instance.GetNonColumnFields(Entity.GetType()), Mapping);
                geometryCmd.CommandText = geometryQueryBuilder.ToString();
                var parameters =
                    new ParametersBuilder(Entity, geometryCmd, geometryQueryBuilder.ObjectFields).Parameters;
                //parameters.Single(o => o.ParameterName == ":G3E_ID").Value =
                //    OracleSequenceValueGenerator.Instance.GenerateTableId(Entity.GetType());
                geometryCmd.Parameters.AddRange(parameters.ToArray());
                var geometryParameter=geometryCmd.CreateParameter() as OracleParameter;
                geometryParameter.ParameterName = "G3E_GEOMETRY";
                geometryParameter.OracleDbType = OracleDbType.Object;
                geometryParameter.UdtTypeName = "MDSYS.SDO_GEOMETRY";
                
                var saGeometry = new SAGeometry();
                saGeometry.SRID = null;
                GeometryElement[] elements=null;
                List<decimal> coordinates = new List<decimal>();
                List<Point> points = null;
                switch (Entity.G3E_GEOMETRY.GeometryType)
                {
                    case egtype.none:
                        
                        break;
                    //case egtype.point:
                    //    saGeometry.Shape = 3001;
                    //    elements = new [] { new GeometryElement { Offset = 1, ElementType = ElementType.Point, Interpretation = 1 },
                    //         new GeometryElement { Offset = 4, ElementType = ElementType.Point, Interpretation = 0 } };
                        
                    //    break;
                    case egtype.multipoint:
                        saGeometry.Shape = 3001;
                        elements = new[] { new GeometryElement { Offset = 1, ElementType = ElementType.Point, Interpretation = 1 },
                             new GeometryElement { Offset = 4, ElementType = ElementType.Point, Interpretation = 0 } };
                        points = (Entity.G3E_GEOMETRY as Multipoint).Points;
                        break;
                    case egtype.linestring:
                        saGeometry.Shape = 3002;
                        elements = new[] { new GeometryElement() { Offset = 1, ElementType = ElementType.LineString, Interpretation = 1 } };
                        points = (Entity.G3E_GEOMETRY as LineString).Points;
                        break;
                    case egtype.polygon:
                        saGeometry.Shape = 3003;
                        elements = new[] { new GeometryElement() { Offset = 1, ElementType = ElementType.Polygon, Interpretation = 1 } };
                        points = (Entity.G3E_GEOMETRY as Polygon).UniqueLineString.Points;
                        break;
                    default:
                        throw new ArgumentOutOfRangeException();
                }
                if (points != null)
                {
                    points.ForEach(o => coordinates.AddRange(new decimal[] { (decimal)o.X, (decimal)o.Y, (decimal)o.Z }));
                }
                saGeometry.GeometryElements = elements;
                saGeometry.Coordinates = coordinates.ToArray();
                geometryParameter.Value = saGeometry;
                geometryCmd.Parameters.Add(geometryParameter);
                geometryCmd.ExecuteNonQuery();
            //    dbManager.RevealedRunEnds(Surrounds, conn);
            //}
            
            return true;
            //return DbManager.Insert(Entity.GetType().Name, Entity, false, surrounds);
        }
    }
}
