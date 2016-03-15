using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;

namespace ElectronTransferFramework
{
    public abstract class Observable
    {
        Collection<Observer> _observers = new Collection<Observer>();
        /*
        public Observable(IEnumerable<Observer> observers)
        {
            AddObservers(observers);
        }

        public void AddObservers(IEnumerable<Observer> observers)
        {
            foreach (Observer observer in observers)
            {
                AddObserver(observer);
            }
        }*/

        public void AddObserver(Observer observer)
        {
            if (_observers.Contains(observer))
                throw new ArgumentException("Observer已存在");
            _observers.Add(observer);
        }
    }
}
