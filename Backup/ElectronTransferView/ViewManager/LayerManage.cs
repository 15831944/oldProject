using System;
using System.Linq;
using System.Windows.Forms;
using Autodesk.AutoCAD.DatabaseServices;
using ElectronTransferDal.Cad;
using ElectronTransferFramework;

namespace ElectronTransferView.ViewManager
{
    public partial class LayerManage : UserControl
    {
        public LayerManage()
        {
            InitializeComponent();
        }

        private readonly string[] LayerGroupName = new [] {"背景地图","中压设备","低压设备","标注","工单锁定框","其他馈线"};
        private void LayerManage_Load(object sender, EventArgs e)
        {
            try
            {
                InitiationLayer();
            }catch(Exception ex)
            {
                LogManager.Instance.Error(ex);
            }
        }
        /// <summary>
        /// 初始化图层数据
        /// </summary>
        public void InitiationLayer()
        {
            DCadApi.isRefreshLayer = false;
            var bl = new bool[6];
            treeView1.Nodes.Clear();
            foreach (var tn in LayerGroupName.Select(lgn => new TreeNode(lgn)))
            {
                treeView1.Nodes.Add(tn);
            }
            using (PublicMethod.Instance.acDoc.LockDocument())
            {
                var db = HostApplicationServices.WorkingDatabase;
                using (var trans = db.TransactionManager.StartTransaction())
                {
                    var lt = PublicMethod.Instance.GetLayerTable(db);
                    foreach (var ltr in from ObjectId id in lt select (LayerTableRecord) trans.GetObject(id, OpenMode.ForRead))
                    {
                        if (ltr.Name.Contains("背景地图"))
                        {
                            SetTreeNode(ltr, bl, 0);
                        }else if(ltr.Name.Contains("标注"))
                        {
                            SetTreeNode(ltr, bl, 3);
                        }
                        else if (ltr.Name.Contains("低压"))
                        {
                            SetTreeNode(ltr, bl, 2);
                        }
                        else if(ltr.Name.Contains("工单"))
                        {
                            SetTreeNode(ltr, bl, 4);
                        }
                        else if(ltr.Name.Contains("lock"))
                        {
                            SetTreeNode(ltr, bl, 5);
                        }
                        else
                        {
                            if (ltr.Name!="0")
                            {
                                SetTreeNode(ltr, bl, 1);
                            }
                        }
                    }
                    trans.Commit();
                    for (var i = 0; i < bl.Length; i++)
                    {
                        treeView1.Nodes[i].Checked = bl[i];
                    }
                    treeView1.ExpandAll();
                    treeView1.SelectedNode = treeView1.Nodes[0];
                }
            }
        }
        /// <summary>
        /// 添加节点
        /// </summary>
        /// <param name="ltr"></param>
        /// <param name="bl"></param>
        /// <param name="index"></param>
        public void SetTreeNode(LayerTableRecord ltr,bool[] bl,int index)
        {
            var nodes = new TreeNode {Tag = ltr.ObjectId, Text = ltr.Name, Checked = !ltr.IsOff};
            treeView1.Nodes[index].Nodes.Add(nodes);
            if (nodes.Checked)
                bl[index] = nodes.Checked;
        }
        /// <summary>
        /// 系列节点 Checked 属性控制
        /// </summary>
        /// <param name="e"></param>
        public static void CheckControl(TreeViewEventArgs e)
        {
            if (e.Action == TreeViewAction.Unknown) return;
            if (e.Node == null || Convert.IsDBNull(e.Node)) return;
            CheckParentNode(e.Node);
            if (e.Node.Nodes.Count > 0)
            {
                CheckAllChildNodes(e.Node, e.Node.Checked);
            }
        }

        #region 私有方法

        private static void CheckAllChildNodes(TreeNode pn, bool IsChecked)
        {
            foreach (TreeNode tn in pn.Nodes)
            {
                tn.Checked = IsChecked;
                LockLayer(tn.Text,tn.Tag, !IsChecked);
                if (tn.Nodes.Count > 0)
                {
                    CheckAllChildNodes(tn, IsChecked);
                }
            }
        }
        private static void CheckParentNode(TreeNode curNode)
        {
            bool bChecked = false;

            if (curNode.Parent != null)
            {
                LockLayer(curNode.Text, curNode.Tag, !curNode.Checked);
                foreach (TreeNode node in curNode.Parent.Nodes)
                {
                    if (node.Checked)
                    {
                        bChecked = true;
                        break;
                    }
                }

                if (bChecked)
                {
                    curNode.Parent.Checked = true;
                    CheckParentNode(curNode.Parent);
                }
                else
                {
                    curNode.Parent.Checked = false;
                    CheckParentNode(curNode.Parent);
                }
            }
        }
        /// <summary>
        /// 锁定特殊层
        /// </summary>
        /// <param name="name"></param>
        /// <param name="obj"></param>
        /// <param name="isCheck"></param>
        private static void LockLayer(string name, object obj, bool isCheck)
        {
            bool isLocked = name.Contains("工单") || name == "lock";
            PublicMethod.Instance.SetLayerDisplay((ObjectId)obj, isCheck, isLocked);
        }
        #endregion

        private void treeView1_AfterCheck(object sender, TreeViewEventArgs e)
        {
            if (DCadApi.isRefreshLayer)
            {
                InitiationLayer();
            }
            else
            {
                CheckControl(e);
            }
        }

        private void ToolSM_Refresh_Click(object sender, EventArgs e)
        {
            InitiationLayer();
        }

        private void treeView1_MouseUp(object sender, MouseEventArgs e)
        {
            PublicMethod.Instance.RegenerateModel();
        }
    }
}
