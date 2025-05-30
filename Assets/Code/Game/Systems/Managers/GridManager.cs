using System;
using System.Collections.Generic;
using Code.Common.EditorUtils;
using Code.Game.Configs;
using Code.Game.Configs.Monsters;
using Code.Game.Logic;
using Code.Game.Model.Rewards;
using Code.Game.Systems.Interfaces;
using Code.Game.Systems.Services;
using Code.UI.Views;
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
        private GridObjectCatalog _catalog;
        private DragDropService _dragDropService;

        public void Init(DraggableFactory factory, GridObjectCatalog catalog, DragDropService dragDropService)
        {
            _draggableFactory = factory;
            _catalog = catalog;
            _dragDropService = dragDropService;
        }
        
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
                _gameMessageService.ShowMessage(obj.ObjectRef.Description, true, obj.ObjectRef);
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
                    objId = obj.ObjectRef.Id;
                }
               
                result.Add(new TileObjectData
                {
                    ObjectId = objId,
                    Position = pos
                });
            }

            return result;
        }
        
        public TileView? GetFirstFreeTile()
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
        
        public bool TryPlaceReward(RewardData reward)
        {
            var freeTile = GetFirstFreeTile();

            if (freeTile == null)
            {
                return false;
            }
            var config = _catalog.GetConfig(reward.Id);
            if (config == null)
            {
                return false;
            }
            var objectPrefab = _catalog.GetPrefab(reward.Id);
            if (objectPrefab == null)
            {
                return false;
            }
            var obj = _draggableFactory.Create(freeTile, config.ObjectRef, objectPrefab);
            if (obj == null)
            {
                return false;

            }
            freeTile.SetObject(obj);
            _dragDropService.RegisterDraggable(obj);            
            return true;
        }
        
        public void NotifyGridChanged()
        {
            OnGridChanged?.Invoke();
        }
    }
}