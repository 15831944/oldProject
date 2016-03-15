using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ElectronTransferFramework
{
    public class DefaultValueFactory:Singleton<DefaultValueFactory>,IDefaultValueFactory
    {
        IDefaultValueFactory _factory = new DefaultValueFactoryImplement();
        #region IDefaultValueFactory 成员

        public object GetDefaultValue(Type type)
        {
            return _factory.GetDefaultValue(type);
        }

        #endregion
    }

    class DefaultValueFactoryImplement : ElectronTransferFramework.IDefaultValueFactory
    {
        Dictionary<Type, object> _defaultValues = new Dictionary<Type, object>();
        public DefaultValueFactoryImplement()
        {
            Initialize();
        }
        public void Initialize()
        {
            _defaultValues.Add(typeof(bool), default(bool));
            _defaultValues.Add(typeof(char), default(char));
            _defaultValues.Add(typeof(byte), default(byte));
            _defaultValues.Add(typeof(sbyte), default(sbyte));
            _defaultValues.Add(typeof(decimal), default(decimal));
            _defaultValues.Add(typeof(double), default(double));
            _defaultValues.Add(typeof(float), default(float));
            _defaultValues.Add(typeof(Int16), default(Int16));
            _defaultValues.Add(typeof(Int32), default(Int32));
            _defaultValues.Add(typeof(Int64), default(Int64));
            _defaultValues.Add(typeof(UInt16), default(UInt16));
            _defaultValues.Add(typeof(UInt32), default(UInt32));
            _defaultValues.Add(typeof(UInt64), default(UInt64));
            _defaultValues.Add(typeof(string), default(string));
            _defaultValues.Add(typeof(DateTime), default(DateTime));
        }

        public object GetDefaultValue(Type type)
        {
            if (_defaultValues.ContainsKey(type))
            {
                return _defaultValues[type];
            }
            else
            {
                return Activator.CreateInstance(type);
            }
        }
    }
}
