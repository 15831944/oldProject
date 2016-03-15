using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ElectronTransferFramework
{
    public class FuncExecution/*<TArgs>*/:IExecution// where TArgs:EventArgs
    {
        
        public ValueEventHandler EventHandler;
        //delegate void( object , ValueEventArgs ) ValueEventHandler;
        //public EventHandler/*<TArgs>*/ EventHandler;
        
        public FuncExecution(ValueEventHandler/*<TArgs>*/ func)
        {
            EventHandler += func;
        }

        internal bool LogError { get; set; }
        internal bool SwallowError { get; set; }

        #region IExecution 成员

        public void Execute(object sender, ValueEventArgs args)
        {
            EventHandler( sender, args/* as TArgs*/);
        }

        #endregion
    }
}
