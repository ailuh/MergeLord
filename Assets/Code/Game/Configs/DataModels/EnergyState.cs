using Code.Common.Reactive;

namespace Code.Game.Configs.DataModels
{
    public class EnergyState
    {
        public ReactiveProperty<int> Current = new(0);
        public int Max;
    }
}