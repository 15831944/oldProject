using System;
using System.Collections;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Windows.Forms;
using Autodesk.AutoCAD.DatabaseServices;
using Autodesk.AutoCAD.Geometry;
using ElectronTransferBll;
using ElectronTransferDal.Cad;
using ElectronTransferDal.Query;
using Application = Autodesk.AutoCAD.ApplicationServices.Application;
using CADColor = Autodesk.AutoCAD.Colors.Color;
using CADException = Autodesk.AutoCAD.Runtime.Exception;

namespace ElectronTransferView.FunctionManager
{
    public partial class CustomSymbolSet : Form
    {
        public CustomSymbolSet()
        {
            InitializeComponent();
        }
        /// <summary>
        /// 图片路径
        /// </summary>
        private string imgPath;
        public ObjectIdCollection objCollection = new ObjectIdCollection();

        private readonly string savePath = string.Format(@"{0}\自定义模版", DBEntityFinder.Instance.SymbolLibraryPath);
        private Dictionary<ObjectId, CADColor> dic = new Dictionary<ObjectId, CADColor>();

        private void Btn_Pickup_Click(object sender, EventArgs e)
        {
            //选择基点
            var point = PublicMethod.Instance.GetPoint("指定基点\n");
            txt_X.Text = point.X.ToString();
            txt_Y.Text = point.Y.ToString();
            txt_Z.Text = point.Z.ToString();
            lal_basepoint.ForeColor = Color.Black;
            lal_basepoint.Text = "拾取基点";
        }

        public void ClearPoint()
        {
            txt_X.Text = string.Empty;
            txt_Y.Text = string.Empty;
            txt_Z.Text = string.Empty;
        }

        private void Btn_SelectEntity_Click(object sender, EventArgs e)
        {
            try
            {
                var sPoint = new Point3d();
                var ePoint = new Point3d();
                if (string.IsNullOrEmpty(txt_SymbolSetName.Text.Trim()))
                {
                    lal_tip.Text = "请输入模版名称！";
                    txt_SymbolSetName.Focus();
                    return;
                }
                //选中的实体
                var objIds = PublicMethod.Instance.SelectEntities(ref sPoint, ref ePoint);
                
                if (objIds != null)
                {
                    if (objIds.Count() <= 120)
                    {
                        if (ePoint.X == 0 && ePoint.Y == 0) return;
                        //获取图片的保存路径
                        imgPath = GetImgPath();
                        //生成图片
                        SaveImage.WindowScreenShot(sPoint, ePoint, imgPath);
                        //显示图片
                        pictureBox1.Load(imgPath);
                        //高亮显示选择的实体对象
                        HighlightEntity(objIds);
                        //刷新屏幕
                        Application.UpdateScreen();
                        lal_EntityCount.ForeColor = Color.Black;
                        lal_EntityCount.Text = string.Format("已选择{0}个对象", objCollection.Count);
                        txt_SymbolSetName.ReadOnly = true;
                    }
                    else
                    {
                        MessageBox.Show("选择实体过多(不多于120个设备)，请重新选择！");
                    }
                }
            }
            catch (Autodesk.AutoCAD.Runtime.Exception exception)
            {
                PublicMethod.Instance.Editor.WriteMessage(exception.Message);
            }
        }
        /// <summary>
        /// 高亮显示实体
        /// </summary>
        /// <param name="objectIds"></param>
        private void HighlightEntity(IEnumerable<ObjectId> objectIds)
        {
            //先取消高亮
            PublicMethod.Instance.UnHighlightEntities(dic);
            //查找所有设备包括标注
            objCollection = PublicMethod.Instance.GetSymbolOrLabel(objectIds);
            //高亮显示 
            dic = Entityhighlight(objCollection);
        }
        /// <summary>
        /// 获取图片的保存路径
        /// </summary>
        /// <returns></returns>
        private string GetImgPath()
        {
            var picturePath = string.Format(@"{0}\自定义模版图片",
                                            DBEntityFinder.Instance.SymbolPicturePath);
            if (!Directory.Exists(picturePath))
            {
                Directory.CreateDirectory(picturePath);
            }
            //图片路径
            imgPath = string.Format(@"{0}\{1}.jpg",
                                    picturePath,
                                    txt_SymbolSetName.Text.Trim());
            return imgPath;
        }

        /// <summary>
        /// 高亮显示
        /// </summary>
        /// <param name="oc"></param>
        /// <returns></returns>
        private static Dictionary<ObjectId, CADColor> Entityhighlight(IEnumerable oc)
        {
            var dic =new Dictionary<ObjectId, CADColor>();
            foreach (ObjectId objectId in oc)
            {
                var oldColor = DCadApi.EditorPointSymbol(objectId, CADColor.FromRgb(0, 255, 0));
                if (!dic.ContainsKey(objectId))
                    dic.Add(objectId, oldColor);
            }
            return dic;
        }
        private void Btn_CustomSymbol_Click(object sender, EventArgs e)
        {
            try
            {
                SaveSymbolSet(savePath);
            }
            catch (Autodesk.AutoCAD.Runtime.Exception exception)
            {
                PublicMethod.Instance.Editor.WriteMessage(exception.Message);
            }
            finally
            {
                PublicMethod.Instance.UnHighlightEntities(dic);
            }
        }

        /// <summary>
        /// 保存自定义模版
        /// </summary>
        /// <param name="savePath">保存路径</param>
        private void SaveSymbolSet(string savePath)
        {
            var symbolSetName = txt_SymbolSetName.Text.Trim();
            if (string.IsNullOrEmpty(txt_SymbolSetName.Text.Trim()))
            {
                lal_tip.Text = "请输入模版名称！";
                txt_SymbolSetName.Focus();
                return;
            }
            if (!Directory.Exists(savePath))
            {
                Directory.CreateDirectory(savePath);
            }
            if (string.IsNullOrEmpty(txt_X.Text.Trim()))
            {
                lal_basepoint.ForeColor = Color.Red;
                lal_basepoint.Text = "请拾取基点！";
                return;
            }

            if (objCollection.Count>0)
            {
                Btn_CustomSymbol.Enabled = false;
                var objIds = PublicMethod.Instance.ObjectIdCollectionToOjbectIds(objCollection);
                var xmlPath = string.Format(@"{0}\{1}.xml", savePath, symbolSetName);
                var basePoint = new Point3d(double.Parse(txt_X.Text.Trim()), double.Parse(txt_Y.Text.Trim()), 0);
                Lal_SaveTip.Text = "正在创建自定义模版，请稍候…";
                Lal_SaveTip.Refresh();
                //创建自定义符号
                var result=CustomSymbol.CreateCustomsSymbol(xmlPath, objIds, basePoint);
                Lal_SaveTip.Text = string.Empty;
                PublicMethod.Instance.AlertDialog(result?"保存成功！":"保存失败！");
                Btn_CustomSymbol.Enabled = true;
                if (!result)
                {
                    if (File.Exists(imgPath))
                        File.Delete(imgPath);
                }
                Close();
            }
            else
            {
                lal_EntityCount.Text = "请选择对象！";
                lal_EntityCount.ForeColor = Color.Red;
            }
        }


        private void Btn_Cancel_Click(object sender, EventArgs e)
        {
            PublicMethod.Instance.UnHighlightEntities(dic);
            Close();
        }

        private void txt_SymbolSetName_TextChanged(object sender, EventArgs e)
        {
            if (File.Exists(string.Format(@"{0}\{1}.xml", savePath, txt_SymbolSetName.Text.Trim())))
            {
                lal_tip.Text = "该名称在符号库已存在！";
                txt_SymbolSetName.Focus();
                Btn_SelectEntity.Enabled = false;
            }
            else
            {
                Btn_SelectEntity.Enabled = true;
                lal_tip.Text = string.Empty;
            }
        }

        private void Btn_Cancel_MouseEnter(object sender, EventArgs e)
        {
            Btn_Cancel.ImageIndex = 2;
            toolTip1.Show("关闭", (Button) sender);
        }

        private void Btn_Cancel_MouseLeave(object sender, EventArgs e)
        {
            Btn_Cancel.ImageIndex = 3;
            toolTip1.Hide((Button) sender);
        }

        private void Btn_CustomSymbol_MouseEnter(object sender, EventArgs e)
        {
            Btn_CustomSymbol.ImageIndex = 0;
            toolTip1.Show("创建自定义模版", (Button) sender);
        }

        private void Btn_CustomSymbol_MouseLeave(object sender, EventArgs e)
        {
            Btn_CustomSymbol.ImageIndex = 1;
            toolTip1.Hide((Button) sender);
        }
    }
}
