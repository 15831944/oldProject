
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using ElectronTransferDal.Cad;
using ElectronTransferDal.Query;
using ElectronTransferDal.QueryVerifyHelper;
using ElectronTransferModel.Config;
using ElectronTransferModel;
using ElectronTransferDal;
using ElectronTransferDal.Common;
using ElectronTransferView.ConnectivityManager;
using ElectronTransferView.ContextMenuManager;
using ElectronTransferView.SearchManager;
using ElectronTransferView.ViewManager;
using V94 = ElectronTransferModel.V9_4;
using ElectronTransferFramework;
using ElectronTransferView.Menu;
using ArxMap;
using Autodesk.AutoCAD.DatabaseServices;
using ElectronTransferDal.AutoGeneration;
using ElectronTransferModel.V9_4;
using DotNetARX;


namespace ElectronTransferView.VerifyManager
{
    public partial class WorkOrderVerify : Form
    {
        [System.Runtime.InteropServices.DllImport("user32.dll", EntryPoint = "SetForegroundWindow")]
        public static extern bool SetForegroundWindow(IntPtr hWnd);
        public WorkOrderVerify()
        {
            InitializeComponent();
            DoubleBuffered = true;
            //强制窗体获取焦点
            SetForegroundWindow(Handle);
            //DevAttribute.workorderVerify -= DevAttribute_workorderVerify;
            ConnectManager.connectivityVerify -= ConnectManager_connectivityVerify;
            PanelControl.FromPanelMsg -= PanelControl_SdkxVerify;
            PanelControl.PanelControlToWorkOrderVerify -= PanelControl_PanelControlToWorkOrderVerify;
            BulkChanges.RefBulkModify -= BulkChanges_RefBulkModify;
            //DevAttribute.workorderVerify += DevAttribute_workorderVerify;
            ConnectManager.connectivityVerify += ConnectManager_connectivityVerify;
            PanelControl.FromPanelMsg += PanelControl_SdkxVerify;
            PanelControl.PanelControlToWorkOrderVerify += PanelControl_PanelControlToWorkOrderVerify;
            BulkChanges.RefBulkModify += BulkChanges_RefBulkModify;
        }

       
        private readonly string[] TzMsg =  { "台账连接失败", "未录入台账数据" ,"未同步台账数据"};
        private readonly string[] TopologyMsg = { " 连接关系数据错误 ", " 从属关系数据错误 ", " 包含关系数据错误 " };
        public static event EventHandler RefPanel;
        /// <summary>
        /// 记录被校验的设备
        /// </summary>
        private static List<VerifyClass> devList = new List<VerifyClass>();
        /// <summary>
        /// 记录属性数据
        /// </summary>
        private static List<VerifyClass> objList = new List<VerifyClass>();
        /// <summary>
        /// 记录拓扑数据
        /// </summary>
        private static List<VerifyClass> topology = new List<VerifyClass>();
        /// <summary>
        /// 列排序
        /// </summary>
        private ListViewColumnSorter lvcs = new ListViewColumnSorter();
        /// <summary>
        /// 储存要刷新连接关系的设备集合
        /// </summary>
        private List<long> SelectDevFid = new List<long>();
        /// <summary>
        /// 储存批量修改设备的fid
        /// </summary>
        private List<G3EObject> bulkModifyFid = new List<G3EObject>();
        private bool IsLoadData = false;
        private void WorkOrderVerify_Load(object sender, EventArgs e)
        {
            try
            {
                InitialzeAttribute();
                InitializeTopology();
                btExport.ImageIndex = 0;
                btExport.Enabled = false;
                btRef.Enabled = false;
                GetVerifyData();
                if (!IsLoadData)
                    LoadData();
                LoadListViewData();
                btExport.Enabled = true;
            }
            catch(Exception ex)
            {
                LogManager.Instance.Error(ex.Message);
            }
        }

        private void AddColumn(ListView lv)
        {
            lv.Items.Clear();
            lv.View = View.Details;
            lv.FullRowSelect = true;
            lv.GridLines = true;
            lv.LabelEdit = false;
            lv.AllowColumnReorder = false;
            lv.HideSelection = true;
            lv.Columns.Add("序号", 45, HorizontalAlignment.Right);
            lv.Columns.Add("G3E_FID", 70, HorizontalAlignment.Center);
            lv.Columns.Add("设备类型", 100, HorizontalAlignment.Center);
            lv.Columns.Add("所属变电站", 120, HorizontalAlignment.Center);
            lv.Columns.Add("受电馈线", 150, HorizontalAlignment.Center);
            lv.Columns.Add("设备名称", 150, HorizontalAlignment.Center);
            lv.Columns.Add("修改状态", 80, HorizontalAlignment.Center);
            lv.Columns.Add("校验结果",240, HorizontalAlignment.Center);
            lv.Visible = true;
            lv.ListViewItemSorter = lvcs;
            CommonHelp.Instance.SizeLastColumn(lv);
            ImageList imgList = new ImageList();
            imgList.ImageSize = new System.Drawing.Size(2, 18);
            lv.SmallImageList = imgList;
        }
        private void InitialzeAttribute()
        {
            AddColumn(LvAttribute);
        }
        private void InitializeTopology()
        {
            AddColumn(LvTopology);
        }
        public void GetVerifyData()
        {
            using (var manager = new ProgressManager("加载设备"))
            {
                if (devList.Any())
                    devList.Clear();
                //获取可能新增的设备
                var values = DBEntityFinder.Instance.GetG3EObjectsInLttID();
                manager.SetTotalOperations(values.Count());
                foreach (var entity in values)
                {
                    manager.Tick();
                    AddVerifyData(entity.G3E_FNO,entity.G3E_FID);
                }
                #region 优化前
                //foreach (var entity in values)
                //{
                //    manager.Tick();
                //    long g3e_fid = 0, g3e_id = 0, g3e_fno = 0;
                //    DBEntityFinder.Instance.GetG3EIds(entity.Key, ref g3e_id, ref g3e_fid, ref g3e_fno);
                //    AddVerifyData((int)g3e_fno, g3e_fid);
                //}
                ////获取当前工单范围内的所有设备,//只加198,199
                //var WorkOrderValues = DBSymbolLTTIDFinder.Instance.Where(o => (o.Value.G3e_Fno == 198 || o.Value.G3e_Fno == 199) && o.Value.LTT_ID == MapConfig.Instance.LTTID);
                //if (!WorkOrderValues.Any()) return;
                //foreach (var i in WorkOrderValues)
                //{
                //    var kggOrTj = DevEventHandler.GetDevTables(i.Value.G3e_Fno, i.Key);
                //    if (kggOrTj != null)
                //    {
                //        if (kggOrTj.ComObj != null && kggOrTj.ComObj.EntityState != EntityState.Delete)
                //        {
                //            AddVerifyData(i.Value.G3e_Fno, i.Key);
                //        }
                //    }
                //}
                #endregion
             
            }
        }
        public void LoadData()
        {
            if (devList.Any())
            {
                LoadAttributeData();
                LoadTopologyData();
            }
            IsLoadData = true;
        }

        private void LoadListViewData()
        {
            if (devList.Any())
            {
                AddAttributeToLvAttribute();
                AddTopologyToLvTopology();
            }
        }

        /// <summary>
        /// 属性校验数据
        /// </summary>
        public void LoadAttributeData()
        {
            if (objList.Count > 0)
            {
                objList.Clear();
            }
            #region 校验功能位置属性是否完整

            using (ProgressManager manager = new ProgressManager("加载属性数据"))
            {
                manager.SetTotalOperations(objList.Count);
                int index = 1;
                foreach (var item in devList)
                {
                    manager.Tick();
                    manager.SetTotalOperations(index);
                    StringBuilder sb = new StringBuilder();
                    try
                    {
                        var tables = DevEventHandler.GetDevTables(item.G3E_FNO, item.G3E_FID);
                        if (tables != null)
                        {
                            var s = GenerateHelper.FieldIsNull(tables, item.G3E_FID, item.G3E_FNO);
                            sb.AppendLine(s.Length > 0 ? s.ToString() : "");

                            if (sb.ToString().Trim().Length <= 0)
                            {
                                item.VerifyResult = "校验成功";
                            }
                            else
                            {
                                item.VerifyResult = sb.ToString();
                            }
                        }
                        item.BH = index;
                        objList.Add(item);
                    }
                    catch (Exception ex)
                    {
                        LogManager.Instance.Error(ex);
                    }
                    index++;
                }
            }
            #endregion
            VerifyTZ();
            
        }
        /// <summary>
        /// 台账校验，查看是否录入了台账
        /// </summary>
        private void VerifyTZ()
        {
            #region 校验台账是否同步
            // 集抄，引线，高压表
            List<string> noVerifyTzDev = new List<string>();
            if (!string.IsNullOrEmpty(MapConfig.Instance.NoVerifyTzFeature))
            {
                noVerifyTzDev.AddRange(MapConfig.Instance.NoVerifyTzFeature.Split(','));
            }
            if (objList.Any())
            {
                string waitTzVerifyData = string.Empty;
                foreach (var item in devList)
                {
                    if (noVerifyTzDev.Any(o =>o.Equals(item.G3E_FNO.ToString()))) continue;
                    #region 对导线类型是连接导线的设备过滤
                    if (item.G3E_FNO == 141||item.G3E_FNO==156)
                    {
                        var gnwz=DBManager.Instance.GetEntity<Gg_pd_gnwzmc_n>(o => o.G3E_FID == item.G3E_FID);
                        if (gnwz != null)
                        {
                            if (gnwz.GNWZ_XLFL!=null&&gnwz.GNWZ_XLFL.Equals("连接导线"))
                                continue;
                        }
                    }
                    #endregion
                    string str;
                    if (CommonHelp.Instance.IsInsertDevice(item.G3E_FNO, item.G3E_FID))
                    {
                        str = "a" + item.G3E_FID + ",";
                    }
                    else
                    {
                        str = item.G3E_FID + ",";
                    }
                    waitTzVerifyData += str;
                }
                using (ProgressManager manager = new ProgressManager("加载台账数据"))
                {
                    manager.SetTotalOperations(objList.Count);
                    if(!string.IsNullOrEmpty(waitTzVerifyData))
                        waitTzVerifyData = waitTzVerifyData.Substring(0, waitTzVerifyData.Length - 1);
                    var verifyRes = GenerateHelper.VerifyTzExists(waitTzVerifyData);
                    //下面的就是已经有台账的设备
                    var exceptRes = waitTzVerifyData.Split(',').Except(verifyRes.Split(','));
                    if (!string.IsNullOrEmpty(verifyRes))
                    {
                        if (!verifyRes.ToLower().Equals("ok"))
                        {
                            var verifyFailedFids = verifyRes.Split(',');
                            var flag = verifyFailedFids[0].Equals("fail");
                            foreach (var item in verifyFailedFids)
                            {
                                manager.Tick();
                                if (item.Equals("fail")) continue;
                                var fidTmp = item.Replace('a', ' ').Trim();
                                long tempFid;
                                if (long.TryParse(fidTmp, out tempFid))
                                {
                                    var dev = objList.SingleOrDefault(o => o.G3E_FID == tempFid);
                                    if (dev != null)
                                    {
                                        var ss = new StringBuilder(dev.VerifyResult);
                                        if (ss.ToString().Contains("校验成功"))
                                        {
                                            dev.VerifyResult = flag ? TzMsg[0] : TzMsg[1];
                                        }
                                        else
                                        {
                                            ss.AppendLine(flag ? TzMsg[0] : TzMsg[1]);
                                            dev.VerifyResult = ss.ToString();
                                        }
                                    }
                                }
                            }
                        }

                    }
                    if(exceptRes.Any())
                    {
                        //对应录入了台账的设备查看是否同步了台账
                        foreach (var item in exceptRes)
                        {
                            var fid = item.Replace('a',' ').Trim();
                            long Fid = 0;
                            if (long.TryParse(fid, out Fid))
                            {
                                var ent = objList.SingleOrDefault(o => o.G3E_FID == Fid);
                                if (ent == null) continue;
                                VerifySingleTZ(ref ent);
                            }
                        }
                    }
                }
            }
            #endregion
        }
        /// <summary>
        /// 校验台账是否同步
        /// </summary>
        /// <param name="fid"></param>
        /// <param name="fno"></param>
        private void VerifySingleTZ(ref VerifyClass ent)
        {
            if (ent == null) return;
            var obj = DevEventHandler.GetDevTables(ent.G3E_FNO, ent.G3E_FID);
            XProps temp = GenerateObj.Instance.GenderObjByFno(ent.G3E_FNO);
            try
            {
                foreach (var attr in temp)
                {
                    object value = GenerateHelper.GetAttributeValueByField(attr, obj);
                    if (attr.ReadOnly)///如果是只读的说明是要从台账同步的
                    {
                        if (value == null || string.IsNullOrEmpty(value.ToString().Trim()))
                        {
                            var ss = new StringBuilder(ent.VerifyResult);
                            if (ss.ToString().Contains("校验成功"))
                            {
                                ent.VerifyResult = TzMsg[2];
                            }
                            else
                            {
                                if (!ss.ToString().Contains(TzMsg[2]))
                                {
                                    ss.AppendLine(TzMsg[2]);
                                    ent.VerifyResult = ss.ToString();
                                }


                            }
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                LogManager.Instance.Error(ex.Message);
            }
        }
        private void AddColumnData(IEnumerable<VerifyClass> workorder,string progressName,ListView lv)
        {
            lv.Items.Clear();
            using (var manager = new ProgressManager(progressName))
            {
                manager.SetTotalOperations(objList.Count);
                foreach (var t in workorder)
                {
                    manager.Tick();
                    var item = new ListViewItem();
                    item.UseItemStyleForSubItems = false;
                    item.Tag = t;
                    item.SubItems[0].Text = t.BH.ToString(CultureInfo.InvariantCulture);
                    item.SubItems.Add(t.G3E_FID.ToString(CultureInfo.InvariantCulture));
                    item.SubItems.Add(t.DeviceType);
                    var ssbdz = string.IsNullOrEmpty(t.SSBDZ) ? "无" : t.SSBDZ;
                    item.SubItems.Add(ssbdz);
                    var ssxl = string.IsNullOrEmpty(t.SSXL) ? "无" : t.SSXL;
                    item.SubItems.Add(ssxl);
                    var sbmc = string.IsNullOrEmpty(t.SBMC) ? "无" : t.SBMC;
                    item.SubItems.Add(sbmc);
                    item.SubItems.Add(t.DevState);
                    item.SubItems.Add(t.VerifyResult);
                    if (!t.VerifyResult.Equals("校验成功"))
                    {
                        item.SubItems[7].ForeColor = System.Drawing.Color.Red;
                    }
                    
                    lv.Items.Add(item);
                }
                lv.Sort();
                CommonHelp.Instance.ReCreateSerialNumber(lv);
            }
        }
        /// <summary>
        /// 填充属性数据 
        /// </summary>
        private void  AddAttributeToLvAttribute()
        {
            AddColumnData(objList, "填充属性数据", LvAttribute);
        }

        private void AddTopologyToLvTopology()
        {
            AddColumnData(topology, "填充拓扑数据", LvTopology);
        }
        /// <summary>
        /// 拓扑校验数据
        /// </summary>
        private void LoadTopologyData()
        {
            if (topology.Count > 0)
            {
                topology.Clear();
            }
            int index = 1;
            foreach (var ent in objList)
            {
                if (ent.G3E_FNO == 198 || ent.G3E_FNO == 199) continue;
                VerifyClass vc = new VerifyClass
                {
                    BH=index,
                    G3E_FID = ent.G3E_FID,
                    DeviceType = ent.DeviceType,
                    SSBDZ=ent.SSBDZ,
                    SSXL=ent.SSXL,
                    SBMC = ent.SBMC,
                    DevState = ent.DevState,
                    VerifyResult = "校验失败",
                    G3E_FNO = ent.G3E_FNO
                };
                SingleDevTopologyVerify(vc);
                topology.Add(vc);
                index++;
            }
        }
        private void SingleDevTopologyVerify(VerifyClass vc)
        {
            if (vc == null) return;
            if (vc.DevState.Equals("已删除"))
            {
                vc.VerifyResult = " 该设备已被删除 ";
                return;
            }
            bool connectivity = false, ownership = false, contain = false;
            StringBuilder sb = new StringBuilder();
            //上面的都是一致的，只有校验结果是不一样的
            var verifyConnectivity = VerifyConnectivity(vc.G3E_FNO, vc.G3E_FID);
            if (verifyConnectivity.RF == ReturnFlag.VERIFYSUCCEED)
            {
                connectivity = true;
            }
            else
            {
                sb.AppendLine(verifyConnectivity.ErrMsg.ToString());
            }
            var verifyOwnShip = VerifyOwnShip(vc.G3E_FNO, vc.G3E_FID);
            if (verifyOwnShip.RF == ReturnFlag.VERIFYSUCCEED)
            {
                ownership = true;
            }
            else
            {
                sb.AppendLine(verifyOwnShip.ErrMsg.ToString());
            }
            var verifyContain = VerifyContain(vc.G3E_FNO, vc.G3E_FID);
            if (verifyContain.RF == ReturnFlag.VERIFYSUCCEED)
            {
                contain = true;
            }
            else
            {
                sb.AppendLine(verifyContain.ErrMsg.ToString());
            }
            if (connectivity & ownership & contain)
            {
                vc.VerifyResult = "校验成功";
            
            }
            else
            {
                vc.VerifyResult = sb.ToString();
            }
        }

        /// <summary>
        /// 添加校验数据l
        /// </summary>
        /// <param name="g3e_fno"></param>
        /// <param name="g3e_fid"></param>
        public void AddVerifyData(int g3e_fno, long g3e_fid)
        {
            try
            {
                var tables = DevEventHandler.GetDevTables(g3e_fno, g3e_fid);
                if (tables == null) return;
                //是否需要添加未编辑的设备
                //if (!IsModify(tables)) return;
                //更新的情况比较特殊，可能只更新一个表
                string sbmc = string.Empty;
                string ssbdz = string.Empty;
                string ssxl = string.Empty;
                if (tables.ComObj == null)
                {
                    if (g3e_fno == 159||g3e_fno==160)//集中抄表箱//散户表
                    {
                        if (tables.SelfObj != null )
                        {
                            if (tables.SelfObj.HasAttribute("HH")&&tables.SelfObj.GetAttribute("HH")!=null)
                            {
                                sbmc = tables.SelfObj.GetAttribute("HH");
                            }
                            if (tables.SelfObj.HasAttribute("CD_SSXL") && tables.SelfObj.GetAttribute("CD_SSXL") != null)
                            {
                                ssxl = tables.SelfObj.GetAttribute("CD_SSXL");
                            }
                        }
                    }
                    else if (g3e_fno == 188)//负控
                    {
                        if (tables.GnwzObj != null && tables.GnwzObj.HasAttribute("MC"))
                        {
                            sbmc = tables.GnwzObj.GetValue("MC").ToString();
                        }
                    }
                }
                else
                {
                    sbmc = tables.ComObj.SBMC;
                    ssbdz = tables.ComObj.CD_SSBDZ;
                    ssxl = tables.ComObj.CD_SSXL;
                }
                VerifyClass vc = new VerifyClass
                {
                    BH=0,
                    G3E_FID = g3e_fid,
                    DeviceType = PublicMethod.Instance.GetDeviceType(g3e_fno),
                    SSBDZ=ssbdz,
                    SSXL=ssxl,
                    SBMC = sbmc,
                    DevState = GetDevEntityState(tables),
                    VerifyResult = "校验失败",
                    G3E_FNO = g3e_fno
                };
                if (devList.Any())
                {
                    var value = devList.Where(o => o.G3E_FID == vc.G3E_FID);
                    if (!value.Any())
                        devList.Add(vc);
                }else
                    devList.Add(vc);
            }
            catch (Exception ex)
            {
                PublicMethod.Instance.ShowMessage(ex.Message);
                LogManager.Instance.Error(ex);
            }

        }

        /// <summary>
        /// 校验连接关系
        /// </summary>
        /// <param name="g3e_fno"></param>
        /// <param name="g3e_fid"></param>
        /// <returns></returns>
        public TopologyVerifyRes VerifyConnectivity(int g3e_fno, long g3e_fid)
        {
             //连接关系节点值配比表     
            //Node1    null   null   0    0       null    value   0       value    value
            //Node2    null   0      0    null    value   null    value   0        value
            TopologyVerifyRes topologyRes = new TopologyVerifyRes();
            topologyRes.ErrMsg = new StringBuilder();
            try
            {
                List<string> fnoList = new List<string>();
                if (!string.IsNullOrEmpty(MapConfig.Instance.NoVerifyConnectivityFeature))
                {
                    fnoList.AddRange(MapConfig.Instance.NoVerifyConnectivityFeature.Split(','));
                }
                if (fnoList.Any(o => o.Equals(g3e_fno.ToString())) || CommonHelp.Instance.RefTopologyFeature.Contains(g3e_fid))
                {
                    topologyRes.RF = ReturnFlag.VERIFYSUCCEED;
                    return topologyRes;
                }
                var value = CDDBManager.Instance.GetEntities<G3e_nodeedgeconnectivity>(o => o.G3E_SOURCEFNO == g3e_fno);
                //先查出当前设备的连接关系
                var connectivityCurrent = DBManager.Instance.GetEntity<Connectivity_n>(o => o.G3E_FID == g3e_fid);
                if (value == null || !value.Any()) //表示不支持建立连接关系
                {
                    //查询当前设备是否有连接关系
                    if (connectivityCurrent != null)
                    {
                        topologyRes.ErrMsg.AppendLine(" 该设备不该有连接关系 ");
                        topologyRes.RF = ReturnFlag.VERIFYFAILED;
                        return topologyRes; //该设备不该有连接关系
                    }
                    topologyRes.RF = ReturnFlag.VERIFYSUCCEED;
                    return topologyRes;

                } //到此说明该设备需要建立连接关系
                if (connectivityCurrent == null)
                {
                    topologyRes.ErrMsg.AppendLine(" 缺失连接关系 ");
                    topologyRes.RF = ReturnFlag.VERIFYFAILED;
                    return topologyRes;
                }
                if ((connectivityCurrent.NODE1_ID == null && connectivityCurrent.NODE2_ID == null) ||
                    (connectivityCurrent.NODE1_ID == 0 && connectivityCurrent.NODE2_ID == 0) ||
                    (connectivityCurrent.NODE1_ID == 0 && connectivityCurrent.NODE2_ID == null) ||
                    (connectivityCurrent.NODE1_ID == null && connectivityCurrent.NODE2_ID == 0))
                {
                    topologyRes.ErrMsg.AppendLine(" 连接关系缺失 ");
                    topologyRes.RF = ReturnFlag.VERIFYFAILED;
                    return topologyRes;
                }

                #region 设备两头校验连接关系

                if (PublicMethod.Instance.N1isN2.Contains(g3e_fno))
                {
                    #region   143 站房母线, 159 低压集中抄表箱  N1isN2 = new[] { 143, 159 };
                    topologyRes.RF = ReturnFlag.VERIFYSUCCEED;
                    return topologyRes;
                    #endregion
                    #region
                    //if (connectivityCurrent.NODE1_ID != null && connectivityCurrent.NODE2_ID != null)
                    //{
                    //    if (connectivityCurrent.NODE1_ID != connectivityCurrent.NODE2_ID)
                    //    {
                    //        topologyRes.ErrMsg.AppendLine(" 当前设备两头连接点不等 ");
                    //        topologyRes.RF = ReturnFlag.VERIFYFAILED;
                    //        return topologyRes;
                    //    }
                    //    topologyRes.RF = ReturnFlag.VERIFYSUCCEED;
                    //    return topologyRes;
                    //}
                    //if (connectivityCurrent.NODE1_ID == null || connectivityCurrent.NODE2_ID == null)
                    //{
                    //    topologyRes.ErrMsg.AppendLine(" 当前设备两头连接点不等 ");
                    //    topologyRes.RF = ReturnFlag.VERIFYFAILED;
                    //    return topologyRes;
                    //}
                    #endregion
                  
                }
                if (PublicMethod.Instance.N2is0.Contains(g3e_fno))
                {
                    #region
                    if (connectivityCurrent.NODE1_ID != null && connectivityCurrent.NODE2_ID != null)
                    {
                        if (connectivityCurrent.NODE1_ID != 0 && connectivityCurrent.NODE2_ID != 0)
                        {
                            topologyRes.ErrMsg.AppendLine(" 当前设备需一个连接点为零一个连接点不为零 ");
                            topologyRes.RF = ReturnFlag.VERIFYFAILED;
                            return topologyRes;
                        }
                        topologyRes.RF = ReturnFlag.VERIFYSUCCEED;
                        return topologyRes;
                    }
                    if (connectivityCurrent.NODE1_ID == null || connectivityCurrent.NODE2_ID == null)
                    {
                        topologyRes.ErrMsg.AppendLine(" 当前设备需一个连接点为零一个连接点不为零 ");
                        topologyRes.RF = ReturnFlag.VERIFYFAILED;
                        return topologyRes;
                    }
                    #endregion
                }
                else
                {
                    #region  两头都要校验 ，到此处剩下的就是至少有一头有值了

                    if (connectivityCurrent.NODE1_ID != null && connectivityCurrent.NODE1_ID != 0 &&
                        connectivityCurrent.NODE2_ID != null && connectivityCurrent.NODE2_ID != 0)
                    {
                        topologyRes.RF = ReturnFlag.VERIFYSUCCEED;
                        return topologyRes;
                    }
                    topologyRes.ErrMsg.AppendLine(" 当前设备另一头未建立连接关系 ");
                    topologyRes.RF = ReturnFlag.VERIFYFAILED;
                    return topologyRes;
                    #endregion
                }

                #endregion
            }
            catch (Exception ex)
            {
                LogManager.Instance.Error(ex);
                topologyRes.ErrMsg.AppendLine(TopologyMsg[0]);
            }
            topologyRes.RF = ReturnFlag.NONE;
            return topologyRes;
        }

        /// <summary>
        /// 校验从属关系
        /// </summary>
        /// <param name="g3e_fno"></param>
        /// <param name="g3e_fid"></param>
        /// <returns></returns>
        public TopologyVerifyRes VerifyOwnShip(int g3e_fno, long g3e_fid)
        {
            TopologyVerifyRes topologyRes = new TopologyVerifyRes();
            topologyRes.ErrMsg = new StringBuilder();
            try
            {
                List<string> fnoList = new List<string>();
                if (!string.IsNullOrEmpty(MapConfig.Instance.NoVerifyOwnshipFeature))
                {
                    fnoList.AddRange(MapConfig.Instance.NoVerifyOwnshipFeature.Split(','));
                }
                if (fnoList.Any(o => o.Equals(g3e_fno.ToString())) || CommonHelp.Instance.RefTopologyFeature.Contains(g3e_fid))
                {
                    topologyRes.RF = ReturnFlag.VERIFYSUCCEED;
                    return topologyRes;
                }
                var ownerValue = CDDBManager.Instance.GetEntities<G3e_ownership>(o => o.G3E_SOURCEFNO == g3e_fno);
                //先查出当前设备的从属关系
                var DependenceCurrent = DBManager.Instance.GetEntity<Common_n>(o => o.G3E_FID == g3e_fid);
                if (ownerValue == null || !ownerValue.Any())
                {
                    //不需要建立从属，如果建立从属也会返回失败
                    if (DependenceCurrent != null)
                    {
                        if (DependenceCurrent.OWNER1_ID == 0 || DependenceCurrent.OWNER1_ID == null)
                        {
                            topologyRes.RF = ReturnFlag.VERIFYSUCCEED;
                            return topologyRes;
                        }
                        topologyRes.ErrMsg.AppendLine(" 该设备不该建立从属关系 ");
                        topologyRes.RF = ReturnFlag.VERIFYFAILED;
                        return topologyRes;
                    }
                    topologyRes.RF = ReturnFlag.VERIFYSUCCEED;
                    return topologyRes;
                }
                //如果没建立从属就返回失败，否则就去检测从属；
                if (DependenceCurrent == null)
                {
                    topologyRes.ErrMsg.AppendLine(" 缺失从属关系 ");
                    topologyRes.RF = ReturnFlag.VERIFYFAILED;
                    return topologyRes;
                }
                var csy = DBManager.Instance.GetEntities<Common_n>(o => o.G3E_ID == DependenceCurrent.OWNER1_ID);
                if (csy == null || !csy.Any())
                {
                    topologyRes.RF = ReturnFlag.VERIFYFAILED;
                    topologyRes.ErrMsg.AppendLine(" 缺失从属关系 ");
                    return topologyRes;
                }
                //只有房内的设备才判断是否在房内
                foreach (var ent in csy)
                {
                    List<int> fnoLists = new List<int> {142, 163, 149, 302, 77};
                    if (ent != null && fnoLists.Contains(ent.G3E_FNO))
                    {
                        if (PublicMethod.Instance.IsPointFeature(g3e_fid))
                        {
                            if (PublicMethod.Instance.IsLineFeature(ent.G3E_FID))
                            {
                                if (!PublicMethod.Instance.IsRangeOfDf(g3e_fid, ent.G3E_FID))
                                {
                                    topologyRes.ErrMsg.AppendLine(" 该设备超出电房范围 ");
                                    topologyRes.RF = ReturnFlag.VERIFYFAILED;
                                    return topologyRes;
                                }
                            }
                        }
                    }
                }

                //说明建立了从属关系
                topologyRes.RF = ReturnFlag.VERIFYSUCCEED;
                return topologyRes;
            }
            catch (System.Exception ex)
            {
                LogManager.Instance.Error(ex);
                topologyRes.ErrMsg.AppendLine(TopologyMsg[1]);
            }
            topologyRes.RF = ReturnFlag.NONE;
            return topologyRes;

        }
        /// <summary>
        /// 校验包含关系
        /// </summary>
        /// <returns></returns>
        public TopologyVerifyRes VerifyContain(int g3e_fno, long g3e_fid)
        {
            TopologyVerifyRes topolgyRes = new TopologyVerifyRes();
            topolgyRes.ErrMsg = new StringBuilder();
            try
            {
                if (CommonHelp.Instance.RefTopologyFeature.Contains(g3e_fid))
                {
                    topolgyRes.RF = ReturnFlag.VERIFYSUCCEED;
                    return topolgyRes;
                }
                if (g3e_fno == 201)
                {
                    //先查出当前设备的包含关系
                    var ConatinCurrent = DBManager.Instance.GetEntities<Contain_n>(o => o.G3E_FID == g3e_fid);
                    if (ConatinCurrent == null)
                    {
                        topolgyRes.RF = ReturnFlag.VERIFYFAILED;
                        topolgyRes.ErrMsg.AppendLine(" 电杆缺失包含关系 ");
                        return topolgyRes;
                    }
                    if (ConatinCurrent.Any(dg => dg.G3E_OWNERFNO != 0 || dg.G3E_OWNERFID != 0))
                    {
                        topolgyRes.RF = ReturnFlag.VERIFYFAILED;
                        topolgyRes.ErrMsg.AppendLine(" 电杆包含关系建立错误 ");
                        return topolgyRes;
                    }
                    topolgyRes.RF = ReturnFlag.VERIFYSUCCEED;
                    return topolgyRes;
                }
                if (g3e_fno == 141)
                {
                    //var GnwzCurrent = DBManager.Instance.GetEntity<V94.Gg_pd_gnwzmc_n>(o => o.G3E_FID == g3e_fid);
                    //if (GnwzCurrent != null)
                    //{
                    //    if (GnwzCurrent.GNWZ_XLFL.Equals("连接导线"))
                    //    {
                    //        topolgyRes.RF = ReturnFlag.VERIFYSUCCEED;
                    //        return topolgyRes;
                    //    }
                    //}
                    //else
                    //{
                        //先查出当前设备的包含关系
                        var ConatinCurrent = DBManager.Instance.GetEntities<Contain_n>(o => o.G3E_FID == g3e_fid);
                        if (ConatinCurrent == null || !ConatinCurrent.Any())
                        {
                            topolgyRes.RF = ReturnFlag.VERIFYFAILED;
                            topolgyRes.ErrMsg.AppendLine(" 导线缺失包含关系 ");
                            return topolgyRes;
                        }
                        //判断多条线连接一条杆时只要该线有一条数据建立不完整就fail
                        if (ConatinCurrent.Any(dx => dx.G3E_OWNERFID == 0 || dx.G3E_OWNERFNO != 201))
                        {
                            topolgyRes.RF = ReturnFlag.VERIFYFAILED;
                            topolgyRes.ErrMsg.AppendLine(" 导线包含关系建立错误 ");
                            return topolgyRes;
                        }
                        topolgyRes.RF = ReturnFlag.VERIFYSUCCEED;
                        return topolgyRes;
                    //}
                }
                //该设备无需建立包含关系
                topolgyRes.RF = ReturnFlag.VERIFYSUCCEED;
                return topolgyRes;
            }
            catch (Exception ex)
            {
                LogManager.Instance.Error(ex);
                topolgyRes.ErrMsg.AppendLine(TopologyMsg[2]);
            }
            topolgyRes.RF = ReturnFlag.VERIFYSUCCEED;
            return topolgyRes;
        }
        /// <summary>
        /// 获取设备状态
        /// </summary>
        /// <param name="tables"></param>
        /// <returns></returns>
        public string GetDevEntityState(RequiredDevTables tables)
        {
            EntityState res = EntityState.None;
            string state = string.Empty;
            if (tables.ComObj != null && tables.ComObj.EntityState != EntityState.None)
            {
                res = tables.ComObj.EntityState;
            }
            else if (tables.SelfObj != null && tables.SelfObj.EntityState != EntityState.None)
            {
                res = tables.SelfObj.EntityState;
            }
            else if (tables.ConnectObj != null && tables.ConnectObj.EntityState != EntityState.None)
            {
                res = tables.ConnectObj.EntityState;
            }
            else if (tables.GnwzObj != null && tables.GnwzObj.EntityState != EntityState.None)
            {
                res = tables.GnwzObj.EntityState;
            }
            else
            {
                res = EntityState.None;
            }
            switch (res)
            {
                case EntityState.Insert:
                    state = "新增";
                    break;
                case EntityState.Update:
                    state = "更新";
                    break;
                case EntityState.Delete:
                    state = "已删除";
                    break;
                case EntityState.None:
                    state = "未修改";
                    break;
                default:
                    break;
            }
            if (res.ToString().Contains("_"))
            {
                state = "更新";
            }
            return state;
        }

        private void bindingUpdate(orderWorkifyArgs owa)
        {
            var ent = objList.SingleOrDefault(o => o.G3E_FID == owa.FID);
            if (ent == null) return;
            //校验台账
            VerifySingleTZ(ref ent);
            var value = UpdateResultAfter(owa);
            if (value == null) return;
            StringBuilder sb = new StringBuilder();
            var rdt = DevEventHandler.GetDevTables(owa.FNO, owa.FID);
            if (rdt != null)
            {
                //校验属性
                var res = GenerateHelper.FieldIsNull(rdt, rdt.Fid, rdt.Fno);
                sb.AppendLine(res.ToString());
            }
            if (value.VerifyResult.Trim().Length <= 0)
            {
                if (sb.ToString().Trim().Length <= 0)
                    value.VerifyResult = "校验成功";
                else
                {
                    value.VerifyResult = sb.ToString();
                }
            }
            else
            {
                if (sb.ToString().Trim().Length > 0)
                {
                    sb.AppendLine(value.VerifyResult);
                    value.VerifyResult = sb.ToString();
                }
            }
               
            if (value.DevState.Equals("未修改"))
                value.DevState = "更新";

            var newRes = objList.SingleOrDefault(o => o.G3E_FID == owa.FID);
            WorkVerify(LvAttribute, newRes);
        }
        /// <summary>
        /// 不用再次校验台账直接用之前的
        /// </summary>
        /// <param name="owa"></param>
        /// <returns></returns>
        private VerifyClass UpdateResultAfter(orderWorkifyArgs owa)
        {
            var res = objList.SingleOrDefault(o => o.G3E_FID == owa.FID);
            if (res == null) return null;
            if (res.VerifyResult.Contains(TzMsg[0]))
            {
                res.VerifyResult = TzMsg[0];
            }
            else if (res.VerifyResult.Contains(TzMsg[1]))
            {
                res.VerifyResult = TzMsg[1];
            }
            else if (res.VerifyResult.Contains(TzMsg[2]))
            {
                res.VerifyResult = TzMsg[2];
            }
            else
            {
                res.VerifyResult = string.Empty;
            }
            return res;
        }
        void DevAttribute_workorderVerify(object sender, EventArgs e)
        {
            orderWorkifyArgs owa = (orderWorkifyArgs)e;
            if (owa == null) return;
            bindingUpdate(owa);
            if (owa.FNO == 148)
            {
                var byqTj = DBManager.Instance.GetEntity<Gg_pd_gnwzmc_n>(owa.FID);
                if (byqTj.GNWZ_FL2 == "台架")
                {
                    long tjfid = 0;
                    if (long.TryParse(byqTj.GNWZ_SSTJ, out tjfid))
                    {
                        owa.FNO = 199;
                        owa.FID = tjfid;
                        bindingUpdate(owa);
                    }
                }
               
            }
        }

        void BulkChanges_RefBulkModify(object sender, EventArgs e)
        {
            bulkOrderWorkifyArgs args = (bulkOrderWorkifyArgs)e;
            if (args == null) return;
            foreach (var item in args.bulkChangeList)
            {
                ObjectId objId = DBEntityFinder.Instance.GetObjectIdByFid(item.SingleG3EObject.G3E_FID);
                orderWorkifyArgs owa = new orderWorkifyArgs
                {
                    FID = item.SingleG3EObject.G3E_FID,
                    FNO = item.SingleG3EObject.G3E_FNO,
                    ObjId = objId
                };
                bindingUpdate(owa);
                if (owa.FNO == 148)
                {
                    var byqTj = DBManager.Instance.GetEntity<Gg_pd_gnwzmc_n>(owa.FID);
                    if (byqTj.GNWZ_FL2 == "台架")
                    {
                        long tjfid = 0;
                        if (long.TryParse(byqTj.GNWZ_SSTJ, out tjfid))
                        {
                            owa.FNO = 199;
                            owa.FID = tjfid;
                            bindingUpdate(owa);
                        }
                    }

                }
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="lv"></param>
        /// <param name="vc"></param>
        private void WorkVerify(ListView lv, VerifyClass vc)
        {
            foreach (ListViewItem item in lv.Items)
            {
                var verify=item.Tag as VerifyClass;
                if (verify != null && verify.G3E_FID == vc.G3E_FID)
                {
                    item.UseItemStyleForSubItems = false;
                    item.SubItems[7].Text = vc.VerifyResult;
                    if (!verify.VerifyResult.Equals("校验成功"))
                    {
                        item.SubItems[7].ForeColor = System.Drawing.Color.Red;
                    }
                    else
                        item.SubItems[7].ForeColor = System.Drawing.Color.Black;
                    break;
                }
            }
        }
        void ConnectManager_connectivityVerify(object sender, EventArgs e)
        {
            try
            {
                var ee = (orderWorkifyArgs)e;
                if (ee == null) return;
                VerifyClass vc = topology.SingleOrDefault(o => o.G3E_FID == ee.FID);
                if (vc == null) return;
                SingleDevTopologyVerify(vc);
                WorkVerify(LvTopology, vc);
                #region 填写好当前设备的拓扑后要反校验它对于的设备
                //日后考虑是否需要情况太多影响效率
                #endregion
            }
            catch (Exception ex)
            {
                PublicMethod.Instance.ShowMessage(ex.Message);
            }

        }

       
        private void btExport_Click(object sender, EventArgs e)
        {
            string filePath;
            WindowState = FormWindowState.Minimized;
            using (SaveFileDialog sfd = new SaveFileDialog())
            {
                sfd.Filter = "Excel工作薄(*.xls)|*.xls";
                if (sfd.ShowDialog() != DialogResult.OK)
                {
                    WindowState = FormWindowState.Normal;
                    return;
                }
                filePath = sfd.FileName;
            }
            WindowState = FormWindowState.Normal;
            try
            {
                ExportToExcel excel = new ExportToExcel();
                excel.CreateSheet("增量包属性校验");
                excel.FillCells(objList);
                excel.CreateSheet("增量包拓扑校验");
                excel.FillCells(topology);
                excel.ExportToFile(filePath);
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
                return;
            }
        }

        private void WorkOrderVerify_FormClosing(object sender, FormClosingEventArgs e)
        {
            FixEntity.Instance.ResetOldEntity();
            MenuControl.orderVerify = null;
        }
        private void ToolStripMenuItemRef_Click(object sender, EventArgs e)
        {
            GetVerifyData();
            LoadData();
            LoadListViewData();
        }
        private void ToolStripMenuItemTopRef_Click(object sender, EventArgs e)
        {
            GetVerifyData();
            LoadData();
            LoadListViewData();
        }

        private void btExport_MouseEnter(object sender, EventArgs e)
        {
            btExport.ImageIndex = 1;
            toolTipExcel.Show("导出到Excel", (Button)sender);
        }

        private void btExport_MouseLeave(object sender, EventArgs e)
        {
            btExport.ImageIndex = 0;
            toolTipExcel.Hide((Button)sender);
        }

        private void tabControlVerify_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (tabControlVerify.SelectedIndex == 2)
            {
                btExport.Enabled = false;
            }
            else
                btExport.Enabled = true;
            if (tabControlVerify.SelectedIndex == 1)
            {
                CommonHelp.Instance.SortByColumn(new ColumnClickEventArgs(5), lvcs, LvTopology);
                btRef.Enabled = true;
                bulkModify.Enabled = false;
            }
            else
            {
                CommonHelp.Instance.SortByColumn(new ColumnClickEventArgs(5), lvcs,LvAttribute);
                btRef.Enabled = false;
                bulkModify.Enabled = true;
            }
        }
    

        public List<VerifyRes> VerifySucceedOrFailed()
        {
            List<VerifyRes> VR = new List<VerifyRes>();
            try
            {
                if (!IsLoadData)
                {
                    GetVerifyData();
                    LoadData();
                }
                
                if (objList.Any())
                {
                    foreach (var item in objList)
                    {
                        string res = item.VerifyResult;
                        if (!res.Equals("校验成功"))
                        {
                            if (res.Contains(TzMsg[0]))
                            {
                                res=res.Replace(TzMsg[0], " ");
                                if (!VR.Contains(VerifyRes.TzDefect))
                                    VR.Add(VerifyRes.TzDefect);
                            }
                            if (res.Contains(TzMsg[1]))
                            {
                                res = res.Replace(TzMsg[1], " ");
                                if (!VR.Contains(VerifyRes.TzDefect))
                                    VR.Add(VerifyRes.TzDefect);
                            }
                            if (res.Contains(TzMsg[2]))
                            {
                                res = res.Replace(TzMsg[2], " ");
                                if (!VR.Contains(VerifyRes.NoSyncTzData))
                                    VR.Add(VerifyRes.NoSyncTzData);
                            }
                            res = res.Replace('\r', ' ');
                            res = res.Replace('\n', ' ');
                            if (res.Trim().Length>0)
                            {
                                if (!VR.Contains(VerifyRes.AttributeDefect))
                                {
                                    VR.Add(VerifyRes.AttributeDefect);
                                    break;
                                }
                            }
                        }
                    }
                    foreach (var item in topology)
                    {
                        string res = item.VerifyResult;
                        if (!res.Equals("校验成功"))
                        {
                            if (res.Contains(TopologyMsg[0]))
                            {
                                res = res.Replace(TopologyMsg[0], " ");
                                if (!VR.Contains(VerifyRes.DataError))
                                    VR.Add(VerifyRes.DataError);
                            }
                            if (res.Contains(TopologyMsg[1]))
                            {
                                res = res.Replace(TopologyMsg[1], " ");
                                if (!VR.Contains(VerifyRes.DataError))
                                    VR.Add(VerifyRes.DataError);
                            }
                            if (res.Contains(TopologyMsg[2]))
                            {
                                res = res.Replace(TopologyMsg[2], " ");
                                if (!VR.Contains(VerifyRes.DataError))
                                    VR.Add(VerifyRes.DataError);
                            }
                            res = res.Replace('\r', ' ');
                            res = res.Replace('\n', ' ');
                            if (res.Trim().Length > 0)
                            {
                                if (!VR.Contains(VerifyRes.TopologyDefect))
                                {
                                    VR.Add(VerifyRes.TopologyDefect);
                                    break;
                                }
                            }
                        }
                    }
                }
            }
            catch (Exception)
            {
                if (!VR.Contains(VerifyRes.VerifyExp))
                    VR.Add(VerifyRes.VerifyExp);
                return VR;
            }
            return VR;
        }

        private void LvTopology_MouseDoubleClick(object sender, MouseEventArgs e)
        {
            if (e.Button == MouseButtons.Left && LvTopology.SelectedItems.Count != 0)
            {
                var item = LvTopology.SelectedItems[0];
                var lvi = item.Tag as VerifyClass;
                if (lvi == null || lvi.G3E_FID <= 0) return;
                if (lvi.G3E_FNO == 198)
                {
                    MessageBox.Show("请到开关柜管理界面修改.", "提示", MessageBoxButtons.OK, MessageBoxIcon.Information, MessageBoxDefaultButton.Button1);
                    return;
                }
                ObjectId objId = DBEntityFinder.Instance.GetObjectIdByFid(lvi.G3E_FID);
                if (objId.IsNull)
                {
                    PublicMethod.Instance.ShowMessage("\n该设备的图形符号已经不存在!!!");
                    return;
                }
                else
                {
                    FixEntity.Instance.ClickFixEntity(objId);
                }
                MenuControl.showConnectManager(lvi.G3E_FID);

                //定位
                //FixEntity.Instance.Fix(lvi.G3E_FID);
            }
        }

        private void LvAttribute_MouseDoubleClick(object sender, MouseEventArgs e)
        {
            if (e.Button == MouseButtons.Left && LvAttribute.SelectedItems.Count != 0)
            {
                var item = LvAttribute.SelectedItems[0];
                var lvi = item.Tag as VerifyClass;
                if (lvi == null || lvi.G3E_FID <= 0) return;
                if (lvi.G3E_FNO == 198)
                {
                    FixEntity.Instance.Fix(lvi.G3E_FID);
                    MessageBox.Show("请到开关柜管理界面修改.", "提示", MessageBoxButtons.OK, MessageBoxIcon.Information, MessageBoxDefaultButton.Button1);
                    return;
                }
                if (lvi.G3E_FNO == 199)
                {
                    //根据台架找到对应的变压器
                    var tjSelf = DBManager.Instance.GetEntity<Gg_gz_tj_n>(lvi.G3E_FID);
                    if (tjSelf != null)
                    {
                        long fid = 0;
                        if (long.TryParse(tjSelf.GNWZ_FID, out fid))
                        {
                            FixEntity.Instance.Fix(fid);
                        }
                    }
                    //MessageBox.Show("请修改相应的变压器后台架会自动更新", "提示", MessageBoxButtons.OK, MessageBoxIcon.Information, MessageBoxDefaultButton.Button1);
                    return;
                }
                ObjectId objId = ObjectId.Null;
                objId = DBEntityFinder.Instance.GetObjectIdByFid(lvi.G3E_FID);
                //if (objId.IsNull)
                //{
                //    MessageBox.Show("该设备已被删除.");
                //    //return;
                //}
                //DevAttrArgs ee = new DevAttrArgs()
                //{
                //    devObjId = objId
                //};
                //右键的功能位置属性
                //ViewHelper.AddDevAttribute(ee);
                if (RefPanel != null)
                {
                    RefPanel(new object(), new orderWorkifyArgs
                    {
                        FID = lvi.G3E_FID,
                        FNO = lvi.G3E_FNO,
                        ObjId = objId
                    });
                }
                FixEntity.Instance.Fix(lvi.G3E_FID);
            }
        }
        private void LvAttribute_ColumnClick(object sender, ColumnClickEventArgs e)
        {
            CommonHelp.Instance.SortByColumn(e, lvcs, LvAttribute);
        }

        private void LvTopology_ColumnClick(object sender, ColumnClickEventArgs e)
        {
            CommonHelp.Instance.SortByColumn(e, lvcs, LvTopology);
        }

        void PanelControl_SdkxVerify(object sender, EventArgs e)
        {
            orderWorkifyArgs owa = (orderWorkifyArgs)e;
            if (owa == null) return;
            bindingUpdate(owa);
        }
        void PanelControl_PanelControlToWorkOrderVerify(object sender, EventArgs e)
        {
            orderWorkifyArgs owa = (orderWorkifyArgs)e;
            if (owa == null) return;
            if (owa.FNO == 148)
            {
                var gnwzTable = DBManager.Instance.GetEntity<Gg_pd_gnwzmc_n>(owa.FID);
                if (gnwzTable != null)
                {
                    long sstj=0;
                    if (long.TryParse(gnwzTable.GNWZ_SSTJ, out sstj))
                    {
                        orderWorkifyArgs tjOwa = new orderWorkifyArgs
                        {
                            FID = sstj,
                            FNO = 199,
                            ObjId = ObjectId.Null
                        };
                        bindingUpdate(tjOwa);
                    }
                }
               
            }
            bindingUpdate(owa);
        }
        private void btRef_Click(object sender, EventArgs e)
        {
            try
            {
                SelectDevFid.Clear();
                foreach (ListViewItem item in LvTopology.SelectedItems)
                {
                    if (item != null)
                    {
                        var verify = item.Tag as VerifyClass;
                        if (verify != null)
                        {
                            if (!SelectDevFid.Contains(verify.G3E_FID))
                            {
                                SelectDevFid.Add(verify.G3E_FID);
                            }
                        }
                    }
                }
                if (SelectDevFid.Count <= 0)
                {
                    MessageBox.Show("请选择要更新拓扑关系的设备", "CAD提示", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    return;
                }
                if (MessageBox.Show("确定要刷新拓扑关系,此操作有可能会引起单线图预生成失败！", "CAD提示", MessageBoxButtons.OKCancel, MessageBoxIcon.Warning) ==
                    DialogResult.OK)
                {
                    foreach (var item in SelectDevFid)
                    {
                        VerifyClass vc = topology.SingleOrDefault(o => o.G3E_FID == item);
                        if (vc == null) continue;
                        if (!CommonHelp.Instance.RefTopologyFeature.Contains(vc.G3E_FID))
                            CommonHelp.Instance.RefTopologyFeature.Add(vc.G3E_FID);
                        vc.VerifyResult = "校验成功";
                        WorkVerify(LvTopology, vc);
                    }
                }
            }
            catch (Exception ex)
            {
                LogManager.Instance.Error(ex.Message);
            }
            finally
            {
                SelectDevFid.Clear();
            }
        }
        
        public enum ReturnFlag
        {
            NONE,
            VERIFYSUCCEED,
            VERIFYFAILED
        };

        public class TopologyVerifyRes
        {
            public ReturnFlag RF { get; set; }
            public StringBuilder ErrMsg { get; set; }
        }

        private void LvAttribute_ItemMouseHover(object sender, ListViewItemMouseHoverEventArgs e)
        {

            ListViewItem lvi =  e.Item;
            if (lvi != null)
            {
                
                var verifyClass = lvi.Tag as VerifyClass;
                if (verifyClass != null)
                {
                    toolTipExcel.Show(verifyClass.VerifyResult, (ListView) sender,1500);
                }
            }
        }

        private void LvAttribute_Resize(object sender, EventArgs e)
        {
            CommonHelp.Instance.SizeLastColumn(LvAttribute);
        }

        private void LvTopology_Resize(object sender, EventArgs e)
        {
            CommonHelp.Instance.SizeLastColumn(LvTopology);
        }

        private void WorkOrderVerify_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyValue == 27)
                this.Close();
        }

        private void bulkModify_Click(object sender, EventArgs e)
        {
            try
            {
                bulkModifyFid.Clear();
                foreach (ListViewItem item in LvAttribute.SelectedItems)
                {
                    if (item != null)
                    {
                        var verify = item.Tag as VerifyClass;
                        if (verify != null)
                        {
                            if (!bulkModifyFid.Exists(o => o.G3E_FID == verify.G3E_FID))
                                bulkModifyFid.Add(new G3EObject() { G3E_FID = verify.G3E_FID, G3E_FNO = verify.G3E_FNO });
                        }
                    }

                }
                if (bulkModifyFid.Count <= 0)
                {
                    MessageBox.Show("请您选择要批量修改的设备", "CAD提示", MessageBoxButtons.OK);
                    return;
                }
                BlukSelectArgs args = new BlukSelectArgs { BulkIds = bulkModifyFid };
                ViewHelper.AddBulkChangesAttribute(args);
            }
            catch (Exception ex)
            {
                LogManager.Instance.Error(ex);
            }
            finally
            {
                bulkModifyFid.Clear();

            }
        }

     
        
    }
}
