using System;
namespace ElectronTransferFramework
{
    interface IExecutionManager
    {
        void EnsureInitialize(System.Reflection.Assembly assembly);
        IExecutionFactory GetFactory(Type type);
        bool HasRegistered(Type type);
        void RegisterFactory(IExecutionFactory factory);
    }
}
