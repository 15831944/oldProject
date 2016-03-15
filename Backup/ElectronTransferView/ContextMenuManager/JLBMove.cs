using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using ElectronTransferDal.Cad;
using ElectronTransferDal.Common;
using ElectronTransferDal.Common.Exceptions;
using ElectronTransferDal.Query;
using ElectronTransferModel;
using ElectronTransferModel.Config;
using ElectronTransferModel.Geo;
using ElectronTransferModel.V9_4;
using Point = System.Drawing.Point;
using System.Drawing;

namespace ElectronTransferView.ContextMenuManager
{
    public partial class JLBMove : Form
    {
        /// <summary>
        /// 当前详图的g3e_deatilID
        /// </summary>
        private long g3e_detailId { get; set; }

        /// <summary>
        /// 当前迁移坐标
        /// </summary>
        private double[] move_pt { get; set; }

        /// <summary>
        /// 计量表的G3E_FID
        /// </summary>
        public long g3e_Fid { get; set; }

        /// <summary>
        /// 户表门牌地址(HH)
        /// </summary>
        public string SBLX { get; set; }

        public JLBMove()
        {
            InitializeComponent();
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="detailid">详表ID</param>
        /// <param name="mpt">迁移坐标</param>
        public JLBMove(long detailid, double[] mpt)
        {
            move_pt = mpt;
            g3e_detailId = detailid;
            InitializeComponent();
        }

        private void btn_JLBmove_Click(object sender, EventArgs e)
        {
            try
            {
                // 提示文字颜色
                lb_JLBWarn.Text = "";
                lb_JLBWarn.ForeColor = Color.Red;
                // 输入文本框是否为空
                if (string.IsNullOrEmpty(tx_JLBYDH.Text))
                {
                    lb_JLBWarn.Text = "用户号不能为空.";
                    return;
                }
                // 迁移户表的自身属性表
                var temp_jlbn =
                    DBManager.Instance.GetEntities<Gg_pd_jlb_n>(o => o.YHH == tx_JLBYDH.Text.Trim()).FirstOrDefault();
                if (temp_jlbn == null)
                {
                    lb_JLBWarn.Text = "用户号不存在.";
                    return;
                }
                // 计量表的设备类型
                SBLX = temp_jlbn.CD_SBLX;
                // 迁移计量表的g3e_fid
                g3e_Fid = temp_jlbn.G3E_FID;
                // 得到迁移计量表的坐标表
                var temp_jlbpt =
                    DBManager.Instance.GetEntities<Gg_jx_jlb_pt>(
                        o => o.G3E_FID == temp_jlbn.G3E_FID && o.EntityState != EntityState.Delete).FirstOrDefault();
                if (temp_jlbpt == null)
                {
                    lb_JLBWarn.Text = "该用户号的计量表坐标表不存在.";
                    return;
                }
                // 判断是否在当前集抄箱
                if (temp_jlbpt.G3E_DETAILID == g3e_detailId)
                {
                    lb_JLBWarn.Text = "该用户号的计量表表已在此详图中.";
                    return;
                }
                // 取出需要被迁移计量表的详表数据
                var temp_detail =
                    DBManager.Instance.GetEntities<Detailreference_n>(
                        o => o.G3E_DETAILID == temp_jlbpt.G3E_DETAILID && o.EntityState != EntityState.Delete)
                        .FirstOrDefault();
                if (temp_detail == null)
                {
                    lb_JLBWarn.Text = "该用户号的户表详表数据不存在.";
                    return;
                }
                // 判断需要被迁移的计量表的详图是否被锁定
                var isLock = DBEntityFinder.Instance.VerifyLTTID(temp_detail.G3E_FID);
                if (!isLock)
                {
                    lb_JLBWarn.Text = "户表所属抄表箱没有被工单锁定,不能迁移!";
                    return;
                }

                var str = "户名: " + temp_jlbn.HM + "\n"
                          + "用户号: " + temp_jlbn.YHH + "\n"
                          + "表号: " + temp_jlbn.BH + "\n"
                          + "确定要迁移?";
                // 搜索成功,弹出提示框
                var res = MessageBox.Show(str, @"确定?", MessageBoxButtons.OKCancel);
                if (res == DialogResult.Cancel) return;
                // 迁移户表操作
                // 1 更改户表的detailID
                temp_jlbpt.G3E_DETAILID = g3e_detailId;
                if (temp_jlbpt.EntityState != EntityState.Insert)
                    temp_jlbpt.EntityState = EntityState.Update;
                // 2 得到户表的FID
                // 3 保持与当前详图的所属线路、单位一致
                var temp_detail_comm =
                    DBManager.Instance.GetEntities<Common_n>(
                        o => o.G3E_FID == temp_detail.G3E_FID && o.EntityState != EntityState.Delete).FirstOrDefault();
                Common_n temp_jlb_comm = null;
                if (temp_detail_comm != null)
                {
                    temp_jlb_comm =
                        DBManager.Instance.GetEntities<Common_n>(
                            o => o.G3E_FID == g3e_Fid && o.EntityState != EntityState.Delete).FirstOrDefault();
                    if (temp_jlb_comm != null)
                    {
                        temp_jlb_comm.CD_SSDW = temp_detail_comm.CD_SSDW;
                        temp_jlb_comm.CD_SSXL = temp_detail_comm.CD_SSXL;
                        if (temp_jlb_comm.EntityState != EntityState.Insert)
                            temp_jlb_comm.EntityState = EntityState.Update;
                    }
                }
                // 4 更改户表的坐标
                if (!UpdateSdo(temp_jlbn.G3E_FID, move_pt))
                {
                    PublicMethod.Instance.AlertDialog("迁移失败!\n");
                    return;
                }
                DBManager.Instance.Update(temp_jlbpt);
                DBManager.Instance.Update(temp_jlb_comm);
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
        private bool UpdateSdo(long g3efid, double[] WorldPt)
        {
            try
            {
                var shbsdo =
                    DBManager.Instance.GetEntities<Gg_jx_jlb_pt_sdogeom>(
                        o => o.G3E_FID == g3efid && o.EntityState != EntityState.Delete).FirstOrDefault();
                var mpValue = new Multipoint();
                mpValue.Points.Add(new ElectronTransferModel.Geo.Point(new[] {WorldPt[1], WorldPt[0], 0}));
                mpValue.Points.Add(new ElectronTransferModel.Geo.Point(new[] {Math.Cos(0.0), Math.Sin(0.0), 0}));
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

