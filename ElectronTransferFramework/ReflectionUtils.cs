using System;
using System.Collections.Generic;
using System.Data;
using System.IO;
using System.Linq;
using System.Text;
using System.Reflection;
using System.Linq.Expressions;
using ElectronTransferFramework.Exceptions;

namespace ElectronTransferFramework
{
    public static class ReflectionUtils
    {
        /// <summary>
        /// 查找类型
        /// </summary>
        /// <param name="assembly">程序集</param>
        /// <param name="baseType">基类</param>
        /// <returns></returns>
        public static IEnumerable<Type> FindTypes( Assembly assembly, Type baseType)
        {
            if (assembly == null) throw new ArgumentNullException("assembly");
            if (baseType == null) throw new ArgumentNullException("baseType");

            return assembly.GetTypes().Where(o => o.IsSubclassOf(baseType) && !o.IsAbstract);
        }

        /// <summary>
        /// 根据特定属性查找对象是否相等，如关键字
        /// </summary>
        /// <param name="obj1">对象1</param>
        /// <param name="obj2">对象2</param>
        /// <param name="properties">属性</param>
        /// <returns></returns>
        public static bool EqualsByProperties( object obj1,object obj2, PropertyInfo[] properties )
        {
            if (obj1 == null) throw new ArgumentNullException("obj1");
            if (obj2 == null) throw new ArgumentNullException("obj2");
            if (properties == null) throw new ArgumentNullException("properties");

            if (obj1.GetType() == obj2.GetType() && properties.All(o => o.ReflectedType == obj1.GetType()))
            {
                return properties.All(o => o.GetValue(obj1, null).Equals( o.GetValue(obj2, null)));
            }
            else
            {
                throw new WrongTypeException("对象类型与声明属性的类型不一致");//modify later
            }
        }
        /// <summary>
        /// 是否存在特定值的属性
        /// </summary>
        /// <param name="obj">对象</param>
        /// <param name="propertyName">属性名</param>
        /// <param name="value">值</param>
        /// <returns></returns>
        public static bool HasProperty(this object obj, string propertyName, object value)
        {
            try
            {
                return value.Equals(obj.GetValue(propertyName));
                
            }
            catch (Exception)
            {

                return false;
            }
        }

        public static bool IsDefault(this object obj, string propertyName) 
        {
            if (obj == null) throw new ArgumentNullException("obj");
            var property = obj.GetProperty(propertyName);
            //if (property==null)throw new po
            var defaultValue = DefaultValueFactory.Instance.GetDefaultValue(property.PropertyType);
            return object.Equals( defaultValue ,obj.GetValue(propertyName));
            
        }
        /// <summary>
        /// 设置回属性类型的默认值
        /// </summary>
        /// <param name="obj"></param>
        public static object ResetDefault(this object obj)
        {
            if (obj == null) throw new ArgumentNullException("obj");
            foreach (var attr in obj.GetPropertyNames())
            {
                var property = obj.GetProperty(attr);
                var defaultValue = DefaultValueFactory.Instance.GetDefaultValue(property.PropertyType);
                if (!object.Equals(defaultValue,obj.GetValue(attr)))
                {
                    obj.SetValue(attr, defaultValue);
                }
            }
            return obj;
        }
        /// <summary>
        /// 获得所有属性名
        /// </summary>
        /// <param name="obj">对象</param>
        /// <returns></returns>
        public static IEnumerable<string> GetPropertyNames(this object obj)
        {
            if (obj == null) throw new ArgumentNullException("obj");
            return obj.GetType().GetProperties().Select(o=>o.Name);
        }

        public static bool Has(this Type type, string propertyName) 
        {
            if (type == null) throw new ArgumentNullException("type");
            return type.GetPropertyNames().Contains(propertyName);
        }

        public static IEnumerable<string> CompareProperties(this object source,object dest) 
        {
            var sourcePropertyNames = source.GetPropertyNames();
            var destPropertyNames = dest.GetPropertyNames();
            var interset = sourcePropertyNames.Intersect(destPropertyNames);
            foreach (var item in interset) 
            {
                var sourceValue = source.GetValue(item);
                var destValue=dest.GetValue(item);
                if (!object.Equals(sourceValue, destValue))
                    yield return item;
                //if (sourceValue != null && destValue != null)
                //{
                //    if (!sourceValue.Equals(destValue))
                //        yield return item;
                //}
                //else if (sourceValue != destValue)
                //    yield return item;
            }
        }

        /// <summary>
        /// 根据属性名获得值
        /// </summary>
        /// <param name="obj">对象</param>
        /// <param name="propertyName">属性名</param>
        /// <returns></returns>
        public static object GetValue(this object obj, string propertyName)
        {
            return GetValueRaw(obj, propertyName);
        }

        private static Dictionary<PropertyInfo, FastInvokeHandler> delegates = new Dictionary<PropertyInfo, FastInvokeHandler>();
        private static PropertyInfo _lastProperty=null;
        private static object GetValueRaw(object obj, string propertyName)
        {

            if (obj == null) throw new ArgumentNullException("obj");
            if (propertyName == null) throw new ArgumentNullException("propertyName");

            var type = obj.GetType();

            if (_lastProperty == null || _lastProperty.ReflectedType != type || _lastProperty.Name != propertyName)
            {
                _lastProperty = type.GetProperty(propertyName);
            }

            var property = _lastProperty;

            if (property == null) throw new ArgumentException("无此属性" + propertyName);
            
            FastInvokeHandler getter = null;
            if (!delegates.TryGetValue(property, out getter))
            {
                getter = property.GetGetMethod().GetMethodInvoker();
                delegates.Add(property, getter);
            }
            return getter(obj,null);
        }
        
        /// <summary>
        /// 根据属性名获得值
        /// </summary>
        /// <typeparam name="T">返回的类型</typeparam>
        /// <param name="obj">对象</param>
        /// <param name="propertyName">属性名</param>
        /// <returns></returns>
        public static T GetValue<T>(this object obj, string propertyName)
        {
            return (T)GetValueRaw(obj,propertyName);
        }

        /// <summary>
        /// 设定值
        /// </summary>
        /// <param name="obj">对象</param>
        /// <param name="propertyName">属性</param>
        /// <param name="value">值</param>
        public static void SetValue(this object obj, string propertyName, object value)
        {
            try
            {
                var property = obj.GetType().GetProperty(propertyName);
                if (property == null)
                {
                    throw new ArgumentException(propertyName + "属性不存在");
                }
                property.SetValue(obj, value, null);
            }
            catch (Exception e)
            {
                LogManager.Instance.Error(e);
            }
        }

        /// <summary>
        /// 检查对象中属性是否有效
        /// </summary>
        /// <param name="properties">属性</param>
        /// <param name="obj">对象</param>
        /// <returns></returns>
        public static bool CheckPropertyAvailable(PropertyInfo[] properties, object obj)
        {
            if (properties == null) throw new ArgumentNullException("properties");
            if (obj == null) throw new ArgumentNullException("obj");

            //not equals default value
            return properties.All(o => !o.GetValue(obj, null).Equals(DefaultValueFactory.Instance.GetDefaultValue(o.PropertyType)));
        }

        /// <summary>
        /// 拷贝对象，要求类型一致
        /// </summary>
        /// <param name="source">源</param>
        /// <param name="dest">目的</param>
        public static void Copy(object source, object dest) 
        {
            if (source == null) throw new ArgumentNullException("source");
            if (dest == null) throw new ArgumentNullException("dest");

            if (source.GetType() != dest.GetType()) 
            {
                throw new ArgumentException("source与dest类型不一致");
            }
            var properties = dest.GetType().GetProperties();
            foreach (var property in properties) 
            {
                var hasSetter = HasSetter(property);
                if (!hasSetter)
                    continue;

                var value = property.GetValue(source, null);
                property.SetValue(dest, value, null);
            }    
        }

        /// <summary>
        /// 局部拷贝，不要求对象一致，只拷贝属性的交集
        /// </summary>
        /// <param name="source">源对象</param>
        /// <param name="dest">目的对象</param>
        public static void PartialCopy(object source, object dest)
        {
            PartialCopyRaw(source, dest, true);
        }

        /// <summary>
        /// 局部拷贝，不要求对象一致，只拷贝属性的交集
        /// </summary>
        /// <param name="source">源对象</param>
        /// <param name="dest">目的对象</param>
        public static void PartialCopy(object source, object dest,bool overwrite)
        {
            PartialCopyRaw(source, dest, overwrite);
        }
        private static PropertyInfo GetProperty(this object obj,string propertyName)
        {
            return obj.GetType().GetProperty(propertyName);
        }
        public static bool Equals(Type type, object obj1, object obj2) 
        {
            return (bool)type.InvokeMember( "Equals", BindingFlags.Static| BindingFlags.Public | BindingFlags.InvokeMethod | BindingFlags.FlattenHierarchy,
                null, null, new object[] { obj1, obj2 });
        }
        private static void PartialCopyRaw(object source, object dest, bool overwrite)
        {
            var sourcePropertyNames = source.GetPropertyNames();
            var destPropertyNames = dest.GetPropertyNames();
            var interset = sourcePropertyNames.Intersect(destPropertyNames);
            foreach (var item in interset)
            {
                var value = source.GetValue(item);
                var hasSetter = HasSetter(dest.GetProperty(item));
                if (!hasSetter)
                    continue;
                var propertyType = source.GetProperty(item).PropertyType;
                //var defaultValue = DefaultValueFactory.Instance.GetDefaultValue(source.GetProperty(item).PropertyType);
                bool write = overwrite || !Equals(propertyType, DefaultValueFactory.Instance.GetDefaultValue( propertyType ), value);
                if (write) 
                {
                    
                    dest.SetValue(item, value);
                }
                
            }
        }

        private static bool HasSetter(PropertyInfo property)
        {
            var hasSetter = property.GetAccessors().Any(o => o.Name.StartsWith("set_"));
            return hasSetter;
        }

        /// <summary>
        /// 按照特定类型创建对象
        /// </summary>
        /// <param name="source">源对象</param>
        /// <param name="type">参照类型</param>
        /// <returns></returns>
        public static object CreateObject(object source, Type type) 
        {
            if (source == null) throw new ArgumentNullException("source");
            if (type == null) throw new ArgumentNullException("type");

            var ret = Activator.CreateInstance(type);
            PartialCopyRaw(source, ret,true);
            //var sourceProperties = source.GetType().GetProperties();
            //var typeProperties = type.GetProperties();
            
            //foreach (var sourceProperty in sourceProperties.Where(o=> typeProperties.Any( p=>o.Name == p.Name))) 
            //{
            //    var value = sourceProperty.GetValue(source, null);
            //    typeProperties.First(o => o.Name == sourceProperty.Name).SetValue( ret, value,null );
            //}
            return ret;
        }

        /// <summary>
        /// 获得程序集的所有引用程序集
        /// </summary>
        /// <param name="assembly">程序集</param>
        /// <returns></returns>
        public static IEnumerable<Assembly> GetReferencedAssemblies(Assembly assembly)
        {
            if (assembly == null) throw new ArgumentNullException("assembly");

            return assembly.GetReferencedAssemblies().Select(o => Assembly.Load(o));
        }

        /// <summary>
        /// 获得程序集的物理路径
        /// </summary>
        /// <param name="assembly">程序集</param>
        /// <returns></returns>
        public static string GetPhysicalDirectory(this Assembly assembly)
        {
            var dir = assembly.CodeBase;
            return Path.GetDirectoryName(dir.Replace("file:///", ""));
        }
    }
}
