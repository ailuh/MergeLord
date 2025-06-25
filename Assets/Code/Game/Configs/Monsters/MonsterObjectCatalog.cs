using System.Collections.Generic;
using System.Linq;
using Code.Common.EditorUtils;
using Code.Game.Logic;
using UnityEngine;

namespace Code.Game.Configs.Monsters
{
    [CreateAssetMenu(menuName = "Game/GridObjectCatalog")]
    public class MonsterObjectCatalog : ScriptableObject
    {
        [SerializeField, CantBeNull] private List<MonsterObjectConfig> _configs = null!;

        private Dictionary<string, MonsterObjectConfig>? _cache;

        private Dictionary<string, MonsterObjectConfig> Cache =>
            _cache ??= _configs.ToDictionary(c => c.Id);
    
        public DraggableObject? GetPrefab(string objectId)
        {
            return Cache != null && Cache.TryGetValue(objectId, out var config) ? config.Prefab : null;
        }

        public MonsterObjectConfig? GetConfig(string objectId)
        {
            return Cache != null && Cache.TryGetValue(objectId, out var config) ? config : null;
        }
    }
}