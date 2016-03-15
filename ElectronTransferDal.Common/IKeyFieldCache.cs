using System;
namespace ElectronTransferDal.Common
{
    interface IKeyFieldCache
    {
        System.Collections.Generic.IList<System.Reflection.PropertyInfo> FindKeyFields(Type type);
        void Add(Type type);
        System.Collections.Generic.IList<System.Reflection.PropertyInfo> this[Type type] { get; }
    }
}
