using Code.Game.Enums;
using Code.Game.State;
using Code.UI.ViewModels;

namespace Code.Game.Logic
{
    public class EnergyViewModelFactory
    {
        private readonly GameState _state;

        public EnergyViewModelFactory(GameState state)
        {
            _state = state;
        }

        public EnergyEntryViewModel Create(EnergyType type)
        {
            return new EnergyEntryViewModel(_state, type);
        }
    }
}
