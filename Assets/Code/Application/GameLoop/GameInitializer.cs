using Code.Application.Factories;
using Code.Application.Interfaces;
using Code.Application.Managers;
using Code.Infrastructure.Configs;
using Code.Infrastructure.Configs.Monsters;
using UnityEngine;
using VContainer;
using VContainer.Unity;

namespace Code.Application.GameLoop
{
    public class GameInitializer : IStartable
    {
        [Inject] private LevelStartupConfig _startupConfig = null!;
        [Inject] private GridManager _gridManager = null!;
        [Inject] private DraggableFactory _factory = null!;
        [Inject] private DragDropManager _dragDropManager = null!;
        [Inject] private IGameMessageService _gameMessageService = null!;
        [Inject] private GridObjectCatalog _catalog = null!;
        
        public void Start()
        {
            _gridManager.InitializeGrid(_startupConfig.Tiles, _gameMessageService);
            foreach (var tileData in _startupConfig.Tiles)
            {
                if (string.IsNullOrEmpty(tileData.ObjectId))
                {
                    continue;
                }
                var tile = _gridManager.GetTileAt(tileData.Position);
                if (tile == null || tile.IsOccupied)
                {
                    continue;
                }
                var config = _catalog.GetConfig(tileData.ObjectId);
                if (config == null)
                {
                    continue;
                }
                var objectPrefab = _catalog.GetPrefab(tileData.ObjectId);
                if (objectPrefab == null)
                {
                    continue;
                }
                var draggableObject = _factory.Create(tile, config.ObjectRef, objectPrefab);
                if (draggableObject == null)
                {
                    continue;
                }
                tile.SetObject(draggableObject);
                _dragDropManager.RegisterDraggable(draggableObject);
            }
            Debug.Log("Game initialized from startup config.");
        }
    }
}
