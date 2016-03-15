using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ElectronTransferFramework.Aspect
{
    [AttributeUsage(AttributeTargets.Method|AttributeTargets.Class, Inherited = false, AllowMultiple = true)]
    public sealed class LogErrorAttribute : Attribute
    {
        // See the attribute guidelines at 
        //  http://go.microsoft.com/fwlink/?LinkId=85236
        private readonly bool _swallow;

        public LogErrorAttribute()
        {
        }
        public LogErrorAttribute(bool swallow)
        {
            _swallow = swallow;
        }

        public bool Swallow
        {
            get { return _swallow; }
        }

    }
}
