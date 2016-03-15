using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows.Forms;
using ElectronTransferDal;
using ElectronTransferDal.AutoGeneration;
using ElectronTransferDal.Cad;
using ElectronTransferDal.Common;
using ElectronTransferDal.Query;
using ElectronTransferFramework;
using ElectronTransferModel;
using ElectronTransferModel.V9_4;
using Autodesk.AutoCAD.DatabaseServices;
using Autodesk.AutoCAD.Geometry;
using cadApplication=Autodesk.AutoCAD.ApplicationServices.Application;

namespace ElectronTransferView.FunctionManager
{
    public partial class RegionalManagement : Form
    {
        public RegionalManagement()
        {
            if (m_ActionForm == null)
            {
                m_ActionForm = this;
            }
            InitializeComponent();
        }

        /// <summary>
        /// 当前选择了哪个馈线节点
        /// </summary>
        private static TreeNode curKX;
        /// <summary>
        /// 表示当前激活的窗体
        /// </summary>
        private static RegionalManagement m_ActionForm;

        private Dictionary<ObjectId, int> dictionary; 

        /// <summary>
        /// 返回当前激活的窗体
        /// </summary>
        /// <returns></returns>
        public static RegionalManagement GetCurActionForm()
        {
            return m_ActionForm;
        }
        /// <summary>
        /// 查看是否当前支线管理窗口正在打开
        /// </summary>
        /// <returns></returns>
        public static bool IsAction()
        {
            return m_ActionForm!=null;
        }
        
        /// <summary>
        /// 获取当前选择的馈线节点
        /// </summary>
        /// <returns></returns>
        public static TreeNode GetSeleteKX()
        {
            if (IsAction()) return curKX;
            return null;
        }
        /// <summary>
        /// 点击添加按钮
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void button1_Click(object sender, EventArgs e)
        {
            try
            {
                NullityButtons();
                if (treeView1.SelectedNode == null ||
                    !(GetTreeNodeLev(treeView1.SelectedNode) == 4 || GetTreeNodeLev(treeView1.SelectedNode) == 3))
                    return;
                Visible = false;
                var objectId = PublicMethod.Instance.GetEntity("选择导线或者电缆\n");
                var g3eObject = new G3EObject();
                if (DBEntityFinder.Instance.GetG3EIds(objectId, ref g3eObject))
                {
                    if (!(g3eObject.G3E_FNO == 141 || g3eObject.G3E_FNO == 140))
                    {
                        PublicMethod.Instance.ShowMessage("只允许选择导线或者电缆\n");
                        return;
                    }
                    //检查是否属于馈线
                    var kxNode = GetSeleteKX();
                    if (kxNode == null)
                    {
                        PublicMethod.Instance.ShowMessage("没有选择馈线,请再重试一遍\n");
                        return;
                    }
                    var zx = DBManager.Instance.GetEntity<Common_n>(o => o.G3E_FID == g3eObject.G3E_FID);
                    
                    if (zx == null)
                    {
                        PublicMethod.Instance.ShowMessage("请重新选择要添加的支线\n");
                        return;
                    }
                    if (zx.CD_SSXL != kxNode.Text)
                    {
                        PublicMethod.Instance.ShowMessage("您选择的支线不属于当前馈线,请重新选择\n");
                        return;
                    }
                    var sbmc = zx.SBMC;
                    var re = new RegionalEditor(formType.regionAdd, g3eObject.G3E_FID.ToString(), sbmc);
                    Autodesk.AutoCAD.ApplicationServices.Application.ShowModalDialog(re);
                }
            }catch(Exception exception)
            {
                LogManager.Instance.Error(exception);
            }finally
            {
                Visible = true;
            }
        }
        /// <summary>
        /// 点击编辑按钮
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void button2_Click(object sender, EventArgs e)
        {
            NullityButtons();
            if (treeView1.SelectedNode == null || GetTreeNodeLev(treeView1.SelectedNode) != 4) return;
            //获取支线的FID与Name
            var fid = treeView1.SelectedNode.Name;
            var name = treeView1.SelectedNode.Text;
            //显示编辑窗口
            var f = new RegionalEditor(formType.regionEdit,fid,name);
            cadApplication.ShowModalDialog(f);
        }
        /// <summary>
        /// 点击删除按钮
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void button3_Click(object sender, EventArgs e)
        {
            if (MessageBox.Show("确定要删除?","确定?",MessageBoxButtons.OKCancel) == DialogResult.Cancel) return;
            NullityButtons();
            if (treeView1.SelectedNode == null || GetTreeNodeLev(treeView1.SelectedNode) != 4) return;
            //获取支线的FID与Name
            var fid = treeView1.SelectedNode.Name;
            //删除支线表中数据
            try
            {
                var gnwzTzZx = DBEntityFinder.Instance.GetGnwzTzZx(long.Parse(fid));
                if (gnwzTzZx.EntityState == EntityState.Insert)
                    DBManager.Instance.Delete(gnwzTzZx);
                else
                {
                    gnwzTzZx.EntityState = EntityState.Delete;
                    DBManager.Instance.Update(gnwzTzZx);
                }
                // 更新common_n表
                var zx_comm = DBEntityFinder.Instance.GetCommon_n(long.Parse(fid));
                if (zx_comm.EntityState == EntityState.Insert)
                {
                    DBManager.Instance.Delete(zx_comm);
                }
                else
                {
                    //如果删除支线的时候就修改它的备注属性
                    var properyName = GenerateHelper.QueryZXField(zx_comm.G3E_FNO);
                    if (!string.IsNullOrEmpty(properyName))
                    {
                        if (zx_comm.EntityState == EntityState.None)
                        {
                            zx_comm.EntityState = EntityState.Update;
                        }
                        zx_comm.SetAttribute(properyName, null);
                        DBManager.Instance.Update(zx_comm);
                    }
                }

                //删除TreeView里支线
                var ptn = treeView1.SelectedNode.Parent;
                ptn.Nodes.Remove(treeView1.SelectedNode);
            }
            catch (Exception )
            {
                PublicMethod.Instance.ShowMessage("删除操作中有错\n");
            }
           
        }

        /// <summary>
        /// 添加区局
        /// </summary>
        private void AddQJ()
        {
            //获取所有区局ssdw
            IEnumerable<Cd_ssxl> query = CDDBManager.Instance.GetEntities<Cd_ssxl>().OrderBy(o => o.CD_SSDW);
            var ssdw = query.Select(o => o.CD_SSDW).Distinct().ToArray();

            //添加入TreeView
            foreach (var qj in ssdw)
            {
                //在TreeView组件中加入子节点
                var tmp = new TreeNode(qj);
                treeView1.Nodes.Add(tmp);
                tmp.Nodes.Add(new TreeNode("1"));//添加一个子节点以显示加号
            }
        }

        
        /// <summary>
        /// 添加对应区局的所有变电站
        /// </summary>
        /// <param name="qj">区局节点</param>
        private void AddBDZ(TreeNode qj)
        {
            if (qj.Nodes.Count == 1 && qj.Nodes[0].Text == "1")//第一次则加载
            {
                var bdz = CDDBManager.Instance.GetEntities<Cd_ssxl>().OrderBy(o => o.SSBDZ);
                var query = (from ssbdz in bdz
                                              where ssbdz.CD_SSDW == qj.Text
                                              select ssbdz);
                var bdzlist = query.Select(o => o.SSBDZ).Distinct().ToArray();

                //清除
                qj.Nodes.Clear();
                //循环添加
                progressBar1.Maximum = bdzlist.Count();
                progressBar1.Value = 0;
                foreach (var bdzmc in bdzlist)
                {
                    var tmp = new TreeNode(bdzmc);
                    qj.Nodes.Add(tmp);
                    tmp.Nodes.Add(new TreeNode("1"));//添加一个子节点以显示加号
                    progressBar1.PerformStep();
                }
                progressBar1.Value = 0;
            }
        }

        /// <summary>
        /// 添加对应变电站所有馈线
        /// </summary>
        /// <param name="bdz">变电站节点</param>
        private void AddKX(TreeNode bdz)
        {
            if (bdz.Nodes.Count == 1 && bdz.Nodes[0].Text == "1")//第一次则加载
            {
                var kx = CDDBManager.Instance.GetEntities<Cd_ssxl>().OrderBy(o => o.NAME);
                var query = (from sskx in kx
                                              where sskx.SSBDZ == bdz.Text
                                              select sskx);
                var kxmc = query.Select(o => o.NAME).Distinct().ToArray();
                //清除
                bdz.Nodes.Clear();
                //循环添加
                progressBar1.Maximum = kxmc.Count();
                progressBar1.Value = 0;
                foreach (var t in kxmc)
                {
                    var tmp = new TreeNode(t);
                    bdz.Nodes.Add(tmp);
                    AddZX(tmp);
                    progressBar1.PerformStep();
                }
                progressBar1.Value = 0;
            }
           
        }


        /// <summary>
        /// 添加对应馈线所有支线
        /// </summary>
        /// <param name="kx">馈线节点</param>
        public void AddZX(TreeNode kx)
        {
            //if (kx.Nodes.Count == 1 && kx.Nodes[0].Text == "1")//第一次则加载
            //{
                //寻找所属馈线下的单元
                var coms = DBManager.Instance.GetEntities<Common_n>(o => o.CD_SSXL == kx.Text);
                //清除
                kx.Nodes.Clear();
                //循环判断是否是支线并添加
                progressBar2.Maximum = coms.Count();
                progressBar2.Value = 0;
                foreach (var v in coms)
                {
                    var zx =DBEntityFinder.Instance.GetGnwzTzZx(v.G3E_FID);//判断是否是支线
                    if (zx != null)//如果是支线
                    {
                        var fid = String.Format("{0}", v.G3E_FID);
                        //添加入支线
                        kx.Nodes.Add(fid, v.SBMC);
                    }
                    progressBar2.PerformStep();
                }
                progressBar2.Value = 0;
            //}
        }
    
        /// <summary>
        /// 加载窗体
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void RegionalManagement_Load(object sender, EventArgs e)
        {
            AddQJ();
            progressBar1.Step = 1;
            progressBar1.Value = 0;
            progressBar2.Step = 1;
            progressBar2.Value = 0;
        }


        /// <summary>
        /// 将要张开节点前,导入对应的子节点信息
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void treeView1_BeforeExpand(object sender, TreeViewCancelEventArgs e)
        {
            var senderNode = e.Node;
            if (senderNode == null) return;
            //判断是区局,变电站,还是馈线            
            if (GetTreeNodeLev(senderNode) == 2)//变电站
            {
                AddKX(senderNode);//添加馈线
            }
            else if (GetTreeNodeLev(senderNode) == 1)//区局
            {
                AddBDZ(senderNode);//添加变电站
            }
            //复位定位的颜色
            ReSetColor();
        }
        /// <summary>
        /// 设置所有按钮失效
        /// </summary>
        public void NullityButtons()
        {
            AddButton.Enabled = false;
            EditButton.Enabled = false;
            DeleteButton.Enabled = false;
            btnLocation.Enabled = false;
        }


        /// <summary>
        /// 更改选定内容后,根据选择了哪层节点决定哪些按钮可用
        /// 区局,变电站 -> 全不可用
        /// 馈线 -> 添加可用
        /// 支线 -> 添加,编辑,删除可用
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void treeView1_AfterSelect(object sender, TreeViewEventArgs e)
        {
            TreeNode senderNode = e.Node;
            if (senderNode == null) return;
            //判断是区局,变电站,还是馈线
            if (GetTreeNodeLev(senderNode)==3)//馈线
            {
                AddButton.Enabled = true;
                EditButton.Enabled = false;
                DeleteButton.Enabled = false;
                btnLocation.Enabled = false;
                curKX = senderNode;
            }
            else if (GetTreeNodeLev(senderNode) == 2)//变电站
            {
                AddButton.Enabled = false;
                EditButton.Enabled = false;
                DeleteButton.Enabled = false;
                btnLocation.Enabled = false;
                curKX = null;
            }
            else if (GetTreeNodeLev(senderNode) == 1)//区局
            {
                AddButton.Enabled = false;
                EditButton.Enabled = false;
                DeleteButton.Enabled = false;
                btnLocation.Enabled = false;
                curKX = null;
            }
            else if (GetTreeNodeLev(senderNode) == 4)//支线
            {
                AddButton.Enabled = true;
                EditButton.Enabled = true;
                DeleteButton.Enabled = true;
                btnLocation.Enabled = true;
                curKX = senderNode.Parent;
            }
            else
            {
                AddButton.Enabled = false;
                EditButton.Enabled = false;
                DeleteButton.Enabled = false;
                btnLocation.Enabled = false;
                curKX = null;
            }
            
        }

        /// <summary>
        /// 计算当前选中节点所在层数
        /// </summary>
        /// <param name="tn"></param>
        /// <returns></returns>
        private int GetTreeNodeLev(TreeNode tn)
        {
            if (tn.Parent != null) //若当前节点有父节点
            {
                return GetTreeNodeLev(tn.Parent) + 1;
            }
            return 1;
        }

        /// <summary>
        /// 窗口关闭之后的操作
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void RegionalManagement_FormClosed(object sender, FormClosedEventArgs e)
        {
            m_ActionForm = null;
            //复位定位的颜色
            ReSetColor();
        }

        /// <summary>
        /// 复位上一个定位过的颜色
        /// </summary>
        private void ReSetColor()
        {
            if (dictionary != null && dictionary.Any())
                ObjectColorTool.Instance.GetObjectSetColor(dictionary);
        }

        private void btnLocation_Click(object sender, EventArgs e)
        {
            try
            {
                //检查是否选择的是支线
                if (treeView1.SelectedNode == null || GetTreeNodeLev(treeView1.SelectedNode) != 4) return;
                ReSetColor();
                var doc = Autodesk.AutoCAD.ApplicationServices.Application.DocumentManager.MdiActiveDocument;
                using (var trans = doc.TransactionManager.StartTransaction())
                {
                    //定位
                    var objId = DBEntityFinder.Instance.GetObjectIdByFid(long.Parse(treeView1.SelectedNode.Name));
                    var ent = trans.GetObject(objId, OpenMode.ForRead) as Entity;
                    if (ent == null || ent.IsErased) return;
                    var exts = ent.GeometricExtents;
                    var ed = PublicMethod.Instance.acDoc.Editor;
                    var view = ed.GetCurrentView();
                    view.CenterPoint = new Point2d((exts.MaxPoint.X - exts.MinPoint.X)/2 + exts.MinPoint.X,
                                                   (exts.MaxPoint.Y - exts.MinPoint.Y)/2 + exts.MinPoint.Y);
                    view.Height = exts.MaxPoint.X - exts.MinPoint.X + 0.4373044545912342962040020103459e-4;
                    view.Width = exts.MaxPoint.X - exts.MinPoint.X + 0.4373044545912342962040020103459e-4;
                    ed.SetCurrentView(view);
                    //高亮显示
                    dictionary = ObjectColorTool.Instance.GetObjectSetColor(objId);
                    trans.Commit();
                    Autodesk.AutoCAD.ApplicationServices.Application.UpdateScreen();
                }
            }
            catch (Exception ex)
            {
                PublicMethod.Instance.ShowMessage(String.Format("{0}\n", ex.Message));
            }
            finally
            {
                DCadApi.isModifySymbol = false;
            }
        }

        private void RegionalManagement_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Escape)
            {
                //treeView1.Nodes.Clear();
                this.Close();
            }
        }
        /// <summary>
        /// 双击节点查看
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void treeView1_MouseDoubleClick(object sender, MouseEventArgs e)
        {
            button2_Click(sender, e);
        }
    }
}
