using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Runtime.CompilerServices;
using System.Text;

namespace ElectronTransferDal.Common
{
    static  class AnonymousTypesSupport
    {
        /// <summary>
        /// 判断是否匿名类型
        /// </summary>
        /// <param name="type"></param>
        /// <returns></returns>
        public static bool IsAnonymous(this Type type)
        {
            return type.IsGenericType && type.IsClass && type.IsSealed && type.Name.StartsWith("<>f__AnonymousType");
        }
    }
}
