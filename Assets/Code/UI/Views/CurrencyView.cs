using System;
using System.Collections.Generic;
using Code.UI.ViewModels;
using UnityEngine;

namespace Code.UI.Views
{
    public class CurrencyView : MonoBehaviour
    {
        [SerializeField] private List<CurrencyItemView> _items = new();

        private readonly List<IDisposable> _subscriptions = new();

        public void Init(CurrencyViewModel viewModel)
        {
            foreach (var binding in _items)
            {
                if (viewModel.Currency.TryGetValue(binding.CurrencyType, out var value))
                {
                    var subscription = value.Subscribe(binding.UpdateAmount);
                    _subscriptions.Add(subscription);
                }
            }
        }

        private void OnDestroy()
        {
            foreach (var sub in _subscriptions)
                sub.Dispose();
        }
    }
    
}
