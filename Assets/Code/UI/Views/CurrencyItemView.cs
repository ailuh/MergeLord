using Code.Common.EditorUtils;
using Code.Game.Enums;
using TMPro;
using UnityEngine;

namespace Code.UI.Views
{
    public class CurrencyItemView : MonoBehaviour
    {
        [SerializeField, CantBeNull] private CurrencyType _currencyType;
        [SerializeField, CantBeNull] private TextMeshProUGUI _amountText = null!;

        public CurrencyType CurrencyType => _currencyType;

        public void UpdateAmount(int amount)
        {
            _amountText.text = amount.ToString();
        }
    }
}