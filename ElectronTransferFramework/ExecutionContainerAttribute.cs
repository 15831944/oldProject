using System;

namespace ElectronTransferFramework
{
    [global::System.AttributeUsage(System.AttributeTargets.Class, Inherited = false, AllowMultiple = true)]
    public sealed class ExecutionContainerAttribute: System.Attribute
    {
        // See the attribute guidelines at 
        //  http://go.microsoft.com/fwlink/?LinkId=85236
        readonly Type _type;
        // This is a positional argument
        public ExecutionContainerAttribute( Type factoryType )
        {
            
            Type currentType = factoryType;
            _type = factoryType;
            //do
            //{
            //    //if (currentType.IsGenericType && currentType.GetGenericTypeDefinition().FullName == "ElectronTransferFramework.ExecutionFactory`1") 
            //    if (currentType == typeof(ExecutionFactory))
            //    {
            //        isExecutionManager = true;
            //        _type = managerType;
            //        break;
            //    }
            //    currentType = currentType.BaseType;

            //} while (currentType.BaseType != typeof(object));

            if (!ExecutionManager.Instance.HasRegistered(factoryType)) 
            {
                throw new ArgumentException(string.Format("{0:{1}", "factoryType未注册", factoryType));
            }
        }

        public Type FactoryType { get { return _type; } }

    }
}