using System;
using System.Collections.Generic;
using Code.Game.Configs.Interfaces;
using Code.Game.Enums;
using Code.UI.Stats;
using Configs.Objects;
using UnityEngine;

namespace Code.Game.Configs.Buildings
{
    [CreateAssetMenu(fileName = "BuildingConfig", menuName = "Game/Building Config")]
    public class BuildingConfig : GridObjectConfigBase, IObjectInfoChainProvider
    {
        [Header("Display")]
        [SerializeField] private string _displayName;

        [Header("Unlock")]
        [SerializeField] private List<CurrencyCost> _costs;
        [SerializeField] private int _requiredLevel;

        [Header("Spawn Settings")]
        [SerializeField] private List<EnergyCost> _energyCosts;
        [SerializeField] private Sprite _energyIcon;

        public string DisplayName => _displayName;
        public List<CurrencyCost> Costs => _costs;
        public int RequiredLevel => _requiredLevel;
        public List<EnergyCost> EnergyCosts => _energyCosts;

        public List<GridObjectConfigBase> GetUpgradeChain()
        {
            var chain = new List<GridObjectConfigBase>();
            var current = this;
            while (current != null)
            {
                chain.Add(current);
                current = current.NextLevelConfig as BuildingConfig;
            }

            return chain;
        }
        
        public override List<IDisplayStat> GetCompactStats()
        {
            var result = new List<IDisplayStat>();
            foreach (var cost in EnergyCosts)
            {
                result.Add(new StatIcon($"-{cost.Amount}", _energyIcon));
            }
            return result;
        }
    }

    [Serializable]
    public class CurrencyCost
    {
        public CurrencyType Currency;
        public int Amount;
    }

    [Serializable]
    public class EnergyCost
    {
        [SerializeField] private EnergyType _energyType;
        [SerializeField] private int _amount;

        public EnergyType EnergyType => _energyType;
        public int Amount => _amount;
    }
}