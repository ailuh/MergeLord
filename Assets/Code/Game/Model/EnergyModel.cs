using System;
using System.Collections.Generic;
using Code.Game.Configs;
using Code.Game.Configs.DataModels;
using Code.Game.Configs.Monsters;
using Code.Game.Enums;
using Code.Game.State;

namespace Code.Game.Model
{
    public class EnergyModel
    {
        private readonly GameState _state;
        private readonly GridObjectCatalog _catalog;
        private DateTime _lastTickTime;

        public EnergyModel(GameState state, GridObjectCatalog catalog)
        {
            _state = state;
            _catalog = catalog;
            _lastTickTime = DateTime.UtcNow;
        }

        public void InitFromDefault(LevelStartupConfig config)
        {
            foreach (var entry in config.DefaultEnergy)
            {
                _state.Energy[entry.Type].Value = entry.Initial;
                _state.MaxEnergy[entry.Type] = entry.Max;
            }
        }

        public void LoadFromSave(Dictionary<EnergyType, EnergyValue> savedEnergy)
        {
            foreach (var pair in savedEnergy)
            {
                _state.Energy[pair.Key].Value = pair.Value.Current;
                _state.MaxEnergy[pair.Key] = pair.Value.Max;
            }
        }

        public void ApplyOfflineProgress(DateTime lastSave, DateTime now)
        {
            var secondsPassed = (now - lastSave).TotalSeconds;

            foreach (var tile in _state.Tiles.Values)
            {
                var config = _catalog.GetConfig(tile.ObjectId);
                if (config == null || config.ObjectRef.Energy.TickSeconds <= 0)
                {
                    continue;
                }

                var ticks = (int)(secondsPassed / config.ObjectRef.Energy.TickSeconds);
                var total = ticks * config.ObjectRef.Energy.EnergyPerTick;

                AddEnergy(config.ObjectRef.Energy.Type, total);
            }
        }

        public void TickOnlineGeneration()
        {
            var now = DateTime.UtcNow;
            var delta = (now - _lastTickTime).TotalSeconds;
            if (delta < 1) return;

            foreach (var tile in _state.Tiles.Values)
            {
                var config = _catalog.GetConfig(tile.ObjectId);
                if (config == null || config.ObjectRef.Energy.TickSeconds <= 0)
                {
                    continue;
                }

                if (delta >= config.ObjectRef.Energy.TickSeconds)
                {
                    AddEnergy(config.ObjectRef.Energy.Type, config.ObjectRef.Energy.EnergyPerTick);
                }
            }

            _lastTickTime = now;
        }

        private void AddEnergy(EnergyType type, int amount)
        {
            var energy = _state.Energy[type];
            var max = _state.MaxEnergy.TryGetValue(type, out var maxVal) ? maxVal : int.MaxValue;
            energy.Value = Math.Min(energy.Value + amount, max);
        }

        public bool TrySpendEnergy(EnergyType type, int amount)
        {
            var current = _state.Energy[type].Value;
            if (current < amount)
            {
                return false;
            }

            _state.Energy[type].Value -= amount;
            return true;
        }
    }
}
