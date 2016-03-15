using System;
namespace ElectronTransferFramework
{
    interface IDefaultValueFactory
    {
        object GetDefaultValue(Type type);
    }
}
