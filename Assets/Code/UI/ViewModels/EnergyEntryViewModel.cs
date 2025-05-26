using Code.Common.Reactive;
using Code.Game.Enums;
using Code.Game.State;

namespace Code.UI.ViewModels
{
    public class EnergyEntryViewModel
    {
        public IReadOnlyReactiveProperty<int> Amount => _state.Energy[_type];

        private readonly GameState _state;
        private readonly EnergyType _type;
        private readonly ReactiveProperty<int> _max;

        public EnergyEntryViewModel(GameState state, EnergyType type)
        {
            _state = state;
            _type = type;

            if (!_state.Energy.ContainsKey(type))
            {
                _state.Energy[type] = new ReactiveProperty<int>(0);
            }
            
            _max = new ReactiveProperty<int>(_state.MaxEnergy.TryGetValue(type, out var val) ? val : 100);
        }

        public float GetEnergyPercentage()
        {
            var current = _state.Energy[_type].Value;
            var max = _max.Value;
            return max > 0 ? (float)current / max : 0f;
        }
    }
    
}
