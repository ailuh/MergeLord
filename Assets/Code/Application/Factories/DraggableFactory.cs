using Code.Infrastructure.Configs.Monsters;
using Code.Presentation.Views;
using UnityEngine;

namespace Code.Application.Factories
{
    public class DraggableFactory
    {
        private readonly Canvas _canvas;

        public DraggableFactory(Canvas canvas)
        {
            _canvas = canvas;
        }

        public DraggableObject? Create(TileView tile, ObjectRef objectRef, DraggableObject draggablePrefab)
        {
            var draggableObject = Object.Instantiate(draggablePrefab, tile.transform);
            if (draggableObject == null)
            {
                Debug.LogError($"[DraggableFactory] No prefab found for objectId: {objectRef.Id}");
                return null;
            }
            
            draggableObject.Init(tile, _canvas, objectRef);
            return draggableObject;
        }
    }
}