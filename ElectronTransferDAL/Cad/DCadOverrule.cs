using System.Collections;
using System.Collections.Generic;
using Autodesk.AutoCAD.ApplicationServices;
using Autodesk.AutoCAD.Colors;
using Autodesk.AutoCAD.DatabaseServices;
using Autodesk.AutoCAD.EditorInput;
using DotNetARX;
using Autodesk.AutoCAD.Geometry;
using System.Linq;
using Autodesk.AutoCAD.GraphicsInterface;
using ElectronTransferDal.Common;
using ElectronTransferDal.FunctionHelper;
using ElectronTransferDal.Query;
using ElectronTransferFramework;
using ElectronTransferModel.V9_4;
using ElectronTransferModel;
using ElectronTransferModel.Base;
using Polyline = Autodesk.AutoCAD.DatabaseServices.Polyline;

namespace ElectronTransferDal.Cad
{
    #region 高亮规则重定义
    public class HightLightRule : HighlightOverrule
    {
        public static List<ObjectId> rejectObj = new List<ObjectId>();
        public static Dictionary<ObjectId, int> objDict = new Dictionary<ObjectId, int>();
        public override void Highlight(Entity entity, FullSubentityPath subId, bool highlightAll)
        {
            if (rejectObj.Contains(entity.ObjectId)) return;
            Database db = entity.Database;
            Document doc = db.GetDocument();
            Editor ed = doc.Editor;
            try
            {
                //开启事务处理，注意锁定文档
                using (db.TransactionManager.StartOpenCloseTransaction()) 
                using (doc.LockDocument())
                {
                    var bl = false;
                    if (entity is DBText)
                        bl = PublicMethod.Instance.GetLayerLockStatus(entity.LayerId);
                    if (!bl)
                    {
                        entity.UpgradeOpen();
                        if (!objDict.ContainsKey(entity.ObjectId))
                        {
                            objDict.Add(entity.ObjectId, entity.ColorIndex);
                        }
                        entity.Color = Color.FromRgb(0, 255, 0); //绿色
                        entity.DowngradeOpen();
                        base.Highlight(entity, subId, highlightAll);
                    }
                }
            }
            catch (System.Exception ex)
            {
                ed.WriteMessageWithReturn(ex);
            }
        }
        public override void Unhighlight(Entity entity, FullSubentityPath subId, bool highlightAll)
        {
            if (rejectObj.Contains(entity.ObjectId)) return;
            Database db = entity.Database;
            Document doc = db.GetDocument();
            Editor ed = doc.Editor;
            //Entity line = entity as Entity;
            try
            {
                using (db.TransactionManager.StartOpenCloseTransaction()) 
                using (doc.LockDocument())
                {
                    entity.UpgradeOpen();
                    if (objDict.ContainsKey(entity.ObjectId))
                    {
                        entity.ColorIndex = objDict[entity.ObjectId];
                    }
                    entity.DowngradeOpen();
                }
                base.Unhighlight(entity, subId, highlightAll);
            }
            catch (System.Exception ex)
            {
                ed.WriteMessageWithReturn(ex);
            }
        }
    }
    #endregion

    #region 夹点规则重定义
    //实现特殊的夹点
    class UserGripData : GripData
    {
        public UserGripData(Point3d position)
        {
            GripPoint = position;
        }
    }
    //夹点重定义
    public class UserGripOverrule : GripOverrule
    {
        public static ObjectId objectId;
        public static Point3d pt = new Point3d();
        public static Point3dCollection nods1 = new Point3dCollection();
        public static Point3dCollection nods2 = new Point3dCollection();
        public static Point3d pt1 = new Point3d();
        public static Point3d pt2 = new Point3d();
        public static Point3d mpt1 = new Point3d();
        public static Point3d mpt2 = new Point3d();
        private ObjectId selents ;
        public static UserGripOverrule TheOverrule = new UserGripOverrule();
        public UserGripOverrule()
        {
            SetXDataFilter(DCadApi.RegAppName);
        }
        //获取夹点,简单实体应重载该函数以获取更灵活的控制
        public override void GetGripPoints(Entity entity, GripDataCollection grips, double curViewUnitSize, int gripSize, Vector3d curViewDir, GetGripPointsFlags bitFlags)
        {
            try
            {
                objectId = entity.ObjectId;
                if (objectId.IsErased) return;
                if (entity is BlockReference)
                {
                    var brf = entity as BlockReference;
                    bool b = FeatureGripsMapping.instance.grips.ContainsKey(brf.Name);
                    if (b)
                    {
                        double xdist1 = FeatureGripsMapping.instance.grips[brf.Name].xdistn1;
                        double ydist1 = FeatureGripsMapping.instance.grips[brf.Name].ydistn1;
                        double xdist2 = FeatureGripsMapping.instance.grips[brf.Name].xdistn2;
                        double ydist2 = FeatureGripsMapping.instance.grips[brf.Name].ydistn2;
                        //double startAngle = FeatureGripsMapping.instance.grips[brf.Name].angle;
                        //grips.Remove(grips[2]);
                        pt1 = new Point3d(brf.Position.X + xdist1, brf.Position.Y + ydist1, 0);
                        pt2 = new Point3d(brf.Position.X + xdist2, brf.Position.Y + ydist2, 0);
                        pt1 = pt1.RotateBy(brf.Rotation/* + Math.PI / 180 *startAngle*/, Vector3d.ZAxis, brf.Position);
                        pt2 = pt2.RotateBy(brf.Rotation/* + Math.PI / 180 * startAngle*/, Vector3d.ZAxis, brf.Position);
                        grips.Add(new UserGripData(pt1));
                        if (pt2 != brf.Position) grips.Add(new UserGripData(pt2));
                        // 取出当前窗口大小
                        var viewsize = (double)Application.GetSystemVariable("VIEWSIZE");
                        // 得到当前选择集的集合
                        var sset=entity.Database.GetEditor().SelectImplied().Value;
                        if (sset != null)
                        {
                            var objIds =sset.GetObjectIds();
                            if (objIds.Any())
                            {
                                var sels = objIds.Last(); // 
                                if (viewsize <= 0.00025)
                                {
                                    if (selents != sels)
                                    {
                                        selents = sels;
                                        snapDevs(entity.ObjectId);
                                    }
                                }
                            }
                        }
                    }
                    else
                    {
                        //pt1 = pt2 = Point3d.Origin;
                        pt1 = pt2 = brf.Position;
                        nods1.Clear(); nods2.Clear();
                    }
                    base.GetGripPoints(entity, grips, curViewUnitSize, gripSize, curViewDir, bitFlags);
                }
                else if(entity is DBText)
                {
                    var txt = entity as DBText;
                    grips.Add(new UserGripData(txt.Position));
                }
                else if (entity is MText)
                {
                    var txt = entity as MText;
                    grips.Add(new UserGripData(txt.Location));
                }
            }
            catch (System.Exception ex)
            {
                PublicMethod.Instance.Editor.WriteMessageWithReturn("UserGripOverrule::GetGripPoints :" + ex.Message + "\n");
            }
        }

        //移动夹点
        public override void MoveGripPointsAt(Entity entity, GripDataCollection grips, Vector3d offset, MoveGripPointsFlags bitFlags)
        {
            var ismove = false;
            foreach (GripData gd in grips)
            {
                if (gd is UserGripData && ismove == false)
                {
                    //UserGripData lagd = (UserGripData)gd;
                    if (entity is BlockReference)
                    {
                        var brf = entity as BlockReference;
                        brf.Position = brf.Position.Add(offset);
                    }
                    else if (entity is DBText)
                    {
                        var txt = entity as DBText;
                        txt.Position = txt.Position.Add(offset);
                        txt.AlignmentPoint = txt.AlignmentPoint.Add(offset);
                    }
                    else if (entity is MText)
                    {
                        var txt = entity as MText;
                        txt.Location = txt.Location.Add(offset);
                    }
                    ismove = true;
                }
            }
            if (grips.Count > 0 )
            {
                if ( entity is BlockReference)
                {
                    if (!ismove) base.MoveGripPointsAt(entity, grips, offset, bitFlags);
                    mpt1 = pt1.Add(offset);
                    mpt2 = pt2.Add(offset);
                    pt = GeTools.MidPoint(mpt1, mpt2);
                } 
                else if(entity is DBText)
                {
                    if (!ismove) base.MoveGripPointsAt(entity, grips, offset, bitFlags);
                    pt =((DBText)entity).Position;
                }
                else if (entity is MText)
                {
                    if (!ismove) base.MoveGripPointsAt(entity, grips, offset, bitFlags);
                    pt = ((MText)entity).Location;
                }
            }

        }
        // 捕捉对象，画连线
        //void snapObjects(ObjectId objid, Point3d pt1, Point3d pt2)
        //{
        //    Database db = HostApplicationServices.WorkingDatabase;
        //    Document doc = db.GetDocument();
        //    Editor ed = db.GetEditor();
        //    double radius = 0.00000075;
        //    Point3dCollection pts1 = new Point3dCollection();
        //    Point3dCollection pts2 = new Point3dCollection();
        //    for (int i = 1; i < 11; i++)
        //    {
        //        pts1.Add(pt1.Add(new Vector3d(Math.Cos(360 / 10 * i) * radius, Math.Sin(360 / 10 * i) * radius, 0)));
        //        pts2.Add(pt2.Add(new Vector3d(Math.Cos(360 / 10 * i) * radius, Math.Sin(360 / 10 * i) * radius, 0)));
        //    }

        //    PromptSelectionResult psr1 = ed.SelectFence(pts1);
        //    PromptSelectionResult psr2 = ed.SelectFence(pts2);
        //    Point3dCollection ptts1 = new Point3dCollection();
        //    Point3dCollection ptts2 = new Point3dCollection();
        //    using (Transaction tr = db.TransactionManager.StartTransaction())
        //    {
        //        using (DocumentLock doclock = doc.LockDocument())
        //        {
        //            if (psr1.Status == PromptStatus.OK)
        //                foreach (var vv in psr1.Value.GetObjectIds())
        //                {
        //                    if (vv == objid) continue;
        //                    Entity ent = tr.GetObject(vv, OpenMode.ForRead) as Entity;
        //                    if (ent is BlockReference)
        //                    {
        //                        BlockReference tempbrf = ent as BlockReference;
        //                        ptts1.Add(tempbrf.Position);
        //                    }
        //                    else if (ent is Curve)
        //                    {
        //                        Curve tempcur = ent as Curve;
        //                        ptts1.Add(tempcur.GetClosestPointTo(pt1, false));
        //                    }
        //                }

        //            if (psr2.Status == PromptStatus.OK)
        //                foreach (var vv in psr2.Value.GetObjectIds())
        //                {
        //                    if (vv == objid) continue;
        //                    Entity ent = tr.GetObject(vv, OpenMode.ForRead) as Entity;
        //                    if (ent is BlockReference)
        //                    {
        //                        BlockReference tempbrf = ent as BlockReference;
        //                        ptts2.Add(tempbrf.Position);
        //                    }
        //                    else if (ent is Curve)
        //                    {
        //                        Curve tempcur = ent as Curve;
        //                        ptts2.Add(tempcur.GetClosestPointTo(pt2, false));
        //                    }
        //                }
        //            tr.Commit();
        //        }
        //    }
        //    foreach (var v in ptts1)
        //    {
        //        ed.DrawVector(pt1, (Point3d)v, 3/*1727987712*/, false);
        //    }
        //    foreach (var v in ptts2)
        //    {
        //        ed.DrawVector(pt2, (Point3d)v, 3/*1727987712*/, false);
        //    }
        //}
        // 得到连接点1、连接点2的集合
        void snapDevs(ObjectId objid)
        {
            nods1.Clear();
            nods2.Clear();
            var doc = HostApplicationServices.WorkingDatabase.GetDocument();
            G3EObject g3eObject = null;
            if (!DBEntityFinder.Instance.GetG3EIds(objid, ref g3eObject)) return;
            var v = DBManager.Instance.GetEntity<Connectivity_n>(g3eObject.G3E_FID);
            if(v==null) return;
            var ns11 =
                DBManager.Instance.GetEntities<Connectivity_n>(
                    o =>
                    o.NODE1_ID == v.NODE1_ID && o.EntityState != EntityState.Delete && o.NODE1_ID != 0 &&
                    o.G3E_FID != v.G3E_FID);
            var ns12 =
                DBManager.Instance.GetEntities<Connectivity_n>(
                    o =>
                    o.NODE2_ID == v.NODE1_ID && o.EntityState != EntityState.Delete && o.NODE2_ID != 0 &&
                    o.G3E_FID != v.G3E_FID);
            var ns1 = ns11.Concat(ns12).Distinct(new ElectronBaseCompare<Connectivity_n>());
            var ns21 =
                DBManager.Instance.GetEntities<Connectivity_n>(
                    o =>
                    o.NODE1_ID == v.NODE2_ID && o.EntityState != EntityState.Delete && o.NODE1_ID != 0 &&
                    o.G3E_FID != v.G3E_FID);
            var ns22 =
                DBManager.Instance.GetEntities<Connectivity_n>(
                    o =>
                    o.NODE2_ID == v.NODE2_ID && o.EntityState != EntityState.Delete && o.NODE2_ID != 0 &&
                    o.G3E_FID != v.G3E_FID);
            var ns2 = ns21.Concat(ns22).Distinct(new ElectronBaseCompare<Connectivity_n>());
            foreach (var n1 in ns1)
            {
                var oid = DBEntityFinder.Instance.GetObjectIdByFid(n1.G3E_FID);
                if (!oid.IsNull)
                {
                    using (Transaction tr = doc.TransactionManager.StartTransaction())
                    {
                        var ent = tr.GetObject(oid, OpenMode.ForRead) as Entity;
                        if (ent is BlockReference)
                        {
                            nods1.Add(((BlockReference) ent).Position);
                        }
                        else if (ent is Polyline)
                        {
                            var pp = ((Polyline) ent).GetClosestPointTo(pt1, false);
                            nods1.Add(pp);
                        }
                    }
                }
            }
            foreach (var n2 in ns2)
            {
                var oid = DBEntityFinder.Instance.GetObjectIdByFid(n2.G3E_FID);
                if (!oid.IsNull)
                {
                    using (var tr = doc.TransactionManager.StartTransaction())
                    {
                        var ent = tr.GetObject(oid, OpenMode.ForRead) as Entity;
                        if (ent is BlockReference)
                        {
                            nods2.Add(((BlockReference) ent).Position);
                        }
                        else if (ent is Polyline)
                        {
                            var pp = ((Polyline) ent).GetClosestPointTo(pt2, false);
                            nods2.Add(pp);
                        }
                    }
                }
            }
        }
    }
    #endregion

    #region 显示规则重定义
    public class UserDrawOverrule : DrawableOverrule
    {
        public static UserDrawOverrule TheOverrule = new UserDrawOverrule();
        public UserDrawOverrule()
        {
            SetXDataFilter(DCadApi.RegAppName);
        }
        public override bool WorldDraw(Drawable drawable, WorldDraw wd)
        {
            var brf = drawable as BlockReference;
            if (brf == null) return base.WorldDraw(drawable, wd);
            if(!DBEntityCopy.Instance.isDBEntityCopy)
            {
                foreach (var v in UserGripOverrule.nods2)
                {
                    wd.Geometry.WorldLine(UserGripOverrule.mpt2, (Point3d)v);
                }
                foreach (var v in UserGripOverrule.nods1)
                {
                    wd.Geometry.WorldLine(UserGripOverrule.mpt1, (Point3d)v);
                }
            }
            return base.WorldDraw(drawable, wd);
        }
    }
    #endregion

    #region 设备、标注 
    public class DCadDrawEntityOverrule : DrawableOverrule
    {
        /// <summary>
        /// 名称
        /// </summary>
        public const string RegAppName = "DrawEntityXdata";

        public static DCadDrawEntityOverrule TheOverrule = new DCadDrawEntityOverrule();
        public DCadDrawEntityOverrule()
        {
            SetXDataFilter(RegAppName);
        }

        public override bool WorldDraw(Drawable drawable, WorldDraw wd)
        {
            if (!DBEntityCopy.Instance.isDBEntityCopy)
            {
                var objectIdCollection = new ObjectIdCollection();
                var points = GetSymbolValueByObjectId(UserGripOverrule.objectId,ref objectIdCollection);
                foreach (var v in points)
                {
                    wd.Geometry.WorldLine(UserGripOverrule.pt, (Point3d) v);
                }
            }
           
            
            //var objIds = new ObjectId[objectIdCollection.Count];
            //objectIdCollection.CopyTo(objIds, 0);
            //ManyEntityCopy dragger = new ManyEntityCopy();

            //var dragResult = dragger.StartDrag(UserGripOverrule.pt, objIds);

            //if (dragResult.Status == PromptStatus.OK)
            //{
            //}


            
            //foreach (Entity draggingEntity in objectIdCollection)
            //{
            //    draggingEntity.TransformBy(displacementMatrix);
            //    wd.Geometry.Draw(draggingEntity);
            //}

            return base.WorldDraw(drawable, wd);
        }


        /// <summary>
        /// 获取对应的设备或标注
        /// </summary>
        /// <param name="objectId">对象ID</param>
        /// <param name="objectIdCollection"> </param>
        /// <returns></returns>
        private Point3dCollection GetSymbolValueByObjectId(ObjectId objectId,ref ObjectIdCollection objectIdCollection)
        {
            var pointCollection = new Point3dCollection();
            try
            {
                if (DBSymbolFinder.Instance.ContainsKey(objectId))
                {
                    if (objectId.IsErased) return pointCollection;
                    var value = DBSymbolFinder.Instance[objectId];
                    var et = PublicMethod.Instance.GetObject(objectId, OpenMode.ForRead) as Entity;
                    if (et is BlockReference || et is Polyline)
                    {
                        //普通标注
                        var listValue =DBSymbolFinder.Instance.Where(o => o.Value.G3E_FID == value.G3E_FID && o.Value.EntityType ==EntityType.Label && !o.Value.IsErased);
                        foreach (var item in listValue)
                        {
                            var entity = PublicMethod.Instance.GetObject(item.Key, OpenMode.ForRead) as Entity;
                            if (entity is DBText)
                            {
                                var dbText = entity as DBText;
                                pointCollection.Add(dbText.Position);
                                objectIdCollection.Add(item.Key);
                            }
                            else if(entity is MText)
                            {
                                var mText = entity as MText;
                                pointCollection.Add(mText.Location);
                                objectIdCollection.Add(item.Key);
                            }
                        }
                        var zxbz = DBEntityFinder.Instance.GetZxbzsByG3e_Fid(value.G3E_FID.ToString());
                        if (zxbz != null)
                        {
                            foreach (var item in zxbz)
                            {
                                var zxpt=DBSymbolFinder.Instance.FirstOrDefault(o => o.Value.G3E_FID == item.G3E_FID);
                                if (!zxpt.Key.IsNull)
                                {
                                    var entity = PublicMethod.Instance.GetObject(zxpt.Key, OpenMode.ForRead) as Entity;
                                    if (entity is DBText)
                                    {
                                        var dbText = entity as DBText;
                                        pointCollection.Add(dbText.Position);
                                        objectIdCollection.Add(zxpt.Key);
                                    }
                                    else if(entity is MText)
                                    {
                                        var mText = entity as MText;
                                        pointCollection.Add(mText.Location);
                                        objectIdCollection.Add(zxpt.Key);
                                    }
                                }
                            }
                        }
                    }
                    else if (et is DBText || et is MText)
                    {
                        IEnumerable<KeyValuePair<ObjectId, ElectronSymbol>> listValue = new List<KeyValuePair<ObjectId, ElectronSymbol>>();
                        //杂项标注
                        if (value.EntityType == EntityType.ZxLabel)
                        {
                            //var zxbz=DBEntityFinder.Instance.GetZxbzByG3e_Fid(value.G3E_FID);
                            //if (zxbz != null)
                            //{
                            //    long dysbfid;
                            //    long.TryParse(zxbz.BZ_DYSB, out dysbfid);
                            //        listValue =
                            //            DBEntityFinder.Instance.GetDBSymbolFinderByFidOrEntityType(
                            //                dysbfid, EntityType.None);
                            //}
                        }
                        else
                            listValue = DBEntityFinder.Instance.GetDBSymbolFinderByFidOrEntityType(value.G3E_FID, EntityType.None);
                        foreach (var item in listValue)
                        {
                            var entity = PublicMethod.Instance.GetObject(item.Key, OpenMode.ForRead) as Entity;
                            if (entity is BlockReference)
                            {
                                var dbText = entity as BlockReference;
                                pointCollection.Add(dbText.Position);
                            }else if(entity is Polyline)
                            {
                                var pl = entity as Polyline;
                                pointCollection.Add(pl.StartPoint);
                            }
                        }
                    }
                }
            }catch(Autodesk.AutoCAD.Runtime.Exception ex)
            {
                LogManager.Instance.Error(ex);
            }
            catch (System.Exception ex)
            {
                LogManager.Instance.Error(ex);
            }
            return pointCollection;
        }
    }
    #endregion
}
