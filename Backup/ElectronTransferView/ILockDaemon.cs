using System;
namespace ElectronTransferView
{
    interface ILockDaemon
    {
        void CheckLock();
        void Start();
        void Stop();
        Action OnUnplug { get; set; }
    }
}
