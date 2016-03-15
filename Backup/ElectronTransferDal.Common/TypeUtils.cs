using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using ElectronTransferModel.Base;
using ElectronTransferDal.Common.Exceptions;

namespace ElectronTransferDal.Common
{
    public static class TypeUtils
    {
        public static bool IsDbType(this Type type) 
        {
            return type.IsSubclassOf(typeof(DBEntity));
        }

        public static void CheckDbType(this Type type) 
        {
            if (!type.IsDbType()) 
            {
                throw new NonDbTypeException();
            }
        }
    }
}
