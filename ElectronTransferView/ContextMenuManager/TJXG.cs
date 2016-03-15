using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using ArxMap;
using ElectronTransferBll.DBEntityHelper;
using ElectronTransferDal.Cad;
using ElectronTransferDal.Common;
using ElectronTransferDal.Common.Exceptions;
using ElectronTransferDal.Query;
using ElectronTransferFramework;
using ElectronTransferModel;
using ElectronTransferModel.Geo;
using ElectronTransferModel.V9_4;

namespace ElectronTransferView.ContextMenuManager
{
    public partial class TJXG : Form
    {
        public static event SHBMap.SyncPanelData TJXGToPanel;
        // 变压器对象
        private G3EObject g3eObject;
        // 变压器功能位置
        private Gg_pd_gnwzmc_n byq_gnwz;
        // 台架的功能位置
        private Gg_pd_gnwzmc_n tj_gnwz;
        // 台架的公共属性
        private Common_n tj_comm;
        // 台架级联设备的功能位置
        private IEnumerable<Gg_pd_gnwzmc_n> tj_devs_gnwz;
        // 台架级联设备公共属性
        private List<Common_n> tj_devs_comm;
        private string tj_name;
        public TJXG()
        { 
            tj_devs_comm = new List<Common_n>();
            InitializeComponent();
        }

        private void bt_choose_Click(object sender, EventArgs e)
        {
            try
            {
                Enabled = false;
                Visible = false;
                GetTJDBEntity();
                Enabled = true;
                Visible = true;
            }
            catch (Exception exception)
            {
                LogManager.Instance.Error(exception);
            }
        }
        /// <summary>
        /// 选择变压器
        /// </summary>
        private void GetTJDBEntity()
        {
            var objectId = PublicMethod.Instance.GetEntity("请选择变压器:\n");
            if (objectId.IsNull) return;
            var isLTTID = DBEntityFinder.Instance.VerifyLTTID(objectId);
            if (isLTTID)
            {
                if (DBEntityFinder.Instance.GetG3EIds(objectId, ref g3eObject))
                {
                    var ptValue = DBSymbolFinder.Instance[objectId];
                    if (ptValue.G3E_FNO == 148)
                    {
                        //var com = DBEntityFinder.Instance.GetCommonByG3e_FID(g3eObject.G3E_FID);
                        var b = GetDevsByBYQFid(g3eObject.G3E_FID);
                        if (b && tj_comm != null)
                        {
                            InitComBo();
                            SetText(tj_comm);
                            bt_Modify.Enabled = true;
                        }
                    }
                    else
                    {
                        bt_Modify.Enabled = false;
                        PublicMethod.Instance.AlertDialog("请选择变压器.\n");
                    }
                }
                else
                    bt_Modify.Enabled = false;
            }
            else
                bt_Modify.Enabled = false;
        }
        /// <summary>
        /// 得到变压器、台架、级联设备的功能位置与公共属性
        /// </summary>
        /// <param name="BYQFid">变压器G3E_FID</param>
        /// <returns></returns>
        private bool GetDevsByBYQFid(long BYQFid)
        {
            try
            {
                byq_gnwz = null;
                // 得到变压器的功能位置
                byq_gnwz = DBManager.Instance.GetEntities<Gg_pd_gnwzmc_n>(o=>o.G3E_FID ==BYQFid).FirstOrDefault();

                if (byq_gnwz != null)
                {
                    long tj_fid;
                    long.TryParse(byq_gnwz.GNWZ_SSTJ, out tj_fid);
                    // 得到台架的功能位置
                    tj_gnwz = null;
                    tj_gnwz = DBManager.Instance.GetEntities<Gg_pd_gnwzmc_n>(o => o.G3E_FID == tj_fid).FirstOrDefault();
                    if (tj_gnwz != null)
                    {
                        tj_comm = null;
                        tj_comm =
                            DBManager.Instance.GetEntities<Common_n>(o => o.G3E_FID == tj_gnwz.G3E_FID).FirstOrDefault();
                        tj_devs_gnwz = null;
                        // 得到台架级联设备的功能位置
                        tj_devs_gnwz =
                            DBManager.Instance.GetEntities<Gg_pd_gnwzmc_n>(o => o.GNWZ_SSTJ == tj_gnwz.G3E_FID.ToString());
                        if (tj_devs_gnwz != null && tj_devs_gnwz.Any())
                        {
                            tj_devs_comm.Clear();
                            // 得到台架级联设备的公共属性
                            tj_devs_comm.AddRange(
                                tj_devs_gnwz.Select(d => DBManager.Instance.GetEntity<Common_n>(d.G3E_FID))
                                    .Where(dc => dc != null));
                            return true;
                        }
                    }
                    else
                    {
                        PublicMethod.Instance.ShowMessage("此变压器没有台架信息。\n");
                    }
                }
                else
                {
                    PublicMethod.Instance.ShowMessage("此变压器没有功能位置信息。\n");
                }
                return false;
            }
            catch (NotExistException ex)
            {
                PublicMethod.Instance.NotExistTable(ex);
                return GetDevsByBYQFid(BYQFid);
            }
        }

        private void SetText(Common_n com)
        {
            if (com != null)
            {
                // FID
                tb_fid.Text = com.G3E_FID.ToString();
                // 名称
                tb_mc.Text = com.SBMC;
                tj_name = com.SBMC;
                //// 所属单位
                //cb_ssdw.Text = com.CD_SSDW;
                //// 所属变电站
                //cb_ssbdz.Text = com.CD_SSBDZ;
                //// 所属线路
                //cb_ssxl.Text = com.CD_SSXL;
                //// 电压等级
                //cb_dydj.Text = com.CD_DYDJ;
                //// 生命周期
                //cb_smzq.Text = com.CD_SMZQ;
                //// 所属供电所
                //cb_ssgds.Text = com.GNWZ_SSGDS;
                //台架下所挂的设备
                var str_devs = tj_devs_comm.Aggregate("", (current, d) => current + (d.G3E_FID + ":" + d.G3E_FNO + ":" + d.SBMC + "\n"));
                rtb_sgsb.Text = str_devs;
            }
        }

        /// <summary>
        /// 修改级联设备
        /// </summary>
        /// <param name="oldName"></param>
        /// <param name="newName"></param>
        private void ModifyDependence(string oldName, string newName)
        {
            if (string.IsNullOrEmpty(newName)) return;
            if (oldName.Equals(newName))
            {
                PublicMethod.Instance.AlertDialog("修改前后名称一样,修改失败！\n");
                return;
            }
            // 更新台架的功能位置
            if (tj_gnwz == null) return;
            tj_gnwz.MC = newName;
            var result = UpdateDBEntity.UpdateGNWZ(tj_gnwz);
            if (!result)
            {
                LogManager.Instance.Error("更新台架功能位置属性出错!\n");
            }
            //更新台架自身公共属性
            if (tj_comm == null) return;
            tj_comm.SBMC = newName; //新名称
            tj_comm.CD_SSDW = cb_ssdw.Text; // 所属单位
            tj_comm.CD_SSBDZ = cb_ssbdz.Text; // 所属变电站
            tj_comm.CD_SSXL = cb_ssxl.Text; // 所属线路
            tj_comm.CD_DYDJ = cb_dydj.Text; // 电压等级
            tj_comm.CD_SMZQ = cb_smzq.Text; // 生命周期
            //tj_comm.GNWZ_SSGDS = cb_ssgds.Text; // 所属供电所
            result = UpdateDBEntity.UpdateCommon(tj_comm);
            if (!result) return;
            //更新台架标注
            UpdateDBText(g3eObject.G3E_FID, newName, oldName);
            //更新级联设备公共属性
            if (tj_devs_comm.Any())
            {
                foreach (var com in tj_devs_comm)
                {
                    if (string.IsNullOrEmpty(com.SBMC)) continue;
                    var oldTextString = com.SBMC;
                    if (string.IsNullOrEmpty(oldName) || !com.SBMC.Contains(oldName)) continue;
                    var newSBMC = com.SBMC.Replace(oldName, newName);
                    com.SBMC = newSBMC;
                    if (com.G3E_FNO == 148)
                    {
                        com.CD_SSDW = cb_ssdw.Text; // 所属单位
                        com.CD_SSBDZ = cb_ssbdz.Text; // 所属变电站
                        com.CD_SSXL = cb_ssxl.Text; // 所属线路
                        if(!String.IsNullOrEmpty(cb_dydj.Text))
                            com.CD_DYDJ = cb_dydj.Text; // 电压等级
                        com.CD_SMZQ = cb_smzq.Text; // 生命周期
                        //com.GNWZ_SSGDS = cb_ssgds.Text; // 所属供电所
                    }
                    var bl = UpdateDBEntity.UpdateCommon(com);
                    if (bl)
                    {
                        //更新从属设备的标注
                        UpdateDBText(com.G3E_FID, newSBMC, oldTextString);
                    }
                }
                //更新级联设备功能位置属性
                foreach (var gnwz in tj_devs_gnwz)
                {
                    if (string.IsNullOrEmpty(gnwz.MC)) continue;
                    if (string.IsNullOrEmpty(oldName) || !gnwz.MC.Contains(oldName)) continue;
                    var newSBMC = gnwz.MC.Replace(oldName, newName);
                    gnwz.MC = newSBMC;
                    UpdateDBEntity.UpdateGNWZ(gnwz);
                }
                PublicMethod.Instance.ShowMessage("修改成功!\n");
            }
            else
            {
                PublicMethod.Instance.AlertDialog("台架无级联设备！");
            }
            //textBox_DF_OLDNAME.Text = newName;
        }

        private void UpdateDBText(long g3e_fid, string newTextString, string oldTextString)
        {
            //获取标注对象
            var values = DBEntityFinder.Instance.GetDBSymbolFinderByFidOrEntityType(g3e_fid, EntityType.Label);

            var objList = values.Select(id => id.Key).ToList();
            //批量更新标注
            foreach (var objectId in objList)
            {
                SymbolLabel.UpdateDBText(objectId, newTextString, oldTextString);
            }
        }

        private void InitComBo()
        {
            //初始化combobox控件;
            // 所属单位 
            cb_ssdw.TextChanged -= cb_ssdw_TextChanged;
            var ssdw = CDDBManager.Instance.GetEntities<Cd_ssxl>().Select(o => o.CD_SSDW).Distinct().ToList();
            cb_ssdw.DataSource = ssdw;
            cb_ssdw.Text = tj_comm.CD_SSDW;
            cb_ssdw.TextChanged += cb_ssdw_TextChanged;
            // 所属变电站
            this.cb_ssbdz.TextChanged -= new System.EventHandler(this.cb_ssbdz_TextChanged);
            var ssbdz = CDDBManager.Instance.GetEntities<Cd_ssxl>().Select(o => o.SSBDZ).Distinct().ToList();
            cb_ssbdz.DataSource = ssbdz;
            cb_ssbdz.Text = tj_comm.CD_SSBDZ;
            this.cb_ssbdz.TextChanged += new System.EventHandler(this.cb_ssbdz_TextChanged);
            // 所属线路 
            List<string> ssxl;
            if (tj_comm.CD_SSBDZ == null)
                ssxl = CDDBManager.Instance.GetEntities<Cd_ssxl>().Select(o => o.NAME).ToList();
            else
                ssxl = CDDBManager.Instance.GetEntities<Cd_ssxl>().Where(o => o.SSBDZ == tj_comm.CD_SSBDZ).Select(o => o.NAME).ToList();
            cb_ssxl.DataSource = ssxl;
            cb_ssxl.Text = tj_comm.CD_SSXL;
            // 所属供电所
            var ssgds = CDDBManager.Instance.GetEntities<Cd_bs>().Where(o => o.SSDW == cb_ssdw.Text).Select(o => o.NAME).ToList();
            cb_ssgds.DataSource = ssgds;
            //cb_ssgds.Text = tj_comm.GNWZ_SSGDS;
            // 电压等级
            var dydj = CDDBManager.Instance.GetEntities<Cd_dydj>().Select(o => o.NAME).ToList();
            cb_dydj.DataSource = dydj;
            cb_dydj.Text = tj_comm.CD_DYDJ;
            // 生命周期
            var smzq = CDDBManager.Instance.GetEntities<Cd_smzq>().Select(o => o.NAME).ToList();
            cb_smzq.DataSource = smzq;
            cb_smzq.Text = tj_comm.CD_SMZQ;
        }

        private void TJXG_Load(object sender, EventArgs e)
        {
            try
            {
                var objectId = mouse.selectedEntityId;
                if (!objectId.IsNull)
                {
                    var isLTTID = DBEntityFinder.Instance.VerifyLTTID(objectId);
                    if (isLTTID)
                    {
                        var pt = DBSymbolFinder.Instance[objectId];
                        if (pt.G3E_FNO == 148)
                        {
                            g3eObject = new G3EObject();
                            bt_Modify.Enabled = true;
                            g3eObject.G3E_FID = pt.G3E_FID;
                            g3eObject.G3E_FNO = pt.G3E_FNO;
                            g3eObject.G3E_ID = pt.G3E_ID;
                            //var com = DBEntityFinder.Instance.GetCommonByG3e_FID(pt.G3E_FID);
                            var b = GetDevsByBYQFid(g3eObject.G3E_FID);
                            if (b && tj_comm != null)
                            {
                                InitComBo();
                                SetText(tj_comm);
                                //bt_Modify.Enabled = false;
                            }
                            //else
                            //    bt_Modify.Enabled = false;
                        }
                        //else
                            //bt_Modify.Enabled = false;
                    }
                }
                bt_Modify.Enabled = false;
            }
            catch (NotExistException ex)
            {
                PublicMethod.Instance.NotExistTable(ex);
            }
            catch (Exception exception)
            {
                LogManager.Instance.Error(exception);
            }
        }

        private void bt_Modify_Click(object sender, EventArgs e)
        {
            try
            {
                var newName = tb_mc.Text.Trim();
                if (string.IsNullOrEmpty(newName))
                {
                    tb_mc.Focus();
                    MessageBox.Show("名称不能为空！", "系统提示", MessageBoxButtons.OK, MessageBoxIcon.Stop);
                    return;
                }
                if (MessageBox.Show("确定要修改？", "系统提示", MessageBoxButtons.YesNo, MessageBoxIcon.Question) ==
                    DialogResult.Yes)
                {
                    ModifyDependence(tj_name, newName);
                    //更新台架下所挂的设备
                    var str_devs = tj_devs_comm.Aggregate("",
                        (current, d) => current + (d.G3E_FID + ":" + d.G3E_FNO + ":" + d.SBMC + "\n"));
                    rtb_sgsb.Text = str_devs;
                    tj_name = newName;
                    // 同步属性窗口
                    if (TJXGToPanel != null)
                    {
                        TJXGToPanel(g3eObject.G3E_FNO, g3eObject.G3E_FID);
                    }
                    Autodesk.AutoCAD.ApplicationServices.Application.UpdateScreen();
                    bt_Modify.Enabled = false;
                }
            }
            catch (Exception exception)
            {
                LogManager.Instance.Error(exception);
            }

        }

        private void cb_ssdw_TextChanged(object sender, EventArgs e)
        {
            //重置所属供电所
            var ssgds = CDDBManager.Instance.GetEntities<Cd_bs>().Where(o => o.SSDW == cb_ssdw.Text).Select(o => o.NAME).ToList();
            cb_ssgds.DataSource = ssgds;
            cb_ssgds.Text = "";
            // 所属变电站
            var ssbdz = CDDBManager.Instance.GetEntities<Cd_ssxl>().Where(o => o.CD_SSDW == cb_ssdw.Text).Select(o => o.SSBDZ).Distinct().ToList();
            cb_ssbdz.DataSource = ssbdz;
            cb_ssbdz.Text = "";
            //重置所属馈线
            var ssxl = CDDBManager.Instance.GetEntities<Cd_ssxl>().Where(o => o.SSBDZ == cb_ssbdz.Text).Select(o => o.NAME).ToList();
            cb_ssxl.DataSource = ssxl;
            cb_ssxl.Text = "";
            //cb_ssgds.SelectedIndex = 0;
            //cb_ssxl.SelectedIndex = 0;
            bt_Modify.Enabled = true;
        }

        private void cb_ssbdz_TextChanged(object sender, EventArgs e)
        {
            //重置所属馈线
            var ssxl = CDDBManager.Instance.GetEntities<Cd_ssxl>().Where(o => o.SSBDZ == cb_ssbdz.Text).Select(o => o.NAME).ToList();
            cb_ssxl.DataSource = ssxl;
            cb_ssxl.Text = "";
        }

        private void tb_mc_TextChanged(object sender, EventArgs e)
        {
            bt_Modify.Enabled = true;
        }
    }
}
