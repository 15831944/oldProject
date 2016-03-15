using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using ElectronTransferFramework;
using System.ComponentModel;
using SysComponent = System.ComponentModel;
using System.Reflection;
using ElectronTransferDal.Cad;
using V94 = ElectronTransferModel.V9_4;
using ElectronTransferDal.Common;
using ElectronTransferModel.Base;
using ElectronTransferModel.Config;
using ElectronTransferModel;
using ElectronTransferDal.Factory;

namespace ElectronTransferDal
{
    public static class SurfaceInteractive
    {
        /// <summary>
        /// 获取属性值
        /// </summary>
        /// <param name="obj">对象</param>
        /// <param name="attribute">属性名</param>
        /// <returns>属性值</returns>
        public static string GetAttribute(this object obj, string attribute)
        {
            try
            {
                var value = obj.GetValue(attribute);
                return value != null ? value.ToString() : null;
            }
            catch (ArgumentException)
            {
                return null;
            }
        }
        /// <summary>
        /// 设置属性值
        /// </summary>
        /// <param name="obj">对象</param>
        /// <param name="attribute">属性名</param>
        /// <param name="value">属性值</param>
        public static void SetAttribute(this object obj, string attribute, object value)
        {
            try
            {
                obj.SetValue(attribute, value);
            }
            catch (ArgumentException ex)
            {

            }
        }
        /// <summary>
        /// 判断对象是否存在该属性
        /// </summary>
        /// <param name="obj"></param>
        /// <param name="PropertyName"></param>
        /// <returns></returns>
        public static bool HasAttribute(this object obj, string PropertyName)
        {
            try
            {
                var properties = obj.GetPropertyNames().Where(o => o.Equals(PropertyName));
                if (properties.Any())
                    return true;
                return false;
            }
            catch
            {
                return false;
            }
        }
    }
}
