using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using Autodesk.AutoCAD.Geometry;
using ElectronTransferDal.Cad;
using ElectronTransferDal.Common;
using ElectronTransferDal.Common.Exceptions;
using ElectronTransferDal.Query;
using ElectronTransferFramework;
using ElectronTransferModel;
using ElectronTransferModel.Base;
using ElectronTransferModel.Geo;
using ElectronTransferModel.V9_4;
using ElectronTransferView.SearchManager;
using Point = ElectronTransferModel.Geo.Point;
using AcApi = Autodesk.AutoCAD.ApplicationServices.Application;

namespace ElectronTransferView.ContextMenuManager
{
    public partial class SHBMove : Form
    {
        /// <summary>
        /// 当前集抄箱的g3e_deatilID
        /// </summary>
        long g3e_detailId { get; set; }
        /// <summary>
        /// 当前迁移坐标
        /// </summary>
        double[] move_pt { get; set; }
        /// <summary>
        /// 户表的G3E_FID
        /// </summary>
        public long g3e_Fid { get; set ; }
        /// <summary>
        /// 户表门牌地址(HH)
        /// </summary>
        public string AZDZ { get; set; }

        public SHBMove()
        {
            InitializeComponent();
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="detailid">详表ID</param>
        /// <param name="mpt">迁移坐标</param>
        public SHBMove(long detailid, double [] mpt)
        {
            move_pt = mpt;
            g3e_detailId = detailid;
            InitializeComponent();
        }

        private void btn_SHBmove_Click(object sender, EventArgs e)
        {
            try
            {
                // 提示文字颜色
                lb_SHBWarn.Text = "";
                lb_SHBWarn.ForeColor = Color.Red;
                // 输入文本框是否为空
                if (string.IsNullOrEmpty(tx_SHBYDH.Text))
                {
                    lb_SHBWarn.Text = "客户协议号不能为空.";
                    return;
                }
                // 迁移户表的自身属性表
                var temp_shbn =
                    DBManager.Instance.GetEntities<Gg_pd_dyshb_n>(o => o.KHXYH == tx_SHBYDH.Text.Trim()).FirstOrDefault();
                if (temp_shbn == null)
                {
                    lb_SHBWarn.Text = "客户协议号不存在.";
                    return;
                }
                // 迁移户表的安装地址
                AZDZ = temp_shbn.AZDZ;
                // 迁移户表的g3e_fid
                g3e_Fid = temp_shbn.G3E_FID;
                // 得到迁移户表的坐标表
                var temp_shbpt = DBManager.Instance.GetEntities<Gg_jx_shbd_pt>( o => o.G3E_FID == temp_shbn.G3E_FID && o.EntityState != EntityState.Delete).FirstOrDefault();
                if (temp_shbpt == null)
                {
                    lb_SHBWarn.Text = "该客户协议号的户表坐标表不存在.";
                    return;
                }
                // 判断是否在当前集抄箱
                if (temp_shbpt.G3E_DETAILID == g3e_detailId)
                {
                    lb_SHBWarn.Text = "该客户协议号的户表已在此集抄箱中.";
                    return;
                }
                // 取出需要被迁移户表的详表数据 (来源集抄箱)
                var temp_detail = DBManager.Instance.GetEntities<Detailreference_n>( o => o.G3E_DETAILID == temp_shbpt.G3E_DETAILID && o.EntityState != EntityState.Delete).FirstOrDefault();
                if (temp_detail == null)
                {
                    lb_SHBWarn.Text = "该客户协议号的户表详表数据不存在.";
                    return;
                }
                // 判断需要被迁移的户表的集抄箱是否被锁定
                var isLock = DBEntityFinder.Instance.VerifyLTTID(temp_detail.G3E_FID);
                if (!isLock)
                {
                    lb_SHBWarn.Text = "户表所属抄表箱没有被工单锁定,不能迁移!";
                    return;
                } 
                // 目标集抄箱
                temp_detail =  DBManager.Instance.GetEntities<Detailreference_n>( o => o.G3E_DETAILID == g3e_detailId && o.EntityState != EntityState.Delete).FirstOrDefault();
                var str = "表号: " + temp_shbn.BH + "\n"
                          + "用户姓名: " + temp_shbn.YHXM + "\n"
                          + "安装地址: " + temp_shbn.HH + "\n"
                          + "确定要迁移?";
                // 搜索成功,弹出提示框
                var res = MessageBox.Show(str, @"确定?", MessageBoxButtons.OKCancel);
                if (res == DialogResult.Cancel) return;
                // 迁移户表操作
                // 1 更改户表的detailID
                temp_shbpt.G3E_DETAILID = g3e_detailId;
                if (temp_shbpt.EntityState != EntityState.Insert)
                    temp_shbpt.EntityState = EntityState.Update;
                // 2 得到户表的FID
                // 3 保持与当前集抄箱的所属线路、单位一致
                var temp_detailn = DBManager.Instance.GetEntities<Gg_pd_cbx_n>( o => o.G3E_FID == temp_detail.G3E_FID && o.EntityState != EntityState.Delete).FirstOrDefault();
                if (temp_detailn != null)
                {
                    temp_shbn.CD_SSDW = temp_detailn.CD_SSDW;
                    temp_shbn.CD_SSXL = temp_detailn.CD_SSXL;
                    if (temp_shbn.EntityState != EntityState.Insert)
                        temp_shbn.EntityState = EntityState.Update;
                }
                // 4 更改户表的坐标
                if (!UpdateSdo(temp_shbn.G3E_FID, move_pt))
                {
                    PublicMethod.Instance.AlertDialog("迁移失败!\n");
                    return;
                }
                DBManager.Instance.Update(temp_shbpt);
                DBManager.Instance.Update(temp_shbn);
                PublicMethod.Instance.AlertDialog("迁移成功!\n");
                DialogResult = DialogResult.OK;
                Close();
            }
            catch (NotExistException ex)
            {
                PublicMethod.Instance.NotExistTable(ex);
            }
            catch (Exception exception)
            {
                PublicMethod.Instance.ShowMessage(exception.Message);
            }
        }
        /// <summary>
        /// 更新户表的坐标
        /// </summary>
        /// <param name="g3efid">户表fid</param>
        /// <param name="WorldPt">户表地理坐标</param>
        private bool UpdateSdo(long g3efid, double [] WorldPt)
        {
            try
            {
                var shbsdo = DBManager.Instance.GetEntities<Gg_jx_shbd_pt_sdogeom>(o => o.G3E_FID == g3efid && o.EntityState != EntityState.Delete).FirstOrDefault();
                var mpValue = new Multipoint();
                mpValue.Points.Add(new Point(new[] {WorldPt[1], WorldPt[0], 0}));
                mpValue.Points.Add(new Point(new[] { Math.Cos(0.0), Math.Sin(0.0), 0 }));
                if (shbsdo == null) return false;
                shbsdo.G3E_GEOMETRY = mpValue;
                if (shbsdo.EntityState != EntityState.Insert)
                {
                    shbsdo.EntityState = EntityState.Update;
                }
                DBManager.Instance.Update(shbsdo);
                return true;
            }
            catch (Exception e)
            {
                return false;
            }
        }
    }
}
