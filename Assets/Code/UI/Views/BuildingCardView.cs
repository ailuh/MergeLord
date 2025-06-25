using System;
using Code.Common.EditorUtils;
using Code.Game.Configs.Buildings;
using Code.UI.ViewModels;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace Code.UI.Views
{
    public class BuildingCardView : MonoBehaviour
    {
        [SerializeField, CantBeNull] private Image _icon = null!;
        [SerializeField, CantBeNull] private TextMeshProUGUI _nameText = null!;
        [SerializeField, CantBeNull] private Transform _costContainer = null!;
        [SerializeField, CantBeNull] private BuildingCostView _costPrefab = null!;
        [SerializeField, CantBeNull] private Button _button = null!;
        [SerializeField, CantBeNull] private Image _background = null!;
        [SerializeField] private Color _availableColor = Color.white;
        [SerializeField] private Color _unavailableColor = Color.gray;

        private BuildingConfig _config;
        private Action<BuildingConfig> _onClick;

        public void Init(BuildingEntry entry, Action<BuildingConfig> onClick)
        {
            _config = entry.Config;
            _onClick = onClick;

            _icon.sprite = _config.Sprite;
            _nameText.text = _config.DisplayName;
            _background.color = entry.CanBuy ? _availableColor : _unavailableColor;

            foreach (Transform child in _costContainer)
            {
                Destroy(child.gameObject);
            }

            foreach (var cost in _config.Costs)
            {
                var costView = Instantiate(_costPrefab, _costContainer);
                costView.Set(cost.Currency, cost.Amount);
            }

            _button.onClick.RemoveAllListeners();
            _button.onClick.AddListener(() => _onClick?.Invoke(_config));
        }
    }
}