using System;
using System.Linq;
using DotNetARX;
using System.Windows.Forms;
using Autodesk.AutoCAD.ApplicationServices;
using Autodesk.AutoCAD.DatabaseServices;
using Autodesk.AutoCAD.EditorInput;
using ElectronTransferDal.Cad;
using ElectronTransferDal.Common;
using ElectronTransferDal.Query;
using ElectronTransferModel.V9_4;
using ElectronTransferView.ViewManager;
using AcadApp = Autodesk.AutoCAD.ApplicationServices.Application;
using ElectronTransferModel;
using ElectronTransferView.Menu;

namespace ElectronTransferView.ConnectivityManager
{
    public partial class ConnectMxToYx : Form
    {
        Document doc;//文档
        Database db;//数据库
        Editor ed;
        
        public ConnectMxToYx()
        {
            InitializeComponent();
            InitializeDialog();
        }

        private void InitializeDialog()
        {
            doc = AcadApp.DocumentManager.MdiActiveDocument;
            db = doc.Database;
            ed = doc.Editor;
            SelYxCount_Lab.Text = @"0";
        }

        private void SelMxBtn_Click(object sender, EventArgs e)
        {
            try
            {
                Visible = false;
                using (doc.LockDocument())
                {
                    using (db.TransactionManager.StartTransaction())
                    {
                        PromptEntityResult per;
                        bool isMx ;
                        long g3eid = 0, g3efid = 0, g3efno = 0;
                        do
                        {
                            isMx = true;
                            // 选择一条母线
                            per = ed.GetEntity("请选择母线 : ");
                            if (per.Status != PromptStatus.OK) { return; }
                            // 得到母线的信息
                            var b = DBEntityFinder.Instance.GetG3EIds(per.ObjectId, ref g3eid, ref g3efid, ref g3efno);
                            if (b != true) { return; }
                            // 判断是否为母线
                            long[] mxarr = { 11, 120, 143, 167, 79 };
                            if (mxarr.Contains(g3efno) != true)
                            {
                                isMx = false;
                                ed.WriteMessageWithReturn("您选择的不是母线! 请重新选择!\n");
                            }
                            if (false == DBEntityFinder.Instance.VerifyLTTID(per.ObjectId))
                            {
                                isMx = false;
                                PublicMethod.Instance.ShowMessage("您选择的设备没有被锁定,请重新选择\n");
                            }
                        } while (isMx == false);

                        // 得到母线在Common表中的记录
                        var comm = DBManager.Instance.GetEntity<Common_n>(g3efid);
                        if (comm == null) { return; }

                        MxFid_textBox.Text = g3efid.ToString();
                        MxFid_textBox.Tag = g3efid;
                        MxName_textBox.Text = comm.SBMC;
                        MxName_textBox.Tag = g3efno;
                    }
                }
            }
            catch (Exception ex) { ed.WriteMessageWithReturn(ex.Message + "\n");  }
            finally                                        { Visible = true; }
        }

        private void SelYxbtn_Click(object sender, EventArgs e)
        {
            try
            {
                if (MxName_textBox.Tag == null || MxFid_textBox.Tag == null)
                {
                    PublicMethod.Instance.AlertDialog("请先选择母线");
                    return;
                }
                Visible = false;
                long g3eid = 0, g3efid = 0, g3efno = 0;
                var mxFno = (long)MxName_textBox.Tag; // 要先选择了母线才能进行过滤
                PromptSelectionResult selper;
                var pso = new PromptSelectionOptions();
                var selFilObjs = new ObjectIdList();
                ed.SetImpliedSelection(new ObjectId[0]);
                do
                {
                    selper = ed.GetSelection(pso);
                    if (selper.Status == PromptStatus.OK) { ed.WriteMessageWithReturn(selper.Value.Count); }
                } while (selper.Status == PromptStatus.Error || selper.Status == PromptStatus.None);

                DCadApi.isModifySymbol = true;
                using (doc.LockDocument())
                {
                    using (var tr = db.TransactionManager.StartTransaction())
                    {
                        foreach (var id in selper.Value.GetObjectIds())
                        {
                            if (false == DBEntityFinder.Instance.VerifyLTTID(id)) continue;
                            var ent = tr.GetObject(id, OpenMode.ForRead) as Entity;
                            if (ent is DBText) continue;
                            if (ent is MText) continue;
                            DBEntityFinder.Instance.GetG3EIds(id, ref g3eid, ref g3efid, ref g3efno);
                            if (g3efid == (long)MxFid_textBox.Tag) continue;
                            var bcon = CDDBManager.Instance.GetEntities<G3e_nodeedgeconnectivity>(o => o.G3E_SOURCEFNO == mxFno).Select(o => o.G3E_CONNECTINGFNO).Contains((int)g3efno);
                            if (bcon) {  selFilObjs.Add(id);  }
                        }
                        ed.SetImpliedSelection(selFilObjs);
                        SelYxCount_Lab.Text = selFilObjs.Count.ToString();
                        Set_View(selFilObjs);
                        tr.Commit();
                    }
                }
            }
            catch (Exception ex) { ed.WriteMessageWithReturn(ex.Message + "\n");  }
            finally
            {
                DCadApi.isModifySymbol = false; 
                Visible = true;
            }
        }

        private void Ok_Btn_Click(object sender, EventArgs e)
        {
            try
            {
                // 是否已选择了母线
                if (MxName_textBox.Tag == null || MxFid_textBox.Tag == null) { PublicMethod.Instance.AlertDialog("请先选择母线"); return; }
                // 是否已选择了上下游设备
                if (SelYxCount_Lab.Text == "0") { PublicMethod.Instance.AlertDialog("请先选择上下游设备"); return; }
                // 是否已确定上下游方向
                if (InRadBtn.Checked == false && OutRadBtn.Checked == false) { PublicMethod.Instance.AlertDialog("请先选择上下游方向"); return; }

                var IdListFromView = new ObjectIdList();
                IdListFromView.AddRange(from ListViewItem item in YxlistView.Items select (ObjectId) item.Tag);

                long g3eid = 0, g3efid = 0, g3efno = 0;
                var mxfid = (long)MxFid_textBox.Tag;
                var nodeid = CYZCommonFunc.getid();
                var strNodC = "";
                using (doc.LockDocument())
                {
                    using (db.TransactionManager.StartTransaction())
                    {
                        var conn = DBManager.Instance.GetEntity<Connectivity_n>(mxfid);
                        var dlgRes = DialogResult.Yes;
                        if ((conn.NODE1_ID != 0 && InRadBtn.Checked) || (conn.NODE2_ID != 0 && OutRadBtn.Checked)
                            || (conn.NODE1_ID !=0 && conn.G3E_FNO == 143) )
                        {
                            dlgRes = MessageBox.Show(
                                                     "已有连接关系"       +
                                                     "\n 是     连接已有 " +
                                                     "\n 否     打断重建 " , 
                                                     "AutoCad", 
                                MessageBoxButtons.YesNoCancel, 
                                MessageBoxIcon.Warning);
                            if (!(dlgRes == DialogResult.Yes || dlgRes == DialogResult.No)) { return; }
                            if (dlgRes == DialogResult.Yes)
                            {
                                if (InRadBtn.Checked)
                                {
                                    if (conn.NODE1_ID != null) nodeid = (int) conn.NODE1_ID;
                                } 
                                else
                                {
                                    if (conn.NODE2_ID != null) nodeid = (int) conn.NODE2_ID;
                                }
                                var tempstr = conn.EntityState.ToString();
                                if (tempstr.Length > 7) tempstr = tempstr.Substring(4, 3);
                                switch (tempstr)
                                {
                                    case "None":
                                        strNodC = "Old";
                                        break;
                                    case "Update":
                                        strNodC = "Old";
                                        break;
                                    default:
                                        strNodC = tempstr;
                                        break;
                                }
                            }
                        }
                        if (dlgRes == DialogResult.No || conn.NODE1_ID == 0)
                        {
                            strNodC = "Add";
                            if (conn.G3E_FNO == 143)
                            {
                                conn.NODE1_ID = conn.NODE2_ID = nodeid;
                                /*改变母线状态*/
                                TopologyMethods.ChangEntStatus(conn, 1, "Add");
                                TopologyMethods.ChangEntStatus(conn, 2, "Add");
                            } 
                            else
                            {
                                if (OutRadBtn.Checked)
                                {
                                    conn.NODE2_ID = nodeid;
                                    TopologyMethods.ChangEntStatus(conn, 2, "Add");
                                }
                                else
                                {
                                    conn.NODE1_ID = nodeid;
                                    TopologyMethods.ChangEntStatus(conn, 1, "Add");
                                }
                            }
                        }
                        //int[] sinNodDevArr = new int[] { 11, 72, 79, 85, 120, 143, 167, 173 };
                        foreach (var id in IdListFromView)
                        {
                            DBEntityFinder.Instance.GetG3EIds(id, ref g3eid, ref g3efid, ref g3efno);
                            conn = DBManager.Instance.GetEntity<Connectivity_n>(g3efid);
                            if (OutRadBtn.Checked || PublicMethod.Instance.N2is0.Concat(PublicMethod.Instance.N1isN2).Contains(conn.G3E_FNO))
                            {
                                conn.NODE1_ID = nodeid;
                                TopologyMethods.ChangEntStatus(conn, 1, strNodC);
                            }
                            else
                            {
                                conn.NODE2_ID = nodeid;
                                TopologyMethods.ChangEntStatus(conn, 2, strNodC);
                            }
                            sinNodDevJudge(conn);
                        }
                    }
                }
                TopologyMethods.bChangeTopo = true;
                PublicMethod.Instance.AlertDialog("建立完成");
            }
            catch (Exception ex) { ed.WriteMessageWithReturn(ex.Message + "\n"); }
        }

        // 一个连接点的设备特殊处理
        void sinNodDevJudge(Connectivity_n conn)
        {
            int[] sinNodDevArr = { 11, 72, 79, 85, 120, 143, 167, 173 };
            if (!sinNodDevArr.Contains(conn.G3E_FNO)) return;
            if (conn.NODE1_ID == 0 && conn.NODE2_ID != 0) conn.NODE1_ID = conn.NODE2_ID;
            else if (conn.NODE2_ID == 0 && conn.NODE1_ID != 0) conn.NODE2_ID = conn.NODE1_ID;
            else PublicMethod.Instance.Editor.WriteMessageWithReturn("数据出错！");
        }

        private void Set_View(ObjectIdList objs)
        {
            YxlistView.BeginUpdate();
            ObjectId objid;
            long fno = 0, fid = 0, id = 0;
            string typeName;
            YxlistView.Clear();
            for (var i = 0; i < objs.Count; i++)
            {
                var lvi = new ListViewItem();
                objid = objs.ElementAt(i);
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
                var devName = DCadApi.getDevNamebyfidfno(fid, fno);
                lvi.Tag = objid;
                lvi.ImageIndex = i;
                lvi.Text = (i + 1) + @" " + typeName + @" " + devName;
                YxlistView.Items.Add(lvi);
            }
            YxlistView.EndUpdate();
        }

        private void ConnectMxToYx_Load(object sender, EventArgs e)
        {
            ViewHelper.HideConnectPsState();
        }

        private void ConnectMxToYx_VisibleChanged(object sender, EventArgs e)
        { 
        }

        private void ConnectMxToYx_FormClosed(object sender, FormClosedEventArgs e)
        {
            //ed.WriteMessageWithReturn("ConnectMxToYx_FormClosed\n");
            MenuControl.showConnectManager();
        }
    }
}
