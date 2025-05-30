using System;
using Code.Game.Model.Quests;

namespace Code.Game.Systems.Interfaces
{
    public interface IQuestEventBus
    {
        void Subscribe(Action<IQuestEvent> handler);
        void Unsubscribe(Action<IQuestEvent> handler);
        void Raise(IQuestEvent questEvent);
    }
}
