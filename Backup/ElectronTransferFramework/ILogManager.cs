using System;
namespace ElectronTransferFramework
{
    interface ILogManager
    {
        void Debug(object message);
        void Error(object message);
        void Fatal(object message);
        void Info(object message);
    }
}
