using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;
using System.Drawing.Imaging;

namespace ArxMap.MapFrame
{
    public class CYZMapFrame
    {

        public static void CreateMapFrame()
        {
            Config.CYZMapConfig.cMapConfPtr = new ArxMap.Config.CYZMapConfig();
            if (Config.CYZMapConfig.cMapConfPtr != null)
            {
                if (ElectronTransferModel.Config.MapConfig.Instance.TileType == "4Tree")
                {
                    Config.CYZMapConfig.cMapConfPtr.Init4Tree();
                }
                else if (ElectronTransferModel.Config.MapConfig.Instance.TileType == "ZoomWayScale")
                {
                    Config.CYZMapConfig.cMapConfPtr.InitZoomwayScale();
                }
            }
            cMapFramePtr = new CYZMapFrame(Config.CYZMapConfig.cMapConfPtr);

        }

        public static CYZMapFrame cMapFramePtr;

        public CYZMapFrame(Config.CYZMapConfig _cMapConfPtr)
        {
            __cMapConfPtr = _cMapConfPtr;

            mTiles = new List<CYZTile>();
            mTiles_back = new List<CYZTile>();
        }
        /**
        * @初始化
        */
        public void initMap(double _SceenMapMinX, double _SceenMapMaxX,
            double _SceenMapMinY, double _SceenMapMaxY,
            int _ScreenWidth, int _ScreenHeight)
        {
            if (createScale(_SceenMapMinX, _SceenMapMaxX, _SceenMapMinY, _SceenMapMaxY, _ScreenWidth, _ScreenHeight))
            {
                create();
                showmap();
            }
        }

        public void showmap()
        {
            //foreach (CYZTile ct in mTiles_back) 
            //{
            //    ct._ShowTileFunc();            
            //}   

            try
            {
                if (cmap != null)
                {
                    cmap.removeobj();
                    if (File.Exists(cmap.ImgFullName))
                    {
                        File.Delete(cmap.ImgFullName);
                    }
                }
                cmap = new ArxMap.Acad.CYZLoadMap();

                cmap.ImgName = "gd.png";
                cmap.ImgFullName = imgName;

                cmap.newImg();

                //高斯投影 正算
                //从 wgs84坐标系 投影到 cad的坐标系(区域投影坐标)
                //double[] xyMin = Projection.gauss.Instance.qy_gauss_zs(tileMinY, tileMinX);
                //double[] xyMax = Projection.gauss.Instance.qy_gauss_zs(tileMaxY, tileMaxX);

                //cmap.setImgScale((xyMax[1] - xyMin[1]) / tilesWidth, (xyMax[0] - xyMin[0]) / tilesHeight, 1.0);

                //cmap.setImgPosition(xyMin[1], xyMin[0], 0);

                cmap.setImgScale((tileMaxX - tileMinX) / tilesWidth, (tileMaxY - tileMinY) / tilesHeight, 1.0);

                cmap.setImgPosition(tileMinX, tileMinY, 0);

                cmap.MoveTop(false);
            }
            catch (Exception ex) { }
        }

        string imgName = "";
        static uint imgIndex = 0;
          
        Acad.CYZLoadMap cmap = null;

        /**
        * @当前屏幕是需要显示的瓦片
        */
        public List<CYZTile> getTiles() { return mTiles; }
        /**
        * @当前屏幕是需要显示的后台瓦片
        */
        public List<CYZTile> getTiles_back() { return mTiles_back; }


        /**
         * @制作瓦片
         */
        private void create()
        {

            //左上角
            double dx = SceenMapMinX - __cMapConfPtr.MapMinX;
            MinC = (int)Math.Floor(dx / (TileLevel.xResolution * TileLevel.PicWidth));
            tileMinX = MinC * (TileLevel.xResolution * TileLevel.PicWidth) + __cMapConfPtr.MapMinX;
           
            double dy = __cMapConfPtr.MapMaxY - SceenMapMaxY;
            MinR = (int)Math.Floor(dy / (TileLevel.yResolution * TileLevel.PicHeight));
            tileMaxY = __cMapConfPtr.MapMaxY - MinR * (TileLevel.yResolution * TileLevel.PicHeight);

            //右下角
            dx = SceenMapMaxX - __cMapConfPtr.MapMinX;
            MaxC = (int)Math.Floor(dx / (TileLevel.xResolution * TileLevel.PicWidth));
            tileMaxX = (MaxC + 1) * (TileLevel.xResolution * TileLevel.PicWidth) + __cMapConfPtr.MapMinX;

            dy = __cMapConfPtr.MapMaxY - SceenMapMinY;
            MaxR = (int)Math.Floor(dy / (TileLevel.yResolution * TileLevel.PicHeight));
            tileMinY = __cMapConfPtr.MapMaxY - (MaxR + 1) * (TileLevel.yResolution * TileLevel.PicHeight);

            tilesWidth = (MaxC - MinC+1) * TileLevel.PicWidth;
            tilesHeight = (MaxR - MinR+1) * TileLevel.PicHeight;


            List<int> rows = new List<int>();
            int rrmin = (MaxR + MinR) / 2;
            int rrmax = (MaxR + MinR) / 2 + 1;
            while (rrmin >= MinR || rrmax <= MaxR)
            {
                if (rrmin >= MinR)
                {
                    rows.Add(rrmin);
                }
                rrmin--;
                if (rrmax <= MaxR)
                {
                    rows.Add(rrmax);
                }
                rrmax++;
            }

            List<int> cols = new List<int>();
            int ccmin = (MaxC + MinC) / 2;
            int ccmax = (MaxC + MinC) / 2 + 1;
            while (ccmin >= MinC || ccmax <= MaxC)
            {
                if (ccmin >= MinC)
                {
                    cols.Add(ccmin);
                }
                ccmin--;
                if (ccmax <= MaxC)
                {
                    cols.Add(ccmax);
                }
                ccmax++;
            }

            //foreach (CYZTile ct in mTiles_back)
            //{
            //    ct._RemoveTileFunc();
            //}
            mTiles_back.Clear();


            /*	if(mTiles!=NULL)
                {
                    iter=mTiles->begin();
                    while(iter!=mTiles->end())
                    {
                        if(iter->HideTileFunc!=NULL){
                            iter->HideTileFunc(&(*iter));
                        }
                        iter->_HideTileFunc();

                        iter++;
                    }
                }*/


            string str;
            for (int i = 0; i < rows.Count; i++)
            {
                for (int j = 0; j < cols.Count; j++)
                {
                    CYZTile ctile = new CYZTile();
                    ctile.TileHeight = TileLevel.PicHeight;
                    ctile.TileWidth = TileLevel.PicWidth;

                    ctile.TileMinX = __cMapConfPtr.MapMinX + cols[j] * (TileLevel.xResolution * TileLevel.PicWidth);
                    ctile.TileMaxX = __cMapConfPtr.MapMinX + (cols[j] + 1) * (TileLevel.xResolution * TileLevel.PicWidth);

                    ctile.TileMaxY = __cMapConfPtr.MapMaxY - rows[i] * (TileLevel.yResolution * TileLevel.PicHeight);
                    ctile.TileMinY = __cMapConfPtr.MapMaxY - (rows[i] + 1) * (TileLevel.yResolution * TileLevel.PicHeight);

                    ctile.TileCol = cols[j];
                    ctile.TileRow = rows[i];
                    ctile.TileLevel = TileLevel.LevelID;

                    ctile.xResolution = TileLevel.xResolution;
                    ctile.yResolution = TileLevel.yResolution;

                    ctile.basedir = __cMapConfPtr.BaseDir;

                    ctile.getImageHLHUrl();
                    ctile.getImageName();
                    ctile.getImageWMSUrl();


                    if (File.Exists(ctile.getImageHLHUrl()))
                    {
                        mTiles_back.Add(ctile);
                    }

                    

                }
            }
            threadobj tobj = new threadobj();
            tobj.MinC = MinC;
            tobj.MaxC = MaxC;
            tobj.MinR = MinR;
            tobj.MaxR = MaxR;
            tobj.mTiles_back = mTiles_back;

            tttttt(tobj);
        }
        /**
         * @初始化比例尺
         */
        private bool createScale(double _SceenMapMinX, double _SceenMapMaxX,
        double _SceenMapMinY, double _SceenMapMaxY,
        int _ScreenWidth, int _ScreenHeight)
        {
            //高斯投影 反算
            //从cad的坐标系(区域投影坐标) 反投影到wgs84坐标系

            //double[] bl = Projection.gauss.Instance.qy_gauss_fs(_SceenMapMinX, _SceenMapMinY);
            //SceenMapMinX = bl[1];
            //SceenMapMinY = bl[0];
            //bl = Projection.gauss.Instance.qy_gauss_fs(_SceenMapMaxX, _SceenMapMaxY);
            //SceenMapMaxX = bl[1];
            //SceenMapMaxY = bl[0];

            SceenMapMinX = _SceenMapMinX;
            SceenMapMinY = _SceenMapMinY;
            SceenMapMaxX = _SceenMapMaxX;
            SceenMapMaxY = _SceenMapMaxY;


            ScreenWidth = _ScreenWidth;
            ScreenHeight = _ScreenHeight;

            Resolution = (SceenMapMaxX - SceenMapMinX) / (double)ScreenWidth;
            Scale = (Resolution * 96 * Config.CYZMapConfig.DD) / 0.0254;

            if (__cMapConfPtr.getMapLevels().Count == __cMapConfPtr.Levels)
            {
                if (__cMapConfPtr.Levels <= 0)
                {
                    return false;
                }
                if (Scale >= __cMapConfPtr.getMapLevels()[0].Scale * 4.0)
                {
                    return false;
                }
                else if (Scale <= __cMapConfPtr.getMapLevels()[__cMapConfPtr.getMapLevels().Count - 1].Scale * 0.3)
                {
                    return false;
                }
                else if (Scale >= __cMapConfPtr.getMapLevels()[0].Scale &&
                    Scale < __cMapConfPtr.getMapLevels()[0].Scale * 4.0
                    )
                {
                    TileLevel = __cMapConfPtr.getMapLevels()[0];
                    return true;
                }
                else if (Scale <= __cMapConfPtr.getMapLevels()[__cMapConfPtr.getMapLevels().Count - 1].Scale &&
                    Scale > __cMapConfPtr.getMapLevels()[__cMapConfPtr.getMapLevels().Count - 1].Scale * 0.3)
                {
                    TileLevel = __cMapConfPtr.getMapLevels()[__cMapConfPtr.getMapLevels().Count - 1];
                    return true;
                }
                else
                {
                    if (__cMapConfPtr.getMapLevels().Count == 1)
                    {
                        TileLevel = __cMapConfPtr.getMapLevels()[0];
                        return true;
                    }
                    else
                    {
                        for (int i = 1; i < __cMapConfPtr.getMapLevels().Count; i++)
                        {
                            double scaletemp = __cMapConfPtr.getMapLevels()[i].Scale +
                                (__cMapConfPtr.getMapLevels()[i - 1].Scale - __cMapConfPtr.getMapLevels()[i].Scale) * 2.0 / 3.0;
                            if (Scale >= scaletemp)
                            {
                                TileLevel = __cMapConfPtr.getMapLevels()[i - 1];
                                return true;
                            }
                            else if (Scale >= __cMapConfPtr.getMapLevels()[i].Scale &&
                                Scale < scaletemp)
                            {
                                TileLevel = __cMapConfPtr.getMapLevels()[i];
                                return true;
                            }
                            else
                            {
                                continue;
                            }
                        }
                    }
                }
            }
            else
            {
                return false;
            }

            return false;
        }

        private void swaptile()
        { }

        private void tttttt(object oo) 
        {
            try
            {
                threadobj tobj = (threadobj)oo;
                System.Drawing.Image img = new System.Drawing.Bitmap((tobj.MaxC - tobj.MinC + 1) * ElectronTransferModel.Config.MapConfig.Instance.PicWidth,
                    (tobj.MaxR - tobj.MinR + 1) * ElectronTransferModel.Config.MapConfig.Instance.PicHeight);

                
                
                System.Drawing.Graphics g = System.Drawing.Graphics.FromImage(img);
                g.Clear(System.Drawing.Color.White);
                System.Drawing.Image imgtemp;
                foreach (CYZTile ct in tobj.mTiles_back) {
                    if (File.Exists(ct._ImageHLHUrl))
                    {
                        imgtemp = System.Drawing.Image.FromFile(ct._ImageHLHUrl);
                        g.DrawImage(imgtemp, (ct.TileCol - tobj.MinC) * ((int)ct.TileWidth), (ct.TileRow - tobj.MinR) * ((int)ct.TileHeight), (int)ct.TileWidth, (int)ct.TileHeight);
                        imgtemp.Dispose();
                    }
                }

                //int txmin = (int)((ElectronTransferModel.Config.MapConfig.Instance.ProjectionMinX - tileMinX) / TileLevel.xResolution);
                //int txmax = (int)((ElectronTransferModel.Config.MapConfig.Instance.ProjectionMaxX - tileMinX) / TileLevel.xResolution);
                //int tymin = (int)(( tileMaxY- ElectronTransferModel.Config.MapConfig.Instance.ProjectionMinY ) / TileLevel.xResolution);
                //int tymax = (int)(( tileMaxY- ElectronTransferModel.Config.MapConfig.Instance.ProjectionMaxY ) / TileLevel.xResolution);

                //g.DrawPolygon(new System.Drawing.Pen(System.Drawing.Color.Red, 1.0f), 
                //    new System.Drawing.Point[] { 
                //    new System.Drawing.Point(txmin,tymin),
                //    new System.Drawing.Point(txmax,tymin),
                //    new System.Drawing.Point(txmax,tymax),
                //    new System.Drawing.Point(txmin,tymax),
                //    new System.Drawing.Point(txmin,tymin)                    
                //    }                    
                //    );

              


                imgName = System.AppDomain.CurrentDomain.BaseDirectory + (imgIndex++).ToString() + "fuck.png";
                img.Save(imgName,ImageFormat.Png);

                g.Dispose();
                img.Dispose();
                
            }
            catch (Exception ex) { }
        }

        /**
        * @当前屏幕可见地图的地理范围	
        */
        private double SceenMapMinX, SceenMapMaxX;
        private double SceenMapMinY, SceenMapMaxY;

        /**
        * @ 当前屏幕大小	
        */
        private int ScreenWidth;
        private int ScreenHeight;

        /**
         * @当前比例尺
         */
        private double Scale;

        /**
         * @每像素地理范围
         *  分辨率
         */
        private double Resolution;

        /**
        * @当前屏幕左上角的瓦片行列号
        */
        private int MaxR, MinC;
        /**
        * @当前屏幕右下角的瓦片行列号
        */
        private int MinR, MaxC;
        /// <summary>
        /// 所有瓦片中最大最小经纬度
        /// </summary>
        private double tileMinX, tileMaxX, tileMinY, tileMaxY;
        private double tilesWidth, tilesHeight;



        /**
        * @当前屏幕是需要显示的瓦片
        */
        private List<CYZTile> mTiles;
        /**
        * @当前屏幕是需要显示的后台瓦片
        */
        private List<CYZTile> mTiles_back;




        /**
         * @与当前比例尺最接近的瓦片图层
         */
        Config.CYZLevelConfig TileLevel;

        /**
         * @地图配置参数
         */
        Config.CYZMapConfig __cMapConfPtr;
    }


    public class threadobj 
    {
        ///// <summary>
        ///// 当前屏幕左下角的瓦片的左下经纬度
        ///// </summary>
        //public double picMinX;
        //public double picMinY;
        ///// <summary>
        ///// 当前屏幕右上角的瓦片的右上经纬度
        ///// </summary>
        //public double picMaxX, picMaxY;

        /**
     * @当前屏幕左下角的瓦片行列号
     */
        public int MaxR, MinC;
        /**
        * @当前屏幕右上角的瓦片行列号
        */
        public int MinR, MaxC;

        public List<CYZTile> mTiles_back;
    }
}
