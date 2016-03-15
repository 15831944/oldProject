using System;
namespace ElectronTransferDal.Common
{
    interface IAvoidFieldsCache
    {
        System.Collections.Generic.IEnumerable<string> GetAvoidFields(Type type, bool isUpdate);
        System.Collections.Generic.IEnumerable<string> GetNonColumnFields(Type type);
        System.Collections.Generic.IEnumerable<string> GetSelectOnlyFields(Type type);
    }
}
