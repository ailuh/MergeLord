using Code.Application.Managers;
using Code.Infrastructure.Configs.Monsters;
using Code.Presentation.Views;
using UnityEngine;
using VContainer;

namespace Code.Application.Factories
{
    public class DraggableFactory
    {
        private readonly IObjectResolver _resolver;
        private readonly Canvas _canvas;
        private readonly GridManager _gridManager;
        private readonly GridObjectCatalog _catalog;

        [Inject]
        public DraggableFactory(GridObjectCatalog catalog, Canvas canvas, GridManager gridManager)
        {
            _canvas = canvas;
            _gridManager = gridManager;
            _catalog = catalog;
        }

        public void Create(TileView tile, string objectId)
        {
            var prefab = _catalog.GetPrefab(objectId);
            if (prefab == null)
            {
                Debug.LogError($"[DraggableFactory] No prefab found for objectId: {objectId}");
                return;
            }

            var draggableObject = Object.Instantiate(prefab, tile.transform);
            draggableObject.Init(tile, _canvas, _gridManager, objectId);
        }
    }
}