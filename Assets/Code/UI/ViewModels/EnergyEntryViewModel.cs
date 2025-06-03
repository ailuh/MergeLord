using System;
using Code.Common.Reactive;
using Code.Game.Enums;
using Code.Game.State;
using Code.UI.Animations;
using UnityEngine;

namespace Code.UI.ViewModels
{
    public class EnergyEntryViewModel
    {
        public IReadOnlyReactiveProperty<int> Amount => _state.Energy[_type];

        private readonly GameState _state;
        private readonly EnergyType _type;
        private readonly ReactiveProperty<int> _max;
        public event Action<string>? OnPopupRequested;
        private PopupNumPoolService? _popupPool;
        private int _previousAmount;

        public EnergyEntryViewModel(GameState state, EnergyType type)
        {
            _state = state;
            _type = type;

            if (!_state.Energy.ContainsKey(type))
            {
                _state.Energy[type] = new ReactiveProperty<int>(0);
            }

            _max = new ReactiveProperty<int>(_state.MaxEnergy.TryGetValue(type, out var val) ? val : 100);
            _previousAmount = _state.Energy[_type].Value;
        }

        public void InitPopupPool(PopupNumPoolService popupPool)
        {
            _popupPool = popupPool;
        }

        public void HandleEnergyChanged(int newAmount)
        {
            var delta = newAmount - _previousAmount;
            _previousAmount = newAmount;

            if (delta > 0)
            {
                OnPopupRequested?.Invoke("+" + delta);
            }
        }
        
        public void TryShowPopup(string text)
        {
            _popupPool?.ShowPopup(text);
        }

        public float GetEnergyPercentage()
        {
            var current = _state.Energy[_type].Value;
            var max = _max.Value;
            return max > 0 ? (float)current / max : 0f;
        }
    }
}
