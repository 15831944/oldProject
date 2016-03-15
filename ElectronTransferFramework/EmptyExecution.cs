using System.Diagnostics;

namespace ElectronTransferFramework
{
    public class EmptyExecution :IExecution
    {
        public void Execute(object sender, ValueEventArgs args) 
        {
            Debug.WriteLine("EmptyExecution");
        }
    }
}