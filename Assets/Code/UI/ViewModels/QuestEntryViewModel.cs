using Code.Game.Configs.Quests;
using Code.Game.Model.Quests;
using Code.Game.State;
using UnityEngine;

namespace Code.UI.ViewModels
{
    public class QuestEntryViewModel
    {
        private readonly QuestState _state;
        private readonly QuestConfig _config;
        private readonly QuestModel _model;
        
        public int CurrentProgress => _state.Progress;
        public int TargetProgress => _config.ConditionValue;
        public string Id => _config.Id;
        public string Description => _config.Description;
        public Sprite RewardIcon => _config.Reward.Icon;
        public bool IsCompleted => _state.IsCompleted;
        public bool RewardClaimed => _state.RewardClaimed;

        public QuestEntryViewModel(QuestState state, QuestConfig config, QuestModel model)
        {
            _state = state;
            _config = config;
            _model = model;
        }

        public void TryClaimReward()
        {
            _model.TryClaimReward(_config.Id);
        }
    }
}
