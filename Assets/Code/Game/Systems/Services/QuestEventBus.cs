using System;
using System.Collections.Generic;
using Code.Game.Model.Quests;
using Code.Game.Systems.Interfaces;

namespace Code.Game.Systems.Services
{
    public class QuestEventBus : IQuestEventBus
    {
        private readonly List<Action<IQuestEvent>> _subscribers = new();

        public void Subscribe(Action<IQuestEvent> handler)
        {
            if (!_subscribers.Contains(handler))
            {
                _subscribers.Add(handler);
            }
        }

        public void Unsubscribe(Action<IQuestEvent> handler)
        {
            _subscribers.Remove(handler);
        }

        public void Raise(IQuestEvent questEvent)
        {
            foreach (var handler in _subscribers)
            {
                handler.Invoke(questEvent);
            }
        }
    }
}
