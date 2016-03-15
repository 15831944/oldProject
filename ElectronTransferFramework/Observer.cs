using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ElectronTransferFramework
{
    public class Observer
    {
        public Observer(Observable observable)
        {
            if ( observable == null ) throw new ArgumentNullException("observable");

            observable.AddObserver(this);
        }
    }
}
