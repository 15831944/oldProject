using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using ElectronTransferDal.Query;
using ElectronTransferView.ContextMenuManager;
using ElectronTransferView.Menu;
using Autodesk.AutoCAD.DatabaseServices;
using ElectronTransferDal.Cad;
using ElectronTransferModel.V9_4;
using ElectronTransferDal.Common;
using Autodesk.AutoCAD.EditorInput;
using Autodesk.AutoCAD.ApplicationServices;
using System.Windows.Forms;
using ElectronTransferView.ViewManager;
using Application = Autodesk.AutoCAD.ApplicationServices.Application;
using ElectronTransferFramework;
using GeneratorHelper=ElectronTransferDal.AutoGeneration.GenerateHelper;

namespace ElectronTransferView.SearchManager
{
    public  partial class SpecialDeviceList : Form
    {
        public SpecialDeviceList(int fno,string str)
        {
            InitializeComponent();
            Width = 436;
            Height = 220;
            curKx = str;
            DynamicLoadList(fno);
            devCount = dgvDevCol.GetCellCount(DataGridViewElementStates.Visible)/2;
        }
        public SpecialDeviceList(int[] fnocol,string str)
        {
            InitializeComponent();
            Width = 436;
            Height = 220;
            curKx = str;
            DynamicLoadList(fnocol);
            devCount = dgvDevCol.GetCellCount(DataGridViewElementStates.Visible) / 2;
        }
        /// <summary>
        /// 记录当前馈线名称
        /// </summary>
        private string curKx { get; set; }
        /// <summary>
        /// 记录查到的设备总数
        /// </summary>
        public static int devCount { get; set; }
     
        public void DynamicLoadList(int fno)
        {
            Dictionary<long,string> values = new Dictionary<long,string>();
            if (fno == 198||fno==199||fno==148)
            {
                values = DBEntityFinder.Instance.GetSymbolByFno(fno);
                
            }else if (fno == 141)
            {
                values = DBEntityFinder.Instance.GetSymbolByFno(fno);
            }
            else
            {
                values = DBEntityFinder.Instance.GetSymbolSBMCByFno(fno);
            }
            Dictionary<long, string> selectRes = new Dictionary<long, string>();
            foreach (var item in values)
            {
                var common = DBManager.Instance.GetEntity<Common_n>(item.Key);
                if (common != null)
                {
                    if (!string.IsNullOrEmpty(common.CD_SSXL))
                    {
                        if (common.CD_SSXL.Equals(curKx))
                        {
                            selectRes.Add(item.Key, item.Value);
                        }
                    }
                }
            }
            //values = values.Where(o =>
            //    DBManager.Instance.GetEntity<Common_n>(o.Key).CD_SSXL.Equals(curKx)).ToDictionary(o => o.Key, o => o.Value);
            var col = from p in selectRes
                      group p by p.Key into g
                      select new SpecialList
                      {
                          fid = g.Key,
                          sbmc = selectRes[g.Key]
                      };
            var dataSource = col.ToList();
            if (CommonHelp.Instance.FromMouseFixEntityFno == 146&&fno==201)
            {
                var ownshipSource = DBManager.Instance.GetEntity<Common_n>(CommonHelp.Instance.FromMouseFixEnitiyFid);
                if (ownshipSource != null)
                {
                    var csy = DBManager.Instance.GetEntities<Common_n>(o => o.G3E_ID == ownshipSource.OWNER1_ID);
                    if (csy != null && csy.Any())
                    {
                        foreach (var item in csy)
                        {
                            var res = dataSource.Where(o => item != null && o.fid == item.G3E_FID);
                            if (res == null || !res.Any())
                            {
                                dataSource.Add(new SpecialList
                                {
                                    fid = item.G3E_FID,
                                    sbmc = item.SBMC
                                });
                            }

                        }
                    }
                    else
                        MessageBox.Show("缺少从属关系可能会杆塔显示不全", "CAD消息", MessageBoxButtons.OK);
                }
            }
            dataSource.Add(new SpecialList{fid=null,sbmc=null});
            dgvDevCol.DataSource = dataSource;
            Refresh();
        }
        public void DynamicLoadList(int[] fnoCol)
        {
            IEnumerable<SpecialList> col = new List<SpecialList>();
            if (fnoCol.Contains(141) || fnoCol.Contains(140))
            {
                var commons =
                    DBManager.Instance.GetEntities<Common_n>(
                        o =>
                            (o.G3E_FNO == 141 || o.G3E_FNO == 140) && !string.IsNullOrEmpty(o.CD_SSXL) &&
                            o.CD_SSXL.Equals(curKx));
                col = from p in commons
                    select new SpecialList
                    {
                        fid = p.G3E_FID,
                        sbmc = p.SBMC
                    };
            }
            else
            {
                Dictionary<long, string> values = new Dictionary<long, string>();
                foreach (int item in fnoCol)
                {
                    Dictionary<long, string> temp = DBEntityFinder.Instance.GetSymbolSBMCByFno(item);
                    values = values.Union(temp).ToDictionary(o => o.Key, o => o.Value);
                }
                col = from p in values
                          group p by p.Key into g
                          select new SpecialList
                          {
                              fid = g.Key,
                              sbmc = values[g.Key]
                          };
            }
            var datasource = col.ToList();
            datasource.Add(new SpecialList{fid = null,sbmc=null});
            dgvDevCol.DataSource = datasource;
        }
        public static void Fix(long DevFID)
        {
            try
            {
                
                Document doc = Application.DocumentManager.MdiActiveDocument;
                ObjectId objId = DBEntityFinder.Instance.GetObjectIdByFid(DevFID);
                if (!objId.IsNull)
                {
                    FixEntity.Instance.ClickFixEntity(objId);
                    using (doc.LockDocument())
                    {
                        for (int i = 0; i < 3; i++)
                        {
                            using (Transaction transaction = doc.TransactionManager.StartTransaction())
                            {
                                DCadApi.isModifySymbol = true;
                                var ent = transaction.GetObject(objId, OpenMode.ForRead) as Entity;
                                ent.UpgradeOpenAndRun();
                                ent.Color = Autodesk.AutoCAD.Colors.Color.FromRgb(0, 255, 0);
                                transaction.Commit();
                                Application.UpdateScreen();
                                DCadApi.isModifySymbol = false;
                            }
                            System.Threading.Thread.Sleep(50);
                            using (Transaction transaction = doc.TransactionManager.StartTransaction())
                            {
                                DCadApi.isModifySymbol = true;
                                var ent = transaction.GetObject(objId, OpenMode.ForRead) as Entity;
                                ent.UpgradeOpenAndRun();
                                ent.Color = Autodesk.AutoCAD.Colors.Color.FromRgb(255, 0, 0);
                                transaction.Commit();
                                Application.UpdateScreen();
                                DCadApi.isModifySymbol = false;
                            }
                             System.Threading.Thread.Sleep(50);
                        }
                    }


                }
                else
                {
                    //PublicMethod.ShowMessage("温馨提示：当前设备在沿布图上没有对应的符号！！");
                }
            }
            catch (SystemException ex)
            {
                LogManager.Instance.Error(ex);
            }
            finally
            {
                DCadApi.isModifySymbol = false;
            }

        }
        private void dgvDevCol_CellMouseClick(object sender, DataGridViewCellMouseEventArgs e)
        {
            if (e.ColumnIndex<0||e.RowIndex<0)
            {
                return;
            }
            if (dgvDevCol.GetCellCount(DataGridViewElementStates.Visible) > 0)
            {
                int ind = dgvDevCol.SelectedCells.Count;
                var DevFID = dgvDevCol.SelectedCells[0].Value;
                var sbmc = dgvDevCol.SelectedCells[1].Value;
                GeneratorHelper.choiceDevice.G3eFid =DevFID!=null?DevFID.ToString():"0";
                GeneratorHelper.choiceDevice.DeviceSbmc =sbmc!=null?sbmc.ToString():"";

                //SurfaceInteractive.ChoiceFid = 0;
                //SurfaceInteractive.ChoiceFid = DevFID;
                //if (DevFID <= 0) return;
                //else
                //    Fix(DevFID);
            }
            else
            {
                MessageBox.Show("当前数据源中没有该设备列表！！！", "提示", MessageBoxButtons.OK, MessageBoxIcon.Information, MessageBoxDefaultButton.Button1);
            }
            this.Close();
        }
      
    }
    public class SpecialList
    {
        public long? fid { get; set; }
        public String sbmc { get; set; }
    }
}
