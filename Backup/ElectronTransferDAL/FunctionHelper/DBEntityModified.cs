using System;
using Autodesk.AutoCAD.DatabaseServices;
using Autodesk.AutoCAD.Geometry;
using ElectronTransferDal.Cad;
using ElectronTransferDal.Common;
using ElectronTransferDal.Query;
using ElectronTransferFramework;
using ElectronTransferModel;
using ElectronTransferModel.Base;
using ElectronTransferModel.Geo;

namespace ElectronTransferDal.FunctionHelper
{
    public class DBEntityModified:Singleton<DBEntityModified>
    {

        #region 实体操作 移动、修改
        ObjectId lastObjectId = ObjectId.Null;
        /// <summary>
        /// 移动实体
        /// </summary>
        /// <param name="e">移动前实体对象</param>
        public void ObjectModified(ObjectEventArgs e)
        {
            try
            {
                var objectId = e.DBObject.Id;
                if (DBEntityFinder.Instance.HasG3EIDS(objectId))
                {
                    //锁定的设备才能修改
                    if (DBEntityFinder.Instance.VerifyLTTID(objectId))
                    {
                        //修改坐标
                        UpdateCoordinateDBEntity(e.DBObject as Entity);
                    }
                    else
                    {
                        //撤销操作会导致进来两次
                        if (lastObjectId.IsNull || lastObjectId != e.DBObject.Id)
                        {
                            lastObjectId = e.DBObject.Id;
                            PublicMethod.Instance.ShowMessage("当前选择有未被锁定的设备，不能修改！");
                            PublicMethod.Instance.Unappended();
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                LogManager.Instance.Error(ex);
            }
        }

        /// <summary>
        /// 更新坐标数据
        /// </summary>
        /// <param name="entity"></param>
        private void UpdateCoordinateDBEntity(Entity entity)
        {
            //获取坐标数据
            var value = PublicMethod.Instance.GetDBSymbolFinder(entity.ObjectId);
            if (value != null)
            {
                var pt = value as ElectronSymbol;
                //更新状态(只有原始数据才需改变状态（None）)
                if (pt.EntityState == EntityState.None)
                {
                    pt.EntityState = EntityState.Update;
                }

                //更新坐标
                if (pt.G3E_GEOMETRY is Multipoint)
                {
                    if (entity is BlockReference)
                    {
                        var blockRef = entity as BlockReference;
                        pt.G3E_GEOMETRY = ConvertGeometry.Instance.GetMultipoint(blockRef);
                    }else if(entity is DBText)
                    {
                        var dbText = entity as DBText;
                        pt.G3E_GEOMETRY = ConvertGeometry.Instance.GetMultipoint(dbText);
                        UpdateLabelLB(pt);
                    }else if (entity is MText)
                    {
                        var mText = entity as MText;
                        pt.G3E_GEOMETRY = ConvertGeometry.Instance.GetMultipoint(mText);  
                    }
                }
                else if (pt.G3E_GEOMETRY is LineString)
                {
                    var pl = entity as Polyline;
                    pt.G3E_GEOMETRY = ConvertGeometry.Instance.GetLineString(pl);
                }
                else if (pt.G3E_GEOMETRY is Polygon)
                {
                    var pl = entity as Polyline;
                    pt.G3E_GEOMETRY = ConvertGeometry.Instance.GetPolygon(pl);
                }
                DBManager.Instance.Update(pt);
                //更新缓存
                PublicMethod.Instance.UpdateDBSymbolFinder(entity.ObjectId, pt);
            }
        }

        /// <summary>
        /// 更新标注偏移
        /// </summary>
        /// <param name="es"></param>
        private void UpdateLabelLB(ElectronSymbol es)
        {
            try
            {
                var lb = DBEntityFinder.Instance.GetLabel_LB(es);
                if (lb != null)
                {
                    var alignment = lb.GetValue<int>("G3E_ALIGNMENT");
                    if (alignment != 9)
                    {
                        //更新状态(只有原始数据才需改变状态（None）)
                        if (lb.EntityState == EntityState.None)
                        {
                            lb.EntityState = EntityState.Update;
                        }
                        lb.SetValue("G3E_ALIGNMENT", 9);
                        DBManager.Instance.Update(lb);
                    }
                }
            }catch
            {
                
            }
        }
        #endregion
    }
    /// <summary>
    /// 转换坐标类
    /// </summary>
    public class ConvertGeometry : Singleton<ConvertGeometry>
    {
        #region 转换坐标

        /// <summary>
        /// 转换线坐标
        /// </summary>
        /// <param name="objectId"></param>
        /// <returns></returns>
        public LineString GetLineString(ObjectId objectId)
        {
            var lineString = new LineString();
            var ent = PublicMethod.Instance.GetObject(objectId, OpenMode.ForRead);
            if (ent is Polyline)
            {
                var polyLine = ent as Polyline;
                for (int i = 0; i < polyLine.NumberOfVertices; i++)
                {
                    Point3d point = polyLine.GetPoint3dAt(i);
                    lineString.Points.Add(new Point(new[] { point.X, point.Y, 0 }));
                }
            }
            return lineString;
        }
        /// <summary>
        /// 转换面坐标
        /// </summary>
        /// <param name="objectId"></param>
        /// <returns></returns>
        public Polygon GetPolygon(ObjectId objectId)
        {
            var pg = new Polygon();
            var lineString = GetLineString(objectId);
            pg.Lines.Add(lineString);
            return pg;
        }

        /// <summary>
        /// 转换面坐标
        /// </summary>
        /// <param name="polyLine"></param>
        /// <returns></returns>
        public Polygon GetPolygon(Polyline polyLine)
        {
            var pg = new Polygon();
            var lineString = GetLineString(polyLine);
            pg.Lines.Add(lineString);
            return pg;
        }


        /// <summary>
        /// 转换点坐标
        /// </summary>
        /// <param name="objectId"></param>
        /// <returns></returns>
        public Multipoint GetMultipoint(ObjectId objectId)
        {
            double x = 0, y = 0, r = 0;
            var ent = PublicMethod.Instance.GetObject(objectId, OpenMode.ForRead);
            if (ent is DBText)
            {
                var dbText = ent as DBText;
                x = dbText.Position.X;
                y = dbText.Position.Y;
                r = dbText.Rotation;
            }
            else if (ent is MText)
            {
                var mText = ent as MText;
                x = mText.Location.X;
                y = mText.Location.Y;
                r = mText.Rotation;
            }
            else if (ent is BlockReference)
            {
                var blockReference = ent as BlockReference;
                x = blockReference.Position.X;
                y = blockReference.Position.Y;
                r = blockReference.Rotation;
            }
            return GetMultipoint(x, y, r);
        }

        /// <summary>
        /// 转换点坐标
        /// </summary>
        /// <param name="blockReference"></param>
        /// <returns></returns>
        public Multipoint GetMultipoint(BlockReference blockReference)
        {
            var x = blockReference.Position.X;
            var y = blockReference.Position.Y;
            var r = blockReference.Rotation;

            return GetMultipoint(x, y, r);
        }

        /// <summary>
        /// 转换点坐标
        /// </summary>
        /// <param name="dbText"></param>
        /// <returns></returns>
        public Multipoint GetMultipoint(DBText dbText)
        {
            var x = dbText.Position.X;
            var y = dbText.Position.Y;
            var r = dbText.Rotation;

            return GetMultipoint(x, y, r);
        }
        /// <summary>
        /// 转换点坐标
        /// </summary>
        /// <param name="mText"></param>
        /// <returns></returns>
        public Multipoint GetMultipoint(MText mText)
        {
            var x = mText.Location.X;
            var y = mText.Location.Y;
            var r = mText.Rotation;

            return GetMultipoint(x, y, r);
        }

        /// <summary>
        /// 转换点坐标
        /// </summary>
        /// <param name="polyLine"></param>
        /// <returns></returns>
        public LineString GetLineString(Polyline polyLine)
        {
            var lineString = new LineString();
            for (int i = 0; i < polyLine.NumberOfVertices; i++)
            {
                Point3d point = polyLine.GetPoint3dAt(i);
                lineString.Points.Add(new Point(new[] { point.X, point.Y, 0 }));
            }
            return lineString;
        }

        /// <summary>
        /// 更新坐标
        /// </summary>
        /// <param name="es"></param>
        /// <param name="objectId"></param>
        /// <returns></returns>
        public ElectronSymbol UpdateG3E_GEOMETRY(ElectronSymbol es, ObjectId objectId)
        {
            if (es.G3E_GEOMETRY is Polygon)
            {
                es.G3E_GEOMETRY = GetPolygon(objectId);
            }
            else if (es.G3E_GEOMETRY is LineString)
            {
                es.G3E_GEOMETRY = GetLineString(objectId);
            }
            else if (es.G3E_GEOMETRY is Multipoint)
            {
                es.G3E_GEOMETRY = GetMultipoint(objectId);
            }
            return es;
        }
        public Multipoint GetMultipoint(double x, double y, double rotation)
        {
            var mpValue = new Multipoint();
            mpValue.Points.Add(new Point(new[] { x, y, 0 }));
            mpValue.Points.Add(new Point(new[] { Math.Cos(rotation), Math.Sin(rotation), 0 }));
            return mpValue;
        }
        #endregion
    }
}
