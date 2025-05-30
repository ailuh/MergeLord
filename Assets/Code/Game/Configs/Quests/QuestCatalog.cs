using System.Collections.Generic;
using UnityEngine;

namespace Code.Game.Configs.Quests
{
    [CreateAssetMenu(menuName = "Config/QuestCatalog")]
    public class QuestCatalog : ScriptableObject
    {
        [SerializeField]
        private List<QuestConfig> _quests = new();

        public IReadOnlyList<QuestConfig> AllQuests => _quests;

        private Dictionary<string, QuestConfig> _lookup;

        private void OnEnable()
        {
            _lookup = new Dictionary<string, QuestConfig>();
            foreach (var quest in _quests)
            {
                if (quest != null && !_lookup.ContainsKey(quest.Id))
                {
                    _lookup[quest.Id] = quest;
                }
            }
        }

        public QuestConfig? GetConfigById(string id)
        {
            _lookup.TryGetValue(id, out var config);
            return config;
        }
    }
}
