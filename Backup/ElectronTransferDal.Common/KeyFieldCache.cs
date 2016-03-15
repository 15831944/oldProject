using System;
using System.Collections.Generic;
using System.Linq;
using ElectronTransferFramework;
using ElectronTransferModel.Base;
using ElectronTransferModel;
using System.Reflection;
using System.Collections.ObjectModel;

namespace ElectronTransferDal.Common
{
    public class KeyFieldCache:Singleton<KeyFieldCache>, ElectronTransferDal.Common.IKeyFieldCache
    {
        private KeyFieldCacheImplement _cache = new KeyFieldCacheImplement();
        #region IKeyFieldCache 成员

        /// <summary>
        /// 获得关键字字段
        /// </summary>
        /// <param name="type">类型</param>
        /// <returns></returns>
        public IList<PropertyInfo> FindKeyFields(Type type)
        {
            return _cache.FindKeyFields(type);
        }

        /// <summary>
        /// 添加类型
        /// </summary>
        /// <param name="type">类型</param>
        public void Add(Type type)
        {
            _cache.Add(type);
        }

        /// <summary>
        /// 获得关键字字段
        /// </summary>
        /// <param name="type">类型</param>
        /// <returns></returns>
        public IList<PropertyInfo> this[Type type]
        {
            get { return _cache[type]; }
        }

        #endregion
    }
    internal class KeyFieldCacheImplement :IKeyFieldCache
    {
        public KeyFieldCacheImplement()
        {
            TypeCache.Instance.GetTypes(typeof(DBEntity)).ToList().ForEach(o => this.Add(o));
        }
        private Dictionary<Type, ReadOnlyCollection<PropertyInfo>> _cache = new Dictionary<Type, ReadOnlyCollection<PropertyInfo>>();
        private IEnumerable<PropertyInfo> FetchKeyFields( Type type ) 
        {
            return type.GetProperties().Where(o => o.GetCustomAttributes(typeof(KeyFieldAttribute),true).Length!=0);
        }

        public void Add(Type type) 
        {
            _cache.Add(type,  FetchKeyFields(type).ToList().AsReadOnly());
        }

        
        public IList<PropertyInfo> FindKeyFields(Type type) 
        {
            return _cache[type];
        }

        
        public IList<PropertyInfo> this[Type type]
        {
            get { return FindKeyFields(type); }
        }

    }
}
