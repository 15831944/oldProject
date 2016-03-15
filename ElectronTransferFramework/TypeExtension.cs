using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ElectronTransferFramework
{
    public static class TypeExtension
    {
        public static bool CheckPropertyAvailable(this Type type,string propertyName, Type propertyType) 
        {
            var property = type.GetProperty(propertyName);
            if (property == null) 
            {
                return false;
            }
            return property.PropertyType == propertyType;
        }
    }
}
