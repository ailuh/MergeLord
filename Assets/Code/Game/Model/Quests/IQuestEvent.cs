using Code.Game.Configs.Quests;

namespace Code.Game.Model.Quests
{
    public interface IQuestEvent
    {
        ConditionType Type { get; }
        bool Match(QuestConfig config);
        int GetProgressDelta(QuestConfig config);
    }
}