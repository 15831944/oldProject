using System;
using System.Linq;
using System.Windows.Forms;
using ElectronTransferDal;
using ElectronTransferDal.AutoGeneration;
using ElectronTransferDal.Common;
using ElectronTransferDal.Query;
using ElectronTransferModel.V9_4;
using ElectronTransferModel;
using ElectronTransferDal.Cad;

namespace ElectronTransferView.FunctionManager
{
    public enum formType { regionAdd, regionEdit };

    public partial class RegionalEditor : Form
    {
        public RegionalEditor()
        {
            
        }
        private String RegionalFID, RegionalName;
        private formType m_ft;
        public RegionalEditor(formType type, String FID, String name)
        {
            m_ft = type;
            RegionalFID = FID;
            RegionalName = name;
            InitializeComponent();
        }

        private void RegionalEditor_Load(object sender, EventArgs e)
        {
            Text = m_ft == formType.regionAdd ? "添加支线" : "编辑支线";
            FidText.Text = RegionalFID;
            NameText.Text = RegionalName;
        }

        
        /// <summary>
        /// 点击确定按钮
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void OkButton_Click(object sender, EventArgs e)
        {
            DialogResult = DialogResult.OK;
            try
            {
                if (string.IsNullOrEmpty(FidText.Text))
                {
                    MessageBox.Show("FID不能为空！！！");
                    return;
                } 
                if (string.IsNullOrEmpty(NameText.Text.Trim()))
                {
                    MessageBox.Show("名称不能为空！！！");
                    return;
                }
                var sbmc = NameText.Text.Trim();
                var com = DBManager.Instance.GetEntity<Common_n>(o => o.G3E_FID == long.Parse(FidText.Text));
                if (com == null) return;
                if (m_ft == formType.regionAdd)//添加操作
                {
                    //查看是否重复添加
                    var kxNode = RegionalManagement.GetSeleteKX();
                    if (kxNode == null)
                    {
                        PublicMethod.Instance.ShowMessage("没有选择馈线,请再重试一遍\n");
                        //显示支线管理窗体
                        Autodesk.AutoCAD.ApplicationServices.Application.ShowModalDialog(RegionalManagement.GetCurActionForm());
                        return;
                    }
                    if (kxNode.Nodes.Cast<TreeNode>().Any(zx => zx.Name == FidText.Text))
                    {
                        PublicMethod.Instance.ShowMessage("支线已经存在\n");
                        return;
                    }
                    //获取属性
                    if (String.IsNullOrEmpty(FidText.Text)) throw new Exception();
                   
                    //查看是否删除状态
                    Gnwz_tz_zx newEnt = null;
                    var tmp =DBEntityFinder.Instance.GetGnwzTzZx(long.Parse(FidText.Text));
                    if (tmp != null && tmp.EntityState == EntityState.Delete)//删除状态,恢复数据
                    {
                        tmp.EntityState = EntityState.Update;
                        DBManager.Instance.Update(tmp);
                    }
                    else//否则
                    {
                        //插入支线表
                        newEnt = new Gnwz_tz_zx
                                     {
                                         ID = CYZCommonFunc.getid(),
                                         G3E_FID = com.G3E_FID,
                                         IN_TIME = DateTime.Now,
                                         EntityState = EntityState.Insert
                                     };
                    }
                    if (!com.Equals(sbmc))
                    {
                        //更新支线名称
                        com.SBMC = NameText.Text;
                        com.EntityState = EntityState.Update;
                        DBManager.Instance.Update(com);
                    }

                    //把支线添加入TreeView中
                    kxNode.Nodes.Add(newEnt.G3E_FID.ToString(), sbmc);
                    DBManager.Instance.Insert(newEnt);
                    //如果是原始的电缆或导线变成了支线也要更新公共表的备注字段
                    var propertyName = GenerateHelper.QueryZXField(com.G3E_FNO);
                    if (!string.IsNullOrEmpty(propertyName))
                    {
                        if (com.EntityState == EntityState.None)
                            com.EntityState = EntityState.Update;
                        com.SetAttribute(propertyName, com.G3E_FID.ToString());
                        DBManager.Instance.Update(com);
                    }
                }
                else//编辑操作
                {
                    //更新支线名称
                    com.SBMC = NameText.Text;
                    if (com.EntityState != EntityState.Insert)//如果之前不是insert
                    {
                        com.EntityState = EntityState.Update;//修改成update
                    }
                    DBManager.Instance.Update(com);

                    //更新TreeView中支线名称
                    var kxNode = RegionalManagement.GetSeleteKX();
                    if (kxNode != null)
                    {
                        kxNode.Nodes.Find(FidText.Text, true)[0].Text = sbmc;
                    }
                    else
                    {
                        PublicMethod.Instance.ShowMessage("没有正确选择支线");
                    }

                }
            }
            catch (Exception exception) {
                PublicMethod.Instance.ShowMessage("操作过程中出现错误"+exception);
            }
            finally
            {
                this.Close();
            }
            
        }

        private void CancelButton_Click(object sender, EventArgs e)
        {
            this.DialogResult = DialogResult.Cancel;
            this.Close();
        }
    }
}
