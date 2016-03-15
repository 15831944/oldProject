using System.Collections.Generic;
using System.Linq;
using Autodesk.AutoCAD.ApplicationServices;
using Autodesk.AutoCAD.DatabaseServices;
using Autodesk.AutoCAD.EditorInput;
using Autodesk.AutoCAD.Geometry;
using Autodesk.AutoCAD.Runtime;
using DotNetARX;
using System.Windows.Forms;
using ElectronTransferDal.Common;
using ElectronTransferDal.Query;
using ElectronTransferModel.V9_4;
using ElectronTransferModel;

namespace ElectronTransferDal.Cad
{
    public class AutoConnect
    {
        readonly ObjectIdList serobjs = new ObjectIdList();
        readonly ObjectIdCollection selretangeidc = new ObjectIdCollection();
        static public Dictionary<ObjectId, int> bakdict = new Dictionary<ObjectId, int>();
        #region 半自动连接        
        [CommandMethod("autoc")]
        public void autoc()
        {
            Document doc = Autodesk.AutoCAD.ApplicationServices.Application.DocumentManager.MdiActiveDocument;
            Database db = HostApplicationServices.WorkingDatabase;
            Editor ed = doc.Editor;
            try
            {
                PublicMethod.Instance.AlertDialog("请选择第一个设备:");
                PromptEntityResult per1 = ed.GetEntity("");
                if (per1.Status != PromptStatus.OK) return;
                // 工单判断
                if (DBEntityFinder.Instance.VerifyLTTID(per1.ObjectId) == false)
                {
                    PublicMethod.Instance.ShowMessage("您选择的设备没有被锁定,请重新选择\n");
                    return;
                }
                long fid = 0, id = 0, fno = 0;
                DBEntityFinder.Instance.GetG3EIds(per1.ObjectId, ref id, ref fid, ref fno);
                var conn = DBManager.Instance.GetEntity<Connectivity_n>(fid);
                if (conn == null)
                {
                    ed.WriteMessageWithReturn("该设备没有连接关系,Connectivity_n表的数据。\n");
                    return;
                }
                if (conn.NODE2_ID != 0)
                {
                    PublicMethod.Instance.AlertDialog("该设备已有连接关系。");
                    return;
                }
                long[] sdarr = { 11, 72, 79, 85, 120, 143, 167, 173 };
                if (sdarr.Contains(conn.G3E_FNO))
                {
                    PublicMethod.Instance.AlertDialog("第一个设备不能是仅有一个连接点的设备,如: 接地刀闸,母线");
                    return;
                }
                changCol(per1.ObjectId);

                PublicMethod.Instance.AlertDialog("请选择第二个设备:");
                PromptEntityResult per2 = ed.GetEntity("");
                if (per2.Status != PromptStatus.OK || per1.ObjectId == per2.ObjectId) return;
                // 工单判断
                if (DBEntityFinder.Instance.VerifyLTTID(per2.ObjectId) == false)
                {
                    PublicMethod.Instance.ShowMessage("您选择的设备没有被锁定,请重新选择\n");
                    return;
                }
                // 把两个设备放进要过滤的列表
                if (!serobjs.Contains(per1.ObjectId)) serobjs.Add(per1.ObjectId);
                if (!serobjs.Contains(per2.ObjectId)) serobjs.Add(per2.ObjectId);
                // 得到当前视图
                ViewTableRecord view = ed.GetCurrentView();
                // 获取一个新的id
                int nodeid = CYZCommonFunc.getid();
                DCadApi.isModifySymbol = true;
                // 判断前两个设备是否能相连
                if (obj21obj(nodeid, per1.ObjectId, per2.ObjectId))
                {
                    using (doc.LockDocument())
                    {
                        db.UpdateExt(true);
                        ed.ZoomExtents(); // 视图最大化
                        autoconn(per2.ObjectId);
                        ed.SetCurrentView(view); // 还原视图
                    }
                    TopologyMethods.bChangeTopo = true;
                    PublicMethod.Instance.AlertDialog("连接结束！");
                }
            }
            catch (Exception ex) { ed.WriteMessageWithReturn(ex); }
            finally
            {
                DCadApi.isModifySymbol = false;
                clearCol();
                serobjs.Clear();
            }
        }
        /// <summary>
        /// 传入两个设备ObjectId,自动连接下游
        /// </summary>
        /// <param name="strId">首个设备ID</param>
        /// <param name="endId">下一个设备ID</param>
        public void autocbyobjid(ObjectId strId, ObjectId endId)
        {
            if (strId == endId) return;
            Document doc = Autodesk.AutoCAD.ApplicationServices.Application.DocumentManager.MdiActiveDocument;
            Database db = HostApplicationServices.WorkingDatabase;
            Editor ed = doc.Editor;
            try
            {
                //PublicMethod.Instance.AlertDialog("请选择第一个设备:");
                //PromptEntityResult per1 = ed.GetEntity("");
                //if (per1.Status != PromptStatus.OK) return;
                long fid = 0, id = 0, fno = 0;
                DBEntityFinder.Instance.GetG3EIds(strId, ref id, ref fid, ref fno);
                var conn = DBManager.Instance.GetEntity<Connectivity_n>(fid);
                if (conn == null)
                {
                    ed.WriteMessageWithReturn("该设备没有连接关系\n");
                    return;
                }
                if (conn.NODE2_ID != 0)
                {
                    PublicMethod.Instance.AlertDialog("该设备已有连接关系");
                    return;
                }
                long[] sdarr = { 11, 72, 79, 85, 120, 143, 167, 173 };
                if (sdarr.Contains(conn.G3E_FNO))
                {
                    PublicMethod.Instance.AlertDialog("第一个设备不能是仅有一个连接点的设备,如: 接地刀闸,母线");
                    return;
                }
                changCol(strId);

                // 把两个设备放进要过滤的列表
                if (!serobjs.Contains(strId)) serobjs.Add(strId);
                if (!serobjs.Contains(endId)) serobjs.Add(endId);
                // 得到当前视图
                ViewTableRecord view = ed.GetCurrentView();
                // 获取一个新的id
                int nodeid = CYZCommonFunc.getid();
                DCadApi.isModifySymbol = true;
                // 判断前两个设备是否能相连
                if (obj21obj(nodeid, strId, endId))
                {
                    using(doc.LockDocument())
                    {
                        db.UpdateExt(true);
                        ed.ZoomExtents();  // 视图最大化
                        autoconn(endId);
                        ed.SetCurrentView(view); // 还原视图
                    }
                    PublicMethod.Instance.AlertDialog("连接结束！");
                }
            }
            catch ( Exception ex) { ed.WriteMessageWithReturn(ex); }
            finally
            {
                DCadApi.isModifySymbol = false;
                clearCol();
                serobjs.Clear();
            }
        }
        /// <summary>
        /// startId 的设备 连接到 endId 的设备
        /// </summary>
        /// <param name="nodeid">新增的NodeId</param>
        /// <param name="startId">起始设备ObjectId</param>
        /// <param name="endId">目标设备ObjectId</param>
        /// <returns></returns>
        bool obj21obj(int nodeid, ObjectId startId, ObjectId endId)
        {
            //long[] mxarr = new long[] { 11, 120, 143, 167, 79 };
            long fid = 0, id = 0, fno = 0;
            DBEntityFinder.Instance.GetG3EIds(startId, ref id, ref fid, ref fno);
            var startconn = DBManager.Instance.GetEntity<Connectivity_n>(fid);
            if (startconn == null)
            {
                PublicMethod.Instance.ShowMessage("fid = " + fid + " 该设备没有连接关系表Connectivity_n数据。\n");
                return false;
            }
            DBEntityFinder.Instance.GetG3EIds(endId, ref id, ref fid, ref fno);
            var endconn = DBManager.Instance.GetEntity<Connectivity_n>(fid);
            if (endconn == null)
            {
                PublicMethod.Instance.ShowMessage("fid = " + fid + " 该设备没有连接关系表Connectivity_n数据。\n");
                return false;
            }
            if (endconn.NODE1_ID != 0 || endconn.NODE2_ID != 0 ) //判断是否已经有连接关系
            {
                if (!PublicMethod.Instance.N1isN2.Contains((int) fno)) //如果有连接关系，却是母线，不排除
                {
                    PublicMethod.Instance.ShowMessage("自动连接遇到有连接关系的设备,停止连接。\n");
                    return false;
                }
            }
            // 判断是否为断开状态的开关,如果是,不做连接
            if (endconn.CD_DQZT == "断开") return false;
            // 规范处理
            if (false == CDDBManager.Instance.GetEntities<G3e_nodeedgeconnectivity>(o => o.G3E_SOURCEFNO == startconn.G3E_FNO).Select(o => o.G3E_CONNECTINGFNO).Contains((int)fno))
            {
                PublicMethod.Instance.Editor.WriteMessageWithReturn(
                    FeatureMapping.instance.features[startconn.G3E_FNO.ToString()]
                    + " fid = "
                    + startconn.G3E_FID
                    + " 不能与 "
                    + FeatureMapping.instance.features[fno.ToString()]
                    + " fid = "
                    + fid
                    + "相连!\n");
                return false;
            }
            if (/*(endconn.NODE1_ID == endconn.NODE2_ID) && */endconn.NODE1_ID != 0)
            {
                startconn.NODE2_ID = endconn.NODE1_ID;
                //TopologyMethods.ChangEntStatus(startconn, 2, (endconn.EntityState.ToString().Substring(4, 3)));
                TopologyMethods.ChangEntStatus(startconn, 2, endconn, 1);
            }
            else if (startconn.NODE2_ID != 0)
            {
                endconn.NODE1_ID = startconn.NODE2_ID;
                //TopologyMethods.ChangEntStatus(endconn, 1, (startconn.EntityState.ToString().Substring(4, 3)));
                TopologyMethods.ChangEntStatus(endconn, 1, startconn, 2);
            }
            else
            {
                endconn.NODE1_ID = startconn.NODE2_ID = nodeid;
                TopologyMethods.ChangEntStatus(startconn, 2, "Add");
                TopologyMethods.ChangEntStatus(endconn, 1, "Add");
            }
            TopologyMethods.sinNodDevJudge(startconn);
            TopologyMethods.sinNodDevJudge(endconn);
            //sinNodDevJudge(startconn);
            //sinNodDevJudge(endconn);
            DBManager.Instance.Update(startconn);
            DBManager.Instance.Update(endconn);
            return true;
        }
        /// <summary>
        /// 对objid 的对象作空间分析
        /// </summary>
        /// <param name="objid">对象ObjectId</param>
        public void autoconn(ObjectId objid)
        {
            // 工单判断
            if (DBEntityFinder.Instance.VerifyLTTID(objid) == false) { return; }
            var doc = Autodesk.AutoCAD.ApplicationServices.Application.DocumentManager.MdiActiveDocument;
            var ed = doc.Editor;
            var autoconns = new ObjectIdList();
            var nodeid = CYZCommonFunc.getid();
            var objs = new List<ObjectId>();
            long[] mxarr = { 11, 120, 143, 167, 79 };
            long idd = 0, fidd = 0, fnoo = 0;
            //ObjectIdList dxIdList = new ObjectIdList();
            try
            {
                DCadApi.isModifySymbol = true;
                Entity ent;
                using (Transaction tr = doc.TransactionManager.StartTransaction())
                {
                    ent = tr.GetObject(objid, OpenMode.ForWrite) as Entity;
                    if (ent == null) return;
                    bakdict.Add(ent.ObjectId, ent.ColorIndex);
                    ent.ColorIndex =  4;
                    PromptSelectionResult psr;
                    Point3dCollection p3dcc;
                    if (ent is BlockReference)
                    {
                        string[] dltArr = { "147_1", "151_1", "169_1", "173_0_0", "173_0_1"};
                        var brf = ent as BlockReference;
                        if (dltArr.Contains(brf.Name)) { /*PublicMethod.Instance.Editor.WriteMessageWithReturn("遇上双电缆头,连接终止.\n"); */return; }
                        var ext = ent.GeometricExtents;
                        //brf.Rotation = 0;

                        var leftup = new Point3d(ext.MinPoint.X, ext.MaxPoint.Y, 0);
                        var leftdown = ext.MinPoint;
                        var rightup = ext.MaxPoint;
                        var rightdown = new Point3d(ext.MaxPoint.X, ext.MinPoint.Y, 0);
                        p3dcc = new Point3dCollection(new[] { leftup, leftdown, rightdown, rightup, leftup });
                        psr = ed.SelectCrossingPolygon(p3dcc);
                        objs.AddRange(psr.Value.GetObjectIds());
                    }
                    else if (ent is Polyline)
                    {
                        var entpoly = ent as Polyline;
                        if (entpoly.NumberOfVertices < 2) return;
                        Point3d[] startendpt = { entpoly.StartPoint, entpoly.EndPoint };
                        for (int i = 0; i < 2; i++)
                        {
                            const double lenght = 0.00000040;
                            p3dcc = new Point3dCollection(new[] {
                                                new Point3d(startendpt[i].X - lenght, startendpt[i].Y + lenght, 0),
                                                new Point3d(startendpt[i].X - lenght, startendpt[i].Y - lenght, 0),
                                                new Point3d(startendpt[i].X + lenght, startendpt[i].Y - lenght, 0),
                                                new Point3d(startendpt[i].X + lenght, startendpt[i].Y + lenght, 0),
                                                new Point3d(startendpt[i].X - lenght, startendpt[i].Y + lenght, 0)});
                            psr = ed.SelectCrossingPolygon(p3dcc);
                            if (psr.Status == PromptStatus.OK) objs.AddRange(psr.Value.GetObjectIds());
                        }
                    }
                    tr.Commit();
                }
                foreach (var id in objs.Distinct())
                {
                    if (serobjs.Contains(id)) continue;
                    using (var tr = doc.TransactionManager.StartTransaction())
                    {
                        bool isMx = false;
                        ent = tr.GetObject(id, OpenMode.ForWrite) as Entity;
                        if (ent == null) { continue; }
                        DBEntityFinder.Instance.GetG3EIds(id, ref idd, ref fidd, ref fnoo);
                        // 判断设备有没连接关系
                        var conn = DBManager.Instance.GetEntity<Connectivity_n>(fidd);
                        if (conn == null) { continue; }

                        if (ent is Polyline)
                        {
                            if (((Polyline)ent).StartPoint == ((Polyline)ent).EndPoint) { continue; }
                            // 如果是母线
                            if (mxarr.Contains(fnoo))
                            {
                                isMx = true;
                                if (obj21obj(nodeid, objid, id))
                                {
                                    long temp = 0;
                                    if (conn.G3E_FNO == 79)
                                    {
                                        if (conn.NODE2_ID != null) temp = (long) conn.NODE2_ID;
                                    }
                                    else
                                    {
                                        if (conn.NODE1_ID != null) temp = (long) conn.NODE1_ID;
                                    }
                                    // 判断母线是否有连接关系
                                    if (temp != 0)
                                    {
                                        // 查找出1头与母线相连的导线集合
                                        var conns =
                                            DBManager.Instance.GetEntities<Connectivity_n>(o => o.NODE1_ID == temp);
                                        // 根据导线的FID打开实体,作几何分析,寻找周边设备
                                        if (conns.Any())
                                        {
                                            var dlgRes = MessageBox.Show("发现母线的下游有连接设备。\n是否往其下游继续连接?", "AutoCad",
                                                MessageBoxButtons.YesNo, MessageBoxIcon.Warning);
                                            if (dlgRes == DialogResult.No)
                                            {
                                                continue;
                                            }
                                            foreach (var dx in conns)
                                                // 导线的2头为0
                                                if (dx.NODE2_ID == 0)
                                                {
                                                    var dxId = DBEntityFinder.Instance.GetObjectIdByFid(dx.G3E_FID);
                                                    autoconns.Add(dxId);
                                                }
                                        }
                                    }
                                }
                                serobjs.Add(id);
                            }
                        }
                        else if (ent is MText) { continue; }  // 如果是标注
                        else if (ent is DBText) { continue; } // 如果是杂项标注

                        if (!isMx) 
                        {
                            if (obj21obj(nodeid, objid, id))
                            {
                                serobjs.Add(id);
                                autoconns.Add(id);
                            }
                        } 
                        tr.Commit();
                    }
                }

                foreach (var id in autoconns)
                {
                    autoconn(id);
                }
            }
            catch (Exception ex) { ed.WriteMessageWithReturn(ex); }
            finally { DCadApi.isModifySymbol = false; }
        }
        /// <summary>
        /// 单节点设备处理
        /// </summary>
        /// <param name="conn"></param>
        void sinNodDevJudge(Connectivity_n conn)
        {
            //int[] sinNodDevArr = new int[] { 11, 72, 79, 85, 120, 143, 159, 167, 173 }; // 单节点
            //int[] sinNodDevArr = new int[] { 143, 159, 79, 151 };
            //if (!PublicMethod.Instance.N1isN2.Concat(PublicMethod.Instance.N2is0).Contains(conn.G3E_FNO)) return;
            if (!PublicMethod.Instance.N1isN2.Contains(conn.G3E_FNO)) return;
            if (conn.NODE1_ID == 0 && conn.NODE2_ID != 0) conn.NODE1_ID = conn.NODE2_ID;
            else if (conn.NODE2_ID == 0 && conn.NODE1_ID != 0) conn.NODE2_ID = conn.NODE1_ID;
        }
        /// <summary>
        /// 清空颜色
        /// </summary>
        public void clearCol()
        {
            Document doc = Autodesk.AutoCAD.ApplicationServices.Application.DocumentManager.MdiActiveDocument;
            //Database db = HostApplicationServices.WorkingDatabase;
            Editor ed = doc.Editor;
            try
            {
                DCadApi.isModifySymbol = true;
                using (doc.LockDocument())
                {
                    using (var tr = doc.TransactionManager.StartTransaction())
                    {
                        foreach (var id in bakdict.Keys.Distinct())
                        {
                            var ent = tr.GetObject(id, OpenMode.ForWrite) as Entity;
                            if (ent == null) continue;
                            ent.ColorIndex = bakdict[id];
                        }
                        bakdict.Clear();
                        tr.Commit();
                    }
                }
                //PublicMethod.SendCommend("re\n");
            }
            catch (Exception ex) { ed.WriteMessageWithReturn(ex); }
            finally { DCadApi.isModifySymbol = false; }
        }
        /// <summary>
        /// 单实体颜色变色
        /// </summary>
        /// <param name="_objid">实体ObjectID</param>
        void changCol(ObjectId _objid)
        {
            Document doc = Autodesk.AutoCAD.ApplicationServices.Application.DocumentManager.MdiActiveDocument;
            Database db = HostApplicationServices.WorkingDatabase;
            try
            {
                DCadApi.isModifySymbol = true;
                using (doc.LockDocument())
                {
                    using (Transaction tr = db.TransactionManager.StartTransaction())
                    {
                        var ent = tr.GetObject(_objid, OpenMode.ForWrite) as Entity;
                        if (ent != null)
                        {
                            bakdict.Add(_objid, ent.ColorIndex);
                            ent.ColorIndex = 4;
                        }
                        tr.Commit();
                    }
                }
            }
            catch (System.Exception ex)
            {
                PublicMethod.Instance.Editor.WriteMessageWithReturn(ex.Message + "\n");
            }
            finally
            {
                DCadApi.isModifySymbol = false;
            }
        }
        #endregion

        #region
        //public void autoconn2(ObjectId objid)
        //{
        //    Document doc = Autodesk.AutoCAD.ApplicationServices.Application.DocumentManager.MdiActiveDocument;
        //    Database db = HostApplicationServices.WorkingDatabase;
        //    Editor ed = doc.Editor;
        //    Entity ent = null;
        //    Extents3d ext;
        //    Point3dList p3dlist = new Point3dList();
        //    try
        //    {
        //        ObjectIdList objs = new ObjectIdList();
        //        PromptSelectionResult psr = null;
        //        Point3dCollection p3dcc = null;
        //        using (Transaction tr = doc.TransactionManager.StartTransaction())
        //        {
        //            /************************************************************************/
        //            BlockTable bt = (BlockTable)(tr.GetObject(db.BlockTableId, OpenMode.ForRead));
        //            BlockTableRecord btr = (BlockTableRecord)tr.GetObject(bt[BlockTableRecord.ModelSpace], OpenMode.ForWrite);
        //            /************************************************************************/
        //            ent = tr.GetObject(objid, OpenMode.ForRead) as Entity;
        //            if (ent == null) return;
        //            if (ent is BlockReference)
        //            {
        //                ext = ent.GeometricExtents;
        //                //psr = ed.SelectCrossingWindow(ext.MaxPoint, ext.MinPoint);
        //                Point3d leftup, leftdown, rightup, rightdown;
        //                leftup = new Point3d(ext.MinPoint.X, ext.MaxPoint.Y, 0);
        //                leftdown = ext.MinPoint;
        //                rightup = ext.MaxPoint;
        //                rightdown = new Point3d(ext.MaxPoint.X, ext.MinPoint.Y, 0);
        //                //Point3dCollection p3dc = new Point3dCollection(new Point3d[] { leftup, leftdown, rightdown, rightup, leftup });
        //                p3dcc = new Point3dCollection(new Point3d[] { leftup, leftdown, rightdown, rightup, leftup });
        //                psr = ed.SelectCrossingPolygon(p3dcc);
        //                objs.AddRange(psr.Value.GetObjectIds());
        //                /************************************************************************/
        //                Polyline pl = new Polyline();
        //                //pl.CreateRectangle(new Point2d(ext.MaxPoint.X, ext.MaxPoint.Y), new Point2d(ext.MinPoint.X, ext.MinPoint.Y));
        //                pl.CreatePolyline(p3dcc);
        //                pl.ColorIndex = 5;
        //                btr.AppendEntity(pl);
        //                tr.AddNewlyCreatedDBObject(pl, true);
        //                if (!serobjs.Contains(pl.ObjectId)) serobjs.Add(pl.ObjectId);
        //                selretangeidc.Add(pl.ObjectId);
        //                /************************************************************************/
        //            }
        //            else if (ent is Polyline)
        //            {
        //                Polyline entpoly = ent as Polyline;
        //                if (entpoly.NumberOfVertices < 2) return;
        //                //int[] startendind = { 0, entpoly.NumberOfVertices};
        //                Point3d[] startendpt = { entpoly.StartPoint, entpoly.EndPoint };
        //                for (int i = 0; i < 2; i++)
        //                {
        //                    p3dlist.Add(startendpt[i]);
        //                    //p3dlist.Add(new Point3d(startendpt[i].X - 0.00000030, startendpt[i].Y + 0.00000030, 0));
        //                    p3dcc = new Point3dCollection(new Point3d[] {
        //                                            new Point3d(startendpt[i].X - 0.00000030, startendpt[i].Y + 0.00000030, 0),
        //                                            new Point3d(startendpt[i].X - 0.00000030, startendpt[i].Y - 0.00000030, 0),
        //                                            new Point3d(startendpt[i].X + 0.00000030, startendpt[i].Y - 0.00000030, 0),
        //                                            new Point3d(startendpt[i].X + 0.00000030, startendpt[i].Y + 0.00000030, 0),
        //                                            new Point3d(startendpt[i].X - 0.00000030, startendpt[i].Y + 0.00000030, 0)});
        //                    psr = ed.SelectCrossingPolygon(p3dcc);
        //                    if (psr.Status == PromptStatus.OK) objs.AddRange(psr.Value.GetObjectIds());
        //                    /************************************************************************/
        //                    Polyline pl = new Polyline();
        //                    pl.CreatePolyline(p3dcc);
        //                    pl.ColorIndex = 5;
        //                    btr.AppendEntity(pl);
        //                    tr.AddNewlyCreatedDBObject(pl, true);
        //                    if (!serobjs.Contains(pl.ObjectId)) serobjs.Add(pl.ObjectId);
        //                    selretangeidc.Add(pl.ObjectId);
        //                    /************************************************************************/
        //                }
        //            }
        //            tr.Commit();
        //        }
        //        /**/
        //        int nodeid = CYZCommonFunc.getid();
        //        ObjectIdList autoconns = new ObjectIdList();
        //         /**/
        //        foreach (var i in objs)
        //        {
        //            if (!serobjs.Contains(i))
        //            {
        //                using (Transaction tr = doc.TransactionManager.StartTransaction())
        //                {
        //                    ent = tr.GetObject(i, OpenMode.ForWrite) as Entity;
        //                    if (ent == null) return;
        //                    if (ent is MText) return;
        //                    if (ent is DBText) return;
        //                    if (ent is Polyline)
        //                    {
        //                        if (((Polyline)ent).StartPoint == ((Polyline)ent).EndPoint)
        //                            continue;
        //                        long idd = 0, fidd = 0, fnoo = 0;
        //                        PublicMethod.GetG3EIds(ent.ObjectId, ref idd, ref fidd, ref fnoo);
        //                        long[] mxarr = new long[] { 11, 120, 143, 167, 79 };
        //                        if (mxarr.Contains(fnoo)) return;
        //                    }
        //                    bakdict.Add(ent.ObjectId, ent.ColorIndex);
        //                    ent.ColorIndex = 4;

        //                    if (!serobjs.Contains(i)) serobjs.Add(i);
        //                    obj21obj(nodeid, objid, i);
        //                    autoconns.Add(i);

        //                    tr.Commit();
        //                }
        //                //autoconn(i);
        //            }
        //        }

        //        foreach (var i in autoconns)
        //        {
        //            autoconn(i);
        //        }
        //    }
        //    catch (Autodesk.AutoCAD.Runtime.Exception ex) { ed.WriteMessageWithReturn(ex); }
        //}
        #endregion

        #region [CommandMethod("shre")]
        //[CommandMethod("shre")]
        //public void autoca()
        //{
        //    var doc = Autodesk.AutoCAD.ApplicationServices.Application.DocumentManager.MdiActiveDocument;
        //    var db = HostApplicationServices.WorkingDatabase;
        //    var ed = doc.Editor;
        //    try
        //    {
        //        var per1 = ed.GetEntity("ssssqqqq\n");
        //        if (per1.Status != PromptStatus.OK) return;
        //        using (var tr = doc.TransactionManager.StartTransaction())
        //        {
        //            var bt = (BlockTable)(tr.GetObject(db.BlockTableId, OpenMode.ForRead));
        //            var btr = (BlockTableRecord)tr.GetObject(bt[BlockTableRecord.ModelSpace], OpenMode.ForWrite);
        //            var ent1 = tr.GetObject(per1.ObjectId, OpenMode.ForRead) as Entity;
        //            if (ent1 == null) return;
        //            if (ent1 is Polyline)
        //            {
        //                var pp = ent1 as Polyline;
        //                ed.WriteMessage(pp.Closed);
        //                ed.WriteMessage("\n");
        //            }
        //            else if (ent1 is BlockReference)
        //            {
        //                var ext = ent1.GeometricExtents;
        //                var pl = new Polyline();
        //                pl.CreateRectangle(new Point2d(ext.MaxPoint.X, ext.MaxPoint.Y), new Point2d(ext.MinPoint.X, ext.MinPoint.Y));
        //                pl.ColorIndex = 2;
        //                btr.AppendEntity(pl);
        //                tr.AddNewlyCreatedDBObject(pl, true);
        //                if (!serobjs.Contains(pl.ObjectId)) serobjs.Add(pl.ObjectId);
        //            }
        //            //ent2 = tr.GetObject(per2.ObjectId, OpenMode.ForRead) as Entity;
        //            tr.Commit();
        //        }
        //    }
        //    catch (Exception ex) { ed.WriteMessageWithReturn(ex); }
        //}
        #endregion

        //#region [CommandMethod("autoc1")]
        //[CommandMethod("autoc1")]
        //public void autocb()
        //{
        //    Document doc = Autodesk.AutoCAD.ApplicationServices.Application.DocumentManager.MdiActiveDocument;
        //    Database db = HostApplicationServices.WorkingDatabase;
        //    Editor ed = doc.Editor;
        //    try
        //    {
        //        //ed.WriteMessage("Hello World\n");
        //        PromptPointResult per1 = ed.GetPoint("point:\n");
        //        if (per1.Status != PromptStatus.OK) return;
        //        Point3d pt = per1.Value;

        //        using (var tr = doc.TransactionManager.StartTransaction())
        //        {
        //            var bt = (BlockTable)(tr.GetObject(db.BlockTableId, OpenMode.ForRead));
        //            var btr = (BlockTableRecord)tr.GetObject(bt[BlockTableRecord.ModelSpace], OpenMode.ForWrite);

        //            const double lenght = 0.00000040;
        //            var leftup = pt.Add(new Vector3d(-lenght, lenght, 0));
        //            var leftdown = pt.Add(new Vector3d(-lenght, -lenght, 0));
        //            var rightup = pt.Add(new Vector3d(lenght, lenght, 0));
        //            var rightdown = pt.Add(new Vector3d(lenght, -lenght, 0));
        //            var p3dc = new Point3dCollection(new[] { leftup, leftdown, rightdown, rightup, leftup });
        //            var psr = ed.SelectCrossingPolygon(p3dc);
        //            if (psr.Status != PromptStatus.OK) return;
        //            ed.WriteMessageWithReturn(psr.Value.Count);

        //            var pl = new Polyline();
        //            pl.CreateRectangle(new Point2d(leftup.X, leftup.Y), new Point2d(rightdown.X, rightdown.Y));
        //            pl.ColorIndex = 2;
        //            btr.AppendEntity(pl);
        //            tr.AddNewlyCreatedDBObject(pl, true);
        //            tr.Commit();
        //        }
        //    }
        //    catch (Autodesk.AutoCAD.Runtime.Exception ex) { ed.WriteMessageWithReturn(ex); }
        //}
        //#endregion

        //#region [CommandMethod("cls2")]
        //[CommandMethod("cls2")]
        //public void cls2()
        //{
        //     var doc = Autodesk.AutoCAD.ApplicationServices.Application.DocumentManager.MdiActiveDocument;
        //     //Database db = HostApplicationServices.WorkingDatabase;
        //     var ed = doc.Editor;
        //     try
        //     {
        //         //ed.WriteMessage("Hello World\n");
        //         DCadApi.isModifySymbol = true;
        //         using (Transaction tr = doc.TransactionManager.StartTransaction())
        //         {
        //             Entity ent;
        //             foreach (var id in selretangeidc)
        //             {
        //                 ent = tr.GetObject(((ObjectId)id), OpenMode.ForRead) as Entity;
        //                 if (ent == null) continue;
        //                 ((ObjectId)id).Erase();
        //             }
        //             foreach (var id in bakdict.Keys)
        //             {
        //                 ent = tr.GetObject(id, OpenMode.ForWrite) as Entity;
        //                 if (ent == null) continue;
        //                 ent.ColorIndex = bakdict[id];
        //             }
        //             tr.Commit();
        //         }
        //         PublicMethod.Instance.SendCommend("re\n");
        //     }
        //     catch (Exception ex) { ed.WriteMessageWithReturn(ex); }
        //     finally { DCadApi.isModifySymbol = false; }
        //}
        //#endregion
    }
}
