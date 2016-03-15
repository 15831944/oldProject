using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using AopAlliance.Intercept;
using Spring.Aop;

namespace ElectronTransferFramework.Aspect
{
    class ExceptionLogAdvice : IThrowsAdvice
    {
        public void AfterThrowing(Exception ex)
        {
            //LogManager.Instance.Error(ex);
        }
    }
}
