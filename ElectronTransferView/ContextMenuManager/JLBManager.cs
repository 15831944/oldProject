using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using ElectronTransferDal;
using ElectronTransferDal.Common;
using ElectronTransferDal.FunctionHelper;
using ElectronTransferFramework;
using ElectronTransferModel.Base;
using ElectronTransferModel.V9_4;
using ElectronTransferDal.Cad;
using ElectronTransferModel;
using ElectronTransferView.FunctionManager;

namespace ElectronTransferView.ContextMenuManager
{
    public partial class JLBManager : Form
    {
        /// <summary>
        /// 变压器或计量柜GE3_FID
        /// </summary>
        private long Fid { get; set; }
        public JLBManager()
        {
            InitializeComponent();
        }
        public JLBManager(long fid)
        {
            Fid = fid;
            InitializeComponent();
        }

        private void addToolStripMenuItem_Click(object sender, EventArgs e)
        {
            ToolStripMenuItem mi = sender as ToolStripMenuItem;
            JLBEditer tt = new JLBEditer(formType.regionAdd,  Fid);
            tt.Tag = mi.Text;
            tt.Text = "新增计量表";
            if (DialogResult.OK == tt.ShowDialog())
            {
                updatelistview();
            }            
        }

        private void editToolStripMenuItem_Click(object sender, EventArgs e)
        {
            ToolStripMenuItem mi = sender as ToolStripMenuItem;
            long tfid = (long)listView1.SelectedItems[0].Tag;
            JLBEditer tt = new JLBEditer(formType.regionEdit, tfid);
            tt.Tag = mi.Text;
            tt.Text = "修改计量表";
            if (DialogResult.OK == tt.ShowDialog())
            {
                updatelistview();
            }    
        }

        private void deleteToolStripMenuItem_Click(object sender, EventArgs e)
        {
            long tfid = (long)listView1.SelectedItems[0].Tag;
            DeleteJLB(tfid,new List<DBEntity>());
            updatelistview();
        }

        private void JLBManager_Load(object sender, EventArgs e)
        {
            updatelistview();
        }

        private void updatelistview()
        {
            try
            {
                listView1.Clear();

                var jlbs = GetJLB_NS();
                foreach (var jlb in jlbs)
                {
                    var lv = listView1.Items.Add(String.Format("{0}", jlb.G3E_FID), jlb.HM, 1);
                    lv.Tag = jlb.G3E_FID;
                }
            }
            catch (Exception ex)
            {
                PublicMethod.Instance.ShowMessage(ex.Message);
            }
        }

        private IEnumerable<Gg_pd_jlb_n> GetJLB_NS()
        {
            try
            {
                var jlbs = new List<Gg_pd_jlb_n>();
                var detail = DBManager.Instance.GetEntities<Detailreference_n>(o => o.G3E_FID == Fid && o.EntityState != EntityState.Delete).FirstOrDefault();
                if (detail == null) return null;
                var jxjlbs = DBManager.Instance.GetEntities<Gg_jx_jlb_pt>(o => o.G3E_DETAILID == detail.G3E_DETAILID && o.EntityState != EntityState.Delete);
                if (jxjlbs == null) return null;
                foreach (var jlb in jxjlbs)
                {
                    jlbs.Add(GetJLB_N(jlb.G3E_FID));
                }
                return jlbs;
            }
            catch (Exception)
            {
                return null;
            }
        }
        private static Gg_pd_jlb_n GetJLB_N(long? fid)
        {
            try
            {
                var t = DBManager.Instance.GetEntities<Gg_pd_jlb_n>(o => o.G3E_FID == fid && o.EntityState != EntityState.Delete).FirstOrDefault();
                return t;
            }
            catch (Exception)
            {
                return null;
            }
        }

        private static List<DBEntity> DeleteJLB(long? fid, List<DBEntity> backupEntity)
        {
            // 删除Gg_jx_jlb_pt
            var jlbpt = DBManager.Instance.GetEntities<Gg_jx_jlb_pt>(o => o.G3E_FID == fid && o.EntityState != EntityState.Delete).FirstOrDefault();
            if (null != jlbpt)
            {
                backupEntity=DBEntityErased.Instance.GetEntityStateByErasedDBEntity(jlbpt,backupEntity);
            }
            // 删除Gg_pd_jlb_n
            var jlbn = GetJLB_N(fid);
            if (null != jlbn)
            {
                backupEntity = DBEntityErased.Instance.GetEntityStateByErasedDBEntity(jlbn, backupEntity);
            }
            // 删除Common_n
            var jlbcomm = DBManager.Instance.GetEntities<Common_n>(o => o.G3E_FID == fid && o.EntityState != EntityState.Delete).FirstOrDefault();
            if (null != jlbcomm)
            {
                backupEntity = DBEntityErased.Instance.GetEntityStateByErasedDBEntity(jlbcomm, backupEntity);
            }

            // 删除Connectivity_n
            var jlbconn = DBManager.Instance.GetEntities<Connectivity_n>(o => o.G3E_FID == fid && o.EntityState != EntityState.Delete).FirstOrDefault();
            if (null != jlbconn)
            {
                backupEntity = DBEntityErased.Instance.GetEntityStateByErasedDBEntity(jlbconn, backupEntity);
            }

            // 删除Gg_pd_gnwzmc_n
            var jlbgnwz = DBManager.Instance.GetEntities<Gg_pd_gnwzmc_n>(o => o.G3E_FID == fid && o.EntityState != EntityState.Delete).FirstOrDefault();
            if (null != jlbgnwz)
            {
                backupEntity = DBEntityErased.Instance.GetEntityStateByErasedDBEntity(jlbgnwz, backupEntity);
            }

            // 删除GG_JX_JLB_LB 暂不需要
            // 删除GNWZ_SBTZ_RSHIP 暂不需要
            return backupEntity;
        }

        private void contextMenuStrip1_Opening(object sender, CancelEventArgs e)
        {
            int cout = listView1.SelectedItems.Count;
            switch (cout)
            {
                case 0:
                    contextMenuStrip1.Items[0].Enabled = true;
                    contextMenuStrip1.Items[1].Enabled = false;
                    contextMenuStrip1.Items[2].Enabled = false;
                    break;
                case 1:
                    contextMenuStrip1.Items[0].Enabled = true;
                    contextMenuStrip1.Items[1].Enabled = true;
                    contextMenuStrip1.Items[2].Enabled = true;
                    break;
                default:
                    contextMenuStrip1.Items[0].Enabled = true;
                    contextMenuStrip1.Items[1].Enabled = false;
                    contextMenuStrip1.Items[2].Enabled = true;
                    break;
            }
        }

        private void listView1_ItemMouseHover(object sender, ListViewItemMouseHoverEventArgs e)
        {
            var jlb = DBManager.Instance.GetEntities<Gg_pd_jlb_n>(o => o.G3E_FID == (long)e.Item.Tag).FirstOrDefault();
            e.Item.ToolTipText = String.Format("户名:{0}\n用户号:{1}\n备注:{2}\nG3E_FID:{3}", jlb.HM, jlb.YHH, jlb.BZ, jlb.G3E_FID);
        }


        /// <summary>
        /// 批量删除
        /// </summary>
        /// <param name="fid"></param>
        /// <param name="backupDBEntity"> </param>
        /// <returns></returns>
        //public static List<DBEntity> BatchDeleteFromJLB(long fid, List<DBEntity> backupDBEntity)
        //{
        //    try
        //    {
        //        //获取所有计量表
        //        var t = DBManager.Instance.GetEntity<Detailreference_n>(fid);
        //        if (t != null)
        //        {
        //            var ents = GetJLB_PT(t.G3E_DETAILID);
        //            if (ents != null)
        //            {
        //                foreach (long tmp in ents)
        //                {
        //                    backupDBEntity= DeleteJLB(tmp, backupDBEntity);
        //                }
        //            }
        //        }
        //    }
        //    catch (Exception ex)
        //    {
        //        LogManager.Instance.Error(ex);
        //    }
        //    return backupDBEntity;
        //}

        //private static IEnumerable<long> GetJLB_PT(long? g3e_detailid)
        //{
        //    try
        //    {
        //        return DBManager.Instance.GetEntities<Gg_jx_jlb_pt>(o => o.G3E_DETAILID == g3e_detailid).Select(o => o.G3E_FID).ToList();
        //    }
        //    catch (Exception)
        //    {
        //        return null;
        //    }
        //}
    }
}
