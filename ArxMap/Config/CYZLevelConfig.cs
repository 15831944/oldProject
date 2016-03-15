using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ArxMap.Config
{
    public class CYZLevelConfig
    {	
        /**
		 * @瓦片的层号
		 */
        public int LevelID;

        /**
         * @ 瓦片图片大小	
         */
        public int PicWidth;
        public int PicHeight;

        /**
         * @瓦片的比例尺
         */
        public double Scale;

        /**
         * @瓦片的每像素地理范围
         *  瓦片分辨率
         */
        public double xResolution;
        public double yResolution;
    }
}