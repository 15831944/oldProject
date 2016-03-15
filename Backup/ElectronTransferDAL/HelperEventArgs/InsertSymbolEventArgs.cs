using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Autodesk.AutoCAD.Colors;
using Autodesk.AutoCAD.DatabaseServices;
using ElectronTransferFramework;
using ElectronTransferModel;
using ElectronTransferModel.Base;
using ElectronTransferModel.Geo;
using ElectronTransferDal.AutoGeneration;

namespace ElectronTransferDal.HelperEventArgs
{
    public class InsertSymbolEventArgs : ValueEventArgs
    {
        /// <summary>
        /// 保存符号类型和开关状态
        /// </summary>
        public string bTypeStr { set; get; }
        /// <summary>
        /// 所有数据（公共属性、自身属性等等）
        /// </summary>
        public XProps symbolObj { set; get; }
        /// <summary>
        /// 颜色
        /// </summary>
        public Color symbolColor { set; get; }

        /// <summary>
        /// 点设备坐标
        /// </summary>
        public Multipoint multipointValue { set; get; }
        /// <summary>
        /// 点设备坐标
        /// </summary>
        public Multipoint multipointValue1 { set; get; }
        /// <summary>
        /// 线设备坐标
        /// </summary>
        public LineString lineStringValue { set; get; }
        /// <summary>
        /// 面坐标
        /// </summary>
        public Polygon polygonPointValue { set; get; }

        public int g3e_fno { set; get; }

        public long g3e_id { set; get; }

        public long g3e_fid { set; get; }
        /// <summary>
        /// 父设备FID
        /// </summary>
        public long parentFid { get; set; }
        //public Entity entity { set;get; }

        /// <summary>
        /// 符号类型
        /// 点类型：multipoint
        /// 线类型：linestring
        /// 面类型：polygon
        /// </summary>
        public egtype isSymbolType { set; get; }
        /// <summary>
        /// 线型
        /// </summary>
        public string lineTypeStr { set; get; }
        /// <summary>
        /// 线宽
        /// </summary>
        public double lineWidth { set; get; }  
        
        
        double _scale = 1.0 / ElectronTransferModel.Config.MapConfig.Instance.earthscale;
        public double scale
        {
            get
            {
                return _scale;
            }
            set { _scale = value; }
        }

        public ElectronSymbol insertobj { set; get; }

        public SpecilaDevField spf = new SpecilaDevField();

        public string SBMC { set; get; }
        /// <summary>
        /// 块定义名称
        /// </summary>
        public string blockName { set; get; }
    }
    public class SpecilaDevField
    {
        public string Ssbyq { get; set; }
        public string Ssgt { get; set; }
        public string Sstj { get; set; }
        public string Sskgg { get; set; }
        public string Ssdf { get; set; }
        public string Sstqhtj { get; set; }
        public string SsZx { get; set; }
    }
}
