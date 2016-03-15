using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.EnterpriseServices.Internal;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using Autodesk.AutoCAD.Geometry;
using Autodesk.AutoCAD.Runtime;
using Autodesk.AutoCAD.ApplicationServices;
using DotNetARX;
using ElectronTransferDal.AutoGeneration;
using ElectronTransferDal.Query;
using ElectronTransferDal.QueryVerifyHelper;
using ElectronTransferModel;
using ElectronTransferDal.Common;
using System.Collections;
using ElectronTransferModel.Geo;
using ElectronTransferModel.V9_4;
using ElectronTransferFramework;
using ArxMap;
using ElectronTransferView.ContextMenuManager;
using ElectronTransferView.ViewManager;
using Application = Autodesk.AutoCAD.ApplicationServices.Application;
using ElectronTransferDal.Common.Exceptions;
using Autodesk.AutoCAD.DatabaseServices;
using ElectronTransferDal.Cad;
using Autodesk.AutoCAD.EditorInput;
using ElectronTransferView.Menu;
using System.Drawing.Drawing2D;
using FontFamily = System.Drawing.FontFamily;


namespace ElectronTransferView.SearchManager
{
    public partial class Query : Form
    {
        //public String[] dataSource = new String[] { };
        //public static DeviceGridView dgv = null;
        private ListViewColumnSorter lvcs = new ListViewColumnSorter();

        [System.Runtime.InteropServices.DllImport("user32", EntryPoint = "SetForegroundWindow")]
        public static extern bool SetForegroundWindow(IntPtr hWnd);
        public Query()
        {
            InitializeComponent();
            DoubleBuffered = true;
            SetForegroundWindow(Handle);
        }
        private void Query_Load(object sender, EventArgs e)
        {
            InitialzeListView();
            //listBox1.DrawMode = DrawMode.OwnerDrawFixed;
            //listBox1.ItemHeight = txtMohu.Font.Height + 1;
            //listBox1.Height = txtMohu.Height;
            //listBox1.Visible = false;
            //var cc = DBManager.Instance.GetEntities<Common_n>(o => !string.IsNullOrEmpty(o.SBMC) && o.SBMC.Trim().Length > 0);
            //if (cc != null && cc.Any())
            //{
            //    dataSource = cc.OrderBy(o => o.SBMC).Select(o => o.SBMC).ToArray();
            //}
        }
        private void AddColumn(ListView lv)
        {
            lv.Items.Clear();
            lv.View = View.Details;
            lv.FullRowSelect = true;
            lv.GridLines = true;
            lv.LabelEdit = false;
            lv.AllowColumnReorder = false;
            lv.HideSelection = false;
            lvcs.SortColumn = 3;
            lv.ListViewItemSorter = lvcs;
            lv.Columns.Add("序号", 45, HorizontalAlignment.Right);
            lv.Columns.Add("G3E_FID", 80, HorizontalAlignment.Center);
            lv.Columns.Add("设备类型", 100, HorizontalAlignment.Center);
            lv.Columns.Add("设备名称", 250, HorizontalAlignment.Center);
            lv.Columns.Add("受电馈线", 210, HorizontalAlignment.Center);
            lv.Columns.Add("运行状态", 70, HorizontalAlignment.Center);
            lv.Visible = true;
            CommonHelp.Instance.SizeLastColumn(lv);
            var imgList = new ImageList {ImageSize = new System.Drawing.Size(2, 18)};
            lv.SmallImageList = imgList;
        }
        private void AddMapColumn(ListView lv)
        {
            lv.Items.Clear();
            lv.View = View.Details;
            lv.FullRowSelect = true;
            lv.GridLines = true;
            lv.LabelEdit = false;
            lv.AllowColumnReorder = false;
            lv.HideSelection = false;
            lvcs.SortColumn = 3;
            lv.ListViewItemSorter = lvcs;
            lv.Columns.Add("序号", 45, HorizontalAlignment.Right);
            lv.Columns.Add("G3E_FID", 80, HorizontalAlignment.Center);
            lv.Columns.Add("地图要素类型", 110, HorizontalAlignment.Center);
            lv.Columns.Add("地图要素名称", 170, HorizontalAlignment.Center);
            lv.Columns.Add("经度", 110, HorizontalAlignment.Center);
            lv.Columns.Add("纬度", 110, HorizontalAlignment.Center);
            lv.Visible = true;
            CommonHelp.Instance.SizeLastColumn(lv);
            var imgList = new ImageList {ImageSize = new System.Drawing.Size(2, 18)};
            lv.SmallImageList = imgList;
        }
        public void setpanel(int _ii)
        {
            FIDQuery.ImageIndex = 0;
            DeviceNameMoHuQuery.ImageIndex = 0;
            if (_ii == 0)
            {
                QuerytabControl.Controls.RemoveAt(1);
                this.FID_textBox.Focus();
                this.AcceptButton = FIDQuery;
            }
            else if (_ii == 1)
            {
                QuerytabControl.Controls.RemoveAt(0); 
                txtMohu.Focus();
                this.AcceptButton = DeviceNameMoHuQuery;
            }else if (_ii == 2)
            {
                textBox_key.Focus();
                this.AcceptButton = button_search;
            }
        }
        private void InitialzeListView()
        {
            AddColumn(lvFidQuery);
            AddColumn(lvSMQuery);
            AddMapColumn(lvMap);
        }
        private void AddColumnData(List<QueryRes> workorder, string progressName, ListView lv)
        {
            lv.Items.Clear();
            using (ProgressManager manager = new ProgressManager(progressName))
            {
                manager.SetTotalOperations(workorder.Count);
                for (int i = 0; i < workorder.Count; i++)
                {
                    manager.Tick();
                    ListViewItem item = new ListViewItem();
                    item.UseItemStyleForSubItems = false;
                    item.Tag = workorder[i];
                    item.SubItems[0].Text = workorder[i].SerialNum.ToString(CultureInfo.InstalledUICulture);
                    item.SubItems.Add(workorder[i].G3E_FID.ToString(CultureInfo.InvariantCulture));
                    item.SubItems.Add(workorder[i].FeatureType);
                    item.SubItems.Add(workorder[i].Sbmc);
                    item.SubItems.Add(workorder[i].SDKX);
                    item.SubItems.Add(workorder[i].State);
                    lv.Items.Add(item);
                }
            }
        }

        public void AddColumnData(List<MapElementQuery> mapElement, string progressName, ListView lv)
        {
            lv.Items.Clear();
            using (ProgressManager manager = new ProgressManager(progressName))
            {
                manager.SetTotalOperations(mapElement.Count);
                for (int i = 0; i < mapElement.Count; i++)
                {
                    manager.Tick();
                    ListViewItem item = new ListViewItem();
                    item.UseItemStyleForSubItems = false;
                    item.Tag = mapElement[i];
                    item.SubItems[0].Text = mapElement[i].SerialNum.ToString(CultureInfo.InstalledUICulture);
                    item.SubItems.Add(mapElement[i].G3E_FID.ToString(CultureInfo.InvariantCulture));
                    item.SubItems.Add(mapElement[i].CD_YSLX);
                    item.SubItems.Add(mapElement[i].MIF_TEXT);
                    item.SubItems.Add(mapElement[i].SDO_X.ToString(CultureInfo.InvariantCulture));
                    item.SubItems.Add(mapElement[i].SDO_Y.ToString(CultureInfo.InvariantCulture));
                    lv.Items.Add(item);
                }
            }
        }
        /// <summary>
        /// 按FID查找设备属性
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void FIDQuery_Click(object sender, EventArgs e)
        {
            long g3eFid = 0;
            if (!string.IsNullOrEmpty(FID_textBox.Text.Trim()) &&(long.TryParse(FID_textBox.Text.Trim(), NumberStyles.Integer, null, out g3eFid))) ;
            {
                var ent = DBEntityFinder.Instance.GetDBSymbolFinderByFid(g3eFid);
                if (ent == null || !ent.Any())
                {
                    Fidlable.Text = "请您输入有效的FID";
                }
                else
                {
                    Fidlable.Text = "";
                    List<QueryRes> fidList = new List<QueryRes>();
                    var common = DBManager.Instance.GetEntity<Common_n>(g3eFid);
                    if (common != null)
                    {
                        var qr = FillQueryResObj(g3eFid, common.G3E_FNO, common.CD_SSXL, common.CD_SMZQ, common.SBMC);
                        fidList.Add(qr);
                    }
                    else
                    {
                        var cbx = DevEventHandler.GetDevTables(159, g3eFid);
                        if (cbx == null)
                        {
                            var cbxSelf = cbx.SelfObj as Gg_pd_cbx_n;
                            if(cbxSelf!=null)
                                fidList.Add(FillQueryResObj(g3eFid, 159, cbxSelf.CD_SSXL, cbxSelf.CD_SMZQ, cbxSelf.HH));
                        }
                        else
                        {
                            var shb = DevEventHandler.GetDevTables(159, g3eFid);
                            if (shb != null)
                            {
                                var shbSelf = shb.SelfObj as Gg_pd_dyshb_n;
                                if (shbSelf != null)
                                    fidList.Add(FillQueryResObj(g3eFid, 160, shbSelf.CD_SSXL, shbSelf.SMZQ, shbSelf.HH));
                            }
                        }
                    }
                    if (fidList.Count <= 0)
                        Fidlable.Text = "当前沿布图查询不到该FID对应的设备";
                    else
                    {
                        AddColumnData(fidList, "根据FID查询设备", lvFidQuery);
                        FixEntity.Instance.Fix(g3eFid);
                    }
                }
            }
        }

        private QueryRes FillQueryResObj(long g3eFid, int g3eFno, string ssxl, string smzq, string sbmc)
        {
            return new QueryRes
            {
                SerialNum=1,
                G3E_FID = g3eFid,
                FeatureType = PublicMethod.Instance.GetDeviceType(g3eFno),
                SDKX = ssxl,
                State = smzq,
                Sbmc = sbmc
            };
        }
        /// <summary>
        /// 显示设备GIS功能属性表
        /// </summary>
        /// <param name="fid">设备FID</param>
        public static bool ShowDeviceAttributeByFid(long fid)
        {
            
            Common_n device_common = DBManager.Instance.GetEntities<Common_n>(o => o.G3E_FID == fid).FirstOrDefault();
            if (device_common != null)
            {
                if (device_common.EntityState == EntityState.Delete)
                {
                    MessageBox.Show("抱歉，您要查找的设备已被删除咯！！！", "CAD警告", MessageBoxButtons.OK, MessageBoxIcon.Warning, MessageBoxDefaultButton.Button1);
                    return false;
                }
                if (device_common.G3E_FNO == 198)
                {
                    MessageBox.Show("请到开关柜管理界面修改.", "提示", MessageBoxButtons.OK, MessageBoxIcon.Information, MessageBoxDefaultButton.Button1);
                    return false;
                }
                if (AttributeQuery(device_common.G3E_FNO, fid) == RetureValueCheck.LoadDialogSuccess)
                    return true;
            }
            else
            {
                return NoCommonAttributeQuery(fid);
            }
            return false;
        }
        public static bool NoCommonAttributeQuery(long fid)
        {
            //没有公共属性表的特殊设备
            List<int> FnoCollection = new List<int>(new int[] { 159, 160 });
            RetureValueCheck flag = RetureValueCheck.LoadNoFound;
            foreach (var fno in FnoCollection)
            {
                flag = AttributeQuery(fno, fid);
                if (flag == RetureValueCheck.LoadDialogSuccess)
                    return true;
                else if (flag == RetureValueCheck.LoadDialogFalied)
                    return false;
            }
            MessageBox.Show("该FID对应的设备不存在或已被删除!!!.", "CAD提示", MessageBoxButtons.OK, MessageBoxIcon.Information, MessageBoxDefaultButton.Button1);
            return false;

        }
       
        /// <summary>
        /// 属性查询
        /// </summary>
        /// <param name="fno">设备fno</param>
        /// <param name="fid">设备fid</param>
        /// <returns></returns>
        public static RetureValueCheck AttributeQuery(long fno, long fid)
        {
            try
            {
                var value = DevEventHandler.GetDevTables(fno, fid);
                if (value == null) return RetureValueCheck.LoadNoFound;
                //这里表示已经找到了该FID+FNO对应的设备
                if (value.DevObj != null)
                {
                    //如果这四张表有一张被删除了，就表示该设备已经被删除
                    if ((value.ComObj != null && value.ComObj.EntityState == EntityState.Delete)
                        || (value.SelfObj != null && value.SelfObj.EntityState == EntityState.Delete)
                        || (value.ConnectObj != null && value.ConnectObj.EntityState == EntityState.Delete)
                        || (value.GnwzObj != null && value.GnwzObj.EntityState == EntityState.Delete))
                    {
                        MessageBox.Show("抱歉，您要查找的设备已被删除咯！！！", "CAD警告", MessageBoxButtons.OK, MessageBoxIcon.Warning, MessageBoxDefaultButton.Button1);
                        return RetureValueCheck.LoadDialogFalied;
                    }
                    ObjectId objId = DBEntityFinder.Instance.GetObjectIdByFid(fid);
                    DevAttrArgs ee = new DevAttrArgs
                    {
                        devObjId = objId
                    };
                    ViewHelper.AddDevAttribute(ee);
                    return RetureValueCheck.LoadDialogSuccess;
                }
            }
            catch (Autodesk.AutoCAD.Runtime.Exception ex)
            {
                LogManager.Instance.Error(ex.Message);
            }
            return RetureValueCheck.LoadNoFound;
        }
        /// <summary>
        /// 按设备名称查找
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void DeviceNameMoHuQuery_Click(object sender, EventArgs e)
        {
            try
            {
                string text = txtMohu.Text;
                if (String.IsNullOrEmpty(text))
                {
                    DeviceMohuLable.Text = "查询内容不能为空！！！";
                    txtMohu.Focus();
                    return;
                }
                QueryEntities(text.Trim());

            }
            catch (System.Exception ex)
            {
                throw new System.Exception(ex.Message);
            }
        }
        /// <summary>
        /// 按设备名称查询，得到多条记录
        /// </summary>
        /// <param name="sbmc"></param>
        public void QueryEntities(string sbmc)
        {
            var commonlist = DBManager.Instance.GetEntities<Common_n>(o => !string.IsNullOrEmpty(o.SBMC) && o.SBMC.Contains(sbmc)).OrderBy(o=>o.SBMC);
            if (commonlist != null && commonlist.Any())
            {
                DeviceMohuLable.Text = string.Empty;
                List<QueryRes> queryResList = new List<QueryRes>();
                int index = 1;
                foreach (var common in commonlist)
                {
                    var queryRes = new QueryRes
                    {
                        SerialNum = index,
                        G3E_FID = common.G3E_FID,
                        FeatureType = PublicMethod.Instance.GetDeviceType(common.G3E_FNO),
                        SDKX = common.CD_SSXL,
                        State = common.CD_SMZQ,
                        Sbmc = common.SBMC
                    };
                    queryResList.Add(queryRes);
                    index++;
                }
                if (queryResList.Count <= 0)
                    DeviceMohuLable.Text = "当前沿布图查询不到该名称对应的设备";
                else
                {
                    AddColumnData(queryResList, "根据名称查询设备", lvSMQuery);
                }
            }
            else
            {
                DeviceMohuLable.Text ="数据库没有与之匹配的设备！！！";
            }
        }
        private void QuerytabControl_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (QuerytabControl.SelectedTab.Text.Equals("按设备FID查询"))
            {
                var firstOrDefault = QuerytabControl.Controls.Find("FID_textBox", true).FirstOrDefault();
                if (firstOrDefault != null)
                    firstOrDefault.TabIndex = 0;
                FID_textBox.Focus();
                this.AcceptButton = FIDQuery;
                if (!String.IsNullOrEmpty(FID_textBox.Text))
                {
                    FID_textBox.Text = "";
                }
            }
            else if (QuerytabControl.SelectedTab.Text.Equals("按设备名称查询"))
            {
                var firstOrDefault = QuerytabControl.Controls.Find("textBox1", true).FirstOrDefault();
                if (firstOrDefault != null)
                    firstOrDefault.TabIndex = 0;
                txtMohu.Focus();
                this.AcceptButton = DeviceNameMoHuQuery;
                if (!String.IsNullOrEmpty(txtMohu.Text))
                {
                    txtMohu.Text = "";
                }
            }
            else if (QuerytabControl.SelectedTab.Text.Equals("按地图要素查询"))
            {
                textBox_key.Focus();
                this.AcceptButton = button_search;
            }
        }



        private void Query_FormClosing(object sender, FormClosingEventArgs e)
        {
            FixEntity.Instance.ResetOldEntity();
            //FixEntity.Instance.RemoveFlag();
            MenuControl.query = null;
        }

        private void FIDQuery_MouseEnter(object sender, EventArgs e)
        {
            FIDQuery.ImageIndex = 1;
            toolTipQuery.Show("查询", (Button)sender);
        }

        private void FIDQuery_MouseLeave(object sender, EventArgs e)
        {
            FIDQuery.ImageIndex = 0;
            toolTipQuery.Hide((Button)sender);
        }

        private void DeviceNameMoHuQuery_MouseEnter(object sender, EventArgs e)
        {
            DeviceNameMoHuQuery.ImageIndex = 1;
            toolTipQuery.Show("查询", (Button)sender);
        }

        private void DeviceNameMoHuQuery_MouseLeave(object sender, EventArgs e)
        {
            DeviceNameMoHuQuery.ImageIndex = 0;
            toolTipQuery.Hide((Button)sender);
        }

        //private void comboBoxQuery_TextUpdate(object sender, EventArgs e)
        //{
        //    ComboBox comb = (ComboBox)sender;
        //    if (comb.Items.Count > 0) comb.Items.Clear();
        //    var input = comb.Text.ToUpper();
        //    comb.AutoCompleteMode = AutoCompleteMode.None;
        //    if (string.IsNullOrEmpty(input))
        //    {
        //        comb.Items.Add("");
        //        //comb.SelectionStart = comb.Text.Length;
        //        comb.DroppedDown = false;
        //        Cursor = Cursors.Default;
        //    }
        //    else
        //    {
        //        comb.SelectionStart = comb.Text.Length;
        //        var newlist = dataSource.Where(x => x.Contains(input)).ToArray();
        //        comb.Items.AddRange(newlist);
        //        comb.Select(comb.Text.Length, 0);
        //        comb.DroppedDown = true;
                
        //    }
        //    Cursor = Cursors.Default;
        //}

        //private void comboBoxQuery_TextChanged(object sender, EventArgs e)
        //{
        //    //ComboBox comb = (ComboBox) sender;
        //    //comb.AutoCompleteMode = AutoCompleteMode.None;
        //    //comb.SelectionStart = comb.Text.Length;
        //    //Cursor = Cursors.Default;
        //}

        //private void listBox1_DrawItem(object sender, DrawItemEventArgs e)
        //{
        //    listBox1.DrawMode = DrawMode.OwnerDrawFixed;
        //    e.DrawBackground();
        //    Brush myBrush = Brushes.Black;
        //    FontFamily fontFamily = new FontFamily("宋体");
        //    Font myFont = new Font(fontFamily, txtMohu.Font.Size);
        //    StringFormat sf = new StringFormat();
        //    sf.FormatFlags = StringFormatFlags.NoWrap;

        //    if ((e.State & DrawItemState.Selected) == DrawItemState.Selected)
        //    {
        //        if (e.Index > -1)
        //        {
        //            e.Graphics.DrawString(listBox1.Items[e.Index].ToString(), myFont, Brushes.White, e.Bounds, sf);
        //        }
        //    }
        //    else
        //    {
        //        if (e.Index > -1)
        //        {
        //            e.Graphics.DrawString(listBox1.Items[e.Index].ToString(), myFont, myBrush, e.Bounds, sf);

        //        }
        //    }
        //    e.DrawFocusRectangle();
        //}

        //private void BindingList(string[] newlist)
        //{
        //    listBox1.Items.Clear();
        //    listBox1.Items.AddRange(newlist);
        //    if (newlist.Count() < 11)
        //    {
        //        listBox1.Height = txtMohu.Font.Height*newlist.Count() + 30;
        //    }
        //    else
        //    {
        //        listBox1.Height = 155;
        //    }
        //    listBox1.Visible = true;
        //}

        //private void textBox1_TextChanged(object sender, EventArgs e)
        //{
        //    var tb = (TextBox) sender;
        //    if (!string.IsNullOrEmpty(tb.Text))
        //    {
        //        var newlist = dataSource.Where(x => x.Contains(txtMohu.Text.Trim())).ToArray();
        //        if (!newlist.Any())
        //        {
        //            listBox1.Visible = false;
        //        }
        //        else
        //        {
        //            BindingList(newlist);
        //        }
        //    }
        //    else
        //    {
        //        listBox1.Items.Clear();
        //        listBox1.Visible = false;
        //    }
        //    DeviceMohuLable.Text = string.Empty;
        //}

        //private void listBox1_SelectedIndexChanged(object sender, EventArgs e)
        //{
        //    if (listBox1.SelectedIndex > -1)
        //    {
        //        txtMohu.Text = listBox1.SelectedItem.ToString();
        //        listBox1.Visible = false;
        //    }
        //}

        private void FID_textBox_TextChanged(object sender, EventArgs e)
        {
            Fidlable.Text = string.Empty;
        }
        //关键字查询
        private void button_search_Click(object sender, EventArgs e)
        {
            try
            {
                string KeyString = textBox_key.Text;
                if (String.IsNullOrEmpty(KeyString))
                {
                    lbKeyWord.Text="关键字不能为空";
                    textBox_key.Focus();
                    return;
                }
                SearchEntities(KeyString);

            }
            catch (System.Exception ex)
            {
                throw new System.Exception(ex.Message);
            }
        }
        public void SearchEntities(string strkey)
        {
            var mapList = DTDBManager.Instance.GetEntities<Gg_dt_wzbz_lb_sdogeom_cad>(o => !string.IsNullOrEmpty(o.MIF_TEXT) && o.MIF_TEXT.Contains(strkey)).OrderBy(o => o.MIF_TEXT);
            if (mapList != null && mapList.Any())
            {
                textBox_key.Text= string.Empty;
                lbKeyWord.Text = string.Empty;
                List<MapElementQuery> mapelementList = new List<MapElementQuery>();
                int index = 1;
                foreach (var mp in mapList)
                {
                    var multipoint = mp.G3E_GEOMETRY as Multipoint;
                    var x = multipoint.Points[0].X;
                    var y = multipoint.Points[0].Y;
                    var queryRes = new MapElementQuery
                    {
                        SerialNum=index,
                        G3E_FID=mp.G3E_FID,
                        CD_YSLX=mp.CD_YSLX,
                        MIF_TEXT=mp.MIF_TEXT,
                        SDO_X=x,
                        SDO_Y=y
                    };
                    mapelementList.Add(queryRes);
                    index++;
                }
                if (mapelementList.Count <= 0)
                    lbKeyWord.Text = "当前沿布图查询不到与之匹配的地图要素";
                else
                {
                    AddColumnData(mapelementList, "查询与之匹配的地图要素", lvMap);
                }
            }
            else
            {
                lbKeyWord.Text="数据库中没有要查询的设备！！";
            }
        }

        private void lvFidQuery_MouseDoubleClick(object sender, MouseEventArgs e)
        {
            if (e.Button == MouseButtons.Left && lvFidQuery.SelectedItems.Count != 0)
            {
                var item = lvFidQuery.SelectedItems[0];
                string fid = item.SubItems[1].Text;
                long g3eFid = 0;
                if (long.TryParse(fid, out g3eFid))
                {
                    FixEntity.Instance.Fix(g3eFid);
                }
            }
        }

        private void lvSMQuery_MouseDoubleClick(object sender, MouseEventArgs e)
        {
            if (e.Button == MouseButtons.Left && lvSMQuery.SelectedItems.Count != 0)
            {
                var item = lvSMQuery.SelectedItems[0];
                string fid = item.SubItems[1].Text;
                long g3eFid = 0;
                if (long.TryParse(fid, out g3eFid))
                {
                    FixEntity.Instance.Fix(g3eFid);
                }
            }
        }

        private void lvMap_MouseDoubleClick(object sender, MouseEventArgs e)
        {
            if (e.Button == MouseButtons.Left && lvMap.SelectedItems.Count != 0)
            {
                var item = lvMap.SelectedItems[0];
                //string fid = item.SubItems[0].Text;
                string sdox = item.SubItems[4].Text;
                string sdoy = item.SubItems[5].Text;
                //long g3eFid = 0;
                double sdo_x = 0;
                double sdo_y = 0;
                if (double.TryParse(sdox, out sdo_x) && double.TryParse(sdoy, out sdo_y))
                {
                    //FixEntity.Instance.RemoveFlag();
                    FixEntity.Instance.CoordinateFix(sdo_x, sdo_y,false);
                }
            }
        }

        private void lvSMQuery_ColumnClick(object sender, ColumnClickEventArgs e)
        {
            CommonHelp.Instance.SortByColumn(e, lvcs, lvSMQuery);
        }

        private void lvMap_ColumnClick(object sender, ColumnClickEventArgs e)
        {
            CommonHelp.Instance.SortByColumn(e, lvcs, lvMap);
        }

        private void Query_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyValue == 27)
                this.Close();
        }

        private void Query_Resize(object sender, EventArgs e)
        {
            CommonHelp.Instance.SizeLastColumn(lvFidQuery);
            CommonHelp.Instance.SizeLastColumn(lvSMQuery);
            CommonHelp.Instance.SizeLastColumn(lvMap);
        }

    }
    public enum RetureValueCheck
    {
        LoadDialogFalied,
        LoadDialogSuccess,
        LoadNoFound
    }

   
}
