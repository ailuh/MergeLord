using Code.Common.EditorUtils;
using Code.Game.Configs.Monsters;
using Code.UI.Animations;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace Code.UI
{
    public class ObjectInfoElement : PopupElementAnimation
    {
        [SerializeField, CantBeNull] private Image _icon = null!;
        [SerializeField, CantBeNull] private TextMeshProUGUI _energy = null!;
        [SerializeField, CantBeNull] private TextMeshProUGUI _lvl = null!;
        
        public void SetData(ObjectRef objectRef)
        {
            _lvl.text = $"Lvl. {objectRef.BaseLevel.ToString()}";   
            _icon.sprite = objectRef.Sprite;
            _energy.text = $"+{objectRef.Energy.EnergyPerTick}";
        }
    }
}