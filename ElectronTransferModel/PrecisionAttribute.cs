using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ElectronTransferModel
{
    [AttributeUsage(AttributeTargets.All, Inherited = false, AllowMultiple = true)]
    public sealed class PrecisionAttribute : Attribute
    {
        private readonly int _precision;

        public int Precision {
            get { return _precision; }
        }

        public PrecisionAttribute(int precision)
        {
            _precision = precision;
        }
    }
}
