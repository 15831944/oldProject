using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Reflection;
using System.Text;
using ElectronTransferFramework;
using ElectronTransferModel;

namespace ElectronTransferDal.Common
{
    public class AvoidFieldsCache : Singleton<AvoidFieldsCache>, IAvoidFieldsCache
    {

        private IAvoidFieldsCache _cache = new AvoidFieldsCacheImplement();
        #region IAvoidFieldsCache 成员

        public IEnumerable<string> GetAvoidFields(Type type, bool isUpdate)
        {
            return _cache.GetAvoidFields(type, isUpdate);
        }

        public IEnumerable<string> GetNonColumnFields(Type type)
        {
            return _cache.GetNonColumnFields(type);
        }

        public IEnumerable<string> GetSelectOnlyFields(Type type)
        {
            return _cache.GetSelectOnlyFields(type);
        }

        #endregion
    }

    internal class AvoidFieldsCacheImplement : IAvoidFieldsCache
    {
        private Dictionary<Type, ReadOnlyCollection<string>> _nonColumnFields = new Dictionary<Type, ReadOnlyCollection<string>>();
        private Dictionary<Type, ReadOnlyCollection<string>> _selectOnlyFields = new Dictionary<Type, ReadOnlyCollection<string>>();
        public IEnumerable<string> GetAvoidFields(Type type, bool isUpdate)
        {
            var ret = GetNonColumnFields(type);
            if (isUpdate)
                ret = ret.Union(GetSelectOnlyFields(type));
            return ret;
        }

        public IEnumerable<string> GetNonColumnFields(Type type)
        {
            ReadOnlyCollection<string> fields = null;
            if (!_nonColumnFields.ContainsKey(type))
            {
                fields=type.GetProperties().Where(IsNonColumnField).Select(o=>o.Name).ToList().AsReadOnly();
                _nonColumnFields.Add(type, fields);
            }
            else
            {
                fields = _nonColumnFields[type];
            }
            return fields;
        }

        public IEnumerable<string> GetSelectOnlyFields(Type type)
        {
            ReadOnlyCollection<string> fields = null;
            if (!_selectOnlyFields.ContainsKey(type))
            {
                fields = type.GetProperties().Where(IsSelectOnlyField).Select(o => o.Name).ToList().AsReadOnly();
                _selectOnlyFields.Add(type, fields);
            }
            else
            {
                fields = _selectOnlyFields[type];
            }
            return fields;
        }

        private bool IsNonColumnField(PropertyInfo propertyInfo)
        {
            return propertyInfo.GetCustomAttributes(typeof(NonColumnAttribute), true).Any();
        }
        private bool IsSelectOnlyField(PropertyInfo propertyInfo)
        {
            return propertyInfo.GetCustomAttributes(typeof(SelectOnlyAttribute), true).Any();
        }
    }
}
