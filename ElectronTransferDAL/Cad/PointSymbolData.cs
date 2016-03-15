using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Autodesk.AutoCAD.DatabaseServices;
using Autodesk.AutoCAD.Geometry;
using ElectronTransferDal.Query;
using CADColor = Autodesk.AutoCAD.Colors.Color;

namespace ElectronTransferDal.Cad
{
    public class PointSymbolData
    {
        /// <summary>
        /// G3E系列ID
        /// </summary>
        public G3EObject g3eObject { set; get; }
        /// <summary>
        /// 块定义名称
        /// </summary>
        public string blockName { set; get; }
        /// <summary>
        /// 角度
        /// </summary>
        public double rotateAngle { set; get; }
        /// <summary>
        /// 颜色
        /// </summary>
        public CADColor color { set; get; }
        /// <summary>
        /// 插入点
        /// </summary>
        public Point3d point { set; get; }
        /// <summary>
        /// 对象ID
        /// </summary>
        public ObjectId objectId { set; get; }
    }
}
