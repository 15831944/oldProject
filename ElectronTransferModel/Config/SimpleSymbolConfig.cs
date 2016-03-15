using System.Collections.Generic;
using ElectronTransferFramework;

namespace ElectronTransferModel.Config
{
    public class SimpleSymbolConfig : ICanSerialize
    {
        public static SimpleSymbolConfig Instance { get; set; }
        /// <summary>
        /// 符号列表
        /// </summary>
        public List<SimpleSymbolConfigEntry> Symbols { set; get; }
    }
    public class SimpleSymbolConfigEntry
    {
        public int Fno { set; get; }
        /// <summary>
        /// 坐标类型名称
        /// </summary>
        public string PtClassName { set; get; }
        /// <summary>
        /// 图层名称
        /// </summary>
        public string LayerName { set; get; }
        /// <summary>
        /// 图元颜色
        /// </summary>
        public string SymbolColor { set; get; }
        public string LineType { set; get; }
        public double LineWidth { set; get; }
        /// <summary>
        /// 公共属性表
        /// </summary>
        public string Common { set; get; }
        /// <summary>
        /// 自身属性表
        /// </summary>
        public string SelfAttribute { set; get; }
        /// <summary>
        /// 连接关系表
        /// </summary>
        public string Connectivity { set; get; }
        /// <summary>
        /// 功能位置表
        /// </summary>
        public string Gnwz { set; get; }
        /// <summary>
        /// 包含表
        /// </summary>
        public string Contain { set; get; }
        public string G3E_CNO { set; get; }
        public string LableClassName { set; get; }
        /// <summary>
        /// 详图关系表
        /// </summary>
        public string Detailreference { set; get; }
        /// <summary>
        /// 受电馈线
        /// </summary>
        public string Gg_Pd_Sdkx_Ac { set; get; }
        /// <summary>
        /// 标注信息
        /// </summary>
        public string LabelName { set; get; }
        /// <summary>
        /// 符号lb
        /// </summary>
        public string PtClassNameLb { set; get; }
        /// <summary>
        /// 标注lb
        /// </summary>
        public string LableClassNameLb { set; get; }
        public static SimpleSymbolConfigEntry Empty = new SimpleSymbolConfigEntry();
    }
}