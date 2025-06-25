using Code.Common.Reactive;
using Code.Game.Enums;
using Code.Game.State;

namespace Code.UI.ViewModels
{
    public class CurrencyViewModel
    {
        public ReactiveDictionary<CurrencyType, ReactiveProperty<int>> Currency => _gameState.Currency;

        private readonly GameState _gameState;

        public CurrencyViewModel(GameState state)
        {
            _gameState = state;
        }
    }
}
