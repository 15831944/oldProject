using System;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Windows.Documents;
using ArxMap;
using Autodesk.AutoCAD.DatabaseServices;
using Autodesk.AutoCAD.Interop;
using Autodesk.AutoCAD.Runtime;
using Autodesk.AutoCAD.Windows;
using System.Windows.Forms;
using ElectronTransferDal.Cad;
using ElectronTransferDal.Common;
using ElectronTransferFramework;
using ElectronTransferModel.V9_4;
using ElectronTransferView.ConnectivityManager;
using ElectronTransferView.ContextMenuManager;
using ElectronTransferView.Menu;
using ElectronTransferView.SearchManager;
using ElectronTransferView.SyncTZDataToGIS;
using ElectronTransferView.TopologyManager;
using Exception = System.Exception;
using acApp = Autodesk.AutoCAD.ApplicationServices.Application;
using ElectronTransferDal.Query;

namespace ElectronTransferView.ViewManager
{
    public class ViewHelper
    {
        public static AcadToolbar TopologyTool;
        public static AcadToolbar DrawTool;
        public static PaletteSet palette;
        private static Control psSymbolPanel;

        private static PaletteSet layerPs;
        private static LayerManage layerManage;
        private static PaletteSet psMenu;

        public static PaletteSet ConnetPs;  //连接关系面板
        private static ConnectManager ConnectCtl;

        public static PaletteSet ConnectSelPs;
        private static ConnectBySel ConnectSelCtr;

        public static PaletteSet BulkChangesPs;
        public static BulkChanges BulkChangesCtl;

        public static PaletteSet DevAttributePs;
        public static DevAttribute DevAttributeCtl;

        public static PaletteSet SyncDataPs;
        public static SyncTzDataToCAD SyncTz;
        /// <summary>
        /// 标注面板
        /// </summary>
        public static PaletteSet labelManagerPs;
        /// <summary>
        /// 标注窗体
        /// </summary>
        public static LabelManager labelManager;
        
        //台帐面板
        public static PaletteSet TZPalette;
        public static TaiZhangPalette TZCtl;
        //杂项标注
        public static PaletteSet zxLabelManagerPs;
        public static ZXLabelForm zxLabelManager;

        //程序集物理路径
        private static readonly string dirPath = Assembly.GetExecutingAssembly().GetPhysicalDirectory();

        #region 菜单面板

        /// <summary>
        /// 菜单面板
        /// </summary>
        [CommandMethod("AddMenuPanel")]
        public static void AddMenuPanel()
        {
            try
            {
                if (psMenu != null)
                {
                    psMenu.Visible = true;
                }
                else
                {
                    psMenu = new PaletteSet("")
                             {
                                 {"", new MenuControl()}
                             };
                    psMenu.Style = PaletteSetStyles.ShowPropertiesMenu;
                    psMenu.Visible = true;
                    psMenu.Size = new Size(124, 30);
                    psMenu.MinimumSize = new Size(124, 30);
                    psMenu.Dock = DockSides.Top;
                }
            }
            catch (Autodesk.AutoCAD.Runtime.Exception)
            {
                PublicMethod.Instance.ShowMessage("菜单面板加载失败！");
            }
        }

        #endregion

        #region 符号库面板

        /// <summary>
        /// 打开属性面板
        /// </summary>
        [CommandMethod("AddSymbolPanel")]
        public static void AddSymbolPanel()
        {
            try
            {
                if (palette != null)
                {
                    palette.Visible = true;
                }
                else
                {
                    psSymbolPanel = new PanelControl();
                    palette = new PaletteSet("符号库")
                                  {
                                      {"用户控件", psSymbolPanel}
                                  };
                    palette.Visible = true;
                    palette.Size = new Size(305, 140);
                    palette.MinimumSize = new Size(295,140);
                    palette.Dock = DockSides.Right;
                    palette.KeepFocus = true;
                }
            }
            catch (Autodesk.AutoCAD.Runtime.Exception)
            {
                PublicMethod.Instance.ShowMessage("属性面板加载失败！");
            }
        }

        #endregion

        #region 图层面板
        /// <summary>
        /// 图层控制面板
        /// </summary>
        [CommandMethod("AddLayerPanel")]
        public static void AddLayerPanel()
        {
            try
            {
                if (layerManage != null && layerPs != null)
                {
                    layerPs.Visible = true;
                    layerManage.InitiationLayer();
                }
                else
                {
                    layerManage = new LayerManage();
                    layerPs = new PaletteSet("图层管理")
                                  {
                                      {"用户控件", layerManage}
                                  };

                    layerPs.Visible = true;
                    layerPs.Size = new Size(220, 140);
                    layerPs.Dock = DockSides.Right;
                    layerPs.StateChanged += layerPs_StateChanged;
                    DCadApi.isShowLayerManage = true;
                }
            }
            catch (Autodesk.AutoCAD.Runtime.Exception)
            {
                PublicMethod.Instance.ShowMessage("图层管理面板加载失败！");
            }
        }

        static void layerPs_StateChanged(object sender, PaletteSetStateEventArgs e)
        {
            switch (e.NewState.ToString())
            {
                case "Hide":
                    DCadApi.isShowLayerManage = false;
                    MenuControl.tsmi.Checked = false;
                    break;
                case "Show":
                    DCadApi.isShowLayerManage = true;
                    break;
            }
        }

        public static void HideLayerPanel()
        {
            if (layerManage != null && layerPs != null)
            {
                layerPs.Visible = false;
            }
        }
        #endregion

        #region 连接关系面板

        /// <summary>
        /// 连接关系面板
        /// </summary>
        /// <param name="ee"> </param>
        public static void AddOrUpdateConnectPanel(selectEntityArgs ee)
        {
            try
            {
                //PublicMethod.Instance.Editor.SetImpliedSelection(new ObjectId[0]);
                if (null == ConnectCtl && null == ConnetPs)
                {
                    if (false == ee.badd)
                    {
                        return;
                    }
                    ConnectCtl = new ConnectManager();
                    //ConnectSel = new ConnectBySel();
                    ConnetPs = new PaletteSet("拓扑关系管理")
                                   {
                                       {"用户控件", ConnectCtl}
                                       //{"yonghu",ConnectSel}
                                   };

                    ConnetPs.Size = new Size(246, 140);
                    ConnetPs.Dock = DockSides.Left;
                    ConnetPs.StateChanged += ConnetPs_StateChanged;
                    ConnetPs.Visible = true;
                }
                else if (false == ee.badd && ConnetPs.Visible != true)
                    return;

                ConnetPs.Visible = true;
                if (ConnectCtl != null) ConnectCtl.SetNod(ee);
            }
            catch (Autodesk.AutoCAD.Runtime.Exception)
            {
                PublicMethod.Instance.ShowMessage("连接关系面板加载失败！");
            }
        }

        static void ConnetPs_StateChanged(object sender, PaletteSetStateEventArgs e)
        {
            switch (e.NewState.ToString())
            {
                case "Hide":
                    ConnectManager.CleanSelcol();
                    break;
                case "Show":
                    break;
            }
        }

        public static void HideConnectPsState()
        {
            if (ConnetPs != null)
                if (ConnetPs.Visible)
                {
                    ConnetPs.Visible = false;
                }
        }
        #endregion

        #region 连接关系BySel面板

        /// <summary>
        /// 连接关系BySel面板
        /// </summary>
        public static void AddConnectBySelPanel()
        {
            try
            {
                if (null != ConnectCtl && null != ConnetPs)
                {
                    if (ConnetPs.Visible) ConnetPs.Visible = false;
                }
                if (null == ConnectSelCtr && null == ConnectSelPs)
                {
                    ConnectSelCtr = new ConnectBySel();
                    ConnectSelPs = new PaletteSet("连接关系")
                              {
                                  {"用户控件", ConnectSelCtr}
                              };
                    ConnectSelPs.Size = new Size(256, 140);
                    ConnectSelPs.MinimumSize = new Size(246,140);
                    ConnectSelPs.Dock = DockSides.Left;
                    ConnectSelPs.StateChanged += ConnectSelPs_StateChanged;
                }
                ConnectSelPs.Visible = true;
            }
            catch (Autodesk.AutoCAD.Runtime.Exception)
            {
                PublicMethod.Instance.Editor.WriteMessage("连接关系BySel面板加载失败！");
            }
        }

        static void ConnectSelPs_StateChanged(object sender, PaletteSetStateEventArgs e)
        {
            if (e.NewState == StateEventIndex.Hide)
            {
                ConnectSelCtr.Visible = false;
            }
            else if (e.NewState == StateEventIndex.Show)
            {
                ConnectSelCtr.Visible = true;
            }

        }
        #endregion

        #region GIS功能位置属性

        private static void LoadDevAttribute()
        {
            if (DevAttributePs == null && DevAttributeCtl == null)
            {
                DevAttributeCtl = new DevAttribute();
                DevAttributePs = new PaletteSet("功能位置属性")
                {
                    {
                        "用户控件", DevAttributeCtl
                    }
                };
                DevAttributePs.Size = new Size(395, 500);
                DevAttributePs.StateChanged += DevAttributePs_StateChanged;
            }
        }
        public static void AddDevAttribute(DevAttrArgs ee)
        {
            try
            {
                LoadDevAttribute();
                DevAttributeCtl.objId = ee.devObjId;
                DevAttributeCtl.AddDevAttribute();
                DevAttributePs.Visible = true;
            }
            catch (Exception)
            {
                PublicMethod.Instance.Editor.WriteMessage("功能位置属性面板加载失败！");
            }
        }

        public static void DevAttributePs_StateChanged(object sender, PaletteSetStateEventArgs e)
        {
            DevAttributeCtl.Visible = e.NewState != StateEventIndex.Hide;
        }

        #endregion

        #region 批量修改GIS功能位置属性

        private static void LoadBulkAttribute()
        {
            if (BulkChangesPs == null && BulkChangesCtl == null)
            {
                BulkChangesCtl = new BulkChanges();
                BulkChangesPs = new PaletteSet("GIS批量修改属性")
                {
                    {"用户控件", BulkChangesCtl}
                };

                BulkChangesPs.Size = new Size(395, 520);
                //BulkChangesPs.StateChanged += BulkChangesPs_StateChanged;
            }
        }
        private static void VerifyBulkEnts(G3EObject g3eObj, ref List<G3EObject> objids)
        {
            if (g3eObj.G3E_FNO == 160) //低压散户表
            {
                var shbd = DBManager.Instance.GetEntity<Gg_jx_shbd_pt>(o => o.G3E_FID == g3eObj.G3E_FID);
                if (shbd != null)
                {
                    var cbx =
                        DBManager.Instance.GetEntity<Detailreference_n>(o => o.G3E_DETAILID == shbd.G3E_DETAILID);
                    if (cbx != null)
                    {
                        if (DBEntityFinder.Instance.VerifyLTTID(cbx.G3E_FID))
                            objids.Add(g3eObj);
                    }
                }
            }
            else if (g3eObj.G3E_FNO == 41) //计量表
            {
                var shbd = DBManager.Instance.GetEntity<Gg_jx_jlb_pt>(o => o.G3E_FID == g3eObj.G3E_FID);
                if (shbd != null)
                {
                    var byq =
                        DBManager.Instance.GetEntity<Detailreference_n>(o => o.G3E_DETAILID == shbd.G3E_DETAILID);
                    if (byq != null)
                    {
                        if (DBEntityFinder.Instance.VerifyLTTID(byq.G3E_FID))
                            objids.Add(g3eObj);
                    }
                }
            }
            else if (DBEntityFinder.Instance.VerifyLTTID(g3eObj.G3E_FID))
                objids.Add(g3eObj);
        }
        /// <summary>
        /// 批量修改属性面板
        /// </summary>
        /// <param name="ee"></param>
        public static void AddBulkChangesAttribute(BlukSelectArgs ee)
        {

            List<G3EObject> objids = new List<G3EObject>();
            int beforeFilter = ee.BulkIds.Count;
            foreach (var g3eObj in ee.BulkIds)
            {
                VerifyBulkEnts(g3eObj, ref objids);
            }
            int afterFilter = objids.Count;
            if (objids.Any())
            {
                string res = beforeFilter == afterFilter
                    ? string.Format("当前选定的{0}个设备全部符合批量修改条件,", afterFilter)
                    : string.Format("已从选定的{0}设备中过滤出符合批量修改的{1}个设备,其余的设备未被锁定,不能修改;", beforeFilter, afterFilter);
                if (MessageBox.Show(res + "是否确定批量修改?", "CAD提示",
                   MessageBoxButtons.YesNo,
                   MessageBoxIcon.Information) != DialogResult.No)
                {
                    try
                    {
                        LoadBulkAttribute();
                        //先清空
                        BulkChangesCtl._cmbItems = objids;
                        BulkChangesCtl.RefAttributeData();
                        BulkChangesPs.Visible = true;
                        BulkChangesPs.Dock = DockSides.Left;
                    }
                    catch (Autodesk.AutoCAD.Runtime.Exception)
                    {
                        PublicMethod.Instance.Editor.WriteMessage("批量修改属性面板加载失败!!!\n");
                    }
                }

            }
            else
            {
                MessageBox.Show("当前选定的设备全部未被锁定，不能修改!!!", "CAD提示", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
               
        }

        //static void BulkChangesPs_StateChanged(object sender, PaletteSetStateEventArgs e)
        //{
        //    if (e.NewState == StateEventIndex.Hide)
        //    {
        //        BulkChangesCtl.Visible = false;
        //    }
        //    else
        //    {
        //        BulkChangesCtl.Visible = true;
        //    }
        //}
        #endregion

        #region 标注面板
        /// <summary>
        /// 加载标注面板
        /// </summary>
        /// <param name="eventArgs"></param>
        public static void LoadLabelManager(FixEntityArgs eventArgs)
        {
            if (labelManagerPs == null && labelManager == null)
            {
                labelManager = new LabelManager {eventArgs = eventArgs};
                labelManagerPs = new PaletteSet("标注")
                                     {
                                         Visible = true,
                                         Dock = DockSides.None,
                                         Size = new Size(390, 230),
                                         MinimumSize = new Size(360, 220)
                                     };
                labelManagerPs.Add("标注面板", labelManager);
            }
            else
            {
                labelManager.eventArgs = eventArgs;
                labelManager.BindLabelSource();
                labelManagerPs.Visible = true;
            }
        }
        #endregion
        #region 同步台帐面板

        public static void LoadSyncTzManager()
        {
            if (SyncDataPs == null && SyncTz == null)
            {
                SyncTz = new SyncTzDataToCAD();
                SyncDataPs = new PaletteSet("同步台帐数据至CAD")
                {
                    Visible = true,
                    Dock = DockSides.None,
                    Size = new Size(393, 379),
                    MinimumSize = new Size(380, 360)
                };
                SyncDataPs.Add("台帐同步面板", SyncTz);
                SyncDataPs.StateChanged+= SyncDataPsOnStateChanged;
            }
            else
            {
                SyncTz.RestListBox();
                if (SyncDataPs != null) SyncDataPs.Visible = true;
            }
        }

        private static void SyncDataPsOnStateChanged(object sender, PaletteSetStateEventArgs paletteSetStateEventArgs)
        {
            FixEntity.Instance.ResetOldEntity();
        }

        #endregion
        #region 台帐面板
        /// <summary>
        /// 显示台帐面板
        /// </summary>
        public static void LoadTZPalette(string strurl)
        {
            try
            {
                if (null == TZPalette && null == TZCtl)
                {
                    TZCtl = new TaiZhangPalette(strurl);
                    TZPalette = new PaletteSet("台帐属性")
                                    {Visible = true, Dock = DockSides.None, Size = new Size(700, 500)};
                    TZPalette.Add("台帐面板",TZCtl);
                }
                else
                {
                    TZCtl.Refresh(strurl);
                    TZPalette.Visible = true;
                }
            }
            catch (Autodesk.AutoCAD.Runtime.Exception)
            {
                PublicMethod.Instance.ShowMessage("台帐面板加载失败！");
            }
        }
        #endregion

        #region 杂项标注
        public static void LoadZXLabelManager(FixEntityArgs args)
        {  
            try
            {
                zxLabelManager = new ZXLabelForm(args);
                zxLabelManagerPs = new PaletteSet("杂项标注")
                                       {
                                           Visible = true,
                                           Dock = DockSides.None,
                                           Size = new Size(363, 260),
                                           MinimumSize = new Size(350, 260)
                                       };
                zxLabelManagerPs.Add("杂项标注面板", zxLabelManager);
            }
            catch (Autodesk.AutoCAD.Runtime.Exception wException)
            {
                LogManager.Instance.Error(wException);
                PublicMethod.Instance.ShowMessage("杂项标注加载失败！");
            }
        }
        #endregion

        public static void RemoveSymbolPanel()
        {
            if (palette != null && !palette.IsDisposed)
            {
                palette.Visible = false;
                palette.Dispose();
                palette = null;
            }
        }
        /// <summary>
        /// 卸载菜单
        /// </summary>
        public static void RemoveMenuPanel()
        {
            if (psMenu != null && !psMenu.IsDisposed)
            {
                psMenu.Visible = false;
                psMenu.Dispose();
                psMenu = null;
            }
        }
        /// <summary>
        /// 卸载图层管理
        /// </summary>
        public static void RemoveLayerManager()
        {
            if (layerManage != null && !layerPs.IsDisposed)
            {
                layerPs.Visible = false;
                layerPs.Dispose();
                layerManage = null;
            }
        }


        #region 自定义工具条

        /// <summary>
        /// 加载工具栏
        /// </summary>
        public static void AddToolBar()
        {
            try
            {
                TopologyManagerBar();
                DrawManagerBar();
            }
            catch
            {
                PublicMethod.Instance.ShowMessage("加载工具栏出错……");
            }
        }
        public static void DrawManagerBar()
        {
            var acadApp = (AcadApplication)acApp.AcadApplication;
            DrawTool = acadApp.MenuGroups.Item(0).Toolbars.Add("绘图管理");
            var pan = Path.Combine(dirPath, "Resource\\漫游.bmp");
            DrawTool.AddToolbarButton(0, "漫游", "", "pan ", Type.Missing).SetBitmaps(pan, pan);

            var pldd = Path.Combine(dirPath, "Resource\\复制.bmp");
            DrawTool.AddToolbarButton(1, "复制", "", "COPY ", Type.Missing).SetBitmaps(pldd, pldd);
            var zdlj = Path.Combine(dirPath, "Resource\\移动.bmp");
            DrawTool.AddToolbarButton(2, "移动", "", "SBYD ", Type.Missing).SetBitmaps(zdlj, zdlj);
            var lxlj = Path.Combine(dirPath, "Resource\\旋转.bmp");
            DrawTool.AddToolbarButton(3, "旋转", "", "SBXZ ", Type.Missing).SetBitmaps(lxlj, lxlj);
            var sbjx = Path.Combine(dirPath, "Resource\\镜像.bmp");
            DrawTool.AddToolbarButton(4, "镜像", "", "mi ", Type.Missing).SetBitmaps(sbjx, sbjx);
            var ddyd = Path.Combine(dirPath, "Resource\\打断于点.bmp");
            DrawTool.AddToolbarButton(5, "打断于点", "", "ddyd ", Type.Missing).SetBitmaps(ddyd, ddyd);
            var lddd = Path.Combine(dirPath, "Resource\\两点打断.bmp");
            DrawTool.AddToolbarButton(6, "两点打断", "", "lddd ", Type.Missing).SetBitmaps(lddd, lddd);
            DrawTool.AddSeparator(7);

            var jdcz = Path.Combine(dirPath, "Resource\\节点操作.bmp");
            DrawTool.AddToolbarButton(8, "新增节点", "", "av ", Type.Missing).SetBitmaps(jdcz, jdcz);
            var scjd = Path.Combine(dirPath, "Resource\\删除节点.bmp");
            DrawTool.AddToolbarButton(9, "删除节点", "", "rv ", Type.Missing).SetBitmaps(scjd, scjd);

            var redraw = Path.Combine(dirPath, "Resource\\重新绘制.bmp");
            DrawTool.AddToolbarButton(10, "重绘设备", "", "ch ", Type.Missing).SetBitmaps(redraw, redraw);
            

            var fidcx = Path.Combine(dirPath, "Resource\\FID.bmp");
            DrawTool.AddToolbarButton(11, "FID查询", "", "fidcx ", Type.Missing).SetBitmaps(fidcx, fidcx);
            var sbdw = Path.Combine(dirPath, "Resource\\设备定位.bmp");
            DrawTool.AddToolbarButton(12, "设备定位", "", "sbdw ", Type.Missing).SetBitmaps(sbdw, sbdw);
            var zbdw = Path.Combine(dirPath, "Resource\\坐标定位.bmp");
            DrawTool.AddToolbarButton(13, "坐标定位", "", "zbdw ", Type.Missing).SetBitmaps(zbdw, zbdw);
            var gdsd = Path.Combine(dirPath, "Resource\\定位锁定框.bmp");
            DrawTool.AddToolbarButton(14, "定位锁定框", "", "zz ", Type.Missing).SetBitmaps(gdsd, gdsd);
            var re = Path.Combine(dirPath, "Resource\\刷新.bmp");
            DrawTool.AddToolbarButton(15, "刷新", "", "rec ", Type.Missing).SetBitmaps(re, re);


            DrawTool.Visible = true;
            DrawTool.top = 100;
            DrawTool.left = 400;
            //工具栏靠右边停靠
            //DrawTool.Dock(Autodesk.AutoCAD.Interop.Common.AcToolbarDockStatus.acToolbarDockLeft);
        }
        public static void TopologyManagerBar()
        {
            var acadApp = (AcadApplication)acApp.AcadApplication;
            TopologyTool = acadApp.MenuGroups.Item(0).Toolbars.Add("拓扑管理");
            var syzz = Path.Combine(dirPath, "Resource\\上游.bmp");
            TopologyTool.AddToolbarButton(0, "上游追踪", "", "SYZZ ", Type.Missing).SetBitmaps(syzz, syzz);
            var xyzz = Path.Combine(dirPath, "Resource\\下游.bmp");
            TopologyTool.AddToolbarButton(1, "下游追踪", "", "XYZZ ", Type.Missing).SetBitmaps(xyzz, xyzz);
            var qxzz = Path.Combine(dirPath, "Resource\\取消和打段.bmp");
            TopologyTool.AddToolbarButton(2, "取消追踪", "", "cls ", Type.Missing).SetBitmaps(qxzz, qxzz);

            var pldd = Path.Combine(dirPath, "Resource\\批量打段.bmp");
            TopologyTool.AddToolbarButton(3, "批量打断", "", "pldd ", Type.Missing).SetBitmaps(pldd, pldd);
            var zdlj = Path.Combine(dirPath, "Resource\\自动连接.bmp");
            TopologyTool.AddToolbarButton(4, "自动连接", "", "autoc ", Type.Missing).SetBitmaps(zdlj, zdlj);
            var lxlj = Path.Combine(dirPath, "Resource\\连选连接.bmp");
            TopologyTool.AddToolbarButton(5, "连选连接", "", "lxlj ", Type.Missing).SetBitmaps(lxlj, lxlj);
            var mxjl = Path.Combine(dirPath, "Resource\\母线建立.bmp");
            TopologyTool.AddToolbarButton(6, "母线建立", "", "mxjl ", Type.Missing).SetBitmaps(mxjl, mxjl);

            TopologyTool.Visible = true;
            //工具栏靠右边停靠
            TopologyTool.Dock(Autodesk.AutoCAD.Interop.Common.AcToolbarDockStatus.acToolbarDockLeft);
        }
        #endregion
    }
}
