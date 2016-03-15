using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ElectronTransferModel
{
    [global::System.AttributeUsage(AttributeTargets.Property, Inherited = true, AllowMultiple = true)]
    public sealed class KeyFieldAttribute : Attribute
    {
        // See the attribute guidelines at 
        //  http://go.microsoft.com/fwlink/?LinkId=85236

        public KeyFieldAttribute()
        {
        }
    }
}
