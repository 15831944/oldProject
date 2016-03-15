using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using ElectronTransferFramework;
using ElectronTransferModel.Base;

namespace ElectronTransferDal.Common
{
    public class TypeCache : Singleton<TypeCache>, ElectronTransferDal.Common.ITypeCache
    {
        ITypeCache _cache = new TypeCacheImplement();
        #region ITypeCache 成员

        public Type GetTypeFromCache(Type baseType, string typeName)
        {
            return _cache.GetTypeFromCache(baseType, typeName);
        }

        public IEnumerable<Type> GetTypes(Type baseType)
        {
            return _cache.GetTypes(baseType);
        }

        #endregion
    }
    internal class TypeItemChache : Dictionary<string, Type>
    {
        public Type Type { get { return _type; } }
        private Type _type;
        public TypeItemChache(Type type,Dictionary<string, Type> children)
        {
            _type = type;
            CopyFrom(children);
        }
        private void CopyFrom(Dictionary<string, Type> source) 
        {
            foreach (var pair in source) 
            {
                Add(pair.Key, pair.Value);
            }
        }
    }

    internal class TypeCacheImplement:ITypeCache
    {
        private Dictionary<Type, TypeItemChache> _cache = new Dictionary<Type, TypeItemChache>();
        public TypeCacheImplement()
        {
            var dbType = typeof(DBEntity);
            _cache.Add(dbType, new TypeItemChache(dbType, ReflectionUtils.FindTypes(dbType.Assembly, dbType).ToDictionary(o => o.Name)));
        }

        /// <summary>
        /// 获得所有子类
        /// </summary>
        /// <param name="baseType">基类类型</param>
        /// <returns></returns>
        public IEnumerable<Type> GetTypes(Type baseType) 
        {
            return _cache[baseType].Values;
        }

        /// <summary>
        /// 获得子类
        /// </summary>
        /// <param name="baseType">基类类型</param>
        /// <param name="typeName">类型名称</param>
        /// <returns></returns>
        public Type GetTypeFromCache(Type baseType,string typeName) 
        {
            return _cache[baseType][typeName];
        }
    }
}
