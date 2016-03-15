using System;
using System.Linq;
using System.Windows.Forms;
using ArxMap;
using Autodesk.AutoCAD.DatabaseServices;
using Autodesk.AutoCAD.Geometry;
using ElectronTransferBll.DBEntityHelper;
using ElectronTransferDal.AutoGeneration;
using ElectronTransferDal.Cad;
using ElectronTransferDal.FunctionHelper;
using ElectronTransferDal.Query;
using ElectronTransferFramework;
using ElectronTransferModel.Base;
using ElectronTransferModel.Config;
using ElectronTransferModel.V9_4;
using ElectronTransferView.ViewManager;

namespace ElectronTransferView.ContextMenuManager
{
    public partial class ZXLabelForm : UserControl
    {
        bool isInsert { set; get; }
        
        private FixEntityArgs entityArgs{set;get;}
        private Gg_gl_zxbz_n zxbzn;
        public ZXLabelForm(FixEntityArgs args)
        {
            InitializeComponent();
            entityArgs = args;
            ObjectContextMenu.FixEntityHandler += ObjectContextMenu_FixEntityHandler;
            VerifyLTTID();
        }

        void ObjectContextMenu_FixEntityHandler(object sender, EventArgs e)
        {
            TXT_DYSBFID.Tag = null;
            TXT_DYSBFID.Text = string.Empty;
            Btn_Add.Enabled = true;
            entityArgs = (FixEntityArgs)e;
            TXT_BZNR.Text = string.Empty;
            GetZXBZ();
            VerifyLTTID();
        }
        private void VerifyLTTID()
        {
            Btn_Add.Enabled = DBEntityFinder.Instance.VerifyLTTID(entityArgs.ObjId);
        }

        private void Btn_Add_Click(object sender, EventArgs e)
        {
            try
            {
                var tag = Btn_Add.Tag.ToString();
                switch(tag)
                {
                    case "add":
                        {
                            if (!isInsert)
                                AddZxbz();
                            else
                                MessageBox.Show("命令未结束，请选择杂项标注插入点！", "系统提示", MessageBoxButtons.OK, MessageBoxIcon.Stop);
                        }
                        break;
                    case "update":
                        {
                                UpdateZxbz();
                        }
                        break;
                }
            }
            catch (Exception ex)
            {
                LogManager.Instance.Error("杂项标注错误！\n" + ex);
            }
            finally
            {
                isInsert = false;
            }
        }
        private void AddZxbz()
        {
            var ssdw = ComBox_SSDW.Text;
            var bzlx = ComBox_BZLX.Text;
            var bzText = TXT_BZNR.Text.Trim(); 
            if (string.IsNullOrEmpty(bzlx))
            {
                MessageBox.Show("标注类型不能为空！", "系统提示", MessageBoxButtons.OK, MessageBoxIcon.Information);
                return;
            }
            if (!string.IsNullOrEmpty(bzText))
            {
                //if (!DBEntityFinder.Instance.HasZxbz(entityArgs.g3eObject.G3E_FID.ToString()))
                //{
                isInsert = true;
                Btn_Add.Enabled = false;
                //杂项标注图层
                var layerID = DCadApi.addLayer("杂项标注");
                //标注大小
                var lbHeight = 0.35/MapConfig.Instance.earthscale;
                //标注颜色
                var color = Autodesk.AutoCAD.Colors.Color.FromRgb(0, 0, 0);
                ViewHelper.zxLabelManagerPs.KeepFocus = false;
                Entity mText;
                if (bzText.Contains("\n"))
                {
                    mText = SymbolLabel.Mtext(bzText, Point3d.Origin, 0, lbHeight, layerID);
                    //if (!PublicMethod.Instance.MultTextStyleId.IsNull) 
                    //    ((MText)mText).TextStyleId = PublicMethod.Instance.MultTextStyleId;
                }
                else
                {
                    mText = SymbolLabel.AddText(bzText, Point3d.Origin, 0, lbHeight, layerID, color);
                }
                if (BlockJig.Jig(mText, true))
                {
                    isInsert = false;
                    DCadApi.AddPinchPoint(mText);
                    DCadApi.AddLabelPinchPoint(mText);
                    //添加到当前模型中
                    var objId = PublicMethod.Instance.ToModelSpace(mText);
                    //转换成空间坐标
                    var mpt = ConvertGeometry.Instance.GetMultipoint(objId);
                    ElectronSymbol pt = null;
                    //添加杂标数据
                    var result = InsertDBEntity.InsertZXBZ(entityArgs.g3eObject.G3E_FID, bzText, bzlx, ssdw, mpt, ref pt);
                    if (pt != null)
                    {
                        //保存记录
                        DBSymbolFinder.Instance.Add(objId, pt);
                    }
                    var message = result ? "新增成功！" : "新增失败！";
                    ViewHelper.zxLabelManagerPs.KeepFocus = true;
                    PublicMethod.Instance.ShowMessage(message);
                }
                Btn_Add.Enabled = true;
                //}
                //else
                //    MessageBox.Show("杂项标注已添加！", "系统提示", MessageBoxButtons.OK, MessageBoxIcon.Stop);
            }
            else
            {
                TXT_BZNR.Focus();
                MessageBox.Show("标注内容不能为空！", "系统提示", MessageBoxButtons.OK, MessageBoxIcon.Stop);
            }
        }

        private void UpdateZxbz()
        {
            var ssdw = ComBox_SSDW.Text;
            var bzlx = ComBox_BZLX.Text;
            var bzText = TXT_BZNR.Text.Trim();
            var dysbfid = TXT_DYSBFID.Tag == null ? string.Empty : TXT_DYSBFID.Tag.ToString();
            if (string.IsNullOrEmpty(dysbfid))
            {
                MessageBox.Show("请选择杂项标注对应的父设备！", "系统提示", MessageBoxButtons.OK, MessageBoxIcon.Information);
                return;
            }
            if(string.IsNullOrEmpty(bzlx))
            {
                MessageBox.Show("标注类型不能为空！", "系统提示", MessageBoxButtons.OK, MessageBoxIcon.Information);
                return;
            }
            if (string.IsNullOrEmpty(bzText)) return;
            if (entityArgs.ObjId == ObjectId.Null) return;
            if (!bzText.Equals(zxbzn.MIF_TEXT))
                SymbolLabel.UpdateDBText(entityArgs.ObjId, bzText);
            var result = UpdateDBEntity.UpdateZXBZN(bzText, bzlx, ssdw,dysbfid, zxbzn);
            var message = result ? "修改成功！" : "修改失败！";
            PublicMethod.Instance.ShowMessage(message);
        }

        /// <summary>
        /// 绑定所属单位
        /// </summary>
        private void BindSSDW()
        {
            var gdj=GetCD_SSDW();
            var sskx = DBEntityFinder.Instance.GetSDKXXG();
            var ssdw = sskx.Select(o => o.CD_SSDW).Distinct().ToList();
            ComBox_SSDW.DataSource = ssdw;
            if (!string.IsNullOrEmpty(gdj))
                ComBox_SSDW.Text = gdj;
        }
        /// <summary>
        /// 获取父设备的所属单位
        /// </summary>
        /// <returns></returns>
        private string GetCD_SSDW()
        {
            var ssdw = string.Empty;
            var value = DevEventHandler.GetDevTables(entityArgs.g3eObject.G3E_FNO, entityArgs.g3eObject.G3E_FID);
            if (value != null)
            {
                var ssgdj = GenerateHelper.GetPropertyValue(value.DevObj,"Common_n", "CD_SSDW");
                if (ssgdj != null && !string.IsNullOrEmpty(ssgdj.ToString()))
                    ssdw = ssgdj.ToString();
            }
            return ssdw;
        }
        /// <summary>
        /// 绑定杂标类型
        /// </summary>
        private void BindBZLX()
        {
            var bzlx = DBEntityFinder.Instance.GetBZLX();
            ComBox_BZLX.ValueMember = "NAME";
            ComBox_BZLX.DisplayMember = "NAME";
            ComBox_BZLX.DataSource = bzlx.ToList();
            ComBox_BZLX.Text = "配电设备";
        }

        private void ZXLabelForm_Load(object sender, EventArgs e)
        {
            ViewHelper.zxLabelManagerPs.KeepFocus = true;
            BindSSDW();
            BindBZLX();
            GetZXBZ();
        }
        private void GetZXBZ()
        {
            //选择杂标显示修改
            if (entityArgs.g3eObject.G3E_FNO == 250)
            {
                var g3e_fid = entityArgs.g3eObject.G3E_FID;
                zxbzn = DBEntityFinder.Instance.GetZxbzByG3e_Fid(g3e_fid);
                if (zxbzn != null)
                {
                    Btn_Add.Enabled = true;
                    SetBtnAttribute(BtnTag.UPDATE);
                    TXT_BZNR.Text = zxbzn.MIF_TEXT;
                    ComBox_SSDW.Text = zxbzn.CD_SSDW;
                    ComBox_BZLX.Text = zxbzn.CD_BZLX;
                    //long parentfid;
                    //long.TryParse(zxbzn.BZ_DYSB, out parentfid);
                    //SetZxbzValue(parentfid);
                }
                else
                {
                    Btn_Add.Enabled = false;
                }
            }
            else
            {
                if (entityArgs.ObjId.IsNull) return;
                var entity=PublicMethod.Instance.GetObject(entityArgs.ObjId, OpenMode.ForRead);
                if (entity is BlockReference || entity is Polyline)
                {
                    Btn_Add.Enabled = true;
                    SetBtnAttribute(BtnTag.ADD);
                    SetZxbzValue(entityArgs.g3eObject.G3E_FID);
                }
                else
                {
                    Btn_Add.Enabled = false;
                }
            }
        }

        private void SetBtnAttribute(BtnTag bt)
        {
           switch(bt)
           {
               case BtnTag.ADD:
                   Btn_Add.Tag = "add";
                   Btn_Add.Text = "添 加";
                   break;
               case BtnTag.UPDATE:
                   Btn_Add.Tag = "update";
                   Btn_Add.Text = "修 改";
                   break;
               case BtnTag.DEL:
                   Btn_Add.Tag = "del";
                   Btn_Add.Text = "删 除";
                   break;
           }
        }

        private void Btn_Select_Click(object sender, EventArgs e)
        {
            ViewHelper.zxLabelManagerPs.Visible = false;
            var objectId = PublicMethod.Instance.GetEntity("请选择父设备\n");
            //if (DBEntityFinder.Instance.VerifyLTTID(objectId))
            //{
                Btn_Add.Enabled = true;
                G3EObject g3eObject = null;
                if (DBEntityFinder.Instance.GetG3EIds(objectId, ref g3eObject))
                    SetZxbzValue(g3eObject.G3E_FID);
            //}
            //else
            //{
            //    MessageBox.Show("请选择已锁定的设备！", "系统提示", MessageBoxButtons.OK, MessageBoxIcon.Information);
            //}
            ViewHelper.zxLabelManagerPs.Visible = true;
        }
        private void SetZxbzValue(long g3e_fid)
        {
            var common = DBEntityFinder.Instance.GetCommon_n(g3e_fid);
            if (common != null)
            {
                TXT_DYSBFID.Tag = g3e_fid;
                TXT_DYSBFID.Text =common.SBMC;
            }
        }
    }

    public enum BtnTag
    {
        ADD,
        UPDATE,
        DEL
    }
}
