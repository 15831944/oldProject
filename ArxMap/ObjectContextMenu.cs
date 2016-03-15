using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Autodesk.AutoCAD.ApplicationServices;
using Autodesk.AutoCAD.DatabaseServices;
using Autodesk.AutoCAD.EditorInput;
using Autodesk.AutoCAD.Geometry;
using Autodesk.AutoCAD.Runtime;
using Autodesk.AutoCAD.Windows;
using DotNetARX;
using ElectronTransferDal.Cad;
using ElectronTransferDal.Common;
using ElectronTransferDal.Query;
using ElectronTransferFramework;
using ElectronTransferModel;
using ElectronTransferModel.Base;
using ElectronTransferModel.V9_4;
using acadApp = Autodesk.AutoCAD.ApplicationServices.Application;

namespace ArxMap
{
    public class ObjectContextMenu
    {
        /// <summary>
        /// 右键Gis功能位置属性
        /// </summary>
        //public static MenuItem mi1;
        /// <summary>
        /// 右键批量修改
        /// </summary>
        public static MenuItem mi3;
        /// <summary>
        /// 右键开关柜管理
        /// </summary>
        public static MenuItem mi6;
        /// <summary>
        /// 右键户表管理
        /// </summary>
        public static MenuItem mi7;
        /// <summary>
        /// 右键新增标注
        /// </summary>
        public static MenuItem labelMI;
        /// <summary>
        /// 右键详图管理
        /// </summary>
        public static MenuItem mi8;
        static ContextMenuExtension m_ContextMenu; //右键菜单
        public static bool isBulkChange;
        private static ObjectId objectId
        {
            get { return mouse.selectedEntityId; }
        }
        private static IEnumerable<ObjectId> objectIds
        {
            get { return mouse._selectedObjectIds; }
        }
        
        public static event EventHandler EntityAttributeHandler;
        public static event EventHandler EntityConnectHandler;
        public static event EventHandler EntitySelectionHandler;
        public static event EventHandler FixEntityHandler;
        public static event EventHandler TZWebHandler;
        public static event EventHandler KGGManager;
        public static event EventHandler SHBManager;
        public static event EventHandler JLBManager;
        public static event EventHandler AddLabelHandler;
        public static event EventHandler ZXBZHandler;

        #region 添加右键菜单
        public static void AddContextMenu()
        {
            m_ContextMenu = new ContextMenuExtension();
            m_ContextMenu.Title = "我的自定义菜单";
            var micx = new MenuItem("扩展属性");
            //mi1 = new MenuItem("功能位置(&N)");
            var mi2 = new MenuItem("拓扑关系管理(Alt+A)");
            mi3 = new MenuItem("批量修改(Alt+X)");
            var mi4 = new MenuItem("台账信息(&T)");
            mi6 = new MenuItem("开关柜管理(&K)");
            mi7 = new MenuItem("户表管理");
            labelMI = new MenuItem("标注管理(&B)");
            mi8 = new MenuItem("详图管理");
            var zxbz = new MenuItem("杂项标注");

            // 关联菜单项的处理函数
            micx.Click += micx_Click;
            //mi1.Click += MI1_OnClick;
            mi2.Click += MI2_OnClick;
            mi3.Click += MI3_OnClick;
            mi4.Click += MI4_OnClick;
            mi6.Click += MI6_OnClick;
            mi7.Click += MI7_OnClick;
            mi8.Click += MI8_OnClick;
            labelMI.Click += mi8_OnClick;
            zxbz.Click += zxbz_Click;

            m_ContextMenu.MenuItems.Add(micx);
            //m_ContextMenu.MenuItems.Add(mi1);
            m_ContextMenu.MenuItems.Add(mi2);
            m_ContextMenu.MenuItems.Add(mi3);
            m_ContextMenu.MenuItems.Add(mi4);
            m_ContextMenu.MenuItems.Add(mi6);
            m_ContextMenu.MenuItems.Add(mi7);
            m_ContextMenu.MenuItems.Add(mi8);
            m_ContextMenu.MenuItems.Add(labelMI);
            //m_ContextMenu.MenuItems.Add(zxbz);
            Application.AddObjectContextMenuExtension(RXClass.GetClass(typeof(Entity)), m_ContextMenu);
        }

        static void zxbz_Click(object sender, EventArgs e)
        {
            var g3eObject = new G3EObject();
            if (DBEntityFinder.Instance.GetG3EIds(objectId, ref g3eObject))
            {
                var ee = new FixEntityArgs
                {
                    g3eObject=g3eObject,
                    ObjId = objectId
                };
                if (ZXBZHandler != null)
                {
                    ZXBZHandler(new object(), ee);
                }
            }
        }

        static void micx_Click(object sender, EventArgs e)
        {
            foreach (ObjectId id in objectIds)
            {
                var g3eObject = new G3EObject();
                DBEntityFinder.Instance.GetG3EIds(id, ref g3eObject);
                PublicMethod.Instance.ShowMessage(string.Format("\nid={0} fid={1} fno={2}\n", g3eObject.G3E_ID, g3eObject.G3E_FID, g3eObject.G3E_FNO));
            }
        }
        #endregion 

        #region 卸载右键菜单
        /// <summary>
        /// 卸载右键菜单
        /// </summary>
        public static void RemoveContextMenu()
        {
            try
            {
                if (m_ContextMenu != null)
                {
                    Application.RemoveObjectContextMenuExtension(RXObject.GetClass(typeof(Entity)), m_ContextMenu);
                    m_ContextMenu.Dispose();
                }
            }
            catch
            {
                PublicMethod.Instance.ShowMessage("卸载右键菜单失败！");
            }
        }
        #endregion

        #region 添加标注
        /// <summary>
        /// 添加标注
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        static void mi8_OnClick(object sender, EventArgs e)
        {
            var g3eObject = new G3EObject();
            if (DBEntityFinder.Instance.GetG3EIds(objectId, ref g3eObject))
            {
                var ee = new FixEntityArgs
                {
                    g3eObject=g3eObject,
                    ObjId = objectId
                };
                if (AddLabelHandler != null)
                {
                    AddLabelHandler(new object(), ee);
                }
            }
        }

        #endregion

        #region GIS功能位置属性
        /// <summary>
        /// 右键菜单  GIS功能位置属性
        /// </summary>
        /// <param name="Sender"></param>
        /// <param name="e"></param>
        static void MI1_OnClick(object Sender, EventArgs e)
        {
            var ent = PublicMethod.Instance.GetObject(objectId, OpenMode.ForRead) as Entity;
            if (ent != null)
            {
                //如果是标注就跳过
                if (ent.Layer.Contains("工单锁定框"))
                {
                    PublicMethod.Instance.ShowMessage("\n该实体没有功能位置属性！！！");
                    return;
                }
                var ee = new DevAttrArgs
                             {
                                 devObjId = ent.ObjectId
                             };
                if (EntityAttributeHandler != null)
                {
                    EntityAttributeHandler(new object(), ee);
                }
            }
        }

        #endregion

        #region 拓扑关系
        static void MI2_OnClick(object Sender, EventArgs e)
        {
            PublicMethod.Instance.UnHighlightEntities();
            AddOrRefreshConnectCtr(true);
        }

        public static void AddOrRefreshConnectCtr(bool badd)
        {
            try
            {
                if (objectId != ObjectId.Null)
                {
                    if (PublicMethod.Instance.lockFrameObjectID == objectId) return;
                    var selectedEntity = PublicMethod.Instance.GetObject(objectId, OpenMode.ForRead) as Entity;
                    if (selectedEntity == null) return;
                    
                    G3EObject g3eObject=null;
                    if (false == DBEntityFinder.Instance.GetG3EIds(objectId, ref g3eObject))
                    {
                        PublicMethod.Instance.ShowMessage("实体没有扩展属性！\n");
                        return;
                    }
                    var ee = new selectEntityArgs
                                 {
                                     g3eObject=g3eObject,
                                     objId = objectId,
                                     badd = badd //新建or更新面板
                                 };
                    if (EntityConnectHandler != null)
                    {
                        EntityConnectHandler(new object(), ee);
                    }
                }
            }
            catch (System.Exception ex)
            {
                PublicMethod.Instance.Editor.WriteMessageWithReturn(ex);
            }
        }

        #endregion

        #region 批量修改功能位置属性
        /// <summary>
        /// 右键菜单批量修改 GIS功能位置属性
        /// </summary>
        /// <param name="Sender"></param>
        /// <param name="e"></param>
        static void MI3_OnClick(object Sender, EventArgs e)
        {
            SelectBulkChangeFeature();
        }

        public static void ShowBulkChange()
        {
            SelectBulkChangeFeature();
        }
        private static void SelectBulkChangeFeature()
        {
            var ee = new BlukSelectArgs();
            try
            {
                isBulkChange = true;
                var selops = new PromptSelectionOptions();
                //建立选择的过滤器
                var filList = new TypedValue[4]; //里面放的是过滤条件的个数
                filList[0] = new TypedValue((int)DxfCode.Operator, "<or");
                filList[1] = new TypedValue((int)DxfCode.Start, "LWPOLYLINE");
                filList[2] = new TypedValue((int)DxfCode.Start, "INSERT");
                filList[3] = new TypedValue((int)DxfCode.Operator, "or>");
                //建立过滤器
                var filter = new SelectionFilter(filList);
                //按照过滤器进行选择
                var ents = PublicMethod.Instance.Editor.GetSelection(selops, filter);
                if (ents.Status == PromptStatus.OK)
                {
                    var ss = ents.Value;
                    ee.ObjIdList.Clear();
                    ee.BulkIds.Clear();
                    foreach (ObjectId objId in ss.GetObjectIds())
                    {
                        var ent = PublicMethod.Instance.GetObject(objId, OpenMode.ForRead);
                        if (ent == null) continue;
                        Entity entity = ent as Entity;
                        if (entity.Layer.Contains("工单锁定框")) continue;
                        G3EObject goEObject = new G3EObject();
                        if (DBEntityFinder.Instance.GetG3EIds(objId, ref goEObject))
                        {
                            ee.BulkIds.Add(goEObject);
                        }
                        ee.ObjIdList.Add(entity.ObjectId);
                    }
                    if (EntitySelectionHandler != null)
                    {
                        if (ee.BulkIds.Count <= 0 && ee.ObjIdList.Count <= 0)
                        {
                            PublicMethod.Instance.AlertDialog("请您选择要批量修改的设备集!!!");
                            return;
                        }
                        EntitySelectionHandler(new object(), ee);
                    }
                }
            }
            catch
            {
                
            }
        }
        #endregion

        #region 开关柜管理
        /// <summary>
        /// 开关柜管理
        /// </summary>
        /// <param name="Sender"></param>
        /// <param name="e"></param>
        static void MI6_OnClick(object Sender, EventArgs e)
        {
            var ent = PublicMethod.Instance.GetObject(objectId, OpenMode.ForRead) as Entity;
            //如果是标注就跳过
            if (ent is DBText || ent is MText)
            {
                return;
            }
            if (ent != null)
            {
                var g3eObject = new G3EObject();
                if (false == DBEntityFinder.Instance.GetG3EIds(ent.ObjectId, ref g3eObject))
                {
                    return;
                }
                var ee = new selectEntityArgs
                             {
                                 g3eObject=g3eObject,
                                 objId = ent.ObjectId
                             };
                if (KGGManager != null)
                {
                    KGGManager(new object(), ee);
                }
            }
        }

        #endregion

        #region 户表管理
        /// <summary>
        /// 户表管理
        /// </summary>
        /// <param name="Sender"></param>
        /// <param name="e"></param>
        static void MI7_OnClick(object Sender, EventArgs e)
        {
            var g3eObject = new G3EObject();
            if (DBEntityFinder.Instance.GetG3EIds(objectId, ref g3eObject))
            {
                var ee = new selectEntityArgs
                {
                    g3eObject=g3eObject,
                    objId = objectId
                };
                if (SHBManager != null)
                {
                    SHBManager(new object(), ee);
                }
            }
        }

        #endregion

        
        #region 台帐信息
        /// <summary>
        /// 鼠标右键 台账信息点击事件
        /// </summary>
        /// <param name="Sender"></param>
        /// <param name="e"></param>
        static void MI4_OnClick(object Sender, EventArgs e)
        {
            try
            {
                var UrlStr = IsNewEquipmentAndURL(objectId);
                var e1 = new TZArgs { urlPath = UrlStr };
                if (TZWebHandler != null)
                {
                    TZWebHandler(new object(), e1);
                }
            }catch
            {
            }
        }
        /// <summary>
        /// 判断新增设备 打开台账编辑页面 
        /// </summary>
        public static string IsNewEquipmentAndURL(ObjectId objectId)
        {
            var urlstr = string.Empty;
            long id = 0, fid = 0, fno = 0;
            try
            {
                var selectedEntity =PublicMethod.Instance.GetObject(objectId, OpenMode.ForRead);
                if (selectedEntity is DBText)
                {
                    PublicMethod.Instance.ShowMessage("你选择的是设备标注，请重新选择！");
                    return null;
                }
                if (DBEntityFinder.Instance.GetG3EIds(selectedEntity.ObjectId, ref id, ref fid, ref fno))
                {
                    var ptVlaue = PublicMethod.Instance.GetDBSymbolFinder(objectId) as ElectronSymbol;
                    var com = CYZCommonFunc.GetModelEntity(fid) as Common_n;
                    if (ptVlaue == null || com == null)
                    {
                        return string.Empty;
                    }
                    //获取gis工单ID
                    var ltt_id = ptVlaue.GetValue("LTT_ID").ToString(); 

                    var sbmc = string.Empty;
                    if (!string.IsNullOrEmpty(com.SBMC))
                    {
                        sbmc = BitConverter.ToString(Encoding.UTF8.GetBytes(com.SBMC)).Replace("-", "<M>");
                    }
                    //获取FID 新增设备需加a
                    var g3efid = ptVlaue.EntityState == EntityState.Insert ? string.Format("a{0}", fid) : fid.ToString();
                    //编辑设备台帐信息默认为'Y'
                    urlstr =string.Format(
                            "http://localhost:9090/emmis/equipGisMappingTemp/getInstallEquipments.gis?" +
                            "g3e_fid={0}&" +
                            "jobId={1}&" +
                            "g3e_fno={2}&editAble=Y&" +
                            "funcplaceName={3}&jgdh=009",
                            g3efid, ltt_id, fno, sbmc);
                }
            }
            catch (System.Exception ex)
            {
                LogManager.Instance.Error(ex);
            }
            return urlstr;
        }

        #endregion

        #region 设备选择事件
        /// 鼠标单击实体调用属性
        public static void ShowAttribute(ObjectId objectId)
        {
            int[] fnoList ={ 142, 149, 163 };
            try
            {
                var entity = PublicMethod.Instance.GetObject(objectId, OpenMode.ForRead) as Entity;
                if (entity != null)
                {
                    var g3eObject = new G3EObject();
                    if (DBEntityFinder.Instance.GetG3EIds(entity.ObjectId, ref g3eObject))
                    {
                        //杂标
                        //if (g3eObject.G3E_FNO == 250) return;
                        var ee = new FixEntityArgs
                                     {
                                         g3eObject=g3eObject,
                                         ObjId = entity.ObjectId,
                                     };

                        #region

                        //如果不是低压集中抄表箱和单选就使右键菜单变为灰色
                        if ((g3eObject.G3E_FNO == 159) /*&& mouse._selectedObjectIds.Count() == 1*/)
                            mi7.Enabled = true;
                        else
                            mi7.Enabled = false;
                        //如果不是变压器、计量柜和单选就使右键菜单变为灰色
                        if ((g3eObject.G3E_FNO == 148 || g3eObject.G3E_FNO == 84) /*&& mouse._selectedObjectIds.Count() == 1*/)
                            mi8.Enabled = true;
                        else
                            mi8.Enabled = false;
                        if (fnoList.Any(o => o == g3eObject.G3E_FNO))
                            mi6.Enabled = true;
                        else
                            mi6.Enabled = false;

                        #endregion
                        ee.devname = entity.Layer;
                        if (FixEntityHandler != null)
                        {
                            FixEntityHandler(new object(), ee);
                        }
                    }
                }
            }
            catch(System.Exception ex)
            {
                PublicMethod.Instance.ShowMessage("鼠标单击实体调用属性错误");
                LogManager.Instance.Error(ex.Message);
            }
        }
        #endregion

        #region 详图
        static void MI8_OnClick(object sender, EventArgs e)
        {
            var g3eObject = new G3EObject();
            if (DBEntityFinder.Instance.GetG3EIds(objectId, ref g3eObject))
            {
                var ee = new selectEntityArgs
                {
                    g3eObject=g3eObject,
                    objId = objectId
                };
                if (JLBManager != null)
                {
                    JLBManager(new object(), ee);
                }
            }
        }
        #endregion
    }
    public class selectEntityArgs : EventArgs
    {
        public G3EObject g3eObject=new G3EObject();
        public ObjectId objId = ObjectId.Null;
        public bool badd;   //新建or更新面板
        public List<ObjectId> DevList = new List<ObjectId>();
    }

    public class DevAttrArgs : EventArgs
    {
        public ObjectId devObjId { get; set; }
    }
    public class BlukSelectArgs : EventArgs
    {
        /// <summary>
        /// 批量框选的设备，这里只是保存了CAD中有ObjectID的设备
        /// </summary>
        public List<ObjectId> ObjIdList = new List<ObjectId>();
        /// <summary>
        /// 存放批量修改所有设备的FID其中包括没有ObjectID 的设备
        /// </summary>
        public List<G3EObject> BulkIds = new List<G3EObject>();
    }
    public class TZArgs : EventArgs
    {
        public string urlPath { set; get; }
    }
    public class FixEntityArgs : EventArgs
    {
        public G3EObject g3eObject=new G3EObject();
        public string devname;
        public ObjectId ObjId = ObjectId.Null;
        public Autodesk.AutoCAD.Colors.Color color;
        public Point3d Position;
        public double Rotation;
        public string DevBlockName;
    }
}
