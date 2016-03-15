using System;
namespace ElectronTransferFramework
{
    public interface IExecutionFactory
    {
        void Add(int key, IExecution exec);
        IExecution GetExecution(int key);
    }
}
