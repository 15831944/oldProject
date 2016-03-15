using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using ElectronTransferBll.DBEntityHelper;
using ElectronTransferDal.Query;
using ElectronTransferModel.V9_4;
using ElectronTransferDal.Cad;
using Autodesk.AutoCAD.DatabaseServices;
using ElectronTransferFramework;

namespace ElectronTransferView.ContextMenuManager
{
    
    public partial class SDKXView : Form
    {
        IEnumerable<Cd_ssxl> sdkxs;
        private static IList<long> fids;
        /// <summary>
        /// 保存有所属馈线的设备的公共属性
        /// </summary>
        private static IEnumerable<Common_n> _sdkxAcCommons { get; set; }
        /// <summary>
        /// 保存所有设备的所属受电馈线的并集
        /// </summary>
        private static IEnumerable<Gg_pd_sdkx_ac> _sdkxAcUnion { set; get; }
        public SDKXView()
        {
            InitializeComponent();
            BindSSKXCombox();
            GetEntities();
        }

        /// <summary>
        /// 获取选择设备的受电馈线的并集
        /// </summary>
        private void GetSymbolSDKXUnion()
        {          
            try
            {
                comboBox_sc_bdz.Items.Clear();
                comboBox_sc_kxmc.Items.Clear();
                //获取选择设备的所属馈线集合
                _sdkxAcUnion = DBEntityFinder.Instance.GetSDKXByG3e_FIDS(fids);
                if (_sdkxAcUnion!=null && _sdkxAcUnion.Any())
                {
                    //绑定所属变电
                    var gdbdzs = _sdkxAcUnion.Select(o => o.GDBDZ).Distinct();
                    foreach (var tmp in gdbdzs)
                    {
                        if (String.IsNullOrEmpty(tmp))
                            continue;
                        comboBox_sc_bdz.Items.Add(tmp);
                    }
                    comboBox_sc_bdz.SelectedIndex = 0;
                }
            }
            catch(Exception ex)
            {
                LogManager.Instance.Error(ex);
            }
        }

        /// <summary>
        /// 绑定Combox
        /// </summary>
        private void BindSSKXCombox()
        {
            try
            {
                //没有供电局则默认查询所有
                sdkxs = DBEntityFinder.Instance.GetSDKXXG();

                if (sdkxs != null)
                {
                    var gdbdzs = sdkxs.Select(o => o.SSBDZ).Distinct();
                    foreach (String tmp in gdbdzs)
                    {
                        if (String.IsNullOrEmpty(tmp))
                            continue;
                        comboBox_xg_bdz.Items.Add(tmp);
                    }
                }
            }
            catch(Exception ex)
            {
                LogManager.Instance.Error(ex);
            }
        }
        /// <summary>
        /// 获取有受电馈线设备的公共属性
        /// </summary>
        /// <param name="objIds"></param>
        /// <param name="fidList"></param>
        /// <returns></returns>
        private IEnumerable<Common_n> GetSDKXCommon(ObjectIdCollection objIds,ref IList<long> fidList)
        {
            var listCommon = new List<Common_n>();
            foreach (ObjectId objectId in objIds)
            {
                G3EObject g3eObject = null;
                if (DBEntityFinder.Instance.GetG3EIds(objectId, ref g3eObject))
                {
                    var entity = PublicMethod.Instance.GetObject(objectId, OpenMode.ForRead);
                    if (entity is DBText || entity is MText)
                        continue;
                    //工单校验
                    var isLTTID=DBEntityFinder.Instance.VerifyLTTID(objectId);
                    if (isLTTID)
                    {
                        //查询设备是否存在受电馈线
                        var value = DBEntityFinder.Instance.GetSymbolConfigByG3eFNO(g3eObject.G3E_FNO);
                        if (!string.IsNullOrEmpty(value.ComponentTable.Gg_Pd_Sdkx_Ac))
                        {
                            //查询公共属性
                            var common = DBEntityFinder.Instance.GetCommon_n(g3eObject.G3E_FID);
                            if (common == null) continue;
                            listCommon.Add(common);
                            fidList.Add(g3eObject.G3E_FID);
                        }
                    }
                }
            }
            return listCommon;
        }

        //按钮事件
        private void button_delete_Click(object sender, EventArgs e)
        {
            try
            {
                foreach (var radioButton in GBox_Option.Controls.OfType<RadioButton>().Where(radioButton => (radioButton).Checked))
                {
                    var bdz = comboBox_xg_bdz.Text.Trim();
                    var kxmc = comboBox_xg_kxmc.Text.Trim();
                    switch (radioButton.Tag.ToString())
                    {
                        case "add":
                            if (IsNotNull(bdz, kxmc))
                                AddSDKX(bdz, kxmc);
                            break;
                        case "update":
                            if (IsNotNull(bdz, kxmc))
                                UpdateSDKX(bdz, kxmc);
                            break;
                        case "del":
                            {
                                DelSDKX();
                            }
                            break;
                    }
                    Refresh();
                }
            }
            catch (Exception ex)
            {
                LogManager.Instance.Error(ex);
            }
        }
        /// <summary>
        /// 判断
        /// </summary>
        /// <param name="bdz"></param>
        /// <param name="kxmc"></param>
        /// <returns></returns>
        private bool IsNotNull(string bdz, string kxmc)
        {
            if (string.IsNullOrEmpty(bdz) && string.IsNullOrEmpty(kxmc))
            {
                MessageBox.Show("变电站或馈线不能为空！", "系统提示", MessageBoxButtons.OK, MessageBoxIcon.Question);
                return false;
            }
            if (!HasBDZ(bdz, kxmc))
            {
                MessageBox.Show("变电站或馈线填写错误！", "系统提示", MessageBoxButtons.OK, MessageBoxIcon.Question);
                return false;
            }
            return true;
        }
        /// <summary>
        /// 值是否存在
        /// </summary>
        /// <param name="bdz"></param>
        /// <param name="kxmc"></param>
        /// <returns></returns>
        private bool HasBDZ(string bdz, string kxmc)
        {
            return sdkxs != null && sdkxs.Any(o => o.SSBDZ == bdz && o.NAME == kxmc);
        }

        /// <summary>
        /// 新增受电馈线
        /// </summary>
        /// <param name="bdz"></param>
        /// <param name="kxmc"></param>
        private void AddSDKX(string bdz,string kxmc)
        {
            if (_sdkxAcCommons != null && _sdkxAcCommons.Any())
            {
                IList<long> errors = new List<long>();
                foreach (var com in _sdkxAcCommons)
                {
                    var kxs = DBEntityFinder.Instance.GetSdkxAcByG3e_FID(com.G3E_FID);
                    var kxh = kxs.Where(o => o.KXH == kxmc && o.GDBDZ==bdz);
                    if (!kxh.Any())
                    {
                        var id = InsertDBEntity.InsertSDKX(com, bdz, kxmc);
                        if (id == 0)
                            errors.Add(com.G3E_FID);
                    }
                }
                if (errors.Any())
                {
                    GetError("新增失败！", errors);
                }
                else
                {
                    MessageBox.Show("新增成功！", "系统提示", MessageBoxButtons.OK, MessageBoxIcon.Question);
                }
                //重新绑定
                GetSymbolSDKXUnion();
            }
        }
        private void GetError(string tip,IList<long> errors)
        {
            if (errors.Any())
            {
                var error = new StringBuilder();
                foreach (long t in errors)
                {
                    error.Append(t + "\n");
                }
                MessageBox.Show(string.Format("{0}\n{1}",tip, error), "系统提示", MessageBoxButtons.OK, MessageBoxIcon.Question);
            }
        }
        /// <summary>
        /// 更新受电馈线
        /// </summary>
        /// <param name="bdz"></param>
        /// <param name="kxmc"></param>
        private void UpdateSDKX(string bdz, string kxmc)
        {
            if (_sdkxAcUnion != null && _sdkxAcUnion.Any())
            {
                var oldbdz = comboBox_sc_bdz.Text;
                var oldkx = comboBox_sc_kxmc.Text;
                if (string.IsNullOrEmpty(oldbdz) && string.IsNullOrEmpty(oldkx))
                {
                    return;
                }
                IList<long> errors = new List<long>();
                foreach (var sdkx in _sdkxAcUnion)
                {
                    if (sdkx.GDBDZ.Equals(oldbdz) && sdkx.KXH.Equals(oldkx))
                    {
                        var isUpdate = UpdateDBEntity.UpdateSDKX_AC(bdz, kxmc, sdkx);
                        if (!isUpdate)
                        {
                            errors.Add(sdkx.G3E_FID);
                        }
                        else
                        {
                            var duplicated = DBEntityFinder.Instance.DuplicatedSdkx(sdkx.G3E_FID, bdz, kxmc);
                            //判断同一个设备是否有重复的受电馈线
                            if (duplicated)
                            {
                                DeleteDBEntity.DeleteSDKX(sdkx.G3E_ID);
                            }
                        }
                    }
                }
                if (errors.Any())
                {
                    GetError("更新失败！", errors);
                }
                else
                {
                    MessageBox.Show("更新成功！", "系统提示", MessageBoxButtons.OK, MessageBoxIcon.Question);
                }
                //重新绑定
                GetSymbolSDKXUnion();
            }
        }

        private void DelSDKX()
        {
            if (_sdkxAcUnion != null && _sdkxAcUnion.Any())
            {
                var oldbdz = comboBox_sc_bdz.Text;
                var oldkx = comboBox_sc_kxmc.Text;
                if (string.IsNullOrEmpty(oldbdz) && string.IsNullOrEmpty(oldkx))
                {
                    MessageBox.Show("变电站或馈线不能为空！", "系统提示", MessageBoxButtons.OK, MessageBoxIcon.Question);
                    return;
                }
                if (MessageBox.Show("确定要删除吗？", "系统提示", MessageBoxButtons.OKCancel, MessageBoxIcon.Question) == DialogResult.OK)
                {
                    IList<long> errors = new List<long>();
                    //删除受电馈线
                    foreach (var sdkx in _sdkxAcUnion.Where(sdkx => sdkx.GDBDZ.Equals(oldbdz) && sdkx.KXH.Equals(oldkx))
                        )
                    {
                        var isDel = DeleteDBEntity.DeleteSDKX(sdkx);
                        if (!isDel) errors.Add(sdkx.G3E_FID);
                    }
                    if (errors.Any())
                    {
                        GetError("删除失败！", errors);
                    }
                    else
                    {
                        MessageBox.Show("删除成功！", "系统提示", MessageBoxButtons.OK, MessageBoxIcon.Question);
                    }
                    //重新绑定
                    GetSymbolSDKXUnion();
                }
            }
        }

        private void button_clear_Click(object sender, EventArgs e)
        {
            try
            {
                _sdkxAcCommons = null; 
                richTextBox_FID.Text = string.Empty;
            }
            catch(Exception ex)
            {
                LogManager.Instance.Error(ex);
            }
        }
        /// <summary>
        /// 新增选项
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void radioButton_add_CheckedChanged(object sender, EventArgs e)
        {
            groupBox3.Enabled = true;
            groupBox2.Enabled = true;
            groupBox3.Text = "馈线集合";
            groupBox2.Text = "新增";
            button_delete.Text = "新增";
            button_clear.Enabled = true;
            button_delete.Enabled = true;
            comboBox_sc_bdz.Enabled = true;
            comboBox_sc_kxmc.Enabled = true;
        }
        //删除选项
        private void radioButton_clear_CheckedChanged(object sender, EventArgs e)
        {
            groupBox3.Text = "删除";
            groupBox2.Text = "修改";
            groupBox2.Enabled=false;
            groupBox3.Enabled = true;
            button_delete.Text = "删除";
            button_clear.Enabled = true;
            button_delete.Enabled = true;
        }
        /// <summary>
        /// 修改选项
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void radioButton_chexiao_CheckedChanged(object sender, EventArgs e)
        {
            groupBox3.Text = "要修改的受电馈线";
            groupBox2.Text = "修改";
            groupBox3.Enabled = true;
            groupBox2.Enabled = true;
            button_delete.Text = "修改";
        }
  
        /// <summary>
        /// 选择设备
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void button_changeEnts_Click(object sender, EventArgs e)
        {
            GetEntities();
        }
        /// <summary>
        /// 选择实体
        /// </summary>
        private void GetEntities()
        {
            try
            {
                richTextBox_FID.Clear();
                fids = new List<long>();
                string strfid = string.Empty;
                Visible = false;
                var objectIds = PublicMethod.Instance.SelectEntities("选择集\n");
                Visible = true;
                _sdkxAcCommons = GetSDKXCommon(objectIds, ref fids);
                GetSymbolSDKXUnion();
                if (fids.Any())
                {
                    foreach (var fid in fids)
                    {
                        strfid += string.Format("{0},", fid);
                    }
                    strfid = strfid.Substring(0, strfid.Length - 1);
                    richTextBox_FID.Text = strfid;
                }
            }
            catch (Exception ex)
            {
                LogManager.Instance.Error(ex);
            }      
        }
        private void comboBox_xg_bdz_SelectedIndexChanged(object sender, EventArgs e)
        {
            try
            {
                comboBox_xg_kxmc.Items.Clear();

                var bdz = comboBox_xg_bdz.Text;
                var kxhs = from quer in sdkxs
                           where quer.SSBDZ == bdz
                           orderby quer.NAME ascending
                           select quer;
                foreach (var tmp in kxhs)
                {
                    if (String.IsNullOrEmpty(tmp.NAME))
                        continue;
                    comboBox_xg_kxmc.Items.Add(tmp.NAME);
                }
                comboBox_xg_kxmc.SelectedIndex = 0;
            }
            catch (Exception ex)
            {
                LogManager.Instance.Error(ex);
            }
        }

        private void comboBox_sc_bdz_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (_sdkxAcUnion != null && _sdkxAcUnion.Any())
            {
                var bdz = comboBox_sc_bdz.Text;
                comboBox_sc_kxmc.Items.Clear();
                //根据变电站筛选馈线
                var kxs = from quer in _sdkxAcUnion
                          where quer.GDBDZ == bdz
                          orderby quer.KXH ascending
                          select quer;
                foreach (var kx in kxs.Select(o=>o.KXH).Distinct())
                {
                    comboBox_sc_kxmc.Items.Add(kx);
                }
                comboBox_sc_kxmc.SelectedIndex = 0;
            }
        }
    }
}
