using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.ComponentModel;

namespace ElectronTransferDal.AutoGeneration
{
    public class XProp
    {
        private string theCategory = string.Empty;//属性所属类型
        private string theName = string.Empty;//属性的名称
        private object theValue = null;//属性的值
        private bool theReadOnly = false;//属性是否只读
        private System.Type theType = null;//类型
        private bool theBrowsable = false;//是否显示
        private TypeConverter theConverter = null;//类型转换
        private object theEditor = null;
        private string theDisplayName;
        private bool saveValueByFid;//是否写入库的是FID

        public string Category
        {
            get { return theCategory; }
            set { theCategory = value; }
        }

        public string Name
        {
            get { return theName; }
            set { theName = value; }
        }

        public bool ReadOnly
        {
            get { return theReadOnly; }
            set { theReadOnly = value; }
        }
        public object Value
        {
            get { return theValue; }
            set { theValue = value; }
        }

        public Type ProType
        {
            get { return theType; }
            set { theType = value; }
        }

        public bool Browsable
        {
            get { return theBrowsable; }
            set { theBrowsable = value; }
        }

        public string DisplayName
        {
            get { return theDisplayName; }
            set { theDisplayName = value; }
        }

        public bool SaveValueByFid
        {
            get { return saveValueByFid; }
            set { saveValueByFid = value; }
        }

     
        public virtual TypeConverter Converter
        {
            get { return theConverter; }
            set { theConverter = value; }
        }
        public virtual object Editor
        {
            get { return theEditor; }
            set { theEditor = value; }
        }
     
    }
}
