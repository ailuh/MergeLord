using System;
using System.Collections.Generic;
using Code.Common.Reactive;
using Code.Game.Enums;
using Code.Game.Model;
using UnityEngine;

namespace Code.Game.State
{
    public class GameState
    {
        public Dictionary<Vector2Int, TileModel> Tiles { get; } = new();
        public ReactiveDictionary<CurrencyType, ReactiveProperty<int>> Currency { get; } = new();
        public ReactiveDictionary<EnergyType, ReactiveProperty<int>> Energy { get; } = new();
        public ReactiveProperty<DateTime> LastSave { get; } = new(DateTime.UtcNow);
        public ReactiveProperty<int> Level { get; } = new(1);
        public Dictionary<EnergyType, int> MaxEnergy { get; } = new();
        public ReactiveDictionary<string, QuestState> Quests { get; } = new();
        public BuildingShopState BuildingShop { get; } = new();
        public GameState()
        {
            foreach (EnergyType type in Enum.GetValues(typeof(EnergyType)))
            {
                Energy[type] = new ReactiveProperty<int>(0);
                MaxEnergy[type] = 0;
            }
            foreach (CurrencyType type in System.Enum.GetValues(typeof(CurrencyType)))
            {
                Currency[type] = new ReactiveProperty<int>(0);
            }
        }
    }
}

