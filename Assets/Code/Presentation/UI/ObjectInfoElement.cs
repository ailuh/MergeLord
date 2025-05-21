using Code.Infrastructure.Configs.Monsters;
using Code.Presentation.Animations;
using Code.Utils;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace Code.Presentation.UI
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
            _energy.text = $"+{objectRef.GeneratedMana.ToString()}";
        }
    }
}