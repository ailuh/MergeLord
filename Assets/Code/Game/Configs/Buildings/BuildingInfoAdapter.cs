using System.Collections.Generic;
using Code.Game.Configs.Interfaces;
using UnityEngine;

namespace Code.Game.Configs.Buildings
{
    public class BuildingInfoAdapter : IUpgradeableObjectInfo
    {
        private readonly BuildingConfig _config;

        public BuildingInfoAdapter(BuildingConfig config)
        {
            _config = config;
        }

        public string Title => $"Level {_config.Level}";
        public string Description => _config.DisplayName;
        public Sprite Icon => _config.Sprite;

        public Dictionary<string, string> GetDisplayStats()
        {
            var stats = new Dictionary<string, string>();

            foreach (var cost in _config.Costs)
            {
                stats[$"Cost ({cost.Currency})"] = $"-{cost.Amount}";
            }

            foreach (var energy in _config.EnergyCosts)
            {
                stats[$"Spawns with ({energy.EnergyType})"] = $"-{energy.Amount}";
            }

            return stats;
        }
    }
}