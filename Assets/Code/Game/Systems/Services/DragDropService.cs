using System.Collections.Generic;
using Code.Game.Logic;
using Code.Game.Logic.Interfaces;
using UnityEngine;
using UnityEngine.EventSystems;

namespace Code.Game.Systems.Services
{
    public class DragDropService
    {
        private readonly MergeService _mergeService;
        private readonly TileService _tileService;

        public DragDropService(MergeService mergeService, TileService tileService)
        {
            _mergeService = mergeService;
            _tileService = tileService;
        }

        public void RegisterDraggable(DraggableObject obj)
        {
            obj.OnDragEnded += HandleDrop;
        }

        private void HandleDrop(DraggableObject dragged, PointerEventData eventData)
        {
            var target = FindTargetUnderPointer(eventData);
            if (target == null || !target.TryGetComponent<IDragTarget>(out var dragTarget))
            {
                dragged.ReturnToStart();
                return;
            }

            var targetTileView = dragTarget.GetTile();
            if (targetTileView == null)
            {
                dragged.ReturnToStart();
                return;
            }

            var targetModel = _tileService.GetTile(targetTileView.GridPosition);
            if (targetModel == null)
            {
                dragged.ReturnToStart();
                return;
            }

            if (!targetModel.IsOccupied)
            {
                dragged.MoveToTile(targetModel);
                _tileService.SetObject(targetModel.Position, dragged.ObjectRef.Id);
            }
            else
            {
                var merged = _mergeService.TryMerge(dragged, targetModel.Position);
                if (merged != null)
                {
                    RegisterDraggable(merged);
                    return;
                }

                dragged.ReturnToStart();
            }
        }

        private GameObject? FindTargetUnderPointer(PointerEventData eventData)
        {
            var results = new List<RaycastResult>();
            EventSystem.current.RaycastAll(eventData, results);

            foreach (var result in results)
            {
                if (result.gameObject.TryGetComponent<IDragTarget>(out _))
                {
                    return result.gameObject;
                }
            }

            return null;
        }
    }
}