using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using ElectronTransferFramework;
using ElectronTransferModel.Base;
using ElectronTransferModel;
using System.Reflection;

namespace ElectronTransferDal.Common
{
    public class PrecisionCache : Singleton<PrecisionCacheImplement>
    {
    }
    public class PrecisionCacheImplement : Dictionary<PropertyInfo, int>
    {
        //private Dictionary<PropertyInfo,int> _precicisons = new Dictionary<PropertyInfo,int>();
        public PrecisionCacheImplement()
        {
            foreach (var type in TypeCache.Instance.GetTypes(typeof(DBEntity))) 
            {
                foreach( var property in type.GetProperties()){
                    var precicisonAttr = property.GetCustomAttributes(typeof(PrecisionAttribute), false).Cast<PrecisionAttribute>().FirstOrDefault();
                    if (precicisonAttr != null) 
                    {
                        Add(property, precicisonAttr.Precision);
                    }
                }
            }
        }

    }
}
