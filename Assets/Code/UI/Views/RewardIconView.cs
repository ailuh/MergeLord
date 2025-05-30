using Code.Common.EditorUtils;
using Code.Game.Model.Rewards;
using UnityEngine;
using UnityEngine.UI;

namespace Code.UI.Views
{
    public class RewardIconView : MonoBehaviour
    {
        [SerializeField, CantBeNull] private Image _icon = null!;
        [SerializeField, CantBeNull] private Button _button = null!;
        
        public void Init(RewardData data, System.Action onClick)
        {
            _icon.sprite = data.Icon;
            _button.onClick.RemoveAllListeners();
            _button.onClick.AddListener(() => onClick?.Invoke());
        }
    }
}