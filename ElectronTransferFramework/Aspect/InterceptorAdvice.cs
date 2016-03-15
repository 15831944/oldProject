using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using AopAlliance.Intercept;

namespace ElectronTransferFramework.Aspect
{
    class InterceptorAdvice : IMethodInterceptor
    {
        public object Invoke(IMethodInvocation invocation)
        {
            FuncExecution execution = invocation.This as FuncExecution;
            try
            {
                return invocation.Proceed();
            }
            catch (Exception ex)
            {
                if (execution.LogError)
                {
                    LogManager.Instance.Error(ex);
                    if (!execution.SwallowError)
                    {
                        throw;
                    }
                }
                return null;
                //throw;
            }
        }
    }
}
