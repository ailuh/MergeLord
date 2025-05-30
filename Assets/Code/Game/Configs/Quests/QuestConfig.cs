using System.Collections.Generic;
using Code.Game.Model.Rewards;
using UnityEngine;

namespace Code.Game.Configs.Quests
{
    [CreateAssetMenu(menuName = "Config/QuestConfig")]
    public class QuestConfig : ScriptableObject
    {
        public string Id;
        public string Description;
        public QuestKind Kind;
        public ConditionType ConditionType;
        public string? ConditionTargetId;
        public int ConditionValue;
        public List<string> RequiredQuestIds;
        public List<QuestConfig> NextQuests;
        public RewardData Reward;
    }
    
    public enum ConditionType
    {
        MergeCount,
        BuildStructure
    }
    
    public enum QuestKind
    {
        Main,
        Regular
    }
}
