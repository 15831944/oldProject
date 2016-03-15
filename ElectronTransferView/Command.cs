using System.Collections.Generic;
using System.ComponentModel;
using System.IO;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using Autodesk.AutoCAD.ApplicationServices;
using Autodesk.AutoCAD.Geometry;
using Autodesk.AutoCAD.Runtime;
using Autodesk.AutoCAD.DatabaseServices;
using Autodesk.AutoCAD.EditorInput;
using DotNetARX;
using ElectronTransferDal.Cad;
using ElectronTransferDal.Factory;
using ElectronTransferDal.FunctionHelper;
using ElectronTransferDal.HelperEventArgs;
using ElectronTransferDal.Query;
using ElectronTransferFramework;
using ArxMap;
using ElectronTransferModel;
using ElectronTransferView.ConnectivityManager;
using ElectronTransferView.ContextMenuManager;
using ElectronTransferView.DataManager;
using ElectronTransferView.FunctionManager;
using ElectronTransferView.SearchManager;
using ElectronTransferView.ViewManager;
using Application = Autodesk.AutoCAD.ApplicationServices.Application;
using Polyline = Autodesk.AutoCAD.DatabaseServices.Polyline;
using ElectronTransferDal.Common;
using ElectronTransferModel.V9_4;
using Exception = Autodesk.AutoCAD.Runtime.Exception;

namespace ElectronTransferView
{
    public class Command
    {
        /// <summary>
        /// 多段线插入顶点
        /// </summary>
        [CommandMethod("av")]
        public void AddPolylineVertex()
        {
            var db = HostApplicationServices.WorkingDatabase;
            var ed = db.GetEditor();
            var doc = db.GetDocument();
            try
            {
                var ent_res = ed.GetEntity("请选择一条多段线:\n");
                if (ent_res.Status != PromptStatus.OK) return;

                var pt_res = ed.GetPoint("请选择一个点:\n");
                if (pt_res.Status != PromptStatus.OK) return;

                var new_pt = pt_res.Value; // 新增的端点
                var pl_Index = 0; // 记录插入第几段
                var pts = new Point3dCollection(); //多段线的顶点集合
                var ptAtpl = Point3d.Origin; // 选择点与多段线的最近点
                var width = 0.0;
                using (doc.LockDocument())
                using (var tr = db.TransactionManager.StartTransaction())
                {
                    var ent = tr.GetObject(ent_res.ObjectId, OpenMode.ForWrite) as Entity;
                    var pl = ent as Polyline;
                    if (pl != null)
                    {
                        if (pl.Closed)
                        {
                            PublicMethod.Instance.ShowMessage("面设备不能增加顶点。");
                            return;
                        }
                        ptAtpl = pl.GetClosestPointTo(new_pt, false);
                        width = pl.GetStartWidthAt(0);
                        for (var index = 0; index < pl.NumberOfVertices; index++)
                        {
                            pts.Add(pl.GetPoint3dAt(index));
                        }
                    }
                    tr.Commit();
                }

                if (pts.Count >= 2)
                {
                    var isPlPt = pts.Contains(ptAtpl); //如果是多段线顶点
                    if (isPlPt)
                    {
                        pl_Index = pts.IndexOf(ptAtpl);
                        ed.WriteMessageWithReturn(string.Format("最近点多段线顶点重合 {0}", pts.IndexOf(ptAtpl)));
                    }
                    else
                    {
                        for (int i = 0; i < pts.Count - 1; i++)
                        {
                            var line = new Line(pts[i], pts[i + 1]);
                            var ptAtline = line.GetClosestPointTo(ptAtpl, false);
                            if (ptAtpl.IsEqualTo(ptAtline))
                            {
                                pl_Index = i;
                                ed.WriteMessageWithReturn(string.Format("最近点在第 {0} 段线上", i));
                            }
                            line.Dispose();
                        }
                    }

                    // 插入顶点
                    using (var tr = db.TransactionManager.StartTransaction())
                    {
                        var ent = tr.GetObject(ent_res.ObjectId, OpenMode.ForWrite) as Entity;
                        var pl = ent as Polyline;
                        if (pl != null)
                        {
                            pl.AddVertexAt(pl_Index + 1, new Point2d(new_pt.X, new_pt.Y), 0, width, width);
                            // 设置线宽度
                        }
                        tr.Commit();
                    }
                }
            }
            catch (System.Exception ex)
            {
                ed.WriteMessageWithReturn(ex);
            }
        }
        /// <summary>
        /// 多段线删除顶点
        /// </summary>
        [CommandMethod("rv")]
        public void RemovePolylineVertex()
        {
            var db = HostApplicationServices.WorkingDatabase;
            var ed = db.GetEditor();
            var doc = db.GetDocument();
            try
            {
                var ent_res = ed.GetEntity("请选择一条多段线:\n");
                if (ent_res.Status != PromptStatus.OK) return;

                var pt_res = ed.GetPoint("请选择一个点:\n");
                if (pt_res.Status != PromptStatus.OK) return;
                var new_pt = pt_res.Value; // 选取的点

                var pts = new Point3dCollection(); //多段线的顶点集合
                var dist_dict = new Dictionary<double, int>();
                var dists = new List<double>();
                // 取出多段线的点集合
                using (doc.LockDocument())
                using (var tr = db.TransactionManager.StartTransaction())
                {
                    var ent = tr.GetObject(ent_res.ObjectId, OpenMode.ForWrite) as Entity;
                    var pl = ent as Polyline;
                    if (pl != null)
                    {
                        if (pl.Closed)
                        {
                            PublicMethod.Instance.ShowMessage("面设备不能删除顶点。");
                            return;
                        }
                        for (var index = 0; index < pl.NumberOfVertices; index++)
                        {
                            var Vept = pl.GetPoint3dAt(index);
                            pts.Add(Vept);
                            dist_dict.Add(Vept.DistanceTo(new_pt), index);
                            dists.Add(Vept.DistanceTo(new_pt));
                        }

                        // 选择距离选择点最近的顶点索引
                        var min_dist = dists.Min();
                        var ve_index = dist_dict[min_dist];
                        // 删除顶点
                        if (pl.NumberOfVertices > 2)
                        {
                            pl.RemoveVertexAt(ve_index);
                        }
                    }
                    tr.Commit();
                }

            }
            catch (System.Exception ex)
            {
                ed.WriteMessageWithReturn(ex);
            }
        }

        [CommandMethod("zmc1")]
        public void zmcc()
        {
            if (!IsLoadDataSource())
                return;
            if (!PublicMethod.Instance.IsExecuteSystemCMD("zmc1"))
            {
                var g = new G3EObject();
                //id=630802963 fid=630802964 fno=148
                var ee = new selectEntityArgs {g3eObject = g};
                g.G3E_FID = 630802964;
                g.G3E_ID = 630802963;
                g.G3E_FNO = 148;
                var tt = new JLBMap(ee);
                Application.ShowModelessDialog(tt);
            }
        }

        [CommandMethod("fidcx")]
        public void Show()
        {
            if (!IsLoadDataSource())
                return;
            if (!PublicMethod.Instance.IsExecuteSystemCMD("fidcx"))
            {
                if (Menu.MenuControl.query == null)
                {
                    Menu.MenuControl.query = new Query();
                }
                else
                {
                    Menu.MenuControl.query.Close();
                    Menu.MenuControl.query = new Query();
                }
                Menu.MenuControl.query.setpanel(0);
                Application.ShowModelessDialog(Menu.MenuControl.query);
            }
        }

        [CommandMethod("sen")]
        public static void sen()
        {
            try
            {
                Document doc = Application.DocumentManager.MdiActiveDocument; //文档
                Editor ed = doc.Editor;
                PromptEntityResult per = ed.GetEntity("entitystatus:\n");
                if (per.Status == PromptStatus.OK)
                {
                    long g3eid = 0, g3efid = 0, g3efno = 0;
                    DBEntityFinder.Instance.GetG3EIds(per.ObjectId, ref g3eid, ref g3efid, ref g3efno);
                    var conn = DBManager.Instance.GetEntity<Connectivity_n>(g3efid);
                    if (conn != null)
                        ed.WriteMessageWithReturn(conn.EntityState + "  "
                                                  + conn.NODE1_ID + "  "
                                                  + conn.NODE2_ID + "\n");
                }
            }
            catch (System.Exception ex)
            {
                LogManager.Instance.Error(ex);
            }
        }

        [CommandMethod("cs")]
        public void CustonsSymbol()
        {
            var objIds = PublicMethod.Instance.SelectEntities("请选择实体");
            DCadApi.Clone(objIds, @"F:\aa.dwg");
        }

        [CommandMethod("lt")]
        public void PolyLineAdjust()
        {
            PublicMethod.Instance.EditPolyLine();
        }

        [CommandMethod("lz")]
        public void AdjustPolyLine()
        {
            PublicMethod.Instance.AdjustPolyLine();
        }
        [CommandMethod("cx")]
        public void GetG3EIDS()
        {
            ObjectIdCollection objectId = PublicMethod.Instance.SelectEntities("选择对象\n");
            foreach (ObjectId id in objectId)
            {
                long iid = 0, fid = 0, fno = 0;
                DBEntityFinder.Instance.GetG3EIds(id, ref iid, ref fid, ref fno);
                PublicMethod.Instance.ShowMessage(string.Format("\nid={0} fid={1} fno={2}\n", iid, fid, fno));
            }
        }

        [CommandMethod("aaa")]
        public void aa()
        {
            var fid = PublicMethod.Instance.GetDouble("Label数据查询，请输入FID");
            var value =
                DBSymbolFinder.Instance.Values.Where(
                    o => o.G3E_FID == long.Parse(fid.ToString()) && o.EntityType == EntityType.Label).ToList();
            if (value.Count > 0)
            {
                PublicMethod.Instance.ShowMessage(string.Format("{0}条Label数据存在", value.Count));
                foreach (var item in value)
                {
                    var str = new StringBuilder();
                    PublicMethod.Instance.ShowMessage(
                        string.Format("FinderFuncNumber:{0}\nOriginalG3e_Fid:{1}\nEntityType:{2}\nErased:{3}",
                            item.FinderFuncNumber, item.OriginalG3e_Fid, item.EntityType, item.IsErased));
                    foreach (var i in item.GetPropertyNames())
                    {
                        var proName = item.GetValue(i);
                        if (proName != null && !string.IsNullOrEmpty(proName.ToString()))
                        {
                            str.Append(string.Format("{0}:{1}\n", i, proName));
                        }
                    }
                    PublicMethod.Instance.AlertDialog(str.ToString());
                }
            }
            else
                PublicMethod.Instance.ShowMessage("不存在");
        }

        [CommandMethod("bb")]
        public void bb()
        {
            var fid = PublicMethod.Instance.GetDouble("Symbol数据查询，请输入FID");
            var value =
                DBSymbolFinder.Instance.Values.Where(
                    o => o.G3E_FID == long.Parse(fid.ToString()) && o.EntityType != EntityType.Label).ToList();
            if (value.Count > 0)
            {
                PublicMethod.Instance.ShowMessage(string.Format("{0}条Symbol数据存在", value.Count));
                foreach (var item in value)
                {
                    var str = new StringBuilder();
                    PublicMethod.Instance.ShowMessage(
                        string.Format(
                            "FinderFuncNumber:{0}\nOriginalG3e_Fid:{1}\nEntityType:{2}\nErased:{3}\nCoordinate:{4}",
                            item.FinderFuncNumber,
                            item.OriginalG3e_Fid, item.EntityType, item.IsErased, item.IsCoordinate));
                    foreach (var i in item.GetPropertyNames())
                    {
                        var proName = item.GetValue(i);
                        if (proName != null && !string.IsNullOrEmpty(proName.ToString()))
                        {
                            str.Append(string.Format("{0}:{1}\n", i, proName));
                        }
                    }
                    PublicMethod.Instance.AlertDialog(str.ToString());
                }
            }
            else
                PublicMethod.Instance.ShowMessage("不存在");
        }


        internal static List<ObjectId> _curves =
            new List<ObjectId>();


        [CommandMethod("lxlj")]
        public void Lxlj()
        {
            if (!IsLoadDataSource())
                return;
            if (!PublicMethod.Instance.IsExecuteSystemCMD("LXLJ"))
            {
                ViewHelper.AddConnectBySelPanel();
            }
        }
        [CommandMethod("mxjl")]
        public void Mxjl()
        {
            if (!IsLoadDataSource())
                return;
            if (!PublicMethod.Instance.IsExecuteSystemCMD("MXJL"))
            {
                var MxToYxDlg = new ConnectMxToYx();
                Application.ShowModelessDialog(MxToYxDlg);
            }
        }
        [CommandMethod("syzz")]
        public void Syzz()
        {
            if (!IsLoadDataSource())
                return;
            if (!PublicMethod.Instance.IsExecuteSystemCMD("syzz"))
            {
                ConnectManager.CleanSelcol();
                TopologyTrace.TraceUp();
            }
        }
        /// <summary>
        /// 下游追踪
        /// </summary>
        [CommandMethod("xyzz")]
        public void Xyzz()
        {
            if (!IsLoadDataSource())
                return;
            if (!PublicMethod.Instance.IsExecuteSystemCMD("xyzz"))
            {
                ConnectManager.CleanSelcol();
                TopologyTrace.TraceDown();
            }
        }
        /// <summary>
        /// 批量打断
        /// </summary>
        [CommandMethod("pldd")]
        public void Pldd()
        {
            if (!IsLoadDataSource())
                return;
            if (!PublicMethod.Instance.IsExecuteSystemCMD("pldd"))
            {
                TopologyMethods.breakall();
            }
        }

        /// <summary>
        /// 快速定位到工单锁定框
        /// </summary>
        [CommandMethod("zz")]
        public void Zoom()
        {
            PublicMethod.Instance.Zoom();
        }
        /// <summary>
        /// 移动
        /// </summary>
        [CommandMethod("sbyd")]
        public void Sbyd()
        {
            if (!IsLoadDataSource())
                return;
            if (!PublicMethod.Instance.IsExecuteSystemCMD("SBYD"))
            {
                if (mouse._selectedObjectIds != null && mouse._selectedObjectIds.Any())
                {
                    PublicMethod.Instance.SelectSymbolOrLabelEntity(mouse._selectedObjectIds);
                    PublicMethod.Instance.SendCommend("move\n");
                }
                else
                {
                    var objIds = PublicMethod.Instance.GetObjectIds("请选择要移动的设备");
                    if (objIds != null)
                    {
                        PublicMethod.Instance.SelectSymbolOrLabelEntity(mouse._selectedObjectIds);
                        PublicMethod.Instance.SendCommend("move\n");
                    }
                }
            }
        }
        /// <summary>
        /// 旋转
        /// </summary>
        [CommandMethod("sbxz")]
        public void Sbxz()
        {
            if (!IsLoadDataSource())
                return;
            if (!PublicMethod.Instance.IsExecuteSystemCMD("SBXZ"))
            {
                var objIds = PublicMethod.Instance.GetObjectIds("请选择要旋转的设备");
                if (objIds != null)
                {
                    PublicMethod.Instance.SelectSymbolOrLabelEntity(objIds);
                    PublicMethod.Instance.SendCommend("ROTATE\n");
                }
            }
        }
        /// <summary>
        /// 坐标定位
        /// </summary>
        [CommandMethod("zbdw")]
        public void Zbdw()
        {
            if (!IsLoadDataSource())
                return;
            if (!PublicMethod.Instance.IsExecuteSystemCMD("ZBDW"))
            {
                if (Menu.MenuControl.coordinateFix == null)
                {
                    Menu.MenuControl.coordinateFix = new CoordinateFix();
                }
                else
                {
                    Menu.MenuControl.coordinateFix.Close();
                    Menu.MenuControl.coordinateFix = new CoordinateFix();
                }
                Application.ShowModelessDialog(Menu.MenuControl.coordinateFix);
            }
        }
        /// <summary>
        /// 设备定位
        /// </summary>
        [CommandMethod("sbdw")]
        public void Sbdw()
        {
            if (!IsLoadDataSource())
                return;
            if (!PublicMethod.Instance.IsExecuteSystemCMD("SBDW"))
            {
                if (Menu.MenuControl.fp == null)
                {
                    Menu.MenuControl.fp = new FixedPosition();
                }
                else
                {
                    Menu.MenuControl.fp.Close();
                    Menu.MenuControl.fp = new FixedPosition();
                }
                Application.ShowModelessDialog(Menu.MenuControl.fp);
            }
        }
        /// <summary>
        /// 打断于点
        /// </summary>
        [CommandMethod("ddyd")]
        public void Ddyd()
        {
            if (!IsLoadDataSource())
                return;
            if (!PublicMethod.Instance.IsExecuteSystemCMD("ddyd"))
            {
                DIYCommand.Break();
            }
        }
        /// <summary>
        /// 两点打断
        /// </summary>
        [CommandMethod("lddd")]
        public void Lddd()
        {
            if (!IsLoadDataSource())
                return;
            if (!PublicMethod.Instance.IsExecuteSystemCMD("lddd"))
            {
                DIYCommand.BreakLine2();
            }
        }

        /// <summary>
        /// 复制
        /// </summary>
        [CommandMethod("COPYCLIP")]
        public void COPYCLIP()
        {
            if (!IsLoadDataSource())
                return;
            if (!PublicMethod.Instance.IsExecuteSystemCMD("COPYCLIP"))
            {
                DBEntityCopy.Instance.DragAndCopyEntities(mouse._selectedObjectIds);
            }
        }

        /// <summary>
        /// 提交数据
        /// </summary>
        [CommandMethod("QSAVE")]
        public void QSave()
        {
            Save();
        }

        public static void Save()
        {
            try
            {
                PublicMethod.Instance.ShowMessage("\n正在保存，请稍候...");
                PublicMethod.Instance.Submit();
                PublicMethod.Instance.ShowMessage("保存成功！");
            }
            catch(Exception ex)
            {
                LogManager.Instance.Error(ex);
                PublicMethod.Instance.ShowMessage("保存失败！");
            }
        }

        [CommandMethod("autoc")]
        public void AutoC()
        {
            if (!PublicMethod.Instance.IsExecuteSystemCMD("autoc"))
            {
                var a = new AutoConnect();
                a.autoc();
            }
        }
        [CommandMethod("cls")]
        public void Cls()
        {
            if (!IsLoadDataSource())
                return;
            if (!PublicMethod.Instance.IsExecuteSystemCMD("cls"))
            {
                FixEntity.Instance.ResetOldEntity();
                ConnectManager.CleanSelcol();
                TopologyTrace.CleanTrace();
            }
        }

        [CommandMethod("rec")]
        public void RecoverEntityColor()
        {
            PublicMethod.Instance.active = true;
            PublicMethod.Instance.wrapup = false;
            PublicMethod.Instance.echocmd = false;
            PublicMethod.Instance.SendCommend("re\n");
            if (!IsLoadDataSource())
                return;
            try
            {
                if (!PublicMethod.Instance.IsExecuteSystemCMD("rec"))
                {
                    //获取工单范围的实体
                    var entities = DBEntityFinder.Instance.GetDevInLttID();
                    foreach (var item in entities)
                    {
                        var psd = DCadApi.GetSymbolDataByObjId(item.Key);
                        if (psd != null)
                        {
                            var rdt = DevEventHandler.GetDevTables(psd.g3eObject.G3E_FNO, psd.g3eObject.G3E_FID);
                            var ise = new InsertSymbolEventArgs
                                          {
                                              symbolObj = rdt.DevObj,
                                              g3e_fno = psd.g3eObject.G3E_FNO
                                          };
                            ExecutionManager.Instance.GetFactory(typeof (InsertSymbolExecutionFactory))
                                .GetExecution(psd.g3eObject.G3E_FNO)
                                .Execute(null, ise);
                            if (psd.color != ise.symbolColor)
                            {
                                DCadApi.EditorPointSymbol(psd.objectId, ise.symbolColor);
                            }
                        }
                    }
                }
            }catch(Exception ex)
            {
                LogManager.Instance.Error(ex);
            }
        }
        /// <summary>
        /// 是否加载数据源
        /// </summary>
        /// <returns></returns>
        public static bool IsLoadDataSource()
        {
            if (!CadDataSource._isLoadDataSource)
            {
                MessageBox.Show("请先加载数据源！！！", "提示", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
            else
            {
                return true;
            }
            return false;
        }


        private static string messTip;
        private static WaitingSplash waitingSplash;
        private static BackgroundWorker bw;
        private delegate void ProgressBarDelegate();
        /// <summary>
        /// 提示框
        /// </summary>
        /// <param name="mess">提示语</param>
        public static void GetBackgroundWorker(string mess)
        {
            messTip = mess;
            bw = new BackgroundWorker();
            bw.DoWork += bw_DoWork;
            bw.RunWorkerAsync();
        }

        private static void bw_DoWork(object sender, DoWorkEventArgs e)
        {
            waitingSplash = new WaitingSplash(messTip);
            waitingSplash.SetDesktopLocation(0, 0);
            waitingSplash.ShowDialog();
        }



        /// <summary>
        /// 用委托去关闭跨线程窗体
        /// </summary>
        public static void CloseProgressBar()
        {
            if (waitingSplash == null) return;
            if (waitingSplash.InvokeRequired)
            {
                var pbd = new ProgressBarDelegate(CloseProgressBar);
                waitingSplash.Invoke(pbd);
            }
            else
            {
                waitingSplash.Close();
                bw.DoWork -= bw_DoWork;
            }
        }


        [CommandMethod("ulcui")]
        public void LoadMyCui()
        {
            var path = ElectronTransferBll.Register.GetItemValue("CustomAutoCAD", "LOADER", false);
            var folderPath = path.Substring(0, path.LastIndexOf('\\'));
            var cuiPath = Path.Combine(folderPath, "zhsc.cuix");

            var doc = Application.DocumentManager.MdiActiveDocument;
            var oldCmdEcho = Application.GetSystemVariable("CMDECHO");
            var oldFileDia = Application.GetSystemVariable("FILEDIA");
            Application.SetSystemVariable("CMDECHO", 0);
            Application.SetSystemVariable("FILEDIA", 0);

            var mainCuiFile = (string)Application.GetSystemVariable("MENUNAME") + ".cuix";
            var str = Path.GetFileNameWithoutExtension(mainCuiFile);

            if (str.ToLower() == "acad")
            {
                doc.SendStringToExecute(
                    "_.cuiunload "
                    + "acad"
                    + " ",
                    false, false, false);

                doc.SendStringToExecute(
                  "_.cuiload "
                  + cuiPath + " ",
                  false, false, false);
            }


            doc.SendStringToExecute(
          "(setvar \"FILEDIA\" "
              + oldFileDia
              + ")(princ) ",
              false, false, false
            );

            doc.SendStringToExecute(
          "(setvar \"CMDECHO\" "
          + oldCmdEcho
              + ")(princ) ",
              false, false, false
        );
        }

        /*设备重绘*/
        [CommandMethod("ch")]
        public void Redraw()
        {
            var doc = Application.DocumentManager.MdiActiveDocument;
            var ed = doc.Editor;
            var per = ed.GetEntity("选择要重绘设备:\n");
            if (per.Status != PromptStatus.OK) return;
            RedrawSymbol.RedrawPolyLine(per.ObjectId);
        }


        [CommandMethod("shre")]
        public void autoca()
        {
            var doc = Application.DocumentManager.MdiActiveDocument;
            var db = HostApplicationServices.WorkingDatabase;
            var ed = doc.Editor;
            try
            {
                var per1 = ed.GetEntity("选择测试设备:\n");
                if (per1.Status != PromptStatus.OK) return;
                using (var tr = doc.TransactionManager.StartTransaction())
                {
                    var bt = (BlockTable)(tr.GetObject(db.BlockTableId, OpenMode.ForRead));
                    var btr = (BlockTableRecord)tr.GetObject(bt[BlockTableRecord.ModelSpace], OpenMode.ForWrite);
                    var ent1 = tr.GetObject(per1.ObjectId, OpenMode.ForRead) as Entity;
                    if (ent1 == null) return;
                    if (ent1 is Polyline)
                    {
                        var pp = ent1 as Polyline;
                        ed.WriteMessage(pp.Closed);
                        ed.WriteMessage("\n");
                    }
                    else if (ent1 is BlockReference)
                    {
                        var rr  = ent1.Clone() as BlockReference;
                        if (rr != null)
                        {
                            // 记录当前角度
                            var rolat = rr.Rotation;
                            // 把角度设置为0
                            rr.Rotation = 0;
                            // 得到角度为0的实体范围
                            var ext = rr.GeometricExtents;

                            // 生成一个测试矩形,颜色为黄色
                            var pl = new Polyline();
                            pl.CreateRectangle(new Point2d(ext.MaxPoint.X, ext.MaxPoint.Y), new Point2d(ext.MinPoint.X, ext.MinPoint.Y));
                            pl.ColorIndex = 2;

                            // 旋转实体范围
                            Matrix3d mt = Matrix3d.Rotation(rolat, Vector3d.ZAxis, GeTools.MidPoint(ext.MaxPoint, ext.MinPoint));
                            pl.TransformBy(mt);

                            // 加入到模型空间
                            btr.AppendEntity(pl);
                            tr.AddNewlyCreatedDBObject(pl, true);
                        }
                    }
                    tr.Commit();
                }
            }
            catch (Exception ex) { ed.WriteMessageWithReturn(ex); }
        }
        [CommandMethod("cxb")]
        public void ggg()
        {
            var fid=PublicMethod.Instance.GetDouble("请输入FID：");
            var G3eFid = (long)fid;
            var com = DBManager.Instance.GetEntity<Common_n>(G3eFid);
            if (com != null)
                PublicMethod.Instance.ShowMessage("存在公共表");
            var conn = DBManager.Instance.GetEntity<Connectivity_n>(G3eFid);
            if (conn != null)
                PublicMethod.Instance.ShowMessage("连接关系表存在");
            var gnwz = DBManager.Instance.GetEntity<Gg_pd_gnwzmc_n>(G3eFid);
            if (gnwz != null)
                PublicMethod.Instance.ShowMessage("功能位置表存在");
            if (com != null || conn != null || gnwz != null)
            {

            }
            else
                PublicMethod.Instance.ShowMessage("自身表不存在");
        }
        [CommandMethod("yd")]
        public void MoveSymbol() {
            if (!IsLoadDataSource())
                return;
            if (!PublicMethod.Instance.IsExecuteSystemCMD("yd"))
            {
                var ms = new MoveSymbol();
                Application.ShowModelessDialog(ms);
            }
        }

        public void VerifyData()
        {
            
        }
    }
}