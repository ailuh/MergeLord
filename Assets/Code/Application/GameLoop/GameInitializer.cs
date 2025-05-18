using Code.Application.Factories;
using Code.Application.Managers;
using Code.Infrastructure.Configs;
using UnityEngine;
using VContainer;
using VContainer.Unity;

namespace Code.Application.GameLoop
{
    public class GameInitializer : IStartable
    {
        [Inject] private LevelStartupConfig _startupConfig;
        [Inject] private GridManager _gridManager;
        [Inject] private DraggableFactory _factory;

        public GameInitializer(LevelStartupConfig config, GridManager grid, DraggableFactory factory)
        {
            _startupConfig = config;
            _gridManager = grid;
            _factory = factory;
        }
        
        public void Start()
        {
            _gridManager.GenerateGrid(_startupConfig.Tiles);

            foreach (var tileData in _startupConfig.Tiles)
            {
                if (string.IsNullOrEmpty(tileData.ObjectId))
                    continue;

                var tile = _gridManager.GetTileAt(tileData.Position);
                if (tile != null && !tile.IsOccupied)
                {
                    _factory.Create(tile, tileData.ObjectId);
                }
            }
            Debug.Log("Game initialized from startup config.");
        }
    }
}
