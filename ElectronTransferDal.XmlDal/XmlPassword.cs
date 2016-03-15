using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using ElectronTransferFramework;

namespace ElectronTransferDal.XmlDal
{
    
    public class XmlPassword :  Singleton<XmlPassword>,IXmlPassword
    {
        private XmlPasswordImplent _xmlPassword = new XmlPasswordImplent();

        public string Password
        {
            get
            {
                return _xmlPassword.Password;
            }
            set
            {
                _xmlPassword.Password = value;
            }
        }

    }
    internal class XmlPasswordImplent : IXmlPassword 
    {
        public XmlPasswordImplent()
        {
            //Password = "93ddcfd45cf466f58de7c1c0e6664551";
        }
        public string Password { get; set; }
    }
}
