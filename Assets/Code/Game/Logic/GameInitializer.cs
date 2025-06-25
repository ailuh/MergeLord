using System;
using System.Collections.Generic;
using Code.Common.Reactive;
using Code.Game.Configs;
using Code.Game.Configs.Interfaces;
using Code.Game.Configs.Monsters;
using Code.Game.Model;
using Code.Game.Model.Buildings;
using Code.Game.Model.Quests;
using Code.Game.Model.Rewards;
using Code.Game.State;
using Code.Game.Systems.Interfaces;
using Code.Game.Systems.Managers;
using Code.Game.Systems.Services;
using Code.UI.ViewModels;
using Code.UI.Views;
using UnityEngine;
using VContainer;
using VContainer.Unity;

namespace Code.Game.Logic
{
    public class GameInitializer : IStartable
    {
        [Inject] private readonly LevelStartupConfig _startupConfig = null!;
        [Inject] private readonly GridManager _gridManager = null!;
        [Inject] private readonly DraggableFactory _draggableFactory = null!;
        [Inject] private readonly EnergyViewModelFactory _energyViewModelFactory = null!;
        [Inject] private readonly DragDropService _dragDropService = null!;
        [Inject] private readonly IGameMessageService _gameMessageService = null!;
        [Inject] private readonly MonsterObjectCatalog _catalog = null!;
        [Inject] private readonly SaveLoadService _saveLoadService = null!;
        [Inject] private readonly EnergyModel _energyModel = null!;
        [Inject] private readonly CurrencyModel _currencyModel = null!;
        [Inject] private readonly GameState _state = null!;
        [Inject] private readonly EnergyView[] _energyViews = null!;
        [Inject] private readonly IQuestEventBus _eventBus = null!;
        [Inject] private readonly MainScreenView _mainScreenView = null!;
        [Inject] private readonly MainScreenViewModel _mainScreenViewModel = null!;
        [Inject] private readonly RewardBufferModel _rewardBufferModel = null!;
        [Inject] private readonly RewardBufferView _rewardBufferView = null!;
        [Inject] private readonly TileService _tileService = null!;
        [Inject] private readonly BuildingShopViewModel _buildingShopViewModel = null!;
        [Inject] private readonly BuildingShopModel _buildingShopModel = null!;
        [Inject] private readonly CurrencyViewModel _currencyViewModel = null!;
        [Inject] private readonly IGridObjectResolver _gridObjectResolver = null!;


        public void Start()
        {
            List<TileObjectData> tilesToUse;
            
            if (_startupConfig.UseSaveData)
            {
                var loadData = _saveLoadService.Load();

                if (loadData != null)
                {
                    tilesToUse = loadData.Grid;
                    _energyModel.InitTileService(_tileService);
                    _energyModel.LoadFromSave(loadData.Energy);
                    _energyModel.ApplyOfflineProgress(loadData.LastSaveUtc, DateTime.UtcNow);
                    _state.Level.Value = loadData.Level;
                    Debug.Log("Loaded game from save.");
                }
                else
                {
                    var questModel = new QuestModel(_state, _startupConfig.QuestCatalog, _eventBus, _rewardBufferModel);
                    _rewardBufferView.Init(_rewardBufferModel, _gridManager, _gameMessageService, _gridObjectResolver);
                    var activeQuests = questModel.GetActiveRegularQuests(4);
                    var questViewModel = new QuestViewModel(questModel, activeQuests);

                    _mainScreenView.Init(questViewModel, _mainScreenViewModel, _buildingShopViewModel, _currencyViewModel);
                    _buildingShopModel.UpdateAvailableBuildingsByLevel(_state.Level.Value);
                    tilesToUse = _startupConfig.Tiles;
                    _energyModel.InitTileService(_tileService);
                    _energyModel.InitFromDefault(_startupConfig);
                    Debug.Log("Initialized game from startup config.");
                }
            }
            else
            {
                foreach (var currencyAmount in _startupConfig.StartingCurrency)
                {
                    if (_state.Currency.TryGetValue(currencyAmount.Type, out var currency))
                    {
                        currency.Value = currencyAmount.Initial;
                    }
                    else
                    {
                        _state.Currency[currencyAmount.Type] = new ReactiveProperty<int>(currencyAmount.Initial);
                    }
                }
                _buildingShopModel.UpdateAvailableBuildingsByLevel(_state.Level.Value);
                var questModel = new QuestModel(_state, _startupConfig.QuestCatalog, _eventBus, _rewardBufferModel);
                _rewardBufferView.Init(_rewardBufferModel, _gridManager, _gameMessageService, _gridObjectResolver);
                var activeQuests = questModel.GetActiveRegularQuests(4);
                var questViewModel = new QuestViewModel(questModel, activeQuests);

                _mainScreenView.Init(questViewModel, _mainScreenViewModel, _buildingShopViewModel, _currencyViewModel);
               
                tilesToUse = _startupConfig.Tiles;
                _energyModel.InitTileService(_tileService);
                _energyModel.InitFromDefault(_startupConfig);
                Debug.Log("Startup config used explicitly. Save ignored.");
                
            }
            foreach (var view in _energyViews)
            {
                var viewModel = _energyViewModelFactory.Create(view.Type);
                view.Init(viewModel);
            }
            _gridManager.Init(_draggableFactory, _dragDropService, _tileService);
            _gridManager.InitializeGrid(tilesToUse, _gameMessageService);
            _state.Level.Subscribe(level =>
            {
                _buildingShopModel.UpdateAvailableBuildingsByLevel(level);
            });
            
            
            foreach (var tileData in tilesToUse)
            {
                if (string.IsNullOrEmpty(tileData.ObjectId))
                    continue;

                var tile = _tileService.GetTile(tileData.Position);
                if (tile == null || tile.IsOccupied)
                    continue;

                var config = _catalog.GetConfig(tileData.ObjectId);
                if (config == null)
                    continue;

                var prefab = _catalog.GetPrefab(tileData.ObjectId);
                if (prefab == null)
                    continue;

                var draggable = _draggableFactory.Create(tile.GetView(), config, prefab);
                if (draggable == null)
                    continue;

                tile.SetObject(draggable);
                _dragDropService.RegisterDraggable(draggable);
            }
        }
    }
}
