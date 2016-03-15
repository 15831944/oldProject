using System;
namespace ElectronTransferFramework
{
    interface ICloneFactory
    {
        void AddType(Type type);
        Func<object, object> GetMethod(Type type);
    }
}
