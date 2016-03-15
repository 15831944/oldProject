using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using Autodesk.AutoCAD.DatabaseServices;
using ElectronTransferDal;
using ElectronTransferDal.Cad;
using ElectronTransferDal.Query;
using ElectronTransferModel;
using ElectronTransferModel.Base;
using ElectronTransferFramework;
using ElectronTransferModel.Config;
using ElectronTransferView.ViewManager;
using V94 = ElectronTransferModel.V9_4;
using ElectronTransferView.Menu;
using ElectronTransferDal.Common;
using ElectronTransferView.SearchManager;
using ElectronTransferView.ConnectivityManager;
using ElectronTransferDal.AutoGeneration;

namespace ElectronTransferView.ContextMenuManager
{
    public partial class DevAttribute : UserControl
    {
        public static event EventHandler workorderVerify;
        public delegate void PanelControlIsEnable(bool visible,long fid,int fno,ObjectId objId);

        public static event PanelControlIsEnable panelIsEnable;
        
        public DevAttribute()
        {
            InitializeComponent();
        }
        public ObjectId objId { get; set; }
        private RequiredDevTables rdt { get; set; }
        private int Fno { get; set; }
        private long Fid { get; set; }

        /// <summary>
        /// 记录当前绑定对象
        /// </summary>
        private static XProps CurPropertyObj;
  
        public bool AddDevAttribute()
        {
            long fid = 0,fno = 0,id = 0;
            if (!DBEntityFinder.Instance.GetG3EIds(objId, ref id, ref fid, ref fno))
                return false;
            Fno = (int)fno;
            Fid = fid;
            rdt = DevEventHandler.GetDevTables(Fno, Fid);
            if (rdt == null) return false;
            if (!DBEntityFinder.Instance.VerifyLTTID(objId))
            {
                PublicMethod.Instance.ShowMessage("\n当前设备未在工单锁定范围，禁止编辑...");
                GenerateHelper.SetAllReadOnly(rdt.DevObj,true);
            }
            GenerateHelper.SetAllBrowsable(rdt.DevObj,true);
            GenerateHelper.GenerateXPropsByAzwz(rdt.DevObj, Fno,false);
            AutoGenerationHelper.Instance.AddUITypeEditor(rdt.DevObj, Fno);
            pgDev.SelectedObject = rdt.DevObj;
            CurPropertyObj = rdt.DevObj;
            pgDev.Refresh();
            // var simpleSymbolConfigEntry = SimpleSymbolConfig.Instance.Symbols.SingleOrDefault(o => o.Fno == Fno);
            //if (simpleSymbolConfigEntry != null)
            //    WindowManager.ChangeDevAttributePsText(simpleSymbolConfigEntry.LayerName + "(*表示必填项)-" +
            //                                           Fid.ToString(CultureInfo.InvariantCulture));
            //this.Refresh();
            //CommonHelp.Instance.RestoreValue(rdt.GnwzObj);
            return true;
        }

        private void pgDev_PropertyValueChanged(object s, PropertyValueChangedEventArgs e)
        {
            try
            {
                ConnectManager.CleanSelcol();
                CommonHelp.Instance.ChangeAttributeRunTime(CurPropertyObj, Fno,new List<long>{Fid},
                    e.ChangedItem.PropertyDescriptor.Name, e.ChangedItem.Value.ToString(),
                    e.OldValue);
                pgDev.SelectedObject = CurPropertyObj;
                ObjectId tempObjId = objId;
                CommonHelp.Instance.UpdateAttribute(CurPropertyObj, ref tempObjId, rdt);
                objId = tempObjId;
                PublicMethod.Instance.UpdateScreen();
            }
            catch (UpdataArgumentException ee)
            {
                CommonHelp.Instance.ShowErrorMsg();
                if (e.ChangedItem.PropertyDescriptor != null)
                    GenerateHelper.SetPropertyValue(CurPropertyObj,ee.TableName,ee.FieldName, e.OldValue);
            }
            catch (Exception ex)
            {
                LogManager.Instance.Error(ex.Message);
            }
            finally
            {
                EventTriggerFunc();
                DevAttributeVisibleChanged();
                GenerateHelper.EmptyFidRecord();
                pgDev.Refresh();
            }
        }
        private void EventTriggerFunc()
        {
            if (MenuControl.orderVerify != null)
            {
                orderWorkifyArgs owa = new orderWorkifyArgs
                {
                    FID = Fid,FNO=Fno
                };
                if (workorderVerify != null)
                {
                    workorderVerify(new object(), owa);
                }
            }
        }

        private void DevAttributeVisibleChanged()
        {
            if (panelIsEnable != null)
                panelIsEnable(Visible, Fid,Fno, objId);
        }
        private void DevAttribute_VisibleChanged(object sender, EventArgs e)
        {
            DevAttributeVisibleChanged();
        }
       
    }
}
