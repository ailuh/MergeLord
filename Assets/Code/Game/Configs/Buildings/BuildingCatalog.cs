using System.Collections.Generic;
using System.Linq;
using Code.Game.Logic;
using UnityEngine;

namespace Code.Game.Configs.Buildings
{
    [CreateAssetMenu(fileName = "BuildingCatalog", menuName = "Game/Building Catalog")]
    public class BuildingCatalog : ScriptableObject
    {
        [SerializeField] private List<BuildingConfig> _allBuildings;
        public List<BuildingConfig> AllBuildings => _allBuildings;

        private Dictionary<string, BuildingConfig>? _cache;
        private Dictionary<string, BuildingConfig> Cache => _cache ??= _allBuildings.ToDictionary(b => b.Id);

        public BuildingConfig? GetById(string id)
        {
            return Cache.TryGetValue(id, out var config) ? config : null;
        }
        
        public BuildingConfig? GetConfig(string id)
        {
            return Cache.TryGetValue(id, out var config) ? config : null;
        }

        public DraggableObject? GetPrefab(string id)
        {
            return Cache.TryGetValue(id, out var config) ? config.Prefab : null;
        }
    }
}
