using System;
using System.Collections.Generic;
using System.Drawing;
using System.Data;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Windows.Forms;
using ElectronTransferBll;
using ElectronTransferBll.DBEntityHelper;
using ElectronTransferDal.HelperEventArgs;
using ElectronTransferDal.Query;
using ElectronTransferModel.Config;
using ElectronTransferModel.V9_4;
using ElectronTransferView.ConnectivityManager;
using ElectronTransferView.ContextMenuManager;
using ElectronTransferView.Menu;
using ElectronTransferView.SyncTZDataToGIS;
using Application = Autodesk.AutoCAD.ApplicationServices.Application;
using ElectronTransferFramework;
using ElectronTransferDal.Cad;
using ArxMap;
using ElectronTransferView.DataManager;
using cadDS =Autodesk.AutoCAD.DatabaseServices;
using ElectronTransferDal.AutoGeneration;
using ElectronTransferView.VerifyManager;

namespace ElectronTransferView.ViewManager
{
    public partial class PanelControl : UserControl
    {

        public PanelControl()
        {
            InitializeComponent();

        }
        private string _ButtonName;
        private const int _ModuleButtonHeight = 25;
        private PreviewSymbol ps;
        private bool isLTTID { set; get; }
        public static event EventHandler FromPanelMsg;
        public static event EventHandler PanelControlToWorkOrderVerify;
        /// <summary>
        /// 记录当前绑定对象
        /// </summary>
        private static XProps CurPropertyObj;
        /// <summary>
        /// 是否单击图元面板图元
        /// </summary>
        private  bool isclickDev;
        private  int CurrentFno { get; set; }
        private long CurrentFid { get; set; }
        private cadDS.ObjectId CurrentObjId { get; set; }
        private  int oldFno { get; set; }

        /// <summary>
        /// 点击地图设备图元传进来的参数
        /// </summary>
       
        /// <summary>
        /// 是否插入了符号
        /// </summary>
        public bool isInsertSymbol { get; set; }
        /// <summary>
        /// 记录按钮属性
        /// </summary>
        public static List<Button> btnList = new List<Button>();
        private static List<string> listGroup { set; get; }
        private readonly ViewManagerHelper viewManagerHelper = new ViewManagerHelper();
        /// <summary>
        /// 初始化符号库
        /// </summary>
        public void Initialize()
        {
            try
            {
                CreateTemplate();
                listGroup = viewManagerHelper.GetGroupListBySymbolPath(DBEntityFinder.Instance.SymbolLibraryPath);
                if (!listGroup.Any()) return;
                panel1.BorderStyle = BorderStyle.FixedSingle;
                for (var i = 0; i < listGroup.Count; i++)
                {
                    var btn = new Button
                                  {
                                      FlatStyle = FlatStyle.Popup,
                                      Width = panel1.Width,
                                      Height = _ModuleButtonHeight,
                                      Name = string.Format("Button{0}", i),
                                      Text = listGroup[i],
                                      Top = _ModuleButtonHeight*i
                                  };
                    btn.Click += btn_Click;
                    panel1.Controls.Add(btn);
                    btnList.Add(btn);
                }
                panel1.Controls.Add(listView1);
                AddEventHandler();
            }
            catch (Exception ex)
            {
                PublicMethod.Instance.ShowMessage(ex.Message);
            }
        }
        /// <summary>
        /// 创建自定义模版文件夹
        /// </summary>
        private void CreateTemplate()
        {
            var templatePath = string.Format(@"{0}\自定义模版", DBEntityFinder.Instance.SymbolLibraryPath);
            if (!Directory.Exists(templatePath))
            {
                Directory.CreateDirectory(templatePath);
            }
        }
  
        void WorkOrderVerify_RefPanel(object sender, EventArgs e)
        {
            var ow = (orderWorkifyArgs) e;
            if (ow != null)
            {
                CurrentFid = ow.FID;
                CurrentFno = ow.FNO;
                CurrentObjId = ow.ObjId;
                SelectDevAttribute(false);
            }
        }
        void CadDataSource_InitPGridInPanelControl(object sender, EventArgs e)
        {
            ClearPropertyGrid();
        }
       /// <summary>
        /// 更新属性面板
       /// </summary>
       /// <param name="isSyncTz">是否是同步台帐调用次方法</param>
        private void SelectDevAttribute(bool isSyncTz)
        {
            var value = DevEventHandler.GetDevTables(CurrentFno, CurrentFid);
            if(value==null)return;
            if (value.DevObj == null) return;
            GenerateHelper.SetAllBrowsable(value.DevObj,true);

            isLTTID = DBEntityFinder.Instance.VerifyLTTID(CurrentFid);
            if (!isLTTID)
            {
                PublicMethod.Instance.ShowMessage("\n当前设备未被锁定，禁止编辑...");
                GenerateHelper.SetAllReadOnly(value.DevObj,true);
            }
            else
            {
                if(isSyncTz)
                    GenerateHelper.SetAllReadOnly(value.DevObj, false);
                DevPropertyGrid.Enabled = true;
            }
            AutoGenerationHelper.Instance.AddUITypeEditor(value.DevObj, CurrentFno);
            GenerateHelper.GenerateXPropsByAzwz(value.DevObj, CurrentFno,false);
            DevPropertyGrid.SelectedObject = value.DevObj;
            CurPropertyObj = value.DevObj;

            if (!DBEntityFinder.Instance.MemoryDevice.Keys.Contains(CurrentFno))
            {
                DBEntityFinder.Instance.MemoryDevice.Add(CurrentFno, CurPropertyObj);
            }
            else
            {
                DBEntityFinder.Instance.MemoryDevice[CurrentFno] = CurPropertyObj;
            }
            
            DevPropertyGrid.Refresh();
            var simpleSymbolConfigEntry = SimpleDBSymbolsConfig.Instance.DBSymbol.SingleOrDefault(o => o.G3E_FNO == CurrentFno);
            if (simpleSymbolConfigEntry != null)
                DevNameGroup.Text = simpleSymbolConfigEntry.OtherProperty.LayerName + "(*表示必填项)-" + CurrentFid.ToString(CultureInfo.InvariantCulture);
            //保证面板属性和点击设备对应
            isclickDev = true;
            oldFno = 0;

            //绑定受电馈线
            GetSDKXBEntity();
        }

        //private void RefPaletteSet()
        //{
        //    if (ViewHelper.DevAttributeCtl != null && ViewHelper.DevAttributeCtl.Visible)
        //    {
        //        ViewHelper.DevAttributeCtl.objId = CurrentObjId;
        //        ViewHelper.DevAttributeCtl.AddDevAttribute();
        //    }
        //}
        /// <summary>
        /// 单击设备后对应的设备自身属性
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        void mouse_FixEntityHandler(object sender, EventArgs e)
        {
            try
            {
                var ee = (FixEntityArgs)e;
                CurrentFno = ee.g3eObject.G3E_FNO;
                CurrentFid = ee.g3eObject.G3E_FID;
                CurrentObjId = ee.ObjId;
                GenerateHelper.EmptyFidRecord();//清空FID的记录
                CommonHelp.Instance.FromMouseFixEnitiyFid = CurrentFid;
                CommonHelp.Instance.FromMouseFixEntityFno = CurrentFno;
                //RefPaletteSet();
                SelectDevAttribute(false);

                if (ViewHelper.labelManager != null && ViewHelper.labelManagerPs.Visible)
                    ViewHelper.LoadLabelManager(ee);

                if (ViewHelper.TZCtl != null && ViewHelper.TZPalette.Visible)
                {
                    var url = ObjectContextMenu.IsNewEquipmentAndURL(ee.ObjId);
                    ViewHelper.LoadTZPalette(url);
                }
            }
            catch (Autodesk.AutoCAD.Runtime.Exception ex)
            {
                LogManager.Instance.Error(ex);
            }
        }
        public void btn_Click(object sender, EventArgs e)
        {
            GetSymbolGroup((sender as Button).Name);
        }
        public void GetSymbolGroup(string btnName)
        {
            var findOutStatus = false;
            listView1.Clear();
            for (int i = 0; i < panel1.Controls.Count; i++)
            {
                if (panel1.Controls[i].GetType().Name == "Button")
                {
                    //重新定义各个button位置
                    if (!findOutStatus)
                    {
                        panel1.Controls[i].Top = _ModuleButtonHeight * i;
                    }
                    else
                    {
                        panel1.Controls[i].Top = panel1.Height - _ModuleButtonHeight * (listGroup.Count - i);
                    }

                    //找到所点击的Button,在其下加载子项
                    if (panel1.Controls[i].Name == btnName)
                    {
                        findOutStatus = true;
                        var panel = new Panel
                                        {
                                            BackColor = Color.AliceBlue,
                                            Top = _ModuleButtonHeight*(i + 1),
                                            Width = panel1.Width,
                                            Height = panel1.Height - _ModuleButtonHeight*listGroup.Count
                                        };

                        _ButtonName = btnName;
                        GetSymbol(i, listGroup[i]);
                        listView1.Top = panel.Top;
                        var lvHeight = panel1.Height - panel.Top - (_ModuleButtonHeight * (listGroup.Count - i - 1));
                        if (lvHeight > 20)
                            listView1.Height = lvHeight;
                        panel1.Controls.Add(panel);
                    }
                }
            }
        }

        private void PanelControl_Load(object sender, EventArgs e)
        {
            //获取程序集物理路径
            var pluginPath = Assembly.GetExecutingAssembly().GetPhysicalDirectory();
            //符号库根目录
            DBEntityFinder.Instance.SymbolLibraryPath = Path.Combine(pluginPath, MapConfig.Instance.SymbolLibraryPath);
            //图片跟目录
            DBEntityFinder.Instance.SymbolPicturePath = Path.Combine(pluginPath, MapConfig.Instance.SymbolPicturePath);
            //初始化符号面板
            Initialize();

            GetSymbolGroup("Button4");
        }

        /// <summary>
        /// 获取设备
        /// </summary>
        /// <param name="index"> </param>
        /// <param name="dirName"> </param>
        public void GetSymbol(int index, string dirName)
        {
            var path = Path.Combine(DBEntityFinder.Instance.SymbolLibraryPath, dirName);
            GetSymbolLibrary(index, path, DBEntityFinder.Instance.SymbolPicturePath);
        }

        /// <summary>
        /// 添加符号
        /// </summary>
        /// <param name="index"> </param>
        /// <param name="symbolPath"> 符号路径</param>
        /// <param name="imagePath">图片路径</param>
        void GetSymbolLibrary(int index, string symbolPath, string imagePath)
        {
            if (IsCustomGroup())
            {
                LoadSymbolImg(string.Format("{0}\\{1}", imagePath, "自定义模版图片"));
                LoadCustomSymbol(symbolPath);
            }
            else
            {
                LoadSymbolImg(imagePath);
                LoadLineSymbol(index);
                LoadSymbolFile(symbolPath);
            }
        }
        /// <summary>
        /// 加载符号图片
        /// </summary>
        /// <param name="imagePath">图片路径</param>
        private void LoadSymbolImg(string imagePath)
        {
            if (Directory.Exists(imagePath))
            {
                var imageDirInfo = new DirectoryInfo(imagePath);
                var imageFileInfo = imageDirInfo.GetFileSystemInfos("*.jpg");

                foreach (var image in imageFileInfo)
                {
                    using (var fs = new FileStream(image.FullName, FileMode.Open))
                    {
                        var ig = Image.FromStream(fs, true, true);
                        imageList1.Images.Add(image.Name, ig);
                    }
                }
            }
            else
            {
                PublicMethod.Instance.Editor.WriteMessage("\n符号图片路径不存在！");
            }
        }

        /// <summary>
        /// 加载设备
        /// </summary>
        /// <param name="symbolPath">设备符号路径</param>
        private void LoadSymbolFile(string symbolPath)
        {
            if (!Directory.Exists(symbolPath))
            {
                PublicMethod.Instance.Editor.WriteMessage("\n符号库路径不存在！");
            }
            else
            {
                var oldName = string.Empty;
                var dinfo = new DirectoryInfo(symbolPath);
                var fileinfo = dinfo.GetFileSystemInfos("*.dwg");
                foreach (var i in fileinfo)
                {
                    var name = Path.GetFileNameWithoutExtension(i.FullName);
                    if (!IsCustomGroup())
                    {
                        var fIndex = i.Name.IndexOf("_", StringComparison.Ordinal);
                        var eIndex = i.Name.LastIndexOf("_", StringComparison.Ordinal);
                        var fno = name.Substring(0, fIndex);
                        name = name.Substring(eIndex + 1, name.Length - eIndex - 1);
                        if (fno != oldName)
                        {
                            listView1.Items.Add(i.FullName, name, Path.ChangeExtension(i.Name, ".jpg"));
                            oldName = fno;
                        }
                    }
                }
            }
        }

        /// <summary>
        /// 自定义符号集
        /// </summary>
        /// <param name="symbolPath"></param>
        private void LoadCustomSymbol(string symbolPath)
        {
            if (!Directory.Exists(symbolPath))
                Directory.CreateDirectory(symbolPath);
            var dinfo = new DirectoryInfo(symbolPath);
            var fileinfo = dinfo.GetFileSystemInfos("*.xml");

            foreach (var i in fileinfo)
            {
                var name = Path.GetFileNameWithoutExtension(i.FullName);
                //自定义符号
                listView1.Items.Add(i.FullName, name, Path.ChangeExtension(i.Name, ".jpg"));
                ClearPropertyGrid();
            }
        }
        /// <summary>
        /// 加载线
        /// </summary>
        private void LoadLineSymbol(int index)
        {
            if (index == 0)
            {
                SetListView(MapConfig.Instance.ListZyLineSymbol);
                SetListView(MapConfig.Instance.ListZyPolygonSymbol);
            }
            else if (index == 1)
            {
                SetListView(MapConfig.Instance.ListDyLineSymbol);
                SetListView(MapConfig.Instance.ListDyPolygonSymbol);
            }
        }
        private void SetListView(IEnumerable<string> list)
        {
            if (list != null)
            {
                foreach (var ls in list)
                {
                    var index = ls.LastIndexOf("_", StringComparison.Ordinal);
                    if (index > 0)
                    {
                        var name = ls.Substring(index + 1, ls.Length - index - 1);
                        listView1.Items.Add(ls, name, string.Format(@"{0}.jpg", ls));
                    }
                }
            }
        }

        /// <summary>
        /// 清空控件值
        /// </summary>
        private void ClearPropertyGrid()
        {
            if (listView1.SelectedItems.Count > 0)
                listView1.SelectedItems[0].ForeColor = Color.Empty;
            DevPropertyGrid.Text = string.Empty;
            DevNameGroup.Text = "属性";
            isclickDev = false;
            CurrentFno= oldFno = 0;
            DevPropertyGrid.SelectedObject = null;
            CurPropertyObj = null;
            DevPropertyGrid.Refresh();
        }

       
        private void listView1_MouseDoubleClick(object sender, MouseEventArgs e)
        {
            try
            {
                if (e.Button == MouseButtons.Right)
                    return;
                if (!CadDataSource._isLoadDataSource)
                {
                    PublicMethod.Instance.ShowMessage("请务必先加载数据源！！！");
                    return;
                }
                if (!isInsertSymbol)
                {
                    //判断是否有其他命令在执行
                    if (PublicMethod.Instance.IsExecuteSystemCMD("")) return;
                    var index = listView1.SelectedItems[0].Index;
                    //获取符号路径
                    var symbolPath = listView1.Items[index].SubItems[0].Name;
                    //获取块名称
                    var blockName = Path.GetFileNameWithoutExtension(symbolPath);
                    //设置符号库面板焦点
                    ViewHelper.palette.KeepFocus = false;
                    //新增符号
                    AddSymbol(symbolPath, blockName);
                }
                else
                {
                    MessageBox.Show("命令未结束！", "系统提示", MessageBoxButtons.OK, MessageBoxIcon.Stop);
                }
            }
            catch (Exception ex)
            {
                LogManager.Instance.Error(ex);
            }
            finally
            {
                ViewHelper.palette.KeepFocus = true;
                if (!IsCustomGroup())
                {
                    //只有插入了符号才会清空原来的值
                    GenerateHelper.EmptyFidRecord();
                    DBEntityFinder.Instance.EmptyDeviceAttribute(CurPropertyObj);
                }
                DevPropertyGrid.Refresh();
            }
        }
        /// <summary>
        /// 新增符号
        /// </summary>
        /// <param name="symbolPath">符号路径</param>
        /// <param name="blockName">块名称</param>
        private void AddSymbol(string symbolPath,string blockName)
        {
            try
            {
                isInsertSymbol = true;
                //单个设备新增
                if (!IsCustomGroup())
                {
                    //特殊符号处理
                    var sdf = new SpecilaDevField
                                  {
                                      Ssbyq = GenerateHelper.Unrf.byqFid,
                                      Ssgt = GenerateHelper.Unrf.gtFid,
                                      Sstj = GenerateHelper.Unrf.tjFid,
                                      Sskgg = GenerateHelper.Unrf.kggFid,
                                      Ssdf = GenerateHelper.Unrf.dfFid,
                                      Sstqhtj = GenerateHelper.Unrf.tqtjFid,
                                      SsZx = GenerateHelper.Unrf.zxFid
                                  };
                    //记忆公共属性
                    AddDevToDict(CurrentFno, CurPropertyObj);
                    //实体存在的点符号
                    DCadApi.InsertSymbol(File.Exists(symbolPath) ? blockName : symbolPath, CurPropertyObj, sdf);
                }
                else
                {
                    //插入自定义符号
                    if (File.Exists(symbolPath))
                    {
                        CustomSymbol.LoadSymbolSetXMl(symbolPath, blockName);
                    }
                }
                isInsertSymbol = false;
            }catch
            {
                isInsertSymbol = false;
            }
        }
        /// <summary>
        /// 自定义符号状态
        /// </summary>
        /// <returns></returns>
        private bool IsCustomGroup()
        {
            bool reval = _ButtonName == "Button4";
            return reval;
        }
        /// <summary>
        /// 保存之前插入地图的设置必填属性值
        /// </summary>
        /// <param name="fno"></param>
        /// <param name="obj"></param>
        private void AddDevToDict(int fno, XProps obj)
        {
            if (DBEntityFinder.Instance.MemoryDevice.Count <= 0)
            {
                DBEntityFinder.Instance.MemoryDevice.Add(fno, obj);
            }
            if (!DBEntityFinder.Instance.MemoryDevice.ContainsKey(fno))
            {
                DBEntityFinder.Instance.MemoryDevice.Add(fno, obj);
            }
            else
            {
                DBEntityFinder.Instance.MemoryDevice[fno] = obj;
            }
        }
        /// <summary>
        /// 鼠标单击图元面板图元
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void listView1_Click(object sender, EventArgs e)
        {
            DevPropertyGrid.Enabled = CadDataSource._isLoadDataSource;
            Tab_gnwz.SelectedIndex = 0;
          
            if (listView1.SelectedItems.Count <= 0) return;
            var index = listView1.SelectedItems[0].Index;
            if (index == -1) return;
            var blockPath = listView1.Items[index].Name;
            blockPath = Path.GetFileNameWithoutExtension(blockPath);
            isclickDev = false;
            //自定义符号组
            if (!IsCustomGroup())
            {
                if (blockPath.Contains("_"))
                {
                    var idx = blockPath.IndexOf("_", StringComparison.Ordinal);
                    if (idx > 0)
                    {
                        CurrentFno = int.Parse(blockPath.Substring(0, idx));
                      
                    }
                }
                if (CurrentFno != oldFno)
                {
                    GenerateHelper.EmptyFidRecord();//防止在没有插入之前点击的设备存入的fid值，一旦有新的fno就清空fid
                    var newObj = GenerateObj.Instance.GenderObjByFno(CurrentFno);
                    if (newObj == null)
                    {
                        MessageBox.Show("新增设备正在完善中...");
                        return;
                    }
                    GenerateHelper.SetAllBrowsable(newObj,true);
                    //设置记忆值
                    newObj = DBEntityFinder.Instance.GetMemoryDevice(CurrentFno, newObj);

                    GenerateHelper.GenerateXPropsByAzwz(newObj, CurrentFno,true);
                   
                    AutoGenerationHelper.Instance.AddUITypeEditor(newObj,CurrentFno);
                    
                    DevPropertyGrid.SelectedObject = newObj;
                    CurPropertyObj = newObj;
                    DevPropertyGrid.Refresh();
                    DevPropertyGrid.Text = CurrentFno.ToString();
                    oldFno = CurrentFno;
                    var simpleSymbolConfigEntry = SimpleDBSymbolsConfig.Instance.DBSymbol.SingleOrDefault(o => o.G3E_FNO == CurrentFno);
                    if (simpleSymbolConfigEntry != null)
                        DevNameGroup.Text = simpleSymbolConfigEntry.OtherProperty.LayerName + "(*表示必填项)";
                }
                if (CurrentFno == 144)
                {
                    var sbmc = GenerateHelper.GetPropertyValue(CurPropertyObj, GenerateHelper.GetCommonTableNameByFno(CurrentFno), "SBMC");
                    if (sbmc == null || string.IsNullOrEmpty(sbmc.ToString().Trim()))
                    {
                        GenerateHelper.SetPropertyValue(CurPropertyObj, GenerateHelper.GetCommonTableNameByFno(CurrentFno), "SBMC", "站房引线");
                         DevPropertyGrid.Refresh();
                    }
                }
            }
        }
      
   
        #region 自定义设备操作
        private void ToolSM_Refresh_Click(object sender, EventArgs e)
        {

        }

        private void ToolSM_Delete_Click(object sender, EventArgs e)
        {
            try
            {
                var index = listView1.SelectedItems[0].Index;
                var modelName = listView1.Items[index].Text;
                if (MessageBox.Show(string.Format("确定要删除【{0}】自定义模版吗？", modelName),
                                    "删除提示",
                                    MessageBoxButtons.OKCancel, MessageBoxIcon.Question) == DialogResult.OK)
                {
                    var blockPath = listView1.Items[index].Name;
                    if (File.Exists(blockPath))
                    {
                        File.Delete(blockPath);
                        var imgPath = string.Format("{0}\\自定义模版图片\\{1}.jpg", MapConfig.Instance.SymbolPicturePath, modelName);
                        if (File.Exists(imgPath))
                        {
                            File.Delete(imgPath);
                        }
                        listView1.Items.RemoveAt(index);
                    }
                }
            }
            catch (Exception ex)
            {
                PublicMethod.Instance.ShowMessage(ex.Message);
            }
        }

        private void listView1_MouseClick(object sender, MouseEventArgs e)
        {
            if (listView1.Items.Count > 0)
            {
                foreach (ListViewItem ii in listView1.Items)
                {
                    ii.ForeColor = Color.FromArgb(0, 0, 0);
                }
                foreach (ListViewItem ii in listView1.SelectedItems)
                {
                    ii.ForeColor = Color.FromArgb(0, 255, 0);
                }
            }
            if (e.Button == MouseButtons.Right)
            {
                listView1.ContextMenuStrip = IsCustomGroup() ? contextMenuStrip1 : null;
            }
            else
            {
                listView1.ContextMenuStrip = null;
            }
        }
        private void listView1_MouseLeave(object sender, EventArgs e)
        {
            if (ps == null) return;
            ps.Close();
            ps = null;
        }

        private void listView1_ItemMouseHover(object sender, ListViewItemMouseHoverEventArgs e)
        {
            PreViewSymbol(e.Item.Name, false);
        }

        private void ToolSM_PreView_Click(object sender, EventArgs e)
        {
            if (listView1.SelectedItems.Count > 0)
            {
                var index = listView1.SelectedItems[0].Index;
                var blockPath = listView1.Items[index].Name;
                PreViewSymbol(blockPath, true);
            }
        }
        public void PreViewSymbol(string imgPath, bool bl)
        {
            try
            {
                var name = Path.GetFileNameWithoutExtension(imgPath);
                imgPath = string.Format("{0}\\自定义模版图片\\{1}.jpg", DBEntityFinder.Instance.SymbolPicturePath, name);
                if (File.Exists(imgPath))
                {
                    if (ps == null)
                    {
                        if (bl)
                        {
                            ps = new PreviewSymbol(imgPath);
                            Application.ShowModelessDialog(ps);
                        }
                    }
                    else
                    {
                        ps.pictureBox1.Load(imgPath);
                    }
                }
            }
            catch (Exception ex)
            {
                PublicMethod.Instance.ShowMessage(ex.Message);
            }
        }
        #endregion


        private void PanelControl_Resize(object sender, EventArgs e)
        {
            foreach (var btn in btnList)
            {
                btn.Width = panel1.Width;
            }
        }

        private void DevPropertyGrid_PropertyValueChanged(object s, PropertyValueChangedEventArgs e)
        {
            ConnectManager.CleanSelcol();
           CommonHelp.Instance.ChangeAttributeRunTime(CurPropertyObj, CurrentFno,new List<long>{CurrentFid}, 
               e.ChangedItem.PropertyDescriptor.Name, e.ChangedItem.Value!=null?e.ChangedItem.Value.ToString():null, e.OldValue);
            DevPropertyGrid.SelectedObject = CurPropertyObj;
            DevPropertyGrid.Refresh();
                //点击了设备但是没有点击图元面板才更新
            if (isclickDev)
            {
                try
                {
                    var rdt = DevEventHandler.GetDevTables(CurrentFno, CurrentFid);
                    cadDS.ObjectId tempObjId = CurrentObjId;
                    CommonHelp.Instance.UpdateAttribute(CurPropertyObj, ref tempObjId, rdt);
                    if (tempObjId != CurrentObjId)
                        CurrentObjId = tempObjId;
                }
                catch (UpdataArgumentException ee)
                {
                    CommonHelp.Instance.ShowErrorMsg();
                    if (e.ChangedItem.PropertyDescriptor != null)
                        GenerateHelper.SetPropertyValue(CurPropertyObj,ee.TableName, ee.FieldName, e.OldValue);
                }
                catch (Exception ex)
                {
                    LogManager.Instance.Error(ex.Message);
                }
                finally
                {
                    //RefPaletteSet();
                    EventTriggerFunc();
                    GenerateHelper.EmptyFidRecord();
                }
            }
        }
        private void EventTriggerFunc()
        {
            if (MenuControl.orderVerify == null) return;
            var owa = new orderWorkifyArgs
                          {
                              FID = CurrentFid,
                              FNO = CurrentFno,
                              ObjId=CurrentObjId
                          };
            if (PanelControlToWorkOrderVerify != null)
            {
                PanelControlToWorkOrderVerify(new object(), owa);
            }
        }
        /// <summary>
        /// 添加事件
        /// </summary>
        private void AddEventHandler()
        {
            ObjectContextMenu.FixEntityHandler -= mouse_FixEntityHandler;
            //DevAttribute.panelIsEnable -= DevAttribute_isEnable;
            CadDataSource.InitPGridInPanelControl -= CadDataSource_InitPGridInPanelControl;
            WorkOrderVerify.RefPanel -= WorkOrderVerify_RefPanel;
            SHBMap.shbMapToPanel -= SHBMap_shbMapToPanel;
            TJXG.TJXGToPanel -= SHBMap_shbMapToPanel;
            SyncTzDataToCAD.SyncTzToRefPanel -= SyncTzDataToCadOnSyncTzToRefPanel;
            ObjectContextMenu.FixEntityHandler += mouse_FixEntityHandler;
            //DevAttribute.panelIsEnable += DevAttribute_isEnable;
            CadDataSource.InitPGridInPanelControl += CadDataSource_InitPGridInPanelControl;
            WorkOrderVerify.RefPanel +=WorkOrderVerify_RefPanel;
            SHBMap.shbMapToPanel += SHBMap_shbMapToPanel;
            TJXG.TJXGToPanel += SHBMap_shbMapToPanel;
            SyncTzDataToCAD.SyncTzToRefPanel+= SyncTzDataToCadOnSyncTzToRefPanel;
        }

        private void SyncTzDataToCadOnSyncTzToRefPanel(object sender, EventArgs eventArgs)
        {
            var ow = (orderWorkifyArgs)eventArgs;
            if (ow != null)
            {
                CurrentFid = ow.FID;
                CurrentFno = ow.FNO;
                CurrentObjId = ow.ObjId;
                SelectDevAttribute(true);
            }
        }

        void SHBMap_shbMapToPanel(long fno, long fid)
        {
            CurrentFno = (int)fno;
            CurrentFid = fid;
            SelectDevAttribute(false);
        }

       
        #region 受电馈线
        /// <summary>
        /// 保存添加过的馈线名称
        /// </summary>
        private List<string> sdkxList;
        /// <summary>
        /// 获取受电馈线
        /// </summary>
        private void GetSDKXBEntity()
        {
            //查看设备是否有所属馈线
            var value = SimpleDBSymbolsConfig.Instance.DBSymbol.SingleOrDefault(o => o.G3E_FNO == CurrentFno);
            if (!string.IsNullOrEmpty(value.ComponentTable.Gg_Pd_Sdkx_Ac))
            {
                ShowTab();
                //绑定控件ComBox值
                BindBDZComBox();
                //绑定受电馈线数据
                BindSDKXD();
            }
            else
            {
                HideTab();
            }
        }
        /// <summary>
        /// 隐藏Tab页
        /// </summary>
        private void HideTab()
        {
            tabPage_SDKX.Parent = null;
        }
        /// <summary>
        /// 显示Tab页
        /// </summary>
        private void ShowTab()
        {
            tabPage_SDKX.Parent = Tab_gnwz;
        }
        /// <summary>
        /// 获取受电馈线数据
        /// </summary>
        private void BindSDKXD()
        {
            var dt = new DataTable();
            dt.Columns.Add("GDBDZ");
            dt.Columns.Add("KXH");
            dt.Columns.Add("G3E_ID");
            sdkxList = new List<string>();
            var sdkxAcByG3EFid = DBEntityFinder.Instance.GetSdkxAcByG3e_FID(CurrentFid);
            if (sdkxAcByG3EFid != null && sdkxAcByG3EFid.Any())
            {
                foreach (var sdkx in sdkxAcByG3EFid)
                {
                    var dr = dt.NewRow();
                    dr[0] = sdkx.GDBDZ;
                    dr[1] = sdkx.KXH;
                    dr[2] = sdkx.G3E_ID;
                    dt.Rows.Add(dr);
                    sdkxList.Add(sdkx.KXH);
                }
            }
            DGV_SDKX.ReadOnly = !isLTTID;
            DGV_SDKX.AutoGenerateColumns = false;
            DGV_SDKX.Columns[0].DataPropertyName = "GDBDZ";
            DGV_SDKX.Columns[1].DataPropertyName = "KXH";
            DGV_SDKX.Columns[2].DataPropertyName = "G3E_ID";
            DGV_SDKX.DataSource = dt;
            DGV_SDKX.Refresh();
        }

        IEnumerable<Cd_ssxl> sdkxs;
        /// <summary>
        /// 绑定变电站下拉列表框
        /// </summary>
        private void BindBDZComBox()
        {
            string gdj = string.Empty;
            var value = (XProps)DevPropertyGrid.SelectedObject;
            if (value != null)
            {
                var ssgdj = GenerateHelper.GetPropertyValue(value, GenerateHelper.GetCommonTableNameByFno(CurrentFno), ("CD_SSDW"));
                if (ssgdj != null && !string.IsNullOrEmpty(ssgdj.ToString()))
                    gdj = ssgdj.ToString();
            }
            sdkxs = !string.IsNullOrEmpty(gdj) ? DBEntityFinder.Instance.GetSDKXXGByGDJ(gdj) : DBEntityFinder.Instance.GetSDKXXG();

            if (sdkxs == null) return;
            var dt = new DataTable();
            dt.Columns.Add("GDBDZ");
            //获取变电站
            var gdbdzs = sdkxs.Select(o => o.SSBDZ).Distinct().ToList();
            foreach (var bdz in gdbdzs)
            {
                if (!string.IsNullOrEmpty(bdz))
                {
                    var dr = dt.NewRow();
                    dr[0] = bdz;
                    dt.Rows.Add(dr);
                }
            }
            GDBDZ.ValueMember = "GDBDZ";
            GDBDZ.DisplayMember = "GDBDZ";
            GDBDZ.DataSource = dt;

            BindKXHComBox(sdkxs);
        }

        /// <summary>
        /// 绑定馈线下拉框
        /// </summary>
        private void BindKXHComBox(IEnumerable<Cd_ssxl> sdkxs)
        {
            var kxdt = new DataTable();
            kxdt.Columns.Add("KXH");
            //获取变电站
            foreach (var kx in sdkxs.Select(o => o.NAME))
            {
                if (!string.IsNullOrEmpty(kx))
                {
                    var dr = kxdt.NewRow();
                    dr[0] = kx;
                    kxdt.Rows.Add(dr);
                }
            }
            KXH.ValueMember = "KXH";
            KXH.DisplayMember = "KXH";
            KXH.DataSource = kxdt;
        }
        /// <summary>
        /// 单元格点击事件
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void DGV_SDKX_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            //点击馈线单元格
            if (e.ColumnIndex == 1 && e.RowIndex != -1)
            {
                CellClickBindKXHComBox();
            }
        }

        /// <summary>
        /// 当前选择馈线名称
        /// </summary>
        private string currentRowKx;

        /// <summary>
        /// 单元格点击时根据所选变电站筛选对应的馈线
        /// </summary>
        private void CellClickBindKXHComBox()
        {
            //变电站
            var bdz = GetCurrentRowCell(0);
            currentRowKx = GetCurrentRowCell(1);

            if (!string.IsNullOrEmpty(bdz))
            {
                var dt = new DataTable();
                dt.Columns.Add("KXH");
                var kxs = sdkxs.Where(o => o.SSBDZ == bdz).Distinct();
                foreach (var sskx in kxs)
                {
                    if (!string.IsNullOrEmpty(sskx.NAME))
                    {
                        var dr = dt.NewRow();
                        dr[0] = sskx.NAME;
                        dt.Rows.Add(dr);
                    }
                }

                foreach (var item in sdkxList)
                {
                    string item1 = item;
                    //如果存在就不添加
                    var value = kxs.Where(o => o.NAME == item1);
                    if (!value.Any())
                    {
                        var dr = dt.NewRow();
                        dr[0] = item;
                        dt.Rows.Add(dr);
                    }
                }

                KXH.ValueMember = "KXH";
                KXH.DisplayMember = "KXH";
                KXH.DataSource = dt;
            }
        }
        /// <summary>
        /// 获取当前行的某个单元格值
        /// </summary>
        /// <param name="index"></param>
        /// <returns></returns>
        private string GetCurrentRowCell(int index)
        {
            var value = string.Empty;
            var objValue = DGV_SDKX.CurrentRow.Cells[index].Value;
            if (objValue != null)
                value = objValue.ToString().Trim();
            return value;
        }
        /// <summary>
        /// 行完成验证时发生
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void DGV_SDKX_RowValidated(object sender, DataGridViewCellEventArgs e)
        {
            if (e.RowIndex == -1) return;

            if (!isLTTID) return;
            //变电站
            var bdz = GetCurrentRowCell(0);
            //变电站
            var kx = GetCurrentRowCell(1);

            if (!string.IsNullOrEmpty(bdz) && !string.IsNullOrEmpty(kx))
            {
                if (DBEntityFinder.Instance.HasSSKX(bdz, kx))
                {
                    try
                    {
                        //获取G3e_id
                        var currentG3e_id = GetCurrentRowCell(2);
                        if (string.IsNullOrEmpty(currentG3e_id))
                        {
                            InsertSDKXAC(bdz, kx, e.RowIndex);
                        }
                        else
                        {
                            UpdateSDKXAC(currentG3e_id, bdz, kx);
                        }
                    }
                    catch{
                    }
                    finally
                    {
                        EventTriggerFunc();
                    }
                  
                }
                else
                {
                    //馈线重复，清空
                    DGV_SDKX.Rows[e.RowIndex].Cells[1].Value = "";
                    PublicMethod.Instance.ShowMessage("当前变电站没有该馈线！");
                }
            }
        }
        /// <summary>
        /// 添加受电馈线
        /// </summary>
        /// <param name="bdz">变电站</param>
        /// <param name="kx">馈线名称</param>
        /// <param name="rowIndex">当前行索引</param>
        private void InsertSDKXAC(string bdz,string kx,int rowIndex)
        {
            //查询受电馈线是否已添加
            var result = DBEntityFinder.Instance.HasSDKX(CurrentFid,bdz, kx);
            if (!result)
            {
                var common = DBEntityFinder.Instance.GetCommon_n(CurrentFid);
                var id = InsertDBEntity.InsertSDKX(common, bdz, kx);
                if (id > 0)
                {
                    DGV_SDKX.CurrentRow.Cells[2].Value = id;
                    AddSdkxList(kx);
                    #region 同步更新校验结果

                    if (MenuControl.orderVerify != null)
                    {
                        var ow = new orderWorkifyArgs
                        {
                            FID = CurrentFid,
                            FNO = CurrentFno
                        };
                        FromPanelMsg(new object(), ow);
                    }
                    #endregion
                }
                PublicMethod.Instance.ShowMessage(id > 0 ? "添加成功！" : "添加失败！");
            }
            else
            {
                //馈线重复，清空
                DGV_SDKX.Rows[rowIndex].Cells[1].Value = "";
                PublicMethod.Instance.ShowMessage("馈线重复！");
            }
        }
        /// <summary>
        /// 更新受电馈线
        /// </summary>
        /// <param name="currentG3e_id"></param>
        /// <param name="bdz"></param>
        /// <param name="kx"></param>
        private void UpdateSDKXAC(string currentG3e_id,string bdz,string kx)
        {
            //更新
            var lg3e_id = long.Parse(currentG3e_id);
            //判断是否有修改
            var result = DBEntityFinder.Instance.HasSDKX(CurrentFid, bdz, kx);
            if (!result)
            {
                var isUpdate = UpdateDBEntity.UpdateSDKX_AC(lg3e_id, bdz, kx);
                if (isUpdate) UpdateSdkxList(currentRowKx, kx);
                PublicMethod.Instance.ShowMessage(isUpdate ? "修改成功！" : "修改失败！");
            }
        }
        /// <summary>
        /// 外部数据分析或验证操作引发异常时发生
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void DGV_SDKX_DataError(object sender, DataGridViewDataErrorEventArgs e)
        {
            PublicMethod.Instance.ShowMessage(e.Exception.Message);
        }
        /// <summary>
        /// 单元格值发生更改时发生
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void DGV_SDKX_CellValueChanged(object sender, DataGridViewCellEventArgs e)
        {
            if (e.ColumnIndex == 0 && e.RowIndex != -1)
            {
                //清空所属馈线
                DGV_SDKX.Rows[e.RowIndex].Cells[1].Value = "";
            }
        }

        /// <summary>
        /// 用户从DataGridView控件中删除行时发生
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void DGV_SDKX_UserDeletingRow(object sender, DataGridViewRowCancelEventArgs e)
        {
            if (isLTTID)
            {
                var kx = e.Row.Cells[1].Value;
                var id = e.Row.Cells[2].Value;
                if (id != null && !string.IsNullOrEmpty(id.ToString()))
                {
                    var g3e_id = id.ToString();
                    var isDel = DeleteDBEntity.DeleteSDKX(long.Parse(g3e_id));

                    PublicMethod.Instance.ShowMessage(isDel ? "删除成功！" : "删除失败！"+string.Format("\t馈线名称：{0}", kx));
                    if (isDel)
                    {
                        RemoveSdkxList(kx.ToString());
                        EventTriggerFunc();
                    }
                }
            }
            else
            {
                PublicMethod.Instance.ShowMessage("该设备未锁定，不能编辑！");
                e.Cancel = true;
            }
        }

        private void AddSdkxList(string sskx)
        {
            if (!sdkxList.Contains(sskx))
                sdkxList.Add(sskx);
        }
        private void UpdateSdkxList(string oldkx,string newkx)
        {
            AddSdkxList(newkx);
            RemoveSdkxList(oldkx);
        }
        private void RemoveSdkxList(string sskx)
        {
            if (sdkxList.Contains(sskx))
            {
                sdkxList.Remove(sskx);
            }
        }

        #endregion

      
    }
}
