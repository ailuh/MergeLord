using System;
using System.Collections.Generic;
using Code.Common.Reactive;
using Code.Game.Configs;
using Code.Game.Configs.DataModels;
using Code.Game.Enums;
using Code.Game.State;
using Code.Game.Systems.Services;

namespace Code.Game.Model
{
    public class EnergyModel
    {
        private readonly GameState _state;
        private TileService _tileService;
        private DateTime _lastTickTime;
        public event Action<EnergyType, int> OnEnergyGained;

        public EnergyModel(GameState state)
        {
            _state = state;
            _lastTickTime = DateTime.UtcNow;
        }

        public void InitTileService(TileService tileService)
        {
            _tileService = tileService;
        }
        
        public void InitFromDefault(LevelStartupConfig startup)
        {
            _state.Energy.Clear();
            _state.MaxEnergy.Clear();

            foreach (var entry in startup.DefaultEnergy)
            {
                _state.Energy[entry.Type] = new ReactiveProperty<int>(entry.Initial);
                _state.MaxEnergy[entry.Type] = entry.Max;
            }
        }

        public void LoadFromSave(Dictionary<EnergyType, EnergyValue> savedValues)
        {
            foreach (var pair in savedValues)
            {
                _state.Energy[pair.Key] = new ReactiveProperty<int>(pair.Value.Current);
            }
        }

        public void ApplyOfflineProgress(DateTime lastSaveUtc, DateTime now)
        {
            var delta = (now - lastSaveUtc).TotalSeconds;

            foreach (var tile in _tileService.GetAllTiles())
            {
                tile.Tick(delta, AddEnergy);
            }
        }
        
        public void TickOnlineGeneration()
        {
            var now = DateTime.UtcNow;
            var delta = (now - _lastTickTime).TotalSeconds;
            _lastTickTime = now;

            if (delta <= 0) return;

            foreach (var tile in _state.Tiles.Values)
            {
                tile.Tick(delta, AddEnergy);
            }
        }

        private void AddEnergy(EnergyType type, int amount)
        {
            if (!_state.Energy.TryGetValue(type, out var energy)) return;

            var max = _state.MaxEnergy.TryGetValue(type, out var maxVal) ? maxVal : int.MaxValue;
            energy.Value = Math.Min(energy.Value + amount, max);
            
        }
    }
}
