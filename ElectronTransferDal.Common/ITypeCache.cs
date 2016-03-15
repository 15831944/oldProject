using System;
using System.Collections.Generic;
namespace ElectronTransferDal.Common
{
    interface ITypeCache
    {
        Type GetTypeFromCache(Type baseType, string typeName);
        IEnumerable<Type> GetTypes(Type baseType);
    }
}
