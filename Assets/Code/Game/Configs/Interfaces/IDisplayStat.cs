using UnityEngine;

namespace Code.Game.Configs.Interfaces
{
    public interface IDisplayStat
    {
        Sprite Icon { get; }
        string Value { get; }
    }
}