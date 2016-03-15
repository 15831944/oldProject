using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Windows.Forms;
using Autodesk.AutoCAD.ApplicationServices;
using Autodesk.AutoCAD.DatabaseServices;
using Autodesk.AutoCAD.EditorInput;
using DotNetARX;
using ElectronTransferDal.Cad;
using ElectronTransferDal.Common;
using ElectronTransferDal.Query;
using ElectronTransferModel;
using ElectronTransferModel.V9_4;
using ElectronTransferView.ConnectivityManager;

namespace ElectronTransferView.TopologyManager
{
    public partial class ConnectBySel : UserControl
    {
        /// <summary>
        /// 要显示在view上的列表字典
        /// </summary>
        static public ObjectIdList objList = new ObjectIdList();
        /// <summary>
        /// 记录已经变色的设备字典
        /// </summary>
        static public Dictionary<ObjectId, int> objColDict = new Dictionary<ObjectId, int>();
        // 新选择的对象字典
        //static public ObjectIdList u2clist = new ObjectIdList();

        public ConnectBySel()
        {
            InitializeComponent();
        }
        //public ConnectBySel(ObjectIdList nodeList)
        //{
        //    InitializeComponent();
        //    InitializeList(nodeList);
        //}
        private void btn_up_Click(object sender, EventArgs e)
        {
            try
            {
                ConnlistView.BeginUpdate();
                if (ConnlistView.Items.Count !=0 )
                {
                    var curItem = ConnlistView.SelectedItems[0];
                    var index = curItem.Index;
                    if (index > 0)
                    {
                        //var tem = (ObjectId)curItem.Tag;
                        ConnlistView.Items.RemoveAt(index);
                        ConnlistView.Items.Insert(index - 1, curItem);
                        ConnlistView.Focus();
                        ConnlistView.Items[index - 1].Selected = true;
                        ConnlistView.Items[index - 1].Focused = true;
                        ConnlistView.Items[index - 1].EnsureVisible();
                    }
                }
                UpdateList();
                ConnlistView.EndUpdate();
            }
            catch (Exception ex)
            {
                PublicMethod.Instance.AlertDialog(ex.Message);
            }
        }
        private void btn_down_Click(object sender, EventArgs e)
        {
            try
            {
                ConnlistView.BeginUpdate();
                if (ConnlistView.Items.Count != 0)
                {
                    var curItem = ConnlistView.SelectedItems[0];
                    var index = curItem.Index;
                    if (index < ConnlistView.Items.Count - 1)
                    {
                        //var tem = (ObjectId)curItem.Tag;
                        ConnlistView.Items.RemoveAt(index);
                        ConnlistView.Items.Insert(index + 1, curItem);
                        ConnlistView.Focus();
                        ConnlistView.Items[index + 1].Selected = true;
                        ConnlistView.Items[index + 1].Focused = true;
                        ConnlistView.Items[index + 1].EnsureVisible();
                    }
                }
                UpdateList();
                ConnlistView.EndUpdate();
            }
            catch (Exception exception)
            {
                PublicMethod.Instance.AlertDialog(exception.Message);
            }
        }
        private void btn_Add_Click(object sender, EventArgs e)
        {
            try
            {
                btn_Add.Visible = false;
                btn_Add.Visible = true;
                ConnectManager.CleanSelcol();
                hiLightSeled();
                InitializeList(user2conn());
            }
            catch (Exception exception)
            {
                PublicMethod.Instance.AlertDialog(exception.Message);
            }
        }
        private void btn_Del_Click(object sender, EventArgs e)
        {
            try
            {
                ConnlistView.BeginUpdate();
                for (var i = 0; i < ConnlistView.SelectedItems.Count; )
                {
                    var curItem = ConnlistView.SelectedItems[0];
                    var tem = (ObjectId)curItem.Tag;
                    ConnlistView.Items.Remove(curItem);
                    objColDict.Remove(tem);
                    objList.Remove(tem);
                }
                UpdateList();
                ConnlistView.EndUpdate();
            }
            catch (Exception exception)
            {
                PublicMethod.Instance.AlertDialog(exception.Message);
            }
        }
        private void btn_ok_Click(object sender, EventArgs e)
        {
            try
            {
                var IdListFromView = new ObjectIdList();
                foreach (ListViewItem item in ConnlistView.Items)
                {
                    item.ForeColor = Color.Black;
                    IdListFromView.Add((ObjectId)item.Tag);
                }
                if (IdListFromView.Any())
                {
                    var b = buildConnByObjIdList(IdListFromView);
                    if (b)
                    {
                        PublicMethod.Instance.AlertDialog("连接有错误,请检查设备的连接顺序!");
                    }
                    else
                    {
                        TopologyMethods.bChangeTopo = true;
                        PublicMethod.Instance.AlertDialog("连接全部建立成功!");
                    }                
                }
            }
            catch (Exception exception)
            {
                if (exception.Message != "cancel")
                    PublicMethod.Instance.AlertDialog(exception.Message);
            }
        }

        /// <summary>
        /// 更新面板序号
        /// </summary>
        void UpdateList()
        {
            if (ConnlistView.Items.Count <= 0) return;
            for (int index = 0; index < ConnlistView.Items.Count; index++)
            {
                ListViewItem item = ConnlistView.Items[index];
                var oldStr = item.Text.Split(' ');
                var t = (index + 1) + @" " + oldStr[1] + @" " + oldStr[2];
                item.Text = t;
            }
        }
        /// <summary>
        /// 当面板被隐藏时清理
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void ConnectByUser_VisibleChanged(object sender, EventArgs e)
        {
            try
            {
                if (Visible == false)
                {
                    cleanObjCol(objList, objColDict);
                    objColDict.Clear();
                    objList.Clear();
                    //u2clist.Clear();
                    ConnlistView.Clear();
                }
            }
            catch (Exception exception)
            {
                PublicMethod.Instance.AlertDialog(exception.Message);
            }
        }
        /// <summary>
        /// 建立连接关系
        /// </summary>
        /// <param name="objIdList">需要建立连接的设备列表</param>
        /// <returns>成功返回true,失败返回false</returns>
        bool buildConnByObjIdList(ObjectIdList objIdList)
        {
            var hasEr = false;
            var db = HostApplicationServices.WorkingDatabase;
            var connList = new List<Connectivity_n>();
            long g3eid = 0, g3efid = 0, g3efno = 0;
            var hasConn = false;
            // 判断已选择的设备是否已经存在连接关系
            foreach (var id in objIdList)
            {
                DBEntityFinder.Instance.GetG3EIds(id, ref g3eid, ref g3efid, ref g3efno);
                var conn = DBManager.Instance.GetEntity<Connectivity_n>(g3efid);
                if (conn.NODE1_ID != 0 || conn.NODE2_ID != 0) hasConn = true;
                connList.Add(conn);
            }
            var dlgRes = DialogResult.Yes;
            if (hasConn)
            {
                dlgRes = MessageBox.Show(@"选择中的设备已有连接关系。
是否打断重新建立?", @"AutoCad", MessageBoxButtons.YesNo, MessageBoxIcon.Warning);
                if (dlgRes != DialogResult.Yes) throw new Exception("cancel");
                // 打断关系
                TopologyMethods.breakall(objIdList);
            }
            using (var tr = db.TransactionManager.StartTransaction())
            {
                // 建立关系
                for (var i = 0; i < connList.Count - 1; i++)
                {
                    var newNode = CYZCommonFunc.getid();
                    if (accordConnCrom(connList[i].G3E_FNO, connList[i+1].G3E_FNO))
                    {      
                        // 判断设备是否只有一个连接点     143-站房母线  159-低压集中抄表箱 79-低压母线 151-10kV电缆终端头 74-低压终端 72-接地挂环 85-PT柜 清空节点2
                        if (PublicMethod.Instance.N2is0.Concat(PublicMethod.Instance.N1isN2).Contains(connList[i].G3E_FNO))
                        {
                            if (connList[i].NODE1_ID == 0) // 若果母线连接1为0
                            {
                                if (PublicMethod.Instance.N1isN2.Contains(connList[i].G3E_FNO))
                                {
                                    connList[i].NODE1_ID = connList[i].NODE2_ID = connList[i + 1].NODE1_ID = newNode;
                                    TopologyMethods.ChangEntStatus(connList[i], 2, "Add");
                                } 
                                else
                                {
                                    connList[i].NODE1_ID = connList[i + 1].NODE1_ID = newNode; 
                                }
                                TopologyMethods.ChangEntStatus(connList[i], 1, "Add");
                            }
                            else // 否则直接将母线连接1赋值给下一个设备
                            {
                                if (PublicMethod.Instance.N1isN2.Contains(connList[i].G3E_FNO))
                                {
                                    connList[i + 1].NODE1_ID = connList[i].NODE2_ID = connList[i].NODE1_ID;
                                    TopologyMethods.ChangEntStatus(connList[i], 2, "Add");
                                }
                                else
                                {
                                    connList[i + 1].NODE1_ID = connList[i].NODE1_ID;
                                }
                            }
                        }
                        else/* if (sinNodDevArr.Contains(connList[i + 1].G3E_FNO))*/
                        {
                            connList[i].NODE2_ID = connList[i + 1].NODE1_ID = newNode;
                            TopologyMethods.ChangEntStatus(connList[i], 2, "Add");
                            if (PublicMethod.Instance.N1isN2.Contains(connList[i + 1].G3E_FNO))
                            {
                                connList[i + 1].NODE2_ID = newNode;
                                TopologyMethods.ChangEntStatus(connList[i+1], 2, "Add");
                            }
                        }
                        TopologyMethods.ChangEntStatus(connList[i + 1], 1, "Add");
                        DBManager.Instance.Update(connList[i]);// 改变状态
                    }
                    else
                    {
                        hasEr = true;
                        ConnlistView.Items[i].ForeColor = Color.Red;
                        ConnlistView.Items[i + 1].ForeColor = Color.Red;
                    }
                }
                // 更新列表最后一个设备连接状态
                DBManager.Instance.Update(connList[connList.Count - 1]);
                tr.Commit();
            }
            return hasEr;
        }

        /// <summary>
        /// 判断是否符合连接关系规范
        /// </summary>
        /// <param name="sourceFno"></param>
        /// <param name="connectFno"></param>
        /// <returns></returns>
        static bool accordConnCrom(int sourceFno, int connectFno)
        {
            var bcon = CDDBManager.Instance.GetEntities<G3e_nodeedgeconnectivity>(o => o.G3E_SOURCEFNO == sourceFno).Select(o => o.G3E_CONNECTINGFNO).Contains(connectFno);
            if (bcon) return true;
            var str1 = FeatureMapping.instance.features[sourceFno.ToString()];
            var str2 = FeatureMapping.instance.features[connectFno.ToString()];
            PublicMethod.Instance.Editor.WriteMessageWithReturn(str1 + " 不能与 " + str2 + "相连!\n");
            return false;
        }
         
        /// <summary>
        /// 连续选择建立连接的设备
        /// </summary>
        /// <returns></returns>
        private List<ObjectId> user2conn()
        {
            // 打开选择中状态，屏蔽PublicMethod.Instance.SetLayerDisplay函数
            PublicMethod.Instance.isSelecting = true;
            var db = HostApplicationServices.WorkingDatabase;
            var doc = db.GetDocument();
            var ed = db.GetEditor();
            //u2clist.Clear();
            long g3eid = 0, g3efid = 0, g3efno = 0;
            try
            {
                using (doc.LockDocument())
                {
                    var per = ed.GetEntity("");
                    while (per.Status == PromptStatus.OK)
                    {
                        DCadApi.isModifySymbol = true;
                        //  工单判断
                        if (DBEntityFinder.Instance.VerifyLTTID(per.ObjectId) == false)
                        {
                            PublicMethod.Instance.ShowMessage("您选择的设备没有被锁定,请重新选择\n");
                            per = ed.GetEntity("");
                            continue;
                        }
                        using (var tr = db.TransactionManager.StartTransaction())
                        {
                            var ent = tr.GetObject(per.ObjectId, OpenMode.ForWrite) as Entity;
                            // 如果选择的对象是空的、标注、杂项标注，不能选择
                            if (!(ent == null || ent is DBText || ent is MText))
                            {
                                // 获取设备信息
                                DBEntityFinder.Instance.GetG3EIds(per.ObjectId, ref g3eid, ref g3efid, ref g3efno);
                                // 判断设备是否可以创建连接关系，若否，不能选择
                                var hasConn = CDDBManager.Instance.GetEntities<G3e_nodeedgeconnectivity>().Select(o => o.G3E_SOURCEFNO).Contains((int)g3efno);
                                if (hasConn)
                                {
                                    // 避免重复选择一个设备
                                    if (!objColDict.Keys.Contains(per.ObjectId))
                                    {
                                        objColDict.Add(per.ObjectId, ent.ColorIndex);
                                        //u2clist.Add(per.ObjectId);
                                        objList.Add(per.ObjectId);
                                        ent.ColorIndex = 4;
                                    }
                                    else
                                    {
                                        PublicMethod.Instance.Editor.WriteMessageWithReturn("此设备已在列表中.\n");
                                    }
                                }
                                else
                                {
                                    PublicMethod.Instance.Editor.WriteMessageWithReturn("此设备不能建立连接关系.\n");
                                }
                            }
                            tr.Commit();
                        }
                        per = ed.GetEntity("");
                    }
                }
                cleanObjCol(objList, objColDict);
                return objList;
            }
            catch (Exception ex)
            {
                PublicMethod.Instance.AlertDialog("user2conn选择出错:" + ex.Message);
                cleanObjCol(objList, objColDict);
                return objList;
            }
            finally
            {
                DCadApi.isModifySymbol = false;
                PublicMethod.Instance.isSelecting = false;
            }
        }
        /// <summary>
        /// 高亮列表选择的设备
        /// </summary>
        void hiLightSeled()
        {
            DCadApi.isModifySymbol = true;
            var db = HostApplicationServices.WorkingDatabase;
            var doc = db.GetDocument();
            try
            {
                using (doc.LockDocument())
                {
                    // 把之前已经选择还没删除的设备高亮
                    using (var tr = db.TransactionManager.StartTransaction())
                    {
                        foreach (var idd in objColDict.Keys)
                        {
                            var ent = tr.GetObject(idd, OpenMode.ForWrite) as Entity;
                            if (ent != null) ent.ColorIndex = 4;
                        }
                        tr.Commit();
                    }
                }
            }
            catch (Exception ex)
            {
                PublicMethod.Instance.Editor.WriteMessageWithReturn(ex.Message + "\n");
            }
            finally
            {
                DCadApi.isModifySymbol = false;
            }
        }
         
        /// <summary>
        /// 初始化面板view控件
        /// </summary>
        /// <param name="nodeList"></param>
        public void InitializeList(List<ObjectId> nodeList)
        {
            Autodesk.AutoCAD.ApplicationServices.Application.UpdateScreen();
            //this.ConnlistView.View = View.List;
            ConnlistView.SmallImageList = imageList1;
            ConnlistView.BeginUpdate();
            ObjectId objid;
            long fno = 0, fid = 0, id = 0;
            string typeName;
            //nodeList.Clear();
            ConnlistView.Clear();
            for (int i = 0; i < nodeList.Count ; i++)
            //for (int i = nodeList.Count - 1; i >= 0; i--)
            {
                var lvi = new ListViewItem();
                objid = nodeList.ElementAt(i);
                DBEntityFinder.Instance.GetG3EIds(objid, ref id, ref fid, ref fno);
                if (FeatureMapping.instance.features.Keys.Contains(fno.ToString()))
                {
                    typeName = FeatureMapping.instance.features[fno.ToString()];
                }
                else
                {
                    typeName = "xxxxx";
                    PublicMethod.Instance.Editor.WriteMessageWithReturn("FeatureMapping字典没有此fno" + fno + "设备");
                }
                string devName = DCadApi.getDevNamebyfidfno(fid, fno);
                lvi.Tag = objid;
                //lvi.ImageIndex = i;
                lvi.Text = (i + 1) + @" " + typeName + @" " + devName;
                ConnlistView.Items.Add(lvi);
            }
            ConnlistView.EndUpdate();
        } 
         
        /// <summary>
        /// 还原指定字典里实体颜色
        /// </summary>
        /// <param name="nodeList">实体列表</param>
        /// <param name="dict"></param>
        private void cleanObjCol(ObjectIdList nodeList, Dictionary<ObjectId, int> dict)
        {
            DCadApi.isModifySymbol = true;
            var db = HostApplicationServices.WorkingDatabase;
            if (db == null) return;
            try
            {
                using (db.GetDocument().LockDocument())
                {
                    using (var tr = db.TransactionManager.StartTransaction())
                    {
                        for (var i = 0; i < nodeList.Count(); i++)
                        {
                            if (nodeList[i].IsErased || nodeList[i].IsNull) continue;
                            var ent = tr.GetObject(nodeList[i], OpenMode.ForWrite) as Entity;
                            if (ent == null) continue;
                            if (!dict.Keys.Contains(ent.ObjectId)) continue;
                            ent.ColorIndex = dict[ent.ObjectId];
                        }
                        tr.Commit();
                    }
                }
            }
            catch (Exception ex)
            {
                PublicMethod.Instance.AlertDialog("cleanObjCol: " + ex.Message);
            }
            finally
            {
                DCadApi.isModifySymbol = false;
            }
        }

        private void ConnlistView_ItemSelectionChanged(object sender, ListViewItemSelectionChangedEventArgs e)
        {
            Database db = HostApplicationServices.WorkingDatabase;
            Document doc = db.GetDocument();
            ListViewItem lvi = e.Item;
            try
            {
                DCadApi.isModifySymbol = true;
                using (doc.LockDocument())
                {
                    using (Transaction tr = db.TransactionManager.StartTransaction())
                    {
                        var ent = tr.GetObject((ObjectId)lvi.Tag, OpenMode.ForWrite) as Entity;
                        if (ent == null) return;
                        if (e.IsSelected)
                        {
                            ent.ColorIndex = 4;
                        }
                        else
                        {
                            ent.ColorIndex = objColDict[ent.ObjectId];
                        }
                        tr.Commit();
                    }
                }
            }
            catch (Exception ex)
            {
                PublicMethod.Instance.Editor.WriteMessageWithReturn(ex.Message + "\n");
            }
            finally 
            {
                Autodesk.AutoCAD.ApplicationServices.Application.UpdateScreen();
                DCadApi.isModifySymbol = false; 
            }
        }

        private void btn_Up_MouseEnter(object sender, EventArgs e)
        {
            btn_Up.ImageIndex = 0;
            toolTip1.Show("上移", (Button)sender);
        }

        private void btn_Up_MouseLeave(object sender, EventArgs e)
        {
            btn_Up.ImageIndex = 1;
            toolTip1.Hide((Button)sender);
        }

        private void btn_Down_MouseEnter(object sender, EventArgs e)
        {
            btn_Down.ImageIndex = 2;
            toolTip1.Show("下移", (Button)sender);
        }

        private void btn_Down_MouseLeave(object sender, EventArgs e)
        {
            btn_Down.ImageIndex = 3;
            toolTip1.Hide((Button)sender);
        }

        private void btn_Add_MouseEnter(object sender, EventArgs e)
        {
            btn_Add.ImageIndex = 4;
            toolTip1.Show("增加", (Button)sender);
        }

        private void btn_Add_MouseLeave(object sender, EventArgs e)
        {
            btn_Add.ImageIndex = 5;
            toolTip1.Hide((Button)sender);
        }

        private void btn_Del_MouseEnter(object sender, EventArgs e)
        {
            btn_Del.ImageIndex = 6;
            toolTip1.Show("移除", (Button)sender);
        }

        private void btn_Del_MouseLeave(object sender, EventArgs e)
        {
            btn_Del.ImageIndex = 7;
            toolTip1.Hide((Button)sender);
        }

        private void btn_ok_MouseEnter(object sender, EventArgs e)
        {
            btn_ok.ImageIndex = 8;
            toolTip1.Show("确认选择", (Button)sender);
        }

        private void btn_ok_MouseLeave(object sender, EventArgs e)
        {
            btn_ok.ImageIndex = 9;
            toolTip1.Hide((Button)sender);
        }

        private void btn_clean_Click(object sender, EventArgs e)
        {
            try
            {
                ConnlistView.BeginUpdate();
                cleanObjCol(objList, objColDict);
                ConnlistView.Clear();
                objColDict.Clear();
                objList.Clear();
                ConnlistView.EndUpdate();
            }
            catch (Exception exception)
            {
                PublicMethod.Instance.AlertDialog(exception.Message);
            }
        }
    }
}
