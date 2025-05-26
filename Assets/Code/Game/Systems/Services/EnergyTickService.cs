using System;
using Code.Game.Model;
using Cysharp.Threading.Tasks;
using VContainer;
using VContainer.Unity;

namespace Code.Game.Systems.Services
{
    public class EnergyTickService : IStartable
    {
        private readonly EnergyModel _energyModel;

        [Inject]
        public EnergyTickService(EnergyModel energyModel)
        {
            _energyModel = energyModel;
        }

        public void Start()
        {
            TickLoop().Forget();
        }

        private async UniTaskVoid TickLoop()
        {
            while (true)
            {
                _energyModel.TickOnlineGeneration();
                await UniTask.Delay(TimeSpan.FromSeconds(1));
            }
        }
    }
}