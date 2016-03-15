using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Reflection;
using System.Linq.Expressions;
using ElectronTransferFramework.Aspect;

namespace ElectronTransferFramework
{
    public class ExecutionManager : Singleton<ExecutionManager>, IExecutionManager 
    {
        IExecutionManager _manager = new ExecutionManagerImplement();
        #region IExecutionManager 成员

        public void EnsureInitialize(Assembly assembly)
        {
            _manager.EnsureInitialize(assembly);
        }

        public IExecutionFactory GetFactory(Type type)
        {
            return _manager.GetFactory(type);
        }

        public bool HasRegistered(Type type)
        {
            return _manager.HasRegistered(type);
        }

        public void RegisterFactory(IExecutionFactory factory)
        {
            _manager.RegisterFactory(factory);
        }

        #endregion
    }
    internal class ExecutionManagerImplement : ElectronTransferFramework.IExecutionManager
    {

        private Dictionary<Type, IExecutionFactory> _factories = new Dictionary<Type, IExecutionFactory>();
        
        public void RegisterFactory(IExecutionFactory factory) 
        {
            _factories.Add(factory.GetType(), factory);
        }
        public bool HasRegistered(Type type) 
        {
            return _factories.ContainsKey(type);
        }
        public IExecutionFactory GetFactory(Type type) 
        {
            return _factories[type];
        }

        
        public void EnsureInitialize( Assembly assembly) 
        {
            foreach (var type in assembly.GetTypes())
            {
                ExecutionContainerAttribute containerAttr =
                    type.GetCustomAttributes(typeof (ExecutionContainerAttribute), false)
                        .Cast<ExecutionContainerAttribute>()
                        .FirstOrDefault();
                if (containerAttr == null) continue;
                LogErrorAttribute classErrorAttr = type.GetCustomAttributes(typeof(LogErrorAttribute), false).Cast<LogErrorAttribute>().FirstOrDefault();
                var factory = _factories[containerAttr.FactoryType];
                var methods = type.GetMethods(  );
                foreach( var method in methods )
                {
                    ExecutionAttribute executionAttr = method.GetCustomAttributes(typeof(ExecutionAttribute), false).Cast<ExecutionAttribute>().FirstOrDefault();
                    LogErrorAttribute logErrorAttr = method.GetCustomAttributes(typeof(LogErrorAttribute), false).Cast<LogErrorAttribute>().FirstOrDefault();
                    if (executionAttr != null)
                    {
                        var handle = method.MethodHandle;

                        ValueEventHandler handler = (object sender, ValueEventArgs e) => MethodBase.GetMethodFromHandle(handle).Invoke(null, new object[] { sender, e });
                        var execution = new FuncExecution(handler);
                        if (logErrorAttr != null)
                        {
                            execution.SwallowError = logErrorAttr.Swallow;
                            execution.LogError = true;
                        }
                        else if (classErrorAttr != null)
                        {
                            execution.SwallowError = classErrorAttr.Swallow;
                            execution.LogError = true;
                        }
                        factory.Add(executionAttr.Key, execution);
                    }
                }
            }
        }
        
    }
}
