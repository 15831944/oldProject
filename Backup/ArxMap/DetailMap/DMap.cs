using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using ArxMap.Config;

namespace ArxMap.DetailMap
{
    public class DMap
    {
        public static double DD = 111319.49079327357264771338;

        public void createDmap(double _ScreenMapMinX, double _ScreenMapMaxX, double _ScreenMapMinY,
            double _ScreenMapMaxY,
            int _ScreenWidth, int _ScreenHeight)
        {
            createScale(_ScreenMapMinX, _ScreenMapMaxX, _ScreenMapMinY,
                _ScreenMapMaxY,
                _ScreenWidth, _ScreenHeight);
        }
        public void reSize(int _ScreenWidth, int _ScreenHeight)
        {
            ////当前场景中心位置
            createScale(ScreenMapMinX, ScreenMapMaxX, ScreenMapMinY_In, ScreenMapMaxY, _ScreenWidth, _ScreenHeight);

        }
        public void move(double _mousedx, double _mousedy)
        {
            ScreenMapMinX -= xResolution * _mousedx;
            ScreenMapMinY_In += yResolution * _mousedy;
            ScreenMapMaxX -= xResolution * _mousedx;
            ScreenMapMaxY += yResolution * _mousedy;

            createScale(ScreenMapMinX, ScreenMapMaxX, ScreenMapMinY_In, ScreenMapMaxY, ScreenWidth, ScreenHeight);
        }
        /// <summary>
        /// 鼠标滚轮滚动后改变地图场景，地图缩放
        /// 按中心点无级缩放
        /// </summary>
        /// <param name="_inout">鼠标缩放标志</param>
        /// <param name="_isnolevel">是否是无级缩放标志</param>
        public void zoom(int _inout, bool _isnolevel)
        {
            if (_isnolevel)
            {
                //当前场景中心位置
                double midScreenX = (ScreenMapMinX + ScreenMapMaxX) / 2;
                double midScreenY = (ScreenMapMinY_In + ScreenMapMaxY) / 2;
                double dx = (ScreenMapMaxX - ScreenMapMinX) / 2.0;
                double dy = (ScreenMapMaxY - ScreenMapMinY_In) / 2.0;
                if (_inout > 0)
                {
                    ScreenMapMinX = midScreenX - dx / 1.3;
                    ScreenMapMaxX = midScreenX + dx / 1.3;
                    ScreenMapMinY_In = midScreenY - dy / 1.3;
                    ScreenMapMaxY = midScreenY + dy / 1.3;
                }
                else
                {
                    ScreenMapMinX = midScreenX - dx * 1.3;
                    ScreenMapMaxX = midScreenX + dx * 1.3;
                    ScreenMapMinY_In = midScreenY - dy * 1.3;
                    ScreenMapMaxY = midScreenY + dy * 1.3;
                }
                createScale(ScreenMapMinX, ScreenMapMaxX, ScreenMapMinY_In, ScreenMapMaxY, ScreenWidth, ScreenHeight);
            }
            else
            {
                // zoom(_inout);
            }
        }
        /// <summary>
        /// 鼠标滚轮滚动后改变地图场景，地图缩放
        /// 按鼠标位置逐级缩放
        /// </summary>
        /// <param name="_inout">鼠标缩放标志</param>
        /// <param name="_mousex">鼠标位置x</param>
        /// <param name="_mousey">鼠标位置y</param>
        /// <param name="_isnolevel">是否是无级缩放标志</param>
        public void zoom(int _inout, double _mousex, double _mousey, bool _isnolevel)
        {
            if (_isnolevel)
            {
                //鼠标所在的当前位置
                double mouseMapX = (_mousex / ScreenWidth) * (ScreenMapMaxX - ScreenMapMinX) + ScreenMapMinX;
                double mouseMapY = ((ScreenHeight - _mousey) / ScreenHeight) * (ScreenMapMaxY - ScreenMapMinY_In) + ScreenMapMinY_In;
                //当前场景中心位置
                double midScreenX = mouseMapX;// (ScreenMapMinX + ScreenMapMaxX) / 2.0;
                double midScreenY = mouseMapY;// (ScreenMapMinY + ScreenMapMaxY) / 2.0;
                //当前场景可视范围
                double dx = (ScreenMapMaxX - ScreenMapMinX) / 2.0;
                double dy = (ScreenMapMaxY - ScreenMapMinY_In) / 2.0;
                if (_inout > 0)
                {
                    ScreenMapMinX = midScreenX - dx / 1.3;
                    ScreenMapMaxX = midScreenX + dx / 1.3;
                    ScreenMapMinY_In = midScreenY - dy / 1.3;
                    ScreenMapMaxY = midScreenY + dy / 1.3;
                }
                else
                {
                    ScreenMapMinX = midScreenX - dx * 1.3;
                    ScreenMapMaxX = midScreenX + dx * 1.3;
                    ScreenMapMinY_In = midScreenY - dy * 1.3;
                    ScreenMapMaxY = midScreenY + dy * 1.3;
                }

                xResolution = (ScreenMapMaxX - ScreenMapMinX) / (double)ScreenWidth;
                xScale = (xResolution * 96 * DD) / 0.0254;

                //yResolution = xResolution;
                //yScale = xScale;
                yResolution = (ScreenMapMaxY - ScreenMapMinY_In) / (double)ScreenHeight;
                yScale = (yResolution * 96 * DD) / 0.0254;

                ScreenMapMinX -= xResolution * (_mousex - ScreenWidth / 2.0);
                ScreenMapMinY_In += yResolution * (_mousey - ScreenHeight / 2.0);
                ScreenMapMaxX -= xResolution * (_mousex - ScreenWidth / 2.0);
                ScreenMapMaxY += yResolution * (_mousey - ScreenHeight / 2.0);

                createScale(ScreenMapMinX, ScreenMapMaxX, ScreenMapMinY_In, ScreenMapMaxY, ScreenWidth, ScreenHeight);
            }
        }
        private bool createScale(double _ScreenMapMinX, double _ScreenMapMaxX, double _ScreenMapMinY, double _ScreenMapMaxY,
       int _ScreenWidth, int _ScreenHeight)
        {
            ScreenMapMinX = _ScreenMapMinX;
            ScreenMapMinY_In = _ScreenMapMinY;
            ScreenMapMaxX = _ScreenMapMaxX;
            ScreenMapMaxY = _ScreenMapMaxY;

            ScreenWidth = _ScreenWidth;
            ScreenHeight = _ScreenHeight;

            xResolution = (ScreenMapMaxX - ScreenMapMinX) / (double)ScreenWidth;
            xScale = (xResolution * 96 * DD) / 0.0254;

            //yResolution = xResolution;
            //yScale = xScale;
            yResolution = (ScreenMapMaxY - ScreenMapMinY_In) / (double)ScreenHeight;
            yScale = (yResolution * 96 * DD) / 0.0254;



            //ScreenMapMinY = ScreenMapMaxY - Resolution * ScreenHeight;
            //ScreenMapMinY_In = ScreenMapMaxY -yResolution * ScreenHeight;

            return false;
        }
        /// <summary>
        /// 获取鼠标所在位置的地理位置
        /// </summary>
        /// <param name="_x">鼠标横向坐标</param>
        /// <param name="_y">鼠标纵向坐标</param>
        /// <returns>返回纬度(纵向),经度(横向)</returns> 
        public double[] screen2geo(double _x, double _y)
        {
            double[] bl = new double[2];
            bl[1] = (ScreenMapMaxX - ScreenMapMinX) * (_x / (double)ScreenWidth) + ScreenMapMinX;
            bl[0] = ScreenMapMaxY - (ScreenMapMaxY - ScreenMapMinY_In) * (_y / (double)ScreenHeight);
            return bl;
        }
        /// <summary>
        /// 从地理位置获取屏幕所在位置
        /// </summary>
        /// <param name="_B">纬度(纵向)</param>
        /// <param name="_L">经度(横向)</param>
        /// <returns>返回 屏幕坐标 x(横向向右)，y(纵向向下)</returns>
        public double[] geo2screen(double _B, double _L)
        {
            var xy = new double[] { 0, 0 };
            if (ScreenMapMaxY != ScreenMapMinY_In)
                xy[1] = ScreenHeight - ((_B - ScreenMapMinY_In) * ScreenHeight / (ScreenMapMaxY - ScreenMapMinY_In));
            if (ScreenMapMaxX != ScreenMapMinX)
                xy[0] = (_L - ScreenMapMinX) * ScreenWidth / (ScreenMapMaxX - ScreenMapMinX);
            return xy;
        }
        ///// <summary>
        ///// 获取鼠标所在位置的地理位置
        ///// </summary>
        ///// <param name="_x">鼠标横向坐标</param>
        ///// <param name="_y">鼠标纵向坐标</param>
        ///// <returns>返回纬度(纵向),经度(横向)</returns> 
        //public double[] screen2geo_y(double _x, double _y)
        //{
        //    double[] bl = new double[2];
        //    bl[1] = (ScreenMapMaxX - ScreenMapMinX) * (_x / (double)ScreenWidth) + ScreenMapMinX;
        //    bl[0] = ScreenMapMaxY - (ScreenMapMaxY - ScreenMapMinY_In) * (_y / (double)ScreenHeight);
        //    return bl;
        //}
        ///// <summary>
        ///// 从地理位置获取屏幕所在位置
        ///// </summary>
        ///// <param name="_B">纬度(纵向)</param>
        ///// <param name="_L">经度(横向)</param>
        ///// <returns>返回 屏幕坐标 x(横向向右)，y(纵向向下)</returns>
        //public double[] geo2screen_y(double _B, double _L)
        //{
        //    var xy = new double[] { 0, 0 };
        //    if (ScreenMapMaxY != ScreenMapMinY_In)
        //        xy[1] = ScreenHeight - ((_B - ScreenMapMinY_In) * ScreenHeight / (ScreenMapMaxY - ScreenMapMinY_In));
        //    if (ScreenMapMaxX != ScreenMapMinX)
        //        xy[0] = (_L - ScreenMapMinX) * ScreenWidth / (ScreenMapMaxX - ScreenMapMinX);
        //    return xy;
        //}


        /// <summary>
        /// 当前屏幕可见地图的地理范围
        /// </summary>
        public double ScreenMapMinX, ScreenMapMaxX, ScreenMapMaxY;
        /// <summary>
        /// 当前屏幕可见地图的地理范围(纵横等分辨率)
        /// </summary>
        //public double ScreenMapMinY;
        /// <summary>
        /// 当前屏幕可见地图的地理范围(用户输入的范围)
        /// </summary>
        public double ScreenMapMinY_In;
        /// <summary>
        /// 当前屏幕大小
        /// </summary>
        public int ScreenWidth, ScreenHeight;
        /// <summary>
        /// 当前比例尺
        /// </summary>
        public double xScale;
        public double yScale;
        /// <summary>
        /// 每像素地理范围
        /// 分辨率
        /// </summary>
        public double xResolution;
        public double yResolution;


    }
}
