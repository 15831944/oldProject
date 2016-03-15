using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows.Forms;
using ArxMap;
using ElectronTransferDal.Common;
using ElectronTransferDal.Common.Exceptions;
using ElectronTransferDal.FunctionHelper;
using ElectronTransferDal.Query;
using ElectronTransferModel.V9_4;
using ElectronTransferModel;
using ElectronTransferDal.Cad;
using ElectronTransferView.FunctionManager;
using ElectronTransferDal;
using ElectronTransferModel.Base;
using ElectronTransferFramework;
using Autodesk.AutoCAD.Geometry;
using ElectronTransferModel.Geo;
using ElectronTransferModel.Config;
using ElectronTransferView.ViewManager;

namespace ElectronTransferView.ContextMenuManager
{
    public partial class JLBView : Form
    {
        /// <summary>
        /// 详表FID
        /// </summary>
        private long detail_fid { set; get; }
        private long detail_id { set; get; }
        private long detail_fno { set; get; }
        private double min_x { set; get; }
        private double max_y { set; get; }
        private double max_x { set; get; }
        private double min_y { set; get; }
        //private double xpix { set; get; }
        //private double ypix { set; get; }
        static private double width = 600;
        static private double hight = 600;
        private double scaleX { set; get; }
        private double scaleY { set; get; }
        private Point3d mouseLocation { set; get; }
        private List<PictureBox> pbs = new List<PictureBox>();

        private bool bMove { get; set; }
        
        public JLBView()
        {
            InitializeComponent();
        }
        public JLBView(selectEntityArgs ee)
        {
            detail_fid = ee.g3eObject.G3E_FID;
            detail_id = ee.g3eObject.G3E_ID;
            detail_fno = ee.g3eObject.G3E_FNO;
            InitializeComponent();
        }

        private void JLBView_Load(object sender, EventArgs e)
        {
            try
            {
                bMove = false;
                setRetangle();
                updateJLBView();
            }
            catch (Exception ex)
            {
                PublicMethod.Instance.AlertDialog(ex.Message);
            }
        }
        /// <summary>
        /// 鼠标弹起事件
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void JLBView_MouseUp(object sender, MouseEventArgs e)
        {
            // 记录鼠标点击位置
            mouseLocation = new Point3d(e.Location.X, e.Location.Y, 0);
        }

        private void setRetangle()
        {
            min_x = MapConfig.Instance.ProjectionMinX;
            min_y = MapConfig.Instance.ProjectionMinY;
            max_x = MapConfig.Instance.ProjectionMaxX;
            max_y = MapConfig.Instance.ProjectionMaxY;
            var jlbs = GetJLB_NS();
            if (jlbs != null)
            {
                int ii = 0;
                foreach (var jlb in jlbs)
                {
                    var jlb1 = jlb;
                    var jlbsdo =
                        DBManager.Instance.GetEntities<Gg_jx_jlb_pt_sdogeom>(
                            o => o.G3E_FID == jlb1.G3E_FID && o.EntityState != EntityState.Delete).FirstOrDefault();
                    if (jlbsdo != null)
                    {
                        var multipoint = jlbsdo.G3E_GEOMETRY as Multipoint;
                        if (multipoint != null)
                        {
                            var pt = new Point3d(multipoint.Points[0].X,
                                multipoint.Points[0].Y, 0);

                            if (ii == 0)
                            {
                                min_x = pt.X;
                                max_x = pt.X;
                                max_y = pt.Y;
                                min_y = pt.Y;
                            }
                            else
                            {
                                if (pt.X < min_x || min_x == 0) min_x = pt.X;
                                if (pt.X > max_x || max_x == 0) max_x = pt.X;
                                if (pt.Y > max_y || max_y == 0) max_y = pt.Y;
                                if (pt.Y < min_y || min_y == 0) min_y = pt.Y;
                            }
                        }
                    }
                    ii++;
                }
            }
            if (min_x == max_x)
            {
                min_x -= 0.01;
                max_x += 0.01;
            }
            if (min_y == max_y)
            {
                min_y -= 0.01;
                max_y += 0.01;
            }
            // 放大两倍
            double ttminx = min_x - (max_x - min_x) / 2;
            double ttmaxx = max_x + (max_x - min_x) / 2;
            double ttminy = min_y - (max_y - min_y) / 2;
            double ttmaxy = max_y + (max_y - min_y) / 2;
            min_x = ttminx;
            max_x = ttmaxx;
            min_y = ttminy;
            max_y = ttmaxy;

            #region

            /*
             公式：scaleX = ((maxLon-minLon)*3600)/h ----------X轴上每像素代表的经度秒数；
             公式：scaleY = ((maxLat-minLat)*3600)/y ----------Y轴上每像素代表的纬度秒数；
             公式：screenX = lon*3600/scaleX；---------屏幕坐标X轴坐标
             公式：screenY = lat*3600/scaleY； ---------屏幕坐标Y轴坐标
             * 
             * 公式：X = (lon - minLon)*3600/scaleX； 
             * 公式：Y = (maxLat - lat)*3600/scaleY；
             * 
             * 公式：lon = X * scaleX/3600 + minLon；
             * 公式：lat = maxLat - y* scaleY/3600；
             */

            #endregion

            //scaleX = ((max_x - min_x)*3600)/width;
            //scaleY = ((max_y - min_y)*3600)/hight;

        }

        private void updateJLBView()
        {
            try
            {
                pbs.ForEach(o => o.Dispose());
                pbs.Clear();
                var jlbs = GetJLB_NS();
                if (jlbs == null) return;
                foreach (var jlb in jlbs)
                {
                    var pt = GetPtFromGg_jx_jlb_pt_sdogeom(jlb.G3E_FID);
                    // 计算屏幕坐标
                    //var x = (int)((pt.X - min_x) * 3600 / scaleX);
                    //var y = (int)((pt.Y - min_y) * 3600 / scaleY);
                    var x = (int)((pt.X - min_x) * this.ClientRectangle.Width / (max_x - min_x));
                    var y = (int)((pt.Y - min_y) * this.ClientRectangle.Height / (max_y - min_y));
                    var pb = new PictureBox();
                    pb.Height = imageList1.ImageSize.Height;
                    pb.Width = imageList1.ImageSize.Width;
                    pb.Location = new System.Drawing.Point(x, y);
                    if (jlb.CD_SBLX == "高供高计")
                        pb.Image = imageList1.Images[0];
                    else
                        pb.Image = imageList1.Images[1];
                    pb.Tag = jlb.G3E_FID;
                    pb.ContextMenuStrip = contextMenuStrip2;
                    pb.MouseDoubleClick += pb_MouseDoubleClick;
                    pb.MouseDown += pb_MouseDown;
                    pb.MouseHover += pb_MouseHover;
                    pb.MouseMove += pb_MouseMove;
                    pb.MouseUp += pb_MouseUp;
                    pbs.Add(pb);
                    Controls.Add(pb);
                }
            }
            catch (Exception ex)
            {
                PublicMethod.Instance.AlertDialog(ex.Message);
            }
        }

        void pb_MouseUp(object sender, MouseEventArgs e)
        {
            try
            {
	            if (e.Button == MouseButtons.Left)
	            {
                    var pb = sender as PictureBox;
	                if (pb != null)
	                {
	                    var fid = (long)pb.Tag;
	                    UpdateSdo(fid, pb.Location);
	                }
	                bMove = false;
	            }
            }
            catch (Exception ex)
            {
                PublicMethod.Instance.AlertDialog(ex.Message);
            }
        }
        void pb_MouseMove(object sender, MouseEventArgs e)
        {
            try
            {
	            if (bMove)
	            {
		            var pb = sender as PictureBox;
	                if (pb != null)
	                {
	                    var p2 = pb.PointToScreen(e.Location);
	                    p2 = PointToClient(p2);
	                    p2.X -= imageList1.ImageSize.Width / 2;
	                    p2.Y -= imageList1.ImageSize.Height / 2;
	                    pb.Location = p2;
	                }
	                // 更新坐标表
	            }
            }
            catch (Exception ex)
            {
                PublicMethod.Instance.AlertDialog(ex.Message);
            }
        }
        void pb_MouseHover(object sender, EventArgs e)
        {
            try
            {
                var jlb = DBManager.Instance.GetEntities<Gg_pd_jlb_n>(o => o.G3E_FID == (long)(((PictureBox)sender).Tag)).FirstOrDefault();
                if (jlb != null)
                    toolTip1.SetToolTip(
                        (Control)sender,
                        String.Format("户名:{0}\n用户号:{1}\n备注:{2}\nG3E_FID:{3}", jlb.HM, jlb.YHH, jlb.BZ, jlb.G3E_FID));
            }
            catch (Exception ex)
            {
                PublicMethod.Instance.AlertDialog(ex.Message);
            }
        }
        void pb_MouseDown(object sender, MouseEventArgs e)
        {
            try
            {
                if (e.Button == MouseButtons.Left)
                {
                    bMove = true;
                }
                else if (e.Button == MouseButtons.Right)
                {
                    contextMenuStrip2.Tag = ((PictureBox)sender).Tag;
                    contextMenuStrip2.Show((Control)sender, e.Location);
                }
                //PublicMethod.Instance.ShowMessage("pb_MouseDown  " + ((PictureBox)sender).Tag);
            }
            catch (Exception ex)
            {
                PublicMethod.Instance.AlertDialog(ex.Message);
            }
        }
        void pb_MouseDoubleClick(object sender, MouseEventArgs e)
        {
            try
            {
                var tfid = (long)((PictureBox)sender).Tag;
                var tt = new JLBEditer(formType.regionEdit, tfid) {Text = @"修改计量表"};
                if (DialogResult.OK == tt.ShowDialog())
                {
                    updateJLBView();
                }
            }
            catch (Exception ex)
            {
                PublicMethod.Instance.AlertDialog(ex.Message);
            }
            //PublicMethod.Instance.ShowMessage("pb_MouseDoubleClick  " + tfid.ToString());
        }

        private void UpdateSdo(long g3efid, System.Drawing.Point ScreenPt)
        {
            var WorldPt = ScreenToWorld(new Point3d(ScreenPt.X, ScreenPt.Y, 0));
                var jlbsdo = DBManager.Instance.GetEntities<Gg_jx_jlb_pt_sdogeom>(o => o.G3E_FID == g3efid && o.EntityState != EntityState.Delete).FirstOrDefault();
                if (jlbsdo == null) return;
                if (jlbsdo.LTT_ID != null && DBEntityFinder.Instance.VerifyLTTID((decimal)jlbsdo.LTT_ID))
                {
                var mpValue = new Multipoint();
                mpValue.Points.Add(new Point(new[] { WorldPt.X, WorldPt.Y, 0 }));
                mpValue.Points.Add(new Point(new[] { Math.Cos(0.0), Math.Sin(0.0), 0 }));
                jlbsdo.G3E_GEOMETRY = mpValue;
                if (jlbsdo.EntityState != EntityState.Insert)
                {
                    jlbsdo.EntityState = EntityState.Update;
                }
                DBManager.Instance.Update(jlbsdo);
            }
        }

        private IEnumerable<Gg_pd_jlb_n> GetJLB_NS()
        {
            try
            {
                var jlbs = new List<Gg_pd_jlb_n>();
                var detail = DBManager.Instance.GetEntities<Detailreference_n>(o => o.G3E_FID == detail_fid && o.EntityState != EntityState.Delete).FirstOrDefault();//为空
                if (detail == null) 
                {
                    var df = new Detailreference_n
                    {
                        G3E_CID = 1,
                        G3E_ID = detail_id,
                        G3E_CNO = 35,
                        G3E_FID = detail_fid,
                        G3E_FNO = (int)detail_fno,
                        LTT_ID = MapConfig.Instance.LTTID,
                        EntityState = EntityState.Insert,
                        G3E_DETAILID = CYZCommonFunc.getid()
                    };
                    DBManager.Instance.Insert(df);
                    return null;
                }
                var jxjlbs = DBManager.Instance.GetEntities<Gg_jx_jlb_pt>(o => o.G3E_DETAILID == detail.G3E_DETAILID && o.EntityState != EntityState.Delete);
                if (jxjlbs == null) return null;
                foreach (var jlb in jxjlbs)
                {
                    jlbs.Add(GetJLB_N(jlb.G3E_FID));
                }
                return jlbs;
            }
            catch (NotExistException ex)
            {
                if (ex.Message == "Detailreference_n不存在")
                {
                    var type = TypeCache.Instance.GetTypeFromCache(typeof(DBEntity), "Detailreference_n");
                    var connectn = ReflectionUtils.CreateObject(new { }, type) as DBEntity;
                    DBManager.Instance.Insert(connectn);
                    // 插入变压器数据
                }
                else if (ex.Message == "Gg_jx_jlb_pt不存在")
                {
                    var type = TypeCache.Instance.GetTypeFromCache(typeof(DBEntity), "Gg_jx_jlb_pt");
                    var connectn = ReflectionUtils.CreateObject(new { }, type) as DBEntity;
                    DBManager.Instance.Insert(connectn);
                }
                else
                {
                    PublicMethod.Instance.AlertDialog(ex.Message);
                }
                return null;
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
        /// <summary>
        /// 新增事件
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void addToolStripMenuItem_Click(object sender, EventArgs e)
        {
            try
            {
                if (DBEntityFinder.Instance.VerifyLTTID(detail_fid) == false)
                {
                    PublicMethod.Instance.AlertDialog("该设备没有被工单锁定,不能新增计量表!");
                    Show();
                    return;
                }
                var mi = sender as ToolStripMenuItem;
                if (mi != null)
                {
                    var isLock = DBEntityFinder.Instance.VerifyLTTID(detail_fid);
                    var tt = new JLBEditer(formType.regionAdd, detail_fid, ScreenToWorld(mouseLocation), isLock)
                    {
                        Tag = mi.Text,
                        Text = @"新增计量表"
                    };
                    if (DialogResult.OK == tt.ShowDialog())
                    {
                        updateJLBView();
                    }
                }
            }
            catch (Exception ex)
            {
                PublicMethod.Instance.AlertDialog(ex.Message);
            }
        }
        /// <summary>
        /// 删除计量表
        /// </summary>
        /// <param name="fid">计量表FID</param>
        /// <param name="backupEntity"></param>
        /// <returns></returns>
        private static List<DBEntity> DeleteJLB(long? fid, List<DBEntity> backupEntity)
        {
            // 删除Gg_jx_jlb_pt
            var jlbpt = DBManager.Instance.GetEntities<Gg_jx_jlb_pt>(o => o.G3E_FID == fid && o.EntityState != EntityState.Delete).FirstOrDefault();
            if (null != jlbpt)
            {
                backupEntity = DBEntityErased.Instance.GetEntityStateByErasedDBEntity(jlbpt, backupEntity);
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
                //DBManager.Instance.Delete(jlbcomm);
                backupEntity = DBEntityErased.Instance.GetEntityStateByErasedDBEntity(jlbconn, backupEntity);
            }

            // 删除Gg_pd_gnwzmc_n
            var jlbgnwz = DBManager.Instance.GetEntities<Gg_pd_gnwzmc_n>(o => o.G3E_FID == fid && o.EntityState != EntityState.Delete).FirstOrDefault();
            if (null != jlbgnwz)
            {
                backupEntity = DBEntityErased.Instance.GetEntityStateByErasedDBEntity(jlbgnwz, backupEntity);
            }
            // 删除Gg_jx_jlb_pt_sdogeom
            var jlbsdo = DBManager.Instance.GetEntities<Gg_jx_jlb_pt_sdogeom>(o => o.G3E_FID == fid && o.EntityState != EntityState.Delete).FirstOrDefault();
            if (null != jlbsdo)
            {
                backupEntity = DBEntityErased.Instance.GetEntityStateByErasedDBEntity(jlbsdo, backupEntity);
            }
            // 删除GG_JX_JLB_LB 暂不需要
            // 删除GNWZ_SBTZ_RSHIP 暂不需要
            return backupEntity;
        }
        /// <summary>
        /// 屏幕坐标转换成经纬度
        /// </summary>
        /// <param name="pt">屏幕坐标</param>
        /// <returns>经纬度坐标</returns>
        private Point3d ScreenToWorld(Point3d pt)
        {
             // * 公式：lon = X * scaleX/3600 + minLon；
             //* 公式：lat = maxLat - y* scaleY/3600；
            var x = (pt.X) * scaleX / 3600 + min_x;
            //var y = max_y - (pt.Y) * scaleY / 3600;
            var y = (pt.Y) * scaleY / 3600 + min_y;
            return new Point3d(x, y, 0);
        }

        /// <summary>
        /// 根据Fid，获取Gg_jx_jlb_pt_sdogeom的坐标
        /// </summary>
        /// <param name="fid">计量表Fid</param>
        /// <returns>计量表坐标</returns>
        private Point3d GetPtFromGg_jx_jlb_pt_sdogeom(long fid)
        {
            var pt = Point3d.Origin;
            var jlbsdo =
                DBManager.Instance.GetEntities<Gg_jx_jlb_pt_sdogeom>(
                    o => o.G3E_FID == fid && o.EntityState != EntityState.Delete).FirstOrDefault();
            if (jlbsdo != null)
            {
                var multipoint = jlbsdo.G3E_GEOMETRY as Multipoint;
                if (multipoint != null)
                    pt = new Point3d(multipoint.Points[0].X,
                        multipoint.Points[0].Y, 0);
            }
            return pt;
        }

        /// <summary>
        /// 点击删除按钮
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void DelMenuItem_Click(object sender, EventArgs e)
        {
            try
            {
                // 获取被删除计量表的FID
                var tfid = (long)contextMenuStrip2.Tag;
                var temshb =
                    DBManager.Instance.GetEntities<Gg_jx_jlb_pt_sdogeom>(
                        o => o.G3E_FID == tfid && o.EntityState != EntityState.Delete).FirstOrDefault();
                if (temshb != null && (temshb.LTT_ID != null && (DBEntityFinder.Instance.VerifyLTTID((decimal)temshb.LTT_ID) == false)))
                {
                    PublicMethod.Instance.AlertDialog("该设备没有被工单锁定,不能删除!");
                    return;
                }
                DeleteJLB(tfid, new List<DBEntity>());
                updateJLBView();
            }
            catch (Exception ex)
            {
                PublicMethod.Instance.AlertDialog(ex.Message);
            }
        }

        private void editToolStripMenuItem_Click(object sender, EventArgs e)
        {
            try
            {
                var tfid = (long)contextMenuStrip2.Tag;
                var tt = new JLBEditer(formType.regionEdit, tfid) {Text = @"修改计量表"};
                if (DialogResult.OK == tt.ShowDialog())
                {
                    updateJLBView();
                }
            }
            catch (Exception ex)
            {
                PublicMethod.Instance.AlertDialog(ex.Message);
            }
        }

        /// <summary>
        /// 批量删除
        /// </summary>
        /// <param name="fid"></param>
        /// <param name="backupDBEntity"> </param>
        /// <returns></returns>
        public static List<DBEntity> BatchDeleteFromJLB(long fid, List<DBEntity> backupDBEntity)
        {
            try
            {
                //获取所有计量表
                var t = DBManager.Instance.GetEntity<Detailreference_n>(fid);
                if (t != null)
                {
                    var ents = GetJLB_PT(t.G3E_DETAILID);
                    if (ents != null)
                    {
                        foreach (long tmp in ents)
                        {
                            backupDBEntity = DeleteJLB(tmp, backupDBEntity);
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                LogManager.Instance.Error(ex);
            }
            return backupDBEntity;
        }
        /// <summary>
        /// 根据详表ID 获取 计量表集合
        /// </summary>
        /// <param name="g3e_detailid">详表ID</param>
        /// <returns>g3e_detailid对应的计量表集合</returns>
        private static IEnumerable<long> GetJLB_PT(long? g3e_detailid)
        {
            try
            {
                return DBManager.Instance.GetEntities<Gg_jx_jlb_pt>(o => o.G3E_DETAILID == g3e_detailid).Select(o => o.G3E_FID).ToList();
            }
            catch (Exception)
            {
                return null;
            }
        }

        private void TZMenuItem_Click(object sender, EventArgs e)
        {
            try
            {
                var tfid = (long)contextMenuStrip2.Tag;

                var g3efid = tfid.ToString();
                const string g3efno = "41";

                var comm = DBManager.Instance.GetEntities<Common_n>(o => o.G3E_FID == tfid).FirstOrDefault();
                if (comm == null) return;
                var gisid = MapConfig.Instance.LTTID.ToString();
                var sbmc = comm.SBMC;
                var s = "";
                if (!string.IsNullOrEmpty(sbmc))
                {
                    s = BitConverter.ToString(System.Text.Encoding.UTF8.GetBytes(sbmc)).Replace("-", "<M>");
                }

                var UrlStr = @"http://localhost:9090/emmis/equipGisMappingTemp/getInstallEquipments.gis?g3e_fid=" +
                                g3efid + "&jobId=" + gisid + "&g3e_fno=" + g3efno + "&editAble=Y&funcplaceName=" + s +
                                "&jgdh=009";
                ViewHelper.LoadTZPalette(UrlStr);
            }
            catch (Exception ex)
            {
                LogManager.Instance.Error(ex);
            }
        }

        private void JLBView_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyValue == 27 || e.KeyValue == 13)
                Close();
        }
    }
}
