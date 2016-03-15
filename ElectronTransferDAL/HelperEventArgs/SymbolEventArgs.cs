using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Autodesk.AutoCAD.Colors;
using ElectronTransferDal.Common;
using ElectronTransferDal.XmlDal;
using ElectronTransferFramework;
using ElectronTransferModel;
using ElectronTransferModel.Base;

namespace ElectronTransferDal.HelperEventArgs
{
    public class SymbolEventArgs: ValueEventArgs
    {
        public long G3e_fid { set; get; }
        /// <summary>
        /// 颜色
        /// </summary>
        public Color color { get; set; }
        /// <summary>
        /// 符号类型
        /// </summary>
        public string SymbolType { get; set; }
        /// <summary>
        /// 开或合
        /// </summary>
        public string BreakerStatus { get; set; }
        /// <summary>
        /// 判断属性是否为空
        /// </summary>
        public bool IsNull { get; set; }

        /// <summary>
        /// 线宽
        /// </summary>
        public double lineWidth { set; get; }

        double _scale = 1.0  / ElectronTransferModel.Config.MapConfig.Instance.earthscale;

        public double scale
        {
            get { return _scale; }
            set { _scale = value; }
        }

        public EntityState EntityState { get; set; }
        /// <summary>
        /// 设备名称
        /// </summary>
        public string SBMC { get; set; }

        public ElectronSymbol electronSymbol { set; get; }

        public XmlDBManager DBManager { set; get; }
    }
}
