using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Autodesk.AutoCAD.DatabaseServices;
using Autodesk.AutoCAD.Geometry;
using ElectronTransferModel.Base;

namespace ElectronTransferDal.Cad
{
    public class EntityObj
    {
        /// <summary>
        /// 工单ID
        /// </summary>
        public decimal LTT_ID { get; set; }
        /// <summary>
        /// 实体对象
        /// </summary>
        public ObjectId objectID { set; get; }
        /// <summary>
        /// 最大点
        /// </summary>
        public Point3d maxPoint { get; set; }
        /// <summary>
        /// 最小点
        /// </summary>
        public Point3d minPoint { get; set; }
        /// <summary>
        /// 设备名称
        /// </summary>
        public string SBMC { get; set; }
        public int G3e_Fno { get; set; }
        /// <summary>
        /// 对象类型（true有符号、反之）
        /// </summary>
        public bool objectType { set; get; }
        public ElectronSymbol electronSymbol { set; get; }
    }
}
