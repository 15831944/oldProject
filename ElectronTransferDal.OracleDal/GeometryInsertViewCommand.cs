using System;
using System.Collections.Generic;
using System.Data.Common;
using System.Linq;
using ElectronTransferDal.Common;
using ElectronTransferModel.Base;
using ElectronTransferModel.Config;
using ElectronTransferModel.Geo;
using Oracle.DataAccess.Client;
namespace ElectronTransferDal.OracleDal
{
    class GeometryInsertViewCommand:GeometryCommand
    {
        public GeometryInsertViewCommand(ElectronSymbol entity, SimpleMapping mapping, RDBManagerBase dbManager, DbConnection conn)
            : base(entity,mapping, dbManager,conn)
        {
        }

        public override bool Execute()
        {
            Dictionary<egtype, decimal> attributes = new Dictionary<egtype, decimal>();
            attributes.Add(egtype.point, 65536);
            attributes.Add(egtype.linestring, 196608);
            attributes.Add(egtype.polygon, 262144);
            attributes.Add(egtype.multipoint, 65536);
            var avoidFields = AvoidFieldsCache.Instance.GetAvoidFields(Entity.GetType(), true).ToList();
            List<Point> points = new List<Point>();
            if (Entity.G3E_GEOMETRY is Point)
            {
                Point point = Entity.G3E_GEOMETRY as Point;
                points.Add(point);
            }
            else if (Entity.G3E_GEOMETRY is Multipoint)
            {
                Multipoint multipoint = Entity.G3E_GEOMETRY as Multipoint;
                points.Add(multipoint.Points.First());
            }
            else if (Entity.G3E_GEOMETRY is LineString)
            {
                LineString lineString = Entity.G3E_GEOMETRY as LineString;
                points.AddRange(lineString.Points);
                //avoidFields.Add("G3E_ORIENTATION");

            }
            else if (Entity.G3E_GEOMETRY is Polygon)
            {
                Polygon polygon = Entity.G3E_GEOMETRY as Polygon;
                points.AddRange(polygon.UniqueLineString.Points);
                //avoidFields.Add("G3E_ORIENTATION");
            }

            points=MakeConnectPoints(points);

            avoidFields.Add("G3E_GEOMETRY");
            int step = 4;
            
            
           
            var geometryQueryBuilder = new InsertPlainGeometryQueryBuilder(Entity, avoidFields, Mapping);
            var dbManager = (DbManager as OracleDBManager);
            var conn = Connection;
            //using (var conn = dbManager.RevealConnection())
            //{
            //    conn.Open();
                //dbManager.RevealedRunsurrounds(Surrounds, conn);


                //new ParametersBuilder(Entity,)
                string sql = geometryQueryBuilder.ToString();
                using (DbTransaction transaction = conn.BeginTransaction())
                {
                    try
                    {
                        //if (Entity.G3E_GEOMETRY is LineString || Entity.G3E_GEOMETRY is Polygon)
                        bool isLineString = Entity.G3E_GEOMETRY is LineString;
                        if (isLineString)
                        {
                            var extCmd = conn.CreateCommand();
                            var extEntity = Entity.Clone();

                            var objectParameters =
                                new ParametersBuilder(Entity, extCmd, geometryQueryBuilder.ObjectFields).Parameters;
                            var geometryParamerters = GetGeometryParamerters(extCmd, new List<Point>()).ToList();
                            objectParameters.Single(o => o.ParameterName == ":G3E_ID").Value =
                                OracleSequenceValueGenerator.Instance.GenerateTableId(Entity.GetType());
                            var seqParameter = extCmd.CreateParameter() as OracleParameter;
                            seqParameter.ParameterName = ":SDO_SEQ";
                            seqParameter.Value = 0;

                            var eseqParameter = extCmd.CreateParameter() as OracleParameter;
                            eseqParameter.ParameterName = ":SDO_ESEQ";
                            eseqParameter.Value = 0;

                            var etypeParameter = extCmd.CreateParameter() as OracleParameter;
                            etypeParameter.ParameterName = ":SDO_ETYPE";
                            etypeParameter.Value = (decimal)10;
                            var orientation = extCmd.CreateParameter() as OracleParameter;
                            orientation.ParameterName = ":SDO_ORIENTATION";

                            var gdoAttributes = extCmd.CreateParameter() as OracleParameter;
                            gdoAttributes.ParameterName = ":GDO_ATTRIBUTES";
                            gdoAttributes.Value = 655360;//attributes[Entity.G3E_GEOMETRY.GeometryType];
                            extCmd.Parameters.AddRange(objectParameters.Concat(geometryParamerters).ToArray());
                            extCmd.Parameters.Add(orientation);
                            extCmd.Parameters.Add(seqParameter);
                            extCmd.Parameters.Add(eseqParameter);
                            extCmd.Parameters.Add(etypeParameter);
                            extCmd.Parameters.Add(gdoAttributes);
                            extCmd.CommandText = sql;
                            extCmd.ExecuteNonQuery();

                        }
                        var allPages = points.Count/step;
                        if (points.Count%step != 0)
                            allPages++;
                        for (int page = 0; page < allPages; page++)
                        {
                            var geometryCmd = conn.CreateCommand();
                            var items = points.Skip(page*step).Take(step);
                            var objectParameters =
                                new ParametersBuilder(Entity, geometryCmd, geometryQueryBuilder.ObjectFields).Parameters;
                            var geometryParamerters = GetGeometryParamerters(geometryCmd, items).ToList();
                            objectParameters.Single(o => o.ParameterName == ":G3E_ID").Value =
                                OracleSequenceValueGenerator.Instance.GenerateTableId(Entity.GetType());
                            var seqParameter = geometryCmd.CreateParameter() as OracleParameter;
                            seqParameter.ParameterName = ":SDO_SEQ";
                            seqParameter.Value = page;

                            var eseqParameter = geometryCmd.CreateParameter() as OracleParameter;
                            eseqParameter.ParameterName = ":SDO_ESEQ";
                            eseqParameter.Value = isLineString?1: 0;

                            var etypeParameter = geometryCmd.CreateParameter() as OracleParameter;
                            etypeParameter.ParameterName = ":SDO_ETYPE";
                            var type=(int)Entity.G3E_GEOMETRY.GeometryType;
                            if (type==2)
                                type=1;
                            etypeParameter.Value = (decimal)type;
                            var orientation = geometryCmd.CreateParameter() as OracleParameter;
                            orientation.ParameterName = ":SDO_ORIENTATION";
                            
                            var gdoAttributes= geometryCmd.CreateParameter() as OracleParameter;
                            gdoAttributes.ParameterName = ":GDO_ATTRIBUTES";
                            gdoAttributes.Value = attributes[Entity.G3E_GEOMETRY.GeometryType];
                            if (Entity.G3E_GEOMETRY is Multipoint)
                            {
                                var normal = (Entity.G3E_GEOMETRY as Multipoint).Points.Last();
                                //orientation.Precision =15;
                                //orientation.Scale = 15;

                                orientation.Value = (Single)Math.Atan2(normal.Y , normal.X);
                               
                            }
                            else
                            {
                                orientation.Value = null;
                            }
                            
                                                         
                            geometryCmd.Parameters.AddRange(objectParameters.Concat(geometryParamerters).ToArray());
                            geometryCmd.Parameters.Add(orientation);
                            geometryCmd.Parameters.Add(seqParameter);
                            geometryCmd.Parameters.Add(eseqParameter);
                            geometryCmd.Parameters.Add(etypeParameter);
                            geometryCmd.Parameters.Add(gdoAttributes);
                            geometryCmd.CommandText = sql;
                            geometryCmd.ExecuteNonQuery();
                        }
                        transaction.Commit();
                    }
                    catch (Exception)
                    {
                        transaction.Rollback();
                    }
                }

                //dbManager.RevealedRunEnds(Surrounds, conn);

                return true;
            //}
            
        }

        private static List<Point> MakeConnectPoints(List<Point> points)
        {
            List<Point> connectedPoints = new List<Point>();
            foreach (var point in points)
            {
                connectedPoints.Add(point);
                if (points.Count > 4)
                {
                    if (connectedPoints.Count % 4 == 0 && point != points.Last())
                    {
                        connectedPoints.Add(point);
                    }
                }
            }
            return connectedPoints;
        }

        //private IEnumerable<DbParameter> GetObjectParamerters(DbCommand cmd, IEnumerable<string> objectFields)
        //{
        //    foreach (var objectField in objectFields)
        //    {
        //        var parameter=cmd.CreateParameter();
        //        parameter.ParameterName = "@" + objectField;
        //        parameter.Value = Entity.GetValue(objectField);
        //        yield return parameter;

        //    }
        //}

        private IEnumerable<DbParameter> GetGeometryParamerters(DbCommand cmd, IEnumerable<Point> points)
        {
            IList<DbParameter> parameters=new List<DbParameter>();
            for (var index = 0; index < 4;index++ )
            {
                var point = points.Skip(index).FirstOrDefault();
                var x = cmd.CreateParameter();
                x.ParameterName = string.Format(":SDO_X{0}", index + 1);
                var y = cmd.CreateParameter();
                y.ParameterName = string.Format(":SDO_Y{0}", index + 1);

                if (point != null)
                {
                    x.Value = point.X;
                    y.Value = point.Y;
                }
                parameters.Add(x);
                parameters.Add(y);
            }
            return parameters;
        }
    }
}
