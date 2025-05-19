using System;
using System.Collections.Generic;
using Cysharp.Threading.Tasks;

namespace Code.Core.Tools
{
    public class ReactiveProperty<T>
    {
        private T _value;
        private readonly List<Action<T>> _subscribers = new();
        private UniTaskCompletionSource? _waitTcs;
        private Func<T, bool>? _currentPredicate;

        public T Value
        {
            get => _value;
            set
            {
                if (Equals(_value, value))
                {
                    return;
                }

                _value = value;
                NotifySubscribers(_value);
            }
        }

        public ReactiveProperty(T initialValue)
        {
            _value = initialValue;
        }

        public void Subscribe(Action<T> callback)
        {
            if (!_subscribers.Contains(callback))
                _subscribers.Add(callback);
        }

        public void Unsubscribe(Action<T> callback)
        {
            _subscribers.Remove(callback);
        }

        private void NotifySubscribers(T value)
        {
            foreach (var subscriber in _subscribers)
            {
                subscriber.Invoke(value);
            }

            HandlePredicateCheck(value);
        }
        
        public void ForceNotify() => NotifySubscribers(_value);

        public async UniTask WaitUntil(Func<T, bool> predicate)
        {
            if (predicate(_value))
            {
                return;
            }

            _currentPredicate = predicate;
            _waitTcs = new UniTaskCompletionSource();
            await _waitTcs.Task;
        }

        private void HandlePredicateCheck(T value)
        {
            if (_currentPredicate == null)
                return;

            if (!_currentPredicate(value))
            {
                return;
            }
            _currentPredicate = null;
            _waitTcs?.TrySetResult();
            _waitTcs = null;
        }
    }
}