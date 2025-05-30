using System.Collections.Generic;
using System.Linq;
using Code.Game.Configs.Quests;
using Code.Game.Model.Rewards;
using Code.Game.State;
using Code.Game.Systems.Interfaces;

namespace Code.Game.Model.Quests
{
    public class QuestModel
    {
        private readonly GameState _state;
        private readonly QuestCatalog _catalog;
        private readonly RewardBufferModel _rewardBufferModel;
        public QuestConfig? GetConfigById(string id) => _catalog.GetConfigById(id);

        public QuestModel(GameState state, QuestCatalog catalog, IQuestEventBus eventBus, RewardBufferModel rewardBufferModel)
        {
            _state = state;
            _catalog = catalog;
            _rewardBufferModel = rewardBufferModel;
            eventBus.Subscribe(OnQuestEvent);
            EnsureInitialQuests();
        }

        private void EnsureInitialQuests()
        {
            foreach (var config in _catalog.AllQuests)
            {
               if (!_state.Quests.ContainsKey(config.Id))
               {
                   _state.Quests[config.Id] = new QuestState { Id = config.Id };
               }
            }
        }

        private void OnQuestEvent(IQuestEvent questEvent)
        {
            var visibleIds = GetCurrentlyVisibleQuestIds();

            foreach (var questId in visibleIds)
            {
                if (!_state.Quests.TryGetValue(questId, out var questState))
                    continue;

                if (questState.IsCompleted)
                    continue;

                var config = _catalog.GetConfigById(questId);
                if (config == null || !questEvent.Match(config))
                    continue;

                var delta = questEvent.GetProgressDelta(config);
                if (delta == 0)
                    continue;

                questState.Progress += delta;

                if (questState.Progress >= config.ConditionValue)
                {
                    questState.IsCompleted = true;
                    ActivateNextQuests(config.NextQuests);
                }
            }
        }

        private void ActivateNextQuests(List<QuestConfig> nextQuests)
        {
            foreach (var quest in nextQuests)
            {
                if (_state.Quests.ContainsKey(quest.Id))
                    continue;

                var allMet = quest.RequiredQuestIds.All(id =>
                    _state.Quests.TryGetValue(id, out var state) && state.IsCompleted);

                if (allMet)
                {
                    _state.Quests[quest.Id] = new QuestState { Id = quest.Id };
                }
            }
        }

        public List<QuestConfig> GetActiveRegularQuests(int limit)
        {
            return _catalog.AllQuests
                .Where(cfg => cfg.Kind == QuestKind.Regular && _state.Quests.ContainsKey(cfg.Id))
                .OrderBy(cfg => cfg.Id)
                .Take(limit)
                .ToList();
        }

        public HashSet<string> GetCurrentlyVisibleQuestIds()
        {
            var ids = new HashSet<string>();

            var mainQuest = GetActiveMainQuest();
            if (mainQuest != null)
            {
                ids.Add(mainQuest.Id);
                foreach (var reqId in mainQuest.RequiredQuestIds)
                {
                    ids.Add(reqId);
                }
            }

            return ids;
        }
        
        public QuestConfig? GetActiveMainQuest()
        {
            foreach (var cfg in _catalog.AllQuests)
            {
                if (cfg.Kind != QuestKind.Main)
                    continue;

                if (!_state.Quests.TryGetValue(cfg.Id, out var questState))
                    continue;

                if (questState.IsCompleted && questState.RewardClaimed)
                    continue;

                bool allDependenciesExist = true;
                foreach (var reqId in cfg.RequiredQuestIds)
                {
                    if (!_state.Quests.ContainsKey(reqId))
                    {
                        allDependenciesExist = false;
                        break;
                    }
                }

                if (!allDependenciesExist)
                    continue;

                return cfg;
            }

            return null;
        }
        
        public QuestState GetState(string id) => _state.Quests[id];

        public bool TryClaimReward(string questId)
        {
            if (!_state.Quests.TryGetValue(questId, out var questState))
            {
                return false;
            }
            if (!questState.IsCompleted || questState.RewardClaimed)
            {
                return false;
            }

            var config = _catalog.GetConfigById(questId);
            if (config == null || config.Reward == null)
            {
                return false;
            }

            if (_rewardBufferModel.IsFull)
            {
                return false;
            }

            if (_rewardBufferModel.TryAddReward(config.Reward))
            {
                questState.RewardClaimed = true;
                return true;
            }

            return false;
        }
    }
}