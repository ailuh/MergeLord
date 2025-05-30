using System;
using System.Collections.Generic;
using System.Linq;
using Code.Game.Configs.Quests;
using Code.Game.Model.Quests;

namespace Code.UI.ViewModels
{
    public class QuestViewModel
    {
        public List<QuestEntryViewModel> QuestEntries { get; private set;}
        public QuestEntryViewModel? FinalRewardEntry { get; private set;}
        public int CompletedCount => QuestEntries.Count(q => q.IsCompleted);

        private readonly QuestModel _model;

        public QuestViewModel(QuestModel model, List<QuestConfig> allConfigs)
        {
            _model = model;

            QuestEntries = allConfigs
                .Select(cfg => new QuestEntryViewModel(_model.GetState(cfg.Id), cfg, model))
                .ToList();

            var mainConfig = model.GetActiveMainQuest();
            if (mainConfig != null)
            {
                FinalRewardEntry = new QuestEntryViewModel(_model.GetState(mainConfig.Id), mainConfig, model);
            }
        }

        private void Refresh()
        {
            var mainConfig = _model.GetActiveMainQuest();

            if (mainConfig != null)
            {
                FinalRewardEntry = new QuestEntryViewModel(_model.GetState(mainConfig.Id), mainConfig, _model);

                QuestEntries = mainConfig.RequiredQuestIds
                    .Select(id =>
                    {
                        var cfg = _model.GetConfigById(id);
                        return cfg != null
                            ? new QuestEntryViewModel(_model.GetState(id), cfg, _model)
                            : null;
                    })
                    .Where(vm => vm != null)
                    .ToList()!;
            }
            else
            {
                FinalRewardEntry = null;
                QuestEntries = new List<QuestEntryViewModel>();
            }
        }
        
        public bool TryClaimFinalReward()
        {
            if (FinalRewardEntry == null)
            {
                return false;
            }
            var success = _model.TryClaimReward(FinalRewardEntry.Id);
            if (success)
            {
                Refresh();
            }                
            return success;
        }
    }
}
