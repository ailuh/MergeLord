using System;
using System.Collections.Generic;
using Code.Game.Enums;

namespace Code.Game.Configs.DataModels
{
    [Serializable]
    public class GameSaveData
    {
        public List<TileObjectData> Grid;
        public Dictionary<EnergyType, EnergyValue> Energy = new();
        public int Level;
        public DateTime LastSaveUtc;
    }
}