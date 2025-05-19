using System;
using Code.Infrastructure.Configs.Monsters;
using Code.Utils;
using UnityEngine;
using UnityEngine.EventSystems;

namespace Code.Presentation.Views
{
    public class DraggableObject : MonoBehaviour, IBeginDragHandler, IDragHandler, IEndDragHandler
    {
        [SerializeField, CantBeNull] private RectTransform _rectTransform = null!;
        [SerializeField, CantBeNull] private CanvasGroup _canvasGroup = null!;
        
        public ObjectRef ObjectRef { get; private set; }
        public event Action<DraggableObject, PointerEventData> OnDragEnded = null!;
        
        private TileView _currentTile = null!;
        private Canvas _canvas = null!;

        public void Init(TileView tile, Canvas canvas, ObjectRef objectId)
        {
            _currentTile = tile;
            _canvas = canvas;
            ObjectRef = objectId;

            MoveToTile(tile);
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

        public void MoveToTile(TileView newTile)
        {
            transform.SetParent(newTile.transform, false); 
            transform.localPosition = Vector3.zero;
            newTile.SetObject(this);
            _currentTile = newTile;
        }

        public void ReturnToStart()
        {
            MoveToTile(_currentTile);
        }
    }
}