using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ElectronTransferFramework
{
    public class DictionaryWithEvent<TKey, TValue> : IDictionary<TKey, TValue>
    {
        public delegate void OnAddEvent(TKey key,TValue value);
        public delegate void OnRemoveEvent(TKey key,TValue value);
        public delegate void OnSetEvent(TKey key, TValue oldValue, TValue newValue);
        public OnAddEvent OnAdd;
        public OnAddEvent OnAddComplete;
        public OnRemoveEvent OnRemove;
        public OnRemoveEvent OnRemoveComplete;
        public OnSetEvent OnSet;
        public OnSetEvent OnSetComplete;
        private Dictionary<TKey, TValue> _storage = new Dictionary<TKey, TValue>();

        #region IDictionary<TKey,TValue> 成员

        public void Add(TKey key, TValue value)
        {
            if (OnAdd != null)
                OnAdd(key, value);
            _storage.Add(key, value);
            if (OnAddComplete != null)
                OnAddComplete(key, value);
            
        }

        public bool ContainsKey(TKey key)
        {
            return _storage.ContainsKey(key);
        }

        public ICollection<TKey> Keys
        {
            get { return _storage.Keys; }
        }

        public bool Remove(TKey key)
        {
            TValue value = this[key];
            if (OnRemove != null)
                OnRemove(key, value);
            bool ret = _storage.Remove(key);
            if (OnRemoveComplete != null)
                OnRemoveComplete(key, value);
            return ret;
        }

        public bool TryGetValue(TKey key, out TValue value)
        {
            return _storage.TryGetValue(key, out value);
        }

        public ICollection<TValue> Values
        {
            get { return _storage.Values; }
        }

        public TValue this[TKey key]
        {
            get
            {
                return _storage[key];
            }
            set
            {
                var oldValue = _storage[key];
                if (OnSet != null)
                    OnSet(key, oldValue, value);
                _storage[key] = value;
                if (OnSetComplete != null)
                    OnSetComplete(key, oldValue, value);
            }
        }

        #endregion

        #region ICollection<KeyValuePair<TKey,TValue>> 成员

        public void Add(KeyValuePair<TKey, TValue> item)
        {
            Add(item.Key, item.Value);
        }

        public void Clear()
        {
            _storage.Clear();
        }

        public bool Contains(KeyValuePair<TKey, TValue> item)
        {
            return _storage.Contains(item);
        }

        public void CopyTo(KeyValuePair<TKey, TValue>[] array, int arrayIndex)
        {
            
        }

        public int Count
        {
            get { return _storage.Count; }
        }

        public bool IsReadOnly
        {
            get { return false; }
        }

        public bool Remove(KeyValuePair<TKey, TValue> item)
        {
            if (!_storage.Contains(item))
                return false;
            return Remove(item.Key);
        }

        #endregion

        #region IEnumerable<KeyValuePair<TKey,TValue>> 成员

        public IEnumerator<KeyValuePair<TKey, TValue>> GetEnumerator()
        {
            return _storage.GetEnumerator();
        }

        #endregion

        #region IEnumerable 成员

        System.Collections.IEnumerator System.Collections.IEnumerable.GetEnumerator()
        {
            return _storage.GetEnumerator();
        }

        #endregion
    }
}
