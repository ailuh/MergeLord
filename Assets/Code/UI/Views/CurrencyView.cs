using System;
using Code.Common.EditorUtils;
using Code.UI.ViewModels;
using TMPro;
using UnityEngine;
using VContainer;

namespace Code.UI.Views
{
    public class CurrencyView : MonoBehaviour
    {
        [SerializeField, CantBeNull] private TextMeshProUGUI _coinsText = null!;
        [Inject] private CurrencyViewModel _viewModel = null!;
        private IDisposable _coinsSubscription = null!;
        
        private void Start()
        {
            _coinsSubscription = _viewModel.Coins.Subscribe(UpdateView);
        }

        private void UpdateView(int coins)
        {
            _coinsText.text = coins.ToString();
        }
        
        private void OnDestroy()
        {
            _coinsSubscription.Dispose();
        }
    }
}
