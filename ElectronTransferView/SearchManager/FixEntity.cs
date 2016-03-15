using System;
using ArxMap;
using Autodesk.AutoCAD.DatabaseServices;
using Autodesk.AutoCAD.EditorInput;
using Autodesk.AutoCAD.Geometry;
using DotNetARX;
using ElectronTransferDal.Common;
using ElectronTransferDal.Query;
using ElectronTransferFramework;
using ElectronTransferDal.Cad;
using Autodesk.AutoCAD.ApplicationServices;
using ElectronTransferModel.V9_4;

namespace ElectronTransferView.SearchManager
{
    public class FixEntity:Singleton<FixEntity>
    {
        //记录改变查找到的实体和其对应的颜色值
        private  ObjectId _FindEntObjId = ObjectId.Null;
        private  Autodesk.AutoCAD.Colors.Color _FindEntCol;
        private ObjectId coordinateObjId = ObjectId.Null;
        /// <summary>
        /// 根据fid定位实体
        /// </summary>
        /// <param name="DevFID">实体的fid</param>
        public void Fix(long DevFID)
        {
            try
            {
               
                //先还原之前的实体的颜色
                ResetOldEntity();
                DCadApi.isModifySymbol = true;
                //给当前实体变色
                ObjectId objId = DBEntityFinder.Instance.GetObjectIdByFid(DevFID);
                if (!objId.IsNull)
                {
                    using (PublicMethod.Instance.acDoc.LockDocument())
                    {
                        using (Transaction transaction = PublicMethod.Instance.acDoc.TransactionManager.StartTransaction())
                        {
                            try
                            {
                                var temp = transaction.GetObject(objId, OpenMode.ForRead,true);
                                if (temp == null || temp.IsErased)
                                {
                                    PublicMethod.Instance.ShowMessage("\n当前设备没有对应的图形符号或实体已被删除！！！");
                                    return;
                                }
                                Entity ent = temp as Entity;
                                if (ent == null || ent is DBText) return;
                                ent.UpgradeOpenAndRun();
                                //记录当前实体的颜色
                                _FindEntObjId = objId;
                                _FindEntCol = ent.Color;
                                //设置当前实体的定位色为绿色
                                ent.Color = Autodesk.AutoCAD.Colors.Color.FromRgb(0, 255, 0);
                                transaction.Commit();
                                ClickFixEntity(objId);
                            }
                            catch (Autodesk.AutoCAD.Runtime.Exception )
                            {
                                _FindEntObjId = ObjectId.Null;
                            }
                        }
                    }
                }
                else
                {
                    PublicMethod.Instance.ShowMessage("\n当前设备没有对应的图形符号,可能会定位到它的关联设备！！！");
                    //如果没找到就看看他的从属关系如198
                    //根据该开关柜找到电房
                    var Common = DBManager.Instance.GetEntity<Common_n>(DevFID);
                    if (Common != null)
                    {
                        try
                        {
                            var dfCommon =
                                DBManager.Instance.GetEntity<Common_n>(o => o.G3E_ID == Common.OWNER1_ID);
                            if (dfCommon != null)
                                Fix(dfCommon.G3E_FID);
                            else if(Common.G3E_FNO==41)
                            {
                                var jlb = DBManager.Instance.GetEntity<Gg_jx_jlb_pt>(Common.G3E_FID);
                                if (jlb != null)
                                {
                                    var jlbRef =
                                        DBManager.Instance.GetEntity<Detailreference_n>(
                                            o => o.G3E_DETAILID == jlb.G3E_DETAILID);
                                    if(jlbRef!=null)
                                        Fix(jlbRef.G3E_FID);
                                }
                            }
                        }
                        catch
                        {
                        }
                    }
                    else
                    {
                        try
                        {
                            var shb = DBManager.Instance.GetEntity<Gg_pd_dyshb_n>(DevFID);
                            if (shb != null)
                            {
                                var shbd = DBManager.Instance.GetEntity<Gg_jx_shbd_pt>(DevFID);
                                if (shbd != null)
                                {
                                    var shbdRef =
                                        DBManager.Instance.GetEntity<Detailreference_n>(
                                            o => o.G3E_DETAILID == shbd.G3E_DETAILID);
                                    if (shbdRef != null)
                                    {
                                        Fix(shbdRef.G3E_FID);
                                    }
                                }
                            }
                        }
                        catch
                        {
                        }

                    }
                    
                }
            }
            catch (SystemException ex)
            {
                LogManager.Instance.Error(ex);
            }
            finally
            {
                DCadApi.isModifySymbol = false;
            }

        }
        /// <summary>
        /// 复位实体的颜色
        /// </summary>
        public void ResetOldEntity()
        {
            if (!_FindEntObjId.IsNull)
            {
                using (Transaction trans = PublicMethod.Instance.acDoc.TransactionManager.StartTransaction())
                {
                    using (PublicMethod.Instance.acDoc.LockDocument())
                    {
                        try
                        {
                            DCadApi.isModifySymbol = true;
                            var tempEnt = trans.GetObject(_FindEntObjId, OpenMode.ForRead,true);
                            if (tempEnt == null || tempEnt.IsErased || tempEnt is DBText)
                            {
                                _FindEntObjId = ObjectId.Null;
                                return;
                            }
                            tempEnt.UpgradeOpenAndRun();
                            Entity entity = tempEnt as Entity;
                            entity.Color = _FindEntCol;
                            trans.Commit();
                            _FindEntObjId = ObjectId.Null;
                        }
                        catch (Autodesk.AutoCAD.Runtime.Exception)
                        {
                            //说明中途该实体已被变色或是删除，如改变生命周期
                            _FindEntObjId = ObjectId.Null;
                        }
                        finally
                        {
                            DCadApi.isModifySymbol = false;
                        }
                      
                    }
                }
            }
        }

        /// <summary>
        /// 重新设置实体记录
        /// </summary>
        /// <param name="newObjId">新实体的objid</param>
        /// <param name="oldObjId">旧实体的objid</param>
        public void ResetEntityRecord(ObjectId newObjId,ObjectId oldObjId)
        {
            try
            {
                DCadApi.isModifySymbol = true;
                using (Transaction trans = PublicMethod.Instance.acDoc.TransactionManager.StartTransaction())
                {
                    using (PublicMethod.Instance.acDoc.LockDocument())
                    {
                        var temp = trans.GetObject(newObjId, OpenMode.ForRead,true);
                        Entity ent = temp as Entity;
                        if (ent != null)
                        {
                            //有可能是只改了颜色
                            //这里是把新的实体的objId和color存起来，替换旧的，因为实体被更新后这些东西发生了改变
                            _FindEntObjId = newObjId;
                            _FindEntCol = ent.Color;
                            //ent.UpgradeOpenAndRun();
                            //ent.Color = Autodesk.AutoCAD.Colors.Color.FromRgb(0, 255, 0);
                            trans.Commit();
                        }
                    }
                }
            }
            catch (Autodesk.AutoCAD.Runtime.Exception)
            {
                //如果实体发生改变颜色自动会跟着改变，否则颜色也是不会改变的
                _FindEntObjId = oldObjId;
            }
            finally
            {
                DCadApi.isModifySymbol = false;
            }
        }

        /// <summary>
        /// 坐标定位
        /// </summary>
        /// <param name="X">定位点X坐标</param>
        /// <param name="Y">定位点Y坐标</param>
        /// <param name="flag">是否显示标记</param>
        public void CoordinateFix(double X, double Y,bool flag)
        {
            using (Transaction trans = PublicMethod.Instance.acDoc.TransactionManager.StartTransaction())
            {
                using (PublicMethod.Instance.acDoc.LockDocument())
                {
                    PublicMethod.Instance.SendCommend("zoom\n" + "c\n" + string.Format("{0},{1} ", X, Y) + "0.4865222729561714810200100517294e-4" + "\n");
                    if (flag)
                    {
                        DBPoint pt = new DBPoint();
                        pt.Position = new Point3d(X, Y, 0);
                        pt.ColorIndex = 1;
                        var layerId = DCadApi.addLayer("标记层");
                        pt.LayerId = layerId;
                        PublicMethod.Instance.DB.AddToModelSpace(pt);
                        coordinateObjId = pt.ObjectId;
                        trans.Commit();
                        PublicMethod.Instance.wrapup = false;
                        PublicMethod.Instance.SendCommend("PDMODE\n" + "99\n");
                    }
                    
                }
            }
        }

        public void RemoveFlag()
        {
            try
            {
                DCadApi.isModifySymbol = true;

                using (Transaction trans = PublicMethod.Instance.acDoc.TransactionManager.StartTransaction())
                {
                    using (PublicMethod.Instance.acDoc.LockDocument())
                    {
                        var temp = trans.GetObject(coordinateObjId, OpenMode.ForRead,true);
                        Entity ent = temp as Entity;
                        if (ent != null)
                        {
                            coordinateObjId.Erase();
                            trans.Commit();
                            coordinateObjId = ObjectId.Null;
                        }
                    }
                }


            }
            catch (Exception)
            {
                coordinateObjId = ObjectId.Null;
            }
            finally
            {
                DCadApi.isModifySymbol = false;
            }
        }


        public  void ClickFixEntity(ObjectId objId)
        {
            DCadApi.isModifySymbol = true;
            try
            {
                var ent = PublicMethod.Instance.GetObject(objId, OpenMode.ForRead);
                if (ent!=null)
                {
                    Entity entity = ent as Entity;
                    if (entity is BlockReference)
                    {
                        var blockReference = ent as BlockReference;
                        if(blockReference!=null)
                            Zoom(blockReference.Position, 0.4373044545912342962040020103459e-4);

                    }
                    else if (entity is Polyline)
                    {
                        PolylineFixed(entity);
                    }
                }
            }
            catch
            {
                PublicMethod.Instance.ShowMessage("定位失败！！！");
            }
            finally
            {
                DCadApi.isModifySymbol = false;
            }
        }

        private  void PolylineFixed(Entity ent)
        {
            Document doc = PublicMethod.Instance.acDoc;
            Editor ed = doc.Editor;
            try
            {
                Extents3d exts = ent.GeometricExtents;
                string str = string.Format("{0},{1} ", exts.MinPoint.X, exts.MinPoint.Y);
                string str1 = string.Format("{0},{1}", exts.MaxPoint.X, exts.MaxPoint.Y);
                G3EObject ids = null;
                DBEntityFinder.Instance.GetG3EIds(ent.ObjectId, ref ids);
                string mid = string.Format("{0},{1}",
                      ((exts.MaxPoint.X - exts.MinPoint.X) / 2 + exts.MinPoint.X).ToString(),
                      ((exts.MaxPoint.Y - exts.MinPoint.Y) / 2 + exts.MinPoint.Y).ToString()
                      );
                if (ids.G3E_FNO == 144)//站房引线
                    doc.SendStringToExecute("zoom\n" + "c\n" + mid + "\n" + "0.6373044545912342962040020103459e-5" + "\n", true, false, true);
                else if (ids.G3E_FNO == 143)//站房母线
                    doc.SendStringToExecute("zoom\n" + "c\n" + mid + "\n" + "0.4373044545912342962040020103459e-4" + "\n", true, false, true);
                else if (ids.G3E_FNO == 142 || ids.G3E_FNO == 163 || ids.G3E_FNO == 149)
                {
                    doc.SendStringToExecute("zoom\n" + "c\n" + mid + "\n" + "0.3373044545912342962040020103459e-4" + "\n", true, false, true);
                }
                else
                    doc.SendStringToExecute("zoom\n" + str + str1 + "\n", true, false, true);
                mouse.showmap();
            }
            catch
            {

            }

        }


        private void Zoom(Point3d ptCenter, double factor)
        {
            //Extents3d extents;
            Editor ed = PublicMethod.Instance.acDoc.Editor;
            Document doc = ed.Document;
            Database db = doc.Database;
            //Point3d ptMin=new Point3d();
            //Point3d  ptMax = new Point3d();
            using (Transaction trans = db.TransactionManager.StartTransaction())
            {
                ViewTableRecord view = ed.GetCurrentView();
                //Matrix3d matWCS2DCS = view.Wcs2Dcs();
                //if (ptCenter.DistanceTo(Point3d.Origin) != 0)
                //{
                //    ptMin = new Point3d(ptCenter.X - view.Width / 2, ptCenter.Y - view.Height / 2, 0);
                //    ptMax = new Point3d(ptCenter.X + view.Width / 2, ptCenter.Y + view.Height / 2, 0);
                //}
                //using (Line line = new Line(ptMin, ptMax))
                //{
                //    extents = new Extents3d(line.GeometricExtents.MinPoint, line.GeometricExtents.MaxPoint);
                //}
                //double viewRatio = view.Width / view.Height;
                //extents.TransformBy(matWCS2DCS);
                //double width, height;
                //Point2d newCenter;
                //if (ptCenter.DistanceTo(Point3d.Origin) != 0)
                //{
                //    width = view.Width;
                //    height = view.Height;
                //    if (factor == 0)
                //    {
                //        ptCenter = ptCenter.TransformBy(matWCS2DCS);
                //    }
                //    newCenter = new Point2d(ptCenter.X, ptCenter.Y);
                //}
                //else
                //{
                //    width = extents.MaxPoint.X - extents.MinPoint.X;
                //    height = extents.MaxPoint.Y - extents.MinPoint.Y;
                //    newCenter = new Point2d((extents.MaxPoint.X + extents.MinPoint.X) * 0.5, (extents.MaxPoint.Y + extents.MinPoint.Y) * 0.5);
                //}
                //if (width > height * viewRatio) height = width / viewRatio;
                //if (factor != 0)
                //{
                //    view.Height = factor;
                //    view.Width =  factor;
                //}
                view.CenterPoint = new Point2d(ptCenter.X, ptCenter.Y);
                view.Height = factor;
                view.Width = factor;
                ed.SetCurrentView(view);
                trans.Commit();
            }
        }
    }
}
