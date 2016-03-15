using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using ElectronTransferDal.Common;
using ElectronTransferModel.V9_4;
using ElectronTransferModel;
using System.Text.RegularExpressions;
using ElectronTransferView.FunctionManager;


namespace ElectronTransferView.ContextMenuManager
{
    public partial class FeederEditor : Form
    {
        public FeederEditor()
        {
            InitializeComponent();
        }
        private String FeederName { get; set; }
        private formType m_ft { get; set; }

        public FeederEditor(formType type, String name)
        {
            m_ft = type;
            FeederName = name;
            InitializeComponent();
        }

        /// <summary>
        /// 从馈线管理窗口的树控件中获取选择的变电站与供电所
        /// </summary>
        private void LoadBDZandGDSfromTree()
        {
            TreeNode tmp = FeederManagement.GetSeleteBDZ();
            if (tmp != null)
            {
                cbGDS1.SelectedIndex = cbGDS1.FindStringExact(tmp.Parent.Text, 0);
                cbBDZMC.SelectedIndex = cbBDZMC.FindStringExact(tmp.Text, 0);
            }
        }

        /// <summary>
        /// 根据供电所加载变电站列表
        /// </summary>
        private void BDZLoad()
        {
            //cbBDZMC.Items.Clear();
            //IEnumerable<Gg_kxinfo> ents = null;
            //if (String.IsNullOrEmpty((String)(cbGDS2.SelectedItem)))
            //    ents = DBManager.Instance.GetEntities<Gg_kxinfo>(o => o.GDS1 == (String)(cbGDS1.SelectedItem));
            //else
            //    ents = DBManager.Instance.GetEntities<Gg_kxinfo>(o => (o.GDS1 == (String)(cbGDS1.SelectedItem) || o.GDS1 == (String)(cbGDS2.SelectedItem)));
            //var bdzmcs = ents.Select(o => o.BDZMC).Distinct().ToList();
            //foreach (String tmp in bdzmcs)
            //{
            //    if (String.IsNullOrEmpty(tmp))
            //        cbBDZMC.Items.Add("");
            //    else
            //        cbBDZMC.Items.Add(tmp);
            //}
        }
        /// <summary>
        /// 窗口加载
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void FeederEditor_Load(object sender, EventArgs e)
        {
            //var ents = DBManager.Instance.GetEntities<Gg_kxinfo>();
            ////赋予combox默认值
            //var xzfls = ents.Select(o => o.XZFL).Distinct().ToList();
            //foreach (String tmp in xzfls)
            //{
            //    if (String.IsNullOrEmpty(tmp))
            //        cbXZFL.Items.Add("");
            //    else
            //        cbXZFL.Items.Add(tmp);
            //}
            //var yflls = ents.Select(o => o.YFLL).Distinct().ToList();
            //foreach (String tmp in yflls)
            //{
            //    if (String.IsNullOrEmpty(tmp))
            //        cbYFLL.Items.Add("");
            //    else
            //        cbYFLL.Items.Add(tmp);
            //}
            //var sfcxs = ents.Select(o => o.SFCX).Distinct().ToList();
            //foreach (decimal tmp in sfcxs)
            //{
            //    cbSFCX.Items.Add(String.Format("{0}", tmp));
            //}
            //var gdss = ents.Select(o => o.GDS1).Distinct().ToList();
            //foreach (String tmp in gdss)
            //{
            //    if (String.IsNullOrEmpty(tmp))
            //    {
            //        cbGDS1.Items.Add("");
            //        cbGDS2.Items.Add("");
            //    }
            //    else
            //    {
            //        cbGDS1.Items.Add(tmp);
            //        cbGDS2.Items.Add(tmp);
            //    }
            //}



            ////
            //dateTimePicker1.Checked = false;
            ////
            //if (m_ft == formType.regionAdd)
            //{
            //    this.Text = "添加馈线";
            //    LoadBDZandGDSfromTree();
            //}
            //else
            //{
            //    this.Text = "编辑馈线";
            //    cbBDZMC.Enabled = false;
            //    cbGDS1.Enabled = false;
            //    LoadInfo();
            //}
        }
        /// <summary>
        /// 根据成员馈线fid加载数据
        /// </summary>
        private void LoadInfo()
        {
            //var ent = DBManager.Instance.GetEntity<Gg_kxinfo>(o => o.KXMC == FeederName);
            //if (ent != null)
            //{
            //    tbKXMC.Text = ent.KXMC;
            //    tbCXBH.Text = ent.CXBH;
            //    cbGDS1.SelectedIndex = cbGDS1.FindStringExact(ent.GDS1, 0);
            //    cbGDS2.SelectedIndex = cbGDS2.FindStringExact(ent.GDS2, 0);
            //    cbBDZMC.SelectedIndex = cbBDZMC.FindStringExact(ent.BDZMC, 0);
            //    cbXZFL.SelectedIndex = cbXZFL.FindStringExact(ent.XZFL, 0);
            //    cbYFLL.SelectedIndex = cbYFLL.FindStringExact(ent.YFLL, 0);
            //    cbSFCX.SelectedIndex = (int)ent.SFCX;
            //    if (ent.TCRQ != null && (DateTimePicker.MaximumDateTime > ent.TCRQ && DateTimePicker.MinimumDateTime < ent.TCRQ))
            //    {
            //        dateTimePicker1.Value = ent.TCRQ;
            //    }
            //    //如果没有所属区局则禁止修改
            //    if (String.IsNullOrEmpty(ent.GDS1))
            //    {
            //        tbKXMC.Enabled = false;
            //        tbCXBH.Enabled = false;
            //        cbSFCX.Enabled = false;
            //        cbXZFL.Enabled = false;
            //        cbYFLL.Enabled = false;
            //        cbGDS2.Enabled = false;
            //        dateTimePicker1.Enabled = false;
            //    }
            //}

        }


        /// <summary>
        /// 点击确定按钮
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void OkButton_Click(object sender, EventArgs e)
        {
            this.DialogResult = DialogResult.OK;
            try
            {
                //if (m_ft == formType.regionAdd)//添加操作
                //{
                //    //检查馈线是否存在
                //    var tmp = DBManager.Instance.GetEntity<Cd_ssxl>(o => o.NAME == tbKXMC.Text);
                //    if (tmp != null)//存在,因为cd_ssxl是view,不需状态判断
                //    {
                //        ElectronTransferDal.Cad.PublicMethod.Instance.ShowMessage("馈线已经存在\n");
                //        return;
                //    }
                //    else//不存在
                //    {
                //        //插入cd_ssxl表
                //        Cd_ssxl ssxl = new Cd_ssxl();
                //        ssxl.NAME = tbKXMC.Text;
                //        ssxl.SSBDZ = (String)(cbBDZMC.SelectedItem);
                //        ssxl.CD_SSDW = (String)(cbGDS1.SelectedItem);
                //        DBManager.Instance.Insert(ssxl);

                //        Gg_kxinfo newkxinfo = null;
                //        Gg_kxmanage newkxmanage = null;
                //        //如果在gg_kxinfo中存在,则更新并修改状态
                //        var kxinfo = DBManager.Instance.GetEntity<Gg_kxinfo>(o => o.KXMC == tbKXMC.Text);
                //        if (kxinfo != null)
                //        {
                //            kxinfo.BDZ_FID = DBManager.Instance.GetEntities<Gg_kxinfo>(o => o.BDZMC == (String)(cbBDZMC.SelectedItem)).ToList()[0].BDZ_FID;
                //            kxinfo.BDZMC = (String)(cbBDZMC.SelectedItem);
                //            kxinfo.CXBH = tbCXBH.Text;
                //            kxinfo.GDS1 = (String)(cbGDS1.SelectedItem);
                //            kxinfo.GDS1_ID = DBManager.Instance.GetEntities<Gg_kxinfo>(o => o.GDS1 == (String)(cbGDS1.SelectedItem)).ToList()[0].GDS1_ID;
                //            kxinfo.GDS2 = (String)(cbGDS2.SelectedItem);
                //            kxinfo.GDS2_ID = DBManager.Instance.GetEntities<Gg_kxinfo>(o => o.GDS1 == (String)(cbGDS2.SelectedItem)).ToList()[0].GDS1_ID;
                //            kxinfo.KXMC = tbKXMC.Text;
                //            kxinfo.SFCX = decimal.Parse((String)(cbSFCX.SelectedItem));
                //            kxinfo.TCRQ = dateTimePicker1.Checked ? dateTimePicker1.Value : DateTime.MinValue;
                //            kxinfo.XZFL = (String)(cbXZFL.SelectedItem);
                //            kxinfo.YFLL = (String)(cbYFLL.SelectedItem);
                //            if (kxinfo.EntityState == EntityState.Delete)//删除状态,恢复数据
                //            {
                //                kxinfo.EntityState = EntityState.Update;
                //            }
                //            DBManager.Instance.Update(kxinfo);
                //            //如果在Gg_kxmanage中存在,则更新并修改状态
                //            var kxmanage = DBManager.Instance.GetEntity<Gg_kxmanage>(o => o.KX_ID == kxinfo.KX_ID);
                //            if (kxmanage != null)
                //            {
                //                kxmanage.SSDW = (String)(cbGDS1.SelectedItem);
                //                if (kxmanage.EntityState == EntityState.Delete)//删除状态,恢复数据
                //                {
                //                    kxmanage.EntityState = EntityState.Update;
                //                }
                //                DBManager.Instance.Update(kxmanage);
                //            }
                //            else//如果在Gg_kxmanage中不存在,则插入
                //            {
                //                newkxmanage = new Gg_kxmanage();
                //                newkxmanage.ID = CYZCommonFunc.getid();
                //                newkxmanage.KX_ID = kxinfo.KX_ID;
                //                newkxmanage.SSDW = (String)(cbGDS1.SelectedItem);
                //                newkxmanage.EntityState = EntityState.Insert;
                //                DBManager.Instance.Insert(newkxmanage);
                //            }
                //        }
                //        else//如果在gg_kxinfo中不存在,则插入
                //        {
                //            newkxinfo = new Gg_kxinfo();
                //            newkxinfo.BDZ_FID = DBManager.Instance.GetEntities<Gg_kxinfo>(o => o.BDZMC == (String)(cbBDZMC.SelectedItem)).ToList()[0].BDZ_FID;
                //            newkxinfo.BDZMC = (String)(cbBDZMC.SelectedItem);
                //            newkxinfo.CXBH = tbCXBH.Text;
                //            newkxinfo.GDS1 = (String)(cbGDS1.SelectedItem);
                //            newkxinfo.GDS1_ID = DBManager.Instance.GetEntities<Gg_kxinfo>(o => o.GDS1 == (String)(cbGDS1.SelectedItem)).ToList()[0].GDS1_ID;
                //            newkxinfo.GDS2 = (String)(cbGDS2.SelectedItem);
                //            newkxinfo.GDS2_ID = DBManager.Instance.GetEntities<Gg_kxinfo>(o => o.GDS1 == (String)(cbGDS2.SelectedItem)).ToList()[0].GDS1_ID;
                //            newkxinfo.KX_ID = CYZCommonFunc.getid();
                //            newkxinfo.KXMC = tbKXMC.Text;
                //            newkxinfo.SFCX = decimal.Parse((String)(cbSFCX.SelectedItem));
                //            newkxinfo.TCRQ = dateTimePicker1.Value;
                //            newkxinfo.XZFL = (String)(cbXZFL.SelectedItem);
                //            newkxinfo.YFLL = (String)(cbYFLL.SelectedItem);
                //            newkxinfo.YX_FID = CYZCommonFunc.getid();
                //            newkxinfo.EntityState = EntityState.Insert;
                //            DBManager.Instance.Insert(newkxinfo);
                //            //并且也插入Gg_kxmanage
                //            newkxmanage = new Gg_kxmanage();
                //            newkxmanage.ID = CYZCommonFunc.getid();
                //            newkxmanage.KX_ID = newkxinfo.KX_ID;
                //            newkxmanage.SSDW = (String)(cbGDS1.SelectedItem);
                //            newkxmanage.EntityState = EntityState.Insert;
                //            DBManager.Instance.Insert(newkxmanage);
                //        }
                //    }
                //}
                //else//编辑操作
                //{
                //    //更新cd_ssxl表
                //    Cd_ssxl ssxl = DBManager.Instance.GetEntity<Cd_ssxl>(o => o.NAME == FeederName);
                //    if (ssxl != null)
                //    {
                //        Cd_ssxl newssxl = (Cd_ssxl)ssxl.Clone();
                //        newssxl.NAME = tbKXMC.Text;
                //        newssxl.SSBDZ = (String)(cbBDZMC.SelectedItem);
                //        newssxl.CD_SSDW = (String)(cbGDS1.SelectedItem);
                //        DBManager.Instance.Update(newssxl, ssxl);
                //    }
                //    else
                //    {
                //        throw new Exception();
                //    }
                //    //更新gg_kxinfo表
                //    Gg_kxinfo kxinfo = DBManager.Instance.GetEntity<Gg_kxinfo>(o => o.KXMC == FeederName);
                //    if (kxinfo != null)
                //    {
                //        kxinfo.BDZ_FID = DBManager.Instance.GetEntities<Gg_kxinfo>(o => o.BDZMC == (String)(cbBDZMC.SelectedItem)).ToList()[0].BDZ_FID;
                //        kxinfo.BDZMC = (String)(cbBDZMC.SelectedItem);
                //        kxinfo.CXBH = tbCXBH.Text;
                //        kxinfo.GDS1 = (String)(cbGDS1.SelectedItem);
                //        kxinfo.GDS1_ID = DBManager.Instance.GetEntities<Gg_kxinfo>(o => o.GDS1 == (String)(cbGDS1.SelectedItem)).ToList()[0].GDS1_ID;
                //        kxinfo.GDS2 = (String)(cbGDS2.SelectedItem);
                //        kxinfo.GDS2_ID = DBManager.Instance.GetEntities<Gg_kxinfo>(o => o.GDS1 == (String)(cbGDS2.SelectedItem)).ToList()[0].GDS1_ID;
                //        kxinfo.KXMC = tbKXMC.Text;
                //        kxinfo.SFCX = decimal.Parse((String)(cbSFCX.SelectedItem));
                //        kxinfo.TCRQ = dateTimePicker1.Checked ? dateTimePicker1.Value : DateTime.MinValue;
                //        kxinfo.XZFL = (String)(cbXZFL.SelectedItem);
                //        kxinfo.YFLL = (String)(cbYFLL.SelectedItem);
                //        if (kxinfo.EntityState != EntityState.Insert)//如果之前不是insert
                //        {
                //            kxinfo.EntityState = EntityState.Update;//修改成update
                //        }
                //        DBManager.Instance.Update(kxinfo);
                //        //更新gg_kxmanage表
                //        Gg_kxmanage kxmanage = DBManager.Instance.GetEntity<Gg_kxmanage>(o => o.KX_ID == kxinfo.KX_ID);
                //        if (kxmanage != null)
                //        {
                //            kxmanage.SSDW = (String)(cbGDS1.SelectedItem);
                //            if (kxmanage.EntityState != EntityState.Insert)//如果之前不是insert
                //            {
                //                kxmanage.EntityState = EntityState.Update;//修改成update
                //            }
                //            DBManager.Instance.Update(kxmanage);
                //        }
                //        else//发现不存在表中则插入
                //        {
                //            Gg_kxmanage newkxmanage = new Gg_kxmanage();
                //            newkxmanage.ID = CYZCommonFunc.getid();
                //            newkxmanage.KX_ID = kxinfo.KX_ID;
                //            newkxmanage.SSDW = (String)(cbGDS1.SelectedItem);
                //            newkxmanage.EntityState = EntityState.Insert;
                //            DBManager.Instance.Insert(newkxmanage);
                //        }
                //    }
                //    else
                //    {
                //        throw new Exception();
                //    }
                //}
            }
            catch (Exception ee)
            {
                ElectronTransferDal.Cad.PublicMethod.Instance.ShowMessage(String.Format("操作过程中出现错误:{0}", ee.Message));
            }
            this.Close();
        }


        /// <summary>
        /// 取消按钮
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void CancelButton_Click(object sender, EventArgs e)
        {
            this.DialogResult = DialogResult.Cancel;
            this.Close();
        }

        /// <summary>
        /// 检查确定按钮是否可用
        /// </summary>
        /// <returns></returns>
        private bool CheckButtonEnable()
        {
            if (String.IsNullOrEmpty(cbBDZMC.Text) ||
                String.IsNullOrEmpty(tbKXMC.Text) ||
                String.IsNullOrEmpty(tbCXBH.Text) ||
                String.IsNullOrEmpty(cbGDS1.Text) ||
                String.IsNullOrEmpty(cbSFCX.Text))
                return false;
            return true;
        }

        private void tbBDZMC_TextChanged(object sender, EventArgs e)
        {
            if (CheckButtonEnable()) btnOK.Enabled = true;
            else btnOK.Enabled = false;
        }

        private void tbCXBH_TextChanged(object sender, EventArgs e)
        {
            if (CheckButtonEnable()) btnOK.Enabled = true;
            else btnOK.Enabled = false;
        }

        private void tbKXMC_TextChanged(object sender, EventArgs e)
        {
            if (CheckButtonEnable()) btnOK.Enabled = true;
            else btnOK.Enabled = false;
            //利用正则去掉前头的中文
            Regex regex = new Regex(@"^[\u4e00-\u9fa5]+");
            Match mat = regex.Match(tbKXMC.Text);
            if (mat.Success)
            {
                String cxbh = tbKXMC.Text.Remove(mat.Index, mat.Length);
                tbCXBH.Text = cxbh;
            }
            else
            {
                tbCXBH.Text = tbKXMC.Text;
            }
        }

        private void cbGDS1_TextChanged(object sender, EventArgs e)
        {
            if (CheckButtonEnable()) btnOK.Enabled = true;
            else btnOK.Enabled = false;
            BDZLoad();
        }

        private void cbSFCX_TextChanged(object sender, EventArgs e)
        {
            if (CheckButtonEnable()) btnOK.Enabled = true;
            else btnOK.Enabled = false;
        }

        private void cbGDS2_TextChanged(object sender, EventArgs e)
        {
            if (m_ft == formType.regionAdd)
            {
                String tmp = (String)cbBDZMC.SelectedItem;
                BDZLoad();
                cbBDZMC.SelectedIndex = cbBDZMC.FindStringExact(tmp);
            }
        }
    }
}
