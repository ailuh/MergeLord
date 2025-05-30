using System.Collections.Generic;
using Code.Game.State;

namespace Code.Game.Configs.Quests
{
    public class QuestGroupState
    {
        public string ChainGroupId;
        public Dictionary<string, QuestState> Quests = new();
        public bool FinalRewardClaimed;
    }
}