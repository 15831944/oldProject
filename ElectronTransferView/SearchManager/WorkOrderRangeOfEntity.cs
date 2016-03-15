using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using Autodesk.AutoCAD.DatabaseServices;
using Autodesk.AutoCAD.Internal;
using ElectronTransferDal;
using ElectronTransferDal.Cad;
using ElectronTransferDal.Common;
using ElectronTransferDal.Query;
using ElectronTransferModel;
using ElectronTransferView.Menu;
using V94=ElectronTransferModel.V9_4;
using ElectronTransferModel.Config;
using ElectronTransferFramework;
using ElectronTransferModel.Base;
using ElectronTransferDal.AutoGeneration;
using DotNetARX;
using ElectronTransferView.ContextMenuManager;

namespace ElectronTransferView.SearchManager
{
    public partial class WorkOrderRangeOfEntity : Form
    {
        [System.Runtime.InteropServices.DllImport("user32", EntryPoint = "SetForegroundWindow")]
        public static extern bool SetForegroundWindow(IntPtr hWnd);
        public WorkOrderRangeOfEntity()
        {
            InitializeComponent();
            DoubleBuffered = true;
            SetForegroundWindow(Handle);
        }
        private ListViewColumnSorter lvwColumnSort = new ListViewColumnSorter();
        private List<WorkOrder> objList = new List<WorkOrder>();
        private void WorkOrderRangeOfEntity_Load(object sender, EventArgs e)
        {
            try
            {

                InitializeListView();
                InitializeData();

            }
            catch (Exception ex)
            {
                LogManager.Instance.Error(ex);
            }
            finally
            {
            }
        }

        private void InitializeData()
        {
            Btn_Analysis.Enabled = true;
            Btn_ClearColor.Enabled = false;
            if (objList.Any()) objList.Clear();
            LoadData();
            if (!objList.Any()) return;
            AddDataToListView();
        }
        /// <summary>
        /// 初始化listview
        /// </summary>
        private void InitializeListView()
        {
            lvFeatureInLtt.Items.Clear();
            lvFeatureInLtt.View = View.Details;
            lvFeatureInLtt.FullRowSelect = true;
            lvFeatureInLtt.GridLines = true;
            lvFeatureInLtt.LabelEdit = false;
            lvFeatureInLtt.HideSelection = false;
            lvFeatureInLtt.AllowColumnReorder = true;
            lvFeatureInLtt.Columns.Add("序号", 45, HorizontalAlignment.Right);
            lvFeatureInLtt.Columns.Add("G3E_FID", 80, HorizontalAlignment.Center);
            lvFeatureInLtt.Columns.Add("设备类型", 120, HorizontalAlignment.Center);
            lvFeatureInLtt.Columns.Add("设备名称", 180, HorizontalAlignment.Center);
            lvFeatureInLtt.Columns.Add("所属馈线", 130, HorizontalAlignment.Center);
            lvFeatureInLtt.Columns.Add("当前状态", 70, HorizontalAlignment.Center);
            lvFeatureInLtt.Visible = true;
            lvFeatureInLtt.AllowColumnReorder = false;
            lvwColumnSort.SortColumn = 3;
            lvFeatureInLtt.ListViewItemSorter = lvwColumnSort;
            ImageList imgList = new ImageList();
            imgList.ImageSize = new System.Drawing.Size(2, 18);
            lvFeatureInLtt.SmallImageList = imgList;
        }

        private void AddDataToListView()
        {
            using (ProgressManager manager = new ProgressManager("填充数据"))
            {
                manager.SetTotalOperations(objList.Count);
                if(lvFeatureInLtt.Items.Count>0)lvFeatureInLtt.Items.Clear();
                for (int i = 0; i < objList.Count; i++)
                {
                    manager.Tick();
                    ListViewItem item = new ListViewItem();
                    item.Tag = objList[i];
                    item.SubItems[0].Text = objList[i].SerialNum.ToString();
                    item.SubItems.Add(objList[i].G3E_FID.ToString());
                    item.SubItems.Add(objList[i].DevType);
                    item.SubItems.Add(objList[i].SBMC);
                    item.SubItems.Add(objList[i].SSXL);
                    item.SubItems.Add(objList[i].DevState);
                    lvFeatureInLtt.Items.Add(item);
                }
                CommonHelp.Instance.ReCreateSerialNumber(lvFeatureInLtt);
            }
        }
        private void LoadData()
        {
            using (ProgressManager manager = new ProgressManager("正在加载设备"))
            {
                //获取可能新增的设备
                var values = DBEntityFinder.Instance.GetDevInLttID();
                if (values.Any())
                {
                    manager.SetTotalOperations(values.Count());
                    int index = 1;
                    foreach (var entity in values)
                    {
                        manager.Tick();
                        long g3e_fid = 0, g3e_id = 0, g3e_fno = 0;
                        DBEntityFinder.Instance.GetG3EIds(entity.Key, ref g3e_id, ref g3e_fid, ref g3e_fno);
                        AddWorkOrderOfDev(entity.Key, g3e_fno, g3e_fid,index);
                        index++;

                    }
                }
            }
        }

        private void AddWorkOrderOfDev(ObjectId objectId, long g3e_fno, long g3e_fid,int index)
        {
            try
            {
                var tables = DevEventHandler.GetDevTables(g3e_fno, g3e_fid);
                if (tables == null) return;
                //更新的情况比较特殊，可能只更新一个表
                string sbmc = string.Empty;
                if (tables.ComObj == null)
                {
                    if (g3e_fno == 159 || g3e_fno == 160) //集中抄表箱//散户表
                    {
                        sbmc = tables.SelfObj.GetAttribute("HH");
                    }
                    else if (g3e_fno == 188) //负控
                    {
                        sbmc = tables.GnwzObj.GetAttribute("MC");
                    }
                }
                else
                {
                    sbmc = tables.ComObj.SBMC;
                }
                var vc = new WorkOrder
                             {
                                 SerialNum=index,
                                 G3E_FID = g3e_fid,
                                 DevType = PublicMethod.Instance.GetDeviceType(Convert.ToInt32(g3e_fno)),
                                 SSXL = tables.ComObj != null ? tables.ComObj.CD_SSXL : string.Empty,
                                 SBMC = sbmc,
                                 DevState = GetDeviceState(tables),
                                 objId = objectId
                             };
                objList.Add(vc);
            }
            catch (Exception ex)
            {
                PublicMethod.Instance.ShowMessage(ex.Message);
                LogManager.Instance.Error(ex);
            }
        }

        //private void dgvWorkOrderOfEntity_RowsAdded(object sender, DataGridViewRowsAddedEventArgs e)
        //{
        //    //for (int i = 0; i < e.RowCount; i++)
        //    //{
        //    //    dgvWorkOrderOfEntity.Rows[e.RowIndex + i].HeaderCell.Style.Alignment =
        //    //        DataGridViewContentAlignment.MiddleCenter;
        //    //    dgvWorkOrderOfEntity.Rows[e.RowIndex + i].HeaderCell.Value = (e.RowIndex + i + 1).ToString();
        //    //}

        //}
        private void WorkOrderRangeOfEntity_FormClosing(object sender, FormClosingEventArgs e)
        {
            FixEntity.Instance.ResetOldEntity();
            if (objectColorList.Any())
            {
                Enabled = false;
                Visible = false;
                ObjectColorTool.Instance.GetObjectSetColor(objectColorList);
                Enabled = true;
                Visible = true;
            }
        }

        private Dictionary<ObjectId, int> objectColorList=new Dictionary<ObjectId,int> ();
        /// <summary>
        /// 查看编辑设备
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void Btn_Analysis_Click(object sender, EventArgs e)
        {
            FixEntity.Instance.ResetOldEntity();
            //获取被编辑的原数据和新增数据
            var values = GetModifyDevice();
            if (values.Any())
            {
                Enabled = false;
                Visible = false;
                objectColorList = ObjectColorTool.Instance.GetObjectSetColor(values);
                Btn_Analysis.Enabled = false;
                Btn_ClearColor.Enabled = true;
                Enabled = true;
                Visible = true;
            }
          
        }
        /// <summary>
        /// 还原颜色
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void Btn_ClearColor_Click(object sender, EventArgs e)
        {
            FixEntity.Instance.ResetOldEntity();
            if (objectColorList.Any())
            {
                Enabled = false;
                Visible = false;
                ObjectColorTool.Instance.GetObjectSetColor(objectColorList);
                Btn_ClearColor.Enabled = false;
                Btn_Analysis.Enabled = true;
                objectColorList.Clear();
                Enabled = true;
                Visible = true;
            }
         
        }

        public string GetState(DBEntity ent)
        {
            string devState = string.Empty;
            if (ent!= null)
            {
                if (ent.EntityState == EntityState.None)
                    devState = "未修改";
                else if (ent.EntityState == EntityState.Insert)
                    devState = "新增";
                else if (ent.EntityState == EntityState.Update)
                    devState = "更新";
                else
                    devState = "已修改";
            }
            return devState;
        }
        public string GetDeviceState(RequiredDevTables rt)
        {
            var comState=GetState(rt.ComObj);
            if(!string.IsNullOrEmpty(comState))
                return comState;
            var selfState=GetState(rt.SelfObj);
            if(!string.IsNullOrEmpty(selfState))
                return selfState;
            var connectState = GetState(rt.ConnectObj);
            if (!string.IsNullOrEmpty(connectState))
                return connectState;
            var gnwzState = GetState(rt.GnwzObj);
            if (!string.IsNullOrEmpty(gnwzState))
                return gnwzState;
            return "未知状态";
        }
        public List<ObjectId> GetModifyDevice()
        {
            List<ObjectId> objIdList = new List<ObjectId>();
            foreach (var item in objList)
            {
                if (!item.DevState.Equals("未修改"))
                    objIdList.Add(item.objId);
            }
            return objIdList;
        }

        private void lvFeatureInLtt_MouseDoubleClick(object sender, MouseEventArgs e)
        {
            if (e.Button == MouseButtons.Left&&lvFeatureInLtt.SelectedItems.Count!=0)
            {
                var  item = lvFeatureInLtt.SelectedItems[0];
                var lvi=item.Tag as WorkOrder;
                if (lvi == null || lvi.G3E_FID <= 0) return;
                FixEntity.Instance.Fix(lvi.G3E_FID);
            }
        }

        private void lvFeatureInLtt_ColumnClick(object sender, ColumnClickEventArgs e)
        {
            CommonHelp.Instance.SortByColumn(e, lvwColumnSort, lvFeatureInLtt);
        }

        private void WorkOrderRangeOfEntity_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyValue == 27)
                this.Close();
        }
    }

    public class WorkOrder
    {
        public int SerialNum { get; set; }
        public long G3E_FID { get; set; }
        public string DevType { get; set; }
        public string SSXL { get; set; }
        public string SBMC { get; set; }
        public string DevState { get; set; }
        public ObjectId objId { get; set; }
    }
}
