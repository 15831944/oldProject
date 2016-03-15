using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Timers;
using ElectronTransferFramework;

namespace ElectronTransferView
{
    class LockDaemon : Singleton<LockDaemon>,ILockDaemon
    {

        private LockDaemonImplement _implement = new LockDaemonImplement();
        #region ILockDaemon 成员

        public void CheckLock()
        {
            _implement.CheckLock();
        }

        public void Start()
        {
            _implement.Start();
        }

        public void Stop()
        {
            _implement.Stop();
        }




        public Action OnUnplug
        {
            get { return _implement.OnUnplug; }
            set
            {
                _implement.OnUnplug = value;
            }

        }

        #endregion
    }
    class LockDaemonImplement : ElectronTransferView.ILockDaemon 
    {
        private Timer _timer;
        public LockDaemonImplement()
        {
            _timer = new Timer(TimeSpan.FromSeconds(30).TotalMilliseconds) { AutoReset = true };
            _timer.Elapsed += (object sender, ElapsedEventArgs e) => CheckLock();
        }
        public void Start()
        {
            
            _timer.Start();
        }
        public void Stop() 
        {
            _timer.Stop();
        }
        public void CheckLock()
        {
            if (SenseLock.Instance.IsOpened)
            {
                if (!SenseLock.Instance.VerifyUserPin("d6465065"))
                {
                    if (OnUnplug != null)
                    {
                        OnUnplug();
                    }
                }
            }
        }
        public Action OnUnplug { get; set; }
    }
}
