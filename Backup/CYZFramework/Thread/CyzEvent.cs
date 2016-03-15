using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
//using System.Threading;
using System.Runtime.InteropServices;

namespace CYZFramework.Thread
{
    class CyzEvent
    {
    }


    public class ProcessSemaphore :  IDisposable
    {

        private uint handle;
        private readonly uint interruptReactionTime;

        public ProcessSemaphore(string name)
            : this(name, 0, int.MaxValue, 500)
        { }

        public ProcessSemaphore(string name, int initial)
            : this(name, initial, int.MaxValue, 500)
        { }

        public ProcessSemaphore(string name, int initial, int max, int interruptReactionTime)
        {
            this.interruptReactionTime = (uint)interruptReactionTime;
            this.handle = WApi.CreateSemaphore(null, initial, max, name);

            if (handle == 0)
            {
                //throw new SemaphoreFailedException();
            }
        }

        public void Acquire()
        {
            while (true)
            {
                //looped 0.5s timeout to make NT-blocked threads interruptable.      
                uint res = WApi.WaitForSingleObject(handle, interruptReactionTime);
                try
                {
                    System.Threading.Thread.Sleep(0);
                }
                catch (System.Threading.ThreadInterruptedException e)
                {
                    if (res == 0)
                    {
                        //Rollback          
                        int previousCount;
                        WApi.ReleaseSemaphore(handle, 1, out previousCount);
                    }
                    throw e;
                }
                if (res == 0)
                    return;
                if (res != 258)
                {
                    //throw new SemaphoreFailedException();
                }
                   
            }
        }

        public void Acquire(TimeSpan timeout)
        {
            uint milliseconds = (uint)timeout.TotalMilliseconds;
            if (WApi.WaitForSingleObject(handle, milliseconds) != 0)
            {
                //throw new SemaphoreFailedException();
            }
        }



        public void Release()
        {
            int previousCount;
            if (!WApi.ReleaseSemaphore(handle, 1, out previousCount))
            {
                //throw new SemaphoreFailedException();
            }
        }
        #region IDisposable Member
        public void Dispose()
        {
            if (handle != 0)
            {
                if (WApi.CloseHandle(handle))
                    handle = 0;
            }
        }
        #endregion
    } 
}
