using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows.Forms;
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

namespace ElectronTransferView.ContextMenuManager
{
    public partial class SHBView : Form
    {
        /// <summary>
        /// 详表FID
        /// </summary>
        private long detail_fid { set; get; }
        private long search_fid { set; get; }
        private double min_x { set; get; }
        private double max_y { set; get; }
        private double max_x { set; get; }
        private double min_y { set; get; }
        static private double width = 600;
        static private double hight = 600;
        private double scaleX { set; get; }
        private double scaleY { set; get; }
        private Point3d mouseLocation { set; get; }
        private List<PictureBox> pbs = new List<PictureBox>();

        private bool bMove { get; set; }

        public SHBView()
        {
            InitializeComponent();
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="fid">集中抄表箱FID</param>
        public SHBView(long fid)
        {
            detail_fid = fid;
            InitializeComponent();
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="fid">集中抄表箱FID</param>
        /// <param name="ser_fid">需要查找的低压户表FID</param>
        public SHBView(long fid,long ser_fid)
        {
            detail_fid = fid;
            search_fid = ser_fid;
            InitializeComponent();
        }

        private void SHBView_Load(object sender, EventArgs e)
        {
            try
            {
                bMove = false;
                var shbs = setRetangle();
                updateSHBView(shbs);
            }
            catch (Exception ex)
            {
                PublicMethod.Instance.AlertDialog(ex.Message);
            }
        }

        private void SHBView_MouseUp(object sender, MouseEventArgs e)
        {
            // 记录鼠标点击位置
            mouseLocation = new Point3d(e.Location.X, e.Location.Y, 0);
        }

        /// <summary>
        /// 把户表集合的经纬度范围转换成窗口范围
        /// </summary>
        /// <returns></returns>
        private IEnumerable<Gg_pd_dyshb_n> setRetangle()
        {
            min_x = MapConfig.Instance.ProjectionMinX;
            min_y = MapConfig.Instance.ProjectionMinY;
            max_x = MapConfig.Instance.ProjectionMaxX;
            max_y = MapConfig.Instance.ProjectionMaxY;
            var shbs = GetSHB_NS();
            if (shbs != null)
            {
                int ii = 0;
                foreach (var shb in shbs)
                {
                    var shb1 = shb;
                    var shbsdo =
                        DBManager.Instance.GetEntities<Gg_jx_shbd_pt_sdogeom>(
                            o => shb1 != null && (o.G3E_FID == shb1.G3E_FID && o.EntityState != EntityState.Delete))
                            .FirstOrDefault();
                    if (shbsdo == null)
                    {
                        throw new Exception("没有设备 " + shb.G3E_FID + " 对应的坐标数据,请联系数据管理人员。");
                    }


                    var multipoint = shbsdo.G3E_GEOMETRY as Multipoint;
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
                    ii++;
                }
            }
            if (min_x == max_x)
            {
                min_x -= 0.001;
                max_x += 0.001;
            }
            if (min_y == max_y)
            {
                min_y -= 0.001;
                max_y += 0.001;
            }
            // 放大两倍
            double ttminx = min_x - (max_x - min_x)/2;
            double ttmaxx = max_x + (max_x - min_x)/2;
            double ttminy = min_y - (max_y - min_y)/2;
            double ttmaxy = max_y + (max_y - min_y)/2;
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

            //var t1 = max_x - min_x;
            //var t2 = max_y - min_y;

            //scaleX = ((max_x - min_x)*3600)/width;
            //scaleY = ((max_y - min_y)*3600)/hight;
            return shbs;
        }

        /// <summary>
        /// 在窗口中更新低压户表集合
        /// </summary>
        /// <param name="shbs"></param>
        private void updateSHBView(IEnumerable<Gg_pd_dyshb_n> shbs)
        {
            try
            {
                pbs.ForEach(o => o.Dispose());
                pbs.Clear();
                if (shbs == null)
                {
                	shbs = GetSHB_NS();
                }
                foreach (var shb in shbs)
                {
                    var pt = GetPtFromGg_jx_shbd_pt_sdogeom(shb.G3E_FID);
                    
                    // 计算屏幕坐标
                    //var x = (int)((pt.X - min_x) * 3600 / scaleX);
                    //var y = (int)((pt.Y - min_y) * 3600 / scaleY);
                    var x = (int)((pt.X - min_x) * this.ClientRectangle.Width / (max_x - min_x));
                    var y = (int)((pt.Y - min_y) * this.ClientRectangle.Height / (max_y - min_y));

                    var pb = new PictureBox
                    {
                        Height = imageList1.ImageSize.Height,
                        Width = imageList1.ImageSize.Width,
                        Location = new System.Drawing.Point(x, y),
                        Image = imageList1.Images[0],
                        Tag = shb.G3E_FID,
                        ContextMenuStrip = contextMenuStrip2
                    };
                    // 如果是查找的户表,高亮
                    if (search_fid == shb.G3E_FID)
                    {
                        pb.Height += 12;
                        pb.Width += 12;
                        pb.SizeMode = PictureBoxSizeMode.CenterImage;
                        pb.BackColor = System.Drawing.Color.Lime;
                    }
                    pb.MouseDoubleClick += pb_MouseDoubleClick;
                    pb.MouseDown += pb_MouseDown;
                    pb.MouseHover += pb_MouseHover;
                    pb.MouseUp += pb_MouseUp;
                    pb.MouseMove += pb_MouseMove;
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
                        var fid = (long) pb.Tag;
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
                var shb = DBManager.Instance.GetEntities<Gg_pd_dyshb_n>(o => o.G3E_FID == (long)(((PictureBox)sender).Tag)).FirstOrDefault();
                if (shb != null)
                    toolTip1.SetToolTip(
                        (Control)sender,
                        String.Format("房号:{0}\n表号:{1}\n备注:{2}\n门牌地址:{3}\n用户用电号:{4}\n用户姓名:{5}", shb.AZDZ, shb.BH, shb.BZ, shb.HH, shb.YDH, shb.YHXM)
                        );
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
                var isLock = DBEntityFinder.Instance.VerifyLTTID(detail_fid);
                var tt = new SHBEditer(formType.regionEdit, tfid,isLock);
                if (DialogResult.OK == tt.ShowDialog())
                {
                    updateSHBView(null);
                }
            }
            catch (Exception ex)
            {
                PublicMethod.Instance.AlertDialog(ex.Message);
            }
        }
        /// <summary>
        /// 把屏幕坐标转换成经纬度，更新到FID=g3efid的户表内存数据
        /// </summary>
        /// <param name="g3efid">散户表</param>
        /// <param name="ScreenPt">屏幕坐标</param>
        private void UpdateSdo(long g3efid, System.Drawing.Point ScreenPt)
        {
            var WorldPt = ScreenToWorld(new Point3d(ScreenPt.X, ScreenPt.Y, 0));
            var shbsdo = DBManager.Instance.GetEntities<Gg_jx_shbd_pt_sdogeom>(o => o.G3E_FID == g3efid && o.EntityState != EntityState.Delete).FirstOrDefault();
            if (shbsdo == null) return;
            //if (shbsdo.LTT_ID != null && DBEntityFinder.Instance.VerifyLTTID((decimal)shbsdo.LTT_ID))
            {
                var mpValue = new Multipoint();
                mpValue.Points.Add(new Point(new[] {WorldPt.X, WorldPt.Y, 0}));
                mpValue.Points.Add(new Point(new[] {Math.Cos(0.0), Math.Sin(0.0), 0}));
                shbsdo.G3E_GEOMETRY = mpValue;
                if (shbsdo.EntityState != EntityState.Insert)
                {
                    shbsdo.EntityState = EntityState.Update;
                }
                DBManager.Instance.Update(shbsdo);
            }
        }
        /// <summary>
        /// 得到g3efid集抄箱里的户表集合
        /// </summary>
        /// <returns>Gg_pd_dyshb_n户表集合</returns>
        private IEnumerable<Gg_pd_dyshb_n> GetSHB_NS()
        {
            try
            {
                var shbs = new List<Gg_pd_dyshb_n>();
                var detail =
                    DBManager.Instance.GetEntities<Detailreference_n>(
                        o => o.G3E_FID == detail_fid && o.EntityState != EntityState.Delete).FirstOrDefault();
                if (detail == null) return null;
                var jxshbs =
                    DBManager.Instance.GetEntities<Gg_jx_shbd_pt>(
                        o => o.G3E_DETAILID == detail.G3E_DETAILID && o.EntityState != EntityState.Delete);
                if (jxshbs == null) return null;
                foreach (var shb in jxshbs)
                {
                    var s = GetSHB_N(shb.G3E_FID);
                    if (s == null)
                    {
                        PublicMethod.Instance.AlertDialog("在Gg_pd_dyshb_n表没有FID = " + shb.G3E_FID + " 的数据.");
                        continue;
                    }
                    shbs.Add(s);
                }
                return shbs;
            }
            catch (NotExistException ex)
            {
                if (ex.Message == "Gg_jx_shbd_pt不存在")
                {
                    var type = TypeCache.Instance.GetTypeFromCache(typeof (DBEntity), "Gg_jx_shbd_pt");
                    var connectn = ReflectionUtils.CreateObject(new {}, type) as DBEntity;
                    DBManager.Instance.Insert(connectn);
                }
                else
                {
                    PublicMethod.Instance.AlertDialog(ex.Message);
                }
                return null;
            }
            catch (Exception ex)
            {
                PublicMethod.Instance.AlertDialog(ex.Message);
                return null;
            }
        }
        /// <summary>
        /// 得到户表的自身属性
        /// </summary>
        /// <param name="fid">户表G3e_Fid</param>
        /// <returns>户表属性</returns>
        private static Gg_pd_dyshb_n GetSHB_N(long? fid)
        {
            try
            {
                var t = DBManager.Instance.GetEntities<Gg_pd_dyshb_n>(o => o.G3E_FID == fid && o.EntityState != EntityState.Delete).FirstOrDefault();
                return t;
            }
            catch (Exception)
            {
                return null;
            }
        }
        /// <summary>
        /// 删除计量表
        /// </summary>
        /// <param name="fid">计量表FID</param>
        /// <param name="backupEntity"></param>
        /// <returns></returns>
        private static List<DBEntity> DeleteSGB(long? fid, List<DBEntity> backupEntity)
        {
            // 删除Gg_jx_shbd_pt
            var shbpt = DBManager.Instance.GetEntities<Gg_jx_shbd_pt>(o => o.G3E_FID == fid && o.EntityState != EntityState.Delete).FirstOrDefault();
            if (null != shbpt)
            {
                backupEntity = DBEntityErased.Instance.GetEntityStateByErasedDBEntity(shbpt, backupEntity);
            }
            // 删除Gg_pd_dyshb_n
            var shbn = GetSHB_N(fid);
            if (null != shbn)
            {
                backupEntity = DBEntityErased.Instance.GetEntityStateByErasedDBEntity(shbn, backupEntity);
            }
            // 删除Connectivity_n
            var shbconn = DBManager.Instance.GetEntities<Connectivity_n>(o => o.G3E_FID == fid && o.EntityState != EntityState.Delete).FirstOrDefault();
            if (null != shbconn)
            {
                backupEntity = DBEntityErased.Instance.GetEntityStateByErasedDBEntity(shbconn, backupEntity);
            }
            // 删除Gg_jx_jlb_pt_sdogeom
            var shbsdo = DBManager.Instance.GetEntities<Gg_jx_shbd_pt_sdogeom>(o => o.G3E_FID == fid && o.EntityState != EntityState.Delete).FirstOrDefault();
            if (null != shbsdo)
            {
                backupEntity = DBEntityErased.Instance.GetEntityStateByErasedDBEntity(shbsdo, backupEntity);
            }
            // 删除GG_JX_SHBD_LB
            var shbjxlb = DBManager.Instance.GetEntities<Gg_jx_shbd_lb>(o => o.G3E_FID == fid && o.EntityState != EntityState.Delete).FirstOrDefault();
            if (null != shbjxlb)
            {
                backupEntity = DBEntityErased.Instance.GetEntityStateByErasedDBEntity(shbjxlb, backupEntity);
            }

            // 删除HYPERLINK_N 暂不需要
            // 删除GG_PD_DYSHB_LB 暂不需要
            // 删除GG_PD_DYSHB_PT 暂不需要
            return backupEntity;
        }
        /// <summary>
        /// 批量删除
        /// </summary>
        /// <param name="fid"></param>
        /// <param name="backupDBEntity"> </param>
        /// <returns></returns>
        public static List<DBEntity> BatchDeleteFromSHB(long fid, List<DBEntity> backupDBEntity)
        {
            try
            {
                //获取所有计量表
                var t = DBManager.Instance.GetEntity<Detailreference_n>(fid);
                if (t != null)
                {
                    var ents = GetSHB_PT(t.G3E_DETAILID);
                    if (ents != null)
                    {
                        foreach (long tmp in ents)
                        {
                            backupDBEntity = DeleteSGB(tmp, backupDBEntity);
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
        private static IEnumerable<long> GetSHB_PT(long? g3e_detailid)
        {
            try
            {
                return DBManager.Instance.GetEntities<Gg_jx_shbd_pt>(o => o.G3E_DETAILID == g3e_detailid).Select(o => o.G3E_FID).ToList();
            }
            catch (Exception)
            {
                return null;
            }
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
        private Point3d GetPtFromGg_jx_shbd_pt_sdogeom(long fid)
        {
            var pt = Point3d.Origin;
            var jlbsdo =
                DBManager.Instance.GetEntities<Gg_jx_shbd_pt_sdogeom>(
                    o => o.G3E_FID == fid && o.EntityState != EntityState.Delete).FirstOrDefault();
            if (jlbsdo != null)
            {
                var multipoint = jlbsdo.G3E_GEOMETRY as Multipoint;
                if (multipoint != null)
                    pt = new Point3d(multipoint.Points[0].X, multipoint.Points[0].Y, 0);
            }
            return pt;
        }

        private void AddSHBItem_Click(object sender, EventArgs e)
        {
            try
            {
                var isLock = DBEntityFinder.Instance.VerifyLTTID(detail_fid);
                if (!isLock)
                {
                    PublicMethod.Instance.AlertDialog("抄表箱没有被工单锁定,不能新增户表!");
                    Show();
                    return;
                }
                var mi = sender as ToolStripMenuItem;
                if (mi != null)
                {
                    var tt = new SHBEditer(formType.regionAdd, detail_fid, ScreenToWorld(mouseLocation),isLock) {Tag = mi.Text};
                    if (DialogResult.OK == tt.ShowDialog())
                    {
                        updateSHBView(null);
                    }
                }
            }
            catch (Exception ex)
            {
                PublicMethod.Instance.AlertDialog(ex.Message);
            }
        }

        private void EditSHBItem_Click(object sender, EventArgs e)
        {
            try
            {
                var tfid = (long)contextMenuStrip2.Tag;
                var isLock = DBEntityFinder.Instance.VerifyLTTID(detail_fid);
                var tt = new SHBEditer(formType.regionEdit, tfid,isLock);
                if (DialogResult.OK == tt.ShowDialog())
                {
                    updateSHBView(null);
                }
            }
            catch (Exception ex)
            {
                PublicMethod.Instance.AlertDialog(ex.Message);
            }
        }

        private void DelSHBItem_Click(object sender, EventArgs e)
        {
            try
            {
                if (MessageBox.Show(@"确定要删除?", @"确定?", MessageBoxButtons.OKCancel) == DialogResult.Cancel) return;
                try
                {
                    // 获取被删除计量表的FID
                    var tfid = (long)contextMenuStrip2.Tag;
                    var temshb =
                        DBManager.Instance.GetEntities<Gg_jx_shbd_pt>(
                            o => o.G3E_FID == tfid && o.EntityState != EntityState.Delete).FirstOrDefault();
                    if (temshb != null && (temshb.LTT_ID != null && (DBEntityFinder.Instance.VerifyLTTID((decimal)temshb.LTT_ID) == false)))
                    {
                        PublicMethod.Instance.AlertDialog("该设备没有被工单锁定,不能删除!");
                        return;
                    }
                    DeleteSGB(tfid, new List<DBEntity>());
                    updateSHBView(null);
                }
                catch (Exception ex)
                {
                    PublicMethod.Instance.AlertDialog(String.Format("删除过程中出现错误:{0}", ex.Message));
                }
            }
            catch (Exception ex)
            {
                PublicMethod.Instance.AlertDialog(ex.Message);
            }
        }

        private void SHBView_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyValue == 27 || e.KeyValue == 13)
                Close();
        }
    }


    //public static class SHBDeleteManager
    //{
    //    /// <summary>
    //    /// 获取户表自身属性
    //    /// </summary>
    //    /// <param name="g3e_fid"></param>
    //    /// <returns></returns>
    //    public static Gg_pd_dyshb_n GetDYSHB_N(long g3e_fid)
    //    {
    //        try
    //        {
    //            return DBManager.Instance.GetEntity<Gg_pd_dyshb_n>(g3e_fid);
    //        }
    //        catch
    //        {
    //            return null;
    //        }
    //    }

    //    /// <summary>
    //    /// 批量删除
    //    /// </summary>
    //    /// <param name="fid"></param>
    //    /// <param name="backupDBEntity"> </param>
    //    /// <returns></returns>
    //    public static List<DBEntity> BatchDeleteFromJCX(long fid, List<DBEntity> backupDBEntity)
    //    {
    //        try
    //        {
    //            //获取集抄箱下的所有户表
    //            var t = DBManager.Instance.GetEntity<Detailreference_n>(fid);
    //            if (t != null)
    //            {
    //                var ents = DBEntityFinder.Instance.GetDYSHB_PT(t.G3E_DETAILID);
    //                if (ents != null)
    //                {
    //                    foreach (long tmp in ents)
    //                    {
    //                        DeleteItem(tmp, backupDBEntity);
    //                    }
    //                }
    //            }
    //        }
    //        catch (Exception ex)
    //        {
    //            LogManager.Instance.Error(ex);
    //        }
    //        return backupDBEntity;
    //    }

    //    /// <summary>
    //    /// 删除单个户表
    //    /// </summary>
    //    /// <param name="fid"></param>
    //    /// <param name="backupDBEntity"> </param>
    //    public static List<DBEntity> DeleteItem(long fid, List<DBEntity> backupDBEntity)
    //    {
    //        backupDBEntity = DeleteItemUndo(fid, backupDBEntity);
    //        return backupDBEntity;
    //    }

    //    private static List<DBEntity> DeleteItemUndo(long g3e_fid, List<DBEntity> backupDBEntity)
    //    {
    //        //删除户表数据本身
    //        var ent = DBManager.Instance.GetEntity<Gg_pd_dyshb_n>(g3e_fid);
    //        if (ent != null)
    //        {
    //            if (ent.EntityState == EntityState.Delete) return backupDBEntity;

    //            if (ent.EntityState == EntityState.Insert)
    //            {
    //                backupDBEntity.Add(ent.Clone() as DBEntity);
    //                DBManager.Instance.Delete(ent);
    //            }
    //            else if (ent.EntityState == EntityState.None || ent.EntityState == EntityState.Update)
    //            {
    //                ent.RedoState = true;
    //                ent.LTT_ID = MapConfig.Instance.LTTID;
    //                backupDBEntity.Add(ent.Clone() as DBEntity);
    //                ent.EntityState = EntityState.Delete;
    //                DBManager.Instance.Update(ent);
    //            }

    //            //删除从属关系
    //            var ent2 = DBManager.Instance.GetEntity<Gg_jx_shbd_pt>(g3e_fid);
    //            if (ent2 != null)
    //            {
    //                if (ent2.EntityState == EntityState.Insert)
    //                {
    //                    backupDBEntity.Add(ent2.Clone() as DBEntity);
    //                    DBManager.Instance.Delete(ent2);
    //                }
    //                else if (ent2.EntityState == EntityState.None || ent2.EntityState == EntityState.Update)
    //                {
    //                    ent2.RedoState = true;
    //                    ent2.LTT_ID = MapConfig.Instance.LTTID;
    //                    backupDBEntity.Add(ent2.Clone() as DBEntity);
    //                    ent2.EntityState = EntityState.Delete;
    //                    DBManager.Instance.Update(ent2);
    //                }
    //            }
    //            else
    //            {
    //                throw new Exception("找不到从属关系");
    //            }
    //        }
    //        else
    //        {
    //            throw new Exception("找不到实体");
    //        }
    //        return backupDBEntity;
    //    }
    //}
}
