using Code.Common.EditorUtils;
using Code.Game.Enums;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace Code.UI.Views
{
    public class BuildingCostView : MonoBehaviour
    {
        [SerializeField, CantBeNull] private Image _icon = null!;
        [SerializeField, CantBeNull] private TextMeshProUGUI _amountText = null!;

        [SerializeField] private Sprite _amethystIcon;
        [SerializeField] private Sprite _citrineIcon;
        [SerializeField] private Sprite _garnetIcon;

        public void Set(CurrencyType currency, int amount)
        {
            _amountText.text = amount.ToString();
            _icon.sprite = GetSprite(currency);
        }

        private Sprite GetSprite(CurrencyType type) => type switch
        {
            CurrencyType.Amethyst => _amethystIcon,
            CurrencyType.Citrine => _citrineIcon,
            CurrencyType.Garnet => _garnetIcon,
            _ => null
        };
    }
}