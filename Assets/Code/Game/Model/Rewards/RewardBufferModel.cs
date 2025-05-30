using System;
using System.Collections.Generic;

namespace Code.Game.Model.Rewards
{
    public class RewardBufferModel
    {
        private const int MaxBufferSize = 8;
        private readonly Queue<RewardData> _buffer = new();

        public event Action OnChanged;

        public IReadOnlyCollection<RewardData> Rewards => _buffer;
        public bool IsFull => _buffer.Count >= MaxBufferSize;
        public bool IsEmpty => _buffer.Count == 0;

        public bool TryAddReward(RewardData reward)
        {
            if (IsFull)
            {
                return false;
            }

            _buffer.Enqueue(reward);
            OnChanged?.Invoke();
            return true;
        }

        public bool TryConsumeReward(out RewardData reward)
        {
            if (_buffer.Count == 0)
            {
                reward = null;
                return false;
            }

            reward = _buffer.Dequeue();
            OnChanged?.Invoke();
            return true;
        }

        public RewardData Peek()
        {
            return _buffer.Count > 0 ? _buffer.Peek() : null;
        }
    }
}

