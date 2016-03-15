using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using Autodesk.AutoCAD.DatabaseServices;
using ElectronTransferDal.Query;
using ElectronTransferView.ConnectivityManager;
using ElectronTransferFramework;
using ElectronTransferDal.Common;
using V94 = ElectronTransferModel.V9_4;
using ElectronTransferModel.Base;
using ElectronTransferDal.Cad;
using Autodesk.AutoCAD.EditorInput;
using ArxMap;
using System.Reflection;
using ElectronTransferModel;
using ElectronTransferView.Menu;
using ElectronTransferDal;
using ElectronTransferDal.AutoGeneration;

namespace ElectronTransferView.ContextMenuManager
{
    public partial class BulkChanges : UserControl
    {
        public static event EventHandler RefBulkModify;
        public BulkChanges()
        {
            InitializeComponent();
            
        }
        /// <summary>
        /// 记录当前绑定对象
        /// </summary>
        private static XProps objDev;
        /// <summary>
        /// 存放批量修改的设备
        /// </summary>
        public List<G3EObject> _cmbItems { get; set; }

        public bool _isRef = false;
        /// <summary>
        /// 记录选择列表的选项
        /// </summary>
        public string _tempItem = "全部";
        /// <summary>
        ///  记录当前修改设备组的FNO，公共属性为0
        /// </summary>
        public static int currentFno { get; set; }
        /// <summary>
        /// 把需要修改的设备梳理
        /// </summary>
        public List<BulkChangeEnt> bulkChangeEnts = new List<BulkChangeEnt>();
        public void RefAttributeData()
        {
            btRef.ImageIndex = 0;
            bulkChangeEnts.Clear();
            foreach (var item in _cmbItems)
            {
                var layer = GenerateHelper.GetLayeraName(item.G3E_FNO);
                bulkChangeEnts.Add(new BulkChangeEnt { LayerName = layer, SingleG3EObject = item });
            }
            LoadFeatureList();
        }

        private void LoadFeatureList()
        {
            CmbBulkChange.Items.Clear();
            GenerateHelper.EmptyFidRecord();
            var items = bulkChangeEnts.Select(o => o.LayerName).Distinct();
            if (items.Count() == 1)
            {
                CmbBulkChange.Items.Add(items.FirstOrDefault() + "  (" + bulkChangeEnts.Count + ")");
                CmbBulkChange.SelectedIndex = 0;
                _tempItem = items.FirstOrDefault();
                RecordCurrentSelectObj();
            }
            else
            {
                CmbBulkChange.Items.Add("全部" + "  (" + bulkChangeEnts.Count + ")");
                var query =
                    from p in bulkChangeEnts
                    group p by p.LayerName
                        into g
                        select new
                        {
                            g.Key,
                            num = g.Count()
                        };
                foreach (var i in query)
                {
                    CmbBulkChange.Items.Add(i.Key + "  (" + i.num + ")");
                }

                CmbBulkChange.Text = CmbBulkChange.Items[0].ToString();
                objDev = GenerateObj.Instance.GenderObjByFno(0);
                GenerateHelper.ResetXProps(ref objDev);
                GenerateHelper.SetAllBrowsable(objDev, true);
                AutoGenerationHelper.Instance.AddUITypeEditor(objDev, currentFno);
                pGridBulkChange.SelectedObject = objDev;
                currentFno = 0;
                _tempItem = "全部";
            }
        }
        /// <summary>
        /// 记录当前选择的对象
        /// </summary>
        private void RecordCurrentSelectObj()
        {
            var ent = bulkChangeEnts.FirstOrDefault(
                o => o.LayerName.Equals(_tempItem));
            currentFno = ent.SingleG3EObject.G3E_FNO;
            objDev = GenerateObj.Instance.GenderObjByFno(currentFno);
            GenerateHelper.SetAllBrowsable(objDev, true);
            GenerateHelper.ResetXProps(ref objDev);
            AutoGenerationHelper.Instance.AddUITypeEditor(objDev, currentFno);
            pGridBulkChange.SelectedObject = objDev;
        }
        private void pGridBulkChange_PropertyValueChanged(object s, PropertyValueChangedEventArgs e)
        {
            ConnectManager.CleanSelcol();
            try
            {
                List<long> fidList = new List<long>();
                if (_tempItem.Equals("全部"))
                {
                    fidList.AddRange(bulkChangeEnts.Select(o => o.SingleG3EObject.G3E_FID));
                }
                else
                {
                    var ents = bulkChangeEnts.Where(o => o.LayerName.Equals(_tempItem));
                    fidList.AddRange(ents.Select(o => o.SingleG3EObject.G3E_FID));
                }
                CommonHelp.Instance.ChangeAttributeRunTime(
                    objDev, currentFno, fidList, e.ChangedItem.PropertyDescriptor.Name,
                    e.ChangedItem.Value.ToString(), e.OldValue);

                pGridBulkChange.SelectedObject = objDev;
                pGridBulkChange.Refresh();
            }
            catch (Autodesk.AutoCAD.Runtime.Exception ex)
            {
                LogManager.Instance.Error(ex);
            }
            finally
            {
            }

        }

        private void CmbBulkChange_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (CmbBulkChange.SelectedIndex < 0)
            {
                CmbBulkChange.Text = CmbBulkChange.Items[0].ToString();
                return;
            }
            GenerateHelper.EmptyFidRecord();
            if (CmbBulkChange.SelectedItem.ToString().Trim().Split(' ')[0].Equals("全部"))
            {
                _tempItem = "全部";
                currentFno = 0;
                objDev = GenerateObj.Instance.GenderObjByFno(0);
                GenerateHelper.SetAllBrowsable(objDev, true);
                GenerateHelper.ResetXProps(ref objDev);
                AutoGenerationHelper.Instance.AddUITypeEditor(objDev, currentFno);
                pGridBulkChange.SelectedObject = objDev;
            }
            else
            {
                _tempItem = CmbBulkChange.SelectedItem.ToString().Trim().Split(' ')[0];
                RecordCurrentSelectObj();
            }
        }
      
        private void btRef_Click(object sender, EventArgs e)
        {
            UpdataResult ur = null;
            try
            {
                //检测有无修改了才更新
                if (!GenerateHelper.IsAllDefault(objDev))
                {
                    _isRef = true;
                }
                if (_isRef)
                {
                    if (MessageBox.Show("确定更新属性吗?", "提示", MessageBoxButtons.OKCancel) == DialogResult.OK)
                    {
                        if (!CommonHelp.Instance.CheckIsNUll(objDev, currentFno))
                        {
                            MessageBox.Show("为确保数据统一建议所属单位、所属变电站、受电馈线同时修改.", "提示", MessageBoxButtons.OK,
                                MessageBoxIcon.Asterisk, MessageBoxDefaultButton.Button1);
                            return;
                        }
                        _isRef = false;
                        //根据更改选项之前的值来更新属性
                        if (_tempItem.Contains("全部"))
                        {
                            ur = CommonHelp.Instance.UpdataCommonAttribute(ref bulkChangeEnts, objDev);
                        }
                        else
                        {
                            //过滤出要更新的设备
                            var ents = bulkChangeEnts.Where(o => o.LayerName.Equals(_tempItem)).ToList();
                            ur = CommonHelp.Instance.UpdataSelfAttribute(ref ents, objDev);
                        }
                    }
                }
            }
            catch (Exception)
            {

            }
            finally
            {
                if (MenuControl.orderVerify != null)
                {
                    List<BulkChangeEnt> temp = new List<BulkChangeEnt>();
                    if (!_tempItem.Equals("全部"))
                    {
                        var ents = bulkChangeEnts.Where(o => o.LayerName.Equals(_tempItem));
                        temp.AddRange(ents);
                    }
                    else
                    {
                        temp.AddRange(bulkChangeEnts);
                    }
                    bulkOrderWorkifyArgs bulkArgs = new bulkOrderWorkifyArgs
                    {
                        bulkChangeList = temp
                    };
                    if (RefBulkModify != null)
                    {
                        RefBulkModify(new object(), bulkArgs);
                    }
                }
                if (ur != null)
                    MessageBox.Show(
                        string.Format("成功修改{0}个设备\n修改失败{1}个设备\n{2}", ur.UpdataSucceedNum, ur.UpdataFailedNum,
                            ur.UpdataFailedNum != 0 ? "未修改成功的设备：\n" + ur.UpdataErrorLog.ToString() : ""), "CAD提示",
                        MessageBoxButtons.OK,
                        MessageBoxIcon.Information);
            }

        }
   
        private void btRef_MouseEnter(object sender, EventArgs e)
        {
            btRef.ImageIndex = 1;
            toolTipBulk.Show("更新", (Button)sender);
        }

        private void btRef_MouseLeave(object sender, EventArgs e)
        {
            btRef.ImageIndex = 0;
            toolTipBulk.Hide((Button)sender);
        }

    }
}
