using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ElectronTransferView
{
    public class dog
    {
        public static bool getaccess()
        {
            try
            {
                return SenseLock.Instance.GetTime() > DateTime.Now;
            }
            catch(Exception)
            {
                return false;
            }
        }
    }
}
