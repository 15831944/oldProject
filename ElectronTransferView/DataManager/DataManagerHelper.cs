using System;
using System.Windows.Forms;
using ArxMap;
using ElectronTransferDal.Cad;
using ElectronTransferDal.Common;
using ElectronTransferDal.FunctionHelper;
using ElectronTransferDal.Query;
using ElectronTransferFramework;
using ElectronTransferModel.V9_4;
using ElectronTransferView.ContextMenuManager;
using ElectronTransferView.FunctionManager;
using ElectronTransferView.ViewManager;
using Application = Autodesk.AutoCAD.ApplicationServices.Application;
namespace ElectronTransferView.DataManager
{
    public class DataManagerHelper
    {
        public void AddContextEvent()
        {
            ObjectContextMenu.EntityAttributeHandler += mouse_EntityAttributeHandler;
            ObjectContextMenu.EntityConnectHandler += mouse_EntityConnectHandler;
            ObjectContextMenu.EntitySelectionHandler += mouse_EntitySelectionHandler;
            ObjectContextMenu.TZWebHandler += muse_TZWebHandler;
            ObjectContextMenu.KGGManager += mouse_KGGManager;
            ObjectContextMenu.SHBManager += mouse_SHBManager;
            ObjectContextMenu.JLBManager += mouse_JLBManager;
            ObjectContextMenu.AddLabelHandler += mouse_AddLabelHandler;
            DBEntityErased.Instance.BatchDeleteFromJCXEvent += SHBDeleteManager.BatchDeleteFromJCX;
            DBEntityErased.Instance.BatchDeleteFromJLBEvent += JLBMap.BatchDeleteFromJLB;
            ObjectContextMenu.ZXBZHandler += ObjectContextMenu_ZXBZHandler;
        }


        public void RemoveContextEvent()
        {
            ObjectContextMenu.EntityAttributeHandler -= mouse_EntityAttributeHandler;
            ObjectContextMenu.EntityConnectHandler -= mouse_EntityConnectHandler;
            ObjectContextMenu.EntitySelectionHandler -= mouse_EntitySelectionHandler;
            ObjectContextMenu.TZWebHandler -= muse_TZWebHandler;
            ObjectContextMenu.KGGManager -= mouse_KGGManager;
            ObjectContextMenu.SHBManager -= mouse_SHBManager;
            ObjectContextMenu.JLBManager -= mouse_JLBManager;
            ObjectContextMenu.AddLabelHandler -= mouse_AddLabelHandler;
            DBEntityErased.Instance.BatchDeleteFromJCXEvent -= SHBDeleteManager.BatchDeleteFromJCX;
            DBEntityErased.Instance.BatchDeleteFromJLBEvent -= JLBMap.BatchDeleteFromJLB;
            ObjectContextMenu.ZXBZHandler -= ObjectContextMenu_ZXBZHandler;
        }

        /// <summary>
        /// 杂项标注
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        void ObjectContextMenu_ZXBZHandler(object sender, EventArgs e)
        {
            var args = (FixEntityArgs)e;
            if (DBEntityFinder.Instance.VerifyLTTID(args.ObjId))
            {
                ViewHelper.LoadZXLabelManager(args);
            }
            else
            {
                PublicMethod.Instance.AlertDialog("当前选择设备未被锁定，不能添加杂项标注");
            }
        }


        public static void mouse_KGGManager(object sender, EventArgs e)
        {
            var args = (selectEntityArgs)e;
            var com = DBManager.Instance.GetEntity<Common_n>(args.g3eObject.G3E_FID);
            //if (args.g3eObject.G3E_FNO == 149)
            //{
            //    var gnzw = DBManager.Instance.GetEntity<Gg_pd_gnwzmc_n>(args.g3eObject.G3E_FID);
            //    if (gnzw.GNWZ_FL.Contains("美式"))
            //    {
            //        PublicMethod.Instance.AlertDialog("美式箱变无开关柜");
            //        return;
            //    }
            //}
            if (com != null)
            {
                if (!SwitchCabinetManage.IsLoadSwitchWnd)
                {
                    var scm = new SwitchCabinetManage(com.G3E_ID, com.G3E_FID);
                    Application.ShowModelessDialog(scm);
                }
            }
        }
        /// <summary>
        /// 台账调用窗口
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        public static void muse_TZWebHandler(object sender, EventArgs e)
        {
            try
            {
                var tz = e as TZArgs;
                ViewHelper.LoadTZPalette(tz.urlPath);
            }
            catch (Exception ex)
            {
                LogManager.Instance.Error(ex);
            }
        }
        /// <summary>
        /// 批量修改
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        public static void mouse_EntitySelectionHandler(object sender, EventArgs e)
        {
            var ee = (BlukSelectArgs)e;
            ViewHelper.AddBulkChangesAttribute(ee);
        }

        /// <summary>
        /// 显示拓展关系对话框
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        public static void mouse_EntityConnectHandler(object sender, EventArgs e)
        {
            var ee = (selectEntityArgs)e;

            ViewHelper.AddOrUpdateConnectPanel(ee);
        }
        /// <summary>
        /// 显示设备属性对话框
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        public static void mouse_EntityAttributeHandler(object sender, EventArgs e)
        {
            var ee = (DevAttrArgs)e;
            ViewHelper.AddDevAttribute(ee);
        }

        /// <summary>
        /// 户表管理
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        public static void mouse_SHBManager(object sender, EventArgs e)
        {
            var ee = (selectEntityArgs)e;
            var shb = new SHBMap(ee);
            Application.ShowModelessDialog(shb);
        }
        /// <summary>
        /// 计量表管理
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        public static void mouse_JLBManager(object sender, EventArgs e)
        {
            var ee = (selectEntityArgs)e;
            var jlb = new JLBMap(ee);
            Application.ShowModelessDialog(jlb);
            jlb.WindowState = FormWindowState.Normal;
            jlb.Visible = true;
        }

        /// <summary>
        /// 添加标注
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        void mouse_AddLabelHandler(object sender, EventArgs e)
        {
            var args = (FixEntityArgs)e;
            if (DBEntityFinder.Instance.VerifyLTTID(args.ObjId))
            {
                ViewHelper.LoadLabelManager(args);
            }
            else
            {
                PublicMethod.Instance.AlertDialog("当前选择设备未被锁定，不能添加标注");
            }
        }
    }
}
