using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using ElectronTransferFramework;

namespace ElectronTransferModel.Config
{
    public class DeviceAttributeConfig : ICanSerialize
    {
        public static DeviceAttributeConfig Instance { get; set; }
        public List<SimpleAttrEntry> Attributes { get; set; }
    }

    public class SimpleAttrEntry
    {
        public int Fno { get; set; }
        public string LayerName { get; set; }
        /// <summary>
        /// 公共属性表
        /// </summary>
        public TableWithProperty Common { set; get; }
        /// <summary>
        /// 自身属性表
        /// </summary>
        public TableWithProperty SelfAttribute { set; get; }
        /// <summary>
        /// 连接关系表
        /// </summary>
        public TableWithProperty Connectivity { set; get; }

        /// <summary>
        /// 功能位置表
        /// </summary>
        public TableWithProperty Gnwz { set; get; }
        /// <summary>
        /// 安装位置配置选项
        /// </summary>
        public ConfigurationOption InstallLocationOption { get; set; }
        /// <summary>
        /// 功能位置线路分类配置选项
        /// </summary>
        public FieldCollection GnwzXlfl { get; set; }
        /// <summary>
        /// 所属支线配置
        /// </summary>
        public FieldCollection SSzxOption { get; set; }
        public static SimpleAttrEntry Empty = new SimpleAttrEntry();
    }

    public class TableWithProperty
    {
        public string TableName { get; set; }
        public List<Property> PropertiesFromTable { get; set; }
        public static TableWithProperty  Empty=new TableWithProperty();
    }
    /// <summary>
    /// 设备安装位置
    /// </summary>
    public class ConfigurationOption
    {
        public string TableName { get; set; }
        public string FieldName { get; set; }
        public List<AzwzOption> AzwzCollection { get; set; }
        public static ConfigurationOption Empty = new ConfigurationOption();
    }

    public class AzwzOption
    {
        public string AzwzState { get; set; } 
        public List<FieldCollection> FilterFields { get; set; } 
        public List<FieldCollection> ShowFields { get; set; } 
        public static AzwzOption Empty=new AzwzOption();
    }

    public class FieldCollection
    {
        public string FieldName { get; set; }
        public string TableName { get; set; }
        public static  FieldCollection Empty=new FieldCollection();
    }
    public class Property
    {
        public string Field { get; set; }
        public string DisplayName { get; set; }
        public bool Browsable { get; set; }
        public string DefaultValue { get; set; }
        public bool ReadOnly { get; set; }
        public string DropDownTable { get; set; }
        public bool SaveValueByFid { get; set; }
        public static Property Empty = new Property();
    }
}
