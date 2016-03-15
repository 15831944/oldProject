using System;
using System.Linq;
using System.Windows.Forms;
using ArxMap;
using ElectronTransferBll.DBEntityHelper;
using ElectronTransferDal.Cad;
using ElectronTransferDal.Query;
using ElectronTransferModel;
using ElectronTransferModel.Geo;
using ElectronTransferModel.V9_4;


namespace ElectronTransferView.ContextMenuManager
{
    public partial class DFMCJLXG : Form
    {
        private G3EObject g3eObject;
        public DFMCJLXG()
        {
            InitializeComponent();
        }

        private void button_Change_Click(object sender, EventArgs e)
        {
            Enabled = false;
            Visible = false;
            GetDFDBEntity();
            Enabled = true;
            Visible = true;
        }

        private void button_Modify_Click(object sender, EventArgs e)
        {
            var newName = textBox_DF_NEWNAME.Text.Trim();
            if(string.IsNullOrEmpty(newName))
            {
                textBox_DF_NEWNAME.Focus();
                MessageBox.Show("名称不能为空！", "系统提示", MessageBoxButtons.OK, MessageBoxIcon.Stop);
                return;
            }
            if (MessageBox.Show("确定要修改？", "系统提示", MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.Yes)
            {
                var bMod = ModifyDFName();
                var result = bMod ? "修改成功！" : "修改失败！";
                PublicMethod.Instance.AlertDialog(result);
            }
        }

        /// <summary>
        /// 获取电房信息
        /// </summary>
        /// <returns></returns>
        private void GetDFDBEntity()
        {
            var objectId=PublicMethod.Instance.GetEntity("请选择电房");
            if (objectId.IsNull) return;
            var isLTTID=DBEntityFinder.Instance.VerifyLTTID(objectId);
            if (isLTTID)
            {
                if (DBEntityFinder.Instance.GetG3EIds(objectId, ref g3eObject))
                {
                    var ptValue = DBSymbolFinder.Instance[objectId];
                    if (ptValue.G3E_GEOMETRY is Polygon)
                    {
                        button_Modify.Enabled = true;
                        var com = DBEntityFinder.Instance.GetCommonByG3e_FID(g3eObject.G3E_FID);
                        SetText(com);
                    }
                    else
                        button_Modify.Enabled = false;
                }
                else
                    button_Modify.Enabled = false;
            }
            else
                button_Modify.Enabled = false;
        }
       
        /// <summary>
        /// 修改集联设备名称
        /// </summary>
        private bool ModifyDFName()
        {
            var oldName = textBox_DF_OLDNAME.Text.Trim();
            var newName = textBox_DF_NEWNAME.Text.Trim();

            if (!string.IsNullOrEmpty(newName))
                ModifyDependence(oldName, newName);
           
            return true;
        }
        /// <summary>
        /// 修改从属
        /// </summary>
        /// <param name="oldName"></param>
        /// <param name="newName"></param>
        private void ModifyDependence(string oldName,string newName)
        {
            if (oldName.Equals(newName)) return;
            //获取自身公共属性
            var common = DBEntityFinder.Instance.GetCommonByG3e_FID(g3eObject.G3E_FID);
            if (common == null) return;
            //新名称
            common.SBMC = newName;
            //更新自身公共属性
            var result=UpdateDBEntity.UpdateCommon(common);
            if (result)
            {
                //更新电房标注
                UpdateDBText(g3eObject.G3E_FID, newName, oldName);
                //获取从属
                var coms = DBEntityFinder.Instance.GetCommonsByG3e_ID(common.G3E_ID);
                if (coms.Any())
                {
                    //更新从属
                    foreach (var com in coms)
                    {
                        if (string.IsNullOrEmpty(com.SBMC)) continue;
                        var oldTextString = com.SBMC;
                        if (!string.IsNullOrEmpty(oldName)&&com.SBMC.Contains(oldName))
                        {
                            var newSBMC = com.SBMC.Replace(oldName, newName);
                            com.SBMC = newSBMC;
                            var bl = UpdateDBEntity.UpdateCommon(com);
                            if (bl)
                            {
                                //更新从属设备的标注
                                UpdateDBText(com.G3E_FID, newSBMC, oldTextString);
                            }
                        }
                    }
                }
                else
                {
                    PublicMethod.Instance.AlertDialog("无从属关系！");
                }
                textBox_DF_OLDNAME.Text = newName;
            }
        }

        private void UpdateDBText(long g3e_fid, string newTextString, string oldTextString)
        {
            //获取标注对象
             var values=DBEntityFinder.Instance.GetDBSymbolFinderByFidOrEntityType(g3e_fid, EntityType.Label);

            var objList = values.Select(id => id.Key).ToList();
            //批量更新标注
            foreach(var objectId in objList)
            {
                SymbolLabel.UpdateDBText(objectId, newTextString, oldTextString);
            }
        }

        private void DFMCJLXG_Load(object sender, EventArgs e)
        {
            var objectId=mouse.selectedEntityId;
            if (!objectId.IsNull)
            {
                var isLTTID = DBEntityFinder.Instance.VerifyLTTID(objectId);
                if (isLTTID)
                {
                    var pt = DBSymbolFinder.Instance[objectId];
                    if (pt.G3E_GEOMETRY is Polygon)
                    {
                        g3eObject = new G3EObject();
                        button_Modify.Enabled = true;
                        g3eObject.G3E_FID = pt.G3E_FID;
                        g3eObject.G3E_FNO = pt.G3E_FNO;
                        g3eObject.G3E_ID = pt.G3E_ID;
                        var com = DBEntityFinder.Instance.GetCommonByG3e_FID(pt.G3E_FID);
                        SetText(com);
                    }
                }
            }
        }

        private void SetText(Common_n com)
        {
            if (com != null)
            {
                textBox_DF_FID.Text = com.G3E_FID.ToString();
                textBox_DF_OLDNAME.Text = com.SBMC;
            }
        }

        private void Btn_Close_Click(object sender, EventArgs e)
        {
            Close();
        }
    }
}
