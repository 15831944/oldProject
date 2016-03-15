using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using ElectronTransferDal.Cad;
using ElectronTransferDal;
using ElectronTransferDal.Common;
using ElectronTransferFramework;
using ElectronTransferModel;
using ElectronTransferModel.Base;
using ElectronTransferModel.V9_4;
using Autodesk.AutoCAD.DatabaseServices;
using Autodesk.AutoCAD.ApplicationServices;
using ElectronTransferView.ContextMenuManager;

namespace ElectronTransferView.FunctionManager
{
    public partial class FeederManagement : Form
    {
        public FeederManagement()
        {
            InitializeComponent();
        }
        /// <summary>
        /// 当前选择了哪个变电站节点
        /// </summary>
        private static TreeNode curBDZ = null;

        /// <summary>
        /// 获取当前选择的馈线节点
        /// </summary>
        /// <returns></returns>
        public static TreeNode GetSeleteBDZ()
        {
           return curBDZ;
        }


        /// <summary>
        /// 点击添加按钮
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void button1_Click(object sender, EventArgs e)
        {
            NullityButtons();
            if (treeView1.SelectedNode == null || !(GetTreeNodeLev(treeView1.SelectedNode) == 2 || GetTreeNodeLev(treeView1.SelectedNode) == 3)) return;
            //显示编辑窗口
            FeederEditor f = new FeederEditor(formType.regionAdd, null);
           if(f.ShowDialog()==DialogResult.OK)
                AddKX(GetSeleteBDZ());
        }
        /// <summary>
        /// 点击编辑按钮
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void button2_Click(object sender, EventArgs e)
        {
            NullityButtons();
            if (treeView1.SelectedNode == null || GetTreeNodeLev(treeView1.SelectedNode) != 3) return;
            //获取支线的FID与Name
            String name;
            name = treeView1.SelectedNode.Text;
            //显示编辑窗口
            FeederEditor f = new FeederEditor(formType.regionEdit, name);
            if(f.ShowDialog()==DialogResult.OK)
                AddKX(GetSeleteBDZ());



            //Application.ShowModalDialog(f);
        }
        /// <summary>
        /// 点击删除按钮
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void button3_Click(object sender, EventArgs e)
        {
            if (treeView1.SelectedNode == null || GetTreeNodeLev(treeView1.SelectedNode) != 3) return;
            if (MessageBox.Show("确定要删除?", "确定?", MessageBoxButtons.OKCancel) == DialogResult.Cancel) return;
            NullityButtons();
            //获取支线的FID与Name
            String name;
            name = treeView1.SelectedNode.Text;
            //删除支线表中数据
            try
            {
                var ent = DBManager.Instance.GetEntity<Gg_kxinfo>(o => o.KXMC == name);
                var ent1 = DBManager.Instance.GetEntity<Gg_kxmanage>(o => o.KX_ID == ent.KX_ID);
                var ent2 = DBManager.Instance.GetEntity<Cd_ssxl>(o => o.NAME == ent.KXMC);
                //删除Cd_ssxl表数据
                DBManager.Instance.Delete(ent2);
                //删除Gg_kxinfo表数据
                if (ent.EntityState == EntityState.Insert)
                    DBManager.Instance.Delete(ent);
                else
                {
                    ent.EntityState = EntityState.Delete;
                    DBManager.Instance.Update(ent);
                }
                //删除Gg_kxmanage表数据
                if (ent1.EntityState == EntityState.Insert)
                    DBManager.Instance.Delete(ent1);
                else
                {
                    ent1.EntityState = EntityState.Delete;
                    DBManager.Instance.Update(ent1);
                }
                AddKX(GetSeleteBDZ());
            }
            catch (System.Exception)
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
            IEnumerable<Cd_ssxl> query = DBManager.Instance.GetEntities<Cd_ssxl>().OrderBy(o => o.CD_SSDW);
            var ssdw = query.Select(o => o.CD_SSDW).Distinct().ToArray();

            //添加入TreeView
            foreach (String qj in ssdw)
            {
                //在TreeView组件中加入子节点
                TreeNode tmp = new TreeNode(qj);
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
            IEnumerable<Cd_ssxl> bdz = DBManager.Instance.GetEntities<Cd_ssxl>().OrderBy(o => o.SSBDZ);
            IEnumerable<Cd_ssxl> query = (from ssbdz in bdz
                                          where ssbdz.CD_SSDW == qj.Text.ToString()
                                          select ssbdz);
            var bdzlist = query.Select(o => o.SSBDZ).Distinct().ToArray();

            //清除
            qj.Nodes.Clear();
            //循环添加
            progressBar1.Maximum = bdzlist.Count();
            progressBar1.Value = 0;
            foreach (String bdzmc in bdzlist)
            {
                TreeNode tmp = new TreeNode(bdzmc);
                qj.Nodes.Add(tmp);
                tmp.Nodes.Add(new TreeNode("1"));//添加一个子节点以显示加号
                progressBar1.PerformStep();
            }
            progressBar1.Value = 0;
        }

        /// <summary>
        /// 添加对应变电站所有馈线
        /// </summary>
        /// <param name="bdz">变电站节点</param>
        private void AddKX(TreeNode bdz)
        {
            IEnumerable<Cd_ssxl> kx = DBManager.Instance.GetEntities<Cd_ssxl>(o => true).OrderBy(o => o.NAME);
            IEnumerable<Cd_ssxl> query = (from sskx in kx
                                          where sskx.SSBDZ == bdz.Text.ToString()
                                          select sskx);
            var kxmc = query.Select(o => o.NAME).Distinct().ToArray();

            //清除
            bdz.Nodes.Clear();
            //循环添加
            progressBar1.Maximum = kxmc.Count();
            progressBar1.Value = 0;
            foreach (String t in kxmc)
            {
                bdz.Nodes.Add(t,t);
                progressBar1.PerformStep();
            }
            progressBar1.Value = 0;

        }

        /// <summary>
        /// 窗口关闭之后的操作
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void FeederManagement_FormClosed(object sender, FormClosedEventArgs e)
        {
            //this.Text = "请稍后,正在提交数据...";
            //DBManager.Instance.Submit();
        }


        /// <summary>
        /// 将要张开节点前,导入对应的子节点信息
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void treeView1_BeforeExpand(object sender, TreeViewCancelEventArgs e)
        {
            TreeNode senderNode = e.Node;
            //判断是区局,变电站,还是馈线            
            if (GetTreeNodeLev(senderNode) == 2)//变电站
            {
                AddKX(senderNode);//添加馈线
            }
            else if (GetTreeNodeLev(senderNode) == 1)//区局
            {
                AddBDZ(senderNode);//添加变电站
            }
        }
        /// <summary>
        /// 设置所有按钮失效
        /// </summary>
        public void NullityButtons()
        {
            AddButton.Enabled = false;
            EditButton.Enabled = false;
            DeleteButton.Enabled = false;
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
            //判断是区局,变电站,还是馈线
            if (GetTreeNodeLev(senderNode) == 3)//馈线
            {
                AddButton.Enabled = true;
                EditButton.Enabled = true;
                DeleteButton.Enabled = true;
                curBDZ = senderNode.Parent;
            }
            else if (GetTreeNodeLev(senderNode) == 2)//变电站
            {
                AddButton.Enabled = true;
                EditButton.Enabled = false;
                DeleteButton.Enabled = false;
                curBDZ = senderNode;
            }
            else
            {
                AddButton.Enabled = false;
                EditButton.Enabled = false;
                DeleteButton.Enabled = false;
                curBDZ = null;
            }
        }

        private void FeederManagement_Load(object sender, EventArgs e)
        {
            AddQJ();

            progressBar1.Step = 1;
            progressBar1.Value = 0;
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
            else
                return 1;
        }


        private void FeederManagement_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Escape)
            {
                //treeView1.Nodes.Clear();
                this.Close();
            }
        }

        private void treeView1_MouseDoubleClick(object sender, MouseEventArgs e)
        {
            button2_Click(sender, e);
        }
    }
}
