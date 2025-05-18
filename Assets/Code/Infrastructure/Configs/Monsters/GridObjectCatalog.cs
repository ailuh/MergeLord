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
        [SerializeField, CantBeNull] private List<GridObjectConfig> _configs;

        private Dictionary<string, GridObjectConfig> _cache;

        private void Init()
        {
            _cache = _configs.ToDictionary(c => c.Id);
        }

        public DraggableObject? GetPrefab(string objectId)
        {
            if (_cache == null)
            {
                Init();
            }
            return _cache != null && _cache.TryGetValue(objectId, out var config) ? config.ObjectPrefab : null;
        }

        public GridObjectConfig? GetConfig(string objectId)
        {
            if (_cache == null)
            {
                Init();
            }
            return _cache != null && _cache.TryGetValue(objectId, out var config) ? config : null;
        }
    }
}