
namespace ElectronTransferFramework
{

    [global::System.AttributeUsage(System.AttributeTargets.Method, Inherited = false, AllowMultiple = true)]
    public sealed class ExecutionAttribute : System.Attribute
    {
        // See the attribute guidelines at 
        //  http://go.microsoft.com/fwlink/?LinkId=85236
        readonly int _key;

        // This is a positional argument
        public ExecutionAttribute(int key)
        {
            _key = key;
        }

        public int Key
        {
            get { return _key; }
        }
    }
}