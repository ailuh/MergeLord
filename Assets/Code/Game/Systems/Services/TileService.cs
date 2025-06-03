using System.Collections.Generic;
using Code.Game.Configs;
using Code.Game.Model;
using Code.Game.State;
using UnityEngine;

namespace Code.Game.Systems.Services
{
    public class TileService
    {
        private readonly Dictionary<Vector2Int, TileModel> _tiles = new();
        private readonly GameState _state;
        private readonly EnergyModel _energyModel;

        public TileService(GameState state, EnergyModel energyModel)
        {
            _state = state;
            _energyModel = energyModel;
        }

        public void InitializeTiles(IEnumerable<TileObjectData> configs)
        {
            _tiles.Clear();
            foreach (var data in configs)
            {
                var model = new TileModel(data.Position);
                if (!string.IsNullOrEmpty(data.ObjectId))
                {
                    model.SetObjectId(data.ObjectId);
                }
                _tiles[data.Position] = model;
            }

            SyncToState();
        }

        public void SetObject(Vector2Int pos, string objectId)
        {
            if (_tiles.TryGetValue(pos, out var tile))
            {
                tile.SetObjectId(objectId);
                _state.Tiles[pos] = tile;
            }
        }
        
        public TileModel? GetTile(Vector2Int pos)
        {
            return _tiles.TryGetValue(pos, out var tile) ? tile : null;
        }

        public IEnumerable<TileModel> GetAllTiles() => _tiles.Values;

        private void SyncToState()
        {
            _state.Tiles.Clear();
            foreach (var tile in _tiles.Values)
            {
                if (!tile.IsOccupied)
                {
                    _state.Tiles[tile.Position] = tile;
                }
            }
        }
    }
}
