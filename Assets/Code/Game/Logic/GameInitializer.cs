using System;
using System.Collections.Generic;
using Code.Game.Configs;
using Code.Game.Configs.Monsters;
using Code.Game.Model;
using Code.Game.State;
using Code.Game.Systems.Interfaces;
using Code.Game.Systems.Managers;
using Code.Game.Systems.Services;
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
        [Inject] private readonly GridObjectCatalog _catalog = null!;
        [Inject] private readonly SaveLoadService _saveLoadService = null!;
        [Inject] private readonly EnergyModel _energyModel = null!;
        [Inject] private readonly CurrencyModel _currencyModel = null!;
        [Inject] private readonly GameState _state = null!;
        [Inject] private readonly EnergyView[] _energyViews = null!;

        public void Start()
        {
            List<TileObjectData> tilesToUse;
            
            if (_startupConfig.UseSaveData)
            {
                var loadData = _saveLoadService.Load();

                if (loadData != null)
                {
                    tilesToUse = loadData.Grid;
                    _energyModel.LoadFromSave(loadData.Energy);
                    _energyModel.ApplyOfflineProgress(loadData.LastSaveUtc, DateTime.UtcNow);
                    _state.Coins.Value = loadData.Coins;
                    _state.Level.Value = loadData.Level;
                    Debug.Log("Loaded game from save.");
                }
                else
                {
                    tilesToUse = _startupConfig.Tiles;
                    _energyModel.InitFromDefault(_startupConfig);
                    Debug.Log("Initialized game from startup config.");
                }
            }
            else
            {
                tilesToUse = _startupConfig.Tiles;
                _energyModel.InitFromDefault(_startupConfig);
                Debug.Log("Startup config used explicitly. Save ignored.");
            }
            foreach (var view in _energyViews)
            {
                var viewModel = _energyViewModelFactory.Create(view.Type);
                view.Init(viewModel);
            }
            _gridManager.InitializeGrid(tilesToUse, _gameMessageService);

            foreach (var tileData in tilesToUse)
            {
                if (string.IsNullOrEmpty(tileData.ObjectId))
                    continue;

                var tile = _gridManager.GetTileAt(tileData.Position);
                if (tile == null || tile.IsOccupied)
                    continue;

                var config = _catalog.GetConfig(tileData.ObjectId);
                if (config == null)
                    continue;

                var prefab = _catalog.GetPrefab(tileData.ObjectId);
                if (prefab == null)
                    continue;

                var draggable = _draggableFactory.Create(tile, config.ObjectRef, prefab);
                if (draggable == null)
                    continue;

                tile.SetObject(draggable);
                _dragDropService.RegisterDraggable(draggable);
            }
        }
    }
}
