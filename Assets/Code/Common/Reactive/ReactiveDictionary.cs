using System;
using System.Collections.Generic;

namespace Code.Common.Reactive
{
    public class ReactiveDictionary<TKey, TValue> : Dictionary<TKey, TValue>, IReadOnlyReactiveDictionary<TKey, TValue>
    {
        public event Action<TKey, TValue>? OnChanged;

        public new void Add(TKey key, TValue value)
        {
            base.Add(key, value);
            OnChanged?.Invoke(key, value);
        }

        public new TValue this[TKey key]
        {
            get => base[key];
            set
            {
                base[key] = value;
                OnChanged?.Invoke(key, value);
            }
        }
    }
}
