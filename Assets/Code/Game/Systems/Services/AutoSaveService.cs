using System;
using Code.Game.Configs.DataModels;
using Code.Game.Systems.Managers;
using VContainer;
using VContainer.Unity;

namespace Code.Game.Systems.Services
{
    public class AutoSaveService : IStartable
    {
        [Inject] private readonly SaveLoadService _saveLoadService;
        //[Inject] private readonly CurrencyManager _currencyManager;
        [Inject] private readonly GridManager _gridManager;
        
        public void Start()
        {
            //_currencyManager.OnChanged += Save;
            _gridManager.OnGridChanged += Save;
        }
        
        private void Save()
        {
            var save = new GameSaveData
            {
                Grid = _gridManager.GetGridState(),
                /*Energy = _currencyManager.Energy,
                Coins = _currencyManager.Coins,*/
                LastSaveUtc = DateTime.UtcNow
            };

            _saveLoadService.Save(save);
        }

        
    }
}