using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using ElectronTransferModel.V9_4;
using ElectronTransferDal.Common;
using ElectronTransferFramework;
using ElectronTransferDal.Cad;
using Autodesk.AutoCAD.DatabaseServices;
using Autodesk.AutoCAD.ApplicationServices;
using Autodesk.AutoCAD.EditorInput;
using DotNetARX;
using ElectronTransferDal;
using ElectronTransferView.SearchManager;

namespace ElectronTransferView.SearchManager
{
    public partial class DeviceGridView : Form
    {
        public DeviceGridView()
        {
            InitializeComponent();
        }
        public void DynamicLoadData()
        {
            BindingCollection<Common_n> objList = new BindingCollection<Common_n>();
            foreach (var item in Listcom.ToList<Common_n>())
            {
                objList.Add(item);
            }
            //bindingSource1.DataSource = Listcom.ToList<Common_n>();
            //不需要DataGridView自动添加列
            DevGridView.AutoGenerateColumns = false;
            DevGridView.AllowUserToAddRows = false;
            DevGridView.DataSource = objList;
        }
        public IEnumerable<Common_n> Listcom { get; set; }
        private void DevGridView_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            if (e.RowIndex < 0 || e.ColumnIndex < 0)
            {
                return;
            }
            Document doc = Autodesk.AutoCAD.ApplicationServices.Application.DocumentManager.MdiActiveDocument;
            Editor ed = doc.Editor;
            int index = DevGridView.SelectedRows[0].Index;
            long DevFID = (long)DevGridView.Rows[index].Cells[0].Value;
            if (DevFID <= 0) return;
            //if (Query.ShowDeviceAttributeByFid(DevFID))
            //{
                FixEntity.Instance.Fix(DevFID);
                
            //}
        }

        private void DeviceGridView_Load(object sender, EventArgs e)
        {
            DynamicLoadData();
        }

        private void DeviceGridView_FormClosing(object sender, FormClosingEventArgs e)
        {
            FixEntity.Instance.ResetOldEntity();
            
            
        }

    }
}
