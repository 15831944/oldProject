using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using ArxMap;
using ElectronTransferDal.AutoGeneration;
using ElectronTransferDal.Query;
using ElectronTransferDal.Cad;
using ElectronTransferDal.Common;
using ElectronTransferFramework;
using ElectronTransferModel.Base;
using ElectronTransferView.Menu;
using ElectronTransferDal;
using ElectronTransferModel;
using ElectronTransferModel.Config;
using ElectronTransferModel.V9_4;
using ElectronTransferView.ViewManager;
using ElectronTransferBll.DBEntityHelper;

namespace ElectronTransferView.ContextMenuManager
{
    public partial class SwitchCabinetManage : Form
    {
        /// <summary>
        /// 按序列保存设备FID
        /// </summary>
        private Dictionary<int, long> dict = new Dictionary<int,long>();
        public long g3eFid = 0;
        private bool isModify = false;
        private bool isAdd = false;
        private bool isBulkModify = false;
        private int devIndex = 0;
        long g3eId { get; set; }
        long dfG3eFid { get; set; }
        /// <summary>
        /// 检测当前窗口是否被加载，由于是非模式对话框防止被加载多次
        /// </summary>
        public static bool IsLoadSwitchWnd = false;
        public SwitchCabinetManage(long g3e_id,long g3e_fid)
        {
            InitializeComponent();
            g3eId = g3e_id;
            dfG3eFid = g3e_fid;
            IsLoadSwitchWnd = true;
        }

        private void SwitchCabinetManage_Load(object sender, EventArgs e)
        {
            AddSwitch.ImageIndex = 0;
            ModifySwitch.ImageIndex = 2;
            DelSwitch.ImageIndex = 4;
            btSave.ImageIndex = 8;
            TZInfo.ImageIndex = 6;
            XProps kgg=GenerateObj.Instance.GenderObjByFno(198);
            GenerateHelper.SetAllBrowsable(kgg,true);
            pgSwitchCabinet.SelectedObject = kgg;
            var values = DBEntityFinder.Instance.GetSymbolByFno(198);
            GenerateHelper.Unrf.kggG3EID = g3eId;
            btSave.Enabled = false;
            if (values.Count > 0)
            {
                RefDict(values);
                if (lbSwitchCabinetcol.Items.Count > 0)
                    lbSwitchCabinetcol.SetSelected(0, true);
            }
        }
        private void RefDict(Dictionary<long,string> symbolDict)
        {
            int i = 0;
            dict.Clear();
            if (lbSwitchCabinetcol.Items.Count > 0)
                lbSwitchCabinetcol.Items.Clear();
            foreach (var fid in symbolDict.Keys)
            {
                var kgg = DBManager.Instance.GetEntity<Common_n>(fid);
                if (kgg==null)
                {
                    continue;
                }
                if (kgg.OWNER1_ID == g3eId)
                {
                    dict.Add(i, fid);
                    if (string.IsNullOrEmpty(symbolDict[fid]))
                        lbSwitchCabinetcol.Items.Add("FID= "+fid.ToString()+" 的开关柜缺失名称");
                    else
                        lbSwitchCabinetcol.Items.Add(symbolDict[fid]);
                    i++;
                }
            } 
            lbSwitchCabinetcol.Refresh();
        }
        private void ResetSwitch(bool flag)
        {
            //复位三个开关按钮
            AddSwitch.Enabled = flag;
            ModifySwitch.Enabled = flag;
            DelSwitch.Enabled = flag;
            btBulk.Enabled = flag;
            btSave.Enabled = !flag;
        }
        private void AddSwitch_Click(object sender, EventArgs e)
        {
            if (!DBEntityFinder.Instance.VerifyLTTID(dfG3eFid))
            {
                PublicMethod.Instance.Editor.WriteMessage("\n电房未在工单锁定范围之内，禁止编辑！！！\n");
                return;
            }
            try
            {
                
                if (lbSwitchCabinetcol.Items.Count > 0)
                    lbSwitchCabinetcol.SetSelected(lbSwitchCabinetcol.SelectedIndex, false);
                isModify = false;
                isAdd = true;

                XProps kgg = GenerateObj.Instance.GenderObjByFno(198);
                GenerateHelper.SetAllBrowsable(kgg,true);
                if (dict.Count > 0)
                {
                    var oldkgg = DevEventHandler.GetDevTables(198, dict[0]);
                    if (oldkgg.DevObj != null)
                        DBEntityFinder.Instance.CopyValueFromOldDev(kgg, oldkgg.DevObj);
                }
                else
                {
                    var dfCom = DBManager.Instance.GetEntity<Common_n>(dfG3eFid);
                    if (dfCom != null)
                    {
                        GenerateHelper.SetPropertyValue(kgg, "Common_n", "CD_SSDW", dfCom.CD_SSDW);
                        GenerateHelper.SetPropertyValue(kgg, "Common_n", "CD_SSBDZ", dfCom.CD_SSBDZ);
                        GenerateHelper.SetPropertyValue(kgg, "Common_n", "CD_SSXL", dfCom.CD_SSXL);
                        GenerateHelper.SetPropertyValue(kgg, "Common_n", "WHBS", dfCom.WHBS);
                        //GenerateHelper.SetPropertyValue(kgg, "Common_n", "GNWZ_SSGDS", dfCom.GNWZ_SSGDS);

                    }
                }
                pgSwitchCabinet.SelectedObject = kgg;
                pgSwitchCabinet.Enabled = true;
            }
            catch (Exception ex)
            {
                LogManager.Instance.Error(ex);
            }
            finally
            {
                ResetSwitch(false);
            }
        }

        private void ModifySwitch_Click(object sender, EventArgs e)
        {

            if (lbSwitchCabinetcol.Items.Count <= 0)
            {
                return;
            }
            int index = lbSwitchCabinetcol.SelectedIndex;
            if (index < 0) return;
            if (!DBEntityFinder.Instance.VerifyLTTID(dfG3eFid))
            {
                PublicMethod.Instance.Editor.WriteMessage("\n电房未在工单锁定范围之内，禁止编辑！！！\n");
                return;
            }
            try
            {
               
                var values = DevEventHandler.GetDevTables(198, dict[index]);
                GenerateHelper.SetAllBrowsable(values.DevObj,true);
                pgSwitchCabinet.SelectedObject = values.DevObj;
                pgSwitchCabinet.Enabled = true;
                isModify = true;
                isAdd = false;
            }
            catch (Exception ex)
            {
                LogManager.Instance.Error(ex);
            }
            finally
            {
                ResetSwitch(false);
            }

        }

        private void DelSwitch_Click(object sender, EventArgs e)
        {

            if (lbSwitchCabinetcol.Items.Count <= 0) return;
            int index = lbSwitchCabinetcol.SelectedIndex;
            if (index < 0) return;
            if (!DBEntityFinder.Instance.VerifyLTTID(dfG3eFid))
            {
                PublicMethod.Instance.Editor.WriteMessage("\n电房未在工单锁定范围之内，禁止编辑！！！\n");
                return;
            }
            try
            {
                string prompt;
                g3eFid = dict[index];
                var entitys = DBEntityFinder.Instance.GetSSKGG(g3eFid);
                if (entitys != null)
                    prompt = entitys.Any() ? "当前开关柜下已挂有开关,确定要删除该开关柜！" : "确定要删除该开关柜！";
                else
                    prompt = "确定要删除该开关柜！";
                if (MessageBox.Show(prompt, "温馨提示", MessageBoxButtons.OKCancel) != DialogResult.OK)
                    return;
                var values = DevEventHandler.GetDevTables(198, g3eFid);
                if (values.ComObj != null)
                {
                    if (values.ComObj.EntityState == EntityState.None)
                        values.ComObj.EntityState = EntityState.Delete;
                    DBManager.Instance.Delete(values.ComObj);
                }
                if (values.SelfObj != null)
                {
                    if (values.SelfObj.EntityState == EntityState.None)
                        values.SelfObj.EntityState = EntityState.Delete;
                    DBManager.Instance.Delete(values.SelfObj);
                }
                if (values.GnwzObj != null)
                {
                    if (values.GnwzObj.EntityState == EntityState.None)
                        values.GnwzObj.EntityState = EntityState.Delete;
                    DBManager.Instance.Delete(values.GnwzObj);
                }
                int indexDel = dict.SingleOrDefault(o => o.Value == g3eFid).Key;
                for (int i = indexDel; i < dict.Count - 1; i++) dict[i] = dict[i + 1];
                dict.Remove(dict.Count - 1);
                lbSwitchCabinetcol.Items.RemoveAt(indexDel);

                if (index != 0)
                {
                    lbSwitchCabinetcol.SetSelected(index - 1, true);
                    //var kgg = DevEventHandler.GetDevTables(198, dict[index - 1]);
                    
                    //pgSwitchCabinet.SelectedObject = kgg.DevObj;
                }
                else if (index == 0 && lbSwitchCabinetcol.Items.Count > 0)
                {
                    lbSwitchCabinetcol.SetSelected(0, true);
                    //var kgg = DevEventHandler.GetDevTables(198, dict[0]);
                    //GenerateHelper.SetAllReadOnly(kgg.DevObj,true);
                    //pgSwitchCabinet.SelectedObject = kgg.DevObj;
                }
                else
                {
                    AddSwitch_Click(null,null);
                }
                pgSwitchCabinet.Refresh();
                //更新开关柜里的开关状态
                UpdateDBEntity.UpdateSSKGG(entitys);
            }
            catch (Exception ex)
            {
                LogManager.Instance.Error(ex);
            }
            //finally
            //{
            //    //删除完成后清空填写了所属开关柜字段为当前开关柜的设备字段值
            //    var entities = DBManager.Instance.GetEntities<Gg_pd_gnwzmc_n>(o=>o.GNWZ_SSKGG==deleteFid.ToString());
            //    if (entities.Any())
            //    {
            //        foreach (var item in entities)
            //        {
            //            item.GNWZ_SSKGG = null;
            //            if (item.EntityState == EntityState.None)
            //                item.EntityState = EntityState.Update;
            //            DBManager.Instance.Update(item);
            //        }
            //    }
            //}
        }

        private void btBulk_Click(object sender, EventArgs e)
        {

            if (lbSwitchCabinetcol.Items.Count <= 0) return;
            int index = lbSwitchCabinetcol.SelectedIndex;
            if (index < 0) return;
            if (!DBEntityFinder.Instance.VerifyLTTID(dfG3eFid))
            {
                PublicMethod.Instance.Editor.WriteMessage("\n电房未在工单锁定范围之内，禁止编辑！！！\n");
                return;
            }
            try
            {
               
                var values = DevEventHandler.GetDevTables(198, dict[index]);
                GenerateHelper.SetAllBrowsable(values.DevObj,true);
                pgSwitchCabinet.SelectedObject = values.DevObj;
                pgSwitchCabinet.Enabled = true;
                isBulkModify = true;
                isModify = false;
                isAdd = false;
            }
            catch (Exception ex)
            {
                LogManager.Instance.Error(ex);
            }
            finally
            {
                ResetSwitch(false);
            }
        }

        private void btSave_Click(object sender, EventArgs e)
        {
            if (MessageBox.Show("确定要新增或修改吗！！！", "提示", MessageBoxButtons.OKCancel) != DialogResult.OK)
            {
                btSave.Enabled = true;
                return;
            }
            try
            {
                if (isModify)
                {
                    var value = DevEventHandler.GetDevTables(198, g3eFid);
                    UpdateAttribute((XProps)pgSwitchCabinet.SelectedObject, value.ComObj, value.SelfObj, value.GnwzObj,false);
                    //DBSymbolLTTIDFinder.Instance[g3eFid].SBMC = value.ComObj.SBMC;
                    int idx = dict.SingleOrDefault(o => o.Value == g3eFid).Key;
                    lbSwitchCabinetcol.Items[idx] = string.IsNullOrEmpty(value.ComObj.SBMC)?value.ComObj.G3E_FID.ToString():value.ComObj.SBMC;
                    //lbSwitchCabinetcol.SetSelected(devIndex, true);
                }
                if (isBulkModify)
                {
                    foreach (var item in dict)
                    {
                        var value = DevEventHandler.GetDevTables(198, item.Value);
                        UpdateAttribute((XProps)pgSwitchCabinet.SelectedObject, value.ComObj, value.SelfObj, value.GnwzObj, true);
                    }
                }
                if (isAdd)
                {
                    var kggRes = DCadApi.AddAttribute((XProps)pgSwitchCabinet.SelectedObject, 198, dfG3eFid);
                    var prevCur = kggRes;
                    dict.Add(lbSwitchCabinetcol.Items.Count, long.Parse(prevCur.G3eFid));
                    lbSwitchCabinetcol.Items.Add(string.IsNullOrEmpty(prevCur.DeviceSbmc)?prevCur.G3eFid.ToString():prevCur.DeviceSbmc);
                    g3eFid =long.Parse(prevCur.G3eFid);
                    lbSwitchCabinetcol.SetSelected(lbSwitchCabinetcol.Items.Count-1, true);
                }
                isModify = false;
                isAdd = false;
            }
            catch (Exception ex)
            {
            	LogManager.Instance.Error(ex);
            }
            finally
            {
                ResetSwitch(true);
                pgSwitchCabinet.Enabled = false;
            }
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="objDev"></param>
        /// <param name="ObjCom"></param>
        /// <param name="ObjSelf"></param>
        /// <param name="ObjGnwz"></param>
        /// <param name="flag">是否是批量</param>
        public void UpdateAttribute(XProps objDev, Common_n ObjCom, DBEntity ObjSelf, Gg_pd_gnwzmc_n ObjGnwz,bool flag)
        {
            var tableName = string.Empty;
            try
            {
              
                if (ObjCom != null)
                {
                    tableName = ObjCom.GetType().Name;
                    if (flag)
                    {
                        string sbmc=ObjCom.SBMC;
                        GenerateHelper.PartialCopyFromCAD(objDev, ObjCom, 198);
                        ObjCom.SBMC = sbmc;
                    }else
                       GenerateHelper.PartialCopyFromCAD(objDev, ObjCom, 198);
                    if (GenerateHelper.Atff.com != null && ObjCom.CompareProperties(GenerateHelper.Atff.com).Any())
                    {
                        if (ObjCom.EntityState == EntityState.None)
                            ObjCom.EntityState = EntityState.Update;
                        DBManager.Instance.Update(ObjCom);
                    }
                }
                if (ObjSelf != null)
                {
                    tableName = ObjSelf.GetType().Name;
                    GenerateHelper.PartialCopyFromCAD(objDev, ObjSelf, 198);
                    if (GenerateHelper.Atff.self != null && ObjSelf.CompareProperties(GenerateHelper.Atff.self).Any())
                    {
                        if (ObjSelf.EntityState == EntityState.None)
                            ObjSelf.EntityState = EntityState.Update;
                        DBManager.Instance.Update(ObjSelf);
                    }
                }
                if (ObjGnwz != null)
                {
                    tableName = ObjGnwz.GetType().Name;
                    if (flag)
                    {
                        string mc = ObjGnwz.MC;
                        GenerateHelper.PartialCopyFromCAD(objDev, ObjGnwz, 198);
                        ObjGnwz.MC = mc;
                    }
                    else
                    {
                        GenerateHelper.PartialCopyFromCAD(objDev, ObjGnwz, 198);
                        ObjGnwz.SetAttribute("MC", GenerateHelper.GetPropertyValue((XProps)pgSwitchCabinet.SelectedObject,"Common_n", "SBMC"));
                    }
                    if (GenerateHelper.Atff.gnwz != null && ObjGnwz.CompareProperties(GenerateHelper.Atff.gnwz).Any())
                    {
                        if (ObjGnwz.EntityState == EntityState.None)
                            ObjGnwz.EntityState = EntityState.Update;
                        DBManager.Instance.Update(ObjGnwz);
                    }
                }
            }
            catch(UpdataArgumentException ee)
            {
                CommonHelp.Instance.ShowErrorMsg();
                GenerateHelper.SetPropertyValue(objDev,tableName, ee.FieldName, null);
            }
        }

        private void pgSwitchCabinet_PropertyValueChanged(object s, PropertyValueChangedEventArgs e)
        {

            CommonHelp.Instance.ChangeAttributeRunTime((XProps)pgSwitchCabinet.SelectedObject, 198,new List<long>{g3eFid},  e.ChangedItem.PropertyDescriptor.Name,
                e.ChangedItem.Value.ToString(), e.OldValue);
        }

        private void lbSwitchCabinetcol_SelectedIndexChanged(object sender, EventArgs e)
        {
            try
            {
                int index = lbSwitchCabinetcol.SelectedIndex;
                if (index < 0) return;
                var values = DevEventHandler.GetDevTables(198, dict[index]);
                GenerateHelper.SetAllBrowsable(values.DevObj,true);
                GenerateHelper.SetAllReadOnly(values.DevObj,true);
                pgSwitchCabinet.SelectedObject = values.DevObj;
                g3eFid = dict[index];
                devIndex = index;
                if (isAdd || isModify)
                {
                    AddSwitch.Enabled = true;
                    ModifySwitch.Enabled = true;
                    DelSwitch.Enabled = true;
                }
            }
            catch (Exception ex)
            {
                LogManager.Instance.Equals(ex);
            }
            finally
            {
                ResetSwitch(true);
            }
          
        }

        private void SwitchCabinetManage_FormClosing(object sender, FormClosingEventArgs e)
        {
            IsLoadSwitchWnd = false;
        }
        /// <summary>
        /// 开关柜台账信息
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void TZInfo_Click(object sender, EventArgs e)
        {
            try
            {
                pgSwitchCabinet.Enabled = false;
                int index = lbSwitchCabinetcol.SelectedIndex;
                if (index < 0) return;
                var values = DevEventHandler.GetDevTables(198, dict[index]);
                string g3efid = values.Fid.ToString();
                string g3efno = "198";
                var kgg = DBManager.Instance.GetEntity<Gg_pd_kgg_n>(dict[index]);
                if (kgg.EntityState == EntityState.Insert)
                    g3efid = "a" + g3efid;
                string gisid = MapConfig.Instance.LTTID.ToString();
                string sbmc = values.ComObj.SBMC;
                string s = "";
                if (!string.IsNullOrEmpty(sbmc))
                {
                    s = BitConverter.ToString(System.Text.Encoding.UTF8.GetBytes(sbmc)).Replace("-", "<M>");
                }

                string UrlStr = @"http://localhost:9090/emmis/equipGisMappingTemp/getInstallEquipments.gis?g3e_fid=" +
                                g3efid + "&jobId=" + gisid + "&g3e_fno=" + g3efno + "&editAble=Y&funcplaceName=" + s +
                                "&jgdh=009";

                if (string.IsNullOrEmpty(UrlStr))
                {
                    return;
                }
                ViewHelper.LoadTZPalette(UrlStr);
            }catch(Exception ex)
            {
                LogManager.Instance.Error(ex);
            }
        }

        private void AddSwitch_MouseEnter(object sender, EventArgs e)
        {
            AddSwitch.ImageIndex = 1;
            toolTipSwitch.Show("新增", (Button)sender);
        }

        private void AddSwitch_MouseLeave(object sender, EventArgs e)
        {
            AddSwitch.ImageIndex = 0;
            toolTipSwitch.Hide((Button)sender);
        }

        private void ModifySwitch_MouseEnter(object sender, EventArgs e)
        {
            ModifySwitch.ImageIndex = 3;
            toolTipSwitch.Show("修改", (Button)sender);
        }

        private void ModifySwitch_MouseLeave(object sender, EventArgs e)
        {
            ModifySwitch.ImageIndex = 2;
            toolTipSwitch.Hide((Button)sender);
        }

        private void DelSwitch_MouseEnter(object sender, EventArgs e)
        {
            DelSwitch.ImageIndex = 5;
            toolTipSwitch.Show("删除", (Button)sender);
        }

        private void DelSwitch_MouseLeave(object sender, EventArgs e)
        {
            DelSwitch.ImageIndex = 4;
            toolTipSwitch.Hide((Button)sender);
        }

        private void btSave_MouseEnter(object sender, EventArgs e)
        {
            btSave.ImageIndex = 9;
            toolTipSwitch.Show("保存", (Button)sender);
        }

        private void btSave_MouseLeave(object sender, EventArgs e)
        {
            btSave.ImageIndex = 8;
            toolTipSwitch.Hide((Button)sender);
        }

        private void TZInfo_MouseEnter(object sender, EventArgs e)
        {
            TZInfo.ImageIndex = 7;
            toolTipSwitch.Show("台账属性", (Button)sender);
        }

        private void TZInfo_MouseLeave(object sender, EventArgs e)
        {
            TZInfo.ImageIndex = 6;
            toolTipSwitch.Hide((Button)sender);
        }

      
      
    }
}
