using System;
using System.Collections.Generic;
using System.Linq;
using Code.Common.EditorUtils;
using Code.Game.Configs;
using Code.Game.Logic;
using Code.Game.Systems.Interfaces;
using Code.Game.Systems.Services;
using Code.UI.Views;
using Configs.Objects;
using UnityEngine;

namespace Code.Game.Systems.Managers
{
    public class GridManager : MonoBehaviour
    {
        [SerializeField, CantBeNull] private TileView _tilePrefab = null!;
        public event Action OnGridChanged;
        private readonly Dictionary<Vector2Int, TileView> _tiles = new();
        private IGameMessageService _gameMessageService = null!;
        private DraggableFactory _draggableFactory;
        private DragDropService _dragDropService;
        private TileService _tileService;

        public void Init(DraggableFactory factory, DragDropService dragDropService, TileService tileService)
        {
            _draggableFactory = factory;
            _dragDropService = dragDropService;
            _tileService = tileService;
        }
            
        public void InitializeGrid(IEnumerable<TileObjectData> tileObjectsData, IGameMessageService gameMessageService)
        {
            _tileService.InitializeTiles(tileObjectsData);

            foreach (var tileModel in _tileService.GetAllTiles())
            {
                var pos = tileModel.Position;
                var tile = Instantiate(_tilePrefab, transform);
                var isDark = (pos.x + pos.y) % 2 != 0;
                tile.InitializeTile(tileModel, isDark, OnTileClicked);
                _tiles[pos] = tile;
            }

            _gameMessageService = gameMessageService;
        }
        
        private void OnTileClicked(TileView tile)
        {
            var obj = tile.TryGetObject();
            if (obj != null)
            {
                _gameMessageService.ShowMessage(obj.Config.Description, true, obj.Config);
            }
            else
            {
                _gameMessageService.ShowMessage("Any item can be placed here");

            }
        }

        public List<TileObjectData> GetGridState()
        {
            var result = new List<TileObjectData>();

            foreach (var pair in _tiles)
            {
                var pos = pair.Key;
                var tile = pair.Value;

                var obj = tile.TryGetObject();
                var objId = String.Empty;
                if (obj != null)
                {
                    objId = obj.Config.Id;
                }
               
                result.Add(new TileObjectData
                {
                    ObjectId = objId,
                    Position = pos
                });
            }

            return result;
        }

        private TileView? GetFirstFreeTile()
        {
            foreach (var tile in _tiles.Values)
            {
                if (!tile.IsOccupied)
                {
                    return tile;
                }
            }

            return null;
        }
        
        public bool TryPlaceObject(GridObjectConfigBase configBase)
        {
            var freeTile = GetFirstFreeTile();
            if (freeTile == null) return false;

            var instance = _draggableFactory.Create(freeTile, configBase, configBase.Prefab);
            if (instance == null) return false;

            freeTile.SetObject(instance);
            _dragDropService.RegisterDraggable(instance);

            return true;
        }
        
        public bool HasFreeTile()
        {
            return _tiles.Values.Any(tile => !tile.IsOccupied);
        }
        
        public void NotifyGridChanged()
        {
            OnGridChanged?.Invoke();
        }
    }
}