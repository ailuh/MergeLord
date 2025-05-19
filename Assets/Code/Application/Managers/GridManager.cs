using System.Collections.Generic;
using Code.Application.Interfaces;
using Code.Infrastructure.Configs;
using Code.Presentation.Views;
using Code.Utils;
using UnityEngine;

namespace Code.Application.Managers
{
    public class GridManager : MonoBehaviour
    {
        [SerializeField, CantBeNull] private TileView _tilePrefab = null!;

        private readonly Dictionary<Vector2Int, TileView> _tiles = new();
        private IGameMessageService _gameMessageService = null!;
        
        public void InitializeGrid(IEnumerable<TileObjectData> tileObjectsData, IGameMessageService gameMessageService)
        {
            foreach (var tileObject in tileObjectsData)
            {
                var pos = new Vector2Int(tileObject.Position.x, tileObject.Position.y);
                var tile = Instantiate(_tilePrefab, transform);
                var isDark = (pos.x + pos.y) % 2 != 0;
                tile.InitializeTile(isDark, OnTileClicked);
                tile.SetGridPosition(pos);
                _tiles[pos] = tile;
            }

            _gameMessageService = gameMessageService;
        }

        public TileView? GetTileAt(Vector2Int pos)
            => _tiles.TryGetValue(pos, out var tile) ? tile : null;
        
        private void OnTileClicked(TileView tile)
        {
            var obj = tile.TryGetObject();
            if (obj != null)
            {
                _gameMessageService.ShowMessage(obj.ObjectRef.Description, true);
            }
            else
            {
                _gameMessageService.ShowMessage("Any item can be placed here");

            }
        }
    }
}