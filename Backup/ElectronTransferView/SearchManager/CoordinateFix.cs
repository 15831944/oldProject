using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using Autodesk.AutoCAD.ApplicationServices;
using Autodesk.AutoCAD.EditorInput;
using ElectronTransferModel.Config;
using Autodesk.AutoCAD.DatabaseServices;
using Autodesk.AutoCAD.Geometry;
using Autodesk.AutoCAD.GraphicsInterface;
using DotNetARX;
using ElectronTransferView.Menu;
using System.IO;
using System.Reflection;
using ElectronTransferDal.Cad;
using ElectronTransferDal.AutoGeneration;


namespace ElectronTransferView.SearchManager
{
    public partial class CoordinateFix : Form
    {

        private double X = 0;
        private double Y = 0;
        //private ObjectId objId = ObjectId.Null;
        public CoordinateFix()
        {
            InitializeComponent();
            btFixed.ImageIndex = 0;
            texCoord.Focus() ;
        }
        private void btFixed_Click(object sender, EventArgs e)
        {
            if (!CheckValue())
                return;
            FixEntity.Instance.CoordinateFix(X, Y,true);
            
        }
        /// <summary>
        /// 检查输入值
        /// </summary>
        public bool CheckValue()
        {
            Document doc = Autodesk.AutoCAD.ApplicationServices.Application.DocumentManager.MdiActiveDocument;
            Editor ed = doc.Editor;
            if (String.IsNullOrEmpty(texCoord.Text.Trim()))
            {
                MessageBox.Show("经纬度不能为空,请您重新输入经纬度！！！");
                return false;

            }
            if (!CheckInput.IsNatural_Decimal(texCoord.Text.Trim()))
            {
                MessageBox.Show("输入经纬度格式不正确！！！");
                return false;
            }
            if (texCoord.Text.Trim().Contains('，'))
            {
                X = System.Convert.ToDouble(texCoord.Text.Trim().Split('，')[0]);  //经度
                Y = System.Convert.ToDouble(texCoord.Text.Trim().Split('，')[1]);//纬度
                //if (X > MapConfig.Instance.MapMaxX || X < MapConfig.Instance.MapMinX || Y < MapConfig.Instance.MapMinY || Y > MapConfig.Instance.MapMaxY)
                //{
                //    MessageBox.Show("输入的经纬度已超出地图的范围或输入经纬度格式不正确！！！");
                //    return false;
                //}
            }
            else if (texCoord.Text.Trim().Contains(','))
            {
                X = System.Convert.ToDouble(texCoord.Text.Trim().Split(',')[0]);  //经度
                Y = System.Convert.ToDouble(texCoord.Text.Trim().Split(',')[1]);//纬度
                //if (X > MapConfig.Instance.ProjectionMaxX || X < MapConfig.Instance.ProjectionMinX || Y < MapConfig.Instance.ProjectionMinY || Y > MapConfig.Instance.ProjectionMaxY)
                //{
                //    MessageBox.Show("输入的经纬度已超出地图的范围或输入经纬度格式不正确！！！");
                //    return false;
                //}
            }
            //double[] xy = Projection.gauss.Instance.qy_gauss_zs(Y, X);
            //string str = string.Format("{0},{1} ", X, Y);
            //doc.SendStringToExecute("zoom\n" + "c\n" + str + "7.1865222729561714810200100517294e-4" + "\n", true, false, true);
            return true;
        }
        private void CoordinateFix_FormClosing(object sender, FormClosingEventArgs e)
        {
            FixEntity.Instance.RemoveFlag();
            MenuControl.coordinateFix = null;
        }
        public void CloseDeviceWindow()
        {
            this.Close();
        }
        private void btFixed_MouseEnter(object sender, EventArgs e)
        {
            btFixed.ImageIndex = 1;
            toolTipBt.Show("定位", (Button)sender);
        }

        private void btFixed_MouseLeave(object sender, EventArgs e)
        {
            btFixed.ImageIndex = 0;
            toolTipBt.Hide((Button)sender);
        }
    }
}
