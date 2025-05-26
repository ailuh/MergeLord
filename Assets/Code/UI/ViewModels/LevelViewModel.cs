using Code.Common.Reactive;
using Code.Game.State;

namespace Code.UI.ViewModels
{
    public class LevelViewModel
    {
        public IReadOnlyReactiveProperty<int> Level => _state.Level;

        private readonly GameState _state;

        public LevelViewModel(GameState state)
        {
            _state = state;
        }
    }
}
