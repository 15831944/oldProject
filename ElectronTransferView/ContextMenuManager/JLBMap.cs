using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Windows.Forms;
using ArxMap;
using ArxMap.DetailMap;
using Autodesk.AutoCAD.Geometry;
using ElectronTransferDal;
using ElectronTransferDal.Cad;
using ElectronTransferDal.Common;
using ElectronTransferDal.Common.Exceptions;
using ElectronTransferDal.FunctionHelper;
using ElectronTransferDal.Query;
using ElectronTransferFramework;
using ElectronTransferModel;
using ElectronTransferModel.Base;
using ElectronTransferModel.Config;
using ElectronTransferModel.Geo;
using ElectronTransferModel.V9_4;
using ElectronTransferView.FunctionManager;
using ElectronTransferView.ViewManager;
using Application = Autodesk.AutoCAD.ApplicationServices.Application;

namespace ElectronTransferView.ContextMenuManager
{
    public partial class JLBMap : Form
    {
        private DMap dddMap = new DMap();
        private List<Gg_pd_jlb_n> pdjlbs = new List<Gg_pd_jlb_n>();
        private List<JlbPB> pbs = new List<JlbPB>();

        /// <summary>
        /// 右键鼠标时屏幕位置
        /// </summary>
        private Point3d mouseLocation { set; get; }

        private bool ismousedown = false;
        private int oldx = 0, oldy = 0;
        private float imgxx = 0;
        private float imgyy = 0;
        private float imgww = 0;
        private float imghh = 0;
        private Bitmap img = null;
        private Bitmap img_back = null;
        private long detail_fid { set; get; }
        private long detail_id { set; get; }
        private long detail_fno { set; get; }
        private long search_fid { set; get; }
        private bool bMove { get; set; }
        private double picMapWidth = 0;
        private double picMapHeight = 0;
        private int wWidth = 0;
        private int wHeight = 0;
        private int picWwidth = 0;
        private int picWheight = 0;

        public JLBMap()
        {
            InitializeComponent();
        }

        public JLBMap(selectEntityArgs ee)
        {
            detail_fid = ee.g3eObject.G3E_FID;
            detail_id = ee.g3eObject.G3E_ID;
            detail_fno = ee.g3eObject.G3E_FNO;
            InitializeComponent();
        }

        public JLBMap(selectEntityArgs ee, long ser_fid)
        {
            detail_fid = ee.g3eObject.G3E_FID;
            detail_id = ee.g3eObject.G3E_ID;
            detail_fno = ee.g3eObject.G3E_FNO;
            search_fid = ser_fid;
            InitializeComponent();
        }

        private void JLBMap_Load(object sender, EventArgs e)
        {

            bMove = false;
            double min_x = 0;
            double min_y = 0;
            double max_x = 0;
            double max_y = 0;

            #region 计算出它的外围矩形。

            var jlbs = GetJLB_NS();
            if (jlbs != null)
            {
                pdjlbs = jlbs.ToList();
                List<double> allx = new List<double>();
                List<double> ally = new List<double>();


                foreach (var jlb in jlbs)
                {
                    var jlb1 = jlb;
                    var shbsdo =
                        DBManager.Instance.GetEntities<Gg_jx_jlb_pt_sdogeom>(
                            o => jlb1 != null && (o.G3E_FID == jlb1.G3E_FID && o.EntityState != EntityState.Delete))
                            .FirstOrDefault();
                    if (shbsdo == null)
                    {
                        PublicMethod.Instance.ShowMessage("没有设备 " + jlb1.G3E_FID + " 对应的坐标数据,请联系数据管理人员。\n");
                        continue;
                    }
                    var multipoint = shbsdo.G3E_GEOMETRY as Multipoint;
                    if (multipoint == null) continue;
                    var pt = new Point3d(multipoint.Points[0].X,
                        multipoint.Points[0].Y, 0);
                    allx.Add(pt.X);
                    ally.Add(pt.Y);
                }
                if (allx.Count > 0)
                {
                    min_x = allx.Min();
                    max_x = allx.Max();
                }
                if (ally.Count > 0)
                {
                    min_y = ally.Min();
                    max_y = ally.Max();
                }
            }

            //2 当没有没有设备时候,以沿布图坐标为中心点,上下加减250米
            Multipoint vcbxpoint;
            if (detail_fno == 148)
            {
                var vcbx = DBManager.Instance.GetEntities<Gg_pd_zfbyq_pt_sdogeom>(o => o.G3E_FID == detail_fid).First();
                vcbxpoint = vcbx.G3E_GEOMETRY as Multipoint;

            }
            else
            {
                var vcbx = DBManager.Instance.GetEntities<Gg_pd_cjlg_pt_sdogeom>(o => o.G3E_FID == detail_fid).First();
                vcbxpoint = vcbx.G3E_GEOMETRY as Multipoint;
            }
            double dx = 5/DMap.DD;
            double dy = dx;
            if (min_x == 0 || max_x == 0 || min_y == 0 || max_x == 0)
            {
                //vcbxpoint = vcbx.G3E_GEOMETRY as Multipoint;
                if (vcbxpoint != null)
                {
                    min_y = vcbxpoint.Points[0].Y - dy;
                    min_x = vcbxpoint.Points[0].X - dy;
                    max_y = vcbxpoint.Points[0].Y + dy;
                    max_x = vcbxpoint.Points[0].X + dy;
                }
            }

            if ((max_x - min_x)/(max_y - min_y) > 3)
            {
                min_y = max_y - (max_x - min_x);
            }
            else if ((max_y - min_y)/(max_x - min_x) > 3)
            {
                max_x = min_x + (max_y - min_y);
            }


            if (min_x == max_x) // 当设备只有一个,或者成一直线时
            {
                min_x -= dx;
                max_x += dx;
            }
            else // 放大
            {
                var t = (max_x - min_x)/4;
                min_x -= t;
                max_x += t;
            }
            if (min_y == max_y)
            {
                min_y -= dy;
                max_y += dy;
            }
            else
            {
                var t = (max_y - min_y)/4;
                min_y -= t;
                max_y += t;
            }

            #endregion

            wWidth = this.ClientSize.Width;
            wHeight = this.ClientSize.Height;

            dddMap.createDmap(min_x, max_x, min_y, max_y, wWidth, wHeight);

            if (jlbs != null) pdjlbs = jlbs.ToList();
            List<int> xindex = new List<int>();
            List<int> yindex = new List<int>();
            foreach (var shb in pdjlbs)
            {
                var pt = GetPtFromGg_jx_jlb_pt_sdogeom(shb.G3E_FID);
                double[] xy = dddMap.geo2screen(pt.Y, pt.X);

                xindex.Add((int) xy[0]);
                yindex.Add((int) xy[0]);
            }
            int xcount = 0, ycount = 0;
            int i = 0;
            for (i = 0; i < xindex.Count - 1; i++)
            {
                if (Math.Floor((double) (xindex[i] - xindex[i + 1])) < 8)
                {
                    xcount++;
                }
            }
            if (xindex.Count >= 2)
                if (Math.Floor((double) (xindex[i - 1] - xindex[i])) < 8)
                {
                    xcount++;
                }
            for (i = 0; i < yindex.Count - 1; i++)
            {
                if (Math.Floor((double) (yindex[i] - yindex[i + 1])) < 10)
                {
                    ycount++;
                }
            }
            if (yindex.Count >= 2)
                if (Math.Floor((double) (yindex[i - 1] - yindex[i])) < 10)
                {
                    ycount++;
                }

            if (xcount > wWidth/16)
            {
                xcount = (int) (wWidth*0.75/xcount);
            }
            else
            {
                xcount = 16;
            }
            if (ycount > wHeight/16)
            {
                ycount = (int) (wHeight*0.75/ycount);
            }
            else
            {
                ycount = 16;
            }

            picMapWidth = ((double) xcount/wWidth)*(dddMap.ScreenMapMaxX - dddMap.ScreenMapMinX);
            picMapHeight = (double) ycount/wHeight*(dddMap.ScreenMapMaxY - dddMap.ScreenMapMinY_In);

            picWwidth = (int) (picMapWidth/(dddMap.ScreenMapMaxX - dddMap.ScreenMapMinX)*wWidth);
            picWheight = (int) (picMapHeight/(dddMap.ScreenMapMaxY - dddMap.ScreenMapMinY_In)*wHeight);

            #region 初始化低压户表

            foreach (var jlb in pdjlbs)
            {
                var pt = GetPtFromGg_jx_jlb_pt_sdogeom(jlb.G3E_FID);
                double[] xy = dddMap.geo2screen(pt.Y, pt.X);

                var pb = new JlbPB();
                pb.Height = picWheight;
                pb.Width = picWwidth;
                pb.Location = new System.Drawing.Point((int) xy[0] - xcount/2, (int) xy[1] - ycount/2);
                pb.Image = imageList1.Images[0];
                if (jlb.CD_SBLX == "高供高计")
                {
                    pb.Image = imageList1.Images[0];
                    if (search_fid == jlb.G3E_FID)
                    {
                        pb.Image = imageList1.Images[2];
                    }
                }
                else
                {
                    pb.Image = imageList1.Images[1];
                    if (search_fid == jlb.G3E_FID)
                    {
                        pb.Image = imageList1.Images[3];
                    }
                }

                pb.Tag = jlb.G3E_FID;
                pb.ContextMenuStrip = contextMenuStrip2;
                pb.JlbPBPosition = pt;
                pb.JlbPBFid = jlb.G3E_FID;
                pb.JlbSBLX = jlb.CD_SBLX;
                pb.SizeMode = PictureBoxSizeMode.StretchImage;
                pb.MouseDoubleClick += pb_MouseDoubleClick;
                pb.MouseDown += pb_MouseDown;
                pb.MouseHover += pb_MouseHover;
                pb.MouseUp += pb_MouseUp;
                pb.MouseMove += pb_MouseMove;
                pbs.Add(pb);
                Controls.Add(pb);
            }

            #endregion

            drawBackImg();
        }

        /// 画详图设备
        private void drawBackImg()
        {
            try
            {
                if (dddMap != null)
                {
                    img_back = new Bitmap(
                        dddMap.ScreenWidth,
                        dddMap.ScreenHeight);

                    Graphics g = Graphics.FromImage(img_back);
                    g.Clear(Color.White);

                    #region

                    //picWwidth = (int)(picMapWidth / (dddMap.ScreenMapMaxX - dddMap.ScreenMapMinX) * wWidth);
                    //picWheight = (int)(picMapHeight / (dddMap.ScreenMapMaxY - dddMap.ScreenMapMinY_In) * wHeight);
                    //float picw = (float) (picMapWidth/(dddMap.ScreenMapMaxX - dddMap.ScreenMapMinX)*wWidth);
                    //float pich = (float) (picMapHeight/(dddMap.ScreenMapMaxY - dddMap.ScreenMapMinY_In)*wHeight);

                    #endregion

                    foreach (var pb in pbs)
                    {
                        var xy = dddMap.geo2screen(pb.JlbPBPosition.Y, pb.JlbPBPosition.X);
                        var index = 0;
                        if (pb.JlbSBLX == "高供低计")
                        {
                            index = 1;
                            if (search_fid == pb.JlbPBFid)
                                index = 3;
                        }
                        else if (search_fid == pb.JlbPBFid)
                        {
                            index = 2;
                        }
                        g.DrawImage(imageList1.Images[index], (float)(xy[0] - picWwidth / 2.0),
                            (float)(xy[1] - picWheight / 2.0), picWwidth, picWheight);
                        // 缩放到一定程度,隐藏标注
                        //if (picWwidth > 10 || picWheight > 10)
                        //    g.DrawString(string.Format("{0}", pb.JlbPBDz), new Font("宋体", 9), new SolidBrush(Color.Red),
                        //        (float) (xy[0] - picWwidth/2.0), (float) (xy[1] + picWheight/2.0));
                    }
                    g.Dispose();

                    img = (Bitmap)img_back.Clone();
                    img_back.Dispose();

                    imgxx = 0;
                    imgyy = 0;
                    imgww = dddMap.ScreenWidth;
                    imghh = dddMap.ScreenHeight;
                }

            }
            catch (Exception ex)
            {
            }


        }

        private void draw()
        {
            BufferedGraphicsContext GraphicManager = BufferedGraphicsManager.Current;
            GraphicManager.MaximumBuffer = new Size(this.Width + 1, this.Height + 1);
            BufferedGraphics ManagedBackBuffer = GraphicManager.Allocate(this.CreateGraphics(), ClientRectangle);
            ManagedBackBuffer.Graphics.Clear(Color.White);
            if (img != null)
            {
                ManagedBackBuffer.Graphics.DrawImage(img,
                    imgxx,
                    imgyy,
                    imgww,
                    imghh
                    );

                // 画中心十字
                var pp2 = new Pen(Color.Blue);
                ManagedBackBuffer.Graphics.DrawLine(pp2, this.ClientRectangle.Width / 2 - 5, this.ClientRectangle.Height / 2,
                    this.ClientRectangle.Width / 2 + 5, this.ClientRectangle.Height / 2);
                ManagedBackBuffer.Graphics.DrawLine(pp2, this.ClientRectangle.Width / 2, this.ClientRectangle.Height / 2 - 5,
                    this.ClientRectangle.Width / 2, this.ClientRectangle.Height / 2 + 5);

            }

            ManagedBackBuffer.Render(this.CreateGraphics());
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

        private IEnumerable<Gg_pd_jlb_n> GetJLB_NS()
        {
            try
            {
                string alertMessage = "";
                var detail =
                    DBManager.Instance.GetEntities<Detailreference_n>(
                        o => o.G3E_FID == detail_fid && o.EntityState != EntityState.Delete).FirstOrDefault(); //为空
                if (detail == null)
                {
                    // 添加详图数据
                    PublicMethod.Instance.Add_Detailreference_n(detail_id, detail_fid, detail_fno);
                    return null;
                }
                if (detail.DETAIL_USERNAME == null) detail.DETAIL_USERNAME = detail.G3E_FID.ToString();
                if (detail.G3E_DETAILID == null) detail.G3E_DETAILID = CYZCommonFunc.getid();
                if (detail.EntityState != EntityState.Insert) detail.EntityState = EntityState.Update;
                DBManager.Instance.Update(detail);

                var jlbs = new List<Gg_pd_jlb_n>();
                var jxjlbs =
                    DBManager.Instance.GetEntities<Gg_jx_jlb_pt>(
                        o => o.G3E_DETAILID == detail.G3E_DETAILID && o.EntityState != EntityState.Delete);
                if (jxjlbs == null) return null;
                foreach (var jlb in jxjlbs)
                {
                    var s = GetJLB_N(jlb.G3E_FID);
                    if (s == null)
                    {
                        alertMessage += " " + jlb.G3E_FID;
                        continue;
                    }
                    jlbs.Add(s);
                }
                if (!string.IsNullOrEmpty(alertMessage))
                    PublicMethod.Instance.AlertDialog("在Gg_pd_jlb_n表没有FID为" + alertMessage + " 的数据.");
                return jlbs;
            }
            catch (NotExistException ex)
            {
                PublicMethod.Instance.NotExistTable(ex);
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
                var t =
                    DBManager.Instance.GetEntities<Gg_pd_jlb_n>(
                        o => o.G3E_FID == fid && o.EntityState != EntityState.Delete).FirstOrDefault();
                return t;
            }
            catch (Exception)
            {
                return null;
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
        /// 删除计量表
        /// </summary>
        /// <param name="fid">计量表FID</param>
        /// <param name="backupEntity"></param>
        /// <returns></returns>
        private static List<DBEntity> DeleteJLB(long? fid, List<DBEntity> backupEntity)
        {
            try
            {
                // 删除Gg_jx_jlb_pt
                var jlbpt =
                    DBManager.Instance.GetEntities<Gg_jx_jlb_pt>(
                        o => o.G3E_FID == fid && o.EntityState != EntityState.Delete).FirstOrDefault();
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
                var jlbcomm =
                    DBManager.Instance.GetEntities<Common_n>(o => o.G3E_FID == fid && o.EntityState != EntityState.Delete)
                        .FirstOrDefault();
                if (null != jlbcomm)
                {
                    backupEntity = DBEntityErased.Instance.GetEntityStateByErasedDBEntity(jlbcomm, backupEntity);
                }

                // 删除Connectivity_n
                var jlbconn =
                    DBManager.Instance.GetEntities<Connectivity_n>(
                        o => o.G3E_FID == fid && o.EntityState != EntityState.Delete).FirstOrDefault();
                if (null != jlbconn)
                {
                    //DBManager.Instance.Delete(jlbcomm);
                    backupEntity = DBEntityErased.Instance.GetEntityStateByErasedDBEntity(jlbconn, backupEntity);
                }

                // 删除Gg_pd_gnwzmc_n
                var jlbgnwz =
                    DBManager.Instance.GetEntities<Gg_pd_gnwzmc_n>(
                        o => o.G3E_FID == fid && o.EntityState != EntityState.Delete).FirstOrDefault();
                if (null != jlbgnwz)
                {
                    backupEntity = DBEntityErased.Instance.GetEntityStateByErasedDBEntity(jlbgnwz, backupEntity);
                }
                // 删除Gg_jx_jlb_pt_sdogeom
                var jlbsdo =
                    DBManager.Instance.GetEntities<Gg_jx_jlb_pt_sdogeom>(
                        o => o.G3E_FID == fid && o.EntityState != EntityState.Delete).FirstOrDefault();
                if (null != jlbsdo)
                {
                    backupEntity = DBEntityErased.Instance.GetEntityStateByErasedDBEntity(jlbsdo, backupEntity);
                }
                // 删除GG_JX_JLB_LB 暂不需要
                // 删除GNWZ_SBTZ_RSHIP 暂不需要
                return backupEntity;
            }
            catch (NotExistException ex)
            {
                PublicMethod.Instance.NotExistTable(ex);
                DeleteJLB(fid, backupEntity);
                return backupEntity;
            }
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
                return
                    DBManager.Instance.GetEntities<Gg_jx_jlb_pt>(o => o.G3E_DETAILID == g3e_detailid)
                        .Select(o => o.G3E_FID)
                        .ToList();
            }
            catch (Exception)
            {
                return null;
            }
        }

        private void UpdateSdo(long g3efid, System.Drawing.Point ScreenPt)
        {
            var WorldPt = dddMap.screen2geo(ScreenPt.X, ScreenPt.Y);
            var shbsdo =
                DBManager.Instance.GetEntities<Gg_jx_jlb_pt_sdogeom>(
                    o => o.G3E_FID == g3efid && o.EntityState != EntityState.Delete).FirstOrDefault();
            var mpValue = new Multipoint();
            mpValue.Points.Add(new ElectronTransferModel.Geo.Point(new[] {WorldPt[1], WorldPt[0], 0}));
            mpValue.Points.Add(new ElectronTransferModel.Geo.Point(new[] {Math.Cos(0.0), Math.Sin(0.0), 0}));
            if (shbsdo != null)
            {
                shbsdo.G3E_GEOMETRY = mpValue;
                if (shbsdo.EntityState != EntityState.Insert)
                {
                    shbsdo.EntityState = EntityState.Update;
                }
                DBManager.Instance.Update(shbsdo);
                var pb = pbs.FirstOrDefault(o => o.JlbPBFid == g3efid);
                if (pb != null) pb.JlbPBPosition = new Point3d(WorldPt[1], WorldPt[0], 0);
            }
        }

        private bool issheew = false;

        protected override void WndProc(ref Message m)
        {
            base.WndProc(ref m);

            try
            {
                switch (m.Msg)
                {
                    case 522:
                        foreach (var pb in pbs)
                        {
                            pb.Visible = false;
                        }
                        if ((int) m.WParam > 0)
                        {
                            dddMap.zoom(1, oldx, oldy, true);
                        }
                        else
                        {
                            dddMap.zoom(-1, oldx, oldy, true);
                        }

                        picWwidth = (int) (picMapWidth/(dddMap.ScreenMapMaxX - dddMap.ScreenMapMinX)*wWidth);
                        picWheight = (int) (picMapHeight/(dddMap.ScreenMapMaxY - dddMap.ScreenMapMinY_In)*wHeight);
                        if (picWwidth <= 2)
                        {
                            picWwidth = 2;
                        }
                        if (picWheight <= 2)
                        {
                            picWheight = 2;
                        }
                        drawBackImg();
                        draw();
                        issheew = true;
                        break;
                }
            }
            catch (Exception ex)
            {
            }
        }

        private void JLBMap_MouseDown(object sender, MouseEventArgs e)
        {
            ismousedown = true;

            foreach (var pb in pbs)
            {
                pb.Visible = false;
            }
            draw();
        }

        private void JLBMap_MouseMove(object sender, MouseEventArgs e)
        {
            if (ismousedown)
            {
                dddMap.move(e.X - oldx, e.Y - oldy);

                imgxx += (e.X - oldx);
                imgyy += (e.Y - oldy);

            }
            draw();
            oldx = e.X;
            oldy = e.Y;

            if (issheew)
            {
                foreach (var pb in pbs)
                {
                    double[] xy = dddMap.geo2screen(pb.JlbPBPosition.Y, pb.JlbPBPosition.X);
                    pb.Location = new System.Drawing.Point((int) (xy[0] - picWwidth/2.0), (int) (xy[1] - picWheight/2.0));
                    pb.Width = picWwidth;
                    pb.Height = picWheight;
                }
                issheew = false;
            }
        }

        private void JLBMap_MouseUp(object sender, MouseEventArgs e)
        {
            ismousedown = false;
            // 记录鼠标点击位置
            mouseLocation = new Point3d(e.Location.X, e.Location.Y, 0);
            if (img != null)
            {
                img.Dispose();
            }
            drawBackImg();
            draw();

            foreach (var pb in pbs)
            {
                double[] xy = dddMap.geo2screen(pb.JlbPBPosition.Y, pb.JlbPBPosition.X);
                pb.Location = new System.Drawing.Point((int) (xy[0] - picWwidth/2.0), (int) (xy[1] - picWheight/2.0));
                pb.Width = picWwidth;
                pb.Height = picWheight;
                pb.Visible = true;
            }
        }

        private void JLBMap_Paint(object sender, PaintEventArgs e)
        {
            drawBackImg();
            draw();
        }

        private void JLBMap_Resize(object sender, EventArgs e)
        {
            wWidth = this.ClientSize.Width;
            wHeight = this.ClientSize.Height;
            drawBackImg();
            draw();
        }

        private void AddJLBItem_Click(object sender, EventArgs e)
        {
            try
            {
                if (DBEntityFinder.Instance.VerifyLTTID(detail_fid) == false)
                {
                    PublicMethod.Instance.AlertDialog("设备没有被工单锁定,不能新增计量表!");
                    Show();
                    return;
                }
                var mi = sender as ToolStripMenuItem;
                if (mi != null)
                {
                    var tshbgeo = dddMap.screen2geo(mouseLocation.X, mouseLocation.Y);
                    var isLock = DBEntityFinder.Instance.VerifyLTTID(detail_fid);
                    var tt = new JLBEditer(formType.regionAdd, detail_fid, tshbgeo, isLock) {Tag = mi.Text};
                    if (DialogResult.OK == tt.ShowDialog())
                    {
                        // 加一个计量表，刷新窗口
                        var pb = new JlbPB();
                        pb.Height = picWheight;
                        pb.Width = picWwidth;
                        pb.Location = new System.Drawing.Point((int) mouseLocation.X - picWwidth/2,
                            (int) mouseLocation.Y - picWheight/2);
                        if (tt.SBLX == "高供高计")
                            pb.Image = imageList1.Images[0];
                        else
                            pb.Image = imageList1.Images[1];
                        pb.Tag = tt.g_newFid;
                        pb.ContextMenuStrip = contextMenuStrip2;
                        pb.JlbPBPosition = new Point3d(tshbgeo[1], tshbgeo[0], 0);
                        pb.JlbPBFid = tt.g_newFid;
                        pb.JlbSBLX = tt.SBLX;
                        pb.SizeMode = PictureBoxSizeMode.StretchImage;
                        pb.MouseDoubleClick += pb_MouseDoubleClick;
                        pb.MouseDown += pb_MouseDown;
                        pb.MouseHover += pb_MouseHover;
                        pb.MouseUp += pb_MouseUp;
                        pb.MouseMove += pb_MouseMove;
                        pbs.Add(pb);
                        Controls.Add(pb);
                    }
                }
            }
            catch (Exception ex)
            {
                PublicMethod.Instance.AlertDialog(ex.Message);
            }
        }

        private void EditJLBItem_Click(object sender, EventArgs e)
        {
            try
            {
                var tfid = (long) contextMenuStrip2.Tag;
                var isLock = DBEntityFinder.Instance.VerifyLTTID(detail_fid);
                var tt = new JLBEditer(formType.regionEdit, tfid, detail_fid, isLock);
                if (DialogResult.OK == tt.ShowDialog())
                {
                    foreach (var t2 in Controls)
                    {
                        var t3 = t2 as JlbPB;
                        if (t3 != null && t3.JlbPBFid == tfid)
                        {
                            t3.JlbSBLX = tt.SBLX;
                            if (tt.SBLX == "高供高计")
                                t3.Image = imageList1.Images[0];
                            else
                                t3.Image = imageList1.Images[1];
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                PublicMethod.Instance.AlertDialog(ex.Message);
            }
        }

        private void DelJLBItem_Click(object sender, EventArgs e)
        {
            try
            {
                if (MessageBox.Show(@"确定要删除?", @"确定?", MessageBoxButtons.OKCancel) == DialogResult.Cancel) return;
                try
                {
                    // 获取被删除计量表的FID
                    var tfid = (long) contextMenuStrip2.Tag;
                    var temjlb =
                        DBManager.Instance.GetEntities<Gg_jx_jlb_pt_sdogeom>(
                            o => o.G3E_FID == tfid && o.EntityState != EntityState.Delete).FirstOrDefault();
                    if (temjlb != null &&
                        (temjlb.LTT_ID != null &&
                         (DBEntityFinder.Instance.VerifyLTTID((decimal) temjlb.LTT_ID) == false)))
                    {
                        PublicMethod.Instance.AlertDialog("该设备没有被工单锁定,不能删除!");
                        return;
                    }
                    DeleteJLB(tfid, new List<DBEntity>());
                    //删除一个户表控件,刷新界面
                    var t1 = Controls;
                    foreach (var t2 in Controls)
                    {
                        var t3 = t2 as JlbPB;
                        if (t3 != null && t3.JlbPBFid == tfid)
                        {
                            pbs.Remove(t3);
                            Controls.Remove(t3);
                        }
                    }

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

        private void MoveJLBItem_Click(object sender, EventArgs e)
        {
            try
            {
                var isLock = DBEntityFinder.Instance.VerifyLTTID(detail_fid);
                if (!isLock)
                {
                    PublicMethod.Instance.AlertDialog("详图没有被工单锁定,不能迁移计量表!");
                    Show();
                    return;
                }
                var movept = dddMap.screen2geo(mouseLocation.X, mouseLocation.Y);
                var detail =
                    DBManager.Instance.GetEntities<Detailreference_n>(
                        o => o.G3E_FID == detail_fid && o.EntityState != EntityState.Delete).FirstOrDefault();
                if (detail != null)
                {
                    if (detail.G3E_DETAILID == null) return;
                    var tt = new JLBMove((long)detail.G3E_DETAILID, movept);
                    var res = Application.ShowModalDialog(tt);
                    if (res == DialogResult.OK)
                    {
                        // 加一个计量表，刷新窗口
                        var pb = new JlbPB();
                        pb.Height = picWheight;
                        pb.Width = picWwidth;
                        pb.Location = new System.Drawing.Point((int)mouseLocation.X - picWwidth / 2,
                            (int)mouseLocation.Y - picWheight / 2);
                        if (tt.SBLX == "高供高计")
                            pb.Image = imageList1.Images[0];
                        else
                            pb.Image = imageList1.Images[1];
                        pb.Tag = tt.g3e_Fid;
                        pb.ContextMenuStrip = contextMenuStrip2;
                        pb.JlbPBPosition = new Point3d(movept[1], movept[0], 0);
                        pb.JlbPBFid = tt.g3e_Fid;
                        pb.JlbSBLX = tt.SBLX;
                        pb.SizeMode = PictureBoxSizeMode.StretchImage;
                        pb.MouseDoubleClick += pb_MouseDoubleClick;
                        pb.MouseDown += pb_MouseDown;
                        pb.MouseHover += pb_MouseHover;
                        pb.MouseUp += pb_MouseUp;
                        pb.MouseMove += pb_MouseMove;
                        pbs.Add(pb);
                        Controls.Add(pb);
                    }
                }
            }
            catch (Exception ex)
            {
                LogManager.Instance.Error(ex);
            }
        }

        private void JLBMap_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyValue == 27 || e.KeyValue == 13)
                Close();
        }

        private void pb_MouseUp(object sender, MouseEventArgs e)
        {
            try
            {
                if (e.Button == MouseButtons.Left)
                {
                    var pb = sender as JlbPB;
                    if (pb != null)
                    {
                        pbs.Add(pb);
                        var fid = pb.JlbPBFid;
                        var pblocation = new System.Drawing.Point(pb.Location.X + picWwidth/2,
                            pb.Location.Y + picWheight/2);
                        UpdateSdo(fid, pblocation);
                        drawBackImg();
                        draw();
                    }
                    bMove = false;
                }
            }
            catch (Exception ex)
            {
                PublicMethod.Instance.AlertDialog(ex.Message);
            }
        }

        private void pb_MouseMove(object sender, MouseEventArgs e)
        {
            try
            {
                if (bMove)
                {
                    var pb = sender as JlbPB;
                    if (pb != null)
                    {
                        var p2 = pb.PointToScreen(e.Location);
                        p2 = PointToClient(p2);
                        p2.X -= picWwidth/2;
                        p2.Y -= picWheight/2;
                        pb.Location = p2;
                    }
                }
            }
            catch (Exception ex)
            {
                PublicMethod.Instance.AlertDialog(ex.Message);
            }
        }

        private void pb_MouseHover(object sender, EventArgs e)
        {
            try
            {
                var jlb =
                    DBManager.Instance.GetEntities<Gg_pd_jlb_n>(o => o.G3E_FID == (long) (((PictureBox) sender).Tag))
                        .FirstOrDefault();
                if (jlb != null)
                    toolTip1.SetToolTip(
                        (Control) sender,
                        String.Format("户名:{0}\n用户号:{1}\n表号:{2}\nG3E_FID:{3}", jlb.HM, jlb.YHH, jlb.BH, jlb.G3E_FID));
            }
            catch (Exception ex)
            {
                PublicMethod.Instance.AlertDialog(ex.Message);
            }
        }

        private void pb_MouseDown(object sender, MouseEventArgs e)
        {
            try
            {
                var pb = sender as JlbPB;
                if (e.Button == MouseButtons.Left)
                {
                    pbs.Remove(pb);
                    bMove = true;
                }
                else if (e.Button == MouseButtons.Right)
                {
                    contextMenuStrip2.Tag = ((PictureBox) sender).Tag;
                    contextMenuStrip2.Show((Control) sender, e.Location);
                }
            }
            catch (Exception ex)
            {
                PublicMethod.Instance.AlertDialog(ex.Message);
            }
        }

        private void pb_MouseDoubleClick(object sender, MouseEventArgs e)
        {
            try
            {
                var tfid = (long) ((PictureBox) sender).Tag;
                var isLock = DBEntityFinder.Instance.VerifyLTTID(detail_fid);
                var tt = new JLBEditer(formType.regionEdit, tfid, detail_fid, isLock);
                //tt.Text = "修改计量表";
                if (DialogResult.OK == tt.ShowDialog())
                {
                    foreach (var t2 in Controls)
                    {
                        var t3 = t2 as JlbPB;
                        if (t3 != null && t3.JlbPBFid == tfid)
                        {
                            t3.JlbSBLX = tt.SBLX;
                            if (tt.SBLX == "高供高计")
                                t3.Image = imageList1.Images[0];
                            else
                                t3.Image = imageList1.Images[1];
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                PublicMethod.Instance.AlertDialog(ex.Message);
            }
        }

        private void TZMenuItem_Click(object sender, EventArgs e)
        {
            try
            {
                var tfid = (long) contextMenuStrip2.Tag;
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
                //新增设备需加a，原设备不需加a
                var g3efid =comm.EntityState==EntityState.Insert? string.Format("a{0}",tfid):tfid.ToString();
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

        private void contextMenuStrip2_MouseLeave(object sender, EventArgs e)
        {
            contextMenuStrip2.Hide();
        }

        private void contextMenuStrip1_MouseLeave(object sender, EventArgs e)
        {
            contextMenuStrip1.Hide();
        }

        private void JLBMap_FormClosed(object sender, FormClosedEventArgs e)
        {
            try
            {
                // 更新详表坐标范围
                var detail =
                    DBManager.Instance.GetEntities<Detailreference_n>(
                        o => o.G3E_FID == detail_fid && o.EntityState != EntityState.Delete).FirstOrDefault();
                if (detail == null) return;
                var distX = dddMap.ScreenMapMaxX - dddMap.ScreenMapMinX;
                var distY = dddMap.ScreenMapMaxY - dddMap.ScreenMapMinY_In;
                detail.DETAIL_MBRXLO = (decimal)dddMap.ScreenMapMinX;
                detail.DETAIL_MBRXHIGH = (decimal)dddMap.ScreenMapMaxX;
                detail.DETAIL_MBRYLO = (decimal)dddMap.ScreenMapMinY_In;
                detail.DETAIL_MBRYHIGH = (decimal)dddMap.ScreenMapMaxY;
                // 距离相差0.0002,返写回GIS,比例刚好符合
                if (distX < 0.0001)
                {
                    detail.DETAIL_MBRXLO = (decimal)(dddMap.ScreenMapMinX - 0.0001);
                    detail.DETAIL_MBRXHIGH = (decimal)(dddMap.ScreenMapMaxX + 0.0001);
                }
                if (distY < 0.0001)
                {
                    detail.DETAIL_MBRYLO = (decimal) (dddMap.ScreenMapMinY_In - 0.0001);
                    detail.DETAIL_MBRYHIGH = (decimal) (dddMap.ScreenMapMaxY + 0.0001);
                }
                if (detail.EntityState != EntityState.Insert)
                {
                    detail.EntityState = EntityState.Update;
                }
                DBManager.Instance.Update(detail);
            }
            catch (Exception exception)
            {
                LogManager.Instance.Error(exception);
            }
        }

        //private bool Add_Detailreference_n(long g3e_id, long g3e_fid, long g3e_fno)
        //{
        //    var df = new Detailreference_n();
        //    df.G3E_CID = 1;
        //    df.G3E_ID = g3e_id;
        //    df.G3E_CNO = 35;
        //    df.G3E_FID = g3e_fid;
        //    df.G3E_FNO = (int)g3e_fno;
        //    df.LTT_ID = MapConfig.Instance.LTTID;
        //    df.EntityState = EntityState.Insert;
        //    df.G3E_DETAILID = CYZCommonFunc.getid();
        //    df.DETAIL_USERNAME = g3e_fid.ToString();
        //    df.DETAIL_LEGENDNUMBER = 35;
        //    if(g3e_fno == 148)
        //        df.DETAIL_LEGENDNUMBER = 34;
        //    else if (g3e_fno == 159)
        //        df.DETAIL_LEGENDNUMBER = 6;
        //    df.DETAIL_MBRXLO = Convert.ToDecimal(112.356172100576);
        //    df.DETAIL_MBRYLO = Convert.ToDecimal(21.9600071382762);
        //    df.DETAIL_MBRXHIGH = Convert.ToDecimal(112.356316442584);
        //    df.DETAIL_MBRYHIGH = Convert.ToDecimal(21.9601062562456);
        //    df.DETAIL_MBRXOFFSET = 50000;
        //    df.DETAIL_MBRYOFFSET = 50000;
        //    return DBManager.Instance.Insert(df);
        //}

        public class JlbPB : PictureBox
        {
            public Point3d JlbPBPosition { get; set; }
            public long JlbPBFid { get; set; }
            public string JlbSBLX { get; set; }
        }

    }
}
