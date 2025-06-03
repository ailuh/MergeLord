using System;
using Code.Common.EditorUtils;
using Code.Game.Logic;
using Code.Game.Logic.Interfaces;
using Code.Game.Model;
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

        private TileModel _model;
        private Action<TileView>? _clickCallback;

        public void InitializeTile(TileModel model, bool isDark, Action<TileView> onClick)
        {
            _model = model;
            _tileImage.sprite = isDark ? _tileSpriteDark : _tileSpriteLight;
            _clickCallback = onClick;
            _model.BindView(this);
        }

        public bool IsOccupied => _model.IsOccupied;
        public Vector2Int GridPosition => _model.Position;

        public void SetObject(DraggableObject obj) => _model.SetObject(obj);
        public DraggableObject? TryGetObject() => _model.ContainedObject;

        public void OnPointerClick(PointerEventData eventData) => _clickCallback?.Invoke(this);
        public TileView GetTile() => this;
    }
}
