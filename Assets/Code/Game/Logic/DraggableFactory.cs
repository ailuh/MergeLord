using Code.Game.Configs.Monsters;
using Code.Game.Systems.Managers;
using Code.UI.Views;
using UnityEngine;

namespace Code.Game.Logic
{
    public class DraggableFactory
    {
        private readonly Canvas _canvas;
        private readonly GridManager _gridManager;

        public DraggableFactory(Canvas canvas, GridManager gridManager)
        {
            _canvas = canvas;
            _gridManager = gridManager;
        }

        public DraggableObject? Create(TileView tile, ObjectRef objectRef, DraggableObject draggablePrefab)
        {
            var draggableObject = Object.Instantiate(draggablePrefab, tile.transform);
            if (draggableObject == null)
            {
                Debug.LogError($"[DraggableFactory] No prefab found for objectId: {objectRef.Id}");
                return null;
            }
            
            draggableObject.Init(tile, _canvas, objectRef, _gridManager);
            return draggableObject;
        }
    }
}