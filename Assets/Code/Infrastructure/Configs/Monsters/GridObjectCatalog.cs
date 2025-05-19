using System.Collections.Generic;
using System.Linq;
using Code.Presentation.Views;
using Code.Utils;
using UnityEngine;

namespace Code.Infrastructure.Configs.Monsters
{
    [CreateAssetMenu(menuName = "Game/GridObjectCatalog")]
    public class GridObjectCatalog : ScriptableObject
    {
        [SerializeField, CantBeNull] private List<GridObjectConfig> _configs = null!;

        private Dictionary<string, GridObjectConfig>? _cache;

        private Dictionary<string, GridObjectConfig> Cache =>
            _cache ??= _configs.ToDictionary(c => c.ObjectRef.Id);
    
        public DraggableObject? GetPrefab(string objectId)
        {
            return Cache != null && Cache.TryGetValue(objectId, out var config) ? config.ObjectPrefab : null;
        }

        public GridObjectConfig? GetConfig(string objectId)
        {
            return Cache != null && Cache.TryGetValue(objectId, out var config) ? config : null;
        }
    }
}