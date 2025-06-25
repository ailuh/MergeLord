
using Code.Common.Reactive;
using Code.Game.Enums;
using Code.Game.State;

namespace Code.Game.Model
{
    public class CurrencyModel
    {
        private readonly GameState _state;

        public CurrencyModel(GameState state)
        {
            _state = state;
        }

        public int GetAmount(CurrencyType type)
        {
            return _state.Currency.TryGetValue(type, out var value)
                ? value.Value
                : 0;
        }

        public void Add(CurrencyType type, int amount)
        {
            if (!_state.Currency.ContainsKey(type))
            {
                _state.Currency[type] = new ReactiveProperty<int>(0);
            }

            _state.Currency[type].Value += amount;
        }

        public bool TrySpend(CurrencyType type, int amount)
        {
            if (!_state.Currency.TryGetValue(type, out var value) || value.Value < amount)
            {
                return false;
            }

            value.Value -= amount;
            return true;
        }

        public bool HasEnough(CurrencyType type, int amount)
        {
            return _state.Currency.TryGetValue(type, out var value) && value.Value >= amount;
        }
    }
}
