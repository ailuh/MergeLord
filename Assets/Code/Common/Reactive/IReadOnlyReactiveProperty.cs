using System;

namespace Code.Common.Reactive
{
    public interface IReadOnlyReactiveProperty<T>
    {
        T Value { get; }
        IDisposable Subscribe(Action<T> listener);
        void Unsubscribe(Action<T> listener);
    }
}