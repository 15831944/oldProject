using System;
using System.Drawing;
using System.Linq;
using Autodesk.AutoCAD.ApplicationServices;
using Autodesk.AutoCAD.DatabaseServices;
using Autodesk.AutoCAD.EditorInput;
using Autodesk.AutoCAD.Geometry;
using Autodesk.AutoCAD.Runtime;
using DotNetARX;
using ElectronTransferDal.Cad;
using ElectronTransferDal.FunctionHelper;
using ElectronTransferDal.Query;
using acadApp = Autodesk.AutoCAD.ApplicationServices.Application;
using ElectronTransferModel;
using ElectronTransferModel.Config;


namespace ArxMap
{
    public class mouse
    {
        const int WM_MOUSEWHEEL = 522;
        const int WM_MOUSEMOVE = 512;
        const int WM_LBUTTONDOWN = 513;
        const int WM_LBUTTONUP = 514;
        //const int WM_LBUTTONDBLCLK = 515;  
        //const int WM_MBUTTONDOWN = 519;
        const int WM_MBUTTONUP = 520;
        const int WM_SYSKEYUP = 261;
        const int WM_SYSKEYDOWN = 260;

        /// <summary>
        /// cad版本号
        /// </summary>
        public static string acadVer;
        private static HightLightRule hightLightRule;//定义规则重定义类  

        public static Vector3d wMin;
        public static Vector3d wMax;
        static double winw, winh;

        static bool bpan;
        public static ObjectId selectedEntityId = ObjectId.Null;

        /// <summary>
        /// 选中的实体数组
        /// </summary>
        public static ObjectId[] _selectedObjectIds;

        public static ObjectId[] ImpliedSelectionIds;

        private static int loadindex;
        /// <summary>
        /// 检测是否是镜像
        /// </summary>
        public static bool isMirror { get; set; }

        public static string UrlStr;

        public static void showmap()
        {
            if (DCadApi.isInsertSymbol) return;
            //根据比例尺决定是否隐藏或显示标注
            Acad.CADFunction.getWorldExtents(ref wMin, ref wMax, ref winw, ref winh);
            if (!DCadApi.isShowLayerManage)
                PublicMethod.Instance.SetLayerDisplay(wMax.X - wMin.X);

            if (MapConfig.Instance.BrowsableMap)
            {
                //如果背景地图不在使用中，不加载地图
                var bjisoff = PublicMethod.Instance.GetLayerOff("背景地图");
                //判断背景图是否被隐藏
                if (!bjisoff)
                {
                    MapFrame.CYZMapFrame.cMapFramePtr.initMap(wMin.X, wMax.X, wMin.Y, wMax.Y, (int)winw, (int)winh);
                }
            }
            acadApp.UpdateScreen();
        }

        public static void loadMap()
        {
            MapFrame.CYZMapFrame.CreateMapFrame();
            showmap();
        }
        /// <summary>
        /// 加载CAD事件
        /// </summary>
        public static void LoadCADEvent()
        {
            acadVer = Register.GetAutoCADVersion();
            MouseEventOff();
            MouseEventAdd();
            //把按钮添加到AutoCAD的状态栏区域;
            if (loadindex == 0)
            {
                openGripRule();
                Overrule.Overruling = true;//开启规则重定义    
            }
            HostApplicationServices.WorkingDatabase.GetEditor().Dragging += ed_Dragging;
            HostApplicationServices.WorkingDatabase.GetEditor().DraggingEnded += ed_DraggingEnded;
            loadindex++;
        }

       
        private static void CommStart(object sender, CommandEventArgs e)
        {
            try
            {
                switch (e.GlobalCommandName)
                {
                    case "PAN":
                        bpan = true;
                        break;
                    case "ERASE":
                        ObjectOperate.EraseVerity(_selectedObjectIds);
                        break;
                    case "PEDIT":
                        ObjectOperate.UpdateVerity(selectedEntityId);
                        break;
                    case "GRIP_STRETCH":
                        ObjectOperate.GripStretchVerity(selectedEntityId);
                        break;
                    case "ROTATE":
                        ObjectOperate.UpdateVerity(true, _selectedObjectIds);
                        break;
                    case "MOVE":
                        ObjectOperate.UpdateVerity(true, _selectedObjectIds);
                        break;
                    case "DROPGEOM":
                        ObjectOperate.UpdateVerity(true, _selectedObjectIds);
                        break;
                    case "UNDO":
                        DCadApi.isEraseRollback = false;
                        break;
                    case "U":
                        DCadApi.isEraseRollback = false;
                        break;
                    case "MIRROR":
                        isMirror = true;
                        DCadApi.isEraseRollback = false;
                        break;
                    case "QUIT":
                        PublicMethod.Instance.SaveScreenXY(wMax, wMin);
                        break;
                    case "COPY":
                        DBEntityCopy.Instance.isDBEntityCopy = true;
                        break;
                    case "COPYCLIP":
                        DBEntityCopy.Instance.isDBEntityCopy = true;
                        break;
                }
            }
            catch (Autodesk.AutoCAD.Runtime.Exception ex)
            {
                PublicMethod.Instance.ShowMessage(string.Format("CommandEnded事件处理错误!\n！{0}\n", ex.Message));
            }
            finally
            {
                DCadApi.isInsertSymbol = true;
            }
        }

       
        private static void CommEnd(object sender, CommandEventArgs e)
        {
            try
            {
                switch (e.GlobalCommandName)
                {
                    case "ZOOM":
                        showmap();
                        break;
                    case "PAN":
                        bpan = false;
                        break;
                    case "ERASE":
                        DCadApi.isModifySymbol = false; 
                        ImpliedSelectionIds = null;
                        PublicMethod.Instance.RegenerateModel();
                        break;
                    case "GRIP_STRETCH":
                        PublicMethod.Instance.RegenerateModel();
                        break;
                    case "ROTATE":
                        PublicMethod.Instance.RegenerateModel();
                        break;
                    case "MOVE":
                        PublicMethod.Instance.RegenerateModel();
                        break;
                    case "DROPGEOM":
                        PublicMethod.Instance.RegenerateModel();
                        break;
                    case "COPY":
                        PublicMethod.Instance.RegenerateModel();
                        break;
                }
            }
            catch (Autodesk.AutoCAD.Runtime.Exception ex)
            {
                PublicMethod.Instance.ShowMessage(string.Format("CommandEnded事件处理错误!\n！{0}\n", ex.Message));
            }
            finally
            {
                DCadApi.isInsertSymbol = false;
            }
        }


        public static void MouseEventAdd()
        {
            // 添加事件
            acadApp.PreTranslateMessage += Application_PreTranslateMessage;
            acadApp.DocumentManager.MdiActiveDocument.CommandEnded += CommEnd;
            acadApp.DocumentManager.MdiActiveDocument.CommandWillStart += CommStart;
            acadApp.DocumentManager.MdiActiveDocument.CommandCancelled += MdiActiveDocument_CommandCancelled;
            acadApp.DocumentManager.MdiActiveDocument.Editor.SelectionAdded += OnSelectionAdded;
            acadApp.DocumentManager.MdiActiveDocument.Editor.PointMonitor += MouseTip.ed_PointMonitor;
            acadApp.DocumentManager.MdiActiveDocument.ImpliedSelectionChanged += MdiActiveDocument_ImpliedSelectionChanged;
            //注册文档切换事件
            //acadApp.DocumentManager.DocumentActivated += DocumentManager_DocumentActivated;
            var db=HostApplicationServices.WorkingDatabase;
            db.ObjectErased += db_ObjectErased;
            db.ObjectModified += db_ObjectModified;
            db.BeginDeepCloneTranslation += db_BeginDeepCloneTranslation;
        }

        static void MdiActiveDocument_CommandCancelled(object sender, CommandEventArgs e)
        {
            try
            {
                switch (e.GlobalCommandName)
                {
                    case "COPY":
                        DBEntityCopy.Instance.isDBEntityCopy = false;
                        break;
                    case "COPYCLIP":
                        DBEntityCopy.Instance.isDBEntityCopy = false;
                        break;
                }
            }
            catch (Autodesk.AutoCAD.Runtime.Exception ex)
            {
                PublicMethod.Instance.ShowMessage(string.Format("CommandCancelled事件处理错误!\n！{0}\n", ex.Message));
            }
            finally
            {
                DCadApi.isInsertSymbol = false;
            }
        }

        //static void DocumentManager_DocumentActivated(object sender, DocumentCollectionEventArgs e)
        //{
        //    ObjectOperate.IsLoadCADPlugin(e.Document.Name);
        //}

        public static void MouseEventOff()
        {
            // 鼠标事件
            acadApp.PreTranslateMessage -= Application_PreTranslateMessage;
            // 命令开始结束
            acadApp.DocumentManager.MdiActiveDocument.CommandEnded -= CommEnd;
            acadApp.DocumentManager.MdiActiveDocument.CommandWillStart -= CommStart;
            acadApp.DocumentManager.MdiActiveDocument.CommandCancelled -= MdiActiveDocument_CommandCancelled;
            // 当选择触发对话框事件
            acadApp.DocumentManager.MdiActiveDocument.Editor.SelectionAdded -= OnSelectionAdded;
            acadApp.DocumentManager.MdiActiveDocument.ImpliedSelectionChanged -= MdiActiveDocument_ImpliedSelectionChanged;
            acadApp.DocumentManager.MdiActiveDocument.Editor.PointMonitor -= MouseTip.ed_PointMonitor;
            //acadApp.DocumentManager.DocumentActivated -= DocumentManager_DocumentActivated;

            var db = HostApplicationServices.WorkingDatabase;
            db.ObjectErased -= db_ObjectErased;
            db.ObjectModified -= db_ObjectModified;
            db.BeginDeepCloneTranslation -= db_BeginDeepCloneTranslation;
        }
        /// <summary>
        /// 对象修改
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private static void db_ObjectModified(object sender, ObjectEventArgs e)
        {
            ObjectOperate.db_ObjectModified(e);
        }
        /// <summary>
        /// 对象删除
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private static void db_ObjectErased(object sender, ObjectErasedEventArgs e)
        {
            ObjectOperate.db_ObjectErased(e);
        }
        /// <summary>
        /// 对象克隆
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private static void db_BeginDeepCloneTranslation(object sender, IdMappingEventArgs e)
        {
            ObjectOperate.ObjectClone(e.IdMapping, _selectedObjectIds);
        }
        /// <summary>
        /// 对象选中
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private static void MdiActiveDocument_ImpliedSelectionChanged(object sender, EventArgs e)
        {
            try
            {
                var pkf = PublicMethod.Instance.Editor.SelectImplied();
                if (pkf.Status == PromptStatus.OK)
                {
                    ObjectOperate.isRollback = false;
                    ImpliedSelectionIds = pkf.Value.GetObjectIds();

                    selectedEntityId = ImpliedSelectionIds[ImpliedSelectionIds.Length - 1];
                    ObjectContextMenu.ShowAttribute(selectedEntityId);
                    ObjectContextMenu.AddOrRefreshConnectCtr(false);
                }
            }
            catch
            {
            }
        }

        /// <summary>
        /// 对象选择
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        public static void OnSelectionAdded(object sender, SelectionAddedEventArgs e)
        {
            try
            {
                _selectedObjectIds = e.AddedObjects.GetObjectIds(); //集合的ID 
            }
            catch (System.Exception ex)
            {
                PublicMethod.Instance.Editor.WriteMessageWithReturn(ex);
            }
        }

        public static Point3d point{set;get;}
        public static event EventHandler LeftButtonDownEventHander;
        public static void Application_PreTranslateMessage(object sender, PreTranslateMessageEventArgs e)
        {
            try
            {
                if (acadApp.DocumentManager.MdiActiveDocument != null)
                {
                    var ed = acadApp.DocumentManager.MdiActiveDocument.Editor;
                    switch (e.Message.message)
                    {
                        case WM_SYSKEYDOWN:
                            if (e.Message.wParam.ToInt32() == 65)
                            {
                                ObjectContextMenu.AddOrRefreshConnectCtr(true);
                            }
                            else if (e.Message.wParam.ToInt32() == 88)
                            {
                                ObjectContextMenu.ShowBulkChange();
                            }
                            else if (e.Message.wParam.ToInt32() == 81)
                            {
                                PublicMethod.Instance.SendCommend("pan\n");
                            }
                            break;
                        case WM_MOUSEWHEEL:
                            //System.Windows.Forms.MessageBox.Show("你在CAD操作中转动滚轮了！");
                            showmap();
                            break;
                        case WM_MOUSEMOVE:
                            // System.Windows.Forms.MessageBox.Show("你在CAD操作中移动鼠标了！");
                            break;
                        case WM_LBUTTONDOWN:
                            //System.Windows.Forms.MessageBox.Show("你在CAD操作中按下左键了！");
                            point=ed.PointToWorld(new Point(e.Message.pt_x, e.Message.pt_y));
                            var leftbuttondown = new FixEntityArgs{ Position=point};
                            if (LeftButtonDownEventHander != null)
                                LeftButtonDownEventHander(new object(), leftbuttondown);
                            break;
                        case WM_LBUTTONUP:
                            //System.Windows.Forms.MessageBox.Show("你在CAD操作中弹起左键了！");
                            if (bpan)
                            {
                                showmap();
                            }
                            break;
                        case WM_MBUTTONUP:
                            //System.Windows.Forms.MessageBox.Show("你在CAD操作中弹起中键了！");
                            showmap();
                            break;
                    }
                }
            }
            catch (System.Exception ex)
            {
                //LogManager.Instance.Error(ex);
            }
        }

        private static void ed_Dragging(object sender, DraggingEventArgs e)
        {
            AddOverrule();
        }
        public static void AddOverrule()
        {
            Overrule.AddOverrule(RXObject.GetClass(typeof(BlockReference)), UserDrawOverrule.TheOverrule, false);
            Overrule.AddOverrule(RXObject.GetClass(typeof(BlockReference)), DCadDrawEntityOverrule.TheOverrule, false);
            Overrule.AddOverrule(RXObject.GetClass(typeof(DBText)), DCadDrawEntityOverrule.TheOverrule, true);
            Overrule.AddOverrule(RXObject.GetClass(typeof(MText)), DCadDrawEntityOverrule.TheOverrule, true);
        }
        private static void ed_DraggingEnded(object sender, DraggingEndedEventArgs e)
        {
            RemoveOverrule();
        }
        public static void RemoveOverrule()
        {
            Overrule.RemoveOverrule(RXObject.GetClass(typeof(BlockReference)), UserDrawOverrule.TheOverrule);
            Overrule.RemoveOverrule(RXObject.GetClass(typeof(BlockReference)), DCadDrawEntityOverrule.TheOverrule);
            Overrule.RemoveOverrule(RXObject.GetClass(typeof(DBText)), DCadDrawEntityOverrule.TheOverrule);
            Overrule.RemoveOverrule(RXObject.GetClass(typeof(MText)), DCadDrawEntityOverrule.TheOverrule);
        }
       
  
        // 开启高亮规则重定义
        public static void openHightLightRule()
        {
            try
            {
                if (hightLightRule == null)
                {
                    hightLightRule = new HightLightRule();
                    //为实体添加亮显重定义
                    Overrule.AddOverrule(RXObject.GetClass(typeof(Entity)), hightLightRule, false);
                    Overrule.Overruling = true;//开启规则重定义
                }
                PublicMethod.Instance.RegenerateModel();
            }
            catch (System.Exception ex)
            {
                PublicMethod.Instance.Editor.WriteMessageWithReturn(ex);
            }
        }
        // 关闭高亮规则重定义
        public static void closeHightLightRule()
        {
            try
            {
                if (hightLightRule != null)
                {
                    Overrule.RemoveOverrule(RXObject.GetClass(typeof(Entity)), hightLightRule);
                    hightLightRule = null;
                    //Overrule.Overruling = false;
                }
                //刷新屏幕，直线被更新为门
                //PublicMethod.Instance.Editor.Regen();
            }
            catch (System.Exception ex)
            {
                PublicMethod.Instance.Editor.WriteMessageWithReturn(ex);
            }
        }
        //开启夹点规则重定义
        public static void openGripRule()
        {
            if (UserGripOverrule.TheOverrule != null)
            {
                Overrule.AddOverrule(RXObject.GetClass(typeof(BlockReference)), UserGripOverrule.TheOverrule, false);
                Overrule.AddOverrule(RXObject.GetClass(typeof(DBText)), UserGripOverrule.TheOverrule, true);
                Overrule.AddOverrule(RXObject.GetClass(typeof(MText)), UserGripOverrule.TheOverrule, true);
            }
        }
        //关闭夹点规则重定义
        public static void closeGripRule()
        {
            if (UserGripOverrule.TheOverrule != null)
            {
                Overrule.RemoveOverrule(RXObject.GetClass(typeof(BlockReference)), UserGripOverrule.TheOverrule);
                Overrule.RemoveOverrule(RXObject.GetClass(typeof(DBText)), UserGripOverrule.TheOverrule);
                Overrule.RemoveOverrule(RXObject.GetClass(typeof(MText)), UserGripOverrule.TheOverrule);
            }
        }
    }

    //#region 鼠标停留事件
    public class MouseTip
    {
        //存储鼠标停留处的块参照的ObjectId
        [CommandMethod("StartMonitor")]
        public static void StartMonitor()
        {
            //添加鼠标监视事件
            //ed.PointMonitor += new PointMonitorEventHandler(ed_PointMonitor);
        }
        public static void ed_PointMonitor(object sender, PointMonitorEventArgs e)
        {
            try
            {
                if (mouse.acadVer.Contains("R18.0"))
                {
                    var paths = e.Context.GetPickedEntities();
                    if (!paths.Any()) return;
                    long id = 0, fid = 0, fno = 0;
                    string blockInfo = ""; //用于存储块参照的信息：名称和个数
                    string strFno = "";
                    //获取命令行对象（鼠标监视事件的发起者），用于获取文档对象
                    //获取鼠标停留处的实体
                    using (
                        var trans =
                            HostApplicationServices.WorkingDatabase.GetDocument().TransactionManager.StartTransaction())
                    {
                        //如果鼠标停留处有实体
                        if (paths.Length > 0)
                        {
                            //获取鼠标停留处的实体
                            var path = paths[0];
                            if (DBEntityFinder.Instance.GetG3EIds(path.GetObjectIds()[0], ref id, ref fid, ref fno))
                            {
                                if (FeatureMapping.instance.features.ContainsKey(fno.ToString()))
                                    strFno = FeatureMapping.instance.features[fno.ToString()];
                                blockInfo = string.Format("FNO:{0}\nFID:{1}\n设备类型:{2}", fno, fid, strFno);
                            }
                        }
                        trans.Commit();
                    }
                    if (blockInfo != "")
                    {
                        e.AppendToolTipText(blockInfo); //在鼠标停留处显示提示信息                  
                    }
                }
            }
            catch (System.Exception ex)
            {
                PublicMethod.Instance.Editor.WriteMessageWithReturn(ex);
            }
        }
        [CommandMethod("StopMonitor")]
        public static void StopMonitor()
        {
            PublicMethod.Instance.Editor.PointMonitor -= ed_PointMonitor;
        }
    }
}