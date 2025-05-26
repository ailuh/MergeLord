using System;
using System.Collections.Generic;

namespace Code.Common.Reactive
{
    public interface IReadOnlyReactiveDictionary<TKey, TValue> : IReadOnlyDictionary<TKey, TValue>
    {
        event Action<TKey, TValue> OnChanged;
    }
}