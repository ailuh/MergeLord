using System.Collections.Generic;
using Code.Game.Configs.Interfaces;
using UnityEngine;

namespace Code.Game.Configs.Monsters
{
    public class MonsterInfoAdapter : IUpgradeableObjectInfo
    {
        private readonly MonsterObjectConfig _config;

        public MonsterInfoAdapter(MonsterObjectConfig config)
        {
            _config = config;
        }

        public string Title => $"Level {_config.Level}";
        public string Description => _config.Description;
        public Sprite Icon => _config.Sprite;

        public Dictionary<string, string> GetDisplayStats()
        {
            var stats = new Dictionary<string, string>();
            var energy = _config.Energy;

            if (energy != null)
            {
                stats["Generates"] = $"{energy.EnergyPerTick} {energy.Type}";
                stats["Tick"] = $"{energy.TickSeconds}s";
            }

            return stats;
        }
    }
}