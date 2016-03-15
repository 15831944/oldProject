using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Autodesk.AutoCAD.ApplicationServices;
using Autodesk.AutoCAD.DatabaseServices;
using Autodesk.AutoCAD.EditorInput;
using Autodesk.AutoCAD.Geometry;
using Autodesk.AutoCAD.GraphicsInterface;
using DotNetARX;

namespace ElectronTransferDal.Cad
{
    public class measurejig : DrawJig
    {
        // 地球半径，假设地球是规则的球体
        private const double EARTH_RADIUS = 6378.137;
        private const double PI = 3.1415926;
        
        // 记录上一个点
        private Point3d _prePosition;
        // 记录当前点
        private Point3d _curPosition;

        public Point3d curPosition
        {
            get { return _curPosition; }
            set { _curPosition = value; }
        }

        public Point3d prePosition
        {
            get { return _prePosition; }
            set { _prePosition = value; }
        }

        public measurejig(Point3d prePosition, Point3d curPosition)
        {
            _prePosition = prePosition;
            _curPosition = curPosition;
        }

        public measurejig()
        {

        }

        protected override bool WorldDraw(WorldDraw draw)
        {
            var wg = draw.Geometry;
            if (wg != null)
            {
                wg.WorldLine(_prePosition, _curPosition);
                //wg.PopModelTransform();
                //wg.PopModelTransform();
            }
            return true;
        }

        protected override SamplerStatus Sampler(JigPrompts prompts)
        {
            var opts = new JigPromptPointOptions();
            opts.UseBasePoint = true;
            opts.Cursor = CursorType.RubberBand;
            opts.BasePoint = prePosition;
            opts.UserInputControls = 
                UserInputControls.GovernedByUCSDetect
                | UserInputControls.NullResponseAccepted
                | UserInputControls.NoZeroResponseAccepted
                | UserInputControls.NoNegativeResponseAccepted;

            opts.Message = "\nSelect point: ";
            var res = prompts.AcquirePoint(opts);

            if (res.Status == PromptStatus.OK)
            {
                if (_prePosition == res.Value)
                {
                    return SamplerStatus.NoChange;
                }
                //_position = res.Value;
                _curPosition = res.Value;
                return SamplerStatus.OK;
            }
            return SamplerStatus.Cancel;
        }

        /**
        *  计算两组经纬度坐标 之间的距离
        *   params ：lat1 纬度1； lng1 经度1； lat2 纬度2； lng2 经度2； len_type （1:m or 2:km);
        *   return m or km
        */
        public double GetDistance(Point3d startPt, Point3d endPt, int len_type)
        {
            return GetDistance(startPt.Y, startPt.X, endPt.Y, endPt.X, len_type);
        }

        private double GetDistance(double lat1, double lng1, double lat2, double lng2, int len_type)
        {
            var radLat1 = lat1 * PI / 180.0; //PI()圆周率
            var radLat2 = lat2 * PI / 180.0;
            var a = radLat1 - radLat2;
            var b = lng1 * (PI / 180.0) - (lng2 * PI / 180.0);
            var s = 2 *Math.Asin(Math.Sqrt(Math.Pow(Math.Sin(a / 2), 2) +Math.Cos(radLat1) * Math.Cos(radLat2) * Math.Pow(Math.Sin(b / 2), 2)));
            s = s * EARTH_RADIUS;
            s = Math.Round(s * 1000,2);
            if (len_type > 1)
            {
                s /= 1000;
            }
            return s;
        }
    }
}
