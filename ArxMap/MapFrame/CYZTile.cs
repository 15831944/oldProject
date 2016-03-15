using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;

namespace ArxMap.MapFrame
{
    public class CYZTile
    {
        public CYZTile() {
            cmap = new ArxMap.Acad.CYZLoadMap();
        }
        /**
         * @瓦片的地理范围	
         */
        public double TileMinX, TileMaxX;
        public double TileMinY, TileMaxY;

        /**
         * @瓦片的图片大小	
         */
        public double TileWidth, TileHeight;

        /**
         * @瓦片的行列号,层号
         */
        public int TileRow, TileCol, TileLevel;

        /**
         * @瓦片的比例尺
         */
        public double TileScale;

        /**
         * @瓦片的每像素地理范围
         *  瓦片分辨率
         */
        public double xResolution;
        public double yResolution;

        public string basedir;
        public string baseurl;



        public string getImageHLHUrl()
        {
            string ss="";
            basedir = basedir.Replace("\\", "/");
            ss = basedir;

            if (ElectronTransferModel.Config.MapConfig.Instance.TileType == "4Tree")
            {
                //ss = basedir + "/" + TileLevel + "/" + TileRow + "/" + TileLevel + "_" + TileRow + "_" + TileCol + ".png";
                ss = ss.Replace("{TileLevel}", TileLevel.ToString());
                ss = ss.Replace("{TileRow}", TileRow.ToString());
                ss = ss.Replace("{TileCol}", TileCol.ToString());
            }
            else if (ElectronTransferModel.Config.MapConfig.Instance.TileType == "ZoomWayScale")
            {
                ss = ss.Replace("{TileLevel}", TileLevel.ToString());
                ss = ss.Replace("{TileRow1}", (TileRow / 30 * 30).ToString());
                ss = ss.Replace("{TileRow2}", (TileRow % 30).ToString());
                ss = ss.Replace("{TileCol1}", (TileCol / 30 * 30).ToString());
                ss = ss.Replace("{TileCol2}", (TileCol % 30).ToString());
            }
           
            
            
            ImageHLHUrl = ss;
            return ImageHLHUrl;

        }
        public string getImageWMSUrl()
        {
            //stringstream ss;
            //ss<<TileLevel<<"/"<<TileRow<<"/"<<TileLevel<<"_"<<TileRow<<"_"<<TileCol<<".png";
            //ss>>ImageName;
            return ImageWMSUrl;
        }
        
        static int ii=0;
        public string getImageName()
        {
            string ss;
            ss = (ii++).ToString() + "_" + TileLevel + "_" + TileRow + "_" + TileCol + ".png";
            ImageName = ss;
            return ImageName;
        }


        Acad.CYZLoadMap cmap;
        public Acad.CYZLoadMap _cmap { get { return cmap; } }



        /**
         * @瓦片的行列号名称
         */
        private string ImageHLHUrl = "";
        public string _ImageHLHUrl { get { return ImageHLHUrl; } }
        /**
         * @瓦片的范围名称
         */
        private string ImageWMSUrl = "";
        public string _ImageWMSUrl { get { return ImageWMSUrl; } }
        /**
         * @瓦片的名称
         */
        private string ImageName = "";
        public string _ImageName { get { return ImageName; } }


        /**
         * @瓦片在某个具体环境中的实体
         *  如opengl, directx, gdi, cad等.
         */
        //private	void* tagTile;

        /**
     * @瓦片在某个具体环境中的展示方式
     *  如opengl, directx, gdi, cad等.
     */
        //public	void (*ShowTileFunc)(void*);
        public void _ShowTileFunc()
        {
            
            //Thread tt = new Thread(new ParameterizedThreadStart(tttttt));
            //tt.IsBackground = true;
            
            //tt.Start(this);


            _cmap.ImgName = _ImageName;
            _cmap.ImgFullName = _ImageHLHUrl;

            _cmap.newImg();

            _cmap.setImgScale((TileMaxX - TileMinX) / TileWidth, (TileMaxY - TileMinY) / TileHeight, 1.0);

            _cmap.setImgPosition(TileMinX, TileMinY, 0);

            _cmap.MoveTop(true);
            
        }

        //void tttttt(object oo) 
        //{
        //    try
        //    {
        //        CYZTile _ct = (CYZTile)oo;


        //        _ct._cmap.ImgName = _ct._ImageName;
        //        _ct._cmap.ImgFullName =_ct._ImageHLHUrl;
              
        //        _cmap.newImg();

        //        _ct._cmap.setImgScale((_ct.TileMaxX - _ct.TileMinX) / _ct.TileWidth, (_ct.TileMaxY - _ct.TileMinY) / _ct.TileHeight, 1.0);

        //        _ct._cmap.setImgPosition(TileMinX, TileMinY, 0);

        //        _ct._cmap.MoveTop(true);
        //    }
        //    catch (Exception ex) 
        //    {
            
        //    }
        //}

        /**
         * @瓦片在某个具体环境中的隐藏方式
         *  如opengl, directx, gdi, cad等.
         */
        //public	void (*HideTileFunc)(void*);
        public void _HideTileFunc() {
            cmap.MoveTop(false);
        }

        /**
         * @瓦片在某个具体环境中的移除方式
         *  如opengl, directx, gdi, cad等.
         */
        //public	void (*RemoveTileFunc)(void*);
        public void _RemoveTileFunc() {
            cmap.removeobj();
        }


    }
}
