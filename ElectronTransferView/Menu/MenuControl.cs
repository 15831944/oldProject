using System;
using System.Windows.Forms;
using ArxMap;
using Autodesk.AutoCAD.DatabaseServices;
using ElectronTransferBll;
using ElectronTransferDal.Cad;
using ElectronTransferDal.Common;
using ElectronTransferDal.Query;
using ElectronTransferDal.QueryVerifyHelper;
using ElectronTransferFramework;
using ElectronTransferModel.Config;
using ElectronTransferView.ConnectivityManager;
using ElectronTransferView.ContextMenuManager;
using ElectronTransferView.DataManager;
using ElectronTransferView.FunctionManager;
using ElectronTransferView.SearchManager;
using ElectronTransferView.TopologyManager;
using ElectronTransferView.VerifyManager;
using ElectronTransferView.ViewManager;
using Application = Autodesk.AutoCAD.ApplicationServices.Application;
using ElectronTransferDal.XmlDal;
using Autodesk.AutoCAD.EditorInput;
using ElectronTransferView.SyncTZDataToGIS;

namespace ElectronTransferView.Menu
{
    public partial class MenuControl : UserControl
    {
        public MenuControl()
        {
            InitializeComponent();
        }

        public static ToolStripMenuItem tsmi;
        /// <summary>
        /// 坐标
        /// </summary>
        public static CoordinateForm coordinateForm;

        /*图层管理*/
        private void ToolSM_LayerManage_Click_1(object sender, EventArgs e)
        {
            tsmi = sender as ToolStripMenuItem;
            if (tsmi.Checked)
            {
                tsmi.Checked = false;
                ViewHelper.RemoveLayerManager();
            }
            else
            {
                tsmi.Checked = true;
                ViewHelper.AddLayerPanel();
            }
        }
        /*符号库管理*/
        private void ToolSM_SymbolLibrary_Click(object sender, EventArgs e)
        {

            var toolStripMenuItem = sender as ToolStripMenuItem;
            if (toolStripMenuItem.Checked)
            {
                toolStripMenuItem.Checked = false;
                ViewHelper.RemoveSymbolPanel();
            }
            else
            {
                toolStripMenuItem.Checked = true;
                ViewHelper.AddSymbolPanel();
            }
        }
        /*加载数据源*/
        private void LoadDataSource_Click(object sender, EventArgs e)
        {
            var cds = new CadDataSource();
            Application.ShowModelessDialog(cds);

        }
        /*提交*/
        private void ToolSM_Commit_Click(object sender, EventArgs e)
        {
            try
            {
                if (IsLoadDataSource())
                {
                    if (MessageBox.Show("确定要保存数据吗？", "保存提示", MessageBoxButtons.OKCancel, MessageBoxIcon.Question) ==
                        DialogResult.OK)
                    {
                        Command.Save();
                    }
                }
            }
            catch (Exception ex)
            {
                LogManager.Instance.Error(ex);
            }
        }


        public static Query query;

        public static FixedPosition fp;

       

        private void TSM_CustomSymbol_Click(object sender, EventArgs e)
        {
            if (!IsLoadDataSource())
                return;
            if (!PublicMethod.Instance.IsExecuteSystemCMD(""))
            {
                ViewHelper.HideConnectPsState();
                var css = new CustomSymbolSet();
                Application.ShowModalDialog(css);
            }
        }
        /*新增支线*/
        private void TSM_AddLateral_Click_1(object sender, EventArgs e)
        {
            using (PublicMethod.Instance.acDoc.LockDocument())
            {
                if (!PublicMethod.Instance.IsExecuteSystemCMD(""))
                {
                    if (!IsLoadDataSource())
                        return;
                    var form = new AddLateral();
                    Application.ShowModalDialog(form);
                }
            }
        }


        public static CoordinateFix coordinateFix;
        public static WorkOrderVerify orderVerify;

        private void 校验ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (!IsLoadDataSource())
                return;
            if (!PublicMethod.Instance.IsExecuteSystemCMD(""))
            {
                if (orderVerify == null)
                {
                    orderVerify = new WorkOrderVerify();
                    
                }
                Application.ShowModelessDialog(orderVerify);
                orderVerify.WindowState = FormWindowState.Normal;
                orderVerify.Visible = true;
            }
        }

        /// <summary>
        /// 显示一个fid 为0 的连接面板
        /// </summary>
        public static void showConnectManager()
        {
            var ee = new selectEntityArgs();
            var g3eObject = new G3EObject();
            var objid = ObjectId.Null;
            var psr = PublicMethod.Instance.Editor.SelectImplied();
            if (psr.Status == PromptStatus.OK)
            {
                objid = psr.Value.GetObjectIds()[0];
                DBEntityFinder.Instance.GetG3EIds(objid, ref g3eObject);
            }
            ee.g3eObject = g3eObject;
            ee.objId = objid;
            ee.badd = true; //新建or更新面板
            ViewHelper.AddOrUpdateConnectPanel(ee);
        }

        public static void showConnectManager(long g3eFid)
        {
            try
            {
                var ee = new selectEntityArgs();
                var objID = DBEntityFinder.Instance.GetObjectIdByFid(g3eFid);
                if (!objID.IsNull)
                {
                    var g3eObject = new G3EObject();
                    DBEntityFinder.Instance.GetG3EIds(objID, ref g3eObject);
                    ee.g3eObject = g3eObject;
                    ee.objId = objID;
                    ee.badd = true;
                    ViewHelper.AddOrUpdateConnectPanel(ee);
                }

            }
            catch (Exception ex)
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

        private void 下载数据ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            using (PublicMethod.Instance.acDoc.LockDocument())
            {
                if (!PublicMethod.Instance.IsExecuteSystemCMD(""))
                {
                    if (!IsLoadDataSource())
                        return;
                    const string downStr = @"http://192.168.30.38:8081/emmis/equipGisMappingTemp/cadRestful/downDataFromOracleToSqlite.gis?gisJobId=720336&tempTableName=tzcad_20131129A&downDataFilePath=ftp://192.168.30.232/cadftp/export/20131129A/";
                    System.Diagnostics.Process.Start(downStr);

                }
            }
        }

        private void 上传数据ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            using (PublicMethod.Instance.acDoc.LockDocument())
            {
                if (!PublicMethod.Instance.IsExecuteSystemCMD(""))
                {
                    if (!IsLoadDataSource())
                        return;
                    const string downStr = @"http://192.168.30.38:8081/emmis/equipGisMappingTemp/cadRestful/uploadDataFromSqliteToOracle.gis?gisJobId=720336&updoadDataFilePath=ftp://192.168.30.232/cadftp/export/20131129A/";
                    System.Diagnostics.Process.Start(downStr);
                }
            }
        }

        /*选项*/
        private void TSMI_Option_Click(object sender, EventArgs e)
        {
            var option = new Option();
            Application.ShowModalDialog(option);
        }

        private void LoadFixMapToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (!PublicMethod.Instance.IsExecuteSystemCMD(""))
            {
                DCadApi.LoadFixMap();
            }
        }

        private void ToolSMI_Xyzz_Click(object sender, EventArgs e)
        {
            if (!IsLoadDataSource())
                return;
            if (!PublicMethod.Instance.IsExecuteSystemCMD(""))
            {
                FixEntity.Instance.ResetOldEntity();
                ConnectManager.CleanSelcol();
                TopologyTrace.TraceDown();
            }
        }

        private void ToolSMI_Syzz_Click(object sender, EventArgs e)
        {
            if (!IsLoadDataSource())
                return;
            if (!PublicMethod.Instance.IsExecuteSystemCMD(""))
            {
                FixEntity.Instance.ResetOldEntity();
                ConnectManager.CleanSelcol();
                TopologyTrace.TraceUp();
            }
        }

        private void ToolSMI_Cls_Click(object sender, EventArgs e)
        {
            if (!IsLoadDataSource())
                return;
            if (!PublicMethod.Instance.IsExecuteSystemCMD(""))
            {
                FixEntity.Instance.ResetOldEntity();
                ConnectManager.CleanSelcol();
                TopologyTrace.CleanTrace();
            }
        }

        private void ToolSMI_Pldd_Click(object sender, EventArgs e)
        {
            if (!IsLoadDataSource())
                return;
            if (!PublicMethod.Instance.IsExecuteSystemCMD(""))
            {
                TopologyMethods.breakall();
            }
        }

        private void ToolSMI_Zdlj_Click(object sender, EventArgs e)
        {
            if (!PublicMethod.Instance.IsExecuteSystemCMD(""))
            {
                ConnectManager.CleanSelcol();
                var a = new AutoConnect();
                a.autoc();
            }
        }

        private void ToolSMI_Lxlj_Click(object sender, EventArgs e)
        {
            if (!IsLoadDataSource())
                return;
            if (!PublicMethod.Instance.IsExecuteSystemCMD(""))
            {
                ViewHelper.AddConnectBySelPanel();
            }
        }

        private void ToolSMI_Mxlj_Click(object sender, EventArgs e)
        {
            if (!IsLoadDataSource())
                return;
            if (!PublicMethod.Instance.IsExecuteSystemCMD(""))
            {
                var MxToYxDlg = new ConnectMxToYx();
                Application.ShowModelessDialog(MxToYxDlg);
            }
        }
        /*支线管理*/
        private void ToolSMI_Zxgl_Click(object sender, EventArgs e)
        {
            if (!IsLoadDataSource())
                return;
            var rm = new RegionalManagement();
            Application.ShowModelessDialog(rm);
        }
        /*馈线管理*/
        private void ToolSMI_Kxgl_Click(object sender, EventArgs e)
        {
            if (!IsLoadDataSource())
                return;
            var fm = new FeederManagement();
            Application.ShowModalDialog(fm);
        }
        /*受电馈线管理*/
        private void ToolSMI_Sdkxgl_Click(object sender, EventArgs e)
        {
            if (!IsLoadDataSource())
                return;
            var SDKXForm = new SDKXView();
            Application.ShowModelessDialog(SDKXForm);
        }
        /*工单锁定设备管理*/
        private void ToolSMI_Gdsd_Click(object sender, EventArgs e)
        {
            if (!IsLoadDataSource())
                return;
            var woroe = new WorkOrderRangeOfEntity();
            Application.ShowModelessDialog(woroe);
        }
        /*拓扑管理*/
        private void ToolSMI_Tpgl_Click(object sender, EventArgs e)
        {
            showConnectManager();
        }
        /*保存*/
        private void ToolSMI_Saves_Click(object sender, EventArgs e)
        {
#if EnableLock
            LockDaemon.Instance.CheckLock();
#endif
            if (!IsLoadDataSource())
                return;
            var sf = new SaveFileDialog {Filter = "*.xml|*.xml|" + SystemSetting.FileExtension};
            var dr = sf.ShowDialog();
            if (dr != DialogResult.Cancel)
            {
                try
                {
                    PublicMethod.Instance.ShowMessage("\n正在保存，请稍候...");
                    if (sf.FileName.ToLower().Contains(".et"))
                        (DBManager.Instance as XmlDBManager).Save(sf.FileName, XmlPassword.Instance.Password);
                    else
                        (DBManager.Instance as XmlDBManager).Save(sf.FileName);

                    PublicMethod.Instance.ShowMessage("保存成功！");
                }catch(Exception ex )
                {
                    LogManager.Instance.Error(ex);
                    PublicMethod.Instance.ShowMessage("保存失败！");
                }
            }
        }
        /*导出增量*/
        private string fileName;
        private void ToolSMI_ExportZl_Click(object sender, EventArgs e)
        {
#if EnableLock
            LockDaemon.Instance.CheckLock();
#endif
            if (!IsLoadDataSource())
                return;

            #region 检查增量数据是否完整

            if (orderVerify == null)
            {
                orderVerify = new WorkOrderVerify();
            }
            var verifyRes = orderVerify.VerifySucceedOrFailed();
            if (verifyRes.Contains(VerifyRes.AttributeDefect) || verifyRes.Contains(VerifyRes.TopologyDefect))
            {
                MessageBox.Show("导出增量包时发现数据缺失严重，禁止导出！！！", "CAD提示", MessageBoxButtons.OK,MessageBoxIcon.Warning);
                Application.ShowModelessDialog(orderVerify);
                orderVerify.WindowState = FormWindowState.Normal;
                orderVerify.Visible = true;
                return;
            }
            if (verifyRes.Count > 0)
            {
                if (verifyRes.Contains(VerifyRes.NoSyncTzData))
                {
                    DialogResult dr = MessageBox.Show("功能位置属性部分字段值需要从台账同步,点击（工具->同步台账数据）", "CAD提示", MessageBoxButtons.YesNo);
                    if (dr == DialogResult.Yes)
                        return;
                }
                DialogResult res = MessageBox.Show("增量包含有不完整数据，是否继续导出.", "CAD提示", MessageBoxButtons.YesNo);
                if (res == DialogResult.No)
                {
                    Application.ShowModelessDialog(orderVerify);
                    orderVerify.WindowState = FormWindowState.Normal;
                    orderVerify.Visible = true;
                    return;
                }
            }
            orderVerify.Close();
            orderVerify = null;
            #endregion

            var sf = new SaveFileDialog {Filter = "*.xml|*.xml"};
            string name;
            var time =DateTime.Now.ToString("ddHHmmss");
            var kxmc = MapConfig.Instance.KXMC.Split(',');
            //按范围导出的时候没有馈线名称
            if ((kxmc.Length==1 && string.IsNullOrEmpty(kxmc[0])) || kxmc.Length>1)
                name = string.Format("{0}_增量{1}", MapConfig.Instance.GCID, time);
            else
                name = string.Format("{0}_{1}_增量{2}", MapConfig.Instance.KXMC, MapConfig.Instance.GCID, time);

            sf.FileName = string.Format("{0}.xml", name);
            if (sf.ShowDialog()==DialogResult.OK)
            {
                PublicMethod.Instance.ShowMessage("\n正在努力导出，请稍候...");
                //导出CAD增量包
                if ((DBManager.Instance as XmlDBManager).SaveVariable(sf.FileName))
                {
                    //拷贝台帐增量包
                    WindowsServices.ExportIncrementPacket(sf.FileName, name);
                    fileName = sf.FileName;
                    PublicMethod.Instance.ShowMessage("导出成功！");
                }
            }
        }

        private void ToolSMI_Search_Click(object sender, EventArgs e)
        {
            Refresh();
            itemFixed.Enabled = fp == null;
            ToolSMI_Coordinate.Enabled = coordinateFix == null;
            Refresh();
        }

        private void itemFixed_Click(object sender, EventArgs e)
        {
            if (!IsLoadDataSource())
                return;
            if (!PublicMethod.Instance.IsExecuteSystemCMD(""))
            {
                if (fp == null)
                {
                    fp = new FixedPosition {WindowState = FormWindowState.Normal, Visible = true};
                    Application.ShowModelessDialog(fp);
                }
                else
                {
                    fp.Visible = true;
                    fp.WindowState = FormWindowState.Normal;
                    fp.Activate();
                }
            }
        }

        private void ToolSMI_Query_Click(object sender, EventArgs e)
        {
            if (!IsLoadDataSource())
                return;
            if (!PublicMethod.Instance.IsExecuteSystemCMD(""))
            {
                if (query == null)
                {
                    query = new Query();
                }
                else
                {
                    query.Close();
                    query = new Query();
                }
                query.setpanel(1);
                Application.ShowModelessDialog(query);
            }
        }

        private void ToolSMI_FIDQuery_Click(object sender, EventArgs e)
        {
            if (!IsLoadDataSource())
                return;
            if (!PublicMethod.Instance.IsExecuteSystemCMD(""))
            {
                if (query == null)
                {
                    query = new Query();
                }
                else
                {
                    query.Close();
                    query = new Query();
                }
                query.setpanel(0);
                Application.ShowModelessDialog(query);
            }
        }

        private void ToolSMI_Coordinate_Click(object sender, EventArgs e)
        {
            if (!IsLoadDataSource())
                return;
            if (!PublicMethod.Instance.IsExecuteSystemCMD(""))
            {
                if (coordinateFix == null)
                {
                    coordinateFix = new CoordinateFix {WindowState = FormWindowState.Normal};
                    Application.ShowModelessDialog(coordinateFix);
                }
                else
                {
                    coordinateFix.WindowState = FormWindowState.Normal;
                    coordinateFix.Activate();
                }
            }
        }
        /// <summary>
        /// 同步台账数据
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void SyncTZDataSToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (!IsLoadDataSource())
                return;
            if (!PublicMethod.Instance.IsExecuteSystemCMD(""))
            {
                ViewHelper.LoadSyncTzManager();
            }
        }

        private void P2PTraceItem_Click(object sender, EventArgs e)
        {
            if (!IsLoadDataSource())
                return;
            if (!PublicMethod.Instance.IsExecuteSystemCMD(""))
            {
                FixEntity.Instance.ResetOldEntity();
                ConnectManager.CleanSelcol();
                TopologyTrace.TraceP2P();
            }
        }

        private void AllTraceItem_Click(object sender, EventArgs e)
        {
            if (!IsLoadDataSource())
                return;
            if (!PublicMethod.Instance.IsExecuteSystemCMD(""))
            {
                FixEntity.Instance.ResetOldEntity();
                ConnectManager.CleanSelcol();
                TopologyTrace.TraceAll();
            }
        }

        private void SetTraceItem_Click(object sender, EventArgs e)
        {
            if (!IsLoadDataSource())
                return;
            if (!PublicMethod.Instance.IsExecuteSystemCMD(""))
            {
                var tt = new SetColor();
                if (Application.ShowModalDialog(tt) == DialogResult.OK)
                {
                    PublicMethod.Instance.traceColor = tt.ccolor;
                }
            }
        }

        private void ToolSMI_UploadCADToGIS_Click(object sender, EventArgs e)
        {
            if (!PublicMethod.Instance.IsExecuteSystemCMD(""))
            {
                var upload = new Upload.UploadCAD(fileName);
                Application.ShowModalDialog(upload);
            }
        }

        private void ToolSMI_SHBSearch_Click(object sender, EventArgs e)
        {
            if (!IsLoadDataSource())
                return;
            if (!PublicMethod.Instance.IsExecuteSystemCMD(""))
            {
                var tt = new YHHSearch();
                Application.ShowModelessDialog(tt);
            }
        }
        /*坐标定位*/
        private void ToolSMI_ZB_Click(object sender, EventArgs e)
        {
            if (!IsLoadDataSource())
                return;
            if (!PublicMethod.Instance.IsExecuteSystemCMD(""))
            {
                coordinateForm = new CoordinateForm();
                Application.ShowModelessDialog(coordinateForm);
            }
        }

        /// <summary>
        /// 电房名称级联修改
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void ToolSMI_DFJLXG_Click(object sender, EventArgs e)
        {
            if (!IsLoadDataSource())
                return;
            if (!PublicMethod.Instance.IsExecuteSystemCMD(""))
            {
                var dfjlxg = new DFMCJLXG();
                Application.ShowModelessDialog(dfjlxg);
            }
        }
        /// <summary>
        /// 台架修改
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void ToolSMI_TJXG_Click(object sender, EventArgs e)
        {
            if (!IsLoadDataSource())
                return;
            if (!PublicMethod.Instance.IsExecuteSystemCMD(""))
            {
                var tjxg = new TJXG();
                Application.ShowModelessDialog(tjxg);
            }
        }
        /*打开绘图管理工具条*/
        private void ToolSMI_DrawTool_Click(object sender, EventArgs e)
        {
            if (ViewHelper.DrawTool == null)
                ViewHelper.DrawManagerBar();
            else
                ViewHelper.DrawTool.Visible = true;
        }
        /*打开拓扑管理工具条*/
        private void ToolSMI_TopologyTool_Click(object sender, EventArgs e)
        {
            if (ViewHelper.TopologyTool == null)
                ViewHelper.TopologyManagerBar();
            else
                ViewHelper.TopologyTool.Visible = true;
        }
        /*测量工具*/
        private void MeasureItem_Click(object sender, EventArgs e)
        {
            if (!IsLoadDataSource())
                return;
            if (!PublicMethod.Instance.IsExecuteSystemCMD("zmc1"))
            {
                var tt = new Measure();
                Application.ShowModelessDialog(tt);
            }
        }
        /*属性复制*/
        private void TSMI_AttributeCopy_Click(object sender, EventArgs e)
        {
            if (!IsLoadDataSource())
                return;
            if (!PublicMethod.Instance.IsExecuteSystemCMD(""))
            {
                MenuHelper.GetAttributeCopy();
            }
        }

        private void TSMI_Topology_Click(object sender, EventArgs e)
        {
            if (!IsLoadDataSource())
                return;
            if (!PublicMethod.Instance.IsExecuteSystemCMD(""))
            {
                var tt = new TraceManager();
                Application.ShowModelessDialog(tt);
            }
        }
        /*增量合并*/
        private void ToolSMI_MergerXml_Click(object sender, EventArgs e)
        {
            if (!IsLoadDataSource())
                return;
            if (!PublicMethod.Instance.IsExecuteSystemCMD(""))
            {
                var mergerXml = new MergerIncrementXml();
                Application.ShowModalDialog(mergerXml);
            }
        }
        /*型号规格管理*/
        private void ToolSMI_CDManager_Click(object sender, EventArgs e)
        {
            if (!IsLoadDataSource())
                return;
            if (!PublicMethod.Instance.IsExecuteSystemCMD(""))
            {
                var xhgeManager = new XHGEManagerForm();
                Application.ShowModalDialog(xhgeManager);
            }
        }
    }
}
