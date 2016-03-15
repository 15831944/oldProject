using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.ComponentModel;

namespace ElectronTransferDal.AutoGeneration
{
    public class XPropDescriptor : PropertyDescriptor
    {
        private XProp theProp;

        public XPropDescriptor(ref XProp prop, Attribute[] attrs)
            : base(prop.DisplayName, attrs)
        {
            theProp = prop;
        }
        public override string Name
        {
            get
            {
                return theProp.Name;
            }
        }

        public override bool CanResetValue(object component)
        {
            return false;
        }
        public override string Category
        {
            get
            {
                return theProp.Category;
            }
        }
        public override TypeConverter Converter
        {
            get
            {
                return theProp.Converter;
            }
        }
        public override Type ComponentType
        {
            get
            {
                return this.GetType();
            }
        }
        public override object GetEditor(Type editorBaseType)
        {
            if (!theProp.ReadOnly)
            {
                return theProp.Editor == null ? base.GetEditor(editorBaseType) : theProp.Editor;
            }
            return base.GetEditor(editorBaseType);
        }
        public override object GetValue(object component)
        {
            if (theProp.Value != null)
            {
                if(theProp.ProType!=typeof(DateTime))
                    return theProp.Value.ToString().Replace(" ", "");
            }
            return theProp.Value;
        }
        public override bool IsBrowsable
        {
            get
            {
                return theProp.Browsable;
            }
        }
        public override string DisplayName
        {
            get
            {
                return theProp.DisplayName;
            }
        }

        public override bool IsReadOnly
        {
            get { return theProp.ReadOnly; }
        }

        public override Type PropertyType
        {
            get { return theProp.ProType; }
        }

        public override void ResetValue(object component)
        {

        }


        public override void SetValue(object component, object value)
        {
            if (value != null)
            {
                if(theProp.ProType!=typeof(DateTime))
                    value = value.ToString().Replace(" ","");
                else if(theProp.ProType ==typeof(DateTime))
                {
                    DateTime dt = (DateTime) value;
                    value = dt.Date;
                }
            }
            theProp.Value = value;
        }

        public override bool ShouldSerializeValue(object component)
        {
            return false;
        }

    }
}
