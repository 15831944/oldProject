using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using ElectronTransferFramework;

namespace ElectronTransferModel.Config
{
    public class MapConfig : ICanSerialize
    {
        public static MapConfig Instance = new MapConfig();

        public string TileType { get; set; }

        /**
        * @全景地图的地理范围	
        */
        public double MapMinX { get; set; }
        public double MapMaxX { get; set; }
        public double MapMinY { get; set; }
        public double MapMaxY { get; set; }

        /**
         * @ 瓦片层数	
         */
        public int Levels { get; set; }

        /**
         * @ 瓦片图片大小	
         */
        public int PicWidth { get; set; }
        public int PicHeight { get; set; }

        /// <summary>
        /// 瓦片地址
        /// </summary>
        public string BaseDir { get; set; }


        public List<double> ZoomWayScale { get; set; }

        /// <summary>
        /// 投影区域
        /// </summary>
        public double ProjectionMinX { get; set; }

        public double ProjectionMaxX { get; set; }
        public double ProjectionMinY { get; set; }
        public double ProjectionMaxY { get; set; }

        public string MySqlConnectString { get; set; }

        /// <summary>
        /// 符号库路径
        /// </summary>
        public string SymbolLibraryPath { get; set; }

        /// <summary>
        /// 符号图片路径
        /// </summary>
        public string SymbolPicturePath { get; set; }

        /// <summary>
        /// 符号库文件夹名称
        /// </summary>
        public string SymbolDirName { get; set; }

        /// <summary>
        /// 中压线
        /// </summary>
        public List<string> ListZyLineSymbol { get; set; }

        /// <summary>
        /// 低压线
        /// </summary>
        public List<string> ListDyLineSymbol { get; set; }

        /// <summary>
        /// 中压面
        /// </summary>
        public List<string> ListZyPolygonSymbol { set; get; }

        /// <summary>
        /// 低压面
        /// </summary>
        public List<string> ListDyPolygonSymbol { set; get; }

        /// <summary>
        /// 
        /// </summary>
        public long LTTID { get; set; }

        /// <summary>
        /// 
        /// </summary>
        public string LTTNAME { get; set; }

        /// <summary>
        /// 
        /// </summary>
        public string GCID { get; set; }

        /// <summary>
        /// 
        /// </summary>
        public string KXMC { get; set; }

        /// <summary>
        /// 显示的标注
        /// </summary>
        public string ListLabelShow { get; set; }
        /// <summary>
        /// 无须校验功能位置名称的设备
        /// </summary>
        public string NoVerifySbmcFeatures { get; set; }
        /// <summary>
        /// 无须校验连接关系的设备
        /// </summary>
        public string NoVerifyConnectivityFeature { get; set; }
        /// <summary>
        /// 无须校验从属关系的设备
        /// </summary>
        public string NoVerifyOwnshipFeature { get; set; }
        /// <summary>
        /// 无须校验台账的设备
        /// </summary>
        public string NoVerifyTzFeature { get; set; } 
        /// <summary>
        /// 需要显示的标注数组
        /// </summary>
        public string[] labels { set; get; }

        public double earthscale = 111319.49079327357264771338267056;
        /// <summary>
        /// 台帐数据包地址
        /// </summary>
        public string TZPacketPath { set; get; }
        /// <summary>
        /// 客户端数据源地址
        /// </summary>
        public string ClientXmlPath { set; get; }
        /// <summary>
        /// 保存退出时的屏幕范围
        /// </summary>
        public List<double> WindowsDefaultLocation { set; get; }
        /// <summary>
        /// 版本号
        /// </summary>
        public int Version { set; get; }

        /// <summary>
        /// 导入增量的服务
        /// </summary>
        public string CadServiceUrl { set; get; }
        /// <summary>
        /// 地图显示
        /// </summary>
        public bool BrowsableMap { set; get; }
        /// <summary>
        /// 时间锁
        /// </summary>
        public string TimeLock { set; get; }
    }
}
