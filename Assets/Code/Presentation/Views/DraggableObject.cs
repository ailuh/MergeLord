using Code.Application.Managers;
using Code.Presentation.Interfaces;
using Code.Utils;
using UnityEngine;
using UnityEngine.EventSystems;

namespace Code.Presentation.Views
{
    public class DraggableObject : MonoBehaviour, IBeginDragHandler, IDragHandler, IEndDragHandler
    {
        [SerializeField, CantBeNull] private RectTransform _rectTransform = null!;
        [SerializeField, CantBeNull] private CanvasGroup _canvasGroup = null!;
        
        private Vector3 _startPosition;
        private Canvas _canvas;
        private TileView _currentTile;

        private string _unitId = "";
        public string UnitId => _unitId;

        public void Init(TileView tile, Canvas canvas, GridManager gridManager, string unitId)
        {
            _unitId = unitId;
            _currentTile = tile;
            _canvas = canvas;
            transform.localPosition = Vector2.zero;
            tile.SetObject(this);
        }

        public void OnBeginDrag(PointerEventData eventData)
        {
            _startPosition = transform.localPosition;
            transform.SetParent(_canvas.transform);
            _canvasGroup.blocksRaycasts = false;
            _currentTile.ClearObject();
        }

        public void OnDrag(PointerEventData eventData)
        {
            _rectTransform.anchoredPosition += eventData.delta / _canvas.scaleFactor;
        }

        public void OnEndDrag(PointerEventData eventData)
        {
            _canvasGroup.blocksRaycasts = true;

            var pointerTarget = eventData.pointerEnter;
            var targetTile = pointerTarget != null && pointerTarget.TryGetComponent<IDragTarget>(out var dragTarget)
                ? dragTarget.GetTile()
                : null;
            if (targetTile != null && !targetTile.IsOccupied)
            {
                targetTile.SetObject(this);
                _currentTile = targetTile;
                transform.SetParent(_currentTile.transform);
                transform.localPosition = Vector2.zero;
            }
            else
            {
                transform.SetParent(_currentTile.transform);
                transform.localPosition = _startPosition;
                _currentTile.SetObject(this);
            }
        }
    }
}