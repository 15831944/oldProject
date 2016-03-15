using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using ArxMap;
using ArxMap.DetailMap;
using Autodesk.AutoCAD.Geometry;
using ElectronTransferDal;
using ElectronTransferDal.Cad;
using ElectronTransferDal.Common;
using ElectronTransferDal.Common.Exceptions;
using ElectronTransferDal.FunctionHelper;
using ElectronTransferFramework;
using ElectronTransferModel;
using ElectronTransferModel.Base;
using ElectronTransferModel.Config;
using ElectronTransferModel.Geo;
using ElectronTransferModel.V9_4;
using ElectronTransferDal.Query;
using ElectronTransferView.FunctionManager;
using acApi = Autodesk.AutoCAD.ApplicationServices.Application;

namespace ElectronTransferView.ContextMenuManager
{
    public partial class SHBMap : Form
    {
        public delegate void SyncPanelData(long fno, long fid);

        public static event SyncPanelData shbMapToPanel;
        /// <summary>
        /// 老大的地图投影类
        /// </summary>
        private DMap dddMap = new DMap();
        /// <summary>
        /// 当前集抄箱下的户表集合
        /// </summary>
        private List<Gg_pd_dyshb_n> dyshbs = new List<Gg_pd_dyshb_n>();
        /// <summary>
        /// 当前SHBMap窗口的 户表控件 集合
        /// </summary>
        private List<Dyhb> pbs = new List<Dyhb>();
        /// <summary>
        /// 右键鼠标时屏幕位置
        /// </summary>
        private Point3d mouseLocation { set; get; }
        /// <summary>
        /// 鼠标是否按下
        /// </summary>
        bool ismousedown = false;
        int oldx = 0, oldy = 0;
        float imgxx = 0;
        float imgyy = 0;
        float imgww = 0;
        float imghh = 0;
        Bitmap img = null;
        Bitmap img_back = null;
        /// <summary>
        /// 集抄箱的G3E_FID
        /// </summary>
        private long detail_fid { set; get; }
        private long detail_id { set; get; }
        private long detail_fno { set; get; }
        /// <summary>
        /// 户表搜索的户表G3E_FID
        /// </summary>
        private long search_fid { set; get; }
        /// <summary>
        /// 鼠标移动时状态
        /// </summary>
        private bool bMove { get; set; }
        private double picMapWidth = 0;
        private double picMapHeight = 0;
        /// <summary>
        /// 当前窗口客户全宽度
        /// </summary>
        private int wWidth = 0;
        /// <summary>
        /// 当前窗口客户全高度
        /// </summary>
        private int wHeight = 0;
        /// <summary>
        /// 当前控件图标宽度
        /// </summary>
        private int picWwidth = 0;
        /// <summary>
        /// 当前控件图标高度
        /// </summary>
        private int picWheight = 0;
        public SHBMap()
        {
            InitializeComponent();
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="fid">集中抄表箱FID</param>
        public SHBMap(selectEntityArgs ee)
        {
            detail_fid = ee.g3eObject.G3E_FID;
            detail_id = ee.g3eObject.G3E_ID;
            detail_fno = ee.g3eObject.G3E_FNO;
            InitializeComponent();
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="fid">集中抄表箱FID</param>
        /// <param name="ser_fid">需要查找的低压户表FID</param>
        public SHBMap(long fid, long ser_fid)
        {
            detail_fid = fid;
            search_fid = ser_fid;
            InitializeComponent();
        }

        public SHBMap(selectEntityArgs ee, long ser_fid)
        {
            detail_fid = ee.g3eObject.G3E_FID;
            detail_id = ee.g3eObject.G3E_ID;
            detail_fno = ee.g3eObject.G3E_FNO;
            search_fid = ser_fid;
            InitializeComponent();
        }
        private void SHBMap_Load(object sender, EventArgs e)
        {
            bMove = false;
            double min_x = 0;
            double min_y = 0;
            double max_x = 0;
            double max_y = 0;

            #region 计算出它的外围矩形。
            // 得到户表集合
            var shbs = GetSHB_NS();
            // 1 当前集抄箱有户表时，取出户表集合坐标范围
            if (shbs != null)
            {
                dyshbs = shbs.ToList();
                var allx = new List<double>();
                var ally = new List<double>();
                // 遍历户表集合的坐标
                foreach (var shb in shbs)
                {
                    var shb1 = shb;
                    var shbsdo = DBManager.Instance.GetEntities<Gg_jx_shbd_pt_sdogeom>( o => shb1 != null && (o.G3E_FID == shb1.G3E_FID && o.EntityState != EntityState.Delete)).FirstOrDefault();
                    if (shbsdo == null)
                    {
                        LogManager.Instance.Error("没有设备 " + shb.G3E_FID + " 对应的坐标数据,请联系数据管理人员。\n");
                        //PublicMethod.Instance.ShowMessage("没有设备 " + shb.G3E_FID + " 对应的坐标数据,请联系数据管理人员。\n");
                        continue;
                    }
                    var multipoint = shbsdo.G3E_GEOMETRY as Multipoint;
                    if (multipoint == null) continue;
                    var pt = new Point3d(multipoint.Points[0].X,multipoint.Points[0].Y, 0);
                    allx.Add(pt.X);
                    ally.Add(pt.Y);
                }
                // 得到x轴的最大点、最小点
                if (allx.Count > 0)
                {
                    min_x = allx.Min();
                    max_x = allx.Max();
                }
                // 得到y轴的最大点、最小点
                if (ally.Count > 0)
                {
                    min_y = ally.Min();
                    max_y = ally.Max();
                }
            }
            //2 当没有没有设备时候,以沿布图集抄箱坐标为中心点,上下加减250米
            var vcbx = DBManager.Instance.GetEntities<Gg_pd_cbx_pt_sdogeom>(o => o.G3E_FID == detail_fid).First();
            double dx = 250/DMap.DD;
            double dy = dx;
            if (min_x == 0 || max_x == 0 || min_y == 0 || max_x == 0)
            {
                var vcbxpoint = vcbx.G3E_GEOMETRY as Multipoint;
                if (vcbxpoint != null)
                {
                    min_y = vcbxpoint.Points[0].Y - dy;
                    min_x = vcbxpoint.Points[0].X - dy;
                    max_y = vcbxpoint.Points[0].Y + dy;
                    max_x = vcbxpoint.Points[0].X + dy;
                }
            }
            // 当长宽比超过3时,拉伸窗口
            if ((max_x - min_x)/(max_y - min_y) > 3)
            {
                min_y = max_y - (max_x - min_x);
            }
            else if ((max_y - min_y) / (max_x - min_x) >3)
            {
                max_x = min_x + (max_y - min_y);
            }

            // 3 当设备只有一个,或者成一直线时
            if (min_x == max_x) 
            {
                min_x -= dx;
                max_x += dx;
            }
            else // 放大
            {
                var t = (max_x - min_x) / 4;
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
                var t = (max_y - min_y) / 4;
                min_y -= t;
                max_y += t;
            }
            #endregion

            wWidth = this.ClientSize.Width;
            wHeight = this.ClientSize.Height;

            dddMap.createDmap(min_x, max_x, min_y, max_y, wWidth, wHeight);

            if (shbs != null) dyshbs = shbs.ToList();
            List<int> xindex = new List<int>();
            List<int> yindex = new List<int>();
            foreach (var shb in dyshbs)
            {
                var pt = GetPtFromGg_jx_shbd_pt_sdogeom(shb.G3E_FID);
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
                ycount = (int)(wHeight * 0.75 / ycount);
            }
            else
            {
                ycount = 16;
            }

            picMapWidth = ((double)xcount / wWidth) * (dddMap.ScreenMapMaxX - dddMap.ScreenMapMinX);
            picMapHeight = (double)ycount / wHeight * (dddMap.ScreenMapMaxY - dddMap.ScreenMapMinY_In);

            picWwidth = (int)(picMapWidth / (dddMap.ScreenMapMaxX - dddMap.ScreenMapMinX) * wWidth);
            picWheight = (int)(picMapHeight / (dddMap.ScreenMapMaxY - dddMap.ScreenMapMinY_In) * wHeight);

            #region 初始化低压户表

            foreach (var shb in dyshbs)
            {
                var pt = GetPtFromGg_jx_shbd_pt_sdogeom(shb.G3E_FID);
                double[] xy = dddMap.geo2screen(pt.Y, pt.X);

                var pb = new Dyhb();
                pb.Height = picWheight;
                pb.Width = picWwidth;
                pb.Location = new System.Drawing.Point((int) xy[0] - xcount/2, (int) xy[1] - ycount/2);
                pb.Image = imageList1.Images[0];
                if (search_fid == shb.G3E_FID)
                {
                    pb.Image = imageList1.Images[1];
                }
                pb.Tag = shb.G3E_FID;
                pb.ContextMenuStrip = contextMenuStrip2;
                pb.DyhbPosition = pt;
                pb.DyhbFid = shb.G3E_FID;
                pb.DyhbDz = shb.AZDZ;
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
                        var xy = dddMap.geo2screen(pb.DyhbPosition.Y, pb.DyhbPosition.X);
                        var index = 0;
                        if (search_fid == pb.DyhbFid) index = 1;
                        g.DrawImage(imageList1.Images[index], (float)(xy[0] - picWwidth / 2.0), (float)(xy[1] - picWheight / 2.0), picWwidth, picWheight);    
                        // 缩放到一定程度,隐藏标注
                        if(picWwidth >10 || picWheight > 10)
                        g.DrawString(string.Format("{0}", pb.DyhbDz), new Font("宋体", 9), new SolidBrush(Color.Red),
                            (float) (xy[0] - picWwidth/2.0), (float) (xy[1] + picWheight/2.0));
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
            catch (Exception ex) { }


        }

        void draw()
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
                Pen pp2 = new Pen(Color.Blue);
                ManagedBackBuffer.Graphics.DrawLine(pp2, this.ClientRectangle.Width / 2 - 5, this.ClientRectangle.Height / 2, this.ClientRectangle.Width / 2 + 5, this.ClientRectangle.Height / 2);
                ManagedBackBuffer.Graphics.DrawLine(pp2, this.ClientRectangle.Width / 2, this.ClientRectangle.Height / 2 - 5, this.ClientRectangle.Width / 2, this.ClientRectangle.Height / 2 + 5);

            }

            ManagedBackBuffer.Render(this.CreateGraphics());
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
        /// <summary>
        /// 得到当前集抄箱的户表集合
        /// </summary>
        /// <returns>户表集合</returns>
        private IEnumerable<Gg_pd_dyshb_n> GetSHB_NS()
        {
            try
            {
                string alertMessage = "";
                var shbs = new List<Gg_pd_dyshb_n>();
                var detail = DBManager.Instance.GetEntities<Detailreference_n>(o => o.G3E_FID == detail_fid && o.EntityState != EntityState.Delete).FirstOrDefault();
                if (detail == null)
                {
                    PublicMethod.Instance.Add_Detailreference_n(detail_id, detail_fid, detail_fno);
                    return null;
                }
                // 更正详图信息
                if (detail.DETAIL_USERNAME == null) detail.DETAIL_USERNAME = detail.G3E_FID.ToString();
                if (detail.G3E_DETAILID == null) detail.G3E_DETAILID = CYZCommonFunc.getid();
                if (detail.EntityState != EntityState.Insert) detail.EntityState = EntityState.Update;
                var jxshbs = DBManager.Instance.GetEntities<Gg_jx_shbd_pt>(o => o.G3E_DETAILID == detail.G3E_DETAILID && o.EntityState != EntityState.Delete);
                if (jxshbs == null) return null;
                foreach (var shb in jxshbs)
                {
                    var s = GetSHB_N(shb.G3E_FID);
                    if (s == null)
                    {
                        alertMessage += " " + shb.G3E_FID;
                        continue;
                    }
                    shbs.Add(s);
                }
                if (!string.IsNullOrEmpty(alertMessage))
                    PublicMethod.Instance.AlertDialog("在Gg_pd_dyshb_n表没有FID为" + alertMessage + " 的数据.");
                return shbs;
            }
            catch (NotExistException ex)
            {
                if (ex.Message.Contains("不存在"))
                {
                    var strlx = ex.Message.Replace("不存在", "");
                    var type = TypeCache.Instance.GetTypeFromCache(typeof (DBEntity), strlx);
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
        public static Gg_pd_dyshb_n GetSHB_N(long? fid)
        {
            try
            {
                var t =
                    DBManager.Instance.GetEntities<Gg_pd_dyshb_n>(
                        o => o.G3E_FID == fid && o.EntityState != EntityState.Delete).FirstOrDefault();
                return t;
            }
            catch (NotExistException ex)
            {
                if (ex.Message.Contains("不存在"))
                {
                    var strlx = ex.Message.Replace("不存在", "");
                    var type = TypeCache.Instance.GetTypeFromCache(typeof (DBEntity), strlx);
                    var connectn = ReflectionUtils.CreateObject(new {}, type) as DBEntity;
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
                            backupDBEntity = SHBDeleteManager.DeleteSGB(tmp, backupDBEntity);
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

        private void UpdateSdo(long g3efid, System.Drawing.Point ScreenPt)
        {
            var WorldPt = dddMap.screen2geo(ScreenPt.X , ScreenPt.Y);
            var shbsdo = DBManager.Instance.GetEntities<Gg_jx_shbd_pt_sdogeom>(o => o.G3E_FID == g3efid && o.EntityState != EntityState.Delete).FirstOrDefault();
            var mpValue = new Multipoint();
            mpValue.Points.Add(new ElectronTransferModel.Geo.Point(new[] { WorldPt[1], WorldPt[0], 0 }));
            mpValue.Points.Add(new ElectronTransferModel.Geo.Point(new[] { Math.Cos(0.0), Math.Sin(0.0), 0 }));
            if (shbsdo != null)
            {
                shbsdo.G3E_GEOMETRY = mpValue;
                if (shbsdo.EntityState != EntityState.Insert)
                {
                    shbsdo.EntityState = EntityState.Update;
                }
                DBManager.Instance.Update(shbsdo);
                var pb = pbs.FirstOrDefault(o => o.DyhbFid == g3efid);
                if (pb != null) pb.DyhbPosition = new Point3d(WorldPt[1],WorldPt[0],0);
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
                        if ((int)m.WParam > 0)
                        {
                            dddMap.zoom(1, oldx, oldy, true);
                        }
                        else
                        {
                            dddMap.zoom(-1, oldx, oldy, true);
                        }

                        picWwidth = (int)(picMapWidth / (dddMap.ScreenMapMaxX - dddMap.ScreenMapMinX) * wWidth);
                        picWheight = (int)(picMapHeight / (dddMap.ScreenMapMaxY - dddMap.ScreenMapMinY_In) * wHeight);
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
            catch (Exception ex) { }
        }

        private void SHBMap_MouseDown(object sender, MouseEventArgs e)
        {
            ismousedown = true;

            foreach (var pb in pbs)
            {
                pb.Visible = false;
            }
            draw();
        }

        private void SHBMap_MouseMove(object sender, MouseEventArgs e)
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
                    double[] xy = dddMap.geo2screen(pb.DyhbPosition.Y, pb.DyhbPosition.X);
                    pb.Location = new System.Drawing.Point((int)(xy[0] - picWwidth / 2.0), (int)(xy[1] - picWheight / 2.0));
                    pb.Width = picWwidth;
                    pb.Height = picWheight;
                }
                issheew = false;
            }
        }

        private void SHBMap_MouseUp(object sender, MouseEventArgs e)
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

            //picWwidth = (int)(picMapWidth / (dddMap.ScreenMapMaxX - dddMap.ScreenMapMinX) * wWidth);
            //picWheight = (int)(picMapHeight / (dddMap.ScreenMapMaxY - dddMap.ScreenMapMinY_In) * wHeight);
            //float picw = (float)(picMapWidth / (dddMap.ScreenMapMaxX - dddMap.ScreenMapMinX) * wWidth);
            //float pich = (float)(picMapHeight / (dddMap.ScreenMapMaxY - dddMap.ScreenMapMinY_In) * wHeight);
            foreach (var pb in pbs)
            {
                double[] xy = dddMap.geo2screen(pb.DyhbPosition.Y, pb.DyhbPosition.X);
                pb.Location = new System.Drawing.Point((int)(xy[0] - picWwidth / 2.0), (int)(xy[1] - picWheight / 2.0));
                pb.Width = picWwidth;
                pb.Height = picWheight;
                pb.Visible = true;
            }
        }

        private void SHBMap_Resize(object sender, EventArgs e)
        {
            wWidth = this.ClientSize.Width;
            wHeight = this.ClientSize.Height;
            drawBackImg();
            draw();
        }

        private void SHBMap_Paint(object sender, PaintEventArgs e)
        {
            drawBackImg();
            draw();
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
                    var tshbgeo = dddMap.screen2geo(mouseLocation.X, mouseLocation.Y);
                    var tt = new SHBEditer(formType.regionAdd, detail_fid, tshbgeo,true) { Tag = mi.Text };
                    if (DialogResult.OK == tt.ShowDialog())
                    {
                        // 加一个散户表，刷新窗口
                        var pb = new Dyhb();
                        pb.Height = picWheight;
                        pb.Width = picWwidth;
                        pb.Location = new System.Drawing.Point((int) mouseLocation.X - picWwidth/2,
                            (int) mouseLocation.Y - picWheight/2);
                        pb.Image = imageList1.Images[0];
                        pb.Tag = tt.g_newFid;
                        pb.ContextMenuStrip = contextMenuStrip2;
                        pb.DyhbPosition = new Point3d(tshbgeo[1], tshbgeo[0], 0);
                        pb.DyhbFid = tt.g_newFid;
                        pb.DyhbDz = tt.AZDZ;
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

        private void MoveSHBItem_Click(object sender, EventArgs e)
        {
            try
            {
                var isLock = DBEntityFinder.Instance.VerifyLTTID(detail_fid);
                if (!isLock)
                {
                    PublicMethod.Instance.AlertDialog("抄表箱没有被工单锁定,不能迁移户表!");
                    Show();
                    return;
                }
                var movept = dddMap.screen2geo(mouseLocation.X, mouseLocation.Y);
                var detail = DBManager.Instance.GetEntities<Detailreference_n>(o => o.G3E_FID == detail_fid && o.EntityState != EntityState.Delete).FirstOrDefault();
                if (detail != null)
                {
                    if (detail.G3E_DETAILID == null) return;
                    var tt = new SHBMove((long)detail.G3E_DETAILID, movept);
                    var res = tt.ShowDialog();
                    //var res = acApi.ShowModalDialog(tt);
                    if (res == DialogResult.OK)
                    {
                        // 加一个散户表，刷新窗口
                        var pb = new Dyhb();
                        pb.Height = picWheight;
                        pb.Width = picWwidth;
                        pb.Location = new System.Drawing.Point((int)mouseLocation.X - picWwidth / 2,
                            (int)mouseLocation.Y - picWheight / 2);
                        pb.Image = imageList1.Images[0];
                        pb.Tag = tt.g3e_Fid;
                        pb.ContextMenuStrip = contextMenuStrip2;
                        pb.DyhbPosition = new Point3d(movept[1], movept[0], 0);
                        pb.DyhbFid = tt.g3e_Fid;
                        pb.DyhbDz = tt.AZDZ;
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

        private void EditSHBItem_Click(object sender, EventArgs e)
        {
            try
            {
                var tfid = (long)contextMenuStrip2.Tag;
                var isLock = DBEntityFinder.Instance.VerifyLTTID(detail_fid);
                var tt = new SHBEditer(formType.regionEdit, tfid, detail_fid, isLock);
                if (DialogResult.OK == tt.ShowDialog())
                {
                    foreach (var t2 in Controls)
                    {
                        var t3 = t2 as Dyhb;
                        if (t3 != null && t3.DyhbFid == tfid)
                        {
                            t3.DyhbDz = tt.AZDZ;
                        }
                    }
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
                    var isLock = DBEntityFinder.Instance.VerifyLTTID(detail_fid);
                    if (!isLock)
                    {
                        PublicMethod.Instance.AlertDialog("该设备没有被工单锁定,不能删除!");
                        return;
                    }

                    //var temshb =
                    //    DBManager.Instance.GetEntities<Gg_jx_shbd_pt>(
                    //        o => o.G3E_FID == tfid && o.EntityState != EntityState.Delete).FirstOrDefault();
                    //if (temshb != null && (temshb.LTT_ID != null && (DBEntityFinder.Instance.VerifyLTTID((decimal)temshb.LTT_ID) == false)))
                    //{
                    //    PublicMethod.Instance.AlertDialog("该设备没有被工单锁定,不能删除!");
                    //    return;
                    //}
                    SHBDeleteManager.DeleteSGB(tfid, new List<DBEntity>());
                    //删除一个户表控件,刷新界面
                    var t1 = Controls;
                    foreach (var t2 in Controls)
                    {
                        var t3 = t2 as Dyhb;
                        if (t3 != null && t3.DyhbFid == tfid)
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

        private void SHBView_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyValue == 27 || e.KeyValue == 13)
                Close();
        }


        void pb_MouseUp(object sender, MouseEventArgs e)
        {
            try
            {
                if (e.Button == MouseButtons.Left && bMove)
                {
                    var pb = sender as Dyhb;
                    if (pb != null)
                    {
                        pbs.Add(pb);
                        var fid = pb.DyhbFid;
                        var pblocation = new System.Drawing.Point(pb.Location.X+picWwidth/2,pb.Location.Y+picWheight/2);
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
        void pb_MouseMove(object sender, MouseEventArgs e)
        {
            try
            {
                if (bMove)
                {
                    var pb = sender as Dyhb;
                    if (pb != null)
                    {
                        var p2 = pb.PointToScreen(e.Location);
                        p2 = PointToClient(p2);
                        p2.X -= picWwidth / 2;
                        p2.Y -= picWheight / 2;
                        pb.Location = p2;
                    }
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
                var pb = sender as Dyhb;
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
        void pb_MouseDoubleClick(object sender, MouseEventArgs e)
        {
            try
            {
                var tfid = (long)((PictureBox)sender).Tag;

                var isLock = DBEntityFinder.Instance.VerifyLTTID(detail_fid);
                var tt = new SHBEditer(formType.regionEdit, tfid,detail_fid, isLock);
                //tt.Text = "修改计量表";
                if (DialogResult.OK == tt.ShowDialog())
                {             
                    foreach (var t2 in Controls)
                    {
                        var t3 = t2 as Dyhb;
                        if (t3 != null && t3.DyhbFid == tfid)
                        {
                            t3.DyhbDz = tt.AZDZ;
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                PublicMethod.Instance.AlertDialog(ex.Message);
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

        private void SHBMap_FormClosed(object sender, FormClosedEventArgs e)
        {
            try
            {
                // 更新详表坐标范围
                var detail =
                    DBManager.Instance.GetEntities<Detailreference_n>(
                        o => o.G3E_FID == detail_fid && o.EntityState != EntityState.Delete).FirstOrDefault();
                if (detail == null) return;
                detail.DETAIL_MBRXLO = (decimal) dddMap.ScreenMapMinX;
                detail.DETAIL_MBRXHIGH = (decimal) dddMap.ScreenMapMaxX;
                detail.DETAIL_MBRYLO = (decimal) dddMap.ScreenMapMinY_In;
                detail.DETAIL_MBRYHIGH = (decimal) dddMap.ScreenMapMaxY;
                if (detail.EntityState != EntityState.Insert)
                {
                    detail.EntityState = EntityState.Update;
                }
                DBManager.Instance.Update(detail);
                // 更新集中抄表箱的户数
                var cbxn =
                    DBManager.Instance.GetEntities<Gg_pd_cbx_n>(
                        o => o.G3E_FID == detail_fid && o.EntityState != EntityState.Delete).FirstOrDefault();
                var shb_count =
                    DBManager.Instance.GetEntities<Gg_jx_shbd_pt>(
                        o => o.G3E_DETAILID == detail.G3E_DETAILID && o.EntityState != EntityState.Delete).Count();
                if (cbxn != null)
                {
                    cbxn.HS = shb_count;
                    if (cbxn.EntityState != EntityState.Insert)
                    {
                        cbxn.EntityState = EntityState.Update;
                    }
                    DBManager.Instance.Update(cbxn);
                }
            }
            catch (Exception exception)
            {
                LogManager.Instance.Error(exception);
            }
            finally
            {
                // 同步属性窗口
                if (shbMapToPanel != null)
                {
                    shbMapToPanel(detail_fno, detail_fid);
                }
            }
        }
    }

    public class Dyhb : PictureBox
    {
        public Point3d DyhbPosition { get; set; }
        public long DyhbFid { get; set; }
        public string DyhbDz { get; set; }
    }

    public static class SHBDeleteManager
    {
        /// <summary>
        /// 获取户表自身属性
        /// </summary>
        /// <param name="g3e_fid"></param>
        /// <returns></returns>
        public static Gg_pd_dyshb_n GetDYSHB_N(long g3e_fid)
        {
            try
            {
                return DBManager.Instance.GetEntity<Gg_pd_dyshb_n>(g3e_fid);
            }
            catch
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
        public static List<DBEntity> BatchDeleteFromJCX(long fid, List<DBEntity> backupDBEntity)
        {
            try
            {
                //获取集抄箱下的所有户表
                var t = DBManager.Instance.GetEntity<Detailreference_n>(fid);
                if (t != null)
                {
                    var ents = DBEntityFinder.Instance.GetDYSHB_PT(t.G3E_DETAILID);
                    if (ents != null)
                    {
                        foreach (long tmp in ents)
                        {
                            DeleteItem(tmp, backupDBEntity);
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
        /// 删除单个户表
        /// </summary>
        /// <param name="fid"></param>
        /// <param name="backupDBEntity"> </param>
        public static List<DBEntity> DeleteItem(long fid, List<DBEntity> backupDBEntity)
        {
            backupDBEntity = DeleteSGB(fid, backupDBEntity);
            return backupDBEntity;
        }

        /// <summary>
        /// 删除计量表
        /// </summary>
        /// <param name="fid">计量表FID</param>
        /// <param name="backupEntity"></param>
        /// <returns></returns>
        public static List<DBEntity> DeleteSGB(long? fid, List<DBEntity> backupEntity)
        {
            try
            {
                // 删除Gg_jx_shbd_pt
                var shbpt = DBManager.Instance.GetEntities<Gg_jx_shbd_pt>(o => o.G3E_FID == fid && o.EntityState != EntityState.Delete).FirstOrDefault();
                if (null != shbpt)
                {
                    backupEntity = DBEntityErased.Instance.GetEntityStateByErasedDBEntity(shbpt, backupEntity);
                }
                // 删除Gg_pd_dyshb_n
                var shbn = SHBMap.GetSHB_N(fid);
                if (null != shbn)
                {
                    backupEntity = DBEntityErased.Instance.GetEntityStateByErasedDBEntity(shbn, backupEntity);
                }
                //// 删除Connectivity_n
                //var shbconn = DBManager.Instance.GetEntities<Connectivity_n>(o => o.G3E_FID == fid && o.EntityState != EntityState.Delete).FirstOrDefault();
                //if (null != shbconn)
                //{
                //    backupEntity = DBEntityErased.Instance.GetEntityStateByErasedDBEntity(shbconn, backupEntity);
                //}
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
            catch (NotExistException ex)
            {
                PublicMethod.Instance.NotExistTable(ex);
                DeleteSGB(fid, backupEntity);
                return backupEntity;
            }
        }
    }
}
