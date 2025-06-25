using Code.Common.EditorUtils;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace Code.UI.Views
{
    public class StatIconView : MonoBehaviour
    {
        [SerializeField, CantBeNull] private Image _icon;
        [SerializeField, CantBeNull] private TextMeshProUGUI _value;

        public void Set(Sprite icon, string value)
        {
            _icon.sprite = icon;
            _value.text = value;
        }
    }
}