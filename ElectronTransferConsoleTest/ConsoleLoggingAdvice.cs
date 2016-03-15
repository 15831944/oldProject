using System;
using AopAlliance.Intercept;

namespace ElectronTransferConsoleTest
{
    public class ConsoleLoggingAdvice : IMethodInterceptor
    {
        public object Invoke(IMethodInvocation invocation)
        {
            try
            {
                return invocation.Proceed();
            }
            catch (Exception)
            {
                
                //throw;
                return null;
            }
            
        }
    }
}