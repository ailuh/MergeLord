using Code.Game.Configs.Quests;
using Code.Game.Model.Quests;

namespace Code.Game.Logic.Events
{
    public class MergeHappenedEvent : IQuestEvent
    {
        public string MergedObjectId { get; }
        public int Count { get; }

        public MergeHappenedEvent(string mergedObjectId, int count = 1)
        {
            MergedObjectId = mergedObjectId;
            Count = count;
        }

        public ConditionType Type => ConditionType.MergeCount;

        public bool Match(QuestConfig config)
        {
            return config.ConditionType == Type &&
                   (string.IsNullOrEmpty(config.ConditionTargetId) || config.ConditionTargetId == MergedObjectId);
        }

        public int GetProgressDelta(QuestConfig config)
        {
            return Match(config) ? Count : 0;
        }
    }
}
