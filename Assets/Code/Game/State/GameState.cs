using System;
using System.Collections.Generic;
using Code.Common.Reactive;
using Code.Game.Configs;
using Code.Game.Enums;
using UnityEngine;

namespace Code.Game.State
{
    public class GameState
    {
        public ReactiveDictionary<Vector2Int, TileObjectData> Tiles { get; } = new();
        public ReactiveProperty<int> Coins { get; } = new(0);
        public ReactiveDictionary<EnergyType, ReactiveProperty<int>> Energy { get; } = new();
        public ReactiveProperty<DateTime> LastSave { get; } = new(DateTime.UtcNow);
        public ReactiveProperty<int> Level { get; } = new(1);
        public Dictionary<EnergyType, int> MaxEnergy { get; } = new();

        public GameState()
        {
            foreach (EnergyType type in Enum.GetValues(typeof(EnergyType)))
            {
                Energy[type] = new ReactiveProperty<int>(0);
                MaxEnergy[type] = 0;
            }
        }
    }
}

