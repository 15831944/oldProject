using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using ElectronTransferDal.Cad;
using Autodesk.AutoCAD.DatabaseServices;

namespace ElectronTransferView.FunctionManager
{
    public partial class MoveSymbol : Form
    {
        public MoveSymbol()
        {
            InitializeComponent();
        }
        private ObjectId[] objectIds;
        private void Btn_SelectSymbol_Click(object sender, EventArgs e)
        {
            SelectSymbol();
        }

        private void Btn_SelectPoint_Click(object sender, EventArgs e)
        {
            SelectPoint();
        }

        private void Btn_Move_Click(object sender, EventArgs e)
        {

        }

        private void SelectSymbol()
        {
            objectIds=PublicMethod.Instance.GetObjectIds("选择需要移动的设备\n");
            if (objectIds != null)
                Lal_SymbolInfo.Text = string.Format("选择了{0}个设备", objectIds.Count());
        }
        private void SelectPoint() {
            //选择基点
            var point = PublicMethod.Instance.GetPoint("指定基点\n");
            Txt_Point.Text = string.Format("{0},{1}",point.X,point.Y);
        }

        private void Move() { 
            
        }
    }
}
