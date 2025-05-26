using System;
using Code.Common.EditorUtils;
using Code.Game.Enums;
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

        private EnergyEntryViewModel _entryViewModel = null!;
        private IDisposable _subscription = null!;
        public EnergyType Type => _type;
        
        public void Init(EnergyEntryViewModel viewModel)
        {
            _entryViewModel = viewModel;
            _subscription = viewModel.Amount.Subscribe(UpdateText);
        }
        
        private void UpdateText(int amount)
        {
            _text.text = amount.ToString();
            _energyIndicator.fillAmount = _entryViewModel.GetEnergyPercentage();
        }

        private void OnDestroy()
        {
            _subscription?.Dispose();
        }
    }
}
