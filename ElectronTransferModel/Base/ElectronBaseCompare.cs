using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ElectronTransferModel.Base
{
    public class ElectronBaseCompare<T> : IEqualityComparer<T>  where T:ElectronBase
    {
        #region IEqualityComparer<ElectronBase> 成员

        public bool Equals(T x, T y)
        {

            return (x as ElectronBase).G3E_FID == (y as ElectronBase).G3E_FID;
        }

        public int GetHashCode(T obj)
        {
           return obj.ToString().ToLower().GetHashCode();
        }

        #endregion
    }
}
