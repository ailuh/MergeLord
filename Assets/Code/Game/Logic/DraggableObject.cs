using System;
using Code.Common.EditorUtils;
using Code.Game.Configs.Monsters;
using Code.Game.Systems.Managers;
using Code.UI.Views;
using UnityEngine;
using UnityEngine.EventSystems;

namespace Code.Game.Logic
{
    public class DraggableObject : MonoBehaviour, IBeginDragHandler, IDragHandler, IEndDragHandler
    {
        [SerializeField, CantBeNull] private RectTransform _rectTransform = null!;
        [SerializeField, CantBeNull] private CanvasGroup _canvasGroup = null!;
        
        public ObjectRef ObjectRef { get; private set; }
        public event Action<DraggableObject, PointerEventData> OnDragEnded = null!;
        
        private TileView _currentTile = null!;
        private Canvas _canvas = null!;
        private GridManager _gridManager;
        
        public void Init(TileView tile, Canvas canvas, ObjectRef objectId, GridManager gridManager)
        {
            _currentTile = tile;
            _canvas = canvas;
            _gridManager = gridManager;
            ObjectRef = objectId;

            MoveToTile(tile, true);
            tile.SetObject(this);
        }
        
        public void OnBeginDrag(PointerEventData eventData)
        {
            _canvasGroup.blocksRaycasts = false;
            transform.SetParent(_canvas.transform);
            _currentTile.ClearObject();
        }

        public void OnDrag(PointerEventData eventData)
        {
            _rectTransform.anchoredPosition += eventData.delta / _canvas.scaleFactor;
        }

        public void OnEndDrag(PointerEventData eventData)
        {
            _canvasGroup.blocksRaycasts = true;
            OnDragEnded?.Invoke(this, eventData);
        }

        public void MoveToTile(TileView newTile, bool isInit = false)
        {
            transform.SetParent(newTile.transform, false); 
            transform.localPosition = Vector3.zero;
            newTile.SetObject(this);
            _currentTile = newTile;
            if (!isInit)
            {
                _gridManager.NotifyGridChanged();
            }
        }

        public void ReturnToStart()
        {
            MoveToTile(_currentTile);
        }
    }
}