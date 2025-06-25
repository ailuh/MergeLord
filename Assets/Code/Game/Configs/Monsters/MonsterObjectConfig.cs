using System;
using System.Collections.Generic;
using Code.Game.Configs.Interfaces;
using Code.Game.Enums;
using Code.UI.Stats;
using Configs.Objects;
using UnityEngine;

namespace Code.Game.Configs.Monsters
{
    [CreateAssetMenu(menuName = "Unit/GridObjectConfig")]
    public class MonsterObjectConfig : GridObjectConfigBase, IObjectInfoChainProvider
    {
        [Header("Energy Generation")] 
        [SerializeField] private EnergyGenerationConfig _energy;
        [SerializeField] private Sprite _energyIcon;
        public EnergyGenerationConfig Energy => _energy;
        
        public List<GridObjectConfigBase> GetUpgradeChain()
        {
            var chain = new List<GridObjectConfigBase>();
            var current = FirstChainConfig;
            while (current != null)
            {
                chain.Add(current);
                current = current.NextLevelConfig as MonsterObjectConfig;
            }

            return chain;
        }
        
        public override List<IDisplayStat> GetCompactStats()
        {
            return new List<IDisplayStat>
            {
                new StatIcon($"{Energy.EnergyPerTick}", _energyIcon),
            };
        }
    }

    [Serializable]
    public class EnergyGenerationConfig
    {
        public EnergyType Type;
        public int EnergyPerTick;
        public float TickSeconds;
    }
}