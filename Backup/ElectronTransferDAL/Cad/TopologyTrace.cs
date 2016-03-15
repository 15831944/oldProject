using System;
using System.Collections.Generic;
using System.Linq;
using Autodesk.AutoCAD.ApplicationServices;
using Autodesk.AutoCAD.Colors;
using Autodesk.AutoCAD.DatabaseServices;
using Autodesk.AutoCAD.EditorInput;
using Autodesk.AutoCAD.Runtime;
using DotNetARX;
using ElectronTransferDal.Common;
using ElectronTransferDal.Query;
using ElectronTransferDal.Topology;
using ElectronTransferFramework;
using ElectronTransferModel;
using ElectronTransferModel.V9_4;
using Exception = System.Exception;

namespace ElectronTransferDal.Cad
{
    public class TopologyTrace
    {
        public static Dictionary<ObjectId, int> traceDit = new Dictionary<ObjectId, int>();
        // 启动追踪
        public static void startTrace(int dire)
        {
            var db = HostApplicationServices.WorkingDatabase;
            var ed = db.GetEditor();
            try
            {
                //选择要追踪的设备
                var pentr = ed.GetEntity("选择要追踪的设备:\n");
                if (pentr.Status != PromptStatus.OK) { return; }
                var entid = pentr.ObjectId;
                if (traceDit.ContainsKey(entid))  //判断对象是否已经追踪过
                {
                    ed.WriteMessageWithReturn("该设备已被追踪过,请先使用Cls清空历史追踪.");
                    { return; }
                }
                // 得到第一个追踪设备的信息
                long id = 0, fid = 0, fno = 0;
                if (!DBEntityFinder.Instance.GetG3EIds(entid, ref id, ref fid, ref fno)) { return; }
                var selconn = DBManager.Instance.GetEntity<Connectivity_n>(o => o.G3E_FID == fid);
                if (selconn == null) { return; }
                if (selconn.CD_SFDD == "不带电" || selconn.CD_DQZT == "断开") { return; }
                long nodeVal = 0;
                // 判断追踪方向
                if (dire == 2)
                {
                    if (selconn.NODE2_ID != null) { nodeVal = (long)selconn.NODE2_ID; }
                }
                else if (selconn.NODE1_ID != null) { nodeVal = (long)selconn.NODE1_ID; }

                DCadApi.isModifySymbol = true;
                using (Transaction tr = db.TransactionManager.StartTransaction())
                {
                    using (Application.DocumentManager.MdiActiveDocument.LockDocument())
                    {
                        // 改变第一个追踪对象的颜色，并加入词典
                        var ent = tr.GetObject(entid, OpenMode.ForRead) as Entity;
                        if (ent == null) return;
                        ent.UpgradeOpenAndRun();
                        traceDit.Add(entid, ent.ColorIndex);
                        ent.ColorIndex = 4;
                    }
                    tr.Commit();
                }
                //var entConn = DBManager.Instance.GetEntity<Connectivity_n>(o => o.G3E_FID == fid);
                // 递归追踪
                Trace(fid, nodeVal, dire);

                PublicMethod.Instance.AlertDialog("追踪完成,请及时清空颜色!\n");
            }
            catch (Exception ex)
            {
                ed.WriteMessageWithReturn(ex.Message);
            }
            finally
            {
                DCadApi.isModifySymbol = false;
            }
        }
        // 拓扑追踪递归
        public static void Trace(long fid, long nodeVal, int direct)
        {
            try
            {
                if (nodeVal == 0) return;
                var db = HostApplicationServices.WorkingDatabase;
                IEnumerable<Connectivity_n> t1;
                if (direct == 2)
                {
                    t1 = DBManager.Instance.GetEntities<Connectivity_n>(o => o.NODE1_ID == nodeVal
                        && o.G3E_FID != fid
                        && o.EntityState != EntityState.Delete);
                }
                else
                {
                    t1 = DBManager.Instance.GetEntities<Connectivity_n>(o => o.NODE2_ID == nodeVal
                        && o.G3E_FID != fid
                        && o.EntityState != EntityState.Delete);
                }
                if (t1.Any())
                {
                    foreach (var t in t1)
                    {
                        if (t.CD_SFDD == "带电" && t.CD_DQZT == "闭合")
                        {
                            ObjectId id = DBEntityFinder.Instance.GetObjectIdByFid(t.G3E_FID);
                            if (id == ObjectId.Null) return;
                            if (traceDit.ContainsKey(id)) return;
                            using (Transaction tr = db.TransactionManager.StartTransaction())
                            {
                                using (Application.DocumentManager.MdiActiveDocument.LockDocument())
                                {
                                    var ent = tr.GetObject(id, OpenMode.ForRead) as Entity;
                                    if (ent == null) continue;
                                    ent.UpgradeOpenAndRun();
                                    traceDit.Add(id, ent.ColorIndex);
                                    ent.ColorIndex = 4;
                                }
                                tr.Commit();
                            }
                            if (t.NODE2_ID != nodeVal)
                            {
                                if (t.NODE2_ID != null) Trace(t.G3E_FID, (long)t.NODE2_ID, direct);
                            }
                            else if (t.NODE1_ID != nodeVal)
                                if (t.NODE1_ID != null) Trace(t.G3E_FID, (long)t.NODE1_ID, direct);
                        }
                        else
                        {
                            PublicMethod.Instance.ShowMessage("设备Fid: " + t.G3E_FID.ToString() + " 断开 或 不带电,追踪结束.");
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                PublicMethod.Instance.Editor.WriteMessageWithReturn(ex.Message);
            }
        }
        // 取消追踪
        static public void CleanTrace()
        {
            try
            {
                var db = HostApplicationServices.WorkingDatabase;
                var doc = db.GetDocument();
                DCadApi.isModifySymbol = true;
                using (doc.LockDocument())
                {
                    using (var tr = db.TransactionManager.StartTransaction())
                    {
                        foreach (var dit in traceDit)
                        {
                            if (dit.Key.IsNull) continue;
                            var ent = tr.GetObject(dit.Key, OpenMode.ForRead) as Entity;
                            if (ent == null) continue;
                            ent.UpgradeOpenAndRun();
                            ent.ColorIndex = dit.Value;
                        }
                        traceDit.Clear();
                        tr.Commit();
                    }
                }
            }
            catch (Autodesk.AutoCAD.Runtime.Exception ex)
            {
                LogManager.Instance.Error(ex);
            }
            finally
            {
                DCadApi.isModifySymbol = false;
            }
        }

        static public void InitTopology(long mfid)
        {
            try
            {
                //如果mfid = 0,就是点对点追踪初始化
                if (TopologyMethods.bChangeTopo == false) return;
                //IEnumerable<Connectivity_n> conns;
                //string strSSKX = "";
                //if (mfid != 0)
                //{
                //    //fno = 159 特殊情况
                //    strSSKX = DBManager.Instance.GetEntity<Common_n>(mfid).CD_SSXL;
                //}
                //if (strSSKX != "")
                //{
                //    var tcomms =
                //        DBManager.Instance.GetEntities<Common_n>(
                //            o => o.CD_SSXL == strSSKX && o.EntityState != EntityState.Delete).Select(o => o.G3E_FID);
                //    var tconns = new List<Connectivity_n>();
                //    foreach (var tcomm in tcomms)
                //    {
                //        var tcomm1 = tcomm;
                //        var tconn = DBManager.Instance.GetEntities<Connectivity_n>(o => o.G3E_FID == tcomm1 && o.EntityState != EntityState.Delete).FirstOrDefault();
                //        if (tconn == null) continue;
                //        tconns.Add(tconn);
                //    }
                //    conns = tconns;
                //    tconns =
                //        DBManager.Instance.GetEntities<Connectivity_n>(
                //            o => (o.G3E_FNO == 159) && o.EntityState != EntityState.Delete).ToList();
                //    conns = conns.Concat(tconns);
                //    tconns =
                //        DBManager.Instance.GetEntities<Connectivity_n>(
                //            o => (o.G3E_FNO == 165) && o.EntityState != EntityState.Delete).ToList();
                //    conns = conns.Concat(tconns);

                //}
                //else
                //{
                //    conns = DBManager.Instance.GetEntities<Connectivity_n>(o => o.EntityState != EntityState.Delete);
                //}
               var     conns = DBManager.Instance.GetEntities<Connectivity_n>(o => o.EntityState != EntityState.Delete);
                var nos = new List<DLAdjNode>();
                if (conns != null)
                    foreach (var conn in conns)
                    {
                        if (conn == null) continue;
                        var no = new DLAdjNode();
                        no.fid = (int) conn.G3E_FID;
                        no.g3e_fno = conn.G3E_FNO;
                        if (conn.NODE1_ID != null) no.node1_id = (int)conn.NODE1_ID;
                        if (conn.NODE2_ID != null) no.node2_id = (int)conn.NODE2_ID;
                        no.cd_dqzt = conn.CD_DQZT;
                        no.cd_sfdd = conn.CD_SFDD;
                        nos.Add(no);
                    }
                TopologyMethod.CYZCreateGraphFromCSharp(nos.ToArray(), nos.Count());
                TopologyMethods.bChangeTopo = false;
            }
            catch (Exception e)
            {
                LogManager.Instance.Error(e);
                PublicMethod.Instance.AlertDialog(e.Message);
            }
        }

        static public void TraceDown()
        {
            try
            {
                var db = HostApplicationServices.WorkingDatabase;
                var ed = db.GetEditor();
                //选择要追踪的设备
                var pentr = ed.GetEntity("选择要追踪下游的设备:\n");
                if (pentr.Status == PromptStatus.OK)
                {
                    var entid = pentr.ObjectId;
                    long id = 0, fid = 0, fno = 0;
                    if (!DBEntityFinder.Instance.GetG3EIds(entid, ref id, ref fid, ref fno))
                    {
                        return;
                    }
                    TraceDown(fid);
                }
            }
            catch (Exception e)
            {
                PublicMethod.Instance.AlertDialog(e.Message);
                LogManager.Instance.Error(e);
            }
            finally
            {
                DCadApi.isModifySymbol = false;
            }
        }
        static public void TraceDown(long fid)
        {
            var db = HostApplicationServices.WorkingDatabase;
            var ed = db.GetEditor();
            InitTopology(fid);
            var downPath = new List<List<int>>();
            var b = TopologyMethod.CYZDownPath2((int)fid, ref downPath);
            if (b)
            {
                using (Transaction tr = db.TransactionManager.StartTransaction())
                {
                    using (Application.DocumentManager.MdiActiveDocument.LockDocument())
                    {
                        DCadApi.isModifySymbol = true;
                        foreach (var no in downPath)
                        {
                            foreach (var noo in no)
                            {
                                var entid = DBEntityFinder.Instance.GetObjectIdByFid(noo);
                                if (entid.IsNull) continue;
                                var ent = tr.GetObject(entid, OpenMode.ForRead) as Entity;
                                if (ent == null) return;
                                ent.UpgradeOpenAndRun();
                                if (!traceDit.Keys.Contains(entid)) traceDit.Add(entid, ent.ColorIndex);
                                ent.Color = Color.FromColor(PublicMethod.Instance.traceColor);
                            }
                        }
                    }
                    tr.Commit();
                }
            }
            b = TopologyMethod.CYZUpPath2((int)fid, ref downPath);
            if (b == false)
            {
                PublicMethod.Instance.AlertDialog("没有搜到电源点，请检查上游是否有设备断开连接。");
            }
        }

        static public void TraceUp()
        {
            try
            {
                var db = HostApplicationServices.WorkingDatabase;
                var ed = db.GetEditor();
                //选择要追踪的设备
                var pentr = ed.GetEntity("选择要追踪上游的设备:\n");
                if (pentr.Status == PromptStatus.OK)
                {
                    var entid = pentr.ObjectId;
                    long id = 0, fid = 0, fno = 0;
                    if (!DBEntityFinder.Instance.GetG3EIds(entid, ref id, ref fid, ref fno)) { return; }
                    TraceUp(fid);
                }
            }
            catch (Exception e)
            {
                PublicMethod.Instance.AlertDialog(e.Message);
                LogManager.Instance.Error(e);
            }
            finally
            {
                DCadApi.isModifySymbol = false;
            }
        }
        static public void TraceUp(long fid)
        {
            var db = HostApplicationServices.WorkingDatabase;
            InitTopology(fid);
            var retPathFids = new List<List<int>>();
            var retPathFid = new List<int>();
            var b = TopologyMethod.CYZUpPath2((int)fid, ref retPathFids);
            if (b)
            {
                retPathFid.AddRange(retPathFids[0]);
                if (retPathFids.Count > 1)
                {
                    for (var index = 0; index < retPathFids.Count; index++)
                    {
                        var bOpen = false;
                        var no = retPathFids[index];
                        foreach (var noo in no)
                        {
                            int noo1 = noo;
                            var conn =
                                DBManager.Instance.GetEntities<Connectivity_n>(
                                    o => o.G3E_FID == noo1 && o.EntityState != EntityState.Delete)
                                    .FirstOrDefault();
                            if (conn != null && conn.CD_DQZT == "断开")
                            {
                                bOpen = true;
                                break;
                            }
                        }
                        if (bOpen == false)
                        {
                            retPathFid.Clear();
                            retPathFid.AddRange(retPathFids[index]);
                            break;
                        }
                    }
                }
                using (Transaction tr = db.TransactionManager.StartTransaction())
                {
                    using (Application.DocumentManager.MdiActiveDocument.LockDocument())
                    {
                        DCadApi.isModifySymbol = true;
                        if (retPathFids.Any())
                            foreach (var no in retPathFid)
                            {
                                var entid = DBEntityFinder.Instance.GetObjectIdByFid(no);
                                if (entid.IsNull) continue;
                                var ent = tr.GetObject(entid, OpenMode.ForRead) as Entity;
                                if (ent == null) continue;
                                ent.UpgradeOpenAndRun();
                                if (!traceDit.Keys.Contains(entid)) traceDit.Add(entid, ent.ColorIndex);
                                ent.Color = Color.FromColor(PublicMethod.Instance.traceColor);
                            }
                    }
                    tr.Commit();
                }
            }
            else
            {
                PublicMethod.Instance.AlertDialog("请检查上游是否有电源点。\n");
            }
        }

        static public void TraceAll()
        {
            try
            {
                var db = HostApplicationServices.WorkingDatabase;
                var ed = db.GetEditor();
                //选择要追踪的设备
                var pentr = ed.GetEntity("选择要全追踪的设备:\n");
                if (pentr.Status == PromptStatus.OK)
                {
                    var entid = pentr.ObjectId;
                    long id = 0, fid = 0, fno = 0;
                    if (!DBEntityFinder.Instance.GetG3EIds(entid, ref id, ref fid, ref fno))
                    {
                        return;
                    }
                    TraceAll(fid);
                }
            }
            catch (Exception e)
            {
                PublicMethod.Instance.AlertDialog(e.Message);
                LogManager.Instance.Error(e);
            }
            finally
            {
                DCadApi.isModifySymbol = false;
            }
        }
        static public void TraceAll(long fid)
        {
            var db = HostApplicationServices.WorkingDatabase;
            InitTopology(fid);
            var allPath = new List<List<int>>();
            var b = TopologyMethod.CYZAllPath((int)fid, ref allPath);
            if (b)
            {
                using (Transaction tr = db.TransactionManager.StartTransaction())
                {
                    using (Application.DocumentManager.MdiActiveDocument.LockDocument())
                    {
                        DCadApi.isModifySymbol = true;
                        foreach (var no in allPath)
                        {
                            foreach (var noo in no)
                            {
                                var entid = DBEntityFinder.Instance.GetObjectIdByFid(noo);
                                if (entid.IsNull) continue;
                                var ent = tr.GetObject(entid, OpenMode.ForRead) as Entity;
                                if (ent == null) return;
                                ent.UpgradeOpenAndRun();
                                if (!traceDit.Keys.Contains(entid)) traceDit.Add(entid, ent.ColorIndex);
                                ent.Color = Color.FromColor(PublicMethod.Instance.traceColor);
                            }
                        }
                    }
                    tr.Commit();
                }
            }
        }

        static public void TraceP2P()
        {
            try
            {
                DCadApi.isModifySymbol = true;
                var db = HostApplicationServices.WorkingDatabase;
                var ed = db.GetEditor();
                //选择要追踪的设备
                PublicMethod.Instance.AlertDialog("选择要追踪第一点设备:\n");
                var pentr1 = ed.GetEntity("选择要追踪第一点设备:\n");
                if (pentr1.Status != PromptStatus.OK)
                {
                    PublicMethod.Instance.ShowMessage("选择错误.\n");
                    return;
                }

                //选择要追踪的设备
                PublicMethod.Instance.AlertDialog("选择要追踪第二点设备:\n");
                var pentr2 = ed.GetEntity("选择要追踪第二点设备:\n");
                if (pentr2.Status != PromptStatus.OK)
                {
                    PublicMethod.Instance.ShowMessage("选择错误.\n");
                    return;
                }

                var entid1 = pentr1.ObjectId;
                var entid2 = pentr2.ObjectId;
                long startid = 0, startfid = 0, startfno = 0;
                long endid = 0, endfid = 0, endfno = 0;
                if (!DBEntityFinder.Instance.GetG3EIds(entid1, ref startid, ref startfid, ref startfno)) { return; }
                if (!DBEntityFinder.Instance.GetG3EIds(entid2, ref endid, ref endfid, ref endfno)) { return; }

                TraceP2P(startfid, endfid);
            }
            catch (Exception e)
            {
                PublicMethod.Instance.AlertDialog(e.Message);
                LogManager.Instance.Error(e);
            }
            finally
            {
                DCadApi.isModifySymbol = false;
            }
        }
        static public void TraceP2P(long startfid, long endfid)
        {
            var db = HostApplicationServices.WorkingDatabase;
            InitTopology(0);
            var allPath = new List<int>();
            float dist = 0;
            var b = TopologyMethod.CYZP2pPath((int)startfid, (int)endfid, ref dist, ref allPath);
            if (b)
            {
                using (var tr = db.TransactionManager.StartTransaction())
                {
                    using (Application.DocumentManager.MdiActiveDocument.LockDocument())
                    {
                        foreach (var noo in allPath)
                        {
                            var entid1 = DBEntityFinder.Instance.GetObjectIdByFid(noo);
                            if (entid1.IsNull) continue;
                            var ent = tr.GetObject(entid1, OpenMode.ForRead) as Entity;
                            if (ent == null) return;
                            ent.UpgradeOpenAndRun();
                            if (!traceDit.Keys.Contains(entid1)) traceDit.Add(entid1, ent.ColorIndex);
                            ent.Color = Color.FromColor(PublicMethod.Instance.traceColor);
                        }
                    }
                    tr.Commit();
                }
            }
        }
    }
}
