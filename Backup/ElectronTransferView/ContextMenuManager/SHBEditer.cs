using System;
using System.Collections.Generic;
using System.Text;
using System.Text.RegularExpressions;
using System.Drawing;
using System.Linq;
using System.Windows.Forms;
using ElectronTransferDal.Cad;
using ElectronTransferDal.Common;
using ElectronTransferDal.Query;
using ElectronTransferModel.V9_4;
using ElectronTransferModel;
using ElectronTransferView.FunctionManager;
using ElectronTransferModel.Config;
using ElectronTransferModel.Geo;
using Autodesk.AutoCAD.Geometry;
using Point = ElectronTransferModel.Geo.Point;
using ElectronTransferDal.Common.Exceptions;
using ElectronTransferModel.Base;
using ElectronTransferFramework;

namespace ElectronTransferView.ContextMenuManager
{
    public partial class SHBEditer : Form
    {
        public SHBEditer()
        {
            InitializeComponent();
        }
        public SHBEditer(formType type, long fid, long detailfid, bool isLock)
        {
            Ft = type;
            FID = fid;
            g_DetailFid = detailfid;
            IsLock = isLock;
            InitializeComponent();
        }
        public SHBEditer(formType type, long fid, Point3d pt, bool isLock)
        {
            Ft = type;
            FID = fid;
            IsLock = isLock;
            shbPositon = pt;
            InitializeComponent();
        }
        public SHBEditer(formType type, long fid, double[] pt, bool isLock)
        {
            Ft = type;
            FID = fid;
            IsLock = isLock;
            shbPositon = new Point3d(pt[1], pt[0], 0);
            InitializeComponent();
        }
        /// <summary>
        /// 抄表箱G3E_FID
        /// </summary>
        private long g_DetailFid { get; set; }
        private formType Ft { get; set; }
        /// <summary>
        /// 如果是编辑则是户表FID,如果是添加则是集抄箱FID
        /// </summary>
        private long FID { get; set; }
        private Point3d shbPositon { get; set; }
        public long g_newFid { get; set; }
        public string AZDZ { get; set; }
        /// <summary>
        /// 是否锁定
        /// </summary>
        private bool IsLock { set; get; }

        private void InitCb()
        {
            var ents_dylb = GetDYSHBN_YDLB();
            var ents_xs = GetDYSHBN_XS();
            var ents_yhlx = GetDYSHBN_YHLX();
            var ents_gzzt = GetDYSHBN_GZZT();
            var ents_smzq = GetDYSHBN_SMZQ();

            //用电类别初始化
            if (ents_dylb != null)
            {
                var ydlbs = ents_dylb.Select(o => o.NAME).Distinct().ToList();
                ydlbs.Add(" ");
                cbYDLB.DataSource = ydlbs;
                cbYDLB.Text = " ";
            }
            // 用户类型初始化
            if (ents_yhlx != null)
            {
                var yhlxs = ents_yhlx.Select(o => o.NAME).Distinct().ToList();
                yhlxs.Add(" ");
                cbYHLX.DataSource = yhlxs;
                cbYHLX.Text = " ";
            }
            //初始化工作状态
            if (ents_gzzt != null)
            {
                var gzzts = ents_gzzt.Select(o => o.NAME).Distinct().ToList();
                cbGZZT.DataSource = gzzts;
                cbGZZT.Text = "正常";
            }
            //初始化相数
            if (ents_xs != null)
            {
                var xss = ents_xs.Select(o => o.NAME).Distinct().ToList();
                xss.Add(" ");
                cbXS.DataSource = xss;
                cbXS.Text = " ";
            }
            //初始化生命周期
            if (ents_smzq != null)
            {
                var smzqs = ents_smzq.Select(o => o.NAME).Distinct().ToList();
                cbSMZQ.DataSource = smzqs;
                cbSMZQ.Text = "投运";
            }
        }

        private void SHBEditer_Load(object sender, EventArgs e)
        {
            //窗体不可调节大小
            FormBorderStyle = FormBorderStyle.FixedSingle;
            InitCb(); 
            Text = Ft == formType.regionAdd ? "添加户表" : "编辑户表";
            LoadInfo();
            VerifyFid();
        }
        /// <summary>
        /// 加载信息
        /// </summary>
        private void LoadInfo()
        {
            try
            {
                if (Ft == formType.regionAdd)
                {
                    //从集抄箱表中加载供电局等已固定数据
                    var ent = DBManager.Instance.GetEntity<Gg_pd_cbx_n>(o => o.G3E_FID == FID);
                    //cbCD_SSDW.SelectedIndex = cbCD_SSDW.FindStringExact(ent.CD_SSDW, 0);
                    tbCD_SSDW.Text = ent.CD_SSDW;
                    tbCD_SSXL.Text = ent.CD_SSXL;
                }
                else
                {
                    //从散户表表中加载数据
                    var ent = GetDYSHB_N();// DBManager.Instance.GetEntity<Gg_pd_dyshb_n>(o => o.G3E_FID == FID);
                    if (ent != null)
                    {
                        tbYDH.Text = ent.YDH;
                        tbYHXM.Text = ent.YHXM;
                        tbBH.Text = ent.BH;
                        tbGDH.Text = ent.GDH;
                        tbHBLX.Text = ent.HBLX;
                        tbDZJL.Text = ent.DZJL;
                        tbAZDZ.Text = ent.AZDZ;
                        //cbCD_SSDW.SelectedIndex = cbCD_SSDW.FindStringExact(ent.CD_SSDW, 0);
                        tbCD_SSDW.Text = ent.CD_SSDW;
                        tbCD_SSXL.Text = ent.CD_SSXL;
                        tbBXH.Text = ent.BXH;
                        cbYDLB.SelectedIndex = cbYDLB.FindStringExact(ent.YDLB, 0);
                        cbYHLX.SelectedIndex = cbYHLX.FindStringExact(ent.YHLX, 0);
                        cbSMZQ.SelectedIndex = cbSMZQ.FindStringExact(ent.SMZQ, 0);
                        tbMBH.Text = ent.MBH;
                        tbBZ.Text = ent.BZ;
                        cbGZZT.SelectedIndex = cbGZZT.FindStringExact(ent.GZZT, 0);
                        tbHH.Text = ent.HH;
                        cbXS.SelectedIndex = cbXS.FindStringExact(ent.XS, 0);
                        tbQT.Text = ent.QT;
                        tbSFYHYB.Text = ent.SFYHYB;
                        tbXYH.Text = ent.KHXYH;
                        ent.LTT_ID = MapConfig.Instance.LTTID;
                        //ent.LTT_DATE;
                        //ent.LTT_STATUS;
                        //ent.LTT_TID;
                        //ent.DWFX_SFSDY;
                        //ent.CD_SSBDZ;
                    }
                    else
                    {
                        throw new Exception("找不到实体");
                    }
                }
            }
            catch (Exception ee)
            {
                PublicMethod.Instance.ShowMessage(String.Format("加载数据过程中出现错误:{0}", ee.Message));
            }
        }

        private void VerifyFid()
        {
            // 校验FID是否被锁定
            //var temshb =DBManager.Instance.GetEntities<Gg_jx_shbd_pt>(o => o.G3E_FID == FID && o.EntityState != EntityState.Delete).FirstOrDefault();
            //var detail =
            //    DBManager.Instance.GetEntities<Detailreference_n>(o => o.G3E_DETAILID == temshb.G3E_DETAILID && o.EntityState != EntityState.Delete).FirstOrDefault();

            if (!IsLock)
            {
                Text = @"查看户表";
                tbAZDZ.ReadOnly = true;
                tbBH.ReadOnly = true;
                tbBXH.ReadOnly = true;
                tbHH.ReadOnly = true;
                tbMBH.ReadOnly = true;
                cbSMZQ.Enabled = false;
                tbHBLX.Enabled = false;
                tbCD_SSDW.ReadOnly = true;
                tbDZJL.ReadOnly = true;
                tbGDH.ReadOnly = true;
                cbGZZT.Enabled = false;
                tbQT.ReadOnly = true;
                tbCD_SSXL.ReadOnly = true;
                tbSFYHYB.ReadOnly = true;
                cbYDLB.Enabled = false;
                cbYHLX.Enabled = false;
                tbYHXM.ReadOnly = true;
                tbYDH.ReadOnly = true;
                cbXS.Enabled = false;
                tbBZ.ReadOnly = true;
                btnOK.Enabled = false;
                tbXYH.ReadOnly = true;
            }
        }

        private bool hasError()
        {
            try
            {
                lbWarn.Text = "";
                lbWarn.ForeColor = Color.Red;
                if (string.IsNullOrEmpty(tbXYH.Text.Trim()))
                {
                    lbWarn.Text = "客户协议不能为空.";
                    return true;
                }
                if (string.IsNullOrEmpty(tbYDH.Text.Trim()))
                {
                    lbWarn.Text = "用户用电号不能为空.";
                    return true;
                }
                var m = Regex.Match(tbYDH.Text.Trim(), @"^[A-Za-z0-9]+$");
                if (!m.Success)
                {
                    lbWarn.Text = "用户用电号只能由数字和英文字母组成.";
                    return true;
                }
                if (string.IsNullOrEmpty(tbBH.Text.Trim()))
                {
                    lbWarn.Text = "表号不能为空.";
                    return true;
                }
                else
                {
                    int len = Encoding.Default.GetByteCount(tbBH.Text.Trim());
                    if (len > 20)
                    {
                        lbWarn.Text = "表号长度不能超过20";
                        return true;
                    }

                }
                var temp =
                    DBManager.Instance.GetEntities<Gg_pd_dyshb_n>(
                        o => o.BH == tbBH.Text && o.G3E_FID != FID && o.EntityState != EntityState.Delete);
                if (temp.Any())
                {
                    lbWarn.Text = "表号已存在.";
                    return true;
                }
                if (string.IsNullOrEmpty(tbYHXM.Text))
                {
                    lbWarn.Text = "用户姓名不能为空.";
                    return true;
                }
                if (string.IsNullOrEmpty(tbHH.Text))
                {
                    lbWarn.Text = "门牌地址不能为空.";
                    return true;
                }
                long m_cbx_fid;
                if (Ft == formType.regionAdd)
                {
                    m_cbx_fid = FID;
                }
                else
                {
                    m_cbx_fid = g_DetailFid;
                }
                // 判断详图的所属单位和所属线路是否一致
                var m_cbx_n = DBManager.Instance.GetEntities<Gg_pd_cbx_n>(o => o.G3E_FID == m_cbx_fid).FirstOrDefault();
                if (m_cbx_n != null)
                {
                    var m_detail_name = FeatureMapping.instance.features[m_cbx_n.G3E_FNO.ToString()];
                    var m_cbx_ssdw = m_cbx_n.CD_SSDW;
                    var m_cbx_ssxl = m_cbx_n.CD_SSXL;
                    if (string.IsNullOrEmpty(tbCD_SSDW.Text))
                    {
                        lbWarn.Text = m_detail_name + "所属单位不能为空,请先填写集抄箱所属单位。";
                        return true;
                    }
                    if (m_cbx_ssdw != tbCD_SSDW.Text)
                    {
                        lbWarn.Text = m_detail_name + "与户表的所属单位不一致。";
                        return true;
                    }
                    if (string.IsNullOrEmpty(tbCD_SSXL.Text))
                    {
                        lbWarn.Text = m_detail_name + "所属线路不能为空,请先填写集抄箱所属线路。";
                        return true;
                    }
                    if (m_cbx_ssxl != tbCD_SSXL.Text)
                    {
                        lbWarn.Text = m_detail_name + "与户表的所属线路不一致。";
                        return true;
                    } 
                }
                else
                {
                    lbWarn.Text = "集抄箱没有自身属性";
                    return true;
                }
                return false;
            }
            catch (NotExistException ex)
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
                    PublicMethod.Instance.AlertDialog(ex.Message);
                }
                return hasError();
            }
            catch (Exception)
            {
                return false;
            }
        }

        private void btnOK_Click(object sender, EventArgs e)
        {
            try
            {
                //判断输入的属性规范
                if (hasError()) return;

                if (Ft == formType.regionAdd)//添加操作
                {
                    //检查户表是否存在
                    var tmp =GetDYSHB_N(); 
                    if (tmp != null)//存在
                    {
                        #region 
                        //如状态是Delete,则改回None
                        if (tmp.EntityState == EntityState.Delete)
                        {
                            tmp.EntityState = EntityState.None;
                            DBManager.Instance.Update(tmp);
                        }
                        //检查从属关系是否存在
                        var v = DBManager.Instance.GetEntity<Gg_jx_shbd_pt>(o => o.G3E_FID==FID);
                        if (v != null)//存在
                        {
                            //如状态是Delete,则改回None
                            if (v.EntityState == EntityState.Delete)
                            {
                                v.EntityState = EntityState.None;
                                DBManager.Instance.Update(v);
                            }
                        }
                        else//不存在则插入
                        {
                            var t = DBManager.Instance.GetEntity<Detailreference_n>(o => o.G3E_FID == FID);
                            Gg_jx_shbd_pt t2 = new Gg_jx_shbd_pt();
                            t2.G3E_DETAILID = t.G3E_DETAILID;
                            t2.G3E_FNO = 160;
                            t2.G3E_CNO = 16004;
                            t2.G3E_FID = tmp.G3E_FID;
                            t2.G3E_ALIGNMENT = 0;
                            t2.G3E_ID = CYZCommonFunc.getid();
                            t2.G3E_CID = 1;
                            t2.LTT_ID = MapConfig.Instance.LTTID;
                            t2.EntityState = EntityState.Insert;
                            DBManager.Instance.Insert(t2);
                        }
                        #endregion
                    }
                    else//不存在
                    {
                        var g3efid = CYZCommonFunc.getid();
                        //插入Gg_pd_dyshb_n表
                        Add_Gg_pd_dyshb_n(g3efid);

                        //增加Gg_jx_shbd_pt表 
                        var sdogid = Add_Gg_jx_shbd_pt(g3efid);

                        //增加Gg_jx_shbd_pt_sdogeom
                        Add_Gg_jx_shbd_pt_sdogeom(sdogid, g3efid);

                        //增加Gg_jx_shbd_lb
                        Add_Gg_jx_shbd_lb(g3efid);

                        //增加Connectivity_n
                        //Add_Connectivity_n(g3efid);
                        g_newFid = g3efid;
                    }
                }
                else//编辑操作
                {
                    //更新表数据
                    UpdateDYSHB();
                }
                AZDZ = tbAZDZ.Text;
                DialogResult = DialogResult.OK;
                Close();
            }
            catch (Exception ee)
            {
                PublicMethod.Instance.ShowMessage(String.Format("操作过程中出现错误:{0}", ee.Message));
            }
        }

        private void btnCancel_Click(object sender, EventArgs e)
        {
            DialogResult = DialogResult.Cancel;
            Close();
        }

        private void Add_Gg_pd_dyshb_n(long g3efid)
        {
            //插入Gg_pd_dyshb_n表
            var newEnt = new Gg_pd_dyshb_n
            {
                G3E_FID = g3efid,
                G3E_ID = CYZCommonFunc.getid(),
                G3E_FNO = 160,
                G3E_CNO = 16001,
                G3E_CID = 1,
                YDH = tbYDH.Text,
                YHXM = tbYHXM.Text,
                BH = tbBH.Text,
                GDH = tbGDH.Text,
                HBLX = tbHBLX.Text,
                DZJL = tbDZJL.Text,
                AZDZ = tbAZDZ.Text,
                CD_SSDW = tbCD_SSDW.Text,
                BXH = tbBXH.Text,
                YDLB = (String) (cbYDLB.SelectedItem),
                YHLX = (String) (cbYHLX.SelectedItem),
                SMZQ = (String) (cbSMZQ.SelectedItem),
                MBH = tbMBH.Text,
                BZ = tbBZ.Text,
                GZZT = (String) (cbGZZT.SelectedItem),
                CD_SSXL = tbCD_SSXL.Text,
                HH = tbHH.Text,
                XS = (String) (cbXS.SelectedItem),
                QT = tbQT.Text,
                SFYHYB = tbSFYHYB.Text,
                //BZ1 = String.Format("{0}@1@{1}", tbYDH.Text, tbBH.Text), //协议号需要配网去填
                KHXYH = tbXYH.Text,
                LTT_ID = MapConfig.Instance.LTTID,
                EntityState = EntityState.Insert
            };
            //newEnt.LTT_DATE;
            //newEnt.LTT_STATUS;
            //newEnt.LTT_TID;
            //newEnt.DWFX_SFSDY;
            //newEnt.CD_SSBDZ;
            if (false == DBManager.Instance.Insert(newEnt))
            {
                throw new Exception("Add_Gg_pd_dyshb_n: 插入失败！");
            }
        }
        private long Add_Gg_jx_shbd_pt(long g3efid)
        {
            //增加Gg_jx_shbd_pt表
            var detail = DBManager.Instance.GetEntity<Detailreference_n>(o => o.G3E_FID == FID);
            var t2 = new Gg_jx_shbd_pt
            {
                G3E_DETAILID = detail.G3E_DETAILID,
                G3E_FNO = 160,
                G3E_CNO = 16004,
                G3E_FID = g3efid,
                G3E_ALIGNMENT = 0,
                G3E_ID = CYZCommonFunc.getid(),
                G3E_CID = 1,
                LTT_ID = MapConfig.Instance.LTTID,
                EntityState = EntityState.Insert
            };

            if (false == DBManager.Instance.Insert(t2))
            {
                throw new Exception("Add_Gg_jx_shbd_pt: 插入失败！");
            }
            if (detail.DETAIL_USERNAME == null)
            {
                detail.DETAIL_USERNAME = detail.G3E_FID.ToString();
                DBManager.Instance.Update(detail);
            }
            return t2.G3E_ID;
        }
        private void Add_Gg_jx_shbd_pt_sdogeom(long sdogid, long g3efid)
        {
            var shbsdo = new Gg_jx_shbd_pt_sdogeom
            {
                G3E_FID = g3efid,
                G3E_FNO = 160,
                G3E_ID = CYZCommonFunc.getid(),
                SDO_GID = sdogid,
                G3E_CNO = 16004,
                G3E_CID = 1,
                LTT_ID = MapConfig.Instance.LTTID,
                EntityState = EntityState.Insert
            };
            double x, y;
            // 获取坐标
            if (shbPositon != Point3d.Origin)
            {
                x = shbPositon.X;
                y = shbPositon.Y;
            }
            else
            {
                x = (MapConfig.Instance.ProjectionMaxX + MapConfig.Instance.ProjectionMinX) / 2;
                y = (MapConfig.Instance.ProjectionMaxY + MapConfig.Instance.ProjectionMinY) / 2;
            }
            var mpValue = new Multipoint();
            mpValue.Points.Add(new Point(new[] { x, y, 0 }));
            mpValue.Points.Add(new Point(new[] { Math.Cos(0.0), Math.Sin(0.0), 0 }));
            shbsdo.G3E_GEOMETRY = mpValue;

            if (false == DBManager.Instance.Insert(shbsdo))
            {
                throw new Exception("Add_Gg_jx_shbd_pt_sdogeom: 插入失败！");
            }
            #region 
            //shbsdo.SDO_ESEQ
            //SDO_ETYPE
            //SDO_SEQ
            //GDO_ATTRIBUTES
            //GDO_NORMAL1
            //GDO_NORMAL2
            //GDO_NORMAL3
            //GDO_RADIUS
            //SDO_ORIENTATION
            //SDO_X1
            //SDO_Y1
            //SDO_X2
            //SDO_Y2
            //SDO_X3
            //SDO_Y3
            //SDO_X4
            //SDO_Y4
            //G3E_ID
            //G3E_FNO
            //G3E_FID
            //LTT_STATUS
            //LTT_DATE
            //LTT_TID
            #endregion
        }
        private void Add_Gg_jx_shbd_lb(long g3efid)
        {      
            //增加Gg_jx_shbd_lb
            var t = DBManager.Instance.GetEntity<Detailreference_n>(o => o.G3E_FID == FID);
            var shblb = new Gg_jx_shbd_lb
            {
                G3E_ALIGNMENT = 5,
                G3E_ID = CYZCommonFunc.getid(),
                G3E_FNO = 160,
                G3E_CNO = 16005,
                G3E_FID = g3efid,
                G3E_CID = 1,
                G3E_DETAILID = t.G3E_DETAILID,
                LTT_ID = MapConfig.Instance.LTTID,
                EntityState = EntityState.Insert
            };
            if (false == DBManager.Instance.Insert(shblb))
            {
                throw new Exception("Add_Gg_jx_shbd_lb: 插入失败！");
            }
            #region 
    //GDO_GID
            //G3E_GEOMETRY
            //CD_SSDW
            //LTT_TID
            //LTT_STATUS
            //LTT_DATE
    #endregion
        }
        private void Add_Connectivity_n(long g3efid)
        {
            // 插入Connectivity_n
            var shbconn = new Connectivity_n
            {
                G3E_ID = CYZCommonFunc.getid(),
                G3E_FNO = 160,
                G3E_CNO = 31,
                G3E_FID = g3efid,
                G3E_CID = 1,
                NODE1_ID = 0,
                NODE2_ID = 0,
                LTT_ID = MapConfig.Instance.LTTID,
                EntityState = EntityState.Add_Nal_Nal
            };
            if (false == DBManager.Instance.Insert(shbconn))
            {
                throw new Exception("Add_Connectivity_n: 插入失败！");
            }
        }

        private void UpdateDYSHB()
        {
            //更新表数据
            var ent = DBManager.Instance.GetEntity<Gg_pd_dyshb_n>(o => o.G3E_FID == FID);
            if (ent != null)
            {
                ent.YDH = tbYDH.Text.Trim();
                ent.YHXM = tbYHXM.Text.Trim();
                ent.BH = tbBH.Text.Trim();
                ent.GDH = tbGDH.Text.Trim();
                ent.HBLX = tbHBLX.Text.Trim();
                ent.DZJL = tbDZJL.Text.Trim();
                ent.AZDZ = tbAZDZ.Text.Trim();
                ent.CD_SSDW = tbCD_SSDW.Text.Trim();//(String)(cbCD_SSDW.SelectedItem);
                ent.BXH = tbBXH.Text.Trim();
                ent.YDLB = (String)(cbYDLB.SelectedItem);
                ent.YHLX = (String)(cbYHLX.SelectedItem);
                ent.SMZQ = (String)(cbSMZQ.SelectedItem);
                ent.MBH = tbMBH.Text.Trim();
                ent.BZ = tbBZ.Text.Trim();
                ent.GZZT = (String)(cbGZZT.SelectedItem);
                ent.CD_SSXL = tbCD_SSXL.Text.Trim();
                ent.HH = tbHH.Text.Trim();
                ent.XS = (String)(cbXS.SelectedItem);
                ent.QT = tbQT.Text.Trim();
                ent.SFYHYB = tbSFYHYB.Text.Trim();
                //ent.BZ1 = String.Format("{0}@1@{1}", tbYDH.Text, tbBH.Text);
                ent.KHXYH = tbXYH.Text.Trim();
                ent.LTT_ID = MapConfig.Instance.LTTID;
                //ent.LTT_DATE;
                //ent.LTT_STATUS;
                //ent.LTT_TID;
                //ent.DWFX_SFSDY;
                //ent.CD_SSBDZ;
                if (ent.EntityState != EntityState.Insert)//如果之前不是insert
                {
                    ent.EntityState = EntityState.Update;//修改成update
                }
                DBManager.Instance.Update(ent);
            }
            else//发现不存在
            {
                throw new Exception("Gg_pd_dyshb_n没有低压散户表数据。\n");
            }
        }

        private Gg_pd_dyshb_n GetDYSHB_N()
        {
            try
            {
                return DBManager.Instance.GetEntity<Gg_pd_dyshb_n>(FID);
            }
            catch (NotExistException ex)
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
                    PublicMethod.Instance.AlertDialog(ex.Message);
                }
                return null;
            }
            catch
            {
                return null;
            }
        }
        private IEnumerable<Gg_pd_dyshb_n> GetDYSHBNS()
        {
            try
            {
                return DBManager.Instance.GetEntities<Gg_pd_dyshb_n>();
            }
            catch
            {
                return null;
            }
        }
        private IEnumerable<Cd_xs> GetDYSHBN_XS()
        {
            try
            {
                return CDDBManager.Instance.GetEntities<Cd_xs>();
            }
            catch
            {
                return null;
            }
        }

        private IEnumerable<Cd_ydlb> GetDYSHBN_YDLB()
        {
            try
            {
                return CDDBManager.Instance.GetEntities<Cd_ydlb>();
            }
            catch
            {
                return null;
            }
        }

        private IEnumerable<Cd_yhlx> GetDYSHBN_YHLX()
        {
            try
            {
                return CDDBManager.Instance.GetEntities<Cd_yhlx>();
            }
            catch
            {
                return null;
            }
        }

        private IEnumerable<Cd_gzzt> GetDYSHBN_GZZT()
        {
            try
            {
                return CDDBManager.Instance.GetEntities<Cd_gzzt>();
            }
            catch
            {
                return null;
            }
        }

        private IEnumerable<Cd_smzq> GetDYSHBN_SMZQ()
        {
            try
            {
                return CDDBManager.Instance.GetEntities<Cd_smzq>();
            }
            catch
            {
                return null;
            }
        }

        private void SHBEditer_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyValue == 27) // 用户按Esc
                btnCancel_Click(sender, e);
            else if (e.KeyValue == 13) // 用户按Enter
                btnOK_Click(sender, e);
        }

        private void tbYDH_Validated(object sender, EventArgs e)
        {
            hasError();
        }

        private void tbBH_Validated(object sender, EventArgs e)
        {
            hasError();
        }

        private void tbBXH_Validated(object sender, EventArgs e)
        {
            hasError();
        }

        private void tbHH_Validated(object sender, EventArgs e)
        {
            hasError();
        }

        private void tbXYH_TextChanged(object sender, EventArgs e)
        {

        }


        //private IEnumerable<Cd_yhlx> GetDYSHBN_YHLX()
        //{
        //    try
        //    {
        //        return DBManager.Instance.GetEntities<Cd_yhlx>();
        //    }
        //    catch
        //    {
        //        return null;
        //    }
        //}
    }
}
