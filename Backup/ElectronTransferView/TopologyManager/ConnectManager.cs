using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Windows.Forms;
using ArxMap;
using Autodesk.AutoCAD.DatabaseServices;
using Autodesk.AutoCAD.EditorInput;
using DotNetARX;
using ElectronTransferDal.Cad;
using ElectronTransferDal.Common;
using ElectronTransferDal.Common.Exceptions;
using ElectronTransferDal.Query;
using ElectronTransferFramework;
using ElectronTransferModel;
using ElectronTransferModel.Base;
using ElectronTransferModel.Config;
using ElectronTransferModel.V9_4;
using ElectronTransferView.Menu;
using ElectronTransferView.ContextMenuManager;

namespace ElectronTransferView.ConnectivityManager
{
    public partial class ConnectManager : UserControl
    {
        #region 变量声明
        // 记录所有初次变色颜色设备
        public static Dictionary<ObjectId, int> OldDit = new Dictionary<ObjectId, int>();
        // 存储从属设备(父类),已变色的子设备      
        static readonly Dictionary<ObjectId, int> SonDevDict = new Dictionary<ObjectId, int>();
        private SelectRule selRule = SelectRule.None;
        private long _fid { get; set; }
        private bool bLTTID { get; set; }
        //单击节点对应的设备
        static ObjectId    m_oldid          =      ObjectId.Null;   
        static int               m_colInd;                        
        //已经变色的设备
        static ObjectId    presel_objid          =      ObjectId.Null;   
        static long           presel_fid;                        
        static int              presel_Ind;                       
        // 单个GetEntity 时 变色对象
        static ObjectId g_objid = ObjectId.Null;       
        static int g_objind;
        // 多个GetSelect时规范FNO
        static long g_fno;
        static readonly Dictionary<ObjectId, int> g_ColDict = new Dictionary<ObjectId, int>();
        public static event EventHandler connectivityVerify;
        #endregion

        public ConnectManager()
        {
            InitializeComponent();
        }
        /// 设置三个view控件节点
        public void SetNod(selectEntityArgs ee)
        {
            // ee包含选中设备的ID,FID,FNO
            var fno = 0;
            if (ee.g3eObject == null)
            {
                _fid = 0;
            }
            else
            {
                _fid = ee.g3eObject.G3E_FID;
                fno = ee.g3eObject.G3E_FNO;
            }
            Setlabtxt(_fid, fno);
            // 工单判断
            VerLttid(ee.objId);
            // 清空颜色
            CleanSelcol();
            // 改变当前选择对象颜色
            ChangSelcol(ee);
            // 刷新连接view
            SetConnViewNod(_fid);
            // 刷新从属view
            SetOwnerViewNod(_fid);
            // 刷新包含view
            SetContainViewNod(_fid); 
            // 当设备没拓扑,禁用该view
            VerRelation(fno);
        }
        // 拓扑判断
        private void VerRelation(long g3e_fno)
        {
            var hasConn = CDDBManager.Instance.GetEntities<G3e_nodeedgeconnectivity>().Select(o => o.G3E_CONNECTINGFNO).Contains((int)g3e_fno);
            if (hasConn == false || _fid == 0)
            {
                connect_tree.Enabled = false;
                connect_tree.BackColor = Color.FromArgb(220, 220, 220);
            }
            var hasOwner1 = CDDBManager.Instance.GetEntities<G3e_ownership>().Select(o => o.G3E_SOURCEFNO).Contains((int)g3e_fno);
            var hasOwner2 = CDDBManager.Instance.GetEntities<G3e_ownership>().Select(o => o.G3E_OWNERFNO).Contains((int)g3e_fno);
            if((hasOwner1 == false && hasOwner2 == false) || _fid == 0)
            {
                owner_tree.Enabled = false;
                owner_tree.BackColor = Color.FromArgb(220, 220, 220);
            }
            if ((g3e_fno != 201 && g3e_fno != 141) || _fid == 0)
            {
                contain_tree.Enabled = false;
                contain_tree.BackColor = Color.FromArgb(220, 220, 220);
            }
        }
        // 工单判断
        private void VerLttid(ObjectId objid)
        {
            bLTTID = true;
            if (DBEntityFinder.Instance.VerifyLTTID(objid) == false)
            {
                bLTTID = false;
            }
        }
        /// 清除presel_objid 和 m_oldid 对象的颜色
        public static void CleanSelcol()
        {
            try
            {
                var db = HostApplicationServices.WorkingDatabase;
                DCadApi.isModifySymbol = true;
                using (var trans = db.TransactionManager.StartTransaction())
                {
                    using (Autodesk.AutoCAD.ApplicationServices.Application.DocumentManager.MdiActiveDocument.LockDocument())
                    {
                        PublicMethod.Instance.UnHighlightDispose();
                        // 清空 树节点选择使变色的设备 颜色
                        if (ObjectId.Null != m_oldid && m_oldid.IsValid)
                        {
                            var ent = trans.GetObject(m_oldid, OpenMode.ForRead, false) as Entity;
                            ent.UpgradeOpenAndRun();
                            if (null != ent)
                            {
                                ent.ColorIndex = m_colInd;
                                m_oldid = ObjectId.Null;
                                m_colInd = 0;
                            }
                        }
                        // 清空 鼠标点击选择的设备 颜色
                        if (ObjectId.Null != presel_objid && presel_objid.IsValid)
                        {
                            var preSelEnt = trans.GetObject(presel_objid, OpenMode.ForRead, false) as Entity;
                            preSelEnt.UpgradeOpenAndRun();
                            if (null != preSelEnt)
                            {
                                preSelEnt.ColorIndex = presel_Ind;
                                presel_objid = ObjectId.Null;
                                presel_fid = 0;
                                presel_Ind = 0;
                            }
                        }
                        if (OldDit != null && OldDit.Count != 0)
                        {
                            foreach (var id in OldDit.Keys)
                            {
                                if (!id.IsValid) continue;
                                var ent = trans.GetObject(id, OpenMode.ForRead, false) as Entity;
                                ent.UpgradeOpenAndRun();
                                if (null != ent)
                                {
                                    ent.ColorIndex = OldDit[id];
                                }
                            }
                            OldDit.Clear();
                        }
                    }
                    trans.Commit();
                    Autodesk.AutoCAD.ApplicationServices.Application.UpdateScreen();
                }
            }
            catch (Autodesk.AutoCAD.Runtime.Exception ex)
            {
                PublicMethod.Instance.ShowMessage(ex.Message);
            }
            catch (Exception ex)
            {
                LogManager.Instance.Error(ex);
            }                
            finally { DCadApi.isModifySymbol = false; }
        }  
        ///  改变ee.fid对象的颜色,通过鼠标点击事件触发
        private void ChangSelcol(selectEntityArgs ee)
        {
            try
            {
                if (presel_fid != ee.g3eObject.G3E_FID)
                {
                    var doc = Autodesk.AutoCAD.ApplicationServices.Application.DocumentManager.MdiActiveDocument;
                    var db = doc.Database;

                    DCadApi.isModifySymbol = true;
                    using (var trans = db.TransactionManager.StartTransaction())
                    {
                        using (Autodesk.AutoCAD.ApplicationServices.Application.DocumentManager.MdiActiveDocument.LockDocument())
                        {
                            if (ObjectId.Null != ee.objId)
                            {
                                var entity = (Entity)trans.GetObject(ee.objId, OpenMode.ForRead, false);
                                entity.UpgradeOpenAndRun();
                                if (null != entity)
                                {
                                    // 存储选中对象的信息，用作下次还原颜色
                                    presel_Ind = entity.ColorIndex;
                                    presel_fid = ee.g3eObject.G3E_FID;
                                    presel_objid = entity.ObjectId;
                                    if (!OldDit.Keys.Contains(entity.Id))
                                        OldDit.Add(entity.Id, entity.ColorIndex);
                                    entity.ColorIndex = 3;
                                    // 获取id，fid， fno 
                                    // 改变label名称
                                    //if (entity is DBText)
                                    //{
                                    //    devname.Text = @"杂项标注";
                                    //}
                                    //else
                                    //{
                                    Setlabtxt(ee.g3eObject.G3E_FID, ee.g3eObject.G3E_FNO);
                                    //}
                                }
                            }
                        }
                        trans.Commit();
                        Autodesk.AutoCAD.ApplicationServices.Application.UpdateScreen();
                    }
                }
            }
            catch (Autodesk.AutoCAD.Runtime.Exception ex)
            {
                PublicMethod.Instance.ShowMessage(ex.Message);
            }
            finally { DCadApi.isModifySymbol = false; }
        }
        // 设置文本框的文字
        private void Setlabtxt(long fid, long fno)
        {
            try
            {
                var strr = "";
                if (FeatureMapping.instance.features.ContainsKey(fno.ToString()) && fid != 0)
                {
                    strr = FeatureMapping.instance.features[fno.ToString()]
                                                    + " - "
                                                    + fid;
                }
                devname.Text = strr;
            }
            catch (Exception ex)
            {
                PublicMethod.Instance.AlertDialog(ex.Message);
            }
        }
        /// 设置连接关系节点
        private void SetConnViewNod(long fid)
        {
            try
            {
                connect_tree.Nodes.Clear();
                connect_tree.Enabled = true;
                connect_tree.BackColor = bLTTID ? Color.White : Color.FromArgb(220, 220, 220);
                connect_tree.Tag = fid;
                var treeNode1 = new TreeNode("节点1");
                var treeNode2 = new TreeNode("节点2");
                treeNode1.Name = fid.ToString();
                treeNode1.Text = "连接1";
                treeNode1.Tag = fid;
                treeNode2.Name = fid.ToString();
                treeNode2.Text = "连接2";
                treeNode2.Tag = fid;
                var device_connect = DBManager.Instance.GetEntity<Connectivity_n>(fid);
                //var device_connect = PublicMethod.Instance.GetBrotherTable(ee.objId, "Connectivity_n") as Connectivity_n;
                if ( null == device_connect)
                {
                    connect_tree.Nodes.AddRange(new[] { treeNode1, treeNode2});
                    //PublicMethod.Instance.Editor.WriteMessageWithReturn("设备" +fid + " 没有连接关系!\n");
                    return;
                }

                IEnumerable<Connectivity_n> device_node11 = null;
                IEnumerable<Connectivity_n> device_node12 = null;
                IEnumerable<Connectivity_n> device_node21 = null;
                IEnumerable<Connectivity_n> device_node22 = null; 
                if ( null != device_connect.NODE1_ID && 0 != device_connect.NODE1_ID )
                {
                    device_node11 = DBManager.Instance.GetEntities<Connectivity_n>(o => o.NODE1_ID == device_connect.NODE1_ID).Where(o => o.G3E_FID != fid && o.EntityState != EntityState.Delete && o.EntityState != EntityState.InsertDelete);
                    device_node12 = DBManager.Instance.GetEntities<Connectivity_n>(o => o.NODE2_ID == device_connect.NODE1_ID).Where(o => o.G3E_FID != fid && o.EntityState != EntityState.Delete && o.EntityState != EntityState.InsertDelete);  
                }

                if ( null != device_connect.NODE2_ID && 0 != device_connect.NODE2_ID )
                {
                    device_node21 = DBManager.Instance.GetEntities<Connectivity_n>(o => o.NODE1_ID == device_connect.NODE2_ID).Where(o => o.G3E_FID != fid && o.EntityState != EntityState.Delete && o.EntityState != EntityState.InsertDelete);
                    device_node22 = DBManager.Instance.GetEntities<Connectivity_n>(o => o.NODE2_ID == device_connect.NODE2_ID).Where(o => o.G3E_FID != fid && o.EntityState != EntityState.Delete && o.EntityState != EntityState.InsertDelete);
                }

                if (null != device_node11)
                foreach (var _no1 in device_node11.Concat(device_node12).Distinct(new ElectronBaseCompare<Connectivity_n>()).OrderBy(o=>o.G3E_FNO))
                {
                    var tn1 = new TreeNode(_no1.G3E_FID.ToString())
                    {
                        Name = _no1.G3E_FID.ToString(),
                        Tag = _no1.G3E_FID,
                        Text = FeatureMapping.instance.features[_no1.G3E_FNO.ToString()]
                               + " - "
                               + _no1.G3E_FID
                    };
                    treeNode1.Nodes.Add(tn1);
                }

                if (null != device_node21)
                foreach (var _no2 in device_node21.Concat(device_node22).Distinct(new ElectronBaseCompare<Connectivity_n>()).OrderBy(o => o.G3E_FNO))
                {
                    var tn2 = new TreeNode(_no2.G3E_FID.ToString())
                    {
                        Name = _no2.G3E_FID.ToString(),
                        Tag = _no2.G3E_FID,
                        Text = FeatureMapping.instance.features[_no2.G3E_FNO.ToString()]
                               + " - " + _no2.G3E_FID
                    };
                    treeNode2.Nodes.Add(tn2);                    
                }

                // 如果是特殊设备,只需要显示一个节点
                var only1Node = false;
                if (PublicMethod.Instance.N2is0.Concat(PublicMethod.Instance.N1isN2).Contains(device_connect.G3E_FNO))
                {
                    treeNode2.Nodes.Clear();
                    only1Node = true;
                }
                if (only1Node)
                {
                    connect_tree.Nodes.AddRange(new[] { treeNode1});
                }
                else
                {
                    connect_tree.Nodes.AddRange(new[] { treeNode1, treeNode2});
                    treeNode2.ExpandAll();
                }
                treeNode1.ExpandAll();
            }
            catch (NotExistException)
            {
                var type = TypeCache.Instance.GetTypeFromCache(typeof(DBEntity), "Connectivity_n");
                var connectn = ReflectionUtils.CreateObject(new { }, type) as DBEntity;
                DBManager.Instance.Insert(connectn);
                SetConnViewNod(fid);
            }
            catch (Autodesk.AutoCAD.Runtime.Exception ex)
            {
                PublicMethod.Instance.Editor.WriteMessageWithReturn(ex.Message);
            }
        }
        /// 设置从属关系节点
        private void SetOwnerViewNod(long fid)
        {
            try
            {
                // 初始化从属界面
                owner_tree.Nodes.Clear(); // 清空树节点
                owner_tree.Enabled = true;
                owner_tree.BackColor = bLTTID ? Color.White : Color.FromArgb(220, 220, 220);
                owner_tree.Tag = fid;
                var treeNode1 = new TreeNode
                {
                    Name = fid.ToString(),
                    Text = "没有从属关系", 
                    Tag = fid
                }; // 根节点

                // 得到节点FID
                var device_common = DBManager.Instance.GetEntities<Common_n>(o => o.G3E_FID == fid).FirstOrDefault();
                if ( null != device_common )
                {
                    //父节点不为0 即 device_common 即有从属关系
                    if ((null != device_common.OWNER1_ID && 0 != device_common.OWNER1_ID) )
                    {
                        Common_n farhter_common = null;
                        if (null != device_common.OWNER1_ID && 0 != device_common.OWNER1_ID)
                        {
                            farhter_common =
                                DBManager.Instance.GetEntities<Common_n>(
                                    o => o.G3E_ID == device_common.OWNER1_ID && o.EntityState != EntityState.Delete)
                                    .FirstOrDefault();
                        }
                        if (null == farhter_common)
                        {
                            PublicMethod.Instance.Editor.WriteMessage("\n数据源Common_n中找不到设备FID为 " 
                                                                            + device_common.G3E_FID.ToString() 
                                                                            + " 的父设备\n");
                        }
                        else
                        {
                            treeNode1.Text = "从属于设备";
                            var tn1 = new TreeNode(farhter_common.G3E_FID.ToString())
                            {
                                Name = farhter_common.G3E_FID.ToString(),
                                Tag = farhter_common.G3E_FID,
                                Text = FeatureMapping.instance.features[farhter_common.G3E_FNO.ToString()]
                                       + " - "
                                       + farhter_common.G3E_FID
                            };
                            treeNode1.Nodes.Add(tn1);
                            //做低压柜等有双重从属关系的特殊处理
                            int[] double_owner = {309, 198, 148, 102, 81, 140};
                            if (double_owner.Contains(device_common.G3E_FNO))
                            {
                                var ttn = new TreeNode(device_common.G3E_FID.ToString())
                                {
                                    Name = device_common.G3E_FID.ToString(),
                                    Tag = device_common.G3E_FID,
                                    Text = FeatureMapping.instance.features[device_common.G3E_FNO.ToString()]
                                           + " - "
                                           + device_common.G3E_FID
                                };
                                tn1.Nodes.Add(ttn);
                                var son_devs =
                                    DBManager.Instance.GetEntities<Common_n>(
                                        o => o.OWNER1_ID == device_common.G3E_ID && o.EntityState != EntityState.Delete);
                                if (son_devs.Any())
                                {
                                    //添加低压柜等子设备设备节点
                                    foreach (var s in son_devs)
                                    {
                                        var ttn1 = new TreeNode(s.G3E_FID.ToString())
                                        {
                                            Name = s.G3E_FID.ToString(),
                                            Tag = s.G3E_FID,
                                            Text = FeatureMapping.instance.features[s.G3E_FNO.ToString()]
                                                   + " - "
                                                   + s.G3E_FID
                                        };
                                        ttn.Nodes.Add(ttn1);
                                    }
                                }
                            }
                        }

                    }
                    else  //父节点为0,判断是否面设备 或者 杆塔设备
                    {
                        var no1_commons =
                            DBManager.Instance.GetEntities<Common_n>(
                                o => o.OWNER1_ID == device_common.G3E_ID && o.EntityState != EntityState.Delete)
                                .OrderBy(o => o.G3E_FNO); // bu 如果面设备没有从属设备，将出现面设备“没有从属关系"
                        //var devn = DBManager.Instance.GetEntities<G3e_ownership>().Select(o=>o.G3E_OWNERFNO);
                        int[] devn = {3,7,9,77,81,102,140,142,148,149,163,198,199,200,201,203,204,215,218,302,309};
                        if (devn.Contains(device_common.G3E_FNO))
                        {
                            treeNode1.Name = device_common.G3E_FID.ToString();
                            treeNode1.Tag = device_common.G3E_FID;
                            treeNode1.Text = FeatureMapping.instance.features[device_common.G3E_FNO.ToString()]
                                                            + " - "
                                                            + device_common.G3E_FID;
                        }
                        if (no1_commons.Any())
                        {
                            foreach (var _no1 in no1_commons)
                            {
                                var tn1 = new TreeNode(_no1.G3E_FID.ToString())
                                {
                                    Name = _no1.G3E_FID.ToString(),
                                    Tag = _no1.G3E_FID,
                                    Text = FeatureMapping.instance.features[_no1.G3E_FNO.ToString()]
                                           + " - "
                                           + _no1.G3E_FID
                                };
                                treeNode1.Nodes.Add(tn1);
                            }
                        }
                    }
                }
                owner_tree.Nodes.AddRange(new[] {
                treeNode1});
                treeNode1.ExpandAll();
            }
            catch (NotExistException)
            {
                var type = TypeCache.Instance.GetTypeFromCache(typeof(DBEntity), "Common_n");
                var commonn = ReflectionUtils.CreateObject(new { }, type) as DBEntity;
                DBManager.Instance.Insert(commonn);
                SetOwnerViewNod(fid);
            }
            catch (Autodesk.AutoCAD.Runtime.Exception ex)
            {
                PublicMethod.Instance.ShowMessage(ex.Message);
            }
        }
        /// 设置包含关系节点
        private void SetContainViewNod(long fid)
        {
            try
            {
                contain_tree.Nodes.Clear();
                contain_tree.Enabled = true;
                contain_tree.BackColor = bLTTID ? Color.White : Color.FromArgb(220, 220, 220);
                var treeNode1 = new TreeNode
                {
                    Name = fid.ToString(), 
                    Text = "没有包含关系"
                };

                // 得到父节点FID
                //IEnumerable<Contain_n> tt = DBManager.Instance.GetEntities<Contain_n>(o => o.G3E_FID == fid);
                var contain_commons = DBManager.Instance.GetEntities<Contain_n>(o => o.G3E_FID == fid
                    && o.EntityState != EntityState.Delete);
                if (contain_commons.Any())
                {
                    //treeNode1.Name = fid.ToString();
                    treeNode1.Text = "被包含";

                    foreach ( var con in contain_commons )
                    {
                        if ( null != con.G3E_OWNERFID && 0 != con.G3E_OWNERFID && 141 == con.G3E_FNO)  // 低压导线 141
                        {
                            var tn = new TreeNode
                            {
                                Name = con.G3E_OWNERFID.ToString(),
                                Text = FeatureMapping.instance.features[con.G3E_OWNERFNO.ToString()]
                                       + " - "
                                       + con.G3E_OWNERFID.ToString()
                            };
                            treeNode1.Nodes.Add(tn);
                        }
                        else if (0 == con.G3E_OWNERFID && 201 == con.G3E_FNO) // 电杆 201
                        {
                            treeNode1.Name = con.G3E_FID.ToString();
                            treeNode1.Text = "包含";
                            var con1 = con;
                            var line_contains = DBManager.Instance.GetEntities<Contain_n>(o => o.G3E_OWNERFID == con1.G3E_FID && o.EntityState != EntityState.Delete); // 得到G3E_OWNERFID = G3E_FID(电杆)  的 低压导线集合
                            if (!line_contains.Any()) continue;
                            foreach (var _con in line_contains)
                            {
                                var tn = new TreeNode
                                {
                                    Name = _con.G3E_FID.ToString(),
                                    Text = FeatureMapping.instance.features[_con.G3E_FNO.ToString()]
                                           + " - "
                                           + _con.G3E_FID
                                };
                                treeNode1.Nodes.Add(tn);
                            }
                        }
                    }
                }
                else
                {
                    var temp = DBManager.Instance.GetEntities<Common_n>(o => o.G3E_FID == fid && o.EntityState != EntityState.Delete).FirstOrDefault();
                    var nv = new Contain_n();
                    if (temp == null) { }
                    else if (temp.G3E_FNO == 141)
                    {
                        treeNode1.Text = "被包含";
                    }
                    else if (temp .G3E_FNO == 201)
                    {
                        treeNode1.Text = "包含";
                    }
                    var ct = DBManager.Instance.GetEntity<Contain_n>(o => o.G3E_FID == fid && o.EntityState != EntityState.Delete);
                    if (ct == null && (temp != null && (temp.G3E_FNO == 141 || temp.G3E_FNO == 201)))
                    {
                        nv.G3E_FID = fid;
                        nv.G3E_FID = fid;
                        nv.G3E_CNO = 38;
                        nv.G3E_CID = 1;
                        nv.G3E_ID = CYZCommonFunc.getid();
                        nv.G3E_OWNERFID = 0;
                        nv.G3E_OWNERFNO = 0;
                        nv.LTT_ID = MapConfig.Instance.LTTID;
                        nv.LTT_DATE = null;
                        nv.LTT_STATUS = null;
                        nv.G3E_FNO = temp.G3E_FNO;
                        nv.EntityState = EntityState.Insert;
                        DBManager.Instance.Insert(nv);
                    }
                }

                contain_tree.Nodes.AddRange(new[] {
                treeNode1});
                treeNode1.ExpandAll();
            }
            catch (NotExistException)
            {
                var type = TypeCache.Instance.GetTypeFromCache(typeof(DBEntity), "Contain_n");
                var containn = ReflectionUtils.CreateObject(new { }, type) as DBEntity;
                DBManager.Instance.Insert(containn);
                SetContainViewNod(fid);
            }
            catch(Autodesk.AutoCAD.Runtime.Exception ex)
            {
                PublicMethod.Instance.ShowMessage(ex.Message);
            }
        }
        /// 选择节点后事件
        void AfterSel(TreeNodeMouseClickEventArgs e)
        {
            try
            {
                if (e.Node.Text.Contains("连接") || e.Node.Text.Contains("从属") || e.Node.Text.Contains("包含")) return;
                var doc = Autodesk.AutoCAD.ApplicationServices.Application.DocumentManager.MdiActiveDocument;
                var db = doc.Database;

                DCadApi.isModifySymbol = true;
                using (Autodesk.AutoCAD.ApplicationServices.Application.DocumentManager.MdiActiveDocument.LockDocument())
                {
                    using (var trans = db.TransactionManager.StartTransaction())
                    {
                        if (ObjectId.Null != m_oldid)
                        {
                            var entity = (Entity)trans.GetObject(m_oldid, OpenMode.ForRead, true);
                            entity.UpgradeOpenAndRun();
                            if (null != entity)
                            {
                                entity.ColorIndex = 3;
                            }
                        }
                        trans.Commit();
                        Autodesk.AutoCAD.ApplicationServices.Application.UpdateScreen();
                    }
                }
            }
            catch (Autodesk.AutoCAD.Runtime.Exception ex)
            {
                PublicMethod.Instance.AlertDialog(ex.Message);
            }
            finally
            {
                DCadApi.isModifySymbol = false;
            }
        }
        /// 选择节点前事件
        void BeforeSel(TreeNodeMouseClickEventArgs e)
        {
            try
            {
                if (e.Node.Text.Contains("连接") || e.Node.Text.Contains("从属") || e.Node.Text.Contains("包含")) return;
                var doc = Autodesk.AutoCAD.ApplicationServices.Application.DocumentManager.MdiActiveDocument;
                var db = doc.Database;
                var idd = long.Parse(e.Node.Name);
                DCadApi.isModifySymbol = true;
                using (var trans = db.TransactionManager.StartTransaction())
                {
                    using (doc.LockDocument())
                    {
                        Entity entity;
                        if (ObjectId.Null != m_oldid)
                        {
                            entity = (Entity)trans.GetObject(m_oldid, OpenMode.ForRead, false);
                            entity.UpgradeOpenAndRun();
                            entity.ColorIndex = m_colInd;
                        }

                        var objidd = DBEntityFinder.Instance.GetObjectIdByFid(idd);
                        if (ObjectId.Null != objidd)
                        {
                            entity = (Entity)trans.GetObject(objidd, OpenMode.ForRead, false);
                            if (null != entity)
                            {
                                if (!OldDit.Keys.Contains(entity.Id))
                                    OldDit.Add(entity.Id, entity.ColorIndex);
                                m_colInd = entity.ColorIndex;
                                m_oldid = entity.Id;
                            }
                        }
                    }
                    trans.Commit();
                    Autodesk.AutoCAD.ApplicationServices.Application.UpdateScreen();
                }
            }
            catch (Autodesk.AutoCAD.Runtime.Exception ex)
            {
                PublicMethod.Instance.AlertDialog(ex.Message);
            }
            finally { DCadApi.isModifySymbol = false; }
        }
        /// 高亮子设备
        void HightLightSonDevice(TreeNodeMouseClickEventArgs e)
        {
            var doc = Autodesk.AutoCAD.ApplicationServices.Application.DocumentManager.MdiActiveDocument;
            var db = doc.Database;
            #region 还原之前变色的设备

            if (SonDevDict.Any())
            {
                using (var trans = db.TransactionManager.StartTransaction())
                {
                    using (doc.LockDocument())
                    {
                        foreach (var dev in SonDevDict.Keys)
                        {
                            if (ObjectId.Null != m_oldid)
                            {
                                var entity = trans.GetObject(dev, OpenMode.ForRead, false) as Entity;
                                if (entity == null) continue;
                                entity.UpgradeOpenAndRun();
                                entity.ColorIndex = SonDevDict[dev];
                            }
                        }
                    }
                    trans.Commit();
                    SonDevDict.Clear();
                }
            }

            #endregion
            // 如果是父类设备
            if (e.Node.Level != 0 || e.Node.Text.Contains("从属")) return;
            var ownerFid = (long)e.Node.Tag;
            var fcomm = DBManager.Instance.GetEntities<Common_n>(o=>o.G3E_FID == ownerFid).FirstOrDefault();
            if (fcomm == null) return;
            var scomms =
                DBManager.Instance.GetEntities<Common_n>(o => o.OWNER1_ID == fcomm.G3E_ID && o.EntityState != EntityState.Delete);
            if (!scomms.Any()) return;

            #region 变色子设备

            using (var trans = db.TransactionManager.StartTransaction())
            {
                using (doc.LockDocument())
                {
                    foreach (var scomm in scomms)
                    {
                        var objidd = DBEntityFinder.Instance.GetObjectIdByFid(scomm.G3E_FID);
                        if (ObjectId.Null != objidd)
                        {
                            var entity = trans.GetObject(objidd, OpenMode.ForRead, false) as Entity;
                            if (entity == null) continue;
                            SonDevDict.Add(entity.ObjectId, entity.ColorIndex);
                            entity.UpgradeOpenAndRun();
                            entity.ColorIndex = 4;
                        }
                    }
                }
                trans.Commit();
            }

        #endregion
        }

        /// 鼠标双击事件
        private void ctr_NodeMouseDoubleClick(object sender, TreeNodeMouseClickEventArgs e)
        {
            try
            {
                var doc = Autodesk.AutoCAD.ApplicationServices.Application.DocumentManager.MdiActiveDocument;
                var db = doc.Database;
                var ed = doc.Editor;
                using (var trans = db.TransactionManager.StartTransaction())
                {
                    var objid = DBEntityFinder.Instance.GetObjectIdByFid(long.Parse(e.Node.Name));
                    if (ObjectId.Null != objid)
                    {
                        var entity = (Entity)trans.GetObject(objid, OpenMode.ForRead, false);

                        if (null == entity) return;

                        if (entity is BlockReference)
                        {
                            var pt = (entity as BlockReference).Position;
                            var str = string.Format("{0},{1} ", pt.X, pt.Y);
                            PublicMethod.Instance.SendCommend(string.Format("zoom\nc\n{0}{1}\n", str, 0.0001));
                        }
                        else if ( entity is Polyline)
                        {
                            var ex = entity.GeometricExtents;                            
                            var pt1 = ex.MinPoint;
                            var pt2 = ex.MaxPoint;
                            var str1 = string.Format("{0},{1}", pt1.X, pt1.Y);
                            var str2 = string.Format("{0},{1}", pt2.X, pt2.Y);
                            
                            PublicMethod.Instance.SendCommend(string.Format("zoom\nw\n{0}\n{1}\n", str1, str2));
                        }
                    }
                    else
                    {
                        ed.WriteMessage("\nCAD图纸中找不到对应 FID =" + e.Node.Name + "设备!\n");
                    }
                }
            }
            catch (Autodesk.AutoCAD.Runtime.Exception ex)
            {
                PublicMethod.Instance.ShowMessage(ex.Message);
            }
        }
        /// 单击面板
        private void connect_tree_NodeMouseClick(object sender, TreeNodeMouseClickEventArgs e)
        {
            try
            {
                //判断是否是右击
                if (e.Button == MouseButtons.Right)
                {
                    //获取节点区域的右下角坐标值
                    var pos = new Point(e.Node.Bounds.X + e.Node.Bounds.Width, e.Node.Bounds.Y + e.Node.Bounds.Height);
                    if (e.Node.Level == 0)
                    {
                        if (bLTTID)
                        {
                            connectMenuStrip.Items["打断"].Visible = true;
                            // 当连接点没有子节点时,不能打断
                            if (0 == e.Node.Nodes.Count)
                                connectMenuStrip.Items["打断"].Visible = false;

                            //在选中的节点的右下角，弹出右键菜单，并设定控制者为treeView
                            connectMenuStrip.Tag = sender;
                            connectMenuStrip.Show(e.Node.TreeView, pos);
                        }
                    }
                    else if (e.Node.Level == 1)
                    {
                        var ffid = (long) e.Node.Tag;
                        if (DBEntityFinder.Instance.VerifyLTTID(ffid))
                        {
                            if ((bLTTID == false && e.Node.Parent.Nodes.Count != 1) || bLTTID)
                            {
                                BreakConMenuStrip.Tag = e.Node.Tag;
                                BreakConMenuStrip.Show(e.Node.TreeView, pos);
                            }
                        }
                    }
                    //选中点击的节点
                    e.Node.TreeView.SelectedNode = e.Node;
                }
                BeforeSel(e);
                AfterSel(e);

            }
            catch (Autodesk.AutoCAD.Runtime.Exception ex)
            {
                PublicMethod.Instance.ShowMessage(ex.Message);
            }

        }
        private void owner_tree_NodeMouseClick(object sender, TreeNodeMouseClickEventArgs e)
        {
            try
            {
                //判断是否是右击
                if (e.Button == MouseButtons.Right)
                {

	                    //获取节点区域的右下角坐标值
	                    var pos = new Point(e.Node.Bounds.X + e.Node.Bounds.Width, e.Node.Bounds.Y + e.Node.Bounds.Height);
                    if (bLTTID)
                    {
                        // 初始化显示
                        for (var i = 0; i < ownerMenuStrip.Items.Count; i ++)
                            ownerMenuStrip.Items[i].Visible = false;
                        // 1 点击lv0 节点
                        if (e.Node.Level == 0)
                        {
                            switch (e.Node.Text)
                            {
                                case "从属于设备":
                                    ownerMenuStrip.Items["删除从属关系"].Visible = true;
                                    break;
                                case "没有从属关系":
                                    ownerMenuStrip.Items["从属于设备"].Visible = true;
                                    break;
                                default:
                                    ownerMenuStrip.Items["从属于设备"].Visible = true;
                                    ownerMenuStrip.Items["添加从属设备"].Visible = true;
                                    ownerMenuStrip.Items["添加从属设备自动"].Visible = true;
                                    ownerMenuStrip.Items["批量添加从属设备"].Visible = true;
                                    ownerMenuStrip.Items["批量删除从属设备"].Visible = true;
                                    break;
                            }
                        }
                            //2  点击>=lv1 子节点
                        else if (e.Node.Level == 1 || e.Node.Level == 3)
                        {
                            if (e.Node.Parent.Text != "从属于设备" && e.Node.Parent.Text != "没有从属关系")
                            {
                                ownerMenuStrip.Items["删除从属设备"].Visible = true;
                            }
                        }
                        else if (e.Node.Level == 2)
                        {
                            ownerMenuStrip.Items["从属于设备"].Visible = true;
                            ownerMenuStrip.Items["添加从属设备"].Visible = true;
                            ownerMenuStrip.Items["添加从属设备自动"].Visible = true;
                            ownerMenuStrip.Items["批量添加从属设备"].Visible = true;
                            ownerMenuStrip.Items["批量删除从属设备"].Visible = true;
                        }
                        ownerMenuStrip.Tag = sender;
                        ownerMenuStrip.Show(e.Node.TreeView, pos);
                    }
                    //选中点击的节点
                    e.Node.TreeView.SelectedNode = e.Node;
                }
                else
                {
                    //HightLightSonDevice(e);
                    BeforeSel(e);
                    AfterSel(e);
                }
            }
            catch (Autodesk.AutoCAD.Runtime.Exception ex)
            {
                PublicMethod.Instance.ShowMessage(ex.Message);
            }
        }
        private void contain_tree_NodeMouseClick(object sender, TreeNodeMouseClickEventArgs e)
        {
            try
            {
                //判断是否是右击
                if (e.Button == MouseButtons.Right)
                {
                    if (bLTTID)
                    {
	                    //获取节点区域的右下角坐标值
	                    var pos = new Point(e.Node.Bounds.X + e.Node.Bounds.Width, e.Node.Bounds.Y + e.Node.Bounds.Height);
	                    // 初始化显示
	                    for (var i = 0; i < containMenuStrip.Items.Count; i++)
	                        containMenuStrip.Items[i].Visible = false;
	
	                    if (e.Node.Level == 0)
	                    {
	
	                        if  (e.Node.Text == "没有包含关系")
	                        {
	                            var vv = DBManager.Instance.GetEntities<Contain_n>(o => o.G3E_FID == long.Parse(e.Node.Name));
	                            if (!vv.Any()) return;
	                            if (141 == vv.First().G3E_FNO)
	                            {
	                                containMenuStrip.Items["被包含"].Visible = true;
	                                containMenuStrip.Items["被包含批量"].Visible = true;
                                    containMenuStrip.Items["被包含自动"].Visible = true;
	                            }
	                            else if (201 == vv.First().G3E_FNO)
	                            {
	                                containMenuStrip.Items["包含"].Visible = true;
	                                containMenuStrip.Items["包含批量"].Visible = true;
	                            }
	                        }
	                        else if (e.Node.Text == "包含")
	                        {
	                            containMenuStrip.Items["包含"].Visible = true;
                                containMenuStrip.Items["包含批量"].Visible = true;
	                        }
	                        else 
	                        {
	                            containMenuStrip.Items["被包含"].Visible = true;
                                containMenuStrip.Items["被包含批量"].Visible = true;
                                containMenuStrip.Items["被包含自动"].Visible = true;
	                        }
	                    }
	                    else if (1 == e.Node.Level)
	                    {
	                        if (e.Node.Parent.Text == "包含")
	                            containMenuStrip.Items["删除线"].Visible = true;
	                        else if (e.Node.Parent.Text == "被包含")
	                            containMenuStrip.Items["删除杆"].Visible = true;
	                    }
	                    //在选中的节点的右下角，弹出右键菜单，并设定控制者为treeView
	                    containMenuStrip.Tag = sender;
	                    containMenuStrip.Show(e.Node.TreeView, pos);
                    }

                    //选中点击的节点
                    e.Node.TreeView.SelectedNode = e.Node;
                }
                else
                {
                    BeforeSel(e);
                    AfterSel(e);
                }
            }
            catch (Autodesk.AutoCAD.Runtime.Exception ex)
            {
                PublicMethod.Instance.ShowMessage(ex.Message);
            }
        }
        /// 面板菜单被单击
        private void connectMenuStrip_ItemClicked(object sender, ToolStripItemClickedEventArgs e)
        {
            try
            {
                var tt = (ContextMenuStrip)sender;
                var t = (TreeView)tt.Tag;
                var fid = long.Parse(t.SelectedNode.Name);
                connectMenuStrip.Hide();// 切换焦点到cad窗口
                DisableView();
                if (e.ClickedItem.Text == "打断")
                {
                    breakConnect(fid, t);
                }
                else if (e.ClickedItem.Text == "连接")
                {
                    insertConnect(fid, t);
                }
            }
            catch (Autodesk.AutoCAD.Runtime.Exception ex)
            {
                PublicMethod.Instance.ShowMessage(ex.Message);
            }
            finally
            {
                EnableView();
            }
        }
        private void ownerMenuStrip_ItemClicked(object sender, ToolStripItemClickedEventArgs e)
        {
            try
            {
                var tt = (ContextMenuStrip)sender;
                var t = (TreeView)tt.Tag;
                var fid = long.Parse(t.SelectedNode.Name);
                ownerMenuStrip.Hide();// 切换焦点到cad窗口
                DisableView();
                switch (e.ClickedItem.Text)
                {
                    case "从属于设备":
                        belong2Area(fid);
                        break;
                    case "删除从属关系":
                        delDevOwner(fid, t);
                        break;
                    case "删除从属设备":
                        delDevOwner(fid, t);
                        break;
                    case "添加从属设备":
                        addDev2Area(fid);
                        break;
                    case "添加从属设备(自动)":
                        OwnerMethods.addDev2AreaAuto(fid);
                        SetOwnerViewNod(fid);
                        break;
                    case "批量添加从属设备":
                        addDevs2Area(fid);
                        break;
                    case "批量删除从属设备":
                        delManyOwner(fid);
                        break;
                }
            }
            catch (Autodesk.AutoCAD.Runtime.Exception ex)
            {
                PublicMethod.Instance.ShowMessage(ex.Message);
            }
            finally
            {
                EnableView();
            }
        }
        private void containMenuStrip_ItemClicked(object sender, ToolStripItemClickedEventArgs e)
        {
            try
            {
                var tt = (ContextMenuStrip) sender;
                var t = (TreeView) tt.Tag;
                var fid = long.Parse(t.SelectedNode.Name);
                containMenuStrip.Hide(); // 切换焦点到cad窗口
                DisableView();
                switch (e.ClickedItem.Text)
                {
                    case "包含":
                        contain(fid);
                        break;
                    case "包含(批量)":
                        contains(fid);
                        break;

                    case "被包含":
                        beContain(fid);
                        break;

                    case "被包含(批量)":
                        beContains(fid);
                        break;

                    case "被包含(自动)":
                        beContainAuto(fid);
                        break;

                    case "删除线":
                        delLine(fid, t);
                        break;

                    case "删除杆":
                        delRole(t);
                        break;
                }
            }
            catch (Autodesk.AutoCAD.Runtime.Exception ex)
            {
                PublicMethod.Instance.ShowMessage(ex.Message);
            }
            finally
            {
                EnableView();
            }
        }

        void EnableView()
        {
            if (connect_tree.BackColor.ToArgb() == Color.White.ToArgb())
                connect_tree.Enabled = true;
            if (owner_tree.BackColor.ToArgb() == Color.White.ToArgb())
                owner_tree.Enabled = true;
            if (contain_tree.BackColor.ToArgb() == Color.White.ToArgb())
                contain_tree.Enabled = true;
        }

        private void DisableView()
        {
            if (connect_tree.BackColor.ToArgb() == Color.White.ToArgb())
                connect_tree.Enabled = false;
            if (owner_tree.BackColor.ToArgb() == Color.White.ToArgb())
                owner_tree.Enabled = false;
            if (contain_tree.BackColor.ToArgb() == Color.White.ToArgb())
                contain_tree.Enabled = false;   
        }

        // 打断连接关系
        void breakConnect(long fid, TreeView t)
        {
            // fid对应设备状态不等于 delete 、 insertdelete、insert、update、个别包含del状态
            var conn = DBManager.Instance.GetEntity<Connectivity_n>(o => o.G3E_FID == fid);
            if (null == conn) { PublicMethod.Instance.Editor.WriteMessage("\nConnectivity_n没有对应fid: {0} 字段!\n", fid);   return; }

            if (1 == t.SelectedNode.Nodes.Count)// 当只有一个设备与其相连,特殊处理
            {
                var temfid = long.Parse(t.SelectedNode.FirstNode.Name);
                // 工单判断
                if (DBEntityFinder.Instance.VerifyLTTID(temfid) == false)
                {
                    PublicMethod.Instance.AlertDialog("与此设备相连的设备没有被工单锁定,不能打断连接.");
                    return;
                }
                var temconn = DBManager.Instance.GetEntity<Connectivity_n>(o => o.G3E_FID == temfid);
                if ("连接1" == t.SelectedNode.Text)
                {
                    if (conn.NODE1_ID == temconn.NODE1_ID)
                    { temconn.NODE1_ID = 0; TopologyMethods.ChangEntStatus(temconn, 1, "Del");  }
                    else if (conn.NODE1_ID == temconn.NODE2_ID)
                    {  temconn.NODE2_ID = 0; TopologyMethods.ChangEntStatus(temconn, 2, "Del"); }
                }
                else // 连接2，用节点2去比较
                {
                    if (conn.NODE2_ID == temconn.NODE1_ID)
                    { temconn.NODE1_ID = 0; TopologyMethods.ChangEntStatus(temconn, 1, "Del"); }
                    else if (conn.NODE2_ID == temconn.NODE2_ID)
                    { temconn.NODE2_ID = 0; TopologyMethods.ChangEntStatus(temconn, 2, "Del"); }
                }
                DBManager.Instance.Update(temconn);
            }

            // 特殊设备处理
            if (PublicMethod.Instance.N2is0.Concat(PublicMethod.Instance.N1isN2).Contains(conn.G3E_FNO))
            {
                conn.NODE1_ID = conn.NODE2_ID = 0;    
                TopologyMethods.ChangEntStatus(conn, 1, "Del");
                if (PublicMethod.Instance.N1isN2.Contains(conn.G3E_FNO)) { TopologyMethods.ChangEntStatus(conn, 2, "Del"); }   
            }
            else if ("连接1" == t.SelectedNode.Text)
            {
                conn.NODE1_ID = 0;
                TopologyMethods.ChangEntStatus(conn, 1, "Del");
            }
            else if ("连接2" == t.SelectedNode.Text)
            {
                conn.NODE2_ID = 0;
                TopologyMethods.ChangEntStatus(conn, 2, "Del");
            }
            TopologyMethods.sinNodDevJudge(conn);
            DBManager.Instance.Update(conn);
            SetConnViewNod(fid);
            TopologyMethods.bChangeTopo = true;
        }
        // 连接关系
        void insertConnect(long fid, TreeView t)
        {
            try
            {
                var conn = DBManager.Instance.GetEntity<Connectivity_n>(fid);
                if (null == conn)
                {
                    PublicMethod.Instance.Editor.WriteMessage("\nConnectivity_n没有对应fid: {0} 字段!\n", fid);
                    return;
                }
                PublicMethod.Instance.Editor.SetImpliedSelection(new ObjectId[0]);// 清空选择集  
                // 选择要连接的设备 
                long selid = 0, selfid = 0, selfno = 0;
                if (false == SelSigleEnt(ref selid, ref selfid, ref selfno)) return;
                if (selfno == 0)
                {
                    PublicMethod.Instance.ShowMessage("没有选择任何设备.");
                    return;
                }
                // 规范处理
                var bcon = CDDBManager.Instance.GetEntities<G3e_nodeedgeconnectivity>(o => o.G3E_SOURCEFNO == conn.G3E_FNO).Select(o => o.G3E_CONNECTINGFNO).Contains((int)selfno);
                if (false == bcon)
                {
                    var str1 = FeatureMapping.instance.features[conn.G3E_FNO.ToString()];
                    var str2 = FeatureMapping.instance.features[selfno.ToString()];
                    PublicMethod.Instance.Editor.WriteMessageWithReturn(str1 + " 不能与 " + str2 + "相连!\n");
                    return;
                }
                // 获取选中图元对应的实体
                var selconn = DBManager.Instance.GetEntities<Connectivity_n>(o => o.G3E_FID == selfid).FirstOrDefault();
                if (null == selconn)
                {
                    PublicMethod.Instance.Editor.WriteMessage("\n选中实体fid = {0} 在Connectivity_n中没有记录!\n", selfid);
                    return;
                }
                #region
                var selconns1 = DBManager.Instance.GetEntities<Connectivity_n>(o => o.NODE1_ID == selconn.NODE1_ID && o.G3E_FID != selfid && o.NODE1_ID != 0).Where(o => o.EntityState != EntityState.Delete && o.EntityState != EntityState.InsertDelete);
                var selconns2 = DBManager.Instance.GetEntities<Connectivity_n>(o => o.NODE2_ID == selconn.NODE1_ID && o.G3E_FID != selfid && o.NODE2_ID != 0).Where(o => o.EntityState != EntityState.Delete && o.EntityState != EntityState.InsertDelete);
                var selconns3 = DBManager.Instance.GetEntities<Connectivity_n>(o => o.NODE1_ID == selconn.NODE2_ID && o.G3E_FID != selfid && o.NODE1_ID != 0).Where(o => o.EntityState != EntityState.Delete && o.EntityState != EntityState.InsertDelete);
                var selconns4 = DBManager.Instance.GetEntities<Connectivity_n>(o => o.NODE2_ID == selconn.NODE2_ID && o.G3E_FID != selfid && o.NODE2_ID != 0).Where(o => o.EntityState != EntityState.Delete && o.EntityState != EntityState.InsertDelete);

                var cc = new ConnectivityForm
                {
                    symbols = new List<SymbolObject>(), 
                    nodes = new List<NodeObject>()
                };
                var no1 = new NodeObject
                {
                    oType = 0,
                    nFid = selconn.NODE1_ID == null ? "0" : selconn.NODE1_ID.ToString(),
                    nIndex = "1"
                };
                var no2 = new NodeObject
                {
                    oType = 0,
                    nFid = selconn.NODE2_ID == null ? "0" : selconn.NODE2_ID.ToString(),
                    nIndex = "2"
                };
                cc.nodes.Add(no1);
                cc.nodes.Add(no2);
                //var vv = DBManager.Instance.GetEntity<Common_n>(o => o.G3E_FID == fid);

                var sy = new SymbolObject();
                long g3eid = 0, g3efid = 0, g3efno = 0;
                DBEntityFinder.Instance.GetG3EIds(DBEntityFinder.Instance.GetObjectIdByFid(fid), ref g3eid, ref g3efid, ref g3efno);
                if (FeatureMapping.instance.features.ContainsKey(g3efno.ToString()))
                {
                    sy.sName = FeatureMapping.instance.features[g3efno.ToString()]
                                                    + " - "
                                                    + fid.ToString();
                }
                sy.oType = 1;
                sy.sFid = fid.ToString();
                //sy.sName = vv.SBMC;
                sy.sType = FeatureMapping.instance.features[g3efno.ToString()];
                sy.nodeIndex = "连接1" == t.SelectedNode.Text ? 1 : 2;
                cc.symbols.Add(sy);
                cc.sym1 = sy;

                //vv = DBManager.Instance.GetEntity<Common_n>(selfid);

                sy = new SymbolObject();
                DBEntityFinder.Instance.GetG3EIds(DBEntityFinder.Instance.GetObjectIdByFid(selfid), ref g3eid, ref g3efid, ref g3efno);
                if (FeatureMapping.instance.features.ContainsKey(g3efno.ToString()))
                {
                    sy.sName = FeatureMapping.instance.features[g3efno.ToString()]
                                                    + " - "
                                                    + fid.ToString();
                }

                sy.oType = 1;
                sy.sFid = selfid.ToString();
                sy.sType = FeatureMapping.instance.features[g3efno.ToString()];
                sy.node1 = no1;
                sy.node2 = no2;
                cc.symbols.Add(sy);
                cc.sym2 = sy;

                var noo1 = selconns1.Concat(selconns2).Distinct(new ElectronBaseCompare<Connectivity_n>()).OrderBy(o => o.G3E_FNO);
                var noo2 = selconns3.Concat(selconns4).Distinct(new ElectronBaseCompare<Connectivity_n>()).OrderBy(o => o.G3E_FNO);
                foreach (var v in noo1)
                {
                    var v1 = v;
                    var __vv = DBManager.Instance.GetEntities<Common_n>(o => o.G3E_FID == v1.G3E_FID).FirstOrDefault();
                    if (__vv != null)
                        sy = new SymbolObject
                        {
                            oType = 1,
                            sFid = __vv.G3E_FID.ToString(),
                            sName = __vv.SBMC,
                            sType = FeatureMapping.instance.features[__vv.G3E_FNO.ToString()],
                            node1 = no1
                        };
                    cc.symbols.Add(sy);
                }
                foreach (var v in noo2)
                {
                    var v1 = v;
                    var __vv = DBManager.Instance.GetEntities<Common_n>(o => o.G3E_FID == v1.G3E_FID).FirstOrDefault();
                    if (__vv != null)
                        sy = new SymbolObject
                        {
                            oType = 1,
                            sFid = __vv.G3E_FID.ToString(),
                            sName = __vv.SBMC,
                            sType = FeatureMapping.instance.features[__vv.G3E_FNO.ToString()],
                            node2 = no2
                        };
                    cc.symbols.Add(sy);
                }
                #endregion
                var diares = Autodesk.AutoCAD.ApplicationServices.Application.ShowModalDialog(cc);
                if (diares == DialogResult.Cancel) return;
                no1 = cc.sym1.getnode();
                if (null == no1) return;
                var str = no1.nIndex;

                // 工单判断
                if (DBEntityFinder.Instance.VerifyLTTID(selconn.G3E_FID) == false)
                {
                    if ("1" == str && (selconn.NODE1_ID == 0 || selconn.NODE1_ID == null))
                    {
                        PublicMethod.Instance.AlertDialog(selconn.G3E_FID + " 没有被工单锁定且节点1为0,连接失败.");
                        return;
                    }
                    if ("2" == str && (selconn.NODE2_ID == 0 || selconn.NODE2_ID == null))
                    {
                        PublicMethod.Instance.AlertDialog(selconn.G3E_FID + " 没有被工单锁定且节点2为0,连接失败.");
                        return;
                    }
                }

                // 4种情况，每种情况有4类  0-1 1-0 0-0 1-1
                if ("连接1" == t.SelectedNode.Text)
                {
                    //连接1 ---- 连接1
                    if ("1" == str)
                    {
                        Nod1ToNod1(conn, selconn);
                        //PublicMethod.Instance.AlertDialog("节点1与节点1不能相连\n");
                    }
                    //连接1 --- 连接2
                    else if ("2" == str)
                    {
                        Nod1ToNod2(conn, selconn);
                    }
                }
                else
                {
                    //连接2 --- 连接1
                    if ("1" == str)
                    {
                        Nod2ToNod1(conn, selconn);
                    }
                    //连接2 --- 连接2
                    else if ("2" == str)
                    {
                        Nod2ToNod2(conn, selconn);
                        //PublicMethod.Instance.AlertDialog("节点2与节点2不能相连\n");
                    }
                }
                // 特殊设备处理
                TopologyMethods.sinNodDevJudge(conn);
                TopologyMethods.sinNodDevJudge(selconn);
                DBManager.Instance.Update(conn);
                DBManager.Instance.Update(selconn);
                SetConnViewNod(fid);
                CheckTopology(fid);
                TopologyMethods.bChangeTopo = true;
            }
            catch (Exception ex) { PublicMethod.Instance.Editor.WriteMessageWithReturn(ex.Message + "\n"); }
        }
        void Nod1ToNod1(Connectivity_n conn, Connectivity_n selconn)
        {
            #region 1       0     -> 0
            if (conn.NODE1_ID == 0 && selconn.NODE1_ID == 0)
            {
                //新增节点
                selconn.NODE1_ID = conn.NODE1_ID = CYZCommonFunc.getid();
                TopologyMethods.ChangEntStatus(conn, 1, "Add");
                TopologyMethods.ChangEntStatus(selconn, 1, "Add");
            }
            #endregion
            #region 2       123 -> 0
            else if (conn.NODE1_ID != 0 && selconn.NODE1_ID == 0)
            {
                selconn.NODE1_ID = conn.NODE1_ID;
                var strSub = "";
                if (conn.EntityState.ToString().Length > 8)
                    strSub = conn.EntityState.ToString().Substring(4, 3);
                switch (selconn.EntityState)
                {                    
                    case EntityState.None:
                        if (conn.EntityState == EntityState.None || conn.EntityState == EntityState.Update)
                            selconn.EntityState = EntityState.Old_Old_Nal;
                        else if (strSub == "Nal")
                            selconn.EntityState = ChangEntStatus(EntityState.Old_Nal_Nal, 4, "Old");
                        else
                            selconn.EntityState = ChangEntStatus(EntityState.Old_Nal_Nal, 4, strSub);
                        break;
                    case EntityState.Update:
                        if (conn.EntityState == EntityState.None || conn.EntityState == EntityState.Update)
                            selconn.EntityState = EntityState.Old_Old_Nal;
                        else if (strSub == "Nal")
                            selconn.EntityState = ChangEntStatus(EntityState.Old_Nal_Nal, 4, "Old");
                        else
                            selconn.EntityState = ChangEntStatus(EntityState.Old_Nal_Nal, 4, strSub);
                        break;
                    case EntityState.Insert:
                        if (conn.EntityState == EntityState.None || conn.EntityState == EntityState.Update)
                            selconn.EntityState = EntityState.Add_Old_Nal;
                        else if (strSub == "Nal")
                            selconn.EntityState = ChangEntStatus(EntityState.Add_Nal_Nal, 4, "Old");
                        else
                            selconn.EntityState = ChangEntStatus(EntityState.Add_Nal_Nal, 4, strSub);
                        break;
                    default:
                        if (conn.EntityState == EntityState.None || conn.EntityState == EntityState.Update || strSub == "Nal")
                            selconn.EntityState = ChangEntStatus(selconn.EntityState, 4, "Old");
                        else
                            selconn.EntityState = ChangEntStatus(selconn.EntityState, 4, strSub);
                        break;
                }
            }
            #endregion
            #region 3       0     -> 123
            else if (conn.NODE1_ID == 0 && selconn.NODE1_ID != 0)
            {
                conn.NODE1_ID = selconn.NODE1_ID;
                var strSub = "";
                if (selconn.EntityState.ToString().Length > 8)
                    strSub = selconn.EntityState.ToString().Substring(4, 3);
                switch (conn.EntityState)
                {
                    case EntityState.None:
                        if (selconn.EntityState == EntityState.None || selconn.EntityState == EntityState.Update)
                            conn.EntityState = EntityState.Old_Old_Nal;
                        else if (strSub == "Nal")
                            conn.EntityState = ChangEntStatus(EntityState.Old_Nal_Nal, 4, "Old");
                        else
                            conn.EntityState = ChangEntStatus(EntityState.Old_Nal_Nal, 4, strSub);
                        break;
                    case EntityState.Update:
                        if (selconn.EntityState == EntityState.None || selconn.EntityState == EntityState.Update)
                            conn.EntityState = EntityState.Old_Old_Nal;
                        else if (strSub == "Nal")
                            conn.EntityState = ChangEntStatus(EntityState.Old_Nal_Nal, 4, "Old");
                        else
                            conn.EntityState = ChangEntStatus(EntityState.Old_Nal_Nal, 4, strSub);
                        break;
                    case EntityState.Insert:
                        if (selconn.EntityState == EntityState.None || selconn.EntityState == EntityState.None)
                            conn.EntityState = EntityState.Add_Old_Nal;
                        else if (strSub == "Nal")
                            conn.EntityState = ChangEntStatus(EntityState.Add_Nal_Nal, 4, "Old");
                        else
                            conn.EntityState = ChangEntStatus(EntityState.Add_Nal_Nal, 4, strSub);
                        break;
                    default:
                        if (selconn.EntityState == EntityState.None || selconn.EntityState == EntityState.Update || strSub == "Nal")
                            conn.EntityState = ChangEntStatus(conn.EntityState, 4, "Old");
                        else
                            conn.EntityState = ChangEntStatus(conn.EntityState, 4, strSub);
                        break;
                }
            }
            #endregion
            #region 4       123 -> 345
            else if (conn.NODE1_ID != 0 && selconn.NODE1_ID != 0)
            {
                PublicMethod.Instance.Editor.WriteMessage("\n连接错误:两个设备的连接点都已经有值, \n"
                                                                + conn.NODE1_ID
                                                                + "  "
                                                                + selconn.NODE1_ID
                                                                + "\n");
            }
            #endregion
        }
        void Nod1ToNod2(Connectivity_n conn, Connectivity_n selconn)
        {
            #region 1       0 -> 0
            if (conn.NODE1_ID == 0 && selconn.NODE2_ID == 0)
            {
                //新增节点
                selconn.NODE2_ID = conn.NODE1_ID = CYZCommonFunc.getid();
                TopologyMethods.ChangEntStatus(conn, 1, "Add");
                TopologyMethods.ChangEntStatus(selconn, 2, "Add");
            }
            #endregion
            #region 2       123 -> 0
            else if (conn.NODE1_ID != 0 && selconn.NODE2_ID == 0)
            {
                selconn.NODE2_ID = conn.NODE1_ID;

                var strSub = "";
                if (conn.EntityState.ToString().Length > 8)
                    strSub = conn.EntityState.ToString().Substring(4, 3);
                switch (selconn.EntityState)
                {
                    case EntityState.None:
                        if (conn.EntityState == EntityState.None || conn.EntityState == EntityState.Update)
                            selconn.EntityState = EntityState.Old_Nal_Old;
                        else if (strSub == "Nal")
                            selconn.EntityState = ChangEntStatus(EntityState.Old_Nal_Nal, 8, "Old");
                        else
                            selconn.EntityState = ChangEntStatus(EntityState.Old_Nal_Nal, 8, strSub);
                        break;
                    case EntityState.Update:
                        if (conn.EntityState == EntityState.None || conn.EntityState == EntityState.Update)
                            selconn.EntityState = EntityState.Old_Nal_Old;
                        else if (strSub == "Nal")
                            selconn.EntityState = ChangEntStatus(EntityState.Old_Nal_Nal, 8, "Old");
                        else
                            selconn.EntityState = ChangEntStatus(EntityState.Old_Nal_Nal, 8, strSub);
                        break;
                    case EntityState.Insert:
                        if (conn.EntityState == EntityState.None || conn.EntityState == EntityState.Update)
                            selconn.EntityState = EntityState.Add_Nal_Old;
                        else if (strSub == "Nal")
                            selconn.EntityState = ChangEntStatus(EntityState.Add_Nal_Nal, 8, "Old");
                        else
                            selconn.EntityState = ChangEntStatus(EntityState.Add_Nal_Nal, 8, strSub);
                        break;
                    default:
                        if (conn.EntityState == EntityState.None || conn.EntityState == EntityState.Update || strSub == "Nal")
                            selconn.EntityState = ChangEntStatus(selconn.EntityState, 8, "Old");
                        else
                            selconn.EntityState = ChangEntStatus(selconn.EntityState, 8, strSub);
                        break;
                }
            }
            #endregion
            #region 3       0 -> 123
            else if (conn.NODE1_ID == 0 && selconn.NODE2_ID != 0)
            {
                conn.NODE1_ID = selconn.NODE2_ID;

                var strSub = "";
                if (selconn.EntityState.ToString().Length > 8)
                    strSub = selconn.EntityState.ToString().Substring(8, 3);
                switch (conn.EntityState)
                {
                    case EntityState.None:
                        if (selconn.EntityState == EntityState.None)
                            conn.EntityState = EntityState.Old_Old_Nal;
                        else if (strSub == "Nal")
                            conn.EntityState = ChangEntStatus(EntityState.Old_Nal_Nal, 4, "Old");
                        else
                            conn.EntityState = ChangEntStatus(EntityState.Old_Nal_Nal, 4, strSub);
                        break;
                    case EntityState.Update:
                        if (selconn.EntityState == EntityState.None || selconn.EntityState == EntityState.Update)
                            conn.EntityState = EntityState.Old_Old_Nal;
                        else if (strSub == "Nal")
                            conn.EntityState = ChangEntStatus(EntityState.Old_Nal_Nal, 4, "Old");
                        else
                            conn.EntityState = ChangEntStatus(EntityState.Old_Nal_Nal, 4, strSub);
                        break;
                    case EntityState.Insert:
                        if (selconn.EntityState == EntityState.None || selconn.EntityState == EntityState.Update)
                            conn.EntityState = EntityState.Add_Old_Nal;
                        else if (strSub == "Nal")
                            conn.EntityState = ChangEntStatus(EntityState.Add_Nal_Nal, 4, "Old");
                        else
                            conn.EntityState = ChangEntStatus(EntityState.Add_Nal_Nal, 4, strSub);
                        break;
                    default:
                        if (selconn.EntityState == EntityState.None || selconn.EntityState == EntityState.Update || strSub == "Nal")
                            conn.EntityState = ChangEntStatus(conn.EntityState, 4, "Old");
                        else
                            conn.EntityState = ChangEntStatus(conn.EntityState, 4, strSub);
                        break;
                }
            }
            #endregion
            #region 4       123 -> 345
            else if (conn.NODE1_ID != 0 && selconn.NODE2_ID != 0)
            {
                PublicMethod.Instance.AlertDialog("连接错误:不能多个设备连接多个设备!\n"
                                                                + conn.NODE1_ID.ToString()
                                                                + "  "
                                                                + selconn.NODE2_ID.ToString()
                                                                + "\n");
            }
            #endregion
        }
        void Nod2ToNod1(Connectivity_n conn, Connectivity_n selconn)
        {
            #region 1       0 -> 0
            if (conn.NODE2_ID == 0 && selconn.NODE1_ID == 0)
            {
                //新增节点
                selconn.NODE1_ID = conn.NODE2_ID = CYZCommonFunc.getid();
                TopologyMethods.ChangEntStatus(conn, 2, "Add");
                TopologyMethods.ChangEntStatus(selconn, 1, "Add");
            }
            #endregion
            #region 2       123 -> 0
            else if (conn.NODE2_ID != 0 && selconn.NODE1_ID == 0)
            {
                selconn.NODE1_ID = conn.NODE2_ID;

                var strSub = "";
                if (conn.EntityState.ToString().Length > 8)
                    strSub = conn.EntityState.ToString().Substring(8, 3);
                switch (selconn.EntityState)
                {
                    case EntityState.None:
                        if (conn.EntityState == EntityState.None || conn.EntityState == EntityState.Update)
                            selconn.EntityState = EntityState.Old_Old_Nal;
                        else if (strSub == "Nal")
                            selconn.EntityState = ChangEntStatus(EntityState.Old_Nal_Nal, 4, "Old");
                        else
                            selconn.EntityState = ChangEntStatus(EntityState.Old_Nal_Nal, 4, strSub);
                        break;
                    case EntityState.Update:
                        if (conn.EntityState == EntityState.None || conn.EntityState == EntityState.Update)
                            selconn.EntityState = EntityState.Old_Old_Nal;
                        else if (strSub == "Nal")
                            selconn.EntityState = ChangEntStatus(EntityState.Old_Nal_Nal, 4, "Old");
                        else
                            selconn.EntityState = ChangEntStatus(EntityState.Old_Nal_Nal, 4, strSub);
                        break;
                    case EntityState.Insert:
                        if (conn.EntityState == EntityState.None || conn.EntityState == EntityState.Update)
                            selconn.EntityState = EntityState.Add_Old_Nal;
                        else if (strSub == "Nal")
                            selconn.EntityState = ChangEntStatus(EntityState.Add_Nal_Nal, 4, "Old");
                        else
                            selconn.EntityState = ChangEntStatus(EntityState.Add_Nal_Nal, 4, strSub);
                        break;
                    default:
                        if (conn.EntityState == EntityState.None || conn.EntityState == EntityState.Update || strSub == "Nal")
                            selconn.EntityState = ChangEntStatus(selconn.EntityState, 4, "Old");
                        else
                            selconn.EntityState = ChangEntStatus(selconn.EntityState, 4, strSub);
                        break;
                }
            }
            #endregion
            #region 3       0 -> 123
            else if (conn.NODE2_ID == 0 && selconn.NODE1_ID != 0)
            {
                conn.NODE2_ID = selconn.NODE1_ID;

                var strSub = "";
                if (selconn.EntityState.ToString().Length > 8)
                    strSub = selconn.EntityState.ToString().Substring(4, 3);
                switch (conn.EntityState)
                {
                    case EntityState.None:
                        if (selconn.EntityState == EntityState.None || selconn.EntityState == EntityState.Update)
                            conn.EntityState = EntityState.Old_Nal_Old;
                        else if (strSub == "Nal")
                            conn.EntityState = ChangEntStatus(EntityState.Old_Nal_Nal, 8, "Old");
                        else
                            conn.EntityState = ChangEntStatus(EntityState.Old_Nal_Nal, 8, strSub);
                        break;
                    case EntityState.Update:
                        if (selconn.EntityState == EntityState.None || selconn.EntityState == EntityState.Update)
                            conn.EntityState = EntityState.Old_Nal_Old;
                        else if (strSub == "Nal")
                            conn.EntityState = ChangEntStatus(EntityState.Old_Nal_Nal, 8, "Old");
                        else
                            conn.EntityState = ChangEntStatus(EntityState.Old_Nal_Nal, 8, strSub);
                        break;
                    case EntityState.Insert:
                        if (selconn.EntityState == EntityState.None || selconn.EntityState == EntityState.Update)
                            conn.EntityState = EntityState.Add_Nal_Old;
                        else if (strSub == "Nal")
                            conn.EntityState = ChangEntStatus(EntityState.Add_Nal_Nal, 8, "Old");
                        else
                            conn.EntityState = ChangEntStatus(EntityState.Add_Nal_Nal, 8, strSub);
                        break;
                    default:
                        if (selconn.EntityState == EntityState.None || selconn.EntityState == EntityState.Update || strSub == "Nal")
                            conn.EntityState = ChangEntStatus(conn.EntityState, 8, "Old");
                        else
                            conn.EntityState = ChangEntStatus(conn.EntityState, 8, strSub);
                        break;
                }
            }
            #endregion
            #region 4       123 -> 345
            else if (conn.NODE2_ID != 0 && selconn.NODE1_ID != 0)
            {
                PublicMethod.Instance.AlertDialog("连接错误:不能多个设备连接多个设备!\n"
                                                                + conn.NODE2_ID.ToString()
                                                                + "  "
                                                                + selconn.NODE1_ID.ToString()
                                                                + "\n");
            }
            #endregion
        }
        void Nod2ToNod2(Connectivity_n conn, Connectivity_n selconn)
        {
            #region 1        0 -> 0
            if (conn.NODE2_ID == 0 && selconn.NODE2_ID == 0)
            {
                selconn.NODE2_ID = conn.NODE2_ID = CYZCommonFunc.getid();
                TopologyMethods.ChangEntStatus(conn, 2, "Add");
                TopologyMethods.ChangEntStatus(selconn, 2, "Add");
            }
            #endregion
            #region 2       123 -> 0
            else if (conn.NODE2_ID != 0 && selconn.NODE2_ID == 0)
            {
                selconn.NODE2_ID = conn.NODE2_ID;

                var strSub = "";
                if (conn.EntityState.ToString().Length > 8)
                    strSub = conn.EntityState.ToString().Substring(8, 3);
                switch (selconn.EntityState)
                {
                    case EntityState.None:
                        if (conn.EntityState == EntityState.None || conn.EntityState == EntityState.Update)
                            selconn.EntityState = EntityState.Old_Nal_Old;
                        else if (strSub == "Nal")
                            selconn.EntityState = ChangEntStatus(EntityState.Old_Nal_Nal, 8, "Old");
                        else
                            selconn.EntityState = ChangEntStatus(EntityState.Old_Nal_Nal, 8, strSub);
                        break;
                    case EntityState.Update:
                        if (conn.EntityState == EntityState.None || conn.EntityState == EntityState.Update)
                            selconn.EntityState = EntityState.Old_Nal_Old;
                        else if (strSub == "Nal")
                            selconn.EntityState = ChangEntStatus(EntityState.Old_Nal_Nal, 8, "Old");
                        else
                            selconn.EntityState = ChangEntStatus(EntityState.Old_Nal_Nal, 8, strSub);
                        break;
                    case EntityState.Insert:
                        if (conn.EntityState == EntityState.None || conn.EntityState == EntityState.Update)
                            selconn.EntityState = EntityState.Add_Nal_Old;
                        else if (strSub == "Nal")
                            selconn.EntityState = ChangEntStatus(EntityState.Add_Nal_Nal, 8, "Old");
                        else
                            selconn.EntityState = ChangEntStatus(EntityState.Add_Nal_Nal, 8, strSub);
                        break;
                    default:
                        if (conn.EntityState == EntityState.None || conn.EntityState == EntityState.Update || strSub == "Nal")
                            selconn.EntityState = ChangEntStatus(selconn.EntityState, 8, "Old");
                        else
                            selconn.EntityState = ChangEntStatus(selconn.EntityState, 8, strSub);
                        break;
                }
            }
            #endregion
            #region 3       0 -> 123
            else if (conn.NODE2_ID == 0 && selconn.NODE2_ID != 0)
            {
                conn.NODE2_ID = selconn.NODE2_ID;

                var strSub = "";
                if (selconn.EntityState.ToString().Length > 8)
                    strSub = selconn.EntityState.ToString().Substring(8, 3);
                switch (conn.EntityState)
                {
                    case EntityState.None:
                        if (selconn.EntityState == EntityState.None || selconn.EntityState == EntityState.Update)
                            conn.EntityState = EntityState.Old_Nal_Old;
                        else if (strSub == "Nal")
                            conn.EntityState = ChangEntStatus(EntityState.Old_Nal_Nal, 8, "Old");
                        else
                            conn.EntityState = ChangEntStatus(EntityState.Old_Nal_Nal, 8, strSub);
                        break;
                    case EntityState.Update:
                        if (selconn.EntityState == EntityState.None || selconn.EntityState == EntityState.Update)
                            conn.EntityState = EntityState.Old_Nal_Old;
                        else if (strSub == "Nal")
                            conn.EntityState = ChangEntStatus(EntityState.Old_Nal_Nal, 8, "Old");
                        else
                            conn.EntityState = ChangEntStatus(EntityState.Old_Nal_Nal, 8, strSub);
                        break;
                    case EntityState.Insert:
                        if (selconn.EntityState == EntityState.None || selconn.EntityState == EntityState.Update)
                            conn.EntityState = EntityState.Add_Nal_Old;
                        else if (strSub == "Nal")
                            conn.EntityState = ChangEntStatus(EntityState.Add_Nal_Nal, 8, "Old");
                        else
                            conn.EntityState = ChangEntStatus(EntityState.Add_Nal_Nal, 8, strSub);
                        break;
                    default:
                        if (selconn.EntityState == EntityState.None || selconn.EntityState == EntityState.Update || strSub == "Nal")
                            conn.EntityState = ChangEntStatus(conn.EntityState, 8, "Old");
                        else
                            conn.EntityState = ChangEntStatus(conn.EntityState, 8, strSub);
                        break;
                }
            }
            #endregion
            #region 4       123 -> 345
            else if (conn.NODE2_ID != 0 && selconn.NODE2_ID != 0)
            {
                PublicMethod.Instance.AlertDialog("连接错误:不能多个设备连接多个设备!\n"
                                                                + conn.NODE2_ID
                                                                + "  "
                                                                + selconn.NODE2_ID
                                                                + "\n");
            }
            #endregion
        }

        // 从属于设备
        void belong2Area(long fid)
        {
            var comm = DBManager.Instance.GetEntities<Common_n>(o => o.G3E_FID == fid).FirstOrDefault();
            if (null == comm)
            {
                PublicMethod.Instance.AlertDialog("数据源中找不到fid= " + fid + " 设备!\n");
                return;
            }
            // 选择一个实体
            long selid = 0, selfid = 0, selfno = 0;
            if (false == SelSigleEnt(ref selid, ref selfid, ref selfno))
                return;
            if (selfno == 0)
            {
                PublicMethod.Instance.ShowMessage("没有选择任何设备.");
                return;
            }
            //做选择设备是否面设备判断，规范！
            var bower = CDDBManager.Instance.GetEntities<G3e_ownership>(o => o.G3E_SOURCEFNO == comm.G3E_FNO).Select(o => o.G3E_OWNERFNO).Contains((int)selfno);
            if (false == bower)
            {
                var str1 = FeatureMapping.instance.features[comm.G3E_FNO.ToString()];
                var str2 = FeatureMapping.instance.features[selfno.ToString()];
                PublicMethod.Instance.AlertDialog(str1 + " 不能从属于 " + str2);
                return;
            }
            var fcomm = DBManager.Instance.GetEntities<Common_n>(o => o.G3E_FID == selfid).FirstOrDefault();
            if (null == fcomm)
            {
                PublicMethod.Instance.AlertDialog("数据源Common_n中找不到fid= " + selfid.ToString() + " 设备!\n");
                return;
            }
            // 根据功能位置判断从属
            var er = OwnerMethods.BelongByAZWZ(comm, fcomm);
            if (!string.IsNullOrEmpty(er))
            {
                PublicMethod.Instance.AlertDialog(er);
                return;
            }
            comm.OWNER1_ID = fcomm.G3E_ID;
            comm.OWNER2_ID = 0;
            //如果设备是台架变压器, 特殊处理
            OwnerMethods.TJCommChange(comm.G3E_FID, fcomm.G3E_ID);
            // 更新数据源 
            if (comm.EntityState == EntityState.None)
            {
                comm.EntityState = EntityState.Update;
            }
            DBManager.Instance.Update(comm);
            SetOwnerViewNod(fid);
            CheckTopology(fid);
        }
        // 删除从属关系
        void delDevOwner(long fid, TreeView t)
        {
            var comm = DBManager.Instance.GetEntities<Common_n>(o => o.G3E_FID == fid).FirstOrDefault();
            var updatefid = fid;
            if (t.SelectedNode.Text != "从属于设备")
            {
                Common_n fcomm;
                if (comm != null && (null != comm.OWNER1_ID && 0 != comm.OWNER1_ID))
                {
                    fcomm = DBManager.Instance.GetEntities<Common_n>(o => o.G3E_ID == comm.OWNER1_ID).FirstOrDefault();
                }
                else
                {
                    fcomm = DBManager.Instance.GetEntities<Common_n>(o => o.G3E_ID == comm.OWNER2_ID).FirstOrDefault();
                }
                if (fcomm != null) updatefid = fcomm.G3E_FID;
            }
            if (comm != null && comm.G3E_FNO != 198) //198开关柜不能在这删除
            {
                comm.OWNER1_ID = 0;
                comm.OWNER2_ID = 0;
                // 更新数据源 
                if (comm.EntityState == EntityState.None)
                {
                    comm.EntityState = EntityState.Update;
                }                
                DBManager.Instance.Update(comm);
                //如果设备是台架变压器, 特殊处理
                OwnerMethods.TJCommChange(comm.G3E_FID, 0);
            }
            SetOwnerViewNod(updatefid);
        }
        // 添加从属设备
        void addDev2Area(long fid)
        {
            var fcomm = DBManager.Instance.GetEntities<Common_n>(o => o.G3E_FID == fid).FirstOrDefault();
            if (null == fcomm)
            {
                PublicMethod.Instance.AlertDialog("数据源中找不到fid= " + fid + " 设备!\n");
                return;
            }
            // 选择
            long selid = 0, selfid = 0, selfno = 0;
            if (false == SelSigleEnt(ref selid, ref selfid, ref selfno)) return;
            if (selfno == 0)
            {
                PublicMethod.Instance.ShowMessage("没有选择任何设备.");
                return;
            }
            // 工单判断
            if (DBEntityFinder.Instance.VerifyLTTID(selfid) == false)
            {
                PublicMethod.Instance.AlertDialog("与此设备相连的设备没有被工单锁定,不能添加从属设备.");
                return;
            }

            // 规范：判断设备是否可从属于设备fcomm
            var _bower = CDDBManager.Instance.GetEntities<G3e_ownership>(o => o.G3E_OWNERFNO == fcomm.G3E_FNO).Select(o => o.G3E_SOURCEFNO).Contains((int)selfno);
            if (false == _bower)
            {
                var str1 = FeatureMapping.instance.features[fcomm.G3E_FNO.ToString()];
                var str2 = FeatureMapping.instance.features[selfno.ToString()];
                PublicMethod.Instance.AlertDialog(str2 + " 不能从属于 " + str1);
                return;
            }

            // 获取实体对象
            var comm = DBManager.Instance.GetEntities<Common_n>(o => o.G3E_FID == selfid).FirstOrDefault();
            if (null == comm)
            {
                PublicMethod.Instance.AlertDialog("数据源中找不到fid= " + selfid.ToString() + " 设备!\n");
                return;
            }
            // 根据功能位置判断从属
            var er = OwnerMethods.BelongByAZWZ(comm, fcomm);
            if (!string.IsNullOrEmpty(er))
            {
                PublicMethod.Instance.AlertDialog(er);
                return;
            }
            comm.OWNER1_ID = fcomm.G3E_ID;
            comm.OWNER2_ID = 0;
            // 更新数据源 
            if (comm.EntityState == EntityState.None)
            {
                comm.EntityState = EntityState.Update;
            }
            DBManager.Instance.Update(comm);
            //如果设备是台架变压器, 特殊处理
            OwnerMethods.TJCommChange(comm.G3E_FID, fcomm.G3E_ID);
            SetOwnerViewNod(fcomm.G3E_FID);
        }
 
        // 批量添加从属设备
        void addDevs2Area(long ffid)
        {                           
            var fcomm = DBManager.Instance.GetEntities<Common_n>(o => o.G3E_FID == ffid).FirstOrDefault();
            if (null == fcomm) return;
            g_fno = fcomm.G3E_FNO;
            selRule = SelectRule.Owner;
            var objs = SelMultiEnt();
            if (objs.Count() == 0) return;

            long id = 0, fid = 0, fno = 0;
            foreach (var idd in objs)
            {
                DBEntityFinder.Instance.GetG3EIds(idd, ref id, ref fid, ref fno);
                // 获取实体对象
                var fid1 = fid;
                var comm = DBManager.Instance.GetEntities<Common_n>(o => o.G3E_FID == fid1).FirstOrDefault();
                if (null == comm) return;
                // 根据功能位置判断从属
                var er = OwnerMethods.BelongByAZWZ(comm, fcomm);
                if (!string.IsNullOrEmpty(er))
                {
                    PublicMethod.Instance.ShowMessage(er);
                    continue;
                }
                comm.OWNER1_ID = fcomm.G3E_ID;
                comm.OWNER2_ID = 0;
                if (comm.EntityState == EntityState.None)  comm.EntityState = EntityState.Update;
                DBManager.Instance.Update(comm);
                //如果设备是台架变压器, 特殊处理
                OwnerMethods.TJCommChange(comm.G3E_FID, fcomm.G3E_FID);
                CheckTopology(fid);
            }
            //if (fcomm.EntityState == EntityState.None) fcomm.EntityState = EntityState.Update; //fcomm 没有做任何修改
            //DBManager.Instance.Update(fcomm);
            SetOwnerViewNod(ffid);
            CheckTopology(ffid);
        }
        // 批量打断从属关系
        public void delManyOwner(long ffid)
        {
            var db = HostApplicationServices.WorkingDatabase;
            var ed = db.GetEditor();
            // 清空选择集,选择设备
            ed.SetImpliedSelection(new ObjectId[0]);
            var fcomm = DBManager.Instance.GetEntities<Common_n>(o => o.G3E_FID == ffid).FirstOrDefault();
            if (fcomm == null) return;
            g_fno = fcomm.G3E_FNO;
            selRule = SelectRule.Owner;
            var objs = SelMultiEnt();  // 选择对象

            long id = 0, fid = 0, fno = 0;
            foreach (var idd in objs)
            {
                // 获取实体对象
                DBEntityFinder.Instance.GetG3EIds(idd, ref id, ref fid, ref fno);
                if (fno == 198) continue;
                var fid1 = fid;
                // 工单判断
                if (DBEntityFinder.Instance.VerifyLTTID(fid))
                {
                    var comm = DBManager.Instance.GetEntities<Common_n>(o => o.G3E_FID == fid1).FirstOrDefault();
                    if (null == comm) continue;
                    // 不能删除其他房的从属设备
                    if (fcomm.G3E_ID != comm.OWNER1_ID && fcomm.G3E_ID != comm.OWNER2_ID) continue;
                    comm.OWNER1_ID = 0;
                    comm.OWNER2_ID = 0;
                    if (comm.EntityState == EntityState.None) comm.EntityState = EntityState.Update;
                    DBManager.Instance.Update(comm);
                    //如果设备是台架变压器, 特殊处理
                    OwnerMethods.TJCommChange(comm.G3E_FID, 0);
                    CheckTopology(fid);
                }
                else
                {
                    PublicMethod.Instance.ShowMessage(fid + " 设备没有被工单锁定。");
                }
            }
            SetOwnerViewNod(ffid);
        }
        // 
        private void BreakConMenuStrip_ItemClicked(object sender, ToolStripItemClickedEventArgs e)
        {
            try
            {
                var ms = sender as ContextMenuStrip;
                if (ms != null)
                {
                    var sourceCtr = (TreeView)ms.SourceControl;
                    var brefid = (long)ms.Tag;
                    var fid = (long)sourceCtr.Tag;
                    // 打断单个设备
                    var tx = sourceCtr.SelectedNode.Parent.Text;
                    BreakConMenuStrip.Hide();// 切换焦点到cad窗口
                    if (DBEntityFinder.Instance.VerifyLTTID(brefid))
                    {
                        TopologyMethods.BreakNodeByFid(fid, brefid, tx);
                        SetConnViewNod(fid);
                    }
                    else
                    {
                        PublicMethod.Instance.AlertDialog("该设备没有被工单锁定.");
                    }
                }
            }
            catch (Exception ex)
            {
                PublicMethod.Instance.AlertDialog(ex.Message);
            }
        }

        // 包含
        void contain(long fid)
        {
            // 选择设备
            long selId = 0, selFid = 0, selFno = 0;
            if (!SelSigleEnt(ref selId, ref selFid, ref selFno)) return;
            if (selFno == 0)
            {
                PublicMethod.Instance.ShowMessage("没有选择任何设备.");
                return;
            }
            if (141 == selFno)
            {
                ContainMethods.addLine(fid, selFid);
                SetContainViewNod(fid);
                CheckTopology(fid);
            }
            else
            {
                PublicMethod.Instance.AlertDialog("选择错误!\n只有10kv 导线能被包含\n");
            }
            // 判断能否包含
        }
        // 包含-批量
        void contains(long fid)
        {
            selRule = SelectRule.Line;
            var selObjIdCol = SelMultiEnt();
            if (selObjIdCol != null) ContainMethods.addLine(fid, selObjIdCol);
            SetContainViewNod(fid);
            CheckTopology(fid);
        }
        // 被包含
        void beContain(long fid)
        {
            // 获取设备信息
            long selId = 0, selFid = 0, selFno = 0;
            if (false == SelSigleEnt(ref selId, ref selFid, ref selFno))
                return;
            if (selFno == 0)
            {
                PublicMethod.Instance.ShowMessage("没有选择任何设备.");
                return;
            }
            // 判断能否被包含
            if (201 == selFno)
            {
                ContainMethods.addRole(fid, selFid);
                SetContainViewNod(fid);
                CheckTopology(fid);
            }
            else
            {
                PublicMethod.Instance.AlertDialog("选择错误!\n只能被杆塔所包含!\n");
            }
        }
        // 被包含-批量
        void beContains(long fid)
        {
            selRule = SelectRule.Role;
            var selObjIdCol = SelMultiEnt();
            if (selObjIdCol != null) ContainMethods.addRole(fid, selObjIdCol);
            SetContainViewNod(fid);
            CheckTopology(fid);
        }
        // 被包含-自动
        void beContainAuto(long fid)
        {
            try
            {
                var lineid = DBEntityFinder.Instance.GetObjectIdByFid(fid);
                ContainMethods.AutoContainByPline(lineid);
            }
            finally
            {
                SetContainViewNod(fid);
                CheckTopology(fid);
            }
        }

        // 删除线
        void delLine(long fid, TreeView t)
        {
            if (DBEntityFinder.Instance.VerifyLTTID(fid))
            {
                var vv =
                    DBManager.Instance.GetEntity<Contain_n>(
                        o =>
                            o.G3E_FID == fid && o.G3E_OWNERFID == long.Parse(t.SelectedNode.Parent.Name) &&
                            o.EntityState != EntityState.Delete);
                if (vv.EntityState == EntityState.None)
                {
                    vv.EntityState = EntityState.Delete;
                    DBManager.Instance.Update(vv);
                }
                else
                {
                    DBManager.Instance.Delete(vv);
                }
                SetContainViewNod(long.Parse(t.SelectedNode.Parent.Name));
            }
            else  { PublicMethod.Instance.AlertDialog("此设备没有被工单锁定,不能删除此线."); }
        }
        // 删除杆
        void delRole(TreeView t)
        {
            var fid = long.Parse(t.SelectedNode.Parent.Name);
            var ownerid = long.Parse(t.SelectedNode.Name);
            var vv = DBManager.Instance.GetEntity<Contain_n>(
                o => o.G3E_FID == fid
                     && o.G3E_OWNERFID == ownerid
                     && o.EntityState != EntityState.Delete);
            if (vv.EntityState == EntityState.None)
            {
                vv.EntityState = EntityState.Delete;
                DBManager.Instance.Update(vv);
            }
            else
            {
                DBManager.Instance.Delete(vv);
            }
            DBManager.Instance.Update(vv);
            SetContainViewNod(long.Parse(t.SelectedNode.Parent.Name));
        }

        /// 选择一个实体
        bool SelSigleEnt(ref long selid, ref long selfid, ref  long selfno)
        {
            var db = HostApplicationServices.WorkingDatabase;
            var ed = db.GetEditor();
            //使用GetSelection，有过滤事件，只能获取最后一个，且选择中颜色变色,右键或者回车结束选择，选择后，实体不返回，存储在静态变量 g_objid中,下一次调用SelEnt将清空 g_objid
            try
            {
                ed.SetImpliedSelection(new ObjectId[0]); // 清空选择集
                // 设置选择消息
                mouse.MouseEventOff();
                ed.SelectionAdded += OnSigleSel;
                // 注意  初始化上一次的选择
                g_objid = ObjectId.Null;
                g_objind = 0;
                // 选择要连接的设备
                PromptSelectionResult _pentres;
                do
                {
                    _pentres = PublicMethod.Instance.Editor.GetSelection();
                } while (PromptStatus.Error != _pentres.Status);
                //清空选择完成后的颜色
                DCadApi.isModifySymbol = true;
                using (var trans = db.TransactionManager.StartTransaction())
                {
                    using (db.GetDocument().LockDocument())
                    {
                        if (ObjectId.Null != g_objid && 0 != g_objind)
                        {
                            var entity = (Entity)trans.GetObject(g_objid, OpenMode.ForRead, true);
                            entity.UpgradeOpenAndRun();
                            if (null != entity)
                                entity.ColorIndex = g_objind;
                        }
                    }
                    trans.Commit();
                }
                // 恢复消息
                ed.SelectionAdded -= OnSigleSel;
                mouse.MouseEventAdd();

                if (ObjectId.Null == g_objid)   return false;
                if (false == DBEntityFinder.Instance.GetG3EIds(g_objid, ref selid, ref selfid, ref selfno))
                {
                    PublicMethod.Instance.AlertDialog("此设备没有扩展属性！\n");
                }
                return true;
            }
            catch (Exception ex)
            {
                PublicMethod.Instance.AlertDialog(ex.Message);
                return true;
            }
            finally { DCadApi.isModifySymbol = false; }
        }
        // 选择单个过滤事件
        void OnSigleSel(object sender, SelectionAddedEventArgs e)
        {
            try
            {
                var db = HostApplicationServices.WorkingDatabase;
                using (var trans = db.TransactionManager.StartTransaction())
                {
                    using (db.GetDocument().LockDocument())
                    {
                        //PromptSelectionResult acSSPrompt = PublicMethod.Instance.Editor.SelectImplied();
                        var selectedEntityIds = e.AddedObjects.GetObjectIds(); //集合的ID

                        if (e.AddedObjects.Count > 0)
                        {
                            DCadApi.isModifySymbol = true;
                            Entity selectedEntity;
                            if (ObjectId.Null != g_objid)
                            {
                                selectedEntity = (Entity)trans.GetObject(g_objid, OpenMode.ForRead);
                                selectedEntity.UpgradeOpenAndRun();
                                selectedEntity.ColorIndex = g_objind;
                            }
                            var selectedEntityId = selectedEntityIds[e.AddedObjects.Count - 1];
                            if (PublicMethod.Instance.lockFrameObjectID != selectedEntityId)
                            {
                                selectedEntity = (Entity)trans.GetObject(selectedEntityId, OpenMode.ForWrite);
                                long id = 0, fno = 0, fid = 0;
                                DBEntityFinder.Instance.GetG3EIds(selectedEntityId, ref id, ref fid, ref fno);
                                if (fid != presel_fid)
                                {
                                    g_objid = selectedEntityId;
                                    g_objind = selectedEntity.ColorIndex;
                                    selectedEntity.ColorIndex = 4;
                                }
                            }
                        }
                        // 移除选择集中的对象
                        for (var entityIndex = 0; entityIndex < e.AddedObjects.Count; entityIndex++)
                        {
                            e.Remove(entityIndex);
                        }
                    }
                    trans.Commit();
                }
            }
            catch (Exception ex) {  PublicMethod.Instance.Editor.WriteMessageWithReturn(ex.Message + "\n"); }
            finally  { DCadApi.isModifySymbol = false; }

        }

        // 选择多个实体
        ObjectId[] SelMultiEnt()
        {
            var db = HostApplicationServices.WorkingDatabase;
            var ed = db.GetEditor();
            db.GetDocument();
            var objs = new ObjectId[0];
            try
            {
                ed.SetImpliedSelection(new ObjectId[0]); // 清空选择集  
                mouse.MouseEventOff(); // 去除鼠标事件
                ed.SelectionAdded += OnMultiSel;
                var psr = PublicMethod.Instance.Editor.GetSelection();
                if (psr.Status == PromptStatus.OK) objs = psr.Value.GetObjectIds();

                // 还原颜色
                DCadApi.isModifySymbol = true;
                using (var tr = db.TransactionManager.StartTransaction())
                {
                    using (db.GetDocument().LockDocument())
                    {
                        foreach (var e in g_ColDict)
                        {
                            var ent = tr.GetObject(e.Key, OpenMode.ForRead, false) as Entity;
                            ent.UpgradeOpenAndRun();
                            if (ent != null) ent.ColorIndex = e.Value;
                        }
                        g_ColDict.Clear();
                        tr.Commit();
                    }
                }
                ed.SelectionAdded -= OnMultiSel;
                mouse.MouseEventAdd(); // 增加鼠标事件
                ed.SetImpliedSelection(new ObjectId[0]);// 清空选择集  
            }
            catch (Exception ex)
            {
                ed.WriteMessageWithReturn(ex);
            }
            finally { DCadApi.isModifySymbol = false; }
            return objs;
        }
        // 选择多个过滤事件
        void OnMultiSel(object sender, SelectionAddedEventArgs e)
        {
            var db = HostApplicationServices.WorkingDatabase;
            var selectedEntityIds = e.AddedObjects.GetObjectIds(); //集合的ID
            var ids = new List<ObjectId>();
            //ObjectIdCollection ids = new ObjectIdCollection();
            if (e.AddedObjects.Count > 0)
            {
                long id = 0, fid = 0, fno = 0;
                foreach (var eid in selectedEntityIds)
                {
                    if (DBEntityFinder.Instance.VerifyLTTID(eid) == false) continue;
                    if (PublicMethod.Instance.lockFrameObjectID == eid) continue;
                    DBEntityFinder.Instance.GetG3EIds(eid, ref id, ref fid, ref fno);
                    var bower = false;
                    switch (selRule)
                    {
                        case SelectRule.Owner:
                        {
                            bower = CDDBManager.Instance.GetEntities<G3e_ownership>(o => o.G3E_OWNERFNO == g_fno)
                                .Select(o => o.G3E_SOURCEFNO)
                                .Contains((int) fno);
                        }
                            break;
                        case SelectRule.Role:
                        {
                            bower = (201 == fno);
                        }
                            break;
                        case SelectRule.Line:
                        {
                            bower = (141 == fno);
                        }
                            break;
                    }
                    if (false == bower) continue;
                    if (!ids.Contains(eid)) ids.Add(eid);
                }
            }
            if (ids.Count != 0)
            {
                using (var tr = db.TransactionManager.StartTransaction())
                {
                    using (db.GetDocument().LockDocument())
                    {
                        var dbTextIds = new List<ObjectId>();
                        foreach (var eid in ids)
                        {
                            var ent = tr.GetObject(eid, OpenMode.ForRead, false) as Entity;
                            if (ent == null) continue;
                            if (ent is DBText)
                            {
                                dbTextIds.Add(eid);
                                continue;
                            }
                            ent.UpgradeOpenAndRun();
                            g_ColDict.Add(eid, ent.ColorIndex);
                            ent.ColorIndex = 4;
                        }
                        foreach (var eid in dbTextIds)
                        {
                            ids.Remove(eid);
                        }
                    }
                    tr.Commit();
                }
            }
            //// 去除不符合规范的设备
            for (var entityIndex = 0; entityIndex < e.AddedObjects.Count; entityIndex++)
            {
                var entId = e.AddedObjects.GetObjectIds()[entityIndex];
                if (!ids.Contains(entId))
                {
                    e.Remove(entityIndex);
                }
            }
        }

        // 改变实体的EntityState状态
        EntityState ChangEntStatus(EntityState entState, int startIndex, string strSubState)
        {
            var strStatus = entState.ToString();
            strStatus = strStatus.Remove(startIndex, 3);
            strStatus = strStatus.Insert(startIndex, strSubState);
            entState = (EntityState)Enum.Parse(typeof(EntityState), strStatus);
            return entState;
        }

        static public void CheckTopology(long fid)
        {
            if (MenuControl.orderVerify != null)
            {
                var va = new orderWorkifyArgs { FID = fid };
                if (connectivityVerify != null)
                {
                    connectivityVerify(new object(), va);
                }
            }
        }

    }
}
