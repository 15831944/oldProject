using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ElectronTransferFramework
{
    public class CloneFactory : Singleton<CloneFactory>, ICloneFactory
    {
        private ICloneFactory _factory = new CloneFactoryImplement();
        #region ICloneFactory 成员

        public void AddType(Type type)
        {
            _factory.AddType(type);
        }

        public Func<object, object> GetMethod(Type type)
        {
            return _factory.GetMethod(type);
        }

        #endregion
    }
    class CloneFactoryImplement : ElectronTransferFramework.ICloneFactory 
    {
        private Dictionary<Type, Func<object, object>> _methods = new Dictionary<Type, Func<object, object>>();
        public void AddType( Type type )
        {
            _methods.Add(type, EmitUtils.CreateCloneMethod(type));
        }
        public Func<object, object> GetMethod(Type type) 
        {
            if (!_methods.ContainsKey(type)) 
            {
                AddType(type);
            }
            return _methods[type];
        }
    }
}
