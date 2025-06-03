using System;
using Code.Game.Model;
using Code.Game.State;
using Cysharp.Threading.Tasks;
using UnityEngine;
using VContainer.Unity;

namespace Code.Game.Systems.Services
{
    public class EnergyTickService : IStartable
    {
        private readonly EnergyModel _energyModel;
        private readonly GameState _gameState;
        private int _tick = 0;

        public EnergyTickService(EnergyModel energyModel, GameState gameState)
        {
            _energyModel = energyModel;
            _gameState = gameState;
        }

        public void Start()
        {
            TickLoop().Forget();
        }

        private async UniTaskVoid TickLoop()
        {
            await UniTask.WaitUntil(() => _gameState.Tiles.Count > 0);

            while (true)
            {
                _energyModel.TickOnlineGeneration();
                Debug.Log($"[Tick] Energy generated at {Time.time}");
                await UniTask.Delay(TimeSpan.FromSeconds(1), DelayType.DeltaTime);
            }
        }
        
    }
}