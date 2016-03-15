using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using ElectronTransferFramework.Exceptions;

namespace ElectronTransferDal.Common
{
    public class DbKey:Dictionary<string,object>
    {
        public override bool Equals(object obj)
        {
            if (obj == null || GetType() != obj.GetType())
            {
                return false;
            }
            return this.All(o=> object.Equals( (obj as DbKey)[o.Key],o.Value));
        }

        public override int GetHashCode()
        {
            return base.GetHashCode();
        }
    }
}
