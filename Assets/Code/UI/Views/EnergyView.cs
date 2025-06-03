using System;
using Code.Common.EditorUtils;
using Code.Game.Enums;
using Code.UI.Animations;
using Code.UI.ViewModels;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace Code.UI.Views
{
    public class EnergyView : MonoBehaviour
    {
        [SerializeField, CantBeNull] private TextMeshProUGUI _text = null!;
        [SerializeField, CantBeNull] private Image _energyIndicator = null!;
        [SerializeField, CantBeNull] private EnergyType _type;
        [SerializeField, CantBeNull] private RectTransform _popupContainer = null!;
        [SerializeField, CantBeNull] private TextMeshProUGUI _popupPrefab = null!;
        public EnergyType Type => _type;
        private EnergyEntryViewModel _entryViewModel;
        private IDisposable _subscription;

        public void Init(EnergyEntryViewModel viewModel)
        {
            _entryViewModel = viewModel;
            _entryViewModel.InitPopupPool(new PopupNumPoolService(_popupPrefab, _popupContainer));
            _entryViewModel.OnPopupRequested += ShowPopup;
            _subscription = viewModel.Amount.Subscribe(OnAmountChanged);
        }

        private void OnAmountChanged(int amount)
        {
            _text.text = amount.ToString();
            _energyIndicator.fillAmount = _entryViewModel.GetEnergyPercentage();
            _entryViewModel.HandleEnergyChanged(amount);
        }
        
        private void ShowPopup(string text)
        {
            _entryViewModel?.TryShowPopup(text);
        }

        
        private void OnDestroy()
        {
            _subscription?.Dispose();
            if (_entryViewModel != null)
            {
                _entryViewModel.OnPopupRequested -= ShowPopup;
            }
        }
    }
}
