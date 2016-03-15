using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ElectronTransferModel
{
    [AttributeUsage(AttributeTargets.Property, Inherited = false, AllowMultiple = true)]
    public sealed class NonColumnAttribute : Attribute
    {
        
    }
}
