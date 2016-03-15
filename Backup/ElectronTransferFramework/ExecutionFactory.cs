using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using ElectronTransferFramework.Aspect;
using Spring.Aop.Framework;
using Spring.Aspects.Exceptions;

namespace ElectronTransferFramework
{
    public abstract class ExecutionFactory : ElectronTransferFramework.IExecutionFactory
    {
        private Dictionary<int, IExecution> _executions = new Dictionary<int, IExecution>();
        public ExecutionFactory()
        {
            Initialize();
        }
        protected virtual void Initialize() 
        {
            ExecutionManager.Instance.RegisterFactory(this);
        }
        public void Add(int key, IExecution exec) 
        {
            _executions.Add(key, exec);
        }

        public IExecution GetExecution(int key) 
        {
            try
            {
                var item = _executions[key];
                ProxyFactory factory = new ProxyFactory(item);
                factory.AddAdvice(new InterceptorAdvice());
                IExecution command = (IExecution)factory.GetProxy();
                //return command;
                return item;
            }
            catch (KeyNotFoundException e) 
            {
                return new EmptyExecution();
            }
        }
        //public void Execute(int key,object sender) 
        //{
        //    try
        //    {
        //        _executions[key].Execute(sender, EventArgs.Empty);
        //    }
        //    catch(KeyNotFoundException e)
        //    {
        //    }
        //}
    }
}
