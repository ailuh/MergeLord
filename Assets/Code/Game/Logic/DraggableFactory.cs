using Code.Game.Configs.Monsters;
using Code.Game.State;
using Code.Game.Systems.Managers;
using Code.UI.Views;
using Configs.Objects;
using UnityEngine;

namespace Code.Game.Logic
{
    public class DraggableFactory
    {
        private readonly Canvas _canvas;
        private readonly GridManager _gridManager;
        private readonly GameState _gameState;

        public DraggableFactory(Canvas canvas, GridManager gridManager, GameState gameState)
        {
            _canvas = canvas;
            _gridManager = gridManager;
            _gameState = gameState;
        }

        public DraggableObject? Create(TileView tileView, GridObjectConfigBase baseConfig, DraggableObject draggablePrefab)
        {
            if (!_gameState.Tiles.TryGetValue(tileView.GridPosition, out var tileModel))
            {
                Debug.LogError("[DraggableFactory] Tile model not found for position: " + tileView.GridPosition);
                return null;
            }

            var draggableObject = Object.Instantiate(draggablePrefab, tileView.transform);
            if (draggableObject == null)
            {
                Debug.LogError("[DraggableFactory] Failed to instantiate prefab for objectId: " + baseConfig.Id);
                return null;
            }

            draggableObject.Init(tileModel, _canvas, baseConfig, _gridManager);
            return draggableObject;
        }
    }
}