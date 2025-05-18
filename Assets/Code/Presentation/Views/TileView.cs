using Code.Presentation.Interfaces;
using UnityEngine;

namespace Code.Presentation.Views
{
    public class TileView : MonoBehaviour, IDragTarget
    {
        private Vector2Int _gridPosition;
        private DraggableObject? _contained;

        public Vector2Int GridPosition => _gridPosition;
        public bool IsOccupied => _contained != null;

        public void SetGridPosition(Vector2Int pos) => _gridPosition = pos;

        public void SetObject(DraggableObject obj) => _contained = obj;
        public void ClearObject() => _contained = null;
        public DraggableObject? GetObject() => _contained;
        public TileView GetTile() => this;
    }
}
