using System.Collections.Generic;
using Code.Game.Configs.Interfaces;
using Code.Game.Logic;
using UnityEngine;

namespace Configs.Objects
{
    public abstract class GridObjectConfigBase : ScriptableObject
    {
        [Header("Base Info")]
        public string Id;
        [TextArea] public string Description;
        public Sprite Sprite;
        public int Level;

        [Header("Object Reference")]
        public DraggableObject Prefab;

        [Header("Upgrade Path")]
        public GridObjectConfigBase? FirstChainConfig;
        public GridObjectConfigBase? NextLevelConfig;
        
        public abstract List<IDisplayStat> GetCompactStats();
    }
}
