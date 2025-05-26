
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

        public void AddCoins(int amount)
        {
            _state.Coins.Value += amount;
        }

        public bool TrySpendCoins(int amount)
        {
            if (_state.Coins.Value < amount)
            {
                return false;
            }

            _state.Coins.Value -= amount;
            return true;
        }
    }
}
