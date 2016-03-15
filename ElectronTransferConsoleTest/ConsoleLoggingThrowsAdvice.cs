using System;
using Spring.Aop;

namespace ElectronTransferConsoleTest
{
    public class ConsoleLoggingThrowsAdvice : IThrowsAdvice
    {
        public void AfterThrowing(Exception ex)
        {
            Console.Error.WriteLine(
                String.Format("Advised method threw this exception : {0}", ex.Message));
        }
    }
}