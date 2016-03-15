using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using ArxMap;
using ElectronTransferDal.Cad;
using ElectronTransferDal.Common;
using ElectronTransferDal.Query;
using ElectronTransferModel;
using ElectronTransferModel.Base;
using ElectronTransferModel.V9_4;
using ElectronTransferView.ContextMenuManager;
using ElectronTransferView.SearchManager;
using ElectronTransferFramework;

namespace ElectronTransferView.SearchManager
{
    public partial class YHHSearch : Form
    {
        public YHHSearch()
        {
            InitializeComponent();
        }

        private void btn_ok_Click(object sender, EventArgs e)
        {
            try
            {
                lb_SHBWarn.Text = "";
                lb_SHBWarn.ForeColor = Color.Red;

                if (string.IsNullOrEmpty(tx_SHBYDH.Text))
                {
                    lb_SHBWarn.Text = "用户号不能为空.";
                    return;
                }
                var temp_shb = DBManager.Instance.GetEntities<Gg_pd_dyshb_n>(o => o.YDH == tx_SHBYDH.Text.Trim()).FirstOrDefault();
                if (temp_shb == null)
                {
                    lb_SHBWarn.Text = "用户号不存在.";
                    return;
                }

                var temp_shbpt =
                    DBManager.Instance.GetEntities<Gg_jx_shbd_pt>(
                        o => o.G3E_FID == temp_shb.G3E_FID && o.EntityState != EntityState.Delete).FirstOrDefault();
                if (temp_shbpt == null) return;

                var temp_detail =
                    DBManager.Instance.GetEntities<Detailreference_n>(
                        o => o.G3E_DETAILID == temp_shbpt.G3E_DETAILID && o.EntityState != EntityState.Delete).FirstOrDefault();
                if (temp_detail == null) return;

                var g3eObject = new G3EObject
                {
                    G3E_ID = temp_detail.G3E_ID,
                    G3E_FID = temp_detail.G3E_FID,
                    G3E_FNO = temp_detail.G3E_FNO
                };

                //var shbview = new SHBMap(temp_detail.G3E_FID, temp_shb.G3E_FID);
                var shbview = new SHBMap(new selectEntityArgs
                {
                    g3eObject = g3eObject
                }, temp_shb.G3E_FID);
                Autodesk.AutoCAD.ApplicationServices.Application.ShowModalDialog(shbview);
                FixEntity.Instance.Fix(temp_detail.G3E_FID);
            }
            catch (Exception ex)
            {
                if (ex.Message.Contains("不存在"))
                {
                    var strlx = ex.Message.Replace("不存在", "");
                    var type = TypeCache.Instance.GetTypeFromCache(typeof(DBEntity), strlx);
                    var connectn = ReflectionUtils.CreateObject(new { }, type) as DBEntity;
                    DBManager.Instance.Insert(connectn);
                }	
                else
                {
                    PublicMethod.Instance.ShowMessage(ex.Message);
                }
            }
        }

        private void SHBSearch_FormClosing(object sender, FormClosingEventArgs e)
        {
            FixEntity.Instance.ResetOldEntity();
        }

        private void btn_JLBOK_Click(object sender, EventArgs e)
        {
            try
            {
                lb_JLBWarn.Text = "";
                lb_JLBWarn.ForeColor = Color.Red;

                if (string.IsNullOrEmpty(tx_JLBYHH.Text))
                {
                    lb_JLBWarn.Text = "用户号不能为空.";
                    return;
                }
                var temp_jlb = DBManager.Instance.GetEntities<Gg_pd_jlb_n>(o => o.YHH == tx_JLBYHH.Text.Trim()).FirstOrDefault();
                if (temp_jlb == null)
                {
                    lb_JLBWarn.Text = "用户号不存在.";
                    return;
                }

                var temp_jlbpt =
                    DBManager.Instance.GetEntities<Gg_jx_jlb_pt>(
                        o => o.G3E_FID == temp_jlb.G3E_FID && o.EntityState != EntityState.Delete).FirstOrDefault();
                if (temp_jlbpt == null) return;

                var temp_detail =
                    DBManager.Instance.GetEntities<Detailreference_n>(
                        o => o.G3E_DETAILID == temp_jlbpt.G3E_DETAILID && o.EntityState != EntityState.Delete).FirstOrDefault();
                if (temp_detail == null) return;

                var g3eObject = new G3EObject
                                          {
                                              G3E_ID = temp_detail.G3E_ID,
                                              G3E_FID = temp_detail.G3E_FID,
                                              G3E_FNO = temp_detail.G3E_FNO
                                          };
                var jlbmap = new JLBMap(new selectEntityArgs
                {
                    g3eObject = g3eObject
                }, temp_jlb.G3E_FID);
                Autodesk.AutoCAD.ApplicationServices.Application.ShowModalDialog(jlbmap);
                FixEntity.Instance.Fix(temp_detail.G3E_FID);
            }
            catch (Exception exception)
            {
                if (exception.Message.Contains("不存在"))
                {
                    var strlx = exception.Message.Replace("不存在", "");
                    var type = TypeCache.Instance.GetTypeFromCache(typeof(DBEntity), strlx);
                    var connectn = ReflectionUtils.CreateObject(new { }, type) as DBEntity;
                    DBManager.Instance.Insert(connectn);
                }
                else
                {
                    PublicMethod.Instance.ShowMessage(exception.Message);
                }
                //Console.WriteLine(exception);
            }
        }
    }
}
