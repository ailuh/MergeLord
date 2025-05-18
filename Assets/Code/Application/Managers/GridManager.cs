using System.Collections.Generic;
using Code.Infrastructure.Configs;
using Code.Presentation.Views;
using Code.Utils;
using UnityEngine;

namespace Code.Application.Managers
{
    public class GridManager : MonoBehaviour
    {
        [SerializeField, CantBeNull] private TileView _tilePrefab;

        private readonly Dictionary<Vector2Int, TileView> _tiles = new();
        public void GenerateGrid(IEnumerable<TileObjectData> tileObjectDatas)
        {
            foreach (var tileObject in tileObjectDatas)
            {
                var pos = new Vector2Int(tileObject.Position.x, tileObject.Position.y);
                var tile = Instantiate(_tilePrefab, transform);
                tile.SetGridPosition(pos);
                _tiles[pos] = tile;
            }
        }

        public TileView? GetTileAt(Vector2Int pos)
            => _tiles.TryGetValue(pos, out var tile) ? tile : null;
        
        public TileView? GetTileUnderPointer(GameObject target)
        {
            return target.GetComponentInParent<TileView>();
        }
        
        public IEnumerable<Vector2Int> AllPositions() => _tiles.Keys;
    }
}