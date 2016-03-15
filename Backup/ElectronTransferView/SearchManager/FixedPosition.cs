using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using Autodesk.AutoCAD.ApplicationServices;
using Autodesk.AutoCAD.Runtime;
using Autodesk.AutoCAD.DatabaseServices;
using Autodesk.AutoCAD.EditorInput;
using ElectronTransferDal.AutoGeneration;
using ElectronTransferDal.Cad;
using ElectronTransferDal;
using ElectronTransferDal.Common;
using ElectronTransferDal.Query;
using ElectronTransferFramework;
using System.Reflection;
using System.Runtime.InteropServices;
using Autodesk.AutoCAD.Geometry;
using ArxMap;
using ElectronTransferModel.V9_4;
using DotNetARX;
using ElectronTransferView.ContextMenuManager;
using ElectronTransferView.Menu;

namespace ElectronTransferView.SearchManager
{
    public partial class FixedPosition : Form
    {
        [System.Runtime.InteropServices.DllImport("user32", EntryPoint = "SetForegroundWindow")]
        public static extern bool SetForegroundWindow(IntPtr hWnd);

        /// <summary>
        /// 保存ListView里面的所有实体
        /// </summary>
        private Dictionary<ObjectId, Entity> entities = new Dictionary<ObjectId, Entity>();
    
        /// <summary>
        /// 记录当前层上的所有实体
        /// </summary>
        private List<FeatureFixedItem> CurrentLayerFeatures = new List<FeatureFixedItem>();

        private ListViewColumnSorter lvcSorter = new ListViewColumnSorter();
        /// <summary>
        /// 记录已加载到listView里面的数据
        /// </summary>
        private IList<ObjectId> loadedData = new List<ObjectId>();
        public FixedPosition()
        {
            InitializeComponent();
            DoubleBuffered = true;
            SetForegroundWindow(Handle);
        }
        private void FixedPosition_Load(object sender, EventArgs e)
        {
            InitializeLayerTree();
            InitializeListView();
            GetDwgLayers();
            GetEntity();
            //LvEntity.MouseWheel += LvEntity_MouseWheel;
        }

        void LvEntity_MouseWheel(object sender, MouseEventArgs e)
        {
            //if (LvEntity.Items.Count < entInCurLayer.Count())
            //{
            //    AddListViewItem();
            ////}
            //if (entInCurLayer.Count > 0)
            //{
            //    AddListViewItem();
            //    CommonHelp.Instance.SortByColumn(new ColumnClickEventArgs(1), lvcSorter, LvEntity);
            //}
        }
        private void InitializeLayerTree()
        {
            treeViewDevLayer.Nodes.Clear();
            treeViewDevLayer.Nodes.Add("中压设备");
            treeViewDevLayer.Nodes.Add("低压设备");
            //treeViewDevLayer.Nodes.Add("工单锁定框");
            treeViewDevLayer.HideSelection = false;
            treeViewDevLayer.DrawMode= TreeViewDrawMode.OwnerDrawText;
            treeViewDevLayer.DrawNode += new DrawTreeNodeEventHandler(treeViewDevLayer_DrawNode);
        }

        void treeViewDevLayer_DrawNode(object sender, DrawTreeNodeEventArgs e)
        {
            e.DrawDefault = true;//这里默认颜色，只需要在treeview失去焦点时选中节点自然凸显
            
            if ((e.State&TreeNodeStates.Selected)!=0)
            {
                e.Graphics.FillRectangle(Brushes.DarkBlue,e.Node.Bounds);
                Font nodeFont = e.Node.NodeFont;
                if (nodeFont == null)
                {
                    nodeFont = ((TreeView) sender).Font;
                }
                e.Graphics.DrawString(e.Node.Text,nodeFont,Brushes.White,Rectangle.Inflate(e.Bounds,0,0));

            }
            else
            {
                e.DrawDefault = true;
            }
            if ((e.State & TreeNodeStates.Focused) != 0)
            {
                using (Pen focusPen=new Pen(Color.Black))
                {
                    focusPen.DashStyle = System.Drawing.Drawing2D.DashStyle.Dot;
                    Rectangle focusRectangle = e.Node.Bounds;
                    focusRectangle.Size=new Size(focusRectangle.Width-1,focusRectangle.Height-1);
                    e.Graphics.DrawRectangle(focusPen,focusRectangle);

                }
            }
        }

        private void InitializeListView()
        {
            LvEntity.Items.Clear();
            LvEntity.View = View.Details;
            LvEntity.FullRowSelect = true;
            LvEntity.GridLines = true;
            LvEntity.LabelEdit = false;
            LvEntity.HideSelection = false;
            LvEntity.AllowColumnReorder = true;
            lvcSorter.SortColumn = 1;
            LvEntity.ListViewItemSorter = lvcSorter;
            LvEntity.Columns.Add("序号", 45, HorizontalAlignment.Left);
            LvEntity.Columns.Add("设备FID", 80, HorizontalAlignment.Center);
            LvEntity.Columns.Add("设备名称", 240, HorizontalAlignment.Center);
            LvEntity.Columns.Add("所属供电局", 120, HorizontalAlignment.Center);
            LvEntity.Columns.Add("受电馈线", 120, HorizontalAlignment.Center);
            LvEntity.Visible = true;
            CommonHelp.Instance.SizeLastColumn(LvEntity);
            ImageList imgList = new ImageList();
            imgList.ImageSize = new System.Drawing.Size(2, 18);
            LvEntity.SmallImageList = imgList;
        }

        /// <summary>
        /// 获取当前DWG文档的所有图层
        /// </summary>
        private void GetDwgLayers()
        {
            Database db = HostApplicationServices.WorkingDatabase;
            using (Transaction trans = db.TransactionManager.StartTransaction())
            {
                //获取数据库层表对象
                LayerTable lt = trans.GetObject(db.LayerTableId, OpenMode.ForRead) as LayerTable;
                //循环遍历每个图层
                foreach (var layer in lt)
                {
                    LayerTableRecord ltr = trans.GetObject(layer, OpenMode.ForRead) as LayerTableRecord;

                    if ((ltr.Name != "0") && (ltr.Name != "背景地图") && (!ltr.Name.Contains("标注")) && (ltr.Name != "lock"))
                    {
                        if (!ltr.IsOff && !ltr.IsFrozen)
                        {
                            if (ltr.Name.Contains("工单锁定框"))
                            {
                                //AddNode(ltr.Name, 2);
                                continue;
                            }
                            if (ltr.Name.Contains("低压"))
                            {
                                AddNode(ltr.Name, 1);
                            }
                            else
                            {
                                AddNode(ltr.Name, 0);
                            }

                        }
                    }

                }
                trans.Commit();
            }
        }
        private void AddNode(String layname, int index)
        {
            TreeNode node = new TreeNode();
            node.Text = layname;
            treeViewDevLayer.Nodes[index].Nodes.Add(node);
        }

        /// <summary>
        /// 获取指定层上的对象 用选择集
        /// </summary>
        /// <returns>对象列表</returns>
        private void GetEntity()
        {
            try
            {
                using (Transaction transaction = PublicMethod.Instance.WorkingDataBase.TransactionManager.StartTransaction())
                {
                    BlockTable blockTable = (BlockTable)transaction.GetObject(HostApplicationServices.WorkingDatabase.BlockTableId, OpenMode.ForRead);
                    BlockTableRecord blockTableRecord = (BlockTableRecord)transaction.GetObject(blockTable[BlockTableRecord.ModelSpace], OpenMode.ForRead);

                    foreach (ObjectId objId in blockTableRecord)
                    {
                        Entity ent = transaction.GetObject(objId, OpenMode.ForRead) as Entity;
                        if (ent.GetType() == typeof(DBText) || ent.Layer.Contains("背景地图") || ent.GetType() == typeof(MText)||ent.Layer.Contains("工单锁定"))
                            continue;
                        entities.Add(ent.ObjectId, ent);
                    }
                    transaction.Commit();
                }

            }
            catch
            {
                PublicMethod.Instance.ShowMessage("获取实体失败！！！");
            }
        }

        /// <summary>
        /// 添加设备列表到listview
        /// </summary>
        private void AddListViewItem()
        {
            try
            {
                if (!CurrentLayerFeatures.Any()) return;
                LvEntity.Items.Clear();
                foreach (var t in CurrentLayerFeatures)
                {
                    var item = new ListViewItem();
                    item.Tag = t;
                    item.SubItems[0].Text = t.BH;
                    item.SubItems.Add(t.FID);
                    var sbmc = string.IsNullOrEmpty(t.SBMC) ? "无" : t.SBMC;
                    item.SubItems.Add(sbmc);
                    var ssdw = string.IsNullOrEmpty(t.SSGDJ) ? "无" : t.SSGDJ;
                    item.SubItems.Add(ssdw);
                    var ssxl = string.IsNullOrEmpty(t.SDKX) ? "无" : t.SDKX;
                    item.SubItems.Add(ssxl);
                    LvEntity.Items.Add(item);
                }
                LvEntity.Sort();
                CommonHelp.Instance.ReCreateSerialNumber(LvEntity);
            }
            catch (System.Exception ex)
            {
                throw new System.Exception(ex.Message);
            }
        }
       
       
        /// <summary>
        /// 根据选择的索引项定位实体
        /// </summary>
        /// <param name="ent"></param>
        private void FixBlock(BlockReference ent)
        {
            try
            {
                double X = Convert.ToDouble(ent.Position.X.ToString());//经度
                double Y = Convert.ToDouble(ent.Position.Y.ToString());//纬度
                string str = string.Format("{0},{1} ", X, Y);
                PublicMethod.Instance.acDoc.SendStringToExecute("zoom\n" + "c\n" + str + "0.6949458523585643053825037693985e-4" + "\n", true, false, true);
                mouse.showmap();
            }
            catch
            {
                //PublicMethod.Instance.ShowMessage("FixEntity函数出错！！！");
            }
        }
        private void FixedPosition_FormClosing(object sender, FormClosingEventArgs e)
        {
            FixEntity.Instance.ResetOldEntity();
            MenuControl.fp = null;
        }
        private void treeViewDevLayer_NodeMouseClick(object sender, TreeNodeMouseClickEventArgs e)
        {
            if (e.Button != MouseButtons.Left)
                return;
            if (e.Node.Parent!=null)
            {
                if (!String.IsNullOrEmpty(e.Node.Text))
                {
                    LvEntity.Items.Clear();
                    //搜索出符合当前层的所有设备
                    var entInCurLayer = entities.Where(o => o.Value.Layer.Equals(e.Node.Text)).Select(o => o.Key);
                    int index = 1;
                    CurrentLayerFeatures.Clear();
                    foreach (var item in entInCurLayer)
                    {
                        G3EObject objIds = new G3EObject();
                        if (DBEntityFinder.Instance.GetG3EIds(item, ref objIds))
                        {
                            var tables = DevEventHandler.GetDevTables(objIds.G3E_FNO, objIds.G3E_FID);
                            if (tables != null)
                            {
                                //更新的情况比较特殊，可能只更新一个表
                                string sbmc = string.Empty;
                                string ssdw = string.Empty;
                                string ssxl = string.Empty;
                                if (tables.ComObj == null)
                                {
                                    if (objIds.G3E_FNO == 159) //集中抄表箱//散户表
                                    {
                                        if (tables.SelfObj != null)
                                        {
                                            if (tables.SelfObj.HasAttribute("HH") &&
                                                tables.SelfObj.GetAttribute("HH") != null)
                                            {
                                                sbmc = tables.SelfObj.GetAttribute("HH");
                                            }
                                            if (tables.SelfObj.HasAttribute("CD_SSXL") &&
                                                tables.SelfObj.GetAttribute("CD_SSXL") != null)
                                            {
                                                ssxl = tables.SelfObj.GetAttribute("CD_SSXL");
                                            }
                                            if (tables.SelfObj.HasAttribute("CD_SSDW") &&
                                                tables.SelfObj.GetAttribute("CD_SSDW") != null)
                                            {
                                                ssdw = tables.SelfObj.GetAttribute("CD_SSDW");
                                            }
                                        }
                                    }
                                    else if (objIds.G3E_FNO == 188) //负控
                                    {
                                        if (tables.GnwzObj != null && tables.GnwzObj.HasAttribute("MC")&&tables.GnwzObj.GetAttribute("MC")!=null)
                                        {
                                            sbmc = tables.GnwzObj.GetAttribute("MC");
                                        }
                                         if (tables.SelfObj != null && tables.SelfObj.HasAttribute("CD_SSDW")&&tables.SelfObj.GetAttribute("CD_SSDW")!=null)
                                         {
                                             ssdw = tables.SelfObj.GetAttribute("CD_SSDW");
                                         }
                                    }
                                }
                                else
                                {
                                    sbmc = tables.ComObj.SBMC;
                                    ssxl = tables.ComObj.CD_SSXL;
                                    ssdw = tables.ComObj.CD_SSDW;
                                }
                                var featureItem = new FeatureFixedItem
                                {
                                    BH = index.ToString(),
                                    FID = objIds.G3E_FID.ToString(),
                                    SBMC =sbmc,
                                    SDKX = ssxl,
                                    SSGDJ = ssdw
                                };
                                if (CurrentLayerFeatures.Any())
                                {
                                    var value = CurrentLayerFeatures.Where(o => o.FID == featureItem.FID);
                                    if (!value.Any())
                                        CurrentLayerFeatures.Add(featureItem);
                                }
                                else
                                    CurrentLayerFeatures.Add(featureItem);
                            }
                        }

                    }
                    AddListViewItem();
                }
            }
        }
       

        private void LvEntity_MouseDoubleClick(object sender, MouseEventArgs e)
        {
            if (e.Button != MouseButtons.Left)
            {
                return;
            }
            try
            {
                if (LvEntity.SelectedIndices.Count <= 0)
                    return;
                //先复位前一个修改的设备颜色
                FixEntity.Instance.ResetOldEntity();
                ListViewItem lvi = LvEntity.SelectedItems[0];
                var FixedItem = lvi.Tag as FeatureFixedItem;
                if (FixedItem != null)
                {
                    long fid = long.Parse(FixedItem.FID);
                    FixEntity.Instance.Fix(fid);
                }
            }
            catch
            {
                PublicMethod.Instance.ShowMessage("定位失败！！！");
            }
        }
        public void CloseDeviceWindow()
        {
            this.Close();
        }

        private void LvEntity_Resize(object sender, EventArgs e)
        {
            CommonHelp.Instance.SizeLastColumn(LvEntity);
        }

        private void FixedPosition_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyValue == 27)
                this.Close();
        }
    }

    public sealed class FeatureFixedItem
    {
        public string BH { get; set; }
        public string FID { get; set; }
        public string SBMC { get; set; }
        public string SSGDJ { get; set; }
        public string SDKX { get; set; }

        public FeatureFixedItem()
        {
        }
    }
}
