using System;
using Code.Common.EditorUtils;
using Code.Game.Configs.Monsters;
using Code.Game.Model;
using Code.Game.Systems.Managers;
using UnityEngine;
using UnityEngine.EventSystems;

namespace Code.Game.Logic
{
    public class DraggableObject : MonoBehaviour, IBeginDragHandler, IDragHandler, IEndDragHandler
    {
        [SerializeField, CantBeNull] private RectTransform _rectTransform = null!;
        [SerializeField, CantBeNull] private CanvasGroup _canvasGroup = null!;

        public ObjectRef ObjectRef { get; private set; }

        public event Action<DraggableObject, PointerEventData>? OnDragEnded;

        private TileModel _currentModel = null!;
        private Canvas _canvas = null!;
        private GridManager _gridManager = null!;

        public void Init(TileModel tileModel, Canvas canvas, ObjectRef objectRef, GridManager gridManager)
        {
            _currentModel = tileModel;
            _canvas = canvas;
            _gridManager = gridManager;
            ObjectRef = objectRef;

            MoveToTile(tileModel, true);
        }

        public void OnBeginDrag(PointerEventData eventData)
        {
            _canvasGroup.blocksRaycasts = false;
            transform.SetParent(_canvas.transform);
            _currentModel.ClearObject();
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

        public void MoveToTile(TileModel newTileModel, bool isInit = false)
        {
            var view = newTileModel.GetView();
            if (view != null)
            {
                transform.SetParent(view.transform, false);
                transform.localPosition = Vector3.zero;
            }

            _currentModel.ClearObject();
            newTileModel.SetObject(this);
            _currentModel = newTileModel;

            if (!isInit)
            {
                _gridManager.NotifyGridChanged();
            }
        }

        public void ReturnToStart()
        {
            MoveToTile(_currentModel);
        }
    }
}