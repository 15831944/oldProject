using System;
using System.Collections.Generic;
using System.Linq;
using Autodesk.AutoCAD.Geometry;
using ElectronTransferDal.Common.Exceptions;
using ElectronTransferDal.Query;
using ElectronTransferFramework;
using ElectronTransferModel.Config;
using ElectronTransferModel.V9_4;
using ElectronTransferModel;
using Autodesk.AutoCAD.Runtime;
using Autodesk.AutoCAD.ApplicationServices;
using Autodesk.AutoCAD.EditorInput;
using DotNetARX;
using Autodesk.AutoCAD.DatabaseServices;
using ElectronTransferDal.Common;
using ElectronTransferModel.Base;
using AcadApp = Autodesk.AutoCAD.ApplicationServices.Application;

namespace ElectronTransferDal.Cad
{
    public enum SelectRule
    {
        None = 0, Role, Line, Owner
    }
    public class TopologyMethods
    {
        public static bool bChangeTopo = true;

        #region 特殊设备处理
        public static void sinNodDevJudge(Connectivity_n conn)
        {
            if (PublicMethod.Instance.N2is0.Contains(conn.G3E_FNO))
            {
                conn.NODE2_ID = 0;
            }
            else if (PublicMethod.Instance.N1isN2.Contains(conn.G3E_FNO))
            {
                conn.NODE2_ID = conn.NODE1_ID;
                //  是否对2头的EntityState作操作
                if (conn.EntityState.ToString().Length > 8)
                {
                    ChangEntStatus(conn, 2, conn.EntityState.ToString().Substring(4, 3));
                }
            }
        }

        #endregion

        #region 实体连接EntityStatus处理

        public static void ChangEntStatus(Connectivity_n dest_conn, int dest_index, Connectivity_n source_conn, int source_index)
        {
            if (source_conn.EntityState == EntityState.None || 
                    source_conn.EntityState == EntityState.Update ||
                        source_conn.EntityState == EntityState.Insert )
            {
                ChangEntStatus(dest_conn, dest_index, "Nal");
            }
            else
            {
                if (source_index == 1)
                {
                    ChangEntStatus(dest_conn, dest_index, (source_conn.EntityState.ToString().Substring(4, 3)));
                }
                else if (source_index == 2)
                {
                    ChangEntStatus(dest_conn, dest_index, (source_conn.EntityState.ToString().Substring(8, 3)));
                }
            }
        }

        public static void ChangEntStatus(Connectivity_n conn, int nodindx, string DelOrAdd)
        {
            switch (conn.EntityState)
            {
                case EntityState.Insert:
                    conn.EntityState = ChangEntStatus(EntityState.Add_Nal_Nal, nodindx, DelOrAdd);
                    break;
                case EntityState.None:
                    conn.EntityState = ChangEntStatus(EntityState.Old_Nal_Nal, nodindx, DelOrAdd);
                    break;
                case EntityState.Update:
                    conn.EntityState = ChangEntStatus(EntityState.Old_Nal_Nal, nodindx, DelOrAdd);
                    break;
                default:
                    conn.EntityState = ChangEntStatus(conn.EntityState, nodindx, DelOrAdd);
                    break;
            }
        }

        public static EntityState ChangEntStatus(EntityState entState, int startIndex, string strSubState)
        {
            if (startIndex == 1) startIndex = 4;
            else if (startIndex == 2) startIndex = 8;
            else return entState;
            string strStatus = entState.ToString();
            strStatus = strStatus.Remove(startIndex, 3);
            strStatus = strStatus.Insert(startIndex, strSubState);
            entState = (EntityState) Enum.Parse(typeof (EntityState), strStatus);
            return entState;
        }

        #endregion

        #region 批量打断
        [CommandMethod("breakall")]
        static public void breakall()
        {
            Editor ed = Application.DocumentManager.MdiActiveDocument.Editor;
            ed.SetImpliedSelection(new ObjectId[0]);
            PublicMethod.Instance.AlertDialog("请选择要打断的设备");
            PromptSelectionResult psr = ed.GetSelection();
            if (psr.Status != PromptStatus.OK) return;
            ObjectIdList objlist = psr.Value.GetObjectIds();
            breakall(objlist);
        }
        static public void breakall(ObjectIdList objIdList)
        {
            DCadApi.isModifySymbol = true;
            Document doc = Application.DocumentManager.MdiActiveDocument;
            Editor ed = doc.Editor;
            try
            {
                long g3eid = 0, g3efid = 0, g3efno = 0;
                using (Transaction tr = doc.TransactionManager.StartTransaction())
                {
                    var breObjsNodList = new List<long>();
                    foreach (var id in objIdList)
                    {
                        DBEntityFinder.Instance.GetG3EIds(id, ref g3eid, ref g3efid, ref g3efno);
                        var conn = DBManager.Instance.GetEntity<Connectivity_n>(g3efid);
                        if (conn == null) continue;
                        if (DBEntityFinder.Instance.VerifyLTTID(conn.G3E_FID) == false) continue;
                        if (conn.NODE1_ID != 0) if (conn.NODE1_ID != null) breObjsNodList.Add((long)conn.NODE1_ID);
                        conn.NODE1_ID = 0;
                        if (conn.NODE2_ID != 0) if (conn.NODE2_ID != null) breObjsNodList.Add((long)conn.NODE2_ID);
                        conn.NODE2_ID = 0;

                        string strr = conn.EntityState.ToString().Substring(0, 3);
                        if (strr == "Ins" || strr == "Add")
                        {
                            conn.EntityState = EntityState.Add_Del_Del;
                        }
                        else
                        {
                            conn.EntityState = EntityState.Old_Del_Del;
                        }
                        DBManager.Instance.Update(conn);
                    }
                    foreach (var nod in breObjsNodList.Distinct())
                    {
                        long nod1 = nod;
                        var conns1 = DBManager.Instance.GetEntities<Connectivity_n>(o => o.NODE1_ID == nod1).Where(o => o.EntityState != EntityState.Delete && o.EntityState != EntityState.InsertDelete);
                        long nod2 = nod;
                        var conns2 = DBManager.Instance.GetEntities<Connectivity_n>(o => o.NODE2_ID == nod2).Where(o => o.EntityState != EntityState.Delete && o.EntityState != EntityState.InsertDelete);
                        conns1 = conns1.Concat(conns2).Distinct(new ElectronBaseCompare<Connectivity_n>());
                        if (conns1.Count() == 1)
                        {
                            var conn = conns1.FirstOrDefault();
                            if (conn != null && DBEntityFinder.Instance.VerifyLTTID(conn.G3E_FID))
                            {
                                if (conn.NODE1_ID == nod)
                                {
                                    conn.NODE1_ID = 0;
                                    ChangEntStatus(conn, 1, "Del");
                                }
                                else
                                {
                                    conn.NODE2_ID = 0;
                                    ChangEntStatus(conn, 2, "Del");
                                }
                            }
                            DBManager.Instance.Update(conn);
                        }
                    }
                    tr.Commit();
                }
                PublicMethod.Instance.AlertDialog("打断结束!\n");
                PublicMethod.Instance.SendCommend("re\n");
            }
            catch (Autodesk.AutoCAD.Runtime.Exception ex) { ed.WriteMessageWithReturn(ex); }
            finally { DCadApi.isModifySymbol = false; }
        }
        #endregion

        #region 删除与fid连接的brefid的设备(打断列表中单个设备)
        /// <summary>
        /// 删除与fid连接的brefid的设备(打断列表中单个设备)
        /// </summary>
        /// <param name="fid"></param>
        /// <param name="brefid"></param>
        /// <param name="txnod">与fid连接的端</param>
        public static void BreakNodeByFid(long fid, long brefid, string txnod)
        {
            bool ismx = false;
            var conn =
                DBManager.Instance.GetEntities<Connectivity_n>(
                    o => o.G3E_FID == fid && o.EntityState != EntityState.Delete).FirstOrDefault();
            if (conn == null)
            {
                PublicMethod.Instance.AlertDialog(String.Format("Connectivity_n 没有 {0} 数据!", fid));
            }
            Connectivity_n node1;
            Connectivity_n node2 = null;
            if (txnod == "连接1")  // 获取与当前设备节点1相连的设备
            {
                node1 =
                    DBManager.Instance.GetEntities<Connectivity_n>(
                        o => o.NODE1_ID == conn.NODE1_ID && o.EntityState != EntityState.Delete && o.G3E_FID == brefid)
                        .FirstOrDefault();
                if(node1 == null)
                node2 =
                    DBManager.Instance.GetEntities<Connectivity_n>(
                        o => o.NODE2_ID == conn.NODE1_ID && o.EntityState != EntityState.Delete && o.G3E_FID == brefid)
                        .FirstOrDefault();
            }
            else
            {
                node1 =
                    DBManager.Instance.GetEntities<Connectivity_n>(
                        o => o.NODE1_ID == conn.NODE2_ID && o.EntityState != EntityState.Delete && o.G3E_FID == brefid)
                        .FirstOrDefault();
                if (node1 == null)
                node2 =
                    DBManager.Instance.GetEntities<Connectivity_n>(
                        o => o.NODE2_ID == conn.NODE2_ID && o.EntityState != EntityState.Delete && o.G3E_FID == brefid)
                        .FirstOrDefault();
            }
            long node1id = 0;
            long node2id = 0;

            #region 非母线

            if (node1 != null)
            {
                // 母线特殊处理
                if (node1.G3E_FNO != 143)
                {
                    if (node1.NODE1_ID != null) node1id = (long) node1.NODE1_ID;
                    node1.NODE1_ID = 0;
                    ChangEntStatus(node1, 1, "Del");
                    sinNodDevJudge(node1);
                    DBManager.Instance.Update(node1);
                }
                else
                {
                    ismx = true;
                }
            }
            else if (node2 != null)
            {
                if (node2.NODE2_ID != null) node2id = (long) node2.NODE2_ID;
                node2.NODE2_ID = 0;
                ChangEntStatus(node2, 2, "Del");
                sinNodDevJudge(node2);
                DBManager.Instance.Update(node2);
            }
            // 单个设备相连处理
            if (node1id != 0)
            {
                var sigconn =
                    DBManager.Instance.GetEntities<Connectivity_n>(
                        o => o.NODE1_ID == node1id && o.EntityState != EntityState.Delete);
                if (sigconn.Count() == 1)
                {
                    var temsig = sigconn.FirstOrDefault();
                    if (temsig != null)
                    {
                        temsig.NODE1_ID = 0;
                        ChangEntStatus(temsig, 1, "Del");
                        sinNodDevJudge(temsig);
                        DBManager.Instance.Update(temsig);
                    }
                }
                sigconn =
                    DBManager.Instance.GetEntities<Connectivity_n>(
                        o => o.NODE2_ID == node1id && o.EntityState != EntityState.Delete);
                if (sigconn.Count() == 1)
                {
                    var temsig = sigconn.FirstOrDefault();
                    if (temsig != null)
                    {
                        temsig.NODE2_ID = 0;
                        ChangEntStatus(temsig, 2, "Del");
                        sinNodDevJudge(temsig);
                        DBManager.Instance.Update(temsig);
                    }
                }
            }
            else if (node2id != 0)
            {
                var sigconn =
                    DBManager.Instance.GetEntities<Connectivity_n>(
                        o => o.NODE1_ID == node2id && o.EntityState != EntityState.Delete);
                if (sigconn.Count() == 1)
                {
                    var temsig = sigconn.FirstOrDefault();
                    if (temsig != null)
                    {
                        temsig.NODE1_ID = 0;
                        ChangEntStatus(temsig, 1, "Del");
                        sinNodDevJudge(temsig);
                        DBManager.Instance.Update(temsig);
                    }
                }
                sigconn =
                    DBManager.Instance.GetEntities<Connectivity_n>(
                        o => o.NODE2_ID == node2id && o.EntityState != EntityState.Delete);
                if (sigconn.Count() == 1)
                {
                    var temsig = sigconn.FirstOrDefault();
                    if (temsig != null)
                    {
                        temsig.NODE2_ID = 0;
                        ChangEntStatus(temsig, 2, "Del");
                        sinNodDevJudge(temsig);
                        DBManager.Instance.Update(temsig);
                    }
                }
            }

            #endregion

            #region 母线特殊处理

            if (ismx)
            {
                long nodeidd = 0;
                if (txnod == "连接1")
                {
                    if (conn != null)
                    {
                        if (conn.NODE1_ID != null) nodeidd = (long) conn.NODE1_ID;
                        conn.NODE1_ID = 0;
                        ChangEntStatus(conn, 1, "Del");
                        sinNodDevJudge(conn);
                        DBManager.Instance.Update(conn);
                    }
                }
                else
                {
                    if (conn != null)
                    {
                        if (conn.NODE2_ID != null) nodeidd = (long) conn.NODE2_ID;
                        conn.NODE2_ID = 0;
                        ChangEntStatus(conn, 2, "Del");
                        sinNodDevJudge(conn);
                        DBManager.Instance.Update(conn);
                    }
                }
                int count = 0;
                var d2m1 =
                    DBManager.Instance.GetEntities<Connectivity_n>(
                        o => o.NODE1_ID == nodeidd && o.G3E_FID != brefid && o.EntityState != EntityState.Delete);
                if (d2m1 != null) count += d2m1.Count();
                var d2m2 =
                    DBManager.Instance.GetEntities<Connectivity_n>(
                        o => o.NODE2_ID == nodeidd && o.G3E_FID != brefid && o.EntityState != EntityState.Delete);
                if (d2m2 != null) count += d2m2.Count();
                if (count == 0)
                {
                    var mxconn =
                        DBManager.Instance.GetEntities<Connectivity_n>(
                            o => o.G3E_FID == brefid && o.EntityState != EntityState.Delete).FirstOrDefault();
                    if (mxconn != null)
                    {
                        mxconn.NODE1_ID = 0;
                        ChangEntStatus(mxconn, 1, "Del");
                        sinNodDevJudge(mxconn);
                        DBManager.Instance.Update(mxconn);
                    }
                }
            }
        #endregion
        }



        #endregion

    }

    public class ContainMethods
    {
        // 建立 与 线 相交的杆 的包含关系
        public static bool AutoContainByPline(ObjectId plid)
        {
            var db = HostApplicationServices.WorkingDatabase;
            var ed = db.GetEditor();
            long lineid = 0, linefid = 0, linefno = 0;
            var b = DBEntityFinder.Instance.GetG3EIds(plid, ref lineid, ref linefid, ref linefno);
            if (b == false) return false;
            var pts = new Point3dCollection();
            // 取出多段线的点坐标
            using (var tr = db.TransactionManager.StartTransaction())
            {
                var ent = tr.GetObject(plid, OpenMode.ForRead) as Entity;
                var pl = ent as Polyline;
                if (pl != null && linefno == 141)
                {
                    for (var index = 0; index < pl.NumberOfVertices; index++)
                    {
                        pts.Add(pl.GetPoint3dAt(index));
                    }
                }
                tr.Commit();
            }
            // 根据 点坐标 求相交设备
            var ids = new ObjectIdList();
            if (pts.Count != 0)
            {
                using (Application.DocumentManager.MdiActiveDocument.LockDocument())
                {
                    // 得到当前视图
                    ViewTableRecord view = ed.GetCurrentView();
                    db.UpdateExt(true);
                    ed.ZoomExtents(); // 视图最大化
                    var ress = ed.SelectFence(pts);
                    ed.SetCurrentView(view); // 还原视图
                    if (ress.Status == PromptStatus.OK)
                    {
                        var tempids = ress.Value.GetObjectIds();
                        //ed.WriteMessageWithReturn(ress.Value.Count);
                        ids = new ObjectIdList(tempids);
                        ids.Remove(plid);
                    }
                }
            }
            // 只取出 杆设备 做操作
            if (ids.Count != 0)
            {
                DCadApi.isModifySymbol = true;
                using (Application.DocumentManager.MdiActiveDocument.LockDocument())
                {
                    using (var tr = db.TransactionManager.StartTransaction())
                    {
                        foreach (var id in ids)
                        {
                            var ent = tr.GetObject(id, OpenMode.ForRead) as Entity;
                            if (ent != null)
                            {
                                if (ent is DBText) continue;
                                long roleid = 0, rolefid = 0, rolefno = 0;
                                DBEntityFinder.Instance.GetG3EIds(id, ref roleid, ref rolefid, ref rolefno);
                                if (rolefno != 201) continue;
                                addRole(linefid, rolefid);
                            }
                        }
                        tr.Commit();
                    }
                }
            }
            return true;
        }
        /// <summary>
        /// 批量添加线
        /// </summary>
        /// <param name="fid">杆的Fid</param>
        /// <param name="selObjIdCol">线的Fid集合</param>
        public static void addLine(long fid, IEnumerable<ObjectId> selObjIdCol)
        {
            long selId = 0, selFid = 0, selFno = 0;
            foreach (var selObjId in selObjIdCol)
            {
                var b = DBEntityFinder.Instance.GetG3EIds(selObjId, ref selId, ref selFid, ref selFno);
                if (b)
                {
                    addLine(fid, selFid);
                }
            }
        }
        /// <summary>
        /// 添加线
        /// </summary>
        /// <param name="fid">杆的Fid</param>
        /// <param name="selFid">线的Fid</param>
        public static void addLine(long fid, long selFid)
        {
            // 判断杆在contain表中是否有数据
            //var role = DBManager.Instance.GetEntities<Contain_n>(o => o.G3E_FID == fid && o.EntityState != EntityState.Delete).FirstOrDefault();
            //if (null == role)
            //{
            //    LogManager.Instance.Error(String.Format("Contain_n 没有此杆的数据,不能做包含关系.杆G3e_Fid = {0}\n", fid));
            //    PublicMethod.Instance.AlertDialog(String.Format("Contain_n 没有此杆的数据,不能做包含关系.杆G3e_Fid = {0}\n", fid));
            //    return;
            //}
            // 工单判断
            if (false == DBEntityFinder.Instance.VerifyLTTID(selFid))
            {
                PublicMethod.Instance.AlertDialog(selFid + " 10kV 导线没有被工单锁定,不能包含.");
                return;
            }
            // 判断是否已经被此线重复包含
            var v = DBManager.Instance.GetEntity<Contain_n>(o => o.G3E_FID == selFid
                                                                 && o.EntityState != EntityState.Delete
                                                                 && o.EntityState != EntityState.InsertDelete
                                                                 && o.G3E_OWNERFID == fid);
            if (null != v)
            {
                PublicMethod.Instance.AlertDialog(selFid + " 10kV 导线已经被此杆包含.\n");
                return;
            }

            // 获取实体对象
            var _v = DBManager.Instance.GetEntities<Contain_n>(o => o.G3E_FID == selFid).Where(o => o.EntityState != EntityState.Delete);
            var nv = new Contain_n
            {
                G3E_FNO = 141,
                G3E_FID = selFid,
                G3E_CNO = 38,
                G3E_ID = CYZCommonFunc.getid(),
                G3E_OWNERFID = fid,
                G3E_OWNERFNO = 201,
                LTT_ID = MapConfig.Instance.LTTID,
                LTT_DATE = null,
                LTT_STATUS = null
            };
            if (_v.Any())
            {
                v = _v.OrderBy(o => o.G3E_CID).Last();

                if (0 == v.G3E_OWNERFID)
                {
                    v.G3E_OWNERFID = fid;
                    v.G3E_OWNERFNO = 201;
                    if (v.EntityState == EntityState.None)
                        v.EntityState = EntityState.Update;
                    DBManager.Instance.Update(v);
                }
                else
                {
                    nv.G3E_CID = v.G3E_CID + 1;
                    // 更新实体ownerid
                    nv.EntityState = EntityState.Insert;
                    DBManager.Instance.Insert(nv);
                }
            }
            else
            {
                nv.G3E_CID = 1;
                // 更新实体ownerid
                nv.EntityState = EntityState.Insert;
                DBManager.Instance.Insert(nv);
                if (DBManager.Instance.GetEntities<Contain_n>(o => o.G3E_FID == fid).All(o => o.EntityState == EntityState.Delete))
                {
                    var onv = new Contain_n
                    {
                        G3E_FNO = 201,
                        G3E_FID = fid,
                        G3E_CNO = 38,
                        G3E_CID = 1,
                        G3E_ID = CYZCommonFunc.getid(),
                        G3E_OWNERFID = 0,
                        G3E_OWNERFNO = 0,
                        LTT_ID = MapConfig.Instance.LTTID,
                        LTT_DATE = null,
                        LTT_STATUS = null,
                        EntityState = EntityState.Insert
                    };
                    DBManager.Instance.Insert(onv);
                }
            }
        }
        /// <summary>
        /// 批量增加杆
        /// </summary>
        /// <param name="fid">线的FID</param>
        /// <param name="selObjIdCol">杆的FID集合</param>
        public static void addRole(long fid, IEnumerable<ObjectId> selObjIdCol)
        {

            long selId = 0, selFid = 0, selFno = 0;
            foreach (var selObjId in selObjIdCol)
            {
                var b = DBEntityFinder.Instance.GetG3EIds(selObjId, ref selId, ref selFid, ref selFno);
                if (b)
                {
                    addRole(fid, selFid);
                }
            }
        }
        /// <summary>
        /// 增加杆
        /// </summary>
        /// <param name="fid">线的FID</param>
        /// <param name="selFid">杆的FID</param>
        public static void addRole(long fid, long selFid)
        {
            // 判断杆在contain表中是否有数据
            //var role =
            //    DBManager.Instance.GetEntities<Contain_n>(
            //        o => o.G3E_FID == selFid && o.EntityState != EntityState.Delete).FirstOrDefault();
            //if (null == role)
            //{
            //    PublicMethod.Instance.AlertDialog(String.Format("Contain_n 没有此杆的数据,不能做包含关系.杆G3e_Fid = {0}\n", selFid));
            //    return;
            //}
            // 获取实体对象(线与杆)
            var _s = DBManager.Instance.GetEntity<Contain_n>(
                o => o.G3E_FID == fid
                && o.G3E_OWNERFID == selFid
                && o.EntityState != EntityState.Delete);
            // 判断是否已经被此杆重复包含
            if (null != _s)
            {
                PublicMethod.Instance.AlertDialog("10kV 导线已经被此杆包含.\n");
                return;
            }
            var __s = DBManager.Instance.GetEntities<Contain_n>(o => o.G3E_FID == fid && o.EntityState != EntityState.Delete);
            var _nv = new Contain_n
            {
                G3E_FNO = 141,
                G3E_FID = fid,
                G3E_CNO = 38,
                G3E_ID = CYZCommonFunc.getid(),
                G3E_OWNERFID = selFid,
                G3E_OWNERFNO = 201,
                LTT_ID = MapConfig.Instance.LTTID,
                LTT_DATE = null,
                LTT_STATUS = null
            };

            if (__s.Any())
            {
                _s = __s.OrderBy(o => o.G3E_CID).Last();
                // 更改实体信息      
                if (0 == _s.G3E_OWNERFID)
                {
                    if (_s.EntityState == EntityState.None)
                        _s.EntityState = EntityState.Update;
                    _s.G3E_OWNERFID = selFid;
                    _s.G3E_OWNERFNO = 201;
                    DBManager.Instance.Update(_s);
                }
                else
                {
                    _nv.G3E_CID = _s.G3E_CID + 1;
                    // 更新实体ownerid
                    _nv.EntityState = EntityState.Insert;
                    DBManager.Instance.Insert(_nv);
                }
            }
            else
            {
                _nv.G3E_CID = 1;
                // 更新实体ownerid
                _nv.EntityState = EntityState.Insert;
                DBManager.Instance.Insert(_nv);
                //DBManager.Instance.Update(_nv);
            }
        }
    }

    public class OwnerMethods
    {
        /// <summary>
        /// 添加从属设备自动
        /// </summary>
        /// <param name="parentfid">面设备Fid</param>
        public static void addDev2AreaAuto(long parentfid)
        {
            try
            {
                var db = HostApplicationServices.WorkingDatabase;
                var ed = db.GetEditor();
                // 取出电房的点坐标
                var parentId = DBEntityFinder.Instance.GetObjectIdByFid(parentfid);
                var pts = new Point3dCollection();
                using (var tr = db.TransactionManager.StartTransaction())
                {
                    var pl = tr.GetObject(parentId, OpenMode.ForRead) as Polyline;
                    // 如果设备不是多段线,退出
                    if (pl == null) return;
                    // 如果线是闭合的
                    if (pl.Closed)
                    {
                        for (var index = 0; index < pl.NumberOfVertices; index++)
                        {
                            pts.Add(pl.GetPoint3dAt(index));
                        }
                    }
                    tr.Commit();
                }
                // 根据 点坐标 求范围内设备
                var ids = new ObjectIdList();
                if (pts.Count != 0)
                {
                    using (Application.DocumentManager.MdiActiveDocument.LockDocument())
                    {
                        // 得到当前视图
                        ViewTableRecord view = ed.GetCurrentView();
                        db.UpdateExt(true);
                        //ed.ZoomExtents(); // 视图最大化
                        ed.ZoomObject(parentId); // 视图定位到对象
                        var ress = ed.SelectCrossingPolygon(pts);
                        ed.SetCurrentView(view); // 还原视图
                        if (ress.Status == PromptStatus.OK)
                        {
                            var tempids = ress.Value.GetObjectIds();
                            ids = new ObjectIdList(tempids); // 存储房内设备的ObjectId
                            ids.Remove(parentId);// 去除自身
                        }
                    }
                }
                if (ids.Count != 0)
                {
                    DCadApi.isModifySymbol = true;
                    using (Application.DocumentManager.MdiActiveDocument.LockDocument())
                    {
                        using (var tr = db.TransactionManager.StartTransaction())
                        {
                            foreach (var id in ids)
                            {
                                var ent = tr.GetObject(id, OpenMode.ForRead) as Entity;
                                if (ent != null)
                                {
                                    if (ent is DBText) continue;
                                    G3EObject sonObj=null;
                                    DBEntityFinder.Instance.GetG3EIds(id, ref sonObj);
                                    if (sonObj == null) continue;
                                    DevBelong2Area(parentfid, sonObj.G3E_FID);
                                }
                            }
                            tr.Commit();
                        }
                    }
                }
            }
            catch (System.Exception e)
            {
                LogManager.Instance.Error(e.Message);
                PublicMethod.Instance.ShowMessage(e.Message);
            }
        }
        /// <summary>
        /// 子设备从属于父设备
        /// </summary>
        /// <param name="fatherDevFid">父设备Fid</param>
        /// <param name="sonDevFid">子设备Fid</param>
        public static void DevBelong2Area(long fatherDevFid, long sonDevFid)
        {
            // 获取子设备
            var scomm = DBManager.Instance.GetEntities<Common_n>(o => o.G3E_FID == sonDevFid).FirstOrDefault();
            if (null == scomm)
            {
                LogManager.Instance.Error("数据源Common_n中找不到fid= " + sonDevFid + " 设备!\n");
                PublicMethod.Instance.ShowMessage("数据源Common_n中找不到fid= " + sonDevFid + " 设备!\n");
                return;
            }
            // 获取父设备
            var fcomm = DBManager.Instance.GetEntities<Common_n>(o => o.G3E_FID == fatherDevFid).FirstOrDefault();
            if (null == fcomm)
            {
                LogManager.Instance.Error("数据源Common_n中找不到fid= " + fatherDevFid + " 设备!\n");
                PublicMethod.Instance.ShowMessage("数据源Common_n中找不到fid= " + fatherDevFid + " 设备!\n");
                return;
            }

            // 工单判断
            if (DBEntityFinder.Instance.VerifyLTTID(sonDevFid) == false)
            {
                LogManager.Instance.Error("该设备没有被工单锁定,不能从属于设备.");
                PublicMethod.Instance.ShowMessage("该设备没有被工单锁定,不能从属于设备.");
                return;
            }

            // 规范：判断设备是否可从属于设备fcomm
            var _bower = CDDBManager.Instance.GetEntities<G3e_ownership>(o => o.G3E_OWNERFNO == fcomm.G3E_FNO).Select(o => o.G3E_SOURCEFNO).Contains(scomm.G3E_FNO);
            if (false == _bower)
            {
                var str1 = FeatureMapping.instance.features[fcomm.G3E_FNO.ToString()];
                var str2 = FeatureMapping.instance.features[scomm.G3E_FNO.ToString()];
                PublicMethod.Instance.ShowMessage(str2 + " 不能从属于 " + str1);
                return;
            }
            var er = OwnerMethods.BelongByAZWZ(scomm, fcomm);
            if (!string.IsNullOrEmpty(er))
            {
                PublicMethod.Instance.ShowMessage(er);
                return;
            }
            // Common_n表赋值
            scomm.OWNER1_ID = fcomm.G3E_ID;
            scomm.OWNER2_ID = 0;
            // 更新数据源 
            if (scomm.EntityState == EntityState.None)
            {
                scomm.EntityState = EntityState.Update;
            }
            DBManager.Instance.Update(scomm);
            //如果设备是台架变压器, 特殊处理
            TJCommChange(scomm.G3E_FID, fcomm.G3E_ID);
        }
        /// <summary>
        /// 台架变压器 从属关系 处理
        /// </summary>
        /// <param name="GNWZ_FID">变压器G3E_FID</param>
        /// <param name="OWNER_ID">父设备G3E_ID</param>
        public static void TJCommChange(long GNWZ_FID, long OWNER_ID)
        {
            try
            {
                var tj_n = DBManager.Instance.GetEntities<Gg_gz_tj_n>(o => o.GNWZ_FID == GNWZ_FID.ToString()).FirstOrDefault();
                if (tj_n != null)
                {
                    var tj_m = DBManager.Instance.GetEntities<Common_n>(o => o.G3E_FID == tj_n.G3E_FID).FirstOrDefault();
                    if (tj_m != null)
                    {
                        tj_m.OWNER1_ID = OWNER_ID;
                        tj_m.OWNER2_ID = 0;
                        // 更新数据源 
                        if (tj_m.EntityState == EntityState.None)
                        {
                            tj_m.EntityState = EntityState.Update;
                        }
                        DBManager.Instance.Update(tj_m);
                    }
                }
            }
            catch (NotExistException ex)
            {
                PublicMethod.Instance.NotExistTable(ex);
            }
        }
        /// <summary>
        /// 根据安装位置判断是否做从属
        /// </summary>
        /// <param name="comm"></param>
        /// <param name="fcomm"></param>
        public static String BelongByAZWZ(Common_n comm, Common_n fcomm)
        {
            var str_err = "";
            switch (comm.G3E_FNO)
            {
                case 146://10kV开关
                {
                    var dlt = DBManager.Instance.GetEntity<Gg_pd_zfhwg_n>(comm.G3E_FID).DLT;
                    //1	户内	62452 //2	户外	46718 //3	柱上	46041 //4	箱式	27373 //5	房内	1217 //6	房外	29 
                    //201电杆//142公用电房//163专用电房//149箱式设备//198开关柜//199台架
                }
                    break;

                    #region 变压器 

                case 148:       
                {
                    var fl = DBManager.Instance.GetEntity<Gg_pd_gnwzmc_n>(comm.G3E_FID).GNWZ_FL2;
                    switch (fl)
                    {
                        case "":
                        {
                            str_err = "请选择安装位置!";
                        }
                            break;
                        case "台架":
                        {
                            if (fcomm.G3E_FNO != 201)
                                str_err = "台架只能从属于杆塔！";
                        }
                            break;
                        case "箱式":
                        {
                            if (fcomm.G3E_FNO != 149)
                                str_err = "当前安装位置只能从属于箱式设备！";
                        }
                            break;
                        case "房内":
                        {
                            if (fcomm.G3E_FNO == 149 || fcomm.G3E_FNO == 201)
                                str_err = "当前安装位置只能从属于电房！";
                        }
                            break;
                    }
                }

                    #endregion

                    break;
                case 177://避雷器
                {
                    var azwz = DBManager.Instance.GetEntity<Gg_pd_blq_n>(comm.G3E_FID).AZWZ;
                    //房外//柱上//户外//198开关柜//149箱式设备//201电杆//163专用电房//199台架//142公用电房
                }
                    break;
                case 81://低压柜
                    break;
                case 82://高压表
                    break;
                case 84://计量柜
                    break;
                case 90://DTU
                    break;
                case 180://FTU
                    break;
                case 307://电压互感器
                    break;
            }
            return str_err;
        }
        /// <summary>
        /// 清空设备从属关系
        /// </summary>
        /// <param name="fid">设备G3E_FID</param>
        public static void delDevOwner(long fid)
        {
            var comm = DBManager.Instance.GetEntities<Common_n>(o => o.G3E_FID == fid).FirstOrDefault();
            if (comm != null && comm.G3E_FNO != 198) //198开关柜不能在这删除
            {
                comm.OWNER1_ID = 0;  
                comm.OWNER2_ID = 0;
                // 更新数据源 
                if (comm.EntityState == EntityState.None)  
                    comm.EntityState = EntityState.Update;
                DBManager.Instance.Update(comm);
                //如果设备是台架变压器, 特殊处理
                TJCommChange(comm.G3E_FID, 0);
            }
        }
        /// <summary>
        /// 清空设备从属关系
        /// </summary>
        /// <param name="fids">设备G3E_FID集合</param>
        public static void delDevOwner(List<long> fids)
        {
            if (!fids.Any()) return;
            foreach (var fid in fids)
            {
                delDevOwner(fid);
            }
        }
    }
}
