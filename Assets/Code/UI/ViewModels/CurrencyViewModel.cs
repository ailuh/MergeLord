using Code.Common.Reactive;
using Code.Game.Enums;
using Code.Game.State;

namespace Code.UI.ViewModels
{
    public class CurrencyViewModel
    {
        public IReadOnlyReactiveProperty<int> Coins => _gameState.Coins;
        public ReactiveDictionary<EnergyType, ReactiveProperty<int>> Energy { get; } = new();

        private readonly GameState _gameState;

        public CurrencyViewModel(GameState state)
        {
            _gameState = state;
        }
    }
}
