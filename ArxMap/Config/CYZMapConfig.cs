using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;

using System.Reflection;
using System.Xml;


namespace ArxMap.Config
{
    public class CYZMapConfig
    {
        public static double DD = 111319.49079327357264771338;

        public static CYZMapConfig cMapConfPtr;

        public CYZMapConfig()
        {
            MapMinX = 0.0;
            MapMaxX = 0.0;
            MapMinY = 0.0;
            MapMaxY = 0.0;
        }
        /**
         * @全景地图的地理范围	
         */
        public double MapMinX, MapMaxX;
        public double MapMinY, MapMaxY;

        /**
         * @ 瓦片层数	
         */
        public int Levels;

        /**
         * @ 瓦片图片大小	
         */
        public int PicWidth;
        public int PicHeight;


        public string BaseDir;

        /**
         * @4叉树切片
         */
        public void Init4Tree()
        {
            MapMinX = ElectronTransferModel.Config.MapConfig.Instance.MapMinX;
            MapMaxX = ElectronTransferModel.Config.MapConfig.Instance.MapMaxX;
            MapMinY = ElectronTransferModel.Config.MapConfig.Instance.MapMinY;
            MapMaxY = ElectronTransferModel.Config.MapConfig.Instance.MapMaxY;

            Levels = ElectronTransferModel.Config.MapConfig.Instance.Levels;
            PicWidth = ElectronTransferModel.Config.MapConfig.Instance.PicWidth;
            PicHeight = ElectronTransferModel.Config.MapConfig.Instance.PicHeight;

            BaseDir = ElectronTransferModel.Config.MapConfig.Instance.BaseDir;


            if (MapMinX == 0.0 && MapMaxX == 0.0)
            {
                return;
            }
            if (MapMinY == 0.0 && MapMaxY == 0.0)
            {
                return;
            }
            if (MapMinX == MapMaxX)
            {
                return;
            }
            if (MapMinY == MapMaxY)
            {
                return;
            }
            if (Levels == 0)
            {
                return;
            }

            MapLevels = new List<CYZLevelConfig>();
            for (int i = 0; i < Levels; i++)
            {
                CYZLevelConfig clc = new CYZLevelConfig();
                clc.LevelID = i;
                clc.PicWidth = PicWidth;
                clc.PicHeight = PicHeight;
                clc.xResolution = (MapMaxX - MapMinX) / (Math.Pow(2.0, (double)i) * clc.PicWidth);
                clc.yResolution = (MapMaxY - MapMinY) / (Math.Pow(2.0, (double)i) * clc.PicHeight);

                clc.Scale = clc.xResolution * DD * 96 / 0.0254;

                MapLevels.Add(clc);
            }
        }
        /**
         * @4ZoomwayScale
         */
        public void InitZoomwayScale()
        {
            MapMinX = ElectronTransferModel.Config.MapConfig.Instance.MapMinX;
            MapMaxX = ElectronTransferModel.Config.MapConfig.Instance.MapMaxX;
            MapMinY = ElectronTransferModel.Config.MapConfig.Instance.MapMinY;
            MapMaxY = ElectronTransferModel.Config.MapConfig.Instance.MapMaxY;

            Levels = ElectronTransferModel.Config.MapConfig.Instance.Levels;
            if (ElectronTransferModel.Config.MapConfig.Instance.ZoomWayScale != null) 
            {
                Levels = ElectronTransferModel.Config.MapConfig.Instance.ZoomWayScale.Count;
            }
            PicWidth = ElectronTransferModel.Config.MapConfig.Instance.PicWidth;
            PicHeight = ElectronTransferModel.Config.MapConfig.Instance.PicHeight;

            BaseDir = ElectronTransferModel.Config.MapConfig.Instance.BaseDir;


            if (MapMinX == 0.0 && MapMaxX == 0.0)
            {
                return;
            }
            if (MapMinY == 0.0 && MapMaxY == 0.0)
            {
                return;
            }
            if (MapMinX == MapMaxX)
            {
                return;
            }
            if (MapMinY == MapMaxY)
            {
                return;
            }
            if (Levels == 0)
            {
                return;
            }

            MapLevels = new List<CYZLevelConfig>();
            for (int i = ElectronTransferModel.Config.MapConfig.Instance.ZoomWayScale.Count-1; i >=0; i--)
            {
                CYZLevelConfig clc = new CYZLevelConfig();
                clc.LevelID = ElectronTransferModel.Config.MapConfig.Instance.ZoomWayScale.Count-1-i;
                clc.PicWidth = PicWidth;
                clc.PicHeight = PicHeight;
                clc.xResolution = ElectronTransferModel.Config.MapConfig.Instance.ZoomWayScale[i] * 0.0254 / DD / 96;
                clc.yResolution = clc.xResolution;

                clc.Scale = ElectronTransferModel.Config.MapConfig.Instance.ZoomWayScale[i];

                MapLevels.Add(clc);
            }
        }

        /**
        * @获取瓦片比例尺
        */
        public List<CYZLevelConfig> getMapLevels()
        {
            return MapLevels;
        }

        private List<CYZLevelConfig> MapLevels;

        /// <summary>
        /// 投影区域
        /// </summary>
        //public double ProjectionMinX, ProjectionMaxX, ProjectionMinY, ProjectionMaxY;
    }
}