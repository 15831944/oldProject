using System.Collections.Generic;
using ElectronTransferFramework;

namespace ElectronTransferModel.Config
{
    public class SimpleDBSymbolsConfig:ICanSerialize
    {
        public static SimpleDBSymbolsConfig Instance { set; get; }
        public List<DBSymbolEntry> DBSymbol { get; set; }
    }
    public class DBSymbolEntry
    {
        /// <summary>
        /// FNO
        /// </summary>
        public int G3E_FNO { get; set; }
        /// <summary>
        /// 坐标表
        /// </summary>
        public string SymbolPtTable { get; set; }
        /// <summary>
        /// 组件表
        /// </summary>
        public ComponentTable ComponentTable { set; get; }
        /// <summary>
        /// 其他属性
        /// </summary>
        public OtherProperty OtherProperty { get; set; }
        /// <summary>
        /// 标注
        /// </summary>
        public List<LabelContent> Label { get; set; }

        public static DBSymbolEntry Empty = new DBSymbolEntry();
    }
    public class OtherProperty
    {
        /// <summary>
        /// 层名称
        /// </summary>
        public string LayerName { set; get; }
        /// <summary>
        /// 符号颜色
        /// </summary>
        public string SymbolColor { set; get; }
        /// <summary>
        /// 线类型
        /// </summary>
        public string LineType { set; get; }
        /// <summary>
        /// 线宽
        /// </summary>
        public double LineWidth { set; get; }
    }
    public class LabelContent
    {
        /// <summary>
        /// 标注CNO
        /// </summary>
        public string CNO { set; get; }
        /// <summary>
        /// 标注名称
        /// </summary>
        public string Name { set; get; }
        /// <summary>
        /// 可浏览
        /// </summary>
        public bool Browsable { set; get; }
        /// <summary>
        /// 坐标表
        /// </summary>
        public string LabelPtTable { set; get; }
        //标注（一个标注可能由多个不同表的字段组合成）
        public List<LabelField> LabelCompose { set; get; } 
        /// <summary>
        /// 标注的lb表（有存储标注对其方式）
        /// </summary>
        public string LabelLbTable { set; get; }
        /// <summary>
        /// 标注颜色
        /// </summary>
        public string LabelColor { set; get; }
        /// <summary>
        /// 标注大小
        /// </summary>
        public string LabelSize { set; get; }
    }
    public class  ComponentTable
    {
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
        /// <summary>
        /// 详图关系表
        /// </summary>
        public string Detailreference { set; get; }
        /// <summary>
        /// 受电馈线
        /// </summary>
        public string Gg_Pd_Sdkx_Ac { set; get; }
    }
    public class LabelField
    {
        /// <summary>
        /// 对应表名
        /// </summary>
        public string CorrespondTableName { set; get; }
        /// <summary>
        /// 字段
        /// </summary>
        public string Field { set; get; }
        /// <summary>
        /// 单位
        /// </summary>
        public string Unit { set; get; }
    }
}
