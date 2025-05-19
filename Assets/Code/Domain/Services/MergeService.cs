using Code.Application.Factories;
using Code.Application.Interfaces;
using Code.Application.Managers;
using Code.Infrastructure.Configs.Monsters;
using Code.Presentation.Views;
using UnityEngine;

namespace Code.Domain.Services
{
    public class MergeService
    {
        private readonly GridManager _grid;
        private readonly DraggableFactory _factory;
        private readonly GridObjectCatalog _catalog;
        private readonly IGameMessageService _messageService;
        
        public MergeService(GridManager grid, DraggableFactory factory, GridObjectCatalog catalog, IGameMessageService messageService)
        {
            _grid = grid;
            _factory = factory;
            _catalog = catalog;
            _messageService = messageService;
        }

        public DraggableObject? TryMerge(DraggableObject fromPosDraggableObject, Vector2Int toPos)
        {
            var toTile = _grid.GetTileAt(toPos);

            if (toTile == null)
            {
                return null;
            }

            if (!toTile.IsOccupied)
            {
                return null;
            }

            var toObj = toTile.TryGetObject();

            if (fromPosDraggableObject == null || toObj == null)
            {
                return null;
            }

            if (fromPosDraggableObject.ObjectRef.Id != toObj.ObjectRef.Id)
            {
                _messageService.ShowMessage("Objects cannot be merged!");
                return null;
            }

            var config = _catalog.GetConfig(toObj.ObjectRef.Id);
            if (config == null || config.NextLevelObject == null)
            {
                _messageService.ShowMessage("Maximum level reached, merge impossible!");
                return null;
            }
            var objectPrefab = _catalog.GetPrefab(config.NextLevelObject.ObjectRef.Id);
            if (objectPrefab == null)
            {
                return null;
            }
            Object.Destroy(fromPosDraggableObject.gameObject);
            Object.Destroy(toObj.gameObject);
            toTile.ClearObject();
            
            var newObject = _factory.Create(toTile, config.NextLevelObject.ObjectRef, objectPrefab);
            _messageService.ShowMessage("Merge is successful!");
            return newObject;
        }
    }
}
