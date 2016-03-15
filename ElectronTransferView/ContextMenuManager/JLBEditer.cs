using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text.RegularExpressions;
using System.Windows.Forms;
using ElectronTransferDal.Common;
using ElectronTransferDal.Query;
using ElectronTransferModel.Base;
using ElectronTransferModel.V9_4;
using ElectronTransferDal.Cad;
using ElectronTransferModel;
using ElectronTransferView.FunctionManager;
using ElectronTransferModel.Geo;
using ElectronTransferModel.Config;
using Autodesk.AutoCAD.Geometry;
using ElectronTransferDal.Common.Exceptions;
using ElectronTransferFramework;

namespace ElectronTransferView.ContextMenuManager
{
    public partial class JLBEditer : Form
    {
        /// <summary>
        /// 新增或修改状态
        /// </summary>
        private formType Ft { get; set; }
        /// <summary>
        /// 如果FT状态是编辑则是户表G3E_FID,如果是添加则是集抄箱G3E_FID
        /// </summary>
        private long FID { get; set; }
        /// <summary>
        /// 抄表箱G3E_FID
        /// </summary>
        private long g_DetailFid { get; set; }
        /// <summary>
        /// 新增户表的经纬坐标
        /// </summary>
        private Point3d jlbPositon { get; set; }
        /// <summary>
        /// 新增户表的G3E_FID
        /// </summary>
        public long g_newFid { get; set; }
        /// <summary>
        /// 户表的设备类型,用于JLBMap显示控件图标
        /// </summary>
        public string SBLX { get; set; }
        /// <summary>
        /// 是否锁定
        /// </summary>
        private bool IsLock { set; get; }
        public JLBEditer()
        {
            InitializeComponent();
        }
        public JLBEditer(formType ft, long shbfid,long detailfid, bool isLock)
        {
            Ft = ft;
            FID = shbfid;
            g_DetailFid = detailfid;
            IsLock = isLock;
            jlbPositon = Point3d.Origin;
            InitializeComponent();
        }
        public JLBEditer(formType ft, long fid, double[] pt, bool isLock)
        {
            Ft = ft;
            FID = fid;
            g_DetailFid = fid;
            IsLock = isLock;
            jlbPositon =  new Point3d(pt[1], pt[0], 0);
            InitializeComponent();
        }

        private void JLBEditer_Load(object sender, EventArgs e)
        {
            try
            {
                Text = "编辑计量表";
                // 初始化Combox控件
                InitCb();
                //  如果是修改状态,把原始值赋到当前面板
                if (Ft == formType.regionEdit)
                {
                    //lbWarn.Text = G3e_Fid.ToString();//测试行
                    var jlbcomm = DBManager.Instance.GetEntities<Common_n>(o => o.G3E_FID == FID).FirstOrDefault();
                    if (jlbcomm == null) 
                    {
                        throw new Exception(String.Format("JLBEditer_Load:Common_n没有{0}数据", FID));
                    }
                    var jlbn = DBManager.Instance.GetEntities<Gg_pd_jlb_n>(o => o.G3E_FID == FID).FirstOrDefault();
                    if (jlbn == null)
                    {
                        throw new Exception(String.Format("JLBEditer_Load:Gg_pd_jlb_n没有{0}数据", FID));
                    }
                    tbBZ.Text           = jlbcomm.BZ;
                    tbJGDBH.Text   = jlbcomm.JGDBH;
                    tbSBMC.Text     = jlbcomm.SBMC;
                    tbHM.Text          = jlbn.HM;
                    tbBH.Text           = jlbn.BH;
                    tbYHH.Text        = jlbn.YHH;
                    tbXYH.Text        = jlbn.BZ;
                    if (jlbcomm.TYRQ != null) tbTYRQ.Value = (DateTime)jlbcomm.TYRQ;
                    cbSBLX.SelectedIndex = (-1 != cbSBLX.FindStringExact(jlbn.CD_SBLX, 0))
                        ? cbSBLX.FindStringExact(jlbn.CD_SBLX, 0)
                        : 0;
                    cbSMZQ.SelectedIndex = cbSMZQ.FindStringExact(jlbcomm.CD_SMZQ, 0);
                }
                // 工单校验
                VerifyFid();
            }
            catch (NotExistException ex)
            {
                PublicMethod.Instance.NotExistTable(ex);
            }
            catch (Exception ex)
            {
                LogManager.Instance.Error(ex);
            }
        }
        /// <summary>
        /// 初始化Combox控件
        /// </summary>
        private void InitCb()
        {
            try
            {
                // 得到变压器的公共属性
                var fcomm = DBManager.Instance.GetEntities<Common_n>(o => o.G3E_FID == g_DetailFid).FirstOrDefault();
                //初始化电压等级(kV)
                var dydj = CDDBManager.Instance.GetEntities<Cd_dydj>().Select(o => o.NAME).ToList();
                dydj.Add("");
                cbDYDJ.DataSource = dydj;
                cbDYDJ.SelectedIndex = fcomm != null && (-1 != cbDYDJ.FindStringExact(fcomm.CD_DYDJ, 0))
                    ? cbDYDJ.FindStringExact(fcomm.CD_DYDJ, 0)
                    : 0;
                //初始化生命周期
                var smzq = CDDBManager.Instance.GetEntities<Cd_smzq>().Select(o => o.NAME).ToList();
                smzq.Add("");
                cbSMZQ.DataSource = smzq;
                if (fcomm != null)
                {
                    cbSMZQ.SelectedIndex = cbSMZQ.FindStringExact(fcomm.CD_SMZQ, 0);
                    //初始化所属单位
                    var ssdw = CDDBManager.Instance.GetEntities<Cd_ssxl>().Select(o => o.CD_SSDW).Distinct().ToList();
                    ssdw.Add("");
                    cbSSDW.DataSource = ssdw;
                    cbSSDW.SelectedIndex = cbSSDW.FindStringExact(fcomm.CD_SSDW, 0);
                    //初始化所属供电所
                    var ssgds =
                        CDDBManager.Instance.GetEntities<Cd_bs>()
                            .Where(o => o.SSDW == cbSSDW.Text)
                            .Select(o => o.NAME)
                            .ToList();
                    ssgds.Add("");
                    cbSSGDS.DataSource = ssgds;
                    //cbSSGDS.Text = fcomm.GNWZ_SSGDS;
                    //初始化维护班所
                    cbWhbs.DataSource = ssgds;
                    cbWhbs.Text = fcomm.WHBS;
                    //初始化所属馈线
                    List<string> ssxl;
                    if (fcomm.CD_SSBDZ == null)
                        ssxl = CDDBManager.Instance.GetEntities<Cd_ssxl>().Select(o => o.NAME).ToList();
                    else
                        ssxl = CDDBManager.Instance.GetEntities<Cd_ssxl>().Where(o => o.SSBDZ == fcomm.CD_SSBDZ).Select(o => o.NAME).ToList();
                    ssxl.Add("");
                    cbSSXL.DataSource = ssxl;
                    cbSSXL.Text = fcomm.CD_SSXL;
                    // 初始化出厂日期
                    SetTYRQ(fcomm);
                }
                // 初始化设备类型;
                var sblx = CDDBManager.Instance.GetEntities<Cd_jlblx>().Select(o => o.NAME).ToList();
                cbSBLX.DataSource = sblx;
            }
            catch (NotExistException ex)
            {
                PublicMethod.Instance.NotExistTable(ex);
            }
            catch (Exception ex)
            {
                LogManager.Instance.Error(ex);
            }
        }
        /// <summary>
        /// 初始化出厂日期
        /// </summary>
        /// <param name="common"></param>
        private void SetTYRQ(Common_n common)
        {
            try
            {
                if (common.TYRQ != null) tbTYRQ.Value = (DateTime)common.TYRQ;
            }catch
            {
                // 初始化出厂日期
                if (String.IsNullOrEmpty(tbTYRQ.Text))
                    tbTYRQ.Value = DateTime.Now;
            }
        }
        /// <summary>
        /// 验证当前集抄箱是否被工单锁定
        /// </summary>
        private void VerifyFid()
        {
            // 校验FID是否被锁定
            if (Ft == formType.regionAdd) return;
            if (!IsLock)
            {
                Text = "查看计量表";
                tbYHH.Enabled = false;
                tbHM.Enabled = false;
                tbBH.Enabled = false;
                cbDYDJ.Enabled = false;
                cbSMZQ.Enabled = false;
                tbJGDBH.Enabled = false;
                tbSBMC.Enabled = false;
                cbSSDW.Enabled = false;
                cbWhbs.Enabled = false;
                cbSSGDS.Enabled = false;
                cbSSXL.Enabled = false;
                cbSBLX.Enabled = false;
                tbTYRQ.Enabled = false;
                tbBZ.Enabled = false;
                btok.Enabled = false;
            }
        }
        /// <summary>
        /// 增加Gg_jx_jlb_pt记录
        /// </summary>
        /// <param name="fid">新户表G3E_FID</param>
        /// <returns>新户表G3E_ID</returns>
        private long Add_Gg_jx_jlb_pt(long fid)
        {
            var detail = DBManager.Instance.GetEntities<Detailreference_n>(o => o.G3E_FID == FID).FirstOrDefault();
            if (detail == null)
            {
                throw new Exception("Add_Gg_jx_jlb_pt error.增加计量表时找不到详表数据,请联系数据管理人员。");
            }
            else { /*是否需要插数据*/ }
            // 插入Gg_jx_jlb_pt
            var jxjlb = new Gg_jx_jlb_pt();
            jxjlb.G3E_GEOMETRY = "0";
            jxjlb.G3E_ID = CYZCommonFunc.getid();
            jxjlb.G3E_FNO = 41;
            jxjlb.G3E_CNO = 4102;
            jxjlb.G3E_FID = fid;
            jxjlb.G3E_CID = 1;
            jxjlb.G3E_DETAILID = detail.G3E_DETAILID;
            jxjlb.LTT_ID = MapConfig.Instance.LTTID;
            jxjlb.EntityState = EntityState.Insert;
            if (false == DBManager.Instance.Insert(jxjlb))
            {
                throw new Exception("Add_Gg_jx_jlb_pt: 插入失败！");
            }
            if (detail.DETAIL_USERNAME == null)
            {
                detail.DETAIL_USERNAME = detail.G3E_FID.ToString();
                DBManager.Instance.Update(detail);
            }
            return jxjlb.G3E_ID;
            #region 未填充字段
            //jxjlb.GDO_GID 
            //jxjlb.SFQTKX
            //jxjlb.G3E_GEOMETRY
            //jxjlb.LTT_TID
            //jxjlb.LTT_STATUS 
            //jxjlb.LTT_DATE 
            #endregion
        }
        /// <summary>
        /// 增加Gg_pd_jlb_n记录
        /// </summary>
        /// <param name="fid">新户表G3E_FID</param>
        private void Add_Gg_pd_jlb_n(long fid)
        {
            // 插入Gg_pd_jlb_n
            var pdjlb = new Gg_pd_jlb_n();
            pdjlb.G3E_ID = CYZCommonFunc.getid();
            pdjlb.G3E_FNO = 41;
            pdjlb.G3E_CNO = 4101;
            pdjlb.G3E_FID = fid;
            pdjlb.G3E_CID = 1;
            pdjlb.YHH = tbYHH.Text;
            pdjlb.BZ = tbXYH.Text;
            pdjlb.HM = tbHM.Text;
            pdjlb.BH = tbBH.Text;
            pdjlb.CD_SBLX = cbSBLX.Text;
            //pdjlb.BZ = tbYHH.Text + "@1@" + tbBH.Text;
            pdjlb.LTT_ID = MapConfig.Instance.LTTID;
            pdjlb.EntityState = EntityState.Insert;
            if (false == DBManager.Instance.Insert(pdjlb))
            {
                throw new Exception("Add_Gg_pd_jlb_n: 插入失败！");
            }
            #region 未填充字段
            //LTT_STATUS
            //LTT_DATE
            //LTT_TID
            #endregion
        }
        /// <summary>
        /// 增加Common_n记录
        /// </summary>
        /// <param name="fid">新户表G3E_FID</param>
        private void Add_Common_n(long fid)
        {
            // 插入Common_n
            var jlbcomm = new Common_n();
            jlbcomm.G3E_ID = CYZCommonFunc.getid();
            jlbcomm.G3E_FNO = 41;
            jlbcomm.G3E_CNO = 30;
            jlbcomm.G3E_FID = fid;
            jlbcomm.G3E_CID = 1;
            jlbcomm.CD_SSDW = cbSSDW.Text;
            jlbcomm.SBMC = tbSBMC.Text;
            jlbcomm.CD_SMZQ = cbSMZQ.Text;
            jlbcomm.CD_DYDJ = cbDYDJ.Text;
            jlbcomm.CD_SSXL = cbSSXL.Text;
            jlbcomm.WHBS = cbWhbs.Text;
            //jlbcomm.GNWZ_SSGDS = cbSSGDS.Text;
            jlbcomm.BZ = tbBZ.Text;
            jlbcomm.TYRQ = tbTYRQ.Value;
            jlbcomm.JGDBH = tbJGDBH.Text;
            jlbcomm.LTT_ID = MapConfig.Instance.LTTID;
            jlbcomm.EntityState = EntityState.Insert;
            if (false == DBManager.Instance.Insert(jlbcomm))
            {
                throw new Exception("Add_Common_n: 插入失败！");
            }
            #region 未填充字段
            //CCRQ
            //CD_ZZCJ
            //YXBH
            //CD_CQSX
            //EDDL
            //EDDY
            //CD_XHGE
            //CD_GSDY
            //CD_SSBDZ
            //OWNER1_ID
            //OWNER2_ID
            //LTT_STATUS
            //LTT_DATE
            //LTT_TID
            //DWFX_DQFH
            //DWFX_SYPJFH
            //DWFX_ZRL
            //WHBS
            //GDZCKH
            //SGDW
            //QT
            //HH
            //SCM_DID
            //CD_SBPJ
            //SBPJSM
            //BZ1
            //BZ2
            //BZ3
            //DQTZ
            #endregion
        }
        /// <summary>
        /// 增加Connectivity_n记录
        /// </summary>
        /// <param name="fid">新户表G3E_FID</param>
        private void Add_Connectivity_n(long fid)
        {
            // 插入Connectivity_n
            var jlbconn = new Connectivity_n();
            jlbconn.G3E_ID = CYZCommonFunc.getid();
            jlbconn.G3E_FNO = 41;
            jlbconn.G3E_CNO = 31;
            jlbconn.G3E_FID = fid;
            jlbconn.NODE1_ID = 0;
            jlbconn.NODE2_ID = 0;
            jlbconn.G3E_CID = 1;
            jlbconn.LTT_ID = MapConfig.Instance.LTTID;
            jlbconn.EntityState = EntityState.Add_Nal_Nal;
            if (false == DBManager.Instance.Insert(jlbconn))
            {
                throw new Exception("Add_Connectivity_n: 插入失败！");
            }
            #region 未填充字段
            //jlbconn.CD_DQZT
            //jlbconn.CD_XW
            //jlbconn.CD_SFDD
            //jlbconn.LTT_STATUS
            //jlbconn.LTT_DATE
            //jlbconn.LTT_TID
            //jlbconn.SCM_DID
            #endregion
        }
        /// <summary>
        /// 增加Gg_pd_gnwzmc_n记录
        /// </summary>
        /// <param name="fid">新户表G3E_FID</param>
        private void Add_Gg_pd_gnwzmc_n(long fid)
        {
            // 插入GG_Gg_pd_gnwzmc_n
            var gnwz = new Gg_pd_gnwzmc_n();
            gnwz.G3E_ID = CYZCommonFunc.getid();
            gnwz.G3E_FNO = 41;
            gnwz.G3E_CNO = 50;
            gnwz.G3E_FID = fid;
            gnwz.G3E_CID = 1;
            gnwz.LTT_ID = MapConfig.Instance.LTTID;
            gnwz.EntityState = EntityState.Insert;
            if (false == DBManager.Instance.Insert(gnwz))
            {
                throw new Exception("Add_Gg_pd_gnwzmc_n: 插入失败！");
            }
            #region 未填充字段
            //gnwz.LX
            //gnwz.BM
            //gnwz.MC
            //gnwz.BZ
            //gnwz.LTT_STATUS
            //gnwz.LTT_DATE
            //gnwz.LTT_TID
            //gnwz.GNWZ_SSFL
            //gnwz.GNWZ_FLZGDYDJ
            //gnwz.GNWZ_QY
            //gnwz.GNWZ_ZHORJG
            //gnwz.GNWZ_XLFL
            //gnwz.BM_SECOND
            //gnwz.GNWZ_FL
            //gnwz.GNWZ_FL2
            //gnwz.GNWZ_SSTJ
            //gnwz.GNWZ_SSKGG
            //gnwz.GNWZ_DYGFL
            //gnwz.GNWZ_KGGLX
            //gnwz.GNWZ_SSDF
            //gnwz.DYDJ
            //gnwz.SJGNWZBM
            //gnwz.GNWZLBBM
            //gnwz.TCRQ
            //gnwz.TYRQB
            //gnwz.GNWZ_SSGT
            //gnwz.GNWZ_BYQRL
            //gnwz.GNWZ_YGGL
            //gnwz.GNWZ_DLYB
            //gnwz.GNWZ_CXD
            //gnwz.GNWZ_DYDXTH
            //gnwz.GNWZ_SFDYRDQ
            //gnwz.GNWZ_SFKQXL
            //gnwz.YXBH
            //gnwz.GNWZ_CLLB
            //gnwz.SFDCJG
            //gnwz.SFZG
            //gnwz.GNWZ_DFLX
            //gnwz.GNWZ_YFZNMS
            //gnwz.GNWZ_SSTQHTJ
            #endregion
        }
        /// <summary>
        /// 增加Gg_jx_jlb_pt_sdogeom记录
        /// </summary>
        /// <param name="g3efid">新户表G3E_FID</param>
        /// <param name="g3eid">新户表G3E_ID</param>
        private void Add_Gg_jx_jlb_pt_sdogeom(long g3efid, long g3eid)
        {
            // 插入Gg_jx_jlb_pt_sdogeom
            var jlbsdo = new Gg_jx_jlb_pt_sdogeom();
            jlbsdo.G3E_ID = CYZCommonFunc.getid();
            jlbsdo.G3E_FID = g3efid;
            jlbsdo.G3E_CID = 1;
            jlbsdo.G3E_FNO = 41;
            jlbsdo.G3E_CNO = 4102;
            jlbsdo.SDO_GID = g3eid;
            jlbsdo.LTT_ID = MapConfig.Instance.LTTID;
            jlbsdo.EntityState = EntityState.Insert;
            double x;
            double y;
            // 获取坐标
            if (jlbPositon != Point3d.Origin)
            {
                x = jlbPositon.X;
                y = jlbPositon.Y;
            }
            else
            {
                x = (MapConfig.Instance.ProjectionMaxX + MapConfig.Instance.ProjectionMinX) / 2;
                y = (MapConfig.Instance.ProjectionMaxY + MapConfig.Instance.ProjectionMinY) / 2;
            }

            var mpValue = new Multipoint();
            mpValue.Points.Add(new ElectronTransferModel.Geo.Point(new[] { x, y, 0 }));
            mpValue.Points.Add(new ElectronTransferModel.Geo.Point(new[] { Math.Cos(0.0), Math.Sin(0.0), 0 }));
            jlbsdo.G3E_GEOMETRY = mpValue;

            if (false == DBManager.Instance.Insert(jlbsdo))
            {
                throw new Exception("Add_Gg_jx_jlb_pt_sdogeom: 插入失败！");
            }
            #region 
            //jlbsdo.SDO_ESEQ
            //jlbsdo.SDO_ETYPE
            //jlbsdo.SDO_SEQ
            //jlbsdo.GDO_ATTRIBUTES
            //jlbsdo.GDO_NORMAL1
            //jlbsdo.GDO_NORMAL2
            //jlbsdo.GDO_NORMAL3
            //jlbsdo.GDO_RADIUS
            //jlbsdo.SDO_ORIENTATION
            //jlbsdo.SDO_X1
            //jlbsdo.SDO_Y1
            //jlbsdo.SDO_X2
            //jlbsdo.SDO_Y2
            //jlbsdo.SDO_X3
            //jlbsdo.SDO_Y3
            //jlbsdo.SDO_X4
            //jlbsdo.SDO_Y4
            //jlbsdo.LTT_STATUS
            //jlbsdo.LTT_DATE
            //jlbsdo.LTT_TID
            #endregion
        }
        /// <summary>
        /// 将面板内容更新到户表里面
        /// </summary>
        private void UpdateJLB()
        {
            // 更新Gg_pd_jlb_n、Common_n 即可
            var pdjlb = DBManager.Instance.GetEntities<Gg_pd_jlb_n>(o => o.G3E_FID == FID & o.EntityState != EntityState.Delete).FirstOrDefault();
            var jlbcomm = DBManager.Instance.GetEntities<Common_n>(o => o.G3E_FID == FID & o.EntityState != EntityState.Delete).FirstOrDefault();
            if (pdjlb == null || jlbcomm == null) throw new Exception("UpdateJLB error.数据源找不到数据。");
            pdjlb.YHH = tbYHH.Text.Trim();
            pdjlb.BZ = tbXYH.Text.Trim();
            pdjlb.HM = tbHM.Text.Trim();
            pdjlb.CD_SBLX = cbSBLX.Text.Trim();
            //pdjlb.BZ = tbBZ.Text;
            pdjlb.BH = tbBH.Text.Trim();
            jlbcomm.CD_SSDW = cbSSDW.Text.Trim();
            jlbcomm.SBMC = tbSBMC.Text.Trim();
            jlbcomm.CD_SMZQ = cbSMZQ.Text.Trim();
            jlbcomm.CD_DYDJ = cbDYDJ.Text.Trim();
            jlbcomm.CD_SSXL = cbSSXL.Text.Trim();
            jlbcomm.WHBS = cbWhbs.Text.Trim();
            //jlbcomm.GNWZ_SSGDS = cbSSGDS.Text.Trim();
            jlbcomm.BZ = tbBZ.Text.Trim();
            jlbcomm.TYRQ = tbTYRQ.Value;
            jlbcomm.JGDBH = tbJGDBH.Text.Trim();
            if (pdjlb.EntityState != EntityState.Insert) pdjlb.EntityState = EntityState.Update;
            if (jlbcomm.EntityState != EntityState.Insert) jlbcomm.EntityState = EntityState.Update;
            DBManager.Instance.Update(pdjlb);
            DBManager.Instance.Update(jlbcomm);
        }
        /// <summary>
        /// 判断用户在窗口的输入是否符合规范
        /// </summary>
        /// <returns>返回true代表有错误</returns>
        private bool hasError()
        {
            lbWarn.Text = "";
            lbWarn.ForeColor = Color.Red;
            if (string.IsNullOrEmpty(tbYHH.Text))
            {
                lbWarn.Text = "用户号不能为空.";
                return true;
            }
            var m = Regex.Match(tbYHH.Text, @"^[A-Za-z0-9]+$");
            if (!m.Success)
            {
                lbWarn.Text = "用户号只能由数字和英文字母组成.";
                return true;
            }
            //try
            //{
            //    var temp =
            //        DBManager.Instance.GetEntities<Gg_pd_jlb_n>(
            //            o => o.YHH == tbYHH.Text && o.G3E_FID != FID && o.EntityState != EntityState.Delete);
            //    if (temp.Any())
            //    {
            //        lbWarn.Text = "用户号已存在.";
            //        return true;
            //    }
            //}
            //catch (NotExistException ex)
            //{
            //    PublicMethod.Instance.NotExistTable(ex);
            //    return hasError();
            //}
            if (string.IsNullOrEmpty(tbHM.Text))
            {
                lbWarn.Text = "户名不能为空.";
                return true;
            }

            if (string.IsNullOrEmpty(tbBH.Text))
            {
                lbWarn.Text = "表号不能为空.";
                return true;
            }
            try
            {
                var temp =
                    DBManager.Instance.GetEntities<Gg_pd_jlb_n>(o => o.BH == tbBH.Text && o.G3E_FID != FID && o.EntityState != EntityState.Delete);
                if (temp.Any())
                {
                    lbWarn.Text = "表号已存在.";
                    return true;
                }
            }
            catch (NotExistException ex)
            {
                PublicMethod.Instance.NotExistTable(ex);
                return hasError();
            }
            m = Regex.Match(tbBH.Text, @"^[A-Za-z0-9]+$");
            if (!m.Success)
            {
                lbWarn.Text = "用户号只能由数字和英文字母组成.";
                return true;
            }

            if (string.IsNullOrEmpty(cbDYDJ.Text))
            {
                lbWarn.Text = "电压等级(kV)不能为空.";
                return true;
            }
            if (string.IsNullOrEmpty(cbSMZQ.Text))
            {
                lbWarn.Text = "生命周期不能为空.";
                return true;
            }
            if (string.IsNullOrEmpty(tbSBMC.Text))
            {
                lbWarn.Text = "功能位置名称不能为空.";
                return true;
            }
            //if (string.IsNullOrEmpty(tbBZ.Text))
            //{
            //    lbWarn.Text = "备注不能为空.";
            //    return true;
            //}
            if (string.IsNullOrEmpty(cbSSDW.Text))
            {
                lbWarn.Text = "所属单位不能为空.";
                return true;
            }
            if (string.IsNullOrEmpty(cbWhbs.Text))
            {
                lbWarn.Text = "维护班所不能为空";
                return true;
            }
            if (string.IsNullOrEmpty(cbSSGDS.Text))
            {
                lbWarn.Text = "所属供电所不能为空.";
                return true;
            }
            if (string.IsNullOrEmpty(cbSSXL.Text))
            {
                lbWarn.Text = "受电馈线不能为空.";
                return true;
            }
            if (string.IsNullOrEmpty(cbSBLX.Text))
            {
                lbWarn.Text = "设备类型不能为空.";
                return true;
            }
            //if (string.IsNullOrEmpty(tbTYRQ.Text))
            //{
            //    lbWarn.Text = "投运日期不能为空.";
            //    return true;
            //}
            //if (string.IsNullOrEmpty(tbJGDBH.Text))
            //{
            //    lbWarn.Text = "竣工单编号不能为空.";
            //    return true;
            //}
            //try
            //{
            //    DateTime.Parse(tbTYRQ.Value);
            //}
            //catch (FormatException)
            //{
            //    lbWarn.Text = "投运日期格式错误.例:2000-01-01";
            //    return true;
            //}
            // 得到集抄箱G3E_FID和户表G3E_FID
            //long m_detail_fid;
            //if (Ft == formType.regionAdd)
            //{
            //    m_detail_fid = FID;
            //}
            //else
            //{
            //    m_detail_fid = g_DetailFid;
            //}
            var m_detail_fid = g_DetailFid;
            // 判断详图的所属单位和所属线路是否一致
            var m_detail_common = DBManager.Instance.GetEntities<Common_n>(o => o.G3E_FID == m_detail_fid).FirstOrDefault();
            if (m_detail_common != null)
            {
                var m_detail_name = FeatureMapping.instance.features[m_detail_common.G3E_FNO.ToString()];
                var m_cbx_ssdw = m_detail_common.CD_SSDW;
                var m_cbx_ssxl = m_detail_common.CD_SSXL;
                if (m_cbx_ssdw != cbSSDW.Text)
                {
                    lbWarn.Text = m_detail_name + "与沿布图设备的所属单位不一致。";
                    return true;
                }
                if (m_cbx_ssxl != cbSSXL.Text)
                {
                    lbWarn.Text = m_detail_name + "与沿布图设备的所属线路不一致。";
                    return true;
                }
            }
            else
            {
                lbWarn.Text = "详图没有公共属性";
                return true;
            }
            return false;
        }

        private void btok_Click(object sender, EventArgs e)
        {
            try
            {
                //判断输入的属性规范
                if (hasError()) return;

                if (Ft == formType.regionAdd)
                {
                    // 得到新户表的G3E_FID
                    long g3efid = CYZCommonFunc.getid();
                    // 插入Gg_jx_jlb_pt
                    long g3eid = Add_Gg_jx_jlb_pt(g3efid);
                    // 插入Gg_pd_jlb_n
                    Add_Gg_pd_jlb_n(g3efid);
                    // 插入Common_n
                    Add_Common_n(g3efid);
                    // 插入Gg_pd_gnwzmc_n
                    Add_Gg_pd_gnwzmc_n(g3efid);
                    // 插入Gg_jx_jlb_pt_sdogeom
                    Add_Gg_jx_jlb_pt_sdogeom(g3efid, g3eid);
                    // 插入Connectivity_n//不需要
                    Add_Connectivity_n(g3efid);
                    // 插入GG_JX_JLB_LB 暂不需要
                    // 插入GNWZ_SBTZ_RSHIP 暂不需要
                    g_newFid = g3efid;
                } 
                else
                {
                    // 修改
                    UpdateJLB();
                }
                SBLX = cbSBLX.Text;
                DialogResult = DialogResult.OK;
                Close();
            }
            catch (Exception ex)
            {
                LogManager.Instance.Error(ex);
                PublicMethod.Instance.AlertDialog(ex.Message);
            }            
        }

        private void btcancel_Click(object sender, EventArgs e)
        {
            DialogResult = DialogResult.Cancel;
            Close();
        }

        private void cbSSDW_TextChanged(object sender, EventArgs e)
        {
            //重置所属供电所
            var ssgds = CDDBManager.Instance.GetEntities<Cd_bs>().Where(o => o.SSDW == cbSSDW.Text).Select(o => o.NAME).ToList();
            cbSSGDS.DataSource = ssgds;
            cbWhbs.DataSource = ssgds;
            //重置所属馈线
            var ssxl = CDDBManager.Instance.GetEntities<Cd_ssxl>().Where(o => o.CD_SSDW == cbSSDW.Text).Select(o => o.NAME).ToList();
            cbSSXL.DataSource = ssxl;
            cbSSGDS.Text = "";
            cbWhbs.Text = "";
            cbSSXL.Text = "";
        }

        private void JLBEditer_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyValue == 27) // 用户按Esc
                btcancel_Click(sender, e);
            else if (e.KeyValue == 13) // 用户按Enter
                btok_Click(sender, e);
        }

        private void tbYHH_Validated(object sender, EventArgs e)
        {
            hasError();
        }

        private void tbHM_Validated(object sender, EventArgs e)
        {
            hasError();
        }

        private void cbDYDJ_Validated(object sender, EventArgs e)
        {
            hasError();
        }

        private void cbSMZQ_Validated(object sender, EventArgs e)
        {
            hasError();
        }

        private void tbSBMC_Validated(object sender, EventArgs e)
        {
            hasError();
        }

        private void cbSSDW_Validated(object sender, EventArgs e)
        {
            hasError();
        }

        private void cbSSGDS_Validated(object sender, EventArgs e)
        {
            hasError();
        }

        private void cbSSXL_Validated(object sender, EventArgs e)
        {
            hasError();
        }

        private void cbSBLX_Validated(object sender, EventArgs e)
        {
            hasError();
        }

        private void tbTYRQ_Validated(object sender, EventArgs e)
        {
            hasError();
        }

        private void cbWhbs_Validated(object sender, EventArgs e)
        {
            hasError();
        }
    }
}
