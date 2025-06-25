using System.Collections.Generic;
using UnityEngine;

namespace Code.Game.Configs.Interfaces
{
    public interface IUpgradeableObjectInfo
    {
        string Title { get; }
        string Description { get; }
        Sprite Icon { get; }
        Dictionary<string, string> GetDisplayStats();
    }
}