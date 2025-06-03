using Code.Game.Configs.Monsters;
using Code.Game.Logic;
using Code.Game.Logic.Events;
using Code.Game.Systems.Interfaces;
using UnityEngine;

namespace Code.Game.Systems.Services
{
    public class MergeService
    {
        private readonly TileService _tileService;
        private readonly DraggableFactory _factory;
        private readonly GridObjectCatalog _catalog;
        private readonly IGameMessageService _messageService;
        private readonly IQuestEventBus _questEventBus;
        
        public MergeService(TileService tileService, DraggableFactory factory, GridObjectCatalog catalog, IGameMessageService messageService, IQuestEventBus questEventBus)
        {
            _tileService = tileService;
            _factory = factory;
            _catalog = catalog;
            _messageService = messageService;
            _questEventBus = questEventBus;
        }

        public DraggableObject? TryMerge(DraggableObject fromPosDraggableObject, Vector2Int toPos)
        {
            var toTileModel = _tileService.GetTile(toPos);

            if (toTileModel == null || !toTileModel.IsOccupied)
            {
                return null;
            }

            var toObj = toTileModel.ContainedObject;

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
            toTileModel.ClearObject();
            _questEventBus.Raise(new MergeHappenedEvent(fromPosDraggableObject.ObjectRef.Id));

            var tileView = toTileModel.GetView();
            var newObject = _factory.Create(tileView, config.NextLevelObject.ObjectRef, objectPrefab);
            _tileService.SetObject(toPos, config.NextLevelObject.ObjectRef.Id);

            _messageService.ShowMessage("Merge is successful!");
            return newObject;
        }
    }
}
