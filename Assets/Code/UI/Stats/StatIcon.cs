using Code.Game.Configs.Interfaces;
using UnityEngine;

namespace Code.UI.Stats
{
    public class StatIcon : IDisplayStat
    {
        public string Value { get; }
        public Sprite Icon { get; }

        public StatIcon(string value, Sprite icon)
        {
            Value = value;
            Icon = icon;
        }
    }
}