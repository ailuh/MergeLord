using System;
using Code.Common.EditorUtils;
using Code.Game.Logic;
using Code.Game.Logic.Interfaces;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

namespace Code.UI.Views
{
    public class TileView : MonoBehaviour, IDragTarget, IPointerClickHandler
    {
        [SerializeField, CantBeNull] private Sprite _tileSpriteLight = null!;
        [SerializeField, CantBeNull] private Sprite _tileSpriteDark = null!;
        [SerializeField, CantBeNull] private Image _tileImage = null!;

        private DraggableObject? _contained;
        private Action<TileView>? _clickCallback;
        public Vector2Int GridPosition { get; private set; }

        public void InitializeTile(bool isDark, Action<TileView> callback)
        {
            _tileImage.sprite = isDark ? _tileSpriteDark : _tileSpriteLight;
            _clickCallback = callback;
            
        }

        public bool IsOccupied => _contained != null;

        public void SetGridPosition(Vector2Int pos) => GridPosition = pos;

        public void SetObject(DraggableObject obj) => _contained = obj;
        public void ClearObject() => _contained = null;
        public DraggableObject? TryGetObject() => _contained;
        public TileView GetTile() => this;
        public void OnPointerClick(PointerEventData eventData)
        {
            _clickCallback?.Invoke(this);
        }
    }
}
