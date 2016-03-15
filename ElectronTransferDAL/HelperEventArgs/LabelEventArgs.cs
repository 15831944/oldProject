using Autodesk.AutoCAD.Colors;
using ElectronTransferDal.XmlDal;
using ElectronTransferFramework;

namespace ElectronTransferDal.HelperEventArgs
{
    public class LabelEventArgs : ValueEventArgs
    {
        /// <summary>
        /// 标注内容
        /// </summary>
        public string lbText { set; get; }

        /// <summary>
        /// 高度
        /// </summary>
        public double lbHeight { set; get; }

        /// <summary>
        /// 颜色
        /// </summary>
        public Color color { get; set; }

        public long g3e_id { set; get; }
        public long g3e_fid { set; get; }

        /// <summary>
        /// 数据源状态，true为自定义数据源，false为原始数据源
        /// </summary>
        public bool Status { set; get; }
        /// <summary>
        /// 数据源
        /// </summary>
        public XmlDBManager xmlDB { set; get; }
    }
}
