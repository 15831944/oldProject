using System;
using System.IO;
using System.Threading;
using System.Windows.Forms;
using ArxMap;
using ElectronTransferBll;
using ElectronTransferDal.Cad;
using ElectronTransferDal.Common;
using ElectronTransferDal.Query;
using ElectronTransferDal.XmlDal;
using ElectronTransferFramework;
using Autodesk.AutoCAD.DatabaseServices;
using ElectronTransferView.ContextMenuManager;
using Application = Autodesk.AutoCAD.ApplicationServices.Application;
using ElectronTransferView.Menu;
using ElectronTransferModel.Config;

namespace ElectronTransferView.DataManager
{
    public partial class CadDataSource : Form
    {
        public CadDataSource()
        {
            InitializeComponent();
        }
        private static int loaddataindex;
        /// <summary>
        /// 数据源客户端路径
        /// </summary>
        private string xmlClientPath { set; get; }
        /// <summary>
        /// 根目录
        /// </summary>
        private string directoryName { set; get; }
        //检测是否加载数据源
        public static bool _isLoadDataSource;
        /// <summary>
        /// 当前活动选项卡
        /// </summary>
        private static string CTAB;
        /// <summary>
        /// 初始化PanelControl面板的PropertyGrid;
        /// </summary>
        public static event EventHandler InitPGridInPanelControl;

        private readonly DataManagerHelper dataManagerHelper = new DataManagerHelper();


        #region 选择数据源
        private void Btn_Select_Click(object sender, EventArgs e)
        {
            var openFile = new OpenFileDialog {Filter = "xml文件|*.xml|" + SystemSetting.FileExtension};
            if (openFile.ShowDialog() == DialogResult.OK)
            {
                var finf = new FileInfo(openFile.FileName);
                xmlClientPath = openFile.FileName;
                directoryName = finf.DirectoryName;
                var dirPath = Path.Combine(finf.DirectoryName, "MapConfig.xml");

                //重置前保存原始值
                var map = MapConfig.Instance.BaseDir;
                var listLable = MapConfig.Instance.ListLabelShow;
                var tzServerPath = MapConfig.Instance.TZPacketPath;
                var noVerifySbmcFeature = MapConfig.Instance.NoVerifySbmcFeatures;
                var noVerifyConnectivityFeature = MapConfig.Instance.NoVerifyConnectivityFeature;
                var noVerifyOwnshipFeature = MapConfig.Instance.NoVerifyOwnshipFeature;
                var noVerifyTzFeature = MapConfig.Instance.NoVerifyTzFeature;
                var browsableMap = MapConfig.Instance.BrowsableMap;
                //重置配置文件
                if (File.Exists(dirPath))
                {
                    try
                    {
                        MapConfig.Instance = ElectronTransferFramework.Serialize.XmlSerializeUtils.Load<MapConfig>(dirPath, new Type[] { });
                        MapConfig.Instance.TZPacketPath = tzServerPath;
                        //客户端路径
                        MapConfig.Instance.ClientXmlPath = xmlClientPath;
                        MapConfig.Instance.BaseDir = map;
                        //需显示的标注
                        MapConfig.Instance.ListLabelShow = listLable;
                        MapConfig.Instance.labels = listLable.Split(',');
                        MapConfig.Instance.NoVerifySbmcFeatures = noVerifySbmcFeature;
                        MapConfig.Instance.NoVerifyConnectivityFeature = noVerifyConnectivityFeature;
                        MapConfig.Instance.NoVerifyOwnshipFeature = noVerifyOwnshipFeature;
                        MapConfig.Instance.NoVerifyTzFeature = noVerifyTzFeature;
                        MapConfig.Instance.BrowsableMap = browsableMap;
                    }
                    catch (Exception ex)
                    {
                        PublicMethod.Instance.Editor.WriteMessage(ex.Message);
                    }
                }
                Txt_FilePath.Text = openFile.FileName;
                Btn_Initialization.Enabled = true;
                ZoomProjectionArea();
            }
        }
        /// <summary>
        /// 定位到投影区域
        /// </summary>
        private void ZoomProjectionArea()
        {
            //判断是否有活动选项卡
            if (!string.IsNullOrEmpty(CTAB))
            {
                PublicMethod.Instance.SendCommend("zoom\n" + MapConfig.Instance.ProjectionMinX + "," +
                                                  MapConfig.Instance.ProjectionMinY + "\n" +
                                                  MapConfig.Instance.ProjectionMaxX +
                                                  "," + MapConfig.Instance.ProjectionMaxY + "\n");
            }
        }
        #endregion

        #region 加载沿布图

        private void Btn_Initialization_Click(object sender, EventArgs e)
        {
            ToolTipText("正在初始化环境,请稍候…");
            string file = Txt_FilePath.Text.Trim();
            if (!string.IsNullOrEmpty(file))
            {
                //获取活动选项卡
                CTAB = PublicMethod.Instance.GetSystemCMD("CTAB");
                if (string.IsNullOrEmpty(CTAB))
                {
                    Application.DocumentManager.Add("acadiso.dwt");
                }
                Btn_Initialization.Enabled = false;
                Btn_Select.Enabled = false;
                //初始化xml数据
                Initialization(file);
            }
        }

        private void OnDataLoadCompleted(object sender, EventArgs e)
        {
            ToolTipText("正在努力加载地图，请稍候…");
            //初始化样式表
            InitiaCDData();
            //初始地图数据
            InitiaDTData();
            if (loaddataindex > 0)
            {
                ClearDeviceWindow();
                RemoveAutocadDB();
                InitPGridInPanelControl(new object(), new EventArgs());
            }
            else
                PublicMethod.Instance.Undefine();
            loaddataindex++;
            //加载沿布图数据
            LoadGISLayout();
            _isLoadDataSource = true;
            Close();
        }
        /// <summary>
        /// 加载沿布图
        /// </summary>
        private void LoadGISLayout()
        {
            //加载地图
            var eManager = new MapManager();
            eManager.LoadMap();
            //加载
            LoadSymbolicLibrary();
            //拷贝台帐数据包
            CopyTzPacket();
            ToolTipText("正在努力加载沿布图，请稍候…");
            //加载设备
            CABLEManager.LoadLayout(true);

            LoadEvent();
            //全景视图定位
            PublicMethod.Instance.GetPanoramaView();
            StartTimer();
        }
        /// <summary>
        /// 加载符号库
        /// </summary>
        private void LoadSymbolicLibrary()
        {
            CABLEManager.ParcentHandler -= CABLEManager_ParcentHandler;
            CABLEManager.ParcentHandler += CABLEManager_ParcentHandler;

            var symbolLibraryPath = DBEntityFinder.Instance.SymbolLibraryPath;
            //加载块定义
            DCadApi.LoadBolckDefinition(symbolLibraryPath);
        }
        //添加事件
        private void LoadEvent()
        {
            //加载CAD事件
            mouse.LoadCADEvent();
            //加载自定义事件
            dataManagerHelper.RemoveContextEvent();
            dataManagerHelper.AddContextEvent();
        }
        void CABLEManager_ParcentHandler(object sender, EventArgs e)
        {
            var parcentArgs = e as ParcentArgs;
            var tipText = string.Format("正在努力加载沿布图,请稍候… {0}%", parcentArgs.Parcent);
            ToolTipText(tipText);
        }

        private void CopyTzPacket()
        {
            var thread = new Thread(WindowsServices.CopyTZPacket);
            thread.Start(xmlClientPath);
        }

        private void ToolTipText(string message)
        {
            Tool_Status.Text = message;
            Refresh();
        }
      
        private  void ClearDeviceWindow()
        {
            if (MenuControl.fp !=null)
            {
                MenuControl.fp.Close();
            }
            if (MenuControl.orderVerify!=null)
            {
                MenuControl.orderVerify.Close();
            }
            if (MenuControl.query!=null)
            {
                MenuControl.query.Close();
            }
            if (MenuControl.coordinateFix!=null)
            {
                MenuControl.coordinateFix.Close();
            }
            //清除记录过被刷新了拓扑的设备列表
            CommonHelp.Instance.RefTopologyFeature.Clear();
        }

        private void CadDataSource_Load(object sender, EventArgs e)
        {
            //获取当前活动的选项卡
            CTAB = Application.GetSystemVariable("CTAB").ToString();
        }


        /// <summary>
        /// 删除当前文档
        /// </summary>
        private void RemoveAutocadDB()
        {
            if (Application.DocumentManager.MdiActiveDocument != null)
            {
                Application.DocumentManager.MdiActiveDocument.CloseAndDiscard();
            }
            Application.DocumentManager.Add("acadiso.dwt");
            using (PublicMethod.Instance.acDoc.LockDocument())
            {
                //初始化数据
                DCadApi.isRefreshLayer = true;
                PublicMethod.Instance.isLayerHiddenStatus = false;
                CADTypes.CreateLineType();
                CADTypes.CreateStyle();
                DBSymbolFinder.Instance.Clear();
                DBSymbolLTTIDFinder.Instance.Clear();
                //SurfaceInteractive.ClearResource();
                mouse._selectedObjectIds = null;
                mouse.selectedEntityId = ObjectId.Null;
                mouse.ImpliedSelectionIds = null;
            }
        }


        /// <summary>
        /// 开启自动保存
        /// </summary>
        private void StartTimer()
        {
            var cadTimer = new CADTimer();
            cadTimer.StartTimer();
        }
        #endregion

        #region 初始化数据
        /// <summary>
        /// 加载数据源
        /// </summary>
        /// <param name="filePath"></param>
        private void Initialization(string filePath)
        {
            var xmldb = new XmlDBManager {FileName = filePath};
#if EnableLock
            xmldb.Password = XmlPassword.Instance.Password;
#endif
            DBManager.Set(xmldb);
            xmldb.InitializeAsync(OnDataLoadCompleted);
        }

        /// <summary>
        /// 初始化CD表
        /// </summary>
        private void InitiaCDData()
        {
            var cdFilePath = Path.Combine(directoryName, "CdConfig.xml");
            if (File.Exists(cdFilePath))
            {
                var xmlcddb = new XmlDBManager {FileName = cdFilePath};
#if EnableLock
            xmlcddb.Password = XmlPassword.Instance.Password;
#endif
                xmlcddb.Initialize();
                CDDBManager.Set(xmlcddb);
            }
            else
                LogManager.Instance.Error("缺失GIS样式数据");
        }
        /// <summary>
        /// 初始化地图数据
        /// </summary>
        private void InitiaDTData()
        {
            var dtFilePath = Path.Combine(directoryName, "DT_WZBZ.xml");
            if (File.Exists(dtFilePath))
            {
                var xmldtdb = new XmlDBManager {FileName = dtFilePath};
#if EnableLock
            xmldtdb.Password = XmlPassword.Instance.Password;
#endif
                DTDBManager.Set(xmldtdb);
                xmldtdb.InitializeAsync(DtEventHandle);
            }
            else
                LogManager.Instance.Error("缺失地图数据！");
        }
        #endregion

        private void DtEventHandle(object sender,EventArgs eargs){}
    }
}
