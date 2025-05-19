using System.Collections.Generic;
using System.Linq;
using Code.Domain.Services;
using Code.Presentation.Interfaces;
using Code.Presentation.Views;
using UnityEngine;
using UnityEngine.EventSystems;

namespace Code.Application.Managers
{
    public class DragDropManager
    {
        private readonly MergeService _merge;

        public DragDropManager(MergeService merge)
        {
            _merge = merge;
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

            var targetTile = dragTarget.GetTile();
            if (targetTile == null)
            {
                dragged.ReturnToStart();
                return;
            }

            if (!targetTile.IsOccupied)
            {
                dragged.MoveToTile(targetTile);
            }
            else
            {
                var merged = _merge.TryMerge(dragged, targetTile.GridPosition);
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
            return (from result in results where result.gameObject.TryGetComponent<IDragTarget>(out _) select result.gameObject).FirstOrDefault();
        }
    }
}